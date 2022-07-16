using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class InwardTestStartedStatus : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Inward Test Status";
                lblUserId.Text = Session["LoginId"].ToString();
                // getCurrentDate();
                LoadInwardType();
                bool testRight = false;
                var user = dc.User_View(Convert.ToInt32(lblUserId.Text), 0, "", "", "");
              
                foreach (var u in user)
                {
                    if (u.USER_Check_right_bit == true)
                        testRight = true;
                }
                if (testRight == true)
                {
                    string strReq = "";
                    strReq = Request.RawUrl;
                    strReq = strReq.Substring(strReq.IndexOf('?') + 1);
                    if (!strReq.Equals(""))
                    {
                        strReq = obj.Decrypt(strReq);
                        if (strReq.Contains("=") == true)
                        {
                            string[] arrMsgs = strReq.Split('&');
                            string[] arrIndMsg;

                            arrIndMsg = arrMsgs[0].Split('=');
                            ddl_InwardTestType.SelectedValue = arrIndMsg[1].ToString().Trim();

                            BindRecords();
                        }
                    }
                }
                else
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }

        }
        //public void getCurrentDate()
        //{
        //    txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
        //    txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        //}

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false; lblMsg.Text = "";
            BindRecords();
        }

        public void BindRecords()
        {
            grdInward.Visible = true;
            //DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            //DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
            //string gtBill = "";

            var Inward = dc.Inward_View_TestNotStarted(ddl_InwardTestType.SelectedItem.Text,  1);
            grdInward.DataSource = Inward;
            grdInward.DataBind();


            int cnt = 0;
            for (int j = 0; j < grdInward.Rows.Count; j++)
            {
                CheckBox chkbox = (CheckBox)grdInward.Rows[j].Cells[0].FindControl("cbxSelect");
                if (chkbox.Checked)
                    cnt++;
            }

            if (grdInward.Rows.Count > 0)
            {
                lbl_RecordsNo.Text = "Total Records   :  " + grdInward.Rows.Count;
                CheckBox ChkBoxHeader = (CheckBox)grdInward.HeaderRow.FindControl("cbxSelectAll");

                if (cnt == grdInward.Rows.Count)
                    ChkBoxHeader.Checked = true;
                else
                    ChkBoxHeader.Checked = false;
            }
            else 
                lbl_RecordsNo.Text = "Total Records   :  " + "0";
        }

        private void LoadInwardType()
        {
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            //ddl_InwardTestType.DataValueField = "MATERIAL_Id";
            ddl_InwardTestType.DataValueField = "MATERIAL_RecordType_var";
            var inwd = dc.Material_View("", "");
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, "---Select---");
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void lnkUpdateStatus_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Visible = false; bool flag = false;
            if (grdInward.Rows.Count > 0)
            {
                string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                for (int i = 0; i < grdInward.Rows.Count; i++)
                {
                    CheckBox chkbox = (CheckBox)grdInward.Rows[i].Cells[0].FindControl("cbxSelect");
                    if (chkbox.Checked)
                    {
                        flag = true;
                        dc.Inward_Update_TestStartedDetails(Convert.ToString(grdInward.Rows[i].Cells[3].Text), Convert.ToString(grdInward.Rows[i].Cells[1].Text));
                        dc.MISDetail_Update_TestStartedDetails(Convert.ToString(grdInward.Rows[i].Cells[3].Text),Convert.ToString(grdInward.Rows[i].Cells[1].Text));

                        #region update inward data for mobile app
                        int categoryId = 0; // 1   AAC, 2   Cement, 3   Masonary, 4   PT, 5   Tile, 6   Flyash
                        int referenceId = 0, testId = 0, categoryWiseTestId = 0;
                        referenceId = dc.app_reference_number_Update(grdInward.Rows[i].Cells[3].Text);

                        if (ddl_InwardTestType.SelectedValue == "AAC")
                        {
                            #region AAC
                            categoryId = 1;
                            dc.app_category_wise_reference_number_Update(categoryId, referenceId);
                            var Inward = dc.ReportStatus_View("AAC Block Testing", null, null, 0, 0, 0, grdInward.Rows[i].Cells[3].Text, 0, 0, 0);
                            foreach (var inwd in Inward)
                            {
                                testId = Convert.ToInt32(inwd.AACINWD_TEST_Id);

                                if (cnStr.ToLower().Contains("metro") == true)
                                {
                                    if (testId == 800) //Compressive Test
                                        categoryWiseTestId = 1;
                                    else if (testId == 801) //Density Test
                                        categoryWiseTestId = 2;
                                    else if (testId == 809) //Dimension Test
                                        categoryWiseTestId = 3;
                                    else if (testId == 810) //Shrinkage Test
                                        categoryWiseTestId = 4;
                                }
                                else if (cnStr.ToLower().Contains("mumbai") == true)
                                {
                                    if (testId == 802) //Compressive Test
                                        categoryWiseTestId = 1;
                                    else if (testId == 803) //Density Test
                                        categoryWiseTestId = 2;
                                    else if (testId == 804) //Dimension Test
                                        categoryWiseTestId = 3;
                                    else if (testId == 805) //Shrinkage Test
                                        categoryWiseTestId = 4;
                                }
                                else if (cnStr.ToLower().Contains("nashik") == true)
                                {
                                    if (testId == 747) //Compressive Test
                                        categoryWiseTestId = 1;
                                    else if (testId == 748) //Density Test
                                        categoryWiseTestId = 2;
                                    else if (testId == 749) //Dimension Test
                                        categoryWiseTestId = 3;
                                    else if (testId == 750) //Shrinkage Test
                                        categoryWiseTestId = 4;
                                }
                                else
                                {
                                    if (testId == 800) //Compressive Test
                                        categoryWiseTestId = 1;
                                    else if (testId == 801) //Density Test
                                        categoryWiseTestId = 2;
                                    else if (testId == 809) //Dimension Test
                                        categoryWiseTestId = 3;
                                    else if (testId == 810) //Shrinkage Test
                                        categoryWiseTestId = 4;
                                }
                                dc.app_category_refno_wise_test_Update(categoryId, referenceId, categoryWiseTestId, inwd.AACINWD_Quantity_tint);
                                dc.app_category_header_Update(categoryId, referenceId, categoryWiseTestId, inwd.AACINWD_Id_Mark_var, null, null, "", "", "", "");
                            }
                            #endregion
                        }
                        else if (ddl_InwardTestType.SelectedValue == "SOLID")
                        {
                            #region SOLID
                            categoryId = 3;
                            dc.app_category_wise_reference_number_Update(categoryId, referenceId);
                            var Inward = dc.Solid_Inward_View(grdInward.Rows[i].Cells[3].Text, 0, "SOLID");
                            foreach (var inwd in Inward)
                            {
                                testId = Convert.ToInt32(inwd.SOLIDINWD_TEST_Id);

                                if (testId == 66) //Compressive 
                                    categoryWiseTestId = 5;
                                else if (testId == 67) //Water Absorption
                                    categoryWiseTestId = 6;

                                dc.app_category_refno_wise_test_Update(categoryId, referenceId, categoryWiseTestId, inwd.SOLIDINWD_Quantity_tint);
                                dc.app_category_header_Update(categoryId, referenceId, categoryWiseTestId, inwd.SOLIDINWD_IdMark_var, inwd.SOLIDINWD_TestingDate_dt, null, "", "", "", "");
                            }
                            #endregion
                        }
                        else if (ddl_InwardTestType.SelectedValue == "PT")
                        {
                            #region PT
                            categoryId = 4;
                            dc.app_category_wise_reference_number_Update(categoryId, referenceId);
                            var Inward = dc.ReportStatus_View("Pavement Block Testing", null, null, 0, 0, 0, grdInward.Rows[i].Cells[3].Text, 0, 0, 0);
                            foreach (var inwd in Inward)
                            {
                                testId = Convert.ToInt32(inwd.PTINWD_TEST_Id);

                                if (testId == 62) //Compressive 
                                    categoryWiseTestId = 7;
                                else if (testId == 63) //Water Absorption
                                    categoryWiseTestId = 10;
                                else if (testId == 64) //Split Tensile Strength
                                    categoryWiseTestId = 9;
                                else if (testId == 65) //Flexural Strength
                                    categoryWiseTestId = 8;

                                dc.app_category_refno_wise_test_Update(categoryId, referenceId, categoryWiseTestId, inwd.PTINWD_Quantity_tint);
                                DateTime? castingDate = null;
                                if (inwd.PTINWD_CastingDate_dt != "NA")
                                    castingDate = DateTime.ParseExact(inwd.PTINWD_CastingDate_dt, "dd/MM/yyyy", null);
                                dc.app_category_header_Update(categoryId, referenceId, categoryWiseTestId, inwd.PTINWD_Id_Mark_var, inwd.PTINWD_TestingDate_dt, castingDate, inwd.PTINWD_Grade_var, "", "", "");
                            }
                            #endregion
                        }
                        else if (ddl_InwardTestType.SelectedValue == "TILE")
                        {
                            #region Tile
                            categoryId = 5;
                            dc.app_category_wise_reference_number_Update(categoryId, referenceId);
                            var Inward = dc.TileInward_View(grdInward.Rows[i].Cells[3].Text, 0);
                            foreach (var inwd in Inward)
                            {
                                testId = Convert.ToInt32(inwd.TILEINWD_TEST_Id);

                                if (testId == 69) //Dimension Analysis 
                                    categoryWiseTestId = 12;
                                else if (testId == 70) //Water Absorption
                                    categoryWiseTestId = 15;
                                else if (testId == 71) //Modulus Of Rupture
                                    categoryWiseTestId = 13;
                                else if (testId == 72) //Crazing Resistance
                                    categoryWiseTestId = 11;
                                else if (testId == 73) //Wet Transverse
                                    categoryWiseTestId = 16;
                                else if (testId == 74) //Water Absorption of Mosiac / Chequered
                                    categoryWiseTestId = 15;
                                else if (testId == 150) //Scratch Hardness
                                    categoryWiseTestId = 14;

                                dc.app_category_refno_wise_test_Update(categoryId, referenceId, categoryWiseTestId, inwd.TILEINWD_Quantity_tint);
                                dc.app_category_header_Update(categoryId, referenceId, categoryWiseTestId, inwd.TILEINWD_IdMark_var, null, null, "", inwd.TILEINWD_TileType_var, "", "");
                            }
                            #endregion
                        }
                        else if (ddl_InwardTestType.SelectedValue == "CEMT")
                        {
                            #region Cement
                            categoryId = 2;
                            dc.app_category_wise_reference_number_Update(categoryId, referenceId);
                            var Inward = dc.ReportStatus_View("Cement Testing", null, null, 0, 0, 0, grdInward.Rows[i].Cells[3].Text, 0, 0, 0);
                            foreach (var inwd in Inward)
                            {
                                var test = dc.AllInward_View("CEMT", 0, grdInward.Rows[i].Cells[3].Text);
                                foreach (var t in test)
                                {
                                    testId = Convert.ToInt32(t.CEMTTEST_TEST_Id);

                                    if (testId == 33) //Standard Consistency 
                                        categoryWiseTestId = 17;
                                    else if (testId == 42) //Density
                                        categoryWiseTestId = 18;
                                    else if (testId == 39) //Initial Setting Time
                                        categoryWiseTestId = 19;
                                    else if (testId == 141) //Final Setting Time
                                        categoryWiseTestId = 19;
                                    else if (testId == 35) //Soundness By Le-Chateliers Apparatus
                                        categoryWiseTestId = 20;
                                    else if (testId == 40) //Fineness By Blain`s Air Permeability Method
                                        categoryWiseTestId = 21;
                                    else if (testId == 41) //Fineness By Dry Sieving
                                        categoryWiseTestId = 21;
                                    else if (testId == 103 && t.CEMTTEST_Days_tint == 3) //Compressive Strength - 3days
                                        categoryWiseTestId = 22;
                                    else if (testId == 103 && t.CEMTTEST_Days_tint == 7) //Compressive Strength - 7days
                                        categoryWiseTestId = 23;
                                    else if (testId == 103 && t.CEMTTEST_Days_tint == 28) //Compressive Strength - 28days
                                        categoryWiseTestId = 24;
                                    int qty = 1;
                                    if (testId == 103)
                                        qty = 3;
                                    dc.app_category_refno_wise_test_Update(categoryId, referenceId, categoryWiseTestId, qty); //inwd.CEMTINWD_Quantity_tint
                                    dc.app_category_header_Update(categoryId, referenceId, categoryWiseTestId, "", null, null, "", "", inwd.CEMTINWD_Description_var, inwd.CEMTINWD_CementName_var);
                                }
                            }
                            #endregion
                        }
                        else if (ddl_InwardTestType.SelectedValue == "FLYASH")
                        {
                            #region FlyAsh
                            categoryId = 6;
                            dc.app_category_wise_reference_number_Update(categoryId, referenceId);
                            var Inward = dc.ReportStatus_View("Fly Ash Testing", null, null, 0, 0, 0, grdInward.Rows[i].Cells[3].Text, 0, 0, 0);
                            foreach (var inwd in Inward)
                            {
                                var test = dc.AllInward_View("FLYASH", 0, grdInward.Rows[i].Cells[3].Text);
                                foreach (var t in test)
                                {
                                    testId = Convert.ToInt32(t.FLYASHTEST_TEST_Id);

                                    if (testId == 49) //Standard Consistency 
                                        categoryWiseTestId = 25;
                                    else if (testId == 50) //Initial Setting Time
                                        categoryWiseTestId = 27;
                                    else if (testId == 262) //Final Setting Time
                                        categoryWiseTestId = 27;
                                    else if (testId == 51) //Soundness By Le-Chateliers Apparatus
                                        categoryWiseTestId = 28;
                                    else if (testId == 48) //Soundness  By AutoClave 
                                        categoryWiseTestId = 28;
                                    else if (testId == 52) //Fineness By Blain`s apparatus
                                        categoryWiseTestId = 26;
                                    else if (testId == 53) //Fineness By Wet Sieving
                                        categoryWiseTestId = 26;
                                    else if ((testId == 928 && cnStr.ToLower().Contains("mumbai") == false) ||
                                        (testId == 1217 && cnStr.ToLower().Contains("mumbai") == true)) //Fineness By Dry Sieving
                                        categoryWiseTestId = 26;
                                    else if (testId == 54) //Specific Gravity
                                        categoryWiseTestId = 29;
                                    else if (testId == 55) //Lime Reactivity
                                        categoryWiseTestId = 30;
                                    else if (testId == 60 && t.FLYASHTEST_Days_tint == 7) //Compressive Strength - 7days
                                        categoryWiseTestId = 31;
                                    else if (testId == 60 && t.FLYASHTEST_Days_tint == 28) //Compressive Strength - 28days
                                        categoryWiseTestId = 32;
                                    else if (testId == 60 && t.FLYASHTEST_Days_tint == 46) //Compressive Strength - 46days
                                        categoryWiseTestId = 33;
                                    else if (testId == 60 && t.FLYASHTEST_Days_tint == 90) //Compressive Strength - 90days
                                        categoryWiseTestId = 34;
                                    int qty = 1;
                                    if (testId == 60)
                                        qty = 3;
                                    dc.app_category_refno_wise_test_Update(categoryId, referenceId, categoryWiseTestId, qty); //inwd.FLYASHINWD_Quantity_tint
                                    dc.app_category_header_Update(categoryId, referenceId, categoryWiseTestId, "", null, null, "", "", inwd.FLYASHINWD_Description_var, inwd.FLYASHINWD_CementName_var);
                                }
                            }
                            #endregion
                        }
                        else if (ddl_InwardTestType.SelectedValue == "STC")
                        {
                            #region Steel Chemical
                            categoryId = 7;
                            dc.app_category_wise_reference_number_Update(categoryId, referenceId);
                            var Inward = dc.ReportStatus_View("Steel Chemical Testing", null, null, 0, 0, 0, grdInward.Rows[i].Cells[3].Text, 0, 0, 0);
                            foreach (var inwd in Inward)
                            {
                                testId = Convert.ToInt32(inwd.STCINWD_TEST_Id);
                                categoryWiseTestId = 35;
                                dc.app_category_refno_wise_test_Update(categoryId, referenceId, categoryWiseTestId, inwd.STCINWD_Quantity_tint);
                                dc.app_category_header_Update(categoryId, referenceId, categoryWiseTestId, inwd.STCINWD_IdMark_var, null, null, "", "", inwd.STCINWD_Description_var, "");
                            }
                            #endregion
                        }
                        else if (ddl_InwardTestType.SelectedValue == "CCH")
                        {
                            #region Cement Chemical
                            categoryId = 9;
                            dc.app_category_wise_reference_number_Update(categoryId, referenceId);
                            var Inward = dc.ReportStatus_View("Cement Chemical Testing", null, null, 0, 0, 0, grdInward.Rows[i].Cells[3].Text, 0, 0, 0);
                            foreach (var inwd in Inward)
                            {
                                var test = dc.AllInward_View("CCH", 0, grdInward.Rows[i].Cells[3].Text);
                                foreach (var t in test)
                                {
                                    testId = Convert.ToInt32(t.CCHTEST_TEST_Id);

                                    //if (testId == 23) //Calcium Oxide (CaO)
                                    //    categoryWiseTestId = 17;
                                    //else if (testId == 24) //Silica (SiO2)
                                    //    categoryWiseTestId = 18;
                                    //else if (testId == 25) //Ferric Oxide (Fe2O3)
                                    //    categoryWiseTestId = 19;
                                    //else if (testId == 26) //Aluminium Oxide (Al2O3)
                                    //    categoryWiseTestId = 19;
                                    //else if (testId == 27) //Magnesium Oxide (MgO)
                                    //    categoryWiseTestId = 20;
                                    //else if (testId == 28) //Sulphuric Anhydride (SO3)
                                    //    categoryWiseTestId = 21;
                                    //else if (testId == 29) //Loss on Ignition (LOI)
                                    //    categoryWiseTestId = 21;
                                    //else if (testId == 30) //Insoluble Residue (IR)
                                    //    categoryWiseTestId = 22;
                                    //else if (testId == 31) //Chlorides (Cl)
                                    //    categoryWiseTestId = 23;
                                    //else if (testId == 32) //Total Alkalies
                                    //    categoryWiseTestId = 24;
                                    //else if (testId == 32) //Lime Saturation Factor (LSF)
                                    //    categoryWiseTestId = 260;
                                    //else if (testId == 32) //Ratio of Alumina & Iron Oxide (A/F)
                                    //    categoryWiseTestId = 261;

                                    categoryWiseTestId = 45;
                                    dc.app_category_refno_wise_test_Update(categoryId, referenceId, categoryWiseTestId, 1); //inwd.CCHINWD_Quantity_tint
                                    dc.app_category_header_Update(categoryId, referenceId, categoryWiseTestId, "", null, null, "", "", inwd.CCHINWD_Description_var, "");
                                }

                            }
                            #endregion
                        }
                        else if (ddl_InwardTestType.SelectedValue == "GGBS")
                        {
                            #region GGBS 
                            categoryId = 8;
                            dc.app_category_wise_reference_number_Update(categoryId, referenceId);
                            var Inward = dc.ReportStatus_View("GGBS Testing", null, null, 0, 0, 0, grdInward.Rows[i].Cells[3].Text, 0, 0, 0);
                            foreach (var inwd in Inward)
                            {
                                var test = dc.AllInward_View("GGBS", 0, grdInward.Rows[i].Cells[3].Text);
                                foreach (var t in test)
                                {
                                    testId = Convert.ToInt32(t.GGBSTEST_TEST_Id);

                                    if (testId == 1724) //Standard Consistency
                                        categoryWiseTestId = 37;
                                    else if (testId == 1725) //Density
                                        categoryWiseTestId = 38;
                                    else if (testId == 1726) //Initial Setting Time
                                        categoryWiseTestId = 39;
                                    else if (testId == 1727) //Final Setting Time
                                        categoryWiseTestId = 39;
                                    //else if (testId == 1728) //Soundness By Le-Chateliers Apparatus
                                    //    categoryWiseTestId = 20;
                                    //else if (testId == 1729) //Retained on 45 micron wet sieve
                                    //    categoryWiseTestId = 21;
                                    else if (testId == 1730) //Fineness By Dry Sieving 
                                        categoryWiseTestId = 41;
                                    else if (testId == 1731) //Fineness By Blain's Air Permeability Method
                                        categoryWiseTestId = 41;
                                    else if (testId == 1732 && t.GGBSTEST_Days_tint == 3) //Ggbs Compressive Strength - 3days
                                        categoryWiseTestId = 42;
                                    else if (testId == 1732 && t.GGBSTEST_Days_tint == 7) //Ggbs Compressive Strength - 7days
                                        categoryWiseTestId = 43;
                                    else if (testId == 1732 && t.GGBSTEST_Days_tint == 28) //Ggbs Compressive Strength - 28days
                                        categoryWiseTestId = 44;
                                    else if (testId == 1733 && t.GGBSTEST_Days_tint == 3) //Cement Compressive Strength - 3days
                                        categoryWiseTestId = 42;
                                    else if (testId == 1733 && t.GGBSTEST_Days_tint == 7) //Cement Compressive Strength - 7days
                                        categoryWiseTestId = 43;
                                    else if (testId == 1733 && t.GGBSTEST_Days_tint == 28) //Cement Compressive Strength - 28days
                                        categoryWiseTestId = 44;

                                    int qty = 1;
                                    if (testId == 1732 || testId == 1733)
                                        qty = 3;
                                    
                                    dc.app_category_refno_wise_test_Update(categoryId, referenceId, categoryWiseTestId, qty); //inwd.GGBSINWD_Quantity_tint
                                    dc.app_category_header_Update(categoryId, referenceId, categoryWiseTestId, "", null, null, "", "", inwd.GGBSINWD_Description_var, "");
                                }

                            }
                            #endregion
                        }
                        else if (ddl_InwardTestType.SelectedValue == "GGBSCH")
                        {
                            #region GGBS Chemical
                            categoryId = 8;
                            dc.app_category_wise_reference_number_Update(categoryId, referenceId);
                            var Inward = dc.ReportStatus_View("GGBS Chemical Testing", null, null, 0, 0, 0, grdInward.Rows[i].Cells[3].Text, 0, 0, 0);
                            foreach (var inwd in Inward)
                            {
                                var test = dc.AllInward_View("GGBSCH", 0, grdInward.Rows[i].Cells[3].Text);
                                foreach (var t in test)
                                {
                                    //testId = Convert.ToInt32(t.GGBSCHTEST_TEST_Id);
                                    //if (testId == 1734) //Calcium Oxide (CaO)
                                    //    categoryWiseTestId = 17;
                                    //else if (testId == 1735) //Silica (SiO2)
                                    //    categoryWiseTestId = 18;
                                    //else if (testId == 1736) //Ferric Oxide (Fe2O3)
                                    //    categoryWiseTestId = 19;
                                    //else if (testId == 1737) //Aluminium Oxide (Al2O3)
                                    //    categoryWiseTestId = 19;
                                    //else if (testId == 1738) //Magnesium Oxide (MgO)
                                    //    categoryWiseTestId = 20;
                                    //else if (testId == 1739) //Sulphuric Anhydride (SO3)/Sulphate (SO3)
                                    //    categoryWiseTestId = 21;
                                    //else if (testId == 1740) //Loss on Ignition (LOI)
                                    //    categoryWiseTestId = 21;
                                    //else if (testId == 1741) //Insoluble Residue (IR)
                                    //    categoryWiseTestId = 22;
                                    //else if (testId == 1742) //Chlorides (Cl)
                                    //    categoryWiseTestId = 22;
                                    //else if (testId == 1743) //Sulphide sulphur (S)
                                    //    categoryWiseTestId = 22;
                                    //else if (testId == 1744) //Manganese Oxide (MnO)
                                    //    categoryWiseTestId = 22;

                                    categoryWiseTestId = 36;
                                    dc.app_category_refno_wise_test_Update(categoryId, referenceId, categoryWiseTestId, 1); //inwd.GGBSCHINWD_Quantity_tint
                                    dc.app_category_header_Update(categoryId, referenceId, categoryWiseTestId, "", null, null, "", "", inwd.GGBSCHINWD_Description_var, "");
                                }

                            }
                            #endregion
                        }
                        #endregion
                    }
                }
            }
            
            if (flag)
            {
                lblMsg.Text = "Updated successfully.";
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Visible = true;
                BindRecords();
            }
          
        }

        protected void grdInward_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox cbxSelect = (CheckBox)e.Row.Cells[2].FindControl("cbxSelect");
                CheckBox cbxSelectAll = (CheckBox)this.grdInward.HeaderRow.FindControl("cbxSelectAll");
                cbxSelect.Attributes["onclick"] = string.Format
                                                       (
                                                          "javascript:ChildClick(this,'{0}');",
                                                          cbxSelectAll.ClientID
                                                       );
            }
        }

        protected void grdInward_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Control ctrl = e.CommandSource as Control;
            string[] arg = new string[2];
            arg = Convert.ToString(e.CommandArgument).Split(';');

            string recordType = Convert.ToString(arg[0]);
            string ReferenceNo = Convert.ToString(arg[1]);
            if (e.CommandName == "UpdateTestStatus")
            {
                dc.Inward_Update_TestStartedDetails(ReferenceNo, recordType);
                dc.MISDetail_Update_TestStartedDetails(ReferenceNo, recordType);
                BindRecords();
            }

        }

        protected void ddl_InwardTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false; lblMsg.Text = "";
        }


    }
}
