using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class MixDesign_Inward : System.Web.UI.Page
    {
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        LabDataDataContext dc = new LabDataDataContext();
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
                lblheading.Text = "Mix Design Inward";
                if (UC_InwardHeader1.EnquiryNo != "")
                {
                    //string[] splitBy = dssd.Split(new string[] { ":line" }, StringSplitOptions.RemoveEmptyEntries);
                    AddRowMixDesignInward();
                    AddRowMaterial();
                }
                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    getMixDesignTesting();
                }
                if (UC_InwardHeader1.EnquiryNo == "" && UC_InwardHeader1.RecordNo == "")
                {
                    AddRowMixDesignInward();
                    AddRowMaterial();
                    UC_InwardHeader1.InwdStatus = "Add";
                    UC_InwardHeader1.RecType = "MF";
                }	
                lnkSave.Visible = true;
            }

        }
        protected void grdMixDesignInward_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl_TestName = (DropDownList)e.Row.FindControl("ddl_TestName");
                var test = dc.MixDesignTest_View(0, "");
                ddl_TestName.DataSource = test;
                ddl_TestName.DataTextField = "TEST_Name_var";
                //ddl_TestName.DataValueField = "TEST_Id";
                ddl_TestName.DataBind();
                ddl_TestName.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        protected void AddRowMixDesignInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MixDesignInwardTable"] != null)
            {
                GetCurrentDataMixDesignInward();
                dt = (DataTable)ViewState["MixDesignInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_Grade", typeof(string)));//
                dt.Columns.Add(new DataColumn("txt_Slump", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_TestName", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_NatureofWork", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_AnySpecialRequirement", typeof(string)));

            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["ddl_Grade"] = string.Empty;
            dr["txt_Slump"] = string.Empty;
            dr["ddl_TestName"] = string.Empty;
            dr["txt_NatureofWork"] = string.Empty;
            dr["txt_AnySpecialRequirement"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["MixDesignInwardTable"] = dt;
            grdMixDesignInward.DataSource = dt;
            grdMixDesignInward.DataBind();
            SetPreviousDataMixDesignInward();
        }
        protected void DeleteRowMixDesignInward(int rowIndex)
        {
            GetCurrentDataMixDesignInward();
            DataTable dt = ViewState["MixDesignInwardTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["MixDesignInwardTable"] = dt;
            grdMixDesignInward.DataSource = dt;
            grdMixDesignInward.DataBind();
            SetPreviousDataMixDesignInward();
        }
        protected void SetPreviousDataMixDesignInward()
        {
            DataTable dt = (DataTable)ViewState["MixDesignInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddl_Grade = (DropDownList)grdMixDesignInward.Rows[i].Cells[1].FindControl("ddl_Grade");
                TextBox txt_Slump = (TextBox)grdMixDesignInward.Rows[i].Cells[2].FindControl("txt_Slump");
                DropDownList ddl_TestName = (DropDownList)grdMixDesignInward.Rows[i].Cells[3].FindControl("ddl_TestName");
                TextBox txt_NatureofWork = (TextBox)grdMixDesignInward.Rows[i].Cells[4].FindControl("txt_NatureofWork");
                TextBox txt_AnySpecialRequirement = (TextBox)grdMixDesignInward.Rows[i].Cells[5].FindControl("txt_AnySpecialRequirement");

                grdMixDesignInward.Rows[i].Cells[0].Text = (i + 1).ToString();
                ddl_Grade.Text = dt.Rows[i]["ddl_Grade"].ToString();
                txt_Slump.Text = dt.Rows[i]["txt_Slump"].ToString();
                ddl_TestName.Text = dt.Rows[i]["ddl_TestName"].ToString();
                txt_NatureofWork.Text = dt.Rows[i]["txt_NatureofWork"].ToString();
                txt_AnySpecialRequirement.Text = dt.Rows[i]["txt_AnySpecialRequirement"].ToString();

            }
        }
        protected void GetCurrentDataMixDesignInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_Grade", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Slump", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_TestName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_NatureofWork", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_AnySpecialRequirement", typeof(string)));
            for (int i = 0; i < grdMixDesignInward.Rows.Count; i++)
            {
                DropDownList ddl_Grade = (DropDownList)grdMixDesignInward.Rows[i].Cells[1].FindControl("ddl_Grade");
                TextBox txt_Slump = (TextBox)grdMixDesignInward.Rows[i].Cells[2].FindControl("txt_Slump");
                DropDownList ddl_TestName = (DropDownList)grdMixDesignInward.Rows[i].Cells[3].FindControl("ddl_TestName");
                TextBox txt_NatureofWork = (TextBox)grdMixDesignInward.Rows[i].Cells[4].FindControl("txt_NatureofWork");
                TextBox txt_AnySpecialRequirement = (TextBox)grdMixDesignInward.Rows[i].Cells[5].FindControl("txt_AnySpecialRequirement");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["ddl_Grade"] = ddl_Grade.Text;
                drRow["txt_Slump"] = txt_Slump.Text;
                drRow["ddl_TestName"] = ddl_TestName.Text;
                drRow["txt_NatureofWork"] = txt_NatureofWork.Text;
                drRow["txt_AnySpecialRequirement"] = txt_AnySpecialRequirement.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["MixDesignInwardTable"] = dtTable;

        }
        
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                //get report client id, site
                var enqcl = dc.EnquiryClient_View(Convert.ToInt32(UC_InwardHeader1.EnquiryNo));
                foreach (var ec in enqcl)
                {
                    lblRptClientId.Text = ec.ENQCL_CL_Id.ToString();
                    lblRptSiteId.Text = ec.ENQCL_SITE_Id.ToString();
                }
                //
                bool updateFlag = true;
                string RefNo, SetOfRecord,totalCost="";
                clsData clsObj = new clsData();
                int SrNo;
                //DateTime d1 = new DateTime();
                string[] strDate = new string[3];
                //DateTime ReceivedDate = Convert.ToDateTime(DateTime.Now.Date.ToString());
                DateTime ReceivedDate = DateTime.ParseExact(UC_InwardHeader1.ReceivedDate, "dd/MM/yyyy HH:mm:ss", null);
                DateTime d1 = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);

                if (UC_InwardHeader1.InwdStatus == "Edit")
                {
                    dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "MF", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, true);
                }
                else
                {
                    try
                    {
                        Int32 NewrecNo = 0;
                        NewrecNo = clsObj.GetnUpdateRecordNo("MF");
                        UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                        clsObj.insertRecordTable(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), NewrecNo, "MF");
                        UC_InwardHeader1.ReferenceNo = clsObj.getRefNo(Convert.ToInt32(UC_InwardHeader1.EnquiryNo)).ToString();
                        dc.Inward_Update_New(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "MF", 0, Convert.ToInt32(UC_InwardHeader1.ReferenceNo), Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate);
                    }
                    catch
                    {
                        updateFlag = false;
                    }
                    //Int32 NewrecNo = 0;
                    //clsData clsObj = new clsData();
                    //NewrecNo = clsObj.GetnUpdateRecordNo("MF");
                    //UC_InwardHeader1.RecordNo = NewrecNo.ToString();
                    //UC_InwardHeader1.ReferenceNo = dc.Inward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), "MF", 0, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate, false).ToString();
                   

                    ////clsData clsd = new clsData();
                    ////UC_InwardHeader1.ReferenceNo = clsd.Inward_Insert(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 0, "MF", Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToInt32(UC_InwardHeader1.ContactPersonId), UC_InwardHeader1.Building, UC_InwardHeader1.Charges, UC_InwardHeader1.WorkOrder, 0, Convert.ToInt32(Session["LoginId"]), d1, TimeSpan.Parse(Convert.ToDateTime(UC_InwardHeader1.CollectionTime).ToString("HH:mm:ss")), Convert.ToInt32(UC_InwardHeader1.TotalQty), Convert.ToInt32(UC_InwardHeader1.Subsets), UC_InwardHeader1.ContactNo, UC_InwardHeader1.EmailId.ToString(), ReceivedDate).ToString();
                    ////var inward = dc.Inward_View(Convert.ToInt32(UC_InwardHeader1.ReferenceNo), 0, "");
                    ////foreach (var inwd in inward)
                    ////{
                    ////    UC_InwardHeader1.RecordNo = inwd.INWD_RecordNo_int.ToString();
                    ////}
                    ////if (UC_InwardHeader1.ReferenceNo == "0")
                    ////    updateFlag = false;
                }
                if (updateFlag == true)
                {
                    dc.MixDesignInward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), "MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), "", "", 0, 0, 0, "", "", "", "", "", 0, false, null, null, "", "", 0, 0,false , true);
                    bool chkTestingMat = false;
                    if (chk_MaterialName.Checked)
                    {
                        chkTestingMat = true;
                    }
                    dc.Enquiry_Update_Status(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), 2);
                    dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "MF", "", null, null, false, false, false, false, false, false, true);//Delete MIS Record
                    dc.MaterialDetail_Update(0, "", "", 0, 0, Convert.ToInt32(UC_InwardHeader1.RecordNo), null, true);
                    if (lblRptClientId.Text != "" && lblRptClientId.Text != "0")
                    {
                        dc.Inward_Update_ReportClient(Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, Convert.ToInt32(lblRptClientId.Text), Convert.ToInt32(lblRptSiteId.Text));
                    }
                    
                    for (int i = 0; i <= grdMixDesignInward.Rows.Count - 1; i++)
                    {
                        DropDownList ddl_Grade = (DropDownList)grdMixDesignInward.Rows[i].Cells[1].FindControl("ddl_Grade");
                        TextBox txt_Slump = (TextBox)grdMixDesignInward.Rows[i].Cells[2].FindControl("txt_Slump");
                        DropDownList ddl_TestName = (DropDownList)grdMixDesignInward.Rows[i].Cells[3].FindControl("ddl_TestName");
                        TextBox txt_NatureofWork = (TextBox)grdMixDesignInward.Rows[i].Cells[4].FindControl("txt_NatureofWork");
                        TextBox txt_AnySpecialRequirement = (TextBox)grdMixDesignInward.Rows[i].Cells[5].FindControl("txt_AnySpecialRequirement");

                        int TestId = 0;
                        var Test = dc.MixDesignTest_View(Convert.ToDecimal(ddl_Grade.SelectedItem.Text.Replace("M", "")), ddl_TestName.SelectedItem.Text);
                        foreach (var m in Test)
                        {
                            TestId = Convert.ToInt32(m.TEST_Id);
                        }
                        SrNo = Convert.ToInt32(grdMixDesignInward.Rows[i].Cells[0].Text);
                        RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                        SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;

                        dc.MixDesignInward_Update(Convert.ToInt32(UC_InwardHeader1.EnquiryNo), "MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, 0, 0, ddl_Grade.SelectedItem.Text, txt_Slump.Text, txt_NatureofWork.Text, txt_AnySpecialRequirement.Text, "", TestId, chkTestingMat, d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), UC_InwardHeader1.ContactNo, Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false);
                        //dc.MaterialDetail_Update(0, RefNo, "", 0, 0, 0, null, true);

                        dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "MF", RefNo, "MDL", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);

                        dc.MISDetail_Update(Convert.ToInt32(UC_InwardHeader1.RecordNo), "MF", RefNo, "Final", Convert.ToDateTime(UC_InwardHeader1.CollectionDate + " " + UC_InwardHeader1.CollectionTime, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat), false, false, false, false, false, false, false);
                    }
                    dc.MixDesignInward_Update_Description(UC_InwardHeader1.ReferenceNo + "/%", Convert.ToInt32(UC_InwardHeader1.RecordNo), UC_InwardHeader1.RecType, txtDescription.Text.Trim());
                    SrNo = 0; RefNo = ""; SetOfRecord = ""; int MatSrNo = 0;
                    for (int i = 0; i < Grd_Material.Rows.Count; i++)
                    {
                        DropDownList ddl_Material = (DropDownList)Grd_Material.Rows[i].Cells[3].FindControl("ddl_Material");
                        TextBox txt_Qty = (TextBox)Grd_Material.Rows[i].Cells[4].FindControl("txt_Qty");
                        TextBox txt_Information = (TextBox)Grd_Material.Rows[i].Cells[5].FindControl("txt_Information");
                        CheckBox chk_SrNo = (CheckBox)Grd_Material.Rows[i].Cells[6].FindControl("chk_SrNo");
                        CheckBox chk_SrNo1 = (CheckBox)Grd_Material.Rows[i].Cells[7].FindControl("chk_SrNo1");
                        CheckBox chk_SrNo2 = (CheckBox)Grd_Material.Rows[i].Cells[8].FindControl("chk_SrNo2");
                        CheckBox chk_SrNo3 = (CheckBox)Grd_Material.Rows[i].Cells[9].FindControl("chk_SrNo3");
                        CheckBox chk_SrNo4 = (CheckBox)Grd_Material.Rows[i].Cells[10].FindControl("chk_SrNo4");
                        CheckBox chk_SrNo5 = (CheckBox)Grd_Material.Rows[i].Cells[11].FindControl("chk_SrNo5");
                        CheckBox chk_SrNo6 = (CheckBox)Grd_Material.Rows[i].Cells[12].FindControl("chk_SrNo6");
                        CheckBox chk_SrNo7 = (CheckBox)Grd_Material.Rows[i].Cells[13].FindControl("chk_SrNo7");
                        CheckBox chk_SrNo8 = (CheckBox)Grd_Material.Rows[i].Cells[14].FindControl("chk_SrNo8");
                        CheckBox chk_SrNo9 = (CheckBox)Grd_Material.Rows[i].Cells[15].FindControl("chk_SrNo9");

                        MatSrNo = Convert.ToInt32(Grd_Material.Rows[i].Cells[2].Text);
                        //
                        dc.AggregateInward_Update("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), "", "", 0, 0, "", "", 0, "", "", 0, Convert.ToInt32(ddl_Material.SelectedValue), null, null, "", "",0,0,false , true);

                        for (int j = 0; j <= grdMixDesignInward.Rows.Count - 1; j++)
                        {
                            SrNo = Convert.ToInt32(grdMixDesignInward.Rows[j].Cells[0].Text);
                            RefNo = UC_InwardHeader1.ReferenceNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;
                            SetOfRecord = UC_InwardHeader1.RecordNo + "/" + UC_InwardHeader1.Subsets + "-" + SrNo;

                            if (chk_SrNo.Checked && j == 0)
                            {
                                dc.MaterialDetail_Update(Convert.ToInt32(ddl_Material.SelectedValue), RefNo, txt_Information.Text, Convert.ToDecimal(txt_Qty.Text), MatSrNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), ReceivedDate, false);
                                dc.AggregateInward_Update("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txt_Qty.Text), "", "", 0, "", ddl_Material.SelectedItem.Text, 0, Convert.ToInt32(ddl_Material.SelectedValue), d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), "", Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false);
                            }
                            if (chk_SrNo1.Checked && j == 1)
                            {
                                dc.MaterialDetail_Update(Convert.ToInt32(ddl_Material.SelectedValue), RefNo, txt_Information.Text, Convert.ToDecimal(txt_Qty.Text), MatSrNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), ReceivedDate, false);
                                dc.AggregateInward_Update("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txt_Qty.Text), "", "", 0, "", ddl_Material.SelectedItem.Text, 0, Convert.ToInt32(ddl_Material.SelectedValue), d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), "", Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false);
                            }
                            if (chk_SrNo2.Checked && j == 2)
                            {
                                dc.MaterialDetail_Update(Convert.ToInt32(ddl_Material.SelectedValue), RefNo, txt_Information.Text, Convert.ToDecimal(txt_Qty.Text), MatSrNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), ReceivedDate, false);
                                dc.AggregateInward_Update("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txt_Qty.Text), "", "", 0, "", ddl_Material.SelectedItem.Text, 0, Convert.ToInt32(ddl_Material.SelectedValue), d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), "", Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false);
                            }
                            if (chk_SrNo3.Checked && j == 3)
                            {
                                dc.MaterialDetail_Update(Convert.ToInt32(ddl_Material.SelectedValue), RefNo, txt_Information.Text, Convert.ToDecimal(txt_Qty.Text), MatSrNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), ReceivedDate, false);
                                dc.AggregateInward_Update("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txt_Qty.Text), "", "", 0, "", ddl_Material.SelectedItem.Text, 0, Convert.ToInt32(ddl_Material.SelectedValue), d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), "", Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false);
                            }
                            if (chk_SrNo4.Checked && j == 4)
                            {
                                dc.MaterialDetail_Update(Convert.ToInt32(ddl_Material.SelectedValue), RefNo, txt_Information.Text, Convert.ToDecimal(txt_Qty.Text), MatSrNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), ReceivedDate, false);
                                dc.AggregateInward_Update("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txt_Qty.Text), "", "", 0, "", ddl_Material.SelectedItem.Text, 0, Convert.ToInt32(ddl_Material.SelectedValue), d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), "", Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false);
                            }
                            if (chk_SrNo5.Checked && j == 5)
                            {
                                dc.MaterialDetail_Update(Convert.ToInt32(ddl_Material.SelectedValue), RefNo, txt_Information.Text, Convert.ToDecimal(txt_Qty.Text), MatSrNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), ReceivedDate, false);
                                dc.AggregateInward_Update("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txt_Qty.Text), "", "", 0, "", ddl_Material.SelectedItem.Text, 0, Convert.ToInt32(ddl_Material.SelectedValue), d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), "", Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false);
                            }
                            if (chk_SrNo6.Checked && j == 6)
                            {
                                dc.MaterialDetail_Update(Convert.ToInt32(ddl_Material.SelectedValue), RefNo, txt_Information.Text, Convert.ToDecimal(txt_Qty.Text), MatSrNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), ReceivedDate, false);
                                dc.AggregateInward_Update("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txt_Qty.Text), "", "", 0, "", ddl_Material.SelectedItem.Text, 0, Convert.ToInt32(ddl_Material.SelectedValue), d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), "", Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false);
                            }
                            if (chk_SrNo7.Checked && j == 7)
                            {
                                dc.MaterialDetail_Update(Convert.ToInt32(ddl_Material.SelectedValue), RefNo, txt_Information.Text, Convert.ToDecimal(txt_Qty.Text), MatSrNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), ReceivedDate, false);
                                dc.AggregateInward_Update("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txt_Qty.Text), "", "", 0, "", ddl_Material.SelectedItem.Text, 0, Convert.ToInt32(ddl_Material.SelectedValue), d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), "", Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false);
                            }
                            if (chk_SrNo8.Checked && j == 8)
                            {
                                dc.MaterialDetail_Update(Convert.ToInt32(ddl_Material.SelectedValue), RefNo, txt_Information.Text, Convert.ToDecimal(txt_Qty.Text), MatSrNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), ReceivedDate, false);
                                dc.AggregateInward_Update("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txt_Qty.Text), "", "", 0, "", ddl_Material.SelectedItem.Text, 0, Convert.ToInt32(ddl_Material.SelectedValue), d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), "", Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false);
                            }
                            if (chk_SrNo9.Checked && j == 9)
                            {
                                dc.MaterialDetail_Update(Convert.ToInt32(ddl_Material.SelectedValue), RefNo, txt_Information.Text, Convert.ToDecimal(txt_Qty.Text), MatSrNo, Convert.ToInt32(UC_InwardHeader1.RecordNo), ReceivedDate, false);
                                dc.AggregateInward_Update("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), RefNo, SetOfRecord, 0, Convert.ToByte(txt_Qty.Text), "", "", 0, "", ddl_Material.SelectedItem.Text, 0, Convert.ToInt32(ddl_Material.SelectedValue), d1, ReceivedDate, UC_InwardHeader1.EmailId.ToString(), "", Convert.ToInt32(UC_InwardHeader1.ClientId), Convert.ToInt32(UC_InwardHeader1.SiteId), Convert.ToBoolean(Convert.ToInt32(ddlTestedAt.SelectedValue)), false);
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

                    #region proforma invoice
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
                    //        ProformaInvoiceNo = ProInv.UpdateProformaInvoice("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), ProformaInvoiceNo);
                    //totalCost = clsObj.getProformaBillNetAmount(ProformaInvoiceNo,1);
                    //    }
                    //    UC_InwardHeader1.ProformaInvoiceNo = ProformaInvoiceNo.ToString();
                    //    //
                    //    lnkBillPrint.Visible = true;
                    //}
                    #endregion

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
                            BillNo = bill.UpdateBill("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), BillNo);
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

                    //UC_InwardHeader1.RecType = null;
                    UC_InwardHeader1.EnquiryNo = "";
                    DropDownList ddlEnquiryList = (DropDownList)UC_InwardHeader1.FindControl("ddlEnquiryList");
                    ddlEnquiryList.ClearSelection();
                    LoadEnquiryList(Convert.ToInt32(UC_InwardHeader1.MaterialId));

                }
                else
                {
                    //Label lblMsg = (Label)Master.FindControl("lblMsg");
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

        protected Boolean ValidateData()
        {
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
            else if (txtDescription.Text.Trim() == "")
            {
                lblMsg.Text = "Enter Description";
                txtDescription.Focus();
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
                for (int i = 0; i < grdMixDesignInward.Rows.Count; i++)
                {
                    DropDownList ddl_Grade = (DropDownList)grdMixDesignInward.Rows[i].Cells[1].FindControl("ddl_Grade");
                    TextBox txt_Slump = (TextBox)grdMixDesignInward.Rows[i].Cells[2].FindControl("txt_Slump");
                    DropDownList ddl_TestName = (DropDownList)grdMixDesignInward.Rows[i].Cells[3].FindControl("ddl_TestName");
                    TextBox txt_NatureofWork = (TextBox)grdMixDesignInward.Rows[i].Cells[4].FindControl("txt_NatureofWork");
                    TextBox txt_AnySpecialRequirement = (TextBox)grdMixDesignInward.Rows[i].Cells[5].FindControl("txt_AnySpecialRequirement");

                    if (ddl_Grade.SelectedItem.Text == "Select")
                    {
                        lblMsg.Text = "Select Grade for Sr No. " + (i + 1) + ".";
                        valid = false;
                        break;
                    }
                    else if (txt_Slump.Text == "")
                    {
                        lblMsg.Text = "Enter Slump for Sr No. " + (i + 1) + ".";
                        txt_Slump.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddl_TestName.SelectedItem.Text == "Select")
                    {
                        lblMsg.Text = "Select Type of Design for Sr No. " + (i + 1) + ".";
                        valid = false;
                        break;
                    }
                    else if (txt_NatureofWork.Text == "")
                    {
                        lblMsg.Text = "Enter Nature of Work for Sr No. " + (i + 1) + ".";
                        txt_NatureofWork.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_AnySpecialRequirement.Text == "")
                    {
                        lblMsg.Text = "Enter Any Special Requirement for Sr No. " + (i + 1) + ".";
                        txt_AnySpecialRequirement.Focus();
                        valid = false;
                        break;
                    }
                }
                if (valid == true)
                {
                    for (int i = 0; i < Grd_Material.Rows.Count; i++)
                    {
                        DropDownList ddl_Material = (DropDownList)Grd_Material.Rows[i].Cells[3].FindControl("ddl_Material");
                        TextBox txt_Qty = (TextBox)Grd_Material.Rows[i].Cells[4].FindControl("txt_Qty");
                        TextBox txt_Information = (TextBox)Grd_Material.Rows[i].Cells[5].FindControl("txt_Information");

                        if (ddl_Material.SelectedItem.Text == "Select")
                        {
                            lblMsg.Text = "Select Material List for Sr No. " + (i + 1) + ".";
                            ddl_Material.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_Qty.Text == "")
                        {
                            lblMsg.Text = "Enter Quantity for Sr No. " + (i + 1) + ".";
                            txt_Qty.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_Information.Text == "")
                        {
                            lblMsg.Text = "Enter Information for Sr No. " + (i + 1) + ".";
                            txt_Information.Focus();
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
        protected void TextChanged(object sender, EventArgs e)
        {
            TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
            if (txtSubsets.Text != "")
            {
                if (Convert.ToInt32(txtSubsets.Text) < grdMixDesignInward.Rows.Count)
                {
                    for (int i = grdMixDesignInward.Rows.Count; i > Convert.ToInt32(txtSubsets.Text); i--)
                    {
                        if (grdMixDesignInward.Rows.Count > 1)
                        {
                            DeleteRowMixDesignInward(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSubsets.Text) > grdMixDesignInward.Rows.Count)
                {
                    for (int i = grdMixDesignInward.Rows.Count + 1; i <= Convert.ToInt32(txtSubsets.Text); i++)
                    {
                        AddRowMixDesignInward();
                    }
                }
                DisplayMaterial();
            }
        }
        protected void Click(object sender, EventArgs e)
        {
            ddlTestedAt.SelectedIndex = 0;
            ViewState["MixDesignInwardTable"] = null;
            ViewState["MaterialTable"] = null;
            if (UC_InwardHeader1.EnquiryNo != "" && UC_InwardHeader1.EnqNoApp != "" && UC_InwardHeader1.EnqNoApp != "0")
            {
                gridAppData();
            }
            else
            {
                AddRowMixDesignInward();
                AddRowMaterial();
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
            txtDescription.Text = "";
            lnkTemp_Click(sender, e);
        }

        public void gridAppData()
        {
            int i = 0, totalQty = 0;
            string strMaterial = "";
            var res = dc.TestRequestDetails_View_Inward(Convert.ToInt32(UC_InwardHeader1.EnqNoApp), Convert.ToInt32(UC_InwardHeader1.MaterialId), 0, "MF").ToList();
            foreach (var a in res)
            {
                AddRowMixDesignInward();
                DropDownList ddl_Grade = (DropDownList)grdMixDesignInward.Rows[i].Cells[1].FindControl("ddl_Grade");
                TextBox txt_Slump = (TextBox)grdMixDesignInward.Rows[i].Cells[2].FindControl("txt_Slump");
                DropDownList ddl_TestName = (DropDownList)grdMixDesignInward.Rows[i].Cells[3].FindControl("ddl_TestName");
                TextBox txt_NatureofWork = (TextBox)grdMixDesignInward.Rows[i].Cells[4].FindControl("txt_NatureofWork");
                TextBox txt_AnySpecialRequirement = (TextBox)grdMixDesignInward.Rows[i].Cells[5].FindControl("txt_AnySpecialRequirement");

                //ddl_Grade.SelectedItem.Text = a.grade.Replace("M", "M ");
                ddl_Grade.Items.FindByText(a.grade.Replace("M", "M ")).Selected = true;
                txt_Slump.Text = a.slump;
                txt_NatureofWork.Text = a.Nature_of_work;
                txt_AnySpecialRequirement.Text = a.material_combination;
                //if (Convert.ToString(a.MFINWD_TestMaterial_bit) == "True")
                //{
                //    chk_MaterialName.Checked = true;
                //}
                //else
                //{
                //    chk_MaterialName.Checked = false;
                //}
                //var testname = dc.Test_View(0, Convert.ToInt32(a.MFINWD_TestId_int), "", 0, 0, 0);
                //foreach (var mt in testname)
                //{
                //    ddl_TestName.ClearSelection();
                //    ddl_TestName.Items.FindByText(mt.TEST_Name_var).Selected = true;
                //}
                if (a.material_type == "Conventional")
                    ddl_TestName.Items.FindByText("Concrete Mix Design").Selected = true;
                else if (a.material_type == "Pumpable")
                    ddl_TestName.Items.FindByText("Pumpable Concrete Mix Design").Selected = true;
                else if (a.material_type == "Self Compacting")
                    ddl_TestName.Items.FindByText("Self Compacting Concrete").Selected = true;
                //else if (a.material_type == "Others")                    
                i++;
                int rowNo = 0;
                var mat = dc.mix_design_material_View(Convert.ToInt32(a.TestReqId));
                foreach (var m in mat)
                {
                    bool matFound = false;
                    if (strMaterial.Contains("," + m.materialName + ",") == false)
                    {
                        AddRowMaterial();
                        rowNo = Grd_Material.Rows.Count - 1;
                        strMaterial += "," + m.materialName + ",";
                    }
                    else
                    {
                        for (int x = 0; x < Grd_Material.Rows.Count ; x++)
                        {
                            DropDownList ddl_MaterialTemp = (DropDownList)Grd_Material.Rows[x].FindControl("ddl_Material");
                            if (m.materialName == ddl_MaterialTemp.SelectedItem.Text)
                            {
                                rowNo = x;
                                break;
                            }
                        }
                        matFound = true;
                    }
                    DropDownList ddl_Material = (DropDownList)Grd_Material.Rows[rowNo].FindControl("ddl_Material");
                    TextBox txt_Qty = (TextBox)Grd_Material.Rows[rowNo].FindControl("txt_Qty");
                    TextBox txt_Information = (TextBox)Grd_Material.Rows[rowNo].FindControl("txt_Information");
                    CheckBox chk_SrNo = (CheckBox)Grd_Material.Rows[rowNo].FindControl("chk_SrNo");
                    CheckBox chk_SrNo1 = (CheckBox)Grd_Material.Rows[rowNo].FindControl("chk_SrNo1");
                    CheckBox chk_SrNo2 = (CheckBox)Grd_Material.Rows[rowNo].FindControl("chk_SrNo2");
                    CheckBox chk_SrNo3 = (CheckBox)Grd_Material.Rows[rowNo].FindControl("chk_SrNo3");
                    CheckBox chk_SrNo4 = (CheckBox)Grd_Material.Rows[rowNo].FindControl("chk_SrNo4");
                    CheckBox chk_SrNo5 = (CheckBox)Grd_Material.Rows[rowNo].FindControl("chk_SrNo5");
                    CheckBox chk_SrNo6 = (CheckBox)Grd_Material.Rows[rowNo].FindControl("chk_SrNo6");
                    CheckBox chk_SrNo7 = (CheckBox)Grd_Material.Rows[rowNo].FindControl("chk_SrNo7");
                    CheckBox chk_SrNo8 = (CheckBox)Grd_Material.Rows[rowNo].FindControl("chk_SrNo8");
                    CheckBox chk_SrNo9 = (CheckBox)Grd_Material.Rows[rowNo].FindControl("chk_SrNo9");

                    if (matFound == false)
                    {
                        chk_SrNo.Checked = false;
                        chk_SrNo1.Checked = false;
                        chk_SrNo2.Checked = false;
                        chk_SrNo3.Checked = false;
                        chk_SrNo4.Checked = false;
                        chk_SrNo5.Checked = false;
                        chk_SrNo6.Checked = false;
                        chk_SrNo7.Checked = false;
                        chk_SrNo8.Checked = false;
                        chk_SrNo9.Checked = false;
                    }
                    ddl_Material.ClearSelection();
                    ddl_Material.Items.FindByText(m.materialName).Selected = true;
                    txt_Qty.Text = m.Quantity.ToString();
                    txt_Information.Text = m.description;
                    if (matFound == false)
                        totalQty += Convert.ToInt32(m.Quantity);
                     
                    switch (i)
                    {
                        case 1:
                            chk_SrNo.Checked = true;
                            break;
                        case 2:
                            chk_SrNo1.Checked = true;
                            break;
                        case 3:
                            chk_SrNo2.Checked = true;
                            break;
                        case 4:
                            chk_SrNo3.Checked = true;
                            break;
                        case 5:
                            chk_SrNo4.Checked = true;
                            break;
                        case 6:
                            chk_SrNo5.Checked = true;
                            break;
                        case 7:
                            chk_SrNo6.Checked = true;
                            break;
                        case 8:
                            chk_SrNo7.Checked = true;
                            break;
                        case 9:
                            chk_SrNo8.Checked = true;
                            break;
                        case 10:
                            chk_SrNo9.Checked = true;
                            break;
                    }
                    Grd_Material.Columns[i + 5].Visible = true;
                }                
            }
            if (totalQty > 0)
                UC_InwardHeader1.TotalQty = totalQty.ToString();
            UC_InwardHeader1.Subsets = grdMixDesignInward.Rows.Count.ToString();
        }

        protected void lnkTemp_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = true;
        }
        public void getMixDesignTesting()
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
            int j = 0;

            lblModifyInwd.Text = "ModifyInward";
            var res = dc.AllInward_View("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            foreach (var a in res)
            {
                AddRowMixDesignInward();
                DropDownList ddl_Grade = (DropDownList)grdMixDesignInward.Rows[i].Cells[1].FindControl("ddl_Grade");
                TextBox txt_Slump = (TextBox)grdMixDesignInward.Rows[i].Cells[2].FindControl("txt_Slump");
                DropDownList ddl_TestName = (DropDownList)grdMixDesignInward.Rows[i].Cells[3].FindControl("ddl_TestName");
                TextBox txt_NatureofWork = (TextBox)grdMixDesignInward.Rows[i].Cells[4].FindControl("txt_NatureofWork");
                TextBox txt_AnySpecialRequirement = (TextBox)grdMixDesignInward.Rows[i].Cells[5].FindControl("txt_AnySpecialRequirement");
                ddl_Grade.SelectedItem.Text = Convert.ToString(a.MFINWD_Grade_var);
                txt_Slump.Text = Convert.ToString(a.MFINWD_Slump_var);
                txt_NatureofWork.Text = a.MFINWD_NatureofWork_var.ToString();
                txt_AnySpecialRequirement.Text = a.MFINWD_SpecialRequirement_var.ToString();
                if (Convert.ToString(a.MFINWD_TestMaterial_bit) == "True")
                {
                    chk_MaterialName.Checked = true;
                }
                else
                {
                    chk_MaterialName.Checked = false;
                }
                var testname = dc.Test_View(0, Convert.ToInt32(a.MFINWD_TestId_int), "", 0, 0, 0);
                if (Convert.ToInt32(a.MFINWD_TestId_int)>0)
                { 
                    foreach (var mt in testname)
                    {
                        ddl_TestName.ClearSelection();
                        ddl_TestName.Items.FindByText(mt.TEST_Name_var).Selected = true;
                    }

                }
                i++;
                ddlTestedAt.SelectedValue = Convert.ToInt32(a.MFINWD_TestedAt_bit).ToString();
                txtDescription.Text = a.MFINWD_Description_var;
            }
            j = 0;
            int SrNo = 0;
            var mat = dc.MaterialDetail_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), "", 0, "", null, null, "");
            foreach (var m in mat)
            {
                int refNos = Convert.ToInt32(m.MaterialDetail_RefNo.Substring(m.MaterialDetail_RefNo.LastIndexOf('-') + 1));
                int Colmun = 5;
                if (SrNo != Convert.ToInt32(m.MaterialDetail_SrNo) || SrNo == 0)
                {
                    if (SrNo > 0)
                    {
                        j++;
                    }
                    AddRowMaterial();
                    DropDownList ddl_Material = (DropDownList)Grd_Material.Rows[j].Cells[3].FindControl("ddl_Material");
                    TextBox txt_Qty = (TextBox)Grd_Material.Rows[j].Cells[4].FindControl("txt_Qty");
                    TextBox txt_Information = (TextBox)Grd_Material.Rows[j].Cells[5].FindControl("txt_Information");
                    CheckBox chk_SrNo = (CheckBox)Grd_Material.Rows[j].Cells[6].FindControl("chk_SrNo");
                    CheckBox chk_SrNo1 = (CheckBox)Grd_Material.Rows[j].Cells[7].FindControl("chk_SrNo1");
                    CheckBox chk_SrNo2 = (CheckBox)Grd_Material.Rows[j].Cells[8].FindControl("chk_SrNo2");
                    CheckBox chk_SrNo3 = (CheckBox)Grd_Material.Rows[j].Cells[9].FindControl("chk_SrNo3");
                    CheckBox chk_SrNo4 = (CheckBox)Grd_Material.Rows[j].Cells[10].FindControl("chk_SrNo4");
                    CheckBox chk_SrNo5 = (CheckBox)Grd_Material.Rows[j].Cells[11].FindControl("chk_SrNo5");
                    CheckBox chk_SrNo6 = (CheckBox)Grd_Material.Rows[j].Cells[12].FindControl("chk_SrNo6");
                    CheckBox chk_SrNo7 = (CheckBox)Grd_Material.Rows[j].Cells[13].FindControl("chk_SrNo7");
                    CheckBox chk_SrNo8 = (CheckBox)Grd_Material.Rows[j].Cells[14].FindControl("chk_SrNo8");
                    CheckBox chk_SrNo9 = (CheckBox)Grd_Material.Rows[j].Cells[15].FindControl("chk_SrNo9");

                    ddl_Material.ClearSelection();
                    ddl_Material.Items.FindByText(m.Material_List).Selected = true;
                    txt_Qty.Text = Convert.ToString(m.MaterialDetail_Quantity);
                    txt_Information.Text = Convert.ToString(m.MaterialDetail_Information);

                    DisplayCheckBoxes(refNos, Colmun, j);

                }
                else
                {
                    CheckBox chk_SrNo = (CheckBox)Grd_Material.Rows[j].Cells[6].FindControl("chk_SrNo");
                    CheckBox chk_SrNo1 = (CheckBox)Grd_Material.Rows[j].Cells[7].FindControl("chk_SrNo1");
                    CheckBox chk_SrNo2 = (CheckBox)Grd_Material.Rows[j].Cells[8].FindControl("chk_SrNo2");
                    CheckBox chk_SrNo3 = (CheckBox)Grd_Material.Rows[j].Cells[9].FindControl("chk_SrNo3");
                    CheckBox chk_SrNo4 = (CheckBox)Grd_Material.Rows[j].Cells[10].FindControl("chk_SrNo4");
                    CheckBox chk_SrNo5 = (CheckBox)Grd_Material.Rows[j].Cells[11].FindControl("chk_SrNo5");
                    CheckBox chk_SrNo6 = (CheckBox)Grd_Material.Rows[j].Cells[12].FindControl("chk_SrNo6");
                    CheckBox chk_SrNo7 = (CheckBox)Grd_Material.Rows[j].Cells[13].FindControl("chk_SrNo7");
                    CheckBox chk_SrNo8 = (CheckBox)Grd_Material.Rows[j].Cells[14].FindControl("chk_SrNo8");
                    CheckBox chk_SrNo9 = (CheckBox)Grd_Material.Rows[j].Cells[15].FindControl("chk_SrNo9");

                    DisplayCheckBoxes(refNos, Colmun, j);

                }
                SrNo = Convert.ToInt32(m.MaterialDetail_SrNo);
            }
            //End
            // DisplayCheck();
            int count = grdMixDesignInward.Rows.Count;
            UC_InwardHeader1.Subsets = count.ToString();
            UC_InwardHeader1.EnquiryNo = "";
            lblModifyInwd.Text = "";
        }
        protected void DisplayCheckBoxes(int refNos, int Colmun, int j)
        {
            switch (refNos)
            {
                case 1:
                    Colmun = Colmun + refNos;
                    CheckBox chk_SrNos = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo");
                    chk_SrNos.Checked = true;
                    Grd_Material.Columns[Colmun].Visible = true;
                    break;
                case 2:
                    Colmun = Colmun + refNos;
                    CheckBox chk_SrNo1s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo1");
                    chk_SrNo1s.Checked = true;
                    Grd_Material.Columns[Colmun].Visible = true;
                    break;

                case 3:
                    Colmun = Colmun + refNos;
                    CheckBox chk_SrNo2s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo2");
                    chk_SrNo2s.Checked = true;
                    Grd_Material.Columns[Colmun].Visible = true;
                    break;
                case 4:
                    Colmun = Colmun + refNos;
                    CheckBox chk_SrNo3s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo3");
                    chk_SrNo3s.Checked = true;
                    Grd_Material.Columns[Colmun].Visible = true;
                    break;
                case 5:
                    Colmun = Colmun + refNos;
                    CheckBox chk_SrNo4s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo4");
                    chk_SrNo4s.Checked = true;
                    Grd_Material.Columns[Colmun].Visible = true;
                    break;
                case 6:
                    Colmun = Colmun + refNos;
                    CheckBox chk_SrNo5s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo5");
                    chk_SrNo5s.Checked = true;
                    Grd_Material.Columns[Colmun].Visible = true;
                    break;
                case 7:
                    Colmun = Colmun + refNos;
                    CheckBox chk_SrNo6s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo6");
                    chk_SrNo6s.Checked = true;
                    Grd_Material.Columns[Colmun].Visible = true;
                    break;
                case 8:
                    Colmun = Colmun + refNos;
                    CheckBox chk_SrNo7s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo7");
                    chk_SrNo7s.Checked = true;
                    Grd_Material.Columns[Colmun].Visible = true;
                    break;
                case 9:
                    Colmun = Colmun + refNos;
                    CheckBox chk_SrNo8s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo8");
                    chk_SrNo8s.Checked = true;
                    Grd_Material.Columns[Colmun].Visible = true;
                    break;
                case 10:
                    Colmun = Colmun + refNos;
                    CheckBox chk_SrNo9s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo9");
                    chk_SrNo9s.Checked = true;
                    Grd_Material.Columns[Colmun].Visible = true;
                    break;
            }

        }

        //protected void DisplayCheck()
        //{
        //      var mat = dc.MaterialDetail_View(Convert.ToInt32(UC_InwardHeader1.RecordNo ),"",0,"",null,null);
        //      foreach (var m in mat)
        //      {
        //          int refNos = Convert.ToInt32(m.MaterialDetail_RefNo.Substring(m.MaterialDetail_RefNo.LastIndexOf('-') + 1));

        //          int RowNo = 0;
        //          for (int j = 0; j < Grd_Material.Rows.Count; j++)
        //          {
        //              RowNo = Convert.ToInt32(m.MaterialDetail_SrNo - 1);
        //              if (j == RowNo)
        //              {
        //                  int  Colmun = 5;
        //                  switch (refNos)
        //                  {
        //                      case 1:
        //                          Colmun = Colmun + refNos;
        //                          CheckBox chk_SrNos = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo");
        //                          chk_SrNos.Checked = true;
        //                          Grd_Material.Columns[Colmun].Visible = true;
        //                          break;
        //                      case 2:
        //                          Colmun = Colmun + refNos;
        //                          CheckBox chk_SrNo1s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo1");
        //                          chk_SrNo1s.Checked = true;
        //                          Grd_Material.Columns[Colmun].Visible = true;
        //                          break;

        //                      case 3:
        //                          Colmun = Colmun + refNos;
        //                          CheckBox chk_SrNo2s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo2");
        //                          chk_SrNo2s.Checked = true;
        //                          Grd_Material.Columns[Colmun].Visible = true;
        //                          break;
        //                      case 4:
        //                          Colmun = Colmun + refNos;
        //                          CheckBox chk_SrNo3s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo3");
        //                          chk_SrNo3s.Checked = true;
        //                          Grd_Material.Columns[Colmun].Visible = true;
        //                          break;
        //                      case 5:
        //                          Colmun = Colmun + refNos;
        //                          CheckBox chk_SrNo4s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo4");
        //                          chk_SrNo4s.Checked = true;
        //                          Grd_Material.Columns[Colmun].Visible = true;
        //                          break;
        //                      case 6:
        //                          Colmun = Colmun + refNos;
        //                          CheckBox chk_SrNo5s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo5");
        //                          chk_SrNo5s.Checked = true;
        //                          Grd_Material.Columns[Colmun].Visible = true;
        //                          break;
        //                      case 7:
        //                          Colmun = Colmun + refNos;
        //                          CheckBox chk_SrNo6s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo6");
        //                          chk_SrNo6s.Checked = true;
        //                          Grd_Material.Columns[Colmun].Visible = true;
        //                          break;
        //                      case 8:
        //                          Colmun = Colmun + refNos;
        //                          CheckBox chk_SrNo7s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo7");
        //                          chk_SrNo7s.Checked = true;
        //                          Grd_Material.Columns[Colmun].Visible = true;
        //                          break;
        //                      case 9:
        //                          Colmun = Colmun + refNos;
        //                          CheckBox chk_SrNo8s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo8");
        //                          chk_SrNo8s.Checked = true;
        //                          Grd_Material.Columns[Colmun].Visible = true;
        //                          break;
        //                      case 10:
        //                          Colmun = Colmun + refNos;
        //                          CheckBox chk_SrNo9s = (CheckBox)Grd_Material.Rows[j].Cells[Colmun].FindControl("chk_SrNo9");
        //                          chk_SrNo9s.Checked = true;
        //                          Grd_Material.Columns[Colmun].Visible = true;
        //                          break;
        //                  }
        //              }

        //          }
        //      }

        //}
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            RptMixDesign();
        }
        public void RptMixDesign()
        {
            //string reportPath;
            string reportStr = "";
            //StreamWriter sw;
            InwardReport rpt = new InwardReport();
            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            if (UC_InwardHeader1.RecordNo != "" && UC_InwardHeader1.ReferenceNo != "")
            {
                reportStr = rpt.getDetailReportMF(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), false);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("MixDesign_Inward", reportStr);
        }
        bool buttonClicked = false;
        protected string getDetailReportMF()
        {
            DateTime Collectiondate = new DateTime();
            Collectiondate = DateTime.ParseExact(UC_InwardHeader1.CollectionDate, "dd/MM/yyyy", null);
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
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Mix Design LabSheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Mix Design Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }
            var b = dc.ModifyInward_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), Collectiondate, "MF", null, null);

            foreach (var nt in b)
            {
                if (buttonClicked == true)
                {
                    mySql += "<tr>" +

                        "<td width='20%' align=left valign=top height=19> </td>" +
                        "<td width='45%' height=19> </td>" +
                        "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td width='1%' height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "MF" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "MF" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
                        "<td height=19><font size=2>" + "MF" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "MF" + "-" + UC_InwardHeader1.ReferenceNo + "</font></td>" +
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
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Grade of Concrete</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Nature of Work</b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Any Special Requirement</b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Slump Requirement </b></font></td>";
            mySql += "</tr>";

            var n = dc.AllInward_View("MF", Convert.ToInt32(UC_InwardHeader1.RecordNo), "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.MFINWD_Grade_var.ToString() + "</font></td>";
                mySql += "<td width= 20% align=center valign=top height=19 ><font size=2>&nbsp;" + c.MFINWD_NatureofWork_var.ToString() + "</font></td>";
                mySql += "<td width= 20% align=center valign=top height=19 ><font size=2>&nbsp;" + c.MFINWD_SpecialRequirement_var.ToString() + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + c.MFINWD_Slump_var.ToString() + "</font></td>";
                mySql += "</tr>";

            }
            mySql += "</table>";
            mySql += "</table>";
            mySql += "<tr><td colspan=7 align=left valign=top height=20 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=4>" + "Material Inward Details :" + "</font></td></tr>";


            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b> Sr No. </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Material Type </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b> Material </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Material Name </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Qty </b></font></td>";
            mySql += "</tr>";

            SrNo = 0;
            int MatSrNo = 0;
            var Mix = dc.MaterialDetail_View(Convert.ToInt32(UC_InwardHeader1.RecordNo), "", 0, "", null, null, "");
            foreach (var m in Mix)
            {
                if (MatSrNo != Convert.ToInt32(m.MaterialDetail_SrNo) || MatSrNo == 0)
                {
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(m.Material_Type) + "</font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(m.Material_List) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(m.MaterialDetail_Information) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(m.MaterialDetail_Quantity) + "</font></td>";
                    mySql += "</tr>";
                }
                MatSrNo = Convert.ToInt32(m.MaterialDetail_SrNo);
            }
            mySql += "<table >";

            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=4>&nbsp;" + "For all technical queries contact on (020)24348027." + "</font></td></tr>";

            mySql += "</table>";
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
            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            ////reportStr = getDetailReportMF();
            InwardReport rpt = new InwardReport();
            if (UC_InwardHeader1.RecordNo != "" && UC_InwardHeader1.ReferenceNo != "")
            {
                reportStr = rpt.getDetailReportMF(Convert.ToInt32(UC_InwardHeader1.RecordNo), Convert.ToInt32(UC_InwardHeader1.ReferenceNo), true);
            }
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.DownloadHtmlReport("MixDesign_LabSheet", reportStr);
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
                Response.Redirect("InwardStatus.aspx?" + obj.Encrypt(string.Format("RecType={0}", "MF")));
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

        protected void DeleteRowMaterial(int rowIndex)
        {
            GetCurrentDataMaterial();
            DataTable dt = ViewState["MaterialTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["MaterialTable"] = dt;
            Grd_Material.DataSource = dt;
            Grd_Material.DataBind();
            SetPreviousDataMaterial();
        }
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowMaterial();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (Grd_Material.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (Grd_Material.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowMaterial(gvr.RowIndex);
            }
        }
        protected void AddRowMaterial()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MaterialTable"] != null)
            {
                GetCurrentDataMaterial();
                dt = (DataTable)ViewState["MaterialTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_Material", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Qty", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Information", typeof(string)));
                dt.Columns.Add(new DataColumn("chk_SrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("chk_SrNo1", typeof(string)));
                dt.Columns.Add(new DataColumn("chk_SrNo2", typeof(string)));
                dt.Columns.Add(new DataColumn("chk_SrNo3", typeof(string)));
                dt.Columns.Add(new DataColumn("chk_SrNo4", typeof(string)));
                dt.Columns.Add(new DataColumn("chk_SrNo5", typeof(string)));
                dt.Columns.Add(new DataColumn("chk_SrNo6", typeof(string)));
                dt.Columns.Add(new DataColumn("chk_SrNo7", typeof(string)));
                dt.Columns.Add(new DataColumn("chk_SrNo8", typeof(string)));
                dt.Columns.Add(new DataColumn("chk_SrNo9", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["ddl_Material"] = string.Empty;
            dr["txt_Qty"] = string.Empty;
            dr["txt_Information"] = string.Empty;
            if (lblModifyInwd.Text == "")
            {
                dr["chk_SrNo"] = true;
                dr["chk_SrNo1"] = true;
                dr["chk_SrNo2"] = true;
                dr["chk_SrNo3"] = true;
                dr["chk_SrNo4"] = true;
                dr["chk_SrNo5"] = true;
                dr["chk_SrNo6"] = true;
                dr["chk_SrNo7"] = true;
                dr["chk_SrNo8"] = true;
                dr["chk_SrNo9"] = true;
            }
            else
            {
                dr["chk_SrNo"] = false;
                dr["chk_SrNo1"] = false;
                dr["chk_SrNo2"] = false;
                dr["chk_SrNo3"] = false;
                dr["chk_SrNo4"] = false;
                dr["chk_SrNo5"] = false;
                dr["chk_SrNo6"] = false;
                dr["chk_SrNo7"] = false;
                dr["chk_SrNo8"] = false;
                dr["chk_SrNo9"] = false;
            }
            dt.Rows.Add(dr);
            ViewState["MaterialTable"] = dt;
            Grd_Material.DataSource = dt;
            Grd_Material.DataBind();
            SetPreviousDataMaterial();
        }


        protected void SetPreviousDataMaterial()
        {
            DataTable dt = (DataTable)ViewState["MaterialTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddl_Material = (DropDownList)Grd_Material.Rows[i].Cells[3].FindControl("ddl_Material");
                TextBox txt_Qty = (TextBox)Grd_Material.Rows[i].Cells[4].FindControl("txt_Qty");
                TextBox txt_Information = (TextBox)Grd_Material.Rows[i].Cells[5].FindControl("txt_Information");
                CheckBox chk_SrNo = (CheckBox)Grd_Material.Rows[i].Cells[6].FindControl("chk_SrNo");
                CheckBox chk_SrNo1 = (CheckBox)Grd_Material.Rows[i].Cells[7].FindControl("chk_SrNo1");
                CheckBox chk_SrNo2 = (CheckBox)Grd_Material.Rows[i].Cells[8].FindControl("chk_SrNo2");
                CheckBox chk_SrNo3 = (CheckBox)Grd_Material.Rows[i].Cells[9].FindControl("chk_SrNo3");
                CheckBox chk_SrNo4 = (CheckBox)Grd_Material.Rows[i].Cells[10].FindControl("chk_SrNo4");
                CheckBox chk_SrNo5 = (CheckBox)Grd_Material.Rows[i].Cells[11].FindControl("chk_SrNo5");
                CheckBox chk_SrNo6 = (CheckBox)Grd_Material.Rows[i].Cells[12].FindControl("chk_SrNo6");
                CheckBox chk_SrNo7 = (CheckBox)Grd_Material.Rows[i].Cells[13].FindControl("chk_SrNo7");
                CheckBox chk_SrNo8 = (CheckBox)Grd_Material.Rows[i].Cells[14].FindControl("chk_SrNo8");
                CheckBox chk_SrNo9 = (CheckBox)Grd_Material.Rows[i].Cells[15].FindControl("chk_SrNo9");

                Grd_Material.Rows[i].Cells[2].Text = (i + 1).ToString();
                ddl_Material.Text = dt.Rows[i]["ddl_Material"].ToString();
                txt_Qty.Text = dt.Rows[i]["txt_Qty"].ToString();
                txt_Information.Text = dt.Rows[i]["txt_Information"].ToString();
                chk_SrNo.Checked = Convert.ToBoolean(dt.Rows[i]["chk_SrNo"].ToString());
                chk_SrNo1.Checked = Convert.ToBoolean(dt.Rows[i]["chk_SrNo1"].ToString());
                chk_SrNo2.Checked = Convert.ToBoolean(dt.Rows[i]["chk_SrNo2"].ToString());
                chk_SrNo3.Checked = Convert.ToBoolean(dt.Rows[i]["chk_SrNo3"].ToString());
                chk_SrNo4.Checked = Convert.ToBoolean(dt.Rows[i]["chk_SrNo4"].ToString());
                chk_SrNo5.Checked = Convert.ToBoolean(dt.Rows[i]["chk_SrNo5"].ToString());
                chk_SrNo6.Checked = Convert.ToBoolean(dt.Rows[i]["chk_SrNo6"].ToString());
                chk_SrNo7.Checked = Convert.ToBoolean(dt.Rows[i]["chk_SrNo7"].ToString());
                chk_SrNo8.Checked = Convert.ToBoolean(dt.Rows[i]["chk_SrNo8"].ToString());
                chk_SrNo9.Checked = Convert.ToBoolean(dt.Rows[i]["chk_SrNo9"].ToString());

            }
        }
        protected void GetCurrentDataMaterial()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_Material", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Qty", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Information", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chk_SrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chk_SrNo1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chk_SrNo2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chk_SrNo3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chk_SrNo4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chk_SrNo5", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chk_SrNo6", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chk_SrNo7", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chk_SrNo8", typeof(string)));
            dtTable.Columns.Add(new DataColumn("chk_SrNo9", typeof(string)));
            for (int i = 0; i < Grd_Material.Rows.Count; i++)
            {
                DropDownList ddl_Material = (DropDownList)Grd_Material.Rows[i].Cells[3].FindControl("ddl_Material");
                TextBox txt_Qty = (TextBox)Grd_Material.Rows[i].Cells[4].FindControl("txt_Qty");
                TextBox txt_Information = (TextBox)Grd_Material.Rows[i].Cells[5].FindControl("txt_Information");

                CheckBox chk_SrNo = (CheckBox)Grd_Material.Rows[i].Cells[6].FindControl("chk_SrNo");
                CheckBox chk_SrNo1 = (CheckBox)Grd_Material.Rows[i].Cells[7].FindControl("chk_SrNo1");
                CheckBox chk_SrNo2 = (CheckBox)Grd_Material.Rows[i].Cells[8].FindControl("chk_SrNo2");
                CheckBox chk_SrNo3 = (CheckBox)Grd_Material.Rows[i].Cells[9].FindControl("chk_SrNo3");
                CheckBox chk_SrNo4 = (CheckBox)Grd_Material.Rows[i].Cells[10].FindControl("chk_SrNo4");
                CheckBox chk_SrNo5 = (CheckBox)Grd_Material.Rows[i].Cells[11].FindControl("chk_SrNo5");
                CheckBox chk_SrNo6 = (CheckBox)Grd_Material.Rows[i].Cells[12].FindControl("chk_SrNo6");
                CheckBox chk_SrNo7 = (CheckBox)Grd_Material.Rows[i].Cells[13].FindControl("chk_SrNo7");
                CheckBox chk_SrNo8 = (CheckBox)Grd_Material.Rows[i].Cells[14].FindControl("chk_SrNo8");
                CheckBox chk_SrNo9 = (CheckBox)Grd_Material.Rows[i].Cells[15].FindControl("chk_SrNo9");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["ddl_Material"] = ddl_Material.Text;
                drRow["txt_Qty"] = txt_Qty.Text;
                drRow["txt_Information"] = txt_Information.Text;
                drRow["chk_SrNo"] = chk_SrNo.Checked;
                drRow["chk_SrNo1"] = chk_SrNo1.Checked;
                drRow["chk_SrNo2"] = chk_SrNo2.Checked;
                drRow["chk_SrNo3"] = chk_SrNo3.Checked;
                drRow["chk_SrNo4"] = chk_SrNo4.Checked;
                drRow["chk_SrNo5"] = chk_SrNo5.Checked;
                drRow["chk_SrNo6"] = chk_SrNo6.Checked;
                drRow["chk_SrNo7"] = chk_SrNo7.Checked;
                drRow["chk_SrNo8"] = chk_SrNo8.Checked;
                drRow["chk_SrNo9"] = chk_SrNo9.Checked;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["MaterialTable"] = dtTable;
        }


        protected void Grd_Material_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl_Material = (DropDownList)e.Row.FindControl("ddl_Material");
                var Mat = dc.MaterialListView("", "", "");
                ddl_Material.DataSource = Mat;
                ddl_Material.DataTextField = "Material_List";
                ddl_Material.DataValueField = "Material_Id";
                ddl_Material.DataBind();
                ddl_Material.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        protected Boolean ValidateGradeData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;

            for (int i = 0; i < grdMixDesignInward.Rows.Count; i++)
            {
                DropDownList ddl_Grade = (DropDownList)grdMixDesignInward.Rows[i].Cells[1].FindControl("ddl_Grade");

                if (ddl_Grade.SelectedItem.Text == "Select")
                {
                    lblMsg.Text = "Select Grade for Sr No. " + (i + 1) + ".";
                    valid = false;
                    break;
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
        protected void lnkSrNoList_Click(object sender, CommandEventArgs e)
        {
            if (ValidateGradeData() == true)
            {
                ModalPopupExtender1.Show();
                chk_SrNo.Items.Clear();
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                TextBox txt_SrNo = (TextBox)clickedRow.FindControl("txt_SrNo");
                int SrNo = 0;

                for (int j = 0; j < grdMixDesignInward.Rows.Count; j++)
                {
                    DropDownList ddl_Grade = (DropDownList)grdMixDesignInward.Rows[j].Cells[1].FindControl("ddl_Grade");
                    SrNo = SrNo + 1;
                    chk_SrNo.Items.Add(ddl_Grade.SelectedItem.Text + " / " + SrNo.ToString());
                }
                if (txt_SrNo.Text != "")
                {
                    string[] lines = txt_SrNo.Text.Split(',');
                    foreach (string line in lines)
                    {
                        if (line != "")
                        {
                            for (int i = 0; i < chk_SrNo.Items.Count; i++)
                            {
                                if (chk_SrNo.Items[i].Text == line)
                                {
                                    chk_SrNo.Items.FindByValue(line).Selected = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        protected void imgCloseSrNo_Click(object sender, ImageClickEventArgs e)
        {
            TextBox txt_SrNo = Grd_Material.SelectedRow.Cells[6].FindControl("txt_SrNo") as TextBox;
            txt_SrNo.Text = string.Empty;
            foreach (ListItem item in chk_SrNo.Items)
            {
                if (item.Selected)
                {
                    txt_SrNo.Text = txt_SrNo.Text + item + ",";
                }
            }
            ModalPopupExtender1.Hide();
        }

        protected void Img_Exit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
        }

        public void DisplayMaterial()
        {
            TextBox txtSubsets = (TextBox)UC_InwardHeader1.FindControl("txtSubsets");
            if (Convert.ToInt32(txtSubsets.Text) <= 10)
            {
                int i = 5;

                int Counter = 5;
                for (int c = 5; c < Grd_Material.Columns.Count; c++)
                {
                    if (Grd_Material.Columns[c].Visible == true)
                    {
                        Counter = Counter + 1;
                    }
                }

                for (int m = 15; m > 5; m--)
                {
                    Grd_Material.Columns[m].Visible = false;
                }

                for (int j = 0; j < grdMixDesignInward.Rows.Count; j++)
                {
                    i = i + 1;
                    Grd_Material.Columns[i].Visible = true;


                    for (int k = 0; k < Grd_Material.Rows.Count; k++)
                    {
                        if (i == 6 && i >= Counter)
                        {
                            CheckBox chk_SrNo = (CheckBox)Grd_Material.Rows[k].Cells[6].FindControl("chk_SrNo");
                            chk_SrNo.Checked = true;
                        }
                        if (i == 7 && i >= Counter)
                        {
                            CheckBox chk_SrNo1 = (CheckBox)Grd_Material.Rows[k].Cells[7].FindControl("chk_SrNo1");
                            chk_SrNo1.Checked = true;
                        }
                        if (i == 8 && i >= Counter)
                        {
                            CheckBox chk_SrNo2 = (CheckBox)Grd_Material.Rows[k].Cells[8].FindControl("chk_SrNo2");
                            chk_SrNo2.Checked = true;
                        }
                        if (i == 9 && i >= Counter)
                        {
                            CheckBox chk_SrNo3 = (CheckBox)Grd_Material.Rows[k].Cells[9].FindControl("chk_SrNo3");
                            chk_SrNo3.Checked = true;
                        }
                        if (i == 10 && i >= Counter)
                        {
                            CheckBox chk_SrNo4 = (CheckBox)Grd_Material.Rows[k].Cells[10].FindControl("chk_SrNo4");
                            chk_SrNo4.Checked = true;
                        }
                        if (i == 11 && i >= Counter)
                        {
                            CheckBox chk_SrNo5 = (CheckBox)Grd_Material.Rows[k].Cells[11].FindControl("chk_SrNo5");
                            chk_SrNo5.Checked = true;
                        }
                        if (i == 12 && i >= Counter)
                        {
                            CheckBox chk_SrNo6 = (CheckBox)Grd_Material.Rows[k].Cells[12].FindControl("chk_SrNo6");
                            chk_SrNo6.Checked = true;
                        }
                        if (i == 13 && i >= Counter)
                        {
                            CheckBox chk_SrNo7 = (CheckBox)Grd_Material.Rows[k].Cells[13].FindControl("chk_SrNo7");
                            chk_SrNo7.Checked = true;
                        }
                        if (i == 14 && i >= Counter)
                        {
                            CheckBox chk_SrNo8 = (CheckBox)Grd_Material.Rows[k].Cells[14].FindControl("chk_SrNo8");
                            chk_SrNo8.Checked = true;
                        }
                        if (i == 15 && i >= Counter)
                        {
                            CheckBox chk_SrNo9 = (CheckBox)Grd_Material.Rows[k].Cells[15].FindControl("chk_SrNo9");
                            chk_SrNo9.Checked = true;
                        }
                    }
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
