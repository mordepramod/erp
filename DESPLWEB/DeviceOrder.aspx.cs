using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class DeviceOrder : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Device Order";
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
            }

        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
        }
       protected void grdReportStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] arg = new string[2];
            arg = Convert.ToString(e.CommandArgument).Split(';');

            GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = grdrow.RowIndex;

            Label lblEnq_Id = (Label)grdReport.Rows[rowindex].FindControl("lblEnq_Id");
           

            if (e.CommandName == "ViewReport")
            {
                ModalPopupExtender1.Show();
                var result = dc.TestRequestDetails_View(2, Convert.ToInt32(arg[0]), Convert.ToInt32(arg[1]));//, Convert.ToInt32(arg[0]));

                grdDetails.DataSource = result;
                grdDetails.DataBind();
            }
            else if (e.CommandName == "EditEnquiry")
            {
                int clId = 0, siteId = 0;
                var result = dc.EnquiryApp_View(Convert.ToInt32(arg[0]),0).ToList();
                if (result.Count > 0)
                {
                    clId = Convert.ToInt32(result.FirstOrDefault().ENQ_CL_Id);
                    siteId = Convert.ToInt32(result.FirstOrDefault().ENQ_SITE_Id);

                    EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                    string strURLWithData = "EnquiryProposal.aspx?" + obj.Encrypt(string.Format("ENQ_Id={0}&EnqType={1}&ClientId={2}&SiteId={3}", arg[0], "VerifyClientApp", clId, siteId));
                    Response.Redirect(strURLWithData);
                }
            }
            else if (e.CommandName == "ModifyReport")
            {
                var result = dc.TestRequestDetails_View(2, Convert.ToInt32(arg[0]), Convert.ToInt32(arg[1]));
                int rowNo = 0;
               // Label lblEnq_Id = (Label)grdReport.Rows[rowindex].FindControl("lblEnq_Id");
                Label lblEnqDetails = (Label)grdReport.Rows[rowindex].FindControl("lblEnqDetails");
                if (Convert.ToString(arg[2]) == "Cube Testing")
                {
                    lblMsgCube.Visible = false;
                    if (lblEnq_Id.Text != "")
                        lblDetailCube.Text = "Enq No. : " + lblEnq_Id.Text + ", Client : " + grdReport.Rows[rowindex].Cells[3].Text + ", Site : " + grdReport.Rows[rowindex].Cells[4].Text + ", Material : " + grdReport.Rows[rowindex].Cells[5].Text;
                    else
                    {
                        string[] arr = lblEnqDetails.Text.Split(';');
                        lblDetailCube.Text = "Enq No. : " + arr[0] + ", Client : " + arr[1] + ", Site : " + arr[2] + ", Material : " + arr[3];
                    }
                    grdCube.DataSource = null;
                    grdCube.DataBind();

                    ModalPopupExtender3.Show();
                    foreach (var data in result)
                    {
                        AddRowCT();
                        Label lblTestRequestId = (Label)grdCube.Rows[rowNo].FindControl("lblTestId");
                        DropDownList ddlGrade = (DropDownList)grdCube.Rows[rowNo].FindControl("ddlGrade");
                        TextBox txtCastingDt = (TextBox)grdCube.Rows[rowNo].FindControl("txtCastingDt");
                        TextBox txtDescriptionWork = (TextBox)grdCube.Rows[rowNo].FindControl("txtDescriptionWork");
                        TextBox txtLoactnWork = (TextBox)grdCube.Rows[rowNo].FindControl("txtLoactnWork");
                        TextBox txtMake = (TextBox)grdCube.Rows[rowNo].FindControl("txtMake");
                        TextBox txtSpecimen = (TextBox)grdCube.Rows[rowNo].FindControl("txtSpecimen");
                        //DropDownList ddlTestingSchedule = (DropDownList)grdCube.Rows[rowNo].FindControl("ddlTestingSchedule");
                        TextBox txtTestingSchedule = (TextBox)grdCube.Rows[rowNo].FindControl("txtTestingSchedule");                        
                        TextBox txtIdmark = (TextBox)grdCube.Rows[rowNo].FindControl("txtIdmark");

                        lblTestRequestId.Text = data.id.ToString();
                        if (data.grade != "" || data.grade != null)
                            ddlGrade.SelectedValue = data.grade;
                        txtCastingDt.Text = data.casting_dt;
                        txtDescriptionWork.Text = data.description;
                        txtLoactnWork.Text = data.Location_of_pour;
                        txtMake.Text = data.make;
                        txtSpecimen.Text = data.no_of_specimen;
                        //if (data.schedule != "" || data.schedule != null)
                        //    ddlTestingSchedule.SelectedValue = data.schedule;
                        txtTestingSchedule.Text = data.schedule.ToString();
                        txtIdmark.Text = data.Idmark1;
                        rowNo++;
                    }

                }
                else if (Convert.ToString(arg[2]) == "Steel Testing")
                {
                    lblMsgSteel.Visible = false;
                    if (lblEnq_Id.Text != "")
                        lblDetailSteel.Text = "Enq No. : " + lblEnq_Id.Text + ", Client : " + grdReport.Rows[rowindex].Cells[3].Text + ", Site : " + grdReport.Rows[rowindex].Cells[4].Text + ", Material : " + grdReport.Rows[rowindex].Cells[5].Text;
                    else
                    {
                        string[] arr = lblEnqDetails.Text.Split(';');
                        lblDetailSteel.Text = "Enq No. : " + arr[0] + ", Client : " + arr[1] + ", Site : " + arr[2] + ", Material : " + arr[3];
                    }
                    grdSteel.DataSource = null;
                    grdSteel.DataBind();

                    ModalPopupExtender4.Show();
                    foreach (var data in result)
                    {
                        AddRowST();
                        Label lblTestRequestId = (Label)grdSteel.Rows[rowNo].FindControl("lblTestId");
                        //DropDownList ddlDiameter = (DropDownList)grdSteel.Rows[rowNo].FindControl("ddlDiameter");
                        TextBox txtDiameter = (TextBox)grdSteel.Rows[rowNo].FindControl("txtDiameter");
                        TextBox txtMake = (TextBox)grdSteel.Rows[rowNo].FindControl("txtMake");
                        TextBox txtSuppiler = (TextBox)grdSteel.Rows[rowNo].FindControl("txtSuppiler");
                        DropDownList ddlGrade = (DropDownList)grdSteel.Rows[rowNo].FindControl("ddlGrade");
                        TextBox txtDescription = (TextBox)grdSteel.Rows[rowNo].FindControl("txtDescription");
                        TextBox txtBars = (TextBox)grdSteel.Rows[rowNo].FindControl("txtBars");
                        TextBox txtIdmark = (TextBox)grdSteel.Rows[rowNo].FindControl("txtIdmark");

                        lblTestRequestId.Text = data.id.ToString();
                        //if (data.diameter != "" || data.diameter != null)
                        //    ddlDiameter.SelectedValue = data.diameter;
                        txtDiameter.Text = data.diameter.ToString();
                        txtMake.Text = data.make;
                        txtSuppiler.Text = data.supplier;
                        //if (data.grade != "" || data.grade != null)
                        //    ddlGrade.Text = data.grade;
                        if (data.specification != "" || data.specification != null)
                            ddlGrade.Text = data.specification;
                        txtDescription.Text = data.description;
                        txtBars.Text = data.count_r;
                        txtIdmark.Text = data.Idmark1;
                        rowNo++;
                    }
                }
                else if (Convert.ToString(arg[2]) == "Mix Design")
                {
                    if (lblEnq_Id.Text != "")
                        lblDetailMixDesign.Text = "Enq No. : " + lblEnq_Id.Text + ", Client : " + grdReport.Rows[rowindex].Cells[3].Text + ", Site : " + grdReport.Rows[rowindex].Cells[4].Text + ", Material : " + grdReport.Rows[rowindex].Cells[5].Text;
                    else
                    {
                        string[] arr = lblEnqDetails.Text.Split(';');
                        lblDetailMixDesign.Text = "Enq No. : " + arr[0] + ", Client : " + arr[1] + ", Site : " + arr[2] + ", Material : " + arr[3];
                    }
                    lblMsgMixDesign.Visible = false;
                    grdMixDesign.DataSource = null;
                    grdMixDesign.DataBind();

                    pnlMaterial.Visible = false;
                    grdMaterial.DataSource = null;
                    grdMaterial.DataBind();

                    ModalPopupExtender5.Show();
                    foreach (var data in result)
                    {
                        //AddRowCT();
                        AddRowMF();
                        Label lblTestRequestId = (Label)grdMixDesign.Rows[rowNo].FindControl("lblTestId");
                        //DropDownList ddlDesign = (DropDownList)grdMixDesign.Rows[rowNo].FindControl("ddlDesign");
                        TextBox txtDesign = (TextBox)grdMixDesign.Rows[rowNo].FindControl("txtDesign");
                        DropDownList ddlGrade = (DropDownList)grdMixDesign.Rows[rowNo].FindControl("ddlGrade");
                        TextBox txtMterialCombn = (TextBox)grdMixDesign.Rows[rowNo].FindControl("txtMterialCombn");
                        TextBox txtSlump = (TextBox)grdMixDesign.Rows[rowNo].FindControl("txtSlump");
                        //DropDownList ddlRetenstionPeriod = (DropDownList)grdMixDesign.Rows[rowNo].FindControl("ddlRetenstionPeriod");
                        TextBox txtRetenstionPeriod = (TextBox)grdMixDesign.Rows[rowNo].FindControl("txtRetenstionPeriod");
                        TextBox txtFlow = (TextBox)grdMixDesign.Rows[rowNo].FindControl("txtFlow");
                        TextBox txtNatureWork = (TextBox)grdMixDesign.Rows[rowNo].FindControl("txtNatureWork");

                        lblTestRequestId.Text = data.id.ToString();
                        //if (data.grade != "" || data.grade != null)
                        //    ddlDesign.SelectedValue = data.grade;
                        txtDesign.Text = data.material_type;
                        if (data.grade != "" || data.grade != null)
                            ddlGrade.SelectedValue = data.grade;
                        txtMterialCombn.Text = data.material_combination;
                        txtSlump.Text = data.slump;
                        //if (data.retention_period != "" || data.retention_period != null)
                        //    ddlRetenstionPeriod.SelectedValue = data.retention_period;
                        txtRetenstionPeriod.Text = data.retention_period.ToString();
                        txtFlow.Text = data.flow;
                        txtNatureWork.Text = data.Nature_of_work;
                        rowNo++;
                    }
                }
                else
                {
                    lblMsgCommon.Visible = false;
                    if (lblEnq_Id.Text != "")
                        lblDetailCommom.Text = "Enq No. : " + lblEnq_Id.Text + ", Client : " + grdReport.Rows[rowindex].Cells[3].Text + ", Site : " + grdReport.Rows[rowindex].Cells[4].Text + ", Material : " + grdReport.Rows[rowindex].Cells[5].Text;
                   else
                    {
                        string[] arr = lblEnqDetails.Text.Split(';');
                        lblDetailCommom.Text = "Enq No. : " + arr[0] + ", Client : " + arr[1] + ", Site : " + arr[2] + ", Material : " + arr[3];
                    }
                    ModalPopupExtender2.Show();
                    grdCommon.DataSource = result;
                    grdCommon.DataBind();
                }
            }
        }
        #region Cube Testing
        protected void AddRowCT()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CTTable"] != null)
            {
                GetCurrentDataCT();
                dt = (DataTable)ViewState["CTTable"];
            }
            else
            {
                
                dt.Columns.Add(new DataColumn("lblTestId", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlGrade", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCastingDt", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescriptionWork", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLoactnWork", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMake", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpecimen", typeof(string)));
                dt.Columns.Add(new DataColumn("txtTestingSchedule", typeof(string)));
                dt.Columns.Add(new DataColumn("txtIdmark", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblTestId"] = string.Empty;
            dr["ddlGrade"] = string.Empty;
            dr["txtCastingDt"] = string.Empty;
            dr["txtDescriptionWork"] = string.Empty;
            dr["txtLoactnWork"] = string.Empty;
            dr["txtMake"] = string.Empty;
            dr["txtSpecimen"] = string.Empty;
            dr["txtTestingSchedule"] = string.Empty;
            dr["txtIdmark"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CTTable"] = dt;
            grdCube.DataSource = dt;
            grdCube.DataBind();
            SetPreviousDataCT();
        }
        protected void SetPreviousDataCT()
        {
            DataTable dt = (DataTable)ViewState["CTTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblTestRequestId = (Label)grdCube.Rows[i].FindControl("lblTestId");
                DropDownList ddlGrade = (DropDownList)grdCube.Rows[i].FindControl("ddlGrade");
                TextBox txtCastingDt = (TextBox)grdCube.Rows[i].FindControl("txtCastingDt");
                TextBox txtDescriptionWork = (TextBox)grdCube.Rows[i].FindControl("txtDescriptionWork");
                TextBox txtLoactnWork = (TextBox)grdCube.Rows[i].FindControl("txtLoactnWork");
                TextBox txtMake = (TextBox)grdCube.Rows[i].FindControl("txtMake");
                TextBox txtSpecimen = (TextBox)grdCube.Rows[i].FindControl("txtSpecimen");
                //DropDownList ddlTestingSchedule = (DropDownList)grdCube.Rows[i].FindControl("ddlTestingSchedule");
                TextBox txtTestingSchedule = (TextBox)grdCube.Rows[i].FindControl("txtTestingSchedule");
                TextBox txtIdmark = (TextBox)grdCube.Rows[i].FindControl("txtIdmark");


                lblTestRequestId.Text = dt.Rows[i]["lblTestId"].ToString();
                ddlGrade.SelectedValue = dt.Rows[i]["ddlGrade"].ToString();
                txtCastingDt.Text = dt.Rows[i]["txtCastingDt"].ToString();
                txtDescriptionWork.Text = dt.Rows[i]["txtDescriptionWork"].ToString();
                txtLoactnWork.Text = dt.Rows[i]["txtLoactnWork"].ToString();
                txtMake.Text = dt.Rows[i]["txtMake"].ToString();
                txtSpecimen.Text = dt.Rows[i]["txtSpecimen"].ToString();
                txtTestingSchedule.Text = dt.Rows[i]["txtTestingSchedule"].ToString();
                txtIdmark.Text = dt.Rows[i]["txtIdmark"].ToString();
            }
        }
        protected void GetCurrentDataCT()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("lblTestId", typeof(string)));
            dt.Columns.Add(new DataColumn("ddlGrade", typeof(string)));
            dt.Columns.Add(new DataColumn("txtCastingDt", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDescriptionWork", typeof(string)));
            dt.Columns.Add(new DataColumn("txtLoactnWork", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMake", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpecimen", typeof(string)));
            dt.Columns.Add(new DataColumn("txtTestingSchedule", typeof(string)));
            dt.Columns.Add(new DataColumn("txtIdmark", typeof(string)));
            for (int i = 0; i < grdCube.Rows.Count; i++)
            {
                Label lblTestRequestId = (Label)grdCube.Rows[i].FindControl("lblTestId");
                DropDownList ddlGrade = (DropDownList)grdCube.Rows[i].FindControl("ddlGrade");
                TextBox txtCastingDt = (TextBox)grdCube.Rows[i].FindControl("txtCastingDt");
                TextBox txtDescriptionWork = (TextBox)grdCube.Rows[i].FindControl("txtDescriptionWork");
                TextBox txtLoactnWork = (TextBox)grdCube.Rows[i].FindControl("txtLoactnWork");
                TextBox txtMake = (TextBox)grdCube.Rows[i].FindControl("txtMake");
                TextBox txtSpecimen = (TextBox)grdCube.Rows[i].FindControl("txtSpecimen");
                TextBox txtTestingSchedule = (TextBox)grdCube.Rows[i].FindControl("txtTestingSchedule");
                TextBox txtIdmark = (TextBox)grdCube.Rows[i].FindControl("txtIdmark");

                dr = dt.NewRow();
                dr["lblTestId"] = lblTestRequestId.Text;
                dr["ddlGrade"] = ddlGrade.SelectedValue;
                dr["txtCastingDt"] = txtCastingDt.Text;
                dr["txtDescriptionWork"] = txtDescriptionWork.Text;
                dr["txtLoactnWork"] = txtLoactnWork.Text;
                dr["txtMake"] = txtMake.Text;
                dr["txtSpecimen"] = txtSpecimen.Text;
                dr["txtTestingSchedule"] = txtTestingSchedule.Text;
                dr["txtIdmark"] = txtIdmark.Text;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["CTTable"] = dt;
        }
        #endregion

        #region Steel Testing
        protected void AddRowST()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["STTable"] != null)
            {
                GetCurrentDataST();
                dt = (DataTable)ViewState["STTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblTestId", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDiameter", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMake", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSuppiler", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlGrade", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("txtBars", typeof(string)));
                dt.Columns.Add(new DataColumn("txtIdmark", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblTestId"] = string.Empty;
            dr["txtDiameter"] = string.Empty;
            dr["txtMake"] = string.Empty;
            dr["txtSuppiler"] = string.Empty;
            dr["ddlGrade"] = string.Empty;
            dr["txtDescription"] = string.Empty;
            dr["txtBars"] = string.Empty;
            dr["txtIdmark"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["STTable"] = dt;
            grdSteel.DataSource = dt;
            grdSteel.DataBind();
            SetPreviousDataST();
        }
        protected void SetPreviousDataST()
        {
            DataTable dt = (DataTable)ViewState["STTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblTestRequestId = (Label)grdSteel.Rows[i].FindControl("lblTestId");
                TextBox txtDiameter = (TextBox)grdSteel.Rows[i].FindControl("txtDiameter");
                TextBox txtMake = (TextBox)grdSteel.Rows[i].FindControl("txtMake");
                TextBox txtSuppiler = (TextBox)grdSteel.Rows[i].FindControl("txtSuppiler");
                DropDownList ddlGrade = (DropDownList)grdSteel.Rows[i].FindControl("ddlGrade");
                TextBox txtDescription = (TextBox)grdSteel.Rows[i].FindControl("txtDescription");
                TextBox txtBars = (TextBox)grdSteel.Rows[i].FindControl("txtBars");
                TextBox txtIdmark = (TextBox)grdSteel.Rows[i].FindControl("txtIdmark");


                lblTestRequestId.Text = dt.Rows[i]["lblTestId"].ToString();
                txtDiameter.Text = dt.Rows[i]["txtDiameter"].ToString();
                txtMake.Text = dt.Rows[i]["txtMake"].ToString();
                txtSuppiler.Text = dt.Rows[i]["txtSuppiler"].ToString();
                ddlGrade.Text = dt.Rows[i]["ddlGrade"].ToString();
                txtDescription.Text = dt.Rows[i]["txtDescription"].ToString();
                txtBars.Text = dt.Rows[i]["txtBars"].ToString();
                txtIdmark.Text = dt.Rows[i]["txtIdmark"].ToString();
            }
        }
        protected void GetCurrentDataST()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("lblTestId", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDiameter", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMake", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSuppiler", typeof(string)));
            dt.Columns.Add(new DataColumn("ddlGrade", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("txtBars", typeof(string)));
            dt.Columns.Add(new DataColumn("txtIdmark", typeof(string)));
            for (int i = 0; i < grdSteel.Rows.Count; i++)
            {
                Label lblTestRequestId = (Label)grdSteel.Rows[i].FindControl("lblTestId");
                TextBox txtDiameter = (TextBox)grdSteel.Rows[i].FindControl("txtDiameter");
                TextBox txtMake = (TextBox)grdSteel.Rows[i].FindControl("txtMake");
                TextBox txtSuppiler = (TextBox)grdSteel.Rows[i].FindControl("txtSuppiler");
                DropDownList ddlGrade = (DropDownList)grdSteel.Rows[i].FindControl("ddlGrade");
                TextBox txtDescription = (TextBox)grdSteel.Rows[i].FindControl("txtDescription");
                TextBox txtBars = (TextBox)grdSteel.Rows[i].FindControl("txtBars");
                TextBox txtIdmark = (TextBox)grdSteel.Rows[i].FindControl("txtIdmark");

                dr = dt.NewRow();
                dr["lblTestId"] = lblTestRequestId.Text;
                dr["txtDiameter"] = txtDiameter.Text;
                dr["txtMake"] = txtMake.Text;
                dr["txtSuppiler"] = txtSuppiler.Text;
                dr["ddlGrade"] = ddlGrade.Text;
                dr["txtDescription"] = txtDescription.Text;
                dr["txtBars"] = txtBars.Text;
                dr["txtIdmark"] = txtIdmark.Text;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["STTable"] = dt;
        }
        #endregion

        #region Mix Design
        protected void AddRowMF()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MFTable"] != null)
            {
                GetCurrentDataMF();
                dt = (DataTable)ViewState["MFTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblTestId", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDesign", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlGrade", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMterialCombn", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSlump", typeof(string)));
                dt.Columns.Add(new DataColumn("txtRetenstionPeriod", typeof(string)));
                dt.Columns.Add(new DataColumn("txtFlow", typeof(string)));
                dt.Columns.Add(new DataColumn("txtNatureWork", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblTestId"] = string.Empty;
            dr["txtDesign"] = string.Empty;
            dr["ddlGrade"] = string.Empty;
            dr["txtMterialCombn"] = string.Empty;
            dr["txtSlump"] = string.Empty;
            dr["txtRetenstionPeriod"] = string.Empty;
            dr["txtFlow"] = string.Empty;
            dr["txtNatureWork"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["MFTable"] = dt;
            grdMixDesign.DataSource = dt;
            grdMixDesign.DataBind();
            SetPreviousDataMF();
        }
        protected void SetPreviousDataMF()
        {
            DataTable dt = (DataTable)ViewState["MFTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblTestRequestId = (Label)grdMixDesign.Rows[i].FindControl("lblTestId");
                TextBox txtDesign = (TextBox)grdMixDesign.Rows[i].FindControl("txtDesign");
                DropDownList ddlGrade = (DropDownList)grdMixDesign.Rows[i].FindControl("ddlGrade");
                TextBox txtMterialCombn = (TextBox)grdMixDesign.Rows[i].FindControl("txtMterialCombn");
                TextBox txtSlump = (TextBox)grdMixDesign.Rows[i].FindControl("txtSlump");
                TextBox txtRetenstionPeriod = (TextBox)grdMixDesign.Rows[i].FindControl("txtRetenstionPeriod");
                TextBox txtFlow = (TextBox)grdMixDesign.Rows[i].FindControl("txtFlow");
                TextBox txtNatureWork = (TextBox)grdMixDesign.Rows[i].FindControl("txtNatureWork");
                
                lblTestRequestId.Text = dt.Rows[i]["lblTestId"].ToString();
                txtDesign.Text = dt.Rows[i]["txtDesign"].ToString();
                ddlGrade.Text = dt.Rows[i]["ddlGrade"].ToString();
                txtMterialCombn.Text = dt.Rows[i]["txtMterialCombn"].ToString();
                txtSlump.Text = dt.Rows[i]["txtSlump"].ToString();
                txtRetenstionPeriod.Text = dt.Rows[i]["txtRetenstionPeriod"].ToString();
                txtFlow.Text = dt.Rows[i]["txtFlow"].ToString();
                txtNatureWork.Text = dt.Rows[i]["txtNatureWork"].ToString();
            }
        }
        protected void GetCurrentDataMF()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("lblTestId", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDesign", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMterialCombn", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSlump", typeof(string)));
            dt.Columns.Add(new DataColumn("ddlGrade", typeof(string)));
            dt.Columns.Add(new DataColumn("txtRetenstionPeriod", typeof(string)));
            dt.Columns.Add(new DataColumn("txtFlow", typeof(string)));
            dt.Columns.Add(new DataColumn("txtNatureWork", typeof(string)));
            for (int i = 0; i < grdMixDesign.Rows.Count; i++)
            {
                Label lblTestRequestId = (Label)grdMixDesign.Rows[i].FindControl("lblTestId");
                TextBox txtDesign = (TextBox)grdMixDesign.Rows[i].FindControl("txtDesign");
                DropDownList ddlGrade = (DropDownList)grdMixDesign.Rows[i].FindControl("ddlGrade");
                TextBox txtMterialCombn = (TextBox)grdMixDesign.Rows[i].FindControl("txtMterialCombn");
                TextBox txtSlump = (TextBox)grdMixDesign.Rows[i].FindControl("txtSlump");
                TextBox txtRetenstionPeriod = (TextBox)grdMixDesign.Rows[i].FindControl("txtRetenstionPeriod");
                TextBox txtFlow = (TextBox)grdMixDesign.Rows[i].FindControl("txtFlow");
                TextBox txtNatureWork = (TextBox)grdMixDesign.Rows[i].FindControl("txtNatureWork");

                dr = dt.NewRow();
                dr["lblTestId"] = lblTestRequestId.Text;
                dr["txtDesign"] = txtDesign.Text;
                dr["ddlGrade"] = ddlGrade.Text;
                dr["txtMterialCombn"] = txtMterialCombn.Text;
                dr["txtSlump"] = txtSlump.Text;
                dr["txtRetenstionPeriod"] = txtRetenstionPeriod.Text;
                dr["txtFlow"] = txtFlow.Text;
                dr["txtNatureWork"] = txtNatureWork.Text;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["MFTable"] = dt;
        }
        #endregion
        protected void imgClosePopup1_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender2.Hide();
        }

        protected void imgClosePopup2_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender3.Hide();
        }
        protected void imgClosePopup3_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender4.Hide();
        }
        protected void imgClosePopup4_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender5.Hide();
        }
        protected void lnkUpdateCube_Click(object sender, EventArgs e)
        {
            if(grdCube.Rows.Count>0)
            {
                for (int rowNo = 0; rowNo < grdCube.Rows.Count; rowNo++)
                {
                    Label lblTestReq_Id = (Label)grdCube.Rows[rowNo].FindControl("lblTestId");
                    DropDownList ddlGrade = (DropDownList)grdCube.Rows[rowNo].FindControl("ddlGrade");
                    TextBox txtCastingDt = (TextBox)grdCube.Rows[rowNo].FindControl("txtCastingDt");
                    TextBox txtDescriptionWork = (TextBox)grdCube.Rows[rowNo].FindControl("txtDescriptionWork");
                    TextBox txtLoactnWork = (TextBox)grdCube.Rows[rowNo].FindControl("txtLoactnWork");
                    TextBox txtMake = (TextBox)grdCube.Rows[rowNo].FindControl("txtMake");
                    TextBox txtSpecimen = (TextBox)grdCube.Rows[rowNo].FindControl("txtSpecimen");
                    TextBox txtTestingSchedule = (TextBox)grdCube.Rows[rowNo].FindControl("txtTestingSchedule");
                    TextBox txtIdmark = (TextBox)grdCube.Rows[rowNo].FindControl("txtIdmark");

                    dc.TestRequestDetails_Update(0, Convert.ToInt32(lblTestReq_Id.Text), ddlGrade.SelectedItem.Text, txtDescriptionWork.Text, "", txtSpecimen.Text, txtMake.Text
                        , txtIdmark.Text, txtCastingDt.Text, txtTestingSchedule.Text, "", "", "", txtSpecimen.Text, "", "", "", "", txtLoactnWork.Text, "","");
                }
                lblMsgCube.Visible = true;
            }
        }
      
        protected void lnkUpdateSteel_Click(object sender, EventArgs e)
        {
            if (grdSteel.Rows.Count > 0)
            {
                for (int rowNo = 0; rowNo < grdSteel.Rows.Count; rowNo++)
                {
                    Label lblTestReq_Id = (Label)grdSteel.Rows[rowNo].FindControl("lblTestId");
                    TextBox txtDiameter = (TextBox)grdSteel.Rows[rowNo].FindControl("txtDiameter");
                    TextBox txtMake = (TextBox)grdSteel.Rows[rowNo].FindControl("txtMake");
                    TextBox txtSuppiler = (TextBox)grdSteel.Rows[rowNo].FindControl("txtSuppiler");
                    DropDownList ddlGrade = (DropDownList)grdSteel.Rows[rowNo].FindControl("ddlGrade");
                    TextBox txtDescription = (TextBox)grdSteel.Rows[rowNo].FindControl("txtDescription");
                    TextBox txtBars = (TextBox)grdSteel.Rows[rowNo].FindControl("txtBars");
                    TextBox txtIdmark = (TextBox)grdSteel.Rows[rowNo].FindControl("txtIdmark");

                    dc.TestRequestDetails_Update(0, Convert.ToInt32(lblTestReq_Id.Text),"", txtDescription.Text, txtDiameter.Text,""
                        ,txtMake.Text, txtIdmark.Text,"", "", "",txtBars.Text, ddlGrade.SelectedItem.Text, "", "", "", "", "", "", txtSuppiler.Text,"");
                }
                lblMsgSteel.Visible = true;
            }

        }
        protected void lnkUpdateMixDesign_Click(object sender, EventArgs e)
        {
            if (grdMixDesign.Rows.Count > 0)
            {
                for (int rowNo = 0; rowNo < grdMixDesign.Rows.Count; rowNo++)
                {
                    Label lblTestReq_Id = (Label)grdMixDesign.Rows[rowNo].FindControl("lblTestId");
                    TextBox txtDesign = (TextBox)grdMixDesign.Rows[rowNo].FindControl("txtDesign");
                    DropDownList ddlGrade = (DropDownList)grdMixDesign.Rows[rowNo].FindControl("ddlGrade");
                    TextBox txtMterialCombn = (TextBox)grdMixDesign.Rows[rowNo].FindControl("txtMterialCombn");
                    TextBox txtSlump = (TextBox)grdMixDesign.Rows[rowNo].FindControl("txtSlump");
                    TextBox txtRetenstionPeriod = (TextBox)grdMixDesign.Rows[rowNo].FindControl("txtRetenstionPeriod");
                    TextBox txtFlow = (TextBox)grdMixDesign.Rows[rowNo].FindControl("txtFlow");
                    TextBox txtNatureWork = (TextBox)grdMixDesign.Rows[rowNo].FindControl("txtNatureWork");

                    dc.TestRequestDetails_Update(0, Convert.ToInt32(lblTestReq_Id.Text), ddlGrade.SelectedItem.Text, "", "", ""
                        , "", "", "", "", txtNatureWork.Text, "", "", "", txtFlow.Text, txtMterialCombn.Text, txtRetenstionPeriod.Text, txtSlump.Text, "", "", txtDesign.Text);
                }
                lblMsgMixDesign.Visible = true;
            }
        }

        protected void lnkUpdateCommon_Click(object sender, EventArgs e)
        {
            if (grdCommon.Rows.Count > 0)
            {
                for (int rowNo = 0; rowNo < grdCommon.Rows.Count; rowNo++)
                {
                    Label lblTestReq_Id = (Label)grdCommon.Rows[rowNo].FindControl("lblTestId");
                    TextBox txtMake = (TextBox)grdCommon.Rows[rowNo].FindControl("txtMake");
                    TextBox txtSupplier = (TextBox)grdCommon.Rows[rowNo].FindControl("txtSupplier");
                    TextBox txtgrade = (TextBox)grdCommon.Rows[rowNo].FindControl("txtgrade");
                    TextBox txtDescription = (TextBox)grdCommon.Rows[rowNo].FindControl("txtDescription");
                    TextBox txtSpecimen = (TextBox)grdCommon.Rows[rowNo].FindControl("txtSpecimen");
                    TextBox txtIdmark = (TextBox)grdCommon.Rows[rowNo].FindControl("txtIdmark");
                    dc.TestRequestDetails_Update(0, Convert.ToInt32(lblTestReq_Id.Text), "", txtDescription.Text, "", txtSpecimen.Text
                        , txtMake.Text, txtIdmark.Text, "", "", "", "", txtgrade.Text, "", "", "", "", "", "", txtSupplier.Text,"");

                }
                lblMsgCommon.Visible = true;
            }
        }
        protected void grdMixDesign_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string arg = e.CommandArgument.ToString();

            GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = grdrow.RowIndex;
            Label lblTestReq_Id = (Label)grdMixDesign.Rows[rowindex].FindControl("lblTestId");
            if (e.CommandName == "ViewMaterial")
            {
                pnlMaterial.Visible = true;
                var result = dc.mix_design_material_View(Convert.ToInt32(lblTestReq_Id.Text));
                grdMaterial.DataSource = result;
                grdMaterial.DataBind();
            }
            
            
        }
        public void clearGrid()
        {
            grdReport.DataSource = null;
            grdReport.DataBind();
            lblTotalRecords.Text = "Total No of Records : 0";
        }
        protected void rbEnqWithLogin_CheckedChanged(object sender, EventArgs e)
        {
            clearGrid();
        }

        protected void rbEnqWithoutLogin_CheckedChanged(object sender, EventArgs e)
        {
            clearGrid();
        }

        protected void rbEnqWithTestReq_CheckedChanged(object sender, EventArgs e)
        {
            clearGrid();
        }

        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            if(rbEnqWithLogin.Checked)
            {
                DisplayAllEnquiry(1);
            }
            else if(rbEnqWithoutLogin.Checked)
            {
                DisplayAllEnquiry(0);
            }
            else if(rbEnqWithTestReq.Checked)
            {
                DisplayAllEnquiry(-1);
                //FirstGridViewRowOfDeviceOrder();
            }
        }

        public void DisplayAllEnquiry(int i)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
           // dt.Columns.Add(new DataColumn("Sr_No", typeof(string)));
            dt.Columns.Add(new DataColumn("testReq_id", typeof(string)));
            dt.Columns.Add(new DataColumn("testEnq_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("material_id", typeof(string)));
            dt.Columns.Add(new DataColumn("enq_date", typeof(string)));
            dt.Columns.Add(new DataColumn("Enq_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("Site_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("MATERIAL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("test_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_contact_person", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_contact_number", typeof(string)));
            dt.Columns.Add(new DataColumn("EnqDetails", typeof(string)));
            dt.Columns.Add(new DataColumn("VeenaEnqNo", typeof(string)));


            if (i == -1)
            {
                dr = dt.NewRow();
                string testName = "", prvTestName = "";
                int rowNo = 1, testReq_id = 0; bool flagSameEnqId = false, flagSameMatId = false;
                var result = dc.TestRequestDetails_View(0, 0, 0);
                foreach (var log in result)
                {
                    testName = "";
                    flagSameEnqId = false; flagSameMatId = false;
                    if (dt.Rows.Count > 0)
                    {
                        if ((dt.Rows[dt.Rows.Count - 1]["testEnq_id"].ToString() == Convert.ToString(log.ENQ_Id)))
                            flagSameEnqId = true;

                        if ((dt.Rows[dt.Rows.Count - 1]["material_id"].ToString() == Convert.ToString(log.material_id)))
                            flagSameMatId = true;

                        prvTestName = dt.Rows[dt.Rows.Count - 1]["test_Name"].ToString();

                        if (!flagSameMatId)
                            prvTestName = "";

                    }


                    dr = dt.NewRow();
                    if (!flagSameEnqId)
                    {
                        //     dr["Sr_No"] = rowNo;
                        dr["Enq_Id"] = log.ENQ_Id;
                        dr["enq_date"] = Convert.ToDateTime(log.ENQ_Date_dt).ToString("dd/MM/yyyy");
                        dr["CL_Name_var"] = log.CL_Name_var;
                        dr["Site_Name_var"] = log.SITE_Name_var;
                        dr["ENQ_contact_person"] = log.ENQ_contact_person;
                        dr["ENQ_contact_number"] = log.ENQ_contact_number;
                        //dr["material_id"] = log.material_id;
                        rowNo++;
                    }
                    dr["testEnq_id"] = log.ENQ_Id;
                    dr["material_id"] = log.material_id;
                    dr["testReq_id"] = log.id;
                    dr["MATERIAL_Name_var"] = log.MATERIAL_Name_var;
                    dr["EnqDetails"] = log.ENQ_Id + ";" + log.CL_Name_var + ";" + log.SITE_Name_var + ";" + log.MATERIAL_Name_var;
                    dr["VeenaEnqNo"] = log.EnquiryNo;
                    testReq_id = Convert.ToInt32(log.id);
                    var rslt = dc.TestRequestDetails_View(1, testReq_id, 0);
                    foreach (var lg in rslt)
                    {
                        if (!prvTestName.Contains(lg.test_name))
                            prvTestName += lg.test_name + ",";

                    }
                    testName = prvTestName;

                    dr["test_Name"] = testName.TrimEnd(',');

                    if (!(flagSameEnqId && flagSameMatId))// 
                    {
                        dt.Rows.Add(dr);

                    }
                }

                grdReport.Columns[0].Visible = true;
                grdReport.Columns[9].Visible = true;
                grdReport.Columns[6].Visible = true;
            }
            else
            {
                int j = 1; string matName = "";
                var result = dc.EnquiryApp_View(0, i);
                foreach (var c in result)
                {
                    matName = "";
                    dr = dt.NewRow();
                    // dr["Sr_No"] = j.ToString();
                    dr["testReq_id"] = 0;
                     dr["testEnq_Id"] = c.ENQ_Id.ToString();
                    dr["material_id"] = Convert.ToString(c.material_id);
                    dr["enq_date"] = Convert.ToDateTime(c.ENQ_Date_dt).ToString("dd/MM/yyyy");
                    dr["Enq_Id"] = c.ENQ_Id;
                    var mat = dc.EnquiryApp_Material_View(Convert.ToInt32(c.ENQ_Id));
                    foreach (var item in mat)
                    {
                        matName += item.MATERIAL_Name_var.ToString() + ",";
                    }
                    dr["MATERIAL_Name_var"] = matName.TrimEnd(',');
                    dr["CL_Name_var"] = Convert.ToString(c.CL_Name_var);
                    dr["Site_Name_var"] = Convert.ToString(c.SITE_Name_var);
                    dr["ENQ_contact_person"] = c.ENQ_contact_person;
                    dr["ENQ_contact_number"] = c.ENQ_contact_number;
                    dr["test_Name"] = "";
                    dr["VeenaEnqNo"] = "";
                    dt.Rows.Add(dr);
                    j++;
                }


                grdReport.Columns[0].Visible = false;
                grdReport.Columns[9].Visible = false;
                grdReport.Columns[6].Visible = false;
            }
            grdReport.DataSource = dt;
            grdReport.DataBind();
            lblTotalRecords.Text = "Total No of Records : " + grdReport.Rows.Count;

          
        }
        protected void cbxSelectOnCheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grdReport.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)grdReport.Rows[i].Cells[0].FindControl("cbxSelect");
                if (cbxSelect.Checked)
                {
                    cbxSelect.Checked = false;
                    break;
                }
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (rbEnqWithTestReq.Checked)
            {
                if (grdReport.Rows.Count > 0)
                {

                    List<string> lstEnqNo = new List<string>();
                    PrintPDFReport rpt = new PrintPDFReport();
                    //bool valid = false;
                    for (int i = 0; i < grdReport.Rows.Count; i++)
                    {
                        CheckBox cbxSelect = (CheckBox)grdReport.Rows[i].Cells[0].FindControl("cbxSelect");
                        Label lblEnq_Id = (Label)grdReport.Rows[i].FindControl("lblEnq_Id");
                        Label lblEnqVeena_Id = (Label)grdReport.Rows[i].FindControl("lblEnqVeena_Id");
                        if (cbxSelect.Checked && cbxSelect.Enabled == true && lblEnqVeena_Id.Text!="")
                        {
                            lstEnqNo.Add(lblEnq_Id.Text);
                        }

                    }

                    for (int i = 0; i < grdReport.Rows.Count; i++)
                    {
                        CheckBox cbxSelect = (CheckBox)grdReport.Rows[i].Cells[0].FindControl("cbxSelect");
                        if (cbxSelect.Checked)
                            cbxSelect.Checked = false;
                    }
                    rpt.AppEnqTestRequestFormPrint(lstEnqNo);

                }
            }
        }

        protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            
                Label lblEnq_Id = (Label)e.Row.FindControl("lblEnq_Id");
                if (lblEnq_Id.Text == "")
                {
                    CheckBox cbxSelect = (CheckBox)e.Row.FindControl("cbxSelect");
                    cbxSelect.Enabled = false;
                }
            }

        }
    }
}