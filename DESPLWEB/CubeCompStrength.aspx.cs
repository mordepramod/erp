using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class CubeCompStrength : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strReq = "";
                strReq = Request.RawUrl;
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);
                if (!strReq.Equals(""))
                {
                    strReq = obj.Decrypt(strReq);
                    if (!strReq.Contains("=") == false)
                    {
                        string[] arrMsgs = strReq.Split('&');
                        string[] arrIndMsg;

                        arrIndMsg = arrMsgs[0].Split('=');
                        txt_RecordType.Text = arrIndMsg[1].ToString().Trim();

                        arrIndMsg = arrMsgs[1].Split('=');
                        txt_ReferenceNo.Text = arrIndMsg[1].ToString().Trim();
                    }
                }

                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                if (txt_RecordType.Text == "GGBS")
                    lblheading.Text = "GGBS Cube Casting";
                else
                    lblheading.Text = "Cement Cube Casting";

                if (txt_ReferenceNo.Text  != "")
                {
                    DisplaygrD();
                }
                getCastingDate();

            }
        }
        public void getCastingDate()
        {
            this.txt_CatsingDt.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }

        public void DisplaygrD()
        {

            int i = 0;
            int TestId = 0;
            bool slagActivityFound = false;
            var cmttest = dc.AllInward_View(txt_RecordType.Text, 0,  txt_ReferenceNo.Text );
            foreach (var wt in cmttest)
            {
                if (Convert.ToInt32(wt.CEMTTEST_TEST_Id) > 0 && Convert.ToString(wt.CEMTTEST_TEST_Id) != string.Empty)
                {
                    TestId = Convert.ToInt32(wt.CEMTTEST_TEST_Id);
                }
                else if (Convert.ToInt32(wt.FLYASHTEST_TEST_Id) > 0 && Convert.ToString(wt.FLYASHTEST_TEST_Id) != string.Empty)
                {
                    TestId = Convert.ToInt32(wt.FLYASHTEST_TEST_Id);
                }
                else if (Convert.ToInt32(wt.GGBSTEST_TEST_Id) > 0 && Convert.ToString(wt.GGBSTEST_TEST_Id) != string.Empty)
                {
                    TestId = Convert.ToInt32(wt.GGBSTEST_TEST_Id);
                }
                var c = dc.Test_View(0, TestId, "", 0, 0, 0);
                foreach (var n in c)
                {
                    if (n.TEST_Name_var.ToString() == "Compressive Strength" || n.TEST_Name_var.ToString() == "Slag activity index")
                    {
                        if (Convert.ToString(wt.CEMTTEST_Days_tint) != "" && Convert.ToInt32(wt.CEMTTEST_Days_tint) != 0)
                        {
                            AddRowEnterCompStrength();
                            TextBox txt_Days = (TextBox)grdCubeCompStrength.Rows[i].Cells[1].FindControl("txt_Days");
                            txt_Days.Text = Convert.ToString(wt.CEMTTEST_Days_tint);
                            i++;
                        }
                        else if (Convert.ToString(wt.FLYASHTEST_Days_tint) != "" && Convert.ToInt32(wt.FLYASHTEST_Days_tint) != 0)
                        {
                            AddRowEnterCompStrength();
                            TextBox txt_Days = (TextBox)grdCubeCompStrength.Rows[i].Cells[1].FindControl("txt_Days");
                            txt_Days.Text = Convert.ToString(wt.FLYASHTEST_Days_tint);
                            i++;
                        }
                        else if (Convert.ToString(wt.GGBSTEST_Days_tint) != "" && Convert.ToInt32(wt.GGBSTEST_Days_tint) != 0)
                        {
                            AddRowEnterCompStrength();
                            TextBox txt_Days = (TextBox)grdCubeCompStrength.Rows[i].Cells[1].FindControl("txt_Days");
                            txt_Days.Text = Convert.ToString(wt.GGBSTEST_Days_tint);
                            i++;
                        }
                        if (Convert.ToString(wt.FLYASHTEST_Days_tint) == "28")
                        {

                            lbl_CementCubes.Visible = true;
                            txt_CementCubes.Visible = true;
                        }
                    }
                    if (n.TEST_Name_var.ToString() == "Slag activity index")
                        slagActivityFound = true;
                }
            }
            if (slagActivityFound == true)
            {
                grdCubeCompStrength.Columns[3].Visible = true;
            }
            else
            {
                grdCubeCompStrength.Columns[3].Visible = false;
            }
            BindCastingValue();
        }
        public void BindCastingValue()
        {
            var inwd = dc.OtherCubeTestView(txt_ReferenceNo.Text, txt_RecordType.Text, 0, 0, "", true, false);
            foreach (var n in inwd)
            {
                if (Convert.ToString(n.CEMTINWD_CubeCastingStatus_tint) == "1")
                {
                    lnkSave.Enabled = false;
                    break;
                }
                if (Convert.ToString(n.FLYASHINWD_CubeCastingStatus_tint) == "1")
                {
                    lnkSave.Enabled = false;
                    break;
                }
                if (Convert.ToString(n.GGBSINWD_CubeCastingStatus_tint) == "1")
                {
                    lnkSave.Enabled = false;
                    break;
                }
            }

            for (int i = 0; i < grdCubeCompStrength.Rows.Count; i++)
            {
                TextBox txt_Days = (TextBox)grdCubeCompStrength.Rows[i].FindControl("txt_Days");
                TextBox txt_NoOfcubes = (TextBox)grdCubeCompStrength.Rows[i].FindControl("txt_NoOfcubes");
                
                var det = dc.OtherCubeTestView(txt_ReferenceNo.Text, txt_RecordType.Text, Convert.ToByte(txt_Days.Text), 0, txt_RecordType.Text, false, false);
                foreach (var cm in det)
                {
                    txt_NoOfcubes.ReadOnly = true;
                    txt_NoOfcubes.Text = Convert.ToString(cm.NoOfCubes_tint);
                }
            }
            if (txt_RecordType.Text == "FLYASH")
            {
                if (txt_CementCubes.Visible == true)
                {
                    var det = dc.OtherCubeTestView(txt_ReferenceNo.Text, txt_RecordType.Text, 28, 0, "CEMT", false, false);
                    foreach (var cm in det)
                    {
                        txt_CementCubes.Text = Convert.ToString(cm.NoOfCubes_tint);
                    }
                }
            }
            else if (txt_RecordType.Text == "GGBS" && grdCubeCompStrength.Columns[3].Visible == true)
            {
                for (int i = 0; i < grdCubeCompStrength.Rows.Count; i++)
                {
                    TextBox txt_Days = (TextBox)grdCubeCompStrength.Rows[i].FindControl("txt_Days");
                    TextBox txt_NoOfCementCubes = (TextBox)grdCubeCompStrength.Rows[i].FindControl("txt_NoOfCementCubes");

                    var det = dc.OtherCubeTestView(txt_ReferenceNo.Text, txt_RecordType.Text, Convert.ToByte(txt_Days.Text), 0, "CEMT", false, false);
                    foreach (var cm in det)
                    {
                        txt_NoOfCementCubes.ReadOnly = true;
                        txt_NoOfCementCubes.Text = Convert.ToString(cm.NoOfCubes_tint);
                    }
                }
            }
        }
    

        protected void AddRowEnterCompStrength()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CompStrngthTable"] != null)
            {
                GetCurrentDataCompStrength();
                dt = (DataTable)ViewState["CompStrngthTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_Days", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_NoOfcubes", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_NoOfCementCubes", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_Days"] = string.Empty;
            dr["txt_NoOfcubes"] = string.Empty;
            dr["txt_NoOfCementCubes"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["CompStrngthTable"] = dt;
            grdCubeCompStrength.DataSource = dt;
            grdCubeCompStrength.DataBind();
            SetPreviousDataCompStrength();
        }
        protected void GetCurrentDataCompStrength()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_Days", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_NoOfcubes", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_NoOfCementCubes", typeof(string)));

            for (int i = 0; i < grdCubeCompStrength.Rows.Count; i++)
            {
                TextBox txt_Days = (TextBox)grdCubeCompStrength.Rows[i].Cells[1].FindControl("txt_Days");
                TextBox txt_NoOfcubes = (TextBox)grdCubeCompStrength.Rows[i].Cells[2].FindControl("txt_NoOfcubes");
                TextBox txt_NoOfCementCubes = (TextBox)grdCubeCompStrength.Rows[i].Cells[3].FindControl("txt_NoOfCementCubes");

                drRow = dtTable.NewRow();
                drRow["txt_Days"] = txt_Days.Text;
                drRow["txt_NoOfcubes"] = txt_NoOfcubes.Text;
                drRow["txt_NoOfCementCubes"] = txt_NoOfCementCubes.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CompStrngthTable"] = dtTable;

        }
        protected void SetPreviousDataCompStrength()
        {
            DataTable dt = (DataTable)ViewState["CompStrngthTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Days = (TextBox)grdCubeCompStrength.Rows[i].Cells[1].FindControl("txt_Days");
                TextBox txt_NoOfcubes = (TextBox)grdCubeCompStrength.Rows[i].Cells[2].FindControl("txt_NoOfcubes");
                TextBox txt_NoOfCementCubes = (TextBox)grdCubeCompStrength.Rows[i].Cells[3].FindControl("txt_NoOfCementCubes");

                txt_Days.Text = dt.Rows[i]["txt_Days"].ToString();
                txt_NoOfcubes.Text = dt.Rows[i]["txt_NoOfcubes"].ToString();
                txt_NoOfCementCubes.Text = dt.Rows[i]["txt_NoOfCementCubes"].ToString();
            }

        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                DateTime Castingdate = DateTime.Now;
                if (txt_CatsingDt.Text == "NA")
                {
                    txt_CatsingDt.Text = txt_CatsingDt.Text.ToUpper();
                }
                DateTime TestingDt = DateTime.Now;
                dc.OtherCubeTest_Update(txt_ReferenceNo.Text, txt_RecordType.Text, 0, 0, 0, "", 0,"", false, true);

                for (int i = 0; i < grdCubeCompStrength.Rows.Count; i++)
                {
                    TextBox txt_Days = (TextBox)grdCubeCompStrength.Rows[i].Cells[1].FindControl("txt_Days");
                    TextBox txt_NoOfcubes = (TextBox)grdCubeCompStrength.Rows[i].Cells[2].FindControl("txt_NoOfcubes");
                    
                    Castingdate = DateTime.ParseExact(txt_CatsingDt.Text, "dd/MM/yyyy", null);
                    TestingDt = Castingdate.AddDays(Convert.ToInt32(txt_Days.Text));

                    dc.OtherCubeTest_Update(txt_ReferenceNo.Text, txt_RecordType.Text, Convert.ToByte(txt_Days.Text), Convert.ToByte(txt_NoOfcubes.Text), 1, txt_RecordType.Text, 0, "", false, false);
                    dc.CubeCastingStatus_Update(txt_ReferenceNo.Text, Convert.ToByte(txt_Days.Text), txt_CatsingDt.Text, TestingDt, 1, txt_RecordType.Text, 0);//UPDATE CEMENT,FLYASH,GGBS INWARD
                }
                if (txt_RecordType.Text == "FLYASH" && txt_CementCubes.Visible == true && txt_CementCubes.Text != string.Empty)
                {
                    Castingdate = DateTime.ParseExact(txt_CatsingDt.Text, "dd/MM/yyyy", null);
                    TestingDt = Castingdate.AddDays(28);
                    dc.OtherCubeTest_Update(txt_ReferenceNo.Text, txt_RecordType.Text, 28, Convert.ToByte(txt_CementCubes.Text), 1, "CEMT", 0, "", false, false);
                    dc.CubeCastingStatus_Update(txt_ReferenceNo.Text, 28, txt_CatsingDt.Text, TestingDt, 1, txt_RecordType.Text, 0);//UPDATE FLYASH INWARD                    
                }
                else if (txt_RecordType.Text == "GGBS" && grdCubeCompStrength.Columns[3].Visible== true)
                {
                    for (int i = 0; i < grdCubeCompStrength.Rows.Count; i++)
                    {
                        TextBox txt_Days = (TextBox)grdCubeCompStrength.Rows[i].Cells[1].FindControl("txt_Days");
                        TextBox txt_NoOfCementCubes = (TextBox)grdCubeCompStrength.Rows[i].Cells[2].FindControl("txt_NoOfCementCubes");
                        
                        Castingdate = DateTime.ParseExact(txt_CatsingDt.Text, "dd/MM/yyyy", null);
                        TestingDt = Castingdate.AddDays(Convert.ToInt32(txt_Days.Text));
                        dc.OtherCubeTest_Update(txt_ReferenceNo.Text, txt_RecordType.Text, Convert.ToByte(txt_Days.Text), Convert.ToByte(txt_NoOfCementCubes.Text), 1, "CEMT", 0, "", false, false);
                        dc.CubeCastingStatus_Update(txt_ReferenceNo.Text, Convert.ToByte(txt_Days.Text), txt_CatsingDt.Text, TestingDt, 1, txt_RecordType.Text, 0);//UPDATE GGBS INWARD
                    }
                }

                //for (int i = 0; i < grdCubeCompStrength.Rows.Count; i++)
                //{
                //    TextBox txt_Days = (TextBox)grdCubeCompStrength.Rows[i].Cells[1].FindControl("txt_Days");
                //    TextBox txt_NoOfcubes = (TextBox)grdCubeCompStrength.Rows[i].Cells[2].FindControl("txt_NoOfcubes");
                //    if (txt_RecordType.Text == "FLYASH")
                //    {
                //        if (Convert.ToString(txt_Days.Text) == "28")
                //        {
                //            string CubeType_var = string.Empty;
                //            if (txt_CementCubes.Visible == true && txt_CementCubes.Text != string.Empty)
                //            {
                //                CubeType_var = "CEMT";
                //            }
                //            dc.OtherCubeTest_Update(txt_ReferenceNo.Text, txt_RecordType.Text, Convert.ToByte(txt_Days.Text), Convert.ToByte(txt_CementCubes.Text), 1, CubeType_var, 0,"", false, false);
                //            TestingDt = Castingdate.AddDays(Convert.ToInt32(txt_Days.Text));

                //            dc.CubeCastingStatus_Update(txt_ReferenceNo.Text, Convert.ToByte(txt_Days.Text), txt_CatsingDt.Text, TestingDt, 1, txt_RecordType.Text, 0);//UPDATE CEMENT INWARD
                //            break;
                //        }
                //    }
                //}
                
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lnkSave.Enabled = false;
            }
        }


        protected Boolean ValidateData()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            string CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime TestingDate = DateTime.ParseExact(txt_CatsingDt.Text, "dd/MM/yyyy", null);
            DateTime currentDate1 = DateTime.ParseExact(CurrentDate, "dd/MM/yyyy", null);
            if (TestingDate > currentDate1)
            {
                lblMsg.Text = "Casting Date should be always less than or equal to the Current Date.";
                valid = false;
            }
            else if (txt_CementCubes.Visible == true && txt_CementCubes.Text == string.Empty)
            {
                lblMsg.Text = "Please Enter No of Cubes of 28 Days Cement Cubes";
                valid = false;
            }
            else if (valid == true)
            {
                //int j = 0;
                for (int i = 0; i < grdCubeCompStrength.Rows.Count; i++)
                {
                    TextBox txt_Days = (TextBox)grdCubeCompStrength.Rows[i].Cells[1].FindControl("txt_Days");
                    TextBox txt_NoOfcubes = (TextBox)grdCubeCompStrength.Rows[i].Cells[2].FindControl("txt_NoOfcubes");
                    TextBox txt_NoOfCementCubes = (TextBox)grdCubeCompStrength.Rows[i].Cells[2].FindControl("txt_NoOfCementCubes");

                    if (txt_NoOfcubes.Text == "")
                    {
                        lblMsg.Text = "Please Enter No of Cubes for row No. " + (i + 1) + ".";
                        txt_NoOfcubes.Focus();
                        valid = false;
                        break;
                    }
                    else if (Convert.ToDouble(txt_NoOfcubes.Text) <= 0)
                    {
                        lblMsg.Text = "No of Cubes should be greater than 0 for row No. " + (i + 1) + ".";
                        txt_NoOfcubes.Focus();
                        valid = false;
                        break;
                    }
                    else if (grdCubeCompStrength.Columns[3].Visible == true)
                    {
                        if (txt_NoOfCementCubes.Text == "")
                        {
                            lblMsg.Text = "Please Enter No of Cement Cubes for row No. " + (i + 1) + ".";
                            txt_NoOfCementCubes.Focus();
                            valid = false;
                            break;
                        }
                        else if (Convert.ToDouble(txt_NoOfCementCubes.Text) <= 0)
                        {
                            lblMsg.Text = "No of Cement Cubes should be greater than 0 for row No. " + (i + 1) + ".";
                            txt_NoOfCementCubes.Focus();
                            valid = false;
                            break;
                        }
                    }
                }
            }
            if (valid == false)
            {
                lblMsg.Visible = true;
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
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