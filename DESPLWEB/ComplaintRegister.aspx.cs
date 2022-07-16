using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ComplaintRegister : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                 lblLoginId.Text = "0";
                if (Session["LoginId"] != null)
                    lblLoginId.Text = Convert.ToString(Session["LoginId"]);
              
           
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Complaint Register";
                LoadInwardList(); LoadUserList();

                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                string strReq = "";
                strReq = Request.RawUrl;
                if(strReq.Contains("?"))
                    strReq = strReq.Substring(strReq.IndexOf('?') + 1);
                else
                    strReq = "";
                if (!strReq.Equals(""))
                {
                    strReq = obj.Decrypt(strReq);
                    string[] arrMsg;
                    arrMsg =  strReq.Split('=');
                    string compId = arrMsg[1].ToString().Trim();
                    ModifyComplaint(compId);
                    lblheading.Text = "Complaint Register - Modify";
              
                }
                else
                {
                    lblCompId.Text = "0";
                    txt_date.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    getComplaint();
                }
            }
        }

        private void ModifyComplaint(string compNo)
        {
            var reslt = dc.ComplaintRegister_View(null,null,Convert.ToInt32(compNo));
            foreach (var item in reslt)
            {
                lblCompId.Text = item.COMP_Id.ToString();
                txt_ComplaintNo.Text = item.COMP_Id.ToString();
                txt_date.Text = Convert.ToDateTime(item.COMP_date_dt).ToString("dd/MM/yyyy");
                txt_Client.Text = item.CL_Name_var.ToString();
                lblClientId.Text = item.COMP_CL_Id.ToString();
                txt_Site.Text = item.SITE_Name_var.ToString();
                lblSiteId.Text = item.COMP_SITE_Id.ToString();
                txt_ClRepresentative.Text = item.COMP_CLRepresentative_var.ToString();
                ddl_ComplaintType.SelectedValue = item.COMP_ComplaintType_int.ToString(); 
                txt_DetailsOfComplaint.Text = item.COMP_DetailsOfComplaint_var.ToString();
                ddl_CompAttendedBy.SelectedValue = item.COMP_AttendedBy_int.ToString();
                ddl_RecordType.SelectedValue = item.COMP_MaterialId_int.ToString();
                txt_RefenceNo.Text = item.COMP_ReferenceNo_int.ToString();
                txt_CorrAction.Text = item.COMP_CorrAction_var.ToString();
                ddl_ActionStatus.SelectedValue = Convert.ToInt32(item.COMP_Status_bit).ToString();
                if (ddl_ActionStatus.SelectedValue.ToString() == "1")
                {
                    lblActionTime.Visible = true;
                    txt_ActionTime.Visible = true;
                    txt_ActionTime.Text = item.COMP_ActionTime_var.ToString();
                }
                txt_ComplaintFormRef.Text = item.COMP_ComplaintFormRef_var.ToString();
                txt_ClosureDate.Text = Convert.ToDateTime(item.COMP_ClosuerDate_dt).ToString("dd/MM/yyyy");
                txt_TechOfficerComment.Text = item.COMP_TechOfficerComment_var.ToString();
                ddl_ActionBy.SelectedValue = item.COMP_ActionBy_int.ToString();
                if (Convert.ToInt32(item.COMP_ReviewdByMD_bit) == 0)
                    cb_ReviewedBy.Checked = false;
                else
                    cb_ReviewedBy.Checked = true;

            }
        }

        private void getComplaint()
        {
            clsData obj = new clsData();
            int enqNo = obj.getComplaintId();
            if (enqNo > 0)
            {
                enqNo += 1;
                txt_ComplaintNo.Text = enqNo.ToString();
            }
            else
                txt_ComplaintNo.Text = "1";
        }
        private void LoadInwardList()
        {
            var inwd = dc.Material_View("", "");
            ddl_RecordType.DataTextField = "MATERIAL_Name_var";
            ddl_RecordType.DataValueField = "MATERIAL_Id";
            ddl_RecordType.DataSource = inwd;
            ddl_RecordType.DataBind();
            ddl_RecordType.Items.Insert(0, "---Select---");
        }
        private void LoadUserList()
        {
            ddl_ActionBy.DataTextField = "USER_Name_var";
            ddl_ActionBy.DataValueField = "USER_Id";
            var user = dc.User_View(0, 0, "", "", "");
            ddl_ActionBy.DataSource = user;
            ddl_ActionBy.DataBind();
            ddl_ActionBy.Items.Insert(0, "---Select---");


            ddl_CompAttendedBy.DataTextField = "USER_Name_var";
            ddl_CompAttendedBy.DataValueField = "USER_Id";
            ddl_CompAttendedBy.DataSource = user;
            ddl_CompAttendedBy.DataBind();
            ddl_CompAttendedBy.Items.Insert(0, "---Select---");
        }

        protected void LnkBtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                bool status = false;
                if (ddl_ActionStatus.SelectedValue.ToString() == "1")
                    status = true;
                DateTime? ClosureDate = null;
                DateTime? ComplaintDate = null;
                ClosureDate = DateTime.ParseExact(txt_ClosureDate.Text, "dd/MM/yyyy", null);
                ComplaintDate = DateTime.ParseExact(txt_date.Text, "dd/MM/yyyy", null);
                if (lblCompId.Text == "0")//insert new record
                    dc.ComplaintRegister_Update(0, Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblSiteId.Text), ComplaintDate, txt_ClRepresentative.Text, Convert.ToInt32(ddl_ComplaintType.SelectedValue), txt_DetailsOfComplaint.Text, Convert.ToInt32(ddl_CompAttendedBy.SelectedValue), Convert.ToInt32(ddl_RecordType.SelectedValue), Convert.ToInt32(txt_RefenceNo.Text), txt_CorrAction.Text, Convert.ToBoolean(status), txt_ActionTime.Text, txt_ComplaintFormRef.Text, ClosureDate, txt_TechOfficerComment.Text, Convert.ToInt32(ddl_ActionBy.SelectedValue), Convert.ToBoolean(cb_ReviewedBy.Checked), Convert.ToInt32(lblLoginId.Text), 0, null);
                else
                    dc.ComplaintRegister_Update(Convert.ToInt32(lblCompId.Text), Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblSiteId.Text), ComplaintDate, txt_ClRepresentative.Text, Convert.ToInt32(ddl_ComplaintType.SelectedValue), txt_DetailsOfComplaint.Text, Convert.ToInt32(ddl_CompAttendedBy.SelectedValue), Convert.ToInt32(ddl_RecordType.SelectedValue), Convert.ToInt32(txt_RefenceNo.Text), txt_CorrAction.Text, Convert.ToBoolean(status), txt_ActionTime.Text, txt_ComplaintFormRef.Text, ClosureDate, txt_TechOfficerComment.Text, Convert.ToInt32(ddl_ActionBy.SelectedValue), Convert.ToBoolean(cb_ReviewedBy.Checked), 0, Convert.ToInt32(lblLoginId.Text), DateTime.Now);

                lblMsg.Text = "Record Saved Successfully.";
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Visible = true;
                LnkBtnSave.Visible = false; Cleartextbox();
            }
        }

        private Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            Boolean valid = true;

          
            if (txt_Client.Text == "")
            {
                lblMsg.Text = "Please Select Client from list";
                txt_Client.Text = "";
                txt_Client.Focus();
                valid = false;
            }
            else if (Convert.ToInt32(lblClientId.Text) == 0)
            {
                lblMsg.Text = "Please Select Client from list";
                txt_Client.Text = "";
                txt_Client.Focus();
                valid = false;
            }
            else if (txt_Site.Text == "")
            {
                lblMsg.Text = "Please Select Site from list";
                txt_Site.Text = "";
                txt_Site.Focus();
                valid = false;
            }
            else if (Convert.ToInt32(lblSiteId.Text) == 0)
            {
                lblMsg.Text = "Please Select Site from list";
                txt_Site.Text = "";
                txt_Site.Focus();
                valid = false;
            }
            else if (txt_ClRepresentative.Text == "")
            {
                lblMsg.Text = "Please enter Client Representative";
                txt_ClRepresentative.Text = "";
                txt_ClRepresentative.Focus();
                valid = false;
            }
            else if (ddl_ComplaintType.SelectedValue == "---Select---")
            {
                lblMsg.Text = "Please select Type of Complaint";
                ddl_ComplaintType.Focus();
                valid = false;
            }
            else if (txt_DetailsOfComplaint.Text == "")
            {
                lblMsg.Text = "Please enter Details Of Complaint";
                txt_DetailsOfComplaint.Text = "";
                txt_DetailsOfComplaint.Focus();
                valid = false;
            }
            else if (ddl_CompAttendedBy.SelectedValue == "---Select---")
            {
                lblMsg.Text = "Please Select Complaint Attended By";
                ddl_CompAttendedBy.Focus();
                valid = false;
            }
            else if (ddl_RecordType.SelectedValue == "---Select---")
            {
                lblMsg.Text = "Please Select Inward Type";
                ddl_RecordType.Focus();
                valid = false;
            }
            else if (txt_RefenceNo.Text == "")
            {
                lblMsg.Text = "Please enter Reference No";
                txt_RefenceNo.Text = "";
                txt_RefenceNo.Focus();
                valid = false;
            }
            else if (txt_CorrAction.Text == "")
            {
                    lblMsg.Text = "Please enter Corrective Action Initiated";
                    txt_CorrAction.Text = "";
                    txt_CorrAction.Focus();
                    valid = false;                
            }
            else if(ddl_ActionStatus.SelectedValue=="1" && txt_ActionTime.Text=="")
            {
                lblMsg.Text = "Please enter Time Taken For Action";
                txt_ActionTime.Text = "";
                txt_ActionTime.Focus();
                valid = false;  
            }
            else if (txt_ComplaintFormRef.Text=="")
            {
                lblMsg.Text = "Please enter Complaint Form Ref";
                txt_ComplaintFormRef.Text = "";
                txt_ComplaintFormRef.Focus();
                valid = false; 
            }
            else if (txt_ClosureDate.Text == "")
            {
                lblMsg.Text = "Please enter Closure Date";
                txt_ClosureDate.Text = "";
                txt_ClosureDate.Focus();
                valid = false; 
            }
            else if (txt_TechOfficerComment.Text == "")
            {
                lblMsg.Text = "Please enter Comment of Technical Officer";
                txt_TechOfficerComment.Text = "";
                txt_TechOfficerComment.Focus();
                valid = false;
            }
            else if (ddl_ActionBy.SelectedValue == "---Select---")
            {
                lblMsg.Text = "Please Select Action By";
                ddl_ActionBy.Focus();
                valid = false;
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
    
        private void Cleartextbox()
        {
            txt_Client.Text = string.Empty;
            txt_RefenceNo.Text = string.Empty;
            lblActionTime.Visible = false;
            txt_ActionTime.Visible = false;
            txt_Site.Text = string.Empty;
            txt_ActionTime.Text = string.Empty;
            txt_TechOfficerComment.Text = string.Empty;
            txt_CorrAction.Text = string.Empty;
            txt_ComplaintNo.Text = string.Empty;
            txt_ComplaintFormRef.Text = string.Empty;
            txt_DetailsOfComplaint.Text = string.Empty;
            txt_ClRepresentative.Text = string.Empty;
            ddl_ActionStatus.SelectedIndex=0;
            txt_ClosureDate.Text = string.Empty;
            cb_ReviewedBy.Checked = false;
            ddl_ComplaintType.SelectedIndex = 0;
            getComplaint();
            ddl_ActionBy.SelectedIndex = 0;
            ddl_CompAttendedBy.SelectedIndex = 0;
            ddl_RecordType.SelectedIndex = 0;
        }

    
        protected void LnkExit_Click(object sender, EventArgs e)
        {
           // Response.Redirect("Home.aspx");
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            txt_Site.Text = string.Empty;
            LnkBtnSave.Visible = true;
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    Session["CL_ID"] = Convert.ToInt32(Request.Form[hfClientId.UniqueID]);
                    lblClientId.Text = Convert.ToString(Request.Form[hfClientId.UniqueID]);
                }
                else
                {
                    Session["CL_ID"] = 0;
                    lblClientId.Text = "0";
                }
                txt_Site.Focus();

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
                            lblSiteId.Text = Convert.ToString(Request.Form[hfSiteId.UniqueID]);
                            Session["SITE_ID"] = Convert.ToInt32(Request.Form[hfSiteId.UniqueID]);
                        }
                        else
                        {
                            lblSiteId.Text = "0"; 
                            Session["SITE_ID"] = "0";
                        }
                        txt_ClRepresentative.Focus();
                    }
                }
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
                Session["CL_ID"] = 0;
                Session["SITE_ID"] = 0; 
                lblClientId.Text = "0";
                lblSiteId.Text = "0";

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
                lblSiteId.Text = "0"; Session["SITE_ID"] = 0; 
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }

        protected void ddl_ActionStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_ActionStatus.SelectedValue.ToString() == "1")
            {
                lblActionTime.Visible = true;
                txt_ActionTime.Visible = true;
                txt_ActionTime.Text = "";
            }
            else
            {
                lblActionTime.Visible = false;
                txt_ActionTime.Visible = false;
                txt_ActionTime.Text = "";
            }
        }


    }
}