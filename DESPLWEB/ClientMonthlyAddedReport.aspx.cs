using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class ClientMonthlyAddedReport : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Datewise Monthly Client Added List";
                txt_FromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }
                
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdClientList.Visible = true;
            LoadClientList();
        }
           
        public void LoadClientList()
        {
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);

            var clientlist = dc.Client_View_DatewiseMonthlyAddedList(Fromdate, Todate);
            grdClientList.DataSource = clientlist;
            grdClientList.DataBind();           
            lbl_RecordsNo.Text = "Total Records   :  " + grdClientList.Rows.Count;            
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdClientList.Rows.Count > 0 && grdClientList.Visible == true)
            {
                string reportStr = "";
                reportStr = rptClientList();
                PrintHTMLReport rptHtml = new PrintHTMLReport();
                rptHtml.DownloadHtmlReport("ClientList", reportStr);
            }
        }

        protected string rptClientList()
        {
            string reportStr = "", mySql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Datewise Monthly Client List</b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b>  Period  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + " - " + DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + "</font></td>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Total No of Records  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + grdClientList.Rows.Count + "</font></td>" +
               "</tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 50% align=Center valign=medium height=19 ><font size=2><b> Client Name </b></font></td>";
            //mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Date  </b></font></td>";
            mySql += "</tr>";

            for (int i = 0; i < grdClientList.Rows.Count; i++)
            {
                mySql += "<tr>";
                mySql += "<td width= 50% align=Left valign=top height=19 ><font size=2>&nbsp;" + grdClientList.Rows[i].Cells[0].Text + "</font></td>";
                //mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdClientList.Rows[i].Cells[1].Text + "</font></td>";              
                mySql += "</tr>";
            }
            mySql += "</table>";

            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += "</table>";
            mySql += "</html>";
            return reportStr = mySql;

        }


    }
}