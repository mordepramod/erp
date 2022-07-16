using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace DESPLWEB
{
    public partial class Solid_Report_WA : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        static int[] remarkDelId = new int[0];
        static int[] remarkEditId = new int[0];
        static string exitFlag; //flagDrywtEdit, flagWetwtEdit
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
                        Session.Abandon();
                        Response.Redirect("Login.aspx");
                    }
                    string[] arrMsgs = strReq.Split('&');
                    string[] arrIndMsg;

                    arrIndMsg = arrMsgs[0].Split('=');
                    TxtreportNoSolid.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[1].Split('=');
                    lblRecordNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[2].Split('=');
                    TxtRefNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[3].Split('=');
                    lblEnter.Text = arrIndMsg[1].ToString().Trim();
                }

                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                //bind Approved by and Checked By
                bindApprovedBy();
                //bind Reference no.
                //bindRefNo();
                getData();
                getremarkData();
                exitFlag = "false";
                if (lblEnter.Text == "Check")
                {
                    lbl_TestedBy.Text = "Approve By";
                    bindApprovedBy();
                }
                else
                {
                    LoadTestedBy();
                }
                Label lblheading = (Label)Master.FindControl("lblheading");
                var solidHeader = dc.Solid_Inward_View("", 2, "SOLID");
                foreach (var h in solidHeader)
                {
                    lblheading.Text = h.TEST_Name_var.ToString();
                }
                LoadReferenceNoList();
            }
        }
        private void LoadReferenceNoList()
        {
            byte reportStatus = 0;
            if (lblEnter.Text == "Enter")
                reportStatus = 1;
            else if (lblEnter.Text == "Check")
                reportStatus = 2;

            var reportList = dc.ReferenceNo_View_StatusWise("SOLID", reportStatus, Convert.ToInt32(lblTestId.Text));
            DrpRefNo.DataTextField = "ReferenceNo";
            DrpRefNo.DataSource = reportList;
            DrpRefNo.DataBind();
            DrpRefNo.Items.Insert(0, new ListItem("---Select---", "0"));
            DrpRefNo.Items.Remove(TxtRefNo.Text);
        }
        private void LoadTestedBy()
        {
            ddl_TestedBy.DataTextField = "USER_Name_var";
            ddl_TestedBy.DataValueField = "USER_Id";
            var testinguser = dc.ReportStatus_View("", null, null, 0, 1, 0, "", 0, 0, 0);
            ddl_TestedBy.DataSource = testinguser;
            ddl_TestedBy.DataBind();
            ddl_TestedBy.Items.Insert(0, "---Select---");
        }
        public void bindApprovedBy()
        {
            //var approvedBy = from k in dc.tbl_Users where k.USER_Approve_right_bit == true select k.USER_Name_var;
            //DrpApprovedBy.DataSource = approvedBy;
            //DrpApprovedBy.DataBind();

            ddl_TestedBy.DataTextField = "USER_Name_var";
            ddl_TestedBy.DataValueField = "USER_Id";
            var testinguser = dc.ReportStatus_View("", null, null, 0, 0, 1, "", 0, 0, 0);
            ddl_TestedBy.DataSource = testinguser;
            ddl_TestedBy.DataBind();
            ddl_TestedBy.Items.Insert(0, "---Select---");
            //var approvedBy = from k in dc.tbl_Users where k.USER_Approve_right_bit == true select k.USER_Name_var;
            //       DrpApprovedBy.DataSource = approvedBy;
            //       DrpApprovedBy.DataBind();
        }

        public void getremarkData()
        {
            #region Remark Gridview
            //if remark is present                
            //var remark_list = dc.Remark_View(TxtRefNo.Text);
            //int qty = 0;
            //if (TxtQty.Text != "")
            //{
            //    qty = Convert.ToInt32(TxtQty.Text);
            //}
            //List<tbl_Solid_Remark> Idlist = remark_list.AsEnumerable()
            //              .Select(o => new tbl_Solid_Remark
            //              {
            //                  ID = o.ID,
            //                  Name = o.Name
            //              }).ToList();
            //var remarkcount = Idlist.Count();
            var re = dc.AllRemark_View("", TxtRefNo.Text, 0, "SOLID").ToList();
            var remarkcount = re.Count();
            if (remarkcount == 0)
            {

                //if remark is not present add empty row
                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("RowNumber", typeof(string)));
                dt1.Columns.Add(new DataColumn("Col2", typeof(string)));
                dr1 = dt1.NewRow();
                dr1["RowNumber"] = 1;
                dr1["Col2"] = string.Empty;
                dt1.Rows.Add(dr1);
                ViewState["CurrentTable"] = dt1;
                GrdRemark.DataSource = dt1;
                GrdRemark.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
                dt.Columns.Add(new DataColumn("Col2", typeof(string)));
                DataRow dr = null;
                for (int i = 0; i < remarkcount; i++)
                {

                    dr = dt.NewRow();
                    dr["RowNumber"] = i + 1;
                    dr["Col2"] = string.Empty;
                    dt.Rows.Add(dr);
                    ViewState["CurrentTable"] = dt;

                }
                GrdRemark.DataSource = dt;
                GrdRemark.DataBind();
            }
            int cnt = 0;
            foreach (GridViewRow row in GrdRemark.Rows)
            {
                TextBox txtRemark = (row.FindControl("TxtRemark") as TextBox);
                if (remarkcount != 0)
                {
                    var remark = dc.AllRemark_View("", "", Convert.ToInt32(re[cnt].SOLIDDetail_Remark_ID), "SOLID");
                    foreach (var rem in remark)
                    {
                        txtRemark.Text = Convert.ToString(rem.SOLID_Remark_var);
                    }
                    cnt++;
                }
            }



            #endregion
        }
        public void bindRefNo()
        {
            var QueryRefNo = dc.RefNo_View(true);
            List<string> refNoList = new List<string>();
            foreach (var q in QueryRefNo)
            {
                //   TxtRefNo.Text = q.SOLIDINWD_ReferenceNo_var;
                refNoList.Add(q.SOLIDINWD_ReferenceNo_var);
            }
            DrpRefNo.DataSource = refNoList;
            DrpRefNo.DataBind();
            DrpRefNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select-"));
            DrpRefNo.Items.Remove(TxtRefNo.Text);
            Label lblheading = (Label)Master.FindControl("lblheading");
            //var header = from h in dc.tbl_Tests where h.Test_RecType_var == "SOLID" && h.TEST_Sr_No == 2 select h.TEST_Name_var;            
            //lblheading.Text = header.FirstOrDefault();
            var solidHeader = dc.Solid_Inward_View("", 2, "SOLID");
            foreach (var h in solidHeader)
            {
                lblheading.Text = h.TEST_Name_var.ToString();
            }
        }
        public void getData()
        {

            try
            {
                #region disply Solid Masonary data
                var queryData = dc.Solid_Inward_View(TxtRefNo.Text, 0, "SOLID");
                string idMark = "";
                foreach (var data in queryData)
                {
                    TxtRefNo.Text = data.SOLIDINWD_ReferenceNo_var.ToString();
                    TxtDesc.Text = data.SOLIDINWD_Description_var;
                    TxtSupplierName.Text = data.SOLIDINWD_SupplierName_var;
                    TxtReportNo.Text = data.SOLIDINWD_SetOfRecord_var;
                    TxtDateOfCast.Text = data.SOLIDINWD_CastingDate_nvar;
                    lblTestId.Text = data.SOLIDINWD_TEST_Id.ToString();
                    //if (TxtDateOfCast.Text != "NA")
                    //{
                    //    TxtDateOfCast.Text = Convert.ToDateTime(data.SOLIDINWD_CastingDate_nvar).ToString("dd/MM/yyyy");
                    //}

                    if (ddl_NablScope.Items.FindByValue(data.SOLIDINWD_NablScope_var) != null)
                    {
                        ddl_NablScope.SelectedValue = Convert.ToString(data.SOLIDINWD_NablScope_var);
                    }
                    if (Convert.ToString(data.SOLIDINWD_NablLocation_int) != null && Convert.ToString(data.SOLIDINWD_NablLocation_int) != "")
                    {
                        ddl_NABLLocation.SelectedValue = Convert.ToString(data.SOLIDINWD_NablLocation_int);
                    }


                    TxtDateOfTest.Text = Convert.ToDateTime(data.SOLIDINWD_TestingDate_dt).ToString("dd/MM/yyyy");
                    TxtQty.Text = data.SOLIDINWD_Quantity_tint.ToString();
                    idMark = data.SOLIDINWD_IdMark_var;
                    if (data.SOLIDINWD_WitnessBy_var != "" && data.SOLIDINWD_WitnessBy_var != null)
                    {
                        TxtWitnesBy.Visible = true;
                        TxtWitnesBy.Text = Convert.ToString(data.SOLIDINWD_WitnessBy_var);
                        ChkboxWitnessBy.Checked = true;
                    }
                    else
                    {
                        TxtWitnesBy.Visible = false;
                        ChkboxWitnessBy.Checked = false;
                    }
                }

                //var checkedUser = from k in dc.tbl_Users where k.USER_Entry_right_bit == true select k.USER_Name_var;
                //TxtcheckBy.Text = checkedUser.FirstOrDefault().ToString();

                //To Repeate Rows according to quantity
                int qty = Convert.ToInt32(TxtQty.Text);
                DataTable dt = new DataTable();
                DataColumn dtcolumn = new DataColumn();

                for (int i = 0; i < qty; i++)
                {
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add("Sr. No.", typeof(string));
                        dt.Columns.Add("ID Mark", typeof(string));
                        dt.Columns.Add("Dry Weight", typeof(string));
                        dt.Columns.Add("Wet Weight", typeof(string));
                        dt.Columns.Add("Water Absorption ", typeof(string));
                        dt.Columns.Add("Avg. Water Absorption %", typeof(string));
                        dt.Columns.Add("ID", typeof(string));
                    }

                    DataRow NewRow = dt.NewRow();
                    dt.Rows.Add(NewRow);

                    GrdViewCS.DataSource = dt;
                    GrdViewCS.DataBind();

                }
                for (int j = 0; j < GrdViewCS.Rows.Count; j++)
                {
                    TextBox LblIdMark = (TextBox)GrdViewCS.Rows[j].Cells[2].FindControl("lblIdMark");
                    LblIdMark.Text = idMark;
                }

                //Display data in grid  
                var list = dc.Solid_Inward_WA_View(TxtRefNo.Text).ToList();
                var remarkcount = list.Count();
                int cnt = 0;
                foreach (var grdData in list)
                {
                    TextBox TxtIdMark = (TextBox)GrdViewCS.Rows[cnt].Cells[1].FindControl("lblIdMark");
                    TextBox TxtDryWt = (TextBox)GrdViewCS.Rows[cnt].Cells[2].FindControl("TxtDryWt");
                    TextBox TxtWetWt = (TextBox)GrdViewCS.Rows[cnt].Cells[3].FindControl("TxtWetWt");
                    TextBox TxtWaterAbsorption = (TextBox)GrdViewCS.Rows[cnt].Cells[4].FindControl("TxtWaterAbsorption");
                    TextBox TxtAvgWaterAbsorp = (TextBox)GrdViewCS.Rows[qty / 2].Cells[5].FindControl("TxtAvgWaterAbsorp");
                    Label LblID = (Label)GrdViewCS.Rows[cnt].Cells[6].FindControl("LblID");
                    string Ids = grdData.ID.ToString();
                    LblID.Text = Ids;
                    TxtIdMark.Text = grdData.SOLIDINWD_WA_ID_Mark_var;
                    decimal DryWt = Convert.ToDecimal(grdData.SOLIDINWD_WA_Dry_wt);
                    TxtDryWt.Text = DryWt.ToString();
                    decimal WetWt = Convert.ToDecimal(grdData.SOLIDINWD_WA_Wet_wt);
                    TxtWetWt.Text = WetWt.ToString();
                    decimal WaterAbsorption = Convert.ToDecimal(grdData.SOLIDINWD_WA_Water_Abs);
                    TxtWaterAbsorption.Text = WaterAbsorption.ToString();
                    decimal AvgWaterAbsorp = Convert.ToDecimal(grdData.SOLIDINWD_WA_Avg_WA);
                    TxtAvgWaterAbsorp.Text = AvgWaterAbsorp.ToString();
                    if (qty < 3)
                    {
                        TxtAvgWaterAbsorp.Text = "***";
                    }
                    else
                    {
                        TxtAvgWaterAbsorp.Text = AvgWaterAbsorp.ToString();
                    }

                    int approved_By = Convert.ToInt32(list.FirstOrDefault().SOLIDINWD_WA_Approved_By);
                    //var approvedUser = from u in dc.tbl_Users where u.USER_Id == approved_By select u.USER_Name_var;
                    //string appby = approvedUser.FirstOrDefault();
                    //DrpApprovedBy.SelectedValue = approvedUser.FirstOrDefault();
                    cnt++;
                }

                if (remarkcount == 0)
                {
                    for (int j = 0; j < GrdViewCS.Rows.Count; j++)
                    {
                        TextBox LblIdMark = (TextBox)GrdViewCS.Rows[j].Cells[1].FindControl("lblIdMark");
                        if (idMark == "" || idMark == null)
                        {
                            LblIdMark.Text = "-";
                        }
                        else
                        {
                            LblIdMark.Text = idMark;
                        }
                    }

                    ChkboxWitnessBy.Checked = false;
                    TxtWitnesBy.Visible = false;
                    TxtWitnesBy.Text = "";
                }


                #region print button enable / disabled
                //bool printFlag = false;
                //for (int j = 0; j < GrdViewCS.Rows.Count; j++)
                //{
                //    TextBox TxtDryWt = (TextBox)GrdViewCS.Rows[j].Cells[3].FindControl("TxtDryWt");
                //    TextBox TxtWetWt = (TextBox)GrdViewCS.Rows[j].Cells[4].FindControl("TxtWetWt");


                //    if (TxtDryWt.Text == "" || TxtWetWt.Text == "")
                //    {
                //        printFlag = true;
                //        break;
                //    }
                //}
                //if (printFlag == true)
                //{
                //    lnkPrint.Enabled = false;
                //}
                //else
                //{
                //    lnkPrint.Enabled = true;
                //}
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        protected void TxtDryWt_TextChanged(object sender, EventArgs e)
        {
            exitFlag = "true";
            GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
            waterAbsCal(currentRow);
            //flagDrywtEdit = "true";
        }
        protected void TxtWetWt_TextChanged(object sender, EventArgs e)
        {
            exitFlag = "true";
            GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
            waterAbsCal(currentRow);
            //flagWetwtEdit = "true";
        }

        protected void waterAbsCal(GridViewRow currentRow)
        {
            try
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                TextBox txtDryWet = (TextBox)currentRow.FindControl("TxtDryWt");
                TextBox txtWetWt = (TextBox)currentRow.FindControl("TxtWetWt");
                TextBox txtwaterAbsorption = (TextBox)currentRow.FindControl("TxtWaterAbsorption");
                decimal dryWet = 0, wetWt = 0;

                if (txtDryWet.Text != "" && txtWetWt.Text != "")
                {
                    dryWet = Convert.ToDecimal(txtDryWet.Text);
                    wetWt = Convert.ToDecimal(txtWetWt.Text);

                    if (dryWet > wetWt)
                    {
                        lblMsg.Text = "Dry wt. must be less than Wet Wt";
                        lblMsg.Visible = true;
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Dry wt. must be less than Wet Wt.');", true);

                        return;
                    }

                    if (dryWet != 0)
                    {
                        decimal waterAbsorption = Math.Round((100 * (wetWt - dryWet)) / dryWet, 2);
                        txtwaterAbsorption.Text = waterAbsorption.ToString();
                    }
                    else
                    {
                        txtwaterAbsorption.Text = "0.00";
                    }

                    decimal total = 0;
                    int qty = Convert.ToInt32(TxtQty.Text);
                    foreach (GridViewRow row in GrdViewCS.Rows)
                    {
                        var numberLabel = row.FindControl("TxtWaterAbsorption") as TextBox;

                        decimal number;
                        if (decimal.TryParse(numberLabel.Text, out number))
                        {
                            total += number;
                        }

                    }

                    int rows = qty / 2;
                    string emptyRemark = ((TextBox)GrdViewCS.Rows[qty - 1].Cells[4].FindControl("TxtWaterAbsorption")).Text;
                    if (qty >= 3)
                    {
                        decimal avgWA = total / qty;
                        decimal average = Math.Round(avgWA, 2);

                        if (emptyRemark != "")
                        {
                            TextBox TxtAvgWaterAbsorp = (TextBox)GrdViewCS.Rows[rows].Cells[5].FindControl("TxtAvgWaterAbsorp");
                            TxtAvgWaterAbsorp.Text = average.ToString();
                        }
                    }
                    else
                    {
                        if (emptyRemark != "")
                        {
                            TextBox TxtAvgWaterAbsorp = (TextBox)GrdViewCS.Rows[qty / 2].Cells[5].FindControl("TxtAvgWaterAbsorp");
                            TxtAvgWaterAbsorp.Text = "***";
                        }

                    }

                }

            }
            catch
            { }
        }



        protected void TxtWaterAbsorption_TextChanged(object sender, EventArgs e)
        {

        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {

        }

        protected void BtnRemove_Click(object sender, EventArgs e)
        {

        }
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }
        protected void lnk_Exit_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        private void AddNewRow()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        TextBox TxtRemark = (TextBox)GrdRemark.Rows[rowIndex].Cells[1].FindControl("TxtRemark");
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;
                        dtCurrentTable.Rows[i - 1]["Col2"] = TxtRemark.Text;
                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;

                    GrdRemark.DataSource = dtCurrentTable;
                    GrdRemark.DataBind();
                }
            }
            else
            {

            }
            SetPreviousData();
        }

        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox TxtRemark = (TextBox)GrdRemark.Rows[rowIndex].Cells[1].FindControl("TxtRemark");
                        TxtRemark.Text = dt.Rows[i]["Col2"].ToString();
                        rowIndex++;
                    }
                }
            }
        }

        private void SetRowData()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        TextBox TxtRemark = (TextBox)GrdRemark.Rows[rowIndex].Cells[1].FindControl("TxtRemark");
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;
                        dtCurrentTable.Rows[i - 1]["Col2"] = TxtRemark.Text;
                        rowIndex++;
                    }

                    ViewState["CurrentTable"] = dtCurrentTable;

                }
            }
            else
            {

            }
            //SetPreviousData();
        }


        protected void GrdRemark_RowEditing(object sender, GridViewEditEventArgs e)
        {
            System.Web.HttpContext.Current.Response.Write("<script type = 'text/javascript'> alert('Record Inserted Successfully');</script>");
        }

        protected void GrdRemark_DataBound(object sender, EventArgs e)
        {

        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {

            if (ValidateData() == true)
            {
                DateTime dateTest = DateTime.ParseExact(TxtDateOfTest.Text, "dd/MM/yyyy", null);
                var referenceno = dc.Solid_Inward_WA_View(TxtRefNo.Text).ToList();
                string approved_by = "", checked_by = "", IdMark = "", average = "";
                decimal dryWt = 0, Wetwt = 0, waterAbs = 0;
                int approvedBy = 0, checkedBy = 0, Pk_ID = 0;
                DateTime TestingDt = DateTime.ParseExact(TxtDateOfTest.Text, "dd/MM/yyyy", null);
                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.MISDetail_Update(0, "SOLID", TxtRefNo.Text, "SOLID", null, true, false, false, false, false, false, false);
                    checked_by = ddl_TestedBy.SelectedValue;
                    dc.AllInwd_Update(TxtRefNo.Text, TxtWitnesBy.Text, 0, TestingDt, TxtDesc.Text, TxtSupplierName.Text, 2, 0, "", 0, "", "", 0, 0, 0, "SOLID");
                    dc.ReportDetails_Update("SOLID", TxtRefNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.MISDetail_Update(0, "SOLID", TxtRefNo.Text, "SOLID", null, false, true, false, false, false, false, false);
                    approved_by = ddl_TestedBy.SelectedValue;
                    dc.AllInwd_Update(TxtRefNo.Text, TxtWitnesBy.Text, 0, TestingDt, TxtDesc.Text, TxtSupplierName.Text, 3, 0, "", 0, "", "", 0, 0, 0, "SOLID");
                    dc.ReportDetails_Update("SOLID", TxtRefNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                }
               
                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(TxtRefNo.Text, "SOLID", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                if (referenceno.Count() == 0)
                {
                    int rowcount = 0;
                    int cnt = 0;
                    foreach (GridViewRow row in GrdViewCS.Rows)
                    {
                        dryWt = Convert.ToDecimal((row.FindControl("TxtDryWt") as TextBox).Text);
                        Wetwt = Convert.ToDecimal((row.FindControl("TxtWetWt") as TextBox).Text);
                        waterAbs = Convert.ToDecimal((row.FindControl("TxtWaterAbsorption") as TextBox).Text);
                        // average = Convert.ToDecimal((row.FindControl("TxtAvgWaterAbsorp") as TextBox).Text);
                        IdMark = (row.FindControl("lblIdMark") as TextBox).Text;
                        //approved_by = DrpApprovedBy.SelectedValue;
                        //checked_by = TxtcheckBy.Text;
                        int qty = Convert.ToInt32(TxtQty.Text);
                        if (rowcount == qty - 1)
                        {
                            average = ((TextBox)GrdViewCS.Rows[qty / 2].Cells[5].FindControl("TxtAvgWaterAbsorp")).Text;
                        }
                        if (average == "***" || average == "")
                        {
                            average = "0.00";
                        }

                        //var approved = from a in dc.tbl_Users where a.USER_Name_var == approved_by select a.USER_Id;
                        //approvedBy = approved.FirstOrDefault();
                        //var checkd = from c in dc.tbl_Users where c.USER_Name_var == checked_by select c.USER_Id;
                        //checkedBy = checkd.FirstOrDefault();


                        dc.Solid_Inword_WA_Update(null, TxtRefNo.Text, TxtReportNo.Text, TxtDesc.Text, TxtSupplierName.Text, IdMark, TxtDateOfCast.Text, dateTest, Convert.ToInt32(TxtQty.Text), dryWt, Wetwt, Convert.ToDecimal(waterAbs), Convert.ToDecimal(average), approvedBy, checkedBy, false);
                        dc.SubmitChanges();

                        rowcount++;

                        var remarkcount = referenceno.Count();
                        if (remarkcount != 0)
                        {
                            Label LblID = (row.FindControl("LblID") as Label);
                            string Ids = referenceno[cnt].ID.ToString();
                            LblID.Text = Ids;
                            cnt++;
                        }
                    }
                }
                else
                {

                    int i = 0;
                    int rowcount = 0;
                    foreach (GridViewRow row in GrdViewCS.Rows)
                    {
                        dryWt = Convert.ToDecimal((row.FindControl("TxtDryWt") as TextBox).Text);
                        Wetwt = Convert.ToDecimal((row.FindControl("TxtWetWt") as TextBox).Text);
                        waterAbs = Convert.ToDecimal((row.FindControl("TxtWaterAbsorption") as TextBox).Text);
                        Pk_ID = Convert.ToInt32((row.FindControl("LblID") as Label).Text);
                        IdMark = (row.FindControl("lblIdMark") as TextBox).Text;
                        //approved_by = DrpApprovedBy.SelectedValue;
                        //checked_by = TxtcheckBy.Text;
                        int qty = Convert.ToInt32(TxtQty.Text);
                        if (rowcount == qty - 1)
                        {
                            average = ((TextBox)GrdViewCS.Rows[qty / 2].Cells[5].FindControl("TxtAvgWaterAbsorp")).Text;
                        }
                        if (average == "***" || average == "")
                        {
                            average = "0.00";
                        }

                        //var approved = from a in dc.tbl_Users where a.USER_Name_var == approved_by select a.USER_Id;
                        //approvedBy = approved.FirstOrDefault();
                        //var checkd = from c in dc.tbl_Users where c.USER_Name_var == checked_by select c.USER_Id;
                        //checkedBy = checkd.FirstOrDefault();

                        dc.Solid_Inword_WA_Update(Pk_ID, TxtRefNo.Text, TxtReportNo.Text, TxtDesc.Text, TxtSupplierName.Text, IdMark, TxtDateOfCast.Text, dateTest, Convert.ToInt32(TxtQty.Text), dryWt, Wetwt, Convert.ToDecimal(waterAbs), Convert.ToDecimal(average), approvedBy, checkedBy, true);
                        dc.SubmitChanges();

                        i++;
                        rowcount++;
                    }
                }
                #region save data in solid inward
                string wtnesBy = null;
                bool WitnessFlag = false;
                if (ChkboxWitnessBy.Checked)
                {
                    wtnesBy = TxtWitnesBy.Text;
                    WitnessFlag = true;
                }
                //dc.Solid_Inword_Update(TxtRefNo.Text, TxtDesc.Text, TxtSupplierName.Text, wtnesBy, IdMark, Convert.ToDateTime(TxtDateOfTest.Text), WitnessFlag);  
                dc.Solid_Inword_Update(TxtRefNo.Text, TxtDesc.Text, TxtSupplierName.Text, wtnesBy, dateTest, TxtReportNo.Text, Convert.ToDecimal(average), TxtDateOfCast.Text, Convert.ToInt32(TxtQty.Text), WitnessFlag);//approvedBy, checkedBy,
                dc.SubmitChanges();
                #endregion
                #region Remark Gridview
                int RemarkId = 0;
                dc.AllRemarkDetail_Update(TxtRefNo.Text, RemarkId, "SOLID", true);
                for (int i = 0; i < GrdRemark.Rows.Count; i++)
                {
                    TextBox TxtRemark = (TextBox)GrdRemark.Rows[i].Cells[1].FindControl("TxtRemark");
                    if (TxtRemark.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AllRemark_View(TxtRemark.Text, "", 0, "SOLID");
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.SOLID_RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AllRemark_View("", TxtRefNo.Text, 0, "SOLID");
                            foreach (var c in chkId)
                            {
                                if (c.SOLIDDetail_Remark_ID == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {

                                dc.AllRemarkDetail_Update(TxtRefNo.Text, RemarkId, "SOLID", false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.AllRemark_Update(0, TxtRemark.Text, "SOLID");
                            var chc = dc.AllRemark_View(TxtRemark.Text, "", 0, "SOLID");
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.SOLID_RemarkId_int);
                                dc.AllRemarkDetail_Update(TxtRefNo.Text, RemarkId, "SOLID", false);
                            }
                        }
                    }
                }
                #endregion
                //#region insert data in remark table

                //var remarkName = from rm in dc.tbl_Solid_Remarks select rm;

                //var remarklist = dc.RemarkList_View(TxtRefNo.Text);
                //List<tbl_Solid_Remark_dtl> remarkIdlist = remarklist.AsEnumerable()
                //              .Select(o => new tbl_Solid_Remark_dtl
                //              {
                //                  ID = o.ID,
                //                  SOLIDINWD_Remark_ID = o.SOLIDINWD_Remark_ID
                //              }).ToList();
                //var remarkcnt = remarkIdlist.Count();


                ////first delete data
                //var rDetail = from rDtl in dc.tbl_Solid_Remark_dtls where rDtl.SOLIDINWD_ReferenceNo_var == TxtRefNo.Text select rDtl;
                //dc.tbl_Solid_Remark_dtls.DeleteAllOnSubmit(rDetail);
                //dc.SubmitChanges();

                //if (remarkName.Count() > 0)
                //{
                //    int count = 0;

                //    foreach (GridViewRow remarkRow in GrdRemark.Rows)
                //    {
                //        string remark_txt = (remarkRow.FindControl("TxtRemark") as TextBox).Text;

                //        foreach (var rmName in remarkName)
                //        {
                //            if (rmName.Name == remark_txt)
                //            {
                //                count = 0;
                //                break;
                //            }
                //            else
                //            {
                //                count = 1;
                //            }
                //        }

                //        if (count != 0)
                //        {
                //            if (remark_txt != "")
                //            {
                //                //sp
                //                dc.Solid_Remark(null, remark_txt, false);
                //                dc.SubmitChanges();

                //                var remarkId = from r in dc.tbl_Solid_Remarks where r.Name == remark_txt select r.ID;

                //                dc.Solid_Remark_dtl(null, TxtRefNo.Text, remarkId.FirstOrDefault(), false);
                //                dc.SubmitChanges();
                //            }
                //        }
                //        else
                //        {
                //            var remark_ID = from r in dc.tbl_Solid_Remarks where r.Name == remark_txt select r.ID;
                //            var r_dtl = from rdtl in dc.tbl_Solid_Remark_dtls where rdtl.SOLIDINWD_ReferenceNo_var == TxtRefNo.Text && rdtl.SOLIDINWD_Remark_ID == remark_ID.FirstOrDefault() select rdtl;

                //            if (r_dtl.Count() == 0)
                //            {
                //                var remarkId = from r in dc.tbl_Solid_Remarks where r.Name == remark_txt select r.ID;

                //                if (remark_txt != "")
                //                {
                //                    //sp
                //                    dc.Solid_Remark_dtl(null, TxtRefNo.Text, remarkId.FirstOrDefault(), false);
                //                    dc.SubmitChanges();
                //                }
                //            }
                //        }
                //    }
                //}
                //else
                //{

                //    int count = 0;
                //    foreach (GridViewRow remarkRow in GrdRemark.Rows)
                //    {
                //        string remark_txt = (remarkRow.FindControl("TxtRemark") as TextBox).Text;
                //        foreach (var rmName in remarkName)
                //        {
                //            if (rmName.Name == remark_txt)
                //            {
                //                count = 0;
                //                break;
                //            }
                //            else
                //            {
                //                count = 1;
                //            }
                //        }

                //        if (count != 0 || remarkName.Count() == 0)
                //        {
                //            if (remark_txt != "")
                //            {
                //                //sp for add new remark
                //                dc.Solid_Remark(null, remark_txt, false);
                //                dc.SubmitChanges();

                //                var remarkId = from r in dc.tbl_Solid_Remarks where r.Name == remark_txt select r.ID;

                //                dc.Solid_Remark_dtl(null, TxtRefNo.Text, remarkId.FirstOrDefault(), false);
                //                dc.SubmitChanges();
                //            }
                //        }
                //        else
                //        {
                //            var remark_ID = from r in dc.tbl_Solid_Remarks where r.Name == remark_txt select r.ID;
                //            var r_dtl = from rdtl in dc.tbl_Solid_Remark_dtls where rdtl.SOLIDINWD_ReferenceNo_var == TxtRefNo.Text && rdtl.SOLIDINWD_Remark_ID == remark_ID.FirstOrDefault() select rdtl;

                //            if (r_dtl.Count() == 0)
                //            {
                //                var remarkId = from r in dc.tbl_Solid_Remarks where r.Name == remark_txt select r.ID;

                //                if (remark_txt != "")
                //                {
                //                    //sp
                //                    dc.Solid_Remark_dtl(null, TxtRefNo.Text, remarkId.FirstOrDefault(), false);
                //                    dc.SubmitChanges();
                //                }
                //            }
                //        }
                //    }
                //}

                //#endregion



                #region delete remarks
                //remarkDelId

                //for (int d_id = 0; d_id < remarkDelId.Count(); d_id++)
                //{

                //    var delete = from del in dc.tbl_Solid_Remark_dtls where del.ID == remarkDelId[d_id] select del;
                //    foreach (var d in delete)
                //    {
                //        dc.tbl_Solid_Remark_dtls.DeleteOnSubmit(d);
                //    }
                //    dc.SubmitChanges();
                //}

                #endregion


                //ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Report Saved Successfully');", true);
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkPrint.Visible = true;
                lnkSave.Enabled = false;
                //fetch PK Id
                var list = dc.Solid_Inward_WA_View(TxtRefNo.Text).ToList();
                int Idcnt = 0;
                foreach (var grdData in list)
                {
                    Label LblID = (Label)GrdViewCS.Rows[Idcnt].Cells[6].FindControl("LblID");

                    LblID.Text = grdData.ID.ToString();
                    Idcnt++;
                }
            }

        }

        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");

            Boolean valid = true;
            lblMsg.ForeColor = System.Drawing.Color.Red;
            DateTime datecast = new DateTime();
            DateTime dateTest = DateTime.ParseExact(TxtDateOfTest.Text, "dd/MM/yyyy", null);
            //DateTime dateTest = Convert.ToDateTime(TxtDateOfTest.Text);
            //Witness by validation
            if (TxtDateOfCast.Text != "NA")
            {
                datecast = DateTime.ParseExact(TxtDateOfCast.Text, "dd/MM/yyyy", null);
            }
            if (ChkboxWitnessBy.Checked == true && TxtWitnesBy.Text == "")
            {
                lblMsg.Text = "Please Enter Witness By Name.";
                TxtWitnesBy.Focus();
                valid = false;
            }
            else if (dateTest > System.DateTime.Now)
            {
                lblMsg.Text = "Date Of Testing must be less than or equal to Current Date.";
                TxtDateOfTest.Focus();
                valid = false;
            }
            else if (TxtDateOfCast.Text != "NA" && dateTest < datecast)
            {
                lblMsg.Text = "Date of Testing must be greater than or equal to Date of Casting.";
                TxtDateOfCast.Focus();
                valid = false;
            }
            else if (TxtDesc.Text == "")
            {
                lblMsg.Text = "Please Enter Description";
                TxtDesc.Focus();
                valid = false;
            }
            else if (TxtSupplierName.Text == "")
            {
                lblMsg.Text = "Please Enter Supplier Name";
                TxtSupplierName.Focus();
                valid = false;
            }
            else if (ddl_NablScope.SelectedItem.Text == "--Select--")
            {
                lblMsg.Text = "Select 'NABL Scope'.";
                valid = false;
                ddl_NablScope.Focus();
            }
            else if (ddl_NABLLocation.SelectedItem.Text == "--Select--" )
            {
                lblMsg.Text = "Select 'NABL Location'.";
                valid = false;
                ddl_NABLLocation.Focus();
            }
            else if (lblEnter.Text == "Check" && ddl_TestedBy.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select the Approve By";
                valid = false;
            }
            else if (lblEnter.Text != "Check" && ddl_TestedBy.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select the Tested By";
                valid = false;
            }
            else if (valid == true)
            {
                for (int i = 0; i < GrdViewCS.Rows.Count; i++)
                {
                    TextBox lblIdMark = (TextBox)GrdViewCS.Rows[i].Cells[1].FindControl("lblIdMark");
                    TextBox txtDryWt = (TextBox)GrdViewCS.Rows[i].Cells[2].FindControl("TxtDryWt");
                    TextBox txtWetWt = (TextBox)GrdViewCS.Rows[i].Cells[3].FindControl("TxtWetWt");

                    if (txtDryWt.Text != "" && txtWetWt.Text != "")
                    {
                        decimal dryWet = Convert.ToDecimal(txtDryWt.Text);
                        decimal wetWt = Convert.ToDecimal(txtWetWt.Text);

                        if (dryWet > wetWt)
                        {
                            lblMsg.Text = "Dry wt. must be less than Wet Wt.";
                            txtWetWt.Focus();
                            valid = false;
                        }
                    }

                    if (lblIdMark.Text == "")
                    {
                        lblMsg.Text = "Enter Id Mark for row no " + (i + 1) + ".";
                        lblIdMark.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtDryWt.Text == "")
                    {
                        lblMsg.Text = "Enter Dry Wt. for row number " + (i + 1) + ".";
                        txtDryWt.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtWetWt.Text == "")
                    {
                        lblMsg.Text = "Enter Wet Wt. for row number " + (i + 1) + ".";
                        txtWetWt.Focus();
                        valid = false;
                        break;
                    }
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }


        protected void DrpRefNo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void GrdViewCS_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void TxtQty_Load(object sender, EventArgs e)
        {
        }

        protected void GrdRemark_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GrdRemark_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void LnkbtnFetch_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            if (DrpRefNo.SelectedValue != "-Select-")
            {
                lblMsg.Visible = false;
                TxtRefNo.Text = DrpRefNo.SelectedValue;
                // Session["ReferenceNo"] = TxtRefNo.Text;
                getData();
                getremarkData();
                //var QueryRefNo = dc.RefNo_View(true);
                //List<string> refNoList = new List<string>();
                //foreach (var q in QueryRefNo)
                //{
                //    //  TxtRefNo.Text = q.SOLIDINWD_ReferenceNo_var;
                //    refNoList.Add(q.SOLIDINWD_ReferenceNo_var);
                //}
                //DrpRefNo.DataSource = refNoList;
                //DrpRefNo.DataBind();
                //DrpRefNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select-"));
                //DrpRefNo.Items.Remove(TxtRefNo.Text);
                bindApprovedBy();
                LoadReferenceNoList();
            }
            else
            {
                lblMsg.Text = "Please Select Reference No.";
                lblMsg.Visible = true;
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning", "alert('Please Select Reference No.');", true); 
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (TxtRefNo.Text != "")
            {
                //rpt.SOLID_WA_PDFReport(TxtRefNo.Text, lblEnter.Text);
                rpt.PrintSelectedReport(TxtreportNoSolid.Text, TxtRefNo.Text, lblEnter.Text, "", "", "", "", "", "", "");
            }

        }
        protected void TxtDateOfTest_TextChanged(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            //DateTime dateTest = Convert.ToDateTime(TxtDateOfTest.Text);
            DateTime dateTest = DateTime.ParseExact(TxtDateOfTest.Text, "dd/MM/yyyy", null);
            if (dateTest > System.DateTime.Now)
            {
                lblMsg.Text = "Date Of Testing must be less than or equal to Current Date.";
                lblMsg.Visible = true;
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Date Of Testing must be less than or equal to Current Date..');", true);
            }
            if (TxtDateOfCast.Text != "NA")
            {
                //DateTime datecast = Convert.ToDateTime(TxtDateOfCast.Text);
                DateTime datecast = DateTime.ParseExact(TxtDateOfCast.Text, "dd/MM/yyyy", null);
                if (dateTest < datecast)
                {
                    lblMsg.Text = "Date of Testing must be greater than or equal to Date of Casting.";
                    lblMsg.Visible = true;
                    //   ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Date of Testing must be greater than or equal to Date of Casting.');", true);
                }
            }

        }

        protected void ButtonDeleteRow_Click(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
            int rowIndex = currentRow.RowIndex;
            TextBox TxtRemark = (TextBox)GrdRemark.Rows[rowIndex].Cells[1].FindControl("TxtRemark");
            string remarkname = TxtRemark.Text;

            //var remarkid = from re in dc.tbl_Solid_Remarks where re.Name == remarkname select re.ID;

            //var rm = from remarkdetail in dc.tbl_Solid_Remark_dtls where remarkdetail.SOLIDINWD_Remark_ID == remarkid.FirstOrDefault() && remarkdetail.SOLIDINWD_ReferenceNo_var == TxtRefNo.Text select remarkdetail.ID;

            //int id = Convert.ToInt32(rm.FirstOrDefault());
            //if (id != 0)
            //{
            //    int arraycnt = remarkDelId.Count() + 1;
            //    Array.Resize(ref remarkDelId, arraycnt);
            //    remarkDelId[arraycnt - 1] = id;
            //}

            SetRowData();
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                //int rowIndex = Convert.ToInt32(e.RowIndex);
                if (dt.Rows.Count > 0)
                {
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    drCurrentRow = dt.NewRow();
                    ViewState["CurrentTable"] = dt;
                    GrdRemark.DataSource = dt;
                    GrdRemark.DataBind();

                    SetPreviousData();
                }
                if (dt.Rows.Count == 0)
                {
                    DataTable dt1 = new DataTable();
                    DataRow dr1 = null;
                    dt1.Columns.Add(new DataColumn("RowNumber", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Col2", typeof(string)));
                    dr1 = dt1.NewRow();
                    dr1["RowNumber"] = 1;
                    dr1["Col2"] = string.Empty;
                    dt1.Rows.Add(dr1);
                    ViewState["CurrentTable"] = dt1;
                    GrdRemark.DataSource = dt1;
                    GrdRemark.DataBind();
                }
            }
        }

        protected void ButtonAddNewRow_Click(object sender, EventArgs e)
        {
            AddNewRow();
        }


        protected void ChkboxWitnessBy_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkboxWitnessBy.Checked)
            {
                TxtWitnesBy.Visible = true;
                TxtWitnesBy.Focus();
            }
            else
            {
                TxtWitnesBy.Visible = false;
                TxtWitnesBy.Text = string.Empty;
            }
            //if (ChkboxWitnessBy.Checked == true)
            //{
            //    var witnessBy = from w in dc.tbl_Solid_Inwards where w.SOLIDINWD_ReferenceNo_var == TxtRefNo.Text select w.SOLIDINWD_WitnessBy_var;
            //    string witnesBy = witnessBy.FirstOrDefault();
            //    if (witnesBy != null)
            //    {
            //        TxtWitnesBy.Visible = true;
            //        TxtWitnesBy.Text = witnessBy.FirstOrDefault();
            //        ChkboxWitnessBy.Checked = true;
            //    }
            //    else
            //    {
            //        TxtWitnesBy.Visible = true;
            //        TxtWitnesBy.Text = "";
            //    }
            //}
            //else
            //{
            //    TxtWitnesBy.Text = "";
            //    TxtWitnesBy.Visible = false;
            //}
        }



        protected void BtnExit_Click1(object sender, EventArgs e)
        {
            if (exitFlag.ToString() == "true")
            {
                var result = System.Windows.Forms.MessageBox.Show("Do you really want to Exit?", "Warning", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning, System.Windows.Forms.MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Response.Redirect("Home.aspx");
                }
                else
                {
                    return;
                }

            }
            bool flag = false;

            foreach (GridViewRow row in GrdViewCS.Rows)
            {
                string TxtDry = "", TxtWet = "";
                TxtDry = (row.FindControl("TxtDryWt") as TextBox).Text;
                TxtWet = (row.FindControl("TxtWetWt") as TextBox).Text;


                if (TxtDry == "" || TxtWet == "")
                {
                    flag = true;
                    break;
                }
            }
            if (flag == true)
            {
                exitFlag = "false";
                var exitResult = System.Windows.Forms.MessageBox.Show("Do you really want to Exit", "Warning", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning, System.Windows.Forms.MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
                if (exitResult == System.Windows.Forms.DialogResult.Yes)
                {
                    Response.Redirect("Home.aspx");
                }
                else
                {
                    Response.Redirect("Solid_Masonary_CS.aspx");
                }
            }
            if (exitFlag.ToString() == "false" && flag == false)
            {
                Response.Redirect("Home.aspx");
            }
        }

        protected void TxtDesc_TextChanged(object sender, EventArgs e)
        {
            exitFlag = "true";
        }

        protected void TxtSupplierName_TextChanged(object sender, EventArgs e)
        {
            exitFlag = "true";
        }

        protected void lblIdMark_TextChanged(object sender, EventArgs e)
        {
            exitFlag = "true";
        }

        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {
            clsData objcls = new clsData();
            DataTable dt;
            string mySql = "";
            int rowNo = 0;
            string strReferenceNo = TxtRefNo.Text;
            //strReferenceNo = "3003/1-2";
            #region CS
            mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, wa.* 
                     from new_gt_app_db.dbo.reference_number ref, new_gt_app_db.dbo.category_header chead, 
                     new_gt_app_db.dbo.masonary_water_abs wa
                     where ref.pk_id = chead.reference_no and chead.pk_id = wa.category_header_fk_id
                     and ref.reference_number = '" + strReferenceNo + "' order by wa.quantity_no";

            dt = objcls.getGeneralData(mySql);
            if (dt.Rows.Count > 0)
            {
                if (rowNo == 0)
                {
                    if (dt.Rows[0]["witness_by"] != null || dt.Rows[0]["witness_by"].ToString() != "")
                    {
                        TxtWitnesBy.Text = dt.Rows[0]["witness_by"].ToString();
                        ChkboxWitnessBy.Checked = true;
                    }
                    TxtDateOfTest.Text = Convert.ToDateTime(dt.Rows[0]["date_of_testing"]).ToString("dd/MM/yyyy");
                }
                TxtQty.Text = dt.Rows.Count.ToString();
                DataTable dt2 = new DataTable();
                DataColumn dtcolumn = new DataColumn();
                for (int i = 0; i < Convert.ToInt32(TxtQty.Text); i++)
                {
                    if (dt2.Columns.Count == 0)
                    {
                        dt2.Columns.Add("Sr. No.", typeof(string));
                        dt2.Columns.Add("ID Mark", typeof(string));
                        dt2.Columns.Add("Dry Weight", typeof(string));
                        dt2.Columns.Add("Wet Weight", typeof(string));
                        dt2.Columns.Add("Water Absorption ", typeof(string));
                        dt2.Columns.Add("Avg. Water Absorption %", typeof(string));
                        dt2.Columns.Add("ID", typeof(string));
                    }
                    DataRow NewRow = dt2.NewRow();
                    dt2.Rows.Add(NewRow);
                }
                GrdViewCS.DataSource = dt2;
                GrdViewCS.DataBind();
                //Display data in Grid
                for (int cnt = 0; cnt < GrdViewCS.Rows.Count; cnt++)
                {
                    TextBox TxtIdMark = (TextBox)GrdViewCS.Rows[cnt].Cells[1].FindControl("lblIdMark");
                    TextBox TxtDryWt = (TextBox)GrdViewCS.Rows[cnt].Cells[2].FindControl("TxtDryWt");
                    TextBox TxtWetWt = (TextBox)GrdViewCS.Rows[cnt].Cells[3].FindControl("TxtWetWt");
                    
                    TxtIdMark.Text = dt.Rows[cnt]["id_mark"].ToString();
                    TxtDryWt.Text = Math.Round(Convert.ToDecimal(dt.Rows[cnt]["dry_wt"].ToString())).ToString();
                    TxtWetWt.Text = Math.Round(Convert.ToDecimal(dt.Rows[cnt]["wet_wt"].ToString())).ToString();

                    exitFlag = "true";
                    waterAbsCal(GrdViewCS.Rows[cnt]);
                }
            }
            dt.Dispose();
            #endregion

            objcls = null;
        }
    }
}