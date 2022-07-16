using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class AAC_Inward : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        static DateTime recdDate = DateTime.Now;
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();

        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "AAC Block Inward";

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
                LoadSupplierList();
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    AddRowAACInward();
                    AddRowAACOtherCharges();
                }
                if (UC_InwardHeader1.RecType != "")
                {
                    DisplayAACTest();
                    DisplayOtherCharges();
                }
                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowAACInward();
                    AddRowAACOtherCharges();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "AAC";
                }
                lnkSave.Visible = true;

            }
        }


        #region add/delete row cube grid
        protected void AddRowAACInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["AACInwardTable"] != null)
            {
                GetCurrentDataAACInward();
                dt = (DataTable)ViewState["AACInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCSQty", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_Test", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));

            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtIdMark"] = string.Empty;
            dr["txtCSQty"] = string.Empty;
            dr["ddl_Test"] = string.Empty;
            dr["txtDescription"] = string.Empty;
            dr["ddlSupplier"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["AACInwardTable"] = dt;
            grdAACInward.DataSource = dt;
            grdAACInward.DataBind();
            SetPreviousDataAACInward();
        }
        protected void DeleteRowAACInward(int rowIndex)
        {
            GetCurrentDataAACInward();
            DataTable dt = ViewState["AACInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["AACInwardTable"] = dt;
            grdAACInward.DataSource = dt;
            grdAACInward.DataBind();
            SetPreviousDataAACInward();
        }
        protected void SetPreviousDataAACInward()
        {
            DataTable dt = (DataTable)ViewState["AACInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdAACInward.Rows[i].Cells[1].FindControl("txtIdMark");
                TextBox box3 = (TextBox)grdAACInward.Rows[i].Cells[2].FindControl("txtCSQty");
                DropDownList box4 = (DropDownList)grdAACInward.Rows[i].Cells[3].FindControl("ddl_Test");
                TextBox box5 = (TextBox)grdAACInward.Rows[i].Cells[4].FindControl("txtDescription");
                DropDownList box6 = (DropDownList)grdAACInward.Rows[i].Cells[5].FindControl("ddlSupplier");

                grdAACInward.Rows[i].Cells[0].Text = (i + 1).ToString();
                box2.Text = dt.Rows[i]["txtIdMark"].ToString();
                box3.Text = dt.Rows[i]["txtCSQty"].ToString();
                box4.Text = dt.Rows[i]["ddl_Test"].ToString();
                box5.Text = dt.Rows[i]["txtDescription"].ToString();
                box6.Text = dt.Rows[i]["ddlSupplier"].ToString();


            }
        }
        protected void GetCurrentDataAACInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtCSQty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_Test", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));
            for (int i = 0; i < grdAACInward.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdAACInward.Rows[i].Cells[1].FindControl("txtIdMark");
                TextBox box3 = (TextBox)grdAACInward.Rows[i].Cells[2].FindControl("txtCSQty");
                DropDownList box4 = (DropDownList)grdAACInward.Rows[i].Cells[3].FindControl("ddl_Test");
                TextBox box5 = (TextBox)grdAACInward.Rows[i].Cells[4].FindControl("txtDescription");
                DropDownList box6 = (DropDownList)grdAACInward.Rows[i].Cells[5].FindControl("ddlSupplier");


                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txtIdMark"] = box2.Text;
                drRow["txtCSQty"] = box3.Text;
                drRow["ddl_Test"] = box4.Text;
                drRow["txtDescription"] = box5.Text;
                drRow["ddlSupplier"] = box6.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["AACInwardTable"] = dtTable;

        }
        #endregion
        #region AACOtherCharges
        public void DisplayOtherCharges()
        {
            int i = 0;
            var re = dc.OtherCharges_View(UC_InwardHeader1.RecType, Convert.ToInt32(UC_InwardHeader1.RecordNo));
            foreach (var r in re)
            {
                AddRowAACOtherCharges();
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
                AddRowAACOtherCharges();
            }

        }
        protected void AddRowAACOtherCharges()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["AACOtherCharges_Table"] != null)
            {
                GetCurrentDataAACOtherCharges();
                dt = (DataTable)ViewState["AACOtherCharges_Table"];
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


            ViewState["AACOtherCharges_Table"] = dt;
            grdOtherCharges.DataSource = dt;
            grdOtherCharges.DataBind();
            SetPreviousDataAACOtherCharges();
        }
        protected void GetCurrentDataAACOtherCharges()
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
            ViewState["AACOtherCharges_Table"] = dtTable;

        }
        protected void SetPreviousDataAACOtherCharges()
        {
            DataTable dt = (DataTable)ViewState["AACOtherCharges_Table"];
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
        protected void DeleteRowAACOtherCharges(int rowIndex)
        {
            GetCurrentDataAACOtherCharges();
            DataTable dt = ViewState["AACOtherCharges_Table"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["AACOtherCharges_Table"] = dt;
            grdOtherCharges.DataSource = dt;
            grdOtherCharges.DataBind();
            SetPreviousDataAACOtherCharges();
        }
        protected void imgInsert_Click(object sender, CommandEventArgs e)
        {
            AddRowAACOtherCharges();
        }
        protected void imgDelete_Click(object sender, CommandEventArgs e)
        {
            if (grdOtherCharges.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdOtherCharges.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowAACOtherCharges(gvr.RowIndex);
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
        protected void grdAACInward_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl_Test = (DropDownList)e.Row.FindControl("ddl_Test");
                int InwardId = 0;
                var mat = dc.Material_View("AAC", "");
                foreach (var m in mat)
                {
                    InwardId = Convert.ToInt32(m.MATERIAL_Id.ToString());
                }
                var test = dc.Test_View(InwardId, 0, "", 0, 0, 0);
                ddl_Test.DataSource = test;
                ddl_Test.DataTextField = "TEST_Name_var";
                ddl_Test.DataValueField = "TEST_Id";
                ddl_Test.DataBind();
                ddl_Test.Items.Insert(0, new ListItem("---Select---", "0"));

                DropDownList ddlSupplier = (DropDownList)e.Row.FindControl("ddlSupplier");
                ddlSupplier.DataSource = ViewState["SupplierTable"];
                ddlSupplier.DataTextField = "SUPPL_Name_var";
                ddlSupplier.DataValueField = "SUPPL_Id";
                ddlSupplier.DataBind();
                ddlSupplier.Items.Insert(0, new ListItem("---Select---", "0"));
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
        protected void LoadEnquiryList(int materialId)
        {
            var enqList = dc.Enquiry_View(0, 1, materialId);
            DropDownList ddlEnquiryList = (DropDownList)UC_InwardHeader1.FindControl("ddlEnquiryList");
            ddlEnquiryList.DataSource = enqList;
            ddlEnquiryList.DataTextField = "ENQ_Id";
            ddlEnquiryList.DataBind();
            ddlEnquiryList.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            //clsData clsObj = new clsData();
            //clsObj.getEnquiryDetailForSMS(58990,"");

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
                string[] strDate = new string[3];
                //DateTime ReceivedDate = Convert.ToDateTime(DateTime.Now.Date.ToString());
                DateTime ReceivedDate = DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null);
                DateTime d1 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "AAC", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, true);
                }
                else
                {
                    Int32 NewrecNo = 0;
                  
                    NewrecNo = clsObj.GetnUpdateRecordNo("AAC");
                    UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                    clsObj.insertRecordTable(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), NewrecNo, "AAC");
                    UC_InwardHeader1.ReferenceNo = clsObj.getRefNo(Convert.ToInt32(UC_InwardHeader1.EnquiryNo)).ToString();
                    dc.Inward_Update_New(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "AAC", 0, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate);

                }

                dc.Enquiry_Update_Status(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 2);
                dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "AAC", "", null, null, false, false, false, false, false, false, true);//Delete MIS Record
                dc.ClientSupplier_Update(Convert.ToInt32(UC_InwardHeader1.ClientId), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, null, true);
                dc.AAC_Inward_Update("AAC", Convert.ToInt32(UC_InwardHeader1.RecordNo), "", "", 0, 0, null, "", "", 0, "", 0, "", null, null, "", "", 0, 0, false, true,0,0);

                if (lblRptClientId.Text != "" && lblRptClientId.Text != "0")
                {
                    dc.Inward_Update_ReportClient(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, Convert.ToInt32(lblRptClientId.Text), Convert.ToInt32(lblRptSiteId.Text));
                }
                for (int i = 0; i <= grdAACInward.Rows.Count - 1; i++)
                {
                    TextBox IdMark = (TextBox)grdAACInward.Rows[i].Cells[1].FindControl("txtIdMark");
                    TextBox Quantity = (TextBox)grdAACInward.Rows[i].Cells[2].FindControl("txtCSQty");
                    DropDownList ddl_Test = (DropDownList)grdAACInward.Rows[i].Cells[3].FindControl("ddl_Test");
                    TextBox Description = (TextBox)grdAACInward.Rows[i].Cells[4].FindControl("txtDescription");
                    DropDownList ddlSupplier = (DropDownList)grdAACInward.Rows[i].Cells[5].FindControl("ddlSupplier");

                    SrNo = Convert.ToInt32(grdAACInward.Rows[i].Cells[0].Text);
                    RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    ///////////////////////////changes01/08/2017
                    dc.AAC_Inward_Update("AAC", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(Quantity.Text), null, Description.Text, ddlSupplier.SelectedItem.Text, 0, "", Convert.ToInt32(ddl_Test.SelectedValue), IdMark.Text, d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false, 0, 0);
                    dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "AAC", RefNo, "AAC", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);
                    dc.ClientSupplier_Update(Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(ddlSupplier.SelectedValue), Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, ReceivedDate, false);
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
                        BillNo = bill.UpdateBill("AAC", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
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
                //        ProformaInvoiceNo = ProInv.UpdateProformaInvoice("AAC", Convert.ToInt32(UC_InwardHeader1.RecordNo), ProformaInvoiceNo);
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
                lnkLabSheet.Visible = true;
                lnkPrint.Visible = true;

                UC_InwardHeader1.EnquiryNo = "";
                DropDownList ddlEnquiryList = (DropDownList)UC_InwardHeader1.FindControl("ddlEnquiryList");
                ddlEnquiryList.ClearSelection();
                LoadEnquiryList(Convert.ToInt32(UC_InwardHeader1.MaterialId));


            }
        }

        protected void TextChanged(object sender, EventArgs e)
        {
            TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
            if (txtSubsets.Text != "")
            {
                if (Convert.ToInt32(txtSubsets.Text) < grdAACInward.Rows.Count)
                {
                    for (int i = grdAACInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdAACInward.Rows.Count > 1)
                        {
                            DeleteRowAACInward(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdAACInward.Rows.Count)
                {
                    for (int i = grdAACInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
                    {
                        AddRowAACInward();
                    }
                }
            }
        }

        protected void Click(object sender, EventArgs e)
        {
            ddlTestedAt.SelectedIndex = 0;
            ViewState["AACInwardTable"] = null;
             if (UC_InwardHeader1.EnquiryNo != "" && UC_InwardHeader1.EnqNoApp != "" && UC_InwardHeader1.EnqNoApp != "0")
                gridAppData();
            else
                AddRowAACInward();
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

        private void gridAppData()
        {
            int i = 0,qty=0;
            var res1 = dc.TestRequestDetails_View_Inward(Convert.ToInt32(UC_InwardHeader1.EnqNoApp), Convert.ToInt32(UC_InwardHeader1.MaterialId), 1, "AAC").ToList();
            if (res1.Count > 0)
            {
                foreach (var w in res1)
                {
                    AddRowAACInward();
                    TextBox IdMark = (TextBox)grdAACInward.Rows[i].Cells[1].FindControl("txtIdMark");
                    TextBox Quantity = (TextBox)grdAACInward.Rows[i].Cells[2].FindControl("txtCSQty");
                    DropDownList ddl_Test = (DropDownList)grdAACInward.Rows[i].Cells[3].FindControl("ddl_Test");
                    TextBox Description = (TextBox)grdAACInward.Rows[i].Cells[4].FindControl("txtDescription");
                    DropDownList ddlSupplier = (DropDownList)grdAACInward.Rows[i].Cells[5].FindControl("ddlSupplier");

                    IdMark.Text = Convert.ToString(w.Idmark1);
                    Quantity.Text = Convert.ToString(w.specimen);

                    if (w.specimen != "" && w.specimen != null)
                        qty += Convert.ToInt32(w.specimen);

                    Description.Text = Convert.ToString(w.description);

                    if (w.make != null && w.make != "")
                        Description.Text += ", Make - " + w.make.ToString();
                    //if (Convert.ToString(w.testTable_id) != "" && Convert.ToString(w.testTable_id) != null)
                    //    ddl_Test.SelectedValue = Convert.ToString(w.testTable_id);

                    if (Convert.ToString(w.testPerReqTestId) != "" && Convert.ToString(w.testPerReqTestId) != null)
                        ddl_Test.SelectedValue = Convert.ToString(w.testPerReqTestId);

                    if (ddlSupplier.Items.FindByText(w.supplier) != null)
                        ddlSupplier.Items.FindByText(w.supplier).Selected = true;
                    else
                    {
                        int suppId = dc.Supplier_Update(w.supplier, true);
                        for (int j = 0; j < grdAACInward.Rows.Count; j++)
                        {
                            DropDownList ddlSupplier1 = (DropDownList)grdAACInward.Rows[j].Cells[5].FindControl("ddlSupplier");
                            ddlSupplier1.Items.Add(new ListItem(w.supplier, suppId.ToString()));
                        }
                        ddlSupplier.Items.FindByText(w.supplier).Selected = true;
                    }

                    i++;

                    //ddlTestedAt.SelectedValue = Convert.ToInt32(w.CTINWD_TestedAt_bit).ToString();

                }
            }
            int count = grdAACInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
           // UC_InwardHeader1.EnquiryNo = "";
            UC_InwardHeader1.TotalQty = qty.ToString();
        }

        protected void lnkTemp_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = true;
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
                for (int i = 0; i < grdAACInward.Rows.Count; i++)
                {


                    TextBox box2 = (TextBox)grdAACInward.Rows[i].Cells[1].FindControl("txtIdMark");
                    TextBox Qty = (TextBox)grdAACInward.Rows[i].Cells[2].FindControl("txtCSQty");
                    DropDownList ddlTest = (DropDownList)grdAACInward.Rows[i].Cells[3].FindControl("ddl_Test");
                    TextBox box5 = (TextBox)grdAACInward.Rows[i].Cells[4].FindControl("txtDescription");
                    DropDownList ddlSupplier = (DropDownList)grdAACInward.Rows[i].Cells[5].FindControl("ddlSupplier");
                    if (box2.Text == "")
                    {
                        lblMsg.Text = "Enter Id Mark for Sr No. " + (i + 1) + ".";
                        box2.Focus();
                        valid = false;
                        break;
                    }
                    else if (Qty.Text == "")
                    {
                        lblMsg.Text = "Enter Quantity for Sr No. " + (i + 1) + ".";
                        Qty.Focus();
                        valid = false;
                        break;
                    }
                    else if (Convert.ToInt32(Qty.Text) <= 0)
                    {
                        lblMsg.Text = "Quantity should be greater than zero for Sr No. " + (i + 1) + " .";
                        Qty.Focus();
                        valid = false;
                        break;
                    }
                    if (ddlTest.Text == "" || ddlTest.SelectedItem.Text == "---Select---")
                    {
                        lblMsg.Text = "Select Test for Sr No. " + (i + 1) + ".";
                        box5.Focus();
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


                    sumQty += Convert.ToInt32(Qty.Text);
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
        public void DisplayAACTest()
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
                ////UC_InwardHeader1.ProposalRateMatch = true;
                string CollectionTime = n.INWD_CollectionTime_time.ToString();
                var timespan = TimeSpan.Parse(CollectionTime);
                var output = new DateTime().Add(timespan).ToString("hh:mm tt");
                UC_InwardHeader1.CollectionTime = output.ToString();
                UC_InwardHeader1.EnquiryNo = n.INWD_ENQ_Id.ToString();
                recdDate = Convert.ToDateTime(n.INWD_ReceivedDate_dt);
                if (n.INWD_RptCL_Id != null)
                    lblRptClientId.Text = n.INWD_RptCL_Id.ToString();
                if (n.INWD_RptSITE_Id != null)
                    lblRptSiteId.Text = n.INWD_RptSITE_Id.ToString();
               // //UC_InwardHeader1.ProposalRateMatch = true;
            }
            AACTestgrid();
        }


        public void AACTestgrid()
        {
            int i = 0; //int li = 0;
            var res = dc.AllInward_View("AAC", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var w in res)
            {
                AddRowAACInward();

                TextBox IdMark = (TextBox)grdAACInward.Rows[i].Cells[1].FindControl("txtIdMark");
                TextBox Quantity = (TextBox)grdAACInward.Rows[i].Cells[2].FindControl("txtCSQty");
                DropDownList ddl_Test = (DropDownList)grdAACInward.Rows[i].Cells[3].FindControl("ddl_Test");
                TextBox Description = (TextBox)grdAACInward.Rows[i].Cells[4].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdAACInward.Rows[i].Cells[5].FindControl("ddlSupplier");

                IdMark.Text = w.AACINWD_Id_Mark_var.ToString();
                Quantity.Text = w.AACINWD_Quantity_tint.ToString();
                Description.Text = w.AACINWD_Description_var.ToString();
                ddl_Test.SelectedValue = w.AACINWD_TEST_Id.ToString();

                if (ddlSupplier.Items.FindByText(w.AACINWD_SupplierName_var) != null)
                    ddlSupplier.Items.FindByText(w.AACINWD_SupplierName_var).Selected = true;

                i++;

                ddlTestedAt.SelectedValue = Convert.ToInt32(w.CTINWD_TestedAt_bit).ToString();

            }
            int count = grdAACInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.EnquiryNo = "";

        }


        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            string reportStr = "";
            InwardReport rpt = new InwardReport();
            if (UC_InwardHeader1.RecordNo != "" && UC_InwardHeader1.ReferenceNo != "")
            {
                reportStr = rpt.getDetailReportAACInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
            }
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("AAC_Inward", reportStr);
        }

        bool buttonClicked = false;
        protected string getDetailReportAACInward()
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
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Compressive Strength Of Autoclaved Aerated Cellular Concrete Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Compressive Strength Of Autoclaved Aerated Cellular Concrete Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("AAC", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }
            var b = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, "AAC", null, null);

            foreach (var nt in b)
            {
                if (buttonClicked == true)
                {
                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "AAC" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "AAC" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + nt.INWD_BILL_Id + "</font></td>" +
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
                        "<td height=19><font size=2>" + "AAC" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "AAC" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.SITE_Address_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + nt.INWD_BILL_Id + "</font></td>" +
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

                    mySql += "<tr>" +

                     "<td align=left valign=top height=19><font size=2><b>Site Address</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + nt.SITE_Address_var + "</font></td>" +
                     "</tr>";
                }
            }

            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";



            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Test to be done</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("AAC", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.AACINWD_Id_Mark_var + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.AACINWD_Quantity_tint + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.AACINWD_Description_var + "</font></td>";
                int TestId = 0;
                TestId = Convert.ToInt32(c.AACINWD_TEST_Id.ToString());

                var Testtype = dc.Test_View(0, Convert.ToInt32(c.AACINWD_TEST_Id.ToString()), "", 0, 0, 0);
                foreach (var t in Testtype)
                {
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + t.TEST_Name_var.ToString() + "</font></td>";
                }

                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.AACINWD_SupplierName_var + "</font></td>";
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
            mySql += "</table>";


            mySql += "</html>";
            return reportStr = mySql;
        }

        protected void lnkLabSheet_Click(object sender, EventArgs e)
        {
            buttonClicked = true;
            string reportStr = "";
            InwardReport rpt = new InwardReport();
            if (UC_InwardHeader1.RecordNo != "" && UC_InwardHeader1.ReferenceNo != "")
            {
                //  reportStr = rpt.getDetailReportAACInwardLabSheet(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo));
                reportStr = rpt.getDetailReportAACInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), true);

            }
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("AAC_LabSheet", reportStr);
        }
        protected void lnkBillPrint_Click(object sender, EventArgs e)
        {
            buttonClicked = true;
            //ProformaInvoiceUpdation Proinv = new ProformaInvoiceUpdation();
            //Proinv.getProformaInvoicePrintString(UC_InwardHeader1.ProformaInvoiceNo, "Print");
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
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "AAC")));
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