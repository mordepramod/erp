using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ClientBalanceUpdate : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Client Balance Updation";
                
                bool userRight = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_Account_right_bit == true)
                        userRight = true;
                }
                if (userRight == true)
                {
                    Session["CL_ID"] = 0;
                    txt_Client.Focus();
                }
                else
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetClientname(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Client_View(0, 0, searchHead, "");
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

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            grdBill.DataSource = null;
            grdBill.DataBind();
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    Session["CL_ID"] = Convert.ToInt32(Request.Form[hfClientId.UniqueID]);
                }
                else
                {
                    Session["CL_ID"] = 0;
                }
                //DisplayBalLmt();
                LoadBillList();
            }
        }
        private void DisplayBalLmt()
        {
            int Cl_Id = 0;
            if (int.TryParse(Session["CL_ID"].ToString(), out Cl_Id))
            {
                if (Convert.ToInt32(Session["CL_ID"]) > 0)
                {
                    var data = dc.Client_View(Convert.ToInt32(Session["CL_ID"]), -1, "", "");
                    foreach (var c in data)
                    {
                        lblBalanceAmt.Text = "0.00";
                        if (c.CL_OutstandingAmt_var != null)
                        {
                            string[] strAmt = c.CL_OutstandingAmt_var.Split('|');
                            lblBalanceAmt.Text = Convert.ToDecimal(strAmt[0]).ToString("0.00");
                        }
                    }
                }
            }
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadBillList();
        }

        protected void LoadBillList()
        {
            grdBill.DataSource = null;
            grdBill.DataBind();

            DataTable dt = new DataTable();
            DataRow dtRow = dt.NewRow();
            dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("lblBillNo", typeof(string)));
            dt.Columns.Add(new DataColumn("lblBillDate", typeof(string)));
            dt.Columns.Add(new DataColumn("lblBillAmount", typeof(string)));                    
            dt.Columns.Add(new DataColumn("lblOutstandingBalance", typeof(string)));
            dt.Columns.Add(new DataColumn("txtChangedBalanceAmt", typeof(string)));

            var billd = dc.Bill_View_Old(Session["CL_ID"].ToString()).ToList();
            for (int i = 0; i < billd.Count(); i++)
            {
                dtRow = dt.NewRow();
                dt.Rows.Add(dtRow);
            }
            grdBill.DataSource = dt;
            grdBill.DataBind();
            int rowNo = 0;
            foreach (var bill in billd)
            {
                Label lblSrNo = (Label)grdBill.Rows[rowNo].FindControl("lblSrNo");
                Label lblBillNo = (Label)grdBill.Rows[rowNo].FindControl("lblBillNo");
                Label lblBillDate = (Label)grdBill.Rows[rowNo].FindControl("lblBillDate");
                Label lblBillAmount = (Label)grdBill.Rows[rowNo].FindControl("lblBillAmount");
                Label lblOutstandingBalance = (Label)grdBill.Rows[rowNo].FindControl("lblOutstandingBalance");
                TextBox txtChangedBalanceAmt = (TextBox)grdBill.Rows[rowNo].FindControl("txtChangedBalanceAmt");
                    
                lblSrNo.Text = (rowNo +1).ToString();
                lblBillNo.Text = bill.BILL_Id.ToString();
                lblBillDate.Text = Convert.ToDateTime(bill.BILL_Date_dt.ToString()).ToString("dd/MM/yyyy");                
                lblBillAmount.Text = bill.BILL_NetAmt_num.ToString();
                lblOutstandingBalance.Text = bill.BILL_PendingAmount.ToString(); ;
                txtChangedBalanceAmt.Text = "";

                rowNo++;
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            bool validData = true;
            bool billSelFlag = false;
            double billAmtToChange = 0;
            string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
            DateTime tillDate = DateTime.Now;
            if (cnStr.ToLower().Contains("mumbai") == true)
                tillDate = DateTime.ParseExact("28/02/2017", "dd/MM/yyyy", null);
            else if (cnStr.ToLower().Contains("nashik") == true)
                tillDate = DateTime.ParseExact("28/02/2017", "dd/MM/yyyy", null);
            else
                tillDate = DateTime.ParseExact("30/04/2016", "dd/MM/yyyy", null);
            for (int i = 0; i < grdBill.Rows.Count; i++)
            {
                Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
                Label lblBillAmount = (Label) grdBill.Rows[i].FindControl("lblBillAmount");
                Label lblOutstandingBalance = (Label)grdBill.Rows[i].FindControl("lblOutstandingBalance");
                TextBox txtChangedBalanceAmt = (TextBox) grdBill.Rows[i].FindControl("txtChangedBalanceAmt");
                CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    billSelFlag = true;
                    if (txtChangedBalanceAmt.Text == "")
                    {
                        validData = false;
                        txtChangedBalanceAmt.Focus();
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Input new outstanding balance amount');", true);
                        break;
                    }
                    else if (double.TryParse(txtChangedBalanceAmt.Text, out billAmtToChange) == false)
                    {
                        validData = false;
                        txtChangedBalanceAmt.Focus();
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Input valid new outstanding balance amount');", true);
                        break;
                    }
                    else if (Convert.ToDouble(txtChangedBalanceAmt.Text) > Convert.ToDouble(lblBillAmount.Text))
                    {
                        validData = false;
                        txtChangedBalanceAmt.Focus();
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('New outstanding balance amount should be less than or equal to bill amount');", true);
                        break;
                    }
                    else if (Convert.ToDouble(txtChangedBalanceAmt.Text) == Convert.ToDouble(lblOutstandingBalance.Text))
                    {
                        validData = false;
                        txtChangedBalanceAmt.Focus();
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('New outstanding balance amount is same as current outstanding balance amount');", true);
                        break;
                    }
                    else if (Convert.ToDouble(lblOutstandingBalance.Text) < Convert.ToDouble(txtChangedBalanceAmt.Text))
                    {
                        var cashdetail = dc.CashDetail_View_BillDateWise(lblBillNo.Text, tillDate);
                        if (cashdetail.Count() > 0)
                        {
                            validData = false;
                            txtChangedBalanceAmt.Focus();
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Receipt entry available for bill no " + lblBillNo.Text + " after date " + tillDate.ToString("dd/MM/yyyy") + ", Can not change outstanding amount.');", true);
                            break;
                        }
                    }
                }
            }
            if (billSelFlag == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select at least one row');", true);
            }
            else if (validData == true)
            {
                decimal adjAmount = 0;
                
                for (int i = 0; i < grdBill.Rows.Count; i++)
                {   
                    CheckBox chkSelect = (CheckBox)grdBill.Rows[i].FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        Label lblBillNo = (Label)grdBill.Rows[i].FindControl("lblBillNo");
                        Label lblBillAmount = (Label)grdBill.Rows[i].FindControl("lblBillAmount");
                        Label lblOutstandingBalance = (Label)grdBill.Rows[i].FindControl("lblOutstandingBalance");
                        TextBox txtChangedBalanceAmt = (TextBox)grdBill.Rows[i].FindControl("txtChangedBalanceAmt");
                        if (Convert.ToDouble(lblOutstandingBalance.Text) > Convert.ToDouble(txtChangedBalanceAmt.Text))
                        {
                            adjAmount = Convert.ToDecimal(lblOutstandingBalance.Text) - Convert.ToDecimal(txtChangedBalanceAmt.Text);
                            dc.CashDetail_Update_Balance(-1, lblBillNo.Text, DateTime.Now, adjAmount * -1, 0, false, false, "Part", Convert.ToInt32(Session["CL_ID"]), "", false);
                        }
                        else if (Convert.ToDouble(lblOutstandingBalance.Text) < Convert.ToDouble(txtChangedBalanceAmt.Text))
                        {
                            adjAmount = Convert.ToDecimal(lblBillAmount.Text) - Convert.ToDecimal(txtChangedBalanceAmt.Text);
                            dc.CashDetail_Update_Balance(0, lblBillNo.Text, tillDate, 0, 0, false, false, "", Convert.ToInt32(Session["CL_ID"]), "", true);
                            dc.CashDetail_Update_Balance(-1, lblBillNo.Text, DateTime.Now, adjAmount * -1, 0, false, false, "Part", Convert.ToInt32(Session["CL_ID"]), "", false);
                        }
                    }
                }
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Balance updated successfully.');", true);
                LoadBillList(); 
            }
        }

        protected void lnk_Exit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)grdBill.HeaderRow.FindControl("chkSelectAll");
            foreach (GridViewRow row in grdBill.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkSelect");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }

    }
}