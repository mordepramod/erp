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
using System.Drawing;
using System.Text;
using System.Collections.Specialized;
using System.Data.OleDb;
using System.Globalization;

namespace DESPLWEB
{
    public partial class Route3 : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (lblheading != null)
                {
                    lblheading.Text = "Route Updation";
                }
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
                else
                {
                    lblRouteStatus.Text = "ALL";
                    grdRouteBind();
                    bindDriverName();
                    bindVehicleNumber();
                    ddlCollectionDay.Items.Clear();
                }
            }

        }
        protected void grdRouteBind()
        {
            var a = dc.Route_View(0, "", lblRouteStatus.Text.ToString(), 0);
            GrdRouteMst.DataSource = a;
            GrdRouteMst.DataBind();

            if (GrdRouteMst.Rows.Count <= 0)
            {
                FirstGridViewRowOfRouteDetails();
            }

        }
        private void LoadDaySpecificDetails()
        {
            //Label lblCollectionDay = (Label)grdDaySpecific.FindControl("lblCollectionDay");
            if (lblCollRouteId.Text != "")
            {
                var a = dc.Collection_View((Convert.ToInt32(lblCollRouteId.Text)));
                grdDaySpecific.DataSource = a;
                grdDaySpecific.DataBind();
            }
            lblRouteGrdDaySpecific.Visible = true;
            if (grdDaySpecific.Rows.Count <= 0)
            {
                FirstGridViewRowOfDaySpecificDetails();
            }

        }
        public void bindDriverName()
        {
           // var a = dc.getDDLUser();
            var a = dc.User_View(0,-1,"","","");
            ddlDriverName.DataSource = a;
            ddlDriverName.DataTextField = "USER_Name_var";
            ddlDriverName.DataValueField = "USER_Id";
            ddlDriverName.DataBind();

            ddlDriverName.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        public void bindVehicleNumber()
        {
            var datasource = from x in new LabDataDataContext().Vehicle_View(0,"",0) //Convert.ToInt32(Session["Vehicle_Id"]), "", 0)
                             select new
                             {
                                 x.Vehicle_Id,
                                 x.Vehicle_Name_var,
                                 x.Vehicle_No_var,
                                 DisplayField = String.Format("{0} -- {1}", x.Vehicle_Name_var, x.Vehicle_No_var)
                             };
            ddlVehicleNumber.DataSource = datasource;
            ddlVehicleNumber.DataTextField = "DisplayField";
            ddlVehicleNumber.DataValueField = "Vehicle_Id";
            ddlVehicleNumber.DataBind();
            ddlVehicleNumber.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        private void clr()
        {
            txtRouteId.Text = "";
            txtRouteName.Text = "";
            ddlRouteStatus.SelectedValue = "true";

            lblMessage.Visible = false;
            //lblRouteGrdDaySpecific.Visible = false;
            lblDaySpecificMessage.Visible = false;

            lnkRouteSave.Enabled = true;
            lnkDaySpecificSave.Enabled = true;

            ddlDriverName.ClearSelection();
            ddlVehicleNumber.ClearSelection();
        }

        protected void lnkLoadDaySpecific(object sender, CommandEventArgs e)
        {
            lblCollRouteId.Text = e.CommandArgument.ToString();
            
            LoadDaySpecificDetails();
            LinkButton lb = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)lb.NamingContainer;
            Label lblRouteName = (Label)gr.FindControl("lblRouteName");
            lblDaySpecificRoute.Text = "Route : " + lblRouteName.Text;
            lblDaySpecificRoute.Visible = true;
            GrdRouteMst.Visible = true;
            grdDaySpecific.Visible = true;

        }

        protected void imgRouteInsert_Click(object sender, EventArgs e)
        {
            lblpopuphead.Text = "Add New Route";
             txtRouteId.Text = "0";
            lblDaySpecificRoute.Visible = false;
            lblRouteGrdDaySpecific.Visible = false;
            ModalPopupExtender1.Show();
        }
        protected void imgInsertDaySpecific_Click(object sender, CommandEventArgs e)
        {
            ddlCollectionDay.Items.Clear();

            lblPopupHeadDaySpecific.Text = "Add New Day Specific Details";
            //lblCollRouteId.Text = 0;
            var culture = CultureInfo.GetCultureInfo("en-US");
            var dateTimeInfo = DateTimeFormatInfo.GetInstance(culture);
            var collectionDay = e.CommandArgument.ToString();

            Boolean flgFound = false;
            foreach (string DayName in dateTimeInfo.DayNames)
            {
                flgFound = false;
                for (int i = 0; i <= grdDaySpecific.Rows.Count - 1; i++)
                {
                    Label lblCollectionDay = (Label)grdDaySpecific.Rows[i].Cells[0].FindControl("lblCollectionDay");

                    if (DayName == lblCollectionDay.Text)
                    {
                        flgFound = true;
                        //goto nxt;
                        break;
                    }
                }
                // nxt:
                if (flgFound == false)
                {
                    ddlCollectionDay.Items.Add(DayName);
                }
            }

            ModalPopupExtender2.Show();

        }
        protected void imgRouteEdit_Click(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            GridViewRow gr = (GridViewRow)lb.NamingContainer;

            Label lblRouteId = (Label)gr.FindControl("lblRouteId");
             txtRouteId.Text = lblRouteId.Text;
            if ( txtRouteId.Text.ToString() == "0")
                ModalPopupExtender1.Hide();

            Label lblRouteName = (Label)gr.FindControl("lblRouteName");
            txtRouteName.Text = lblRouteName.Text;

            Label lblRouteStatus = (Label)gr.FindControl("lblRouteStatus");
            ddlRouteStatus.Text = lblRouteStatus.Text;

            lblpopuphead.Text = "Edit Route Details";
            ModalPopupExtender1.Show();

        }
        protected void imgEditDaySpecific_Click(object sender, CommandEventArgs e)
        {
            ddlCollectionDay.Items.Clear();
            ImageButton lb = (ImageButton)sender;
            GridViewRow gr = (GridViewRow)lb.NamingContainer;

            lblPopupHeadDaySpecific.Text = "Edit Day Specific Details";
            Label lblDRouteId = (Label)gr.FindControl("lblDRouteId");
            lblCollRouteId.Text = lblDRouteId.Text;

            ddlCollectionDay.Items.Add(e.CommandArgument.ToString());

            Label lblDriverId = (gr.FindControl("lblDriverId") as Label);
            ddlDriverName.ClearSelection();
            ddlDriverName.Items.FindByText(lblDriverId.Text).Selected = true;
            lblCollDriverId.Text = ddlDriverName.Text;

            Label lblVehicleId = (gr.FindControl("lblVehicleId") as Label);
            ddlVehicleNumber.ClearSelection();
            ddlVehicleNumber.Items.FindByText(lblVehicleId.Text).Selected = true;
            lblCollVehicleId.Text = ddlVehicleNumber.Text;

            var culture = CultureInfo.GetCultureInfo("en-US");
            var dateTimeInfo = DateTimeFormatInfo.GetInstance(culture);
            var collectionDay = e.CommandArgument.ToString();

            Boolean flgFound = false;
            foreach (string DayName in dateTimeInfo.DayNames)
            {
                flgFound = false;
                for (int i = 0; i <= grdDaySpecific.Rows.Count - 1; i++)
                {
                    Label lblCollectionDay1 = (Label)grdDaySpecific.Rows[i].Cells[0].FindControl("lblCollectionDay");

                    if (DayName == lblCollectionDay1.Text)
                    {
                        flgFound = true;

                        //goto nxt;
                        break;
                    }

                }
                // nxt:
                if (flgFound == false)
                {
                    ddlCollectionDay.Items.Add(DayName);

                }

            }

            ModalPopupExtender2.Show();
        }

        protected void imgClosePopupRoute_Click(object sender, ImageClickEventArgs e)
        {
            clr();
            ModalPopupExtender1.Hide();
        }

        protected void lnkRouteSave_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (Page.IsValid)
            {
                bool routeStatus = false;
                //check for duplicate Route
                var rt = dc.Route_View(0, txtRouteName.Text, lblRouteStatus.Text.ToString(), 0);
                foreach (var Route in rt)
                {
                    if (Convert.ToInt32( txtRouteId.Text) == 0)
                    {
                        routeStatus = true;
                        lblMessage.Text = "Duplicate Route Name..";
                        lblMessage.Visible = true;
                        break;
                    }
                    else if (Convert.ToInt32( txtRouteId.Text) > 0)
                    {
                        if (Route.ROUTE_Id != Convert.ToInt32( txtRouteId.Text))
                        {
                            routeStatus = true;
                            lblMessage.Text = "Duplicate Route Name..";
                            lblMessage.Visible = true;
                            break;
                        }
                    }
                }
                //
                if (routeStatus == false)
                {
                    if (ddlRouteStatus.SelectedValue == "true")
                        routeStatus = false;
                    else
                        routeStatus = true;

                    dc.Route_Update(Convert.ToInt32( txtRouteId.Text), txtRouteName.Text, routeStatus);

                    lblMessage.Text = "Updated Successfully";
                    lblMessage.Visible = true;
                    grdRouteBind();
                    //ClearAllControls();                 
                    //ModalPopupExtender1.Hide();  
                    lnkRouteSave.Enabled = false;
                }
            }
        }
        protected void imgClosePopupDaySpecific_Click(object sender, ImageClickEventArgs e)
        {
            clr();
            ModalPopupExtender2.Hide();
        }

        protected void lnkDaySpecificSave_Click(object sender, EventArgs e)
        {
            if (lblPopupHeadDaySpecific.Text.Equals("Add New Day Specific Details"))
            {
                dc.Collection_Update("Insert", ddlCollectionDay.Text, lblCollRouteId.Text, ddlDriverName.Text, ddlVehicleNumber.Text, "", "");

            }
            else if (lblPopupHeadDaySpecific.Text.Equals("Edit Day Specific Details"))
            {
                dc.Collection_Update("Update", ddlCollectionDay.Text, lblCollRouteId.Text , lblCollDriverId.Text, lblCollVehicleId.Text , ddlDriverName.Text, ddlVehicleNumber.Text);

            }

            lblDaySpecificMessage.Text = "Updated Successfully";
            lblDaySpecificMessage.Visible = true;
            LoadDaySpecificDetails();
            lnkDaySpecificSave.Enabled = false;


        }


        private void FirstGridViewRowOfRouteDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Route_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Route_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("Route_Status_bit", typeof(string)));

            dr = dt.NewRow();
            dr["Route_Id"] = 0;
            dr["Route_Name_var"] = string.Empty;
            dr["Route_Status_bit"] = string.Empty;

            dt.Rows.Add(dr);

            //grdDaySpecific.DataSource = dt;
            //grdDaySpecific.DataBind();

            GrdRouteMst.DataSource = dt;
            GrdRouteMst.DataBind();

        }
        private void FirstGridViewRowOfDaySpecificDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Coll_Day_var", typeof(string)));
            dt.Columns.Add(new DataColumn("Coll_Route_Id_int", typeof(string)));
            dt.Columns.Add(new DataColumn("USER_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("Vehicle_No_var", typeof(string)));
            dt.Columns.Add(new DataColumn("Vehicle_Name_var", typeof(string)));
            //dt.Columns.Add(new DataColumn("Coll_Temp_Id", typeof(string)));

            dr = dt.NewRow();
            dr["Coll_Day_var"] = string.Empty; ;
            dr["Coll_Route_Id_int"] = 0;
            dr["USER_Name_var"] = string.Empty;
            dr["Vehicle_No_var"] = string.Empty;
            dr["Vehicle_Name_var"] = string.Empty;
            //dr["Coll_Temp_Id"] = 0;

            dt.Rows.Add(dr);

            grdDaySpecific.DataSource = dt;
            grdDaySpecific.DataBind();

        }

        protected void ddlVehNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();
        }
        protected void ddlCollDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();
        }
        protected void ddlDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();
        }

        protected void grdDaySpecific_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDRouteId = ((Label)e.Row.FindControl("lblDRouteId"));
                if (lblDRouteId.Text == "0")
                {
                    ((ImageButton)e.Row.FindControl("imgEditDaySpecific")).Visible = false;
                }
            }
        }

        protected void GrdRouteMst_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRouteId = ((Label)e.Row.FindControl("lblRouteId"));
                if (lblRouteId.Text == "0")
                {
                    ((ImageButton)e.Row.FindControl("imgRouteEdit")).Visible = false;
                    ((LinkButton)e.Row.FindControl("btnDaySpecific")).Visible = false;
                }
            }
        }

        protected void DDLSelectedRouteStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList DDLSelectedRouteStatus = (DropDownList)sender;

            lblRouteStatus.Text = DDLSelectedRouteStatus.SelectedValue;
            this.grdRouteBind();

            lblRouteGrdDaySpecific.Visible = false;
            lblDaySpecificRoute.Visible = false;
            grdDaySpecific.Visible = false;

        }


    }
}
