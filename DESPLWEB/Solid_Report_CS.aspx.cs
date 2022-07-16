using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace DESPLWEB
{
    public partial class Solid_Masonary_CS : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        static int[] remarkDelId = new int[0];
        static int[] remarkEditId = new int[0];
        static string exitFlag;
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
                if (TxtreportNoSolid.Text != "")
                {
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
                    var solidHeader = dc.Solid_Inward_View("", 1, "SOLID");
                    foreach (var h in solidHeader)
                    {
                        lblheading.Text = h.TEST_Name_var.ToString();
                    }
                    LoadReferenceNoList();
                }
            }
        }
        private void LoadReferenceNoList()
        {
            byte reportStatus = 0;
            if (lblEnter.Text == "Enter")
                reportStatus = 1;
            else if (lblEnter.Text == "Check")
                reportStatus = 2;

            var reportList = dc.ReferenceNo_View_StatusWise("SOLID", reportStatus,Convert.ToInt32(lblTestId.Text) );
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
            var QueryRefNo = dc.RefNo_View(false);
            List<string> refNoList = new List<string>();
            foreach (var q in QueryRefNo)
            {
                refNoList.Add(q.SOLIDINWD_ReferenceNo_var);
            }
            DrpRefNo.DataSource = refNoList;
            DrpRefNo.DataBind();
            DrpRefNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select-"));
            DrpRefNo.Items.Remove(TxtRefNo.Text);

            Label lblheading = (Label)Master.FindControl("lblheading");            
            var solidHeader = dc.Solid_Inward_View("", 1, "SOLID");
            foreach (var h in solidHeader)
            {
                lblheading.Text = h.TEST_Name_var.ToString();
            }
        }
        public void getData()
        {
            #region Display Solid Massonary CS data                
            var queryData = dc.Solid_Inward_View(Convert.ToString(TxtRefNo.Text), 0, "SOLID");
            DateTime datecast = DateTime.Now, dateTest = DateTime.Now; string idMark = "";
            foreach (var data in queryData)
            {
                TxtRefNo.Text = data.SOLIDINWD_ReferenceNo_var.ToString();
                TxtDesc.Text = data.SOLIDINWD_Description_var;
                TxtSupplierName.Text = data.SOLIDINWD_SupplierName_var;
                TxtReportNo.Text = data.SOLIDINWD_SetOfRecord_var;
                lblTestId.Text = data.SOLIDINWD_TEST_Id.ToString();
                
                if (ddl_NablScope.Items.FindByValue(data.SOLIDINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(data.SOLIDINWD_NablScope_var);
                }
                if (Convert.ToString(data.SOLIDINWD_NablLocation_int) != null && Convert.ToString(data.SOLIDINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(data.SOLIDINWD_NablLocation_int);
                }

                if (data.SOLIDINWD_CastingDate_nvar == null)
                    TxtDateOfCast.Text = DateTime.Now.ToString("dd/MM/yyyy");
                else
                    TxtDateOfCast.Text = data.SOLIDINWD_CastingDate_nvar;
                if (TxtDateOfCast.Text != "NA")
                {
                    datecast = DateTime.ParseExact(TxtDateOfCast.Text, "dd/MM/yyyy", null);
                }
                if (data.SOLIDINWD_TestingDate_dt == null)
                    TxtDateOfTest.Text = DateTime.Now.ToString("dd/MM/yyyy");
                else
                    TxtDateOfTest.Text = Convert.ToDateTime(data.SOLIDINWD_TestingDate_dt).ToString("dd/MM/yyyy");
                if (TxtDateOfTest.Text == "" || lblEnter.Text == "Enter")
                {
                    TxtDateOfTest.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                dateTest = DateTime.ParseExact(TxtDateOfTest.Text, "dd/MM/yyyy", null);
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
                    dt.Columns.Add("Age", typeof(string));

                    dt.Columns.Add("Length", typeof(string));
                    dt.Columns.Add("Width", typeof(string));
                    dt.Columns.Add("Load", typeof(string));

                    dt.Columns.Add("Area", typeof(string));
                    dt.Columns.Add("Strength", typeof(string));
                    dt.Columns.Add("Average", typeof(string));
                    dt.Columns.Add("ID", typeof(string));
                }
                DataRow NewRow = dt.NewRow();
                dt.Rows.Add(NewRow);

                GrdViewCS.DataSource = dt;
                GrdViewCS.DataBind();
            }

            //display Age
            string age = "";
            if (TxtDateOfCast.Text != "NA")
            {
                if (dateTest >= datecast)
                {
                    string agediff = (dateTest - datecast).TotalDays.ToString();
                    age = agediff;

                }
                else
                {
                    age = "0";
                }
            }
            else if (datecast == null || TxtDateOfCast.Text == "NA")
            {
                age = "NA";
            }

            for (int i = 0; i < GrdViewCS.Rows.Count; i++)
            {
                TextBox LblAge = (TextBox)GrdViewCS.Rows[i].Cells[2].FindControl("LblAge");
                LblAge.Text = age;
            }

            //Display data in Grid
            var list = dc.Solid_Inward_CS_View(TxtRefNo.Text);

            int g = 0;
            foreach (var grdData in list)
            {
                TextBox TxtIdMark = (TextBox)GrdViewCS.Rows[g].Cells[1].FindControl("lblIdMark");
                TextBox TxtLength = (TextBox)GrdViewCS.Rows[g].Cells[3].FindControl("TxtLength");
                TextBox TxtWidth = (TextBox)GrdViewCS.Rows[g].Cells[4].FindControl("TxtWidth");
                TextBox TxtLoad = (TextBox)GrdViewCS.Rows[g].Cells[5].FindControl("TxtLoad");
                TextBox TxtArea = (TextBox)GrdViewCS.Rows[g].Cells[6].FindControl("TxtArea");
                TextBox TxtStrength = (TextBox)GrdViewCS.Rows[g].Cells[7].FindControl("TxtStrength");
                TextBox TxtAvg = (TextBox)GrdViewCS.Rows[qty / 2].Cells[8].FindControl("TxtAvg");
                Label LblID = (Label)GrdViewCS.Rows[g].Cells[9].FindControl("LblID");

                LblID.Text = grdData.ID.ToString();
                TxtIdMark.Text = grdData.SOLIDINWD_CS_ID_Mark_var;
                TxtLength.Text = grdData.SOLIDINWD_CS_Length_dec.ToString();
                TxtWidth.Text = grdData.SOLIDINWD_CS_width_dec.ToString();
                TxtLoad.Text = grdData.SOLIDINWD_CS_Load_Dec.ToString();
                TxtArea.Text = grdData.SOLIDINWD_CS_Area_int.ToString();
                TxtStrength.Text = grdData.SOLIDINWD_CS_Strength_Dec.ToString();
                decimal avg = Convert.ToDecimal(grdData.SOLIDINWD_CS_Average_Dec);
                if (qty < 8)
                {
                    TxtAvg.Text = "***";
                }
                else
                {
                    TxtAvg.Text = avg.ToString();
                }
                int approved_By = Convert.ToInt32(grdData.SOLIDINWD_CS_Approved_By);
                //var approvedUser = from u in dc.tbl_Users where u.USER_Id == approved_By select u.USER_Name_var;
                //string appby = approvedUser.FirstOrDefault();
                //DrpApprovedBy.SelectedValue = approvedUser.FirstOrDefault();

                g++;
            }

            var listIdmark = dc.Solid_Inward_CS_View(TxtRefNo.Text);
            if (listIdmark.Count() == 0)
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
            //    TextBox TxtLength = (TextBox)GrdViewCS.Rows[j].Cells[3].FindControl("TxtLength");
            //    TextBox TxtWidth = (TextBox)GrdViewCS.Rows[j].Cells[4].FindControl("TxtWidth");
            //    TextBox TxtArea = (TextBox)GrdViewCS.Rows[j].Cells[5].FindControl("TxtArea");

            //    if(TxtLength.Text == "" || TxtWidth.Text == "" || TxtArea.Text == "")
            //    {
            //        printFlag = true;
            //        break;
            //    }                  
            //}
            //if(printFlag == true)
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
        protected void LnkbtnFetch_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            if (DrpRefNo.SelectedValue != "-Select-")
            {
                lblMsg.Visible = false;
                TxtRefNo.Text = DrpRefNo.SelectedValue;
                TxtRefNo.Text = TxtRefNo.Text;
                getData();
                getremarkData();
                bindApprovedBy();
                LoadReferenceNoList();
            }
            else
            {
                lblMsg.Text = "Please Select Reference No.";
                lblMsg.Visible = true;
            }
        }
        protected void TxtDateOfTest_TextChanged(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            //var queryData = from d in dc.tbl_Solid_Inwards where d.SOLIDINWD_ReferenceNo_var==TxtRefNo.Text select d;
            var queryDat = dc.Solid_Inward_View(TxtRefNo.Text, 0, "SOLID");
            foreach (var data in queryDat)
            {
                if (TxtDateOfCast.Text != "NA")
                {
                    //DateTime datecast = Convert.ToDateTime(queryData.FirstOrDefault().SOLIDINWD_CastingDate_nvar);
                    //DateTime dateTest = Convert.ToDateTime(queryData.FirstOrDefault().SOLIDINWD_TestingDate_dt);
                    //DateTime datecast = Convert.ToDateTime(data.SOLIDINWD_CastingDate_nvar);
                    DateTime datecast = DateTime.ParseExact(data.SOLIDINWD_CastingDate_nvar, "dd/MM/yyyy", null);
                    DateTime dateTest = Convert.ToDateTime(data.SOLIDINWD_TestingDate_dt);
                    //dateTest = Convert.ToDateTime(TxtDateOfTest.Text);
                    dateTest = DateTime.ParseExact(TxtDateOfTest.Text, "dd/MM/yyyy", null);
                    if (dateTest <= System.DateTime.Now)
                    {
                        string age = "";
                        if (dateTest >= datecast)
                        {
                            string diff2 = (dateTest - datecast).TotalDays.ToString();
                            age = diff2;
                        }
                        else
                        {
                            age = "0";
                            lblMsg.Text = "Date of Testing must be greater than or equal to Date of Casting";
                            lblMsg.Visible = true;
                            // ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Date of Testing must be greater than or equal to Date of Casting.');", true);
                        }

                        if (datecast == null)
                        {
                            age = "NA";
                        }
                        //if (TxtDateOfCast.Text == "NA")
                        //{
                        //    age = "NA";
                        //}
                        for (int i = 0; i < GrdViewCS.Rows.Count; i++)
                        {
                            TextBox LblAge = (TextBox)GrdViewCS.Rows[i].Cells[2].FindControl("LblAge");
                            LblAge.Text = age;
                        }
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Date Of Testing must be less than or equal to Current Date.";
                    }
                }
                else
                {
                    for (int i = 0; i < GrdViewCS.Rows.Count; i++)
                    {
                        TextBox LblAge = (TextBox)GrdViewCS.Rows[i].Cells[2].FindControl("LblAge");
                        LblAge.Text = "NA";
                    }
                }
            }
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
            //var witnessBy = from w in dc.tbl_Solid_Inwards where w.SOLIDINWD_ReferenceNo_var == TxtRefNo.Text select w.SOLIDINWD_WitnessBy_var;
            //string witnesBy = witnessBy.FirstOrDefault();

            //var ct = dc.ReportStatus_View("Masonary Block Testing", null, null, 1, 0, 0, TxtRefNo.Text, 0, 0, 0);
            //foreach (var c in ct)
            //{
            //    if (c.SOLIDINWD_WitnessBy_var != null)
            //    {
            //        TxtWitnesBy.Visible = true;
            //        TxtWitnesBy.Text = c.SOLIDINWD_WitnessBy_var.FirstOrDefault().ToString();
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
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                string length = "", width = "", area = "", load = "", strength = "", average = "", age = "", IdMark = "", approved_by = "", checked_by = "";
                int approvedBy = 0, checkedBy = 0, Pk_ID = 0;
                decimal totalAvr = 0;
                //approved_by = DrpApprovedBy.SelectedValue;
                //checked_by = TxtcheckBy.Text;

                //var approved = from a in dc.tbl_Users where a.USER_Name_var == approved_by select a.USER_Id;
                //approvedBy = approved.FirstOrDefault();
                //var checkd = from c in dc.tbl_Users where c.USER_Name_var == checked_by select c.USER_Id;
                //checkedBy = checkd.FirstOrDefault();

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

                var referenceno = dc.Solid_Inward_CS_View(TxtRefNo.Text).ToList();

                if (referenceno.Count() == 0)
                {
                    int rowcount = 0, cnt = 0;
                    foreach (GridViewRow row in GrdViewCS.Rows)
                    {
                        length = (row.FindControl("TxtLength") as TextBox).Text;
                        width = (row.FindControl("TxtWidth") as TextBox).Text;
                        area = (row.FindControl("TxtArea") as TextBox).Text;
                        load = (row.FindControl("TxtLoad") as TextBox).Text;
                        strength = (row.FindControl("TxtStrength") as TextBox).Text;
                        int qty = Convert.ToInt32(TxtQty.Text);

                        if (rowcount == qty - 1)
                        {
                            average = ((TextBox)GrdViewCS.Rows[qty / 2].Cells[8].FindControl("TxtAvg")).Text;
                            if (average == "***" || average == "")
                            {
                                average = "0.00";
                                totalAvr = Convert.ToDecimal(average);
                            }
                            else
                            {
                                totalAvr = Convert.ToDecimal(average);
                            }
                        }

                        age = (row.FindControl("LblAge") as TextBox).Text;

                        IdMark = (row.FindControl("lblIdMark") as TextBox).Text;

                        //sp
                        DateTime dateTest = DateTime.ParseExact(TxtDateOfTest.Text, "dd/MM/yyyy", null);
                        dc.Solid_Inword_CS_Update(null, TxtRefNo.Text, TxtReportNo.Text, TxtDesc.Text, TxtSupplierName.Text, IdMark, age, Convert.ToInt32(length), Convert.ToInt32(width), Convert.ToInt32(area), Convert.ToDecimal(load), Convert.ToDecimal(strength), totalAvr, TxtDateOfCast.Text, dateTest, Convert.ToInt32(TxtQty.Text), approvedBy, checkedBy, false);
                        // dc.SubmitChanges();
                        rowcount++;

                        var remarkcount = referenceno.Count();
                        if (remarkcount != 0)
                        {
                            Label LblID = (row.FindControl("LblID") as Label);
                            string Ids = referenceno[cnt].ID.ToString();
                            LblID.Text = Ids;
                            cnt++;
                        }
                        //sp end
                    }
                }
                else
                {
                    int i = 0;
                    int rowcount = 0;
                    foreach (GridViewRow row in GrdViewCS.Rows)
                    {

                        length = (row.FindControl("TxtLength") as TextBox).Text;
                        width = (row.FindControl("TxtWidth") as TextBox).Text;
                        area = (row.FindControl("TxtArea") as TextBox).Text;
                        load = (row.FindControl("TxtLoad") as TextBox).Text;
                        strength = (row.FindControl("TxtStrength") as TextBox).Text;
                        Pk_ID = Convert.ToInt32((row.FindControl("LblID") as Label).Text);//sp

                        int qty = Convert.ToInt32(TxtQty.Text);
                        if (rowcount == qty - 1)
                        {
                            average = ((TextBox)GrdViewCS.Rows[qty / 2].Cells[8].FindControl("TxtAvg")).Text;

                            if (average == "***" || average == "")
                            {
                                average = "0.00";
                                totalAvr = Convert.ToDecimal(average);
                            }
                            else { totalAvr = Convert.ToDecimal(average); }
                        }

                        age = (row.FindControl("LblAge") as TextBox).Text;
                        IdMark = (row.FindControl("lblIdMark") as TextBox).Text;

                        dc.Solid_Inword_CS_Update(Pk_ID, TxtRefNo.Text, TxtReportNo.Text, TxtDesc.Text, TxtSupplierName.Text, IdMark, age, Convert.ToDecimal(length), Convert.ToDecimal(width), Convert.ToInt32(area), Convert.ToDecimal(load), Convert.ToDecimal(strength), totalAvr, TxtDateOfCast.Text, TestingDt, Convert.ToInt32(TxtQty.Text), approvedBy, checkedBy, true);
                        // dc.SubmitChanges();
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
                dc.Solid_Inword_Update(TxtRefNo.Text, TxtDesc.Text, TxtSupplierName.Text, wtnesBy, TestingDt, TxtReportNo.Text, Convert.ToDecimal(average), TxtDateOfCast.Text, Convert.ToInt32(TxtQty.Text), WitnessFlag);          // approvedBy, checkedBy     
                //  dc.SubmitChanges();
                #endregion

                #region insert data in remark table

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

                #endregion

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

                // Session["Edit"] = null;            
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Report Saved Successfully');", true);
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkPrint.Visible = true;
                lnkSave.Enabled = false;

                //fetch PK ID
                var list = dc.Solid_Inward_CS_View(TxtRefNo.Text);
                int g = 0;
                foreach (var grdData in list)
                {
                    Label LblID = (Label)GrdViewCS.Rows[g].Cells[9].FindControl("LblID");
                    LblID.Text = grdData.ID.ToString();
                    g++;
                }
            }
        }
        public string compl_notes(string average, string age)
        {
            #region comp

            int flgCompl = 0, Age = 0;
            string tGrade = "", Complaince_Note = ""; ;
            double avg = 0;
            if (average != "***")
            {

                avg = Convert.ToDouble(average);
            }

            if (age != "NA")
            {

                Age = Convert.ToInt32(age);
            }
            if (average != "***" && Age >= 28)
            {
                if (avg >= 3.5)
                {
                    flgCompl = 1;

                    if (avg >= 15)
                    {

                        tGrade = "A(15.0)";
                    }
                    else if (avg >= 12.5)
                    {

                        tGrade = "A(12.5)";
                    }
                    else if (avg >= 10)
                    {

                        tGrade = "A(10)";
                    }
                    else if (avg >= 8.5)
                    {

                        tGrade = "A(8.5)";
                    }
                    else if (avg >= 7)
                    {

                        tGrade = "A(7.0)";
                    }
                    else if (avg >= 5.5)
                    {

                        tGrade = "A(5.5)";
                    }

                    else if (avg >= 4.5)
                    {

                        tGrade = "A(4.5)";
                    }
                    else if (avg >= 3.5)
                    {

                        tGrade = "A(3.5)";
                    }
                    else
                    {
                        tGrade = "";
                        flgCompl = 3;
                    }
                }
            }
            else
            {
                flgCompl = 3;
            }


            if (flgCompl > 0)
            {
                if (flgCompl == 1)
                {
                    Complaince_Note = "Tested sample confirms to the " + tGrade + " grade of  Masonary Units.";
                }

                if (flgCompl == 3)
                {
                    Complaince_Note = "Tested sample does not comply to any grade of Masonary Units";
                }
            }


            #endregion

            return Complaince_Note;
        }
        protected Boolean ValidateData()
        {


            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            lblMsg.ForeColor = System.Drawing.Color.Red;
            DateTime datecast = new DateTime();
            DateTime dateTest = DateTime.ParseExact(TxtDateOfTest.Text, "dd/MM/yyyy", null);
            // DateTime dateTest = Convert.ToDateTime(TxtDateOfTest.Text);
            if (TxtDateOfCast.Text != "NA")
            {
                datecast = DateTime.ParseExact(TxtDateOfCast.Text, "dd/MM/yyyy", null);
            }
            // Witness by validation

            //date validation             
            if (dateTest > System.DateTime.Now)
            {
                lblMsg.Text = "Date Of Testing must be less than or equal to Current Date.";
                TxtDateOfTest.Focus();
                valid = false;
            }
            else if (TxtDateOfCast.Text != "NA" && dateTest < datecast)
            {
                lblMsg.Text = "Date of Testing must be greater than or equal to Date of Casting";
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
            else if (ddl_NABLLocation.SelectedItem.Text == "--Select--")
            {
                lblMsg.Text = "Select 'NABL Location'.";
                valid = false;
                ddl_NABLLocation.Focus();
            }
            else if (ChkboxWitnessBy.Checked == true && TxtWitnesBy.Text == "")
            {
                lblMsg.Text = "Please Enter Witness By Name.";
                TxtWitnesBy.Focus();
                valid = false;
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
                    TextBox txtlength = (TextBox)GrdViewCS.Rows[i].Cells[3].FindControl("TxtLength");
                    TextBox txtwidth = (TextBox)GrdViewCS.Rows[i].Cells[4].FindControl("TxtWidth");
                    TextBox txtLoad = (TextBox)GrdViewCS.Rows[i].Cells[5].FindControl("TxtLoad");
                    if (lblIdMark.Text == "")
                    {
                        lblMsg.Text = "Enter Id Mark for row no " + (i + 1) + ".";
                        lblIdMark.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtlength.Text == "")
                    {
                        lblMsg.Text = "Enter Length for row number " + (i + 1) + ".";
                        txtlength.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtwidth.Text == "")
                    {
                        lblMsg.Text = "Enter Width for row number " + (i + 1) + ".";
                        txtwidth.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtLoad.Text == "")
                    {
                        lblMsg.Text = "Enter Load for row number " + (i + 1) + ".";
                        txtLoad.Focus();
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
        protected void TxtLength_TextChanged(object sender, EventArgs e)
        {
            exitFlag = "true";
            GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
            areacal(currentRow);

        }
        protected void TxtWidth_TextChanged(object sender, EventArgs e)
        {
            exitFlag = "true";
            GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
            areacal(currentRow);
        }
        public void areacal(GridViewRow currentRow)
        {

            try
            {
                TextBox txtLength = (TextBox)currentRow.FindControl("TxtLength");
                TextBox txtWidth = (TextBox)currentRow.FindControl("TxtWidth");
                int length = 0, width = 0;
                if (txtLength.Text != "" && txtWidth.Text != "")
                {
                    length = Convert.ToInt32(txtLength.Text);
                    width = Convert.ToInt32(txtWidth.Text);
                    int area = (length * width);

                    TextBox txtArea = (TextBox)currentRow.FindControl("TxtArea");
                    txtArea.Text = area.ToString();

                    //change strength            
                    TextBox txtLoad = (TextBox)currentRow.FindControl("TxtLoad");
                    decimal load = Math.Round(Convert.ToDecimal(txtLoad.Text), 1);
                    if (txtLoad.Text != "")
                    {
                        TextBox txtStrength = (TextBox)currentRow.FindControl("TxtStrength");

                        if (area != 0)
                        {
                            decimal strength = Math.Round((load / area) * 1000, 2);
                            txtStrength.Text = strength.ToString();
                        }
                        else
                        {
                            txtStrength.Text = "0.00";
                        }
                    }
                    //calculate average
                    decimal total = 0, avg = 0; int rowIndex = 0;
                    int qty = Convert.ToInt32(TxtQty.Text);
                    foreach (GridViewRow row in GrdViewCS.Rows)
                    {
                        rowIndex = row.RowIndex + 1;
                        var txtstrength = row.FindControl("TxtStrength") as TextBox;
                        decimal number;
                        if (decimal.TryParse(txtstrength.Text, out number))
                        {
                            total += number;

                        }

                    }

                    string emptyRemark = ((TextBox)GrdViewCS.Rows[qty - 1].Cells[5].FindControl("TxtLoad")).Text;
                    if (qty >= 8)
                    {
                        avg = total / qty;
                        //decimal average = Math.Round(avg, 2);                    
                        int rows = qty / 2;
                        if (emptyRemark != "")
                        {
                            TextBox TxtAvg = (TextBox)GrdViewCS.Rows[rows].Cells[8].FindControl("TxtAvg");
                            //TxtAvg.Text = average.ToString();
                            TxtAvg.Text = avg.ToString("0.00");
                        }
                    }
                    else
                    {
                        if (qty < 8)
                        {
                            if (emptyRemark != "")
                            {
                                TextBox TxtAvg = (TextBox)GrdViewCS.Rows[qty / 2].Cells[8].FindControl("TxtAvg");
                                TxtAvg.Text = "***";
                            }

                        }
                    }
                }


            }
            catch
            { }


        }
        protected void TxtLoad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                exitFlag = "true";
                int qty = Convert.ToInt32(TxtQty.Text);
                GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
                TextBox txtLoad = (TextBox)currentRow.FindControl("TxtLoad");
                decimal load = Math.Round(Convert.ToDecimal(txtLoad.Text), 1);

                TextBox txtArea = (TextBox)currentRow.FindControl("TxtArea");
                int area = Convert.ToInt32(txtArea.Text);
                TextBox txtStrength = (TextBox)currentRow.FindControl("TxtStrength");

                if (area != 0)
                {
                    decimal strength = Math.Round((load / area) * 1000, 2);
                    txtStrength.Text = strength.ToString();
                }
                else
                {
                    txtStrength.Text = "0.00";
                }

                decimal total = 0, avg = 0; int rowIndex = 0;

                foreach (GridViewRow row in GrdViewCS.Rows)
                {
                    rowIndex = row.RowIndex + 1;
                    var txtstrength = row.FindControl("TxtStrength") as TextBox;
                    decimal number;
                    if (decimal.TryParse(txtstrength.Text, out number))
                    {
                        total += number;

                    }

                }
                string emptyRemark = ((TextBox)GrdViewCS.Rows[qty - 1].Cells[5].FindControl("TxtLoad")).Text;
                if (qty >= 8)
                {
                    avg = total / qty;
                    //decimal average = Math.Round(avg, 2);                    
                    int rows = qty / 2;
                    if (emptyRemark != "")
                    {
                        TextBox TxtAvg = (TextBox)GrdViewCS.Rows[rows].Cells[8].FindControl("TxtAvg");
                        //TxtAvg.Text = average.ToString();
                        TxtAvg.Text = avg.ToString("0.00");
                    }
                }
                else
                {
                    if (qty < 8)
                    {
                        if (emptyRemark != "")
                        {
                            TextBox TxtAvg = (TextBox)GrdViewCS.Rows[qty / 2].Cells[8].FindControl("TxtAvg");
                            TxtAvg.Text = "***";
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }

        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {

            PrintPDFReport rpt = new PrintPDFReport();
            if (TxtRefNo.Text != "")
            {
                //rpt.SOLID_CS_PDFReport(TxtRefNo.Text, lblEnter.Text);
                rpt.PrintSelectedReport(TxtreportNoSolid.Text, TxtRefNo.Text, lblEnter.Text, "", "", "", "", "", "", "");
            }
            //#region for RefNo
            //try
            //{

            //    if (ValidateData() == true)
            //    {
            //        #region for RefNo
            //        string[] RefNo = TxtRefNo.Text.Split('/');
            //        string refNo = RefNo[0];
            //        int ref_No = Convert.ToInt32(refNo);
            //        #endregion
            //        #region fetch data from db
            //        //var clientdata = from c in dc.tbl_Inwards where (c.INWD_ReferenceNo_int == ref_No) select c;
            //        //var solidInward = from s in dc.tbl_Solid_Inwards where (s.SOLIDINWD_ReferenceNo_var == TxtRefNo.Text) select s;
            //        //var client_ID = from cust in dc.tbl_Clients where (cust.CL_Id == clientdata.FirstOrDefault().INWD_CL_Id) select cust;
            //        //var siteData = from site in dc.tbl_Sites where (site.SITE_Id == clientdata.FirstOrDefault().INWD_SITE_Id) select site;

            //        Paragraph paragraph = new Paragraph();
            //        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            //        var fileName = "Solid_MasonaryCS-" + RefNo[0] + "-" + RefNo[1] + " " + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".pdf";
            //        string foldername = "Veena";
            //        if (!Directory.Exists(@"D:\" + foldername))
            //            Directory.CreateDirectory(@"D:/" + foldername);

            //        string Subfoldername = foldername + "/Solid Masonry";
            //        if (!Directory.Exists(@"D:\" + Subfoldername))
            //            Directory.CreateDirectory(@"D:/" + Subfoldername);

            //        string Subfoldername1 = Subfoldername + "/CS";

            //        if (!Directory.Exists(@"D:\" + Subfoldername1))
            //            Directory.CreateDirectory(@"D:/" + Subfoldername1);

            //        PdfWriter.GetInstance(pdfDoc, new FileStream(@"D:/" + Subfoldername1 + "/" + fileName, FileMode.Create));
            //        pdfDoc.Open();


            //        PdfPTable table1 = new PdfPTable(9);
            //        table1.WidthPercentage = 100;
            //        pdfDoc.Open();
            //        PdfPTable MaindataTable = new PdfPTable(4);
            //        MaindataTable.WidthPercentage = 100;
            //        MaindataTable.DefaultCell.Border = PdfPCell.NO_BORDER;
            //        paragraph.Alignment = Element.ALIGN_CENTER;
            //        paragraph.Add("Test Report");
            //        pdfDoc.Add(paragraph);


            //        paragraph = new Paragraph();
            //        Font fontTitle = new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD);
            //        Font fontH1 = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.UNDEFINED);
            //        Font fontH2 = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD);
            //        Font fontH3 = new Font(Font.FontFamily.TIMES_ROMAN, 7, Font.UNDEFINED);
            //        #endregion
            //        string Witnessby = string.Empty;
            //        DateTime ApproveDt = new DateTime();
            //        int TestId = 0;
            //        #region data
            //        var SolidInward = dc.ReportStatus_View("Masonary Block Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.TxtRefNo.Text), 0, 2, 0);
            //        foreach (var solid in SolidInward)
            //            {

            //                paragraph = new Paragraph();
            //                paragraph.Alignment = Element.ALIGN_CENTER;
            //                paragraph.Add("Masonry Unit(Compressive Strength)");
            //                if (solid.SOLIDINWD_Status_tint != 6)
            //                {
            //                    paragraph.SpacingAfter = 20;
            //                }
            //                pdfDoc.Add(paragraph);
            //                if (solid.SOLIDINWD_Status_tint == 6)
            //                {
            //                    var blackListText = FontFactory.GetFont("italic", 8);
            //                    paragraph = new Paragraph();
            //                    paragraph.Alignment = Element.ALIGN_CENTER;
            //                    paragraph.Font = blackListText;
            //                    paragraph.Add("DUPLICATE COPY");
            //                    paragraph.SpacingAfter = 20;
            //                    pdfDoc.Add(paragraph);
            //                }

            //                float[] widths = new float[] { 15f, 55f, 15f, 15f };
            //                MaindataTable.SetWidths(widths);
            //                PdfPCell Cust_Namecell = new PdfPCell(new Phrase("Customer Name", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase(": -" + solid.CL_Name_var, fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("Date of Issue", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                if (solid.SOLIDINWD_IssueDate_dt != null)
            //                {
            //                    DateTime iDT = solid.SOLIDINWD_IssueDate_dt.Value;
            //                    string issuedt = iDT.ToString("dd-MMM-yyyy");

            //                    Cust_Namecell = new PdfPCell(new Phrase(": " + issuedt, fontH1));
            //                }
            //                else
            //                {
            //                    Cust_Namecell = new PdfPCell(new Phrase(": -", fontH1));
            //                }
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("Office address", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase(": " + solid.CL_OfficeAddress_var, fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("Record No.", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase(": SOLID " + TxtReportNo.Text, fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("Sample Ref No.", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase(": SOLID -" + TxtRefNo.Text, fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("Site Name", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase(": " + solid.SITE_Name_var, fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("Bill No", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase(": " + Convert.ToString(solid.INWD_BILL_Id), fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("Description", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase(": " + solid.SOLIDINWD_Description_var, fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("Date of receipt", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                if (solid.INWD_ReceivedDate_dt != null)
            //                {
            //                    DateTime rDT = solid.INWD_ReceivedDate_dt.Value;
            //                    string receiptDt = rDT.ToString("dd-MMM-yyyy");
            //                    Cust_Namecell = new PdfPCell(new Phrase(": " + receiptDt, fontH1));
            //                }
            //                else
            //                {
            //                    Cust_Namecell = new PdfPCell(new Phrase(": -"));
            //                }
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("Supplier Name", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase(": " + solid.SOLIDINWD_SupplierName_var, fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("Date of casting", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                if (solid.SOLIDINWD_CastingDate_nvar != null && solid.SOLIDINWD_CastingDate_nvar != "NA")
            //                {
            //                    DateTime cDT = Convert.ToDateTime(solid.SOLIDINWD_CastingDate_nvar);
            //                    string CastingDt = cDT.ToString("dd-MMM-yyyy");
            //                    Cust_Namecell = new PdfPCell(new Phrase(": " + CastingDt, fontH1));
            //                }
            //                else
            //                {
            //                    Cust_Namecell = new PdfPCell(new Phrase(": NA", fontH1));
            //                }
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("Type of Masonry Block", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase(": " + solid.SOLIDINWD_BlockType_var, fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                Cust_Namecell = new PdfPCell(new Phrase("Date of testing", fontH1));
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
            //                MaindataTable.AddCell(Cust_Namecell);
            //                if (solid.SOLIDINWD_TestingDate_dt != null)
            //                {
            //                    DateTime tDT = Convert.ToDateTime(solid.SOLIDINWD_TestingDate_dt);
            //                    string testingDt = tDT.ToString("dd-MMM-yyyy");
            //                    Cust_Namecell = new PdfPCell(new Phrase(": " + testingDt, fontH1));
            //                }
            //                else
            //                {
            //                    Cust_Namecell = new PdfPCell(new Phrase(": -"));
            //                }
            //                Cust_Namecell.Border = PdfPCell.NO_BORDER;

            //                MaindataTable.AddCell(Cust_Namecell);
            //                Witnessby = solid.SOLIDINWD_WitnessBy_var.ToString();
            //                TestId =Convert.ToInt32(solid.SOLIDINWD_TEST_Id);
            //            }
            //        pdfDoc.Add(MaindataTable);
            //        #endregion

            //        var solidInward_CA = dc.Solid_Inward_CS_View(TxtRefNo.Text).ToList();
            //        var count = solidInward_CA.Count();
            //        int i = 0;

            //        table1.SpacingBefore = 10;
            //        pdfDoc.Add(new Paragraph("OBSERVATIONS & CALCULATIONS", fontH2));
            //        table1.SpacingBefore = 5;
            //        string[] headers = { "Sr.No", "ID Mark", "Age", "Dimensions", "Cross Section Area", "Load", "Compressive strength", "Average strength" };
            //        PdfPCell cell1;
            //        for (int h = 0; h < headers.Count(); h++)
            //        {
            //            if (h < 2)
            //            {
            //                cell1 = new PdfPCell(new Phrase(headers[h], fontH1));
            //                cell1.Rowspan = 2;
            //            }
            //            else
            //            {
            //                if (h == 3)
            //                {
            //                    cell1 = new PdfPCell(new Phrase(headers[h], fontH1));
            //                    cell1.Colspan = 2;

            //                }
            //                else
            //                {
            //                    cell1 = new PdfPCell(new Phrase(headers[h], fontH1));
            //                }
            //            }
            //            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //            table1.AddCell(cell1);
            //        }
            //        table1.HorizontalAlignment = Element.ALIGN_LEFT;

            //        string[] subheaders = { "", "", "(days)", "Length", "Width", "(mm²)", "(kN)", "(N/mm²)", "(N/mm²)" };
            //        cell1 = new PdfPCell();
            //        for (int h = 2; h < subheaders.Count(); h++)
            //        {
            //            cell1 = new PdfPCell(new Phrase(subheaders[h], fontH1));
            //            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //            table1.AddCell(cell1);
            //        }
            //        table1.HorizontalAlignment = Element.ALIGN_LEFT;
            //        table1.WidthPercentage = 100;
            //        pdfDoc.Add(table1);



            //        #region tbldata
            //        table1 = new PdfPTable(9);
            //        table1.HorizontalAlignment = Element.ALIGN_LEFT;
            //        table1.WidthPercentage = 100;
            //        for (int j = 0; j < count; j++)
            //        {

            //            string srNo = Convert.ToString(i + 1);
            //            cell1 = new PdfPCell(new Phrase(srNo, fontH1));
            //            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //            table1.AddCell(cell1);
            //            if (solidInward_CA[j].SOLIDINWD_CS_ID_Mark_var == "" || solidInward_CA[j].SOLIDINWD_CS_ID_Mark_var == null)
            //            {
            //                cell1 = new PdfPCell(new Phrase("-", fontH1));
            //            }
            //            else
            //            {
            //                cell1 = new PdfPCell(new Phrase(solidInward_CA[j].SOLIDINWD_CS_ID_Mark_var, fontH1));
            //            }
            //            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //            table1.AddCell(cell1);
            //            if (solidInward_CA[j].SOLIDINWD_CS_Age_var == "NA")
            //            {
            //                cell1 = new PdfPCell(new Phrase("NA", fontH1));
            //            }
            //            else
            //            {
            //                cell1 = new PdfPCell(new Phrase(Convert.ToString(solidInward_CA[j].SOLIDINWD_CS_Age_var), fontH1));
            //            }
            //            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //            table1.AddCell(cell1);
            //            cell1 = new PdfPCell(new Phrase(Convert.ToString(solidInward_CA[j].SOLIDINWD_CS_Length_dec), fontH1));
            //            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //            table1.AddCell(cell1);
            //            cell1 = new PdfPCell(new Phrase(Convert.ToString(solidInward_CA[j].SOLIDINWD_CS_width_dec), fontH1));
            //            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //            table1.AddCell(cell1);
            //            int area = Convert.ToInt32(solidInward_CA[j].SOLIDINWD_CS_Length_dec) * Convert.ToInt32(solidInward_CA[j].SOLIDINWD_CS_width_dec);
            //            cell1 = new PdfPCell(new Phrase(Convert.ToString(area), fontH1));
            //            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //            table1.AddCell(cell1);
            //            cell1 = new PdfPCell(new Phrase(Convert.ToString(solidInward_CA[j].SOLIDINWD_CS_Load_Dec), fontH1));
            //            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //            table1.AddCell(cell1);
            //            cell1 = new PdfPCell(new Phrase(Convert.ToString(solidInward_CA[j].SOLIDINWD_CS_Strength_Dec), fontH1));
            //            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //            table1.AddCell(cell1);
            //            if (i == 0)
            //            {
            //                int qty = Convert.ToInt32(TxtQty.Text);
            //                if (qty < 8)
            //                {
            //                    cell1 = new PdfPCell(new Phrase("***", fontH1));
            //                }
            //                else
            //                {
            //                    cell1 = new PdfPCell(new Phrase(Convert.ToString(solidInward_CA[count - 1].SOLIDINWD_CS_Average_Dec), fontH1));
            //                }
            //                cell1.Rowspan = count;
            //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            //                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            //                table1.AddCell(cell1);
            //            }

            //            i++;

            //        }

            //        pdfDoc.Add(table1);

            //        #endregion
            //        table1.SpacingBefore = 10;
            //        pdfDoc.Add(new Paragraph("Compliance", fontH2));

            //        string notes = compl_notes(Convert.ToString(solidInward_CA[count - 1].SOLIDINWD_CS_Average_Dec), Convert.ToString(solidInward_CA[count - 1].SOLIDINWD_CS_Age_var));

            //        pdfDoc.Add(new Paragraph(notes, fontH1));


            //        pdfDoc.Add(new Paragraph("References/Notes", fontH2));
            //        #region iscode

            //        int serial_no = 0;
            //        //string blocktype = solidInward.FirstOrDefault().SOLIDINWD_BlockType_var;
            //        //var materialId = from w in dc.tbl_Materials where w.MATERIAL_RecordType_var == blocktype select w.MATERIAL_Id;
            //        //int mID = Convert.ToInt32(materialId.FirstOrDefault());
            //        //var iscode_desc = from w in dc.tbl_ISCodes where w.Isc_materialId_int == mID select w.Isc_Description_var;
            //        //if (iscode_desc.Count() > 0)
            //        //{
            //        //    pdfDoc.Add(new Paragraph((serial_no + 1) + "." + iscode_desc.FirstOrDefault(), fontH1));
            //        //    serial_no = serial_no + 1;
            //        //}

            //            var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, TestId, 0, 0, 0, "", 0, "").ToList();
            //            var IScount = iscd.Count();
            //            for (int cd = 0; cd < IScount; cd++)
            //            {
            //                if (serial_no == 0)
            //                {
            //                    pdfDoc.Add(new Paragraph("References/Notes :", fontH2));
            //                }
            //                serial_no++;
            //                pdfDoc.Add(new Paragraph((serial_no) + "." + Convert.ToString(iscd[cd].Isc_Description_var), fontH1));
            //                serial_no = serial_no + 1;
            //            }

            //        #endregion


            //        #region Remark

            //        serial_no = 0;
            //        table1.SpacingBefore = 5;
            //        var reference = dc.AllRemark_View("", System.Web.HttpContext.Current.TxtRefNo.Text.ToString(), 0, "SOLID").ToList();
            //        var referencecount = reference.Count();
            //        for (int r = 0; r < referencecount; r++)
            //        {
            //            if (r == 0)
            //            {
            //                pdfDoc.Add(new Paragraph("Remarks : ", fontH2));
            //            }
            //            var remark = dc.AllRemark_View("", "", Convert.ToInt32(reference[r].SOLIDDetail_Remark_ID), "SOLID").ToList();
            //            var remarkcount = remark.Count();
            //            for (int remk = 0; remk < remarkcount; remk++)
            //            {
            //                serial_no++;
            //                pdfDoc.Add(new Paragraph(serial_no + ")" + Convert.ToString(remark[remk].SOLID_Remark_var), fontH1));
            //            }
            //        }
            //        //var remarkID = from w in dc.tbl_Solid_Remark_dtls where (w.SOLIDINWD_ReferenceNo_var == TxtRefNo.Text) select w;
            //        //List<tbl_Solid_Remark_dtl> remarkIDList = remarkID.AsEnumerable()
            //        //        .Select(o => new tbl_Solid_Remark_dtl
            //        //        {
            //        //            SOLIDINWD_Remark_ID = o.SOLIDINWD_Remark_ID
            //        //        }).ToList();
            //        //if (remarkID.Count() > 0)
            //        //{
            //        //    if (remarkID.FirstOrDefault().SOLIDINWD_Remark_ID.Value != null)
            //        //    {
            //        //        for (int c = 0; c < remarkIDList.Count; c++)
            //        //        {
            //        //            int rID = Convert.ToInt32(remarkIDList[c].SOLIDINWD_Remark_ID);

            //        //            var list = from w in dc.tbl_Solid_Remarks where w.ID == rID select new { w.Name };

            //        //            List<tbl_Solid_Remark> remarkList = list.AsEnumerable()
            //        //                      .Select(o => new tbl_Solid_Remark
            //        //                      {
            //        //                          Name = o.Name
            //        //                      }).ToList();
            //        //            var remarkcount = remarkList.Count();

            //        //            foreach (var item in remarkList)
            //        //            {
            //        //                pdfDoc.Add(new Paragraph((serial_no + 1) + "." + item.Name, fontH1));
            //        //                serial_no = serial_no + 1;
            //        //            }
            //        //        }
            //        //    }

            //        //}

            //        int quantity = Convert.ToInt32(TxtQty.Text);
            //        if (quantity < 8)
            //        {
            //            pdfDoc.Add(new Paragraph(serial_no + 1 + ".At least 8 specimens are required for the testing, however as per customer request testing is done on lesser no. of specimens", fontH1));
            //        }

            //        #endregion
            //        //  pdfDoc.Add(Chunk.NEWLINE);


            //        //PdfPTable MaindataTable1 = new PdfPTable(2);  //tbl
            //        //MaindataTable1.SpacingBefore = 30;
            //        //MaindataTable1.WidthPercentage = 100;
            //        //PdfPCell cellbottom = new PdfPCell(new Paragraph("Authorized Signatory", fontH1));
            //        //cellbottom.Border = PdfPCell.NO_BORDER;
            //        //MaindataTable1.AddCell(cellbottom);

            //        //if (count != 0)
            //        //{
            //        //    if (solidInward_CA.FirstOrDefault().SOLIDINWD_CS_Checked_By != null)
            //        //    {
            //        //        var check_name = from a in dc.tbl_Users where a.USER_Id == solidInward_CA.FirstOrDefault().SOLIDINWD_CS_Checked_By select a;
            //        //        cellbottom = new PdfPCell(new Paragraph("Checked by : " + check_name.FirstOrDefault().USER_Name_var, fontH1));
            //        //    }
            //        //}
            //        //else
            //        //{
            //        //    cellbottom = new PdfPCell(new Paragraph("Checked by : -", fontH1));
            //        //}
            //        //cellbottom.Border = PdfPCell.NO_BORDER;
            //        //MaindataTable1.AddCell(cellbottom);
            //        //MaindataTable1.SpacingAfter = 30;
            //        //pdfDoc.Add(MaindataTable1);

            //        //if (count != 0)
            //        //{
            //        //    if (solidInward_CA.FirstOrDefault().SOLIDINWD_CS_Approved_By != null)
            //        //    {
            //        //        var auth_name = from a in dc.tbl_Users where a.USER_Id == solidInward_CA.FirstOrDefault().SOLIDINWD_CS_Approved_By select a;
            //        //        pdfDoc.Add(new Paragraph("(" + auth_name.FirstOrDefault().USER_Designation_var + ")", fontH1));


            //        //    }
            //        //}


            //        //pdfDoc.Add(paragraph);

            //        //if (count != 0)
            //        //{
            //        //    if (solidInward_CA.FirstOrDefault().SOLIDINWD_CS_Approved_By != null)
            //        //    {
            //        //        var auth_name = from a in dc.tbl_Users where a.USER_Id == solidInward_CA.FirstOrDefault().SOLIDINWD_CS_Approved_By select a;

            //        //        pdfDoc.Add(new Paragraph(auth_name.FirstOrDefault().USER_Name_var, fontH1));
            //        //    }
            //        //}
            //        //else
            //        //{
            //        //    pdfDoc.Add(new Paragraph("-", fontH1));

            //        //}

            //        PdfPTable MaindataTable1 = new PdfPTable(1);
            //        MaindataTable1.SpacingBefore = 20;
            //        MaindataTable1.WidthPercentage = 100;
            //        PdfPCell cellbottom = new PdfPCell(new Paragraph("Authorized Signatory", fontH1));
            //        cellbottom.Border = PdfPCell.NO_BORDER;
            //        MaindataTable1.AddCell(cellbottom);
            //        MaindataTable1.SpacingAfter = 40;
            //        pdfDoc.Add(MaindataTable1);

            //        var solidChkby = dc.ReportStatus_View("Masonary Block Testing", null, null, 1, 0, 0, System.Web.HttpContext.Current.TxtRefNo.Text.ToString(), 0, 0, 0).ToList();
            //        var RecNocount = solidChkby.Count();
            //        for (int r = 0; r < RecNocount; r++)
            //        {
            //            if (Convert.ToString(solidChkby[r].SOLIDINWD_ApprovedBy_tint) != null)
            //            {
            //                var User = dc.User_View(Convert.ToInt32(solidChkby[r].SOLIDINWD_ApprovedBy_tint), -1, "").ToList();
            //                var ucount = User.Count();
            //                for (int userId = 0; userId < ucount; userId++)
            //                {
            //                    if (Convert.ToString(System.Web.HttpContext.Current.Session["Print"]) != "")
            //                    {
            //                        pdfDoc.Add(new Paragraph(Convert.ToString(ApproveDt.ToString("dd/MM/yy")), fontH3));
            //                    }
            //                    else
            //                    {
            //                        pdfDoc.Add(new Paragraph("", fontH3));
            //                    }
            //                    pdfDoc.Add(new Paragraph(Convert.ToString(User[userId].USER_Name_var), fontH1));
            //                    pdfDoc.Add(new Paragraph("(" + Convert.ToString(User[userId].USER_Designation_var) + ")", fontH1));
            //                }
            //            }
            //            PdfPTable MaindataTable2 = new PdfPTable(2);
            //            MaindataTable2.SpacingBefore = 10;
            //            MaindataTable2.WidthPercentage = 100;
            //            PdfPCell cellbottom1;
            //            if (Witnessby != string.Empty)
            //            {
            //                cellbottom1 = new PdfPCell(new Paragraph("Witness by :  " + Witnessby, fontH1));
            //                cellbottom1.Border = PdfPCell.NO_BORDER;
            //                cellbottom1.HorizontalAlignment = Element.ALIGN_LEFT;
            //                MaindataTable2.AddCell(cellbottom1);
            //            }
            //            else
            //            {
            //                cellbottom1 = new PdfPCell(new Paragraph(" ", fontH1));
            //                cellbottom1.Border = PdfPCell.NO_BORDER;
            //                cellbottom1.HorizontalAlignment = Element.ALIGN_LEFT;
            //                MaindataTable2.AddCell(cellbottom1);
            //            }
            //            if (Convert.ToString(solidChkby[r].SOLIDINWD_CheckedBy_tint) != null)
            //            {
            //                var lgin = dc.User_View(Convert.ToInt32(solidChkby[r].SOLIDINWD_CheckedBy_tint), -1, "").ToList();
            //                var lginCount = lgin.Count();
            //                for (int loginusr = 0; loginusr < lginCount; loginusr++)
            //                {
            //                    cellbottom1 = new PdfPCell(new Paragraph("Checked by :  " + Convert.ToString(lgin[loginusr].USER_Name_var), fontH1));
            //                    cellbottom1.Border = PdfPCell.NO_BORDER;
            //                    cellbottom1.HorizontalAlignment = Element.ALIGN_RIGHT;
            //                    MaindataTable2.AddCell(cellbottom1);
            //                }
            //            }
            //            pdfDoc.Add(MaindataTable2);
            //        }

            //        var blackListTextFont = FontFactory.GetFont("Verdana", 4);
            //        paragraph = new Paragraph();
            //        paragraph.Alignment = Element.ALIGN_CENTER;
            //        paragraph.Font = blackListTextFont;
            //        paragraph.Add("--End of Report--");
            //        pdfDoc.Add(paragraph);
            //        pdfDoc.Add(new Paragraph("CIN-U28939PN1999PTC014212", fontH1));
            //        pdfDoc.Add(new Paragraph("REGD.ADD-1160/5, GHARPURE Colony Shivaji Nagar, Pune 411005,Maharashtra India", fontH1));
            //        pdfDoc.Close();
            //        string pdfPath = @"D:/" + Subfoldername1 + "/" + fileName;
            //        System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + fileName);
            //        System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
            //        System.Web.HttpContext.Current.Response.WriteFile(pdfPath);
            //        //
            //    }
            //}
            //catch (Exception ex)
            //{

            //}

            //#endregion

        }
        protected void ButtonDeleteRow_Click(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
            int rowIndex = currentRow.RowIndex;
            TextBox TxtRemark = (TextBox)GrdRemark.Rows[rowIndex].Cells[1].FindControl("TxtRemark");
            string remarkname = TxtRemark.Text;

            //var remarkid = from re in dc.tbl_Solid_Remarks where re.Name == remarkname select re.ID;

            //var rm = from remarkdetail in dc.tbl_Solid_Remark_dtls where remarkdetail.SOLIDINWD_Remark_ID == remarkid.FirstOrDefault() && remarkdetail.SOLIDINWD_ReferenceNo_var == TxtRefNo.Text select remarkdetail.ID;

            //  int id = Convert.ToInt32(rm.FirstOrDefault());
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

        }
        protected void BtnExit_Click(object sender, EventArgs e)
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
                string length = "", width = "", load = "";
                length = (row.FindControl("TxtLength") as TextBox).Text;
                width = (row.FindControl("TxtWidth") as TextBox).Text;
                load = (row.FindControl("TxtLoad") as TextBox).Text;

                if (length == "" || width == "" || load == "")
                {
                    flag = true;
                    break;
                }
            }
            if (flag == true)
            {
                exitFlag = "false";
                var exitResult = System.Windows.Forms.MessageBox.Show("Do you really want to Exit?", "Warning", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning, System.Windows.Forms.MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
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
        protected void TxtRemark_TextChanged(object sender, EventArgs e)
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
        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {
            clsData objcls = new clsData();
            DataTable dt;
            string mySql = ""; string age = "";
            int rowNo = 0;
            DateTime datecast = DateTime.Now, dateTest = DateTime.Now;
            string strReferenceNo = TxtRefNo.Text;
            //strReferenceNo = "3003/1-2";
            #region CS
            mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, cs.* 
                     from new_gt_app_db.dbo.reference_number ref, new_gt_app_db.dbo.category_header chead, 
                     new_gt_app_db.dbo.masonary_comp_str cs
                     where ref.pk_id = chead.reference_no and chead.pk_id = cs.category_header_fk_id
                     and ref.reference_number = '" + strReferenceNo + "' order by cs.quantity_no";

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

                    if (TxtDateOfTest.Text == "")
                    {
                        TxtDateOfTest.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    dateTest = DateTime.ParseExact(TxtDateOfTest.Text, "dd/MM/yyyy", null);
                    
                    if (TxtDateOfCast.Text != "NA")
                    {
                        datecast = DateTime.ParseExact(TxtDateOfCast.Text, "dd/MM/yyyy", null);
                        if (dateTest >= datecast)
                        {
                            string agediff = (dateTest - datecast).TotalDays.ToString();
                            age = agediff;
                        }
                        else
                        {
                            age = "0";
                        }
                    }
                    else if (datecast == null || TxtDateOfCast.Text == "NA")
                    {
                        age = "NA";
                    }
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
                        dt2.Columns.Add("Age", typeof(string));
                        dt2.Columns.Add("Length", typeof(string));
                        dt2.Columns.Add("Width", typeof(string));
                        dt2.Columns.Add("Load", typeof(string));
                        dt2.Columns.Add("Area", typeof(string));
                        dt2.Columns.Add("Strength", typeof(string));
                        dt2.Columns.Add("Average", typeof(string));
                        dt2.Columns.Add("ID", typeof(string));
                    }
                    DataRow NewRow = dt2.NewRow();
                    dt2.Rows.Add(NewRow);                    
                }
                GrdViewCS.DataSource = dt2;
                GrdViewCS.DataBind();
                //Display data in Grid
                for (int g = 0; g < GrdViewCS.Rows.Count; g++)
                {
                    TextBox TxtIdMark = (TextBox)GrdViewCS.Rows[g].Cells[1].FindControl("lblIdMark");
                    TextBox TxtLength = (TextBox)GrdViewCS.Rows[g].Cells[3].FindControl("TxtLength");
                    TextBox TxtWidth = (TextBox)GrdViewCS.Rows[g].Cells[4].FindControl("TxtWidth");
                    TextBox TxtLoad = (TextBox)GrdViewCS.Rows[g].Cells[5].FindControl("TxtLoad");
                    TextBox LblAge = (TextBox)GrdViewCS.Rows[g].Cells[2].FindControl("LblAge");

                    TxtIdMark.Text = dt.Rows[g]["id_mark"].ToString();
                    TxtLength.Text = Math.Round(Convert.ToDecimal(dt.Rows[g]["length"].ToString())).ToString();
                    TxtWidth.Text = Math.Round(Convert.ToDecimal(dt.Rows[g]["width"].ToString())).ToString(); 
                    TxtLoad.Text = Math.Round(Convert.ToDecimal(dt.Rows[g]["load"].ToString())).ToString();
                    LblAge.Text = age;

                    exitFlag = "true";
                    areacal(GrdViewCS.Rows[g]);
                }
            }
            dt.Dispose();
            #endregion

            objcls = null;
        }
    }
}