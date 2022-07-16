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
using System.Text;
using System.Collections.Specialized;
using System.Data.OleDb;
using System.Globalization;

namespace DESPLWEB
{
    public partial class ClientAssignment : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (lblheading != null)
                {
                    lblheading.Text = "Client Assignment Updation";
                }
                
                lblClient.Text = "ALL";
                grdClientAssignmentBind();
                bindDDLSelectedClient();

            }

        }
        protected void grdClientAssignmentBind()
        {
            var a = dc.ClientAssignment_View(lblClient.Text);
            grdClientAssignment.DataSource = a;
            grdClientAssignment.DataBind();

            this.bindDDLSelectedClient();

        }

        public void bindDDLSelectedClient()
        {
           // var a = dc.getDDLSelectedClient();

           // DDLSelectedClient.DataSource = a;
            DDLSelectedClient.DataTextField = "CL_Name_var";
            DDLSelectedClient.DataValueField = "CL_Name_var";
            DDLSelectedClient.DataBind();

            DDLSelectedClient.Items.Insert(0, new ListItem("ALL", "ALL"));
            //DDLSelectedClient.ClearSelection();

            DDLSelectedClient.Items.FindByValue(lblClient.Text).Selected = true;
        }

        protected void grdClientAssignment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdClientAssignment.EditIndex = e.NewEditIndex;
            grdClientAssignmentBind();

        }

        protected void grdClientAssignment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdClientAssignment.EditIndex = -1;
            grdClientAssignmentBind();
        }

        protected void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && grdClientAssignment.EditIndex == e.Row.RowIndex)
            {
                //Binding DDL Category
                DropDownList DdlCategoryDesc = (DropDownList)e.Row.FindControl("DdlCategoryDesc");
                var Category = dc.getCategory();

                DdlCategoryDesc.DataSource = Category;
                DdlCategoryDesc.DataTextField = "CategoryDescription_var";
                DdlCategoryDesc.DataValueField = "CategoryId_int";
                DdlCategoryDesc.DataBind();

                DdlCategoryDesc.Items.Insert(0, new ListItem("--Select--", "0"));
                Label lblCategoryDesc = (e.Row.FindControl("lblCategoryDesc") as Label);
                DdlCategoryDesc.Items.FindByText(lblCategoryDesc.Text).Selected = true;

            }
        }

        protected void grdClientAssignment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label lblClientName = (Label)grdClientAssignment.Rows[e.RowIndex].FindControl("lblClientName");

            TextBox txtCreditLimit = (TextBox)grdClientAssignment.Rows[e.RowIndex].FindControl("txtCreditLimit");
            DropDownList DdlCategoryDesc = (DropDownList)grdClientAssignment.Rows[e.RowIndex].FindControl("DdlCategoryDesc");

            dc.ClientAssignment_Update(lblClientName.Text, txtCreditLimit.Text, DdlCategoryDesc.Text);

            grdClientAssignment.EditIndex = -1;
            grdClientAssignmentBind();
        }

        protected void DDLSelectedClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList DDLSelectedClient = (DropDownList)sender;

            lblClient.Text = DDLSelectedClient.SelectedValue;
            this.grdClientAssignmentBind();

        }
              

    }
}
