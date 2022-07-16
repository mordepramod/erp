using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.IO;

namespace DESPLWEB
{
    public partial class Core_Report : System.Web.UI.Page
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
                }

                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Core Test - Report Entry";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);

                if (txt_RecType.Text != "")
                {

                    DisplayCoreDetails();
                    DisplayGridCoreData();
                    DisplayRemark();

                    if (lblEntry.Text == "Check")
                    {
                        //lblEntry.Text = Session["Check"].ToString();//
                        lblheading.Text = "Core Test - Report Check";
                        //LoadOtherPendingCheckRpt();
                        LoadApproveBy();
                        ViewWitnessBy();
                        lbl_TestedBy.Text = "Approve By";
                    }
                    else
                    {
                        //LoadOtherPendingRpt();
                        LoadTestedBy();
                    }
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

            var reportList = dc.ReferenceNo_View_StatusWise("CR", reportStatus, 0);
            ddl_OtherPendingRpt.DataTextField = "ReferenceNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_OtherPendingRpt.Items.Remove(txt_ReferenceNo.Text);
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
        private void LoadApproveBy()
        {
            if (lblEntry.Text == "Check")
            {
                ddl_TestedBy.DataTextField = "USER_Name_var";
                ddl_TestedBy.DataValueField = "USER_Id";
                var testinguser = dc.ReportStatus_View("", null, null, 0, 0, 1, "", 0, 0, 0);
                ddl_TestedBy.DataSource = testinguser;
                ddl_TestedBy.DataBind();
                ddl_TestedBy.Items.Insert(0, "---Select---");
            }
            else
            {
                LoadTestedBy();
            }
        }

        private void LoadOtherPendingRpt()
        {
            string Refno = txt_ReferenceNo.Text;
            var testinguser = dc.ReportStatus_View("Core Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 1, 0);
            ddl_OtherPendingRpt.DataTextField = "CRINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataValueField = "CRINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataSource = testinguser;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
            ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(Refno);
            if (itemToRemove != null)
            {
                ddl_OtherPendingRpt.Items.Remove(itemToRemove);
            }
        }
        public void DisplayCoreDetails()
        {
            try
            {

                var InwardCore = dc.ReportStatus_View("Core Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
                foreach (var cr in InwardCore)
                {
                    txt_ReferenceNo.Text = cr.CRINWD_ReferenceNo_var.ToString();
                    txt_RecType.Text = cr.CRINWD_RecordType_var.ToString();
                    //if (cr.CRINWD_CastingDate_dt != "NA")
                    //{
                    //    txt_DtOfCasting.Text = cr.CRINWD_CastingDate_dt.ToString();
                    //}
                    //else
                    //{
                    //    txt_DtOfCasting.Text = "NA";
                    //}
                    if (Convert.ToString(cr.CRINWD_TestingDate_dt) != "")
                    {
                        txt_TestingDt.Text = Convert.ToDateTime(cr.CRINWD_TestingDate_dt).ToString("dd/MM/yyyy");
                    }
                    if (txt_TestingDt.Text == "" || lblEntry.Text == "Enter")
                    {
                        txt_TestingDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    txt_GradeOfConcrete.Text = cr.CRINWD_Grade_var.ToString();
                    txt_Description.Text = cr.CRINWD_Description_var.ToString();
                    if (Convert.ToString(cr.CRINWD_SpecimenExtDate_dt) != "")
                    {
                        txt_DtSpecExtraction.Text = (cr.CRINWD_SpecimenExtDate_dt).ToString();
                    }
                    if (Convert.ToString(cr.CRINWD_PulseVelocity_bit) == "True" && Convert.ToString(cr.CRINWD_PulseVelocity_bit) != string.Empty)
                    {
                        Chk_PrntPulseVel.Checked = true;
                    }
                    txt_ConcreteMember.Text = cr.CRINWD_ConcreteMember_var.ToString();
                    txt_ReportNo.Text = cr.CRINWD_SetOfRecord_var.ToString();

                    if (ddl_NablScope.Items.FindByValue(cr.CRINWD_NablScope_var) != null)
                    {
                        ddl_NablScope.SelectedValue = Convert.ToString(cr.CRINWD_NablScope_var);
                    }
                    if (Convert.ToString(cr.CRINWD_NablLocation_int) != null && Convert.ToString(cr.CRINWD_NablLocation_int) != "")
                    {
                        ddl_NABLLocation.SelectedValue = Convert.ToString(cr.CRINWD_NablLocation_int);
                    }

                    if (Convert.ToString(cr.CRINWD_CurrCondition_var) != null)
                    {
                        ddl_CurrCondition.ClearSelection();
                        ddl_CurrCondition.Items.FindByText(cr.CRINWD_CurrCondition_var).Selected = true;
                    }
                    if (cr.CRINWD_Cylinder_bit != null)
                    {
                        if (cr.CRINWD_Cylinder_bit == true)
                        {
                            chkCylinder.Checked = true;
                        }
                        else
                        {
                            chkCylinder.Checked = false;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            finally
            {

            }
        }
        public void DisplayGridCoreData()
        {
            int i = 0;
            string RowNo = "";
            int RowIndex = 0;
            var cr = dc.TestDetail_Title_View(txt_ReferenceNo.Text, 0, "CR", false);
            foreach (var cor in cr)
            {
                AddRowEnterReportCoreInward();
                TextBox txt_Description = (TextBox)grdCoreTestInward.Rows[i].Cells[4].FindControl("txt_Description");
                DropDownList ddl_Grade = (DropDownList)grdCoreTestInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                TextBox txt_CastingDt = (TextBox)grdCoreTestInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                TextBox txt_Dia = (TextBox)grdCoreTestInward.Rows[i].Cells[7].FindControl("txt_Dia");
                TextBox txt_length = (TextBox)grdCoreTestInward.Rows[i].Cells[8].FindControl("txt_length");
                TextBox txt_lengthwthCaping = (TextBox)grdCoreTestInward.Rows[i].Cells[9].FindControl("txt_lengthwthCaping");
                TextBox txt_weight = (TextBox)grdCoreTestInward.Rows[i].Cells[10].FindControl("txt_weight");
                TextBox txt_Reading = (TextBox)grdCoreTestInward.Rows[i].Cells[11].FindControl("txt_Reading");
                TextBox txt_PulseVel = (TextBox)grdCoreTestInward.Rows[i].Cells[12].FindControl("txt_PulseVel");
                DropDownList ddl_ModeOfFailure = (DropDownList)grdCoreTestInward.Rows[i].Cells[13].FindControl("ddl_ModeOfFailure");
                TextBox txt_age = (TextBox)grdCoreTestInward.Rows[i].Cells[14].FindControl("txt_age");
                TextBox txt_CsArea = (TextBox)grdCoreTestInward.Rows[i].Cells[15].FindControl("txt_CsArea");
                TextBox txt_Density = (TextBox)grdCoreTestInward.Rows[i].Cells[16].FindControl("txt_Density");
                TextBox txt_CompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[17].FindControl("txt_CompStr");
                TextBox txt_CorrCompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[18].FindControl("txt_CorrCompStr");
                TextBox txt_EquCubeStr = (TextBox)grdCoreTestInward.Rows[i].Cells[19].FindControl("txt_EquCubeStr");

                if (Convert.ToString(cor.Description_var) != "")
                {
                    txt_Description.Text = Convert.ToString(cor.Description_var);
                }
                else
                {
                    // ShowMerge(i);
                    RowNo = RowNo + " " + i;
                    if (Convert.ToString(cor.TitleId_int) != "")
                    {
                        if (Convert.ToInt32(cor.TitleId_int) > 0)
                        {
                            var crr = dc.TestDetail_Title_View(txt_ReferenceNo.Text, Convert.ToInt32(cor.TitleId_int), "CR", false);
                            foreach (var title in crr)
                            {
                                txt_Description.Text = title.TitleDesc_var.ToString();
                            }
                        }
                    }

                }
                if (Convert.ToString(cor.Grade_var) != "")
                {
                    ddl_Grade.SelectedItem.Text = Convert.ToString(cor.Grade_var);
                }
                if (Convert.ToString(cor.Castingdate_var) != "")
                {
                    txt_CastingDt.Text = Convert.ToString(cor.Castingdate_var);
                }
                if (Convert.ToString(cor.Dia_int) != "" && Convert.ToInt32(cor.Dia_int) > 0)
                {
                    txt_Dia.Text = Convert.ToString(cor.Dia_int);
                }
                if (Convert.ToString(cor.Length_num) != "" && Convert.ToInt32(cor.Length_num) > 0)
                {
                    txt_length.Text = Convert.ToString(cor.Length_num);
                }
                if (Convert.ToString(cor.LengthCaping_num) != "" && Convert.ToInt32(cor.LengthCaping_num) > 0)
                {
                    txt_lengthwthCaping.Text = Convert.ToString(cor.LengthCaping_num);
                }
                if (Convert.ToString(cor.Reading_dec) != "" && Convert.ToInt32(cor.Reading_dec) > 0)
                {
                    txt_Reading.Text = Convert.ToString(cor.Reading_dec);
                }
                if (Convert.ToString(cor.Weight_dec) != "" && Convert.ToInt32(cor.Weight_dec) > 0)
                {
                    txt_weight.Text = Convert.ToString(cor.Weight_dec);
                }
                if (Convert.ToString(cor.PulseVelocity_dec) != "" && Convert.ToInt32(cor.PulseVelocity_dec) > 0)
                {
                    txt_PulseVel.Text = Convert.ToString(cor.PulseVelocity_dec);
                }
                if (Convert.ToString(cor.ModeOfFailure_var) != "")
                {
                    ddl_ModeOfFailure.SelectedItem.Text = cor.ModeOfFailure_var.ToString();
                }
                if (Convert.ToString(cor.Age_var) != "")
                {
                    txt_age.Text = cor.Age_var.ToString();
                }
                if (Convert.ToString(cor.CsArea_dec) != "" && Convert.ToInt32(cor.CsArea_dec) > 0)
                {
                    txt_CsArea.Text = cor.CsArea_dec.ToString();
                }
                if (Convert.ToString(cor.Density_dec) != "" && Convert.ToInt32(cor.Density_dec) > 0)
                {
                    txt_Density.Text = cor.Density_dec.ToString();
                }
                if (Convert.ToString(cor.CompStr_dec) != "" && Convert.ToInt32(cor.CompStr_dec) > 0)
                {
                    txt_CompStr.Text = cor.CompStr_dec.ToString();
                }
                if (Convert.ToString(cor.CorrCompStr_dec) != "" && Convert.ToInt32(cor.CorrCompStr_dec) > 0)
                {
                    txt_CorrCompStr.Text = cor.CorrCompStr_dec.ToString();
                }
                if (Convert.ToString(cor.EquCubeStr_dec) != "" && Convert.ToInt32(cor.EquCubeStr_dec) > 0)
                {
                    txt_EquCubeStr.Text = cor.EquCubeStr_dec.ToString();
                }
                i++;
            }
            if (RowNo != "")
            {
                string[] rowindexx = Convert.ToString(RowNo).Split(' ');
                foreach (var rowx in rowindexx)
                {
                    if (rowx != "")
                    {
                        RowIndex = Convert.ToInt32(rowx);
                        if (RowIndex <= grdCoreTestInward.Rows.Count - 1)
                        {
                            ShowMerge(RowIndex);
                        }
                    }
                }
            }
            if (grdCoreTestInward.Rows.Count <= 0)
            {
                AddRowEnterReportCoreInward();
            }
        }
        protected void imgBtnCoreTestAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowEnterReportCoreInward();
            ShowMergeeRow();

        }
        protected void imgBtnCoreTestDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdCoreTestInward.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdCoreTestInward.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowReportCoreInward(gvr.RowIndex);
                ShowMergeeRow();

            }
        }
        public void ShowMergeeRow()
        {
            int rowindex = 0;
            if (grdCoreTestInward.Rows.Count > 1)
            {
                if (ViewState["RowNo"] != null)
                {
                    string[] rowindexx = Convert.ToString(ViewState["RowNo"]).Split(' ');
                    foreach (var rowx in rowindexx)
                    {
                        if (rowx != "")
                        {
                            if (int.TryParse(rowx, out rowindex))
                            {
                                rowindex = Convert.ToInt32(rowx);
                                if (rowindex < grdCoreTestInward.Rows.Count - 1)
                                {
                                    ShowMerge(rowindex);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ViewState["RowNo"] = null;
            }
        }
        protected void DeleteRowReportCoreInward(int rowIndex)
        {
            GetCurrentDataCoreTestInward();
            DataTable dt = ViewState["CoreTestTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CoreTestTable"] = dt;
            grdCoreTestInward.DataSource = dt;
            grdCoreTestInward.DataBind();
            SetPreviousDataCoreTestInward();
        }
        protected void AddRowEnterReportCoreInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CoreTestTable"] != null)
            {
                GetCurrentDataCoreTestInward();
                dt = (DataTable)ViewState["CoreTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Description", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_Grade", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CastingDt", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Dia", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_length", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_lengthwthCaping", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_weight", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Reading", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_PulseVel", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_ModeOfFailure", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_age", typeof(string)));//
                dt.Columns.Add(new DataColumn("txt_CsArea", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Density", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CompStr", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CorrCompStr", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_EquCubeStr", typeof(string)));//

            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_Description"] = string.Empty;
            dr["ddl_Grade"] = string.Empty;
            dr["txt_CastingDt"] = string.Empty;
            dr["txt_Dia"] = string.Empty;
            dr["txt_length"] = string.Empty;
            dr["txt_lengthwthCaping"] = string.Empty;
            dr["txt_weight"] = string.Empty;
            dr["txt_Reading"] = string.Empty;
            dr["txt_PulseVel"] = string.Empty;
            dr["ddl_ModeOfFailure"] = string.Empty;
            dr["txt_age"] = string.Empty;
            dr["txt_CsArea"] = string.Empty;
            dr["txt_Density"] = string.Empty;
            dr["txt_CompStr"] = string.Empty;
            dr["txt_CorrCompStr"] = string.Empty;
            dr["txt_EquCubeStr"] = string.Empty;

            dt.Rows.Add(dr);

            ViewState["CoreTestTable"] = dt;
            grdCoreTestInward.DataSource = dt;
            grdCoreTestInward.DataBind();
            SetPreviousDataCoreTestInward();

        }
        protected void GetCurrentDataCoreTestInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Description", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_Grade", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CastingDt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Dia", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_length", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_lengthwthCaping", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_weight", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Reading", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_PulseVel", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_ModeOfFailure", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_age", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CsArea", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Density", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CompStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CorrCompStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_EquCubeStr", typeof(string)));

            for (int i = 0; i < grdCoreTestInward.Rows.Count; i++)
            {
                TextBox txt_Description = (TextBox)grdCoreTestInward.Rows[i].Cells[4].FindControl("txt_Description");
                DropDownList ddl_Grade = (DropDownList)grdCoreTestInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                TextBox txt_CastingDt = (TextBox)grdCoreTestInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                TextBox txt_Dia = (TextBox)grdCoreTestInward.Rows[i].Cells[7].FindControl("txt_Dia");
                TextBox txt_length = (TextBox)grdCoreTestInward.Rows[i].Cells[8].FindControl("txt_length");
                TextBox txt_lengthwthCaping = (TextBox)grdCoreTestInward.Rows[i].Cells[9].FindControl("txt_lengthwthCaping");
                TextBox txt_weight = (TextBox)grdCoreTestInward.Rows[i].Cells[10].FindControl("txt_weight");
                TextBox txt_Reading = (TextBox)grdCoreTestInward.Rows[i].Cells[11].FindControl("txt_Reading");
                TextBox txt_PulseVel = (TextBox)grdCoreTestInward.Rows[i].Cells[12].FindControl("txt_PulseVel");
                DropDownList ddl_ModeOfFailure = (DropDownList)grdCoreTestInward.Rows[i].Cells[13].FindControl("ddl_ModeOfFailure");
                TextBox txt_age = (TextBox)grdCoreTestInward.Rows[i].Cells[14].FindControl("txt_age");
                TextBox txt_CsArea = (TextBox)grdCoreTestInward.Rows[i].Cells[15].FindControl("txt_CsArea");
                TextBox txt_Density = (TextBox)grdCoreTestInward.Rows[i].Cells[16].FindControl("txt_Density");
                TextBox txt_CompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[17].FindControl("txt_CompStr");
                TextBox txt_CorrCompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[18].FindControl("txt_CorrCompStr");
                TextBox txt_EquCubeStr = (TextBox)grdCoreTestInward.Rows[i].Cells[19].FindControl("txt_EquCubeStr");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_Description"] = txt_Description.Text;
                drRow["ddl_Grade"] = ddl_Grade.Text;
                drRow["txt_CastingDt"] = txt_CastingDt.Text;
                drRow["txt_Dia"] = txt_Dia.Text;
                drRow["txt_length"] = txt_length.Text;
                drRow["txt_lengthwthCaping"] = txt_lengthwthCaping.Text;
                drRow["txt_weight"] = txt_weight.Text;
                drRow["txt_Reading"] = txt_Reading.Text;
                drRow["txt_PulseVel"] = txt_PulseVel.Text;
                drRow["ddl_ModeOfFailure"] = ddl_ModeOfFailure.Text;
                drRow["txt_age"] = txt_age.Text;
                drRow["txt_CsArea"] = txt_CsArea.Text;
                drRow["txt_Density"] = txt_Density.Text;
                drRow["txt_CompStr"] = txt_CompStr.Text;
                drRow["txt_CorrCompStr"] = txt_CorrCompStr.Text;
                drRow["txt_EquCubeStr"] = txt_EquCubeStr.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CoreTestTable"] = dtTable;

        }
        protected void SetPreviousDataCoreTestInward()
        {
            DataTable dt = (DataTable)ViewState["CoreTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Description = (TextBox)grdCoreTestInward.Rows[i].Cells[4].FindControl("txt_Description");
                DropDownList ddl_Grade = (DropDownList)grdCoreTestInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                TextBox txt_CastingDt = (TextBox)grdCoreTestInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                TextBox txt_Dia = (TextBox)grdCoreTestInward.Rows[i].Cells[7].FindControl("txt_Dia");
                TextBox txt_length = (TextBox)grdCoreTestInward.Rows[i].Cells[8].FindControl("txt_length");
                TextBox txt_lengthwthCaping = (TextBox)grdCoreTestInward.Rows[i].Cells[9].FindControl("txt_lengthwthCaping");
                TextBox txt_weight = (TextBox)grdCoreTestInward.Rows[i].Cells[10].FindControl("txt_weight");
                TextBox txt_Reading = (TextBox)grdCoreTestInward.Rows[i].Cells[11].FindControl("txt_Reading");
                TextBox txt_PulseVel = (TextBox)grdCoreTestInward.Rows[i].Cells[12].FindControl("txt_PulseVel");
                DropDownList ddl_ModeOfFailure = (DropDownList)grdCoreTestInward.Rows[i].Cells[13].FindControl("ddl_ModeOfFailure");
                TextBox txt_age = (TextBox)grdCoreTestInward.Rows[i].Cells[14].FindControl("txt_age");
                TextBox txt_CsArea = (TextBox)grdCoreTestInward.Rows[i].Cells[15].FindControl("txt_CsArea");
                TextBox txt_Density = (TextBox)grdCoreTestInward.Rows[i].Cells[16].FindControl("txt_Density");
                TextBox txt_CompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[17].FindControl("txt_CompStr");
                TextBox txt_CorrCompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[18].FindControl("txt_CorrCompStr");
                TextBox txt_EquCubeStr = (TextBox)grdCoreTestInward.Rows[i].Cells[19].FindControl("txt_EquCubeStr");

                grdCoreTestInward.Rows[i].Cells[3].Text = (i + 1).ToString();
                txt_Description.Text = dt.Rows[i]["txt_Description"].ToString();
                ddl_Grade.Text = dt.Rows[i]["ddl_Grade"].ToString();
                txt_CastingDt.Text = dt.Rows[i]["txt_CastingDt"].ToString();
                txt_Dia.Text = dt.Rows[i]["txt_Dia"].ToString();
                txt_length.Text = dt.Rows[i]["txt_length"].ToString();
                txt_lengthwthCaping.Text = dt.Rows[i]["txt_lengthwthCaping"].ToString();
                txt_weight.Text = dt.Rows[i]["txt_weight"].ToString();
                txt_Reading.Text = dt.Rows[i]["txt_Reading"].ToString();
                txt_PulseVel.Text = dt.Rows[i]["txt_PulseVel"].ToString();
                ddl_ModeOfFailure.Text = dt.Rows[i]["ddl_ModeOfFailure"].ToString();
                txt_age.Text = dt.Rows[i]["txt_age"].ToString();
                txt_CsArea.Text = dt.Rows[i]["txt_CsArea"].ToString();
                txt_Density.Text = dt.Rows[i]["txt_Density"].ToString();
                txt_CompStr.Text = dt.Rows[i]["txt_CompStr"].ToString();
                txt_CorrCompStr.Text = dt.Rows[i]["txt_CorrCompStr"].ToString();
                txt_EquCubeStr.Text = dt.Rows[i]["txt_EquCubeStr"].ToString();
            }

        }
        private void AddNewRowToGrid(int rowIndex)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CoreTestTable"] != null)
            {
                GetData(rowIndex);
                dt = (DataTable)ViewState["CoreTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Description", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_Grade", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CastingDt", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Dia", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_length", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_lengthwthCaping", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_weight", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Reading", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_PulseVel", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_ModeOfFailure", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_age", typeof(string)));//
                dt.Columns.Add(new DataColumn("txt_CsArea", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Density", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CompStr", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CorrCompStr", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_EquCubeStr", typeof(string)));//
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_Description"] = string.Empty;
            dr["ddl_Grade"] = string.Empty;
            dr["txt_CastingDt"] = string.Empty;
            dr["txt_Dia"] = string.Empty;
            dr["txt_length"] = string.Empty;
            dr["txt_lengthwthCaping"] = string.Empty;
            dr["txt_weight"] = string.Empty;
            dr["txt_Reading"] = string.Empty;
            dr["txt_PulseVel"] = string.Empty;
            dr["ddl_ModeOfFailure"] = string.Empty;
            dr["txt_age"] = string.Empty;
            dr["txt_CsArea"] = string.Empty;
            dr["txt_Density"] = string.Empty;
            dr["txt_CompStr"] = string.Empty;
            dr["txt_CorrCompStr"] = string.Empty;
            dr["txt_EquCubeStr"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["CoreTestTable"] = dt;
            grdCoreTestInward.DataSource = dt;
            grdCoreTestInward.DataBind();
            SetPreviousData(rowIndex);
        }
        private void GetData(int rowIndex)
        {
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Description", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_Grade", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CastingDt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Dia", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_length", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_lengthwthCaping", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_weight", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Reading", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_PulseVel", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_ModeOfFailure", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_age", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CsArea", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Density", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CompStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CorrCompStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_EquCubeStr", typeof(string)));

            for (int i = 0; i < grdCoreTestInward.Rows.Count; i++)
            {
                TextBox txt_Description = (TextBox)grdCoreTestInward.Rows[i].Cells[4].FindControl("txt_Description");
                DropDownList ddl_Grade = (DropDownList)grdCoreTestInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                TextBox txt_CastingDt = (TextBox)grdCoreTestInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                TextBox txt_Dia = (TextBox)grdCoreTestInward.Rows[i].Cells[7].FindControl("txt_Dia");
                TextBox txt_length = (TextBox)grdCoreTestInward.Rows[i].Cells[8].FindControl("txt_length");
                TextBox txt_lengthwthCaping = (TextBox)grdCoreTestInward.Rows[i].Cells[9].FindControl("txt_lengthwthCaping");
                TextBox txt_weight = (TextBox)grdCoreTestInward.Rows[i].Cells[10].FindControl("txt_weight");
                TextBox txt_Reading = (TextBox)grdCoreTestInward.Rows[i].Cells[11].FindControl("txt_Reading");
                TextBox txt_PulseVel = (TextBox)grdCoreTestInward.Rows[i].Cells[12].FindControl("txt_PulseVel");
                DropDownList ddl_ModeOfFailure = (DropDownList)grdCoreTestInward.Rows[i].Cells[13].FindControl("ddl_ModeOfFailure");
                TextBox txt_age = (TextBox)grdCoreTestInward.Rows[i].Cells[14].FindControl("txt_age");
                TextBox txt_CsArea = (TextBox)grdCoreTestInward.Rows[i].Cells[15].FindControl("txt_CsArea");
                TextBox txt_Density = (TextBox)grdCoreTestInward.Rows[i].Cells[16].FindControl("txt_Density");
                TextBox txt_CompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[17].FindControl("txt_CompStr");
                TextBox txt_CorrCompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[18].FindControl("txt_CorrCompStr");
                TextBox txt_EquCubeStr = (TextBox)grdCoreTestInward.Rows[i].Cells[19].FindControl("txt_EquCubeStr");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_Description"] = txt_Description.Text;
                drRow["ddl_Grade"] = ddl_Grade.Text;
                drRow["txt_CastingDt"] = txt_CastingDt.Text;
                drRow["txt_Dia"] = txt_Dia.Text;
                drRow["txt_length"] = txt_length.Text;
                drRow["txt_lengthwthCaping"] = txt_lengthwthCaping.Text;
                drRow["txt_weight"] = txt_weight.Text;
                drRow["txt_Reading"] = txt_Reading.Text;
                drRow["txt_PulseVel"] = txt_PulseVel.Text;
                drRow["ddl_ModeOfFailure"] = ddl_ModeOfFailure.Text;
                drRow["txt_age"] = txt_age.Text;
                drRow["txt_CsArea"] = txt_CsArea.Text;
                drRow["txt_Density"] = txt_Density.Text;
                drRow["txt_CompStr"] = txt_CompStr.Text;
                drRow["txt_CorrCompStr"] = txt_CorrCompStr.Text;
                drRow["txt_EquCubeStr"] = txt_EquCubeStr.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CoreTestTable"] = dtTable;
        }
        private void SetPreviousData(int rowIndex)
        {
            DataTable dt = (DataTable)ViewState["CoreTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Description = (TextBox)grdCoreTestInward.Rows[i].Cells[4].FindControl("txt_Description");
                DropDownList ddl_Grade = (DropDownList)grdCoreTestInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                TextBox txt_CastingDt = (TextBox)grdCoreTestInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                TextBox txt_Dia = (TextBox)grdCoreTestInward.Rows[i].Cells[7].FindControl("txt_Dia");
                TextBox txt_length = (TextBox)grdCoreTestInward.Rows[i].Cells[8].FindControl("txt_length");
                TextBox txt_lengthwthCaping = (TextBox)grdCoreTestInward.Rows[i].Cells[9].FindControl("txt_lengthwthCaping");
                TextBox txt_weight = (TextBox)grdCoreTestInward.Rows[i].Cells[10].FindControl("txt_weight");
                TextBox txt_Reading = (TextBox)grdCoreTestInward.Rows[i].Cells[11].FindControl("txt_Reading");
                TextBox txt_PulseVel = (TextBox)grdCoreTestInward.Rows[i].Cells[12].FindControl("txt_PulseVel");
                DropDownList ddl_ModeOfFailure = (DropDownList)grdCoreTestInward.Rows[i].Cells[13].FindControl("ddl_ModeOfFailure");
                TextBox txt_age = (TextBox)grdCoreTestInward.Rows[i].Cells[14].FindControl("txt_age");
                TextBox txt_CsArea = (TextBox)grdCoreTestInward.Rows[i].Cells[15].FindControl("txt_CsArea");
                TextBox txt_Density = (TextBox)grdCoreTestInward.Rows[i].Cells[16].FindControl("txt_Density");
                TextBox txt_CompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[17].FindControl("txt_CompStr");
                TextBox txt_CorrCompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[18].FindControl("txt_CorrCompStr");
                TextBox txt_EquCubeStr = (TextBox)grdCoreTestInward.Rows[i].Cells[19].FindControl("txt_EquCubeStr");

                grdCoreTestInward.Rows[i].Cells[3].Text = (i + 1).ToString();
                txt_Description.Text = dt.Rows[i]["txt_Description"].ToString();
                ddl_Grade.Text = dt.Rows[i]["ddl_Grade"].ToString();
                txt_CastingDt.Text = dt.Rows[i]["txt_CastingDt"].ToString();
                txt_Dia.Text = dt.Rows[i]["txt_Dia"].ToString();
                txt_length.Text = dt.Rows[i]["txt_length"].ToString();
                txt_lengthwthCaping.Text = dt.Rows[i]["txt_lengthwthCaping"].ToString();
                txt_weight.Text = dt.Rows[i]["txt_weight"].ToString();
                txt_Reading.Text = dt.Rows[i]["txt_Reading"].ToString();
                txt_PulseVel.Text = dt.Rows[i]["txt_PulseVel"].ToString();
                ddl_ModeOfFailure.Text = dt.Rows[i]["ddl_ModeOfFailure"].ToString();
                txt_age.Text = dt.Rows[i]["txt_age"].ToString();
                txt_CsArea.Text = dt.Rows[i]["txt_CsArea"].ToString();
                txt_Density.Text = dt.Rows[i]["txt_Density"].ToString();
                txt_CompStr.Text = dt.Rows[i]["txt_CompStr"].ToString();
                txt_CorrCompStr.Text = dt.Rows[i]["txt_CorrCompStr"].ToString();
                txt_EquCubeStr.Text = dt.Rows[i]["txt_EquCubeStr"].ToString();
            }

        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                DateTime TestingDt = DateTime.ParseExact(txt_TestingDt.Text, "dd/MM/yyyy", null);
                //DateTime SpecimenExDt = DateTime.ParseExact(txt_DtSpecExtraction.Text, "dd/MM/yyyy", null);
                bool PrintPulseVel = false;
                if (Chk_PrntPulseVel.Checked)
                {
                    PrintPulseVel = true;
                }
                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.CoreTestInward_Update(txt_ReferenceNo.Text, 2, TestingDt, txt_DtSpecExtraction.Text, "", txt_Description.Text, txt_witnessBy.Text, txt_GradeOfConcrete.Text, txt_ConcreteMember.Text, ddl_CurrCondition.SelectedItem.Text, PrintPulseVel, "CR");
                    dc.ReportDetails_Update("CR", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                    dc.MISDetail_Update(0, "CR", txt_ReferenceNo.Text, "CR", null, true, false, false, false, false, false, false);
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.CoreTestInward_Update(txt_ReferenceNo.Text, 3, TestingDt, txt_DtSpecExtraction.Text, "", txt_Description.Text, txt_witnessBy.Text, txt_GradeOfConcrete.Text, txt_ConcreteMember.Text, ddl_CurrCondition.SelectedItem.Text, PrintPulseVel, "CR");
                    dc.ReportDetails_Update("CR", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "CR", txt_ReferenceNo.Text, "CR", null, false, true, false, false, false, false, false);
                }

                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "CR", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                dc.CoreTestDetail_Update(txt_ReferenceNo.Text, "", "", "", "", 0, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, 0, 0, true);
                int TitleId = 0;
                dc.Title_Update(txt_ReferenceNo.Text, "", "CR", 0, true);//delete title
                for (int i = 0; i < grdCoreTestInward.Rows.Count; i++)
                {
                    TextBox txt_Description = (TextBox)grdCoreTestInward.Rows[i].Cells[4].FindControl("txt_Description");
                    DropDownList ddl_Grade = (DropDownList)grdCoreTestInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                    TextBox txt_CastingDt = (TextBox)grdCoreTestInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                    TextBox txt_Dia = (TextBox)grdCoreTestInward.Rows[i].Cells[7].FindControl("txt_Dia");
                    TextBox txt_length = (TextBox)grdCoreTestInward.Rows[i].Cells[8].FindControl("txt_length");
                    TextBox txt_lengthwthCaping = (TextBox)grdCoreTestInward.Rows[i].Cells[9].FindControl("txt_lengthwthCaping");
                    TextBox txt_weight = (TextBox)grdCoreTestInward.Rows[i].Cells[10].FindControl("txt_weight");
                    TextBox txt_Reading = (TextBox)grdCoreTestInward.Rows[i].Cells[11].FindControl("txt_Reading");
                    TextBox txt_PulseVel = (TextBox)grdCoreTestInward.Rows[i].Cells[12].FindControl("txt_PulseVel");
                    DropDownList ddl_ModeOfFailure = (DropDownList)grdCoreTestInward.Rows[i].Cells[13].FindControl("ddl_ModeOfFailure");
                    TextBox txt_age = (TextBox)grdCoreTestInward.Rows[i].Cells[14].FindControl("txt_age");
                    TextBox txt_CsArea = (TextBox)grdCoreTestInward.Rows[i].Cells[15].FindControl("txt_CsArea");
                    TextBox txt_Density = (TextBox)grdCoreTestInward.Rows[i].Cells[16].FindControl("txt_Density");
                    TextBox txt_CompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[17].FindControl("txt_CompStr");
                    TextBox txt_CorrCompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[18].FindControl("txt_CorrCompStr");
                    TextBox txt_EquCubeStr = (TextBox)grdCoreTestInward.Rows[i].Cells[19].FindControl("txt_EquCubeStr");

                    if (
                        grdCoreTestInward.Rows[i].Cells[5].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[6].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[7].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[8].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[9].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[10].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[11].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[12].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[13].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[14].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[15].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[16].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[17].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[18].Visible == true &&
                        grdCoreTestInward.Rows[i].Cells[19].Visible == true)
                    {

                        dc.CoreTestDetail_Update(txt_ReferenceNo.Text, txt_Description.Text, ddl_Grade.SelectedItem.Text, txt_CastingDt.Text, txt_age.Text,
                                                Convert.ToDecimal(txt_Dia.Text), Convert.ToDecimal(txt_length.Text), Convert.ToDecimal(txt_lengthwthCaping.Text),
                                                Convert.ToDecimal(txt_weight.Text), Convert.ToDecimal(txt_Reading.Text), Convert.ToDecimal(txt_PulseVel.Text),
                                                ddl_ModeOfFailure.SelectedItem.Text, Convert.ToDecimal(txt_CsArea.Text), Convert.ToDecimal(txt_Density.Text),
                                                Convert.ToDecimal(txt_CompStr.Text), Convert.ToDecimal(txt_CorrCompStr.Text), Convert.ToDecimal(txt_EquCubeStr.Text), TitleId, false);

                    }
                    else
                    {

                        dc.Title_Update(txt_ReferenceNo.Text, txt_Description.Text, "CR", 0, false);
                        var t = dc.TestDetail_Title_View(txt_ReferenceNo.Text, 0, "CR", true);
                        foreach (var tt in t)
                        {
                            TitleId = Convert.ToInt32(tt.TitleId_int);

                        }
                        dc.CoreTestDetail_Update(txt_ReferenceNo.Text, "", "", "", "",
                                             0, 0, 0,
                                            0, 0, 0,
                                            "", 0, 0,
                                             0, 0, 0,
                                                TitleId, false);
                    }



                }
                clsData objData = new clsData();
                objData.updateCylinder(txt_ReferenceNo.Text, chkCylinder.Checked );
                #region Remark Gridview
                int RemarkId = 0;
                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "CR", true);
                for (int i = 0; i < grdRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AllRemark_View(txt_REMARK.Text, "", 0, "CR");
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.CR_RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "CR");
                            foreach (var c in chkId)
                            {
                                if (c.CRDetail_RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "CR", false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.AllRemark_Update(0, txt_REMARK.Text, "CR");
                            var chc = dc.AllRemark_View(txt_REMARK.Text, "", 0, "CR");
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.CR_RemarkId_int);
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "CR", false);
                            }
                        }
                    }
                }
                #endregion
                lnkPrint.Visible = true;

                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkSave.Enabled = false;
            }
        }
        public void SaveTitleDesc()
        {
            dc.Title_Update(txt_ReferenceNo.Text, "", "CR", 0, true);//delete title
            for (int i = 0; i < grdCoreTestInward.Rows.Count; i++)
            {
                TextBox txt_Description = (TextBox)grdCoreTestInward.Rows[i].Cells[1].FindControl("txt_Description");
                if (
                  grdCoreTestInward.Rows[i].Cells[5].Visible == false &&
                  grdCoreTestInward.Rows[i].Cells[6].Visible == false &&
                  grdCoreTestInward.Rows[i].Cells[7].Visible == false &&
                  grdCoreTestInward.Rows[i].Cells[8].Visible == false &&
                  grdCoreTestInward.Rows[i].Cells[9].Visible == false &&
                  grdCoreTestInward.Rows[i].Cells[10].Visible == false &&
                  grdCoreTestInward.Rows[i].Cells[11].Visible == false &&
                  grdCoreTestInward.Rows[i].Cells[12].Visible == false &&
                  grdCoreTestInward.Rows[i].Cells[13].Visible == false &&
                  grdCoreTestInward.Rows[i].Cells[14].Visible == false &&
                  grdCoreTestInward.Rows[i].Cells[15].Visible == false &&
                  grdCoreTestInward.Rows[i].Cells[16].Visible == false &&
                  grdCoreTestInward.Rows[i].Cells[17].Visible == false &&
                  grdCoreTestInward.Rows[i].Cells[18].Visible == false &&
                   grdCoreTestInward.Rows[i].Cells[19].Visible == false)
                {
                    dc.Title_Update(txt_ReferenceNo.Text, txt_Description.Text, "CR", 0, false);
                }
            }
        }
        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            lblMsg.ForeColor = System.Drawing.Color.Red;
            string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime TestingDate = DateTime.ParseExact(txt_TestingDt.Text, "dd/MM/yyyy", null);
            DateTime currentDate1 = DateTime.ParseExact(CurrentDate, "dd/MM/yyyy", null);

            if (txt_DtSpecExtraction.Text == "")
            {
                lblMsg.Text = "Enter Date of Spec. Extraction";
                txt_DtSpecExtraction.Focus();
                valid = false;
            }
            else if (TestingDate > currentDate1)
            {
                lblMsg.Text = "Testing Date should be always less than or equal to the Current Date.";
                txt_TestingDt.Focus();
                valid = false;
            }
            else if (ddl_CurrCondition.SelectedItem.Text == "Select")
            {
                lblMsg.Text = "Please Select the Curring Condition";
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
            else if (chk_WitnessBy.Checked && txt_witnessBy.Text == "" && txt_witnessBy.Enabled == true)
            {
                lblMsg.Text = "Please Enter Witness By";
                txt_witnessBy.Focus();
                valid = false;
            }
            else if (valid == true)
            {
                for (int i = 0; i < grdCoreTestInward.Rows.Count; i++)
                {
                    TextBox txt_Description = (TextBox)grdCoreTestInward.Rows[i].Cells[4].FindControl("txt_Description");
                    DropDownList ddl_Grade = (DropDownList)grdCoreTestInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                    TextBox txt_CastingDt = (TextBox)grdCoreTestInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                    TextBox txt_Dia = (TextBox)grdCoreTestInward.Rows[i].Cells[7].FindControl("txt_Dia");
                    TextBox txt_length = (TextBox)grdCoreTestInward.Rows[i].Cells[8].FindControl("txt_length");
                    TextBox txt_lengthwthCaping = (TextBox)grdCoreTestInward.Rows[i].Cells[9].FindControl("txt_lengthwthCaping");
                    TextBox txt_weight = (TextBox)grdCoreTestInward.Rows[i].Cells[10].FindControl("txt_weight");
                    TextBox txt_Reading = (TextBox)grdCoreTestInward.Rows[i].Cells[11].FindControl("txt_Reading");
                    TextBox txt_PulseVel = (TextBox)grdCoreTestInward.Rows[i].Cells[12].FindControl("txt_PulseVel");
                    DropDownList ddl_ModeOfFailure = (DropDownList)grdCoreTestInward.Rows[i].Cells[13].FindControl("ddl_ModeOfFailure");

                    if (txt_Description.Text == "" && txt_Description.Visible == true && grdCoreTestInward.Rows[i].Cells[4].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[5].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[6].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[7].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[8].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[9].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[10].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[11].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[12].Visible == true &&
                 grdCoreTestInward.Rows[i].Cells[13].Visible == true &&
                 grdCoreTestInward.Rows[i].Cells[14].Visible == true &&
                 grdCoreTestInward.Rows[i].Cells[15].Visible == true &&
                 grdCoreTestInward.Rows[i].Cells[16].Visible == true &&
                 grdCoreTestInward.Rows[i].Cells[17].Visible == true &&
                 grdCoreTestInward.Rows[i].Cells[18].Visible == true)
                    {
                        lblMsg.Text = "Enter Description for Sr No. " + (i + 1) + ".";
                        txt_Description.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddl_Grade.SelectedItem.Text == "Select" && ddl_Grade.Visible == true)
                    {
                        lblMsg.Text = "Select Grade for Sr No. " + (i + 1) + ".";
                        valid = false;
                        break;
                    }
                    else if (txt_CastingDt.Text == "" && txt_CastingDt.Visible == true)
                    {
                        lblMsg.Text = "Enter Casting Date for Sr No. " + (i + 1) + ".";
                        txt_CastingDt.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_Dia.Text == "" && txt_Dia.Visible == true)
                    {
                        lblMsg.Text = "Enter Diameter for Sr No. " + (i + 1) + ".";
                        txt_Dia.Focus();
                        valid = false;
                        break;
                    }

                    else if (txt_length.Text == "" && txt_length.Visible == true)
                    {
                        lblMsg.Text = "Enter Length for Sr No. " + (i + 1) + ".";
                        txt_length.Focus();
                        valid = false;
                        break;
                    }

                    else if (txt_lengthwthCaping.Text == "" && txt_lengthwthCaping.Visible == true)
                    {
                        lblMsg.Text = "Enter Length with caping for Sr No. " + (i + 1) + ".";
                        txt_lengthwthCaping.Focus();
                        valid = false;
                        break;
                    }

                    else if (txt_weight.Text == "" && txt_weight.Visible == true)
                    {
                        lblMsg.Text = "Enter Weight for Sr No. " + (i + 1) + ".";
                        txt_weight.Focus();
                        valid = false;
                        break;
                    }

                    else if (txt_Reading.Text == "" && txt_Reading.Visible == true)
                    {
                        lblMsg.Text = "Enter Reading for Sr No. " + (i + 1) + ".";
                        txt_Reading.Focus();
                        valid = false;
                        break;
                    }

                    else if (txt_PulseVel.Text == "" && txt_PulseVel.Visible == true)
                    {
                        lblMsg.Text = "Enter Pulse Velocity for Sr No. " + (i + 1) + ".";
                        txt_PulseVel.Focus();
                        valid = false;
                        break;
                    }

                    else if (ddl_ModeOfFailure.SelectedItem.Text == "Select" && ddl_ModeOfFailure.Visible == true)
                    {
                        lblMsg.Text = "Select Mode Of Failure for Sr No. " + (i + 1) + ".";
                        valid = false;
                        break;
                    }
                    //
                    if (txt_Dia.Text != "")
                    {
                        if (Convert.ToDecimal(txt_Dia.Text) <= 0)
                        {
                            lblMsg.Text = "Diameter should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_Dia.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_length.Text != "")
                    {
                        if (Convert.ToDecimal(txt_length.Text) <= 0 && txt_length.Visible == true)
                        {
                            lblMsg.Text = "Length should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_length.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_lengthwthCaping.Text != "")
                    {
                        if (Convert.ToDecimal(txt_lengthwthCaping.Text) <= 0 && txt_lengthwthCaping.Visible == true)
                        {
                            lblMsg.Text = "Length with Caping should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_lengthwthCaping.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_weight.Text != "")
                    {
                        if (Convert.ToDecimal(txt_weight.Text) <= 0 && txt_weight.Visible == true)
                        {
                            lblMsg.Text = "Weight should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_weight.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_Reading.Text != "")
                    {
                        if (Convert.ToDecimal(txt_Reading.Text) <= 0 && txt_Reading.Visible == true)
                        {
                            lblMsg.Text = "Reading should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_Reading.Focus();
                            valid = false;
                            break;
                        }
                    }
                    if (txt_PulseVel.Text != "")
                    {
                        if (Convert.ToDecimal(txt_PulseVel.Text) <= 0 && txt_PulseVel.Visible == true)
                        {
                            lblMsg.Text = "Pulse Velocity should be greater than 0 for Sr No. " + (i + 1) + ".";
                            txt_PulseVel.Focus();
                            valid = false;
                            break;
                        }
                    }

                    if (txt_Description.Text == "" && txt_Description.Visible == true && grdCoreTestInward.Rows[i].Cells[4].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[5].Visible == false &&
                   grdCoreTestInward.Rows[i].Cells[6].Visible == false &&
                   grdCoreTestInward.Rows[i].Cells[7].Visible == false &&
                   grdCoreTestInward.Rows[i].Cells[8].Visible == false &&
                   grdCoreTestInward.Rows[i].Cells[9].Visible == false &&
                   grdCoreTestInward.Rows[i].Cells[10].Visible == false &&
                   grdCoreTestInward.Rows[i].Cells[11].Visible == false &&
                   grdCoreTestInward.Rows[i].Cells[12].Visible == false &&
                 grdCoreTestInward.Rows[i].Cells[13].Visible == false &&
                 grdCoreTestInward.Rows[i].Cells[14].Visible == false &&
                 grdCoreTestInward.Rows[i].Cells[15].Visible == false &&
                 grdCoreTestInward.Rows[i].Cells[16].Visible == false &&
                 grdCoreTestInward.Rows[i].Cells[17].Visible == false &&
                 grdCoreTestInward.Rows[i].Cells[18].Visible == false)
                    {
                        lblMsg.Text = "Enter Title Sr No. between  " + (i) + " and " + (i + 1) + ".";
                        txt_Description.Focus();
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
                Calculation();
            }
            return valid;
        }
        protected void Lnk_Calculate_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Calculation();
            }
        }
        public void Calculation()
        {
            string LenforCore = "";
            string DiameterforCore = "";
            string Multifactor = "";
            for (int i = 0; i < grdCoreTestInward.Rows.Count; i++)
            {
                TextBox txt_Description = (TextBox)grdCoreTestInward.Rows[i].Cells[4].FindControl("txt_Description");
                DropDownList ddl_Grade = (DropDownList)grdCoreTestInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                TextBox txt_CastingDt = (TextBox)grdCoreTestInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                TextBox txt_Dia = (TextBox)grdCoreTestInward.Rows[i].Cells[7].FindControl("txt_Dia");
                TextBox txt_length = (TextBox)grdCoreTestInward.Rows[i].Cells[8].FindControl("txt_length");
                TextBox txt_lengthwthCaping = (TextBox)grdCoreTestInward.Rows[i].Cells[9].FindControl("txt_lengthwthCaping");
                TextBox txt_weight = (TextBox)grdCoreTestInward.Rows[i].Cells[10].FindControl("txt_weight");
                TextBox txt_Reading = (TextBox)grdCoreTestInward.Rows[i].Cells[11].FindControl("txt_Reading");
                TextBox txt_PulseVel = (TextBox)grdCoreTestInward.Rows[i].Cells[12].FindControl("txt_PulseVel");
                DropDownList ddl_ModeOfFailure = (DropDownList)grdCoreTestInward.Rows[i].Cells[13].FindControl("ddl_ModeOfFailure");
                TextBox txt_age = (TextBox)grdCoreTestInward.Rows[i].Cells[14].FindControl("txt_age");
                TextBox txt_CsArea = (TextBox)grdCoreTestInward.Rows[i].Cells[15].FindControl("txt_CsArea");
                TextBox txt_Density = (TextBox)grdCoreTestInward.Rows[i].Cells[16].FindControl("txt_Density");
                TextBox txt_CompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[17].FindControl("txt_CompStr");
                TextBox txt_CorrCompStr = (TextBox)grdCoreTestInward.Rows[i].Cells[18].FindControl("txt_CorrCompStr");
                TextBox txt_EquCubeStr = (TextBox)grdCoreTestInward.Rows[i].Cells[19].FindControl("txt_EquCubeStr");

                if (grdCoreTestInward.Rows[i].Cells[4].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[5].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[6].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[7].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[8].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[9].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[10].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[11].Visible == true &&
                   grdCoreTestInward.Rows[i].Cells[12].Visible == true &&
                 grdCoreTestInward.Rows[i].Cells[13].Visible == true &&
                 grdCoreTestInward.Rows[i].Cells[14].Visible == true &&
                 grdCoreTestInward.Rows[i].Cells[15].Visible == true &&
                 grdCoreTestInward.Rows[i].Cells[16].Visible == true &&
                 grdCoreTestInward.Rows[i].Cells[17].Visible == true &&
                 grdCoreTestInward.Rows[i].Cells[18].Visible == true)
                {
                    txt_CastingDt.Text = txt_CastingDt.Text.ToUpper();
                    if (txt_CastingDt.Text != "NA")
                    {
                        DateTime Testdt = DateTime.ParseExact(txt_TestingDt.Text, "dd/MM/yyyy", null);
                        DateTime Castdt = DateTime.ParseExact(txt_CastingDt.Text, "dd/MM/yyyy", null);
                        // DateTime Castdt = Convert.ToDateTime(txt_DtOfCasting.Text);
                        int AgeDays = 0;
                        AgeDays = Convert.ToInt32((Testdt - Castdt).TotalDays);
                        txt_age.Text = Convert.ToInt32(AgeDays).ToString();
                    }
                    else
                    {
                        txt_age.Text = "NA";
                    }

                    txt_CsArea.Text = ((Convert.ToDouble(txt_Dia.Text) / 2) * (Convert.ToDouble(txt_Dia.Text) / 2) * (22.00 / 7.00)).ToString("##.00");
                    txt_Density.Text = (Convert.ToDouble(txt_weight.Text) / ((Convert.ToDouble(txt_CsArea.Text) / 1000000) * (Convert.ToDouble(txt_length.Text) / 1000))).ToString("##.00");
                    txt_CompStr.Text = (Convert.ToDouble(txt_Reading.Text) / (Convert.ToDouble(txt_CsArea.Text) / 1000)).ToString("##.00");

                    DiameterforCore = Convert.ToDouble(txt_Dia.Text).ToString();
                    LenforCore = Convert.ToDouble(txt_lengthwthCaping.Text).ToString();
                    LenforCore = (Convert.ToDouble(txt_lengthwthCaping.Text) / Convert.ToDouble(txt_Dia.Text)).ToString("00.00");//##.##
                    Multifactor = Convert.ToDouble((0.106 * Convert.ToDouble(LenforCore)) + 0.786).ToString("0.000");

                    DateTime xTestdt = DateTime.ParseExact(txt_TestingDt.Text, "dd/MM/yyyy", null);
                    //DateTime xdt1 = DateTime.ParseExact("05/05/2019", "dd/MM/yyyy", null);
                    if (xTestdt >= DateTime.ParseExact("05/05/2019", "dd/MM/yyyy", null))
                    {
                        if (Convert.ToDouble(DiameterforCore) < 70)
                        {
                            //1.06
                            txt_CorrCompStr.Text = (Convert.ToDouble(txt_CompStr.Text) * Convert.ToDouble(Multifactor) * 1.06).ToString("##.00");

                        }
                        else if (Convert.ToDouble(DiameterforCore) >= 70 && Convert.ToDouble(DiameterforCore) < 100)
                        {
                            //1.03
                            txt_CorrCompStr.Text = (Convert.ToDouble(txt_CompStr.Text) * Convert.ToDouble(Multifactor) * 1.03).ToString("##.00");
                        }
                        else if (Convert.ToDouble(DiameterforCore) >= 100)
                        {
                            txt_CorrCompStr.Text = (Convert.ToDouble(txt_CompStr.Text) * Convert.ToDouble(Multifactor) * 1).ToString("##.00");
                        }
                    }
                    else
                    {
                        if (Convert.ToDouble(DiameterforCore) < 100)
                        {
                            txt_CorrCompStr.Text = (Convert.ToDouble(txt_CompStr.Text) * Convert.ToDouble(Multifactor) * 1.08).ToString("##.00");

                        }
                        else if (Convert.ToDouble(DiameterforCore) >= 100)
                        {
                            txt_CorrCompStr.Text = (Convert.ToDouble(txt_CompStr.Text) * Convert.ToDouble(Multifactor) * 1).ToString("##.00");
                        }
                    }
                    txt_EquCubeStr.Text = (Convert.ToDouble(txt_CorrCompStr.Text) * 1.25).ToString("##.00");
                }
            }


            bool valid = false;
            bool validNA = false;
            for (int i = 0; i < grdCoreTestInward.Rows.Count; i++)
            {
                DropDownList ddl_Grade = (DropDownList)grdCoreTestInward.Rows[i].Cells[5].FindControl("ddl_Grade");
                TextBox txt_CastingDt = (TextBox)grdCoreTestInward.Rows[i].Cells[6].FindControl("txt_CastingDt");
                TextBox txt_age = (TextBox)grdCoreTestInward.Rows[i].Cells[14].FindControl("txt_age");
                if (ddl_Grade.SelectedItem.Text == "NA" || txt_CastingDt.Text == "NA" || txt_age.Text == "NA")
                {
                    valid = true;
                    for (int J = 0; J < grdRemark.Rows.Count; J++)
                    {
                        TextBox txt_REMARK = (TextBox)grdRemark.Rows[J].Cells[1].FindControl("txt_REMARK");
                        // txt_REMARK.Text = "NA  indicates - not available";

                        if (txt_REMARK.Text.Trim() == "NA  indicates - not available")
                        {
                            validNA = true;
                            break;
                        }
                    }
                    if (validNA == true || valid == true)
                    {
                        break;
                    }
                }
            }
            if (valid == true && validNA == false)
            {
                int J = 0;
                for (J = 0; J < grdRemark.Rows.Count; J++)
                {
                    TextBox txt_REMARK = (TextBox)grdRemark.Rows[J].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "" && J == grdRemark.Rows.Count - 1)
                    {
                        AddRowCoreRemark();
                        break;
                    }
                }
                for (J = grdRemark.Rows.Count - 1; J < grdRemark.Rows.Count; J--)
                {
                    TextBox txt_REMARK = (TextBox)grdRemark.Rows[J].Cells[1].FindControl("txt_REMARK");
                    if (J > 0)
                    {
                        TextBox txt_REMARKn = (TextBox)grdRemark.Rows[J - 1].Cells[1].FindControl("txt_REMARK");
                        txt_REMARK.Text = txt_REMARKn.Text;
                    }
                    else
                    {
                        txt_REMARK.Text = "NA  indicates - not available";
                        break;
                    }
                }
            }
        }
        protected void imgBtnMergeRow_Click(object sender, CommandEventArgs e)
        {
            ImageButton img = (ImageButton)sender;
            GridViewRow row = (GridViewRow)img.Parent.Parent;
            int rowindex = row.RowIndex;

            //  Demerge(rowindex);
            ShowMerge(rowindex);
        }

        public void Demerge(int RowIndex)
        {
            if (Convert.ToInt32(RowIndex) >= 0)
            {
                //for (int i = 0; i < grdCoreTestInward.Rows.Count; i++)
                //{
                if (
                   grdCoreTestInward.Rows[RowIndex].Cells[5].Visible == false &&
                   grdCoreTestInward.Rows[RowIndex].Cells[6].Visible == false &&
                   grdCoreTestInward.Rows[RowIndex].Cells[7].Visible == false &&
                   grdCoreTestInward.Rows[RowIndex].Cells[8].Visible == false &&
                   grdCoreTestInward.Rows[RowIndex].Cells[9].Visible == false &&
                   grdCoreTestInward.Rows[RowIndex].Cells[10].Visible == false &&
                   grdCoreTestInward.Rows[RowIndex].Cells[11].Visible == false &&
                   grdCoreTestInward.Rows[RowIndex].Cells[12].Visible == false &&
                   grdCoreTestInward.Rows[RowIndex].Cells[13].Visible == false &&
                   grdCoreTestInward.Rows[RowIndex].Cells[14].Visible == false &&
                   grdCoreTestInward.Rows[RowIndex].Cells[15].Visible == false &&
                   grdCoreTestInward.Rows[RowIndex].Cells[16].Visible == false &&
                   grdCoreTestInward.Rows[RowIndex].Cells[17].Visible == false &&
                   grdCoreTestInward.Rows[RowIndex].Cells[18].Visible == false)
                {
                    DeleteRowReportCoreInward(RowIndex);
                    AddRowEnterReportCoreInward();
                    // break;
                }
                //}
            }
        }
        public void ShowMerge(int rowindex)
        {
            if (Convert.ToInt32(rowindex) >= 0)
            {
                if (
                   grdCoreTestInward.Rows[rowindex].Cells[5].Visible == false &&
                   grdCoreTestInward.Rows[rowindex].Cells[6].Visible == false &&
                   grdCoreTestInward.Rows[rowindex].Cells[7].Visible == false &&
                   grdCoreTestInward.Rows[rowindex].Cells[8].Visible == false &&
                   grdCoreTestInward.Rows[rowindex].Cells[9].Visible == false &&
                   grdCoreTestInward.Rows[rowindex].Cells[10].Visible == false &&
                   grdCoreTestInward.Rows[rowindex].Cells[11].Visible == false &&
                   grdCoreTestInward.Rows[rowindex].Cells[12].Visible == false &&
                   grdCoreTestInward.Rows[rowindex].Cells[13].Visible == false &&
                   grdCoreTestInward.Rows[rowindex].Cells[14].Visible == false &&
                   grdCoreTestInward.Rows[rowindex].Cells[15].Visible == false &&
                   grdCoreTestInward.Rows[rowindex].Cells[16].Visible == false &&
                   grdCoreTestInward.Rows[rowindex].Cells[17].Visible == false &&
                   grdCoreTestInward.Rows[rowindex].Cells[18].Visible == false)
                {
                    DeleteRowReportCoreInward(rowindex);
                    AddRowEnterReportCoreInward();
                }
                else
                {
                    for (int i = 0; i < grdCoreTestInward.Rows.Count; i++)
                    {
                        TextBox txt_Description = (TextBox)grdCoreTestInward.Rows[i].Cells[4].FindControl("txt_Description");
                        if (i == Convert.ToInt32(rowindex))
                        {
                            bool valid = false;
                            if (ViewState["RowNo"] != null)
                            {
                                int row = 0;
                                string[] rowindexx = Convert.ToString(ViewState["RowNo"]).Split(' ');
                                foreach (var rowx in rowindexx)
                                {
                                    if (rowx != "")
                                    {
                                        if (int.TryParse(rowx, out row))
                                        {
                                            row = Convert.ToInt32(rowx);
                                            if (row == rowindex)
                                            {
                                                valid = true;
                                            }
                                        }
                                    }
                                }
                            }
                            if (valid == false)
                            {
                                ViewState["RowNo"] = ViewState["RowNo"] + " " + rowindex;
                            }
                            grdCoreTestInward.Rows[i].Cells[4].ColumnSpan += 16;
                            txt_Description.Width = 960;
                            txt_Description.CssClass = "Titlecol";
                            grdCoreTestInward.Rows[i].Cells[3].CssClass = "Srcol";
                            grdCoreTestInward.Rows[i].Cells[4].CssClass = "hiddencol";
                            grdCoreTestInward.Rows[i].Cells[5].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[6].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[7].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[8].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[9].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[10].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[11].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[12].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[13].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[14].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[15].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[16].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[17].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[18].Visible = false;
                            grdCoreTestInward.Rows[i].Cells[19].Visible = false;
                        }
                    }
                }
                CountSrNo();
            }

        }

        public void CountSrNo()
        {
            int j = 1;
            for (int i = 0; i < grdCoreTestInward.Rows.Count; i++)
            {
                TextBox txt_Description = (TextBox)grdCoreTestInward.Rows[i].Cells[4].FindControl("txt_Description");
                grdCoreTestInward.Rows[i].Cells[3].Text = (j++).ToString();

                if (
                     grdCoreTestInward.Rows[i].Cells[5].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[6].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[7].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[8].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[9].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[10].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[11].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[12].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[13].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[14].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[15].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[16].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[17].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[18].Visible == false &&
                     grdCoreTestInward.Rows[i].Cells[19].Visible == false)
                {
                    j--;
                    grdCoreTestInward.Rows[i].Cells[3].Text = "";
                }
            }
        }

        public void ViewWitnessBy()
        {
              txt_witnessBy.Visible = false;
            chk_WitnessBy.Checked = false;
            if (lblEntry.Text == "Check")
            {
                var ct = dc.ReportStatus_View("Core Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
                foreach (var c in ct)
                {
                    if (c.CRINWD_WitnessBy_var.ToString() != null && c.CRINWD_WitnessBy_var.ToString() != "")
                    {
                        txt_witnessBy.Visible = true;
                        txt_witnessBy.Text = c.CRINWD_WitnessBy_var.ToString();
                        chk_WitnessBy.Checked = true;
                    }
                }
            }

        }
        private void LoadOtherPendingCheckRpt()
        {
            if (lblEntry.Text == "Check")
            {
                string Refno = txt_ReferenceNo.Text;
                var testinguser = dc.ReportStatus_View("Core Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 2, 0);
                ddl_OtherPendingRpt.DataTextField = "CRINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataValueField = "CRINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataSource = testinguser;
                ddl_OtherPendingRpt.DataBind();
                ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
                ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(Refno);
                if (itemToRemove != null)
                {
                    ddl_OtherPendingRpt.Items.Remove(itemToRemove);
                }
                lbl_TestedBy.Text = "Approve By";
            }
            else
            {
                LoadOtherPendingRpt();
            }
        }
        public void DisplayRemark()
        {
            int i = 0;
            var re = dc.AllRemark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, "CR");
            foreach (var r in re)
            {
                AddRowCoreRemark();
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CRDetail_RemarkId_int), "CR");
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.CR_Remark_var.ToString();
                    i++;
                }
            }
            if (grdRemark.Rows.Count <= 0)
            {
                AddRowCoreRemark();
            }
        }
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowCoreRemark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowCoreRemark(gvr.RowIndex);
            }
        }

        protected void DeleteRowCoreRemark(int rowIndex)
        {
            GetCurrentDataCoreRemark();
            DataTable dt = ViewState["CoreRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CoreRemarkTable"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataCoreRemark();
        }
        protected void AddRowCoreRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CoreRemarkTable"] != null)
            {
                GetCurrentDataCoreRemark();
                dt = (DataTable)ViewState["CoreRemarkTable"];
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
            ViewState["CoreRemarkTable"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataCoreRemark();
        }

        protected void GetCurrentDataCoreRemark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            for (int i = 0; i < grdRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");


                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CoreRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataCoreRemark()
        {
            DataTable dt = (DataTable)ViewState["CoreRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
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
        protected void lnk_Fetch_Click(object sender, EventArgs e)
        {
            FetchRefNo();
        }
        public void FetchRefNo()
        {
            if (ddl_OtherPendingRpt.SelectedValue != "---Select---")
            {
                try
                {
                    lnkSave.Enabled = true;
                    lnkPrint.Visible = false;
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Visible = false;
                    txt_ReferenceNo.Text = ddl_OtherPendingRpt.SelectedItem.Text;
                    txt_ReferenceNo.Text = txt_ReferenceNo.Text;
                    DisplayCoreDetails();
                    //LoadOtherPendingCheckRpt();
                    LoadReferenceNoList();
                    ViewWitnessBy();
                    LoadApproveBy();
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
                finally
                {
                    grdCoreTestInward.DataSource = null;
                    grdCoreTestInward.DataBind();
                    DisplayGridCoreData();
                    grdRemark.DataSource = null;
                    grdRemark.DataBind();
                    DisplayRemark();
                }
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                //rpt.Core_PDFReport(txt_ReferenceNo.Text  , lblEntry.Text);
                rpt.PrintSelectedReport(txt_RecType.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", "", "", "", "");
            }
            //RptCore_Testing();
        }

        public void RptCore_Testing()
        {
            //string reportPath;
            //string reportStr = "";
            //StreamWriter sw;
            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            //reportStr = getDetailReportCoreTesting();
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.CoreReport_Html(txt_ReferenceNo.Text, lblEntry.Text);
        }

        //protected string getDetailReportCoreTesting()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";
        //    String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Concrete Core Compressive Strength </b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    int PrintPulse = 0;
        //    var Core = dc.ReportStatus_View("Core Testing", null, null, 0, 0, 0, txt_ReferenceNo.Text, 0, 2, 0);
        //    foreach (var CoreTest in Core)
        //    {
        //        mySql += "<tr>" +
        //        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td width='40%' height=19><font size=2>" + CoreTest.CL_Name_var + "</font></td>" +
        //        "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td height=19><font size=2>" + "-" + "</font></td>" +
        //        "<td height=19><font size=2>&nbsp;</font></td>" +
        //        "</tr>";


        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font></td>" +
        //            "<td width='40%' height=19><font size=2>" + CoreTest.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "CR" + "-" + CoreTest.CRINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //         "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //         "<td width='2%' height=19><font size=2></font></td>" +
        //         "<td width='10%' height=19><font size=2></font></td>" +
        //         "<td height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //         "<td height=19><font size=2>:</font></td>" +
        //         "<td height=19><font size=2>" + "CR" + "-" + CoreTest.CRINWD_ReferenceNo_var + "</font></td>" +
        //         "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + CoreTest.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Sample Ref No. </b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "CR" + "-" + "" + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //           "<td align=left valign=top height=19><font size=2><b> Concrete Member </b></font></td>" +
        //           "<td  width='2%' height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + CoreTest.CRINWD_ConcreteMember_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "CR" + "-" + " " + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +
        //            "<td align=left valign=top height=19><font size=2><b> Grade Of Concrete</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + CoreTest.CRINWD_Grade_var + "</font></td>" +
        //            "<td align=left valign=top height=19><font size=2><b>Specimen extraction Date </b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(CoreTest.CRINWD_SpecimenExtDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Curring Conditions</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + CoreTest.CRINWD_CurrCondition_var + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(CoreTest.CRINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
        //              "</tr>";
        //        if (Convert.ToString(CoreTest.CRINWD_PulseVelocity_bit) != null)
        //        {
        //            PrintPulse = Convert.ToInt32(CoreTest.CRINWD_PulseVelocity_bit);
        //        }
        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% rowspan=1 align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Diameter </br> </br> </br> (mm) </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Date of Casting </b></font></td>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Age of Concrete </br> </br> (Days) </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Area of Cross Section  (mm <sup>2</sup>)</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Weight before capping (kg)</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Density of concrete </br> </br> (kg/m <sup>3</sup>)</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Load at failure </br> </br> (kN)</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Comp. Strength </br> </br> (N/mm<sup>2</sup>)</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Corrected Comp. Strength (N/mm<sup>2</sup>)</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Equivalent cube Comp. Strength (N/mm<sup>2</sup>)</b></font></td>";
        //    mySql += "</tr>";

        //    int SrNo = 0;
        //    var CoreTesting = dc.TestDetail_Title_View(txt_ReferenceNo.Text, 0, "CR", false);
        //    foreach (var core in CoreTesting)
        //    {
        //        SrNo++;
        //        if (Convert.ToString(core.Description_var) != "")
        //        {
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Description_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Dia_int) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Castingdate_var) + "</font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Age_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.CsArea_dec) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Weight_dec) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Density_dec) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Reading_dec) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.CompStr_dec) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.CorrCompStr_dec) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.EquCubeStr_dec) + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //        else
        //        {
        //            if (Convert.ToString(core.TitleId_int) != "")
        //            {
        //                if (Convert.ToInt32(core.TitleId_int) > 0)
        //                {
        //                    var crr = dc.TestDetail_Title_View(txt_ReferenceNo.Text, Convert.ToInt32(core.TitleId_int), "CR", false);
        //                    foreach (var title in crr)
        //                    {
        //                        mySql += "<tr>";
        //                        mySql += "<td width= 10% colspan=12 align=center valign=top height=19 ><font size=2> <b>&nbsp;" + Convert.ToString(title.TitleDesc_var) + "</b></font></td>";
        //                        mySql += "</tr>";
        //                        SrNo--;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    mySql += "</table>";
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "GENERAL INFORMATION & MODE OF FAILURE: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=80% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2%  rowspan=2 align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 10%  rowspan=2 align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";



        //    mySql += "<td width= 5% align=center colspan=2 valign=top height=19 ><font size=2><b>Correction Factor </b></font></td>";
        //    mySql += "<td width= 5% align=center colspan=2 valign=top height=19 ><font size=2><b>Core Length (mm) </b></font></td>";

        //    if (PrintPulse == 1)
        //    {
        //        mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Pulse  Velcocity</b></font></td>";
        //    }
        //    mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Mode of Failure</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "<td width= 5%  align=center valign=top height=19 ><font size=2><b>L/D </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Diameter </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Original </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>with cap</b></font></td>";

        //    SrNo = 0;
        //    decimal Diameter = 0;
        //    decimal Lenforcore = 0;
        //    decimal Multifactor = 0;

        //    var CrTesting = dc.TestDetail_Title_View(txt_ReferenceNo.Text, 0, "CR", false);
        //    foreach (var core in CrTesting)
        //    {
        //        SrNo++;
        //        if (Convert.ToString(core.Description_var) != "")
        //        {
        //            if (core.Dia_int < 100)
        //            {
        //                Diameter = Convert.ToDecimal(1.08);
        //            }
        //            else if (core.Dia_int >= 100)
        //            {
        //                Diameter = Convert.ToDecimal(1);
        //            }
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";

        //            if (Convert.ToString(core.Dia_int) != "" && Convert.ToString(core.LengthCaping_num) != "")
        //            {
        //                Lenforcore = (Convert.ToDecimal(core.Dia_int) / Convert.ToDecimal(core.LengthCaping_num));
        //                if (Convert.ToDecimal(core.Dia_int) > 0 && Convert.ToDecimal(core.LengthCaping_num) > 0)
        //                {
        //                    Multifactor = (Convert.ToDecimal(0.106) * Lenforcore) + Convert.ToDecimal(0.786);
        //                }
        //            }
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Description_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(Multifactor).ToString("0.000") + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Diameter) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Length_num) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.LengthCaping_num) + "</font></td>";
        //            if (PrintPulse == 1)
        //            {
        //                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.PulseVelocity_dec) + "</font></td>";
        //            }
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.ModeOfFailure_var) + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //        else
        //        {
        //            if (Convert.ToString(core.TitleId_int) != "")
        //            {
        //                if (Convert.ToInt32(core.TitleId_int) > 0)
        //                {
        //                    var crr = dc.TestDetail_Title_View(txt_ReferenceNo.Text, Convert.ToInt32(core.TitleId_int), "CR", false);
        //                    foreach (var title in crr)
        //                    {
        //                        mySql += "<tr>";
        //                        mySql += "<td width= 5% colspan=12 align=center valign=top height=19 ><font size=2><b>&nbsp;" + Convert.ToString(title.TitleDesc_var) + "</b></font></td>";
        //                        mySql += "</tr>";
        //                        SrNo--;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    SrNo = 0;
        //    var matid = dc.Material_View("CR", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }

        //    }
        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, "CR");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CRDetail_RemarkId_int), "CR");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.CR_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (lblEntry.Text == "Check")
        //    {
        //        var RecNo = dc.ReportStatus_View("Core Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (Convert.ToString(r.CRINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.CRINWD_CheckedBy_tint) != string.Empty)
        //            {
        //                var U = dc.User_View(r.CRINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.CRINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999CRC014212" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
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

    }
}