using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ComplaintList : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Complaint list";
                txt_FromDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");


            }
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadComplaintList();
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdComplaint.Rows.Count > 0)
            {
                PrintGrid.ComplaintRegisterPrintGridView(grdComplaint, "Complaint Register List", "ComplaintRegisterList");
            }

        }
        protected void lnkModify_OnCommand(object sender, CommandEventArgs e)
        {
            string compNo = e.CommandArgument.ToString();
            if (compNo != "")
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;
                Label lblComplaintStatus = (Label)grdComplaint.Rows[index].FindControl("lblComplaintStatus");
                if (lblComplaintStatus.Text == "Open")
                {
                    EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                    string strURLWithData = "ComplaintRegister.aspx?" + obj.Encrypt(string.Format("COMP_Id={0}", Convert.ToString(compNo)));
                    Response.Redirect(strURLWithData);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('You can not modify Close Complaint!!');", true);
               
                }
            }

        }

       private void LoadComplaintList()
        {
            BindgrdRowOfComplaint();
            if (grdComplaint.Rows.Count <= 0)
            {
                FirstGridViewRowOfEnquiry();
            }
            else
            {
                lblTotalRecords.Text = "Total No of Records : " + grdComplaint.Rows.Count;

            }
        }
        private void BindgrdRowOfComplaint()
        {
            //int status = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("COMP_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("Complaint_Status", typeof(string)));
            dt.Columns.Add(new DataColumn("Complaint_Type", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_DetailsOfComplaint_var", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_AttendedBy", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_RecordType", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_ActionIntiated", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_ClouserDate", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_CommentOfTechOfficer", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_ActionBy_var", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_CreatedBy_var", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_ReviewBy_var", typeof(string)));
          
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);

            var enq = dc.ComplaintRegister_View(Fromdate, Todate,0);
            foreach (var c in enq)
            {
                
                    dr = dt.NewRow();
                    dr["COMP_Id"] = c.COMP_Id.ToString();
                    dr["CL_Id"] = c.COMP_CL_Id.ToString();
                    dr["SITE_Id"] = c.COMP_SITE_Id.ToString();
                    dr["COMP_Date_dt"] = Convert.ToDateTime(c.COMP_date_dt).ToString("dd/MM/yyyy");
                    dr["CL_Name_var"] = Convert.ToString(c.CL_Name_var);
                    dr["SITE_Name_var"] = Convert.ToString(c.SITE_Name_var);
                    if (Convert.ToBoolean(c.COMP_Status_bit)==false)
                         dr["Complaint_Status"] ="Open";
                    else
                        dr["Complaint_Status"] = "Close";

                    if (Convert.ToInt32(c.COMP_ComplaintType_int) == 1)
                        dr["Complaint_Type"] = "Service (Collection & Report)";
                    else if (Convert.ToInt32(c.COMP_ComplaintType_int) == 2)
                        dr["Complaint_Type"] = "Technical";
                    else if (Convert.ToInt32(c.COMP_ComplaintType_int) == 3)
                        dr["Complaint_Type"] = "Marketing";
                    else if (Convert.ToInt32(c.COMP_ComplaintType_int) == 4)
                        dr["Complaint_Type"] = "Testing";
                    else if (Convert.ToInt32(c.COMP_ComplaintType_int) == 5)
                        dr["Complaint_Type"] = "Billing";
                    else if (Convert.ToInt32(c.COMP_ComplaintType_int) == 6)
                        dr["Complaint_Type"] = "Response";
                    else if (Convert.ToInt32(c.COMP_ComplaintType_int) == 7)
                        dr["Complaint_Type"] = "Other";

                    dr["COMP_DetailsOfComplaint_var"] = Convert.ToString(c.COMP_DetailsOfComplaint_var);
                    dr["COMP_AttendedBy"] = Convert.ToString(c.Comp_Attended_By);
                    dr["COMP_RecordType"] = Convert.ToString(c.MATERIAL_Name_var);
                    dr["COMP_ActionIntiated"] = Convert.ToString(c.COMP_CorrAction_var);
                    dr["COMP_ClouserDate"] = Convert.ToDateTime(c.COMP_ClosuerDate_dt).ToString("dd/MM/yyyy"); 
                    dr["COMP_CommentOfTechOfficer"] = Convert.ToString(c.COMP_TechOfficerComment_var);
                    dr["COMP_ActionBy_var"] = Convert.ToString(c.Comp_Action_By);
                    dr["COMP_CreatedBy_var"] = Convert.ToString(c.Comp_Created_By);
                    if (Convert.ToBoolean(c.COMP_ReviewdByMD_bit) == true)
                        dr["COMP_ReviewBy_var"] = "Yes";
                    else
                        dr["COMP_ReviewBy_var"] = "No";
                    dt.Rows.Add(dr);
             
            }
            grdComplaint.DataSource = dt;
            grdComplaint.DataBind();

            
        }

        private void FirstGridViewRowOfEnquiry()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("COMP_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_Date_dt", typeof(string)));
             dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("Complaint_Status", typeof(string)));
            dt.Columns.Add(new DataColumn("Complaint_Type", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_DetailsOfComplaint_var", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_AttendedBy", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_RecordType", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_ActionIntiated", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_ClouserDate", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_CommentOfTechOfficer", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_ActionBy_var", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_CreatedBy_var", typeof(string)));
            dt.Columns.Add(new DataColumn("COMP_ReviewBy_var", typeof(string)));
            dr = dt.NewRow();
            dr["COMP_Id"] = string.Empty;
            dr["CL_Id"] = string.Empty;
            dr["SITE_Id"] = string.Empty;
            dr["COMP_Date_dt"] = string.Empty;
            dr["CL_Name_var"] = string.Empty;
            dr["SITE_Name_var"] = string.Empty;
            dr["Complaint_Status"] = string.Empty;
            dr["Complaint_Type"] = string.Empty;
            dr["COMP_DetailsOfComplaint_var"] = string.Empty;
            dr["COMP_AttendedBy"] = string.Empty;
            dr["COMP_RecordType"] = string.Empty;
            dr["COMP_ActionIntiated"] = string.Empty;
            dr["COMP_ClouserDate"] = string.Empty;
            dr["COMP_CommentOfTechOfficer"] = string.Empty;
            dr["COMP_ActionBy_var"] = string.Empty;
            dr["COMP_CreatedBy_var"] = string.Empty;
            dr["COMP_ReviewBy_var"] = string.Empty;
            dt.Rows.Add(dr);
            grdComplaint.DataSource = dt;
            grdComplaint.DataBind();

        }

      
    }
}