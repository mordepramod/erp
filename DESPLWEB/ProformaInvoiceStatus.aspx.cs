using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Data;

namespace DESPLWEB
{
    public partial class ProformaInvoiceStatus : System.Web.UI.Page
    {        
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Proforma Invoice Status";
                txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                LoadInwardTypeList();
            }
        }

        private void LoadInwardTypeList()
        {   
            var inwd = dc.Material_View("","");
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardTestType.DataValueField = "MATERIAL_RecordType_var";
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, new ListItem("All",""));
        }
        
        public void LoadProformaInvoiceList()
        {           
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            int ClientId = 0;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }

            var ProformaInvoice = dc.ProformaInvoice_View("0", ClientId, 0, ddl_InwardTestType.SelectedItem.Value, "", false, false, Fromdate, Todate);
            grdModifyProformaInvoice.DataSource = ProformaInvoice;
            grdModifyProformaInvoice.DataBind();

            if (grdModifyProformaInvoice.Rows.Count > 0)
            {
                bool billRight = false, billPrintRight = false, billApproveRight = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_Bill_right_bit == true)
                        billRight = true;
                    if (u.USER_BillPrint_right_bit == true)
                        billPrintRight = true;
                    if (u.USER_Approve_right_bit == true)
                        billApproveRight = true;
                }
                decimal ProformaInvoiceTotal = 0;
                for (int i = 0; i < grdModifyProformaInvoice.Rows.Count; i++)
                {
                    LinkButton lnkModifyProformaInvoice = (LinkButton)grdModifyProformaInvoice.Rows[i].FindControl("lnkModifyProformaInvoice");
                    LinkButton lnkPrintProformaInvoice = (LinkButton)grdModifyProformaInvoice.Rows[i].FindControl("lnkPrintProformaInvoice");
                    LinkButton lnkApproveProformaInvoice = (LinkButton)grdModifyProformaInvoice.Rows[i].FindControl("lnkApproveProformaInvoice");
                    Label lblApproveStatus = (Label)grdModifyProformaInvoice.Rows[i].FindControl("lblApproveStatus");
                    LinkButton lnkEmailProformaInvoice = (LinkButton)grdModifyProformaInvoice.Rows[i].FindControl("lnkEmailProformaInvoice");

                    if (billRight == true && lblApproveStatus.Text != "True")
                    {
                        lnkModifyProformaInvoice.Enabled = true;
                    }
                    else
                    {
                        lnkModifyProformaInvoice.Enabled = false;
                    }
                    if (billApproveRight == true && lblApproveStatus.Text != "True")
                    {
                        lnkApproveProformaInvoice.Enabled = true;
                    }
                    else
                    {
                        lnkApproveProformaInvoice.Enabled = false;
                    }
                    if (billPrintRight == true)
                    {
                        lnkPrintProformaInvoice.Enabled = true;
                        if (lblApproveStatus.Text == "True")
                        {
                            lnkEmailProformaInvoice.Enabled = true;
                        }
                        else
                        {
                            lnkEmailProformaInvoice.Enabled = false;
                        }
                    }
                    else
                    {
                        lnkPrintProformaInvoice.Enabled = false;
                        lnkEmailProformaInvoice.Enabled = false;
                    }
                    ProformaInvoiceTotal += Convert.ToDecimal(grdModifyProformaInvoice.Rows[i].Cells[4].Text);
                    
                }
                lblTotal.Text = "Total : " + ProformaInvoiceTotal.ToString("0.00");
            }
        }
        
        protected void grdModifyInward_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int RowIndex = gvr.RowIndex; 
            
            Session["ProInvId"] = null;
            Session["ProInvId"] = Convert.ToString(e.CommandArgument);
            string ProformaInvoiceid = Convert.ToString(e.CommandArgument);
            var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
            foreach (var u in user)
            {
                var ProformaInvoice = dc.ProformaInvoice_View(ProformaInvoiceid, 0, 0, "", "", false, false, null, null).ToList();                    
                if (u.USER_BillPrint_right_bit == true && e.CommandName == "lnkPrintProformaInvoice")
                {
                    printProformaInvoice(Session["ProInvId"].ToString(), "html");
                    break;
                }
                else if (u.USER_BillPrint_right_bit == true && e.CommandName == "lnkEmailProformaInvoice")
                {   
                    printProformaInvoice(Session["ProInvId"].ToString(),"pdf");
                    break;
                }
                else if (u.USER_Bill_right_bit == true && e.CommandName == "lnkModifyProformaInvoice")
                {
                    bool modifyProformaInvoiceFlag = true;
                    //var ProformaInvoice = dc.ProformaInvoice_View(Convert.ToInt32(ProformaInvoiceid), 0, 0, "", 0, false, false, null, null);
                    foreach (var bl in ProformaInvoice)
                    {
                        var master = dc.MasterSetting_View(0);
                        foreach (var mst in master)
                        {
                            if (mst.MASTER_BillLockDate_dt != null)
                            {
                                if (mst.MASTER_BillLockDate_dt >= bl.PROINV_Date_dt)
                                {
                                    modifyProformaInvoiceFlag = false;
                                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Proforma Invoice Modification Locked by Account Dept. ..');", true);
                                }
                            }
                        }
                        
                    }
                    if (modifyProformaInvoiceFlag == true)
                    {
                        string CouponProformaInvoice = "False";
                        if (ddl_InwardTestType.SelectedItem.Value == "---")
                            CouponProformaInvoice = "True";
                        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                        string strURLWithData = "ProformaInvoice.aspx?" + obj.Encrypt(string.Format("ProformaInvoiceId={0}&CouponProformaInvoice={1}", ProformaInvoiceid, CouponProformaInvoice));
                        PrintGrid.Redirect1(strURLWithData, "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                    }
                }
                else if (u.USER_Approve_right_bit == true && e.CommandName == "lnkApproveProformaInvoice")
                {
                    dc.ProformaInvoice_Update_ApproveStatus(ProformaInvoiceid,Convert.ToInt32(Session["LoginId"]), true);
                    Label lblApproveStatus = (Label)grdModifyProformaInvoice.Rows[RowIndex].FindControl("lblApproveStatus");
                    LinkButton lnkModifyProformaInvoice = (LinkButton)grdModifyProformaInvoice.Rows[RowIndex].FindControl("lnkModifyProformaInvoice");
                    LinkButton lnkApproveProformaInvoice = (LinkButton)grdModifyProformaInvoice.Rows[RowIndex].FindControl("lnkApproveProformaInvoice");
                    lblApproveStatus.Text = "True";
                    lnkModifyProformaInvoice.Enabled = false;
                    lnkApproveProformaInvoice.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Approved Successfully !');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Access is Denied !');", true);
                }
            }
        }
        
        private void printProformaInvoice(string ProformaInvoiceNo, string filetype)
        {
            //if (filetype == "html")
            //{
                ProformaInvoiceUpdation ProformaInvoice = new ProformaInvoiceUpdation();
                ProformaInvoice.getProformaInvoicePrintString(ProformaInvoiceNo, "Print");
            //}
            //else
            //{
            //    PrintPDFReport obj = new PrintPDFReport();
            //    obj.ProformaInvoice_PDFPrint(Convert.ToInt32(ProformaInvoiceNo), "DisplayLogo");
            //}
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadProformaInvoiceList();
        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdModifyProformaInvoice.Rows.Count > 0 && grdModifyProformaInvoice.Visible == true)
            {

                string Subheader = "";
                Subheader = "" + "|" + lblProformaInvoicedt.Text + "|" + txtFromDate.Text + " - " + txtToDate.Text + "|" + lblInwdtype.Text + "|" + ddl_InwardTestType.SelectedItem.Text;
                //string reportPath;
                //string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
                string grddata = "";
                string grdColumn = "";
                string grdview = "";
                PrintHTMLReport rpt = new PrintHTMLReport();
                for (int j = 0; j < grdModifyProformaInvoice.Columns.Count; j++)
                {
                    if (grdModifyProformaInvoice.Columns[j].Visible == true)
                    {
                        grdColumn += grdModifyProformaInvoice.Columns[j].HeaderText + "|";
                    }
                }
                for (int i = 0; i < grdModifyProformaInvoice.Rows.Count; i++)
                {
                    grddata += "$";
                    for (int c = 0; c < grdModifyProformaInvoice.Rows[i].Cells.Count; c++)
                    {
                        grddata += grdModifyProformaInvoice.Rows[i].Cells[c].Text + "~";
                    }
                }
                grdview = grdColumn + grddata;
                rpt.RptHTMLgrid("Proforma Invoice Status", Subheader, grdview);

            }

        }

        protected void lnkEnteredProformaInvoicePrint_Click(object sender, EventArgs e)
        {
            if (txtProformaInvoiceNo.Text.Trim() != "")
            {
                int tempProformaInvoiceNo = 0;
                if (int.TryParse(txtProformaInvoiceNo.Text, out tempProformaInvoiceNo) == true)
                {
                    var ProformaInvoice = dc.ProformaInvoice_View(txtProformaInvoiceNo.Text, 0, 0, "", "", false, false, null, null).ToList();
                    if (ProformaInvoice.Count > 0)
                    {
                        printProformaInvoice(txtProformaInvoiceNo.Text, "html");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('ProformaInvoice not available. ..');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Enter valid ProformaInvoice number. ..');", true);
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