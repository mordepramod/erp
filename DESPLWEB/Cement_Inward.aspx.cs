using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Cement_Inward : System.Web.UI.Page
    {
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
                if (lblheading != null)
                {
                    lblheading.Text = "Cement Inward";
                }
                LoadSupplierList();
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    AddRowCementInward();

                }
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    DisplayCementTest();
                }
                GridTest();
                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowCementInward();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "CEMT";
                }
                lnkSave.Visible = true;
            }

        }
        protected void LoadSupplierList()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("SUPPL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SUPPL_Name_var", typeof(string)));
            var suppl = dc.Supplier_View("");
            foreach (var supp in suppl)
            {
                dr = dt.NewRow();
                dr["SUPPL_Id"] = supp.SUPPL_Id;
                dr["SUPPL_Name_var"] = supp.SUPPL_Name_var;
                dt.Rows.Add(dr);
            }
            ViewState["SupplierTable"] = dt;
        }

        protected void grdCementInward_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlSupplier = (DropDownList)e.Row.FindControl("ddlSupplier");
                ddlSupplier.DataSource = ViewState["SupplierTable"];
                ddlSupplier.DataTextField = "SUPPL_Name_var";
                ddlSupplier.DataValueField = "SUPPL_Id";
                ddlSupplier.DataBind();
                ddlSupplier.Items.Insert(0, new ListItem("---Select---", "0"));
            }
        }
        protected void AddRowCementInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            if (ViewState["CementInwardTable"] != null)
            {
                GetCurrentDataCementInward();
                dt = (DataTable)ViewState["CementInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCementName", typeof(string)));
                dt.Columns.Add(new DataColumn("txtQty", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));
                dt.Columns.Add(new DataColumn("txtTestDetails", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtCementName"] = string.Empty;
            dr["txtQty"] = string.Empty;
            dr["txtDescription"] = string.Empty;
            dr["ddlSupplier"] = string.Empty;
            dr["txtTestDetails"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["CementInwardTable"] = dt;
            grdCementInward.DataSource = dt;
            grdCementInward.DataBind();
            SetPreviousCementInward();

        }
        protected void DeleteRowCementInward(int rowIndex)
        {
            GetCurrentDataCementInward();
            DataTable dt = ViewState["CementInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CementInwardTable"] = dt;
            grdCementInward.DataSource = dt;
            grdCementInward.DataBind();
            SetPreviousCementInward();
        }
        protected void SetPreviousCementInward()
        {
            DataTable dt = (DataTable)ViewState["CementInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtCementName = (TextBox)grdCementInward.Rows[i].Cells[1].FindControl("txtCementName");
                TextBox txtQty = (TextBox)grdCementInward.Rows[i].Cells[2].FindControl("txtQty");
                TextBox txtDescription = (TextBox)grdCementInward.Rows[i].Cells[3].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdCementInward.Rows[i].Cells[4].FindControl("ddlSupplier");
                TextBox txtTestDetails = (TextBox)grdCementInward.Rows[i].Cells[5].FindControl("txtTestDetails");

                grdCementInward.Rows[i].Cells[0].Text = (i + 1).ToString();
                txtCementName.Text = dt.Rows[i]["txtCementName"].ToString();
                txtQty.Text = dt.Rows[i]["txtQty"].ToString();
                txtDescription.Text = dt.Rows[i]["txtDescription"].ToString();
                ddlSupplier.SelectedValue = dt.Rows[i]["ddlSupplier"].ToString();
                txtTestDetails.Text = dt.Rows[i]["txtTestDetails"].ToString();
            }
        }
        protected void GetCurrentDataCementInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtCementName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtQty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtTestDetails", typeof(string)));
            for (int i = 0; i < grdCementInward.Rows.Count; i++)
            {
                TextBox txtCementName = (TextBox)grdCementInward.Rows[i].Cells[1].FindControl("txtCementName");
                TextBox txtQty = (TextBox)grdCementInward.Rows[i].Cells[2].FindControl("txtQty");
                TextBox txtDescription = (TextBox)grdCementInward.Rows[i].Cells[3].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdCementInward.Rows[i].Cells[4].FindControl("ddlSupplier");
                TextBox txtTestDetails = (TextBox)grdCementInward.Rows[i].Cells[5].FindControl("txtTestDetails");
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txtCementName"] = txtCementName.Text;
                drRow["txtQty"] = txtQty.Text;
                drRow["txtDescription"] = txtDescription.Text;
                drRow["ddlSupplier"] = ddlSupplier.SelectedValue;
                drRow["txtTestDetails"] = txtTestDetails.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CementInwardTable"] = dtTable;

        }
        protected void TextChanged(object sender, EventArgs e)
        {
            TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
            if (txtSubsets.Text != "")
            {
                if (Convert.ToInt32(txtSubsets.Text) < grdCementInward.Rows.Count)
                {
                    for (int i = grdCementInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdCementInward.Rows.Count > 1)
                        {
                            DeleteRowCementInward(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdCementInward.Rows.Count)
                {
                    for (int i = grdCementInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
                    {
                        AddRowCementInward();
                    }
                }
            }
        }
        protected void Click(object sender, EventArgs e)
        {
            ViewState["CementInwardTable"] = null;
             if (UC_InwardHeader1.EnquiryNo != "" && UC_InwardHeader1.EnqNoApp != "" && UC_InwardHeader1.EnqNoApp != "0")
                gridAppData();
            else
                AddRowCementInward();
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
        public void gridAppData()
        {
            int i = 0, qty = 0;
            string testName = "";

            var res1 = dc.TestRequestDetails_View_Inward(Convert.ToInt32(UC_InwardHeader1.EnqNoApp), Convert.ToInt32(UC_InwardHeader1.MaterialId), 0, "CEMT").ToList();
            foreach (var n in res1)
            {
                testName = "";
                AddRowCementInward();
                TextBox txtCementName = (TextBox)grdCementInward.Rows[i].Cells[1].FindControl("txtCementName");
                TextBox txtQty = (TextBox)grdCementInward.Rows[i].Cells[2].FindControl("txtQty");
                TextBox txtDescription = (TextBox)grdCementInward.Rows[i].Cells[3].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdCementInward.Rows[i].Cells[4].FindControl("ddlSupplier");
                TextBox txtTestDetails = (TextBox)grdCementInward.Rows[i].Cells[5].FindControl("txtTestDetails");

                txtCementName.Text = Convert.ToString(n.Idmark1);
                txtQty.Text = n.specimen.ToString();
                if (n.specimen != "" && n.specimen != null)
                    qty += Convert.ToInt32(n.specimen);
                if (ddlSupplier.Items.FindByText(Convert.ToString(n.supplier)) != null)
                    ddlSupplier.Items.FindByText(Convert.ToString(n.supplier)).Selected = true;
                else
                {
                    int suppId = dc.Supplier_Update(n.supplier, true);
                    for (int j = 0; j < grdCementInward.Rows.Count; j++)
                    {
                        DropDownList ddlSupplier1 = (DropDownList)grdCementInward.Rows[j].FindControl("ddlSupplier");
                        ddlSupplier1.Items.Add(new ListItem(n.supplier, suppId.ToString()));
                    }
                    ddlSupplier.Items.FindByText(n.supplier).Selected = true;
                }
                txtDescription.Text = Convert.ToString(n.description);
                if (n.make != null && n.make != "")
                    txtDescription.Text += ", Make - " + n.make.ToString();
                var test = dc.TestRequestDetails_View_ForPrint(n.TestReqId);
                foreach (var t in test)
                {
                    testName += t.test_name+",";
                }
                txtTestDetails.Text = testName;


                i++;
            }
            int count = grdCementInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.TotalQty = qty.ToString();
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
                string totalCost = "0";
                clsData clsObj = new clsData();
                int SrNo;
                //DateTime d1 = new DateTime();
                //DateTime ReceivedDate = Convert.ToDateTime(DateTime.Now.Date.ToString());
                DateTime ReceivedDate = DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null);
                DateTime d1 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "CEMT", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, true);
                    var res = dc.AllInward_View("CEMT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
                    foreach (var a1 in res)
                    {
                        dc.CementTest_Update(a1.CEMTINWD_ReferenceNo_var.ToString(), 0, 0, 0, true);
                    }
                }
                else
                {
                    Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    NewrecNo = clsObj.GetnUpdateRecordNo("CEMT");
                    UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                    clsObj.insertRecordTable(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), NewrecNo, "CEMT");
                    UC_InwardHeader1.ReferenceNo = clsObj.getRefNo(Convert.ToInt32(UC_InwardHeader1.EnquiryNo)).ToString();
                    dc.Inward_Update_New(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "CEMT", 0, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate);

                    //NewrecNo = clsObj.GetnUpdateRecordNo("CEMT");
                    //UC_InwardHeader1.RecordNo = NewrecNo.ToString(); 

                    //UC_InwardHeader1.ReferenceNo = dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "CEMT", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, false).ToString();
                    ////var inward = dc.Inward_View(Convert.ToInt32(UC_InwardHeader1.ReferenceNo), 0, "");
                    ////foreach (var inwd in inward)
                    ////{
                    ////    UC_InwardHeader1.RecordNo = inwd.INWD_RecordNo_int.ToString();
                    ////}
                }
                dc.Enquiry_Update_Status(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 2);
                dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "CEMT", "", null, null, false, false, false, false, false, false, true);//Delete MIS Record
                dc.ClientSupplier_Update(Convert.ToInt32(UC_InwardHeader1.ClientId), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, null, true);

                dc.CementInward_Update("CEMT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "", "", 0, 0, "", "", 0, "", "", "", null, null, "", "", 0, 0, true);
                if (lblRptClientId.Text != "" && lblRptClientId.Text != "0")
                {
                    dc.Inward_Update_ReportClient(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, Convert.ToInt32(lblRptClientId.Text), Convert.ToInt32(lblRptSiteId.Text));
                }
                for (int i = 0; i <= grdCementInward.Rows.Count - 1; i++)
                {
                    TextBox txtCementName = (TextBox)grdCementInward.Rows[i].Cells[1].FindControl("txtCementName");
                    TextBox txtQty = (TextBox)grdCementInward.Rows[i].Cells[2].FindControl("txtQty");
                    TextBox txtDescription = (TextBox)grdCementInward.Rows[i].Cells[3].FindControl("txtDescription");
                    DropDownList ddlSupplier = (DropDownList)grdCementInward.Rows[i].Cells[4].FindControl("ddlSupplier");
                    TextBox txtTestDetails = (TextBox)grdCementInward.Rows[i].Cells[5].FindControl("txtTestDetails");

                    SrNo = Convert.ToInt32(grdCementInward.Rows[i].Cells[0].Text);
                    RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;

                    dc.CementInward_Update("CEMT", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txtQty.Text), txtDescription.Text, ddlSupplier.SelectedItem.Text, 0, "", txtCementName.Text, "", d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), false);
                    dc.ClientSupplier_Update(Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(ddlSupplier.SelectedValue), Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, ReceivedDate, false);
                    dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "CEMT", RefNo, "CEMT", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);

                    string[] lines = txtTestDetails.Text.Split(',');
                    foreach (string line in lines)
                    {
                        var s = dc.Test(0, "", 0, "CEMT", line, 0);
                        foreach (var n in s)
                        {
                            var t = dc.Test(Convert.ToInt32(n.TEST_Sr_No), "", 0, "CEMT", "", 0);
                            foreach (var ts in t)
                            {
                                dc.CementTest_Update(RefNo, Convert.ToInt32(ts.TEST_Id), 0, Convert.ToByte(ts.TEST_Sr_No), false);
                            }
                        }

                        string[] line1 = line.Split('|');
                        foreach (string line2 in line1)
                        {
                            if (line2 == "Compressive Strength")
                            {
                                string[] line3 = line.Split('|');
                                foreach (string line4 in line3)
                                {
                                    var s1 = dc.Test(0, "", 0, "CEMT", line4, 0);
                                    foreach (var n in s1)
                                    {
                                        var t = dc.Test(Convert.ToInt32(n.TEST_Sr_No), "", 0, "CEMT", "", 0);
                                        foreach (var ts in t)
                                        {
                                            int TestId = Convert.ToInt32(ts.TEST_Id);
                                            string[] line5 = line.Split('|');
                                            foreach (string line6 in line5)
                                            {
                                                if (line6 != "Compressive Strength")
                                                {
                                                    if (line6.ToString() != "")
                                                    {
                                                        dc.CementTest_Update(RefNo, TestId, Convert.ToByte(line6.ToString()), Convert.ToByte(n.TEST_Sr_No), false);
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
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
                        BillNo = bill.UpdateBill("CEMT", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
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
                //        ProformaInvoiceNo = ProInv.UpdateProformaInvoice("CEMT", Convert.ToInt32(UC_InwardHeader1.RecordNo), ProformaInvoiceNo);
                //        totalCost = clsObj.getProformaBillNetAmount(ProformaInvoiceNo, 1);
                //    }
                //    UC_InwardHeader1.ProformaInvoiceNo = ProformaInvoiceNo.ToString();
                //    //
                //    lnkBillPrint.Visible = true;
                //}

                //sms
                if (UC_InwardHeader1.InwdStatus != "Edit")
                {
                    clsObj.sendInwardReportMsg(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), UC_InwardHeader1.EnqDate, UC_InwardHeader1.SiteMonthlyStatus, UC_InwardHeader1.ClCreditLimitExcededStatus, totalCost,"", UC_InwardHeader1.ContactNo.ToString(), "Inward");
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
                for (int i = 0; i <= grdCementInward.Rows.Count - 1; i++)
                {
                    TextBox txtCementName = (TextBox)grdCementInward.Rows[i].Cells[1].FindControl("txtCementName");
                    TextBox txtQty = (TextBox)grdCementInward.Rows[i].Cells[2].FindControl("txtQty");
                    TextBox txtDescription = (TextBox)grdCementInward.Rows[i].Cells[3].FindControl("txtDescription");
                    DropDownList ddlSupplier = (DropDownList)grdCementInward.Rows[i].Cells[4].FindControl("ddlSupplier");
                    TextBox txtTestDetails = (TextBox)grdCementInward.Rows[i].Cells[5].FindControl("txtTestDetails");
                    if (txtCementName.Text == "")
                    {
                        lblMsg.Text = "Enter Cement Name for Sr No. " + (i + 1) + ".";
                        txtCementName.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtQty.Text == "")
                    {
                        lblMsg.Text = "Enter Quantity for Sr No. " + (i + 1) + ".";
                        txtQty.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtDescription.Text == "")
                    {
                        lblMsg.Text = "Enter Description for Sr No. " + (i + 1) + ".";
                        txtDescription.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddlSupplier.SelectedItem.Text == "---Select---")
                    {
                        lblMsg.Text = "Select Supplier Name for Sr No. " + (i + 1) + ".";
                        ddlSupplier.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtTestDetails.Text == "")
                    {
                        lblMsg.Text = "Select Test Details for Sr No. " + (i + 1) + ".";
                        txtTestDetails.Focus();
                        valid = false;
                        break;
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
        public void DisplayCementTest()
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
            CementTestgrid();
        }
        public void CementTestgrid()
        {
            int i = 0;
            var res = dc.AllInward_View("CEMT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var w in res)
            {
                AddRowCementInward();
                TextBox txtCementName = (TextBox)grdCementInward.Rows[i].Cells[1].FindControl("txtCementName");
                TextBox txtQty = (TextBox)grdCementInward.Rows[i].Cells[2].FindControl("txtQty");
                txtCementName.Text = w.CEMTINWD_CementName_var.ToString();
                txtQty.Text = w.CEMTINWD_Quantity_tint.ToString();

                i++;
            }
            Testgrid();
        }
        public void Testgrid()
        {

            int i = 0;
            var res = dc.AllInward_View("CEMT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var w in res)
            {
                TextBox txtDescription = (TextBox)grdCementInward.Rows[i].Cells[3].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdCementInward.Rows[i].Cells[4].FindControl("ddlSupplier");
                Boolean valid = false;
                string ReferenceNo = w.CEMTINWD_ReferenceNo_var.ToString();
                var wttest = dc.AllInward_View("CEMT", 0, ReferenceNo);
                foreach (var wt in wttest)
                {
                    var c = dc.Test_View(0, Convert.ToInt32(wt.CEMTTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var n in c)
                    {
                        string TEST_Name_var = n.TEST_Name_var.ToString();
                        TextBox txtTestDetails = (TextBox)grdCementInward.Rows[i].Cells[5].FindControl("txtTestDetails");
                        if (TEST_Name_var == "Compressive Strength")
                        {
                            if (wt.CEMTTEST_Days_tint.ToString() != "" && wt.CEMTTEST_Days_tint.ToString() != null && wt.CEMTTEST_Days_tint.ToString() != "0")
                            {
                                txtTestDetails.Text = txtTestDetails.Text + TEST_Name_var.ToString() + "|" + wt.CEMTTEST_Days_tint.ToString() + ",";
                                valid = true;
                                break;
                            }
                        }
                        if (valid == false)
                        {
                            txtTestDetails.Text = txtTestDetails.Text + TEST_Name_var + ",";
                        }
                    }

                }
                txtDescription.Text = w.CEMTINWD_Description_var.ToString();
                if (ddlSupplier.Items.FindByText(w.CEMTINWD_SupplierName_var) != null)
                {
                    //ddlSupplier.Items.FindByText(w.CEMTINWD_SupplierName_var).Selected = true;
                    ddlSupplier.SelectedValue = ddlSupplier.Items.FindByText(w.CEMTINWD_SupplierName_var).Value;
                }
                i++;
            }
            int count = grdCementInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.EnquiryNo = "";
        }
        protected void lnkRateList_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender1.Show();
            gridClear();
            string Test = "";
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            TextBox txtTestDetails = (TextBox)clickedRow.FindControl("txtTestDetails");
            if (txtTestDetails.Text != "")
            {
                for (int i = 0; i < grdTestDetails.Rows.Count; i++)
                {
                    TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                    CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                    Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                    bool valid = false;
                    int count = 0;
                    string[] lines = txtTestDetails.Text.Split(',');
                    foreach (string line in lines)
                    {
                        count++;
                        Boolean Founder = false;
                        string[] Testline = Test.Split(',');
                        foreach (string tstline in Testline)
                        {
                            if (count.ToString() == tstline.ToString())
                            {
                                Founder = true;
                                break;
                            }
                        }
                        if (Founder == false)
                        {
                            string[] line1 = line.Split('|');
                            foreach (string line2 in line1)
                            {
                                if (line2 == lblTest.Text)
                                {
                                    string[] line3 = line.Split('|');
                                    foreach (string line4 in line3)
                                    {
                                        if (line4 != "Compressive Strength")
                                        {
                                            txtDAYS.Text = line4.ToString();
                                            Test = Test + count + ",";
                                            cbxSelect.Checked = true;
                                            valid = true;
                                        }
                                    }
                                }
                            }
                        }
                        if (line == lblTest.Text)
                        {
                            cbxSelect.Checked = true;
                            break;
                        }
                        if (valid == true)
                        {
                            AddRowCement();
                            CountRow();
                            DeleteRow();
                            CountRowChk();
                            break;
                        }
                    }
                }
            }
        }

        public void gridClear()
        {
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                cbxSelect.Checked = false;
                txtDAYS.Text = "";
            }
        }
        protected void imgCloseTestDetails_Click(object sender, ImageClickEventArgs e)
        {
            lbl_ErrMsg.Visible = false;
            if (ValidateCheck() == true && ValidateChkDays() == false && ValidateCheckSoundness() == true)
            {
                ValidateInitial_Final();
                TextBox txtTestDetails = grdCementInward.SelectedRow.Cells[5].FindControl("txtTestDetails") as TextBox;
                txtTestDetails.Text = string.Empty;
                for (int i = 0; i < grdTestDetails.Rows.Count; i++)
                {
                    TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                    CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                    Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                    if (cbxSelect.Checked)
                    {
                        if (txtDAYS.Text != "" && txtDAYS.Visible == true)
                        {
                            txtTestDetails.Text = txtTestDetails.Text + lblTest.Text + "|" + txtDAYS.Text + ",";
                        }
                        else
                        {
                            txtTestDetails.Text = txtTestDetails.Text + lblTest.Text + ",";
                        }
                    }
                }
                ModalPopupExtender1.Hide();
            }
        }
        protected Boolean ValidateChkDays()
        {
            Boolean valid = false;
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                if (cbxSelect.Checked == true && lblTest.Text == "Compressive Strength")
                {
                    if (txtDAYS.Text == "")
                    {
                        valid = true;
                        break;
                    }
                }
            }
            if (valid == true)
            {
                lbl_ErrMsg.Visible = true;
                lbl_ErrMsg.Text = "Please Enter Days of Compressive Strength Otherwise Uncheck it ";
                //  ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Please Enter Days of Compressive Strength' + '\\n' + 'Otherwise'+ '\\n' + 'Uncheck it ');", true);
            }
            else
            {
                lbl_ErrMsg.Visible = false;
            }
            return valid;
        }
        protected Boolean ValidateCheckSoundness()
        {
            Boolean valid = true;
            int j = 0;
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");


                if (lblTest.Text == "Soundness By Le-Chateliers Apparatus")
                {
                    if (cbxSelect.Checked == true)
                    {
                        valid = false;
                        j++;
                    }
                }
                if (lblTest.Text == "Soundness By AutoClave")
                {
                    if (cbxSelect.Checked == true)
                    {
                        valid = false;
                        j++;
                    }
                }
                //if (lblTest.Text == "Initial Setting" || lblTest.Text == "final Setting")
                //{
                //    cbxSelect.Checked = true;
                //}
            }
            if (j == 2)
            {
                if (valid == false)
                {
                    lbl_ErrMsg.Visible = true;
                    lbl_ErrMsg.Text = "Please Select either Soundness By Le-Chateliers Apparatus or  Soundness By Autoclave ";
                    // ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Please Select either Soundness By Le-Chateliers Apparatus' + '\\n' + 'or'+ '\\n' + 'Soundness By Autoclave');", true);
                }
                else
                {
                    lbl_ErrMsg.Visible = false;
                }
            }
            else
            {
                valid = true;
            }
            return valid;

        }
        protected Boolean ValidateCheck()
        {
            Boolean valid = true;
            int j = 0;
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");


                if (lblTest.Text == "Fineness By Dry Seiving")
                {
                    if (cbxSelect.Checked == true)
                    {
                        valid = false;
                        j++;
                    }
                }
                if (lblTest.Text == "Fineness By Blain`s Air Permeability Method")
                {
                    if (cbxSelect.Checked == true)
                    {
                        valid = false;
                        j++;
                    }
                }
            }
            if (j == 2)
            {
                if (valid == false)
                {
                    lbl_ErrMsg.Visible = true;
                    lbl_ErrMsg.Text = "Please Select either Fineness By Dry Seiving or Fineness By Blain`s Air Permeability Method";
                    // ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Please Select either Fineness By Dry Seiving' + '\\n' + 'or'+ '\\n' + 'Fineness By Blain`s Air Permeability Method');", true);
                }
                else
                {
                    lbl_ErrMsg.Visible = false;
                }
            }
            else
            {
                valid = true;
            }
            return valid;

        }
        protected void ValidateInitial_Final()
        {
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");

                if (lblTest.Text == "Initial Setting")
                {
                    if (cbxSelect.Checked == true)
                    {
                        if (i > 0)
                        {
                            CheckBox cbxSelectt = (CheckBox)grdTestDetails.Rows[i + 1].Cells[2].FindControl("cbxSelect");
                            cbxSelectt.Checked = true;
                        }
                    }
                }
                if (lblTest.Text == "Final Setting")
                {
                    if (cbxSelect.Checked == true)
                    {
                        if (i > 0)
                        {
                            CheckBox cbxSelectt = (CheckBox)grdTestDetails.Rows[i - 1].Cells[2].FindControl("cbxSelect");
                            cbxSelectt.Checked = true;
                        }
                    }
                }
            }
        }
        protected void GridTestDetailsBind()
        {
            int MaterialId = 0;
            var InwardId = dc.Material_View("CEMT", "");
            foreach (var n in InwardId)
            {
                MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
            }
            var a = dc.Test_View(MaterialId, 0, "", 0, 0, 0);
            grdTestDetails.DataSource = a;
            grdTestDetails.DataBind();
            if (grdTestDetails.Rows.Count <= 0)
            {

            }
            else
            {
                GridTest();
            }
        }
        protected void grdTestDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Add"))
            {
                AddRowCement();
                CountRow();
            }

        }

        protected void lnkDeleterow_Click(object sender, CommandEventArgs e)
        {
            if (grdTestDetails.Rows.Count > 1)
            {
                LinkButton btn = (LinkButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                for (int i = 0; i < grdTestDetails.Rows.Count - 1; i++)
                {
                    LinkButton lnkDeleterow = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkDeleterow");
                    if (lnkDeleterow.Visible == true)
                    {
                        DeleteRowCemenTest(gvr.RowIndex);
                        CountRow();
                        break;
                    }
                }
            }
        }
        public void DeleteRow()
        {
            if (grdTestDetails.Rows.Count > 1)
            {
                for (int i = 0; i < grdTestDetails.Rows.Count - 1; i++)
                {
                    TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                    LinkButton lnkDeleterow = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkDeleterow");
                    if (lnkDeleterow.Visible == true && txtDAYS.Text == "")
                    {
                        DeleteRowCemenTest(i);
                        CountRow();
                    }
                }
            }
        }

        public void GridTest()
        {
            int MaterialId = 0;
            var InwardId = dc.Material_View("CEMT", "");
            foreach (var n in InwardId)
            {
                MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
            }
            int i = 0;
            var a = dc.Test_View(MaterialId, 0, "", 0, 0, 0);
            foreach (var cemtTest in a)
            {
                AddRowCement();
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                Label lblDays = (Label)grdTestDetails.Rows[i].Cells[1].FindControl("lblDays");
                TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[3].FindControl("cbxSelect");
                LinkButton lnkAddrow = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkAddrow");
                lblTest.Text = cemtTest.TEST_Name_var.ToString();
                i++;
            }
            CountRow();

        }
        public void CountRowChk()
        {
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[3].FindControl("cbxSelect");
                if (txtDAYS.Text != "")
                {
                    cbxSelect.Checked = true;
                }
            }
        }
        public void CountRow()
        {
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                Label lblDays = (Label)grdTestDetails.Rows[i].Cells[1].FindControl("lblDays");
                TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[3].FindControl("cbxSelect");
                LinkButton lnkAddrow = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkAddrow");
                LinkButton lnkDeleterow = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkDeleterow");
                if (i == grdTestDetails.Rows.Count - 1 || lblTest.Text == "Compressive Strength")
                {
                    lblDays.Visible = true;
                    txtDAYS.Visible = true;
                    lnkAddrow.Text = "+";
                    lnkAddrow.Font.Bold = true;
                    lnkAddrow.Font.Size = 14;
                    lnkDeleterow.Text = "X";
                    lnkDeleterow.Font.Bold = true;
                    lnkDeleterow.Font.Size = 10;
                    lnkAddrow.Visible = true;
                    lnkDeleterow.Visible = true;
                    lblTest.Text = "Compressive Strength";
                }
                else
                {
                    lblDays.Visible = false;
                    txtDAYS.Visible = false;
                    lnkAddrow.Visible = false;
                    lnkDeleterow.Visible = false;
                }

            }
        }
        protected void AddRowCement()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            if (ViewState["TestTable"] != null)
            {
                GetCurrentDataCementTest();
                dt = (DataTable)ViewState["TestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblTest", typeof(string)));
                dt.Columns.Add(new DataColumn("lblDays", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDAYS", typeof(string)));
                dt.Columns.Add(new DataColumn("lnkAddrow", typeof(string)));
                dt.Columns.Add(new DataColumn("lnkDeleterow", typeof(string)));
                dt.Columns.Add(new DataColumn("lblTestId", typeof(string)));
                dt.Columns.Add(new DataColumn("cbxSelect", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblTest"] = string.Empty;
            dr["lblDays"] = "Days";
            dr["txtDAYS"] = string.Empty;
            dr["lnkAddrow"] = string.Empty;
            dr["lnkDeleterow"] = string.Empty;
            dr["lblTestId"] = string.Empty;
            dr["cbxSelect"] = false;

            dt.Rows.Add(dr);
            ViewState["TestTable"] = dt;
            grdTestDetails.DataSource = dt;
            grdTestDetails.DataBind();
            SetPreviousCementTest();

        }
        protected void GetCurrentDataCementTest()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow drRow = null;
            dt.Columns.Add(new DataColumn("lblTest", typeof(string)));
            dt.Columns.Add(new DataColumn("lblDays", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDAYS", typeof(string)));
            dt.Columns.Add(new DataColumn("lnkAddrow", typeof(string)));
            dt.Columns.Add(new DataColumn("lnkDeleterow", typeof(string)));
            dt.Columns.Add(new DataColumn("lblTestId", typeof(string)));
            dt.Columns.Add(new DataColumn("cbxSelect", typeof(string)));

            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                {
                    Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                    Label lblTestId = (Label)grdTestDetails.Rows[i].Cells[2].FindControl("lblTestId");
                    Label lblDays = (Label)grdTestDetails.Rows[i].Cells[1].FindControl("lblDays");
                    TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                    LinkButton lnkAddrow = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkAddrow");
                    LinkButton lnkDeleterow = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkDeleterow");
                    CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");

                    drRow = dt.NewRow();
                    drRow["lblTest"] = lblTest.Text;
                    drRow["lblDays"] = "Days";
                    drRow["txtDAYS"] = txtDAYS.Text;
                    drRow["lnkAddrow"] = lnkAddrow.Text;
                    drRow["lnkDeleterow"] = lnkDeleterow.Text;
                    drRow["cbxSelect"] = cbxSelect.Checked;
                    dt.Rows.Add(drRow);
                    rowIndex++;
                }
                ViewState["TestTable"] = dt;
            }
        }
        protected void SetPreviousCementTest()
        {
            DataTable dt = (DataTable)ViewState["TestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                Label lblTestId = (Label)grdTestDetails.Rows[i].Cells[2].FindControl("lblTestId");
                Label lblDays = (Label)grdTestDetails.Rows[i].Cells[1].FindControl("lblDays");
                TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                LinkButton lnkAddrow = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkAddrow");
                LinkButton lnkDeleterow = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkDeleterow");
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                grdTestDetails.Rows[i].Cells[2].Text = (i + 1).ToString();

                lblTest.Text = dt.Rows[i]["lblTest"].ToString();
                lblTestId.Text = dt.Rows[i]["lblTestId"].ToString();
                lblDays.Text = dt.Rows[i]["lblDays"].ToString();
                txtDAYS.Text = dt.Rows[i]["txtDAYS"].ToString();
                lnkAddrow.Text = dt.Rows[i]["lnkAddrow"].ToString();
                lnkDeleterow.Text = dt.Rows[i]["lnkDeleterow"].ToString();
                cbxSelect.Checked = Convert.ToBoolean(dt.Rows[i]["cbxSelect"].ToString());
            }

        }

        protected void DeleteRowCemenTest(int rowIndex)
        {
            GetCurrentDataCementTest();
            DataTable dt = ViewState["TestTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["TestTable"] = dt;
            grdTestDetails.DataSource = dt;
            grdTestDetails.DataBind();
            SetPreviousCementTest();
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
                reportStr = rpt.getDetailReportCementInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Cement_Inward", reportStr);
        }
        bool buttonClicked = false;
        protected string getDetailReportCementInward()
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
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Cement Test Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Cement Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


            var b = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, "CEMT", null, null);

            foreach (var nt in b)
            {
                if (buttonClicked == true)
                {
                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CEMT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CEMT" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
                        "<td height=19><font size=2>" + "CEMT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "CEMT" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Cement Name</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Qty </b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Test </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Test Done As Per</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";

            int i = 0;
            var n = dc.AllInward_View("CEMT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CEMTINWD_CementName_var + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CEMTINWD_Quantity_tint.ToString() + "</font></td>";

                mySql += "<td >";
                mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";

                var wttest = dc.AllInward_View("CEMT", 0, c.CEMTINWD_ReferenceNo_var.ToString());
                foreach (var wt in wttest)
                {
                    mySql += "<tr>";
                    var c1 = dc.Test_View(0, Convert.ToInt32(wt.CEMTTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var n2 in c1)
                    {
                        string TEST_Name_var = n2.TEST_Name_var.ToString();
                        if (TEST_Name_var == "Compressive Strength")
                        {
                            if (wt.CEMTTEST_Days_tint.ToString() != "" && wt.CEMTTEST_Days_tint.ToString() != null && wt.CEMTTEST_Days_tint.ToString() != "0")
                            {
                                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + wt.CEMTTEST_Days_tint.ToString() + " " + "Days" + " " + TEST_Name_var + "</font></td>";
                            }
                        }
                        else
                        {
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + n2.TEST_Name_var.ToString() + "</font></td>";
                        }
                        mySql += "</tr>";
                    }
                }

                mySql += "</table>";
                mySql += "</td>";

                mySql += "<td>";
                mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "IS:4031(PartIV)-1988)" + "</font></td>";
                mySql += "</tr>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "IS:4031(PartIV)-1988)" + "</font></td>";
                mySql += "</tr>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "IS:4031(PartIV)-1988)" + "</font></td>";
                mySql += "</tr>";
                mySql += "</table>";
                mySql += "</td>";




                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CEMTINWD_Description_var + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CEMTINWD_SupplierName_var + "</font></td>";

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
                reportStr = rpt.getDetailReportCementInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), true);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Cement_LabSheet", reportStr);
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
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "CEMT")));
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