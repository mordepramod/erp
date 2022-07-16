using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace DESPLWEB
{
    public partial class Cube_Inward : System.Web.UI.Page
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
                lblheading.Text = "Cube Inward";

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

                LoadCouponList();
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    AddRowCubeInward();
                }
                if (UC_InwardHeader1.RecType != "")
                {
                    DisplayCubeTest();
                }
                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowCubeInward();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "CT";
                }
                lnkSave.Visible = true;

            }
        }

        #region add/delete row cube grid
        protected void AddRowCubeInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CubeInwardTable"] != null)
            {
                GetCurrentDataCubeInward();
                dt = (DataTable)ViewState["CubeInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCSQty", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDateOfCasting", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlGrade", typeof(string)));
                dt.Columns.Add(new DataColumn("txtNatureOfWork", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSchedule", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDateOfTesting", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtIdMark"] = string.Empty;
            dr["txtCSQty"] = string.Empty;
            dr["txtDateOfCasting"] = string.Empty;
            dr["ddlGrade"] = string.Empty;
            dr["txtNatureOfWork"] = string.Empty;
            dr["txtDescription"] = string.Empty;
            dr["txtSchedule"] = string.Empty;
            dr["txtDateOfTesting"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CubeInwardTable"] = dt;
            grdCubeInward.DataSource = dt;
            grdCubeInward.DataBind();
            SetPreviousDataCubeInward();
        }
        protected void DeleteRowCubeInward(int rowIndex)
        {
            GetCurrentDataCubeInward();
            DataTable dt = ViewState["CubeInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CubeInwardTable"] = dt;
            grdCubeInward.DataSource = dt;
            grdCubeInward.DataBind();
            SetPreviousDataCubeInward();
        }
        protected void SetPreviousDataCubeInward()
        {
            DataTable dt = (DataTable)ViewState["CubeInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txtIdMark");
                TextBox box3 = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txtCSQty");
                TextBox box4 = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txtDateOfCasting");
                DropDownList box5 = (DropDownList)grdCubeInward.Rows[i].Cells[4].FindControl("ddlGrade");
                TextBox box6 = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txtNatureOfWork");
                TextBox box7 = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txtDescription");
                TextBox box8 = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txtSchedule");
                TextBox box9 = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txtDateOfTesting");

                grdCubeInward.Rows[i].Cells[0].Text = (i + 1).ToString();
                box2.Text = dt.Rows[i]["txtIdMark"].ToString();
                box3.Text = dt.Rows[i]["txtCSQty"].ToString();
                box4.Text = dt.Rows[i]["txtDateOfCasting"].ToString();
                box5.Text = dt.Rows[i]["ddlGrade"].ToString();
                box6.Text = dt.Rows[i]["txtNatureOfWork"].ToString();
                box7.Text = dt.Rows[i]["txtDescription"].ToString();
                box8.Text = dt.Rows[i]["txtSchedule"].ToString();
                box9.Text = dt.Rows[i]["txtDateOfTesting"].ToString();

            }
        }
        protected void GetCurrentDataCubeInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtCSQty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDateOfCasting", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlGrade", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtNatureOfWork", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtSchedule", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDateOfTesting", typeof(string)));
            for (int i = 0; i < grdCubeInward.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txtIdMark");
                TextBox box3 = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txtCSQty");
                TextBox box4 = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txtDateOfCasting");
                DropDownList box5 = (DropDownList)grdCubeInward.Rows[i].Cells[4].FindControl("ddlGrade");
                TextBox box6 = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txtNatureOfWork");
                TextBox box7 = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txtDescription");
                TextBox box8 = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txtSchedule");
                TextBox box9 = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txtDateOfTesting");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txtIdMark"] = box2.Text;
                drRow["txtCSQty"] = box3.Text;
                drRow["txtDateOfCasting"] = box4.Text;
                drRow["ddlGrade"] = box5.Text;
                drRow["txtNatureOfWork"] = box6.Text;
                drRow["txtDescription"] = box7.Text;
                drRow["txtSchedule"] = box8.Text;
                drRow["txtDateOfTesting"] = box9.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CubeInwardTable"] = dtTable;

        }
        #endregion

        protected void LoadCouponList()
        {
            int ClientId = 0, SiteId = 0;
            if (UC_InwardHeader1.EnquiryNo != "")
            {
                var enquiry = dc.Enquiry_View(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 1, 0);
                foreach (var enq in enquiry)
                {
                    ClientId = enq.CL_Id;
                    SiteId = enq.SITE_Id;
                }
                var couponsitespec = dc.Coupon_View_Sitewise(ClientId, SiteId, 0, recdDate).ToList();
                lblNoOfCoupons.Text = couponsitespec.Count().ToString();
                if (couponsitespec.Count() == 0)
                {
                    var coupon = dc.Coupon_View("", 0, 0, ClientId, SiteId, 0, recdDate).ToList();
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
                var couponsitespec = dc.Coupon_View_Sitewise(ClientId, SiteId, 0, recdDate).ToList();
                lblNoOfCoupons.Text = couponsitespec.Count().ToString();
                if (couponsitespec.Count() > 0)
                {
                    var couponsitespec2 = dc.Coupon_View_Sitewise(ClientId, SiteId, Convert.ToInt32(UC_InwardHeader1.RecordNo), recdDate).ToList();
                    lblNoOfCoupons.Text = couponsitespec2.Count().ToString();
                }
                else
                {
                    var coupon = dc.Coupon_View("", 0, 0, ClientId, SiteId, Convert.ToInt32(UC_InwardHeader1.RecordNo), recdDate).ToList();
                    lblNoOfCoupons.Text = coupon.Count().ToString();
                }
            }
            if (lblNoOfCoupons.Text == "")
            {
                lblNoOfCoupons.Text = "0";
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
                bool updateFlag = true;
                string RefNo, SetOfRecord, TestType, CouponNo;
                string totalCost = "0";
                clsData clsObj = new clsData();
                int SrNo, TestId, CouponCount;
                string[] strDate = new string[3];
                DateTime ReceivedDate = DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null);
                DateTime d1 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);
                TestType = ddlTestType.SelectedItem.Text;
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    dc.Inward_Update_2(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "CT", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, UC_InwardHeader1.POFileName, false, 0, true);
                }
                else
                {
                    //Int32 NewrecNo = 0;
                    try
                    {
                        UC_InwardHeader1.RecordNo = clsObj.GetnUpdateRecordNo("CT").ToString();
                        //UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                        UC_InwardHeader1.ReferenceNo = clsObj.insertRecordTable_2(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), Convert.ToInt32(UC_InwardHeader1.RecordNo), "CT").ToString();
                        //UC_InwardHeader1.ReferenceNo = clsObj.getRefNo(Convert.ToInt32(UC_InwardHeader1.EnquiryNo)).ToString();
                        dc.Inward_Update_2(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "CT", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, UC_InwardHeader1.POFileName, UC_InwardHeader1.OtherClient, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
                        //dc.Enquiry_Update_Status(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 2);
                    }
                    catch
                    {
                        updateFlag = false;
                    }
                }
                if (updateFlag == true)
                {
                    //dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "CT", "", null, null, false, false, false, false, false, false, true);//Delete MIS Record
                    TestType = ddlTestType.SelectedItem.Text;
                    TestId = 0;
                    var test = dc.Test((ddlTestType.SelectedIndex + 1), "", 0, "CT", "", 0);
                    foreach (var t in test)
                    {
                        TestId = Convert.ToInt32(t.TEST_Id);
                    }
                    //delete coupon no
                    dc.Coupon_Update("", 0, 0, 0, null, 0, null, "", Convert.ToInt32(UC_InwardHeader1.RecordNo), null, false, false);
                    CouponCount = 0;
                    dc.CubeInward_Update("CT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "", "", 0, 0, 0, null, "", "", "", 0, "", "","", "", 0, "", "", 0, 0, null, null, "", "", 0, 0, false, true);
                    //if (lblRptClientId.Text != "" && lblRptClientId.Text != "0")
                    //{
                    //    dc.Inward_Update_ReportClient(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, Convert.ToInt32(lblRptClientId.Text), Convert.ToInt32(lblRptSiteId.Text));
                    //}
                    string mySql, mySql2;
                    mySql = @"insert into tbl_Cube_Inward (CTINWD_RecordType_var,CTINWD_RecordNo_int,CTINWD_ReferenceNo_var,
                    CTINWD_SetOfRecord_var,CTINWD_SrNo_int,CTINWD_Status_tint,CTINWD_Quantity_tint,CTINWD_TestingDate_dt,
                    CTINWD_CastingDate_dt,CTINWD_Description_var, CTINWD_SupplierName_var, CTINWD_RecheckUserId_int,CTINWD_WitnessBy_var,
                    CTINWD_IdMark_var,CTINWD_Grade_var,CTINWD_WorkingNature_var,CTINWD_TestType_var,
                    CTINWD_CouponNo_var,CTINWD_Schedule_tint,CTINWD_TEST_Id,CTINWD_CollectionDate_dt,CTINWD_ReceivedDate_dt,
                    CTINWD_EmailId_var, CTINWD_ContactNo_var, CTINWD_CL_Id, CTINWD_SITE_Id, CTINWD_TestedAt_bit
                    ) values ";
                    mySql2 = @"insert into tbl_MISDetail (MISRecordNo, MISRefNo, MISRecType, MISTestType, MISCollectionDt, MISRecievedDt) values";
                    for (int i = 0; i <= grdCubeInward.Rows.Count - 1; i++)
                    {
                        TextBox IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txtIdMark");
                        TextBox Quantity = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txtCSQty");
                        TextBox CastingDate = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txtDateOfCasting");
                        DropDownList Grade = (DropDownList)grdCubeInward.Rows[i].Cells[4].FindControl("ddlGrade");
                        TextBox NatureOfWork = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txtNatureOfWork");
                        TextBox Description = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txtDescription");
                        TextBox Schedule = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txtSchedule");
                        TextBox TestingDate = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txtDateOfTesting");
                        CastingDate.Text = CastingDate.Text.ToUpper();
                        //strDate = CastingDate.Text.Split('/');
                        //d1 = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
                        //strDate = TestingDate.Text.Split('/');
                        //d2 = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

                        SrNo = Convert.ToInt32(grdCubeInward.Rows[i].Cells[0].Text);
                        RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                        SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                        DateTime txtDateofTestingTxt = DateTime.ParseExact(TestingDate.Text, "dd/MM/yyyy", null);
                        CouponNo = "";
                        int tempQty = 0;
                        if (ddlTestType.SelectedValue == "Concrete Cube" && Convert.ToInt32(lblNoOfCoupons.Text) > 0)
                        {

                            var couponsitespec = dc.Coupon_View_Sitewise(Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), 0, recdDate).ToList();
                            for (int c = 0; c < couponsitespec.Count; c++)
                            {
                                if (tempQty < Convert.ToInt32(Quantity.Text))
                                {
                                    CouponNo = CouponNo + couponsitespec[c].COUP_Id + ",";
                                    dc.Coupon_Update("", couponsitespec[c].COUP_Id, Convert.ToInt32(UC_InwardHeader1.ClientId), 0, null, 1, DateTime.Now, RefNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), null, false, false);
                                    tempQty++;
                                    CouponCount++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (couponsitespec.Count() == 0)
                            {
                                var coupon = dc.Coupon_View("", 0, 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), 0, recdDate).ToList();
                                for (int c = 0; c < coupon.Count; c++)
                                {
                                    if (tempQty < Convert.ToInt32(Quantity.Text))
                                    {
                                        CouponNo = CouponNo + coupon[c].COUP_Id + ",";
                                        dc.Coupon_Update("", coupon[c].COUP_Id, Convert.ToInt32(UC_InwardHeader1.ClientId), 0, null, 1, DateTime.Now, RefNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), null, false, false);
                                        tempQty++;
                                        CouponCount++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        string mGrade = "0";
                        if (Grade.Text != "NA")
                            mGrade = Grade.Text.Replace("M ", "");
                        //dc.CubeInward_Update("CT", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, SrNo, 0, Convert.ToByte(Quantity.Text), txtDateofTestingTxt, CastingDate.Text, Description.Text, "", 0, txt_witnessBy.Text, IdMark.Text, mGrade, NatureOfWork.Text, 0, TestType, CouponNo, Convert.ToInt16(Schedule.Text), TestId, d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false);
                        //dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "CT", RefNo, "CT", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);

                        if (i != 0)
                        {
                            mySql += ",";
                            mySql2 += ",";
                        }

                        mySql += "(" + "'CT'" + "," + UC_InwardHeader1.RecordNo + ",'" + RefNo + "','" + SetOfRecord + "'," + SrNo.ToString() + ", 0," + Convert.ToByte(Quantity.Text).ToString() +
                            ", CONVERT(DATE,'" + txtDateofTestingTxt.ToString("yyyy/MM/dd") + "'), '" + CastingDate.Text + "','" + Description.Text + "', '', 0, '" + txt_witnessBy.Text + "','" + IdMark.Text.ToString() + "','" +
                            mGrade + "','" + NatureOfWork.Text + "', '" + TestType + "', '" + CouponNo + "', " +
                            Schedule.Text + "," + TestId.ToString() + ", CONVERT(DATE,'" + d1.ToString("yyyy/MM/dd") + "'), CONVERT(DATE,'" + ReceivedDate.ToString("yyyy/MM/dd") + "'), '" + UC_InwardHeader1.EmailId.ToString() + "', '" +
                            UC_InwardHeader1.ContactNo + "', " + UC_InwardHeader1.ClientId + "," +  UC_InwardHeader1.SiteId + "," + ddlTestedAt.SelectedValue + ")";

                        mySql2 += "(" + UC_InwardHeader1.RecordNo + ", '" + RefNo + "', 'CT', 'CT', '" + d1.ToString("yyyy/MM/dd") + " " + Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat).ToString().Split(' ')[1] + "', '" + DateTime.Now.ToString("yyyy/MM/dd") + " " + Convert.ToDateTime(DateTime.Now, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat).ToString().Split(' ')[1] + "')";
                        
                    }

                    clsObj.updateCubeInward(mySql);
                    clsObj.updateCubeInward(mySql2);

                    if (CouponCount > 0)
                    {
                        dc.Inward_Update_CouponStatus(Convert.ToInt32(UC_InwardHeader1.RecordNo), "CT", true);
                    }
                    else
                    {
                        dc.Inward_Update_CouponStatus(Convert.ToInt32(UC_InwardHeader1.RecordNo), "CT", false);
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
                    if (UC_InwardHeader1.OtherClient == false && UC_InwardHeader1.BillNo != "0" && CouponCount == 0)
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
                            BillNo = bill.UpdateBill("CT", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
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
                    if (UC_InwardHeader1.InwdStatus != "Edit" && CouponCount == 0)
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
                    ddlEnquiryList.Items.Remove(ddlEnquiryList.SelectedItem.Value);
                    ddlEnquiryList.ClearSelection();
                    //LoadEnquiryList(Convert.ToInt32(UC_InwardHeader1.MaterialId));
                }
                //else
                //{
                //    Label lblMsg = (Label)Master.FindControl("lblMsg");
                //    lblMsg.Text = "Please try again...";
                //    lblMsg.Visible = true;
                //}
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
                if (Convert.ToInt32(txtSubsets.Text) < grdCubeInward.Rows.Count)
                {
                    for (int i = grdCubeInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdCubeInward.Rows.Count > 1)
                        {
                            DeleteRowCubeInward(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdCubeInward.Rows.Count)
                {
                    for (int i = grdCubeInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
                    {
                        AddRowCubeInward();
                    }
                }
            }
        }

        protected void Click(object sender, EventArgs e)
        {
            ddlTestType.Enabled = true;
            ddlTestType.SelectedIndex = 0;
            ddlTestedAt.SelectedIndex = 0;
            LoadCouponList();
            ViewState["CubeInwardTable"] = null;
            if (UC_InwardHeader1.EnquiryNo != "" && UC_InwardHeader1.EnqNoApp != "" && UC_InwardHeader1.EnqNoApp != "0")
                gridAppData();
            else
                AddRowCubeInward();
            lnkSave.Visible = true;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Text = "";
            lblMsg.Visible = false;
            lnkLabSheet.Visible = false;
            lnkPrint.Visible = false;
            lnkBillPrint.Visible = false;
            lblRptClientId.Text = "0";
            lblRptSiteId.Text = "0";
            txt_witnessBy.Text = string.Empty;
            chk_WitnessBy.Checked = false;
            txt_witnessBy.Visible = false;
            UC_InwardHeader1.InwdStatus = "Add";
            lnkTemp_Click(sender, e);
        }

        protected Boolean ValidateData()
        {

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            int sumQty = 0;
            DateTime d1 = new DateTime();
            DateTime d2 = new DateTime();
            DateTime dt;
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
            //else if (UC_InwardHeader1.ProposalRateMatch == false)
            //{
            //    lblMsg.Text = "Please confirm that proposal rates matches with email confirmation / work order.";
            //    CheckBox chkPropRateMatch = (CheckBox)UC_InwardHeader1.FindControl("chkPropRateMatch");
            //    chkPropRateMatch.Focus();
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
                    bool monthlyBilling = true, withoutBill = false;
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
                                monthlyBilling = false;
                            }
                        }
                        if (monthlyBilling == false)
                        {
                            var withoutbill = dc.WithoutBill_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType);
                            if (withoutbill.Count() > 0)
                            {
                                withoutBill = true;
                            }
                        }
                    }
                    else
                    {
                        valid = false;
                        monthlyBilling = false;
                    }
                    int CouponCount = 0;
                    if (valid == true && withoutBill == false)
                    {
                        if (ddlTestType.SelectedValue == "Concrete Cube" && Convert.ToInt32(lblNoOfCoupons.Text) > 0)
                        {
                            int totQty = 0;
                            for (int i = 0; i <= grdCubeInward.Rows.Count - 1; i++)
                            {
                                TextBox Quantity = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txtCSQty");

                                totQty += Convert.ToInt32(Quantity.Text);
                                int tempQty = 0;
                                var couponsitespec = dc.Coupon_View_Sitewise(Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), 0, recdDate).ToList();
                                for (int c = 0; c < couponsitespec.Count; c++)
                                {
                                    if (tempQty < Convert.ToInt32(Quantity.Text))
                                    {
                                        tempQty++;
                                        CouponCount++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                if (couponsitespec.Count() == 0)
                                {
                                    var coupon = dc.Coupon_View("", 0, 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), 0, recdDate).ToList();
                                    for (int c = 0; c < coupon.Count; c++)
                                    {
                                        if (tempQty < Convert.ToInt32(Quantity.Text))
                                        {
                                            tempQty++;
                                            CouponCount++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            if (UC_InwardHeader1.InwdStatus == "Edit" && CouponCount == 0 && monthlyBilling == false && Convert.ToInt32(lblNoOfCoupons.Text) >= totQty)
                            {
                                CouponCount = totQty;
                            }
                        }
                    }
                    if (CouponCount == 0 && monthlyBilling == false && withoutBill == false)
                    {
                        lblMsg.Text = "Please upload PO File";
                        valid = false;
                    }
                }
            }
            if (valid == true)
            {
                for (int i = 0; i < grdCubeInward.Rows.Count; i++)
                {
                    TextBox box2 = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txtIdMark");
                    TextBox Qty = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txtCSQty");
                    TextBox txtDateOfCasting = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txtDateOfCasting");
                    DropDownList box5 = (DropDownList)grdCubeInward.Rows[i].Cells[4].FindControl("ddlGrade");
                    TextBox box6 = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txtNatureOfWork");
                    TextBox box7 = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txtDescription");
                    TextBox txtSchedule = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txtSchedule");
                    TextBox txtDateOfTesting = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txtDateOfTesting");
                    txtDateOfCasting.Text = txtDateOfCasting.Text.ToUpper();
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
                    else if (txtDateOfCasting.Text == "")
                    {
                        lblMsg.Text = "Enter Date Of Casting for Sr No. " + (i + 1) + ".";
                        txtDateOfCasting.Focus();
                        valid = false;
                        break;
                    }
                    else if (DateTime.TryParseExact(txtDateOfCasting.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt) == false)
                    {
                        //strDate = txtDateOfCasting.Text.Split('/');
                        //strDate = Convert.ToDateTime(txtDateOfCasting.Text);
                        //if (strDate[0].Length != 2 || strDate[1].Length != 2 || strDate[2].Length != 4)
                        //{
                        lblMsg.Text = "Invalid Date Of Casting for Sr No. " + (i + 1) + ". Enter Date in dd/MM/yyyy format.";
                        txtDateOfCasting.Focus();
                        valid = false;
                        break;
                        //}
                    }
                    if (box5.Text == "" || box5.Text == "Select")
                    {
                        lblMsg.Text = "Select Grade for Sr No. " + (i + 1) + ".";
                        box5.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtSchedule.Text == "")
                    {
                        lblMsg.Text = "Enter Schedule for Sr No. " + (i + 1) + ".";
                        txtSchedule.Focus();
                        valid = false;
                        break;
                    }
                    if (txtDateOfTesting.Text == "")
                    {
                        lblMsg.Text = "Enter Date Of Testing for Sr No. " + (i + 1) + ".";
                        txtDateOfTesting.Focus();
                        valid = false;
                        break;
                    }
                    else if (box6.Text == "")
                    {
                        lblMsg.Text = "Enter Nature Of Work for Sr No. " + (i + 1) + ".";
                        box6.Focus();
                        valid = false;
                        break;
                    }
                    else if (box7.Text == "")
                    {
                        lblMsg.Text = "Enter Description for Sr No. " + (i + 1) + ".";
                        box7.Focus();
                        valid = false;
                        break;
                    }
                    else if (Convert.ToInt32(txtSchedule.Text) <= 0)
                    {
                        lblMsg.Text = "Schedule should be greater than zero for Sr No. " + (i + 1) + " .";
                        txtSchedule.Focus();
                        valid = false;
                        break;
                    }

                    //else
                    //{
                    //    strDate = txtDateOfTesting.Text.Split('/');
                    //    if (strDate[0].Length != 2 || strDate[1].Length != 2 || strDate[2].Length != 4)
                    //    {
                    //        dispalyMsg = "Invalid Date Of Testing for row number " + (i + 1) + ".";
                    //        txtDateOfCasting.Focus();
                    //        valid = false;
                    //        break;
                    //    }
                    //}
                    if (txtDateOfCasting.Text != "NA")
                    {
                        d1 = DateTime.ParseExact(txtDateOfCasting.Text, "dd/MM/yyyy", null);
                        // d1 = Convert.ToDateTime(txtDateOfCasting.Text);
                        d2 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);// Convert.ToDateTime(UC_InwardHeader1.CollectionDate);
                        if (txtSchedule.Text != "")
                        {
                            int Schedule = Convert.ToInt32(txtSchedule.Text);
                            //  var txtDateCasting = Convert.ToDateTime(txtDateOfCasting.Text);
                            DateTime txtDateCasting = DateTime.ParseExact(txtDateOfCasting.Text, "dd/MM/yyyy", null);
                            string castingDate = (DateTime.Parse(txtDateCasting.ToString()).AddDays(Schedule)).ToString("dd/MM/yyyy");
                            txtDateOfTesting.Text = castingDate.ToString();
                        }
                        if (d1 > d2)
                        {
                            lblMsg.Text = "Date Of Casting should be less than equal to Collection Date for Sr No. " + (i + 1) + ".";
                            txtDateOfCasting.Focus();
                            valid = false;
                            break;
                        }
                    }
                    string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime txtDateOfTesting1 = DateTime.ParseExact(txtDateOfTesting.Text, "dd/MM/yyyy", null);
                    DateTime currentDate1 = DateTime.ParseExact(CurrentDate, "dd/MM/yyyy", null);
                    //if (UC_InwardHeader1.RecType == null)
                    //{
                    if (UC_InwardHeader1.InwdStatus == "Edit")
                    {
                        DateTime ReceivedDate = DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null);
                        CurrentDate = ReceivedDate.ToString("dd/MM/yyyy");
                        currentDate1 = DateTime.ParseExact(CurrentDate, "dd/MM/yyyy", null);

                    }
                        if (txtDateOfTesting1 < currentDate1)
                        {
                            lblMsg.Text = "Testing Date should be greater than or equal to the Current Date for Sr No." + (i + 1) + "."; ;
                            valid = false;
                            break;
                        }
                   // }
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
                else if (ddlTestType.SelectedValue == "Concrete Cube")
                {
                    if (Convert.ToDecimal(lblNoOfCoupons.Text) > 0 && Convert.ToDecimal(UC_InwardHeader1.TotalQty) > Convert.ToDecimal(lblNoOfCoupons.Text))
                    {
                        lblMsg.Text = "Total Quantity should be less than equal to the total coupon Quantity. ";
                        valid = false;
                    }
                }
                //else
                //{
                //    sumQty = 0;
                //    foreach (ListItem item in chkListCoupon.Items)
                //    {
                //        if (item.Selected == true)
                //            sumQty++;
                //    }
                //    if (Convert.ToDecimal(UC_InwardHeader1.TotalQty) != Convert.ToDecimal(sumQty))
                //    {
                //        lblMsg.Text = "Total Quantity does not match to the total selected coupon Quantity. ";
                //        valid = false;
                //    }
                //}
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
        public void DisplayCubeTest()
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
                recdDate = Convert.ToDateTime(n.INWD_ReceivedDate_dt);
                if (n.INWD_RptCL_Id != null)
                    lblRptClientId.Text = n.INWD_RptCL_Id.ToString();
                if (n.INWD_RptSITE_Id != null)
                    lblRptSiteId.Text = n.INWD_RptSITE_Id.ToString();
                
                
            }
            CubeTestgrid();
        }
        public void CubeTestgrid()
        {
            int i = 0; //int li = 0; 

            var res = dc.AllInward_View("CT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var w in res)
            {
                AddRowCubeInward();
                TextBox IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txtIdMark");
                TextBox Quantity = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txtCSQty");
                TextBox CastingDate = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txtDateOfCasting");
                DropDownList Grade = (DropDownList)grdCubeInward.Rows[i].Cells[4].FindControl("ddlGrade");
                TextBox NatureOfWork = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txtNatureOfWork");
                TextBox Description = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txtDescription");
                TextBox Schedule = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txtSchedule");
                TextBox TestingDate = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txtDateOfTesting");

                IdMark.Text = w.CTINWD_IdMark_var.ToString();
                Quantity.Text = w.CTINWD_Quantity_tint.ToString();
                if (w.CTINWD_CastingDate_dt != "NA")
                {
                    CastingDate.Text = w.CTINWD_CastingDate_dt.ToString();
                }
                else
                {
                    CastingDate.Text = "NA";
                }
                if (w.CTINWD_Grade_var == "0")
                    Grade.SelectedValue = "NA";
                else
                    Grade.SelectedItem.Text = "M " + w.CTINWD_Grade_var; //Convert.ToInt32(Grade.Text.Replace("M ", ""))
                NatureOfWork.Text = w.CTINWD_WorkingNature_var.ToString();
                Description.Text = w.CTINWD_Description_var.ToString();
                Schedule.Text = w.CTINWD_Schedule_tint.ToString();
                if (w.CTINWD_TestingDate_dt != null)
                {
                    TestingDate.Text = Convert.ToDateTime(w.CTINWD_TestingDate_dt).ToString("dd/MM/yyyy");
                }
                i++;

                ddlTestType.SelectedItem.Text = w.CTINWD_TestType_var.ToString();
                ddlTestedAt.SelectedValue = Convert.ToInt32(w.CTINWD_TestedAt_bit).ToString();
                if (w.CTINWD_WitnessBy_var != null)
                {
                    if (w.CTINWD_WitnessBy_var.Trim() != "")
                    {   
                        txt_witnessBy.Text = w.CTINWD_WitnessBy_var;
                        chk_WitnessBy.Checked = true;
                        txt_witnessBy.Visible = true;
                    }
                    else
                    {
                        chk_WitnessBy.Checked = false;
                    }
                }
                else
                {
                    chk_WitnessBy.Checked = false;                                        
                }

            }

            int count = grdCubeInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.EnquiryNo = "";

        }
        public void gridAppData()
        {
            int i = 0,qty=0; //int li = 0; 
            string testDate = "NA";

            var res1 = dc.TestRequestDetails_View_Inward(Convert.ToInt32(UC_InwardHeader1.EnqNoApp), Convert.ToInt32(UC_InwardHeader1.MaterialId), 0, "CT").ToList();
            if (res1.Count > 0)
            {
                foreach (var w in res1)
                {
                    AddRowCubeInward();
                    TextBox IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txtIdMark");
                    TextBox Quantity = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txtCSQty");
                    TextBox CastingDate = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txtDateOfCasting");
                    DropDownList Grade = (DropDownList)grdCubeInward.Rows[i].Cells[4].FindControl("ddlGrade");
                    TextBox NatureOfWork = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txtNatureOfWork");
                    TextBox Description = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txtDescription");
                    TextBox Schedule = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txtSchedule");
                    TextBox TestingDate = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txtDateOfTesting");
                    
                    IdMark.Text = Convert.ToString(w.Idmark1);
                    Quantity.Text = Convert.ToString(w.no_of_specimen);

                    if (w.no_of_specimen != "" && w.no_of_specimen != null)
                        qty += Convert.ToInt32(w.no_of_specimen);

                    if (Convert.ToString(w.casting_dt) != "" && Convert.ToString(w.casting_dt) != "NA" && Convert.ToString(w.casting_dt) != null)
                    {
                        CastingDate.Text = w.casting_dt.ToString();
                    }
                    else
                    {
                        CastingDate.Text = "NA";
                    }
                    
                    //if (w.grade == "0")
                    //    Grade.SelectedValue = "NA";
                    //else
                    //    Grade.SelectedItem.Text = w.grade; //Convert.ToInt32(Grade.Text.Replace("M ", ""))

                    string mgrd = Convert.ToString(w.grade).ToUpper();
                    mgrd = mgrd.Replace("M", "");
                    mgrd = mgrd.Replace("-", "");
                    mgrd = mgrd.Replace(" ", "");
                    mgrd = "M " + mgrd.Trim();

                    if (Grade.Items.FindByValue(mgrd) != null)
                    {
                        Grade.SelectedValue = mgrd;
                    }

                    NatureOfWork.Text = Convert.ToString(w.Location_of_pour);
                    //Description.Text = Convert.ToString(w.Nature_of_work);                    
                    Description.Text = Convert.ToString(w.description);
                    if (w.make != null && w.make != "")
                        Description.Text += ", Make - " + w.make.ToString();
                    DateTime d2;
                    int schedule = 0;
                    if (int.TryParse(w.schedule, out schedule) == true) // w.schedule != "" && w.schedule != null && w.schedule != "Other" && w.schedule != "Select Option")
                    {
                        Schedule.Text = Convert.ToString(w.schedule);
                        if (Convert.ToString(w.casting_dt) != "" && Convert.ToString(w.casting_dt) != "NA" && Convert.ToString(w.casting_dt) != null)
                        {
                            string castingDate = w.casting_dt.ToString();
                            castingDate.Replace(".", "/");
                            castingDate.Replace("-", "/");
                            string[] strDate = castingDate.Split('/');
                            if (strDate.Count() == 3)
                            {
                                d2 = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
                                d2 = d2.AddDays(Convert.ToInt32(w.schedule));
                                testDate = d2.ToString("dd/MM/yyyy");
                            }
                        }
                        else if (Convert.ToString(w.casting_dt) != "NA")
                            testDate = DateTime.Today.ToString("dd/MM/yyyy"); 

                    }
                    //castingdt+schedule =testing if no castingdat then testing is currntdt

                    TestingDate.Text = testDate;
                    i++;

                    //ddlTestType.SelectedItem.Text = w.CTINWD_TestType_var.ToString();
                    //if (Convert.ToString(w.testTable_id) != "" && Convert.ToString(w.testTable_id) != null)
                    //    ddlTestType.SelectedValue = Convert.ToString(w.testTable_id);
                    //ddlTestedAt.SelectedValue = Convert.ToInt32(w.CTINWD_TestedAt_bit).ToString();

                }
            }


            int count = grdCubeInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            //UC_InwardHeader1.EnquiryNo = "";
            UC_InwardHeader1.TotalQty = qty.ToString();
        }

        protected void lnkTemp_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = true;
        }

        protected void txt_Schedule_TextChanged(object sender, EventArgs e)
        {
            //for (int i = 0; i <= grdCubeInward.Rows.Count - 1; i++)
            //{
            //    TextBox txtCastingDate = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txtDateOfCasting");
            //    TextBox txtSchedule = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txtSchedule");
            //    TextBox txtTestingDate = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txtDateOfTesting");
            //    txtCastingDate.Text = txtCastingDate.Text.ToUpper();
            //    if (txtSchedule.Text != "" && txtCastingDate.Text != "")
            //    {
            //        if (txtCastingDate.Text != "NA")
            //        {
            //            int Schedule = Convert.ToInt32(txtSchedule.Text);
            //            //var txtDateCasting = Convert.ToDateTime(txtCastingDate.Text);
            //            DateTime txtDateCasting = DateTime.ParseExact(txtCastingDate.Text, "dd/MM/yyyy", null);
            //            string castingDate = (DateTime.Parse(txtDateCasting.ToString()).AddDays(Schedule)).ToString("dd/MM/yyyy");
            //            txtTestingDate.Text = castingDate.ToString();
            //        }
            //        else
            //        {
            //            txtTestingDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            //        }
            //    }
            //}

            GridViewRow gvr = ((TextBox)sender).NamingContainer as GridViewRow;
            if (gvr != null)
            {
                TextBox txtCastingDate = (TextBox)gvr.FindControl("txtDateOfCasting");
                TextBox txtSchedule = (TextBox)gvr.FindControl("txtSchedule");
                TextBox txtTestingDate = (TextBox)gvr.FindControl("txtDateOfTesting");
                txtCastingDate.Text = txtCastingDate.Text.ToUpper();
                DateTime dt;
                if (txtSchedule.Text != "" && txtCastingDate.Text != ""
                    && DateTime.TryParseExact(txtCastingDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    if (txtCastingDate.Text != "NA")
                    {
                        int Schedule = Convert.ToInt32(txtSchedule.Text);
                        DateTime CastingDate = DateTime.ParseExact(txtCastingDate.Text, "dd/MM/yyyy", null);
                        DateTime TestingDate = CastingDate.AddDays(Schedule);

                        txtTestingDate.Text = (DateTime.Parse(CastingDate.ToString()).AddDays(Schedule)).ToString("dd/MM/yyyy");
                        if (UC_InwardHeader1.InwdStatus != "Edit")
                        {
                            if (TestingDate < DateTime.Now)
                            {
                                txtTestingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                                TestingDate = DateTime.Now;
                                txtSchedule.Text = (TestingDate - CastingDate).Days.ToString();
                            }
                        }

                    }
                    else
                    {
                        txtTestingDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
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
                reportStr = rpt.getDetailReportCubeInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Cube_Inward", reportStr);
        }

        bool buttonClicked = false;
        protected string getDetailReportCubeInward()
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
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Compressive Strength Of Concrete Cube Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Compressive Strength Of Concrete Cube Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("CT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }
            var b = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, "CT", null, null);

            foreach (var nt in b)
            {
                if (buttonClicked == true)
                {
                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CT" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
                        "<td height=19><font size=2>" + "CT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "CT" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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



            mySql += "<td width= 8% align=center valign=top height=19 ><font size=2><b>Date Of Casting</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Grade Of Conc.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Nature Of Work</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Sched. Of Test</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Testing Date</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Coupon Nos.</b></font></td>";
            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("CT", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_IdMark_var + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_Quantity_tint + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_CastingDate_dt.ToString() + "</font></td>";
                if (c.CTINWD_Grade_var == "0")
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "NA" + "</font></td>";
                else
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "M " + c.CTINWD_Grade_var + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_WorkingNature_var + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_Description_var + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_Schedule_tint + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDateTime(c.CTINWD_TestingDate_dt).ToString("dd/MM/yyyy") + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_CouponNo_var.ToString() + "</font></td>";
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
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Testing done as per IS 516-1959." + "</font></td>";
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
                //reportStr = rpt.getDetailReportCubeInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), true);
                reportStr = rpt.getDetailReportCubeInwardLabSheet(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo));
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Cube_LabSheet", reportStr);
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
        //        Response.Redirect("InwardStatus.aspx");
        // }
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
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "CT")));
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

        protected void chk_WitnessBy_CheckedChanged(object sender, EventArgs e)
        {
            txt_witnessBy.Text = string.Empty;
            if (chk_WitnessBy.Checked)
            {
                txt_witnessBy.Visible = true;
                txt_witnessBy.Focus();
            }
            else
            {
                txt_witnessBy.Visible = false;
            }
        }
    }
}
