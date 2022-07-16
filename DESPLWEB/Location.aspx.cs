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
using System.Text;
using System.Drawing;

namespace DESPLWEB
{
    public partial class Location : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        static int LocationId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LocationId = 0;
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (lblheading != null)
                {
                    lblheading.Text = "Location Updation";
                }
                grdLocationBind();
                bindChkRoute();
            }
        }

        protected void grdLocationBind()
        {
            var a = dc.Location_View(0, "", 0);
            grdLocation.DataSource = a;
            grdLocation.DataBind();

            if (grdLocation.Rows.Count <= 0)
            {
                FirstGridViewRowOfLocation();
            }
        }
        public void bindChkRoute()
        {
            var a = dc.getChkRoute();

            chkRoute.DataSource = a;
            chkRoute.DataTextField = "Route_Name_var";
            chkRoute.DataValueField = "Route_Id";
            chkRoute.DataBind();

        }
        private void clr()
        {
            lblpopuphead.Text = "";
            txtLocationName.Text = "";
            ddlLocationStatus.SelectedValue = "true";
            lblLocationMessage.Visible = false;
            lnkLocationSave.Enabled = true;
            for (int j = 0; j < chkRoute.Items.Count; j++)
            {
                chkRoute.Items[j].Selected = false;
            }

        }
        protected void imgLocationInsert_Click(object sender, EventArgs e)
        {
            lblpopuphead.Text = "Add New Location";
            LocationId = 0;
            ModalPopupExtender1.Show();
            lblRoute.Visible = false;
            chkPnl.Visible = false;
            lblLocationMessage.Visible = false;

        }

        protected void imgLocationEdit_Click(object sender, EventArgs e)
        {
            lblRoute.Visible = true;
            chkPnl.Visible = true;

            ImageButton lb = (ImageButton)sender;
            GridViewRow gr = (GridViewRow)lb.NamingContainer;

            Label lblLocationId = (Label)gr.FindControl("lblLocationId");
            LocationId = Convert.ToInt32(lblLocationId.Text) ;

            if (LocationId.ToString() == "0")
                ModalPopupExtender1.Hide();

            Label lblLocationName = (Label)gr.FindControl("lblLocationName");
            txtLocationName.Text = lblLocationName.Text;

            Label lblLocationStatus = (Label)gr.FindControl("lblLocationStatus");
            ddlLocationStatus.Text = lblLocationStatus.Text;

            var s = dc.Route_View(0, "", "", Convert.ToInt32(LocationId)); //Session["Route_Status_bit"].ToString(), Convert.ToInt32(LocationId));

            foreach (var route in s)
            {
                for (int j = 0; j < chkRoute.Items.Count; j++)
                {
                    if (route.RouteLocations_ROUTE_Id == Convert.ToInt32(chkRoute.Items[j].Value))
                    {
                        chkRoute.Items[j].Selected = true;
                        break;
                    }
                }
            }

            lblpopuphead.Text = "Edit Location Details";
            ModalPopupExtender1.Show();
        }



        private void FirstGridViewRowOfLocation()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("LOCATION_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("LOCATION_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("LOCATION_Status_bit", typeof(string)));
            dt.Columns.Add(new DataColumn("LOCATION_CreatedOn_dt", typeof(string)));

            dr = dt.NewRow();
            dr["LOCATION_Id"] = 0;
            dr["LOCATION_Name_var"] = string.Empty;
            dr["LOCATION_Status_bit"] = string.Empty;
            dr["LOCATION_CreatedOn_dt"] = string.Empty;

            dt.Rows.Add(dr);

            grdLocation.DataSource = dt;
            grdLocation.DataBind();

        }

        protected void grdLocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblLocationId = ((Label)e.Row.FindControl("lblLocationId"));
                if (lblLocationId.Text == "0")
                {
                    ((ImageButton)e.Row.FindControl("imgLocationEdit")).Visible = false;
                }
            }

        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            clr();
            ModalPopupExtender1.Hide();
        }

        protected void lnkLocationSave_Click(object sender, EventArgs e)
        {

         
            lblLocationMessage.Visible = false;
            if (Page.IsValid)
            {
                bool LocationStatus = false;
                //check for duplicate Location
                var s = dc.Location_View(0, txtLocationName.Text, 0);
                foreach (var Location in s)
                {
                    if (Convert.ToInt32(LocationId) == 0)
                    {
                        LocationStatus = true;
                        lblLocationMessage.Text = "Duplicate Location Name..";
                        lblLocationMessage.Visible = true;
                        break;
                    }
                    else if (Convert.ToInt32(LocationId) > 0)
                    {
                        if (Location.LOCATION_Id != Convert.ToInt32(LocationId))
                        {
                            LocationStatus = true;
                            lblLocationMessage.Text = "Duplicate Location Name..";
                            lblLocationMessage.Visible = true;
                            break;
                        }
                    }
                }
                //
                if (LocationStatus == false)
                {
                    if (ddlLocationStatus.SelectedValue == "true")//active
                        LocationStatus = false;
                    else
                        LocationStatus = true;

                    dc.Location_Update(Convert.ToInt32(LocationId), txtLocationName.Text, LocationStatus);
                    dc.LocationRoute_Update("delete", LocationId.ToString(), "");

                    if (LocationStatus)//if location status deactivated then make loaction id 0 in tbl_site where that location is assign
                    {
                        dc.Site_Update_Location(Convert.ToInt32(LocationId));
                    }

                    foreach (ListItem oItem in chkRoute.Items)
                    {
                        if (oItem.Selected)
                        {
                            dc.LocationRoute_Update("add", LocationId.ToString(), oItem.Value);
                        }
                    }
                    lblLocationMessage.Text = "Updated Successfully";
                    lblLocationMessage.Visible = true;
                    grdLocationBind();
                    //ClearAllControls();
                    //ModalPopupExtender2.Hide();
                    lnkLocationSave.Enabled = false;
                }

            }

        }


    }
}
