using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class GST : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool userRight = false;
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "GST Setting";
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
                        if (u.USER_SuperAdmin_right_bit == true)
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
                    txt_Fromdate.Text = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");
                    CalendarExtender2.SelectedDate = DateTime.Today.AddDays(1);
                    loadGstDetails();
                }
            }

        }

        private void loadGstDetails()
        {
            var details = dc.GST_View(0, null);
            gvDetailsView.DataSource = details;
            gvDetailsView.DataBind();
        }


        protected void LnkBtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateDate())
            {
                lblErrMsg.Visible = false;
                DateTime? frmDate = null, toDate = null;
                frmDate = DateTime.ParseExact(txt_Fromdate.Text, "dd/MM/yyyy", null);
                var chkGstDate = dc.GST_View(2, frmDate).ToList();
                if (chkGstDate.Count() == 0)
                {
                    var details = dc.GST_View(0, null).ToList();
                    if (details.Count() > 0)
                    {
                        foreach (var dt in details)
                        {
                            toDate = DateTime.Parse(frmDate.ToString()).AddDays(-1);
                            dc.GST_Update(1, dt.GST_Id, null, toDate, 0, 0, 0);
                            dc.GST_Update(0, 0, frmDate, null, Convert.ToDecimal(txtSGST.Text), Convert.ToDecimal(txtCGST.Text), Convert.ToDecimal(txtIGST.Text));
                            break;

                        }
                    }
                    else
                    {
                        dc.GST_Update(0, 0, frmDate, toDate, Convert.ToDecimal(txtSGST.Text), Convert.ToDecimal(txtCGST.Text), Convert.ToDecimal(txtIGST.Text));
                    }
                    lblresult.Text = "Record Updated Successfully";
                    loadGstDetails();
                    Clear();
                }
                else
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Record already exist for this Date.";
                }


            }
        }

        private void Clear()
        {
            txtCGST.Text = ""; txtSGST.Text = ""; txtIGST.Text = "";
        }

        private bool ValidateDate()
        {
            bool flag = false;
            if (txt_Fromdate.Text != "")
            {
                DateTime validateDate = DateTime.Today.AddDays(1);
                DateTime compareDate = DateTime.ParseExact(txt_Fromdate.Text, "dd/MM/yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                if (compareDate < validateDate)
                {
                    lblErrMsg.Visible = true;
                    flag = false;
                }
                else
                {
                    lblErrMsg.Visible = false;
                    flag = true;
                }
            }
            return flag;
        }

        protected void txt_Fromdate_txtChanged(object sender, EventArgs e)
        {
            lblErrMsg.Visible = false;
            lblresult.Text = "";
        }
    }
}