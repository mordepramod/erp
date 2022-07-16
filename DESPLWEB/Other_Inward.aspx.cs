using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Other_Inward : System.Web.UI.Page
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
                lblheading.Text = "Other Inward";
                LoadSupplierList();
                LoadOtherTestList();
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    AddRowOtherInward();
                    AddRowOTOtherCharge();
                }
                if (UC_InwardHeader1.RecType != "")
                {
                    DisplayOtherInwardType();
                    DisplayOtherCharges();
                }

                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowOtherInward();
                    AddRowOTOtherCharge();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "OT";
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
        protected void grdOtherInward_RowDataBound(object sender, GridViewRowEventArgs e)
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
        #region add row inward
        protected void AddRowOtherInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["OtherInwardTable"] != null)
            {
                GetCurrentDataOtherInward();
                dt = (DataTable)ViewState["OtherInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));
                dt.Columns.Add(new DataColumn("txtTestDetails", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtDescription"] = string.Empty;
            dr["ddlSupplier"] = string.Empty;
            dr["txtTestDetails"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["OtherInwardTable"] = dt;
            grdOtherInward.DataSource = dt;
            grdOtherInward.DataBind();
            SetPreviousDataOtherInward();

        }
        protected void DeleteRowOtherInward(int rowIndex)
        {
            GetCurrentDataOtherInward();
            DataTable dt = ViewState["OtherInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["OtherInwardTable"] = dt;
            grdOtherInward.DataSource = dt;
            grdOtherInward.DataBind();
            SetPreviousDataOtherInward();
        }
        protected void SetPreviousDataOtherInward()
        {
            DataTable dt = (DataTable)ViewState["OtherInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtDescription = (TextBox)grdOtherInward.Rows[i].Cells[1].FindControl("txtDescription");
                //TextBox txtSupplierName = (TextBox)grdOtherInward.Rows[i].Cells[2].FindControl("txtSupplierName");
                DropDownList ddlSupplier = (DropDownList)grdOtherInward.Rows[i].Cells[2].FindControl("ddlSupplier");
                TextBox txtTestDetails = (TextBox)grdOtherInward.Rows[i].Cells[3].FindControl("txtTestDetails");

                grdOtherInward.Rows[i].Cells[0].Text = (i + 1).ToString();
                txtDescription.Text = dt.Rows[i]["txtDescription"].ToString();
                ddlSupplier.SelectedValue = dt.Rows[i]["ddlSupplier"].ToString();
                txtTestDetails.Text = dt.Rows[i]["txtTestDetails"].ToString();
            }

        }
        protected void GetCurrentDataOtherInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtTestDetails", typeof(string)));
            for (int i = 0; i < grdOtherInward.Rows.Count; i++)
            {
                TextBox txtDescription = (TextBox)grdOtherInward.Rows[i].Cells[1].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdOtherInward.Rows[i].Cells[2].FindControl("ddlSupplier");
                TextBox txtTestDetails = (TextBox)grdOtherInward.Rows[i].Cells[3].FindControl("txtTestDetails");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txtDescription"] = txtDescription.Text;
                drRow["ddlSupplier"] = ddlSupplier.SelectedValue;
                drRow["txtTestDetails"] = txtTestDetails.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["OtherInwardTable"] = dtTable;

        }
        #endregion
        
        #region OTOtherCharges
        public void DisplayOtherCharges()
        {
            int i = 0;
            var re = dc.OtherCharges_View(UC_InwardHeader1.RecType, Convert.ToInt32(UC_InwardHeader1.RecordNo));
            foreach (var r in re)
            {
                AddRowOTOtherCharge();
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
                AddRowOTOtherCharge();
            }

        }
        protected void AddRowOTOtherCharge()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["OTOtherCharges_Table"] != null)
            {
                GetCurrentDataOTOtherCharges();
                dt = (DataTable)ViewState["OTOtherCharges_Table"];
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


            ViewState["OTOtherCharges_Table"] = dt;
            grdOtherCharges.DataSource = dt;
            grdOtherCharges.DataBind();
            SetPreviousDataOTOtherCharges();
        }
        protected void GetCurrentDataOTOtherCharges()
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
            ViewState["OTOtherCharges_Table"] = dtTable;

        }
        protected void SetPreviousDataOTOtherCharges()
        {
            DataTable dt = (DataTable)ViewState["OTOtherCharges_Table"];
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
        protected void DeleteRowOTOtherCharges(int rowIndex)
        {
            GetCurrentDataOTOtherCharges();
            DataTable dt = ViewState["OTOtherCharges_Table"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["OTOtherCharges_Table"] = dt;
            grdOtherCharges.DataSource = dt;
            grdOtherCharges.DataBind();
            SetPreviousDataOTOtherCharges();
        }
        protected void imgInsert_Click(object sender, CommandEventArgs e)
        {
            AddRowOTOtherCharge();
        }
        protected void imgDelete_Click(object sender, CommandEventArgs e)
        {
            if (grdOtherCharges.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdOtherCharges.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowOTOtherCharges(gvr.RowIndex);
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
        
        protected void imgCloseTestDetails_Click(object sender, ImageClickEventArgs e)
        {
            TextBox txtTestDetails = grdOtherInward.SelectedRow.Cells[3].FindControl("txtTestDetails") as TextBox;
            txtTestDetails.Text = string.Empty;
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                TextBox txtQty = (TextBox)grdTestDetails.Rows[i].Cells[2].FindControl("txtQty");
                //TextBox txtRate = (TextBox)grdTestDetails.Rows[i].Cells[3].FindControl("txtRate");
                if (cbxSelect.Checked)
                {
                    //if (txtQty.Text != "" && txtRate.Text != "")
                    if (txtQty.Text != "")
                    {
                        //txtTestDetails.Text = txtTestDetails.Text + lblTest.Text + "," + txtQty.Text + "," + txtRate.Text + "|";
                        txtTestDetails.Text = txtTestDetails.Text + lblTest.Text + "~" + txtQty.Text + "|";
                    }
                }
            }
            chkAddNewSubTest.Checked = false;
            txt_AddNewSubTest.Text = "";
            BtnAddNewSubTest.Visible = false;
            txt_AddNewSubTest.Visible = false;
            lblNewTestRate.Visible = false;
            txtNewTestRate.Visible = false;
            ModalPopupExtender1.Hide();
        }

        protected void lnkRateList_Click(object sender, EventArgs e)
        {
            if (ddlTest.SelectedIndex == 0)
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = true;
                lblMsg.Text = "Select Sub Test";
                ddlTest.Focus();
            }
            else
            {
                LoadOtherSubTestList();
                ModalPopupExtender1.Show();
                gridClear();
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                TextBox txtTestDetails = (TextBox)clickedRow.FindControl("txtTestDetails");
                if (txtTestDetails.Text != "")
                {
                    for (int i = 0; i < grdTestDetails.Rows.Count; i++)
                    {
                        //Boolean valid = false; ;
                        CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                        Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                        TextBox txtQty = (TextBox)grdTestDetails.Rows[i].Cells[2].FindControl("txtQty");
                        //TextBox txtRate = (TextBox)grdTestDetails.Rows[i].Cells[3].FindControl("txtRate");
                        string[] linef = txtTestDetails.Text.Split('|');
                        foreach (string lines in linef)
                        {
                            string[] line2 = lines.Split('~');
                            if (line2[0] == lblTest.Text)
                            {
                                string[] line3 = lines.Split('~');
                                txtQty.Text = line3[1];
                                //txtRate.Text = line3[2];
                                cbxSelect.Checked = true;
                                //valid = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void gridClear()
        {
            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                TextBox txtQty = (TextBox)grdTestDetails.Rows[i].Cells[2].FindControl("txtQty");
                cbxSelect.Checked = false;
                txtQty.Text = "";
            }
            if (grdTestDetails.Rows.Count > 0)
            {
                CheckBox cbxSelectAll = (CheckBox)grdTestDetails.HeaderRow.Cells[2].FindControl("cbxSelectAll");
                cbxSelectAll.Checked = false;
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                ////get report client id, site
                //var enqcl = dc.EnquiryClient_View(Convert.ToInt32(UC_InwardHeader1.EnquiryNo));
                //foreach (var ec in enqcl)
                //{
                //    lblRptClientId.Text = ec.ENQCL_CL_Id.ToString();
                //    lblRptSiteId.Text = ec.ENQCL_SITE_Id.ToString();
                //}
                ////
                string RefNo, SetOfRecord;
                string totalCost = "0";
                clsData clsObj = new clsData();
                int SrNo;
                int Subsets = 0;
                DateTime ReceivedDate = DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null);
                DateTime d1 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);
                int TotalQty = Convert.ToInt32(UC_InwardHeader1.TotalQty);
                if (UC_InwardHeader1.Subsets != "")
                {
                    Subsets = Convert.ToInt32(UC_InwardHeader1.Subsets);
                }
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    dc.Inward_Update_2(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "OT", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Subsets, UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, UC_InwardHeader1.POFileName, false, 0, true);

                    //var res = dc.AllInward_View("OT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
                    //foreach (var a1 in res)
                    //{
                    //    dc.OtherTest_Update(a1.OTINWD_ReferenceNo_var.ToString(), 0, 0, true);
                    //    dc.Other_RateIn_Update(a1.OTINWD_ReferenceNo_var.ToString(), 0, 0, 0, true);
                    //}
                }
                else
                {
                    //Int32 NewrecNo = 0;
                    UC_InwardHeader1.RecordNo = clsObj.GetnUpdateRecordNo("OT").ToString();
                    //UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                    UC_InwardHeader1.ReferenceNo = clsObj.insertRecordTable_2(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), Convert.ToInt32(UC_InwardHeader1.RecordNo), "OT").ToString();
                    //UC_InwardHeader1.ReferenceNo = clsObj.getRefNo(Convert.ToInt32(UC_InwardHeader1.EnquiryNo)).ToString();
                    dc.Inward_Update_2(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "OT", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, UC_InwardHeader1.POFileName, UC_InwardHeader1.OtherClient, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);                    
                }

                //dc.Enquiry_Update_Status(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 2);
                //dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "OT", "", null, null, false, false, false, false, false, false, true);//Delete MIS Record

                dc.OtherInward_Update("OT", Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.ReferenceNo + "/%", "", 0, "", "", 0, "", "", 0, 0, 0, null, null, "", "", 0, 0, 0, "", true);
                //if (lblRptClientId.Text != "" && lblRptClientId.Text != "0")
                //{
                //    dc.Inward_Update_ReportClient(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, Convert.ToInt32(lblRptClientId.Text), Convert.ToInt32(lblRptSiteId.Text));
                //}
                for (int i = 0; i <= grdOtherInward.Rows.Count - 1; i++)
                {
                    TextBox txtDescription = (TextBox)grdOtherInward.Rows[i].Cells[1].FindControl("txtDescription");
                    DropDownList ddlSupplier = (DropDownList)grdOtherInward.Rows[i].Cells[2].FindControl("ddlSupplier");
                    TextBox txtTestDetails = (TextBox)grdOtherInward.Rows[i].Cells[3].FindControl("txtTestDetails");

                    SrNo = Convert.ToInt32(grdOtherInward.Rows[i].Cells[0].Text);
                    RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;

                    dc.Other_RateIn_Update(RefNo, 0, 0, 0, true);
                    dc.OtherInward_Update("OT", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, txtDescription.Text, ddlSupplier.SelectedItem.Text, 0, "", ddlTest.SelectedItem.Text, 0, 0, 0, d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(ddlTest.SelectedItem.Value), Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), txtInterBranchRefNo.Text, false);
                    dc.ClientSupplier_Update(Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(ddlSupplier.SelectedValue), Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, ReceivedDate, false);
                    dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "OT", RefNo, "OT", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);

                    string[] linef = txtTestDetails.Text.Split('|');
                    foreach (string line1 in linef)
                    {
                        string[] lines = line1.Split('~');
                        var s = dc.Test(0, "", 0, "OTHER", lines[0], Convert.ToInt32(ddlTest.SelectedValue)); //OTHER
                        foreach (var n in s)
                        {
                            Int32   Qty = Convert.ToInt32(lines[1].ToString());
                            dc.Other_RateIn_Update(RefNo, n.TEST_Id, Qty, 0, false);
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
                //if (UC_InwardHeader1.OtherClient == true)
                //{
                //    dc.WithoutBill_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType);
                //    if (UC_InwardHeader1.BillNo != "" && UC_InwardHeader1.BillNo != "0")
                //    {
                //        dc.Inward_Update_BillNo(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, UC_InwardHeader1.BillNo);
                //    }
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
                        BillNo = bill.UpdateBill("OT", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
                        //totalCost = clsObj.getProformaBillNetAmount(BillNo,1);
                    }
                    UC_InwardHeader1.BillNo = BillNo.ToString();
                    //
                    if (BillNo != "0")
                        lnkBillPrint.Visible = true;
                }
                //if (UC_InwardHeader1.POFileName != "")
                //{
                //    dc.Inward_Update_POFileName(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, UC_InwardHeader1.BillNo, UC_InwardHeader1.POFileName);
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

                UC_InwardHeader1.EnquiryNo = "";
                DropDownList ddlEnquiryList = (DropDownList)UC_InwardHeader1.FindControl("ddlEnquiryList");
                ddlEnquiryList.Items.Remove(ddlEnquiryList.SelectedItem.Value);
                ddlEnquiryList.ClearSelection();
                //LoadEnquiryList(Convert.ToInt32(UC_InwardHeader1.MaterialId));
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

        public void DisplayOtherInwardType()
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
            OtherInwardgrid();
        }
        
        public void OtherInwardgrid()
        {
            int i = 0;
            var otherInward = dc.AllInward_View("OT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var n in otherInward)
            {
                AddRowOtherInward();
                TextBox txtDescription = (TextBox)grdOtherInward.Rows[i].Cells[1].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdOtherInward.Rows[i].Cells[2].FindControl("ddlSupplier");

                txtDescription.Text = n.OTINWD_Description_var.ToString();
                
                if (ddlSupplier.Items.FindByText(n.OTINWD_SupplierName_var) != null)
                    ddlSupplier.Items.FindByText(n.OTINWD_SupplierName_var).Selected = true;
                i++;

                //ddlTest.SelectedValue = n.OTINWD_TEST_Id.ToString();
                
                if (ddlTest.Items.FindByValue(n.OTINWD_TEST_Id.ToString()) != null)
                    ddlTest.Items.FindByValue(n.OTINWD_TEST_Id.ToString()).Selected = true;
                txtInterBranchRefNo.Text = n.OTINWD_InterBranchRefNo_var;
            }
            int count = grdOtherInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.EnquiryNo = "";
            for (int j = 0; j < grdOtherInward.Rows.Count;j++ )
            {
                TextBox txtTestDetails = (TextBox)grdOtherInward.Rows[j].Cells[3].FindControl("txtTestDetails");
                string RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + (j + 1);
                var ottest = dc.AllInward_View("OT", 0, RefNo);
                foreach (var ot in ottest)
                {
                    //txtTestDetails.Text = txtTestDetails.Text + ot.TEST_Name_var + "," + ot.OTRATEIN_Quantity_tint.ToString() + "," + ot.OTRATEIN_Rate_int.ToString() + "|";
                    txtTestDetails.Text = txtTestDetails.Text + ot.TEST_Name_var + "~" + ot.OTRATEIN_Quantity_tint.ToString() + "|";
                }
            }
        }

        protected void TextChanged(object sender, EventArgs e)
        {
            TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
            if (txtSubsets.Text != "")
            {
                if (Convert.ToInt32(txtSubsets.Text) < grdOtherInward.Rows.Count)
                {
                    for (int i = grdOtherInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdOtherInward.Rows.Count > 1)
                        {
                            DeleteRowOtherInward(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdOtherInward.Rows.Count)
                {
                    for (int i = grdOtherInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
                    {
                        AddRowOtherInward();
                    }
                }
            }
        }
        
        protected void Click(object sender, EventArgs e)
        {
            ViewState["OtherInwardTable"] = null;
            if (UC_InwardHeader1.EnquiryNo != "" && UC_InwardHeader1.EnqNoApp != "" && UC_InwardHeader1.EnqNoApp != "0")
                gridAppData();
            else
                AddRowOtherInward();
            lnkSave.Visible = true;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Text = "";
            lblMsg.Visible = false;
            lnkLabSheet.Visible = false;
            lnkPrint.Visible = false;
            lnkBillPrint.Visible = false;
            lblRptClientId.Text = "0";
            lblRptSiteId.Text = "0";
            txtInterBranchRefNo.Text = "";
            txtInterBranchRefNo.Visible = true;
            chkInterBranchRefNo.Checked = true;
            UC_InwardHeader1.InwdStatus = "Add";
            lnkTemp_Click(sender, e);
        }
        public void gridAppData()
        {
            int i = 0, qty = 0, testId = 0;
            string testName = "";

            var res1 = dc.TestRequestDetails_View_Inward(Convert.ToInt32(UC_InwardHeader1.EnqNoApp), Convert.ToInt32(UC_InwardHeader1.MaterialId), 0, "OT").ToList();
            foreach (var n in res1)
            {
                testName = "";
                AddRowOtherInward();
                TextBox txtDescription = (TextBox)grdOtherInward.Rows[i].Cells[1].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdOtherInward.Rows[i].Cells[2].FindControl("ddlSupplier");
                TextBox txtTestDetails = (TextBox)grdOtherInward.Rows[i].Cells[3].FindControl("txtTestDetails");
                
                if (ddlSupplier.Items.FindByText(Convert.ToString(n.supplier)) != null)
                    ddlSupplier.Items.FindByText(Convert.ToString(n.supplier)).Selected = true;
                else
                {
                    int suppId = dc.Supplier_Update(n.supplier, true);
                    for (int j = 0; j < grdOtherInward.Rows.Count; j++)
                    {
                        DropDownList ddlSupplier1 = (DropDownList)grdOtherInward.Rows[j].FindControl("ddlSupplier");
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
                    testName += t.test_name + "~" + t.specimen + "|";
                    testId = t.test_id;                    
                    qty += Convert.ToInt32(n.specimen);
                }
                txtTestDetails.Text = testName;

                i++;
            }
            int count = grdOtherInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.TotalQty = qty.ToString();
            if (testId > 0)
            {
                var test = dc.Test_View(0, testId, "", 0, 0, 0);
                foreach (var t in test)
                {
                    if (ddlTest.Items.FindByValue(t.TEST_SubTest_Id.ToString()) != null)
                    {
                        ddlTest.ClearSelection();
                        ddlTest.Items.FindByValue(t.TEST_SubTest_Id.ToString()).Selected = true;
                    }
                }
                
            }
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
            txtInterBranchRefNo.Text = txtInterBranchRefNo.Text.Trim();
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
            else if (ddlTest.SelectedIndex == 0)
            {
                lblMsg.Text = "Select Sub Test";
                ddlTest.Focus();
                valid = false;
            }
            else if (chkInterBranchRefNo.Checked == true && txtInterBranchRefNo.Text.Trim() == "")
            {
                lblMsg.Text = "Enter Inter Branch Reference No.";
                txtInterBranchRefNo.Focus();
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
                for (int i = 0; i <= grdOtherInward.Rows.Count - 1; i++)
                {
                    TextBox txtDescription = (TextBox)grdOtherInward.Rows[i].Cells[1].FindControl("txtDescription");
                    DropDownList ddlSupplier = (DropDownList)grdOtherInward.Rows[i].Cells[2].FindControl("ddlSupplier");
                    TextBox txtTestDetails = (TextBox)grdOtherInward.Rows[i].Cells[3].FindControl("txtTestDetails");

                    if (txtDescription.Text == "")
                    {
                        lblMsg.Text = "Enter Description for Sr no. " + (i + 1) + ".";
                        txtDescription.Focus();
                        valid = false;
                        break;
                    }
                    //else if (txtSupplierName.Text == "")
                    //{
                    //    lblMsg.Text = "Enter Suppliers Name for Sr no. " + (i + 1) + ".";
                    //    txtSupplierName.Focus();
                    //    valid = false;
                    //    break;
                    //}
                    else if (ddlSupplier.SelectedItem.Text == "---Select---")
                    {
                        lblMsg.Text = "Select Supplier Name for Sr No. " + (i + 1) + ".";
                        ddlSupplier.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtTestDetails.Text == "")
                    {
                        lblMsg.Text = "Select Test Details for Sr No " + (i + 1) + ".";
                        txtTestDetails.Focus();
                        valid = false;
                        break;
                    }
                    else
                    {
                        string[] strTest = txtTestDetails.Text.Split('|');
                        for (int j = 0; j < strTest.Count()-1; j++)
                        {
                            string[] strTest2 = strTest[j].Split('~');
                            sumQty += Convert.ToInt32(strTest2[1]);
                        }
                    }
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
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
        
        protected void chkAddNewTest_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAddNewTest.Checked)
            {
                txt_AddNewTest.Visible = true;
                BtnAddNewTest.Visible = true;
            }
            else
            {
                txt_AddNewTest.Visible = false;
                BtnAddNewTest.Visible = false;
            }
        }
        
        protected void BtnAddNewTest_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            txt_AddNewTest.Text = txt_AddNewTest.Text.Trim();
            if (txt_AddNewTest.Text == "")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Enter Test Name";
                txt_AddNewTest.Focus();
            }            
            else
            {
                int MaterialId = 0;
                var InwardId = dc.Material_View("OT", "");
                foreach (var n in InwardId)
                {
                    MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
                }

                var test = dc.Test(0, "", 0, "OT", txt_AddNewTest.Text, 0); //OTHER
                if (test.Count() == 0)
                {
                    dc.Test_Update(0, txt_AddNewTest.Text.Trim(), MaterialId, 0, "OT", 0, 0); //OTHER
                    LoadOtherTestList();
                    ddlTest.Items.FindByText(txt_AddNewTest.Text).Selected = true;
                    ClearSelectedTest();
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Test already available.";
                }
                
                //for (int i = 0; i <= grdOtherInward.Rows.Count - 1; i++)
                //{
                //    DropDownList ddl_Testname = (DropDownList)grdOtherInward.Rows[i].Cells[2].FindControl("ddl_Testname");
                //    var ot = dc.Test_View(MaterialId, 0, "", 0, 0);
                //    ddl_Testname.DataTextField = "TEST_Name_var";
                //    ddl_Testname.DataValueField = "TEST_Id";
                //    ddl_Testname.DataSource = ot;
                //    ddl_Testname.DataBind();
                //    ddl_Testname.Items.Insert(0, "Select");
                //    txt_AddNewTest.Text = string.Empty;
                //}
            }
            
        }

        protected void LoadOtherTestList()
        {

            //ddlTest.Items.Clear();
            //int MaterialId = 0;
            //var InwardId = dc.Material_View("OT", "");
            //foreach (var n in InwardId)
            //{
            //    MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
            //}
            //var a = dc.Test_View(MaterialId, 0, "OT", 0, 0, 0); //OTHER
            //ddlTest.DataSource = a;
            //ddlTest.DataTextField = "TEST_Name_var";
            //ddlTest.DataValueField = "TEST_Id";
            //try
            //{
            //    ddlTest.DataBind();
            //}
            //catch { }
            //ddlTest.Items.Insert(0, new ListItem("---Select---", "0"));
            ddlTest.Items.Clear();
            var a = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, 0);
            //var a = dc.Test_View_ForInward("OTHER", 0);
            ddlTest.DataSource = a;
            ddlTest.DataTextField = "TEST_Name_var";
            ddlTest.DataValueField = "TEST_Id";
            ddlTest.DataBind();
            ddlTest.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        protected void LoadOtherSubTestList()
        {
            //int MaterialId = 0;
            //var InwardId = dc.Material_View("OT", "");
            //foreach (var n in InwardId)
            //{
            //    MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
            //}
            //var a = dc.Test_View(MaterialId, 0, "", 0, 0, Convert.ToInt32(ddlTest.SelectedValue));

            //var a = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, Convert.ToInt32(ddlTest.SelectedValue));
            var a = dc.Test_View_ForInward("OTHER", Convert.ToInt32(ddlTest.SelectedValue));
            grdTestDetails.DataSource = a;
            grdTestDetails.DataBind();
        }

        protected void chkAddNewSubTest_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAddNewSubTest.Checked)
            {
                txt_AddNewSubTest.Visible = true;
                BtnAddNewSubTest.Visible = true;
                lblNewTestRate.Visible = true;
                txtNewTestRate.Visible = true;
            }
            else
            {
                txt_AddNewSubTest.Visible = false;
                BtnAddNewSubTest.Visible = false;
                lblNewTestRate.Visible = false;
                txtNewTestRate.Visible = false;
            }
        }

        protected void BtnAddNewSubTest_Click(object sender, EventArgs e)
        {
            lblRateList.Visible = false;
            txt_AddNewSubTest.Text = txt_AddNewSubTest.Text.Trim();
            
            if (txt_AddNewSubTest.Text == "")
            {
                lblRateList.Visible = true;
                lblRateList.Text = "Enter Test Name";
                txt_AddNewSubTest.Focus();
            }
            else if (txtNewTestRate.Text == "")
            {
                lblRateList.Visible = true;
                lblRateList.Text = "Enter Test Rate";
                txtNewTestRate.Focus();
            }
            else
            {
                int MaterialId = 0;
                var InwardId = dc.Material_View("OT", "");
                foreach (var n in InwardId)
                {
                    MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
                }

                var test = dc.Test(0, "", 0, "OT", txt_AddNewSubTest.Text, Convert.ToInt32(ddlTest.SelectedValue)); //OTHER
                if (test.Count() == 0)
                {
                    dc.Test_Update(0, txt_AddNewSubTest.Text.Trim(), MaterialId, Convert.ToInt32(txtNewTestRate.Text), "OT", 0, Convert.ToInt32(ddlTest.SelectedValue)); //OTHER
                }
                else
                {
                    lblRateList.Visible = true;
                    lblRateList.Text = "Test already available.";
                }
                lblTestDetails.Text = string.Empty;
                for (int i = 0; i < grdTestDetails.Rows.Count; i++)
                {
                    CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                    Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                    TextBox txtQty = (TextBox)grdTestDetails.Rows[i].Cells[2].FindControl("txtQty");
                    //TextBox txtRate = (TextBox)grdTestDetails.Rows[i].Cells[3].FindControl("txtRate");
                    if (cbxSelect.Checked)
                    {
                        //if (txtQty.Text != "" && txtRate.Text != "")
                        if (txtQty.Text != "")
                        {
                            //lblTestDetails.Text = lblTestDetails.Text + lblTest.Text + "," + txtQty.Text + "," + txtRate.Text + "|";
                            lblTestDetails.Text = lblTestDetails.Text + lblTest.Text + "~" + txtQty.Text + "|";
                        }
                    }
                }
                LoadOtherSubTestList();
                if (lblTestDetails.Text != "")
                {
                    for (int i = 0; i < grdTestDetails.Rows.Count; i++)
                    {
                        Boolean valid = false; ;
                        CheckBox cbxSelect = (CheckBox)grdTestDetails.Rows[i].Cells[2].FindControl("cbxSelect");
                        Label lblTest = (Label)grdTestDetails.Rows[i].Cells[0].FindControl("lblTest");
                        TextBox txtQty = (TextBox)grdTestDetails.Rows[i].Cells[2].FindControl("txtQty");
                        //TextBox txtRate = (TextBox)grdTestDetails.Rows[i].Cells[3].FindControl("txtRate");
                        string[] linef = lblTestDetails.Text.Split('|');
                        foreach (string lines in linef)
                        {
                            string[] line2 = lines.Split('~');
                            foreach (string line in line2)
                            {
                                if (line == lblTest.Text)
                                {
                                    string[] line3 = lines.Split('~');
                                    txtQty.Text = line3[1];
                                    //txtRate.Text = line3[2];
                                    cbxSelect.Checked = true;
                                    //valid = true;
                                    break;
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
                reportStr = rpt.getDetailReportOT(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Other_Inward", reportStr);
        }
        
        bool buttonClicked = false;
        protected string getDetailReportOT()
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
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Other Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Other Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>" + ddlTest.SelectedItem.Text + "</b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


            var b = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, "OT", null, null);

            foreach (var nt in b)
            {

                if (buttonClicked == true)
                {
                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "OT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "OT" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
                        "<td height=19><font size=2>" + "OT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";


                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "OT" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Sample Description</b></font></td>";
            //mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Test</b></font></td>";
            //mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";
            //
            //mySql += "<td>";
            //mySql += "<table  width=100% id=AutoNumber2>";
            //mySql += "<tr>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b> Test </b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b> Qty </b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b> Rate </b></font></td>";
            //mySql += "</tr>";
            //mySql += "</table>";
            //mySql += "</td>";
            //
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";


            var n = dc.AllInward_View("OT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SrNo = 0;

            foreach (var c in n)
            {
                SrNo++;
                int i = 0;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.OTINWD_Description_var.ToString() + "</font></td>";

                var sp = dc.Test_View(0, Convert.ToInt32(c.OTINWD_ReportForTestId_int), "", 0, 0, 0);
                foreach (var ot in sp)
                {
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + ot.TEST_Name_var.ToString() + "</font></td>";
                }


                //////////
                //mySql += "<td > ";
                //mySql += "<table border=0 cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";

                //var wttest = dc.AllInward_View("OT", 0, c.OTINWD_ReferenceNo_var.ToString());
                //foreach (var ot in wttest)
                //{
                //    var c1 = dc.Test_View(0, Convert.ToInt32(ot.OTTEST_TEST_Id));
                //    foreach (var n2 in c1)
                //    {
                //        mySql += "<tr>";
                //        mySql += "<td>";//
                //        mySql += "<table border:none width=100% style=border-collapse:collapse cellspacing=0 cellpadding=0>";
                //        mySql += "<tr>";
                //        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + n2.TEST_Name_var.ToString() + "</font></td>";

                //        var c11 = dc.Test_View(0, Convert.ToInt32(ot.OTTEST_TEST_Id));
                //        foreach (var d1 in c11)
                //        {
                //            string TEST_Name_var = d1.TEST_Name_var.ToString();
                //            var Rateint = dc.Test_View(0, Convert.ToInt32(ot.OTTEST_TEST_Id));
                //            foreach (var r in Rateint)
                //            {
                //                if (ot.OTTEST_Quantity_tint.ToString() != null && ot.OTTEST_Quantity_tint.ToString() != "")
                //                {
                //mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.OTINWD_Quantity_tint.ToString() + "</font></td>";
                //        }
                //    }
                //}
                //mySql += "</tr>";
                //mySql += "</table>";
                //mySql += "</td>";
                //mySql += "</tr>";
                //}
                //}
                //mySql += "</table>";
                //mySql += "</td>";


                //mySql += "<td>";
                //mySql += "<table border=1 cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
                //mySql += "<tr>";
                //string[] li = txtTestDetails.Text.Split('|');
                //foreach (string line in li)
                //{
                //    string[] line1 = line.Split(',');
                //    foreach (string line2 in line1)
                //    {
                //        if (line2 != "")
                //        {
                //            if (j == 0)
                //            {

                //                mySql += "<td width= 30% align=left valign=top height=19 ><font size=2>&nbsp;" + line2.ToString() + "</font></td>";


                //               // mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + line2.ToString() + "</font></td>";
                //                j++;
                //            }
                //            else
                //            {

                //                mySql += "<td width= 30% align=left valign=top height=19 ><font size=2>&nbsp;" + line2.ToString() + "</font></td>";



                //                //mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + line2.ToString() + "</font></td>";
                //                j++;
                //            }
                //            if (j == 2)
                //            {
                //                break;
                //            }
                //        }
                //    }

                //}
                //mySql += "</tr>";
                //mySql += "</table>";
                //mySql += "</td>";


                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.OTINWD_Quantity_tint.ToString() + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(c.OTINWD_Rate) + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.OTINWD_SupplierName_var.ToString() + "</font></td>";
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
                reportStr = rpt.getDetailReportOT(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), true);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Other_LabSheet", reportStr);
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

        //protected void ddl_TestnameselectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList ddl = (DropDownList)sender;
        //    GridViewRow row = (GridViewRow)ddl.Parent.Parent;
        //    int idx = row.RowIndex;
        //    TextBox txtRate = (TextBox)row.Cells[3].FindControl("txtRate");
        //    if (ddl.SelectedItem.Text != "Select")
        //    {
        //        var data = dc.Test_View(0, Convert.ToInt32(ddl.SelectedValue), "", 0, 0);
        //        foreach (var c in data)
        //        {
        //            txtRate.Text = Convert.ToString(c.TEST_Rate_int);
        //        }
        //    }
        //}
        //public void GrdTestDetailsClear()
        //{
        //    for (int i = 0; i < grdTestDetails.Rows.Count; i++)
        //    {
        //        Label lblTest = (Label)(grdTestDetails.Rows[i].Cells[0].FindControl("lblTest"));
        //        TextBox txtQuantity = (TextBox)(grdTestDetails.Rows[i].Cells[1].FindControl("txtQuantity"));
        //        TextBox txtRate = (TextBox)(grdTestDetails.Rows[i].Cells[2].FindControl("txtRate"));
        //        txtQuantity.Text = string.Empty;

        //    }
        //}

        //protected void lnkRateList_Click(object sender, CommandEventArgs e)
        //{
        //    //Panel2.Visible = true;
        //    lblMessageTestDetails.Visible = false;
        //    ModalPopupExtender1.Show();
        //    GrdTestDetailsClear();
        //    GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
        //    TextBox txtTestDetails = (TextBox)clickedRow.FindControl("txtTestDetails");
        //    #region Start
        //    string text = "";
        //    string testDetails;
        //    testDetails = txtTestDetails.Text;
        //    string[] lines = testDetails.Split('|');
        //    foreach (string line in lines)
        //    {
        //        string[] li = line.Split(',');
        //        foreach (string secondline in li)
        //        {
        //            text = secondline.ToString();
        //            for (int i = 0; i < grdTestDetails.Rows.Count; i++)
        //            {
        //                TextBox txtQuantity = (TextBox)(grdTestDetails.Rows[i].Cells[1].FindControl("txtQuantity"));
        //                TextBox txtRate = (TextBox)(grdTestDetails.Rows[i].Cells[2].FindControl("txtRate"));
        //                Label lblTest = (Label)(grdTestDetails.Rows[i].Cells[0].FindControl("lblTest"));
        //                if (line != string.Empty)
        //                {
        //                    int j = 0;
        //                    if (lblTest.Text == text)
        //                    {
        //                        string[] third = line.Split(',');
        //                        foreach (string thirdline in third)
        //                        {
        //                            if (j == 0)
        //                            {
        //                                j++;
        //                            }
        //                            else
        //                            {
        //                                string qty = thirdline.ToString();
        //                                txtQuantity.Text = qty.ToString();
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    #endregion
        //}
        //protected void grdTestDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    int MaterialId = 0;
        //    var InwardId = dc.Material_View("OT");
        //    foreach (var n in InwardId)
        //    {
        //        MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
        //    }
        //    if (e.CommandName.Equals("Insert"))
        //    {
        //        TextBox txtNewTest = (TextBox)grdTestDetails.FooterRow.FindControl("txtNewTest");
        //        TextBox txtNewQuantity = (TextBox)grdTestDetails.FooterRow.FindControl("txtNewQuantity");
        //        TextBox txtNewRate = (TextBox)grdTestDetails.FooterRow.FindControl("txtNewRate");
        //        if (txtNewTest.Text != "" && txtNewQuantity.Text != "" && txtNewRate.Text != "")
        //        {
        //            dc.Test_Update(Convert.ToInt32(Session["TEST_Id"]), txtNewTest.Text,MaterialId, Convert.ToInt32(txtNewRate.Text),"OT",0);
        //            lblMessageTestDetails.Text = "Updated Successfully";
        //            lblMessageTestDetails.Visible = true;
        //           // GridTestDetailsBind();
        //        }
        //    }
        //}
        //protected void imgCloseTestDetails_Click(object sender, ImageClickEventArgs e) 
        //{
        //    #region Display
        //    ViewState["TestDetails"] = null;
        //    Session["TestDetails"] = null;
        //    int k = 0;
        //    int grd = 0;
        //    var TestDetails = "";
        //    TextBox txtTestDetails = grdOtherInward.SelectedRow.Cells[4].FindControl("txtTestDetails") as TextBox;
        //    txtTestDetails.Text = string.Empty;
        //    foreach (GridViewRow grdRow in grdTestDetails.Rows)
        //    {
        //        TextBox txtQuantity = (TextBox)(grdTestDetails.Rows[grdRow.RowIndex].Cells[1].FindControl("txtQuantity"));
        //        TextBox txtRate = (TextBox)(grdTestDetails.Rows[grdRow.RowIndex].Cells[2].FindControl("txtRate"));
        //        Label lblTest = (Label)(grdTestDetails.Rows[grdRow.RowIndex].Cells[0].FindControl("lblTest"));
        //        if (txtQuantity.Text != "")
        //        {
        //            string Qty = txtQuantity.Text;
        //            string TEST_Name_var = lblTest.Text;
        //            string TEST_Rate_int = txtRate.Text;
        //            TestDetails = String.Format("{0},{1},{2}", TEST_Name_var, Qty, TEST_Rate_int);
        //            Session["TestDetails"] = TestDetails.ToString();
        //            ViewState["TestDetails"] = Session["TestDetails"];
        //            Session.Remove("TestDetails");
        //            txtTestDetails.Text += ViewState["TestDetails"].ToString() + "|";
        //            grd++;
        //        }
        //    }
        //    k++;
        //    ModalPopupExtender1.Hide();
        //    //Panel2.Visible = false;
        //    #endregion
        //}
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
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "OT")));
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

        protected void ddlTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearSelectedTest();
        }

        protected void ClearSelectedTest()
        {
            for (int i = 0; i < grdOtherInward.Rows.Count; i++)
            {
                TextBox txtTestDetails = (TextBox)(grdOtherInward.Rows[i].FindControl("txtTestDetails"));
                txtTestDetails.Text = "";
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

        protected void chkInterBranchRefNo_CheckedChanged(object sender, EventArgs e)
        {
            txtInterBranchRefNo.Text = "";
            if (chkInterBranchRefNo.Checked == true)
                txtInterBranchRefNo.Visible = true;
            else
                txtInterBranchRefNo.Visible = false;
        }
    }
}
