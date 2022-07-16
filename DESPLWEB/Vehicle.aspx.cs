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

namespace DESPLWEB
{
    public partial class Vehicle : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblVehiclId.Text = "0";
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (lblheading != null)
                {
                    lblheading.Text = "Vehicle Updation";
                }
                grdVehicleBind();

            }
        }
        protected void grdVehicleBind()
        {
            var a = dc.Vehicle_View(0, "", 0);
            grdVehicle.DataSource = a;
            grdVehicle.DataBind();

            if (grdVehicle.Rows.Count <= 0)
            {
                FirstGridViewRowOfVehicleDetails();
            }


        }
        private void clr()
        {
            txtVehicleId.Text = "";
            txtVehicleName.Text = "";
            txtVehicleNo.Text = "";
            ddlfVehicleStatus.SelectedValue = "true";
            lblVehicleMessage.Visible = false;
            lnkSave.Enabled = true;
        }

        protected void imgVehicleInsert_Click(object sender, EventArgs e)
        {
            lblpopuphead.Text = "Add New Vehicle";
            lblVehiclId.Text = "0";
            ModalPopupExtender1.Show();

        }

        protected void imgVehicleEdit_Click(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            GridViewRow gr = (GridViewRow)lb.NamingContainer;

            Label lblVehicleId = (Label)gr.FindControl("lblVehicleId");
            lblVehiclId.Text = lblVehicleId.Text;

            if (lblVehiclId.Text.ToString() == "0")
                ModalPopupExtender1.Hide();

            Label lblVehicleName = (Label)gr.FindControl("lblVehicleName");
            txtVehicleName.Text = lblVehicleName.Text;

            Label lblVehicleNo = (Label)gr.FindControl("lblVehicleNo");
            txtVehicleNo.Text = lblVehicleNo.Text;

            Label lblVehicleStatus = (Label)gr.FindControl("lblVehicleStatus");
            ddlfVehicleStatus.Text = lblVehicleStatus.Text;

            lblpopuphead.Text = "Edit Vehicle Details";
            ModalPopupExtender1.Show();

        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            clr();

            ModalPopupExtender1.Hide();
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            lblVehicleMessage.Visible = false;
            if (Page.IsValid)
            {
                bool VehicleStatus = false;
                //check for duplicate Vehicle
                var s = dc.Vehicle_View(0, txtVehicleNo.Text, 0);
                foreach (var Vehicle in s)
                {
                    if (Convert.ToInt32(lblVehiclId.Text) == 0)
                    {
                        VehicleStatus = true;
                        lblVehicleMessage.Text = "Duplicate Vehicle No..";
                        lblVehicleMessage.Visible = true;
                        break;
                    }
                    else if (Convert.ToInt32(lblVehiclId.Text) > 0)
                    {
                        if (Vehicle.Vehicle_Id != Convert.ToInt32(lblVehiclId.Text))
                        {
                            VehicleStatus = true;
                            lblVehicleMessage.Text = "Duplicate Vehicle No..";
                            lblVehicleMessage.Visible = true;
                            break;
                        }
                    }
                }
                //
                if (VehicleStatus == false)
                {
                    if (ddlfVehicleStatus.SelectedValue == "true")
                        VehicleStatus = false;
                    else
                        VehicleStatus = true;

                    dc.Vehicle_Update(Convert.ToInt32(lblVehiclId.Text), txtVehicleName.Text, txtVehicleNo.Text, VehicleStatus);

                    lblVehicleMessage.Text = "Updated Successfully";
                    lblVehicleMessage.Visible = true;
                    grdVehicleBind();
                    //ClearAllControls();
                    //ModalPopupExtender2.Hide();
                    lnkSave.Enabled = false;
                }

            }
        }


        private void FirstGridViewRowOfVehicleDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Vehicle_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Vehicle_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("Vehicle_No_var", typeof(string)));
            dt.Columns.Add(new DataColumn("Vehicle_Status_bit", typeof(string)));

            dr = dt.NewRow();
            dr["Vehicle_Id"] = 0;
            dr["Vehicle_Name_var"] = string.Empty;
            dr["Vehicle_No_var"] = string.Empty;
            dr["Vehicle_Status_bit"] = string.Empty;

            dt.Rows.Add(dr);

            grdVehicle.DataSource = dt;
            grdVehicle.DataBind();

        }

        protected void grdVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblVehicleId = ((Label)e.Row.FindControl("lblVehicleId"));
                if (lblVehicleId.Text == "0")
                {
                    ((ImageButton)e.Row.FindControl("imgVehicleEdit")).Visible = false;
                }
            }
        }



    }
}
