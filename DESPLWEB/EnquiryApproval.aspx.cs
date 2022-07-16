using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

namespace DESPLWEB
{
    public partial class EnquiryApproval : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Enquiry Approval";
                txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                //optPending.Checked = true;
                optCRPending.Enabled = false;
                optPending.Enabled = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_CRLimitApprove_right_bit  == true)
                    {
                        optCRPending.Enabled = true;
                    }
                    if (u.USER_EnqApprove_right_bit == true)
                    {
                        optPending.Enabled = true;
                    }
                }
                if (optPending.Enabled == true)
                {
                    optPending.Checked = true;
                }
                else if (optCRPending.Enabled == true)
                {
                    optCRPending.Checked = true;
                }
                else
                {
                    optApproved.Checked = true;
                }
            }
        }
        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)grdEnquiry.HeaderRow.FindControl("chkSelectAll");
            foreach (GridViewRow row in grdEnquiry.Rows)
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

        protected void optPending_CheckedChanged(object sender, EventArgs e)
        {
            grdEnquiry.DataSource = null;
            grdEnquiry.DataBind();
            chkSearchByDate.Text = "Enquiry Date";
        }

        protected void optApproved_CheckedChanged(object sender, EventArgs e)
        {
            grdEnquiry.DataSource = null;
            grdEnquiry.DataBind();
            chkSearchByDate.Text = "Approve Date";
        }

        protected void chkSearchByDate_CheckedChanged(object sender, EventArgs e)
        {
            grdEnquiry.DataSource = null;
            grdEnquiry.DataBind();
        }

        protected void lnkFetch_Click(object sender, EventArgs e)
        {
            LoadEnquiryList();
        }
        protected void LoadEnquiryList()
        {
            DateTime? SearchByDate = null;
            int enqStatus = 0;
            if (optPending.Checked == true)
            {
                lnkApprove.Text = "Approve";
                enqStatus = 5;
            }
            else if (optCRPending.Checked == true)
            {
                lnkApprove.Text = "Approve";
                enqStatus = 6;
            }
            else if (optApproved.Checked == true)
            {
                lnkApprove.Text = "Save";
                enqStatus = 0;
            }
            if (chkSearchByDate.Checked == true)
            {
                //SearchByDate =Convert.ToDateTime(txtDate.Text);
                SearchByDate = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null);
            }
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("EnquiryDate",typeof(string)));
            dt.Columns.Add(new DataColumn("ExpectedDate", typeof(string)));
            dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt.Columns.Add(new DataColumn("SiteName", typeof(string)));
            dt.Columns.Add(new DataColumn("EnquiryFor", typeof(string)));
            dt.Columns.Add(new DataColumn("CollectionDate", typeof(string)));
            dt.Columns.Add(new DataColumn("txtEditedCollectionDate", typeof(string)));
            dt.Columns.Add(new DataColumn("Location", typeof(string)));
            dt.Columns.Add(new DataColumn("Route", typeof(string)));
            dt.Columns.Add(new DataColumn("Limit", typeof(string)));
            dt.Columns.Add(new DataColumn("BalanceAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("EnquiryNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Comment", typeof(string)));
            dt.Columns.Add(new DataColumn("RouteId", typeof(string)));
            dt.Columns.Add(new DataColumn("LocationId", typeof(string)));
            dt.Columns.Add(new DataColumn("DriverId", typeof(string)));
            dt.Columns.Add(new DataColumn("VehicleId", typeof(string)));
            dt.Columns.Add(new DataColumn("ContactPerson", typeof(string)));
            dt.Columns.Add(new DataColumn("ContactNo", typeof(string)));
            dt.Columns.Add(new DataColumn("PaymentMode", typeof(string)));
            dt.Columns.Add(new DataColumn("CarryForwardDate", typeof(string)));
            dt.Columns.Add(new DataColumn("txtReasonForApprove", typeof(string)));
            dt.Columns.Add(new DataColumn("lblEnquiryStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("ME", typeof(string)));
            dt.Columns.Add(new DataColumn("Note", typeof(string)));
            dt.Columns.Add(new DataColumn("ContactNoForApproval", typeof(string)));

            DateTime tempDate = DateTime.Now;
            var enquiry = dc.Enquiry_View_Approval(enqStatus, SearchByDate);
            foreach (var enq in enquiry)
            {
                dr = dt.NewRow();
                dr["EnquiryDate"] = enq.ENQ_Date_dt.ToString("dd/MM/yyyy");
                if (enq.ENQ_ClientExpectedDate_dt != null)
                {
                    tempDate = Convert.ToDateTime(enq.ENQ_ClientExpectedDate_dt);
                    dr["ExpectedDate"] = tempDate.ToString("dd/MM/yyyy");
                }
                
                dr["ClientName"] = enq.CL_Name_var;
                dr["SiteName"] = enq.SITE_Name_var;
                dr["EnquiryFor"] = enq.MATERIAL_Name_var;   
                if (enq.ENQ_CollectionDate_dt != null)
                {
                    tempDate = Convert.ToDateTime(enq.ENQ_CollectionDate_dt);
                    dr["CollectionDate"] = tempDate.ToString("dd/MM/yyyy");
                }
                if (enq.ENQ_ModifiedCollectionDate_dt != null)
                {
                    tempDate = Convert.ToDateTime(enq.ENQ_ModifiedCollectionDate_dt);
                    dr["txtEditedCollectionDate"] = tempDate.ToString("dd/MM/yyyy");
                }                
                dr["Location"] = enq.Location_Name;
                dr["Route"] = enq.Route_Name;
                dr["Limit"] = enq.CL_Limit_mny;
                dr["BalanceAmount"] = enq.CL_BalanceAmt_mny;
                dr["EnquiryNo"] = enq.ENQ_Id;
                dr["Comment"] = enq.ENQ_Comment_var;
                dr["RouteId"] = enq.ENQ_ROUTE_Id;
                dr["LocationId"] = enq.ENQ_LOCATION_Id;
                dr["DriverId"] = enq.ENQ_DriverId;
                //dr["VehicleId"] = enq.Vehicle_Id;
                dr["ContactPerson"] = enq.CONT_Name_var;
                dr["ContactNo"] = enq.CONT_ContactNo_var;
                dr["PaymentMode"] = enq.ENQ_PaymentMode_var;
                if (enq.ENQ_Carryforword_dt != null)
                {
                    tempDate = Convert.ToDateTime(enq.ENQ_Carryforword_dt);
                    dr["CarryForwardDate"] = tempDate.ToString("dd/MM/yyyy");
                }                
                dr["txtReasonForApprove"] = enq.ENQ_UnlockNote_var;
                dr["lblEnquiryStatus"] = enq.ENQ_OpenEnquiryStatus_var;
                dr["ME"] = enq.ME_Name;
                dr["Note"] = enq.ENQ_Note_var;
                if (enq.ENQ_CRLimitApprStatus_tint == 0) //RJ
                {
                    dr["ContactNoForApproval"] = "9011094931";
                }
                else if (enq.ENQ_CRLimitApprStatus_tint == 1) //SK
                {
                    dr["ContactNoForApproval"] = "9921398231";
                }
                else if (enq.ENQ_CRLimitApprStatus_tint == 3) //AJ
                {
                    dr["ContactNoForApproval"] = "9011016464";
                }
                dt.Rows.Add(dr);
            }
            grdEnquiry.DataSource = dt;
            grdEnquiry.DataBind();
            lblRecords.Text = "Total No of Records : " + grdEnquiry.Rows.Count;
        }

        protected void grdEnquiry_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[25].Text == "9011094931")
                {
                    e.Row.Cells[3].BackColor = System.Drawing.Color.LightGreen ;
                }
                else if (e.Row.Cells[25].Text == "9921398231")
                {
                    e.Row.Cells[3].BackColor = System.Drawing.Color.LightYellow;
                }
                else if (e.Row.Cells[25].Text == "9011016464")
                {
                    e.Row.Cells[3].BackColor = System.Drawing.Color.LightPink;
                }
            }
        }

        protected void lnkApprove_Click(object sender, EventArgs e)
        {
            bool selectedFlag = false;
            bool validate = true;
            for (int i = 0; i < grdEnquiry.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdEnquiry.Rows[i].FindControl("chkSelect");
                TextBox txtReasonForApprove = (TextBox)grdEnquiry.Rows[i].FindControl("txtReasonForApprove");
                txtReasonForApprove.Text = txtReasonForApprove.Text.Trim();
                if (chkSelect.Checked == true)
                {
                    selectedFlag = true;
                    if (txtReasonForApprove.Text == "") //should not validate for amol joshi - do later
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Input Reason for approval');", true);
                        txtReasonForApprove.Focus();
                        validate = false;
                        break;
                    }
                }
            }
            if (selectedFlag == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select at least one enquiry');", true);
            }
            else if (validate == true)
            {
                DateTime? EditedCollectionDate = null;
                DateTime? ApproveDate = null;
                byte enqStatus = 0;
                for (int i = 0; i < grdEnquiry.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdEnquiry.Rows[i].FindControl("chkSelect");
                    TextBox txtReasonForApprove = (TextBox)grdEnquiry.Rows[i].FindControl("txtReasonForApprove");
                    TextBox txtEditedCollectionDate = (TextBox)grdEnquiry.Rows[i].FindControl("txtEditedCollectionDate");
                    Label lblEnquiryStatus = (Label)grdEnquiry.Rows[i].FindControl("lblEnquiryStatus");
                    
                    if (chkSelect.Checked == true)
                    {
                        EditedCollectionDate = null;
                        if (txtEditedCollectionDate.Text != "")
                            EditedCollectionDate = Convert.ToDateTime(txtEditedCollectionDate.Text);
                        if (optPending.Checked == true || optCRPending.Checked == true)
                            ApproveDate = DateTime.Now;

                        if (lblEnquiryStatus.Text == "To be Collected")
                            enqStatus = 0;
                        else
                            enqStatus = 1;
                        dc.Enquiry_Update_Approve(Convert.ToInt32(grdEnquiry.Rows[i].Cells[12].Text), enqStatus, txtReasonForApprove.Text, EditedCollectionDate, ApproveDate, Convert.ToInt32(Session["LoginId"]));
                        if (lblEnquiryStatus.Text == "To be Collected")
                        {
                            var enqDetail = dc.Enquiry_View_ForPickup(Convert.ToInt32(grdEnquiry.Rows[i].Cells[12].Text));
                            foreach (var enq in enqDetail)
                            {
                                dc.PickUpAllocation_Update(Convert.ToInt32(grdEnquiry.Rows[i].Cells[12].Text), enq.CL_Id, enq.SITE_Id, enq.LOCATION_Id, enq.ENQ_ROUTE_Id, enq.ENQ_CollectionDate_dt, enq.CONT_Name_var, enq.unUsedCoupon.ToString(), enq.ME_Name, enq.CONT_ContactNo_var, enq.Driver_id, enq.MATERIAL_Name_var, Convert.ToInt32(enq.ENQ_Quantity), enq.ENQ_ContactNoForCollection_var, enq.ENQ_ContactPersonForCollection_var, false);
                            }
                        }
                    }
                }
                if (optPending.Checked == true || optCRPending.Checked == true)
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Approved successfully.');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Saved successfully.');", true);

                LoadEnquiryList();
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdEnquiry.Rows.Count > 0)
            {
                string Subheader = "";
                if (optPending.Checked ==true)
                    Subheader = "Enquiry List Pending for Approval";
                else if (optPending.Checked == true)
                    Subheader = "Enquiry List Pending for CR Limit Approval";
                else
                    Subheader = "All Approved Enquiry List";
                if (chkSearchByDate.Checked == true)
                {
                    Subheader = Subheader + " for date " + txtDate.Text;
                }
                PrintHTMLReport rpt = new PrintHTMLReport();                
                string grddata = "";
                string grdColumn = "";
                string grdview = "";
                for (int j = 0; j < grdEnquiry.Columns.Count; j++)
                {
                    if (j == 1 || j == 3 || j == 4 || j == 18 || j == 19 || j == 5 || j == 11 || j == 10)
                    {
                        grdColumn += grdEnquiry.Columns[j].HeaderText + "|";
                    }
                }
                for (int i = 0; i < grdEnquiry.Rows.Count; i++)
                {
                    grddata += "$";
                    for (int c = 0; c < grdEnquiry.Rows[i].Cells.Count; c++)
                    {
                        if (c == 1 || c == 3 || c == 4 || c == 18 || c == 19 || c == 5 || c == 11 || c == 10)
                        {
                            grddata += grdEnquiry.Rows[i].Cells[c].Text + "~";
                        }
                    }
                }
                grdview = grdColumn + grddata;
                rpt.RptHTMLgrid("Enquiry List", Subheader, grdview);
            }
        }
    }
}