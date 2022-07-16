using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Solid_Inward : System.Web.UI.Page
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
                    lblheading.Text = "Solid Block Inward";
                }
                LoadSupplierList();
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    AddRowSolidBlockInward();
                }
                if (UC_InwardHeader1.RecType != "")
                {
                    DisplaySolidInwardType();
                }
                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowSolidBlockInward();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "SOLID";
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


        protected void AddRowSolidBlockInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            if (ViewState["SolidInwardTable"] != null)
            {
                GetCurrentDataSoildInward();
                dt = (DataTable)ViewState["SolidInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_TypeOfBlocks", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDateofCasting", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_Test", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Quantity", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Schedule", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_DateofTesting", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Description", typeof(string)));//
                dt.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["ddl_TypeOfBlocks"] = string.Empty;
            dr["txt_IdMark"] = string.Empty;
            dr["txtDateofCasting"] = string.Empty;
            dr["ddl_Test"] = string.Empty;
            dr["txt_Quantity"] = string.Empty;
            dr["txt_Schedule"] = string.Empty;
            dr["txt_DateofTesting"] = string.Empty;
            dr["txt_Description"] = string.Empty;
            dr["ddlSupplier"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["SolidInwardTable"] = dt;
            grdSolidBlockInward.DataSource = dt;
            grdSolidBlockInward.DataBind();
            SetPreviousSolidInward();

        }
        protected void DeleteRowSolidInward(int rowIndex)
        {
            GetCurrentDataSoildInward();
            DataTable dt = ViewState["SolidInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["SolidInwardTable"] = dt;
            grdSolidBlockInward.DataSource = dt;
            grdSolidBlockInward.DataBind();
            SetPreviousSolidInward();
        }
        protected void SetPreviousSolidInward()
        {
            DataTable dt = (DataTable)ViewState["SolidInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddl_TypeOfBlocks = (DropDownList)grdSolidBlockInward.Rows[i].Cells[1].FindControl("ddl_TypeOfBlocks");
                TextBox txt_IdMark = (TextBox)grdSolidBlockInward.Rows[i].Cells[2].FindControl("txt_IdMark");
                TextBox txtDateofCasting = (TextBox)grdSolidBlockInward.Rows[i].Cells[3].FindControl("txtDateofCasting");
                DropDownList ddl_Test = (DropDownList)grdSolidBlockInward.Rows[i].Cells[4].FindControl("ddl_Test");
                TextBox txt_Quantity = (TextBox)grdSolidBlockInward.Rows[i].Cells[5].FindControl("txt_Quantity");
                TextBox txt_Schedule = (TextBox)grdSolidBlockInward.Rows[i].Cells[6].FindControl("txt_Schedule");
                TextBox txt_DateofTesting = (TextBox)grdSolidBlockInward.Rows[i].Cells[7].FindControl("txt_DateofTesting");
                TextBox txt_Description = (TextBox)grdSolidBlockInward.Rows[i].Cells[8].FindControl("txt_Description");
                DropDownList ddlSupplier = (DropDownList)grdSolidBlockInward.Rows[i].Cells[9].FindControl("ddlSupplier");

                grdSolidBlockInward.Rows[i].Cells[0].Text = (i + 1).ToString();
                ddl_TypeOfBlocks.Text = dt.Rows[i]["ddl_TypeOfBlocks"].ToString();
                txt_IdMark.Text = dt.Rows[i]["txt_IdMark"].ToString();
                txtDateofCasting.Text = dt.Rows[i]["txtDateofCasting"].ToString();
                ddl_Test.Text = dt.Rows[i]["ddl_Test"].ToString();
                txt_Quantity.Text = dt.Rows[i]["txt_Quantity"].ToString();
                txt_Schedule.Text = dt.Rows[i]["txt_Schedule"].ToString();
                txt_DateofTesting.Text = dt.Rows[i]["txt_DateofTesting"].ToString();
                txt_Description.Text = dt.Rows[i]["txt_Description"].ToString();
                ddlSupplier.SelectedValue = dt.Rows[i]["ddlSupplier"].ToString();
            }
        }
        protected void GetCurrentDataSoildInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_TypeOfBlocks", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDateofCasting", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_Test", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Quantity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Schedule", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_DateofTesting", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Description", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));
            for (int i = 0; i < grdSolidBlockInward.Rows.Count; i++)
            {
                DropDownList ddl_TypeOfBlocks = (DropDownList)grdSolidBlockInward.Rows[i].Cells[1].FindControl("ddl_TypeOfBlocks");
                TextBox txt_IdMark = (TextBox)grdSolidBlockInward.Rows[i].Cells[2].FindControl("txt_IdMark");
                TextBox txtDateofCasting = (TextBox)grdSolidBlockInward.Rows[i].Cells[3].FindControl("txtDateofCasting");
                DropDownList ddl_Test = (DropDownList)grdSolidBlockInward.Rows[i].Cells[4].FindControl("ddl_Test");
                TextBox txt_Quantity = (TextBox)grdSolidBlockInward.Rows[i].Cells[5].FindControl("txt_Quantity");
                TextBox txt_Schedule = (TextBox)grdSolidBlockInward.Rows[i].Cells[6].FindControl("txt_Schedule");
                TextBox txt_DateofTesting = (TextBox)grdSolidBlockInward.Rows[i].Cells[7].FindControl("txt_DateofTesting");
                TextBox txt_Description = (TextBox)grdSolidBlockInward.Rows[i].Cells[8].FindControl("txt_Description");
                DropDownList ddlSupplier = (DropDownList)grdSolidBlockInward.Rows[i].Cells[9].FindControl("ddlSupplier");
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["ddl_TypeOfBlocks"] = ddl_TypeOfBlocks.Text;
                drRow["txt_IdMark"] = txt_IdMark.Text;
                drRow["txtDateofCasting"] = txtDateofCasting.Text;
                drRow["ddl_Test"] = ddl_Test.Text;
                drRow["txt_Quantity"] = txt_Quantity.Text;
                drRow["txt_Schedule"] = txt_Schedule.Text;
                drRow["txt_DateofTesting"] = txt_DateofTesting.Text;
                drRow["txt_Description"] = txt_Description.Text;
                drRow["ddlSupplier"] = ddlSupplier.SelectedValue;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["SolidInwardTable"] = dtTable;

        }
        protected void TextChanged(object sender, EventArgs e)
        {
            TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
            if (txtSubsets.Text != "")
            {
                if (Convert.ToInt32(txtSubsets.Text) < grdSolidBlockInward.Rows.Count)
                {
                    for (int i = grdSolidBlockInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdSolidBlockInward.Rows.Count > 1)
                        {
                            DeleteRowSolidInward(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdSolidBlockInward.Rows.Count)
                {
                    for (int i = grdSolidBlockInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
                    {
                        AddRowSolidBlockInward();
                    }
                }
            }

        }
        protected void Click(object sender, EventArgs e)
        {
            ViewState["SolidInwardTable"] = null;
             if (UC_InwardHeader1.EnquiryNo != "" && UC_InwardHeader1.EnqNoApp != "" && UC_InwardHeader1.EnqNoApp != "0")
                gridAppData();
            else
                AddRowSolidBlockInward();
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
        //protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("Enquiry_List.aspx");
        //}
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
                //DateTime ReceivedDate = Convert.ToDateTime(DateTime.Now.Date.ToString());
                DateTime ReceivedDate = DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null);
                DateTime d1 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "SOLID", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, true);
                }
                else
                {
                    Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    NewrecNo = clsObj.GetnUpdateRecordNo("SOLID");
                    UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                    clsObj.insertRecordTable(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), NewrecNo, "SOLID");
                    UC_InwardHeader1.ReferenceNo = clsObj.getRefNo(Convert.ToInt32(UC_InwardHeader1.EnquiryNo)).ToString();
                    dc.Inward_Update_New(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "SOLID", 0, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate);

                    //Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    //NewrecNo = clsObj.GetnUpdateRecordNo("SOLID");
                    //UC_InwardHeader1.RecordNo = NewrecNo.ToString(); 
                    //UC_InwardHeader1.ReferenceNo = dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "SOLID", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, false).ToString();
                    ////var inward = dc.Inward_View(Convert.ToInt32(UC_InwardHeader1.ReferenceNo), 0, "");
                    ////foreach (var inwd in inward)
                    ////{
                    ////    UC_InwardHeader1.RecordNo = inwd.INWD_RecordNo_int.ToString();
                    ////}
                }

                dc.Enquiry_Update_Status(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 2);
                dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "SOLID", "", null, null, false, false, false, false, false, false, true);//Delete MIS Record
                dc.ClientSupplier_Update(Convert.ToInt32(UC_InwardHeader1.ClientId), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, null, true);

                dc.SolidInward_Update("SOLID", Convert.ToInt32(UC_InwardHeader1.RecordNo), "", "", 0, 0, "", null, "", "", 0, "", "", "", 0, 0, null, null, "", "", 0, 0, true);
                if (lblRptClientId.Text != "" && lblRptClientId.Text != "0")
                {
                    dc.Inward_Update_ReportClient(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, Convert.ToInt32(lblRptClientId.Text), Convert.ToInt32(lblRptSiteId.Text));
                }
                for (int i = 0; i <= grdSolidBlockInward.Rows.Count - 1; i++)
                {
                    DropDownList ddl_TypeOfBlocks = (DropDownList)grdSolidBlockInward.Rows[i].Cells[1].FindControl("ddl_TypeOfBlocks");
                    TextBox txt_IdMark = (TextBox)grdSolidBlockInward.Rows[i].Cells[2].FindControl("txt_IdMark");
                    TextBox txtDateofCasting = (TextBox)grdSolidBlockInward.Rows[i].Cells[3].FindControl("txtDateofCasting");
                    DropDownList ddl_Test = (DropDownList)grdSolidBlockInward.Rows[i].Cells[4].FindControl("ddl_Test");
                    TextBox txt_Quantity = (TextBox)grdSolidBlockInward.Rows[i].Cells[5].FindControl("txt_Quantity");
                    TextBox txt_Schedule = (TextBox)grdSolidBlockInward.Rows[i].Cells[6].FindControl("txt_Schedule");
                    TextBox txt_DateofTesting = (TextBox)grdSolidBlockInward.Rows[i].Cells[7].FindControl("txt_DateofTesting");
                    TextBox txt_Description = (TextBox)grdSolidBlockInward.Rows[i].Cells[8].FindControl("txt_Description");
                    DropDownList ddlSupplier = (DropDownList)grdSolidBlockInward.Rows[i].Cells[9].FindControl("ddlSupplier");
                    txtDateofCasting.Text = txtDateofCasting.Text.ToUpper();
                    SrNo = Convert.ToInt32(grdSolidBlockInward.Rows[i].Cells[0].Text);
                    RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    DateTime txtDateofTestingTxt = DateTime.ParseExact(txt_DateofTesting.Text, "dd/MM/yyyy", null);
                    dc.SolidInward_Update("SOLID", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txt_Quantity.Text), txtDateofCasting.Text, txtDateofTestingTxt, txt_Description.Text, ddlSupplier.SelectedItem.Text, 0, "", txt_IdMark.Text, ddl_TypeOfBlocks.SelectedItem.Text, Convert.ToByte(txt_Schedule.Text), Convert.ToInt32(ddl_Test.SelectedValue), d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), false);

                    dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "SOLID", RefNo, "SOLID", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);

                    dc.ClientSupplier_Update(Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(ddlSupplier.SelectedValue), Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, ReceivedDate, false);
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
                        BillNo = bill.UpdateBill("SOLID", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
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
                //        ProformaInvoiceNo = ProInv.UpdateProformaInvoice("SOLID", Convert.ToInt32(UC_InwardHeader1.RecordNo), ProformaInvoiceNo);
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
            int sumQty = 0;
            DateTime d1 = new DateTime();
            DateTime d2 = new DateTime();
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
                for (int i = 0; i <= grdSolidBlockInward.Rows.Count - 1; i++)
                {
                    DropDownList ddl_TypeOfBlocks = (DropDownList)grdSolidBlockInward.Rows[i].Cells[1].FindControl("ddl_TypeOfBlocks");
                    TextBox txt_IdMark = (TextBox)grdSolidBlockInward.Rows[i].Cells[2].FindControl("txt_IdMark");
                    TextBox txtDateofCasting = (TextBox)grdSolidBlockInward.Rows[i].Cells[3].FindControl("txtDateofCasting");
                    DropDownList ddl_Test = (DropDownList)grdSolidBlockInward.Rows[i].Cells[4].FindControl("ddl_Test");
                    TextBox txt_Quantity = (TextBox)grdSolidBlockInward.Rows[i].Cells[5].FindControl("txt_Quantity");
                    TextBox txt_Schedule = (TextBox)grdSolidBlockInward.Rows[i].Cells[6].FindControl("txt_Schedule");
                    TextBox txt_DateofTesting = (TextBox)grdSolidBlockInward.Rows[i].Cells[7].FindControl("txt_DateofTesting");
                    TextBox txt_Description = (TextBox)grdSolidBlockInward.Rows[i].Cells[8].FindControl("txt_Description");
                    DropDownList ddlSupplier = (DropDownList)grdSolidBlockInward.Rows[i].Cells[9].FindControl("ddlSupplier");
                    txtDateofCasting.Text = txtDateofCasting.Text.ToUpper();
                    if (ddl_TypeOfBlocks.SelectedItem.Text == "Select")
                    {
                        lblMsg.Text = "Select Type of Blocks for row no " + (i + 1) + ".";
                        ddl_TypeOfBlocks.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_IdMark.Text == "")
                    {
                        lblMsg.Text = "Enter Id Mark for row no " + (i + 1) + ".";
                        txt_IdMark.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtDateofCasting.Text == "")
                    {
                        lblMsg.Text = "Enter Date of Casting for row no " + (i + 1) + ".";
                        txtDateofCasting.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddl_Test.SelectedItem.Text == "Select")
                    {
                        lblMsg.Text = "Select the Test Type for row no " + (i + 1) + ".";
                        ddl_Test.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_Quantity.Text == "")
                    {
                        lblMsg.Text = "Enter Quantity for row no " + (i + 1) + ".";
                        txt_Quantity.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_Schedule.Text == "")
                    {
                        lblMsg.Text = "Enter Schedule for row no " + (i + 1) + ".";
                        txt_Schedule.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_DateofTesting.Text == "")
                    {
                        lblMsg.Text = "Enter Date of Testing for row no " + (i + 1) + ".";
                        txt_DateofTesting.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_Description.Text == "")
                    {
                        lblMsg.Text = "Enter Description for row no " + (i + 1) + ".";
                        txt_Description.Focus();
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
                    if (txtDateofCasting.Text != "NA")
                    {
                        strDate = txtDateofCasting.Text.Split('/');
                        d1 = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
                        strDate = UC_InwardHeader1.CollectionDate.Split('/');
                        d2 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);
                        if (txt_Schedule.Text != "")
                        {
                            int Schedule = Convert.ToInt32(txt_Schedule.Text);
                            //var txtDateCasting = Convert.ToDateTime(txtDateofCasting.Text);
                            var txtDateCasting = DateTime.ParseExact(txtDateofCasting.Text, "dd/MM/yyyy", null);
                            string castingDate = (DateTime.Parse(txtDateCasting.ToString()).AddDays(Schedule)).ToString("dd/MM/yyyy");
                            txt_DateofTesting.Text = castingDate.ToString();
                        }
                        if (d1 > d2)
                        {
                            lblMsg.Text = "Date Of Casting should be less than equal to Collection Date for row number " + (i + 1) + ".";
                            txtDateofCasting.Focus();
                            valid = false;
                            break;
                        }
                    }
                    string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime txtDateOfTesting1 = DateTime.ParseExact(txt_DateofTesting.Text, "dd/MM/yyyy", null);
                    DateTime currentDate1 = DateTime.ParseExact(CurrentDate, "dd/MM/yyyy", null);
                    if (UC_InwardHeader1.RecType == null)
                    {
                        if (txtDateOfTesting1 < currentDate1)
                        {
                            lblMsg.Text = "Testing Date should be greater than or equal to the Current Date for row number" + (i + 1) + "."; ;
                            valid = false;
                            break;
                        }
                    }
                    sumQty += Convert.ToInt32(txt_Quantity.Text);
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
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + dispalyMsg + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
        public void DisplaySolidInwardType()
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
            SolidInwardgrid();
        }
        public void SolidInwardgrid()
        {
            int i = 0;
            var PVINWARD = dc.AllInward_View("SOLID", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var n in PVINWARD)
            {
                AddRowSolidBlockInward();
                DropDownList ddl_TypeOfBlocks = (DropDownList)grdSolidBlockInward.Rows[i].Cells[1].FindControl("ddl_TypeOfBlocks");
                TextBox txt_IdMark = (TextBox)grdSolidBlockInward.Rows[i].Cells[2].FindControl("txt_IdMark");
                TextBox txtDateofCasting = (TextBox)grdSolidBlockInward.Rows[i].Cells[3].FindControl("txtDateofCasting");
                DropDownList ddl_Test = (DropDownList)grdSolidBlockInward.Rows[i].Cells[4].FindControl("ddl_Test");
                TextBox txt_Quantity = (TextBox)grdSolidBlockInward.Rows[i].Cells[5].FindControl("txt_Quantity");
                TextBox txt_Schedule = (TextBox)grdSolidBlockInward.Rows[i].Cells[6].FindControl("txt_Schedule");
                TextBox txt_DateofTesting = (TextBox)grdSolidBlockInward.Rows[i].Cells[7].FindControl("txt_DateofTesting");
                TextBox txt_Description = (TextBox)grdSolidBlockInward.Rows[i].Cells[8].FindControl("txt_Description");
                DropDownList ddlSupplier = (DropDownList)grdSolidBlockInward.Rows[i].Cells[9].FindControl("ddlSupplier");

                ddl_TypeOfBlocks.SelectedItem.Text = n.SOLIDINWD_BlockType_var.ToString();
                txt_IdMark.Text = n.SOLIDINWD_IdMark_var.ToString();
                ddl_Test.SelectedValue = n.SOLIDINWD_TEST_Id.ToString();
                txt_Quantity.Text = n.SOLIDINWD_Quantity_tint.ToString();
                txt_Schedule.Text = n.SOLIDINWD_Schedule_tint.ToString();
                txt_DateofTesting.Text = Convert.ToDateTime(n.SOLIDINWD_TestingDate_dt).ToString("dd/MM/yyyy");
                if (ddlSupplier.Items.FindByText(n.SOLIDINWD_SupplierName_var) != null)
                    ddlSupplier.Items.FindByText(n.SOLIDINWD_SupplierName_var).Selected = true;
                txt_Description.Text = n.SOLIDINWD_Description_var.ToString();
                if (n.SOLIDINWD_CastingDate_nvar != "NA")
                {
                    txtDateofCasting.Text = n.SOLIDINWD_CastingDate_nvar.ToString();
                }
                else
                {
                    txtDateofCasting.Text = "NA";
                }
                i++;
            }
            int count = grdSolidBlockInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.EnquiryNo = "";
        }
        public void gridAppData()
        {
            int i = 0, qty = 0;
            string testDate = "NA";

            var res1 = dc.TestRequestDetails_View_Inward(Convert.ToInt32(UC_InwardHeader1.EnqNoApp), Convert.ToInt32(UC_InwardHeader1.MaterialId), 1, "SOLID").ToList();
            foreach (var n in res1)
            {
                AddRowSolidBlockInward();
                DropDownList ddl_TypeOfBlocks = (DropDownList)grdSolidBlockInward.Rows[i].Cells[1].FindControl("ddl_TypeOfBlocks");
                TextBox txt_IdMark = (TextBox)grdSolidBlockInward.Rows[i].Cells[2].FindControl("txt_IdMark");
                TextBox txtDateofCasting = (TextBox)grdSolidBlockInward.Rows[i].Cells[3].FindControl("txtDateofCasting");
                DropDownList ddl_Test = (DropDownList)grdSolidBlockInward.Rows[i].Cells[4].FindControl("ddl_Test");
                TextBox txt_Quantity = (TextBox)grdSolidBlockInward.Rows[i].Cells[5].FindControl("txt_Quantity");
                TextBox txt_Schedule = (TextBox)grdSolidBlockInward.Rows[i].Cells[6].FindControl("txt_Schedule");
                TextBox txt_DateofTesting = (TextBox)grdSolidBlockInward.Rows[i].Cells[7].FindControl("txt_DateofTesting");
                TextBox txt_Description = (TextBox)grdSolidBlockInward.Rows[i].Cells[8].FindControl("txt_Description");
                DropDownList ddlSupplier = (DropDownList)grdSolidBlockInward.Rows[i].Cells[9].FindControl("ddlSupplier");

                ddl_TypeOfBlocks.SelectedItem.Text = Convert.ToString(n.specification);
                txt_IdMark.Text = Convert.ToString(n.Idmark1);
                txt_Quantity.Text = n.specimen.ToString();
                if (n.specimen != "" && n.specimen != null)
                    qty += Convert.ToInt32(n.specimen);
                txt_Schedule.Text = Convert.ToString(n.schedule);
                DateTime d2;
                if (n.schedule != "" && n.schedule != null)
                {
                    if (Convert.ToString(n.casting_dt) != "" && Convert.ToString(n.casting_dt) != "NA" && Convert.ToString(n.casting_dt) != null)
                    {
                        string[] strDate = n.casting_dt.Split('/');
                        d2 = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
                        d2 = d2.AddDays(Convert.ToInt32(n.schedule));
                        testDate = d2.ToString("dd/MM/yyyy");
                    }
                    else if (Convert.ToString(n.casting_dt) != "NA")
                       testDate = DateTime.Today.ToString("dd/MM/yyyy");

                }
                txt_DateofTesting.Text = testDate;
                if (ddlSupplier.Items.FindByText(Convert.ToString(n.supplier)) != null)
                    ddlSupplier.Items.FindByText(Convert.ToString(n.supplier)).Selected = true;
                else
                {
                    int suppId = dc.Supplier_Update(n.supplier, true);
                    for (int j = 0; j < grdSolidBlockInward.Rows.Count; j++)
                    {
                        DropDownList ddlSupplier1 = (DropDownList)grdSolidBlockInward.Rows[j].FindControl("ddlSupplier");
                        ddlSupplier1.Items.Add(new ListItem(n.supplier, suppId.ToString()));
                    }
                    ddlSupplier.Items.FindByText(n.supplier).Selected = true;
                }
                txt_Description.Text = Convert.ToString(n.description);
                if (n.make != null && n.make != "")
                    txt_Description.Text += ", Make - " + n.make.ToString();
                if (Convert.ToString(n.casting_dt) != "NA" && n.casting_dt != "" && n.casting_dt != null)
                {
                    txtDateofCasting.Text = Convert.ToString(n.casting_dt);
                }
                else
                {
                    txtDateofCasting.Text = "NA";
                }
                if (Convert.ToString(n.testPerReqTestId) != "" && Convert.ToString(n.testPerReqTestId) != null)
                    ddl_Test.SelectedValue = Convert.ToString(n.testPerReqTestId);


                i++;
            }
            int count = grdSolidBlockInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.TotalQty = qty.ToString();
        }

        protected void grdSolidBlockInward_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl_Test = (DropDownList)e.Row.FindControl("ddl_Test");
                int InwardId = 0;
                var mat = dc.Material_View("SOLID", "");
                foreach (var m in mat)
                {
                    InwardId = Convert.ToInt32(m.MATERIAL_Id.ToString());
                }
                var test = dc.Test_View(InwardId, 0, "", 0, 0, 0);
                ddl_Test.DataSource = test;
                ddl_Test.DataTextField = "TEST_Name_var";
                ddl_Test.DataValueField = "TEST_Id";
                ddl_Test.DataBind();
                ddl_Test.Items.Insert(0, new ListItem("Select", "0"));

                DropDownList ddlSupplier = (DropDownList)e.Row.FindControl("ddlSupplier");
                ddlSupplier.DataSource = ViewState["SupplierTable"];
                ddlSupplier.DataTextField = "SUPPL_Name_var";
                ddlSupplier.DataValueField = "SUPPL_Id";
                ddlSupplier.DataBind();
                ddlSupplier.Items.Insert(0, new ListItem("---Select---", "0"));
            }
        }
        protected void txt_Schedule_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i <= grdSolidBlockInward.Rows.Count - 1; i++)
            {
                TextBox txtDateofCasting = (TextBox)grdSolidBlockInward.Rows[i].Cells[3].FindControl("txtDateofCasting");
                TextBox txt_Schedule = (TextBox)grdSolidBlockInward.Rows[i].Cells[6].FindControl("txt_Schedule");
                TextBox txt_DateofTesting = (TextBox)grdSolidBlockInward.Rows[i].Cells[7].FindControl("txt_DateofTesting");
                txtDateofCasting.Text = txtDateofCasting.Text.ToUpper();
                if (txt_Schedule.Text != "" && txtDateofCasting.Text != "")
                {
                    if (txtDateofCasting.Text != "NA")
                    {
                        int Schedule = Convert.ToInt32(txt_Schedule.Text);
                        DateTime txtDateCasting = DateTime.ParseExact(txtDateofCasting.Text, "dd/MM/yyyy", null);
                        //var txtDateCasting = Convert.ToDateTime(txtDateofCasting.Text);
                        string castingDate = (DateTime.Parse(txtDateCasting.ToString()).AddDays(Schedule)).ToString("dd/MM/yyyy");
                        txt_DateofTesting.Text = castingDate.ToString();
                    }
                    else
                    {
                        txt_DateofTesting.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                    }
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
                reportStr = rpt.getDetailReportMasonaryInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Solid_Inward", reportStr);
        }
        bool buttonClicked = false;
        protected string getDetailReportMasonaryInward()
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
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Solid Masonry Block Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Solid Masonry Block Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("SOLID", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }


            var b = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, "SOLID", null, null);

            foreach (var nt in b)
            {
                if (buttonClicked == true)
                {
                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "SOLID" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "SOLID" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
                        "<td height=19><font size=2>" + "SOLID" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "SOLID" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Type</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Date Of Casting</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sched.Of Test</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Test</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Date Of Testing</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("SOLID", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_BlockType_var + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_IdMark_var + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_CastingDate_nvar.ToString() + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_Quantity_tint + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_Schedule_tint + "</font></td>";


                var Testtype = dc.Test_View(0, Convert.ToInt32(c.SOLIDINWD_TEST_Id.ToString()), "", 0, 0, 0);
                foreach (var t in Testtype)
                {
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + t.TEST_Name_var.ToString() + "</font></td>";
                }


                mySql += "<td width= 2  % align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDateTime(c.SOLIDINWD_TestingDate_dt).ToString("dd/MM/yyyy") + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_Description_var + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_SupplierName_var + "</font></td>";


                mySql += "</tr>";
                i++;
            }


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Remark" + "</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Testing done as per IS-2185(Part I):2005." + "</font></td>";
            mySql += "</tr>";
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
                reportStr = rpt.getDetailReportMasonaryInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), true);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Solid_LabSheet", reportStr);
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
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "SOLID")));
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
