using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ReportMixDesignStrength : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Mix Design Report";
                
                bool superAdminRight = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_SuperAdmin_right_bit == true)
                        superAdminRight = true;
                }
                if (superAdminRight == true)
                {
                    //ddl_Grade.SelectedValue = "M 20";
                }
                else
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }
        }

        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            int count = 0;
            DataTable dt = new DataTable();
            DataRow dr1 = null;
            dt.Columns.Add(new DataColumn("28DaysStrength", typeof(string)));
            dt.Columns.Add(new DataColumn("7DaysStrength", typeof(string)));
            dt.Columns.Add(new DataColumn("3DaysStrength", typeof(string)));
            dt.Columns.Add(new DataColumn("Grade", typeof(string)));
            dt.Columns.Add(new DataColumn("Cement", typeof(string)));
            dt.Columns.Add(new DataColumn("FlyAsh", typeof(string)));
            dt.Columns.Add(new DataColumn("WCRatio", typeof(string)));
            dt.Columns.Add(new DataColumn("Admixture", typeof(string)));
            dt.Columns.Add(new DataColumn("AdmixturePercent", typeof(string)));
            dt.Columns.Add(new DataColumn("MonthOfCasting", typeof(string)));
            var mf = dc.MixDesign_ReportStrength(0);
            foreach (var mfinwd in mf)
            {

                string Strength3days = "", Strength7days = "", Strength28days = "", Cement = "", FlyAsh = "", WCRatio = "", Admixture = "", AdmixturePercent = "";
                var cubeCast = dc.OtherCubeTestView(mfinwd.MFINWD_ReferenceNo_var, "MF", 0, mfinwd.Trial_Id, "Trial", false, false);
                foreach (var c in cubeCast)
                {
                    if (c.Days_tint == 3)
                    {
                        Strength3days = c.Avg_var;
                    }
                    else if (c.Days_tint == 7)
                    {
                        Strength7days = c.Avg_var;
                    }
                    else if (c.Days_tint == 28)
                    {
                        Strength28days = c.Avg_var;
                    }
                }
                var data = dc.TrialDetail_View(mfinwd.MFINWD_ReferenceNo_var, mfinwd.Trial_Id);
                foreach (var t in data)
                {
                    if (t.TrialDetail_MaterialName == "Cement")
                    {
                        Cement = t.TrialDetail_Weight;
                    }
                    else if (t.TrialDetail_MaterialName == "Fly Ash")
                    {
                        FlyAsh = t.TrialDetail_Weight;
                    }
                    else if (t.TrialDetail_MaterialName == "W/C Ratio")
                    {
                        WCRatio = t.TrialDetail_Weight;
                    }
                    else if (t.TrialDetail_MaterialName == "Admixture")
                    {
                        Admixture = t.TrialDetail_Weight;
                    }
                }
               
                dr1 = dt.NewRow();
                dr1["28DaysStrength"] = Strength28days;
                dr1["7DaysStrength"] = Strength7days;
                dr1["3DaysStrength"] = Strength3days;
                dr1["Grade"] = mfinwd.MFINWD_Grade_var.Replace("M ", "");
                dr1["Cement"] = Cement;
                dr1["FlyAsh"] = FlyAsh;
                dr1["WCRatio"] = WCRatio;
                dr1["Admixture"] = Admixture;
                decimal temp = 0;
                if (Admixture != "")
                {
                    if (Cement != "")
                        temp += Convert.ToDecimal(Cement);
                    if (FlyAsh != "")
                        temp += Convert.ToDecimal(FlyAsh);

                    temp = ((temp / 50) * Convert.ToDecimal(Admixture)) / 1000;
                    AdmixturePercent = temp.ToString("0.00");
                }
                dr1["AdmixturePercent"] = AdmixturePercent;
                int monthOfCasting = Convert.ToInt32(Convert.ToDateTime(mfinwd.Trial_Date).ToString("MM"));
                if (monthOfCasting == 1)
                    monthOfCasting = 20;
                else if (monthOfCasting == 2)
                    monthOfCasting = 22;
                else if (monthOfCasting == 3)
                    monthOfCasting = 21;
                else if (monthOfCasting == 4)
                    monthOfCasting = 28;
                else if (monthOfCasting == 5)
                    monthOfCasting = 30;
                else if (monthOfCasting == 6)
                    monthOfCasting = 27;
                else if (monthOfCasting == 7)
                    monthOfCasting = 25;
                else if (monthOfCasting == 8)
                    monthOfCasting = 25;
                else if (monthOfCasting == 9)
                    monthOfCasting = 25;
                else if (monthOfCasting == 10)
                    monthOfCasting = 25;
                else if (monthOfCasting == 11)
                    monthOfCasting = 22;
                else if (monthOfCasting == 12)
                    monthOfCasting = 20;

                dr1["MonthOfCasting"] = monthOfCasting;
                
                dt.Rows.Add(dr1);
                count++;
            }
            grdMF.DataSource = dt;
            grdMF.DataBind();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdMF.Rows.Count > 0 && grdMF.Visible == true)
            {
                string Subheader = "";

                //Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text + "|" + lblToDate.Text + "|" + txtToDate.Text;
                Subheader += "|" + "" + "|" + "";

                PrintGrid.PrintGridView(grdMF, Subheader, "MixDesignReport");
            }
        }

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

    }
}