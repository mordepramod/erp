using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class TallyTransfer_BillBooking : System.Web.UI.Page
    {

        LabDataDataContext dc = new LabDataDataContext();
        static string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Tally Transfer - Bill Booking";
                txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                optJournal.Checked = true;
            }
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadList();
        }

        protected void LoadList()
        {
            grdBillBooking.DataSource = null;
            grdBillBooking.DataBind();

            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);

            if (optJournal.Checked == true || optPurchase.Checked == true)
            {
                #region display bill booking of type journal , purchase
                string strType = "";
                if (optJournal.Checked == true)
                {
                    strType = "Journal";
                }
                else if (optPurchase.Checked == true)
                {
                    strType = "Purchase";
                }
                DataTable dt = new DataTable();
                var billb = dc.BillBooking_View_TallyTr(0, Fromdate, Todate, strType).ToList();
                for (int i = 0; i < billb.Count(); i++)
                {
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add(new DataColumn("lblBookingNo", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBookDate", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblSupplierInvoiceNo", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblInvoiceDate", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblTotalNetPayble", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblType", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblVendor", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblVendorId", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblNarration", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblComment", typeof(string)));
                    }
                    DataRow NewRow = dt.NewRow();
                    dt.Rows.Add(NewRow);
                    grdBillBooking.DataSource = dt;
                    grdBillBooking.DataBind();
                }
                int rowNo = 0;
                foreach (var bb in billb)
                {
                    Label lblBookingNo = (Label)grdBillBooking.Rows[rowNo].FindControl("lblBookingNo");
                    Label lblBookDate = (Label)grdBillBooking.Rows[rowNo].FindControl("lblBookDate");
                    Label lblSupplierInvoiceNo = (Label)grdBillBooking.Rows[rowNo].FindControl("lblSupplierInvoiceNo");
                    Label lblInvoiceDate = (Label)grdBillBooking.Rows[rowNo].FindControl("lblInvoiceDate");
                    Label lblTotalNetPayble = (Label)grdBillBooking.Rows[rowNo].FindControl("lblTotalNetPayble");
                    Label lblType = (Label)grdBillBooking.Rows[rowNo].FindControl("lblType");
                    Label lblVendor = (Label)grdBillBooking.Rows[rowNo].FindControl("lblVendor");
                    Label lblVendorId = (Label)grdBillBooking.Rows[rowNo].FindControl("lblVendorId");
                    Label lblNarration = (Label)grdBillBooking.Rows[rowNo].FindControl("lblNarration");
                    Label lblComment = (Label)grdBillBooking.Rows[rowNo].FindControl("lblComment");

                    lblBookingNo.Text = bb.BILLBOOK_Id.ToString();
                    lblBookDate.Text = Convert.ToDateTime(bb.BILLBOOK_Date_dt).ToString("dd/MM/yyyy");
                    lblSupplierInvoiceNo.Text = bb.BILLBOOK_SupplierInvoiceNo_var;
                    lblInvoiceDate.Text = Convert.ToDateTime(bb.BILLBOOK_InvoiceDate_dt).ToString("dd/MM/yyyy");
                    lblTotalNetPayble.Text = Convert.ToDecimal(bb.BILLBOOK_NetPayableAmount_num).ToString("0.00");
                    lblType.Text = bb.BILLBOOK_Type_var;
                    lblVendor.Text = bb.VEND_FirmName_var.ToString();
                    lblVendorId.Text = bb.BILLBOOK_VEND_Id.ToString();
                    lblNarration.Text = bb.BILLBOOK_Narration_var;
                    lblComment.Text = bb.BILLBOOK_Comment_var;

                    rowNo++;
                }

                if (grdBillBooking.Rows.Count > 0)
                {
                    grdBillBooking.HeaderRow.Cells[1].Text = "Booking No.";
                    grdBillBooking.HeaderRow.Cells[2].Text = "Book Date";
                    grdBillBooking.HeaderRow.Cells[3].Text = "Supplier Invoice No.";
                    grdBillBooking.HeaderRow.Cells[4].Text = "Invoice Date";
                    grdBillBooking.HeaderRow.Cells[5].Text = "Total Net Payble";
                    grdBillBooking.HeaderRow.Cells[6].Text = "Type";
                    grdBillBooking.HeaderRow.Cells[7].Text = "Vendor";
                    grdBillBooking.HeaderRow.Cells[8].Text = "Narration";
                    grdBillBooking.HeaderRow.Cells[9].Text = "Comment";
                }
                #endregion
            }
            else if (optCashPayment.Checked == true || optBankPayment.Checked == true)
            {
                #region display vendor payment of type cash , bank
                string strType = "";
                if (optJournal.Checked == true)
                {
                    strType = "Cash";
                }
                else if (optPurchase.Checked == true)
                {
                    strType = "Bank";
                }
                DataTable dt = new DataTable();
                //var billb = dc.CashPayment_View(0, "", Fromdate, Todate, strType).ToList();
                var billb = dc.CashPayment_View(0, "", Fromdate, Todate).ToList();
                for (int i = 0; i < billb.Count(); i++)
                {
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add(new DataColumn("lblBookingNo", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblBookDate", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblSupplierInvoiceNo", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblInvoiceDate", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblTotalNetPayble", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblType", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblVendor", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblVendorId", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblNarration", typeof(string)));
                        dt.Columns.Add(new DataColumn("lblComment", typeof(string)));
                    }
                    DataRow NewRow = dt.NewRow();
                    dt.Rows.Add(NewRow);
                    grdBillBooking.DataSource = dt;
                    grdBillBooking.DataBind();
                }
                int rowNo = 0;
                foreach (var bb in billb)
                {
                    Label lblBookingNo = (Label)grdBillBooking.Rows[rowNo].FindControl("lblBookingNo");
                    Label lblBookDate = (Label)grdBillBooking.Rows[rowNo].FindControl("lblBookDate");
                    Label lblSupplierInvoiceNo = (Label)grdBillBooking.Rows[rowNo].FindControl("lblSupplierInvoiceNo");
                    Label lblInvoiceDate = (Label)grdBillBooking.Rows[rowNo].FindControl("lblInvoiceDate");
                    Label lblTotalNetPayble = (Label)grdBillBooking.Rows[rowNo].FindControl("lblTotalNetPayble");
                    Label lblType = (Label)grdBillBooking.Rows[rowNo].FindControl("lblType");
                    Label lblVendor = (Label)grdBillBooking.Rows[rowNo].FindControl("lblVendor");
                    Label lblVendorId = (Label)grdBillBooking.Rows[rowNo].FindControl("lblVendorId");
                    Label lblNarration = (Label)grdBillBooking.Rows[rowNo].FindControl("lblNarration");
                    Label lblComment = (Label)grdBillBooking.Rows[rowNo].FindControl("lblComment");

                    lblBookingNo.Text = bb.CASHPAY_Id.ToString();
                    lblBookDate.Text = Convert.ToDateTime(bb.CASHPAY_VoucherDate_dt).ToString("dd/MM/yyyy");
                    lblSupplierInvoiceNo.Text = bb.CASHPAY_VoucherNo_var;
                    lblInvoiceDate.Text = Convert.ToDateTime(bb.CASHPAY_VoucherDate_dt).ToString("dd/MM/yyyy");
                    lblTotalNetPayble.Text = Convert.ToDecimal(bb.CASHPAY_TotalAmount_num).ToString("0.00");
                    //lblType.Text = bb.CASHPAY_PaymentStatus_var;
                    lblVendor.Text = bb.VEND_FirmName_var.ToString();
                    lblVendorId.Text = bb.CASHPAY_VEND_Id.ToString();
                    lblNarration.Text = bb.CASHPAY_Narration_var;
                    //lblComment.Text = bb.CASHPAY_BankVoucherType_var;

                    rowNo++;
                }
                if (grdBillBooking.Rows.Count > 0)
                {
                    grdBillBooking.HeaderRow.Cells[1].Text = "Cash Payment No.";
                    grdBillBooking.HeaderRow.Cells[2].Text = "Voucher Date";
                    grdBillBooking.HeaderRow.Cells[3].Text = "Voucher No.";
                    grdBillBooking.HeaderRow.Cells[4].Text = "Voucher Date";
                    grdBillBooking.HeaderRow.Cells[5].Text = "Total Amount";
                    grdBillBooking.HeaderRow.Cells[6].Text = "Payment Status";
                    grdBillBooking.HeaderRow.Cells[7].Text = "Vendor";
                    grdBillBooking.HeaderRow.Cells[8].Text = "Narration";
                    grdBillBooking.HeaderRow.Cells[9].Text = "Bank Voucher Type";
                }
                #endregion
            }
        }


        protected void lnkTransfer_Click(object sender, EventArgs e)
        {
            bool selectedFlag = false;
            if (grdBillBooking.Rows.Count > 0)
            {
                for (int i = 0; i < grdBillBooking.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdBillBooking.Rows[i].FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        selectedFlag = true;
                        break;
                    }
                }
            }
            if (selectedFlag == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select at least one row');", true);
            }
            else
            {
                if (optJournal.Checked == true)
                {
                    JournalTallyTransfer();
                }
                else if (optPurchase.Checked == true)
                {
                    PurchaseTallyTransfer();
                }
                else if (optCashPayment.Checked == true)
                {
                    VendorCashPaymentTallyTransfer();
                }
                else if (optBankPayment.Checked == true)
                {
                    //VendorBankPaymentTallyTransfer();
                }
            }
        }

        private void JournalTallyTransfer()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
            xmldoc.AppendChild(RootNode);

            XmlElement HEADER = xmldoc.CreateElement("HEADER");
            XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
            TALLYREQUEST.InnerText = "Import Data";
            HEADER.AppendChild(TALLYREQUEST);
            RootNode.AppendChild(HEADER);

            XmlElement BODY = xmldoc.CreateElement("BODY");
            XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
            XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
            XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
            REPORTNAME.InnerText = "Vouchers";
            REQUESTDESC.AppendChild(REPORTNAME);

            XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
            XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");
            //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
            //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
            //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
            if (cnStr.ToLower().Contains("mumbai") == true)
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
            else if (cnStr.ToLower().Contains("nashik") == true)
                SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
            else
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2015";
            STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
            REQUESTDESC.AppendChild(STATICVARIABLES);
            XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
            #region JOURNL
            for (int i = 0; i < grdBillBooking.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdBillBooking.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblBookingNo = (Label)grdBillBooking.Rows[i].FindControl("lblBookingNo");
                    Label lblBookDate = (Label)grdBillBooking.Rows[i].FindControl("lblBookDate");
                    Label lblSupplierInvoiceNo = (Label)grdBillBooking.Rows[i].FindControl("lblSupplierInvoiceNo");
                    Label lblInvoiceDate = (Label)grdBillBooking.Rows[i].FindControl("lblInvoiceDate");
                    Label lblTotalNetPayble = (Label)grdBillBooking.Rows[i].FindControl("lblTotalNetPayble");
                    Label lblType = (Label)grdBillBooking.Rows[i].FindControl("lblType");
                    Label lblVendor = (Label)grdBillBooking.Rows[i].FindControl("lblVendor");
                    Label lblNarration = (Label)grdBillBooking.Rows[i].FindControl("lblNarration");
                    Label lblComment = (Label)grdBillBooking.Rows[i].FindControl("lblComment");

                    XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
                    XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
                    double CreditAmt = 0;
                    var billBookingDetail = dc.BillBookingDetail_View(Convert.ToInt32(lblBookingNo.Text)).ToList();
                    foreach (var bbd in billBookingDetail)
                    {

                        TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");
                        VOUCHER.SetAttribute("REMOTEID", "");
                        VOUCHER.SetAttribute("VCHKEY", "");
                        VOUCHER.SetAttribute("VCHTYPE", "Journal 1");
                        VOUCHER.SetAttribute("ACTION", "Create");
                        VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

                        XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSf.InnerText = "-1";
                        OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
                        VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

                        XmlElement DATE = xmldoc.CreateElement("DATE");
                        DateTime date = DateTime.ParseExact(lblBookDate.Text, "dd/MM/yyyy", null);
                        DATE.InnerText = date.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(DATE);

                        XmlElement GUID = xmldoc.CreateElement("GUID");
                        GUID.InnerText = "";
                        VOUCHER.AppendChild(GUID);

                        XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
                        NARRATION.InnerText = lblNarration.Text;
                        VOUCHER.AppendChild(NARRATION);

                        XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
                        if (cnStr.ToLower().Contains("nashik") == true)
                            VOUCHERTYPENAME.InnerText = "Journal A";
                        else
                            VOUCHERTYPENAME.InnerText = "Journal 1";
                        VOUCHER.AppendChild(VOUCHERTYPENAME);

                        XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
                        VOUCHERNUMBER.InnerText = lblSupplierInvoiceNo.Text;
                        VOUCHER.AppendChild(VOUCHERNUMBER);

                        XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
                        PARTYLEDGERNAME.InnerText = lblVendor.Text.Replace("&", "AND");
                        VOUCHER.AppendChild(PARTYLEDGERNAME);
                        XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
                        VOUCHER.AppendChild(CSTFORMISSUETYPE);
                        XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
                        VOUCHER.AppendChild(CSTFORMRECVTYPE);
                        XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
                        FBTPAYMENTTYPE.InnerText = "Default";
                        VOUCHER.AppendChild(FBTPAYMENTTYPE);

                        XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
                        PERSISTEDVIEW.InnerText = "Accounting Voucher View";
                        VOUCHER.AppendChild(PERSISTEDVIEW);
                        XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
                        VOUCHER.AppendChild(VCHGSTCLASS);

                        XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
                        ENTEREDBY.InnerText = "DESPL";
                        VOUCHER.AppendChild(ENTEREDBY);

                        //XmlElement VOUCHERTYPEORIGNAME = xmldoc.CreateElement("VOUCHERTYPEORIGNAME");
                        //VOUCHERTYPEORIGNAME.InnerText = "Journal";
                        //VOUCHER.AppendChild(VOUCHERTYPEORIGNAME);

                        XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
                        DIFFACTUALQTY.InnerText = "No";
                        VOUCHER.AppendChild(DIFFACTUALQTY);

                        XmlElement ISMSTFROMSYNC = xmldoc.CreateElement("ISMSTFROMSYNC");
                        ISMSTFROMSYNC.InnerText = "No";
                        VOUCHER.AppendChild(ISMSTFROMSYNC);

                        XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        ASORIGINAL.InnerText = "No";
                        VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
                        AUDITED.InnerText = "No";
                        VOUCHER.AppendChild(AUDITED);

                        XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
                        FORJOBCOSTING.InnerText = "No";
                        VOUCHER.AppendChild(FORJOBCOSTING);

                        XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
                        ISOPTIONAL.InnerText = "No";
                        VOUCHER.AppendChild(ISOPTIONAL);

                        XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
                        DateTime date2 = DateTime.ParseExact(lblBookDate.Text, "dd/MM/yyyy", null);
                        EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(EFFECTIVEDATE);

                        XmlElement USEFOREXCISE = xmldoc.CreateElement("USEFOREXCISE");
                        USEFOREXCISE.InnerText = "No";
                        VOUCHER.AppendChild(USEFOREXCISE);

                        XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
                        ISFORJOBWORKIN.InnerText = "No";
                        VOUCHER.AppendChild(ISFORJOBWORKIN);

                        XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
                        ALLOWCONSUMPTION.InnerText = "No";
                        VOUCHER.AppendChild(ALLOWCONSUMPTION);

                        XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
                        USEFORINTEREST.InnerText = "No";
                        VOUCHER.AppendChild(USEFORINTEREST);

                        XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
                        USEFORGAINLOSS.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGAINLOSS);

                        XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
                        USEFORGODOWNTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

                        XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                        USEFORCOMPOUND.InnerText = "No";
                        VOUCHER.AppendChild(USEFORCOMPOUND);

                        XmlElement USEFORSERVICETAX = xmldoc.CreateElement("USEFORSERVICETAX");
                        USEFORSERVICETAX.InnerText = "No";
                        VOUCHER.AppendChild(USEFORSERVICETAX);


                        XmlElement ISEXCISEVOUCHER = xmldoc.CreateElement("ISEXCISEVOUCHER");
                        ISEXCISEVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEVOUCHER);

                        XmlElement EXCISETAXOVERRIDE = xmldoc.CreateElement("EXCISETAXOVERRIDE");
                        EXCISETAXOVERRIDE.InnerText = "No";
                        VOUCHER.AppendChild(EXCISETAXOVERRIDE);

                        XmlElement USEFORTAXUNITTRANSFER = xmldoc.CreateElement("USEFORTAXUNITTRANSFER");
                        USEFORTAXUNITTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORTAXUNITTRANSFER);

                        XmlElement IGNOREPOSVALIDATION = xmldoc.CreateElement("IGNOREPOSVALIDATION");
                        IGNOREPOSVALIDATION.InnerText = "No";
                        VOUCHER.AppendChild(IGNOREPOSVALIDATION);

                        XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
                        EXCISEOPENING.InnerText = "No";
                        VOUCHER.AppendChild(EXCISEOPENING);

                        XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
                        USEFORFINALPRODUCTION.InnerText = "No";
                        VOUCHER.AppendChild(USEFORFINALPRODUCTION);

                        XmlElement ISTDSOVERRIDDEN = xmldoc.CreateElement("ISTDSOVERRIDDEN");
                        ISTDSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSOVERRIDDEN);

                        XmlElement ISTCSOVERRIDDEN = xmldoc.CreateElement("ISTCSOVERRIDDEN");
                        ISTCSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTCSOVERRIDDEN);

                        XmlElement ISTDSTCSCASHVCH = xmldoc.CreateElement("ISTDSTCSCASHVCH");
                        ISTDSTCSCASHVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSTCSCASHVCH);

                        XmlElement INCLUDEADVPYMTVCH = xmldoc.CreateElement("INCLUDEADVPYMTVCH");
                        INCLUDEADVPYMTVCH.InnerText = "No";
                        VOUCHER.AppendChild(INCLUDEADVPYMTVCH);

                        XmlElement ISSUBWORKSCONTRACT = xmldoc.CreateElement("ISSUBWORKSCONTRACT");
                        ISSUBWORKSCONTRACT.InnerText = "No";
                        VOUCHER.AppendChild(ISSUBWORKSCONTRACT);

                        XmlElement ISVATOVERRIDDEN = xmldoc.CreateElement("ISVATOVERRIDDEN");
                        ISVATOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISVATOVERRIDDEN);

                        XmlElement IGNOREORIGVCHDATE = xmldoc.CreateElement("IGNOREORIGVCHDATE");
                        IGNOREORIGVCHDATE.InnerText = "No";
                        VOUCHER.AppendChild(IGNOREORIGVCHDATE);

                        XmlElement ISVATPAIDATCUSTOMS = xmldoc.CreateElement("ISVATPAIDATCUSTOMS");
                        ISVATPAIDATCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPAIDATCUSTOMS);

                        XmlElement ISDECLAREDTOCUSTOMS = xmldoc.CreateElement("ISDECLAREDTOCUSTOMS");
                        ISDECLAREDTOCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISDECLAREDTOCUSTOMS);

                        XmlElement ISSERVICETAXOVERRIDDEN = xmldoc.CreateElement("ISSERVICETAXOVERRIDDEN");
                        ISSERVICETAXOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISSERVICETAXOVERRIDDEN);

                        XmlElement ISISDVOUCHER = xmldoc.CreateElement("ISISDVOUCHER");
                        ISISDVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISISDVOUCHER);

                        XmlElement ISEXCISEOVERRIDDEN = xmldoc.CreateElement("ISEXCISEOVERRIDDEN");
                        ISEXCISEOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEOVERRIDDEN);

                        XmlElement ISEXCISESUPPLYVCH = xmldoc.CreateElement("ISEXCISESUPPLYVCH");
                        ISEXCISESUPPLYVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISESUPPLYVCH);

                        XmlElement ISGSTOVERRIDDEN = xmldoc.CreateElement("ISGSTOVERRIDDEN");
                        ISGSTOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISGSTOVERRIDDEN);


                        XmlElement GSTNOTEEXPORTED = xmldoc.CreateElement("GSTNOTEXPORTED");
                        GSTNOTEEXPORTED.InnerText = "No";
                        VOUCHER.AppendChild(GSTNOTEEXPORTED);

                        XmlElement IGNOREGSTINVALIDATION = xmldoc.CreateElement("IGNOREGSTINVALIDATION");
                        IGNOREGSTINVALIDATION.InnerText = "No";
                        VOUCHER.AppendChild(IGNOREGSTINVALIDATION);

                        XmlElement ISVATPRINCIPALACCOUNT = xmldoc.CreateElement("ISVATPRINCIPALACCOUNT");
                        ISVATPRINCIPALACCOUNT.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPRINCIPALACCOUNT);

                        XmlElement ISBOENOTAPPLICABLE = xmldoc.CreateElement("ISBOENOTAPPLICABLE");
                        ISBOENOTAPPLICABLE.InnerText = "No";
                        VOUCHER.AppendChild(ISBOENOTAPPLICABLE);

                        XmlElement ISSHIPPINGWITHINSTATE = xmldoc.CreateElement("ISSHIPPINGWITHINSTATE");
                        ISSHIPPINGWITHINSTATE.InnerText = "No";
                        VOUCHER.AppendChild(ISSHIPPINGWITHINSTATE);

                        XmlElement ISOVERSEASTOURISTTRANS = xmldoc.CreateElement("ISOVERSEASTOURISTTRANS");
                        ISOVERSEASTOURISTTRANS.InnerText = "No";
                        VOUCHER.AppendChild(ISOVERSEASTOURISTTRANS);

                        XmlElement ISDESIGNATEDZONEPARTY = xmldoc.CreateElement("ISDESIGNATEDZONEPARTY");
                        ISDESIGNATEDZONEPARTY.InnerText = "No";
                        VOUCHER.AppendChild(ISDESIGNATEDZONEPARTY);

                        XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
                        ISCANCELLED.InnerText = "No";
                        VOUCHER.AppendChild(ISCANCELLED);

                        XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
                        HASCASHFLOW.InnerText = "No";
                        VOUCHER.AppendChild(HASCASHFLOW);

                        XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
                        ISPOSTDATED.InnerText = "No";
                        VOUCHER.AppendChild(ISPOSTDATED);

                        XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
                        USETRACKINGNUMBER.InnerText = "No";
                        VOUCHER.AppendChild(USETRACKINGNUMBER);

                        XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
                        ISINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISINVOICE);

                        XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
                        MFGJOURNAL.InnerText = "No";
                        VOUCHER.AppendChild(MFGJOURNAL);

                        XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
                        HASDISCOUNTS.InnerText = "No";
                        VOUCHER.AppendChild(HASDISCOUNTS);

                        XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
                        ASPAYSLIP.InnerText = "No";
                        VOUCHER.AppendChild(ASPAYSLIP);

                        XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
                        ISCOSTCENTRE.InnerText = "No";
                        VOUCHER.AppendChild(ISCOSTCENTRE);

                        XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
                        ISSTXNONREALIZEDVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

                        XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
                        ISEXCISEMANUFACTURERON.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

                        XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
                        ISBLANKCHEQUE.InnerText = "No";
                        VOUCHER.AppendChild(ISBLANKCHEQUE);

                        XmlElement ISVOID = xmldoc.CreateElement("ISVOID");
                        ISVOID.InnerText = "No";
                        VOUCHER.AppendChild(ISVOID);

                        XmlElement ISONHOLD = xmldoc.CreateElement("ISONHOLD");
                        ISONHOLD.InnerText = "No";
                        VOUCHER.AppendChild(ISONHOLD);

                        XmlElement ORDERLINESTATUS = xmldoc.CreateElement("ORDERLINESTATUS");
                        ORDERLINESTATUS.InnerText = "No";
                        VOUCHER.AppendChild(ORDERLINESTATUS);

                        XmlElement VATISAGNSTCANCSALES = xmldoc.CreateElement("VATISAGNSTCANCSALES");
                        VATISAGNSTCANCSALES.InnerText = "No";
                        VOUCHER.AppendChild(VATISAGNSTCANCSALES);

                        XmlElement VATISPURCEXEMPTED = xmldoc.CreateElement("VATISPURCEXEMPTED");
                        VATISPURCEXEMPTED.InnerText = "No";
                        VOUCHER.AppendChild(VATISPURCEXEMPTED);

                        XmlElement ISVATRESTAXINVOICE = xmldoc.CreateElement("ISVATRESTAXINVOICE");
                        ISVATRESTAXINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISVATRESTAXINVOICE);

                        XmlElement VATISASSESABLECALCVCH = xmldoc.CreateElement("VATISASSESABLECALCVCH");
                        VATISASSESABLECALCVCH.InnerText = "No";
                        VOUCHER.AppendChild(VATISASSESABLECALCVCH);

                        XmlElement ISVATDUTYPAID = xmldoc.CreateElement("ISVATDUTYPAID");
                        ISVATDUTYPAID.InnerText = "Yes";
                        VOUCHER.AppendChild(ISVATDUTYPAID);

                        XmlElement ISDELIVERYSAMEASCONSIGNEE = xmldoc.CreateElement("ISDELIVERYSAMEASCONSIGNEE");
                        ISDELIVERYSAMEASCONSIGNEE.InnerText = "No";
                        VOUCHER.AppendChild(ISDELIVERYSAMEASCONSIGNEE);

                        XmlElement ISDISPATCHSAMEASCONSIGNOR = xmldoc.CreateElement("ISDISPATCHSAMEASCONSIGNOR");
                        ISDISPATCHSAMEASCONSIGNOR.InnerText = "No";
                        VOUCHER.AppendChild(ISDISPATCHSAMEASCONSIGNOR);

                        XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
                        ISDELETED.InnerText = "No";
                        VOUCHER.AppendChild(ISDELETED);

                        //XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        //ASORIGINAL.InnerText = "No";
                        //VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement CHANGEVCHMODE = xmldoc.CreateElement("CHANGEVCHMODE");
                        CHANGEVCHMODE.InnerText = "No";
                        VOUCHER.AppendChild(CHANGEVCHMODE);

                        XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
                        XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
                        XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
                        XmlElement EXCLUDEDTAXATION = xmldoc.CreateElement("EXCLUDEDTAXATION.LIST");
                        XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement DUTYHEADDETAILS = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                        XmlElement SUPPLEMENTARYDUTYHEADDETAILS = xmldoc.CreateElement("SUPPLEMENTARYDUTYHEADDETAILS.LIST");
                        XmlElement EWAYBILLDETAILS = xmldoc.CreateElement("EWAYBILLDETAILS.LIST");
                        XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
                        XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
                        XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
                        XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");
                        XmlElement ORIGINVOICEDETAILS = xmldoc.CreateElement("ORIGINVOICEDETAILS.LIST");
                        XmlElement INVOICEEXPORTLIST = xmldoc.CreateElement("INVOICEEXPORTLIST.LIST");

                        ALTERID.InnerText = "";
                        VOUCHER.AppendChild(ALTERID);

                        MASTERID.InnerText = "";
                        VOUCHER.AppendChild(MASTERID);

                        VOUCHERKEY.InnerText = "";
                        VOUCHER.AppendChild(VOUCHERKEY);

                        VOUCHER.AppendChild(EXCLUDEDTAXATION);
                        VOUCHER.AppendChild(OLDAUDITENTRIES);
                        VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
                        VOUCHER.AppendChild(AUDITENTRIES);
                        VOUCHER.AppendChild(DUTYHEADDETAILS);
                        VOUCHER.AppendChild(SUPPLEMENTARYDUTYHEADDETAILS);
                        VOUCHER.AppendChild(EWAYBILLDETAILS);
                        VOUCHER.AppendChild(INVOICEDELNOTES);
                        VOUCHER.AppendChild(INVOICEORDERLIST);
                        VOUCHER.AppendChild(INVOICEINDENTLIST);
                        VOUCHER.AppendChild(ATTENDANCEENTRIES);
                        VOUCHER.AppendChild(ORIGINVOICEDETAILS);
                        VOUCHER.AppendChild(INVOICEEXPORTLIST);

                        foreach (var bbd1 in billBookingDetail)
                        {
                            if (bbd1.BOOKDETAIL_DrCr_var == "Debit")
                            {
                                XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                                XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                                OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                                XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                                OLDAUDITENTRYIDSs.InnerText = "-1";
                                OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                                ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                                XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
                                LEDGERNAMEe.InnerText = bbd1.BOOKDETAIL_LedgerName_var;
                                ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                                XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                                ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                                XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                ISDEEMEDPOSITIVEa.InnerText = "Yes";
                                ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);

                                XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                                LEDGERFROMITEMa.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

                                XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                                REMOVEZEROENTRIESq.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

                                XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                                ISPARTYLEDGERw.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

                                XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                                //ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                                ISLASTDEEMEDPOSITIVEr.InnerText = "Yes";
                                ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

                                XmlElement ISCAPVATTAXALTEREDr = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                                ISCAPVATTAXALTEREDr.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(ISCAPVATTAXALTEREDr);

                                XmlElement ISCAPVATNOTCLAIMEDdr = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                                ISCAPVATNOTCLAIMEDdr.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(ISCAPVATNOTCLAIMEDdr);

                                XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
                                AMOUNTav.InnerText = "-" + bbd1.BOOKDETAIL_Amount_num.ToString();
                                ALLLEDGERENTRIESM.AppendChild(AMOUNTav);

                                XmlElement VATEXPAMOUNT = xmldoc.CreateElement("VATEXPAMOUNT");
                                VATEXPAMOUNT.InnerText = "-" + bbd1.BOOKDETAIL_Amount_num.ToString();
                                ALLLEDGERENTRIESM.AppendChild(VATEXPAMOUNT);

                                XmlElement SERVICETAXDETAILS = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                                ALLLEDGERENTRIESM.AppendChild(SERVICETAXDETAILS);

                                if (bbd1.BOOKDETAIL_Category_var != "---Select---" && bbd1.BOOKDETAIL_Category_var != "NA" && bbd1.BOOKDETAIL_Category_var != null)
                                {
                                    XmlElement CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
                                    XmlElement CATEGORY = xmldoc.CreateElement("CATEGORY");
                                    CATEGORY.InnerText = bbd1.BOOKDETAIL_Category_var;
                                    CATEGORYALLOCATIONS.AppendChild(CATEGORY);

                                    XmlElement ISDEEMEDPOSITIVEc = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                    ISDEEMEDPOSITIVEc.InnerText = "Yes";
                                    CATEGORYALLOCATIONS.AppendChild(ISDEEMEDPOSITIVEc);

                                    XmlElement COSTCENTREALLOCATIONS = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
                                    XmlElement NAMEc = xmldoc.CreateElement("NAME");
                                    NAMEc.InnerText = bbd1.BOOKDETAIL_CostCenter_var;
                                    COSTCENTREALLOCATIONS.AppendChild(NAMEc);

                                    XmlElement AMOUNTc = xmldoc.CreateElement("AMOUNT");
                                    AMOUNTc.InnerText = "-" + bbd1.BOOKDETAIL_Amount_num.ToString();
                                    COSTCENTREALLOCATIONS.AppendChild(AMOUNTc);

                                    CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONS);
                                    ALLLEDGERENTRIESM.AppendChild(CATEGORYALLOCATIONS);
                                }
                                XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                                XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                                XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                                XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                                XmlElement INPUTCRALLOCSs = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                                XmlElement DUTYHEADDETAILSs = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                                XmlElement EXCISEDUTYHEADDETAILSs = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                                XmlElement RATEDETAILSs = xmldoc.CreateElement("RATEDETAILS.LIST");
                                XmlElement SUMMARYALLOCSs = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                                XmlElement STPYMTDETAILSs = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                                XmlElement EXCISEPAYMENTALLOCATIONSs = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                                XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                                XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                                XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                                XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                                XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                                XmlElement REFVOUCHERDETAILs = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                                XmlElement INVOICEWISEDETAILs = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                                XmlElement VATITCDETAILs = xmldoc.CreateElement("VATITCDETAILS.LIST");
                                XmlElement ADVANCETAXDETAILs = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                                ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
                                ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
                                ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
                                ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
                                ALLLEDGERENTRIESM.AppendChild(INPUTCRALLOCSs);
                                ALLLEDGERENTRIESM.AppendChild(DUTYHEADDETAILSs);
                                ALLLEDGERENTRIESM.AppendChild(EXCISEDUTYHEADDETAILSs);
                                ALLLEDGERENTRIESM.AppendChild(RATEDETAILSs);
                                ALLLEDGERENTRIESM.AppendChild(SUMMARYALLOCSs);
                                ALLLEDGERENTRIESM.AppendChild(STPYMTDETAILSs);
                                ALLLEDGERENTRIESM.AppendChild(EXCISEPAYMENTALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
                                ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(REFVOUCHERDETAILs);
                                ALLLEDGERENTRIESM.AppendChild(INVOICEWISEDETAILs);
                                ALLLEDGERENTRIESM.AppendChild(VATITCDETAILs);
                                ALLLEDGERENTRIESM.AppendChild(ADVANCETAXDETAILs);
                                VOUCHER.AppendChild(ALLLEDGERENTRIESM);

                                //cgst
                                if (bbd1.BOOKDETAIL_CgstAmount_num > 0)
                                {
                                    XmlElement ALLLEDGERENTRIESMcg = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                                    XmlElement OLDAUDITENTRYIDSzcg = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                                    OLDAUDITENTRYIDSzcg.SetAttribute("TYPE", "Number");
                                    XmlElement OLDAUDITENTRYIDSscg = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                                    OLDAUDITENTRYIDSscg.InnerText = "-1";
                                    OLDAUDITENTRYIDSzcg.AppendChild(OLDAUDITENTRYIDSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(OLDAUDITENTRYIDSzcg);

                                    XmlElement LEDGERNAMEecg = xmldoc.CreateElement("LEDGERNAME");
                                    LEDGERNAMEecg.InnerText = "CGST " + bbd1.BOOKDETAIL_CgstPercent_num + "% Input Credit";
                                    ALLLEDGERENTRIESMcg.AppendChild(LEDGERNAMEecg);
                                    XmlElement GSTCLASSscg = xmldoc.CreateElement("GSTCLASS");
                                    ALLLEDGERENTRIESMcg.AppendChild(GSTCLASSscg);

                                    XmlElement ISDEEMEDPOSITIVEacg = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                    ISDEEMEDPOSITIVEacg.InnerText = "Yes";
                                    ALLLEDGERENTRIESMcg.AppendChild(ISDEEMEDPOSITIVEacg);

                                    XmlElement LEDGERFROMITEMacg = xmldoc.CreateElement("LEDGERFROMITEM");
                                    LEDGERFROMITEMacg.InnerText = "No";
                                    ALLLEDGERENTRIESMcg.AppendChild(LEDGERFROMITEMacg);

                                    XmlElement REMOVEZEROENTRIESqcg = xmldoc.CreateElement("REMOVEZEROENTRIES");
                                    REMOVEZEROENTRIESqcg.InnerText = "No";
                                    ALLLEDGERENTRIESMcg.AppendChild(REMOVEZEROENTRIESqcg);

                                    XmlElement ISPARTYLEDGERwcg = xmldoc.CreateElement("ISPARTYLEDGER");
                                    ISPARTYLEDGERwcg.InnerText = "No";
                                    ALLLEDGERENTRIESMcg.AppendChild(ISPARTYLEDGERwcg);

                                    XmlElement ISLASTDEEMEDPOSITIVErcg = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                                    ISLASTDEEMEDPOSITIVErcg.InnerText = "Yes";
                                    ALLLEDGERENTRIESMcg.AppendChild(ISLASTDEEMEDPOSITIVErcg);

                                    XmlElement ISCAPVATTAXALTEREDrcg = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                                    ISCAPVATTAXALTEREDrcg.InnerText = "No";
                                    ALLLEDGERENTRIESMcg.AppendChild(ISCAPVATTAXALTEREDrcg);

                                    XmlElement ISCAPVATNOTCLAIMEDdrcg = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                                    ISCAPVATNOTCLAIMEDdrcg.InnerText = "No";
                                    ALLLEDGERENTRIESMcg.AppendChild(ISCAPVATNOTCLAIMEDdrcg);

                                    XmlElement AMOUNTavcg = xmldoc.CreateElement("AMOUNT");
                                    AMOUNTavcg.InnerText = "-" + bbd1.BOOKDETAIL_CgstAmount_num.ToString();
                                    ALLLEDGERENTRIESMcg.AppendChild(AMOUNTavcg);

                                    XmlElement VATEXPAMOUNTcg = xmldoc.CreateElement("VATEXPAMOUNT");
                                    VATEXPAMOUNTcg.InnerText = "-" + bbd1.BOOKDETAIL_CgstAmount_num.ToString();
                                    ALLLEDGERENTRIESMcg.AppendChild(VATEXPAMOUNTcg);

                                    XmlElement SERVICETAXDETAILScg = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                                    ALLLEDGERENTRIESMcg.AppendChild(SERVICETAXDETAILScg);

                                    XmlElement BANKALLOCATIONSscg = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                                    XmlElement BILLALLOCATIONSscg = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                    XmlElement INTERESTCOLLECTIONscg = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                    XmlElement OLDAUDITENTRIESscg = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                                    XmlElement ACCOUNTAUDITENTRIESSscg = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                                    XmlElement AUDITENTRIESswcg = xmldoc.CreateElement("AUDITENTRIES.LIST");
                                    XmlElement INPUTCRALLOCSscg = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                                    XmlElement DUTYHEADDETAILSscg = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                                    XmlElement EXCISEDUTYHEADDETAILSscg = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                                    XmlElement RATEDETAILSscg = xmldoc.CreateElement("RATEDETAILS.LIST");
                                    XmlElement SUMMARYALLOCSscg = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                                    XmlElement STPYMTDETAILSscg = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                                    XmlElement EXCISEPAYMENTALLOCATIONSscg = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                                    XmlElement TAXBILLALLOCATIONSscg = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                                    XmlElement TAXOBJECTALLOCATIONSscg = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                                    XmlElement TDSEXPENSEALLOCATIONSscg = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                                    XmlElement VATSTATUTORYDETAILSSscg = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                                    XmlElement COSTTRACKALLOCATIONSscg = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                                    XmlElement REFVOUCHERDETAILscg = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                                    XmlElement INVOICEWISEDETAILscg = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                                    XmlElement VATITCDETAILscg = xmldoc.CreateElement("VATITCDETAILS.LIST");
                                    XmlElement ADVANCETAXDETAILscg = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                                    ALLLEDGERENTRIESMcg.AppendChild(BANKALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(BILLALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(INTERESTCOLLECTIONscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(OLDAUDITENTRIESscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(ACCOUNTAUDITENTRIESSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(AUDITENTRIESswcg);
                                    ALLLEDGERENTRIESMcg.AppendChild(INPUTCRALLOCSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(DUTYHEADDETAILSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(EXCISEDUTYHEADDETAILSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(RATEDETAILSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(SUMMARYALLOCSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(STPYMTDETAILSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(EXCISEPAYMENTALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(TAXBILLALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(TAXOBJECTALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(TDSEXPENSEALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(VATSTATUTORYDETAILSSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(COSTTRACKALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(REFVOUCHERDETAILscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(INVOICEWISEDETAILscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(VATITCDETAILscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(ADVANCETAXDETAILscg);
                                    VOUCHER.AppendChild(ALLLEDGERENTRIESMcg);
                                }
                                //
                                //sgst
                                if (bbd1.BOOKDETAIL_SgstAmount_num > 0)
                                {
                                    XmlElement ALLLEDGERENTRIESMsg = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                                    XmlElement OLDAUDITENTRYIDSzsg = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                                    OLDAUDITENTRYIDSzsg.SetAttribute("TYPE", "Number");
                                    XmlElement OLDAUDITENTRYIDSssg = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                                    OLDAUDITENTRYIDSssg.InnerText = "-1";
                                    OLDAUDITENTRYIDSzsg.AppendChild(OLDAUDITENTRYIDSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(OLDAUDITENTRYIDSzsg);

                                    XmlElement LEDGERNAMEesg = xmldoc.CreateElement("LEDGERNAME");
                                    LEDGERNAMEesg.InnerText = "SGST " + bbd1.BOOKDETAIL_SgstPercent_num + "% Input Credit";
                                    ALLLEDGERENTRIESMsg.AppendChild(LEDGERNAMEesg);
                                    XmlElement GSTCLASSssg = xmldoc.CreateElement("GSTCLASS");
                                    ALLLEDGERENTRIESMsg.AppendChild(GSTCLASSssg);

                                    XmlElement ISDEEMEDPOSITIVEasg = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                    ISDEEMEDPOSITIVEasg.InnerText = "Yes";
                                    ALLLEDGERENTRIESMsg.AppendChild(ISDEEMEDPOSITIVEasg);

                                    XmlElement LEDGERFROMITEMasg = xmldoc.CreateElement("LEDGERFROMITEM");
                                    LEDGERFROMITEMasg.InnerText = "No";
                                    ALLLEDGERENTRIESMsg.AppendChild(LEDGERFROMITEMasg);

                                    XmlElement REMOVEZEROENTRIESqsg = xmldoc.CreateElement("REMOVEZEROENTRIES");
                                    REMOVEZEROENTRIESqsg.InnerText = "No";
                                    ALLLEDGERENTRIESMsg.AppendChild(REMOVEZEROENTRIESqsg);

                                    XmlElement ISPARTYLEDGERwsg = xmldoc.CreateElement("ISPARTYLEDGER");
                                    ISPARTYLEDGERwsg.InnerText = "No";
                                    ALLLEDGERENTRIESMsg.AppendChild(ISPARTYLEDGERwsg);

                                    XmlElement ISLASTDEEMEDPOSITIVErsg = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                                    //ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                                    ISLASTDEEMEDPOSITIVErsg.InnerText = "Yes";
                                    ALLLEDGERENTRIESMsg.AppendChild(ISLASTDEEMEDPOSITIVErsg);

                                    XmlElement ISCAPVATTAXALTEREDrsg = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                                    ISCAPVATTAXALTEREDrsg.InnerText = "No";
                                    ALLLEDGERENTRIESMsg.AppendChild(ISCAPVATTAXALTEREDrsg);

                                    XmlElement ISCAPVATNOTCLAIMEDdrsg = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                                    ISCAPVATNOTCLAIMEDdrsg.InnerText = "No";
                                    ALLLEDGERENTRIESMsg.AppendChild(ISCAPVATNOTCLAIMEDdrsg);

                                    XmlElement AMOUNTavsg = xmldoc.CreateElement("AMOUNT");
                                    AMOUNTavsg.InnerText = "-" + bbd1.BOOKDETAIL_SgstAmount_num.ToString();
                                    ALLLEDGERENTRIESMsg.AppendChild(AMOUNTavsg);

                                    XmlElement VATEXPAMOUNTsg = xmldoc.CreateElement("VATEXPAMOUNT");
                                    VATEXPAMOUNTsg.InnerText = "-" + bbd1.BOOKDETAIL_SgstAmount_num.ToString();
                                    ALLLEDGERENTRIESMsg.AppendChild(VATEXPAMOUNTsg);

                                    XmlElement SERVICETAXDETAILSsg = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                                    ALLLEDGERENTRIESMsg.AppendChild(SERVICETAXDETAILSsg);

                                    XmlElement BANKALLOCATIONSssg = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                                    XmlElement BILLALLOCATIONSssg = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                    XmlElement INTERESTCOLLECTIONssg = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                    XmlElement OLDAUDITENTRIESssg = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                                    XmlElement ACCOUNTAUDITENTRIESSssg = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                                    XmlElement AUDITENTRIESswsg = xmldoc.CreateElement("AUDITENTRIES.LIST");
                                    XmlElement INPUTCRALLOCSssg = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                                    XmlElement DUTYHEADDETAILSssg = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                                    XmlElement EXCISEDUTYHEADDETAILSssg = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                                    XmlElement RATEDETAILSssg = xmldoc.CreateElement("RATEDETAILS.LIST");
                                    XmlElement SUMMARYALLOCSssg = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                                    XmlElement STPYMTDETAILSssg = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                                    XmlElement EXCISEPAYMENTALLOCATIONSssg = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                                    XmlElement TAXBILLALLOCATIONSssg = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                                    XmlElement TAXOBJECTALLOCATIONSssg = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                                    XmlElement TDSEXPENSEALLOCATIONSssg = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                                    XmlElement VATSTATUTORYDETAILSSssg = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                                    XmlElement COSTTRACKALLOCATIONSssg = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                                    XmlElement REFVOUCHERDETAILssg = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                                    XmlElement INVOICEWISEDETAILssg = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                                    XmlElement VATITCDETAILssg = xmldoc.CreateElement("VATITCDETAILS.LIST");
                                    XmlElement ADVANCETAXDETAILssg = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                                    ALLLEDGERENTRIESMsg.AppendChild(BANKALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(BILLALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(INTERESTCOLLECTIONssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(OLDAUDITENTRIESssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(ACCOUNTAUDITENTRIESSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(AUDITENTRIESswsg);
                                    ALLLEDGERENTRIESMsg.AppendChild(INPUTCRALLOCSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(DUTYHEADDETAILSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(EXCISEDUTYHEADDETAILSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(RATEDETAILSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(SUMMARYALLOCSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(STPYMTDETAILSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(EXCISEPAYMENTALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(TAXBILLALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(TAXOBJECTALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(TDSEXPENSEALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(VATSTATUTORYDETAILSSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(COSTTRACKALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(REFVOUCHERDETAILssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(INVOICEWISEDETAILssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(VATITCDETAILssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(ADVANCETAXDETAILssg);
                                    VOUCHER.AppendChild(ALLLEDGERENTRIESMsg);
                                }
                                //
                                //igst
                                if (bbd1.BOOKDETAIL_IgstAmount_num > 0)
                                {
                                    XmlElement ALLLEDGERENTRIESMig = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                                    XmlElement OLDAUDITENTRYIDSzig = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                                    OLDAUDITENTRYIDSzig.SetAttribute("TYPE", "Number");
                                    XmlElement OLDAUDITENTRYIDSsig = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                                    OLDAUDITENTRYIDSsig.InnerText = "-1";
                                    OLDAUDITENTRYIDSzig.AppendChild(OLDAUDITENTRYIDSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(OLDAUDITENTRYIDSzig);

                                    XmlElement LEDGERNAMEeig = xmldoc.CreateElement("LEDGERNAME");
                                    LEDGERNAMEeig.InnerText = "IGST " + bbd1.BOOKDETAIL_IgstPercent_num + "% Input Credit";
                                    ALLLEDGERENTRIESMig.AppendChild(LEDGERNAMEeig);
                                    XmlElement GSTCLASSsig = xmldoc.CreateElement("GSTCLASS");
                                    ALLLEDGERENTRIESMig.AppendChild(GSTCLASSsig);

                                    XmlElement ISDEEMEDPOSITIVEaig = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                    ISDEEMEDPOSITIVEaig.InnerText = "Yes";
                                    ALLLEDGERENTRIESMig.AppendChild(ISDEEMEDPOSITIVEaig);

                                    XmlElement LEDGERFROMITEMaig = xmldoc.CreateElement("LEDGERFROMITEM");
                                    LEDGERFROMITEMaig.InnerText = "No";
                                    ALLLEDGERENTRIESMig.AppendChild(LEDGERFROMITEMaig);

                                    XmlElement REMOVEZEROENTRIESqig = xmldoc.CreateElement("REMOVEZEROENTRIES");
                                    REMOVEZEROENTRIESqig.InnerText = "No";
                                    ALLLEDGERENTRIESMig.AppendChild(REMOVEZEROENTRIESqig);

                                    XmlElement ISPARTYLEDGERwig = xmldoc.CreateElement("ISPARTYLEDGER");
                                    ISPARTYLEDGERwig.InnerText = "No";
                                    ALLLEDGERENTRIESMig.AppendChild(ISPARTYLEDGERwig);

                                    XmlElement ISLASTDEEMEDPOSITIVErig = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                                    ISLASTDEEMEDPOSITIVErig.InnerText = "Yes";
                                    ALLLEDGERENTRIESMig.AppendChild(ISLASTDEEMEDPOSITIVErig);

                                    XmlElement ISCAPVATTAXALTEREDrig = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                                    ISCAPVATTAXALTEREDrig.InnerText = "No";
                                    ALLLEDGERENTRIESMig.AppendChild(ISCAPVATTAXALTEREDrig);

                                    XmlElement ISCAPVATNOTCLAIMEDdrig = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                                    ISCAPVATNOTCLAIMEDdrig.InnerText = "No";
                                    ALLLEDGERENTRIESMig.AppendChild(ISCAPVATNOTCLAIMEDdrig);

                                    XmlElement AMOUNTavig = xmldoc.CreateElement("AMOUNT");
                                    AMOUNTavig.InnerText = "-" + bbd1.BOOKDETAIL_IgstAmount_num.ToString();
                                    ALLLEDGERENTRIESMig.AppendChild(AMOUNTavig);

                                    XmlElement VATEXPAMOUNTig = xmldoc.CreateElement("VATEXPAMOUNT");
                                    VATEXPAMOUNTig.InnerText = "-" + bbd1.BOOKDETAIL_IgstAmount_num.ToString();
                                    ALLLEDGERENTRIESMig.AppendChild(VATEXPAMOUNTig);

                                    XmlElement SERVICETAXDETAILSig = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                                    ALLLEDGERENTRIESMig.AppendChild(SERVICETAXDETAILSig);

                                    XmlElement BANKALLOCATIONSsig = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                                    XmlElement BILLALLOCATIONSsig = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                    XmlElement INTERESTCOLLECTIONsig = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                    XmlElement OLDAUDITENTRIESsig = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                                    XmlElement ACCOUNTAUDITENTRIESSsig = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                                    XmlElement AUDITENTRIESswig = xmldoc.CreateElement("AUDITENTRIES.LIST");
                                    XmlElement INPUTCRALLOCSsig = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                                    XmlElement DUTYHEADDETAILSsig = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                                    XmlElement EXCISEDUTYHEADDETAILSsig = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                                    XmlElement RATEDETAILSsig = xmldoc.CreateElement("RATEDETAILS.LIST");
                                    XmlElement SUMMARYALLOCSsig = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                                    XmlElement STPYMTDETAILSsig = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                                    XmlElement EXCISEPAYMENTALLOCATIONSsig = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                                    XmlElement TAXBILLALLOCATIONSsig = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                                    XmlElement TAXOBJECTALLOCATIONSsig = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                                    XmlElement TDSEXPENSEALLOCATIONSsig = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                                    XmlElement VATSTATUTORYDETAILSSsig = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                                    XmlElement COSTTRACKALLOCATIONSsig = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                                    XmlElement REFVOUCHERDETAILsig = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                                    XmlElement INVOICEWISEDETAILsig = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                                    XmlElement VATITCDETAILsig = xmldoc.CreateElement("VATITCDETAILS.LIST");
                                    XmlElement ADVANCETAXDETAILsig = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                                    ALLLEDGERENTRIESMig.AppendChild(BANKALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(BILLALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(INTERESTCOLLECTIONsig);
                                    ALLLEDGERENTRIESMig.AppendChild(OLDAUDITENTRIESsig);
                                    ALLLEDGERENTRIESMig.AppendChild(ACCOUNTAUDITENTRIESSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(AUDITENTRIESswig);
                                    ALLLEDGERENTRIESMig.AppendChild(INPUTCRALLOCSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(DUTYHEADDETAILSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(EXCISEDUTYHEADDETAILSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(RATEDETAILSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(SUMMARYALLOCSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(STPYMTDETAILSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(EXCISEPAYMENTALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(TAXBILLALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(TAXOBJECTALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(TDSEXPENSEALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(VATSTATUTORYDETAILSSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(COSTTRACKALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(REFVOUCHERDETAILsig);
                                    ALLLEDGERENTRIESMig.AppendChild(INVOICEWISEDETAILsig);
                                    ALLLEDGERENTRIESMig.AppendChild(VATITCDETAILsig);
                                    ALLLEDGERENTRIESMig.AppendChild(ADVANCETAXDETAILsig);
                                    VOUCHER.AppendChild(ALLLEDGERENTRIESMig);
                                }
                                //
                            }
                            else// if (bbd1.BOOKDETAIL_LedgerName_var.ToLower().Contains("tds") == false)
                            {
                                CreditAmt += Convert.ToDouble(bbd1.BOOKDETAIL_Amount_num.ToString());
                            }
                        }
                        XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSm.InnerText = "-1";
                        OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
                        ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

                        XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAME.InnerText = lblVendor.Text.Replace("&", "AND");
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
                        XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

                        XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

                        XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEM.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

                        XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIES.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

                        XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGER.InnerText = "Yes";
                        ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

                        XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);

                        XmlElement ISCAPVATTAXALTERED = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                        ISCAPVATTAXALTERED.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISCAPVATTAXALTERED);

                        XmlElement ISCAPVATNOTCLAIMED = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                        ISCAPVATNOTCLAIMED.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISCAPVATNOTCLAIMED);

                        XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
                        AMOUNT.InnerText = lblTotalNetPayble.Text;
                        ALLLEDGERENTRIESmain.AppendChild(AMOUNT);

                        XmlElement VATEXPAMOUNTs = xmldoc.CreateElement("VATEXPAMOUNT");
                        VATEXPAMOUNTs.InnerText = lblTotalNetPayble.Text;
                        ALLLEDGERENTRIESmain.AppendChild(VATEXPAMOUNTs);

                        XmlElement SERVICETAXDETAILSs = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                        ALLLEDGERENTRIESmain.AppendChild(SERVICETAXDETAILSs);

                        XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);

                        XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement NAME = xmldoc.CreateElement("NAME");
                        XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
                        NAME.InnerText = lblSupplierInvoiceNo.Text;
                        BILLALLOCATIONS.AppendChild(NAME);

                        BILLTYPE.InnerText = "New Ref";
                        BILLALLOCATIONS.AppendChild(BILLTYPE);

                        XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
                        TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
                        BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

                        XmlElement AMT = xmldoc.CreateElement("AMOUNT");
                        AMT.InnerText = lblTotalNetPayble.Text;
                        BILLALLOCATIONS.AppendChild(AMT);

                        XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
                        XmlElement STBILLCATEGORIES = xmldoc.CreateElement("STBILLCATEGORIES.LIST");
                        BILLALLOCATIONS.AppendChild(STBILLCATEGORIES);
                        ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);

                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INPUTCRALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("DUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("RATEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("SUMMARYALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("STPYMTDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("REFVOUCHERDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INVOICEWISEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATITCDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ADVANCETAXDETAILS.LIST"));
                        VOUCHER.AppendChild(ALLLEDGERENTRIESmain);
                        ////

                        foreach (var bbd2 in billBookingDetail)
                        {
                            if (bbd2.BOOKDETAIL_DrCr_var == "Credit") // && bbd2.BOOKDETAIL_LedgerName_var.ToLower().Contains("tds") == true) //credit
                            {
                                XmlElement ALLLEDGERENTRIESMtds = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                                XmlElement OLDAUDITENTRYIDSztds = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                                OLDAUDITENTRYIDSztds.SetAttribute("TYPE", "Number");
                                XmlElement OLDAUDITENTRYIDSstds = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                                OLDAUDITENTRYIDSstds.InnerText = "-1";
                                OLDAUDITENTRYIDSztds.AppendChild(OLDAUDITENTRYIDSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(OLDAUDITENTRYIDSztds);

                                XmlElement LEDGERNAMEetds = xmldoc.CreateElement("LEDGERNAME");
                                LEDGERNAMEetds.InnerText = bbd2.BOOKDETAIL_LedgerName_var;
                                ALLLEDGERENTRIESMtds.AppendChild(LEDGERNAMEetds);
                                XmlElement GSTCLASSstds = xmldoc.CreateElement("GSTCLASS");
                                ALLLEDGERENTRIESMtds.AppendChild(GSTCLASSstds);

                                XmlElement ISDEEMEDPOSITIVEatds = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                ISDEEMEDPOSITIVEatds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(ISDEEMEDPOSITIVEatds);

                                XmlElement LEDGERFROMITEMatds = xmldoc.CreateElement("LEDGERFROMITEM");
                                LEDGERFROMITEMatds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(LEDGERFROMITEMatds);

                                XmlElement REMOVEZEROENTRIESqtds = xmldoc.CreateElement("REMOVEZEROENTRIES");
                                REMOVEZEROENTRIESqtds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(REMOVEZEROENTRIESqtds);

                                XmlElement ISPARTYLEDGERwtds = xmldoc.CreateElement("ISPARTYLEDGER");
                                ISPARTYLEDGERwtds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(ISPARTYLEDGERwtds);

                                XmlElement ISLASTDEEMEDPOSITIVErtds = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                                ISLASTDEEMEDPOSITIVErtds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(ISLASTDEEMEDPOSITIVErtds);

                                XmlElement ISCAPVATTAXALTEREDrtds = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                                ISCAPVATTAXALTEREDrtds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(ISCAPVATTAXALTEREDrtds);

                                XmlElement ISCAPVATNOTCLAIMEDdrtds = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                                ISCAPVATNOTCLAIMEDdrtds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(ISCAPVATNOTCLAIMEDdrtds);

                                XmlElement AMOUNTavtds = xmldoc.CreateElement("AMOUNT");
                                //AMOUNTavtds.InnerText = bbd2.BOOKDETAIL_Amount_num.ToString();
                                AMOUNTavtds.InnerText = CreditAmt.ToString("0.00");
                                ALLLEDGERENTRIESMtds.AppendChild(AMOUNTavtds);

                                XmlElement VATEXPAMOUNTtds = xmldoc.CreateElement("VATEXPAMOUNT");
                                //VATEXPAMOUNTtds.InnerText = bbd2.BOOKDETAIL_Amount_num.ToString();
                                VATEXPAMOUNTtds.InnerText = CreditAmt.ToString("0.00");
                                ALLLEDGERENTRIESMtds.AppendChild(VATEXPAMOUNTtds);

                                XmlElement SERVICETAXDETAILStds = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                                ALLLEDGERENTRIESMtds.AppendChild(SERVICETAXDETAILStds);

                                XmlElement BANKALLOCATIONSstds = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                                XmlElement BILLALLOCATIONSstds = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                XmlElement INTERESTCOLLECTIONstds = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                XmlElement OLDAUDITENTRIESstds = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                                XmlElement ACCOUNTAUDITENTRIESSstds = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                                XmlElement AUDITENTRIESswtds = xmldoc.CreateElement("AUDITENTRIES.LIST");
                                XmlElement INPUTCRALLOCSstds = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                                XmlElement DUTYHEADDETAILSstds = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                                XmlElement EXCISEDUTYHEADDETAILSstds = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                                XmlElement RATEDETAILSstds = xmldoc.CreateElement("RATEDETAILS.LIST");
                                XmlElement SUMMARYALLOCSstds = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                                XmlElement STPYMTDETAILSstds = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                                XmlElement EXCISEPAYMENTALLOCATIONSstds = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                                XmlElement TAXBILLALLOCATIONSstds = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                                XmlElement TAXOBJECTALLOCATIONSstds = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                                XmlElement TDSEXPENSEALLOCATIONSstds = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                                XmlElement VATSTATUTORYDETAILSSstds = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                                XmlElement COSTTRACKALLOCATIONSstds = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                                XmlElement REFVOUCHERDETAILstds = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                                XmlElement INVOICEWISEDETAILstds = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                                XmlElement VATITCDETAILstds = xmldoc.CreateElement("VATITCDETAILS.LIST");
                                XmlElement ADVANCETAXDETAILstds = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                                ALLLEDGERENTRIESMtds.AppendChild(BANKALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(BILLALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(INTERESTCOLLECTIONstds);
                                ALLLEDGERENTRIESMtds.AppendChild(OLDAUDITENTRIESstds);
                                ALLLEDGERENTRIESMtds.AppendChild(ACCOUNTAUDITENTRIESSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(AUDITENTRIESswtds);
                                ALLLEDGERENTRIESMtds.AppendChild(INPUTCRALLOCSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(DUTYHEADDETAILSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(EXCISEDUTYHEADDETAILSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(RATEDETAILSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(SUMMARYALLOCSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(STPYMTDETAILSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(EXCISEPAYMENTALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(TAXBILLALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(TAXOBJECTALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(TDSEXPENSEALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(VATSTATUTORYDETAILSSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(COSTTRACKALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(REFVOUCHERDETAILstds);
                                ALLLEDGERENTRIESMtds.AppendChild(INVOICEWISEDETAILstds);
                                ALLLEDGERENTRIESMtds.AppendChild(VATITCDETAILstds);
                                ALLLEDGERENTRIESMtds.AppendChild(ADVANCETAXDETAILstds);
                                VOUCHER.AppendChild(ALLLEDGERENTRIESMtds);
                                break;
                            }
                        }
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INPUTCRALLOCS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("DUTYHEADDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("RATEDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("SUMMARYALLOCS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("STPYMTDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("REFVOUCHERDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INVOICEWISEDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATITCDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ADVANCETAXDETAILS.LIST"));
                        //VOUCHER.AppendChild(ALLLEDGERENTRIESmain);
                        XmlElement PAYROLLMODEOFPAYMENT = xmldoc.CreateElement("PAYROLLMODEOFPAYMENT.LIST");
                        VOUCHER.AppendChild(PAYROLLMODEOFPAYMENT);
                        XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
                        VOUCHER.AppendChild(ATTDRECORDS);
                        XmlElement GSTEWAYCONSIGNORADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNORADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNORADDRESS);
                        XmlElement GSTEWAYCONSIGNEEADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNEEADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNEEADDRESS);
                        XmlElement TEMPGSTRATEDETAILS = xmldoc.CreateElement("TEMPGSTRATEDETAILS.LIST");
                        VOUCHER.AppendChild(TEMPGSTRATEDETAILS);

                        TALLYMESSAGE.AppendChild(VOUCHER);
                        REQUESTDATA.AppendChild(TALLYMESSAGE);
                        dc.BillBooking_Update_TallyStatus(Convert.ToInt32(lblBookingNo.Text), true);
                        break;
                    }
                }
            }
            #endregion
            RootNode.AppendChild(HEADER);
            IMPORTDATA.AppendChild(REQUESTDESC);
            IMPORTDATA.AppendChild(REQUESTDATA);
            BODY.AppendChild(IMPORTDATA);
            RootNode.AppendChild(BODY);

            if (!Directory.Exists(@"d:\xml"))
                Directory.CreateDirectory(@"d:\xml");
            string fileName = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                fileName = "BILLBOOKING_Journal_Mumbai";
            else if (cnStr.ToLower().Contains("nashik") == true)
                fileName = "BILLBOOKING_Journal_Nashik";
            else if (cnStr.ToLower().Contains("metro") == true)
                fileName = "BILLBOOKING_Journal_Metro";
            else
                fileName = "BILLBOOKING_Journal_Pune";
            xmldoc.Save(@"d:\xml\" + fileName + ".xml");

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
            response.TransmitFile("d:\\xml\\" + fileName + ".xml");
            response.Flush();
            response.End();
        }

        private void PurchaseTallyTransfer()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
            xmldoc.AppendChild(RootNode);

            XmlElement HEADER = xmldoc.CreateElement("HEADER");
            XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
            TALLYREQUEST.InnerText = "Import Data";
            HEADER.AppendChild(TALLYREQUEST);
            RootNode.AppendChild(HEADER);

            XmlElement BODY = xmldoc.CreateElement("BODY");
            XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
            XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
            XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
            REPORTNAME.InnerText = "Vouchers";
            REQUESTDESC.AppendChild(REPORTNAME);

            XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
            XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");
            //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
            //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
            //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
            if (cnStr.ToLower().Contains("mumbai") == true)
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
            else if (cnStr.ToLower().Contains("nashik") == true)
                SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
            else
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2015";
            STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
            REQUESTDESC.AppendChild(STATICVARIABLES);
            XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
            #region JOURNL
            for (int i = 0; i < grdBillBooking.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdBillBooking.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblBookingNo = (Label)grdBillBooking.Rows[i].FindControl("lblBookingNo");
                    Label lblBookDate = (Label)grdBillBooking.Rows[i].FindControl("lblBookDate");
                    Label lblSupplierInvoiceNo = (Label)grdBillBooking.Rows[i].FindControl("lblSupplierInvoiceNo");
                    Label lblInvoiceDate = (Label)grdBillBooking.Rows[i].FindControl("lblInvoiceDate");
                    Label lblTotalNetPayble = (Label)grdBillBooking.Rows[i].FindControl("lblTotalNetPayble");
                    Label lblType = (Label)grdBillBooking.Rows[i].FindControl("lblType");
                    Label lblVendor = (Label)grdBillBooking.Rows[i].FindControl("lblVendor");
                    Label lblNarration = (Label)grdBillBooking.Rows[i].FindControl("lblNarration");
                    Label lblComment = (Label)grdBillBooking.Rows[i].FindControl("lblComment");

                    XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
                    XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
                    double CreditAmt = 0;
                    var billBookingDetail = dc.BillBookingDetail_View(Convert.ToInt32(lblBookingNo.Text)).ToList();
                    foreach (var bbd in billBookingDetail)
                    {

                        TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");
                        VOUCHER.SetAttribute("REMOTEID", "");
                        VOUCHER.SetAttribute("VCHKEY", "");
                        VOUCHER.SetAttribute("VCHTYPE", "Purchase");
                        VOUCHER.SetAttribute("ACTION", "Create");
                        VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

                        XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSf.InnerText = "-1";
                        OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
                        VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

                        XmlElement DATE = xmldoc.CreateElement("DATE");
                        DateTime date = DateTime.ParseExact(lblBookDate.Text, "dd/MM/yyyy", null);
                        DATE.InnerText = date.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(DATE);

                        XmlElement GUID = xmldoc.CreateElement("GUID");
                        GUID.InnerText = "";
                        VOUCHER.AppendChild(GUID);

                        XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
                        NARRATION.InnerText = lblNarration.Text;
                        VOUCHER.AppendChild(NARRATION);

                        XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
                        VOUCHERTYPENAME.InnerText = "Purchase";
                        VOUCHER.AppendChild(VOUCHERTYPENAME);

                        XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
                        VOUCHERNUMBER.InnerText = lblSupplierInvoiceNo.Text;
                        VOUCHER.AppendChild(VOUCHERNUMBER);

                        XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
                        PARTYLEDGERNAME.InnerText = lblVendor.Text.Replace("&", "AND");
                        VOUCHER.AppendChild(PARTYLEDGERNAME);
                        XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
                        VOUCHER.AppendChild(CSTFORMISSUETYPE);
                        XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
                        VOUCHER.AppendChild(CSTFORMRECVTYPE);
                        XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
                        FBTPAYMENTTYPE.InnerText = "Default";
                        VOUCHER.AppendChild(FBTPAYMENTTYPE);

                        XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
                        PERSISTEDVIEW.InnerText = "Accounting Voucher View";
                        VOUCHER.AppendChild(PERSISTEDVIEW);
                        XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
                        VOUCHER.AppendChild(VCHGSTCLASS);

                        XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
                        ENTEREDBY.InnerText = "DESPL";
                        VOUCHER.AppendChild(ENTEREDBY);

                        //XmlElement VOUCHERTYPEORIGNAME = xmldoc.CreateElement("VOUCHERTYPEORIGNAME");
                        //VOUCHERTYPEORIGNAME.InnerText = "Journal";
                        //VOUCHER.AppendChild(VOUCHERTYPEORIGNAME);

                        XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
                        DIFFACTUALQTY.InnerText = "No";
                        VOUCHER.AppendChild(DIFFACTUALQTY);

                        XmlElement ISMSTFROMSYNC = xmldoc.CreateElement("ISMSTFROMSYNC");
                        ISMSTFROMSYNC.InnerText = "No";
                        VOUCHER.AppendChild(ISMSTFROMSYNC);

                        XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        ASORIGINAL.InnerText = "No";
                        VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
                        AUDITED.InnerText = "No";
                        VOUCHER.AppendChild(AUDITED);

                        XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
                        FORJOBCOSTING.InnerText = "No";
                        VOUCHER.AppendChild(FORJOBCOSTING);

                        XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
                        ISOPTIONAL.InnerText = "No";
                        VOUCHER.AppendChild(ISOPTIONAL);

                        XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
                        DateTime date2 = DateTime.ParseExact(lblBookDate.Text, "dd/MM/yyyy", null);
                        EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(EFFECTIVEDATE);

                        XmlElement USEFOREXCISE = xmldoc.CreateElement("USEFOREXCISE");
                        USEFOREXCISE.InnerText = "No";
                        VOUCHER.AppendChild(USEFOREXCISE);

                        XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
                        ISFORJOBWORKIN.InnerText = "No";
                        VOUCHER.AppendChild(ISFORJOBWORKIN);

                        XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
                        ALLOWCONSUMPTION.InnerText = "No";
                        VOUCHER.AppendChild(ALLOWCONSUMPTION);

                        XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
                        USEFORINTEREST.InnerText = "No";
                        VOUCHER.AppendChild(USEFORINTEREST);

                        XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
                        USEFORGAINLOSS.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGAINLOSS);

                        XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
                        USEFORGODOWNTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

                        XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                        USEFORCOMPOUND.InnerText = "No";
                        VOUCHER.AppendChild(USEFORCOMPOUND);

                        XmlElement USEFORSERVICETAX = xmldoc.CreateElement("USEFORSERVICETAX");
                        USEFORSERVICETAX.InnerText = "No";
                        VOUCHER.AppendChild(USEFORSERVICETAX);


                        XmlElement ISEXCISEVOUCHER = xmldoc.CreateElement("ISEXCISEVOUCHER");
                        ISEXCISEVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEVOUCHER);

                        XmlElement EXCISETAXOVERRIDE = xmldoc.CreateElement("EXCISETAXOVERRIDE");
                        EXCISETAXOVERRIDE.InnerText = "No";
                        VOUCHER.AppendChild(EXCISETAXOVERRIDE);

                        XmlElement USEFORTAXUNITTRANSFER = xmldoc.CreateElement("USEFORTAXUNITTRANSFER");
                        USEFORTAXUNITTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORTAXUNITTRANSFER);

                        XmlElement IGNOREPOSVALIDATION = xmldoc.CreateElement("IGNOREPOSVALIDATION");
                        IGNOREPOSVALIDATION.InnerText = "No";
                        VOUCHER.AppendChild(IGNOREPOSVALIDATION);

                        XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
                        EXCISEOPENING.InnerText = "No";
                        VOUCHER.AppendChild(EXCISEOPENING);

                        XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
                        USEFORFINALPRODUCTION.InnerText = "No";
                        VOUCHER.AppendChild(USEFORFINALPRODUCTION);

                        XmlElement ISTDSOVERRIDDEN = xmldoc.CreateElement("ISTDSOVERRIDDEN");
                        ISTDSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSOVERRIDDEN);

                        XmlElement ISTCSOVERRIDDEN = xmldoc.CreateElement("ISTCSOVERRIDDEN");
                        ISTCSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTCSOVERRIDDEN);

                        XmlElement ISTDSTCSCASHVCH = xmldoc.CreateElement("ISTDSTCSCASHVCH");
                        ISTDSTCSCASHVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSTCSCASHVCH);

                        XmlElement INCLUDEADVPYMTVCH = xmldoc.CreateElement("INCLUDEADVPYMTVCH");
                        INCLUDEADVPYMTVCH.InnerText = "No";
                        VOUCHER.AppendChild(INCLUDEADVPYMTVCH);

                        XmlElement ISSUBWORKSCONTRACT = xmldoc.CreateElement("ISSUBWORKSCONTRACT");
                        ISSUBWORKSCONTRACT.InnerText = "No";
                        VOUCHER.AppendChild(ISSUBWORKSCONTRACT);

                        XmlElement ISVATOVERRIDDEN = xmldoc.CreateElement("ISVATOVERRIDDEN");
                        ISVATOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISVATOVERRIDDEN);

                        XmlElement IGNOREORIGVCHDATE = xmldoc.CreateElement("IGNOREORIGVCHDATE");
                        IGNOREORIGVCHDATE.InnerText = "No";
                        VOUCHER.AppendChild(IGNOREORIGVCHDATE);

                        XmlElement ISVATPAIDATCUSTOMS = xmldoc.CreateElement("ISVATPAIDATCUSTOMS");
                        ISVATPAIDATCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPAIDATCUSTOMS);

                        XmlElement ISDECLAREDTOCUSTOMS = xmldoc.CreateElement("ISDECLAREDTOCUSTOMS");
                        ISDECLAREDTOCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISDECLAREDTOCUSTOMS);

                        XmlElement ISSERVICETAXOVERRIDDEN = xmldoc.CreateElement("ISSERVICETAXOVERRIDDEN");
                        ISSERVICETAXOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISSERVICETAXOVERRIDDEN);

                        XmlElement ISISDVOUCHER = xmldoc.CreateElement("ISISDVOUCHER");
                        ISISDVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISISDVOUCHER);

                        XmlElement ISEXCISEOVERRIDDEN = xmldoc.CreateElement("ISEXCISEOVERRIDDEN");
                        ISEXCISEOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEOVERRIDDEN);

                        XmlElement ISEXCISESUPPLYVCH = xmldoc.CreateElement("ISEXCISESUPPLYVCH");
                        ISEXCISESUPPLYVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISESUPPLYVCH);

                        XmlElement ISGSTOVERRIDDEN = xmldoc.CreateElement("ISGSTOVERRIDDEN");
                        ISGSTOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISGSTOVERRIDDEN);


                        XmlElement GSTNOTEEXPORTED = xmldoc.CreateElement("GSTNOTEXPORTED");
                        GSTNOTEEXPORTED.InnerText = "No";
                        VOUCHER.AppendChild(GSTNOTEEXPORTED);

                        XmlElement IGNOREGSTINVALIDATION = xmldoc.CreateElement("IGNOREGSTINVALIDATION");
                        IGNOREGSTINVALIDATION.InnerText = "No";
                        VOUCHER.AppendChild(IGNOREGSTINVALIDATION);

                        XmlElement ISVATPRINCIPALACCOUNT = xmldoc.CreateElement("ISVATPRINCIPALACCOUNT");
                        ISVATPRINCIPALACCOUNT.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPRINCIPALACCOUNT);

                        XmlElement ISBOENOTAPPLICABLE = xmldoc.CreateElement("ISBOENOTAPPLICABLE");
                        ISBOENOTAPPLICABLE.InnerText = "No";
                        VOUCHER.AppendChild(ISBOENOTAPPLICABLE);

                        XmlElement ISSHIPPINGWITHINSTATE = xmldoc.CreateElement("ISSHIPPINGWITHINSTATE");
                        ISSHIPPINGWITHINSTATE.InnerText = "No";
                        VOUCHER.AppendChild(ISSHIPPINGWITHINSTATE);

                        XmlElement ISOVERSEASTOURISTTRANS = xmldoc.CreateElement("ISOVERSEASTOURISTTRANS");
                        ISOVERSEASTOURISTTRANS.InnerText = "No";
                        VOUCHER.AppendChild(ISOVERSEASTOURISTTRANS);

                        XmlElement ISDESIGNATEDZONEPARTY = xmldoc.CreateElement("ISDESIGNATEDZONEPARTY");
                        ISDESIGNATEDZONEPARTY.InnerText = "No";
                        VOUCHER.AppendChild(ISDESIGNATEDZONEPARTY);

                        XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
                        ISCANCELLED.InnerText = "No";
                        VOUCHER.AppendChild(ISCANCELLED);

                        XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
                        HASCASHFLOW.InnerText = "No";
                        VOUCHER.AppendChild(HASCASHFLOW);

                        XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
                        ISPOSTDATED.InnerText = "No";
                        VOUCHER.AppendChild(ISPOSTDATED);

                        XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
                        USETRACKINGNUMBER.InnerText = "No";
                        VOUCHER.AppendChild(USETRACKINGNUMBER);

                        XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
                        ISINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISINVOICE);

                        XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
                        MFGJOURNAL.InnerText = "No";
                        VOUCHER.AppendChild(MFGJOURNAL);

                        XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
                        HASDISCOUNTS.InnerText = "No";
                        VOUCHER.AppendChild(HASDISCOUNTS);

                        XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
                        ASPAYSLIP.InnerText = "No";
                        VOUCHER.AppendChild(ASPAYSLIP);

                        XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
                        ISCOSTCENTRE.InnerText = "No";
                        VOUCHER.AppendChild(ISCOSTCENTRE);

                        XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
                        ISSTXNONREALIZEDVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

                        XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
                        ISEXCISEMANUFACTURERON.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

                        XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
                        ISBLANKCHEQUE.InnerText = "No";
                        VOUCHER.AppendChild(ISBLANKCHEQUE);

                        XmlElement ISVOID = xmldoc.CreateElement("ISVOID");
                        ISVOID.InnerText = "No";
                        VOUCHER.AppendChild(ISVOID);

                        XmlElement ISONHOLD = xmldoc.CreateElement("ISONHOLD");
                        ISONHOLD.InnerText = "No";
                        VOUCHER.AppendChild(ISONHOLD);

                        XmlElement ORDERLINESTATUS = xmldoc.CreateElement("ORDERLINESTATUS");
                        ORDERLINESTATUS.InnerText = "No";
                        VOUCHER.AppendChild(ORDERLINESTATUS);

                        XmlElement VATISAGNSTCANCSALES = xmldoc.CreateElement("VATISAGNSTCANCSALES");
                        VATISAGNSTCANCSALES.InnerText = "No";
                        VOUCHER.AppendChild(VATISAGNSTCANCSALES);

                        XmlElement VATISPURCEXEMPTED = xmldoc.CreateElement("VATISPURCEXEMPTED");
                        VATISPURCEXEMPTED.InnerText = "No";
                        VOUCHER.AppendChild(VATISPURCEXEMPTED);

                        XmlElement ISVATRESTAXINVOICE = xmldoc.CreateElement("ISVATRESTAXINVOICE");
                        ISVATRESTAXINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISVATRESTAXINVOICE);

                        XmlElement VATISASSESABLECALCVCH = xmldoc.CreateElement("VATISASSESABLECALCVCH");
                        VATISASSESABLECALCVCH.InnerText = "No";
                        VOUCHER.AppendChild(VATISASSESABLECALCVCH);

                        XmlElement ISVATDUTYPAID = xmldoc.CreateElement("ISVATDUTYPAID");
                        ISVATDUTYPAID.InnerText = "Yes";
                        VOUCHER.AppendChild(ISVATDUTYPAID);

                        XmlElement ISDELIVERYSAMEASCONSIGNEE = xmldoc.CreateElement("ISDELIVERYSAMEASCONSIGNEE");
                        ISDELIVERYSAMEASCONSIGNEE.InnerText = "No";
                        VOUCHER.AppendChild(ISDELIVERYSAMEASCONSIGNEE);

                        XmlElement ISDISPATCHSAMEASCONSIGNOR = xmldoc.CreateElement("ISDISPATCHSAMEASCONSIGNOR");
                        ISDISPATCHSAMEASCONSIGNOR.InnerText = "No";
                        VOUCHER.AppendChild(ISDISPATCHSAMEASCONSIGNOR);

                        XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
                        ISDELETED.InnerText = "No";
                        VOUCHER.AppendChild(ISDELETED);

                        //XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        //ASORIGINAL.InnerText = "No";
                        //VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement CHANGEVCHMODE = xmldoc.CreateElement("CHANGEVCHMODE");
                        CHANGEVCHMODE.InnerText = "No";
                        VOUCHER.AppendChild(CHANGEVCHMODE);

                        XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
                        XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
                        XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
                        XmlElement EXCLUDEDTAXATION = xmldoc.CreateElement("EXCLUDEDTAXATION.LIST");
                        XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement DUTYHEADDETAILS = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                        XmlElement SUPPLEMENTARYDUTYHEADDETAILS = xmldoc.CreateElement("SUPPLEMENTARYDUTYHEADDETAILS.LIST");
                        XmlElement EWAYBILLDETAILS = xmldoc.CreateElement("EWAYBILLDETAILS.LIST");
                        XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
                        XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
                        XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
                        XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");
                        XmlElement ORIGINVOICEDETAILS = xmldoc.CreateElement("ORIGINVOICEDETAILS.LIST");
                        XmlElement INVOICEEXPORTLIST = xmldoc.CreateElement("INVOICEEXPORTLIST.LIST");

                        ALTERID.InnerText = "";
                        VOUCHER.AppendChild(ALTERID);

                        MASTERID.InnerText = "";
                        VOUCHER.AppendChild(MASTERID);

                        VOUCHERKEY.InnerText = "";
                        VOUCHER.AppendChild(VOUCHERKEY);

                        VOUCHER.AppendChild(EXCLUDEDTAXATION);
                        VOUCHER.AppendChild(OLDAUDITENTRIES);
                        VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
                        VOUCHER.AppendChild(AUDITENTRIES);
                        VOUCHER.AppendChild(DUTYHEADDETAILS);
                        VOUCHER.AppendChild(SUPPLEMENTARYDUTYHEADDETAILS);
                        VOUCHER.AppendChild(EWAYBILLDETAILS);
                        VOUCHER.AppendChild(INVOICEDELNOTES);
                        VOUCHER.AppendChild(INVOICEORDERLIST);
                        VOUCHER.AppendChild(INVOICEINDENTLIST);
                        VOUCHER.AppendChild(ATTENDANCEENTRIES);
                        VOUCHER.AppendChild(ORIGINVOICEDETAILS);
                        VOUCHER.AppendChild(INVOICEEXPORTLIST);

                        //**
                        XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSm.InnerText = "-1";
                        OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
                        ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

                        XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAME.InnerText = lblVendor.Text.Replace("&", "AND");
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
                        XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

                        XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

                        XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEM.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

                        XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIES.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

                        XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGER.InnerText = "Yes";
                        ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

                        XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);

                        XmlElement ISCAPVATTAXALTERED = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                        ISCAPVATTAXALTERED.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISCAPVATTAXALTERED);

                        XmlElement ISCAPVATNOTCLAIMED = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                        ISCAPVATNOTCLAIMED.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISCAPVATNOTCLAIMED);

                        XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
                        AMOUNT.InnerText = lblTotalNetPayble.Text;
                        ALLLEDGERENTRIESmain.AppendChild(AMOUNT);

                        XmlElement VATEXPAMOUNTs = xmldoc.CreateElement("VATEXPAMOUNT");
                        VATEXPAMOUNTs.InnerText = lblTotalNetPayble.Text;
                        ALLLEDGERENTRIESmain.AppendChild(VATEXPAMOUNTs);

                        XmlElement SERVICETAXDETAILSs = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                        ALLLEDGERENTRIESmain.AppendChild(SERVICETAXDETAILSs);

                        XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);

                        XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement NAME = xmldoc.CreateElement("NAME");
                        XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
                        NAME.InnerText = lblSupplierInvoiceNo.Text;
                        BILLALLOCATIONS.AppendChild(NAME);

                        BILLTYPE.InnerText = "New Ref";
                        BILLALLOCATIONS.AppendChild(BILLTYPE);

                        XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
                        TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
                        BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

                        XmlElement AMT = xmldoc.CreateElement("AMOUNT");
                        AMT.InnerText = lblTotalNetPayble.Text;
                        BILLALLOCATIONS.AppendChild(AMT);

                        XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
                        XmlElement STBILLCATEGORIES = xmldoc.CreateElement("STBILLCATEGORIES.LIST");
                        BILLALLOCATIONS.AppendChild(STBILLCATEGORIES);
                        ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);

                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INPUTCRALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("DUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("RATEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("SUMMARYALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("STPYMTDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("REFVOUCHERDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INVOICEWISEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATITCDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ADVANCETAXDETAILS.LIST"));
                        VOUCHER.AppendChild(ALLLEDGERENTRIESmain);
                        ////
                        //**

                        foreach (var bbd1 in billBookingDetail)
                        {
                            if (bbd1.BOOKDETAIL_DrCr_var == "Debit")
                            {
                                XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                                XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                                OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                                XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                                OLDAUDITENTRYIDSs.InnerText = "-1";
                                OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                                ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                                XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
                                LEDGERNAMEe.InnerText = bbd1.BOOKDETAIL_LedgerName_var;
                                ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                                XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                                ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                                XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                ISDEEMEDPOSITIVEa.InnerText = "Yes";
                                ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);

                                XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                                LEDGERFROMITEMa.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

                                XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                                REMOVEZEROENTRIESq.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

                                XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                                ISPARTYLEDGERw.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

                                XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                                //ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                                ISLASTDEEMEDPOSITIVEr.InnerText = "Yes";
                                ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

                                XmlElement ISCAPVATTAXALTEREDr = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                                ISCAPVATTAXALTEREDr.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(ISCAPVATTAXALTEREDr);

                                XmlElement ISCAPVATNOTCLAIMEDdr = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                                ISCAPVATNOTCLAIMEDdr.InnerText = "No";
                                ALLLEDGERENTRIESM.AppendChild(ISCAPVATNOTCLAIMEDdr);

                                XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
                                AMOUNTav.InnerText = "-" + bbd1.BOOKDETAIL_Amount_num.ToString();
                                ALLLEDGERENTRIESM.AppendChild(AMOUNTav);

                                XmlElement VATEXPAMOUNT = xmldoc.CreateElement("VATEXPAMOUNT");
                                VATEXPAMOUNT.InnerText = "-" + bbd1.BOOKDETAIL_Amount_num.ToString();
                                ALLLEDGERENTRIESM.AppendChild(VATEXPAMOUNT);

                                XmlElement SERVICETAXDETAILS = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                                ALLLEDGERENTRIESM.AppendChild(SERVICETAXDETAILS);

                                if (bbd1.BOOKDETAIL_Category_var != "---Select---" && bbd1.BOOKDETAIL_Category_var != "NA" && bbd1.BOOKDETAIL_Category_var != null)
                                {
                                    XmlElement CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
                                    XmlElement CATEGORY = xmldoc.CreateElement("CATEGORY");
                                    CATEGORY.InnerText = bbd1.BOOKDETAIL_Category_var;
                                    CATEGORYALLOCATIONS.AppendChild(CATEGORY);

                                    XmlElement ISDEEMEDPOSITIVEc = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                    ISDEEMEDPOSITIVEc.InnerText = "Yes";
                                    CATEGORYALLOCATIONS.AppendChild(ISDEEMEDPOSITIVEc);

                                    XmlElement COSTCENTREALLOCATIONS = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
                                    XmlElement NAMEc = xmldoc.CreateElement("NAME");
                                    NAMEc.InnerText = bbd1.BOOKDETAIL_CostCenter_var;
                                    COSTCENTREALLOCATIONS.AppendChild(NAMEc);

                                    XmlElement AMOUNTc = xmldoc.CreateElement("AMOUNT");
                                    AMOUNTc.InnerText = "-" + bbd1.BOOKDETAIL_Amount_num.ToString();
                                    COSTCENTREALLOCATIONS.AppendChild(AMOUNTc);

                                    CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONS);
                                    ALLLEDGERENTRIESM.AppendChild(CATEGORYALLOCATIONS);
                                }
                                XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                                XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                                XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                                XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                                XmlElement INPUTCRALLOCSs = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                                XmlElement DUTYHEADDETAILSs = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                                XmlElement EXCISEDUTYHEADDETAILSs = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                                XmlElement RATEDETAILSs = xmldoc.CreateElement("RATEDETAILS.LIST");
                                XmlElement SUMMARYALLOCSs = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                                XmlElement STPYMTDETAILSs = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                                XmlElement EXCISEPAYMENTALLOCATIONSs = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                                XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                                XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                                XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                                XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                                XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                                XmlElement REFVOUCHERDETAILs = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                                XmlElement INVOICEWISEDETAILs = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                                XmlElement VATITCDETAILs = xmldoc.CreateElement("VATITCDETAILS.LIST");
                                XmlElement ADVANCETAXDETAILs = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                                ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
                                ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
                                ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
                                ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
                                ALLLEDGERENTRIESM.AppendChild(INPUTCRALLOCSs);
                                ALLLEDGERENTRIESM.AppendChild(DUTYHEADDETAILSs);
                                ALLLEDGERENTRIESM.AppendChild(EXCISEDUTYHEADDETAILSs);
                                ALLLEDGERENTRIESM.AppendChild(RATEDETAILSs);
                                ALLLEDGERENTRIESM.AppendChild(SUMMARYALLOCSs);
                                ALLLEDGERENTRIESM.AppendChild(STPYMTDETAILSs);
                                ALLLEDGERENTRIESM.AppendChild(EXCISEPAYMENTALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
                                ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
                                ALLLEDGERENTRIESM.AppendChild(REFVOUCHERDETAILs);
                                ALLLEDGERENTRIESM.AppendChild(INVOICEWISEDETAILs);
                                ALLLEDGERENTRIESM.AppendChild(VATITCDETAILs);
                                ALLLEDGERENTRIESM.AppendChild(ADVANCETAXDETAILs);
                                VOUCHER.AppendChild(ALLLEDGERENTRIESM);

                                //cgst
                                if (bbd1.BOOKDETAIL_CgstAmount_num > 0)
                                {
                                    XmlElement ALLLEDGERENTRIESMcg = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                                    XmlElement OLDAUDITENTRYIDSzcg = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                                    OLDAUDITENTRYIDSzcg.SetAttribute("TYPE", "Number");
                                    XmlElement OLDAUDITENTRYIDSscg = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                                    OLDAUDITENTRYIDSscg.InnerText = "-1";
                                    OLDAUDITENTRYIDSzcg.AppendChild(OLDAUDITENTRYIDSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(OLDAUDITENTRYIDSzcg);

                                    XmlElement LEDGERNAMEecg = xmldoc.CreateElement("LEDGERNAME");
                                    LEDGERNAMEecg.InnerText = "CGST " + bbd1.BOOKDETAIL_CgstPercent_num + "% Input Credit";
                                    ALLLEDGERENTRIESMcg.AppendChild(LEDGERNAMEecg);
                                    XmlElement GSTCLASSscg = xmldoc.CreateElement("GSTCLASS");
                                    ALLLEDGERENTRIESMcg.AppendChild(GSTCLASSscg);

                                    XmlElement ISDEEMEDPOSITIVEacg = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                    ISDEEMEDPOSITIVEacg.InnerText = "Yes";
                                    ALLLEDGERENTRIESMcg.AppendChild(ISDEEMEDPOSITIVEacg);

                                    XmlElement LEDGERFROMITEMacg = xmldoc.CreateElement("LEDGERFROMITEM");
                                    LEDGERFROMITEMacg.InnerText = "No";
                                    ALLLEDGERENTRIESMcg.AppendChild(LEDGERFROMITEMacg);

                                    XmlElement REMOVEZEROENTRIESqcg = xmldoc.CreateElement("REMOVEZEROENTRIES");
                                    REMOVEZEROENTRIESqcg.InnerText = "No";
                                    ALLLEDGERENTRIESMcg.AppendChild(REMOVEZEROENTRIESqcg);

                                    XmlElement ISPARTYLEDGERwcg = xmldoc.CreateElement("ISPARTYLEDGER");
                                    ISPARTYLEDGERwcg.InnerText = "No";
                                    ALLLEDGERENTRIESMcg.AppendChild(ISPARTYLEDGERwcg);

                                    XmlElement ISLASTDEEMEDPOSITIVErcg = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                                    ISLASTDEEMEDPOSITIVErcg.InnerText = "Yes";
                                    ALLLEDGERENTRIESMcg.AppendChild(ISLASTDEEMEDPOSITIVErcg);

                                    XmlElement ISCAPVATTAXALTEREDrcg = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                                    ISCAPVATTAXALTEREDrcg.InnerText = "No";
                                    ALLLEDGERENTRIESMcg.AppendChild(ISCAPVATTAXALTEREDrcg);

                                    XmlElement ISCAPVATNOTCLAIMEDdrcg = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                                    ISCAPVATNOTCLAIMEDdrcg.InnerText = "No";
                                    ALLLEDGERENTRIESMcg.AppendChild(ISCAPVATNOTCLAIMEDdrcg);

                                    XmlElement AMOUNTavcg = xmldoc.CreateElement("AMOUNT");
                                    AMOUNTavcg.InnerText = "-" + bbd1.BOOKDETAIL_CgstAmount_num.ToString();
                                    ALLLEDGERENTRIESMcg.AppendChild(AMOUNTavcg);

                                    XmlElement VATEXPAMOUNTcg = xmldoc.CreateElement("VATEXPAMOUNT");
                                    VATEXPAMOUNTcg.InnerText = "-" + bbd1.BOOKDETAIL_CgstAmount_num.ToString();
                                    ALLLEDGERENTRIESMcg.AppendChild(VATEXPAMOUNTcg);

                                    XmlElement SERVICETAXDETAILScg = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                                    ALLLEDGERENTRIESMcg.AppendChild(SERVICETAXDETAILScg);

                                    XmlElement BANKALLOCATIONSscg = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                                    XmlElement BILLALLOCATIONSscg = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                    XmlElement INTERESTCOLLECTIONscg = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                    XmlElement OLDAUDITENTRIESscg = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                                    XmlElement ACCOUNTAUDITENTRIESSscg = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                                    XmlElement AUDITENTRIESswcg = xmldoc.CreateElement("AUDITENTRIES.LIST");
                                    XmlElement INPUTCRALLOCSscg = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                                    XmlElement DUTYHEADDETAILSscg = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                                    XmlElement EXCISEDUTYHEADDETAILSscg = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                                    XmlElement RATEDETAILSscg = xmldoc.CreateElement("RATEDETAILS.LIST");
                                    XmlElement SUMMARYALLOCSscg = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                                    XmlElement STPYMTDETAILSscg = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                                    XmlElement EXCISEPAYMENTALLOCATIONSscg = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                                    XmlElement TAXBILLALLOCATIONSscg = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                                    XmlElement TAXOBJECTALLOCATIONSscg = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                                    XmlElement TDSEXPENSEALLOCATIONSscg = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                                    XmlElement VATSTATUTORYDETAILSSscg = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                                    XmlElement COSTTRACKALLOCATIONSscg = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                                    XmlElement REFVOUCHERDETAILscg = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                                    XmlElement INVOICEWISEDETAILscg = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                                    XmlElement VATITCDETAILscg = xmldoc.CreateElement("VATITCDETAILS.LIST");
                                    XmlElement ADVANCETAXDETAILscg = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                                    ALLLEDGERENTRIESMcg.AppendChild(BANKALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(BILLALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(INTERESTCOLLECTIONscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(OLDAUDITENTRIESscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(ACCOUNTAUDITENTRIESSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(AUDITENTRIESswcg);
                                    ALLLEDGERENTRIESMcg.AppendChild(INPUTCRALLOCSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(DUTYHEADDETAILSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(EXCISEDUTYHEADDETAILSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(RATEDETAILSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(SUMMARYALLOCSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(STPYMTDETAILSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(EXCISEPAYMENTALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(TAXBILLALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(TAXOBJECTALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(TDSEXPENSEALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(VATSTATUTORYDETAILSSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(COSTTRACKALLOCATIONSscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(REFVOUCHERDETAILscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(INVOICEWISEDETAILscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(VATITCDETAILscg);
                                    ALLLEDGERENTRIESMcg.AppendChild(ADVANCETAXDETAILscg);
                                    VOUCHER.AppendChild(ALLLEDGERENTRIESMcg);
                                }
                                //
                                //sgst
                                if (bbd1.BOOKDETAIL_SgstAmount_num > 0)
                                {
                                    XmlElement ALLLEDGERENTRIESMsg = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                                    XmlElement OLDAUDITENTRYIDSzsg = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                                    OLDAUDITENTRYIDSzsg.SetAttribute("TYPE", "Number");
                                    XmlElement OLDAUDITENTRYIDSssg = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                                    OLDAUDITENTRYIDSssg.InnerText = "-1";
                                    OLDAUDITENTRYIDSzsg.AppendChild(OLDAUDITENTRYIDSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(OLDAUDITENTRYIDSzsg);

                                    XmlElement LEDGERNAMEesg = xmldoc.CreateElement("LEDGERNAME");
                                    LEDGERNAMEesg.InnerText = "SGST " + bbd1.BOOKDETAIL_SgstPercent_num + "% Input Credit";
                                    ALLLEDGERENTRIESMsg.AppendChild(LEDGERNAMEesg);
                                    XmlElement GSTCLASSssg = xmldoc.CreateElement("GSTCLASS");
                                    ALLLEDGERENTRIESMsg.AppendChild(GSTCLASSssg);

                                    XmlElement ISDEEMEDPOSITIVEasg = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                    ISDEEMEDPOSITIVEasg.InnerText = "Yes";
                                    ALLLEDGERENTRIESMsg.AppendChild(ISDEEMEDPOSITIVEasg);

                                    XmlElement LEDGERFROMITEMasg = xmldoc.CreateElement("LEDGERFROMITEM");
                                    LEDGERFROMITEMasg.InnerText = "No";
                                    ALLLEDGERENTRIESMsg.AppendChild(LEDGERFROMITEMasg);

                                    XmlElement REMOVEZEROENTRIESqsg = xmldoc.CreateElement("REMOVEZEROENTRIES");
                                    REMOVEZEROENTRIESqsg.InnerText = "No";
                                    ALLLEDGERENTRIESMsg.AppendChild(REMOVEZEROENTRIESqsg);

                                    XmlElement ISPARTYLEDGERwsg = xmldoc.CreateElement("ISPARTYLEDGER");
                                    ISPARTYLEDGERwsg.InnerText = "No";
                                    ALLLEDGERENTRIESMsg.AppendChild(ISPARTYLEDGERwsg);

                                    XmlElement ISLASTDEEMEDPOSITIVErsg = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                                    //ISLASTDEEMEDPOSITIVEr.InnerText = "No";
                                    ISLASTDEEMEDPOSITIVErsg.InnerText = "Yes";
                                    ALLLEDGERENTRIESMsg.AppendChild(ISLASTDEEMEDPOSITIVErsg);

                                    XmlElement ISCAPVATTAXALTEREDrsg = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                                    ISCAPVATTAXALTEREDrsg.InnerText = "No";
                                    ALLLEDGERENTRIESMsg.AppendChild(ISCAPVATTAXALTEREDrsg);

                                    XmlElement ISCAPVATNOTCLAIMEDdrsg = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                                    ISCAPVATNOTCLAIMEDdrsg.InnerText = "No";
                                    ALLLEDGERENTRIESMsg.AppendChild(ISCAPVATNOTCLAIMEDdrsg);

                                    XmlElement AMOUNTavsg = xmldoc.CreateElement("AMOUNT");
                                    AMOUNTavsg.InnerText = "-" + bbd1.BOOKDETAIL_SgstAmount_num.ToString();
                                    ALLLEDGERENTRIESMsg.AppendChild(AMOUNTavsg);

                                    XmlElement VATEXPAMOUNTsg = xmldoc.CreateElement("VATEXPAMOUNT");
                                    VATEXPAMOUNTsg.InnerText = "-" + bbd1.BOOKDETAIL_SgstAmount_num.ToString();
                                    ALLLEDGERENTRIESMsg.AppendChild(VATEXPAMOUNTsg);

                                    XmlElement SERVICETAXDETAILSsg = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                                    ALLLEDGERENTRIESMsg.AppendChild(SERVICETAXDETAILSsg);

                                    XmlElement BANKALLOCATIONSssg = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                                    XmlElement BILLALLOCATIONSssg = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                    XmlElement INTERESTCOLLECTIONssg = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                    XmlElement OLDAUDITENTRIESssg = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                                    XmlElement ACCOUNTAUDITENTRIESSssg = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                                    XmlElement AUDITENTRIESswsg = xmldoc.CreateElement("AUDITENTRIES.LIST");
                                    XmlElement INPUTCRALLOCSssg = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                                    XmlElement DUTYHEADDETAILSssg = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                                    XmlElement EXCISEDUTYHEADDETAILSssg = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                                    XmlElement RATEDETAILSssg = xmldoc.CreateElement("RATEDETAILS.LIST");
                                    XmlElement SUMMARYALLOCSssg = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                                    XmlElement STPYMTDETAILSssg = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                                    XmlElement EXCISEPAYMENTALLOCATIONSssg = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                                    XmlElement TAXBILLALLOCATIONSssg = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                                    XmlElement TAXOBJECTALLOCATIONSssg = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                                    XmlElement TDSEXPENSEALLOCATIONSssg = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                                    XmlElement VATSTATUTORYDETAILSSssg = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                                    XmlElement COSTTRACKALLOCATIONSssg = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                                    XmlElement REFVOUCHERDETAILssg = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                                    XmlElement INVOICEWISEDETAILssg = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                                    XmlElement VATITCDETAILssg = xmldoc.CreateElement("VATITCDETAILS.LIST");
                                    XmlElement ADVANCETAXDETAILssg = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                                    ALLLEDGERENTRIESMsg.AppendChild(BANKALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(BILLALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(INTERESTCOLLECTIONssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(OLDAUDITENTRIESssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(ACCOUNTAUDITENTRIESSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(AUDITENTRIESswsg);
                                    ALLLEDGERENTRIESMsg.AppendChild(INPUTCRALLOCSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(DUTYHEADDETAILSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(EXCISEDUTYHEADDETAILSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(RATEDETAILSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(SUMMARYALLOCSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(STPYMTDETAILSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(EXCISEPAYMENTALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(TAXBILLALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(TAXOBJECTALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(TDSEXPENSEALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(VATSTATUTORYDETAILSSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(COSTTRACKALLOCATIONSssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(REFVOUCHERDETAILssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(INVOICEWISEDETAILssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(VATITCDETAILssg);
                                    ALLLEDGERENTRIESMsg.AppendChild(ADVANCETAXDETAILssg);
                                    VOUCHER.AppendChild(ALLLEDGERENTRIESMsg);
                                }
                                //
                                //igst
                                if (bbd1.BOOKDETAIL_IgstAmount_num > 0)
                                {
                                    XmlElement ALLLEDGERENTRIESMig = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                                    XmlElement OLDAUDITENTRYIDSzig = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                                    OLDAUDITENTRYIDSzig.SetAttribute("TYPE", "Number");
                                    XmlElement OLDAUDITENTRYIDSsig = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                                    OLDAUDITENTRYIDSsig.InnerText = "-1";
                                    OLDAUDITENTRYIDSzig.AppendChild(OLDAUDITENTRYIDSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(OLDAUDITENTRYIDSzig);

                                    XmlElement LEDGERNAMEeig = xmldoc.CreateElement("LEDGERNAME");
                                    LEDGERNAMEeig.InnerText = "IGST " + bbd1.BOOKDETAIL_IgstPercent_num + "% Input Credit";
                                    ALLLEDGERENTRIESMig.AppendChild(LEDGERNAMEeig);
                                    XmlElement GSTCLASSsig = xmldoc.CreateElement("GSTCLASS");
                                    ALLLEDGERENTRIESMig.AppendChild(GSTCLASSsig);

                                    XmlElement ISDEEMEDPOSITIVEaig = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                    ISDEEMEDPOSITIVEaig.InnerText = "Yes";
                                    ALLLEDGERENTRIESMig.AppendChild(ISDEEMEDPOSITIVEaig);

                                    XmlElement LEDGERFROMITEMaig = xmldoc.CreateElement("LEDGERFROMITEM");
                                    LEDGERFROMITEMaig.InnerText = "No";
                                    ALLLEDGERENTRIESMig.AppendChild(LEDGERFROMITEMaig);

                                    XmlElement REMOVEZEROENTRIESqig = xmldoc.CreateElement("REMOVEZEROENTRIES");
                                    REMOVEZEROENTRIESqig.InnerText = "No";
                                    ALLLEDGERENTRIESMig.AppendChild(REMOVEZEROENTRIESqig);

                                    XmlElement ISPARTYLEDGERwig = xmldoc.CreateElement("ISPARTYLEDGER");
                                    ISPARTYLEDGERwig.InnerText = "No";
                                    ALLLEDGERENTRIESMig.AppendChild(ISPARTYLEDGERwig);

                                    XmlElement ISLASTDEEMEDPOSITIVErig = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                                    ISLASTDEEMEDPOSITIVErig.InnerText = "Yes";
                                    ALLLEDGERENTRIESMig.AppendChild(ISLASTDEEMEDPOSITIVErig);

                                    XmlElement ISCAPVATTAXALTEREDrig = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                                    ISCAPVATTAXALTEREDrig.InnerText = "No";
                                    ALLLEDGERENTRIESMig.AppendChild(ISCAPVATTAXALTEREDrig);

                                    XmlElement ISCAPVATNOTCLAIMEDdrig = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                                    ISCAPVATNOTCLAIMEDdrig.InnerText = "No";
                                    ALLLEDGERENTRIESMig.AppendChild(ISCAPVATNOTCLAIMEDdrig);

                                    XmlElement AMOUNTavig = xmldoc.CreateElement("AMOUNT");
                                    AMOUNTavig.InnerText = "-" + bbd1.BOOKDETAIL_IgstAmount_num.ToString();
                                    ALLLEDGERENTRIESMig.AppendChild(AMOUNTavig);

                                    XmlElement VATEXPAMOUNTig = xmldoc.CreateElement("VATEXPAMOUNT");
                                    VATEXPAMOUNTig.InnerText = "-" + bbd1.BOOKDETAIL_IgstAmount_num.ToString();
                                    ALLLEDGERENTRIESMig.AppendChild(VATEXPAMOUNTig);

                                    XmlElement SERVICETAXDETAILSig = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                                    ALLLEDGERENTRIESMig.AppendChild(SERVICETAXDETAILSig);

                                    XmlElement BANKALLOCATIONSsig = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                                    XmlElement BILLALLOCATIONSsig = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                    XmlElement INTERESTCOLLECTIONsig = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                    XmlElement OLDAUDITENTRIESsig = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                                    XmlElement ACCOUNTAUDITENTRIESSsig = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                                    XmlElement AUDITENTRIESswig = xmldoc.CreateElement("AUDITENTRIES.LIST");
                                    XmlElement INPUTCRALLOCSsig = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                                    XmlElement DUTYHEADDETAILSsig = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                                    XmlElement EXCISEDUTYHEADDETAILSsig = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                                    XmlElement RATEDETAILSsig = xmldoc.CreateElement("RATEDETAILS.LIST");
                                    XmlElement SUMMARYALLOCSsig = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                                    XmlElement STPYMTDETAILSsig = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                                    XmlElement EXCISEPAYMENTALLOCATIONSsig = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                                    XmlElement TAXBILLALLOCATIONSsig = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                                    XmlElement TAXOBJECTALLOCATIONSsig = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                                    XmlElement TDSEXPENSEALLOCATIONSsig = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                                    XmlElement VATSTATUTORYDETAILSSsig = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                                    XmlElement COSTTRACKALLOCATIONSsig = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                                    XmlElement REFVOUCHERDETAILsig = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                                    XmlElement INVOICEWISEDETAILsig = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                                    XmlElement VATITCDETAILsig = xmldoc.CreateElement("VATITCDETAILS.LIST");
                                    XmlElement ADVANCETAXDETAILsig = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                                    ALLLEDGERENTRIESMig.AppendChild(BANKALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(BILLALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(INTERESTCOLLECTIONsig);
                                    ALLLEDGERENTRIESMig.AppendChild(OLDAUDITENTRIESsig);
                                    ALLLEDGERENTRIESMig.AppendChild(ACCOUNTAUDITENTRIESSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(AUDITENTRIESswig);
                                    ALLLEDGERENTRIESMig.AppendChild(INPUTCRALLOCSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(DUTYHEADDETAILSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(EXCISEDUTYHEADDETAILSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(RATEDETAILSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(SUMMARYALLOCSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(STPYMTDETAILSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(EXCISEPAYMENTALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(TAXBILLALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(TAXOBJECTALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(TDSEXPENSEALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(VATSTATUTORYDETAILSSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(COSTTRACKALLOCATIONSsig);
                                    ALLLEDGERENTRIESMig.AppendChild(REFVOUCHERDETAILsig);
                                    ALLLEDGERENTRIESMig.AppendChild(INVOICEWISEDETAILsig);
                                    ALLLEDGERENTRIESMig.AppendChild(VATITCDETAILsig);
                                    ALLLEDGERENTRIESMig.AppendChild(ADVANCETAXDETAILsig);
                                    VOUCHER.AppendChild(ALLLEDGERENTRIESMig);
                                }
                                //
                            }
                            else// if (bbd1.BOOKDETAIL_LedgerName_var.ToLower().Contains("tds") == false)
                            {
                                CreditAmt += Convert.ToDouble(bbd1.BOOKDETAIL_Amount_num.ToString());
                            }
                        }
                        

                        foreach (var bbd2 in billBookingDetail)
                        {
                            if (bbd2.BOOKDETAIL_DrCr_var == "Credit") // && bbd2.BOOKDETAIL_LedgerName_var.ToLower().Contains("tds") == true) //credit
                            {
                                XmlElement ALLLEDGERENTRIESMtds = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                                XmlElement OLDAUDITENTRYIDSztds = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                                OLDAUDITENTRYIDSztds.SetAttribute("TYPE", "Number");
                                XmlElement OLDAUDITENTRYIDSstds = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                                OLDAUDITENTRYIDSstds.InnerText = "-1";
                                OLDAUDITENTRYIDSztds.AppendChild(OLDAUDITENTRYIDSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(OLDAUDITENTRYIDSztds);

                                XmlElement LEDGERNAMEetds = xmldoc.CreateElement("LEDGERNAME");
                                LEDGERNAMEetds.InnerText = bbd2.BOOKDETAIL_LedgerName_var;
                                ALLLEDGERENTRIESMtds.AppendChild(LEDGERNAMEetds);
                                XmlElement GSTCLASSstds = xmldoc.CreateElement("GSTCLASS");
                                ALLLEDGERENTRIESMtds.AppendChild(GSTCLASSstds);

                                XmlElement ISDEEMEDPOSITIVEatds = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                                ISDEEMEDPOSITIVEatds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(ISDEEMEDPOSITIVEatds);

                                XmlElement LEDGERFROMITEMatds = xmldoc.CreateElement("LEDGERFROMITEM");
                                LEDGERFROMITEMatds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(LEDGERFROMITEMatds);

                                XmlElement REMOVEZEROENTRIESqtds = xmldoc.CreateElement("REMOVEZEROENTRIES");
                                REMOVEZEROENTRIESqtds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(REMOVEZEROENTRIESqtds);

                                XmlElement ISPARTYLEDGERwtds = xmldoc.CreateElement("ISPARTYLEDGER");
                                ISPARTYLEDGERwtds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(ISPARTYLEDGERwtds);

                                XmlElement ISLASTDEEMEDPOSITIVErtds = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                                ISLASTDEEMEDPOSITIVErtds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(ISLASTDEEMEDPOSITIVErtds);

                                XmlElement ISCAPVATTAXALTEREDrtds = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                                ISCAPVATTAXALTEREDrtds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(ISCAPVATTAXALTEREDrtds);

                                XmlElement ISCAPVATNOTCLAIMEDdrtds = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
                                ISCAPVATNOTCLAIMEDdrtds.InnerText = "No";
                                ALLLEDGERENTRIESMtds.AppendChild(ISCAPVATNOTCLAIMEDdrtds);

                                XmlElement AMOUNTavtds = xmldoc.CreateElement("AMOUNT");
                                //AMOUNTavtds.InnerText = bbd2.BOOKDETAIL_Amount_num.ToString();
                                AMOUNTavtds.InnerText = CreditAmt.ToString("0.00");
                                ALLLEDGERENTRIESMtds.AppendChild(AMOUNTavtds);

                                XmlElement VATEXPAMOUNTtds = xmldoc.CreateElement("VATEXPAMOUNT");
                                //VATEXPAMOUNTtds.InnerText = bbd2.BOOKDETAIL_Amount_num.ToString();
                                VATEXPAMOUNTtds.InnerText = CreditAmt.ToString("0.00");
                                ALLLEDGERENTRIESMtds.AppendChild(VATEXPAMOUNTtds);

                                XmlElement SERVICETAXDETAILStds = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                                ALLLEDGERENTRIESMtds.AppendChild(SERVICETAXDETAILStds);

                                XmlElement BANKALLOCATIONSstds = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                                XmlElement BILLALLOCATIONSstds = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                                XmlElement INTERESTCOLLECTIONstds = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                                XmlElement OLDAUDITENTRIESstds = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                                XmlElement ACCOUNTAUDITENTRIESSstds = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                                XmlElement AUDITENTRIESswtds = xmldoc.CreateElement("AUDITENTRIES.LIST");
                                XmlElement INPUTCRALLOCSstds = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                                XmlElement DUTYHEADDETAILSstds = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                                XmlElement EXCISEDUTYHEADDETAILSstds = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                                XmlElement RATEDETAILSstds = xmldoc.CreateElement("RATEDETAILS.LIST");
                                XmlElement SUMMARYALLOCSstds = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                                XmlElement STPYMTDETAILSstds = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                                XmlElement EXCISEPAYMENTALLOCATIONSstds = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                                XmlElement TAXBILLALLOCATIONSstds = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                                XmlElement TAXOBJECTALLOCATIONSstds = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                                XmlElement TDSEXPENSEALLOCATIONSstds = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                                XmlElement VATSTATUTORYDETAILSSstds = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                                XmlElement COSTTRACKALLOCATIONSstds = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                                XmlElement REFVOUCHERDETAILstds = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                                XmlElement INVOICEWISEDETAILstds = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                                XmlElement VATITCDETAILstds = xmldoc.CreateElement("VATITCDETAILS.LIST");
                                XmlElement ADVANCETAXDETAILstds = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                                ALLLEDGERENTRIESMtds.AppendChild(BANKALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(BILLALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(INTERESTCOLLECTIONstds);
                                ALLLEDGERENTRIESMtds.AppendChild(OLDAUDITENTRIESstds);
                                ALLLEDGERENTRIESMtds.AppendChild(ACCOUNTAUDITENTRIESSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(AUDITENTRIESswtds);
                                ALLLEDGERENTRIESMtds.AppendChild(INPUTCRALLOCSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(DUTYHEADDETAILSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(EXCISEDUTYHEADDETAILSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(RATEDETAILSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(SUMMARYALLOCSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(STPYMTDETAILSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(EXCISEPAYMENTALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(TAXBILLALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(TAXOBJECTALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(TDSEXPENSEALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(VATSTATUTORYDETAILSSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(COSTTRACKALLOCATIONSstds);
                                ALLLEDGERENTRIESMtds.AppendChild(REFVOUCHERDETAILstds);
                                ALLLEDGERENTRIESMtds.AppendChild(INVOICEWISEDETAILstds);
                                ALLLEDGERENTRIESMtds.AppendChild(VATITCDETAILstds);
                                ALLLEDGERENTRIESMtds.AppendChild(ADVANCETAXDETAILstds);
                                VOUCHER.AppendChild(ALLLEDGERENTRIESMtds);
                                break;
                            }
                        }
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INPUTCRALLOCS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("DUTYHEADDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("RATEDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("SUMMARYALLOCS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("STPYMTDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("REFVOUCHERDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INVOICEWISEDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATITCDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ADVANCETAXDETAILS.LIST"));
                        //VOUCHER.AppendChild(ALLLEDGERENTRIESmain);
                        XmlElement PAYROLLMODEOFPAYMENT = xmldoc.CreateElement("PAYROLLMODEOFPAYMENT.LIST");
                        VOUCHER.AppendChild(PAYROLLMODEOFPAYMENT);
                        XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
                        VOUCHER.AppendChild(ATTDRECORDS);
                        XmlElement GSTEWAYCONSIGNORADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNORADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNORADDRESS);
                        XmlElement GSTEWAYCONSIGNEEADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNEEADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNEEADDRESS);
                        XmlElement TEMPGSTRATEDETAILS = xmldoc.CreateElement("TEMPGSTRATEDETAILS.LIST");
                        VOUCHER.AppendChild(TEMPGSTRATEDETAILS);

                        TALLYMESSAGE.AppendChild(VOUCHER);
                        REQUESTDATA.AppendChild(TALLYMESSAGE);
                        dc.BillBooking_Update_TallyStatus(Convert.ToInt32(lblBookingNo.Text), true);
                        break;
                    }
                }
            }
            #endregion
            RootNode.AppendChild(HEADER);
            IMPORTDATA.AppendChild(REQUESTDESC);
            IMPORTDATA.AppendChild(REQUESTDATA);
            BODY.AppendChild(IMPORTDATA);
            RootNode.AppendChild(BODY);

            if (!Directory.Exists(@"d:\xml"))
                Directory.CreateDirectory(@"d:\xml");
            string fileName = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                fileName = "BILLBOOKING_Purchase_Mumbai";
            else if (cnStr.ToLower().Contains("nashik") == true)
                fileName = "BILLBOOKING_Purchase_Nashik";
            else if (cnStr.ToLower().Contains("metro") == true)
                fileName = "BILLBOOKING_Purchase_Metro";
            else
                fileName = "BILLBOOKING_Purchase_Pune";
            xmldoc.Save(@"d:\xml\" + fileName + ".xml");

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
            response.TransmitFile("d:\\xml\\" + fileName + ".xml");
            response.Flush();
            response.End();
        }

        //private void PurchaseTallyTransfer()
        //{
        //    XmlDocument xmldoc = new XmlDocument();
        //    XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
        //    xmldoc.AppendChild(RootNode);

        //    XmlElement HEADER = xmldoc.CreateElement("HEADER");
        //    XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
        //    TALLYREQUEST.InnerText = "Import Data";
        //    HEADER.AppendChild(TALLYREQUEST);
        //    RootNode.AppendChild(HEADER);

        //    XmlElement BODY = xmldoc.CreateElement("BODY");
        //    XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
        //    XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
        //    XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
        //    REPORTNAME.InnerText = "Vouchers";
        //    REQUESTDESC.AppendChild(REPORTNAME);

        //    XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
        //    XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");
        //    //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
        //    //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
        //    //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
        //    if (cnStr.ToLower().Contains("mumbai") == true)
        //        SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
        //    else if (cnStr.ToLower().Contains("nashik") == true)
        //        SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
        //    else
        //        SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010";
        //    STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
        //    REQUESTDESC.AppendChild(STATICVARIABLES);
        //    XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
        //    #region PURCHASE
        //    for (int i = 0; i < grdBillBooking.Rows.Count; i++)
        //    {
        //        CheckBox chkSelect = (CheckBox)grdBillBooking.Rows[i].FindControl("chkSelect");
        //        if (chkSelect.Checked == true)
        //        {
        //            Label lblBookingNo = (Label)grdBillBooking.Rows[i].FindControl("lblBookingNo");
        //            Label lblBookDate = (Label)grdBillBooking.Rows[i].FindControl("lblBookDate");
        //            Label lblSupplierInvoiceNo = (Label)grdBillBooking.Rows[i].FindControl("lblSupplierInvoiceNo");
        //            Label lblInvoiceDate = (Label)grdBillBooking.Rows[i].FindControl("lblInvoiceDate");
        //            Label lblTotalNetPayble = (Label)grdBillBooking.Rows[i].FindControl("lblTotalNetPayble");
        //            Label lblType = (Label)grdBillBooking.Rows[i].FindControl("lblType");
        //            Label lblVendor = (Label)grdBillBooking.Rows[i].FindControl("lblVendor");
        //            Label lblVendorId = (Label)grdBillBooking.Rows[i].FindControl("lblVendorId");
        //            Label lblNarration = (Label)grdBillBooking.Rows[i].FindControl("lblNarration");
        //            Label lblComment = (Label)grdBillBooking.Rows[i].FindControl("lblComment");

        //            XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
        //            XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
        //            string strAddress = "", strStateName = "", strCountryName = "", strGSTNo = "";
        //            var vendor = dc.Vendor_View(Convert.ToInt32(lblVendorId.Text), "", "");
        //            foreach (var vend in vendor)
        //            {
        //                strAddress = vend.VEND_Address_var;
        //                strStateName = vend.VEND_State_var;
        //                strCountryName = vend.VEND_Country_var;
        //                strGSTNo = vend.VEND_GSTNo_var;
        //            }
        //            double CreditAmt = 0;
        //            var billBookingDetail = dc.BillBookingDetail_View(Convert.ToInt32(lblBookingNo.Text)).ToList();
        //            foreach (var bbd in billBookingDetail)
        //            {

        //                TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");
        //                VOUCHER.SetAttribute("REMOTEID", "");
        //                VOUCHER.SetAttribute("VCHKEY", "");
        //                VOUCHER.SetAttribute("VCHTYPE", "Purchase");
        //                VOUCHER.SetAttribute("ACTION", "Create");
        //                VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

        //                XmlElement ADDRESSList = xmldoc.CreateElement("ADDRESS.LIST");
        //                ADDRESSList.SetAttribute("TYPE", "String");
        //                XmlElement ADDRESS = xmldoc.CreateElement("ADDRESS");
        //                ADDRESS.InnerText = strAddress;
        //                ADDRESSList.AppendChild(ADDRESS);
        //                VOUCHER.AppendChild(ADDRESSList);

        //                XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
        //                OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
        //                XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
        //                OLDAUDITENTRYIDSf.InnerText = "-1";
        //                OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
        //                VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

        //                XmlElement DATE = xmldoc.CreateElement("DATE");
        //                DateTime date = DateTime.ParseExact(lblBookDate.Text, "dd/MM/yyyy", null);
        //                DATE.InnerText = date.ToString("yyyyMMdd");
        //                VOUCHER.AppendChild(DATE);

        //                XmlElement REFERENCEDATE = xmldoc.CreateElement("REFERENCEDATE");
        //                REFERENCEDATE.InnerText = date.ToString("yyyyMMdd");
        //                VOUCHER.AppendChild(REFERENCEDATE);

        //                XmlElement GUID = xmldoc.CreateElement("GUID");
        //                GUID.InnerText = "";
        //                VOUCHER.AppendChild(GUID);

        //                XmlElement STATENAME = xmldoc.CreateElement("STATENAME");
        //                STATENAME.InnerText = strStateName;
        //                VOUCHER.AppendChild(STATENAME);

        //                XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
        //                NARRATION.InnerText = lblNarration.Text;
        //                VOUCHER.AppendChild(NARRATION);

        //                XmlElement COUNTRYOFRESIDENCE = xmldoc.CreateElement("COUNTRYOFRESIDENCE");
        //                COUNTRYOFRESIDENCE.InnerText = strCountryName;
        //                VOUCHER.AppendChild(COUNTRYOFRESIDENCE);

        //                XmlElement PARTYGSTIN = xmldoc.CreateElement("PARTYGSTIN");
        //                PARTYGSTIN.InnerText = strGSTNo;
        //                VOUCHER.AppendChild(PARTYGSTIN);

        //                XmlElement TAXUNITNAME = xmldoc.CreateElement("TAXUNITNAME");
        //                TAXUNITNAME.InnerText = "Default Tax Unit";
        //                VOUCHER.AppendChild(TAXUNITNAME);

        //                XmlElement PARTYNAME = xmldoc.CreateElement("PARTYNAME");
        //                PARTYNAME.InnerText = lblVendor.Text.Replace("&", "AND");
        //                VOUCHER.AppendChild(PARTYNAME);

        //                XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
        //                VOUCHERTYPENAME.InnerText = "Purchase";
        //                VOUCHER.AppendChild(VOUCHERTYPENAME);

        //                XmlElement REFERENCE = xmldoc.CreateElement("REFERENCE");
        //                REFERENCE.InnerText = lblSupplierInvoiceNo.Text;
        //                VOUCHER.AppendChild(REFERENCE);

        //                XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
        //                PARTYLEDGERNAME.InnerText = lblVendor.Text.Replace("&", "AND");
        //                VOUCHER.AppendChild(PARTYLEDGERNAME);

        //                XmlElement BASICBASEPARTYNAME = xmldoc.CreateElement("BASICBASEPARTYNAME");
        //                BASICBASEPARTYNAME.InnerText = lblVendor.Text.Replace("&", "AND");
        //                VOUCHER.AppendChild(BASICBASEPARTYNAME);

        //                XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
        //                VOUCHER.AppendChild(CSTFORMISSUETYPE);

        //                XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
        //                VOUCHER.AppendChild(CSTFORMRECVTYPE);

        //                //XmlElement BUYERSCSTNUMBER = xmldoc.CreateElement("BUYERSCSTNUMBER");
        //                //BUYERSCSTNUMBER.InnerText = "41002/S/435";
        //                //VOUCHER.AppendChild(BUYERSCSTNUMBER);

        //                XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
        //                FBTPAYMENTTYPE.InnerText = "Default";
        //                VOUCHER.AppendChild(FBTPAYMENTTYPE);

        //                XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
        //                PERSISTEDVIEW.InnerText = "Accounting Voucher View";
        //                VOUCHER.AppendChild(PERSISTEDVIEW);

        //                //XmlElement BASICBUYERSSALESTAXNO = xmldoc.CreateElement("BASICBUYERSSALESTAXNO");
        //                //BASICBUYERSSALESTAXNO.InnerText = "41002/S/435";
        //                //VOUCHER.AppendChild(BASICBUYERSSALESTAXNO);

        //                XmlElement BASICDATETIMEOFINVOICE = xmldoc.CreateElement("BASICDATETIMEOFINVOICE");
        //                DateTime dateInvoice = DateTime.ParseExact(lblInvoiceDate.Text, "dd/MM/yyyy", null);
        //                BASICDATETIMEOFINVOICE.InnerText = dateInvoice.ToString("dd-MMM-yyyy"); //2-Apr-2019 at 15:17
        //                VOUCHER.AppendChild(BASICDATETIMEOFINVOICE);

        //                XmlElement BASICDATETIMEOFREMOVAL = xmldoc.CreateElement("BASICDATETIMEOFREMOVAL");
        //                BASICDATETIMEOFREMOVAL.InnerText = dateInvoice.ToString("dd-MMM-yyyy"); //2-Apr-2019 at 15:17
        //                VOUCHER.AppendChild(BASICDATETIMEOFREMOVAL);

        //                XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
        //                VOUCHER.AppendChild(VCHGSTCLASS);

        //                XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
        //                ENTEREDBY.InnerText = "DESPL";
        //                VOUCHER.AppendChild(ENTEREDBY);

        //                //XmlElement VOUCHERTYPEORIGNAME = xmldoc.CreateElement("VOUCHERTYPEORIGNAME");
        //                //VOUCHERTYPEORIGNAME.InnerText = "Journal";
        //                //VOUCHER.AppendChild(VOUCHERTYPEORIGNAME);

        //                XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
        //                DIFFACTUALQTY.InnerText = "No";
        //                VOUCHER.AppendChild(DIFFACTUALQTY);

        //                XmlElement ISMSTFROMSYNC = xmldoc.CreateElement("ISMSTFROMSYNC");
        //                ISMSTFROMSYNC.InnerText = "No";
        //                VOUCHER.AppendChild(ISMSTFROMSYNC);

        //                XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
        //                ASORIGINAL.InnerText = "No";
        //                VOUCHER.AppendChild(ASORIGINAL);

        //                XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
        //                AUDITED.InnerText = "No";
        //                VOUCHER.AppendChild(AUDITED);

        //                XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
        //                FORJOBCOSTING.InnerText = "No";
        //                VOUCHER.AppendChild(FORJOBCOSTING);

        //                XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
        //                ISOPTIONAL.InnerText = "No";
        //                VOUCHER.AppendChild(ISOPTIONAL);

        //                XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
        //                DateTime date2 = DateTime.ParseExact(lblBookDate.Text, "dd/MM/yyyy", null);
        //                EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
        //                VOUCHER.AppendChild(EFFECTIVEDATE);

        //                XmlElement USEFOREXCISE = xmldoc.CreateElement("USEFOREXCISE");
        //                USEFOREXCISE.InnerText = "No";
        //                VOUCHER.AppendChild(USEFOREXCISE);

        //                XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
        //                ISFORJOBWORKIN.InnerText = "No";
        //                VOUCHER.AppendChild(ISFORJOBWORKIN);

        //                XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
        //                ALLOWCONSUMPTION.InnerText = "No";
        //                VOUCHER.AppendChild(ALLOWCONSUMPTION);

        //                XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
        //                USEFORINTEREST.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORINTEREST);

        //                XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
        //                USEFORGAINLOSS.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORGAINLOSS);

        //                XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
        //                USEFORGODOWNTRANSFER.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

        //                XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
        //                USEFORCOMPOUND.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORCOMPOUND);

        //                XmlElement USEFORSERVICETAX = xmldoc.CreateElement("USEFORSERVICETAX");
        //                USEFORSERVICETAX.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORSERVICETAX);


        //                XmlElement ISEXCISEVOUCHER = xmldoc.CreateElement("ISEXCISEVOUCHER");
        //                ISEXCISEVOUCHER.InnerText = "No";
        //                VOUCHER.AppendChild(ISEXCISEVOUCHER);

        //                XmlElement EXCISETAXOVERRIDE = xmldoc.CreateElement("EXCISETAXOVERRIDE");
        //                EXCISETAXOVERRIDE.InnerText = "No";
        //                VOUCHER.AppendChild(EXCISETAXOVERRIDE);

        //                XmlElement USEFORTAXUNITTRANSFER = xmldoc.CreateElement("USEFORTAXUNITTRANSFER");
        //                USEFORTAXUNITTRANSFER.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORTAXUNITTRANSFER);

        //                XmlElement IGNOREPOSVALIDATION = xmldoc.CreateElement("IGNOREPOSVALIDATION");
        //                IGNOREPOSVALIDATION.InnerText = "No";
        //                VOUCHER.AppendChild(IGNOREPOSVALIDATION);

        //                XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
        //                EXCISEOPENING.InnerText = "No";
        //                VOUCHER.AppendChild(EXCISEOPENING);

        //                XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
        //                USEFORFINALPRODUCTION.InnerText = "No";
        //                VOUCHER.AppendChild(USEFORFINALPRODUCTION);

        //                XmlElement ISTDSOVERRIDDEN = xmldoc.CreateElement("ISTDSOVERRIDDEN");
        //                ISTDSOVERRIDDEN.InnerText = "No";
        //                VOUCHER.AppendChild(ISTDSOVERRIDDEN);

        //                XmlElement ISTCSOVERRIDDEN = xmldoc.CreateElement("ISTCSOVERRIDDEN");
        //                ISTCSOVERRIDDEN.InnerText = "No";
        //                VOUCHER.AppendChild(ISTCSOVERRIDDEN);

        //                XmlElement ISTDSTCSCASHVCH = xmldoc.CreateElement("ISTDSTCSCASHVCH");
        //                ISTDSTCSCASHVCH.InnerText = "No";
        //                VOUCHER.AppendChild(ISTDSTCSCASHVCH);

        //                XmlElement INCLUDEADVPYMTVCH = xmldoc.CreateElement("INCLUDEADVPYMTVCH");
        //                INCLUDEADVPYMTVCH.InnerText = "No";
        //                VOUCHER.AppendChild(INCLUDEADVPYMTVCH);

        //                XmlElement ISSUBWORKSCONTRACT = xmldoc.CreateElement("ISSUBWORKSCONTRACT");
        //                ISSUBWORKSCONTRACT.InnerText = "No";
        //                VOUCHER.AppendChild(ISSUBWORKSCONTRACT);

        //                XmlElement ISVATOVERRIDDEN = xmldoc.CreateElement("ISVATOVERRIDDEN");
        //                ISVATOVERRIDDEN.InnerText = "No";
        //                VOUCHER.AppendChild(ISVATOVERRIDDEN);

        //                XmlElement IGNOREORIGVCHDATE = xmldoc.CreateElement("IGNOREORIGVCHDATE");
        //                IGNOREORIGVCHDATE.InnerText = "No";
        //                VOUCHER.AppendChild(IGNOREORIGVCHDATE);

        //                XmlElement ISVATPAIDATCUSTOMS = xmldoc.CreateElement("ISVATPAIDATCUSTOMS");
        //                ISVATPAIDATCUSTOMS.InnerText = "No";
        //                VOUCHER.AppendChild(ISVATPAIDATCUSTOMS);

        //                XmlElement ISDECLAREDTOCUSTOMS = xmldoc.CreateElement("ISDECLAREDTOCUSTOMS");
        //                ISDECLAREDTOCUSTOMS.InnerText = "No";
        //                VOUCHER.AppendChild(ISDECLAREDTOCUSTOMS);

        //                XmlElement ISSERVICETAXOVERRIDDEN = xmldoc.CreateElement("ISSERVICETAXOVERRIDDEN");
        //                ISSERVICETAXOVERRIDDEN.InnerText = "No";
        //                VOUCHER.AppendChild(ISSERVICETAXOVERRIDDEN);

        //                XmlElement ISISDVOUCHER = xmldoc.CreateElement("ISISDVOUCHER");
        //                ISISDVOUCHER.InnerText = "No";
        //                VOUCHER.AppendChild(ISISDVOUCHER);

        //                XmlElement ISEXCISEOVERRIDDEN = xmldoc.CreateElement("ISEXCISEOVERRIDDEN");
        //                ISEXCISEOVERRIDDEN.InnerText = "No";
        //                VOUCHER.AppendChild(ISEXCISEOVERRIDDEN);

        //                XmlElement ISEXCISESUPPLYVCH = xmldoc.CreateElement("ISEXCISESUPPLYVCH");
        //                ISEXCISESUPPLYVCH.InnerText = "No";
        //                VOUCHER.AppendChild(ISEXCISESUPPLYVCH);

        //                XmlElement ISGSTOVERRIDDEN = xmldoc.CreateElement("ISGSTOVERRIDDEN");
        //                ISGSTOVERRIDDEN.InnerText = "No";
        //                VOUCHER.AppendChild(ISGSTOVERRIDDEN);


        //                XmlElement GSTNOTEEXPORTED = xmldoc.CreateElement("GSTNOTEXPORTED");
        //                GSTNOTEEXPORTED.InnerText = "No";
        //                VOUCHER.AppendChild(GSTNOTEEXPORTED);

        //                XmlElement IGNOREGSTINVALIDATION = xmldoc.CreateElement("IGNOREGSTINVALIDATION");
        //                IGNOREGSTINVALIDATION.InnerText = "No";
        //                VOUCHER.AppendChild(IGNOREGSTINVALIDATION);

        //                XmlElement ISVATPRINCIPALACCOUNT = xmldoc.CreateElement("ISVATPRINCIPALACCOUNT");
        //                ISVATPRINCIPALACCOUNT.InnerText = "No";
        //                VOUCHER.AppendChild(ISVATPRINCIPALACCOUNT);

        //                XmlElement ISBOENOTAPPLICABLE = xmldoc.CreateElement("ISBOENOTAPPLICABLE");
        //                ISBOENOTAPPLICABLE.InnerText = "No";
        //                VOUCHER.AppendChild(ISBOENOTAPPLICABLE);

        //                XmlElement ISSHIPPINGWITHINSTATE = xmldoc.CreateElement("ISSHIPPINGWITHINSTATE");
        //                ISSHIPPINGWITHINSTATE.InnerText = "No";
        //                VOUCHER.AppendChild(ISSHIPPINGWITHINSTATE);

        //                XmlElement ISOVERSEASTOURISTTRANS = xmldoc.CreateElement("ISOVERSEASTOURISTTRANS");
        //                ISOVERSEASTOURISTTRANS.InnerText = "No";
        //                VOUCHER.AppendChild(ISOVERSEASTOURISTTRANS);

        //                XmlElement ISDESIGNATEDZONEPARTY = xmldoc.CreateElement("ISDESIGNATEDZONEPARTY");
        //                ISDESIGNATEDZONEPARTY.InnerText = "No";
        //                VOUCHER.AppendChild(ISDESIGNATEDZONEPARTY);

        //                XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
        //                ISCANCELLED.InnerText = "No";
        //                VOUCHER.AppendChild(ISCANCELLED);

        //                XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
        //                HASCASHFLOW.InnerText = "No";
        //                VOUCHER.AppendChild(HASCASHFLOW);

        //                XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
        //                ISPOSTDATED.InnerText = "No";
        //                VOUCHER.AppendChild(ISPOSTDATED);

        //                XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
        //                USETRACKINGNUMBER.InnerText = "No";
        //                VOUCHER.AppendChild(USETRACKINGNUMBER);

        //                XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
        //                ISINVOICE.InnerText = "No";
        //                VOUCHER.AppendChild(ISINVOICE);

        //                XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
        //                MFGJOURNAL.InnerText = "No";
        //                VOUCHER.AppendChild(MFGJOURNAL);

        //                XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
        //                HASDISCOUNTS.InnerText = "No";
        //                VOUCHER.AppendChild(HASDISCOUNTS);

        //                XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
        //                ASPAYSLIP.InnerText = "No";
        //                VOUCHER.AppendChild(ASPAYSLIP);

        //                XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
        //                ISCOSTCENTRE.InnerText = "No";
        //                VOUCHER.AppendChild(ISCOSTCENTRE);

        //                XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
        //                ISSTXNONREALIZEDVCH.InnerText = "No";
        //                VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

        //                XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
        //                ISEXCISEMANUFACTURERON.InnerText = "No";
        //                VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

        //                XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
        //                ISBLANKCHEQUE.InnerText = "No";
        //                VOUCHER.AppendChild(ISBLANKCHEQUE);

        //                XmlElement ISVOID = xmldoc.CreateElement("ISVOID");
        //                ISVOID.InnerText = "No";
        //                VOUCHER.AppendChild(ISVOID);

        //                XmlElement ISONHOLD = xmldoc.CreateElement("ISONHOLD");
        //                ISONHOLD.InnerText = "No";
        //                VOUCHER.AppendChild(ISONHOLD);

        //                XmlElement ORDERLINESTATUS = xmldoc.CreateElement("ORDERLINESTATUS");
        //                ORDERLINESTATUS.InnerText = "No";
        //                VOUCHER.AppendChild(ORDERLINESTATUS);

        //                XmlElement VATISAGNSTCANCSALES = xmldoc.CreateElement("VATISAGNSTCANCSALES");
        //                VATISAGNSTCANCSALES.InnerText = "No";
        //                VOUCHER.AppendChild(VATISAGNSTCANCSALES);

        //                XmlElement VATISPURCEXEMPTED = xmldoc.CreateElement("VATISPURCEXEMPTED");
        //                VATISPURCEXEMPTED.InnerText = "No";
        //                VOUCHER.AppendChild(VATISPURCEXEMPTED);

        //                XmlElement ISVATRESTAXINVOICE = xmldoc.CreateElement("ISVATRESTAXINVOICE");
        //                ISVATRESTAXINVOICE.InnerText = "No";
        //                VOUCHER.AppendChild(ISVATRESTAXINVOICE);

        //                XmlElement VATISASSESABLECALCVCH = xmldoc.CreateElement("VATISASSESABLECALCVCH");
        //                VATISASSESABLECALCVCH.InnerText = "No";
        //                VOUCHER.AppendChild(VATISASSESABLECALCVCH);

        //                XmlElement ISVATDUTYPAID = xmldoc.CreateElement("ISVATDUTYPAID");
        //                ISVATDUTYPAID.InnerText = "Yes";
        //                VOUCHER.AppendChild(ISVATDUTYPAID);

        //                XmlElement ISDELIVERYSAMEASCONSIGNEE = xmldoc.CreateElement("ISDELIVERYSAMEASCONSIGNEE");
        //                ISDELIVERYSAMEASCONSIGNEE.InnerText = "No";
        //                VOUCHER.AppendChild(ISDELIVERYSAMEASCONSIGNEE);

        //                XmlElement ISDISPATCHSAMEASCONSIGNOR = xmldoc.CreateElement("ISDISPATCHSAMEASCONSIGNOR");
        //                ISDISPATCHSAMEASCONSIGNOR.InnerText = "No";
        //                VOUCHER.AppendChild(ISDISPATCHSAMEASCONSIGNOR);

        //                XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
        //                ISDELETED.InnerText = "No";
        //                VOUCHER.AppendChild(ISDELETED);

        //                //XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
        //                //ASORIGINAL.InnerText = "No";
        //                //VOUCHER.AppendChild(ASORIGINAL);

        //                XmlElement CHANGEVCHMODE = xmldoc.CreateElement("CHANGEVCHMODE");
        //                CHANGEVCHMODE.InnerText = "No";
        //                VOUCHER.AppendChild(CHANGEVCHMODE);

        //                XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
        //                XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
        //                XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
        //                XmlElement EXCLUDEDTAXATION = xmldoc.CreateElement("EXCLUDEDTAXATION.LIST");
        //                XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
        //                XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
        //                XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
        //                XmlElement DUTYHEADDETAILS = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
        //                XmlElement SUPPLEMENTARYDUTYHEADDETAILS = xmldoc.CreateElement("SUPPLEMENTARYDUTYHEADDETAILS.LIST");
        //                XmlElement EWAYBILLDETAILS = xmldoc.CreateElement("EWAYBILLDETAILS.LIST");
        //                XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
        //                XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
        //                XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
        //                XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");
        //                XmlElement ORIGINVOICEDETAILS = xmldoc.CreateElement("ORIGINVOICEDETAILS.LIST");
        //                XmlElement INVOICEEXPORTLIST = xmldoc.CreateElement("INVOICEEXPORTLIST.LIST");

        //                ALTERID.InnerText = "";
        //                VOUCHER.AppendChild(ALTERID);

        //                MASTERID.InnerText = "";
        //                VOUCHER.AppendChild(MASTERID);

        //                VOUCHERKEY.InnerText = "";
        //                VOUCHER.AppendChild(VOUCHERKEY);

        //                VOUCHER.AppendChild(EXCLUDEDTAXATION);
        //                VOUCHER.AppendChild(OLDAUDITENTRIES);
        //                VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
        //                VOUCHER.AppendChild(AUDITENTRIES);
        //                VOUCHER.AppendChild(DUTYHEADDETAILS);
        //                VOUCHER.AppendChild(SUPPLEMENTARYDUTYHEADDETAILS);
        //                VOUCHER.AppendChild(EWAYBILLDETAILS);
        //                VOUCHER.AppendChild(INVOICEDELNOTES);
        //                VOUCHER.AppendChild(INVOICEORDERLIST);
        //                VOUCHER.AppendChild(INVOICEINDENTLIST);
        //                VOUCHER.AppendChild(ATTENDANCEENTRIES);
        //                VOUCHER.AppendChild(ORIGINVOICEDETAILS);
        //                VOUCHER.AppendChild(INVOICEEXPORTLIST);

        //                foreach (var bbd1 in billBookingDetail)
        //                {
        //                    if (bbd1.BOOKDETAIL_DrCr_var == "Debit")
        //                    {
        //                        XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
        //                        XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
        //                        OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
        //                        XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
        //                        OLDAUDITENTRYIDSs.InnerText = "-1";
        //                        OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
        //                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

        //                        XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
        //                        LEDGERNAMEe.InnerText = bbd1.BOOKDETAIL_LedgerName_var;
        //                        ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
        //                        XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
        //                        ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

        //                        XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
        //                        ISDEEMEDPOSITIVEa.InnerText = "Yes";
        //                        ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);

        //                        XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
        //                        LEDGERFROMITEMa.InnerText = "No";
        //                        ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

        //                        XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
        //                        REMOVEZEROENTRIESq.InnerText = "No";
        //                        ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

        //                        XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
        //                        ISPARTYLEDGERw.InnerText = "No";
        //                        ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

        //                        XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
        //                        //ISLASTDEEMEDPOSITIVEr.InnerText = "No";
        //                        ISLASTDEEMEDPOSITIVEr.InnerText = "Yes";
        //                        ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

        //                        XmlElement ISCAPVATTAXALTEREDr = xmldoc.CreateElement("ISCAPVATTAXALTERED");
        //                        ISCAPVATTAXALTEREDr.InnerText = "No";
        //                        ALLLEDGERENTRIESM.AppendChild(ISCAPVATTAXALTEREDr);

        //                        XmlElement ISCAPVATNOTCLAIMEDdr = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
        //                        ISCAPVATNOTCLAIMEDdr.InnerText = "No";
        //                        ALLLEDGERENTRIESM.AppendChild(ISCAPVATNOTCLAIMEDdr);

        //                        XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
        //                        AMOUNTav.InnerText = "-" + bbd1.BOOKDETAIL_Amount_num.ToString();
        //                        ALLLEDGERENTRIESM.AppendChild(AMOUNTav);

        //                        XmlElement VATEXPAMOUNT = xmldoc.CreateElement("VATEXPAMOUNT");
        //                        VATEXPAMOUNT.InnerText = "-" + bbd1.BOOKDETAIL_Amount_num.ToString();
        //                        ALLLEDGERENTRIESM.AppendChild(VATEXPAMOUNT);

        //                        XmlElement SERVICETAXDETAILS = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
        //                        ALLLEDGERENTRIESM.AppendChild(SERVICETAXDETAILS);

        //                        if (bbd1.BOOKDETAIL_Category_var != "---Select---" && bbd1.BOOKDETAIL_Category_var != "NA" && bbd1.BOOKDETAIL_Category_var != null)
        //                        {
        //                            XmlElement CATEGORYALLOCATIONS = xmldoc.CreateElement("CATEGORYALLOCATIONS.LIST");
        //                            XmlElement CATEGORY = xmldoc.CreateElement("CATEGORY");
        //                            CATEGORY.InnerText = bbd1.BOOKDETAIL_Category_var;
        //                            CATEGORYALLOCATIONS.AppendChild(CATEGORY);

        //                            XmlElement ISDEEMEDPOSITIVEc = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
        //                            ISDEEMEDPOSITIVEc.InnerText = "Yes";
        //                            CATEGORYALLOCATIONS.AppendChild(ISDEEMEDPOSITIVEc);

        //                            XmlElement COSTCENTREALLOCATIONS = xmldoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
        //                            XmlElement NAMEc = xmldoc.CreateElement("NAME");
        //                            NAMEc.InnerText = bbd1.BOOKDETAIL_CostCenter_var;
        //                            COSTCENTREALLOCATIONS.AppendChild(NAMEc);

        //                            XmlElement AMOUNTc = xmldoc.CreateElement("AMOUNT");
        //                            AMOUNTc.InnerText = "-" + bbd1.BOOKDETAIL_Amount_num.ToString();
        //                            COSTCENTREALLOCATIONS.AppendChild(AMOUNTc);

        //                            CATEGORYALLOCATIONS.AppendChild(COSTCENTREALLOCATIONS);
        //                            ALLLEDGERENTRIESM.AppendChild(CATEGORYALLOCATIONS);
        //                        }
        //                        XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
        //                        XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
        //                        XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
        //                        XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
        //                        XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
        //                        XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
        //                        XmlElement INPUTCRALLOCSs = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
        //                        XmlElement DUTYHEADDETAILSs = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
        //                        XmlElement EXCISEDUTYHEADDETAILSs = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
        //                        XmlElement RATEDETAILSs = xmldoc.CreateElement("RATEDETAILS.LIST");
        //                        XmlElement SUMMARYALLOCSs = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
        //                        XmlElement STPYMTDETAILSs = xmldoc.CreateElement("STPYMTDETAILS.LIST");
        //                        XmlElement EXCISEPAYMENTALLOCATIONSs = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
        //                        XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
        //                        XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
        //                        XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
        //                        XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
        //                        XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
        //                        XmlElement REFVOUCHERDETAILs = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
        //                        XmlElement INVOICEWISEDETAILs = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
        //                        XmlElement VATITCDETAILs = xmldoc.CreateElement("VATITCDETAILS.LIST");
        //                        XmlElement ADVANCETAXDETAILs = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
        //                        ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);
        //                        ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
        //                        ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
        //                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
        //                        ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
        //                        ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
        //                        ALLLEDGERENTRIESM.AppendChild(INPUTCRALLOCSs);
        //                        ALLLEDGERENTRIESM.AppendChild(DUTYHEADDETAILSs);
        //                        ALLLEDGERENTRIESM.AppendChild(EXCISEDUTYHEADDETAILSs);
        //                        ALLLEDGERENTRIESM.AppendChild(RATEDETAILSs);
        //                        ALLLEDGERENTRIESM.AppendChild(SUMMARYALLOCSs);
        //                        ALLLEDGERENTRIESM.AppendChild(STPYMTDETAILSs);
        //                        ALLLEDGERENTRIESM.AppendChild(EXCISEPAYMENTALLOCATIONSs);
        //                        ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
        //                        ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
        //                        ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
        //                        ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
        //                        ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
        //                        ALLLEDGERENTRIESM.AppendChild(REFVOUCHERDETAILs);
        //                        ALLLEDGERENTRIESM.AppendChild(INVOICEWISEDETAILs);
        //                        ALLLEDGERENTRIESM.AppendChild(VATITCDETAILs);
        //                        ALLLEDGERENTRIESM.AppendChild(ADVANCETAXDETAILs);
        //                        VOUCHER.AppendChild(ALLLEDGERENTRIESM);
        //                    }
        //                    else
        //                    {
        //                        CreditAmt += Convert.ToDouble(bbd1.BOOKDETAIL_Amount_num.ToString());
        //                    }
        //                }
        //                XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
        //                XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
        //                OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
        //                XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
        //                OLDAUDITENTRYIDSm.InnerText = "-1";
        //                OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
        //                ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

        //                XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
        //                LEDGERNAME.InnerText = lblVendor.Text.Replace("&", "AND");
        //                ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
        //                XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
        //                ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

        //                XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
        //                ISDEEMEDPOSITIVE.InnerText = "No";
        //                ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

        //                XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
        //                LEDGERFROMITEM.InnerText = "No";
        //                ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

        //                XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
        //                REMOVEZEROENTRIES.InnerText = "No";
        //                ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

        //                XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
        //                ISPARTYLEDGER.InnerText = "Yes";
        //                ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

        //                XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
        //                ISLASTDEEMEDPOSITIVE.InnerText = "Yes";
        //                ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);

        //                XmlElement ISCAPVATTAXALTERED = xmldoc.CreateElement("ISCAPVATTAXALTERED");
        //                ISCAPVATTAXALTERED.InnerText = "No";
        //                ALLLEDGERENTRIESmain.AppendChild(ISCAPVATTAXALTERED);

        //                XmlElement ISCAPVATNOTCLAIMED = xmldoc.CreateElement("ISCAPVATNOTCLAIMED");
        //                ISCAPVATNOTCLAIMED.InnerText = "No";
        //                ALLLEDGERENTRIESmain.AppendChild(ISCAPVATNOTCLAIMED);

        //                XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
        //                AMOUNT.InnerText = lblTotalNetPayble.Text;
        //                ALLLEDGERENTRIESmain.AppendChild(AMOUNT);

        //                XmlElement VATEXPAMOUNTs = xmldoc.CreateElement("VATEXPAMOUNT");
        //                VATEXPAMOUNTs.InnerText = lblTotalNetPayble.Text;
        //                ALLLEDGERENTRIESmain.AppendChild(VATEXPAMOUNTs);

        //                XmlElement SERVICETAXDETAILSs = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
        //                ALLLEDGERENTRIESmain.AppendChild(SERVICETAXDETAILSs);

        //                XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
        //                ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);
        //                ////
        //                foreach (var bbd2 in billBookingDetail)
        //                {
        //                    if (bbd2.BOOKDETAIL_DrCr_var == "Credit") //credit
        //                    {
        //                        XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
        //                        XmlElement NAME = xmldoc.CreateElement("NAME");
        //                        XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
        //                        NAME.InnerText = bbd2.BOOKDETAIL_LedgerName_var;
        //                        BILLALLOCATIONS.AppendChild(NAME);

        //                        BILLTYPE.InnerText = "Agst Ref";
        //                        BILLALLOCATIONS.AppendChild(BILLTYPE);

        //                        XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
        //                        TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
        //                        BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

        //                        XmlElement AMT = xmldoc.CreateElement("AMOUNT");
        //                        AMT.InnerText = bbd2.BOOKDETAIL_Amount_num.ToString();
        //                        BILLALLOCATIONS.AppendChild(AMT);

        //                        XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
        //                        BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
        //                        XmlElement STBILLCATEGORIES = xmldoc.CreateElement("STBILLCATEGORIES.LIST");
        //                        BILLALLOCATIONS.AppendChild(STBILLCATEGORIES);
        //                        ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
        //                    }
        //                }
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INPUTCRALLOCS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("DUTYHEADDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("RATEDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("SUMMARYALLOCS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("STPYMTDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("REFVOUCHERDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INVOICEWISEDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATITCDETAILS.LIST"));
        //                ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ADVANCETAXDETAILS.LIST"));
        //                VOUCHER.AppendChild(ALLLEDGERENTRIESmain);
        //                XmlElement PAYROLLMODEOFPAYMENT = xmldoc.CreateElement("PAYROLLMODEOFPAYMENT.LIST");
        //                VOUCHER.AppendChild(PAYROLLMODEOFPAYMENT);
        //                XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
        //                VOUCHER.AppendChild(ATTDRECORDS);
        //                XmlElement GSTEWAYCONSIGNORADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNORADDRESS.LIST");
        //                VOUCHER.AppendChild(GSTEWAYCONSIGNORADDRESS);
        //                XmlElement GSTEWAYCONSIGNEEADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNEEADDRESS.LIST");
        //                VOUCHER.AppendChild(GSTEWAYCONSIGNEEADDRESS);
        //                XmlElement TEMPGSTRATEDETAILS = xmldoc.CreateElement("TEMPGSTRATEDETAILS.LIST");
        //                VOUCHER.AppendChild(TEMPGSTRATEDETAILS);

        //                TALLYMESSAGE.AppendChild(VOUCHER);
        //                REQUESTDATA.AppendChild(TALLYMESSAGE);
        //                //dc.BillBooking_Update_TallyStatus(Convert.ToInt32(lblBookingNo.Text), true);
        //                break;
        //            }
        //        }
        //    }
        //    #endregion
        //    RootNode.AppendChild(HEADER);
        //    IMPORTDATA.AppendChild(REQUESTDESC);
        //    IMPORTDATA.AppendChild(REQUESTDATA);
        //    BODY.AppendChild(IMPORTDATA);
        //    RootNode.AppendChild(BODY);

        //    if (!Directory.Exists(@"d:\xml"))
        //        Directory.CreateDirectory(@"d:\xml");
        //    string fileName = "";
        //    if (cnStr.ToLower().Contains("mumbai") == true)
        //        fileName = "BILLBOOKING_Purchase_Mumbai";
        //    else if (cnStr.ToLower().Contains("nashik") == true)
        //        fileName = "BILLBOOKING_Purchase_Nashik";
        //    else
        //        fileName = "BILLBOOKING_Purchase_Pune";
        //    xmldoc.Save(@"d:\xml\" + fileName + ".xml");

        //    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        //    response.ClearContent();
        //    response.Clear();
        //    //response.ContentType = "text/plain";
        //    response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
        //    response.TransmitFile("d:\\xml\\" + fileName + ".xml");
        //    response.Flush();
        //    response.End();
        //}

        private void VendorCashPaymentTallyTransfer()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement RootNode = xmldoc.CreateElement("ENVELOPE");
            xmldoc.AppendChild(RootNode);

            XmlElement HEADER = xmldoc.CreateElement("HEADER");
            XmlElement TALLYREQUEST = xmldoc.CreateElement("TALLYREQUEST");
            TALLYREQUEST.InnerText = "Import Data";
            HEADER.AppendChild(TALLYREQUEST);
            RootNode.AppendChild(HEADER);

            XmlElement BODY = xmldoc.CreateElement("BODY");
            XmlElement IMPORTDATA = xmldoc.CreateElement("IMPORTDATA");
            XmlElement REQUESTDESC = xmldoc.CreateElement("REQUESTDESC");
            XmlElement REPORTNAME = xmldoc.CreateElement("REPORTNAME");
            REPORTNAME.InnerText = "Vouchers";
            REQUESTDESC.AppendChild(REPORTNAME);

            XmlElement STATICVARIABLES = xmldoc.CreateElement("STATICVARIABLES");
            XmlElement SVCURRENTCOMPANY = xmldoc.CreateElement("SVCURRENTCOMPANY");
            //pune - DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010
            //mumbai - DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)
            //nashik - Durocrete Engg Services Pvt Ltd ( Nashik )
            if (cnStr.ToLower().Contains("mumbai") == true)
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENGG.SERVICES PVT LTD(Mumbai)";
            else if (cnStr.ToLower().Contains("nashik") == true)
                SVCURRENTCOMPANY.InnerText = "Durocrete Engg Services Pvt Ltd ( Nashik )";
            else
                SVCURRENTCOMPANY.InnerText = "DUROCRETE ENG.SERVICES PVT LTD - From 1-Apr-2010";
            STATICVARIABLES.AppendChild(SVCURRENTCOMPANY);
            REQUESTDESC.AppendChild(STATICVARIABLES);
            XmlElement REQUESTDATA = xmldoc.CreateElement("REQUESTDATA");
            #region receipt
            for (int i = 0; i < grdBillBooking.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdBillBooking.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblBookingNo = (Label)grdBillBooking.Rows[i].FindControl("lblBookingNo");
                    Label lblBookDate = (Label)grdBillBooking.Rows[i].FindControl("lblBookDate");
                    Label lblSupplierInvoiceNo = (Label)grdBillBooking.Rows[i].FindControl("lblSupplierInvoiceNo");
                    Label lblInvoiceDate = (Label)grdBillBooking.Rows[i].FindControl("lblInvoiceDate");
                    Label lblTotalNetPayble = (Label)grdBillBooking.Rows[i].FindControl("lblTotalNetPayble");
                    Label lblType = (Label)grdBillBooking.Rows[i].FindControl("lblType");
                    Label lblVendor = (Label)grdBillBooking.Rows[i].FindControl("lblVendor");
                    Label lblNarration = (Label)grdBillBooking.Rows[i].FindControl("lblNarration");
                    Label lblComment = (Label)grdBillBooking.Rows[i].FindControl("lblComment");

                    //var cashpayment = dc.CashPayment_View(Convert.ToInt32(lblBookingNo.Text), lblSupplierInvoiceNo.Text, null, null, "");
                    var cashpayment = dc.CashPayment_View(Convert.ToInt32(lblBookingNo.Text), lblSupplierInvoiceNo.Text, null, null);
                    foreach (var rcptd in cashpayment)
                    {
                        XmlElement TALLYMESSAGE = xmldoc.CreateElement("TALLYMESSAGE");
                        TALLYMESSAGE.SetAttribute("xmlns:UDF", "TallyUDF");

                        XmlElement VOUCHER = xmldoc.CreateElement("VOUCHER");
                        VOUCHER.SetAttribute("REMOTEID", "");
                        VOUCHER.SetAttribute("VCHKEY", "");
                        VOUCHER.SetAttribute("VCHTYPE", "Payment");
                        VOUCHER.SetAttribute("ACTION", "Create");
                        VOUCHER.SetAttribute("OBJVIEW", "Accounting Voucher View");

                        XmlElement OLDAUDITENTRYIDSem = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSem.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSf = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSf.InnerText = "-1";
                        OLDAUDITENTRYIDSem.AppendChild(OLDAUDITENTRYIDSf);
                        VOUCHER.AppendChild(OLDAUDITENTRYIDSem);

                        XmlElement GUID = xmldoc.CreateElement("GUID");
                        XmlElement DATE = xmldoc.CreateElement("DATE");
                        DateTime date = DateTime.ParseExact(lblInvoiceDate.Text, "dd/MM/yyyy", null);
                        DATE.InnerText = date.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(DATE);
                        GUID.InnerText = "";
                        VOUCHER.AppendChild(GUID);

                        XmlElement NARRATION = xmldoc.CreateElement("NARRATION");
                        //string NarrationStr = "";
                        //if (rcptd.Cash_)
                        //{
                        //    NarrationStr = NarrationStr + "AS PER CASH BOOK ";
                        //}
                        //else
                        //{
                        //    NarrationStr = "Ch. No. " + Convert.ToInt32(rcptd.Cash_ChequeNo_int).ToString("000000");
                        //    NarrationStr += " DT " + Convert.ToDateTime(rcptd.Cash_ChqDate_date).ToString("dd/MM/yyyy");
                        //    NarrationStr += " " + rcptd.Cash_BankName_var;
                        //    NarrationStr += " " + rcptd.Cash_Branch_var;
                        //}
                        //NarrationStr += rcptd.Cash_Note_var;
                        //if (rcptd.Cash_CollectionDetail != null && rcptd.Cash_CollectionDetail != "")
                        //{
                        //    string[] CollectionDetail = new string[1];
                        //    CollectionDetail = Convert.ToString(rcptd.Cash_CollectionDetail).Split('|');
                        //    NarrationStr += " " + Convert.ToString(CollectionDetail[0]);
                        //    NarrationStr += " - " + Convert.ToString(CollectionDetail[1]);
                        //}
                        //NARRATION.InnerText = NarrationStr;
                        NARRATION.InnerText = lblNarration.Text;
                        VOUCHER.AppendChild(NARRATION);

                        XmlElement TAXUNITNAME = xmldoc.CreateElement("TAXUNITNAME");
                        TAXUNITNAME.InnerText = "Default Tax Unit";
                        VOUCHER.AppendChild(TAXUNITNAME);

                        XmlElement VOUCHERTYPENAME = xmldoc.CreateElement("VOUCHERTYPENAME");
                        if (lblType.Text == "Cash")
                            VOUCHERTYPENAME.InnerText = "PAYMENT";
                        else
                            VOUCHERTYPENAME.InnerText = lblComment.Text; 

                        VOUCHER.AppendChild(VOUCHERTYPENAME);

                        XmlElement VOUCHERNUMBER = xmldoc.CreateElement("VOUCHERNUMBER");
                        //if (DateTime.Now.Month <= 3)
                        //    NarrationStr = (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
                        //else
                        //    NarrationStr = (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();

                        //NarrationStr = lblBillNo.Text.Replace("-", "") + "/" + NarrationStr;
                        VOUCHERNUMBER.InnerText = lblSupplierInvoiceNo.Text;
                        VOUCHER.AppendChild(VOUCHERNUMBER);

                        XmlElement CSTFORMISSUETYPE = xmldoc.CreateElement("CSTFORMISSUETYPE");
                        XmlElement CSTFORMRECVTYPE = xmldoc.CreateElement("CSTFORMRECVTYPE");
                        XmlElement PARTYLEDGERNAME = xmldoc.CreateElement("PARTYLEDGERNAME");
                        PARTYLEDGERNAME.InnerText = lblVendor.Text.Replace("&", "AND");
                        VOUCHER.AppendChild(PARTYLEDGERNAME);
                        VOUCHER.AppendChild(CSTFORMISSUETYPE);
                        VOUCHER.AppendChild(CSTFORMRECVTYPE);

                        XmlElement FBTPAYMENTTYPE = xmldoc.CreateElement("FBTPAYMENTTYPE");
                        FBTPAYMENTTYPE.InnerText = "Default";
                        VOUCHER.AppendChild(FBTPAYMENTTYPE);

                        XmlElement VCHGSTCLASS = xmldoc.CreateElement("VCHGSTCLASS");
                        XmlElement PERSISTEDVIEW = xmldoc.CreateElement("PERSISTEDVIEW");
                        PERSISTEDVIEW.InnerText = "Accounting Voucher View";
                        VOUCHER.AppendChild(PERSISTEDVIEW);
                        VOUCHER.AppendChild(VCHGSTCLASS);

                        XmlElement ENTEREDBY = xmldoc.CreateElement("ENTEREDBY");
                        ENTEREDBY.InnerText = "DESPL";
                        VOUCHER.AppendChild(ENTEREDBY);

                        //if (rcptd.CASHPAY_PaymentStatus_var == "Cash")
                        //{
                        //    XmlElement VOUCHERTYPEORIGNAME = xmldoc.CreateElement("VOUCHERTYPEORIGNAME");
                        //    VOUCHERTYPEORIGNAME.InnerText = "Receipt";
                        //    VOUCHER.AppendChild(VOUCHERTYPEORIGNAME);
                        //}

                        XmlElement DIFFACTUALQTY = xmldoc.CreateElement("DIFFACTUALQTY");
                        DIFFACTUALQTY.InnerText = "No";
                        VOUCHER.AppendChild(DIFFACTUALQTY);

                        XmlElement ISMSTFROMSYNC = xmldoc.CreateElement("ISMSTFROMSYNC");
                        ISMSTFROMSYNC.InnerText = "No";
                        VOUCHER.AppendChild(ISMSTFROMSYNC);

                        XmlElement ASORIGINAL = xmldoc.CreateElement("ASORIGINAL");
                        ASORIGINAL.InnerText = "No";
                        VOUCHER.AppendChild(ASORIGINAL);

                        XmlElement AUDITED = xmldoc.CreateElement("AUDITED");
                        AUDITED.InnerText = "No";
                        VOUCHER.AppendChild(AUDITED);

                        XmlElement FORJOBCOSTING = xmldoc.CreateElement("FORJOBCOSTING");
                        FORJOBCOSTING.InnerText = "No";
                        VOUCHER.AppendChild(FORJOBCOSTING);

                        XmlElement ISOPTIONAL = xmldoc.CreateElement("ISOPTIONAL");
                        ISOPTIONAL.InnerText = "No";
                        VOUCHER.AppendChild(ISOPTIONAL);

                        XmlElement EFFECTIVEDATE = xmldoc.CreateElement("EFFECTIVEDATE");
                        DateTime date2 = DateTime.ParseExact(lblInvoiceDate.Text, "dd/MM/yyyy", null);
                        EFFECTIVEDATE.InnerText = date2.ToString("yyyyMMdd");
                        VOUCHER.AppendChild(EFFECTIVEDATE);

                        XmlElement USEFOREXCISE = xmldoc.CreateElement("USEFOREXCISE");
                        USEFOREXCISE.InnerText = "No";
                        VOUCHER.AppendChild(USEFOREXCISE);

                        XmlElement ISFORJOBWORKIN = xmldoc.CreateElement("ISFORJOBWORKIN");
                        ISFORJOBWORKIN.InnerText = "No";
                        VOUCHER.AppendChild(ISFORJOBWORKIN);

                        XmlElement ALLOWCONSUMPTION = xmldoc.CreateElement("ALLOWCONSUMPTION");
                        ALLOWCONSUMPTION.InnerText = "No";
                        VOUCHER.AppendChild(ALLOWCONSUMPTION);

                        XmlElement USEFORINTEREST = xmldoc.CreateElement("USEFORINTEREST");
                        USEFORINTEREST.InnerText = "No";
                        VOUCHER.AppendChild(USEFORINTEREST);

                        XmlElement USEFORGAINLOSS = xmldoc.CreateElement("USEFORGAINLOSS");
                        USEFORGAINLOSS.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGAINLOSS);

                        XmlElement USEFORGODOWNTRANSFER = xmldoc.CreateElement("USEFORGODOWNTRANSFER");
                        USEFORGODOWNTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORGODOWNTRANSFER);

                        XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                        USEFORCOMPOUND.InnerText = "No";
                        VOUCHER.AppendChild(USEFORCOMPOUND);


                        XmlElement USEFORSERVICETAX = xmldoc.CreateElement("USEFORSERVICETAX");
                        USEFORSERVICETAX.InnerText = "No";
                        VOUCHER.AppendChild(USEFORSERVICETAX);

                        XmlElement ISEXCISEVOUCHER = xmldoc.CreateElement("ISEXCISEVOUCHER");
                        ISEXCISEVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEVOUCHER);

                        XmlElement EXCISETAXOVERRIDE = xmldoc.CreateElement("EXCISETAXOVERRIDE");
                        EXCISETAXOVERRIDE.InnerText = "No";
                        VOUCHER.AppendChild(EXCISETAXOVERRIDE);

                        XmlElement USEFORTAXUNITTRANSFER = xmldoc.CreateElement("USEFORTAXUNITTRANSFER");
                        USEFORTAXUNITTRANSFER.InnerText = "No";
                        VOUCHER.AppendChild(USEFORTAXUNITTRANSFER);

                        XmlElement EXCISEOPENING = xmldoc.CreateElement("EXCISEOPENING");
                        EXCISEOPENING.InnerText = "No";
                        VOUCHER.AppendChild(EXCISEOPENING);

                        XmlElement USEFORFINALPRODUCTION = xmldoc.CreateElement("USEFORFINALPRODUCTION");
                        USEFORFINALPRODUCTION.InnerText = "No";
                        VOUCHER.AppendChild(USEFORFINALPRODUCTION);

                        XmlElement ISTDSOVERRIDDEN = xmldoc.CreateElement("ISTDSOVERRIDDEN");
                        ISTDSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSOVERRIDDEN);

                        XmlElement ISTCSOVERRIDDEN = xmldoc.CreateElement("ISTCSOVERRIDDEN");
                        ISTCSOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISTCSOVERRIDDEN);

                        XmlElement ISTDSTCSCASHVCH = xmldoc.CreateElement("ISTDSTCSCASHVCH");
                        ISTDSTCSCASHVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISTDSTCSCASHVCH);

                        XmlElement INCLUDEADVPYMTVCH = xmldoc.CreateElement("INCLUDEADVPYMTVCH");
                        INCLUDEADVPYMTVCH.InnerText = "No";
                        VOUCHER.AppendChild(INCLUDEADVPYMTVCH);

                        XmlElement ISSUBWORKSCONTRACT = xmldoc.CreateElement("ISSUBWORKSCONTRACT");
                        ISSUBWORKSCONTRACT.InnerText = "No";
                        VOUCHER.AppendChild(ISSUBWORKSCONTRACT);

                        XmlElement ISVATOVERRIDDEN = xmldoc.CreateElement("ISVATOVERRIDDEN");
                        ISVATOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISVATOVERRIDDEN);

                        XmlElement IGNOREORIGVCHDATE = xmldoc.CreateElement("IGNOREORIGVCHDATE");
                        IGNOREORIGVCHDATE.InnerText = "No";
                        VOUCHER.AppendChild(IGNOREORIGVCHDATE);

                        XmlElement ISVATPAIDATCUSTOMS = xmldoc.CreateElement("ISVATPAIDATCUSTOMS");
                        ISVATPAIDATCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPAIDATCUSTOMS);

                        XmlElement ISDECLAREDTOCUSTOMS = xmldoc.CreateElement("ISDECLAREDTOCUSTOMS");
                        ISDECLAREDTOCUSTOMS.InnerText = "No";
                        VOUCHER.AppendChild(ISDECLAREDTOCUSTOMS);

                        XmlElement ISSERVICETAXOVERRIDDEN = xmldoc.CreateElement("ISSERVICETAXOVERRIDDEN");
                        ISSERVICETAXOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISSERVICETAXOVERRIDDEN);

                        XmlElement ISISDVOUCHER = xmldoc.CreateElement("ISISDVOUCHER");
                        ISISDVOUCHER.InnerText = "No";
                        VOUCHER.AppendChild(ISISDVOUCHER);

                        XmlElement ISEXCISEOVERRIDDEN = xmldoc.CreateElement("ISEXCISEOVERRIDDEN");
                        ISEXCISEOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEOVERRIDDEN);

                        XmlElement ISEXCISESUPPLYVCH = xmldoc.CreateElement("ISEXCISESUPPLYVCH");
                        ISEXCISESUPPLYVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISESUPPLYVCH);

                        XmlElement ISGSTOVERRIDDEN = xmldoc.CreateElement("ISGSTOVERRIDDEN");
                        ISGSTOVERRIDDEN.InnerText = "No";
                        VOUCHER.AppendChild(ISGSTOVERRIDDEN);


                        XmlElement GSTNOTEEXPORTED = xmldoc.CreateElement("GSTNOTEXPORTED");
                        GSTNOTEEXPORTED.InnerText = "No";
                        VOUCHER.AppendChild(GSTNOTEEXPORTED);

                        XmlElement ISVATPRINCIPALACCOUNT = xmldoc.CreateElement("ISVATPRINCIPALACCOUNT");
                        ISVATPRINCIPALACCOUNT.InnerText = "No";
                        VOUCHER.AppendChild(ISVATPRINCIPALACCOUNT);

                        XmlElement ISBOENOTAPPLICABLE = xmldoc.CreateElement("ISBOENOTAPPLICABLE");
                        ISBOENOTAPPLICABLE.InnerText = "No";
                        VOUCHER.AppendChild(ISBOENOTAPPLICABLE);

                        XmlElement ISSHIPPINGWITHINSTATE = xmldoc.CreateElement("ISSHIPPINGWITHINSTATE");
                        ISSHIPPINGWITHINSTATE.InnerText = "No";
                        VOUCHER.AppendChild(ISSHIPPINGWITHINSTATE);

                        XmlElement ISOVERSEASTOURISTTRANS = xmldoc.CreateElement("ISOVERSEASTOURISTTRANS");
                        ISOVERSEASTOURISTTRANS.InnerText = "No";
                        VOUCHER.AppendChild(ISOVERSEASTOURISTTRANS);

                        XmlElement ISCANCELLED = xmldoc.CreateElement("ISCANCELLED");
                        ISCANCELLED.InnerText = "No";
                        VOUCHER.AppendChild(ISCANCELLED);

                        XmlElement HASCASHFLOW = xmldoc.CreateElement("HASCASHFLOW");
                        HASCASHFLOW.InnerText = "Yes";
                        VOUCHER.AppendChild(HASCASHFLOW);

                        XmlElement ISPOSTDATED = xmldoc.CreateElement("ISPOSTDATED");
                        ISPOSTDATED.InnerText = "No";
                        VOUCHER.AppendChild(ISPOSTDATED);

                        XmlElement USETRACKINGNUMBER = xmldoc.CreateElement("USETRACKINGNUMBER");
                        USETRACKINGNUMBER.InnerText = "No";
                        VOUCHER.AppendChild(USETRACKINGNUMBER);

                        XmlElement ISINVOICE = xmldoc.CreateElement("ISINVOICE");
                        ISINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISINVOICE);

                        XmlElement MFGJOURNAL = xmldoc.CreateElement("MFGJOURNAL");
                        MFGJOURNAL.InnerText = "No";
                        VOUCHER.AppendChild(MFGJOURNAL);

                        XmlElement HASDISCOUNTS = xmldoc.CreateElement("HASDISCOUNTS");
                        HASDISCOUNTS.InnerText = "No";
                        VOUCHER.AppendChild(HASDISCOUNTS);

                        XmlElement ASPAYSLIP = xmldoc.CreateElement("ASPAYSLIP");
                        ASPAYSLIP.InnerText = "No";
                        VOUCHER.AppendChild(ASPAYSLIP);

                        XmlElement ISCOSTCENTRE = xmldoc.CreateElement("ISCOSTCENTRE");
                        ISCOSTCENTRE.InnerText = "No";
                        VOUCHER.AppendChild(ISCOSTCENTRE);

                        XmlElement ISSTXNONREALIZEDVCH = xmldoc.CreateElement("ISSTXNONREALIZEDVCH");
                        ISSTXNONREALIZEDVCH.InnerText = "No";
                        VOUCHER.AppendChild(ISSTXNONREALIZEDVCH);

                        XmlElement ISEXCISEMANUFACTURERON = xmldoc.CreateElement("ISEXCISEMANUFACTURERON");
                        ISEXCISEMANUFACTURERON.InnerText = "No";
                        VOUCHER.AppendChild(ISEXCISEMANUFACTURERON);

                        XmlElement ISBLANKCHEQUE = xmldoc.CreateElement("ISBLANKCHEQUE");
                        ISBLANKCHEQUE.InnerText = "No";
                        VOUCHER.AppendChild(ISBLANKCHEQUE);

                        XmlElement ISVOID = xmldoc.CreateElement("ISVOID");
                        ISVOID.InnerText = "No";
                        VOUCHER.AppendChild(ISVOID);

                        XmlElement ISONHOLD = xmldoc.CreateElement("ISONHOLD");
                        ISONHOLD.InnerText = "No";
                        VOUCHER.AppendChild(ISONHOLD);

                        XmlElement ORDERLINESTATUS = xmldoc.CreateElement("ORDERLINESTATUS");
                        ORDERLINESTATUS.InnerText = "No";
                        VOUCHER.AppendChild(ORDERLINESTATUS);

                        XmlElement VATISAGNSTCANCSALES = xmldoc.CreateElement("VATISAGNSTCANCSALES");
                        VATISAGNSTCANCSALES.InnerText = "No";
                        VOUCHER.AppendChild(VATISAGNSTCANCSALES);

                        XmlElement VATISPURCEXEMPTED = xmldoc.CreateElement("VATISPURCEXEMPTED");
                        VATISPURCEXEMPTED.InnerText = "No";
                        VOUCHER.AppendChild(VATISPURCEXEMPTED);

                        XmlElement ISVATRESTAXINVOICE = xmldoc.CreateElement("ISVATRESTAXINVOICE");
                        ISVATRESTAXINVOICE.InnerText = "No";
                        VOUCHER.AppendChild(ISVATRESTAXINVOICE);

                        XmlElement VATISASSESABLECALCVCH = xmldoc.CreateElement("VATISASSESABLECALCVCH");
                        VATISASSESABLECALCVCH.InnerText = "No";
                        VOUCHER.AppendChild(VATISASSESABLECALCVCH);

                        XmlElement ISVATDUTYPAID = xmldoc.CreateElement("ISVATDUTYPAID");
                        ISVATDUTYPAID.InnerText = "Yes";
                        VOUCHER.AppendChild(ISVATDUTYPAID);

                        XmlElement ISDELIVERYSAMEASCONSIGNEE = xmldoc.CreateElement("ISDELIVERYSAMEASCONSIGNEE");
                        ISDELIVERYSAMEASCONSIGNEE.InnerText = "No";
                        VOUCHER.AppendChild(ISDELIVERYSAMEASCONSIGNEE);

                        XmlElement ISDISPATCHSAMEASCONSIGNOR = xmldoc.CreateElement("ISDISPATCHSAMEASCONSIGNOR");
                        ISDISPATCHSAMEASCONSIGNOR.InnerText = "No";
                        VOUCHER.AppendChild(ISDISPATCHSAMEASCONSIGNOR);

                        XmlElement ISDELETED = xmldoc.CreateElement("ISDELETED");
                        ISDELETED.InnerText = "No";
                        VOUCHER.AppendChild(ISDELETED);

                        XmlElement CHANGEVCHMODE = xmldoc.CreateElement("CHANGEVCHMODE");
                        CHANGEVCHMODE.InnerText = "No";
                        VOUCHER.AppendChild(CHANGEVCHMODE);

                        //XmlElement VCHISFROMSYNC = xmldoc.CreateElement("VCHISFROMSYNC");
                        //VCHISFROMSYNC.InnerText = "No";
                        //VOUCHER.AppendChild(VCHISFROMSYNC);

                        //XmlElement USEFORCOMPOUND = xmldoc.CreateElement("USEFORCOMPOUND");
                        //USEFORCOMPOUND.InnerText = "No";
                        //VOUCHER.AppendChild(USEFORCOMPOUND);


                        XmlElement ALTERID = xmldoc.CreateElement("ALTERID");
                        XmlElement MASTERID = xmldoc.CreateElement("MASTERID");
                        XmlElement VOUCHERKEY = xmldoc.CreateElement("VOUCHERKEY");
                        XmlElement EXCLUDEDTAXATIONS = xmldoc.CreateElement("EXCLUDEDTAXATIONS.LIST");
                        XmlElement OLDAUDITENTRIES = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIES = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIES = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement DUTYHEADDETAILS = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                        XmlElement SUPPLEMENTARYDUTYHEADDETAILS = xmldoc.CreateElement("SUPPLEMENTARYDUTYHEADDETAILS.LIST");
                        XmlElement EWAYBILLDETAILS = xmldoc.CreateElement("EWAYBILLDETAILS.LIST");
                        XmlElement INVOICEDELNOTES = xmldoc.CreateElement("INVOICEDELNOTES.LIST");
                        XmlElement INVOICEORDERLIST = xmldoc.CreateElement("INVOICEORDERLIST.LIST");
                        XmlElement INVOICEINDENTLIST = xmldoc.CreateElement("INVOICEINDENTLIST.LIST");
                        XmlElement ATTENDANCEENTRIES = xmldoc.CreateElement("ATTENDANCEENTRIES.LIST");
                        XmlElement ORIGINVOICEDETAILS = xmldoc.CreateElement("ORIGINVOICEDETAILS.LIST");
                        XmlElement INVOICEEXPORTLIST = xmldoc.CreateElement("INVOICEEXPORTLIST.LIST");

                        ALTERID.InnerText = "";
                        VOUCHER.AppendChild(ALTERID);

                        MASTERID.InnerText = "";
                        VOUCHER.AppendChild(MASTERID);

                        VOUCHERKEY.InnerText = "";
                        VOUCHER.AppendChild(VOUCHERKEY);

                        VOUCHER.AppendChild(EXCLUDEDTAXATIONS);
                        VOUCHER.AppendChild(OLDAUDITENTRIES);
                        VOUCHER.AppendChild(ACCOUNTAUDITENTRIES);
                        VOUCHER.AppendChild(AUDITENTRIES);
                        VOUCHER.AppendChild(DUTYHEADDETAILS);
                        VOUCHER.AppendChild(SUPPLEMENTARYDUTYHEADDETAILS);
                        VOUCHER.AppendChild(EWAYBILLDETAILS);
                        VOUCHER.AppendChild(INVOICEDELNOTES);
                        VOUCHER.AppendChild(INVOICEORDERLIST);
                        VOUCHER.AppendChild(INVOICEINDENTLIST);
                        VOUCHER.AppendChild(ATTENDANCEENTRIES);
                        VOUCHER.AppendChild(ORIGINVOICEDETAILS);
                        VOUCHER.AppendChild(INVOICEEXPORTLIST);

                        XmlElement ALLLEDGERENTRIESmain = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDS = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDS.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSm = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSm.InnerText = "-1";
                        OLDAUDITENTRYIDS.AppendChild(OLDAUDITENTRYIDSm);
                        ALLLEDGERENTRIESmain.AppendChild(OLDAUDITENTRYIDS);

                        XmlElement GSTCLASSk = xmldoc.CreateElement("GSTCLASS");
                        XmlElement LEDGERNAME = xmldoc.CreateElement("LEDGERNAME");
                        LEDGERNAME.InnerText = lblVendor.Text.Replace("&", "AND");
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERNAME);
                        ALLLEDGERENTRIESmain.AppendChild(GSTCLASSk);

                        XmlElement ISDEEMEDPOSITIVE = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISDEEMEDPOSITIVE);

                        XmlElement LEDGERFROMITEM = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEM.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(LEDGERFROMITEM);

                        XmlElement REMOVEZEROENTRIES = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIES.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(REMOVEZEROENTRIES);

                        XmlElement ISPARTYLEDGER = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGER.InnerText = "Yes";
                        ALLLEDGERENTRIESmain.AppendChild(ISPARTYLEDGER);

                        XmlElement ISLASTDEEMEDPOSITIVE = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVE.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISLASTDEEMEDPOSITIVE);


                        XmlElement ISCAPVATTAXALTERED = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                        ISCAPVATTAXALTERED.InnerText = "No";
                        ALLLEDGERENTRIESmain.AppendChild(ISCAPVATTAXALTERED);


                        XmlElement BANKALLOCATIONST = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        XmlElement AMOUNT = xmldoc.CreateElement("AMOUNT");
                        AMOUNT.InnerText = lblTotalNetPayble.Text;
                        ALLLEDGERENTRIESmain.AppendChild(AMOUNT);

                        XmlElement VATEXPAMOUNT = xmldoc.CreateElement("VATEXPAMOUNT");
                        VATEXPAMOUNT.InnerText = lblTotalNetPayble.Text;
                        ALLLEDGERENTRIESmain.AppendChild(VATEXPAMOUNT);

                        XmlElement SERVICETAXDETAILS = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                        ALLLEDGERENTRIESmain.AppendChild(SERVICETAXDETAILS);
                        ALLLEDGERENTRIESmain.AppendChild(BANKALLOCATIONST);
                        //
                        int bno = 0;
                        var rcpt = dc.CashPaymentDetail_View(lblSupplierInvoiceNo.Text);
                        foreach (var rcptdt in rcpt)
                        {

                            XmlElement BILLALLOCATIONS = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                            XmlElement NAME = xmldoc.CreateElement("NAME");
                            XmlElement BILLTYPE = xmldoc.CreateElement("BILLTYPE");
                            //if (rcptdt.CashDetail_Settlement_var == "On A/c")
                            //{
                            //    BILLTYPE.InnerText = "On Account";
                            //    BILLALLOCATIONS.AppendChild(BILLTYPE);
                            //}
                            //else if (int.TryParse(rcptdt.CashDetail_BillNo_int, out bno) == true && Convert.ToInt32(rcptdt.CashDetail_BillNo_int) != 0)
                            //{
                            //    if (Convert.ToInt32(rcptdt.CashDetail_BillNo_int) < 0)
                            //        NAME.InnerText = "DT - " + (Convert.ToInt32(rcptdt.CashDetail_BillNo_int) * (-1));
                            //    else
                            //        NAME.InnerText = "DT - " + rcptdt.CashDetail_BillNo_int;

                            //    BILLALLOCATIONS.AppendChild(NAME);
                            //    BILLTYPE.InnerText = "Agst Ref";
                            //    BILLALLOCATIONS.AppendChild(BILLTYPE);
                            //}
                            if (rcptdt.CASHPAYDETAIL_InvoiceNo_var != "" && rcptdt.CASHPAYDETAIL_InvoiceNo_var != "0")
                            {
                                NAME.InnerText = rcptdt.CASHPAYDETAIL_InvoiceNo_var;
                                BILLALLOCATIONS.AppendChild(NAME);

                                BILLTYPE.InnerText = "Agst Ref";
                                BILLALLOCATIONS.AppendChild(BILLTYPE);
                            }
                            //else if (rcptdt.CashDetail_NoteNo_var != "")
                            //{
                            //    NAME.InnerText = rcptdt.CashDetail_NoteNo_var;
                            //    BILLALLOCATIONS.AppendChild(NAME);

                            //    BILLTYPE.InnerText = "Agst Ref";
                            //    BILLALLOCATIONS.AppendChild(BILLTYPE);
                            //}
                            XmlElement TDSDEDUCTEEISSPECIALRATE = xmldoc.CreateElement("TDSDEDUCTEEISSPECIALRATE");
                            TDSDEDUCTEEISSPECIALRATE.InnerText = "No";
                            BILLALLOCATIONS.AppendChild(TDSDEDUCTEEISSPECIALRATE);

                            XmlElement AMT = xmldoc.CreateElement("AMOUNT");
                            AMT.InnerText = (rcptdt.CASHPAYDETAIL_CreditAmount_num * -1).ToString();
                            BILLALLOCATIONS.AppendChild(AMT);

                            XmlElement INTERESTCOLLECTIONL = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                            BILLALLOCATIONS.AppendChild(INTERESTCOLLECTIONL);
                            ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);

                            XmlElement STBILLCATEGORIES = xmldoc.CreateElement("STBILLCATEGORIES.LIST");
                            BILLALLOCATIONS.AppendChild(STBILLCATEGORIES);
                            ALLLEDGERENTRIESmain.AppendChild(BILLALLOCATIONS);
                        }

                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INTERESTCOLLECTION.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("OLDAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("AUDITENTRIES.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INPUTCRALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("DUTYHEADDETAILS.LIST"));
                        //ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ESCISEDUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("RATEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("SUMMARYALLOCS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("STPYMTDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("REFVOUCHERDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("INVOICEWISEDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("VATITCDETAILS.LIST"));
                        ALLLEDGERENTRIESmain.AppendChild(xmldoc.CreateElement("ADVANCETAXDETAILS.LIST"));
                        VOUCHER.AppendChild(ALLLEDGERENTRIESmain);

                        XmlElement ALLLEDGERENTRIESM = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        XmlElement OLDAUDITENTRYIDSz = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        OLDAUDITENTRYIDSz.SetAttribute("TYPE", "Number");
                        XmlElement OLDAUDITENTRYIDSs = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        OLDAUDITENTRYIDSs.InnerText = "-1";
                        OLDAUDITENTRYIDSz.AppendChild(OLDAUDITENTRYIDSs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRYIDSz);

                        XmlElement LEDGERNAMEe = xmldoc.CreateElement("LEDGERNAME");
                        //if (rcptd.CASHPAY_PaymentStatus_var == "Cash")
                        //{
                        //    if (cnStr.ToLower().Contains("mumbai") == true)
                        //        LEDGERNAMEe.InnerText = "Cash";
                        //    else if (cnStr.ToLower().Contains("nashik") == true)
                        //        LEDGERNAMEe.InnerText = "Cash";
                        //    else
                        //        LEDGERNAMEe.InnerText = "Petty Cash - Sinhgad Road";
                        //}
                        //else
                        //{
                            LEDGERNAMEe.InnerText = lblType.Text;
                        //}
                        ALLLEDGERENTRIESM.AppendChild(LEDGERNAMEe);
                        XmlElement GSTCLASSs = xmldoc.CreateElement("GSTCLASS");
                        ALLLEDGERENTRIESM.AppendChild(GSTCLASSs);

                        XmlElement ISDEEMEDPOSITIVEa = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ISDEEMEDPOSITIVEa.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISDEEMEDPOSITIVEa);

                        XmlElement LEDGERFROMITEMa = xmldoc.CreateElement("LEDGERFROMITEM");
                        LEDGERFROMITEMa.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(LEDGERFROMITEMa);

                        XmlElement REMOVEZEROENTRIESq = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        REMOVEZEROENTRIESq.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(REMOVEZEROENTRIESq);

                        XmlElement ISPARTYLEDGERw = xmldoc.CreateElement("ISPARTYLEDGER");
                        ISPARTYLEDGERw.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISPARTYLEDGERw);

                        XmlElement ISLASTDEEMEDPOSITIVEr = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        ISLASTDEEMEDPOSITIVEr.InnerText = "Yes";
                        ALLLEDGERENTRIESM.AppendChild(ISLASTDEEMEDPOSITIVEr);

                        XmlElement ISCAPVATTAXALTEREDEr = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                        ISCAPVATTAXALTEREDEr.InnerText = "No";
                        ALLLEDGERENTRIESM.AppendChild(ISCAPVATTAXALTEREDEr);

                        XmlElement AMOUNTav = xmldoc.CreateElement("AMOUNT");
                        //if (rcptd.Cash_TDS_num > 0)
                        //{
                        //    AMOUNTav.InnerText = "-" + (rcptd.Cash_Amount_num - rcptd.Cash_TDS_num);
                        //}
                        //else
                        //{
                            AMOUNTav.InnerText = (rcptd.CASHPAY_TotalAmount_num * -1).ToString();
                        //}
                        ALLLEDGERENTRIESM.AppendChild(AMOUNTav);

                        XmlElement AMOUNTVAT = xmldoc.CreateElement("VATEXPAMOUNT");
                        //if (rcptd.Cash_TDS_num > 0)
                        //{
                        //    AMOUNTVAT.InnerText = "-" + (rcptd.Cash_Amount_num - rcptd.Cash_TDS_num);
                        //}
                        //else
                        //{
                            AMOUNTVAT.InnerText = (rcptd.CASHPAY_TotalAmount_num * -1).ToString();
                        //}
                        ALLLEDGERENTRIESM.AppendChild(AMOUNTVAT);

                        XmlElement SERVICETAXDETAILSS = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                        ALLLEDGERENTRIESM.AppendChild(SERVICETAXDETAILSS);

                        XmlElement BANKALLOCATIONSs = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        //
                        ////if (rcptd.Cash_PaymentType_bit == true)
                        ////{
                        ////    XmlElement TEMPXmlElemt = xmldoc.CreateElement("DATE");
                        ////    date = DateTime.ParseExact(lblBillDate.Text, "dd/MM/yyyy", null);
                        ////    TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("INSTRUMENTDATE");
                        ////    date = Convert.ToDateTime(rcptd.Cash_ChqDate_date);
                        ////    TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    //TEMPXmlElemt = xmldoc.CreateElement("BANKERSDATE");
                        ////    //date = Convert.ToDateTime(rcptd.Cash_ChqDate_date);
                        ////    //TEMPXmlElemt.InnerText = date.ToString("yyyyMMdd");
                        ////    //BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("NAME");
                        ////    //TEMPXmlElemt.InnerText = rcptd.Cash_Branch_var;
                        ////    //TEMPXmlElemt.InnerText = rcptd.Cash_BankName_var;
                        ////    if (DateTime.Now.Month <= 3)
                        ////        NarrationStr = (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
                        ////    else
                        ////        NarrationStr = (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();

                        ////    NarrationStr = lblBillNo.Text.Replace("-", "") + "/" + NarrationStr;
                        ////    TEMPXmlElemt.InnerText = NarrationStr;
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("TRANSACTIONTYPE");
                        ////    TEMPXmlElemt.InnerText = "Cheque/DD";
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("BANKNAME");
                        ////    TEMPXmlElemt.InnerText = rcptd.Cash_BankName_var;
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("BANKBRANCHNAME");
                        ////    TEMPXmlElemt.InnerText = rcptd.Cash_Branch_var;
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("PAYMENTFAVOURING");
                        ////    TEMPXmlElemt.InnerText = lblClientName.Text;
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("BANKID");
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("INSTRUMENTNUMBER");
                        ////    TEMPXmlElemt.InnerText = Convert.ToInt32(rcptd.Cash_ChequeNo_int).ToString("000000");
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("UNIQUEREFERENCENUMBER");
                        ////    TEMPXmlElemt.InnerText = "";
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("STATUS");
                        ////    TEMPXmlElemt.InnerText = "No";
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("PAYMENTMODE");
                        ////    TEMPXmlElemt.InnerText = "Transacted";
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    //TEMPXmlElemt = xmldoc.CreateElement("SECONDARYSTATUS");
                        ////    //BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("BANKPARTYNAME");
                        ////    TEMPXmlElemt.InnerText = lblClientName.Text.Replace("&", "AND"); ;
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("ISCONNECTEDPAYMENT");
                        ////    TEMPXmlElemt.InnerText = "No";
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("ISSPLIT");
                        ////    TEMPXmlElemt.InnerText = "No";
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("ISCONTRACTUSED");
                        ////    TEMPXmlElemt.InnerText = "No";
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("ISACCEPTEDWITHWARNING");
                        ////    TEMPXmlElemt.InnerText = "No";
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("ISTRANSFORCED");
                        ////    TEMPXmlElemt.InnerText = "No";
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);


                        ////    TEMPXmlElemt = xmldoc.CreateElement("CHEQUEPRINTED");
                        ////    TEMPXmlElemt.InnerText = "1";
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("AMOUNT");
                        ////    if (rcptd.Cash_TDS_num > 0)
                        ////    {
                        ////        TEMPXmlElemt.InnerText = "-" + (rcptd.Cash_Amount_num - rcptd.Cash_TDS_num);
                        ////    }
                        ////    else
                        ////    {
                        ////        TEMPXmlElemt.InnerText = "-" + rcptd.Cash_Amount_num;
                        ////    }
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("CONTRACTDETAILS.LIST");
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);

                        ////    TEMPXmlElemt = xmldoc.CreateElement("BANKSTATUSINFO.LIST");
                        ////    BANKALLOCATIONSs.AppendChild(TEMPXmlElemt);


                        ////}
                        //
                        ALLLEDGERENTRIESM.AppendChild(BANKALLOCATIONSs);

                        XmlElement BILLALLOCATIONSs = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        XmlElement INTERESTCOLLECTIONs = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        XmlElement OLDAUDITENTRIESs = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        XmlElement ACCOUNTAUDITENTRIESSs = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        XmlElement AUDITENTRIESsw = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        XmlElement INPUTCRALLOCSS = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                        XmlElement DUTYHEADDETAILSs = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                        XmlElement EXCISEDUTYHEADDETAILSS = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                        XmlElement RATEDETAILSS = xmldoc.CreateElement("RATEDETAILS.LIST");
                        XmlElement SUMMARYALLOCSS = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                        XmlElement STPYMTDETAILSS = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                        XmlElement EXCISEPAYMENTALLOCATIONSS = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                        XmlElement TAXBILLALLOCATIONSs = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        XmlElement TAXOBJECTALLOCATIONSs = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        XmlElement TDSEXPENSEALLOCATIONSs = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        XmlElement VATSTATUTORYDETAILSSs = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        XmlElement COSTTRACKALLOCATIONSs = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                        XmlElement REFVOUCHERDETAILs = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                        XmlElement INVOICEWISEDETAILs = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                        XmlElement VATITCDETAILs = xmldoc.CreateElement("VATITCDETAILS.LIST");
                        XmlElement ADVANCETAXDETAILs = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                        ALLLEDGERENTRIESM.AppendChild(BILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(INTERESTCOLLECTIONs);
                        ALLLEDGERENTRIESM.AppendChild(OLDAUDITENTRIESs);
                        ALLLEDGERENTRIESM.AppendChild(ACCOUNTAUDITENTRIESSs);
                        ALLLEDGERENTRIESM.AppendChild(AUDITENTRIESsw);
                        ALLLEDGERENTRIESM.AppendChild(INPUTCRALLOCSS);
                        ALLLEDGERENTRIESM.AppendChild(DUTYHEADDETAILSs);
                        ALLLEDGERENTRIESM.AppendChild(EXCISEDUTYHEADDETAILSS);
                        ALLLEDGERENTRIESM.AppendChild(RATEDETAILSS);
                        ALLLEDGERENTRIESM.AppendChild(SUMMARYALLOCSS);
                        ALLLEDGERENTRIESM.AppendChild(STPYMTDETAILSS);
                        ALLLEDGERENTRIESM.AppendChild(EXCISEPAYMENTALLOCATIONSS);
                        ALLLEDGERENTRIESM.AppendChild(TAXBILLALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TAXOBJECTALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(TDSEXPENSEALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(VATSTATUTORYDETAILSSs);
                        ALLLEDGERENTRIESM.AppendChild(COSTTRACKALLOCATIONSs);
                        ALLLEDGERENTRIESM.AppendChild(REFVOUCHERDETAILs);
                        ALLLEDGERENTRIESM.AppendChild(INVOICEWISEDETAILs);
                        ALLLEDGERENTRIESM.AppendChild(VATITCDETAILs);
                        ALLLEDGERENTRIESM.AppendChild(ADVANCETAXDETAILs);
                        VOUCHER.AppendChild(ALLLEDGERENTRIESM);

                        //tds
                        //XmlElement ALLLEDGERENTRIESMt = xmldoc.CreateElement("ALLLEDGERENTRIES.LIST");
                        //XmlElement OLDAUDITENTRYIDSzt = xmldoc.CreateElement("OLDAUDITENTRYIDS.LIST");
                        //OLDAUDITENTRYIDSzt.SetAttribute("TYPE", "Number");
                        //XmlElement OLDAUDITENTRYIDSst = xmldoc.CreateElement("OLDAUDITENTRYIDS");
                        //OLDAUDITENTRYIDSst.InnerText = "-1";
                        //OLDAUDITENTRYIDSzt.AppendChild(OLDAUDITENTRYIDSst);
                        //ALLLEDGERENTRIESMt.AppendChild(OLDAUDITENTRYIDSzt);

                        //XmlElement LEDGERNAMEet = xmldoc.CreateElement("LEDGERNAME");
                        //DateTime tempDate = Convert.ToDateTime(rcptd.CASHPAY_VoucherDate_dt);
                        //if (tempDate.Month > 3)
                        //    LEDGERNAMEet.InnerText = "TDS " + (tempDate.Year) + "-" + (tempDate.Year + 1);
                        //else
                        //    LEDGERNAMEet.InnerText = "TDS " + (tempDate.Year - 1) + "-" + (tempDate.Year);

                        //ALLLEDGERENTRIESMt.AppendChild(LEDGERNAMEet);
                        //XmlElement GSTCLASSst = xmldoc.CreateElement("GSTCLASS");
                        //ALLLEDGERENTRIESMt.AppendChild(GSTCLASSst);

                        ////XmlElement ISDEEMEDPOSITIVEat = xmldoc.CreateElement("ISDEEMEDPOSITIVE");
                        ////ISDEEMEDPOSITIVEat.InnerText = "Yes";
                        ////ALLLEDGERENTRIESMt.AppendChild(ISDEEMEDPOSITIVEat);

                        //XmlElement LEDGERFROMITEMat = xmldoc.CreateElement("LEDGERFROMITEM");
                        //LEDGERFROMITEMat.InnerText = "No";
                        //ALLLEDGERENTRIESMt.AppendChild(LEDGERFROMITEMat);

                        //XmlElement REMOVEZEROENTRIESqt = xmldoc.CreateElement("REMOVEZEROENTRIES");
                        //REMOVEZEROENTRIESqt.InnerText = "No";
                        //ALLLEDGERENTRIESMt.AppendChild(REMOVEZEROENTRIESqt);

                        //XmlElement ISPARTYLEDGERwt = xmldoc.CreateElement("ISPARTYLEDGER");
                        //ISPARTYLEDGERwt.InnerText = "Yes";
                        //ALLLEDGERENTRIESMt.AppendChild(ISPARTYLEDGERwt);

                        //XmlElement ISLASTDEEMEDPOSITIVErt = xmldoc.CreateElement("ISLASTDEEMEDPOSITIVE");
                        //ISLASTDEEMEDPOSITIVErt.InnerText = "Yes";
                        //ALLLEDGERENTRIESMt.AppendChild(ISLASTDEEMEDPOSITIVErt);

                        //XmlElement ISCAPVATTAXALTEREd = xmldoc.CreateElement("ISCAPVATTAXALTERED");
                        //ISCAPVATTAXALTEREd.InnerText = "No";
                        //ALLLEDGERENTRIESMt.AppendChild(ISCAPVATTAXALTEREd);

                        //XmlElement AMOUNTavt = xmldoc.CreateElement("AMOUNT");
                        //AMOUNTavt.InnerText = "-" + rcptd.Cash_TDS_num;
                        //ALLLEDGERENTRIESMt.AppendChild(AMOUNTavt);

                        //XmlElement VATEXPAYMENT = xmldoc.CreateElement("VATEXPAYMENT");
                        //VATEXPAYMENT.InnerText = "-" + rcptd.Cash_TDS_num;
                        //ALLLEDGERENTRIESMt.AppendChild(VATEXPAYMENT);

                        //XmlElement SERVICETAXDETAILs = xmldoc.CreateElement("SERVICETAXDETAILS.LIST");
                        //ALLLEDGERENTRIESMt.AppendChild(SERVICETAXDETAILs);

                        //XmlElement BANKALLOCATIONSst = xmldoc.CreateElement("BANKALLOCATIONS.LIST");
                        //ALLLEDGERENTRIESMt.AppendChild(BANKALLOCATIONSst);

                        //XmlElement BILLALLOCATIONSst = xmldoc.CreateElement("BILLALLOCATIONS.LIST");
                        //XmlElement INTERESTCOLLECTIONst = xmldoc.CreateElement("INTERESTCOLLECTION.LIST");
                        //XmlElement OLDAUDITENTRIESst = xmldoc.CreateElement("OLDAUDITENTRIES.LIST");
                        //XmlElement ACCOUNTAUDITENTRIESSst = xmldoc.CreateElement("ACCOUNTAUDITENTRIES.LIST");
                        //XmlElement AUDITENTRIESswt = xmldoc.CreateElement("AUDITENTRIES.LIST");
                        //XmlElement INPUTCRALLOCS = xmldoc.CreateElement("INPUTCRALLOCS.LIST");
                        //XmlElement DUTYHEADDETAILs = xmldoc.CreateElement("DUTYHEADDETAILS.LIST");
                        //XmlElement EXCISEDUTYHEADDETAILS = xmldoc.CreateElement("EXCISEDUTYHEADDETAILS.LIST");
                        //XmlElement RATEDETAILS = xmldoc.CreateElement("RATEDETAILS.LIST");
                        //XmlElement SUMMARYALLOCS = xmldoc.CreateElement("SUMMARYALLOCS.LIST");
                        //XmlElement STPYMTDETAILS = xmldoc.CreateElement("STPYMTDETAILS.LIST");
                        //XmlElement EXCISEPAYMENTALLOCATIONS = xmldoc.CreateElement("EXCISEPAYMENTALLOCATIONS.LIST");
                        //XmlElement TAXBILLALLOCATIONSst = xmldoc.CreateElement("TAXBILLALLOCATIONS.LIST");
                        //XmlElement TAXOBJECTALLOCATIONSst = xmldoc.CreateElement("TAXOBJECTALLOCATIONS.LIST");
                        //XmlElement TDSEXPENSEALLOCATIONSst = xmldoc.CreateElement("TDSEXPENSEALLOCATIONS.LIST");
                        //XmlElement VATSTATUTORYDETAILSSst = xmldoc.CreateElement("VATSTATUTORYDETAILS.LIST");
                        //XmlElement COSTTRACKALLOCATIONSst = xmldoc.CreateElement("COSTTRACKALLOCATIONS.LIST");
                        //XmlElement REFVOUCHERDETAILS = xmldoc.CreateElement("REFVOUCHERDETAILS.LIST");
                        //XmlElement INVOICEWISEDETAILS = xmldoc.CreateElement("INVOICEWISEDETAILS.LIST");
                        //XmlElement VATITCDETAILS = xmldoc.CreateElement("VATITCDETAILS.LIST");
                        //XmlElement ADVANCETAXDETAILS = xmldoc.CreateElement("ADVANCETAXDETAILS.LIST");
                        //ALLLEDGERENTRIESMt.AppendChild(BILLALLOCATIONSst);
                        //ALLLEDGERENTRIESMt.AppendChild(INTERESTCOLLECTIONst);
                        //ALLLEDGERENTRIESMt.AppendChild(OLDAUDITENTRIESst);
                        //ALLLEDGERENTRIESMt.AppendChild(ACCOUNTAUDITENTRIESSst);
                        //ALLLEDGERENTRIESMt.AppendChild(AUDITENTRIESswt);
                        //ALLLEDGERENTRIESMt.AppendChild(INPUTCRALLOCS);
                        //ALLLEDGERENTRIESMt.AppendChild(DUTYHEADDETAILs);
                        //ALLLEDGERENTRIESMt.AppendChild(EXCISEDUTYHEADDETAILS);
                        //ALLLEDGERENTRIESMt.AppendChild(RATEDETAILS);
                        //ALLLEDGERENTRIESMt.AppendChild(SUMMARYALLOCS);
                        //ALLLEDGERENTRIESMt.AppendChild(STPYMTDETAILS);
                        //ALLLEDGERENTRIESMt.AppendChild(EXCISEPAYMENTALLOCATIONS);
                        //ALLLEDGERENTRIESMt.AppendChild(TAXBILLALLOCATIONSst);
                        //ALLLEDGERENTRIESMt.AppendChild(TAXOBJECTALLOCATIONSst);
                        //ALLLEDGERENTRIESMt.AppendChild(TDSEXPENSEALLOCATIONSst);
                        //ALLLEDGERENTRIESMt.AppendChild(VATSTATUTORYDETAILSSst);
                        //ALLLEDGERENTRIESMt.AppendChild(COSTTRACKALLOCATIONSst);
                        //ALLLEDGERENTRIESMt.AppendChild(REFVOUCHERDETAILS);
                        //ALLLEDGERENTRIESMt.AppendChild(INVOICEWISEDETAILS);
                        //ALLLEDGERENTRIESMt.AppendChild(VATITCDETAILS);
                        //ALLLEDGERENTRIESMt.AppendChild(ADVANCETAXDETAILS);
                        //VOUCHER.AppendChild(ALLLEDGERENTRIESMt);
                        //

                        XmlElement PAYROLLMODEOFPAYMENT = xmldoc.CreateElement("PAYROLLMODEOFPAYMENT.LIST");
                        VOUCHER.AppendChild(PAYROLLMODEOFPAYMENT);
                        XmlElement ATTDRECORDS = xmldoc.CreateElement("ATTDRECORDS.LIST");
                        VOUCHER.AppendChild(ATTDRECORDS);
                        XmlElement GSTEWAYCONSIGNORADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNORADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNORADDRESS);
                        XmlElement GSTEWAYCONSIGNEEADDRESS = xmldoc.CreateElement("GSTEWAYCONSIGNEEADDRESS.LIST");
                        VOUCHER.AppendChild(GSTEWAYCONSIGNEEADDRESS);
                        XmlElement TEMPGSTRATEDETAILS = xmldoc.CreateElement("TEMPGSTRATEDETAILS.LIST");
                        VOUCHER.AppendChild(TEMPGSTRATEDETAILS);
                        TALLYMESSAGE.AppendChild(VOUCHER);
                        REQUESTDATA.AppendChild(TALLYMESSAGE);
                        //dc.CashPayment_Update_TallyStatus(Convert.ToInt32(lblBookingNo.Text), true);
                    }

                }
            }
            #endregion
            RootNode.AppendChild(HEADER);
            IMPORTDATA.AppendChild(REQUESTDESC);
            IMPORTDATA.AppendChild(REQUESTDATA);
            BODY.AppendChild(IMPORTDATA);
            RootNode.AppendChild(BODY);

            if (!Directory.Exists(@"d:\xml"))
                Directory.CreateDirectory(@"d:\xml");
            string fileName = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                fileName = "VendorCashPayment_Mumbai";
            else if (cnStr.ToLower().Contains("nashik") == true)
                fileName = "VendorCashPayment_Nashik";
            else if (cnStr.ToLower().Contains("metro") == true)
                fileName = "VendorCashPayment_Metro";
            else
                fileName = "VendorCashPayment_Pune";
            xmldoc.Save(@"d:\xml\" + fileName + ".xml");
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xml");
            response.TransmitFile("d:\\xml\\" + fileName + ".xml");
            response.Flush();
            response.End();
        }
        
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void optJournal_CheckedChanged(object sender, EventArgs e)
        {
            grdBillBooking.DataSource = null;
            grdBillBooking.DataBind();
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)grdBillBooking.HeaderRow.FindControl("chkSelectAll");
            foreach (GridViewRow row in grdBillBooking.Rows)
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

        protected void optCashPayment_CheckedChanged(object sender, EventArgs e)
        {
            grdBillBooking.DataSource = null;
            grdBillBooking.DataBind();
        }
        protected void optBankPayment_CheckedChanged(object sender, EventArgs e)
        {
            grdBillBooking.DataSource = null;
            grdBillBooking.DataBind();
        }
    }
}