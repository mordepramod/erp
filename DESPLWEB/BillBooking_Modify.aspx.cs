using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class BillBooking_Modify : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (!IsPostBack)
            {
                Label lblheader = (Label)Master.FindControl("lblHeading");
                lblheader.Text = "Modify Bill Booking / Cash Payment";
                optBillBooking.Checked = true;
                txt_FrmDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txt_Todate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            DateTime From_Dtae = DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null);
            DateTime To_Dtae = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);
            if (To_Dtae >= From_Dtae)
            {
                LoadBillBookingOrCashPaymentList();
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = true;
                lblMsg.Text = "To Date should be Greater than or equal to the From Date";
                gvDetailsView.Visible = false;
            }
        }
        private void LoadBillBookingOrCashPaymentList()
        {
            for (int i = 0; i < gvDetailsView.Columns.Count; i++)
            {
                if ((optBillBooking.Checked == true && i <= 7)
                    || (optCashPayment.Checked == true && i >= 8))
                {
                    gvDetailsView.Columns[i].Visible = true;
                }
                else
                {
                    gvDetailsView.Columns[i].Visible = false;
                }
            }
            gvDetailsView.Visible = true;
            gvDetailsView.DataSource = null;
            gvDetailsView.DataBind();

            DateTime From_Dtae = DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null);
            DateTime To_Dtae = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);
            DataTable dt = new DataTable();
            DataRow dr1 = null;
            dt.Columns.Add(new DataColumn("BILLBOOK_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("BILLBOOK_Vend_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("BILLBOOK_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("BILLBOOK_SupplierInvoiceNo_var", typeof(string)));
            dt.Columns.Add(new DataColumn("BILLBOOK_InvoiceDate_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("BILLBOOK_NetPayableAmount_num", typeof(string)));
            dt.Columns.Add(new DataColumn("BILLBOOK_Type_var", typeof(string)));
            dt.Columns.Add(new DataColumn("VEND_FirmName_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CASHPAY_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CASHPAY_VoucherNo_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CASHPAY_VoucherDate_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("CASHPAY_TotalAmount_num", typeof(string)));
            if (optBillBooking.Checked == true)
            {
                var result = dc.BillBooking_View(0, From_Dtae, To_Dtae, "", Convert.ToBoolean(Convert.ToInt32(ddlStatus.SelectedValue)));
                foreach (var billb in result)
                {
                    dr1 = dt.NewRow();
                    dr1["BILLBOOK_Id"] = billb.BILLBOOK_Id;
                    dr1["BILLBOOK_Vend_Id"] = billb.BILLBOOK_VEND_Id;
                    dr1["BILLBOOK_Date_dt"] = Convert.ToDateTime(billb.BILLBOOK_Date_dt).ToString("dd/MM/yyyy"); ;
                    dr1["BILLBOOK_SupplierInvoiceNo_var"] = billb.BILLBOOK_SupplierInvoiceNo_var;
                    dr1["BILLBOOK_InvoiceDate_dt"] = Convert.ToDateTime(billb.BILLBOOK_InvoiceDate_dt).ToString("dd/MM/yyyy"); ;
                    dr1["BILLBOOK_NetPayableAmount_num"] = billb.BILLBOOK_NetPayableAmount_num;
                    dr1["BILLBOOK_Type_var"] = billb.BILLBOOK_Type_var;
                    dr1["VEND_FirmName_var"] = billb.VEND_FirmName_var;
                    dt.Rows.Add(dr1);
                }                
                gvDetailsView.DataSource = dt;
                gvDetailsView.DataBind();
            }
            else if (optCashPayment.Checked == true)
            {
                //var result = dc.CashPayment_View(0, "", From_Dtae, To_Dtae, "");
                var result = dc.CashPayment_View(0, "", From_Dtae, To_Dtae);
                foreach (var cashPay in result)
                {
                    dr1 = dt.NewRow();
                    dr1["CASHPAY_Id"] = cashPay.CASHPAY_Id;
                    dr1["CASHPAY_VoucherNo_var"] = cashPay.CASHPAY_VoucherNo_var;
                    dr1["CASHPAY_VoucherDate_dt"] = Convert.ToDateTime(cashPay.CASHPAY_VoucherDate_dt).ToString("dd/MM/yyyy"); 
                    dr1["CASHPAY_TotalAmount_num"] = cashPay.CASHPAY_TotalAmount_num;
                    dt.Rows.Add(dr1);
                }
                gvDetailsView.DataSource = dt;
                gvDetailsView.DataBind();
            }
        }
        protected void gvDetailsView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] arg = new string[2];
            arg = e.CommandArgument.ToString().Split(';');
            if (optBillBooking.Checked == true)
            {
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                string strURLWithData = "BillBooking.aspx?" + obj.Encrypt(string.Format("BillBookingId={0}&VendId={1}", arg[0],arg[1]));
                //Response.Redirect(strURLWithData)
                PrintGrid.Redirect1(strURLWithData, "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            }
            else if (optCashPayment.Checked == true)
            {
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                string strURLWithData = "VendorCashPayment.aspx?" + obj.Encrypt(string.Format("CashPaymentId={0}", arg[0]));
                //Response.Redirect(strURLWithData);
                PrintGrid.Redirect1(strURLWithData, "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (gvDetailsView.Rows.Count > 0 && gvDetailsView.Visible == true)
            {
                string reportStr = "";
                reportStr = rptModifyBillBookingOrCashPayment();
                PrintHTMLReport rptHtml = new PrintHTMLReport();
                string strFileName = "";
                if (optBillBooking.Checked == true)
                {
                    strFileName = "BillBookingReport";
                }
                else
                {
                    strFileName = "CashPaymentReport";
                }
                rptHtml.DownloadHtmlReport(strFileName, reportStr);
            }
        }
        protected string rptModifyBillBookingOrCashPayment()
        {
            string reportStr = "", mySql = "", tempSql = "", strHeading = "";
            if (optBillBooking.Checked == true)
            {
                strHeading = "Bill Booking List";
            }
            else
            {
                strHeading = "Cash Payment List";
            }
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> " + strHeading + " </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            mySql += "<tr>" +
            "<td width='15%' align=left valign=top height=19><font size=2><b> Period </b></font></td>" +
            "<td width='2%' height=19><font size=2>:</font></td>" +
            "<td width='40%' height=19><font size=2>" + DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + " - " + DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + "</font></td>" +
            "</tr>";
                        
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            for (int i = 1; i < gvDetailsView.Columns.Count; i++)
            {
                mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>" + gvDetailsView.Columns[i].HeaderText + " </b></font></td>";
            }
            mySql += "</tr>";

            for (int i = 2; i < gvDetailsView.Rows.Count; i++)
            {
                if (gvDetailsView.Rows[i].Visible == true)
                {
                    mySql += "<tr>";
                    for (int j = 1; j < gvDetailsView.Columns.Count; j++)
                    {
                        if (j != 8)
                            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + gvDetailsView.Rows[i].Cells[j].Text + "</font></td>";
                    }
                    mySql += "</tr>";
                }
            }
            
            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            return reportStr = mySql;
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void optBillBooking_CheckedChanged(object sender, EventArgs e)
        {
            gvDetailsView.DataSource = null;
            gvDetailsView.DataBind();
            lblStatus.Visible = true;
            ddlStatus.Visible = true;
        }

        protected void optCashPayment_CheckedChanged(object sender, EventArgs e)
        {
            gvDetailsView.DataSource = null;
            gvDetailsView.DataBind();
            lblStatus.Visible = false;
            ddlStatus.Visible = false;
        }

    }
}