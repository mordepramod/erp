using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;

namespace DESPLWEB
{
    public partial class ProposalList : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Proposal list";
                //LoadEnquiryList();
                txt_FromDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        private void LoadEnquiryList(int showAllStatus)
        {

            BindgrdRowOfEnquiry(showAllStatus);
            if (grdProposal.Rows.Count <= 0)
            {
                //FirstGridViewRowOfEnquiry();
            }
            else
            {
                lblTotalRecords.Text = "Total No of Records : " + grdProposal.Rows.Count;

                if (rbProposalPending.Checked)
                {
                    grdProposal.Columns[0].Visible = true;
                    grdProposal.Columns[1].Visible = false;
                    grdProposal.Columns[2].Visible = false;
                    grdProposal.Columns[3].Visible = false;
                    grdProposal.Columns[4].Visible = false;
                    grdProposal.Columns[5].Visible = false;

                    grdProposal.Columns[7].Visible = false;
                    grdProposal.Columns[8].Visible = true;
                    grdProposal.Columns[9].Visible = true;
                    grdProposal.Columns[10].Visible = false;
                    grdProposal.Columns[11].Visible = false;
                    grdProposal.Columns[15].Visible = true;
                    grdProposal.Columns[16].Visible = false;
                    grdProposal.Columns[17].Visible = false;
                    grdProposal.Columns[18].Visible = false;
                    grdProposal.Columns[19].Visible = false;
                    grdProposal.Columns[20].Visible = false;
                }
                else if (rbProposalComplete.Checked || rbProposalPendingForApproval.Checked)
                {
                    grdProposal.Columns[0].Visible = true;
                    grdProposal.Columns[1].Visible = true;
                    grdProposal.Columns[2].Visible = true;
                    grdProposal.Columns[3].Visible = true;
                    grdProposal.Columns[4].Visible = true;
                    grdProposal.Columns[5].Visible = true;

                    grdProposal.Columns[7].Visible = true;
                    grdProposal.Columns[8].Visible = false;
                    grdProposal.Columns[9].Visible = false;
                    grdProposal.Columns[10].Visible = true;
                    grdProposal.Columns[11].Visible = true;
                    grdProposal.Columns[15].Visible = false;
                    grdProposal.Columns[16].Visible = true;
                    grdProposal.Columns[17].Visible = true;
                    grdProposal.Columns[18].Visible = true;
                    grdProposal.Columns[19].Visible = true;
                    grdProposal.Columns[20].Visible = true;
                }
                else if (rbProposalApp.Checked)
                {
                    grdProposal.Columns[0].Visible = false;
                    grdProposal.Columns[1].Visible = false;
                    grdProposal.Columns[2].Visible = true;
                    grdProposal.Columns[3].Visible = false;
                    grdProposal.Columns[4].Visible = false;
                    grdProposal.Columns[5].Visible = false;

                    grdProposal.Columns[7].Visible = true;
                    grdProposal.Columns[8].Visible = false;
                    grdProposal.Columns[9].Visible = false;
                    grdProposal.Columns[10].Visible = true;
                    grdProposal.Columns[11].Visible = false;
                    grdProposal.Columns[15].Visible = false;
                    grdProposal.Columns[16].Visible = true;
                    grdProposal.Columns[17].Visible = true;
                    grdProposal.Columns[18].Visible = true;
                    grdProposal.Columns[19].Visible = false;
                    grdProposal.Columns[20].Visible = false;
                }
            }
        }
        private void BindgrdRowOfEnquiry(int showAllStatus)
        {
            int status = 0, ClientId = 0;
            string inwdType = "", clName = "";

            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("Proposal_No", typeof(string)));
            dt.Columns.Add(new DataColumn("Proposal_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("Proposal_Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Proposal_NetAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("MATERIAL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_OpenEnquiryStatus_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQNewClientStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_EmailID_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_ContactNo_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_Status_tint", typeof(string)));
            dt.Columns.Add(new DataColumn("Proposal_EmailToCc_var", typeof(string)));
            dt.Columns.Add(new DataColumn("PROINV_Id", typeof(string)));
            //var enq = dc.Enquiry_View(0, 1, 0);
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
            Label lblMsg = (Label)Master.FindControl("lblMsg");

            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "" && lblClientId.Text != "0")
                    ClientId = Convert.ToInt32(lblClientId.Text);
                else //if new proposal client
                    ClientId = -1;

            }
            if (ddl_InwardTestType.SelectedIndex > 0)
                inwdType = ddl_InwardTestType.SelectedItem.Text;

            if (rbProposalPending.Checked)
            {
                status = 3;
            }
            if (rbProposalComplete.Checked || rbProposalPendingForApproval.Checked)
            {
                status = showAllStatus;
            }
            else if (rbProposalApp.Checked)
            {
                status = 2; 
                inwdType = ""; 
                ClientId = 0;
            }

            int rowNo = 0, statusOfEnq = 0;
           // inwdType = "STCOUPON";
            var enq = dc.Enquiry_View_ForProposal(Fromdate, Todate, status, ClientId, inwdType, rbProposalPendingForApproval.Checked);
            foreach (var c in enq)
            {
                dr = dt.NewRow();
                dr["ENQ_Id"] = c.ENQ_Id.ToString();
                dr["CL_Id"] = c.ENQ_CL_Id.ToString();
                dr["SITE_Id"] = c.ENQ_SITE_Id.ToString();
                dr["CONT_Id"] = c.ENQ_CONT_Id.ToString();
                dr["ENQ_Date_dt"] = Convert.ToDateTime(c.ENQ_Date_dt).ToString("dd/MM/yyyy");

                if (status == 2)
                {
                    var clSiteResult = dc.EnquiryApp_View(Convert.ToInt32(c.ENQ_Id.ToString()), -1);
                    foreach (var item in clSiteResult)
                    {
                        dr["CL_Id"] = item.ENQ_CL_Id.ToString();
                        dr["SITE_Id"] = item.ENQ_SITE_Id.ToString();
                        dr["CL_Name_var"] = Convert.ToString(item.CL_Name_var);
                        dr["SITE_Name_var"] = Convert.ToString(item.SITE_Name_var);
                    }

                    var result = dc.EnquiryApp_Material_View(Convert.ToInt32(c.ENQ_Id.ToString()));
                    string materialNm = "";
                    foreach (var item in result)
                    {
                        materialNm += Convert.ToString(item.MATERIAL_Name_var) + ",";

                    }
                    dr["MATERIAL_Name_var"] = Convert.ToString(materialNm.TrimEnd(','));

                }
                else
                    dr["MATERIAL_Name_var"] = Convert.ToString(c.MATERIAL_Name_var);
                if (status != 3)//completed
                {
                    dr["Proposal_Id"] = c.Proposal_Id.ToString();
                    dr["Proposal_No"] = c.Proposal_No.ToString();
                    dr["Proposal_Date"] = Convert.ToDateTime(c.Proposal_Date).ToString("dd/MM/yyyy");
                    dr["Proposal_NetAmount"] = c.Proposal_NetAmount.ToString();

                    dr["SITE_EmailID_var"] = Convert.ToString(c.SITE_EmailID_var);

                    if (status == 2)
                    {
                        dr["CONT_Name_var"] = c.ENQ_contact_person;
                        dr["CONT_ContactNo_var"] = c.ENQ_contact_number;
                    }
                    else
                    {
                        dr["CONT_Name_var"] = c.CONT_Name_var;
                        dr["CONT_ContactNo_var"] = c.CONT_ContactNo_var;
                    }
                    if (Convert.ToString(c.ENQ_Status_tint) != "")
                    {
                        statusOfEnq = Convert.ToInt32(c.ENQ_Status_tint);
                    }
                    if (statusOfEnq == 2)
                        dr["ENQ_Status_tint"] = "Close";
                    else
                        dr["ENQ_Status_tint"] = "Pending";

                    if (status == 2)
                    {
                        dr["Proposal_EmailToCc_var"] = Convert.ToString(c.ENQ_contact_emailid);
                    }
                    else
                    {
                        if (Convert.ToString(c.Proposal_EmailToCc_var) != "")
                            dr["Proposal_EmailToCc_var"] = Convert.ToString(c.Proposal_EmailToCc_var);
                        else
                            dr["Proposal_EmailToCc_var"] = "";
                    }

                }
                else
                {
                    dr["Proposal_Id"] = "0";
                    dr["Proposal_No"] = string.Empty;
                    dr["Proposal_Date"] = Convert.ToDateTime(c.Proposal_Date).ToString("dd/MM/yyyy");
                    dr["Proposal_NetAmount"] = string.Empty;
                    dr["SITE_EmailID_var"] = string.Empty;
                    dr["CONT_Name_var"] = string.Empty;
                    dr["CONT_ContactNo_var"] = string.Empty;
                    dr["Proposal_EmailToCc_var"] = string.Empty;
                    dr["ENQ_Status_tint"] = string.Empty;
                }
                if (status != 2)
                {
                    dr["CL_Name_var"] = Convert.ToString(c.CL_Name_var);
                    dr["SITE_Name_var"] = Convert.ToString(c.SITE_Name_var);
                }
                if (status == 2)
                    dr["ENQ_OpenEnquiryStatus_var"] = Convert.ToString(c.ENQ_OpenEnquiryStatus_var1);
                else
                    dr["ENQ_OpenEnquiryStatus_var"] = Convert.ToString(c.ENQ_OpenEnquiryStatus_var);
                dr["ENQNewClientStatus"] = "False";

                if (c.PROINV_Id != null)
                    dr["PROINV_Id"] = c.PROINV_Id.ToString();
                else
                    dr["PROINV_Id"] = "";

                dt.Rows.Add(dr);
                rowNo++;
            }

            if (chkClientSpecific.Checked == true)
            {
                if (ClientId == 0 || ClientId == -1)
                {
                    if (txt_Client.Text != "")
                    {
                        clName = txt_Client.Text;
                        lblMsg.Visible = false;

                    }
                }
                else
                    clName = txt_Client.Text;
            }

            if (status != 2)
            {
                var enqNew = dc.EnquiryNewClient_View_ForProposal(Fromdate, Todate, status, clName, inwdType, rbProposalPendingForApproval.Checked);
                foreach (var c in enqNew)
                {
                    dr = dt.NewRow();
                    dr["ENQ_Id"] = c.ENQNEW_Id.ToString();
                    dr["CL_Id"] = "0";
                    dr["SITE_Id"] = "0";
                    dr["CONT_Id"] = "0";
                    dr["ENQ_Date_dt"] = Convert.ToDateTime(c.ENQNEW_Date_dt).ToString("dd/MM/yyyy");
                    dr["MATERIAL_Name_var"] = Convert.ToString(c.MATERIAL_Name_var);

                    dr["Proposal_Id"] = c.Proposal_Id.ToString();
                    if (c.Proposal_No != null)
                        dr["Proposal_No"] = c.Proposal_No.ToString();
                    else
                        dr["Proposal_No"] = "";
                    dr["Proposal_Date"] = Convert.ToDateTime(c.Proposal_Date).ToString("dd/MM/yyyy");
                    dr["Proposal_NetAmount"] = c.Proposal_NetAmount.ToString();
                    dr["CONT_Name_var"] = c.ENQNEW_ContactPersonName_var;
                    dr["CONT_ContactNo_var"] = c.ENQNEW_ContactNo_var;
                    dr["ENQ_Status_tint"] = string.Empty;
                    if (Convert.ToString(c.ENQNEW_Status_tint) != "")
                    {
                        statusOfEnq = Convert.ToInt32(c.ENQNEW_Status_tint);
                    }
                    if (statusOfEnq == 2)
                        dr["ENQ_Status_tint"] = "Close";
                    else
                        dr["ENQ_Status_tint"] = "Pending";

                    if (Convert.ToString(c.Proposal_NewClientMailId_var) != "")
                        dr["SITE_EmailID_var"] = Convert.ToString(c.Proposal_NewClientMailId_var);

                    if (Convert.ToString(c.Proposal_EmailToCc_var) != "")
                        dr["Proposal_EmailToCc_var"] = Convert.ToString(c.Proposal_EmailToCc_var);
                    else
                        dr["Proposal_EmailToCc_var"] = "";

                    dr["CL_Name_var"] = c.ENQNEW_ClientName_var;
                    dr["SITE_Name_var"] = c.ENQNEW_SiteName_var;
                    dr["ENQ_OpenEnquiryStatus_var"] = c.ENQNEW_OpenEnquiryStatus_var;
                    dr["ENQNewClientStatus"] = "True";
                    if (c.PROINV_Id != null)
                        dr["PROINV_Id"] = c.PROINV_Id.ToString();
                    else
                        dr["PROINV_Id"] = "";
                    dt.Rows.Add(dr);
                }
            }
            grdProposal.DataSource = dt;
            grdProposal.DataBind();
            if (status == 2)
            {
                rowNo = 0;
            }
            for (int i = rowNo; i < grdProposal.Rows.Count; i++)
            {
                for (int j = 0; j < grdProposal.Columns.Count; j++)
                {
                    grdProposal.Rows[i].Cells[j].BackColor = System.Drawing.Color.LightGray;
                }
            }
            bool proposalRight = false;
            var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
            foreach (var u in user)
            {
                if (u.USER_Proposal_right_bit == true || u.USER_Mkt_right_bit == true)
                    proposalRight = true;
            }
            if (grdProposal.Rows.Count > 0)
            {
                for (int i = 0; i < grdProposal.Rows.Count; i++)
                {
                    LinkButton lnkProposal = (LinkButton)grdProposal.Rows[i].FindControl("lnkProposal");
                    LinkButton lnkProposalEmail = (LinkButton)grdProposal.Rows[i].FindControl("lnkProposalEmail");
                    LinkButton lnkGenerateProforma = (LinkButton)grdProposal.Rows[i].FindControl("lnkGenerateProforma");
                    lnkProposal.Enabled = false;
                    if (status == 1)
                    {
                        lnkProposal.Text = "Modify";
                        if (proposalRight == true)
                            lnkProposal.Enabled = true;
                    }
                    else
                    {
                        lnkProposal.Text = "New";
                    }
                    if (rbProposalPendingForApproval.Checked)                    
                        lnkProposalEmail.Enabled = false;

                    if (rbProposalComplete.Checked)
                        lnkGenerateProforma.Enabled = true;
                    else
                        lnkGenerateProforma.Enabled = false;
                }
            }
        }

        private void FirstGridViewRowOfEnquiry()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("Proposal_No", typeof(string)));
            dt.Columns.Add(new DataColumn("Proposal_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_Date_dt", typeof(string)));
            dt.Columns.Add(new DataColumn("Proposal_Date", typeof(string)));
            dt.Columns.Add(new DataColumn("Proposal_NetAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("MATERIAL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_OpenEnquiryStatus_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQNewClientStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_EmailID_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CONT_ContactNo_var", typeof(string)));
            dt.Columns.Add(new DataColumn("ENQ_Status_tint", typeof(string)));
            dt.Columns.Add(new DataColumn("PROINV_Id", typeof(string)));

            dr = dt.NewRow();
            dr["Proposal_No"] = string.Empty;
            dr["Proposal_Id"] = "0";
            dr["ENQ_Id"] = string.Empty;
            dr["CL_Id"] = string.Empty;
            dr["SITE_Id"] = string.Empty;
            dr["CONT_Id"] = string.Empty;
            dr["ENQ_Date_dt"] = string.Empty;
            dr["Proposal_Date"] = string.Empty;
            dr["Proposal_NetAmount"] = string.Empty;
            dr["MATERIAL_Name_var"] = string.Empty;
            dr["CL_Name_var"] = string.Empty;
            dr["SITE_Name_var"] = string.Empty;
            dr["ENQ_OpenEnquiryStatus_var"] = string.Empty;
            dr["ENQNewClientStatus"] = string.Empty;
            dr["SITE_EmailID_var"] = string.Empty;
            dr["CONT_Name_var"] = string.Empty;
            dr["CONT_ContactNo_var"] = string.Empty;
            dr["ENQ_Status_tint"] = string.Empty;
            dr["PROINV_Id"] = string.Empty;
            dt.Rows.Add(dr);
            grdProposal.DataSource = dt;
            grdProposal.DataBind();

        }

        protected void lnkProposal_OnCommand(object sender, CommandEventArgs e)
        {
            //int flagModify=0;
            string strURLWithData = "";
            string[] arg = new string[2];
            arg = e.CommandArgument.ToString().Split(';');
            if (Convert.ToString(arg[0]) != "")
            {
                //string SelectedInward = arg[1];
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                if (rbProposalPending.Checked)                    //flagModify = 1;
                {
                    //   strURLWithData = "Proposal.aspx?" + obj.Encrypt(string.Format("ENQ_Id={0}&ModifyType={1}&Proposal_Id={2}&EnqNewClient={3}", arg[0], "0", arg[2], arg[1]));//0 means new proposal
                }
                else
                    strURLWithData = "Proposal.aspx?" + obj.Encrypt(string.Format("ENQ_Id={0}&ModifyType={1}&Proposal_Id={2}&EnqNewClient={3}", arg[0], "2", arg[2], arg[1]));//2 means modify proposal

                //if (rbProposalPending.Checked)
                //    flagModify = 1;
                //string strURLWithData = "Proposal.aspx?" + obj.Encrypt(string.Format("ENQ_Id={0}&ModifyType={1}&Status={2}&EnqNewClient={3}", arg[0], "0", flagModify, arg[2]));

                Response.Redirect(strURLWithData);
            }
        }
        protected void lnkProposalRev_OnCommand(object sender, CommandEventArgs e)
        {
            string[] arg = new string[2];
            arg = e.CommandArgument.ToString().Split(';');
            if (Convert.ToString(arg[0]) != "")
            {
                //string SelectedInward = arg[1];
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                string strURLWithData = "Proposal.aspx?" + obj.Encrypt(string.Format("ENQ_Id={0}&ModifyType={1}&Proposal_Id={2}&EnqNewClient={3}", arg[0], "1", arg[2], arg[1]));//1 means revise proposal
                                                                                                                                                                                 //  string strURLWithData = "Proposal.aspx?" + obj.Encrypt(string.Format("ENQ_Id={0}&ModifyType={1}&Status={2}&EnqNewClient={3}", arg[0], "1", 0, arg[2]));
                Response.Redirect(strURLWithData);
            }
        }

        protected void lnkPrint_OnCommand(object sender, CommandEventArgs e)
        {
            //string enqNo = e.CommandArgument.ToString();
            string[] arg = new string[2];
            arg = e.CommandArgument.ToString().Split(';');
            string enqNo = arg[0];
            if (enqNo != "")
            {
                //var res = dc.Proposal_View(Convert.ToInt32(enqNo), 0, Convert.ToBoolean(arg[1]), Convert.ToInt32(arg[2])).ToList();
                //if (res.Count > 0)
                //{

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;
                Label lblProposalNo = (Label)grdProposal.Rows[index].FindControl("lblProposalNo");

                if (arg[2].ToString() != "0")
                {
                    PrintPDFReport rpt = new PrintPDFReport();

                    if (rbProposalApp.Checked)
                    {
                        bool notRegClient = false;
                        var result = dc.EnquiryApp_View(Convert.ToInt32(enqNo), -1);
                        foreach (var item in result)
                        {
                            if (item.ENQ_CONT_Id == 0)
                                notRegClient = true;
                        }

                        rpt.ProposalApp_PDF(Convert.ToInt32(enqNo), 1, Convert.ToInt32(arg[2]), false, true, "View", lblProposalNo.Text, notRegClient);
                    }
                    else
                        rpt.Proposal_PDF(Convert.ToInt32(enqNo), 0, Convert.ToInt32(arg[2]), Convert.ToBoolean(arg[1]), "View", lblProposalNo.Text);
                }
                //}
            }

        }
        protected void lnkProposalEmail_OnCommand(object sender, CommandEventArgs e)
        {
            //string enqNo = e.CommandArgument.ToString();
            string[] arg = new string[3];
            arg = e.CommandArgument.ToString().Split(';');
            string enqNo = arg[0];
            if (enqNo != "")
            {
                bool sendMail = true;
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;
                Label lblEnquiryId = (Label)grdProposal.Rows[index].FindControl("lblEnquiryId");
                Label lblEnqNewClient = (Label)grdProposal.Rows[index].FindControl("lblEnqNewClient");
                Label lblProposalNo = (Label)grdProposal.Rows[index].FindControl("lblProposalNo");
                TextBox txtEmailIdTo = (TextBox)grdProposal.Rows[index].FindControl("txtEmailIdTo");
                TextBox txtEmailIdCc = (TextBox)grdProposal.Rows[index].FindControl("txtEmailIdCc");
                Label lblMaterialName = (Label)grdProposal.Rows[index].FindControl("lblMaterialName");
                if (txtEmailIdTo.Text == "")
                {
                    Label lblMsg = (Label)Master.FindControl("lblMsg");
                    lblMsg.Text = "Please enter email id.";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Visible = true;
                }
                else
                {
                    if (txtEmailIdTo.Text.Trim() == "" || txtEmailIdTo.Text.Trim().ToLower() == "na@unknown.com" ||
                        txtEmailIdTo.Text.Trim().ToLower() == "na" || txtEmailIdTo.Text.Trim().ToLower().Contains("na@") == true ||
                        txtEmailIdTo.Text.Trim().ToLower().Contains("@") == false || txtEmailIdTo.Text.Trim().ToLower().Contains(".") == false)
                    {
                        sendMail = false;
                    }
                    if (IsValidEmailAddress(txtEmailIdTo.Text.Trim()) == false)
                    {
                        sendMail = false;
                        Label lblMsg = (Label)Master.FindControl("lblMsg");
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Text = "Invalid email id.";
                        lblMsg.Visible = true;
                    }
                    if (sendMail == true)
                    {
                        int proposalId = Convert.ToInt32(arg[2]);
                        PrintPDFReport rpt = new PrintPDFReport();
                        rpt.Proposal_PDF(Convert.ToInt32(lblEnquiryId.Text), 0, proposalId, Convert.ToBoolean(arg[1]), "Email", lblProposalNo.Text);
                        string reportPath = "C:/temp/Veena/" + "Proposal_" + lblProposalNo.Text.Replace("/", "_") + ".pdf";
                        if (File.Exists(@reportPath))
                        {
                            string mTo = "", mCC = "", mSubject = "", mbody = "", mReplyTo = "";                             
                            mTo = txtEmailIdTo.Text.Trim();                            
                            if (txtEmailIdCc.Text != "")
                                mCC = txtEmailIdCc.Text.Trim();
                            //mTo = "shital.bandal@gmail.com";
                            //mCC = "";
                            mSubject = "Proposal for " + lblMaterialName.Text;
                            mSubject = mSubject + " : Proposal No. " + lblProposalNo.Text;
                            mbody = "Dear Customer,<br><br>";
                            mbody = mbody + "We are pleased to submit our proposal based on our discussion. <br/>You are requested to approve the proposal by work order/ confirmatory email from your <br/>registered company email id for further action.";
                            if (lblEnqNewClient.Text == "True")
                            {
                                mbody = mbody + "<br/><br/>Also find attached KYC data sheet for your project. <br/>We request you to fill the same and send us by email along with confirmatory mail.";
                                string kycSheet = "";
                                string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                                if (cnStr.ToLower().Contains("mumbai") == true)
                                    kycSheet = "DurocreteKYCDetailsSheet_Mumbai.xls";
                                else if (cnStr.ToLower().Contains("nashik") == true)
                                    kycSheet = "DurocreteKYCDetailsSheet_Nashik.xls";
                                else
                                    kycSheet = "DurocreteKYCDetailsSheet_Pune.xls";
                                reportPath += "," + System.Web.HttpContext.Current.Server.MapPath(".") + "/Images/" + kycSheet;
                            }
                            //mbody = mbody + "<br/>";
                            //mbody = mbody + "<br/>";
                            //mbody = mbody + "Following are commercial details ";
                            //mbody = mbody + "<br/>";
                            //mbody = mbody + getProposalDetailHtml();
                            mbody = mbody + "<br/>";
                            mbody = mbody + "<br/>";
                            mbody = mbody + "Detail terms and conditions has attached within a proposal.";
                            mbody = mbody + "<br/>";
                            mbody = mbody + "<br/>";
                            //mbody = mbody + "Please approve the proposal for further action.<br/>Please send approval email to us from your registered email Id." + " <br><br><br>";
                            mbody = mbody + "<br/>";
                            mbody = mbody + "<br/>";
                            mbody = mbody + "Best Regards,";
                            mbody = mbody + "<br/>";
                            mbody = mbody + "<br/>";
                            mbody = mbody + "DUROCRETE ENGINEERING SERVICES PVT. LTD.";

                            clsSendMail objMail = new clsSendMail();
                            objMail.SendMailProposal(mTo, mCC, mSubject, mbody, reportPath, mReplyTo);

                            if (lblEnqNewClient.Text == "True")
                                dc.Proposal_Update_Email(Convert.ToInt32(proposalId), mTo, mCC, false);
                        }
                        Label lblMsg = (Label)Master.FindControl("lblMsg");
                        lblMsg.Text = "Email Send Successfully.";
                        lblMsg.ForeColor = System.Drawing.Color.Green;
                        lblMsg.Visible = true;
                    }
                }

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
        protected void lnkOrderRec_OnCommand(object sender, CommandEventArgs e)
        {
            string arg = e.CommandArgument.ToString();
            if (Convert.ToString(arg) != "0")
            {
                txtOrderDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                txtOrderNo.Text = "";
                txtOrderValue.Text = "";
                lblMessage.Visible = false;
                ModalPopupExtender1.Show();
                lblProposalId.Text = arg;
                if (lblProposalId.Text != "0")
                {
                    var reslt = dc.ProposalOrder_Update(Convert.ToInt32(lblProposalId.Text), "", null, 0, true, false).ToList();
                    if (reslt.Count > 0)
                    {
                        foreach (var item in reslt)
                        {
                            if (Convert.ToString(item.Proposal_OrderDate_dt) != null && Convert.ToString(item.Proposal_OrderDate_dt) != "")
                                txtOrderDate.Text = Convert.ToDateTime(item.Proposal_OrderDate_dt).ToString("dd/MM/yyyy");
                            txtOrderNo.Text = Convert.ToString(item.Proposal_OrderNo_var);
                            txtOrderValue.Text = Convert.ToString(item.Proposal_OrderValue_dec);
                        }
                    }
                }

            }
        }
        protected void lnkAddOrderRec_Click(object sender, EventArgs e)
        {
            bool flag = true;
            lblMessage.Visible = false;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            if (txtOrderNo.Text == "")
            {
                flag = false;
                txtOrderNo.Focus();
                lblMessage.Visible = true;
                lblMessage.Text = "Input Order No";
            }
            else if (txtOrderDate.Text == "")
            {
                flag = false;
                txtOrderDate.Focus();
                lblMessage.Visible = true;
                lblMessage.Text = "Input Order Date";
            }
            else if (txtOrderValue.Text == "")
            {
                flag = false;
                txtOrderValue.Focus();
                lblMessage.Visible = true;
                lblMessage.Text = "Input Order Value";
            }
            else if (Convert.ToDouble(txtOrderValue.Text) == 0)
            {
                flag = false;
                txtOrderValue.Focus();
                lblMessage.Visible = true;
                lblMessage.Text = "Order Value should be grater than 0";
            }


            if (flag)
            {
                DateTime orderDt = DateTime.ParseExact(txtOrderDate.Text, "dd/MM/yyyy", null);
                if (lblProposalId.Text != "0")
                    dc.ProposalOrder_Update(Convert.ToInt32(lblProposalId.Text), txtOrderNo.Text, orderDt, Convert.ToDecimal(txtOrderValue.Text), true, true);
                lblMessage.Visible = true;
                lblMessage.Text = "Updated Successfully..";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }

        }
        protected void lnkGenerateProforma_OnCommand(object sender, CommandEventArgs e)
        {
            string[] arg = new string[2];
            arg = e.CommandArgument.ToString().Split(';');
            if (Convert.ToString(arg[0]) != "")
            {
                string EnqId = arg[0];
                string NewClientStatus = arg[1];
                string ProposalNo = arg[2];
                string ProformaInvoiceNo = arg[3];
                
                ProformaInvoiceUpdation proformaInvoice = new ProformaInvoiceUpdation();
                proformaInvoice.UpdateProformaInvoice(EnqId, ProposalNo, NewClientStatus, ProformaInvoiceNo);
                ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Proforna invoice updated successfully !');", true);
                if (chkShowAll.Checked)
                    LoadEnquiryList(0);
                else
                    LoadEnquiryList(1);
            }
        }

        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (chkShowAll.Checked)
                LoadEnquiryList(0);
            else
                LoadEnquiryList(1);

        }
        protected void imgExit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
        }

        protected void rbProposalComplete_CheckedChanged(object sender, EventArgs e)
        {
            if (rbProposalComplete.Checked)
            {
                LoadInwardType();
                reset();
                lblInwdType.Visible = true;
                ddl_InwardTestType.Visible = true;
                chkClientSpecific.Visible = true;
                chkShowAll.Visible = true;
                chkShowAll.Checked = false;
                txt_Client.Visible = true;
                lnkPrint.Visible = true;
            }
        }
        protected void rbProposalPendingForApproval_CheckedChanged(object sender, EventArgs e)
        {
            if (rbProposalPendingForApproval.Checked)
            {
                LoadInwardType();
                reset();
                lblInwdType.Visible = true;
                ddl_InwardTestType.Visible = true;
                chkClientSpecific.Visible = true;
                chkShowAll.Visible = true;
                chkShowAll.Checked = false;
                txt_Client.Visible = true;
                lnkPrint.Visible = true;
            }
        }
        
        protected void rbProposalPending_CheckedChanged(object sender, EventArgs e)
        {
            if (rbProposalPending.Checked)
            {
                reset();
                lblInwdType.Visible = false;
                ddl_InwardTestType.Visible = false;
                chkClientSpecific.Visible = false;
                chkShowAll.Visible = false;
                txt_Client.Visible = false;
                lnkPrint.Visible = false;
            }
        }

        protected void rdProposalApp_CheckedChanged(object sender, EventArgs e)
        {
            if (rbProposalApp.Checked)
            {
                reset();
                lblInwdType.Visible = false;
                ddl_InwardTestType.Visible = false;
                chkClientSpecific.Visible = false;
                chkShowAll.Visible = false;
                txt_Client.Visible = false;
                lnkPrint.Visible = false;
            }
        }

        private void LoadInwardType()
        {
            var inwd = dc.Material_View("", "");
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardTestType.DataValueField = "MATERIAL_Id";
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, "---Select All---");
        }

        private void reset()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            grdProposal.DataSource = null;
            grdProposal.DataBind();
            lblTotalRecords.Text = "Total No of Records : 0 ";
            //txt_FromDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            //txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }
        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    lblClientId.Text = Request.Form[hfClientId.UniqueID];
                }
                else
                {
                    lblClientId.Text = "0";
                }
            }
            else
                lblClientId.Text = "0";
        }
        protected Boolean ChkClientName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Client.Text;
            Boolean valid = false;
            var query = dc.Client_View(0, 0, searchHead, "");
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Client.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Client is not in the list";
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetClientname(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Client_View(0, -1, searchHead, "");
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("CL_Name_var");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                var clnm = db.Client_View(0, 0, "", "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> CL_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CL_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return CL_Name_var;

        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdProposal.Rows.Count > 0)
            {
                PrintGrid.PrintGridViewProposalList(grdProposal, "Proposal List", "ProposalList");
            }
        }

        
    }
}