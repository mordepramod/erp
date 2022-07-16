using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace DESPLWEB
{
    public partial class SoilInvestigation_Inward : System.Web.UI.Page
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
                    lblheading.Text = "Soil Investigation Inward";
                }
                
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    AddRowSoilInvestigationInward();
                    AddRowSoilInvestigationOtherCharge();
                }
                if (UC_InwardHeader1.RecType != "")
                {   
                    DisplaySoilInvestigationTest();
                    DisplayOtherCharges();
                }
                else
                {
                    Session["RABill"] = null;
                }
                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowSoilInvestigationInward();
                    AddRowSoilInvestigationOtherCharge();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "GT";
                }
                lnkSave.Visible = true;
            }
        }
        protected void grdSoilInvestigationInward_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl_Test = (DropDownList)e.Row.FindControl("ddl_Test");
                int InwardId = 0;
                var mat = dc.Material_View("GT", "");
                foreach (var m in mat)
                {
                    InwardId = Convert.ToInt32(m.MATERIAL_Id.ToString());
                }
                //var test = dc.Test_View(InwardId, 0, "", 0, 0, 0);
                var test = dc.Test_View_ForBillModify("GT", InwardId, 0);
                ddl_Test.DataSource = test;
                ddl_Test.DataTextField = "TEST_Name_var";
                ddl_Test.DataValueField = "TEST_Id";
                ddl_Test.DataBind();
                ddl_Test.Items.Insert(0, new ListItem("---Select---", "0"));
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
                dt.Columns.Add(new DataColumn("ddl_Test", typeof(string)));
                dt.Columns.Add(new DataColumn("txtUnit", typeof(string)));
                dt.Columns.Add(new DataColumn("txtQty", typeof(string)));
                dt.Columns.Add(new DataColumn("txtRate", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtDescription"] = string.Empty;
            dr["ddl_Test"] = string.Empty;
            dr["txtUnit"] = string.Empty;
            dr["txtQty"] = string.Empty;
            dr["txtRate"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["SoilInvestigationInwardTable"] = dt;
            grdSoilInvestigationInward.DataSource = dt;
            grdSoilInvestigationInward.DataBind();
            SetPreviousSoilInvestigatinInward();

        }
        protected void DeleteRowSoilInvestigationInward(int rowIndex)
        {
            GetCurrentDataSoilInvestigationInward();
            DataTable dt = ViewState["SoilInvestigationInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["SoilInvestigationInwardTable"] = dt;
            grdSoilInvestigationInward.DataSource = dt;
            grdSoilInvestigationInward.DataBind();
            SetPreviousSoilInvestigatinInward();
        }
        protected void SetPreviousSoilInvestigatinInward()
        {
            DataTable dt = (DataTable)ViewState["SoilInvestigationInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtDescription = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("txtDescription");
                DropDownList ddl_Test = (DropDownList)grdSoilInvestigationInward.Rows[i].FindControl("ddl_Test");
                TextBox txtUnit = (TextBox)grdSoilInvestigationInward.Rows[i].FindControl("txtUnit");
                TextBox txtQty = (TextBox)grdSoilInvestigationInward.Rows[i].FindControl("txtQty");
                TextBox txtRate = (TextBox)grdSoilInvestigationInward.Rows[i].FindControl("txtRate");

                grdSoilInvestigationInward.Rows[i].Cells[2].Text = (i + 1).ToString();
                txtDescription.Text = dt.Rows[i]["txtDescription"].ToString();
                ddl_Test.SelectedValue = dt.Rows[i]["ddl_Test"].ToString();
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
            dtTable.Columns.Add(new DataColumn("ddl_Test", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtUnit", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtQty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtRate", typeof(string)));

            for (int i = 0; i < grdSoilInvestigationInward.Rows.Count; i++)
            {
                TextBox txtDescription = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("txtDescription");
                DropDownList ddl_Test = (DropDownList)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("ddl_Test");
                TextBox txtUnit = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[2].FindControl("txtUnit");
                TextBox txtQty = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[3].FindControl("txtQty");
                TextBox txtRate = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[4].FindControl("txtRate");

                drRow = dtTable.NewRow();
                drRow["txtDescription"] = txtDescription.Text;
                drRow["ddl_Test"] = ddl_Test.SelectedValue;
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
                if (Convert.ToInt32(txtSubsets.Text) < grdSoilInvestigationInward.Rows.Count)
                {
                    for (int i = grdSoilInvestigationInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdSoilInvestigationInward.Rows.Count > 1)
                        {
                            DeleteRowSoilInvestigationInward(i - 1);
                            ShowMerge();
                            ChkLumShup();
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdSoilInvestigationInward.Rows.Count)
                {
                    for (int i = grdSoilInvestigationInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
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

        #region SoilInvestigationOtherCharges
        public void DisplayOtherCharges()
        {
            int i = 0;
            var re = dc.OtherCharges_View(UC_InwardHeader1.RecType, Convert.ToInt32(UC_InwardHeader1.RecordNo));
            foreach (var r in re)
            {
                AddRowSoilInvestigationOtherCharge();
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
                AddRowSoilInvestigationOtherCharge();
            }

        }
        protected void AddRowSoilInvestigationOtherCharge()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["SoilInvestigationOtherCharges_Table"] != null)
            {
                GetCurrentDataSoilInvestigationOtherCharges();
                dt = (DataTable)ViewState["SoilInvestigationOtherCharges_Table"];
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


            ViewState["SoilInvestigationOtherCharges_Table"] = dt;
            grdOtherCharges.DataSource = dt;
            grdOtherCharges.DataBind();
            SetPreviousDataSoilInvestigationOtherCharges();
        }
        protected void GetCurrentDataSoilInvestigationOtherCharges()
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
            ViewState["SoilInvestigationOtherCharges_Table"] = dtTable;

        }
        protected void SetPreviousDataSoilInvestigationOtherCharges()
        {
            DataTable dt = (DataTable)ViewState["SoilInvestigationOtherCharges_Table"];
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
        protected void DeleteRowSoilInvestigationOtherCharges(int rowIndex)
        {
            GetCurrentDataSoilInvestigationOtherCharges();
            DataTable dt = ViewState["SoilInvestigationOtherCharges_Table"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["SoilInvestigationOtherCharges_Table"] = dt;
            grdOtherCharges.DataSource = dt;
            grdOtherCharges.DataBind();
            SetPreviousDataSoilInvestigationOtherCharges();
        }
        protected void imgInsert_Click(object sender, CommandEventArgs e)
        {
            AddRowSoilInvestigationOtherCharge();
        }
        protected void imgDelete_Click(object sender, CommandEventArgs e)
        {
            if (grdOtherCharges.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdOtherCharges.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowSoilInvestigationOtherCharges(gvr.RowIndex);
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
                    dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "GT", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, true);
                    var res = dc.AllInward_View("GT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
                    foreach (var a1 in res)
                    {
                        dc.SoilInvestigationTest_Update(a1.GTTEST_RefNo_var.ToString(), 0,"", "GT", 0, "", 0, 0, true);
                    }
                }
                else
                {
                    Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    NewrecNo = clsObj.GetnUpdateRecordNo("GT");
                    UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                    clsObj.insertRecordTable(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), NewrecNo, "GT");
                    UC_InwardHeader1.ReferenceNo = clsObj.getRefNo(Convert.ToInt32(UC_InwardHeader1.EnquiryNo)).ToString();
                    dc.Inward_Update_New(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "GT", 0, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate);

                    //Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    //NewrecNo = clsObj.GetnUpdateRecordNo("GT");
                    //UC_InwardHeader1.RecordNo = NewrecNo.ToString(); 
                    //UC_InwardHeader1.ReferenceNo = dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "GT", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, false).ToString();
                    ////var inward = dc.Inward_View(Convert.ToInt32(UC_InwardHeader1.ReferenceNo), 0, "");
                    ////foreach (var inwd in inward)
                    ////{
                    ////    UC_InwardHeader1.RecordNo = inwd.INWD_RecordNo_int.ToString();
                    ////}
                }

                dc.Enquiry_Update_Status(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 2);
                dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "GT", "", null, null, false, false, false, false, false, false, true);//Delete MIS Record

                dc.GTInward_Update("", Convert.ToInt32(UC_InwardHeader1.RecordNo), "GT", "", 0, 0, "", null, null, "", "",0,0, true);
                if (lblRptClientId.Text != "" && lblRptClientId.Text != "0")
                {
                    dc.Inward_Update_ReportClient(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, Convert.ToInt32(lblRptClientId.Text), Convert.ToInt32(lblRptSiteId.Text));
                }
                for (int i = 0; i <= grdSoilInvestigationInward.Rows.Count - 1; i++)
                {
                    TextBox txtDescription = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("txtDescription");
                    DropDownList ddl_Test = (DropDownList)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("ddl_Test");
                    TextBox txtUnit = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[2].FindControl("txtUnit");
                    TextBox txtQty = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[3].FindControl("txtQty");
                    TextBox txtRate = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[4].FindControl("txtRate");

                    SrNo = Convert.ToInt32(grdSoilInvestigationInward.Rows[i].Cells[2].Text);
                    RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    if (grdSoilInvestigationInward.Rows[i].Cells[6].Visible == false)
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
                        //Session["txtRate"] = txtRate.Text;
                        //ViewState["txtRate"] = Session["txtRate"].ToString();
                        //Session["txtRate"] = null;
                        ViewState["txtRate"] = txtRate.Text;

                    }
                    dc.GTInward_Update(RefNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), "GT", SetOfRecord, Convert.ToByte(chk_Lumshup.Checked), 0, "", d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), false);
                    //dc.SoilInvestigationTest_Update(RefNo, txtDescription.Text, "GT", Convert.ToByte(SrNo), txtUnit.Text, Convert.ToByte(txtQty.Text), Convert.ToInt32(txtRate.Text), false);
                    dc.SoilInvestigationTest_Update(RefNo, Convert.ToInt32(ddl_Test.SelectedValue), txtDescription.Text, "GT", Convert.ToByte(SrNo), txtUnit.Text, Convert.ToByte(txtQty.Text), Convert.ToInt32(txtRate.Text), false);

                    dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "GT", RefNo, "GT", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);
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
                //if (UC_InwardHeader1.OtherClient == true)
                //{
                //    dc.WithoutBill_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType);
                //}
                //if (UC_InwardHeader1.OtherClient == false && UC_InwardHeader1.BillNo != "0" &&
                //    UC_InwardHeader1.BillNo != "" && Convert.ToInt32(UC_InwardHeader1.BillNo) > 0 && Session["RABill"] == null)
                
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                if (UC_InwardHeader1.OtherClient == false && UC_InwardHeader1.BillNo != "0" && UC_InwardHeader1.BillNo != "")
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
                        BillNo = bill.UpdateBill("GT", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
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
                if (UC_InwardHeader1.BillNo == "" || UC_InwardHeader1.BillNo == "0")
                {
                    lnkGenerateBill.Visible = true;
                    var site = dc.Site_View(Convert.ToInt32(UC_InwardHeader1.SiteId), 0, 0, "");
                    foreach (var st in site)
                    {
                        if (st.SITE_RABillStatus_bit == true)
                        {
                            lnkGenerateBill.Visible = false;
                            //if (Session["RABill"] != null && Session["RABill"].ToString() == "True")
                            //{
                            lnkGenerateRABill.Visible = true;
                            //}
                        }
                    }

                }
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

        protected void lnkGenerateBill_Click(object sender, EventArgs e)
        {
            if (UC_InwardHeader1.OtherClient == false &&
                (UC_InwardHeader1.BillNo == "" || UC_InwardHeader1.BillNo == "0"))
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
                    if (b.BILL_ApproveStatus_bit != null)
                    {
                        if (b.BILL_ApproveStatus_bit == true)
                            updateBillFlag = false;
                        else
                            updateBillFlag = true;
                    }
                }
                if (updateBillFlag == true)
                {
                    BillNo = bill.UpdateBill("GT", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
                }
                UC_InwardHeader1.BillNo = BillNo.ToString();
                //
                if (UC_InwardHeader1.POFileName != "")
                {
                    dc.Inward_Update_POFileName(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, UC_InwardHeader1.BillNo, UC_InwardHeader1.POFileName);
                }
                lnkBillPrint.Visible = true;
                lnkGenerateBill.Visible = false;
                lnkGenerateRABill.Visible = false;
            }
        }
        protected void lnkGenerateRABill_Click(object sender, EventArgs e)
        {
            //bill updation
            BillUpdation bill = new BillUpdation();
            string BillNo = "0";
            BillNo = bill.UpdateBill("GT", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
            UC_InwardHeader1.BillNo = BillNo.ToString();
            //
            lnkBillPrint.Visible = true;
            lnkGenerateBill.Visible = false;
            lnkGenerateRABill.Visible = false;
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
            else if (valid == true)
            {
                for (int i = 0; i <= grdSoilInvestigationInward.Rows.Count - 1; i++)
                {
                    TextBox txtDescription = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("txtDescription");
                    DropDownList ddl_Test = (DropDownList)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("ddl_Test");
                    TextBox txtUnit = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[2].FindControl("txtUnit");
                    TextBox txtQty = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[3].FindControl("txtQty");
                    TextBox txtRate = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[4].FindControl("txtRate");
                    
                    if (ddl_Test.SelectedIndex <= 0 || ddl_Test.SelectedItem.Text == "---Select---" || ddl_Test.SelectedItem.Text == "Miscellaneous")
                    {
                        lblMsg.Text = "Select test from list for row no " + (i + 1) + ".";
                        ddl_Test.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtDescription.Text == "")
                    {
                        lblMsg.Text = "Enter Description for row no " + (i + 1) + ".";
                        txtDescription.Focus();
                        valid = false;
                        break;
                    }
                    else if (grdSoilInvestigationInward.Columns[4].Visible == true)
                    {
                        if (txtUnit.Text == "")
                        {
                            lblMsg.Text = "Enter Unit for row no " + (i + 1) + ".";
                            txtUnit.Focus();
                            valid = false;
                            break;
                        }
                    }

                    else if (grdSoilInvestigationInward.Columns[5].Visible == true)
                    {
                        if (txtQty.Text == "")
                        {
                            lblMsg.Text = "Enter Quantity for row no " + (i + 1) + ".";
                            txtQty.Focus();
                            valid = false;
                            break;
                        }
                    }
                    else if (grdSoilInvestigationInward.Rows[i].Cells[6].Visible == true)
                    {
                        if (txtRate.Text == "")
                        {
                            lblMsg.Text = "Enter Rate for row no " + (i + 1) + ".";
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
        public void DisplaySoilInvestigationTest()
        {
            var Modify = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, UC_InwardHeader1.RecType .ToString(), null, null);
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
                if (n.INWD_BILL_Id == "0")
                {
                    lnkGenerateBill.Visible = true;
                }
                if (n.SITE_RABillStatus_bit == true)
                {
                    lnkGenerateBill.Visible = false;
                    if (Session["RABill"] != null && Session["RABill"].ToString() == "True")
                    {
                        lnkGenerateRABill.Visible = true;
                    }
                }
                if (n.INWD_RptCL_Id != null)
                    lblRptClientId.Text = n.INWD_RptCL_Id.ToString();
                if (n.INWD_RptSITE_Id != null)
                    lblRptSiteId.Text = n.INWD_RptSITE_Id.ToString();

            }
            SolidInvestigationTestgrid();
        }
        public void SolidInvestigationTestgrid()
        {
            int i = 0;
            var res = dc.AllInward_View("GT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var s in res)
            {
                AddRowSoilInvestigationInward();
                TextBox txtDescription = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("txtDescription");
                DropDownList ddl_Test = (DropDownList)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("ddl_Test");
                TextBox txtUnit = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[2].FindControl("txtUnit");
                TextBox txtQty = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[3].FindControl("txtQty");
                TextBox txtRate = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[4].FindControl("txtRate");
                txtDescription.Text = s.GTTEST_Description_var.ToString();
                ddl_Test.SelectedValue = s.GTTEST_TEST_Id.ToString();
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
                    grdSoilInvestigationInward.Columns[4].Visible = false;
                    grdSoilInvestigationInward.Columns[5].Visible = false;
                    SetWidth();
                    setAutoWidth();
                }
            }
            Displaycell();
            int count = grdSoilInvestigationInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.EnquiryNo = "";
        }
        public void Displaycell()
        {
            int i = 0;
            int C = 0;
            string Rate = "";
            var res = dc.AllInward_View("GT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var s in res)
            {
                if (s.GTINW_LumpSump_tint > 0)
                {
                    TextBox txtRate = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[4].FindControl("txtRate");
                    if (Rate != "")
                    {
                        if (Rate == s.GTTEST_Rate_int.ToString())
                        {
                            TextBox txtRate1 = (TextBox)grdSoilInvestigationInward.Rows[i - 1].Cells[4].FindControl("txtRate");
                            txtRate.BorderStyle = BorderStyle.None;
                            txtRate1.BorderStyle = BorderStyle.None;
                            grdSoilInvestigationInward.Rows[i].Cells[6].Visible = false;
                            grdSoilInvestigationInward.BackColor = System.Drawing.Color.White;
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
                            grdSoilInvestigationInward.Rows[i].Cells[6].Visible = true;
                            grdSoilInvestigationInward.Rows[i].Cells[6].BorderColor = System.Drawing.Color.White;
                        }
                    }
                    if (s.GTTEST_Rate_int.ToString() != "")
                    {
                        txtRate.Text = s.GTTEST_Rate_int.ToString();
                        Rate = txtRate.Text;
                    }
                    C++;
                    i++;

                    if (txtToRow.Text != "")
                    {
                        txtToRow.Text = (Convert.ToInt32(txtToRow.Text) + 1).ToString();
                    }
                }
                else
                {
                    break;
                }
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
                    for (int i = 0; i < grdSoilInvestigationInward.Rows.Count; i++)
                    {
                        TextBox txtRate1 = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[4].FindControl("txtRate");
                        Boolean foundIt = false;
                        if (i >= FromRowNo && i <= ToRowNo)
                        {
                            for (int j = 0; j < ToRowNo; j++)
                            {
                                if (j == i)
                                {
                                    TextBox txtRate = (TextBox)grdSoilInvestigationInward.Rows[i - 1].Cells[4].FindControl("txtRate");
                                    txtRate.BorderStyle = BorderStyle.None;
                                    grdSoilInvestigationInward.Rows[i].Cells[6].Visible = false;
                                    grdSoilInvestigationInward.BackColor = System.Drawing.Color.White;
                                    foundIt = true;
                                    break;
                                }
                                else
                                {
                                    txtRate1.BorderStyle = BorderStyle.NotSet;
                                    grdSoilInvestigationInward.Rows[i].Cells[6].Visible = true;
                                    grdSoilInvestigationInward.Rows[i].Cells[6].BorderColor = System.Drawing.Color.White;
                                }
                            }
                        }
                        if (foundIt == false)
                        {
                            grdSoilInvestigationInward.Rows[i].Cells[6].Visible = true;
                            txtRate1.BorderStyle = BorderStyle.NotSet;
                        }
                    }
                }
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
                grdSoilInvestigationInward.Columns[4].Visible = false;
                grdSoilInvestigationInward.Columns[5].Visible = false;
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
                grdSoilInvestigationInward.Columns[4].Visible = true;
                grdSoilInvestigationInward.Columns[5].Visible = true;
                setAutoWidth();
                txtFrmRow.Text = string.Empty;
                txtFrmRow.Visible = false;
                txtToRow.Visible = false;
                txtToRow.Text = string.Empty;
                Btn_Apply.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
                for (int i = 0; i < grdSoilInvestigationInward.Rows.Count; i++)
                {
                    if (grdSoilInvestigationInward.Columns[4].Visible == true &&
                       grdSoilInvestigationInward.Columns[5].Visible == true)
                    {
                        TextBox txtDescription = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("txtDescription");
                        TextBox txtRate = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[4].FindControl("txtRate");
                        grdSoilInvestigationInward.Rows[i].Cells[6].Visible = true;
                        //grdSoilInvestigationInward.Columns[6].ItemStyle.Width = 155;
                        txtRate.BorderStyle = BorderStyle.NotSet;
                    }
                }
            }

        }
        public void SetWidth()
        {

            for (int i = 0; i < grdSoilInvestigationInward.Rows.Count; i++)
            {
                TextBox txtDescription = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("txtDescription");
                DropDownList ddl_Test = (DropDownList)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("ddl_Test");
                TextBox txtRate = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[4].FindControl("txtRate");
                if (grdSoilInvestigationInward.Columns[4].Visible == false &&
                    grdSoilInvestigationInward.Columns[5].Visible == false)
                {
                    ////grdSoilInvestigationInward.Columns[5].ItemStyle.Width = 690;
                    ////grdSoilInvestigationInward.Columns[6].ItemStyle.Width = 150;
                    //grdSoilInvestigationInward.Columns[3].ItemStyle.Width = 690;
                    ////txtDescription.Width = 690;
                    ////ddl_Test.Width = 690;
                    //txtDescription.Width = 520;
                    ////ddl_Test.Width = 150;
                    ////txtRate.Width = 100;
                }

            }
        }
        public void setAutoWidth()
        {
            for (int i = 0; i < grdSoilInvestigationInward.Rows.Count; i++)
            {
                if (grdSoilInvestigationInward.Columns[4].Visible == true &&
                   grdSoilInvestigationInward.Columns[5].Visible == true)
                {
                    TextBox txtDescription = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("txtDescription");
                    DropDownList ddl_Test = (DropDownList)grdSoilInvestigationInward.Rows[i].Cells[1].FindControl("ddl_Test");
                    TextBox txtRate = (TextBox)grdSoilInvestigationInward.Rows[i].Cells[4].FindControl("txtRate");

                    ////grdSoilInvestigationInward.Rows[i].Cells[6].Visible = true;
                    ////grdSoilInvestigationInward.Columns[6].ItemStyle.Width = 155;
                    //grdSoilInvestigationInward.Columns[3].ItemStyle.Width = 580;
                    
                    ////txtDescription.Width = 518;
                    ////ddl_Test.Width = 518;
                    //txtDescription.Width = 350;
                    ////ddl_Test.Width = 150;
                    ////txtRate.Width = 100; 
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
                reportStr = rpt.getDetailReportSoilInvestigation(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo),false);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("GT_Inward", reportStr);
        }
        bool buttonClicked = false;
        protected string getDetailReportSoilInvestigation()
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
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Soil Investigation Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Soil Investigation Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("GT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }

        
            var b = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, "GT", null, null);

            foreach (var nt in b)
            {
                if (buttonClicked == true)
                {
                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' align=left valign=top height=19><font size=2><b>Enquiry No. </b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + UC_InwardHeader1.EnquiryNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td  height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "GT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "GT" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
                     "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                     "<td height=19><font size=2></font></td>" +
                     "<td height=19><font size=2></font></td>" +
                     "<td align=left valign=top height=19><font size=2><b>Enquiry No. </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + UC_InwardHeader1.EnquiryNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "GT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "GT" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
                        "<td align=left valign=top height=19><font size=2><b>No of Samples </b></font></td>" +
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
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Unit</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Quantity </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Rate</b></font></td>";
            mySql += "</tr>";


            var n = dc.AllInward_View("GT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
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
                reportStr = rpt.getDetailReportSoilInvestigation(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), true);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("GT_LabSheet", reportStr);
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
            //BillUpdation bill = new BillUpdation();
            //bill.getBillPrintString(UC_InwardHeader1.BillNo, false);
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
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "GT")));
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

        protected void ddl_Test_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txtDescription = (TextBox)grdSoilInvestigationInward.Rows[rowindex].FindControl("txtDescription");
            if (ddl.SelectedIndex > 0)
            {
                txtDescription.Text = ddl.SelectedItem.Text;
            }
            else
            {
                txtDescription.Text = "";
            }
        }
    }
}
