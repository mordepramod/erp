using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class Cube28DaysTestingDue : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ViewState["ReffUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "28 Days Testing Due List";
                txt_FromDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            DisplayReportList();
        }

        public void DisplayReportList()
        {
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);

            var report = dc.CubeInward_View_28DaysTestingDue(Fromdate, Todate);
            grdReportList.DataSource = report;
            grdReportList.DataBind();
            lblTotalRecords.Text = "Total Records   :  " + grdReportList.Rows.Count;            
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdReportList.Rows.Count > 0 && grdReportList.Visible == true)
            {
                string reportStr = "";
                reportStr = RptReportList();
                PrintHTMLReport rptHtml = new PrintHTMLReport();
                rptHtml.DownloadHtmlReport("28DaysDue", reportStr);
            }
        }

        protected string RptReportList()
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
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> 28 Days Testing Due List </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b>  Mat. Recd  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + " - " + DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yy") + "</font></td>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Total No of Records  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + grdReportList.Rows.Count + "</font></td>" +
               "</tr>";


            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";

            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Client Name </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Site Name  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Contact Person </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Contact No. </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Record Type </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Record No  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Reference No </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Schedule </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Quantity </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Casting Date </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Testing Date </b></font></td>";

            for (int i = 0; i < grdReportList.Rows.Count; i++)
            {
                mySql += "<tr>";
                mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;" + grdReportList.Rows[i].Cells[0].Text + "</font></td>";
                mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;" + grdReportList.Rows[i].Cells[1].Text + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + grdReportList.Rows[i].Cells[2].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportList.Rows[i].Cells[3].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportList.Rows[i].Cells[4].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportList.Rows[i].Cells[5].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportList.Rows[i].Cells[6].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportList.Rows[i].Cells[7].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportList.Rows[i].Cells[8].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportList.Rows[i].Cells[9].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdReportList.Rows[i].Cells[10].Text + "</font></td>";
                mySql += "</tr>";
            }
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
            //object refUrl = ViewState["RefUrl"];
            //if (refUrl != null)
            //{
            //    Response.Redirect((string)refUrl);
            //}
            Response.Redirect("Home.aspx");
        }


    }
}