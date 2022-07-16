using System;
using System.Data;

namespace DESPLWEB
{
    public partial class _Default : System.Web.UI.Page
    {
        //string cnStr = "Data Source=Dipl-Server;Initial Catalog=Duro;Persist Security Info=True;User ID=dipl;Password=dipl";
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        static string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
       // LabDataDataContext dc = new LabDataDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
           
            //HtmlMeta keywords = new HtmlMeta();
            //keywords.Name = "keywords";
            //keywords.Content = "Report System";
            //Page.Header.Controls.Add(keywords);
            //txtPass.Attributes.Add("onKeyPress", "doClick('" + LoginButton.ClientID + "',event)");
            txtPass.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + LoginButton.UniqueID + "').click();return false;}} else {return true}; ");
            if (!IsPostBack)
            {
                LoadRecordTypeList();
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            double clID = 0;
            Boolean validData = false, userLogin = false;
            validData = true;
            if (ddlReportAuthenicate.SelectedIndex==0 ||userName.Text == "" || txtPass.Text == "" || userName.Text.Length < 3)
            {
                validData = false;
            }
            else
            {
                if (ddlReportAuthenicate.SelectedValue == "Pune")
                    cnStr = System.Configuration.ConfigurationManager.AppSettings["conStrPune"].ToString();
                else if (ddlReportAuthenicate.SelectedValue == "Mumbai")
                    cnStr = System.Configuration.ConfigurationManager.AppSettings["conStrMumbai"].ToString();
                else if (ddlReportAuthenicate.SelectedValue == "Nashik") //Nashik
                    cnStr = System.Configuration.ConfigurationManager.AppSettings["conStrNashik"].ToString();
                else if (ddlReportAuthenicate.SelectedValue == "MAHA-METRO")
                    cnStr = System.Configuration.ConfigurationManager.AppSettings["conStrMetro"].ToString();


                myDataComm myDb = new myDataComm(cnStr);
                Session["databasename"] = "veena2016";
                if (Session["databasename"].ToString() != "")
                {
                    clID = myDb.verifyLoginUser(userName.Text, txtPass.Text, Session["databasename"].ToString());
                    if (clID == 0)
                    {
                        DataTable dt = new DataTable();
                        dt = myDb.getDiviceLoginForBillApproval(userName.Text, txtPass.Text);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            clID = Convert.ToDouble(dt.Rows[i]["DL_CL_Id"].ToString());
                            Session["ContactNo"] = dt.Rows[i]["DL_ContNo_var"].ToString();
                            Session["BillApproval"] = Convert.ToBoolean(dt.Rows[i]["DL_BillApproval_bit"].ToString());
                            Session["DL_Id"] = dt.Rows[i]["DL_Id"].ToString();
                        }
                        if (clID == 0)
                        {
                            dt = new DataTable();
                            clID = myDb.verifyLoginUser_UserTable(userName.Text, txtPass.Text);
                            userLogin = true;
                        }
                        if (clID == 0)
                        {
                            validData = false;
                        }
                    }
                    if (userLogin == false && ddlReportAuthenicate.SelectedValue == "MAHA-METRO")
                    {
                        lblErrorMsg.Text = "Invalid Location..";
                        lblErrorMsg.Visible = true;
                        validData = false;
                    }
                }
            }
            if (validData == true)
            {
                Session["Location"] = ddlReportAuthenicate.SelectedItem.Text;
                LabDataDataContext dc = new LabDataDataContext(cnStr);
                dc.ClientWebLoginHistory_Update(Convert.ToInt32(clID), DateTime.Today.Date, TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss")), null, ddlReportAuthenicate.SelectedItem.Text.ToString());
                if (userLogin == true)
                {
                    Session["UserId"] = clID.ToString();
                    EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                    string strURLWithData = "BillStatusUser.aspx?" + obj.Encrypt(string.Format("Location={0}", ddlReportAuthenicate.SelectedItem.Text));
                    Response.Redirect(strURLWithData);
                }   
                //else if (clID == 16129)
                //{
                //        //lblLocation.Text = "Pune";
                //    Session["ClientID"] = clID.ToString();
                //    string strURLWithData = "ClientHomeMetro.aspx?" + obj.Encrypt(string.Format("Location={0}", "Pune"));
                //    Response.Redirect(strURLWithData);
                //}
                 else
                {
                    Session["ClientID"] = clID.ToString();
                    Response.Redirect("WebHome.aspx");
                }                
            }
            else
            {
                lblErrorMsg.Text = "Invalid Location/Login Name/Password..";
                lblErrorMsg.Visible = true;
            }

        }

        protected void lnkChangePassword_Click(object sender, EventArgs e)
        {
            if (ddlReportAuthenicate.SelectedIndex != 0)
            {
                if (PnlChangePassowrd.Visible == false)
                {
                    lblErrorMsg.Visible = false;
                    ModalPopupExtender1.Show();
                    txtUserName.Text = "";
                    if (userName.Text.Trim() != "")
                    {
                        txtUserName.Text = userName.Text;
                        txtCurrentPassword.Focus();
                    }
                    PnlChangePassowrd.Visible = true;
                    lblMessage.Visible = false;
                    txtCurrentPassword.Attributes["value"] = "";
                    txtNewPassword.Text = "";
                    txtConfirmNewPassword.Text = "";
                    lnkChangePasswordButton.Enabled = true;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    ModalPopupExtender1.Hide();
                }
            }
            else
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Text = "Please Select Location.";
           }

        }

        protected void lnkExit_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide(); PnlChangePassowrd.Visible = false;
        }
        protected void lnkChangePasswordButton_Click(object sender, EventArgs e)
        {
            if (ValidatePwd() == true)
            {
                LabDataDataContext dc = new LabDataDataContext(cnStr);
                bool valid = false;
                int clientId = 0;
                var client = dc.Client_View_Login(0, txtUserName.Text, "");
                foreach (var c in client)
                {
                    clientId = c.CL_Id;
                }
                var us = dc.Client_View_Login(0, txtUserName.Text, txtCurrentPassword.Text);
                foreach (var u in us)
                {
                    if (u.CL_Password_var.Equals(txtCurrentPassword.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        valid = true;
                    }
                }
                if (valid == true)
                {
                    if (clientId > 0)
                    {
                        dc.Client_Update(clientId, "", 0, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", false, "", txtNewPassword.Text, "", "", "", 0, "", null, false, "",0);
                        lblMessage.Text = "Password has been changed Sucessfully";
                        txtCurrentPassword.Attributes["value"] = "";
                        lblMessage.Visible = true;
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lnkChangePasswordButton.Enabled = false;
                    }
                }
                else
                {
                    lblMessage.Text = "Invalid Login Name /Password...";
                    lblMessage.Visible = true;
                }
            }
        }
        protected Boolean ValidatePwd()
        {
            Boolean valid = true;
            if (txtUserName.Text == string.Empty)
            {
                lblMessage.Text = "Enter Login Name ";
                txtUserName.Focus();
                valid = false;
            }
            else if (txtCurrentPassword.Text == string.Empty)
            {
                lblMessage.Text = "Enter Current Password";
                txtCurrentPassword.Focus();
                valid = false;
            }
            else if (txtNewPassword.Text == string.Empty)
            {
                lblMessage.Text = "Enter new Password";
                txtNewPassword.Focus();
                valid = false;
            }
            else if (txtConfirmNewPassword.Text == string.Empty)
            {
                lblMessage.Text = "Enter Confirm Password";
                txtConfirmNewPassword.Focus();
                valid = false;
            }
            else if (txtConfirmNewPassword.Text != txtNewPassword.Text)
            {
                lblMessage.Text = "New/Confirm Password mismatch";
                txtConfirmNewPassword.Focus();
                valid = false;
            }
            if (valid == false)
            {
                lblMessage.Visible = true;
            }
            else
            {
                lblMessage.Visible = false;
            }
            return valid;
        }

        private void LoadRecordTypeList()
        {
            clsData obj = new clsData();
            DataTable dt = obj.getMaterialList();
        
        }

        protected void lnkNewReg_Click(object sender, EventArgs e)
        {
            Response.Redirect("ClientRegistration.aspx");
        }

        //protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        //{
        //lblMsg.Visible = false; lblMsg.Text = "";
        //if (txtRecordNo.Text != "" && txtRefNo.Text != "" && txtSubsetNo.Text != "" && ddlReportFor.SelectedValue != "--Select--")
        //{                
        //    string cnStr = "";

        //    if(ddlReportAuthenicate.SelectedValue=="Pune")
        //            cnStr = System.Configuration.ConfigurationManager.AppSettings["conStrPune"].ToString();
        //    else if (ddlReportAuthenicate.SelectedValue == "Mumbai")
        //        cnStr = System.Configuration.ConfigurationManager.AppSettings["conStrMumbai"].ToString();
        //    else if (ddlReportAuthenicate.SelectedValue == "Nashik") //Nashik
        //        cnStr = System.Configuration.ConfigurationManager.AppSettings["conStrNashik"].ToString();

        //    LabDataDataContext dc = new LabDataDataContext(cnStr);

        //    bool valFlag = false; int reportCnt = 0, subsetNo = 0, subsetNo1=0;

        //    subsetNo = Convert.ToInt32(txtSubsetNo.Text);
        //    subsetNo1 = Convert.ToInt32(txtSubsetNo1.Text);

        //    //if (subsetNo <= reportCnt && subsetNo!=0)
        //    if (subsetNo !=0 && subsetNo != 0)
        //    {
        //        //string refNo1 = txtRefNo.Text + "/" + reportCnt + "-" + subsetNo;
        //        string refNo = txtRefNo.Text + "/" + subsetNo + "-" + subsetNo1;
        //        var validateRecord = dc.Inward_View(Convert.ToInt32(txtRefNo.Text), Convert.ToInt32(txtRecordNo.Text), ddlReportFor.SelectedValue.ToString(), null, null);
        //        foreach (var vr in validateRecord)
        //        {
        //            reportCnt = Convert.ToInt32(vr.INWD_ReportCount_int);
        //            if (subsetNo1 <= reportCnt)
        //                valFlag = true;
        //        }

        //        if (valFlag)
        //            ViewReport(refNo, ddlReportFor.SelectedValue.ToString(), cnStr);
        //        else
        //        {
        //            lblMsg.Visible = true; lblMsg.Text = "No Record Found!!!";
        //             ddlReportFor.SelectedIndex = -1;
        //        }

        //    }
        //    else
        //    {
        //        lblMsg.Visible = true; lblMsg.Text = "No Record Found!!!";
        //        //txtRecordNo.Text = "";
        //        //txtRefNo.Text = "";
        //        //txtSubsetNo.Text = "";
        //        ddlReportFor.SelectedIndex = -1;
        //    }

        //}
        //}
        
        //protected void ViewReport(string RefNo, string RecType, string con)
        //{
        //    LabDataDataContext dc = new LabDataDataContext(con);
        //    PrintPDFReport rpt = new PrintPDFReport(con);
          
        //    string strAction = "DisplayLogoWithoutNABL";
        //    switch (RecType)
        //    {
        //        case "SO":
        //            var smp = dc.SoilSampleTest_View(RefNo, "");
        //            foreach (var so in smp)
        //            {
        //                rpt.Soil_PDFReport(RefNo, Convert.ToString(so.SOSMPLTEST_SampleName_var), strAction);
        //                break;
        //            }
        //            break;
        //        case "TILE":
        //            rpt.Tile_PDFReport(RefNo, strAction);
        //            break;
        //        case "BT-":
        //            rpt.Brick_PDFReport(RefNo, strAction);
        //            break;
        //        case "FLYASH":
        //            rpt.FlyAsh_PDFReport(RefNo, strAction);
        //            break;
        //        case "CEMT":
        //            rpt.Cement_PDFReport(RefNo, strAction);
        //            break;
        //        case "CCH":
        //            rpt.CCH_PDFReport(RefNo, strAction);
        //            break;
        //        case "CT":
        //            rpt.Cube_PDFReport(RefNo, 0, RecType, "", strAction, "", "");
        //            break;
        //        case "PILE":
        //            rpt.Pile_PDFReport(RefNo, strAction);
        //            break;
        //        case "STC":
        //            rpt.STC_PDFReport(RefNo, strAction);
        //            break;
        //        case "ST":
        //            rpt.ST_PDFReport(RefNo, strAction);
        //            break;
        //        case "WT":
        //            rpt.WT_PDFReport(RefNo, strAction);
        //            break;
        //        case "AGGT":
        //            rpt.Aggregate_PDFReport(RefNo, RecType, "", 0, strAction);
        //            break;
        //        case "SOLID":
        //            var details = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "SOLID");
        //            foreach (var solid in details)
        //            {
        //                if (Convert.ToString(solid.TEST_Sr_No) == "1")//(solid.SOLIDINWD_TEST_Id) == "66")
        //                {
        //                    rpt.SOLID_CS_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(solid.TEST_Sr_No) == "2")
        //                {
        //                    rpt.SOLID_WA_PDFReport(RefNo, strAction);
        //                }
        //            }
        //            break;
        //        case "AAC":
        //            var detail = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "AAC");
        //            foreach (var aac in detail)
        //            {
        //                if (Convert.ToString(aac.TEST_Sr_No) == "1")//(solid.SOLIDINWD_TEST_Id) == "66")
        //                {
        //                    rpt.AAC_CS_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(aac.TEST_Sr_No) == "2")
        //                {
        //                    rpt.AAC_DS_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(aac.TEST_Sr_No) == "3")
        //                {
        //                    rpt.AAC_DM_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(aac.TEST_Sr_No) == "4")
        //                {
        //                    rpt.AAC_SN_PDFReport(RefNo, strAction);
        //                }
        //            }
        //            break;
        //        case "OT":
        //            rpt.OT_PDFReport(RefNo, strAction);
        //            break;
        //        case "CR":
        //            rpt.Core_PDFReport(RefNo, strAction);
        //            break;
        //        case "NDT":
        //            rpt.NDT_PDFReport(RefNo, strAction, "");
        //            break;
        //        case "PT":
        //            var PTdetails = dc.AllInwdDetails_View(RefNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "PT");
        //            foreach (var PTWA in PTdetails)
        //            {
        //                if (Convert.ToString(PTWA.TEST_Sr_No) == "1")//1
        //                {
        //                    rpt.Pavement_CS_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(PTWA.TEST_Sr_No) == "2")//2 //(Convert.ToString(PTWA.PTINWD_TEST_Id) == "63")
        //                {
        //                    rpt.Pavement_WA_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(PTWA.TEST_Sr_No) == "3")//3
        //                {
        //                    rpt.Pavement_TS_PDFReport(RefNo, strAction);
        //                }
        //                else if (Convert.ToString(PTWA.TEST_Sr_No) == "4")//4
        //                {
        //                    rpt.Pavement_FS_PDFReport(RefNo, strAction);
        //                }
        //            }
        //            break;
        //        case "MF":
        //            int trialId = 0;
        //            var trial = dc.Trial_View(RefNo, true);
        //            foreach (var t in trial)
        //            {
        //                trialId = t.Trial_Id;
        //            }
        //            //rpt.TrialMDLetter_Html(RefNo, trialId, "MF", "MDL", strAction);
        //            rpt.MF_MDLetter_PDFReport(RefNo, trialId, "MF", "MDL", strAction);
        //            break;
        //    }
        //}        
        
    }
}