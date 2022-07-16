using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ReportDelayList : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "List of delay reports";
                LoadReportList();
            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
                
        private void LoadReportList()
        {
            int userId = Convert.ToInt32(Session["LoginId"]);
            if (Session["LoginUserName"].ToString().ToLower().Contains("vishwas") ||
                Session["LoginUserName"].ToString().ToLower().Contains("ujwal") ||
                 Session["LoginUserName"].ToString().ToLower().Contains("sampada") ||
                 Session["LoginUserName"].ToString().ToLower().Contains("sandeep"))
            {
                userId = 0;
            }
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ALERTDLRPT_RecordType_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ALERTDLRPT_ReferenceNo_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ALERTDLRPT_RecordNo_int", typeof(string)));
            dt.Columns.Add(new DataColumn("ALERTDLRPT_ReceivedDate_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("ALERTDLRPT_TargetDate_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("ALERTDLRPT_DelayInDays_int", typeof(string)));
            dt.Columns.Add(new DataColumn("ALERTDLRPT_CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ALERTDLRPT_SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ALERTDLRPT_ReportStatus_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ALERTDLRPT_Reason_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ALERTDLRPT_Pending_bit", typeof(string)));

            string strPrevRecType = "";
            int srNo = 1;
            var report = dc.AlertDelayReportList_View(userId).ToList();
            foreach (var c in report)
            {
                if (strPrevRecType != "" && strPrevRecType != c.ALERTDLRPT_RecordType_var)
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    srNo = 1;
                }
                dr = dt.NewRow();
                dr["SrNo"] = srNo.ToString();
                dr["ALERTDLRPT_RecordType_var"] = c.ALERTDLRPT_RecordType_var.ToString();
                dr["ALERTDLRPT_ReferenceNo_var"] = c.ALERTDLRPT_ReferenceNo_var.ToString();
                dr["ALERTDLRPT_RecordNo_int"] = c.ALERTDLRPT_RecordNo_int.ToString();
                dr["ALERTDLRPT_ReceivedDate_dt"] = Convert.ToDateTime(c.ALERTDLRPT_ReceivedDate_dt).ToString("dd/MM/yyyy");
                dr["ALERTDLRPT_TargetDate_dt"] = Convert.ToDateTime(c.ALERTDLRPT_TargetDate_dt).ToString("dd/MM/yyyy");
                dr["ALERTDLRPT_DelayInDays_int"] = c.ALERTDLRPT_DelayInDays_int.ToString();
                dr["ALERTDLRPT_CL_Name_var"] = c.ALERTDLRPT_CL_Name_var.ToString();
                dr["ALERTDLRPT_SITE_Name_var"] = c.ALERTDLRPT_SITE_Name_var.ToString();
                dr["ALERTDLRPT_ReportStatus_var"] = c.ALERTDLRPT_ReportStatus_var.ToString();
                dr["ALERTDLRPT_Reason_var"] = c.ALERTDLRPT_Reason_var.ToString();
                dr["ALERTDLRPT_Pending_bit"] = c.ALERTDLRPT_Pending_bit.ToString();
                dt.Rows.Add(dr);
                strPrevRecType = c.ALERTDLRPT_RecordType_var;
                srNo++;
            }
            grdReports.DataSource = dt;
            grdReports.DataBind();
            ViewState["ReportTable"] = dt;
            lblTotalRecords.Text = "Total Records : " + report.Count();
            for (int i = 0; i < grdReports.Rows.Count; i++)
            {
                if (grdReports.Rows[i].Cells[0].Text == "" || grdReports.Rows[i].Cells[0].Text == "&nbsp;")
                {
                    //merge row
                    grdReports.Rows[i].BackColor = System.Drawing.Color.LightBlue;
                }
                else if (Convert.ToInt32(grdReports.Rows[i].Cells[6].Text) == 0)
                {
                    grdReports.Rows[i].ForeColor = System.Drawing.Color.DarkGreen;
                    grdReports.Rows[i].BackColor = System.Drawing.Color.White;
                }
                else if (Convert.ToInt32(grdReports.Rows[i].Cells[6].Text) == 1)
                {
                    grdReports.Rows[i].ForeColor = System.Drawing.Color.DarkBlue;
                    grdReports.Rows[i].BackColor = System.Drawing.Color.White;
                }
                else if (Convert.ToInt32(grdReports.Rows[i].Cells[6].Text) >= 2)
                {
                    grdReports.Rows[i].ForeColor = System.Drawing.Color.Red;
                    grdReports.Rows[i].BackColor = System.Drawing.Color.WhiteSmoke;
                }
                
            }
        }
        protected void grdReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strBillNo = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "UpdateReason")
            {
                GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int rowindex = grdrow.RowIndex;
                TextBox txtReason = (TextBox)grdReports.Rows[rowindex].FindControl("txtReason");

                if (txtReason.Text.Trim() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Input Reason..');", true);
                    txtReason.Focus();
                }
                else
                {   
                    dc.ReportDelayReason_Update(grdReports.Rows[rowindex].Cells[1].Text, grdReports.Rows[rowindex].Cells[2].Text, txtReason.Text, Convert.ToInt32(Session["LoginId"].ToString()));
                    LoadReportList();
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Updated Successfully..');", true);
                }
            }
        }
        protected void lnkFetch_Click(object sender, EventArgs e)
        {
            LoadReportList();
        }
                
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdReports.Rows.Count > 0)
                PrintGrid.PrintGridView(grdReports, "List of delay reports", "DelayReport");
        }

        protected void chkPending_CheckedChanged(object sender, EventArgs e)
        {
            int totalVisibleCount = 0;
            for (int i = 0; i < grdReports.Rows.Count; i++)
            {
                Label lblPending = (Label)grdReports.Rows[i].FindControl("lblPending");
                if (lblPending.Text != "")
                {                    
                    if (chkPending.Checked == false || (chkPending.Checked == true && lblPending.Text == "False"))
                    {
                        grdReports.Rows[i].Visible = true;
                        totalVisibleCount++;
                    }
                    else
                    {
                        grdReports.Rows[i].Visible = false;
                    }
                }

            }
            lblTotalRecords.Text = "Total Records : " + totalVisibleCount;
        }
    }
}