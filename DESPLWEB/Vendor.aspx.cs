using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class Vendor : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Vendor Master";
                
                //bool userRight = false;
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                //else
                //{
                //    var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                //    foreach (var u in user)
                //    {
                //        if (u.USER_Account_right_bit == true)
                //        {
                //            userRight = true;
                //        }
                //    }
                //}
                //userRight = true;
                //if (userRight == false)
                //{
                //    pnlContent.Visible = false;
                //    lblAccess.Visible = true;
                //    lblAccess.Text = "Access is Denied.. ";
                //}
                optAddNew.Checked = true;
                LoadCountryList();
                LoadStateList();
            }
        }

        private void LoadCountryList()
        {
            var country = dc.Country_View();
            ddlCountry.DataSource = country;
            ddlCountry.DataTextField = "COUNTRY_Name_var";
            ddlCountry.DataValueField = "COUNTRY_Id";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        private void LoadStateList()
        {
            var stateGrp = dc.State_View();
            ddlState.DataSource = stateGrp;
            ddlState.DataTextField = "State_Name_var";
            ddlState.DataValueField = "State_Id";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        private void BindCity()
        {
            var reg = dc.City_View(Convert.ToInt32(ddlState.SelectedValue));
            ddlCity.DataSource = reg;
            ddlCity.DataTextField = "City_Name_var";
            ddlCity.DataValueField = "City_Name_var";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        protected void txtVendor_TextChanged(object sender, EventArgs e)
        {            
            if (ChkVendorName(txtVendor.Text) == true)
            {
                ClearControls();
                if (txtVendor.Text != "")
                {
                    lblVendorId.Text = Request.Form[hfVendorId.UniqueID];
                }
                else
                {
                    lblVendorId.Text = "0";
                }
            }
        }

        protected Boolean ChkVendorName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txtVendor.Text;
            Boolean valid = false;
            var query = dc.Vendor_View(0, searchHead, "");
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txtVendor.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Vendor is not in the list";
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetVendorName(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Vendor_View(0, searchHead, "");
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("VEND_FirmName_var");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.VEND_FirmName_var, rowObj.VEND_Id.ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                var clnm = db.Vendor_View(0, "", "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.VEND_FirmName_var, rowObj.VEND_Id.ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> VEND_FirmName_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                VEND_FirmName_var.Add(dt.Rows[i][0].ToString());
            }
            return VEND_FirmName_var;

        }

        protected void lnkSaveCity_Click(object sender, EventArgs e)
        {
            if (ddlState.SelectedIndex != -1 && ddlState.SelectedIndex != 0)
            {
                if (txtCity.Text != "")
                {
                    ListItem item = ddlCity.Items.FindByText(txtCity.Text);
                    if (item == null && txtCity.Text != "")
                    {
                        ddlCity.Items.Add(txtCity.Text);
                    }

                    if (ddlCity.Items.FindByText(txtCity.Text) != null)
                    {
                        ddlCity.ClearSelection();
                        ddlCity.Items.FindByText(txtCity.Text).Selected = true;
                    }

                    ddlCity.Visible = true;
                    txtCity.Visible = false; txtCity.Text = "";
                    lnkAddCity.Text = "New City";
                    lnkSaveCity.Visible = false;
                }
            }
            else
            {
                lblVendorMessage.ForeColor = System.Drawing.Color.Red;
                lblVendorMessage.Visible = true;
                lblVendorMessage.Text = "Please Select State";
            }
        }

        protected void lnkAddCity_Click(object sender, EventArgs e)
        {
            if (lnkAddCity.Text == "New City")
            {
                ddlCity.Visible = false;
                lnkAddCity.Text = "Old City"; txtCity.Text = "";
                txtCity.Visible = true; lnkSaveCity.Visible = true;
            }
            else
            {
                ddlCity.Visible = true;
                lnkAddCity.Text = "New City";
                txtCity.Visible = false; lnkSaveCity.Visible = false;
            }
        }
        
        private void ClearAllControls()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");            
            lblMsg.Visible = false;
            lblVendorId.Text = "0";
            txtVendor.Text = "";
            txtVendor.Visible = false;
            btnDisplay.Visible = false;
            lblVendor.Visible = false;

            ClearControls(); 
        }

        private void ClearControls()
        {
            txtFirmName.Text = "";
            txtAddress.Text = "";
            txtContactNo.Text = "";
            txtEmailId.Text = "";
            ddlCountry.SelectedValue = "0";
            ddlState.SelectedValue = "0";
            ddlCity.Items.Clear();
            txtCity.Text = "";
            txtPincode.Text = "";
            txtPanNo.Text = "";
            ddlGSTRegistrationType.SelectedIndex = 0;
            txtGstNo.Text = "";
            txtOwnerName.Text = "";
            txtBankName.Text = "";
            txtAccountNo.Text = "";
            txtIFSC.Text = "";
            txtBranch.Text = "";
            lblVendorMessage.Visible = false;
            lnkSave.Enabled = true;

            valName1.Visible = false;
            txtGstNo.Enabled = false;
            ddlCity.Visible = true;
            txtCity.Visible = false;
            txtCity.Text = "";
            lnkAddCity.Text = "New City";
            lnkSaveCity.Visible = false;
        }

        protected void optAddNew_CheckedChanged(object sender, EventArgs e)
        {
            ClearAllControls();
        }

        protected void optEditExisting_CheckedChanged(object sender, EventArgs e)
        {
            ClearAllControls();
            txtVendor.Visible = true;
            btnDisplay.Visible = true;
            lblVendor.Visible = true;
        }

        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            lblVendorMessage.Visible = false;
            if (optEditExisting.Checked == true)
            {
                if (lblVendorId.Text == "" || lblVendorId.Text == "0")
                {
                    lblVendorMessage.ForeColor = System.Drawing.Color.Red;
                    lblVendorMessage.Visible = true;
                    lblVendorMessage.Text = "Select vendor from list.";
                }
                else
                {
                    var vendor = dc.Vendor_View(Convert.ToInt32(lblVendorId.Text), "", "");
                    foreach (var vend in vendor)
                    {
                        txtFirmName.Text = vend.VEND_FirmName_var;
                        txtAddress.Text = vend.VEND_Address_var;
                        txtContactNo.Text = vend.VEND_ContactNo_var;
                        txtEmailId.Text = vend.VEND_EmailId_var;
                        txtPincode.Text = vend.VEND_Pincode_var;
                        txtPanNo.Text = vend.VEND_PanNo_var;
                        ddlGSTRegistrationType.SelectedValue = vend.VEND_GSTRegistrationType_var;
                        if (ddlGSTRegistrationType.SelectedValue != "Un Registered")                        
                            txtGstNo.Enabled = true;                        
                        else                        
                            txtGstNo.Enabled = false;                        
                        txtGstNo.Text = vend.VEND_GSTNo_var;
                        txtOwnerName.Text = vend.VEND_OwnerName_var;
                        txtBankName.Text = vend.VEND_BankName_var;
                        txtAccountNo.Text = vend.VEND_AccountNo_var;
                        txtIFSC.Text = vend.VEND_IFSC_var;
                        txtBranch.Text = vend.VEND_Branch_var;

                        ddlCountry.ClearSelection();
                        ddlState.ClearSelection();
                        ddlCity.ClearSelection();

                        if (ddlCountry.Items.FindByText(vend.VEND_Country_var) != null)
                            ddlCountry.Items.FindByText(vend.VEND_Country_var).Selected = true;
                        else if (vend.VEND_Country_var != null && vend.VEND_Country_var != "")
                            ddlCountry.SelectedItem.Text = vend.VEND_Country_var;

                        if (ddlState.Items.FindByText(vend.VEND_State_var) != null)
                            ddlState.Items.FindByText(vend.VEND_State_var).Selected = true;
                        else if (vend.VEND_State_var != null && vend.VEND_State_var != "")
                            ddlState.SelectedItem.Text = vend.VEND_State_var;

                        if (ddlState.SelectedValue != "")
                            BindCity();

                        if (ddlCity.Items.FindByText(vend.VEND_City_var) != null)
                            ddlCity.Items.FindByText(vend.VEND_City_var).Selected = true;
                        else if (vend.VEND_City_var != null && vend.VEND_City_var != "")
                            ddlCity.SelectedItem.Text = vend.VEND_City_var;

                        break;
                    }
                }
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                lblVendorMessage.Visible = false;
                bool flag = true;
                txtFirmName.Text = txtFirmName.Text.Replace("\t", "");
                txtGstNo.Text = txtGstNo.Text.Replace("\t", "");
                valName1.Visible = false;
                //if (txtPincode.Text != "" && txtPincode.Text.Length < 6)
                //{
                //    flag = false;
                //    lblVendorMessage.ForeColor = System.Drawing.Color.Red;
                //    lblVendorMessage.Visible = true;
                //    lblVendorMessage.Text = "Invalid PinCode";
                //}
                //else if (ddlCity.SelectedItem.Text == "--Select--" || ddlCity.SelectedItem.Text == "")
                //{
                //    flag = false;
                //    lblVendorMessage.ForeColor = System.Drawing.Color.Red;
                //    lblVendorMessage.Visible = true;
                //    lblVendorMessage.Text = "Please Select City";
                //}
                //else if (txtCity.Visible == true)
                //{
                //    flag = false;
                //    lblVendorMessage.ForeColor = System.Drawing.Color.Red;
                //    lblVendorMessage.Visible = true;
                //    lblVendorMessage.Text = "Please Select City";
                //}
                //else if (ddlGSTRegistrationType.SelectedValue != "Un Registered")
                //{
                //    if (txtGstNo.Text == "")
                //    {
                //        valName1.Visible = true;
                //        flag = false;
                //    }
                //    else
                //    {
                //        if (txtGstNo.Text.Length < 15)
                //        {
                //            lblVendorMessage.Text = "Invalid GST No.";
                //            lblVendorMessage.ForeColor = System.Drawing.Color.Red;
                //            lblVendorMessage.Visible = true;
                //            flag = false;
                //        }
                //        else
                //        {
                //            var vendor = dc.Vendor_View(0, "", txtGstNo.Text).ToList();
                //            if (vendor.Count > 0 && (optAddNew.Checked == true ||  (optEditExisting.Checked == true && Convert.ToInt32(lblVendorId.Text) != vendor.FirstOrDefault().VEND_Id)))
                //            {
                //                lblVendorMessage.Text = "GST No already updated to vendor - " + vendor.FirstOrDefault().VEND_FirmName_var;
                //                lblVendorMessage.ForeColor = System.Drawing.Color.Red;
                //                lblVendorMessage.Visible = true;
                //                flag = false;
                //            }
                //        }
                //    }
                //}

                if (flag)
                {
                    byte vendorStatus = 0;
                    //check for duplicate firm name
                    var vendor = dc.Vendor_View(0, txtFirmName.Text, "");
                    foreach (var vend in vendor)
                    {
                        if (Convert.ToInt32(lblVendorId.Text) == 0)
                        {
                            vendorStatus = 1;
                            lblVendorMessage.Text = "Duplicate Firm Name..";
                            lblVendorMessage.Visible = true;
                            break;
                        }
                        else if (Convert.ToInt32(lblVendorId.Text) > 0)
                        {
                            if (vend.VEND_Id != Convert.ToInt32(lblVendorId.Text))
                            {
                                vendorStatus = 1;
                                lblVendorMessage.Text = "Duplicate Firm Name..";
                                lblVendorMessage.Visible = true;
                                break;
                            }
                        }
                    }
                    if (vendorStatus == 0)
                    {
                        string strCity=null ;

                        //if (ddlCity.Visible == true )
                        //    strCity = ddlCity.SelectedItem.Text;
                        //else
                        //    strCity = txtCity.Text;
                        int vendorId = 0;
                        if (lblVendorId.Text == "")
                        {
                            vendorId = dc.Vendor_Update(Convert.ToInt32("0"), txtFirmName.Text, txtAddress.Text, txtContactNo.Text, txtEmailId.Text, ddlCountry.SelectedItem.Text, ddlState.SelectedItem.Text, strCity, txtPincode.Text, txtPanNo.Text, ddlGSTRegistrationType.SelectedValue, txtGstNo.Text, txtOwnerName.Text, txtBankName.Text, txtAccountNo.Text, txtIFSC.Text, txtBranch.Text);
                        }
                        else
                        {
                            vendorId = dc.Vendor_Update(Convert.ToInt32(lblVendorId.Text), txtFirmName.Text, txtAddress.Text, txtContactNo.Text, txtEmailId.Text, ddlCountry.SelectedItem.Text, ddlState.SelectedItem.Text, strCity, txtPincode.Text, txtPanNo.Text, ddlGSTRegistrationType.SelectedValue, txtGstNo.Text, txtOwnerName.Text, txtBankName.Text, txtAccountNo.Text, txtIFSC.Text, txtBranch.Text);
                        }
                        var cityDetails = dc.City_Update(strCity, Convert.ToInt32(ddlState.SelectedValue), 0).ToList();
                        if (cityDetails.Count() == 0)
                            dc.City_Update(strCity, Convert.ToInt32(ddlState.SelectedValue), 1);

                        lblVendorMessage.Text = "Updated Successfully";
                        lblVendorMessage.ForeColor = System.Drawing.Color.Green;
                        lblVendorMessage.Visible = true;
                        lnkSave.Enabled = false;
                    }
                }
            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlState.SelectedIndex > 0)
            {
                BindCity();
                lblVendorMessage.Visible = false;
            }
        }

        protected void lnkClear_Click(object sender, EventArgs e)
        {
            ClearAllControls();
            optAddNew.Checked = true;
            optEditExisting.Checked = false; 
        }

        protected void ddlGSTRegistrationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGSTRegistrationType.SelectedValue != "Un Registered")
            {
                txtGstNo.Enabled = true; 
            }
            else
            {
                txtGstNo.Enabled = false;
            }
        }
    }
}