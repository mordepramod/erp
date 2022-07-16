using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;


namespace DESPLWEB
{
    public partial class TrialOtherInfo : System.Web.UI.Page
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
                    //if (strReq.Contains("=") == false)
                    //{
                    //    Session.Abandon();
                    //    Response.Redirect("Login.aspx");
                    //}
                    if (strReq.Contains("=") == true)
                    {
                        string[] arrMsgs = strReq.Split('&');
                        string[] arrIndMsg;

                        arrIndMsg = arrMsgs[0].Split('=');
                        txt_RefNo.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[1].Split('=');
                        lbl_TrialId.Text = arrIndMsg[1].ToString().Trim();
                    }
                }

                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Trial Other Information";

                txt_Castingdt.Text = DateTime.Today.ToString("dd/MM/yyyy");
                LoadReferenceNoList();
                ShowTrailOtherInfo();
            }
        }
        protected void ddl_OtherPendingRpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlTrial.Items.Clear();             
            LoadTrialList();
        }
        private void LoadReferenceNoList()
        {
            var reportList = dc.Trial_View("", false);
            ddl_OtherPendingRpt.DataTextField = "Trial_RefNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
        }

        private void LoadTrialList()
        {
            var trial = dc.Trial_View(ddl_OtherPendingRpt.SelectedValue, false);
            ddlTrial.DataTextField = "Trial_Name";
            ddlTrial.DataValueField = "Trial_Id";
            ddlTrial.DataSource = trial;
            ddlTrial.DataBind();
            ddlTrial.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        protected void lnkFetch_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (ddl_OtherPendingRpt.SelectedIndex == 0)
            {
                lblMsg.Text = "Select report number.";
                lblMsg.Visible = true;
            }
            else if (ddlTrial.SelectedIndex == 0)
            {        
                lblMsg.Text = "Select trial.";
                lblMsg.Visible = true;
            }
            else 
            {
                ClearAllControls();
                txt_RefNo.Text = ddl_OtherPendingRpt.SelectedValue;
                lbl_TrialId.Text = ddlTrial.SelectedValue;
                ShowTrailOtherInfo();                
                CheckMDLStatus();
            }
        }

        protected void CheckMDLStatus()
        {
            lnkSave.Enabled = true;
            
            var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
            foreach (var u in user)
            {
                if (u.USER_SuperAdmin_right_bit == false)
                {
                    var mfinwd = dc.MF_View(txt_RefNo.Text, 0, txt_RecType.Text);
                    foreach (var mf in mfinwd)
                    {
                        if (mf.MFINWD_Status_tint >= 3)
                        {
                            lnkSave.Enabled = false;
                        }
                    }
                }
            }
            
        }

        protected void ClearAllControls()
        {   
            chk_MDLetter.Checked = false;
            ddl_Batching.SelectedValue = "---Select---";
            txt_Slump1.Text = "";
            txt_Slump2.Text = "";
            txt_Slump3.Text = "";
            ddl_Remark.SelectedValue = "---Select---";
            chk_Retention.Checked = false;
            txt_AfterMin.Text = "";
            txt_RetSlump1.Text = "";
            txt_RetSlump2.Text = "";
            txt_RetSlump3.Text = "";
            txtYield.Text = "";
            txtFinalWCRatio.Text = "";
            txtCompactionFactor.Text = "";
            txt_TrialNoCubes.Text = "";
            txt_Castingdt.Text = DateTime.Today.ToString("dd/MM/yyyy");
            chk_CubeCasting.Checked =false ;
            grdCubeCasting.DataSource = null;
            grdCubeCasting.DataBind();
        }
        
        protected void ShowTrailOtherInfo()
        {
            int i = 0;           
            txt_RecType.Text = "MF";
            double FinalWCRatio=0;
            if (lbl_TrialId.Text != "")
            {
                var data = dc.TrialDetail_View(txt_RefNo.Text, Convert.ToInt32(lbl_TrialId.Text));
                foreach (var t in data)
                {
                    if (i == 0)
                    {
                        txt_Castingdt.Text = Convert.ToDateTime(t.Trial_Date).ToString("dd/MM/yyyy");
                        lblTrialName.Text = Convert.ToString(t.Trial_Name);
                        txt_TrialNoCubes.Text = t.Trial_NoOfCubes.ToString();
                        if (t.Trial_MDletter_Status == true && t.Trial_Status == 1)
                        {
                            chk_MDLetter.Checked = true;
                            var mdl = dc.MDLetter_View(txt_RefNo.Text, "MDL");
                            if (mdl.Count() > 0)
                            {
                                lblMDLetterMsg.Visible = true;
                                lblMDLetterMsg.Text = "On update MD Letter information will get reset.";
                            }
                        }
                        if (t.Trial_Batching != "" && t.Trial_Batching != null)
                        {
                            //ddl_Batching.Items.FindByText(t.Trial_Batching).Selected = true;
                            ddl_Batching.SelectedValue = t.Trial_Batching;
                        }
                        if (t.Trial_Remark != "" && t.Trial_Remark != null)
                        {
                            //ddl_Remark.Items.FindByText(t.Trial_Remark).Selected = true;
                            ddl_Remark.SelectedValue = t.Trial_Remark;
                        }

                        if (t.Trial_BatchSlumpValue != null && t.Trial_BatchSlumpValue != "")
                        {
                            string[] BatchSlump = t.Trial_BatchSlumpValue.Split('|');
                            foreach (string slump in BatchSlump)
                            {
                                if (slump != "")
                                {
                                    if (txt_Slump1.Text == "")
                                    {
                                        txt_Slump1.Text = slump.ToString();
                                    }
                                    else if (txt_Slump2.Text == "")
                                    {
                                        txt_Slump2.Text = slump.ToString();
                                    }
                                    else if (txt_Slump3.Text == "")
                                    {
                                        txt_Slump3.Text = slump.ToString();
                                    }
                                }
                            }
                        }
                        if (t.Trial_RetentionStatus == true)
                        {
                            chk_Retention.Checked = true;
                            ShowRetention();
                            txt_AfterMin.Text = t.Trial_RetTimeDuration.ToString();
                            string[] RetnSlump = t.Trial_RetentionSlumpValue.Split('|');
                            foreach (string slump in RetnSlump)
                            {
                                if (slump != "")
                                {
                                    if (txt_RetSlump1.Text == "")
                                    {
                                        txt_RetSlump1.Text = slump.ToString();
                                    }
                                    else if (txt_RetSlump2.Text == "")
                                    {
                                        txt_RetSlump2.Text = slump.ToString();
                                    }
                                    else if (txt_RetSlump3.Text == "")
                                    {
                                        txt_RetSlump3.Text = slump.ToString();
                                    }
                                }
                            }
                        }
                        if (Convert.ToString(t.Trial_Compactability) == "Good")
                        {
                            Rdn_Good.Checked = true;
                        }
                        if (Convert.ToString(t.Trial_Compactability) == "Average")
                        {
                            Rdn_Average.Checked = true;
                        }
                        if (Convert.ToString(t.Trial_Compactability) == "Poor")
                        {
                            Rdn_Poor.Checked = true;
                        }
                        //if (t.Trial_CubeCastStatus == true)
                        //{
                            //chk_CubeCasting.Checked = true;
                            PnlCubeCast.Visible = true;
                            grdCubeCasting.Visible = true;
                            ShowCubeCast();
                        //}
                    }
                    if (t.TrialDetail_MaterialName == "Cement" || t.TrialDetail_MaterialName == "Fly Ash" ||
                        t.TrialDetail_MaterialName == "G G B S" || t.TrialDetail_MaterialName == "Micro Silica")
                        FinalWCRatio = FinalWCRatio + Convert.ToDouble(t.TrialDetail_Weight);
                    
                    if (txtYield.Text == "" && t.TrialDetail_MaterialName == "Plastic Density")
                    {
                        txtYield.Text = t.TrialDetail_Weight;
                    }
                    if (txtFinalWCRatio.Text == "" && t.TrialDetail_MaterialName == "W/C Ratio")
                    {
                        txtFinalWCRatio.Text = t.TrialDetail_Weight;
                    }
                    if (txtCompactionFactor.Text == "" )
                    {
                        txtCompactionFactor.Text = "0";
                    }
                    i++;
                }
            }
        } 
        protected void ShowCubeCast()
        {
            int i = 0;
            if (lbl_TrialId.Text != "")
            {
                var cubeCast = dc.OtherCubeTestView(txt_RefNo.Text, txt_RecType.Text, 0, Convert.ToInt32(lbl_TrialId.Text), "Trial", false, false);
                foreach (var c in cubeCast)
                {
                    AddRowCubeCasting();
                    TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_Days");
                    TextBox txt_Cubes = (TextBox)grdCubeCasting.Rows[i].Cells[4].FindControl("txt_Cubes");

                    txt_Days.Text = Convert.ToString(c.Days_tint);
                    txt_Cubes.Text = Convert.ToString(c.NoOfCubes_tint);
                    i++;
                }
                if (grdCubeCasting.Rows.Count == 0)
                    AddRowCubeCasting();
            }

        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                bool MDLetter = false;
                string BatchSlump = string.Empty;
                string Compactibility = string.Empty;
                string RetentionSlump = string.Empty;
                int TrialStatus = 0;
                DateTime Castingdate = DateTime.Now;
                DateTime TestingDt = DateTime.Now;

                bool Rentention = false; bool CubeCasting = false;
                if (chk_CubeCasting.Checked)
                {
                    CubeCasting = true;
                }
                if (Rdn_Good.Checked)
                {
                    Compactibility = lbl_Good.Text;
                }
                if (Rdn_Average.Checked)
                {
                    Compactibility = lbl_Avg.Text;
                }
                if (Rdn_Poor.Checked)
                {
                    Compactibility = lbl_Poor.Text;
                }
                if (chk_Retention.Checked)
                {
                    Rentention = true; 
                    RetentionSlump = RetentionSlump + txt_RetSlump1.Text + "|" + txt_RetSlump2.Text + "|" + txt_RetSlump3.Text;
                }
                BatchSlump = BatchSlump + txt_Slump1.Text + "|" + txt_Slump2.Text + "|" + txt_Slump3.Text;
                if (chk_MDLetter.Checked)
                {
                    MDLetter = true;

                    TrialStatus = 1;
                    dc.MDLetter_Update(txt_RefNo.Text, 0, "MDL", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true);
                    dc.MDLetter_Update(txt_RefNo.Text, 0, "Final", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, true);
                    dc.AllInwd_Update(txt_RefNo.Text, "", 0, null, "", "", 1, 0, "", 0, "", "", 0, 0, 0, "MF");
                }

                dc.Trail_Update(Convert.ToInt32(lbl_TrialId.Text), txt_RefNo.Text, "", "", "", "", "","","","", "", "", "","","","", null, null, "", "", "", txt_AfterMin.Text, "", "", "",
                      0, MDLetter, ddl_Batching.SelectedItem.Text, BatchSlump, Compactibility, ddl_Remark.SelectedItem.Text, Rentention, RetentionSlump, CubeCasting, "", "","","","","",0,"",0, false, TrialStatus, true, false);
                if (chk_CubeCasting.Checked)
                {
                    var otherInfo= dc.OtherCubeTestView(txt_RefNo.Text, txt_RecType.Text, 0, Convert.ToInt32(lbl_TrialId.Text),"Trial", false, false);
                    foreach (var o in otherInfo)
                    {
                        dc.OtherCubeTestDetail_Update(txt_RefNo.Text, "", 0, 0, 0, 0, "", "", 0, 0, "", o.Days_tint, "", "", null, "Trial", "MF","","","", false, false, true, Convert.ToInt32(lbl_TrialId.Text));
                    }
                    dc.OtherCubeTest_Update(txt_RefNo.Text, txt_RecType.Text, 0, 0, 3, "", Convert.ToInt32(lbl_TrialId.Text),"", false, true);
                    
                    for (int i = 0; i < grdCubeCasting.Rows.Count; i++)
                    {
                        TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_Days");
                        TextBox txt_Cubes = (TextBox)grdCubeCasting.Rows[i].Cells[4].FindControl("txt_Cubes");

                        dc.OtherCubeTest_Update(txt_RefNo.Text, txt_RecType.Text, Convert.ToByte(txt_Days.Text), Convert.ToByte(txt_Cubes.Text), 0, "Trial", Convert.ToInt32(lbl_TrialId.Text),"", false, false);                        

                        Castingdate = DateTime.ParseExact(txt_Castingdt.Text, "dd/MM/yyyy", null);
                        TestingDt = Castingdate.AddDays(Convert.ToInt32(txt_Days.Text));
                        dc.CubeCastingStatus_Update(txt_RefNo.Text, Convert.ToByte(txt_Days.Text), txt_Castingdt.Text, TestingDt, 0, txt_RecType.Text, Convert.ToInt32(lbl_TrialId.Text));//UPDATE Other Cube Test

                    }
                }
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkSave.Enabled = false;
            }
        }
        protected void chk_Retention_CheckedChanged(object sender, EventArgs e)
        {
            ShowRetention();
        }
        protected void ShowRetention()
        {
            if (chk_Retention.Checked)
            {
                //lbl_EnterTime.Visible = true;
                txt_AfterMin.Enabled = true;
                txt_RetSlump1.Enabled = true;
                txt_RetSlump2.Enabled = true;
                txt_RetSlump3.Enabled = true;
                lbl_Min.Enabled = true;
            }
            else
            {
                //lbl_EnterTime.Visible = false;
                txt_AfterMin.Enabled = false;
                txt_RetSlump1.Enabled = false;
                txt_RetSlump2.Enabled = false;
                txt_RetSlump3.Enabled = false;
                txt_RetSlump1.Text = "";
                txt_RetSlump2.Text = "";
                txt_RetSlump3.Text = "";
                lbl_Min.Enabled = false;
            }
        }
       

        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
        
            Boolean valid = true;
            int SumofCubes = 0;

            if (txt_RefNo.Text == "")
            {
                lblMsg.Text = "Select report number ";
                txt_RefNo.Focus();
                valid = false;
            }
            else if (ddl_Batching.SelectedValue == "---Select---")
            {
                lblMsg.Text = "Select Batching ";
                ddl_Batching.Focus();
                valid = false;
            }
            else if (txt_Slump1.Text == "")
            {
                lblMsg.Text = "Enter Slump 1 ";
                txt_Slump1.Focus();
                valid = false;
            }
            else if (txt_Slump2.Text == "")
            {
                lblMsg.Text = "Enter Slump 2 ";
                txt_Slump2.Focus();
                valid = false;
            }
            else if (txt_Slump3.Text == "")
            {
                lblMsg.Text = "Enter Slump 3 ";
                txt_Slump3.Focus();
                valid = false;
            }
            else if (!Rdn_Average.Checked && !Rdn_Good.Checked && !Rdn_Poor.Checked)
            {
                lblMsg.Text = "Check anyone of Compactability";
                valid = false;
            }
            else if (ddl_Remark.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Select Remark ";
                ddl_Remark.Focus();
                valid = false;
            }
            else if (chk_Retention.Checked && txt_AfterMin.Text == "")
            {
                lblMsg.Text = "Enter Time Duration After Min";
                txt_AfterMin.Focus();
                valid = false;
            }
            else if (chk_Retention.Checked && txt_RetSlump1.Text == "")
            {
                lblMsg.Text = "Enter Slump 1 of Retention Slump Required";
                txt_RetSlump1.Focus();
                valid = false;
            }
            else if (chk_Retention.Checked && txt_RetSlump2.Text == "")
            {
                lblMsg.Text = "Enter Slump 2 of Retention Slump Required";
                txt_RetSlump2.Focus();
                valid = false;
            }
            else if (chk_Retention.Checked && txt_RetSlump3.Text == "")
            {
                lblMsg.Text = "Enter Slump 3 of Retention Slump Required";
                txt_RetSlump3.Focus();
                valid = false;
            }
            else if (valid == true && chk_CubeCasting.Checked == true)
            {
                for (int i = 0; i < grdCubeCasting.Rows.Count; i++)
                {
                    TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_Days");
                    TextBox txt_Cubes = (TextBox)grdCubeCasting.Rows[i].Cells[4].FindControl("txt_Cubes");

                    if (txt_Days.Text == "")
                    {
                        lblMsg.Text = "Enter Days for Sr No. " + (i + 1) + ".";
                        txt_Days.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_Cubes.Text == "")
                    {
                        lblMsg.Text = "Enter Cubes for Sr No. " + (i + 1) + ".";
                        txt_Cubes.Focus();
                        valid = false;
                        break;
                    }
                    if (txt_Cubes.Text != "")
                    {
                        SumofCubes += Convert.ToInt32(txt_Cubes.Text);
                    }
                }

                if (valid == true)
                {
                    for (int i = 0; i < grdCubeCasting.Rows.Count; i++)
                    {
                        TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_Days");
                        for (int j = 1; j < grdCubeCasting.Rows.Count; j++)
                        {
                            TextBox subtxt_Days = (TextBox)grdCubeCasting.Rows[j].Cells[3].FindControl("txt_Days");
                            if (j != i)
                            {
                                if (txt_Days.Text == subtxt_Days.Text)
                                {
                                    lblMsg.Text = "Duplicate No. of Days .";
                                    subtxt_Days.Focus();
                                    valid = false;
                                    break;
                                }
                            }
                        }
                        if (valid == false)
                        {
                            break;
                        }
                    }
                }
                if (valid == true)
                {
                    if (SumofCubes != Convert.ToInt32(txt_TrialNoCubes.Text))
                    {
                        lblMsg.Text = " Total No. of cubes does not match to the Trial No of cubes  ";
                        valid = false;
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

        protected void chk_CubeCasting_CheckedChanged(object sender, EventArgs e)
        {
            //if (chk_CubeCasting.Checked)
            //{
            //    grdCubeCasting.Visible = true;
            //    grdCubeCasting.DataSource = null;
            //    grdCubeCasting.DataBind();
            //    AddRowCubeCasting();
            //}
            //else
            //{
            //    grdCubeCasting.Visible = false;
            //}
           
        }
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowCubeCasting();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdCubeCasting.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdCubeCasting.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowCubeCasting(gvr.RowIndex);
            }
        }

        protected void DeleteRowCubeCasting(int rowIndex)
        {
            GetCurrentDataCubeCasting();
            DataTable dt = ViewState["CubeCastingTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CubeCastingTable"] = dt;
            grdCubeCasting.DataSource = dt;
            grdCubeCasting.DataBind();
            SetPreviousDataCubeCasting();
        }
        protected void AddRowCubeCasting()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CubeCastingTable"] != null)
            {
                GetCurrentDataCubeCasting();
                dt = (DataTable)ViewState["CubeCastingTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Days", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Cubes", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_Days"] = string.Empty;
            dr["txt_Cubes"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["CubeCastingTable"] = dt;
            grdCubeCasting.DataSource = dt;
            grdCubeCasting.DataBind();
            SetPreviousDataCubeCasting();
        }
        protected void GetCurrentDataCubeCasting()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Days", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Cubes", typeof(string)));

            for (int i = 0; i < grdCubeCasting.Rows.Count; i++)
            {
                TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_Days");
                TextBox txt_Cubes = (TextBox)grdCubeCasting.Rows[i].Cells[4].FindControl("txt_Cubes");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_Days"] = txt_Days.Text;
                drRow["txt_Cubes"] = txt_Cubes.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CubeCastingTable"] = dtTable;

        }
        protected void SetPreviousDataCubeCasting()
        {
            DataTable dt = (DataTable)ViewState["CubeCastingTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Days = (TextBox)grdCubeCasting.Rows[i].Cells[3].FindControl("txt_Days");
                TextBox txt_Cubes = (TextBox)grdCubeCasting.Rows[i].Cells[4].FindControl("txt_Cubes");

                grdCubeCasting.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_Days.Text = dt.Rows[i]["txt_Days"].ToString();
                txt_Cubes.Text = dt.Rows[i]["txt_Cubes"].ToString();
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
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }


        

        
    }
}