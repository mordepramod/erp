using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class NDTAppEnquiryAllocation : System.Web.UI.Page
    {

        LabDataDataContext dc = new LabDataDataContext();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Enquiry list";
                //LoadEnquiryList();
                optPending.Checked = true; 
                LoadUserList();
            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        private void LoadUserList()
        {
            ddlUser.DataTextField = "USER_Name_var";
            ddlUser.DataValueField = "USER_Id";
            var user = dc.User_View(0, 0, "", "", "");
            //var user = dc.Driver_View(true);
            ddlUser.DataSource = user;
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, "---Select---");
        }
        private void LoadEnquiryList()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("ENQ_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_OpenEnquiryStatus_var", typeof(string)));
            dt.Columns.Add(new DataColumn("USER_Name_var", typeof(string)));

            var enq1 = dc.Enquiry_View_ForNDTApp(11, optAssigned.Checked);
            foreach (var c in enq1)
            {
                bool valid = false;
                if (c.ENQ_OpenEnquiryStatus_var == "To be Collected")
                {
                    if (c.ENQ_CollectedOn_dt == null)
                    {
                        valid = true;
                    }
                }
                if (valid == false)
                {
                    dr = dt.NewRow();
                    dr["ENQ_Id"] = c.ENQ_Id.ToString();
                    dr["CL_Id"] = c.ENQ_CL_Id.ToString();
                    dr["SITE_Id"] = c.ENQ_SITE_Id.ToString();
                    dr["CONT_Id"] = c.ENQ_CONT_Id.ToString();
                    dr["ENQ_Date_dt"] = Convert.ToDateTime(c.ENQ_Date_dt).ToString("dd/MM/yyyy");
                    dr["CL_Name_var"] = Convert.ToString(c.CL_Name_var);
                    dr["SITE_Name_var"] = Convert.ToString(c.SITE_Name_var);
                    dr["ENQ_OpenEnquiryStatus_var"] = Convert.ToString(c.ENQ_OpenEnquiryStatus_var);
                    dr["USER_Name_var"] = Convert.ToString(c.USER_Name_var);
                    dt.Rows.Add(dr);
                }
            }
            grdEnquiry.DataSource = dt;
            grdEnquiry.DataBind();
            
            //if (grdEnquiry.Rows.Count <= 0)
            //{
            //    FirstGridViewRowOfEnquiry();
            //}
            //else
            //{
                lblTotalRecords.Text = "Total No of Records : " + grdEnquiry.Rows.Count;
            //}
            grdEnquiry.Visible = true;
            lblTotalRecords.Visible = true;            
        }

        private void FirstGridViewRowOfEnquiry()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("ENQ_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_OpenEnquiryStatus_var", typeof(string)));
            dt.Columns.Add(new DataColumn("USER_Name_var", typeof(string)));
            dr = dt.NewRow();
            dr["ENQ_Id"] = string.Empty;
            dr["CL_Id"] = string.Empty;
            dr["SITE_Id"] = string.Empty;
            dr["CONT_Id"] = string.Empty;
            dr["ENQ_Date_dt"] = string.Empty;
            dr["CL_Name_var"] = string.Empty;
            dr["SITE_Name_var"] = string.Empty;
            dr["ENQ_OpenEnquiryStatus_var"] = string.Empty;
            dr["USER_Name_var"] = string.Empty;
            dt.Rows.Add(dr);
            grdEnquiry.DataSource = dt;
            grdEnquiry.DataBind();

        }
                            
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadEnquiryList();            
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)grdEnquiry.HeaderRow.FindControl("chkSelectAll");
            foreach (GridViewRow row in grdEnquiry.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkSelect");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (optPending.Checked == true)
            {
                bool selectedFlag = false;
                if (grdEnquiry.Rows.Count > 0)
                {
                    for (int i = 0; i < grdEnquiry.Rows.Count; i++)
                    {
                        CheckBox chkSelect = (CheckBox)grdEnquiry.Rows[i].FindControl("chkSelect");
                        if (chkSelect.Checked == true)
                        {
                            selectedFlag = true;
                            break;
                        }
                    }
                }
                if (selectedFlag == false)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select at least one enquiry');", true);
                }
                else if (ddlUser.SelectedIndex <= 0)
                {
                    ddlUser.Focus();
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select user');", true);
                }
                else
                {
                    dc.NDTApp_User_Update(Convert.ToInt32(ddlUser.SelectedValue), ddlUser.SelectedItem.Text, Convert.ToInt32(Session["LoginId"]));
                    for (int i = 0; i < grdEnquiry.Rows.Count; i++)
                    {
                        CheckBox chkSelect = (CheckBox)grdEnquiry.Rows[i].FindControl("chkSelect");
                        Label lblEnquiryId = (Label)grdEnquiry.Rows[i].FindControl("lblEnquiryId");
                        Label lblClientName = (Label)grdEnquiry.Rows[i].FindControl("lblClientName");
                        Label lblSiteName = (Label)grdEnquiry.Rows[i].FindControl("lblSiteName");
                        if (chkSelect.Checked == true)
                        {
                            dc.NDTApp_UserWiseEnquiry_Update(Convert.ToInt32(ddlUser.SelectedValue), lblEnquiryId.Text, Convert.ToInt32(Session["LoginId"]), lblClientName.Text, lblSiteName.Text); 
                        }
                    }
                    
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enquiry assigned successfully');", true);
                    LoadEnquiryList();
                }
            }
        }

        protected void optEnquiry_CheckedChanged(object sender, EventArgs e)
        {
            grdEnquiry.DataSource = null;
            grdEnquiry.DataBind();
            if (optAssigned.Checked == true)
                grdEnquiry.Columns[6].Visible = true;
            else
                grdEnquiry.Columns[6].Visible = false;
        }
    }
}
