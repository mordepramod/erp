using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class BillStatusUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["userId"]) == "0")
                Response.Redirect("default.aspx");
            if (!IsPostBack)
            {
                string strReq = "";
                strReq = Request.RawUrl;
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                if (!strReq.Equals(""))
                {
                    strReq = obj.Decrypt(strReq);
                }
                string[] arrMsgs = strReq.Split('=');
                lblLocation.Text = arrMsgs[1].ToString();
                if (arrMsgs[1].ToString().Equals("Pune"))
                    lblConnection.Text = System.Configuration.ConfigurationManager.AppSettings["conStrPune"].ToString();
                else if (arrMsgs[1].ToString().Equals("Mumbai"))
                    lblConnection.Text = System.Configuration.ConfigurationManager.AppSettings["conStrMumbai"].ToString();
                else if (arrMsgs[1].ToString().Equals("Nashik")) 
                    lblConnection.Text = System.Configuration.ConfigurationManager.AppSettings["conStrNashik"].ToString();
                else if (arrMsgs[1].ToString().Equals("MAHA-METRO"))
                    lblConnection.Text = System.Configuration.ConfigurationManager.AppSettings["conStrMetro"].ToString();

                lblConnectionLive.Text = lblConnection.Text;
                lblBranch.Text = "Location : " + lblLocation.Text;

                LinkButton lnkLogOut = (LinkButton)Master.FindControl("lnkLogOut");
                LinkButton lnkExit = (LinkButton)Master.FindControl("lnkExit");
                lnkLogOut.Visible = true;
                lnkExit.Visible = false;
                txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                LoadInwardTypeList();
            }
        }

        private void LoadInwardTypeList()
        {
            LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);
            var inwd = dc.Material_View("", "").ToList();
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardTestType.DataValueField = "MATERIAL_RecordType_var";
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, new ListItem("All", ""));
            ddl_InwardTestType.Items.Insert(1, new ListItem("Coupon", "---"));
            ddl_InwardTestType.Items.Insert(2, new ListItem("Monthly", "Monthly"));
        }

        public void LoadBillList(bool ModifiedFlag)
        {
            LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            int ClientId = 0;
            bool billStatus = false;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }
            if (ddlStatus.SelectedValue == "1")
            {
                billStatus = true;
            }
            var bill = dc.Bill_View_Status(ClientId, ddl_InwardTestType.SelectedItem.Value, 0, billStatus, Fromdate, Todate, ModifiedFlag, Convert.ToByte(ddlClientApproveStatus.SelectedValue), 2);
            grdModifyBill.DataSource = bill;
            grdModifyBill.DataBind();

            var inwd = dc.Section_View().ToList();
            if (grdModifyBill.Rows.Count > 0)
            {
                int otherSectionId = 0;
                foreach (var item in inwd)
                {
                    if (item.Section_Name_var.ToLower().Equals("others"))
                        otherSectionId = Convert.ToInt32(item.Section_Id.ToString());
                }
                bool billPrintRight = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_BillPrint_right_bit == true)
                        billPrintRight = true;
                }
                decimal billTotal = 0, billPaidAmount = 0, billPendingAmount = 0;
                for (int i = 0; i < grdModifyBill.Rows.Count; i++)
                {
                    string recType = grdModifyBill.Rows[i].Cells[2].Text.ToString();
                    LinkButton lnkPrintBill = (LinkButton)grdModifyBill.Rows[i].FindControl("lnkPrintBill");
                    DropDownList ddl_Inward = (DropDownList)grdModifyBill.Rows[i].FindControl("ddl_Inward");
                    Label lblSectionId = (Label)grdModifyBill.Rows[i].FindControl("lblSectionId");
                    ddl_Inward.DataSource = inwd;
                    ddl_Inward.DataTextField = "Section_Name_var";
                    ddl_Inward.DataValueField = "Section_Id";
                    ddl_Inward.DataBind();
                    ddl_Inward.Items.Insert(0, new ListItem("---Select---", "0"));
                    if (lblSectionId.Text != "" && Convert.ToInt32(lblSectionId.Text) > 0)
                    {
                        ddl_Inward.SelectedValue = lblSectionId.Text;
                    }
                    else if (recType == "OT")
                    {
                        ddl_Inward.SelectedValue = otherSectionId.ToString();
                    }
                    else
                    {
                        int sec_id = 0, flag = 0;
                        if (recType != "---" && recType != "Monthly")
                        {
                            foreach (var item in inwd)
                            {
                                if (Convert.ToString(item.Section_RecordType_var) != null && Convert.ToString(item.Section_RecordType_var) != "")
                                {
                                    if (flag == 1)
                                        break;

                                    string[] arr = item.Section_RecordType_var.Split(',');
                                    for (int j = 0; j < arr.Length; j++)
                                    {
                                        if (arr[j].Equals(recType))
                                        {
                                            sec_id = Convert.ToInt32(item.Section_Id);
                                            flag = 1;
                                        }
                                    }

                                }

                            }
                        }
                        ddl_Inward.SelectedValue = sec_id.ToString();
                    }
                    
                    if (billPrintRight == true)
                    {
                        lnkPrintBill.Enabled = true;
                    }
                    else
                    {
                        lnkPrintBill.Enabled = false;
                    }
                    billTotal += Convert.ToDecimal(grdModifyBill.Rows[i].Cells[6].Text);
                    if (grdModifyBill.Rows[i].Cells[13].Text != "" && grdModifyBill.Rows[i].Cells[14].Text != "&nbsp;")
                        billPaidAmount += Convert.ToDecimal(grdModifyBill.Rows[i].Cells[14].Text);
                    if (grdModifyBill.Rows[i].Cells[14].Text != "" && grdModifyBill.Rows[i].Cells[15].Text != "&nbsp;")
                        billPendingAmount += Convert.ToDecimal(grdModifyBill.Rows[i].Cells[15].Text);
                }
                lblTotal.Text = "Total : " + billTotal.ToString("0.00");
                lblPaidAmount.Text = "Total Paid Amount : " + billPaidAmount.ToString("0.00");
                lblPendingAmount.Text = "Total Pending Amount : " + billPendingAmount.ToString("0.00");
            }
        }

        protected void grdModifyInward_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int RowIndex = gvr.RowIndex;
            DropDownList ddl = (DropDownList)gvr.FindControl("ddl_Inward");
            Session["BillId"] = Convert.ToString(e.CommandArgument);
            string billid = Convert.ToString(e.CommandArgument);
            var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
            foreach (var u in user)
            {
                var bill = dc.Bill_View(billid, 0, 0, "", 0, false, false, null, null).ToList();
                if (u.USER_BillPrint_right_bit == true && e.CommandName == "lnkPrintBill")
                {
                    printBill(Session["BillId"].ToString(), "pdf", "Print");
                    break;
                }
                else if (e.CommandName == "DownloadFile")
                {
                    //string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                    string cnStr = lblConnection.Text;
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
                else if (e.CommandName == "DownloadMOUFile")
                {
                    //string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                    string cnStr = lblConnection.Text;
                    string filePath = "D:/MOUFiles/";
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
                else if (e.CommandName == "ViewProposal")
                {
                    if (billid != "")
                    {
                        string[] strVal = billid.Split(';');
                        PrintPDFReport rpt = new PrintPDFReport(lblConnection.Text);
                        rpt.Proposal_PDF(Convert.ToInt32(strVal[0]), 0, Convert.ToInt32(strVal[1]), false, "View", strVal[2]);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Access is Denied !');", true);
                }
            }
        }
        public bool IsValidEmailAddress(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");
            }
        }
        private void printBill(string billNo, string filetype, string Action)
        {
            PrintPDFReport obj = new PrintPDFReport(lblConnection.Text);
            obj.Bill_PDFPrint(billNo, false, Action);
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadBillList(false);
        }
        
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdModifyBill.Rows.Count > 0 && grdModifyBill.Visible == true)
            {

                string Subheader = "";
                Subheader = "" + "|" + lblBilldt.Text + "|" + txtFromDate.Text + " - " + txtToDate.Text + "|" + lblInwdtype.Text + "|" + ddl_InwardTestType.SelectedItem.Text;
                string grddata = "";
                string grdColumn = "";
                string grdview = "";
                PrintHTMLReport rpt = new PrintHTMLReport();
                for (int j = 0; j <= grdModifyBill.Columns.Count - 1; j++)
                {
                    if (grdModifyBill.Columns[j].HeaderText != "Section" && grdModifyBill.Columns[j].HeaderText != "Gst No")
                        grdColumn += grdModifyBill.Columns[j].HeaderText + "|";
                    
                }
                for (int i = 0; i <= grdModifyBill.Rows.Count - 1; i++)
                {
                    grddata += "$";
                    for (int c = 0; c <= grdModifyBill.Rows[i].Cells.Count - 1; c++)
                    {
                        grddata += grdModifyBill.Rows[i].Cells[c].Text + "~";
                    }
                }
                grdview = grdColumn + grddata;
                rpt.RptHTMLgrid("Bill Status", Subheader, grdview);

            }

        }

        protected void lnkEnteredBillPrint_Click(object sender, EventArgs e)
        {
            LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);
            if (txtBillNo.Text.Trim() != "")
            {
                if (txtBillNo.Text.Trim() != "")
                {
                    var bill = dc.Bill_View(txtBillNo.Text, 0, 0, "", 0, false, false, null, null).ToList();
                    if (bill.Count > 0)
                    {
                        printBill(txtBillNo.Text, "pdf", "Print");
                        
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Bill not available. ..');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Enter valid bill number. ..');", true);
                }
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
            LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);
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
        
        protected void lnkMonthlyBillDetail_Click(object sender, EventArgs e)
        {
            LabDataDataContext dc = new LabDataDataContext(lblConnectionLive.Text);
            if (txtBillNo.Text.Trim() != "")
            {
                //string tempBillNo ;
                if (txtBillNo.Text.Trim() != "")
                {
                    var bill = dc.Bill_View(txtBillNo.Text, 0, 0, "", 0, false, false, null, null).ToList();
                    if (bill.Count > 0)
                    {
                        if (bill[0].BILL_RecordType_var == "Monthly")
                        {
                            BillUpdation billPrint = new BillUpdation();
                            billPrint.getMonthlyBillDetailPrint(txtBillNo.Text);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Can not print details for regular bill..');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Bill not available. ..');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Enter valid bill number. ..');", true);
                }
            }
        }

        protected void ImgBtnSearchClApprStatusWise_Click(object sender, ImageClickEventArgs e)
        {
            LoadBillList(false);
        }
    }
}