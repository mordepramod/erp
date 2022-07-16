using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class Cube_Report : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
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
                    if (strReq.Contains("=") == true)
                    {
                        string[] arrMsgs = strReq.Split('&');
                        string[] arrIndMsg;

                        arrIndMsg = arrMsgs[0].Split('=');
                        txt_RecordType.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[1].Split('=');
                        lblRecordNo.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[2].Split('=');
                        txt_ReferenceNo.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[3].Split('=');
                        lblEntry.Text = arrIndMsg[1].ToString().Trim();

                        if (txt_ReferenceNo.Text == "")
                        {
                            arrIndMsg = arrMsgs[4].Split('=');
                            txt_ReferenceNo.Text = arrIndMsg[1].ToString().Trim();
                        }
                        arrIndMsg = arrMsgs[4].Split('=');
                        txt_ReportNo.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[5].Split('=');
                        lblTrialId.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[6].Split('=');
                        lblCubecompstr.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[7].Split('=');
                        lblResult.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[8].Split('=');
                        lblComprTest.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[9].Split('=');
                        lblChkStatus.Text = arrIndMsg[1].ToString().Trim();
                        arrIndMsg = arrMsgs[10].Split('=');
                        lblDays.Text = arrIndMsg[1].ToString().Trim();
                    }
                }
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");//

                lblheading.Text = "Cube - Report Entry";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);

                if (lblCubecompstr.Text == "TrialCubeCompStr" && lblCubecompstr.Text != "CubeCompStrength" && lblCubecompstr.Text != "CementStrength")
                {
                    lblheading.Text = "Mix Design Cube Compressive Strength";
                    BindCubeDaysMF();
                }
                else if (lblCubecompstr.Text == "CubeCompStrength" || lblCubecompstr.Text == "CementStrength")
                {
                    if (Convert.ToString(txt_RecordType.Text) == "CEMT")
                    {
                        lblheading.Text = "Cement Cube Compressive Strength";
                    }
                    else if (Convert.ToString(txt_RecordType.Text) == "FLYASH")
                    {
                        lblheading.Text = "Flyash Cube Compressive Strength";
                    }
                    else if (Convert.ToString(txt_RecordType.Text) == "GGBS")
                    {
                        lblheading.Text = "GGBS Cube Compressive Strength";
                    }
                    DisplayCubeCompStrGridRow();
                }
                else if (txt_RecordType.Text != "")
                {
                    lblCubecompstr.Text = "";
                    DisplayCubeDetails();
                    DisplayGridRow();
                    DisplayRemark();
                    DisplayIdMark();
                    txt_RecordType.Text = "CT";

                    if (lblEntry.Text == "Check")
                    {
                        lblheading.Text = "Cube - Report Check";
                        LoadApproveBy();
                        DisplayCubeGrid();
                        lbl_TestedBy.Text = "Approve By";
                    }
                    else
                    {
                        LoadTestedBy();
                    }
                    ViewWitnessBy();
                    LoadReferenceNoList();
                }
                else
                {
                    lblOtherPendingRptMF.Visible = true;
                    ddlOtherPendingRptMF.Visible = true;
                    lblTrial.Visible = true;
                    ddlTrial.Visible = true;

                    lblCubecompstr.Text = "TrialCubeCompStr";
                    lblheading.Text = "Mix Design Cube Compressive Strength";
                    lbl_OtherPending.Text = "Days";
                    txt_RecordType.Text = "MF";
                    lblcubetype.Text = "MF";
                    txt_DtOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    EnableFalse();
                    LoadMFReferenceNoList();
                }
                if (lblcubetype.Text == "CT" || lblcubetype.Text == "MF")
                    lnkGetData.Visible = false;
            }
        }
        
        private void LoadMFReferenceNoList()
        {
            var reportList = dc.MaterialDetail_View_List("Trial");
            ddlOtherPendingRptMF.DataTextField = "MaterialDetail_RefNo";
            ddlOtherPendingRptMF.DataSource = reportList;
            ddlOtherPendingRptMF.DataBind();
            ddlOtherPendingRptMF.Items.Insert(0, "---Select---");
        }
        
        private void LoadTrialList()
        {
            var trial = dc.Trial_View(ddlOtherPendingRptMF.SelectedItem.Value, false);
            ddlTrial.DataTextField = "Trial_Name";
            ddlTrial.DataValueField = "Trial_Id";
            ddlTrial.DataSource = trial;
            ddlTrial.DataBind();
            ddlTrial.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        
        protected void ddlOtherPendingRptMF_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOtherPendingRptMF.SelectedIndex > 0)
            {
                LoadTrialList();
            }
            else
            {
                ddlTrial.Items.Clear();
                ddl_OtherPendingRpt.Items.Clear(); 
            }
        }

        protected void ddlTrial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTrial.SelectedIndex > 0)
            {
                txt_ReferenceNo.Text = ddlOtherPendingRptMF.SelectedValue;
                txt_ReportNo.Text = txt_ReferenceNo.Text;
                lblTrialId.Text = ddlTrial.SelectedValue; 
                BindCubeDaysMF();
            }
            else
            {
                ddl_OtherPendingRpt.Items.Clear();
            }
        }

        private void LoadReferenceNoList()
        {
            byte reportStatus = 0;
            if (lblEntry.Text == "Enter")
                reportStatus = 1;
            else if (lblEntry.Text == "Check")
                reportStatus = 2;

            var reportList = dc.ReferenceNo_View_StatusWise("CT", reportStatus, 0);
            ddl_OtherPendingRpt.DataTextField = "ReferenceNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_OtherPendingRpt.Items.Remove(txt_ReferenceNo.Text);
        }

        protected void ddl_OtherPendingRpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txt_RecordType.Text == "MF" && ddl_OtherPendingRpt.SelectedItem.Text != "---Select---")
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                lnkSave.Enabled = true;
                grdCubeInward.DataSource = null;
                grdCubeInward.DataBind();
                grdRemark.DataSource = null;
                grdRemark.DataBind();
                ShowMFCubeCasting();
            }
        }
        private void ShowCastingdt()
        {
            int days = 0;
            if (txt_RecordType.Text == "MF")
            {
                lblcubetype.Text = "Trial";
                days = Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue);
            }
            else
            {
                days = Convert.ToInt32(lblDays.Text);
            }
            var CubeCast = dc.OtherCubeTestView(Convert.ToString(txt_ReferenceNo.Text), txt_RecordType.Text, Convert.ToByte(days), 0, lblcubetype.Text, false, false);
            foreach (var cu in CubeCast)
            {
                if (Convert.ToString(cu.CastingDate) != "")
                {
                    txt_DtOfCasting.Text = cu.CastingDate.ToString();
                }
                //else
                //{
                //    this.txt_DtOfCasting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //}
                if (Convert.ToString(cu.TestingDate) != "")
                {
                    txt_DtOfTesting.Text = Convert.ToDateTime(cu.TestingDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    this.txt_DtOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }
        
        protected void BindCubeDaysMF()
        {           
          
            var cubeCast = dc.OtherCubeTestView(txt_ReferenceNo.Text, "MF", 0, Convert.ToInt32(lblTrialId.Text), "Trial", false,false);
            
            ddl_OtherPendingRpt.DataTextField = "Days_tint";
            ddl_OtherPendingRpt.DataValueField = "Days_tint";
            ddl_OtherPendingRpt.DataSource = cubeCast;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
            ddl_TestedBy.Items.Insert(0, "");
        }
        protected void ShowMFCubeCasting()
        {
            if (lblCubecompstr.Text == "TrialCubeCompStr")
            {
                int i = 0;
                string Avg = string.Empty;               
                var cube2 = dc.OtherCubeTestMF(txt_ReferenceNo.Text, txt_RecordType.Text, Convert.ToByte(ddl_OtherPendingRpt.SelectedValue), Convert.ToInt32(ddlTrial.SelectedValue),"Trial");
                foreach (var c1 in cube2)
                {
                    if (c1.Avg_var != null)
                    {
                        Avg = Convert.ToString(c1.Avg_var);
                    }
                    if (Convert.ToString(c1.TestingDate) != "")
                    {
                        txt_DtOfTesting.Text = Convert.ToDateTime(c1.TestingDate).ToString("dd/MM/yyyy");
                    }
                    if (Convert.ToString(c1.CastingDate) != "")
                    {
                        txt_DtOfCasting.Text = Convert.ToString(c1.CastingDate);
                    }
                    if (Convert.ToString(c1.WitnessBy_var) != "" && Convert.ToString(c1.WitnessBy_var) != null)
                    {
                        txt_witnessBy.Visible = true;
                        txt_witnessBy.Text = c1.WitnessBy_var.ToString();
                        chk_WitnessBy.Checked = true;
                    }
                }
                var mfInward = dc.ReportStatus_View("Mix Design", null, null, 0, 0, 0, txt_ReferenceNo.Text, 0, 2, 0);
                foreach (var cube in mfInward)
                { 
                    ddl_gradeOfConcrete.SelectedValue = cube.MFINWD_Grade_var;
                }

                //var cube = dc.OtherCubeTestView(txt_ReferenceNo.Text, txt_RecordType.Text, Convert.ToByte(ddl_OtherPendingRpt.SelectedValue),Convert.ToInt32( ddlTrial.SelectedValue), "Trial", false, true);
                var cube1 = dc.OtherCubeDetailForMF(txt_ReferenceNo.Text, txt_RecordType.Text, Convert.ToByte(ddl_OtherPendingRpt.SelectedValue), Convert.ToInt32(ddlTrial.SelectedValue),"Trial");
                
                foreach (var c in cube1)
                {
                    AddRowEnterReportCubeInward();
                    TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                    TextBox txt_Length = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txt_Length");
                    TextBox txt_Breadth = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txt_Breadth");
                    TextBox txt_Height = (TextBox)grdCubeInward.Rows[i].Cells[4].FindControl("txt_Height");
                    TextBox txt_Weight = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txt_Weight");
                    TextBox txt_Reading = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txt_Reading");
                    TextBox txt_Age = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txt_Age");
                    TextBox txt_CSArea = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txt_CSArea");
                    TextBox txt_DEnsity = (TextBox)grdCubeInward.Rows[i].Cells[9].FindControl("txt_DEnsity");
                    TextBox txt_CompStr = (TextBox)grdCubeInward.Rows[i].Cells[10].FindControl("txt_CompStr");
                    TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[i].Cells[11].FindControl("txt_AvgStr");
                    Label lblImage1 = (Label)grdCubeInward.Rows[i].FindControl("lblImage1");
                    Label lblImage2 = (Label)grdCubeInward.Rows[i].FindControl("lblImage2");
                    Label lblImage3 = (Label)grdCubeInward.Rows[i].FindControl("lblImage3");

                    txt_IdMark.Text = c.IdMark_var.ToString();
                    txt_Length.Text = c.Length_var.ToString();
                    txt_Breadth.Text = c.Breadth_dec.ToString();
                    txt_Height.Text = c.Height_dec.ToString();
                    txt_Weight.Text = c.Weight_dec.ToString();
                    //txt_Reading.Text = c.Reading_var.ToString();
                    //txt_Age.Text = c.Age_var.ToString();
                    //txt_CSArea.Text = c.CSArea_dec.ToString();
                    //txt_DEnsity.Text = c.Density_dec.ToString();
                    //txt_CompStr.Text = c.CompStr_var.ToString();
                    if (c.Reading_var != null)
                    {
                        txt_Reading.Text = c.Reading_var.ToString();
                    }
                    if (Convert.ToString(c.Image_var) != null)
                        lblImage1.Text = c.Image_var.ToString();
                    else
                        lblImage1.Text = "";
                    if (Convert.ToString(c.Image_var1) != null)
                        lblImage2.Text = c.Image_var1.ToString();
                    else
                        lblImage2.Text = "";
                    if (Convert.ToString(c.Image_var2) != null)
                        lblImage3.Text = c.Image_var2.ToString();
                    else
                        lblImage3.Text = "";
                    if (c.Age_var != null)
                    {
                        txt_Age.Text = c.Age_var.ToString();
                    }
                    if (c.CSArea_dec != null)
                    {
                        txt_CSArea.Text = c.CSArea_dec.ToString();
                    }
                    if (c.Density_dec != null)
                    {
                        txt_DEnsity.Text = c.Density_dec.ToString();
                    }
                    if (c.CompStr_var != null)
                    {
                        txt_CompStr.Text = c.CompStr_var.ToString();
                    }
                    i++;
                }
                if (grdCubeInward.Rows.Count > 0)
                {
                    if (Avg != "")
                    {
                        int NoOfrows = grdCubeInward.Rows.Count / 2;
                        TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[NoOfrows].Cells[11].FindControl("txt_AvgStr");
                        txt_AvgStr.Text = Convert.ToString(Avg);
                    }
                }
                if (grdCubeInward.Rows.Count <= 0)
                {
                    InitialGrdMF();
                }
                i = 0;
                var re = dc.OtherCubeTestRemark_View("", txt_ReferenceNo.Text, 0, Convert.ToByte(ddl_OtherPendingRpt.SelectedValue), "MF");
                foreach (var rm in re)
                {
                    var remark = dc.OtherCubeTestRemark_View("", "", Convert.ToInt32(rm.RemarkId_int), Convert.ToByte(ddl_OtherPendingRpt.SelectedValue), "MF");
                    foreach (var rem in remark)
                    {
                        AddRowCubeRemark();
                        TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                        txt_REMARK.Text = rem.Remark_var.ToString();
                        i++;
                    }
                }
                if (grdRemark.Rows.Count <= 0)
                {
                    AddRowCubeRemark();
                }
            }
        }
        protected void InitialGrdMF()
        {
            var cubeCast = dc.OtherCubeTestView(txt_ReferenceNo.Text, txt_RecordType.Text, Convert.ToByte(ddl_OtherPendingRpt.SelectedValue), 0, "Trial", false, false);
            clsData obj = new clsData();
            string mySql = "select  NoOfCubes_tint from tbl_OtherCubeTest where RefNo_var='" + txt_ReferenceNo.Text + "'";
            mySql+= " and RecType_var='"+txt_RecordType.Text +"'";
			mySql+= " and  CubeType_var='Trial' and Trial_Id = "+Convert.ToInt32(lblTrialId.Text);
            mySql += " and Days_tint=" + Convert.ToByte(ddl_OtherPendingRpt.SelectedValue);

            DataTable dt = obj.getGeneralData(mySql);
           if (dt.Rows.Count >0 )
           {
                for (int i = 0; i < Convert.ToInt32( dt.Rows[0]["NoOfCubes_tint"].ToString()) ; i++)
                {
                    AddRowEnterReportCubeInward();
                    TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                    TextBox txt_Length = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txt_Length");
                    TextBox txt_Breadth = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txt_Breadth");
                    TextBox txt_Height = (TextBox)grdCubeInward.Rows[i].Cells[4].FindControl("txt_Height");
                    TextBox txt_Weight = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txt_Weight");
                    TextBox txt_Reading = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txt_Reading");
                    TextBox txt_Age = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txt_Age");
                    TextBox txt_CSArea = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txt_CSArea");
                    TextBox txt_DEnsity = (TextBox)grdCubeInward.Rows[i].Cells[9].FindControl("txt_DEnsity");
                    TextBox txt_CompStr = (TextBox)grdCubeInward.Rows[i].Cells[10].FindControl("txt_CompStr");
                    TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[i].Cells[11].FindControl("txt_AvgStr");
                }
           }
        }
        protected void EnableFalse()
        {
            //txt_DtOfCasting.ReadOnly = true;
            if (txt_RecordType.Text != "MF")
            {
                ddl_OtherPendingRpt.Enabled = false;
                //txt_DtOfCasting.ReadOnly = false ;
            }
         
            ddl_gradeOfConcrete.Enabled = false;
            txt_NatureOfwork.Enabled = false;
            txt_Description.Enabled = false;
            //ddl_testtype.Enabled = false;
            lnk_Fetch.Enabled = false;
            lbl_OtherPending.Enabled = false;
            lbl_GradeOfConcrte.Enabled = false;
            lbl_Testtype.Enabled = false;
            lbl_Desc.Enabled = false;
            lbl_NatureOfWork.Enabled = false;
            ddl_TestedBy.Enabled = false;
            lbl_RptNo.Enabled = false;
            txt_ReportNo.Enabled = false;
            txt_Qty.Enabled = false;
        }
        public void DisplayCubeCompStrGridRow()
        {
            ddl_TestedBy.Items.Insert(0, "");
            EnableFalse();
            string Material_Name = string.Empty;
            string RecType = string.Empty;
            if (txt_RecordType.Text == "CEMT")
            {
                Material_Name = "Cement Testing";
            }
            else if (txt_RecordType.Text == "FLYASH")
            {
                Material_Name = "Fly Ash Testing";
            }
            else if (txt_RecordType.Text == "GGBS")
            {
                Material_Name = "GGBS Testing";
            }

            int Days = 0;
            int NoOfCubes = 0;
            Days = Convert.ToInt32(lblDays.Text);
            lblcubetype.Text = txt_RecordType.Text;
          
            ShowCastingdt();
            if (Days != 0)
            {
                if (lblCubecompstr.Text == "CementStrength")
                {
                    if (txt_RecordType.Text == "FLYASH" || txt_RecordType.Text == "GGBS")
                    {
                        lblcubetype.Text = txt_RecordType.Text;
                        RecType = "CEMT";
                        txt_RecordType.Text = "CEMT";
                        if ((lblcubetype.Text == "FLYASH" && Days == 28) || lblcubetype.Text == "GGBS")
                        {
                            var Cube = dc.OtherCubeTestView(txt_ReferenceNo.Text, lblcubetype.Text, Convert.ToByte(Days), 0, "CEMT", false, false);
                            foreach (var cm in Cube)
                            {
                                NoOfCubes = Convert.ToInt32(cm.NoOfCubes_tint);
                                break;
                            }
                        }
                    }
                    else
                    {
                        RecType = txt_RecordType.Text;
                    }
                }
                else
                {
                    RecType = txt_RecordType.Text;
                    var Ct = dc.OtherCubeTestView(txt_ReferenceNo.Text, lblcubetype.Text, Convert.ToByte(Days), 0, lblcubetype.Text, false, false);
                    foreach (var cm in Ct)
                    {
                        NoOfCubes = Convert.ToInt32(cm.NoOfCubes_tint);
                    }
                }
            }
            string Avg = string.Empty; 
            if (Convert.ToInt32(lblChkStatus.Text) >= 2 ||   lblCubecompstr.Text == "CemtCubeComprStr" || Convert.ToString(lblResult.Text) != "Awaited")//|| Convert.ToInt32(lblChkStatus.Text) <= 2)
            {
                int i = 0;
                var cubeCompstr = dc.OtherCubeTestView(txt_ReferenceNo.Text, lblcubetype.Text, Convert.ToByte(Days), 0, RecType, false, true);
                foreach (var cubecm in cubeCompstr)
                {
                    AddRowEnterReportCubeInward();
                    TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                    TextBox txt_Length = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txt_Length");
                    TextBox txt_Breadth = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txt_Breadth");
                    TextBox txt_Height = (TextBox)grdCubeInward.Rows[i].Cells[4].FindControl("txt_Height");
                    TextBox txt_Weight = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txt_Weight");
                    TextBox txt_Reading = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txt_Reading");
                    TextBox txt_Age = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txt_Age");
                    TextBox txt_CSArea = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txt_CSArea");
                    TextBox txt_DEnsity = (TextBox)grdCubeInward.Rows[i].Cells[9].FindControl("txt_DEnsity");
                    TextBox txt_CompStr = (TextBox)grdCubeInward.Rows[i].Cells[10].FindControl("txt_CompStr");
                    TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[i].Cells[11].FindControl("txt_AvgStr");
                    txt_IdMark.Text = cubecm.IdMark_var.ToString();
                    txt_Length.Text = cubecm.Length_var.ToString();
                    txt_Breadth.Text = cubecm.Breadth_dec.ToString();
                    txt_Height.Text = cubecm.Height_dec.ToString();
                    txt_Weight.Text = cubecm.Weight_dec.ToString();
                    txt_Reading.Text = cubecm.Reading_var.ToString();
                    txt_Age.Text = cubecm.Age_var.ToString();
                    txt_CSArea.Text = cubecm.CSArea_dec.ToString();
                    txt_DEnsity.Text = cubecm.Density_dec.ToString();
                    txt_CompStr.Text = cubecm.CompStr_var.ToString();
                    if (i == 0)
                    {
                        var cubeAvg = dc.OtherCubeTestView(txt_ReferenceNo.Text, lblcubetype.Text, Convert.ToByte(Days), 0, RecType, false, false);
                        foreach (var cub in cubeAvg)
                        {
                            Avg = Convert.ToString(cub.Avg_var);
                            if (Convert.ToString(cub.TestingDate) != "")
                            {
                                txt_DtOfTesting.Text = Convert.ToDateTime(cub.TestingDate).ToString("dd/MM/yyyy");
                            }
                            if (Convert.ToString(cub.WitnessBy_var) != "" && Convert.ToString(cub.WitnessBy_var) != null)
                            {
                                txt_witnessBy.Visible = true;
                                txt_witnessBy.Text = cub.WitnessBy_var.ToString();
                                chk_WitnessBy.Checked = true;
                            }
                        }
                    }
                    i++;
                }
                if (grdCubeInward.Rows.Count > 0)
                {
                    if (Avg != "")
                    {
                        int NoOfrows = grdCubeInward.Rows.Count / 2;
                        TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[NoOfrows].Cells[11].FindControl("txt_AvgStr");
                        txt_AvgStr.Text = Convert.ToString(Avg);
                    }
                }
            }
            if (grdCubeInward.Rows.Count <= 0)
            {
                for (int c = 0; c < NoOfCubes; c++)
                {
                    AddRowEnterReportCubeInward();
                }
            }
            //Remark
            int r = 0;
            var re = dc.OtherCubeTestRemark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, Convert.ToByte(Days), "CT");
            foreach (var rm in re)
            {
                var remark = dc.OtherCubeTestRemark_View("", "", Convert.ToInt32(rm.RemarkId_int), Convert.ToByte(Days), "CT");
                foreach (var rem in remark)
                {
                    AddRowCubeRemark();
                    TextBox txt_REMARK = (TextBox)grdRemark.Rows[r].Cells[1].FindControl("txt_REMARK");
                    txt_REMARK.Text = rem.Remark_var.ToString();
                    r++;
                }
            }
            for (int j = 0; j < grdCubeInward.Rows.Count; j++)
            {
                TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[j].Cells[1].FindControl("txt_IdMark");
                TextBox txt_Length = (TextBox)grdCubeInward.Rows[j].Cells[2].FindControl("txt_Length");
                TextBox txt_Breadth = (TextBox)grdCubeInward.Rows[j].Cells[3].FindControl("txt_Breadth");
                TextBox txt_Height = (TextBox)grdCubeInward.Rows[j].Cells[4].FindControl("txt_Height");
                TextBox txt_Weight = (TextBox)grdCubeInward.Rows[j].Cells[5].FindControl("txt_Weight");
                TextBox txt_Reading = (TextBox)grdCubeInward.Rows[j].Cells[6].FindControl("txt_Reading");
                if (lblEntry.Text == "Enter")
                {
                    if (Convert.ToInt32(lblChkStatus.Text) >= 2)
                    {
                        txt_IdMark.ReadOnly = true;
                        txt_Length.ReadOnly = true;
                        txt_Breadth.ReadOnly = true;
                        txt_Height.ReadOnly = true;
                        txt_Weight.ReadOnly = true;
                        txt_Reading.ReadOnly = true;
                        // lbl_Msg.Visible = true;
                        txt_DtOfTesting.ReadOnly = true;
                        lnkSave.Enabled = false;
                        //  lbl_Msg.Text = "Available For Checking ! Modification does not allowed";
                    }
                    if (Convert.ToInt32(lblChkStatus.Text) > 2)
                    {
                        // lbl_Msg.Text = "Available For Viewing ! Modification does not allowed";
                    }
                }
                else
                {
                    if (Convert.ToInt32(lblChkStatus.Text) > 2)
                    {
                        txt_IdMark.ReadOnly = true;
                        txt_Length.ReadOnly = true;
                        txt_Breadth.ReadOnly = true;
                        txt_Height.ReadOnly = true;
                        txt_Weight.ReadOnly = true;
                        txt_Reading.ReadOnly = true;
                        // lbl_Msg.Visible = true;
                        lnkSave.Enabled = false;
                        txt_DtOfTesting.ReadOnly = true;
                        //lbl_Msg.Text = "Available For Viewing ! Modification does not allowed";
                    }
                }
            }
            if (grdRemark.Rows.Count <= 0)
            {
                AddRowCubeRemark();
            }
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
            var testinguser = dc.ReportStatus_View("Cube Testing", null, null, 0, 0, 0, "", Convert.ToInt32( lblRecordNo.Text ), 1, 0);
            ddl_OtherPendingRpt.DataTextField = "CTINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataValueField = "CTINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataSource = testinguser;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
            ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(Refno);
            if (itemToRemove != null)
            {
                ddl_OtherPendingRpt.Items.Remove(itemToRemove);
            }
        }
        public void ViewWitnessBy()
        {
            txt_witnessBy.Visible = false;
            chk_WitnessBy.Checked = false;

            var ct = dc.ReportStatus_View("Cube Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var c in ct)
            {
                if (c.CTINWD_WitnessBy_var.ToString() != null && c.CTINWD_WitnessBy_var.ToString() != "")
                {
                    txt_witnessBy.Visible = true;
                    txt_witnessBy.Text = c.CTINWD_WitnessBy_var.ToString();
                    chk_WitnessBy.Checked = true;
                }
            }
        }
        private void LoadOtherPendingCheckRpt()
        {
            if (lblEntry.Text == "Check")
            {
                string Refno = txt_ReferenceNo.Text;
                var testinguser = dc.ReportStatus_View("Cube Testing", null, null, 0, 0, 0, "", Convert.ToInt32( lblRecordNo.Text ), 2, 0);
                ddl_OtherPendingRpt.DataTextField = "CTINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataValueField = "CTINWD_ReferenceNo_var";
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
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowCubeRemark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowCubeRemark(gvr.RowIndex);
            }
        }

        protected void DeleteRowCubeRemark(int rowIndex)
        {
            GetCurrentDataCubeRemark();
            DataTable dt = ViewState["CubeRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CubeRemarkTable"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataCubeRemark();
        }

        protected void AddRowCubeRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CubeRemarkTable"] != null)
            {
                GetCurrentDataCubeRemark();
                dt = (DataTable)ViewState["CubeRemarkTable"];
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

            ViewState["CubeRemarkTable"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataCubeRemark();
        }

        protected void GetCurrentDataCubeRemark()
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
            ViewState["CubeRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataCubeRemark()
        {
            DataTable dt = (DataTable)ViewState["CubeRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");

                grdRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }

        protected void AddRowEnterReportCubeInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CubeTestTable"] != null)
            {
                GetCurrentDataCubeTestInward();
                dt = (DataTable)ViewState["CubeTestTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Length", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Breadth", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Height", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Weight", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Reading", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Age", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CSArea", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_DEnsity", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_CompStr", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_AvgStr", typeof(string)));
                dt.Columns.Add(new DataColumn("lblImage1", typeof(string)));
                dt.Columns.Add(new DataColumn("lblImage2", typeof(string)));
                dt.Columns.Add(new DataColumn("lblImage3", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_IdMark"] = string.Empty;
            dr["txt_Length"] = string.Empty;
            dr["txt_Breadth"] = string.Empty;
            dr["txt_Height"] = string.Empty;
            dr["txt_Weight"] = string.Empty;
            dr["txt_Reading"] = string.Empty;
            dr["txt_Age"] = string.Empty;
            dr["txt_CSArea"] = string.Empty;
            dr["txt_DEnsity"] = string.Empty;
            dr["txt_CompStr"] = string.Empty;
            dr["txt_AvgStr"] = string.Empty;
            dr["lblImage1"] = string.Empty;
            dr["lblImage2"] = string.Empty;
            dr["lblImage3"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["CubeTestTable"] = dt;
            grdCubeInward.DataSource = dt;
            grdCubeInward.DataBind();
            SetPreviousDataCubeTestInward();
        }
        protected void GetCurrentDataCubeTestInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_IdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Length", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Breadth", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Height", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Weight", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Reading", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Age", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CSArea", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_DEnsity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_CompStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_AvgStr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblImage1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblImage2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblImage3", typeof(string)));

            for (int i = 0; i < grdCubeInward.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_Length = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txt_Length");
                TextBox txt_Breadth = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txt_Breadth");
                TextBox txt_Height = (TextBox)grdCubeInward.Rows[i].Cells[4].FindControl("txt_Height");
                TextBox txt_Weight = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txt_Weight");
                TextBox txt_Reading = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txt_Reading");
                TextBox txt_Age = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txt_Age");
                TextBox txt_CSArea = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txt_CSArea");
                TextBox txt_DEnsity = (TextBox)grdCubeInward.Rows[i].Cells[9].FindControl("txt_DEnsity");
                TextBox txt_CompStr = (TextBox)grdCubeInward.Rows[i].Cells[10].FindControl("txt_CompStr");
                TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[i].Cells[11].FindControl("txt_AvgStr");
                Label lblImage1 = (Label)grdCubeInward.Rows[i].FindControl("lblImage1");
                Label lblImage2 = (Label)grdCubeInward.Rows[i].FindControl("lblImage2");
                Label lblImage3 = (Label)grdCubeInward.Rows[i].FindControl("lblImage3");

                drRow = dtTable.NewRow();

                drRow["txt_IdMark"] = txt_IdMark.Text;
                drRow["txt_Length"] = txt_Length.Text;
                drRow["txt_Breadth"] = txt_Breadth.Text;
                drRow["txt_Height"] = txt_Height.Text;
                drRow["txt_Weight"] = txt_Weight.Text;
                drRow["txt_Reading"] = txt_Reading.Text;
                drRow["txt_Age"] = txt_Age.Text;
                drRow["txt_CSArea"] = txt_CSArea.Text;
                drRow["txt_DEnsity"] = txt_DEnsity.Text;
                drRow["txt_CompStr"] = txt_CompStr.Text;
                drRow["txt_AvgStr"] = txt_AvgStr.Text;
                drRow["lblImage1"] = lblImage1.Text;
                drRow["lblImage2"] = lblImage2.Text;
                drRow["lblImage3"] = lblImage3.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CubeTestTable"] = dtTable;

        }
        protected void SetPreviousDataCubeTestInward()
        {
            DataTable dt = (DataTable)ViewState["CubeTestTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_Length = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txt_Length");
                TextBox txt_Breadth = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txt_Breadth");
                TextBox txt_Height = (TextBox)grdCubeInward.Rows[i].Cells[4].FindControl("txt_Height");
                TextBox txt_Weight = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txt_Weight");
                TextBox txt_Reading = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txt_Reading");

                TextBox txt_Age = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txt_Age");
                TextBox txt_CSArea = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txt_CSArea");
                TextBox txt_DEnsity = (TextBox)grdCubeInward.Rows[i].Cells[9].FindControl("txt_DEnsity");
                TextBox txt_CompStr = (TextBox)grdCubeInward.Rows[i].Cells[10].FindControl("txt_CompStr");
                TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[i].Cells[11].FindControl("txt_AvgStr");
                Label lblImage1 = (Label)grdCubeInward.Rows[i].FindControl("lblImage1");
                Label lblImage2 = (Label)grdCubeInward.Rows[i].FindControl("lblImage2");
                Label lblImage3 = (Label)grdCubeInward.Rows[i].FindControl("lblImage3");

                txt_IdMark.Text = dt.Rows[i]["txt_IdMark"].ToString();
                txt_Length.Text = dt.Rows[i]["txt_Length"].ToString();
                txt_Breadth.Text = dt.Rows[i]["txt_Breadth"].ToString();
                txt_Height.Text = dt.Rows[i]["txt_Height"].ToString();
                txt_Weight.Text = dt.Rows[i]["txt_Weight"].ToString();
                txt_Reading.Text = dt.Rows[i]["txt_Reading"].ToString();

                txt_Age.Text = dt.Rows[i]["txt_Age"].ToString();
                txt_CSArea.Text = dt.Rows[i]["txt_CSArea"].ToString();
                txt_DEnsity.Text = dt.Rows[i]["txt_DEnsity"].ToString();
                txt_CompStr.Text = dt.Rows[i]["txt_CompStr"].ToString();
                txt_AvgStr.Text = dt.Rows[i]["txt_AvgStr"].ToString();
                lblImage1.Text = dt.Rows[i]["lblImage1"].ToString();
                lblImage2.Text = dt.Rows[i]["lblImage2"].ToString();
                lblImage3.Text = dt.Rows[i]["lblImage3"].ToString();
            }

        }
        protected void txt_QtyOnTextChanged(object sender, EventArgs e)
        {
            if (txt_Qty.Text != "")
            {
                int qty = 0;
                if (int.TryParse(txt_Qty.Text, out qty))
                {
                    if (Convert.ToInt32(txt_Qty.Text) > 0)
                    {
                        if (Convert.ToInt32(txt_Qty.Text) < grdCubeInward.Rows.Count)
                        {
                            for (int i = grdCubeInward.Rows.Count; i > Convert.ToInt32(txt_Qty.Text); i--)
                            {
                                if (grdCubeInward.Rows.Count > 1)
                                {
                                    DeleteDataCubeTestInward(i - 1);
                                }
                            }
                        }
                        else
                        {
                            DisplayGridRow();
                        }
                    }
                }
                else
                {
                    txt_Qty.Text = string.Empty;
                }
            }
        }
        protected void DeleteDataCubeTestInward(int rowIndex)
        {
            GetCurrentDataCubeTestInward();
            DataTable dt = ViewState["CubeTestTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CubeTestTable"] = dt;
            grdCubeInward.DataSource = dt;
            grdCubeInward.DataBind();
            SetPreviousDataCubeTestInward();
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
            bool valid = true;
            //decimal x;
            if (txt_DtOfCasting.Text == "")
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please enter date of casting.";
                lblMsg.Visible = true;
                valid = false;
            }
            if (valid == true)
            {
                decimal SumCompstr = 0;
                for (int i = 0; i < grdCubeInward.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                    TextBox txt_Length = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txt_Length");
                    TextBox txt_Breadth = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txt_Breadth");
                    TextBox txt_Height = (TextBox)grdCubeInward.Rows[i].Cells[4].FindControl("txt_Height");
                    TextBox txt_Weight = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txt_Weight");
                    TextBox txt_Reading = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txt_Reading");

                    TextBox txt_Age = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txt_Age");
                    TextBox txt_CSArea = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txt_CSArea");
                    TextBox txt_DEnsity = (TextBox)grdCubeInward.Rows[i].Cells[9].FindControl("txt_DEnsity");
                    TextBox txt_CompStr = (TextBox)grdCubeInward.Rows[i].Cells[10].FindControl("txt_CompStr");
                    TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[i].Cells[11].FindControl("txt_AvgStr");
                    txt_AvgStr.Text = string.Empty;
                    if (txt_DtOfCasting.Text != "NA")
                    {
                        DateTime Testdt = DateTime.ParseExact(txt_DtOfTesting.Text, "dd/MM/yyyy", null);
                        // DateTime Castdt = Convert.ToDateTime(txt_DtOfCasting.Text);
                        DateTime Castdt = DateTime.ParseExact(txt_DtOfCasting.Text, "dd/MM/yyyy", null);
                        int AgeDays = 0;
                        AgeDays = Convert.ToInt32((Testdt - Castdt).TotalDays);
                        txt_Age.Text = Convert.ToInt32(AgeDays).ToString();

                    }
                    else
                    {
                        txt_Age.Text = "NA";
                    }
                    if (txt_Length.Text != "" && txt_Breadth.Text != "")
                    {
                        txt_CSArea.Text = (Convert.ToDecimal(txt_Length.Text) * Convert.ToDecimal(txt_Breadth.Text)).ToString("0.00");
                    }
                    if (txt_Weight.Text != "" && txt_Length.Text != "" && txt_Height.Text != ""
                        && txt_Breadth.Text != "")
                    {
                        txt_DEnsity.Text = (Convert.ToDecimal(txt_Weight.Text) / ((Convert.ToDecimal(txt_Length.Text) *
                                                                    Convert.ToDecimal(txt_Height.Text) *
                                                                    Convert.ToDecimal(txt_Breadth.Text) / 1000000000))).ToString("0.00");
                    }

                    if (txt_Reading.Text != "" && txt_CSArea.Text != "")
                    {
                        if (txt_Reading.Text == "#")
                        {
                            txt_Reading.Text = "#";
                            txt_CompStr.Text = "---";

                        }
                        else
                        {
                            txt_CompStr.Text = ((Convert.ToDecimal(txt_Reading.Text)) / (Convert.ToDecimal(txt_CSArea.Text) / 1000)).ToString("0.00");
                            //if (txt_RecordType.Text == "MF" && txt_Age.Text == "1" && ddl_testtype.SelectedItem.Text == "Accelerated Curing")
                            if (txt_Age.Text == "1" && ddl_testtype.SelectedItem.Text == "Accelerated Curing")
                            {
                                txt_CompStr.Text = ((Convert.ToDecimal(txt_CompStr.Text) * Convert.ToDecimal(1.64)) + Convert.ToDecimal(8.09)).ToString("0.00");
                            }

                            SumCompstr += Convert.ToDecimal(txt_CompStr.Text);
                        }
                    }
                }
                // added newly  --avg of 2 nearest values
                #region avg of 2 nearest values
                bool flgAvg3 = true;
                decimal avgNew = 0;
                if (txt_RecordType.Text == "CT" || txt_RecordType.Text == "MF")
                {
                    decimal[] mCompStr = new decimal[3];
                    avgNew = SumCompstr / 3;
                    avgNew = Math.Round(avgNew, 2);
                    for (int i = 0; i < grdCubeInward.Rows.Count; i++)
                    {
                        TextBox txt_CompStr = (TextBox)grdCubeInward.Rows[i].Cells[10].FindControl("txt_CompStr");
                        mCompStr[i] = Convert.ToDecimal(txt_CompStr.Text);
                        if (Convert.ToDecimal(txt_CompStr.Text) < (avgNew * Convert.ToDecimal("0.85"))
                            || Convert.ToDecimal(txt_CompStr.Text) > (avgNew * Convert.ToDecimal("1.15")))
                        {
                            flgAvg3 = false;
                        }
                    }
                    if (flgAvg3 == false)
                    {
                        SumCompstr = 0;
                        decimal[] diff = new decimal[3];
                        diff[0] = Math.Abs(mCompStr[0] - mCompStr[1]);
                        diff[1] = Math.Abs(mCompStr[1] - mCompStr[2]);
                        diff[2] = Math.Abs(mCompStr[0] - mCompStr[2]);
                        if (diff[0] == diff.Min())
                        {
                            SumCompstr = mCompStr[0] + mCompStr[1];
                        }
                        else if (diff[1] == diff.Min())
                        {
                            SumCompstr = mCompStr[1] + mCompStr[2];
                        }
                        else if (diff[2] == diff.Min())
                        {
                            SumCompstr = mCompStr[0] + mCompStr[2];
                        }
                        if (SumCompstr > 0)
                        {
                            avgNew = SumCompstr / 2;
                            avgNew = Math.Round(avgNew * 2, MidpointRounding.ToEven) / 2;
                        }
                    }
                }
                #endregion 

                int NoOfrows = grdCubeInward.Rows.Count / 2;
                //for (int i = 0; i < grdCubeInward.Rows.Count; i++)
                //{
                //    //TextBox txt_Reading = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txt_Reading");
                TextBox txt_AvgStrr = (TextBox)grdCubeInward.Rows[NoOfrows].Cells[11].FindControl("txt_AvgStr");

                //    if (i == grdCubeInward.Rows.Count - 1)
                //    {
                if (grdCubeInward.Rows.Count > 0 && SumCompstr > 0)
                {
                    if (txt_RecordType.Text == "CEMT" || txt_RecordType.Text == "FLYASH" || txt_RecordType.Text == "GGBS")
                        txt_AvgStrr.Text = Math.Round(Convert.ToDecimal(SumCompstr / grdCubeInward.Rows.Count), 0).ToString();
                    else if (flgAvg3 == false)
                        txt_AvgStrr.Text = avgNew.ToString("0.00");
                    else
                    {
                        avgNew = Convert.ToDecimal(SumCompstr / grdCubeInward.Rows.Count);
                        avgNew = Math.Round(avgNew * 2, MidpointRounding.ToEven) / 2;
                        txt_AvgStrr.Text = avgNew.ToString("0.00");
                        //txt_AvgStrr.Text = Convert.ToDecimal(SumCompstr / grdCubeInward.Rows.Count).ToString("0.00");
                    }
                }
                else
                {
                    //if (txt_Reading.Text == "#")
                    //{
                    txt_AvgStrr.Text = "***";
                }
                //    }

                //}
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                DateTime DateofTesting = DateTime.ParseExact(txt_DtOfTesting.Text, "dd/MM/yyyy", null);
                #region Save MixDesign
                if (lblCubecompstr.Text == "TrialCubeCompStr")
                {
                    dc.OtherCubeTestDetail_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", Convert.ToByte(ddl_OtherPendingRpt.SelectedValue), "", "", null, "Trial", txt_RecordType.Text, "", "", "", false, false, true,Convert.ToInt32(ddlTrial.SelectedValue ));
                    for (int i = 0; i < grdCubeInward.Rows.Count; i++)
                    {
                        TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                        TextBox txt_Length = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txt_Length");
                        TextBox txt_Breadth = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txt_Breadth");
                        TextBox txt_Height = (TextBox)grdCubeInward.Rows[i].Cells[4].FindControl("txt_Height");
                        TextBox txt_Weight = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txt_Weight");
                        TextBox txt_Reading = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txt_Reading");
                        TextBox txt_Age = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txt_Age");
                        TextBox txt_CSArea = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txt_CSArea");
                        TextBox txt_DEnsity = (TextBox)grdCubeInward.Rows[i].Cells[9].FindControl("txt_DEnsity");
                        TextBox txt_CompStr = (TextBox)grdCubeInward.Rows[i].Cells[10].FindControl("txt_CompStr");
                        TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[i].Cells[11].FindControl("txt_AvgStr");
                        Label lblImage1 = (Label)grdCubeInward.Rows[i].FindControl("lblImage1");
                        Label lblImage2 = (Label)grdCubeInward.Rows[i].FindControl("lblImage2");
                        Label lblImage3 = (Label)grdCubeInward.Rows[i].FindControl("lblImage3");

                        dc.OtherCubeTestDetail_Update(txt_ReferenceNo.Text, txt_IdMark.Text, Convert.ToDecimal(txt_Length.Text), Convert.ToDecimal(txt_Breadth.Text),
                        Convert.ToDecimal(txt_Height.Text), Convert.ToDecimal(txt_Weight.Text), txt_Reading.Text, Convert.ToString(txt_Age.Text),
                        Convert.ToDecimal(txt_CSArea.Text), Convert.ToDecimal(txt_DEnsity.Text), txt_CompStr.Text, Convert.ToByte(ddl_OtherPendingRpt.SelectedValue), "", "", null, "Trial", txt_RecordType.Text, lblImage1.Text, lblImage2.Text, lblImage3.Text, false, false, false, Convert.ToInt32(ddlTrial.SelectedValue));
                    }
                    for (int i = 0; i < grdCubeInward.Rows.Count; i++)
                    {
                        TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[i].Cells[11].FindControl("txt_AvgStr");
                        if (txt_AvgStr.Text != "")
                        {
                            dc.OtherCubeTestDetail_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", Convert.ToByte(ddl_OtherPendingRpt.SelectedValue), txt_AvgStr.Text, "", null, "Trial", txt_RecordType.Text, "", "", "", true, false, false, Convert.ToInt32(ddlTrial.SelectedValue));
                        }
                    }
                    dc.OtherCubeTestDetail_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", Convert.ToByte(ddl_OtherPendingRpt.SelectedValue), "", Convert.ToString(txt_witnessBy.Text), DateofTesting, "Trial", txt_RecordType.Text,"", "", "", false, true, false, Convert.ToInt32(ddlTrial.SelectedValue));
                                        
                    //Remark 
                    int RemarkId = 0;
                    dc.OtherCubeTestRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue), true);
                    for (int i = 0; i < grdRemark.Rows.Count; i++)
                    {
                        TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                        if (txt_REMARK.Text != "")
                        {
                            bool valid = false;
                            var chcek = dc.OtherCubeTestRemark_View(txt_REMARK.Text, "", 0, Convert.ToByte(ddl_OtherPendingRpt.SelectedValue), "MF");
                            foreach (var n in chcek)
                            {
                                valid = true;
                                RemarkId = Convert.ToInt32(n.RemarkId_int);
                                Boolean chk = false;
                                var chkId = dc.OtherCubeTestRemark_View("", txt_ReferenceNo.Text, 0, Convert.ToByte(ddl_OtherPendingRpt.SelectedValue), "MF");
                                foreach (var c in chkId)
                                {
                                    if (c.RemarkId_int == RemarkId)
                                    {
                                        chk = true;
                                    }
                                }
                                if (chk == false)
                                {
                                    dc.OtherCubeTestRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue), false);
                                }
                            }
                            if (valid == false)
                            {
                                dc.OtherCubeTestRemark_Update(0, txt_REMARK.Text);
                                var chc = dc.OtherCubeTestRemark_View(txt_REMARK.Text, "", 0, Convert.ToByte(ddl_OtherPendingRpt.SelectedValue), "MF");
                                foreach (var n in chc)
                                {
                                    RemarkId = Convert.ToInt32(n.RemarkId_int);
                                    dc.OtherCubeTestRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue), false);
                                }
                            }
                        }
                    }
                }
                #endregion
                #region Save FLAYSH, Cement, Ggbs
                else if (lblCubecompstr.Text == "CubeCompStrength" || lblCubecompstr.Text == "CementStrength")
                {
                    int Days = 0;
                    
                    Days = Convert.ToInt32(lblDays.Text);

                    //dc.OtherCubeTestDetail_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", Convert.ToByte(Days), "", "", null, txt_RecordType.Text, Convert.ToString(txt_RecordType.Text), false, false, true);
                    dc.OtherCubeTestDetail_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", Convert.ToByte(Days), "", "", null, txt_RecordType.Text, lblcubetype.Text, "", "", "", false, false, true,0);
                    for (int i = 0; i < grdCubeInward.Rows.Count; i++)
                    {
                        TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                        TextBox txt_Length = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txt_Length");
                        TextBox txt_Breadth = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txt_Breadth");
                        TextBox txt_Height = (TextBox)grdCubeInward.Rows[i].Cells[4].FindControl("txt_Height");
                        TextBox txt_Weight = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txt_Weight");
                        TextBox txt_Reading = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txt_Reading");
                        TextBox txt_Age = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txt_Age");
                        TextBox txt_CSArea = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txt_CSArea");
                        TextBox txt_DEnsity = (TextBox)grdCubeInward.Rows[i].Cells[9].FindControl("txt_DEnsity");
                        TextBox txt_CompStr = (TextBox)grdCubeInward.Rows[i].Cells[10].FindControl("txt_CompStr");
                        TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[i].Cells[11].FindControl("txt_AvgStr");

                        //dc.OtherCubeTestDetail_Update(txt_ReferenceNo.Text, txt_IdMark.Text, Convert.ToDecimal(txt_Length.Text), Convert.ToDecimal(txt_Breadth.Text),
                        //Convert.ToDecimal(txt_Height.Text), Convert.ToDecimal(txt_Weight.Text), txt_Reading.Text, Convert.ToString(txt_Age.Text),
                        //Convert.ToDecimal(txt_CSArea.Text), Convert.ToDecimal(txt_DEnsity.Text), txt_CompStr.Text, Convert.ToByte(Days), "", "", null, txt_RecordType.Text, Convert.ToString(txt_RecordType.Text), false, false, false);
                        dc.OtherCubeTestDetail_Update(txt_ReferenceNo.Text, txt_IdMark.Text, Convert.ToDecimal(txt_Length.Text), Convert.ToDecimal(txt_Breadth.Text),
                            Convert.ToDecimal(txt_Height.Text), Convert.ToDecimal(txt_Weight.Text), txt_Reading.Text, Convert.ToString(txt_Age.Text),
                            Convert.ToDecimal(txt_CSArea.Text), Convert.ToDecimal(txt_DEnsity.Text), txt_CompStr.Text, Convert.ToByte(Days), "", "", null, txt_RecordType.Text, lblcubetype.Text, "", "", "", false, false, false, 0);
                    }
                    for (int i = 0; i < grdCubeInward.Rows.Count; i++)
                    {
                        TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[i].Cells[11].FindControl("txt_AvgStr");
                        if (txt_AvgStr.Text != "")
                        {
                            //dc.OtherCubeTestDetail_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", Convert.ToByte(Days), txt_AvgStr.Text, "", null, txt_RecordType.Text, Convert.ToString(txt_RecordType.Text), true, false, false);                            
                            dc.OtherCubeTestDetail_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", Convert.ToByte(Days), txt_AvgStr.Text, "", null, txt_RecordType.Text, lblcubetype.Text, "", "", "", true, false, false, 0);
                        }
                    }
                    //dc.OtherCubeTestDetail_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", Convert.ToByte(Days), "", Convert.ToString(txt_witnessBy.Text), DateofTesting, txt_RecordType.Text, Convert.ToString(txt_RecordType.Text), false, true, false);
                    dc.OtherCubeTestDetail_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", Convert.ToByte(Days), "", Convert.ToString(txt_witnessBy.Text), DateofTesting, txt_RecordType.Text, lblcubetype.Text, "", "", "", false, true, false, 0);
                    //Remark 
                    int RemarkId = 0;
                    dc.OtherCubeTestRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, Days, true);
                    for (int i = 0; i < grdRemark.Rows.Count; i++)
                    {
                        TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                        if (txt_REMARK.Text != "")
                        {
                            bool valid = false;
                            var chcek = dc.OtherCubeTestRemark_View(txt_REMARK.Text, "", 0, Convert.ToByte(Days), "CT");
                            foreach (var n in chcek)
                            {
                                valid = true;
                                RemarkId = Convert.ToInt32(n.RemarkId_int);
                                Boolean chk = false;
                                var chkId = dc.OtherCubeTestRemark_View("", txt_ReferenceNo.Text, 0, Convert.ToByte(Days), "CT");
                                foreach (var c in chkId)
                                {
                                    if (c.RemarkId_int == RemarkId)
                                    {
                                        chk = true;
                                    }
                                }
                                if (chk == false)
                                {
                                    dc.OtherCubeTestRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, Days, false);
                                }
                            }
                            if (valid == false)
                            {
                                dc.OtherCubeTestRemark_Update(0, txt_REMARK.Text);
                                var chc = dc.OtherCubeTestRemark_View(txt_REMARK.Text, "", 0, Convert.ToByte(Days), "CT");
                                foreach (var n in chc)
                                {
                                    RemarkId = Convert.ToInt32(n.RemarkId_int);
                                    dc.OtherCubeTestRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, Days, false);
                                }
                            }
                        }
                    }
                }
                #endregion
                else
                {
                    txt_DtOfCasting.Text = txt_DtOfCasting.Text.ToUpper();
                    dc.CubeTest_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", 0, "", "", "", "", "", null, "", 0, "", "", "", "", 0, true);//delete Cube Test
                    txt_Qty.Text = grdCubeInward.Rows.Count.ToString();
                    if (chk_WitnessBy.Checked == false)
                    {
                        txt_witnessBy.Text = "";
                    }
                    string mGrade = "0";
                    if (ddl_gradeOfConcrete.Text != "NA")
                        mGrade = ddl_gradeOfConcrete.Text.Replace("M ", "");
                    if (lbl_TestedBy.Text == "Tested By")
                    {
                        dc.CubeTest_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", 2, txt_witnessBy.Text, txt_DtOfCasting.Text, mGrade, txt_Description.Text, ddl_testtype.SelectedItem.Text, DateofTesting, txt_NatureOfwork.Text, Convert.ToByte(txt_Qty.Text), "", "", "", "", 0, false);
                        dc.ReportDetails_Update("CT", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                        dc.MISDetail_Update(0, "CT", txt_ReferenceNo.Text, "CT", null, true, false, false, false, false, false, false);
                    }
                    else if (lbl_TestedBy.Text == "Approve By")
                    {
                        dc.CubeTest_Update(txt_ReferenceNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", 3, txt_witnessBy.Text, txt_DtOfCasting.Text, mGrade, txt_Description.Text, ddl_testtype.SelectedItem.Text, DateofTesting, txt_NatureOfwork.Text, Convert.ToByte(txt_Qty.Text), "", "", "", "", 0, false);
                        dc.ReportDetails_Update("CT", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                        dc.MISDetail_Update(0, "CT", txt_ReferenceNo.Text, "CT", null, false, true, false, false, false, false, false);
                    }
                    //update NABL details
                  
                        clsData cd = new clsData();
                        string nablScope = "";
                        if (grdCubeInward.Rows.Count < 3 && ddl_testtype.SelectedItem.Text == "Concrete Cube")
                            nablScope = "NA";
                        else
                            nablScope = ddl_NablScope.SelectedItem.Text;
                  
                    cd.updateNABLDetails(txt_ReferenceNo.Text, "CT", nablScope, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                    for (int i = 0; i < grdCubeInward.Rows.Count; i++)
                    {
                        TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                        TextBox txt_Length = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txt_Length");
                        TextBox txt_Breadth = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txt_Breadth");
                        TextBox txt_Height = (TextBox)grdCubeInward.Rows[i].Cells[4].FindControl("txt_Height");
                        TextBox txt_Weight = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txt_Weight");
                        TextBox txt_Reading = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txt_Reading");

                        TextBox txt_Age = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txt_Age");
                        TextBox txt_CSArea = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txt_CSArea");
                        TextBox txt_DEnsity = (TextBox)grdCubeInward.Rows[i].Cells[9].FindControl("txt_DEnsity");
                        TextBox txt_CompStr = (TextBox)grdCubeInward.Rows[i].Cells[10].FindControl("txt_CompStr");
                        TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[i].Cells[11].FindControl("txt_AvgStr");

                        Label lblImage1 = (Label)grdCubeInward.Rows[i].FindControl("lblImage1");
                        Label lblImage2 = (Label)grdCubeInward.Rows[i].FindControl("lblImage2");
                        Label lblImage3 = (Label)grdCubeInward.Rows[i].FindControl("lblImage3");

                        if (txt_Reading.Text == "#")
                        {
                            txt_Reading.Text = "#";
                        }

                        if (txt_Age.Text == "")
                        {
                            txt_Age.Text = "";

                        }
                        if (txt_CompStr.Text == "---")
                        {
                            txt_CompStr.Text = "---";
                        }
                        if (txt_AvgStr.Text != "")
                        {
                            dc.CubeTest_Update(txt_ReferenceNo.Text, "", 0, 0, 0,
                                               0, "",
                                               "", 0,
                                               0, "", 0, "", "", "", "", "", null, "", 0, txt_AvgStr.Text, "","","",0, false);
                        }
                        dc.CubeTest_Update(txt_ReferenceNo.Text, txt_IdMark.Text, Convert.ToDecimal(txt_Length.Text), Convert.ToDecimal(txt_Breadth.Text), Convert.ToDecimal(txt_Height.Text),
                                                 Convert.ToDecimal(txt_Weight.Text), txt_Reading.Text,
                                                 txt_Age.Text, Convert.ToDecimal(txt_CSArea.Text),
                                                 Convert.ToDecimal(txt_DEnsity.Text), txt_CompStr.Text, 0, "", "", "", "", "", null, "", 0, "", lblImage1.Text, lblImage2.Text, lblImage3.Text, (i+1), false);
                    }

                    //Remark 
                    #region Remark Gridview
                    int RemarkId = 0;
                    dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "CT", true);
                    for (int i = 0; i < grdRemark.Rows.Count; i++)
                    {
                        TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                        if (txt_REMARK.Text != "")
                        {
                            bool valid = false;
                            var chcek = dc.AllRemark_View(txt_REMARK.Text, "", 0, "CT");
                            foreach (var n in chcek)
                            {
                                valid = true;
                                RemarkId = Convert.ToInt32(n.CT_RemarkId_int);
                                Boolean chk = false;
                                var chkId = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "CT");
                                foreach (var c in chkId)
                                {
                                    if (c.CTDetail_RemarkId_int == RemarkId)
                                    {
                                        chk = true;
                                    }
                                }
                                if (chk == false)
                                {
                                    dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "CT", false);
                                }
                            }
                            if (valid == false)
                            {
                                dc.AllRemark_Update(0, txt_REMARK.Text, "CT");
                                var chc = dc.AllRemark_View(txt_REMARK.Text, "", 0, "CT");
                                foreach (var n in chc)
                                {
                                    RemarkId = Convert.ToInt32(n.CT_RemarkId_int);
                                    dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "CT", false);
                                }
                            }
                        }
                    }
                    #endregion
                    //approve cube report
                    if (lbl_TestedBy.Text == "Approve By")
                    {   
                        ApproveReports();
                    }
                }
                lnkPrint.Visible = true;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = true;
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.ForeColor = System.Drawing.Color.Green;
                //  ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Record Saved Successfully');", true);
                lnkSave.Enabled = false;
            }
        }

        public void ApproveReports()
        {
            string ReferenceNo = txt_ReferenceNo.Text;            
            int RecordNo = Convert.ToInt32(txt_ReportNo.Text.Split('/')[0]);
            
            //dc.ReportDetails_Update("CT", ReferenceNo, 0, 0, 0, Convert.ToByte(Convert.ToByte(ddl_TestedBy.SelectedValue)), true, "Approved By");
            //dc.MISDetail_Update(0, "CT", ReferenceNo, "CT", null, false, false, true, false, false, false, false);

            #region crBitUpdate
            //update MISCRLApprStatus to 1 if SITE_CRBypass_bit is 1
            int siteCRbypssBit = 0; clsData cd = new clsData();
            siteCRbypssBit = cd.getClientCrlBypassBit("CT", RecordNo);
            //cd.getSITECRBypassBit("CT", RecordNo);
            if (siteCRbypssBit == 1)
                dc.MISDETAIL_Update_CRLimitBit(ReferenceNo, "CT");

            //old dc.MISDETAIL_Update_CRLimitBit(txt_ReferenceNo.Text, Convert.ToInt32(txt_ReportNo.Text.Split('/')[0]), "CT");
                 
            #endregion


            dc.ReportDetails_Update("CT", ReferenceNo, 0, 0, 0, Convert.ToByte(Convert.ToByte(ddl_TestedBy.SelectedValue)), true, "Approved By");
            dc.MISDetail_Update(0, "CT", ReferenceNo, "CT", null, false, false, true, false, false, false, false);
                       
        }
        public bool IsValidEmailAddress(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");
            }
        }  
        protected Boolean ValidateData()
        {
            Boolean valid = true;
            Label lblMsg = (Label)Master.FindControl("lblMsg");//
            lblMsg.ForeColor = System.Drawing.Color.Red;
            string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime TestingDate = DateTime.ParseExact(txt_DtOfTesting.Text, "dd/MM/yyyy", null);
            DateTime currentDate1 = DateTime.ParseExact(CurrentDate, "dd/MM/yyyy", null);
            if (TestingDate > currentDate1)
            {
                lblMsg.Text = "Testing Date should be always less than or equal to the Current Date.";
                valid = false;
            }
            else if (lblCubecompstr.Text == "TrialCubeCompStr" && txt_ReferenceNo.Text == "")
            {
                lblMsg.Text = "Please Select Report No.";
                valid = false;
            }
            else if (lblCubecompstr.Text == "TrialCubeCompStr" && (ddlTrial.Items.Count == 0 || ddlTrial.SelectedIndex == 0))
            {
                lblMsg.Text = "Please Select Trial.";
                valid = false;
            }
            else if (lblCubecompstr.Text != "" && lblCubecompstr.Text != "CubeCompStrength" && lblCubecompstr.Text != "CementStrength" && ddl_OtherPendingRpt.SelectedItem.Text == "---Select---" && lblCubecompstr.Text == "TrialCubeCompStr")
            {
                lblMsg.Text = "Please Select Days";
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
            else if (lblEntry.Text == "Check" && lblCubecompstr.Text != "CubeCompStrength" && lblCubecompstr.Text != "CementStrength" && ddl_TestedBy.SelectedItem.Text == "---Select---" && lblCubecompstr.Text != "TrialCubeCompStr")
            {
                lblMsg.Text = "Please Select the Approve By";
                valid = false;
            }
            else if (lblEntry.Text == "Enter" && lblCubecompstr.Text != "CubeCompStrength" && lblCubecompstr.Text != "CementStrength" && ddl_TestedBy.SelectedItem.Text == "---Select---" && lblCubecompstr.Text != "TrialCubeCompStr")
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
                for (int i = 0; i < grdCubeInward.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                    TextBox txt_Length = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txt_Length");
                    TextBox txt_Breadth = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txt_Breadth");
                    TextBox txt_Height = (TextBox)grdCubeInward.Rows[i].Cells[4].FindControl("txt_Height");
                    TextBox txt_Weight = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txt_Weight");
                    TextBox txt_Reading = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txt_Reading");

                    if (txt_IdMark.Text == "")
                    {
                        lblMsg.Text = "Enter Id Mark for Sr No. " + (i + 1) + ".";
                        txt_IdMark.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_Length.Text == "")
                    {
                        lblMsg.Text = "Enter Length for Sr No. " + (i + 1) + ".";
                        txt_Length.Focus();
                        valid = false;
                        break;
                    }
                    else if (Convert.ToDecimal(txt_Length.Text.ToString()) < (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) || Convert.ToDecimal(txt_Length.Text.ToString()) > (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5))
                    {
                        lblMsg.Text = "Invalid Length for Sr No. " + (i + 1) + ". Required in Range " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) + " - " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5) + ".";
                        txt_Length.Focus();
                        valid = false;
                        break; 
                    }
                    if (txt_Breadth.Text == "")
                    {
                        lblMsg.Text = "Enter Breadth for Sr No. " + (i + 1) + ".";
                        txt_Breadth.Focus();
                        valid = false;
                        break;
                    }
                    else if (Convert.ToDecimal(txt_Breadth.Text.ToString()) < (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) || Convert.ToDecimal(txt_Breadth.Text.ToString()) > (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5))
                    {
                        lblMsg.Text = "Invalid Breadth for Sr No. " + (i + 1) + ". Required in Range " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) + " - " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5) + ".";
                        txt_Breadth.Focus();
                        valid = false;
                        break;

                    }
                    if (txt_Height.Text == "")
                    {
                        lblMsg.Text = "Enter Height for Sr No. " + (i + 1) + ".";
                        txt_Height.Focus();
                        valid = false;
                        break;
                    }
                    else if (Convert.ToDecimal(txt_Height.Text.ToString()) < (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) || Convert.ToDecimal(txt_Height.Text.ToString()) > (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5))
                    {
                        lblMsg.Text = "Invalid Height for Sr No. " + (i + 1) + ". Required in Range " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) - 5) + " - " + (Convert.ToDecimal(ddlCubeSize.SelectedValue) + 5) + ".";
                        txt_Height.Focus();
                        valid = false;
                        break;

                    }
                    if (txt_Weight.Text == "")
                    {
                        lblMsg.Text = "Enter Weight for Sr No. " + (i + 1) + ".";
                        txt_Weight.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_Reading.Text == "")
                    {
                        lblMsg.Text = "Enter Reading for Sr No. " + (i + 1) + ".";
                        txt_Reading.Focus();
                        valid = false;
                        break;
                    }

                }
            }
            if (valid == true)
            {
                if (lbl_TestedBy.Text == "Approve By")
                {
                    string ReferenceNo = txt_ReferenceNo.Text;
                    string Recordtype = "CT"; //, EmailId = "";
                    int RecordNo = Convert.ToInt32(txt_ReportNo.Text.Split('/')[0]);

                    #region Bill Generation checking
                    bool generateBillFlag = true;
                    if (DateTime.Now.Day >= 26)
                    {
                        generateBillFlag = false;
                    }
                    if (generateBillFlag == true)
                    {
                        var inward = dc.Inward_View(0, RecordNo, Recordtype, null, null).ToList();
                        foreach (var inwd in inward)
                        {
                            if (inwd.INWD_BILL_Id != null && inwd.INWD_BILL_Id != "0")
                            {
                                generateBillFlag = false;
                            }
                            if (inwd.SITE_MonthlyBillingStatus_bit != null && inwd.SITE_MonthlyBillingStatus_bit == true)
                            {
                                generateBillFlag = false;
                            }
                            if (inwd.INWD_MonthlyBill_bit == true)
                            {
                                generateBillFlag = false;
                            }
                        }
                    }
                    if (generateBillFlag == true && Recordtype == "CT")
                    {
                        var ctinwd = dc.AllInwdDetails_View(ReferenceNo, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, RecordNo, "", 0, Recordtype);
                        foreach (var ct in ctinwd)
                        {
                            if (ct.CTINWD_CouponNo_var != null && ct.CTINWD_CouponNo_var != "")
                            {
                                generateBillFlag = false;
                                break;
                            }
                        }
                    }
                    if (generateBillFlag == true)
                    {
                        var withoutbill = dc.WithoutBill_View(RecordNo, Recordtype);
                        if (withoutbill.Count() > 0)
                        {
                            generateBillFlag = false;
                        }
                    }
                    if (generateBillFlag == true)
                    {
                        int NewrecNo = 0;
                        clsData clsObj = new clsData();
                        NewrecNo = clsObj.GetCurrentRecordNo("BillNo");
                        //var gstbillCount = dc.Bill_View("0", 0, 0, "", 0, false, false, DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                        var gstbillCount = dc.Bill_View_Count(DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                        if (gstbillCount.Count() != NewrecNo - 1)
                        {
                            generateBillFlag = false;
                            lblMsg.Text = "Bill No. mismatch. Can not check report.";                            
                            valid = false;
                        }
                    }
                   
                    #endregion
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

        public void FetchRefNo()
        {
            try
            {
                lnkSave.Enabled = true;
                lnkPrint.Visible = false;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                txt_ReferenceNo.Text = ddl_OtherPendingRpt.SelectedItem.Text;
                txt_ReferenceNo.Text = txt_ReferenceNo.Text;
                DisplayCubeDetails();
                //LoadOtherPendingCheckRpt();
                LoadReferenceNoList();
                ViewWitnessBy();
               // LoadApproveBy();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            finally
            {
                grdCubeInward.DataSource = null;
                grdCubeInward.DataBind();
                DisplayCubeGrid();
                DisplayIdMark();
                grdRemark.DataSource = null;
                grdRemark.DataBind();
                DisplayRemark();
            }
        }
        public void DisplayGridRow()
        {
            if (txt_Qty.Text != "")
            {
                if (Convert.ToInt32(txt_Qty.Text) > 0)
                {
                    if (Convert.ToInt32(txt_Qty.Text) > grdCubeInward.Rows.Count)
                    {
                        for (int i = grdCubeInward.Rows.Count + 1; i <= Convert.ToInt32(txt_Qty.Text); i++)
                        {
                            AddRowEnterReportCubeInward();
                        }
                    }
                }
            }
        }
        public void DisplayCubeDetails()
        {
            var Inwardc = dc.ReportStatus_View("Cube Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var cu in Inwardc)
            {
                txt_ReferenceNo.Text = cu.CTINWD_ReferenceNo_var.ToString();
                txt_RecordType.Text = "CT";
                if (cu.CTINWD_CastingDate_dt != "NA")
                {
                    txt_DtOfCasting.Text = cu.CTINWD_CastingDate_dt.ToString();
                }
                else
                {
                    txt_DtOfCasting.Text = "NA";
                }
                txt_DtOfTesting.Text = Convert.ToDateTime(cu.CTINWD_TestingDate_dt).ToString("dd/MM/yyyy");
                txt_NatureOfwork.Text = cu.CTINWD_WorkingNature_var.ToString();
                txt_Description.Text = cu.CTINWD_Description_var.ToString();
                ddl_gradeOfConcrete.ClearSelection();
                if (cu.CTINWD_Grade_var == "0")
                    ddl_gradeOfConcrete.SelectedValue = "NA";
                else
                    ddl_gradeOfConcrete.SelectedValue = "M " + "" + cu.CTINWD_Grade_var.ToString();
                ddl_testtype.ClearSelection();
                txt_Qty.Text = cu.CTINWD_Quantity_tint.ToString();
                //ddl_testtype.SelectedItem.Text = cu.CTINWD_TestType_var.ToString();
                ddl_testtype.SelectedValue = cu.CTINWD_TestType_var;
                txt_ReportNo.Text = cu.CTINWD_SetOfRecord_var.ToString();

                if (ddl_NablScope.Items.FindByValue(cu.CTINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(cu.CTINWD_NablScope_var);
                }
                //else if (dc.Connection.Database.ToLower() == "veenanashik")
                //{
                //    ddl_NablScope.SelectedValue = "NA";
                //}
                else
                {
                    ddl_NablScope.SelectedValue = "F";
                }
                if (Convert.ToString(cu.CTINWD_NablLocation_int) != null && Convert.ToString(cu.CTINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(cu.CTINWD_NablLocation_int);
                }


            }


        }
        public void DisplayIdMark()
        {
            var ct = dc.ReportStatus_View("Cube Testing", null, null, 0, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 2, 0);
            foreach (var cu in ct)
            {
                for (int i = 0; i < grdCubeInward.Rows.Count; i++)
                {
                    TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                    txt_IdMark.Text = cu.CTINWD_IdMark_var.ToString();
                }
            }
        }
        public void DisplayCubeGrid()
        {
            grdCubeInward.DataSource = null;
            grdCubeInward.DataBind();
            int i = 0;
            var ct = dc.CubeTestDetails_View(Convert.ToString(txt_ReferenceNo.Text), "CT");
            foreach (var cu in ct)
            {
                AddRowEnterReportCubeInward();
                TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_Length = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txt_Length");
                TextBox txt_Breadth = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txt_Breadth");
                TextBox txt_Height = (TextBox)grdCubeInward.Rows[i].Cells[4].FindControl("txt_Height");
                TextBox txt_Weight = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txt_Weight");
                TextBox txt_Reading = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txt_Reading");

                TextBox txt_Age = (TextBox)grdCubeInward.Rows[i].Cells[7].FindControl("txt_Age");
                TextBox txt_CSArea = (TextBox)grdCubeInward.Rows[i].Cells[8].FindControl("txt_CSArea");
                TextBox txt_DEnsity = (TextBox)grdCubeInward.Rows[i].Cells[9].FindControl("txt_DEnsity");
                TextBox txt_CompStr = (TextBox)grdCubeInward.Rows[i].Cells[10].FindControl("txt_CompStr");
                Label lblImage1 = (Label)grdCubeInward.Rows[i].FindControl("lblImage1");
                Label lblImage2 = (Label)grdCubeInward.Rows[i].FindControl("lblImage2");
                Label lblImage3 = (Label)grdCubeInward.Rows[i].FindControl("lblImage3");

                txt_IdMark.Text = cu.CTTEST_IdMark_var.ToString();
                txt_Length.Text = cu.CTTEST_Length_dec.ToString();
                txt_Breadth.Text = cu.CTTEST_Breadth_dec.ToString();
                txt_Height.Text = cu.CTTEST_Height_dec.ToString();
                txt_Weight.Text = cu.CTTEST_Weight_dec.ToString();
                txt_Reading.Text = cu.CTTEST_Reading_var.ToString();
                if (Convert.ToString(cu.CTTEST_Image_var) != null)
                    lblImage1.Text = cu.CTTEST_Image_var.ToString();
                else
                    lblImage1.Text = "";
                if (Convert.ToString(cu.CTTEST_Image_var1) != null)
                    lblImage2.Text = cu.CTTEST_Image_var1.ToString();
                else
                    lblImage2.Text = "";
                if (Convert.ToString(cu.CTTEST_Image_var2) != null)
                    lblImage3.Text = cu.CTTEST_Image_var2.ToString();
                else
                    lblImage3.Text = "";
                //txt_Age.Text = cu.CTTEST_Age_var.ToString();
                //txt_CSArea.Text = cu.CTTEST_CSArea_dec.ToString();
                //txt_DEnsity.Text = cu.CTTEST_Density_dec.ToString();
                //txt_CompStr.Text = cu.CTTEST_CompStr_var.ToString();
                if (cu.CTTEST_Age_var != null)
                {
                    txt_Age.Text = cu.CTTEST_Age_var.ToString();
                }
                if (cu.CTTEST_CSArea_dec != null)
                {
                    txt_CSArea.Text = cu.CTTEST_CSArea_dec.ToString();
                }
                if (cu.CTTEST_Density_dec != null)
                {
                    txt_DEnsity.Text = cu.CTTEST_Density_dec.ToString();
                }
                if (cu.CTTEST_CompStr_var != null)
                {
                    txt_CompStr.Text = cu.CTTEST_CompStr_var.ToString();
                }
                i++;
            }
            var avg = dc.ReportStatus_View("Cube Testing", null, null, 0, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var c in avg)
            {
                if (grdCubeInward.Rows.Count > 0)
                {
                    int NoOfrows = grdCubeInward.Rows.Count / 2;
                    TextBox txt_AvgStr = (TextBox)grdCubeInward.Rows[NoOfrows].Cells[11].FindControl("txt_AvgStr");
                    if (c.CTINWD_AvgStr_var != null)
                    {
                        txt_AvgStr.Text = c.CTINWD_AvgStr_var.ToString();
                    }
                        break;
                }
            }
            if (grdCubeInward.Rows.Count <= 0)
            {
                DisplayGridRow();
            }
        }
        public void DisplayRemark()
        {
            int i = 0;
            var re = dc.AllRemark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, "CT");
            foreach (var r in re)
            {
                AddRowCubeRemark();
                TextBox txt_REMARK = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CTDetail_RemarkId_int), "CT");
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.CT_Remark_var.ToString();
                    i++;
                }
            }
            if (grdRemark.Rows.Count <= 0)
            {
                AddRowCubeRemark();
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
            if (ddl_OtherPendingRpt.SelectedItem.Text != "---Select---")
            {

                FetchRefNo();

            }
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            //int Days = 0;
            //if (lbl_OtherPending.Text == "Days")
            //{
            //    Days = Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue);
            //}
            //PrintPDFReport rpt = new PrintPDFReport();
            //rpt.Cube_PDFReport(txt_ReferenceNo.Text, Days, txt_RecordType.Text, lblEntry.Text, lblCubecompstr.Text, lblComprTest.Text);
            int Days = 0;
            if (lblDays.Text != "")
            {
                Days = Convert.ToInt32(lblDays.Text);
            }
            if (lbl_OtherPending.Text == "Days")
            {
                Days = Convert.ToInt32(ddl_OtherPendingRpt.SelectedValue);
            }
            if (txt_RecordType.Text == "MF")
            {
                lblcubetype.Text = "MF";
            }
            if (ddlTrial.Visible == true)
            {
                lblComprTest.Text = ddlTrial.SelectedValue; 
            }
            PrintPDFReport rpt = new PrintPDFReport();
            rpt.Cube_PDFReport(txt_ReferenceNo.Text, Days, txt_RecordType.Text, lblcubetype.Text, lblEntry.Text, lblCubecompstr.Text, lblComprTest.Text);


        }


        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }
        protected void LnkExit_Click(object sender, EventArgs e)
        {
            string strURLWithData = "";

            //if (txt_RecordType.Text == "FLYASH")
            if (txt_RecordType.Text == "FLYASH" || (txt_RecordType.Text == "CEMT" && lblcubetype.Text == "FLYASH"))
            {
                strURLWithData = "FlyAsh_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&CubeCompStr={4}", "FLYASH", lblRecordNo.Text, txt_ReferenceNo.Text, lblEntry.Text, "CubeCompStrength"));
                Response.Redirect(strURLWithData);
            }
            else if (txt_RecordType.Text == "GGBS" || (txt_RecordType.Text == "CEMT" && lblcubetype.Text == "GGBS"))
            {
                strURLWithData = "Ggbs_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&CubeCompStr={4}", "GGBS", lblRecordNo.Text, txt_ReferenceNo.Text, lblEntry.Text, "CubeCompStrength"));
                Response.Redirect(strURLWithData);
            }
            else if (txt_RecordType.Text == "CEMT")
            {
                strURLWithData = "Cement_Report.aspx?" + obj.Encrypt(string.Format("RecType={0}&RecNo={1}&RefNo={2}&Action={3}&CubeCompStr={4}", "CEMT", lblRecordNo.Text, txt_ReferenceNo.Text, lblEntry.Text, "CementStrength"));
                Response.Redirect(strURLWithData);
            }
            else
            {
                object refUrl = ViewState["RefUrl"];
                if (refUrl != null)
                {
                    Response.Redirect((string)refUrl);//Hav to do sesssion
                }
            }
        }

        protected void grdCubeInward_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewImages")
            {
                GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int rowindex = grdrow.RowIndex;
                Label lblImage1 = (Label)grdCubeInward.Rows[rowindex].FindControl("lblImage1");
                Label lblImage2 = (Label)grdCubeInward.Rows[rowindex].FindControl("lblImage2");
                Label lblImage3 = (Label)grdCubeInward.Rows[rowindex].FindControl("lblImage3");

                if (lblImage1.Text != "" || lblImage2.Text != "" || lblImage3.Text != "")
                {
                    string strImages = getCubeImagesString(txt_ReferenceNo.Text, rowindex);
                    //NewWindows.PrintReport_Html(strImages, Server.MapPath("~"));
                    PrintHTMLReport rptHtml = new PrintHTMLReport();
                    rptHtml.DownloadHtmlReport("CubeImages", strImages);
                }
            }
        }

        protected string getCubeImagesString(string referenceNo, int serialNo)
        {
            LabDataDataContext dc = new LabDataDataContext();

            string reportStr = "", myStr = "";
            myStr += "<html>";
            myStr += "<head>";
            myStr += "<style type='text/css'>";
            myStr += "body {margin-left:2em margin-right:2em}";
            myStr += "</style>";
            myStr += "</head>";

            myStr += "<tr><td width='100%'height=105>";
            //myStr += "<img border=0 src='Images/" + imgName + ".JPG' >";
            myStr += "&nbsp;</td></tr>";

            myStr += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            myStr += "<tr><td width='99%' colspan=3 height=19>&nbsp;</td></tr>";
            myStr += "<tr><td width='99%' colspan=3 align=center valign=top height=19><font size=4><b>Cube Images</b></font></td></tr>";
            myStr += "<tr><td width='99%' colspan=3>&nbsp;</td></tr>";

            myStr += "<tr><td width= 20% align=left valign=top height=19 ><font size=2>" + "Reference No : " + "</font></td>";
            myStr += "<td width= 2% align=left valign=top height=19 ><font size=2>" + ":" + "</font></td>";
            myStr += "<td width= 87% align=left valign=top height=19 ><font size=2>" + referenceNo + "</font></td></tr>";

            myStr += "<tr><td width= 20% align=left valign=top height=19 ><font size=2>" + "Serial No : " + "</font></td>";
            myStr += "<td width= 2% align=left valign=top height=19 ><font size=2>" + ":" + "</font></td>";
            myStr += "<td width= 87% align=left valign=top height=19 ><font size=2>" + (serialNo + 1) + "</font></td></tr>";

            myStr += "<tr><td width= 10% align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td>";
            myStr += "<td width= 2% align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td>";
            myStr += "<td width= 87% align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            myStr += "<tr><td colspan=3 width= 10% align=left valign=top>";
            myStr += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            string imgLink = "http://92.204.136.64:81/cubephoto/";
            string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
            if (cnStr.ToLower().Contains("mumbai") == true)
            {
                imgLink += "mumbai/";
            }
            else if (cnStr.ToLower().Contains("nashik") == true)
            {
                imgLink += "nashik/";
            }
            else if (cnStr.ToLower().Contains("metro") == true)
            {
                imgLink += "metro/";
            }

            int srNo = 0;
            Label lblImage1 = (Label)grdCubeInward.Rows[serialNo].FindControl("lblImage1");
            Label lblImage2 = (Label)grdCubeInward.Rows[serialNo].FindControl("lblImage2");
            Label lblImage3 = (Label)grdCubeInward.Rows[serialNo].FindControl("lblImage3");

            if (lblImage1.Text != "" || lblImage2.Text != "" || lblImage3.Text != "")
            {

                if (lblImage1.Text != "" &&
                    NewWindows.CheckFileExist(imgLink + lblImage1.Text) == true)
                {
                    srNo++;
                    myStr += "<tr>";
                    myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + srNo + "</font></td>";
                    myStr += "<td width= 40% >";
                    //myStr += "<img border=0 Width=500 height=200 src='" + imgLink + lblImage1.Text + "' >";
                    myStr += "<img border=0 src='" + imgLink + lblImage1.Text + "' >";
                    myStr += "</td>";
                    myStr += "</tr>";
                }
                myStr += "<tr><td width= 5% align=left valign=top height=19 colspan=2><font size=2>&nbsp;</font></td></tr>";

                if (lblImage2.Text != "" &&
                    NewWindows.CheckFileExist(imgLink + lblImage2.Text) == true)
                {
                    srNo++;
                    myStr += "<tr>";
                    myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + srNo + "</font></td>";
                    myStr += "<td width= 40% >";
                    myStr += "<img border=0 src='" + imgLink + lblImage2.Text + "' >";
                    myStr += "</td>";
                    myStr += "</tr>";
                }
                myStr += "<tr><td width= 5% align=left valign=top height=19 colspan=2><font size=2>&nbsp;</font></td></tr>";

                if (lblImage3.Text != "" &&
                    NewWindows.CheckFileExist(imgLink + lblImage3.Text) == true)
                {
                    srNo++;
                    myStr += "<tr>";
                    myStr += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + srNo + "</font></td>";
                    myStr += "<td width= 40% >";
                    myStr += "<img border=0 src='" + imgLink + lblImage3.Text + "' >";
                    myStr += "</td>";
                    myStr += "</tr>";
                }
                myStr += "<tr><td width= 5% align=left valign=top height=19 colspan=2><font size=2>&nbsp;</font></td></tr>";

                srNo++;
            }

            myStr += "</table>";
            myStr += "</td></tr>";

            myStr += "</table>";
            myStr += "</html>";
            return reportStr = myStr;

        }

        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {
            clsData objcls = new clsData();
            string mySql = "";
            DataTable dt;

            string strRefNo = txt_ReferenceNo.Text;
            //strRefNo = "91517/2-2"
            #region Flyash
            if (lblcubetype.Text == "FLYASH")
            {
                if (lblDays.Text == "7")
                {
                    mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, day7Comp.* 
                        from new_gt_app_db.dbo.reference_number ref join new_gt_app_db.dbo.category_header chead on ref.pk_id = chead.reference_no 
                        join new_gt_app_db.dbo.flyash_compstr_seven_days day7Comp on chead.pk_id = day7Comp.category_header_fk_id
                        where ref.reference_number = '" + strRefNo + "' and chead.test_fk_id = 31 order by day7Comp.quantity_no";
                }
                else if (lblDays.Text == "28")
                {
                    mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, day28Comp.* 
                        from new_gt_app_db.dbo.reference_number ref join new_gt_app_db.dbo.category_header chead on ref.pk_id = chead.reference_no 
                        join new_gt_app_db.dbo.flyash_compstr_twentyeight_days day28Comp on chead.pk_id = day28Comp.category_header_fk_id
                        where ref.reference_number = '" + strRefNo + "' and chead.test_fk_id = 32 order by day28Comp.quantity_no";

                }
                else if (lblDays.Text == "46")
                {
                    mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, day46Comp.* 
                        from new_gt_app_db.dbo.reference_number ref join new_gt_app_db.dbo.category_header chead on ref.pk_id = chead.reference_no 
                        join new_gt_app_db.dbo.flaysh_compstr_fortysix_days day46Comp on chead.pk_id = day46Comp.category_header_fk_id 
                        where ref.reference_number = '" + strRefNo + "' and chead.test_fk_id = 33 order by day46Comp.quantity_no";
                }
                else if (lblDays.Text == "90")
                {
                    mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, day90Comp.* 
                        from new_gt_app_db.dbo.reference_number ref join new_gt_app_db.dbo.category_header chead on ref.pk_id = chead.reference_no 
                        join new_gt_app_db.dbo.flayash_compstr_ninety_days day90Comp on chead.pk_id = day90Comp.category_header_fk_id                     
                        where ref.reference_number = '" + strRefNo + "' and chead.test_fk_id = 34 order by day90Comp.quantity_no";
                }
            }
            else if (lblcubetype.Text == "CEMT")
            {
                if (lblDays.Text == "3")
                {
                    mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, day3Comp.* 
                        from new_gt_app_db.dbo.reference_number ref join new_gt_app_db.dbo.category_header chead on ref.pk_id = chead.reference_no 
                        join new_gt_app_db.dbo.cement_compstr day3Comp on chead.pk_id = day3Comp.category_header_fk_id
                        where ref.reference_number = '" + strRefNo + "' and chead.test_fk_id = 22 order by day3Comp.quantity_no";
                }
                else if (lblDays.Text == "7")
                {
                    mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, day7Comp.* 
                        from new_gt_app_db.dbo.reference_number ref join new_gt_app_db.dbo.category_header chead on ref.pk_id = chead.reference_no 
                        join new_gt_app_db.dbo.cement_compstr_seven_day day7Comp on chead.pk_id = day7Comp.category_header_fk_id
                        where ref.reference_number = '" + strRefNo + "' and chead.test_fk_id = 23 order by day7Comp.quantity_no";
                }
                else if (lblDays.Text == "28")
                {
                    mySql = @"select ref.reference_number, chead.date_of_testing, chead.witness_by, chead.description, day28Comp.* 
                        from new_gt_app_db.dbo.reference_number ref join new_gt_app_db.dbo.category_header chead on ref.pk_id = chead.reference_no 
                        join new_gt_app_db.dbo.cement_compstr_twentyeight_days day28Comp on chead.pk_id = day28Comp.category_header_fk_id
                        where ref.reference_number = '" + strRefNo + "' and chead.test_fk_id = 24 order by day28Comp.quantity_no";

                }
            }
            dt = objcls.getGeneralData(mySql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_IdMark = (TextBox)grdCubeInward.Rows[i].Cells[1].FindControl("txt_IdMark");
                TextBox txt_Length = (TextBox)grdCubeInward.Rows[i].Cells[2].FindControl("txt_Length");
                TextBox txt_Breadth = (TextBox)grdCubeInward.Rows[i].Cells[3].FindControl("txt_Breadth");
                TextBox txt_Height = (TextBox)grdCubeInward.Rows[i].Cells[4].FindControl("txt_Height");
                TextBox txt_Weight = (TextBox)grdCubeInward.Rows[i].Cells[5].FindControl("txt_Weight");
                TextBox txt_Reading = (TextBox)grdCubeInward.Rows[i].Cells[6].FindControl("txt_Reading");
                Label lblImage1 = (Label)grdCubeInward.Rows[i].FindControl("lblImage1");
                Label lblImage2 = (Label)grdCubeInward.Rows[i].FindControl("lblImage2");
                Label lblImage3 = (Label)grdCubeInward.Rows[i].FindControl("lblImage3");

                if (i == 0)
                {
                    if (dt.Rows[i]["date_of_testing"].ToString() != "")
                        txt_DtOfTesting.Text = dt.Rows[i]["date_of_testing"].ToString();
                    txt_Description.Text = dt.Rows[i]["description"].ToString();

                    if (dt.Rows[0]["witness_by"] != null || dt.Rows[i]["witness_by"].ToString() != "")
                    {
                        txt_witnessBy.Text = dt.Rows[i]["witness_by"].ToString();
                        chk_WitnessBy.Checked = true;
                        txt_witnessBy.Visible = true;
                    }
                }
                if (lblcubetype.Text == "FLYASH" && txt_RecordType.Text == "CEMT" && lblDays.Text == "28")
                {
                    txt_IdMark.Text = dt.Rows[i]["cement_id_mark"].ToString();
                    txt_Length.Text = dt.Rows[i]["cement_length"].ToString();
                    txt_Breadth.Text = dt.Rows[i]["cement_breadth"].ToString();
                    txt_Height.Text = dt.Rows[i]["cement_height"].ToString();
                    txt_Weight.Text = dt.Rows[i]["cement_weight"].ToString();
                    txt_Reading.Text = dt.Rows[i]["cement_reading"].ToString();
                    lblImage1.Text = dt.Rows[i]["cement_img"].ToString();
                }
                else
                {
                    txt_IdMark.Text = dt.Rows[i]["id_mark"].ToString();
                    txt_Length.Text = dt.Rows[i]["length"].ToString();
                    txt_Breadth.Text = dt.Rows[i]["breadth"].ToString();
                    txt_Height.Text = dt.Rows[i]["height"].ToString();
                    txt_Weight.Text = dt.Rows[i]["weight"].ToString();
                    txt_Reading.Text = dt.Rows[i]["reading"].ToString();

                    if (lblcubetype.Text == "CEMT")
                    {
                        if (lblDays.Text == "3")
                        {
                            lblImage1.Text = dt.Rows[i]["img_three_1"].ToString();
                            lblImage2.Text = dt.Rows[i]["img_three_2"].ToString();
                            lblImage3.Text = dt.Rows[i]["img_three_3"].ToString();
                        }
                        else if (lblDays.Text == "7")
                        {
                            lblImage1.Text = dt.Rows[i]["img_seven_1"].ToString();
                            lblImage2.Text = dt.Rows[i]["img_seven_2"].ToString();
                            lblImage3.Text = dt.Rows[i]["img_seven_3"].ToString();
                        }
                        else if (lblDays.Text == "28")
                        {
                            lblImage1.Text = dt.Rows[i]["img_twentyeight_1"].ToString();                            
                            lblImage2.Text = dt.Rows[i]["img_twentyeight_2"].ToString();
                            lblImage3.Text = dt.Rows[i]["img_twentyeight_3"].ToString();                            
                        }
                    }
                    else if (lblcubetype.Text == "FLYASH")
                    { 
                        if (lblDays.Text == "7")
                        {
                            lblImage1.Text = dt.Rows[i]["img_seven_1"].ToString();
                            lblImage2.Text = dt.Rows[i]["img_seven_2"].ToString();
                            lblImage3.Text = dt.Rows[i]["img_seven_3"].ToString();
                        }
                        else if (lblDays.Text == "28")
                        {
                            lblImage1.Text = dt.Rows[i]["img_twentyeight_1"].ToString();
                        }
                        else if (lblDays.Text == "46")
                        {
                            lblImage1.Text = dt.Rows[i]["img_fortysix_days"].ToString();
                        }
                        else if (lblDays.Text == "90")
                        {
                            lblImage1.Text = dt.Rows[i]["img_ninety_days"].ToString();
                        }
                    }
                }
            }
            #endregion
            dt.Dispose();
            objcls = null;
        }
    }
}
