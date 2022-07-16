using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public partial class TestSchdule : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Testing Schedule ";
                getCurrentDate();
            }
        }
        public void getCurrentDate()
        {
            this.txt_FromDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            this.txt_Todate.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);
            var data = dc.TestingScheduleView(ddl_InwardType.SelectedValue, Fromdate, Todate);
            grdTesting.DataSource = data;
            grdTesting.DataBind();
            if (ddl_InwardType.SelectedValue == "CT")            
                grdTesting.Columns[7].Visible = true;            
            else            
                grdTesting.Columns[7].Visible = false;
            
            //DataTable dt = new DataTable();
            //DataRow dr = null;
            //dt.Columns.Add(new DataColumn("RefNo", typeof(string)));
            //dt.Columns.Add(new DataColumn("CastingDt", typeof(string)));
            //dt.Columns.Add(new DataColumn("TestingDt", typeof(string)));
            //dt.Columns.Add(new DataColumn("Days_tint", typeof(string)));
            //dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
            //foreach (var d in data)
            //{
            //    dr = dt.NewRow();
            //    dr["RefNo"] = d.RefNo.ToString();
            //    dr["CastingDt"] = d.CastingDt.ToString();
            //    dr["TestingDt"] = d.TestingDt.ToString();
            //    if (d.RefNo.Contains("CT-") ==true )
            //        dr["Days_tint"] = d.CTINWD_Schedule_tint.ToString();
            //    else
            //        dr["Days_tint"] = d.Days_tint.ToString();
            //    dr["Quantity"] = d.Quantity.ToString();
            //    dt.Rows.Add(dr);
            //}
            //grdTesting.DataSource = dt;
            //grdTesting.DataBind();

        }
        protected void lnk_Print_Click(object sender, EventArgs e)
        {
            if (grdTesting.Rows.Count > 0 && grdTesting.Visible == true)
            {
                //string reportPath;
                string reportStr = "";
                //StreamWriter sw;
                //reportPath = Server.MapPath("~") + "\\report.html";
                //sw = File.CreateText(reportPath);
                reportStr = RptTestingScdule();
                //sw.WriteLine(reportStr);
                //sw.Close();
                //NewWindows.Redirect1("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");

                PrintHTMLReport rptHtml = new PrintHTMLReport();
                rptHtml.DownloadHtmlReport("Testing Schedule", reportStr);
            }
        }
        protected string RptTestingScdule()
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
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4> Testing Schedule For </font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=3><b> In Duration   " + txt_FromDate.Text + "  to  " + txt_Todate.Text + " </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
          
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";

            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b> Sr No. </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Reference No  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Date Of Casting </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Date Of Testing </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Schedule Of Testing </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Qty.  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Curing Tank  </b></font></td>";
            if (grdTesting.Columns[7].Visible == true)
                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Enquiry No  </b></font></td>";
            int SrNo = 0;
            for (int i = 0; i < grdTesting.Rows.Count; i++)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdTesting.Rows[i].Cells[1].Text + "</font></td>";
                mySql += "<td width =5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdTesting.Rows[i].Cells[2].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdTesting.Rows[i].Cells[3].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdTesting.Rows[i].Cells[4].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdTesting.Rows[i].Cells[5].Text + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdTesting.Rows[i].Cells[6].Text + "</font></td>";
                if (grdTesting.Columns[7].Visible == true)
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + grdTesting.Rows[i].Cells[7].Text + "</font></td>";
                mySql += "</tr>";
            }
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>" +
                "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp; User Name : </font></td>" +
                "<td width= 5% align=left valign=top height=19 ><font size=2>" + Convert.ToString(Session["LoginUserName"]) + "</font></td>" +
                "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp; Print Date : </font></td>" +
                "<td width= 5% align=left valign=top height=19 ><font size=2>" + DateTime.Now.ToString("dd/MM/yyyy") + "</font></td>" +
                "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp; Time : </font></td>" +
                "<td width= 5% align=left valign=top height=19 ><font size=2>" + DateTime.Now.ToString("HH:mm:ss tt") + "</font></td>" +
                "<tr>";
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

        protected void ddl_InwardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_InwardType.SelectedValue == "MF")
                lnk_PrintLabSheet.Visible = true;
            else
                lnk_PrintLabSheet.Visible = false;
        }

        protected void lnk_PrintLabSheet_Click(object sender, EventArgs e)
        {
            string fileName = "MF_LabSheet", reportStr = "";
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_Todate.Text, "dd/MM/yyyy", null);
            InwardReport InwdRpt = new InwardReport();
            reportStr = InwdRpt.getDetailReportMFInwardLabSheet(Fromdate, Todate);
            PrintHTMLReport rpt = new PrintHTMLReport();
            rpt.DownloadHtmlReport(fileName, reportStr);
        }

        protected void txt_FromDate_TextChanged(object sender, EventArgs e)
        {

        }
    }
}