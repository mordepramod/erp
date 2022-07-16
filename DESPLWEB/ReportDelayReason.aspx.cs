using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class ReportDelayReason : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Add Report Delay Reason";

                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                LoadInwardType();
            }
        }
        private void LoadInwardType()
        {
            var inwd = dc.Material_View("", "");
            ddlRecordType.DataTextField = "MATERIAL_RecordType_var";
            //ddlRecordType.DataValueField = "MATERIAL_Id";
            ddlRecordType.DataSource = inwd;
            ddlRecordType.DataBind();
            ddlRecordType.Items.Insert(0, new ListItem("---Select---",""));
        }
        private void ClearAllControls()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            ddlRecordType.SelectedValue = "";
            txtReason.Text = "";
            txtReferenceNoPart1.Text = "";
            txtReferenceNoPart2.Text = "";
            txtReferenceNoPart3.Text = "";
            lblReasonMessage.Visible = false;
            lnkSave.Enabled = true;
        }
        protected void lnkClear_Click(object sender, EventArgs e)
        {
            ClearAllControls();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                lblReasonMessage.Visible = false;
                txtReason.Text = txtReason.Text.Trim();
                txtReferenceNoPart1.Text = txtReferenceNoPart1.Text.Trim();
                txtReferenceNoPart2.Text = txtReferenceNoPart2.Text.Trim();
                txtReferenceNoPart3.Text = txtReferenceNoPart3.Text.Trim();
                bool addFlag = true;

                string strRefNo = txtReferenceNoPart1.Text.Trim() + "/" + txtReferenceNoPart2.Text.Trim() + "-" + txtReferenceNoPart3.Text.Trim();
                if (ddlRecordType.SelectedIndex <= 0)
                {
                    lblReasonMessage.Text = "Select Record Type.";
                    lblReasonMessage.Visible = true;
                    ddlRecordType.Focus();
                    addFlag = false;
                }
                else if (txtReferenceNoPart1.Text.Trim() == "")
                {
                    lblReasonMessage.Text = "Please enter reference no.";
                    lblReasonMessage.Visible = true;
                    txtReferenceNoPart1.Focus();
                    addFlag = false;
                }
                else if (txtReferenceNoPart2.Text.Trim() == "")
                {
                    lblReasonMessage.Text = "Please enter reference no.";
                    lblReasonMessage.Visible = true;
                    txtReferenceNoPart2.Focus();
                    addFlag = false;
                }
                else if (txtReferenceNoPart3.Text.Trim() == "")
                {
                    lblReasonMessage.Text = "Please enter reference no.";
                    lblReasonMessage.Visible = true;
                    txtReferenceNoPart3.Focus();
                    addFlag = false;
                }
                else if (txtReason.Text.Trim() == "")
                {
                    lblReasonMessage.Text = "Please enter reason for report delay.";
                    lblReasonMessage.Visible = true;
                    txtReason.Focus();
                    addFlag = false;
                }
                else
                {
                    var mis = dc.MISDetail_View_ReferenceNo(ddlRecordType.SelectedValue, strRefNo).ToList();
                    if (mis.Count() == 0)
                    {
                        lblReasonMessage.Text = "Reference no. does not exist";
                        lblReasonMessage.Visible = true;
                        txtReferenceNoPart1.Focus();
                        addFlag = false;
                    }
                }
                if (addFlag == true)
                {
                    dc.ReportDelayReason_Update(ddlRecordType.SelectedValue, strRefNo, txtReason.Text, Convert.ToInt32(Session["LoginId"].ToString()));

                    lblReasonMessage.Text = "Updated Successfully";
                    lblReasonMessage.ForeColor = System.Drawing.Color.Green;
                    lblReasonMessage.Visible = true;
                    
                }
            }
        }
        
        
    }
}