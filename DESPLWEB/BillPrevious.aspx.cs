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
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;

namespace DESPLWEB
{
    public partial class BillPrevious : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool userRight = false;
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Add Previous Bill";
                
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                    foreach (var u in user)
                    {
                        if (u.USER_Bill_right_bit == true)
                        {
                            userRight = true;
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
                        lblCouponBill.Text = "False";
                    }
                    ////
                    LoadServiceTax();
                    LoadSwachhBharatTax();
                    LoadKisanKrishiTax();
                    if (lblBillId.Text != "")
                    {
                        LoadBillDetail();
                    }
                    else
                    {
                        ClearData();
                        LoadClientList();
                        //AddRowBillDetail();
                        txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtRecordNo.Text = "0";
                        //txtRecordType.Text = "---";
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
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("txtQuantity", typeof(string)));
                dt.Columns.Add(new DataColumn("txtRate", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAmount", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtDescription"] = string.Empty;
            dr["txtQuantity"] = string.Empty;
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
                TextBox box1 = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtDescription");
                TextBox box2 = (TextBox)grdDetails.Rows[i].Cells[4].FindControl("txtQuantity");
                TextBox box3 = (TextBox)grdDetails.Rows[i].Cells[5].FindControl("txtRate");
                TextBox box4 = (TextBox)grdDetails.Rows[i].Cells[6].FindControl("txtAmount");

                grdDetails.Rows[i].Cells[2].Text = (i + 1).ToString();
                box1.Text = dt.Rows[i]["txtDescription"].ToString();
                box2.Text = dt.Rows[i]["txtQuantity"].ToString();
                box3.Text = dt.Rows[i]["txtRate"].ToString();
                box4.Text = dt.Rows[i]["txtAmount"].ToString();
            }
        }

        protected void GetCurrentDataBillDetail()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtQuantity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAmount", typeof(string)));
            for (int i = 0; i < grdDetails.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtDescription");
                TextBox box2 = (TextBox)grdDetails.Rows[i].Cells[4].FindControl("txtQuantity");
                TextBox box3 = (TextBox)grdDetails.Rows[i].Cells[5].FindControl("txtRate");
                TextBox box4 = (TextBox)grdDetails.Rows[i].Cells[6].FindControl("txtAmount");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txtDescription"] = box1.Text;
                drRow["txtQuantity"] = box2.Text;
                drRow["txtRate"] = box3.Text;
                drRow["txtAmount"] = box4.Text;

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
        protected void grdDetails_RowDataBound(object sender, GridViewRowEventArgs eventArgs)
        {
            if (eventArgs.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtDescription = (TextBox)eventArgs.Row.FindControl("txtDescription");
                //Label lblCoupFrom = (Label)eventArgs.Row.FindControl("lblCoupFrom");
                //TextBox txtCoupFrom = (TextBox)eventArgs.Row.FindControl("txtCoupFrom");
                //Label lblCoupTo = (Label)eventArgs.Row.FindControl("lblCoupTo");
                //TextBox txtCoupTo = (TextBox)eventArgs.Row.FindControl("txtCoupTo");
                ImageButton imgInsert = (ImageButton)eventArgs.Row.FindControl("imgInsert");
                ImageButton imgDelete = (ImageButton)eventArgs.Row.FindControl("imgDelete");

                //if (Session["CouponBill"].ToString() == "True")
                //{
                //    txtDescription.Visible = false;
                //    lblCoupFrom.Visible = true;
                //    txtCoupFrom.Visible = true;
                //    lblCoupTo.Visible = true;
                //    txtCoupTo.Visible = true;
                //    imgInsert.Visible = false;
                //    imgDelete.Visible = false;
                //}
                //else
                //{
                //    txtDescription.Visible = true;
                //    lblCoupFrom.Visible = false;
                //    txtCoupFrom.Visible = false;
                //    lblCoupTo.Visible = false;
                //    txtCoupTo.Visible = false;
                //}
                if (lblCouponBill.Text == "True")
                {
                    txtDescription.ReadOnly = true;
                    imgInsert.Visible = false;
                    imgDelete.Visible = false;
                }
                else
                {
                    txtDescription.ReadOnly = false;
                }
            }
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs eventArgs)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txtDescription = (TextBox)grdDetails.Rows[rowindex].Cells[1].FindControl("txtDescription");
            TextBox txtQuantity = (TextBox)grdDetails.Rows[rowindex].Cells[2].FindControl("txtQuantity");
            TextBox txtRate = (TextBox)grdDetails.Rows[rowindex].Cells[3].FindControl("txtRate");
            TextBox txtAmount = (TextBox)grdDetails.Rows[rowindex].Cells[4].FindControl("txtAmount");
            TextBox txtCoupFrom = (TextBox)grdDetails.Rows[rowindex].Cells[2].FindControl("txtCoupFrom");
            TextBox txtCoupTo = (TextBox)grdDetails.Rows[rowindex].Cells[3].FindControl("txtCoupTo");
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
            else if (lblCouponBill.Text == "True" && txtCoupFrom.Text != "")
            {
                txtCoupTo.Text = (Convert.ToInt32(txtCoupFrom.Text) + Convert.ToInt32(txtQuantity.Text) - 1).ToString();
                txtDescription.Text = "Bill For Concrete Cube Testing Coupons - From " + txtCoupFrom.Text + " To " + txtCoupTo.Text;
            }
            if (txtRate.Text != "" && txtQuantity.Text != "")
            {
                txtAmount.Text = Convert.ToDecimal(Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQuantity.Text)).ToString("0.00");
            }
            CalculateBill();
        }

        protected void txtRate_TextChanged(object sender, EventArgs eventArgs)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            TextBox txtQuantity = (TextBox)grdDetails.Rows[rowindex].Cells[2].FindControl("txtQuantity");
            TextBox txtRate = (TextBox)grdDetails.Rows[rowindex].Cells[3].FindControl("txtRate");
            TextBox txtAmount = (TextBox)grdDetails.Rows[rowindex].Cells[4].FindControl("txtAmount");

            if (txtRate.Text == "")
            {
                txtAmount.Text = "";
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
            //txtBillNo.Text = "Create New.......";
            txtBillNo.Text = "";
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
            txtRoundingOff.Text = "0.00";
            txtNet.Text = "0.00";
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
                    ////txtCoupFrom.Text = (coupon.Count() + 1).ToString();
                    //foreach (var coup in coupon)
                    //{
                    //    txtCoupFrom.Text = (coup.COUP_Id + 1).ToString();
                    //    if (coup.COUP_Id < 0)
                    //    {
                    //        txtCoupFrom.Text = "1";
                    //    }
                    //    break;
                    //}
                    //if (txtCoupFrom.Text == "")
                    //{
                    //    txtCoupFrom.Text = "1";
                    //}
                    if (txtCoupFrom.Text != "" && txtQuantity.Text != "")
                    {
                        txtCoupTo.Text = (Convert.ToInt32(txtCoupFrom.Text) + Convert.ToInt32(txtQuantity.Text) - 1).ToString();
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
        protected void txtSerTaxPer_TextChanged(object sender, EventArgs eventArgs)
        {

            if (txtTotal.Text == "0.00")
                txtSerTaxPer.Text = "0.00";

            if (txtSerTaxPer.Text != "0.00" & txtSerTaxPer.Text != "" & txtTotal.Text != "0.00")
            {
                if (Convert.ToDecimal(txtSerTaxPer.Text) > 90)
                {
                    string msg = "alert(' Service tax percentage should not be greater than 90%')";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                    txtSerTaxPer.Text = "0.00";
                }
            }
            CalculateBill();
        }
        protected void txtSwachhBharatTaxPer_TextChanged(object sender, EventArgs eventArgs)
        {

            if (txtTotal.Text == "0.00")
                txtSwachhBharatTaxPer.Text = "0.00";

            if (txtSwachhBharatTaxPer.Text != "0.00" & txtSwachhBharatTaxPer.Text != "" & txtTotal.Text != "0.00")
            {
                if (Convert.ToDecimal(txtSwachhBharatTaxPer.Text) > 90)
                {
                    string msg = "alert(' Swachh Bharat Cess percentage should not be greater than 90%')";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                    txtSwachhBharatTaxPer.Text = "0.00";
                }
            }
            CalculateBill();
        }
        protected void txtKisanKrishiTaxPer_TextChanged(object sender, EventArgs eventArgs)
        {

            if (txtTotal.Text == "0.00")
                txtKisanKrishiTaxPer.Text = "0.00";

            if (txtKisanKrishiTaxPer.Text != "0.00" & txtKisanKrishiTaxPer.Text != "" & txtTotal.Text != "0.00")
            {
                if (Convert.ToDecimal(txtKisanKrishiTaxPer.Text) > 90)
                {
                    string msg = "alert(' Krishi Kalyan Cess percentage should not be greater than 90%')";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                    txtKisanKrishiTaxPer.Text = "0.00";
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
            //Sertax
            if (lblSerTaxPer.Text == "")
                lblSerTaxPer.Text = "0.00";
            txtServiceTax.Text = Convert.ToDecimal((GrossAmount - Convert.ToDecimal(txtDiscount.Text)) * Convert.ToDecimal(lblSerTaxPer.Text.Replace("(", "").Replace("%)", "")) / 100).ToString("0.00");
            if (txtSerTaxPer.Text == "")
                txtSerTaxPer.Text = "0.00";
            txtServiceTax.Text = Convert.ToDecimal((GrossAmount - Convert.ToDecimal(txtDiscount.Text)) * Convert.ToDecimal(txtSerTaxPer.Text) / 100).ToString("0.00");

            //swachh bharat tax
            if (lblSwachhBharatTaxPer.Text == "")
                lblSwachhBharatTaxPer.Text = "0.00";
            txtSwachhBharatTax.Text = Convert.ToDecimal((GrossAmount - Convert.ToDecimal(txtDiscount.Text)) * Convert.ToDecimal(lblSwachhBharatTaxPer.Text.Replace("(", "").Replace("%)", "")) / 100).ToString("0.00");
            if (txtSwachhBharatTaxPer.Text == "")
                txtSwachhBharatTaxPer.Text = "0.00";
            txtSwachhBharatTax.Text = Convert.ToDecimal((GrossAmount - Convert.ToDecimal(txtDiscount.Text)) * Convert.ToDecimal(txtSwachhBharatTaxPer.Text) / 100).ToString("0.00");

            //KisanKrishi tax
            if (lblKisanKrishiTaxPer.Text == "")
                lblKisanKrishiTaxPer.Text = "0.00";
            txtKisanKrishiTax.Text = Convert.ToDecimal((GrossAmount - Convert.ToDecimal(txtDiscount.Text)) * Convert.ToDecimal(lblKisanKrishiTaxPer.Text.Replace("(", "").Replace("%)", "")) / 100).ToString("0.00");
            if (txtKisanKrishiTaxPer.Text == "")
                txtKisanKrishiTaxPer.Text = "0.00";
            txtKisanKrishiTax.Text = Convert.ToDecimal((GrossAmount - Convert.ToDecimal(txtDiscount.Text)) * Convert.ToDecimal(txtKisanKrishiTaxPer.Text) / 100).ToString("0.00");

            ////Edu Tax        
            //txtEducationTax.Text = Convert.ToDecimal(Convert.ToDecimal(txtServiceTax.Text) * 2 / 100).ToString("0.00");

            ////Second higher secondaru edu tax
            //txtHigherEduTax.Text = Convert.ToDecimal(Convert.ToDecimal(txtServiceTax.Text) * 1 / 100).ToString("0.00");

            //Gross
            //txtNet.Text = (GrossAmount - Convert.ToDecimal(txtDiscount.Text) + Convert.ToDecimal(txtServiceTax.Text) + Convert.ToDecimal(txtEducationTax.Text) + Convert.ToDecimal(txtHigherEduTax.Text)).ToString();
            txtNet.Text = (GrossAmount - Convert.ToDecimal(txtDiscount.Text) + Convert.ToDecimal(txtServiceTax.Text) + Convert.ToDecimal(txtSwachhBharatTax.Text) + Convert.ToDecimal(txtKisanKrishiTax.Text)).ToString();

            //Round set
            txtNet.Text = Math.Round(Convert.ToDecimal(txtNet.Text)).ToString("0.00");

            //Round
            //decimal varResultGrossAmt = (GrossAmount - Convert.ToDecimal(txtDiscount.Text)) + Convert.ToDecimal(txtServiceTax.Text) + Convert.ToDecimal(txtEducationTax.Text) + Convert.ToDecimal(txtHigherEduTax.Text);
            decimal varResultGrossAmt = (GrossAmount - Convert.ToDecimal(txtDiscount.Text)) + Convert.ToDecimal(txtServiceTax.Text) + Convert.ToDecimal(txtSwachhBharatTax.Text) + Convert.ToDecimal(txtKisanKrishiTax.Text);
            if (Convert.ToDecimal(txtNet.Text) > varResultGrossAmt)
                txtRoundingOff.Text = "+" + (Convert.ToDecimal(txtNet.Text) - varResultGrossAmt).ToString("0.00");
            else
                txtRoundingOff.Text = (Convert.ToDecimal(txtNet.Text) - varResultGrossAmt).ToString("0.00");

        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            //Session["BillId"] = Convert.ToString(txtBillNo.Text);
            //Response.Redirect("ReportPDF.aspx");
            printBill(txtBillNo.Text);
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

            var bill = dc.Bill_View(BillNumber, 0, 0, "", 0, false, false, null, null);
            foreach (var b in bill)
            {
                LoadClientList();
                ddlClient.SelectedValue = b.BILL_CL_Id.ToString();
                LoadSiteList();
                ddlSite.SelectedValue = b.BILL_SITE_Id.ToString();
                //LoadServiceTax();
                //LoadSwachhBharatTax();
                //LoadKisanKrishiTax();
                txtAddress.Text = b.CL_OfficeAddress_var;
                txtBillNo.Text = b.BILL_Id.ToString();
                txtDate.Text = Convert.ToDateTime(b.BILL_Date_dt).ToString("dd/MM/yyyy");
                txtWorkOrderNo.Text = b.BILL_WorkOrderNo_var;
                txtRecordNo.Text = b.BILL_RecordNo_int.ToString();
                txtRecordType.Text = b.BILL_RecordType_var;
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
                }
                lblSerTaxPer.Text = "(" + b.BILL_SerTax_num.ToString() + "%)";
                if (b.BILL_SwachhBharatTax_num != null)
                    lblSwachhBharatTaxPer.Text = "(" + b.BILL_SwachhBharatTax_num.ToString() + "%)";
                else
                    lblSwachhBharatTaxPer.Text = "(" + "0" + "%)";

                if (b.BILL_KisanKrishiTax_num != null)
                    lblKisanKrishiTaxPer.Text = "(" + b.BILL_KisanKrishiTax_num.ToString() + "%)";
                else
                    lblKisanKrishiTaxPer.Text = "(" + "0" + "%)";

                txtSerTaxPer.Text = b.BILL_SerTax_num.ToString();
                if (b.BILL_SwachhBharatTax_num != null)
                    txtSwachhBharatTaxPer.Text = b.BILL_SwachhBharatTax_num.ToString();
                else
                    txtSwachhBharatTaxPer.Text = "0" ;

                if (b.BILL_KisanKrishiTax_num != null)
                    txtKisanKrishiTaxPer.Text = b.BILL_KisanKrishiTax_num.ToString() ;
                else
                    txtKisanKrishiTaxPer.Text = "0";
                txtServiceTax.Text = Convert.ToDecimal(b.BILL_SerTaxAmt_num).ToString("0.00");
                //txtEducationTax.Text = Convert.ToDecimal(b.BILL_EdCessAmt_num).ToString("0.00");
                //txtHigherEduTax.Text = Convert.ToDecimal(b.BILL_HighEdCessAmt_num).ToString("0.00");
                txtSwachhBharatTax.Text = Convert.ToDecimal(b.BILL_SwachhBharatTaxAmt_num).ToString("0.00");
                txtKisanKrishiTax.Text = Convert.ToDecimal(b.BILL_KisanKrishiTaxAmt_num).ToString("0.00");
                txtRoundingOff.Text = Convert.ToDecimal(b.BILL_RoundOffAmt_num).ToString("0.00");
                txtNet.Text = Convert.ToDecimal(b.BILL_NetAmt_num).ToString("0.00");
            }
            var billDetail = dc.BillDetail_View(BillNumber);
            foreach (var bd in billDetail)
            {
                AddRowBillDetail();
                TextBox txtDescription = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtDescription");
                TextBox txtQuantity = (TextBox)grdDetails.Rows[i].Cells[4].FindControl("txtQuantity");
                TextBox txtRate = (TextBox)grdDetails.Rows[i].Cells[5].FindControl("txtRate");
                TextBox txtAmount = (TextBox)grdDetails.Rows[i].Cells[6].FindControl("txtAmount");
                TextBox txtCoupFrom = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtCoupFrom");
                TextBox txtCoupTo = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtCoupTo");

                txtDescription.Text = bd.BILLD_TEST_Name_var;
                txtQuantity.Text = bd.BILLD_Quantity_int.ToString();
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
        protected bool ValidateData()
        {
            bool valid = true;
            string msg = "";
            int billNo = 0;
            if (txtBillNo.Text == "")
            {
                msg = "alert('Please Enter Bill Number..')";
                txtBillNo.Focus();
                valid = false;
            }
            else if (int.TryParse(txtBillNo.Text, out billNo) == false)
            {
                msg = "alert('Invalid Bill Number..')";
                txtBillNo.Focus();
                valid = false;
            }
            //else if (Convert.ToInt32(txtBillNo.Text) > 0)
            //{
            //    msg = "alert('Invalid Bill Number, Bill number should be less than zero.')";
            //    txtBillNo.Focus();
            //    valid = false;
            //}
            else if (txtRecordType.Text == "")
            {
                msg = "alert('Please Enter Record Type..')";
                txtRecordType.Focus();
                valid = false;
            }
            else if (txtRecordNo.Text == "")
            {
                msg = "alert('Please Enter Record Number..')";
                txtRecordType.Focus();
                valid = false;
            }
            else if (int.TryParse(txtRecordNo.Text, out billNo) == false)
            {
                msg = "alert('Invalid Record Number..')";
                txtRecordNo.Focus();
                valid = false;
            }
            else if (Convert.ToInt32(txtRecordNo.Text) == 0 &&
                (int.TryParse(txtBillNo.Text, out billNo) == false ||
                  (int.TryParse(txtBillNo.Text, out billNo) == true && Convert.ToInt32(txtBillNo.Text) > 0)))
            {
                msg = "alert('Invalid Record Number..')";
                txtRecordNo.Focus();
                valid = false;
            }
            //else if (DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null) > DateTime.ParseExact("30/04/2017", "dd/MM/yyyy", null))
            else if (DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null) > DateTime.ParseExact("30/06/2017", "dd/MM/yyyy", null))
            {
                msg = "alert('Bill date should be less than or equal to 30/06/2017..')";
                txtDate.Focus();
                valid = false;
            }
            else
            {
                var bill = dc.Bill_View(txtBillNo.Text, 0, 0, "", 0, false, false, null, null);
                if (bill.Count() > 0)
                {
                    msg = "alert('Bill Already Exist..')";
                    txtBillNo.Focus();
                    valid = false;
                }
            }
            if (valid == true)
            {
                var inward = dc.Inward_View(0, Convert.ToInt32(txtRecordNo.Text), txtRecordType.Text, null, null).ToList();
                if (inward.Count() == 0)
                {
                    msg = "alert('Inward not available with entered record number and record type..')";
                    txtRecordNo.Focus();
                    valid = false;
                }
                else if (inward.FirstOrDefault().INWD_BILL_Id != "0" && inward.FirstOrDefault().INWD_BILL_Id != null)
                {
                    msg = "alert('Bill already generated for entered record number and record type..')";
                    txtRecordNo.Focus();
                    valid = false;
                }
                else if (Convert.ToDateTime(inward.FirstOrDefault().INWD_CollectionDate_dt) > DateTime.ParseExact("30/06/2017", "dd/MM/yyyy", null))                    
                {
                    msg = "alert('Can not generate bill for entered record number and record type, collection date is greater than 30/06/2017')";
                    txtRecordNo.Focus();
                    valid = false;
                }
            }

            if (valid == false)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
            }
            return valid;
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            bool updateBillFlag = true, billStatus = false;
            string TallyNarration = "";
            int BillNo = 0;
            //byte couponStatus = 0;

            if (ValidateData() == false)
            {
                updateBillFlag = false;
            }
            if (txtBillNo.Text != "Create New......." && updateBillFlag == true)
            {
                BillNo = Convert.ToInt32(txtBillNo.Text);
            }
            
            //check recipt entry done
            if (BillNo > 0 && updateBillFlag == true)
            {
                var rcpt = dc.CashDetail_View_bill(txtBillNo.Text).ToList();
                if (rcpt.Count > 0)
                {
                    string msg = "alert('Receipt has been added for this Bill Number  !'+ '\\n' +'It can not be modified ')";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
                    updateBillFlag = false;
                }
            }
            if (ddlStatus.SelectedValue == "Ok")
            {
                billStatus = false;
                //couponStatus = 0;
            }
            else
            {
                billStatus = true;
                //couponStatus = 2;
            }
            //check for duplicate coupon numbers
            //if (lblCouponBill.Text == "True" && updateBillFlag == true)
            //{
            //    foreach (GridViewRow row in grdDetails.Rows)
            //    {
            //        TextBox txtCoupFrom = (TextBox)row.FindControl("txtCoupFrom");
            //        TextBox txtCoupTo = (TextBox)row.FindControl("txtCoupTo");
            //        for (int i = Convert.ToInt32(txtCoupFrom.Text); i <= Convert.ToInt32(txtCoupTo.Text); i++)
            //        {
            //            var coupon = dc.Coupon_View(BillNo, i, 0, Convert.ToInt32(ddlClient.SelectedValue), 0, 0, null);
            //            if (coupon.Count() > 0)
            //            {
            //                string msg = "alert('Coupon number " + i.ToString() + " already alloted  !'+ '\\n' +'Can not save bill.')";
            //                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
            //                updateBillFlag = false;
            //                break;
            //            }
            //            //var coupon1 = dc.Coupon_View(BillNo, i, 1, Convert.ToInt32(ddlClient.SelectedValue), 0, 0, null);
            //            var coupon1 = dc.Coupon_View(BillNo, i, 1, 0, 0, 0, null);
            //            if (coupon1.Count() > 0)
            //            {
            //                string msg = "alert('Coupon number " + i.ToString() + " already used  !'+ '\\n' +'Can not change bill.')";
            //                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
            //                updateBillFlag = false;
            //                break;
            //            }
            //        }
            //    }
            //}
            if (updateBillFlag == true)
            {
                foreach (GridViewRow row in grdDetails.Rows)
                {
                    TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
                    //TextBox txtCoupFrom = (TextBox)row.FindControl("txtCoupFrom");
                    //TextBox txtCoupTo = (TextBox)row.FindControl("txtCoupTo");
                    //if (lblCouponBill.Text == "True")
                    //{
                    //    txtDescription.Text = "Bill For Concrete Cube Testing Coupons - From " + txtCoupFrom.Text + " To " + txtCoupTo.Text;
                    //}
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
                dc.BillDetail_Update(BillNo.ToString(), 0, 0, 0, "", 0, "", 0, 0, false, 0, true);
                //dc.Coupon_Update(BillNo, 0, 0, 0, null, 0, null, "", 0, null, false, true);
                BillNo = dc.Bill_Update_Previous(BillNo.ToString(), Convert.ToInt32(ddlClient.SelectedValue), ddlClient.SelectedItem.Text, Convert.ToInt32(ddlSite.SelectedValue), optPercentage.Checked, Convert.ToDecimal(txtDiscPer.Text), Convert.ToDecimal(txtDiscount.Text), Convert.ToDecimal(txtSerTaxPer.Text),
                    Convert.ToDecimal(txtServiceTax.Text), Convert.ToDecimal(txtSwachhBharatTaxPer.Text), Convert.ToDecimal(txtSwachhBharatTax.Text), Convert.ToDecimal(txtKisanKrishiTaxPer.Text), Convert.ToDecimal(txtKisanKrishiTax.Text), Convert.ToDecimal(txtNet.Text), 0, 0, Convert.ToDecimal(txtRoundingOff.Text), txtRecordType.Text, Convert.ToInt32(txtRecordNo.Text), billStatus, TallyNarration,
                    false, Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), Convert.ToBoolean(lblCouponBill.Text), DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null), txtWorkOrderNo.Text.Trim());
                txtBillNo.Text = BillNo.ToString();
                //bool sitespecstatus = false;
                //if (lblSiteSpecCoupStatus.Text == "Site Specific Coupons")
                //{
                //    sitespecstatus = true;
                //}
                //if (chkAsPerClient.Enabled == true && chkAsPerClient.Checked == true &&
                //    lblSiteSpecCoupStatus.Text != lblClientCouponSetting.Text)
                //{
                //    if (lblClientCouponSetting.Text == "Site Specific Coupons")
                //    {
                //        sitespecstatus = true;
                //    }
                //    else
                //    {
                //        sitespecstatus = false;
                //    }
                //}
                foreach (GridViewRow row in grdDetails.Rows)
                {
                    TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
                    TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
                    TextBox txtRate = (TextBox)row.FindControl("txtRate");
                    TextBox txtAmount = (TextBox)row.FindControl("txtAmount");
                    TextBox txtCoupFrom = (TextBox)row.FindControl("txtCoupFrom");
                    TextBox txtCoupTo = (TextBox)row.FindControl("txtCoupTo");

                    var b = dc.BillDetail_Update(BillNo.ToString(), Convert.ToInt32(row.Cells[2].Text), Convert.ToInt32(txtQuantity.Text), Convert.ToDecimal(txtAmount.Text), txtDescription.Text, Convert.ToDecimal(txtRate.Text), "", 0, 0, false, 0,false);
                    //if (lblCouponBill.Text == "True")
                    //{
                    //    for (int i = Convert.ToInt32(txtCoupFrom.Text); i <= Convert.ToInt32(txtCoupTo.Text); i++)
                    //    {
                    //        dc.Coupon_Update(BillNo, i, Convert.ToInt32(ddlClient.SelectedValue), Convert.ToInt32(ddlSite.SelectedValue), DateTime.Now, couponStatus, null, "", 0, DateTime.Now.AddDays(730), sitespecstatus, false);
                    //    }
                    //}
                }
                dc.Inward_Update_BillNo(Convert.ToInt32(txtRecordNo.Text) , txtRecordType.Text , BillNo.ToString());
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
            CalculateBill();
        }

        private void LoadServiceTax()
        {
            //if (ddlSite.SelectedIndex > 0)
            //{        
            //decimal xSrvTax = dc.Site_View_ServiceTax(Convert.ToInt32(ddlSite.SelectedItem.Value));
            int siteId = 0;
            if (ddlSite.SelectedIndex > 0)
            {
                siteId = Convert.ToInt32(ddlSite.SelectedValue);
            }
            var master = dc.MasterSetting_View(siteId);
            decimal xSrvTax = 0;
            //xSrvTax = Convert.ToDecimal(master.FirstOrDefault().MASTER_ServiceTax_num);
            foreach (var serTax in master)
            {
                xSrvTax = Convert.ToDecimal(serTax.MASTER_ServiceTax_num);
            }
            lblSerTaxPer.Text = "(" + xSrvTax.ToString() + "%)";
            txtSerTaxPer.Text = xSrvTax.ToString() ;
            //}
        }

        private void LoadSwachhBharatTax()
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
            txtSwachhBharatTaxPer.Text = xSwachhBharat.ToString();
        }

        private void LoadKisanKrishiTax()
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
            txtKisanKrishiTaxPer.Text = xKisanKrishi.ToString();
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


    }
}