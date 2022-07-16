using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ReportSiteVisitLog : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Site Visit Log - Device";
                //bool superAdminRight = false;
                //var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                //foreach (var u in user)
                //{
                //    if (u.USER_SuperAdmin_right_bit == true)
                //        superAdminRight = true;
                //}
                //if (superAdminRight == true)
                //{
                txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                LoadMEList();
                //}
                //else
                //{
                //    pnlContent.Visible = false;
                //    lblAccess.Visible = true;
                //    lblAccess.Text = "Access is Denied.. ";
                //}
            }
        }

        private void LoadMEList()
        {
            var data = dc.User_View_ME(-1).ToList();
            ddl_ME.DataSource = data;
            ddl_ME.DataTextField = "USER_Name_var";
            ddl_ME.DataValueField = "USER_Id";
            ddl_ME.DataBind();
            ddl_ME.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            LoadReportList();
        }

        private void LoadReportList()
        {
            DataTable dt = new DataTable();
            DataRow dr1 = null;

            dt.Columns.Add(new DataColumn("Sr_No", typeof(string)));
            dt.Columns.Add(new DataColumn("USER_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("date", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("in_time", typeof(string)));
            dt.Columns.Add(new DataColumn("out_time", typeof(string)));
            dt.Columns.Add(new DataColumn("contact_person", typeof(string)));
            dt.Columns.Add(new DataColumn("contact_no", typeof(string)));
            dt.Columns.Add(new DataColumn("designation", typeof(string)));
            dt.Columns.Add(new DataColumn("Lead_discription", typeof(string)));
            dt.Columns.Add(new DataColumn("Response", typeof(string)));
            dt.Columns.Add(new DataColumn("Next_date", typeof(string)));
            dt.Columns.Add(new DataColumn("Site_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Address_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_BulidUpArea_dec", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_RERA_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_ConstPeriod_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_ComplitionDate_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_RccConsltnt_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Architect_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_ConstMangmt_var", typeof(string)));
            dt.Columns.Add(new DataColumn("OutSourceName", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_GeoInvstgn_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_BldgsUnderConst_int", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_WorkStatus_ForRCC", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_WorkStatus_ForBWP", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_WorkStatus_ForFinishesh", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_ProposedBldgs_int", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_StartDate_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_CompletedBldgs_int", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_CurrTestingDetails_var", typeof(string)));
            int usrId = 0; // , SiteId = 0, ClientId = 0;
          
            if (ddl_ME.SelectedItem.Text != "")
                usrId = Convert.ToInt32(ddl_ME.SelectedValue);

            //if (chkClientSpecific.Checked == true)
            //{
            //    if (lblClientId.Text != "" && lblClientId.Text != "0")
            //        ClientId = Convert.ToInt32(lblClientId.Text);

            //    if (lblSiteId.Text != "" && lblSiteId.Text != "0")
            //        SiteId = Convert.ToInt32(lblSiteId.Text);
            //}

            int rowNo = 1;
            DateTime Fromdate = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null);
            var saleslog = dc.SalesLog_View(Fromdate, Todate, usrId);
            foreach (var log in saleslog)
            {
                dr1 = dt.NewRow();
                dr1["Sr_No"] = rowNo;
                dr1["USER_Name_var"] = log.USER_Name_var;
                dr1["date"] = log.date;
                dr1["CL_Name_var"] = log.CL_Name_var;
                dr1["SITE_Name_var"] = log.SITE_Name_var;
                dr1["in_time"] = log.in_time;
                dr1["out_time"] = log.out_time;
                dr1["contact_person"] = log.contact_person;
                dr1["contact_no"] = log.contact_no;
                dr1["designation"] = log.designation;
                dr1["Lead_discription"] = log.Lead_discription;
                dr1["Response"] = log.Response;
                dr1["Next_date"] = log.Next_date;
                dr1["Site_Id"] = log.Site_Id;
                var kycDetails = dc.SiteKYC_View(Convert.ToInt32(log.Site_Id), -1, 1).ToList();
                for (int i = 0; i < kycDetails.Count; i++)
                {

                    dr1["SITE_Address_var"] = kycDetails[i].SITE_Address_var;
                    dr1["SITE_BulidUpArea_dec"] = kycDetails[i].SITE_BulidUpArea_dec;
                    dr1["SITE_RERA_var"] = kycDetails[i].SITE_ReRa_var;
                    dr1["SITE_ConstPeriod_var"] = kycDetails[i].SITE_ConstPeriod_var;
                    if (kycDetails[i].SITE_ComplitionDate_dt.ToString() != "")
                        dr1["SITE_ComplitionDate_dt"] = Convert.ToDateTime(kycDetails[i].SITE_ComplitionDate_dt).ToString("dd/MM/yyyy");
                    dr1["SITE_RccConsltnt_var"] = kycDetails[i].SITE_RccConsltnt_var;
                    dr1["SITE_Architect_var"] = kycDetails[i].SITE_Architect_var;
                    dr1["SITE_ConstMangmt_var"] = kycDetails[i].SITE_ConstMangmt_var;
                    dr1["SITE_GeoInvstgn_var"] = kycDetails[i].SITE_GeoInvstgn_var;
                    dr1["SITE_BldgsUnderConst_int"] = kycDetails[i].SITE_BldgsUnderConst_int;
                    dr1["SITE_ProposedBldgs_int"] = kycDetails[i].proposed_buildings;
                    if (kycDetails[i].proposed_start_date.ToString() != "")
                        dr1["SITE_StartDate_dt"] = Convert.ToDateTime(kycDetails[i].proposed_start_date).ToString("dd/MM/yyyy");
                    dr1["SITE_CompletedBldgs_int"] = kycDetails[i].no_of_buildings;
                    dr1["SITE_WorkStatus_ForRCC"] = kycDetails[i].RCC_status;
                    dr1["SITE_WorkStatus_ForBWP"] = kycDetails[i].block_work_plaster_status;
                    dr1["SITE_WorkStatus_ForFinishesh"] = kycDetails[i].finishes_status;
                    var sitemateriallab = dc.SiteMaterialLabDtl_View(Convert.ToInt32(log.Site_Id)).ToList();
                    if (sitemateriallab.Count > 0)
                    {

                        for (int j = 0; j < sitemateriallab.Count; j++)
                        {
                            if (j > 0)
                            {
                                dt.Rows.Add(dr1);
                                dr1 = dt.NewRow();
                               
                            }
                            dr1["SITE_CurrTestingDetails_var"] = sitemateriallab[j].MATERIAL_Name_var + "-" + sitemateriallab[j].lab_name;
                        }
                    }
                    else if (kycDetails[i].SITE_CurrTestingDetails_var != "" && kycDetails[i].SITE_CurrTestingDetails_var != null)
                    {
                        string testingDetails = "";
                        string[] currentTesting = kycDetails[i].SITE_CurrTestingDetails_var.Split('~');
                        for (int j = 0; j < currentTesting.Length; j++)
                        {
                            string[] value = currentTesting[j].Split('/');
                            testingDetails += value[0] + "-" + value[1];
                            if (j > 0)
                            {
                                dt.Rows.Add(dr1);
                                dr1 = dt.NewRow();

                            }
                            dr1["SITE_CurrTestingDetails_var"] = value[0] + "-" + value[1];

                        }
                    }

                }
                dt.Rows.Add(dr1);
                rowNo++;
            }
            grdReport.DataSource = dt;
            grdReport.DataBind();
            FilterGridByClientSiteName();
       }
        private void FilterGridByClientSiteName()
        {
            if (chkClientSpecific.Checked == true)
            {
                DataTable dtt = new DataTable();
                if (grdReport.Rows.Count > 0)
                {
                    dtt.Columns.Add(new DataColumn("Sr_No", typeof(string)));
                    dtt.Columns.Add(new DataColumn("USER_Name_var", typeof(string)));
                    dtt.Columns.Add(new DataColumn("date", typeof(string)));
                    dtt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
                    dtt.Columns.Add(new DataColumn("in_time", typeof(string)));
                    dtt.Columns.Add(new DataColumn("out_time", typeof(string)));
                    dtt.Columns.Add(new DataColumn("contact_person", typeof(string)));
                    dtt.Columns.Add(new DataColumn("contact_no", typeof(string)));
                    dtt.Columns.Add(new DataColumn("designation", typeof(string)));
                    dtt.Columns.Add(new DataColumn("Lead_discription", typeof(string)));
                    dtt.Columns.Add(new DataColumn("Response", typeof(string)));
                    dtt.Columns.Add(new DataColumn("Next_date", typeof(string)));
                    dtt.Columns.Add(new DataColumn("Site_Id", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_Address_var", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_BulidUpArea_dec", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_RERA_var", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_ConstPeriod_var", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_ComplitionDate_dt", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_RccConsltnt_var", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_Architect_var", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_ConstMangmt_var", typeof(string)));
                    dtt.Columns.Add(new DataColumn("OutSourceName", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_GeoInvstgn_var", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_BldgsUnderConst_int", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_WorkStatus_ForRCC", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_WorkStatus_ForBWP", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_WorkStatus_ForFinishesh", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_ProposedBldgs_int", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_StartDate_dt", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_CompletedBldgs_int", typeof(string)));
                    dtt.Columns.Add(new DataColumn("SITE_CurrTestingDetails_var", typeof(string)));

                    //for (int i = 0; i < grdReport.Columns.Count; i++)
                    //{
                    //    dtt.Columns.Add(grdReport.HeaderRow.Cells[i].Text.Replace(' ', '_'));
                    //}
                    foreach (GridViewRow row in grdReport.Rows)
                    {
                        Label lblSrNo = (Label)row.FindControl("lblSrNo");
                        Label lblMEName = (Label)row.FindControl("lblMEName");
                        TextBox lblSITE_Address_var = (TextBox)row.FindControl("lblSITE_Address_var");
                        TextBox lblCL_Name_var = (TextBox)row.FindControl("lblCL_Name_var");
                        TextBox lblSITE_Name_var = (TextBox)row.FindControl("lblSITE_Name_var");

                        DataRow dr = dtt.NewRow();
                        for (int j = 0; j < grdReport.Columns.Count; j++)
                        {

                            if (j == 0)
                            {
                                if (lblSrNo.Text != "" && lblSrNo.Text != "&nbsp;")
                                    dr["Sr_No"] = lblSrNo.Text.ToString().Trim();
                            }
                            else if (j == 1)
                            {
                                if (lblMEName.Text != "" && lblMEName.Text != "&nbsp;")
                                    dr["USER_Name_var"] = lblMEName.Text.ToString().Trim();
                            }
                            else if (j == 3)
                            {
                                if (lblCL_Name_var.Text != "" && lblCL_Name_var.Text != "&nbsp;")
                                    dr["CL_Name_var"] = lblCL_Name_var.Text.ToString().Trim();
                            }
                            else if (j == 4)
                            {
                                if (lblSITE_Name_var.Text != "" && lblSITE_Name_var.Text != "&nbsp;")
                                    dr["SITE_Name_var"] = lblSITE_Name_var.Text.ToString().Trim();
                            }
                            else if (j == 13)
                            {
                                if (lblSITE_Address_var.Text != "" && lblSITE_Address_var.Text != "&nbsp;")
                                    dr["SITE_Address_var"] = lblSITE_Address_var.Text.ToString().Trim();
                            }
                            else
                            {
                                if (row.Cells[j].Text.ToString().Trim() != "" && row.Cells[j].Text.ToString().Trim() != "&nbsp;")
                                    dr[j] = row.Cells[j].Text.ToString().Trim();
                                // dr[grdReport.HeaderRow.Cells[j].Text.Replace(' ', '_')] = row.Cells[j].Text;
                            }
                        }

                        dtt.Rows.Add(dr);
                    }

                    DataView dv;
                    if (lblSiteId.Text != "0" && lblClientId.Text != "0")
                        dv = new DataView(dtt, "CL_Name_var = '" + txt_Client.Text + "' and SITE_Name_var= '" + txt_Site.Text + "' ", "CL_Name_var Desc", DataViewRowState.CurrentRows);
                    else
                        dv = new DataView(dtt, "CL_Name_var = '" + txt_Client.Text + "' or SITE_Name_var= '" + txt_Site.Text + "' ", "CL_Name_var Desc", DataViewRowState.CurrentRows);

                    grdReport.DataSource = dv;
                    grdReport.DataBind();
                }
            }

            for (int i = 0; i < grdReport.Rows.Count; i++)
            {
                Label lblSrNo = (Label)grdReport.Rows[i].FindControl("lblSrNo");
                TextBox lblSITE_Address_var = (TextBox)grdReport.Rows[i].FindControl("lblSITE_Address_var");
                TextBox lblCL_Name_var = (TextBox)grdReport.Rows[i].FindControl("lblCL_Name_var");
                TextBox lblSITE_Name_var = (TextBox)grdReport.Rows[i].FindControl("lblSITE_Name_var");

                if (lblSrNo.Text == "")
                {
                    lblCL_Name_var.Visible = false;
                    lblSITE_Address_var.Visible = false;
                    lblSITE_Name_var.Visible = false;
                }

            }
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdReport.Rows.Count > 0 && grdReport.Visible == true)
            {
                string Subheader = "";

                Subheader = lblFromDate.Text + "\t" + txtFromDate.Text + "~" + lblToDate.Text + "\t" + txtToDate.Text;
                //if (ddlME.SelectedIndex > 0)
                //    Subheader += "~" + "Marketing Person" + "\t" + ddlME.SelectedItem.Text;

                var fileName = "SiteVisitLog" + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss");
                PrintGrid.PrintGridViewSiteVisitLog(grdReport, Subheader, fileName);

            }
        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            grdReport.DataSource = null;
            grdReport.DataBind();
        }

        protected void ddl_ME_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdReport.DataSource = null;
            grdReport.DataBind();
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
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetSitename(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            DataRow row = null;
            DataTable dt = new DataTable();
            if (Convert.ToInt32(HttpContext.Current.Session["CL_ID"]) > 0)
            {
                int ClientId = 0;
                if (int.TryParse(HttpContext.Current.Session["CL_ID"].ToString(), out ClientId))
                {

                    var res = db.Site_View(0, Convert.ToInt32(HttpContext.Current.Session["CL_ID"]), 0, searchHead);
                    dt.Columns.Add("SITE_Name_var");
                    foreach (var obj in res)
                    {
                        row = dt.NewRow();
                        string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.SITE_Name_var, obj.SITE_Id.ToString());
                        dt.Rows.Add(listitem);
                    }
                    if (row == null)
                    {
                        var resclnm = db.Site_View(0, Convert.ToInt32(HttpContext.Current.Session["CL_ID"]), 0, "");
                        foreach (var obj in resclnm)
                        {
                            row = dt.NewRow();
                            string listitem = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(obj.SITE_Name_var, obj.SITE_Id.ToString());
                            dt.Rows.Add(listitem);
                        }
                    }
                }
            }
            List<string> SITE_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SITE_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return SITE_Name_var;
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
        protected Boolean ChkSiteName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Site.Text;
            Boolean valid = false;
            var res = dc.Site_View(0, Convert.ToInt32(lblClientId.Text), 0, searchHead);
            foreach (var obj in res)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Site.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Site Name is not in the list ";
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }
        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    Session["CL_ID"] = Convert.ToInt32(Request.Form[hfClientId.UniqueID]);
                    lblClientId.Text = Request.Form[hfClientId.UniqueID];
                    FilterGridByClientSiteName();
                }
                else
                {
                    Session["CL_ID"] = "0";
                    lblClientId.Text = "0";
                }
             //   txt_Site.Focus();
               
            }

        }
        protected void txt_Site_TextChanged(object sender, EventArgs e)
        {
            int cl_Id = 0;
            if (Convert.ToInt32(lblClientId.Text) > 0)
            {
                if (int.TryParse(lblClientId.Text.ToString(), out cl_Id))
                {
                    if (ChkSiteName(txt_Site.Text) == true)
                    {
                        int SiteId = 0;
                        if (int.TryParse(Request.Form[hfSiteId.UniqueID], out SiteId))
                        {
                            Session["SITE_ID"] = Convert.ToInt32(Request.Form[hfSiteId.UniqueID]);
                             lblSiteId.Text =  Convert.ToInt32(Request.Form[hfSiteId.UniqueID]).ToString();
                             FilterGridByClientSiteName();
                        }
                        else
                        {
                            Session["SITE_ID"] = 0;
                            lblSiteId.Text = "0";
                        }
                    }
                }
            }
        }

        protected void chkClientSpecific_CheckedChanged(object sender, EventArgs e)
        {
            txt_Client.Text = "";
            txt_Site.Text = "";
            if (chkClientSpecific.Checked)
            {
                txt_Client.Visible = true;
                lblSiteNm.Visible = true;
                txt_Site.Visible = true;
            }
            else
            {
                txt_Client.Visible = false;
                lblSiteNm.Visible = false;
                txt_Site.Visible = false;
                LoadReportList();
            }
        }
     
    }
}