using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for Class1
/// </summary>
public static class PrintGrid
{
    public static void Redirect(string url, string target, string windowFeatures)
    {
        HttpContext context = HttpContext.Current;

        if ((String.IsNullOrEmpty(target) ||
            target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
            String.IsNullOrEmpty(windowFeatures))
        {

            context.Response.Redirect(url);
        }
        else
        {
            Page page = (Page)context.Handler;
            if (page == null)
            {
                throw new InvalidOperationException(
                    "Cannot redirect to new window outside Page context.");
            }
            url = page.ResolveClientUrl(url);

            string script;
            if (!String.IsNullOrEmpty(windowFeatures))
            {
                script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            }
            else
            {
                script = @"window.open(""{0}"", ""{1}"");";
            }

            script = String.Format(script, url, target, windowFeatures);
            ScriptManager.RegisterStartupScript(page,
                typeof(Page),
                "Redirect",
                script,
                true);
        }
    }
    public static void SiteAllocnPrintGridView(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write("\t" + strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 0; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd.Columns.Count; col++)
            {
                if (col == 1)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblClientName")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 2)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblSiteName")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }

                if (col == 5)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblSiteAddress")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }

            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }
    public static void Redirect1(string url, string target, string windowFeatures)
    {
        HttpContext context = HttpContext.Current;

        if ((String.IsNullOrEmpty(target) ||
            target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
            String.IsNullOrEmpty(windowFeatures))
        {

            context.Response.Redirect(url);
        }
        else
        {
            Page page = (Page)context.Handler;
            if (page == null)
            {
                throw new InvalidOperationException(
                    "Cannot redirect to new window outside Page context.");
            }
            url = page.ResolveClientUrl(url);

            string script;
            if (!String.IsNullOrEmpty(windowFeatures))
            {
                script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            }
            else
            {
                script = @"window.open(""{0}"", ""{1}"");";
            }

            script = String.Format(script, url, target, windowFeatures);
            ScriptManager.RegisterStartupScript(page,
                typeof(Page),
                "Redirect",
                script,
                true);
        }
    }
    public static void PrintGridView_BusinessSnapshot(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t\t\t\t\t\t\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write(strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 0; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd.Columns.Count; col++)
            {
                if (col == 0)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 1)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfTotalEnquiries")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 2)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfAutoEnquiries")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 3)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfProposalsSentCurrent")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }

                if (col == 4)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfProposalsSent")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 5)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfOrdersReceivedCurrent")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 6)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfOrdersReceived")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 7)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkValueOfOrdersReceivedCurrent")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 8)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkValueOfOrdersReceived")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 9)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkValueOfOrdersFromNewClientCurrent")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 10)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkValueOfOrdersFromNewClient")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 11)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfEnquiriesAgainstWhichMaterialCollected")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 12)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfTestsCompleted")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 13)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfReportsApproved")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 14)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfBillsGenerated")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 15)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkValueOfBillsGenerated")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 16)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfReportsPrinted")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 17)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfBillsAndReportsReceivedByClient")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }
    public static void PrintGridView(GridView grd,string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write("\t" + strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 0; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd.Columns.Count; col++)
            {
                if (grd.Columns[col].Visible == true)
                {
                    if (grd.Rows[row].Cells[col].Text != "&nbsp;")
                    {
                        context.Response.Write(str + " " + grd.Rows[row].Cells[col].Text);
                    }
                    else
                    {
                        context.Response.Write(str);
                    }
                    str = "\t";
                }
            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }
    public static void ComplaintRegisterPrintGridView(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t\t\t\t\t\t\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write("\t" + strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 1; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 1; col < grd.Columns.Count; col++)
            {
                if (col == 1)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblCompId")).Text;
                    context.Response.Write(str1);
                    str = "\t";
                }
                if (col == 2)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblComplaintDate")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 3)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblClientName")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }

                if (col == 4)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblSiteName")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 5)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblComplaintStatus")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 6)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblComplaintType")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 7)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblComplaintDetails")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 8)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblAttendedBy")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 9)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblCOMP_RecordType")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 10)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblCOMP_ActionIntiated")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 11)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblCOMP_ClouserDate")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 12)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblCOMP_CommentOfTechOfficer")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 13)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblCOMP_ActionBy_var")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 14)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblCOMP_CreatedBy_var")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 15)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblCOMP_ReviewBy_var")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }
    public static void PrintTimeReport(DataTable grd, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty;

        context.Response.Write("\n");
        for (int col = 0; col < grd.Columns.Count; col++)
        {

            context.Response.Write(str + grd.Columns[col].ColumnName);
            str = "\t";

        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd.Columns.Count; col++)
            {

                context.Response.Write(str + " " + grd.Rows[row][col].ToString());

                str = "\t";

            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }
    public static void EquipmentPrintGridView(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t\t\t\t\t\t\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write("\t" + strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 1; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 1; col < grd.Columns.Count; col++)
            {
                if (col == 1)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtSrNo")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 2)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtEquipment")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }

                if (col == 3)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtSection")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 4)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtInternalIdMark")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col ==5)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtCalibrationStatus")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 6)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtSerialNo")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 7)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtMake")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 8)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtCertificate")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 9)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtLeastCount")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 10)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtRecdOn")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 11)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtRange")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 12)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtStatus")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
              
            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }
    public static void OutwardPrintGridView(GridView grd, string subTitle, string fileName)
    {
        //HttpContext context = HttpContext.Current;
        ////Export data to excel from gridview
        //context.Response.ClearContent();
        //context.Response.Buffer = true;
        //context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        //context.Response.ContentType = "application/ms-excel";
        ////DataTable dt = BindDatatable();
        //string str = string.Empty; string str1 = string.Empty;
        //if (subTitle.Contains("~") != true)
        //{
        //    context.Response.Write("\t\t\t\t\t\t\t" + subTitle);
        //}
        //else
        //{
        //    string[] strData = subTitle.Split('~');
        //    foreach (string strTemp in strData)
        //    {
        //        context.Response.Write("\t" + strTemp);
        //        context.Response.Write("\n");
        //    }
        //}
        //context.Response.Write("\n");
        //context.Response.Write("\n");
        ////foreach (DataColumn dtcol in dt.Columns)
        //for (int col = 1; col < grd.Columns.Count; col++)
        //{
        //    if (grd.Columns[col].Visible == true)
        //    {
        //        context.Response.Write(str + grd.Columns[col].HeaderText);
        //        str = "\t";
        //    }
        //}
        //context.Response.Write("\n");
        //for (int row = 0; row < grd.Rows.Count; row++)
        //{
        //    str = "";
        //    for (int col = 1; col < grd.Columns.Count; col++)
        //    {
        //        if (col == 1)
        //        {
        //            str1 = grd.Rows[row].Cells[col].Text;
        //            context.Response.Write(str + " " + str1);
        //            str = "\t";
        //        }
        //        if (col == 2)
        //        {
        //            str1 = grd.Rows[row].Cells[col].Text;
        //            context.Response.Write(str + " " + str1);
        //            str = "\t";
        //        }

        //        if (col == 3)
        //        {
        //            str1 = ((TextBox)grd.Rows[row].FindControl("txtOutwardBy")).Text;
        //            context.Response.Write(str + " " + str1);
        //            str = "\t";
        //        }
        //        if (col == 4)
        //        {
        //            str1 = ((TextBox)grd.Rows[row].FindControl("txtOutwardTo")).Text;
        //            context.Response.Write("\t" + " " + str1);
        //            str = "\t";
        //        }
        //        if (col == 5)
        //        {
        //            str1 = ((TextBox)grd.Rows[row].FindControl("txtOutwardDate")).Text;
        //            context.Response.Write("\t" + " " + str1);
        //            str = "\t";
        //        }
        //        if (col == 6)
        //        {
        //            str1 = ((TextBox)grd.Rows[row].FindControl("txtRegisterNo")).Text;
        //            context.Response.Write("\t" + " " + str1);
        //            str = "\t";
        //        }
        //        if (col == 7)
        //        {
        //            str1 = ((TextBox)grd.Rows[row].FindControl("txtDeliveredTo")).Text;
        //            context.Response.Write("\t" + " " + str1);
        //            str = "\t";
        //        }
        //        if (col == 8)
        //        {
        //            str1 = ((TextBox)grd.Rows[row].FindControl("txtDeliveredDate")).Text;
        //            context.Response.Write("\t" + " " + str1);
        //            str = "\t";
        //        }
        //        if (col == 9)
        //        {
        //            str1 = ((TextBox)grd.Rows[row].FindControl("txtRemark")).Text;
        //            context.Response.Write("\t" + " " + str1);
        //            str = "\t";
        //        }
        //        if (col == 11)
        //        {
        //            str1 = grd.Rows[row].Cells[col].Text;
        //            context.Response.Write("\t" + " " + str1);
        //            str = "\t";
        //        }
        //        if (col == 12)
        //        {
        //            str1 = ((Label)grd.Rows[row].FindControl("lblRecordDetail")).Text;
        //            context.Response.Write("\t" + " " + str1);
        //            str = "\t";
        //        }


        //    }
        //    context.Response.Write("\n");
        //}
        //context.Response.End();
    }
    public static void PrintGridView_FollowUp(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write("\t" + strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 0; col < grd.Columns.Count; col++)
        {
           
                if (grd.Columns[col].Visible == true)
                {
                    context.Response.Write(str + grd.Columns[col].HeaderText);
                    str = "\t";
                }
            
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd.Columns.Count; col++)
            {
                if (col == 0)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write((row+1).ToString() + " " + str1);
                    str = "\t";
                }

                if (col == 1)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblClName")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 2)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblSiteName")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }

                if (col == 3)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblRefeNo")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 4)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblReason")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 5)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblPayDate")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 6)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblAmount")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }

            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }
 
    public static void PrintGridView_MarketVisitStatus(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t\t\t\t\t\t\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write(strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 0; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd.Columns.Count; col++)
            {
                if (col == 0)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 1)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfEnquiriesGenerated")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 2)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfHotEnquiries")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }

                if (col == 3)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfWarmEnquiries")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 4)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfAutoEnquiries")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 5)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfOrdersReceived")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 6)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkValueOfOrdersReceived")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 7)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkValueOfOrdersFromNewClient")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 8)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfPendingEnquiries")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }

    public static void PrintGridView_EnquiryStatus(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t\t\t\t\t\t\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write(strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 0; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd.Columns.Count; col++)
            {
                if (col == 0)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 1)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfEnquiries")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 2)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfProposalsSentCurrent")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }

                if (col == 3)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfProposalsSent")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 4)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfOrdersReceivedCurrent")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 5)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfOrdersReceived")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 6)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkValueOfOrdersReceivedCurrent")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 7)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkValueOfOrdersReceived")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 8)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfPendingEnquiries")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }

    public static void PrintGridView_LogisticsStatus(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t\t\t\t\t\t\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write(strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 0; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd.Columns.Count; col++)
            {
                if (col == 0)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 1)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfOrdersReceived")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 2)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkMaterialCollected")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }

                if (col == 3)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkReportsPrinted")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 4)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkReportPendingForPrintingDueToCRL")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 5)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkReportsOutward")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 6)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkReportsReceivedByClient")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }

    public static void PrintGridView_SalesStatus(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t\t\t\t\t\t\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write(strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 0; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd.Columns.Count; col++)
            {
                if (col == 0 || col == 3 || col >= 7)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 1)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfOrdersReceived")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 2)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkValueOfOrdersReceived")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }

                if (col == 3)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkReportPendingForPrintingDueToCRL")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 4)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkNoOfBillsModified")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 5)
                {
                    str1 = ((LinkButton)grd.Rows[row].FindControl("lnkValueOfBillsModified")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }
    public static void PrintGridView_SalesAppReport(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write("\t" + strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 0; col < grd.Columns.Count; col++)
        {

            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }

        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd.Columns.Count; col++)
            {
                if (col == 0 && grd.Columns[col].Visible == true)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblRowNumber")).Text;
                    context.Response.Write("" + " " + str1);
                    str = "\t";
                }

                if (col == 1 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                if (col == 2 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }

                if (col == 3 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 4 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 5 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 6 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 7 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 8 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 9 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 10 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 11 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 12 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 13 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 14 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 15 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 16 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 17 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 18 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 19 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 20 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 21 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 22 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 23 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 24 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 25 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 26 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 27 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 28 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 29 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 30 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                if (col == 31 && grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }

    public static void PrintGridView_ClientListBusinessWise(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t\t\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write(strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 0; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            if (grd.Rows[row].Visible == true)
            {
                str = "";
                for (int col = 0; col < grd.Columns.Count; col++)
                {
                    if (col == 0)
                    {
                        str1 = ((Label)grd.Rows[row].FindControl("lblClientName")).Text;
                        context.Response.Write(str + " " + str1);
                        str = "\t";
                    }
                    if (col == 1 && grd.Columns[col].Visible == true)
                    {
                        str1 = ((Label)grd.Rows[row].FindControl("lblSiteName")).Text;
                        context.Response.Write(str + " " + str1);
                        str = "\t";
                    }
                    if (col == 2)
                    {
                        str1 = ((Label)grd.Rows[row].FindControl("lblAddress")).Text;
                        context.Response.Write(str + " " + str1);
                        str = "\t";
                    }

                    if (col == 3)
                    {
                        str1 = ((Label)grd.Rows[row].FindControl("lblInchargeName")).Text;
                        context.Response.Write(str + " " + str1);
                        str = "\t";
                    }
                    if (col == 4)
                    {
                        str1 = ((TextBox)grd.Rows[row].FindControl("lblInchargeEmailId")).Text;
                        context.Response.Write("\t" + " " + str1);
                        str = "\t";
                    }
                    if (col == 5)
                    {
                        str1 = ((Label)grd.Rows[row].FindControl("lblOfficeTelNo")).Text;
                        context.Response.Write("\t" + " " + str1);
                        str = "\t";
                    }
                    if (col == 6)
                    {
                        str1 = ((TextBox)grd.Rows[row].FindControl("lblClientEmailId")).Text;
                        context.Response.Write("\t" + " " + str1);
                        str = "\t";
                    }
                    if (col == 7)
                    {
                        str1 = ((Label)grd.Rows[row].FindControl("lblBusinessPrevious")).Text;
                        context.Response.Write("\t" + " " + str1);
                        str = "\t";
                    }
                    if (col == 8)
                    {
                        str1 = ((Label)grd.Rows[row].FindControl("lblBusinessCurrent")).Text;
                        context.Response.Write("\t" + " " + str1);
                        str = "\t";
                    }
                    if (col == 9)
                    {
                        str1 = ((Label)grd.Rows[row].FindControl("lblBusinessDropbyPercent")).Text;
                        context.Response.Write("\t" + " " + str1);
                        str = "\t";
                    }
                }
                context.Response.Write("\n");
            }
           
        }
        context.Response.End();
    }

    public static void PrintGridViewSiteVisitLog(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write("\t" + strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 0; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd.Columns.Count; col++)
            {
                if (col == 0)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblSrNo")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (col == 1)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblMEName")).Text;
                    context.Response.Write("\t" + " " + str1);
                    str = "\t";
                }
                else if (col == 3)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("lblCL_Name_var")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (col == 4)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("lblSITE_Name_var")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (col == 13)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("lblSITE_Address_var")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else 
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
               
            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }

    public static void PrintGridViewBillOutward(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write("\t" + strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 1; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true && col != 8 && col != 10 && col != 13)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 1; col < grd.Columns.Count; col++)
            {
                if (grd.Columns[col].Visible == true && col == 4)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtOutwardBy")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 5)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtOutwardTo")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 6)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtOutwardDate")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true &&  col == 7)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtAckDate")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 9)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblAckDate")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 11)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtBookedDate")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col != 8 && col != 10 && col != 13)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
              


            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }

    public static void PrintGridViewBillRecovery(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write("\t" + strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 1; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true && col != 9 && col != 10 && col != 17)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";

            for (int col = 1; col < grd.Columns.Count; col++)
            {

                if (grd.Columns[col].Visible == true && col == 11)
                {
                    str1 = ((DropDownList)grd.Rows[row].FindControl("ddlStatus")).Text;
                    if (str1 == "---Select---")
                        str1 = "";
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 12)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtBillBookedAmt")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 13)
                {
                    str1 = ((DropDownList)grd.Rows[row].FindControl("ddlReason")).Text;
                    if (str1 == "---Select---")
                        str1 = "";
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 14)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtRemark")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 15)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtExpectedPaymentDate")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 16)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtExpectedAmt")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col != 9 && col != 10 && col != 17)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    if (str1 == "&nbsp;")
                        str1 = "";
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }
    
    public static void PrintGridViewRecoveryAging(GridView grd, GridView grd2, GridView grd3, string subTitle, string subTitle2, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty;
        context.Response.Write("\t" + subTitle);
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 0; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd.Columns.Count; col++)
            {
                if (grd.Columns[col].Visible == true)
                {
                    if (grd.Rows[row].Cells[col].Text != "&nbsp;")
                    {
                        context.Response.Write(str + " " + grd.Rows[row].Cells[col].Text);
                    }
                    else
                    {
                        context.Response.Write(str);
                    }
                    str = "\t";
                }
            }
            context.Response.Write("\n");
        }
        str = "";
        for (int col = 0; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.FooterRow.Cells[col].Text);
                str = "\t";
            }
        }
        context.Response.Write("\n");

        context.Response.Write("\n");
        context.Response.Write("\n");
        context.Response.Write("\t" + subTitle2);
        context.Response.Write("\n");
        context.Response.Write("\n");
        str = "";
        for (int col = 0; col < grd2.Columns.Count; col++)
        {
            if (grd2.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd2.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd2.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd2.Columns.Count; col++)
            {
                if (grd2.Columns[col].Visible == true)
                {
                    if (grd2.Rows[row].Cells[col].Text != "&nbsp;")
                    {
                        context.Response.Write(str + " " + grd2.Rows[row].Cells[col].Text);
                    }
                    else
                    {
                        context.Response.Write(str);
                    }
                    str = "\t";
                }
            }
            context.Response.Write("\n");
        }
        str = "";
        for (int col = 0; col < grd2.Columns.Count; col++)
        {
            if (grd2.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd2.FooterRow.Cells[col].Text);
                str = "\t";
            }
        }
        context.Response.Write("\n");

        context.Response.Write("\n");
        str = "";
        for (int col = 0; col < grd3.Columns.Count; col++)
        {
            if (grd3.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd3.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd3.Rows.Count; row++)
        {
            str = "";
            for (int col = 0; col < grd3.Columns.Count; col++)
            {
                if (grd3.Columns[col].Visible == true)
                {
                    if (grd3.Rows[row].Cells[col].Text != "&nbsp;")
                    {
                        context.Response.Write(str + " " + grd3.Rows[row].Cells[col].Text);
                    }
                    else
                    {
                        context.Response.Write(str);
                    }
                    str = "\t";
                }
            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }
    public static void PrintGridViewProposalList(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty; string str1 = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write("\t" + strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        //foreach (DataColumn dtcol in dt.Columns)
        for (int col = 6; col < grd.Columns.Count; col++)
        {
            if (grd.Columns[col].Visible == true)
            {
                context.Response.Write(str + grd.Columns[col].HeaderText);
                str = "\t";
            }
        }
        context.Response.Write("\n");
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            str = "";

            for (int col = 6; col < grd.Columns.Count; col++)
            {
                if (grd.Columns[col].Visible == true && col == 6)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblProposalNo")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 7)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblEnquiryId")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 8)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblEnquiryDate")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 9)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblProposalDate")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 10)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblProposalAmt")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 11)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblMaterialName")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 12)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblClientName")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 13)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblSiteName")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 14)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblEnquiryStatus")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 15)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblContctName")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 16)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblContctNo")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 17)
                {
                    str1 = ((Label)grd.Rows[row].FindControl("lblOrderStatus")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 18)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtEmailIdTo")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true && col == 19)
                {
                    str1 = ((TextBox)grd.Rows[row].FindControl("txtEmailIdCc")).Text;
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
                else if (grd.Columns[col].Visible == true)
                {
                    str1 = grd.Rows[row].Cells[col].Text;
                    if (str1 == "&nbsp;")
                        str1 = "";
                    context.Response.Write(str + " " + str1);
                    str = "\t";
                }
            }
            context.Response.Write("\n");
        }
        context.Response.End();
    }

    public static void PrintGridView_BillAckPendingList(GridView grd, string subTitle, string fileName)
    {
        HttpContext context = HttpContext.Current;
        //Export data to excel from gridview
        context.Response.ClearContent();
        context.Response.Buffer = true;
        context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
        context.Response.ContentType = "application/ms-excel";
        //DataTable dt = BindDatatable();
        string str = string.Empty;
        if (subTitle.Contains("~") != true)
        {
            context.Response.Write("\t" + subTitle);
        }
        else
        {
            string[] strData = subTitle.Split('~');
            foreach (string strTemp in strData)
            {
                context.Response.Write("\t" + strTemp);
                context.Response.Write("\n");
            }
        }
        context.Response.Write("\n");
        context.Response.Write("\n");
        
        context.Response.Write(str + "Sr. No.");
        str = "\t";
        context.Response.Write(str + "Client Name");
        context.Response.Write(str + "Site Name");
        context.Response.Write(str + "Bill No.");
        context.Response.Write(str + "Bill Date");
        context.Response.Write(str + "Testing Type");
        context.Response.Write(str + "Bill Amount");
        context.Response.Write(str + "Pending Amount");

        context.Response.Write("\n");
        int srNo = 1;
        for (int row = 0; row < grd.Rows.Count; row++)
        {
            if (grd.Rows[row].Cells[21].Text == "Pending")
            {
                str = "";
                context.Response.Write(str + " " + srNo.ToString());
                str = "\t";
                context.Response.Write(str + " " + grd.Rows[row].Cells[1].Text);
                context.Response.Write(str + " " + grd.Rows[row].Cells[2].Text);
                context.Response.Write(str + " " + grd.Rows[row].Cells[3].Text);
                context.Response.Write(str + " " + grd.Rows[row].Cells[4].Text);
                context.Response.Write(str + " " + grd.Rows[row].Cells[5].Text);
                context.Response.Write(str + " " + grd.Rows[row].Cells[6].Text);
                context.Response.Write(str + " " + grd.Rows[row].Cells[7].Text);
                context.Response.Write("\n");
                srNo++;
            }
        }
        context.Response.End();
    }
}
