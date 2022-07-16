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
    public partial class CashReceipt_Modify : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (!IsPostBack)
            {
                Label lblheader = (Label)Master.FindControl("lblHeading");
                lblheader.Text = "Modify Receipt";
                dateShow();
            }
        }

        private void dateShow()
        {
            this.txt_FrmDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            this.txt_Todate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            ddlClient.Items.Insert(0, "---All---");
            ddlAmount.Items.Insert(0, "---All---");
            ddlPaymttype.Items.Insert(0, "---All---");
            ddlStatus.Items.Insert(0, "---All---");
            ddlApprStatus.Items.Insert(0, "---All---");
        }


        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            DateTime From_Dtae = DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null);
            DateTime To_Dtae = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);
            if (To_Dtae >= From_Dtae)
            {
                CashReceiptModifyView();
                DisplayAppvStatus();
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = true;
                lblMsg.Text = "ToDate should be Greater than or equal to the From Date";
                gvDetailsView.Visible = false;
            }
        }
        protected void gvDetailsView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve" || e.CommandName == "Modify Receipt")
            {
              
                string[] argg = new string[2];
                argg = Convert.ToString(e.CommandArgument).Split(';');

                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                string strURLWithData = "CashReceipt_Entry.aspx?" + obj.Encrypt(string.Format("ReceiptNo={0}&BillNo={1}&Approve={2}", Convert.ToString(argg[0]), Convert.ToString(argg[1]) , Convert.ToString(e.CommandName) ));
                Response.Redirect(strURLWithData);

            }
            //else if (e.CommandName == "Print")
            //{
            //    string[] argg = new string[2];
            //    argg = Convert.ToString(e.CommandArgument).Split(';');
            //    PrintPDFReport rpt = new PrintPDFReport();
            //    rpt.CashReceipt_PDF(Convert.ToInt32(argg[0]));
            //}
        }

        public void DisplayAppvStatus()
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
                    if (gvDetailsView.Rows[i].Cells[9].Text == "Pending")
                    {
                        LinkButton lnkApproveReceipt = (LinkButton)gvDetailsView.Rows[i].Cells[9].FindControl("lnkApproveReceipt");
                        lnkApproveReceipt.Visible = true;
                    }
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
                if (e.Row.Cells[8].Text == "False")
                {
                    e.Row.Cells[8].Text = "Ok";
                }
                if (e.Row.Cells[7].Text == "True")
                {
                    e.Row.Cells[7].Text = "Cheque";
                }
                if (e.Row.Cells[7].Text == "False")
                {
                    e.Row.Cells[7].Text = "Cash";
                }
                LinkButton lnkApproveReceipt = (LinkButton)e.Row.FindControl("lnkApproveReceipt"); 
                if (e.Row.Cells[9].Text == "True")
                {
                    e.Row.Cells[9].Text = "Approved";
                    lnkApproveReceipt.Visible = false;
                }
                if (e.Row.Cells[9].Text == "False")
                {
                    e.Row.Cells[9].Text = "Pending";
                    lnkApproveReceipt.Visible = true;
                }
                
            }
        }


        public static DropDownList DDL_Client(DropDownList ddlClient)
        {
            for (int i = 0; i <= ddlClient.Items.Count - 1; i++)
            {
                ddlClient.SelectedIndex = i;
                string str = ddlClient.SelectedItem.ToString();
                for (int counter = i + 1; counter <= ddlClient.Items.Count - 1; counter++)
                {
                    ddlClient.SelectedIndex = counter;
                    string compareStr = ddlClient.SelectedItem.ToString();
                    if (str == compareStr)
                    {
                        ddlClient.Items.RemoveAt(counter);
                        counter = counter - 1;
                    }
                }
            }
            return ddlClient;
        }
        public static DropDownList DDL_Amount(DropDownList ddlAmount)
        {
            for (int i = 0; i <= ddlAmount.Items.Count - 1; i++)
            {
                ddlAmount.SelectedIndex = i;
                string str = ddlAmount.SelectedItem.ToString();
                for (int counter = i + 1; counter <= ddlAmount.Items.Count - 1; counter++)
                {
                    ddlAmount.SelectedIndex = counter;
                    string compareStr = ddlAmount.SelectedItem.ToString();
                    if (str == compareStr)
                    {
                        ddlAmount.Items.RemoveAt(counter);
                        counter = counter - 1;
                    }
                }
            }
            return ddlAmount;
        }
        private void BindClient()
        {
            DateTime From_Dtae = DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null);
            DateTime To_Dtae = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);

            var result = dc.CashModify_View(From_Dtae, To_Dtae);
            ddlClient.DataTextField = "CL_Name_var";
            ddlClient.DataValueField = "CL_Id";
            ddlClient.DataSource = result;
            ddlClient.DataBind();
            ddlClient.Items.Insert(0, new ListItem("---All---", "0"));
            DDL_Client(ddlClient);
            ddlClient.SelectedValue = "0";
            var amt = dc.CashModify_View(From_Dtae, To_Dtae);
            ddlAmount.DataSource = amt;
            ddlAmount.DataTextField = "Cash_Amount_num";
            ddlAmount.DataValueField = "Cash_Amount_num";
            ddlAmount.DataBind();
            ddlAmount.Items.Insert(0, new ListItem("---All---", "0"));
            DDL_Amount(ddlAmount);
            ddlAmount.SelectedValue = "0";
        }

        private void CashReceiptModifyView()
        {
            gvDetailsView.Visible = true;
            gvDetailsView.DataSource = null;
            gvDetailsView.DataBind();
            DateTime From_Dtae = DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null);
            DateTime To_Dtae = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);
            var result = dc.CashModify_View(From_Dtae, To_Dtae);
            gvDetailsView.DataSource = result;
            gvDetailsView.DataBind();
            BindClient();
            BindFilter();
        }
        private void BindFilter()
        {
            double Total = 0;
            for (int i = 0; i < gvDetailsView.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)gvDetailsView.Rows[i].Cells[0].FindControl("cbxSelect");
                string Paymenttype = "";
                string Status = "";
                string apprStatus = "";
                if (ddlStatus.SelectedValue == "False")
                {
                    Status = "Ok";
                }
                else if (ddlStatus.SelectedValue == "True")
                {
                    Status = "Hold";
                }
                if (ddlPaymttype.SelectedValue == "True")
                {
                    Paymenttype = "Cheque";
                }
                else if (ddlPaymttype.SelectedValue == "False")
                {
                    Paymenttype = "Cash";
                }
                if (ddlApprStatus.SelectedValue == "True")
                {
                    apprStatus = "Approved";
                }
                else if (ddlApprStatus.SelectedValue == "False")
                {
                    apprStatus = "Pending";
                }
                gvDetailsView.Rows[i].Visible = true;
                string BankDetail = (cbxSelect.CssClass);
                string[] arg = new string[4];
                arg = BankDetail.Split(';');
                int Cl_ID = Convert.ToInt32(arg[4]);
                if (ddlClient.SelectedItem.Text != "---All---")
                {
                    if (Convert.ToInt32(ddlClient.SelectedValue) == Cl_ID)
                    {
                        gvDetailsView.Rows[i].Visible = true;
                    }
                    else
                    {
                        gvDetailsView.Rows[i].Visible = false;
                    }
                }
                if (gvDetailsView.Rows[i].Visible == true)
                {
                    if (ddlAmount.SelectedItem.Text != "---All---")
                    {
                        if (Convert.ToDecimal(ddlAmount.SelectedValue) == Convert.ToDecimal(gvDetailsView.Rows[i].Cells[4].Text))
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
                    if (ddlPaymttype.SelectedItem.Text != "---All---")
                    {
                        if (Paymenttype == gvDetailsView.Rows[i].Cells[7].Text)
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
                    if (ddlApprStatus.SelectedItem.Text != "---All---")
                    {
                        if (apprStatus == gvDetailsView.Rows[i].Cells[9].Text)
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
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int index = gvRow.RowIndex;
            CheckBox cbxSelect = (CheckBox)gvDetailsView.Rows[index].FindControl("cbxSelect");
            if (cbxSelect.Checked && (gvDetailsView.Rows[index].Cells[7].Text == "Cash" || gvDetailsView.Rows[index].Cells[8].Text == "Hold"))
            {
                cbxSelect.Checked = false;
            }
            //for (int i = 0; i < gvDetailsView.Rows.Count; i++)
            //{
            //    CheckBox cbxSelect = (CheckBox)gvDetailsView.Rows[i].Cells[0].FindControl("cbxSelect");
            //    if (cbxSelect.Checked && (gvDetailsView.Rows[i].Cells[7].Text == "Cash" || gvDetailsView.Rows[i].Cells[8].Text == "Hold"))
            //    {
            //        cbxSelect.Checked = false;
            //        break;
            //    }
            //}
        }
        protected void cbxSelectAllOnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbxSelectAll = (CheckBox)gvDetailsView.HeaderRow.FindControl("cbxSelectAll");
            for (int i = 0; i < gvDetailsView.Rows.Count; i++)
            {
                CheckBox cbxSelect = (CheckBox)gvDetailsView.Rows[i].Cells[0].FindControl("cbxSelect");
                if (gvDetailsView.Rows[i].Cells[7].Text == "Cash" || gvDetailsView.Rows[i].Cells[8].Text == "Hold")
                {
                    cbxSelect.Checked = false;
                }
                else
                {
                    cbxSelect.Checked = cbxSelectAll.Checked;
                }
            }
        }
        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFilter();
        }

        protected void ddlAmount_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFilter();
        }

        protected void ddlPaymttype_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFilter();
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFilter();
        }

        protected void ddlApprStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFilter();
        }

        protected void lnkPrntBankSlip_Click(object sender, EventArgs e)
        {
            if (gvDetailsView.Rows.Count > 0 && gvDetailsView.Visible == true)
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
                 "<td width='5%'align=center  valign=top height=19><font size=2><b>" + strAccNo.Substring(0, 1)  + "</b></font></td>" +
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
            "</td>" ;
            
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
                    string[] arg = new string[4];
                    arg = BankDetail.Split(';');
                    string BankName = Convert.ToString(arg[1]);
                    string BranchName = Convert.ToString(arg[2]);
                    string ChequeNo = Convert.ToString(arg[3]);
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
                    string[] arg = new string[4];
                    arg = BankDetail.Split(';');
                    string BankName = Convert.ToString(arg[1]);
                    string BranchName = Convert.ToString(arg[2]);
                    string ChequeNo = Convert.ToString(arg[3]);
                    //string CityName = Convert.ToString(arg[4]);
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
                        if (j==0)
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
                if (cbxSelect.Checked && gvDetailsView.Rows[i].Cells[7].Text == "Cheque" 
                    && gvDetailsView.Rows[i].Cells[8].Text == "Ok")
                {
                    string[] arg = new string[4];
                    arg = BankDetail.Split(';');
                    string BankName = Convert.ToString(arg[1]);
                    string BranchName = Convert.ToString(arg[2]);
                    string ChequeNo = Convert.ToString(arg[3]);
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
                    string[] arg = new string[4];
                    arg = BankDetail.Split(';');
                    string BankName = Convert.ToString(arg[1]);
                    string BranchName = Convert.ToString(arg[2]);
                    string ChequeNo = Convert.ToString(arg[3]);
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
            if (txtReceiptNo.Text!="")
            {
                PrintPDFReport rpt = new PrintPDFReport();
                rpt.CashReceipt_PDF(Convert.ToInt32(txtReceiptNo.Text));
                //txtReceiptNo.Text = "";
            }
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
                rptHtml.DownloadHtmlReport("ModifyReceipt", reportStr);
            }
        }
        protected string RptModifyCashRecpt()
        {
            string reportStr = "", mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Modify Receipt </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            mySql += "<tr>" +
            "<td width='15%' align=left valign=top height=19><font size=2><b> Period </b></font></td>" +
            "<td width='2%' height=19><font size=2>:</font></td>" +
            "<td width='40%' height=19><font size=2>" + DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + " - " + DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + "</font></td>" +
            "</tr>";
            if (ddlClient.SelectedItem.Text != "---All---")
            {
                mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Client </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2> " + ddlClient.SelectedItem.Text + " </font></td>" +
               "</tr>";
            }
            if (ddlAmount.SelectedItem.Text != "---All---")
            {
                mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Amount </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2> " + ddlAmount.SelectedItem.Text + " </font></td>" +
               "</tr>";
            }
            if (ddlPaymttype.SelectedItem.Text != "---All---")
            {
                mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Payment Type </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2> " + ddlPaymttype.SelectedItem.Text + " </font></td>" +
               "</tr>";
            }
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

            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b> Receipt No </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Date  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Receipt Amount </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> TDS Amount </b></font></td>";
            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b> Client Name </b></font></td>";
            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>  Payment Type  </b></font></td>";
            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>  Status  </b></font></td>";
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

        protected void lnkPrntBankSlipExcel_Click(object sender, EventArgs e)
        {
            PrintExcelReport rptExcel = new PrintExcelReport();
           // rptExcel.BankSlip_ExcelPrint();
        }

    }
}