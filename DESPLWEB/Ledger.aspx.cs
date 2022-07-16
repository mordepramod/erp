using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class Ledger : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Ledger Master";
                rdbCategory.Checked = true;
                pnlCategory.Visible = true;
                pnlCostCenter.Visible = false;
                pnlLedger.Visible = false;
                LoadCategoryList();
            }
        }

        private void LoadCategoryListInViewState()
        {
            Session["CategoryList"] = null;
            var Catagory = dc.CostCatagory_View("").ToList();
            Session["CategoryList"] = Catagory;
        }
        private void LoadCostCenterListInViewState()
        {
            Session["CostCenterList"] = null;
            var costCenter = dc.CostCenter_View(0,0).ToList();
            Session["CostCenterList"] = costCenter;
        }
        private void LoadCategoryList()
        {
            grdCategory.DataSource = null;
            grdCategory.DataBind();
            var Catagory = dc.CostCatagory_View("");
            int rowNo = 0;
            foreach (var catg in Catagory)
            {
                AddRowCategory();
                Label lblSrNo = (Label)grdCategory.Rows[rowNo].FindControl("lblSrNo");
                TextBox txtCategory = (TextBox)grdCategory.Rows[rowNo].FindControl("txtCategory");
                Label lblCategoryId = (Label)grdCategory.Rows[rowNo].FindControl("lblCategoryId");

                lblSrNo.Text = (rowNo + 1).ToString();
                txtCategory.Text = catg.CostCatagory_Description;
                lblCategoryId.Text = catg.CostCatagory_Id.ToString();
                rowNo++;
            }
            if (grdCategory.Rows.Count == 0)
            {
                AddRowCategory();
            }
        }
        private void LoadCostCenterList()
        {
            grdCostCenter.DataSource = null;
            grdCostCenter.DataBind();
            var costCenter = dc.CostCenter_View(0,0);
            int rowNo = 0;
            foreach (var costc in costCenter)
            {
                AddRowCostCenter();
                Label lblSrNo = (Label)grdCostCenter.Rows[rowNo].FindControl("lblSrNo");
                TextBox txtCostCenter = (TextBox)grdCostCenter.Rows[rowNo].FindControl("txtCostCenter");
                DropDownList ddlCategory = (DropDownList)grdCostCenter.Rows[rowNo].FindControl("ddlCategory");
                Label lblCostCenterId = (Label)grdCostCenter.Rows[rowNo].FindControl("lblCostCenterId");

                lblSrNo.Text = (rowNo + 1).ToString();
                txtCostCenter.Text = costc.CostCenter_Description;
                ddlCategory.SelectedValue = costc.CostCatagory_Id.ToString();
                lblCostCenterId.Text = costc.CostCenter_Id.ToString();
                rowNo++;
            }
            if (grdCostCenter.Rows.Count == 0)
            {
                AddRowCostCenter();
            }
        }
        private void LoadLedgerList()
        {
            grdLedger.DataSource = null;
            grdLedger.DataBind();
            var ledger = dc.Ledger_View(0,0,false,"");
            int rowNo = 0;
            foreach (var ledg in ledger)
            {
                AddRowLedger();
                Label lblSrNo = (Label)grdLedger.Rows[rowNo].FindControl("lblSrNo");
                TextBox txtLedger = (TextBox)grdLedger.Rows[rowNo].FindControl("txtLedger");
                DropDownList ddlCostCenter = (DropDownList)grdLedger.Rows[rowNo].FindControl("ddlCostCenter");
                DropDownList ddlCategory = (DropDownList)grdLedger.Rows[rowNo].FindControl("ddlCategory");
                Label lblLedgerId = (Label)grdLedger.Rows[rowNo].FindControl("lblLedgerId");

                lblSrNo.Text = (rowNo + 1).ToString();
                txtLedger.Text = ledg.LedgerName_Description;
                ddlCostCenter.SelectedValue = ledg.LedgerName_CostCenter_Id.ToString();
                ddlCategory.SelectedValue = ledg.LedgerName_Catagory_Id.ToString();
                lblLedgerId.Text = ledg.LedgerName_Id.ToString();
                rowNo++;
            }
            if (grdLedger.Rows.Count == 0)
            {
                AddRowLedger();
            }
        }

        #region add/delete rows category grid
        protected void AddRowCategory()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CategoryTable"] != null)
            {
                GetCurrentDataCategory();
                dt = (DataTable)ViewState["CategoryTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCategory", typeof(string)));
                dt.Columns.Add(new DataColumn("lblCategoryId", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = string.Empty;
            dr["txtCategory"] = string.Empty;
            dr["lblCategoryId"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CategoryTable"] = dt;
            grdCategory.DataSource = dt;
            grdCategory.DataBind();
            SetPreviousDataCategory();
        }

        protected void DeleteRowCategory(int rowIndex)
        {
            GetCurrentDataCategory();
            DataTable dt = ViewState["CategoryTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CategoryTable"] = dt;
            grdCategory.DataSource = dt;
            grdCategory.DataBind();
            SetPreviousDataCategory();
        }

        protected void SetPreviousDataCategory()
        {
            DataTable dt = (DataTable)ViewState["CategoryTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblSrNo = (Label)grdCategory.Rows[i].FindControl("lblSrNo");
                TextBox txtCategory = (TextBox)grdCategory.Rows[i].FindControl("txtCategory");
                Label lblCategoryId = (Label)grdCategory.Rows[i].FindControl("lblCategoryId");

                lblSrNo.Text =(i+1).ToString();
                txtCategory.Text = dt.Rows[i]["txtCategory"].ToString();
                lblCategoryId.Text = dt.Rows[i]["lblCategoryId"].ToString();
            }
        }

        protected void GetCurrentDataCategory()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtCategory", typeof(string)));
            dt.Columns.Add(new DataColumn("lblCategoryId", typeof(string)));
            for (int i = 0; i < grdCategory.Rows.Count; i++)
            {
                Label lblSrNo = (Label)grdCategory.Rows[i].FindControl("lblSrNo");
                TextBox txtCategory = (TextBox)grdCategory.Rows[i].FindControl("txtCategory");
                Label lblCategoryId = (Label)grdCategory.Rows[i].FindControl("lblCategoryId");

                dr = dt.NewRow();
                dr["lblSrNo"] = lblSrNo.Text;
                dr["txtCategory"] = txtCategory.Text;
                dr["lblCategoryId"] = lblCategoryId.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["CategoryTable"] = dt;

        }

        protected void ImgInsertCategory_Click(object sender, EventArgs e)
        {
            AddRowCategory();
        }
        
        protected void ImgDeleteCategory_Click(object sender, EventArgs e)
        {
            if (grdCategory.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;

                Label lblCategoryId = (Label)gvr.FindControl("lblCategoryId");
                if (lblCategoryId.Text == "")
                    DeleteRowCategory(gvr.RowIndex);
            }
        }
        #endregion

        #region add/delete rows cost center grid
        protected void AddRowCostCenter()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CostCenterTable"] != null)
            {
                GetCurrentDataCostCenter();
                dt = (DataTable)ViewState["CostCenterTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCostCenter", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlCategory", typeof(string)));
                dt.Columns.Add(new DataColumn("lblCostCenterId", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = string.Empty;
            dr["txtCostCenter"] = string.Empty;
            dr["ddlCategory"] = string.Empty;
            dr["lblCostCenterId"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CostCenterTable"] = dt;
            grdCostCenter.DataSource = dt;
            grdCostCenter.DataBind();
            SetPreviousDataCostCenter();
        }

        protected void DeleteRowCostCenter(int rowIndex)
        {
            GetCurrentDataCostCenter();
            DataTable dt = ViewState["CostCenterTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CostCenterTable"] = dt;
            grdCostCenter.DataSource = dt;
            grdCostCenter.DataBind();
            SetPreviousDataCostCenter();
        }

        protected void SetPreviousDataCostCenter()
        {
            DataTable dt = (DataTable)ViewState["CostCenterTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblSrNo = (Label)grdCostCenter.Rows[i].FindControl("lblSrNo");
                TextBox txtCostCenter = (TextBox)grdCostCenter.Rows[i].FindControl("txtCostCenter");
                DropDownList ddlCategory = (DropDownList)grdCostCenter.Rows[i].FindControl("ddlCategory");
                Label lblCostCenterId = (Label)grdCostCenter.Rows[i].FindControl("lblCostCenterId");

                lblSrNo.Text = (i + 1).ToString();
                txtCostCenter.Text = dt.Rows[i]["txtCostCenter"].ToString();
                ddlCategory.SelectedValue = dt.Rows[i]["ddlCategory"].ToString();
                lblCostCenterId.Text = dt.Rows[i]["lblCostCenterId"].ToString();
            }
        }

        protected void GetCurrentDataCostCenter()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtCostCenter", typeof(string)));
            dt.Columns.Add(new DataColumn("ddlCategory", typeof(string)));
            dt.Columns.Add(new DataColumn("lblCostCenterId", typeof(string)));
            for (int i = 0; i < grdCostCenter.Rows.Count; i++)
            {
                Label lblSrNo = (Label)grdCostCenter.Rows[i].FindControl("lblSrNo");
                TextBox txtCostCenter = (TextBox)grdCostCenter.Rows[i].FindControl("txtCostCenter");
                DropDownList ddlCategory = (DropDownList)grdCostCenter.Rows[i].FindControl("ddlCategory");
                Label lblCostCenterId = (Label)grdCostCenter.Rows[i].FindControl("lblCostCenterId");

                dr = dt.NewRow();
                dr["lblSrNo"] = lblSrNo.Text;
                dr["txtCostCenter"] = txtCostCenter.Text;
                dr["ddlCategory"] = ddlCategory.SelectedValue;
                dr["lblCostCenterId"] = lblCostCenterId.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["CostCenterTable"] = dt;

        }

        protected void ImgInsertCostCenter_Click(object sender, EventArgs e)
        {
            AddRowCostCenter();
        }
        
        protected void ImgDeleteCostCenter_Click(object sender, EventArgs e)
        {
            if (grdCostCenter.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;

                Label lblCostCenterId = (Label)gvr.FindControl("lblCostCenterId");
                if (lblCostCenterId.Text == "")
                    DeleteRowCostCenter(gvr.RowIndex);
            }
        }
        #endregion

        #region add/delete rows ledger grid
        protected void AddRowLedger()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["LedgerTable"] != null)
            {
                GetCurrentDataLedger();
                dt = (DataTable)ViewState["LedgerTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLedger", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlCostCenter", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlCategory", typeof(string)));
                dt.Columns.Add(new DataColumn("lblLedgerId", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = string.Empty;
            dr["txtLedger"] = string.Empty;
            dr["ddlCostCenter"] = string.Empty;
            dr["ddlCategory"] = string.Empty;
            dr["lblLedgerId"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["LedgerTable"] = dt;
            grdLedger.DataSource = dt;
            grdLedger.DataBind();
            SetPreviousDataLedger();
        }

        protected void DeleteRowLedger(int rowIndex)
        {
            GetCurrentDataLedger();
            DataTable dt = ViewState["LedgerTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["LedgerTable"] = dt;
            grdLedger.DataSource = dt;
            grdLedger.DataBind();
            SetPreviousDataLedger();
        }

        protected void SetPreviousDataLedger()
        {
            DataTable dt = (DataTable)ViewState["LedgerTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblSrNo = (Label)grdLedger.Rows[i].FindControl("lblSrNo");
                TextBox txtLedger = (TextBox)grdLedger.Rows[i].FindControl("txtLedger");
                DropDownList ddlCostCenter = (DropDownList)grdLedger.Rows[i].FindControl("ddlCostCenter");
                DropDownList ddlCategory = (DropDownList)grdLedger.Rows[i].FindControl("ddlCategory");
                Label lblLedgerId = (Label)grdLedger.Rows[i].FindControl("lblLedgerId");

                lblSrNo.Text = (i + 1).ToString();
                txtLedger.Text = dt.Rows[i]["txtLedger"].ToString();
                ddlCostCenter.SelectedValue = dt.Rows[i]["ddlCostCenter"].ToString();
                ddlCategory.SelectedValue = dt.Rows[i]["ddlCategory"].ToString();
                lblLedgerId.Text = dt.Rows[i]["lblLedgerId"].ToString();
            }
        }

        protected void GetCurrentDataLedger()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtLedger", typeof(string)));
            dt.Columns.Add(new DataColumn("ddlCostCenter", typeof(string)));
            dt.Columns.Add(new DataColumn("ddlCategory", typeof(string)));
            dt.Columns.Add(new DataColumn("lblLedgerId", typeof(string)));
            for (int i = 0; i < grdLedger.Rows.Count; i++)
            {
                Label lblSrNo = (Label)grdLedger.Rows[i].FindControl("lblSrNo");
                TextBox txtLedger = (TextBox)grdLedger.Rows[i].FindControl("txtLedger");
                DropDownList ddlCostCenter = (DropDownList)grdLedger.Rows[i].FindControl("ddlCostCenter");
                DropDownList ddlCategory = (DropDownList)grdLedger.Rows[i].FindControl("ddlCategory");
                Label lblLedgerId = (Label)grdLedger.Rows[i].FindControl("lblLedgerId");

                dr = dt.NewRow();
                dr["lblSrNo"] = lblSrNo.Text;
                dr["txtLedger"] = txtLedger.Text;
                dr["ddlCostCenter"] = ddlCostCenter.SelectedValue;
                dr["ddlCategory"] = ddlCategory.SelectedValue;
                dr["lblLedgerId"] = lblLedgerId.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["LedgerTable"] = dt;

        }

        protected void ImgInsertLedger_Click(object sender, EventArgs e)
        {
            AddRowLedger();
        }

        protected void ImgDeleteLedger_Click(object sender, EventArgs e)
        {
            if (grdLedger.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                Label lblLedgerId = (Label)gvr.FindControl("lblLedgerId");
                if (lblLedgerId.Text == "")
                    DeleteRowLedger(gvr.RowIndex);
            }
        }
        #endregion

        protected void rdbCategory_CheckedChanged(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            pnlCategory.Visible = true;
            pnlCostCenter.Visible = false;
            pnlLedger.Visible = false;
            LoadCategoryList();
        }

        protected void rdbCostCenter_CheckedChanged(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            pnlCategory.Visible = false;
            pnlCostCenter.Visible = true;
            pnlLedger.Visible = false;
            LoadCategoryListInViewState();
            LoadCostCenterList();
        }

        protected void rdbLedger_CheckedChanged(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            pnlCategory.Visible = false;
            pnlCostCenter.Visible = false;
            pnlLedger.Visible = true;
            LoadCategoryListInViewState();
            LoadCostCenterListInViewState();
            LoadLedgerList();
        }

        protected void ddlCostCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            if (gvr != null)
            {
                DropDownList ddlCostCenter = (DropDownList)gvr.FindControl("ddlCostCenter");
                DropDownList ddlCategory = (DropDownList)gvr.FindControl("ddlCategory");
                if (ddlCostCenter.SelectedIndex > 0)
                {
                    var costcenter = dc.CostCenter_View(Convert.ToInt32(ddlCostCenter.SelectedValue), 0);
                    foreach (var costc in costcenter)
                    {
                        if(costc.CostCatagory_Id != 0)
                            ddlCategory.SelectedValue = costc.CostCatagory_Id.ToString();
                    }
                }
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                if (rdbCategory.Checked == true)
                {
                    for (int i = 0; i < grdCategory.Rows.Count; i++)
                    {
                        int CategoryId = 0;
                        TextBox txtCategory = (TextBox)grdCategory.Rows[i].FindControl("txtCategory");
                        Label lblCategoryId = (Label)grdCategory.Rows[i].FindControl("lblCategoryId");
                        if (lblCategoryId.Text != "")
                        {
                            CategoryId = Convert.ToInt32(lblCategoryId.Text); 
                        }
                        dc.CostCatagory_Update(CategoryId, txtCategory.Text);
                    }
                    LoadCategoryList();
                }
                else if (rdbCostCenter.Checked == true)
                {
                    for (int i = 0; i < grdCostCenter.Rows.Count; i++)
                    {
                        int CostCenterId = 0;
                        TextBox txtCostCenter = (TextBox)grdCostCenter.Rows[i].FindControl("txtCostCenter");
                        DropDownList ddlCategory = (DropDownList)grdCostCenter.Rows[i].FindControl("ddlCategory");
                        Label lblCostCenterId = (Label)grdCostCenter.Rows[i].FindControl("lblCostCenterId");
                        if (lblCostCenterId.Text != "")
                        {
                            CostCenterId = Convert.ToInt32(lblCostCenterId.Text);
                        }
                        dc.CostCenter_Update(CostCenterId,Convert.ToInt32(ddlCategory.SelectedValue), txtCostCenter.Text,false);
                    }
                    LoadCostCenterList();
                }
                else if (rdbLedger.Checked == true)
                {
                    for (int i = 0; i < grdLedger.Rows.Count; i++)
                    {
                        int LedgerId = 0;
                        TextBox txtLedger = (TextBox)grdLedger.Rows[i].FindControl("txtLedger");
                        DropDownList ddlCostCenter = (DropDownList)grdLedger.Rows[i].FindControl("ddlCostCenter");
                        DropDownList ddlCategory = (DropDownList)grdLedger.Rows[i].FindControl("ddlCategory");
                        Label lblLedgerId = (Label)grdLedger.Rows[i].FindControl("lblLedgerId");
                        if (lblLedgerId.Text != "")
                        {
                            LedgerId = Convert.ToInt32(lblLedgerId.Text);
                        }
                        dc.Ledger_Update(LedgerId, Convert.ToInt32(ddlCategory.SelectedValue),Convert.ToInt32(ddlCostCenter.SelectedValue), false, txtLedger.Text);
                    }
                    LoadLedgerList();
                }
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Record Saved Sucessfully";
            }
        }

        protected Boolean ValidateData()
        {

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            Boolean valid = true;
            if (rdbCategory.Checked == true)
            {
                for (int i = 0; i < grdCategory.Rows.Count; i++)
                {
                    TextBox txtCategory = (TextBox)grdCategory.Rows[i].FindControl("txtCategory");
                    txtCategory.Text = txtCategory.Text.Trim();
                    if (txtCategory.Text.Trim() == "")
                    {
                        lblMsg.Text = "Enter Category description for Sr No. " + (i + 1) + ".";
                        txtCategory.Focus();
                        valid = false;
                        break;
                    }
                }
                if (valid == true)
                {
                    for (int i = 0; i < grdCategory.Rows.Count; i++)
                    {
                        TextBox txtCategory = (TextBox)grdCategory.Rows[i].FindControl("txtCategory");
                        for (int j = 0; j < grdCategory.Rows.Count; j++)
                        {
                            if (i != j)
                            {
                                TextBox txtCategoryCopy = (TextBox)grdCategory.Rows[j].FindControl("txtCategory");
                                if (txtCategory.Text == txtCategoryCopy.Text)
                                {
                                    lblMsg.Text = "Duplicate Category description for Sr No. " + (j + 1) + ".";
                                    txtCategoryCopy.Focus();
                                    valid = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else if (rdbCostCenter.Checked == true)
            {
                for (int i = 0; i < grdCostCenter.Rows.Count; i++)
                {
                    TextBox txtCostCenter = (TextBox)grdCostCenter.Rows[i].FindControl("txtCostCenter");
                    DropDownList ddlCategory = (DropDownList)grdCostCenter.Rows[i].FindControl("ddlCategory");
                    txtCostCenter.Text = txtCostCenter.Text.Trim();
                    if (txtCostCenter.Text.Trim() == "")
                    {
                        lblMsg.Text = "Enter Cost Center description for Sr No. " + (i + 1) + ".";
                        txtCostCenter.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddlCategory.SelectedIndex <= 0)
                    {
                        lblMsg.Text = "Select Category for Sr No. " + (i + 1) + ".";
                        ddlCategory.Focus();
                        valid = false;
                        break;
                    }
                }
                if (valid == true)
                {
                    for (int i = 0; i < grdCostCenter.Rows.Count; i++)
                    {
                        TextBox txtCostCenter = (TextBox)grdCostCenter.Rows[i].FindControl("txtCostCenter");
                        DropDownList ddlCategory = (DropDownList)grdCostCenter.Rows[i].FindControl("ddlCategory");
                        for (int j = 0; j < grdCostCenter.Rows.Count; j++)
                        {
                            if (i != j)
                            {
                                TextBox txtCostCenterCopy = (TextBox)grdCostCenter.Rows[j].FindControl("txtCostCenter");
                                DropDownList ddlCategoryCopy = (DropDownList)grdCostCenter.Rows[j].FindControl("ddlCategory");
                                if (txtCostCenter.Text == txtCostCenterCopy.Text 
                                    && ddlCategory.SelectedValue == ddlCategoryCopy.SelectedValue)
                                {
                                    lblMsg.Text = "Duplicate Cost Center description for Sr No. " + (j + 1) + ".";
                                    txtCostCenterCopy.Focus();
                                    valid = false;
                                    break;
                                }
                            }
                        }

                    }
                }
            }
            else if (rdbLedger.Checked == true)
            {
                for (int i = 0; i < grdLedger.Rows.Count; i++)
                {
                    TextBox txtLedger = (TextBox)grdLedger.Rows[i].FindControl("txtLedger");
                    DropDownList ddlCostCenter = (DropDownList)grdLedger.Rows[i].FindControl("ddlCostCenter");
                    DropDownList ddlCategory = (DropDownList)grdLedger.Rows[i].FindControl("ddlCategory");
                    txtLedger.Text = txtLedger.Text.Trim();
                    if (txtLedger.Text.Trim() == "")
                    {
                        lblMsg.Text = "Enter Ledger description for Sr No. " + (i + 1) + ".";
                        txtLedger.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddlCostCenter.SelectedIndex <= 0)
                    {
                        lblMsg.Text = "Select Cost Center for Sr No. " + (i + 1) + ".";
                        ddlCostCenter.Focus();
                        valid = false;
                        break;
                    }
                    else if (ddlCategory.SelectedIndex <= 0)
                    {
                        lblMsg.Text = "Select Category for Sr No. " + (i + 1) + ".";
                        ddlCategory.Focus();
                        valid = false;
                        break;
                    }
                }
                if (valid == true)
                {
                    for (int i = 0; i < grdLedger.Rows.Count; i++)
                    {
                        TextBox txtLedger = (TextBox)grdLedger.Rows[i].FindControl("txtLedger");
                        DropDownList ddlCostCenter = (DropDownList)grdLedger.Rows[i].FindControl("ddlCostCenter");
                        DropDownList ddlCategory = (DropDownList)grdLedger.Rows[i].FindControl("ddlCategory");
                        for (int j = 0; j < grdLedger.Rows.Count; j++)
                        {
                            if (i != j)
                            {
                                TextBox txtLedgerCopy = (TextBox)grdLedger.Rows[j].FindControl("txtLedger");
                                DropDownList ddlCostCenterCopy = (DropDownList)grdLedger.Rows[j].FindControl("ddlCostCenter");
                                DropDownList ddlCategoryCopy = (DropDownList)grdLedger.Rows[j].FindControl("ddlCategory");
                                if (txtLedger.Text == txtLedgerCopy.Text
                                    && ddlCategory.SelectedValue == ddlCategoryCopy.SelectedValue
                                    && ddlCostCenter.SelectedValue == ddlCostCenterCopy.SelectedValue)
                                {
                                    lblMsg.Text = "Duplicate Ledger description for Sr No. " + (j + 1) + ".";
                                    txtLedgerCopy.Focus();
                                    valid = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (valid == false)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Visible = true;
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + dispalyMsg + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
        protected void lnkExit_Click(object sender, EventArgs e)
        {

        }

        protected void grdCostCenter_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlCategory = (DropDownList)e.Row.FindControl("ddlCategory");
                //var CostCatagory = dc.CostCatagory_View("");
                //ddlCategory.DataSource = CostCatagory;
                ddlCategory.DataSource = Session["CategoryList"];
                ddlCategory.DataTextField = "CostCatagory_Description";
                ddlCategory.DataValueField = "CostCatagory_Id";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("---Select---", "0"));
            }
        }

        protected void grdLedger_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlCategory = (DropDownList)e.Row.FindControl("ddlCategory");
                //var CostCatagory = dc.CostCatagory_View("");
                //ddlCategory.DataSource = CostCatagory;
                ddlCategory.DataSource = Session["CategoryList"];
                ddlCategory.DataTextField = "CostCatagory_Description";
                ddlCategory.DataValueField = "CostCatagory_Id";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("---Select---", "0"));

                DropDownList ddlCostCenter = (DropDownList)e.Row.FindControl("ddlCostCenter");
                //var CostCenter = dc.CostCenter_View(0);
                //ddlCostCenter.DataSource = CostCenter;
                ddlCostCenter.DataSource = Session["CostCenterList"];
                ddlCostCenter.DataTextField = "CostCenter_Description";
                ddlCostCenter.DataValueField = "CostCenter_Id";
                ddlCostCenter.DataBind();
                ddlCostCenter.Items.Insert(0, new ListItem("---Select---", "0"));
            }
        }
    }
}