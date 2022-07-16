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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class WebMobileUsers : System.Web.UI.Page
    {
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        static string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["clientId"]) == "0")
                Response.Redirect("default.aspx");
            if (!IsPostBack)
            {
                string strReq = "";
                strReq = Request.RawUrl;
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);

                if (!strReq.Equals(""))
                {
                    strReq = obj.Decrypt(strReq);
                }
                string[] arrMsgs = strReq.Split('=');
                lblLocation.Text = arrMsgs[1].ToString();
                myDataComm cd = new myDataComm();
                string conn = cd.getConnectionStringForWeb(lblLocation.Text);
                //LabDataDataContext dc = new LabDataDataContext(conn);
                lblConnection.Text = conn;
                cnStr=lblConnection.Text;
                bindData();
            }
        }

        public void bindData()
        {
            try
            {
                myDataComm myData = new myDataComm(lblConnection.Text);

                lblclientName.Text = myData.getClientName(Convert.ToDouble(Session["ClientID"].ToString()), "veena2016");
                lblClId.Text = Session["ClientID"].ToString();
                lblEmail.Text = myData.getClientEmail(Convert.ToDouble(lblClId.Text.ToString()));
                DataTable dt = new DataTable();
                dt = myData.getDiviceLoginDetails(Convert.ToDouble(Session["ClientID"].ToString()));
                //grdView.DataSource = dt;
                //grdView.DataBind();
                //ViewState["UserTable"] = dt;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AddRowUser();
                    TextBox txtContcPerson = (TextBox)grdView.Rows[i].Cells[1].FindControl("txtContcPerson");
                    TextBox txtMailId = (TextBox)grdView.Rows[i].Cells[2].FindControl("txtMailId");
                    TextBox txtContact = (TextBox)grdView.Rows[i].Cells[3].FindControl("txtContact");
                    DropDownList ddl_Site = (DropDownList)grdView.Rows[i].FindControl("ddl_Site");
                    Label lblSiteId = (Label)grdView.Rows[i].FindControl("lblSiteId");
                    DropDownList ddl_Status = (DropDownList)grdView.Rows[i].FindControl("ddl_Status");
                    Label lblStatus = (Label)grdView.Rows[i].FindControl("lblStatus");
                    Label lblDLId = (Label)grdView.Rows[i].FindControl("lblDLId");
                    CheckBox chkBillApproval = (CheckBox)grdView.Rows[i].FindControl("chkBillApproval");

                    lblDLId.Text = dt.Rows[i]["DL_Id"].ToString();
                    txtContcPerson.Text = dt.Rows[i]["DL_ContPerson_var"].ToString();
                    txtMailId.Text = dt.Rows[i]["DL_ContEmail_var"].ToString();
                    txtContact.Text = dt.Rows[i]["DL_ContNo_var"].ToString();
                    lblSiteId.Text = dt.Rows[i]["DL_Site_id"].ToString();
                    lblStatus.Text = dt.Rows[i]["DL_Status_bit"].ToString();
                    chkBillApproval.Checked = Convert.ToBoolean(dt.Rows[i]["DL_BillApproval_bit"].ToString());

                    //var data = dc.Site_View(0, Convert.ToInt32(Session["ClientID"]), 0, "").ToList();
                    //ddl_Site.DataSource = data;
                    //ddl_Site.DataTextField = "Site_Name_var";
                    //ddl_Site.DataValueField = "Site_Id";
                    //ddl_Site.DataBind();
                    //ddl_Site.Items.Insert(0, new ListItem("---All---", "0"));
                    if (lblSiteId.Text != "" && Convert.ToInt32(lblSiteId.Text) > 0)
                    {
                        ddl_Site.SelectedValue = lblSiteId.Text;
                    }
                    else
                    {
                        ddl_Site.SelectedValue = "0";
                    }
                    if (lblStatus.Text != "")
                    {
                        ddl_Status.SelectedValue = lblStatus.Text;
                    }

                }

                if (grdView.Rows.Count <= 0)
                {
                    FirstGridViewRowOfUserDetails();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FirstGridViewRowOfUserDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            LabDataDataContext dc = new LabDataDataContext(lblConnection.Text);

            dt.Columns.Add(new DataColumn("DL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("DL_ContPerson_var", typeof(string)));
            dt.Columns.Add(new DataColumn("DL_ContEmail_var", typeof(string)));
            dt.Columns.Add(new DataColumn("DL_ContNo_var", typeof(string)));
            dt.Columns.Add(new DataColumn("DL_Site_id", typeof(string)));
            dt.Columns.Add(new DataColumn("DL_Status_bit", typeof(string)));
            
            dr = dt.NewRow();
            dr["DL_Id"] = 0;
            dr["DL_ContPerson_var"] = string.Empty;
            dr["DL_ContEmail_var"] = string.Empty;
            dr["DL_ContNo_var"] = string.Empty;
            dr["DL_Site_id"] = 0;
            dr["DL_Status_bit"] = string.Empty;

            dt.Rows.Add(dr);

            grdView.DataSource = dt;
            grdView.DataBind();

            for (int i = 0; i < grdView.Rows.Count; i++)
            {
                DropDownList ddl_Site = (DropDownList)grdView.Rows[i].FindControl("ddl_Site");
                var data = dc.Site_View(0, Convert.ToInt32(lblClId.Text), 0, "").ToList();
                ddl_Site.DataSource = data;
                ddl_Site.DataTextField = "Site_Name_var";
                ddl_Site.DataValueField = "Site_Id";
                ddl_Site.DataBind();
                ddl_Site.Items.Insert(0, new ListItem("---All---", "0"));

            }
        }

        protected void imgInsert_Click(object sender, EventArgs e)
        {
            AddRowUser();
        }

        private void AddRowUser()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["UserTable"] != null)
            {
                GetCurrentDataUser();
                dt = (DataTable)ViewState["UserTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("DL_Id", typeof(string)));
                dt.Columns.Add(new DataColumn("DL_ContPerson_var", typeof(string)));
                dt.Columns.Add(new DataColumn("DL_ContEmail_var", typeof(string)));
                dt.Columns.Add(new DataColumn("DL_ContNo_var", typeof(string)));
                dt.Columns.Add(new DataColumn("DL_Site_id", typeof(string)));
                dt.Columns.Add(new DataColumn("DL_Status_bit", typeof(string)));
                dt.Columns.Add(new DataColumn("DL_BillApproval_bit", typeof(string)));
            }

            dr = dt.NewRow();
            dr["DL_Id"] = 0;
            dr["DL_ContPerson_var"] = string.Empty;
            dr["DL_ContEmail_var"] = string.Empty;
            dr["DL_ContNo_var"] = string.Empty;
            dr["DL_Site_id"] = 0;
            dr["DL_Status_bit"] = string.Empty;
            dr["DL_BillApproval_bit"] = "False";

            dt.Rows.Add(dr);

            ViewState["UserTable"] = dt;
            grdView.DataSource = dt;
            grdView.DataBind();
            SetPreviousDataUser();
        }

        private void SetPreviousDataUser()
        {
            LabDataDataContext dc = new LabDataDataContext(lblConnection.Text);
            DataTable dt = (DataTable)ViewState["UserTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtContcPerson = (TextBox)grdView.Rows[i].Cells[1].FindControl("txtContcPerson");
                TextBox txtMailId = (TextBox)grdView.Rows[i].Cells[2].FindControl("txtMailId");
                TextBox txtContact = (TextBox)grdView.Rows[i].Cells[3].FindControl("txtContact");
                DropDownList ddl_Site = (DropDownList)grdView.Rows[i].FindControl("ddl_Site");
                Label lblSiteId = (Label)grdView.Rows[i].FindControl("lblSiteId");
                DropDownList ddl_Status = (DropDownList)grdView.Rows[i].FindControl("ddl_Status");
                Label lblStatus = (Label)grdView.Rows[i].FindControl("lblStatus");
                Label lblDLId = (Label)grdView.Rows[i].FindControl("lblDLId");
                CheckBox chkBillApproval = (CheckBox)grdView.Rows[i].FindControl("chkBillApproval");

                lblDLId.Text = dt.Rows[i]["DL_Id"].ToString();
                txtContcPerson.Text = dt.Rows[i]["DL_ContPerson_var"].ToString();
                txtMailId.Text = dt.Rows[i]["DL_ContEmail_var"].ToString();
                txtContact.Text = dt.Rows[i]["DL_ContNo_var"].ToString();
                lblSiteId.Text = dt.Rows[i]["DL_Site_id"].ToString();
                lblStatus.Text = dt.Rows[i]["DL_Status_bit"].ToString();
                chkBillApproval.Checked = Convert.ToBoolean(dt.Rows[i]["DL_BillApproval_bit"].ToString());

                var data = dc.Site_View(0, Convert.ToInt32(Session["ClientID"]), 0, "").ToList();
                ddl_Site.DataSource = data;
                ddl_Site.DataTextField = "Site_Name_var";
                ddl_Site.DataValueField = "Site_Id";
                ddl_Site.DataBind();
                ddl_Site.Items.Insert(0, new ListItem("---All---", "0"));
                if (lblSiteId.Text != "" && Convert.ToInt32(lblSiteId.Text) > 0)
                {
                    ddl_Site.SelectedValue = lblSiteId.Text;
                }
                else
                    ddl_Site.SelectedValue = "0";
                if (lblStatus.Text != "")
                {
                    ddl_Status.SelectedValue = lblStatus.Text;
                }

            }

        }

        private void GetCurrentDataUser()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("DL_Id", typeof(string)));
            dtTable.Columns.Add(new DataColumn("DL_ContPerson_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("DL_ContEmail_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("DL_ContNo_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("DL_Site_id", typeof(string)));
            dtTable.Columns.Add(new DataColumn("DL_Status_bit", typeof(string)));
            dtTable.Columns.Add(new DataColumn("DL_BillApproval_bit", typeof(string)));

            for (int i = 0; i < grdView.Rows.Count; i++)
            {

                TextBox txtContcPerson = (TextBox)grdView.Rows[i].Cells[1].FindControl("txtContcPerson");
                TextBox txtMailId = (TextBox)grdView.Rows[i].Cells[2].FindControl("txtMailId");
                TextBox txtContact = (TextBox)grdView.Rows[i].Cells[3].FindControl("txtContact");
                DropDownList ddl_Site = (DropDownList)grdView.Rows[i].FindControl("ddl_Site");
                Label lblSiteId = (Label)grdView.Rows[i].FindControl("lblSiteId");
                DropDownList ddl_Status = (DropDownList)grdView.Rows[i].FindControl("ddl_Status");
                Label lblStatus = (Label)grdView.Rows[i].FindControl("lblStatus");
                Label lblDLId = (Label)grdView.Rows[i].FindControl("lblDLId");
                CheckBox chkBillApproval = (CheckBox)grdView.Rows[i].FindControl("chkBillApproval");

                drRow = dtTable.NewRow();
                drRow["DL_Id"] = lblDLId.Text;
                drRow["DL_ContPerson_var"] = txtContcPerson.Text;
                drRow["DL_ContEmail_var"] = txtMailId.Text;
                drRow["DL_ContNo_var"] = txtContact.Text;
                drRow["DL_Site_id"] = lblSiteId.Text;
                drRow["DL_Status_bit"] = lblStatus.Text;
                drRow["DL_BillApproval_bit"] = chkBillApproval.Checked;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["UserTable"] = dtTable;

        }

        protected void grdView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdView.PageIndex = e.NewPageIndex;
            bindData();
            grdView.Visible = true;
        }

        protected void lnkStatus_OnCommand(object sender, CommandEventArgs e)
        {
            string strURLWithData = "";
            strURLWithData = e.CommandArgument.ToString();
            if (strURLWithData != "")
            {

            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("WebHome.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (grdView.Rows.Count > 0)
            {
                bool flag = false;
                List<string> newLogins = new List<string>();
                int status = 1; string password = ""; string strSite = "";
                LabDataDataContext dc = new LabDataDataContext(lblConnection.Text);
                clsSendMail objcls = new clsSendMail();
                for (int i = 0; i < grdView.Rows.Count; i++)
                {
                    TextBox txtContcPerson = (TextBox)grdView.Rows[i].Cells[1].FindControl("txtContcPerson");
                    TextBox txtMailId = (TextBox)grdView.Rows[i].Cells[2].FindControl("txtMailId");
                    TextBox txtContact = (TextBox)grdView.Rows[i].Cells[3].FindControl("txtContact");
                    DropDownList ddl_Site = (DropDownList)grdView.Rows[i].FindControl("ddl_Site");
                    Label lblSiteId = (Label)grdView.Rows[i].FindControl("lblSiteId");
                    DropDownList ddl_Status = (DropDownList)grdView.Rows[i].FindControl("ddl_Status");
                    Label lblStatus = (Label)grdView.Rows[i].FindControl("lblStatus");
                    Label lblDLId = (Label)grdView.Rows[i].FindControl("lblDLId");
                    CheckBox chkBillApproval = (CheckBox)grdView.Rows[i].FindControl("chkBillApproval");

                    if (ddl_Status.SelectedValue.ToString() == "Active")
                        status = 0;
                    if (lblDLId.Text != "" && lblDLId.Text != "0")
                    {
                        dc.DeviceLogin_Update(0, Convert.ToInt32(ddl_Site.SelectedValue), "", "", Convert.ToBoolean(status), txtContcPerson.Text, txtContact.Text, txtMailId.Text, true, Convert.ToInt32(lblDLId.Text), Convert.ToDateTime(DateTime.Now.ToShortDateString()), chkBillApproval.Checked);
                        flag = true;
                    }
                    else
                    {
                        if (txtContcPerson.Text != "" && txtContact.Text != "")
                        {
                            if (ddl_Site.SelectedValue.ToString() == "0")
                                password = "cl" + (Convert.ToInt32(lblClId.Text) + 10).ToString();
                            else
                                password = "st" + (Convert.ToInt32(ddl_Site.SelectedValue.ToString()) + 20).ToString();

                            dc.DeviceLogin_Update(Convert.ToInt32(lblClId.Text), Convert.ToInt32(ddl_Site.SelectedValue), txtContact.Text, password, Convert.ToBoolean(status), txtContcPerson.Text, txtContact.Text, txtMailId.Text, false, 0, Convert.ToDateTime(DateTime.Now.ToShortDateString()), chkBillApproval.Checked);
                            strSite = ddl_Site.SelectedItem.Text;
                            if (ddl_Site.SelectedItem.Text == "---All---")
                                strSite = "All Sites";
                               
                           newLogins.Add(txtContcPerson.Text + "|" + txtMailId.Text + "|" + txtContact.Text + "|" + strSite + "|" + ddl_Status.SelectedItem.Text + "|" + password);
                            flag = true;
                            //send msg to that newly added login
                            if (txtMailId.Text != "" && txtContact.Text != "")
                            {
                                //txtContact.Text = "9545292464";
                                string appLink = "";
                                //if (cnStr.ToLower().Contains("mumbai") == true)
                                //    appLink = "https://play.google.com/store/apps/details?id=com.mumbaiclient";
                                //else if (cnStr.ToLower().Contains("nashik") == true)
                                //    appLink = "https://play.google.com/store/apps/details?id=com.nashik_client";
                                //else
                                appLink = "https://play.google.com/store/apps/details?id=com.all_durocrete_client";
                                string strSms = "Dear Customer, You are added as a registered user on our mobile App. Your password is " + password + ". To download our App click " + appLink + " . You can view reports and place orders on the Durocrete Client App.";
                                objcls.sendSMS(txtContact.Text, strSms, "DUROCR", "1007207804422357658");
                                SendMailToNewlyAddedLogin(txtMailId.Text, txtContact.Text, password, Convert.ToInt32(ddl_Site.SelectedValue), lblClId.Text, strSite);
                            }
                        }
                    }
                }
                if (newLogins.Count > 0)
                {
                    //send mail to client that new logins added
                    //lblEmail.Text = "renuka.p@durocrete.acts-int.com";
                    SendMailToClientNewlyAddedLogin(lblEmail.Text, newLogins);
                }
                if (flag)
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Successfully Saved !');", true);

            }
        }
        private void SendMailToNewlyAddedLogin(string mailIdTo, string conTctNo, string Password, int siteId, string clId, string siteName)
        {
            bool sendMail = true;
            string mCC = "";
            if (mailIdTo == "" || mailIdTo.Trim().ToLower() == "na@unknown.com" ||
                           mailIdTo.Trim().ToLower() == "na" || mailIdTo.Trim().ToLower().Contains("na@") == true ||
                           mailIdTo.Trim().ToLower().Contains("@") == false || mailIdTo.Trim().ToLower().Contains(".") == false)
            {
                sendMail = false;
            }

            if (IsValidEmailAddress(mailIdTo.Trim()) == false)
            {
                sendMail = false;
            }


            if (sendMail == true)
            {
                clsSendMail objMail = new clsSendMail();
                string mSubject = "", mbody = "", mReplyTo = "", tollFree = "";
                if (cnStr.ToLower().Contains("mumbai") == true)
                    tollFree = "9850500013";
                else if (cnStr.ToLower().Contains("nashik") == true)
                    tollFree = "7720006754";
                else
                    tollFree = "18001206465";
                List<string> siteNameList = new List<string>();
                if (siteId != 0)
                {
                    siteNameList.Add(siteName);
                }
                else
                {
                    LabDataDataContext dc = new LabDataDataContext(lblConnection.Text);
                    var data = dc.Site_View(0, Convert.ToInt32(clId), 0, "").ToList();
                    foreach (var item in data)
                        siteNameList.Add(item.SITE_Name_var);

                }
                mSubject = "Login Request Received";

                mbody = "Dear Customer,<br><br>";
                mbody = mbody + @"Welcome to Durocrete portal. You have been added  as registered user for Durocrete Mobile App. You can now place enquiries, view reports directly on your mobile through durocrete app. 
                                    <br> You may download the Durocrete mobile app from play store.";
                mbody = mbody + "<br>Your credentials for our mobile app are as follows : ";
                mbody = mbody + "<br>Login : " + conTctNo + " <br>  Password : " + Password + ".";
                mbody = mbody + "<br>You will be able to access reports for following sites.<br>";
                for (int i = 0; i < siteNameList.Count; i++)
                {
                    mbody = mbody + (i + 1)+ ")&nbsp;&nbsp;" + siteNameList[i] + "<br>";
                }
                mbody = mbody + "<br>For any assistance, please contact on our Toll free no." + tollFree;
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>";
                mbody = mbody + "<br>";
                mbody = mbody + "Best Regards,";
                mbody = mbody + "<br>";
                mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.";

                try
                {
                    objMail.SendMail(mailIdTo, mCC, mSubject, mbody, "", mReplyTo);

                }
                catch { }
            }
        }

        private void SendMailToClientNewlyAddedLogin(string mailIdTo, List<string> newLogins)
        {
            bool sendMail = true;
            string mCC = "";
            if (mailIdTo == "" || mailIdTo.Trim().ToLower() == "na@unknown.com" ||
                           mailIdTo.Trim().ToLower() == "na" || mailIdTo.Trim().ToLower().Contains("na@") == true ||
                           mailIdTo.Trim().ToLower().Contains("@") == false || mailIdTo.Trim().ToLower().Contains(".") == false)
            {
                sendMail = false;
            }

            if (IsValidEmailAddress(mailIdTo.Trim()) == false)
            {
                sendMail = false;
            }


            if (sendMail == true)
            {
                clsSendMail objMail = new clsSendMail();
                string mSubject = "", mbody = "", mReplyTo = "", tollFree = "";
                if (cnStr.ToLower().Contains("mumbai") == true)
                    tollFree = "9850500013";
                else if (cnStr.ToLower().Contains("nashik") == true)
                    tollFree = "7720006754";
                else
                    tollFree = "18001206465";
                mSubject = "Newly Added Login List";

                mbody = "Dear Customer,<br><br>";
                mbody = mbody + "Welcome to Durocrete portal. You have added following users for using Durocrete Mobile App.";
                mbody = mbody + "<br>&nbsp;";
                mbody += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
                mbody += "<tr>";
                mbody += "<td width= 20% align=center valign=top height=19 ><font size=2><b>&nbsp;" + "Login Name" + "</b></font></td>";
                mbody += "<td width= 15% align=center valign=top height=19 ><font size=2><b>" + "Mail Id " + "</b></font></td>";
                mbody += "<td width= 5% align=center valign=top height=19 ><font size=2><b>" + "Contact No" + "</b></font></td>";
                mbody += "<td width= 5% align=center valign=top height=19 ><font size=2><b>" + "Site Name" + "</b></font></td>";
                mbody += "<td width= 5% align=center valign=top height=19 ><font size=2><b>" + "Status " + "</b></font></td>";
                mbody += "</tr>";
                for (int i = 0; i < newLogins.Count; i++)
                {
                    string[] arr = newLogins[i].Split('|');
                    mbody += "<tr>";
                    mbody += "<td width= 20% align=left valign=top height=19 ><font size=2>" + Convert.ToString(arr[0]) + "</font></td>";
                    mbody += "<td width= 15% align=left valign=top height=19 ><font size=2>" + Convert.ToString(arr[1]) + "</font></td>";
                    mbody += "<td width= 10% align=center valign=top height=19 ><font size=2>" + Convert.ToString(arr[2]) + "</font></td>";
                    mbody += "<td width= 15% align=left valign=top height=19 ><font size=2>" + Convert.ToString(arr[3]) + "</font></td>";
                    mbody += "<td width= 8% align=center valign=top height=19 ><font size=2>" + Convert.ToString(arr[4]) + "</font></td>";
                    mbody += "</tr>";

                }
                mbody += "</table>";
                mbody = mbody + "<br>&nbsp;";
                mbody = mbody + "<br>You can de-activate the users from our web portal.";
                mbody = mbody + "<br>For any assistance, please contact on our Toll free no." + tollFree;
                mbody = mbody + "<br>";
                mbody = mbody + "<br>";
                mbody = mbody + "<br>";
                mbody = mbody + "Best Regards,";
                mbody = mbody + "<br>";
                mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.";

                try
                {
                    objMail.SendMail(mailIdTo, mCC, mSubject, mbody, "", mReplyTo);

                }
                catch { }
            }
        }

        public bool IsValidEmailAddress(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");

            }
        }

    }
}
