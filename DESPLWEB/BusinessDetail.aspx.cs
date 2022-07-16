using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class BusinessDetail : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Business Details";
                Session["CL_ID"] = 0;
                Session["SITE_ID"] = 0;
                LoadMEList();
                LoadTestingTypeList();
                txt_FromDate.Text = DateTime.Today.AddDays(-DateTime.Today.Day).AddDays(1).ToString("dd/MM/yyyy");
                txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }
        protected void LoadMEList()
        {
            var data = dc.User_View_ME(-1);
            ddlME.DataSource = data;
            ddlME.DataTextField = "USER_Name_var";
            ddlME.DataValueField = "USER_Id";
            ddlME.DataBind();
            ddlME.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        protected void LoadTestingTypeList()
        {
            var inwd = dc.Material_View("", "");
            ddlTestingType.DataSource = inwd;
            ddlTestingType.DataTextField = "MATERIAL_Name_var";
            ddlTestingType.DataValueField = "MATERIAL_RecordType_var";
            ddlTestingType.DataBind();
            ddlTestingType.Items.Insert(0, new ListItem("---All---", ""));
            ddlTestingType.Items.Insert(1, new ListItem("Coupon", "---"));
        }
        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            lblClientId.Text = "0";
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    lblClientId.Text = Request.Form[hfClientId.UniqueID];
                }
                txt_Site.Focus();
            }
            Session["CL_ID"] = lblClientId.Text;
        }

        protected void txt_Site_TextChanged(object sender, EventArgs e)
        {
            lblSiteId.Text = "0";
            int cl_Id = 0;
            if (Convert.ToInt32(Session["CL_ID"]) > 0)
            {
                if (int.TryParse(Session["CL_ID"].ToString(), out cl_Id))
                {
                    if (ChkSiteName(txt_Client.Text) == true)
                    {
                        int SiteId = 0;
                        if (int.TryParse(Request.Form[hfSiteId.UniqueID], out SiteId))
                        {
                            lblSiteId.Text = Request.Form[hfSiteId.UniqueID];
                        }
                    }
                }
            }
            Session["SITE_ID"] = lblSiteId.Text;
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
        protected Boolean ChkSiteName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Site.Text;
            Boolean valid = false;
            var res = dc.Site_View(0, Convert.ToInt32(Session["CL_ID"]), 0, searchHead);
            foreach (var obj in res)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Site.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Site Name is not in the list ";
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetSitename(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            DataRow row = null;
            DataTable dt = new DataTable();
            if (Convert.ToInt32(HttpContext.Current.Session["CL_ID"]) > 0)
            {
                int ClientId = 0;
                if (int.TryParse(HttpContext.Current.Session["CL_ID"].ToString(), out ClientId))
                {

                    var res = db.Site_View(0, Convert.ToInt32(HttpContext.Current.Session["CL_ID"]), 0, searchHead);
                    dt.Columns.Add("SITE_Name_var");
                    foreach (var obj in res)
                    {
                        row = dt.NewRow();
                        string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.SITE_Name_var, obj.SITE_Id.ToString());
                        dt.Rows.Add(listitem);
                    }
                    if (row == null)
                    {
                        var resclnm = db.Site_View(0, Convert.ToInt32(HttpContext.Current.Session["CL_ID"]), 0, "");
                        foreach (var obj in resclnm)
                        {
                            row = dt.NewRow();
                            string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.SITE_Name_var, obj.SITE_Id.ToString());
                            dt.Rows.Add(listitem);
                        }
                    }
                }
            }
            List<string> SITE_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SITE_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return SITE_Name_var;
        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            string[] strDate = txt_FromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            strDate = txt_ToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            DateTime PrevFromDate = FromDate.AddMonths(-6);

            decimal billAmt = 0, billPendAmt = 0, businessAmt = 0, netTotal = 0, newBisunessTotal = 0, balTotal = 0, frmAmt = 0, toAmt = 0;
            bool clientFlag = false, siteFlag = false;
            if (txt_AmtFrom.Text != "")
                frmAmt = Convert.ToDecimal(txt_AmtFrom.Text);

            if (txt_AmtTo.Text != "")
                toAmt = Convert.ToDecimal(txt_AmtTo.Text);

            if (chkClient.Checked)
                clientFlag = true;

            if (chkSite.Checked)
                siteFlag = true;
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dtNew.Columns.Add(new DataColumn("SiteName", typeof(string)));
            dtNew.Columns.Add(new DataColumn("BusinessFrom", typeof(string)));
            dtNew.Columns.Add(new DataColumn("BillAmt", typeof(string)));
            dtNew.Columns.Add(new DataColumn("NewBusiness", typeof(string)));
            dtNew.Columns.Add(new DataColumn("PendingAmt", typeof(string)));
            dtNew.Columns.Add(new DataColumn("BillNo", typeof(string)));
            dtNew.Columns.Add(new DataColumn("TestingType", typeof(string)));

            DataTable dt = new DataTable();
            DataRow dr1 = null;

            dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt.Columns.Add(new DataColumn("SiteName", typeof(string)));
            dt.Columns.Add(new DataColumn("BusinessFrom", typeof(string)));
            dt.Columns.Add(new DataColumn("BillAmt", typeof(string)));
            dt.Columns.Add(new DataColumn("NewBusiness", typeof(string)));
            dt.Columns.Add(new DataColumn("PendingAmt", typeof(string)));
            dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
            dt.Columns.Add(new DataColumn("TestingType", typeof(string)));

            var billList = dc.Bill_View_Business(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblSiteId.Text), ddlTestingType.SelectedValue, Convert.ToInt32(ddlME.SelectedValue), PrevFromDate, FromDate, ToDate).ToList();
            foreach (var bill in billList)
            {

                //billAmt = Convert.ToDecimal(bill.BILL_SerTaxAmt_num + bill.BILL_EdCessAmt_num + bill.BILL_HighEdCessAmt_num + bill.BILL_SwachhBharatTaxAmt_num + bill.BILL_KisanKrishiTaxAmt_num);
                //billAmt = Convert.ToDecimal(bill.BILL_NetAmt_num) - billAmt;
                billAmt = Convert.ToDecimal(bill.BILL_NetAmt_num);
                netTotal = netTotal + billAmt;
                balTotal = balTotal + Convert.ToDecimal(bill.BILL_PendingAmount);
                //if (bill.MaxBillDate >= PrevFromDate && bill.MaxBillDate < FromDate)
                //if (bill.MaxBillDate == null || bill.MaxBillDate < PrevFromDate || bill.MaxBillDate > FromDate)
                if (bill.MaxBillDate == null)
                {
                    newBisunessTotal = newBisunessTotal + billAmt;
                }

                dr1 = dt.NewRow();
                dr1["ClientName"] = bill.CL_Name_var;
                dr1["SiteName"] = bill.SITE_Name_var;
                if (bill.MEName == null)
                    dr1["BusinessFrom"] = "NA";
                else
                    dr1["BusinessFrom"] = bill.MEName;
                dr1["BillAmt"] = billAmt.ToString("0.00");
                if (bill.MaxBillDate == null)
                {
                    businessAmt = billAmt;
                    dr1["NewBusiness"] = businessAmt.ToString("0.00");

                }
                billPendAmt = Convert.ToDecimal(bill.BILL_PendingAmount);
                dr1["PendingAmt"] = billPendAmt.ToString("0.00");
                dr1["BillNo"] = bill.BILL_Id;
                if (bill.BILL_RecordType_var == "---")
                    dr1["TestingType"] = "Coupon";
                else
                    dr1["TestingType"] = bill.BILL_RecordType_var;



              if (clientFlag && dt.Rows.Count > 0)
                {
                    if (dr1["ClientName"].ToString().Equals(dt.Rows[dt.Rows.Count - 1][0].ToString()))
                    {
                        if (dt.Rows[dt.Rows.Count - 1][3].ToString() != "")
                        {
                            dt.Rows[dt.Rows.Count - 1][3] = Convert.ToDecimal(dt.Rows[dt.Rows.Count - 1][3].ToString()) + billAmt;
                        }
                        if (dt.Rows[dt.Rows.Count - 1][4].ToString() != "")
                        {
                            dt.Rows[dt.Rows.Count - 1][4] = Convert.ToDecimal(dt.Rows[dt.Rows.Count - 1][4].ToString()) + businessAmt;
                        }
                        if (dt.Rows[dt.Rows.Count - 1][5].ToString() != "")
                        {
                            dt.Rows[dt.Rows.Count - 1][5] = Convert.ToDecimal(dt.Rows[dt.Rows.Count - 1][5].ToString()) + billPendAmt;
                        }
                        dt.Rows[dt.Rows.Count - 1][1] = "-";
                        dt.Rows[dt.Rows.Count - 1][6] = "-";
                        dt.Rows[dt.Rows.Count - 1][7] = "-";

                    }
                    else
                    {
                         dt.Rows.Add(dr1);
                    }
                }
                else if (siteFlag && dt.Rows.Count > 0)
                {
                    if (dr1["SiteName"].ToString().Equals(dt.Rows[dt.Rows.Count - 1][1].ToString()))
                    {
                        if (dt.Rows[dt.Rows.Count - 1][3].ToString() != "")
                        {
                            dt.Rows[dt.Rows.Count - 1][3] = Convert.ToDecimal(dt.Rows[dt.Rows.Count - 1][3].ToString()) + billAmt;
                        }
                        if (dt.Rows[dt.Rows.Count - 1][4].ToString() != "")
                        {
                            dt.Rows[dt.Rows.Count - 1][4] = Convert.ToDecimal(dt.Rows[dt.Rows.Count - 1][4].ToString()) + businessAmt;
                        }
                        if (dt.Rows[dt.Rows.Count - 1][5].ToString() != "")
                        {
                            dt.Rows[dt.Rows.Count - 1][5] = Convert.ToDecimal(dt.Rows[dt.Rows.Count - 1][5].ToString()) + billPendAmt;
                        }
                        dt.Rows[dt.Rows.Count - 1][6] = "-";
                        dt.Rows[dt.Rows.Count - 1][7] = "-";

                    }
                    else
                    {
                        dt.Rows.Add(dr1);
                    }
                }
                else
                {
                     dt.Rows.Add(dr1);

                }
            }
            decimal amt = 0 , sumBillAmt = 0, sumBillPendingAmt = 0, sumNweBusinessAmt = 0;
            bool flag = false; 
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (frmAmt != 0 && toAmt != 0)
                {

                    if (dt.Rows[i][3].ToString() != "")
                    {
                        amt = Convert.ToDecimal(dt.Rows[i][3].ToString());
                        if (amt >= frmAmt && amt <= toAmt)
                        {
                            flag = true;
                            DataRow dr = dtNew.NewRow();
                            dr.ItemArray = dt.Rows[i].ItemArray;
                            dtNew.Rows.Add(dr);
                            sumBillAmt = sumBillAmt + amt;
                            if (dt.Rows[i][4].ToString() != "")
                                sumNweBusinessAmt = sumNweBusinessAmt + Convert.ToDecimal(dt.Rows[i][4].ToString());
                            if (dt.Rows[i][5].ToString() != "")
                                sumBillPendingAmt = sumBillPendingAmt + Convert.ToDecimal(dt.Rows[i][5].ToString());
                        }
                    }
                }
                else
                {
                    flag = true;
                    break; 
                }
            }



            if (dtNew.Rows.Count > 0 || (dtNew.Rows.Count ==0 && flag == false)) 
            {
                grdDetail.DataSource = dtNew;
                grdDetail.DataBind();
                lblRecords.Text = "Total No of Records : " + grdDetail.Rows.Count;
                lbl_Total.Text = "Total Billing (Including Tax)  =  " + sumBillAmt.ToString();
                lbl_Total.Text += "&nbsp;&nbsp;&nbsp;&nbsp;" + "New Business(Including Tax) =   " + sumNweBusinessAmt.ToString();
                lbl_Total.Text += "&nbsp;&nbsp;&nbsp;&nbsp;" + "Pending (Including Tax) =  " + sumBillPendingAmt.ToString();


            }
            else
            {
                grdDetail.DataSource = dt;
                grdDetail.DataBind();
                lblRecords.Text = "Total No of Records : " + grdDetail.Rows.Count;
                lbl_Total.Text = "Total Billing (Including Tax)  =  " + netTotal.ToString("0.00");
                lbl_Total.Text += "&nbsp;&nbsp;&nbsp;&nbsp;" + "New Business(Including Tax) =   " + newBisunessTotal.ToString("0.00");
                lbl_Total.Text += "&nbsp;&nbsp;&nbsp;&nbsp;" + "Pending (Including Tax) =  " + balTotal.ToString("0.00");


            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdDetail.Rows.Count > 0)
            {
                PrintGrid.PrintGridView(grdDetail, "Business Details", "BusinessDeatils");
            }
        }

        protected void chkClient_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClient.Checked)
                chkSite.Checked = false;
        }

        protected void chkSite_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSite.Checked)
                chkClient.Checked = false;
        }
    }
}