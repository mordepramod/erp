using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class STC_Inward : System.Web.UI.Page
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
                lblheading.Text = "Steel Chemical Inward";
                LoadSupplierList();
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    AddRowSteelChemicalInward();
                }
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    getSteelChemicalInwardTesting();
                }
                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowSteelChemicalInward();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "STC";
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

        protected void grdSteelChemicalInward_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void AddRowSteelChemicalInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["SteelChemicalInwardTable"] != null)
            {
                GetCurrentDataSteelChemicalInward();
                dt = (DataTable)ViewState["SteelChemicalInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDia", typeof(string)));
                dt.Columns.Add(new DataColumn("txtQty", typeof(string)));
                dt.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlTypeOfSteel", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlGradeOfSteel", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("chkSupplier", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));

            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtDia"] = string.Empty;
            dr["txtQty"] = string.Empty;
            dr["txtIdMark"] = string.Empty;
            dr["ddlTypeOfSteel"] = string.Empty;
            dr["ddlGradeOfSteel"] = string.Empty;
            dr["txtDescription"] = string.Empty;
            dr["chkSupplier"] = false;
            dr["ddlSupplier"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["SteelChemicalInwardTable"] = dt;
            grdSteelChemicalInward.DataSource = dt;
            grdSteelChemicalInward.DataBind();
            SetPreviousDataSteelChemicalInward();
        }
        protected void DeleteRowSteelChemicalInward(int rowIndex)
        {
            GetCurrentDataSteelChemicalInward();
            DataTable dt = ViewState["SteelChemicalInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["SteelChemicalInwardTable"] = dt;
            grdSteelChemicalInward.DataSource = dt;
            grdSteelChemicalInward.DataBind();
            SetPreviousDataSteelChemicalInward();
        }
        protected void SetPreviousDataSteelChemicalInward()
        {
            DataTable dt = (DataTable)ViewState["SteelChemicalInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtDia = (TextBox)grdSteelChemicalInward.Rows[i].Cells[1].FindControl("txtDia");
                TextBox txtQty = (TextBox)grdSteelChemicalInward.Rows[i].Cells[2].FindControl("txtQty");
                TextBox txtIdMark = (TextBox)grdSteelChemicalInward.Rows[i].Cells[3].FindControl("txtIdMark");
                DropDownList ddlTypeOfSteel = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[4].FindControl("ddlTypeOfSteel");
                DropDownList ddlGradeOfSteel = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[5].FindControl("ddlGradeOfSteel");
                TextBox txtDescription = (TextBox)grdSteelChemicalInward.Rows[i].Cells[6].FindControl("txtDescription");
                CheckBox chkSupplier = (CheckBox)grdSteelChemicalInward.Rows[i].Cells[7].FindControl("chkSupplier");
                DropDownList ddlSupplier = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[8].FindControl("ddlSupplier");

                grdSteelChemicalInward.Rows[i].Cells[0].Text = (i + 1).ToString();
                txtDia.Text = dt.Rows[i]["txtDia"].ToString();
                txtQty.Text = dt.Rows[i]["txtQty"].ToString();
                txtIdMark.Text = dt.Rows[i]["txtIdMark"].ToString();
                ddlTypeOfSteel.Text = dt.Rows[i]["ddlTypeOfSteel"].ToString();
                ddlGradeOfSteel.Text = dt.Rows[i]["ddlGradeOfSteel"].ToString();
                txtDescription.Text = dt.Rows[i]["txtDescription"].ToString();
                chkSupplier.Checked = Convert.ToBoolean(dt.Rows[i]["chkSupplier"].ToString());
                ddlSupplier.SelectedValue = dt.Rows[i]["ddlSupplier"].ToString();
            }
        }
        protected void GetCurrentDataSteelChemicalInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDia", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtQty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlTypeOfSteel", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlGradeOfSteel", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chkSupplier", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));

            for (int i = 0; i < grdSteelChemicalInward.Rows.Count; i++)
            {
                TextBox txtDia = (TextBox)grdSteelChemicalInward.Rows[i].Cells[1].FindControl("txtDia");
                TextBox txtQty = (TextBox)grdSteelChemicalInward.Rows[i].Cells[2].FindControl("txtQty");
                TextBox txtIdMark = (TextBox)grdSteelChemicalInward.Rows[i].Cells[3].FindControl("txtIdMark");
                DropDownList ddlTypeOfSteel = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[4].FindControl("ddlTypeOfSteel");
                DropDownList ddlGradeOfSteel = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[5].FindControl("ddlGradeOfSteel");
                TextBox txtDescription = (TextBox)grdSteelChemicalInward.Rows[i].Cells[6].FindControl("txtDescription");
                CheckBox chkSupplier = (CheckBox)grdSteelChemicalInward.Rows[i].Cells[7].FindControl("chkSupplier");
                DropDownList ddlSupplier = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[8].FindControl("ddlSupplier");
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txtDia"] = txtDia.Text;
                drRow["txtQty"] = txtQty.Text;
                drRow["txtIdMark"] = txtIdMark.Text;
                drRow["ddlTypeOfSteel"] = ddlTypeOfSteel.Text;
                drRow["ddlGradeOfSteel"] = ddlGradeOfSteel.Text;
                drRow["txtDescription"] = txtDescription.Text;
                drRow["chkSupplier"] = chkSupplier.Checked;
                drRow["ddlSupplier"] = ddlSupplier.SelectedValue;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["SteelChemicalInwardTable"] = dtTable;
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
                bool Supplier;
                //DateTime ReceivedDate = Convert.ToDateTime(DateTime.Now.Date.ToString());
                DateTime ReceivedDate = DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null);
                DateTime d1 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "STC", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, true);
                }
                else
                {
                    Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    NewrecNo = clsObj.GetnUpdateRecordNo("STC");
                    UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                    clsObj.insertRecordTable(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), NewrecNo, "STC");
                    UC_InwardHeader1.ReferenceNo = clsObj.getRefNo(Convert.ToInt32(UC_InwardHeader1.EnquiryNo)).ToString();
                    dc.Inward_Update_New(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "STC", 0, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate);

                    //Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    //NewrecNo = clsObj.GetnUpdateRecordNo("STC");
                    //UC_InwardHeader1.RecordNo = NewrecNo.ToString(); 
                    //UC_InwardHeader1.ReferenceNo = dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "STC", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, false).ToString();
                    ////var inward = dc.Inward_View(Convert.ToInt32(UC_InwardHeader1.ReferenceNo), 0, "");
                    ////foreach (var inwd in inward)
                    ////{
                    ////    UC_InwardHeader1.RecordNo = inwd.INWD_RecordNo_int.ToString();
                    ////}
                }

                dc.Enquiry_Update_Status(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 2);
                dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "STC", "", null, null, false, false, false, false, false, false, true);//Delete MIS Record

                dc.SteelChemicalInward_Update("STC", Convert.ToInt32(UC_InwardHeader1.RecordNo), "", "", 0, 0, "", "", 0, "", "", "", 0, "", 0, false, null, null, "", "", 0, 0, true);
                dc.ClientSupplier_Update(Convert.ToInt32(UC_InwardHeader1.ClientId), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, null, true);

                if (lblRptClientId.Text != "" && lblRptClientId.Text != "0")
                {
                    dc.Inward_Update_ReportClient(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, Convert.ToInt32(lblRptClientId.Text), Convert.ToInt32(lblRptSiteId.Text));
                }
                string strSupplier = "";
                for (int i = 0; i <= grdSteelChemicalInward.Rows.Count - 1; i++)
                {
                    TextBox txtDia = (TextBox)grdSteelChemicalInward.Rows[i].Cells[1].FindControl("txtDia");
                    TextBox txtQty = (TextBox)grdSteelChemicalInward.Rows[i].Cells[2].FindControl("txtQty");
                    TextBox txtIdMark = (TextBox)grdSteelChemicalInward.Rows[i].Cells[3].FindControl("txtIdMark");
                    DropDownList ddlTypeOfSteel = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[4].FindControl("ddlTypeOfSteel");
                    DropDownList ddlGradeOfSteel = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[5].FindControl("ddlGradeOfSteel");
                    TextBox txtDescription = (TextBox)grdSteelChemicalInward.Rows[i].Cells[6].FindControl("txtDescription");
                    CheckBox chkSupplier = (CheckBox)grdSteelChemicalInward.Rows[i].Cells[7].FindControl("chkSupplier");
                    DropDownList ddlSupplier = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[8].FindControl("ddlSupplier");

                    SrNo = Convert.ToInt32(grdSteelChemicalInward.Rows[i].Cells[0].Text);
                    RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;

                    dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "STC", RefNo, "STC", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);
                    if (chkSupplier.Checked)
                    {
                        Supplier = true;
                        strSupplier = ddlSupplier.SelectedItem.Text;
                        dc.ClientSupplier_Update(Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(ddlSupplier.SelectedValue), Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, ReceivedDate, false);
                    }
                    else
                    {
                        strSupplier = "";
                        Supplier = false;
                    }
                    int TestId = 0;
                    var testid = dc.Test_View(0, 0, "STC", 0, 0, 0);
                    foreach (var testd in testid)
                    {
                        TestId = Convert.ToInt32(testd.TEST_Id);
                        break;
                    }
                    dc.SteelChemicalInward_Update("STC", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txtQty.Text), txtDescription.Text, strSupplier, 0, "", txtIdMark.Text, ddlGradeOfSteel.Text, Convert.ToByte(txtDia.Text), ddlTypeOfSteel.Text, TestId, Supplier, d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), false);
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
                        BillNo = bill.UpdateBill("STC", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
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
                //        ProformaInvoiceNo = ProInv.UpdateProformaInvoice("STC", Convert.ToInt32(UC_InwardHeader1.RecordNo), ProformaInvoiceNo);
                //        totalCost = clsObj.getProformaBillNetAmount(ProformaInvoiceNo,1);
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

        protected void TextChanged(object sender, EventArgs e)
        {
            TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
            if (txtSubsets.Text != "")
            {
                if (Convert.ToInt32(txtSubsets.Text) < grdSteelChemicalInward.Rows.Count)
                {
                    for (int i = grdSteelChemicalInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdSteelChemicalInward.Rows.Count > 1)
                        {
                            DeleteRowSteelChemicalInward(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdSteelChemicalInward.Rows.Count)
                {
                    for (int i = grdSteelChemicalInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
                    {
                        AddRowSteelChemicalInward();
                    }
                }
            }
        }
        protected void Click(object sender, EventArgs e)
        {
            ViewState["SteelChemicalInwardTable"] = null;
             if (UC_InwardHeader1.EnquiryNo != "" && UC_InwardHeader1.EnqNoApp != "" && UC_InwardHeader1.EnqNoApp != "0")
                gridAppData();
            else
                AddRowSteelChemicalInward();
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

        protected void chkSupplier_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((CheckBox)sender).NamingContainer as GridViewRow;
            CheckBox chkSupplier = (CheckBox)clickedRow.FindControl("chkSupplier");
            DropDownList ddlSupplier = (DropDownList)clickedRow.FindControl("ddlSupplier");
            if (chkSupplier.Checked)
            {
                ddlSupplier.Focus();
            }
            else
            {
                ddlSupplier.SelectedValue = "0";
            }
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
                for (int i = 0; i < grdSteelChemicalInward.Rows.Count; i++)
                {
                    TextBox txtDia = (TextBox)grdSteelChemicalInward.Rows[i].Cells[1].FindControl("txtDia");
                    TextBox txtQty = (TextBox)grdSteelChemicalInward.Rows[i].Cells[2].FindControl("txtQty");
                    TextBox txtIdMark = (TextBox)grdSteelChemicalInward.Rows[i].Cells[3].FindControl("txtIdMark");
                    DropDownList ddlTypeOfSteel = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[4].FindControl("ddlTypeOfSteel");
                    DropDownList ddlGradeOfSteel = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[5].FindControl("ddlGradeOfSteel");
                    TextBox txtDescription = (TextBox)grdSteelChemicalInward.Rows[i].Cells[6].FindControl("txtDescription");
                    CheckBox chkSupplier = (CheckBox)grdSteelChemicalInward.Rows[i].Cells[7].FindControl("chkSupplier");
                    DropDownList ddlSupplier = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[8].FindControl("ddlSupplier");
                    if (txtDia.Text == "")
                    {
                        lblMsg.Text = "Enter Dia. for Sr No " + (i + 1) + ".";
                        txtDia.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtQty.Text == "")
                    {
                        lblMsg.Text = "Enter Quantity for Sr No " + (i + 1) + ".";
                        txtQty.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtIdMark.Text == "")
                    {
                        lblMsg.Text = "Enter Id Mark for Sr No " + (i + 1) + ".";
                        txtIdMark.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddlTypeOfSteel.Text == "Select")
                    {
                        lblMsg.Text = "Select Type Of Steel for Sr No " + (i + 1) + ".";
                        ddlTypeOfSteel.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddlGradeOfSteel.Text == "Select")
                    {
                        lblMsg.Text = "Select Grade Of Steel for Sr No " + (i + 1) + ".";
                        ddlGradeOfSteel.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtDescription.Text == "")
                    {
                        lblMsg.Text = "Enter Description for Sr No " + (i + 1) + ".";
                        txtDescription.Focus();
                        valid = false;
                        break;
                    }
                    else if (chkSupplier.Checked && ddlSupplier.SelectedItem.Text == "---Select---")
                    {
                        lblMsg.Text = "Select Supplier Name for Sr No " + (i + 1) + ".";
                        ddlSupplier.Focus();
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
        public void getSteelChemicalInwardTesting()
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
            grid();
        }
        public void grid()
        {
            int i = 0;
            var res = dc.AllInward_View("STC", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var a in res)
            {
                AddRowSteelChemicalInward();
                TextBox txtDia = (TextBox)grdSteelChemicalInward.Rows[i].Cells[1].FindControl("txtDia");
                TextBox txtQty = (TextBox)grdSteelChemicalInward.Rows[i].Cells[2].FindControl("txtQty");
                TextBox txtIdMark = (TextBox)grdSteelChemicalInward.Rows[i].Cells[3].FindControl("txtIdMark");
                DropDownList ddlTypeOfSteel = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[4].FindControl("ddlTypeOfSteel");
                DropDownList ddlGradeOfSteel = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[5].FindControl("ddlGradeOfSteel");
                TextBox txtDescription = (TextBox)grdSteelChemicalInward.Rows[i].Cells[6].FindControl("txtDescription");
                CheckBox chkSupplier = (CheckBox)grdSteelChemicalInward.Rows[i].Cells[7].FindControl("chkSupplier");
                DropDownList ddlSupplier = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[8].FindControl("ddlSupplier");

                txtDia.Text = a.STCINWD_Daimeter_tint.ToString();
                txtQty.Text = a.STCINWD_Quantity_tint.ToString();
                txtIdMark.Text = a.STCINWD_IdMark_var.ToString();
                ddlTypeOfSteel.SelectedItem.Text = a.STCINWD_SteelType_var.ToString();
                ddlGradeOfSteel.SelectedItem.Text = a.STCINWD_Grade_var.ToString();
                if (ddlSupplier.Items.FindByText(a.STCINWD_SupplierName_var) != null)
                    ddlSupplier.Items.FindByText(a.STCINWD_SupplierName_var).Selected = true;
                if (a.STCINWD_SupplierFlag_bit == true)
                {
                    chkSupplier.Checked = true;
                }
                i++;
            }
            Chkdesc();
            int count = grdSteelChemicalInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.EnquiryNo = "";
        }
        public void Chkdesc()
        {
            int i = 0;
            var res = dc.AllInward_View("STC", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var a in res)
            {
                TextBox txtDescription = (TextBox)grdSteelChemicalInward.Rows[i].Cells[8].FindControl("txtDescription");
                CheckBox chkSupplier = (CheckBox)grdSteelChemicalInward.Rows[i].Cells[7].FindControl("chkSupplier");
                if (a.STCINWD_SupplierFlag_bit == true)
                {
                    chkSupplier.Checked = true;
                }
                else
                {
                    chkSupplier.Checked = false;
                }
                txtDescription.Text = a.STCINWD_Description_var.ToString();
                i++;
            }
        }

        public void gridAppData()
        {
            int i = 0, qty = 0; //int li = 0; 
         
            var res1 = dc.TestRequestDetails_View_Inward(Convert.ToInt32(UC_InwardHeader1.EnqNoApp), Convert.ToInt32(UC_InwardHeader1.MaterialId), 0, "STC").ToList();
            if (res1.Count > 0)
            {
                foreach (var a in res1)
                {
                    AddRowSteelChemicalInward();
                    TextBox txtDia = (TextBox)grdSteelChemicalInward.Rows[i].Cells[1].FindControl("txtDia");
                    TextBox txtQty = (TextBox)grdSteelChemicalInward.Rows[i].Cells[2].FindControl("txtQty");
                    TextBox txtIdMark = (TextBox)grdSteelChemicalInward.Rows[i].Cells[3].FindControl("txtIdMark");
                    DropDownList ddlTypeOfSteel = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[4].FindControl("ddlTypeOfSteel");
                    DropDownList ddlGradeOfSteel = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[5].FindControl("ddlGradeOfSteel");
                    TextBox txtDescription = (TextBox)grdSteelChemicalInward.Rows[i].Cells[6].FindControl("txtDescription");
                    CheckBox chkSupplier = (CheckBox)grdSteelChemicalInward.Rows[i].Cells[7].FindControl("chkSupplier");
                    DropDownList ddlSupplier = (DropDownList)grdSteelChemicalInward.Rows[i].Cells[8].FindControl("ddlSupplier");

                    txtDia.Text = Convert.ToString(a.diameter);
                    txtQty.Text = Convert.ToString(a.specimen);

                    if (a.specimen != "" && a.specimen != null)
                        qty += Convert.ToInt32(a.specimen);
                    txtIdMark.Text = Convert.ToString(a.Idmark1);
                    //ddlTypeOfSteel.SelectedItem.Text = Convert.ToString(a.STCINWD_SteelType_var);
                    if (a.grade != null)
                    {
                        //ddlGradeOfSteel.SelectedItem.Text = Convert.ToString(a.grade);
                        if (ddlGradeOfSteel.Items.FindByText(Convert.ToString(a.grade)) != null)
                            ddlGradeOfSteel.Items.FindByText(Convert.ToString(a.grade)).Selected = true;
                    }
                    else if (a.specification != null)
                    {
                        if (ddlGradeOfSteel.Items.FindByText(Convert.ToString(a.specification)) != null)
                            ddlGradeOfSteel.Items.FindByText(Convert.ToString(a.specification)).Selected = true;
                    }
                    if (ddlSupplier.Items.FindByText(Convert.ToString(a.supplier)) != null)
                        ddlSupplier.Items.FindByText(Convert.ToString(a.supplier)).Selected = true;
                    else
                    {
                        int suppId = dc.Supplier_Update(a.supplier, true);
                        for (int j = 0; j < grdSteelChemicalInward.Rows.Count; j++)
                        {
                            DropDownList ddlSupplier1 = (DropDownList)grdSteelChemicalInward.Rows[j].FindControl("ddlSupplier");
                            ddlSupplier1.Items.Add(new ListItem(a.supplier, suppId.ToString()));
                        }
                        ddlSupplier.Items.FindByText(a.supplier).Selected = true;
                    }
                    if (ddlSupplier.Items.FindByText(Convert.ToString(a.supplier)) != null)
                    {
                        chkSupplier.Checked = true;
                    }
                    else
                    {
                        chkSupplier.Checked = false;
                    }
                    txtDescription.Text = Convert.ToString(a.description);
                    if (a.make != null && a.make != "")
                        txtDescription.Text += ", Make - " + a.make.ToString();
                    i++;

                }
            }


            int count = grdSteelChemicalInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            //UC_InwardHeader1.EnquiryNo = "";
            UC_InwardHeader1.TotalQty = qty.ToString();
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
                reportStr = rpt.getDetailReportSteelChemicalTestInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("STC_Inward", reportStr);
        }
        bool buttonClicked = false;
        protected string getDetailReportSteelChemicalTestInward()
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
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Steel Chemical Test Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Steel Chemical Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("STC", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }


            var b = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, "STC", null, null);

            foreach (var nt in b)
            {

                if (buttonClicked == true)
                {
                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "STC" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "STC" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
                        "<td height=19><font size=2>" + "STC" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "STC" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
            mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>Dia(mm)</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";

            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Grade</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Sample Description</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Supplier Name/Manufacturer Name</b></font></td>";

            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("STC", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 1% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 1% align=center valign=top height=19 ><font size=2>&nbsp;" + c.STCINWD_Daimeter_tint + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.STCINWD_Quantity_tint + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.STCINWD_IdMark_var + "</font></td>";
                //mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + "Fe " + "" + c.STCINWD_Grade_var + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.STCINWD_Grade_var + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.STCINWD_Description_var + "</font></td>";
                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + c.STCINWD_SupplierName_var + "</font></td>";


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
                reportStr = rpt.getDetailReportSteelChemicalTestInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), true);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("STC_Inward", reportStr);
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
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "STC")));
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
