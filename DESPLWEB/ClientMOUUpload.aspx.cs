using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace DESPLWEB
{
    public partial class ClientMOUUpload : System.Web.UI.Page
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
                lblheading.Text = "Client - Upload MOU";
                txtSearch.Attributes.Add("onkeypress", "return clickButton(event,'" + ImgBtnSearch.ClientID + "')");
                Session["Cl_Id"] = 0;
                FirstGridViewRowOfClient();
            }
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
            dt.Columns.Add(new DataColumn("CL_MOUFileName_var", typeof(string)));
            dr = dt.NewRow();
            dr["CL_Id"] = 0;
            dr["CL_Name_var"] = string.Empty;
            dr["CL_MOUFileName_var"] = string.Empty;
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

        protected void grdClient_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int RowIndex = gvr.RowIndex;
            string clientId = Convert.ToString(e.CommandArgument);

            if (e.CommandName == "lnkUploadMOU")
            {
                FileUpload FileUploadPO = (FileUpload)grdClient.Rows[RowIndex].FindControl("FileUploadMOU");
                if (FileUploadPO.HasFile == false)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('No file available..');", true);
                }
                else if (FileUploadPO.HasFile == true)
                {
                    string filename = "";
                    string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                    filename = Path.GetFileName(FileUploadPO.PostedFile.FileName);
                    string ext = Path.GetExtension(filename);
                    string filePath = "D:/MOUFiles/";
                    if (cnStr.ToLower().Contains("mumbai") == true)
                        filePath += "Mumbai/";
                    else if (cnStr.ToLower().Contains("nashik") == true)
                        filePath += "Nashik/";
                    else if (cnStr.ToLower().Contains("metro") == true)
                        filePath += "Metro/";
                    else
                        filePath += "Pune/";
                    if (!Directory.Exists(@filePath))
                        Directory.CreateDirectory(@filePath);
                    filePath += Path.GetFileName(filename);
                    FileUploadPO.PostedFile.SaveAs(filePath);

                    dc.Client_Update_MOUFileName(Convert.ToInt32(clientId), filename);
                    LoadClientList();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Uploaded Successfully !');", true);
                }
            }
            else if (e.CommandName == "DownloadFile")
            {
                string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                string filePath = "D:/MOUFiles/";
                if (cnStr.ToLower().Contains("mumbai") == true)
                    filePath += "Mumbai/";
                else if (cnStr.ToLower().Contains("nashik") == true)
                    filePath += "Nashik/";
                else if (cnStr.ToLower().Contains("metro") == true)
                    filePath += "Metro/";
                else
                    filePath += "Pune/";

                filePath += clientId;
                if (File.Exists(@filePath))
                {
                    HttpResponse res = HttpContext.Current.Response;
                    res.Clear();
                    res.AppendHeader("content-disposition", "attachment; filename=" + filePath);
                    res.ContentType = "application/octet-stream";
                    res.WriteFile(filePath);
                    res.Flush();
                    res.End();
                }
            }

        }
        
        
    }
}
