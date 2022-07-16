using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class BillWiseReportStatus : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Bill Wise Report Status";
                txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }

        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr1 = null;
            DataTable dt2 = new DataTable();
            DataRow dr2 = null;

            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt.Columns.Add(new DataColumn("SiteName", typeof(string)));
            dt.Columns.Add(new DataColumn("CRLimit", typeof(string)));
            dt.Columns.Add(new DataColumn("BalanceAmt", typeof(string)));
            dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
            dt.Columns.Add(new DataColumn("BillDate", typeof(string)));
            dt.Columns.Add(new DataColumn("BillAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("BillModifiedDate", typeof(string)));
            dt.Columns.Add(new DataColumn("BillRecordType", typeof(string)));
            dt.Columns.Add(new DataColumn("BillRecordNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ReferenceNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ApprovedDate", typeof(string)));
            dt.Columns.Add(new DataColumn("ReportStatus", typeof(string)));

            dt2.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt2.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt2.Columns.Add(new DataColumn("SiteName", typeof(string)));
            dt2.Columns.Add(new DataColumn("CRLimit", typeof(string)));
            dt2.Columns.Add(new DataColumn("BalanceAmt", typeof(string)));
            dt2.Columns.Add(new DataColumn("BillNo", typeof(string)));
            dt2.Columns.Add(new DataColumn("BillDate", typeof(string)));
            dt2.Columns.Add(new DataColumn("BillAmount", typeof(string)));
            dt2.Columns.Add(new DataColumn("BillModifiedDate", typeof(string)));
            dt2.Columns.Add(new DataColumn("BillRecordType", typeof(string)));
            dt2.Columns.Add(new DataColumn("BillRecordNo", typeof(string)));
            dt2.Columns.Add(new DataColumn("ReferenceNo", typeof(string)));
            dt2.Columns.Add(new DataColumn("ApprovedDate", typeof(string)));
            dt2.Columns.Add(new DataColumn("ReportStatus", typeof(string)));

            decimal billTotal = 0;
            int i = 0;
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            var billList = dc.Bill_View_Status(0, "", 0, false, Fromdate, Todate, false, 2, 2);
            foreach(var bl in billList)
            {
                i++;
                dr1 = dt.NewRow();
                dr1["SrNo"] = i;
                dr1["ClientName"] = bl.CL_Name_var;
                dr1["SiteName"] = bl.SITE_Name_var;
                //dr1["CRLimit"] = Convert.ToDecimal(bl.CL_Limit_mny).ToString("0.00");
                //dr1["BalanceAmt"] = Convert.ToDecimal(bl.CL_BalanceAmt_mny).ToString("0.00");
                dr1["BillNo"] = bl.BILL_Id;
                dr1["BillDate"] = Convert.ToDateTime(bl.BILL_Date_dt).ToString("dd/MM/yyyy");
                dr1["BillAmount"] = bl.BILL_NetAmt_num;
                dr1["BillModifiedDate"] = Convert.ToDateTime(bl.BILL_ModifiedOn_dt).ToString("dd/MM/yyyy");
                dr1["BillRecordType"] = bl.BILL_RecordType_var;
                dr1["BillRecordNo"] = bl.BILL_RecordNo_int;

                billTotal += Convert.ToDecimal(bl.BILL_NetAmt_num);
                if (bl.BILL_RecordType_var == "Monthly")
                {
                    int billdCnt = 0;
                    var billdetail = dc.BillDetail_View(bl.BILL_Id).ToList();
                    foreach (var bld in billdetail)
                    {
                        billdCnt++;
                        int misCnt = 0;
                        var misdetail = dc.MISDetail_View(0, bld.BILLD_ReferenceNo_int, "").ToList();
                        foreach (var mis in misdetail)
                        {
                            misCnt++;
                            //common 
                            dr1["SrNo"] = i;
                            dr1["ClientName"] = bl.CL_Name_var;
                            dr1["SiteName"] = bl.SITE_Name_var;
                            //dr1["CRLimit"] = Convert.ToDecimal(bl.CL_Limit_mny).ToString("0.00");
                            //dr1["BalanceAmt"] = Convert.ToDecimal(bl.CL_BalanceAmt_mny).ToString("0.00");
                            dr1["BillNo"] = bl.BILL_Id;
                            dr1["BillDate"] = Convert.ToDateTime(bl.BILL_Date_dt).ToString("dd/MM/yyyy");
                            dr1["BillAmount"] = bl.BILL_NetAmt_num;
                            dr1["BillModifiedDate"] = Convert.ToDateTime(bl.BILL_ModifiedOn_dt).ToString("dd/MM/yyyy");
                            dr1["BillRecordType"] = bl.BILL_RecordType_var;
                            dr1["BillRecordNo"] = bl.BILL_RecordNo_int;
                            //
                            dr1["BillRecordType"] = mis.MISRecType;
                            dr1["BillRecordNo"] = mis.MISRecordNo;
                            dr1["ReferenceNo"] = mis.MISRefNo;
                            if (mis.MISApprovedDt != null)
                                dr1["ApprovedDate"] = Convert.ToDateTime(mis.MISApprovedDt).ToString("dd/MM/yyyy");

                            if (mis.MISPhysicalOutwardDt != null)
                                dr1["ReportStatus"] = "Physical Outward Complete";
                            else if (mis.MISOutwardDt != null)
                                dr1["ReportStatus"] = "Outward Complete";
                            else if (mis.MISPrintedDt != null)
                                dr1["ReportStatus"] = "Printed";
                            else if (mis.MISApprovedDt != null)
                                dr1["ReportStatus"] = "Approved";
                            else if (mis.MISCheckedDt != null)
                                dr1["ReportStatus"] = "Checked";
                            else if (mis.MISEnteredDt != null)
                                dr1["ReportStatus"] = "Entered";
                            else
                                dr1["ReportStatus"] = "To be Entered";

                            if (misCnt < misdetail.Count)
                            {
                                dt.Rows.Add(dr1);
                                dr1 = dt.NewRow();
                            }
                        }
                        if (billdCnt < billdetail.Count)
                        {
                            dt.Rows.Add(dr1);
                            dr1 = dt.NewRow();
                        }
                    }
                }
                else
                {
                    int misCnt = 0;
                    var misdetail = dc.MISDetail_View(bl.BILL_RecordNo_int, 0, bl.BILL_RecordType_var).ToList();
                    foreach (var mis in misdetail)
                    {
                        misCnt++;
                        //common 
                        dr1["SrNo"] = i;
                        dr1["ClientName"] = bl.CL_Name_var;
                        dr1["SiteName"] = bl.SITE_Name_var;
                        //dr1["CRLimit"] = Convert.ToDecimal(bl.CL_Limit_mny).ToString("0.00");
                        //dr1["BalanceAmt"] = Convert.ToDecimal(bl.CL_BalanceAmt_mny).ToString("0.00");
                        dr1["BillNo"] = bl.BILL_Id;
                        dr1["BillDate"] = Convert.ToDateTime(bl.BILL_Date_dt).ToString("dd/MM/yyyy");
                        dr1["BillAmount"] = bl.BILL_NetAmt_num;
                        dr1["BillModifiedDate"] = Convert.ToDateTime(bl.BILL_ModifiedOn_dt).ToString("dd/MM/yyyy");
                        dr1["BillRecordType"] = bl.BILL_RecordType_var;
                        dr1["BillRecordNo"] = bl.BILL_RecordNo_int;
                        //
                        dr1["ReferenceNo"] = mis.MISRefNo;
                        if (mis.MISApprovedDt != null)
                            dr1["ApprovedDate"] = Convert.ToDateTime(mis.MISApprovedDt).ToString("dd/MM/yyyy");
                        if (mis.MISPhysicalOutwardDt != null)
                            dr1["ReportStatus"] = "Physical Outward Complete";
                        else if (mis.MISOutwardDt != null)
                            dr1["ReportStatus"] = "Outward Complete";
                        else if (mis.MISPrintedDt != null)
                            dr1["ReportStatus"] = "Printed";
                        else if (mis.MISApprovedDt != null)
                            dr1["ReportStatus"] = "Approved";
                        else if (mis.MISCheckedDt != null)
                            dr1["ReportStatus"] = "Checked";
                        else if (mis.MISEnteredDt != null)
                            dr1["ReportStatus"] = "Entered";
                        else
                            dr1["ReportStatus"] = "To be Entered";

                        if (misCnt < misdetail.Count)
                        {
                            dt.Rows.Add(dr1);
                            dr1 = dt.NewRow();
                        }

                    }
                }
                dt.Rows.Add(dr1);
            }
            dr1 = dt.NewRow();
            dr1["BillDate"] = "Total = ";
            dr1["BillAmount"] = billTotal.ToString("0.00");
            dt.Rows.Add(dr1);

            string prevBillNo = "";
            int srNo = 1;
            for (int j = 0; j < dt.Rows.Count ; j++)
            {
                if (chkCRLimitApprPending.Checked == false ||
                    (chkCRLimitApprPending.Checked == true && dt.Rows[j]["ReportStatus"].ToString() != "Physical Outward Complete"
                    && dt.Rows[j]["ReportStatus"].ToString() != "Outward Complete" && dt.Rows[j]["ReportStatus"].ToString() != "Printed" && dt.Rows[j]["ReportStatus"].ToString() != "")
                    && dt.Rows[j]["BalanceAmt"].ToString() != "" && Convert.ToDecimal(dt.Rows[j]["BalanceAmt"].ToString()) > Convert.ToDecimal(dt.Rows[j]["CRLimit"].ToString()))
                {
                    dr2 = dt2.NewRow();
                    if (prevBillNo != dt.Rows[j]["BillNo"].ToString())
                    {
                        dr2["SrNo"] = srNo;
                        dr2["ClientName"] = dt.Rows[j]["ClientName"];
                        dr2["SiteName"] = dt.Rows[j]["SiteName"];
                        dr2["CRLimit"] = dt.Rows[j]["CRLimit"];
                        dr2["BalanceAmt"] = dt.Rows[j]["BalanceAmt"];
                        dr2["BillNo"] = dt.Rows[j]["BillNo"];
                        dr2["BillDate"] = dt.Rows[j]["BillDate"];
                        dr2["BillAmount"] = dt.Rows[j]["BillAmount"];
                        dr2["BillModifiedDate"] = dt.Rows[j]["BillModifiedDate"];
                        srNo++;
                    }
                    dr2["BillRecordType"] = dt.Rows[j]["BillRecordType"];
                    dr2["BillRecordNo"] = dt.Rows[j]["BillRecordNo"];
                    dr2["ReferenceNo"] = dt.Rows[j]["ReferenceNo"];
                    dr2["ApprovedDate"] = dt.Rows[j]["ApprovedDate"];
                    dr2["ReportStatus"] = dt.Rows[j]["ReportStatus"];
                    prevBillNo = dt.Rows[j]["BillNo"].ToString();
                    dt2.Rows.Add(dr2);
                }
            }
            grdBillList.DataSource = dt2;
            grdBillList.DataBind();
            lblTotal.Text = "Total Bill Amount : " + billTotal.ToString("0.00");
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdBillList.Rows.Count > 0 && grdBillList.Visible == true)
            {
                string Subheader = "";

                Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text + "|" + lblToDate.Text + "|" + txtToDate.Text;
                Subheader += "|" + "" + "|" + "";

                PrintGrid.PrintGridView(grdBillList, Subheader, "BillWiseReportStatus_Report");
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

        protected void lnkDisplayCRLimitPendingRpt_Click(object sender, EventArgs e)
        {

        }
    }
}