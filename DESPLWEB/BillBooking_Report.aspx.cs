using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


namespace DESPLWEB
{
    public partial class BillBooking_Report : System.Web.UI.Page
    {

        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (!IsPostBack)
            {
                Label lblheader = (Label)Master.FindControl("lblHeading");
                lblheader.Text = "Bill Booking Report";
                
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
                LoadBillBooking();
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = true;
                lblMsg.Text = "To Date should be Greater than or equal to the From Date";
                gvDetailsView.Visible = false;
            }
        }
        private void LoadBillBooking()
        {
            gvDetailsView.Visible = true;
            gvDetailsView.DataSource = null;
            gvDetailsView.DataBind();

            DateTime From_Date = DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null);
            DateTime To_Date = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);
            DataTable dt = new DataTable();
            DataRow dr1 = null;
            dt.Columns.Add(new DataColumn("NameoftheSupplier", typeof(string)));
            dt.Columns.Add(new DataColumn("SupplierGSTIN", typeof(string)));
            dt.Columns.Add(new DataColumn("PlaceofSupply", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceNo", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalInvoiceValue", typeof(string)));
            dt.Columns.Add(new DataColumn("TaxableValue", typeof(string)));
            dt.Columns.Add(new DataColumn("GSTRate", typeof(string)));
            dt.Columns.Add(new DataColumn("IGSTAmountofTaxPaid", typeof(string)));
            dt.Columns.Add(new DataColumn("CGSTAmountofTaxPaid", typeof(string)));
            dt.Columns.Add(new DataColumn("SGST/UTAmountofTaxPaid", typeof(string)));
            dt.Columns.Add(new DataColumn("CessAmountofTaxPaid", typeof(string)));
            dt.Columns.Add(new DataColumn("EligibilityforITC", typeof(string)));
            dt.Columns.Add(new DataColumn("IGSTAmountofITCavailable", typeof(string)));
            dt.Columns.Add(new DataColumn("CGSTAmountofITCavailable", typeof(string)));
            dt.Columns.Add(new DataColumn("SGST/UTAmountITCavailable", typeof(string)));
            dt.Columns.Add(new DataColumn("CessAmountofITCavailable", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceType", typeof(string)));
            dt.Columns.Add(new DataColumn("ReverseCharge", typeof(string)));
            dt.Columns.Add(new DataColumn("HSN", typeof(string)));
            dt.Columns.Add(new DataColumn("SAC", typeof(string)));
            dt.Columns.Add(new DataColumn("UQC", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(string)));

            var result = dc.BillBooking_View_Report(From_Date, To_Date);
            foreach (var billb in result)
            {
                decimal totalInvoiceValue = 0, gstRate = 0;
                totalInvoiceValue = Convert.ToDecimal(billb.BOOKDETAIL_Amount_num) + Convert.ToDecimal(billb.BOOKDETAIL_IgstAmount_num) + Convert.ToDecimal(billb.BOOKDETAIL_CgstAmount_num) + Convert.ToDecimal(billb.BOOKDETAIL_SgstAmount_num);
                gstRate = Convert.ToDecimal(billb.BOOKDETAIL_IgstPercent_num) + Convert.ToDecimal(billb.BOOKDETAIL_CgstPercent_num) + Convert.ToDecimal(billb.BOOKDETAIL_SgstPercent_num);
                dr1 = dt.NewRow();
                dr1["NameoftheSupplier"] = billb.VEND_FirmName_var;
                dr1["SupplierGSTIN"] = billb.VEND_GSTNo_var;                
                dr1["PlaceofSupply"] = "";
                dr1["InvoiceNo"] = billb.BILLBOOK_SupplierInvoiceNo_var;
                dr1["InvoiceDate"] = Convert.ToDateTime(billb.BILLBOOK_InvoiceDate_dt).ToString("dd/MM/yyyy"); ;
                dr1["TotalInvoiceValue"] = totalInvoiceValue;
                dr1["TaxableValue"] = billb.BOOKDETAIL_Amount_num;
                dr1["GSTRate"] = gstRate;
                dr1["IGSTAmountofTaxPaid"] = billb.BOOKDETAIL_IgstAmount_num;
                dr1["CGSTAmountofTaxPaid"] = billb.BOOKDETAIL_CgstAmount_num;
                dr1["SGST/UTAmountofTaxPaid"] = billb.BOOKDETAIL_SgstAmount_num;
                dr1["CessAmountofTaxPaid"] = "";
                dr1["EligibilityforITC"] = billb.BOOKDETAIL_Type_var;
                dr1["IGSTAmountofITCavailable"] = "";
                dr1["CGSTAmountofITCavailable"] = "";
                dr1["SGST/UTAmountITCavailable"] = "";
                dr1["CessAmountofITCavailable"] = "";
                dr1["InvoiceType"] = billb.BILLBOOK_Type_var;
                dr1["ReverseCharge"] = "";
                dr1["HSN"] = billb.BOOKDETAIL_HSNcode_var;
                dr1["SAC"] = billb.BOOKDETAIL_SACcode_var;
                dr1["UQC"] = billb.BOOKDETAIL_UQC_var;
                dr1["Description"] = billb.BOOKDETAIL_Description_var;
                dr1["Quantity"] = billb.BOOKDETAIL_Qty_num;
                dt.Rows.Add(dr1);
            }
            gvDetailsView.DataSource = dt;
            gvDetailsView.DataBind();            
            
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (gvDetailsView.Rows.Count > 0 && gvDetailsView.Visible == true)
            {
                //string reportStr = "";
                //reportStr = rptBillBooking();
                //PrintHTMLReport rptHtml = new PrintHTMLReport();
                //string strFileName = "";
                //strFileName = "BillBookingReport";
                //rptHtml.DownloadHtmlReport(strFileName, reportStr);

                HttpContext context = HttpContext.Current;
                //Export data to excel from gridview
                context.Response.ClearContent();
                context.Response.Buffer = true;
                context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "billBookingReport.xls"));
                context.Response.ContentType = "application/ms-excel";
                //DataTable dt = BindDatatable();
                string str = string.Empty;
                string subTitle="Period : "+ DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + " - " + DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy");
                if (subTitle.Contains("~") != true)
                {
                    context.Response.Write("\t" + subTitle);
                }
                else
                {
                    string[] strData = subTitle.Split('~');
                    foreach (string strTemp in strData)
                    {
                        context.Response.Write("\t" + strTemp);
                        context.Response.Write("\n");
                    }
                }
                context.Response.Write("\n");
                context.Response.Write("\n");
                //foreach (DataColumn dtcol in dt.Columns)
                for (int col = 0; col < gvDetailsView.Columns.Count; col++)
                {
                    if (gvDetailsView.Columns[col].Visible == true)
                    {
                        context.Response.Write(str + gvDetailsView.Columns[col].HeaderText);
                        str = "\t";
                    }
                }
                context.Response.Write("\n");
                for (int row = 0; row < gvDetailsView.Rows.Count; row++)
                {
                    str = "";
                    for (int col = 0; col < gvDetailsView.Columns.Count; col++)
                    {
                        if (gvDetailsView.Columns[col].Visible == true)
                        {
                            if (gvDetailsView.Rows[row].Cells[col].Text != "&nbsp;")
                            {
                                context.Response.Write(str + " " + gvDetailsView.Rows[row].Cells[col].Text);
                            }
                            else
                            {
                                context.Response.Write(str);
                            }
                            str = "\t";
                        }
                    }
                    context.Response.Write("\n");
                }
                context.Response.End();

            }
        }
        protected string rptBillBooking()
        {
            string reportStr = "", mySql = "", tempSql = "", strHeading = "";            
            strHeading = "Bill Booking Report";
            
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
            for (int i = 0; i < gvDetailsView.Columns.Count; i++)
            {
                mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>" + gvDetailsView.Columns[i].HeaderText + " </b></font></td>";
            }
            mySql += "</tr>";

            for (int i = 0; i < gvDetailsView.Rows.Count; i++)
            {
                if (gvDetailsView.Rows[i].Visible == true)
                {
                    mySql += "<tr>";
                    for (int j = 0; j < gvDetailsView.Columns.Count; j++)
                    {
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