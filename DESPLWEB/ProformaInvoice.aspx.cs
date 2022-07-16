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
    public partial class ProformaInvoice : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool userRight = false;
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Proforma Invoice";
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
                        lblProformaInvoiceId.Text = arrIndMsg[1].ToString().Trim();
                        arrIndMsg = arrMsgs[1].Split('=');
                        lblCouponProformaInvoice.Text = arrIndMsg[1].ToString().Trim();
                    }
                    else
                    {
                        lblProformaInvoiceId.Text = "";
                        lblCouponProformaInvoice.Text = "False";
                    }
                    ////
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    LoadServiceTax();
                    LoadSwachhBharatTax();
                    LoadKisanKrishiTax();
                    LoadGSTax();
                    if (lblProformaInvoiceId.Text != "")
                    {
                        LoadProformaInvoiceDetail();
                    }
                    else
                    {
                        ClearData();
                        LoadClientList();
                        //AddRowProformaInvoiceDetail();
                        txtRecordNo.Text = "0";
                        txtRecordType.Text = "---";
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
        #region ProformaInvoiceDetailGridEdit
        protected void AddRowProformaInvoiceDetail()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["ProformaInvoiceDetailTable"] != null)
            {
                GetCurrentDataProformaInvoiceDetail();
                dt = (DataTable)ViewState["ProformaInvoiceDetailTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSACCode", typeof(string)));
                dt.Columns.Add(new DataColumn("txtQuantity", typeof(string)));
                dt.Columns.Add(new DataColumn("txtRate", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAmount", typeof(string)));
            }

            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txtDescription"] = string.Empty;
            if (txtRecordType.Text == "SO" || txtRecordType.Text == "GT")
                dr["txtSACCode"] = "998341";
            else
                dr["txtSACCode"] = "998346";
            dr["txtQuantity"] = string.Empty;
            dr["txtRate"] = string.Empty;
            dr["txtAmount"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["ProformaInvoiceDetailTable"] = dt;
            grdDetails.DataSource = dt;
            grdDetails.DataBind();
            SetPreviousDataProformaInvoiceDetail();
        }

        protected void DeleteRowProformaInvoiceDetail(int rowIndex)
        {
            GetCurrentDataProformaInvoiceDetail();
            DataTable dt = ViewState["ProformaInvoiceDetailTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["ProformaInvoiceDetailTable"] = dt;
            grdDetails.DataSource = dt;
            grdDetails.DataBind();
            SetPreviousDataProformaInvoiceDetail();
        }

        protected void SetPreviousDataProformaInvoiceDetail()
        {
            DataTable dt = (DataTable)ViewState["ProformaInvoiceDetailTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtDescription");
                TextBox box2 = (TextBox)grdDetails.Rows[i].Cells[4].FindControl("txtSACCode");
                TextBox box3 = (TextBox)grdDetails.Rows[i].Cells[5].FindControl("txtQuantity");
                TextBox box4 = (TextBox)grdDetails.Rows[i].Cells[6].FindControl("txtRate");
                TextBox box5 = (TextBox)grdDetails.Rows[i].Cells[7].FindControl("txtAmount");

                grdDetails.Rows[i].Cells[2].Text = (i + 1).ToString();
                box1.Text = dt.Rows[i]["txtDescription"].ToString();
                box2.Text = dt.Rows[i]["txtSACCode"].ToString();
                box3.Text = dt.Rows[i]["txtQuantity"].ToString();
                box4.Text = dt.Rows[i]["txtRate"].ToString();
                box5.Text = dt.Rows[i]["txtAmount"].ToString();
            }
        }

        protected void GetCurrentDataProformaInvoiceDetail()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDescription", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtSACCode", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtQuantity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAmount", typeof(string)));
            for (int i = 0; i < grdDetails.Rows.Count; i++)
            {
                TextBox box1 = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtDescription");
                TextBox box2 = (TextBox)grdDetails.Rows[i].Cells[4].FindControl("txtSACCode");
                TextBox box3 = (TextBox)grdDetails.Rows[i].Cells[5].FindControl("txtQuantity");
                TextBox box4 = (TextBox)grdDetails.Rows[i].Cells[6].FindControl("txtRate");
                TextBox box5 = (TextBox)grdDetails.Rows[i].Cells[7].FindControl("txtAmount");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txtDescription"] = box1.Text;
                drRow["txtSACCode"] = box2.Text;
                drRow["txtQuantity"] = box3.Text;
                drRow["txtRate"] = box4.Text;
                drRow["txtAmount"] = box5.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["ProformaInvoiceDetailTable"] = dtTable;
        }

        protected void imgInsert_Click(object sender, CommandEventArgs e)
        {
            AddRowProformaInvoiceDetail();
            CalculateProformaInvoice();
        }

        protected void imgDelete_Click(object sender, CommandEventArgs e)
        {
            if (grdDetails.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowProformaInvoiceDetail(gvr.RowIndex);
                CalculateProformaInvoice();
            }
        }


        protected void txtQuantity_TextChanged(object sender, EventArgs eventArgs)
        {
            TextBox tbox = (TextBox)sender;
            GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            int rowindex = row.RowIndex;
            //TextBox txtDescription = (TextBox)grdDetails.Rows[rowindex].Cells[1].FindControl("txtDescription");
            TextBox txtQuantity = (TextBox)grdDetails.Rows[rowindex].Cells[2].FindControl("txtQuantity");
            TextBox txtRate = (TextBox)grdDetails.Rows[rowindex].Cells[3].FindControl("txtRate");
            TextBox txtAmount = (TextBox)grdDetails.Rows[rowindex].Cells[4].FindControl("txtAmount");
            //TextBox txtCoupFrom = (TextBox)grdDetails.Rows[rowindex].Cells[2].FindControl("txtCoupFrom");
            //TextBox txtCoupTo = (TextBox)grdDetails.Rows[rowindex].Cells[3].FindControl("txtCoupTo");
            txtDiscPer.Text = "0.00";
            txtDiscount.Text = "0.00";

            if (txtQuantity.Text == "")
            {
                txtAmount.Text = "";
                //txtCoupTo.Text = "";
                //if (lblCouponProformaInvoice.Text == "True")
                //    txtDescription.Text = "";
            }
            //else if (lblCouponProformaInvoice.Text == "True" && txtCoupFrom.Text != "")
            //{
            //    txtCoupTo.Text = (Convert.ToInt32(txtCoupFrom.Text) + Convert.ToInt32(txtQuantity.Text) - 1).ToString();
            //    txtDescription.Text = "Proforma Invoice For Concrete Cube Testing Coupons - From " + txtCoupFrom.Text + " To " + txtCoupTo.Text;
            //}
            if (txtRate.Text != "" && txtQuantity.Text != "")
            {
                txtAmount.Text = Convert.ToDecimal(Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQuantity.Text)).ToString("0.00");
            }
            CalculateProformaInvoice();
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
            CalculateProformaInvoice();
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
            CalculateProformaInvoice();
        }

        public void ClearData()
        {
            txtProformaInvoiceNo.Text = "Create New.......";
            ddlSite.Items.Clear();
            txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            ddlStatus.SelectedValue = "Ok";
            txtAddress.Text = "";
            txtRecordNo.Text = "";
            txtWorkOrderNo.Text = "";
            grdDetails.DataSource = null;
            grdDetails.DataBind();
            AddRowProformaInvoiceDetail();
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
                LoadSiteList();
                var address = dc.Client_View(Convert.ToInt32(ddlClient.SelectedValue), 0, "", "");
                foreach (var adds in address)
                {
                    txtAddress.Text = adds.CL_OfficeAddress_var;
                    if (lblCouponProformaInvoice.Text == "True")
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

                //if (lblProformaInvoiceId.Text == "" && lblCouponProformaInvoice.Text == "True")
                //{
                //    TextBox txtCoupFrom = (TextBox)grdDetails.Rows[0].Cells[3].FindControl("txtCoupFrom");
                //    TextBox txtCoupTo = (TextBox)grdDetails.Rows[0].Cells[3].FindControl("txtCoupTo");
                //    TextBox txtQuantity = (TextBox)grdDetails.Rows[0].Cells[3].FindControl("txtQuantity");
                //    TextBox txtDescription = (TextBox)grdDetails.Rows[0].Cells[3].FindControl("txtDescription");
                //    txtCoupFrom.Text = "";
                //    txtCoupTo.Text = "";
                //    var coupon = dc.Coupon_View("", 0, 0, Convert.ToInt32(ddlClient.SelectedValue), 0, 0, null);
                //    //txtCoupFrom.Text = (coupon.Count() + 1).ToString();
                //    foreach (var coup in coupon)
                //    {
                //        txtCoupFrom.Text = (coup.COUP_Id + 1).ToString();
                //        if (coup.COUP_Id < 0)
                //        {
                //            txtCoupFrom.Text = "1";
                //        }
                //        break;
                //    }
                //    if (txtCoupFrom.Text == "")
                //    {
                //        txtCoupFrom.Text = "1";
                //    }
                //    if (txtCoupFrom.Text != "" && txtQuantity.Text != "")
                //    {
                //        txtCoupTo.Text = (Convert.ToInt32(txtCoupFrom.Text) + Convert.ToInt32(txtQuantity.Text) - 1).ToString();
                //        txtDescription.Text = "ProformaInvoice For Concrete Cube Testing Coupons - From " + txtCoupFrom.Text + " To " + txtCoupTo.Text;
                //    }
                //}
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
            CalculateProformaInvoice();
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
            CalculateProformaInvoice();
        }

        public void CalculateProformaInvoice()
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
            if (chkDiscount.Checked == true)
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
            printProformaInvoice(txtProformaInvoiceNo.Text);
        }
        private void printProformaInvoice(string ProformaInvoiceNo)
        {
            ProformaInvoiceUpdation ProInv = new ProformaInvoiceUpdation();
            ProInv.getProformaInvoicePrintString(ProformaInvoiceNo, "Print");
        }
        public void LoadProformaInvoiceDetail()
        {
            string ProformaInvoiceNumber = lblProformaInvoiceId.Text;
            int i = 0;

            var ProformaInvoice = dc.ProformaInvoice_View(ProformaInvoiceNumber, 0, 0, "", "", false, false, null, null);
            foreach (var b in ProformaInvoice)
            {
                LoadClientList();
                ddlClient.SelectedValue = b.PROINV_CL_Id.ToString();
                LoadSiteList();
                ddlSite.SelectedValue = b.PROINV_SITE_Id.ToString();
                txtAddress.Text = b.CL_OfficeAddress_var;
                txtProformaInvoiceNo.Text = b.PROINV_Id.ToString();
                txtDate.Text = Convert.ToDateTime(b.PROINV_Date_dt).ToString("dd/MM/yyyy");
                txtWorkOrderNo.Text = b.PROINV_WorkOrderNo_var;
                txtRecordNo.Text = b.PROINV_RecordNo_int.ToString();
                txtRecordType.Text = b.PROINV_RecordType_var;

                //LoadServiceTax();
                //LoadSwachhBharatTax();
                //LoadKisanKrishiTax();
                //LoadGSTax();

                if (b.PROINV_RecordType_var == "---")
                {
                    lblCouponProformaInvoice.Text = "True";
                }
                if (b.PROINV_Status_bit == true)
                    ddlStatus.SelectedValue = "Cancel";
                else
                    ddlStatus.SelectedValue = "Ok";

                txtDiscount.Text = Convert.ToDecimal(b.PROINV_DiscountAmt_num).ToString("0.00");
                if (txtDiscount.Text != "0.00" && txtDiscount.Text != "")
                {
                    chkDiscount.Checked = true;
                    txtDiscPer.Text = b.PROINV_Discount_num.ToString();
                    txtDiscPer.Enabled = true;
                    txtDiscPer.Visible = true;
                    optPercentage.Visible = true;
                    optLumpsum.Visible = true;

                    if (b.PROINV_DiscountPerStatus_bit == false)
                        optLumpsum.Checked = true;
                    else if (b.PROINV_DiscountPerStatus_bit == true)
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

                if (b.PROINV_SerTax_num != null && b.PROINV_SerTax_num != 0)
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
                if (b.PROINV_SwachhBharatTax_num != null && b.PROINV_SwachhBharatTax_num != 0)
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
                if (b.PROINV_KisanKrishiTax_num != null && b.PROINV_KisanKrishiTax_num != 0)
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

                if (b.PROINV_CGST_num != null && b.PROINV_CGST_num != 0)
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
                if (b.PROINV_SGST_num != null && b.PROINV_SGST_num != 0)
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
                if (b.PROINV_IGST_num != null && b.PROINV_IGST_num != 0)
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

                if (b.PROINV_SerTax_num != null)
                    lblSerTaxPer.Text = "(" + b.PROINV_SerTax_num.ToString() + "%)";
                else
                    lblSerTaxPer.Text = "(" + "0" + "%)";

                if (b.PROINV_SwachhBharatTax_num != null)
                    lblSwachhBharatTaxPer.Text = "(" + b.PROINV_SwachhBharatTax_num.ToString() + "%)";
                else
                    lblSwachhBharatTaxPer.Text = "(" + "0" + "%)";

                if (b.PROINV_KisanKrishiTax_num != null)
                    lblKisanKrishiTaxPer.Text = "(" + b.PROINV_KisanKrishiTax_num.ToString() + "%)";
                else
                    lblKisanKrishiTaxPer.Text = "(" + "0" + "%)";

                if (b.PROINV_CGST_num != null)
                    lblCGSTPer.Text = "(" + b.PROINV_CGST_num.ToString() + "%)";
                else
                    lblCGSTPer.Text = "(" + "0" + "%)";

                if (b.PROINV_SGST_num != null)

                    lblSGSTPer.Text = "(" + b.PROINV_SGST_num.ToString() + "%)";
                else
                    lblSGSTPer.Text = "(" + "0" + "%)";

                if (b.PROINV_IGST_num != null)
                    lblIGSTPer.Text = "(" + b.PROINV_IGST_num.ToString() + "%)";
                else
                    lblIGSTPer.Text = "(" + "0" + "%)";
                checkRateAsPerCurrentSetting();

                txtServiceTax.Text = Convert.ToDecimal(b.PROINV_SerTaxAmt_num).ToString("0.00");
                txtSwachhBharatTax.Text = Convert.ToDecimal(b.PROINV_SwachhBharatTaxAmt_num).ToString("0.00");
                txtKisanKrishiTax.Text = Convert.ToDecimal(b.PROINV_KisanKrishiTaxAmt_num).ToString("0.00");
                txtCGST.Text = Convert.ToDecimal(b.PROINV_CGSTAmt_num).ToString("0.00");
                txtSGST.Text = Convert.ToDecimal(b.PROINV_SGSTAmt_num).ToString("0.00");
                txtIGST.Text = Convert.ToDecimal(b.PROINV_IGSTAmt_num).ToString("0.00");

                txtRoundingOff.Text = Convert.ToDecimal(b.PROINV_RoundOffAmt_num).ToString("0.00");
                txtNet.Text = Convert.ToDecimal(b.PROINV_NetAmt_num).ToString("0.00");
                if (b.PROINV_AdvancePaid_num != null)
                    txtAdvancePaid.Text = Convert.ToDecimal(b.PROINV_AdvancePaid_num).ToString("0.00");
            }
            var ProformaInvoiceDetail = dc.ProformaInvoiceDetail_View(ProformaInvoiceNumber);
            foreach (var bd in ProformaInvoiceDetail)
            {
                AddRowProformaInvoiceDetail();
                TextBox txtDescription = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtDescription");
                TextBox txtSACCode = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtSACCode");
                TextBox txtQuantity = (TextBox)grdDetails.Rows[i].Cells[4].FindControl("txtQuantity");
                TextBox txtRate = (TextBox)grdDetails.Rows[i].Cells[5].FindControl("txtRate");
                TextBox txtAmount = (TextBox)grdDetails.Rows[i].Cells[6].FindControl("txtAmount");
                //TextBox txtCoupFrom = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtCoupFrom");
                //TextBox txtCoupTo = (TextBox)grdDetails.Rows[i].Cells[3].FindControl("txtCoupTo");

                txtDescription.Text = bd.PROINVD_TEST_Name_var;
                txtSACCode.Text = bd.PROINVD_SACCode_var;
                txtQuantity.Text = bd.PROINVD_Quantity_int.ToString();
                txtRate.Text = Convert.ToDecimal(bd.PROINVD_Rate_num).ToString("0.00");
                txtAmount.Text = Convert.ToDecimal(bd.PROINVD_Amt_num).ToString("0.00");
                //if (lblCouponProformaInvoice.Text == "True")
                //{
                //    txtCoupFrom.ReadOnly = true;
                //    txtCoupTo.ReadOnly = true;
                //    txtQuantity.ReadOnly = true;
                //    ddlClient.Enabled = false;
                //    ddlSite.Enabled = false;

                //    string[] words1 = Regex.Split(txtDescription.Text, "From");
                //    string[] words2 = Regex.Split(words1[1], "To");
                //    txtCoupFrom.Text = words2[0].Trim();
                //    txtCoupTo.Text = words2[1].Trim();                    
                //}
                i++;
            }
            //if (lblCouponProformaInvoice.Text == "True")
            //{
            //    TextBox txtCoupFrom = (TextBox)grdDetails.Rows[0].Cells[3].FindControl("txtCoupFrom");
            //    var coup = dc.Coupon_View(ProformaInvoiceNumber, 0, 0, 0, 0, 0, null);
            //    foreach (var c in coup)
            //    {
            //        if (c.COUP_SiteSpecStatus_bit == true)
            //        {
            //            lblSiteSpecCoupStatus.Text = "Site Specific Coupons";
            //        }
            //        else
            //        {
            //            lblSiteSpecCoupStatus.Text = "Client Specific Coupons";
            //        }
            //        break;
            //    }

            //    var client = dc.Client_View(Convert.ToInt32(ddlClient.SelectedValue), 0, "", "");
            //    foreach (var cl in client)
            //    {
            //        if (cl.CL_SiteSpecificCoupon_bit == true)
            //        {
            //            lblClientCouponSetting.Text = "Site Specific Coupons";
            //        }
            //        else
            //        {
            //            lblClientCouponSetting.Text = "Client Specific Coupons";
            //        }
            //        var coupon1 = dc.Coupon_View(ProformaInvoiceNumber, 0, 1, 0, 0, 0, null);
            //        if (coupon1.Count() == 0)
            //        {
            //            if ((cl.CL_SiteSpecificCoupon_bit == true && lblSiteSpecCoupStatus.Text == "Client Specific Coupons")
            //                || (cl.CL_SiteSpecificCoupon_bit == false && lblSiteSpecCoupStatus.Text == "Site Specific Coupons"))
            //            {
            //                chkAsPerClient.Visible = true;
            //                chkAsPerClient.Enabled = true;
            //            }

            //        }
            //        else
            //        {
            //            chkAsPerClient.Enabled = false;
            //            if (lblClientCouponSetting.Text != lblSiteSpecCoupStatus.Text)
            //            {
            //                chkAsPerClient.Visible = true;
            //            }
            //        }
            //    }
            //}
            txtProformaInvoiceNo.ReadOnly = true;
            txtDate.ReadOnly = true;
            txtRecordNo.ReadOnly = true;
            txtRecordType.ReadOnly = true;
            CalendarExtender1.Enabled = false;
            if (grdDetails.Rows.Count == 0)
            {
                //lblMessage.Text = "No records Found !!";
                //lblMessage.Visible = true;
                AddRowProformaInvoiceDetail();
            }
            CalculateProformaInvoice();
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
                DateTime ProformaInvoiceDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

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
                    var master = dc.GST_View(1, ProformaInvoiceDate);
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

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            bool updateProformaInvoiceFlag = true, ProformaInvoiceStatus = false;
            string TallyNarration = "";
            string ProformaInvoiceNo = "0";
            //byte couponStatus = 0;

            if (txtProformaInvoiceNo.Text != "Create New.......")
            {
                ProformaInvoiceNo = txtProformaInvoiceNo.Text;
            }
            if (ddlStatus.SelectedValue == "Ok")
            {
                ProformaInvoiceStatus = false;
                //couponStatus = 0;
            }
            else
            {
                ProformaInvoiceStatus = true;
                //couponStatus = 2;
            }

            clsData clsDt = new clsData();
            
            //check gst information is updated or not
            if (clsDt.checkGSTInfoUpdated(ddlClient.SelectedValue, ddlSite.SelectedValue, "") == false)
            {
                updateProformaInvoiceFlag = false;
                string msg = "alert('Please update client & site GST details. Can not generate ProformaInvoice.')";
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
            }

            ////check for duplicate coupon numbers
            //if (lblCouponProformaInvoice.Text == "True" && updateProformaInvoiceFlag == true)
            //{
            //    foreach (GridViewRow row in grdDetails.Rows)
            //    {
            //        TextBox txtCoupFrom = (TextBox)row.FindControl("txtCoupFrom");
            //        TextBox txtCoupTo = (TextBox)row.FindControl("txtCoupTo");
            //        for (int i = Convert.ToInt32(txtCoupFrom.Text); i <= Convert.ToInt32(txtCoupTo.Text); i++)
            //        {
            //            var coupon = dc.Coupon_View(ProformaInvoiceNo, i, 0, Convert.ToInt32(ddlClient.SelectedValue), 0, 0, null);
            //            if (coupon.Count() > 0)
            //            {
            //                string msg = "alert('Coupon number " + i.ToString() + " already alloted  !'+ '\\n' +'Can not save ProformaInvoice.')";
            //                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
            //                updateProformaInvoiceFlag = false;
            //                break;
            //            }
            //            //var coupon1 = dc.Coupon_View(ProformaInvoiceNo, i, 1, Convert.ToInt32(ddlClient.SelectedValue), 0, 0, null);
            //            var coupon1 = dc.Coupon_View(ProformaInvoiceNo, i, 1, 0, 0, 0, null);
            //            if (coupon1.Count() > 0)
            //            {
            //                string msg = "alert('Coupon number " + i.ToString() + " already used  !'+ '\\n' +'Can not change ProformaInvoice.')";
            //                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", msg, true);
            //                updateProformaInvoiceFlag = false;
            //                break;
            //            }
            //        }
            //    }
            //}
            if (txtAdvancePaid.Text == "")
                txtAdvancePaid.Text = "0.00";
            if (updateProformaInvoiceFlag == true)
            {
                foreach (GridViewRow row in grdDetails.Rows)
                {
                    TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
                    TextBox txtCoupFrom = (TextBox)row.FindControl("txtCoupFrom");
                    TextBox txtCoupTo = (TextBox)row.FindControl("txtCoupTo");
                    if (lblCouponProformaInvoice.Text == "True")
                    {
                        txtDescription.Text = "ProformaInvoice For Concrete Cube Testing Coupons - From " + txtCoupFrom.Text + " To " + txtCoupTo.Text;
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
                dc.ProformaInvoiceDetail_Update(ProformaInvoiceNo, 0, 0, 0, "", 0, "",0,0, true);
                dc.Coupon_Update(ProformaInvoiceNo, 0, 0, 0, null, 0, null, "", 0, null, false, true);
                bool ProformaInvoicePrintLockStatus = false;
                bool insertProformaInvoice = false;
                if (ProformaInvoiceNo == "0")
                {
                    //var client = dc.Client_View(Convert.ToInt32(ddlClient.SelectedValue), 0, "", "");
                    //foreach (var cl in client)
                    //{
                    //    ProformaInvoicePrintLockStatus = Convert.ToBoolean(cl.CL_MonthlyBilling_bit);
                    //}
                    var site = dc.Site_View(Convert.ToInt32(ddlSite.SelectedValue), 0, 0, "");
                    foreach (var st in site)
                    {
                        ProformaInvoicePrintLockStatus = Convert.ToBoolean(st.SITE_MonthlyBillingStatus_bit);
                    }
                    int NewrecNo = 0;
                    clsData clsObj = new clsData();
                    NewrecNo = clsObj.GetnUpdateRecordNo("ProformaInvoiceNo");
                    var master = dc.MasterSetting_View(0);
                    foreach (var mst in master)
                    {
                        //BillNo = mst.MASTER_AccountingYear_var + "/" + mst.MASTER_Region_var + "/" + NewrecNo.ToString();
                        ProformaInvoiceNo = mst.MASTER_AccountingYear_var + mst.MASTER_Region_var + NewrecNo.ToString();
                    }
                    insertProformaInvoice = true;
                }
                //ProformaInvoiceNo = 
                dc.ProformaInvoice_Update(ProformaInvoiceNo, Convert.ToInt32(ddlClient.SelectedValue), ddlClient.SelectedItem.Text, Convert.ToInt32(ddlSite.SelectedValue), optPercentage.Checked, Convert.ToDecimal(txtDiscPer.Text), Convert.ToDecimal(txtDiscount.Text), Convert.ToDecimal(lblSerTaxPer.Text.Replace("(", "").Replace("%)", "")),
                    Convert.ToDecimal(txtServiceTax.Text), Convert.ToDecimal(lblSwachhBharatTaxPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtSwachhBharatTax.Text), Convert.ToDecimal(lblKisanKrishiTaxPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtKisanKrishiTax.Text),
                    Convert.ToDecimal(lblCGSTPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtCGST.Text), Convert.ToDecimal(lblSGSTPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtSGST.Text), Convert.ToDecimal(lblIGSTPer.Text.Replace("(", "").Replace("%)", "")), Convert.ToDecimal(txtIGST.Text), Convert.ToDecimal(txtAdvancePaid.Text),
                    Convert.ToDecimal(txtNet.Text), 0, 0, Convert.ToDecimal(txtRoundingOff.Text), txtRecordType.Text, txtRecordNo.Text, ProformaInvoiceStatus, TallyNarration,
                    false, Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), Convert.ToBoolean(lblCouponProformaInvoice.Text), txtWorkOrderNo.Text.Trim(), insertProformaInvoice);
                txtProformaInvoiceNo.Text = ProformaInvoiceNo.ToString();
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
                    TextBox txtSACCode = (TextBox)row.FindControl("txtSACCode");
                    TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
                    TextBox txtRate = (TextBox)row.FindControl("txtRate");
                    TextBox txtAmount = (TextBox)row.FindControl("txtAmount");
                    //TextBox txtCoupFrom = (TextBox)row.FindControl("txtCoupFrom");
                    //TextBox txtCoupTo = (TextBox)row.FindControl("txtCoupTo");

                    dc.ProformaInvoiceDetail_Update(ProformaInvoiceNo, Convert.ToInt32(row.Cells[2].Text), Convert.ToInt32(txtQuantity.Text), Convert.ToDecimal(txtAmount.Text), txtDescription.Text, Convert.ToDecimal(txtRate.Text), txtSACCode.Text, 0,0,false);
                    //if (lblCouponProformaInvoice.Text == "True")
                    //{
                    //    for (int i = Convert.ToInt32(txtCoupFrom.Text); i <= Convert.ToInt32(txtCoupTo.Text); i++)
                    //    {
                    //        dc.Coupon_Update(ProformaInvoiceNo, i, Convert.ToInt32(ddlClient.SelectedValue), Convert.ToInt32(ddlSite.SelectedValue), DateTime.Now, couponStatus, null, "", 0, DateTime.Now.AddDays(730), sitespecstatus, false);
                    //    }
                    //}
                }

                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "alert", "alert('Records Sucessfully Updated ')", true);
                lnkSave.Visible = false;
                lnkPrint.Visible = true;
                //ClearData();
            }
        }

        protected void imgClosePopup_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProformaInvoiceStatus.aspx");
        }

        protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadServiceTax();
            LoadSwachhBharatTax();
            LoadKisanKrishiTax();
            LoadGSTax();
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
                DateTime ProformaInvoiceDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

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
                    var master = dc.GST_View(1, ProformaInvoiceDate);
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
            //TextBox tbox = (TextBox)sender;
            //GridViewRow row = (GridViewRow)tbox.Parent.Parent;
            //int rowindex = row.RowIndex;
            //TextBox txtCoupFrom = (TextBox)grdDetails.Rows[rowindex].Cells[3].FindControl("txtCoupFrom");
            //TextBox txtCoupTo = (TextBox)grdDetails.Rows[rowindex].Cells[3].FindControl("txtCoupTo");
            //TextBox txtQuantity = (TextBox)grdDetails.Rows[rowindex].Cells[2].FindControl("txtQuantity");
            //TextBox txtRate = (TextBox)grdDetails.Rows[rowindex].Cells[3].FindControl("txtRate");
            //TextBox txtAmount = (TextBox)grdDetails.Rows[rowindex].Cells[4].FindControl("txtAmount");

            //if (txtQuantity.Text == "")
            //{
            //    txtAmount.Text = "";
            //}
            //else if (txtCoupFrom.Text != "")
            //{
            //    txtCoupTo.Text = (Convert.ToInt32(txtCoupFrom.Text) + Convert.ToInt32(txtQuantity.Text) - 1).ToString();
            //}
            //if (txtRate.Text != "" && txtQuantity.Text != "")
            //{
            //    txtAmount.Text = Convert.ToDecimal(Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtQuantity.Text)).ToString("0.00");
            //}
            //CalculateProformaInvoice();
        }

        protected void lnkRateAsPerCurrent_Click(object sender, EventArgs e)
        {
            if (lnkRateAsPerCurrent.Text == "Calculate Tax As Per Current Setting")
            {
                lnkRateAsPerCurrent.Text = "Calculate Tax As Per ProformaInvoice Old Setting";
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
                var ProformaInvoice = dc.ProformaInvoice_View(txtProformaInvoiceNo.Text, 0, 0, "", "", false, false, null, null);
                foreach (var b in ProformaInvoice)
                {

                    if (b.PROINV_CGST_num != null && b.PROINV_CGST_num != 0)
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
                    if (b.PROINV_SGST_num != null && b.PROINV_SGST_num != 0)
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
                    if (b.PROINV_IGST_num != null && b.PROINV_IGST_num != 0)
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

                    if (b.PROINV_CGST_num != null)
                        lblCGSTPer.Text = "(" + b.PROINV_CGST_num.ToString() + "%)";
                    else
                        lblCGSTPer.Text = "(" + "0" + "%)";

                    if (b.PROINV_SGST_num != null)
                        lblSGSTPer.Text = "(" + b.PROINV_SGST_num.ToString() + "%)";
                    else
                        lblSGSTPer.Text = "(" + "0" + "%)";

                    if (b.PROINV_IGST_num != null)
                        lblIGSTPer.Text = "(" + b.PROINV_IGST_num.ToString() + "%)";
                    else
                        lblIGSTPer.Text = "(" + "0" + "%)";

                }
            }

            CalculateProformaInvoice();
        }

        private bool CheckGSTFlag(DateTime BillDate)
        {
            bool gstFlag = false;
            //string[] strDate = txtDate.Text.Split('/');
            //DateTime ProformaInvoiceDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
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
    }
}