using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace DESPLWEB
{
    public partial class SiteList : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
       //DataTable dtTable;
          
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Site List";
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                bool userRight = false;
                Session["Cl_Id"] = 0;
                txt_Fromdate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                //CalendarExtender2.SelectedDate = DateTime.Today.AddDays(-7);
                txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                //CalendarExtender1.SelectedDate = DateTime.Today;
                ViewState["Details"] = null;
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
                        if (u.USER_Admin_right_bit == true)
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
            }

        }
      
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
          
                string fileNm="";
                //NewWindows.PrintDataTable((DataTable)ViewState["gvMobile"], "Site List", "SiteDetailList");
                DataTable dt = (DataTable)ViewState["Details"];
                if (rdnActive.Checked)
                    fileNm = rdnActive.Text.Replace(' ','_');
                else if (rdnDeactive.Checked)
                    fileNm = rdnDeactive.Text.Replace(' ', '_');
                else if (rdnModifiedList.Checked)
                    fileNm = rdnModifiedList.Text.Replace(' ', '_');
                else if (rdnGstComplete.Checked)
                    fileNm = rdnGstComplete.Text.Replace(' ', '_');
                else if (rdnGstInComplete.Checked)
                    fileNm = rdnGstInComplete.Text.Replace(' ', '_');


                PrintGrid.PrintGridView(grdSiteList, fileNm, fileNm + "_List");

           
        }
        private void enableFileds()
        {
            lblFromDt.Visible = true;
            lblToDt.Visible = true;
            txt_Fromdate.Visible = true;
            txt_ToDate.Visible = true;
            ImgBtnSearch.Visible = true;
        }
        private void disableFileds()
        {
            lblFromDt.Visible = false;
            lblToDt.Visible = false;
            txt_Fromdate.Visible = false;
            txt_ToDate.Visible = false;
            ImgBtnSearch.Visible = false;
        }

        protected void rdnModifiedList_CheckedChanged(object sender, EventArgs e)
        {
            if (rdnModifiedList.Checked)
            {
                enableFileds();
                grdSiteList.DataSource = null;
                grdSiteList.DataBind();
                lblTotalRecords.Text = "Total No of Records : 0 ";
            }

        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (txt_Fromdate.Text != "" && txt_ToDate.Text != "")
            {
                DateTime? frmDate, toDate;
                frmDate = DateTime.ParseExact(txt_Fromdate.Text, "dd/MM/yyyy", null);
                toDate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
                GetDataOfSite(1, 1, frmDate, toDate);
            }

        }
        private void GetDataOfSite(int status, int flag, DateTime? frmDate, DateTime? toDate)
        {
           
            if (flag == 0)
            {
                var cl = dc.SiteList_View(status);
                grdSiteList.DataSource = cl;
                grdSiteList.Columns[9].Visible = false;
                grdSiteList.DataBind();
                lblTotalRecords.Text = "Total No Of Records:" + grdSiteList.Rows.Count;
       
            }
            else if (flag == 1)
            {
                 var cl = dc.Site_ModifiedView(frmDate, toDate);
                 grdSiteList.DataSource = cl;
                 grdSiteList.Columns[9].Visible=true;
                 grdSiteList.DataBind();
                 lblTotalRecords.Text = "Total No Of Records:" + grdSiteList.Rows.Count;
       
               
            }
            else//gst
            {
                var cl = dc.Site_View_GST(status);

                grdSiteList.DataSource = cl;
                grdSiteList.Columns[9].Visible = false;
                grdSiteList.DataBind();
                lblTotalRecords.Text = "Total No Of Records:" + grdSiteList.Rows.Count;

            }
        }
     
        protected void rdnActive_CheckedChanged(object sender, EventArgs e)
        {
            if (rdnActive.Checked)
            {
                disableFileds();
                GetDataOfSite(0, 0, null, null);
            }
        }

        protected void rdnDeactive_CheckedChanged(object sender, EventArgs e)
        {
            if (rdnDeactive.Checked)
            {
                disableFileds();
                GetDataOfSite(1, 0, null, null);
               
           }
        }

        protected void rdnGstComplete_CheckedChanged(object sender, EventArgs e)
        {
            if (rdnGstComplete.Checked)
            {
                disableFileds();
                GetDataOfSite(0, 2, null, null);
            }
        }

        protected void rdnGstInComplete_CheckedChanged(object sender, EventArgs e)
        {
            if (rdnGstInComplete.Checked)
            {
                disableFileds();
                GetDataOfSite(1, 2, null, null);
            }
        }
     
    }
}