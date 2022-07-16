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
    public partial class Enquiry_List : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Enquiry list";
                LoadInwardType();
                //LoadEnquiryList();
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
            //ddlInwardType.DataValueField = "MATERIAL_RecordType_var";
            var inwd = dc.Material_View("", "");
            ddlInwardType.DataSource = inwd;
            ddlInwardType.DataBind();
            ddlInwardType.Items.Insert(0, new ListItem("---Select---","0"));
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
            dt.Columns.Add(new DataColumn("ENQ_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("MATERIAL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_OpenEnquiryStatus_var", typeof(string)));

            //var enq = dc.Enquiry_View(0, 1, 0);
            var enq = dc.Enquiry_View(0, 1, Convert.ToInt32(ddlInwardType.SelectedValue));
            foreach (var c in enq)
            {
                bool valid = false;
                if (c.ENQ_OpenEnquiryStatus_var == "To be Collected")
                {
                    if (c.ENQ_CollectedOn_dt == null)
                    {
                        valid = true;
                    }
                }
                if (valid == false)
                {
                    dr = dt.NewRow();
                    dr["ENQ_Id"] = c.ENQ_Id.ToString();
                    dr["CL_Id"] = c.ENQ_CL_Id.ToString();
                    dr["SITE_Id"] = c.ENQ_SITE_Id.ToString();
                    dr["CONT_Id"] = c.ENQ_CONT_Id.ToString();
                    dr["ENQ_Date_dt"] = Convert.ToDateTime(c.ENQ_Date_dt).ToString("dd/MM/yyyy");
                    dr["MATERIAL_Name_var"] = Convert.ToString(c.MATERIAL_Name_var);
                    dr["CL_Name_var"] = Convert.ToString(c.CL_Name_var);
                    dr["SITE_Name_var"] = Convert.ToString(c.SITE_Name_var);
                    dr["ENQ_OpenEnquiryStatus_var"] = Convert.ToString(c.ENQ_OpenEnquiryStatus_var);
                    dt.Rows.Add(dr);
                }
            }
            grdEnquiry.DataSource = dt;
            grdEnquiry.DataBind();

            bool inwardRight = false;
            var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
            foreach (var u in user)
            {
                if (u.USER_Inward_right_bit == true)
                    inwardRight = true;
            }
            if (grdEnquiry.Rows.Count > 0)
            {
                for (int i = 0; i < grdEnquiry.Rows.Count; i++)
                {
                    ImageButton imgEditEnquiry = (ImageButton)grdEnquiry.Rows[i].FindControl("imgEditEnquiry");
                    LinkButton btnInward = (LinkButton)grdEnquiry.Rows[i].FindControl("btnInward");
                    btnInward.Enabled = false;
                    imgEditEnquiry.Enabled = false;
                    if (inwardRight == true)
                    {
                        btnInward.Enabled = true;
                        imgEditEnquiry.Enabled = true;
                    }
                }
            }
        }

        private void FirstGridViewRowOfEnquiry()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("ENQ_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("MATERIAL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_OpenEnquiryStatus_var", typeof(string)));
            dr = dt.NewRow();
            dr["ENQ_Id"] = string.Empty;
            dr["CL_Id"] = string.Empty;
            dr["SITE_Id"] = string.Empty;
            dr["CONT_Id"] = string.Empty;
            dr["ENQ_Date_dt"] = string.Empty;
            dr["MATERIAL_Name_var"] = string.Empty;
            dr["CL_Name_var"] = string.Empty;
            dr["SITE_Name_var"] = string.Empty;
            dr["ENQ_OpenEnquiryStatus_var"] = string.Empty;
            dt.Rows.Add(dr);
            grdEnquiry.DataSource = dt;
            grdEnquiry.DataBind();

        }
   
        protected void imgInsertEnquiry_Click(object sender, CommandEventArgs e)
        {
            //string strURLWithData = "Enquiry.aspx?" + obj.Encrypt(string.Format("EnqNo={0}&ClientId={1}&SiteId={2}&ContId={3}", "", "", "", ""));
            string strURLWithData = "EnquiryProposal.aspx?" + obj.Encrypt(string.Format("EnqNo={0}&ClientId={1}&SiteId={2}&ContId={3}", "", "", "", ""));
            Response.Redirect(strURLWithData);
        }

        protected void imgEditEnquiry_Click(object sender, CommandEventArgs e)
        {
            string Idsplit = Convert.ToString(e.CommandArgument);
            string[] arg = new string[4];
            arg = Idsplit.Split(';');
            if (Convert.ToString(arg[0]) != "")
            {
                //string strURLWithData = "Enquiry.aspx?" + obj.Encrypt(string.Format("EnqNo={0}&ClientId={1}&SiteId={2}&ContId={3}", Convert.ToString(arg[0]), Convert.ToInt32(arg[1]), Convert.ToInt32(arg[2]), Convert.ToInt32(arg[3])));
                string strURLWithData = "EnquiryProposal.aspx?" + obj.Encrypt(string.Format("EnqNo={0}&ClientId={1}&SiteId={2}&ContId={3}", Convert.ToString(arg[0]), Convert.ToInt32(arg[1]), Convert.ToInt32(arg[2]), Convert.ToInt32(arg[3])));
                Response.Redirect(strURLWithData);
            }
        }
   
        protected void lnkAddInward(object sender, CommandEventArgs e)
        {
            

            return;
            //string[] arg = new string[2];
            //arg = e.CommandArgument.ToString().Split(';');
            //string SelectedInward = arg[1];
            //////clsData obj1 = new clsData();
            //////DataTable dt = obj1.getGeneralData("SELECT * FROM tbl_Bill where BILL_ApproveStatus_bit = 0 and BILL_Date_dt < '" + DateTime.Now + "'");
            ////var bill = dc.Bill_View_ApprPending(DateTime.Now);
            ////if (bill.Count() > 0 )
            ////{
            ////    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Previous bills are pending for approval. Can not add inward." + "');", true);
            ////}
            ////check inward already added for selected enquiry or not
            //if (arg[0] != "")
            //{
            //    clsData clsDt = new clsData();
            //    var enq = dc.Enquiry_View(Convert.ToInt32(arg[0]), -1, 0).ToList(); 
            //    var inwd = dc.Inward_View_Enquiry(Convert.ToInt32(arg[0])).ToList();
            //    if (inwd.Count() > 0)
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Inward already added for selected enquiry. Can not add inward." + "');", true);
            //    }
            //    else if (clsDt.checkGSTInfoUpdated(enq.FirstOrDefault().ENQ_CL_Id.ToString(), enq.FirstOrDefault().ENQ_SITE_Id.ToString(), enq.FirstOrDefault().MATERIAL_RecordType_var) == false)
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Please update client & site GST details. Can not add inward." + "');", true);
            //    }
            //    else if (SelectedInward == "AAC Block Testing")
            //    {
            //        string strURLWithData = "AAC_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Cube Testing")
            //    {
            //        string strURLWithData = "Cube_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Steel Testing")
            //    {
            //        string strURLWithData = "Steel_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Aggregate Testing")
            //    {
            //        string strURLWithData = "Aggregate_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Other Testing")
            //    {
            //        string strURLWithData = "Other_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Pile Testing")
            //    {
            //        string strURLWithData = "Pile_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Pavement Block Testing")
            //    {
            //        string strURLWithData = "Pavement_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Tile Testing")
            //    {
            //        string strURLWithData = "Tile_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Steel Chemical Testing")
            //    {
            //        string strURLWithData = "STC_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Core Testing")
            //    {
            //        string strURLWithData = "Core_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Non Destructive Testing")
            //    {
            //        string strURLWithData = "NDT_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Brick Testing")
            //    {
            //        string strURLWithData = "Brick_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Water Testing")
            //    {
            //        string strURLWithData = "Water_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Cement Chemical Testing")
            //    {
            //        string strURLWithData = "CementChemical_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Cement Testing")
            //    {
            //        string strURLWithData = "Cement_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Cube Testing")
            //    {
            //        string strURLWithData = "Cube_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Fly Ash Testing")
            //    {
            //        string strURLWithData = "Flyash_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Masonary Block Testing")
            //    {
            //        string strURLWithData = "Solid_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Soil Testing")
            //    {
            //        string strURLWithData = "Soil_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Soil Investigation")
            //    {
            //        Session["RABill"] = null;
            //        string strURLWithData = "SoilInvestigation_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Rain Water Harvesting")
            //    {
            //        string strURLWithData = "RWH_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Core Cutting")
            //    {
            //        string strURLWithData = "CoreCutting_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
            //    }
            //    else if (SelectedInward == "Mix Design")
            //    {
            //        string strURLWithData = "MixDesign_Inward.aspx?" + obj.Encrypt(string.Format("RecType1={0}&RecordNo={1}&RefNo={2}&EnqNo={3}&InwdSt={4}", "", "", "", Convert.ToString(arg[0]), "Add"));
            //        Response.Redirect(strURLWithData);
             //   }
          //  }
        }
        protected void grdEnquiry_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblClientId = ((Label)e.Row.FindControl("lblEnquiryId"));
                if (lblClientId.Text == "0")
                {
                    ((ImageButton)e.Row.FindControl("imgEditEnquiry")).Visible = false;
                    ((LinkButton)e.Row.FindControl("btnInward")).Visible = false;//latest
                }
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
            if (ddlInwardType.SelectedIndex > 0)
            {
                LoadEnquiryList();
            }
        }
    }
}
