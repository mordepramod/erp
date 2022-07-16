using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.IO;

namespace DESPLWEB
{
    public partial class RWH_Inward : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                string strReq = "";
                strReq = Request.RawUrl;
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);
                if (!strReq.Equals(""))
                {
                    strReq = obj.Decrypt(strReq);
                    if (strReq.Contains("=") == false)
                    {
                        //Session.Abandon();
                        //Response.Redirect("Login.aspx");
                    }
                    else
                    {
                        string[] arrMsgs = strReq.Split('&');
                        string[] arrIndMsg;

                        arrIndMsg = arrMsgs[0].Split('=');
                        UC_InwardHeader1.RecType = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[1].Split('=');
                        UC_InwardHeader1.RecordNo = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[2].Split('=');
                        UC_InwardHeader1.ReferenceNo = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[3].Split('=');
                        UC_InwardHeader1.EnquiryNo = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[4].Split('=');
                        UC_InwardHeader1.InwdStatus = arrIndMsg[1].ToString().Trim();
                    }
                }


                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (lblheading != null)
                {
                    lblheading.Text = "Rain Water Harvesting Inward";
                }
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    AddRowSoilInvestigationInward();
                }
                if (UC_InwardHeader1.RecType != "")
                {
                    DisplayRainWaterHarvestingTest();
                }

                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowSoilInvestigationInward();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "RWH";
                }
                lnkSave.Visible = true;

            }

        }
        protected void AddRowSoilInvestigationInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            if (ViewState["SoilInvestigationInwardTable"] != null)
            {
                GetCurrentDataSoilInvestigationInward();
                dt = (DataTable)ViewState["SoilInvestigationInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("txtUnit", typeof(string)));
                dt.Columns.Add(new DataColumn("txtQty", typeof(string)));
                dt.Columns.Add(new DataColumn("txtRate", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtDescription"] = string.Empty;
            dr["txtUnit"] = string.Empty;
            dr["txtQty"] = string.Empty;
            dr["txtRate"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["SoilInvestigationInwardTable"] = dt;
            grdRainWaterHarvestingInward.DataSource = dt;
            grdRainWaterHarvestingInward.DataBind();
            SetPreviousSoilInvestigatinInward();

        }
        protected void DeleteRowSoilInvestigationInward(int rowIndex)
        {
            GetCurrentDataSoilInvestigationInward();
            DataTable dt = ViewState["SoilInvestigationInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["SoilInvestigationInwardTable"] = dt;
            grdRainWaterHarvestingInward.DataSource = dt;
            grdRainWaterHarvestingInward.DataBind();
            SetPreviousSoilInvestigatinInward();
        }
        protected void SetPreviousSoilInvestigatinInward()
        {
            DataTable dt = (DataTable)ViewState["SoilInvestigationInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtDescription = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[1].FindControl("txtDescription");
                TextBox txtUnit = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[2].FindControl("txtUnit");
                TextBox txtQty = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[3].FindControl("txtQty");
                TextBox txtRate = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[4].FindControl("txtRate");

                grdRainWaterHarvestingInward.Rows[i].Cells[2].Text = (i + 1).ToString();
                txtDescription.Text = dt.Rows[i]["txtDescription"].ToString();
                txtUnit.Text = dt.Rows[i]["txtUnit"].ToString();
                txtQty.Text = dt.Rows[i]["txtQty"].ToString();
                txtRate.Text = dt.Rows[i]["txtRate"].ToString();
            }
        }
        protected void GetCurrentDataSoilInvestigationInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtUnit", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtQty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtRate", typeof(string)));

            for (int i = 0; i < grdRainWaterHarvestingInward.Rows.Count; i++)
            {
                TextBox txtDescription = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[1].FindControl("txtDescription");
                TextBox txtUnit = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[2].FindControl("txtUnit");
                TextBox txtQty = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[3].FindControl("txtQty");
                TextBox txtRate = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[4].FindControl("txtRate");

                drRow = dtTable.NewRow();
                drRow["txtDescription"] = txtDescription.Text;
                drRow["txtUnit"] = txtUnit.Text;
                drRow["txtQty"] = txtQty.Text;
                drRow["txtRate"] = txtRate.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["SoilInvestigationInwardTable"] = dtTable;

        }
        protected void TextChanged(object sender, EventArgs e)
        {
            TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
            if (txtSubsets.Text != "")
            {
                if (Convert.ToInt32(txtSubsets.Text) < grdRainWaterHarvestingInward.Rows.Count)
                {
                    for (int i = grdRainWaterHarvestingInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdRainWaterHarvestingInward.Rows.Count > 1)
                        {
                            DeleteRowSoilInvestigationInward(i - 1);
                            ShowMerge();
                            ChkLumShup();
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdRainWaterHarvestingInward.Rows.Count)
                {
                    for (int i = grdRainWaterHarvestingInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
                    {
                        AddRowSoilInvestigationInward();
                        ShowMerge();
                        ChkLumShup();
                    }
                }
            }

        }
        protected void Click(object sender, EventArgs e)
        {
            ViewState["SoilInvestigationInwardTable"] = null;
            AddRowSoilInvestigationInward();
            lnkSave.Visible = true;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Text = "";
            lblMsg.Visible = false;
            lnkLabSheet.Visible = false;
            lnkPrint.Visible = false;
            lnkBillPrint.Visible = false;
            lblRptClientId.Text = "0";
            lblRptSiteId.Text = "0";
            lnkTemp_Click(sender, e);
        }

        protected void lnkTemp_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = true;
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                //get report client id, site
                var enqcl = dc.EnquiryClient_View(Convert.ToInt32(UC_InwardHeader1.EnquiryNo));
                foreach (var ec in enqcl)
                {
                    lblRptClientId.Text = ec.ENQCL_CL_Id.ToString();
                    lblRptSiteId.Text = ec.ENQCL_SITE_Id.ToString();
                }
                //
                string RefNo, SetOfRecord;
                int SrNo;
                string totalCost = "0";
                clsData clsObj = new clsData();
                //DateTime d1 = new DateTime();
                //DateTime ReceivedDate = Convert.ToDateTime(DateTime.Now.Date.ToString());
                DateTime ReceivedDate = DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null);
                DateTime d1 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "RWH", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, true);
                    var res = dc.AllInward_View("RWH", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
                    foreach (var a1 in res)
                    {
                        dc.SoilInvestigationTest_Update(a1.GTTEST_RefNo_var.ToString(),0, "", "RWH", 0, "", 0, 0, true);
                    }
                }
                else
                {
                    Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    NewrecNo = clsObj.GetnUpdateRecordNo("RWH");
                    UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                    clsObj.insertRecordTable(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), NewrecNo, "RWH");
                    UC_InwardHeader1.ReferenceNo = clsObj.getRefNo(Convert.ToInt32(UC_InwardHeader1.EnquiryNo)).ToString();
                    dc.Inward_Update_New(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "RWH", 0, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate);

                    //Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    //NewrecNo = clsObj.GetnUpdateRecordNo("RWH");
                    //UC_InwardHeader1.RecordNo = NewrecNo.ToString(); 
                    //UC_InwardHeader1.ReferenceNo = dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "RWH", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, false).ToString();
                    ////var inward = dc.Inward_View(Convert.ToInt32(UC_InwardHeader1.ReferenceNo), 0, "");
                    ////foreach (var inwd in inward)
                    ////{
                    ////    UC_InwardHeader1.RecordNo = inwd.INWD_RecordNo_int.ToString();
                    ////}
                }

                dc.Enquiry_Update_Status(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 2);
                dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "RWH", "", null, null, false, false, false, false, false, false, true);//Delete MIS Record

                dc.GTInward_Update("", Convert.ToInt32(UC_InwardHeader1.RecordNo), "RWH", "", 0, 0, "", null, null, "", "", 0, 0, true);
                if (lblRptClientId.Text != "" && lblRptClientId.Text != "0")
                {
                    dc.Inward_Update_ReportClient(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, Convert.ToInt32(lblRptClientId.Text), Convert.ToInt32(lblRptSiteId.Text));
                }
                for (int i = 0; i <= grdRainWaterHarvestingInward.Rows.Count - 1; i++)
                {
                    TextBox txtDescription = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[1].FindControl("txtDescription");
                    TextBox txtUnit = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[2].FindControl("txtUnit");
                    TextBox txtQty = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[3].FindControl("txtQty");
                    TextBox txtRate = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[4].FindControl("txtRate");

                    SrNo = Convert.ToInt32(grdRainWaterHarvestingInward.Rows[i].Cells[2].Text);
                    RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    if (grdRainWaterHarvestingInward.Rows[i].Cells[6].Visible == false)
                    {
                        txtRate.Text = "";
                    }
                    if (txtUnit.Text == "")
                    {
                        txtUnit.Text = "0";
                    }
                    if (txtQty.Text == "")
                    {
                        txtQty.Text = "0";
                    }
                    if (txtRate.Text == "")
                    {
                        txtRate.Text = ViewState["txtRate"].ToString();
                    }
                    else
                    {
                        ViewState["txtRate"] = txtRate.Text;
                    }
                    dc.GTInward_Update(RefNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), "RWH", SetOfRecord, 0, 0, "", d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), false);
                    dc.SoilInvestigationTest_Update(RefNo,0, txtDescription.Text, "RWH", Convert.ToByte(SrNo), txtUnit.Text, Convert.ToByte(txtQty.Text), Convert.ToInt32(txtRate.Text), false);

                    dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "RWH", RefNo, "RWH", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);
                }
                UC_InwardHeader1.TestReqFormNo = "Test Request Form No : " + UC_InwardHeader1.ReferenceNo + "/" + DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null).Year.ToString().Substring(2, 2);
                //if (UC_InwardHeader1.OtherClient == true)
                //{
                //    dc.WithoutBill_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType);
                //}
                
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                if (UC_InwardHeader1.OtherClient == false && UC_InwardHeader1.BillNo != "0")
                {
                    //bill updation
                    BillUpdation bill = new BillUpdation();
                    string BillNo = "0";
                    if (UC_InwardHeader1.BillNo != "")
                        BillNo = UC_InwardHeader1.BillNo;

                    bool updateBillFlag = true;
                    var billTable = dc.Bill_View(BillNo, 0, 0, "", 0, false, false, null, null);
                    foreach (var b in billTable)
                    {
                        totalCost = Convert.ToString(b.BILL_NetAmt_num);

                        if (b.BILL_ApproveStatus_bit != null)
                        {
                            if (b.BILL_ApproveStatus_bit == true)
                                updateBillFlag = false;
                            else
                                updateBillFlag = true;
                        }
                    }
                    if (BillNo == "0")
                    {
                        if (DateTime.Now.Day >= 26)
                        {
                            updateBillFlag = false;
                        }
                        else
                        {
                            var inward = dc.Inward_View(0, Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, null, null).ToList();
                            foreach (var inwd in inward)
                            {
                                if (inwd.SITE_MonthlyBillingStatus_bit != null && inwd.SITE_MonthlyBillingStatus_bit == true)
                                {
                                    updateBillFlag = false;
                                }
                            }
                        }
                        if (updateBillFlag == true)
                        {
                            int NewrecNo = 0;
                            //clsData clsObj = new clsData();
                            NewrecNo = clsObj.GetCurrentRecordNo("BillNo");
                            var gstbillCount = dc.Bill_View_Count(DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                            if (gstbillCount.Count() != NewrecNo - 1)
                            {
                                updateBillFlag = false;
                                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Bill No. mismatch. Can not approve report.');", true);
                                lblMsg.Text = "Record Saved Successfully, Bill No. mismatch. Can not generate bill.";
                            }
                        }
                    }
                    if (updateBillFlag == true)
                    {
                        BillNo = bill.UpdateBill("RWH", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
                        //totalCost = clsObj.getProformaBillNetAmount(BillNo,1);
                    }
                    UC_InwardHeader1.BillNo = BillNo.ToString();
                    //
                    if (BillNo != "0")
                        lnkBillPrint.Visible = true;
                }
                if (UC_InwardHeader1.POFileName != "")
                {
                    dc.Inward_Update_POFileName(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, UC_InwardHeader1.BillNo, UC_InwardHeader1.POFileName);
                }
                if (lblMsg.Text.Contains("Bill No. mismatch") == false)
                    lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkSave.Visible = false;
                lnkPrint.Visible = true;
                lnkLabSheet.Visible = true;

                //UC_InwardHeader1.RecType = null;
                UC_InwardHeader1.EnquiryNo = "";
                DropDownList ddlEnquiryList = (DropDownList)UC_InwardHeader1.FindControl("ddlEnquiryList");
                ddlEnquiryList.ClearSelection();
                LoadEnquiryList(Convert.ToInt32(UC_InwardHeader1.MaterialId));


            }
        }

        protected void LoadEnquiryList(int materialId)
        {
            var enqList = dc.Enquiry_View(0, 1, materialId);
            DropDownList ddlEnquiryList = (DropDownList)UC_InwardHeader1.FindControl("ddlEnquiryList");
            ddlEnquiryList.DataSource = enqList;
            ddlEnquiryList.DataTextField = "ENQ_Id";
            ddlEnquiryList.DataBind();
            ddlEnquiryList.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        protected Boolean ValidateData()
        {

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            string[] strDate = new string[3];
            if (UC_InwardHeader1.EnquiryNo == "")
            {
                lblMsg.Text = "Select Enquiry No.";
                DropDownList ddlEnquiryList = (DropDownList)UC_InwardHeader1.FindControl("ddlEnquiryList");
                ddlEnquiryList.Focus();
                valid = false;
            }
            else if (UC_InwardHeader1.ContactPerson == "---Select---")
            {
                lblMsg.Text = "Select Contact Person";
                AjaxControlToolkit.ComboBox cmbContactPerson = (AjaxControlToolkit.ComboBox)UC_InwardHeader1.FindControl("cmbContactPerson");
                cmbContactPerson.Focus();
                valid = false;
            }
            else if (UC_InwardHeader1.ContactNo == "")
            {
                lblMsg.Text = "Enter Contact Number";
                TextBox txtContactNo = (TextBox)UC_InwardHeader1.FindControl("txtContactNo");
                txtContactNo.Focus();
                valid = false;
            }
            else if (UC_InwardHeader1.EmailId == "")
            {
                lblMsg.Text = "Enter Email Id";
                TextBox txtEmailId = (TextBox)UC_InwardHeader1.FindControl("txtEmailId");
                txtEmailId.Focus();
                valid = false;
            }
            else if (UC_InwardHeader1.Building == "")
            {
                lblMsg.Text = "Enter Buliding";
                AjaxControlToolkit.ComboBox cmbBuilding = (AjaxControlToolkit.ComboBox)UC_InwardHeader1.FindControl("cmbBuilding");
                cmbBuilding.Focus();
                valid = false;
            }
            else if (UC_InwardHeader1.WorkOrder == "")
            {
                lblMsg.Text = "Enter Work Order";
                TextBox txtWorkOrder = (TextBox)UC_InwardHeader1.FindControl("txtWorkOrder");
                txtWorkOrder.Focus();
                valid = false;
            }
            else if (UC_InwardHeader1.Charges == "")
            {
                lblMsg.Text = "Enter Charges";
                TextBox txtCharges = (TextBox)UC_InwardHeader1.FindControl("txtCharges");
                txtCharges.Focus();
                valid = false;
            }
            else if (UC_InwardHeader1.TotalQty == "")
            {
                lblMsg.Text = "Enter Total Quantity";
                TextBox txtTotalQty = (TextBox)UC_InwardHeader1.FindControl("txtTotalQty");
                txtTotalQty.Focus();
                valid = false;
            }

            else if (UC_InwardHeader1.Subsets == "")
            {
                lblMsg.Text = "Enter Subsets";
                TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
                txtSubsets.Focus();
                valid = false;
            }
            //else if (//UC_InwardHeader1.ProposalRateMatch == false)
            //{
            //    lblMsg.Text = "Please confirm that proposal rates matches with email confirmation / work order.";
            //    CheckBox chkPropRateMatch = (CheckBox)UC_InwardHeader1.FindControl("chkPropRateMatch");
            //    //chkPropRateMatch.Focus();
            //    valid = false;
            //}
            else if (valid == true)
            {
                for (int i = 0; i <= grdRainWaterHarvestingInward.Rows.Count - 1; i++)
                {
                    TextBox txtDescription = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[1].FindControl("txtDescription");
                    TextBox txtUnit = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[2].FindControl("txtUnit");
                    TextBox txtQty = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[3].FindControl("txtQty");
                    TextBox txtRate = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[4].FindControl("txtRate");
                    if (txtDescription.Text == "")
                    {
                        lblMsg.Text = "Enter Description for row no " + (i + 1) + ".";
                        txtDescription.Focus();
                        valid = false;
                        break;
                    }
                    else if (grdRainWaterHarvestingInward.Columns[4].Visible == true)
                    {
                        if (txtUnit.Text == "")
                        {
                            lblMsg.Text = "Enter Unit for row no " + (i + 1) + ".";
                            txtUnit.Focus();
                            valid = false;
                            break;
                        }
                    }

                    if (grdRainWaterHarvestingInward.Columns[5].Visible == true)
                    {
                        if (txtQty.Text == "")
                        {
                            lblMsg.Text = "Enter Quantity for row no " + (i + 1) + ".";
                            txtQty.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (grdRainWaterHarvestingInward.Rows[i].Cells[6].Visible == true)
                    {
                        if (txtRate.Text == "")
                        {
                            lblMsg.Text = "Enter Rate for row no " + (i + 1) + ".";
                            txtRate.Focus();
                            valid = false;
                            break;
                        }
                        else if (Convert.ToInt32(txtRate.Text) <= 0)
                        {
                            lblMsg.Text = "Enter Rate greater than zero for row no " + (i + 1) + ".";
                            txtRate.Focus();
                            valid = false;
                            break;
                        }
                    }
                }
            }
            if (valid == true)
            {
                if (UC_InwardHeader1.POFileName == "" && UC_InwardHeader1.OtherClient == false)
                {
                    string BillNo = "0";
                    if (UC_InwardHeader1.BillNo != "")
                        BillNo = UC_InwardHeader1.BillNo;
                    if (BillNo == "0")
                    {
                        var site = dc.Site_View(Convert.ToInt32(UC_InwardHeader1.SiteId), 0, 0, "").ToList();
                        foreach (var st in site)
                        {
                            if (st.SITE_MonthlyBillingStatus_bit != true)
                            {
                                valid = false;
                            }
                        }
                        if (valid == false)
                        {
                            var withoutbill = dc.WithoutBill_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType);
                            if (withoutbill.Count() > 0)
                            {
                                valid = true;
                            }
                        }
                    }
                    else
                    {
                        valid = false;
                    }
                    if (valid == false)
                    {
                        lblMsg.Text = "Please upload PO File";
                    }
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
        public void DisplayRainWaterHarvestingTest()
        {

            var Modify = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, UC_InwardHeader1.RecType.ToString(), null, null);
            foreach (var n in Modify)
            {
                UC_InwardHeader1.ReceivedDate = Convert.ToDateTime(n.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy HH:mm:ss");
                UC_InwardHeader1.CollectionDate = Convert.ToDateTime(n.INWD_CollectionDate_dt).ToString("dd/MM/yyyy");
                UC_InwardHeader1.RecordNo = n.INWD_RecordNo_int.ToString();
                UC_InwardHeader1.ReferenceNo = n.INWD_ReferenceNo_int.ToString();
                UC_InwardHeader1.WorkOrder = n.INWD_WorkOrderNo_var.ToString();
                UC_InwardHeader1.Building = n.INWD_Building_var.ToString();
                UC_InwardHeader1.TotalQty = n.INWD_TotalQty_int.ToString();
                UC_InwardHeader1.ClientName = n.CL_Name_var.ToString();
                UC_InwardHeader1.SiteName = n.SITE_Name_var.ToString();
                UC_InwardHeader1.EnquiryNo = n.INWD_ENQ_Id.ToString();
                UC_InwardHeader1.ContactNo = n.INWD_ContactNo_var.ToString();
                UC_InwardHeader1.Charges = n.INWD_Charges_var.ToString();
                UC_InwardHeader1.EmailId = n.INWD_EmailId_var.ToString();
                //UC_InwardHeader1.ProposalRateMatch = true;
                string CollectionTime = n.INWD_CollectionTime_time.ToString();
                var timespan = TimeSpan.Parse(CollectionTime);
                var output = new DateTime().Add(timespan).ToString("hh:mm tt");
                UC_InwardHeader1.CollectionTime = output.ToString();
                UC_InwardHeader1.EnquiryNo = n.INWD_ENQ_Id.ToString();
                if (n.INWD_RptCL_Id != null)
                    lblRptClientId.Text = n.INWD_RptCL_Id.ToString();
                if (n.INWD_RptSITE_Id != null)
                    lblRptSiteId.Text = n.INWD_RptSITE_Id.ToString();
            }
            RainWaterHarvestingTestgrid();
        }
        public void RainWaterHarvestingTestgrid()
        {
            int i = 0;
            var res = dc.AllInward_View("RWH", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var s in res)
            {
                AddRowSoilInvestigationInward();
                TextBox txtDescription = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[1].FindControl("txtDescription");
                TextBox txtUnit = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[2].FindControl("txtUnit");
                TextBox txtQty = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[3].FindControl("txtQty");
                TextBox txtRate = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[4].FindControl("txtRate");
                txtDescription.Text = s.GTTEST_Description_var.ToString();
                if (s.GTTEST_Unit_var != "")
                {
                    txtUnit.Text = s.GTTEST_Unit_var.ToString();
                }
                if (s.GTTEST_Quantity_tint.ToString() != "")
                {
                    txtQty.Text = s.GTTEST_Quantity_tint.ToString();
                }
                if (s.GTTEST_Rate_int.ToString() != "")
                {
                    txtRate.Text = s.GTTEST_Rate_int.ToString();
                }
                i++;
                if (txtUnit.Text == "0" && txtQty.Text.ToString() == "0")
                {
                    grdRainWaterHarvestingInward.Columns[4].Visible = false;
                    grdRainWaterHarvestingInward.Columns[5].Visible = false;
                    SetWidth();
                    setAutoWidth();
                }
            }
            Displaycell();
            int count = grdRainWaterHarvestingInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.EnquiryNo = "";
        }
        public void Displaycell()
        {
            int i = 0;
            int C = 0;
            string Rate = "";
            var res = dc.AllInward_View("RWH", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var s in res)
            {
                TextBox txtRate = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[4].FindControl("txtRate");
                if (Rate != "")
                {
                    if (Rate == s.GTTEST_Rate_int.ToString())
                    {
                        TextBox txtRate1 = (TextBox)grdRainWaterHarvestingInward.Rows[i - 1].Cells[4].FindControl("txtRate");
                        txtRate.BorderStyle = BorderStyle.None;
                        txtRate1.BorderStyle = BorderStyle.None;
                        grdRainWaterHarvestingInward.Rows[i].Cells[6].Visible = false;
                        grdRainWaterHarvestingInward.BackColor = System.Drawing.Color.White;
                        txtFrmRow.Visible = true;
                        Label2.Visible = true;
                        Label3.Visible = true;
                        txtToRow.Visible = true;
                        Btn_Apply.Visible = true;
                        chk_Lumshup.Checked = true;
                        if (txtFrmRow.Text == "")
                        {
                            txtFrmRow.Text = C.ToString();
                        }
                        txtToRow.Text = C.ToString();
                    }
                    else
                    {
                        txtRate.BorderStyle = BorderStyle.NotSet;
                        grdRainWaterHarvestingInward.Rows[i].Cells[6].Visible = true;
                        grdRainWaterHarvestingInward.Rows[i].Cells[6].BorderColor = System.Drawing.Color.White;
                    }
                }
                if (s.GTTEST_Rate_int.ToString() != "")
                {
                    txtRate.Text = s.GTTEST_Rate_int.ToString();
                    Rate = txtRate.Text;
                }
                C++;
                i++;
            }
            if (txtToRow.Text != "")
            {
                txtToRow.Text = (Convert.ToInt32(txtToRow.Text) + 1).ToString();
            }
        }



        protected void Btn_Apply_OnClick(object sender, EventArgs e)
        {
            ShowMerge();
            ChkLumShup();
        }
        protected void chk_Lumshup_OnCheckedChanged(object sender, EventArgs e)
        {
            ShowMerge();
            ChkLumShup();
        }
        public void ChkLumShup()
        {
            if (chk_Lumshup.Checked == true)
            {
                grdRainWaterHarvestingInward.Columns[4].Visible = false;
                grdRainWaterHarvestingInward.Columns[5].Visible = false;
                SetWidth();
                txtFrmRow.Focus();
                txtFrmRow.Visible = true;
                txtToRow.Visible = true;
                Btn_Apply.Visible = true;
                Label2.Visible = true;
                Label3.Visible = true;
            }
            else
            {
                grdRainWaterHarvestingInward.Columns[4].Visible = true;
                grdRainWaterHarvestingInward.Columns[5].Visible = true;
                setAutoWidth();
                txtFrmRow.Text = string.Empty;
                txtFrmRow.Visible = false;
                txtToRow.Visible = false;
                txtToRow.Text = string.Empty;
                Btn_Apply.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
            }
        }
        public void ShowMerge()
        {
            if (txtFrmRow.Text != "" && txtToRow.Text != "")
            {
                int FromRowNo = Convert.ToInt32(txtFrmRow.Text);
                int ToRowNo = Convert.ToInt32(txtToRow.Text);
                if (ToRowNo > FromRowNo)
                {
                    for (int i = 0; i < grdRainWaterHarvestingInward.Rows.Count; i++)
                    {
                        TextBox txtRate1 = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[4].FindControl("txtRate");
                        Boolean foundIt = false;
                        if (i >= FromRowNo && i <= ToRowNo)
                        {
                            for (int j = 0; j < ToRowNo; j++)
                            {
                                if (j == i)
                                {
                                    TextBox txtRate = (TextBox)grdRainWaterHarvestingInward.Rows[i - 1].Cells[4].FindControl("txtRate");
                                    txtRate.BorderStyle = BorderStyle.None;
                                    grdRainWaterHarvestingInward.Rows[i].Cells[6].Visible = false;
                                    grdRainWaterHarvestingInward.BackColor = System.Drawing.Color.White;
                                    foundIt = true;
                                    break;
                                }
                                else
                                {
                                    txtRate1.BorderStyle = BorderStyle.NotSet;
                                    grdRainWaterHarvestingInward.Rows[i].Cells[6].Visible = true;
                                    grdRainWaterHarvestingInward.Rows[i].Cells[6].BorderColor = System.Drawing.Color.White;
                                }
                            }
                        }
                        if (foundIt == false)
                        {
                            grdRainWaterHarvestingInward.Rows[i].Cells[6].Visible = true;
                            txtRate1.BorderStyle = BorderStyle.NotSet;
                        }
                    }
                }
            }
        }
        public void SetWidth()
        {

            for (int i = 0; i < grdRainWaterHarvestingInward.Rows.Count; i++)
            {
                TextBox txtDescription = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[1].FindControl("txtDescription");
                TextBox txtRate = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[4].FindControl("txtRate");
                if (grdRainWaterHarvestingInward.Columns[4].Visible == false &&
                    grdRainWaterHarvestingInward.Columns[5].Visible == false)
                {
                    grdRainWaterHarvestingInward.Columns[5].ItemStyle.Width = 690;
                    grdRainWaterHarvestingInward.Columns[6].ItemStyle.Width = 200;
                    txtDescription.Width = 690;
                    txtRate.Width = 200;

                }

            }
        }
        public void setAutoWidth()
        {
            for (int i = 0; i < grdRainWaterHarvestingInward.Rows.Count; i++)
            {
                if (grdRainWaterHarvestingInward.Columns[4].Visible == true &&
                   grdRainWaterHarvestingInward.Columns[5].Visible == true)
                {
                    TextBox txtDescription = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[1].FindControl("txtDescription");
                    TextBox txtRate = (TextBox)grdRainWaterHarvestingInward.Rows[i].Cells[4].FindControl("txtRate");
                    grdRainWaterHarvestingInward.Rows[i].Cells[6].Visible = true;
                    grdRainWaterHarvestingInward.Columns[6].ItemStyle.Width = 130;
                    txtRate.BorderStyle = BorderStyle.NotSet;
                    txtDescription.Width = 518;
                    txtRate.Width = 155;//130;
                    txtRate.Text = "";
                }
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            //string reportPath;
            string reportStr = "";
            //StreamWriter sw;
            InwardReport rpt = new InwardReport();
            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            if (UC_InwardHeader1.RecordNo != "" && UC_InwardHeader1.ReferenceNo != "")
            {
                reportStr = rpt.getDetailReportRWH(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("RWH_Inward", reportStr);
        }
        bool buttonClicked = false;
        protected string getDetailReportRWH()
        {
            string reportStr = "", mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (buttonClicked == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Rain Water Harvesting Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Rain Water Harvesting Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("RWH", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }


            var b = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, "RWH", null, null);

            foreach (var nt in b)
            {
                if (buttonClicked == true)
                {
                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "RWH" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "RWH" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                      "</tr>";
                }
                else
                {

                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "RWH" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "RWH" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.SITE_Address_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Unit </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Quantity </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Rate</b></font></td>";
            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("RWH", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.GTTEST_Description_var + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.GTTEST_Unit_var.ToString() + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.GTTEST_Quantity_tint.ToString() + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.GTTEST_Rate_int.ToString() + "</font></td>";

                mySql += "</tr>";
                i++;
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

        protected void lnkLabSheet_Click(object sender, EventArgs e)
        {
            buttonClicked = true;
            //string reportPath;
            string reportStr = "";
            //StreamWriter sw;
            InwardReport rpt = new InwardReport();
            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            if (UC_InwardHeader1.RecordNo != "" && UC_InwardHeader1.ReferenceNo != "")
            {
                reportStr = rpt.getDetailReportRWH(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), true);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("RWH_LabSheet", reportStr);
        }
        //protected void imgClosePopup_Click(object sender, EventArgs e)
        //{
        //    if (Session["InwardStatus"].ToString() == "Add")
        //        Response.Redirect("Enquiry_List.aspx");
        //    else if (Session["InwardStatus"].ToString() == "Edit")
        //        Response.Redirect("Frm_InwardStatus.aspx");
        //}
        protected void lnkBillPrint_Click(object sender, EventArgs e)
        {
            buttonClicked = true;
            //string reportPath;
            //string reportStr = "";
            //StreamWriter sw;
            //BillUpdation bill = new BillUpdation();

            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            //reportStr = bill.getBillPrintString(Convert.ToInt32(UC_InwardHeader1.BillNo), false);

            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            //BillUpdation bill = new BillUpdation();
            //bill.getBillPrintString(UC_InwardHeader1.BillNo, false);
            PrintPDFReport obj = new PrintPDFReport();
            obj.Bill_PDFPrint(UC_InwardHeader1.BillNo, false, "print");
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            LoadPreviousPage();
        }
        protected void lnk_Exit_Click(object sender, EventArgs e)
        {
            LoadPreviousPage();
        }
        protected void LoadPreviousPage()
        {
            if (UC_InwardHeader1.InwdStatus == "Edit")
            {
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "RWH")));
            }
            else
            {
                object refUrl = ViewState["RefUrl"];
                if (refUrl != null)
                {
                    Response.Redirect((string)refUrl);
                }
            }
        }

        protected void lnkSelectRptClientSite_Click(object sender, EventArgs e)
        {
            if (UC_InwardHeader1.EnquiryNo == "")
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Select Enquiry No.";
                lblMsg.Visible = true;
                DropDownList ddlEnquiryList = (DropDownList)UC_InwardHeader1.FindControl("ddlEnquiryList");
                ddlEnquiryList.Focus();
            }
            else
            {
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                string strURLWithData = "ReportForClient.aspx?" + obj.Encrypt(string.Format("EnquiryNo={0}&RecordType={1}&RecordNo={2}", UC_InwardHeader1.EnquiryNo, UC_InwardHeader1.RecType, UC_InwardHeader1.RecordNo));
                //Response.Redirect(strURLWithData);
                PrintGrid.Redirect1(strURLWithData, "_blank", "menubar=1,HEIGHT=300,WIDTH=820,scrollbars=yes,resizable=yes");
            }
        }
    }
}
