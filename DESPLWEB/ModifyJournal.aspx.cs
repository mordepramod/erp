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
    public partial class ModifyJournal : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheader = (Label)Master.FindControl("lblHeading");
                lblheader.Text = "Modify Journal";
                DateShow();
                Rdn_Ok.Checked = true;
                Rdn_Debit.Checked = true;
                txt_Client.Focus();
            }
        }
        private void DateShow()
        {
            this.txt_FrmDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            this.txt_Todate.Text = DateTime.Today.ToString("dd/MM/yyyy");
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
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            JournalView();
        }

        public void JournalView()
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            DateTime From_Dtae = DateTime.ParseExact(txt_FrmDate.Text, "dd/MM/yyyy", null);
            DateTime To_Dtae = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);
            if (Rdn_Credit.Checked || Rdn_Debit.Checked)
            {
                if (To_Dtae >= From_Dtae)
                {
                    lblMsg.Visible = false;
                    string NoteType = string.Empty;
                    bool Status = false;
                    if (Rdn_Ok.Checked)
                    {
                        Status = false;
                    }
                    else if (Rdn_Hold.Checked)
                    {
                        Status = true;
                    }
                    if (Rdn_Debit.Checked)
                    {
                        NoteType = "DB" + "%";
                    }
                    else if (Rdn_Credit.Checked)
                    {
                        NoteType = "CR" + "%";
                    }
                    string searchClient = string.Empty;
                    if (txt_Client.Text != "")
                        searchClient = "%" + txt_Client.Text + "%";

                    var result = dc.JournalModify_View(searchClient, NoteType, From_Dtae, To_Dtae, Status, false);
                    grdDetailsView.DataSource = result;
                    grdDetailsView.DataBind();

                }
                else
                {
                    lblMsg.Text = "ToDate should be Greater than or equal to the From Date";
                    lblMsg.Visible = true;
                    // ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('ToDate should be Greater than or equal to the From Date');", true);
                    grdDetailsView.Visible = false;
                }
            }

        }
        protected void grdDetailsView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Rdn_Credit.Checked == true || Rdn_Debit.Checked == true)
            {
                if (e.CommandName == "Modify Receipt")
                {
                    //  Session["Journal_NoteNo"] = Convert.ToString(e.CommandArgument);

                    EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                    string strURLWithData = "JournalEntry.aspx?" + obj.Encrypt(string.Format("Journal_NoteNo={0}", Convert.ToString(e.CommandArgument)));
                    //Response.Redirect(strURLWithData);
                    PrintGrid.Redirect1(strURLWithData, "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");

                    //  Response.Redirect("JournalEntry.aspx");
                }
                else if (e.CommandName == "Print Receipt")
                {
                    DateTime BillDate = DateTime.Now;
                   
                    var JOurnal = dc.Journal_View(Convert.ToString(e.CommandArgument), false, true, false).ToList();
                    foreach (var Credit in JOurnal)
                    {
                        BillDate = Convert.ToDateTime(Credit.BILL_Date_dt);
                        break;
                    }

                    PrintPDFReport rpt = new PrintPDFReport();
                    if (CheckGSTFlag(BillDate) == false)
                        rpt.Journal_PDF(Convert.ToString(e.CommandArgument));//service tax old format
                    else
                        rpt.Journal_PDF_GST(Convert.ToString(e.CommandArgument));//GST new print
                }
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
        protected void grdDetailsView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[6].Text == "False")
                {
                    e.Row.Cells[6].Text = "Ok";
                }
                if (e.Row.Cells[6].Text == "True")
                {
                    e.Row.Cells[6].Text = "Hold";
                }
                if (e.Row.Cells[6].Text == "True")
                {
                    e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                }
            }
        }
        protected void Rdn_Debit_CheckedChanged(object sender, EventArgs e)
        {
            JournalView();
        }
        protected void Rdn_Credit_CheckedChanged(object sender, EventArgs e)
        {
            JournalView();
        }
        protected void Rdn_Ok_CheckedChanged(object sender, EventArgs e)
        {
            JournalView();
        }
        protected void Rdn_Hold_CheckedChanged(object sender, EventArgs e)
        {
            JournalView();
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdDetailsView.Rows.Count > 0)
            {
                string status = "", NoteType = "";
                if (Rdn_Ok.Checked)
                {
                    status = "OK";
                }
                else if (Rdn_Hold.Checked)
                {
                    status = "Cancel";
                }
                if (Rdn_Debit.Checked)
                {
                    NoteType = "Debit Note";
                }
                else if (Rdn_Credit.Checked)
                {
                    NoteType = "Credit Note";
                }

                if (Rdn_Credit.Checked)
                {
                    //credit note format printing
                    string creditNo = ""; //, note = "";
                    DataTable dt = new DataTable();
                    DataRow dr = null;
                    dt.Columns.Add(new DataColumn("Credit Note No", typeof(string)));
                    dt.Columns.Add(new DataColumn("Credit Date", typeof(string)));
                    dt.Columns.Add(new DataColumn("Client Name", typeof(string)));
                    dt.Columns.Add(new DataColumn("Bill No", typeof(string)));
                    dt.Columns.Add(new DataColumn("Bill Date", typeof(string)));
                    dt.Columns.Add(new DataColumn("Client GST No", typeof(string)));
                    dt.Columns.Add(new DataColumn("Site GST No", typeof(string)));
                    dt.Columns.Add(new DataColumn("Credit Amount", typeof(string)));
                    dt.Columns.Add(new DataColumn("Credit Note", typeof(string)));
                    //int srNo = 0;
                    for (int i = 0; i < grdDetailsView.Rows.Count; i++)
                    {
                        //srNo = 0;
                        creditNo = grdDetailsView.Rows[i].Cells[2].Text.ToString();
                        var jrnlDt = dc.JournalDetail_View_Report(creditNo).ToList();
                        if (jrnlDt.Count > 0)
                        {
                            dr = dt.NewRow();
                            dr["Credit Note No"] = grdDetailsView.Rows[i].Cells[2].Text.ToString();
                            dr["Credit Date"] = grdDetailsView.Rows[i].Cells[3].Text.ToString();
                            dr["Client Name"] = grdDetailsView.Rows[i].Cells[5].Text.ToString();
                            dr["Credit Amount"] = grdDetailsView.Rows[i].Cells[4].Text.ToString();
                            dt.Rows.Add(dr);
                            foreach (var item in jrnlDt)
                            {
                                dr = dt.NewRow();

                                if (Convert.ToString(item.JournalDetail_BillNo_var) != "" && Convert.ToString(item.JournalDetail_BillNo_var) != null)
                                    dr["Bill No"] = Convert.ToString(item.JournalDetail_BillNo_var);
                                if (Convert.ToString(item.BILL_Date_dt) != "" && Convert.ToString(item.BILL_Date_dt) != null)
                                    dr["Bill Date"] = Convert.ToDateTime(item.BILL_Date_dt).ToString("dd-MM-yyyy");
                                if (Convert.ToString(item.BILL_CL_GSTNo_var) != "" && Convert.ToString(item.BILL_CL_GSTNo_var) != null)
                                    dr["Client GST No"] = Convert.ToString(item.BILL_CL_GSTNo_var);
                                if (Convert.ToString(item.BILL_SITE_GSTNo_var) != "" && Convert.ToString(item.BILL_SITE_GSTNo_var) != null)
                                    dr["Site GST No"] = Convert.ToString(item.BILL_SITE_GSTNo_var);

                                dr["Credit Amount"] = item.JournalDetail_Amount_dec.ToString();
                                dr["Credit Note"] = Convert.ToString(item.LedgerName_Description);

                                dt.Rows.Add(dr);
                            }
                        }

                        dr = dt.NewRow();
                        dt.Rows.Add(dr);
                    }
                    if (dt.Rows.Count > 0)
                        PrintGrid.PrintTimeReport(dt, "List_Of_Journal_CreditNote");
                }
                else
                    PrintGrid.PrintGridView(grdDetailsView, "List of Journal - " + NoteType + "(" + status + ")", "List_of_Journal_" + NoteType);
            }
        }
    }
}