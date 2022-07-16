using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Test : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Add New Other Test";
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    bool userRight = false;
                    var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                    foreach (var u in user)
                    {
                        //if (u.USER_Admin_right_bit == true)
                        //if (u.USER_RptApproval_right_bit == true)
                        //if (u.USER_Name_var.ToLower().Contains("vishwas") == true
                        if (u.USER_Name_var.ToLower().Contains("irsha") == true)
                            userRight = true;
                    }
                    if (userRight == false)
                    {
                        pnlContent.Visible = false;
                        lblAccess.Visible = true;
                        lblAccess.Text = "Access is Denied.. ";
                    }
                    else
                    {
                        LoadOtherTestList();
                    }
                }
            }
        }

        protected void LoadOtherTestList()
        {
            ddlTest.Items.Clear();
            int MaterialId = 0;
            var InwardId = dc.Material_View("OT", "");
            foreach (var n in InwardId)
            {
                MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
            }
            var a = dc.Test_View(MaterialId, 0, "OTHER", 0, 0, 0);
            ddlTest.DataSource = a;
            ddlTest.DataTextField = "TEST_Name_var";
            ddlTest.DataValueField = "TEST_Id";
            ddlTest.DataBind();
            ddlTest.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        protected void lnkAddTest_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (Page.IsValid)
            {
                txtTestName.Text = txtTestName.Text.Trim();
                if (txtTestName.Text == "")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Enter Test Name";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    txtTestName.Focus();
                }
                else
                {
                    int MaterialId = 0;
                    var InwardId = dc.Material_View("OT", "");
                    foreach (var n in InwardId)
                    {
                        MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
                    }

                    var test = dc.Test(0, "", 0, "OTHER", txtTestName.Text, 0);
                    if (test.Count() == 0)
                    {
                        dc.Test_Update(0, txtTestName.Text.Trim(), MaterialId, 0, "OTHER", 0, 0);
                        LoadOtherTestList();
                        ddlTest.Items.FindByText(txtTestName.Text).Selected = true;
                        lblMessage.Visible = true;
                        lblMessage.Text = "Test added successfully.";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Test already available.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        protected void lnkAddSubTest_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (Page.IsValid)
            {
                txtSubTestName.Text = txtSubTestName.Text.Trim();
                if (ddlTest.SelectedValue == "0")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Select Test from list";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    ddlTest.Focus();
                }
                else if (txtSubTestName.Text == "")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Enter Sub Test Name";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    txtSubTestName.Focus();
                }
                else if (txtSubTestRate.Text == "")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Enter Sub Test Rate";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    txtSubTestRate.Focus();
                }
                else
                {
                    int MaterialId = 0;
                    var InwardId = dc.Material_View("OT", "");
                    foreach (var n in InwardId)
                    {
                        MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
                    }
                    var test = dc.Test(0, "", 0, "OTHER", txtSubTestName.Text, Convert.ToInt32(ddlTest.SelectedValue));
                    if (test.Count() == 0)
                    {
                        dc.Test_Update(0, txtSubTestName.Text.Trim(), MaterialId, Convert.ToInt32(txtSubTestRate.Text), "OTHER", 0, Convert.ToInt32(ddlTest.SelectedValue));
                        lblMessage.Visible = true;
                        lblMessage.Text = "Sub Test added successfully.";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Test already available.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        protected void lnkExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

    }
}