using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Ionic.Zip;

namespace DESPLWEB
{
    public partial class POUpload : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "PO Upload for previous Bill";
                txtFromDate.Text = "01/04/2019";
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }
        
        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            LoadBillList();
        }

        protected void LoadBillList()
        {
            DataTable dt = new DataTable();
            DataRow dr1 = null;

            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt.Columns.Add(new DataColumn("SiteName", typeof(string)));
            dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
            dt.Columns.Add(new DataColumn("BillDate", typeof(string)));
            dt.Columns.Add(new DataColumn("TestingType", typeof(string)));
            dt.Columns.Add(new DataColumn("BillAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("PendingAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("DaySpending", typeof(string)));
            
            DateTime tempDate;
            int i = 0;
            decimal PendingAmount = 0;
            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            string strFromDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];
            strDate = txtToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            string strToDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];
            
            int ClientId = 0;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }
            clsData obj = new clsData();
            DataTable dtClientList = obj.getClientOutstanding2_forPOUpload(ClientId, strFromDate, strToDate, 0);
            for (int cllist = 0; cllist < dtClientList.Rows.Count; cllist++)
            {   
                DataTable dtClient = obj.getClientOutstanding(Convert.ToInt32(dtClientList.Rows[cllist]["BILL_CL_Id"]), strFromDate, strToDate, 0);
                for (int cl = 0; cl < dtClient.Rows.Count; cl++)
                {
                    int rowNo = 0;
                    //bills
                    var billList = dc.Bill_View_Outstanding_forPOUpload(Convert.ToInt32(dtClient.Rows[cl]["CL_Id"]), 0, FromDate, ToDate, false).ToList();
                    foreach (var bill in billList)
                    {
                        i++; rowNo++;
                        tempDate = Convert.ToDateTime(bill.BILL_Date_dt);
                        dr1 = dt.NewRow();
                        dr1["SrNo"] = i;
                        dr1["ClientName"] = bill.CL_Name_var;
                        dr1["SiteName"] = bill.SITE_Name_var;
                        dr1["BillNo"] = bill.BILL_Id;
                        dr1["BillDate"] = tempDate.ToString("dd/MM/yyyy");
                        if (bill.BILL_RecordType_var == "---")
                            dr1["TestingType"] = "Coupon";
                        else if (bill.BILL_RecordType_var == "Monthly")
                            dr1["TestingType"] = "Monthly";
                        else
                            dr1["TestingType"] = bill.testtype;
                        dr1["BillAmount"] = bill.BILL_NetAmt_num;
                        PendingAmount = Convert.ToDecimal(bill.BILL_NetAmt_num + bill.ReceivedAmt);
                        dr1["PendingAmount"] = PendingAmount.ToString("0.00");
                        dr1["DaySpending"] = (DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null) - Convert.ToDateTime(bill.BILL_Date_dt)).Days;
                        dt.Rows.Add(dr1);

                    }
                }
            }
            grdOutstanding.DataSource = dt;
            grdOutstanding.DataBind();
            Session["OutstandingDetails"] = dt;
        }

        protected void grdOutstanding_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int RowIndex = gvr.RowIndex;
            string billid = Convert.ToString(e.CommandArgument);

            if (e.CommandName == "lnkConfirmClientApproval")
            {   
                dc.Bill_Update_ApproveStatusByClient(billid, 0);
                LoadBillList();
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Approved Successfully !');", true);
            }
            else if (e.CommandName == "lnkUploadPO")
            {
                FileUpload FileUploadPO = (FileUpload)grdOutstanding.Rows[RowIndex].FindControl("FileUploadPO");
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
                    string filePath = "D:/POFiles/";
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
                    
                    dc.Bill_Update_POFile(billid, filename);
                    LoadBillList();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Uploaded Successfully !');", true);
                }
            }
            else if (e.CommandName == "DownloadFile")
            {
                string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                string filePath = "D:/POFiles/";
                if (cnStr.ToLower().Contains("mumbai") == true)
                    filePath += "Mumbai/";
                else if (cnStr.ToLower().Contains("nashik") == true)
                    filePath += "Nashik/";
                else if (cnStr.ToLower().Contains("metro") == true)
                    filePath += "Metro/";
                else
                    filePath += "Pune/";

                filePath += billid;
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

    }
}