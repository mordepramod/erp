using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class Ggbs_Report : System.Web.UI.Page
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
                        Session.Abandon();
                        Response.Redirect("Login.aspx");
                    }
                    string[] arrMsgs = strReq.Split('&');
                    string[] arrIndMsg;

                    arrIndMsg = arrMsgs[0].Split('=');
                    txt_RecType.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[1].Split('=');
                    lblRecordNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[2].Split('=');
                    txt_ReferenceNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[3].Split('=');
                    lblEntry.Text = arrIndMsg[1].ToString().Trim();
                    //arrIndMsg = arrMsgs[4].Split('=');
                    //lblCubeCompStr.Text = arrIndMsg[1].ToString().Trim();
                }

                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "GGBS  - Report Entry ";
                if (txt_RecType.Text != "")
                {
                    txt_DateOfTesting.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    DisplayGgbsDetails();
                    DisplayGgbsDetailsGrid();
                    if (lblEntry.Text == "Check")
                    {
                        lblheading.Text = "GGBS - Report Check ";
                        lbl_TestedBy.Text = "Approve By";
                        DisplayRemark();                        
                    }
                    else
                    {
                        AddRowGgbsRemark();
                    }
                    LoadTestedOrApproveBy();
                    LoadReferenceNoList();
                }
            }
        }
        private void LoadReferenceNoList()
        {
            byte reportStatus = 0;
            if (lblEntry.Text == "Enter")
                reportStatus = 1;
            else if (lblEntry.Text == "Check")
                reportStatus = 2;

            var reportList = dc.ReferenceNo_View_StatusWise("GGBS", reportStatus, 0);
            ddl_OtherPendingRpt.DataTextField = "ReferenceNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_OtherPendingRpt.Items.Remove(txt_ReferenceNo.Text);
        }
        private void LoadTestedOrApproveBy()
        {
            byte testingBit = 0;
            byte approveBit = 0;
            if (lblEntry.Text == "Check")
                approveBit = 1;
            else
                testingBit = 1;
            var testinguser = dc.ReportStatus_View("", null, null, 0, testingBit, approveBit, "", 0, 0, 0);
            ddl_TestedBy.DataTextField = "USER_Name_var";
            ddl_TestedBy.DataValueField = "USER_Id";
            ddl_TestedBy.DataSource = testinguser;
            ddl_TestedBy.DataBind();
            ddl_TestedBy.Items.Insert(0, "---Select---");
        }
        protected void lnkFetch_Click(object sender, EventArgs e)
        {
            if (ddl_OtherPendingRpt.SelectedValue != "---Select---")
            {
                try
                {
                    lnkSave.Enabled = true;
                    lnkPrint.Visible = false;
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Visible = false;
                    ddl_TestedBy.SelectedIndex = 0;
                    grdGgbsReport.DataSource = null;
                    grdGgbsReport.DataBind();
                    grdGgbsRemark.DataSource = null;
                    grdGgbsRemark.DataBind();

                    txt_ReferenceNo.Text = ddl_OtherPendingRpt.SelectedItem.Text;                    
                    LoadReferenceNoList();
                    DisplayGgbsDetails();
                    DisplayGgbsDetailsGrid();
                    DisplayRemark();
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
        }

        #region Display data
        public void DisplayGgbsDetails()
        {
            txt_witnessBy.Visible = false;
            chkWitnessBy.Checked = false;
            var ggbsInwd = dc.ReportStatus_View("GGBS Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var g in ggbsInwd)
            {
                txt_ReferenceNo.Text = g.GGBSINWD_ReferenceNo_var.ToString();
                txt_RecType.Text = g.GGBSINWD_RecordType_var.ToString();
                txt_ReportNo.Text = g.GGBSINWD_SetOfRecord_var.ToString();
                if (g.GGBSINWD_TestedDate_dt.ToString() != null && g.GGBSINWD_TestedDate_dt.ToString() != "")
                {
                    txt_DateOfTesting.Text = Convert.ToDateTime(g.GGBSINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                }
                if (txt_DateOfTesting.Text == "" || lblEntry.Text == "Enter")
                {
                    txt_DateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                txt_Description.Text = g.GGBSINWD_Description_var.ToString();
                txt_Supplier.Text = g.GGBSINWD_SupplierName_var.ToString();
                txt_GgbsUsed.Text = g.GGBSINWD_GgbsName_var.ToString();
                txt_CementUsed.Text = g.GGBSINWD_CementName_var.ToString();
                if (g.GGBSINWD_ReceivedDate_dt != null && g.GGBSINWD_ReceivedDate_dt.ToString() != "")
                {
                    txt_ReceivedDate.Text = g.GGBSINWD_ReceivedDate_dt.ToString();
                }
                else
                {
                    txt_ReceivedDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                }                
                if (ddl_NablScope.Items.FindByValue(g.GGBSINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(g.GGBSINWD_NablScope_var);
                }
                if (Convert.ToString(g.GGBSINWD_NablLocation_int) != null && Convert.ToString(g.GGBSINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(g.GGBSINWD_NablLocation_int);
                }
                if (g.GGBSINWD_WitnessBy_var.ToString() != null && g.GGBSINWD_WitnessBy_var.ToString() != "")
                {
                    txt_witnessBy.Visible = true;
                    txt_witnessBy.Text = g.GGBSINWD_WitnessBy_var.ToString();
                    chkWitnessBy.Checked = true;
                }
            }
        }        
        public void DisplayGgbsDetailsGrid()
        {
            int i = 0;            
            var ggbsTest = dc.GgbsInward_View(0, txt_ReferenceNo.Text, true);
            foreach (var gt in ggbsTest)
            {
                AddRowGgbsReport();
                TextBox txt_NameOfTest = (TextBox)grdGgbsReport.Rows[i].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdGgbsReport.Rows[i].FindControl("txt_result");
                TextBox txt_Status = (TextBox)grdGgbsReport.Rows[i].FindControl("txt_Status");
                TextBox txt_TestDetails = (TextBox)grdGgbsReport.Rows[i].FindControl("txt_TestDetails");
                LinkButton lnkStrength = (LinkButton)grdGgbsReport.Rows[i].FindControl("lnkStrength");
                LinkButton lnkCementStrength = (LinkButton)grdGgbsReport.Rows[i].FindControl("lnkCementStrength");
                Label lbl_TestId = (Label)grdGgbsReport.Rows[i].FindControl("lbl_TestId");
                HiddenField ggbsStrength = (HiddenField)grdGgbsReport.Rows[i].FindControl("ggbsStrength");
                HiddenField cementStrength = (HiddenField)grdGgbsReport.Rows[i].FindControl("cementStrength");
                HiddenField CubeCastingStatus = (HiddenField)grdGgbsReport.Rows[i].FindControl("CubeCastingStatus");

                lbl_TestId.Text = Convert.ToString(gt.GGBSTEST_TEST_Id);
                if (Convert.ToString(gt.GGBSTEST_Result_var) != "" && Convert.ToString(gt.GGBSTEST_Result_var) != null)
                {
                    txt_result.Text = Convert.ToString(gt.GGBSTEST_Result_var);
                }
                if (Convert.ToString(gt.GGBSTEST_TestStatus_tint) != null && Convert.ToString(gt.GGBSTEST_TestStatus_tint) != "")
                {
                    txt_Status.Text = Convert.ToString(gt.GGBSTEST_TestStatus_tint);
                }
                txt_TestDetails.Text = Convert.ToString(gt.GGBSTEST_Details_var);

                txt_NameOfTest.Text = " " + gt.TEST_Name_var.ToString();
                
                if (gt.TEST_Name_var.ToString() == "Compressive Strength" || gt.TEST_Name_var.ToString() == "Slag activity index")
                {
                    CubeCastingStatus.Value = Convert.ToString(gt.GGBSINWD_CubeCastingStatus_tint);
                    if (Convert.ToString(gt.GGBSINWD_CubeCastingStatus_tint) == null || Convert.ToInt32(gt.GGBSINWD_CubeCastingStatus_tint) <= 0)
                    {
                        txt_result.Text = "Awaited";
                    }
                    if (gt.GGBSTEST_Days_tint.ToString() != "" && gt.GGBSTEST_Days_tint.ToString() != null && gt.GGBSTEST_Days_tint.ToString() != "0")
                    {
                        txt_NameOfTest.Text = " " + "(" + "" + gt.GGBSTEST_Days_tint.ToString() + " " + "Days" + " " + ")" + " " + gt.TEST_Name_var.ToString();

                        lnkStrength.Text = "GGBS";
                        var CompStr = dc.OtherCubeTestView(txt_ReferenceNo.Text, "GGBS", Convert.ToByte(gt.GGBSTEST_Days_tint), 0, "GGBS", false, true);
                        foreach (var cms in CompStr)
                        {
                            ggbsStrength.Value = Convert.ToString(cms.Avg_var);
                            if (gt.TEST_Name_var.ToString() == "Compressive Strength")
                            {
                                txt_result.Text = ggbsStrength.Value;
                            }
                        }
                        if (gt.TEST_Name_var.ToString() == "Slag activity index")
                        {
                            lnkCementStrength.Text = "Cement";
                            var CompStrCemt = dc.OtherCubeTestView(txt_ReferenceNo.Text, "GGBS", Convert.ToByte(gt.GGBSTEST_Days_tint), 0, "CEMT", false, true);
                            foreach (var cms in CompStrCemt)
                            {
                                cementStrength.Value = Convert.ToString(cms.Avg_var);
                                if (ggbsStrength.Value != "" && ggbsStrength.Value != null)
                                {
                                    txt_result.Text = Convert.ToDecimal(((Convert.ToDecimal(ggbsStrength.Value) / Convert.ToDecimal(cementStrength.Value)) * 100)).ToString("0.00");
                                }
                            }

                        }
                        
                    }                    
                }                
                i++;
            }
            for (int j = 0; j < grdGgbsReport.Rows.Count; j++)
            {
                TextBox txt_NameOfTest = (TextBox)grdGgbsReport.Rows[j].FindControl("txt_NameOfTest");
                LinkButton lnkStrength = (LinkButton)grdGgbsReport.Rows[j].FindControl("lnkStrength");
                LinkButton lnkCementStrength = (LinkButton)grdGgbsReport.Rows[j].FindControl("lnkCementStrength");
                HiddenField CubeCastingStatus = (HiddenField)grdGgbsReport.Rows[j].FindControl("CubeCastingStatus");
                if (txt_NameOfTest.Text.Contains("Compressive Strength") || txt_NameOfTest.Text.Contains("Slag activity index"))
                {
                    if (Convert.ToString(CubeCastingStatus.Value) == "" || Convert.ToInt32(CubeCastingStatus.Value) <= 0)
                    {
                        lnkStrength.Enabled = false;
                        lnkCementStrength.Enabled = false;
                    }
                    else
                    {
                        lnkStrength.Enabled = true;
                        lnkCementStrength.Enabled = true;
                    }
                }             
            }            
            DisplayRowItems();
        }
       
        public void DisplayRowItems()
        {
            for (int j = 0; j < grdGgbsReport.Rows.Count; j++)
            {
                TextBox txt_NameOfTest = (TextBox)grdGgbsReport.Rows[j].Cells[0].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdGgbsReport.Rows[j].Cells[1].FindControl("txt_result");
                Button btnResult = (Button)grdGgbsReport.Rows[j].Cells[1].FindControl("btnResult");
                TextBox txt_TestDetails = (TextBox)grdGgbsReport.Rows[j].FindControl("txt_TestDetails");
                TextBox txt_Status = (TextBox)grdGgbsReport.Rows[j].FindControl("txt_Status");
                if (txt_NameOfTest.Text == " Standard Consistency")
                {
                    btnResult.Visible = false;
                    txt_result.Visible = true;
                    if (txt_result.Text == "Awaited")
                    {
                        txt_result.Text = string.Empty;
                    }
                }
                else if (txt_NameOfTest.Text == " Density" || txt_NameOfTest.Text == " Soundness  By AutoClave" || txt_NameOfTest.Text == " Soundness By Le-Chateliers Apparatus" 
                    || txt_NameOfTest.Text == " Initial Setting Time" || txt_NameOfTest.Text == " Final Setting Time")
                {
                    btnResult.Visible = true;
                    txt_result.Visible = false;
                    if (txt_result.Text != "")
                    {
                        btnResult.Text = txt_result.Text;
                    }
                    else
                    {
                        btnResult.Text = "Awaited";
                    }
                    btnResult.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    btnResult.Visible = false;
                    txt_result.Visible = true;
                    if (txt_result.Text == string.Empty || txt_result.Text == "")
                    {
                        txt_result.Text = "Awaited";
                    }
                }
                // show shatus
                if (Convert.ToString(txt_Status.Text) == string.Empty)
                {
                    txt_Status.Text = "0";
                }
                if (txt_NameOfTest.Text.Contains("Compressive Strength"))
                {
                    txt_result.ReadOnly = true;
                }
                else if (txt_result.Visible == true)
                {
                    if (lblEntry.Text == "Check")
                    {
                        if (txt_result.Text == "Awaited")
                        {
                            txt_result.ReadOnly = true;
                        }
                        else if (txt_result.Text != string.Empty)
                        {
                            if (Convert.ToInt32(txt_Status.Text) == 2)
                            {
                                txt_result.ReadOnly = false;
                            }
                            else
                            {
                                txt_result.ReadOnly = true;
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(txt_Status.Text) < 2 || txt_result.Text == "Awaited")
                        {
                            txt_result.ReadOnly = false;
                        }
                        else if (txt_result.Text != string.Empty)
                        {
                            txt_result.ReadOnly = true;
                        }
                    }
                }
                //ShoWGrid()
                if (Convert.ToInt32(txt_Status.Text) == 2)
                {
                    if (txt_result.Text != "Awaited" && txt_result.Visible == true)
                    {
                        grdGgbsReport.Rows[j].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                        txt_NameOfTest.BackColor = System.Drawing.Color.LightYellow;
                        txt_result.BackColor = System.Drawing.Color.LightYellow;
                        grdGgbsReport.Rows[j].Cells[1].BackColor = System.Drawing.Color.LightYellow;
                        grdGgbsReport.Rows[j].Cells[2].BackColor = System.Drawing.Color.LightYellow;
                    }
                    if (btnResult.Text != "Awaited" && btnResult.Visible == true)
                    {
                        grdGgbsReport.Rows[j].Cells[0].BackColor = System.Drawing.Color.LightYellow;
                        txt_NameOfTest.BackColor = System.Drawing.Color.LightYellow;
                        btnResult.BackColor = System.Drawing.Color.LightYellow;
                        grdGgbsReport.Rows[j].Cells[1].BackColor = System.Drawing.Color.LightYellow;
                        grdGgbsReport.Rows[j].Cells[2].BackColor = System.Drawing.Color.LightYellow;
                    }
                }
                else if (Convert.ToInt32(txt_Status.Text) > 2)
                {
                    if (txt_result.Text != "Awaited" && txt_result.Visible == true)
                    {
                        grdGgbsReport.Rows[j].Cells[0].BackColor = System.Drawing.Color.LightPink;
                        txt_NameOfTest.BackColor = System.Drawing.Color.LightPink;
                        txt_result.BackColor = System.Drawing.Color.LightPink;
                        grdGgbsReport.Rows[j].Cells[1].BackColor = System.Drawing.Color.LightPink;
                        grdGgbsReport.Rows[j].Cells[2].BackColor = System.Drawing.Color.LightPink;
                    }
                    if (btnResult.Text != "Awaited" && btnResult.Visible == true)
                    {
                        grdGgbsReport.Rows[j].Cells[0].BackColor = System.Drawing.Color.LightPink;
                        txt_NameOfTest.BackColor = System.Drawing.Color.LightPink;
                        btnResult.BackColor = System.Drawing.Color.LightPink;
                        grdGgbsReport.Rows[j].Cells[1].BackColor = System.Drawing.Color.LightPink;
                        grdGgbsReport.Rows[j].Cells[2].BackColor = System.Drawing.Color.LightPink;
                    }
                }
            }
        }        
        
        public void DisplayRemark()
        {
            int i = 0;
            var re = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "GGBS");
            foreach (var r in re)
            {
                AddRowGgbsRemark();
                TextBox txt_REMARK = (TextBox)grdGgbsRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.GGBSDetail_RemarkId_int), "GGBS");
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.GGBS_Remark_var.ToString();
                    i++;
                }
            }
            if (grdGgbsRemark.Rows.Count <= 0)
            {
                AddRowGgbsRemark();
            }

        }
        #endregion

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                DateTime TestingDt = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);

                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, txt_Supplier.Text, 0, 0, "", 0, "", "", 0, 0, 0, "GGBS");
                    dc.ReportDetails_Update("GGBS", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                    dc.MISDetail_Update(0, "GGBS", txt_ReferenceNo.Text, "GGBS", null, true, false, false, false, false, false, false);
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, txt_Supplier.Text, 0, 0, "", 0, "", "", 0, 0, 0, "GGBS");
                    dc.ReportDetails_Update("GGBS", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "GGBS", txt_ReferenceNo.Text, "GGBS", null, false, true, false, false, false, false, false);
                }
                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "GGBS", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                #region SaveData
                bool chkTestStatus = false;
                for (int i = 0; i < grdGgbsReport.Rows.Count; i++)
                {
                    Button btnResult = (Button)grdGgbsReport.Rows[i].FindControl("btnResult");
                    TextBox txt_NameOfTest = (TextBox)grdGgbsReport.Rows[i].FindControl("txt_NameOfTest");
                    TextBox txt_TestDetails = (TextBox)grdGgbsReport.Rows[i].FindControl("txt_TestDetails");
                    TextBox txt_result = (TextBox)grdGgbsReport.Rows[i].FindControl("txt_result");
                    TextBox txt_Status = (TextBox)grdGgbsReport.Rows[i].FindControl("txt_Status");
                    Label lbl_TestId = (Label)grdGgbsReport.Rows[i].FindControl("lbl_TestId");

                    string line = txt_NameOfTest.Text.Substring(txt_NameOfTest.Text.LastIndexOf(')') + 1);
                    byte Days = 0;
                    string TestName = txt_NameOfTest.Text;
                    if (line.Trim() == "Compressive Strength" || line.Trim() == "Slag activity index")
                    {
                        string[] days = txt_NameOfTest.Text.Split('(', ' ', ')');
                        foreach (string cdays in days)
                        {
                            if (cdays != "" && cdays != " ")
                            {
                                Days = Convert.ToByte(cdays.ToString());
                                break;
                            }
                        }
                        if (txt_result.Text != "Awaited")
                        {
                            byte compStrStatus = 0;
                            if (lbl_TestedBy.Text == "Tested By")
                            {
                                compStrStatus = 2;
                            }
                            else if (lbl_TestedBy.Text == "Approve By")
                            {
                                compStrStatus = 3;
                            }
                            dc.OtherCubeTest_Update(txt_ReferenceNo.Text, "GGBS", Convert.ToByte(Days), 0, compStrStatus, "", 0, "", true, false);
                        }
                        TestName = line.ToString();
                    }
                                        
                    string Result = string.Empty;
                    if (btnResult.Text.Trim() != "Awaited" && btnResult.Visible == true)
                    {
                        Result = btnResult.Text;
                        if (lbl_TestedBy.Text == "Tested By")
                        {
                            if (Convert.ToByte(txt_Status.Text) < 2)
                            {
                                txt_Status.Text = "2";
                            }
                        }
                        else if (lbl_TestedBy.Text == "Approve By")
                        {
                            if (Convert.ToByte(txt_Status.Text) == 2)
                            {
                                txt_Status.Text = "3";
                            }
                        }
                    }
                    if (txt_result.Text.Trim() != "Awaited" && txt_result.Visible == true)
                    {
                        Result = txt_result.Text;
                        if (lbl_TestedBy.Text == "Tested By")
                        {
                            if (Convert.ToByte(txt_Status.Text) < 2)
                            {
                                txt_Status.Text = "2";
                            }
                        }
                        else if (lbl_TestedBy.Text == "Approve By")
                        {
                            if (Convert.ToByte(txt_Status.Text) == 2)
                            {
                                txt_Status.Text = "3";
                            }
                        }
                    }

                    if ((btnResult.Text.Trim() == "Awaited" && btnResult.Visible == true)
                        || (txt_result.Text.Trim() == "Awaited" && txt_result.Visible == true))
                    {
                        Result = "Awaited";
                    }
                    
                    if (Days <= 0)
                    {
                        dc.AllInwd_Update(txt_ReferenceNo.Text, "", 0, null, "", "", 0, Convert.ToInt32(lbl_TestId.Text), Convert.ToString(Result), 0, "", Convert.ToString(txt_TestDetails.Text), Convert.ToByte(txt_Status.Text), 0, 0, "GGBS");
                    }
                    else
                    {
                        dc.AllInwd_Update(txt_ReferenceNo.Text, "", 0, null, "", "", 0, Convert.ToInt32(lbl_TestId.Text), Convert.ToString(Result), 0, "", Convert.ToString(txt_TestDetails.Text), Convert.ToByte(txt_Status.Text), Days, 0, "GGBS");
                    }                                        
                    if (Convert.ToString(txt_Status.Text) == string.Empty)
                    {
                        txt_Status.Text = Convert.ToString(0);
                    }
                    if (Convert.ToInt32(txt_Status.Text) < 2 || Convert.ToString(txt_Status.Text) == string.Empty || Convert.ToInt32(txt_Status.Text) > 3)
                    {
                        chkTestStatus = true;
                    }

                }
                if (chkTestStatus == false)
                {
                    //Update Report StATUS
                    if (lbl_TestedBy.Text == "Tested By")
                    {
                        dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, txt_Supplier.Text, 2, 0, "", 0, "", "", 0, 0, 0, "GGBS");
                    }
                    else if (lbl_TestedBy.Text == "Approve By")
                    {
                        dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, txt_Supplier.Text, 3, 0, "", 0, "", "", 0, 0, 0, "GGBS");
                    }
                }
                #endregion

                #region Remark Gridview
                int RemarkId = 0;
                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "GGBS", true);
                for (int i = 0; i < grdGgbsRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdGgbsRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AllRemark_View(txt_REMARK.Text, "", 0, "GGBS");
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.CEMT_RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "GGBS");
                            foreach (var c in chkId)
                            {
                                if (c.CEMTDetail_RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "GGBS", false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.AllRemark_Update(0, txt_REMARK.Text, "GGBS");
                            var chc = dc.AllRemark_View(txt_REMARK.Text, "", 0, "GGBS");
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.CEMT_RemarkId_int);
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "GGBS", false);
                            }
                        }
                    }
                }
                #endregion
                lnkPrint.Visible = true;
                //
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkSave.Enabled = false;
            }
        }
        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            Boolean valid = true;
            string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime TestingDate = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);
            DateTime currentDate1 = DateTime.ParseExact(CurrentDate, "dd/MM/yyyy", null);
            if (TestingDate > currentDate1)
            {
                lblMsg.Text = "Testing Date should be always less than or equal to the Current Date.";
                valid = false;
            }
            else if (txt_Description.Text == "")
            {
                lblMsg.Text = "Please Enter Description";
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
            else if (chkWitnessBy.Checked && txt_witnessBy.Text == "" && txt_witnessBy.Enabled == true)
            {
                lblMsg.Text = "Please Enter Witness By";
                txt_witnessBy.Focus();
                valid = false;
            }
            else if (lblEntry.Text == "Check" && ddl_TestedBy.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select the Approve By";
                valid = false;
            }
            else if (lblEntry.Text == "Enter" && ddl_TestedBy.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select the Tested By";
                valid = false;
            }
            else if (valid == true)
            {
                int j = 0;
                for (int i = 0; i < grdGgbsReport.Rows.Count; i++)
                {
                    TextBox txt_result = (TextBox)grdGgbsReport.Rows[i].FindControl("txt_result");
                    Button btnResult = (Button)grdGgbsReport.Rows[i].FindControl("btnResult");
                    if (btnResult.Text == "Awaited" || txt_result.Text == "Awaited")
                    {
                        j++;
                    }
                    if (txt_result.Visible == true && txt_result.Text == "")
                    {
                        lblMsg.Text = "Please Enter result in (%) for Sr No. " + (i + 1) + ".";
                        txt_result.Focus();
                        valid = false;
                        break;
                    }
                    else if (j == grdGgbsReport.Rows.Count)
                    {
                        lblMsg.Text = "It requires at least one result";
                        valid = false;
                        break;
                    }
                    else if (txt_result.Visible == true && txt_result.Text != "Awaited" && txt_result.Text != "*" && txt_result.Text.ToUpper() != "NIL" && Convert.ToDouble(txt_result.Text) <= 0)
                    {
                        lblMsg.Text = "Result should be greater than 0 for Sr No. " + (i + 1) + ".";
                        txt_result.Focus();
                        valid = false;
                        break;
                    }
                    else if (btnResult.Visible == true && btnResult.Text != "Awaited" && btnResult.Text != "*" && btnResult.Text.ToUpper() != "NIL" && Convert.ToDouble(btnResult.Text) <= 0)
                    {
                        lblMsg.Text = "Result should be greater than 0 for Sr No. " + (i + 1) + ".";
                        valid = false;
                        break;
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

        #region Ggbs Report grid
        protected void AddRowGgbsReport()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["GgbsTestTable"] != null)
            {
                GetCurrentDataGgbsReport();
                dt = (DataTable)ViewState["GgbsTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_NameOfTest", typeof(string)));//
                dt.Columns.Add(new DataColumn("txt_result", typeof(string)));
                dt.Columns.Add(new DataColumn("btnResult", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_TestDetails", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Status", typeof(string)));//
                dt.Columns.Add(new DataColumn("lbl_TestId", typeof(string)));
                dt.Columns.Add(new DataColumn("ggbsStrength", typeof(string)));
                dt.Columns.Add(new DataColumn("cementStrength", typeof(string)));
                dt.Columns.Add(new DataColumn("CubeCastingStatus", typeof(string)));
                dt.Columns.Add(new DataColumn("lnkStrength", typeof(string)));
                dt.Columns.Add(new DataColumn("lnkCementStrength", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_NameOfTest"] = string.Empty;            
            dr["txt_result"] = string.Empty;
            dr["btnResult"] = string.Empty;
            dr["txt_TestDetails"] = string.Empty;
            dr["txt_Status"] = string.Empty;
            dr["lbl_TestId"] = string.Empty;
            dr["ggbsStrength"] = string.Empty;
            dr["cementStrength"] = string.Empty;
            dr["CubeCastingStatus"] = string.Empty;
            dr["lnkStrength"] = string.Empty;
            dr["lnkCementStrength"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["GgbsTestTable"] = dt;
            grdGgbsReport.DataSource = dt;
            grdGgbsReport.DataBind();
            SetPreviousDataGgbsReport();
        }
        protected void GetCurrentDataGgbsReport()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_NameOfTest", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_result", typeof(string)));
            dtTable.Columns.Add(new DataColumn("btnResult", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_TestDetails", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Status", typeof(string)));//
            dtTable.Columns.Add(new DataColumn("lbl_TestId", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ggbsStrength", typeof(string)));
            dtTable.Columns.Add(new DataColumn("cementStrength", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CubeCastingStatus", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lnkStrength", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lnkCementStrength", typeof(string)));

            for (int i = 0; i < grdGgbsReport.Rows.Count; i++)
            {
                TextBox txt_NameOfTest = (TextBox)grdGgbsReport.Rows[i].Cells[0].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdGgbsReport.Rows[i].Cells[1].FindControl("txt_result");
                Button btnResult = (Button)grdGgbsReport.Rows[i].Cells[1].FindControl("btnResult");
                TextBox txt_TestDetails = (TextBox)grdGgbsReport.Rows[i].Cells[2].FindControl("txt_TestDetails");
                TextBox txt_Status = (TextBox)grdGgbsReport.Rows[i].Cells[3].FindControl("txt_Status");
                Label lbl_TestId = (Label)grdGgbsReport.Rows[i].Cells[4].FindControl("lbl_TestId");
                HiddenField ggbsStrength = (HiddenField)grdGgbsReport.Rows[i].Cells[4].FindControl("ggbsStrength");
                HiddenField cementStrength = (HiddenField)grdGgbsReport.Rows[i].Cells[4].FindControl("cementStrength");
                HiddenField CubeCastingStatus = (HiddenField)grdGgbsReport.Rows[i].Cells[4].FindControl("CubeCastingStatus");
                LinkButton lnkStrength = (LinkButton)grdGgbsReport.Rows[i].Cells[0].FindControl("lnkStrength");
                LinkButton lnkCementStrength = (LinkButton)grdGgbsReport.Rows[i].Cells[0].FindControl("lnkCementStrength");

                drRow = dtTable.NewRow();
                drRow["txt_NameOfTest"] = txt_NameOfTest.Text;
                drRow["txt_result"] = txt_result.Text;
                drRow["btnResult"] = btnResult.Text;
                drRow["txt_TestDetails"] = txt_TestDetails.Text;
                drRow["txt_Status"] = txt_Status.Text;
                drRow["lbl_TestId"] = lbl_TestId.Text;
                drRow["ggbsStrength"] = ggbsStrength.Value;
                drRow["cementStrength"] = cementStrength.Value;
                drRow["CubeCastingStatus"] = CubeCastingStatus.Value;
                drRow["lnkStrength"] = lnkStrength.Text;
                drRow["lnkCementStrength"] = lnkCementStrength.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["GgbsTestTable"] = dtTable;

        }
        protected void SetPreviousDataGgbsReport()
        {
            DataTable dt = (DataTable)ViewState["GgbsTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_NameOfTest = (TextBox)grdGgbsReport.Rows[i].Cells[0].FindControl("txt_NameOfTest");
                TextBox txt_result = (TextBox)grdGgbsReport.Rows[i].Cells[1].FindControl("txt_result");
                Button btnResult = (Button)grdGgbsReport.Rows[i].Cells[1].FindControl("btnResult");
                TextBox txt_TestDetails = (TextBox)grdGgbsReport.Rows[i].Cells[2].FindControl("txt_TestDetails");
                TextBox txt_Status = (TextBox)grdGgbsReport.Rows[i].Cells[3].FindControl("txt_Status");
                Label lbl_TestId = (Label)grdGgbsReport.Rows[i].Cells[4].FindControl("lbl_TestId");
                HiddenField ggbsStrength = (HiddenField)grdGgbsReport.Rows[i].Cells[4].FindControl("ggbsStrength");
                HiddenField cementStrength = (HiddenField)grdGgbsReport.Rows[i].Cells[4].FindControl("cementStrength");
                HiddenField CubeCastingStatus = (HiddenField)grdGgbsReport.Rows[i].Cells[4].FindControl("CubeCastingStatus");
                LinkButton lnkStrength = (LinkButton)grdGgbsReport.Rows[i].Cells[0].FindControl("lnkStrength");
                LinkButton lnkCementStrength = (LinkButton)grdGgbsReport.Rows[i].Cells[0].FindControl("lnkCementStrength");

                grdGgbsReport.Rows[i].Cells[1].BackColor = System.Drawing.Color.White;
                grdGgbsReport.Rows[i].Cells[2].BackColor = System.Drawing.Color.White;
                txt_NameOfTest.Text = dt.Rows[i]["txt_NameOfTest"].ToString();
                txt_result.Text = dt.Rows[i]["txt_result"].ToString();
                btnResult.Text = dt.Rows[i]["btnResult"].ToString();
                txt_TestDetails.Text = dt.Rows[i]["txt_TestDetails"].ToString();
                txt_Status.Text = dt.Rows[i]["txt_Status"].ToString();
                lbl_TestId.Text = dt.Rows[i]["lbl_TestId"].ToString();
                ggbsStrength.Value = dt.Rows[i]["ggbsStrength"].ToString();
                cementStrength.Value = dt.Rows[i]["cementStrength"].ToString();
                CubeCastingStatus.Value = dt.Rows[i]["CubeCastingStatus"].ToString();
                lnkStrength.Text = dt.Rows[i]["lnkStrength"].ToString();
                lnkCementStrength.Text = dt.Rows[i]["lnkCementStrength"].ToString();

            }

        }
        #endregion

        #region Ir Fr grid
        protected void AddRowIRFR()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["IRFRTable"] != null)
            {
                GetCurrentDataIRFR();
                dt = (DataTable)ViewState["IRFRTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_IRgrid", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_FRgrid", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_IRgrid"] = string.Empty;
            dr["txt_FRgrid"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["IRFRTable"] = dt;
            grdIRFR.DataSource = dt;
            grdIRFR.DataBind();
            SetPreviousDataIRFR();
        }
        protected void GetCurrentDataIRFR()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_IRgrid", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_FRgrid", typeof(string)));

            for (int i = 0; i < grdIRFR.Rows.Count; i++)
            {
                TextBox txt_IRgrid = (TextBox)grdIRFR.Rows[i].Cells[0].FindControl("txt_IRgrid");
                TextBox txt_FRgrid = (TextBox)grdIRFR.Rows[i].Cells[1].FindControl("txt_FRgrid");

                drRow = dtTable.NewRow();
                drRow["txt_IRgrid"] = txt_IRgrid.Text;
                drRow["txt_FRgrid"] = txt_FRgrid.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["IRFRTable"] = dtTable;

        }
        protected void SetPreviousDataIRFR()
        {
            DataTable dt = (DataTable)ViewState["IRFRTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_IRgrid = (TextBox)grdIRFR.Rows[i].Cells[0].FindControl("txt_IRgrid");
                TextBox txt_FRgrid = (TextBox)grdIRFR.Rows[i].Cells[1].FindControl("txt_FRgrid");

                txt_IRgrid.Text = dt.Rows[i]["txt_IRgrid"].ToString();
                txt_FRgrid.Text = dt.Rows[i]["txt_FRgrid"].ToString();
            }
        }
        #endregion

        #region Remark grid
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowGgbsRemark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdGgbsRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdGgbsRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowGgbsRemark(gvr.RowIndex);
            }
        }
        protected void DeleteRowGgbsRemark(int rowIndex)
        {
            GetCurrentDataGgbsRemark();
            DataTable dt = ViewState["GgbsRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["GgbsRemarkTable"] = dt;
            grdGgbsRemark.DataSource = dt;
            grdGgbsRemark.DataBind();
            SetPreviousDataGgbsRemark();
        }
        protected void AddRowGgbsRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["GgbsRemarkTable"] != null)
            {
                GetCurrentDataGgbsRemark();
                dt = (DataTable)ViewState["GgbsRemarkTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_REMARK"] = string.Empty;

            dt.Rows.Add(dr);

            ViewState["GgbsRemarkTable"] = dt;
            grdGgbsRemark.DataSource = dt;
            grdGgbsRemark.DataBind();
            SetPreviousDataGgbsRemark();
        }
        protected void GetCurrentDataGgbsRemark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            for (int i = 0; i < grdGgbsRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdGgbsRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;
                
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["GgbsRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataGgbsRemark()
        {
            DataTable dt = (DataTable)ViewState["GgbsRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdGgbsRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdGgbsRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }
        #endregion
                      
        protected void btnResult_Click(object sender, CommandEventArgs e)
        {
            ModalPopupExtender1.Show();
            GridViewRow clickedRow = ((Button)sender).NamingContainer as GridViewRow;
            TextBox txt_NameOfTest = (TextBox)clickedRow.FindControl("txt_NameOfTest");
            TextBox txt_TestDetails = (TextBox)clickedRow.FindControl("txt_TestDetails");
            Button btnResult = (Button)clickedRow.FindControl("btnResult");
            TextBox txt_Status = (TextBox)clickedRow.FindControl("txt_Status");
            if (txt_NameOfTest.Text.Trim() == "Density")
            {
                divDensity.Visible = true;
                divSndnesautoclave.Visible = false;
                divSndnsByLe_Chateliers.Visible = false;
                divIni_finSet.Visible = false;
                txt_Weight.Focus();
            }
            if (txt_NameOfTest.Text.Trim() == "Soundness  By AutoClave")
            {
                divSndnesautoclave.Visible = true;
                divDensity.Visible = false;
                divSndnsByLe_Chateliers.Visible = false;
                divIni_finSet.Visible = false;
                txt_IR.Focus();
            }
            if (txt_NameOfTest.Text.Trim() == "Soundness By Le-Chateliers Apparatus")
            {
                divSndnsByLe_Chateliers.Visible = true;
                divDensity.Visible = false;
                divSndnesautoclave.Visible = false;
                divIni_finSet.Visible = false;
                grdIRFR.DataSource = null;
                grdIRFR.DataBind();
                for (int i = 0; i < 2; i++)
                {
                    AddRowIRFR();
                }
            }
            if (txt_NameOfTest.Text.Trim() == "Initial Setting Time" || txt_NameOfTest.Text.Trim() == "Final Setting Time")
            {
                divIni_finSet.Visible = true;
                divSndnesautoclave.Visible = false;
                divDensity.Visible = false;
                divSndnsByLe_Chateliers.Visible = false;
                txt_WaterAdded.Focus();
            }
            
            int Testid = 0;
            int j = 0;
            txt_Weight.Text = string.Empty;
            txt_Volume.Text = string.Empty;
            txt_IR.Text = string.Empty;
            txt_FR.Text = string.Empty;
            txt_WaterAdded.Text = string.Empty;
            txt_IST.Text = string.Empty;
            txt_FST.Text = string.Empty;
            if (lblEntry.Text == "Check")
            {
                if (Convert.ToInt32(txt_Status.Text) > 2 || Convert.ToInt32(txt_Status.Text) < 2 
                    || Convert.ToInt32(txt_Status.Text) != 2)
                {
                    chk_Awaited.Enabled = false;
                    txt_Weight.ReadOnly = true;
                    txt_Volume.ReadOnly = true;
                    txt_IR.ReadOnly = true;
                    txt_FR.ReadOnly = true;
                    txt_WaterAdded.ReadOnly = true;
                    txt_IST.ReadOnly = true;
                    txt_FST.ReadOnly = true;
                }
                else
                {
                    if (btnResult.Text != "Awaited")
                    {
                        chk_Awaited.Enabled = false;
                    }
                    else
                    {
                        chk_Awaited.Enabled = true;
                    }
                    txt_Weight.ReadOnly = false;
                    txt_Volume.ReadOnly = false;
                    txt_IR.ReadOnly = false;
                    txt_FR.ReadOnly = false;
                    txt_WaterAdded.ReadOnly = false;
                    txt_IST.ReadOnly = false;
                    txt_FST.ReadOnly = false;
                }
            }
            else
            {
                if (Convert.ToInt32(txt_Status.Text) >= 2)
                {
                    chk_Awaited.Enabled = false;
                    txt_Weight.ReadOnly = true;
                    txt_Volume.ReadOnly = true;
                    txt_IR.ReadOnly = true;
                    txt_FR.ReadOnly = true;
                    txt_WaterAdded.ReadOnly = true;
                    txt_IST.ReadOnly = true;
                    txt_FST.ReadOnly = true;
                }
                else
                {
                    chk_Awaited.Enabled = true;
                    txt_Weight.ReadOnly = false;
                    txt_Volume.ReadOnly = false;
                    txt_IR.ReadOnly = false;
                    txt_FR.ReadOnly = false;
                    txt_WaterAdded.ReadOnly = false;
                    txt_IST.ReadOnly = false;
                    txt_FST.ReadOnly = false;
                }
            }

            var ggbstTest = dc.Test(0, "", 0, "GGBS", txt_NameOfTest.Text.Trim(), 0);
            foreach (var ggbs in ggbstTest)
            {
                Testid = Convert.ToInt32(ggbs.TEST_Id);
            }
            if (txt_TestDetails.Text != string.Empty)
            {
                if (btnResult.Text.Trim() != "Awaited")
                {
                    string line = txt_TestDetails.Text.Substring(txt_TestDetails.Text.LastIndexOf('|') + 1);
                    string[] line2 = line.Split('~');
                    foreach (string line3 in line2)
                    {
                        j = 0;
                        if (divDensity.Visible == true && txt_NameOfTest.Text.Trim() == "Density")
                        {
                            chk_Awaited.Checked = false;
                            if (line3 != "" && txt_Weight.Text == "")
                            {
                                txt_Weight.Text = line3.ToString();
                                j++;
                            }
                            if (line3 != "" && j == 0)
                            {
                                txt_Volume.Text = line3.ToString();
                            }
                        }
                        if (divSndnesautoclave.Visible == true && txt_NameOfTest.Text.Trim() == "Soundness  By AutoClave")
                        {
                            chk_Awaited.Checked = false;
                            if (line3 != "" && txt_IR.Text == "")
                            {
                                txt_IR.Text = line3.ToString();
                                j++;
                            }
                            if (line3 != "" && j == 0 && txt_FR.Text == "")
                            {
                                txt_FR.Text = line3.ToString();
                            }
                        }
                        if (divSndnsByLe_Chateliers.Visible == true && txt_NameOfTest.Text.Trim() == "Soundness By Le-Chateliers Apparatus")
                        {
                            chk_Awaited.Checked = false;
                            grdIRFR.DataSource = null;
                            grdIRFR.DataBind();
                            int g = 0;
                            string[] line4 = txt_TestDetails.Text.Split('$');
                            foreach (var line6 in line4)
                            {
                                if (line6 != "")
                                {
                                    AddRowIRFR();
                                    TextBox txt_IRgrid = (TextBox)grdIRFR.Rows[g].Cells[0].FindControl("txt_IRgrid");
                                    TextBox txt_FRgrid = (TextBox)grdIRFR.Rows[g].Cells[1].FindControl("txt_FRgrid");
                                    string lineS = line6.Substring(line6.LastIndexOf('|') + 1);
                                    string[] resultline = lineS.Split('~');
                                    foreach (string rline in resultline)
                                    {
                                        if (rline != "")
                                        {
                                            j = 0;
                                            if (txt_IRgrid.Text == "")
                                            {
                                                txt_IRgrid.Text = rline.ToString();
                                                j++;
                                            }
                                            if (txt_FRgrid.Text == "" && j == 0)
                                            {
                                                txt_FRgrid.Text = rline.ToString();
                                                g++;
                                            }
                                        }
                                    }
                                    if (g == 2)
                                    {
                                        if (lblEntry.Text == "Check")
                                        {
                                            if (Convert.ToInt32(txt_Status.Text) != 2)
                                            {
                                                DisplayGr();
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(txt_Status.Text) >= 2)
                                            {
                                                DisplayGr();
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        if (divIni_finSet.Visible == true && txt_NameOfTest.Text.Trim() == "Initial Setting Time" || txt_NameOfTest.Text.Trim() == "Final Setting Time")
                        {
                            j = 0;
                            chk_Awaited.Checked = false;
                            if (line3 != "" && txt_WaterAdded.Text == "")
                            {
                                txt_WaterAdded.Text = line3.ToString();
                                j++;
                            }
                            if (line3 != "" && j == 0 && txt_IST.Text == "")
                            {
                                txt_IST.Text = line3.ToString();
                                j++;
                            }
                            if (line3 != "" && j == 0 && txt_FST.Text == "")
                            {
                                txt_FST.Text = line3.ToString();
                            }
                        }
                    }
                }
                else
                {
                    btnResult.Text = "Awaited";
                    txt_TestDetails.Text = string.Empty;
                }
            }
            if (btnResult.Text == "Awaited")
            {
                if (lblEntry.Text == "Check")
                {
                    if (Convert.ToInt32(txt_Status.Text) != 2)
                    {
                        DisplayGr();
                    }
                }
                else
                {
                    if (Convert.ToInt32(txt_Status.Text) >= 2)
                    {
                        DisplayGr();
                    }
                }
            }
        }                       
        public void DisplayGr()
        {
            for (int i = 0; i < grdIRFR.Rows.Count; i++)
            {
                TextBox txt_IRgrid = (TextBox)grdIRFR.Rows[i].Cells[0].FindControl("txt_IRgrid");
                TextBox txt_FRgrid = (TextBox)grdIRFR.Rows[i].Cells[1].FindControl("txt_FRgrid");
                txt_IRgrid.ReadOnly = true;
                txt_FRgrid.ReadOnly = true;
            }
        }

        protected void lnkStrength_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            TextBox txt_NameOfTest = (TextBox)clickedRow.FindControl("txt_NameOfTest");
            TextBox txt_Status = (TextBox)clickedRow.FindControl("txt_Status");//
            TextBox txt_result = (TextBox)clickedRow.FindControl("txt_result");

            int Days = 0;
            string[] test = txt_NameOfTest.Text.Split('(', ')', ' ');
            foreach (var Comp in test)
            {
                if (Comp != "")
                {
                    if (int.TryParse(Comp, out Days))
                    {
                        Days = Convert.ToInt32(Comp.ToString());
                        break;
                    }
                }
            }
            EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
            string strURLWithData = "Cube_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&ReportNo={4}&TrailId={5}&TrialCubeCompStr={6}&Result={7}&ComressiveTest={8}&CheckStatus={9}&Days={10}"
                                                                                     , "GGBS", lblRecordNo.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "CubeCompStrength", txt_result.Text, txt_NameOfTest.Text, txt_Status.Text, Days));
            Response.Redirect(strURLWithData);

        }
        protected void lnkCementStrength_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            TextBox txt_NameOfTest = (TextBox)clickedRow.FindControl("txt_NameOfTest");
            TextBox txt_Status = (TextBox)clickedRow.FindControl("txt_Status");//
            TextBox txt_result = (TextBox)clickedRow.FindControl("txt_result");

            int Days = 0;
            string[] test = txt_NameOfTest.Text.Split('(', ')', ' ');
            foreach (var Comp in test)
            {
                if (Comp != "")
                {
                    if (int.TryParse(Comp, out Days))
                    {
                        Days = Convert.ToInt32(Comp.ToString());
                        break;
                    }
                }
            }
            EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
            string strURLWithData = "Cube_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&ReportNo={4}&TrailId={5}&TrialCubeCompStr={6}&Result={7}&ComressiveTest={8}&CheckStatus={9}&Days={10}"
                                                                                     , "GGBS", lblRecordNo.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "CementStrength", txt_result.Text, txt_NameOfTest.Text, txt_Status.Text, Days));
            Response.Redirect(strURLWithData);

        }

        protected void imgCloseTestDetails_Click(object sender, ImageClickEventArgs e)
        {
            if (ValidateCalculateData() == true)
            {
                ModalPopupExtender1.Hide();
                int Testid = 0;
                for (int i = 0; i < grdGgbsReport.Rows.Count; i++)
                {
                    TextBox txt_NameOfTest = (TextBox)grdGgbsReport.Rows[i].Cells[0].FindControl("txt_NameOfTest");
                    Button btnResult = (Button)grdGgbsReport.Rows[i].Cells[1].FindControl("btnResult");
                    TextBox txt_TestDetails = (TextBox)grdGgbsReport.Rows[i].Cells[2].FindControl("txt_TestDetails");

                    if (divDensity.Visible == true)
                    {
                        if (txt_NameOfTest.Text.Trim() == "Density")
                        {
                            if (chk_Awaited.Checked == false)
                            {
                                chk_Awaited.Checked = false;
                                var cemtytest = dc.Test(0, "", 0, "GGBS", txt_NameOfTest.Text.Trim(), 0);
                                foreach (var cemt in cemtytest)
                                {
                                    Testid = Convert.ToInt32(cemt.TEST_Id);
                                }
                                if (Convert.ToDecimal(txt_Weight.Text) > 0 && Convert.ToDecimal(txt_Volume.Text) > 0)
                                {
                                    btnResult.Text = (Convert.ToDecimal(txt_Weight.Text) / Convert.ToDecimal(txt_Volume.Text)).ToString("0.00");

                                    if (Testid > 0 && btnResult.Text != "Awaited")
                                    {
                                        txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + txt_Weight.Text + "~" + txt_Volume.Text;
                                    }
                                }
                            }
                            else
                            {
                                txt_TestDetails.Text = string.Empty;
                                btnResult.Text = "Awaited";
                            }
                        }
                    }
                    if (divIni_finSet.Visible == true)
                    {
                        if (txt_NameOfTest.Text.Trim() == "Initial Setting Time")
                        {
                            if (chk_Awaited.Checked == false)
                            {
                                chk_Awaited.Checked = false;
                                var cemtytest = dc.Test(0, "", 0, "GGBS", txt_NameOfTest.Text.Trim(), 0);
                                foreach (var cemt in cemtytest)
                                {
                                    Testid = Convert.ToInt32(cemt.TEST_Id);
                                }

                                if (txt_WaterAdded.Text != "" && txt_IST.Text != "")
                                {
                                    if (txt_NameOfTest.Text.Trim() == "Initial Setting Time")
                                    {
                                        DateTime WaterAdded = new DateTime();
                                        WaterAdded = Convert.ToDateTime(txt_WaterAdded.Text);
                                        DateTime IST = new DateTime();
                                        IST = Convert.ToDateTime(txt_IST.Text);
                                        TimeSpan ts = WaterAdded.Subtract(IST);
                                        btnResult.Text = Convert.ToString(Math.Abs(ts.TotalMinutes));
                                        if (Testid > 0 && btnResult.Text != "Awaited")
                                        {
                                            txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + txt_WaterAdded.Text + "~" + txt_IST.Text + "~" + txt_FST.Text;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                btnResult.Text = "Awaited";
                                txt_TestDetails.Text = string.Empty;
                            }
                        }
                        if (txt_NameOfTest.Text.Trim() == "Final Setting Time")
                        {
                            if (chk_Awaited.Checked == false)
                            {
                                if (txt_WaterAdded.Text != "" && txt_FST.Text != "")
                                {
                                    chk_Awaited.Checked = false;
                                    DateTime WaterAdded = new DateTime();
                                    WaterAdded = Convert.ToDateTime(txt_WaterAdded.Text);
                                    DateTime FST = new DateTime();
                                    FST = Convert.ToDateTime(txt_FST.Text);
                                    TimeSpan ts = WaterAdded.Subtract(FST);
                                    btnResult.Text = Convert.ToString(Math.Abs(ts.TotalMinutes));

                                    if (Testid > 0 && btnResult.Text != "Awaited")
                                    {
                                        txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + txt_WaterAdded.Text + "~" + txt_IST.Text + "~" + txt_FST.Text;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                btnResult.Text = "Awaited";
                                txt_TestDetails.Text = string.Empty;
                            }

                        }
                    }
                    if (divSndnsByLe_Chateliers.Visible == true)
                    {
                        decimal Diffrnce = 0;
                        if (txt_NameOfTest.Text.Trim() == "Soundness By Le-Chateliers Apparatus")
                        {
                            if (chk_Awaited.Checked == false)
                            {
                                chk_Awaited.Checked = false;
                                var cemtytest = dc.Test(0, "", 0, "GGBS", txt_NameOfTest.Text.Trim(), 0);
                                foreach (var cemt in cemtytest)
                                {
                                    Testid = Convert.ToInt32(cemt.TEST_Id);
                                }
                                txt_TestDetails.Text = string.Empty;
                                for (int j = 0; j < grdIRFR.Rows.Count; j++)
                                {
                                    TextBox txt_IRgrid = (TextBox)grdIRFR.Rows[j].Cells[0].FindControl("txt_IRgrid");
                                    TextBox txt_FRgrid = (TextBox)grdIRFR.Rows[j].Cells[1].FindControl("txt_FRgrid");
                                    if (Convert.ToDecimal(txt_FRgrid.Text) > Convert.ToDecimal(txt_IRgrid.Text))
                                    {
                                        btnResult.Text = Convert.ToString(Convert.ToDecimal(txt_FRgrid.Text) - Convert.ToDecimal(txt_IRgrid.Text));
                                        if (j == 0)
                                        {
                                            Diffrnce = Convert.ToDecimal(btnResult.Text);
                                        }
                                        if (Testid > 0 && btnResult.Text != "Awaited")
                                        {
                                            txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + txt_IRgrid.Text + "~" + txt_FRgrid.Text + "$";
                                        }
                                    }
                                }
                                if (Diffrnce > 0 && Convert.ToDecimal(btnResult.Text) > 0)
                                {
                                    btnResult.Text = Convert.ToString(Convert.ToDecimal(Diffrnce + Convert.ToDecimal(btnResult.Text)) / 2);
                                    if (btnResult.Text == Convert.ToString(Math.Round(Convert.ToDecimal(btnResult.Text))))
                                    {
                                        btnResult.Text = Convert.ToDecimal(btnResult.Text).ToString("00.00");
                                    }
                                    else if (Convert.ToDouble(btnResult.Text) != Math.Floor(Convert.ToDouble(btnResult.Text)) + 0.5 && Convert.ToDouble(btnResult.Text) < Math.Floor(Convert.ToDouble(btnResult.Text)) + 0.5 && (Convert.ToDouble(btnResult.Text) != Math.Floor(Convert.ToDouble(btnResult.Text))))
                                    {
                                        btnResult.Text = Convert.ToString(Math.Floor(Convert.ToDouble(btnResult.Text)) + 0.5);
                                    }
                                    else
                                    {
                                        btnResult.Text = Convert.ToString(Math.Floor(Convert.ToDouble(btnResult.Text)) + 0.5 + 0.5);
                                    }

                                    btnResult.Text = Convert.ToDecimal(btnResult.Text).ToString("00.00");
                                }
                            }
                            else
                            {
                                btnResult.Text = "Awaited";
                                txt_TestDetails.Text = string.Empty;
                            }
                        }
                    }

                    if (divSndnesautoclave.Visible == true)
                    {
                        if (txt_NameOfTest.Text.Trim() == "Soundness  By AutoClave")
                        {
                            if (chk_Awaited.Checked == false)
                            {
                                chk_Awaited.Checked = false;
                                var cemtytest = dc.Test(0, "", 0, "GGBS", txt_NameOfTest.Text.Trim(), 0);
                                foreach (var cemt in cemtytest)
                                {
                                    Testid = Convert.ToInt32(cemt.TEST_Id);
                                }
                                if (Convert.ToDecimal(txt_FR.Text) > Convert.ToDecimal(txt_IR.Text))
                                {
                                    btnResult.Text = Convert.ToDecimal(Convert.ToDecimal(txt_FR.Text) - Convert.ToDecimal(txt_IR.Text)).ToString();
                                    if (btnResult.Text == Convert.ToString(Math.Round(Convert.ToDecimal(btnResult.Text))))
                                    {
                                        btnResult.Text = Convert.ToDecimal(btnResult.Text).ToString("00.00");
                                    }
                                    else if (Convert.ToDouble(btnResult.Text) != Math.Floor(Convert.ToDouble(btnResult.Text)) + 0.5 && Convert.ToDouble(btnResult.Text) < Math.Floor(Convert.ToDouble(btnResult.Text)) + 0.5 && (Convert.ToDouble(btnResult.Text) != Math.Floor(Convert.ToDouble(btnResult.Text))))
                                    {
                                        btnResult.Text = Convert.ToString(Math.Floor(Convert.ToDouble(btnResult.Text)) + 0.5);
                                    }
                                    else
                                    {
                                        btnResult.Text = Convert.ToString(Math.Floor(Convert.ToDouble(btnResult.Text)) + 0.5 + 0.5);
                                    }
                                    btnResult.Text = Convert.ToDecimal(btnResult.Text).ToString("00.00");
                                    if (Testid > 0 && btnResult.Text != "Awaited")
                                    {
                                        txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + txt_IR.Text + "~" + txt_FR.Text;
                                    }
                                }
                            }
                            else
                            {
                                btnResult.Text = "Awaited";
                                txt_TestDetails.Text = string.Empty;
                            }
                        }

                    }
                }
                ModalPopupExtender1.Hide();
            }
        }
        protected Boolean ValidateCalculateData()
        {
            bool valid = true;

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            if (chk_Awaited.Checked == false && divDensity.Visible == true && txt_Weight.Visible == true && txt_Weight.Text == "")
            {
                lblMsg.Text = "Please Enter Weight";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divDensity.Visible == true && txt_Volume.Visible == true && txt_Volume.Text == "")
            {
                lblMsg.Text = "Please Enter Volume";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divSndnesautoclave.Visible == true && txt_IR.Visible == true && txt_IR.Text == "")
            {
                lblMsg.Text = "Please Enter IR";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divSndnesautoclave.Visible == true && txt_FR.Visible == true && txt_FR.Text == "")
            {
                lblMsg.Text = "Please Enter FR";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divIni_finSet.Visible == true && txt_WaterAdded.Visible == true && txt_WaterAdded.Text == "")
            {
                lblMsg.Text = "Please Enter Water Added ";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divIni_finSet.Visible == true && txt_IST.Visible == true && txt_IST.Text == "")
            {
                lblMsg.Text = "Please Enter IST";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divIni_finSet.Visible == true && txt_FST.Visible == true && txt_FST.Text == "")
            {
                lblMsg.Text = "Please Enter FST";
                valid = false;
            }
            else if (chk_Awaited.Checked == false && divSndnesautoclave.Visible == true && valid == true)
            {
                if (Convert.ToDecimal(txt_FR.Text) < Convert.ToDecimal(txt_IR.Text))
                {
                    lblMsg.Text = "FR value should be greater than the IR value";
                    valid = false;
                }
            }
            else if (chk_Awaited.Checked == false && divSndnsByLe_Chateliers.Visible == true && valid == true)
            {
                if (grdIRFR.Visible == true)
                {
                    for (int i = 0; i < grdIRFR.Rows.Count; i++)
                    {
                        TextBox txt_IRgrid = (TextBox)grdIRFR.Rows[i].Cells[0].FindControl("txt_IRgrid");
                        TextBox txt_FRgrid = (TextBox)grdIRFR.Rows[i].Cells[1].FindControl("txt_FRgrid");

                        if (txt_IRgrid.Text == "")
                        {
                            lblMsg.Text = "Please Enter  IR  of Grid for Sr No. " + (i + 1) + ".";
                            valid = false;
                        }
                        else if (txt_FRgrid.Text == "")
                        {
                            lblMsg.Text = "Please Enter  FR  of Grid for Sr No. " + (i + 1) + ".";
                            valid = false;
                        }
                        else if (Convert.ToDecimal(txt_FRgrid.Text) < Convert.ToDecimal(txt_IRgrid.Text))
                        {
                            lblMsg.Text = "FR value should be greater than the IR value for Sr No. " + (i + 1) + ".";
                            valid = false;
                        }
                    }
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + lblMsg.Text + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {
            clsData objcls = new clsData();
            string mySql = "";
            DataTable dt;
            #region GGBS
            mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, cwt.test_name, chead.description, gp.* 
                     from new_gt_app_db.dbo.reference_number ref, new_gt_app_db.dbo.category_header chead, 
                     new_gt_app_db.dbo.ggbs_physical gp, new_gt_app_db.dbo.category_wise_test as cwt
                     where ref.pk_id = chead.reference_no and chead.pk_id = gp.category_header_fk_id
                     and gp.category_wise_test_fk_id = cwt.pk_id
                     and ref.reference_number = '" + txt_ReferenceNo.Text + "' order by gp.quantity_no "; 

            dt = objcls.getGeneralData(mySql);
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                txt_DateOfTesting.Text = Convert.ToDateTime(dt.Rows[0]["date_of_testing"]).ToString("dd/MM/yyyy");
                txt_Description.Text = dt.Rows[0]["description"].ToString();

                if (dt.Rows[0]["witness_by"] != null || dt.Rows[0]["witness_by"].ToString() != "")
                {
                    txt_witnessBy.Text = dt.Rows[0]["witness_by"].ToString();
                    chkWitnessBy.Checked = true;
                    txt_witnessBy.Visible = true;
                }

                for (int i = 0; i < grdGgbsReport.Rows.Count; i++)
                {
                    TextBox txt_NameOfTest = (TextBox)grdGgbsReport.Rows[i].Cells[1].FindControl("txt_NameOfTest");
                    TextBox txt_result = (TextBox)grdGgbsReport.Rows[i].Cells[2].FindControl("txt_result");
                    Button btnResult = (Button)grdGgbsReport.Rows[i].FindControl("btnResult");
                    TextBox txt_TestDetails = (TextBox)grdGgbsReport.Rows[i].Cells[2].FindControl("txt_TestDetails");

                    if (dt.Rows[j]["test_name"].ToString() == "Consistency")
                    {
                        if (txt_NameOfTest.Text.Trim() == "Standard Consistency")
                        {
                            txt_result.Text = dt.Rows[j]["consistency"].ToString();
                            break;
                        }
                    }
                    else if (dt.Rows[j]["test_name"].ToString() == "Density")
                    {   
                        if (txt_NameOfTest.Text.Trim() == "Density")
                        {
                            int Testid = 0;
                            var cemtytest = dc.Test(0, "", 0, "GGBS", txt_NameOfTest.Text.Trim(), 0);
                            foreach (var cemt in cemtytest)
                            {
                                Testid = Convert.ToInt32(cemt.TEST_Id);
                            }
                            if (Convert.ToDecimal(dt.Rows[j]["weight"].ToString()) > 0 && Convert.ToDecimal(dt.Rows[j]["volume"].ToString()) > 0)
                            {
                                btnResult.Text = (Convert.ToDecimal(dt.Rows[j]["weight"].ToString()) / Convert.ToDecimal(dt.Rows[j]["volume"].ToString())).ToString("0.00");
                                txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + dt.Rows[j]["weight"].ToString() + "~" + dt.Rows[j]["volume"].ToString();
                            }
                            break;
                        }
                    }
                    else if (dt.Rows[j]["test_name"].ToString() == "IST-FST")
                    {                        
                        if (txt_NameOfTest.Text.Trim() == "Initial Setting Time")
                        {
                            int Testid = 0;
                            var cemtytest = dc.Test(0, "", 0, "GGBS", txt_NameOfTest.Text.Trim(), 0);
                            foreach (var cemt in cemtytest)
                            {
                                Testid = Convert.ToInt32(cemt.TEST_Id);
                            }
                                                        
                            DateTime WaterAdded = new DateTime();
                            WaterAdded = Convert.ToDateTime(dt.Rows[j]["water_added"].ToString());
                            DateTime IST = new DateTime();
                            IST = Convert.ToDateTime(dt.Rows[j]["initial_setting_time"].ToString());
                            TimeSpan ts = WaterAdded.Subtract(IST);
                            btnResult.Text = Convert.ToString(Math.Abs(ts.TotalMinutes));
                            txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + dt.Rows[j]["water_added"].ToString() + "~" + dt.Rows[j]["initial_setting_time"].ToString() + "~" + dt.Rows[j]["final_setting_time"].ToString();
                        }
                        else if (txt_NameOfTest.Text.Trim() == "Final Setting Time")
                        {
                            int Testid = 0;
                            var cemtytest = dc.Test(0, "", 0, "GGBS", txt_NameOfTest.Text.Trim(), 0);
                            foreach (var cemt in cemtytest)
                            {
                                Testid = Convert.ToInt32(cemt.TEST_Id);
                            }

                            DateTime WaterAdded = new DateTime();
                            WaterAdded = Convert.ToDateTime(dt.Rows[j]["water_added"].ToString());
                            DateTime FST = new DateTime();
                            FST = Convert.ToDateTime(dt.Rows[j]["final_setting_time"].ToString());
                            TimeSpan ts = WaterAdded.Subtract(FST);
                            btnResult.Text = Convert.ToString(Math.Abs(ts.TotalMinutes));
                            txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + dt.Rows[j]["water_added"].ToString() + "~" + dt.Rows[j]["initial_setting_time"].ToString() + "~" + dt.Rows[j]["final_setting_time"].ToString();
                            break;
                        }
                    }
                    else if (dt.Rows[j]["test_name"].ToString() == "")
                    {
                        if (txt_NameOfTest.Text.Trim() == "Soundness By Le-Chateliers Apparatus")
                        {
                            int Testid = 0;
                            var cemtytest = dc.Test(0, "", 0, "GGBS", txt_NameOfTest.Text.Trim(), 0);
                            foreach (var cemt in cemtytest)
                            {
                                Testid = Convert.ToInt32(cemt.TEST_Id);
                            }

                            decimal Diffrnce = 0;
                            if (Convert.ToDecimal(dt.Rows[j]["fr_1"].ToString()) > Convert.ToDecimal(dt.Rows[j]["ir_1"].ToString()))
                            {
                                btnResult.Text = Convert.ToString(Convert.ToDecimal(dt.Rows[j]["fr_1"].ToString()) - Convert.ToDecimal(dt.Rows[j]["ir_1"].ToString()));

                                Diffrnce = Convert.ToDecimal(btnResult.Text);

                                txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + dt.Rows[j]["ir_1"].ToString() + "~" + dt.Rows[j]["fr_1"].ToString() + "$";
                            }
                            if (Convert.ToDecimal(dt.Rows[j]["fr_2"].ToString()) > Convert.ToDecimal(dt.Rows[j]["ir_2"].ToString()))
                            {
                                btnResult.Text = Convert.ToString(Convert.ToDecimal(dt.Rows[j]["fr_2"].ToString()) - Convert.ToDecimal(dt.Rows[j]["ir_2"].ToString()));
                                txt_TestDetails.Text = txt_TestDetails.Text + Testid + "|" + dt.Rows[j]["ir_2"].ToString() + "~" + dt.Rows[j]["fr_2"].ToString() + "$";
                            }
                            if (Diffrnce > 0 && Convert.ToDecimal(btnResult.Text) > 0)
                            {
                                btnResult.Text = Convert.ToString(Convert.ToDecimal(Diffrnce + Convert.ToDecimal(btnResult.Text)) / 2);
                                if (btnResult.Text == Convert.ToString(Math.Round(Convert.ToDecimal(btnResult.Text))))
                                {
                                    btnResult.Text = Convert.ToDecimal(btnResult.Text).ToString("00.00");
                                }
                                else if (Convert.ToDouble(btnResult.Text) != Math.Floor(Convert.ToDouble(btnResult.Text)) + 0.5 && Convert.ToDouble(btnResult.Text) < Math.Floor(Convert.ToDouble(btnResult.Text)) + 0.5 && (Convert.ToDouble(btnResult.Text) != Math.Floor(Convert.ToDouble(btnResult.Text))))
                                {
                                    btnResult.Text = Convert.ToString(Math.Floor(Convert.ToDouble(btnResult.Text)) + 0.5);
                                }
                                else
                                {
                                    btnResult.Text = Convert.ToString(Math.Floor(Convert.ToDouble(btnResult.Text)) + 0.5 + 0.5);
                                }

                                btnResult.Text = Convert.ToDecimal(btnResult.Text).ToString("00.00");
                            }
                            break;
                        }
                    }

                    //if (txt_NameOfTest.Text == "Retained on 45 micron wet sieve")
                    //{
                    //    txt_result.Text = dt.Rows[j]["Retained_on_45_micron_wet_sieve"].ToString();
                    //}
                    ////if (dt.Rows[j]["test_name"].ToString() == "Fineness-Method")
                    ////{
                    //if (txt_NameOfTest.Text == "Fineness By Dry Sieving ")
                    //{
                    //    txt_result.Text = dt.Rows[j]["Fineness_By_Dry_Sieving"].ToString();
                    //}
                    //if (txt_NameOfTest.Text == "Fineness By Blain's Air Permeability Method")
                    //{
                    //    txt_result.Text = dt.Rows[j]["Fineness_By_Blains_Air_Permeability_Method"].ToString();
                    //}
                    
                }

            }
            dt.Dispose();
            #endregion
            objcls = null;
        }
        protected void chkWitnessBy_CheckedChanged(object sender, EventArgs e)
        {
            txt_witnessBy.Text = string.Empty;
            if (chkWitnessBy.Checked)
            {
                txt_witnessBy.Visible = true;
                txt_witnessBy.Focus();
            }
            else
            {
                txt_witnessBy.Visible = false;
            }
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                rpt.PrintSelectedReport(txt_RecType.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", "", "", "", "");
            }
        }               
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ReportStatus.aspx");
        }
        protected void lnkExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportStatus.aspx");
        }

    }
}