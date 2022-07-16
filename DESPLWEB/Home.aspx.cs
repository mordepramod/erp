using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace DESPLWEB
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");                
                lblheading.Text = "Home";                
              //  Session["LoginId"] = 1;
            }
        }
        //protected void lnkAggregate_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 1;
        //    Response.Redirect("Aggregate_inward.aspx");
        //}
        //protected void lnkBricks_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 2;
        //    Response.Redirect("Brick_Inward.aspx");
        //}
        //protected void lnkCCH_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 3;
        //    Response.Redirect("CementChemical_Inward.aspx");
        //}
        //protected void lnkCement_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 4;
        //    Response.Redirect("Cement_inward.aspx");
        //}
        //protected void lnkCoreCuting_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 5;
        //    Response.Redirect("CoreCutting_Inward.aspx");
        //}
        //protected void lnkCore_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 6;
        //    Response.Redirect("Core_Inward.aspx");
        //}
        //protected void lnkCube_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 7;
        //    Response.Redirect("Cube_inward.aspx");
        //}
        //protected void lnkFlyash_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 8;
        //    Response.Redirect("Flyash_Inward.aspx");
        //}
        //protected void lnkSoilInvestigation_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 9;
        //    Response.Redirect("SoilInvestigation_Inward.aspx");
        //}
        //protected void lnkMixDesign_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 10;
        //    Response.Redirect("MixDesign_Inward.aspx");
        //}
        //protected void lnkNDT_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 11;
        //    Response.Redirect("NDT_inward.aspx");
        //}
        //protected void lnkPile_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 12;
        //    Response.Redirect("Pile_Inward.aspx");
        //}
        //protected void lnkPavmt_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 13;
        //    Response.Redirect("Pavement_Inward.aspx");
        //}
        //protected void lnkSoil_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 14;
        //    Response.Redirect("Soil_inward.aspx");
        //}
        //protected void lnkMasonry_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 15;
        //    Response.Redirect("Solid_Inward.aspx");
        //}
        //protected void lnkSteel_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 16;
        //    Response.Redirect("Steel_inward.aspx");
        //}
        //protected void lnkSTC_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 17;
        //    Response.Redirect("STC_Inward.aspx");
        //}
        //protected void lnkTile_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 18;
        //    Response.Redirect("Tile_inward.aspx");
        //}
        //protected void lnkWater_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 19;
        //    Response.Redirect("Water_Inward.aspx");
        //}
        //protected void lnkOther_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 20;
        //    Response.Redirect("Other_Inward.aspx");
        //}
        //protected void lnkRWH_Click(object sender, EventArgs e)
        //{
        //    Session["materialId"] = 21;
        //    Response.Redirect("RWH_Inward.aspx");
        //}
    }
}
