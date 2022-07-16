using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace DESPLWEB
{
    public partial class BillMonthly : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool userRight = false;
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Bill";
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    lblDiscountModifyRight.Text = "False";
                    var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                    foreach (var u in user)
                    {
                        if (u.USER_Bill_right_bit == true)
                        {
                            userRight = true;
                        }
                        if (u.User_DiscountModify_right_bit == true)
                        {
                            lblDiscountModifyRight.Text = "True";
                        }
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
                    //Query string decrypt
                    string strReq = "", strReq1 = "";
                    strReq1 = Request.RawUrl;
                    strReq = strReq1.Substring(strReq1.IndexOf('?') + 1);
                    if (!strReq.Equals("") && strReq1.IndexOf('?') >= 0)
                    {
                        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                        strReq = obj.Decrypt(strReq);
                        if (strReq.Contains("=") == false)
                        {
                            Session.Abandon();
                            Response.Redirect("Login.aspx");
                        }

                        string[] arrMsgs = strReq.Split('&');
                        string[] arrIndMsg;

                        arrIndMsg = arrMsgs[0].Split('=');
                        lblBillId.Text = arrIndMsg[1].ToString().Trim();
                        arrIndMsg = arrMsgs[1].Split('=');
                        lblCouponBill.Text = arrIndMsg[1].ToString().Trim();
                    }
                    else
                    {
                        lblBillId.Text = "";
                        lblCouponBill.Text = "True";
                    }
                    ////
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    LoadServiceTax();
                    LoadSwachhBharatTax();
                    LoadKisanKrishiTax();
                    LoadGSTax();
                    if (lblBillId.Text != "")
                    {
                        LoadBillDetail();
                    }
                    else
                    {
                        ClearData();
                        LoadClientList(0);
                        //AddRowBillDetail();
                        txtRecordNo.Text = "0";
                        txtRecordType.Text = "---";
                    }
                    lnkSave.Visible = true;
                    lnkPrint.Visible = false;
                }
            }
        }

        private void LoadClientList(int clientId)
        {
            var cl = dc.Client_View(clientId, 0, "", "");
            ddlClient.DataSource = cl;
            ddlClient.DataTextField = "CL_Name_var";
            ddlClient.DataValueField = "CL_Id";
            ddlClient.DataBind();
            ddlClient.Items.Insert(0, new ListItem("-----Select----", "0"));
        }
        #region BillDetailGridEdit
        protected void AddRowBillDetail()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["BillDetailTable"] != null)
            {
                GetCurrentDataBillDetail();
                dt = (DataTable)ViewState["BillDetailTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("lblSubTestId", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlSubTest", typeof(string)));
                dt.Columns.Add(new DataColumn("lblTestId", typeof(string)));
                dt.Columns.Add(new DataColumn("ddl_Test", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("txtReferenceNo", typeof(string)));
                dt.Columns.Add(new DataColumn("lblReceivedDate", typeof(string)));
                dt.Columns.Add(new DataColumn("lblMaterialName", typeof(string)));
                dt.Columns.Add(new DataColumn("lblDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSACCode", typeof(string)));
                dt.Columns.Add(new DataColumn("txtQuantity", typeof(string)));
                dt.Columns.Add(new DataColumn("txtActualRate", typeof(string)));
                dt.Columns.Add(new DataColumn("txtTestDiscount", typeof(string)));
                dt.Columns.Add(new DataColumn("txtRate", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAmount", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["lblSubTestId"] = string.Empty;
            dr["ddlSubTest"] = string.Empty;
            dr["lblTestId"] = string.Empty;
            dr["ddl_Test"] = string.Empty;
            dr["txtDescription"] = string.Empty;
            dr["txtReferenceNo"] = string.Empty;
            dr["lblReceivedDate"] = string.Empty;
            dr["lblMaterialName"] = string.Empty;
            dr["lblDescription"] = string.Empty;
            //dr["txtSACCode"] = "00440249";
            if (txtRecordType.Text == "SO" || txtRecordType.Text == "GT")
                dr["txtSACCode"] = "998341";
            else
                dr["txtSACCode"] = "998346";
            dr["txtQuantity"] = string.Empty;
            dr["txtActualRate"] = string.Empty;
            dr["txtTestDiscount"] = string.Empty;
            dr["txtRate"] = string.Empty;
            dr["txtAmount"] = string.Empty;            
            dt.Rows.Add(dr);

            ViewState["BillDetailTable"] = dt;
            grdDetails.DataSource = dt;
            grdDetails.DataBind();
            SetPreviousDataBillDetail();
        }

        protected void DeleteRowBillDetail(int rowIndex)
        {
            GetCurrentDataBillDetail();
            DataTable dt = ViewState["BillDetailTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["BillDetailTable"] = dt;
            grdDetails.DataSource = dt;
            grdDetails.DataBind();
            SetPreviousDataBillDetail();
        }

        protected void SetPreviousDataBillDetail()
        {
            DataTable dt = (DataTable)ViewState["BillDetailTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DropDownList ddlSubTest = (DropDownList)grdDetails.Rows[i].FindControl("ddlSubTest");
                Label lblSubTestId = (Label)grdDetails.Rows[i].FindControl("lblSubTestId");
                Label lblTestId = (Label)grdDetails.Rows[i].FindControl("lblTestId");
                DropDownList ddl_Test = (DropDownList)grdDetails.Rows[i].FindControl("ddl_Test");
                TextBox txtDescription = (TextBox)grdDetails.Rows[i].FindControl("txtDescription");
                TextBox txtReferenceNo = (TextBox)grdDetails.Rows[i].FindControl("txtReferenceNo");
                Label lblReceivedDate = (Label)grdDetails.Rows[i].FindControl("lblReceivedDate");
                Label lblMaterialName = (Label)grdDetails.Rows[i].FindControl("lblMaterialName");
                Label lblDescription = (Label)grdDetails.Rows[i].FindControl("lblDescription");
                TextBox txtSACCode = (TextBox)grdDetails.Rows[i].FindControl("txtSACCode");
                TextBox txtQuantity = (TextBox)grdDetails.Rows[i].FindControl("txtQuantity");
                TextBox txtActualRate = (TextBox)grdDetails.Rows[i].FindControl("txtActualRate");
                TextBox txtTestDiscount = (TextBox)grdDetails.Rows[i].FindControl("txtTestDiscount");
                TextBox txtRate = (TextBox)grdDetails.Rows[i].FindControl("txtRate");
                TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");

                grdDetails.Rows[i].Cells[2].Text = (i + 1).ToString();
                txtReferenceNo.Text = dt.Rows[i]["txtReferenceNo"].ToString();
                int refNo = 0;
                if (txtReferenceNo.Text != "" && (int.TryParse(txtReferenceNo.Text, out refNo)) == true)
                {
                    string recordType = "";
                    var inward = dc.Inward_View(Convert.ToInt32(txtReferenceNo.Text), 0, "", null, null);
                    foreach (var inwd in inward)
                    {
                        recordType = inwd.INWD_RecordType_var;
                    }
                    if (recordType == "")
                        recordType = "MS";
                    if (recordType == "OT")
                    {
                        var otherTest = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, 0);
                        ddlSubTest.DataSource = otherTest;
                        ddlSubTest.DataTextField = "TEST_Name_var";
                        ddlSubTest.DataValueField = "TEST_Id";
                        ddlSubTest.DataBind();
                        ddlSubTest.Items.Insert(0, new ListItem("---Select---", "0"));
                        ddlSubTest.Visible = true;

                        if (dt.Rows[i]["lblSubTestId"].ToString() != "")
                        {
                            lblSubTestId.Text = dt.Rows[i]["lblSubTestId"].ToString();
                            ddlSubTest.SelectedValue = dt.Rows[i]["ddlSubTest"].ToString();

                            int SubTestId = 0;
                            if (lblSubTestId.Text != "")
                                SubTestId = Convert.ToInt32(lblSubTestId.Text);
                            if (lblSubTestId.Text == "0" || lblSubTestId.Text == "")
                            {
                                ddl_Test.DataSource = null;
                                ddl_Test.DataBind();
                            }
                            else
                            {
                                var test = dc.Test_View_ForBillModify(recordType, 0, SubTestId);
                                ddl_Test.DataSource = test;
                                ddl_Test.DataTextField = "TEST_Name_var";
                                ddl_Test.DataValueField = "TEST_Id";
                                ddl_Test.DataBind();
                                ddl_Test.Items.Insert(0, new ListItem("---Select---", "0"));

                                lblTestId.Text = dt.Rows[i]["lblTestId"].ToString();
                                ddl_Test.SelectedValue = dt.Rows[i]["ddl_Test"].ToString();
                            }
                        }
                    }
                    else
                    {
                        var test = dc.Test_View_ForBillModify(recordType, 0, 0);
                        ddl_Test.DataSource = test;
                        ddl_Test.DataTextField = "TEST_Name_var";
                        ddl_Test.DataValueField = "TEST_Id";
                        ddl_Test.DataBind();
                        ddl_Test.Items.Insert(0, new ListItem("---Select---", "0"));

                        lblTestId.Text = dt.Rows[i]["lblTestId"].ToString();
                        ddl_Test.SelectedValue = dt.Rows[i]["ddl_Test"].ToString();
                        ddlSubTest.Visible = false;
                    }
                }
                txtDescription.Text = dt.Rows[i]["txtDescription"].ToString();                
                lblReceivedDate.Text = dt.Rows[i]["lblReceivedDate"].ToString();
                lblMaterialName.Text = dt.Rows[i]["lblMaterialName"].ToString();
                lblDescription.Text = dt.Rows[i]["lblDescription"].ToString();
                txtSACCode.Text = dt.Rows[i]["txtSACCode"].ToString();
                txtQuantity.Text = dt.Rows[i]["txtQuantity"].ToString();
                txtActualRate.Text = dt.Rows[i]["txtActualRate"].ToString();
                txtTestDiscount.Text = dt.Rows[i]["txtTestDiscount"].ToString();
                if (txtActualRate.Text == "")
                    txtActualRate.Text = "0";
                if (txtTestDiscount.Text == "")
                    txtTestDiscount.Text = "0";
                txtRate.Text = dt.Rows[i]["txtRate"].ToString();
                txtAmount.Text = dt.Rows[i]["txtAmount"].ToString();
            }
        }

        protected void GetCurrentDataBillDetail()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddlSubTest", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblSubTestId", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblTestId", typeof(string)));
            dtTable.Columns.Add(new DataColumn("ddl_Test", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtReferenceNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblReceivedDate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblMaterialName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtSACCode", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtQuantity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtActualRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtTestDiscount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAmount", typeof(string)));
            for (int i = 0; i < grdDetails.Rows.Count; i++)
            {
                DropDownList ddlSubTest = (DropDownList)grdDetails.Rows[i].FindControl("ddlSubTest");
                Label lblSubTestId = (Label)grdDetails.Rows[i].FindControl("lblSubTestId");
                Label lblTestId = (Label)grdDetails.Rows[i].FindControl("lblTestId");
                DropDownList ddl_Test = (DropDownList)grdDetails.Rows[i].FindControl("ddl_Test");
                TextBox txtDescription = (TextBox)grdDetails.Rows[i].FindControl("txtDescription");
                TextBox txtReferenceNo = (TextBox)grdDetails.Rows[i].FindControl("txtReferenceNo");
                Label lblReceivedDate = (Label)grdDetails.Rows[i].FindControl("lblReceivedDate");
                Label lblMaterialName = (Label)grdDetails.Rows[i].FindControl("lblMaterialName");
                Label lblDescription = (Label)grdDetails.Rows[i].FindControl("lblDescription");
                TextBox txtSACCode = (TextBox)grdDetails.Rows[i].FindControl("txtSACCode");
                TextBox txtQuantity = (TextBox)grdDetails.Rows[i].FindControl("txtQuantity");
                TextBox txtActualRate = (TextBox)grdDetails.Rows[i].FindControl("txtActualRate");
                TextBox txtTestDiscount = (TextBox)grdDetails.Rows[i].FindControl("txtTestDiscount");
                TextBox txtRate = (TextBox)grdDetails.Rows[i].FindControl("txtRate");
                TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["ddlSubTest"] = ddlSubTest.SelectedValue;
                drRow["lblSubTestId"] = ddlSubTest.SelectedValue;
                drRow["lblTestId"] = lblTestId.Text;
                drRow["ddl_Test"] = ddl_Test.SelectedValue;
                drRow["txtDescription"] = txtDescription.Text;
                drRow["txtReferenceNo"] = txtReferenceNo.Text;
                drRow["lblReceivedDate"] = lblReceivedDate.Text;
                drRow["lblMaterialName"] = lblMaterialName.Text;
                drRow["lblDescription"] = lblDescription.Text;
                drRow["txtSACCode"] = txtSACCode.Text;
                drRow["txtQuantity"] = txtQuantity.Text;
                drRow["txtActualRate"] = txtActualRate.Text;
                drRow["txtTestDiscount"] = txtTestDiscount.Text;
                drRow["txtRate"] = txtRate.Text;
                drRow["txtAmount"] = txtAmount.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["BillDetailTable"] = dtTable;
        }

        protected void imgInsert_Click(object sender, CommandEventArgs e)
        {
            AddRowBillDetail();
            CalculateBill();
        }

        protected void imgDelete_Click(object sender, CommandEventArgs e)
        {
            if (grdDetails.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowBillDetail(gvr.RowIndex);
                CalculateBill();
            }
        }

        protected void grdDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {                
                //TextBox txtReferenceNo = (TextBox)e.Row.FindControl("txtReferenceNo");
                //int refNo = 0;
                //if (txtReferenceNo.Text != "" && int.TryParse(txtReferenceNo.Text, out refNo) == true)
                //{
                //    var inward = dc.Inward_View(refNo, 0, "", null, null);
                //    string recordType = "";
                //    foreach (var inwd in inward)
                //    {
                //        recordType = inwd.INWD_RecordType_var;                        
                //    }
                //    if (recordType == "")
                //        recordType = "MS";
                //    DropDownList ddl_Test = (DropDownList)e.Row.FindControl("ddl_Test");

                //    if (recordType == "OT")
                //    {
                //        var otherTest = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, 0);
                //        DropDownList ddlSubTest = (DropDownList)e.Row.FindControl("ddlSubTest");
                //        //ddlSubTest.DataSource = ViewState["OtherSubTestList"];
                //        ddlSubTest.DataSource = otherTest;
                //        ddlSubTest.DataTextField = "TEST_Name_var";
                //        ddlSubTest.DataValueField = "TEST_Id";
                //        ddlSubTest.DataBind();
                //        ddlSubTest.Items.Insert(0, new ListItem("---Select---", "0"));
                //        ddlSubTest.Visible = true;
                //    }
                //    else
                //    {
                //        var test = dc.Test_View_ForBillModify(recordType, 0, 0);
                //        ddl_Test.DataSource = test;
                //        ddl_Test.DataTextField = "TEST_Name_var";
                //        ddl_Test.DataValueField = "TEST_Id";
                //        ddl_Test.DataBind();
                //        ddl_Test.Items.Insert(0, new ListItem("---Select---", "0"));
                //    }
                //}
                if (lblDiscountModifyRight.Text == "True")
                {   
                    TextBox txtTestDiscount = (TextBox)e.Row.FindControl("txtTestDiscount");
                    TextBox txtActualRate = (TextBox)e.Row.FindControl("txtActualRate");
                    txtTestDiscount.ReadOnly = false;
                    txtActualRate.ReadOnly = false;
                }
            }
        }
        
        protected void txtReferenceNo_TextChanged(object sender, EventArgs eventArgs)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;

            TextBox txtReferenceNo = (TextBox)grdDetails.Rows[rowindex].FindControl("txtReferenceNo");
            Label lblReceivedDate = (Label)grdDetails.Rows[rowindex].FindControl("lblReceivedDate");
            Label lblMaterialName = (Label)grdDetails.Rows[rowindex].FindControl("lblMaterialName");
            Label lblDescription = (Label)grdDetails.Rows[rowindex].FindControl("lblDescription");
            DropDownList ddlSubTest = (DropDownList)grdDetails.Rows[rowindex].FindControl("ddlSubTest");
            DropDownList ddl_Test = (DropDownList)grdDetails.Rows[rowindex].FindControl("ddl_Test");
            if (txtReferenceNo.Text != "" && (int.TryParse(txtReferenceNo.Text, out rowindex) ) == true)
            {
                string recordType = "";
                var inward= dc.Inward_View(Convert.ToInt32(txtReferenceNo.Text), 0, "", null, null);
                foreach (var inwd in inward)
                {
                    lblReceivedDate.Text = inwd.INWD_ReceivedDate_dt.ToString();
                    lblMaterialName.Text = inwd.MATERIAL_Name_var;
                    recordType = inwd.INWD_RecordType_var;
                }
                
                if (recordType == "")
                    recordType = "MS";
                
                if (recordType == "OT")
                {
                    var otherTest = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, 0);                    
                    ddlSubTest.DataSource = otherTest;
                    ddlSubTest.DataTextField = "TEST_Name_var";
                    ddlSubTest.DataValueField = "TEST_Id";
                    ddlSubTest.DataBind();
                    ddlSubTest.Items.Insert(0, new ListItem("---Select---", "0"));
                    ddlSubTest.Visible = true;
                }
                else
                {
                    var test = dc.Test_View_ForBillModify(recordType, 0, 0);
                    ddl_Test.DataSource = test;
                    ddl_Test.DataTextField = "TEST_Name_var";
                    ddl_Test.DataValueField = "TEST_Id";
                    ddl_Test.DataBind();
                    ddl_Test.Items.Insert(0, new ListItem("---Select---", "0"));
                    ddlSubTest.Visible = false;
                }
            }
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs eventArgs)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txtQuantity = (TextBox)grdDetails.Rows[rowindex].FindControl("txtQuantity");
            TextBox txtRate = (TextBox)grdDetails.Rows[rowindex].FindControl("txtRate");
            TextBox txtAmount = (TextBox)grdDetails.Rows[rowindex].FindControl("txtAmount");
            txtDiscPer.Text = "0.00";
            txtDiscount.Text = "0.00";

            txtAmount.Text = "";
            if (txtRate.Text != "" && txtQuantity.Text != "")
            {
                txtAmount.Text = Convert.ToDecimal(Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQuantity.Text)).ToString("0.00");
            }
            CalculateBill();
        }

        protected void txtActualRate_TextChanged(object sender, EventArgs eventArgs)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txtQuantity = (TextBox)grdDetails.Rows[rowindex].FindControl("txtQuantity");
            TextBox txtRate = (TextBox)grdDetails.Rows[rowindex].FindControl("txtRate");
            TextBox txtAmount = (TextBox)grdDetails.Rows[rowindex].FindControl("txtAmount");
            TextBox txtActualRate = (TextBox)grdDetails.Rows[rowindex].FindControl("txtActualRate");
            TextBox txtTestDiscount = (TextBox)grdDetails.Rows[rowindex].FindControl("txtTestDiscount");

            txtRate.Text = txtActualRate.Text;
            if (txtActualRate.Text != "" && txtTestDiscount.Text != "")
            {
                if (Convert.ToDecimal(txtTestDiscount.Text) >= 100)
                {
                    txtTestDiscount.Text = "0";
                    txtTestDiscount.Focus();
                    string msg = "alert('Discount should be less than 100% ')";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                }
                else if (Convert.ToDecimal(txtTestDiscount.Text) > 0)
                {
                    txtRate.Text = (Convert.ToDecimal(txtActualRate.Text) - (Convert.ToDecimal(txtActualRate.Text) * (Convert.ToDecimal(txtTestDiscount.Text) / 100))).ToString("0.00");
                }
            }
            txtDiscPer.Text = "0.00";
            txtDiscount.Text = "0.00";
            if (txtRate.Text != "" && txtQuantity.Text != "")
            {
                txtAmount.Text = Convert.ToDecimal(Convert.ToDecimal(txtQuantity.Text) * Convert.ToDecimal(txtRate.Text)).ToString("0.00");
            }
            CalculateBill();
        }

        protected void txtRate_TextChanged(object sender, EventArgs eventArgs)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txtQuantity = (TextBox)grdDetails.Rows[rowindex].FindControl("txtQuantity");
            TextBox txtRate = (TextBox)grdDetails.Rows[rowindex].FindControl("txtRate");
            TextBox txtAmount = (TextBox)grdDetails.Rows[rowindex].FindControl("txtAmount");
            TextBox txtActualRate = (TextBox)grdDetails.Rows[rowindex].FindControl("txtActualRate");
            TextBox txtTestDiscount = (TextBox)grdDetails.Rows[rowindex].FindControl("txtTestDiscount");

            if (txtRate.Text == "")
            {
                txtAmount.Text = "";
            }
            else if (txtActualRate.Text != "" && Convert.ToDecimal(txtRate.Text) > Convert.ToDecimal(txtActualRate.Text))
            {
                string msg = "alert(' Discounted Rate should be less than or equal to Actual Rate. ')";
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                txtTestDiscount.Text = "0.00";
                txtRate.Text = txtActualRate.Text;
            }
            txtDiscPer.Text = "0.00";
            txtDiscount.Text = "0.00";
            if (txtRate.Text != "" && txtQuantity.Text != "")
            {
                txtAmount.Text = Convert.ToDecimal(Convert.ToDecimal(txtQuantity.Text) * Convert.ToDecimal(txtRate.Text)).ToString("0.00");
            }
            CalculateBill();
        }
        #endregion

        protected void chkDiscount_CheckedChanged(object sender, EventArgs e)
        {
            txtDiscPer.Visible = false;
            txtDiscPer.Text = "0.00";
            txtDiscount.Text = "0.00";
            optLumpsum.Checked = false;
            optPercentage.Checked = false;

            if (chkDiscount.Checked)
            {
                optLumpsum.Visible = true;
                optPercentage.Visible = true;
            }
            else
            {
                optLumpsum.Visible = false;
                optPercentage.Visible = false;
            }
            CalculateBill();
        }

        public void ClearData()
        {
            txtBillNo.Text = "Create New.......";
            ddlSite.Items.Clear();
            txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            ddlStatus.SelectedValue = "Ok";
            txtAddress.Text = "";
            txtRecordNo.Text = "";
            txtWorkOrderNo.Text = "";
            grdDetails.DataSource = null;
            grdDetails.DataBind();
            AddRowBillDetail();
            optLumpsum.Visible = false;
            optPercentage.Visible = false;
            txtDiscPer.Visible = false;
            txtDiscPer.Enabled = false;
            txtDiscPer.Text = "0.00";
            optLumpsum.Checked = false;
            optPercentage.Checked = false;
            chkDiscount.Checked = false;
            txtDiscount.Text = "0.00";
            txtServiceTax.Text = "0.00";
            //txtEducationTax.Text = "0.00";
            //txtHigherEduTax.Text = "0.00";
            txtSwachhBharatTax.Text = "0.00";
            txtKisanKrishiTax.Text = "0.00";
            txtCGST.Text = "0.00";
            txtSGST.Text = "0.00";
            txtIGST.Text = "0.00";
            lnkRateAsPerCurrent.Text = "Load Tax As Per Current Setting";
            lnkRateAsPerCurrent.Visible = false;

            lblSerTaxPer.Visible = false;
            lblServiceTax.Visible = false;
            lblSwachhBharatTax.Visible = false;
            lblSwachhBharatTaxPer.Visible = false;
            lblKisanKrishiTax.Visible = false;
            lblKisanKrishiTaxPer.Visible = false;
            txtServiceTax.Visible = false;
            txtSwachhBharatTax.Visible = false;
            txtKisanKrishiTax.Visible = false;
            lblCGST.Visible = false;
            lblCGSTPer.Visible = false;
            lblSGST.Visible = false;
            lblSGSTPer.Visible = false;
            lblIGST.Visible = false;
            lblIGSTPer.Visible = false;
            txtCGST.Visible = false;
            txtSGST.Visible = false;
            txtIGST.Visible = false;

            txtRoundingOff.Text = "0.00";
            txtNet.Text = "0.00";
            txtAdvancePaid.Text = "0.00";
            lblMessage.Visible = false;
            lnkSave.Visible = true;
            chkAsPerClient.Visible = false;
            chkAsPerClient.Enabled = true;
            lblClientCouponSetting.Text = "";
            lblSiteSpecCoupStatus.Text = "";

        }

        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSite.Items.Clear();
            if (Convert.ToInt32(ddlClient.SelectedValue) != 0)
            {
                LoadSiteList(0);
                var address = dc.Client_View(Convert.ToInt32(ddlClient.SelectedValue), 0, "", "");
                foreach (var adds in address)
                {
                    txtAddress.Text = adds.CL_OfficeAddress_var;
                }
            }
        }

        private void LoadSiteList(int siteId)
        {
            var site = dc.Site_View(siteId, Convert.ToInt32(ddlClient.SelectedValue), 0, "");
            ddlSite.DataSource = site;
            ddlSite.DataTextField = "Site_Name_var";
            ddlSite.DataValueField = "Site_Id";
            ddlSite.DataBind();
            ddlSite.Items.Insert(0, new ListItem("----Select----", "0"));
        }

        private void DiscountOptionChanged()
        {
            txtDiscPer.Text = "0.00";
            txtDiscPer.Enabled = true;
            txtDiscPer.Visible = true;
            txtDiscount.Text = "0.00";
            txtDiscPer.Focus();
            CalculateBill();
            txtDiscPer.Attributes.Add("onfocus", "this.select();");
        }

        protected void optLumpsum_CheckedChanged(object sender, EventArgs eventArgs)
        {
            if (optLumpsum.Checked == true)
            {
                DiscountOptionChanged();
            }
        }

        protected void optPercentage_CheckedChanged(object sender, EventArgs eventArgs)
        {
            if (optPercentage.Checked == true)
            {
                DiscountOptionChanged();
            }
        }

        protected void txtDiscPer_TextChanged(object sender, EventArgs eventArgs)
        {

            if (txtTotal.Text == "0.00")
                txtDiscPer.Text = "0.00";

            if (txtDiscPer.Text != "0.00" & txtDiscPer.Text != "" & txtTotal.Text != "0.00")
            {
                if (optPercentage.Checked == true)
                {
                    if (Convert.ToDecimal(txtDiscPer.Text) > 90)
                    {
                        string msg = "alert(' Discount Value should not be greater than 90%')";
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                        txtDiscPer.Text = "0.00";
                    }
                }
                else if (optLumpsum.Checked == true)
                {
                    if (Convert.ToDecimal(txtDiscPer.Text) > Convert.ToDecimal(txtTotal.Text))
                    {
                        string msg = "alert(' Discount Value should not be greater than the SubTotal Amount')";
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                        txtDiscPer.Text = "0.00";
                    }
                }
            }
            CalculateBill();
        }

        public void CalculateBill()
        {
            //gross total
            decimal GrossAmount = 0;
            foreach (GridViewRow r in grdDetails.Rows)
            {
                TextBox textT = r.FindControl("txtAmount") as TextBox;
                decimal number = 0; ;
                if (decimal.TryParse(textT.Text, out number))
                {
                    GrossAmount += number;
                }
            }
            txtTotal.Text = GrossAmount.ToString("0.00");
            //discount
            if (chkDiscount.Checked == true && txtDiscPer.Text != "")
            {
                if (optPercentage.Checked == true)
                {
                    txtDiscount.Text = (GrossAmount * (Convert.ToDecimal(txtDiscPer.Text) / 100)).ToString("0.00");
                }
                else if (optLumpsum.Checked == true)
                {
                    txtDiscount.Text = Convert.ToDecimal(txtDiscPer.Text).ToString("0.00");
                }
            }
            else
            {
                txtDiscount.Text = "0.00";
            }
            if (lblSerTaxPer.Text == "")
                lblSerTaxPer.Text = "0.00";
            //Sertax
            txtServiceTax.Text = Convert.ToDecimal((GrossAmount - Convert.ToDecimal(txtDiscount.Text)) * Convert.ToDecimal(lblSerTaxPer.Text.Replace("(", "").Replace("%)", "")) / 100).ToString("0.00");

            //swachh bharat tax
            if (lblSwachhBharatTaxPer.Text == "")
                lblSwachhBharatTaxPer.Text = "0.00";
            txtSwachhBharatTax.Text = Convert.ToDecimal((GrossAmount - Convert.ToDecimal(txtDiscount.Text)) * Convert.ToDecimal(lblSwachhBharatTaxPer.Text.Replace("(", "").Replace("%)", "")) / 100).ToString("0.00");

            //KisanKrishi tax
            if (lblKisanKrishiTaxPer.Text == "")
                lblKisanKrishiTaxPer.Text = "0.00";
            txtKisanKrishiTax.Text = Convert.ToDecimal((GrossAmount - Convert.ToDecimal(txtDiscount.Text)) * Convert.ToDecimal(lblKisanKrishiTaxPer.Text.Replace("(", "").Replace("%)", "")) / 100).ToString("0.00");

            ////Edu Tax        
            //txtEducationTax.Text = Convert.ToDecimal(Convert.ToDecimal(txtServiceTax.Text) * 2 / 100).ToString("0.00");

            ////Second higher secondaru edu tax
            //txtHigherEduTax.Text = Convert.ToDecimal(Convert.ToDecimal(txtServiceTax.Text) * 1 / 100).ToString("0.00");

            //CGSTax
            if (lblCGSTPer.Text == "")
                lblCGSTPer.Text = "0.00";
            txtCGST.Text = Convert.ToDecimal((GrossAmount - Convert.ToDecimal(txtDiscount.Text)) * Convert.ToDecimal(lblCGSTPer.Text.Replace("(", "").Replace("%)", "")) / 100).ToString("0.00");
            //SGSTax
            if (lblSGSTPer.Text == "")
                lblSGSTPer.Text = "0.00";
            txtSGST.Text = Convert.ToDecimal((GrossAmount - Convert.ToDecimal(txtDiscount.Text)) * Convert.ToDecimal(lblSGSTPer.Text.Replace("(", "").Replace("%)", "")) / 100).ToString("0.00");
            //IGSTax
            if (lblIGSTPer.Text == "")
                lblIGSTPer.Text = "0.00";
            txtIGST.Text = Convert.ToDecimal((GrossAmount - Convert.ToDecimal(txtDiscount.Text)) * Convert.ToDecimal(lblIGSTPer.Text.Replace("(", "").Replace("%)", "")) / 100).ToString("0.00");

            //Gross
            //txtNet.Text = (GrossAmount - Convert.ToDecimal(txtDiscount.Text) + Convert.ToDecimal(txtServiceTax.Text) + Convert.ToDecimal(txtEducationTax.Text) + Convert.ToDecimal(txtHigherEduTax.Text)).ToString();
            txtNet.Text = (GrossAmount - Convert.ToDecimal(txtDiscount.Text) + Convert.ToDecimal(txtServiceTax.Text) + Convert.ToDecimal(txtSwachhBharatTax.Text) + Convert.ToDecimal(txtKisanKrishiTax.Text) + Convert.ToDecimal(txtCGST.Text) + Convert.ToDecimal(txtSGST.Text) + Convert.ToDecimal(txtIGST.Text)).ToString();

            //Round set
            txtNet.Text = Math.Round(Convert.ToDecimal(txtNet.Text)).ToString("0.00");

            //Round
            //decimal varResultGrossAmt = (GrossAmount - Convert.ToDecimal(txtDiscount.Text)) + Convert.ToDecimal(txtServiceTax.Text) + Convert.ToDecimal(txtEducationTax.Text) + Convert.ToDecimal(txtHigherEduTax.Text);
            decimal varResultGrossAmt = (GrossAmount - Convert.ToDecimal(txtDiscount.Text)) + Convert.ToDecimal(txtServiceTax.Text) + Convert.ToDecimal(txtSwachhBharatTax.Text) + Convert.ToDecimal(txtKisanKrishiTax.Text) + Convert.ToDecimal(txtCGST.Text) + Convert.ToDecimal(txtSGST.Text) + Convert.ToDecimal(txtIGST.Text);
            if (Convert.ToDecimal(txtNet.Text) > varResultGrossAmt)
                txtRoundingOff.Text = "+" + (Convert.ToDecimal(txtNet.Text) - varResultGrossAmt).ToString("0.00");
            else
                txtRoundingOff.Text = (Convert.ToDecimal(txtNet.Text) - varResultGrossAmt).ToString("0.00");

        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            //var bill = dc.Bill_View(txtBillNo.Text, 0, 0, "", 0, false, false, null, null).ToList();
            //if (bill.FirstOrDefault().BILL_PrintLock_bit == false)
            //{
                printBill(txtBillNo.Text);
            //}
        }
        private void printBill(string billNo)
        {
            BillUpdation bill = new BillUpdation();
            bill.getBillPrintString(billNo, false);
        }
        public void LoadBillDetail()
        {
            string BillNumber = lblBillId.Text;
            int i = 0;
            lblReason.Visible = true;
            txtReason.Visible = true;

            var bill = dc.Bill_View(BillNumber, 0, 0, "", 0, false, false, null, null);
            foreach (var b in bill)
            {
                LoadClientList(Convert.ToInt32(b.BILL_CL_Id));
                ddlClient.SelectedValue = b.BILL_CL_Id.ToString();
                LoadSiteList(Convert.ToInt32(b.BILL_SITE_Id));
                ddlSite.SelectedValue = b.BILL_SITE_Id.ToString();
                txtAddress.Text = b.CL_OfficeAddress_var;
                txtBillNo.Text = b.BILL_Id.ToString();
                txtDate.Text = Convert.ToDateTime(b.BILL_Date_dt).ToString("dd/MM/yyyy");
                txtWorkOrderNo.Text = b.BILL_WorkOrderNo_var;
                txtRecordNo.Text = b.BILL_RecordNo_int.ToString();
                txtRecordType.Text = b.BILL_RecordType_var;
                txtReason.Text = b.BILL_Reason_var;
                lblBillingPeriod.Text = b.BILL_BillingPeriod_var;
                if (b.BILL_Status_bit == true)
                    ddlStatus.SelectedValue = "Cancel";
                else
                    ddlStatus.SelectedValue = "Ok";

                txtDiscount.Text = Convert.ToDecimal(b.BILL_DiscountAmt_num).ToString("0.00");
                if (txtDiscount.Text != "0.00" && txtDiscount.Text != "")
                {
                    chkDiscount.Checked = true;
                    txtDiscPer.Text = b.BILL_Discount_num.ToString();
                    txtDiscPer.Enabled = true;
                    txtDiscPer.Visible = true;
                    optPercentage.Visible = true;
                    optLumpsum.Visible = true;

                    if (b.BILL_DiscountPerStatus_bit == false)
                        optLumpsum.Checked = true;
                    else if (b.BILL_DiscountPerStatus_bit == true)
                        optPercentage.Checked = true;
                    txtDiscount.Visible = true;
                    chkDiscount.Visible = true;
                }
                else
                {
                    optPercentage.Checked = false;
                    optLumpsum.Checked = false;
                    optPercentage.Visible = false;
                    optLumpsum.Visible = false;
                    txtDiscPer.Visible = false;
                    txtDiscPer.Text = "0.00";
                    txtDiscPer.Enabled = false;
                    txtDiscount.Visible = false;
                    chkDiscount.Visible = false;
                }

                if (b.BILL_SerTax_num != null && b.BILL_SerTax_num != 0)
                {
                    lblSerTaxPer.Visible = true;
                    lblServiceTax.Visible = true;
                    txtServiceTax.Visible = true;
                }
                else
                {
                    lblSerTaxPer.Visible = false;
                    lblServiceTax.Visible = false;
                    txtServiceTax.Visible = false;
                }
                if (b.BILL_SwachhBharatTax_num != null && b.BILL_SwachhBharatTax_num != 0)
                {
                    lblSwachhBharatTax.Visible = true;
                    lblSwachhBharatTaxPer.Visible = true;
                    txtSwachhBharatTax.Visible = true;
                }
                else
                {
                    lblSwachhBharatTax.Visible = false;
                    lblSwachhBharatTaxPer.Visible = false;
                    txtSwachhBharatTax.Visible = false;
                }
                if (b.BILL_KisanKrishiTax_num != null && b.BILL_KisanKrishiTax_num != 0)
                {
                    lblKisanKrishiTax.Visible = true;
                    lblKisanKrishiTaxPer.Visible = true;
                    txtKisanKrishiTax.Visible = true;
                }
                else
                {
                    lblKisanKrishiTax.Visible = false;
                    lblKisanKrishiTaxPer.Visible = false;
                    txtKisanKrishiTax.Visible = false;
                }

                if (b.BILL_CGST_num != null && b.BILL_CGST_num != 0)
                {
                    lblCGST.Visible = true;
                    lblCGSTPer.Visible = true;
                    txtCGST.Visible = true;
                }
                else
                {
                    lblCGST.Visible = false;
                    lblCGSTPer.Visible = false;
                    txtCGST.Visible = false;
                }
                if (b.BILL_SGST_num != null && b.BILL_SGST_num != 0)
                {
                    lblSGST.Visible = true;
                    lblSGSTPer.Visible = true;
                    txtSGST.Visible = true;
                }
                else
                {
                    lblSGST.Visible = false;
                    lblSGSTPer.Visible = false;
                    txtSGST.Visible = false;
                }
                if (b.BILL_IGST_num != null && b.BILL_IGST_num != 0)
                {
                    lblIGST.Visible = true;
                    lblIGSTPer.Visible = true;
                    txtIGST.Visible = true;
                }
                else
                {
                    lblIGST.Visible = false;
                    lblIGSTPer.Visible = false;
                    txtIGST.Visible = false;
                }

                if (b.BILL_SerTax_num != null)
                    lblSerTaxPer.Text = "(" + b.BILL_SerTax_num.ToString() + "%)";
                else
                    lblSerTaxPer.Text = "(" + "0" + "%)";

                if (b.BILL_SwachhBharatTax_num != null)
                    lblSwachhBharatTaxPer.Text = "(" + b.BILL_SwachhBharatTax_num.ToString() + "%)";
                else
                    lblSwachhBharatTaxPer.Text = "(" + "0" + "%)";

                if (b.BILL_KisanKrishiTax_num != null)
                    lblKisanKrishiTaxPer.Text = "(" + b.BILL_KisanKrishiTax_num.ToString() + "%)";
                else
                    lblKisanKrishiTaxPer.Text = "(" + "0" + "%)";

                if (b.BILL_CGST_num != null)
                    lblCGSTPer.Text = "(" + b.BILL_CGST_num.ToString() + "%)";
                else
                    lblCGSTPer.Text = "(" + "0" + "%)";

                if (b.BILL_SGST_num != null)

                    lblSGSTPer.Text = "(" + b.BILL_SGST_num.ToString() + "%)";
                else
                    lblSGSTPer.Text = "(" + "0" + "%)";

                if (b.BILL_IGST_num != null)
                    lblIGSTPer.Text = "(" + b.BILL_IGST_num.ToString() + "%)";
                else
                    lblIGSTPer.Text = "(" + "0" + "%)";
                checkRateAsPerCurrentSetting();

                txtServiceTax.Text = Convert.ToDecimal(b.BILL_SerTaxAmt_num).ToString("0.00");
                txtSwachhBharatTax.Text = Convert.ToDecimal(b.BILL_SwachhBharatTaxAmt_num).ToString("0.00");
                txtKisanKrishiTax.Text = Convert.ToDecimal(b.BILL_KisanKrishiTaxAmt_num).ToString("0.00");
                txtCGST.Text = Convert.ToDecimal(b.BILL_CGSTAmt_num).ToString("0.00");
                txtSGST.Text = Convert.ToDecimal(b.BILL_SGSTAmt_num).ToString("0.00");
                txtIGST.Text = Convert.ToDecimal(b.BILL_IGSTAmt_num).ToString("0.00");

                txtRoundingOff.Text = Convert.ToDecimal(b.BILL_RoundOffAmt_num).ToString("0.00");
                txtNet.Text = Convert.ToDecimal(b.BILL_NetAmt_num).ToString("0.00");
                if (b.BILL_AdvancePaid_num != null)
                    txtAdvancePaid.Text = Convert.ToDecimal(b.BILL_AdvancePaid_num).ToString("0.00");
            }
            //var billDetail = dc.BillDetail_View(BillNumber);
            var billDetail = dc.BillDetailMonthly_View(BillNumber);
            foreach (var bd in billDetail)
            {
                AddRowBillDetail();
                DropDownList ddlSubTest = (DropDownList)grdDetails.Rows[i].Cells[3].FindControl("ddlSubTest");
                Label lblSubTestId = (Label)grdDetails.Rows[i].Cells[3].FindControl("lblSubTestId");
                DropDownList ddl_Test = (DropDownList)grdDetails.Rows[i].Cells[3].FindControl("ddl_Test");
                Label lblTestId = (Label)grdDetails.Rows[i].Cells[3].FindControl("lblTestId");
                TextBox txtDescription = (TextBox)grdDetails.Rows[i].FindControl("txtDescription");
                TextBox txtReferenceNo = (TextBox)grdDetails.Rows[i].FindControl("txtReferenceNo");
                Label lblReceivedDate = (Label)grdDetails.Rows[i].FindControl("lblReceivedDate");
                Label lblMaterialName = (Label)grdDetails.Rows[i].FindControl("lblMaterialName");
                Label lblDescription = (Label)grdDetails.Rows[i].FindControl("lblDescription");
                TextBox txtSACCode = (TextBox)grdDetails.Rows[i].FindControl("txtSACCode");
                TextBox txtQuantity = (TextBox)grdDetails.Rows[i].FindControl("txtQuantity");
                TextBox txtActualRate = (TextBox)grdDetails.Rows[i].FindControl("txtActualRate");
                TextBox txtTestDiscount = (TextBox)grdDetails.Rows[i].FindControl("txtTestDiscount");
                TextBox txtRate = (TextBox)grdDetails.Rows[i].FindControl("txtRate");
                TextBox txtAmount = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");
                
                txtReferenceNo.Text = bd.MBILLD_ReferenceNo_int.ToString();
                int refNo = 0;
                if (txtReferenceNo.Text != "" && (int.TryParse(txtReferenceNo.Text, out refNo)) == true)
                {
                    string recordType = "";
                    var inward = dc.Inward_View(Convert.ToInt32(txtReferenceNo.Text), 0, "", null, null);
                    foreach (var inwd in inward)
                    {
                        recordType = inwd.INWD_RecordType_var;
                    }
                    if (recordType == "")
                        recordType = "MS";
                    if (recordType == "OT")
                    {
                        var otherTest = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, 0);
                        ddlSubTest.DataSource = otherTest;
                        ddlSubTest.DataTextField = "TEST_Name_var";
                        ddlSubTest.DataValueField = "TEST_Id";
                        ddlSubTest.DataBind();
                        ddlSubTest.Items.Insert(0, new ListItem("---Select---", "0"));
                        ddlSubTest.Visible = true;

                        var subtest = dc.Test_View(0, Convert.ToInt32(bd.MBILLD_TEST_Id), "", 0, 0, 0);
                        foreach (var t in subtest)
                        {
                            if (t.TEST_SubTest_Id > 0)
                            {
                                if (ddlSubTest.Items.FindByValue(t.TEST_SubTest_Id.ToString()) != null)
                                    ddlSubTest.SelectedValue = t.TEST_SubTest_Id.ToString();

                                lblSubTestId.Text = t.TEST_SubTest_Id.ToString();
                                break;
                            }
                        }
                        if (ddlSubTest.Items.Count > 1 && lblSubTestId.Text == "")
                        {
                            ddlSubTest.SelectedIndex = 1;
                            lblSubTestId.Text = ddlSubTest.SelectedValue;
                        }
                        int SubTestId = 0;
                        if (lblSubTestId.Text != "")
                            SubTestId = Convert.ToInt32(lblSubTestId.Text);
                        if (lblSubTestId.Text == "0" || lblSubTestId.Text == "")
                        {
                            ddl_Test.DataSource = null;
                            ddl_Test.DataBind();
                        }
                        else
                        {
                            var test = dc.Test_View_ForBillModify(recordType, 0, SubTestId);
                            ddl_Test.DataSource = test;
                            ddl_Test.DataTextField = "TEST_Name_var";
                            ddl_Test.DataValueField = "TEST_Id";
                            ddl_Test.DataBind();
                            ddl_Test.Items.Insert(0, new ListItem("---Select---", "0"));

                            lblTestId.Text = bd.MBILLD_TEST_Id.ToString();
                            ddl_Test.SelectedValue = bd.MBILLD_TEST_Id.ToString();
                        }
                    }
                    else
                    {
                        var test = dc.Test_View_ForBillModify(recordType, 0, 0);
                        ddl_Test.DataSource = test;
                        ddl_Test.DataTextField = "TEST_Name_var";
                        ddl_Test.DataValueField = "TEST_Id";
                        ddl_Test.DataBind();
                        ddl_Test.Items.Insert(0, new ListItem("---Select---", "0"));

                        lblTestId.Text = bd.MBILLD_TEST_Id.ToString();
                        ddl_Test.SelectedValue = bd.MBILLD_TEST_Id.ToString();
                    }
                }

                txtDescription.Text = bd.MBILLD_TEST_Name_var;                
                lblReceivedDate.Text = bd.MBILLD_ReceivedDate_dt.ToString();
                lblMaterialName.Text = bd.MaterialName;
                lblDescription.Text = bd.MBILLD_Description_var; 
                txtSACCode.Text = bd.MBILLD_SACCode_var;
                txtQuantity.Text = Convert.ToDecimal(bd.MBILLD_Quantity_int).ToString("0.##");
                if (bd.MBILLD_ActualRate_num != null)
                    txtActualRate.Text = Convert.ToDecimal(bd.MBILLD_ActualRate_num).ToString("0.00");
                else
                    txtActualRate.Text = "0";
                if (bd.MBILLD_ActualRate_num != null)
                    txtTestDiscount.Text = Convert.ToDecimal(bd.MBILLD_DiscountPer_num).ToString("0.##");
                else
                    txtTestDiscount.Text = "0";
                txtRate.Text = Convert.ToDecimal(bd.MBILLD_Rate_num).ToString("0.00");
                txtAmount.Text = Convert.ToDecimal(bd.MBILLD_Amt_num).ToString("0.00");
                i++;
            }
            txtBillNo.ReadOnly = true;
            txtDate.ReadOnly = true;
            txtRecordNo.ReadOnly = true;
            txtRecordType.ReadOnly = true;
            CalendarExtender1.Enabled = false;
            if (grdDetails.Rows.Count == 0)
            {
                AddRowBillDetail();
            }
            CalculateBill();
        }

        public void checkRateAsPerCurrentSetting()
        {
            // check gst current setting
            //if (DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null) >= DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null))
            if (CheckGSTFlag(DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null)) == true)
            {
                bool cgstFlag = false, sgstFlag = false, igstFlag = false;
                int siteId = 0;
                if (ddlSite.SelectedIndex > 0)
                {
                    siteId = Convert.ToInt32(ddlSite.SelectedValue);
                }
                var site = dc.Site_View(siteId, 0, 0, "");
                foreach (var st in site)
                {
                    if (st.SITE_GST_bit != null)
                    {
                        if (st.SITE_SEZStatus_bit == false && st.SITE_State_var.ToLower() == "maharashtra")
                        {
                            cgstFlag = true;
                            sgstFlag = true;
                        }
                        else
                        {
                            igstFlag = true;
                        }
                    }
                    break;
                }
                if (cgstFlag == false && sgstFlag == false && igstFlag == false)
                {
                    cgstFlag = true;
                    sgstFlag = true;
                }
                string[] strDate = txtDate.Text.Split('/');
                DateTime BillDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

                decimal xCGST = 0, xSGST = 0, xIGST = 0;
                bool siteGSTFlag = false;
                var siteGst = dc.GST_Site_View(siteId);
                foreach (var GSTax in siteGst)
                {
                    if (cgstFlag == true)
                        xCGST = Convert.ToDecimal(GSTax.GSTSITE_CGST_dec);
                    if (sgstFlag == true)
                        xSGST = Convert.ToDecimal(GSTax.GSTSITE_SGST_dec);
                    if (igstFlag == true)
                        xIGST = Convert.ToDecimal(GSTax.GSTSITE_IGST_dec);
                    siteGSTFlag = true;
                    break;
                }
                if (siteGSTFlag == false)
                {
                    var master = dc.GST_View(1, BillDate);
                    foreach (var GSTax in master)
                    {
                        if (cgstFlag == true)
                            xCGST = Convert.ToDecimal(GSTax.GST_CGST_dec);
                        if (sgstFlag == true)
                            xSGST = Convert.ToDecimal(GSTax.GST_SGST_dec);
                        if (igstFlag == true)
                            xIGST = Convert.ToDecimal(GSTax.GST_IGST_dec);
                        break;
                    }
                }
                if (Convert.ToDecimal(lblCGSTPer.Text.Replace("(", "").Replace("%)", "")) != xCGST)
                    lnkRateAsPerCurrent.Visible = true;
                else if (Convert.ToDecimal(lblSGSTPer.Text.Replace("(", "").Replace("%)", "")) != xSGST)
                    lnkRateAsPerCurrent.Visible = true;
                else if (Convert.ToDecimal(lblIGSTPer.Text.Replace("(", "").Replace("%)", "")) != xIGST)
                    lnkRateAsPerCurrent.Visible = true;

                if ((cgstFlag == true && lblCGSTPer.Visible == false) || (cgstFlag == false && lblCGSTPer.Visible == true))
                    lnkRateAsPerCurrent.Visible = true;
                else if ((sgstFlag == true && lblSGSTPer.Visible == false) || (sgstFlag == false && lblSGSTPer.Visible == true))
                    lnkRateAsPerCurrent.Visible = true;
                else if ((igstFlag == true && lblIGSTPer.Visible == false) || (igstFlag == false && lblIGSTPer.Visible == true))
                    lnkRateAsPerCurrent.Visible = true;

            }
            ///
        }
        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            CalculateBill();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            bool updateBillFlag = true, billStatus = false;
            string TallyNarration = "";
            string BillNo = "0";

            CalculateBill();
            if (txtBillNo.Text != "Create New.......")
            {
                BillNo = txtBillNo.Text;
            }
            if (ddlStatus.SelectedValue == "Ok")
            {
                billStatus = false;
            }
            else
            {
                billStatus = true;
            }

            clsData clsDt = new clsData();
            //check recipt entry done
            if (BillNo != "0")
            {
                var rcpt = dc.CashDetail_View_bill(txtBillNo.Text).ToList();
                if (rcpt.Count > 0)
                {
                    string msg = "alert('Receipt has been added for this Bill Number  !'+ '\\n' +'It can not be modified ')";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                    updateBillFlag = false;
                }
            }
            //check gst information is updated or not
            else if (clsDt.checkGSTInfoUpdated(ddlClient.SelectedValue, ddlSite.SelectedValue, "") == false)
            {
                updateBillFlag = false;
                string msg = "alert('Please update client & site GST details. Can not generate bill.')";
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
            }
            else
            {
                int NewrecNo = 0;
                clsData clsObj = new clsData();
                NewrecNo = clsObj.GetCurrentRecordNo("BillNo");
                //var gstbillCount = dc.Bill_View("0", 0, 0, "", 0, false, false, DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                var gstbillCount = dc.Bill_View_Count(DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null), DateTime.Now);
                if (gstbillCount.Count() != NewrecNo - 1)
                {
                    updateBillFlag = false;
                    string msg = "alert('Bill No. mismatch. Can not generate bill.')";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                }
            }

            
            if (txtAdvancePaid.Text == "")
                txtAdvancePaid.Text = "0.00";
            if (updateBillFlag == true)
            {
                int temp = 0;
                for (int row = 0; row < grdDetails.Rows.Count; row++)
                {
                    TextBox txtReferenceNo = (TextBox)grdDetails.Rows[row].FindControl("txtReferenceNo");
                    Label lblReceivedDate = (Label)grdDetails.Rows[row].FindControl("lblReceivedDate");
                    Label lblMaterialName = (Label)grdDetails.Rows[row].FindControl("lblMaterialName");
                    Label lblDescription = (Label)grdDetails.Rows[row].FindControl("lblDescription");
                    if (lblMaterialName.Text == "" && lblReceivedDate.Text == ""
                        && txtReferenceNo.Text != "" && (int.TryParse(txtReferenceNo.Text, out temp)) == true)
                    {
                        int rowPrev = 0;
                        if (row > 0)
                            rowPrev = row - 1;
                        TextBox txtReferenceNoPrev = (TextBox)grdDetails.Rows[rowPrev].FindControl("txtReferenceNo");
                        Label lblReceivedDatePrev = (Label)grdDetails.Rows[rowPrev].FindControl("lblReceivedDate");
                        Label lblMaterialNamePrev = (Label)grdDetails.Rows[rowPrev].FindControl("lblMaterialName");
                        Label lblDescriptionPrev = (Label)grdDetails.Rows[rowPrev].FindControl("lblDescriptionPrev");

                        if (row > 0 && txtReferenceNo.Text.Trim() == txtReferenceNoPrev.Text.Trim())
                        {
                            lblReceivedDate.Text = lblReceivedDatePrev.Text;
                            lblMaterialName.Text = lblMaterialNamePrev.Text;
                            lblDescription.Text = lblDescriptionPrev.Text;
                        }
                        else
                        {
                            var inward = dc.Inward_View(Convert.ToInt32(txtReferenceNo.Text), 0, "", null, null);
                            foreach (var inwd in inward)
                            {
                                lblReceivedDate.Text = inwd.INWD_ReceivedDate_dt.ToString();
                                lblMaterialName.Text = inwd.MATERIAL_Name_var;
                            }
                        }
                    }

                    if (TallyNarration == "")
                    {
                        TallyNarration = lblMaterialName.Text;
                    }
                    else if (TallyNarration.Contains(lblMaterialName.Text) == false)
                    {
                        TallyNarration = TallyNarration + ", " + lblMaterialName.Text;
                    }
                }
                TallyNarration = TallyNarration.ToUpper();
                //
                dc.BillDetail_Update(BillNo, 0, 0, 0, "", 0, "", 0, 0, false,0, true);
                dc.BillDetailMonthly_Update(BillNo, 0, 0, 0, "", 0, "", 0, null, "", 0, 0, false, 0, true,"");
                
                bool insertBill = false;
                if (BillNo == "0")
                {
                    int NewrecNo = 0;
                    clsData clsObj = new clsData();
                    NewrecNo = clsObj.GetnUpdateRecordNo("BillNo");
                    var master = dc.MasterSetting_View(0);
                    foreach (var mst in master)
                    {
                        BillNo = mst.MASTER_AccountingYear_var + mst.MASTER_Region_var + NewrecNo.ToString();
                    }
                    insertBill = true;
                }
                //BillNo = 
                dc.Bill_Update(BillNo, Convert.ToInt32(ddlClient.SelectedValue), ddlClient.SelectedItem.Text, Convert.ToInt32(ddlSite.SelectedValue), optPercentage.Checked, Convert.ToDecimal(txtDiscPer.Text), Convert.ToDecimal(txtDiscount.Text), Convert.ToDecimal(lblSerTaxPer.Text.Replace("(", "").Replace("%)", "")),
                    Convert.ToDecimal(txtServiceTax.Text), Convert.ToDecimal(lblSwachhBharatTaxPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtSwachhBharatTax.Text), Convert.ToDecimal(lblKisanKrishiTaxPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtKisanKrishiTax.Text),
                    Convert.ToDecimal(lblCGSTPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtCGST.Text), Convert.ToDecimal(lblSGSTPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtSGST.Text), Convert.ToDecimal(lblIGSTPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtIGST.Text), Convert.ToDecimal(txtAdvancePaid.Text),
                    Convert.ToDecimal(txtNet.Text), 0, 0, Convert.ToDecimal(txtRoundingOff.Text), txtRecordType.Text, Convert.ToInt32(txtRecordNo.Text), billStatus, TallyNarration,
                    false, Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), Convert.ToBoolean(lblCouponBill.Text), txtWorkOrderNo.Text.Trim(), false, txtReason.Text, insertBill);
                txtBillNo.Text = BillNo.ToString();
                bool foundFlag = false;
                int iDetail = 0;
                string[,] arrTest = new string[100, 8];
                foreach (GridViewRow row in grdDetails.Rows)
                {
                    Label lblTestId = (Label)row.FindControl("lblTestId");
                    DropDownList ddl_Test = (DropDownList)row.FindControl("ddl_Test");
                    TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
                    TextBox txtReferenceNo = (TextBox)row.FindControl("txtReferenceNo");
                    Label lblReceivedDate = (Label)row.FindControl("lblReceivedDate");
                    Label lblMaterialName = (Label)row.FindControl("lblMaterialName");
                    Label lblDescription = (Label)row.FindControl("lblDescription");
                    TextBox txtSACCode = (TextBox)row.FindControl("txtSACCode");
                    TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
                    TextBox txtActualRate = (TextBox)row.FindControl("txtActualRate");
                    TextBox txtTestDiscount = (TextBox)row.FindControl("txtTestDiscount");
                    TextBox txtRate = (TextBox)row.FindControl("txtRate");
                    TextBox txtAmount = (TextBox)row.FindControl("txtAmount");

                    DateTime? ReceivedDate = null;
                    if (lblReceivedDate.Text != "")
                    {
                        ReceivedDate = Convert.ToDateTime(lblReceivedDate.Text);
                    }
                    
                    lblTestId.Text = "0";
                    lblTestId.Text = ddl_Test.SelectedValue;                    
                    var b = dc.BillDetailMonthly_Update(BillNo, Convert.ToInt32(row.Cells[2].Text), Convert.ToDecimal(txtQuantity.Text), Convert.ToDecimal(txtAmount.Text), txtDescription.Text, Convert.ToDecimal(txtRate.Text), txtSACCode.Text, Convert.ToInt32(txtReferenceNo.Text), ReceivedDate, lblBillingPeriod.Text, Convert.ToDecimal(txtActualRate.Text), Convert.ToDecimal(txtTestDiscount.Text), false, Convert.ToInt32(lblTestId.Text), false,lblDescription.Text);

                    foundFlag = false;
                    for (int j = 0; j <= iDetail; j++)
                    {
                        if (arrTest[j, 3] == txtReferenceNo.Text)
                        {
                            arrTest[j, 1] = (Convert.ToDecimal(arrTest[j, 1]) + Convert.ToDecimal(txtAmount.Text)).ToString();
                            foundFlag = true;
                        }
                    }
                    if (foundFlag == false)
                    {
                        arrTest[iDetail, 0] = lblMaterialName.Text;
                        arrTest[iDetail, 1] = txtAmount.Text;
                        arrTest[iDetail, 2] = txtSACCode.Text;
                        arrTest[iDetail, 3] = txtReferenceNo.Text;
                        arrTest[iDetail, 4] = lblReceivedDate.Text;
                        arrTest[iDetail, 5] = txtActualRate.Text;
                        arrTest[iDetail, 6] = txtTestDiscount.Text;
                        arrTest[iDetail, 7] = lblDescription.Text;
                        iDetail = iDetail + 1;
                    }
                }
                for (int j = 0; j <= iDetail; j++)
                {
                    //DateTime receivedDate = DateTime.ParseExact(arrTest[j, 4], "dd/MM/yyyy", null);
                    DateTime? ReceivedDate = null;
                    if (arrTest[j, 4] != "")
                    {
                        ReceivedDate = Convert.ToDateTime(arrTest[j, 4]);
                    }
                    if (arrTest[j, 1] != "" && arrTest[j, 1] != null)
                    {
                        dc.BillDetail_Update_Monthly(BillNo, j + 1, 1, Convert.ToDecimal(arrTest[j, 1]), arrTest[j, 0], Convert.ToDecimal(arrTest[j, 1]), arrTest[j, 2], Convert.ToInt32(arrTest[j, 3]), ReceivedDate, lblBillingPeriod.Text, Convert.ToDecimal(arrTest[j, 5]), Convert.ToDecimal(arrTest[j, 6]), false);
                    }
                }
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", "alert('Records Sucessfully Updated ')", true);
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Records Sucessfully Updated";
                lblMsg.ForeColor = Color.Green; 
                //lnkSave.Visible = false;
                //lnkPrint.Visible = true;
                lnkSave.Enabled = false;
            }
        }

        protected void imgClosePopup_Click(object sender, EventArgs e)
        {
            //Response.Redirect("BillStatus.aspx");
            Response.Redirect("MonthlyBilling.aspx");
        }

        protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadServiceTax();
            LoadSwachhBharatTax();
            LoadKisanKrishiTax();
            LoadGSTax();
            CalculateBill();
        }

        private void LoadServiceTax()
        {
            lblSerTaxPer.Text = "";
            lblSerTaxPer.Visible = false;
            lblServiceTax.Visible = false;
            txtServiceTax.Visible = false;
            //if (DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null) < DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null))
            if (CheckGSTFlag(DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null)) == false)
            {
                int siteId = 0;
                if (ddlSite.SelectedIndex > 0)
                {
                    siteId = Convert.ToInt32(ddlSite.SelectedValue);
                }
                var master = dc.MasterSetting_View(siteId);
                decimal xSrvTax = 0;
                foreach (var serTax in master)
                {
                    xSrvTax = Convert.ToDecimal(serTax.MASTER_ServiceTax_num);
                }
                lblSerTaxPer.Text = "(" + xSrvTax.ToString() + "%)";
                lblSerTaxPer.Visible = true;
                lblServiceTax.Visible = true;
                txtServiceTax.Visible = true;
            }
        }

        private void LoadSwachhBharatTax()
        {
            lblSwachhBharatTaxPer.Text = "";
            lblSwachhBharatTaxPer.Visible = false;
            lblSwachhBharatTax.Visible = false;
            txtSwachhBharatTax.Visible = false;
            //if (DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null) < DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null))
            if (CheckGSTFlag(DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null)) == false)
            {
                int siteId = 0;
                if (ddlSite.SelectedIndex > 0)
                {
                    siteId = Convert.ToInt32(ddlSite.SelectedValue);
                }
                var master = dc.MasterSetting_View(siteId);
                decimal xSwachhBharat = 0;
                //xSwachhBharat = Convert.ToDecimal(master.FirstOrDefault().MASTER_SwachhBharatTax_num);
                foreach (var serTax in master)
                {
                    xSwachhBharat = Convert.ToDecimal(serTax.MASTER_SwachhBharatTax_num);
                }
                lblSwachhBharatTaxPer.Text = "(" + xSwachhBharat.ToString() + "%)";
                lblSwachhBharatTaxPer.Visible = true;
                lblSwachhBharatTax.Visible = true;
                txtSwachhBharatTax.Visible = true;
            }
        }

        private void LoadKisanKrishiTax()
        {
            lblKisanKrishiTaxPer.Text = "";
            lblKisanKrishiTaxPer.Visible = false;
            lblKisanKrishiTax.Visible = false;
            txtKisanKrishiTax.Visible = false;
            //if (DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null) < DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null))
            if (CheckGSTFlag(DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null)) == false)
            {
                int siteId = 0;
                if (ddlSite.SelectedIndex > 0)
                {
                    siteId = Convert.ToInt32(ddlSite.SelectedValue);
                }
                var master = dc.MasterSetting_View(siteId);
                decimal xKisanKrishi = 0;
                //xKisanKrishi = Convert.ToDecimal(master.FirstOrDefault().MASTER_KisanKrishiTax_num);
                foreach (var serTax in master)
                {
                    xKisanKrishi = Convert.ToDecimal(serTax.MASTER_KisanKrishiTax_num);
                }
                lblKisanKrishiTaxPer.Text = "(" + xKisanKrishi.ToString() + "%)";
                lblKisanKrishiTaxPer.Visible = true;
                lblKisanKrishiTax.Visible = true;
                txtKisanKrishiTax.Visible = true;
            }
        }

        private void LoadGSTax()
        {
            lblCGSTPer.Text = "";
            lblSGSTPer.Text = "";
            lblIGSTPer.Text = "";
            lblCGSTPer.Visible = false;
            lblCGST.Visible = false;
            lblSGSTPer.Visible = false;
            lblSGST.Visible = false;
            lblIGSTPer.Visible = false;
            lblIGST.Visible = false;
            txtCGST.Visible = false;
            txtSGST.Visible = false;
            txtIGST.Visible = false;
            txtCGST.Text = "";
            txtSGST.Text = "";
            txtIGST.Text = "";

            //if (DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null) >= DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null))
            if (CheckGSTFlag(DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null)) == true)
            {
                bool cgstFlag = false, sgstFlag = false, igstFlag = false;
                int siteId = 0;
                if (ddlSite.SelectedIndex > 0)
                {
                    siteId = Convert.ToInt32(ddlSite.SelectedValue);
                }
                var site = dc.Site_View(siteId, 0, 0, "");
                foreach (var st in site)
                {
                    if (st.SITE_GST_bit != null)
                    {
                        if (st.SITE_SEZStatus_bit == false && st.SITE_State_var.ToLower() == "maharashtra")
                        {
                            cgstFlag = true;
                            sgstFlag = true;
                            lblCGSTPer.Visible = true;
                            lblCGST.Visible = true;
                            lblSGSTPer.Visible = true;
                            lblSGST.Visible = true;
                            txtCGST.Visible = true;
                            txtSGST.Visible = true;
                        }
                        else
                        {
                            igstFlag = true;
                            lblIGSTPer.Visible = true;
                            lblIGST.Visible = true;
                            txtIGST.Visible = true;
                        }
                    }
                    break;
                }
                if (cgstFlag == false && sgstFlag == false && igstFlag == false)
                {
                    cgstFlag = true;
                    sgstFlag = true;
                    lblCGSTPer.Visible = true;
                    lblCGST.Visible = true;
                    lblSGSTPer.Visible = true;
                    lblSGST.Visible = true;
                    txtCGST.Visible = true;
                    txtSGST.Visible = true;
                }
                string[] strDate = txtDate.Text.Split('/');
                DateTime BillDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

                decimal xCGST = 0, xSGST = 0, xIGST = 0;
                bool siteGSTFlag = false;
                var siteGst = dc.GST_Site_View(siteId);
                foreach (var GSTax in siteGst)
                {
                    xCGST = Convert.ToDecimal(GSTax.GSTSITE_CGST_dec);
                    xSGST = Convert.ToDecimal(GSTax.GSTSITE_SGST_dec);
                    xIGST = Convert.ToDecimal(GSTax.GSTSITE_IGST_dec);
                    siteGSTFlag = true;
                    break;
                }
                if (siteGSTFlag == false)
                {
                    var master = dc.GST_View(1, BillDate);
                    foreach (var GSTax in master)
                    {
                        xCGST = Convert.ToDecimal(GSTax.GST_CGST_dec);
                        xSGST = Convert.ToDecimal(GSTax.GST_SGST_dec);
                        xIGST = Convert.ToDecimal(GSTax.GST_IGST_dec);
                        break;
                    }
                }
                if (cgstFlag == true)
                    lblCGSTPer.Text = "(" + xCGST.ToString() + "%)";
                if (sgstFlag == true)
                    lblSGSTPer.Text = "(" + xSGST.ToString() + "%)";
                if (igstFlag == true)
                    lblIGSTPer.Text = "(" + xIGST.ToString() + "%)";
            }
        }

        protected void lnkRateAsPerCurrent_Click(object sender, EventArgs e)
        {
            if (lnkRateAsPerCurrent.Text == "Calculate Tax As Per Current Setting")
            {
                lnkRateAsPerCurrent.Text = "Calculate Tax As Per Bill Old Setting";
                LoadGSTax();
            }
            else
            {
                lnkRateAsPerCurrent.Text = "Calculate Tax As Per Current Setting";
                lblCGSTPer.Text = "";
                lblSGSTPer.Text = "";
                lblIGSTPer.Text = "";
                txtCGST.Text = "";
                txtSGST.Text = "";
                txtIGST.Text = "";
                var bill = dc.Bill_View(txtBillNo.Text, 0, 0, "", 0, false, false, null, null);
                foreach (var b in bill)
                {

                    if (b.BILL_CGST_num != null && b.BILL_CGST_num != 0)
                    {
                        lblCGST.Visible = true;
                        lblCGSTPer.Visible = true;
                        txtCGST.Visible = true;
                    }
                    else
                    {
                        lblCGST.Visible = false;
                        lblCGSTPer.Visible = false;
                        txtCGST.Visible = false;
                    }
                    if (b.BILL_SGST_num != null && b.BILL_SGST_num != 0)
                    {
                        lblSGST.Visible = true;
                        lblSGSTPer.Visible = true;
                        txtSGST.Visible = true;
                    }
                    else
                    {
                        lblSGST.Visible = false;
                        lblSGSTPer.Visible = false;
                        txtSGST.Visible = false;
                    }
                    if (b.BILL_IGST_num != null && b.BILL_IGST_num != 0)
                    {
                        lblIGST.Visible = true;
                        lblIGSTPer.Visible = true;
                        txtIGST.Visible = true;
                    }
                    else
                    {
                        lblIGST.Visible = false;
                        lblIGSTPer.Visible = false;
                        txtIGST.Visible = false;
                    }

                    if (b.BILL_CGST_num != null)
                        lblCGSTPer.Text = "(" + b.BILL_CGST_num.ToString() + "%)";
                    else
                        lblCGSTPer.Text = "(" + "0" + "%)";

                    if (b.BILL_SGST_num != null)
                        lblSGSTPer.Text = "(" + b.BILL_SGST_num.ToString() + "%)";
                    else
                        lblSGSTPer.Text = "(" + "0" + "%)";

                    if (b.BILL_IGST_num != null)
                        lblIGSTPer.Text = "(" + b.BILL_IGST_num.ToString() + "%)";
                    else
                        lblIGSTPer.Text = "(" + "0" + "%)";

                }
            }

            CalculateBill();
        }

        private bool CheckGSTFlag(DateTime BillDate)
        {
            bool gstFlag = false;
            //string[] strDate = txtDate.Text.Split('/');
            //DateTime BillDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            var master = dc.GST_View(1, BillDate);
            if (master.Count() > 0)
            {
                gstFlag = true;
            }
            else
            {
                gstFlag = false;
            }
            return gstFlag;
        }

        protected void ddlSubTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int rowindex = row.RowIndex;

            if (ddl.SelectedIndex > 0)
            {
                DropDownList ddl_Test = (DropDownList)grdDetails.Rows[rowindex].FindControl("ddl_Test");
                int subTestId = Convert.ToInt32(ddl.SelectedValue);
                ddl_Test.Items.Clear();
                var test = dc.Test_View_ForBillModify("", 0, subTestId);
                ddl_Test.DataSource = test;
                ddl_Test.DataTextField = "TEST_Name_var";
                ddl_Test.DataValueField = "TEST_Id";
                ddl_Test.DataBind();
                ddl_Test.Items.Insert(0, new ListItem("---Select---", "0"));
                
            }
        }

      
    }
}