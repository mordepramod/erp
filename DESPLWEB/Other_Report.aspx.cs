using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace DESPLWEB
{
    public partial class Other_Report : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected string PostBackStr;
        protected void Page_Load(object sender, EventArgs e)
        {
            PostBackStr = Page.ClientScript.GetPostBackEventReference(this, "MyCustomArgument");
            string eventArg = Request.Params.Get("__EVENTARGUMENT");
            if (eventArg == "MyCustomArgument")
            {
                lblFileName.Visible = true;
                btnCancelDwnl.Visible = true;
                FileUpload1.Visible = false;

                if (Session["FileUpload1"] == null && FileUpload1.HasFile)
                {
                    Session["FileUpload1"] = FileUpload1;
                    lblFileName.Text = FileUpload1.FileName;
                }
                else if (Session["FileUpload1"] != null && (!FileUpload1.HasFile))
                {
                    FileUpload1 = (FileUpload)Session["FileUpload1"];
                    lblFileName.Text = FileUpload1.FileName;
                }
                else if (FileUpload1.HasFile)
                {
                    Session["FileUpload1"] = FileUpload1;
                    lblFileName.Text = FileUpload1.FileName;
                }
                //**************************************************
                if (Session["FileUpload1"] != null && (!FileUpload1.HasFile))
                {
                    FileUpload1 = (FileUpload)Session["FileUpload1"];
                    lblFileName.Text = FileUpload1.FileName;
                }
                // Read the file and convert it to Byte Array
                string filePath = FileUpload1.PostedFile.FileName;
                string filename = Path.GetFileName(filePath);
                string ext = Path.GetExtension(filename);
                string contenttype = String.Empty;

                if (ext != ".pdf")
                {
                    Session["FileUpload1"] = null;
                    lblFileName.Text = "";
                    lblFileName.Visible = false;
                    btnCancelDwnl.Visible = false;
                    FileUpload1.Visible = true;
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Report should be in pdf format.');", true);
                }
                else
                {
                    //Stream fs = FileUpload1.PostedFile.InputStream;
                    //BinaryReader br = new BinaryReader(fs);
                    //Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                    ////Set the contenttype based on File Extension
                    //switch (ext)
                    //{
                    //    case ".doc":
                    //        contenttype = "application/vnd.ms-word";
                    //        break;

                    //    case ".docx":
                    //        contenttype = "application/vnd.officedocument.wordprocessingml.document";
                    //        break;

                    //    case ".xls":
                    //        contenttype = "application/vnd.ms-excel";
                    //        break;

                    //    case ".xlsx":
                    //        contenttype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //        break;

                    //    case ".jpg":
                    //        contenttype = "image/jpg";
                    //        break;

                    //    case ".png":
                    //        contenttype = "image/png";
                    //        break;

                    //    case ".gif":
                    //        contenttype = "image/gif";
                    //        break;

                    //    case ".pdf":
                    //        contenttype = "application/pdf";
                    //        break;
                    //    case ".txt":
                    //        contenttype = "text/plain";
                    //        break;
                    //    case ".zip":
                    //        contenttype = "application/octet-stream"; //"application/x-compressed"; //application/octet-stream
                    //        break;
                    //    case ".rar":
                    //        contenttype = "application/rar";
                    //        break;
                    //}
                    //filename = filename.Replace(" ", "").ToString();
                    //dc.TempFile_Update(Convert.ToInt32(Session["LoginId"].ToString()), "", "", null, true);
                    //dc.TempFile_Update(Convert.ToInt32(Session["LoginId"].ToString()), filename, contenttype, bytes, false);
                    //Session["FileUpload1"] = null;
                }
                //*****************************************************************
            }

            if (!this.IsPostBack)
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

                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Other - Report Entry";
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                if (txt_RecordType.Text != "")
                {
                    LoadInwardType();
                    txt_DtOfTesting.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    DisplayOtherDetails();

                    LoadAddSign();
                    LoadOtherTestList();
                    if (lblEntry.Text == "Check")
                    {
                        //lblAddSign.Visible = false;
                        //ddlAddSign.Visible = false;
                        lnkTemplate.Visible = false;
                        lblheading.Text = "Other - Report Check";
                        lbl_TestedBy.Text = "Approve By";

                        LoadApproveBy();
                        ViewWitnessBy();
                    }
                    else
                    {
                        LoadTestedBy();
                    }
                    LoadReferenceNoList();
                    
                }

            }
        }

        #region load list
        private void LoadInwardType()
        {
            var inwd = dc.Material_View("", "");
            ddlSection.DataTextField = "MATERIAL_Name_var";
            ddlSection.DataValueField = "MATERIAL_RecordType_var";
            ddlSection.DataSource = inwd;
            ddlSection.DataBind();
            ddlSection.Items.Insert(ddlSection.Items.IndexOf(ddlSection.Items.FindByValue("FLYASH")) + 1, new ListItem("Fly Ash Chemical Testing", "FLYASHCH"));
            ddlSection.Items.Insert(ddlSection.Items.IndexOf(ddlSection.Items.FindByValue("TILE")) + 1, new ListItem("Tile Chemical Testing", "TILECH"));            
            ddlSection.Items.Insert(0, "---Select---");
            ddlSection.Items.Remove(ddlSection.Items.FindByValue("OT"));
            ddlSection.Items.Add(new ListItem("Other Testing", "OT"));
        }
        public void LoadOtherTest()
        {
            int MaterialId = 0;
            var InwardId = dc.Material_View("OT", "");
            foreach (var n in InwardId)
            {
                MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
            }
            var ot = dc.Test_View(MaterialId, 0, "OT", 0, 0, 0);
            ddl_ReportFor.DataTextField = "TEST_Name_var";
            ddl_ReportFor.DataValueField = "TEST_Sr_No"; // "TEST_Id";
            ddl_ReportFor.DataSource = ot;
            ddl_ReportFor.DataBind();
            ddl_ReportFor.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        private void LoadOtherPendingRpt()
        {
            string Refno = txt_ReferenceNo.Text;
            var testinguser = dc.ReportStatus_View("Other Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 1, 0);
            ddl_OtherPendingRpt.DataTextField = "OTINWD_ReferenceNo_var";
            ddl_OtherPendingRpt.DataValueField = "OTINWD_ReferenceNo_var";
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
                var testinguser = dc.ReportStatus_View("Other Testing", null, null, 0, 0, 0, "", Convert.ToInt32(lblRecordNo.Text), 2, 0);
                ddl_OtherPendingRpt.DataTextField = "OTINWD_ReferenceNo_var";
                ddl_OtherPendingRpt.DataValueField = "OTINWD_ReferenceNo_var";
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
        private void LoadAddSign()
        {

            //ddlAddSign.DataTextField = "USER_Name_var";
            //ddlAddSign.DataValueField = "USER_Id";
            //var testinguser = dc.ReportStatus_View("", null, null, 0, 0, 1, "", 0, 0, 0);
            //ddlAddSign.DataSource = testinguser;
            //ddlAddSign.DataBind();
            //ddlAddSign.Items.Insert(0, "---Select---");

        }
        private void LoadReferenceNoList()
        {
            byte reportStatus = 0;
            if (lblEntry.Text == "Enter")
                reportStatus = 1;
            else if (lblEntry.Text == "Check")
                reportStatus = 2;

            var reportList = dc.ReferenceNo_View_StatusWise("OT", reportStatus, 0);
            ddl_OtherPendingRpt.DataTextField = "ReferenceNo";
            ddl_OtherPendingRpt.DataSource = reportList;
            ddl_OtherPendingRpt.DataBind();
            ddl_OtherPendingRpt.Items.Insert(0, new ListItem("---Select---", "0"));
            ddl_OtherPendingRpt.Items.Remove(txt_ReferenceNo.Text);
        }
        #endregion
        protected void LoadOtherTestList()
        {
            var test = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, 0);
            ddl_Category.DataSource = test;
            ddl_Category.DataTextField = "TEST_Name_var";
            ddl_Category.DataValueField = "TEST_Id";
            ddl_Category.DataBind();
            ddl_Category.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        protected void DeleteColumn(int ColIndex)
        {
            DataTable dt = ViewState["OtherTable1"] as DataTable;
            dt.Columns.RemoveAt(ColIndex);
            dt.AcceptChanges();
        }

        # region add delete row
        protected void DeleteRowDetail1(int rowIndex)
        {
            GetCurrentDataDetail1();
            DataTable dt = ViewState["OtherTable1"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["OtherTable1"] = dt;
            grdDetail1.DataSource = dt;
            grdDetail1.DataBind();
            SetPreviousDataDetail1();
        }
        protected void AddRowDetail1()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["OtherTable1"] != null)
            {
                GetCurrentDataDetail1();
                dt = (DataTable)ViewState["OtherTable1"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a9", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_a1"] = string.Empty;
            if (ddl_ReportFor.SelectedValue == "20")
            {
                dr["txt_a2"] = "N/mm²";
            }
            else
            {
                dr["txt_a2"] = string.Empty;
            }

            dr["txt_a3"] = string.Empty;
            dr["txt_a4"] = string.Empty;
            dr["txt_a5"] = string.Empty;
            dr["txt_a6"] = string.Empty;
            dr["txt_a7"] = string.Empty;
            dr["txt_a8"] = string.Empty;
            dr["txt_a9"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["OtherTable1"] = dt;

            grdDetail1.DataSource = dt;
            grdDetail1.DataBind();
            SetPreviousDataDetail1();

        }
        protected void GetCurrentDataDetail1()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a5", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a6", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a7", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a8", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a9", typeof(string)));

            for (int i = 0; i < grdDetail1.Rows.Count; i++)
            {
                TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                TextBox txt_a8 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");
                TextBox txt_a9 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a9");

                grdDetail1.Rows[i].Cells[0].Width = 30;
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_a1"] = txt_a1.Text;
                drRow["txt_a2"] = txt_a2.Text;
                drRow["txt_a3"] = txt_a3.Text;
                drRow["txt_a4"] = txt_a4.Text;
                drRow["txt_a5"] = txt_a5.Text;
                drRow["txt_a6"] = txt_a6.Text;
                drRow["txt_a7"] = txt_a7.Text;
                drRow["txt_a8"] = txt_a8.Text;
                drRow["txt_a9"] = txt_a9.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["OtherTable1"] = dtTable;
        }
        protected void SetPreviousDataDetail1()
        {
            DataTable dt = (DataTable)ViewState["OtherTable1"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                TextBox txt_a8 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");
                TextBox txt_a9 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a9");

                grdDetail1.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_a1.Text = dt.Rows[i]["txt_a1"].ToString();
                txt_a2.Text = dt.Rows[i]["txt_a2"].ToString();
                txt_a3.Text = dt.Rows[i]["txt_a3"].ToString();
                txt_a4.Text = dt.Rows[i]["txt_a4"].ToString();
                txt_a5.Text = dt.Rows[i]["txt_a5"].ToString();
                txt_a6.Text = dt.Rows[i]["txt_a6"].ToString();
                txt_a7.Text = dt.Rows[i]["txt_a7"].ToString();
                txt_a8.Text = dt.Rows[i]["txt_a8"].ToString();
                txt_a9.Text = dt.Rows[i]["txt_a9"].ToString();
            }
        }

        protected void DeleteRowDetail2(int rowIndex)
        {
            GetCurrentDataDetail2();
            DataTable dt = ViewState["OtherTable2"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["OtherTable2"] = dt;
            grdDetail2.DataSource = dt;
            grdDetail2.DataBind();
            SetPreviousDataDetail2();
        }
        protected void AddRowDetail2()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["OtherTable2"] != null)
            {
                GetCurrentDataDetail2();
                dt = (DataTable)ViewState["OtherTable2"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_b1", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_b2", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_b3", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_b4", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_b5", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_b6", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_b7", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_b8", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_b9", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_b10", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_b11", typeof(string)));

            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_b1"] = string.Empty;
            dr["txt_b2"] = string.Empty;
            dr["txt_b3"] = string.Empty;
            dr["txt_b4"] = string.Empty;
            dr["txt_b5"] = string.Empty;
            dr["txt_b6"] = string.Empty;
            dr["txt_b7"] = string.Empty;
            dr["txt_b8"] = string.Empty;
            dr["txt_b9"] = string.Empty;
            dr["txt_b10"] = string.Empty;
            dr["txt_b11"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["OtherTable2"] = dt;
            grdDetail2.DataSource = dt;
            grdDetail2.DataBind();
            SetPreviousDataDetail2();

        }
        protected void GetCurrentDataDetail2()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_b1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_b2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_b3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_b4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_b5", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_b6", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_b7", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_b8", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_b9", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_b10", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_b11", typeof(string)));

            for (int i = 0; i < grdDetail2.Rows.Count; i++)
            {
                TextBox txt_b1 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b1");
                TextBox txt_b2 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b2");
                TextBox txt_b3 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b3");
                TextBox txt_b4 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b4");
                TextBox txt_b5 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b5");
                TextBox txt_b6 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b6");
                TextBox txt_b7 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b7");
                TextBox txt_b8 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b8");
                TextBox txt_b9 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b9");
                TextBox txt_b10 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b10");
                TextBox txt_b11 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b11");

                grdDetail2.Rows[i].Cells[0].Width = 30;
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_b1"] = txt_b1.Text;
                drRow["txt_b2"] = txt_b2.Text;
                drRow["txt_b3"] = txt_b3.Text;
                drRow["txt_b4"] = txt_b4.Text;
                drRow["txt_b5"] = txt_b5.Text;
                drRow["txt_b6"] = txt_b6.Text;
                drRow["txt_b7"] = txt_b7.Text;
                drRow["txt_b8"] = txt_b8.Text;
                drRow["txt_b9"] = txt_b9.Text;
                drRow["txt_b10"] = txt_b10.Text;
                drRow["txt_b11"] = txt_b11.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["OtherTable2"] = dtTable;
        }
        protected void SetPreviousDataDetail2()
        {
            DataTable dt = (DataTable)ViewState["OtherTable2"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_b1 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b1");
                TextBox txt_b2 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b2");
                TextBox txt_b3 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b3");
                TextBox txt_b4 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b4");
                TextBox txt_b5 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b5");
                TextBox txt_b6 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b6");
                TextBox txt_b7 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b7");
                TextBox txt_b8 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b8");
                TextBox txt_b9 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b9");
                TextBox txt_b10 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b10");
                TextBox txt_b11 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b11");

                grdDetail2.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_b1.Text = dt.Rows[i]["txt_b1"].ToString();
                txt_b2.Text = dt.Rows[i]["txt_b2"].ToString();
                txt_b3.Text = dt.Rows[i]["txt_b3"].ToString();
                txt_b4.Text = dt.Rows[i]["txt_b4"].ToString();
                txt_b5.Text = dt.Rows[i]["txt_b5"].ToString();
                txt_b6.Text = dt.Rows[i]["txt_b6"].ToString();
                txt_b7.Text = dt.Rows[i]["txt_b7"].ToString();
                txt_b8.Text = dt.Rows[i]["txt_b8"].ToString();
                txt_b9.Text = dt.Rows[i]["txt_b9"].ToString();
                txt_b10.Text = dt.Rows[i]["txt_b10"].ToString();
                txt_b11.Text = dt.Rows[i]["txt_b11"].ToString();

            }
        }

        protected void DeleteRowDetail3(int rowIndex)
        {
            GetCurrentDataDetail3();
            DataTable dt = ViewState["OtherTable3"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["OtherTable3"] = dt;
            grdDetail3.DataSource = dt;
            grdDetail3.DataBind();
            SetPreviousDataDetail3();
        }
        protected void AddRowDetail3()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["OtherTable3"] != null)
            {
                GetCurrentDataDetail3();
                dt = (DataTable)ViewState["OtherTable3"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_c1", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_c2", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_c3", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_c4", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_c5", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_c6", typeof(string)));

                dt.Columns.Add(new DataColumn("txt_c7", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_c8", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_c9", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_c1"] = string.Empty;
            dr["txt_c2"] = string.Empty;
            dr["txt_c3"] = string.Empty;
            dr["txt_c4"] = string.Empty;
            dr["txt_c5"] = string.Empty;
            dr["txt_c6"] = string.Empty;

            dr["txt_c7"] = string.Empty;
            dr["txt_c8"] = string.Empty;
            dr["txt_c9"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["OtherTable3"] = dt;
            grdDetail3.DataSource = dt;
            grdDetail3.DataBind();
            SetPreviousDataDetail3();

        }
        protected void GetCurrentDataDetail3()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_c1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_c2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_c3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_c4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_c5", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_c6", typeof(string)));

            dtTable.Columns.Add(new DataColumn("txt_c7", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_c8", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_c9", typeof(string)));

            for (int i = 0; i < grdDetail3.Rows.Count; i++)
            {
                TextBox txt_c1 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c1");
                TextBox txt_c2 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c2");
                TextBox txt_c3 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c3");
                TextBox txt_c4 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c4");
                TextBox txt_c5 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c5");
                TextBox txt_c6 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c6");

                TextBox txt_c7 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c7");
                TextBox txt_c8 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c8");
                TextBox txt_c9 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c9");

                grdDetail3.Rows[i].Cells[0].Width = 30;
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_c1"] = txt_c1.Text;
                drRow["txt_c2"] = txt_c2.Text;
                drRow["txt_c3"] = txt_c3.Text;
                drRow["txt_c4"] = txt_c4.Text;
                drRow["txt_c5"] = txt_c5.Text;
                drRow["txt_c6"] = txt_c6.Text;

                drRow["txt_c7"] = txt_c7.Text;
                drRow["txt_c8"] = txt_c8.Text;
                drRow["txt_c9"] = txt_c9.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["OtherTable3"] = dtTable;
        }
        protected void SetPreviousDataDetail3()
        {
            DataTable dt = (DataTable)ViewState["OtherTable3"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_c1 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c1");
                TextBox txt_c2 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c2");
                TextBox txt_c3 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c3");
                TextBox txt_c4 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c4");
                TextBox txt_c5 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c5");
                TextBox txt_c6 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c6");

                TextBox txt_c7 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c7");
                TextBox txt_c8 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c8");
                TextBox txt_c9 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c9");

                grdDetail3.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_c1.Text = dt.Rows[i]["txt_c1"].ToString();
                txt_c2.Text = dt.Rows[i]["txt_c2"].ToString();
                txt_c3.Text = dt.Rows[i]["txt_c3"].ToString();
                txt_c4.Text = dt.Rows[i]["txt_c4"].ToString();
                txt_c5.Text = dt.Rows[i]["txt_c5"].ToString();
                txt_c6.Text = dt.Rows[i]["txt_c6"].ToString();

                txt_c7.Text = dt.Rows[i]["txt_c7"].ToString();
                txt_c8.Text = dt.Rows[i]["txt_c8"].ToString();
                txt_c9.Text = dt.Rows[i]["txt_c9"].ToString();
            }
        }

        protected void DeleteRowDetail4(int rowIndex)
        {
            GetCurrentDataDetail4();
            DataTable dt = ViewState["OtherTable4"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["OtherTable4"] = dt;
            grdDetail4.DataSource = dt;
            grdDetail4.DataBind();
            SetPreviousDataDetail4();
        }
        protected void AddRowDetail4()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["OtherTable4"] != null)
            {
                GetCurrentDataDetail4();
                dt = (DataTable)ViewState["OtherTable4"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_d1", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_d2", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_d3", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_d4", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_d5", typeof(string)));

                dt.Columns.Add(new DataColumn("txt_d6", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_d7", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_d8", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_d9", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_d1"] = string.Empty;
            dr["txt_d2"] = string.Empty;
            dr["txt_d3"] = string.Empty;
            dr["txt_d4"] = string.Empty;
            dr["txt_d5"] = string.Empty;

            dr["txt_d6"] = string.Empty;
            dr["txt_d7"] = string.Empty;
            dr["txt_d8"] = string.Empty;
            dr["txt_d9"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["OtherTable4"] = dt;
            grdDetail4.DataSource = dt;
            grdDetail4.DataBind();
            SetPreviousDataDetail4();

        }
        protected void GetCurrentDataDetail4()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_d1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_d2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_d3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_d4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_d5", typeof(string)));

            dtTable.Columns.Add(new DataColumn("txt_d6", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_d7", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_d8", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_d9", typeof(string)));
            for (int i = 0; i < grdDetail4.Rows.Count; i++)
            {
                TextBox txt_d1 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d1");
                TextBox txt_d2 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d2");
                TextBox txt_d3 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d3");
                TextBox txt_d4 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d4");
                TextBox txt_d5 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d5");

                TextBox txt_d6 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d6");
                TextBox txt_d7 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d7");
                TextBox txt_d8 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d8");
                TextBox txt_d9 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d9");

                grdDetail4.Rows[i].Cells[0].Width = 30;
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_d1"] = txt_d1.Text;
                drRow["txt_d2"] = txt_d2.Text;
                drRow["txt_d3"] = txt_d3.Text;
                drRow["txt_d4"] = txt_d4.Text;
                drRow["txt_d5"] = txt_d5.Text;

                drRow["txt_d6"] = txt_d6.Text;
                drRow["txt_d7"] = txt_d7.Text;
                drRow["txt_d8"] = txt_d8.Text;
                drRow["txt_d9"] = txt_d9.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["OtherTable4"] = dtTable;
        }
        protected void SetPreviousDataDetail4()
        {
            DataTable dt = (DataTable)ViewState["OtherTable4"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_d1 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d1");
                TextBox txt_d2 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d2");
                TextBox txt_d3 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d3");
                TextBox txt_d4 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d4");
                TextBox txt_d5 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d5");

                TextBox txt_d6 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d6");
                TextBox txt_d7 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d7");
                TextBox txt_d8 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d8");
                TextBox txt_d9 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d9");

                grdDetail4.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_d1.Text = dt.Rows[i]["txt_d1"].ToString();
                txt_d2.Text = dt.Rows[i]["txt_d2"].ToString();
                txt_d3.Text = dt.Rows[i]["txt_d3"].ToString();
                txt_d4.Text = dt.Rows[i]["txt_d4"].ToString();
                txt_d5.Text = dt.Rows[i]["txt_d5"].ToString();

                txt_d6.Text = dt.Rows[i]["txt_d6"].ToString();
                txt_d7.Text = dt.Rows[i]["txt_d7"].ToString();
                txt_d8.Text = dt.Rows[i]["txt_d8"].ToString();
                txt_d9.Text = dt.Rows[i]["txt_d9"].ToString();
            }
        }

        protected void DeleteRowDetail5(int rowIndex)
        {
            GetCurrentDataDetail5();
            DataTable dt = ViewState["OtherTable5"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["OtherTable5"] = dt;
            grdDetail5.DataSource = dt;
            grdDetail5.DataBind();
            SetPreviousDataDetail5();
        }
        protected void AddRowDetail5()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["OtherTable5"] != null)
            {
                GetCurrentDataDetail5();
                dt = (DataTable)ViewState["OtherTable5"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_e1", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_e2", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_e3", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_e4", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_e5", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_e1"] = string.Empty;
            dr["txt_e2"] = string.Empty;
            dr["txt_e3"] = string.Empty;
            dr["txt_e4"] = string.Empty;
            dr["txt_e5"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["OtherTable5"] = dt;
            grdDetail5.DataSource = dt;
            grdDetail5.DataBind();
            SetPreviousDataDetail5();

        }
        protected void GetCurrentDataDetail5()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_e1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_e2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_e3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_e4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_e5", typeof(string)));

            for (int i = 0; i < grdDetail5.Rows.Count; i++)
            {
                TextBox txt_e1 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e1");
                TextBox txt_e2 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e2");
                TextBox txt_e3 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e3");
                TextBox txt_e4 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e4");
                TextBox txt_e5 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e5");

                grdDetail5.Rows[i].Cells[0].Width = 30;
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_e1"] = txt_e1.Text;
                drRow["txt_e2"] = txt_e2.Text;
                drRow["txt_e3"] = txt_e3.Text;
                drRow["txt_e4"] = txt_e4.Text;
                drRow["txt_e5"] = txt_e5.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["OtherTable5"] = dtTable;
        }
        protected void SetPreviousDataDetail5()
        {
            DataTable dt = (DataTable)ViewState["OtherTable5"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_e1 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e1");
                TextBox txt_e2 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e2");
                TextBox txt_e3 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e3");
                TextBox txt_e4 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e4");
                TextBox txt_e5 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e5");

                grdDetail5.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_e1.Text = dt.Rows[i]["txt_e1"].ToString();
                txt_e2.Text = dt.Rows[i]["txt_e2"].ToString();
                txt_e3.Text = dt.Rows[i]["txt_e3"].ToString();
                txt_e4.Text = dt.Rows[i]["txt_e4"].ToString();
                txt_e5.Text = dt.Rows[i]["txt_e5"].ToString();
            }
        }

        protected void DeleteRowDetail6(int rowIndex)
        {
            GetCurrentDataDetail6();
            DataTable dt = ViewState["OtherTable6"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["OtherTable6"] = dt;
            grdDetail6.DataSource = dt;
            grdDetail6.DataBind();
            SetPreviousDataDetail6();
        }
        protected void AddRowDetail6()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["OtherTable6"] != null)
            {
                GetCurrentDataDetail6();
                dt = (DataTable)ViewState["OtherTable6"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_f1", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_f2", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_f3", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_f4", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_f5", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_f1"] = string.Empty;
            dr["txt_f2"] = string.Empty;
            dr["txt_f3"] = string.Empty;
            dr["txt_f4"] = string.Empty;
            dr["txt_f5"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["OtherTable6"] = dt;
            grdDetail6.DataSource = dt;
            grdDetail6.DataBind();
            SetPreviousDataDetail6();

        }
        protected void GetCurrentDataDetail6()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_f1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_f2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_f3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_f4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_f5", typeof(string)));

            for (int i = 0; i < grdDetail6.Rows.Count; i++)
            {
                TextBox txt_f1 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f1");
                TextBox txt_f2 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f2");
                TextBox txt_f3 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f3");
                TextBox txt_f4 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f4");
                TextBox txt_f5 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f5");

                grdDetail6.Rows[i].Cells[0].Width = 30;
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_f1"] = txt_f1.Text;
                drRow["txt_f2"] = txt_f2.Text;
                drRow["txt_f3"] = txt_f3.Text;
                drRow["txt_f4"] = txt_f4.Text;
                drRow["txt_f5"] = txt_f5.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["OtherTable6"] = dtTable;
        }
        protected void SetPreviousDataDetail6()
        {
            DataTable dt = (DataTable)ViewState["OtherTable6"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_f1 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f1");
                TextBox txt_f2 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f2");
                TextBox txt_f3 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f3");
                TextBox txt_f4 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f4");
                TextBox txt_f5 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f5");

                grdDetail6.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_f1.Text = dt.Rows[i]["txt_f1"].ToString();
                txt_f2.Text = dt.Rows[i]["txt_f2"].ToString();
                txt_f3.Text = dt.Rows[i]["txt_f3"].ToString();
                txt_f4.Text = dt.Rows[i]["txt_f4"].ToString();
                txt_f5.Text = dt.Rows[i]["txt_f5"].ToString();
            }
        }

        protected void DeleteRowDetail7(int rowIndex)
        {
            GetCurrentDataDetail7();
            DataTable dt = ViewState["OtherTable7"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["OtherTable7"] = dt;
            grdDetail7.DataSource = dt;
            grdDetail7.DataBind();
            SetPreviousDataDetail7();
        }
        protected void AddRowDetail7()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["OtherTable7"] != null)
            {
                GetCurrentDataDetail7();
                dt = (DataTable)ViewState["OtherTable7"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_g1", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_g2", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_g3", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_g4", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_g5", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_g1"] = string.Empty;
            dr["txt_g2"] = string.Empty;
            dr["txt_g3"] = string.Empty;
            dr["txt_g4"] = string.Empty;
            dr["txt_g5"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["OtherTable7"] = dt;
            grdDetail7.DataSource = dt;
            grdDetail7.DataBind();
            SetPreviousDataDetail7();

        }
        protected void GetCurrentDataDetail7()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_g1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_g2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_g3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_g4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_g5", typeof(string)));

            for (int i = 0; i < grdDetail7.Rows.Count; i++)
            {
                TextBox txt_g1 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g1");
                TextBox txt_g2 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g2");
                TextBox txt_g3 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g3");
                TextBox txt_g4 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g4");
                TextBox txt_g5 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g5");

                grdDetail7.Rows[i].Cells[0].Width = 30;
                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_g1"] = txt_g1.Text;
                drRow["txt_g2"] = txt_g2.Text;
                drRow["txt_g3"] = txt_g3.Text;
                drRow["txt_g4"] = txt_g4.Text;
                drRow["txt_g5"] = txt_g5.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["OtherTable7"] = dtTable;
        }
        protected void SetPreviousDataDetail7()
        {
            DataTable dt = (DataTable)ViewState["OtherTable7"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_g1 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g1");
                TextBox txt_g2 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g2");
                TextBox txt_g3 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g3");
                TextBox txt_g4 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g4");
                TextBox txt_g5 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g5");

                grdDetail7.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_g1.Text = dt.Rows[i]["txt_g1"].ToString();
                txt_g2.Text = dt.Rows[i]["txt_g2"].ToString();
                txt_g3.Text = dt.Rows[i]["txt_g3"].ToString();
                txt_g4.Text = dt.Rows[i]["txt_g4"].ToString();
                txt_g5.Text = dt.Rows[i]["txt_g5"].ToString();
            }
        }
        protected void DeleteRowDetail8(int rowIndex)
        {
            GetCurrentDataDetail8();
            DataTable dt = ViewState["OtherTable1"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["OtherTable1"] = dt;
            grdDetail8.DataSource = dt;
            grdDetail8.DataBind();
            SetPreviousDataDetail8();
        }
        protected void AddRowDetail8(int rowIndex)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            int count = grdDetail8.Rows.Count;

            if (ViewState["OtherTable1"] != null)
            {
                GetCurrentDataDetail8();
                dt = (DataTable)ViewState["OtherTable1"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_a9", typeof(string)));
                dt.Columns.Add(new DataColumn("lblMergFlag", typeof(string)));


            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_a1"] = string.Empty;
            dr["txt_a2"] = string.Empty;
            dr["txt_a3"] = string.Empty;
            dr["txt_a4"] = string.Empty;
            dr["txt_a5"] = string.Empty;
            dr["txt_a6"] = string.Empty;
            dr["txt_a7"] = string.Empty;
            dr["txt_a8"] = string.Empty;
            dr["txt_a9"] = string.Empty;
            dr["lblMergFlag"] = string.Empty;


            if ((rowIndex + 1) != count)
                dt.Rows.InsertAt(dr, rowIndex);
            else
                dt.Rows.Add(dr);

            ViewState["OtherTable1"] = dt;
            grdDetail8.DataSource = dt;
            grdDetail8.DataBind();

            SetPreviousDataDetail8();

        }
        protected void GetCurrentDataDetail8()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a5", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a6", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a7", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a8", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_a9", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblMergFlag", typeof(string)));


            for (int i = 0; i < grdDetail8.Rows.Count; i++)
            {
                TextBox txt_a1 = (TextBox)grdDetail8.Rows[i].Cells[4].FindControl("txt_a1");
                TextBox txt_a2 = (TextBox)grdDetail8.Rows[i].Cells[5].FindControl("txt_a2");
                TextBox txt_a3 = (TextBox)grdDetail8.Rows[i].Cells[6].FindControl("txt_a3");
                TextBox txt_a4 = (TextBox)grdDetail8.Rows[i].Cells[7].FindControl("txt_a4");
                TextBox txt_a5 = (TextBox)grdDetail8.Rows[i].Cells[8].FindControl("txt_a5");
                TextBox txt_a6 = (TextBox)grdDetail8.Rows[i].Cells[9].FindControl("txt_a6");
                TextBox txt_a7 = (TextBox)grdDetail8.Rows[i].Cells[10].FindControl("txt_a7");
                TextBox txt_a8 = (TextBox)grdDetail8.Rows[i].Cells[11].FindControl("txt_a8");
                TextBox txt_a9 = (TextBox)grdDetail8.Rows[i].Cells[12].FindControl("txt_a9");
                Label lblMergFlag = (Label)grdDetail8.Rows[i].FindControl("lblMergFlag");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_a1"] = txt_a1.Text;
                drRow["txt_a2"] = txt_a2.Text;
                drRow["txt_a3"] = txt_a3.Text;
                drRow["txt_a4"] = txt_a4.Text;
                drRow["txt_a5"] = txt_a5.Text;
                drRow["txt_a6"] = txt_a6.Text;
                drRow["txt_a7"] = txt_a7.Text;
                drRow["txt_a8"] = txt_a8.Text;
                drRow["txt_a9"] = txt_a9.Text;

                drRow["lblMergFlag"] = lblMergFlag.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["OtherTable1"] = dtTable;
        }
        protected void SetPreviousDataDetail8()
        {
            DataTable dt = (DataTable)ViewState["OtherTable1"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_a1 = (TextBox)grdDetail8.Rows[i].Cells[4].FindControl("txt_a1");
                TextBox txt_a2 = (TextBox)grdDetail8.Rows[i].Cells[5].FindControl("txt_a2");
                TextBox txt_a3 = (TextBox)grdDetail8.Rows[i].Cells[6].FindControl("txt_a3");
                TextBox txt_a4 = (TextBox)grdDetail8.Rows[i].Cells[7].FindControl("txt_a4");
                TextBox txt_a5 = (TextBox)grdDetail8.Rows[i].Cells[8].FindControl("txt_a5");
                TextBox txt_a6 = (TextBox)grdDetail8.Rows[i].Cells[9].FindControl("txt_a6");
                TextBox txt_a7 = (TextBox)grdDetail8.Rows[i].Cells[10].FindControl("txt_a7");
                TextBox txt_a8 = (TextBox)grdDetail8.Rows[i].Cells[11].FindControl("txt_a8");
                TextBox txt_a9 = (TextBox)grdDetail8.Rows[i].Cells[12].FindControl("txt_a9");

                Label lblMergFlag = (Label)grdDetail8.Rows[i].FindControl("lblMergFlag");

                grdDetail8.Rows[i].Cells[3].Text = (i + 1).ToString();

                txt_a1.Text = dt.Rows[i]["txt_a1"].ToString();
                txt_a2.Text = dt.Rows[i]["txt_a2"].ToString();
                txt_a3.Text = dt.Rows[i]["txt_a3"].ToString();
                txt_a4.Text = dt.Rows[i]["txt_a4"].ToString();
                txt_a5.Text = dt.Rows[i]["txt_a5"].ToString();
                txt_a6.Text = dt.Rows[i]["txt_a6"].ToString();
                txt_a7.Text = dt.Rows[i]["txt_a7"].ToString();
                txt_a8.Text = dt.Rows[i]["txt_a8"].ToString();
                txt_a9.Text = dt.Rows[i]["txt_a9"].ToString();

                lblMergFlag.Text = dt.Rows[i]["lblMergFlag"].ToString();

            }

        }


        protected void DeleteRowOtherRemark(int rowIndex)
        {
            GetCurrentDataOtherRemark();
            DataTable dt = ViewState["OtherRemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["OtherRemarkTable"] = dt;
            grdOtherRemark.DataSource = dt;
            grdOtherRemark.DataBind();
            SetPreviousDataOtherRemark();
        }
        protected void AddRowOtherRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["OtherRemarkTable"] != null)
            {
                GetCurrentDataOtherRemark();
                dt = (DataTable)ViewState["OtherRemarkTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_REMARK"] = string.Empty;
            dr["ddlRefType"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["OtherRemarkTable"] = dt;
            grdOtherRemark.DataSource = dt;
            grdOtherRemark.DataBind();
            SetPreviousDataOtherRemark();
        }
        protected void GetCurrentDataOtherRemark()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

            for (int i = 0; i < grdOtherRemark.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdOtherRemark.Rows[i].FindControl("txt_REMARK");
                DropDownList ddlRefType = (DropDownList)grdOtherRemark.Rows[i].FindControl("ddlRefType");


                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_REMARK"] = txt_REMARK.Text;
                drRow["ddlRefType"] = ddlRefType.SelectedItem.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["OtherRemarkTable"] = dtTable;

        }
        protected void SetPreviousDataOtherRemark()
        {
            DataTable dt = (DataTable)ViewState["OtherRemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_REMARK = (TextBox)grdOtherRemark.Rows[i].FindControl("txt_REMARK");
                DropDownList ddlRefType = (DropDownList)grdOtherRemark.Rows[i].FindControl("ddlRefType");

                grdOtherRemark.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_REMARK.Text = dt.Rows[i]["txt_REMARK"].ToString();
            }
        }
        #endregion

        protected void ddl_ReportFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayColumnsPerTest();
            grdDetail1.DataSource = null;
            grdDetail1.DataBind();
            grdDetail2.DataSource = null;
            grdDetail2.DataBind();
            grdDetail3.DataSource = null;
            grdDetail3.DataBind();
            grdDetail4.DataSource = null;
            grdDetail4.DataBind();
            grdDetail5.DataSource = null;
            grdDetail5.DataBind();
            grdDetail6.DataSource = null;
            grdDetail6.DataBind();
            grdDetail7.DataSource = null;
            grdDetail7.DataBind();
            grdDetail8.DataSource = null;
            grdDetail8.DataBind();

            grdOtherRemark.DataSource = null;
            grdOtherRemark.DataBind();
            DisplayDefaultRptValues();
            DisplayDefaultRemarks();

            lnkCalculate.Visible = false;

            if (ddl_ReportFor.SelectedIndex == 0)
            {
                PnlDetails.Enabled = false;
                pnlRemark.Enabled = false;
            }
            else
            {
                if (ddl_ReportFor.SelectedIndex == 13 || ddl_ReportFor.SelectedIndex == 21 || ddl_ReportFor.SelectedIndex == 26 || ddl_ReportFor.SelectedIndex == 27 || ddl_ReportFor.SelectedIndex == 29 ||
                ddl_ReportFor.SelectedIndex == 30 || ddl_ReportFor.SelectedIndex == 31 || ddl_ReportFor.SelectedIndex == 32 || ddl_ReportFor.SelectedIndex == 33 || ddl_ReportFor.SelectedIndex == 34)
                {
                    lnkCalculate.Visible = true;
                }

                PnlDetails.Enabled = true;
                pnlRemark.Enabled = true;
                DisplayDefaultRptValues();
            }

            

        }
        protected void lnk_Fetch_Click(object sender, EventArgs e)
        {
            if (ddl_OtherPendingRpt.SelectedValue != "---Select---")
            {
                lnkCalculate.Enabled = true;
                lnkSave.Enabled = true;
                lnkPrint.Visible = false;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                txt_ReferenceNo.Text = ddl_OtherPendingRpt.SelectedItem.Text;
                DeleteFileUploadData();
                DisplayOtherDetails();
                LoadReferenceNoList();
                ViewWitnessBy();
                LoadApproveBy();
                Session["FileUpload1"] = null;
            }
        }


        public void DisplayRemark()
        {
            int i = 0;
            var re = dc.AllRemark_View("", Convert.ToString(txt_ReferenceNo.Text), 0, "OT");
            foreach (var r in re)
            {
                AddRowOtherRemark();
                TextBox txt_REMARK = (TextBox)grdOtherRemark.Rows[i].FindControl("txt_REMARK");
                DropDownList ddlRefType = (DropDownList)grdOtherRemark.Rows[i].FindControl("ddlRefType");
                Label lblType = (Label)grdOtherRemark.Rows[i].FindControl("lblType");
                lblType.Text = r.OTDetail_RemarkType_var;
                ddlRefType.SelectedValue = r.OTDetail_RemarkType_var;
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.OTDetail_RemarkId_int), "OT");
                foreach (var rem in remark)
                {
                    txt_REMARK.Text = rem.OT_Remark_var.ToString();
                    i++;
                }
            }
            if (grdOtherRemark.Rows.Count <= 0)
            {
                AddRowOtherRemark();
            }
        }
        public void DisplayOtherDetails()
        {

            int rptStatus = 0;
            var wInwd = dc.ReportStatus_View("Other Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
            foreach (var w in wInwd)
            {
                txt_ReferenceNo.Text = w.OTINWD_ReferenceNo_var.ToString();
                if (w.OTINWD_TestedDate_dt.ToString() != null && w.OTINWD_TestedDate_dt.ToString() != "")
                {
                    txt_DtOfTesting.Text = Convert.ToDateTime(w.OTINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                }
                if (txt_DtOfTesting.Text == "" || lblEntry.Text == "Enter")
                {
                    txt_DtOfTesting.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }

                if (ddl_NablScope.Items.FindByValue(w.OTINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(w.OTINWD_NablScope_var);
                }
                if (Convert.ToString(w.OTINWD_NablLocation_int) != null && Convert.ToString(w.OTINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(w.OTINWD_NablLocation_int);
                }

                //  LoadOtherTest();
                // LoadOtherTestList();

                txt_RecordType.Text = w.OTINWD_RecordType_var.ToString();
                txt_ReportNo.Text = w.OTINWD_SetOfRecord_var.ToString();
                ddl_ReportFor.SelectedValue = w.OTINWD_ReportForTestId_int.ToString();
                if (ddl_ReportFor.SelectedIndex == 13 || ddl_ReportFor.SelectedIndex == 21 || ddl_ReportFor.SelectedIndex == 26 || ddl_ReportFor.SelectedIndex == 27 || ddl_ReportFor.SelectedIndex == 29 ||
                ddl_ReportFor.SelectedIndex == 30 || ddl_ReportFor.SelectedIndex == 31 || ddl_ReportFor.SelectedIndex == 32 || ddl_ReportFor.SelectedIndex == 33 || ddl_ReportFor.SelectedIndex == 34)
                {
                    lnkCalculate.Visible = true;
                }
                txt_Description.Text = w.OTINWD_Description_var.ToString();
                txt_SupplierName.Text = w.OTINWD_SupplierName_var.ToString();

                if (w.OTINWD_RptTitle_var != null)
                    txtHeading.Text = w.OTINWD_RptTitle_var.ToString();

                ////if (Convert.ToString(w.OTINWD_TestCategoryId_int )!= null && Convert.ToString(w.OTINWD_TestCategoryId_int) != "")
                //// if (w.OTINWD_TestedBy_tint != null)

                if (w.OTINWD_TestCategoryId_int != null && w.OTINWD_TestCategoryId_int != 0)
                {
                    ddl_Category.SelectedValue = w.OTINWD_TestCategoryId_int.ToString();
                }

                //if (w.OTINWD_TestedBy_tint != null && w.OTINWD_TestedBy_tint != 0)
                //{
                //   ddl_TestedBy.SelectedValue = w.OTINWD_TestedBy_tint.ToString();
                //}
                rptStatus = Convert.ToInt32(w.OTINWD_Status_tint);
                if (w.OTINWD_Section_var != null && w.OTINWD_Section_var != "")
                    ddlSection.SelectedValue = w.OTINWD_Section_var;
            }


            var fileupl = dc.OtherReport_View(txt_ReferenceNo.Text).ToList();
            foreach (var f in fileupl)
            {
                lblFileName.Text = f.OTRPT_FileName_var;
                lblFileName.Visible = true;
            }
            DisplayColumnsPerTest();

            string[] OtherDeatils;
            string[] grdCheckedStatus;
            string[] grdRow;
            string[] grdCol;
            int grdCnt = 0;
            DataTable dt1 = new DataTable();
            DataRow dr = null;
            dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
            dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
            dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
            dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
            dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
            dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
            dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
            dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
            dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

            DataTable dt2 = new DataTable();
            dt2.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dt2.Columns.Add(new DataColumn("txt_b1", typeof(string)));
            dt2.Columns.Add(new DataColumn("txt_b2", typeof(string)));
            dt2.Columns.Add(new DataColumn("txt_b3", typeof(string)));
            dt2.Columns.Add(new DataColumn("txt_b4", typeof(string)));
            dt2.Columns.Add(new DataColumn("txt_b5", typeof(string)));
            dt2.Columns.Add(new DataColumn("txt_b6", typeof(string)));
            dt2.Columns.Add(new DataColumn("txt_b7", typeof(string)));
            dt2.Columns.Add(new DataColumn("txt_b8", typeof(string)));
            dt2.Columns.Add(new DataColumn("txt_b9", typeof(string)));
            dt2.Columns.Add(new DataColumn("txt_b10", typeof(string)));
            dt2.Columns.Add(new DataColumn("txt_b11", typeof(string)));

            DataTable dt3 = new DataTable();
            dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dt3.Columns.Add(new DataColumn("txt_c1", typeof(string)));
            dt3.Columns.Add(new DataColumn("txt_c2", typeof(string)));
            dt3.Columns.Add(new DataColumn("txt_c3", typeof(string)));
            dt3.Columns.Add(new DataColumn("txt_c4", typeof(string)));
            dt3.Columns.Add(new DataColumn("txt_c5", typeof(string)));
            dt3.Columns.Add(new DataColumn("txt_c6", typeof(string)));

            dt3.Columns.Add(new DataColumn("txt_c7", typeof(string)));
            dt3.Columns.Add(new DataColumn("txt_c8", typeof(string)));
            dt3.Columns.Add(new DataColumn("txt_c9", typeof(string)));

            DataTable dt4 = new DataTable();
            dt4.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dt4.Columns.Add(new DataColumn("txt_d1", typeof(string)));
            dt4.Columns.Add(new DataColumn("txt_d2", typeof(string)));
            dt4.Columns.Add(new DataColumn("txt_d3", typeof(string)));
            dt4.Columns.Add(new DataColumn("txt_d4", typeof(string)));
            dt4.Columns.Add(new DataColumn("txt_d5", typeof(string)));

            dt4.Columns.Add(new DataColumn("txt_d6", typeof(string)));
            dt4.Columns.Add(new DataColumn("txt_d7", typeof(string)));
            dt4.Columns.Add(new DataColumn("txt_d8", typeof(string)));
            dt4.Columns.Add(new DataColumn("txt_d9", typeof(string)));

            DataTable dt5 = new DataTable();
            dt5.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dt5.Columns.Add(new DataColumn("txt_e1", typeof(string)));
            dt5.Columns.Add(new DataColumn("txt_e2", typeof(string)));
            dt5.Columns.Add(new DataColumn("txt_e3", typeof(string)));
            dt5.Columns.Add(new DataColumn("txt_e4", typeof(string)));
            dt5.Columns.Add(new DataColumn("txt_e5", typeof(string)));

            DataTable dt6 = new DataTable();
            dt6.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dt6.Columns.Add(new DataColumn("txt_f1", typeof(string)));
            dt6.Columns.Add(new DataColumn("txt_f2", typeof(string)));
            dt6.Columns.Add(new DataColumn("txt_f3", typeof(string)));
            dt6.Columns.Add(new DataColumn("txt_f4", typeof(string)));
            dt6.Columns.Add(new DataColumn("txt_f5", typeof(string)));

            DataTable dt7 = new DataTable();
            dt7.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dt7.Columns.Add(new DataColumn("txt_g1", typeof(string)));
            dt7.Columns.Add(new DataColumn("txt_g2", typeof(string)));
            dt7.Columns.Add(new DataColumn("txt_g3", typeof(string)));
            dt7.Columns.Add(new DataColumn("txt_g4", typeof(string)));
            dt7.Columns.Add(new DataColumn("txt_g5", typeof(string)));

            DataTable dt8 = new DataTable();
            dt8.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dt8.Columns.Add(new DataColumn("txt_a1", typeof(string)));
            dt8.Columns.Add(new DataColumn("txt_a2", typeof(string)));
            dt8.Columns.Add(new DataColumn("txt_a3", typeof(string)));
            dt8.Columns.Add(new DataColumn("txt_a4", typeof(string)));
            dt8.Columns.Add(new DataColumn("txt_a5", typeof(string)));
            dt8.Columns.Add(new DataColumn("txt_a6", typeof(string)));
            dt8.Columns.Add(new DataColumn("txt_a7", typeof(string)));
            dt8.Columns.Add(new DataColumn("txt_a8", typeof(string)));
            dt8.Columns.Add(new DataColumn("txt_a9", typeof(string)));
            dt8.Columns.Add(new DataColumn("lblMergFlag", typeof(string)));


            var OTtest = dc.AllInwdDetails_View(txt_ReferenceNo.Text, "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "", 0, "OT").ToList();
            if (OTtest.Count > 0)
            {
                foreach (var t in OTtest)
                {
                    if (ddl_ReportFor.SelectedValue == "21")
                    {
                        chkBox1.Checked = true;
                        chkBox8.Checked = true;
                    }
                    else if (Convert.ToString(t.OTTEST_GridCheckedStatus_var) != null)
                    {
                        grdCheckedStatus = Convert.ToString(t.OTTEST_GridCheckedStatus_var).Split('|');
                        for (int i = 0; i < grdCheckedStatus.Length; i++)
                        {
                            if (grdCheckedStatus[i] == "1")
                            {
                                if (i == 0)
                                    chkBox1.Checked = true;
                                else if (i == 1)
                                    chkBox2.Checked = true;
                                else if (i == 2)
                                    chkBox3.Checked = true;
                                else if (i == 3)
                                    chkBox4.Checked = true;
                                else if (i == 4)
                                    chkBox5.Checked = true;
                                else if (i == 5)
                                    chkBox6.Checked = true;
                                else if (i == 6)
                                    chkBox7.Checked = true;
                                else if (i == 7)
                                    chkBox8.Checked = true;

                            }
                            else
                            {
                                if (i == 0)
                                    chkBox1.Checked = false;
                                else if (i == 1)
                                    chkBox2.Checked = false;
                                else if (i == 2)
                                    chkBox3.Checked = false;
                                else if (i == 3)
                                    chkBox4.Checked = false;
                                else if (i == 4)
                                    chkBox5.Checked = false;
                                else if (i == 5)
                                    chkBox6.Checked = false;
                                else if (i == 6)
                                    chkBox7.Checked = false;
                                else if (i == 7)
                                    chkBox8.Checked = false;
                            }
                        }
                    }

                    OtherDeatils = t.OTDETAIL_DetailTest_var.Split('$');
                    var Count = OtherDeatils.Count();

                    if (ddl_ReportFor.SelectedValue == "21" || (ddl_ReportFor.SelectedValue == "22"))
                    {
                        foreach (string grdDeatils in OtherDeatils)
                        {
                            int count = 0;
                            grdCnt++;
                            if (ddl_ReportFor.SelectedValue == "22")
                                grdCnt++;
                            grdRow = grdDeatils.Split('|');
                            foreach (string rowData in grdRow)
                            {
                                if (rowData != "")
                                {
                                    grdCol = rowData.Split('~');
                                    foreach (string rowData1 in grdRow)
                                    {
                                        if (rowData1 != "")
                                        {
                                            grdCol = rowData1.Split('~');
                                            if (grdCnt == 1)
                                            {
                                                dr = dt1.NewRow();
                                                dr["lblSrNo"] = dt1.Rows.Count + 1;
                                                if (grdCol.Count() > 0)
                                                    dr["txt_a1"] = grdCol[0];
                                                if (grdCol.Count() > 1)
                                                    dr["txt_a2"] = grdCol[1];
                                                if (grdCol.Count() > 2)
                                                    dr["txt_a3"] = grdCol[2];
                                                if (grdCol.Count() > 3)
                                                    dr["txt_a4"] = grdCol[3];
                                                if (grdCol.Count() > 4)
                                                    dr["txt_a5"] = grdCol[4];
                                                if (grdCol.Count() > 5)
                                                    dr["txt_a6"] = grdCol[5];
                                                if (grdCol.Count() > 6)
                                                    dr["txt_a7"] = grdCol[6];
                                                if (grdCol.Count() > 7)
                                                    dr["txt_a8"] = grdCol[7];
                                                if (grdCol.Count() > 8)
                                                    dr["txt_a9"] = grdCol[8];
                                                dt1.Rows.Add(dr);
                                            }
                                            else if (grdCnt == 2)
                                            {
                                                if (grdCol[0] == "0")
                                                {
                                                    count++;
                                                    dr = dt8.NewRow();
                                                    dr["lblSrNo"] = count;
                                                    if (grdCol.Count() > 1)
                                                        dr["txt_a1"] = grdCol[1];
                                                    if (grdCol.Count() > 2)
                                                        dr["txt_a2"] = grdCol[2];
                                                    if (grdCol.Count() > 3)
                                                        dr["txt_a3"] = grdCol[3];
                                                    if (grdCol.Count() > 4)
                                                        dr["txt_a4"] = grdCol[4];
                                                    if (grdCol.Count() > 5)
                                                        dr["txt_a5"] = grdCol[5];
                                                    if (grdCol.Count() > 6)
                                                        dr["txt_a7"] = grdCol[6];
                                                    if (grdCol.Count() > 7)
                                                        dr["txt_a8"] = grdCol[7];
                                                    if (grdCol.Count() > 8)
                                                        dr["txt_a9"] = grdCol[8];
                                                    dt8.Rows.Add(dr);
                                                }
                                                else if (grdCol[0] == "1")
                                                {
                                                    dr = dt8.NewRow();
                                                    dr["lblMergFlag"] = "1";
                                                    dr["txt_a1"] = grdCol[1];
                                                    dt8.Rows.Add(dr);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                                break;
                            }
                        }
                    }
                    #region 22 commented
                    //else if (ddl_ReportFor.SelectedValue == "22")
                    //{
                    //    foreach (string grdDeatils in OtherDeatils)
                    //    {
                    //        int count = 0;
                    //        grdCnt++;
                    //        count = 0;

                    //        grdRow = grdDeatils.Split('|');
                    //        foreach (string rowData in grdRow)
                    //        {
                    //            if (rowData != "")
                    //            {
                    //                grdCol = rowData.Split('~');
                    //                foreach (string rowData1 in grdRow)
                    //                {
                    //                    if (rowData1 != "")
                    //                    {
                    //                        grdCol = rowData1.Split('~');
                    //                        for (int i = 0; i <= grdDetail8.Rows.Count; i++)
                    //                        {
                    //                            grdDetail8.Rows[i].Cells[4].ColumnSpan = 4;
                    //                            grdDetail8.Rows[i].Cells[4].Attributes.Add("colspan", rowData1);
                    //                            break;
                    //                        }
                    //                        foreach (string colData in grdCol)
                    //                        {
                    //                            if (colData == "0")
                    //                            {
                    //                                count++;
                    //                                dr = dt8.NewRow();
                    //                                dr["lblSrNo"] = count;
                    //                            }
                    //                            if (Convert.ToString(OTtest[0].OTTEST_ReferenceNo_var) != "")
                    //                            {
                    //                                if (colData == "1")
                    //                                {
                    //                                    foreach (string colData1 in grdCol)
                    //                                    {
                    //                                        if (colData1 != "1")
                    //                                        {
                    //                                            for (int i = 0; i <= grdDetail8.Rows.Count; i++)
                    //                                            {
                    //                                                dr = dt8.NewRow();
                    //                                                if (grdCol.Count() > 9)
                    //                                                    dr["lblMergFlag"] = grdCol[9];
                    //                                                if (grdCol.Count() > 1)
                    //                                                    dr["txt_a1"] = grdCol[1];
                    //                                                dt8.Rows.Add(dr);
                    //                                                break;
                    //                                            }
                    //                                            break;
                    //                                        }
                    //                                    }
                    //                                    break;
                    //                                }
                    //                                else
                    //                                {
                    //                                    if (colData != "0" && colData != "")
                    //                                    {
                    //                                        dr = dt8.NewRow();
                    //                                        if (grdCol.Count() > 1)
                    //                                            dr["txt_a1"] = grdCol[1];
                    //                                        if (grdCol.Count() > 2)
                    //                                            dr["txt_a2"] = grdCol[2];
                    //                                        if (grdCol.Count() > 3)
                    //                                            dr["txt_a3"] = grdCol[3];
                    //                                        if (grdCol.Count() > 4)
                    //                                            dr["txt_a4"] = grdCol[4];
                    //                                        if (grdCol.Count() > 5)
                    //                                            dr["txt_a5"] = grdCol[5];
                    //                                        if (grdCol.Count() > 6)
                    //                                            dr["txt_a7"] = grdCol[6];
                    //                                        if (grdCol.Count() > 7)
                    //                                            dr["txt_a8"] = grdCol[7];
                    //                                        if (grdCol.Count() > 8)
                    //                                            dr["txt_a9"] = grdCol[8];
                    //                                        dt8.Rows.Add(dr);
                    //                                        break;
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //                break;
                    //            }
                    //            break;
                    //        }
                    //    }
                    //}
                    #endregion
                    else
                    {
                        foreach (string grdDeatils1 in OtherDeatils)
                        {
                            int count1 = 0;
                            grdCnt++;
                            grdRow = grdDeatils1.Split('|');
                            foreach (string rowData1 in grdRow)
                            {
                                if (rowData1 != "")
                                {
                                    grdCol = rowData1.Split('~');
                                    if (grdCnt == 1 && ddl_ReportFor.SelectedValue == "34" 
                                        && rowData1.Contains("The Average Modulus of Elasticity of the sample") == true)
                                    {
                                        lblHeading5.Text = rowData1;
                                        lblHeading5.Visible = true; 
                                    }
                                    else if (grdCnt == 1 && ddl_ReportFor.SelectedValue != "22")
                                    {
                                        dr = dt1.NewRow();
                                        dr["lblSrNo"] = dt1.Rows.Count + 1;
                                        if (grdCol.Count() > 0)
                                            dr["txt_a1"] = grdCol[0];
                                        if (grdCol.Count() > 1)
                                            dr["txt_a2"] = grdCol[1];
                                        if (grdCol.Count() > 2)
                                            dr["txt_a3"] = grdCol[2];
                                        if (grdCol.Count() > 3)
                                            dr["txt_a4"] = grdCol[3];
                                        if (grdCol.Count() > 4)
                                            dr["txt_a5"] = grdCol[4];
                                        if (grdCol.Count() > 5)
                                            dr["txt_a6"] = grdCol[5];
                                        if (grdCol.Count() > 6)
                                            dr["txt_a7"] = grdCol[6];
                                        if (grdCol.Count() > 7)
                                            dr["txt_a8"] = grdCol[7];
                                        if (grdCol.Count() > 8)
                                            dr["txt_a9"] = grdCol[8];
                                        dt1.Rows.Add(dr);
                                    }
                                    else if (grdCnt == 2)
                                    {
                                        dr = dt2.NewRow();
                                        dr["lblSrNo"] = dt2.Rows.Count + 1;
                                        if (grdCol.Count() > 0)
                                            dr["txt_b1"] = grdCol[0];
                                        if (grdCol.Count() > 1)
                                            dr["txt_b2"] = grdCol[1];
                                        if (grdCol.Count() > 2)
                                            dr["txt_b3"] = grdCol[2];
                                        if (grdCol.Count() > 3)
                                            dr["txt_b4"] = grdCol[3];
                                        if (grdCol.Count() > 4)
                                            dr["txt_b5"] = grdCol[4];
                                        if (grdCol.Count() > 5)
                                            dr["txt_b6"] = grdCol[5];
                                        if (grdCol.Count() > 6)
                                            dr["txt_b7"] = grdCol[6];
                                        if (grdCol.Count() > 7)
                                            dr["txt_b8"] = grdCol[7];
                                        if (grdCol.Count() > 8)
                                            dr["txt_b9"] = grdCol[8];
                                        if (grdCol.Count() > 9)
                                            dr["txt_b10"] = grdCol[9];
                                        if (grdCol.Count() > 10)
                                            dr["txt_b11"] = grdCol[10];
                                        dt2.Rows.Add(dr);
                                    }
                                    else if (grdCnt == 3)
                                    {
                                        if (ddl_ReportFor.SelectedValue == "36")
                                        {
                                            txtDiameterOfSample.Text = grdCol[0];
                                            txtTemperatureDuringTest.Text = grdCol[1];
                                        }
                                        else
                                        {
                                            dr = dt3.NewRow();
                                            dr["lblSrNo"] = dt3.Rows.Count + 1;
                                            if (grdCol.Count() > 0)
                                                dr["txt_c1"] = grdCol[0];
                                            if (grdCol.Count() > 1)
                                                dr["txt_c2"] = grdCol[1];
                                            if (grdCol.Count() > 2)
                                                dr["txt_c3"] = grdCol[2];
                                            if (grdCol.Count() > 3)
                                                dr["txt_c4"] = grdCol[3];
                                            if (grdCol.Count() > 4)
                                                dr["txt_c5"] = grdCol[4];
                                            if (grdCol.Count() > 5)
                                                dr["txt_c6"] = grdCol[5];

                                            if (grdCol.Count() > 6)
                                                dr["txt_c7"] = grdCol[6];
                                            if (grdCol.Count() > 7)
                                                dr["txt_c8"] = grdCol[7];
                                            if (grdCol.Count() > 8)
                                                dr["txt_c9"] = grdCol[8];
                                            dt3.Rows.Add(dr);
                                        }
                                    }
                                    else if (grdCnt == 4)
                                    {
                                        dr = dt4.NewRow();
                                        dr["lblSrNo"] = dt4.Rows.Count + 1;
                                        if (grdCol.Count() > 0)
                                            dr["txt_d1"] = grdCol[0];
                                        if (grdCol.Count() > 1)
                                            dr["txt_d2"] = grdCol[1];
                                        if (grdCol.Count() > 2)
                                            dr["txt_d3"] = grdCol[2];
                                        if (grdCol.Count() > 3)
                                            dr["txt_d4"] = grdCol[3];
                                        if (grdCol.Count() > 4)
                                            dr["txt_d5"] = grdCol[4];

                                        if (grdCol.Count() > 5)
                                            dr["txt_d6"] = grdCol[5];
                                        if (grdCol.Count() > 6)
                                            dr["txt_d7"] = grdCol[6];
                                        if (grdCol.Count() > 7)
                                            dr["txt_d8"] = grdCol[7];
                                        if (grdCol.Count() > 8)
                                            dr["txt_d9"] = grdCol[8];
                                        dt4.Rows.Add(dr);
                                    }
                                    else if (grdCnt == 5)
                                    {
                                        dr = dt5.NewRow();
                                        dr["lblSrNo"] = dt5.Rows.Count + 1;
                                        if (grdCol.Count() > 0)
                                            dr["txt_e1"] = grdCol[0];
                                        if (grdCol.Count() > 1)
                                            dr["txt_e2"] = grdCol[1];
                                        if (grdCol.Count() > 2)
                                            dr["txt_e3"] = grdCol[2];
                                        if (grdCol.Count() > 3)
                                            dr["txt_e4"] = grdCol[3];
                                        if (grdCol.Count() > 4)
                                            dr["txt_e5"] = grdCol[4];
                                        dt5.Rows.Add(dr);
                                    }
                                    else if (grdCnt == 6)
                                    {
                                        dr = dt6.NewRow();
                                        dr["lblSrNo"] = dt6.Rows.Count + 1;
                                        if (grdCol.Count() > 0)
                                            dr["txt_f1"] = grdCol[0];
                                        if (grdCol.Count() > 1)
                                            dr["txt_f2"] = grdCol[1];
                                        if (grdCol.Count() > 2)
                                            dr["txt_f3"] = grdCol[2];
                                        if (grdCol.Count() > 3)
                                            dr["txt_f4"] = grdCol[3];
                                        if (grdCol.Count() > 4)
                                            dr["txt_f5"] = grdCol[4];
                                        dt6.Rows.Add(dr);
                                    }
                                    else if (grdCnt == 7)
                                    {
                                        dr = dt7.NewRow();
                                        dr["lblSrNo"] = dt7.Rows.Count + 1;
                                        if (grdCol.Count() > 0)
                                            dr["txt_g1"] = grdCol[0];
                                        if (grdCol.Count() > 1)
                                            dr["txt_g2"] = grdCol[1];
                                        if (grdCol.Count() > 2)
                                            dr["txt_g3"] = grdCol[2];
                                        if (grdCol.Count() > 3)
                                            dr["txt_g4"] = grdCol[3];
                                        if (grdCol.Count() > 4)
                                            dr["txt_g5"] = grdCol[4];
                                        dt7.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                    }
                }
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();
                grdDetail2.DataSource = dt2;
                grdDetail2.DataBind();
                grdDetail3.DataSource = dt3;
                grdDetail3.DataBind();
                grdDetail4.DataSource = dt4;
                grdDetail4.DataBind();
                grdDetail5.DataSource = dt5;
                grdDetail5.DataBind();
                grdDetail6.DataSource = dt6;
                grdDetail6.DataBind();
                grdDetail7.DataSource = dt7;
                grdDetail7.DataBind();
                grdDetail8.DataSource = dt8;
                grdDetail8.DataBind();
                DisplayRemark();
                if (grdDetail1.Rows.Count == 0)
                    AddRowDetail1();
                if (grdDetail2.Rows.Count == 0)
                    AddRowDetail2();
                if (grdDetail3.Rows.Count == 0)
                    AddRowDetail3();
                if (grdDetail4.Rows.Count == 0)
                    AddRowDetail4();
                if (grdDetail5.Rows.Count == 0)
                    AddRowDetail5();
                if (grdDetail6.Rows.Count == 0)
                    AddRowDetail6();
                if (grdDetail7.Rows.Count == 0)
                    AddRowDetail7();
                if (grdDetail8.Rows.Count == 0)
                    AddRowDetail8(grdDetail8.Rows.Count);
                else
                    ShowMergeRow();
                //grdDetail1.Rows[0].Cells[3].Width = 100;
                //grdDetail1.Columns[5].ItemStyle.Width = 100;
                if (ddl_ReportFor.SelectedValue == "34")
                {
                    int noOfSpecimens = 0;
                    TextBox txtNoOfSpecimens = (TextBox)grdDetail1.Rows[1].FindControl("txt_a7");
                    if (txtNoOfSpecimens.Text != "" && Int32.TryParse(txtNoOfSpecimens.Text, out noOfSpecimens))
                    {
                        txtNoOfSpecimens.Text = "1";
                        chkBox3.Checked = false;
                        chkBox3.Visible = false;
                        lblHeading3.Visible = false;
                        grdDetail3.Visible = false;
                        chkBox4.Checked = false;
                        chkBox4.Visible = false;
                        lblHeading4.Visible = false;
                        grdDetail4.Visible = false;

                        if (noOfSpecimens >= 3)
                        {
                            txtNoOfSpecimens.Text = "3";
                            chkBox3.Checked = true;
                            chkBox3.Visible = true;
                            lblHeading3.Visible = true;
                            grdDetail3.Visible = true;
                            chkBox4.Checked = true;
                            chkBox4.Visible = true;
                            lblHeading4.Visible = true;
                            grdDetail4.Visible = true;
                        }
                        else if (noOfSpecimens == 2)
                        {
                            chkBox3.Checked = true;
                            chkBox3.Visible = true;
                            lblHeading3.Visible = true;
                            grdDetail3.Visible = true;
                        }
                    }

                }
            }
            else
            {
                DisplayDefaultRptValues();
                DisplayDefaultRemarks();
            }
            if (ddl_ReportFor.SelectedIndex == 21 || ddl_ReportFor.SelectedIndex == 26 || ddl_ReportFor.SelectedIndex == 27 || ddl_ReportFor.SelectedIndex == 29 ||
             ddl_ReportFor.SelectedIndex == 30 || ddl_ReportFor.SelectedIndex == 31 || ddl_ReportFor.SelectedIndex == 32 || ddl_ReportFor.SelectedIndex == 33 || ddl_ReportFor.SelectedIndex == 34) 
            {
                lnkCalculate.Visible = true;
            }
        }

        protected void DisplayColumnsPerTest()
        {
            lblDiameterOfSample.Visible = false;
            txtDiameterOfSample.Visible = false;
            lblTemperatureDuringTest.Visible = false;
            txtTemperatureDuringTest.Visible = false;
            txtDiameterOfSample.Text = "";
            txtTemperatureDuringTest.Text = "";

            chkBox1.Visible = false;
            chkBox2.Visible = false;
            chkBox3.Visible = false;
            chkBox4.Visible = false;
            chkBox5.Visible = false;
            chkBox6.Visible = false;
            chkBox7.Visible = false;
            chkBox8.Visible = false;

            lblHeading1.Visible = false;
            lblHeading2.Visible = false;
            lblHeading3.Visible = false;
            lblHeading4.Visible = false;
            lblHeading5.Visible = false;
            lblHeading6.Visible = false;
            lblHeading7.Visible = false;
            lblHeading8.Visible = false;

            grdDetail1.Visible = false;
            grdDetail2.Visible = false;
            grdDetail3.Visible = false;
            grdDetail4.Visible = false;
            grdDetail5.Visible = false;
            grdDetail6.Visible = false;
            grdDetail7.Visible = false;
            grdDetail8.Visible = false;

            lblHeading1.Text = "";
            lblHeading2.Text = "";
            lblHeading3.Text = "";
            lblHeading4.Text = "";
            lblHeading5.Text = "";
            lblHeading6.Text = "";
            lblHeading7.Text = "";
            lblHeading8.Text = "";

            for (int i = 3; i < grdDetail1.Columns.Count; i++)
            {
                grdDetail1.Columns[i].Visible = false;
            }
            for (int i = 3; i < grdDetail2.Columns.Count; i++)
            {
                grdDetail2.Columns[i].Visible = false;
            }
            for (int i = 3; i < grdDetail3.Columns.Count; i++)
            {
                grdDetail3.Columns[i].Visible = false;
            }
            for (int i = 3; i < grdDetail4.Columns.Count; i++)
            {
                grdDetail4.Columns[i].Visible = false;
            }
            for (int i = 3; i < grdDetail5.Columns.Count; i++)
            {
                grdDetail5.Columns[i].Visible = false;
            }
            for (int i = 3; i < grdDetail6.Columns.Count; i++)
            {
                grdDetail6.Columns[i].Visible = false;
            }
            for (int i = 3; i < grdDetail7.Columns.Count; i++)
            {
                grdDetail7.Columns[i].Visible = false;
            }

            for (int i = 4; i < grdDetail8.Columns.Count; i++)
            {
                grdDetail8.Columns[i].Visible = false;
            }

            #region 1-15-123
            if (ddl_ReportFor.SelectedValue == "1")
            {
                chkBox1.Visible = true;
                chkBox2.Visible = true;
                chkBox3.Visible = true;

                lblHeading2.Text = "Requirement as per IS 2547(Part-1) - 1976, RA-2002, Amend No.2 'Specification For Gypsum Building Plaster.' ";
                lblHeading3.Text = "Requirement as per IS 2547(Part-1) - 1976, RA-2002, 'Specification For Gypsum Building Plaster.' ";

                lblHeading2.Visible = true;
                lblHeading3.Visible = true;
                grdDetail1.Visible = true;
                grdDetail2.Visible = true;
                grdDetail3.Visible = true;
                grdDetail1.Width = 3 * 200;
                grdDetail2.Width = 6 * 100;
                grdDetail3.Width = 6 * 100;
                for (int i = 3; i <= 5; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 8; i++)
                {
                    grdDetail2.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 8; i++)
                {
                    grdDetail3.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Test";
                grdDetail1.Columns[4].HeaderText = "Unit";
                grdDetail1.Columns[5].HeaderText = "Test Results";

                grdDetail2.Columns[3].HeaderText = "Test";
                grdDetail2.Columns[4].HeaderText = "Unit";
                grdDetail2.Columns[5].HeaderText = "Type A (short time seting)";
                grdDetail2.Columns[6].HeaderText = "Type A (Long time seting)";
                grdDetail2.Columns[7].HeaderText = "Anhydrous Gypsum plaster";
                grdDetail2.Columns[8].HeaderText = "Keene's Plaster";

                grdDetail3.Columns[3].HeaderText = "Test";
                grdDetail3.Columns[4].HeaderText = "Unit";
                grdDetail3.Columns[5].HeaderText = "Browning Plaster";
                grdDetail3.Columns[6].HeaderText = "Metal Lathing Plaster";
                grdDetail3.Columns[7].HeaderText = "Bonding Plaster";
                grdDetail3.Columns[8].HeaderText = "Final Coat Plaster (Type B), Finish Plaster";
            }
            #endregion
            #region 2-16-124
            else if (ddl_ReportFor.SelectedValue == "2")
            {
                chkBox1.Visible = true;
                chkBox2.Visible = true;

                lblHeading1.Text = "A) Chemical Analysis";
                lblHeading2.Text = "B) Specifications: IS 2547(Part - 1) - 1976 :'Specification For Gypsum Building Plaster.'";
                lblHeading2.Visible = true;
                lblHeading3.Visible = true;
                grdDetail1.Visible = true;
                grdDetail2.Visible = true;
                grdDetail1.Width = 3 * 200;
                grdDetail2.Width = 6 * 100;

                for (int i = 3; i <= 5; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 8; i++)
                {
                    grdDetail2.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "Test";
                grdDetail1.Columns[4].HeaderText = "Unit";
                grdDetail1.Columns[5].HeaderText = "Test Results";

                grdDetail2.Columns[3].HeaderText = "Test";
                grdDetail2.Columns[4].HeaderText = "Unit";
                grdDetail2.Columns[5].HeaderText = "Plaster of paris";
                grdDetail2.Columns[6].HeaderText = "Retarded Hemihydrate Gypsum plaster";
                grdDetail2.Columns[7].HeaderText = "Anhydrous Gypsum plaster";
                grdDetail2.Columns[8].HeaderText = "Keene's Plaster";
            }
            #endregion
            #region 5-29-683
            else if (ddl_ReportFor.SelectedValue == "5")
            {
                chkBox1.Visible = true;
                chkBox2.Visible = true;

                lblHeading2.Text = "Classification of Rock Based on Compressive Strength";
                lblHeading2.Visible = true;
                grdDetail1.Visible = true;
                grdDetail2.Visible = true;
                grdDetail1.Width = 9 * 100;
                grdDetail2.Width = 2 * 200;

                for (int i = 3; i <= 11; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 4; i++)
                {
                    grdDetail2.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "ID Mark";
                grdDetail1.Columns[4].HeaderText = "Diameter of Core <br />(cm)";
                grdDetail1.Columns[5].HeaderText = "Length of Sample <br />(cm)";
                grdDetail1.Columns[6].HeaderText = "Oven Dry Weight <br />(gm)";
                grdDetail1.Columns[7].HeaderText = "Area of cross section <br /> (cm²)";
                grdDetail1.Columns[8].HeaderText = "Load at failure <br />(kN)";
                grdDetail1.Columns[9].HeaderText = "Comp. Strength <br /> (Kg/cm²)";
                grdDetail1.Columns[10].HeaderText = "Corrective Compressive Strength (Kg/cm²)";
                grdDetail1.Columns[11].HeaderText = "Remarks";

                grdDetail2.Columns[3].HeaderText = "Strength in N/mm²";
                grdDetail2.Columns[4].HeaderText = "Classification of Rock";
            }
            #endregion
            #region 6-30-684
            else if (ddl_ReportFor.SelectedValue == "6")
            {
                chkBox1.Visible = true;
                chkBox2.Visible = true;
                chkBox3.Visible = true;

                lblHeading1.Text = "1) Compressive strength 28 days";
                lblHeading2.Text = "2) Splitting tensile strength 28 days";
                lblHeading3.Text = "3) Pull off test 28 days test on light weight block";

                lblHeading1.Visible = true;
                lblHeading2.Visible = true;
                lblHeading3.Visible = true;
                grdDetail1.Visible = true;
                grdDetail2.Visible = true;
                grdDetail3.Visible = true;
                grdDetail1.Width = 9 * 100;
                grdDetail2.Width = 8 * 100;
                grdDetail3.Width = 6 * 100;
                for (int i = 3; i <= 11; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 10; i++)
                {
                    grdDetail2.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 8; i++)
                {
                    grdDetail3.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Age <br />(Day)";
                grdDetail1.Columns[4].HeaderText = "Length <br />(mm)";
                grdDetail1.Columns[5].HeaderText = "Breadth <br />(mm)";
                grdDetail1.Columns[6].HeaderText = "Height <br />(mm)";
                grdDetail1.Columns[7].HeaderText = "Weight <br />(g)";
                grdDetail1.Columns[8].HeaderText = "Cross Sectional Area <br />(mm²)";
                grdDetail1.Columns[9].HeaderText = "Load <br />(kN)";
                grdDetail1.Columns[10].HeaderText = "Comp. Strength <br />(N/mm²)";
                grdDetail1.Columns[11].HeaderText = "Avg. Comp. Strength <br />(N/mm²)";

                grdDetail2.Columns[3].HeaderText = "Id Mark";
                grdDetail2.Columns[4].HeaderText = "Length <br />(mm)";
                grdDetail2.Columns[5].HeaderText = "Breadth <br />(mm)";
                grdDetail2.Columns[6].HeaderText = "Weight <br />(mm)";
                grdDetail2.Columns[7].HeaderText = "Load <br />(kN)";
                grdDetail2.Columns[8].HeaderText = "Minimum Splitting Tensile Strength (MPa)";
                grdDetail2.Columns[9].HeaderText = "Maximum Splitting Tensile Strength (MPa)";
                grdDetail2.Columns[10].HeaderText = "Observation";

                grdDetail3.Columns[3].HeaderText = "Area Contact <br />(mm²)";
                grdDetail3.Columns[4].HeaderText = "Load Sustained <br />(N)";
                grdDetail3.Columns[5].HeaderText = "Bond Strength <br />(N/mm²)";
                grdDetail3.Columns[6].HeaderText = "Average Bond Strength <br />(N/mm²)";
                grdDetail3.Columns[7].HeaderText = "Observation";
                grdDetail3.Columns[8].HeaderText = "Specified BS - 1881 (Part 207) : 1993";
            }
            #endregion
            #region 7-31-685
            else if (ddl_ReportFor.SelectedValue == "7")
            {
                chkBox1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 6 * 150;
                for (int i = 3; i <= 8; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Description of Member";
                grdDetail1.Columns[4].HeaderText = "Main Bar in Length";
                grdDetail1.Columns[5].HeaderText = "Diameter of Bar (mm)*";
                grdDetail1.Columns[6].HeaderText = "Stirrups@ Specing";
                grdDetail1.Columns[7].HeaderText = "Diameter of Stirrups (mm)*";
                grdDetail1.Columns[8].HeaderText = "Average Cover (mm)#";

            }
            #endregion
            #region 8-32-689
            else if (ddl_ReportFor.SelectedValue == "8")
            {
                chkBox1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 4 * 200;
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Test Parameter";
                grdDetail1.Columns[4].HeaderText = "Result";
                grdDetail1.Columns[5].HeaderText = "Unit";
                grdDetail1.Columns[6].HeaderText = "Desirable Limits IS 10500:2012";
            }
            #endregion
            #region 3-18-126
            else if (ddl_ReportFor.SelectedValue == "3")
            {
                chkBox1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 4 * 200;
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Test Parameter";
                grdDetail1.Columns[4].HeaderText = "Result";
                grdDetail1.Columns[5].HeaderText = "Unit";
                grdDetail1.Columns[6].HeaderText = "Method of testing";
            }
            #endregion
            #region 9-33-690
            else if (ddl_ReportFor.SelectedValue == "9")
            {
                chkBox1.Visible = true;
                lblHeading1.Text = "Pull off test 7 days";
                lblHeading1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 7 * 100;

                for (int i = 3; i <= 8; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "Area Contact <br />(mm²)";
                grdDetail1.Columns[4].HeaderText = "Load Sustained <br />(N)";
                grdDetail1.Columns[5].HeaderText = "Bond Strength <br />(N/mm²)";
                grdDetail1.Columns[6].HeaderText = "Average Bond Strength <br />(N/mm²)";
                grdDetail1.Columns[7].HeaderText = "Observation";
                grdDetail1.Columns[8].HeaderText = "Specified BS - 1881 <br /> (Part 207) : 1993";
            }
            #endregion
            #region 4-21-129
            else if (ddl_ReportFor.SelectedValue == "4")
            {
                chkBox1.Visible = true;
                chkBox2.Visible = true;
                chkBox3.Visible = true;

                lblHeading1.Text = "1) Determination of Density & Moisture Content";
                lblHeading2.Text = "2) Test for Water Resistance";
                lblHeading3.Text = "3) Determination Nail & Screw Holiding Power";

                lblHeading1.Visible = true;
                lblHeading2.Visible = true;
                lblHeading3.Visible = true;
                grdDetail1.Visible = true;
                grdDetail2.Visible = true;
                grdDetail3.Visible = true;
                grdDetail1.Width = 5 * 100;
                grdDetail2.Width = 5 * 100;
                grdDetail3.Width = 5 * 100;
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail2.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail3.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Id Mark";
                grdDetail1.Columns[4].HeaderText = "Test Parameter";
                grdDetail1.Columns[5].HeaderText = "Test Results";
                grdDetail1.Columns[6].HeaderText = "Unit";

                grdDetail2.Columns[3].HeaderText = "Id Mark";
                grdDetail2.Columns[4].HeaderText = "Test Parameter";
                grdDetail2.Columns[5].HeaderText = "Test Results";
                grdDetail2.Columns[6].HeaderText = "Unit";

                grdDetail3.Columns[3].HeaderText = "Id Mark";
                grdDetail3.Columns[4].HeaderText = "Test Parameter";
                grdDetail3.Columns[5].HeaderText = "Test Results";
                grdDetail3.Columns[6].HeaderText = "Unit";
            }
            #endregion
            #region 10-34-501
            else if (ddl_ReportFor.SelectedValue == "10")
            {
                chkBox1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 4 * 200;
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Particular Test";
                grdDetail1.Columns[4].HeaderText = "Result";
                grdDetail1.Columns[5].HeaderText = "Unit";
                grdDetail1.Columns[6].HeaderText = "Specified Limit";
            }
            #endregion
            #region 11-35-691
            else if (ddl_ReportFor.SelectedValue == "11")
            {
                chkBox1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 2 * 300;
                for (int i = 3; i <= 4; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Sample Description";
                grdDetail1.Columns[4].HeaderText = "Depth of Carbonation (mm)";
            }
            #endregion
            #region 12-36-692
            else if (ddl_ReportFor.SelectedValue == "12")
            {
                chkBox1.Visible = true;
                chkBox2.Visible = true;

                lblHeading1.Text = "Half Cell Potentiometer readings are generally interpreted as follows :";
                lblHeading2.Text = "Half cell readings obtained at site ";

                lblHeading1.Visible = true;
                lblHeading2.Visible = true;
                grdDetail1.Visible = true;
                grdDetail2.Visible = true;
                grdDetail1.Width = 2 * 300;
                grdDetail2.Width = 5 * 100;
                for (int i = 3; i <= 4; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 7; i++)
                {
                    grdDetail2.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Half Cell Potential readings (mv)";
                grdDetail1.Columns[4].HeaderText = "Conclusion";

                grdDetail2.Columns[3].HeaderText = "Description / Id Mark";
                grdDetail2.Columns[4].HeaderText = "Average Half cell Potentiometer readings (mv)";
                grdDetail2.Columns[5].HeaderText = "Temp. at the time of testing";
                grdDetail2.Columns[6].HeaderText = "Temp. Coefficient (mv)";
                grdDetail2.Columns[7].HeaderText = "Corrected average Potentiometer readings (mv)";
            }
            #endregion
            #region 13-23-131
            else if (ddl_ReportFor.SelectedValue == "13")
            {
                chkBox1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 5 * 150;
                for (int i = 3; i <= 7; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Dia of Bar<br />(mm)";
                grdDetail1.Columns[4].HeaderText = "ID Mark";
                grdDetail1.Columns[5].HeaderText = "Ultimate load<br />(KN)";
                grdDetail1.Columns[6].HeaderText = "C/S Area";
                grdDetail1.Columns[7].HeaderText = "Ultimate Tensile<br />(Stress N/mm²)";

            }
            #endregion
            #region 14-3-111
            else if (ddl_ReportFor.SelectedValue == "14")
            {
                chkBox1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 5 * 150;
                for (int i = 3; i <= 5; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "Test";
                grdDetail1.Columns[4].HeaderText = "Unit";
                grdDetail1.Columns[5].HeaderText = "Test Results";

            }
            #endregion
            #region 15-37-383
            else if (ddl_ReportFor.SelectedValue == "15")
            {
                chkBox1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 5 * 150;
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "Contaminent";
                grdDetail1.Columns[4].HeaderText = "Unit";
                grdDetail1.Columns[5].HeaderText = "Results";
                grdDetail1.Columns[6].HeaderText = "Specified limits as per IS 383";

            }
            #endregion
            #region 16-10-118
            else if (ddl_ReportFor.SelectedValue == "16")
            {
                chkBox1.Visible = true;
                chkBox2.Visible = true;
                chkBox3.Visible = true;
                chkBox4.Visible = true;
                chkBox5.Visible = true;
                chkBox6.Visible = true;
                chkBox7.Visible = true;
                lblHeading1.Visible = true;
                lblHeading2.Visible = true;
                lblHeading3.Visible = true;
                lblHeading4.Visible = true;
                lblHeading5.Visible = true;
                lblHeading6.Visible = true;
                lblHeading7.Visible = true;
                grdDetail1.Visible = true;
                grdDetail2.Visible = true;
                grdDetail3.Visible = true;
                grdDetail4.Visible = true;
                grdDetail5.Visible = true;
                grdDetail6.Visible = true;
                grdDetail7.Visible = true;
                lblHeading1.Text = "1) Glue Adhesion Test";
                lblHeading2.Text = "2) Knife Test";
                lblHeading3.Text = "3) Screw Holding Power";
                lblHeading4.Text = "4) Shock Resistance Test";
                lblHeading5.Text = "5) End Immersion Test";
                lblHeading6.Text = "6) Slamming Test";
                lblHeading7.Text = "7) Misuse Test";
                grdDetail1.Width = 5 * 150;
                grdDetail2.Width = 5 * 150;
                grdDetail3.Width = 5 * 150;
                grdDetail4.Width = 5 * 150;
                grdDetail5.Width = 5 * 150;
                grdDetail6.Width = 5 * 150;
                grdDetail7.Width = 5 * 150;
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail2.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail3.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail4.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail5.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail6.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail7.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "ID mark";
                grdDetail1.Columns[4].HeaderText = "Sample";
                grdDetail1.Columns[5].HeaderText = "Observation";
                grdDetail1.Columns[6].HeaderText = "Remark";

                grdDetail2.Columns[3].HeaderText = "ID mark";
                grdDetail2.Columns[4].HeaderText = "Sample";
                grdDetail2.Columns[5].HeaderText = "Observation";
                grdDetail2.Columns[6].HeaderText = "Remark";

                grdDetail3.Columns[3].HeaderText = "ID mark";
                grdDetail3.Columns[4].HeaderText = "Sample";
                grdDetail3.Columns[5].HeaderText = "Observation";
                grdDetail3.Columns[6].HeaderText = "Remark";

                grdDetail4.Columns[3].HeaderText = "ID mark";
                grdDetail4.Columns[4].HeaderText = "Sample";
                grdDetail4.Columns[5].HeaderText = "Observation";
                grdDetail4.Columns[6].HeaderText = "Remark";

                grdDetail5.Columns[3].HeaderText = "ID mark";
                grdDetail5.Columns[4].HeaderText = "Sample";
                grdDetail5.Columns[5].HeaderText = "Observation";
                grdDetail5.Columns[6].HeaderText = "Remark";

                grdDetail6.Columns[3].HeaderText = "ID mark";
                grdDetail6.Columns[4].HeaderText = "Sample";
                grdDetail6.Columns[5].HeaderText = "Observation";
                grdDetail6.Columns[6].HeaderText = "Remark";

                grdDetail7.Columns[3].HeaderText = "ID mark";
                grdDetail7.Columns[4].HeaderText = "Sample";
                grdDetail7.Columns[5].HeaderText = "Observation";
                grdDetail7.Columns[6].HeaderText = "Remark";

            }
            #endregion
            #region 17-25-133
            else if (ddl_ReportFor.SelectedValue == "17")
            {
                chkBox1.Visible = true;
                chkBox2.Visible = true;

                lblHeading1.Text = "Observed values of elements";
                lblHeading2.Text = "Specified Composition";
                lblHeading1.Visible = true;
                lblHeading2.Visible = true;

                grdDetail1.Visible = true;
                grdDetail2.Visible = true;
                grdDetail1.Width = 9 * 60;
                grdDetail2.Width = 9 * 60;
                for (int i = 3; i <= 11; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 11; i++)
                {
                    grdDetail2.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "ID Mark";
                grdDetail1.Columns[4].HeaderText = "Size<br />(thk)mm";
                grdDetail1.Columns[5].HeaderText = "C<br /> % ";
                grdDetail1.Columns[6].HeaderText = "Mn<br /> % ";
                grdDetail1.Columns[7].HeaderText = "Cr<br /> % ";
                grdDetail1.Columns[8].HeaderText = "Ni<br /> % ";
                grdDetail1.Columns[9].HeaderText = "Si<br /> % ";
                grdDetail1.Columns[10].HeaderText = "S<br /> % ";
                grdDetail1.Columns[11].HeaderText = "P<br /> % ";


                grdDetail2.Columns[3].HeaderText = "ID Mark";
                grdDetail2.Columns[4].HeaderText = "Size<br />(thk)mm";
                grdDetail2.Columns[5].HeaderText = "C<br /> % ";
                grdDetail2.Columns[6].HeaderText = "Mn<br /> % ";
                grdDetail2.Columns[7].HeaderText = "Cr<br /> % ";
                grdDetail2.Columns[8].HeaderText = "Ni<br /> % ";
                grdDetail2.Columns[9].HeaderText = "Si\n % ";
                grdDetail2.Columns[10].HeaderText = "S\n % ";
                grdDetail2.Columns[11].HeaderText = "P\n % ";

            }
            #endregion
            #region 18-34-501
            else if (ddl_ReportFor.SelectedValue == "18")
            {
                chkBox1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 5 * 180;
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "Particular Test";
                grdDetail1.Columns[4].HeaderText = "Result";
                grdDetail1.Columns[5].HeaderText = "Unit";
                grdDetail1.Columns[6].HeaderText = "Specified limits as per (IS 15477:2004)";

            }
            #endregion
            #region 19-37-383
            else if (ddl_ReportFor.SelectedValue == "19")
            {
                DataTable dt2 = new DataTable();
                chkBox1.Visible = true;
                chkBox2.Visible = true;
                chkBox3.Visible = true;

                grdDetail1.Visible = true;
                grdDetail2.Visible = true;
                grdDetail3.Visible = true;

                lblHeading1.Text = "A) Chemical Analysis";
                lblHeading2.Text = "B)Specified Limits For 080A42 (EN-8D)";
                lblHeading3.Text = "C)Specified Limits For 080A40 (EN-8)";

                lblHeading2.Visible = true;
                lblHeading3.Visible = true;
                lblHeading4.Visible = true;

                grdDetail1.Width = 5 * 120;
                grdDetail2.Width = 5 * 100;
                grdDetail3.Width = 5 * 100;

                for (int i = 3; i <= 9; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }

                for (int i = 3; i <= 9; i++)
                {
                    grdDetail2.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 8; i++)
                {
                    grdDetail3.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "Dia Of Coupler(mm)";
                grdDetail1.Columns[4].HeaderText = "Carbon (%)";
                grdDetail1.Columns[5].HeaderText = "Manganese (%)";
                grdDetail1.Columns[6].HeaderText = "Sulphur (%)";
                grdDetail1.Columns[7].HeaderText = "Phosphorus (%)";
                grdDetail1.Columns[8].HeaderText = "Silicon (%)";
                grdDetail1.Columns[9].HeaderText = "Compliance";

                grdDetail2.Columns[2].HeaderText = "";
                grdDetail2.Columns[3].HeaderText = "";
                grdDetail2.Columns[4].HeaderText = "Carbon (%)";
                grdDetail2.Columns[5].HeaderText = "Manganese (%)";
                grdDetail2.Columns[6].HeaderText = "Sulphur (%)";
                grdDetail2.Columns[7].HeaderText = "Phosphorus (%)";
                grdDetail2.Columns[8].HeaderText = "Silicon (%)";
                grdDetail2.Columns[9].HeaderText = "Compliance";


                grdDetail3.Columns[2].HeaderText = "";
                grdDetail3.Columns[3].HeaderText = "";
                grdDetail3.Columns[4].HeaderText = "Carbon (%)";
                grdDetail3.Columns[5].HeaderText = "Manganese (%)";
                grdDetail3.Columns[6].HeaderText = "Sulphur (%)";
                grdDetail3.Columns[7].HeaderText = "Phosphorus (%)";
                grdDetail3.Columns[8].HeaderText = "Silicon (%)";
                //grdDetail3.Columns[9].HeaderText = "Compliance";

            }
            #endregion
            #region 20-18-126
            else if (ddl_ReportFor.SelectedValue == "20")
            {
                chkBox1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 4 * 200;
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Description";
                grdDetail1.Columns[4].HeaderText = "Unit";
                grdDetail1.Columns[5].HeaderText = "Test Result";
                grdDetail1.Columns[6].HeaderText = "Observation";
            }


            #endregion
            #region 21-16-124
            else if (ddl_ReportFor.SelectedValue == "21")
            {
                chkBox1.Checked = true;
                chkBox1.Visible = true;
                chkBox8.Checked = true;
                chkBox8.Visible = true;

                lblHeading1.Text = "A) Half Cell Potentiometer readings are generally interpreted as follows:";
                lblHeading8.Text = "B) Half cell readings obtained at site ";

                lblHeading1.Visible = true;
                lblHeading8.Visible = true;

                grdDetail1.Visible = true;
                grdDetail8.Visible = true;

                grdDetail1.Width = 3 * 200;
                grdDetail8.Width = 6 * 100;

                for (int i = 3; i <= 4; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                for (int i = 4; i <= 8; i++)
                {
                    grdDetail8.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "Half Cell potential Readings ( mv )";
                grdDetail1.Columns[4].HeaderText = "Conclusion";

                grdDetail8.Columns[4].HeaderText = "Description / ID Mark";
                grdDetail8.Columns[5].HeaderText = "Average Half cell Potentiometer readings (mv)";
                grdDetail8.Columns[6].HeaderText = "Temp. at the time of testing (°C)";
                grdDetail8.Columns[7].HeaderText = "Temp. Coefficient (mv)";
                grdDetail8.Columns[8].HeaderText = "Corrected average Potentiometer Reading (mv)";
            }
            #endregion
            #region 22-18-126
            else if (ddl_ReportFor.SelectedValue == "22")
            {
                chkBox8.Visible = true;
                grdDetail8.Visible = true;
                lblHeading8.Text = "Carbonation readings obtained at site:";
                lblHeading8.Visible = true;

                grdDetail8.Width = 4 * 230;
                for (int i = 4; i <= 6; i++)
                {
                    grdDetail8.Columns[i].Visible = true;
                }
                grdDetail8.Columns[4].HeaderText = "Description / ID Mark";
                grdDetail8.Columns[5].HeaderText = "Depth Of  Carbonation (mm)";
                grdDetail8.Columns[6].HeaderText = "Concrete Cover (mm)";

            }


            #endregion
            #region 23-18-126
            else if (ddl_ReportFor.SelectedValue == "23")
            {
                chkBox1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 6 * 150;
                for (int i = 3; i <= 7; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Nominal Dia (mm)";
                grdDetail1.Columns[4].HeaderText = "ID Mark";
                grdDetail1.Columns[5].HeaderText = "C/s Area (mm²)";
                grdDetail1.Columns[6].HeaderText = "Elongation (%)";
                grdDetail1.Columns[7].HeaderText = "Breaking  Load (N)";

            }
            #endregion
            #region 24-18-126
            else if (ddl_ReportFor.SelectedValue == "24")
            {
                chkBox1.Visible = true;

                lblHeading1.Text = "A) Chemical Analysis:-";
                lblHeading1.Visible = true;

                grdDetail1.Visible = true;
                grdDetail1.Width = 6 * 150;
                for (int i = 3; i <= 7; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Test Parameters";
                grdDetail1.Columns[4].HeaderText = "Unit";
                grdDetail1.Columns[5].HeaderText = "Results";
                //grdDetail1.Columns[6].HeaderText = "Requirement As per IS 12089:1987";
                grdDetail1.Columns[6].HeaderText = "Requirement As per IS 16714-2018";
                grdDetail1.Columns[7].HeaderText = "Compliance";

            }
            #endregion
            #region 25-16-124
            else if (ddl_ReportFor.SelectedValue == "25")
            {
                chkBox1.Visible = true;
                chkBox2.Visible = true;

                lblHeading1.Text = "A) Chemical Analysis";
                lblHeading2.Text = "B) Specifications:- IS 3812 (Part-1) : 2003 'Specification For Fuel Ash Sample'";
                lblHeading1.Visible = true;
                lblHeading2.Visible = true;
                grdDetail1.Visible = true;
                grdDetail2.Visible = true;

                grdDetail1.Width = 3 * 250;
                grdDetail2.Width = 6 * 150;

                for (int i = 3; i <= 5; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 6; i++)
                {
                    grdDetail2.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "Test";
                grdDetail1.Columns[4].HeaderText = "Unit";
                grdDetail1.Columns[5].HeaderText = "Test Results";

                grdDetail2.Columns[3].HeaderText = "Test";
                grdDetail2.Columns[4].HeaderText = "Unit";
                grdDetail2.Columns[5].HeaderText = "Siliceous Pulverized Fuel Ash";
                grdDetail2.Columns[6].HeaderText = "Calcareous Pulverized Fuel Ash";

            }
            #endregion
            #region 26-18-126
            else if (ddl_ReportFor.SelectedValue == "26")
            {
                chkBox1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 9 * 100;
                for (int i = 3; i <= 10; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "ID Mark";
                grdDetail1.Columns[4].HeaderText = "Age(Days)";
                grdDetail1.Columns[5].HeaderText = "Span Length(mm)";
                grdDetail1.Columns[6].HeaderText = "Width(mm)";
                grdDetail1.Columns[7].HeaderText = "Thickness(mm)";
                grdDetail1.Columns[8].HeaderText = "Load(KN)";
                grdDetail1.Columns[9].HeaderText = "Flexural Strength(N/mm2)";
                grdDetail1.Columns[10].HeaderText = " Avg. Flexural Strength(N/mm2)";

            }
            #endregion
            #region 27-18-126
            else if (ddl_ReportFor.SelectedValue == "27")
            {
                chkBox1.Visible = true;

                // lblHeading1.Text = "Sieve Analysis";
                //lblHeading1.Visible = true;

                grdDetail1.Visible = true;
                grdDetail1.Width = 8 * 100;
                for (int i = 3; i <= 9; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Sample ID";
                grdDetail1.Columns[4].HeaderText = "Length (mm)";
                grdDetail1.Columns[5].HeaderText = "Breadth (mm)";
                grdDetail1.Columns[6].HeaderText = "Height (mm)";
                grdDetail1.Columns[7].HeaderText = "Depth of Penetration (mm)";
                grdDetail1.Columns[8].HeaderText = "Avg Depth of Penetration(mm)";
                grdDetail1.Columns[9].HeaderText = "At Pressure for 72 ± 2 Hrs(Kg/cm2)";


            }
            #endregion
            #region 28-18-126
            else if (ddl_ReportFor.SelectedValue == "28")
            {
                chkBox1.Visible = true;

                // lblHeading1.Text = "Sieve Analysis";
                //lblHeading1.Visible = true;

                grdDetail1.Visible = true;
                grdDetail1.Width = 4 * 200;
                for (int i = 3; i <= 5; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Test Parameter";
                grdDetail1.Columns[4].HeaderText = "Result (%)";
                grdDetail1.Columns[5].HeaderText = "Specified Limits as per IS 15388-2003";



            }
            #endregion
            #region 29-18-126
            else if (ddl_ReportFor.SelectedValue == "29")
            {
                chkBox1.Visible = true;

                lblHeading1.Text = "Sieve Analysis of Readymix Plaster :";
                lblHeading1.Visible = true;
                grdDetail1.Visible = true;

                grdDetail1.Width = 6 * 120;
                for (int i = 3; i <= 7; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Sieve Sizes";
                grdDetail1.Columns[4].HeaderText = "Wt. Retained (g)";
                grdDetail1.Columns[5].HeaderText = "Wt. Retain (%)";
                grdDetail1.Columns[6].HeaderText = "Cumu. Wt. Retain (%)";
                grdDetail1.Columns[7].HeaderText = "Cumu. Passing (%)";

            }
            #endregion
            #region 30-18-126
            else if (ddl_ReportFor.SelectedValue == "30")
            {
                chkBox1.Visible = true;

                //lblHeading1.Text = "Sieve Analysis";
                //lblHeading1.Visible = true;

                grdDetail1.Visible = true;
                grdDetail1.Width = 8 * 80;
                for (int i = 3; i <= 9; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "ID Mark";
                grdDetail1.Columns[4].HeaderText = "Length";
                grdDetail1.Columns[5].HeaderText = "Breadth";
                grdDetail1.Columns[6].HeaderText = "Height";
                grdDetail1.Columns[7].HeaderText = "Weight(kg)";
                grdDetail1.Columns[8].HeaderText = "Density (kg/m3)";
                grdDetail1.Columns[9].HeaderText = "Avg. Density (kg/m3)";

            }
            #endregion
            #region 31-18-126
            else if (ddl_ReportFor.SelectedValue == "31")
            {
                chkBox1.Visible = true;

                //lblHeading1.Text = "Sieve Analysis";
                //lblHeading1.Visible = true;

                grdDetail1.Visible = true;
                grdDetail1.Width = 6 * 100;
                for (int i = 3; i <= 7; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "ID Mark";
                grdDetail1.Columns[4].HeaderText = "Wet Weight (Kg)";
                grdDetail1.Columns[5].HeaderText = "Dry Weight (Kg)";
                grdDetail1.Columns[6].HeaderText = "Water Absorption (%)";
                grdDetail1.Columns[7].HeaderText = "Avg.Water Absorption (%)";

            }
            #endregion
            #region 32-18-126
            else if (ddl_ReportFor.SelectedValue == "32")
            {
                chkBox1.Visible = true;

                // lblHeading1.Text = "Sieve Analysis";
                //lblHeading1.Visible = true;

                grdDetail1.Visible = true;
                grdDetail1.Width = 9 * 70;
                for (int i = 3; i <= 10; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "ID Mark";
                grdDetail1.Columns[4].HeaderText = "Length";
                grdDetail1.Columns[5].HeaderText = "Breadth";
                grdDetail1.Columns[6].HeaderText = "Height";
                grdDetail1.Columns[7].HeaderText = "Cross sectional  Area(mm2)";
                grdDetail1.Columns[8].HeaderText = "Load(KN)";
                grdDetail1.Columns[9].HeaderText = "Compressive Strength (N/mm2)";
                grdDetail1.Columns[10].HeaderText = "Avg. Compressive Strength(N/mm2)";

            }
            #endregion
            #region 33-18-126
            else if (ddl_ReportFor.SelectedValue == "33")
            {
                chkBox1.Visible = true;

                // lblHeading1.Text = "Sieve Analysis";
                //lblHeading1.Visible = true;

                grdDetail1.Visible = true;
                grdDetail1.Width = 9 * 50;
                for (int i = 3; i <= 10; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "Dia(mm)";
                grdDetail1.Columns[4].HeaderText = "ID Mark";
                grdDetail1.Columns[5].HeaderText = "Weight( gm )";
                grdDetail1.Columns[6].HeaderText = "Length( mm )";
                grdDetail1.Columns[7].HeaderText = "Weight( Kg )/meter";
                grdDetail1.Columns[8].HeaderText = "Cross sectional  Area(mm2)";
                grdDetail1.Columns[9].HeaderText = "Full Gauge Length( mm )";
                grdDetail1.Columns[10].HeaderText = "Half Gauge Length( mm )";

            }
            #endregion
            #region 34
            if (ddl_ReportFor.SelectedValue == "34")
            {
                chkBox1.Visible = true;
                chkBox2.Visible = true;
                //chkBox3.Visible = true;
                //chkBox4.Visible = true;

                lblHeading1.Text = "Observation and readings";
                lblHeading2.Text = "Cylinder I";
                lblHeading3.Text = "Cylinder II";
                lblHeading4.Text = "Cylinder III";

                lblHeading1.Visible = true;
                lblHeading2.Visible = true;
                //lblHeading3.Visible = true;
                //lblHeading4.Visible = true;

                grdDetail1.Visible = true;
                grdDetail2.Visible = true;
                //grdDetail3.Visible = true;
                //grdDetail4.Visible = true;

                grdDetail1.Width = 7 * 100;
                grdDetail2.Width = 9 * 100;
                grdDetail3.Width = 9 * 100;
                grdDetail4.Width = 9 * 100;
                
                grdDetail1.Columns[2].Visible = false;
                grdDetail2.Columns[2].Visible = false;
                grdDetail3.Columns[2].Visible = false;
                grdDetail4.Columns[2].Visible = false;

                for (int i = 3; i <= 9; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 11; i++)
                {
                    grdDetail2.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 11; i++)
                {
                    grdDetail3.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 11; i++)
                {
                    grdDetail4.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "";
                grdDetail1.Columns[4].HeaderText = "";
                grdDetail1.Columns[5].HeaderText = "";
                grdDetail1.Columns[6].HeaderText = "";
                grdDetail1.Columns[7].HeaderText = "";
                grdDetail1.Columns[8].HeaderText = "";
                grdDetail1.Columns[9].HeaderText = "";

                grdDetail2.Columns[3].HeaderText = "Load Applied (KN)";
                grdDetail2.Columns[4].HeaderText = "Area in mm²";
                grdDetail2.Columns[5].HeaderText = "Stress (N/mm²)";
                grdDetail2.Columns[6].HeaderText = "Loading Cycle 1 - Dial Read";
                grdDetail2.Columns[7].HeaderText = "Loading Cycle 1 - strain";
                grdDetail2.Columns[8].HeaderText = "Loading Cycle 2 - Dial Read";
                grdDetail2.Columns[9].HeaderText = "Loading Cycle 2 - strain";
                grdDetail2.Columns[10].HeaderText = "% Diffrence";
                grdDetail2.Columns[11].HeaderText = "Average MOE (Gpa)";

                grdDetail3.Columns[3].HeaderText = "Load Applied (KN)";
                grdDetail3.Columns[4].HeaderText = "Area in mm²";
                grdDetail3.Columns[5].HeaderText = "Stress (N/mm²)";
                grdDetail3.Columns[6].HeaderText = "Loading Cycle 1-Dial Read";
                grdDetail3.Columns[7].HeaderText = "Loading Cycle 1-strain";
                grdDetail3.Columns[8].HeaderText = "Loading Cycle 2-Dial Read";
                grdDetail3.Columns[9].HeaderText = "Loading Cycle 2-strain";
                grdDetail3.Columns[10].HeaderText = "% Diffrence";
                grdDetail3.Columns[11].HeaderText = "Average MOE (Gpa)";

                grdDetail4.Columns[3].HeaderText = "Load Applied (KN)";
                grdDetail4.Columns[4].HeaderText = "Area in mm²";
                grdDetail4.Columns[5].HeaderText = "Stress (N/mm²)";
                grdDetail4.Columns[6].HeaderText = "Loading Cycle 1-Dial Read";
                grdDetail4.Columns[7].HeaderText = "Loading Cycle 1-strain";
                grdDetail4.Columns[8].HeaderText = "Loading Cycle 2-Dial Read";
                grdDetail4.Columns[9].HeaderText = "Loading Cycle 2-strain";
                grdDetail4.Columns[10].HeaderText = "% Diffrence";
                grdDetail4.Columns[11].HeaderText = "Average MOE (Gpa)";
            }
            #endregion
            #region 35
            if (ddl_ReportFor.SelectedValue == "35")
            {
                chkBox1.Visible = true;
                lblHeading1.Text = "Results and Observations";
                lblHeading1.Visible = true;
                grdDetail1.Visible = true;
                grdDetail1.Width = 3 * 200;
                for (int i = 3; i <= 7; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                grdDetail1.Columns[3].HeaderText = "Sample ID & Dimension";
                grdDetail1.Columns[4].HeaderText = "Age of Specimen (Days) ";
                grdDetail1.Columns[5].HeaderText = "Maximum Water Penetration Depth (mm)";
                grdDetail1.Columns[6].HeaderText = "Average";
                grdDetail1.Columns[7].HeaderText = "Compliance";
            }
            #endregion
            #region 36
            else if (ddl_ReportFor.SelectedValue == "36")
            {
                lblDiameterOfSample.Visible = true;
                txtDiameterOfSample.Visible = true;
                lblTemperatureDuringTest.Visible = true;
                txtTemperatureDuringTest.Visible = true;
                
                chkBox1.Visible = true;
                chkBox2.Visible = true;
                lblHeading2.Text = "Requirements - Chloride Ion Penetrability based on charge passed as per ASTM - C 1202-19";
                lblHeading2.Visible = true;
                grdDetail1.Visible = true;
                grdDetail2.Visible = true;
                grdDetail1.Width = 6 * 150;
                grdDetail2.Width = 3 * 150;

                for (int i = 3; i <= 7; i++)
                {
                    grdDetail1.Columns[i].Visible = true;
                }
                for (int i = 3; i <= 4; i++)
                {
                    grdDetail2.Columns[i].Visible = true;
                }

                grdDetail1.Columns[3].HeaderText = "ID";
                grdDetail1.Columns[4].HeaderText = "Charge Passed in (Coulombs)";
                grdDetail1.Columns[5].HeaderText = "Corrected Charge Passed in (Coulombs)";
                grdDetail1.Columns[6].HeaderText = "Average Charge Passed in (Coulombs)";
                grdDetail1.Columns[7].HeaderText = "Chloride Ion Penetrability";

                grdDetail2.Columns[3].HeaderText = "Charge Passed in (Coulombs)";
                grdDetail2.Columns[4].HeaderText = "Chloride Ion Penetration";
            }
            #endregion
            int cnt = 0;
            if (grdDetail1.Visible == false)
                cnt++;
            if (grdDetail2.Visible == true)
                cnt++;
            if (grdDetail3.Visible == true)
                cnt++;
            if (grdDetail4.Visible == true)
                cnt++;
            if (grdDetail5.Visible == true)
                cnt++;
            if (grdDetail6.Visible == true)
                cnt++;
            if (grdDetail7.Visible == true)
                cnt++;
            if (grdDetail8.Visible == true)
                cnt++;
            if (cnt == 1)
                chkBox1.Enabled = false;
            else
                chkBox1.Enabled = true;

            DisplayDefaultRptValues();
        }
        protected void DisplayDefaultRptValues()
        {

            chkBox1.Checked = true;
            chkBox2.Checked = true;
            chkBox3.Checked = true;
            chkBox4.Checked = true;
            chkBox5.Checked = true;
            chkBox6.Checked = true;
            chkBox7.Checked = true;
            chkBox8.Checked = true;

            #region 1-15-123
            if (ddl_ReportFor.SelectedValue == "1")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b1", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b2", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b3", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b4", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b5", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b6", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b7", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b8", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b9", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b10", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b11", typeof(string)));

                DataTable dt3 = new DataTable();
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c1", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c2", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c3", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c4", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c5", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c6", typeof(string)));

                dt3.Columns.Add(new DataColumn("txt_c7", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c8", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c9", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Normal Consistency";
                dr["txt_a2"] = "%";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Setting Time";
                dr["txt_a2"] = "Minutes";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Flexural Strength (1 Day)";
                dr["txt_a2"] = "Kg/cm²";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Compressive Strength (1 Day)";
                dr["txt_a2"] = "N/mm²";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Compressive Strength (7 Day)";
                dr["txt_a2"] = "N/mm²";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Loose Bulk Density";
                dr["txt_a2"] = "Kg/m³";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Residue on 150 um sieve";
                dr["txt_a2"] = "%";
                dt1.Rows.Add(dr);
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Normal Consistency";
                dr["txt_b2"] = "%";
                dr["txt_b3"] = "-";
                dr["txt_b4"] = "-";
                dr["txt_b5"] = "-";
                dr["txt_b6"] = "-";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Setting Time";
                dr["txt_b2"] = "Minutes";
                dr["txt_b3"] = "-";
                dr["txt_b4"] = "-";
                dr["txt_b5"] = "-";
                dr["txt_b6"] = "-";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "a) Plaster sand mixture";
                dr["txt_b2"] = "Minutes";
                dr["txt_b3"] = "45-120";
                dr["txt_b4"] = "120-900";
                dr["txt_b5"] = "-";
                dr["txt_b6"] = "-";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "b) Neat Plaster";
                dr["txt_b2"] = "Minutes";
                dr["txt_b3"] = "10-30";
                dr["txt_b4"] = "60-180";
                dr["txt_b5"] = "20-360";
                dr["txt_b6"] = "20-360";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Flexural Strength (Min)";
                dr["txt_b2"] = "kg/cm²";
                dr["txt_b3"] = "5";
                dr["txt_b4"] = "4*";
                dr["txt_b5"] = "-";
                dr["txt_b6"] = "-";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Residue on 150um sieve (Max)";
                dr["txt_b2"] = "%";
                dr["txt_b3"] = "5.0";
                dr["txt_b4"] = "5.0* (1.0)+";
                dr["txt_b5"] = "2.0";
                dr["txt_b6"] = "2.0";
                dt2.Rows.Add(dr);
                ViewState["OtherTable2"] = dt2;
                grdDetail2.DataSource = dt2;
                grdDetail2.DataBind();

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_c1"] = "Compressive Strength(Min.)";
                dr["txt_c2"] = "N/mm²";
                dr["txt_c3"] = "0.93";
                dr["txt_c4"] = "1.0";
                dr["txt_c5"] = "1.0";
                dr["txt_c6"] = "-";
                dt3.Rows.Add(dr);

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_c1"] = "Loose Bulk Density(Max.)";
                dr["txt_c2"] = "Kg/m³";
                dr["txt_c3"] = "640";
                dr["txt_c4"] = "770";
                dr["txt_c5"] = "770";
                dr["txt_c6"] = "-";
                dt3.Rows.Add(dr);
                ViewState["OtherTable3"] = dt3;
                grdDetail3.DataSource = dt3;
                grdDetail3.DataBind();
            }
            #endregion
            #region 2-16-124
            else if (ddl_ReportFor.SelectedValue == "2")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b1", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b2", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b3", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b4", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b5", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b6", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b7", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b8", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b9", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b10", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b11", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Sulphate (SO₃)";
                dr["txt_a2"] = "%";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Total Calcium (CaO)";
                dr["txt_a2"] = "%";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Loss on Ignition";
                dr["txt_a2"] = "%";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Free Lime";
                dr["txt_a2"] = "%";
                dt1.Rows.Add(dr);
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Sulphate (SO₃)";
                dr["txt_b2"] = "%";
                dr["txt_b3"] = "Min 35";
                dr["txt_b4"] = "Min 35";
                dr["txt_b5"] = "Min 40";
                dr["txt_b6"] = "Min 47";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Total Calcium (CaO)";
                dr["txt_b2"] = "%";
                dr["txt_b3"] = "Min 2/3 of SO3";
                dr["txt_b4"] = "Min 2/3 of SO3";
                dr["txt_b5"] = "Min 2/3 of SO3";
                dr["txt_b6"] = "Min 2/3 of SO3";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Loss on Ignition";
                dr["txt_b2"] = "%";
                dr["txt_b3"] = "4 - 9";
                dr["txt_b4"] = "4 - 9";
                dr["txt_b5"] = "Max 3.0";
                dr["txt_b6"] = "Max 2.0";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Free Lime";
                dr["txt_b2"] = "%";
                dr["txt_b3"] = "-";
                dr["txt_b4"] = "3+";
                dr["txt_b5"] = "-";
                dr["txt_b6"] = "-";
                dt2.Rows.Add(dr);
                ViewState["OtherTable2"] = dt2;
                grdDetail2.DataSource = dt2;
                grdDetail2.DataBind();
            }
            #endregion
            #region 5-29-683
            else if (ddl_ReportFor.SelectedValue == "5")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b1", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b2", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b3", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b4", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b5", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b6", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b7", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b8", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b9", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b10", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b11", typeof(string)));

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Up to 1.25";
                dr["txt_b2"] = "Very Weak";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "1.25 to 5.0";
                dr["txt_b2"] = "Weak";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "5 to 12.5";
                dr["txt_b2"] = "Moderately Weak";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "12.5 to 50";
                dr["txt_b2"] = "Moderately Strong";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "50 to 100";
                dr["txt_b2"] = "Strong";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "100 to 200";
                dr["txt_b2"] = "Very Strong";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "200";
                dr["txt_b2"] = "Extremely Strong";
                dt2.Rows.Add(dr);
                ViewState["OtherTable2"] = dt2;
                grdDetail2.DataSource = dt2;
                grdDetail2.DataBind();

                if (grdDetail1.Rows.Count == 0)
                    AddRowDetail1();
            }
            #endregion
            #region 6-30-684
            else if (ddl_ReportFor.SelectedValue == "6")
            {
                if (grdDetail1.Rows.Count == 0)
                    AddRowDetail1();
                if (grdDetail2.Rows.Count == 0)
                    AddRowDetail2();
                if (grdDetail3.Rows.Count == 0)
                    AddRowDetail3();
            }
            #endregion
            #region 7-31-685
            else if (ddl_ReportFor.SelectedValue == "7")
            {
                if (grdDetail1.Rows.Count == 0)
                    AddRowDetail1();
            }
            #endregion
            #region 8-32-689
            else if (ddl_ReportFor.SelectedValue == "8")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Color";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "Unit";
                dr["txt_a4"] = "5";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Odour";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "---";
                dr["txt_a4"] = "Agreeable";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Turbidity";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "NTU";
                dr["txt_a4"] = "5";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "pH";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "---";
                dr["txt_a4"] = "6.5-8.5";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Total Hardness (as CaCO₃)";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "mg/lit";
                dr["txt_a4"] = "200";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Calcium Hardness (as Ca)";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "mg/lit";
                dr["txt_a4"] = "75";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Magnesium Hardness (as Mg)";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "mg/lit";
                dr["txt_a4"] = "30";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Total Dissolved Solids";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "mg/lit";
                dr["txt_a4"] = "500";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Chlorides (as Cl)";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "mg/lit";
                dr["txt_a4"] = "250";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Sulphates (SO₄)";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "mg/lit";
                dr["txt_a4"] = "200";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "M-Alkalinity (as CaCO₃)";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "mg/lit";
                dr["txt_a4"] = "200";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Total Coliforms (M.P.N.)";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "---";
                dr["txt_a4"] = "Absent/100 ml";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "E. Coli";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "----";
                dr["txt_a4"] = "Absent/100 ml";
                dt1.Rows.Add(dr);
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

                if (grdDetail2.Rows.Count == 0)
                    AddRowDetail2();
                if (grdDetail3.Rows.Count == 0)
                    AddRowDetail3();
            }
            #endregion
            #region 3-18-126
            else if (ddl_ReportFor.SelectedValue == "3")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Standard Consistency";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "%";
                dr["txt_a4"] = "IS:4031 (Pt-4) - 1988-RA(2014)";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Initial Setting Time";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "Minutes";
                dr["txt_a4"] = "IS:4031 (Pt-5) - 1988-RA(2014)";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Final Setting Time";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "Minutes";
                dr["txt_a4"] = "IS:4031 (Pt-5) - 1988-RA(2014)";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Density";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "g/cm³";
                dr["txt_a4"] = "IS:4031 (Pt-11) - 1988-RA(2014)";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Fineness By Blain's Air Permeability Method";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "m²/kg";
                dr["txt_a4"] = "IS:4031 (Pt-2) - 1999-RA(2013)";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "72 + 1hr (3 Days) Compressive Strength";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "N/mm²";
                dr["txt_a4"] = "IS:4031 (Pt-3) - 1988-RA(2014)";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "168 + 2hr (7 Days) Compressive Strength";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "N/mm²";
                dr["txt_a4"] = "IS:4031 (Pt-6) - 1988-RA(2014)";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "672 + 4hr (28 Days) Compressive Strength";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "N/mm²";
                dr["txt_a4"] = "IS:4031 (Pt-6) - 1988-RA(2014)";
                dt1.Rows.Add(dr);
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();
                grdDetail1.Columns[3].ItemStyle.Width = 300;
                grdDetail1.Columns[4].ItemStyle.Width = 100;
                grdDetail1.Columns[5].ItemStyle.Width = 100;
                grdDetail1.Columns[6].ItemStyle.Width = 200;

                if (grdDetail2.Rows.Count == 0)
                    AddRowDetail2();
                if (grdDetail3.Rows.Count == 0)
                    AddRowDetail3();
            }
            #endregion
            #region 9-33-690
            else if (ddl_ReportFor.SelectedValue == "9")
            {
                if (grdDetail1.Rows.Count == 0)
                    AddRowDetail1();
            }
            #endregion
            #region 4-21-129
            if (ddl_ReportFor.SelectedValue == "4")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b1", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b2", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b3", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b4", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b5", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b6", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b7", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b8", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b9", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b10", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b11", typeof(string)));

                DataTable dt3 = new DataTable();
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c1", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c2", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c3", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c4", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c5", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c6", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c7", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c8", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c9", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "";
                dr["txt_a2"] = "Density";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "g/cm³";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "";
                dr["txt_a2"] = "Moisture Content";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "%";
                dt1.Rows.Add(dr);
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "";
                dr["txt_b2"] = "Water Resistance";
                dr["txt_b3"] = "";
                dr["txt_b4"] = "---";
                dt2.Rows.Add(dr);

                grdDetail2.DataSource = dt2;
                grdDetail2.DataBind();

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_c1"] = "";
                dr["txt_c2"] = "Nails Holiding Power";
                dr["txt_c3"] = "";
                dr["txt_c4"] = "KN";
                dt3.Rows.Add(dr);

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_c1"] = "";
                dr["txt_c2"] = "Screw Holding Power";
                dr["txt_c3"] = "";
                dr["txt_c4"] = "KN";
                dt3.Rows.Add(dr);
                ViewState["OtherTable3"] = dt3;
                grdDetail3.DataSource = dt3;
                grdDetail3.DataBind();
            }

            #endregion
            #region 10-34-501
            if (ddl_ReportFor.SelectedValue == "10")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Water Demand";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "%";
                dr["txt_a4"] = "---";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Open Time";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "minutes";
                dr["txt_a4"] = "IS 15477 : 2004, Min. 15minutes";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Final Setting time";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "minutes";
                dr["txt_a4"] = "---";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Adjustability";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "minutes";
                dr["txt_a4"] = "IS 15477 : 2004, Min. 15minutes";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Tensile Adhesion Dry";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "N";
                dr["txt_a4"] = "IS 15477 : 2004, 750 N";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Tensile Adhesion Wet";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "N";
                dr["txt_a4"] = "IS 15477 : 2004, 450 N";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Shear strength 24 hrs";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "kN";
                dr["txt_a4"] = "IS 15477 : 2004, 2.50 kN";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Shear strength 14 days Dry";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "kN";
                dr["txt_a4"] = "IS 15477 : 2004, 8.00 kN";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Shear strength 7 + 7 days Wet";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "kN";
                dr["txt_a4"] = "IS 15477 : 2004, 4.00 kN";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Shear strength 7 + 7 days Heat aging";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "kN";
                dr["txt_a4"] = "IS 15477 : 2004, 4.00 kN";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Pull Off Test 14 days Dry";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "N/mm²";
                dr["txt_a4"] = "BS - 5980 min. 0.168 N/mm²";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Pull Off Test 28 days Dry";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "N/mm²";
                dr["txt_a4"] = "BS - 5980 min. 0.168 N/mm²";
                dt1.Rows.Add(dr);
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();
            }

            #endregion
            #region 11-35-691
            if (ddl_ReportFor.SelectedValue == "11")
            {
                if (grdDetail1.Rows.Count == 0)
                    AddRowDetail1();
            }

            #endregion
            #region 12-36-692
            if (ddl_ReportFor.SelectedValue == "12")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b1", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b2", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b3", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b4", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b5", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b6", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b7", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b8", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b9", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b10", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b11", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Less than -500";
                dr["txt_a2"] = "Visible evidence of corrosion.";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Between -350 to -500";
                dr["txt_a2"] = "95% chances of starting of corrosion.";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Above -350 to -200";
                dr["txt_a2"] = "50% chances of starting of corrosion.";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Greater than -200";
                dr["txt_a2"] = "5% chance of start of corrosion activity.";
                dt1.Rows.Add(dr);
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

                if (grdDetail2.Rows.Count == 0)
                    AddRowDetail2();
            }

            #endregion
            #region 13-23-131
            else if (ddl_ReportFor.SelectedValue == "13")
            {
                if (grdDetail1.Rows.Count == 0)
                    AddRowDetail1();
            }
            #endregion
            #region 14-3-111
            else if (ddl_ReportFor.SelectedValue == "14")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));


                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "pH Value";
                dr["txt_a2"] = "----";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Relative Density";
                dr["txt_a2"] = "----";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Dry Material Content";// "Solid Content";
                dr["txt_a2"] = "%";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Ash Content";
                dr["txt_a2"] = "%";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Chloride Content";
                dr["txt_a2"] = "%";
                dt1.Rows.Add(dr);
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

            }
            #endregion
            #region  15-37-383
            else if (ddl_ReportFor.SelectedValue == "15")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));


                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Coal & Lignite";
                dr["txt_a2"] = "%";
                dr["txt_a4"] = "1";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Clay Lumps";
                dr["txt_a2"] = "%";
                dr["txt_a4"] = "1";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Clay, Fine Silt and  Dust (Sedimentation Method)";
                dr["txt_a2"] = "%";
                dr["txt_a4"] = "----";
                dt1.Rows.Add(dr);

                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

            }
            #endregion
            #region 16-10-118
            else if (ddl_ReportFor.SelectedValue == "16")
            {
                if (grdDetail1.Rows.Count == 0)
                    AddRowDetail1();

                if (grdDetail2.Rows.Count == 0)
                    AddRowDetail2();

                if (grdDetail3.Rows.Count == 0)
                    AddRowDetail3();

                if (grdDetail4.Rows.Count == 0)
                    AddRowDetail4();

                if (grdDetail5.Rows.Count == 0)
                    AddRowDetail5();

                if (grdDetail6.Rows.Count == 0)
                    AddRowDetail6();

                //   if (grdDetail7.Rows.Count == 0)
                //      AddRowDetail7();
            }
            #endregion
            #region 17-25-133
            else if (ddl_ReportFor.SelectedValue == "17")
            {
                if (grdDetail1.Rows.Count == 0)
                    AddRowDetail1();
                if (grdDetail2.Rows.Count == 0)
                    AddRowDetail2();
            }
            #endregion
            #region 18-34-501
            else if (ddl_ReportFor.SelectedValue == "18")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));


                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Water Demand";
                dr["txt_a3"] = "%";
                dr["txt_a4"] = "----";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Open Time";
                dr["txt_a3"] = "minutes";
                dr["txt_a4"] = "Min. 15 minutes";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Setting time";
                dr["txt_a3"] = "minutes";
                dr["txt_a4"] = "----";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Adjustability";
                dr["txt_a3"] = "minutes";
                dr["txt_a4"] = "Min. 15 minutes";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Dry Tensile Adhesion";
                dr["txt_a3"] = "N";
                dr["txt_a4"] = "Type I -750 N";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Wet Tensile Adhesion";
                dr["txt_a3"] = "N";
                dr["txt_a4"] = "Type I -450 N";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Shear strength 24 hrs";
                dr["txt_a3"] = "kN";
                dr["txt_a4"] = "Type-I- 2.50 kN, Type-II-4.00 kN";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Shear strength 14 Days Dry";
                dr["txt_a3"] = "kN";
                dr["txt_a4"] = "Type-I- 8 .0 kN,Type-II-10.0  kN";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Shear strength 7 + 7 days Wet";
                dr["txt_a3"] = "kN";
                dr["txt_a4"] = "Type-I- 4.0 kN,Type-II-5.0 kN";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Shear strength 7 + 7 days Heat aging";
                dr["txt_a3"] = "kN";
                dr["txt_a4"] = " Type-I-4.0 kN, Type-II-5.0 kN";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Pull Out Test 7/14/28 Days";
                dr["txt_a3"] = "N/mm2";
                dr["txt_a4"] = "----";
                dt1.Rows.Add(dr);
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

            }
            #endregion
            #region  19-37-383
            else if (ddl_ReportFor.SelectedValue == "19")
            {
                //DataTable dt1 = new DataTable();
                DataRow dr = null;
                //dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                //dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                //dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                //dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                //dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                //dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                //dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                //dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                //dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                //dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b1", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b2", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b3", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b4", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b5", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b6", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b7", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b8", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b9", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b10", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b11", typeof(string)));

                DataTable dt3 = new DataTable();
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c1", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c2", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c3", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c4", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c5", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c6", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c7", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c8", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c9", typeof(string)));

                //dr = dt1.NewRow();
                //dr["lblSrNo"] = dt1.Rows.Count + 1;
                //dr["txt_a1"] = "20";
                //dr["txt_a2"] = "0.42";
                //dr["txt_a3"] = "#0.7846";
                //dr["txt_a4"] = "0.017";
                //dr["txt_a5"] = "0.043";
                //dr["txt_a6"] = "0.25";
                //dr["txt_a7"] = "Pass";
                //dt1.Rows.Add(dr);
                //grdDetail1.DataSource = dt1;
                //grdDetail1.DataBind();
                if (grdDetail1.Rows.Count == 0)
                    AddRowDetail1();

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Permitted variations of product Analysis from specified range/";
                dr["txt_b2"] = "0.40-0.45";
                dr["txt_b3"] = "0.70-0.90";
                dr["txt_b4"] = "0.050 max";
                dr["txt_b5"] = "0.050max";
                dr["txt_b6"] = "0.10-0.40";
                dr["txt_b7"] = "-";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Over Max(%)";
                dr["txt_b2"] = "0.03";
                dr["txt_b3"] = "0.04";
                dr["txt_b4"] = "0.008";
                dr["txt_b5"] = "0.008";
                dr["txt_b6"] = "0.03";
                dr["txt_b7"] = "-";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Under min(%)";
                dr["txt_b2"] = "0.03";
                dr["txt_b3"] = "0.04";
                dr["txt_b4"] = "-";
                dr["txt_b5"] = "-";
                dr["txt_b6"] = "0.03";
                dr["txt_b7"] = "-";
                dt2.Rows.Add(dr);
                ViewState["OtherTable2"] = dt2;
                grdDetail2.DataSource = dt2;
                grdDetail2.DataBind();

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_c1"] = "Permitted variations of product Analysis from specified range /";
                dr["txt_c2"] = "0.36-0.44";
                dr["txt_c3"] = "0.60-1.00";
                dr["txt_c4"] = "0.050 max";
                dr["txt_c5"] = "0.050max";
                dr["txt_c6"] = "0.10-0.40";
                dr["txt_c7"] = "-";
                dt3.Rows.Add(dr);

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_c1"] = "Over Max(%)";
                dr["txt_c2"] = "0.03";
                dr["txt_c3"] = "0.04";
                dr["txt_c4"] = "0.008";
                dr["txt_c5"] = "0.008";
                dr["txt_c6"] = "0.03";
                dr["txt_c7"] = "-";
                dt3.Rows.Add(dr);

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_c1"] = "Under min(%)";
                dr["txt_c2"] = "0.03";
                dr["txt_c3"] = "0.04";
                dr["txt_c4"] = "-";
                dr["txt_c5"] = "-";
                dr["txt_c6"] = "0.03";
                dr["txt_c7"] = "-";
                dt3.Rows.Add(dr);
                ViewState["OtherTable3"] = dt3;
                grdDetail3.DataSource = dt3;
                grdDetail3.DataBind();

            }
            #endregion
            #region 20-18-126
            else if (ddl_ReportFor.SelectedValue == "20")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));


                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "";
                dr["txt_a2"] = "N/mm²";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dt1.Rows.Add(dr);

                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();
                grdDetail1.Columns[3].ItemStyle.Width = 300;
                grdDetail1.Columns[4].ItemStyle.Width = 100;
                grdDetail1.Columns[5].ItemStyle.Width = 100;
                grdDetail1.Columns[6].ItemStyle.Width = 200;

                if (grdDetail2.Rows.Count == 0)
                    AddRowDetail2();
                if (grdDetail3.Rows.Count == 0)
                    AddRowDetail3();
            }
            #endregion
            #region 21-16-124
            else if (ddl_ReportFor.SelectedValue == "21")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_a9", typeof(string)));
                dt2.Columns.Add(new DataColumn("lblMergFlag", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Less than -500";
                dr["txt_a2"] = "Visible evidence of corrosion";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Between -350 to -500";
                dr["txt_a2"] = "95 % chances of starting of corrosion";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Between -200 to -350";
                dr["txt_a2"] = "50% chances of starting of corrosion";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Greater than -200";
                dr["txt_a2"] = "5% chance of start of corrosion activity";
                dt1.Rows.Add(dr);
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_a1"] = "";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt2.Rows.Add(dr);

                ViewState["OtherTable2"] = dt2;
                grdDetail8.DataSource = dt2;
                grdDetail8.DataBind();
            }
            #endregion
            #region 22-16-124
            else if (ddl_ReportFor.SelectedValue == "22")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));
                dt1.Columns.Add(new DataColumn("lblMergFlag", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";

                dt1.Rows.Add(dr);
                ViewState["OtherTable1"] = dt1;
                grdDetail8.DataSource = dt1;
                grdDetail8.DataBind();

            }
            #endregion
            #region 23-18-126
            else if (ddl_ReportFor.SelectedValue == "23")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();
                grdDetail1.Columns[3].ItemStyle.Width = 300;
                grdDetail1.Columns[4].ItemStyle.Width = 100;
                grdDetail1.Columns[5].ItemStyle.Width = 100;
                grdDetail1.Columns[6].ItemStyle.Width = 200;

                if (grdDetail2.Rows.Count == 0)
                    AddRowDetail2();
                if (grdDetail3.Rows.Count == 0)
                    AddRowDetail3();
            }
            #endregion
            #region 24-18-126
            else if (ddl_ReportFor.SelectedValue == "24")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Calcium Oxide (CaO)";
                dr["txt_a2"] = "%";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "NA";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Silica (SiO2)";
                dr["txt_a2"] = "%";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "NA";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);


                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Ferric Oxide (Fe2O3)";
                dr["txt_a2"] = "%";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "NA";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);


                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Aluminium Oxide (Al2O3)";
                dr["txt_a2"] = "%";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "NA";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Magnesium Oxide (MgO)";
                dr["txt_a2"] = "%";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "Maximum 17";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Sulphuric Anhydride (SO3)";
                dr["txt_a2"] = "%";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "NA";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);



                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Loss On Ignition (LOI)";
                dr["txt_a2"] = "%";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "NA";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Insoluble residue(IR)";
                dr["txt_a2"] = "%";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "NA";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "CaO+MgO+ Al2O3/ SiO2";
                dr["txt_a2"] = "-";
                dr["txt_a3"] = "";
                dr["txt_a4"] = ">=1";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "CaO+MgO+ 1/3Al2O3 / SiO2 + 2/3Al2O3    ";
                dr["txt_a2"] = "-";
                dr["txt_a3"] = "";
                dr["txt_a4"] = ">=1";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();
                grdDetail1.Columns[3].ItemStyle.Width = 300;
                grdDetail1.Columns[4].ItemStyle.Width = 100;
                grdDetail1.Columns[5].ItemStyle.Width = 100;
                grdDetail1.Columns[6].ItemStyle.Width = 200;

                if (grdDetail2.Rows.Count == 0)
                    AddRowDetail2();
                if (grdDetail3.Rows.Count == 0)
                    AddRowDetail3();
            }
            #endregion
            #region 25-16-124
            else if (ddl_ReportFor.SelectedValue == "25")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b1", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b2", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b3", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b4", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b5", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b6", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b7", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b8", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b9", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b10", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b11", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "SiO2 +  Al2O3 + Fe2O3";
                dr["txt_a2"] = "%";
                dr["txt_a3"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Silicon Dioxide (SiO2)";
                dr["txt_a2"] = "%";
                dr["txt_a3"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Loss on Ignition";
                dr["txt_a2"] = "%";
                dr["txt_a3"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Total  Sulphur as Sulphur trioxide (SO3)";
                dr["txt_a2"] = "%";
                dr["txt_a3"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Magnesium Oxide (MgO)";
                dr["txt_a2"] = "%";
                dr["txt_a3"] = "";
                dt1.Rows.Add(dr);

                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "SiO2 +  Al2O3 + Fe2O3";
                dr["txt_b2"] = "%";
                dr["txt_b3"] = "Min 70.0";
                dr["txt_b4"] = "Min 50.0";

                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Silicon Dioxide (SiO2)";
                dr["txt_b2"] = "%";
                dr["txt_b3"] = "Min 35.0";
                dr["txt_b4"] = "Min 25.0";

                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Loss on Ignition";
                dr["txt_b2"] = "%";
                dr["txt_b3"] = "Max 5.0";
                dr["txt_b4"] = "Max 5.0";

                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Total  Sulphur as Sulphur trioxide (SO3)";
                dr["txt_b2"] = "%";
                dr["txt_b3"] = "Max 3.0";
                dr["txt_b4"] = "Max 3.0";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Magnesium Oxide (MgO)";
                dr["txt_b2"] = "%";
                dr["txt_b3"] = "Max 5.0";
                dr["txt_b4"] = "Max 5.0";
                dt2.Rows.Add(dr);

                ViewState["OtherTable2"] = dt2;
                grdDetail2.DataSource = dt2;
                grdDetail2.DataBind();
            }
            #endregion
            #region 26-16-124
            else if (ddl_ReportFor.SelectedValue == "26")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dr["txt_a6"] = "";
                dr["txt_a7"] = "";
                dr["txt_a8"] = "";
                dt1.Rows.Add(dr);

                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

            }
            #endregion
            #region 27-16-124
            else if (ddl_ReportFor.SelectedValue == "27")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                dr = dt1.NewRow();

                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dr["txt_a6"] = "";
                dr["txt_a7"] = "";
                dt1.Rows.Add(dr);

                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

            }
            #endregion
            #region 28-16-124
            else if (ddl_ReportFor.SelectedValue == "28")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));
                dr = dt1.NewRow();

                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Moisture Content";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "< 3 %";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Loss on Ignition";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "< 4 %";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Silica Content";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "> 85 %";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Residue on 45 micron";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "< 5 %";
                dt1.Rows.Add(dr);

                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

            }
            #endregion
            #region 29-18-126
            else if (ddl_ReportFor.SelectedValue == "29")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "10 mm";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "4.75 mm";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "2.36 mm";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "1.18 mm";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "600 Micron";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "300 Micron";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "150 Micron";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "75 Micron";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Pan";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "Total";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);

                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();
            }
            #endregion
            #region 30-16-124
            else if (ddl_ReportFor.SelectedValue == "30")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "-";
                dr["txt_a2"] = "60.9";
                dr["txt_a3"] = "20.6";
                dr["txt_a4"] = "15.3";
                dr["txt_a5"] = "13.395";
                dr["txt_a6"] = "698";

                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "-";
                dr["txt_a2"] = "60.5";
                dr["txt_a3"] = "20.4";
                dr["txt_a4"] = "15.1";
                dr["txt_a5"] = "13.680";
                dr["txt_a6"] = "734";

                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "-";
                dr["txt_a2"] = "60.8";
                dr["txt_a3"] = "20.5";
                dr["txt_a4"] = "15.9";
                dr["txt_a5"] = "15.830";
                dr["txt_a6"] = "799";

                dt1.Rows.Add(dr);
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

            }
            #endregion
            #region 31-16-124
            else if (ddl_ReportFor.SelectedValue == "31")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "-";
                dr["txt_a2"] = "18.53";
                dr["txt_a3"] = "15.438";
                dr["txt_a4"] = "20.03";
                dr["txt_a5"] = "";

                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "-";
                dr["txt_a2"] = "19.36";
                dr["txt_a3"] = "16.352";
                dr["txt_a4"] = "18.40";
                dr["txt_a5"] = "";

                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "-";
                dr["txt_a2"] = "17.388";
                dr["txt_a3"] = "13.890";
                dr["txt_a4"] = "25.18";
                dr["txt_a5"] = "";

                dt1.Rows.Add(dr);

                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

            }
            #endregion
            #region 32-16-124
            else if (ddl_ReportFor.SelectedValue == "32")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dr["txt_a6"] = "";
                dr["txt_a7"] = "";
                dr["txt_a8"] = "";
                dt1.Rows.Add(dr);

                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

            }
            #endregion
            #region 33-16-124
            else if (ddl_ReportFor.SelectedValue == "33")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "6 mm";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "125";
                dr["txt_a4"] = "599";
                dr["txt_a5"] = "";
                dr["txt_a6"] = "";
                dr["txt_a7"] = "";
                dr["txt_a8"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "126";
                dr["txt_a4"] = "600";
                dr["txt_a5"] = "";
                dr["txt_a6"] = "";
                dr["txt_a7"] = "";
                dr["txt_a8"] = "";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "134";
                dr["txt_a4"] = "600";
                dr["txt_a5"] = "";
                dr["txt_a6"] = "";
                dr["txt_a7"] = "";
                dr["txt_a8"] = "";
                dt1.Rows.Add(dr);

                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

            }
            #endregion
            #region 34
            if (ddl_ReportFor.SelectedValue == "34")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b1", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b2", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b3", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b4", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b5", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b6", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b7", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b8", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b9", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b10", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b11", typeof(string)));

                DataTable dt3 = new DataTable();
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c1", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c2", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c3", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c4", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c5", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c6", typeof(string)));

                dt3.Columns.Add(new DataColumn("txt_c7", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c8", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_c9", typeof(string)));

                DataTable dt4 = new DataTable();
                dt4.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt4.Columns.Add(new DataColumn("txt_d1", typeof(string)));
                dt4.Columns.Add(new DataColumn("txt_d2", typeof(string)));
                dt4.Columns.Add(new DataColumn("txt_d3", typeof(string)));
                dt4.Columns.Add(new DataColumn("txt_d4", typeof(string)));
                dt4.Columns.Add(new DataColumn("txt_d5", typeof(string)));

                dt4.Columns.Add(new DataColumn("txt_d6", typeof(string)));
                dt4.Columns.Add(new DataColumn("txt_d7", typeof(string)));
                dt4.Columns.Add(new DataColumn("txt_d8", typeof(string)));
                dt4.Columns.Add(new DataColumn("txt_d9", typeof(string)));

                dr = dt1.NewRow();
                dr["txt_a1"] = "Description";
                dr["txt_a2"] = "C+5";
                dr["txt_a3"] = "C+1.5";
                dr["txt_a4"] = "Unit";
                dr["txt_a5"] = "";
                dr["txt_a6"] = "Grade of concrete";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["txt_a1"] = "Cube compressive strength achieved";
                dr["txt_a4"] = "N/mm²";
                dr["txt_a6"] = "No of specimens";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["txt_a1"] = "Hence  C = Compressive strength/3";
                dr["txt_a4"] = "N/mm²";
                dr["txt_a6"] = "Date of casting";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["txt_a1"] = "Convert to Kg/cm²";                
                dr["txt_a4"] = "Kg/cm²";
                dr["txt_a6"] = "date of testing";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["txt_a1"] = "Add 5 kg/cm2 pressure as stated";                
                dr["txt_a4"] = "Kg/cm²";
                dr["txt_a6"] = "Age of concrete in days";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["txt_a1"] = "convert pressure in to load        C";
                dr["txt_a4"] = "kg";
                dr["txt_a6"] = "Diameter of cylinder (mm)";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["txt_a1"] = "Ultimate Load to be applied      C+5";
                dr["txt_a4"] = "kN";
                dr["txt_a6"] = "Length of the cylinder (mm)";
                dt1.Rows.Add(dr);

                dr = dt1.NewRow();
                dr["txt_a1"] = "Load increament for Stress/Strain Graph";
                dr["txt_a4"] = "kN";
                dr["txt_a6"] = "Area of cylinder (cm²)";
                dt1.Rows.Add(dr);
                               
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

                dr = dt2.NewRow();
                dr["txt_b1"] = "";
                dr["txt_b2"] = "";
                dr["txt_b3"] = "";
                dr["txt_b4"] = "";
                dr["txt_b5"] = "";
                dr["txt_b6"] = "";
                dr["txt_b7"] = "";
                dr["txt_b8"] = "";
                dr["txt_b9"] = "";
                dt2.Rows.Add(dr);

                ViewState["OtherTable2"] = dt2;
                grdDetail2.DataSource = dt2;
                grdDetail2.DataBind();

                dr = dt3.NewRow();
                dr["txt_c1"] = "";
                dr["txt_c2"] = "";
                dr["txt_c3"] = "";
                dr["txt_c4"] = "";
                dr["txt_c5"] = "";
                dr["txt_c6"] = "";
                dr["txt_c7"] = "";
                dr["txt_c8"] = "";
                dr["txt_c9"] = "";
                dt3.Rows.Add(dr);

                ViewState["OtherTable3"] = dt3;
                grdDetail3.DataSource = dt3;
                grdDetail3.DataBind();

                dr = dt4.NewRow();
                dr["txt_d1"] = "";
                dr["txt_d2"] = "";
                dr["txt_d3"] = "";
                dr["txt_d4"] = "";
                dr["txt_d5"] = "";
                dr["txt_d6"] = "";
                dr["txt_d7"] = "";
                dr["txt_d8"] = "";
                dr["txt_d9"] = "";
                dt4.Rows.Add(dr);

                ViewState["OtherTable4"] = dt4;
                grdDetail4.DataSource = dt4;
                grdDetail4.DataBind();
            }
            #endregion
            #region 35
            if (ddl_ReportFor.SelectedValue == "35")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));
                
                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);
                
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();
                
            }
            #endregion
            #region 36
            else if (ddl_ReportFor.SelectedValue == "36")
            {
                DataTable dt1 = new DataTable();
                DataRow dr = null;
                dt1.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a1", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a2", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a3", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a4", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a5", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a6", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a7", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a8", typeof(string)));
                dt1.Columns.Add(new DataColumn("txt_a9", typeof(string)));

                DataTable dt2 = new DataTable();
                dt2.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b1", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b2", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b3", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b4", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b5", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b6", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b7", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b8", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b9", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b10", typeof(string)));
                dt2.Columns.Add(new DataColumn("txt_b11", typeof(string)));

                dr = dt1.NewRow();
                dr["lblSrNo"] = dt1.Rows.Count + 1;
                dr["txt_a1"] = "";
                dr["txt_a2"] = "";
                dr["txt_a3"] = "";
                dr["txt_a4"] = "";
                dr["txt_a5"] = "";
                dt1.Rows.Add(dr);
                                
                ViewState["OtherTable1"] = dt1;
                grdDetail1.DataSource = dt1;
                grdDetail1.DataBind();

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "Less Than 100";
                dr["txt_b2"] = "Negligible";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "100-1000";
                dr["txt_b2"] = "Very Low";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "1000-2000";
                dr["txt_b2"] = "Low";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "2000-4000";
                dr["txt_b2"] = "Moderate";
                dt2.Rows.Add(dr);

                dr = dt2.NewRow();
                dr["lblSrNo"] = dt2.Rows.Count + 1;
                dr["txt_b1"] = "More Than 4000";
                dr["txt_b2"] = "High";
                dt2.Rows.Add(dr);

                ViewState["OtherTable2"] = dt2;
                grdDetail2.DataSource = dt2;
                grdDetail2.DataBind();
            }
            #endregion
        }

        protected void DisplayDefaultRemarks()
        {
            #region 1-15-123
            if (ddl_ReportFor.SelectedValue == "1")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                //dr["txt_REMARK"] = "IS 2547(Part -1) - 1976,RA-2002 - Specification For Gypsum Building Plasters(Part-I Excluding Premixed Lightweight Plasters).";
                dr["txt_REMARK"] = "IS 2547(Part -1 & 2) - 1976,RA-2002 - Specification For Gypsum Building Plasters(Part-I Excluding Premixed Lightweight Plasters).";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                //dr = dt3.NewRow();
                //dr["lblSrNo"] = dt3.Rows.Count + 1;
                //dr["txt_REMARK"] = "IS 2547(Part -2) - 1976,RA-2002 - Specification For Gypsum Building Plasters(Part-II Premixed Lightweight Plasters).";
                //dr["ddlRefType"] = "Reference";
                //dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 2542(Part 1/Sec 1 to 12) - 1978,RA-2002 - Methods of test for Gypsum Plaster, Concrete & Products.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "* Applicable to undercoat plasters only.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "+ Applicable to final coat plasters.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 2-16-124
            else if (ddl_ReportFor.SelectedValue == "2")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 2547(Part - 1) - 1976 : 'Specification For Gypsum Building Plaster.'";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 1288-1973 Method of test for mineral gypsum & gypsum product.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The Test Reports and results relate to the particular specimen/sample(s) of the material as delivered/received and tested in the laboratory.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "+ Applicable to metal lathing plaster.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 5-29-683
            else if (ddl_ReportFor.SelectedValue == "5")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 1121 - 1974 Part 1 Method of Test for Determination of Compressive Strength of Natural Building Stone.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 4464 - 1985 Code of Practice for presentation pf drilling information and core description in foundation investigation.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The Test Reports and results relate to the particular specimen/sample(s) of the material as delivered/received and tested in the laboratory.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The test report shall not be reproduced except in full, without the written from Durocrete.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 6-30-684
            else if (ddl_ReportFor.SelectedValue == "6")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The Test Reports and results relate to the particular specimen/sample(s) of the material as delivered/received and tested in the laboratory.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The test report shall not be reproduced except in full, without the written permission from Durocrete.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 7-31-685
            else if (ddl_ReportFor.SelectedValue == "7")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "# Average value rounded off to nearest 5mm for cover.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "* Bar diameter detected at 2-3 places at random on column.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "Results are given as per the images of profometer PM 650 equipment limitations of the equipment's are applicable to the results too.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "For details please refer to the images provided along with this report.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The Test Reports and result relates to the members tested as site.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "Any test report shall not be reproduced except in full, without permission from Durocrete.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 8-32-689
            else if (ddl_ReportFor.SelectedValue == "8")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 3025 : Method of sampling and test (physical & chemical) for water and waste water.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 1622 : Method of sampling & microbiological examination of water.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 10500 : Drinking water specification.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The given sample of water is suitable for drinking purpose.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "All test values are obtained by subcontracting with vendor lab.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 3-18-126
            else if (ddl_ReportFor.SelectedValue == "3")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 12269 - 1987 for OPC 53 Grade.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 12089 - 1987 Specification for granulated slag for the manufacture of portland slag cement.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "*BS 6699:1987 specification for GGBS use with ordinary portland cement.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The test reports and results relate to the particular specimen/sample(s) of the material as delivered/received and tested in the laboratory.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "Any test report shall not be reproduced except in full, without the written permission from Durocrete.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 9-33-690
            else if (ddl_ReportFor.SelectedValue == "9")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The test reports and results relate to the particular specimen/sample(s) of the material as delivered/received and tested in the laboratory.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The test report shall not be reproduced except in full, without the written permission from Durocrete.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 4-21-129
            else if (ddl_ReportFor.SelectedValue == "4")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 1734 : Part 1 to 20 : 1983 Methods of Test for Plywood.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The test reports and results relate to the particular specimen/sample(s) of the material as delivered/received and tested in the laboratory.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The test report shall not be reproduced except in full, without the written permission from Durocrete.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 10-34-501
            else if (ddl_ReportFor.SelectedValue == "10")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The test reports and results relate to the particular specimen/sample(s) of the material as delivered/received and tested in the laboratory.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The test report shall not be reproduced except in full, without the written permission from Durocrete.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 11-35-691
            else if (ddl_ReportFor.SelectedValue == "11")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "Guide book on non destructive testing of concrete structures by international atomic energy agency (2002)";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The test reports and results relate to the particular specimen/sample(s) of the material as delivered/received and tested in the laboratory.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The test report shall not be reproduced except in full, without the written permission from Durocrete.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 12-36-692
            else if (ddl_ReportFor.SelectedValue == "12")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "ASTM C876 standard test method for half cell potential of uncoated reinforcing steel in concrete.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "Guide book on non destructive testing of concrete structures by international atomic energy agency (2002)";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The test reports and results relate to the particular specimen/sample(s) of the material as delivered/received and tested in the laboratory.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The test report shall not be reproduced except in full, without the written permission from Durocrete.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 13-23-131
            else if (ddl_ReportFor.SelectedValue == "13")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS : 1786 - 2008," + '"' + "High Strength deformed steel bar and wires for concrete reinforcement-specification(fourth revision)." + '"';
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "As per IS 16172:2014(Clause 9.2.1) the minimum tensile strength shall not be less than 600N/mm².";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 14-3-111
            else if (ddl_ReportFor.SelectedValue == "14")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 9103 : 1999 Concrete Admixture Specification.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 6925: Specification for Methods of Test for Determination of Water Soluble Chlorides in Concrete Admixtures.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 15-37-383
            if (ddl_ReportFor.SelectedValue == "15")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 456:2000  Indian Standard Plain & Reinforced Concrete code of Practice.(Fourth Revision).";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 383 -1970 Coarse and fine aggregates from natural sources for concrete.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 2386 (Part II ) - 1963 - Methods of test for Aggregates for Concrete .(Estimation of Deletarious Material and Organic Impurity)";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 16-10-118
            else if (ddl_ReportFor.SelectedValue == "16")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 4020  : 1998 Method Of Test : Door Shutter.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 2202 (Part 1) 1999:- Specification for Wooden Flush Door Shutters (Solid Core Type).";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 17-25-133
            else if (ddl_ReportFor.SelectedValue == "17")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "Optical Emission Spectrometer(ASTM - E - 1086 - 2008).";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 18-34-501
            else if (ddl_ReportFor.SelectedValue == "18")
            {
                if (grdOtherRemark.Rows.Count == 0)
                    AddRowOtherRemark();
            }
            #endregion
            #region 19-37-383
            if (ddl_ReportFor.SelectedValue == "19")
            {

                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "BS 970 - Part3 : 1991 Specification for Wrought Steel For Mechanical and Allied Engineering Purposes(Part - 3 Bright Bars for General Engineering Purposes).";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "Optical Emission Spectrometer (ASTM - E - 415 - 2017).";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "Please collect tested specimen in 7 days, after which they will be disposed off";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "'#' This constituent does not cover under NABL scope";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 20-18-126
            else if (ddl_ReportFor.SelectedValue == "20")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "Testing is done onside by using pull off testor(Dolly size-50mm).";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);

                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion          
            #region 21-16-124
            else if (ddl_ReportFor.SelectedValue == "21")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = " ASTM C876 standard test method for half cell potential of uncoated reinforcing steel in concrete";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = " Guide book on non destructive testing of concrete structures by international atomic energy agency (2002)";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The average reading is greater than -200 mV, which is indicative of having less than  5 % chance of start of  corrosion activity in the RCC Members.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 22-16-124
            else if (ddl_ReportFor.SelectedValue == "22")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "BS EN 14630-2006 Test Methods- Determination of carbonation depth in hardened concrete by the Phenolphthalein method. ";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = " Guide book on non destructive testing of concrete structures by international atomic energy agency (2002)";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);

                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 23-18-126
            else if (ddl_ReportFor.SelectedValue == "23")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = " IS 14268:2017 - Uncoated stress relieved low relaxation seven-wire (Ply) strand for prestressed concrete-specification.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = " For 15.2mm 7-ply Min. breaking load should be 240200 N.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = " For 15.2mm 7-ply  the minimum percentage elongation should be 3.5%.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);

                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 24-18-126
            else if (ddl_ReportFor.SelectedValue == "24")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                //dr["txt_REMARK"] = "IS 12089-1987 Specification for granulated Slag for the manufacture of Portland Slag Cement.";
                dr["txt_REMARK"] = "IS 16714-2018 Specification for granulated Slag for the manufacture of Portland Slag Cement.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 4032-1985 Method of chemical analysis of hydraulic cement.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);



                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 25-16-124
            else if (ddl_ReportFor.SelectedValue == "25")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 3812(Part -1) - 2003 : 'Specification For Fuel Ash.'";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 1727 - 1967 Methods of test for Pozzolanic materials.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = " The given sample satisfies the criteria given in IS 3812 :2003.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);

                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 26-16-124
            else if (ddl_ReportFor.SelectedValue == "26")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "Testing done as per IS : 516 -1959 (Reaffirmed 2016) Clause No.8.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "Specifications as per IS 456:2000.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);

                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 27-16-124
            else if (ddl_ReportFor.SelectedValue == "27")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();

                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = " IS 516 Part-2 /Sec-1 : Hardened Concrete- Methods of Test Properties of Hardened Concrete other than Strength -Density of Hardened Concrete and Depth of Water Penetration Under Pressure";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);


                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "  All test values are obtained by subcontracting with vendor lab. (Durocrete Engineering Services Pvt. Ltd., Mumbai Branch)";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);

                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 28-16-124
            else if (ddl_ReportFor.SelectedValue == "28")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();

                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 15388-2003 (RA-2017) Silica Fume Specification";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);


                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 1727 - 1967 Methods of test for Pozzolanic materials.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);

                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 29-16-124
            else if (ddl_ReportFor.SelectedValue == "29")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();

                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = " IS 2386 : Part I - 1963 - Methods of test for Aggregates for concrete (Particle size and shape)";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);

                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 30-16-124
            else if (ddl_ReportFor.SelectedValue == "30")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 2185 (Part4) : 2008 CONCRETE MASONRY UNITS-SPECIFICATION Part 4 Preformed foam Cellular Concrete Blocks.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);

                //dr = dt3.NewRow();
                //dr["lblSrNo"] = dt3.Rows.Count + 1;
                //dr["txt_REMARK"] = " 2) Specifications as per IS 456:2000.";
                //dr["ddlRefType"] = "Reference";
                //dt3.Rows.Add(dr);

                //dr = dt3.NewRow();
                //dr["lblSrNo"] = dt3.Rows.Count + 1;
                //dr["txt_REMARK"] = " The given sample satisfies the criteria given in IS 3812 :2003.";
                //dr["ddlRefType"] = "Remark";
                //dt3.Rows.Add(dr);

                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();
            }
            #endregion
            #region 31-16-124
            else if (ddl_ReportFor.SelectedValue == "31")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 2185 (Part4) : 2008 CONCRETE MASONRY UNITS-SPECIFICATION Part 4 Preformed foam Cellular Concrete Blocks.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);

                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();
            }
            #endregion
            #region 32-16-124
            else if (ddl_ReportFor.SelectedValue == "32")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = " IS 2185 (Part4) : 2008 CONCRETE MASONRY UNITS-SPECIFICATION Part 4 Preformed foam Cellular Concrete Blocks.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);

                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();
            }
            #endregion
            #region 33-16-124
            else if (ddl_ReportFor.SelectedValue == "33")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 2185 (Part4) : 2008 CONCRETE MASONRY UNITS-SPECIFICATION Part 4 Preformed foam Cellular Concrete Blocks.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);

                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();
            }
            #endregion
            #region 34
            if (ddl_ReportFor.SelectedValue == "34")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 516-1959 RA 2016: Method of tests for strength of concrete clause No.8";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 35
            if (ddl_ReportFor.SelectedValue == "35")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "IS 516 (Part 2/Section 1) :2018 Hardened Concrete - Method of Test (Density of Hardened Concrete and Depth of Water Penetration Under Pressue)";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                
                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "Direction of application of water pressure on top of the specimen perpendicular to direction of testing.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The result above may be interpreted accordingly.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "The tested specimens may be collected from the laboratory within 7 days of date of testing, after which they shall be disposed.";
                dr["ddlRefType"] = "Remark";
                dt3.Rows.Add(dr);
                
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion
            #region 35
            if (ddl_ReportFor.SelectedValue == "36")
            {
                DataTable dt3 = new DataTable();
                DataRow dr = null;
                dt3.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt3.Columns.Add(new DataColumn("txt_REMARK", typeof(string)));
                dt3.Columns.Add(new DataColumn("ddlRefType", typeof(string)));

                dr = dt3.NewRow();
                dr["lblSrNo"] = dt3.Rows.Count + 1;
                dr["txt_REMARK"] = "ASTM C 1202-19  Standard Test Method For Electrical Indication of Concrete's Ability to Resist Chloride Ion Penetration.";
                dr["ddlRefType"] = "Reference";
                dt3.Rows.Add(dr);
                
                ViewState["OtherRemarkTable"] = dt3;
                grdOtherRemark.DataSource = dt3;
                grdOtherRemark.DataBind();

            }
            #endregion

        }
        protected void grdOtherRemark_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlRefType = (e.Row.FindControl("ddlRefType") as DropDownList);
                string refType = (e.Row.FindControl("lblType") as Label).Text;
                if (refType != "")
                {
                    ddlRefType.Items.FindByValue(refType).Selected = true;
                }
            }
        }
        public void ViewWitnessBy()
        {
            txt_witnessBy.Visible = false;
            chk_WitnessBy.Checked = false;
            if (lblEntry.Text == "Check")
            {
                var ct = dc.ReportStatus_View("Other Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
                foreach (var c in ct)
                {
                    if (c.OTINWD_WitnessBy_var != null && c.OTINWD_WitnessBy_var.ToString() != "")
                    {
                        txt_witnessBy.Visible = true;
                        txt_witnessBy.Text = c.OTINWD_WitnessBy_var.ToString();
                        chk_WitnessBy.Checked = true;
                    }
                }
            }

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

        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowOtherRemark();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (grdOtherRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdOtherRemark.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowOtherRemark(gvr.RowIndex);
            }
        }

        protected void imgBtnAddRow1_Click(object sender, CommandEventArgs e)
        {
            AddRowDetail1();
        }
        protected void imgBtnDeleteRow1_Click(object sender, CommandEventArgs e)
        {
            if (grdDetail1.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowDetail1(gvr.RowIndex);
            }
        }
        protected void imgBtnAddRow2_Click(object sender, CommandEventArgs e)
        {
            AddRowDetail2();
        }
        protected void imgBtnDeleteRow2_Click(object sender, CommandEventArgs e)
        {
            if (grdDetail2.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowDetail2(gvr.RowIndex);
            }
        }
        protected void imgBtnAddRow3_Click(object sender, CommandEventArgs e)
        {
            AddRowDetail3();
        }
        protected void imgBtnDeleteRow3_Click(object sender, CommandEventArgs e)
        {
            if (grdDetail3.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowDetail3(gvr.RowIndex);
            }
        }
        protected void imgBtnAddRow4_Click(object sender, CommandEventArgs e)
        {
            AddRowDetail4();
        }
        protected void imgBtnDeleteRow4_Click(object sender, CommandEventArgs e)
        {
            if (grdDetail4.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowDetail4(gvr.RowIndex);
            }
        }
        protected void imgBtnAddRow5_Click(object sender, CommandEventArgs e)
        {
            AddRowDetail5();
        }
        protected void imgBtnDeleteRow5_Click(object sender, CommandEventArgs e)
        {
            if (grdDetail5.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowDetail5(gvr.RowIndex);
            }
        }
        protected void imgBtnAddRow6_Click(object sender, CommandEventArgs e)
        {
            AddRowDetail6();
        }
        protected void imgBtnDeleteRow6_Click(object sender, CommandEventArgs e)
        {
            if (grdDetail6.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowDetail6(gvr.RowIndex);
            }
        }
        protected void imgBtnAddRow7_Click(object sender, CommandEventArgs e)
        {
            AddRowDetail7();
        }
        protected void imgBtnDeleteRow7_Click(object sender, CommandEventArgs e)
        {
            if (grdDetail7.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowDetail7(gvr.RowIndex);
            }
        }

        protected void imgBtnAddRow8_Click(object sender, CommandEventArgs e)
        {
            ImageButton img = (ImageButton)sender;
            GridViewRow row = (GridViewRow)img.Parent.Parent;
            int rowindex = row.RowIndex;
            AddRowDetail8(rowindex);
            ShowMergeRow();
        }
        protected void imgBtnDeleteRow8_Click(object sender, CommandEventArgs e)
        {
            if (grdDetail8.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                
                DeleteRowDetail8(gvr.RowIndex);
                ShowMergeRow();
            }
        }
        
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                Calculate();

                if (txtHeading.Text == null)
                {
                    txtHeading.Text = "";
                }
                DateTime TestingDt = DateTime.ParseExact(txt_DtOfTesting.Text, "dd/MM/yyyy", null);
                if (lbl_TestedBy.Text == "Tested By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, txt_SupplierName.Text, 2, Convert.ToInt32(ddl_ReportFor.SelectedValue), txtHeading.Text, Convert.ToInt32(ddl_Category.SelectedValue), ddlSection.SelectedValue, "", 0, 0, 0, "OT");
                    dc.ReportDetails_Update("OT", txt_ReferenceNo.Text, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), 0, 0, false, "Entered By");
                    dc.MISDetail_Update(0, "OT", txt_ReferenceNo.Text, "OT", null, true, false, false, false, false, false, false);
                }
                else if (lbl_TestedBy.Text == "Approve By")
                {
                    dc.AllInwd_Update(txt_ReferenceNo.Text, txt_witnessBy.Text, 0, TestingDt, txt_Description.Text, txt_SupplierName.Text, 3, Convert.ToInt32(ddl_ReportFor.SelectedValue), txtHeading.Text, Convert.ToInt32(ddl_Category.SelectedValue), ddlSection.SelectedValue, "", 0, 0, 0, "OT");
                    dc.ReportDetails_Update("OT", txt_ReferenceNo.Text, 0, 0, Convert.ToByte(Session["LoginId"]), Convert.ToByte(ddl_TestedBy.SelectedValue), false, "Checked By");
                    dc.MISDetail_Update(0, "OT", txt_ReferenceNo.Text, "OT", null, false, true, false, false, false, false, false);
                }

                clsData cd = new clsData();
                cd.updateNABLDetails(txt_ReferenceNo.Text, "OT", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                string OtherTestDetails = "", GridCheckedStatus = "";

                dc.OtherDetail_Update(txt_ReferenceNo.Text, "", true, "");
                dc.Title_Update(txt_ReferenceNo.Text, "", "OT", 0, true);
                
                if (grdDetail1.Visible == false && ddl_ReportFor.SelectedItem.Value != "22")
                {
                    if (chkBox1.Checked == true)
                        GridCheckedStatus = GridCheckedStatus + "1|";
                    else
                        GridCheckedStatus = GridCheckedStatus + "0|";

                    for (int i = 0; i < grdDetail1.Rows.Count; i++)
                    {
                        TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                        TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                        TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                        TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                        TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                        TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                        TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                        TextBox txt_a8 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");
                        TextBox txt_a9 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a9");
                        if (txt_a1.Visible == false)
                            OtherTestDetails = OtherTestDetails + txt_a1.Text + "~";
                        if (txt_a2.Visible == false)
                            OtherTestDetails = OtherTestDetails + txt_a2.Text + "~";
                        if (txt_a3.Visible == false)
                            OtherTestDetails = OtherTestDetails + txt_a3.Text + "~";
                        if (txt_a4.Visible == false)
                            OtherTestDetails = OtherTestDetails + txt_a4.Text + "~";
                        if (txt_a5.Visible == false)
                            OtherTestDetails = OtherTestDetails + txt_a5.Text + "~";
                        if (txt_a6.Visible == false)
                            OtherTestDetails = OtherTestDetails + txt_a6.Text + "~";
                        if (txt_a7.Visible == false)
                            OtherTestDetails = OtherTestDetails + txt_a7.Text + "~";
                        if (txt_a8.Visible == false)
                            OtherTestDetails = OtherTestDetails + txt_a8.Text + "~";
                        if (txt_a9.Visible == false)
                            OtherTestDetails = OtherTestDetails + txt_a9.Text + "~";
                        OtherTestDetails = OtherTestDetails + "|";
                    }
                }

                if (grdDetail1.Visible == true)
                {
                    if (chkBox1.Checked == true)
                        GridCheckedStatus = GridCheckedStatus + "1|";
                    else
                        GridCheckedStatus = GridCheckedStatus + "0|";

                    if (ddl_ReportFor.SelectedItem.Value == "31")
                    {
                        for (int i = 0; i < grdDetail1.Rows.Count; i++)
                        {
                            TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                            TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                            TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                            TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                            TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                            TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                            TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                            TextBox txt_a8 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");
                            TextBox txt_a9 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a9");
                            if (txt_a1.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a1.Text + "~";
                            if (txt_a2.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a2.Text + "~";
                            if (txt_a3.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a3.Text + "~";
                            if (txt_a4.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a4.Text + "~";
                            if (txt_a5.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a5.Text;
                            OtherTestDetails = OtherTestDetails + "|";
                        }

                    }
                    else if (ddl_ReportFor.SelectedItem.Value == "29")
                    {
                        for (int i = 0; i < grdDetail1.Rows.Count; i++)
                        {
                            TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                            TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                            TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                            TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                            TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                            TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                            TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                            TextBox txt_a8 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");
                            TextBox txt_a9 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a9");
                            if (txt_a1.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a1.Text + "~";
                            if (txt_a2.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a2.Text + "~";
                            if (txt_a3.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a3.Text + "~";
                            if (txt_a4.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a4.Text + "~";
                            if (txt_a5.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a5.Text;

                            // if (txt_a6.Visible == true)
                            //      OtherTestDetails = OtherTestDetails + txt_a6.Text + "~";
                            //  if (txt_a7.Visible == true)
                            //     OtherTestDetails = OtherTestDetails + txt_a7.Text;
                            //  if (txt_a8.Visible == true)
                            //     OtherTestDetails = OtherTestDetails + txt_a8.Text;
                            // if (txt_a9.Visible == true)
                            //    OtherTestDetails = OtherTestDetails + txt_a9.Text + "~";

                            OtherTestDetails = OtherTestDetails + "|";
                        }

                    }
                    else if (ddl_ReportFor.SelectedItem.Value == "32")
                    {
                        for (int i = 0; i < grdDetail1.Rows.Count; i++)
                        {
                            TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                            TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                            TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                            TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                            TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                            TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                            TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                            TextBox txt_a8 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");
                            TextBox txt_a9 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a9");
                            if (txt_a1.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a1.Text + "~";
                            if (txt_a2.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a2.Text + "~";
                            if (txt_a3.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a3.Text + "~";
                            if (txt_a4.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a4.Text + "~";
                            if (txt_a5.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a5.Text + "~";
                            if (txt_a6.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a6.Text + "~";
                            if (txt_a7.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a7.Text + "~";
                            if (txt_a8.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a8.Text;
                            //if (txt_a9.Visible == true)
                            //    OtherTestDetails = OtherTestDetails + txt_a9.Text + "~";

                            OtherTestDetails = OtherTestDetails + "|";
                        }

                    }
                    else if (ddl_ReportFor.SelectedItem.Value == "26")
                    {
                        for (int i = 0; i < grdDetail1.Rows.Count; i++)
                        {
                            TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                            TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                            TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                            TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                            TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                            TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                            TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                            TextBox txt_a8 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");
                            TextBox txt_a9 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a9");
                            if (txt_a1.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a1.Text + "~";
                            if (txt_a2.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a2.Text + "~";
                            if (txt_a3.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a3.Text + "~";
                            if (txt_a4.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a4.Text + "~";
                            if (txt_a5.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a5.Text + "~";
                            if (txt_a6.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a6.Text + "~";
                            if (txt_a7.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a7.Text + "~";
                            if (txt_a8.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a8.Text;
                            //if (txt_a9.Visible == true)
                            //    OtherTestDetails = OtherTestDetails + txt_a9.Text + "~";

                            OtherTestDetails = OtherTestDetails + "|";
                        }

                    }
                    else if (ddl_ReportFor.SelectedItem.Value == "27")
                    {
                        for (int i = 0; i < grdDetail1.Rows.Count; i++)
                        {
                            TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                            TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                            TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                            TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                            TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                            TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                            TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                            TextBox txt_a8 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");
                            TextBox txt_a9 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a9");
                            if (txt_a1.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a1.Text + "~";
                            if (txt_a2.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a2.Text + "~";
                            if (txt_a3.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a3.Text + "~";
                            if (txt_a4.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a4.Text + "~";
                            if (txt_a5.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a5.Text + "~";
                            if (txt_a6.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a6.Text + "~";
                            if (txt_a7.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a7.Text;
                            if (txt_a8.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a8.Text;
                            //if (txt_a9.Visible == true)
                            //    OtherTestDetails = OtherTestDetails + txt_a9.Text + "~";

                            OtherTestDetails = OtherTestDetails + "|";
                        }

                    }
                    else
                    {
                        for (int i = 0; i < grdDetail1.Rows.Count; i++)
                        {
                            TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                            TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                            TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                            TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                            TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                            TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                            TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                            TextBox txt_a8 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");
                            TextBox txt_a9 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a9");
                            if (txt_a1.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a1.Text + "~";
                            if (txt_a2.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a2.Text + "~";
                            if (txt_a3.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a3.Text + "~";
                            if (txt_a4.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a4.Text + "~";
                            if (txt_a5.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a5.Text + "~";
                            if (txt_a6.Visible == true)
                                OtherTestDetails = OtherTestDetails + txt_a6.Text + "~";

                            if (ddl_ReportFor.SelectedItem.Value == "30")
                            {
                                if (txt_a7.Visible == true)
                                    OtherTestDetails = OtherTestDetails + txt_a7.Text;
                                if (txt_a8.Visible == true)
                                    OtherTestDetails = OtherTestDetails + txt_a8.Text;
                            }
                            else
                            {
                                if (txt_a7.Visible == true)
                                    OtherTestDetails = OtherTestDetails + txt_a7.Text + "~";
                                if (txt_a8.Visible == true)
                                    OtherTestDetails = OtherTestDetails + txt_a8.Text + "~";
                                if (txt_a9.Visible == true)
                                    OtherTestDetails = OtherTestDetails + txt_a9.Text + "~";
                            }

                            OtherTestDetails = OtherTestDetails + "|";
                        }
                        if (ddl_ReportFor.SelectedItem.Value == "34")
                        {
                            OtherTestDetails = OtherTestDetails + lblHeading5.Text;
                            OtherTestDetails = OtherTestDetails + "|";
                        }                        
                    }
                }


                if (grdDetail2.Visible == true)
                {
                    if (chkBox2.Checked == true)
                        GridCheckedStatus = GridCheckedStatus + "1|";
                    else
                        GridCheckedStatus = GridCheckedStatus + "0|";
                    OtherTestDetails = OtherTestDetails + "$";
                    for (int i = 0; i < grdDetail2.Rows.Count; i++)
                    {
                        TextBox txt_b1 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b1");
                        TextBox txt_b2 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b2");
                        TextBox txt_b3 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b3");
                        TextBox txt_b4 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b4");
                        TextBox txt_b5 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b5");
                        TextBox txt_b6 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b6");
                        TextBox txt_b7 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b7");
                        TextBox txt_b8 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b8");
                        TextBox txt_b9 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b9");
                        TextBox txt_b10 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b10");
                        TextBox txt_b11 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b11");

                        if (txt_b1.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_b1.Text + "~";
                        if (txt_b2.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_b2.Text + "~";
                        if (txt_b3.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_b3.Text + "~";
                        if (txt_b4.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_b4.Text + "~";
                        if (txt_b5.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_b5.Text + "~";
                        if (txt_b6.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_b6.Text + "~";
                        if (txt_b7.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_b7.Text + "~";
                        if (txt_b8.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_b8.Text + "~";
                        if (txt_b9.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_b9.Text + "~";
                        if (txt_b10.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_b10.Text + "~";
                        if (txt_b11.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_b11.Text + "~";
                        OtherTestDetails = OtherTestDetails + "|";
                    }
                    if (ddl_ReportFor.SelectedItem.Value == "36")
                    {
                        OtherTestDetails = OtherTestDetails + "$";
                        OtherTestDetails = OtherTestDetails + txtDiameterOfSample.Text;
                        OtherTestDetails = OtherTestDetails + "~";
                        OtherTestDetails = OtherTestDetails + txtTemperatureDuringTest.Text;
                        OtherTestDetails = OtherTestDetails + "|";
                    }
                }

                if (grdDetail3.Visible == true)
                {
                    if (chkBox3.Checked == true)
                        GridCheckedStatus = GridCheckedStatus + "1|";
                    else
                        GridCheckedStatus = GridCheckedStatus + "0|";

                    OtherTestDetails = OtherTestDetails + "$";
                    for (int i = 0; i < grdDetail3.Rows.Count; i++)
                    {
                        TextBox txt_c1 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c1");
                        TextBox txt_c2 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c2");
                        TextBox txt_c3 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c3");
                        TextBox txt_c4 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c4");
                        TextBox txt_c5 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c5");
                        TextBox txt_c6 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c6");

                        TextBox txt_c7 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c7");
                        TextBox txt_c8 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c8");
                        TextBox txt_c9 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c9");

                        if (txt_c1.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_c1.Text + "~";
                        if (txt_c2.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_c2.Text + "~";
                        if (txt_c3.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_c3.Text + "~";
                        if (txt_c4.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_c4.Text + "~";
                        if (txt_c5.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_c5.Text + "~";
                        if (txt_c6.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_c6.Text + "~";

                        if (txt_c7.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_c7.Text + "~";
                        if (txt_c8.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_c8.Text + "~";
                        if (txt_c9.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_c9.Text + "~";
                        OtherTestDetails = OtherTestDetails + "|";
                    }
                }
                if (grdDetail4.Visible == true)
                {
                    if (chkBox4.Checked == true)
                        GridCheckedStatus = GridCheckedStatus + "1|";
                    else
                        GridCheckedStatus = GridCheckedStatus + "0|";

                    OtherTestDetails = OtherTestDetails + "$";
                    for (int i = 0; i < grdDetail3.Rows.Count; i++)
                    {
                        TextBox txt_d1 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d1");
                        TextBox txt_d2 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d2");
                        TextBox txt_d3 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d3");
                        TextBox txt_d4 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d4");
                        TextBox txt_d5 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d5");

                        TextBox txt_d6 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d6");
                        TextBox txt_d7 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d7");
                        TextBox txt_d8 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d8");
                        TextBox txt_d9 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d9");
                        if (txt_d1.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_d1.Text + "~";
                        if (txt_d2.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_d2.Text + "~";
                        if (txt_d3.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_d3.Text + "~";
                        if (txt_d4.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_d4.Text + "~";
                        if (txt_d5.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_d5.Text + "~";

                        if (txt_d6.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_d6.Text + "~";
                        if (txt_d7.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_d7.Text + "~";
                        if (txt_d8.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_d8.Text + "~";
                        if (txt_d9.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_d9.Text + "~";
                        OtherTestDetails = OtherTestDetails + "|";
                    }
                }
                if (grdDetail5.Visible == true)
                {
                    if (chkBox5.Checked == true)
                        GridCheckedStatus = GridCheckedStatus + "1|";
                    else
                        GridCheckedStatus = GridCheckedStatus + "0|";

                    OtherTestDetails = OtherTestDetails + "$";
                    for (int i = 0; i < grdDetail5.Rows.Count; i++)
                    {
                        TextBox txt_e1 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e1");
                        TextBox txt_e2 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e2");
                        TextBox txt_e3 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e3");
                        TextBox txt_e4 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e4");
                        TextBox txt_e5 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e5");
                        if (txt_e1.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_e1.Text + "~";
                        if (txt_e2.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_e2.Text + "~";
                        if (txt_e3.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_e3.Text + "~";
                        if (txt_e4.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_e4.Text + "~";
                        if (txt_e5.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_e5.Text + "~";
                        OtherTestDetails = OtherTestDetails + "|";
                    }
                }
                if (grdDetail6.Visible == true)
                {
                    if (chkBox6.Checked == true)
                        GridCheckedStatus = GridCheckedStatus + "1|";
                    else
                        GridCheckedStatus = GridCheckedStatus + "0|";

                    OtherTestDetails = OtherTestDetails + "$";
                    for (int i = 0; i < grdDetail6.Rows.Count; i++)
                    {
                        TextBox txt_f1 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f1");
                        TextBox txt_f2 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f2");
                        TextBox txt_f3 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f3");
                        TextBox txt_f4 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f4");
                        TextBox txt_f5 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f5");
                        if (txt_f1.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_f1.Text + "~";
                        if (txt_f2.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_f2.Text + "~";
                        if (txt_f3.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_f3.Text + "~";
                        if (txt_f4.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_f4.Text + "~";
                        if (txt_f5.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_f5.Text + "~";
                        OtherTestDetails = OtherTestDetails + "|";
                    }
                }
                if (grdDetail7.Visible == true)
                {
                    if (chkBox7.Checked == true)
                        GridCheckedStatus = GridCheckedStatus + "1|";
                    else
                        GridCheckedStatus = GridCheckedStatus + "0|";

                    OtherTestDetails = OtherTestDetails + "$";
                    for (int i = 0; i < grdDetail7.Rows.Count; i++)
                    {
                        TextBox txt_g1 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g1");
                        TextBox txt_g2 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g2");
                        TextBox txt_g3 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g3");
                        TextBox txt_g4 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g4");
                        TextBox txt_g5 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g5");
                        if (txt_g1.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_g1.Text + "~";
                        if (txt_g2.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_g2.Text + "~";
                        if (txt_g3.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_g3.Text + "~";
                        if (txt_g4.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_g4.Text + "~";
                        if (txt_g5.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_g5.Text + "~";
                        OtherTestDetails = OtherTestDetails + "|";
                    }
                }

                if (grdDetail8.Visible == true)
                {
                    if (chkBox8.Checked == true)
                        GridCheckedStatus = GridCheckedStatus + "1|";
                    else
                        GridCheckedStatus = GridCheckedStatus + "0|";
                    if (ddl_ReportFor.SelectedValue == "21" )
                        OtherTestDetails = OtherTestDetails + "$";
                    for (int i = 0; i < grdDetail8.Rows.Count; i++)
                    {
                        Label lblMergFlag = (Label)grdDetail8.Rows[i].FindControl("lblMergFlag");
                        TextBox txt_a1 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a1");
                        TextBox txt_a2 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a2");
                        TextBox txt_a3 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a3");
                        TextBox txt_a4 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a4");
                        TextBox txt_a5 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a5");
                        TextBox txt_a6 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a6");
                        TextBox txt_a7 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a7");
                        TextBox txt_a8 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a8");
                        TextBox txt_a9 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a9");

                        if (lblMergFlag.Visible == true)
                            OtherTestDetails = OtherTestDetails + lblMergFlag.Text + "~";
                        if (txt_a1.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_a1.Text + "~";
                        if (txt_a2.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_a2.Text + "~";
                        if (txt_a3.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_a3.Text + "~";
                        if (txt_a4.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_a4.Text + "~";
                        if (txt_a5.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_a5.Text + "~";
                        if (txt_a6.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_a6.Text + "~";
                        if (txt_a7.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_a7.Text + "~";
                        if (txt_a8.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_a8.Text + "~";
                        if (txt_a9.Visible == true)
                            OtherTestDetails = OtherTestDetails + txt_a9.Text + "~";
                        OtherTestDetails = OtherTestDetails + "|";
                    }
                }

                dc.OtherDetail_Update(txt_ReferenceNo.Text, OtherTestDetails, false, GridCheckedStatus);

                #region Remark Gridview
                int RemarkId = 0;
                dc.AllRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "OT", true);
                for (int i = 0; i < grdOtherRemark.Rows.Count; i++)
                {
                    TextBox txt_REMARK = (TextBox)grdOtherRemark.Rows[i].FindControl("txt_REMARK");
                    DropDownList ddlRefType = (DropDownList)grdOtherRemark.Rows[i].FindControl("ddlRefType");
                    if (txt_REMARK.Text != "")
                    {
                        bool valid = false;
                        var chcek = dc.AllRemark_View(txt_REMARK.Text, "", 0, "OT");
                        foreach (var n in chcek)
                        {
                            valid = true;
                            RemarkId = Convert.ToInt32(n.OT_RemarkId_int);
                            Boolean chk = false;
                            var chkId = dc.AllRemark_View("", txt_ReferenceNo.Text, 0, "OT");
                            foreach (var c in chkId)
                            {
                                if (c.OTDetail_RemarkId_int == RemarkId)
                                {
                                    chk = true;
                                }
                            }
                            if (chk == false)
                            {
                                dc.OtherRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "OT", ddlRefType.SelectedItem.Text, false);
                            }
                        }
                        if (valid == false)
                        {
                            dc.AllRemark_Update(0, txt_REMARK.Text, "OT");
                            var chc = dc.AllRemark_View(txt_REMARK.Text, "", 0, "OT");
                            foreach (var n in chc)
                            {
                                RemarkId = Convert.ToInt32(n.OT_RemarkId_int);
                                dc.OtherRemarkDetail_Update(txt_ReferenceNo.Text, RemarkId, "OT", ddlRefType.SelectedItem.Text, false);
                            }
                        }
                    }
                }
                #endregion

                if (lbl_TestedBy.Text == "Approve By")
                {
                    string strInterBranchRefNo = "";
                    dc.Inward_Update_ULRNo(Convert.ToInt32(lblRecordNo.Text), "OT", txt_ReferenceNo.Text, ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text), "OT");
                    var wInwd = dc.ReportStatus_View("Other Testing", null, null, 1, 0, 0, Convert.ToString(txt_ReferenceNo.Text), 0, 0, 0);
                    foreach (var w in wInwd)
                    {
                        lblULRNo.Text = "ULR No : " + w.OTINWD_ULRNo_var;
                        lblULRNo.Visible = true;
                        strInterBranchRefNo = w.OTINWD_InterBranchRefNo_var;
                    }
                    //Approve on check if interbranch reference no.
                    if (strInterBranchRefNo != "")
                    {
                        ApproveReports("OT", txt_ReferenceNo.Text, Convert.ToInt32(lblRecordNo.Text)); 
                                                                                                        
                        int siteCRbypssBit = 0; 
                        siteCRbypssBit = cd.getClientCrlBypassBit("OT", Convert.ToInt32(lblRecordNo.Text));
                        if (siteCRbypssBit == 1)
                            dc.MISDETAIL_Update_CRLimitBit(txt_ReferenceNo.Text, "OT");

                        //SMS
                        var res = dc.SMSDetailsForReportApproval_View(Convert.ToInt32(lblRecordNo.Text), "OT").ToList();
                        if (res.Count > 0)
                        {
                            cd.sendInwardReportMsg(Convert.ToInt32(res.FirstOrDefault().ENQ_Id), Convert.ToString(res.FirstOrDefault().ENQ_Date_dt), Convert.ToInt32(res.FirstOrDefault().SITE_MonthlyBillingStatus_bit).ToString(), res.FirstOrDefault().crLimitExceededStatus.ToString(), res.FirstOrDefault().BILL_NetAmt_num.ToString(), txt_ReferenceNo.Text, Convert.ToString(res.FirstOrDefault().INWD_ContactNo_var), "Report");
                        }
                    }
                }
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkPrint.Visible = true;
                lnkSave.Enabled = false;
                lnkCalculate.Enabled = false;
                Session["FileUpload1"] = null;

            }
        }
        public void ApproveReports(string Recordtype, string ReferenceNo, int RecordNo) //, string EmailId, string ContactNo)
        {
            string tempRecType = Recordtype;
            string testType = Recordtype;

            #region Bill Generation
            bool approveRptFlag = true;
            bool generateBillFlag = true;
            string BillNo = "0";
            if (DateTime.Now.Day >= 26)
            {
                generateBillFlag = false;
            }
            if (generateBillFlag == true)
            {
                var inward = dc.Inward_View(0, RecordNo, Recordtype, null, null).ToList();
                foreach (var inwd in inward)
                {
                    if (inwd.INWD_BILL_Id != null && inwd.INWD_BILL_Id != "0")
                    {
                        BillNo = inwd.INWD_BILL_Id;
                        generateBillFlag = false;
                    }
                    if (inwd.SITE_MonthlyBillingStatus_bit != null && inwd.SITE_MonthlyBillingStatus_bit == true)
                    {
                        generateBillFlag = false;
                    }
                    if (inwd.INWD_MonthlyBill_bit == true)
                    {
                        generateBillFlag = false;
                    }
                }
            }            
            if (generateBillFlag == true)
            {
                var withoutbill = dc.WithoutBill_View(RecordNo, Recordtype);
                if (withoutbill.Count() > 0)
                {
                    generateBillFlag = false;
                }
            }
            if (generateBillFlag == true)
            {
                int NewrecNo = 0;
                clsData clsObj = new clsData();
                NewrecNo = clsObj.GetCurrentRecordNo("BillNo");
                //var gstbillCount = dc.Bill_View("0", 0, 0, "", 0, false, false, DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                var gstbillCount = dc.Bill_View_Count(DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                if (gstbillCount.Count() != NewrecNo - 1)
                {
                    generateBillFlag = false;
                    approveRptFlag = false;
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Bill No. mismatch. Can not approve report.');", true);
                }
            }
            if (approveRptFlag == true)
            {
                //Generate bill
                if (generateBillFlag == true)
                {
                    BillUpdation bill = new BillUpdation();
                    BillNo = bill.UpdateBill(Recordtype, RecordNo, BillNo);
                }
                //
                dc.ReportDetails_Update(tempRecType, ReferenceNo, 0, 0, 0, Convert.ToByte(ddl_TestedBy.SelectedValue), true, "Approved By");
                dc.MISDetail_Update(0, Recordtype, ReferenceNo, testType, null, false, false, true, false, false, false, false);
            }
            #endregion
            
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Report Approved Successfully.";
            lblMsg.Visible = true;
        }
        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            Calculate();
        }

        private void Calculate()
        {
            if (ValidateData())
            {
                if (ddl_ReportFor.SelectedItem.Value == "13")
                    CalculateReinforcementSpliceBar();
                if (ddl_ReportFor.SelectedItem.Value == "21")
                    CalculateHalfCellPotentiometer();
                //if (ddl_ReportFor.SelectedItem.Value == "23")
                //   CalculatePTStrandCable();
                if (ddl_ReportFor.SelectedItem.Value == "26")
                    CalculateFlexuralStrength();
                if (ddl_ReportFor.SelectedItem.Value == "27")
                    CalculateWaterPermability();
                if (ddl_ReportFor.SelectedItem.Value == "29")
                    CalculationSieveAnalysis();
                if (ddl_ReportFor.SelectedItem.Value == "30")
                    CalculateCLCDensityTest();
                if (ddl_ReportFor.SelectedItem.Value == "31")
                    CalculateCLCWATERABSORPTION();
                if (ddl_ReportFor.SelectedItem.Value == "32")
                    CalculateCLCCOMPRESSIVETEST();
                if (ddl_ReportFor.SelectedItem.Value == "34")
                    CalculateMOE();
                if (ddl_ReportFor.SelectedItem.Value == "35")
                    CalculateWaterPermeabilityAvg();
                if (ddl_ReportFor.SelectedItem.Value == "36")
                    CalculateRapidChloridePenetration();
            }
        }
        public void CalculateRapidChloridePenetration()
        {
            decimal total = 0;
            for (int i = 0; i < grdDetail1.Rows.Count; i++)
            {
                TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");

                total += Convert.ToDecimal(txt_a3.Text);
                if (i > 0)
                {
                    txt_a4.Text = " ";
                    txt_a5.Text = " ";
                }
            }
            if (grdDetail1.Rows.Count > 0)
            {
                TextBox txt_a4_Avg = (TextBox)grdDetail1.Rows[0].FindControl("txt_a4");
                TextBox txt_a5_Penetrability = (TextBox)grdDetail1.Rows[0].FindControl("txt_a5");
                txt_a4_Avg.Text = (total / Convert.ToDecimal(grdDetail1.Rows.Count)).ToString("0.00");                
                decimal average = Convert.ToDecimal(txt_a4_Avg.Text);
                string strPenetrability = "";
                for (int i = 0; i < grdDetail2.Rows.Count; i++)
                {
                    TextBox txt_b1 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b1");
                    TextBox txt_b2 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b2");

                    decimal lowerLimit = 0, upperLimit = 0;
                    if (txt_b1.Text.Contains("Less Than ") == true)
                    {
                        lowerLimit = 0;
                        upperLimit = Convert.ToDecimal(txt_b1.Text.Replace("Less Than ", ""));
                        if (average > lowerLimit && average <= upperLimit)
                            strPenetrability = txt_b2.Text;
                    }
                    else if (txt_b1.Text.Contains("More Than ") == true)
                    {                        
                        lowerLimit = Convert.ToDecimal(txt_b1.Text.Replace("More Than ", ""));
                        upperLimit = 0;
                        if (average > lowerLimit)
                            strPenetrability = txt_b2.Text;
                    }
                    else
                    {
                        string[] strVal = txt_b1.Text.Split('-');
                        lowerLimit = Convert.ToDecimal(strVal[0]);
                        upperLimit = Convert.ToDecimal(strVal[1]);
                        if (average > lowerLimit && average <= upperLimit)
                            strPenetrability = txt_b2.Text;
                    }
                }
                txt_a5_Penetrability.Text = strPenetrability;
            }
        }
        public void CalculateWaterPermeabilityAvg()
        {
            decimal total = 0;
            for (int i = 0; i < grdDetail1.Rows.Count; i++)
            {
                TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");

                total += Convert.ToDecimal(txt_a3.Text);
                if (i > 0)
                {
                    txt_a4.Text = " ";
                    txt_a5.Text = " ";
                }
                if (txt_a5.Text == "")
                    txt_a5.Text = " ";
            }
            if (grdDetail1.Rows.Count > 0)
            {
                TextBox txt_a4_Avg = (TextBox)grdDetail1.Rows[0].FindControl("txt_a4");
                txt_a4_Avg.Text = (total / Convert.ToDecimal(grdDetail1.Rows.Count)).ToString("0.00");
            }
        }
        public void CalculateReinforcementSpliceBar()
        {
            double csArea = 0;
            double ultTensile = 0;

            for (int i = 0; i < grdDetail1.Rows.Count; i++)
            {
                TextBox txt_dia = (TextBox)grdDetail1.Rows[i].Cells[3].FindControl("txt_a1");
                TextBox txt_load = (TextBox)grdDetail1.Rows[i].Cells[5].FindControl("txt_a3");
                TextBox txt_cs = (TextBox)grdDetail1.Rows[i].Cells[6].FindControl("txt_a4");
                TextBox txt_tensile = (TextBox)grdDetail1.Rows[i].Cells[7].FindControl("txt_a5");

                csArea = Math.Round((3.14 / 4 * (Convert.ToDouble(txt_dia.Text)) * (Convert.ToDouble(txt_dia.Text))), 2);
                txt_cs.Text = csArea.ToString("0.00");

                ultTensile = Math.Round((Convert.ToDouble(txt_load.Text) / csArea * 1000), 2);
                txt_tensile.Text = ultTensile.ToString("0.00");
            }
        }
        public void CalculateHalfCellPotentiometer()
        {
            double corrAvgReading = 0;
            for (int i = 0; i < grdDetail8.Rows.Count; i++)
            {
                Label lblMergFlag = (Label)grdDetail8.Rows[i].FindControl("lblMergFlag");
                if (lblMergFlag.Text != "1")
                {
                    TextBox txt_avgReading = (TextBox)grdDetail8.Rows[i].FindControl("txt_a2");
                    TextBox txt_temp = (TextBox)grdDetail8.Rows[i].FindControl("txt_a3");
                    TextBox txt_tempCoeff = (TextBox)grdDetail8.Rows[i].FindControl("txt_a4");
                    TextBox txt_corrAvgReading = (TextBox)grdDetail8.Rows[i].FindControl("txt_a5");

                    corrAvgReading = Math.Round((Convert.ToDouble(txt_avgReading.Text) + (((Convert.ToDouble(txt_temp.Text) * 1.8 + 32) - 72) * Convert.ToDouble(txt_tempCoeff.Text))), 2);
                    txt_corrAvgReading.Text = corrAvgReading.ToString("0.00");
                }
            }
        }
        private void CalculatePTStrandCable()
        {
            decimal cs = 0, avgcs = 0;

            for (int i = 0; i < grdDetail1.Rows.Count; i++)
            {
                //compressive strength
                TextBox txtNominalDia = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                TextBox txtIDMark = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                TextBox txtCSArea = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                TextBox txtElongation = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                TextBox txtBreakingLoad = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                TextBox txtLoad = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                TextBox txtFlexStrength = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                TextBox txtFlexStrengthAvg = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");

                cs = 0;
                if (txtCSArea.Text != "" && txtElongation.Text != "")
                {
                    //txtThickness.Text = (Convert.ToDecimal(txtLength.Text) * Convert.ToDecimal(txtWidth.Text)).ToString("0.0");

                    if (txtLoad.Text != "")
                    {
                        // if (Convert.ToDecimal(txtThickness.Text) > 0)

                        cs = (Convert.ToDecimal(txtLoad.Text) * Convert.ToDecimal(txtCSArea.Text)) * 1000 / (Convert.ToDecimal(txtElongation.Text) * (Convert.ToDecimal(txtBreakingLoad.Text) * Convert.ToDecimal(txtBreakingLoad.Text)));

                        // cs = Convert.ToDecimal(txtLoad.Text) / Convert.ToDecimal(txtThickness.Text) * 1000;
                        // else
                        // cs = Convert.ToDecimal(txtLoad.Text) * 1000;

                        txtBreakingLoad.Text = cs.ToString("0.00");
                        avgcs = avgcs + cs;
                    }
                }
                txtBreakingLoad.Text = "";
            }


            //TextBox txtFlexStrengthAvg1 = (TextBox)grdDetail1.Rows[(grdDetail1.Rows.Count / 2)].FindControl("txt_a8");
            //txtFlexStrengthAvg1.Text = "0.00";
            //if (avgcs > 0)
            //    txtFlexStrengthAvg1.Text = (avgcs / grdDetail1.Rows.Count).ToString("0.00");

            //if (grdDetail1.Rows.Count < 3)
            //    txtFlexStrengthAvg1.Text = "***";
        }
        private void CalculateFlexuralStrength()
        {
            decimal cs = 0, avgcs = 0;

            for (int i = 0; i < grdDetail1.Rows.Count; i++)
            {
                //compressive strength
                TextBox txtIdMark = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                TextBox txtAge = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                TextBox txtLength = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                TextBox txtWidth = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                TextBox txtThickness = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                TextBox txtLoad = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                TextBox txtFlexStrength = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                TextBox txtFlexStrengthAvg = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");

                cs = 0;
                if (txtLength.Text != "" && txtWidth.Text != "" && txtLoad.Text != "" && txtThickness.Text != "")
                {
                    if (Convert.ToDecimal(txtWidth.Text) != 0 && Convert.ToDecimal(txtThickness.Text) != 0)
                    {
                        cs = (Convert.ToDecimal(txtLoad.Text) * Convert.ToDecimal(txtLength.Text)) * 1000 / (Convert.ToDecimal(txtWidth.Text) * (Convert.ToDecimal(txtThickness.Text) * Convert.ToDecimal(txtThickness.Text)));
                        txtFlexStrength.Text = cs.ToString("0.00");
                        avgcs = avgcs + cs;
                    }
                }
                txtFlexStrengthAvg.Text = "";
            }
            // TextBox txtFlexStrengthAvg1 = (TextBox)grdDetail1.Rows[(grdDetail1.Rows.Count / 2)].FindControl("txt_a8");     
            TextBox txtFlexStrengthAvg1 = (TextBox)grdDetail1.Rows[0].FindControl("txt_a8");

            txtFlexStrengthAvg1.Text = "0.00";
            if (avgcs > 0)
                txtFlexStrengthAvg1.Text = (avgcs / grdDetail1.Rows.Count).ToString("0.00");

            if (grdDetail1.Rows.Count < 3)
                txtFlexStrengthAvg1.Text = "***";
        }
        private void CalculateWaterPermability()
        {
            decimal cs = 0, avgcs = 0;

            for (int i = 0; i < grdDetail1.Rows.Count; i++)
            {
                //compressive strength
                TextBox txtIdMark = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                TextBox txtLength = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                TextBox txtWidth = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                TextBox txtHeight = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                TextBox txtDepthofPenetration = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                TextBox txtAvgDepthofPenetration = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");

                TextBox txtAtPressure = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");


                cs = 0;
                if (txtLength.Text != "" && txtWidth.Text != "")
                {
                    //   txtThickness.Text = (Convert.ToDecimal(txtLength.Text) * Convert.ToDecimal(txtWidth.Text)).ToString("0.0");

                    if (txtDepthofPenetration.Text != "")
                    {
                        // if (Convert.ToDecimal(txtThickness.Text) > 0)

                        //  cs =  Convert.ToDecimal(txtLength.Text) * 1000 / (Convert.ToDecimal(txtWidth.Text) * (Convert.ToDecimal(txtHeight.Text) * Convert.ToDecimal(txtHeight.Text)));

                        //   cs = Convert.ToDecimal(txtLoad.Text) / Convert.ToDecimal(txtThickness.Text) * 1000;
                        //  else

                        cs = Convert.ToDecimal(txtDepthofPenetration.Text);

                        // txtDepthofPenetration.Text = cs.ToString("0.00");

                        avgcs = avgcs + cs;
                    }
                }
                txtAvgDepthofPenetration.Text = "";
            }


            TextBox txtAvgDepthofPenetration1 = (TextBox)grdDetail1.Rows[(grdDetail1.Rows.Count / 2)].FindControl("txt_a6");
            txtAvgDepthofPenetration1.Text = "0.00";
            if (avgcs >= 0)
                txtAvgDepthofPenetration1.Text = (avgcs / grdDetail1.Rows.Count).ToString("0.00");

            if (grdDetail1.Rows.Count < 3)
                txtAvgDepthofPenetration1.Text = "***";
        }
        public void CalculationSieveAnalysis()
        {
            double SpecificGravity = 0;
            double tempValue = 0;
            decimal val = 0;

            double TotalAmt = 0;
            double PreviousrowWt = 0;
            double FM = 0;


            for (int i = 0; i < grdDetail1.Rows.Count; i++)
            {
                TextBox txt_SieveSize = (TextBox)grdDetail1.Rows[i].Cells[1].FindControl("txt_a1");
                TextBox txt_Wt = (TextBox)grdDetail1.Rows[i].Cells[2].FindControl("txt_a2");
                TextBox txt_WtRet = (TextBox)grdDetail1.Rows[i].Cells[2].FindControl("txt_a3");


                if (txt_SieveSize.Text != "Total")
                {
                    TotalAmt += Convert.ToDouble(txt_Wt.Text);
                }
                else
                {
                    txt_Wt.Text = TotalAmt.ToString();

                    break;
                }
            }


            for (int i = 0; i < grdDetail1.Rows.Count; i++)
            {
                TextBox txt_SieveSize = (TextBox)grdDetail1.Rows[i].Cells[1].FindControl("txt_a1");
                TextBox txt_Wt = (TextBox)grdDetail1.Rows[i].Cells[2].FindControl("txt_a2");
                TextBox txt_WtRet = (TextBox)grdDetail1.Rows[i].Cells[3].FindControl("txt_a3");
                TextBox txt_CumWtRet = (TextBox)grdDetail1.Rows[i].Cells[4].FindControl("txt_a4");
                TextBox txt_CumuPassing = (TextBox)grdDetail1.Rows[i].Cells[5].FindControl("txt_a5");

                if (i > 0)
                {
                    TextBox txt_CumWtRetpre = (TextBox)grdDetail1.Rows[i - 1].Cells[4].FindControl("txt_a4");
                    PreviousrowWt = Convert.ToDouble(txt_CumWtRetpre.Text);
                }
                else
                {
                    PreviousrowWt = 0;
                }
                if (TotalAmt > 0)
                {
                    txt_WtRet.Text = ((Convert.ToDouble(txt_Wt.Text) * 100) / TotalAmt).ToString("0.000");
                }

                txt_CumWtRet.Text = (PreviousrowWt + Convert.ToDouble(txt_WtRet.Text)).ToString("0.00");
                txt_CumuPassing.Text = (100 - Convert.ToDouble(txt_CumWtRet.Text)).ToString("0.00");

                val = Convert.ToDecimal(txt_CumWtRet.Text);
                if (val > 100)
                    val = Convert.ToDecimal(100.00);
                else if (val < 0)
                    val = Convert.ToDecimal(0.00);
                txt_CumWtRet.Text = Convert.ToString(val);

                val = Convert.ToDecimal(txt_CumuPassing.Text);
                if (val > 100)
                    val = Convert.ToDecimal(100.00);
                else if (val < 0)
                    val = Convert.ToDecimal(0.00);

                txt_CumuPassing.Text = Convert.ToString(val);

                if (txt_SieveSize.Text != "Pan" && txt_SieveSize.Text != "Total" && txt_SieveSize.Text != "75 micron")
                {
                    FM += Convert.ToDouble(txt_CumWtRet.Text);
                }
                //}
                if (txt_SieveSize.Text == "Total")
                {

                    //txt_WtRet.Text = "0";
                    txt_CumWtRet.Text = "";
                    txt_CumuPassing.Text = "";

                }
            }


        }
        private void CalculateCLCDensityTest()
        {
            decimal cs = 0, avgcs = 0;

            for (int i = 0; i < grdDetail1.Rows.Count; i++)
            {
                //compressive strength
                TextBox txtIdMark = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                TextBox txtLength = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                TextBox txtBreadth = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                TextBox txtHeight = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                TextBox txtWeight = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                TextBox txtDensity = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                TextBox txtAvgDensity = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");

                cs = 0;
                if (txtLength.Text != "" && txtBreadth.Text != "")
                {
                    //   txtThickness.Text = (Convert.ToDecimal(txtLength.Text) * Convert.ToDecimal(txtWidth.Text)).ToString("0.0");

                    if (txtWeight.Text != "")
                    {
                        // if (Convert.ToDecimal(txtThickness.Text) > 0)

                        cs = Convert.ToDecimal(txtWeight.Text) / (Convert.ToDecimal(txtLength.Text) * Convert.ToDecimal(txtBreadth.Text) * Convert.ToDecimal(txtHeight.Text)) * 1000000;

                        //   cs = Convert.ToDecimal(txtLoad.Text) / Convert.ToDecimal(txtThickness.Text) * 1000;
                        //  else
                        // cs = Convert.ToDecimal(txtDensity.Text);

                        txtDensity.Text = cs.ToString("0.00");
                        avgcs = avgcs + cs;
                    }
                }
                txtAvgDensity.Text = "";
            }


            //   TextBox txtAvgDensity1 = (TextBox)grdDetail1.Rows[(grdDetail1.Rows.Count / 2)].FindControl("txt_a7");
            TextBox txtAvgDensity1 = (TextBox)grdDetail1.Rows[0].FindControl("txt_a7");
            txtAvgDensity1.Text = "0.00";
            if (avgcs > 0)
                txtAvgDensity1.Text = (avgcs / grdDetail1.Rows.Count).ToString("0.00");
            if (grdDetail1.Rows.Count < 3)
                txtAvgDensity1.Text = "***";
        }
        private void CalculateCLCWATERABSORPTION()
        {
            decimal cs = 0, avgcs = 0;

            for (int i = 0; i < grdDetail1.Rows.Count; i++)
            {
                //compressive strength
                TextBox txtIdMark = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                TextBox txtWetWeight = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                TextBox txtDryWeight = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                TextBox txtWaterAbsorption = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                TextBox txtAvgWaterAbsorption = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");

                cs = 0;
                if (txtWetWeight.Text != "" && txtDryWeight.Text != "")
                {
                    //   txtThickness.Text = (Convert.ToDecimal(txtLength.Text) * Convert.ToDecimal(txtWidth.Text)).ToString("0.0");

                    if (txtDryWeight.Text != "")
                    {
                        // if (Convert.ToDecimal(txtThickness.Text) > 0)

                        //   cs = (Convert.ToDecimal(txtWeight.Text) * Convert.ToDecimal(txtLength.Text)) * 1000 / (Convert.ToDecimal(txtBreadth.Text) * (Convert.ToDecimal(txtHeight.Text) * Convert.ToDecimal(txtHeight.Text)));

                        cs = (Convert.ToDecimal(txtWetWeight.Text) - Convert.ToDecimal(txtDryWeight.Text)) / Convert.ToDecimal(txtDryWeight.Text) * 100;
                        //  else
                        //cs = Convert.ToDecimal(txtWaterAbsorption.Text);

                        txtWaterAbsorption.Text = cs.ToString("0.00");
                        avgcs = avgcs + cs;
                    }
                }
                txtAvgWaterAbsorption.Text = "";
            }


            // TextBox txtAvgWaterAbsorption1 = (TextBox)grdDetail1.Rows[(grdDetail1.Rows.Count / 2)].FindControl("txt_a5");
            TextBox txtAvgWaterAbsorption1 = (TextBox)grdDetail1.Rows[0].FindControl("txt_a5");
            txtAvgWaterAbsorption1.Text = "0.00";
            if (avgcs > 0)
                txtAvgWaterAbsorption1.Text = (avgcs / grdDetail1.Rows.Count).ToString("0.00");
            if (grdDetail1.Rows.Count < 3)
                txtAvgWaterAbsorption1.Text = "***";

        }
        private void CalculateCLCCOMPRESSIVETEST()
        {
            decimal cs = 0, avgcs = 0;

            for (int i = 0; i < grdDetail1.Rows.Count; i++)
            {
                //compressive strength
                TextBox txtIdMark = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                TextBox txtLength = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                TextBox txtBreadth = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                TextBox txtHeight = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");

                TextBox txtCrossectionalArea = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                TextBox txtLoad = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                TextBox txtCompressiveStrength = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                TextBox txtAvgCompressiveStrength = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");

                cs = 0; decimal Crosss = 0;
                if (txtLength.Text != "" && txtBreadth.Text != "")
                {

                    if (txtHeight.Text != "")
                    {
                        // if (Convert.ToDecimal(txtThickness.Text) > 0)
                        //   cs = (Convert.ToDecimal(txtWeight.Text) * Convert.ToDecimal(txtLength.Text)) * 1000 / (Convert.ToDecimal(txtBreadth.Text) * (Convert.ToDecimal(txtHeight.Text) * Convert.ToDecimal(txtHeight.Text)));
                        Crosss = (Convert.ToDecimal(txtLength.Text) * Convert.ToDecimal(txtBreadth.Text));

                        txtCrossectionalArea.Text = Crosss.ToString("0.00");


                        if (txtCrossectionalArea.Text != "")
                        {
                            cs = Convert.ToDecimal(txtLoad.Text) / Convert.ToDecimal(txtCrossectionalArea.Text) * 1000;
                            //  else
                            //cs = Convert.ToDecimal(txtCompressiveStrength.Text);

                            txtCompressiveStrength.Text = cs.ToString("0.00");
                            avgcs = avgcs + cs;
                        }
                    }
                }
                txtAvgCompressiveStrength.Text = "";
            }


            TextBox txtAvgDensity1 = (TextBox)grdDetail1.Rows[0].FindControl("txt_a8");
            txtAvgDensity1.Text = "0.00";
            if (avgcs > 0)
                txtAvgDensity1.Text = (avgcs / grdDetail1.Rows.Count).ToString("0.00");

            if (grdDetail1.Rows.Count < 3)
                txtAvgDensity1.Text = "***";

        }

        private void CalculateMOE()
        {
            decimal avgMoe = 0, totAvgMoe = 0, diaCylinder = 0, area = 0, slopeCycle1 = 0, slopeCycle2 = 0, totStress = 0, totStrainCycle1 = 0, totStrainCycle2 = 0;
            int cylinderCnt = 1;
            TextBox txtDiaCylinder = (TextBox)grdDetail1.Rows[5].FindControl("txt_a7");
            if (txtDiaCylinder.Text != "" && decimal.TryParse(txtDiaCylinder.Text, out diaCylinder) == true)
            {
                area = Convert.ToDecimal("3.14") / 4 * diaCylinder * diaCylinder;
            }
            #region cylinder1
            for (int i = 0; i < grdDetail2.Rows.Count; i++)
            {
                TextBox txtLoadApplied = (TextBox)grdDetail2.Rows[i].FindControl("txt_b1");
                TextBox txtArea = (TextBox)grdDetail2.Rows[i].FindControl("txt_b2");
                TextBox txtStress = (TextBox)grdDetail2.Rows[i].FindControl("txt_b3");
                TextBox txtDialReadCycle1 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b4");
                TextBox txtStrainCycle1 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b5");
                TextBox txtDialReadCycle2 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b6");
                TextBox txtStrainCycle2 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b7");
                TextBox txtPercentDiffrence = (TextBox)grdDetail2.Rows[i].FindControl("txt_b8");

                if (txtLoadApplied.Text != "Modulus of Elasticity")
                {
                    txtArea.Text = area.ToString("0.0");
                    if (txtLoadApplied.Text != "" && txtArea.Text != "")
                    {
                        txtStress.Text = Convert.ToDecimal(Convert.ToDecimal(txtLoadApplied.Text) / Convert.ToDecimal(txtArea.Text) * 1000).ToString("0.00");
                        totStress += Convert.ToDecimal(txtStress.Text);
                    }
                    if (txtDialReadCycle1.Text != "")
                    {
                        txtStrainCycle1.Text = Convert.ToDecimal(Convert.ToDecimal(txtDialReadCycle1.Text) / 300).ToString("0.00000");
                        totStrainCycle1 += Convert.ToDecimal(txtStrainCycle1.Text);
                    }
                    if (txtDialReadCycle2.Text != "")
                    {
                        txtStrainCycle2.Text = Convert.ToDecimal(Convert.ToDecimal(txtDialReadCycle2.Text) / 300).ToString("0.00000");
                        totStrainCycle2 += Convert.ToDecimal(txtStrainCycle2.Text);
                    }
                    if (txtDialReadCycle1.Text != "" && txtDialReadCycle2.Text != "")
                    {
                        txtPercentDiffrence.Text = Convert.ToDecimal(((Convert.ToDecimal(txtDialReadCycle1.Text) - Convert.ToDecimal(txtDialReadCycle2.Text)) / Convert.ToDecimal(txtDialReadCycle1.Text)) * 100).ToString("0");
                    }
                }
            }
            slopeCycle1 = (totStrainCycle1 * totStress) / (totStrainCycle1 * totStrainCycle1);
            slopeCycle2 = (totStrainCycle2 * totStress) / (totStrainCycle2 * totStrainCycle2);
            TextBox txt_b1 = (TextBox)grdDetail2.Rows[grdDetail2.Rows.Count - 1].FindControl("txt_b1");
            if (txt_b1.Text != "Modulus of Elasticity")
            {
                AddRowDetail2();
                TextBox txtLoadApplied = (TextBox)grdDetail2.Rows[grdDetail2.Rows.Count - 1].FindControl("txt_b1");
                txtLoadApplied.Text = "Modulus of Elasticity";
            }
            TextBox txt_b4 = (TextBox)grdDetail2.Rows[grdDetail2.Rows.Count - 1].FindControl("txt_b4");
            TextBox txt_b6 = (TextBox)grdDetail2.Rows[grdDetail2.Rows.Count - 1].FindControl("txt_b6");
            txt_b4.Text = slopeCycle1.ToString("0");
            txt_b6.Text = slopeCycle2.ToString("0");

            TextBox txt_b2 = (TextBox)grdDetail2.Rows[grdDetail2.Rows.Count - 1].FindControl("txt_b2");
            TextBox txt_b3 = (TextBox)grdDetail2.Rows[grdDetail2.Rows.Count - 1].FindControl("txt_b3");
            TextBox txt_b5 = (TextBox)grdDetail2.Rows[grdDetail2.Rows.Count - 1].FindControl("txt_b5");
            TextBox txt_b7 = (TextBox)grdDetail2.Rows[grdDetail2.Rows.Count - 1].FindControl("txt_b7");
            TextBox txt_b8 = (TextBox)grdDetail2.Rows[grdDetail2.Rows.Count - 1].FindControl("txt_b8");
            TextBox txt_b9 = (TextBox)grdDetail2.Rows[grdDetail2.Rows.Count - 1].FindControl("txt_b9");
            txt_b2.Text = " ";
            txt_b3.Text = " ";
            txt_b5.Text = " ";
            txt_b7.Text = " ";
            txt_b8.Text = " ";
            txt_b9.Text = " ";

            avgMoe = (slopeCycle1 + slopeCycle2) / 2 / 1000;
            TextBox txtAvgMoeCylinder1 = (TextBox)grdDetail2.Rows[0].FindControl("txt_b9");
            txtAvgMoeCylinder1.Text = avgMoe.ToString("0");
            totAvgMoe += Convert.ToDecimal(txtAvgMoeCylinder1.Text);
            #endregion
            #region cylinder2
            if (grdDetail3.Visible == true)
            {
                slopeCycle1 = 0; slopeCycle2 = 0; totStress = 0; totStrainCycle1 = 0; totStrainCycle2 = 0;
                for (int i = 0; i < grdDetail3.Rows.Count; i++)
                {
                    TextBox txtLoadApplied = (TextBox)grdDetail3.Rows[i].FindControl("txt_c1");
                    TextBox txtArea = (TextBox)grdDetail3.Rows[i].FindControl("txt_c2");
                    TextBox txtStress = (TextBox)grdDetail3.Rows[i].FindControl("txt_c3");
                    TextBox txtDialReadCycle1 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c4");
                    TextBox txtStrainCycle1 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c5");
                    TextBox txtDialReadCycle2 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c6");
                    TextBox txtStrainCycle2 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c7");
                    TextBox txtPercentDiffrence = (TextBox)grdDetail3.Rows[i].FindControl("txt_c8");

                    if (txtLoadApplied.Text != "Modulus of Elasticity")
                    {
                        txtArea.Text = area.ToString("0.0");
                        if (txtLoadApplied.Text != "" && txtArea.Text != "")
                        {
                            txtStress.Text = Convert.ToDecimal(Convert.ToDecimal(txtLoadApplied.Text) / Convert.ToDecimal(txtArea.Text) * 1000).ToString("0.00");
                            totStress += Convert.ToDecimal(txtStress.Text);
                        }
                        if (txtDialReadCycle1.Text != "")
                        {
                            txtStrainCycle1.Text = Convert.ToDecimal(Convert.ToDecimal(txtDialReadCycle1.Text) / 300).ToString("0.00000");
                            totStrainCycle1 += Convert.ToDecimal(txtStrainCycle1.Text);
                        }
                        if (txtDialReadCycle2.Text != "")
                        {
                            txtStrainCycle2.Text = Convert.ToDecimal(Convert.ToDecimal(txtDialReadCycle2.Text) / 300).ToString("0.00000");
                            totStrainCycle2 += Convert.ToDecimal(txtStrainCycle2.Text);
                        }
                        if (txtDialReadCycle1.Text != "" && txtDialReadCycle2.Text != "")
                        {
                            txtPercentDiffrence.Text = Convert.ToDecimal(((Convert.ToDecimal(txtDialReadCycle1.Text) - Convert.ToDecimal(txtDialReadCycle2.Text)) / Convert.ToDecimal(txtDialReadCycle1.Text)) * 100).ToString("0");
                        }
                    }
                }
                slopeCycle1 = (totStrainCycle1 * totStress) / (totStrainCycle1 * totStrainCycle1);
                slopeCycle2 = (totStrainCycle2 * totStress) / (totStrainCycle2 * totStrainCycle2);
                TextBox txt_c1 = (TextBox)grdDetail3.Rows[grdDetail3.Rows.Count - 1].FindControl("txt_c1");
                if (txt_c1.Text != "Modulus of Elasticity")
                {
                    AddRowDetail3();
                    TextBox txtLoadApplied = (TextBox)grdDetail3.Rows[grdDetail3.Rows.Count - 1].FindControl("txt_c1");
                    txtLoadApplied.Text = "Modulus of Elasticity";
                }
                TextBox txt_c4 = (TextBox)grdDetail3.Rows[grdDetail3.Rows.Count - 1].FindControl("txt_c4");
                TextBox txt_c6 = (TextBox)grdDetail3.Rows[grdDetail3.Rows.Count - 1].FindControl("txt_c6");
                txt_c4.Text = slopeCycle1.ToString("0");
                txt_c6.Text = slopeCycle2.ToString("0");

                TextBox txt_c2 = (TextBox)grdDetail3.Rows[grdDetail3.Rows.Count - 1].FindControl("txt_c2");
                TextBox txt_c3 = (TextBox)grdDetail3.Rows[grdDetail3.Rows.Count - 1].FindControl("txt_c3");
                TextBox txt_c5 = (TextBox)grdDetail3.Rows[grdDetail3.Rows.Count - 1].FindControl("txt_c5");
                TextBox txt_c7 = (TextBox)grdDetail3.Rows[grdDetail3.Rows.Count - 1].FindControl("txt_c7");
                TextBox txt_c8 = (TextBox)grdDetail3.Rows[grdDetail3.Rows.Count - 1].FindControl("txt_c8");
                TextBox txt_c9 = (TextBox)grdDetail3.Rows[grdDetail3.Rows.Count - 1].FindControl("txt_c9");
                txt_c2.Text = " ";
                txt_c3.Text = " ";
                txt_c5.Text = " ";
                txt_c7.Text = " ";
                txt_c8.Text = " ";
                txt_c9.Text = " ";

                avgMoe = (slopeCycle1 + slopeCycle2) / 2 / 1000;
                TextBox txtAvgMoeCylinder2 = (TextBox)grdDetail3.Rows[0].FindControl("txt_c9");
                txtAvgMoeCylinder2.Text = avgMoe.ToString("0");
                totAvgMoe += Convert.ToDecimal(txtAvgMoeCylinder2.Text);
                cylinderCnt++;
            }
            #endregion

            #region cylinder3
            if (grdDetail4.Visible == true)
            {
                slopeCycle1 = 0; slopeCycle2 = 0; totStress = 0; totStrainCycle1 = 0; totStrainCycle2 = 0;
                for (int i = 0; i < grdDetail4.Rows.Count; i++)
                {
                    TextBox txtLoadApplied = (TextBox)grdDetail4.Rows[i].FindControl("txt_d1");
                    TextBox txtArea = (TextBox)grdDetail4.Rows[i].FindControl("txt_d2");
                    TextBox txtStress = (TextBox)grdDetail4.Rows[i].FindControl("txt_d3");
                    TextBox txtDialReadCycle1 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d4");
                    TextBox txtStrainCycle1 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d5");
                    TextBox txtDialReadCycle2 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d6");
                    TextBox txtStrainCycle2 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d7");
                    TextBox txtPercentDiffrence = (TextBox)grdDetail4.Rows[i].FindControl("txt_d8");

                    if (txtLoadApplied.Text != "Modulus of Elasticity")
                    {
                        txtArea.Text = area.ToString("0.0");
                        if (txtLoadApplied.Text != "" && txtArea.Text != "")
                        {
                            txtStress.Text = Convert.ToDecimal(Convert.ToDecimal(txtLoadApplied.Text) / Convert.ToDecimal(txtArea.Text) * 1000).ToString("0.00");
                            totStress += Convert.ToDecimal(txtStress.Text);
                        }
                        if (txtDialReadCycle1.Text != "")
                        {
                            txtStrainCycle1.Text = Convert.ToDecimal(Convert.ToDecimal(txtDialReadCycle1.Text) / 300).ToString("0.00000");
                            totStrainCycle1 += Convert.ToDecimal(txtStrainCycle1.Text);
                        }
                        if (txtDialReadCycle2.Text != "")
                        {
                            txtStrainCycle2.Text = Convert.ToDecimal(Convert.ToDecimal(txtDialReadCycle2.Text) / 300).ToString("0.00000");
                            totStrainCycle2 += Convert.ToDecimal(txtStrainCycle2.Text);
                        }
                        if (txtDialReadCycle1.Text != "" && txtDialReadCycle2.Text != "")
                        {
                            txtPercentDiffrence.Text = Convert.ToDecimal(((Convert.ToDecimal(txtDialReadCycle1.Text) - Convert.ToDecimal(txtDialReadCycle2.Text)) / Convert.ToDecimal(txtDialReadCycle1.Text)) * 100).ToString("0");
                        }
                    }
                }
                slopeCycle1 = (totStrainCycle1 * totStress) / (totStrainCycle1 * totStrainCycle1);
                slopeCycle2 = (totStrainCycle2 * totStress) / (totStrainCycle2 * totStrainCycle2);
                TextBox txt_d1 = (TextBox)grdDetail4.Rows[grdDetail4.Rows.Count - 1].FindControl("txt_d1");
                if (txt_d1.Text != "Modulus of Elasticity")
                {
                    AddRowDetail4();
                    TextBox txtLoadApplied = (TextBox)grdDetail4.Rows[grdDetail4.Rows.Count - 1].FindControl("txt_d1");
                    txtLoadApplied.Text = "Modulus of Elasticity";
                }
                TextBox txt_d4 = (TextBox)grdDetail4.Rows[grdDetail4.Rows.Count - 1].FindControl("txt_d4");
                TextBox txt_d6 = (TextBox)grdDetail4.Rows[grdDetail4.Rows.Count - 1].FindControl("txt_d6");
                txt_d4.Text = slopeCycle1.ToString("0");
                txt_d6.Text = slopeCycle2.ToString("0");

                TextBox txt_d2 = (TextBox)grdDetail4.Rows[grdDetail4.Rows.Count - 1].FindControl("txt_d2");
                TextBox txt_d3 = (TextBox)grdDetail4.Rows[grdDetail4.Rows.Count - 1].FindControl("txt_d3");
                TextBox txt_d5 = (TextBox)grdDetail4.Rows[grdDetail4.Rows.Count - 1].FindControl("txt_d5");
                TextBox txt_d7 = (TextBox)grdDetail4.Rows[grdDetail4.Rows.Count - 1].FindControl("txt_d7");
                TextBox txt_d8 = (TextBox)grdDetail4.Rows[grdDetail4.Rows.Count - 1].FindControl("txt_d8");
                TextBox txt_d9 = (TextBox)grdDetail4.Rows[grdDetail4.Rows.Count - 1].FindControl("txt_d9");
                txt_d2.Text = " ";
                txt_d3.Text = " ";
                txt_d5.Text = " ";
                txt_d7.Text = " ";
                txt_d8.Text = " ";
                txt_d9.Text = " ";

                avgMoe = (slopeCycle1 + slopeCycle2) / 2 / 1000;
                TextBox txtAvgMoeCylinder3 = (TextBox)grdDetail4.Rows[0].FindControl("txt_d9");
                txtAvgMoeCylinder3.Text = avgMoe.ToString("0");
                totAvgMoe += Convert.ToDecimal(txtAvgMoeCylinder3.Text);
                cylinderCnt++;
            }
            #endregion
            totAvgMoe = Math.Round(totAvgMoe / cylinderCnt);
            lblHeading5.Visible = true;
            lblHeading5.Text = "The Average Modulus of Elasticity of the sample " + totAvgMoe.ToString() + " Giga Pascal";
        }

        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            Boolean valid = true, validCheckBox = false;
            decimal val;
            bool dataAvailable = false;

            if (ddlSection.SelectedIndex <= 0)
            {
                lblMsg.Text = "Select Section.";
                ddlSection.Focus();
                valid = false;
            }
            if (ddl_ReportFor.SelectedValue == "21")
            {
                validCheckBox = true;

                for (int i = 0; i < grdDetail8.Rows.Count; i++)
                {
                    TextBox txt_a1 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a1");
                    TextBox txt_a2 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a2");
                    TextBox txt_a3 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a3");
                    TextBox txt_a4 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a4");
                    TextBox txt_a5 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a5");
                    TextBox txt_a6 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a6");
                    TextBox txt_a7 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a7");
                    TextBox txt_a8 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a8");
                    TextBox txt_a9 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a9");

                    //if (txt_a1.Text == "" && txt_a1.Visible == true &&

                    //         grdDetail8.Rows[i].Cells[5].Visible == true &&
                    //         grdDetail8.Rows[i].Cells[6].Visible == true &&
                    //         grdDetail8.Rows[i].Cells[7].Visible == true &&
                    //         grdDetail8.Rows[i].Cells[8].Visible == true )
                    //{
                    //    lblMsg.Text = "Enter Description for Sr No. " + (i + 1) + ".";
                    //    txt_a1.Focus();
                    //    valid = false;
                    //    break;
                    //}
                    if (txt_a1.Text == "" && txt_a1.Visible == true)
                    {
                        lblMsg.Text = "Invalid input for Sr No. " + (i + 1) + ".";
                        txt_a1.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_a2.Text == "" && txt_a2.Visible == true)
                    {
                        lblMsg.Text = "Invalid input for Sr No. " + (i + 1) + ".";
                        txt_a2.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_a3.Text == "" && txt_a3.Visible == true)
                    {
                        lblMsg.Text = "Invalid input for Sr No. " + (i + 1) + ".";
                        txt_a3.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_a4.Text == "" && txt_a4.Visible == true)
                    {
                        lblMsg.Text = "Invalid input for Sr No. " + (i + 1) + ".";
                        txt_a4.Focus();
                        valid = false;
                        break;
                    }

                    if (txt_a1.Text.Trim() != "" || txt_a2.Text.Trim() != "" || txt_a3.Text.Trim() != "" ||
                        txt_a4.Text.Trim() != "" || txt_a5.Text.Trim() != "")
                    {
                        dataAvailable = true;
                    }
                }
            }
            if (ddl_ReportFor.SelectedValue == "22")
            {
                validCheckBox = true;

                for (int i = 0; i < grdDetail8.Rows.Count; i++)
                {
                    TextBox txt_a1 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a1");
                    TextBox txt_a2 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a2");
                    TextBox txt_a3 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a3");
                    TextBox txt_a4 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a4");
                    TextBox txt_a5 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a5");
                    TextBox txt_a6 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a6");
                    TextBox txt_a7 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a7");
                    TextBox txt_a8 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a8");
                    TextBox txt_a9 = (TextBox)grdDetail8.Rows[i].FindControl("txt_a9");

                    if (txt_a1.Text == "" && txt_a1.Visible == true &&

                             grdDetail8.Rows[i].Cells[5].Visible == true &&
                             grdDetail8.Rows[i].Cells[6].Visible == true &&
                             grdDetail8.Rows[i].Cells[7].Visible == true &&
                             grdDetail8.Rows[i].Cells[8].Visible == true &&
                             grdDetail8.Rows[i].Cells[9].Visible == true &&
                             grdDetail8.Rows[i].Cells[10].Visible == true &&
                             grdDetail8.Rows[i].Cells[11].Visible == true)
                    {
                        lblMsg.Text = "Enter Description for Sr No. " + (i + 1) + ".";
                        txt_a1.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_a1.Text == "" && txt_a1.Visible == true)
                    {
                        lblMsg.Text = "Invalid input for Sr No. " + (i + 1) + ".";
                        txt_a1.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_a2.Text == "" && txt_a2.Visible == true)
                    {
                        lblMsg.Text = "Invalid input for Sr No. " + (i + 1) + ".";
                        txt_a2.Focus();
                        valid = false;
                        break;
                    }
                    else if (txt_a3.Text == "" && txt_a3.Visible == true)
                    {
                        lblMsg.Text = "Invalid input for Sr No. " + (i + 1) + ".";
                        txt_a3.Focus();
                        valid = false;
                        break;
                    }


                    if (txt_a1.Text.Trim() != "" || txt_a2.Text.Trim() != "" || txt_a3.Text.Trim() != "" ||
                        txt_a4.Text.Trim() != "" || txt_a5.Text.Trim() != "" || txt_a6.Text.Trim() != "" ||
                        txt_a7.Text.Trim() != "" || txt_a8.Text.Trim() != "" || txt_a9.Text.Trim() != "")
                    {
                        dataAvailable = true;
                    }
                }
            }

            else if (chkBox8.Visible == true && chkBox8.Checked == false && validCheckBox != true)
                validCheckBox = false;

            if (chkBox1.Visible == true && chkBox1.Checked && ddl_ReportFor.SelectedValue != "22")
            {
                if (ddl_ReportFor.SelectedValue == "26")
                {
                    CalculateFlexuralStrength();

                    validCheckBox = true;
                    for (int i = 0; i < grdDetail1.Rows.Count; i++)
                    {
                        TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                        TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                        TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                        TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                        TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                        TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                        TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                        TextBox txt_a8 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");
                        TextBox txt_a9 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a9");

                        if (txt_a1.Text == "" && grdDetail1.Columns[3].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[3].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a1.Focus();
                            valid = false;
                            break;
                        }
                        else if (!decimal.TryParse(txt_a1.Text, out val) && ddl_ReportFor.SelectedValue == "13")
                        {
                            lblMsg.Text = "Invalid input '" + grdDetail1.HeaderRow.Cells[3].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ". Enter Only numeric/decimal values";
                            txt_a1.Focus();
                            valid = false;
                            break;

                        }
                        else if (txt_a2.Text == "" && grdDetail1.Columns[4].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[4].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a2.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a3.Text == "" && grdDetail1.Columns[5].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[5].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a3.Focus();
                            valid = false;
                            break;
                        }
                        else if (!decimal.TryParse(txt_a3.Text, out val) && ddl_ReportFor.SelectedValue == "13")
                        {
                            lblMsg.Text = "Invalid input '" + grdDetail1.HeaderRow.Cells[5].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ". Enter Only numeric/decimal values";
                            txt_a3.Focus();
                            valid = false;
                            break;

                        }
                        else if (txt_a4.Text == "" && grdDetail1.Columns[6].Visible == true && txt_a4.Enabled == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[6].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a4.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a5.Text == "" && grdDetail1.Columns[7].Visible == true && txt_a5.Enabled == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[7].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a5.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a6.Text == "" && grdDetail1.Columns[8].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[8].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a6.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a7.Text == "" && grdDetail1.Columns[9].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[9].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a7.Focus();
                            valid = false;
                            break;
                        }


                        if (txt_a1.Text.Trim() != "" || txt_a2.Text.Trim() != "" || txt_a3.Text.Trim() != "" ||
                            txt_a4.Text.Trim() != "" || txt_a5.Text.Trim() != "" || txt_a6.Text.Trim() != "" ||
                            txt_a7.Text.Trim() != "" || txt_a8.Text.Trim() != "" || txt_a9.Text.Trim() != "")
                        {
                            dataAvailable = true;
                        }

                    }
                }
                else if (ddl_ReportFor.SelectedValue == "27" || ddl_ReportFor.SelectedValue == "29")
                {
                    validCheckBox = true;
                    for (int i = 0; i < grdDetail1.Rows.Count; i++)
                    {
                        TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                        TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                        TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                        TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                        TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                        TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                        TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                        TextBox txt_a8 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");
                        TextBox txt_a9 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a9");


                        if (ddl_ReportFor.SelectedValue == "29")
                        {
                            if (txt_a1.Text == "" && grdDetail1.Columns[3].Visible == true)
                            {
                                lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[3].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                                txt_a1.Focus();
                                valid = false;
                                break;
                            }
                            // if (txt_a2.Text == "" && grdDetail1.Columns[4].Visible == true)
                            //{
                            //    lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[4].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            //    txt_a2.Focus();
                            //    valid = false;
                            //    break;
                            //}

                        }
                        else
                        {
                            if (txt_a1.Text == "" && grdDetail1.Columns[3].Visible == true)
                            {
                                lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[3].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                                txt_a1.Focus();
                                valid = false;
                                break;
                            }

                            else if (txt_a2.Text == "" && grdDetail1.Columns[4].Visible == true)
                            {
                                lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[4].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                                txt_a2.Focus();
                                valid = false;
                                break;
                            }
                            else if (txt_a3.Text == "" && grdDetail1.Columns[5].Visible == true)
                            {
                                lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[5].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                                txt_a3.Focus();
                                valid = false;
                                break;
                            }
                            else if (txt_a4.Text == "" && grdDetail1.Columns[6].Visible == true && txt_a4.Enabled == true)
                            {
                                lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[6].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                                txt_a4.Focus();
                                valid = false;
                                break;
                            }
                            else if (txt_a5.Text == "" && grdDetail1.Columns[7].Visible == true && txt_a5.Enabled == true)
                            {
                                lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[7].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                                txt_a5.Focus();
                                valid = false;
                                break;
                            }
                            // else if (txt_a6.Text == "" && grdDetail1.Columns[8].Visible == true)
                            //{
                            //    lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[8].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            //    txt_a6.Focus();
                            //    valid = false;
                            //    break;
                            //}

                            else if (txt_a7.Text == "" && grdDetail1.Columns[9].Visible == true)
                            {
                                lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[9].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                                txt_a7.Focus();
                                valid = false;
                                break;
                            }
                        }

                        if (txt_a1.Text.Trim() != "" || txt_a2.Text.Trim() != "" || txt_a3.Text.Trim() != "" ||
                            txt_a4.Text.Trim() != "" || txt_a5.Text.Trim() != "" || txt_a6.Text.Trim() != "" ||
                            txt_a7.Text.Trim() != "" || txt_a8.Text.Trim() != "" || txt_a9.Text.Trim() != "")
                        {
                            dataAvailable = true;
                        }

                    }
                }

                else if (ddl_ReportFor.SelectedValue == "30" || ddl_ReportFor.SelectedValue == "31" || ddl_ReportFor.SelectedValue == "32")
                {

                    validCheckBox = true;
                    for (int i = 0; i < grdDetail1.Rows.Count; i++)
                    {
                        TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                        TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                        TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                        TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                        TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                        TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                        TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                        TextBox txt_a8 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");
                        TextBox txt_a9 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a9");

                        if (txt_a1.Text == "" && grdDetail1.Columns[3].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[3].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a1.Focus();
                            valid = false;
                            break;
                        }

                        else if (txt_a2.Text == "" && grdDetail1.Columns[4].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[4].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a2.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a3.Text == "" && grdDetail1.Columns[5].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[5].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a3.Focus();
                            valid = false;
                            break;
                        }

                        if (ddl_ReportFor.SelectedValue == "31")
                        {
                            if (txt_a4.Text == "" && grdDetail1.Columns[6].Visible == true && txt_a4.Enabled == true)
                            {
                                lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[6].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                                txt_a4.Focus();
                                valid = false;
                                break;
                            }
                        }

                        else if (ddl_ReportFor.SelectedValue == "30")
                        {
                            if (txt_a4.Text == "" && grdDetail1.Columns[6].Visible == true && txt_a4.Enabled == true)
                            {
                                lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[6].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                                txt_a4.Focus();
                                valid = false;
                                break;
                            }
                            else if (txt_a5.Text == "" && grdDetail1.Columns[7].Visible == true && txt_a5.Enabled == true)
                            {
                                lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[7].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                                txt_a5.Focus();
                                valid = false;
                                break;
                            }
                            else if (txt_a6.Text == "" && grdDetail1.Columns[8].Visible == true)
                            {
                                lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[8].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                                txt_a6.Focus();
                                valid = false;
                                break;
                            }
                        }

                        else if (ddl_ReportFor.SelectedValue == "32")
                        {
                            if (txt_a4.Text == "" && grdDetail1.Columns[6].Visible == true && txt_a4.Enabled == true)
                            {
                                lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[6].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                                txt_a4.Focus();
                                valid = false;
                                break;
                            }
                            //else if (txt_a5.Text == "" && grdDetail1.Columns[7].Visible == true && txt_a5.Enabled == true)
                            //{
                            //    lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[7].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            //    txt_a5.Focus();
                            //    valid = false;
                            //    break;
                            //}
                            else if (txt_a6.Text == "" && grdDetail1.Columns[8].Visible == true)
                            {
                                lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[8].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                                txt_a6.Focus();
                                valid = false;
                                break;
                            }
                            //else if (txt_a7.Text == "" && grdDetail1.Columns[9].Visible == true)
                            //{
                            //    lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[9].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            //    txt_a7.Focus();
                            //    valid = false;
                            //    break;
                            //}
                            //else if (txt_a8.Text == "" && grdDetail1.Columns[10].Visible == true)
                            //{
                            //    lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[10].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            //    txt_a8.Focus();
                            //    valid = false;
                            //    break;
                            //}
                        }


                        if (txt_a1.Text.Trim() != "" || txt_a2.Text.Trim() != "" || txt_a3.Text.Trim() != "" ||
                            txt_a4.Text.Trim() != "" || txt_a5.Text.Trim() != "" || txt_a6.Text.Trim() != "" ||
                            txt_a7.Text.Trim() != "" || txt_a8.Text.Trim() != "" || txt_a9.Text.Trim() != "")
                        {
                            dataAvailable = true;
                        }

                    }
                }
                else if (ddl_ReportFor.SelectedValue == "34")
                {

                    validCheckBox = true;
                    for (int i = 0; i < grdDetail1.Rows.Count; i++)
                    {
                        TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                        TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                        TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                        TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                        TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                        TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                        TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");

                        if (txt_a1.Text == "" && grdDetail1.Columns[3].Visible == true)
                        {
                            lblMsg.Text = "Input '" + "Description" + "' for Sr No. " + (i + 1) + ".";
                            txt_a1.Focus();
                            valid = false;
                            break;
                        }

                        else if (txt_a2.Text == "" && grdDetail1.Columns[4].Visible == true)
                        {
                            lblMsg.Text = "Input '" + "C+5" + "' for " + txt_a1.Text + ".";
                            txt_a2.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a3.Text == "" && grdDetail1.Columns[5].Visible == true)
                        {
                            lblMsg.Text = "Input '" + "C+1.5" + "' for " + txt_a1.Text + ".";
                            txt_a3.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a4.Text == "" && grdDetail1.Columns[6].Visible == true)
                        {
                            lblMsg.Text = "Input '" + "Unit" + "' for " + txt_a1.Text + ".";
                            txt_a4.Focus();
                            valid = false;
                            break;
                        }                        
                        else if (txt_a6.Text == "" && grdDetail1.Columns[8].Visible == true)
                        {
                            lblMsg.Text = "Input '" + "Description" + "' for Sr No. " + (i + 1) + ".";
                            txt_a6.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a7.Text == "" && grdDetail1.Columns[9].Visible == true)
                        {
                            lblMsg.Text = "Input '" + txt_a6.Text + "' for Sr No. " + (i + 1) + ".";
                            txt_a7.Focus();
                            valid = false;
                            break;
                        }

                        if (txt_a1.Text.Trim() != "" || txt_a2.Text.Trim() != "" || txt_a3.Text.Trim() != "" ||
                            txt_a4.Text.Trim() != "" || txt_a5.Text.Trim() != "" || txt_a6.Text.Trim() != "" ||
                            txt_a7.Text.Trim() != "")
                        {
                            dataAvailable = true;
                        }

                    }
                }
                else
                {
                    validCheckBox = true;
                    for (int i = 0; i < grdDetail1.Rows.Count; i++)
                    {
                        TextBox txt_a1 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a1");
                        TextBox txt_a2 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a2");
                        TextBox txt_a3 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a3");
                        TextBox txt_a4 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a4");
                        TextBox txt_a5 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a5");
                        TextBox txt_a6 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a6");
                        TextBox txt_a7 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a7");
                        TextBox txt_a8 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a8");
                        TextBox txt_a9 = (TextBox)grdDetail1.Rows[i].FindControl("txt_a9");

                        if (txt_a1.Text == "" && grdDetail1.Columns[3].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[3].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a1.Focus();
                            valid = false;
                            break;
                        }
                        else if (!decimal.TryParse(txt_a1.Text, out val) && ddl_ReportFor.SelectedValue == "13")
                        {
                            lblMsg.Text = "Invalid input '" + grdDetail1.HeaderRow.Cells[3].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ". Enter Only numeric/decimal values";
                            txt_a1.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a2.Text == "" && grdDetail1.Columns[4].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[4].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a2.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a3.Text == "" && grdDetail1.Columns[5].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[5].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a3.Focus();
                            valid = false;
                            break;
                        }
                        else if (!decimal.TryParse(txt_a3.Text, out val) && (ddl_ReportFor.SelectedValue == "13" || ddl_ReportFor.SelectedValue == "35" || ddl_ReportFor.SelectedValue == "36"))
                        {
                            lblMsg.Text = "Invalid input '" + grdDetail1.HeaderRow.Cells[5].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ". Enter Only numeric/decimal values";
                            txt_a3.Focus();
                            valid = false;
                            break;

                        }
                        else if (txt_a4.Text == "" && grdDetail1.Columns[6].Visible == true && txt_a4.Enabled == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[6].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a4.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a5.Text == "" && grdDetail1.Columns[7].Visible == true && txt_a5.Enabled == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[7].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a5.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a6.Text == "" && grdDetail1.Columns[8].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[8].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a6.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a7.Text == "" && grdDetail1.Columns[9].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[9].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a7.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a8.Text == "" && grdDetail1.Columns[10].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[10].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a8.Focus();
                            valid = false;
                            break;
                        }
                        else if (txt_a9.Text == "" && grdDetail1.Columns[11].Visible == true)
                        {
                            lblMsg.Text = "Input '" + grdDetail1.HeaderRow.Cells[11].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                            txt_a9.Focus();
                            valid = false;
                            break;
                        }

                        if (txt_a1.Text.Trim() != "" || txt_a2.Text.Trim() != "" || txt_a3.Text.Trim() != "" ||
                            txt_a4.Text.Trim() != "" || txt_a5.Text.Trim() != "" || txt_a6.Text.Trim() != "" ||
                            txt_a7.Text.Trim() != "" || txt_a8.Text.Trim() != "" || txt_a9.Text.Trim() != "")
                        {
                            dataAvailable = true;
                        }

                    }
                }

            }

            else if (chkBox1.Visible == true && chkBox1.Checked == false && validCheckBox != true)
                validCheckBox = false;


            if (grdDetail2.Visible == true && valid == true && chkBox2.Checked == true)
            {
                validCheckBox = true;
                for (int i = 0; i < grdDetail2.Rows.Count; i++)
                {
                    TextBox txt_b1 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b1");
                    TextBox txt_b2 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b2");
                    TextBox txt_b3 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b3");
                    TextBox txt_b4 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b4");
                    TextBox txt_b5 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b5");
                    TextBox txt_b6 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b6");
                    TextBox txt_b7 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b7");
                    TextBox txt_b8 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b8");
                    TextBox txt_b9 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b9");
                    TextBox txt_b10 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b10");
                    TextBox txt_b11 = (TextBox)grdDetail2.Rows[i].FindControl("txt_b11");

                    if (txt_b1.Text == "" && grdDetail2.Columns[3].Visible == true && txt_b1.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail2.HeaderRow.Cells[3].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_b1.Focus();
                        break;
                    }
                    else if (txt_b2.Text == "" && grdDetail2.Columns[4].Visible == true && txt_b2.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail2.HeaderRow.Cells[4].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_b2.Focus();
                        break;
                    }
                    else if (txt_b3.Text == "" && grdDetail2.Columns[5].Visible == true && txt_b3.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail2.HeaderRow.Cells[5].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_b3.Focus();
                        break;
                    }
                    else if (txt_b4.Text == "" && grdDetail2.Columns[6].Visible == true && txt_b4.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail2.HeaderRow.Cells[6].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_b4.Focus();
                        break;
                    }
                    else if (txt_b5.Text == "" && grdDetail2.Columns[7].Visible == true && txt_b5.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail2.HeaderRow.Cells[7].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_b5.Focus();
                        break;
                    }
                    else if (txt_b6.Text == "" && grdDetail2.Columns[8].Visible == true && txt_b6.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail2.HeaderRow.Cells[8].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_b6.Focus();
                        break;
                    }
                    else if (txt_b7.Text == "" && grdDetail2.Columns[9].Visible == true && txt_b7.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail2.HeaderRow.Cells[9].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_b7.Focus();
                        break;
                    }
                    else if (txt_b8.Text == "" && grdDetail2.Columns[10].Visible == true && txt_b8.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail2.HeaderRow.Cells[10].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_b8.Focus();
                        break;
                    }
                    else if (txt_b9.Text == "" && grdDetail2.Columns[11].Visible == true && txt_b9.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail2.HeaderRow.Cells[11].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_b9.Focus();
                        break;
                    }
                    else if (txt_b10.Text == "" && grdDetail2.Columns[12].Visible == true && txt_b10.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail2.HeaderRow.Cells[12].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_b10.Focus();
                        break;
                    }
                    else if (txt_b11.Text == "" && grdDetail2.Columns[13].Visible == true && txt_b11.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail2.HeaderRow.Cells[13].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_b11.Focus();
                        break;
                    }
                    if (txt_b1.Text.Trim() != "" || txt_b2.Text.Trim() != "" || txt_b3.Text.Trim() != "" ||
                        txt_b4.Text.Trim() != "" || txt_b5.Text.Trim() != "" || txt_b6.Text.Trim() != "" ||
                        txt_b7.Text.Trim() != "" || txt_b8.Text.Trim() != "" || txt_b9.Text.Trim() != "" ||
                        txt_b10.Text.Trim() != "" || txt_b11.Text.Trim() != "")
                    {
                        dataAvailable = true;
                    }
                }
            }
            else if (chkBox2.Visible == true && chkBox2.Checked == false && validCheckBox != true)
                validCheckBox = false;
            if (txtDiameterOfSample.Visible == true && txtDiameterOfSample.Text.Trim() == "")
            {
                lblMsg.Text = "Input " + lblDiameterOfSample.Text;
                valid = false;
                txtDiameterOfSample.Focus();
            }
            else if (txtTemperatureDuringTest.Visible == true && txtTemperatureDuringTest.Text.Trim() == "")
            {
                lblMsg.Text = "Input " + lblTemperatureDuringTest.Text;
                valid = false;
                txtTemperatureDuringTest.Focus();
            }
            if (grdDetail3.Visible == true && valid == true && chkBox3.Checked == true)
            {
                validCheckBox = true;
                for (int i = 0; i < grdDetail3.Rows.Count; i++)
                {
                    TextBox txt_c1 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c1");
                    TextBox txt_c2 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c2");
                    TextBox txt_c3 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c3");
                    TextBox txt_c4 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c4");
                    TextBox txt_c5 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c5");
                    TextBox txt_c6 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c6");
                    TextBox txt_c7 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c7");
                    TextBox txt_c8 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c8");
                    TextBox txt_c9 = (TextBox)grdDetail3.Rows[i].FindControl("txt_c9");

                    if (txt_c1.Text == "" && grdDetail3.Columns[3].Visible == true && txt_c1.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail3.HeaderRow.Cells[3].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_c1.Focus();
                        break;
                    }
                    else if (txt_c2.Text == "" && grdDetail3.Columns[4].Visible == true && txt_c2.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail3.HeaderRow.Cells[4].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_c2.Focus();
                        break;
                    }
                    else if (txt_c3.Text == "" && grdDetail3.Columns[5].Visible == true && txt_c3.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail3.HeaderRow.Cells[5].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_c3.Focus();
                        break;
                    }
                    else if (txt_c4.Text == "" && grdDetail3.Columns[6].Visible == true && txt_c4.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail3.HeaderRow.Cells[6].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_c4.Focus();
                        break;
                    }
                    else if (txt_c5.Text == "" && grdDetail3.Columns[7].Visible == true && txt_c5.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail3.HeaderRow.Cells[7].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_c5.Focus();
                        break;
                    }
                    else if (txt_c6.Text == "" && grdDetail3.Columns[8].Visible == true && txt_c6.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail3.HeaderRow.Cells[8].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_c6.Focus();
                        break;
                    }
                    else if (txt_c7.Text == "" && grdDetail3.Columns[9].Visible == true && txt_c7.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail3.HeaderRow.Cells[9].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_c7.Focus();
                        break;
                    }
                    else if (txt_c8.Text == "" && grdDetail3.Columns[10].Visible == true && txt_c8.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail3.HeaderRow.Cells[10].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_c8.Focus();
                        break;
                    }
                    else if (txt_c9.Text == "" && grdDetail3.Columns[11].Visible == true && txt_c9.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail3.HeaderRow.Cells[11].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_c9.Focus();
                        break;
                    }
                    if (txt_c1.Text.Trim() != "" || txt_c2.Text.Trim() != "" || txt_c3.Text.Trim() != "" ||
                        txt_c4.Text.Trim() != "" || txt_c5.Text.Trim() != "" || txt_c6.Text.Trim() != "" || txt_c7.Text.Trim() != "" || txt_c8.Text.Trim() != "" || txt_c9.Text.Trim() != "")
                    {
                        dataAvailable = true;
                    }
                }
            }
            else if (chkBox3.Visible == true && chkBox3.Checked == false && validCheckBox != true)
                validCheckBox = false;

            if (grdDetail4.Visible == true && valid == true && chkBox4.Checked == true)
            {
                validCheckBox = true;
                for (int i = 0; i < grdDetail4.Rows.Count; i++)
                {
                    TextBox txt_d1 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d1");
                    TextBox txt_d2 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d2");
                    TextBox txt_d3 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d3");
                    TextBox txt_d4 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d4");
                    TextBox txt_d5 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d5");

                    TextBox txt_d6 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d6");
                    TextBox txt_d7 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d7");
                    TextBox txt_d8 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d8");
                    TextBox txt_d9 = (TextBox)grdDetail4.Rows[i].FindControl("txt_d9");

                    if (txt_d1.Text == "" && grdDetail4.Columns[3].Visible == true && txt_d1.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail4.HeaderRow.Cells[3].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_d1.Focus();
                        break;
                    }
                    else if (txt_d2.Text == "" && grdDetail4.Columns[4].Visible == true && txt_d2.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail4.HeaderRow.Cells[4].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_d2.Focus();
                        break;
                    }
                    else if (txt_d3.Text == "" && grdDetail4.Columns[5].Visible == true && txt_d3.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail4.HeaderRow.Cells[5].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_d3.Focus();
                        break;
                    }
                    else if (txt_d4.Text == "" && grdDetail4.Columns[6].Visible == true && txt_d4.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail4.HeaderRow.Cells[6].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_d4.Focus();
                        break;
                    }
                    else if (txt_d5.Text == "" && grdDetail4.Columns[7].Visible == true && txt_d5.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail4.HeaderRow.Cells[7].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_d5.Focus();
                        break;
                    }

                    else if (txt_d6.Text == "" && grdDetail4.Columns[8].Visible == true && txt_d6.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail4.HeaderRow.Cells[8].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_d6.Focus();
                        break;
                    }
                    else if (txt_d7.Text == "" && grdDetail4.Columns[9].Visible == true && txt_d7.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail4.HeaderRow.Cells[9].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_d7.Focus();
                        break;
                    }
                    else if (txt_d8.Text == "" && grdDetail4.Columns[10].Visible == true && txt_d8.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail4.HeaderRow.Cells[10].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_d8.Focus();
                        break;
                    }
                    else if (txt_d9.Text == "" && grdDetail4.Columns[11].Visible == true && txt_d9.Enabled == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail4.HeaderRow.Cells[11].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_d9.Focus();
                        break;
                    }
                    if (txt_d1.Text.Trim() != "" || txt_d2.Text.Trim() != "" || txt_d3.Text.Trim() != "" ||
                         txt_d4.Text.Trim() != "" || txt_d5.Text.Trim() != "" || txt_d6.Text.Trim() != "" || txt_d7.Text.Trim() != "" || txt_d8.Text.Trim() != "" || txt_d9.Text.Trim() != "")
                    {
                        dataAvailable = true;
                    }
                }
            }
            else if (chkBox4.Visible == true && chkBox4.Checked == false && validCheckBox != true)
                validCheckBox = false;

            if (grdDetail5.Visible == true && valid == true && chkBox5.Checked == true)
            {
                validCheckBox = true;
                for (int i = 0; i < grdDetail5.Rows.Count; i++)
                {
                    TextBox txt_e1 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e1");
                    TextBox txt_e2 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e2");
                    TextBox txt_e3 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e3");
                    TextBox txt_e4 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e4");
                    TextBox txt_e5 = (TextBox)grdDetail5.Rows[i].FindControl("txt_e5");

                    if (txt_e1.Text == "" && grdDetail5.Columns[3].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail5.HeaderRow.Cells[3].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_e1.Focus();
                        break;
                    }
                    else if (txt_e2.Text == "" && grdDetail5.Columns[4].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail5.HeaderRow.Cells[4].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_e2.Focus();
                        break;
                    }
                    else if (txt_e3.Text == "" && grdDetail5.Columns[5].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail5.HeaderRow.Cells[5].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_e3.Focus();
                        break;
                    }
                    else if (txt_e4.Text == "" && grdDetail5.Columns[6].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail5.HeaderRow.Cells[6].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_e4.Focus();
                        break;
                    }
                    else if (txt_e5.Text == "" && grdDetail5.Columns[7].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail5.HeaderRow.Cells[7].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_e5.Focus();
                        break;
                    }
                    if (txt_e1.Text.Trim() != "" || txt_e2.Text.Trim() != "" || txt_e3.Text.Trim() != "" ||
                        txt_e4.Text.Trim() != "" || txt_e5.Text.Trim() != "")
                    {
                        dataAvailable = true;
                    }
                }
            }
            else if (chkBox5.Visible == true && chkBox5.Checked == false && validCheckBox != true)
                validCheckBox = false;

            if (grdDetail6.Visible == true && valid == true && chkBox6.Checked == true)
            {
                validCheckBox = true;
                for (int i = 0; i < grdDetail6.Rows.Count; i++)
                {
                    TextBox txt_f1 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f1");
                    TextBox txt_f2 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f2");
                    TextBox txt_f3 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f3");
                    TextBox txt_f4 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f4");
                    TextBox txt_f5 = (TextBox)grdDetail6.Rows[i].FindControl("txt_f5");

                    if (txt_f1.Text == "" && grdDetail6.Columns[3].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail6.HeaderRow.Cells[3].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_f1.Focus();
                        break;
                    }
                    else if (txt_f2.Text == "" && grdDetail6.Columns[4].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail6.HeaderRow.Cells[4].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_f2.Focus();
                        break;
                    }
                    else if (txt_f3.Text == "" && grdDetail6.Columns[5].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail6.HeaderRow.Cells[5].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_f3.Focus();
                        break;
                    }
                    else if (txt_f4.Text == "" && grdDetail6.Columns[6].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail6.HeaderRow.Cells[6].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_f4.Focus();
                        break;
                    }
                    else if (txt_f5.Text == "" && grdDetail6.Columns[7].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail6.HeaderRow.Cells[7].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_f5.Focus();
                        break;
                    }
                    if (txt_f1.Text.Trim() != "" || txt_f2.Text.Trim() != "" || txt_f3.Text.Trim() != "" ||
                        txt_f4.Text.Trim() != "" || txt_f5.Text.Trim() != "")
                    {
                        dataAvailable = true;
                    }
                }
            }
            else if (chkBox6.Visible == true && chkBox6.Checked == false && validCheckBox != true)
                validCheckBox = false;



            if (grdDetail7.Visible == true && valid == true && chkBox7.Checked == true)
            {
                validCheckBox = true;

                for (int i = 0; i < grdDetail7.Rows.Count; i++)
                {
                    TextBox txt_g1 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g1");
                    TextBox txt_g2 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g2");
                    TextBox txt_g3 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g3");
                    TextBox txt_g4 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g4");
                    TextBox txt_g5 = (TextBox)grdDetail7.Rows[i].FindControl("txt_g5");

                    if (txt_g1.Text == "" && grdDetail7.Columns[4].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail7.HeaderRow.Cells[4].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_g1.Focus();
                        break;
                    }
                    else if (txt_g2.Text == "" && grdDetail7.Columns[5].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail7.HeaderRow.Cells[5].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_g2.Focus();
                        break;
                    }
                    else if (txt_g3.Text == "" && grdDetail7.Columns[6].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail7.HeaderRow.Cells[6].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_g3.Focus();
                        break;
                    }
                    else if (txt_g4.Text == "" && grdDetail7.Columns[7].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail7.HeaderRow.Cells[7].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_g4.Focus();
                        break;
                    }
                    else if (txt_g5.Text == "" && grdDetail7.Columns[8].Visible == true)
                    {
                        lblMsg.Text = "Input '" + grdDetail7.HeaderRow.Cells[8].Text.Replace("<br />", "") + "' for Sr No. " + (i + 1) + ".";
                        valid = false;
                        txt_g5.Focus();
                        break;
                    }

                    if (txt_g1.Text.Trim() != "" || txt_g2.Text.Trim() != "" || txt_g3.Text.Trim() != "" ||
                        txt_g4.Text.Trim() != "" || txt_g5.Text.Trim() != "")
                    {
                        dataAvailable = true;
                    }
                }
            }
            else if (chkBox7.Visible == true && chkBox7.Checked == false && validCheckBox != true)
                validCheckBox = false;

            if (dataAvailable == false && lblEntry.Text == "Check" && valid == true)
            {

                var fileupl = dc.OtherReport_View(txt_ReferenceNo.Text).ToList();
                if (dataAvailable == false && fileupl.Count() == 0) //&& lblFileName.Text == ""
                {
                    lblMsg.Text = "Either enter data or upload file.";
                    valid = false;
                }
            }
            if (ddl_Category.SelectedIndex <= 0 && valid == true)
            {
                lblMsg.Text = "Select 'Category'.";
                valid = false;
                ddl_Category.Focus();
            }
            else if (ddl_NablScope.SelectedItem.Text == "--Select--" && valid == true)
            {
                lblMsg.Text = "Select 'NABL Scope'.";
                valid = false;
                ddl_NablScope.Focus();
            }
            else if (ddl_NABLLocation.SelectedItem.Text == "--Select--" && valid == true)
            {
                lblMsg.Text = "Select 'NABL Location'.";
                valid = false;
                ddl_NABLLocation.Focus();
            }
            else if (ddl_TestedBy.SelectedIndex <= 0 && valid == true)
            {
                lblMsg.Text = "Select " + lbl_TestedBy.Text;
                valid = false;
                ddl_TestedBy.Focus();
            }

            if (valid == true && (chkBox1.Visible == false && chkBox2.Visible == false && chkBox3.Visible == false && chkBox4.Visible == false && chkBox5.Visible == false && chkBox6.Visible == false && chkBox7.Visible == false && chkBox8.Visible == false))
                validCheckBox = true;

            if (valid == true && validCheckBox == false)
            {
                valid = false;
                lblMsg.Text = "Select at least one test.";
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
            }
            else
            {
                lblMsg.Visible = false;
                //if (ddl_ReportFor.SelectedValue == "13")
                //    Calculation();
            }
            if (ddl_Category.Text.Trim() != "" || ddl_NABLLocation.Text.Trim() != "" || ddl_TestedBy.Text.Trim() != "")
            {
                dataAvailable = true;
            }
            return valid;
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (txt_ReferenceNo.Text != "")
            {
                rpt.OT_PDFReport(txt_ReferenceNo.Text, "");
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

        protected void lnk_Exit_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        #region upload & download template
        protected void lnkTemplate_Click(object sender, EventArgs e)
        {
            //string path = "";
            //Label lblMsg = (Label)Master.FindControl("lblMsg");
            //bool flag = true;
            //if (lblEntry.Text == "Enter")
            //{
            //    if (ddlAddSign.SelectedIndex == -1 || ddlAddSign.SelectedItem.Text == "---Select---")
            //    {
            //        lblMsg.ForeColor = System.Drawing.Color.Red;
            //        lblMsg.Visible = true;
            //        flag = false;
            //        lblMsg.Text = "Please Select Add Sign.";

            //    }
            //    else
            //        path = convetImag(ddlAddSign.SelectedValue.ToString(), ddlAddSign.SelectedItem.Text.Replace(' ', '_'));

            //}
            //else
            //    lblMsg.Visible = false;

            //if (flag)
            //{
            PrintHTMLReport obj = new PrintHTMLReport();
            if (ddl_Format.SelectedItem.Text == "Word")
                obj.OtherReport_Template(txt_ReferenceNo.Text, lblEntry.Text, "", "");//ddlAddSign.SelectedValue.ToString(), path);
            else if (ddl_Format.SelectedItem.Text == "Excel")
                obj.OtherReport_TemplateExcel(txt_ReferenceNo.Text, lblEntry.Text, "", "");//ddlAddSign.SelectedValue.ToString(), path);

            //}
        }


        public string convetImag(string signVal, string signNm)
        {
            string path = "";
            var data = dc.Sign_View(Convert.ToInt32(signVal));
            foreach (var g in data)
            {
                string path2 = Server.MapPath("~/Images/");
                byte[] imageBytes = g.UserSign.ToArray();
                MemoryStream mxs1 = new MemoryStream(imageBytes);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(imageBytes);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                path = path2 + signNm + ".jpg";
                img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                break;
            }

            return path;
        }
        #endregion
        #region upload & download report        
        ////protected void lnkUpload_Click(object sender, EventArgs e)
        ////{
        ////    //Get temporory uploaded file
        ////    string filename = "";
        ////    string contenttype = "";
        ////    Byte[] bytes = new byte[0];
        ////    Label lblMsg = (Label)Master.FindControl("lblMsg");
        ////    lblMsg.Visible = false;
        ////    var file = dc.TempFile_View(Convert.ToInt32(Session["LoginId"].ToString()), 0);
        ////    foreach (var tempf in file)
        ////    {
        ////        filename = tempf.FileName;
        ////        contenttype = tempf.FileContentType;
        ////        //bytes = mData.getFile(Convert.ToInt32(rw["FileId"].ToString()));
        ////        var file1 = dc.TempFile_View(0, tempf.FileId);
        ////        foreach (var tempf1 in file1)
        ////        {
        ////            bytes = tempf1.FileData.ToArray();
        ////        }
        ////    }
        ////    if (filename != "" && lblFileName.Text != "lblFileName" && lblFileName.Text != "")
        ////    {
        ////        //insert the file into database
        ////        string[] str = txt_ReportNo.Text.Split('/');
        ////        dc.OtherReport_Update(Convert.ToInt32(str[0]), txt_ReferenceNo.Text, filename, contenttype, bytes, false);
        ////        dc.TempFile_Update(Convert.ToInt32(Session["LoginId"].ToString()), "", "", null, true);

        ////        lblMsg.Text = "File Uploaded Successfully"; // "File Uploaded Successfully";                
        ////    }
        ////    else
        ////    {
        ////        lblMsg.Text = "No file available to upload";
        ////    }
        ////    lblMsg.Visible = true;
        ////}

        ////protected void lnkDownload_Click(object sender, EventArgs e)
        ////{
        ////    // Get the file from the database
        ////    var q = dc.OtherReport_View(txt_ReferenceNo.Text);
        ////    foreach (var quote in q)
        ////    {
        ////        string name = (string)quote.OTRPT_FileName_var;
        ////        string contentType = (string)quote.OTRPT_FileContentType_var;
        ////        Byte[] data = (Byte[])quote.OTRPT_FileData_varb.ToArray();

        ////        // Send the file to the browser
        ////        Response.AddHeader("Content-type", contentType);
        ////        Response.AddHeader("Content-Disposition", "attachment; filename=" + name);
        ////        Response.BinaryWrite(data);
        ////        Response.Flush();
        ////        Response.End();
        ////    }
        ////}

        ////protected void lnkRemove_Click(object sender, EventArgs e)
        ////{
        ////    if (lblFileName.Text != "" && lblFileName.Visible == true)
        ////    {
        ////        DeleteFileUploadData();
        ////        Session["FileUpload1"] = null;
        ////        string[] str = txt_ReportNo.Text.Split('/');
        ////        dc.OtherReport_Update(Convert.ToInt32(str[0]), txt_ReferenceNo.Text, "", "", null, true);
        ////        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('File removed successfully..');", true);
        ////    }
        ////}

        ////protected void btnCancelDwnl_Click(object sender, EventArgs e)
        ////{
        ////    DeleteFileUploadData();
        ////}

        ////private void DeleteFileUploadData()
        ////{
        ////    lblFileName.Visible = false;
        ////    lblFileName.Text = "";
        ////    FileUpload1.Visible = true;
        ////    btnCancelDwnl.Visible = false;
        ////    dc.TempFile_Update(Convert.ToInt32(Session["LoginId"].ToString()), "", "", null, true);
        ////}
        protected void lnkUpload_Click(object sender, EventArgs e)
        {
            ////Get temporory uploaded file
            //string filename = "";
            //string contenttype = "";
            //Byte[] bytes = new byte[0];
            //Label lblMsg = (Label)Master.FindControl("lblMsg");
            //lblMsg.Visible = false;
            //var file = dc.TempFile_View(Convert.ToInt32(Session["LoginId"].ToString()), 0);
            //foreach (var tempf in file)
            //{
            //    filename = tempf.FileName;
            //    contenttype = tempf.FileContentType;
            //    //bytes = mData.getFile(Convert.ToInt32(rw["FileId"].ToString()));
            //    var file1 = dc.TempFile_View(0, tempf.FileId);
            //    foreach (var tempf1 in file1)
            //    {
            //        bytes = tempf1.FileData.ToArray();
            //    }
            //}
            //if (filename != "" && lblFileName.Text != "lblFileName" && lblFileName.Text != "")
            //{
            //    //insert the file into database
            //    string[] str = txt_ReportNo.Text.Split('/');
            //    dc.OtherReport_Update(Convert.ToInt32(str[0]), txt_ReferenceNo.Text, filename, contenttype, bytes, false);
            //    dc.TempFile_Update(Convert.ToInt32(Session["LoginId"].ToString()), "", "", null, true);

            //    lblMsg.Text = "File Uploaded Successfully"; // "File Uploaded Successfully";                
            //}
            //else
            //{
            //    lblMsg.Text = "No file available to upload";
            //}
            //lblMsg.Visible = true;

            if (FileUpload1.HasFile == false)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('No file available..');", true);
            }
            else if (FileUpload1.HasFile == true)
            {
                string filename = "";
                string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string ext = Path.GetExtension(filename);
                string contenttype = String.Empty;
                ////Set the contenttype based on File Extension
                switch (ext)
                {
                    case ".doc":
                        contenttype = "application/vnd.ms-word";
                        break;

                    case ".docx":
                        contenttype = "application/vnd.officedocument.wordprocessingml.document";
                        break;

                    case ".xls":
                        contenttype = "application/vnd.ms-excel";
                        break;

                    case ".xlsx":
                        contenttype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;

                    case ".jpg":
                        contenttype = "image/jpg";
                        break;

                    case ".png":
                        contenttype = "image/png";
                        break;

                    case ".gif":
                        contenttype = "image/gif";
                        break;

                    case ".pdf":
                        contenttype = "application/pdf";
                        break;
                    case ".txt":
                        contenttype = "text/plain";
                        break;
                    case ".zip":
                        contenttype = "application/octet-stream"; //"application/x-compressed"; //application/octet-stream
                        break;
                    case ".rar":
                        contenttype = "application/rar";
                        break;
                }

                string filePath = "D:/OtherReportFiles/";
                if (cnStr.ToLower().Contains("mumbai") == true)
                    filePath += "Mumbai/";
                else if (cnStr.ToLower().Contains("nashik") == true)
                    filePath += "Nashik/";
                else if (cnStr.ToLower().Contains("metro") == true)
                    filePath += "Metro/";
                else
                    filePath += "Pune/";
                if (!Directory.Exists(@filePath))
                    Directory.CreateDirectory(@filePath);
                filePath += Path.GetFileName(filename);
                FileUpload1.PostedFile.SaveAs(filePath);

                lblFileName.Text = filename;
                lblFileName.Visible = true;
                if (filename != "" && lblFileName.Text != "lblFileName" && lblFileName.Text != "")
                {
                    string[] str = txt_ReportNo.Text.Split('/');
                    dc.OtherReport_Update(Convert.ToInt32(str[0]), txt_ReferenceNo.Text, filename, contenttype, null, false);
                    //dc.TempFile_Update(Convert.ToInt32(Session["LoginId"].ToString()), "", "", null, true);

                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('File Uploaded Successfully !');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('No file available to upload !');", true);
                }
                
            }
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            //// Get the file from the database
            //var q = dc.OtherReport_View(txt_ReferenceNo.Text);
            //foreach (var quote in q)
            //{
            //    string name = (string)quote.OTRPT_FileName_var;
            //    string contentType = (string)quote.OTRPT_FileContentType_var;
            //    Byte[] data = (Byte[])quote.OTRPT_FileData_varb.ToArray();

            //    // Send the file to the browser
            //    Response.AddHeader("Content-type", contentType);
            //    Response.AddHeader("Content-Disposition", "attachment; filename=" + name);
            //    Response.BinaryWrite(data);
            //    Response.Flush();
            //    Response.End();
            //}
            string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
            string filePath = "D:/OtherReportFiles/";
            if (cnStr.ToLower().Contains("mumbai") == true)
                filePath += "Mumbai/";
            else if (cnStr.ToLower().Contains("nashik") == true)
                filePath += "Nashik/";
            else if (cnStr.ToLower().Contains("metro") == true)
                filePath += "Metro/";
            else
                filePath += "Pune/";

            filePath += lblFileName.Text;
            if (File.Exists(@filePath))
            {
                //HttpResponse res = HttpContext.Current.Response;
                Response.Clear();
                Response.AppendHeader("content-disposition", "attachment; filename=" + filePath);
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(filePath);
                Response.Flush();
                Response.End();
            }
        }

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            if (lblFileName.Text != "" && lblFileName.Visible == true)
            {
                DeleteFileUploadData();
                Session["FileUpload1"] = null;
                string[] str = txt_ReportNo.Text.Split('/');
                dc.OtherReport_Update(Convert.ToInt32(str[0]), txt_ReferenceNo.Text, "", "", null, true);
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('File removed successfully..');", true);
            }
        }

        protected void btnCancelDwnl_Click(object sender, EventArgs e)
        {
            DeleteFileUploadData();
        }

        private void DeleteFileUploadData()
        {
            lblFileName.Visible = false;
            lblFileName.Text = "";
            FileUpload1.Visible = true;
            btnCancelDwnl.Visible = false;
            //dc.TempFile_Update(Convert.ToInt32(Session["LoginId"].ToString()), "", "", null, true);
        }

        protected void lnlDownloadAllFiles_Click(object sender, EventArgs e)
        {
            string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
            string filePath = "D:/OtherReportFiles/";
            if (cnStr.ToLower().Contains("mumbai") == true)
                filePath += "Mumbai/";
            else if (cnStr.ToLower().Contains("nashik") == true)
                filePath += "Nashik/";
            else if (cnStr.ToLower().Contains("metro") == true)
                filePath += "Metro/";
            else
                filePath += "Pune/";

            var q = dc.OtherReport_View("");
            foreach (var quote in q)
            {
                //string filename = (string)quote.OTRPT_FileName_var;
                string filename = "OT_" + quote.OTRPT_ReferenceNo_var.Replace('/', '_');
                string contentType = (string)quote.OTRPT_FileContentType_var;
                if (contentType == "application/vnd.officedocument.wordprocessingml.document")
                    filename += ".docx";
                else if (contentType == "application/vnd.ms-word")
                    filename += ".doc";
                else if (contentType == "application/vnd.ms-excel")
                    filename += ".xls";
                else if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    filename += ".xlsx";
                else if (contentType == "application/pdf")
                    filename += ".pdf";
                else if (contentType == "" && quote.OTRPT_FileName_var.Contains(".xlsb") == true)
                    filename += ".xlsb";
                else if (contentType == "" && quote.OTRPT_FileName_var.Contains(".ods") == true)
                    filename += ".ods";
                
                if (!File.Exists(filePath + filename))
                {
                    //Byte[] data = (Byte[])quote.OTRPT_FileData_varb.ToArray();
                    //FileStream fs = new FileStream(filePath + filename, FileMode.Create);
                    //fs.Write(data, 0, data.Length);
                    //fs.Close();
                    //dc.OtherReport_Update_NewFileName(quote.OTRPT_ReferenceNo_var, filename);
                }
            }
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('File downloaded successfully..');", true);
        }
        #endregion

        protected void grdDetail1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //on you condition
                if (ddl_ReportFor.SelectedValue == "13")
                {

                    TextBox txt_a4 = (TextBox)e.Row.FindControl("txt_a4");
                    TextBox txt_a5 = (TextBox)e.Row.FindControl("txt_a5");
                    txt_a4.Enabled = false;
                    txt_a5.Enabled = false;

                }
                if (ddl_ReportFor.SelectedValue == "26")
                {

                    TextBox txt_a7 = (TextBox)e.Row.FindControl("txt_a7");
                    TextBox txt_a8 = (TextBox)e.Row.FindControl("txt_a8");

                    txt_a7.Enabled = false;
                    txt_a8.Enabled = false;
                    // txt_a5.Enabled = false;
                    //txt_a5.ReadOnly = true;
                }
                if (ddl_ReportFor.SelectedValue == "27")
                {
                    TextBox txt_a6 = (TextBox)e.Row.FindControl("txt_a6");
                    txt_a6.Enabled = false;
                    //txt_a5.ReadOnly = true;
                }
                if (ddl_ReportFor.SelectedValue == "29")
                {

                    TextBox txt_a3 = (TextBox)e.Row.FindControl("txt_a3");
                    TextBox txt_a4 = (TextBox)e.Row.FindControl("txt_a4");
                    TextBox txt_a5 = (TextBox)e.Row.FindControl("txt_a5");
                    txt_a3.Enabled = false;
                    txt_a4.Enabled = false;
                    txt_a5.Enabled = false;
                    //txt_a5.ReadOnly = true;
                }
                if (ddl_ReportFor.SelectedValue == "30")
                {

                    TextBox txt_a6 = (TextBox)e.Row.FindControl("txt_a6");
                    TextBox txt_a7 = (TextBox)e.Row.FindControl("txt_a7");

                    txt_a6.Enabled = false;
                    txt_a7.Enabled = false;

                    //txt_a5.ReadOnly = true;
                }
                if (ddl_ReportFor.SelectedValue == "31")
                {

                    //   TextBox txt_a3 = (TextBox)e.Row.FindControl("txt_a3");
                    //   TextBox txt_a4 = (TextBox)e.Row.FindControl("txt_a4");
                    TextBox txt_a5 = (TextBox)e.Row.FindControl("txt_a5");
                    //  txt_a3.Enabled = false;
                    //   txt_a4.Enabled = false;
                    txt_a5.Enabled = false;

                }
                if (ddl_ReportFor.SelectedValue == "32")
                {
                    TextBox txt_a5 = (TextBox)e.Row.FindControl("txt_a5");
                    TextBox txt_a7 = (TextBox)e.Row.FindControl("txt_a7");
                    TextBox txt_a8 = (TextBox)e.Row.FindControl("txt_a8");
                    txt_a5.Enabled = false;
                    txt_a7.Enabled = false;
                    txt_a8.Enabled = false;
                    //txt_a5.ReadOnly = true;
                }
                if (ddl_ReportFor.SelectedValue == "34")
                {
                    TextBox txt_a1 = (TextBox)e.Row.FindControl("txt_a1");
                    TextBox txt_a2 = (TextBox)e.Row.FindControl("txt_a2");
                    TextBox txt_a3 = (TextBox)e.Row.FindControl("txt_a3");
                    TextBox txt_a4 = (TextBox)e.Row.FindControl("txt_a4");
                    TextBox txt_a5 = (TextBox)e.Row.FindControl("txt_a5");
                    TextBox txt_a6 = (TextBox)e.Row.FindControl("txt_a6");
                    TextBox txt_a7 = (TextBox)e.Row.FindControl("txt_a7");
                    txt_a1.Width = 300;
                    txt_a2.Width = 50;
                    txt_a3.Width = 50;
                    txt_a4.Width = 50;
                    txt_a5.Width = 10;
                    txt_a6.Width = 150;
                    txt_a7.Width = 50;
                }
                if (ddl_ReportFor.SelectedValue == "35")
                {
                    TextBox txt_a4 = (TextBox)e.Row.FindControl("txt_a4");
                    TextBox txt_a5 = (TextBox)e.Row.FindControl("txt_a5");
                    txt_a4.Enabled = false;                    
                    txt_a4.ReadOnly = true;                    
                    if (e.Row.RowIndex > 0)
                    {
                        txt_a4.Text = " ";
                        txt_a5.Text = " ";
                        txt_a5.Enabled = false;
                        txt_a5.ReadOnly = true;
                    }
                    if (txt_a5.Text == "")
                        txt_a5.Text = " ";
                }
                if (ddl_ReportFor.SelectedValue == "36")
                {
                    TextBox txt_a4 = (TextBox)e.Row.FindControl("txt_a4");
                    TextBox txt_a5 = (TextBox)e.Row.FindControl("txt_a5");
                    txt_a4.Enabled = false;
                    txt_a5.Enabled = false;
                    if (e.Row.RowIndex > 0)
                    {
                        txt_a4.Text = " ";
                        txt_a5.Text = " ";
                        txt_a5.Enabled = false;
                        txt_a5.Enabled = false;
                    }
                }
            }
        }

        protected void grdDetail2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //on you condition
                if (ddl_ReportFor.SelectedValue == "17")
                {
                    ImageButton imgBtnAddRow2 = (ImageButton)e.Row.FindControl("imgBtnAddRow2");
                    ImageButton imgBtnDeleteRow2 = (ImageButton)e.Row.FindControl("imgBtnDeleteRow2");

                    TextBox txt_b1 = (TextBox)e.Row.FindControl("txt_b1");
                    TextBox txt_b2 = (TextBox)e.Row.FindControl("txt_b2");
                    txt_b1.Enabled = false;
                    txt_b2.Enabled = false;
                    imgBtnAddRow2.Enabled = false;
                    imgBtnDeleteRow2.Enabled = false;
                    //txt_a5.ReadOnly = true;
                }
                else if (ddl_ReportFor.SelectedValue == "34")
                {
                    TextBox txt_b2 = (TextBox)e.Row.FindControl("txt_b2");
                    TextBox txt_b3 = (TextBox)e.Row.FindControl("txt_b3");
                    TextBox txt_b5 = (TextBox)e.Row.FindControl("txt_b5");
                    TextBox txt_b7 = (TextBox)e.Row.FindControl("txt_b7");
                    TextBox txt_b8 = (TextBox)e.Row.FindControl("txt_b8");
                    TextBox txt_b9 = (TextBox)e.Row.FindControl("txt_b9");

                    txt_b2.Enabled = false;
                    txt_b3.Enabled = false;
                    txt_b5.Enabled = false;
                    txt_b7.Enabled = false;
                    txt_b8.Enabled = false;
                    txt_b9.Enabled = false;
                }

            }
        }
        protected void grdDetail3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ddl_ReportFor.SelectedValue == "34")
                {
                    TextBox txt_c2 = (TextBox)e.Row.FindControl("txt_c2");
                    TextBox txt_c3 = (TextBox)e.Row.FindControl("txt_c3");
                    TextBox txt_c5 = (TextBox)e.Row.FindControl("txt_c5");
                    TextBox txt_c7 = (TextBox)e.Row.FindControl("txt_c7");
                    TextBox txt_c8 = (TextBox)e.Row.FindControl("txt_c8");
                    TextBox txt_c9 = (TextBox)e.Row.FindControl("txt_c9");

                    txt_c2.Enabled = false;
                    txt_c3.Enabled = false;
                    txt_c5.Enabled = false;
                    txt_c7.Enabled = false;
                    txt_c8.Enabled = false;
                    txt_c9.Enabled = false;
                }

            }
        }
        protected void grdDetail4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ddl_ReportFor.SelectedValue == "34")
                {
                    TextBox txt_d2 = (TextBox)e.Row.FindControl("txt_d2");
                    TextBox txt_d3 = (TextBox)e.Row.FindControl("txt_d3");
                    TextBox txt_d5 = (TextBox)e.Row.FindControl("txt_d5");
                    TextBox txt_d7 = (TextBox)e.Row.FindControl("txt_d7");
                    TextBox txt_d8 = (TextBox)e.Row.FindControl("txt_d8");
                    TextBox txt_d9 = (TextBox)e.Row.FindControl("txt_d9");

                    txt_d2.Enabled = false;
                    txt_d3.Enabled = false;
                    txt_d5.Enabled = false;
                    txt_d7.Enabled = false;
                    txt_d8.Enabled = false;
                    txt_d9.Enabled = false;
                }

            }
        }
        protected void grdDetail8_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ddl_ReportFor.SelectedValue == "21")
                {
                    TextBox txt_a5 = (TextBox)e.Row.FindControl("txt_a5");
                    txt_a5.Enabled = false;
                }
                else if (ddl_ReportFor.SelectedValue == "32")
                {
                    TextBox txt_a4 = (TextBox)e.Row.FindControl("txt_a4");
                    TextBox txt_a5 = (TextBox)e.Row.FindControl("txt_a5");
                    txt_a4.Enabled = false;
                    txt_a5.Enabled = false;
                }
            }
        }

        protected void imgBtnMergeRow_Click(object sender, CommandEventArgs e)
        {
            ImageButton img = (ImageButton)sender;
            GridViewRow row = (GridViewRow)img.Parent.Parent;
            int rowindex = row.RowIndex;
            Label lblMergFlag = (Label)grdDetail8.Rows[rowindex].FindControl("lblMergFlag");
            if (lblMergFlag.Text == "1")
                UnMergeRow(rowindex);
            else
                MergeRow(rowindex);
        }
        public void ShowMergeRow()
        {
            int j = 1;
            for (int i = 0; i < grdDetail8.Rows.Count; i++)
            {
                Label lblMergFlag1 = (Label)grdDetail8.Rows[i].FindControl("lblMergFlag");

                if (lblMergFlag1.Text == "1")
                {
                    grdDetail8.Rows[i].Cells[3].Text = "";
                    MergeRow(i);
                    //   MergeRows(grdDetail8);
                }
                else
                {
                    grdDetail8.Rows[i].Cells[3].Text = (j++).ToString();
                    UnMergeRow(i);
                    // MergeRows(grdDetail8);

                }
            }
        }
        public void MergeRow(int rowindex)
        {
            Label lblMergFlag = (Label)grdDetail8.Rows[rowindex].FindControl("lblMergFlag");
            TextBox txt_a1 = (TextBox)grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[4].FindControl("txt_a1");

            if (ddl_ReportFor.SelectedValue == "21")
                grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[4].ColumnSpan += 5;
            else
                grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[4].ColumnSpan += 4;

            txt_a1.Width = 150;
            txt_a1.CssClass = "Titlecol";

            grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[5].Visible = false;
            grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[6].Visible = false;
            grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[7].Visible = false;
            grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[8].Visible = false;
            grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[9].Visible = false;
            grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[10].Visible = false;
            //grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[11].Visible = false;


            lblMergFlag.Text = "1";
            int j = 1;
            for (int i = 0; i < grdDetail8.Rows.Count; i++)
            {
                Label lblMergFlag1 = (Label)grdDetail8.Rows[i].FindControl("lblMergFlag");
                grdDetail8.Rows[i].Cells[3].Text = (j++).ToString();
                if (lblMergFlag1.Text == "1")
                {
                    j--;
                    grdDetail8.Rows[i].Cells[3].Text = "";
                }
            }
        }
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                if (row.Cells[0].Text == previousRow.Cells[0].Text)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                }
            }
        }
        public void UnMergeRow(int rowindex)
        {
            Label lblMergFlag = (Label)grdDetail8.Rows[rowindex].FindControl("lblMergFlag");
            TextBox txt_a1 = (TextBox)grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[4].FindControl("txt_a1");
            grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[4].ColumnSpan = 1;
            txt_a1.Width = 150;
            txt_a1.CssClass = "Detailcol";
            grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[5].Visible = true;
            grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[6].Visible = true;
            grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[7].Visible = true;
            grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[8].Visible = true;
            grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[9].Visible = true;
            grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[10].Visible = true;
            //grdDetail8.Rows[Convert.ToInt32(rowindex)].Cells[11].Visible = true;

            lblMergFlag.Text = "0";
            int j = 1;
            for (int i = 0; i < grdDetail8.Rows.Count; i++)
            {
                Label lblMergFlag1 = (Label)grdDetail8.Rows[i].FindControl("lblMergFlag");
                grdDetail8.Rows[i].Cells[3].Text = (j++).ToString();
                if (lblMergFlag1.Text == "1")
                {
                    j--;
                    grdDetail8.Rows[i].Cells[3].Text = "";
                }
            }
        }
        protected void txtNoOfSpecimens_TextChanged(object sender, EventArgs e)
        {
            if (ddl_ReportFor.SelectedValue == "34")
            {
                GridViewRow gvr = ((TextBox)sender).NamingContainer as GridViewRow;
                if (gvr != null && gvr.RowIndex == 1)
                {
                    int noOfSpecimens = 0;
                    TextBox txtNoOfSpecimens = (TextBox)gvr.FindControl("txt_a7");
                    if (txtNoOfSpecimens.Text != "" && Int32.TryParse(txtNoOfSpecimens.Text, out noOfSpecimens))
                    {
                        txtNoOfSpecimens.Text = "1";
                        chkBox3.Checked = false;
                        chkBox3.Visible = false;
                        lblHeading3.Visible = false;
                        grdDetail3.Visible = false;
                        chkBox4.Checked = false;
                        chkBox4.Visible = false;
                        lblHeading4.Visible = false;
                        grdDetail4.Visible = false;

                        if (noOfSpecimens > 3)
                        {
                            txtNoOfSpecimens.Text = "3";
                            chkBox3.Checked = true;
                            chkBox3.Visible = true;
                            lblHeading3.Visible = true;
                            grdDetail3.Visible = true;
                            chkBox4.Checked = true;
                            chkBox4.Visible = true;
                            lblHeading4.Visible = true;
                            grdDetail4.Visible = true;
                        }
                        else if (noOfSpecimens == 2)
                        {
                            chkBox3.Checked = true;
                            chkBox3.Visible = true;
                            lblHeading3.Visible = true;
                            grdDetail3.Visible = true;                            
                        }
                    }
                }
            }
        }
    }
}
