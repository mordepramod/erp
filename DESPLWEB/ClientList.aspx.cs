using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.IO;

namespace DESPLWEB
{
    public partial class ClientList : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Client List";
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                txt_Fromdate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
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

        private void GetDataOfClient(int status, int flag, DateTime? frmDate, DateTime? toDate)
        {
            if (flag == 0)
            {
                var cl = dc.Client_View(0, status, "", "");
                grdClientList.DataSource = cl;
                grdClientList.Columns[11].Visible = false;
                grdClientList.DataBind();
                lblTotalRecords.Text = "Total No Of Records:" + grdClientList.Rows.Count;
            }
            else if (flag == 1)//modified client
            {
                var cl = dc.Client_ModifiedView(frmDate, toDate);

                grdClientList.DataSource = cl;
                grdClientList.Columns[11].Visible = true;
                grdClientList.DataBind();
                lblTotalRecords.Text = "Total No Of Records:" + grdClientList.Rows.Count;
     
            }
            else//gst
            {
                var cl = dc.Client_View_GST(status);
                
                grdClientList.DataSource = cl;
                grdClientList.Columns[11].Visible = false;
                grdClientList.DataBind();
                lblTotalRecords.Text = "Total No Of Records:" + grdClientList.Rows.Count;
     
            }

        }

        protected void AllClient_CheckedChanged(object sender, EventArgs e)
        {
            if (RdnClient.Checked)
            {
                disableFileds();
                GetDataOfClient(-1, 0, null, null);
            }
        }
        protected void contClient_CheckedChanged(object sender, EventArgs e)
        {
            if (RdnContinued.Checked)
            {
                disableFileds();
                GetDataOfClient(1, 0, null, null);
            }
        }
        protected void disContClient_CheckedChanged(object sender, EventArgs e)
        {
            if (RdnDiscontinued.Checked)
            {
                disableFileds();
                GetDataOfClient(0, 0, null, null);
            }
        }
        protected void RdnModifiedList_CheckedChanged(object sender, EventArgs e)
        {
            if (RdnModifiedList.Checked)
            {
                enableFileds();
                grdClientList.DataSource = null;
                grdClientList.DataBind();
                lblTotalRecords.Text = "Total No of Records : 0 ";
     
     
            }

        }
        protected void RdnGstComplete_CheckedChanged(object sender, EventArgs e)
        {
            if (RdnGstComplete.Checked)
            {
                disableFileds();
                GetDataOfClient(0, 2, null, null);
            }
        }
        protected void RdnGstInComplete_CheckedChanged(object sender, EventArgs e)
        {
            if (RdnGstInComplete.Checked)
            {
                disableFileds();
                GetDataOfClient(1, 2, null, null);
            }
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            DateTime? frmDate = null, toDate = null;

            if (txt_Fromdate.Text != "" && txt_ToDate.Text != "")
            {
                frmDate = DateTime.ParseExact(txt_Fromdate.Text, "dd/MM/yyyy", null);
                toDate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
                GetDataOfClient(1, 1, frmDate, toDate);
        
            }

        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {

            string fileNm = "";
            if (RdnClient.Checked)
                fileNm = RdnClient.Text.Replace(' ', '_');
            else if (RdnContinued.Checked)
                fileNm = RdnContinued.Text.Replace(' ', '_');
            else if (RdnDiscontinued.Checked)
                fileNm = RdnDiscontinued.Text.Replace(' ', '_');
            else if (RdnModifiedList.Checked)
                fileNm = RdnModifiedList.Text.Replace(' ', '_');
            else if (RdnGstComplete.Checked)
                fileNm = RdnGstComplete.Text.Replace(' ', '_');
            else if (RdnGstInComplete.Checked)
                fileNm = RdnGstInComplete.Text.Replace(' ', '_');

            PrintGrid.PrintGridView(grdClientList, fileNm, fileNm + "_List");
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


    }
}