using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class MFMaterialUpdation : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "MF Material Updation";
                txtFromDate.Text = DateTime.Today.AddDays(-120).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                LoadMaterialList();
                LoadReferenceNoList();
            }
        }
        private void LoadReferenceNoList()
        {
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);            
            var MFlist = dc.MaterialDetail_View(0, "", 0, "", Fromdate, Todate, "");
            ddlReferenceNo.DataValueField = "MaterialDetail_RefNo";
            //ddlReferenceNo.DataValueField = "MaterialDetail_RecordNo";
            ddlReferenceNo.DataSource = MFlist;
            ddlReferenceNo.DataBind();
            ddlReferenceNo.Items.Insert(0, new ListItem("---Select---", "0")); 
        }
        private void LoadExReferenceNoList()
        {   
            var MFlist = dc.MaterialDetail_View(0, "", 0, "", null, null, ddlReferenceNo.SelectedValue);
            ddlExReferenceNo.DataValueField = "MaterialDetail_RefNo";
            ddlExReferenceNo.DataSource = MFlist;
            ddlExReferenceNo.DataBind();
            ddlExReferenceNo.Items.Insert(0, new ListItem("---Select---", "0"));
            ListItem itemToRemove = ddlExReferenceNo.Items.FindByText(ddlReferenceNo.SelectedItem.Text);
            ddlExReferenceNo.Items.Remove(itemToRemove);
        }
        private void LoadMaterialList()
        {
            var Mat = dc.MaterialListView("", "", "");
            ddlNewMaterial.DataSource = Mat;
            ddlNewMaterial.DataTextField = "Material_List";
            ddlNewMaterial.DataValueField = "Material_Id";
            ddlNewMaterial.DataBind();
            ddlNewMaterial.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        private void LoadMFMaterialList()
        {
            var Mat = dc.MaterialListView(ddlReferenceNo.SelectedValue, "", "");
            ddlMaterial.DataSource = Mat;
            ddlMaterial.DataTextField = "Material_List";
            ddlMaterial.DataValueField = "Material_Id";
            ddlMaterial.DataBind();
            ddlMaterial.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        private void LoadExMFMaterialList()
        {
            var Mat = dc.MaterialListView(ddlExReferenceNo.SelectedValue , "", "");
            ddlExMaterial.DataSource = Mat;
            ddlExMaterial.DataTextField = "Material_List";
            ddlExMaterial.DataValueField = "Material_Id";
            ddlExMaterial.DataBind();
            ddlExMaterial.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            ClearAllControls();
            LoadReferenceNoList();
        }
        private void ClearAllControls()
        {
            ddlMaterial.Items.Clear();
            ddlReferenceNo.Items.Clear();
            ddlExReferenceNo.Items.Clear();
            ddlExMaterial.Items.Clear();
            txtInformation.Text = "";
            txtQuantity.Text = "";
            txtDescription.Text = "";
        }
        protected void lnkNotToUseMaterial_Click(object sender, EventArgs e)
        {
            bool flgValid = true;
            string errMsg = "";
            if (ddlReferenceNo.Items.Count == 0 || ddlReferenceNo.SelectedIndex <= 0)
            {
                flgValid = false;
                errMsg = "Select Reference No.";
            }
            else if (ddlMaterial.Items.Count == 0 || ddlMaterial.SelectedIndex <= 0)
            {
                flgValid = false;
                errMsg = "Select Material.";
            }
            if (flgValid == true)
            {
                dc.MaterialDetail_Update(Convert.ToInt32(ddlMaterial.SelectedValue), ddlReferenceNo.SelectedValue, "", 0, 0, 0, null, true);

                LoadMFMaterialList();
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Material deleted successfully.";
                lblMsg.Visible = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + errMsg + "');", true);
            }
        }

        protected void lnkUseMaterial_Click(object sender, EventArgs e)
        {
            bool flgValid = true;
            string errMsg = "";
            if (ddlReferenceNo.Items.Count == 0 || ddlReferenceNo.SelectedIndex <= 0)
            {
                flgValid = false;
                errMsg = "Select Reference No.";
            }
            else if (ddlExReferenceNo.Items.Count == 0 || ddlExReferenceNo.SelectedIndex <= 0)
            {
                flgValid = false;
                errMsg = "Select Existing Reference No.";
            }
            else if (ddlExMaterial.Items.Count == 0 || ddlExMaterial.SelectedIndex <= 0)
            {
                flgValid = false;
                errMsg = "Select Existing Material.";
            }
            else if (txtInformation.Text.Trim() == "")
            {
                flgValid = false;
                errMsg = "Enter Information.";
                txtInformation.Focus();
            }
            else if (txtQuantity.Text.Trim() == "")
            {
                flgValid = false;
                errMsg = "Enter Quanity.";
                txtQuantity.Focus();
            }
            else
            {
                for (int i = 0; i < ddlMaterial.Items.Count; i++)
                {
                    if (ddlExMaterial.SelectedValue == ddlMaterial.Items[i].Value)
                    {
                        flgValid = false;
                        errMsg = "Material already exist.";
                        break;
                    }
                }
            }

            if (flgValid == true)
            {
                int recordNo = 0, srNo = ddlMaterial.Items.Count + 1;
                DateTime dtReceived = DateTime.Now;
                var inwd = dc.AllInward_View("MF", 0, ddlReferenceNo.SelectedValue);
                foreach (var mf in inwd)
                {
                    dtReceived = Convert.ToDateTime(mf.MFINWD_ReceivedDate_dt);
                    recordNo = Convert.ToInt32(mf.MFINWD_RecordNo_int);
                }
                dc.MaterialDetail_Update(Convert.ToInt32(ddlExMaterial.SelectedValue), ddlReferenceNo.SelectedValue, txtInformation.Text, Convert.ToDecimal(txtQuantity.Text), srNo, recordNo, dtReceived, false);
                LoadMFMaterialList();

                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully.";
                lblMsg.Visible = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + errMsg + "');", true);
            }
        }

        protected void lnkAddMaterial_Click(object sender, EventArgs e)
        {
            bool flgValid = true;
            string errMsg = "";
            if (ddlReferenceNo.Items.Count == 0 || ddlReferenceNo.SelectedIndex <= 0)
            {
                flgValid = false;
                errMsg = "Select Reference No.";
            }
            else if (ddlNewMaterial.Items.Count == 0 || ddlNewMaterial.SelectedIndex <= 0)
            {
                flgValid = false;
                errMsg = "Select New Material.";
            }
            else if (txtInformation.Text.Trim() == "")
            {
                flgValid = false;
                errMsg = "Enter Information.";
                txtInformation.Focus();
            }
            else if (txtQuantity.Text.Trim() == "")
            {
                flgValid = false;
                errMsg = "Enter Quanity.";
                txtQuantity.Focus();
            }
            else
            {
                for (int i = 0; i < ddlMaterial.Items.Count; i++)
                {
                    if (ddlNewMaterial.SelectedValue == ddlMaterial.Items[i].Value)
                    {
                        flgValid = false;
                        errMsg = "Material already exist.";
                        break;
                    }
                }
            }
            if (flgValid == true)
            {
                int recordNo = 0, srNo = ddlMaterial.Items.Count + 1;
                DateTime dtReceived = DateTime.Now;
                var inwd = dc.AllInward_View("MF", 0, ddlReferenceNo.SelectedValue);
                foreach (var mf in inwd)
                {
                    dtReceived = Convert.ToDateTime(mf.MFINWD_ReceivedDate_dt);
                    recordNo = Convert.ToInt32(mf.MFINWD_RecordNo_int);
                }
                dc.MaterialDetail_Update(Convert.ToInt32(ddlNewMaterial.SelectedValue), ddlReferenceNo.SelectedValue, txtInformation.Text, Convert.ToDecimal(txtQuantity.Text), srNo, recordNo, dtReceived, false);
                LoadMFMaterialList();
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully.";
                lblMsg.Visible = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + errMsg + "');", true);
            }
        }

        protected void ddlReferenceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDescription.Text = "";
            ddlMaterial.Items.Clear();
            ddlExReferenceNo.Items.Clear();
            ddlExMaterial.Items.Clear(); 
            if (ddlReferenceNo.SelectedIndex > 0)
            {
                LoadMFMaterialList();
                LoadExReferenceNoList();
                var mfinward = dc.MixDesign_View(ddlReferenceNo.SelectedValue, 1);
                foreach (var mfinwd in mfinward)
                {
                    if (mfinwd.MFINWD_Description_var != null)
                        txtDescription.Text = mfinwd.MFINWD_Description_var;
                    break;
                }
            }
        }

        protected void ddlExReferenceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlExMaterial.Items.Clear();
            if (ddlExReferenceNo.SelectedIndex > 0)
            {
                LoadExMFMaterialList();
            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void lnkUpdateAliasName_Click(object sender, EventArgs e)
        {
            bool flgValid = true;
            string errMsg = "";
            if (ddlReferenceNo.Items.Count == 0 || ddlReferenceNo.SelectedIndex <= 0)
            {
                flgValid = false;
                errMsg = "Select Reference No.";
            }
            else if (ddlMaterial.Items.Count == 0 || ddlMaterial.SelectedIndex <= 0)
            {
                flgValid = false;
                errMsg = "Select Material.";
            }
            else if (txtMaterialAliasName.Text.Trim() == "")
            {
                flgValid = false;
                errMsg = "Enter Alias Name of Material.";
            }
            if (flgValid == true)
            {
                dc.MaterialAlias_Update(Convert.ToInt32(ddlMaterial.SelectedValue), ddlReferenceNo.SelectedValue, txtMaterialAliasName.Text.Trim());

                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Material Alias Name successfully.";
                lblMsg.Visible = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + errMsg + "');", true);
            }
        
        }

        protected void lnkUpdateDescription_Click(object sender, EventArgs e)
        {

            bool flgValid = true;
            string errMsg = "";
            if (ddlReferenceNo.Items.Count == 0 || ddlReferenceNo.SelectedIndex <= 0)
            {
                flgValid = false;
                errMsg = "Select Reference No.";
            }
            else if (txtDescription.Text.Trim() == "")
            {
                flgValid = false;
                errMsg = "Enter Description.";
                txtDescription.Focus();
            }
            if (flgValid == true)
            {
                string referenceNo = ddlReferenceNo.SelectedValue.Split('/')[0];

                dc.MixDesignInward_Update_Description(referenceNo + "/%", 0, "", txtDescription.Text.Trim());
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Description updated successfully.";
                lblMsg.Visible = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + errMsg + "');", true);
            }

        }
    }
}