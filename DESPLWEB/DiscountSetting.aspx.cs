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

using System.IO;

namespace DESPLWEB
{
    public partial class DiscountSetting : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        clsData cd = new clsData();
        public static int Cl_ID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Discount Setting";
                ddl_Site.Items.Insert(0, "---Select---");
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

        public void LoadMaterialtype()
        {
            var material = dc.Material_View("", "");
            grdDiscount.DataSource = material;
            grdDiscount.DataBind();
        }
        public void Cleartxt()
        {
            for (int j = 0; j < grdDiscount.Rows.Count; j++)
            {
                TextBox txt_Discount = (TextBox)grdDiscount.Rows[j].Cells[2].FindControl("txt_Discount");
                txt_Discount.Text = "";
            }
        }
        public void ViewMatDiscount()
        {
            Cleartxt();
            int SiteId = 0;
            //if (!chkAllSite.Checked)//ddl_Site.SelectedItem.Text != "All"
            //{
                SiteId = Convert.ToInt32(ddl_Site.SelectedValue);
            //}
            //else
            //    SiteId = Convert.ToInt32(ddl_Site.SelectedValue.ToString());

            var chk = dc.DiscountSetting_Update(Cl_ID, SiteId, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, "", true, false, false);
            foreach (var c in chk)
            {
                for (int m = 0; m < grdDiscount.Rows.Count; m++)
                {
                    TextBox txt_Discount = (TextBox)grdDiscount.Rows[m].Cells[2].FindControl("txt_Discount");
                    if (c.DISCOUNT_AAC_tint != null && c.DISCOUNT_AAC_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "AAC")//0
                        {
                            txt_Discount.Text = c.DISCOUNT_AAC_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_AGGT_tint != null && c.DISCOUNT_AGGT_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "AGGT")//1
                        {
                            txt_Discount.Text = c.DISCOUNT_AGGT_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_BT_tint != null && c.DISCOUNT_BT_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "BT-")//2
                        {
                            txt_Discount.Text = c.DISCOUNT_BT_tint.ToString();

                        }

                    }
                    if (c.DISCOUNT_CCH_tint != null && c.DISCOUNT_CCH_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "CCH")//3
                        {
                            txt_Discount.Text = c.DISCOUNT_CCH_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_CEMT_tint != null && c.DISCOUNT_CEMT_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "CEMT")//4
                        {
                            txt_Discount.Text = c.DISCOUNT_CEMT_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_CORECUT_tint != null && c.DISCOUNT_CORECUT_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "CORECUT")//5
                        {
                            txt_Discount.Text = c.DISCOUNT_CORECUT_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_CR_tint != null && c.DISCOUNT_CR_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "CR")//6
                        {
                            txt_Discount.Text = c.DISCOUNT_CR_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_CT_tint != null && c.DISCOUNT_CT_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "CT")//7
                        {
                            txt_Discount.Text = c.DISCOUNT_CT_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_FLYASH_tint != null && c.DISCOUNT_FLYASH_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "FLYASH")//8
                        {
                            txt_Discount.Text = c.DISCOUNT_FLYASH_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_GT_tint != null && c.DISCOUNT_GT_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "GT")//9
                        {
                            txt_Discount.Text = c.DISCOUNT_GT_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_MF_tint != null && c.DISCOUNT_MF_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "MF")
                        {
                            txt_Discount.Text = c.DISCOUNT_MF_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_NDT_tint != null && c.DISCOUNT_NDT_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "NDT")//10
                        {
                            txt_Discount.Text = c.DISCOUNT_NDT_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_OT_tint != null && c.DISCOUNT_OT_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "OT")//11
                        {
                            txt_Discount.Text = c.DISCOUNT_OT_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_PILE_tint != null && c.DISCOUNT_PILE_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "PILE")//12
                        {
                            txt_Discount.Text = c.DISCOUNT_PILE_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_PT_tint != null && c.DISCOUNT_PT_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "PT")//13
                        {
                            txt_Discount.Text = c.DISCOUNT_PT_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_RWH_tint != null && c.DISCOUNT_RWH_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "RWH")//14
                        {
                            txt_Discount.Text = c.DISCOUNT_RWH_tint.ToString();

                        }

                    }
                    if (c.DISCOUNT_SO_tint != null && c.DISCOUNT_SO_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "SO")//15
                        {
                            txt_Discount.Text = c.DISCOUNT_SO_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_SOLID_tint != null && c.DISCOUNT_SOLID_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "SOLID")//16
                        {
                            txt_Discount.Text = c.DISCOUNT_SOLID_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_ST_tint != null && c.DISCOUNT_ST_tint != 0)
                    {

                        if (grdDiscount.Rows[m].Cells[0].Text == "ST")//17
                        {
                            txt_Discount.Text = c.DISCOUNT_ST_tint.ToString();
                        }

                    }
                    if (c.DISCOUNT_STC_tint != null && c.DISCOUNT_STC_tint != 0)
                    {


                        if (grdDiscount.Rows[m].Cells[0].Text == "STC")
                        {
                            txt_Discount.Text = c.DISCOUNT_STC_tint.ToString();//18
                        }


                    }
                    if (c.DISCOUNT_TILE_tint != null && c.DISCOUNT_TILE_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "TILE")
                        {
                            txt_Discount.Text = c.DISCOUNT_TILE_tint.ToString();//19
                        }

                    }
                    if (c.DISCOUNT_WT_tint != null && c.DISCOUNT_WT_tint != 0)
                    {
                        if (grdDiscount.Rows[m].Cells[0].Text == "WT")
                        {
                            txt_Discount.Text = c.DISCOUNT_WT_tint.ToString();//20
                        }
                    }
                }
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            int SiteId = 0,discInt=0; bool flag = false;
            string RecordType = "";
            if (!chkAllSite.Checked)//ddl_Site.SelectedItem.Text != "All"
            {
                SiteId = Convert.ToInt32(ddl_Site.SelectedValue);
            }
            else
                discInt = 1;

          

            byte AacAmt = 0;
            byte AggtAmt = 0;
            byte BtAmt = 0;
            byte CchAmt = 0;
            byte CemtAmt = 0;
            byte CoreCutAmt = 0;
            byte CrAmt = 0;
            byte CtAmt = 0;
            byte FlyashAmt = 0;
            byte GtAmt = 0;
            byte MfAmt = 0;
            byte NdtAmt = 0;
            byte PileAmt = 0;
            byte PTAmt = 0;
            byte SoAmt = 0;
            byte SolidAmt = 0;
            byte StAmt = 0;
            byte StcAmt = 0;
            byte TileAmt = 0;
            byte WtAmt = 0;
            byte OtAmt = 0;
            byte RwhAmt = 0;
            for (int i = 0; i < grdDiscount.Rows.Count; i++)
            {
                TextBox txt_Discount = (TextBox)grdDiscount.Rows[i].Cells[2].FindControl("txt_Discount");
                //if (txt_Discount.Text != "")
                //{
                if (grdDiscount.Rows[i].Cells[0].Text != "")
                {
                    RecordType = grdDiscount.Rows[i].Cells[0].Text;

                }
                if (RecordType == "AAC")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            AacAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "AGGT")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            AggtAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "BT-")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            BtAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "CCH")
                {
                    if (txt_Discount.Text != "")
                    {
                        CchAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "CEMT")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            CemtAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "CORECUT")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            CoreCutAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "CR")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            CrAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "CT")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            CtAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "FLYASH")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            FlyashAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "GT")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            GtAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "MF")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            MfAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "NDT")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            NdtAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "PILE")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            PileAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "PT")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            PTAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "SO")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            SoAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "SOLID")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            SolidAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "ST")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            StAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "STC")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            StcAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "TILE")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            TileAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "WT")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            WtAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "OT")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            OtAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }
                if (RecordType == "RWH")
                {
                    if (txt_Discount.Text != "")
                    {
                        if (Convert.ToDouble(txt_Discount.Text) > 0)
                            RwhAmt = Convert.ToByte(txt_Discount.Text);
                    }
                }

           
            }

            if (AacAmt == 0 && AggtAmt == 0 && BtAmt == 0 && CchAmt == 0 && CemtAmt == 0 &&
                           CoreCutAmt == 0 && CrAmt == 0 && CtAmt == 0 && FlyashAmt == 0 && GtAmt == 0 && MfAmt == 0 && NdtAmt == 0 && PileAmt == 0 && PTAmt == 0 && SoAmt == 0 && SolidAmt == 0 && StAmt == 0 && StcAmt == 0 && TileAmt == 0 &&
                              WtAmt == 0 && OtAmt == 0 && RwhAmt == 0)
            {
               flag = true;
            }

            bool valid = false;
            
            if (SiteId > 0)
            {
                if (flag)
                {
                      dc.DiscountSetting_Update(Cl_ID, SiteId, AacAmt, AggtAmt, BtAmt, CchAmt, CemtAmt,
                          CoreCutAmt, CrAmt, CtAmt, FlyashAmt, GtAmt, MfAmt, NdtAmt, PileAmt, PTAmt, SoAmt, SolidAmt, StAmt, StcAmt, TileAmt,
                          WtAmt, OtAmt, RwhAmt, RecordType, false, false, true);

                }
                else
                {
                    dc.DiscountSetting_Update(Cl_ID, 0, AacAmt, AggtAmt, BtAmt, CchAmt, CemtAmt,
                           CoreCutAmt, CrAmt, CtAmt, FlyashAmt, GtAmt, MfAmt, NdtAmt, PileAmt, PTAmt, SoAmt, SolidAmt, StAmt, StcAmt, TileAmt,
                           WtAmt, OtAmt, RwhAmt, RecordType, false, false, true);
                    var chk = dc.DiscountSetting_Update(Cl_ID, SiteId, 0, 0, 0,
                                             0, 0, 0, 0, 0, 0, 0, 0,
                                             0, 0, 0, 0, 0, 0, 0, 0, 0,
                                             0, 0, "", true, false, false);
                    foreach (var c in chk)
                    {
                        if (c.DISCOUNT_SiteId_int == SiteId && c.DISCOUNT_SiteId_int > 0)
                        {

                            dc.DiscountSetting_Update(Cl_ID, SiteId, AacAmt, AggtAmt, BtAmt, CchAmt, CemtAmt,
                                CoreCutAmt, CrAmt, CtAmt, FlyashAmt, GtAmt, MfAmt, NdtAmt, PileAmt, PTAmt, SoAmt, SolidAmt, StAmt, StcAmt, TileAmt,
                                WtAmt, OtAmt, RwhAmt, RecordType, false, true, false);
                            valid = true;
                        }
                    }
                    if (valid == false)
                    {


                        dc.DiscountSetting_Update(Cl_ID, SiteId, AacAmt, AggtAmt, BtAmt, CchAmt, CemtAmt,
                               CoreCutAmt, CrAmt, CtAmt, FlyashAmt, GtAmt, MfAmt, NdtAmt, PileAmt, PTAmt, SoAmt, SolidAmt, StAmt, StcAmt, TileAmt,
                                  WtAmt, OtAmt, RwhAmt, RecordType, false, false, false);
                    }
                    ListItem itemToRemove = ddl_Site.Items.FindByValue("All");
                    ddl_Site.Items.Remove(itemToRemove);
                }
             
            }

            if (SiteId == 0)
            {
                dc.DiscountSetting_Update(Cl_ID, -1, AacAmt, AggtAmt, BtAmt, CchAmt, CemtAmt,
                              CoreCutAmt, CrAmt, CtAmt, FlyashAmt, GtAmt, MfAmt, NdtAmt, PileAmt, PTAmt, SoAmt, SolidAmt, StAmt, StcAmt, TileAmt,
                              WtAmt, OtAmt, RwhAmt, RecordType, false, false, true);


                //get all site of selected client and save discount for all site of that client
                if (!flag)
                {
                    var cl = dc.Site_View(0, Cl_ID, 0, "").ToList();
                    foreach (var item in cl)
                    {
                        dc.DiscountSetting_Update(Cl_ID, item.SITE_Id, AacAmt, AggtAmt, BtAmt, CchAmt, CemtAmt,
                     CoreCutAmt, CrAmt, CtAmt, FlyashAmt, GtAmt, MfAmt, NdtAmt, PileAmt, PTAmt, SoAmt, SolidAmt, StAmt, StcAmt, TileAmt,
                     WtAmt, OtAmt, RwhAmt, RecordType, false, false, false);

                    }
                }
                else
                {
                  //  dc.DiscountSetting_Update(Cl_ID, SiteId,AacAmt, AggtAmt, BtAmt, CchAmt, CemtAmt,
                  //CoreCutAmt, CrAmt, CtAmt, FlyashAmt, GtAmt, MfAmt, NdtAmt, PileAmt, PTAmt, SoAmt, SolidAmt, StAmt, StcAmt, TileAmt,
                  //WtAmt, OtAmt, RwhAmt, RecordType, false, false, false);
                }
             
            }

            cd.updateClientDiscount(Cl_ID,discInt);

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Text = "Record Saved Successfully";
            lblMsg.Visible = true;
            lblMsg.ForeColor = System.Drawing.Color.Green;
            // ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Record Saved Sucessfully');", true);
            lnkSave.Enabled = false;
        }

        protected void ImgBtbSearchClient_Click(object sender, EventArgs e)
        {
            ListClients.Items.Clear();
            ListClients.DataTextField = "CL_Name_var";
            ListClients.DataValueField = "CL_Id";
            var cl = dc.Client_View(0, 0, txt_ClentName.Text + "%", "");
            ListClients.DataSource = cl;
            ListClients.DataBind();
            ModalPopupExtender1.Show();

        }
        protected void ImgBtnClose_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();

        }
        protected void ImgBtnOk_Click(object sender, ImageClickEventArgs e)
        {
            if (ListClients.SelectedItem != null)
            {
                txt_ClientName.Text = ListClients.SelectedItem.Text;
                Cl_ID = Convert.ToInt32(ListClients.SelectedItem.Value);
                ModalPopupExtender1.Hide();
                txt_ClientName.Text = ListClients.SelectedItem.ToString();
                LoadSite();
                LoadMaterialtype();
                ViewMatDiscount();
            }

        }
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            txt_ClentName.Text = string.Empty;
            ModalPopupExtender1.Show();
            pnl1.Visible = true;
            LoadClientList();
            lnkSave.Enabled = true;
        }
        private void LoadSite()
        {
            var site = dc.Site_View(0, Cl_ID, 0, "");
            ddl_Site.DataTextField = "SITE_Name_var";
            ddl_Site.DataValueField = "SITE_Id";
            ddl_Site.DataSource = site;
            ddl_Site.DataBind();
            //ddl_Site.Items.Insert(0, "All");
            //var ck = dc.DiscountSetting_Update((Cl_ID), -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", true, false, false);

            var ck = dc.Client_View(Cl_ID, 0, "", "");
            foreach (var c in ck)
            {

                if (Convert.ToInt32(c.CL_DiscSetting_tint) == 1)
                {
                    //ListItem itemToRemove = ddl_Site.Items.FindByValue("All");
                    //ddl_Site.Items.Remove(itemToRemove);
                    chkAllSite.Checked = true;
                }
                else
                    chkAllSite.Checked = false;
            }
        }
        private void LoadClientList()
        {
            ModalPopupExtender1.Show();
            if (txt_ClientName.Text != "")
            {
                ListClients.Items.Clear();
                ListClients.DataTextField = "CL_Name_var";
                ListClients.DataValueField = "CL_Id";
                var client = dc.Client_View(0, 0, txt_ClientName.Text + "%", "");
                ListClients.DataSource = client;
                ListClients.DataBind();
            }
            else
            {
                ListClients.Items.Clear();
                ListClients.DataTextField = "CL_Name_var";
                ListClients.DataValueField = "CL_Id";
                var cl = dc.Client_View(0, 0, "", "");
                ListClients.DataSource = cl;
                ListClients.DataBind();
            }

        }

        protected void ddl_Site_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            LoadMaterialtype();
            ViewMatDiscount();
            lnkSave.Enabled = true;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }
    }
}
