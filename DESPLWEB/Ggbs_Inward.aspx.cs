using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class Ggbs_Inward : System.Web.UI.Page
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
                    if (strReq.Contains("=") == true)
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
                lblheading.Text = "GGBS Inward";                
                LoadSupplierList();
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    AddRowGgbsInward();
                }
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    DisplayGgbsDetails();
                }
                DisplayGgbsTest();
                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowGgbsInward();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "GGBS";
                }
                lnkSave.Visible = true;
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
        public void DisplayGgbsDetails()
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
            //GgbsInwardgrid();
            int i = 0;
            var res = dc.AllInward_View("GGBS", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var w in res)
            {
                AddRowGgbsInward();
                TextBox txtGgbsName = (TextBox)grdGgbsInward.Rows[i].Cells[1].FindControl("txtGgbsName");
                TextBox txtCementName = (TextBox)grdGgbsInward.Rows[i].Cells[1].FindControl("txtCementName");
                TextBox txtQty = (TextBox)grdGgbsInward.Rows[i].Cells[2].FindControl("txtQty");
                txtGgbsName.Text = w.GGBSINWD_GgbsName_var.ToString();
                txtCementName.Text = w.GGBSINWD_CementName_var.ToString();
                txtQty.Text = w.GGBSINWD_Quantity_tint.ToString();

                i++;
            }
            //Testgrid();
            i = 0;
            var res1 = dc.AllInward_View("GGBS", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var w in res1)
            {
                TextBox txtDescription = (TextBox)grdGgbsInward.Rows[i].Cells[3].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdGgbsInward.Rows[i].Cells[4].FindControl("ddlSupplier");
                Boolean valid = false;
                string ReferenceNo = w.GGBSINWD_ReferenceNo_var.ToString();
                var wttest = dc.AllInward_View("GGBS", 0, ReferenceNo);
                foreach (var wt in wttest)
                {
                    var c = dc.Test_View(0, Convert.ToInt32(wt.GGBSTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var n in c)
                    {
                        string TEST_Name_var = n.TEST_Name_var.ToString();
                        TextBox txtTestDetails = (TextBox)grdGgbsInward.Rows[i].Cells[5].FindControl("txtTestDetails");
                        if (TEST_Name_var == "Compressive Strength" || TEST_Name_var == "Slag activity index")
                        {
                            if (wt.GGBSTEST_Days_tint.ToString() != "" && wt.GGBSTEST_Days_tint.ToString() != null && wt.GGBSTEST_Days_tint.ToString() != "0")
                            {
                                txtTestDetails.Text = txtTestDetails.Text + TEST_Name_var.ToString() + "|" + wt.GGBSTEST_Days_tint.ToString() + ",";
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
                txtDescription.Text = w.GGBSINWD_Description_var.ToString();
                if (ddlSupplier.Items.FindByText(w.GGBSINWD_SupplierName_var) != null)
                {
                    ddlSupplier.SelectedValue = ddlSupplier.Items.FindByText(w.GGBSINWD_SupplierName_var).Value;
                }
                i++;
            }
            int count = grdGgbsInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.EnquiryNo = "";
        }
        public void DisplayGgbsTest()
        {
            int MaterialId = 0;
            var InwardId = dc.Material_View("GGBS", "");
            foreach (var n in InwardId)
            {
                MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
            }
            int row = 0;
            var a = dc.Test_View(MaterialId, 0, "", 0, 0, 0);
            foreach (var ggbsTest in a)
            {
                AddRowGgbsTest();
                Label lblTest = (Label)grdTestDetails.Rows[row].Cells[0].FindControl("lblTest");                
                lblTest.Text = ggbsTest.TEST_Name_var.ToString();
                row++;
            }
            CountRow();
        }
        protected void grdGgbsInward_RowDataBound(object sender, GridViewRowEventArgs e)
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

        #region Ggbs inward grid
        protected void AddRowGgbsInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            if (ViewState["GgbsInwardTable"] != null)
            {
                GetCurrentDataGgbsInward();
                dt = (DataTable)ViewState["GgbsInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtGgbsName", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCementName", typeof(string)));
                dt.Columns.Add(new DataColumn("txtQty", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));
                dt.Columns.Add(new DataColumn("txtTestDetails", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtGgbsName"] = string.Empty;
            dr["txtCementName"] = string.Empty;
            dr["txtQty"] = string.Empty;
            dr["txtDescription"] = string.Empty;
            dr["ddlSupplier"] = string.Empty;
            dr["txtTestDetails"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["GgbsInwardTable"] = dt;
            grdGgbsInward.DataSource = dt;
            grdGgbsInward.DataBind();
            SetPreviousGgbsInward();

        }
        protected void DeleteRowGgbsInward(int rowIndex)
        {
            GetCurrentDataGgbsInward();
            DataTable dt = ViewState["GgbsInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["GgbsInwardTable"] = dt;
            grdGgbsInward.DataSource = dt;
            grdGgbsInward.DataBind();
            SetPreviousGgbsInward();
        }
        protected void SetPreviousGgbsInward()
        {
            DataTable dt = (DataTable)ViewState["GgbsInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtGgbsName = (TextBox)grdGgbsInward.Rows[i].Cells[1].FindControl("txtGgbsName");
                TextBox txtCementName = (TextBox)grdGgbsInward.Rows[i].Cells[1].FindControl("txtCementName");
                TextBox txtQty = (TextBox)grdGgbsInward.Rows[i].Cells[2].FindControl("txtQty");
                TextBox txtDescription = (TextBox)grdGgbsInward.Rows[i].Cells[3].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdGgbsInward.Rows[i].Cells[4].FindControl("ddlSupplier");
                TextBox txtTestDetails = (TextBox)grdGgbsInward.Rows[i].Cells[5].FindControl("txtTestDetails");

                grdGgbsInward.Rows[i].Cells[0].Text = (i + 1).ToString();
                txtGgbsName.Text = dt.Rows[i]["txtGgbsName"].ToString();
                txtCementName.Text = dt.Rows[i]["txtCementName"].ToString();
                txtQty.Text = dt.Rows[i]["txtQty"].ToString();
                txtDescription.Text = dt.Rows[i]["txtDescription"].ToString();
                ddlSupplier.SelectedValue = dt.Rows[i]["ddlSupplier"].ToString();
                txtTestDetails.Text = dt.Rows[i]["txtTestDetails"].ToString();
            }
        }
        protected void GetCurrentDataGgbsInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtGgbsName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtCementName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtQty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtTestDetails", typeof(string)));
            for (int i = 0; i < grdGgbsInward.Rows.Count; i++)
            {
                TextBox txtGgbsName = (TextBox)grdGgbsInward.Rows[i].Cells[1].FindControl("txtGgbsName");
                TextBox txtCementName = (TextBox)grdGgbsInward.Rows[i].Cells[1].FindControl("txtCementName");
                TextBox txtQty = (TextBox)grdGgbsInward.Rows[i].Cells[2].FindControl("txtQty");
                TextBox txtDescription = (TextBox)grdGgbsInward.Rows[i].Cells[3].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdGgbsInward.Rows[i].Cells[4].FindControl("ddlSupplier");
                TextBox txtTestDetails = (TextBox)grdGgbsInward.Rows[i].Cells[5].FindControl("txtTestDetails");
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txtGgbsName"] = txtGgbsName.Text;
                drRow["txtCementName"] = txtCementName.Text;
                drRow["txtQty"] = txtQty.Text;
                drRow["txtDescription"] = txtDescription.Text;
                drRow["ddlSupplier"] = ddlSupplier.SelectedValue;
                drRow["txtTestDetails"] = txtTestDetails.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["GgbsInwardTable"] = dtTable;

        }
        #endregion

        protected void TextChanged(object sender, EventArgs e)
        {
            TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
            if (txtSubsets.Text != "")
            {
                if (Convert.ToInt32(txtSubsets.Text) < grdGgbsInward.Rows.Count)
                {
                    for (int i = grdGgbsInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdGgbsInward.Rows.Count > 1)
                        {
                            DeleteRowGgbsInward(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdGgbsInward.Rows.Count)
                {
                    for (int i = grdGgbsInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
                    {
                        AddRowGgbsInward();
                    }
                }
            }
        }
        protected void Click(object sender, EventArgs e)
        {
            ViewState["GgbsInwardTable"] = null;
            if (UC_InwardHeader1.EnquiryNo != "" && UC_InwardHeader1.EnqNoApp != "" && UC_InwardHeader1.EnqNoApp != "0")
                gridAppData();
            else
                AddRowGgbsInward();
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

            var res1 = dc.TestRequestDetails_View_Inward(Convert.ToInt32(UC_InwardHeader1.EnqNoApp), Convert.ToInt32(UC_InwardHeader1.MaterialId), 0, "GGBS").ToList();
            foreach (var n in res1)
            {
                testName = "";
                AddRowGgbsInward();
                TextBox txtGgbsName = (TextBox)grdGgbsInward.Rows[i].Cells[1].FindControl("txtGgbsName");
                TextBox txtCementName = (TextBox)grdGgbsInward.Rows[i].Cells[1].FindControl("txtCementName");
                TextBox txtQty = (TextBox)grdGgbsInward.Rows[i].Cells[2].FindControl("txtQty");
                TextBox txtDescription = (TextBox)grdGgbsInward.Rows[i].Cells[3].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdGgbsInward.Rows[i].Cells[4].FindControl("ddlSupplier");
                TextBox txtTestDetails = (TextBox)grdGgbsInward.Rows[i].Cells[5].FindControl("txtTestDetails");

                txtGgbsName.Text = Convert.ToString(n.Idmark1);
                //txtCementName.Text = Convert.ToString(n.Idmark1);
                txtQty.Text = n.specimen.ToString();
                if (n.specimen != "" && n.specimen != null)
                    qty += Convert.ToInt32(n.specimen);
                if (ddlSupplier.Items.FindByText(Convert.ToString(n.supplier)) != null)
                    ddlSupplier.Items.FindByText(Convert.ToString(n.supplier)).Selected = true;
                else
                {
                    int suppId = dc.Supplier_Update(n.supplier, true);
                    for (int j = 0; j < grdGgbsInward.Rows.Count; j++)
                    {
                        DropDownList ddlSupplier1 = (DropDownList)grdGgbsInward.Rows[j].FindControl("ddlSupplier");
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
                    testName += t.test_name + ",";
                }
                txtTestDetails.Text = testName;


                i++;
            }
            int count = grdGgbsInward.Rows.Count;
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
                bool updateFlag = true;
                string RefNo, SetOfRecord;
                string totalCost = "0";
                clsData clsObj = new clsData();
                int SrNo;
                DateTime ReceivedDate = DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null);
                DateTime d1 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    dc.Inward_Update_2(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "GGBS", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, UC_InwardHeader1.POFileName, false, 0, true);                    
                }
                else
                {
                    try
                    { 
                        UC_InwardHeader1.RecordNo = clsObj.GetnUpdateRecordNo("GGBS").ToString();
                        UC_InwardHeader1.ReferenceNo = clsObj.insertRecordTable_2(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), Convert.ToInt32(UC_InwardHeader1.RecordNo), "ST").ToString();
                        dc.Inward_Update_2(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "GGBS", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, UC_InwardHeader1.POFileName, UC_InwardHeader1.OtherClient, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
                    }
                    catch
                    {
                        updateFlag = false;
                    }
                }
                if (updateFlag == true)
                {
                    dc.GgbsInward_Update("GGBS", Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.ReferenceNo + "/%", "", 0, 0, "", "", 0, "", "","", null, null, "", "", 0, 0, true);
                    for (int i = 0; i <= grdGgbsInward.Rows.Count - 1; i++)
                    {
                        TextBox txtGgbsName = (TextBox)grdGgbsInward.Rows[i].Cells[1].FindControl("txtGgbsName");
                        TextBox txtCementName = (TextBox)grdGgbsInward.Rows[i].Cells[1].FindControl("txtCementName");
                        TextBox txtQty = (TextBox)grdGgbsInward.Rows[i].Cells[2].FindControl("txtQty");
                        TextBox txtDescription = (TextBox)grdGgbsInward.Rows[i].Cells[3].FindControl("txtDescription");
                        DropDownList ddlSupplier = (DropDownList)grdGgbsInward.Rows[i].Cells[4].FindControl("ddlSupplier");
                        TextBox txtTestDetails = (TextBox)grdGgbsInward.Rows[i].Cells[5].FindControl("txtTestDetails");

                        SrNo = Convert.ToInt32(grdGgbsInward.Rows[i].Cells[0].Text);
                        RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                        SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;

                        dc.GgbsInward_Update("GGBS", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txtQty.Text), txtDescription.Text, ddlSupplier.SelectedItem.Text, 0, "", txtGgbsName.Text,txtCementName.Text, d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), false);
                        dc.ClientSupplier_Update(Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(ddlSupplier.SelectedValue), Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, ReceivedDate, false);
                        dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "GGBS", RefNo, "GGBS", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);

                        string[] testList = txtTestDetails.Text.Split(',');
                        for (int j = 0; j < testList.Count()-1; j++)
                        {
                            string[] testName = testList[j].Split('|');
                            var test = dc.Test(0, "", 0, "GGBS", testName[0], 0);
                            foreach (var t in test)
                            {
                                byte days = 0;
                                if (testName[0] == "Compressive Strength" || testName[0] == "Slag activity index")
                                {
                                    days =  Convert.ToByte(testName[1]);
                                }
                                dc.GgbsTest_Update(RefNo, Convert.ToInt32(t.TEST_Id), days, Convert.ToByte(t.TEST_Sr_No), false);
                            }
                        }
                    }
                    UC_InwardHeader1.TestReqFormNo = "Test Request Form No : " + UC_InwardHeader1.ReferenceNo + "/" + DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null).Year.ToString().Substring(2, 2);
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
                                NewrecNo = clsObj.GetCurrentRecordNo("BillNo");
                                var gstbillCount = dc.Bill_View_Count(DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                                if (gstbillCount.Count() != NewrecNo - 1)
                                {
                                    updateBillFlag = false;
                                    lblMsg.Text = "Record Saved Successfully, Bill No. mismatch. Can not generate bill.";
                                }
                            }
                        }
                        if (updateBillFlag == true)
                        {
                            BillNo = bill.UpdateBill("GGBS", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
                        }
                        UC_InwardHeader1.BillNo = BillNo.ToString();
                        //
                        if (BillNo != "0")
                            lnkBillPrint.Visible = true;
                    }
                    
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

                    UC_InwardHeader1.EnquiryNo = "";
                    DropDownList ddlEnquiryList = (DropDownList)UC_InwardHeader1.FindControl("ddlEnquiryList");
                    ddlEnquiryList.Items.Remove(ddlEnquiryList.SelectedItem.Value);
                    ddlEnquiryList.ClearSelection();
                }
                else
                {
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Text = "Please try again...";
                    lblMsg.Visible = true;
                }
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
                for (int i = 0; i <= grdGgbsInward.Rows.Count - 1; i++)
                {
                    TextBox txtGgbsName = (TextBox)grdGgbsInward.Rows[i].Cells[1].FindControl("txtGgbsName");
                    TextBox txtCementName = (TextBox)grdGgbsInward.Rows[i].Cells[1].FindControl("txtCementName");
                    TextBox txtQty = (TextBox)grdGgbsInward.Rows[i].Cells[2].FindControl("txtQty");
                    TextBox txtDescription = (TextBox)grdGgbsInward.Rows[i].Cells[3].FindControl("txtDescription");
                    DropDownList ddlSupplier = (DropDownList)grdGgbsInward.Rows[i].Cells[4].FindControl("ddlSupplier");
                    TextBox txtTestDetails = (TextBox)grdGgbsInward.Rows[i].Cells[5].FindControl("txtTestDetails");
                    if (txtGgbsName.Text == "")
                    {
                        lblMsg.Text = "Enter GGBS Name for Sr No. " + (i + 1) + ".";
                        txtGgbsName.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtCementName.Text == "")
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
        protected void lnkRateList_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender1.Show();
            //gridClear();
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                cbxSelect.Checked = false;
                txtDAYS.Text = "";
            }
            
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            TextBox txtTestDetails = (TextBox)clickedRow.FindControl("txtTestDetails");
            if (txtTestDetails.Text != "")
            {
                string[] testList = txtTestDetails.Text.Split(',');
                for (int i = 0; i < testList.Count() - 1; i++)
                {
                    string[] testName = testList[i].Split('|');
                    bool testFound = false;
                    for (int j = 0; j < grdTestDetails.Rows.Count; j++)
                    {
                        Label lblTest = (Label)grdTestDetails.Rows[j].Cells[0].FindControl("lblTest");
                        TextBox txtDAYS = (TextBox)grdTestDetails.Rows[j].Cells[1].FindControl("txtDAYS");
                        CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[j].Cells[2].FindControl("cbxSelect");                        
                        if (lblTest.Text == testName[0])
                        {
                            if (testName[0] == "Compressive Strength" || testName[0] == "Slag activity index")
                            {
                                if (txtDAYS.Text == "")
                                {
                                    txtDAYS.Text = testName[1];
                                    cbxSelect.Checked = true;
                                    testFound = true;
                                    break;
                                }                                
                            }
                            else
                            {
                                cbxSelect.Checked = true;
                                testFound = true;
                                break;
                            }
                        }
                    }
                    if (testFound == false && (testName[0] == "Compressive Strength" || testName[0] == "Slag activity index"))
                    {
                        AddRowGgbsTest();
                        Label lblTest = (Label)grdTestDetails.Rows[grdTestDetails.Rows.Count-1].FindControl("lblTest");
                        lblTest.Text = testName[0];
                        CountRow();
                        TextBox txtDAYS = (TextBox)grdTestDetails.Rows[grdTestDetails.Rows.Count - 1].FindControl("txtDAYS");
                        CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[grdTestDetails.Rows.Count - 1].FindControl("cbxSelect");
                        cbxSelect.Checked = true;
                        txtDAYS.Text = testName[1];
                    }
                }   
            }
        }                
        protected void imgCloseTestDetails_Click(object sender, ImageClickEventArgs e)
        {
            lbl_ErrMsg.Visible = false;
            if (ValidateDays() == true && ValidateCompStrSlagActivity() == true)
            {
                ValidateCheckInitial_Final();
                TextBox txtTestDetails = grdGgbsInward.SelectedRow.Cells[5].FindControl("txtTestDetails") as TextBox;
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

        protected Boolean ValidateDays()
        {
            Boolean valid = true;
            lbl_ErrMsg.Visible = false;
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                if (cbxSelect.Checked == true && txtDAYS.Text == "" &&
                    (lblTest.Text == "Compressive Strength" || lblTest.Text == "Slag activity index"))
                {
                    lbl_ErrMsg.Visible = true;
                    lbl_ErrMsg.Text = "Please Enter Days of " + lblTest.Text + " Otherwise Uncheck it ";                    
                    valid = false;
                    break;
                }
            }
            return valid;
        }
        protected Boolean ValidateSoundness()
        {
            Boolean valid = true;
            lbl_ErrMsg.Visible = false;
            int j = 0;
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");

                if (cbxSelect.Checked == true)
                {
                    if (lblTest.Text == "Soundness By Le-Chateliers Apparatus")
                    {
                        j++;
                    }
                    if (lblTest.Text == "Soundness By AutoClave")
                    {
                        j++;
                    }
                }             
            }
            if (j == 2)
            {
                valid = false;
                lbl_ErrMsg.Visible = true;
                lbl_ErrMsg.Text = "Please Select either Soundness By Le-Chateliers Apparatus or  Soundness By Autoclave ";
            }            
            return valid;

        }
        protected Boolean ValidateFineness()
        {
            Boolean valid = true;
            lbl_ErrMsg.Visible = false;
            int j = 0;
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                if (cbxSelect.Checked == true)
                {
                    if (lblTest.Text == "Fineness By Dry Seiving")
                    {
                        j++;
                    }
                    if (lblTest.Text == "Fineness By Blain`s Air Permeability Method")
                    {                        
                        j++;
                    }
                }
            }
            if (j == 2)
            {
                valid = false;
                lbl_ErrMsg.Visible = true;
                lbl_ErrMsg.Text = "Please Select either Fineness By Dry Seiving or Fineness By Blain`s Air Permeability Method";
            }            
            return valid;

        }
        protected Boolean ValidateCompStrSlagActivity()
        {
            Boolean valid = true;
            lbl_ErrMsg.Visible = false;
            bool compStr = false, SlagActivity = false;
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                if (cbxSelect.Checked == true)
                {
                    if (lblTest.Text == "Compressive Strength")
                    {
                        compStr = true;
                    }
                    else if (lblTest.Text == "Slag activity index")
                    {
                        SlagActivity = true;
                    }
                }
            }
            if (compStr == true && SlagActivity == true)
            {
                valid = false;
                lbl_ErrMsg.Visible = true;
                lbl_ErrMsg.Text = "Please Select either Compressive Strength or Slag activity index";
            }            
            return valid;

        }
        protected void ValidateCheckInitial_Final()
        {
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                if (cbxSelect.Checked == true)
                {
                    if (lblTest.Text == "Initial Setting Time")
                    {
                        CheckBox cbxSelectt = (CheckBox)grdTestDetails.Rows[i + 1].Cells[2].FindControl("cbxSelect");
                        cbxSelectt.Checked = true;
                    }

                    if (lblTest.Text == "Final Setting Time")
                    {
                        CheckBox cbxSelectt = (CheckBox)grdTestDetails.Rows[i - 1].Cells[2].FindControl("cbxSelect");
                        cbxSelectt.Checked = true;
                    }
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
                LinkButton lnkAddRowTest = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkAddRowTest");
                LinkButton lnkDeleteRowTest = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkDeleteRowTest");
                //if (i == grdTestDetails.Rows.Count - 1 
                if (lblTest.Text == "Compressive Strength" || lblTest.Text == "Slag activity index")
                {
                    lblDays.Visible = true;
                    txtDAYS.Visible = true;
                    lnkAddRowTest.Text = "+";
                    lnkAddRowTest.Font.Bold = true;
                    lnkAddRowTest.Font.Size = 14;
                    lnkDeleteRowTest.Text = "X";
                    lnkDeleteRowTest.Font.Bold = true;
                    lnkDeleteRowTest.Font.Size = 10;
                    lnkAddRowTest.Visible = true;
                    lnkDeleteRowTest.Visible = true;
                    //lblTest.Text = "Compressive Strength";
                }
                else
                {
                    lblDays.Visible = false;
                    txtDAYS.Visible = false;
                    lnkAddRowTest.Visible = false;
                    lnkDeleteRowTest.Visible = false;
                }

            }
        }

        #region ggbs test rows
        protected void lnkAddRowTest_Click(object sender, CommandEventArgs e)
        {
            AddRowGgbsTest();
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            Label lblTest = (Label)gvr.FindControl("lblTest");
            //gvr.RowIndex
            if (lblTest.Text == "Compressive Strength" || lblTest.Text == "Slag activity index")
            {
                Label lblTestAdded = (Label)grdTestDetails.Rows[grdTestDetails.Rows.Count-1].FindControl("lblTest");
                lblTestAdded.Text = lblTest.Text;
            }
            CountRow();
        }
        protected void lnkDeleteRowTest_Click(object sender, CommandEventArgs e)
        {
            if (grdTestDetails.Rows.Count > 1)
            {
                LinkButton btn = (LinkButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                Label lblTest = (Label)gvr.FindControl("lblTest");
                int cntTest = 0;
                for (int i = 0; i < grdTestDetails.Rows.Count; i++)
                {
                    Label lblTestFind = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                    if (lblTest.Text == lblTestFind.Text)
                    {
                        cntTest++;
                        if (cntTest > 1)
                            break;
                    }
                }                
                if (cntTest > 1)
                {
                    DeleteRowGgbsTest(gvr.RowIndex);
                    CountRow();
                }                
            }
        }
        protected void AddRowGgbsTest()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            if (ViewState["TestTable"] != null)
            {
                GetCurrentDataGgbsTest();
                dt = (DataTable)ViewState["TestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblTest", typeof(string)));
                dt.Columns.Add(new DataColumn("lblDays", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDAYS", typeof(string)));
                dt.Columns.Add(new DataColumn("lnkAddRowTest", typeof(string)));
                dt.Columns.Add(new DataColumn("lnkDeleteRowTest", typeof(string)));
                dt.Columns.Add(new DataColumn("lblTestId", typeof(string)));
                dt.Columns.Add(new DataColumn("cbxSelect", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblTest"] = string.Empty;
            dr["lblDays"] = "Days";
            dr["txtDAYS"] = string.Empty;
            dr["lnkAddRowTest"] = string.Empty;
            dr["lnkDeleteRowTest"] = string.Empty;
            dr["lblTestId"] = string.Empty;
            dr["cbxSelect"] = false;

            dt.Rows.Add(dr);
            ViewState["TestTable"] = dt;
            grdTestDetails.DataSource = dt;
            grdTestDetails.DataBind();
            SetPreviousGgbsTest();

        }
        protected void GetCurrentDataGgbsTest()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow drRow = null;
            dt.Columns.Add(new DataColumn("lblTest", typeof(string)));
            dt.Columns.Add(new DataColumn("lblDays", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDAYS", typeof(string)));
            dt.Columns.Add(new DataColumn("lnkAddRowTest", typeof(string)));
            dt.Columns.Add(new DataColumn("lnkDeleteRowTest", typeof(string)));
            dt.Columns.Add(new DataColumn("lblTestId", typeof(string)));
            dt.Columns.Add(new DataColumn("cbxSelect", typeof(string)));

            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                {
                    Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                    Label lblTestId = (Label)grdTestDetails.Rows[i].Cells[2].FindControl("lblTestId");
                    Label lblDays = (Label)grdTestDetails.Rows[i].Cells[1].FindControl("lblDays");
                    TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                    LinkButton lnkAddRowTest = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkAddRowTest");
                    LinkButton lnkDeleteRowTest = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkDeleteRowTest");
                    CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");

                    drRow = dt.NewRow();
                    drRow["lblTest"] = lblTest.Text;
                    drRow["lblDays"] = "Days";
                    drRow["txtDAYS"] = txtDAYS.Text;
                    drRow["lnkAddRowTest"] = lnkAddRowTest.Text;
                    drRow["lnkDeleteRowTest"] = lnkDeleteRowTest.Text;
                    drRow["cbxSelect"] = cbxSelect.Checked;
                    dt.Rows.Add(drRow);
                    rowIndex++;
                }
                ViewState["TestTable"] = dt;
            }
        }
        protected void SetPreviousGgbsTest()
        {
            DataTable dt = (DataTable)ViewState["TestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                Label lblTestId = (Label)grdTestDetails.Rows[i].Cells[2].FindControl("lblTestId");
                Label lblDays = (Label)grdTestDetails.Rows[i].Cells[1].FindControl("lblDays");
                TextBox txtDAYS = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtDAYS");
                LinkButton lnkAddRowTest = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkAddRowTest");
                LinkButton lnkDeleteRowTest = (LinkButton)grdTestDetails.Rows[i].Cells[1].FindControl("lnkDeleteRowTest");
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                grdTestDetails.Rows[i].Cells[2].Text = (i + 1).ToString();

                lblTest.Text = dt.Rows[i]["lblTest"].ToString();
                lblTestId.Text = dt.Rows[i]["lblTestId"].ToString();
                lblDays.Text = dt.Rows[i]["lblDays"].ToString();
                txtDAYS.Text = dt.Rows[i]["txtDAYS"].ToString();
                lnkAddRowTest.Text = dt.Rows[i]["lnkAddRowTest"].ToString();
                lnkDeleteRowTest.Text = dt.Rows[i]["lnkDeleteRowTest"].ToString();
                cbxSelect.Checked = Convert.ToBoolean(dt.Rows[i]["cbxSelect"].ToString());
            }

        }
        protected void DeleteRowGgbsTest(int rowIndex)
        {
            GetCurrentDataGgbsTest();
            DataTable dt = ViewState["TestTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["TestTable"] = dt;
            grdTestDetails.DataSource = dt;
            grdTestDetails.DataBind();
            SetPreviousGgbsTest();
        }
        #endregion
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            string reportStr = "";
            InwardReport rpt = new InwardReport();
            if (UC_InwardHeader1.RecordNo != "" && UC_InwardHeader1.ReferenceNo != "")
            {
                reportStr = rpt.getDetailReportGgbsInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
            }
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Ggbs_Inward", reportStr);
        }                
        protected void lnkLabSheet_Click(object sender, EventArgs e)
        {
            string reportStr = "";
            InwardReport rpt = new InwardReport();
            if (UC_InwardHeader1.RecordNo != "" && UC_InwardHeader1.ReferenceNo != "")
            {
                reportStr = rpt.getDetailReportGgbsInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), true);
            }
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Ggbs_LabSheet", reportStr);
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
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "GGBS")));
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
                PrintGrid.Redirect1(strURLWithData, "_blank", "menubar=1,HEIGHT=300,WIDTH=820,scrollbars=yes,resizable=yes");
            }
        }
    }
}