using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Core_Inward : System.Web.UI.Page
    {
        InwardReport rpt = new InwardReport();
        LabDataDataContext dc = new LabDataDataContext();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                lblheading.Text = "Core Testing";
                LoadTest();
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    AddRowCorelInward();
                    AddRowCoreOtherCharge();
                }
                  
                if (UC_InwardHeader1.RecType != "")
                {
                    DisplayCoreInwardTest();
                    DisplayOtherCharges();
                }
                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowCorelInward();
                    AddRowCoreOtherCharge();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "CR";
                }
                lnkSave.Visible = true;
            }
        }
        protected void LoadTest()
        {
            //var test = dc.Test_View(0, 0, "CR", 0, 0, 0);
            var test = dc.Test_View_ForInward("CR", 0);
            ddl_BillFor.DataSource = test;
            ddl_BillFor.DataTextField = "TEST_Name_var";
            ddl_BillFor.DataValueField = "TEST_Id";
            ddl_BillFor.DataBind();
        }
        protected void AddRowCorelInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CoreInwardTable"] != null)
            {
                GetCurrentDataCoreInward();
                dt = (DataTable)ViewState["CoreInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDateOfCasting", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlGradeOfConcrete", typeof(string)));
                dt.Columns.Add(new DataColumn("txtQty", typeof(string)));
                dt.Columns.Add(new DataColumn("txtConcreteMember", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDateOfSpecExtraction", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtDescription"] = string.Empty;
            dr["txtDateOfCasting"] = string.Empty;
            dr["ddlGradeOfConcrete"] = string.Empty;
            dr["txtQty"] = string.Empty;
            dr["txtConcreteMember"] = string.Empty;
            dr["txtDateOfSpecExtraction"] = string.Empty;

            dt.Rows.Add(dr);

            ViewState["CoreInwardTable"] = dt;
            grdCoreInward.DataSource = dt;
            grdCoreInward.DataBind();
            SetPreviousDataCoreInward();
        }
        protected void DeleteRowCoreInward(int rowIndex)
        {
            GetCurrentDataCoreInward();
            DataTable dt = ViewState["CoreInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CoreInwardTable"] = dt;
            grdCoreInward.DataSource = dt;
            grdCoreInward.DataBind();
            SetPreviousDataCoreInward();
        }
        protected void SetPreviousDataCoreInward()
        {
            DataTable dt = (DataTable)ViewState["CoreInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdCoreInward.Rows[i].Cells[1].FindControl("txtDescription");
                TextBox box3 = (TextBox)grdCoreInward.Rows[i].Cells[2].FindControl("txtDateOfCasting");
                DropDownList box4 = (DropDownList)grdCoreInward.Rows[i].Cells[3].FindControl("ddlGradeOfConcrete");
                TextBox box5 = (TextBox)grdCoreInward.Rows[i].Cells[4].FindControl("txtQty");
                TextBox box6 = (TextBox)grdCoreInward.Rows[i].Cells[5].FindControl("txtConcreteMember");
                TextBox box7 = (TextBox)grdCoreInward.Rows[i].Cells[6].FindControl("txtDateOfSpecExtraction");
                grdCoreInward.Rows[i].Cells[0].Text = (i + 1).ToString();
                box2.Text = dt.Rows[i]["txtDescription"].ToString();
                box3.Text = dt.Rows[i]["txtDateOfCasting"].ToString();
                box4.Text = dt.Rows[i]["ddlGradeOfConcrete"].ToString();
                box5.Text = dt.Rows[i]["txtQty"].ToString();
                box6.Text = dt.Rows[i]["txtConcreteMember"].ToString();
                box7.Text = dt.Rows[i]["txtDateOfSpecExtraction"].ToString();

            }
        }
        protected void GetCurrentDataCoreInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDateOfCasting", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlGradeOfConcrete", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtQty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtConcreteMember", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDateOfSpecExtraction", typeof(string)));

            for (int i = 0; i < grdCoreInward.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdCoreInward.Rows[i].Cells[1].FindControl("txtDescription");
                TextBox box3 = (TextBox)grdCoreInward.Rows[i].Cells[2].FindControl("txtDateOfCasting");
                DropDownList box4 = (DropDownList)grdCoreInward.Rows[i].Cells[3].FindControl("ddlGradeOfConcrete");
                TextBox box5 = (TextBox)grdCoreInward.Rows[i].Cells[4].FindControl("txtQty");
                TextBox box6 = (TextBox)grdCoreInward.Rows[i].Cells[5].FindControl("txtConcreteMember");
                TextBox box7 = (TextBox)grdCoreInward.Rows[i].Cells[6].FindControl("txtDateOfSpecExtraction");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txtDescription"] = box2.Text;
                drRow["txtDateOfCasting"] = box3.Text;
                drRow["ddlGradeOfConcrete"] = box4.Text;
                drRow["txtQty"] = box5.Text;
                drRow["txtConcreteMember"] = box6.Text;
                drRow["txtDateOfSpecExtraction"] = box7.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CoreInwardTable"] = dtTable;
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                //get report client id, site
                string totalCost = "0";
                clsData clsObj = new clsData();
                var enqcl = dc.EnquiryClient_View(Convert.ToInt32(UC_InwardHeader1.EnquiryNo));
                foreach (var ec in enqcl)
                {
                    lblRptClientId.Text = ec.ENQCL_CL_Id.ToString();
                    lblRptSiteId.Text = ec.ENQCL_SITE_Id.ToString();
                }
                //
                string RefNo, SetOfRecord;
                int SrNo;
                //DateTime ReceivedDate = Convert.ToDateTime(DateTime.Now.Date.ToString());
                DateTime ReceivedDate = DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null);
                DateTime d1 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "CR", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, true);
                }
                else
                {
                    Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    NewrecNo = clsObj.GetnUpdateRecordNo("CR");
                    UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                    clsObj.insertRecordTable(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), NewrecNo, "CR");
                    UC_InwardHeader1.ReferenceNo = clsObj.getRefNo(Convert.ToInt32(UC_InwardHeader1.EnquiryNo)).ToString();
                    dc.Inward_Update_New(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "CR", 0, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate);
                    //NewrecNo = clsObj.GetnUpdateRecordNo("CR");
                    //UC_InwardHeader1.RecordNo = NewrecNo.ToString(); 
                    //UC_InwardHeader1.ReferenceNo = dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "CR", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, false).ToString();
                    ////var inward = dc.Inward_View(Convert.ToInt32(UC_InwardHeader1.ReferenceNo), 0, "");
                    ////foreach (var inwd in inward)
                    ////{
                    ////    UC_InwardHeader1.RecordNo = inwd.INWD_RecordNo_int.ToString();
                    ////}
                }
                dc.Enquiry_Update_Status(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 2);
                dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "CR", "", null, null, false, false, false, false, false, false, true);//Delete MIS Record

                dc.CoreInward_Update("CR", Convert.ToInt32(UC_InwardHeader1.RecordNo), "", "", 0, 0, "", null, "", 0, "", "", "", null, "", 0,null,null,"","",0,0, true);
                if (lblRptClientId.Text != "" && lblRptClientId.Text != "0")
                {
                    dc.Inward_Update_ReportClient(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, Convert.ToInt32(lblRptClientId.Text), Convert.ToInt32(lblRptSiteId.Text));
                }
                for (int i = 0; i <= grdCoreInward.Rows.Count - 1; i++)
                {
                    TextBox txtDescription = (TextBox)grdCoreInward.Rows[i].Cells[1].FindControl("txtDescription");
                    TextBox txtDateOfCasting = (TextBox)grdCoreInward.Rows[i].Cells[2].FindControl("txtDateOfCasting");
                    DropDownList ddlGradeOfConcrete = (DropDownList)grdCoreInward.Rows[i].Cells[3].FindControl("ddlGradeOfConcrete");
                    TextBox txtQty = (TextBox)grdCoreInward.Rows[i].Cells[4].FindControl("txtQty");
                    TextBox txtConcreteMember = (TextBox)grdCoreInward.Rows[i].Cells[5].FindControl("txtConcreteMember");
                    TextBox txtDateOfSpecExtraction = (TextBox)grdCoreInward.Rows[i].Cells[6].FindControl("txtDateOfSpecExtraction");

                    SrNo = Convert.ToInt32(grdCoreInward.Rows[i].Cells[0].Text);
                    RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    //int TestId = 0;
                    //var testid = dc.Test_View(0, 0, "CR", 0, 0);
                    //foreach (var testd in testid)
                    //{
                    //    TestId = testd.TEST_Id;
                    //}
                    dc.CoreInward_Update("CR", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txtQty.Text), txtDescription.Text, d1, txtDateOfCasting.Text, 0, "", ddlGradeOfConcrete.Text, txtConcreteMember.Text, txtDateOfSpecExtraction.Text, ddl_BillFor.SelectedItem.Text, Convert.ToInt32(ddl_BillFor.SelectedValue), d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), false);

                    dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "CR", RefNo, "CR", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);
                }
                SrNo = 1;
                dc.OtherCharges_Update(0, "", 0, 0, 0, UC_InwardHeader1.RecType, Convert.ToInt32(UC_InwardHeader1.RecordNo), true);
                for (int i = 0; i <= grdOtherCharges.Rows.Count - 1; i++)
                {
                    TextBox txtDescription = (TextBox)grdOtherCharges.Rows[i].Cells[3].FindControl("txtDescription");
                    TextBox txtQuantity = (TextBox)grdOtherCharges.Rows[i].Cells[4].FindControl("txtQuantity");
                    TextBox txtRate = (TextBox)grdOtherCharges.Rows[i].Cells[5].FindControl("txtRate");
                    TextBox txtAmount = (TextBox)grdOtherCharges.Rows[i].Cells[6].FindControl("txtAmount");

                    if (txtDescription.Text != "" && txtQuantity.Text != "" && txtRate.Text != "" && txtAmount.Text != "")
                    {
                        dc.OtherCharges_Update(SrNo, txtDescription.Text, Convert.ToDecimal(txtQuantity.Text), Convert.ToDecimal(txtRate.Text), Convert.ToDecimal(txtAmount.Text), UC_InwardHeader1.RecType, Convert.ToInt32(UC_InwardHeader1.RecordNo), false);
                        SrNo++;
                    }
                }
                UC_InwardHeader1.TestReqFormNo = "Test Request Form No : " + UC_InwardHeader1.ReferenceNo + "/" + DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null).Year.ToString().Substring(2, 2);
                if (UC_InwardHeader1.OtherClient == true)
                {
                    dc.WithoutBill_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType);
                    if (UC_InwardHeader1.BillNo != "" && UC_InwardHeader1.BillNo != "0")
                    {
                        dc.Inward_Update_BillNo(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, UC_InwardHeader1.BillNo);
                    }
                }
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
                        BillNo = bill.UpdateBill("CR", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
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
                //if (UC_InwardHeader1.OtherClient == false && UC_InwardHeader1.ProformaInvoiceNo != "0")
                //{
                //    //ProformaInvoice updation
                //    ProformaInvoiceUpdation ProInv = new ProformaInvoiceUpdation();
                //    string ProformaInvoiceNo = "0";
                //    if (UC_InwardHeader1.ProformaInvoiceNo != "")
                //        ProformaInvoiceNo = UC_InwardHeader1.ProformaInvoiceNo;

                //    bool updateProformaInvoiceFlag = true;
                //    var ProformaInvoiceTable = dc.ProformaInvoice_View(ProformaInvoiceNo, 0, 0, "", 0, false, false, null, null);
                //    foreach (var b in ProformaInvoiceTable)
                //    {
                //        if (b.PROINV_ApproveStatus_bit != null)
                //        {
                //            if (b.PROINV_ApproveStatus_bit == true)
                //                updateProformaInvoiceFlag = false;
                //            else
                //                updateProformaInvoiceFlag = true;
                //        }
                //    }
                //    if (updateProformaInvoiceFlag == true)
                //    {
                //        ProformaInvoiceNo = ProInv.UpdateProformaInvoice("CR", Convert.ToInt32(UC_InwardHeader1.RecordNo), ProformaInvoiceNo);
                //        totalCost = clsObj.getProformaBillNetAmount(ProformaInvoiceNo, 1);
                //    }
                //    UC_InwardHeader1.ProformaInvoiceNo = ProformaInvoiceNo.ToString();
                //    //
                //    lnkBillPrint.Visible = true;
                //}
                //sms
                if (UC_InwardHeader1.InwdStatus != "Edit")
                {
                    clsObj.sendInwardReportMsg(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), UC_InwardHeader1.EnqDate, UC_InwardHeader1.SiteMonthlyStatus, UC_InwardHeader1.ClCreditLimitExcededStatus, totalCost, "", UC_InwardHeader1.ContactNo.ToString(), "Inward");
                }

                if (lblMsg.Text.Contains("Bill No. mismatch") == false)
                    lblMsg.Text = "Record Saved Successfully";
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

        #region CoreOtherCharges
        public void DisplayOtherCharges()
        {
            int i = 0;
            var re = dc.OtherCharges_View(UC_InwardHeader1.RecType, Convert.ToInt32(UC_InwardHeader1.RecordNo));
            foreach (var r in re)
            {
                AddRowCoreOtherCharge();
                TextBox txtDescription = (TextBox)grdOtherCharges.Rows[i].Cells[3].FindControl("txtDescription");
                TextBox txtQuantity = (TextBox)grdOtherCharges.Rows[i].Cells[4].FindControl("txtQuantity");
                TextBox txtRate = (TextBox)grdOtherCharges.Rows[i].Cells[5].FindControl("txtRate");
                TextBox txtAmount = (TextBox)grdOtherCharges.Rows[i].Cells[6].FindControl("txtAmount");

                txtDescription.Text = r.OTHERCHRG_Description_var;
                txtQuantity.Text = Convert.ToDecimal(r.OTHERCHRG_Quantity_int).ToString("0.##");
                txtRate.Text = Convert.ToDecimal(r.OTHERCHRG_Rate_num).ToString("0.00");
                txtAmount.Text = Convert.ToDecimal(r.OTHERCHRG_Amount_num).ToString("0.00");
                i++;
            }
            if (grdOtherCharges.Rows.Count <= 0)
            {
                AddRowCoreOtherCharge();
            }

        }
        protected void AddRowCoreOtherCharge()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CoreOtherCharges_Table"] != null)
            {
                GetCurrentDataCoreOtherCharges();
                dt = (DataTable)ViewState["CoreOtherCharges_Table"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("txtQuantity", typeof(string)));
                dt.Columns.Add(new DataColumn("txtRate", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAmount", typeof(string)));

            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtDescription"] = string.Empty;
            dr["txtQuantity"] = string.Empty;
            dr["txtRate"] = string.Empty;
            dr["txtAmount"] = string.Empty;
            dt.Rows.Add(dr);


            ViewState["CoreOtherCharges_Table"] = dt;
            grdOtherCharges.DataSource = dt;
            grdOtherCharges.DataBind();
            SetPreviousDataCoreOtherCharges();
        }
        protected void GetCurrentDataCoreOtherCharges()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtQuantity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAmount", typeof(string)));

            for (int i = 0; i < grdOtherCharges.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)grdOtherCharges.Rows[i].FindControl("txtDescription");
                TextBox box2 = (TextBox)grdOtherCharges.Rows[i].FindControl("txtQuantity");
                TextBox box3 = (TextBox)grdOtherCharges.Rows[i].FindControl("txtRate");
                TextBox box4 = (TextBox)grdOtherCharges.Rows[i].FindControl("txtAmount");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txtDescription"] = box1.Text;
                drRow["txtQuantity"] = box2.Text;
                drRow["txtRate"] = box3.Text;
                drRow["txtAmount"] = box4.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CoreOtherCharges_Table"] = dtTable;

        }
        protected void SetPreviousDataCoreOtherCharges()
        {
            DataTable dt = (DataTable)ViewState["CoreOtherCharges_Table"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)grdOtherCharges.Rows[i].FindControl("txtDescription");
                TextBox box2 = (TextBox)grdOtherCharges.Rows[i].FindControl("txtQuantity");
                TextBox box3 = (TextBox)grdOtherCharges.Rows[i].FindControl("txtRate");
                TextBox box4 = (TextBox)grdOtherCharges.Rows[i].FindControl("txtAmount");

                grdOtherCharges.Rows[i].Cells[2].Text = (i + 1).ToString();
                box1.Text = dt.Rows[i]["txtDescription"].ToString();
                box2.Text = dt.Rows[i]["txtQuantity"].ToString();
                box3.Text = dt.Rows[i]["txtRate"].ToString();
                box4.Text = dt.Rows[i]["txtAmount"].ToString();

            }
        }
        protected void DeleteRowCoreOtherCharges(int rowIndex)
        {
            GetCurrentDataCoreOtherCharges();
            DataTable dt = ViewState["CoreOtherCharges_Table"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CoreOtherCharges_Table"] = dt;
            grdOtherCharges.DataSource = dt;
            grdOtherCharges.DataBind();
            SetPreviousDataCoreOtherCharges();
        }
        protected void imgInsert_Click(object sender, CommandEventArgs e)
        {
            AddRowCoreOtherCharge();
        }
        protected void imgDelete_Click(object sender, CommandEventArgs e)
        {
            if (grdOtherCharges.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdOtherCharges.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowCoreOtherCharges(gvr.RowIndex);
            }
        }
        protected void txtQuantity_TextChanged(object sender, EventArgs eventArgs)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txtQuantity = (TextBox)grdOtherCharges.Rows[rowindex].FindControl("txtQuantity");
            TextBox txtRate = (TextBox)grdOtherCharges.Rows[rowindex].FindControl("txtRate");
            TextBox txtAmount = (TextBox)grdOtherCharges.Rows[rowindex].FindControl("txtAmount");

            if (txtQuantity.Text == "")
            {
                txtAmount.Text = "";
            }
            if (txtRate.Text != "" && txtQuantity.Text != "")
            {
                txtAmount.Text = Convert.ToDecimal(Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQuantity.Text)).ToString("0.00");
            }
        }
        protected void txtRate_TextChanged(object sender, EventArgs eventArgs)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txtQuantity = (TextBox)grdOtherCharges.Rows[rowindex].FindControl("txtQuantity");
            TextBox txtRate = (TextBox)grdOtherCharges.Rows[rowindex].FindControl("txtRate");
            TextBox txtAmount = (TextBox)grdOtherCharges.Rows[rowindex].FindControl("txtAmount");

            if (txtRate.Text == "")
            {
                txtAmount.Text = "";
            }
            if (txtRate.Text != "" && txtQuantity.Text != "")
            {
                txtAmount.Text = Convert.ToDecimal(Convert.ToDecimal(txtQuantity.Text) * Convert.ToDecimal(txtRate.Text)).ToString("0.00");
            }
        }
        #endregion

        protected void LoadEnquiryList(int materialId)
        {
            var enqList = dc.Enquiry_View(0, 1, materialId);
            DropDownList ddlEnquiryList = (DropDownList)UC_InwardHeader1.FindControl("ddlEnquiryList");
            ddlEnquiryList.DataSource = enqList;
            ddlEnquiryList.DataTextField = "ENQ_Id";
            ddlEnquiryList.DataBind();
            ddlEnquiryList.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        protected void TextChanged(object sender, EventArgs e)
        {
            TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
            if (txtSubsets.Text != "")
            {
                if (Convert.ToInt32(txtSubsets.Text) < grdCoreInward.Rows.Count)
                {
                    for (int i = grdCoreInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdCoreInward.Rows.Count > 1)
                        {
                            DeleteRowCoreInward(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdCoreInward.Rows.Count)
                {
                    for (int i = grdCoreInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
                    {
                        AddRowCorelInward();
                    }
                }
            }
        }
        protected void Click(object sender, EventArgs e)
        {
            ViewState["CoreInwardTable"] = null;
            AddRowCorelInward();
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

        protected Boolean ValidateData()
        {
            ClientScriptManager CSM = Page.ClientScript;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            int sumQty = 0;


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
            //else if (UC_InwardHeader1.Charges == "")
            //{
            //    lblMsg.Text = "Enter Charges";
            //    TextBox txtCharges = (TextBox)UC_InwardHeader1.FindControl("txtCharges");
            //    txtCharges.Focus();
            //    valid = false;
            //}
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
            else if (UC_InwardHeader1.OtherClient == true && UC_InwardHeader1.BillNo != "" && UC_InwardHeader1.BillNo != "0")
            {
                string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                if ((cnStr.ToLower().Contains("mumbai") == true && UC_InwardHeader1.BillNo.ToLower().Contains("mum") == true) ||
                    (cnStr.ToLower().Contains("nashik") == true && UC_InwardHeader1.BillNo.ToLower().Contains("nsk") == true) ||
                    (cnStr.ToLower().Contains("live") == true && UC_InwardHeader1.BillNo.ToLower().Contains("pun") == true))
                {
                    lblMsg.Text = "Enter valid bill number, bill number should not be of same branch.";
                    TextBox txtBillNo = (TextBox)UC_InwardHeader1.FindControl("txtBillNo");
                    txtBillNo.Focus();
                    valid = false;
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
            if (valid == true)
            {
                for (int i = 0; i < grdCoreInward.Rows.Count; i++)
                {
                    TextBox txtDescription = (TextBox)grdCoreInward.Rows[i].Cells[1].FindControl("txtDescription");
                    TextBox txtDateOfCasting = (TextBox)grdCoreInward.Rows[i].Cells[2].FindControl("txtDateOfCasting");
                    DropDownList ddlGradeOfConcrete = (DropDownList)grdCoreInward.Rows[i].Cells[3].FindControl("ddlGradeOfConcrete");
                    TextBox txtQty = (TextBox)grdCoreInward.Rows[i].Cells[4].FindControl("txtQty");
                    TextBox txtConcreteMember = (TextBox)grdCoreInward.Rows[i].Cells[5].FindControl("txtConcreteMember");
                    TextBox txtDateOfSpecExtraction = (TextBox)grdCoreInward.Rows[i].Cells[6].FindControl("txtDateOfSpecExtraction");
                    txtDateOfCasting.Text = txtDateOfCasting.Text.ToUpper();
                    if (txtDescription.Text == "")
                    {
                        lblMsg.Text = "Enter Description for Sr No. " + (i + 1) + ".";
                        txtDescription.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtDateOfCasting.Text == "")
                    {
                        lblMsg.Text = "Enter Date Of Casting for Sr No. " + (i + 1) + ".";
                        txtDateOfCasting.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddlGradeOfConcrete.Text == "Select")
                    {
                        lblMsg.Text = "Select Grade Of Concrete for Sr No.  " + (i + 1) + ".";
                        ddlGradeOfConcrete.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtQty.Text == "")
                    {
                        lblMsg.Text = "Enter Quantity for Sr No.  " + (i + 1) + ".";
                        txtQty.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtConcreteMember.Text == "")
                    {
                        lblMsg.Text = "Enter Concrete Member for Sr No.  " + (i + 1) + ".";
                        txtConcreteMember.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtDateOfSpecExtraction.Text == "")
                    {
                        lblMsg.Text = "Enter Date Of Spec. Extraction for Sr No.  " + (i + 1) + ".";
                        txtDateOfSpecExtraction.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtDateOfCasting.Text != "NA")
                    {
                        // DateTime txtDateOfCasting1 = Convert.ToDateTime(txtDateOfCasting.Text);
                        DateTime txtDateOfCasting1 = DateTime.ParseExact(txtDateOfCasting.Text, "dd/MM/yyyy", null);
                        DateTime txtDateOfSpecExtraction1 = DateTime.ParseExact(txtDateOfSpecExtraction.Text, "dd/MM/yyyy", null);
                        // DateTime txtDateOfSpecExtraction1 = Convert.ToDateTime(txtDateOfSpecExtraction.Text);
                        if (txtDateOfCasting1 >= txtDateOfSpecExtraction1)
                        {
                            lblMsg.Text = "Casting Date should be always less than the Date of Spec. Extraction for Sr No.  " + (i + 1) + ".";
                            txtDateOfCasting.Focus();
                            valid = false;
                            break;
                        }
                    }
                    sumQty += Convert.ToInt32(txtQty.Text);
                }
            }
            if (valid == true)
            {
                if (Convert.ToDecimal(UC_InwardHeader1.TotalQty) != Convert.ToDecimal(sumQty))
                {
                    lblMsg.Text = "Total Quantity does not match to the below Total Grid Qty ";
                    valid = false;

                }
            }
            if (valid == true)
            {
                for (int i = 0; i < grdOtherCharges.Rows.Count; i++)
                {
                    TextBox txtDescription = (TextBox)grdOtherCharges.Rows[i].Cells[3].FindControl("txtDescription");
                    TextBox txtQuantity = (TextBox)grdOtherCharges.Rows[i].Cells[4].FindControl("txtQuantity");
                    TextBox txtRate = (TextBox)grdOtherCharges.Rows[i].Cells[5].FindControl("txtRate");
                    TextBox txtAmount = (TextBox)grdOtherCharges.Rows[i].Cells[6].FindControl("txtAmount");

                    if (txtDescription.Text.Trim() != "" || txtQuantity.Text.Trim() != "" || txtRate.Text.Trim() != "")
                    {
                        if (txtDescription.Text == "")
                        {
                            lblMsg.Text = "Enter Particular for Sr No. " + (i + 1) + ".";
                            txtDescription.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtQuantity.Text == "")
                        {
                            lblMsg.Text = "Enter Quantity for Sr No. " + (i + 1) + ".";
                            txtQuantity.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtRate.Text == "")
                        {
                            lblMsg.Text = "Enter Rate for Sr No. " + (i + 1) + ".";
                            txtRate.Focus();
                            valid = false;
                            break;
                        }
                    }

                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
                // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + dispalyMsg + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
        public void DisplayCoreInwardTest()
        {         
            
            var Modify = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo ), null,  UC_InwardHeader1.RecType.ToString(), null, null);
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
                //UC_InwardHeader1.Charges = n.INWD_Charges_var.ToString();
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
            DisplayCoreTestGrid();

        }
        public void DisplayCoreTestGrid()
        {
            int i = 0;
            var res = dc.AllInward_View("CR", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var cr in res)
            {
                AddRowCorelInward();
                TextBox txtDescription = (TextBox)grdCoreInward.Rows[i].Cells[1].FindControl("txtDescription");
                TextBox txtDateOfCasting = (TextBox)grdCoreInward.Rows[i].Cells[2].FindControl("txtDateOfCasting");
                DropDownList ddlGradeOfConcrete = (DropDownList)grdCoreInward.Rows[i].Cells[3].FindControl("ddlGradeOfConcrete");
                TextBox txtQty = (TextBox)grdCoreInward.Rows[i].Cells[4].FindControl("txtQty");
                TextBox txtConcreteMember = (TextBox)grdCoreInward.Rows[i].Cells[5].FindControl("txtConcreteMember");
                TextBox txtDateOfSpecExtraction = (TextBox)grdCoreInward.Rows[i].Cells[6].FindControl("txtDateOfSpecExtraction");
                txtDescription.Text = cr.CRINWD_Description_var.ToString();
                //ddl_BillFor.ClearSelection();
                //if (cr.CRINWD_BillFor_var == "Cutting & Testing")
                //{
                //    ddl_BillFor.Items.FindByText("Cutting & Testing").Selected = true;
                //}
                //else
                //{
                //    ddl_BillFor.Items.FindByText("Only Core Testing").Selected = true;
                //}
                ddl_BillFor.SelectedValue = cr.CRINWD_TestId_int.ToString();
                if (cr.CRINWD_CastingDate_dt == "NA")
                {
                    txtDateOfCasting.Text = "NA";
                }
                else
                {
                    txtDateOfCasting.Text = cr.CRINWD_CastingDate_dt.ToString();
                    //txtDateOfCasting.Text = Convert.ToDateTime(cr.CRINWD_CastingDate_dt).ToString("dd/MM/yyyy");
                }
                ddlGradeOfConcrete.SelectedItem.Text = cr.CRINWD_Grade_var.ToString();
                txtQty.Text = cr.CRINWD_Quantity_tint.ToString();
                txtConcreteMember.Text = cr.CRINWD_ConcreteMember_var.ToString();
                if (cr.CRINWD_SpecimenExtDate_dt != "")
                {
                    txtDateOfSpecExtraction.Text = cr.CRINWD_SpecimenExtDate_dt.ToString();
                    //txtDateOfSpecExtraction.Text = Convert.ToDateTime(cr.CRINWD_SpecimenExtDate_dt).ToString("dd/MM/yyyy");
                }
                i++;
            }
            int count = grdCoreInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
             UC_InwardHeader1.EnquiryNo = "";
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            //string reportPath;
            string reportStr = "";
            //StreamWriter sw;
           
            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
          
            if (UC_InwardHeader1.RecordNo != "" && UC_InwardHeader1.ReferenceNo != "")
            {
                reportStr = rpt.getDetailReportCR(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo),false);
            }

            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("CoreInward", reportStr);
        }
        bool buttonClicked = false;
        protected string getDetailReportCR()
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
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Compressive Strength Of Concrete Core/s- Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Compressive Strength Of Concrete Core/s- Inward Form</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";



          
            var b = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo) ,null, "CR", null, null);

            foreach (var nt in b)
            {
                if (buttonClicked == true)
                {
                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CR" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CR" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
                        "<td height=19><font size=2>" + "CR" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "CR" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Date of Casting </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Grade of Concrete </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";

            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("CR", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CRINWD_Description_var + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CRINWD_CastingDate_dt.ToString() + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CRINWD_Grade_var.ToString() + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CRINWD_Quantity_tint.ToString() + "</font></td>";

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
                reportStr = rpt.getDetailReportCR(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo),true);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Core_Inward", reportStr);
        }
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
            //ProformaInvoiceUpdation Proinv = new ProformaInvoiceUpdation();
            //Proinv.getProformaInvoicePrintString(UC_InwardHeader1.ProformaInvoiceNo, "Print");
            PrintPDFReport obj = new PrintPDFReport();
            obj.Bill_PDFPrint(UC_InwardHeader1.BillNo, false, "print");
        }
        //protected void imgClosePopup_Click(object sender, EventArgs e)
        //{
        //    if (Session["InwardStatus"].ToString() == "Add")
        //        Response.Redirect("Enquiry_List.aspx");
        //    else if (Session["InwardStatus"].ToString() == "Edit")
        //        Response.Redirect("Frm_InwardStatus.aspx");
        //}
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
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "CR")));
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
