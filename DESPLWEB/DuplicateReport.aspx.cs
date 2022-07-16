using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class DuplicateReport : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUserId.Text = Session["LoginId"].ToString();
                bool viewRight = false;
                var user = dc.User_View(Convert.ToInt32(lblUserId.Text), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_Print_right_bit == true)
                        viewRight = true;
                }
                if (viewRight == true)
                {
                    Label lblheading = (Label)Master.FindControl("lblheading");
                    lblheading.Text = "Duplicate Report";
                    LoadInwardType();
                    txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                    txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                }
                else
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        private void LoadInwardType()
        {
            var inwd = dc.Material_View("", "");
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardTestType.DataValueField = "MATERIAL_Id";
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, "---Select---");
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
            string finalStatus = "";
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design")
            {
                finalStatus = " " + ddlMF.SelectedValue;
            }
            byte rptStatus = 11;
            var Inward = dc.ReportStatus_View_All(ddl_InwardTestType.SelectedItem.Text + finalStatus, Fromdate, Todate, rptStatus, ClientId, 0, -1).ToList();
            grdReports.DataSource = Inward;
            grdReports.DataBind();
            lbl_RecordsNo.Text = "Total Records   :  " + grdReports.Rows.Count;
        }

        protected void grdReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strReportDetails = Convert.ToString(e.CommandArgument);
            string[] arg = new string[3];
            arg = strReportDetails.Split(';');

            string Recordtype = Convert.ToString(arg[0]);
            int RecordNo = Convert.ToInt32(arg[1]);
            string ReferenceNo = Convert.ToString(arg[2]);
            string mfType = "";
            if (ddl_InwardTestType.SelectedItem.Text == "Mix Design" && ddlMF.SelectedValue == "Final")
            {
                mfType = "Final";
            }
            string[] strRefNo = ReferenceNo.Split('/');
            //var chkBlockStatus = dc.Inward_View_ClientBlock(Convert.ToInt32(strRefNo[0])).ToList();
            var chkBalance = dc.Inward_View_ClientBalance(ReferenceNo, Recordtype + mfType);
            //if (chkBlockStatus.FirstOrDefault().CL_BlockStatus_bit == true)
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Client blocked, So can not display report.');", true);
            //}
            if (chkBalance.Count() > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Client is blocked as per credit policy, can not view report.');", true);
            }
            else
            {
                //PrintSelectedReport(Recordtype, ReferenceNo, "Print");
                PrintPDFReport rpt = new PrintPDFReport();
                rpt.PrintSelectedReport(Recordtype, ReferenceNo, "Print", "", "", ddlMF.SelectedValue, "", "", "", "");
            }
        }
        
        //public void PrintSelectedReport(string Rectype, string RefNo, string Print)
        //{
        //    PrintPDFReport rpt = new PrintPDFReport();
        //    switch (Rectype)
        //    {
        //        case "SO":
        //            var smp = dc.SoilSampleTest_View(RefNo, "");
        //            foreach (var so in smp)
        //            {
        //                rpt.Soil_PDFReport(RefNo, Convert.ToString(so.SOSMPLTEST_SampleName_var), Print);
        //                break;
        //            }
        //            break;
        //        case "TILE":
        //            rpt.Tile_PDFReport(RefNo, Print);
        //            break;
        //        case "BT-":
        //            rpt.Brick_PDFReport(RefNo, Print);
        //            break;
        //        case "FLYASH":
        //            rpt.FlyAsh_PDFReport(RefNo, Print);
        //            break;
        //        case "CEMT":
        //            rpt.Cement_PDFReport(RefNo, Print);
        //            break;
        //        case "CCH":
        //            rpt.CCH_PDFReport(RefNo, Print);
        //            break;
        //        case "CT":
        //            rpt.Cube_PDFReport(RefNo, 0, Rectype, "", Print, "", "");
        //            break;
        //        case "PILE":
        //            rpt.Pile_PDFReport(RefNo, Print);
        //            break;
        //        case "STC":
        //            rpt.STC_PDFReport(RefNo, Print);
        //            break;
        //        case "ST":
        //            rpt.ST_PDFReport(RefNo, Print);
        //            break;
        //        case "WT":
        //            rpt.WT_PDFReport(RefNo, Print);
        //            break;
        //        case "AGGT":
        //            rpt.Aggregate_PDFReport(RefNo, Rectype, "", 0, Print);
        //            break;
        //        case "SOLID":
        //            var details = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "SOLID");
        //            foreach (var solid in details)
        //            {
        //                if (Convert.ToString(solid.TEST_Sr_No) == "1")//(solid.SOLIDINWD_TEST_Id) == "66")
        //                {
        //                    rpt.SOLID_CS_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(solid.TEST_Sr_No) == "2")
        //                {
        //                    rpt.SOLID_WA_PDFReport(RefNo, Print);
        //                }
        //            }
        //            break;
        //        case "OT":
        //            rpt.OT_PDFReport(RefNo, Print);
        //            break;
        //        case "CR":
        //            rpt.Core_PDFReport(RefNo, Print);
        //            break;
        //        case "NDT":
        //            rpt.NDT_PDFReport(RefNo, Print, "");
        //            break;
        //        case "PT":
        //            var PTdetails = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "PT");
        //            foreach (var PTWA in PTdetails)
        //            {
        //                if (Convert.ToString(PTWA.TEST_Sr_No) == "1")//1
        //                {
        //                    rpt.Pavement_CS_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(PTWA.TEST_Sr_No) == "2")//2 //(Convert.ToString(PTWA.PTINWD_TEST_Id) == "63")
        //                {
        //                    rpt.Pavement_WA_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(PTWA.TEST_Sr_No) == "3")//3
        //                {
        //                    rpt.Pavement_TS_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(PTWA.TEST_Sr_No) == "4")//4
        //                {
        //                    rpt.Pavement_FS_PDFReport(RefNo, Print);
        //                }
        //            }
        //            break;
        //        case "MF":
        //            PrintHTMLReport rptHtml = new PrintHTMLReport();
        //            int trailId = 0;
        //            var mixd = dc.Trial_View(RefNo, true);
        //            foreach (var mf in mixd)
        //            {
        //                trailId = mf.Trial_Id;
        //            }
        //            rpt.MF_MDLetter_PDFReport(RefNo, trailId, "MF", ddlMF.SelectedValue, Print);
        //            break;
        //        case "AAC":
        //            var detail = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "AAC");
        //            foreach (var aac in detail)
        //            {
        //                if (Convert.ToString(aac.TEST_Sr_No) == "1")
        //                {
        //                    rpt.AAC_CS_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(aac.TEST_Sr_No) == "2")
        //                {
        //                    rpt.AAC_DS_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(aac.TEST_Sr_No) == "3")
        //                {
        //                    rpt.AAC_DM_PDFReport(RefNo, Print);
        //                }
        //                else if (Convert.ToString(aac.TEST_Sr_No) == "4")
        //                {
        //                    rpt.AAC_SN_PDFReport(RefNo, Print);
        //                }
        //            }
        //            break;
        //    }
        //}

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

    }

}
