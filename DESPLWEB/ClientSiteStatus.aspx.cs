using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.IO;

namespace DESPLWEB
{
    public partial class ClientSiteStatus : System.Web.UI.Page
    {

        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Client/Site Status Setting";
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
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
            Response.Redirect("Enquiry_List.aspx");
        }
        protected void RdnClient_CheckedChanged(object sender, EventArgs e)
        {
            ChkSpecificClient.Checked = true;
            ddl_ClientAndSite.Visible = false;
            txt_ClientName.Visible = true;
            ImgBtnSearch.Visible = true;
        }
        protected void RdnSite_CheckedChanged(object sender, EventArgs e)
        {
            ChkSpecificClient.Checked = true;
            ddl_ClientAndSite.Visible = true;
            txt_ClientName.Visible = false;
            ImgBtnSearch.Visible = true;
            LoadClient();
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindData();
        }
        public void BindData()
        {
            if (RdnClient.Checked)
            {
                if (RdnContinued.Checked)
                {
                    ClientContinue();
                }
                else if (RdnDiscontinued.Checked)
                {
                    ClientDisContinue();
                }
                else
                {
                    ViewTestClientAll();//has to check
                }
            }
            else if (RdnSite.Checked)
            {
                if (RdnContinued.Checked)
                {
                    ViewSiteContinue();
                }
                else if (RdnDiscontinued.Checked)
                {
                    ViewSiteDisContinue();
                }
                else
                {
                    ClientwiseAllSiteGrid();
                }
            }
        }
        public void ViewTestClientAll()
        {
            string searchtextClient = "";
            if (txt_ClientName.Text != "")
                searchtextClient = txt_ClientName.Text + "%";
            var Statuscl = dc.ClientSiteStatus_View(txt_ClientName.Text + "%", false, false, false, 0, 0, -1, 0, -1, "");
            grdClientSiteStatus.DataSource = Statuscl;
            grdClientSiteStatus.DataBind();
            if (grdClientSiteStatus.Rows.Count > 0)
            {
                var Status = dc.ClientSiteStatus_View(txt_ClientName.Text + "%", false, false, false, 0, 0, -1, 0, -1, "");
                int i = 0;
                foreach (var grd in Status)
                {
                    TextBox txt_Name = (TextBox)grdClientSiteStatus.Rows[i].Cells[1].FindControl("txt_Name");
                    TextBox txt_Address = (TextBox)grdClientSiteStatus.Rows[i].Cells[2].FindControl("txt_Address");
                    TextBox txt_Status = (TextBox)grdClientSiteStatus.Rows[i].Cells[3].FindControl("txt_Status");

                    txt_Name.Text = grd.CL_Name_var.ToString();
                    if (grd.CL_OfficeAddress_var != null && grd.CL_OfficeAddress_var != "")
                    {
                        txt_Address.Text = grd.CL_OfficeAddress_var.ToString();
                    }
                    else
                    {
                        txt_Address.Text = "";
                    }
                    if (Convert.ToBoolean(grd.CL_Status_bit).ToString() == "False")
                    {

                        txt_Status.Text = "Continue";
                    }
                    else
                    {
                        txt_Status.Text = "Discontinue";
                        txt_Status.BackColor = System.Drawing.Color.LightSalmon;
                    }
                    i++;
                }
            }
        }
        public void ClientContinue()
        {
            string searchtextClient = "";
            if (txt_ClientName.Text != "")
                searchtextClient = txt_ClientName.Text + "%";
            var Statuscl = dc.ClientSiteStatus_View(searchtextClient, false, false, false, 0, 0, -1, 0, 0, "");
            grdClientSiteStatus.DataSource = Statuscl;
            grdClientSiteStatus.DataBind();
            if (grdClientSiteStatus.Rows.Count > 0)
            {
                var Status = dc.ClientSiteStatus_View(searchtextClient, false, false, false, 0, 0, -1, 0, 0, "");
                int i = 0;
                foreach (var grd in Status)
                {
                    TextBox txt_Name = (TextBox)grdClientSiteStatus.Rows[i].Cells[1].FindControl("txt_Name");
                    TextBox txt_Address = (TextBox)grdClientSiteStatus.Rows[i].Cells[2].FindControl("txt_Address");
                    TextBox txt_Status = (TextBox)grdClientSiteStatus.Rows[i].Cells[3].FindControl("txt_Status");
                    txt_Name.Text = grd.CL_Name_var.ToString();
                    if (grd.CL_OfficeAddress_var != null && grd.CL_OfficeAddress_var != "")
                    {
                        txt_Address.Text = grd.CL_OfficeAddress_var.ToString();
                    }
                    else
                    {
                        txt_Address.Text = "";
                    }
                    if (Convert.ToBoolean(grd.CL_Status_bit).ToString() == "False")
                    {
                        txt_Status.Text = "Continue";
                    }
                    else
                    {
                        txt_Status.Text = "Discontinue";
                        txt_Status.BackColor = System.Drawing.Color.LightSalmon;
                    }
                    i++;
                }
            }
        }
        public void ClientDisContinue()
        {
            string searchtextClient = "";
            if (txt_ClientName.Text != "")
                searchtextClient = txt_ClientName.Text + "%";
            var Statuscl = dc.ClientSiteStatus_View(searchtextClient, false, false, false, 0, 0, -1, 0, 1, "");
            grdClientSiteStatus.DataSource = Statuscl;
            grdClientSiteStatus.DataBind();
            if (grdClientSiteStatus.Rows.Count > 0)
            {
                var Status = dc.ClientSiteStatus_View(searchtextClient, false, false, false, 0, 0, -1, 0, 1, "");
                int i = 0;
                foreach (var grd in Status)
                {
                    TextBox txt_Name = (TextBox)grdClientSiteStatus.Rows[i].Cells[1].FindControl("txt_Name");
                    TextBox txt_Address = (TextBox)grdClientSiteStatus.Rows[i].Cells[2].FindControl("txt_Address");
                    TextBox txt_Status = (TextBox)grdClientSiteStatus.Rows[i].Cells[3].FindControl("txt_Status");

                    txt_Name.Text = grd.CL_Name_var.ToString();
                    if (grd.CL_OfficeAddress_var != null && grd.CL_OfficeAddress_var != "")
                    {
                        txt_Address.Text = grd.CL_OfficeAddress_var.ToString();
                    }
                    else
                    {
                        txt_Address.Text = "";
                    }
                    if (Convert.ToBoolean(grd.CL_Status_bit).ToString() == "False")
                    {

                        txt_Status.Text = "Continue";
                    }
                    else
                    {
                        txt_Status.Text = "Discontinue";
                        txt_Status.BackColor = System.Drawing.Color.LightSalmon;
                    }
                    i++;
                }
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                bool valid = false;
                for (int i = 0; i < grdClientSiteStatus.Rows.Count; i++)
                {
                    CheckBox cbxSelect = (CheckBox)grdClientSiteStatus.Rows[i].Cells[0].FindControl("cbxSelect");
                    TextBox txt_Name = (TextBox)grdClientSiteStatus.Rows[i].Cells[1].FindControl("txt_Name");
                    TextBox txt_Status = (TextBox)grdClientSiteStatus.Rows[i].Cells[3].FindControl("txt_Status");
                    TextBox txt_Note = (TextBox)grdClientSiteStatus.Rows[i].Cells[4].FindControl("txt_Note");

                    if (cbxSelect.Checked)
                    {
                        int SiteId = int.Parse(txt_Name.CssClass);
                        int ClId = int.Parse(cbxSelect.CssClass);
                        if (RdnSite.Checked)
                        {
                            if (txt_Note.Text != "")
                            {
                                if (txt_Status.Text == "Discontinue")
                                {
                                    dc.ClientSiteStatus_View("", true, false, true, SiteId, 0, 0, 0, 0, txt_Note.Text);
                                    valid = true;
                                }
                            }
                            if (txt_Status.Text == "Continue")
                            {
                                dc.ClientSiteStatus_View("", true, false, false, SiteId, 0, 1, 0, 0, "");
                                valid = true;
                            }
                        }
                        if (RdnClient.Checked)
                        {
                            if (txt_Note.Text != "")
                            {
                                if (txt_Status.Text == "Discontinue")
                                {
                                    dc.ClientSiteStatus_View("", false, true, true, 0, 0, 0, ClId, 0, txt_Note.Text);
                                    valid = true;
                                }
                            }
                            if (txt_Status.Text == "Continue")
                            {
                                dc.ClientSiteStatus_View("", false, true, false, 0, 0, 0, ClId, 1, "");
                                valid = true;
                            }
                        }
                    }
                }
                if (valid == true)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Record updated sucessfully ";
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    BindData();
                }
                else
                {
                    lblMsg.Visible = false;
                }
            }
        }
        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            for (int i = 0; i < grdClientSiteStatus.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdClientSiteStatus.Rows[i].Cells[0].FindControl("cbxSelect");
                TextBox txt_Status = (TextBox)grdClientSiteStatus.Rows[i].Cells[3].FindControl("txt_Status");
                TextBox txt_Note = (TextBox)grdClientSiteStatus.Rows[i].Cells[4].FindControl("txt_Note");
                if (cbxSelect.Checked)
                {
                    if (txt_Status.Text == "Discontinue")
                    {
                        if (txt_Note.Text == "")
                        {
                            lblMsg.Text = "Note is Mondatary for row number " + (i + 1) + ".";
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


        protected void ChkSpecificClient_CheckedChanged(object sender, EventArgs e)
        {

            if (ChkSpecificClient.Checked)
            {
                ChkSpecificClient.Checked = true;
                if (RdnClient.Checked == true)
                {
                    txt_ClientName.Visible = true;
                    ImgBtnSearch.Visible = true;
                    ddl_ClientAndSite.Visible = false;
                }

                if (RdnSite.Checked)
                {
                    txt_ClientName.Visible = false;
                    ImgBtnSearch.Visible = true;
                    ddl_ClientAndSite.Visible = true;
                    LoadClient();
                }
            }
            else
            {
                ChkSpecificClient.Checked = false;
                ddl_ClientAndSite.Visible = false;
                txt_ClientName.Visible = false;
                ImgBtnSearch.Visible = false;
            }


        }

        public void ViewSiteContinue()
        {
            if (Convert.ToInt32(ddl_ClientAndSite.SelectedValue) != 0)
            {
                var Statusc = dc.ClientSiteStatus_View("", false, false, false, 0, Convert.ToInt32(ddl_ClientAndSite.SelectedValue), 0, -1, -1, "");
                grdClientSiteStatus.DataSource = Statusc;
                grdClientSiteStatus.DataBind();
                if (grdClientSiteStatus.Rows.Count > 0)
                {
                    var Status = dc.ClientSiteStatus_View("", false, false, false, 0, Convert.ToInt32(ddl_ClientAndSite.SelectedValue), 0, -1, -1, "");
                    int i = 0;
                    foreach (var grd in Status)
                    {

                        TextBox txt_Name = (TextBox)grdClientSiteStatus.Rows[i].Cells[1].FindControl("txt_Name");
                        TextBox txt_Address = (TextBox)grdClientSiteStatus.Rows[i].Cells[2].FindControl("txt_Address");
                        TextBox txt_Status = (TextBox)grdClientSiteStatus.Rows[i].Cells[3].FindControl("txt_Status");

                        txt_Name.Text = grd.SITE_Name_var.ToString();
                        if (grd.SITE_Address_var != null && grd.SITE_Address_var != "")
                        {
                            txt_Address.Text = grd.SITE_Address_var.ToString();
                        }
                        else
                        {
                            txt_Address.Text = "";
                        }
                        if (grd.SITE_Status_bit.ToString() == "False")
                        {

                            txt_Status.Text = "Continue";
                        }
                        else
                        {
                            txt_Status.Text = "Discontinue";
                            txt_Status.BackColor = System.Drawing.Color.LightSalmon;
                        }
                        i++;
                    }
                }
            }
            else
            {


            }

        }

        public void ViewSiteDisContinue()
        {
            if (Convert.ToInt32(ddl_ClientAndSite.SelectedValue) != 0)
            {
                var Statusc = dc.ClientSiteStatus_View("", false, false, false, 0, Convert.ToInt32(ddl_ClientAndSite.SelectedValue), 1, 0, -1, "");
                grdClientSiteStatus.DataSource = Statusc;
                grdClientSiteStatus.DataBind();
                if (grdClientSiteStatus.Rows.Count > 0)
                {
                    var Status = dc.ClientSiteStatus_View("", false, false, false, 0, Convert.ToInt32(ddl_ClientAndSite.SelectedValue), 1, -1, -1, "");// dc.sp_SiteStatus_View(0, 0, 1, "", false, false, false, "");
                    int i = 0;
                    foreach (var grd in Status)
                    {
                        TextBox txt_Name = (TextBox)grdClientSiteStatus.Rows[i].Cells[1].FindControl("txt_Name");
                        TextBox txt_Address = (TextBox)grdClientSiteStatus.Rows[i].Cells[2].FindControl("txt_Address");
                        TextBox txt_Status = (TextBox)grdClientSiteStatus.Rows[i].Cells[3].FindControl("txt_Status");

                        txt_Name.Text = grd.SITE_Name_var.ToString();
                        if (grd.SITE_Address_var != null && grd.SITE_Address_var != "")
                        {
                            txt_Address.Text = grd.SITE_Address_var.ToString();
                        }
                        else
                        {
                            txt_Address.Text = "";
                        }
                        if (grd.SITE_Status_bit.ToString() == "False")
                        {

                            txt_Status.Text = "Continue";
                        }
                        else
                        {
                            txt_Status.Text = "Discontinue";
                            txt_Status.BackColor = System.Drawing.Color.LightSalmon;
                        }
                        i++;
                    }
                }
            }


        }
        public void LoadClient()
        {
            var allClent = dc.Client_View(0, -1, "", "");
            ddl_ClientAndSite.DataSource = allClent;
            ddl_ClientAndSite.DataTextField = "CL_Name_var";
            ddl_ClientAndSite.DataValueField = "CL_Id";
            ddl_ClientAndSite.DataBind();
            ddl_ClientAndSite.Items.Insert(0, new ListItem("---Select---", "0"));
        }


        protected void ddl_ClientAndSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RdnSite.Checked)
            {
                if (RdnContinued.Checked)
                {
                    ViewSiteContinue();
                }
                else if (RdnDiscontinued.Checked)
                {
                    ViewSiteDisContinue();
                }
                else
                {
                    ClientwiseAllSiteGrid();
                }
            }
        }
        public void ClientwiseAllSiteGrid()
        {
            if (Convert.ToInt32(ddl_ClientAndSite.SelectedValue) != 0)
            {
                var Statusc = dc.ClientSiteStatus_View("", false, false, false, 0, Convert.ToInt32(ddl_ClientAndSite.SelectedValue), -1, 0, -1, "");
                grdClientSiteStatus.DataSource = Statusc;
                grdClientSiteStatus.DataBind();
                if (grdClientSiteStatus.Rows.Count > 0)
                {
                    var Status = dc.ClientSiteStatus_View("", false, false, false, 0, Convert.ToInt32(ddl_ClientAndSite.SelectedValue), -1, 0, -1, ""); //dc.sp_SiteStatus_View(0, Convert.ToInt32(ddl_ClientAndSite.SelectedValue), -1, "", false, false, false, "");
                    int i = 0;
                    foreach (var grd in Status)
                    {

                        TextBox txt_Name = (TextBox)grdClientSiteStatus.Rows[i].Cells[1].FindControl("txt_Name");
                        TextBox txt_Address = (TextBox)grdClientSiteStatus.Rows[i].Cells[2].FindControl("txt_Address");
                        TextBox txt_Status = (TextBox)grdClientSiteStatus.Rows[i].Cells[3].FindControl("txt_Status");

                        txt_Name.Text = grd.SITE_Name_var.ToString();
                        if (grd.SITE_Address_var != null && grd.SITE_Address_var != "")
                        {
                            txt_Address.Text = grd.SITE_Address_var.ToString();
                        }
                        else
                        {
                            txt_Address.Text = "";
                        }
                        if (grd.SITE_Status_bit.ToString() == "False")
                        {

                            txt_Status.Text = "Continue";
                        }
                        else
                        {
                            txt_Status.Text = "Discontinue";
                            txt_Status.BackColor = System.Drawing.Color.LightSalmon;
                        }
                        i++;
                    }
                }
            }


        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdClientSiteStatus.Rows.Count > 0 && grdClientSiteStatus.Visible == true)
            {
                string header = "";
                string Subheader = "";
                if (RdnClient.Checked == true)
                    header = "Client Status";
                else
                {
                    header = "Site Status";
                    Subheader = "Client : " + ddl_ClientAndSite.SelectedItem.Text;  
                }
                //Subheader = "" + "|" + lblBilldt.Text + "|" + txtFromDate.Text + " - " + txtToDate.Text + "|" + lblInwdtype.Text + "|" + ddl_InwardTestType.SelectedItem.Text;
                string grddata = "";
                string grdColumn = "";
                string grdview = "";
                PrintHTMLReport rpt = new PrintHTMLReport();
                grdColumn += "Sr. No." + "|";
                grdColumn += "Name" + "|";
                grdColumn += "Address" + "|";
                grdColumn += "Status" + "|";
                for (int i = 0; i < grdClientSiteStatus.Rows.Count; i++)
                {
                    TextBox txt_Name = (TextBox)grdClientSiteStatus.Rows[i].Cells[1].FindControl("txt_Name");
                    TextBox txt_Address = (TextBox)grdClientSiteStatus.Rows[i].Cells[2].FindControl("txt_Address");
                    TextBox txt_Status = (TextBox)grdClientSiteStatus.Rows[i].Cells[3].FindControl("txt_Status");
                    grddata += "$";
                    grddata += (i +1) + "~";
                    grddata += txt_Name.Text + "~";
                    grddata += txt_Address.Text + "~";
                    grddata += txt_Status.Text + "~";
                }
                grdview = grdColumn + grddata;
                rpt.RptHTMLgrid_ClientSiteStatus(header, Subheader, grdview);

            }

        }

    }
}
