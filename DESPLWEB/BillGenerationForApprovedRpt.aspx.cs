using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class BillGenerationForApprovedRpt : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Generate Bill for Approved Reports";
                
                
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

        public void LoadPendingReportList()
        {
            DateTime FromDate = new DateTime();
            if (DateTime.Now.Month == 1)
                FromDate = new DateTime(DateTime.Now.Year - 1, 12, 26);
            else
                FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 26);

            var pendingCl = dc.ApprovedReportListForBillGeneration_View(FromDate);
            grdReport.DataSource = pendingCl;
            grdReport.DataBind();
            lblTotalRecords.Text = "Total Records : " + grdReport.Rows.Count;
            if (grdReport.Rows.Count == 0)
            {
                string strMsg = "Pending report not available for bill generation..";
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('" + strMsg + " ');", true);
            }
            
        }
        
        protected void lnkLoadPendingRptList_Click(object sender, EventArgs e)
        {
            LoadPendingReportList();
        }

        protected void grdReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strReportDetails = Convert.ToString(e.CommandArgument);
            string[] arg = new string[3];
            arg = strReportDetails.Split(';');

            string Recordtype = Convert.ToString(arg[0]);
            int RecordNo = Convert.ToInt32(arg[1]);
            string ReferenceNo = Convert.ToString(arg[2]);

            if (e.CommandName == "GenerateBill")
            {
                #region Bill Generation
                bool generateBillFlag = true;
                string BillNo = "0";
                
                var inward = dc.Inward_View(0, RecordNo, Recordtype, null, null).ToList();
                foreach (var inwd in inward)
                {
                    if (inwd.INWD_BILL_Id != null && inwd.INWD_BILL_Id != "0")
                    {
                        BillNo = inwd.INWD_BILL_Id;
                        generateBillFlag = false;
                    }
                   if (inwd.SITE_MonthlyBillingStatus_bit != null && inwd.SITE_MonthlyBillingStatus_bit == true)
                    {
                        generateBillFlag = false;
                    }
                    if (inwd.INWD_MonthlyBill_bit == true)
                    {
                        generateBillFlag = false;
                    }
                }
                
                if (generateBillFlag == true && Recordtype == "CT")
                {
                    var ctinwd = dc.AllInwdDetails_View(ReferenceNo, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, RecordNo, "", 0, Recordtype);
                    foreach (var ct in ctinwd)
                    {
                        if (ct.CTINWD_CouponNo_var != null && ct.CTINWD_CouponNo_var != "")
                        {
                            generateBillFlag = false;
                            break;
                        }
                    }
                }
                if (generateBillFlag == true)
                {
                    var withoutbill = dc.WithoutBill_View(RecordNo, Recordtype);
                    if (withoutbill.Count() > 0)
                    {
                        generateBillFlag = false;
                    }
                }
                if (generateBillFlag == true)
                {
                    int NewrecNo = 0;
                    clsData clsObj = new clsData();
                    NewrecNo = clsObj.GetCurrentRecordNo("BillNo");
                    //var gstbillCount = dc.Bill_View("0", 0, 0, "", 0, false, false, DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                    var gstbillCount = dc.Bill_View_Count(DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                    if (gstbillCount.Count() != NewrecNo - 1)
                    {
                        generateBillFlag = false;
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Bill No. mismatch. Can not generate bill.');", true);
                    }
                }
                //Generate bill
                if (generateBillFlag == true)
                {
                    BillUpdation bill = new BillUpdation();
                    BillNo = bill.UpdateBill(Recordtype, RecordNo, BillNo);
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Bill No. " + BillNo + " generated successfully.');", true);
                    LoadPendingReportList();
                }
                //
                #endregion
            }
        }
        
    }
}