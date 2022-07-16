using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Ionic.Zip;

namespace DESPLWEB
{
    public partial class SiteWiseRateList : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        clsData db = new clsData();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Sitewise Rate List";
                
                Session["Cl_Id"] = 0;
                Session["Site_Id"] = 0;
                lblCLId.Text = "0";
                lblSiteId.Text = "0";
                ViewState["TestDetails"] = null;
                
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    bool userRight = false;
                    var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                    foreach (var u in user)
                    {
                        if (u.User_sitewise_rate_bit == true)
                        {
                            userRight = true;
                        }
                    }

                    if (userRight == false)
                    {
                        pnlContent.Visible = false;
                        lblAccess.Visible = true;
                        lblAccess.Text = "Access is Denied.. ";
                    }
                    else
                    {
                        LoadRecordTypeList();
                        LoadOtherTestList();
                    }
                }
            }
        }

        private void LoadRecordTypeList()
        {
            var inwd = dc.Material_View("", "");
            ddlRecordType.DataSource = inwd;
            ddlRecordType.DataTextField = "MATERIAL_Name_var";
            ddlRecordType.DataValueField = "MATERIAL_RecordType_var";
            ddlRecordType.DataBind();
            ddlRecordType.Items.Insert(0, new ListItem("---All---"));
        }

        protected void LoadOtherTestList()
        {
            var test = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, 0);
            ddlOtherTest.DataSource = test;
            ddlOtherTest.DataTextField = "TEST_Name_var";
            ddlOtherTest.DataValueField = "TEST_Id";
            ddlOtherTest.DataBind();
            ddlOtherTest.Items.Insert(0, new ListItem("---All---", "0"));

            //int subTestId = Convert.ToInt32(ddlOtherTest.SelectedValue);
            //var res = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, subTestId);
        }
        
        protected void AddTestDetail()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["TestDetails"] != null)
            {
                dt.Columns.Add(new DataColumn("Sr.No", typeof(string)));
                dt.Columns.Add(new DataColumn("Record Type", typeof(string)));
                dt.Columns.Add(new DataColumn("Test Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Sub Test Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Criteria", typeof(string)));
                dt.Columns.Add(new DataColumn("Current Rate", typeof(string)));
                dt.Columns.Add(new DataColumn("New Rate", typeof(string)));
            }

            dr = dt.NewRow();
            dr["Sr.No"] = dt.Rows.Count + 1;
            dr["Record Type"] = string.Empty;
            dr["Test Name"] = string.Empty;
            dr["Sub Test Name"] = string.Empty;
            dr["Criteria"] = string.Empty;
            dr["Current Rate"] = string.Empty;
            dr["New Rate"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["TestDetails"] = dt;
        }
        
        DataTable GetDataTable(GridView dtg)
        {
            DataTable dt = new DataTable();
            string[] arr = { "Sr.No", "Test_Id", "SITERATE_Id", "Test_RecType_var", "TEST_Name_var", "otherTestType", "TEST_From_num", "TEST_To_num", "TEST_Rate_int", "SITERATE_Test_Rate_int" };
            // add the columns to the datatable
            if (dtg.HeaderRow != null)
            {
                for (int i = 0; i < arr.Count(); i++)
                {
                    dt.Columns.Add(arr[i]);
                }
            }

            // add each of the data rows to the table
            foreach (GridViewRow row in dtg.Rows)
            {
                DataRow dr;
                dr = dt.NewRow();
                Label lblSrNo = (Label)row.FindControl("lblSrNo");
                Label lblTestId = (Label)row.FindControl("lblTestId");
                Label lblRateId = (Label)row.FindControl("lblRateId");
                Label lblRecordType = (Label)row.FindControl("lblRecordType");
                Label lblTestName = (Label)row.FindControl("lblTestName");
                Label lblOtherTestType = (Label)row.FindControl("lblOtherTestType");
                Label lblCriteria = (Label)row.FindControl("lblCriteria");
                Label lblRate = (Label)row.FindControl("lblRate");
                TextBox txt_NewRate = (TextBox)row.FindControl("txt_NewRate");
                string[] a = lblCriteria.Text.Split('-');//TEST_From_num-TEST_To_num
                dr[0] = lblSrNo.Text;
                dr[1] = lblTestId.Text;
                dr[2] = lblRateId.Text;
                dr[3] = lblRecordType.Text;
                dr[4] = lblTestName.Text;
                dr[5] = lblOtherTestType.Text;
                dr[6] = a[0];
                dr[7] = a[1];
                dr[8] = lblRate.Text;
                dr[9] = txt_NewRate.Text;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (txt_Client.Text != "" && txt_Site.Text != "")
            {
                txtPer.Text = "";
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                int CL_ID = Convert.ToInt32(lblCLId.Text);
                int SITE_ID = Convert.ToInt32(lblSiteId.Text);

                var rt = dc.SiteWiseRate_View(CL_ID, SITE_ID, 0, false);
                grdRate.DataSource = rt;
                grdRate.DataBind();

                DataTable dt = new DataTable();
                dt = GetDataTable(grdRate);
                ViewState["TestDetails"] = dt;
                chkApplyToAll.Checked = false;
                
                string recordTypeValue = "";
                if (ddlRecordType.SelectedIndex != -1 && ddlRecordType.SelectedItem.Text != "")
                {
                    if (ViewState["TestDetails"] != null)
                    {
                        dt = (DataTable)ViewState["TestDetails"];
                        DataView dv = new DataView(dt);
                        if (ddlRecordType.SelectedItem.Text != "---All---" && dt.Rows.Count > 0)
                        {
                            recordTypeValue = ddlRecordType.SelectedValue;
                            if (recordTypeValue == "OT")
                            {   
                                recordTypeValue = "OTHER";
                                string strFilter = "Test_RecType_var = '" + recordTypeValue + "'";
                                if (ddlOtherTest.SelectedValue != "0")
                                {
                                    strFilter += "and otherTestType = '" + ddlOtherTest.SelectedItem.Text  + "'";
                                }
                                dv.RowFilter = strFilter;
                            }
                            else
                            {
                                dv.RowFilter = "Test_RecType_var = '" + recordTypeValue + "'";
                            }
                        }
                        grdRate.DataSource = dv;
                        grdRate.DataBind();
                    }
                }

            }
        }
        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            if (grdRate.Rows.Count > 0)
            {
                int subTestId = 0;
                string filterType = ddlRecordType.SelectedValue;
                if (ddlRecordType.SelectedValue == "OT")
                {
                    filterType = "OTHER";
                    subTestId = Convert.ToInt32(ddlOtherTest.SelectedValue);
                }
                int CL_ID = Convert.ToInt32(lblCLId.Text);
                int SITE_ID = Convert.ToInt32(lblSiteId.Text);
                if (lblDisc.Text == "0") //delete rate from sitewise table
                {
                    //if dicount set to 0 then delete all site 
                    if (chkApplyToAll.Checked == false) //indivisually update rate as per selected inward type to selected site
                    {
                        dc.SiteWiseRate_Update(0, 0, CL_ID, SITE_ID, 0, 3, filterType, subTestId); //delete all record of that client and site
                    }
                    else
                    {
                        //var res = dc.Site_View(0, CL_ID, 0, "").ToList();
                        //if (res.Count > 0)
                        //{
                        //    dc.SiteWiseRate_Update(0, 0, CL_ID, 0, 0, 2, filterType); //delete all record of that client
                        //}
                        dc.SiteWiseRate_Update(0, 0, CL_ID, 0, 0, 2, filterType, subTestId); //delete all record of that client
                    }
                }
                else
                {
                    if (chkApplyToAll.Checked == false)//indivisually update rate as per selected inward type to selected site
                    {
                        dc.SiteWiseRate_Update(0, 0, CL_ID, SITE_ID, 0, 3, filterType, subTestId);//delete all record of that client and site
                        for (int i = 0; i < grdRate.Rows.Count; i++)
                        {
                            Label lblTestId = (Label)grdRate.Rows[i].Cells[0].FindControl("lblTestId");
                            Label lblRateId = (Label)grdRate.Rows[i].Cells[1].FindControl("lblRateId");
                            Label lblTestName = (Label)grdRate.Rows[i].Cells[2].FindControl("lblTestName");
                            Label lblRecordType = (Label)grdRate.Rows[i].Cells[3].FindControl("lblRecordType");
                            Label lblRate = (Label)grdRate.Rows[i].Cells[4].FindControl("lblRate");
                            TextBox txt_NewRate = (TextBox)grdRate.Rows[i].Cells[5].FindControl("txt_NewRate");

                            if (txt_NewRate.Text != "")
                            {
                                if (Convert.ToDecimal(txt_NewRate.Text) <= Convert.ToDecimal(lblRate.Text))
                                    dc.SiteWiseRate_Update(Convert.ToInt32(lblTestId.Text), Convert.ToDecimal(txt_NewRate.Text), CL_ID, SITE_ID, 0, 0, "", 0);//insert new record
                            }
                        }
                    }
                    else//update rate as per selected inward type to all site
                    {
                        var res = dc.Site_View(0, CL_ID, 0, "").ToList();
                        if (res.Count > 0)
                        {
                            dc.SiteWiseRate_Update(0, 0, CL_ID, 0, 0, 2, filterType, subTestId);//delete all record of that client
                        }
                        foreach (var item in res)
                        {
                            for (int i = 0; i < grdRate.Rows.Count; i++)
                            {
                                Label lblTestId = (Label)grdRate.Rows[i].Cells[0].FindControl("lblTestId");
                                Label lblRateId = (Label)grdRate.Rows[i].Cells[1].FindControl("lblRateId");
                                Label lblTestName = (Label)grdRate.Rows[i].Cells[2].FindControl("lblTestName");
                                Label lblRecordType = (Label)grdRate.Rows[i].Cells[3].FindControl("lblRecordType");
                                Label lblRate = (Label)grdRate.Rows[i].Cells[4].FindControl("lblRate");
                                TextBox txt_NewRate = (TextBox)grdRate.Rows[i].Cells[5].FindControl("txt_NewRate");

                                if (txt_NewRate.Text != "")
                                {
                                    if (Convert.ToDecimal(txt_NewRate.Text) <= Convert.ToDecimal(lblRate.Text))
                                        dc.SiteWiseRate_Update(Convert.ToInt32(lblTestId.Text), Convert.ToDecimal(txt_NewRate.Text), CL_ID, Convert.ToInt32(item.SITE_Id), 0, 0, "", 0);//insert
                                }
                            }
                        }
                    }
                }

                ddlRecordType.SelectedIndex = 0;
                ddlOtherTest.Visible = false;

                //DataTable dt = new DataTable();
                //dt = db.getSiteWiseData(CL_ID, SITE_ID);
                //ViewState["TestDetails"] = dt;
                //grdRate.DataSource = dt;
                //grdRate.DataBind();

                var rt = dc.SiteWiseRate_View(CL_ID, SITE_ID, 0, false);
                grdRate.DataSource = rt;
                grdRate.DataBind();
                DataTable dt = new DataTable();
                dt = GetDataTable(grdRate);
                ViewState["TestDetails"] = dt;

                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Visible = true;
                lblMsg.Text = "Successfully Updated..";
            }
        }
        protected void lnkApplyPer_Click(object sender, EventArgs e)
        {
            if (txtPer.Text != "")// && txtPer.Text != "0"
            {
                double per = Convert.ToDouble(txtPer.Text);
                lblDisc.Text = per.ToString();
                double NewRate = 0;
                for (int i = 0; i < grdRate.Rows.Count; i++)
                {
                    Label lblRate = (Label)grdRate.Rows[i].FindControl("lblRate");
                    TextBox txt_NewRate = (TextBox)grdRate.Rows[i].FindControl("txt_NewRate");
                    
                    if (lblRate.Text != "" && lblRate.Text != "0")
                    {
                        NewRate = Convert.ToDouble(lblRate.Text) - (Convert.ToDouble(lblRate.Text) * (per / 100));
                        txt_NewRate.Text = NewRate.ToString("0.00");
                    }
                }             
            }           
        }
        protected void lnkClear_Click(object sender, EventArgs e)
        {
            txt_Client.Text = "";
            txt_Site.Text = "";
            Clear();
        }
        private void Clear()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            grdRate.DataSource = null;
            grdRate.DataBind();
            ViewState["TestDetails"] = null;
            ddlRecordType.SelectedIndex = -1;
            chkApplyToAll.Checked = false;
            ddlOtherTest.Visible = false;

            fileUpload1.Enabled = false;
            ddlFiles.Items.Clear();
            btnUpload.Enabled = false;
            btnDownload.Enabled = false;
            lbltxt.Text = "";
        }
        protected void ddlRecordType_OnSelectedIndexChanged(object sender, EventArgs e)
        {   
            if (ddlRecordType.SelectedValue == "OT")
            {
                ddlOtherTest.Visible = true;
                ddlOtherTest.SelectedIndex = 0;
            }
            else
            {
                ddlOtherTest.Visible = false;
            }
        }


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetClientname(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Client_View(0, 0, searchHead, "");
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
        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            txt_Site.Text = "";
            Clear();
            if (txt_Client.Text != "")
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                if (ChkClientName(txt_Client.Text) == true)
                {
                    if (txt_Client.Text != "")
                    {
                        Session["CL_ID"] = Convert.ToInt32(Request.Form[hfClientId.UniqueID]);
                        lblCLId.Text = Convert.ToInt32(Request.Form[hfClientId.UniqueID]).ToString();
                    }
                    else
                    {
                        Session["CL_ID"] = 0;
                        lblCLId.Text = "0";
                    }
                }
            }
            //txt_Site.Text = "";
            //Clear();
        }
        protected void txt_Site_TextChanged(object sender, EventArgs e)
        {
            int cl_Id = 0;
            Clear();
            if (Convert.ToInt32(lblCLId.Text) > 0)
            {
                if (int.TryParse(lblCLId.Text.ToString(), out cl_Id))
                {
                    if (ChkSiteName(txt_Site.Text) == true)
                    {
                        int SiteId = 0;
                        if (int.TryParse(Request.Form[hfSiteId.UniqueID], out SiteId))
                        {
                            lblSiteId.Text = Convert.ToInt32(Request.Form[hfSiteId.UniqueID]).ToString();
                            Session["SITE_ID"] = Convert.ToInt32(Request.Form[hfSiteId.UniqueID]);
                            fileUpload1.Enabled = true;
                            btnUpload.Enabled = true;
                            btnDownload.Enabled = true;
                            LoadMouFileList();
                        }
                        else
                        {
                            Session["SITE_ID"] = 0;
                            lblSiteId.Text = "0";                            
                        }

                    }
                }
            }
            //Clear();
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
                lblMsg.ForeColor = System.Drawing.Color.Red;

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
            var res = dc.Site_View(0, Convert.ToInt32(Session["CL_ID"]), 0, searchHead);
            foreach (var obj in res)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Site.Focus();
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;

                lblMsg.Text = "This Site Name is not in the list ";
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }

        protected void LoadMouFileList()
        {
            string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
            string filePath = "D:/MOUFiles/";
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
            
            //string[] listOfFiles = Directory.GetFiles(filePath);
            List<ListItem> files = new List<ListItem>();
            //foreach (string path in filesInDir)
            //{
            //    files.Add(new ListItem(Path.GetFileName(path)));
            //}
            string partialName = lblSiteId.Text + "_";
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(filePath);
            FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + partialName + "*");
            foreach (FileInfo foundFile in filesInDir)            
            {
                files.Add(new ListItem(foundFile.Name));
            }
            ddlFiles.DataSource = files;
            ddlFiles.DataBind();
            if (ddlFiles.Items.Count > 0)
                ddlFiles.Items.Insert(0, new ListItem("---Select---"));
        }
        
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileUpload1.HasFile)
            {
                string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                string filePath = "D:/MOUFiles/";
                if (cnStr.ToLower().Contains("mumbai") == true)
                    filePath += "Mumbai/";
                else if (cnStr.ToLower().Contains("nashik") == true)
                    filePath += "Nashik/";
                else if (cnStr.ToLower().Contains("metro") == true)
                    filePath += "Metro/";
                else
                    filePath += "Pune/";

                string fileExtension = Path.GetExtension(fileUpload1.PostedFile.FileName).Substring(1);
                string partialName = lblSiteId.Text + "_";
                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(filePath);
                FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + partialName + "*");
                int siteFileCnt = filesInDir.Count();
                siteFileCnt++;
                filePath += lblSiteId.Text + "_" + siteFileCnt + "." + fileExtension;
                fileUpload1.SaveAs(filePath);
                lbltxt.Text = "File Uploaded Successfully";
                LoadMouFileList();
            }
        }
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (ddlFiles.SelectedIndex <= 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select file name..');", true);
                ddlFiles.Focus();
            }
            else
            {
                string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                string filePath = "D:/MOUFiles/";
                if (cnStr.ToLower().Contains("mumbai") == true)
                    filePath += "Mumbai/";
                else if (cnStr.ToLower().Contains("nashik") == true)
                    filePath += "Nashik/";
                else if (cnStr.ToLower().Contains("metro") == true)
                    filePath += "Metro/";
                else
                    filePath += "Pune/";
                filePath += ddlFiles.SelectedItem.Text;
                if (File.Exists(@filePath))
                {
                    HttpResponse res = HttpContext.Current.Response;
                    res.Clear();
                    res.AppendHeader("content-disposition", "attachment; filename=" + filePath);
                    res.ContentType = "application/octet-stream";
                    res.WriteFile(filePath);
                    res.Flush();
                    res.End();
                }
            }
        }

        protected void btnDeleteDuplicate_Click(object sender, EventArgs e)
        {
            bool deleteFlag = false;
            var siteRate = dc.SiteWiseRate_View_Duplicate(0, 0, 0);
            foreach (var sr in siteRate)
            {
                int cnt = 0;
                var rateSiteWise = dc.SiteWiseRate_View_Duplicate(sr.SITERATE_Site_Id, sr.SITERATE_Test_Id, 0);
                foreach (var rate in rateSiteWise)
                {
                    if (cnt == 0)
                    {
                        dc.SiteWiseRate_View_Duplicate(sr.SITERATE_Site_Id, sr.SITERATE_Test_Id, rate.SITERATE_Id);
                        deleteFlag = true;
                        cnt++;
                    }
                }
            }
            if (deleteFlag == true)
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Duplicates deleted..');", true);
        }

        // Zip all files from folder
        //protected void btnDownload_Click(object sender, EventArgs e)
        //{
        //    using (ZipFile zip = new ZipFile())
        //    {
        //        foreach (DropDownList gvrow in DropDownList1.Items)
        //        {

        //                string fileName = gvrow.Items[1].Text;
        //                string filePath = Server.MapPath("~/files/" + fileName);
        //                zip.AddFile(filePath, "files");

        //        }
        //        Response.Clear();
        //        Response.AddHeader("Content-Disposition", "attachment; filename=DownloadedFile.zip");
        //        Response.ContentType = "application/zip";
        //        zip.Save(Response.OutputStream);
        //        Response.End();
        //    }
        //}


    }
}
