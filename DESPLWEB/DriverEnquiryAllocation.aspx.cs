using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class DriverEnquiryAllocation : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Enquiry-Driver Allocation";

                // bool userRight = false;
                bool userRight = true;
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    //var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                    //foreach (var u in user)
                    //{
                    //    if (u.USER_Admin_right_bit == true || u.USER_CS_right_bit == true)
                    //    {
                    //        userRight = true;
                    //    }
                    //}
                }
                if (userRight == false)
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
                else
                {
                   LoadDriver();
                }
            }

        }
        private void LoadDriver()
        {
            ddlDriver.DataTextField = "USER_Name_var";
            ddlDriver.DataValueField = "USER_Id";
            var driver = dc.Driver_View(false);
            ddlDriver.DataSource = driver;
            ddlDriver.DataBind();
            ddlDriver.Items.Insert(0, "---Select---");
        }
        private void DisplayEnquiryAllocation(int DriverId)
        {
            grdRoute.DataSource = null;
            grdRoute.DataBind();
            //Label lblMsg = (Label)Master.FindControl("lblMsg");
            //lblMsg.Visible = false;

            DataTable dt = new DataTable();
            //DataRow drow = null;
            dt.Columns.Add(new DataColumn("ROUTE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ROUTE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("driver_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Enq_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("collection_date", typeof(string)));
            dt.Columns.Add(new DataColumn("id", typeof(string)));
            var result = dc.PickUpAlloctionDriver_View(DriverId);
            grdRoute.DataSource = result;
            grdRoute.DataBind();
           
            var data = dc.Driver_View(false).ToList();
            for (int i = 0; i < grdRoute.Rows.Count; i++)
            {   
                DropDownList ddl_Driver = (DropDownList)grdRoute.Rows[i].FindControl("ddl_Driver");
                Label lblDriverId = (Label)grdRoute.Rows[i].FindControl("lblDriverId");
                ddl_Driver.DataSource = data;
                ddl_Driver.DataTextField = "USER_Name_var";
                ddl_Driver.DataValueField = "USER_Id";
                ddl_Driver.DataBind();
                ddl_Driver.Items.Insert(0, new ListItem("---Select---", "0"));
                if (lblDriverId.Text != "" && Convert.ToInt32(lblDriverId.Text) > 0)
                {
                    ddl_Driver.SelectedValue = lblDriverId.Text;
                }
                
            }
            lblRec.Text = "Total No. of Records : " + grdRoute.Rows.Count;
            //lnkSave.Enabled = true;
        }

    

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
            else
            {
                Response.Redirect("Home.aspx");
            }
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if(ddlDriver.SelectedIndex>0)
             DisplayEnquiryAllocation(Convert.ToInt32(ddlDriver.SelectedItem.Value));
        }

        protected void grdRoute_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            LinkButton lnkEnterReport = (LinkButton)grdrow.FindControl("lnkSave");
            Label lblPicUpAlloctnId = (Label)grdrow.FindControl("lblPicUpAlloctnId");
            DropDownList ddl_Driver = (DropDownList)grdrow.FindControl("ddl_Driver");
            int rowindex = grdrow.RowIndex;
            if (e.CommandName == "SaveDiverToEnq")
            {
                if (Convert.ToInt32(ddl_Driver.SelectedItem.Value) > 0)
                {
                    dc.PickUpAllocation_Update_Driver(Convert.ToInt32(lblPicUpAlloctnId.Text), Convert.ToInt32(ddl_Driver.SelectedItem.Value));
                    lblMsg.Text = "Records updated Successfully";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    DisplayEnquiryAllocation(Convert.ToInt32(ddlDriver.SelectedValue));
                }
                else
                {
                    lblMsg.Text = "Select driver to update.";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    ddl_Driver.Focus();
                }
            }
        }
    }
}