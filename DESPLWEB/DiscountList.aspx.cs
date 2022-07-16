using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class DiscountList : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Client Discount List";
            }
        }

        protected void rdnAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rdnAll.Checked)
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                txt_Client.Visible = false;
                ImgBtnSearch.Visible = false;
                grdDiscList.DataSource = null;
                grdDiscList.DataBind();
                var result = dc.DiscountSetting_View(0, "").ToList();
                if (result.Count > 0)
                {
                    grdDiscList.DataSource = result;
                    grdDiscList.DataBind();
                }

                if (grdDiscList.Rows.Count > 0)
                    lblTotalRecords.Text = "Total No of Records : " + grdDiscList.Rows.Count;
                else
                    lblTotalRecords.Text = "Total No of Records : 0";
            }
        }

        protected void rdnSelected_CheckedChanged(object sender, EventArgs e)
        {
            if (rdnSelected.Checked)
            {
                txt_Client.Visible = true;
                txt_Client.Text = "";
                ImgBtnSearch.Visible = true;
                grdDiscList.DataSource = null;
                grdDiscList.DataBind();

                lblTotalRecords.Text = "Total No of Records : 0";
            }
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (txt_Client.Text != "")
            {
                if (lblClientId.Text != "0")
                {
                    var result = dc.DiscountSetting_View(Convert.ToInt32(lblClientId.Text),"").ToList();
                    if (result.Count > 0)
                    {
                        grdDiscList.DataSource = result;
                        grdDiscList.DataBind();
                    }

                    if (grdDiscList.Rows.Count > 0)
                        lblTotalRecords.Text = "Total No of Records : " + grdDiscList.Rows.Count;
                    else
                        lblTotalRecords.Text = "Total No of Records : 0";
                }
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Enter client name";
            }
        }
        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
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

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdDiscList.Rows.Count > 0)
                PrintGrid.PrintGridView(grdDiscList, "Client Discount List", "Client_Discount_List");
        }

       

    }
}