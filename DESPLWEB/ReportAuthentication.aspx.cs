using System;
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
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace DESPLWEB
{
    public partial class ReportAuthentication : System.Web.UI.Page
    {
        //string cnStr = "Data Source=Dipl-Server;Initial Catalog=Duro;Persist Security Info=True;User ID=dipl;Password=dipl";

        static string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
    
        protected void Page_Load(object sender, EventArgs e)
        {
           
            HtmlMeta keywords = new HtmlMeta();
            keywords.Name = "keywords";
            keywords.Content = "Report System";
            //Page.Header.Controls.Add(keywords);
            // PassWord.Attributes.Add("onKeyPress", "doClick('" + LoginButton.ClientID + "',event)");
            if (!IsPostBack)
            {
               
                LoadRecordTypeList();
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        
        }

        protected void lnkChangePassword_Click(object sender, EventArgs e)
        {
        
        }

        protected void lnkExit_Click(object sender, EventArgs e)
        {
           // PnlChangePassowrd.Visible = false;
        }
        protected void lnkChangePasswordButton_Click(object sender, EventArgs e)
        {
            if (ValidatePwd() == true)
            {
                //LabDataDataContext dc = new LabDataDataContext();
                //bool valid = false;
                //int clientId = 0;
                //var client = dc.Client_View_Login(0, txtUserName.Text, "");
                //foreach (var c in client)
                //{
                //    clientId = c.CL_Id;
                //}
                //var us = dc.Client_View_Login(0, txtUserName.Text, txtCurrentPassword.Text);
                //foreach (var u in us)
                //{
                //    if (u.CL_Password_var.Equals(txtCurrentPassword.Text, StringComparison.OrdinalIgnoreCase))
                //    {
                //        valid = true;
                //    }
                //}
                //if (valid == true)
                //{
                //    if (clientId > 0)
                //    {
                //        dc.Client_Update(clientId, "", 0, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", false, "", txtNewPassword.Text, "", "", "", 0, "", null, false, "",0);
                //        lblMessage.Text = "Password has been changed Sucessfully";
                //        txtCurrentPassword.Attributes["value"] = "";
                //        lblMessage.Visible = true;
                //        lblMessage.ForeColor = System.Drawing.Color.Green;
                //        lnkChangePasswordButton.Enabled = false;
                //    }
                //}
                //else
                //{
                //    lblMessage.Text = "Invalid Login Name /Password...";
                //    lblMessage.Visible = true;
                //}
            }
        }
        protected Boolean ValidatePwd()
        {
            Boolean valid = true;
            //if (txtUserName.Text == string.Empty)
            //{
            //    lblMessage.Text = "Enter Login Name ";
            //    txtUserName.Focus();
            //    valid = false;
            //}
            //else if (txtCurrentPassword.Text == string.Empty)
            //{
            //    lblMessage.Text = "Enter Current Password";
            //    txtCurrentPassword.Focus();
            //    valid = false;
            //}
            //else if (txtNewPassword.Text == string.Empty)
            //{
            //    lblMessage.Text = "Enter new Password";
            //    txtNewPassword.Focus();
            //    valid = false;
            //}
            //else if (txtConfirmNewPassword.Text == string.Empty)
            //{
            //    lblMessage.Text = "Enter Confirm Password";
            //    txtConfirmNewPassword.Focus();
            //    valid = false;
            //}
            //else if (txtConfirmNewPassword.Text != txtNewPassword.Text)
            //{
            //    lblMessage.Text = "New/Confirm Password mismatch";
            //    txtConfirmNewPassword.Focus();
            //    valid = false;
            //}
            //if (valid == false)
            //{
            //    lblMessage.Visible = true;
            //}
            //else
            //{
            //    lblMessage.Visible = false;
            //}
            return valid;
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            
            lblMsg.Visible = false; lblMsg.Text = "";
            if (txtRecordNo.Text != "" && txtRefNo.Text != "" && txtSubsetNo.Text != "" && ddlReportFor.SelectedValue != "--Select--")
            {                
                string cnStr = "";

                if(ddlReportAuthenicate.SelectedValue=="Pune")
                    cnStr = System.Configuration.ConfigurationManager.AppSettings["conStrPune"].ToString();
                else if (ddlReportAuthenicate.SelectedValue == "Mumbai")
                    cnStr = System.Configuration.ConfigurationManager.AppSettings["conStrMumbai"].ToString();
                else if (ddlReportAuthenicate.SelectedValue == "Nashik") //Nashik
                    cnStr = System.Configuration.ConfigurationManager.AppSettings["conStrNashik"].ToString();
                //else if (ddlReportAuthenicate.SelectedValue == "Metro") 
                //    cnStr = System.Configuration.ConfigurationManager.AppSettings["conStrMetro"].ToString();

                LabDataDataContext dc = new LabDataDataContext(cnStr);

                bool valFlag = false; int reportCnt = 0, subsetNo = 0, subsetNo1=0;
              
                subsetNo = Convert.ToInt32(txtSubsetNo.Text);
                subsetNo1 = Convert.ToInt32(txtSubsetNo1.Text);

                //if (subsetNo <= reportCnt && subsetNo!=0)
                if (subsetNo !=0 && subsetNo != 0)
                {
                    //string refNo1 = txtRefNo.Text + "/" + reportCnt + "-" + subsetNo;
                    string refNo = txtRefNo.Text + "/" + subsetNo + "-" + subsetNo1;
                    var validateRecord = dc.Inward_View(Convert.ToInt32(txtRefNo.Text), Convert.ToInt32(txtRecordNo.Text), ddlReportFor.SelectedValue.ToString(), null, null);
                    foreach (var vr in validateRecord)
                    {
                        reportCnt = Convert.ToInt32(vr.INWD_ReportCount_int);
                        if (subsetNo1 <= reportCnt)
                            valFlag = true;
                    }

                    if (valFlag)
                    {
                        //ViewReport(refNo, ddlReportFor.SelectedValue.ToString(), cnStr);
                        PrintPDFReport rpt = new PrintPDFReport(cnStr);
                        rpt.PrintSelectedReport(ddlReportFor.SelectedValue.ToString(), refNo, "DisplayLogoWithoutNABL", "", "", "MDL", "", "", "", "");
                    }
                    else
                    {
                        lblMsg.Visible = true; lblMsg.Text = "No Record Found!!!";
                        ddlReportFor.SelectedIndex = -1;
                    }

                }
                else
                {
                    lblMsg.Visible = true; lblMsg.Text = "No Record Found!!!";
                    //txtRecordNo.Text = "";
                    //txtRefNo.Text = "";
                    //txtSubsetNo.Text = "";
                    ddlReportFor.SelectedIndex = -1;
                }

            }
        }
        private void LoadRecordTypeList()
        {
            //LabDataDataContext dc = new LabDataDataContext();
            //var inwd = dc.Material_View("", "");
            clsData obj = new clsData();
            DataTable dt = obj.getMaterialList();
            ddlReportFor.DataSource = dt;
            ddlReportFor.DataTextField = "MatNewColm";
            ddlReportFor.DataValueField = "MATERIAL_RecordType_var";
            ddlReportFor.DataBind();
            ddlReportFor.Items.Insert(0, new ListItem("--Select--", "--Select--"));

        }

        protected void lnkReportAuth_Click(object sender, EventArgs e)
        {
            Response.Redirect("");
        }

        //protected void ViewReport(string RefNo, string RecType,string con)
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