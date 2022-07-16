using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class MistakeRegister : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                RdnNewMistake.Checked = true;
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Mistake Register";
                //BindUserListDoneBy();
                txt_DoneBy.Text = Session["LoginUserName"].ToString();
                BindUserListDetectedBy();
                CurrentDate();

                bool approveRight = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_RptApproval_right_bit == true)
                        approveRight = true;
                }
                if (approveRight == false)
                {
                    RdnApproveMIstake.Enabled = false;
                }
            }

        }
        public void CurrentDate()
        {
            ddl_InwardType_DT.Items.Add("---Select---");
            txt_FromDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txt_MistakeDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }

        public void cleartext()
        {
            ddl_MistakeFor.ClearSelection();
            //ddl_DoneBy.ClearSelection();
            txt_DoneBy.Text = Session["LoginUserName"].ToString();
            ddl_DetectedBy.ClearSelection();
            ddl_InwardType_DT.ClearSelection();
            ddl_TypeofMistake.ClearSelection();
            txt_PreventiveMeasure.Text = string.Empty;
            txt_Reason.Text = string.Empty;
            txt_Description.Text = string.Empty;
            txt_CorrectiveAction.Text = string.Empty;
            grdMistake.Visible = false;
            ddlMF.SelectedValue = "MDL";
            chkAddSupersedeNote.Visible = false;
            chkAddSupersedeNote.Checked = false;
        }

        protected void RdnNewMistake_CheckedChanged(object sender, EventArgs e)
        {
            if (RdnNewMistake.Checked)
            {
                lnkSave.Enabled = true;
                ModalPopupExtender1.Hide();
                cleartext();
                checkApprove();
            }
          
        }
        protected void RdnApproveMIstake_CheckedChanged(object sender, EventArgs e)
        {
            if (RdnApproveMIstake.Checked)
            {
                lnkSave.Enabled = true;
                ModalPopupExtender1.Show();
                pnlMistake.Visible = true;
                lblFromSearch.Visible = false;
                txtFromSearch.Visible = false;
                lblToSearch.Visible = false;
                txtToSearch.Visible = false;
                optPending.Visible = false;
                optApproved.Visible = false;
                lnkFetchList.Visible = false;
                lblApproveMistake.Text = "Approve Mistake";
                BindApproveGrid();
                checkApprove();
            }
        }
        public void checkApprove()
        {
            if (RdnApproveMIstake.Checked)
            {
                Mainpan.Width = 200;
                grdMistake.Columns[2].Visible = true;
                for (int i = 0; i <= grdMistake.Rows.Count - 1; i++)
                {
                    Label lbl_Number = (Label)grdMistake.Rows[i].Cells[1].FindControl("lbl_Number");
                    DropDownList ddl_RecheckBy = (DropDownList)grdMistake.Rows[i].Cells[2].FindControl("ddl_RecheckBy");
                    lbl_Number.Width = 70;
                    ddl_RecheckBy.Width = 100;
                }
            }
            else if (RdnNewMistake.Checked)
            {
                lnkSave.Text = "Save";
                Mainpan.Width = 170;
                grdMistake.Columns[2].Visible = false;
            }
        }
        protected void imgCloseMistakePopup_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();

            if (lnkFetchList.Visible == false)
            {
                LoadSelectedMistake();
            }
            else
            {
                RdnNewMistake.Checked = true;
                RdnApproveMIstake.Checked = false;
            }
        }
        protected void grdMistake_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl_RecheckBy = (DropDownList)e.Row.FindControl("ddl_RecheckBy");
                var Rechcekuser = dc.User_View(0, -1, "", "", "");
                ddl_RecheckBy.DataSource = Rechcekuser;
                ddl_RecheckBy.DataTextField = "USER_Name_var";
                ddl_RecheckBy.DataValueField = "USER_Id";
                ddl_RecheckBy.DataBind();
                ddl_RecheckBy.Items.Insert(0, new ListItem("---Select---", "0"));
            }
        }
        protected void grdApproveMistake_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewMisTake")
            {
                //ModalPopupExtender1.Hide();
                string MisTake = Convert.ToString(e.CommandArgument);
                string[] arg = new string[3];
                arg = MisTake.Split(';');
               
                lblMistakeId.Text = Convert.ToString(arg[0]);
                lblMistakeRectype.Text = Convert.ToString(arg[1]);
                lblMistakeRecNo.Text = Convert.ToString(arg[2]);
                var Mistakeres = dc.MistakeRegister_View( Convert.ToInt32(lblMistakeId.Text), "", "", null, null, false, false,false);
                foreach (var n in Mistakeres)
                {
                    ddl_MistakeFor.ClearSelection();
                    ddl_MistakeFor.Items.FindByText(n.MISTAKE_MistakeFor_var.ToString()).Selected = true;

                    ddl_TypeofMistake.ClearSelection();
                    ddl_TypeofMistake.Items.FindByText(n.MISTAKE_MistakeType_var.ToString()).Selected = true;

                    //ddl_DoneBy.ClearSelection();
                    //BindUserListDoneBy();
                    //ddl_DoneBy.Items.FindByText(n.MISTAKE_DoneBy_var).Selected = true;
                    txt_DoneBy.Text = n.MISTAKE_DoneBy_var;

                    ddl_DetectedBy.ClearSelection();
                    BindUserListDetectedBy();
                    ddl_DetectedBy.Items.FindByText(n.MISTAKE_DetectedBy_var).Selected = true;


                    txt_MistakeDate.Text = Convert.ToDateTime(n.MISTAKE_Date).ToString("dd/MM/yyyy");
                    txt_Description.Text = n.MISTAKE_Description_var.ToString();
                    txt_PreventiveMeasure.Text = n.MISTAKE_PreventiveMeasure_var.ToString();
                    txt_CorrectiveAction.Text = n.MISTAKE_CorrectiveAction_var.ToString();
                    txt_Reason.Text = n.MISTAKE_Reason_var.ToString();

                    LoadInwarDType();
                    ddl_InwardType_DT.SelectedValue = n.MISTAKE_RecordType_var.ToString();
                }

                var MistakeDetail = dc.MistakeRegister_View(Convert.ToInt32(lblMistakeId.Text), lblMistakeRectype.Text, "", null, null, false, false, false);
                grdMistake.DataSource = MistakeDetail;
                grdMistake.DataBind();
                grdMistake.Visible = true;
                lnkSave.Text = "Approve";
                int i = 0;
                var MisTakeD = dc.MistakeRegister_View(Convert.ToInt32(lblMistakeId.Text), lblMistakeRectype.Text, "", null, null, false, false, false);
                foreach (var grd in MisTakeD)
                {
                    CheckBox cbxSelect = (CheckBox)grdMistake.Rows[i].Cells[0].FindControl("cbxSelect");
                    Label lbl_Number = (Label)grdMistake.Rows[i].Cells[1].FindControl("lbl_Number");
                    lbl_Number.Text = grd.MISTAKEDETAIL_RefNo_var.ToString();
                    cbxSelect.Checked = true;
                    i++;
                }
                checkApprove();

            }
        }

        private void BindRefNoforRecheck()
        {
            if (txt_FromDate.Text != "" && txt_ToDate.Text != "")
            {
                DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
                DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
                string recordType = ddl_InwardType_DT.SelectedValue;
                if (recordType == "MF" )
                {
                    ddlMF.Visible = true;
                    if (ddlMF.SelectedValue=="Final")
                        recordType = "MF_Final";
                }
                var MisTake = dc.getRecheckReport(recordType, Fromdate, Todate);
                grdMistake.DataSource = MisTake;
                grdMistake.DataBind();
                grdMistake.Visible = true;
                int i = 0;
                //var aa = dc.getRecheckReport(ddl_InwardType_DT.SelectedValue,Fromdate, Todate);
                var aa = dc.getRecheckReport(recordType, Fromdate, Todate);
                foreach (var grd in aa)
                {
                    Label lbl_Number = (Label)grdMistake.Rows[i].Cells[1].FindControl("lbl_Number");
                    if (ddl_InwardType_DT.SelectedValue == "AGGT")
                    {
                        lbl_Number.Text = grd.AGGTINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "AAC")
                    {
                        lbl_Number.Text = grd.AACINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "BT-")
                    {
                        lbl_Number.Text = grd.BTINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "CCH")
                    {
                        lbl_Number.Text = grd.CCHINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "CEMT")
                    {
                        lbl_Number.Text = grd.CEMTINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "CORECUT")
                    {
                        lbl_Number.Text = grd.CORECUTINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "CR")
                    {
                        lbl_Number.Text = grd.CRINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "CT")
                    {
                        lbl_Number.Text = grd.CTINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "FLYASH")
                    {
                        lbl_Number.Text = grd.FLYASHINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "GGBS")
                    {
                        lbl_Number.Text = grd.GGBSINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "GGBSCH")
                    {
                        lbl_Number.Text = grd.GGBSCHINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "GT")
                    {
                        lbl_Number.Text = grd.GTINW_RefNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "MF")
                    {
                        lbl_Number.Text = grd.MFINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "NDT")
                    {
                        lbl_Number.Text = grd.NDTINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "PILE")
                    {
                        lbl_Number.Text = grd.PILEINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "PT")
                    {
                        lbl_Number.Text = grd.PTINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "SO")
                    {
                        lbl_Number.Text = grd.SOINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "SOLID")
                    {
                        lbl_Number.Text = grd.SOLIDINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "ST")
                    {
                        lbl_Number.Text = grd.STINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "STC")
                    {
                        lbl_Number.Text = grd.STCINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "TILE")
                    {
                        lbl_Number.Text = grd.TILEINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "WT")
                    {
                        lbl_Number.Text = grd.WTINWD_ReferenceNo_var.ToString();
                    }
                    else if (ddl_InwardType_DT.SelectedValue == "OT")
                    {
                        lbl_Number.Text = grd.OTINWD_ReferenceNo_var.ToString();
                    }
                    i++;
                }
            }
        }

        public void BindRecordForModifyInward()
        {
            if (txt_FromDate.Text != "" && txt_ToDate.Text != "")
            {
                DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
                DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);

                //var MisTake = dc.MistakeRegister_View(0, "", ddl_InwardType_DT.SelectedItem.Text, Fromdate, Todate, false, false).ToList();
                var MisTake = dc.Inward_View_RecordNoForMistake(ddl_InwardType_DT.SelectedItem.Text, Fromdate, Todate).ToList();
                grdMistake.DataSource = MisTake;
                grdMistake.DataBind();
                grdMistake.Visible = true;
                int i = 0;
                //var aa = dc.MistakeRegister_View(0, "", ddl_InwardType_DT.SelectedItem.Text, Fromdate, Todate, false, false);
                foreach (var grd in MisTake)
                {
                    Label lbl_Number = (Label)grdMistake.Rows[i].Cells[1].FindControl("lbl_Number");
                    //lbl_Number.Text = grd.INWD_RecordNo_int.ToString();
                    lbl_Number.Text = grd.RecordNo.ToString();
                    i++;
                }
            }
        }
        public void BindApproveGrid()
        {
            if (txt_FromDate.Text != "" && txt_ToDate.Text != "")
            {
                DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
                DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);

                var App = dc.MistakeRegister_View(0, "", "", Fromdate, Todate, false, false, false);
                grdApproveMistake.DataSource = App;
                grdApproveMistake.DataBind();
            }
        }
        protected void ddl_InwardType_DT_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }
        protected void LoadRecords()
        {
            if (ddl_InwardType_DT.SelectedItem.Text != "DT")
            {
                if (ddl_MistakeFor.SelectedItem.Text == "Recheck Report")
                {
                    BindRefNoforRecheck();                    
                }
                else
                {
                    BindRecordForModifyInward();
                }
                checkApprove();
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
               // Label lblMsg = (Label)Master.FindControl("lblMsg");
                DateTime MistakeDate = DateTime.ParseExact(txt_MistakeDate.Text, "dd/MM/yyyy", null);
                int RecordNo = 0;
                int MistakeId = 0;
                var res = dc.MistakeRegister_View(0, "", "", null, null, false, true, false);
                foreach (var n in res)
                {
                    MistakeId = Convert.ToInt32(n.MISTAKE_Id_int);
                }
                bool mfFinalStatus = false;
                if (ddl_InwardType_DT.SelectedValue == "MF" && ddlMF.SelectedValue == "Final")
                {
                    mfFinalStatus = true;
                }
                if (RdnApproveMIstake.Checked)
                {
                    dc.MistakeDetailRegister_Update(Convert.ToInt32(lblMistakeId.Text), lblMistakeRectype.Text.ToString(), true, ddl_InwardType_DT.SelectedValue.ToString(), 0, true);
                    dc.MistakeRegister_Update(Convert.ToInt32(lblMistakeId.Text), MistakeDate, ddl_InwardType_DT.SelectedValue.ToString(), Convert.ToInt32(lblMistakeRecNo.Text), ddl_TypeofMistake.SelectedItem.Text, ddl_MistakeFor.SelectedItem.Text, txt_DoneBy.Text, ddl_DetectedBy.SelectedItem.Text, txt_Description.Text, txt_CorrectiveAction.Text, txt_PreventiveMeasure.Text, txt_Reason.Text, 0, Convert.ToInt32(Session["LoginId"]), mfFinalStatus);
                    for (int i = 0; i <= grdMistake.Rows.Count - 1; i++)
                    {
                        CheckBox cbxSelect = (CheckBox)grdMistake.Rows[i].Cells[0].FindControl("cbxSelect");
                        Label lbl_Number = (Label)grdMistake.Rows[i].Cells[1].FindControl("lbl_Number");
                        DropDownList ddl_RecheckBy = (DropDownList)grdMistake.Rows[i].Cells[2].FindControl("ddl_RecheckBy");
                        if (cbxSelect.Checked)
                        {
                            dc.MistakeDetailRegister_Update(MistakeId, lbl_Number.Text, true, ddl_InwardType_DT.SelectedValue.ToString(), Convert.ToInt32(ddl_RecheckBy.SelectedValue), false);
                            if (ddl_MistakeFor.SelectedItem.Text == "Modify Inward")
                            {
                                dc.Inwd_update("", 0, ddl_InwardType_DT.SelectedValue, Convert.ToInt32(lbl_Number.Text));
                            }
                            else if (ddl_MistakeFor.SelectedItem.Text == "Recheck Report")
                            {
                                string recordType = ddl_InwardType_DT.SelectedValue;
                                if (mfFinalStatus == true)
                                    recordType = "MF_Final";

                                dc.Inward_Update_SupersedeDetail(lbl_Number.Text, recordType, chkAddSupersedeNote.Checked);
                                dc.Inwd_update(lbl_Number.Text, 2, recordType, 0);
                            }
                            lnkSave.Text = "Save";
                            pnlMistake.Visible = false;
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Record Approved Sucessfully');", true);
                            //lblMsg.Text = "Record Approved Successfully";
                            //lblMsg.Visible = true;
                            //lblMsg.ForeColor = System.Drawing.Color.Green;
                           
                            RdnApproveMIstake.Checked = false;
                            RdnNewMistake.Checked = true;
                        }
                    }
                }
                else
                {
                    MistakeId++;
                    dc.MistakeRegister_Update(0, MistakeDate, ddl_InwardType_DT.SelectedValue.ToString(), RecordNo, ddl_TypeofMistake.SelectedItem.Text, ddl_MistakeFor.SelectedItem.Text, txt_DoneBy.Text, ddl_DetectedBy.SelectedItem.Text, txt_Description.Text, txt_CorrectiveAction.Text, txt_PreventiveMeasure.Text, txt_Reason.Text, Convert.ToInt32(Session["LoginId"]), 0, mfFinalStatus);
                    for (int i = 0; i <= grdMistake.Rows.Count - 1; i++)
                    {
                        CheckBox cbxSelect = (CheckBox)grdMistake.Rows[i].Cells[0].FindControl("cbxSelect");
                        Label lbl_Number = (Label)grdMistake.Rows[i].Cells[1].FindControl("lbl_Number");
                        if (cbxSelect.Checked)
                        {
                            dc.MistakeDetailRegister_Update(MistakeId, lbl_Number.Text, false, ddl_InwardType_DT.SelectedValue.ToString(), 0, false);
                            pnlMistake.Visible = false;

                            //lblMsg.Text = "Record Saved Successfully";
                            //lblMsg.Visible = true;
                            //lblMsg.ForeColor = System.Drawing.Color.Green;
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Record Saved Sucessfully');", true);
                            lnkSave.Enabled = false;
                        }
                    }
                }
                cleartext();
            }
        }
        protected Boolean ValidateData()
        {
            //Label lblMsg = (Label)Master.FindControl("lblMsg");
            string dispalyMsg = "";
            Boolean valid = true;

            if (ddl_MistakeFor.SelectedItem.Text == "---Select---")
            {
               dispalyMsg = " Please Select the Mistake For";
                valid = false;
            }
            else if (ddl_TypeofMistake.SelectedItem.Text == "---Select---")
            {
               dispalyMsg = " Please Select the Mistake Type";
                valid = false;
            }
            //else if (ddl_DoneBy.SelectedItem.Text == "---Select---")
            //{
            //   dispalyMsg = " Please Select the Done By";
            //    valid = false;
            //}
            else if (txt_DoneBy.Text == "")
            {
                dispalyMsg = " Please Enter the Done By";
                valid = false;
            }
            else if (ddl_DetectedBy.SelectedItem.Text == "---Select---")
            {
               dispalyMsg = " Please Select the Detected By";
                valid = false;
            }
            else if (txt_MistakeDate.Text == "")
            {
               dispalyMsg = " Please Enter the Date";
                valid = false;
                txt_MistakeDate.Focus();
            }
            else if (txt_Description.Text == "")
            {
               dispalyMsg = " Please Enter Description";
                valid = false;
                txt_Description.Focus();
            }
            else if (txt_Reason.Text == "")
            {
               dispalyMsg = " Please Enter Reason";
                valid = false;
                txt_Reason.Focus();
            }
            else if (txt_PreventiveMeasure.Text == "")
            {
               dispalyMsg = " Please Enter Preventive Measure";
                valid = false;
                txt_PreventiveMeasure.Focus();
            }
            else if (txt_CorrectiveAction.Text == "")
            {
               dispalyMsg = " Please Enter Corrective Action";
                valid = false;
                txt_CorrectiveAction.Focus();
            }
            else if (ddl_InwardType_DT.SelectedItem.Text == "---Select---")
            {
               dispalyMsg = " Please Select the Reccord Type ";
                valid = false;
            }
            //else if (Convert.ToDateTime(txt_MistakeDate.Text) > DateTime.Now.Date)
            //{
            //    dispalyMsg = " Date should be less than or equal to the Current Date";
            //    valid = false;

            //}
            else if (grdMistake.Rows.Count > 0)
            {
                bool chk = false;
                for (int i = 0; i <= grdMistake.Rows.Count - 1; i++)
                {
                    CheckBox cbxSelect = (CheckBox)grdMistake.Rows[i].Cells[0].FindControl("cbxSelect");
                    if (cbxSelect.Checked)
                    {
                        chk = true;
                    }
                }
                if (chk == false)
                {
                   dispalyMsg = "Please Select at least one CheckBox";
                    valid = false;
                }
            }
            else if (grdMistake.Rows.Count <= 0)
            {
               dispalyMsg = "There are no Records !";
                valid = false;
            }

            if (valid == true)
            {
                for (int i = 0; i < grdMistake.Rows.Count; i++)
                {
                    CheckBox cbxSelect = (CheckBox)grdMistake.Rows[i].Cells[0].FindControl("cbxSelect");
                    DropDownList ddl_RecheckBy = (DropDownList)grdMistake.Rows[i].Cells[2].FindControl("ddl_RecheckBy");
                    if (grdMistake.Columns[2].Visible == true)
                    {
                        if (cbxSelect.Checked)
                        {

                            if (ddl_RecheckBy.SelectedItem.Text == "---Select---")
                            {
                               dispalyMsg = "Please Select Recheck By User for row  " + (i + 1) + ".";
                                valid = false;
                                break;
                            }
                        }
                    }
                }
            }
            if (valid == false)
            {
                pnlMistake.Visible = false;
               // lblMsg.Visible = true;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + dispalyMsg + "');", true);
            }
            else
            {
                //lblMsg.Visible = false;
            }
            return valid;
        }
        //public void BindUserListDoneBy()
        //{
        //    var allUser = dc.User_View(0, -1, "", "", "");
        //    ddl_DoneBy.DataSource = allUser;
        //    ddl_DoneBy.DataTextField = "USER_Name_var";
        //    ddl_DoneBy.DataValueField = "USER_Id";
        //    ddl_DoneBy.DataBind();
        //    ddl_DoneBy.Items.Insert(0, new ListItem("---Select---", "0"));
        //    ddl_DoneBy.Items.Add("Other");
        //}
        public void BindUserListDetectedBy()
        {
            var allUser = dc.User_View(0, -1, "", "", "");
            ddl_DetectedBy.DataSource = allUser;
            ddl_DetectedBy.DataTextField = "USER_Name_var";
            ddl_DetectedBy.DataValueField = "USER_Id";
            ddl_DetectedBy.DataBind();
            ddl_DetectedBy.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_DetectedBy.Items.Add("Client");
            ddl_DetectedBy.Items.Add("Other");
        }

        private void LoadInwarDType()
        {
            var inwd = dc.Material_View("", "");
            ddl_InwardType_DT.DataSource = inwd;
            ddl_InwardType_DT.DataTextField = "MATERIAL_Name_var";
            ddl_InwardType_DT.DataValueField = "MATERIAL_RecordType_var";
            ddl_InwardType_DT.DataBind();
            ddl_InwardType_DT.Items.Insert(0, "---Select---");
        }

        private void LoadDT()
        {
            ddl_InwardType_DT.Items.Clear();
            ddl_InwardType_DT.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_InwardType_DT.Items.Add("DT");
        }

        protected void ddl_MistakeFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdMistake.DataSource = null;
            grdMistake.DataBind();
            lnkSave.Enabled = true;
            if (ddl_MistakeFor.SelectedItem.Text == "Modify Inward" || ddl_MistakeFor.SelectedItem.Text == "Recheck Report")
            {
                LoadInwarDType();
            }
            if (ddl_MistakeFor.SelectedItem.Text == "Modify Bill")
            {
                LoadDT();
            }
            if (ddl_MistakeFor.SelectedItem.Text == "Other")
            {
                ddl_InwardType_DT.Items.Clear();
                ddl_InwardType_DT.Items.Insert(0, new ListItem("---Select---", "0"));
            }
        }

        //protected void ddl_DoneBy_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddl_DoneBy.SelectedItem.Text == "Other")
        //    {
        //        //txt_DoneByOther.Visible = true;
        //    }
        //    else
        //    {
        //        // txt_DoneByOther.Visible = false;
        //    }
        //}
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void txt_FromDate_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        protected void txt_ToDate_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        protected void lnkViewList_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
            pnlMistake.Visible = true;
            lblFromSearch.Visible = true;
            txtFromSearch.Visible = true;
            lblToSearch.Visible = true;
            txtToSearch.Visible = true;
            optPending.Visible = true;
            optApproved.Visible = true;
            lnkFetchList.Visible = true;
            optPending.Checked = true;
            lblApproveMistake.Text = "List of Mistake";
            txtFromSearch.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtToSearch.Text = DateTime.Today.ToString("dd/MM/yyyy");
            //BindApproveGrid();
            LoadMistakeList();
        }
        public void LoadMistakeList()
        {
            if (txtFromSearch.Text != "" && txtToSearch.Text != "")
            {
                DateTime Fromdate = DateTime.ParseExact(txtFromSearch.Text, "dd/MM/yyyy", null);
                DateTime Todate = DateTime.ParseExact(txtToSearch.Text, "dd/MM/yyyy", null);
                
                var App = dc.MistakeRegister_View(0, "", "", Fromdate, Todate, false, false, optApproved.Checked);
                grdApproveMistake.DataSource = App;
                grdApproveMistake.DataBind();

                for (int i = 0; i < grdApproveMistake.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdApproveMistake.Rows[i].FindControl("chkSelect");
                    chkSelect.Visible = false;
                }
            }
        }
        protected void lnkFetchList_Click(object sender, EventArgs e)
        {
            LoadMistakeList();
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {   
            CheckBox chkSelect = (CheckBox)sender;
            if (chkSelect.Checked == true)
            {
                GridViewRow row = (GridViewRow)chkSelect.NamingContainer;                
                int rowindex = row.RowIndex;
                for (int i = 0; i < grdApproveMistake.Rows.Count; i++)
                { 
                    if (i != rowindex)
                    {
                        CheckBox chkSelectRow = (CheckBox)grdApproveMistake.Rows[i].FindControl("chkSelect");
                        chkSelectRow.Checked = false;
                    }
                }
            }
        }

        protected void LoadSelectedMistake()
        {
            bool selFlag = false;
            for (int rowNo = 0; rowNo < grdApproveMistake.Rows.Count; rowNo++)
            {
                CheckBox chkSelectRow = (CheckBox)grdApproveMistake.Rows[rowNo].FindControl("chkSelect");
                if (chkSelectRow.Checked == true)
                {
                    selFlag = true;
                    lblMistakeId.Text = grdApproveMistake.Rows[rowNo].Cells[1].Text;
                    lblMistakeRectype.Text = grdApproveMistake.Rows[rowNo].Cells[3].Text;
                    lblMistakeRecNo.Text = grdApproveMistake.Rows[rowNo].Cells[4].Text;

                    var Mistakeres = dc.MistakeRegister_View(Convert.ToInt32(lblMistakeId.Text), "", "", null, null, false, false, false);
                    foreach (var n in Mistakeres)
                    {
                        ddl_MistakeFor.ClearSelection();
                        ddl_MistakeFor.Items.FindByText(n.MISTAKE_MistakeFor_var.ToString()).Selected = true;

                        ddl_TypeofMistake.ClearSelection();
                        ddl_TypeofMistake.Items.FindByText(n.MISTAKE_MistakeType_var.ToString()).Selected = true;

                        //ddl_DoneBy.ClearSelection();
                        //BindUserListDoneBy();
                        //ddl_DoneBy.Items.FindByText(n.MISTAKE_DoneBy_var).Selected = true;
                        txt_DoneBy.Text = n.MISTAKE_DoneBy_var;

                        ddl_DetectedBy.ClearSelection();
                        BindUserListDetectedBy();
                        ddl_DetectedBy.Items.FindByText(n.MISTAKE_DetectedBy_var).Selected = true;

                        txt_MistakeDate.Text = Convert.ToDateTime(n.MISTAKE_Date).ToString("dd/MM/yyyy");
                        txt_Description.Text = n.MISTAKE_Description_var.ToString();
                        txt_PreventiveMeasure.Text = n.MISTAKE_PreventiveMeasure_var.ToString();
                        txt_CorrectiveAction.Text = n.MISTAKE_CorrectiveAction_var.ToString();
                        txt_Reason.Text = n.MISTAKE_Reason_var.ToString();

                        LoadInwarDType();
                        ddl_InwardType_DT.SelectedValue = n.MISTAKE_RecordType_var.ToString();
                        if (n.MISTAKE_MFFinalStatus_bit == true)
                            ddlMF.SelectedValue = "Final";
                        else
                            ddlMF.SelectedValue = "MDL";
                    }

                    var MistakeDetail = dc.MistakeRegister_View(Convert.ToInt32(lblMistakeId.Text), lblMistakeRectype.Text, "", null, null, false, false, false);
                    grdMistake.DataSource = MistakeDetail;
                    grdMistake.DataBind();
                    grdMistake.Visible = true;
                    lnkSave.Text = "Approve";
                    int i = 0;
                    var MisTakeD = dc.MistakeRegister_View(Convert.ToInt32(lblMistakeId.Text), lblMistakeRectype.Text, "", null, null, false, false, false);
                    foreach (var grd in MisTakeD)
                    {
                        CheckBox cbxSelect = (CheckBox)grdMistake.Rows[i].Cells[0].FindControl("cbxSelect");
                        Label lbl_Number = (Label)grdMistake.Rows[i].Cells[1].FindControl("lbl_Number");
                        lbl_Number.Text = grd.MISTAKEDETAIL_RefNo_var.ToString();
                        cbxSelect.Checked = true;
                        i++;
                    }
                    checkApprove();
                    if (ddl_MistakeFor.SelectedValue == "Recheck Report")
                    {
                        chkAddSupersedeNote.Checked = false;
                        chkAddSupersedeNote.Visible = true;
                    }
                    else
                    {
                        chkAddSupersedeNote.Checked = true;
                        chkAddSupersedeNote.Visible = false;
                    }
                }
            }
            if (selFlag == false)
            {
                RdnNewMistake.Checked = true;
                RdnApproveMIstake.Checked = false;
            }
        }

        protected void ddlMF_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        

    }
}
