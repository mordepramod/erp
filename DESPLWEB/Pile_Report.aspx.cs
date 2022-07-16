using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.IO;

namespace DESPLWEB
{
    public partial class Pile_Report : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        public static int ClId = 0;
        public static int SiteId = 0;
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
                    if (strReq.Contains("=") == false)
                    {
                        Session.Abandon();
                        Response.Redirect("Login.aspx");
                    }
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
                }
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Pile - Report Entry";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                if (txt_RecordType.Text != "")
                {
                    getCurrentTestingDate();
                    DisplayPileDetails();
                    LoadContactPersonList();
                    BindDetailsGrid();
                    DisplayRemark();

                    if (lblEntry.Text == "Check")
                    {
                        lblheading.Text = "Pile - Report Check";
                        // lblEntry.Text = Session["Check"].ToString();//
                        lbl_TestedBy.Text = "Approve By";
                        BindIdentification();
                        LoadApproveBy();
                        //LoadOtherPendingCheckRpt();
                        ViewWitnessBy();
                    }
                    else
                    {
                        //LoadOtherPendingRpt();
                        LoadTestedBy();
                    }
                    LoadReferenceNoList();
                }
            }
        }
        private void LoadReferenceNoList()
        {
            byte reportStatus = 0;
            if (lblEntry.Text == "Enter")
                reportStatus = 1;
            else if (lblEntry.Text == "Check")
                reportStatus = 2;

            var reportList = dc.ReferenceNo_View_StatusWise("PILE", reportStatus, 0);
            ddl_OtherPendingRpt.DataTextField = "ReferenceNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_OtherPendingRpt.Items.Remove(txt_ReferenceNo.Text);
        }
        public void getCurrentTestingDate()
        {
            txt_DateOfTesting.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }
        private void LoadOtherPendingRpt()
        {
            string Refno = txt_ReferenceNo.Text;
            var testinguser = dc.ReportStatus_View("Pile Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 1, 0);
            ddl_OtherPendingRpt.DataTextField = "PILEINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataValueField = "PILEINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataSource = testinguser;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, "---Select---");
            ListItem itemToRemove = ddl_OtherPendingRpt.Items.FindByValue(Refno);
            if (itemToRemove != null)
            {
                ddl_OtherPendingRpt.Items.Remove(itemToRemove);
            }
        }
        private void LoadOtherPendingCheckRpt()
        {
            if (lblEntry.Text == "Check")
            {
                string Refno = txt_ReferenceNo.Text;
                var testinguser = dc.ReportStatus_View("Pile Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 2, 0);
                ddl_OtherPendingRpt.DataTextField = "PILEINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataValueField = "PILEINWD_ReferenceNo_var";
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
        public void DisplayPileDetails()
        {
            var pInwd = dc.ReportStatus_View("Pile Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var pi in pInwd)
            {
                txt_ReferenceNo.Text = pi.PILEINWD_ReferenceNo_var.ToString();
                ClId = Convert.ToInt32(pi.INWD_CL_Id);
                SiteId = Convert.ToInt32(pi.INWD_SITE_Id);
                txt_Description.Text = pi.PILEINWD_Description_var.ToString();
                txt_ReportNo.Text = pi.PILEINWD_SetOfRecord_var.ToString();
                if (pi.PILEINWD_TestedDate_dt.ToString() != null && pi.PILEINWD_TestedDate_dt.ToString() != "")
                {
                    txt_DateOfTesting.Text = Convert.ToDateTime(pi.PILEINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                }
                if (txt_DateOfTesting.Text == "" || lblEntry.Text == "Enter")
                {
                    txt_DateOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (pi.PILEINWD_KindAttentionId_int != null && pi.PILEINWD_KindAttentionId_int.ToString() != "")
                {
                    LoadContactPersonList();
                    ddl_KindAttention.SelectedValue = pi.PILEINWD_KindAttentionId_int.ToString();
                }
                if (ddl_NablScope.Items.FindByValue(pi.PILEINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(pi.PILEINWD_NablScope_var);
                }
                if (Convert.ToString(pi.PILEINWD_NablLocation_int) != null && Convert.ToString(pi.PILEINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(pi.PILEINWD_NablLocation_int);
                }
                txt_SupplierName.Text = pi.PILEINWD_SupplierName_var.ToString();
                txt_RecordType.Text = pi.PILEINWD_RecordType_var.ToString();
            }
        }
        public void BindDetailsGrid()
        {
            // int i = 0;
            var details = dc.PileDetailsView("", 0, "");
            grdPileEntryRptInward.DataSource = details;
            grdPileEntryRptInward.DataBind();

            //foreach (var pl in details)
            //{
            //    Label txt_Catagory = (Label)grdPileEntryRptInward.Rows[i].Cells[1].FindControl("txt_Catagory");
            //    Label txt_Descp = (Label)grdPileEntryRptInward.Rows[i].Cells[2].FindControl("txt_Descp");
            //    Label txt_Idenitfication = (Label)grdPileEntryRptInward.Rows[i].Cells[3].FindControl("txt_Idenitfication");
            //    LinkButton lnk_Identification = (LinkButton)grdPileEntryRptInward.Rows[i].Cells[4].FindControl("lnk_Identification");

            //    txt_Catagory.Text = pl.PILE_Name_var.ToString();
            //    txt_Descp.Text = pl.PILE_Description_var.ToString();
            //    lnk_Identification.Text = "Select";
            //    i++;
            //}

        }
        public void BindIdentification()
        {
            string Identificationvar = "";
            for (int i = 0; i < grdPileEntryRptInward.Rows.Count; i++)
            {

                TextBox txt_Catagory = (TextBox)grdPileEntryRptInward.Rows[i].Cells[1].FindControl("txt_Catagory");
                TextBox txt_Idenitfication = (TextBox)grdPileEntryRptInward.Rows[i].Cells[3].FindControl("txt_Idenitfication");

                var pl = dc.PileDetailsView(Convert.ToString(txt_ReferenceNo.Text), 0, "");
                foreach (var p in pl)
                {
                    if (Convert.ToInt32(p.PILEDETAIL_CatagoryId_int) > 0)
                    {
                        var c = dc.PileDetailsView("", Convert.ToInt32(p.PILEDETAIL_CatagoryId_int), "");
                        foreach (var n in c)
                        {
                            if (n.PILE_Name_var.ToString() == txt_Catagory.Text)
                            {
                                if (p.PILEDETAIL_CatagoryId_int > 0)
                                {
                                    Identificationvar = p.PILEDETAIL_Identification_var.ToString();
                                }
                                txt_Idenitfication.Text = txt_Idenitfication.Text = txt_Idenitfication.Text + Identificationvar + ",";
                            }
                        }
                    }
                }
                if (txt_Idenitfication.Text == "")
                {
                    txt_Idenitfication.Text = "NA";
                }
            }
        }
        public void DisplayGridRow()
        {
            if (4 > grdPileEntryRptInward.Rows.Count)
            {
                for (int i = grdPileEntryRptInward.Rows.Count + 1; i <= 4; i++)
                {
                    AddRowEntryRptPileInward();
                }
            }
        }
        protected void AddRowEntryRptPileInward()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["PileDetailsTable"] != null)
            {
                GetCurrentDataRptPileInward();
                dt = (DataTable)ViewState["PileDetailsTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Catagory", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Descp", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Idenitfication", typeof(string)));
                dt.Columns.Add(new DataColumn("lnk_Identification", typeof(string)));

            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_Catagory"] = string.Empty;
            dr["txt_Descp"] = string.Empty;
            dr["txt_Idenitfication"] = string.Empty;
            dr["lnk_Identification"] = string.Empty;

            dt.Rows.Add(dr);

            ViewState["PileDetailsTable"] = dt;
            grdPileEntryRptInward.DataSource = dt;
            grdPileEntryRptInward.DataBind();
            SetPreviousDataRptPileInward();
        }
        protected void GetCurrentDataRptPileInward()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Catagory", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Descp", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Idenitfication", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lnk_Identification", typeof(string)));

            for (int i = 0; i < grdPileEntryRptInward.Rows.Count; i++)
            {
                TextBox txt_Catagory = (TextBox)grdPileEntryRptInward.Rows[i].Cells[1].FindControl("txt_Catagory");
                TextBox txt_Descp = (TextBox)grdPileEntryRptInward.Rows[i].Cells[2].FindControl("txt_Descp");
                TextBox txt_Idenitfication = (TextBox)grdPileEntryRptInward.Rows[i].Cells[3].FindControl("txt_Idenitfication");
                LinkButton lnk_Identification = (LinkButton)grdPileEntryRptInward.Rows[i].Cells[4].FindControl("lnk_Identification");


                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_Catagory"] = txt_Catagory.Text;
                drRow["txt_Descp"] = txt_Descp.Text;
                drRow["txt_Idenitfication"] = txt_Idenitfication.Text;
                drRow["lnk_Identification"] = lnk_Identification.Text;



                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["PileDetailsTable"] = dtTable;

        }
        protected void SetPreviousDataRptPileInward()
        {
            DataTable dt = (DataTable)ViewState["PileDetailsTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Catagory = (TextBox)grdPileEntryRptInward.Rows[i].Cells[1].FindControl("txt_Catagory");
                TextBox txt_Descp = (TextBox)grdPileEntryRptInward.Rows[i].Cells[2].FindControl("txt_Descp");
                TextBox txt_Idenitfication = (TextBox)grdPileEntryRptInward.Rows[i].Cells[3].FindControl("txt_Idenitfication");
                LinkButton lnk_Identification = (LinkButton)grdPileEntryRptInward.Rows[i].Cells[4].FindControl("lnk_Identification");

                grdPileEntryRptInward.Rows[i].Cells[2].Text = (i + 1).ToString();

                txt_Catagory.Text = dt.Rows[i]["txt_Catagory"].ToString();
                txt_Descp.Text = dt.Rows[i]["txt_Descp"].ToString();
                txt_Idenitfication.Text = dt.Rows[i]["txt_Idenitfication"].ToString();
                lnk_Identification.Text = dt.Rows[i]["lnk_Identification"].ToString();
            }

        }
        protected void AddRowPileRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["PileRemarkTable"] != null)
            {
                GetCurrentDataPileRemark();
                dt = (DataTable)ViewState["PileRemarkTable"];
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

            ViewState["PileRemarkTable"] = dt;
            grdPileRemark.DataSource = dt;
            grdPileRemark.DataBind();
            SetPreviousDataPileRemark();
        }

        protected void GetCurrentDataPileRemark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));

            for (int i = 0; i < grdPileRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdPileRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["PileRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataPileRemark()
        {
            DataTable dt = (DataTable)ViewState["PileRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdPileRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                grdPileRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }

        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowPileRemark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdPileRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdPileRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowPileRemark(gvr.RowIndex);
            }
        }
        protected void DeleteRowPileRemark(int rowIndex)
        {
            GetCurrentDataPileRemark();
            DataTable dt = ViewState["PileRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["PileRemarkTable"] = dt;
            grdPileRemark.DataSource = dt;
            grdPileRemark.DataBind();
            SetPreviousDataPileRemark();
        }
        private void LoadContactPersonList()
        {
            ddl_KindAttention.DataTextField = "CONT_Name_var";
            ddl_KindAttention.DataValueField = "CONT_Id";
            var Per = dc.Contact_View(0, Convert.ToInt32(SiteId), Convert.ToInt32(ClId), "");
            ddl_KindAttention.DataSource = Per;
            ddl_KindAttention.DataBind();
            ddl_KindAttention.Items.Insert(0, "---Select---");
        }
        public void DisplayRemark()
        {
            int i = 0;
            var re = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "PILE");
            foreach (var r in re)
            {
                AddRowPileRemark();
                TextBox txt_REMARK = (TextBox)grdPileRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.PILEDetail_RemarkId_int), "PILE");
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.PILE_Remark_var.ToString();
                    i++;
                }
            }
            if (grdPileRemark.Rows.Count <= 0)
            {
                AddRowPileRemark();
            }

        }
        protected void lnk_Fetch_Click(object sender, EventArgs e)
        {
            FetchRefNo();
        }
        public void FetchRefNo()
        {
            if (ddl_OtherPendingRpt.SelectedValue != "---Select---")
            {
                try
                {
                    lnkSave.Enabled = true;
                    lnkPrint.Visible = false;
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Visible = false;
                    txt_ReferenceNo.Text = ddl_OtherPendingRpt.SelectedItem.Text;
                    txt_ReferenceNo.Text = txt_ReferenceNo.Text;
                    DisplayPileDetails();
                    //LoadOtherPendingCheckRpt();
                    LoadReferenceNoList();
                    ViewWitnessBy();
                    LoadApproveBy();
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
                finally
                {
                    grdPileEntryRptInward.DataSource = null;
                    grdPileEntryRptInward.DataBind();
                    BindDetailsGrid();
                    grdPileRemark.DataSource = null;
                    grdPileRemark.DataBind();
                    DisplayRemark();
                }
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
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                DateTime TestingDt = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);

                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, Convert.ToInt32(ddl_KindAttention.SelectedValue), TestingDt, txt_Description.Text, "", 2, 0, "", 0, "", "", 0, 0, 0, "PILE");
                    dc.ReportDetails_Update("PILE", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                    dc.MISDetail_Update(0, "PILE", txt_ReferenceNo.Text, "PILE", null, true, false, false, false, false, false, false);
                }
                else
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, Convert.ToInt32(ddl_KindAttention.SelectedValue), TestingDt, txt_Description.Text, "", 3, 0, "", 0, "", "", 0, 0, 0, "PILE");
                    dc.ReportDetails_Update("PILE", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "PILE", txt_ReferenceNo.Text, "PILE", null, false, true, false, false, false, false, false);
                }
                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "PILE", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                int CatagoryId = 0;
                int j = 0;
                for (int i = 0; i < grdPileEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_Catagory = (TextBox)grdPileEntryRptInward.Rows[i].Cells[1].FindControl("txt_Catagory");
                    TextBox txt_Idenitfication = (TextBox)grdPileEntryRptInward.Rows[i].Cells[3].FindControl("txt_Idenitfication");
                    j++;
                    string[] line = txt_Idenitfication.Text.Split(',');
                    foreach (string line1 in line)
                    {
                        if (line1 != "")
                        {
                            var pc = dc.PileDetailsView("", 0, txt_Catagory.Text);
                            foreach (var p in pc)
                            {
                                if (txt_Idenitfication.Text != "NA")
                                {
                                    CatagoryId = p.PILE_CatagoryId_int;
                                }
                            }
                            if (txt_Idenitfication.Text != "NA")
                            {
                                dc.PileDtls_Update(txt_ReferenceNo.Text, line1.ToString(), CatagoryId, true);
                            }
                            else
                            {
                                dc.PileDtls_Update(txt_ReferenceNo.Text, line1.ToString().Trim(), Convert.ToInt32(j), false);
                            }
                        }
                    }
                }

                #region Remark Gridview

                int RemarkId = 0;
                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "PILE", true);
                for (int i = 0; i < grdPileRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdPileRemark.Rows[i].Cells[1].FindControl("txt_REMARK");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AllRemark_View(txt_REMARK.Text, "", 0, "PILE");
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.PILE_RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "PILE");
                            foreach (var c in chkId)
                            {
                                if (c.PILEDetail_RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "PILE", false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.AllRemark_Update(0, txt_REMARK.Text, "PILE");
                            var chc = dc.AllRemark_View(txt_REMARK.Text, "", 0, "PILE");
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.PILE_RemarkId_int);
                                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "PILE", false);
                            }
                        }
                    }
                }
                #endregion

                lnkSave.Enabled = false;
                lnkPrint.Visible = true;

                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkSave.Enabled = false;
            }
        }
        protected Boolean ValidateData()
        {

            Boolean valid = true;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime TestingDate = DateTime.ParseExact(txt_DateOfTesting.Text, "dd/MM/yyyy", null);
            DateTime currentDate1 = DateTime.ParseExact(CurrentDate, "dd/MM/yyyy", null);

            if (ddl_KindAttention.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select Kind Attention";
                valid = false;
            }
            else if (TestingDate > currentDate1)
            {
                lblMsg.Text = "Testing Date should be always less than or equal to the Current Date.";
                valid = false;
            }
            else if (txt_Description.Text == "")
            {
                lblMsg.Text = "Please Enter Description";
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
            else if (lblEntry.Text == "Check" && ddl_TestedBy.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Please Select the Approve By";
                valid = false;
            }
            else if (lblEntry.Text == "Enter" && ddl_TestedBy.SelectedItem.Text == "---Select---")
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
                if (grdPileEntryRptInward.Rows.Count <= 0)
                {
                    lblMsg.Text = "There are no Records ";
                    valid = false;
                }
            }
            else if (valid == true)
            {
                for (int i = 0; i < grdPileEntryRptInward.Rows.Count; i++)
                {
                    TextBox txt_Idenitfication = (TextBox)grdPileEntryRptInward.Rows[i].Cells[3].FindControl("txt_Idenitfication");
                    if (txt_Idenitfication.Text == "")
                    {
                        txt_Idenitfication.Text = "NA";

                        //dispalyMsg = "Please Select the Identification for row No. " + (i + 1) + ".";
                        //txt_Idenitfication.Focus();
                        // valid = false;
                        // break;
                    }
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
                // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + dispalyMsg + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
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

        protected void lnk_Identification_Click(object sender, EventArgs e)
        {

            GridViewRow clickedRow1 = ((LinkButton)sender).NamingContainer as GridViewRow;
            TextBox txt_Idenitfication = (TextBox)clickedRow1.FindControl("txt_Idenitfication");
            ModalPopupExtender1.Show();
            grdIdentification.DataSource = null;
            grdIdentification.DataBind();
            int j = 0;
            if (txt_Idenitfication.Text != "")
            {
                string[] line2 = txt_Idenitfication.Text.Split(',');
                foreach (string line3 in line2)
                {
                    if (line3 != "" && line3 != "NA")
                    {
                        AddRowIdentification();
                        Label lbl_Identifcton = (Label)grdIdentification.Rows[j].Cells[1].FindControl("lbl_Identifcton");
                        lbl_Identifcton.Text = line3.ToString();
                        j++;
                    }
                }
            }
            LinkButton lnk_Identification = (LinkButton)clickedRow1.FindControl("lnk_Identification");
            string Identification = "";
            var catagory = dc.PileDetailsView(txt_ReferenceNo.Text, 0, "");
            foreach (var desc in catagory)
            {
                Identification = desc.PILEDETAIL_Identification_var.ToString();
                string[] line = Identification.Split(',');
                foreach (string line1 in line)
                {
                    if (line1 != " ")
                    {
                        bool valid = false;
                        for (int m = 0; m < grdPileEntryRptInward.Rows.Count; m++)
                        {
                            TextBox txt_Idenitfication1 = (TextBox)grdPileEntryRptInward.Rows[m].Cells[3].FindControl("txt_Idenitfication");
                            string[] line2 = txt_Idenitfication1.Text.Split(',');
                            foreach (string line3 in line2)
                            {
                                if (line3 != "")
                                {
                                    if (line3 == line1)
                                    {
                                        valid = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (valid == false)
                        {
                            if (line1 != "NA")
                            {
                                AddRowIdentification();
                                Label lbl_Identifcton = (Label)grdIdentification.Rows[j].Cells[1].FindControl("lbl_Identifcton");
                                lbl_Identifcton.Text = line1.ToString();
                                j++;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < grdIdentification.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdIdentification.Rows[i].Cells[0].FindControl("cbxSelect");
                Label lbl_Identifcton = (Label)grdIdentification.Rows[i].Cells[1].FindControl("lbl_Identifcton");

                string[] line2 = txt_Idenitfication.Text.Split(',');
                foreach (string line3 in line2)
                {
                    if (line3 != "")
                    {
                        if (lbl_Identifcton.Text == line3)
                        {
                            cbxSelect.Checked = true;
                        }
                    }
                }
            }
        }

        public void ViewWitnessBy()
        {
            txt_witnessBy.Visible = false;
            chk_WitnessBy.Checked = false;
            if (lblEntry.Text == "Check")
            {
                var ct = dc.ReportStatus_View("Pile Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
                foreach (var c in ct)
                {
                    if (c.PILEINWD_WitnessBy_var.ToString() != null && c.PILEINWD_WitnessBy_var.ToString() != "")
                    {
                        txt_witnessBy.Visible = true;
                        txt_witnessBy.Text = c.PILEINWD_WitnessBy_var.ToString();
                        chk_WitnessBy.Checked = true;
                    }
                }
            }

        }
        public void DeleteRow()
        {
            if (grdPileRemark.Rows.Count > 1)
            {
                DeleteRowPileRemark(grdPileRemark.Rows.Count - 1);
            }
        }

        protected void AddRowIdentification()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            if (ViewState["Identification"] != null)
            {
                GetCurrentDataIdentification();
                dt = (DataTable)ViewState["Identification"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("cbxSelect", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_Identifcton", typeof(string)));

            }
            dr = dt.NewRow();
            dr["cbxSelect"] = string.Empty;// dt.Rows.Count + 1;
            dr["lbl_Identifcton"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["Identification"] = dt;
            grdIdentification.DataSource = dt;
            grdIdentification.DataBind();
            SetPreviousDataIdentification();
        }

        protected void GetCurrentDataIdentification()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("cbxSelect", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_Identifcton", typeof(string)));

            for (int i = 0; i < grdIdentification.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdIdentification.Rows[i].Cells[0].FindControl("cbxSelect");
                Label lbl_Identifcton = (Label)grdIdentification.Rows[i].Cells[1].FindControl("lbl_Identifcton");
                drRow = dtTable.NewRow();
                drRow["cbxSelect"] = cbxSelect.Text;//i + 1;
                drRow["lbl_Identifcton"] = lbl_Identifcton.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["Identification"] = dtTable;

        }
        protected void SetPreviousDataIdentification()
        {
            DataTable dt = (DataTable)ViewState["Identification"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdIdentification.Rows[i].Cells[0].FindControl("cbxSelect");
                Label lbl_Identifcton = (Label)grdIdentification.Rows[i].Cells[1].FindControl("lbl_Identifcton");

                cbxSelect.Text = dt.Rows[i]["cbxSelect"].ToString();
                lbl_Identifcton.Text = dt.Rows[i]["lbl_Identifcton"].ToString();
            }
        }
        protected Boolean ValidateChk()
        {
            Boolean valid = true;

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            for (int i = 0; i < grdIdentification.Rows.Count; i++)
            {

                CheckBox cbxSelect = (CheckBox)grdIdentification.Rows[i].Cells[0].FindControl("cbxSelect");
                Label lbl_Identifcton = (Label)grdIdentification.Rows[i].Cells[1].FindControl("lbl_Identifcton");
                if (cbxSelect.Checked)
                {
                    for (int m = 0; m < grdPileEntryRptInward.Rows.Count; m++)
                    {
                        TextBox txt_Idenitfication = (TextBox)grdPileEntryRptInward.Rows[m].Cells[3].FindControl("txt_Idenitfication");
                        string[] line = txt_Idenitfication.Text.Split(',');
                        foreach (string line1 in line)
                        {
                            if (line1 != " ")
                            {
                                if (lbl_Identifcton.Text == line1)
                                {
                                    lblMsg.Visible = true;
                                    lblMsg.Text = "This Identification is already selected  for row number" + (i + 1) + ".";
                                    //dispalyMsg = "This Identification is already selected  for row number" + (i + 1) + ".";
                                    valid = false;
                                    break;
                                }
                                else
                                {
                                    lblMsg.Visible = false;
                                }
                            }
                        }
                    }
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
                //  ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + dispalyMsg + "');", true);
            }

            return valid;
        }


        protected void imgCloseTestDetails_Click(object sender, ImageClickEventArgs e)
        {
            TextBox txt_Idenitfication = grdPileEntryRptInward.SelectedRow.Cells[3].FindControl("txt_Idenitfication") as TextBox;
            txt_Idenitfication.Text = string.Empty;
            for (int i = 0; i < grdIdentification.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdIdentification.Rows[i].Cells[0].FindControl("cbxSelect");
                Label lbl_Identifcton = (Label)grdIdentification.Rows[i].Cells[1].FindControl("lbl_Identifcton");
                if (cbxSelect.Checked)
                {
                    if (txt_Idenitfication.Text == "")
                        txt_Idenitfication.Text = lbl_Identifcton.Text;
                    else
                        txt_Idenitfication.Text = txt_Idenitfication.Text + "," + lbl_Identifcton.Text;
                }
            }
            if (grdIdentification.Rows.Count <= 0)
            {
                txt_Idenitfication.Text = "NA";
            }
            ModalPopupExtender1.Hide();

        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                //rpt.Pile_PDFReport(txt_ReferenceNo.Text, lblEntry.Text);
                rpt.PrintSelectedReport(txt_RecordType.Text, txt_ReferenceNo.Text, lblEntry.Text, "", "", "", "", "", "", "");
            }
            //RptPile();
        }
        public void RptPile()
        {
            //string reportPath;
            //string reportStr = "";
            //StreamWriter sw;
            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            //reportStr = getDetailReportPile();
            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
            PrintHTMLReport rptHtml = new PrintHTMLReport();
            rptHtml.PileReport_Html(txt_ReferenceNo.Text, Convert.ToInt32(lblRecordNo.Text), lblEntry.Text);
        }
        //protected string getDetailReportPile()
        //{

        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";

        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Pile Integrity</b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    var ct = dc.ReportStatus_View("Pile Testing", null, null, 0, 0, 0, txt_ReferenceNo.Text, 0, 2, 0);

        //    foreach (var pile in ct)
        //    {
        //        mySql += "<tr>" +
        //              "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td width='40%' height=19><font size=2>" + pile.CL_Name_var + "</font></td>" +
        //              "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + "-" + "</font></td>" +
        //              "<td height=19><font size=2>&nbsp;</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +

        //             "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //             "<td width='2%' height=19><font size=2></font></td>" +
        //             "<td width='10%' height=19><font size=2></font></td>" +
        //             "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "PILE" + "-" + pile.PILEINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td  width='2%' height=19><font size=2></font>:</td>" +
        //             "<td width='40%' height=19><font size=2>" + pile.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "PILE" + "-" + pile.PILEINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //            "<td width='2%' height=19><font size=2></font></td>" +
        //            "<td width='10%' height=19><font size=2></font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "PILE" + "-" + "" + "</font></td>" +
        //            "</tr>";


        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + pile.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(pile.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //            "</tr>";



        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + pile.PILEINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(pile.PILEINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //              "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test Results : " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";


        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";


        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Catagory</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Inference</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Pile Identification </b></font></td>";
        //    mySql += "</tr>";

        //    int i = 0;
        //    int SrNo = 0;
        //    int CountPiles = 0;
        //    var details = dc.PileDetailsView("", 0, "");
        //    foreach (var t in details)
        //    {

        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + t.PILE_Name_var + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + t.PILE_Description_var + "</font></td>";


        //        bool valid = true;

        //        string Identi = "";
        //        var pl = dc.PileDetailsView(txt_ReferenceNo.Text, 0, "");
        //        foreach (var p in pl)
        //        {
        //            if (Convert.ToInt32(p.PILEDETAIL_CatagoryId_int) > 0)
        //            {
        //                var c = dc.PileDetailsView("", Convert.ToInt32(p.PILEDETAIL_CatagoryId_int), "");
        //                foreach (var n in c)
        //                {
        //                    if (p.PILEDETAIL_Identification_var != null)
        //                    {
        //                        if (n.PILE_Name_var.ToString() == t.PILE_Name_var.ToString())
        //                        {
        //                            CountPiles++;
        //                            valid = false;
        //                            Identi = Identi + p.PILEDETAIL_Identification_var.ToString() + ",";
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (valid == true)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "NA" + "</font></td>";
        //        }
        //        else
        //        {
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + Identi.ToString() + "</font></td>";
        //        }

        //        mySql += "</tr>";
        //    }

        //    i++;

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "Total No Of Piles Tested :" + "</font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + CountPiles + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    SrNo = 0;
        //    var matid = dc.Material_View("PILE", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }

        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, "PILE");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.PILEDetail_RemarkId_int), "PILE");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.PILE_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";

        //        }
        //    }

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";
        //    if (lblEntry.Text == "Check")
        //    {
        //        var RecNo = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, Convert.ToInt32(lblRecordNo.Text), "", 0, "PILE");
        //        foreach (var r in RecNo)
        //        {
        //            var Auth = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, r.PILEINWD_ApprovedBy_tint, 0, 0, "", 0, "PILE");

        //            foreach (var Approve in Auth)
        //            {

        //                mySql += "<tr>";
        //                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Approve.USER_Name_var.ToString() + "</font></td>";
        //                mySql += "</tr>";
        //                mySql += "<tr>";
        //                mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + Approve.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                mySql += "</tr>";

        //            }
        //        }
        //    }


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
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
    }
}
