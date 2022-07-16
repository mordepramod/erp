using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Steel_Inward : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        static DateTime recdDate = DateTime.Now;
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
                lblheading.Text = "Steel Testing Inward";
                LoadSupplierList();

                // coupons
                LoadCouponList();
                //--
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    AddRowSteelInward();
                }
                if (UC_InwardHeader1.RecType != "")
                {
                    getSteelInwardTesting();
                }
                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowSteelInward();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "ST";
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

        protected void grdSteelInward_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void AddRowSteelInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["SteelInwardTable"] != null)
            {
                GetCurrentDataSteelInward();
                dt = (DataTable)ViewState["SteelInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDiameter", typeof(string)));
                dt.Columns.Add(new DataColumn("txtQuantity", typeof(string)));
                dt.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlSteelType", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlGrade", typeof(string)));
                dt.Columns.Add(new DataColumn("chkTensileStrength", typeof(string)));
                dt.Columns.Add(new DataColumn("chkElongation", typeof(string)));
                dt.Columns.Add(new DataColumn("chkRebend", typeof(string)));
                dt.Columns.Add(new DataColumn("chkWeightPerMeter", typeof(string)));
                dt.Columns.Add(new DataColumn("chkBend", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("chkSupplier", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtDiameter"] = string.Empty;
            dr["txtQuantity"] = string.Empty;
            dr["txtIdMark"] = string.Empty;
            dr["ddlSteelType"] = string.Empty;
            dr["ddlGrade"] = string.Empty;
            dr["chkTensileStrength"] = false;
            dr["chkElongation"] = false;
            dr["chkRebend"] = false;
            dr["chkWeightPerMeter"] = false;
            dr["chkBend"] = false;
            dr["txtDescription"] = string.Empty;
            dr["chkSupplier"] = false;
            dr["ddlSupplier"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["SteelInwardTable"] = dt;
            grdSteelInward.DataSource = dt;
            grdSteelInward.DataBind();
            SetPreviousDataSteelInward();
        }
        protected void DeleteRowSteelInward(int rowIndex)
        {
            GetCurrentDataSteelInward();
            DataTable dt = ViewState["SteelInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["SteelInwardTable"] = dt;
            grdSteelInward.DataSource = dt;
            grdSteelInward.DataBind();
            SetPreviousDataSteelInward();
        }
        protected void SetPreviousDataSteelInward()
        {
            DataTable dt = (DataTable)ViewState["SteelInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)grdSteelInward.Rows[i].Cells[1].FindControl("txtDiameter");
                TextBox box2 = (TextBox)grdSteelInward.Rows[i].Cells[2].FindControl("txtQuantity");
                TextBox box3 = (TextBox)grdSteelInward.Rows[i].Cells[3].FindControl("txtIdMark");
                DropDownList box4 = (DropDownList)grdSteelInward.Rows[i].Cells[4].FindControl("ddlSteelType");
                DropDownList box5 = (DropDownList)grdSteelInward.Rows[i].Cells[5].FindControl("ddlGrade");
                CheckBox box6 = (CheckBox)grdSteelInward.Rows[i].Cells[6].FindControl("chkTensileStrength");
                CheckBox box7 = (CheckBox)grdSteelInward.Rows[i].Cells[7].FindControl("chkElongation");
                CheckBox box8 = (CheckBox)grdSteelInward.Rows[i].Cells[8].FindControl("chkRebend");
                CheckBox box9 = (CheckBox)grdSteelInward.Rows[i].Cells[9].FindControl("chkWeightPerMeter");
                CheckBox box10 = (CheckBox)grdSteelInward.Rows[i].Cells[10].FindControl("chkBend");
                TextBox box11 = (TextBox)grdSteelInward.Rows[i].Cells[11].FindControl("txtDescription");//
                CheckBox chkSupplier = (CheckBox)grdSteelInward.Rows[i].Cells[12].FindControl("chkSupplier");
                DropDownList box12 = (DropDownList)grdSteelInward.Rows[i].Cells[13].FindControl("ddlSupplier");

                grdSteelInward.Rows[i].Cells[0].Text = (i + 1).ToString();
                box1.Text = dt.Rows[i]["txtDiameter"].ToString();
                box2.Text = dt.Rows[i]["txtQuantity"].ToString();
                box3.Text = dt.Rows[i]["txtIdMark"].ToString();
                box4.Text = dt.Rows[i]["ddlSteelType"].ToString();
                box5.Text = dt.Rows[i]["ddlGrade"].ToString();
                box6.Checked = Convert.ToBoolean(dt.Rows[i]["chkTensileStrength"].ToString());
                box7.Checked = Convert.ToBoolean(dt.Rows[i]["chkElongation"].ToString());
                box8.Checked = Convert.ToBoolean(dt.Rows[i]["chkRebend"].ToString());
                box9.Checked = Convert.ToBoolean(dt.Rows[i]["chkWeightPerMeter"].ToString());
                box10.Checked = Convert.ToBoolean(dt.Rows[i]["chkBend"].ToString());
                box11.Text = dt.Rows[i]["txtDescription"].ToString();
                chkSupplier.Checked = Convert.ToBoolean(dt.Rows[i]["chkSupplier"].ToString());
                box12.SelectedValue = dt.Rows[i]["ddlSupplier"].ToString();
            }
        }
        protected void GetCurrentDataSteelInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDiameter", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtQuantity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlSteelType", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlGrade", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chkTensileStrength", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chkElongation", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chkRebend", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chkWeightPerMeter", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chkBend", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));//
            dtTable.Columns.Add(new DataColumn("chkSupplier", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));

            for (int i = 0; i < grdSteelInward.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)grdSteelInward.Rows[i].Cells[1].FindControl("txtDiameter");
                TextBox box2 = (TextBox)grdSteelInward.Rows[i].Cells[2].FindControl("txtQuantity");
                TextBox box3 = (TextBox)grdSteelInward.Rows[i].Cells[3].FindControl("txtIdMark");
                DropDownList box4 = (DropDownList)grdSteelInward.Rows[i].Cells[4].FindControl("ddlSteelType");
                DropDownList box5 = (DropDownList)grdSteelInward.Rows[i].Cells[5].FindControl("ddlGrade");
                CheckBox box6 = (CheckBox)grdSteelInward.Rows[i].Cells[6].FindControl("chkTensileStrength");
                CheckBox box7 = (CheckBox)grdSteelInward.Rows[i].Cells[7].FindControl("chkElongation");
                CheckBox box8 = (CheckBox)grdSteelInward.Rows[i].Cells[8].FindControl("chkRebend");
                CheckBox box9 = (CheckBox)grdSteelInward.Rows[i].Cells[9].FindControl("chkWeightPerMeter");
                CheckBox box10 = (CheckBox)grdSteelInward.Rows[i].Cells[10].FindControl("chkBend");
                TextBox box11 = (TextBox)grdSteelInward.Rows[i].Cells[11].FindControl("txtDescription");
                CheckBox chkSupplier = (CheckBox)grdSteelInward.Rows[i].Cells[12].FindControl("chkSupplier");
                DropDownList ddlSupplier = (DropDownList)grdSteelInward.Rows[i].Cells[13].FindControl("ddlSupplier");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txtDiameter"] = box1.Text;
                drRow["txtQuantity"] = box2.Text;
                drRow["txtIdMark"] = box3.Text;
                drRow["ddlSteelType"] = box4.Text;
                drRow["ddlGrade"] = box5.Text;
                drRow["chkTensileStrength"] = box6.Checked;
                drRow["chkElongation"] = box7.Checked;
                drRow["chkRebend"] = box8.Checked;
                drRow["chkWeightPerMeter"] = box9.Checked;
                drRow["chkBend"] = box10.Checked;
                drRow["txtDescription"] = box11.Text;
                drRow["chkSupplier"] = chkSupplier.Checked;
                drRow["ddlSupplier"] = ddlSupplier.SelectedValue;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["SteelInwardTable"] = dtTable;

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
                bool updateFlag = true;
                string RefNo, SetOfRecord;
                string totalCost = "0";
                clsData clsObj = new clsData();
                int SrNo, TestId;
                TestId = 0;
                bool Supplier;
                DateTime ReceivedDate = DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null);
                DateTime d1 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    dc.Inward_Update_2(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "ST", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, UC_InwardHeader1.POFileName, false, 0, true);
                    
                    //var res = dc.AllInward_View("ST", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
                    //foreach (var a1 in res)
                    //{
                    //    dc.SteelTest_Update(a1.STINWD_ReferenceNo_var.ToString(), 0, true);
                    //}

                }
                else
                {
                    try
                    {
                        //Int32 NewrecNo = 0;
                        UC_InwardHeader1.RecordNo = clsObj.GetnUpdateRecordNo("ST").ToString();
                        //UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                        UC_InwardHeader1.ReferenceNo = clsObj.insertRecordTable_2(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), Convert.ToInt32(UC_InwardHeader1.RecordNo), "ST").ToString();
                        //UC_InwardHeader1.ReferenceNo = clsObj.getRefNo(Convert.ToInt32(UC_InwardHeader1.EnquiryNo)).ToString();
                        dc.Inward_Update_2(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "ST", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, UC_InwardHeader1.POFileName, UC_InwardHeader1.OtherClient, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
                        //dc.Enquiry_Update_Status(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 2);
                    }
                    catch
                    {
                        updateFlag = false;
                    }
                }
                if (updateFlag == true)
                {
                    //dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "ST", "", null, null, false, false, false, false, false, false, true);//Delete MIS Record
                    dc.SteelInward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), "ST", Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.ReferenceNo + "/%", "", 0, 0, "", "", 0, "", "", "", 0, "", false, null, null, "", "", 0, 0, true,"");

                    //delete coupon no
                    dc.Coupon_UpdateST("", 0, 0, 0, null, 0, null, "", Convert.ToInt32(UC_InwardHeader1.RecordNo), null, false, false);

                    //dc.ClientSupplier_Update(Convert.ToInt32(UC_InwardHeader1.ClientId), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, null, true);

                    //if (lblRptClientId.Text != "" && lblRptClientId.Text != "0")
                    //{
                    //    dc.Inward_Update_ReportClient(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, Convert.ToInt32(lblRptClientId.Text), Convert.ToInt32(lblRptSiteId.Text));
                    //}

                    string strSupplier = "";
                    string CouponNo = "";
                    for (int i = 0; i <= grdSteelInward.Rows.Count - 1; i++)
                    {
                        TextBox Diameter = (TextBox)grdSteelInward.Rows[i].Cells[1].FindControl("txtDiameter");
                        TextBox Quantity = (TextBox)grdSteelInward.Rows[i].Cells[2].FindControl("txtQuantity");
                        TextBox IdMark = (TextBox)grdSteelInward.Rows[i].Cells[3].FindControl("txtIdMark");
                        DropDownList SteelType = (DropDownList)grdSteelInward.Rows[i].Cells[4].FindControl("ddlSteelType");
                        DropDownList Grade = (DropDownList)grdSteelInward.Rows[i].Cells[5].FindControl("ddlGrade");
                        CheckBox TensileStrength = (CheckBox)grdSteelInward.Rows[i].Cells[6].FindControl("chkTensileStrength");
                        CheckBox Elongation = (CheckBox)grdSteelInward.Rows[i].Cells[7].FindControl("chkElongation");
                        CheckBox Rebend = (CheckBox)grdSteelInward.Rows[i].Cells[8].FindControl("chkRebend");
                        CheckBox WeightPerMeter = (CheckBox)grdSteelInward.Rows[i].Cells[9].FindControl("chkWeightPerMeter");
                        CheckBox Bend = (CheckBox)grdSteelInward.Rows[i].Cells[10].FindControl("chkBend");
                        TextBox Description = (TextBox)grdSteelInward.Rows[i].Cells[11].FindControl("txtDescription");
                        CheckBox chkSupplier = (CheckBox)grdSteelInward.Rows[i].Cells[12].FindControl("chkSupplier");
                        DropDownList ddlSupplier = (DropDownList)grdSteelInward.Rows[i].Cells[13].FindControl("ddlSupplier");
                        TextBox TestingDate = new TextBox();
                        TestingDate.Text = DateTime.Now.ToShortDateString();
                        SrNo = Convert.ToInt32(grdSteelInward.Rows[i].Cells[0].Text);
                        RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                        SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;

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

                        // coupon
                        CouponNo = "";
                        int tempQty = 0;
                        if (Convert.ToInt32(lblNoOfCoupons.Text) > 0)
                        {

                            var couponsitespec = dc.Coupon_View_SitewiseST(Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), 0, recdDate).ToList();
                            for (int c = 0; c < couponsitespec.Count; c++)
                            {
                                if (tempQty < Convert.ToInt32(Quantity.Text))
                                {
                                    CouponNo = CouponNo + couponsitespec[c].COUP_Id + ",";
                                    dc.Coupon_UpdateST("", couponsitespec[c].COUP_Id, Convert.ToInt32(UC_InwardHeader1.ClientId), 0, null, 1, DateTime.Now, RefNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), null, false, false);
                                    tempQty++;
                                    //CouponCount++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (couponsitespec.Count() == 0)
                            {
                                var coupon = dc.Coupon_ViewST("", 0, 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), 0, recdDate).ToList();
                                for (int c = 0; c < coupon.Count; c++)
                                {
                                    if (tempQty < Convert.ToInt32(Quantity.Text))
                                    {
                                        CouponNo = CouponNo + coupon[c].COUP_Id + ",";
                                        dc.Coupon_UpdateST("", coupon[c].COUP_Id, Convert.ToInt32(UC_InwardHeader1.ClientId), 0, null, 1, DateTime.Now, RefNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), null, false, false);
                                        tempQty++;
                                        //  CouponCount++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        //---



                        dc.SteelInward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), "ST", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(Quantity.Text), Description.Text, strSupplier, 0, "", IdMark.Text, Grade.Text, Convert.ToByte(Diameter.Text), SteelType.SelectedItem.Text, Supplier, d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), false,CouponNo );
                        dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "ST", RefNo, "ST", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);

                        int Test_SrNo = 0;
                        if (TensileStrength.Checked && Elongation.Checked)
                        {
                            if (Convert.ToInt32(Diameter.Text) >= 0 && Convert.ToInt32(Diameter.Text) <= 12)
                                Test_SrNo = 7;
                            else if (Convert.ToInt32(Diameter.Text) >= 16 && Convert.ToInt32(Diameter.Text) <= 25)
                                Test_SrNo = 6;
                            else if (Convert.ToInt32(Diameter.Text) >= 26 && Convert.ToInt32(Diameter.Text) <= 100)
                                Test_SrNo = 8;

                            var a = dc.Test(Test_SrNo, "", 0, "ST", "", 0);
                            foreach (var n in a)
                            {
                                TestId = Convert.ToInt32(n.TEST_Id.ToString());
                                dc.SteelTest_Update(RefNo, TestId, false);
                            }
                        }
                        if (TensileStrength.Checked && Test_SrNo == 0)
                        {
                            Test_SrNo = 1;
                            var a = dc.Test(Test_SrNo, "", 0, "ST", "", 0);
                            foreach (var n in a)
                            {
                                TestId = Convert.ToInt32(n.TEST_Id.ToString());
                                dc.SteelTest_Update(RefNo, TestId, false);
                            }
                        }
                        if (Elongation.Checked && Test_SrNo == 0)
                        {
                            Test_SrNo = 2;
                            var a = dc.Test(Test_SrNo, "", 0, "ST", "", 0);
                            foreach (var n in a)
                            {
                                TestId = Convert.ToInt32(n.TEST_Id.ToString());
                                dc.SteelTest_Update(RefNo, TestId, false);
                            }
                        }
                        if (Rebend.Checked)
                        {
                            Test_SrNo = 3;
                            var a = dc.Test(Test_SrNo, "", 0, "ST", "", 0);
                            foreach (var n in a)
                            {
                                TestId = Convert.ToInt32(n.TEST_Id.ToString());
                                dc.SteelTest_Update(RefNo, TestId, false);
                            }
                        }
                        if (WeightPerMeter.Checked)
                        {
                            Test_SrNo = 4;
                            var a = dc.Test(Test_SrNo, "", 0, "ST", "", 0);
                            foreach (var n in a)
                            {
                                TestId = Convert.ToInt32(n.TEST_Id.ToString());
                                dc.SteelTest_Update(RefNo, TestId, false);
                            }
                        }
                        if (Bend.Checked)
                        {
                            Test_SrNo = 5;
                            var a = dc.Test(Test_SrNo, "", 0, "ST", "", 0);
                            foreach (var n in a)
                            {
                                TestId = Convert.ToInt32(n.TEST_Id.ToString());
                                dc.SteelTest_Update(RefNo, TestId, false);
                            }
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
                    if (UC_InwardHeader1.OtherClient == false && UC_InwardHeader1.BillNo != "0" && Convert.ToInt32(lblNoOfCoupons.Text) <=0)
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
                            BillNo = bill.UpdateBill("ST", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
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
                    //
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
                else
                {
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Text = "Please try again...";
                    lblMsg.Visible = true;
                }
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
        protected int getSteelTestId(int testNo, int diameter)
        {
            int testId = 0;
            return testId;
        }
        protected void TextChanged(object sender, EventArgs e)
        {
            TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
            if (txtSubsets.Text != "")
            {
                if (Convert.ToInt32(txtSubsets.Text) < grdSteelInward.Rows.Count)
                {
                    for (int i = grdSteelInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdSteelInward.Rows.Count > 1)
                        {
                            DeleteRowSteelInward(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdSteelInward.Rows.Count)
                {
                    for (int i = grdSteelInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
                    {
                        AddRowSteelInward();
                    }
                }
            }
        }
        protected void Click(object sender, EventArgs e)
        {
            ViewState["SteelInwardTable"] = null;
            LoadCouponList();
            if (UC_InwardHeader1.EnquiryNo != "" && UC_InwardHeader1.EnqNoApp != "" && UC_InwardHeader1.EnqNoApp != "0")
            {
                gridAppData();
            }
            else
            {
                AddRowSteelInward();
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
            UC_InwardHeader1.InwdStatus = "Add";
            ChkAll.Checked = false; 
            lnkTemp_Click(sender, e);
        }
        public void gridAppData()
        {
            int i = 0, totalQty = 0;
            var res = dc.TestRequestDetails_View_Inward(Convert.ToInt32(UC_InwardHeader1.EnqNoApp), Convert.ToInt32(UC_InwardHeader1.MaterialId), 0, "ST").ToList();
            foreach (var a in res)
            {
                AddRowSteelInward();
                TextBox txtDiameter = (TextBox)grdSteelInward.Rows[i].Cells[1].FindControl("txtDiameter");
                TextBox txtQuantity = (TextBox)grdSteelInward.Rows[i].Cells[2].FindControl("txtQuantity");
                TextBox txtIdMark = (TextBox)grdSteelInward.Rows[i].Cells[3].FindControl("txtIdMark");
                DropDownList ddlSteelType = (DropDownList)grdSteelInward.Rows[i].Cells[4].FindControl("ddlSteelType");
                DropDownList ddlGrade = (DropDownList)grdSteelInward.Rows[i].Cells[5].FindControl("ddlGrade");
                TextBox txtDescription = (TextBox)grdSteelInward.Rows[i].Cells[11].FindControl("txtDescription");
                CheckBox chkSupplier = (CheckBox)grdSteelInward.Rows[i].Cells[12].FindControl("chkSupplier");
                DropDownList ddlSupplier = (DropDownList)grdSteelInward.Rows[i].Cells[13].FindControl("ddlSupplier");

                txtDiameter.Text = a.diameter;
                //txtQuantity.Text = a.material_quantity.ToString();
                txtQuantity.Text = a.no_of_specimen.ToString();
                if (txtQuantity.Text != "")
                    totalQty += Convert.ToInt32(txtQuantity.Text);
                txtIdMark.Text = a.Idmark1;
                //ddlSteelType.SelectedItem.Text = a.STINWD_SteelType_var.ToString();
                ddlGrade.SelectedItem.Text = a.specification;
                if (a.supplier != "")
                {
                    chkSupplier.Checked = true;
                }
                else
                {
                    chkSupplier.Checked = false;
                }
                txtDescription.Text = a.description;
                if (a.make != null && a.make != "")
                    txtDescription.Text += ", Make - " + a.make.ToString();
                if (ddlSupplier.Items.FindByText(a.supplier) != null)
                    ddlSupplier.Items.FindByText(a.supplier).Selected = true;
                else
                {
                    int suppId = dc.Supplier_Update(a.supplier, true);
                    for (int j = 0; j < grdSteelInward.Rows.Count; j++)
                    {
                        DropDownList ddlSupplier1 = (DropDownList)grdSteelInward.Rows[j].FindControl("ddlSupplier");
                        ddlSupplier1.Items.Add(new ListItem(a.supplier, suppId.ToString()));
                    }
                    ddlSupplier.Items.FindByText(a.supplier).Selected = true;
                }
                //test
                CheckBox chkTensileStrength = (CheckBox)grdSteelInward.Rows[i].Cells[6].FindControl("chkTensileStrength");
                CheckBox chkElongation = (CheckBox)grdSteelInward.Rows[i].Cells[7].FindControl("chkElongation");
                CheckBox chkRebend = (CheckBox)grdSteelInward.Rows[i].Cells[8].FindControl("chkRebend");
                CheckBox chkWeightPerMeter = (CheckBox)grdSteelInward.Rows[i].Cells[9].FindControl("chkWeightPerMeter");
                CheckBox chkBend = (CheckBox)grdSteelInward.Rows[i].Cells[10].FindControl("chkBend");
                
                var test = dc.TestRequestDetails_View_ForPrint(a.TestReqId);
                foreach (var t in test)
                {
                    if (t.test_name.Contains("Tensile Strength") == true)
                    {
                        chkTensileStrength.Checked = true;
                    }
                    if (t.test_name.Contains("Percentage Elongation") == true)
                    {
                        chkElongation.Checked = true;
                    }
                    if (t.test_name.Contains("Rebend") == true)
                    {
                        chkRebend.Checked = true;
                    }
                    else if (t.test_name.Contains("Weight Per Meter") == true)
                    {
                        chkWeightPerMeter.Checked = true;
                    }
                    else if (t.test_name.Contains("Bend") == true)
                    {
                        chkBend.Checked = true;
                    }
                }
                i++;
            }
            if (totalQty > 0)
                UC_InwardHeader1.TotalQty = totalQty.ToString();
            UC_InwardHeader1.Subsets = grdSteelInward.Rows.Count.ToString();
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
            if (valid == true && Convert.ToInt32(lblNoOfCoupons.Text ) <=0 )
            {
                if (UC_InwardHeader1.POFileName == "" && UC_InwardHeader1.OtherClient == false )
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
                for (int i = 0; i < grdSteelInward.Rows.Count; i++)
                {
                    TextBox box1 = (TextBox)grdSteelInward.Rows[i].Cells[1].FindControl("txtDiameter");
                    TextBox box2 = (TextBox)grdSteelInward.Rows[i].Cells[2].FindControl("txtQuantity");
                    TextBox box3 = (TextBox)grdSteelInward.Rows[i].Cells[3].FindControl("txtIdMark");
                    DropDownList box4 = (DropDownList)grdSteelInward.Rows[i].Cells[4].FindControl("ddlSteelType");
                    DropDownList box5 = (DropDownList)grdSteelInward.Rows[i].Cells[5].FindControl("ddlGrade");
                    CheckBox box6 = (CheckBox)grdSteelInward.Rows[i].Cells[6].FindControl("chkTensileStrength");
                    CheckBox box7 = (CheckBox)grdSteelInward.Rows[i].Cells[7].FindControl("chkElongation");
                    CheckBox box8 = (CheckBox)grdSteelInward.Rows[i].Cells[8].FindControl("chkRebend");
                    CheckBox box9 = (CheckBox)grdSteelInward.Rows[i].Cells[9].FindControl("chkWeightPerMeter");
                    CheckBox box10 = (CheckBox)grdSteelInward.Rows[i].Cells[10].FindControl("chkBend");
                    TextBox box11 = (TextBox)grdSteelInward.Rows[i].Cells[11].FindControl("txtDescription");
                    CheckBox chkSupplier = (CheckBox)grdSteelInward.Rows[i].Cells[12].FindControl("chkSupplier");
                    DropDownList ddlSupplier = (DropDownList)grdSteelInward.Rows[i].Cells[13].FindControl("ddlSupplier");
                    if (box1.Text == "")
                    {
                        lblMsg.Text = "Enter Diameter for Sr No " + (i + 1) + ".";
                        box1.Focus();
                        valid = false;
                        break;
                    }
                    else if (box2.Text == "")
                    {
                        lblMsg.Text = "Enter Quantity for Sr No " + (i + 1) + ".";
                        box2.Focus();
                        valid = false;
                        break;
                    }
                    else if (Convert.ToInt32(box2.Text) <= 0)
                    {
                        lblMsg.Text = "Quantity should be greater than zero for Sr No " + (i + 1) + " .";
                        box2.Focus();
                        valid = false;
                        break;
                    }
                    else if (box3.Text == "")
                    {
                        lblMsg.Text = "Enter Id Mark for Sr No " + (i + 1) + ".";
                        box3.Focus();
                        valid = false;
                        break;
                    }
                    else if (box4.Text == "" || box4.Text == "Select")
                    {
                        lblMsg.Text = "Enter Steel Type for Sr No " + (i + 1) + ".";
                        box4.Focus();
                        valid = false;
                        break;
                    }
                    else if (box5.Text == "" || box5.Text == "Select")
                    {
                        lblMsg.Text = "Enter Grade for Sr No " + (i + 1) + ".";
                        box5.Focus();
                        valid = false;
                        break;
                    }
                    else if (box6.Checked == false && box7.Checked == false && box8.Checked == false && box9.Checked == false && box10.Checked == false)
                    {
                        lblMsg.Text = "Select at least one Test for Sr No " + (i + 1) + ".";
                        box6.Focus();
                        valid = false;
                        break;
                    }
                    else if (box11.Text == "")
                    {
                        lblMsg.Text = "Enter Description for Sr No " + (i + 1) + ".";
                        box11.Focus();
                        valid = false;
                        break;
                    }
                    else if (chkSupplier.Checked && ddlSupplier.SelectedItem.Text == "---Select---")
                    {
                        lblMsg.Text = "Select Supplier or Manufacturer for Sr No " + (i + 1) + ".";
                        ddlSupplier.Focus();
                        valid = false;
                        break;
                    }
                    sumQty += Convert.ToInt32(box2.Text);
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
        public void DisableSupplierName()
        {
            for (int i = 0; i < grdSteelInward.Rows.Count; i++)
            {
                TextBox txtSupplier = (TextBox)grdSteelInward.Rows[i].Cells[12].FindControl("txtSupplier");
                TextBox txtManufactur = (TextBox)grdSteelInward.Rows[i].Cells[13].FindControl("txtManufactur");
                if (txtSupplier.Text != "")
                {
                    txtManufactur.Text = string.Empty;
                    txtManufactur.Enabled = false;
                }
                else
                {
                    txtManufactur.Enabled = true;
                }
                if (txtManufactur.Text != "")
                {
                    txtSupplier.Text = string.Empty;
                    txtSupplier.Enabled = false;
                }
                else
                {
                    txtSupplier.Enabled = true;
                }
            }

        }
        protected void txtSupplier_OnTextChanged(object sender, EventArgs e)
        {
            DisableSupplierName();
        }
        protected void txtManufactur_OnTextChanged(object sender, EventArgs e)
        {
            DisableSupplierName();
        }
        //protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("Enquiry_List.aspx");
        //}
        public void getSteelInwardTesting()
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
            var res = dc.AllInward_View("ST", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var a in res)
            {
                AddRowSteelInward();
                TextBox txtDiameter = (TextBox)grdSteelInward.Rows[i].Cells[1].FindControl("txtDiameter");
                TextBox txtQuantity = (TextBox)grdSteelInward.Rows[i].Cells[2].FindControl("txtQuantity");
                TextBox txtIdMark = (TextBox)grdSteelInward.Rows[i].Cells[3].FindControl("txtIdMark");
                DropDownList ddlSteelType = (DropDownList)grdSteelInward.Rows[i].Cells[4].FindControl("ddlSteelType");
                DropDownList ddlGrade = (DropDownList)grdSteelInward.Rows[i].Cells[5].FindControl("ddlGrade");
                TextBox txtDescription = (TextBox)grdSteelInward.Rows[i].Cells[11].FindControl("txtDescription");
                CheckBox chkSupplier = (CheckBox)grdSteelInward.Rows[i].Cells[12].FindControl("chkSupplier");
                DropDownList ddlSupplier = (DropDownList)grdSteelInward.Rows[i].Cells[13].FindControl("ddlSupplier");
                txtDiameter.Text = a.STINWD_Diameter_tint.ToString();
                txtQuantity.Text = a.STINWD_Quantity_tint.ToString();
                txtIdMark.Text = a.STINWD_IdMark_var.ToString();
                ddlSteelType.SelectedItem.Text = a.STINWD_SteelType_var.ToString();
                //ddlGrade.SelectedItem.Text = "Fe " + "" + a.STINWD_Grade_var.ToString();
                ddlGrade.SelectedItem.Text = a.STINWD_Grade_var.ToString();
                if (a.STINWD_SupplierFlag_bit == true)
                {
                    chkSupplier.Checked = true;
                }
                else
                {
                    chkSupplier.Checked = false;
                }
                txtDescription.Text = a.STINWD_Description_var.ToString();
                if (ddlSupplier.Items.FindByText(a.STINWD_SupplierName_var) != null)
                    ddlSupplier.Items.FindByText(a.STINWD_SupplierName_var).Selected = true;
                i++;
            }
            gridCheckBx();
            int count = grdSteelInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.EnquiryNo = "";
        }

        public void gridCheckBx()
        {
            int i = 0;
            var res = dc.AllInward_View("ST", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var a in res)
            {
                CheckBox chkSupplier = (CheckBox)grdSteelInward.Rows[i].Cells[12].FindControl("chkSupplier");
                int CheckBoxSelect = 0;
                int TestId = 0;
                var chk = dc.AllInward_View("ST", 0, a.STINWD_ReferenceNo_var.ToString());
                foreach (var ch in chk)
                {
                    TestId = Convert.ToInt32(ch.STTEST_TEST_Id);
                    var sp = dc.Test_View(0, TestId, "", 0, 0, 0);
                    foreach (var b in sp)
                    {
                        CheckBoxSelect = Convert.ToInt32(b.TEST_Sr_No);
                    }
                    CheckBox chkTensileStrength = (CheckBox)grdSteelInward.Rows[i].Cells[6].FindControl("chkTensileStrength");
                    CheckBox chkElongation = (CheckBox)grdSteelInward.Rows[i].Cells[7].FindControl("chkElongation");
                    CheckBox chkRebend = (CheckBox)grdSteelInward.Rows[i].Cells[8].FindControl("chkRebend");
                    CheckBox chkWeightPerMeter = (CheckBox)grdSteelInward.Rows[i].Cells[9].FindControl("chkWeightPerMeter");
                    CheckBox chkBend = (CheckBox)grdSteelInward.Rows[i].Cells[10].FindControl("chkBend");
                    if (CheckBoxSelect == 1)
                    {
                        chkTensileStrength.Checked = true;
                    }
                    if (CheckBoxSelect == 2)
                    {
                        chkElongation.Checked = true;
                    }
                    if (CheckBoxSelect == 3)
                    {
                        chkRebend.Checked = true;
                    }
                    if (CheckBoxSelect == 4)
                    {
                        chkWeightPerMeter.Checked = true;
                    }
                    if (CheckBoxSelect == 5)
                    {
                        chkBend.Checked = true;
                    }
                    if (CheckBoxSelect == 6 || CheckBoxSelect == 7 || CheckBoxSelect == 8)
                    {
                        chkTensileStrength.Checked = true;
                        chkElongation.Checked = true;
                    }
                }
                if (a.STINWD_SupplierFlag_bit == true)
                {
                    chkSupplier.Checked = true;
                }
                else
                {
                    chkSupplier.Checked = false;
                }
                i++;
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
                reportStr = rpt.getDetailReportSteelInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Steel_Inward", reportStr);
        }
        bool buttonClicked = false;
        protected string getDetailReportSteelInward()
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
            string castingDate = "";
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (buttonClicked == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Steel Test Lab Sheet </b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Steel Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("ST", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }


            var b = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, "ST", null, null);

            foreach (var nt in b)
            {
                if (buttonClicked == true)
                {
                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "ST" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "ST" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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

                    DateTime Expectdate = Convert.ToDateTime(nt.INWD_ReceivedDate_dt);
                    castingDate = (DateTime.Parse(Expectdate.ToString()).AddDays(3)).ToString("dd/MM/yyyy");

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
                        "<td height=19><font size=2>" + "ST" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "ST" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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

                    DateTime Expectdate = Convert.ToDateTime(nt.INWD_ReceivedDate_dt);
                    castingDate = (DateTime.Parse(Expectdate.ToString()).AddDays(3)).ToString("dd/MM/yyyy");

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
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Dia(mm)</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";

            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Test</b></font></td>";

            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Grade</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sample Description</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Supplier Name/Manufacturer Name</b></font></td>";

            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("ST", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.STINWD_Diameter_tint + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.STINWD_Quantity_tint + "</font></td>";
                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + c.STINWD_IdMark_var + "</font></td>";



                mySql += "<td > ";
                mySql += "<table border=0 cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";

                var wttest = dc.AllInward_View("ST", 0, c.STINWD_ReferenceNo_var.ToString());
                foreach (var wt in wttest)
                {
                    var c1 = dc.Test_View(0, Convert.ToInt32(wt.STTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var n2 in c1)
                    {
                        mySql += "<tr>";
                        mySql += "<td>";//

                        mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
                        mySql += "<tr>";
                        mySql += "<td width= 30% align=left valign=top height=19 ><font size=2>&nbsp;" + n2.TEST_Name_var.ToString() + "</font></td>";


                        mySql += "</tr>";
                        mySql += "</table>";
                        mySql += "</td>";
                        mySql += "</tr>";
                    }
                }
                mySql += "</table>";
                mySql += "</td>";


                //mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + "Fe " + "" + c.STINWD_Grade_var + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.STINWD_Grade_var + "</font></td>";
                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + c.STINWD_Description_var + "</font></td>";
                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + c.STINWD_SupplierName_var + "</font></td>";


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
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Expected Date Of Report - " + castingDate.ToString() + "</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "* Tensile Test shall be carried out as per IS 1786 - 2008,Clause No. 8.2 or 8.2.1 & 8.2.2 . Client may indicate his choice before our testing schedule." + "</font></td>";
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
                reportStr = rpt.getDetailReportSteelInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), true);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Steel_LabSheet", reportStr);
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
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "ST")));
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



        protected void LoadCouponList()
        {
            int ClientId = 0, SiteId = 0;
//            lblNoOfCoupons.Text = "0";
            if (UC_InwardHeader1.EnquiryNo != "")
            {
                var enquiry = dc.Enquiry_View(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 1, 0);
                foreach (var enq in enquiry)
                {
                    ClientId = enq.CL_Id;
                    SiteId = enq.SITE_Id;
                }
                var couponsitespec = dc.Coupon_View_SitewiseST(ClientId, SiteId, 0, recdDate).ToList();
                lblNoOfCoupons.Text = couponsitespec.Count().ToString();
                if (couponsitespec.Count() == 0)
                {

                    var coupon = dc.Coupon_ViewST("", 0, 0, ClientId, SiteId, 0, recdDate).ToList();
                    lblNoOfCoupons.Text = coupon.Count().ToString();
                }
            }
            if (UC_InwardHeader1.RecType != "" && UC_InwardHeader1.RecordNo != "")
            {
                var Modify = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, UC_InwardHeader1.RecType.ToString(), null, null);
                foreach (var n in Modify)
                {
                    ClientId = Convert.ToInt32(n.INWD_CL_Id);
                    SiteId = Convert.ToInt32(n.INWD_SITE_Id);
                }
                var couponsitespec = dc.Coupon_View_SitewiseST(ClientId, SiteId, 0, recdDate).ToList();
                lblNoOfCoupons.Text = couponsitespec.Count().ToString();
                if (couponsitespec.Count() > 0)
                {
                    var couponsitespec2 = dc.Coupon_View_SitewiseST(ClientId, SiteId, Convert.ToInt32(UC_InwardHeader1.RecordNo), recdDate).ToList();
                    lblNoOfCoupons.Text = couponsitespec2.Count().ToString();
                }
                else
                {
                    var coupon = dc.Coupon_ViewST("", 0, 0, ClientId, SiteId, Convert.ToInt32(UC_InwardHeader1.RecordNo), recdDate).ToList();
                    lblNoOfCoupons.Text = coupon.Count().ToString();
                }
            }
            if (lblNoOfCoupons.Text == "")
            {
                lblNoOfCoupons.Text = "0";
            }
        }

    }
}
