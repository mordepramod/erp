using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DESPLWEB
{
    public partial class MaterialCollection : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);

                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Material Collection Status";
                getCollectionDate();
                ddlListRoute_Driver.Items.Insert(0, "---Select---");

                lnk_Carryforword.Enabled = false;
                lnk_Collect.Enabled = false;
                lnkSave.Enabled = false;
                LoadDriver2();
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_EnqApprove_right_bit == true || u.USER_CS_right_bit == true)
                    {
                        lnk_Carryforword.Enabled = true;
                        lnk_Collect.Enabled = true;
                        lnkSave.Enabled = true;
                    }
                }
            }
        }

        public void getCollectionDate()
        {
            txt_CollectionDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_ActualCollectedOnDt.Text = txt_CollectionDate.Text;
        }

        private void LoadRoute()
        {
            ddlListRoute_Driver.DataTextField = "ROUTE_Name_var";
            ddlListRoute_Driver.DataValueField = "ROUTE_Id";
            var Route = dc.Route_View(0, "", "False", 0);
            ddlListRoute_Driver.DataSource = Route;
            ddlListRoute_Driver.DataBind();
            ddlListRoute_Driver.Items.Insert(0, "---Select---");
        }
        private void LoadDriver()
        {
            ddlListRoute_Driver.DataTextField = "USER_Name_var";
            ddlListRoute_Driver.DataValueField = "USER_Id";
            var driver = dc.Driver_View(false);
            ddlListRoute_Driver.DataSource = driver;
            ddlListRoute_Driver.DataBind();
            ddlListRoute_Driver.Items.Insert(0, "---Select---");
        }
        private void LoadDriver2()
        {
            ddlDriver.DataTextField = "USER_Name_var";
            ddlDriver.DataValueField = "USER_Id";
            var driver = dc.Driver_View(false);
            ddlDriver.DataSource = driver;
            ddlDriver.DataBind();
            ddlDriver.Items.Insert(0, "---Select---");
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (ddl_ListFor.SelectedItem.Text != "---Select---")//&& txt_CollectionDate.Text != "")
            {
                BindgrdMaterialCollection();
            }
        }

        protected void BindgrdMaterialCollection()
        {
            grdMaterial.DataSource = null;
            grdMaterial.DataBind();

            int siteRouteId = 0;
            int RouteId = 0;
            int DriverId = 0;
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DateTime CollectionDt = DateTime.Now;
            if (txt_CollectionDate.Text != "")
            {
                CollectionDt = DateTime.ParseExact(txt_CollectionDate.Text, "dd/MM/yyyy", null);
            }
            DataRow drow = null;
            dtTable.Columns.Add(new DataColumn("ENQ_Id", typeof(int)));
            //dtTable.Columns.Add(new DataColumn("ENQ_Date_dt", typeof(DateTime)));
            dtTable.Columns.Add(new DataColumn("ENQ_Date_dt", typeof(string)));
            //dtTable.Columns.Add(new DataColumn("ENQ_ClientExpectedDate_dt", typeof(DateTime)));
            dtTable.Columns.Add(new DataColumn("ENQ_ClientExpectedDate_dt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("MATERIAL_Name_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ENQ_CollectedOn_dt", typeof(string)));
            //dtTable.Columns.Add(new DataColumn("ENQ_CollectionDate_dt", typeof(DateTime)));
            //dtTable.Columns.Add(new DataColumn("ENQ_ModifiedCollectionDate_dt", typeof(DateTime)));
            dtTable.Columns.Add(new DataColumn("ENQ_CollectionDate_dt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ENQ_ModifiedCollectionDate_dt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("LOCATION_Name_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CL_Limit_mny", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CL_BalanceAmt_mny", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ENQ_Comment_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CONT_Name_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CONT_ContactNo_var", typeof(string)));
            //dtTable.Columns.Add(new DataColumn("ENQ_Carryforword_dt", typeof(DateTime)));
            dtTable.Columns.Add(new DataColumn("ENQ_Carryforword_dt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ME_Name", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ENQ_Quantity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("unUsedCoupon", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ENQ_Reference_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ENQ_Note_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ENQ_CarryFwdReason_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ENQ_MobileAppEnqNo_int", typeof(string)));
            //dtTable.Columns.Add(new DataColumn("ENQ_LetterNo_var", typeof(string)));

            if (ddlFilter.SelectedItem.Text == "Specific Driver" && ddlListRoute_Driver.SelectedItem.Text != "---Select---")
            {
                DriverId = Convert.ToInt32(ddlListRoute_Driver.SelectedValue);
            }
            else if (ddlFilter.SelectedItem.Text == "Specific Route" && ddlListRoute_Driver.SelectedItem.Text != "---Select---")
            {
                RouteId = Convert.ToInt32(ddlListRoute_Driver.SelectedValue);
            }
            if (ddl_ListFor.SelectedItem.Text == "Collected")
            {
                var Collected = dc.Enquiry_View_MaterialCollection(CollectionDt, null, null, null, RouteId, DriverId, 0, 0, ddl_ListFor.SelectedItem.Text, "");
                grdMaterial.DataSource = Collected;
                grdMaterial.DataBind();
            }
            else if (ddl_ListFor.SelectedItem.Text == "Not Collected" && chk_CollectionDt.Checked)
            {
                var data = dc.Enquiry_View_MaterialCollection(CollectionDt, null, null, null, RouteId, DriverId, 0, 0, "", "");
                foreach (var d in data)
                {
                    bool valid = false;
                    var dis = dc.Enquiry_View_MaterialCollection(CollectionDt, null, null, null, 0, 0, Convert.ToInt32(d.ENQ_Id), 0, "", "");
                    foreach (var s in dis)
                    {
                        if (s.ENQ_ClientExpectedDate_dt != null)
                        {
                            valid = true;
                        }
                        else if (s.ENQ_Carryforword_dt != null)
                        {
                            valid = true;
                        }
                        else if (s.ENQ_ModifiedCollectionDate_dt != null)
                        {
                            valid = true;
                        }
                        else if (s.ENQ_CollectionDate_dt != null)
                        {
                            if (Convert.ToDateTime(s.ENQ_CollectionDate_dt).Date == CollectionDt.Date)
                            {
                                valid = true;
                            }
                        }
                        if (valid == true)
                        {
                            var Collec = dc.Enquiry_View_MaterialCollection(CollectionDt, s.ENQ_ClientExpectedDate_dt, s.ENQ_Carryforword_dt, s.ENQ_ModifiedCollectionDate_dt, RouteId, DriverId, 0, 0, ddl_ListFor.SelectedItem.Text, "");
                            foreach (var c in Collec)
                            {
                                if (d.ENQ_Id.ToString() == c.ENQ_Id.ToString())
                                {
                                    drow = dtTable.NewRow();
                                    drow["ENQ_Id"] = c.ENQ_Id.ToString();
                                    //drow["ENQ_Date_dt"] = DateTime.Parse(c.ENQ_Date_dt.ToString(), System.Globalization.CultureInfo.InvariantCulture);// Convert.ToDateTime(c.ENQ_Date_dt).ToString("dd/MM/yyyy");
                                    drow["ENQ_Date_dt"] = Convert.ToDateTime(c.ENQ_Date_dt).ToString("dd/MM/yyyy");
                                    if (c.ENQ_ClientExpectedDate_dt != null)
                                    {
                                        //drow["ENQ_ClientExpectedDate_dt"] = DateTime.Parse(c.ENQ_ClientExpectedDate_dt.ToString(), System.Globalization.CultureInfo.InvariantCulture); //Convert.ToDateTime(c.ENQ_ClientExpectedDate_dt).ToString("dd/MM/yyyy");
                                        drow["ENQ_ClientExpectedDate_dt"] = Convert.ToDateTime(c.ENQ_ClientExpectedDate_dt).ToString("dd/MM/yyyy");
                                    }
                                    drow["CL_Name_var"] = Convert.ToString(c.CL_Name_var);
                                    drow["SITE_Name_var"] = Convert.ToString(c.SITE_Name_var);
                                    drow["MATERIAL_Name_var"] = Convert.ToString(c.MATERIAL_Name_var);
                                    if (c.ENQ_CollectionDate_dt != null)
                                    {
                                        //drow["ENQ_CollectionDate_dt"] = DateTime.Parse(c.ENQ_CollectionDate_dt.ToString(), System.Globalization.CultureInfo.InvariantCulture);// Convert.ToDateTime(c.ENQ_CollectionDate_dt).ToString("dd/MM/yyyy");
                                        drow["ENQ_CollectionDate_dt"] = Convert.ToDateTime(c.ENQ_CollectionDate_dt).ToString("dd/MM/yyyy");
                                    }
                                    if (c.ENQ_ModifiedCollectionDate_dt != null)
                                    {
                                        //drow["ENQ_ModifiedCollectionDate_dt"] = DateTime.Parse(c.ENQ_ModifiedCollectionDate_dt.ToString(), System.Globalization.CultureInfo.InvariantCulture); //Convert.ToDateTime(c.ENQ_ModifiedCollectionDate_dt).ToString("dd/MM/yyyy");
                                        drow["ENQ_ClientExpectedDate_dt"] = Convert.ToDateTime(c.ENQ_ClientExpectedDate_dt).ToString("dd/MM/yyyy");
                                    }
                                    drow["LOCATION_Name_var"] = Convert.ToString(c.LOCATION_Name_var);

                                    if (c.ENQ_Comment_var != null && c.ENQ_Comment_var != "")
                                    {
                                        drow["ENQ_Comment_var"] = Convert.ToString(c.ENQ_Comment_var);
                                    }

                                    drow["CONT_Name_var"] = Convert.ToString(c.CONT_Name_var);
                                    drow["CONT_ContactNo_var"] = Convert.ToString(c.CONT_ContactNo_var);
                                    if (c.ENQ_Carryforword_dt != null)
                                    {
                                        //drow["ENQ_Carryforword_dt"] = DateTime.Parse(c.ENQ_Carryforword_dt.ToString(), System.Globalization.CultureInfo.InvariantCulture); //Convert.ToDateTime(c.ENQ_Carryforword_dt).ToString("dd/MM/yyyy");
                                        drow["ENQ_Carryforword_dt"] = Convert.ToDateTime(c.ENQ_Carryforword_dt).ToString("dd/MM/yyyy");
                                    }

                                    if (Convert.ToString(c.ENQ_ROUTE_Id) != "" && Convert.ToString(c.ENQ_ROUTE_Id) != null)
                                        siteRouteId = Convert.ToInt32(c.ENQ_ROUTE_Id);
                                    var site = dc.User_View_ME(siteRouteId);
                                    foreach (var m in site)
                                    {
                                        drow["ME_Name"] = Convert.ToString(m.USER_Name_var);
                                    }

                                    drow["ENQ_Quantity"] = Convert.ToString(c.ENQ_Quantity);
                                    drow["unUsedCoupon"] = Convert.ToString(c.unUsedCoupon);
                                    drow["ENQ_Reference_var"] = Convert.ToString(c.ENQ_Reference_var);
                                    drow["ENQ_Note_var"] = Convert.ToString(c.ENQ_Note_var);
                                    drow["ENQ_CarryFwdReason_var"] = Convert.ToString(c.ENQ_CarryFwdReason_var);
                                    drow["ENQ_MobileAppEnqNo_int"] = Convert.ToString(c.ENQ_MobileAppEnqNo_int);
                                    //drow["ENQ_LetterNo_var"] = Convert.ToString(c.ENQ_LetterNo_var);

                                    dtTable.Rows.Add(drow);
                                    dtTable.AcceptChanges();
                                    grdMaterial.DataSource = dtTable;
                                    grdMaterial.DataBind();
                                    rowIndex++;
                                }
                            }
                        }
                    }
                }
            }
            else if (ddl_ListFor.SelectedItem.Text == "Not Collected" && chk_CollectionDt.Checked == false)
            {
                var NotCollect = dc.Enquiry_View_MaterialCollection(null, null, null, null, RouteId, DriverId, 0, 0, ddl_ListFor.SelectedItem.Text, "All");
                foreach (var nc in NotCollect)
                {
                    drow = dtTable.NewRow();
                    drow["ENQ_Id"] = nc.ENQ_Id.ToString();
                    //drow["ENQ_Date_dt"] = DateTime.Parse(nc.ENQ_Date_dt.ToString(), System.Globalization.CultureInfo.InvariantCulture);//DateTime.Parse(nc.ENQ_Date_dt.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    drow["ENQ_Date_dt"] = Convert.ToDateTime(nc.ENQ_Date_dt).ToString("dd/MM/yyyy");

                    if (nc.ENQ_ClientExpectedDate_dt != null)
                    {
                        //drow["ENQ_ClientExpectedDate_dt"] = DateTime.Parse(nc.ENQ_ClientExpectedDate_dt.ToString(), System.Globalization.CultureInfo.InvariantCulture);// Convert.ToDateTime(nc.ENQ_ClientExpectedDate_dt).ToString("dd/MM/yyyy");
                        drow["ENQ_ClientExpectedDate_dt"] = Convert.ToDateTime(nc.ENQ_ClientExpectedDate_dt).ToString("dd/MM/yyyy");
                    }
                    drow["CL_Name_var"] = Convert.ToString(nc.CL_Name_var);
                    drow["SITE_Name_var"] = Convert.ToString(nc.SITE_Name_var);
                    drow["MATERIAL_Name_var"] = Convert.ToString(nc.MATERIAL_Name_var);
                    if (nc.ENQ_CollectionDate_dt != null)
                    {
                        //drow["ENQ_CollectionDate_dt"] = DateTime.Parse(nc.ENQ_CollectionDate_dt.ToString(), System.Globalization.CultureInfo.InvariantCulture);// Convert.ToDateTime(nc.ENQ_CollectionDate_dt).ToString("dd/MM/yyyy");
                        drow["ENQ_CollectionDate_dt"] = Convert.ToDateTime(nc.ENQ_CollectionDate_dt).ToString("dd/MM/yyyy");
                    }
                    if (nc.ENQ_ModifiedCollectionDate_dt != null)
                    {
                        //drow["ENQ_ModifiedCollectionDate_dt"] = DateTime.Parse(nc.ENQ_ModifiedCollectionDate_dt.ToString(), System.Globalization.CultureInfo.InvariantCulture);// Convert.ToDateTime(nc.ENQ_ModifiedCollectionDate_dt).ToString("dd/MM/yyyy");
                        drow["ENQ_ModifiedCollectionDate_dt"] = Convert.ToDateTime(nc.ENQ_ModifiedCollectionDate_dt).ToString("dd/MM/yyyy");
                    }
                    drow["LOCATION_Name_var"] = Convert.ToString(nc.LOCATION_Name_var);

                    if (nc.ENQ_Comment_var != null && nc.ENQ_Comment_var != "")
                    {
                        drow["ENQ_Comment_var"] = Convert.ToString(nc.ENQ_Comment_var);
                    }

                    drow["CONT_Name_var"] = Convert.ToString(nc.CONT_Name_var);
                    drow["CONT_ContactNo_var"] = Convert.ToString(nc.CONT_ContactNo_var);
                    if (nc.ENQ_Carryforword_dt != null)
                    {
                        //drow["ENQ_Carryforword_dt"] = DateTime.Parse(nc.ENQ_Carryforword_dt.ToString(), System.Globalization.CultureInfo.InvariantCulture);// Convert.ToDateTime(nc.ENQ_Carryforword_dt).ToString("dd/MM/yyyy");
                        drow["ENQ_Carryforword_dt"] = Convert.ToDateTime(nc.ENQ_Carryforword_dt).ToString("dd/MM/yyyy");
                    }

                    if (Convert.ToString(nc.ENQ_ROUTE_Id) != "" && Convert.ToString(nc.ENQ_ROUTE_Id) != null)
                        siteRouteId = Convert.ToInt32(nc.ENQ_ROUTE_Id);
                    var site = dc.User_View_ME(siteRouteId);
                    foreach (var m in site)
                    {
                        drow["ME_Name"] = Convert.ToString(m.USER_Name_var);
                    }

                    drow["ENQ_Quantity"] = Convert.ToString(nc.ENQ_Quantity);
                    drow["unUsedCoupon"] = Convert.ToString(nc.unUsedCoupon);
                    drow["ENQ_Reference_var"] = Convert.ToString(nc.ENQ_Reference_var);
                    drow["ENQ_Note_var"] = Convert.ToString(nc.ENQ_Note_var);
                    drow["ENQ_CarryFwdReason_var"] = Convert.ToString(nc.ENQ_CarryFwdReason_var);
                    drow["ENQ_MobileAppEnqNo_int"] = Convert.ToString(nc.ENQ_MobileAppEnqNo_int);
                    //drow["ENQ_LetterNo_var"] = Convert.ToString(nc.ENQ_LetterNo_var);
                    dtTable.Rows.Add(drow);
                    dtTable.AcceptChanges();
                    rowIndex++;
                }
                grdMaterial.DataSource = dtTable;
                grdMaterial.DataBind();
            }
            DisplayTotal();
        }
        private void DisplayTotal()
        {
            if (grdMaterial.Rows.Count > 0)
            {
                lblTotal.Text = "Total Records   :   " + grdMaterial.Rows.Count;
            }
            else
            {
                lblTotal.Text = "Total Records   :   0";
            }
            int j = 0;
            lblCarryfwd.Text = "Carry Forward   :   " + j;
            for (int i = 0; i < grdMaterial.Rows.Count; i++)
            {
                if (grdMaterial.Rows[i].Cells[17].Text != "" && grdMaterial.Rows[i].Cells[17].Text != "&nbsp;")
                {
                    j++;
                    lblCarryfwd.Text = "Carry Forward   :   " + j;
                }
            }
        }


        protected void grdMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[17].Text != "" && e.Row.Cells[17].Text != "&nbsp;")
                {
                    e.Row.BackColor = System.Drawing.Color.LightCyan;
                }

                DropDownList ddl_Route = (DropDownList)e.Row.FindControl("ddl_Route");
                TextBox txt_EditedCollectionDt = (TextBox)e.Row.FindControl("txt_EditedCollectionDt");
                ddl_Route.DataTextField = "ROUTE_Name_var";
                ddl_Route.DataValueField = "ROUTE_Id";
                var Route = dc.Route_View(0, "", "False", 0);
                ddl_Route.DataSource = Route;
                ddl_Route.DataBind();
                ddl_Route.Items.Insert(0, "---Select---");

                var dis = dc.Enquiry_View_MaterialCollection(null, null, null, null, 0, 0, Convert.ToInt32(e.Row.Cells[1].Text), 0, "", "");
                foreach (var s in dis)
                {
                    if (s.ENQ_ROUTE_Id > 0)
                    {
                        ddl_Route.SelectedValue = s.ENQ_ROUTE_Id.ToString();
                    }
                    if (s.ENQ_ModifiedCollectionDate_dt != null)
                    {
                        txt_EditedCollectionDt.Text = Convert.ToDateTime(s.ENQ_ModifiedCollectionDate_dt).ToString("dd/MM/yyyy");
                    }
                }
            }
        }
        protected void lnk_Carryforword_Click(object sender, EventArgs e)
        {
            int EnqId = 0;
            bool selectedFlag = false;
            bool valid = true;
            DateTime CollectionDt = DateTime.ParseExact(txt_CollectionDate.Text, "dd/MM/yyyy", null);
            CollectionDt = CollectionDt.AddDays(1);
            for (int i = 0; i < grdMaterial.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdMaterial.Rows[i].Cells[0].FindControl("cbxSelect");
                TextBox txt_ENQ_CarryFwdReason_var = (TextBox)grdMaterial.Rows[i].FindControl("txt_ENQ_CarryFwdReason_var");
                txt_ENQ_CarryFwdReason_var.Text = txt_ENQ_CarryFwdReason_var.Text.Trim();
                if (cbxSelect.Checked)
                {

                    selectedFlag = true;
                    if (txt_ENQ_CarryFwdReason_var.Text == "") //should not validate for amol joshi - do later
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Input Reason for Carry Forward');", true);
                        txt_ENQ_CarryFwdReason_var.Focus();
                        valid = false;
                        break;
                    }

                }
            }
            if (selectedFlag == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select at least one enquiry');", true);
            }
            else if (valid == true)
            {
                for (int i = 0; i < grdMaterial.Rows.Count; i++)
                {
                    CheckBox cbxSelect = (CheckBox)grdMaterial.Rows[i].Cells[0].FindControl("cbxSelect");
                    TextBox ENQ_CarryFwdReason_var = (TextBox)grdMaterial.Rows[i].FindControl("ENQ_CarryFwdReason_var");
                    if (cbxSelect.Checked)
                    {
                        EnqId = Convert.ToInt32(cbxSelect.CssClass);
                        dc.CollectionDt_Update(EnqId, null, CollectionDt, null, 0, ENQ_CarryFwdReason_var.Text);
                    }
                }

                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Updated Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                BindgrdMaterialCollection();
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateEditeDt() == true)
            {
                bool valid = false;
                int EnqId = 0;
                for (int i = 0; i < grdMaterial.Rows.Count; i++)
                {
                    CheckBox cbxSelect = (CheckBox)grdMaterial.Rows[i].Cells[0].FindControl("cbxSelect");
                    TextBox txt_EditedCollectionDt = (TextBox)grdMaterial.Rows[i].Cells[8].FindControl("txt_EditedCollectionDt");
                    DropDownList ddl_Route = (DropDownList)grdMaterial.Rows[i].Cells[10].FindControl("ddl_Route");

                    if (cbxSelect.Checked)
                    {
                        valid = true;
                        EnqId = Convert.ToInt32(cbxSelect.CssClass);
                        if (txt_EditedCollectionDt.Text != "")
                        {
                            DateTime ModifiedCollectionDt = DateTime.ParseExact(txt_EditedCollectionDt.Text, "dd/MM/yyyy", null);
                            dc.CollectionDt_Update(EnqId, null, null, ModifiedCollectionDt, 0, "");
                        }
                        if (Convert.ToInt32(ddl_Route.SelectedValue) != 0)
                        {
                            dc.CollectionDt_Update(EnqId, null, null, null, Convert.ToInt32(ddl_Route.SelectedValue), "");
                        }
                    }
                }
                if (valid == true)
                {
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Text = "Record Updated Successfully";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    BindgrdMaterialCollection();
                }
            }
        }
        protected Boolean ValidateEditeDt()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            Boolean valid = true;
            for (int i = 0; i < grdMaterial.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdMaterial.Rows[i].Cells[0].FindControl("cbxSelect");
                if (cbxSelect.Checked)
                {
                    TextBox EditedCollectionDt = (TextBox)grdMaterial.Rows[i].Cells[8].FindControl("txt_EditedCollectionDt");
                    if (EditedCollectionDt.Text != "")
                    {
                        if (DateTime.ParseExact(EditedCollectionDt.Text, "dd/MM/yyyy", null) < DateTime.ParseExact(grdMaterial.Rows[i].Cells[8].Text, "dd/MM/yyyy", null))
                        //if (Convert.ToDateTime(EditedCollectionDt.Text) < Convert.ToDateTime(grdMaterial.Rows[i].Cells[8].Text))
                        {
                            lblMsg.Text = "Edited Date should be greater than or equal to the Collection Date for Enq No .  " + grdMaterial.Rows[i].Cells[1].Text;
                            EditedCollectionDt.Focus();
                            valid = false;
                            break;
                        }
                    }
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }

        protected void lnk_Collect_Click(object sender, EventArgs e)
        {
            int EnqId = 0;
            bool valid = false;
            DateTime ActualCollectionDt = DateTime.Now;
            if (txt_ActualCollectedOnDt.Text != "")
            {
                ActualCollectionDt = DateTime.ParseExact(txt_ActualCollectedOnDt.Text, "dd/MM/yyyy", null);
            }
            for (int i = 0; i < grdMaterial.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdMaterial.Rows[i].Cells[0].FindControl("cbxSelect");
                if (cbxSelect.Checked)
                {
                    valid = true;
                    EnqId = Convert.ToInt32(cbxSelect.CssClass);
                    dc.CollectionDt_Update(EnqId, ActualCollectionDt, null, null, 0, "");
                }
            }
            if (valid == true)
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Updated Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                BindgrdMaterialCollection();
            }
        }

        protected void lnk_Print_Click(object sender, EventArgs e)
        {
            if (grdMaterial.Rows.Count > 0)
            {
                int DriverId = 0;
                int RouteId = 0;
                DateTime CollectionDt = DateTime.ParseExact(txt_CollectionDate.Text, "dd/MM/yyyy", null);
                if (ddlFilter.SelectedItem.Text == "Specific Driver" && ddlListRoute_Driver.SelectedItem.Text != "---Select---")
                {
                    DriverId = Convert.ToInt32(ddlListRoute_Driver.SelectedValue);
                }
                else if (ddlFilter.SelectedItem.Text == "Specific Route" && ddlListRoute_Driver.SelectedItem.Text != "---Select---")
                {
                    RouteId = Convert.ToInt32(ddlListRoute_Driver.SelectedValue);
                }
                RptMaterialCollection(ddl_ListFor.SelectedItem.Text, RouteId, DriverId, CollectionDt);
            }
        }



        protected void txt_CollectionDate_TextChanged(object sender, EventArgs e)
        {
            txt_ActualCollectedOnDt.Text = txt_CollectionDate.Text;
        }

        protected void ddl_ListFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdMaterial.DataSource = null;
            grdMaterial.DataBind();
            DisplayTotal();
            if (ddl_ListFor.SelectedItem.Text == "Collected")
            {
                lnk_Collect.Enabled = false;
                lnk_Carryforword.Enabled = false;
                lnkSave.Enabled = false;
                txt_ActualCollectedOnDt.Enabled = true;
                grdMaterial.Columns[0].Visible = false;
                grdMaterial.Columns[7].Visible = true;
                chk_CollectionDt.Enabled = false;
                chk_CollectionDt.Checked = false;
                txt_CollectionDate.Enabled = true;
            }
            else if (ddl_ListFor.SelectedItem.Text == "Not Collected")
            {
                lnk_Collect.Enabled = true;
                lnk_Carryforword.Enabled = true;
                lnkSave.Enabled = true;
                txt_ActualCollectedOnDt.Enabled = false;
                grdMaterial.Columns[0].Visible = true;
                grdMaterial.Columns[7].Visible = false;
                chk_CollectionDt.Enabled = true;
                chk_CollectionDt.Checked = true;
            }
        }
        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdMaterial.DataSource = null;
            grdMaterial.DataBind();
            ddlListRoute_Driver.Enabled = true;
            DisplayTotal();
            if (ddlFilter.SelectedItem.Text == "Specific Route")
            {
                LoadRoute();
            }
            else if (ddlFilter.SelectedItem.Text == "Specific Driver")
            {
                LoadDriver();
            }
            else
            {
                ddlListRoute_Driver.Items.Clear();
                ddlListRoute_Driver.Items.Insert(0, "---Select---");
                ddlListRoute_Driver.Enabled = false;
            }
        }
        protected void ddlListRoute_Driver_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdMaterial.DataSource = null;
            grdMaterial.DataBind();
            DisplayTotal();
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }
        public void RptMaterialCollection(string Collectype, int RouteId, int DriverId, DateTime CollectionDt)
        {
            try
            {
                Paragraph paragraph = new Paragraph();
                Document pdfDoc = new Document(PageSize.A4, 55f, 45f, 100f, 0f);
                var fileName = "MaterialCollection" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".pdf";
                //string foldername = "Veena";
                string foldername = "C:/temp/Veena";

                //if (!Directory.Exists(@"D:\" + foldername))
                //    Directory.CreateDirectory(@"D:/" + foldername);

                //string Subfoldername = foldername + "/Material Collection";
                //if (!Directory.Exists(@"D:\" + Subfoldername))
                //    Directory.CreateDirectory(@"D:/" + Subfoldername);

                //string Subfoldername1 = Subfoldername + "/Material Collection";

                //if (!Directory.Exists(@"D:\" + Subfoldername1))
                //    Directory.CreateDirectory(@"D:/" + Subfoldername1);

                //PdfWriter.GetInstance(pdfDoc, new FileStream(@"D:/" + Subfoldername1 + "/" + fileName, FileMode.Create));

                if (!Directory.Exists(@foldername))
                    Directory.CreateDirectory(@foldername);
                string Subfoldername = foldername + "/MaterialCollection";
                if (!Directory.Exists(@Subfoldername))
                    Directory.CreateDirectory(@Subfoldername);
                string Subfoldername1 = Subfoldername;
                PdfWriter.GetInstance(pdfDoc, new FileStream(@Subfoldername1 + "/" + fileName, FileMode.Create));

                pdfDoc.Open();
                PdfPTable table1 = new PdfPTable(7);
                paragraph = new Paragraph();
                Font fontTitle = new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD);
                Font fontH1 = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.UNDEFINED);
                Font fontH2 = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD);
                #region data
                paragraph.Font = fontTitle;
                paragraph.Add("Material Collection Report");
                paragraph.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(paragraph);

                PdfPCell cell1;
                table1 = new PdfPTable(2);
                table1.SpacingBefore = 5;
                table1.WidthPercentage = 100;
                table1.HorizontalAlignment = Element.ALIGN_CENTER;
                table1.SetTotalWidth(new float[] { 50f, 50f });

                if (RouteId > 0)
                {
                    cell1 = new PdfPCell(new Phrase("Route Name    :" + " " + ddlListRoute_Driver.SelectedItem.Text, fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell1.Border = PdfPCell.NO_BORDER;
                    table1.AddCell(cell1);
                }
                else if (DriverId > 0)
                {
                    cell1 = new PdfPCell(new Phrase("Driver Name    :" + " " + ddlListRoute_Driver.SelectedItem.Text, fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell1.Border = PdfPCell.NO_BORDER;
                    table1.AddCell(cell1);
                }
                else
                {
                    cell1 = new PdfPCell(new Phrase("", fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell1.Border = PdfPCell.NO_BORDER;
                    table1.AddCell(cell1);
                }
                cell1 = new PdfPCell(new Phrase("Date     :" + " " + Convert.ToDateTime(CollectionDt).ToString("dd-MMM-yy"), fontH1));
                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell1.Border = PdfPCell.NO_BORDER;
                table1.AddCell(cell1);
                pdfDoc.Add(table1);

                for (int i = 0; i < grdMaterial.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        if (Collectype == "Not Collected")
                        {
                            table1 = new PdfPTable(13);
                            table1.SpacingBefore = 10;
                            table1.WidthPercentage = 100;
                            table1.HorizontalAlignment = Element.ALIGN_LEFT;
                            table1.SetTotalWidth(new float[] { 9f, 15f, 12f, 12f, 10f, 12f, 10f, 14f, 11f, 11f, 10f, 8f, 9f });
                            string[] header = { "Enq No.", "Client Name", "Site Name", "Location", "Order For", "Collection Date", "Contact Person", "Contact No", "Remark", "Unused Coupons", "ME", "Qty.", "App EnqNo"};
                            for (int h = 0; h < header.Count(); h++)
                            {
                                cell1 = new PdfPCell(new Phrase(header[h], fontH2));
                                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                                table1.AddCell(cell1);
                            }
                        }
                        else if (Collectype == "Collected")
                        {
                            table1 = new PdfPTable(14);
                            table1.SpacingBefore = 10;
                            table1.WidthPercentage = 100;
                            table1.HorizontalAlignment = Element.ALIGN_LEFT;
                            table1.SetTotalWidth(new float[] { 9f, 15f, 12f, 12f, 10f, 12f, 12f, 10f, 14f, 11f, 11f, 10f, 8f, 9f });
                            string[] header = { "Enq No.", "Client Name", "Site Name", "Location", "Order For", "Collected Date", "Collection Date", "Contact Person", "Contact No", "Remark", "Unused Coupons", "ME", "Qty.", "App EnqNo"};
                            for (int h = 0; h < header.Count(); h++)
                            {
                                cell1 = new PdfPCell(new Phrase(header[h], fontH2));
                                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                                table1.AddCell(cell1);
                            }
                        }
                    }
                    cell1 = new PdfPCell(new Phrase(grdMaterial.Rows[i].Cells[1].Text, fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    if (grdMaterial.Rows[i].Cells[21].Text.Length >= 5)
                    {
                        cell1.Rowspan = 2;
                    }
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(grdMaterial.Rows[i].Cells[4].Text, fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(grdMaterial.Rows[i].Cells[5].Text, fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(grdMaterial.Rows[i].Cells[10].Text, fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(Convert.ToString(grdMaterial.Rows[i].Cells[6].Text).Replace("Testing", ""), fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(cell1);

                    if (Collectype == "Collected")
                    {
                        cell1 = new PdfPCell(new Phrase(grdMaterial.Rows[i].Cells[7].Text, fontH1));
                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                        table1.AddCell(cell1);
                    }

                    cell1 = new PdfPCell(new Phrase(Convert.ToDateTime(DateTime.ParseExact(grdMaterial.Rows[i].Cells[8].Text, "dd/MM/yyyy", null)).ToString("dd/MMM/yy"), fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(grdMaterial.Rows[i].Cells[15].Text, fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(grdMaterial.Rows[i].Cells[16].Text, fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(cell1);

                    if (grdMaterial.Rows[i].Cells[22].Text != "" && grdMaterial.Rows[i].Cells[22].Text != "&nbsp;")
                    {
                        cell1 = new PdfPCell(new Phrase(grdMaterial.Rows[i].Cells[22].Text, fontH1));
                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                        table1.AddCell(cell1);
                    }
                    else
                    {
                        cell1 = new PdfPCell(new Phrase("", fontH1));
                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                        table1.AddCell(cell1);
                    }

                    cell1 = new PdfPCell(new Phrase(grdMaterial.Rows[i].Cells[19].Text, fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(cell1);

                    if (grdMaterial.Rows[i].Cells[18].Text != "" && grdMaterial.Rows[i].Cells[18].Text != "&nbsp;")
                    {
                        cell1 = new PdfPCell(new Phrase(grdMaterial.Rows[i].Cells[18].Text, fontH1));
                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                        table1.AddCell(cell1);
                    }
                    else
                    {
                        cell1 = new PdfPCell(new Phrase("", fontH1));
                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                        table1.AddCell(cell1);
                    }

                    cell1 = new PdfPCell(new Phrase(grdMaterial.Rows[i].Cells[20].Text, fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(grdMaterial.Rows[i].Cells[24].Text, fontH1));
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    table1.AddCell(cell1);

                    //cell1 = new PdfPCell(new Phrase(grdMaterial.Rows[i].Cells[25].Text.Replace("&nbsp;",""), fontH1));
                    //cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    //table1.AddCell(cell1);

                    string referance = "";
                    if (grdMaterial.Rows[i].Cells[21].Text.Length >= 5)
                    {
                        if (grdMaterial.Rows[i].Cells[21].Text != "" && grdMaterial.Rows[i].Cells[21].Text != "&nbsp;")
                        {
                            referance = grdMaterial.Rows[i].Cells[21].Text;
                        }
                        PdfPCell cellRow = new PdfPCell(new Phrase("Reference : " + referance, fontH1));
                        if (Collectype == "Collected")
                        {
                            cellRow.Colspan = 14;
                        }
                        else
                        {
                            cellRow.Colspan = 13;
                        }

                        cellRow.HorizontalAlignment = Element.ALIGN_LEFT;
                        table1.AddCell(cellRow);
                    }
                }
                pdfDoc.Add(table1);


                table1 = new PdfPTable(2);
                table1.SpacingBefore = 10;
                table1.WidthPercentage = 30;
                table1.HorizontalAlignment = Element.ALIGN_LEFT;
                table1.SetTotalWidth(new float[] { 10f, 8f });

                string Desc = "";
                for (int j = 0; j < 3; j++)
                {
                    if (j == 0)
                    {
                        Desc = "Total Site";
                    }
                    if (j == 1)
                    {
                        Desc = "Collection Completed";
                    }
                    if (j == 2)
                    {
                        Desc = "Collection Pending";
                    }
                    cell1 = new PdfPCell(new Phrase(Desc, fontH2));
                    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                    table1.AddCell(cell1);

                    if (j == 0)
                    {
                        cell1 = new PdfPCell(new Phrase(grdMaterial.Rows.Count.ToString(), fontH1));
                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                        table1.AddCell(cell1);
                    }
                    else
                    {
                        cell1 = new PdfPCell(new Phrase("", fontH1));
                        table1.AddCell(cell1);
                    }


                }
                pdfDoc.Add(table1);

                #endregion

                var blackListTextFont = FontFactory.GetFont("Verdana", 4);
                paragraph = new Paragraph();
                paragraph.Alignment = Element.ALIGN_RIGHT;
                paragraph.Font = fontH1;
                paragraph.Add("Authorized Signature");
                pdfDoc.Add(paragraph);
                pdfDoc.Close();
                string pdfPath = @Subfoldername1 + "/" + fileName;
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                System.Web.HttpContext.Current.Response.WriteFile(pdfPath);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        protected void chk_CollectionDt_CheckedChanged(object sender, EventArgs e)
        {
            if (!chk_CollectionDt.Checked)
            {
                txt_CollectionDate.Enabled = false;
                lblCollecDt.Enabled = false;
            }
            else
            {
                getCollectionDate();
                txt_CollectionDate.Enabled = true;
                lblCollecDt.Enabled = true;
            }
        }

        protected void lnkUpdatePickUp_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            bool valid = false;
            if (grdMaterial.Rows.Count == 0)
            {
                lblMsg.Text = "No enquiry available to update.";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
            else if (ddlDriver.Items.Count == 0 || ddlDriver.SelectedIndex <= 0)
            {
                lblMsg.Text = "Select driver to update.";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
                ddlDriver.Focus();
            }
            else
            {
                //dc.PickUpAllocation_Update(0, 0, 0, 0, 0, null, "", "", "", "", Convert.ToInt32(ddlDriver.SelectedValue), true);
                for (int i = 0; i < grdMaterial.Rows.Count; i++)
                {
                    CheckBox cbxSelect = (CheckBox)grdMaterial.Rows[i].Cells[0].FindControl("cbxSelect");
                    if (cbxSelect.Checked)
                    {
                        valid = true;
                        var enqDetail = dc.Enquiry_View_ForPickup(Convert.ToInt32(grdMaterial.Rows[i].Cells[1].Text));
                        foreach (var enq in enqDetail)
                        {
                            dc.PickUpAllocation_Update(Convert.ToInt32(grdMaterial.Rows[i].Cells[1].Text), enq.CL_Id, enq.SITE_Id, enq.LOCATION_Id, enq.ENQ_ROUTE_Id, enq.ENQ_CollectionDate_dt, enq.CONT_Name_var, enq.unUsedCoupon.ToString(), enq.ME_Name, enq.CONT_ContactNo_var, Convert.ToInt32(ddlDriver.SelectedValue), enq.MATERIAL_Name_var, Convert.ToInt32(enq.ENQ_Quantity), enq.ENQ_ContactNoForCollection_var, enq.ENQ_ContactPersonForCollection_var, false);
                        }
                    }
                }
                if (valid == true)
                {
                    lblMsg.Text = "Record Updated Successfully";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

     
        protected void lnkClearPreviousAllocation_Click(object sender, EventArgs e)
        {
            if (chkClearPreviousAllocation.Checked)
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                if (ddlDriver.Items.Count == 0 || ddlDriver.SelectedIndex <= 0)
                {
                    lblMsg.Text = "Select driver to clear allocation.";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    ddlDriver.Focus();
                }
                else
                {
                    dc.PickUpAllocation_Update(0, 0, 0, 0, 0, null, "", "", "", "", Convert.ToInt32(ddlDriver.SelectedValue),"",0,"","", true);
                }
            }
        }


    }
}