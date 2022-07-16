using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace DESPLWEB
{
    public partial class OutwardReport : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUserName.Text = Session["LoginUserName"].ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Outward Report";
                LoadInwardType();
                LoadUserList();
                txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                optOutwardPending.Checked = true;
                lblOutwardBy.Visible = true;
                txtOutwardBy.Visible = true;
                lblOutwardTo.Visible = true;
                ddlUser.Visible = true;
                lblOutwardDate.Visible = true;
                txtOutwardDate.Visible = true;
                lnkOutwardReport.Visible = true;
                txtOutwardBy.Text = lblUserName.Text;
            }
        }
        private void LoadUserList()
        {
            ddlUser.DataTextField = "USER_Name_var";
            ddlUser.DataValueField = "USER_Id";
            //var user = dc.User_View(0, -1, "", "", "");
            var user = dc.Driver_View(true);
            ddlUser.DataSource = user;
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, "---Select---");
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        private void LoadInwardType()
        {
            var inwd = dc.Material_View("", "");
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardTestType.DataValueField = "MATERIAL_RecordType_var";
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, new ListItem("---All---", ""));
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdReports.Visible = true;
            DisplayReports();
        }

        public void DisplayReports()
        {
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);

            int ClientId = 0;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }
            //string finalStatus = "";
            //if (ddl_InwardTestType.SelectedItem.Text == "Mix Design")
            //{
            //    finalStatus = " " + ddlMF.SelectedValue;
            //}
            byte rptStatus = 4;
            if (optOutwardPending.Checked == true)
                rptStatus = 5;
            else if (optOutwardCompleted.Checked == true)
                rptStatus = 6;
            else if (optPhysicalOutwardCompleted.Checked == true)
                rptStatus = 7;

            if (optOutwardPending.Checked == true)
            {
                grdReports.Columns[3].Visible = true;
                grdReports.Columns[4].Visible = false;
                grdReports.Columns[10].Visible = false;
                grdReports.Columns[7].Visible = false;
                grdReports.Columns[8].Visible = false;
                grdReports.Columns[9].Visible = false;
            }


           // var Inward = dc.ReportStatus_View_All(ddl_InwardTestType.SelectedItem.Text + finalStatus, Fromdate, Todate, rptStatus, ClientId).ToList();
            var Inward = dc.MISDetail_View_Outward(ddl_InwardTestType.SelectedValue, Fromdate, Todate, rptStatus, ClientId, ddlMF.SelectedValue).ToList();
            grdReports.DataSource = Inward;
            grdReports.DataBind();


            lbl_RecordsNo.Text = "Total Records   :  " + grdReports.Rows.Count;
            if (optOutwardPending.Checked == true)
            {
                if (grdReports.Rows.Count > 0)
                {
                    DropDownList ddlOTFilter = (DropDownList)grdReports.HeaderRow.FindControl("ddlOTFilter");
                    ddlOTFilter.Visible = false;
                }
            }
            else if (optOutwardCompleted.Checked == true)
            {
                if (grdReports.Rows.Count > 0)
                {
                    DropDownList ddlOTFilter = (DropDownList)grdReports.HeaderRow.FindControl("ddlOTFilter");
                    ddlOTFilter.Visible = true;

                    getFilterData();
                }
                grdReports.Columns[3].Visible = false;
                grdReports.Columns[4].Visible = true;
                grdReports.Columns[7].Visible = true;
                grdReports.Columns[8].Visible = true;
                grdReports.Columns[9].Visible = true;
                grdReports.Columns[10].Visible = true;
            }
            else if (optPhysicalOutwardCompleted.Checked == true)
            {
                if (grdReports.Rows.Count > 0)
                {
                    DropDownList ddlOTFilter = (DropDownList)grdReports.HeaderRow.FindControl("ddlOTFilter");
                    ddlOTFilter.Visible = false;
                }
                grdReports.Columns[3].Visible = false;
                grdReports.Columns[2].Visible = true;
                grdReports.Columns[9].Visible = true;
                grdReports.Columns[6].Visible = true;
                grdReports.Columns[7].Visible = true;
                grdReports.Columns[8].Visible = true;
            }
        }
        protected void grdReportStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    DropDownList ddlOTFilter = (DropDownList)e.Row.FindControl("ddlOTFilter");
            //    getFilterData(ddlOTFilter);
            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkOutwardReport = (LinkButton)e.Row.FindControl("lnkOutwardReport");
                TextBox txtOutwardBy = (TextBox)e.Row.FindControl("txtOutwardBy");
                TextBox txtOutwardTo = (TextBox)e.Row.FindControl("txtOutwardTo");
                TextBox txtOutwardDate = (TextBox)e.Row.FindControl("txtOutwardDate");
                TextBox txtRegisterNo = (TextBox)e.Row.FindControl("txtRegisterNo");
                TextBox txtDeliveredTo = (TextBox)e.Row.FindControl("txtDeliveredTo");
                TextBox txtDeliveredDate = (TextBox)e.Row.FindControl("txtDeliveredDate");
                TextBox txtRemark = (TextBox)e.Row.FindControl("txtRemark");

                if (optOutwardPending.Checked == true)
                {
                    lnkOutwardReport.Text = "Outward";
                }
                else if (optOutwardCompleted.Checked == true)
                {
                    txtOutwardBy.ReadOnly = true;
                    txtOutwardTo.ReadOnly = true;
                    txtOutwardDate.ReadOnly = true;
                    lnkOutwardReport.Text = "Phy-Outward";
                }
                else if (optPhysicalOutwardCompleted.Checked == true)
                {
                    txtOutwardBy.ReadOnly = true;
                    txtOutwardTo.ReadOnly = true;
                    txtOutwardDate.ReadOnly = true;
                    txtRegisterNo.ReadOnly = true;
                    txtDeliveredTo.ReadOnly = true;
                    txtDeliveredDate.ReadOnly = true;
                    txtRemark.ReadOnly = true;
                    txtOutwardTo.ReadOnly = true;
                    txtOutwardDate.ReadOnly = true;
                    lnkOutwardReport.Visible = false;
                }
            }
        }
        protected void grdReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strReportDetails = Convert.ToString(e.CommandArgument);
            string[] arg = new string[4];
            arg = strReportDetails.Split(';');

            string Recordtype = Convert.ToString(arg[0]);
            int RecordNo = Convert.ToInt32(arg[1]);
            string ReferenceNo = Convert.ToString(arg[2]);
            string SetOfRecord = Convert.ToString(arg[3]);
            string testType = Convert.ToString(arg[4]);
            string BillNo = Convert.ToString(arg[5]);
            string tempRecType = Recordtype;
            if (testType == "Final")
            {
                tempRecType = tempRecType + " " + testType;
            }

            GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = grdrow.RowIndex;
            TextBox txtOutwardBy = (TextBox)grdReports.Rows[rowindex].FindControl("txtOutwardBy");
            TextBox txt_OutwardTo = (TextBox)grdReports.Rows[rowindex].FindControl("txtOutwardTo");
            TextBox txt_OutwardDate = (TextBox)grdReports.Rows[rowindex].FindControl("txtOutwardDate");
            TextBox txtRegisterNo = (TextBox)grdReports.Rows[rowindex].FindControl("txtRegisterNo");
            TextBox txtDeliveredTo = (TextBox)grdReports.Rows[rowindex].FindControl("txtDeliveredTo");
            TextBox txtDeliveredDate = (TextBox)grdReports.Rows[rowindex].FindControl("txtDeliveredDate");
            TextBox txtRemark = (TextBox)grdReports.Rows[rowindex].FindControl("txtRemark");
            string outwordTo = "", outwordDate = "";
            bool updateFlag = true;
            if (optOutwardPending.Checked == true)
            {
                txtOutwardBy.Text = lblUserName.Text;
                outwordTo = txt_OutwardTo.Text;
                outwordDate = txt_OutwardDate.Text;


                if (txt_OutwardTo.Text.Trim() == "" && txt_OutwardDate.Text.Trim() == "")
                {

                    if (ddlUser.SelectedIndex != 0 && txtOutwardDate.Text != "")
                    {
                        outwordTo = ddlUser.SelectedItem.Text;
                        outwordDate = txtOutwardDate.Text;
                    }
                    else if (ddlUser.SelectedIndex == 0 || txtOutwardDate.Text == "")
                    {
                        if (ddlUser.SelectedIndex == 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select Outward To..');", true);
                            ddlUser.Focus();
                            updateFlag = false;

                        }
                        else if (txtOutwardDate.Text.Trim() == "")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Outward Date..');", true);
                            txtOutwardDate.Focus();
                            updateFlag = false;
                        }
                    }


                }
                else
                {
                    if (txt_OutwardTo.Text.Trim() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Outward To..');", true);
                        txt_OutwardTo.Focus();
                        updateFlag = false;
                    }
                    else if (txt_OutwardDate.Text.Trim() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Outward Date..');", true);
                        txt_OutwardDate.Focus();
                        updateFlag = false;
                    }
                }


                if (updateFlag == true)
                {
                    dc.MISDetail_Update(0, Recordtype, ReferenceNo, testType, null, false, false, false, false, true, false, false);
                    dc.ReportDetails_Update(tempRecType, ReferenceNo, 0, 0, 0, 0, false, "Outward By");
                    dc.Outward_Update(Recordtype, ReferenceNo, RecordNo, SetOfRecord, testType, txtOutwardBy.Text, outwordTo, DateTime.ParseExact(outwordDate, "dd/MM/yyyy", null), 0, "", null, "", false);
                    //update bill OutwardStatus
                    if (chkWithBill.Checked)
                    {
                        if (BillNo != "0" || BillNo.ToString() != "")
                        {
                            var rslt = dc.Bill_View(BillNo, 0, 0, Recordtype, RecordNo, false, false, null, null).ToList();
                            if (rslt.Count > 0)
                            {
                                int status = Convert.ToInt32(rslt.FirstOrDefault().BILL_OutwardStatus_bit);
                                if (status == 0)
                                {
                                    dc.Bill_Update_OutwardStatus(BillNo, true);
                                    dc.Outward_Update("DT", BillNo, 0, "", "", txtOutwardBy.Text, outwordTo, DateTime.ParseExact(outwordDate, "dd/MM/yyyy", null), 0, "", null, "", false);
                                }
                            }
                        }
                    }
                    //
                    DisplayReports();
                }
            }
            else if (optOutwardCompleted.Checked == true)
            {
                if (txtRegisterNo.Text.Trim() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Register No..');", true);
                    txtRegisterNo.Focus();
                    updateFlag = false;
                }
                else if (txtDeliveredTo.Text.Trim() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Delivered To..');", true);
                    txtDeliveredTo.Focus();
                    updateFlag = false;
                }
                else if (txtDeliveredDate.Text.Trim() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Delivered Date..');", true);
                    txtDeliveredDate.Focus();
                    updateFlag = false;
                }
                else if (txtRemark.Text.Trim() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Remark..');", true);
                    txtRemark.Focus();
                    updateFlag = false;
                }
                if (updateFlag == true)
                {
                    dc.MISDetail_Update(0, Recordtype, ReferenceNo, testType, null, false, false, false, false, false, true, false);
                    dc.ReportDetails_Update(tempRecType, ReferenceNo, 0, 0, 0, 0, false, "Phy Outward By");
                    dc.Outward_Update(Recordtype, ReferenceNo, RecordNo, SetOfRecord, testType, "", "", null, Convert.ToInt32(txtRegisterNo.Text), txtDeliveredTo.Text, DateTime.ParseExact(txtDeliveredDate.Text, "dd/MM/yyyy", null), txtRemark.Text, true);
                    DisplayReports();
                }
            }
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdReports.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("Set Of Record", typeof(string)));
                dt.Columns.Add(new DataColumn("Reference No", typeof(string)));
                dt.Columns.Add(new DataColumn("Outward By", typeof(string)));
                dt.Columns.Add(new DataColumn("Outward To", typeof(string)));
                dt.Columns.Add(new DataColumn("Outward Date", typeof(string)));
                dt.Columns.Add(new DataColumn("Register No", typeof(string)));
                dt.Columns.Add(new DataColumn("Delivered To", typeof(string)));
                dt.Columns.Add(new DataColumn("Delivered Date", typeof(string)));
                dt.Columns.Add(new DataColumn("Remark", typeof(string)));
                dt.Columns.Add(new DataColumn("MIS TestType", typeof(string)));
                dt.Columns.Add(new DataColumn("Record Detail", typeof(string)));
                for (int i = 0; i < grdReports.Rows.Count; i++)
                {
                    if (grdReports.Rows[i].Visible == true)
                    {
                        TextBox txtOutwardBy = (TextBox)grdReports.Rows[i].Cells[1].FindControl("txtOutwardBy");
                        TextBox txtOutwardTo = (TextBox)grdReports.Rows[i].Cells[1].FindControl("txtOutwardTo");
                        TextBox txtOutwardDate = (TextBox)grdReports.Rows[i].Cells[1].FindControl("txtOutwardDate");
                        TextBox txtRegisterNo = (TextBox)grdReports.Rows[i].Cells[1].FindControl("txtRegisterNo");
                        TextBox txtDeliveredTo = (TextBox)grdReports.Rows[i].Cells[1].FindControl("txtDeliveredTo");
                        TextBox txtDeliveredDate = (TextBox)grdReports.Rows[i].Cells[1].FindControl("txtDeliveredDate");
                        TextBox txtRemark = (TextBox)grdReports.Rows[i].Cells[1].FindControl("txtRemark");
                        Label lblRecordDetail = (Label)grdReports.Rows[i].Cells[1].FindControl("lblRecordDetail");


                        dr = dt.NewRow();
                        dr["Set Of Record"] = grdReports.Rows[i].Cells[1].Text.ToString();
                        dr["Reference No"] = grdReports.Rows[i].Cells[2].Text.ToString();
                        dr["Outward By"] = txtOutwardBy.Text;
                        dr["Outward To"] = txtOutwardTo.Text;
                        dr["Outward Date"] = txtOutwardDate.Text;
                        dr["Register No"] = txtRegisterNo.Text;
                        dr["Delivered To"] = txtDeliveredTo.Text;
                        dr["Delivered Date"] = txtDeliveredDate.Text;
                        dr["Remark"] = txtRemark.Text;
                        dr["MIS TestType"] = grdReports.Rows[i].Cells[11].Text.ToString();
                        dr["Record Detail"] = lblRecordDetail.Text;

                        dt.Rows.Add(dr);
                    }
                }
                PrintGrid.PrintTimeReport(dt, "PhysicalOutwardPendingReport");
                //PrintGrid.OutwardPrintGridView(grdReports,"Physical Outward Pending Report","PhysicalOutwardPendingReport");
            }
        }

        protected void lnkOutwardReport_Click(object sender, EventArgs e)
        {
            bool selectedFlag = false;
            bool updateFlag = true;
            if (grdReports.Rows.Count > 0)
            {
                for (int i = 0; i < grdReports.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdReports.Rows[i].FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        selectedFlag = true;
                        break;
                    }
                }
            }
            if (selectedFlag == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select at least one row');", true);
                updateFlag = false;
            }
            else
            {
                if (optOutwardPending.Checked == true)
                {
                    txtOutwardBy.Text = lblUserName.Text;
                    if (ddlUser.SelectedIndex == -1 || ddlUser.SelectedIndex == 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select Outward To..');", true);
                        ddlUser.Focus();
                        updateFlag = false;
                    }
                    else if (txtOutwardDate.Text.Trim() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Outward Date..');", true);
                        txtOutwardDate.Focus();
                        updateFlag = false;
                    }
                }
                else if (optOutwardCompleted.Checked == true)
                {
                    if (txtRegisterNo.Text.Trim() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Register No..');", true);
                        txtRegisterNo.Focus();
                        updateFlag = false;
                    }
                    else if (txtDeliveredTo.Text.Trim() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Delivered To..');", true);
                        txtDeliveredTo.Focus();
                        updateFlag = false;
                    }
                    else if (txtDeliveredDate.Text.Trim() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Delivered Date..');", true);
                        txtDeliveredDate.Focus();
                        updateFlag = false;
                    }
                    else if (txtRemark.Text.Trim() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Remark..');", true);
                        txtRemark.Focus();
                        updateFlag = false;
                    }
                }
            }
            if (updateFlag == true)
            {
                for (int i = 0; i < grdReports.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdReports.Rows[i].FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        Label lblRecordDetail = (Label)grdReports.Rows[i].FindControl("lblRecordDetail");

                        string strReportDetails = lblRecordDetail.Text;
                        string[] arg = new string[5];
                        arg = strReportDetails.Split(';');

                        string Recordtype = Convert.ToString(arg[0]);
                        int RecordNo = Convert.ToInt32(arg[1]);
                        string ReferenceNo = Convert.ToString(arg[2]);
                        string SetOfRecord = Convert.ToString(arg[3]);
                        string BillNo = Convert.ToString(arg[5]);
                        //string testType = Recordtype;
                        //if (Recordtype == "MF")
                        //{
                        //    testType = ddlMF.SelectedValue;
                        //}
                        string testType = Convert.ToString(arg[4]);
                        string tempRecType = Recordtype;
                        if (testType == "Final")
                        {
                            tempRecType = tempRecType + " " + testType;
                        }

                        if (optOutwardPending.Checked == true)
                        {
                            dc.MISDetail_Update(0, Recordtype, ReferenceNo, testType, null, false, false, false, false, true, false, false);
                            dc.ReportDetails_Update(tempRecType, ReferenceNo, 0, 0, 0, 0, false, "Outward By");
                            dc.Outward_Update(Recordtype, ReferenceNo, RecordNo, SetOfRecord, testType, txtOutwardBy.Text, ddlUser.SelectedItem.Text, DateTime.ParseExact(txtOutwardDate.Text, "dd/MM/yyyy", null), 0, "", null, "", false);
                            //update bill OutwardStatus
                            if (chkWithBill.Checked)
                            {
                                if (BillNo != "0" || BillNo.ToString() != "")
                                {
                                    var rslt = dc.Bill_View(BillNo, 0, 0, Recordtype, RecordNo, false, false, null, null).ToList();
                                    if (rslt.Count > 0)
                                    {
                                        int status = Convert.ToInt32(rslt.FirstOrDefault().BILL_OutwardStatus_bit);
                                        if (status == 0)
                                        {
                                            dc.Bill_Update_OutwardStatus(BillNo, true);
                                            dc.Outward_Update("DT", BillNo, 0, "", "", txtOutwardBy.Text, ddlUser.SelectedItem.Text, DateTime.ParseExact(txtOutwardDate.Text, "dd/MM/yyyy", null), 0, "", null, "", false);
                                        }
                                    }
                                }
                            }
                           //

                        }
                        else if (optOutwardCompleted.Checked == true)
                        {
                            dc.MISDetail_Update(0, Recordtype, ReferenceNo, testType, null, false, false, false, false, false, true, false);
                            dc.ReportDetails_Update(tempRecType, ReferenceNo, 0, 0, 0, 0, false, "Phy Outward By");
                            dc.Outward_Update(Recordtype, ReferenceNo, RecordNo, SetOfRecord, testType, "", "", null, Convert.ToInt32(txtRegisterNo.Text), txtDeliveredTo.Text, DateTime.ParseExact(txtDeliveredDate.Text, "dd/MM/yyyy", null), txtRemark.Text, true);
                        }
                    }
                }
                DisplayReports();
            }
        }
        protected void ddl_InwardTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearReportList();
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design")
            {
                //lblMF.Visible = true;
                ddlMF.Visible = true;
            }
            else
            {
                //lblMF.Visible = false;
                ddlMF.Visible = false;
            }
        }

        private void ClearReportList()
        {
            grdReports.Visible = false;
            lbl_RecordsNo.Text = "";
            grdReports.DataSource = null;
            grdReports.DataBind();

        }

        protected void optOutwardPending_CheckedChanged(object sender, EventArgs e)
        {
            ClearReportList();
            lnkPrint.Visible = false;
            lblRegisterNo.Visible = false;
            txtRegisterNo.Visible = false;
            lblDeliveredTo.Visible = false;
            txtDeliveredTo.Visible = false;
            lblDeliveredDate.Visible = false;
            txtDeliveredDate.Visible = false;
            lblRemark.Visible = false;
            txtRemark.Visible = false;
            chkWithBill.Visible = true;
          
            lblOutwardBy.Visible = true;
            txtOutwardBy.Visible = true;
            lblOutwardTo.Visible = true;
            ddlUser.Visible = true;
            lblOutwardDate.Visible = true;
            txtOutwardDate.Visible = true;
            lnkOutwardReport.Visible = true;
            lnkOutwardReport.Text = "Outward";

        }

        protected void optOutwardCompleted_CheckedChanged(object sender, EventArgs e)
        {
            ClearReportList();
            lnkPrint.Visible = true;
            lblOutwardBy.Visible = false;
            txtOutwardBy.Visible = false;
            lblOutwardTo.Visible = false;
            ddlUser.Visible = false;
            lblOutwardDate.Visible = false;
            txtOutwardDate.Visible = false;
            chkWithBill.Visible = false;
            lblRegisterNo.Visible = true;
            txtRegisterNo.Visible = true;
            lblDeliveredTo.Visible = true;
            txtDeliveredTo.Visible = true;
            lblDeliveredDate.Visible = true;
            txtDeliveredDate.Visible = true;
            lblRemark.Visible = true;
            txtRemark.Visible = true;
            lnkOutwardReport.Visible = true;
            lnkOutwardReport.Text = "Phy-Outward";


        }

        private void getFilterData()
        {
            List<string> lstOutwardTo = new List<string>();
            foreach (GridViewRow row in grdReports.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtOutwardTo = row.FindControl("txtOutwardTo") as TextBox;
                    lstOutwardTo.Add(txtOutwardTo.Text);
                }
            }
            List<string> outwardTo = lstOutwardTo.Distinct().ToList();

            DropDownList ddlOTFilter = (DropDownList)grdReports.HeaderRow.FindControl("ddlOTFilter");
            ddlOTFilter.DataSource = outwardTo;
            ddlOTFilter.DataBind();
            ddlOTFilter.Items.Insert(0, new ListItem("---All---", ""));
        }

        protected void ddlOTFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            int cnt = 0;
            DropDownList ddlOTFilter = (DropDownList)sender;
            string filterName = ddlOTFilter.SelectedItem.Text;

            for (int i = 0; i < grdReports.Rows.Count; i++)
            {
                TextBox txtOutwardTo = (TextBox)grdReports.Rows[i].FindControl("txtOutwardTo");
                if (txtOutwardTo.Text.Equals(filterName) || filterName.Equals("---All---"))
                {
                    grdReports.Rows[i].Visible = true;
                    cnt++;
                }
                else if (!filterName.Equals("---All---"))
                    grdReports.Rows[i].Visible = false;
             
            }
            lbl_RecordsNo.Text = "Total Records   :  " + cnt;
        }

        protected void optPhysicalOutwardCompleted_CheckedChanged(object sender, EventArgs e)
        {
            ClearReportList();
            lnkPrint.Visible = false;
            lblOutwardBy.Visible = false;
            txtOutwardBy.Visible = false;
            lblOutwardTo.Visible = false;
            ddlUser.Visible = false;
            lblOutwardDate.Visible = false;
            chkWithBill.Visible = false;
            txtOutwardDate.Visible = false;
            lblRegisterNo.Visible = false;
            txtRegisterNo.Visible = false;
            lblDeliveredTo.Visible = false;
            txtDeliveredTo.Visible = false;
            lblDeliveredDate.Visible = false;
            txtDeliveredDate.Visible = false;
            lblRemark.Visible = false;
            txtRemark.Visible = false;
            lnkOutwardReport.Visible = false;
        }

        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    lblClientId.Text = Request.Form[hfClientId.UniqueID];
                }
                else
                {
                    lblClientId.Text = "0";
                }
            }
        }

        protected Boolean ChkClientName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Client.Text;
            Boolean valid = false;
            var query = dc.Client_View(0, 0, searchHead, "");
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Client.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Client is not in the list";
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetClientname(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Client_View(0, -1, searchHead, "");
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("CL_Name_var");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                var clnm = db.Client_View(0, 0, "", "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> CL_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CL_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return CL_Name_var;

        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)grdReports.HeaderRow.FindControl("chkSelectAll");
            foreach (GridViewRow row in grdReports.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkSelect");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }



    }

}
