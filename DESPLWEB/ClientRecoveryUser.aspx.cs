using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ClientRecoveryUser : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Client - Recovery User Allocation";
                txtSearch.Attributes.Add("onkeypress", "return clickButton(event,'" + ImgBtnSearch.ClientID + "')");
                Session["Cl_Id"] = 0;
                FirstGridViewRowOfClient();
                LoadUserList();
            }
        }
        private void LoadUserList()
        {
            ddlRecoveryUser.DataTextField = "USER_Name_var";
            ddlRecoveryUser.DataValueField = "USER_Id";
            var users = dc.User_View(0, 0, "", "", "");
            ddlRecoveryUser.DataSource = users;
            ddlRecoveryUser.DataBind();
            ddlRecoveryUser.Items.Insert(0, new ListItem("NA", "0"));
            ddlRecoveryUser.Items.Insert(0, new ListItem("---Select---", "-1"));
        }
        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            if (ChkClientName(txtSearch.Text) == true)
            {
                if (txtSearch.Text != "")
                {
                    lblClientId.Text = Request.Form[hfClientId.UniqueID];
                }
                else
                {
                    lblClientId.Text = "0";
                }
            }
        }
        protected Boolean ChkClientName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txtSearch.Text;
            Boolean valid = false;
            var query = dc.Client_View(0, 0, searchHead, "");
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txtSearch.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Client is not in the list";
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetClientname(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Client_View_Search(searchHead, 0);
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("CL_Name_var");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string[] item = rowObj.CL_Name_var.Split(' ');
                // string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                for (int i = 0; i < item.Length; i++)
                {
                    dt.Rows.Add(item[i]);
                }

            }
            if (row == null)
            {
                var clnm = db.Client_View(0, 0, "", "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    // string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    string[] item = rowObj.CL_Name_var.Split(' ');
                    // string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    for (int i = 0; i < item.Length; i++)
                    {
                        dt.Rows.Add(item[i]);
                    }
                    //  dt.Rows.Add(item);
                }
            }
            List<string> CL_Name_var = new List<string>();

            DataTable dt1 = new DataTable();
            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "CL_Name_var");
            if (distinctValues.Select("CL_Name_var LIKE '" + searchHead + "%'").Length != 0)
                dt1 = distinctValues.Select("CL_Name_var LIKE '" + searchHead + "%'").CopyToDataTable();
            //else
            //    dt1 = distinctValues.Copy();

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                CL_Name_var.Add(dt1.Rows[i][0].ToString());
            }
            return CL_Name_var;

        }
        
        private void LoadClientList()
        {
            string searchHead = "";
            if (txtSearch.Text != "")
                searchHead = txtSearch.Text + "%";
            var cl = dc.Client_View_Search(searchHead, 0);
            grdClient.DataSource = cl;
            grdClient.DataBind();
            lblClient.Visible = true;
            if (grdClient.Rows.Count <= 0)
                FirstGridViewRowOfClient();
        }
                
        private void FirstGridViewRowOfClient()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("RecoveryUserName", typeof(string)));
            dr = dt.NewRow();
            dr["CL_Id"] = 0;
            dr["CL_Name_var"] = string.Empty;
            dr["RecoveryUserName"] = string.Empty;
            dt.Rows.Add(dr);

            grdClient.DataSource = dt;
            grdClient.DataBind();

        }
        
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;

            if (txtSearch.Text != "")
            {
                LoadClientList();
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Please Enter the Client Name";
            }

        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            string strMsg = "";            
            if (ddlRecoveryUser.SelectedIndex == 0)
            {
                ddlRecoveryUser.Focus();
                strMsg = "Select user to be allocate";
            }
            else
            {
                bool clientSelected = false;
                for (int i = 0; i < grdClient.Rows.Count; i++)
                {
                    CheckBox cbxSelect = (CheckBox)grdClient.Rows[i].FindControl("cbxSelect");
                    Label lblClientId = (Label)grdClient.Rows[i].FindControl("lblClientId");

                    if (cbxSelect.Checked == true && lblClientId.Text != "0")
                    {
                        clientSelected = true;
                        dc.Client_Update_RecoveryUser(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(ddlRecoveryUser.SelectedValue));
                    }
                }
                if (clientSelected == false)
                {
                    strMsg = "Select client from list.";
                }
                else
                {
                    strMsg = "Saved successfully.";
                    LoadClientList();
                }
            }
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
        }
    }
}
