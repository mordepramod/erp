using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class SectionWiseEmail : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               // ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "SectionWise Email Setting";
                bool userRight = false;
                LoadMaterialDetails();
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                    foreach (var u in user)
                    {
                        if (u.USER_SuperAdmin_right_bit == true)
                            userRight = true;
                    }
                    if (userRight == false)
                    {
                        pnlContent.Visible = false;
                        lblAccess.Visible = true;
                        lblAccess.Text = "Access is Denied.. ";
                    }

                }
            }
        }

        private void LoadMaterialDetails()
        {
            grdSectionEmail.DataSource = null;
            grdSectionEmail.DataBind();

            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("Material_Id", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Material_Name_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtEmailId", typeof(string)));
            var emailCnt = dc.Material_View("", "").ToList();
            if (emailCnt.Count > 0)
            {
                foreach (var alrt in emailCnt)
                {
                    drRow = dtTable.NewRow();
                    drRow["Material_Id"] = alrt.MATERIAL_Id.ToString();
                    drRow["Material_Name_var"] = alrt.MATERIAL_Name_var.ToString();
                    drRow["txtEmailId"] = Convert.ToString(alrt.MATERIAL_MailId_var);
                    dtTable.Rows.Add(drRow);
                }
            }
            
            grdSectionEmail.DataSource = dtTable;
            grdSectionEmail.DataBind();

            for (int i = 0; i < grdSectionEmail.Rows.Count; i++)
            {
                Label lblMaterial_Id = (Label)grdSectionEmail.Rows[i].FindControl("lblMaterial_Id");
                TextBox txtEmailId = (TextBox)grdSectionEmail.Rows[i].FindControl("txtEmailId");
                lblMaterial_Id.Text = dtTable.Rows[i]["Material_Id"].ToString();
                txtEmailId.Text = dtTable.Rows[i]["txtEmailId"].ToString();

            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            //string matId = "";
            for (int i = 0; i < grdSectionEmail.Rows.Count; i++)
            {
                Label lblMaterial_Id = (Label)grdSectionEmail.Rows[i].FindControl("lblMaterial_Id");
                TextBox txtEmailId = (TextBox)grdSectionEmail.Rows[i].FindControl("txtEmailId");
                if (lblMaterial_Id.Text != "" && txtEmailId.Text != "")
                {
                    //update email against that material
                    dc.Material_Update_SectionWiseEmail(Convert.ToInt32(lblMaterial_Id.Text), txtEmailId.Text);
                }
             
            }            
            lblMessage.Text = "Updated Successfully";
            lblMessage.Visible = true;

            LoadMaterialDetails();
        }


    }
}