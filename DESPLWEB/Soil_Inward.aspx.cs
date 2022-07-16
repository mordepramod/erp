using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Soil_Inward : System.Web.UI.Page
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
                    lblheading.Text = "Soil Inward";
                }
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    AddRowSoilInward();
                    AddRowSoilOtherCharges();
                }

                if (UC_InwardHeader1.RecType != "")
                {
                    DisplaySolidTest();
                    DisplayOtherCharges();
                }
                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowSoilInward();
                    AddRowSoilOtherCharges();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "SO";
                }
                GridTestDetailsBind();
                lnkSave.Visible = true;
            }
        }
        protected void AddRowSoilInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            if (ViewState["SoilInwardTable"] != null)
            {
                GetCurrentDataSoilInward();
                dt = (DataTable)ViewState["SoilInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("txtTestDetails", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSupplierName", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtIdMark"] = string.Empty;
            dr["txtDescription"] = string.Empty;
            dr["txtTestDetails"] = string.Empty;
            dr["txtSupplierName"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["SoilInwardTable"] = dt;
            grdSoilInward.DataSource = dt;
            grdSoilInward.DataBind();
            SetPreviousSoilInward();

        }
        protected void DeleteRowSoilInward(int rowIndex)
        {
            GetCurrentDataSoilInward();
            DataTable dt = ViewState["SoilInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["SoilInwardTable"] = dt;
            grdSoilInward.DataSource = dt;
            grdSoilInward.DataBind();
            SetPreviousSoilInward();
        }
        protected void SetPreviousSoilInward()
        {
            DataTable dt = (DataTable)ViewState["SoilInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtIdMark = (TextBox)grdSoilInward.Rows[i].Cells[1].FindControl("txtIdMark");
                TextBox txtDescription = (TextBox)grdSoilInward.Rows[i].Cells[2].FindControl("txtDescription");
                TextBox txtSupplierName = (TextBox)grdSoilInward.Rows[i].Cells[3].FindControl("txtSupplierName");
                TextBox txtTestDetails = (TextBox)grdSoilInward.Rows[i].Cells[4].FindControl("txtTestDetails");

                grdSoilInward.Rows[i].Cells[0].Text = (i + 1).ToString();
                txtIdMark.Text = dt.Rows[i]["txtIdMark"].ToString();
                txtDescription.Text = dt.Rows[i]["txtDescription"].ToString();
                txtSupplierName.Text = dt.Rows[i]["txtSupplierName"].ToString();
                txtTestDetails.Text = dt.Rows[i]["txtTestDetails"].ToString();
            }
        }
        protected void GetCurrentDataSoilInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtTestDetails", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtSupplierName", typeof(string)));
            for (int i = 0; i < grdSoilInward.Rows.Count; i++)
            {
                TextBox txtIdMark = (TextBox)grdSoilInward.Rows[i].Cells[1].FindControl("txtIdMark");
                TextBox txtDescription = (TextBox)grdSoilInward.Rows[i].Cells[2].FindControl("txtDescription");
                TextBox txtSupplierName = (TextBox)grdSoilInward.Rows[i].Cells[3].FindControl("txtSupplierName");
                TextBox txtTestDetails = (TextBox)grdSoilInward.Rows[i].Cells[4].FindControl("txtTestDetails");

                drRow = dtTable.NewRow();
                drRow["txtIdMark"] = txtIdMark.Text;
                drRow["txtDescription"] = txtDescription.Text;
                drRow["txtTestDetails"] = txtTestDetails.Text;
                drRow["txtSupplierName"] = txtSupplierName.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["SoilInwardTable"] = dtTable;

        }
        protected void GridTestDetailsBind()
        {
            int MaterialId = 0;
            var InwardId = dc.Material_View("SO", "");
            foreach (var n in InwardId)
            {
                MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
            }
            var a = dc.Test_View(MaterialId, 0, "", 0, 0, 0);
            grdTestDetails.DataSource = a;
            grdTestDetails.DataBind();
            if (grdTestDetails.Rows.Count > 0)
            {
                EnableQty();
            }
        }
        protected void TextChanged(object sender, EventArgs e)
        {
            TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
            if (txtSubsets.Text != "")
            {
                if (Convert.ToInt32(txtSubsets.Text) < grdSoilInward.Rows.Count)
                {
                    for (int i = grdSoilInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdSoilInward.Rows.Count > 1)
                        {
                            DeleteRowSoilInward(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdSoilInward.Rows.Count)
                {
                    for (int i = grdSoilInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
                    {
                        AddRowSoilInward();
                    }
                }
            }

        }
        protected void Click(object sender, EventArgs e)
        {
            ddlTestedAt.SelectedIndex = 0;
            ViewState["SoilInwardTable"] = null;
             if (UC_InwardHeader1.EnquiryNo != "" && UC_InwardHeader1.EnqNoApp != "" && UC_InwardHeader1.EnqNoApp != "0")
            {
                SoilTestgridAppData();
            }
            else
            {
                AddRowSoilInward();
            }
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
        public void SoilTestgridAppData()
        {
            int i = 0, totalQty = 0;
            var res = dc.TestRequestDetails_View_Inward(Convert.ToInt32(UC_InwardHeader1.EnqNoApp), Convert.ToInt32(UC_InwardHeader1.MaterialId), 0, "SO").ToList();
            foreach (var s in res)
            {
                AddRowSoilInward();
                TextBox txtIdMark = (TextBox)grdSoilInward.Rows[i].Cells[1].FindControl("txtIdMark");
                TextBox txtDescription = (TextBox)grdSoilInward.Rows[i].Cells[2].FindControl("txtDescription");
                TextBox txtSupplierName = (TextBox)grdSoilInward.Rows[i].Cells[3].FindControl("txtSupplierName");
                TextBox txtTestDetails = (TextBox)grdSoilInward.Rows[i].Cells[4].FindControl("txtTestDetails");

                txtIdMark.Text = s.Idmark1;
                txtDescription.Text = s.description;
                if (s.make != null && s.make != "")
                    txtDescription.Text += ", Make - " + s.make.ToString();
                txtSupplierName.Text = s.supplier;

                var test = dc.TestRequestDetails_View_ForPrint(s.TestReqId);
                foreach (var t in test)
                {
                    if (t.test_name.Contains("Field Density by Sand") == true ||
                        t.test_name.Contains("Field Density by Core Cutting") == true ||
                        t.test_name.Contains("Classification") == true)
                    {
                        txtTestDetails.Text += t.test_name + "|" + s.specimen.ToString() + ",";
                        totalQty += Convert.ToInt32(s.specimen);
                    }
                    else
                    {
                        txtTestDetails.Text += t.test_name + ",";
                    }
                }
                i++;
            }
            if (totalQty > 0)
                UC_InwardHeader1.TotalQty = totalQty.ToString();
            UC_InwardHeader1.Subsets = grdSoilInward.Rows.Count.ToString();
        }

        #region SoilOtherCharges
        public void DisplayOtherCharges()
        {
            int i = 0;
            var re = dc.OtherCharges_View(UC_InwardHeader1.RecType, Convert.ToInt32(UC_InwardHeader1.RecordNo));
            foreach (var r in re)
            {
                AddRowSoilOtherCharges();
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
                AddRowSoilOtherCharges();
            }

        }
        protected void AddRowSoilOtherCharges()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["SoilOtherCharges_Table"] != null)
            {
                GetCurrentDataSoilOtherCharges();
                dt = (DataTable)ViewState["SoilOtherCharges_Table"];
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


            ViewState["SoilOtherCharges_Table"] = dt;
            grdOtherCharges.DataSource = dt;
            grdOtherCharges.DataBind();
            SetPreviousDataSoilOtherCharges();
        }
        protected void GetCurrentDataSoilOtherCharges()
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
            ViewState["SoilOtherCharges_Table"] = dtTable;

        }
        protected void SetPreviousDataSoilOtherCharges()
        {
            DataTable dt = (DataTable)ViewState["SoilOtherCharges_Table"];
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
        protected void DeleteRowSoilOtherCharges(int rowIndex)
        {
            GetCurrentDataSoilOtherCharges();
            DataTable dt = ViewState["SoilOtherCharges_Table"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["SoilOtherCharges_Table"] = dt;
            grdOtherCharges.DataSource = dt;
            grdOtherCharges.DataBind();
            SetPreviousDataSoilOtherCharges();
        }
        protected void imgInsert_Click(object sender, CommandEventArgs e)
        {
            AddRowSoilOtherCharges();
        }
        protected void imgDelete_Click(object sender, CommandEventArgs e)
        {
            if (grdOtherCharges.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdOtherCharges.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowSoilOtherCharges(gvr.RowIndex);
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
                    var inwd = dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "SO", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, true);
                    var res = dc.AllInward_View("SO", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
                    foreach (var a1 in res)
                    {
                        dc.SoilTest_Update(a1.SOINWD_ReferenceNo_var.ToString(), 0, 0, true);
                    }
                }
                else
                {
                    Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    NewrecNo = clsObj.GetnUpdateRecordNo("SO");
                    UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                    clsObj.insertRecordTable(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), NewrecNo, "SO");
                    UC_InwardHeader1.ReferenceNo = clsObj.getRefNo(Convert.ToInt32(UC_InwardHeader1.EnquiryNo)).ToString();
                    dc.Inward_Update_New(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "SO", 0, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate);

                    //Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    //NewrecNo = clsObj.GetnUpdateRecordNo("SO");
                    //UC_InwardHeader1.RecordNo = NewrecNo.ToString(); 
                    //UC_InwardHeader1.ReferenceNo = dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "SO", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, false).ToString();
                    ////var inward = dc.Inward_View(Convert.ToInt32(UC_InwardHeader1.ReferenceNo), 0, "");
                    ////foreach (var inwd in inward)
                    ////{
                    ////    UC_InwardHeader1.RecordNo = inwd.INWD_RecordNo_int.ToString();
                    ////}

                }
                dc.Enquiry_Update_Status(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 2);
                dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "SO", "", null, null, false, false, false, false, false, false, true);//Delete MIS Record

                dc.SoilInward_Update("SO", Convert.ToInt32(UC_InwardHeader1.RecordNo), "", "", 0, "", "", "", "", 0, 0, 0, 0, "", null, null, "", "", 0, 0, false, true);
                if (lblRptClientId.Text != "" && lblRptClientId.Text != "0")
                {
                    dc.Inward_Update_ReportClient(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, Convert.ToInt32(lblRptClientId.Text), Convert.ToInt32(lblRptSiteId.Text));
                }
                for (int i = 0; i <= grdSoilInward.Rows.Count - 1; i++)
                {

                    TextBox txtIdMark = (TextBox)grdSoilInward.Rows[i].Cells[1].FindControl("txtIdMark");
                    TextBox txtDescription = (TextBox)grdSoilInward.Rows[i].Cells[2].FindControl("txtDescription");
                    TextBox txtSupplierName = (TextBox)grdSoilInward.Rows[i].Cells[3].FindControl("txtSupplierName");
                    TextBox txtTestDetails = (TextBox)grdSoilInward.Rows[i].Cells[4].FindControl("txtTestDetails");

                    SrNo = Convert.ToInt32(grdSoilInward.Rows[i].Cells[0].Text);
                    RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;

                    dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "SO", RefNo, "SO", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);

                    byte NoOfpits = 0, NoOfCore = 0, Quantity = 0;
                    string[] line3 = txtTestDetails.Text.Split(',');
                    foreach (string line4 in line3)
                    {
                        string[] line8 = line4.Split('|');
                        foreach (string line in line8)
                        {
                            if (line.Contains("Field Density by Sand") == true)
                            {
                                string[] line6 = line4.Split('|');
                                foreach (string line7 in line6)
                                {
                                    if (line7.Contains("Field Density by Sand") ==false)
                                    {
                                        NoOfpits = Convert.ToByte(line7.ToString());
                                        break;
                                    }
                                }
                            }
                            if (line == "Field Density by Core Cutting")
                            {
                                string[] line12 = line4.Split('|');
                                foreach (string line7 in line12)
                                {
                                    if (line7 != "Field Density by Core Cutting")
                                    {
                                        NoOfCore = Convert.ToByte(line7.ToString());
                                        break;
                                    }
                                }
                            }
                            if (line == "Classification")
                            {
                                string[] line12 = line4.Split('|');
                                foreach (string line7 in line12)
                                {
                                    if (line7 != "Classification")
                                    {
                                        Quantity = Convert.ToByte(line7.ToString());
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    dc.SoilInward_Update("SO", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, txtDescription.Text, txtSupplierName.Text, "", txtIdMark.Text, NoOfpits, NoOfCore, Quantity, 0, "", d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false);
                    string[] lines = txtTestDetails.Text.Split(',');
                    foreach (string line in lines)
                    {
                        string[] line10 = line.Split('|', ',');
                        foreach (string line11 in line10)
                        {
                            var g = dc.Test(0, "", 0, "SO", line11, 0);
                            foreach (var n in g)
                            {
                                var t = dc.Test(Convert.ToInt32(n.TEST_Sr_No), "", 0, "SO", "", 0);
                                foreach (var ts in t)
                                {
                                    dc.SoilTest_Update(RefNo, Convert.ToInt32(ts.TEST_Id), 0, false);
                                }
                            }
                        }
                    }
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
                        BillNo = bill.UpdateBill("SO", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
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
                //        ProformaInvoiceNo = ProInv.UpdateProformaInvoice("SO", Convert.ToInt32(UC_InwardHeader1.RecordNo), ProformaInvoiceNo);
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
                for (int i = 0; i <= grdSoilInward.Rows.Count - 1; i++)
                {
                    TextBox txtIdMark = (TextBox)grdSoilInward.Rows[i].Cells[1].FindControl("txtIdMark");
                    TextBox txtDescription = (TextBox)grdSoilInward.Rows[i].Cells[2].FindControl("txtDescription");
                    TextBox txtSupplierName = (TextBox)grdSoilInward.Rows[i].Cells[3].FindControl("txtSupplierName");
                    TextBox txtTestDetails = (TextBox)grdSoilInward.Rows[i].Cells[4].FindControl("txtTestDetails");
                    if (txtIdMark.Text == "")
                    {
                        lblMsg.Text = "Enter Id Mark Details for row no " + (i + 1) + ".";
                        txtIdMark.Focus();
                        valid = false;
                        break;
                    }
                    if (txtDescription.Text == "")
                    {
                        lblMsg.Text = "Enter Description for row no " + (i + 1) + ".";
                        txtDescription.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtSupplierName.Text == "")
                    {
                        lblMsg.Text = "Enter Supplier Name for row no " + (i + 1) + ".";
                        txtSupplierName.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtTestDetails.Text == "")
                    {
                        lblMsg.Text = "Select Test Details for row no " + (i + 1) + ".";
                        txtTestDetails.Focus();
                        valid = false;
                        break;
                    }
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
            gridClear();
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            TextBox txtTestDetails = (TextBox)clickedRow.FindControl("txtTestDetails");
            if (txtTestDetails.Text != "")
            {
                for (int i = 0; i < grdTestDetails.Rows.Count; i++)
                {
                    bool valid = false;
                    TextBox txtTestQty = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtTestQty");
                    CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                    Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                    string[] lines = txtTestDetails.Text.Split(',');
                    foreach (string line in lines)
                    {
                        if (line == lblTest.Text)
                        {
                            cbxSelect.Checked = true;
                            break;
                        }
                        else
                        {
                            cbxSelect.Checked = false;
                        }
                        string[] line3 = line.Split('|');
                        foreach (string line4 in line3)
                        {
                            if (line4 == lblTest.Text)
                            {
                                string[] line6 = line.Split('|');
                                foreach (string line7 in line6)
                                {
                                    if (line7.Contains("Field Density by Sand")==false && line7 != "Field Density by Core Cutting" && line7 != "Classification")
                                    {
                                        txtTestQty.Text = line7.ToString();
                                        cbxSelect.Checked = true;
                                        valid = true;
                                        break;
                                    }

                                }

                            }

                        }
                        if (valid == true)
                        {
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
                TextBox txtTestQty = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtTestQty");
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                cbxSelect.Checked = false;
                txtTestQty.Text = "";
            }
        }
        protected Boolean ValidateCheck()
        {
            string dispalyMsg = "";
            Boolean valid = true;
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                TextBox txtTestQty = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtTestQty");
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                if (lblTest.Text.Contains("Field Density by Sand") == true)
                {
                    if (cbxSelect.Checked == true)
                    {
                        if (txtTestQty.Text == string.Empty)
                        {
                            dispalyMsg = "Please Enter the Quantity of Field Density by Sand ";
                            valid = false;
                            break;
                        }
                    }
                }
                if (lblTest.Text == "Field Density by Core Cutting")
                {
                    if (cbxSelect.Checked == true)
                    {
                        if (txtTestQty.Text == string.Empty)
                        {
                            dispalyMsg = "Please Enter the Quantity of Field Density by Core Cutting ";
                            valid = false;
                            break;
                        }
                    }
                }
                if (lblTest.Text == "Classification")
                {
                    if (cbxSelect.Checked == true)
                    {
                        if (txtTestQty.Text == string.Empty)
                        {
                            dispalyMsg = "Please Enter the Quantity of Classification ";
                            valid = false;
                            break;
                        }
                    }
                }
            }
            if (valid == false)
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + dispalyMsg + "');", true);
            }
            return valid;

        }
        protected void imgCloseTestDetails_Click(object sender, ImageClickEventArgs e)
        {
            if (ValidateCheck() == true)
            {
                TextBox txtTestDetails = grdSoilInward.SelectedRow.Cells[4].FindControl("txtTestDetails") as TextBox;
                txtTestDetails.Text = string.Empty;
                for (int i = 0; i < grdTestDetails.Rows.Count; i++)
                {
                    TextBox txtTestQty = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtTestQty");
                    CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                    Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");

                    if (cbxSelect.Checked)
                    {
                        if (txtTestQty.Text != "")
                        {
                            txtTestDetails.Text = txtTestDetails.Text + lblTest.Text + "|" + txtTestQty.Text + ",";
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
        public void DisplaySolidTest()
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
            SolidTestgrid();
        }
        public void SolidTestgrid()
        {
            int i = 0;
            var res = dc.AllInward_View("SO", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var s in res)
            {
                AddRowSoilInward();
                TextBox txtIdMark = (TextBox)grdSoilInward.Rows[i].Cells[1].FindControl("txtIdMark");
                TextBox txtDescription = (TextBox)grdSoilInward.Rows[i].Cells[2].FindControl("txtDescription");
                TextBox txtSupplierName = (TextBox)grdSoilInward.Rows[i].Cells[3].FindControl("txtSupplierName");

                txtIdMark.Text = s.SOINWD_IdMark_var.ToString();
                txtDescription.Text = s.SOINWD_Description_var.ToString();
                txtSupplierName.Text = s.SOINWD_SupplierName_var.ToString();
                i++;
                ddlTestedAt.SelectedValue = Convert.ToInt32(s.SOINWD_TestedAt_bit).ToString();
            }
            Testgrid();
        }
        public void Testgrid()
        {
            int i = 0;
            var res = dc.AllInward_View("SO", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var s in res)
            {
                string ReferenceNo = s.SOINWD_ReferenceNo_var.ToString();
                var sttest = dc.AllInward_View("SO", 0, ReferenceNo);
                foreach (var st in sttest)
                {
                    var c = dc.Test_View(0, Convert.ToInt32(st.SOTEST_TEST_int), "", 0, 0, 0);
                    foreach (var n in c)
                    {
                        string TEST_Name_var = n.TEST_Name_var.ToString();
                        TextBox txtTestDetails = (TextBox)grdSoilInward.Rows[i].Cells[4].FindControl("txtTestDetails");
                        var sq = dc.AllInward_View("SO", Convert.ToInt32(UC_InwardHeader1.RecordNo), ReferenceNo);
                        bool valid = true;
                        if (TEST_Name_var.Contains("Field Density by Sand") == true)
                        {
                            foreach (var q in sq)
                            {
                                if (q.SOINWD_Pits_tint.ToString() != "" && q.SOINWD_Pits_tint.ToString() != null && q.SOINWD_Pits_tint.ToString() != "0")
                                {
                                    txtTestDetails.Text = txtTestDetails.Text + TEST_Name_var.ToString() + "|" + q.SOINWD_Pits_tint.ToString() + ",";
                                    valid = false;
                                    break;
                                }
                            }
                        }
                        if (TEST_Name_var == "Field Density by Core Cutting")
                        {
                            foreach (var q in sq)
                            {
                                if (q.SOINWD_Cores_tint.ToString() != "" && q.SOINWD_Cores_tint.ToString() != null && q.SOINWD_Cores_tint.ToString() != "0")
                                {
                                    txtTestDetails.Text = txtTestDetails.Text + TEST_Name_var.ToString() + "|" + q.SOINWD_Cores_tint.ToString() + ",";
                                    valid = false;
                                    break;
                                }

                            }
                        }
                        if (TEST_Name_var == "Classification")
                        {
                            foreach (var q in sq)
                            {
                                if (q.SOINWD_Quantity_tint.ToString() != "" && q.SOINWD_Quantity_tint.ToString() != null && q.SOINWD_Quantity_tint.ToString() != "0")
                                {
                                    txtTestDetails.Text = txtTestDetails.Text + TEST_Name_var.ToString() + "|" + q.SOINWD_Quantity_tint.ToString() + ",";
                                    valid = false;
                                    break;
                                }

                            }



                        }
                        if (valid == true)
                        {
                            txtTestDetails.Text = txtTestDetails.Text + TEST_Name_var + ",";
                        }
                    }
                }
                i++;
            }
            int count = grdSoilInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.EnquiryNo = "";
        }
        public void EnableQty()
        {
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                {
                    Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                    TextBox txtTestQty = (TextBox)grdTestDetails.Rows[i].Cells[1].FindControl("txtTestQty");//
                    if (lblTest.Text.Contains("Field Density by Sand") == true || lblTest.Text == "Field Density by Core Cutting" || lblTest.Text == "Classification")
                    {
                        txtTestQty.Visible = true;
                    }
                    else
                    {
                        txtTestQty.Visible = false;
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
                reportStr = rpt.getDetailReportSoilTestInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Soil_Inward", reportStr);
        }
        bool buttonClicked = false;
        protected string getDetailReportSoilTestInward()
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
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Soil Test Lab Sheet </b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Soil Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("SO", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }


            var b = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, "SO", null, null);

            foreach (var nt in b)
            {
                if (buttonClicked == true)
                {
                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "SO" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "SO" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
                        "<td height=19><font size=2>" + "SO" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "SO" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Test to be done</b></font></td>";
            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("SO", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 1% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + c.SOINWD_IdMark_var + "</font></td>";
                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + c.SOINWD_Description_var + "</font></td>";


                mySql += "<td > ";

                var sttest = dc.AllInward_View("SO", 0, c.SOINWD_ReferenceNo_var.ToString());
                foreach (var st in sttest)
                {
                    var cc = dc.Test_View(0, Convert.ToInt32(st.SOTEST_TEST_int), "", 0, 0, 0);
                    foreach (var nn in cc)
                    {
                        string TEST_Name_var = nn.TEST_Name_var.ToString();
                        mySql += "<table border=0 cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
                        mySql += "<tr>";
                        var sq = dc.AllInward_View("SO", Convert.ToInt32(UC_InwardHeader1.RecordNo), c.SOINWD_ReferenceNo_var.ToString());
                        foreach (var q in sq)
                        {
                            if (TEST_Name_var.Contains("Field Density by Sand") == true)
                            {

                                if (q.SOINWD_Pits_tint.ToString() != "" && q.SOINWD_Pits_tint.ToString() != null && q.SOINWD_Pits_tint.ToString() != "0")
                                {
                                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + TEST_Name_var + " " + "(" + q.SOINWD_Pits_tint.ToString() + ")" + "</font></td>";
                                }

                            }
                            else if (TEST_Name_var == "Field Density by Core Cutting")
                            {
                                if (q.SOINWD_Cores_tint.ToString() != "" && q.SOINWD_Cores_tint.ToString() != null && q.SOINWD_Cores_tint.ToString() != "0")
                                {
                                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + TEST_Name_var + " " + "(" + q.SOINWD_Cores_tint.ToString() + ")" + "</font></td>";
                                }

                            }
                            else if (TEST_Name_var == "Classification")
                            {
                                if (q.SOINWD_Quantity_tint.ToString() != "" && q.SOINWD_Cores_tint.ToString() != null && q.SOINWD_Quantity_tint.ToString() != "0")
                                {
                                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + TEST_Name_var + " " + "(" + q.SOINWD_Quantity_tint.ToString() + ")" + "</font></td>";
                                }

                            }
                            else
                            {
                                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + TEST_Name_var + "</font></td>";
                            }
                        }
                        mySql += "</tr>";
                        mySql += "</table>";



                    }
                }



                mySql += "</td>";
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
                reportStr = rpt.getDetailReportSoilTestInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), true);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Soil_LabSheet", reportStr);
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
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "SO")));
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
