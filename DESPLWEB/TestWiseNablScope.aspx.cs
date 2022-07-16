using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class TestWiseNablScope : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        //DataTable dt;
        clsData db = new clsData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Testwise NABL Status";
                LoadRecordTypeList();
                ViewState["TestDetails"] = null;
                bool userRight = false;
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

            }
        }

        protected void AddTestDetail()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["TestDetails"] != null)
            {
                dt.Columns.Add(new DataColumn("Sr.No", typeof(string)));
                dt.Columns.Add(new DataColumn("Record Type", typeof(string)));
                dt.Columns.Add(new DataColumn("Test Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Criteria", typeof(string)));
                dt.Columns.Add(new DataColumn("Current Rate", typeof(string)));
                dt.Columns.Add(new DataColumn("New Rate", typeof(string)));

            }

            dr = dt.NewRow();
            dr["Sr.No"] = dt.Rows.Count + 1;
            dr["Record Type"] = string.Empty;
            dr["Test Name"] = string.Empty;
            dr["Criteria"] = string.Empty;
            dr["Current Rate"] = string.Empty;
            dr["New Rate"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["TestDetails"] = dt;

        }

        private void LoadRecordTypeList()
        {
            var inwd = dc.Material_View("", "");
            ddlRecordType.DataSource = inwd;
            ddlRecordType.DataTextField = "MATERIAL_Name_var";
            ddlRecordType.DataValueField = "MATERIAL_RecordType_var";
            ddlRecordType.DataBind();
            ddlRecordType.Items.Insert(0, new ListItem("---Select All---", "---Select All---"));

        }
     
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            grdTest.DataSource = null;
            grdTest.DataBind();

            DisplayTest();
        }
        public void DisplayTest()
        {
            DataTable dt = new DataTable();
            DataRow drow = null;
            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Test_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Test_RecType_var", typeof(string)));
            dt.Columns.Add(new DataColumn("TEST_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("TEST_NablScope_var", typeof(string)));
            dt.Columns.Add(new DataColumn("TEST_NablLocation_int", typeof(int)));

            int id = 0;
            if(ddlRecordType.SelectedValue!="---Select All---")
                 id = db.getMaterialTypeId(ddlRecordType.SelectedValue);
            int i = 0;
            var rslt = dc.Test_View_ForNablStatus(id, ddlRecordType.SelectedValue);
            foreach (var rt in rslt)
            {
                drow = dt.NewRow();
                drow["SrNo"] = i.ToString();
                drow["Test_Id"] = rt.TEST_Id;
                drow["Test_RecType_var"] = rt.Test_RecType_var;
                drow["TEST_Name_var"] = rt.TEST_Name_var;
                if(Convert.ToString(rt.TEST_NablScope_var)=="" || Convert.ToString(rt.TEST_NablScope_var) == null)
                    drow["TEST_NablScope_var"] = "NA";
                else
                     drow["TEST_NablScope_var"] = rt.TEST_NablScope_var;
                drow["TEST_NablLocation_int"] = rt.TEST_NablLocation_int;
                dt.Rows.Add(drow);
                i++;
            }
            grdTest.DataSource = dt;
            grdTest.DataBind();


            for (int j = 0; j < grdTest.Rows.Count; j++)
            {
                DropDownList ddlNABLScope = (DropDownList)grdTest.Rows[j].FindControl("ddlNABLScope");
                Label lblNABLScope = (Label)grdTest.Rows[j].FindControl("lblNABLScope");
                DropDownList ddlNABLLocation = (DropDownList)grdTest.Rows[j].FindControl("ddlNABLLocation");
                Label lblNABLLocation = (Label)grdTest.Rows[j].FindControl("lblNABLLocation");
                if (lblNABLScope.Text != "")
                {
                    ddlNABLScope.SelectedValue = lblNABLScope.Text;
                }
                if (lblNABLLocation.Text != "" && Convert.ToInt32(lblNABLLocation.Text) > 0)
                {
                    ddlNABLLocation.SelectedValue = lblNABLLocation.Text;
                }
            }

        }
        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            //bool flag = false;
            if (grdTest.Rows.Count > 0)
            {

               
                for (int i = 0; i < grdTest.Rows.Count; i++)
                {
                    Label lblTestId = (Label)grdTest.Rows[i].Cells[0].FindControl("lblTestId");
                    DropDownList ddlNABLScope = (DropDownList)grdTest.Rows[i].Cells[4].FindControl("ddlNABLScope");
                    DropDownList ddlNABLLocation = (DropDownList)grdTest.Rows[i].Cells[5].FindControl("ddlNABLLocation");

                    if (ddlNABLLocation.SelectedIndex != 0 && ddlNABLScope.SelectedIndex != 0)
                    {
                        dc.TestWiseNablStatus_Update(Convert.ToInt32(lblTestId.Text), ddlNABLScope.SelectedValue.ToString(), Convert.ToInt32(ddlNABLLocation.SelectedValue));//insert
                    }
                }


                DisplayTest();
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Visible = true;
                lblMsg.Text = "Successfully Updated!!";

            }
        }
        protected void lnkClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            grdTest.DataSource = null;
            grdTest.DataBind();
            ddlRecordType.SelectedIndex = -1;

        }

        protected void grdTest_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string lblNABLScope = (e.Row.FindControl("lblNABLScope") as Label).Text;
                DropDownList ddlNABLScope = (e.Row.FindControl("ddlNABLScope") as DropDownList);
                ddlNABLScope.Items.FindByValue(lblNABLScope).Selected = true;

                string lblNABLLocation = (e.Row.FindControl("lblNABLLocation") as Label).Text;
                DropDownList ddlNABLLocation = (e.Row.FindControl("ddlNABLLocation") as DropDownList);
                ddlNABLLocation.Items.FindByValue(lblNABLLocation).Selected = true;
            }
        }
    }
}
