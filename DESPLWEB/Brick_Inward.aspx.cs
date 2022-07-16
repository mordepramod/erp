using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Brick_Inward : System.Web.UI.Page
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
                lblheading.Text = "Brick Inward";
                LoadSupplierList();
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    AddRowBrickInward();
                }
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    DisplayBrickInwardgridType();
                }
                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowBrickInward();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "BT-";
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

        protected void grdBrickInward_RowDataBound(object sender, GridViewRowEventArgs e)
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
        protected void AddRowBrickInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["BrickInwardTable"] != null)
            {
                GetCurrentDataBrickInward();
                dt = (DataTable)ViewState["BrickInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlTypeOfBrick", typeof(string)));
                dt.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("chkCS", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCSQty", typeof(string)));
                dt.Columns.Add(new DataColumn("chkWA", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWAQty", typeof(string)));
                dt.Columns.Add(new DataColumn("chkDA", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDAQty", typeof(string)));
                dt.Columns.Add(new DataColumn("chkEff", typeof(string)));
                dt.Columns.Add(new DataColumn("txtEffQty", typeof(string)));
                dt.Columns.Add(new DataColumn("chkDensity", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDensityQty", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["ddlTypeOfBrick"] = string.Empty;
            dr["txtIdMark"] = string.Empty;
            dr["chkCS"] = false;
            dr["txtCSQty"] = string.Empty;
            dr["chkWA"] = false;
            dr["txtWAQty"] = string.Empty;
            dr["chkDA"] = false;
            dr["txtDAQty"] = string.Empty;
            dr["chkEff"] = false;
            dr["txtEffQty"] = string.Empty;
            dr["chkDensity"] = false;
            dr["txtDensityQty"] = string.Empty;
            dr["txtDescription"] = string.Empty;
            dr["ddlSupplier"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["BrickInwardTable"] = dt;
            grdBrickInward.DataSource = dt;
            grdBrickInward.DataBind();
            SetPreviousDataBrickInward();
        }
        protected void DeleteRowBrickInward(int rowIndex)
        {
            GetCurrentDataBrickInward();
            DataTable dt = ViewState["BrickInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["BrickInwardTable"] = dt;
            grdBrickInward.DataSource = dt;
            grdBrickInward.DataBind();
            SetPreviousDataBrickInward();
        }
        protected void SetPreviousDataBrickInward()
        {
            DataTable dt = (DataTable)ViewState["BrickInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList box2 = (DropDownList)grdBrickInward.Rows[i].Cells[1].FindControl("ddlTypeOfBrick");
                TextBox box3 = (TextBox)grdBrickInward.Rows[i].Cells[2].FindControl("txtIdMark");
                CheckBox box4 = (CheckBox)grdBrickInward.Rows[i].Cells[3].FindControl("chkCS");
                TextBox box5 = (TextBox)grdBrickInward.Rows[i].Cells[4].FindControl("txtCSQty");
                CheckBox box6 = (CheckBox)grdBrickInward.Rows[i].Cells[5].FindControl("chkWA");
                TextBox box7 = (TextBox)grdBrickInward.Rows[i].Cells[6].FindControl("txtWAQty");
                CheckBox box8 = (CheckBox)grdBrickInward.Rows[i].Cells[7].FindControl("chkDA");
                TextBox box9 = (TextBox)grdBrickInward.Rows[i].Cells[8].FindControl("txtDAQty");
                CheckBox box10 = (CheckBox)grdBrickInward.Rows[i].Cells[9].FindControl("chkEff");
                TextBox box11 = (TextBox)grdBrickInward.Rows[i].Cells[10].FindControl("txtEffQty");
                CheckBox box12 = (CheckBox)grdBrickInward.Rows[i].Cells[11].FindControl("chkDensity");
                TextBox box13 = (TextBox)grdBrickInward.Rows[i].Cells[12].FindControl("txtDensityQty");
                TextBox box14 = (TextBox)grdBrickInward.Rows[i].Cells[13].FindControl("txtDescription");
                DropDownList box15 = (DropDownList)grdBrickInward.Rows[i].Cells[14].FindControl("ddlSupplier");

                grdBrickInward.Rows[i].Cells[0].Text = (i + 1).ToString();
                box2.Text = dt.Rows[i]["ddlTypeOfBrick"].ToString();
                box3.Text = dt.Rows[i]["txtIdMark"].ToString();
                box4.Checked = Convert.ToBoolean(dt.Rows[i]["chkCS"].ToString());
                box5.Text = dt.Rows[i]["txtCSQty"].ToString();
                box6.Checked = Convert.ToBoolean(dt.Rows[i]["chkWA"].ToString());
                box7.Text = dt.Rows[i]["txtWAQty"].ToString();
                box8.Checked = Convert.ToBoolean(dt.Rows[i]["chkDA"].ToString());
                box9.Text = dt.Rows[i]["txtDAQty"].ToString();
                box10.Checked = Convert.ToBoolean(dt.Rows[i]["chkEff"].ToString());
                box11.Text = dt.Rows[i]["txtEffQty"].ToString();
                box12.Checked = Convert.ToBoolean(dt.Rows[i]["chkDensity"].ToString());
                box13.Text = dt.Rows[i]["txtDensityQty"].ToString();
                box14.Text = dt.Rows[i]["txtDescription"].ToString();
                box15.SelectedValue = dt.Rows[i]["ddlSupplier"].ToString();

            }
        }
        protected void GetCurrentDataBrickInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlTypeOfBrick", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chkCS", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtCSQty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chkWA", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWAQty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chkDA", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDAQty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chkEff", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtEffQty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chkDensity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDensityQty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlSupplier", typeof(string)));

            for (int i = 0; i < grdBrickInward.Rows.Count; i++)
            {
                DropDownList box2 = (DropDownList)grdBrickInward.Rows[i].Cells[1].FindControl("ddlTypeOfBrick");
                TextBox box3 = (TextBox)grdBrickInward.Rows[i].Cells[2].FindControl("txtIdMark");
                CheckBox box4 = (CheckBox)grdBrickInward.Rows[i].Cells[3].FindControl("chkCS");
                TextBox box5 = (TextBox)grdBrickInward.Rows[i].Cells[4].FindControl("txtCSQty");
                CheckBox box6 = (CheckBox)grdBrickInward.Rows[i].Cells[5].FindControl("chkWA");
                TextBox box7 = (TextBox)grdBrickInward.Rows[i].Cells[6].FindControl("txtWAQty");
                CheckBox box8 = (CheckBox)grdBrickInward.Rows[i].Cells[7].FindControl("chkDA");
                TextBox box9 = (TextBox)grdBrickInward.Rows[i].Cells[8].FindControl("txtDAQty");
                CheckBox box10 = (CheckBox)grdBrickInward.Rows[i].Cells[9].FindControl("chkEff");
                TextBox box11 = (TextBox)grdBrickInward.Rows[i].Cells[10].FindControl("txtEffQty");
                CheckBox box12 = (CheckBox)grdBrickInward.Rows[i].Cells[11].FindControl("chkDensity");
                TextBox box13 = (TextBox)grdBrickInward.Rows[i].Cells[12].FindControl("txtDensityQty");
                TextBox box14 = (TextBox)grdBrickInward.Rows[i].Cells[13].FindControl("txtDescription");
                DropDownList box15 = (DropDownList)grdBrickInward.Rows[i].Cells[14].FindControl("ddlSupplier");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["ddlTypeOfBrick"] = box2.Text;
                drRow["txtIdMark"] = box3.Text;
                drRow["chkCS"] = box4.Checked;
                drRow["txtCSQty"] = box5.Text;
                drRow["chkWA"] = box6.Checked;
                drRow["txtWAQty"] = box7.Text;
                drRow["chkDA"] = box8.Checked;
                drRow["txtDAQty"] = box9.Text;
                drRow["chkEff"] = box10.Checked;
                drRow["txtEffQty"] = box11.Text;
                drRow["chkDensity"] = box12.Checked;
                drRow["txtDensityQty"] = box13.Text;
                drRow["txtDescription"] = box14.Text;
                drRow["ddlSupplier"] = box15.SelectedValue;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["BrickInwardTable"] = dtTable;

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
                int SrNo;
                string totalCost = "0";
                clsData clsObj = new clsData();
                //DateTime d1 = new DateTime();
                //DateTime ReceivedDate = Convert.ToDateTime(DateTime.Now.Date.ToString());
                DateTime d1 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);
                DateTime ReceivedDate = DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null);
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    var inwd = dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "BT-", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, true);
                    var res = dc.AllInward_View("BT-", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
                    foreach (var a1 in res)
                    {
                        string s = a1.BTINWD_ReferenceNo_var.ToString();
                        dc.BrickTest_Update(a1.BTINWD_ReferenceNo_var.ToString(), 0, 0, true);
                    }
                }
                else
                {
                    Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    NewrecNo = clsObj.GetnUpdateRecordNo("BT");
                    UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                    clsObj.insertRecordTable(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), NewrecNo, "BT-");
                    UC_InwardHeader1.ReferenceNo = clsObj.getRefNo(Convert.ToInt32(UC_InwardHeader1.EnquiryNo)).ToString();
                    dc.Inward_Update_New(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "BT-", 0, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate);


                    //Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    //NewrecNo = clsObj.GetnUpdateRecordNo("BT");
                    //UC_InwardHeader1.RecordNo = NewrecNo.ToString(); 
                    //UC_InwardHeader1.ReferenceNo = dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "BT-", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, false).ToString();
                    //var inward = dc.Inward_View(Convert.ToInt32(UC_InwardHeader1.ReferenceNo), 0, "");
                    //foreach (var inwd in inward)
                    //{
                    //    UC_InwardHeader1.RecordNo = inwd.INWD_RecordNo_int.ToString();
                    //}
                }
                dc.Enquiry_Update_Status(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 2);
                dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "BT-", "", null, null, false, false, false, false, false, false, true);//Delete MIS Record
                dc.ClientSupplier_Update(Convert.ToInt32(UC_InwardHeader1.ClientId), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, null, true);

                dc.BrickInward_Update("BT-", Convert.ToInt32(UC_InwardHeader1.RecordNo), "", "", 0, 0, "", "", 0, "", "", "", null, null, "", "", 0, 0, true);
                if (lblRptClientId.Text != "" && lblRptClientId.Text != "0")
                {
                    dc.Inward_Update_ReportClient(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, Convert.ToInt32(lblRptClientId.Text), Convert.ToInt32(lblRptSiteId.Text));
                }
                for (int i = 0; i <= grdBrickInward.Rows.Count - 1; i++)
                {
                    DropDownList ddlTypeOfBrick = (DropDownList)grdBrickInward.Rows[i].Cells[1].FindControl("ddlTypeOfBrick");
                    TextBox txtIdMark = (TextBox)grdBrickInward.Rows[i].Cells[2].FindControl("txtIdMark");
                    CheckBox chkCS = (CheckBox)grdBrickInward.Rows[i].Cells[3].FindControl("chkCS");
                    TextBox txtCSQty = (TextBox)grdBrickInward.Rows[i].Cells[4].FindControl("txtCSQty");
                    CheckBox chkWA = (CheckBox)grdBrickInward.Rows[i].Cells[5].FindControl("chkWA");
                    TextBox txtWAQty = (TextBox)grdBrickInward.Rows[i].Cells[6].FindControl("txtWAQty");
                    CheckBox chkDA = (CheckBox)grdBrickInward.Rows[i].Cells[7].FindControl("chkDA");
                    TextBox txtDAQty = (TextBox)grdBrickInward.Rows[i].Cells[8].FindControl("txtDAQty");
                    CheckBox chkEff = (CheckBox)grdBrickInward.Rows[i].Cells[9].FindControl("chkEff");
                    TextBox txtEffQty = (TextBox)grdBrickInward.Rows[i].Cells[10].FindControl("txtEffQty");
                    CheckBox chkDensity = (CheckBox)grdBrickInward.Rows[i].Cells[11].FindControl("chkDensity");
                    TextBox txtDensityQty = (TextBox)grdBrickInward.Rows[i].Cells[12].FindControl("txtDensityQty");
                    TextBox txtDescription = (TextBox)grdBrickInward.Rows[i].Cells[13].FindControl("txtDescription");
                    DropDownList ddlSupplier = (DropDownList)grdBrickInward.Rows[i].Cells[14].FindControl("ddlSupplier");

                    SrNo = Convert.ToInt32(grdBrickInward.Rows[i].Cells[0].Text);
                    RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                    dc.BrickInward_Update("BT-", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, 0, txtDescription.Text, ddlSupplier.SelectedItem.Text, 0, "", txtIdMark.Text, ddlTypeOfBrick.Text, d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), false);
                    //   Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat)
                    dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "BT-", RefNo, "BT-", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);
                    dc.ClientSupplier_Update(Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(ddlSupplier.SelectedValue), Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, ReceivedDate, false);

                    int TestId = 0;
                    int Test_SrNo = 0;
                    if (chkCS.Checked == true)
                    {
                        //CheckBox = "Compressive Strength";
                        Test_SrNo = 1;
                        var a = dc.Test(Test_SrNo, "", 0, "BT-", "", 0);
                        foreach (var n in a)
                        {
                            TestId = Convert.ToInt32(n.TEST_Id.ToString());

                            dc.BrickTest_Update(RefNo, TestId, Convert.ToByte(txtCSQty.Text), false);
                        }
                    }
                    if (chkWA.Checked == true)
                    {
                        Test_SrNo = 2;
                        //CheckBox = "Water Absorption";
                        var a = dc.Test(Test_SrNo, "", 0, "BT-", "", 0);
                        foreach (var n in a)
                        {
                            TestId = Convert.ToInt32(n.TEST_Id.ToString());
                            dc.BrickTest_Update(RefNo, TestId, Convert.ToByte(txtWAQty.Text), false);
                        }

                    }
                    if (chkDA.Checked == true)
                    {
                        //CheckBox = "Dimension Analysis";
                        Test_SrNo = 3;
                        var a = dc.Test(Test_SrNo, "", 0, "BT-", "", 0);
                        foreach (var n in a)
                        {
                            TestId = Convert.ToInt32(n.TEST_Id.ToString());
                            dc.BrickTest_Update(RefNo, TestId, Convert.ToByte(txtDAQty.Text), false);
                        }

                    }
                    if (chkEff.Checked == true)
                    {
                        //CheckBox = "Efflorescence";
                        Test_SrNo = 4;
                        var a = dc.Test(Test_SrNo, "", 0, "BT-", "", 0);
                        foreach (var n in a)
                        {
                            TestId = Convert.ToInt32(n.TEST_Id.ToString());
                            dc.BrickTest_Update(RefNo, TestId, Convert.ToByte(txtEffQty.Text), false);
                        }
                    }
                    if (chkDensity.Checked == true)
                    {
                        Test_SrNo = 5;
                        //CheckBox = "Density";
                        var a = dc.Test(Test_SrNo, "", 0, "BT-", "", 0);
                        foreach (var n in a)
                        {
                            TestId = Convert.ToInt32(n.TEST_Id.ToString());
                            dc.BrickTest_Update(RefNo, TestId, Convert.ToByte(txtDensityQty.Text), false);
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
                        BillNo = bill.UpdateBill("BT-", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
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
                //       // totalCost = Convert.ToString(b.PROINV_NetAmt_num);
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
                //        ProformaInvoiceNo = ProInv.UpdateProformaInvoice("BT-", Convert.ToInt32(UC_InwardHeader1.RecordNo), ProformaInvoiceNo);
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

        public void DisplayBrickInwardgridType()
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
                //////UC_InwardHeader1.ProposalRateMatch = true;
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
            OtherBrickInwardgrid();

        }
        public void OtherBrickInwardgrid()
        {
            int i = 0;
            var otherInward = dc.AllInward_View("BT-", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var n in otherInward)
            {
                AddRowBrickInward();
                DropDownList ddlTypeOfBrick = (DropDownList)grdBrickInward.Rows[i].Cells[1].FindControl("ddlTypeOfBrick");
                TextBox txtIdMark = (TextBox)grdBrickInward.Rows[i].Cells[2].FindControl("txtIdMark");
                TextBox txtDescription = (TextBox)grdBrickInward.Rows[i].Cells[13].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdBrickInward.Rows[i].Cells[14].FindControl("ddlSupplier");
                string RefNo = n.BTINWD_ReferenceNo_var.ToString();
                ddlTypeOfBrick.SelectedItem.Text = n.BTINWD_BrickType_var.ToString();
                txtDescription.Text = n.BTINWD_Description_var.ToString();
                txtIdMark.Text = n.BTINWD_IdMark_var.ToString();
                if (ddlSupplier.Items.FindByText(n.BTINWD_SupplierName_var) != null)
                    ddlSupplier.Items.FindByText(n.BTINWD_SupplierName_var).Selected = true;
                i++;
            }
            gridCheckBx();
            int count = grdBrickInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            DisplayDensity();
            UC_InwardHeader1.EnquiryNo = "";
        }
        public void gridCheckBx()
        {
            int i = 0;
            var res = dc.AllInward_View("BT-", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var a in res)
            {
                string refno = "";
                refno = a.BTINWD_ReferenceNo_var.ToString();
                string CheckBoxSelect = "";
                int TestId = 0;
                var chk = dc.AllInward_View("BT-", 0, refno);
                foreach (var ch in chk)
                {
                    TestId = Convert.ToInt32(ch.BTTEST_TEST_Id);
                    var sp = dc.Test_View(0, TestId, "", 0, 0, 0);
                    foreach (var b in sp)
                    {
                        CheckBoxSelect = b.TEST_Name_var;
                    }
                    CheckBox chkCS = (CheckBox)grdBrickInward.Rows[i].Cells[3].FindControl("chkCS");
                    TextBox txtCSQty = (TextBox)grdBrickInward.Rows[i].Cells[4].FindControl("txtCSQty");
                    CheckBox chkWA = (CheckBox)grdBrickInward.Rows[i].Cells[5].FindControl("chkWA");
                    TextBox txtWAQty = (TextBox)grdBrickInward.Rows[i].Cells[6].FindControl("txtWAQty");
                    CheckBox chkDA = (CheckBox)grdBrickInward.Rows[i].Cells[7].FindControl("chkDA");
                    TextBox txtDAQty = (TextBox)grdBrickInward.Rows[i].Cells[8].FindControl("txtDAQty");
                    CheckBox chkEff = (CheckBox)grdBrickInward.Rows[i].Cells[9].FindControl("chkEff");
                    TextBox txtEffQty = (TextBox)grdBrickInward.Rows[i].Cells[10].FindControl("txtEffQty");
                    CheckBox chkDensity = (CheckBox)grdBrickInward.Rows[i].Cells[11].FindControl("chkDensity");
                    TextBox txtDensityQty = (TextBox)grdBrickInward.Rows[i].Cells[12].FindControl("txtDensityQty");
                    if (CheckBoxSelect == "Compressive Strength")
                    {
                        chkCS.Checked = true;
                        txtCSQty.Text = ch.BTTEST_Quantity_tint.ToString();
                    }
                    if (CheckBoxSelect == "Water Absorption")
                    {
                        chkWA.Checked = true;
                        txtWAQty.Text = ch.BTTEST_Quantity_tint.ToString();
                    }
                    if (CheckBoxSelect == "Dimension Analysis")
                    {
                        chkDA.Checked = true;
                        txtDAQty.Text = ch.BTTEST_Quantity_tint.ToString();
                    }
                    if (CheckBoxSelect == "Efflorescence")
                    {
                        chkEff.Checked = true;
                        txtEffQty.Text = ch.BTTEST_Quantity_tint.ToString();
                    }
                    if (CheckBoxSelect == "Density")
                    {
                        chkDensity.Checked = true;
                        txtDensityQty.Text = ch.BTTEST_Quantity_tint.ToString();
                    }
                }
                i++;
            }
        }

        protected void TextChanged(object sender, EventArgs e)
        {
            TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
            if (txtSubsets.Text != "")
            {
                if (Convert.ToInt32(txtSubsets.Text) < grdBrickInward.Rows.Count)
                {
                    for (int i = grdBrickInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdBrickInward.Rows.Count > 1)
                        {
                            DeleteRowBrickInward(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdBrickInward.Rows.Count)
                {
                    for (int i = grdBrickInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
                    {
                        AddRowBrickInward();
                    }
                }
                DisplayDensity();
            }
        }
        protected void Click(object sender, EventArgs e)
        {
            ViewState["BrickInwardTable"] = null;
             if (UC_InwardHeader1.EnquiryNo != "" && UC_InwardHeader1.EnqNoApp != "" && UC_InwardHeader1.EnqNoApp != "0")
            {
                OtherBrickInwardgridAppData();
            }
            else
            {
                AddRowBrickInward();
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
        public void OtherBrickInwardgridAppData()
        {
            int i = 0, totalQty = 0;
            var res = dc.TestRequestDetails_View_Inward(Convert.ToInt32(UC_InwardHeader1.EnqNoApp), Convert.ToInt32(UC_InwardHeader1.MaterialId), 0, "BT-").ToList();
            foreach (var n in res)
            {
                AddRowBrickInward();
                DropDownList ddlTypeOfBrick = (DropDownList)grdBrickInward.Rows[i].Cells[1].FindControl("ddlTypeOfBrick");
                TextBox txtIdMark = (TextBox)grdBrickInward.Rows[i].Cells[2].FindControl("txtIdMark");
                TextBox txtDescription = (TextBox)grdBrickInward.Rows[i].Cells[13].FindControl("txtDescription");
                DropDownList ddlSupplier = (DropDownList)grdBrickInward.Rows[i].Cells[14].FindControl("ddlSupplier");

                //ddlTypeOfBrick.SelectedItem.Text = n.BTINWD_BrickType_var.ToString();
                txtDescription.Text = n.description;
                if (n.make != null && n.make != "")
                    txtDescription.Text += ", Make - " + n.make.ToString();
                txtIdMark.Text = n.Idmark1;
                if (ddlSupplier.Items.FindByText(n.supplier) != null)
                    ddlSupplier.Items.FindByText(n.supplier).Selected = true;
                else
                {
                    int suppId = dc.Supplier_Update(n.supplier, true);
                    for (int j = 0; j < grdBrickInward.Rows.Count; j++)
                    {
                        DropDownList ddlSupplier1 = (DropDownList)grdBrickInward.Rows[j].FindControl("ddlSupplier");
                        ddlSupplier1.Items.Add(new ListItem(n.supplier, suppId.ToString()));
                    }
                    ddlSupplier.Items.FindByText(n.supplier).Selected = true;
                }
                //test
                CheckBox chkCS = (CheckBox)grdBrickInward.Rows[i].Cells[3].FindControl("chkCS");
                TextBox txtCSQty = (TextBox)grdBrickInward.Rows[i].Cells[4].FindControl("txtCSQty");
                CheckBox chkWA = (CheckBox)grdBrickInward.Rows[i].Cells[5].FindControl("chkWA");
                TextBox txtWAQty = (TextBox)grdBrickInward.Rows[i].Cells[6].FindControl("txtWAQty");
                CheckBox chkDA = (CheckBox)grdBrickInward.Rows[i].Cells[7].FindControl("chkDA");
                TextBox txtDAQty = (TextBox)grdBrickInward.Rows[i].Cells[8].FindControl("txtDAQty");
                CheckBox chkEff = (CheckBox)grdBrickInward.Rows[i].Cells[9].FindControl("chkEff");
                TextBox txtEffQty = (TextBox)grdBrickInward.Rows[i].Cells[10].FindControl("txtEffQty");
                CheckBox chkDensity = (CheckBox)grdBrickInward.Rows[i].Cells[11].FindControl("chkDensity");
                TextBox txtDensityQty = (TextBox)grdBrickInward.Rows[i].Cells[12].FindControl("txtDensityQty");
                string CheckBoxSelect = "";
                int rowQty = 0;
                var test = dc.TestRequestDetails_View_ForPrint(n.TestReqId);
                foreach (var t in test)
                {
                    CheckBoxSelect = t.test_name;
                    if (CheckBoxSelect == "Compressive Strength")
                    {
                        chkCS.Checked = true;
                        txtCSQty.Text = n.material_quantity.ToString();
                        rowQty += Convert.ToInt32(txtCSQty.Text);
                    }
                    else if (CheckBoxSelect == "Water Absorption")
                    {
                        chkWA.Checked = true;
                        txtWAQty.Text = n.material_quantity.ToString();
                        rowQty += Convert.ToInt32(txtWAQty.Text);
                    }
                    else if (CheckBoxSelect == "Dimension Analysis")
                    {
                        chkDA.Checked = true;
                        txtDAQty.Text = n.material_quantity.ToString();
                        rowQty += Convert.ToInt32(txtDAQty.Text);
                    }
                    else if (CheckBoxSelect == "Efflorescence")
                    {
                        chkEff.Checked = true;
                        txtEffQty.Text = n.material_quantity.ToString();
                        rowQty += Convert.ToInt32(txtEffQty.Text);
                    }
                    else if (CheckBoxSelect == "Density")
                    {
                        chkDensity.Checked = true;
                        txtDensityQty.Text = n.material_quantity.ToString();
                        rowQty = Convert.ToInt32(txtDensityQty.Text);
                    }
                }
                totalQty += rowQty;
                //
                i++;
            }
            if (totalQty > 0)
                UC_InwardHeader1.TotalQty = totalQty.ToString();
            UC_InwardHeader1.Subsets = grdBrickInward.Rows.Count.ToString();
            DisplayDensity();
        }
        protected void lnkTemp_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = true;
        }
        protected Boolean ValidateData()
        {
            decimal sumQty = 0;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;

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
                for (int i = 0; i < grdBrickInward.Rows.Count; i++)
                {
                    DropDownList ddlTypeOfBrick = (DropDownList)grdBrickInward.Rows[i].Cells[1].FindControl("ddlTypeOfBrick");
                    TextBox txtIdMark = (TextBox)grdBrickInward.Rows[i].Cells[2].FindControl("txtIdMark");
                    CheckBox chkCS = (CheckBox)grdBrickInward.Rows[i].Cells[3].FindControl("chkCS");
                    TextBox txtCSQty = (TextBox)grdBrickInward.Rows[i].Cells[4].FindControl("txtCSQty");
                    CheckBox chkWA = (CheckBox)grdBrickInward.Rows[i].Cells[5].FindControl("chkWA");
                    TextBox txtWAQty = (TextBox)grdBrickInward.Rows[i].Cells[6].FindControl("txtWAQty");
                    CheckBox chkDA = (CheckBox)grdBrickInward.Rows[i].Cells[7].FindControl("chkDA");
                    TextBox txtDAQty = (TextBox)grdBrickInward.Rows[i].Cells[8].FindControl("txtDAQty");
                    CheckBox chkEff = (CheckBox)grdBrickInward.Rows[i].Cells[9].FindControl("chkEff");
                    TextBox txtEffQty = (TextBox)grdBrickInward.Rows[i].Cells[10].FindControl("txtEffQty");
                    CheckBox chkDensity = (CheckBox)grdBrickInward.Rows[i].Cells[11].FindControl("chkDensity");
                    TextBox txtDensityQty = (TextBox)grdBrickInward.Rows[i].Cells[12].FindControl("txtDensityQty");
                    TextBox txtDescription = (TextBox)grdBrickInward.Rows[i].Cells[13].FindControl("txtDescription");
                    DropDownList ddlSupplier = (DropDownList)grdBrickInward.Rows[i].Cells[14].FindControl("ddlSupplier");

                    if (ddlTypeOfBrick.Text == "Select")
                    {
                        lblMsg.Text = "Select Type of Brick of Sr No. " + (i + 1) + ".";
                        ddlTypeOfBrick.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtIdMark.Text == "")
                    {
                        lblMsg.Text = "Enter Id Mark of Sr No. " + (i + 1) + ".";
                        txtIdMark.Focus();
                        valid = false;
                        break;
                    }
                    else if (chkCS.Checked == false && chkWA.Checked == false && chkDA.Checked == false && chkEff.Checked == false && chkDensity.Checked == false)
                    {
                        lblMsg.Text = "Select at least one Test for Sr No " + (i + 1) + ".";
                        valid = false;
                        break;
                    }
                    else if (chkCS.Visible == true && chkCS.Checked == true && txtCSQty.Text == "")
                    {
                        lblMsg.Text = "Enter CS Quanity of Sr No." + (i + 1) + "."; ;
                        txtCSQty.Focus();
                        valid = false;
                        break;
                    }
                    else if (chkWA.Visible == true && chkWA.Checked == true && txtWAQty.Text == "")
                    {
                        lblMsg.Text = "Enter WA Quanity of Sr No." + (i + 1) + "."; ;
                        txtWAQty.Focus();
                        valid = false;
                        break;
                    }
                    else if (chkDA.Visible == true && chkDA.Checked == true && txtDAQty.Text == "")
                    {
                        lblMsg.Text = "Enter DA Quanity of Sr No." + (i + 1) + "."; ;
                        txtDAQty.Focus();
                        valid = false;
                        break;
                    }
                    else if (chkEff.Visible == true && chkEff.Checked == true && txtEffQty.Text == "")
                    {
                        lblMsg.Text = "Enter Eff Quanity of Sr No." + (i + 1) + "."; ;
                        txtEffQty.Focus();
                        valid = false;
                        break;
                    }
                    else if (chkDensity.Checked == true && txtDensityQty.Text == "")
                    {
                        lblMsg.Text = "Enter Density Quanity of Sr No." + (i + 1) + "."; ;
                        txtDensityQty.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtDescription.Text == "")
                    {
                        lblMsg.Text = "Enter Description of Sr No." + (i + 1) + ".";
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
                    decimal Csqty = 0, WAQty = 0, DAQty = 0, EffQty = 0, DensityyQty = 0;
                    if (txtCSQty.Text != "" && chkCS.Checked)
                    {
                        Csqty += Convert.ToDecimal(txtCSQty.Text);
                    }
                    if (txtWAQty.Text != "" && chkWA.Checked)
                    {
                        WAQty += Convert.ToDecimal(txtWAQty.Text);
                    }
                    if (txtDAQty.Text != "" && chkDA.Checked)
                    {
                        DAQty += Convert.ToDecimal(txtDAQty.Text);
                    }
                    if (txtEffQty.Text != "" && chkEff.Checked)
                    {
                        EffQty += Convert.ToDecimal(txtEffQty.Text);
                    }
                    if (txtDensityQty.Text != "" && chkDensity.Checked)
                    {
                        DensityyQty += Convert.ToDecimal(txtDensityQty.Text);
                    }
                    sumQty += Csqty + WAQty + DAQty + EffQty + DensityyQty;
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
        
        protected void chkCS_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grdBrickInward.Rows.Count; i++)
            {
                CheckBox chkCS = (CheckBox)grdBrickInward.Rows[i].Cells[3].FindControl("chkCS");
                TextBox txtCSQty = (TextBox)grdBrickInward.Rows[i].Cells[4].FindControl("txtCSQty");

                if (chkCS.Checked == false)
                {
                    txtCSQty.Text = "";
                }
                else
                {
                    txtCSQty.Focus();
                }
            }

        }
        protected void chkWA_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grdBrickInward.Rows.Count; i++)
            {
                CheckBox chkWA = (CheckBox)grdBrickInward.Rows[i].Cells[5].FindControl("chkWA");
                TextBox txtWAQty = (TextBox)grdBrickInward.Rows[i].Cells[6].FindControl("txtWAQty");

                if (chkWA.Checked == false)
                {
                    txtWAQty.Text = "";
                }
                else
                {
                    txtWAQty.Focus();
                }
            }


        }
        protected void chkDA_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grdBrickInward.Rows.Count; i++)
            {
                CheckBox chkDA = (CheckBox)grdBrickInward.Rows[i].Cells[7].FindControl("chkDA");
                TextBox txtDAQty = (TextBox)grdBrickInward.Rows[i].Cells[8].FindControl("txtDAQty");
                if (chkDA.Checked == false)
                {
                    txtDAQty.Text = "";
                }
                else
                {
                    txtDAQty.Focus();
                }
            }

        }
        protected void chkEff_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grdBrickInward.Rows.Count; i++)
            {
                CheckBox chkEff = (CheckBox)grdBrickInward.Rows[i].Cells[9].FindControl("chkEff");
                TextBox txtEffQty = (TextBox)grdBrickInward.Rows[i].Cells[10].FindControl("txtEffQty");
                if (chkEff.Checked == false)
                {
                    txtEffQty.Text = "";
                }
                else
                {
                    txtEffQty.Focus();
                }
            }


        }
        protected void chkDensity_CheckedChanged(object sender, EventArgs e)
        {
            DisplayDensity();
        }
        public void DisplayDensity()
        {
            for (int i = 0; i < grdBrickInward.Rows.Count; i++)
            {
                CheckBox chkCS = (CheckBox)grdBrickInward.Rows[i].Cells[3].FindControl("chkCS");
                TextBox txtCSQty = (TextBox)grdBrickInward.Rows[i].Cells[4].FindControl("txtCSQty");
                CheckBox chkWA = (CheckBox)grdBrickInward.Rows[i].Cells[5].FindControl("chkWA");
                TextBox txtWAQty = (TextBox)grdBrickInward.Rows[i].Cells[6].FindControl("txtWAQty");
                CheckBox chkDA = (CheckBox)grdBrickInward.Rows[i].Cells[7].FindControl("chkDA");
                TextBox txtDAQty = (TextBox)grdBrickInward.Rows[i].Cells[8].FindControl("txtDAQty");
                CheckBox chkEff = (CheckBox)grdBrickInward.Rows[i].Cells[9].FindControl("chkEff");
                TextBox txtEffQty = (TextBox)grdBrickInward.Rows[i].Cells[10].FindControl("txtEffQty");
                CheckBox chkDensity = (CheckBox)grdBrickInward.Rows[i].Cells[11].FindControl("chkDensity");
                TextBox txtDensityQty = (TextBox)grdBrickInward.Rows[i].Cells[12].FindControl("txtDensityQty");
                if (chkDensity.Checked)
                {
                    txtDensityQty.Focus();
                    chkCS.Enabled = false;
                    chkCS.Checked = false;
                    txtCSQty.Enabled = false;
                    chkWA.Enabled = false;
                    chkWA.Checked = false;
                    txtWAQty.Enabled = false;
                    chkDA.Enabled = false;
                    chkDA.Checked = false;
                    txtDAQty.Enabled = false;
                    chkEff.Enabled = false;
                    chkEff.Checked = false;
                    txtEffQty.Enabled = false;
                    txtCSQty.Text = "";
                    txtWAQty.Text = "";
                    txtDAQty.Text = "";
                    txtEffQty.Text = "";
                }
                else
                {
                    txtDensityQty.Text = "";
                    chkCS.Enabled = true;
                    //chkCS.Checked = false;
                    txtCSQty.Enabled = true;
                    // chkWA.Checked = false;
                    chkWA.Enabled = true;
                    txtWAQty.Enabled = true;
                    chkDA.Enabled = true;
                    // chkDA.Checked = false;
                    txtDAQty.Enabled = true;
                    chkEff.Enabled = true;
                    // chkEff.Checked = false;
                    txtEffQty.Enabled = true;
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
                reportStr = rpt.getDetailReportBrickInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Brick_Inward", reportStr);
        }
        bool buttonClicked = false;
        protected string getDetailReportBrickInward()
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
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Brick Test Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Brick Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("BT-", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }


            var b = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), null, "BT-", null, null);

            foreach (var nt in b)
            {
                if (buttonClicked == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "BT-" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "BT-" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2> DT - " + nt.INWD_BILL_Id + "</font></td>" +
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
                        "<td height=19><font size=2>" + "BT-" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "BT-" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.SITE_Address_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2> DT - " + nt.INWD_BILL_Id + "</font></td>" +
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
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Brick Type</b></font></td>";

            mySql += "<td>";
            mySql += "<table  width=100% id=AutoNumber2>";
            mySql += "<tr>";
            mySql += "<td width= 30% align=center valign=top height=19 ><font size=2><b> Test to be Conducted </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b> Qty</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";
            mySql += "</td>";


            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("BT-", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.BTINWD_IdMark_var + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.BTINWD_BrickType_var.ToString() + "</font></td>";

                mySql += "<td > ";
                mySql += "<table border=0 cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";

                var wttest = dc.AllInward_View("BT-", 0, c.BTINWD_ReferenceNo_var.ToString());
                foreach (var wt in wttest)
                {
                    var c1 = dc.Test_View(0, Convert.ToInt32(wt.BTTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var n2 in c1)
                    {
                        mySql += "<tr>";
                        mySql += "<td>";//

                        mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
                        mySql += "<tr>";
                        mySql += "<td width= 30% align=left valign=top height=19 ><font size=2>&nbsp;" + n2.TEST_Name_var.ToString() + "</font></td>";

                        var c11 = dc.Test_View(0, Convert.ToInt32(wt.BTTEST_TEST_Id), "", 0, 0, 0);
                        foreach (var d1 in c11)
                        {
                            string TEST_Name_var = d1.TEST_Name_var.ToString();
                            var Rateint = dc.Test_View(0, Convert.ToInt32(wt.BTTEST_TEST_Id), "", 0, 0, 0);
                            foreach (var r in Rateint)
                            {
                                if (wt.BTTEST_Quantity_tint.ToString() != null && wt.BTTEST_Quantity_tint.ToString() != "")
                                {
                                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + wt.BTTEST_Quantity_tint.ToString() + "</font></td>";
                                }
                            }
                        }

                        mySql += "</tr>";
                        mySql += "</table>";
                        mySql += "</td>";
                        mySql += "</tr>";
                    }
                }
                mySql += "</table>";
                mySql += "</td>";


                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.BTINWD_SupplierName_var + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.BTINWD_Description_var + "</font></td>";
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
                reportStr = rpt.getDetailReportBrickInward(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), true);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("Brick_LabSheet", reportStr);
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
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "BT-")));
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
