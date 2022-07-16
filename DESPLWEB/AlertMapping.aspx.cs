using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class AlertMapping : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Alert Mapping";
                //LoadAlertList(1,"Trigger");
                txt_FromDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                bool userRight = false;
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                    foreach (var u in user)
                    {
                        if (u.USER_SuperAdmin_right_bit == true)
                            userRight = true;
                    }
                    if (userRight == false)
                    {
                        pnlContent.Visible = false;
                        lblAccess.Visible = true;
                        lblAccess.Text = "Access is Denied.. ";
                    }
                  
                }
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
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (RdnTrigger.Checked)
                LoadAlertList(1, "Trigger");
            else
                LoadAlertList(2, "Escalation");
        }
        protected void RdnTrigger_CheckedChanged(object sender, EventArgs e)
        {
            grdAlert.DataSource = null;
            grdAlert.DataBind();
            tr1.Visible = false;
            tr2.Visible = false;
            tr3.Visible = false;
        }
        protected void RdnEscalation_CheckedChanged(object sender, EventArgs e)
        {
            grdAlert.DataSource = null;
            grdAlert.DataBind();
            tr1.Visible = false;
            tr2.Visible = false;
            tr3.Visible = false;
        }
        protected void LoadAlertList(int parentId, string type)
        {
            ClearControls();
            grdAlert.DataSource = null;
            grdAlert.DataBind();
            if (parentId == 2)//escalation
            {
                grdAlert.Columns[3].Visible = false;
            }
            else //trigger
            {
                grdAlert.Columns[3].Visible = true;
            }
            DataTable dtTable = new DataTable();
            DataRow drRow = null;
            dtTable.Columns.Add(new DataColumn("lblAlertId", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Alert_Description_var", typeof(string)));
            dtTable.Columns.Add(new DataColumn("Alert_Delay_int", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtEmailId", typeof(string)));
            string xstr;

            var alert = dc.Alert_View(0, type).ToList();
            int alertNo = 0, count = 0;
            if (alert.Count > 0)
            {
                foreach (var alrt in alert)
                {
                    drRow = dtTable.NewRow();
                    drRow["lblAlertId"] = alrt.Alert_Id.ToString();
                    drRow["Alert_Description_var"] = alrt.Alert_Description_var.ToString();
                    drRow["Alert_Delay_int"] = alrt.Alert_Delay_int.ToString();
                    dtTable.Rows.Add(drRow);
                }
            }
            if (dtTable.Rows.Count > 0)
                count = Convert.ToInt32(dtTable.Rows[0][0].ToString());
            var alert1 = dc.Alert_View(parentId, type);
            foreach (var alrt in alert1)
            {
                if (parentId == alrt.AlertMap_ParentLevelId_int)
                {
                    alertNo = alrt.Alert_Id - count;
                    if (alrt.AlertMap_EmailId_var != "")
                    {
                        xstr = dtTable.Rows[alertNo]["txtEmailId"].ToString();
                        if (xstr != "")
                            dtTable.Rows[alertNo]["txtEmailId"] = xstr + "," + alrt.AlertMap_EmailId_var;
                        else
                            dtTable.Rows[alertNo]["txtEmailId"] = alrt.AlertMap_EmailId_var;
                    }
                }
            }
            grdAlert.DataSource = dtTable;
            grdAlert.DataBind();
            clsData cd = new clsData();
            for (int i = 0; i < grdAlert.Rows.Count; i++)
            {
                Label lblAlertId = (Label)grdAlert.Rows[i].FindControl("lblAlertId");
                TextBox txtEmailId = (TextBox)grdAlert.Rows[i].FindControl("txtEmailId");
                LinkButton lnkViewDetails = (LinkButton)grdAlert.Rows[i].FindControl("lnkViewDetails");
            
                lblAlertId.Text = dtTable.Rows[i]["lblAlertId"].ToString();
                txtEmailId.Text = dtTable.Rows[i]["txtEmailId"].ToString();
               
                if (lblAlertId.Text != "0" && lblAlertId.Text != "11" && lblAlertId.Text != "17"  && lblAlertId.Text != "29" && lblAlertId.Text != "42" )
                    lnkViewDetails.Enabled = true;
                else
                    lnkViewDetails.Enabled = false;
            }

        }
   
    
        protected void lnkViewDetails(object sender, CommandEventArgs e)
        {
            clsData cd = new clsData();
            LinkButton lb = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)lb.NamingContainer;
            Label lblAlertId = (Label)gr.FindControl("lblAlertId");
            Label lblRowNumber = (Label)gr.FindControl("lblRowNumber");
            //string AlertName = lblRowNumber.Text+") " + gr.Cells[2].Text;

            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
            int Status = 0;
            if (chkPending.Checked == true)
                Status = 1;
            lblAlertNo.Text = lblRowNumber.Text + ") ";
            lblAlertName.Text = gr.Cells[2].Text;
            tr1.Visible = true;
            tr2.Visible = true;
            tr3.Visible = true;

            grdAlertList.Columns[0].Visible = false;
            grdAlertList.Columns[1].Visible = false;
            grdAlertList.Columns[2].Visible = false;
            grdAlertList.Columns[3].Visible = false;
            grdAlertList.Columns[4].Visible = false;
            grdAlertList.Columns[5].Visible = false;
            grdAlertList.Columns[6].Visible = false;
            grdAlertList.Columns[7].Visible = false;
            grdAlertList.Columns[8].Visible = false;
            grdAlertList.Columns[9].Visible = false;
            grdAlertList.Columns[10].Visible = false;
            grdAlertList.Columns[11].Visible = false;
            grdAlertList.Columns[12].Visible = false;
            grdAlertList.Columns[13].Visible = false;
            grdAlertList.Columns[14].Visible = false;
            grdAlertList.Columns[15].Visible = false;
            grdAlertList.Columns[16].Visible = false;
            grdAlertList.Columns[19].Visible = false;
            if (lblAlertId.Text == "1")
            {
                grdAlertList.Columns[0].Visible = true;
                grdAlertList.Columns[1].Visible = true;
               
            }
            else if (lblAlertId.Text == "2")
            {
                grdAlertList.Columns[0].Visible = true;
                grdAlertList.Columns[1].Visible = true;
                grdAlertList.Columns[3].Visible = true;
                grdAlertList.Columns[4].Visible = true;
               
            }
            else if (lblAlertId.Text == "3")
            {
                grdAlertList.Columns[0].Visible = true;
                grdAlertList.Columns[1].Visible = true;
                grdAlertList.Columns[3].Visible = true;
                grdAlertList.Columns[4].Visible = true;
                grdAlertList.Columns[5].Visible = true;

            }
            else if (lblAlertId.Text == "4")
            {
                grdAlertList.Columns[0].Visible = true;
                grdAlertList.Columns[1].Visible = true;
                grdAlertList.Columns[2].Visible = true;
            }
            else if (lblAlertId.Text == "5" || lblAlertId.Text == "8")
            {
                grdAlertList.Columns[11].Visible = true;
                grdAlertList.Columns[12].Visible = true;
                grdAlertList.Columns[13].Visible = true;
            }
            else if (lblAlertId.Text == "7" || lblAlertId.Text == "28")
            {
                grdAlertList.Columns[11].Visible = true;
                grdAlertList.Columns[12].Visible = true;
                grdAlertList.Columns[2].Visible = true;
                grdAlertList.Columns[19].Visible = true;
            }
            else if (lblAlertId.Text == "6"|| lblAlertId.Text == "12" || lblAlertId.Text == "13" || lblAlertId.Text == "14")
            {
                grdAlertList.Columns[11].Visible = true;
                grdAlertList.Columns[12].Visible = true;
                grdAlertList.Columns[14].Visible = true;
            }
            else if (lblAlertId.Text == "9")
            {
                grdAlertList.Columns[11].Visible = true;
                grdAlertList.Columns[12].Visible = true;
                grdAlertList.Columns[15].Visible = true;
            }
            else if (lblAlertId.Text == "10")
            {
                grdAlertList.Columns[11].Visible = true;
                grdAlertList.Columns[12].Visible = true;
                grdAlertList.Columns[16].Visible = true;
            }
            else if (lblAlertId.Text == "15" || lblAlertId.Text == "18")
            {
                grdAlertList.Columns[6].Visible = true;
                grdAlertList.Columns[7].Visible = true;
                grdAlertList.Columns[10].Visible = true;
            }
            else if (lblAlertId.Text == "16")
            {
                grdAlertList.Columns[6].Visible = true;
                grdAlertList.Columns[7].Visible = true;
                grdAlertList.Columns[8].Visible = true;
                grdAlertList.Columns[10].Visible = true;
            }
            else if (lblAlertId.Text == "19" || lblAlertId.Text == "20" || lblAlertId.Text == "21" || lblAlertId.Text == "22")
            {
                grdAlertList.Columns[6].Visible = true;
                grdAlertList.Columns[7].Visible = true;
                grdAlertList.Columns[9].Visible = true;
                grdAlertList.Columns[10].Visible = true;
            }
            else if (lblAlertId.Text == "23" || lblAlertId.Text == "24")
            {
                grdAlertList.Columns[0].Visible = true;
                grdAlertList.Columns[1].Visible = true;
                grdAlertList.Columns[3].Visible = true;
                grdAlertList.Columns[4].Visible = true;
            }
            else if (lblAlertId.Text == "25" )
            {
                grdAlertList.Columns[0].Visible = true;
                grdAlertList.Columns[1].Visible = true;
                grdAlertList.Columns[3].Visible = true;
                grdAlertList.Columns[4].Visible = true;
                grdAlertList.Columns[5].Visible = true;
            }
            else if (lblAlertId.Text == "26")
            {
                grdAlertList.Columns[0].Visible = true;
                grdAlertList.Columns[1].Visible = true;
                grdAlertList.Columns[2].Visible = true;
             }
            else if (lblAlertId.Text == "27")
            {
                grdAlertList.Columns[11].Visible = true;
                grdAlertList.Columns[12].Visible = true;
                grdAlertList.Columns[13].Visible = true;
            }
            else if (lblAlertId.Text == "30" || lblAlertId.Text == "31")
            {
                grdAlertList.Columns[11].Visible = true;
                grdAlertList.Columns[12].Visible = true;
                grdAlertList.Columns[15].Visible = true;
            }
            else if (lblAlertId.Text == "32" || lblAlertId.Text == "33" || lblAlertId.Text == "34" || lblAlertId.Text == "35")
            {
                grdAlertList.Columns[11].Visible = true;
                grdAlertList.Columns[12].Visible = true;
                grdAlertList.Columns[14].Visible = true;
            }
            else if (lblAlertId.Text == "36" || lblAlertId.Text == "37")
            {
                grdAlertList.Columns[6].Visible = true;
                grdAlertList.Columns[7].Visible = true;
                grdAlertList.Columns[8].Visible = true;
                grdAlertList.Columns[10].Visible = true;
            }
            else if (lblAlertId.Text == "38")
            {
                grdAlertList.Columns[6].Visible = true;
                grdAlertList.Columns[7].Visible = true;
                grdAlertList.Columns[10].Visible = true;
            }
            else if (lblAlertId.Text == "39" || lblAlertId.Text == "40" || lblAlertId.Text == "41")
            {
                grdAlertList.Columns[6].Visible = true;
                grdAlertList.Columns[7].Visible = true;
                grdAlertList.Columns[9].Visible = true;
                grdAlertList.Columns[10].Visible = true;
            }
            if (!Fromdate.Equals(Todate) && txt_FromDate.Visible==true && txt_ToDate.Visible==true)//if fromdate and todate not equal then process the trigger
            {
               //ProcessTrigger(Status, Fromdate, Todate);
                string[] strDate = txt_FromDate.Text.Split('/');
                DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
                string strFromDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];
                strDate = txt_ToDate.Text.Split('/');
                DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
                string strToDate = strDate[1] + "/" + strDate[0] + "/" + strDate[2];
                
                
                //var rslt = cd.AlertDetailsViewTriggerEscalation(Convert.ToInt32(lblAlertId.Text));
                //grdAlertList.DataSource = rslt;
                //grdAlertList.DataBind();
                if (RdnTrigger.Checked)
                {
                    var rslt = cd.AlertDetailsViewForTrigger(Convert.ToInt32(lblAlertId.Text), strFromDate, strToDate, Status);
                    grdAlertList.DataSource = rslt;
                    grdAlertList.DataBind();
                }
                else
                {
                    var rslt = cd.AlertDetailsViewForEscalation(Convert.ToInt32(lblAlertId.Text), strFromDate, strToDate, Status);
                    grdAlertList.DataSource = rslt;
                    grdAlertList.DataBind();
                }
            }
            else
            {  
                    var rslt = cd.AlertDetailsViewTriggerEscalation(Convert.ToInt32(lblAlertId.Text));
                    grdAlertList.DataSource = rslt;
                    grdAlertList.DataBind();
               
            }
            lbl_RecordsNo.Text = "Total Records : " + grdAlertList.Rows.Count;
            lnkPrint.Focus();
            //TextBox1.Focus();
       }
      
        private System.Net.Mail.MailAddress[] GetEmails(string s)
        {
            List<System.Net.Mail.MailAddress> lst = new List<System.Net.Mail.MailAddress>();
            if (string.IsNullOrEmpty(s))
                return lst.ToArray();

            string[] addrs = s.Split(new char[] { ',' }, StringSplitOptions.None);
            foreach (string email in addrs)
            {
                try
                {
                    if (email != null && email != "")
                    {
                        System.Net.Mail.MailAddress ma = new System.Net.Mail.MailAddress(email);
                        lst.Add(ma);
                    }
                }
                catch (FormatException)
                {
                    lst.Add(default(System.Net.Mail.MailAddress));
                }
            }
            return lst.ToArray();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            int parentId = 1;
            if (RdnEscalation.Checked)
                parentId = 2;

            DataTable dtM = new DataTable();
            dtM.Columns.Add(new DataColumn("Alert_Id", typeof(int)));
            dtM.Columns.Add(new DataColumn("Alert_EmailId_var", typeof(string)));
            dtM.Columns.Add(new DataColumn("AlertMap_ParentLevelId_int", typeof(int)));

            for (int i = 0; i < grdAlert.Rows.Count; i++)
            {
                Label lblAlertId = (Label)grdAlert.Rows[i].FindControl("lblAlertId");
                TextBox txtEmailId = (TextBox)grdAlert.Rows[i].FindControl("txtEmailId");
                string[] strEmail = txtEmailId.Text.ToString().Split(',');
                foreach (string str in strEmail)
                {
                    if (str.Trim() != "")
                    {
                        DataRow dr = dtM.NewRow();
                        dr["Alert_Id"] = Convert.ToInt32(lblAlertId.Text.ToString());
                        dr["Alert_EmailId_var"] = str;
                        dr["AlertMap_ParentLevelId_int"] = parentId;
                        dtM.Rows.InsertAt(dr, dtM.Rows.Count + 1);
                    }
                }
            }
            clsData cls = new clsData();
            if (dtM.Rows.Count > 0)
            {
                cls.InsertAlertMapping(dtM, parentId);
            }
            ClearControls();
            lblMessage.Text = "Updated Successfully";
            lblMessage.Visible = true;

        }

        protected void lnkClear_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        private void ClearControls()
        {
            lblMessage.Visible = false;
            lblErrorMsg.Visible = false;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;

        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdAlertList.Rows.Count > 0)
            {
                PrintGrid.PrintGridView(grdAlertList, lblAlertName.Text, "Alert_Details");
            }
        }
        protected void lnkGenerate_Click(object sender, EventArgs e)
        {
            //if(chkAll.Checked)
            //    ProcessTriggerForFixedPeriod(0);
            //else
            //    ProcessTriggerForFixedPeriod(1);
            //Label lblMsg = (Label)Master.FindControl("lblMsg");
            //lblMsg.Visible = true;
            //lblMsg.Text = "Report generated successfully!!";

        }

        private void ProcessTriggerForFixedPeriod(int status)
        {
            string dtEnq = "", dtProposal = "", dtBill = "";
            DateTime? DtEnq = null, DtProposal = null, DtBill = null;
            if (RdnTrigger.Checked)
            {
                dc.AlertDetails_Update(0, 0, 0, null, 0, "", null, "", null, 0, 0, 0, "", 0, 0, "", "", "", 0, "");//delete all record from alter details table
                var data = dc.AlertDetailsViewFor_TriggerFixedPeriod(Convert.ToBoolean(status)).ToList();
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        DtEnq = null; DtProposal = null; DtBill = null;
                        dtEnq = ""; dtProposal = ""; dtBill = "";
                        //insert record to alter details table
                        if (Convert.ToString(item.ENQ_Date_dt) != "" && Convert.ToString(item.ENQ_Date_dt) != null)
                            dtEnq = Convert.ToDateTime(item.ENQ_Date_dt).ToString("dd/MM/yyyy");

                        if (dtEnq != "")
                            DtEnq = DateTime.ParseExact(dtEnq, "dd/MM/yyyy", null);

                        if (Convert.ToString(item.Proposal_Date) != "" && Convert.ToString(item.Proposal_Date) != null)
                            dtProposal = Convert.ToDateTime(item.Proposal_Date).ToString("dd/MM/yyyy");
                        if (dtProposal != "")
                            DtProposal = DateTime.ParseExact(dtProposal, "dd/MM/yyyy", null);

                        if (Convert.ToString(item.BILL_Date_dt) != "" && Convert.ToString(item.BILL_Date_dt) != null)
                            dtBill = Convert.ToDateTime(item.BILL_Date_dt).ToString("dd/MM/yyyy");
                        if (dtBill != "")
                            DtBill = DateTime.ParseExact(dtBill, "dd/MM/yyyy", null);

                        dc.AlertDetails_Update(1, Convert.ToInt32(item.AlertId), Convert.ToInt32(item.ENQ_Id), DtEnq, Convert.ToInt32(item.Proposal_Id), Convert.ToString(item.Proposal_No), DtProposal, Convert.ToString(item.Bill_Id)
                            , DtBill, Convert.ToDecimal(item.BILL_NetAmt_num), 0, Convert.ToInt32(item.MISRecordNo), Convert.ToString(item.MISRefNo),
                            Convert.ToInt32(item.CL_Id), Convert.ToInt32(item.SITE_Id), Convert.ToString(item.CL_Name_var), Convert.ToString(item.SITE_Name_var), Convert.ToString(item.MATERIAL_Name_var), 0, Convert.ToString(item.MEName));
                    }
                }
            }
            else
            {
                dc.AlertDetails_Update(2, 0, 0, null, 0, "", null, "", null, 0, 0, 0, "", 0, 0, "", "", "", 0, "");//delete all record from alter details table
                var esc = dc.AlertDetailsViewFor_EscalationFixedPeriod(Convert.ToInt32(status)).ToList();
                if (esc.Count > 0)
                {
                    foreach (var item in esc)
                    {
                        dc.AlertDetails_Update(3, Convert.ToInt32(item.AlertId), 0, DtEnq, 0, "", DtProposal, ""
                            , DtBill, 0, 0, 0, "", 0, 0, "", "", "", Convert.ToInt32(item.AlertCount), "");
                    }
                }
            }
        }
      

        protected void lnkMail_Click(object sender, EventArgs e)
        {
            PrintPDFReport obj = new PrintPDFReport();
            var rslt = dc.AlertMapping_View(false);
            foreach (var dt in rslt)
            {
                obj.AlertDetails_PDFReport(dt.AlertMap_EmailId_var, 0);
            }

            // escallation
            var rslt1 = dc.AlertMapping_View(true);
            foreach (var dt in rslt1)
            {
                obj.AlertDetails_PDFReport(dt.AlertMap_EmailId_var, 1);
            }
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = true;
            lblMsg.Text = "Email Sent !!";
        }
        private void ProcessTrigger(int Status, DateTime Fromdate, DateTime Todate)
        {
            string dtEnq = "", dtProposal = "", dtBill = "";
            DateTime? DtEnq = null, DtProposal = null, DtBill = null;
            if (RdnTrigger.Checked)
            {
                dc.AlertDetails_Update(0, 0, 0, null, 0, "", null, "", null, 0, 0, 0, "", 0, 0, "", "", "", 0, "");//delete all record from alter details table
                var data = dc.AlertDetails_View_For_Trigger(Fromdate, Todate, Convert.ToBoolean(Status)).ToList();
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        DtEnq = null; DtProposal = null; DtBill = null;
                        dtEnq = ""; dtProposal = ""; dtBill = "";
                        //insert record to alter details table
                        if (Convert.ToString(item.ENQ_Date_dt) != "" && Convert.ToString(item.ENQ_Date_dt) != null)
                            dtEnq = Convert.ToDateTime(item.ENQ_Date_dt).ToString("dd/MM/yyyy");

                        if (dtEnq != "")
                            DtEnq = DateTime.ParseExact(dtEnq, "dd/MM/yyyy", null);

                        if (Convert.ToString(item.Proposal_Date) != "" && Convert.ToString(item.Proposal_Date) != null)
                            dtProposal = Convert.ToDateTime(item.Proposal_Date).ToString("dd/MM/yyyy");
                        if (dtProposal != "")
                            DtProposal = DateTime.ParseExact(dtProposal, "dd/MM/yyyy", null);

                        if (Convert.ToString(item.BILL_Date_dt) != "" && Convert.ToString(item.BILL_Date_dt) != null)
                            dtBill = Convert.ToDateTime(item.BILL_Date_dt).ToString("dd/MM/yyyy");
                        if (dtBill != "")
                            DtBill = DateTime.ParseExact(dtBill, "dd/MM/yyyy", null);

                        dc.AlertDetails_Update(1, Convert.ToInt32(item.AlertId), Convert.ToInt32(item.ENQ_Id), DtEnq, Convert.ToInt32(item.Proposal_Id), Convert.ToString(item.Proposal_No), DtProposal, Convert.ToString(item.Bill_Id)
                            , DtBill, Convert.ToDecimal(item.BILL_NetAmt_num), 0, Convert.ToInt32(item.MISRecordNo), Convert.ToString(item.MISRefNo),
                            Convert.ToInt32(item.CL_Id), Convert.ToInt32(item.SITE_Id), Convert.ToString(item.CL_Name_var), Convert.ToString(item.SITE_Name_var), Convert.ToString(item.MATERIAL_Name_var), 0, Convert.ToString(item.MEName));
                    }
                }
            }
            else
            {
                dc.AlertDetails_Update(2, 0, 0, null, 0, "", null, "", null, 0, 0, 0, "", 0, 0, "", "", "", 0, "");//delete all record from alert details table
                var esc = dc.AlertDetails_View_For_Escalation(Fromdate, Todate, Convert.ToBoolean(Status)).ToList();
                if (esc.Count > 0)
                {
                    foreach (var item in esc)
                    {
                        DtEnq = null; DtProposal = null; DtBill = null;
                        dtEnq = ""; dtProposal = ""; dtBill = "";
                        //insert record to alter details table
                        //    if (Convert.ToString(item.ENQ_Date_dt) != "" && Convert.ToString(item.ENQ_Date_dt) != null)
                        //        dtEnq = Convert.ToDateTime(item.ENQ_Date_dt).ToString("dd/MM/yyyy");

                        //    if (dtEnq != "")
                        //        DtEnq = DateTime.ParseExact(dtEnq, "dd/MM/yyyy", null);

                        //    if (Convert.ToString(item.Proposal_Date) != "" && Convert.ToString(item.Proposal_Date) != null)
                        //        dtProposal = Convert.ToDateTime(item.Proposal_Date).ToString("dd/MM/yyyy");
                        //    if (dtProposal != "")
                        //        DtProposal = DateTime.ParseExact(dtProposal, "dd/MM/yyyy", null);

                        //    if (Convert.ToString(item.BILL_Date_dt) != "" && Convert.ToString(item.BILL_Date_dt) != null)
                        //        dtBill = Convert.ToDateTime(item.BILL_Date_dt).ToString("dd/MM/yyyy");
                        //    if (dtBill != "")
                        //        DtBill = DateTime.ParseExact(dtBill, "dd/MM/yyyy", null);

                        //    dc.AlertDetails_Update(1, Convert.ToInt32(item.AlertId), Convert.ToInt32(item.ENQ_Id), DtEnq, Convert.ToInt32(item.Proposal_Id), Convert.ToString(item.Proposal_No), DtProposal, Convert.ToString(item.Bill_Id)
                        //        , DtBill, Convert.ToDecimal(item.BILL_NetAmt_num), 0, Convert.ToInt32(item.MISRecordNo), Convert.ToString(item.MISRefNo),
                        //        Convert.ToInt32(item.CL_Id), Convert.ToInt32(item.SITE_Id), Convert.ToString(item.CL_Name_var), Convert.ToString(item.SITE_Name_var), Convert.ToString(item.MATERIAL_Name_var), 0, Convert.ToString(item.MEName));


                        //    dc.AlertDetails_Update(1, Convert.ToInt32(item.AlertId), Convert.ToInt32(item.ENQ_Id), DtEnq, Convert.ToInt32(item.Proposal_Id), Convert.ToString(item.Proposal_No), DtProposal, Convert.ToString(item.Bill_Id)
                        //, DtBill, Convert.ToDecimal(item.BILL_NetAmt_num), 0, Convert.ToInt32(item.MISRecordNo), Convert.ToString(item.MISRefNo),
                        //Convert.ToInt32(item.CL_Id), Convert.ToInt32(item.SITE_Id), Convert.ToString(item.CL_Name_var), Convert.ToString(item.SITE_Name_var), Convert.ToString(item.MATERIAL_Name_var), 0, Convert.ToString(item.MEName));

                        dc.AlertDetails_Update(3, Convert.ToInt32(item.AlertId), 0, DtEnq, 0, "", DtProposal, ""
                            , DtBill, 0, 0, 0, "", 0, 0, "", "", "", Convert.ToInt32(item.AlertCount), "");
                    }


                }

            }
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
            {
                lblFrm.Visible = true;
                lblTo.Visible = true;
                txt_FromDate.Visible = true;
                txt_ToDate.Visible = true;
            }
        }

        protected void chkPending_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPending.Checked)
            {
                lblFrm.Visible = false;
                lblTo.Visible = false;
                txt_FromDate.Visible = false;
                txt_ToDate.Visible = false;
            }
        }

        
    
    }

}