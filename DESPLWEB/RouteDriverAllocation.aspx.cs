using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class RouteDriverAllocation : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Route Driver Allocation";
                
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
                    DisplayRouteMEAllocation();
                }
            }

        }

        private void DisplayRouteMEAllocation()
        {
            grdRoute.DataSource = null;
            grdRoute.DataBind();
            
            DataTable dt = new DataTable();
            DataRow drow = null;
            dt.Columns.Add(new DataColumn("ROUTE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ROUTE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("ROUTE_Driver_Id", typeof(string)));
            var route = dc.Route_View(0, "", "0", 0); //"ALL"
            foreach (var rt in route)
            {   
                drow = dt.NewRow();
                drow["ROUTE_Name_var"] = rt.ROUTE_Name_var;
                drow["ROUTE_Id"] = rt.ROUTE_Id;
                drow["ROUTE_Driver_Id"] = rt.ROUTE_Driver_Id;
                dt.Rows.Add(drow);
            }
            grdRoute.DataSource = dt;
            grdRoute.DataBind();

            var data = dc.Driver_View(false).ToList();
            for (int i = 0; i < grdRoute.Rows.Count; i++)
            {   
                CheckBox cbxSelect = (CheckBox)grdRoute.Rows[i].FindControl("cbxSelect");
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
                cbxSelect.Checked = false;
            }
            CheckBox cbxSelectAll = (CheckBox)grdRoute.HeaderRow.FindControl("cbxSelectAll");
            cbxSelectAll.Checked = false;
            lblRec.Text = "Total No. of Records : " + grdRoute.Rows.Count;
            lnkSave.Enabled = true;
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            lblMsg.ForeColor = System.Drawing.Color.Red;
            bool selectedFlag = false, updateFlag= true;
            for (int i = 0; i < grdRoute.Rows.Count; i++)
            {
                Label lblRouteName = (Label)grdRoute.Rows[i].FindControl("lblRouteName");
                CheckBox cbxSelect = (CheckBox)grdRoute.Rows[i].FindControl("cbxSelect");
                DropDownList ddl_Driver = (DropDownList)grdRoute.Rows[i].FindControl("ddl_Driver");
                if (cbxSelect.Checked == true)
                {
                    selectedFlag = true;
                    if (ddl_Driver.SelectedIndex == 0)
                    {
                        lblMsg.Text = "Select Driver for route '" + lblRouteName.Text + "'";
                        lblMsg.Visible = true;
                        updateFlag = false;
                    }
                }
            }
            if (selectedFlag == false)
            {
                updateFlag = false;
                lblMsg.Text = "Select at least one route for update. ";
                lblMsg.Visible = true;
            }
            if (updateFlag == true)
            {
                for (int i = 0; i < grdRoute.Rows.Count; i++)
                {
                    CheckBox cbxSelect = (CheckBox)grdRoute.Rows[i].FindControl("cbxSelect");
                    Label lblRouteId = (Label)grdRoute.Rows[i].FindControl("lblRouteId");
                    Label lblDriverId = (Label)grdRoute.Rows[i].FindControl("lblDriverId");
                    DropDownList ddl_Driver = (DropDownList)grdRoute.Rows[i].FindControl("ddl_Driver");
                    if (cbxSelect.Checked == true && ddl_Driver.SelectedIndex > 0)
                    {
                        dc.Route_Update_Driver(Convert.ToInt32(lblRouteId.Text), Convert.ToInt32(ddl_Driver.SelectedValue));
                        updateFlag = true;
                    }
                }
                lblMsg.Text = "Records updated Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                updateFlag = true;
                DisplayRouteMEAllocation();
            }
        }

        //protected void RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        //Binding DDL Driver
        //        DropDownList ddl_ME = (DropDownList)e.Row.FindControl("ddl_ME");
        //        var data = dc.User_View_ME();
        //        ddl_ME.DataSource = data;
        //        ddl_ME.DataTextField = "USER_Name_var";
        //        ddl_ME.DataValueField = "USER_Id";
        //        ddl_ME.DataBind();
        //        ddl_ME.Items.Insert(0, new ListItem("---Select---", "0"));

        //    }
        //}
        protected void grdRoute_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox cbxSelect = (CheckBox)e.Row.Cells[2].FindControl("cbxSelect");
                CheckBox cbxSelectAll = (CheckBox)this.grdRoute.HeaderRow.FindControl("cbxSelectAll");
                cbxSelect.Attributes["onclick"] = string.Format
                                                       (
                                                          "javascript:ChildClick(this,'{0}');",
                                                          cbxSelectAll.ClientID
                                                       );
            }
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

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdRoute.Rows.Count > 0)
            {
                string reportStr = "";
                reportStr = getStringOfRouteDriverAllocation();
                PrintHTMLReport rpt = new PrintHTMLReport();
                rpt.DownloadHtmlReport("RouteDriverAllocation", reportStr);
            }
        }

        protected string getStringOfRouteDriverAllocation()
        {
            string reportStr = "", mySql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Route - Driver Allocation </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            mySql += "<tr>" +
                "<td width='99%' colspan='7'><font size=2><b>Total No of Records : " + grdRoute.Rows.Count + "</b></font></td>" +
                "</tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            
            mySql += "<tr>";
            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b> Route Name  </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Driver </b></font></td>";
            mySql += "</tr>";
            for (int i = 0; i < grdRoute.Rows.Count; i++)
            {
                Label lblRouteName = (Label)grdRoute.Rows[i].FindControl("lblRouteName");
                DropDownList ddl_Driver = (DropDownList)grdRoute.Rows[i].FindControl("ddl_Driver");
                
                mySql += "<tr>";
                mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + lblRouteName.Text + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + ddl_Driver.SelectedItem.Text + "</font></td>";
                mySql += "</tr>";
            }
            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += "</table>";
            mySql += "</html>";
            return reportStr = mySql;
        }
    }
}