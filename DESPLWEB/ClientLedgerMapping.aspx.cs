using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections;


namespace DESPLWEB
{
    public partial class ClientLedgerMapping : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        Dictionary<string, int> Client = new Dictionary<string, int>();
        public static List<Tuple<string, int>> arrClient =new List<Tuple<string,int>>() ;
        public static DataTable dtClient = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                bool userRight = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_Account_right_bit == true)
                        userRight = true;
                }
                if (userRight == true)
                {
                    Label lblheading = (Label)Master.FindControl("lblheading");
                    lblheading.Text = "Client - Ledger Mapping";

                    //ddl_List.SelectedIndex = 1;
                    //LoadClientLedgerMappingListNew(Convert.ToInt32(ddl_List.SelectedValue.ToString()));
                }
                else
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            if (lblClientId.Text == "" || lblClientId.Text == "0")
            {
                strMsg = "Select client from list.";
                txt_Client.Focus();
            }
            else if (lblLedgerId.Text == "" || lblLedgerId.Text == "0")
            {
                strMsg = "Select ledger name from list.";
                txt_Ledger.Focus();
            }
            else
            {
                for (int i = 0; i < grdMapping.Rows.Count; i++)
                {
                    Label CLLedger_CL_Id = (Label)grdMapping.Rows[i].FindControl("CLLedger_CL_Id");
                    Label CLLedger_Ledger_Id = (Label)grdMapping.Rows[i].FindControl("CLLedger_Ledger_Id");
                    if (lblClientId.Text == CLLedger_CL_Id.Text && lblLedgerId.Text == CLLedger_Ledger_Id.Text)
                    {
                        strMsg = "Client Ledger mapping already exist.";
                    }
                }
            }
            if (strMsg == "")
            {
                dc.ClientLedger_Update(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblLedgerId.Text), false);
                strMsg = "Client Ledger mapping saved successfully.";
                LoadClientLedgerMappingList();
            }

            if (strMsg != "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
            }
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            clsData obj = new clsData();
            DataTable dt = obj.getClientList();
            if (dt.Rows.Count > 0)
            {
                dtClient.Merge(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                   
                  //  arrClient.Add(new Tuple(dt.Rows[i][1].ToString(),Convert.ToInt32(dt.Rows[i][0].ToString()));
                    arrClient.Add(new Tuple<string, int>(dt.Rows[i][1].ToString(), Convert.ToInt32(dt.Rows[i][0].ToString())));
                 }
            }
            LoadClientLedgerMappingListNew(Convert.ToInt32(ddl_List.SelectedValue.ToString()));
        }
 
        public void LoadClientLedgerMappingListNew(int statusLedger)
        {
            grdMappingList.Visible = true;
            grdMapping.Visible = false;
            var mapping = dc.ClientLedger_View(Convert.ToInt32(lblClientId.Text), 0, statusLedger, true);
            grdMappingList.DataSource = mapping;
            grdMappingList.DataBind();

            if (grdMappingList.Rows.Count > 0)
                lblTotalRecords.Text = "Total No of Records : " + grdMappingList.Rows.Count;
            else
                lblTotalRecords.Text = "Total No of Records : 0";
        }
        protected void lnkUpdate(object sender, CommandEventArgs e)
        {
            string strMsg = "";
            string ledgerId = e.CommandArgument.ToString();
            LinkButton lb = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)lb.NamingContainer;
            Label lblCL_Id = (Label)gr.FindControl("lblCL_Id");
            Label lblCL_IdNew = (Label)gr.FindControl("lblCL_IdNew");
            Label lblLedger_Id = (Label)gr.FindControl("CLLedger_Ledger_Id");
            if (((lblCL_Id.Text != "" && lblCL_Id.Text != "0") || (lblCL_IdNew.Text != "" && lblCL_IdNew.Text != "0")) && lblLedger_Id.Text != "")
            {
                var result = dc.ClientLedger_View(Convert.ToInt32(lblCL_IdNew.Text), Convert.ToInt32(lblLedger_Id.Text), 0, false).ToList();
                if (result.Count == 0)
                {
                    //delete prv mapping
                    if (lblCL_Id.Text != "" && lblCL_Id.Text != "0")
                        dc.ClientLedger_Update(Convert.ToInt32(lblCL_Id.Text), Convert.ToInt32(lblLedger_Id.Text), true);
                    //insert new mapping
                    if (lblCL_IdNew.Text != "" && lblCL_IdNew.Text != "0")
                        dc.ClientLedger_Update(Convert.ToInt32(lblCL_IdNew.Text), Convert.ToInt32(lblLedger_Id.Text), false);
                    strMsg = "Client Ledger mapping saved successfully.";
                }
                else
                    strMsg = "Client Ledger mapping already exist.";
            }
            if (strMsg != "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
            }

        }
        protected void lnkUpdateAll_Click(object sender, EventArgs e)
        {
            if (grdMappingList.Rows.Count > 0)
            {
                string strMsg = "";
                for (int i = 0; i < grdMappingList.Rows.Count; i++)
                {
                    Label lblCL_Id = (Label)grdMappingList.Rows[i].Cells[2].FindControl("lblCL_Id");
                    Label lblCL_IdNew = (Label)grdMappingList.Rows[i].Cells[2].FindControl("lblCL_IdNew");
                    Label lblLedger_Id = (Label)grdMappingList.Rows[i].Cells[1].FindControl("CLLedger_Ledger_Id");
                    if (((lblCL_Id.Text != "" && lblCL_Id.Text != "0") || (lblCL_IdNew.Text != "" && lblCL_IdNew.Text != "0")) && lblLedger_Id.Text != "")
                    {
                        var result = dc.ClientLedger_View(Convert.ToInt32(lblCL_IdNew.Text), Convert.ToInt32(lblLedger_Id.Text), 0, false).ToList();
                        if (result.Count == 0)
                        {
                            //delete prv mapping
                            if (lblCL_Id.Text != "" && lblCL_Id.Text != "0")
                                dc.ClientLedger_Update(Convert.ToInt32(lblCL_Id.Text), Convert.ToInt32(lblLedger_Id.Text), true);
                            //insert new mapping
                            if (lblCL_IdNew.Text != "" && lblCL_IdNew.Text != "0")
                                dc.ClientLedger_Update(Convert.ToInt32(lblCL_IdNew.Text), Convert.ToInt32(lblLedger_Id.Text), false);

                            strMsg = "Client Ledger mapping saved successfully.";
                        }

                    }
                }

                if (strMsg != "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
                }

            }
        }
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            string strMsg = "";

            if (lblClientId.Text == "" || lblClientId.Text == "0")
            {
                strMsg = "Select client from list.";
                txt_Client.Focus();
            }
            else if (lblLedgerId.Text == "" || lblLedgerId.Text == "0")
            {
                strMsg = "Select ledger name from list.";
                txt_Ledger.Focus();
            }
            else
            {
                bool foundFlag = false;
                for (int i = 0; i < grdMapping.Rows.Count; i++)
                {
                    Label CLLedger_CL_Id = (Label)grdMapping.Rows[i].FindControl("CLLedger_CL_Id");
                    Label CLLedger_Ledger_Id = (Label)grdMapping.Rows[i].FindControl("CLLedger_Ledger_Id");
                    if (lblClientId.Text == CLLedger_CL_Id.Text && lblLedgerId.Text == CLLedger_Ledger_Id.Text)
                    {
                        foundFlag = true;
                        break;
                    }
                }
                if (foundFlag == false)
                {
                    strMsg = "Client Ledger mapping does not exist.";
                }
            }

            if (strMsg == "")
            {
                dc.ClientLedger_Update(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblLedgerId.Text), true);
                strMsg = "Client Ledger mapping deleted successfully.";
                LoadClientLedgerMappingList();
            }
            if (strMsg != "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + strMsg + "');", true);
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
        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            lblClientId.Text = "0";
            grdMappingList.Visible = false;
            grdMapping.Visible = true;
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    lblClientId.Text = Request.Form[hfClientId.UniqueID];
                    LoadClientLedgerMappingList();
                    ddl_List.SelectedIndex = 0;
                }
            }
        }
        public void LoadClientLedgerMappingList()
        {
            //var mapping = dc.ClientLedger_View(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblLedgerId.Text));
            var mapping = dc.ClientLedger_View(Convert.ToInt32(lblClientId.Text), 0, 0, false);
            grdMapping.DataSource = mapping;
            grdMapping.DataBind();
            if (grdMapping.Rows.Count > 0)
                lblTotalRecords.Text = "Total No of Records : " + grdMapping.Rows.Count;
            else
                lblTotalRecords.Text = "Total No of Records : 0";
        }
        protected Boolean ChkClientName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Client.Text;
            Boolean valid = false;
            var query = dc.Client_View(0, 0, searchHead, "");
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Client.Focus();
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
        protected void txt_ClientTextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
            TextBox txtClient = (TextBox)gvRow.FindControl("txtClient");
            Label lblCL_IdNew = (Label)gvRow.FindControl("lblCL_IdNew");

            if (ChkClient_Name(txtClient.Text) == true)
            {
                if (txtClient.Text != "")
                {
                    lblCL_IdNew.Text = Request.Form[hfClId.UniqueID];
                 }
            }
        }
        protected Boolean ChkClient_Name(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            //searchHead = txt_Client.Text;
            Boolean valid = false;
           // var query = dc.Client_View(0, 0, searchHead, "");
            string strQuery = "CL_Name_var like '" + searchHead + "'";
            DataTable dt = new DataTable();
            DataRow[] drFilterRows = dtClient.Select(strQuery);
            //if(drFilterRows.Length>0)
            //    valid = true;
            foreach (var obj in drFilterRows)
                 valid = true;
           
            if (valid == false)
            {
                txt_Client.Focus();
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
        public static List<string> Get_Client_name(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            //var query = db.Client_View(0, -1, searchHead, "");
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("CL_Name_var");
            string strQuery = "CL_Name_var like '" + searchHead + "'";
            DataRow[] drFilterRows = dtClient.Select(strQuery);
            foreach (var rowObj in drFilterRows)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj[1].ToString(), rowObj[0].ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                // var clnm = db.Client_View(0, 0, "", "");
                searchHead = "";
                string strQuery1 = "CL_Name_var like '" + searchHead + "'";
                DataRow[] drFilterRows1 = dtClient.Select(strQuery);

                foreach (var rowObj in drFilterRows1)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj[1].ToString(), rowObj[0].ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> CL_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            { 
                CL_Name_var.Add(dt.Rows[i][0].ToString());
            }

            //foreach (var rowObj in drFilterRows)
            //{
            //    CL_Name_var.Add(rowObj[1].ToString());
            //}
            return CL_Name_var;

        }


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetClientname(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Client_View(0, -1, searchHead, "");
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("CL_Name_var");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                var clnm = db.Client_View(0, 0, "", "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> CL_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CL_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return CL_Name_var;

        }

        protected void txt_Ledger_TextChanged(object sender, EventArgs e)
        {
            lblLedgerId.Text = "0";
            if (ChkLedgerName(txt_Ledger.Text) == true)
            {
                if (txt_Ledger.Text != "")
                {
                    lblLedgerId.Text = Request.Form[hfLedgerId.UniqueID].ToString();
                    //LoadClientLedgerMappingList();
                }
            }
        }

        protected Boolean ChkLedgerName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Ledger.Text;
            Boolean valid = false;
            var query = dc.Ledger_View(0, 0, true, searchHead);
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Ledger.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This ledger name is not in the list";
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
        public static List<string> GetLedgername(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Ledger_View(0, 0, true, searchHead);
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("LedgerName_Description");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.LedgerName_Description, rowObj.LedgerName_Id.ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                var clnm = db.Ledger_View(0, 0, true, "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.LedgerName_Description, rowObj.LedgerName_Id.ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> Ledger_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Ledger_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return Ledger_Name_var;

        }

        protected void grdMappingList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = new DataTable();
            if (ViewState["DataTable"] != null)
                dt = (DataTable)ViewState["DataTable"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txt = (TextBox)e.Row.FindControl("txtClient");
                if (grdMappingList.DataSource == dt)
                {
                    txt.Text = DataBinder.Eval(e.Row.DataItem, "CL_Name_var").ToString();
                }
            }
        }

        protected void rbClient_CheckedChanged(object sender, EventArgs e)
        {
            if (rbClient.Checked)
            {
                tr4.Visible = false;
                grdMappingList.Visible = false;
                tr1.Visible = true;
                tr2.Visible = true;
                tr3.Visible = true;
                grdMapping.Visible = true;
            }
        }

        protected void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAll.Checked)
            {
                tr1.Visible = false;
                tr2.Visible = false;
                tr3.Visible = false;
                grdMapping.Visible = false;
                tr4.Visible = true;
                grdMappingList.Visible = true;
                txt_Client.Text = "";
                txt_Ledger.Text = "";
         
               
            }
        }

      
    }
}