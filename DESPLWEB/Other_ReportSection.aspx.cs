using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Other_ReportSection : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Other Report Section - Updation";
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                LoadInwardType();
            }
        }

        private void LoadInwardType()
        {
            var inwd = dc.Material_View("", "");
            ddlSection.DataTextField = "MATERIAL_Name_var";
            ddlSection.DataValueField = "MATERIAL_RecordType_var";
            ddlSection.DataSource = inwd;
            ddlSection.DataBind();
            ddlSection.Items.Insert(ddlSection.Items.IndexOf(ddlSection.Items.FindByValue("FLYASH")) + 1, new ListItem("Fly Ash Chemical Testing", "FLYASHCH"));
            ddlSection.Items.Insert(ddlSection.Items.IndexOf(ddlSection.Items.FindByValue("TILE")) + 1, new ListItem("Tile Chemical Testing", "TILECH"));
            ddlSection.Items.Insert(0, "---Select---");
            ddlSection.Items.Remove(ddlSection.Items.FindByValue("OT"));
            ddlSection.Items.Add(new ListItem("Other Testing", "OT"));
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            ddlSection.SelectedIndex = 0;
            DisplayReportList();
        }

        public void DisplayReportList()
        {
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            var Inward = dc.OtherInward_View(Fromdate, Todate).ToList();
            ddlReferenceNo.DataTextField = "OTINWD_ReferenceNo_var";
            ddlReferenceNo.DataSource = Inward;
            ddlReferenceNo.DataBind();
            if (ddlReferenceNo.Items.Count > 0)
                ddlReferenceNo.Items.Insert(0, new ListItem("---Select---", ""));
        }

        protected void ClearAllControls()
        {
            ddlReferenceNo.Items.Clear();
            ddlSection.SelectedIndex = 0;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
        }

        protected void ddlReferenceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSection.SelectedIndex = 0;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (ddlReferenceNo.SelectedIndex > 0)
            {
                var wInwd = dc.ReportStatus_View("Other Testing", null, null, 1, 0, 0, ddlReferenceNo.SelectedItem.Text, 0, 0, 0);
                foreach (var w in wInwd)
                {
                    if (w.OTINWD_Section_var != null && w.OTINWD_Section_var != "")
                        ddlSection.SelectedValue = w.OTINWD_Section_var;
                }
            }
        }
                
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;

            if (ddlReferenceNo.Items.Count == 0 || ddlReferenceNo.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select Reference No.";
                ddlReferenceNo.Focus();
            }
            else if (ddlSection.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select Section.";
                ddlSection.Focus();
            }
            else
            {
                dc.OtherInward_Update_Section(ddlReferenceNo.SelectedItem.Text, ddlSection.SelectedItem.Value);
                lblMsg.Text = "Updated Successfully.";
            }
            lblMsg.Visible = true;

        }
        
        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            ClearAllControls();
            DisplayReportList();
        }

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            ClearAllControls();
            DisplayReportList();
        }
   
    }

}