using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class Bill : System.Web.UI.Page
    {

        static string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
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
                        rdoCT.Visible = false;
                        rdoST.Visible = false;
                        LoadBillDetail();
                        lblAdvanceRcpt.Visible = false;
                        txtAdvRecpt.Visible = false;
                        txtAdvRecptBal.Visible = false;
                    }
                    else
                    {
                        ClearData();
                        LoadClientList();
                        //AddRowBillDetail();
                        txtRecordNo.Text = "0";
                        txtRecordType.Text = "---";
                        lblMaterialId.Text = "0";
                        rdoST.Visible = true;
                        rdoCT.Visible = true;
                    }
                    lnkSave.Visible = true;
                    lnkPrint.Visible = false;
                }
            }
        }

        private void LoadClientList()
        {
            var cl = dc.Client_View(0, 0, "", "");
            ddlClient.DataSource = cl;
            ddlClient.DataTextField = "CL_Name_var";
            ddlClient.DataValueField = "CL_Id";
            ddlClient.DataBind();
            ddlClient.Items.Insert(0, new ListItem("-----Select----", "0"));
        }

        //private void LoadOtherSubTestList()
        //{
        //    var otherTest = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, 0);
        //    ViewState["OtherSubTestList"] = otherTest;
        //}
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
                dt.Columns.Add(new DataColumn("txtSACCode", typeof(string)));
                dt.Columns.Add(new DataColumn("txtQuantity", typeof(string)));
                dt.Columns.Add(new DataColumn("txtActualRate", typeof(string)));
                dt.Columns.Add(new DataColumn("txtTestDiscount", typeof(string)));
                dt.Columns.Add(new DataColumn("txtRate", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAmount", typeof(string)));
                dt.Columns.Add(new DataColumn("lblExist", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["lblSubTestId"] = string.Empty;
            dr["ddlSubTest"] = string.Empty;
            dr["lblTestId"] = string.Empty;            
            dr["ddl_Test"] = string.Empty;
            dr["txtDescription"] = string.Empty;
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
            dr["lblExist"] = "False";
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
                TextBox box1 = (TextBox)grdDetails.Rows[i].FindControl("txtDescription");
                TextBox box2 = (TextBox)grdDetails.Rows[i].FindControl("txtSACCode");
                TextBox box3 = (TextBox)grdDetails.Rows[i].FindControl("txtQuantity");

                TextBox txtActualRate = (TextBox)grdDetails.Rows[i].FindControl("txtActualRate");
                TextBox txtTestDiscount = (TextBox)grdDetails.Rows[i].FindControl("txtTestDiscount");

                TextBox box4 = (TextBox)grdDetails.Rows[i].FindControl("txtRate");
                TextBox box5 = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");
                Label lblExist = (Label)grdDetails.Rows[i].FindControl("lblExist");

                grdDetails.Rows[i].Cells[2].Text = (i + 1).ToString();
                if (txtRecordType.Text == "OT")
                {
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
                            var test = dc.Test_View_ForBillModify(txtRecordType.Text, Convert.ToInt32(lblMaterialId.Text), SubTestId);
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
                    lblTestId.Text = dt.Rows[i]["lblTestId"].ToString();
                    ddl_Test.SelectedValue = dt.Rows[i]["ddl_Test"].ToString();
                }
                
                box1.Text = dt.Rows[i]["txtDescription"].ToString();
                box2.Text = dt.Rows[i]["txtSACCode"].ToString();
                box3.Text = dt.Rows[i]["txtQuantity"].ToString();

                txtActualRate.Text = dt.Rows[i]["txtActualRate"].ToString();
                txtTestDiscount.Text = dt.Rows[i]["txtTestDiscount"].ToString();
                if (txtActualRate.Text == "")
                    txtActualRate.Text = "0";
                if (txtTestDiscount.Text == "")
                    txtTestDiscount.Text = "0";
                box4.Text = dt.Rows[i]["txtRate"].ToString();
                box5.Text = dt.Rows[i]["txtAmount"].ToString();
                lblExist.Text = dt.Rows[i]["lblExist"].ToString();
                if (lblExist.Text == "False" && lblCouponBill.Text != "True")
                {
                    txtActualRate.ReadOnly = false;
                }
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
            dtTable.Columns.Add(new DataColumn("txtSACCode", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtQuantity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtActualRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtTestDiscount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAmount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblExist", typeof(string)));
            for (int i = 0; i < grdDetails.Rows.Count; i++)
            {
                DropDownList ddlSubTest = (DropDownList)grdDetails.Rows[i].FindControl("ddlSubTest");
                Label lblSubTestId = (Label)grdDetails.Rows[i].FindControl("lblSubTestId");
                Label lblTestId = (Label)grdDetails.Rows[i].FindControl("lblTestId");
                DropDownList ddl_Test = (DropDownList)grdDetails.Rows[i].FindControl("ddl_Test");
                TextBox box1 = (TextBox)grdDetails.Rows[i].FindControl("txtDescription");
                TextBox box2 = (TextBox)grdDetails.Rows[i].FindControl("txtSACCode");
                TextBox box3 = (TextBox)grdDetails.Rows[i].FindControl("txtQuantity");
                TextBox txtActualRate = (TextBox)grdDetails.Rows[i].FindControl("txtActualRate");
                TextBox txtTestDiscount = (TextBox)grdDetails.Rows[i].FindControl("txtTestDiscount");
                TextBox box4 = (TextBox)grdDetails.Rows[i].FindControl("txtRate");
                TextBox box5 = (TextBox)grdDetails.Rows[i].FindControl("txtAmount");
                Label lblExist = (Label)grdDetails.Rows[i].FindControl("lblExist");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["ddlSubTest"] = ddlSubTest.SelectedValue;
                //drRow["lblSubTestId"] = lblSubTestId.Text;
                drRow["lblSubTestId"] = ddlSubTest.SelectedValue;
                drRow["lblTestId"] = lblTestId.Text;
                drRow["ddl_Test"] = ddl_Test.SelectedValue;
                drRow["txtDescription"] = box1.Text;
                drRow["txtSACCode"] = box2.Text;
                drRow["txtQuantity"] = box3.Text;
                drRow["txtActualRate"] = txtActualRate.Text;
                drRow["txtTestDiscount"] = txtTestDiscount.Text;
                drRow["txtRate"] = box4.Text;
                drRow["txtAmount"] = box5.Text;
                drRow["lblExist"] = lblExist.Text;

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
                DropDownList ddl_Test = (DropDownList)e.Row.FindControl("ddl_Test");
                #region for billmodify
                if (txtRecordType.Text == "OT")
                {
                    DropDownList ddlSubTest = (DropDownList)e.Row.FindControl("ddlSubTest");
                    //ddlSubTest.DataSource = ViewState["OtherSubTestList"];
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
                    var test = dc.Test_View_ForBillModify(txtRecordType.Text, Convert.ToInt32(lblMaterialId.Text), 0);
                    ddl_Test.DataSource = test;
                    ddl_Test.DataTextField = "TEST_Name_var";
                    ddl_Test.DataValueField = "TEST_Id";
                    ddl_Test.DataBind();
                    ddl_Test.Items.Insert(0, new ListItem("---Select---", "0"));
                }
                #endregion

                TextBox txtDescription = (TextBox)e.Row.FindControl("txtDescription");
                ImageButton imgInsert = (ImageButton)e.Row.FindControl("imgInsert");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                TextBox txtActualRate = (TextBox)e.Row.FindControl("txtActualRate");
                TextBox txtTestDiscount = (TextBox)e.Row.FindControl("txtTestDiscount");
                TextBox txtRate = (TextBox)e.Row.FindControl("txtRate");

                if (lblCouponBill.Text == "True")
                {
                    txtDescription.ReadOnly = true;
                    imgInsert.Visible = false;
                    imgDelete.Visible = false;
                    ddl_Test.Visible = false;
                }
                else
                {
                    txtDescription.ReadOnly = false;
                    ddl_Test.Visible = true;
                }
                if (lblDiscountModifyRight.Text == "True")
                {
                    //txtActualRate.ReadOnly = false;
                    txtTestDiscount.ReadOnly = false;
                    txtRate.ReadOnly = false;
                }
            
            }
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs eventArgs)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txtDescription = (TextBox)grdDetails.Rows[rowindex].FindControl("txtDescription");
            TextBox txtQuantity = (TextBox)grdDetails.Rows[rowindex].FindControl("txtQuantity");
            TextBox txtActualRate = (TextBox)grdDetails.Rows[rowindex].FindControl("txtActualRate");
            TextBox txtTestDiscount = (TextBox)grdDetails.Rows[rowindex].FindControl("txtTestDiscount");
            TextBox txtRate = (TextBox)grdDetails.Rows[rowindex].FindControl("txtRate");
            TextBox txtAmount = (TextBox)grdDetails.Rows[rowindex].FindControl("txtAmount");
            TextBox txtCoupFrom = (TextBox)grdDetails.Rows[rowindex].FindControl("txtCoupFrom");
            TextBox txtCoupTo = (TextBox)grdDetails.Rows[rowindex].FindControl("txtCoupTo");
            
            txtDiscPer.Text = "0.00";
            txtDiscount.Text = "0.00";

            if (txtQuantity.Text == "")
            {
                txtAmount.Text = "";
                //txtCoupFrom.Text = "";
                txtCoupTo.Text = "";
                if (lblCouponBill.Text == "True")
                    txtDescription.Text = "";
            }
            else if (lblCouponBill.Text == "True")
            {
                if (txtCoupFrom.Text != "")
                {
                    txtCoupTo.Text = (Convert.ToInt32(txtCoupFrom.Text) + Convert.ToInt32(txtQuantity.Text) - 1).ToString();

                    txtDescription.Text = "Bill For ";
                    if (rdoCT.Checked )
                        txtDescription.Text += " Concrete Cube Testing ";
                    else if (rdoST.Checked )
                        txtDescription.Text += " Steel Testing ";
                    txtDescription.Text += " Coupons - From " + txtCoupFrom.Text + " To " + txtCoupTo.Text;
                }
                string coupType;
                if (rdoST.Checked )
                    coupType = "STCOUPON";
                else
                    coupType = "Coupon";

                var test =  dc.Test_View(0, 0,coupType , 0, 0, 0);
                foreach (var tst in test)
                {
                    if (Convert.ToInt32(txtQuantity.Text) <= 51 && tst.TEST_Sr_No == 1)
                    {
                        txtActualRate.Text = Convert.ToDecimal(tst.TEST_Rate_int).ToString("0.00");
                        break;
                    }
                    else if (Convert.ToInt32(txtQuantity.Text) <= 100 && tst.TEST_Sr_No == 2)
                    {
                        txtActualRate.Text = Convert.ToDecimal(tst.TEST_Rate_int).ToString("0.00");
                        break;
                    }
                    else if (Convert.ToInt32(txtQuantity.Text) > 100 && tst.TEST_Sr_No == 3)
                    {
                        txtActualRate.Text = Convert.ToDecimal(tst.TEST_Rate_int).ToString("0.00");
                        break;
                    }
                }
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
            }
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
                txtRate.Text = txtActualRate.Text;
            }
            txtTestDiscount.Text = "0.00";
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

            if (!cnStr.ToLower().Contains("nashik"))
            {
                lblAdvanceRcpt.Visible = true;
                txtAdvRecpt.Visible = true;
                txtAdvRecptBal.Visible = true;
            }
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

            lblMaterialId.Text = "0";
        }

        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSite.Items.Clear();
            if (Convert.ToInt32(ddlClient.SelectedValue) != 0)
            {
                LoadSiteList();
                var address = dc.Client_View(Convert.ToInt32(ddlClient.SelectedValue), 0, "", "");
                foreach (var adds in address)
                {
                    txtAddress.Text = adds.CL_OfficeAddress_var;
                    if (lblCouponBill.Text == "True")
                    {
                        if (adds.CL_SiteSpecificCoupon_bit == true)
                        {
                            lblSiteSpecCoupStatus.Text = "Site Specific Coupons";
                        }
                        else
                        {
                            lblSiteSpecCoupStatus.Text = "Client Specific Coupons";
                        }
                    }
                }

                if (lblBillId.Text == "" && lblCouponBill.Text == "True")
                {
                    TextBox txtCoupFrom = (TextBox)grdDetails.Rows[0].Cells[3].FindControl("txtCoupFrom");
                    TextBox txtCoupTo = (TextBox)grdDetails.Rows[0].Cells[3].FindControl("txtCoupTo");
                    TextBox txtQuantity = (TextBox)grdDetails.Rows[0].Cells[3].FindControl("txtQuantity");
                    TextBox txtDescription = (TextBox)grdDetails.Rows[0].Cells[3].FindControl("txtDescription");
                    txtCoupFrom.Text = "";
                    txtCoupTo.Text = "";
                    var coupon = dc.Coupon_View("", 0, 0, Convert.ToInt32(ddlClient.SelectedValue), 0, 0, null);
                    //txtCoupFrom.Text = (coupon.Count() + 1).ToString();
                    foreach (var coup in coupon)
                    {
                        txtCoupFrom.Text = (coup.COUP_Id + 1).ToString();
                        if (coup.COUP_Id < 0)
                        {
                            txtCoupFrom.Text = "1";
                        }
                        break;
                    }
                    if (txtCoupFrom.Text == "")
                    {
                        txtCoupFrom.Text = "1";
                    }
                    if (txtCoupFrom.Text != "" && txtQuantity.Text != "")
                    {
                        txtCoupTo.Text = (Convert.ToInt32(txtCoupFrom.Text) + Convert.ToInt32(txtQuantity.Text) - 1).ToString();
                        if (rdoST.Checked )
                            txtDescription.Text = "Bill For Steel Testing Coupons - From " + txtCoupFrom.Text + " To " + txtCoupTo.Text;
                        else 
                            txtDescription.Text = "Bill For Concrete Cube Testing Coupons - From " + txtCoupFrom.Text + " To " + txtCoupTo.Text;
                    }
                }
            }
        }

        private void LoadSiteList()
        {
            var site = dc.Site_View(0, Convert.ToInt32(ddlClient.SelectedValue), 0, "");
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
            //GrossAmount = 9990;
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
            //Session["BillId"] = Convert.ToString(txtBillNo.Text);
            //Response.Redirect("ReportPDF.aspx");
            var bill = dc.Bill_View(txtBillNo.Text, 0, 0, "", 0, false, false, null, null).ToList();
            if (bill.FirstOrDefault().BILL_PrintLock_bit == false)
            {
                printBill(txtBillNo.Text);
            }
        }
        private void printBill(string billNo)
        {
            //string reportPath;
            //string reportStr = "";
            //StreamWriter sw;
            //BillUpdation bill = new BillUpdation();

            //reportPath = Server.MapPath("~") + "\\report.html";
            //sw = File.CreateText(reportPath);
            //reportStr = bill.getBillPrintString(Convert.ToInt32(billNo),false);

            //sw.WriteLine(reportStr);
            //sw.Close();
            //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");

            BillUpdation bill = new BillUpdation();
            bill.getBillPrintString(billNo, false);
            //PrintPDFReport obj = new PrintPDFReport();
            //obj.Bill_PDFPrint(Convert.ToInt32(billNo), false, "print");
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
                LoadClientList();
                ddlClient.SelectedValue = b.BILL_CL_Id.ToString();
                LoadSiteList();
                ddlSite.SelectedValue = b.BILL_SITE_Id.ToString();                
                txtAddress.Text = b.CL_OfficeAddress_var;
                txtBillNo.Text = b.BILL_Id.ToString();
                txtDate.Text = Convert.ToDateTime(b.BILL_Date_dt).ToString("dd/MM/yyyy");
                txtWorkOrderNo.Text = b.BILL_WorkOrderNo_var;
                txtRecordNo.Text = b.BILL_RecordNo_int.ToString();
                txtRecordType.Text = b.BILL_RecordType_var;
                txtReason.Text = b.BILL_Reason_var;
                lblMaterialId.Text = "0";
                var mat = dc.Material_View(txtRecordType.Text, ", ");
                foreach (var m in mat)
                {
                    lblMaterialId.Text = m.MATERIAL_Id.ToString();
                }
                //if (txtRecordType.Text == "OT")
                //    LoadOtherSubTestList();

                //LoadServiceTax();
                //LoadSwachhBharatTax();
                //LoadKisanKrishiTax();
                //LoadGSTax();

                if (b.BILL_RecordType_var == "---")
                {
                    lblCouponBill.Text = "True";
                }
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
                //txtEducationTax.Text = Convert.ToDecimal(b.BILL_EdCessAmt_num).ToString("0.00");
                //txtHigherEduTax.Text = Convert.ToDecimal(b.BILL_HighEdCessAmt_num).ToString("0.00");
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
            var billDetail = dc.BillDetail_View(BillNumber);
            foreach (var bd in billDetail)
            {
                AddRowBillDetail();
                DropDownList ddlSubTest = (DropDownList)grdDetails.Rows[i].Cells[3].FindControl("ddlSubTest");
                Label lblSubTestId = (Label)grdDetails.Rows[i].Cells[3].FindControl("lblSubTestId");
                DropDownList ddl_Test = (DropDownList)grdDetails.Rows[i].Cells[3].FindControl("ddl_Test");
                Label lblTestId = (Label)grdDetails.Rows[i].Cells[3].FindControl("lblTestId");
                TextBox txtDescription = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtDescription");
                TextBox txtSACCode = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtSACCode");
                TextBox txtQuantity = (TextBox)grdDetails.Rows[i].Cells[4].FindControl("txtQuantity");
                TextBox txtActualRate = (TextBox)grdDetails.Rows[i].FindControl("txtActualRate");
                TextBox txtTestDiscount = (TextBox)grdDetails.Rows[i].FindControl("txtTestDiscount");
                TextBox txtRate = (TextBox)grdDetails.Rows[i].Cells[5].FindControl("txtRate");
                TextBox txtAmount = (TextBox)grdDetails.Rows[i].Cells[6].FindControl("txtAmount");
                TextBox txtCoupFrom = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtCoupFrom");
                TextBox txtCoupTo = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtCoupTo");
                Label lblExist = (Label)grdDetails.Rows[i].Cells[3].FindControl("lblExist");

                if (txtRecordType.Text == "OT")
                {
                    var subtest = dc.Test_View(0, Convert.ToInt32(bd.BILLD_TEST_Id), "", 0, 0, 0);
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
                        var test = dc.Test_View_ForBillModify(txtRecordType.Text, Convert.ToInt32(lblMaterialId.Text), SubTestId);
                        ddl_Test.DataSource = test;
                        ddl_Test.DataTextField = "TEST_Name_var";
                        ddl_Test.DataValueField = "TEST_Id";
                        ddl_Test.DataBind();
                        ddl_Test.Items.Insert(0, new ListItem("---Select---", "0"));

                        lblTestId.Text = bd.BILLD_TEST_Id.ToString();
                        ddl_Test.SelectedValue = bd.BILLD_TEST_Id.ToString();
                    }                    
                }
                else
                {
                    lblTestId.Text = bd.BILLD_TEST_Id.ToString();
                    ddl_Test.SelectedValue = bd.BILLD_TEST_Id.ToString();
                }
                txtActualRate.ReadOnly = true;
                lblExist.Text = "True";                
                txtDescription.Text = bd.BILLD_TEST_Name_var;
                txtSACCode.Text = bd.BILLD_SACCode_var;
                txtQuantity.Text = Convert.ToDecimal(bd.BILLD_Quantity_int).ToString("0.##");
                if (bd.BILLD_ActualRate_num != null)
                    txtActualRate.Text = Convert.ToDecimal(bd.BILLD_ActualRate_num).ToString("0.00");
                else
                    txtActualRate.Text = "0";
                if (bd.BILLD_ActualRate_num != null)
                    txtTestDiscount.Text = Convert.ToDecimal(bd.BILLD_DiscountPer_num).ToString("0.##");
                else
                    txtTestDiscount.Text = "0";
                txtRate.Text = Convert.ToDecimal(bd.BILLD_Rate_num).ToString("0.00");
                txtAmount.Text = Convert.ToDecimal(bd.BILLD_Amt_num).ToString("0.00");
                if (lblCouponBill.Text == "True")
                {
                    txtCoupFrom.ReadOnly = true;
                    txtCoupTo.ReadOnly = true;
                    txtQuantity.ReadOnly = true;
                    ddlClient.Enabled = false;
                    ddlSite.Enabled = false;

                    string[] words1 = Regex.Split(txtDescription.Text, "From");
                    string[] words2 = Regex.Split(words1[1], "To");
                    txtCoupFrom.Text = words2[0].Trim();
                    txtCoupTo.Text = words2[1].Trim();
                    //for (int ii = Convert.ToInt32(txtCoupFrom.Text); ii <= Convert.ToInt32(txtCoupTo.Text); ii++)
                    //{
                    //    var coupon = dc.Coupon_View(BillNumber, ii, true,0,0,0,null);
                    //    if (coupon.Count() > 0)
                    //    {
                    //        //string msg = "alert('Coupon number " + ii.ToString() + " already used  !'+ '\\n' +'Can not change bill.')";
                    //        //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                    //        txtCoupFrom.ReadOnly = true;
                    //        txtCoupTo.ReadOnly = true;
                    //        txtQuantity.ReadOnly = true;
                    //        break;
                    //    }
                    //}
                }
                i++;
            }
            if (lblCouponBill.Text == "True")
            {
                TextBox txtCoupFrom = (TextBox)grdDetails.Rows[0].Cells[3].FindControl("txtCoupFrom");
                var coup = dc.Coupon_View(BillNumber, 0, 0, 0, 0, 0, null);
                foreach (var c in coup)
                {
                    if (c.COUP_SiteSpecStatus_bit == true)
                    {
                        lblSiteSpecCoupStatus.Text = "Site Specific Coupons";
                    }
                    else
                    {
                        lblSiteSpecCoupStatus.Text = "Client Specific Coupons";
                    }
                    break;
                }

                var client = dc.Client_View(Convert.ToInt32(ddlClient.SelectedValue), 0, "", "");
                foreach (var cl in client)
                {
                    if (cl.CL_SiteSpecificCoupon_bit == true)
                    {
                        lblClientCouponSetting.Text = "Site Specific Coupons";
                    }
                    else
                    {
                        lblClientCouponSetting.Text = "Client Specific Coupons";
                    }
                    var coupon1 = dc.Coupon_View(BillNumber, 0, 1, 0, 0, 0, null);
                    if (coupon1.Count() == 0)
                    {
                        if ((cl.CL_SiteSpecificCoupon_bit == true && lblSiteSpecCoupStatus.Text == "Client Specific Coupons")
                            || (cl.CL_SiteSpecificCoupon_bit == false && lblSiteSpecCoupStatus.Text == "Site Specific Coupons"))
                        {
                            chkAsPerClient.Visible = true;
                            chkAsPerClient.Enabled = true;
                        }

                    }
                    else
                    {
                        chkAsPerClient.Enabled = false;
                        if (lblClientCouponSetting.Text != lblSiteSpecCoupStatus.Text)
                        {
                            chkAsPerClient.Visible = true;
                        }
                    }
                }

            }
            txtBillNo.ReadOnly = true;
            txtDate.ReadOnly = true;
            txtRecordNo.ReadOnly = true;
            txtRecordType.ReadOnly = true;
            CalendarExtender1.Enabled = false;
            if (grdDetails.Rows.Count == 0)
            {
                //lblMessage.Text = "No records Found !!";
                //lblMessage.Visible = true;
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

                if((cgstFlag == true && lblCGSTPer.Visible == false) || (cgstFlag == false && lblCGSTPer.Visible == true) )
                    lnkRateAsPerCurrent.Visible = true;
                else if((sgstFlag == true && lblSGSTPer.Visible == false) || (sgstFlag == false && lblSGSTPer.Visible == true) )
                    lnkRateAsPerCurrent.Visible = true;
                else if ((igstFlag == true && lblIGSTPer.Visible == false) || (igstFlag == false && lblIGSTPer.Visible == true))
                    lnkRateAsPerCurrent.Visible = true;

            }
            ///
        }
        protected void txtAdvRecpt_TextChanged(object sender, EventArgs e)
        {
            if (txtAdvRecpt.Text != "")
            {
                var rslt = dc.AdvanceDetail_View_ForCoupon(Convert.ToInt32(txtAdvRecpt.Text)).ToList();
                if (rslt.Count > 0)
                {
                    txtAdvRecptBal.Text = Convert.ToString(rslt.FirstOrDefault().Amount);
                }
            }
            else
                txtAdvRecptBal.Text = "0";
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            bool updateBillFlag = true, billStatus = false;
            string TallyNarration = "";
            string BillNo = "0";
            byte couponStatus = 0;

            if (txtBillNo.Text != "Create New.......")
            {
                lnkSave.Enabled = true;
                BillNo = txtBillNo.Text;
            }
            else if (!cnStr.ToLower().Contains("nashik"))
            {
                if(txtAdvRecpt.Text=="")
                {
                    string msg = "alert('Please enter Advanced Receipt No!')";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                    updateBillFlag = false;
                }
                else //chek adv rcpt bal amount
                {
                    if (Convert.ToDecimal(txtAdvRecptBal.Text) >= Convert.ToDecimal(txtNet.Text))
                    {
                        updateBillFlag = true;
                    }
                    else
                    {
                        string msg = "alert('Advanced balance amount should be greater than equal to Bill amount!!')";
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                        updateBillFlag = false;
                    }
                    
                }
            }
            //else if (!cnStr.ToLower().Contains("nashik"))
            //{
            //    lnkSave.Enabled = false;
            //    string msg = "alert('This facility is not available.')";
            //    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
            //    return;
            //}
            if (ddlStatus.SelectedValue == "Ok")
            {
                billStatus = false;
                couponStatus = 0;
            }
            else
            {
                billStatus = true;
                couponStatus = 2;
            }

            
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
            clsData clsDt = new clsData();
            //check gst information is updated or not
            if (updateBillFlag == true && clsDt.checkGSTInfoUpdated(ddlClient.SelectedValue, ddlSite.SelectedValue, "") == false)
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
            
            //check for duplicate coupon numbers
            if (lblCouponBill.Text == "True" && updateBillFlag == true)
            {
                foreach (GridViewRow row in grdDetails.Rows)
                {
                    TextBox txtCoupFrom = (TextBox)row.FindControl("txtCoupFrom");
                    TextBox txtCoupTo = (TextBox)row.FindControl("txtCoupTo");
                    for (int i = Convert.ToInt32(txtCoupFrom.Text); i <= Convert.ToInt32(txtCoupTo.Text); i++)
                    {
                        var coupon=dc.Coupon_View("01", i, 0, Convert.ToInt32(ddlClient.SelectedValue), 0, 0, null);
                        if (rdoST.Checked )
                            coupon  = dc.Coupon_View(BillNo, i, 0, Convert.ToInt32(ddlClient.SelectedValue), 0, 0, null);
                        else
                            coupon = dc.Coupon_View(BillNo, i, 0, Convert.ToInt32(ddlClient.SelectedValue), 0, 0, null);

                        if (coupon.Count() > 0)
                        {
                            string msg = "alert('Coupon number " + i.ToString() + " already alloted  !'+ '\\n' +'Can not save bill.')";
                            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                            updateBillFlag = false;
                            break;
                        }
                        var coupon1 = dc.Coupon_View("01", i, 1, 0, 0, 0, null);
                        if (rdoST.Checked)
                            coupon1 = dc.Coupon_View(BillNo, i, 1, 0, 0, 0, null);
                        else
                            coupon1 = dc.Coupon_View(BillNo, i, 1, 0, 0, 0, null);

                        if (coupon1.Count() > 0)
                        {
                            string msg = "alert('Coupon number " + i.ToString() + " already used  !'+ '\\n' +'Can not change bill.')";
                            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                            updateBillFlag = false;
                            break;
                        }
                    }
                }
            }
            if (updateBillFlag == true)
            {
                for (int i = 0; i < grdDetails.Rows.Count; i++)
                {
                    DropDownList ddl_Test = (DropDownList)grdDetails.Rows[i].FindControl("ddl_Test");
                    if (ddl_Test.Visible == true)
                    {
                        if (ddl_Test.SelectedItem.Text == "---Select---" || ddl_Test.SelectedItem.Text == "Miscellaneous")
                        {
                            string msg = "alert('Select test from list for row no. " + i.ToString() + ".')";
                            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                            updateBillFlag = false;
                            ddl_Test.Focus();
                            break;
                        }
                    }
                }
            }
            if (txtAdvancePaid.Text == "")
                txtAdvancePaid.Text = "0.00";
            if (updateBillFlag == true)
            {
                foreach (GridViewRow row in grdDetails.Rows)
                {
                    TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
                    TextBox txtCoupFrom = (TextBox)row.FindControl("txtCoupFrom");
                    TextBox txtCoupTo = (TextBox)row.FindControl("txtCoupTo");
                    if (lblCouponBill.Text == "True")
                    {
                        if (rdoST.Checked )
                            txtDescription.Text = "Bill For Steel Testing Coupons - From " + txtCoupFrom.Text + " To " + txtCoupTo.Text;
                        else
                            txtDescription.Text = "Bill For Concrete Cube Testing Coupons - From " + txtCoupFrom.Text + " To " + txtCoupTo.Text;
                    }
                    if (TallyNarration == "")
                    {
                        TallyNarration = txtDescription.Text;
                    }
                    else
                    {
                        TallyNarration = TallyNarration + ", " + txtDescription.Text;
                    }
                }
                TallyNarration = TallyNarration.ToUpper();
                //
                dc.BillDetail_Update(BillNo, 0, 0, 0, "", 0, "", 0, 0, false,0, true);
                dc.Coupon_Update(BillNo, 0, 0, 0, null, 0, null, "", 0, null, false ,true);
                bool billPrintLockStatus = false;
                bool insertBill = false;
                if (BillNo == "0")
                {
                    //var client = dc.Client_View(Convert.ToInt32(ddlClient.SelectedValue), 0, "", "");
                    //foreach (var cl in client)
                    //{
                    //    billPrintLockStatus = Convert.ToBoolean(cl.CL_MonthlyBilling_bit);
                    //}
                    var site = dc.Site_View(Convert.ToInt32(ddlSite.SelectedValue), 0, 0, "");
                    foreach (var st in site)
                    {
                        billPrintLockStatus = Convert.ToBoolean(st.SITE_MonthlyBillingStatus_bit);
                    }
                    int NewrecNo = 0;
                    clsData clsObj = new clsData();
                    NewrecNo = clsObj.GetnUpdateRecordNo("BillNo");
                    var master = dc.MasterSetting_View(0);
                    foreach (var mst in master)
                    {
                        //BillNo = mst.MASTER_AccountingYear_var + "/" + mst.MASTER_Region_var + "/" + NewrecNo.ToString();
                        BillNo = mst.MASTER_AccountingYear_var + mst.MASTER_Region_var + NewrecNo.ToString();
                    }
                    insertBill = true;
                }
                //BillNo = 
                dc.Bill_Update(BillNo, Convert.ToInt32(ddlClient.SelectedValue), ddlClient.SelectedItem.Text, Convert.ToInt32(ddlSite.SelectedValue), optPercentage.Checked, Convert.ToDecimal(txtDiscPer.Text), Convert.ToDecimal(txtDiscount.Text), Convert.ToDecimal(lblSerTaxPer.Text.Replace("(", "").Replace("%)", "")),
                    Convert.ToDecimal(txtServiceTax.Text), Convert.ToDecimal(lblSwachhBharatTaxPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtSwachhBharatTax.Text), Convert.ToDecimal(lblKisanKrishiTaxPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtKisanKrishiTax.Text),
                    Convert.ToDecimal(lblCGSTPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtCGST.Text), Convert.ToDecimal(lblSGSTPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtSGST.Text), Convert.ToDecimal(lblIGSTPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtIGST.Text), Convert.ToDecimal(txtAdvancePaid.Text),
                    Convert.ToDecimal(txtNet.Text), 0, 0, Convert.ToDecimal(txtRoundingOff.Text), txtRecordType.Text, Convert.ToInt32(txtRecordNo.Text), billStatus, TallyNarration,
                    false, Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), Convert.ToBoolean(lblCouponBill.Text), txtWorkOrderNo.Text.Trim(), billPrintLockStatus, txtReason.Text, insertBill);
                txtBillNo.Text = BillNo.ToString();
                bool sitespecstatus = false;
                if (lblSiteSpecCoupStatus.Text == "Site Specific Coupons")
                {
                    sitespecstatus = true;
                }
                if (chkAsPerClient.Enabled == true && chkAsPerClient.Checked == true &&
                    lblSiteSpecCoupStatus.Text != lblClientCouponSetting.Text)
                {
                    if (lblClientCouponSetting.Text == "Site Specific Coupons")
                    {
                        sitespecstatus = true;
                    }
                    else
                    {
                        sitespecstatus = false;
                    }
                }
                foreach (GridViewRow row in grdDetails.Rows)
                {
                    Label lblTestId = (Label)row.FindControl("lblTestId");
                    DropDownList ddl_Test = (DropDownList)row.FindControl("ddl_Test");
                    TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
                    TextBox txtSACCode = (TextBox)row.FindControl("txtSACCode");
                    TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
                    TextBox txtActualRate = (TextBox)row.FindControl("txtActualRate");
                    TextBox txtTestDiscount = (TextBox)row.FindControl("txtTestDiscount");
                    TextBox txtRate = (TextBox)row.FindControl("txtRate");
                    TextBox txtAmount = (TextBox)row.FindControl("txtAmount");
                    TextBox txtCoupFrom = (TextBox)row.FindControl("txtCoupFrom");
                    TextBox txtCoupTo = (TextBox)row.FindControl("txtCoupTo");
                    //if (lblTestId.Text == "")
                    //{
                        lblTestId.Text = "0";
                        if (lblCouponBill.Text == "True")
                        {
                            var testMinBill = dc.Test(0, "", 0, "COUPON", "Concrete Cube Testing Coupons", 0);
                            foreach (var testId in testMinBill)
                            {
                                lblTestId.Text = testId.TEST_Id.ToString();
                            }
                        }
                        else
                        {
                            lblTestId.Text = ddl_Test.SelectedValue;
                        }
                    //}
                    var b = dc.BillDetail_Update(BillNo, Convert.ToInt32(row.Cells[2].Text), Convert.ToDecimal(txtQuantity.Text), Convert.ToDecimal(txtAmount.Text), txtDescription.Text, Convert.ToDecimal(txtRate.Text), txtSACCode.Text, Convert.ToDecimal(txtActualRate.Text), Convert.ToDecimal(txtTestDiscount.Text), false, Convert.ToInt32(lblTestId.Text), false);
                    if (lblCouponBill.Text == "True")
                    {
                        for (int i = Convert.ToInt32(txtCoupFrom.Text); i <= Convert.ToInt32(txtCoupTo.Text); i++)
                        {
                            if (rdoST.Checked )
                                dc.Coupon_UpdateST(BillNo, i, Convert.ToInt32(ddlClient.SelectedValue), Convert.ToInt32(ddlSite.SelectedValue), DateTime.Now, couponStatus, null, "", 0, DateTime.Now.AddDays(730), sitespecstatus, false);
                            else
                             dc.Coupon_Update(BillNo, i, Convert.ToInt32(ddlClient.SelectedValue), Convert.ToInt32(ddlSite.SelectedValue), DateTime.Now, couponStatus, null, "", 0, DateTime.Now.AddDays(730), sitespecstatus, false);
                        }
                    }
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", "alert('Records Sucessfully Updated ')", true);
                lnkSave.Visible = false;
                lnkPrint.Visible = true;
                //ClearData();
            }
        }

        protected void imgClosePopup_Click(object sender, EventArgs e)
        {
            Response.Redirect("BillStatus.aspx");
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

        protected void txtCoupFrom_TextChanged(object sender, EventArgs eventArgs)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txtCoupFrom = (TextBox)grdDetails.Rows[rowindex].Cells[3].FindControl("txtCoupFrom");
            TextBox txtCoupTo = (TextBox)grdDetails.Rows[rowindex].Cells[3].FindControl("txtCoupTo");
            TextBox txtQuantity = (TextBox)grdDetails.Rows[rowindex].Cells[2].FindControl("txtQuantity");
            TextBox txtRate = (TextBox)grdDetails.Rows[rowindex].Cells[3].FindControl("txtRate");
            TextBox txtAmount = (TextBox)grdDetails.Rows[rowindex].Cells[4].FindControl("txtAmount");

            if (txtQuantity.Text == "")
            {
                txtAmount.Text = "";
            }
            else if (txtCoupFrom.Text != "")
            {
                txtCoupTo.Text = (Convert.ToInt32(txtCoupFrom.Text) + Convert.ToInt32(txtQuantity.Text) - 1).ToString();
            }
            if (txtRate.Text != "" && txtQuantity.Text != "")
            {
                txtAmount.Text = Convert.ToDecimal(Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQuantity.Text)).ToString("0.00");
            }
            CalculateBill();
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
                var test = dc.Test_View_ForBillModify(txtRecordType.Text, Convert.ToInt32(lblMaterialId.Text), subTestId);
                ddl_Test.DataSource = test;
                ddl_Test.DataTextField = "TEST_Name_var";
                ddl_Test.DataValueField = "TEST_Id";
                ddl_Test.DataBind();
                ddl_Test.Items.Insert(0, new ListItem("---Select---", "0"));
                
            }
        }
        protected void ddl_Test_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txtDescription = (TextBox)grdDetails.Rows[rowindex].FindControl("txtDescription");
            if (ddl.SelectedIndex > 0)
            {
                txtDescription.Text = ddl.SelectedItem.Text;
            }
            else
            {
                txtDescription.Text = "";
            }
        }

        //protected void rdoST_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rdoST.Checked == true)
        //    {
        //        rdoCT.Checked = false;
        //    }
        //}

        //protected void rdoCT_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rdoCT.Checked == true)
        //    {
        //        rdoST.Checked = false;
        //    }
        //}

      
    }
}