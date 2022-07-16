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
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Services;

namespace DESPLWEB
{
    public partial class Advance_Modify : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (!IsPostBack)
            {
                Label lblheader = (Label)Master.FindControl("lblHeading");
                lblheader.Text = "Modify Advance";
                optAdvReceipt.Checked = true;
                txt_FrmDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txt_Todate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }
                
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            DateTime From_Dtae = DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null);
            DateTime To_Dtae = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);
            if (To_Dtae >= From_Dtae)
            {
                LoadReceiptOrNoteList();
                CheckApproveRight();
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = true;
                lblMsg.Text = "ToDate should be Greater than or equal to the From Date";
                gvDetailsView.Visible = false;
            }
        }
        private void LoadReceiptOrNoteList()
        {
            if (optAdvReceipt.Checked == true)
            {
                gvDetailsView.Columns[2].HeaderText = "Receipt No";
                gvDetailsView.Columns[3].HeaderText = "Date";
                gvDetailsView.Columns[4].HeaderText = "Receipt Amount";                
            }
            else if (optAdvAdjustedNote.Checked == true)
            {
                gvDetailsView.Columns[2].HeaderText = "Note No";
                gvDetailsView.Columns[3].HeaderText = "Date";
                gvDetailsView.Columns[4].HeaderText = "Note Amount";  
            }
            gvDetailsView.Visible = true;
            gvDetailsView.DataSource = null;
            gvDetailsView.DataBind();
            DateTime From_Dtae = DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null);
            DateTime To_Dtae = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);
            var result = dc.Advance_View_List(From_Dtae, To_Dtae, optAdvReceipt.Checked);
            gvDetailsView.DataSource = result;
            gvDetailsView.DataBind();

            LoadFilterData();
        }
        protected void gvDetailsView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Approve" || e.CommandName == "Modify Receipt")
            //{
            //    if (e.CommandName == "Approve")
            //    {
            //        Session["Approve"] = e.CommandName.ToString();
            //    }
            //    else
            //    {
            //        Session["Approve"] = null;
            //    }
                string[] argg = new string[1];

                if (optAdvReceipt.Checked == true)
                {
                    //Session["AdvReceiptNo"] = e.CommandArgument;
                    //Response.Redirect("AdvanceReciptEntry.aspx");

                    EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                    string strURLWithData = "AdvanceReciptEntry.aspx?" + obj.Encrypt(string.Format("AdvReceiptNo={0}", e.CommandArgument));
                    //Response.Redirect(strURLWithData);
                    PrintGrid.Redirect1(strURLWithData, "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                }
                else if (optAdvAdjustedNote.Checked == true)
                {
                    //Session["AdvNoteNo"] = e.CommandArgument;
                    //Response.Redirect("AdvanceJournalEntry.aspx");

                    EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                    string strURLWithData = "AdvanceJournalEntry.aspx?" + obj.Encrypt(string.Format("AdvNoteNo={0}", e.CommandArgument));
                    //Response.Redirect(strURLWithData);
                    PrintGrid.Redirect1(strURLWithData, "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                }
            //}
        }

        public void CheckApproveRight()
        {
            var res = dc.UserRight_View(Convert.ToInt32(Session["LoginID"]), 0, true, false);
            bool Status = false;
            foreach (var u in res)
            {
                //Status = Convert.ToBoolean(u.USER_Approve_right_bit);
                Status = Convert.ToBoolean(u.USER_ReceiptApprove_bit);
            }
            if (Status == true)
            {
                for (int i = 0; i < gvDetailsView.Rows.Count; i++)
                {
                    LinkButton lnkApproveReceipt = (LinkButton)gvDetailsView.Rows[i].Cells[9].FindControl("lnkApproveReceipt");
                    lnkApproveReceipt.Visible = true;
                }
            }
            else
            {
                gvDetailsView.Columns[9].Visible = false;
            }

        }
        
        protected void gvDetailsView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[8].Text == "True")
                {
                    e.Row.Cells[8].Text = "Hold";
                }
                else if (e.Row.Cells[8].Text == "False")
                {
                    e.Row.Cells[8].Text = "Ok";
                }
                if (e.Row.Cells[7].Text == "True")
                {
                    e.Row.Cells[7].Text = "Cheque";
                }
                else if (e.Row.Cells[7].Text == "False")
                {
                    e.Row.Cells[7].Text = "Cash";
                }
            }
        }
                       
        private void LoadFilterData()
        {
            double Total = 0;
            for (int i = 0; i < gvDetailsView.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)gvDetailsView.Rows[i].Cells[0].FindControl("cbxSelect");
                string Status = "";
                if (ddlStatus.SelectedValue == "False")
                {
                    Status = "Ok";
                }
                else if (ddlStatus.SelectedValue == "True")
                {
                    Status = "Hold";
                }
                
                gvDetailsView.Rows[i].Visible = true;
                string BankDetail = (cbxSelect.CssClass);
                string[] arg = new string[3];
                arg = BankDetail.Split(';');
                int Cl_ID = Convert.ToInt32(arg[2]);
                                
                if (gvDetailsView.Rows[i].Visible == true)
                {
                    if (ddlStatus.SelectedItem.Text != "---All---")
                    {
                        if (Status == gvDetailsView.Rows[i].Cells[8].Text)
                        {
                            gvDetailsView.Rows[i].Visible = true;
                        }
                        else
                        {
                            gvDetailsView.Rows[i].Visible = false;
                        }
                    }
                }
                
                if (gvDetailsView.Rows[i].Visible == true)
                {
                    gvDetailsView.ShowFooter = false;
                    if (gvDetailsView.Rows[i].Cells[4].Text != "" && gvDetailsView.Rows[i].Cells[4].Text != "&nbsp;")
                    {
                        gvDetailsView.ShowFooter = true;
                        gvDetailsView.FooterRow.BackColor = System.Drawing.Color.White;
                        Total += (Convert.ToDouble(gvDetailsView.Rows[i].Cells[4].Text));
                        gvDetailsView.FooterRow.Cells[4].Text = Convert.ToDouble(Total).ToString("0.00");
                        gvDetailsView.FooterRow.Cells[4].Font.Bold = true;
                        gvDetailsView.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                        gvDetailsView.FooterStyle.BackColor = System.Drawing.Color.White;
                    }
                }
            }
        }
        protected void cbxSelectOnCheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gvDetailsView.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)gvDetailsView.Rows[i].Cells[0].FindControl("cbxSelect");
                if (cbxSelect.Checked && gvDetailsView.Rows[i].Cells[7].Text == "Cash")
                {
                    cbxSelect.Checked = false;
                    break;
                }
            }
        }
        
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFilterData();
        }

        protected void lnkPrntBankSlip_Click(object sender, EventArgs e)
        {
            if (gvDetailsView.Rows.Count > 0 && gvDetailsView.Visible == true && optAdvReceipt.Checked ==true)
            {
                bool valid = false;
                for (int i = 0; i < gvDetailsView.Rows.Count; i++)
                {
                    CheckBox cbxSelect = (CheckBox)gvDetailsView.Rows[i].Cells[0].FindControl("cbxSelect");
                    if (cbxSelect.Checked && gvDetailsView.Rows[i].Cells[7].Text == "Cheque"
                        && gvDetailsView.Rows[i].Cells[8].Text == "Ok")
                    {
                        valid = true;
                        break;
                    }
                }
                if (valid == true)
                {

                    string reportStr = "", strAccNo = "";
                    PrintHTMLReport rpt = new PrintHTMLReport();
                    if (ddlBank.SelectedValue == "NKGSB")
                    {
                        reportStr = RptBANKChqRpt_NKGSB();
                        rpt.DownloadHtmlReport("BankSlip_NKGSB", reportStr);
                    }
                    else if (ddlBank.SelectedValue == "HDFC Pune 1") //pune 1
                    {
                        strAccNo = "08252000000218";
                        reportStr = RptBANKChqRpt_HDFC(strAccNo);
                        rpt.DownloadHtmlReport("BankSlip_HDFC", reportStr);
                    }
                    else if (ddlBank.SelectedValue == "HDFC Pune 2") //pune 2
                    {
                        strAccNo = "50200022140042";
                        reportStr = RptBANKChqRpt_HDFC(strAccNo);
                        rpt.DownloadHtmlReport("BankSlip_HDFC", reportStr);
                    }
                    else if (ddlBank.SelectedValue == "HDFC Mumbai") //mumbai
                    {
                        strAccNo = "05402000024568";
                        reportStr = RptBANKChqRpt_HDFC(strAccNo);
                        rpt.DownloadHtmlReport("BankSlip_HDFC", reportStr);
                    }
                    else if (ddlBank.SelectedValue == "HDFC Nashik") //nashik
                    {
                        strAccNo = "50200023762951";
                        reportStr = RptBANKChqRpt_HDFC(strAccNo);
                        rpt.DownloadHtmlReport("BankSlip_HDFC", reportStr);
                    }
                }
            }
        }

        protected string RptBANKChqRpt_HDFC(string strAccNo)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            string reportStr = "", mySql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=5 align=center valign=top height=19><font size=4><b>  </b></font></td></tr>";

            mySql += "<tr>" +
            "<td width='20%' align=left valign=top height=19 rowspan=2><font size=4><b> HDFC BANK </b></font></td>" +
            "<td width='15%' align=left valign=top height=19><font size=1>&#9633; Cash</font></td>" +
            "<td width='15%' align=left valign=top height=19><font size=1>.For NRO Account</font></td>" +
            "<td width='15%' align=left valign=top height=19><font size=2>Date :</font></td>" +
            "<td width='15%' align=left valign=top height=19><font size=1>Deposit Slip (Bank Copy)</font></td>" +
            "</tr>";

            string strDate = DateTime.Now.ToString("ddMMyyyy");
            mySql += "<tr>" +
            "<td width='20%' align=left valign=top height=19><font size=1>&#9633; Local Cheque</font></td>" +
            "<td width='15%' align=left valign=top height=19><font size=1>&#9633; Dividend/Interest Warrant</font></td>" +
            "<td width='15%' align=left valign=top colspan=2 rowspan=2>" +
            "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>" +
            "<tr>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2>" + strDate.Substring(0, 1) + "</font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2>" + strDate.Substring(1, 1) + "</font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2>" + strDate.Substring(2, 1) + "</font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2>" + strDate.Substring(3, 1) + "</font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2>" + strDate.Substring(4, 1) + "</font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2>" + strDate.Substring(5, 1) + "</font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2>" + strDate.Substring(6, 1) + "</font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2>" + strDate.Substring(7, 1) + "</font></td>" +
                 "</tr>" +
            "</table>" +
            "</td>" +
            "</tr>";

            mySql += "<tr>" +
                "<td width='20%' align=left  valign=top height=19 ><font size=1>We understand your world</font></td>" +
             "<td width='15%' align=left valign=top height=19><font size=1>&#9633; Outstation Cheque </font></td>" +
             "<td width='15%' align=left valign=top height=19 rowspan=2><font size=1>&#9633; Rent/ Pension/ Sale Proceeds of Assets </font></td>" +
            "</tr>";

            mySql += "<tr>" +
                "<td width='20%' align=left  valign=top height=19 ><font size=1>&nbsp;</font></td>" +
                "<td width='20%' align=left  valign=top height=19><font size=1>&#9633; Credit Card Payment </font></td>" +

            "</tr>";

            mySql += "<tr>" +
                "<td width='20%' align=left  valign=top height=19 colspan=2><font size=2> Contact No : </font></td>" +
             "<td width='15%' align=left valign=top height=19 colspan=3><font size=2> Account Number : </font></td>" +
            "</tr>";

            mySql += "<tr>" +
            "<td width='15%' align=left valign=top colspan='2'>" +
                "<table align=left border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>" +
                "<tr>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "</tr>" +
                "</table>&nbsp;&nbsp;&nbsp;&nbsp;" +
            "</td>";
            
                mySql += "<td width='15%' align=left valign=top colspan='3'>" +
                    "<table align=left border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>" +
                    "<tr>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(0, 1) + "</b></font></td>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(1, 1) + "</b></font></td>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(2, 1) + "</b></font></td>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(3, 1) + "</b></font></td>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(4, 1) + "</b></font></td>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(5, 1) + "</b></font></td>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(6, 1) + "</b></font></td>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(7, 1) + "</b></font></td>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(8, 1) + "</b></font></td>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(9, 1) + "</b></font></td>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(10, 1) + "</b></font></td>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(11, 1) + "</b></font></td>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(12, 1) + "</b></font></td>" +
                     "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(13, 1) + "</b></font></td>" +
                     "</tr>" +
                    "</table>" +
                "</td>";
            

            mySql += "</tr>";

            mySql += "<tr>" +
                "<td width='20%' align=left  valign=top height=19 colspan=2><font size=2> PAN No. : </font></td>" +
             "<td width='15%' align=left valign=top height=19 colspan=3><font size=2> Credit Card Number : </font></td>" +
            "</tr>";

            mySql += "<tr>" +
            "<td width='15%' align=left valign=top colspan=2>" +
                "<table align=left border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>" +
                "<tr>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "</tr>" +
                "</table>&nbsp;&nbsp;&nbsp;&nbsp;" +
            "</td>" +

            "<td width='15%' align=left valign=top colspan=3>" +
                "<table align=left border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>" +
                "<tr>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>   </b></font></td>" +
                 "</tr>" +
                "</table>" +
            "</td>" +

            "</tr>";

            mySql += "<tr>" +
                "<td width='20%' align=left  valign=top height=19 colspan=5><font size=2> Name : <u>Durocrete Engineering Services Pvt. Ltd.</u></font></td>" +
            "</tr>";

            double Total = 0;
            for (int i = 0; i < gvDetailsView.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)gvDetailsView.Rows[i].Cells[0].FindControl("cbxSelect");
                string BankDetail = (cbxSelect.CssClass);
                if (cbxSelect.Checked && gvDetailsView.Rows[i].Cells[7].Text == "Cheque"
                    && gvDetailsView.Rows[i].Cells[8].Text == "Ok")
                {
                    string[] arg1 = BankDetail.Split(';');
                    string[] arg = arg1[1].Split('|');
                    string ChequeNo = Convert.ToString(arg[0]);
                    string BankName = Convert.ToString(arg[1]);
                    string BranchName = Convert.ToString(arg[3]);
                    if (BankName != "---Select---" && BranchName != null && BranchName != "" && BankName != "" && ChequeNo != null && ChequeNo != "")
                    {
                        if (gvDetailsView.Rows[i].Cells[4].Text != "" && gvDetailsView.Rows[i].Cells[4].Text != "&nbsp;")
                        {
                            Total += (Convert.ToDouble(gvDetailsView.Rows[i].Cells[4].Text) - Convert.ToDouble(gvDetailsView.Rows[i].Cells[5].Text));
                        }
                    }
                }
            }
            int j = 0;
            int Paisa = 0;
            for (int i = 0; i < gvDetailsView.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)gvDetailsView.Rows[i].Cells[0].FindControl("cbxSelect");
                string BankDetail = (cbxSelect.CssClass);
                if (cbxSelect.Checked)
                {
                    string[] arg1 = BankDetail.Split(';');
                    string[] arg = arg1[1].Split('|');
                    string ChequeNo = Convert.ToString(arg[0]);
                    string BankName = Convert.ToString(arg[1]);
                    string BranchName = Convert.ToString(arg[3]);
                    if (BankName != "---Select---" && BranchName != null && BranchName != "" && BankName != "" && ChequeNo != null && ChequeNo != "")
                    {
                        if (j == 0)
                        {

                            mySql += "<tr><td width='99%' colspan=5>";
                            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
                            mySql += "<tr>";
                            mySql += "<td width= 20% align=center colspan=1  valign=medium height=19 ><font size=2><b> Client Name </b></font></td>";
                            mySql += "<td width= 20% align=center colspan=1  valign=medium height=19 ><font size=2><b> Bank & Branch </b></font></td>";
                            mySql += "<td width= 15% align=center colspan=1  valign=medium height=19 ><font size=2><b> City  </b></font></td>";
                            mySql += "<td width= 12% align=center colspan=1  valign=medium height=19 ><font size=2><b> Cheque No. </b></font></td>";
                            mySql += "<td width= 10% align=center colspan=2  valign=medium height=19 ><font size=2><b> Denomination </b></font></td>";
                            mySql += "<td width= 10% align=center colspan=1 valign=medium height=19 ><font size=2><b> Rupees </b></font></td>";
                            mySql += "</tr>";
                        }
                        mySql += "<tr>";
                        mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;" + gvDetailsView.Rows[i].Cells[6].Text + "</font></td>";
                        mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;" + BankName + "</font></td>";
                        mySql += "<td width= 15% align=left valign=top height=19 ><font size=2>&nbsp;" + BranchName + "</font></td>";
                        mySql += "<td width= 12% align=left valign=top height=19 ><font size=2>&nbsp;" + ChequeNo + "</font></td>";
                        if (j == 0)
                            mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "1000x" + "</font></td>";
                        else if (j == 1)
                            mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "500x" + "</font></td>";
                        else if (j == 2)
                            mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "100x" + "</font></td>";
                        else if (j == 3)
                            mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "50x" + "</font></td>";
                        else if (j == 4)
                            mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "20x" + "</font></td>";
                        else if (j == 5)
                            mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "10x" + "</font></td>";
                        else
                            mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";

                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";

                        string[] paise = new string[1];
                        arg = (Convert.ToDecimal(gvDetailsView.Rows[i].Cells[4].Text) - Convert.ToDecimal(gvDetailsView.Rows[i].Cells[5].Text)).ToString().Split('.');
                        if (Convert.ToInt32(arg[1]) > 0)
                        {
                            Paisa += Convert.ToInt32(arg[1]);
                        }
                        mySql += "<td width= 5% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(arg[0]) + "." + Convert.ToString(arg[1]) + "</font></td>";
                        mySql += "</tr>";
                        j++;
                    }
                }
            }

            if (j < 5)
            {
                for (int i = j; i < 6; i++)
                {
                    mySql += "<tr>";
                    mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                    mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                    mySql += "<td width= 15% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                    mySql += "<td width= 12% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                    if (i == 0)
                        mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "1000x" + "</font></td>";
                    else if (i == 1)
                        mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "500x" + "</font></td>";
                    else if (i == 2)
                        mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "100x" + "</font></td>";
                    else if (i == 3)
                        mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "50x" + "</font></td>";
                    else if (i == 4)
                        mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "20x" + "</font></td>";
                    else if (i == 5)
                        mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "10x" + "</font></td>";
                    else
                        mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";

                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                    mySql += "<td width= 5% align=right valign=top height=19 ><font size=2>&nbsp;</font></td>";
                    mySql += "</tr>";
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 10% align=left  colspan=4 rowspan=2 valign=middle height=19 ><font size=2> &nbsp; Rupees (in words) : <u>" + rpt.CnvtAmttoWords(Convert.ToInt32(Math.Abs(Convert.ToDecimal(Total)))) + " Only </u></font></td>";
            mySql += "<td width= 10% align=center colspan=1 valign=top height=19 ><font size=2>&nbsp; Others</font></td>";
            mySql += "<td width= 10% align=right colspan=1 valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "<td width= 10% align=right colspan=1 valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 10% align=center colspan=2 valign=top height=19 ><font size=2>&nbsp;Total Rupees</font></td>";
            mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDouble(Total).ToString("0.00") + "</font></td>";
            mySql += "</tr>";
            mySql += "</table>";

            mySql += "</td></tr>";

            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr><td colspan=5><font size=2>&nbsp;Purpose for Payment : ___________________________________________________________</font></td></tr>";

            mySql += "<tr><td colspan=5>&nbsp;</td></tr>";

            mySql += "<tr><td colspan=2> _______________________ </td>";
            mySql += "<td colspan=2> &nbsp;&nbsp;&nbsp;&nbsp;_______________________ </td>";
            mySql += "<td colspan=1><font size=1> *For cash deposits greater than Rs. 49,999/- mention PAN No. </font></td></tr>";

            mySql += "<tr><td colspan=2><font size=2> &nbsp; Depositor's Signature </font></td>";
            mySql += "<td colspan=2><font size=2>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Teller's Signature</font></td>";
            mySql += "<tr><td> &nbsp; </td></tr>";


            mySql += "</table>";
            //mySql += "</td>";
            //mySql += "<td>&nbsp;</td>";
            //mySql += "</tr>";
            mySql += "</table>";
            mySql += "</html>";
            return reportStr = mySql;
        }
        protected string RptBANKChqRpt_NKGSB()
        {
            PrintPDFReport rpt = new PrintPDFReport();
            string reportStr = "", mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>  </b></font></td></tr>";

            mySql += "<tr>" +
            "<td width='15%' align=left valign=top height=19><font size=1><b>  </b></font></td>" +
            "<td width='5%' align=left valign=top height=19><font size=1><b>  </b></font></td>" +
            "<td width='20%' align=left valign=top height=19><font size=6><b> NKGSB </b></font></td>" +
            "</tr>";
            mySql += "<tr>" +
             "<td width='15%' align=left valign=top height=19><font size=1><b>  </b></font></td>" +
            "<td width='5%' align=left valign=top height=19><font size=1><b>  </b></font></td>" +
            "<td width='20%' align=left  valign=top height=19><font size=4><b> CO-OP. BANK LTD. </b></font></td>" +
            "</tr>";
            mySql += "<tr>" +
            "<td width='15%' align=left valign=top height=19><font size=1><b>  </b></font></td>" +
            "<td width='5%' align=left valign=top height=19><font size=1><b>  </b></font></td>" +
            "<td width='20%' align=left  valign=top height=19><font size=2> (Multi-State Scheduled Bank) </font></td>" +
            "</tr>";
            mySql += "<tr><td width='15%' colspan=3 align=left valign=top height=19><font size=2><b> Regd. Office : Laxmi Sadan,361, V.P. Road, Girgam, Mumbai 400 004. Tel. : 6754 50000 Fax : 6754 5023 Email : sec@nkgsb-bank.com \n Customer Care : 022-28602000  </b></font></td></tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<tr>" +
                    "<td width='20%'align=left valign=top height=19><font size=2><b>  </b></font></td>" +
                    "<td width='20%'align=left valign=top height=19><font size=2><b> Paud Road Branch  </b></font></td>" +
                    "<td width='5%' align=left valign=top height=19><font size=2><b> Date   " + System.DateTime.Now.ToString("dd/MM/yyyy") + " </b></font></td>" +

                    "</tr>";
            double Total = 0;
            for (int i = 0; i < gvDetailsView.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)gvDetailsView.Rows[i].Cells[0].FindControl("cbxSelect");
                string BankDetail = (cbxSelect.CssClass);
                if (cbxSelect.Checked && gvDetailsView.Rows[i].Cells[8].Text == "Ok")
                {
                    string[] arg1 = BankDetail.Split(';');
                    string[] arg = arg1[1].Split('|');
                    string ChequeNo = Convert.ToString(arg[0]);
                    string BankName = Convert.ToString(arg[1]);
                    string BranchName = Convert.ToString(arg[3]);
                    if (BankName != "---Select---" && BranchName != null && BranchName != "" && BankName != "" && ChequeNo != null && ChequeNo != "")
                    {
                        if (gvDetailsView.Rows[i].Cells[4].Text != "" && gvDetailsView.Rows[i].Cells[4].Text != "&nbsp;")
                        {
                            //Total += Convert.ToDouble(gvDetailsView.Rows[i].Cells[4].Text);
                            Total += (Convert.ToDouble(gvDetailsView.Rows[i].Cells[4].Text) - Convert.ToDouble(gvDetailsView.Rows[i].Cells[5].Text));
                        }
                    }
                }
            }
            int j = 0;
            int Paisa = 0;
            for (int i = 0; i < gvDetailsView.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)gvDetailsView.Rows[i].Cells[0].FindControl("cbxSelect");
                string BankDetail = (cbxSelect.CssClass);
                if (cbxSelect.Checked)
                {                    
                    string[] arg1 = BankDetail.Split(';');
                    string[] arg = arg1[1].Split('|');
                    string ChequeNo = Convert.ToString(arg[0]);
                    string BankName = Convert.ToString(arg[1]);
                    string BranchName = Convert.ToString(arg[3]);
                    
                    if (BankName != "---Select---" && BranchName != null && BranchName != "" && BankName != "" && ChequeNo != null && ChequeNo != "")
                    {
                        if (j == 0)
                        {
                            mySql += "<tr>" +
                            "<td width='15%' align=left valign=top height=19><font size=2><b>  Rs. " + Convert.ToDouble(Total).ToString("0.00") + " by Cash/Cheque  </b></font></td>" +
                            "</tr>";

                            mySql += "<tr>" +
                            "<td width='10%' align=left valign=top height=19><font size=2><b>   </b></font></td>";
                            mySql += "<td width='5%' align=left valign=top height=19><font size=2><b>CC/OD A/c No. </b></font></td>";
                            mySql += "<td align=left> ";
                            mySql += "<table align=left border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
                            mySql += "<tr>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 0  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 8  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 9  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 1  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 3  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 0  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 1  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 0  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 0  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 0  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 0  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 0  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 0  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 0  </b></font></td>" +
                                 "<td width='5%'align=center  valign=top height=19><font size=2><b> 3  </b></font></td>" +
                                 "</tr>";
                            mySql += "</table>";
                            mySql += "</td>";
                            mySql += "</tr>";

                            mySql += "<tr>" +
                            "<td width='10%' align=left colspan=2 valign=top height=19><font size=2><b> Paid into the credit of   Durocrete Engineering Services Pvt. Ltd   </b></font></td>" +
                                // "<td width='5%' align=left valign=top height=19><font size=2><b><p> .<hr></p></b></font></td>" +
                                // "<td width='5%'align=center  valign=top height=19><font size=2></font></td>" +
                            "</tr>";

                            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
                            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
                            mySql += "<tr>";
                            mySql += "<td width= 15% align=center rowspan=2  valign=medium height=19 ><font size=2><b> DRAWN ON WHICH BANK </b></font></td>";
                            mySql += "<td width= 15% align=center rowspan=2  valign=medium height=19 ><font size=2><b> BRANCH  </b></font></td>";
                            mySql += "<td width= 10% align=center rowspan=2  valign=medium height=19 ><font size=2><b> CHEQUE NO. </b></font></td>";
                            mySql += "<td width= 10% align=center rowspan=2  valign=medium height=19 ><font size=2><b> CASH /  NOTES </b></font></td>";
                            mySql += "<td width= 10% align=center colspan=2 valign=medium height=19 ><font size=2><b> AMOUNT </b></font></td>";
                            mySql += "</tr>";
                            mySql += "<td width= 5% align=center  valign=medium height=19 ><font size=2><b> Rs. </b></font></td>";
                            mySql += "<td width= 5% align=center  valign=medium height=19 ><font size=2><b> P. </b></font></td>";
                        }
                        mySql += "<tr>";
                        mySql += "<td width= 15% align=left valign=top height=19 ><font size=2>&nbsp;" + BankName + "</font></td>";
                        mySql += "<td width= 15% align=left valign=top height=19 ><font size=2>&nbsp;" + BranchName + "</font></td>";
                        mySql += "<td width =10% align=center valign=top height=19 ><font size=2>&nbsp;" + ChequeNo + "</font></td>";
                        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "X" + "</font></td>";
                        string[] paise = new string[1];
                        //arg = Convert.ToDecimal(gvDetailsView.Rows[i].Cells[4].Text).ToString().Split('.');
                        arg = (Convert.ToDecimal(gvDetailsView.Rows[i].Cells[4].Text) - Convert.ToDecimal(gvDetailsView.Rows[i].Cells[5].Text)).ToString().Split('.');
                        if (Convert.ToInt32(arg[1]) > 0)
                        {
                            Paisa += Convert.ToInt32(arg[1]);
                        }
                        mySql += "<td width= 5% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(arg[0]) + "</font></td>";
                        mySql += "<td width= 5% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(arg[1]) + "</font></td>";
                        mySql += "</tr>";
                        j++;
                    }
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 10% align=right colspan=4 valign=top height=19 ><font size=2><b>&nbsp;Total Rs.</b></font></td>";
            mySql += "<td width= 10% align=right valign=top height=19 ><font size=2><b>&nbsp;" + Convert.ToDouble(Total).ToString() + "<b></font></td>";
            mySql += "<td width= 10% align=right valign=top height=19 ><font size=2><b>&nbsp;" + Paisa + " </b></font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left colspan=6 valign=top height=19 ><font size=2><b> &nbsp; Rs. (in words) " + rpt.CnvtAmttoWords(Convert.ToInt32(Math.Abs(Convert.ToDecimal(Total)))) + " Only </b> </font></td>";
            mySql += "</tr>";


            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7><b> &nbsp; Please use separate slip for CASH AND CHEQUE. </b></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp; </td></tr>";
            mySql += "<tr>";
            mySql += "<td width='10%' >&nbsp; </td>";
            mySql += "<td width='50%' align=right >";
            mySql += "<table align=right  border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=70% id=AutoNumber1>";
            mySql += "<tr>" +
                  "<td width='20%'align=left rowspan=2 valign=top height=19><font size=2><b> Scroll No.  </b></font></td>" +
                  "<td width='40%'align=left valign=top height=19><font size=2><b> &nbsp; Entered by  </b></font></td>" +
                  "</tr>";
            mySql += "<tr><td width='20%'align=left valign=top height=19><font size=2><b>&nbsp; Checked by  </b></font></tr>";
            mySql += "</table>";
            mySql += "</td>";
            mySql += "</tr>";

            mySql += "<tr><td width='99%' colspan=5>&nbsp;</td></tr>";

            mySql += "<tr>" +
            "<td width='40%'  align=left ><b> &nbsp; (Cheque will be Credited on realisation)</b> </td>" +
            "<td width='5%'  align=left >&nbsp; Deposited by <p> <hr> </p>  </td>" +
                // "<td width='10%' align=left>&nbsp; <p> <hr> </p> </td>" +
            "</tr>";

            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            return reportStr = mySql;
        }
        protected void lnkPrintReceipt_Click(object sender, EventArgs e)
        {
            if (txtReceiptNo.Text != "")
            {
                //PrintPDFReport rpt = new PrintPDFReport();
                //rpt.AdvanceReceipt_PDF(txtReceiptNo.Text);
                DateTime BillDate = DateTime.Now;
                var AdvReceipt = dc.Advance_View(null, null, Convert.ToInt32(txtReceiptNo.Text), "", true);
                foreach (var adv in AdvReceipt)
                {
                    BillDate = Convert.ToDateTime(adv.ReceiptDate);
                    break;
                }

                PrintPDFReport rpt = new PrintPDFReport();
                if (CheckGSTFlag(BillDate) == false)
                    rpt.AdvanceReceipt_PDF(txtReceiptNo.Text);//service tax old format
                else
                    rpt.AdvanceReceipt_PDF_GST(txtReceiptNo.Text);//GST new format
            }
        }
        public bool CheckGSTFlag(DateTime BillDate)
        {
            bool gstFlag = false;
            var master = dc.GST_View(1, BillDate);
            if (master.Count() > 0)
            {
                gstFlag = true;
            }
            else
            {
                gstFlag = false;
            }
            return gstFlag;
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (gvDetailsView.Rows.Count > 0 && gvDetailsView.Visible == true)
            {
                //string reportPath;
                string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
                reportStr = RptModifyCashRecpt();
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                PrintHTMLReport rptHtml = new PrintHTMLReport();
                rptHtml.DownloadHtmlReport("AdvanceReport", reportStr);
            }
        }
        protected string RptModifyCashRecpt()
        {
            string reportStr = "", mySql = "", tempSql = "", strHeading="";
            if (optAdvReceipt.Checked == true)
            {
                strHeading = "Advance Receipt List";
            }
            else
            {
                strHeading = "Adjusted Advance Note List";
            }
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> " + strHeading + " </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            mySql += "<tr>" +
            "<td width='15%' align=left valign=top height=19><font size=2><b> Period </b></font></td>" +
            "<td width='2%' height=19><font size=2>:</font></td>" +
            "<td width='40%' height=19><font size=2>" + DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + " - " + DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + "</font></td>" +
            "</tr>";
            //if (ddlClient.SelectedItem.Text != "---All---")
            //{
            //    mySql += "<tr>" +
            //   "<td width='15%' align=left valign=top height=19><font size=2><b> Client </b></font></td>" +
            //   "<td width='2%' height=19><font size=2>:</font></td>" +
            //   "<td width='40%' height=19><font size=2> " + ddlClient.SelectedItem.Text + " </font></td>" +
            //   "</tr>";
            //}
            //if (ddlAmount.SelectedItem.Text != "---All---")
            //{
            //    mySql += "<tr>" +
            //   "<td width='15%' align=left valign=top height=19><font size=2><b> Amount </b></font></td>" +
            //   "<td width='2%' height=19><font size=2>:</font></td>" +
            //   "<td width='40%' height=19><font size=2> " + ddlAmount.SelectedItem.Text + " </font></td>" +
            //   "</tr>";
            //}
            //if (ddlPaymttype.SelectedItem.Text != "---All---")
            //{
            //    mySql += "<tr>" +
            //   "<td width='15%' align=left valign=top height=19><font size=2><b> Payment Type </b></font></td>" +
            //   "<td width='2%' height=19><font size=2>:</font></td>" +
            //   "<td width='40%' height=19><font size=2> " + ddlPaymttype.SelectedItem.Text + " </font></td>" +
            //   "</tr>";
            //}
            if (ddlStatus.SelectedItem.Text != "---All---")
            {
                mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Status </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2> " + ddlStatus.SelectedItem.Text + " </font></td>" +
               "</tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";

            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>" + gvDetailsView.Columns[2].HeaderText + " </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>" + gvDetailsView.Columns[3].HeaderText + "</b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>" + gvDetailsView.Columns[4].HeaderText + " </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>" + gvDetailsView.Columns[5].HeaderText + " </b></font></td>";
            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b> " + gvDetailsView.Columns[6].HeaderText + " </b></font></td>";
            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b> " + gvDetailsView.Columns[7].HeaderText + "</b></font></td>";
            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b> " + gvDetailsView.Columns[8].HeaderText + " </b></font></td>";
            double Total = 0;
            for (int i = 0; i < gvDetailsView.Rows.Count; i++)
            {
                if (gvDetailsView.Rows[i].Visible == true)
                {
                    Total += Convert.ToDouble(gvDetailsView.Rows[i].Cells[4].Text);
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + gvDetailsView.Rows[i].Cells[2].Text + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + gvDetailsView.Rows[i].Cells[3].Text + "</font></td>";
                    mySql += "<td width = 5% align=center valign=top height=19 ><font size=2>&nbsp;" + gvDetailsView.Rows[i].Cells[4].Text + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + gvDetailsView.Rows[i].Cells[5].Text + "</font></td>";
                    mySql += "<td width= 15% align=left valign=top height=19 ><font size=2>&nbsp;" + gvDetailsView.Rows[i].Cells[6].Text + "</font></td>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + gvDetailsView.Rows[i].Cells[7].Text + "</font></td>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + gvDetailsView.Rows[i].Cells[8].Text + "</font></td>";
                    mySql += "</tr>";
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 2% align=right colspan=2 valign=top height=19 ><font size=2> Total Amount &nbsp;&nbsp; </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDouble(Total).ToString("0.00") + "</font></td>";
            mySql += "<td width= 2% align=center colspan=4 valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            return reportStr = mySql;
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void optAdvReceipt_CheckedChanged(object sender, EventArgs e)
        {
            gvDetailsView.DataSource = null;
            gvDetailsView.DataBind();
        }

        protected void optAdvAdjustedNote_CheckedChanged(object sender, EventArgs e)
        {
            gvDetailsView.DataSource = null;
            gvDetailsView.DataBind();    
        }

    }
}