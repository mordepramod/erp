using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class DispatchSheetMEWise : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDt.Text = DateTime.Today.ToString("dd/MM/yyyy");
                  
                lblUserId.Text = Session["LoginId"].ToString();
                //bool dispatchSheetRight = false;
                //var user = dc.User_View(Convert.ToInt32(lblUserId.Text), 0, "", "", "");
                //foreach (var u in user)
                //{
                //    if (u.USER_SuperAdmin_right_bit == true)
                //        dispatchSheetRight = true;
                //}
                //if (dispatchSheetRight == true)
                //{
                    Label lblheading = (Label)Master.FindControl("lblheading");
                    lblheading.Text = "Dispatch Sheet";

                    LoadRouteList();
                //}
                //else
                //{
                //    pnlContent.Visible = false;
                //    lblAccess.Visible = true;
                //    lblAccess.Text = "Access is Denied.. ";
                //}
            }
        }
        private void LoadMeList()
        {
            var data = dc.User_View_ME(-1);
            ddl_ME_Route.DataSource = data;
            ddl_ME_Route.DataTextField = "USER_Name_var";
            ddl_ME_Route.DataValueField = "USER_Id";
            ddl_ME_Route.DataBind();
            ddl_ME_Route.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        private void LoadRouteList()
        {
            var routeList = dc.Route_View(0, "", "All", 0);
            ddl_ME_Route.DataTextField = "Route_Name_var";
            ddl_ME_Route.DataValueField = "Route_Id";
            ddl_ME_Route.DataSource = routeList;
            ddl_ME_Route.DataBind();
            ddl_ME_Route.Items.Insert(0, "---Select---");
            ddl_ME_Route.Items.Insert(1, "All");
          
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (ddl_ME_Route.SelectedIndex != 0)
            {

                DateTime otDt = DateTime.ParseExact(txtDt.Text, "dd/MM/yyyy", null);
               
                if (ddlReportFor.SelectedItem.Text == "MeWise")//me
                {
                    var Inward = dc.Report_View_MEWise_DispatchSheet(Convert.ToInt32(ddl_ME_Route.SelectedValue.ToString()), 0, otDt);
                    grdReports.DataSource = Inward;
                    grdReports.DataBind();
                }
                else if (ddlReportFor.SelectedItem.Text == "RouteWise")//route
                {
                    if (ddl_ME_Route.SelectedIndex == 1)//for all
                    {
                        var Inward = dc.Report_View_MEWise_DispatchSheet(-1, 1, otDt);
                        grdReports.DataSource = Inward;
                        grdReports.DataBind();
                    }
                    else
                    {
                        var Inward = dc.Report_View_MEWise_DispatchSheet(0, Convert.ToInt32(ddl_ME_Route.SelectedValue.ToString()), otDt);
                        grdReports.DataSource = Inward;
                        grdReports.DataBind();
                    }
                }
                lblCount.Text = "Total No of Records : "+grdReports.Rows.Count;
            }
        }

       
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            string str = "", MeNo="",MeName="";
            if (grdReports.Rows.Count > 0)
            {
                if (ddlReportFor.SelectedItem.Text.Equals("RouteWise"))
                {
                    if (ddl_ME_Route.SelectedIndex > 1)
                    {
                        var data = dc.User_View_ME(Convert.ToInt32(ddl_ME_Route.SelectedValue)).ToList();
                        if (data.Count > 0)
                        {
                            foreach (var item in data)
                            {
                                MeNo = Convert.ToString(item.USER_ContactNo_var);
                                MeName = Convert.ToString(item.USER_Name_var);
                            }
                            if (MeNo != "" && MeNo != null)
                                MeNo = " - " + MeNo;

                            str = "(Route Name :" + ddl_ME_Route.SelectedItem.Text + " , ME Name :" + MeName + MeNo + ")";

                        }
                    }
                    else
                    {
                        str = "(Route Name :" + ddl_ME_Route.SelectedItem.Text + " )";
                    }
                }
                else
                {
                    str = "(ME Name :" + ddl_ME_Route.SelectedItem.Text + " )";
                }
                
                PrintGrid.PrintGridView(grdReports, ddlReportFor.SelectedItem.Text + " Dispatch Sheet "+str, ddlReportFor.SelectedItem.Text + "DispatchSheet_" + ddl_ME_Route.SelectedItem.Text.Replace(" ","_"));

            }
        }

        protected void ddl_ME_Route_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdReports.DataSource = null;
            grdReports.DataBind();
            lblCount.Text = "Total No of Records : " + grdReports.Rows.Count;
        }

      
        protected void ddlReportFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReportFor.SelectedItem.Text == "RouteWise")//route wise
            {
                lblMe.Text = "List of Route";
                LoadRouteList();
            }
            else if (ddlReportFor.SelectedItem.Text == "MeWise")
            {
                lblMe.Text = "List of Me";
                LoadMeList();
            }
            else
            {
                ddl_ME_Route.SelectedIndex = 0;
            }

            grdReports.DataSource = null;
            grdReports.DataBind();
            lblCount.Text = "Total No of Records : " + grdReports.Rows.Count;
        }
    }
}