using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace DESPLWEB
{
    public partial class EnquiryVerifyClient : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Enquiry list to verify client";
                LoadInwardType();
            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        private void LoadInwardType()
        {
            ddlInwardType.DataTextField = "MATERIAL_Name_var";
            ddlInwardType.DataValueField = "MATERIAL_Id";
            var inwd = dc.Material_View("", "");
            ddlInwardType.DataSource = inwd;
            ddlInwardType.DataBind();
            ddlInwardType.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        private void LoadEnquiryList()
        {
            BindgrdRowOfEnquiry();
            if (grdEnquiry.Rows.Count <= 0)
            {
                FirstGridViewRowOfEnquiry();
            }
            else
            {
                lblTotalRecords.Text = "Total No of Records : " + grdEnquiry.Rows.Count;
            }
            grdEnquiry.Visible = true;
            lblTotalRecords.Visible = true;
        }
        private void BindgrdRowOfEnquiry()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("ENQNEW_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQNEW_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("MATERIAL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQNEW_ClientName_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQNEW_SiteName_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            var enq = dc.EnquiryNewClient_View(0, false, Convert.ToInt32(ddlInwardType.SelectedValue));
            foreach (var c in enq)
            {   
                dr = dt.NewRow();
                dr["ENQNEW_Id"] = c.ENQNEW_Id.ToString();
                dr["ENQNEW_Date_dt"] = Convert.ToDateTime(c.ENQNEW_Date_dt).ToString("dd/MM/yyyy");
                dr["MATERIAL_Name_var"] = Convert.ToString(c.MATERIAL_Name_var);
                dr["ENQNEW_ClientName_var"] = Convert.ToString(c.ENQNEW_ClientName_var);
                dr["ENQNEW_SiteName_var"] = Convert.ToString(c.ENQNEW_SiteName_var);
                dr["CL_Id"] = 0;
                dr["SITE_Id"] = 0;
                dt.Rows.Add(dr);
            }
            grdEnquiry.DataSource = dt;
            grdEnquiry.DataBind();
        }

        private void FirstGridViewRowOfEnquiry()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("ENQNEW_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQNEW_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("MATERIAL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQNEW_ClientName_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQNEW_SiteName_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dr = dt.NewRow();
            dr["ENQNEW_Id"] = string.Empty;
            dr["ENQNEW_Date_dt"] = string.Empty;
            dr["MATERIAL_Name_var"] = string.Empty;
            dr["ENQNEW_ClientName_var"] = string.Empty;
            dr["ENQNEW_SiteName_var"] = string.Empty;
            dr["CL_Id"] = string.Empty;
            dr["SITE_Id"] = string.Empty;
            dt.Rows.Add(dr);
            grdEnquiry.DataSource = dt;
            grdEnquiry.DataBind();

        }

        protected void lnkVerifyClient(object sender, CommandEventArgs e)
        {
            string Idsplit = Convert.ToString(e.CommandArgument);
            string[] arg = new string[4];
            arg = Idsplit.Split(';');
            if (Convert.ToString(arg[0]) != "")
            {
                string strURLWithData ="";
                if (chkLoadAppEnquiry.Checked == true)
                    strURLWithData = "EnquiryProposal.aspx?" + obj.Encrypt(string.Format("EnqNo={0}&EnqType={1}&ClientId={2}&SiteId={3}", Convert.ToString(arg[0]), "VerifyClientApp", Convert.ToInt32(arg[1]), Convert.ToInt32(arg[2])));
                //strURLWithData = "Enquiry.aspx?" + obj.Encrypt(string.Format("EnqNo={0}&EnqType={1}&ClientId={2}&SiteId={3}", Convert.ToString(arg[0]), "VerifyClientApp", Convert.ToInt32(arg[1]), Convert.ToInt32(arg[2])));
                else
                    strURLWithData = "EnquiryProposal.aspx?" + obj.Encrypt(string.Format("EnqNo={0}&EnqType={1}", Convert.ToString(arg[0]), "VerifyClient"));
                Response.Redirect(strURLWithData);
            }
        }

        protected void ddlInwardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdEnquiry.DataSource = null;
            grdEnquiry.DataBind();
            grdEnquiry.Visible = false;
            lblTotalRecords.Visible = false;
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            //if (ddlInwardType.SelectedIndex > 0)
            //{
                chkLoadAppEnquiry.Checked = false;
                LoadEnquiryList();
            //}
        }

        protected void chkLoadAppEnquiry_CheckedChanged(object sender, EventArgs e)
        {
            grdEnquiry.DataSource = null;
            grdEnquiry.DataBind();
            if (chkLoadAppEnquiry.Checked == true)
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("ENQNEW_Id", typeof(string)));
                dt.Columns.Add(new DataColumn("ENQNEW_Date_dt", typeof(string)));
                dt.Columns.Add(new DataColumn("MATERIAL_Name_var", typeof(string)));
                dt.Columns.Add(new DataColumn("ENQNEW_ClientName_var", typeof(string)));
                dt.Columns.Add(new DataColumn("ENQNEW_SiteName_var", typeof(string)));
                dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
                dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));

                var enq = dc.EnquiryApp_View(0, 0);
                foreach (var c in enq)
                {
                    dr = dt.NewRow();
                    dr["ENQNEW_Id"] = c.ENQ_Id.ToString();
                    dr["ENQNEW_Date_dt"] = Convert.ToDateTime(c.ENQ_Date_dt).ToString("dd/MM/yyyy");
                    //dr["MATERIAL_Name_var"] = Convert.ToString(c.MATERIAL_Name_var);
                    dr["MATERIAL_Name_var"] = "";
                    dr["ENQNEW_ClientName_var"] = Convert.ToString(c.CL_Name_var);
                    dr["ENQNEW_SiteName_var"] = Convert.ToString(c.SITE_Name_var);
                    dr["CL_Id"] = Convert.ToString(c.CL_Id);
                    dr["SITE_Id"] = Convert.ToString(c.SITE_Id);
                    dt.Rows.Add(dr);
                }
                grdEnquiry.DataSource = dt;
                grdEnquiry.DataBind();
                if (grdEnquiry.Rows.Count <= 0)
                {
                    FirstGridViewRowOfEnquiry();
                }
                else
                {
                    lblTotalRecords.Text = "Total No of Records : " + grdEnquiry.Rows.Count;
                }
                grdEnquiry.Visible = true;
                lblTotalRecords.Visible = true;
            }
        }
    }
}
