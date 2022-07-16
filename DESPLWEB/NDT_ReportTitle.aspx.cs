using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;

namespace DESPLWEB
{
    public partial class NDT_ReportTitle : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "NDT - Report Title";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                LoadReferenceNoList();
            }
        }
        private void LoadReferenceNoList()
        {
            byte reportStatus = 11;
            var reportList = dc.ReferenceNo_View_StatusWise("NDT", reportStatus, 0);
            ddlReferenceNo.DataTextField = "ReferenceNo";
            ddlReferenceNo.DataSource = reportList;
            ddlReferenceNo.DataBind();
            ddlReferenceNo.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        protected void ddlReferenceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAllControls();
            if (ddlReferenceNo.SelectedIndex > 0)
            {
                DisplayReportDetails();
            }
        }
        protected void ClearAllControls()
        {
            txtClientName.Text = "";
            txtSiteName.Text = "";
            grdWBS.DataSource = null;
            grdWBS.DataBind();
            grdNDTDetails.DataSource = null;
            grdNDTDetails.DataBind();
            grdWBSMerge.DataSource = null;
            grdWBSMerge.DataBind();
            grdNDTDetails2.DataSource = null;
            grdNDTDetails2.DataBind();
        }
        protected void DisplayReportDetails()
        {
            var inward = dc.Inward_View(Convert.ToInt32(ddlReferenceNo.SelectedItem.Value.Split('/')[0].ToString()), 0, "NDT", null, null);
            foreach (var inwd in inward)
            {
                txtClientName.Text = inwd.ClientName;
                txtSiteName.Text = inwd.SiteName;
            }
            grdWBS.DataSource = null;
            grdWBS.DataBind();
            grdWBSMerge.DataSource = null;
            grdWBSMerge.DataBind();
            lblSelectedWbsMerge.Text = "";
            lblSelectedWbs.Text = ""; 
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("lblWBSId", typeof(string)));
            dt1.Columns.Add(new DataColumn("Building", typeof(string)));
            dt1.Columns.Add(new DataColumn("Floor", typeof(string)));
            dt1.Columns.Add(new DataColumn("MemberType", typeof(string)));
            dt1.Columns.Add(new DataColumn("MemberId", typeof(string)));
            int i = 0;
            var wbs = dc.NDTWBS_View_All(ddlReferenceNo.SelectedValue, 0, "", "", "", "").ToList();
            foreach (var w in wbs)
            {
                dr1 = dt1.NewRow();
                dr1["lblSrNo"] = (i + 1).ToString();
                dr1["lblWBSId"] = w.NDTWBS_Id;
                dr1["Building"] = w.NDTWBS_Building_var;
                dr1["Floor"] = w.NDTWBS_Floor_var;
                dr1["MemberType"] = w.NDTWBS_MemberType_var;
                dr1["MemberId"] = w.NDTWBS_MemberId_var;
                dt1.Rows.Add(dr1);
                
                AddRowWBS();
                Label lblWBSId = (Label)grdWBS.Rows[i].FindControl("lblWBSId");
                TextBox txtBuilding = (TextBox)grdWBS.Rows[i].FindControl("txtBuilding");
                TextBox txtFloor = (TextBox)grdWBS.Rows[i].FindControl("txtFloor");
                TextBox txtMemberType = (TextBox)grdWBS.Rows[i].FindControl("txtMemberType");
                TextBox txtMemberId = (TextBox)grdWBS.Rows[i].FindControl("txtMemberId");

                grdWBS.Rows[i].Cells[1].Text = (i + 1).ToString();
                lblWBSId.Text = w.NDTWBS_Id.ToString();
                txtBuilding.Text = w.NDTWBS_Building_var;
                txtFloor.Text = w.NDTWBS_Floor_var;
                txtMemberType.Text = w.NDTWBS_MemberType_var;
                txtMemberId.Text = w.NDTWBS_MemberId_var;

                i++;
            }            
            grdWBSMerge.DataSource = dt1;
            grdWBSMerge.DataBind();
            //if (grdWBS.Rows.Count > 0)
            //    lnkGetData.Enabled = false;
            //else
            //    lnkGetData.Enabled = true; 
        }
        protected void chkSelectWBS_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grdWBSMerge.Rows.Count; i++)
            {
                CheckBox chkSelectWBSMerge1 = (CheckBox)grdWBSMerge.Rows[i].FindControl("chkSelectWBSMerge");
                chkSelectWBSMerge1.Checked = false;
            }
            grdNDTDetails.DataSource = null;
            grdNDTDetails.DataBind();
            grdNDTDetails2.DataSource = null;
            grdNDTDetails2.DataBind();
            lblSelectedWbs.Text = "";
            lblSelectedWbsMerge.Text = "";
            CheckBox btn = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            int j = gvr.RowIndex;

            int cntSel = 0;
            for (int i = 0; i < grdWBS.Rows.Count; i++)
            {
                CheckBox chkSelectWBS1 = (CheckBox)grdWBS.Rows[i].FindControl("chkSelectWBS");
                if (chkSelectWBS1.Checked == true)
                {
                    cntSel++;
                    j = i;
                }
            }
            CheckBox chkSelectWBS = (CheckBox)grdWBS.Rows[j].FindControl("chkSelectWBS");
            if (chkSelectWBS.Checked == true && cntSel == 1)
            {
                Label lblWBSId = (Label)grdWBS.Rows[j].FindControl("lblWBSId");
                TextBox txtBuilding = (TextBox)grdWBS.Rows[j].FindControl("txtBuilding");
                TextBox txtFloor = (TextBox)grdWBS.Rows[j].FindControl("txtFloor");
                TextBox txtMemberType = (TextBox)grdWBS.Rows[j].FindControl("txtMemberType");
                TextBox txtMemberId = (TextBox)grdWBS.Rows[j].FindControl("txtMemberId");
                lblSelectedWbs.Text = "Building - " + txtBuilding.Text + ", Floor - " + txtFloor.Text + ", Member Type - " + txtMemberType.Text + ", Member Id - " + txtMemberId.Text;

                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblDescription", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblGrade", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblCastingDate", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblAlphaAngle", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblReboundIndex", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblPulseVel", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblAge", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblIndStr", typeof(string)));

                int i = 0;
                var wbs = dc.NDTDetail_View(ddlReferenceNo.SelectedValue, Convert.ToInt32(lblWBSId.Text)).ToList();
                foreach (var w in wbs)
                {
                    dr1 = dt1.NewRow();
                    dr1["lblSrNo"] = (i + 1).ToString();
                    dr1["lblDescription"] = w.Description_var;
                    dr1["lblGrade"] = w.Grade_var;
                    dr1["lblCastingDate"] = w.Castingdate_var;
                    dr1["lblAlphaAngle"] = w.AlphaAngle_var;
                    dr1["lblReboundIndex"] = w.ReboundIndex_var;
                    dr1["lblPulseVel"] = w.PulseVelocity_var;
                    dr1["lblAge"] = w.Age_var;
                    dr1["lblIndStr"] = w.IndicativeStrength_var;
                    dt1.Rows.Add(dr1);
                    
                    i++;
                }
                grdNDTDetails.DataSource = dt1;
                grdNDTDetails.DataBind();
            }

        }

        protected void chkSelectWBSMerge_CheckedChanged(object sender, EventArgs e)
        {
            grdNDTDetails2.DataSource = null;
            grdNDTDetails2.DataBind();
            lblSelectedWbsMerge.Text = "";
            CheckBox btn = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            int j = gvr.RowIndex;
            int cntSel = 0;
            CheckBox chkSelectWBSMerge = (CheckBox)grdWBSMerge.Rows[j].FindControl("chkSelectWBSMerge");
            CheckBox chkSelectWBS = (CheckBox)grdWBS.Rows[j].FindControl("chkSelectWBS");
            if (chkSelectWBSMerge.Checked == true && chkSelectWBS.Checked == true)
            {
                chkSelectWBSMerge.Checked = false;
                //ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "key", "alert('Can not select same title to merge.')", true);
            }
            for (int i = 0; i < grdWBSMerge.Rows.Count; i++)
            {
                CheckBox chkSelectWBSMerge1 = (CheckBox)grdWBSMerge.Rows[i].FindControl("chkSelectWBSMerge");
                if (chkSelectWBSMerge1.Checked == true)
                {
                    cntSel++;
                    j = i;
                }
            }
            if (cntSel == 1)
            {
                Label lblWBSIdMerge = (Label)grdWBSMerge.Rows[j].FindControl("lblWBSId");
                lblSelectedWbsMerge.Text = "Building - " + grdWBSMerge.Rows[j].Cells[2].Text + ", Floor - " + grdWBSMerge.Rows[j].Cells[3].Text + ", Member Type - " + grdWBSMerge.Rows[j].Cells[4].Text + ", Member Id - " + grdWBSMerge.Rows[j].Cells[5].Text;

                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblDescription", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblGrade", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblCastingDate", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblAlphaAngle", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblReboundIndex", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblPulseVel", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblAge", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblIndStr", typeof(string)));

                int i = 0;
                var wbs = dc.NDTDetail_View(ddlReferenceNo.SelectedValue, Convert.ToInt32(lblWBSIdMerge.Text)).ToList();
                foreach (var w in wbs)
                {
                    dr1 = dt1.NewRow();
                    dr1["lblSrNo"] = (i + 1).ToString();
                    dr1["lblDescription"] = w.Description_var;
                    dr1["lblGrade"] = w.Grade_var;
                    dr1["lblCastingDate"] = w.Castingdate_var;
                    dr1["lblAlphaAngle"] = w.AlphaAngle_var;
                    dr1["lblReboundIndex"] = w.ReboundIndex_var;
                    dr1["lblPulseVel"] = w.PulseVelocity_var;
                    dr1["lblAge"] = w.Age_var;
                    dr1["lblIndStr"] = w.IndicativeStrength_var;
                    dt1.Rows.Add(dr1);

                    i++;
                }
                grdNDTDetails2.DataSource = dt1;
                grdNDTDetails2.DataBind();
            }
            
        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            int j = gvr.RowIndex;

            Label lblWBSId = (Label)grdWBS.Rows[j].FindControl("lblWBSId");
            TextBox txtBuilding = (TextBox)grdWBS.Rows[j].FindControl("txtBuilding");
            TextBox txtFloor = (TextBox)grdWBS.Rows[j].FindControl("txtFloor");
            TextBox txtMemberType = (TextBox)grdWBS.Rows[j].FindControl("txtMemberType");
            TextBox txtMemberId = (TextBox)grdWBS.Rows[j].FindControl("txtMemberId");

            string msg = "";
            if (txtBuilding.Text.Trim() == "")
            {
                msg = "Input Building";
                txtBuilding.Focus();
            }
            else if (txtFloor.Text.Trim() == "")
            {
                msg = "Input Floor";
                txtFloor.Focus();
            }
            else if (txtMemberType.Text.Trim() == "")
            {
                msg = "Input Membar Type";
                txtMemberType.Focus();
            }
            else if (txtMemberId.Text.Trim() == "")
            {
                msg = "Input Member Id";
                txtMemberId.Focus();
            }
            else
            {
                var wbs = dc.NDTWBS_View_All(ddlReferenceNo.SelectedItem.Text, Convert.ToInt32(lblWBSId.Text), txtBuilding.Text, txtFloor.Text, txtMemberType.Text, txtMemberId.Text);
                if (wbs.Count() > 0)
                {
                    msg = "Duplicate title";
                    txtBuilding.Focus();
                }
                else
                {
                    dc.NDTWBS_Update(ddlReferenceNo.SelectedItem.Text, Convert.ToInt32(lblWBSId.Text), txtBuilding.Text, txtFloor.Text, txtMemberType.Text, txtMemberId.Text, false);
                    msg = "Updated Successfully";
                    grdWBSMerge.Rows[j].Cells[2].Text = txtBuilding.Text;
                    grdWBSMerge.Rows[j].Cells[3].Text = txtFloor.Text;
                    grdWBSMerge.Rows[j].Cells[4].Text = txtMemberType.Text;
                    grdWBSMerge.Rows[j].Cells[5].Text = txtMemberId.Text;
                    for (int k = 0; k < grdWBS.Rows.Count; k++)
                    {
                        CheckBox chkSelectWBS = (CheckBox)grdWBS.Rows[k].FindControl("chkSelectWBS");
                        if (chkSelectWBS.Checked == true)
                        {
                            chkSelectWBS.Checked = false;
                            grdNDTDetails.DataSource = null;
                            grdNDTDetails.DataBind();
                            lblSelectedWbs.Text = "";                            
                            for (int l = 0; l < grdWBSMerge.Rows.Count; l++)
                            {
                                CheckBox chkSelectWBSMerge = (CheckBox)grdWBSMerge.Rows[l].FindControl("chkSelectWBSMerge");
                                if (chkSelectWBSMerge.Checked == true)
                                {
                                    chkSelectWBSMerge.Checked = false;
                                    grdNDTDetails2.DataSource = null;
                                    grdNDTDetails2.DataBind();
                                    lblSelectedWbsMerge.Text = "";
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "key", "alert('" + msg + "')", true);            
        }
        protected void lnkMerge_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            int j = gvr.RowIndex;
            int wbsId = 0;
            for (int k = 0; k < grdWBS.Rows.Count; k++)
            {
                CheckBox chkSelectWBS = (CheckBox)grdWBS.Rows[k].FindControl("chkSelectWBS");
                if (chkSelectWBS.Checked == true)
                {
                    Label lblWBSId = (Label)grdWBS.Rows[k].FindControl("lblWBSId");
                    wbsId = Convert.ToInt32(lblWBSId.Text); 
                }
            }
            string msg = "";
            if (wbsId == 0)
            {
                msg = "Select title in which you want to merge.";
            }
            else
            {
                Label lblWBSIdMerge = (Label)grdWBSMerge.Rows[j].FindControl("lblWBSId");
                if (wbsId != Convert.ToInt32(lblWBSIdMerge.Text))
                {
                    dc.NDTDetail_Update_WbsId(ddlReferenceNo.SelectedItem.Text, Convert.ToInt32(lblWBSIdMerge.Text), wbsId);
                    msg = "Merged Successfully.";

                    grdNDTDetails.DataSource = null;
                    grdNDTDetails.DataBind();
                    lblSelectedWbs.Text = "";
                    grdNDTDetails2.DataSource = null;
                    grdNDTDetails2.DataBind();
                    DisplayReportDetails();
                }
            }
            if (msg != "")
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "key", "alert('" + msg + "')", true);
            }
        }

        #region grdWBS
        protected void imgBtnAddRowWBS_Click(object sender, CommandEventArgs e)
        {
            AddRowWBS();
        }

        protected void imgBtnDeleteRowWBS_Click(object sender, CommandEventArgs e)
        {
            if (grdWBS.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdWBS.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowWBS(gvr.RowIndex);
            }
        }

        protected void DeleteRowWBS(int rowIndex)
        {
            if (grdWBS.Rows.Count > 1)
            {
                GetCurrentDataWBS();
                DataTable dt = ViewState["WBS_Table"] as DataTable;
                dt.Rows[rowIndex].Delete();
                ViewState["WBS_Table"] = dt;
                grdWBS.DataSource = dt;
                grdWBS.DataBind();
                SetPreviousDataWBS();
            }
        }

        protected void AddRowWBS()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["WBS_Table"] != null)
            {
                GetCurrentDataWBS();
                dt = (DataTable)ViewState["WBS_Table"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("lblWBSId", typeof(string)));
                dt.Columns.Add(new DataColumn("txtBuilding", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFloor", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMemberType", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMemberId", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["lblWBSId"] = string.Empty;
            dr["txtBuilding"] = string.Empty;
            dr["txtFloor"] = string.Empty;
            dr["txtMemberType"] = string.Empty;
            dr["txtMemberId"] = string.Empty;

            dt.Rows.Add(dr);

            ViewState["WBS_Table"] = dt;
            grdWBS.DataSource = dt;
            grdWBS.DataBind();
            SetPreviousDataWBS();
        }

        protected void GetCurrentDataWBS()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblWBSId", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtBuilding", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtFloor", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtMemberType", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtMemberId", typeof(string)));


            for (int i = 0; i < grdWBS.Rows.Count; i++)
            {
                Label lblWBSId = (Label)grdWBS.Rows[i].FindControl("lblWBSId");
                TextBox txtBuilding = (TextBox)grdWBS.Rows[i].FindControl("txtBuilding");
                TextBox txtFloor = (TextBox)grdWBS.Rows[i].FindControl("txtFloor");
                TextBox txtMemberType = (TextBox)grdWBS.Rows[i].FindControl("txtMemberType");
                TextBox txtMemberId = (TextBox)grdWBS.Rows[i].FindControl("txtMemberId");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["lblWBSId"] = lblWBSId.Text;
                drRow["txtBuilding"] = txtBuilding.Text;
                drRow["txtFloor"] = txtFloor.Text;
                drRow["txtMemberType"] = txtMemberType.Text;
                drRow["txtMemberId"] = txtMemberId.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["WBS_Table"] = dtTable;

        }

        protected void SetPreviousDataWBS()
        {
            DataTable dt = (DataTable)ViewState["WBS_Table"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblWBSId = (Label)grdWBS.Rows[i].FindControl("lblWBSId");
                TextBox txtBuilding = (TextBox)grdWBS.Rows[i].FindControl("txtBuilding");
                TextBox txtFloor = (TextBox)grdWBS.Rows[i].FindControl("txtFloor");
                TextBox txtMemberType = (TextBox)grdWBS.Rows[i].FindControl("txtMemberType");
                TextBox txtMemberId = (TextBox)grdWBS.Rows[i].FindControl("txtMemberId");

                grdWBS.Rows[i].Cells[1].Text = (i + 1).ToString();
                lblWBSId.Text = dt.Rows[i]["lblWBSId"].ToString();
                txtBuilding.Text = dt.Rows[i]["txtBuilding"].ToString();
                txtFloor.Text = dt.Rows[i]["txtFloor"].ToString();
                txtMemberType.Text = dt.Rows[i]["txtMemberType"].ToString();
                txtMemberId.Text = dt.Rows[i]["txtMemberId"].ToString();
            }
        }
        #endregion

        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {
            #region getNDT_Details();
            bool Chk_Indirect = false;
            string NDTBy = "", HammerNo = "", txt_TestingDt = "", EnquiryNo = "";
            if (txtEnquiryNo.Text != "")
            {
                EnquiryNo = txtEnquiryNo.Text;
            }
            else
            {
                var inward = dc.Inward_View(Convert.ToInt32(ddlReferenceNo.SelectedItem.Value.Split('/')[0].ToString()), 0, "NDT", null, null);
                foreach (var inwd in inward)
                {
                    EnquiryNo = inwd.INWD_ENQ_Id.ToString();
                }
            }
            var ndtAppData = dc.NDTApp_ndtDetails_View(EnquiryNo, "All", "", "", "", "", "").ToList();
            foreach (var ndtapp in ndtAppData)
            {
                txt_TestingDt = Convert.ToDateTime(ndtapp.enquiry_date).ToString("dd/MM/yyyy");

                bool upvFlag = false, hammFlag = false;
                foreach (var ndtBy in ndtAppData)
                {
                    if (ndtBy.fk_machine_id == "UPV")
                        upvFlag = true;
                    else if (ndtBy.fk_machine_id == "RH")
                        hammFlag = true;
                    if (upvFlag == true && hammFlag == true)
                        break;
                }
                if (upvFlag == true && hammFlag == true)
                    NDTBy = "Both";
                else if (upvFlag == true && hammFlag == false)
                    NDTBy = "UPV";
                else if (upvFlag == false && hammFlag == true)
                    NDTBy = "Rebound Hammer";

                if (ndtapp.probing_type == 2)
                {
                    //Chk_Indirect.Visible = true;
                    //Chk_Indirect.Checked = true;
                    Chk_Indirect = true;
                }
                if (ndtapp.hammer_no != null && ndtapp.hammer_no != string.Empty)
                {
                    HammerNo = ndtapp.hammer_no;
                }
                //if (NDTBy == "Both" || NDTBy == "Rebound Hammer")
                //{
                //    ddl_HammerNo.Enabled = true;
                //}
                //else
                //{
                //    ddl_HammerNo.Enabled = false;
                //}
                break;
            }
            #endregion

            #region getNDTGridData();
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("txt_Description", typeof(string)));
            dt1.Columns.Add(new DataColumn("ddl_Grade", typeof(string)));
            dt1.Columns.Add(new DataColumn("txt_CastingDt", typeof(string)));
            dt1.Columns.Add(new DataColumn("ddl_AlphaAngle", typeof(string)));
            dt1.Columns.Add(new DataColumn("txt_ReboundIndexDetails", typeof(string)));
            dt1.Columns.Add(new DataColumn("txt_PulseVelDetails", typeof(string)));
            dt1.Columns.Add(new DataColumn("lblMergFlag", typeof(string)));
            dt1.Columns.Add(new DataColumn("lblImage1", typeof(string)));
            dt1.Columns.Add(new DataColumn("lblImage2", typeof(string)));
            dt1.Columns.Add(new DataColumn("lblImage3", typeof(string)));
            dt1.Columns.Add(new DataColumn("lblImage4", typeof(string)));
            dt1.Columns.Add(new DataColumn("building", typeof(string)));
            dt1.Columns.Add(new DataColumn("floor", typeof(string)));
            dt1.Columns.Add(new DataColumn("memberType", typeof(string)));
            dt1.Columns.Add(new DataColumn("memberId", typeof(string)));
            int i = 0;
            string RowNo = "", mgrd = "";
            var ndtAppDataTitle = dc.NDTApp_ndtDetails_View(EnquiryNo, "", "", "", "", "", "").ToList();
            foreach (var nt in ndtAppDataTitle)
            {
                //add row
                dr1 = dt1.NewRow();
                dt1.Rows.Add(dr1);
                //
                string txt_Description = "", lblMergFlag = "";
                lblMergFlag = "1";
                RowNo = RowNo + " " + i;
                txt_Description = nt.title + " " + nt.floor_type + " " + nt.member_type + " " + nt.id_mark;

                dt1.Rows[i]["txt_Description"] = txt_Description;
                dt1.Rows[i]["building"] = nt.title;
                dt1.Rows[i]["floor"] = nt.floor_type;
                dt1.Rows[i]["memberType"] = nt.member_type;
                dt1.Rows[i]["memberId"] = nt.id_mark;
                dt1.Rows[i]["lblMergFlag"] = lblMergFlag;
                //end row
                i++;

                bool upvFound = false;
                int rhRow = 0;
                var ndtAppDataRh = dc.NDTApp_ndtDetails_View(EnquiryNo, nt.title, nt.floor_type, nt.member_type, nt.id_mark, "", "RH").ToList();
                foreach (var nrh in ndtAppDataRh)
                {
                    //add row
                    dr1 = dt1.NewRow();
                    dt1.Rows.Add(dr1);
                    //
                    string txt_DescriptionRh = "";
                    string ddl_Grade = "";
                    string txt_CastingDt = "";
                    string ddl_AlphaAngle = "";
                    string txt_ReboundIndexDetails = "";
                    string lblMergFlagRh = "";
                    string lblImage1 = "";
                    string lblImage2 = "";

                    string txt_PulseVelDetails = "";
                    string lblImage3 = "";
                    string lblImage4 = "";

                    txt_DescriptionRh = nrh.location;
                    lblMergFlagRh = "0";

                    mgrd = Convert.ToString(nrh.grade).ToUpper();
                    mgrd = mgrd.Replace("M", "");
                    mgrd = mgrd.Replace("-", "");
                    mgrd = mgrd.Replace(" ", "");
                    mgrd = "M " + mgrd.Trim();
                    ddl_Grade = mgrd;

                    if (Convert.ToString(nrh.cast_date) != "")
                    {
                        if (Convert.ToDateTime(nrh.cast_date).ToString("dd/MM/yyyy") == "01/01/1900")
                            txt_CastingDt = "NA";
                        else
                            txt_CastingDt = Convert.ToDateTime(nrh.cast_date).ToString("dd/MM/yyyy");
                    }

                    if (Convert.ToString(nrh.angle) != "")
                    {
                        if (nrh.angle.Trim() == "0")
                            ddl_AlphaAngle = "a = 0°";
                        else if (nrh.angle.Trim() == "+90")
                            ddl_AlphaAngle = "a = +90°";
                        else if (nrh.angle.Trim() == "-90")
                            ddl_AlphaAngle = "a = -90°";
                    }
                    txt_ReboundIndexDetails = nrh.reading1.ToString() + "," + nrh.reading2.ToString() + "," + nrh.reading3.ToString() + "," + nrh.reading4.ToString() + "," + nrh.reading5.ToString() + "," + nrh.reading6.ToString() + "," + nrh.reading7.ToString() + "," + nrh.reading8.ToString() + "," + nrh.reading9.ToString() + "," + nrh.reading10.ToString();
                    #region calculate rebound index

                    double sclavgcnt = 0;
                    double Upperlimt = 0;
                    double Lowerlimt = 0;
                    int Counter = 0;
                    double Total = 0;
                    //Button ReboundIndex = (Button)grdNDTInward.Rows[i].Cells[8].FindControl("ReboundIndex");
                    string ReboundIndex = "";
                    sclavgcnt = ((Convert.ToDouble(nrh.reading1.ToString()) + Convert.ToDouble(nrh.reading2.ToString()) + Convert.ToDouble(nrh.reading3.ToString()) + Convert.ToDouble(nrh.reading4.ToString()) + Convert.ToDouble(nrh.reading5.ToString()) + Convert.ToDouble(nrh.reading6.ToString()) + Convert.ToDouble(nrh.reading7.ToString()) + Convert.ToDouble(nrh.reading8.ToString()) + Convert.ToDouble(nrh.reading9.ToString()) + Convert.ToDouble(nrh.reading10.ToString())) / 10);
                    ReboundIndex = Convert.ToDouble(sclavgcnt).ToString("0.00");//chk again

                    Upperlimt = sclavgcnt + (sclavgcnt * 0.15);
                    Lowerlimt = sclavgcnt - (sclavgcnt * 0.15);

                    if (Convert.ToDouble(nrh.reading1.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading1.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading1.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading2.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading2.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading2.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading3.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading3.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading3.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading4.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading4.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading4.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading5.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading5.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading5.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading6.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading6.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading6.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading7.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading7.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading7.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading8.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading8.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading8.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading9.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading9.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading9.ToString());
                    }
                    if (Convert.ToDouble(nrh.reading10.ToString()) >= Lowerlimt && Convert.ToDouble(nrh.reading10.ToString()) <= Upperlimt)
                    {
                        Counter++;
                        Total += Convert.ToDouble(nrh.reading10.ToString());
                    }

                    if (Total > 0)
                    {
                        sclavgcnt = 0;
                        sclavgcnt = Total / Counter;
                        ReboundIndex = Convert.ToDouble(sclavgcnt).ToString("0.00");
                        //IndStrOnSclAvg(Convert.ToDouble(ReboundIndex.Text), i);//Mluitply by CorectionFcator                            
                    }
                    txt_ReboundIndexDetails = ReboundIndex + "|" + txt_ReboundIndexDetails;
                    #endregion

                    if (Convert.ToString(nrh.image1) != null && Convert.ToString(nrh.image1) != "NULL")
                        lblImage1 = nrh.image1;
                    if (Convert.ToString(nrh.image2) != null && Convert.ToString(nrh.image2) != "NULL")
                        lblImage2 = nrh.image2;

                    if (NDTBy == "Both")
                    {
                        var ndtAppDataUpv = dc.NDTApp_ndtDetails_View(EnquiryNo, nt.title, nt.floor_type, nt.member_type, nt.id_mark, nrh.location, "UPV").ToList();
                        foreach (var nupv in ndtAppDataUpv)
                        {
                            upvFound = true;
                            if (nupv.probing_type == 2)
                            {
                                //txt_PulseVelDetails = "Distance-Time, "
                                //    + nupv.dist_1 + "-" + nupv.time_1 + ", "
                                //    + nupv.dist_2 + "-" + nupv.time_2 + ", "
                                //    + nupv.dist_3 + "-" + nupv.time_3 + ", "
                                //    + nupv.dist_4 + "-" + nupv.time_4;
                                #region calculate pulse velocity
                                decimal[] X = new decimal[4];
                                decimal[] Y = new decimal[4];
                                decimal[] Xbar = new decimal[4];
                                decimal[] Ybar = new decimal[4];
                                decimal[] XbarSQ = new decimal[4];
                                decimal[] XbarYbar = new decimal[4];
                                decimal Xavg = 0, Yavg = 0, m_slope = 0;
                                X[0] = Convert.ToDecimal(nupv.dist_1);
                                X[1] = Convert.ToDecimal(nupv.dist_2);
                                X[2] = Convert.ToDecimal(nupv.dist_3);
                                X[3] = Convert.ToDecimal(nupv.dist_4);
                                Y[0] = Convert.ToDecimal(nupv.time_1);
                                Y[1] = Convert.ToDecimal(nupv.time_2);
                                Y[2] = Convert.ToDecimal(nupv.time_3);
                                Y[3] = Convert.ToDecimal(nupv.time_4);
                                Xavg = Math.Round(((X[0] + X[1] + X[2] + X[3]) / 4), 2);
                                Yavg = Math.Round(((Y[0] + Y[1] + Y[2] + Y[3]) / 4), 2);
                                Xbar[0] = X[0] - Xavg;
                                Xbar[1] = X[1] - Xavg;
                                Xbar[2] = X[2] - Xavg;
                                Xbar[3] = X[3] - Xavg;
                                Ybar[0] = Y[0] - Yavg;
                                Ybar[1] = Y[1] - Yavg;
                                Ybar[2] = Y[2] - Yavg;
                                Ybar[3] = Y[3] - Yavg;
                                XbarSQ[0] = Xbar[0] * Xbar[0];
                                XbarSQ[1] = Xbar[1] * Xbar[1];
                                XbarSQ[2] = Xbar[2] * Xbar[2];
                                XbarSQ[3] = Xbar[3] * Xbar[3];
                                XbarYbar[0] = Xbar[0] * Ybar[0];
                                XbarYbar[1] = Xbar[1] * Ybar[1];
                                XbarYbar[2] = Xbar[2] * Ybar[2];
                                XbarYbar[3] = Xbar[3] * Ybar[3];
                                m_slope = (XbarYbar[0] + XbarYbar[1] + XbarYbar[2] + XbarYbar[3]) / (XbarSQ[0] + XbarSQ[1] + XbarSQ[2] + XbarSQ[3]);
                                decimal PulseVel = Math.Round(1/m_slope,2);
                                if (PulseVel > 3)
                                    PulseVel += Convert.ToDecimal(0.5);
                                txt_PulseVelDetails = PulseVel + "|" + nupv.dist_1 + "-" + nupv.time_1 + ", "
                                                                + nupv.dist_2 + "-" + nupv.time_2 + ", "
                                                                + nupv.dist_3 + "-" + nupv.time_3 + ", "
                                                                + nupv.dist_4 + "-" + nupv.time_4;
                                #endregion
                            }
                            else
                            {
                                txt_PulseVelDetails = nupv.distance + "," + nupv.ndt_time;
                                #region calculate pulse velocity
                                string PulseVel = "";
                                PulseVel = (Convert.ToDecimal(nupv.distance) / Convert.ToDecimal(nupv.ndt_time)).ToString("0.00");
                                txt_PulseVelDetails = PulseVel + "|" + txt_PulseVelDetails;
                                #endregion
                            }

                            if (Convert.ToString(nupv.image1) != null && Convert.ToString(nupv.image1) != "NULL")
                                lblImage3 = nupv.image1;
                            if (Convert.ToString(nupv.image2) != null && Convert.ToString(nupv.image2) != "NULL")
                                lblImage4 = nupv.image2;
                        }
                    }

                    dt1.Rows[i]["txt_Description"] = txt_DescriptionRh;
                    dt1.Rows[i]["ddl_Grade"] = ddl_Grade;
                    dt1.Rows[i]["txt_CastingDt"] = txt_CastingDt;
                    dt1.Rows[i]["ddl_AlphaAngle"] = ddl_AlphaAngle;
                    dt1.Rows[i]["txt_ReboundIndexDetails"] = txt_ReboundIndexDetails;
                    dt1.Rows[i]["txt_PulseVelDetails"] = txt_PulseVelDetails;
                    dt1.Rows[i]["lblMergFlag"] = lblMergFlagRh;
                    dt1.Rows[i]["lblImage1"] = lblImage1;
                    dt1.Rows[i]["lblImage2"] = lblImage2;
                    dt1.Rows[i]["lblImage3"] = lblImage3;
                    dt1.Rows[i]["lblImage4"] = lblImage4;
                    //end row
                    i++;
                    rhRow++;
                }

                if (NDTBy != "Both" || upvFound == false)
                {
                    int upvRow = 0;
                    i = i - rhRow;
                    var ndtAppDataUpv = dc.NDTApp_ndtDetails_View(EnquiryNo, nt.title, nt.floor_type, nt.member_type, nt.id_mark, "", "UPV").ToList();
                    foreach (var nupv in ndtAppDataUpv)
                    {
                        if (upvRow >= rhRow)
                        {
                            //add row
                            dr1 = dt1.NewRow();
                            dt1.Rows.Add(dr1);
                            //
                        }
                        string txt_DescriptionRh = "";
                        string ddl_Grade = "";
                        string txt_CastingDt = "";
                        string txt_PulseVelDetails = "";
                        string lblMergFlagRh = "";
                        string lblImage3 = "";
                        string lblImage4 = "";

                        txt_DescriptionRh = nupv.location;
                        lblMergFlagRh = "0";

                        mgrd = Convert.ToString(nupv.grade).ToUpper();
                        mgrd = mgrd.Replace("M", "");
                        mgrd = mgrd.Replace("-", "");
                        mgrd = mgrd.Replace(" ", "");
                        mgrd = "M " + mgrd.Trim();
                        ddl_Grade = mgrd;

                        if (Convert.ToString(nupv.cast_date) != "")
                        {
                            if (Convert.ToDateTime(nupv.cast_date).ToString("dd/MM/yyyy") == "01/01/1900")
                                txt_CastingDt = "NA";
                            else
                                txt_CastingDt = Convert.ToDateTime(nupv.cast_date).ToString("dd/MM/yyyy");
                        }

                        if (nupv.probing_type == 2)
                        {
                            //txt_PulseVelDetails = "Distance-Time, "
                            //    + nupv.dist_1 + "-" + nupv.time_1 + ", "
                            //    + nupv.dist_2 + "-" + nupv.time_2 + ", "
                            //    + nupv.dist_3 + "-" + nupv.time_3 + ", "
                            //    + nupv.dist_4 + "-" + nupv.time_4;
                            #region calculate pulse velocity
                            decimal[] X = new decimal[4];
                            decimal[] Y = new decimal[4];
                            decimal[] Xbar = new decimal[4];
                            decimal[] Ybar = new decimal[4];
                            decimal[] XbarSQ = new decimal[4];
                            decimal[] XbarYbar = new decimal[4];
                            decimal Xavg = 0, Yavg = 0, m_slope = 0, PulseVel = 0;
                            X[0] = Convert.ToDecimal(nupv.dist_1);
                            X[1] = Convert.ToDecimal(nupv.dist_2);
                            X[2] = Convert.ToDecimal(nupv.dist_3);
                            X[3] = Convert.ToDecimal(nupv.dist_4);
                            Y[0] = Convert.ToDecimal(nupv.time_1);
                            Y[1] = Convert.ToDecimal(nupv.time_2);
                            Y[2] = Convert.ToDecimal(nupv.time_3);
                            Y[3] = Convert.ToDecimal(nupv.time_4);
                            Xavg = Math.Round(((X[0] + X[1] + X[2] + X[3]) / 4), 2);
                            Yavg = Math.Round(((Y[0] + Y[1] + Y[2] + Y[3]) / 4), 2);
                            Xbar[0] = X[0] - Xavg;
                            Xbar[1] = X[1] - Xavg;
                            Xbar[2] = X[2] - Xavg;
                            Xbar[3] = X[3] - Xavg;
                            Ybar[0] = Y[0] - Yavg;
                            Ybar[1] = Y[1] - Yavg;
                            Ybar[2] = Y[2] - Yavg;
                            Ybar[3] = Y[3] - Yavg;
                            XbarSQ[0] = Xbar[0] * Xbar[0];
                            XbarSQ[1] = Xbar[1] * Xbar[1];
                            XbarSQ[2] = Xbar[2] * Xbar[2];
                            XbarSQ[3] = Xbar[3] * Xbar[3];
                            XbarYbar[0] = Xbar[0] * Ybar[0];
                            XbarYbar[1] = Xbar[1] * Ybar[1];
                            XbarYbar[2] = Xbar[2] * Ybar[2];
                            XbarYbar[3] = Xbar[3] * Ybar[3];
                            m_slope = (XbarYbar[0] + XbarYbar[1] + XbarYbar[2] + XbarYbar[3]) / (XbarSQ[0] + XbarSQ[1] + XbarSQ[2] + XbarSQ[3]);
                            PulseVel = 1 / m_slope;
                            if (PulseVel >= 3)
                                PulseVel += Convert.ToDecimal(0.5);
                            PulseVel = Math.Round(PulseVel, 2);
                            txt_PulseVelDetails = PulseVel + "|" + nupv.dist_1 + "-" + nupv.time_1 + ", "
                                                                + nupv.dist_2 + "-" + nupv.time_2 + ", "
                                                                + nupv.dist_3 + "-" + nupv.time_3 + ", "
                                                                + nupv.dist_4 + "-" + nupv.time_4;
                            #endregion
                        }
                        else
                        {
                            txt_PulseVelDetails = nupv.distance + "," + nupv.ndt_time;
                            #region calculate pulse velocity
                            string PulseVel = "";
                            PulseVel = (Convert.ToDecimal(nupv.distance) / Convert.ToDecimal(nupv.ndt_time)).ToString("0.00");
                            txt_PulseVelDetails = PulseVel + "|" + txt_PulseVelDetails;
                            #endregion
                        }

                        if (Convert.ToString(nupv.image1) != null && Convert.ToString(nupv.image1) != "NULL")
                            lblImage3 = nupv.image1;
                        if (Convert.ToString(nupv.image2) != null && Convert.ToString(nupv.image2) != "NULL")
                            lblImage4 = nupv.image2;

                        dt1.Rows[i]["txt_Description"] = txt_DescriptionRh;
                        dt1.Rows[i]["ddl_Grade"] = ddl_Grade;
                        dt1.Rows[i]["txt_CastingDt"] = txt_CastingDt;
                        dt1.Rows[i]["txt_PulseVelDetails"] = txt_PulseVelDetails;
                        dt1.Rows[i]["lblMergFlag"] = lblMergFlagRh;
                        dt1.Rows[i]["lblImage3"] = lblImage3;
                        dt1.Rows[i]["lblImage4"] = lblImage4;
                        //end row
                        i++;
                        upvRow++;
                    }
                    if (upvRow < rhRow)
                    {
                        i = i + (rhRow - upvRow);
                    }
                }
            }
            //grdNDTData.DataSource = dt1;
            //grdNDTData.DataBind();
            #endregion

            if (dt1.Rows.Count > 0)
            {
                #region update ndt data     
                DateTime TestingDt = DateTime.ParseExact(txt_TestingDt, "dd/MM/yyyy", null);
                dc.NDT_TestInward_Update(ddlReferenceNo.SelectedItem.Text, 2, TestingDt, "", HammerNo, NDTBy, "NDT", "", false, "", Chk_Indirect, false);

                dc.NDTDetail_Update(ddlReferenceNo.SelectedItem.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, true, "", "", "", "", "", 0);
                dc.Title_Update(ddlReferenceNo.SelectedItem.Text, "", "NDT", 0, true);
                dc.NDTWBS_Update(ddlReferenceNo.SelectedItem.Text, 0, "", "", "", "", true);
                int wbsid = 0;
                for (int d = 0; d < dt1.Rows.Count; d++)
                {
                    int TitleId = 0;
                    if (dt1.Rows[d]["lblMergFlag"].ToString() != "1")
                    {
                        dc.NDTDetail_Update(ddlReferenceNo.SelectedItem.Text, dt1.Rows[d]["txt_Description"].ToString(), dt1.Rows[d]["ddl_Grade"].ToString(), dt1.Rows[d]["txt_CastingDt"].ToString(), dt1.Rows[d]["ddl_AlphaAngle"].ToString(), dt1.Rows[d]["txt_ReboundIndexDetails"].ToString(), dt1.Rows[d]["txt_PulseVelDetails"].ToString(), "", "", "", "", "", "", "", "", "", TitleId, false, "", dt1.Rows[d]["lblImage1"].ToString(), dt1.Rows[d]["lblImage2"].ToString(), dt1.Rows[d]["lblImage3"].ToString(), dt1.Rows[d]["lblImage4"].ToString(), wbsid);
                    }
                    else
                    {
                        wbsid = dc.NDTWBS_Update(ddlReferenceNo.SelectedItem.Text, 0, dt1.Rows[d]["building"].ToString(), dt1.Rows[d]["floor"].ToString(), dt1.Rows[d]["membertype"].ToString(), dt1.Rows[d]["memberid"].ToString(), false);
                        TitleId = dc.Title_Update(ddlReferenceNo.SelectedItem.Text, dt1.Rows[d]["txt_Description"].ToString(), "NDT", wbsid, false);
                        //var t = dc.TestDetail_Title_View(ddlReferenceNo.SelectedItem.Text, 0, "NDT", true);
                        //foreach (var tt in t)
                        //{
                        //    TitleId = Convert.ToInt32(tt.TitleId_int);
                        //}                    
                        //dc.NDTDetail_Update(ddlReferenceNo.SelectedItem.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", TitleId, false, "", "", "", "", "", wbsid);
                    }
                }
                //dc.NDTInward_Update_Status(ddlReferenceNo.SelectedItem.Text, 2);

                #endregion

                #region update grade wise equation 
                dc.NDTGradeEquation_Update(ddlReferenceNo.SelectedValue, "", "", true);
                var ndtdetailgrade = dc.NDTDetail_View_Grades(ddlReferenceNo.SelectedValue, "").ToList();
                foreach (var grd in ndtdetailgrade)
                {
                    int countUpv = 0, countHamm = 0;
                    decimal sumUpv = 0, sumHamm = 0;
                    string strEquation = "";
                    var ndtdetail = dc.NDTDetail_View_Grades(ddlReferenceNo.SelectedValue, grd.Grade_var);
                    foreach (var nd in ndtdetail)
                    {
                        if (nd.PulseVelocity_var.Contains('|') && nd.PulseVelocity_var.Split('|')[0].ToString() != "NO" && nd.PulseVelocity_var.Split('|')[0].ToString() != "")
                        {
                            sumUpv += Convert.ToDecimal(nd.PulseVelocity_var.Split('|')[0].ToString());
                            countUpv++;
                        }
                        if (nd.ReboundIndex_var.Contains('|')  && nd.ReboundIndex_var.Split('|')[0].ToString() != "NO" && nd.ReboundIndex_var.Split('|')[0].ToString() != "")
                        {
                            sumHamm += Convert.ToDecimal(nd.ReboundIndex_var.Split('|')[0].ToString());
                            countHamm++;
                        }
                    }
                    if (countUpv > 0 && countHamm > 0)
                    {
                        sumUpv = Math.Round(sumUpv / countUpv, 2);
                        sumHamm = Math.Round(sumHamm / countHamm, 2);
                        
                        if (sumUpv > 0 && sumHamm > 0)
                        {
                            if (sumHamm < 30)
                            {
                                if (sumUpv < Convert.ToDecimal(3.5))
                                {
                                    strEquation = "Regular";
                                }
                                else if (sumUpv >= Convert.ToDecimal(3.5) && sumUpv <= Convert.ToDecimal(4))
                                {
                                    strEquation = "Regular";
                                }
                                else if (sumUpv >= Convert.ToDecimal(4) && sumUpv <= Convert.ToDecimal(4.5))
                                {
                                    strEquation = "Regular";
                                }
                                else if (sumUpv >= Convert.ToDecimal(4.5))
                                {
                                    strEquation = "";
                                }
                            }
                            else if (sumHamm >= 30 && sumHamm <= 40)
                            {
                                if (sumUpv < Convert.ToDecimal(3.5))
                                {
                                    strEquation = "Regular";
                                }
                                else if (sumUpv >= Convert.ToDecimal(3.5) && sumUpv <= Convert.ToDecimal(4))
                                {
                                    strEquation = "With in 10";
                                }
                                else if (sumUpv >= Convert.ToDecimal(4) && sumUpv <= Convert.ToDecimal(4.5))
                                {
                                    strEquation = "With in 10";
                                }
                                else if (sumUpv >= Convert.ToDecimal(4.5))
                                {
                                    strEquation = "";
                                }
                            }
                            else if (sumHamm >= 40 && sumHamm <= 50)
                            {
                                if (sumUpv < Convert.ToDecimal(3.5))
                                {
                                    strEquation = "Minus 10";
                                }
                                else if (sumUpv >= Convert.ToDecimal(3.5) && sumUpv <= Convert.ToDecimal(4))
                                {
                                    strEquation = "With in 10";
                                }
                                else if (sumUpv >= Convert.ToDecimal(4) && sumUpv <= Convert.ToDecimal(4.5))
                                {
                                    strEquation = "With in 10";
                                }
                                else if (sumUpv > Convert.ToDecimal(4.5))
                                {
                                    strEquation = "Plus 10";
                                }
                            }
                            else if (sumHamm >= 50)
                            {
                                if (sumUpv < Convert.ToDecimal(3.5))
                                {
                                    strEquation = "Minus 10";
                                }
                                else if (sumUpv >= Convert.ToDecimal(3.5) && sumUpv <= Convert.ToDecimal(4))
                                {
                                    strEquation = "Minus 10";
                                }
                                else if (sumUpv >= Convert.ToDecimal(4) && sumUpv <= Convert.ToDecimal(4.5))
                                {
                                    strEquation = "With in 10";
                                }
                                else if (sumUpv >= Convert.ToDecimal(4.5))
                                {
                                    strEquation = "Plus 10";
                                }
                            }
                        }
                        else if (sumUpv > 0 && sumHamm == 0)
                        {
                            if (sumUpv < Convert.ToDecimal(3.5))
                            {
                                strEquation = "";
                            }
                            else if (sumUpv >= Convert.ToDecimal(3.5) && sumUpv <= Convert.ToDecimal(4))
                            {
                                strEquation = "";
                            }
                            else if (sumUpv >= Convert.ToDecimal(4) && sumUpv <= Convert.ToDecimal(4.5))
                            {
                                strEquation = "";
                            }
                            else if (sumUpv >= Convert.ToDecimal(4.5))
                            {
                                strEquation = "";
                            }
                        }
                        else if (sumUpv == 0 && sumHamm > 0)
                        {
                            if (sumHamm < 30)
                            {
                                strEquation = "";
                            }
                            else if (sumHamm >= 30 && sumHamm <= 40)
                            {
                                strEquation = "";
                            }
                            else if (sumHamm >= 40 && sumHamm <= 50)
                            {
                                strEquation = "";
                            }
                            else if (sumHamm >= 50)
                            {
                                strEquation = "";
                            }
                        }
                    }
                    if (strEquation == "")
                        strEquation = "Regular";
                    dc.NDTGradeEquation_Update(ddlReferenceNo.SelectedValue, grd.Grade_var, strEquation, false);

                }
                #endregion

                ClearAllControls();
                DisplayReportDetails();
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "key", "alert('Data updated successfully.')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "key", "alert('Data not available.')", true);
            }
        }

        protected void lnkExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void grdWBS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "")
            {
                GridViewRow grdrow = (GridViewRow)(((CheckBox)e.CommandSource).NamingContainer);
                int j = grdrow.RowIndex;

                for (int i = 0; i < grdWBSMerge.Rows.Count; i++)
                {
                    CheckBox chkSelectWBSMerge1 = (CheckBox)grdWBSMerge.Rows[i].FindControl("chkSelectWBSMerge");
                    chkSelectWBSMerge1.Checked = false;
                }
                grdNDTDetails.DataSource = null;
                grdNDTDetails.DataBind();
                grdNDTDetails2.DataSource = null;
                grdNDTDetails2.DataBind();
                lblSelectedWbs.Text = "";
                //CheckBox btn = (CheckBox)sender;
                //GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                //int j = gvr.RowIndex;

                int cntSel = 0;
                for (int i = 0; i < grdWBS.Rows.Count; i++)
                {
                    //if (i != j)
                    //{
                    //    CheckBox chkSelectWBS1 = (CheckBox)grdWBS.Rows[i].FindControl("chkSelectWBS");
                    //    chkSelectWBS1.Checked = false;
                    //}
                    CheckBox chkSelectWBS1 = (CheckBox)grdWBS.Rows[i].FindControl("chkSelectWBS");
                    if (chkSelectWBS1.Checked == true)
                        cntSel++;
                }
                CheckBox chkSelectWBS = (CheckBox)grdWBS.Rows[j].FindControl("chkSelectWBS");
                if (chkSelectWBS.Checked == true && cntSel == 1)
                {
                    Label lblWBSId = (Label)grdWBS.Rows[j].FindControl("lblWBSId");
                    TextBox txtBuilding = (TextBox)grdWBS.Rows[j].FindControl("txtBuilding");
                    TextBox txtFloor = (TextBox)grdWBS.Rows[j].FindControl("txtFloor");
                    TextBox txtMemberType = (TextBox)grdWBS.Rows[j].FindControl("txtMemberType");
                    TextBox txtMemberId = (TextBox)grdWBS.Rows[j].FindControl("txtMemberId");
                    lblSelectedWbs.Text = "Building - " + txtBuilding.Text + ", Floor - " + txtFloor.Text + ", Member Type - " + txtMemberType.Text + ", Member Id - " + txtMemberId.Text;

                    DataTable dt1 = new DataTable();
                    DataRow dr1 = null;
                    dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                    dt1.Columns.Add(new DataColumn("lblDescription", typeof(string)));
                    dt1.Columns.Add(new DataColumn("lblGrade", typeof(string)));
                    dt1.Columns.Add(new DataColumn("lblCastingDate", typeof(string)));
                    dt1.Columns.Add(new DataColumn("lblAlphaAngle", typeof(string)));
                    dt1.Columns.Add(new DataColumn("lblReboundIndex", typeof(string)));
                    dt1.Columns.Add(new DataColumn("lblPulseVel", typeof(string)));
                    dt1.Columns.Add(new DataColumn("lblAge", typeof(string)));
                    dt1.Columns.Add(new DataColumn("lblIndStr", typeof(string)));

                    int i = 0;
                    var wbs = dc.NDTDetail_View(ddlReferenceNo.SelectedValue, Convert.ToInt32(lblWBSId.Text)).ToList();
                    foreach (var w in wbs)
                    {
                        dr1 = dt1.NewRow();
                        dr1["lblSrNo"] = (i + 1).ToString();
                        dr1["lblDescription"] = w.Description_var;
                        dr1["lblGrade"] = w.Grade_var;
                        dr1["lblCastingDate"] = w.Castingdate_var;
                        dr1["lblAlphaAngle"] = w.AlphaAngle_var;
                        dr1["lblReboundIndex"] = w.ReboundIndex_var;
                        dr1["lblPulseVel"] = w.PulseVelocity_var;
                        dr1["lblAge"] = w.Age_var;
                        dr1["lblIndStr"] = w.IndicativeStrength_var;
                        dt1.Rows.Add(dr1);

                        i++;
                    }
                    grdNDTDetails.DataSource = dt1;
                    grdNDTDetails.DataBind();
                }
            }
        }

        private bool ValidationForUpdate()
        {
            bool valid = true;
            int cntSel = 0;
            string msg = "";
            for (int i = 0; i < grdWBS.Rows.Count; i++)
            {
                CheckBox chkSelectWBS1 = (CheckBox)grdWBS.Rows[i].FindControl("chkSelectWBS");
                if (chkSelectWBS1.Checked == true)
                {
                    cntSel++;

                    Label lblWBSId = (Label)grdWBS.Rows[i].FindControl("lblWBSId");
                    TextBox txtBuilding = (TextBox)grdWBS.Rows[i].FindControl("txtBuilding");
                    TextBox txtFloor = (TextBox)grdWBS.Rows[i].FindControl("txtFloor");
                    TextBox txtMemberType = (TextBox)grdWBS.Rows[i].FindControl("txtMemberType");
                    TextBox txtMemberId = (TextBox)grdWBS.Rows[i].FindControl("txtMemberId");
                                        
                    if (txtBuilding.Text.Trim() == "")
                    {
                        msg = "Input Building";
                        txtBuilding.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtFloor.Text.Trim() == "")
                    {
                        msg = "Input Floor";
                        txtFloor.Focus();
                        valid = false;
                        break;
                    }
                    else if (txtMemberType.Text.Trim() == "")
                    {
                        msg = "Input Membar Type";
                        txtMemberType.Focus();
                        valid = false;
                    }
                    else if (txtMemberId.Text.Trim() == "")
                    {
                        msg = "Input Member Id";
                        txtMemberId.Focus();
                        valid = false;
                        break;
                    }                    
                }
            }
            for (int i = 0; i < grdWBS.Rows.Count; i++)
            {
                TextBox txtBuilding = (TextBox)grdWBS.Rows[i].FindControl("txtBuilding");
                TextBox txtFloor = (TextBox)grdWBS.Rows[i].FindControl("txtFloor");
                TextBox txtMemberType = (TextBox)grdWBS.Rows[i].FindControl("txtMemberType");
                TextBox txtMemberId = (TextBox)grdWBS.Rows[i].FindControl("txtMemberId");

                CheckBox chkSelectWBS1 = (CheckBox)grdWBS.Rows[i].FindControl("chkSelectWBS");
                if (chkSelectWBS1.Checked == true)
                {
                    for (int j = 0; j < grdWBS.Rows.Count; j++)
                    {
                        TextBox txtBuilding1 = (TextBox)grdWBS.Rows[j].FindControl("txtBuilding");
                        TextBox txtFloor1 = (TextBox)grdWBS.Rows[j].FindControl("txtFloor");
                        TextBox txtMemberType1 = (TextBox)grdWBS.Rows[j].FindControl("txtMemberType");
                        TextBox txtMemberId1 = (TextBox)grdWBS.Rows[j].FindControl("txtMemberId");
                        if (i != j)
                        {
                            if (txtBuilding.Text.Trim() == txtBuilding1.Text.Trim() && txtFloor.Text.Trim() == txtFloor1.Text.Trim()
                                && txtMemberType.Text.Trim() == txtMemberType1.Text.Trim() && txtMemberId.Text.Trim() == txtMemberId1.Text.Trim())
                            {
                                msg = "Duplicate title";
                                txtBuilding1.Focus();
                                valid = false;
                                break;
                            }
                        }
                    }
                }
            }
            if (cntSel == 0)
            {
                valid = false;
                msg = "Select at least one title for updation.";
            }
            if (valid == false)
            {               
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "key", "alert('" + msg + "')", true);
            }
            return valid;
        }

        protected void lnkUpdateAllSelected_Click(object sender, EventArgs e)
        {
            if (ValidationForUpdate() == true)
            {
                for (int i = 0; i < grdWBS.Rows.Count; i++)
                {
                    CheckBox chkSelectWBS1 = (CheckBox)grdWBS.Rows[i].FindControl("chkSelectWBS");
                    if (chkSelectWBS1.Checked == true)
                    {
                        Label lblWBSId = (Label)grdWBS.Rows[i].FindControl("lblWBSId");
                        TextBox txtBuilding = (TextBox)grdWBS.Rows[i].FindControl("txtBuilding");
                        TextBox txtFloor = (TextBox)grdWBS.Rows[i].FindControl("txtFloor");
                        TextBox txtMemberType = (TextBox)grdWBS.Rows[i].FindControl("txtMemberType");
                        TextBox txtMemberId = (TextBox)grdWBS.Rows[i].FindControl("txtMemberId");

                        dc.NDTWBS_Update(ddlReferenceNo.SelectedItem.Text, Convert.ToInt32(lblWBSId.Text), txtBuilding.Text, txtFloor.Text, txtMemberType.Text, txtMemberId.Text, false);
                        
                        grdWBSMerge.Rows[i].Cells[2].Text = txtBuilding.Text;
                        grdWBSMerge.Rows[i].Cells[3].Text = txtFloor.Text;
                        grdWBSMerge.Rows[i].Cells[4].Text = txtMemberType.Text;
                        grdWBSMerge.Rows[i].Cells[5].Text = txtMemberId.Text;                        
                    }
                }
                for (int k = 0; k < grdWBS.Rows.Count; k++)
                {
                    CheckBox chkSelectWBS = (CheckBox)grdWBS.Rows[k].FindControl("chkSelectWBS");
                    if (chkSelectWBS.Checked == true)
                    {
                        chkSelectWBS.Checked = false;
                    }
                }
                for (int l = 0; l < grdWBSMerge.Rows.Count; l++)
                {
                    CheckBox chkSelectWBSMerge = (CheckBox)grdWBSMerge.Rows[l].FindControl("chkSelectWBSMerge");
                    if (chkSelectWBSMerge.Checked == true)
                    {
                        chkSelectWBSMerge.Checked = false;
                    }
                }
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "key", "alert('Data updated successfully.')", true);
                grdNDTDetails.DataSource = null;
                grdNDTDetails.DataBind();
                lblSelectedWbs.Text = "";
                grdNDTDetails2.DataSource = null;
                grdNDTDetails2.DataBind();
                lblSelectedWbsMerge.Text = "";
            }
        }

        protected void lnkMergeAllSelected_Click(object sender, EventArgs e)
        {
            bool valid = true;
            int cntSel = 0, wbsId = 0;
            string msg = "";
            for (int i = 0; i < grdWBS.Rows.Count; i++)
            {
                CheckBox chkSelectWBS1 = (CheckBox)grdWBS.Rows[i].FindControl("chkSelectWBS");
                if (chkSelectWBS1.Checked == true)
                {
                    Label lblWBSId = (Label)grdWBS.Rows[i].FindControl("lblWBSId");
                    wbsId = Convert.ToInt32(lblWBSId.Text);
                    cntSel++;
                    if (cntSel > 1)
                    {                        
                        msg = "Select only one title in which you want to merge.";
                        valid = false;
                        break;
                    }
                }
            }
            if (cntSel == 0)
            {
                msg = "Select title in which you want to merge.";
                valid = false;
            }
            if (valid == true)
            {
                cntSel = 0;
                for (int k = 0; k < grdWBSMerge.Rows.Count; k++)
                {
                    CheckBox chkSelectWBSMerge = (CheckBox)grdWBSMerge.Rows[k].FindControl("chkSelectWBSMerge");
                    if (chkSelectWBSMerge.Checked == true)
                    {                        
                        cntSel++;
                        break;
                    }
                }
                if (cntSel == 0)
                {
                    msg = "Select title to merge.";
                    valid = false;
                }
            }
            if (valid == true)
            {
                for (int i = 0; i < grdWBSMerge.Rows.Count; i++)
                {
                    CheckBox chkSelectWBSMerge = (CheckBox)grdWBSMerge.Rows[i].FindControl("chkSelectWBSMerge");
                    if (chkSelectWBSMerge.Checked == true)
                    {
                        Label lblWBSIdMerge = (Label)grdWBSMerge.Rows[i].FindControl("lblWBSId");
                        dc.NDTDetail_Update_WbsId(ddlReferenceNo.SelectedItem.Text, Convert.ToInt32(lblWBSIdMerge.Text), wbsId);
                    }
                }
                msg = "Merged Successfully.";

                grdNDTDetails.DataSource = null;
                grdNDTDetails.DataBind();
                lblSelectedWbs.Text = "";
                grdNDTDetails2.DataSource = null;
                grdNDTDetails2.DataBind();
                DisplayReportDetails();
            }
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "key", "alert('" + msg + "')", true);
        }
    }
}