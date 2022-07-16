using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class CashPayment_Modify : System.Web.UI.Page
    {

        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (!IsPostBack)
            {
                Label lblheader = (Label)Master.FindControl("lblHeading");
                lblheader.Text = "Modify Cash Payment";
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
                LoadCashPaymentList();
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = true;
                lblMsg.Text = "To Date should be Greater than or equal to the From Date";
                gvDetailsView.Visible = false;
            }
        }
        private void LoadCashPaymentList()
        {
            gvDetailsView.Visible = true;
            gvDetailsView.DataSource = null;
            gvDetailsView.DataBind();

            DateTime From_Dtae = DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null);
            DateTime To_Dtae = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);
            DataTable dt = new DataTable();
            DataRow dr1 = null;
            dt.Columns.Add(new DataColumn("CASHBANKPAY_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CASHBANKPAY_VoucherNo_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CASHBANKPAY_VoucherDate_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("CASHBANKPAY_TotalAmount_num", typeof(string)));

            var result = dc.CashBankPayment_View(0, "", From_Dtae, To_Dtae);
            foreach (var cashPay in result)
            {
                dr1 = dt.NewRow();
                dr1["CASHBANKPAY_Id"] = cashPay.CASHBANKPAY_Id;
                dr1["CASHBANKPAY_VoucherNo_var"] = cashPay.CASHBANKPAY_VoucherNo_var;
                dr1["CASHBANKPAY_VoucherDate_dt"] = Convert.ToDateTime(cashPay.CASHBANKPAY_VoucherDate_dt).ToString("dd/MM/yyyy");
                dr1["CASHBANKPAY_TotalAmount_num"] = cashPay.CASHBANKPAY_TotalAmount_num;
                dt.Rows.Add(dr1);
            }
            gvDetailsView.DataSource = dt;
            gvDetailsView.DataBind();

        }
        protected void gvDetailsView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] arg = new string[2];
            arg = e.CommandArgument.ToString().Split(';');

            EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
            string strURLWithData = "CashPayment.aspx?" + obj.Encrypt(string.Format("CashPaymentId={0}", arg[0]));
            Response.Redirect(strURLWithData); 
            //PrintGrid.Redirect1(strURLWithData, "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");

        }
        protected void lnkAddNew_Click(object sender, EventArgs e)
        {
            string strURLWithData = "CashPayment.aspx"; 
            //PrintGrid.Redirect1(strURLWithData, "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            Response.Redirect(strURLWithData);
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (gvDetailsView.Rows.Count > 0 && gvDetailsView.Visible == true)
            {
                string reportStr = "";
                reportStr = rptModifyCashPayment();
                PrintHTMLReport rptHtml = new PrintHTMLReport();
                string strFileName = "";
                strFileName = "CashPaymentReport";
                rptHtml.DownloadHtmlReport(strFileName, reportStr);
            }
        }

        protected string rptModifyCashPayment()
        {
            string reportStr = "", mySql = "", tempSql = "", strHeading = "";
            strHeading = "Cash Payment List";
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

        
    }
}