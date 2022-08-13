using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data;
using System.Text;

namespace DESPLWEB
{
    public class PrintHTMLReport
    {
        LabDataDataContext dc = new LabDataDataContext();
        static string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
        public void DownloadHtmlReport(string fileName, string reportStr)
        {
            if (System.Web.HttpContext.Current.Session["LoginId"] != null)
                fileName = fileName + "_" + System.Web.HttpContext.Current.Session["LoginId"] + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".html";
            else
                fileName = fileName + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".html";

            string reportPath = "C:/temp/" + fileName;

            //Process process = new Process();
            //process.StartInfo.UseShellExecute = true;
            //process.StartInfo.FileName = reportPath;
            //process.Start();
            //process.WaitForExit();// Waits here for the process to exit.
            StreamWriter sw;
            sw = File.CreateText(reportPath);
            sw.WriteLine(reportStr);
            sw.Close();
            System.Web.HttpContext.Current.Response.ContentType = "text/HTML";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            System.Web.HttpContext.Current.Response.TransmitFile(reportPath);
            System.Web.HttpContext.Current.Response.End();
        }
        private string reportAuthenticateCode()
        {
            string authCode = "00";

            if (cnStr.ToLower().Contains("mumbai") == true)
                authCode = "02";
            else if (cnStr.ToLower().Contains("nashik") == true)
                authCode = "03";
            else
                authCode = "01";///pune
            ///

            return authCode;
        }
      
        public void AggregateReport_Html(string ReferenceNo, int materialId, string chkStatus)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            string AggregateType = string.Empty;
            string AggregateName = string.Empty;
            string WitnessBy = string.Empty;
            var AggtTest = dc.ReportStatus_View("Aggregate Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);
            foreach (var aggt in AggtTest)
            {
                AggregateName = aggt.AGGTINWD_AggregateName_var.ToString();
                if (Convert.ToString(aggt.AGGTINWD_AggregateName_var) == "10 mm" || Convert.ToString(aggt.AGGTINWD_AggregateName_var) == "20 mm" || Convert.ToString(aggt.AGGTINWD_AggregateName_var) == "40 mm")
                {
                    AggregateType = "Coarse Aggregate";
                }
                else
                {
                    AggregateType = "Fine Aggregate";
                }
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>" + AggregateType + "   " + "(" + Convert.ToString(aggt.AGGTINWD_AggregateName_var) + ")" + " </b></font></td></tr>";

                mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

                mySql += "<tr>" +
                    "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                     "<td width='40%' height=19><font size=2>" + aggt.CL_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "-" + "</font></td>" +
                    "<td height=19><font size=2>&nbsp;</font></td>" +
                    "</tr>";


                mySql += "<tr>" +
                     "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
                     "<td height=19><font size=2></font></td>" +
                    "<td width='40%' height=19><font size=2>" + aggt.CL_OfficeAddress_var + "</font></td>" +
                     "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "AGGT" + "-" + aggt.AGGTINWD_SetOfRecord_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +

                  "<td width='20%' height=19><font size=2><b></b></font></td>" +
                  "<td width='2%' height=19><font size=2></font></td>" +
                  "<td width='10%' height=19><font size=2></font></td>" +
                  "<td height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                  "<td height=19><font size=2>:</font></td>" +
                  "<td height=19><font size=2>" + "AGGT" + "-" + aggt.AGGTINWD_ReferenceNo_var.ToString() + "</font></td>" +
                  "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + aggt.SITE_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "DT" + "-" + " " + "</font></td>" +
                   "</tr>";

                mySql += "<tr>" +

                     "<td width='20%' height=19><font size=2>  </font></td>" +
                     "<td width='2%' height=19><font size=2> </font></td>" +
                     "<td width='10%' height=19><font size=2></font></td>" +
                     "<td height=19><font size=2><b>Date of receipt </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + Convert.ToDateTime(aggt.INWD_ReceivedDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
                    "</tr>";
                WitnessBy = aggt.AGGTINWD_WitnessBy_var.ToString();
                break;

            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>&nbsp;</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "</table>";

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";
            bool SpecGrav = false;
            bool lbd = false;
            bool Moist = false;
            bool Sild = false;
            bool Sieve = false;
            bool Impact = false;
            bool Elong = false;
            bool Crush = false;
            bool Flaki = false;
            var aggtTestname = dc.AllInwdDetails_View(ReferenceNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "AGGT");
            foreach (var aggtt in aggtTestname)
            {
                if (aggtt.TEST_Sr_No == 1)
                {
                    Sieve = true;
                }
                if (aggtt.TEST_Sr_No == 3)
                {
                    SpecGrav = true;
                }
                if (aggtt.TEST_Sr_No == 4)
                {
                    lbd = true;
                }
                if (aggtt.TEST_Sr_No == 9)
                {
                    Moist = true;
                }
                if (aggtt.TEST_Sr_No == 2)
                {
                    Sild = true;
                }
                if (aggtt.TEST_Sr_No == 7)
                {
                    Impact = true;
                }
                if (aggtt.TEST_Sr_No == 6)
                {
                    Elong = true;
                }
                if (aggtt.TEST_Sr_No == 8)
                {
                    Crush = true;
                }
                if (aggtt.TEST_Sr_No == 5)
                {
                    Flaki = true;
                }
            }
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
            mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
            mySql += "<td width= 2% align=left  valign=top height=19 rowspan=5  ><font size=2> " + " " + " </font></td>";
            mySql += "<td width= 3% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
            mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
            mySql += "</tr>";

            var Inward_aggt = dc.ReportStatus_View("Aggregate Testing", null, null, 1, 0, 0, ReferenceNo, 0, 0, 0);
            foreach (var aggt in Inward_aggt)
            {
                if (aggt.AGGTINWD_AggregateName_var == "Natural Sand" || aggt.AGGTINWD_AggregateName_var == "Crushed Sand" || aggt.AGGTINWD_AggregateName_var == "Stone Dust" || aggt.AGGTINWD_AggregateName_var == "Grit")
                {
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Specific Gravity" + "</font></td>";
                    if (SpecGrav == true)
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SpecificGravity_var) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                    }
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";

                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Moisture Content" + " </font></td>";
                    if (Moist == true)
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_MoistureContent_var) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                    }
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " % " + "</font></td>";
                    mySql += "</tr>";

                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Water Absorption" + "</font></td>";
                    if (SpecGrav == true)
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_WaterAborp_var) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                    }
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "% " + "</font></td>";

                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Condition of the Sample" + "</font></td>";

                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SampleCondition_var) + "</font></td>";

                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "  " + "</b></font></td>";
                    mySql += "</tr>";
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=middle height=19 ><font size=2>&nbsp;" + "Loose Bulk Density" + "</font></td>";
                    if (lbd == true)
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_LBD_var) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                    }
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "kg/lit " + "</font></td>";

                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Material finer than 75 u </br> (by wet sieving)" + "</font></td>";
                    if (Sild == true)
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SildContent_var) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                    }
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " %  " + "</font></td>";
                    mySql += "</tr>";
                }
                if (aggt.AGGTINWD_AggregateName_var == "10 mm" || aggt.AGGTINWD_AggregateName_var == "20 mm" || aggt.AGGTINWD_AggregateName_var == "40 mm" || aggt.AGGTINWD_AggregateName_var == "Mix Aggt")
                {
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Specific Gravity" + "</font></td>";
                    if (SpecGrav == true)
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SpecificGravity_var) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                    }
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";

                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Impact Value" + " </font></td>";
                    if (Impact == true)
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_ImpactValue_var) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                    }
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "  " + "</font></td>";
                    mySql += "</tr>";

                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Water Absorption" + "</font></td>";
                    if (SpecGrav == true)
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_WaterAborp_var) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                    }
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "% " + "</font></td>";

                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Condition of the Sample" + "</font></td>";

                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SampleCondition_var) + "</font></td>";

                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "  " + "</b></font></td>";
                    mySql += "</tr>";
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=middle height=19 ><font size=2>&nbsp;" + "Loose Bulk Density" + "</font></td>";
                    if (lbd == true)
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_LBD_var) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                    }
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "kg/lit " + "</font></td>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Flakiness Value" + "</font></td>";
                    if (Flaki == true)
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_Flakiness_var) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                    }
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " " + "</font></td>";
                    mySql += "</tr>";
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Elongness Value" + "</font></td>";
                    if (Elong == true)
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_Elongation_var) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                    }
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Crushing Value" + "</font></td>";
                    if (Crush == true)
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_CrushingValue_var) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                    }
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "  " + "</font></td>";
                    mySql += "</tr>";
                }
            }
            mySql += "</table>";

            if (Sieve == true)
            {
                mySql += "<table>";
                mySql += "<tr>";
                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "Sieve Analysis (by dry sieving) " + "</b></font></td>";
                mySql += "<td width= 3% align=left valign=top height=19 ><font size=2><b>" + AggregateName + "</b></font></td>";
                mySql += "</tr>";
                mySql += "</table>";
                mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

                int i = 0;
                var aggtTest = dc.AggregateAllTestView(ReferenceNo, materialId, "AGGTSA");
                foreach (var aggtt in aggtTest)
                {
                    if (i == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2%  rowspan=2 align=center valign=middle height=19 ><font size=2><b>Sieve Size</b></font></td>";
                        mySql += "<td width= 10%  align=center colspan=3 valign=top height=19 ><font size=2><b>Weight retained</b></font></td>";
                        mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2><b>Passing </b></font></td>";
                        if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
                        {
                            mySql += "<td width= 2%   align=center rowspan=2 valign=middle height=19 ><font size=2><b>IS Passing % Limits </b></font></td>";
                        }
                        mySql += "</tr>";
                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(g)</b></font></td>";
                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(%)</b></font></td>";
                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>Cummu (%)</b></font></td>";
                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(%)</b></font></td>";
                    }
                    mySql += "<tr>";
                    mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_SeiveSize_var.ToString() + "</font></td>";
                    mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_Weight_num.ToString() + "</font></td>";
                    if (aggtt.AGGTSA_SeiveSize_var != "Total")
                    {
                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + Convert.ToDecimal(aggtt.AGGTSA_WeightRet_dec).ToString("0.00") + "</font></td>";
                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_CumuWeightRet_dec.ToString() + "</font></td>";
                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_CumuPassing_dec.ToString() + "</font></td>";
                        if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
                        {
                            mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2> " + aggtt.AGGTSA_IsPassingLmt_var.ToString() + " </font></td>";
                        }
                    }
                    else if (aggtt.AGGTSA_SeiveSize_var == "Total")
                    {
                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + "" + "</font></td>";
                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + "Fineness Modulus" + "</font></td>";
                        mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTINWD_FM_var.ToString() + "</font></td>";
                        if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
                        {
                            mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2> " + "" + " </font></td>";
                        }
                    }
                    i++;
                }
                mySql += "</tr>";
                mySql += "</table>";
            }


            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";


            int SrNo = 0;
            var matid = dc.Material_View("AGGT", "");
            foreach (var m in matid)
            {
                var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
                foreach (var cd in iscd)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }
            SrNo = 0;
            var re = dc.AllRemark_View("", ReferenceNo, 0, "AGGT");
            foreach (var r in re)
            {
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.AGGTDetail_RemarkId_int), "AGGT");
                foreach (var remk in remark)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.AGGT_Remark_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            //if (lblEntry.Text == "Check")
            if (chkStatus == "Check")
            {
                var RecNo = dc.ReportStatus_View("Aggregate Testing", null, null, 1, 0, 0, ReferenceNo, 0, 0, 0);
                foreach (var r in RecNo)
                {
                    if (Convert.ToString(r.AGGTINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.AGGTINWD_CheckedBy_tint) != string.Empty)
                    {
                        var U = dc.User_View(r.AGGTINWD_ApprovedBy_tint, -1, "", "", "");
                        foreach (var r1 in U)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
                            mySql += "</tr>";
                        }
                        mySql += "<tr>";
                        if (WitnessBy != string.Empty)
                        {
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> Witness by : " + WitnessBy + " </font></td>";
                        }
                        var lgin = dc.User_View(r.AGGTINWD_CheckedBy_tint, -1, "", "", "");
                        foreach (var loginusr in lgin)
                        {

                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                            mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";

                        }
                        mySql += "</tr>";
                    }
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999CRC014212" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
            mySql += "</tr>";

            mySql += "</table>";

            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("Aggregate_Report", mySql);
        }
        public void NDTReport_Html(string ReferenceNo, string chkStatus)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Non Destructive Testing of R.C.C. </b></font></td></tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


            string NdtBy_type = string.Empty;
            var NDT_Test = dc.ReportStatus_View("Non Destructive Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);
            foreach (var NDTtest in NDT_Test)
            {
                mySql += "<tr>" +
                "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td width='40%' height=19><font size=2>" + NDTtest.CL_Name_var + "</font></td>" +
                "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td height=19><font size=2>" + "-" + "</font></td>" +
                "<td height=19><font size=2>&nbsp;</font></td>" +
                "</tr>";


                mySql += "<tr>" +
                     "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
                     "<td height=19><font size=2></font></td>" +
                    "<td width='40%' height=19><font size=2>" + NDTtest.CL_OfficeAddress_var + "</font></td>" +
                     "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "NDT" + "-" + NDTtest.NDTINWD_SetOfRecord_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + NDTtest.SITE_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "NDT" + "-" + " " + "</font></td>" +
                   "</tr>";

                mySql += "<tr>" +
                    "<td align=left valign=top height=19><font size=2><b> Kind Attention </b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + NDTtest.NDTINWD_KindAttention_var + "</font></td>" +
                    "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + Convert.ToDateTime(NDTtest.NDTINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
                   "</tr>";
                NdtBy_type = NDTtest.NDTINWD_NDTBy_var.ToString();
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            if (NdtBy_type.Trim() == "UPV" || NdtBy_type.Trim() == "UPV with Grading")
            {
                mySql += "<td width= 2%  rowspan=1 align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
                mySql += "<td width= 10% rowspan=1 align=center valign=top height=19 ><font size=2><b>Location & </br> Identification</b></font></td>";
                mySql += "<td width= 5%  rowspan=1 align=center valign=top height=19 ><font size=2><b>Grade of </br> Concrete </b></font></td>";
                mySql += "<td width= 5% rowspan=1 align=center valign=top height=19 ><font size=2><b>Date of  </br> Casting </b></font></td>";
                mySql += "<td width= 2% rowspan=1 align=center valign=top height=19 ><font size=2><b>Age </br>  </br> (Days)</b></font></td>";

                if (NdtBy_type.Trim() != "Rebound Hammer")
                {
                    mySql += "<td width= 5% rowspan=1 align=center valign=top height=19 ><font size=2><b>Pulse  </br> Velocity </br>(km/s)</b></font></td>";
                }
                if (NdtBy_type.Trim() == "UPV with Grading")
                {
                    mySql += "<td width= 5% rowspan=1 align=center valign=top height=19 ><font size=2><b>Concrete </br> quality </br> grading</b></font></td>";
                }
                else
                {
                    mySql += "<td width= 5% rowspan=1 align=center valign=top height=19 ><font size=2><b>Indicative </br> Strength </br> (N/mm<sup>2</sup>)</b></font></td>";
                }
                mySql += "</tr>";
            }
            else
            {
                mySql += "<td width= 2% rowspan=2 align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
                mySql += "<td width= 15% rowspan=2 align=center valign=top height=19 ><font size=2><b>Location & </br> Identification</b></font></td>";
                mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Grade of </br> Concrete </b></font></td>";
                mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Date of  </br> Casting </b></font></td>";
                mySql += "<td width= 2% rowspan=2 align=center valign=top height=19 ><font size=2><b>Age </br> </br> </br> (Days)</b></font></td>";
                if (NdtBy_type.Trim() != "UPV with Grading" && NdtBy_type.Trim() != "UPV")
                {
                    mySql += "<td width= 2% colspan=2  align=center valign=top height=19 ><font size=2><b>Mech. Sclerometer </br> (Rebound Hammer) </b></font></td>";
                }
                if (NdtBy_type.Trim() != "Rebound Hammer")
                {
                    mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Pulse  </br> Velocity  </br> </br>(km/s)</b></font></td>";
                }
                if (NdtBy_type.Trim() == "UPV with Grading")
                {
                    mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Concrete </br> quality </br> grading</b></font></td>";
                }
                else
                {
                    mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Indicative  </br> Strength </br> </br> (N/mm<sup>2</sup>)</b></font></td>";
                }
                mySql += "</tr>";
                if (NdtBy_type.Trim() != "UPV with Grading" && NdtBy_type.Trim() != "UPV")
                {
                    mySql += "<td width= 5%  align=center valign=top height=19 ><font size=2><b>Angle of inclination</b></font></td>";
                    mySql += "<td width= 5%  align=center valign=top height=19 ><font size=2><b>Avearge Reading</b></font></td>";
                }
            }
            int SrNo = 0;
            var NDT_Testing = dc.TestDetail_Title_View(ReferenceNo, 0, "NDT", false);
            foreach (var Ndt in NDT_Testing)
            {
                SrNo++;
                if (Convert.ToString(Ndt.Description_var) != "")
                {
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.Description_var) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.Grade_var) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.Castingdate_var) + "</font></td>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.Age_var) + "</font></td>";
                    if (NdtBy_type.Trim() != "UPV with Grading" && NdtBy_type.Trim() != "UPV")
                    {
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.AlphaAngle_var).Replace("°", "") + "<sup>0</sup> </font></td>";
                    }
                    if (Convert.ToString(Ndt.ReboundIndex_var) != "")
                    {
                        string[] Rebound = Convert.ToString(Ndt.ReboundIndex_var).Split('|');
                        foreach (var RebdIndex in Rebound)
                        {
                            if (RebdIndex != "")
                            {
                                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + RebdIndex + "</font></td>";
                                break;
                            }
                        }
                    }
                    if (NdtBy_type.Trim() != "Rebound Hammer")
                    {
                        if (Convert.ToString(Ndt.PulseVelocity_var) != "")
                        {
                            string[] PulseVelc = Convert.ToString(Ndt.PulseVelocity_var).Split('|');
                            foreach (var Pulse in PulseVelc)
                            {
                                if (Pulse != "")
                                {
                                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Pulse + "</font></td>";
                                    break;
                                }
                            }
                        }
                    }

                    if (Ndt.EditedIndStr_var != null && Ndt.EditedIndStr_var != string.Empty)
                    {
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.EditedIndStr_var) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.IndicativeStrength_var) + "</font></td>";
                    }
                    mySql += "</tr>";
                }
                else
                {
                    if (Convert.ToString(Ndt.TitleId_int) != "")
                    {
                        if (Convert.ToInt32(Ndt.TitleId_int) > 0)
                        {
                            var crr = dc.TestDetail_Title_View(ReferenceNo, Convert.ToInt32(Ndt.TitleId_int), "NDT", false);
                            foreach (var title in crr)
                            {
                                mySql += "<tr>";
                                mySql += "<td width= 10% colspan=12 align=center valign=top height=19 ><font size=2> <b>&nbsp;" + Convert.ToString(title.TitleDesc_var) + "</b></font></td>";
                                mySql += "</tr>";
                                SrNo--;
                                break;
                            }
                        }
                    }
                }
            }
            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";


            SrNo = 0;
            var matid = dc.Material_View("NDT", "");
            foreach (var m in matid)
            {
                var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
                foreach (var cd in iscd)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }
            SrNo = 0;
            var re = dc.AllRemark_View("", ReferenceNo, 0, "NDT");
            foreach (var r in re)
            {
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.NDTDetail_RemarkId_int), "NDT");
                foreach (var remk in remark)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Uncertainty levels :" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.NDT_Remark_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            //if (lblEntry.Text == "Check")
            if (chkStatus == "Check")
            {
                var RecNo = dc.ReportStatus_View("Non Destructive Testing", null, null, 1, 0, 0, ReferenceNo, 0, 0, 0);
                foreach (var r in RecNo)
                {
                    if (Convert.ToString(r.NDTINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.NDTINWD_CheckedBy_tint) != string.Empty)
                    {
                        var U = dc.User_View(r.NDTINWD_ApprovedBy_tint, -1, "", "", "");
                        foreach (var r1 in U)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
                            mySql += "</tr>";
                        }
                        var lgin = dc.User_View(r.NDTINWD_CheckedBy_tint, -1, "", "", "");
                        foreach (var loginusr in lgin)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                            mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                        }
                    }
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999CRC014212" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
            mySql += "</tr>";

            mySql += "</table>";


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("NDT_Report", mySql);
        }
        public void CementReport_Html(string ReferenceNo, string strGrade, string chkStatus)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Cement Testing</b></font></td></tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var Cement = dc.ReportStatus_View("Cement Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);
            foreach (var w in Cement)
            {
                mySql += "<tr>" +
                      "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td width='40%' height=19><font size=2>" + w.CL_Name_var + "</font></td>" +
                      "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + "-" + "</font></td>" +// DateTime.Now.ToString("dd/MM/yy")
                      "<td height=19><font size=2>&nbsp;</font></td>" +
                      "</tr>";

                mySql += "<tr>" +

                     "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
                     "<td height=19><font size=2></font>:</td>" +
                     "<td width='40%' height=19><font size=2>" + w.CL_OfficeAddress_var + "</font></td>" +
                     "<td height=19><font size=2><b>Record No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "CEMT" + "-" + w.CEMTINWD_SetOfRecord_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +
                    "<td width='20%' height=19><font size=2><b></b></font></td>" +
                    "<td width='2%' height=19><font size=2></font></td>" +
                    "<td width='10%' height=19><font size=2></font></td>" +
                     "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "CEMT" + "-" + w.CEMTINWD_ReferenceNo_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + w.SITE_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "CEMT" + "-" + "" + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Cement Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + w.CEMTINWD_CementName_var + "</font></td>" +
                    "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + Convert.ToDateTime(w.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                      "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + w.CEMTINWD_Description_var.ToString() + "</font></td>" +
                      "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(w.CEMTINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                mySql += "<tr>" +
                    "<td align=left valign=top height=19><font size=2><b>Supplier Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + w.CEMTINWD_SupplierName_var.ToString() + "</font></td>" +
                    "<td align=left valign=top height=19></td>" +
                    "<td height=19></td>" +
                    "<td height=19></td>" +
                    "</tr>";

            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";


            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "RESULTS : " + "</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";


            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Name Of Test</b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Unit </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Result </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Specified Limits </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Compliance </b></font></td>";
            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Method Of Testing </b></font></td>";
            mySql += "</tr>";

            int SrNo = 0;

            var details = dc.AllInwdDetails_View(ReferenceNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "CEMT");
            foreach (var CEMT in details)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                bool valid = false;
                string TEST_Name_var = "";

                if (CEMT.TEST_Name_var.ToString() == "Compressive Strength")
                {
                    if (CEMT.CEMTTEST_Days_tint.ToString() != "" && CEMT.CEMTTEST_Days_tint.ToString() != null && CEMT.CEMTTEST_Days_tint.ToString() != "0")
                    {
                        TEST_Name_var = " " + "(" + "" + CEMT.CEMTTEST_Days_tint.ToString() + " " + "Days" + " " + ")" + " " + CEMT.TEST_Name_var.ToString();
                        mySql += "<td width=20% align=left valign=top height=19 ><font size=2>&nbsp;" + TEST_Name_var + "</font></td>";
                    }
                }
                else
                {
                    mySql += "<td width=20% align=left valign=top height=19 ><font size=2>&nbsp;" + CEMT.TEST_Name_var.ToString() + "</font></td>";
                }
                var Id = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, true, CEMT.TEST_Id, "", 0, 0, 0, 0, 0, "", 0, "CEMT");
                foreach (var testid in Id)
                {
                    valid = true;
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(testid.splmt_Unit_var) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(CEMT.CEMTTEST_Result_var) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(testid.splmt_SpecifiedLimit_var) + "</font></td>";
                    int SpecifiedLmtRes = 0;
                    bool validmax = false;
                    string res = "";

                    string[] SpceifiedLmt = Convert.ToString(testid.splmt_SpecifiedLimit_var).Split(' ', ',');
                    foreach (var Comp in SpceifiedLmt)
                    {
                        if (Comp != "")
                        {
                            if (Comp.Trim() == "Maximum")
                            {
                                validmax = true;
                            }
                            if (Comp.Trim() == "PCC" || Comp.Trim() == "RCC")
                            {
                                res = res + " " + "-" + " " + Comp + "</br>";
                            }
                            if (int.TryParse(Comp, out SpecifiedLmtRes))
                            {
                                SpecifiedLmtRes = Convert.ToInt32(Comp.ToString());
                                if (validmax == true)
                                {
                                    if (Convert.ToString(CEMT.CEMTTEST_Result_var).Trim() != "Awaited" && Convert.ToString(CEMT.CEMTTEST_Result_var).Trim() != "*" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "---" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "" && Convert.ToString(testid.splmt_SpecifiedLimit_var).Trim() != "Not Specified")
                                    {
                                        if ((Convert.ToDouble(CEMT.CEMTTEST_Result_var)) < Convert.ToInt32(SpecifiedLmtRes))
                                        {
                                            res = res + "Pass ";
                                            //  mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "Pass" + "</font></td>";
                                        }
                                        else
                                        {
                                            res = res + "Fail ";
                                            //  mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "Fail" + "</font></td>";
                                        }
                                    }
                                    else
                                    {
                                        res = "---";
                                        //mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "---" + "</font></td>";
                                    }

                                }
                                else
                                {
                                    if (Convert.ToString(CEMT.CEMTTEST_Result_var).Trim() != "Awaited" && Convert.ToString(CEMT.CEMTTEST_Result_var).Trim() != "*" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "---" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "" && Convert.ToString(testid.splmt_SpecifiedLimit_var).Trim() != "Not Specified")
                                    {
                                        if ((Convert.ToDouble(CEMT.CEMTTEST_Result_var)) > Convert.ToInt32(SpecifiedLmtRes))
                                        {
                                            res = res + "Pass ";
                                            // mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "Pass" + "</font></td>";
                                        }
                                        else
                                        {
                                            res = res + "Fail ";
                                            // mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "Fail" + "</font></td>";
                                        }
                                    }
                                    else
                                    {
                                        res = "---";
                                        // mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "---" + "</font></td>";
                                    }

                                }
                            }

                        }
                    }
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + res + "</font></td>";
                    mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>" + Convert.ToString(testid.splmt_testingMethod_var) + "</font></td>";
                    break;
                }
                if (valid == false)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(CEMT.CEMTTEST_Result_var) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
                }
                mySql += "</tr>";
            }

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            SrNo = 0;
            var matid = dc.Material_View("CEMT", "");
            foreach (var m in matid)
            {
                var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, strGrade, 0, "CEMT");
                foreach (var cd in iscd)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + " )" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }


            SrNo = 0;
            var re = dc.AllRemark_View("", ReferenceNo, 0, "CEMT");
            foreach (var r in re)
            {
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CEMTDetail_RemarkId_int), "CEMT");
                foreach (var remk in remark)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.CEMT_Remark_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            //if (lblEntry.Text == "Check")
            if (chkStatus == "Check")
            {
                var RecNo = dc.ReportStatus_View("Chemical Testing", null, null, 1, 0, 0, ReferenceNo, 0, 0, 0);
                foreach (var r in RecNo)
                {
                    if (r.CEMTINWD_ApprovedBy_tint != null && r.CEMTINWD_CheckedBy_tint != null)
                    {
                        var U = dc.User_View(r.CEMTINWD_ApprovedBy_tint, -1, "", "", "");
                        foreach (var r1 in U)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
                            mySql += "</tr>";
                        }
                        var lgin = dc.User_View(r.CEMTINWD_CheckedBy_tint, -1, "", "", "");
                        foreach (var loginusr in lgin)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                            mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                        }
                    }
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page of 1 of 1 " + "</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PNI999PTC014212" + "</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add - 1160/5,Gharpure Colony Shivaji Nagar, Pune 411005,Maharastra India." + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "</table>";

            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("Cement_Report", mySql);
        }
        public void FlyAshReport_Html(string ReferenceNo, string chkStatus)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Fly Ash Testing</b></font></td></tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var FlyashInwd = dc.ReportStatus_View("Fly Ash Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);
            string CementCubeStrength = string.Empty;
            var cemtavg = dc.OtherCubeTestView(ReferenceNo, "FLYASH", 28, 0, "CEMT", false, true);
            foreach (var cmavg in cemtavg)
            {
                CementCubeStrength = Convert.ToString(cmavg.Avg_var);
                break;
            }
            foreach (var flyash in FlyashInwd)
            {
                mySql += "<tr>" +
                      "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td width='40%' height=19><font size=2>" + flyash.CL_Name_var + "</font></td>" +
                      "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + "-" + "</font></td>" +
                      "<td height=19><font size=2>&nbsp;</font></td>" +
                      "</tr>";

                mySql += "<tr>" +

                     "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
                     "<td height=19><font size=2></font>:</td>" +
                     "<td width='40%' height=19><font size=2>" + flyash.CL_OfficeAddress_var + "</font></td>" +
                     "<td height=19><font size=2><b>Record No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "FLYASH" + "-" + flyash.FLYASHINWD_SetOfRecord_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +
                    "<td width='20%' height=19><font size=2><b></b></font></td>" +
                    "<td width='2%' height=19><font size=2></font></td>" +
                    "<td width='10%' height=19><font size=2></font></td>" +
                     "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "FLYASH" + "-" + flyash.FLYASHINWD_ReferenceNo_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + flyash.SITE_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "FLYASH" + "-" + "" + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Cement Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + flyash.FLYASHINWD_CementName_var + "</font></td>" +
                    "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + Convert.ToDateTime(flyash.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                      "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
                      "<td height=19><font size=2></font></td>" +
                      "<td height=19><font size=2>" + flyash.FLYASHINWD_Description_var.ToString() + "</font></td>" +
                      "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(flyash.FLYASHINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";


                mySql += "<tr>" +
                            "<td align=left valign=top height=19><font size=2><b>Supplier Name</b></font></td>" +
                            "<td width='2%' height=19><font size=2>:</font></td>" +
                            "<td height=19><font size=2>" + flyash.FLYASHINWD_SupplierName_var.ToString() + "</font></td>" +
                             "<td align=left valign=top height=19><font size=2><b>Cement Cube Strength</b></font></td>" +
                             "<td height=19><font size=2>:</font></td>" +
                             "<td height=19><font size=2>" + CementCubeStrength + "</font></td>" +
                    "</tr>";


                mySql += "<tr>" +
                  "<td align=left valign=top height=19><font size=2><b>Flyash Name</b></font></td>" +
                  "<td width='2%' height=19><font size=2>:</font></td>" +
                  "<td height=19><font size=2>" + flyash.FLYASHINWD_FlyAshName_var + "</font></td>" +
                  "<td align=left valign=top height=19></td>" +
                  "<td height=19></td>" +
                  "<td height=19></td>" +
                  "</tr>";

            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";


            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "RESULTS : " + "</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";


            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Name Of Test</b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Unit </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Result </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Specified Limits </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Compliance </b></font></td>";
            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Method Of Testing </b></font></td>";
            mySql += "</tr>";


            int i = 0;
            int SrNo = 0;
            var details = dc.AllInwdDetails_View(ReferenceNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "FLYASH");
            foreach (var FLYASH in details)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                bool valid = false;
                string TEST_Name_var = "";

                if (FLYASH.TEST_Name_var.ToString() == "Compressive Strength")
                {
                    if (FLYASH.FLYASHTEST_Days_tint.ToString() != "" && FLYASH.FLYASHTEST_Days_tint.ToString() != null && FLYASH.FLYASHTEST_Days_tint.ToString() != "0")
                    {
                        TEST_Name_var = " " + "(" + "" + FLYASH.FLYASHTEST_Days_tint.ToString() + " " + "Days" + " " + ")" + " " + FLYASH.TEST_Name_var.ToString();
                        mySql += "<td width=10% align=center valign=top height=19 ><font size=2>&nbsp;" + TEST_Name_var + "</font></td>";
                    }
                }
                else
                {
                    mySql += "<td width=20% align=center valign=top height=19 ><font size=2>&nbsp;" + FLYASH.TEST_Name_var.ToString() + "</font></td>";
                }
                var Id = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, true, FLYASH.TEST_Id, "", 0, 0, 0, 0, 0, "", 0, "FLYASH");
                foreach (var testid in Id)
                {
                    valid = true;
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(testid.splmt_Unit_var) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(FLYASH.FLYASHTEST_Result_var) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(testid.splmt_SpecifiedLimit_var) + "</font></td>";
                    int SpecifiedLmtRes = 0;
                    bool validmax = false;
                    string res = "";

                    string[] SpceifiedLmt = Convert.ToString(testid.splmt_SpecifiedLimit_var).Split(' ', ',');
                    foreach (var Comp in SpceifiedLmt)
                    {
                        if (Comp != "")
                        {
                            if (Comp.Trim() == "Maximum")
                            {
                                validmax = true;
                            }
                            if (Comp.Trim() == "PCC" || Comp.Trim() == "RCC")
                            {
                                res = res + " " + "-" + " " + Comp + "</br>";
                            }
                            if (int.TryParse(Comp, out SpecifiedLmtRes))
                            {
                                SpecifiedLmtRes = Convert.ToInt32(Comp.ToString());
                                if (validmax == true)
                                {
                                    if (Convert.ToString(FLYASH.FLYASHTEST_Result_var).Trim() != "Awaited" && Convert.ToString(FLYASH.FLYASHTEST_Result_var).Trim() != "*" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "---" && Convert.ToString(testid.splmt_SpecifiedLimit_var).Trim() != "Not Specified")
                                    {
                                        if ((Convert.ToDouble(FLYASH.FLYASHTEST_Result_var.ToString())) < Convert.ToInt32(SpecifiedLmtRes))
                                        {
                                            res = res + "Pass ";
                                            // mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "Pass" + "</font></td>";
                                        }
                                        else
                                        {
                                            res = res + "Fail ";
                                            // mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "Fail" + "</font></td>";
                                        }
                                    }
                                    else
                                    {
                                        res = "---";
                                        // mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "---" + "</font></td>";
                                    }
                                }
                                else
                                {
                                    if (Convert.ToString(FLYASH.FLYASHTEST_Result_var).Trim() != "Awaited" && Convert.ToString(FLYASH.FLYASHTEST_Result_var).Trim() != "*" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "---" && Convert.ToString(testid.splmt_SpecifiedLimit_var).Trim() != "Not Specified")
                                    {
                                        if ((Convert.ToDouble(FLYASH.FLYASHTEST_Result_var.ToString())) > Convert.ToInt32(SpecifiedLmtRes))
                                        {
                                            res = res + "Pass ";
                                            //  mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "Pass" + "</font></td>";
                                        }
                                        else
                                        {
                                            res = res + "Fail ";
                                            //  mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "Fail" + "</font></td>";
                                        }
                                    }
                                    else
                                    {
                                        res = "---";
                                        //  mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + "---" + "</font></td>";
                                    }
                                }
                            }
                        }
                    }
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + res + "</font></td>";
                    mySql += "<td width= 20% align=center valign=top height=19 ><font size=2>" + Convert.ToString(testid.splmt_testingMethod_var) + "</font></td>";
                    break;
                }
                if (valid == false)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(FLYASH.FLYASHTEST_Result_var) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
                }
                mySql += "</tr>";
            }
            i++;

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";


            SrNo = 0;
            var matid = dc.Material_View("FLYASH", "");
            foreach (var m in matid)
            {
                var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "FLYASH");
                foreach (var cd in iscd)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + " )" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }

            SrNo = 0;
            var re = dc.AllRemark_View("", ReferenceNo, 0, "FLYASH");
            foreach (var r in re)
            {
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.FLYASHDetail_RemarkId_int), "FLYASH");
                foreach (var remk in remark)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.FLYASH_Remark_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            //if (lblEntry.Text == "Check")
            if (chkStatus == "Check")
            {
                var RecNo = dc.ReportStatus_View("Fly Ash Testing", null, null, 1, 0, 0, ReferenceNo, 0, 0, 0);
                foreach (var r in RecNo)
                {
                    if (r.FLYASHINWD_ApprovedBy_tint != null && r.FLYASHINWD_CheckedBy_tint != null)
                    {
                        var U = dc.User_View(r.FLYASHINWD_ApprovedBy_tint, -1, "", "", "");
                        foreach (var r1 in U)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
                            mySql += "</tr>";
                        }
                        var lgin = dc.User_View(r.FLYASHINWD_CheckedBy_tint, -1, "", "", "");
                        foreach (var loginusr in lgin)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                            mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                        }
                    }
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page of 1 of 1 " + "</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PNI999PTC014212" + "</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add - 1160/5,Gharpure Colony Shivaji Nagar, Pune 411005,Maharastra India." + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "</table>";


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("FlyAsh_Report", mySql);
        }
        public void CoreReport_Html(string ReferenceNo, string chkStatus)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Concrete Core Compressive Strength </b></font></td></tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            int PrintPulse = 0;
            var Core = dc.ReportStatus_View("Core Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);
            foreach (var CoreTest in Core)
            {
                mySql += "<tr>" +
                "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td width='40%' height=19><font size=2>" + CoreTest.CL_Name_var + "</font></td>" +
                "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td height=19><font size=2>" + "-" + "</font></td>" +
                "<td height=19><font size=2>&nbsp;</font></td>" +
                "</tr>";


                mySql += "<tr>" +
                     "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
                     "<td height=19><font size=2></font></td>" +
                    "<td width='40%' height=19><font size=2>" + CoreTest.CL_OfficeAddress_var + "</font></td>" +
                     "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "CR" + "-" + CoreTest.CRINWD_SetOfRecord_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +

                 "<td width='20%' height=19><font size=2><b></b></font></td>" +
                 "<td width='2%' height=19><font size=2></font></td>" +
                 "<td width='10%' height=19><font size=2></font></td>" +
                 "<td height=19><font size=2><b>Sample Ref No.</b></font></td>" +
                 "<td height=19><font size=2>:</font></td>" +
                 "<td height=19><font size=2>" + "CR" + "-" + CoreTest.CRINWD_ReferenceNo_var + "</font></td>" +
                 "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + CoreTest.SITE_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Sample Ref No. </b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "CR" + "-" + "" + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +

                   "<td align=left valign=top height=19><font size=2><b> Concrete Member </b></font></td>" +
                   "<td  width='2%' height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + CoreTest.CRINWD_ConcreteMember_var + "</font></td>" +
                    "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "CR" + "-" + " " + "</font></td>" +
                   "</tr>";

                mySql += "<tr>" +
                    "<td align=left valign=top height=19><font size=2><b> Grade Of Concrete</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + CoreTest.CRINWD_Grade_var + "</font></td>" +
                    "<td align=left valign=top height=19><font size=2><b>Specimen extraction Date </b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + Convert.ToDateTime(CoreTest.CRINWD_SpecimenExtDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                      "<td align=left valign=top height=19><font size=2><b>Curring Conditions</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + CoreTest.CRINWD_CurrCondition_var + "</font></td>" +
                      "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(CoreTest.CRINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
                      "</tr>";
                if (Convert.ToString(CoreTest.CRINWD_PulseVelocity_bit) != null)
                {
                    PrintPulse = Convert.ToInt32(CoreTest.CRINWD_PulseVelocity_bit);
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 2% rowspan=1 align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Diameter </br> </br> </br> (mm) </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Date of Casting </b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Age of Concrete </br> </br> (Days) </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Area of Cross Section  (mm <sup>2</sup>)</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Weight before capping (kg)</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Density of concrete </br> </br> (kg/m <sup>3</sup>)</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Load at failure </br> </br> (kN)</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Comp. Strength </br> </br> (N/mm<sup>2</sup>)</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Corrected Comp. Strength (N/mm<sup>2</sup>)</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Equivalent cube Comp. Strength (N/mm<sup>2</sup>)</b></font></td>";
            mySql += "</tr>";

            int SrNo = 0;
            var CoreTesting = dc.TestDetail_Title_View(ReferenceNo, 0, "CR", false);
            foreach (var core in CoreTesting)
            {
                SrNo++;
                if (Convert.ToString(core.Description_var) != "")
                {
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Description_var) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Dia_int) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Castingdate_var) + "</font></td>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Age_var) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.CsArea_dec) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Weight_dec) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Density_dec) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Reading_dec) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.CompStr_dec) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.CorrCompStr_dec) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.EquCubeStr_dec) + "</font></td>";
                    mySql += "</tr>";
                }
                else
                {
                    if (Convert.ToString(core.TitleId_int) != "")
                    {
                        if (Convert.ToInt32(core.TitleId_int) > 0)
                        {
                            var crr = dc.TestDetail_Title_View(ReferenceNo, Convert.ToInt32(core.TitleId_int), "CR", false);
                            foreach (var title in crr)
                            {
                                mySql += "<tr>";
                                mySql += "<td width= 10% colspan=12 align=center valign=top height=19 ><font size=2> <b>&nbsp;" + Convert.ToString(title.TitleDesc_var) + "</b></font></td>";
                                mySql += "</tr>";
                                SrNo--;
                                break;
                            }
                        }
                    }
                }
            }
            mySql += "</table>";
            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;</b></font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "GENERAL INFORMATION & MODE OF FAILURE: " + "</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=80% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 2%  rowspan=2 align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10%  rowspan=2 align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";



            mySql += "<td width= 5% align=center colspan=2 valign=top height=19 ><font size=2><b>Correction Factor </b></font></td>";
            mySql += "<td width= 5% align=center colspan=2 valign=top height=19 ><font size=2><b>Core Length (mm) </b></font></td>";

            if (PrintPulse == 1)
            {
                mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Pulse  Velcocity</b></font></td>";
            }
            mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Mode of Failure</b></font></td>";
            mySql += "</tr>";
            mySql += "<td width= 5%  align=center valign=top height=19 ><font size=2><b>L/D </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Diameter </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Original </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>with cap</b></font></td>";

            SrNo = 0;
            decimal Diameter = 0;
            decimal Lenforcore = 0;
            decimal Multifactor = 0;

            var CrTesting = dc.TestDetail_Title_View(ReferenceNo, 0, "CR", false);
            foreach (var core in CrTesting)
            {
                SrNo++;
                if (Convert.ToString(core.Description_var) != "")
                {
                    if (core.Dia_int < 100)
                    {
                        Diameter = Convert.ToDecimal(1.08);
                    }
                    else if (core.Dia_int >= 100)
                    {
                        Diameter = Convert.ToDecimal(1);
                    }
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";

                    if (Convert.ToString(core.Dia_int) != "" && Convert.ToString(core.LengthCaping_num) != "")
                    {
                        Lenforcore = (Convert.ToDecimal(core.Dia_int) / Convert.ToDecimal(core.LengthCaping_num));
                        if (Convert.ToDecimal(core.Dia_int) > 0 && Convert.ToDecimal(core.LengthCaping_num) > 0)
                        {
                            Multifactor = (Convert.ToDecimal(0.106) * Lenforcore) + Convert.ToDecimal(0.786);
                        }
                    }
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Description_var) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(Multifactor).ToString("0.000") + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Diameter) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Length_num) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.LengthCaping_num) + "</font></td>";
                    if (PrintPulse == 1)
                    {
                        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.PulseVelocity_dec) + "</font></td>";
                    }
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.ModeOfFailure_var) + "</font></td>";
                    mySql += "</tr>";
                }
                else
                {
                    if (Convert.ToString(core.TitleId_int) != "")
                    {
                        if (Convert.ToInt32(core.TitleId_int) > 0)
                        {
                            var crr = dc.TestDetail_Title_View(ReferenceNo, Convert.ToInt32(core.TitleId_int), "CR", false);
                            foreach (var title in crr)
                            {
                                mySql += "<tr>";
                                mySql += "<td width= 5% colspan=12 align=center valign=top height=19 ><font size=2><b>&nbsp;" + Convert.ToString(title.TitleDesc_var) + "</b></font></td>";
                                mySql += "</tr>";
                                SrNo--;
                                break;
                            }
                        }
                    }
                }
            }
            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";


            SrNo = 0;
            var matid = dc.Material_View("CR", "");
            foreach (var m in matid)
            {
                var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
                foreach (var cd in iscd)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }

            }
            SrNo = 0;
            var re = dc.AllRemark_View("", ReferenceNo, 0, "CR");
            foreach (var r in re)
            {
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CRDetail_RemarkId_int), "CR");
                foreach (var remk in remark)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.CR_Remark_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            //if (lblEntry.Text == "Check")
            if (chkStatus == "Check")
            {
                var RecNo = dc.ReportStatus_View("Core Testing", null, null, 1, 0, 0, ReferenceNo, 0, 0, 0);
                foreach (var r in RecNo)
                {
                    if (Convert.ToString(r.CRINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.CRINWD_CheckedBy_tint) != string.Empty)
                    {
                        var U = dc.User_View(r.CRINWD_ApprovedBy_tint, -1, "", "", "");
                        foreach (var r1 in U)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
                            mySql += "</tr>";
                        }
                        var lgin = dc.User_View(r.CRINWD_CheckedBy_tint, -1, "", "", "");
                        foreach (var loginusr in lgin)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                            mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                        }
                    }
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999CRC014212" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
            mySql += "</tr>";

            mySql += "</table>";


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("Core_Report", mySql);
        }
        public void Pavement_TS_Report_Html(string ReferenceNo, int TestId, string chkStatus)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Precast Concrete Blocks for Paving - Splitting Tensile Strength </b></font></td></tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


            var pt = dc.ReportStatus_View("Pavement Block Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);

            foreach (var pavmt in pt)
            {
                mySql += "<tr>" +
                "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td width='40%' height=19><font size=2>" + pavmt.CL_Name_var + "</font></td>" +
                "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td height=19><font size=2>" + "-" + "</font></td>" +
                "<td height=19><font size=2>&nbsp;</font></td>" +
                "</tr>";


                mySql += "<tr>" +
                     "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
                     "<td height=19><font size=2></font></td>" +
                    "<td width='40%' height=19><font size=2>" + pavmt.CL_OfficeAddress_var + "</font></td>" +
                     "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_SetOfRecord_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +

                 "<td width='20%' height=19><font size=2><b></b></font></td>" +
                 "<td width='2%' height=19><font size=2></font></td>" +
                 "<td width='10%' height=19><font size=2></font></td>" +
                 "<td height=19><font size=2><b>Sample Ref No.</b></font></td>" +
                 "<td height=19><font size=2>:</font></td>" +
                 "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_ReferenceNo_var + "</font></td>" +
                 "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + pavmt.SITE_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "PT" + "-" + " " + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +

                   "<td align=left valign=top height=19><font size=2><b>Nature Of Work</b></font></td>" +
                   "<td  width='2%' height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + pavmt.PTINWD_WorkingNature_var + "</font></td>" +
                   "<td align=left valign=top height=19><font size=2><b> Date of Casting</b></font></td>" +
                   "<td height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + pavmt.PTINWD_CastingDate_dt + "</font></td>" +
                   "</tr>";

                mySql += "<tr>" +
                    "<td align=left valign=top height=19><font size=2><b> Grade Of Concrete</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + pavmt.PTINWD_Grade_var + "</font></td>" +
                    "<td align=left valign=top height=19><font size=2><b>Date Of Receipt </b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.INWD_ReceivedDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                      "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + pavmt.PTINWD_Description_var.ToString() + "</font></td>" +
                      "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.PTINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
                      "</tr>";

            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";


            mySql += "<table cell=9>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<th width= 2%  rowspan=2 align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></th>";
            mySql += "<th width= 10% rowspan=2  align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></th>";
            mySql += "<th width= 5%  rowspan=2   align=center valign=top height=19 ><font size=2><b>Age </br></br> </br> (Days)</b></font></th>";
            mySql += "<th width= 10% rowspan=2  align=center valign=top height=19 ><font size=2><b>Thickness </br></br></br> (mm) </b></font></th>";
            mySql += "<th width= 10% rowspan=2  align=center valign=top height=19 ><font size=2><b>Failure Load </br></br></br> (N) </b></font></th>";
            mySql += "<th width= 10% align=center  colspan=2  valign=top height=19 ><font size=2><b>Mean Failure </br></br> </b></font></th>";
            mySql += "<th width= 10% align=center valign=top  rowspan=2  height=19 ><font size=2><b>Failure Load per Unit Length </br> (N/mm)</b></font></th>";
            mySql += "<th width= 10% align=center valign=top rowspan=2  height=19 ><font size=2><b>Splitting Tensile Strength (N/mm <sup>2</sup>)</b></font></th>";
            mySql += "<th width= 10% align=center valign=top rowspan=2  height=19 ><font size=2><b>Average </br></br> </br>(N/mm <sup>2</sup>)</b></font></t>";
            mySql += "</tr>";
            mySql += "<th width= 10%   align=center valign=top height=19 ><font size=2><b>Length </br> (mm)</b></font></th>";
            mySql += "<th width= 10%   align=center valign=top height=19 ><font size=2><b>Thickness </br> (mm)</b></font></th>";

            int SrNo = 0;
            int i = 0;
            var PT_TS = dc.Pavement_Test_View(ReferenceNo, TestId, "TS");
            var count = PT_TS.Count();
            var PT_TensileStr = dc.Pavement_Test_View(ReferenceNo, TestId, "TS");
            foreach (var ptts in PT_TensileStr)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.IdMark_var) + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.Age_var) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.Thickness_var).Replace("mm", "") + "</font></td>";
                mySql += "<td width= 10%  align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.FailureLoad_num) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.FailureLength_num) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.FailureThickness_int) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.FailureLoadPerUnitLen_num) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.TensileStrength_dec) + "</font></td>";

                if (i == 0)
                {
                    mySql += "<td  BorderStyle=none width= 10%  align=center rowspan=" + count + " valign=middle height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.PTINWD_AvgStr_var) + "</font></td>";
                }
                i++;
                mySql += "</tr>";
            }

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";


            SrNo = 0;
            var matid = dc.Material_View("PT", "");
            foreach (var m in matid)
            {
                var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
                foreach (var cd in iscd)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }

            }

            SrNo = 0;
            var re = dc.Pavement_Test_Remark_View("", ReferenceNo, 0, TestId);
            foreach (var r in re)
            {
                var remark = dc.Pavement_Test_Remark_View("", "", Convert.ToInt32(r.RemarkId_int), TestId);
                foreach (var remk in remark)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.Remark_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            //if (lblEntry.Text == "Check")
            if (chkStatus == "Check")
            {
                var RecNo = dc.ReportStatus_View("Pavement Block Testing", null, null, 1, 0, 0, ReferenceNo, 0, 0, 0);
                foreach (var r in RecNo)
                {
                    if (Convert.ToString(r.PTINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.PTINWD_CheckedBy_tint) != string.Empty)
                    {
                        var U = dc.User_View(r.PTINWD_ApprovedBy_tint, -1, "", "", "");
                        foreach (var r1 in U)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
                            mySql += "</tr>";
                        }
                        var lgin = dc.User_View(r.PTINWD_CheckedBy_tint, -1, "", "", "");
                        foreach (var loginusr in lgin)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                            mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                        }
                    }
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999PTC014212" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
            mySql += "</tr>";

            mySql += "</table>";


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("Pavement_TS_Report", mySql);
        }
        public void Pavement_CS_Report_Html(string ReferenceNo, int TestId, string chkStatus)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Precast Concrete Blocks for Paving - Compressive Strength </b></font></td></tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


            var pt = dc.ReportStatus_View("Pavement Block Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);

            foreach (var pavmt in pt)
            {
                mySql += "<tr>" +
                "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td width='40%' height=19><font size=2>" + pavmt.CL_Name_var + "</font></td>" +
                "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td height=19><font size=2>" + "-" + "</font></td>" +
                "<td height=19><font size=2>&nbsp;</font></td>" +
                "</tr>";


                mySql += "<tr>" +
                     "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
                     "<td height=19><font size=2></font></td>" +
                    "<td width='40%' height=19><font size=2>" + pavmt.CL_OfficeAddress_var + "</font></td>" +
                     "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_SetOfRecord_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +

                 "<td width='20%' height=19><font size=2><b></b></font></td>" +
                 "<td width='2%' height=19><font size=2></font></td>" +
                 "<td width='10%' height=19><font size=2></font></td>" +
                 "<td height=19><font size=2><b>Sample Ref No.</b></font></td>" +
                 "<td height=19><font size=2>:</font></td>" +
                 "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_ReferenceNo_var + "</font></td>" +
                 "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + pavmt.SITE_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "PT" + "-" + " " + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +

                   "<td align=left valign=top height=19><font size=2><b>Nature Of Work</b></font></td>" +
                   "<td  width='2%' height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + pavmt.PTINWD_WorkingNature_var + "</font></td>" +
                   "<td align=left valign=top height=19><font size=2><b> Date of Casting</b></font></td>" +
                   "<td height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + pavmt.PTINWD_CastingDate_dt + "</font></td>" +
                   "</tr>";

                mySql += "<tr>" +
                    "<td align=left valign=top height=19><font size=2><b> Grade Of Concrete</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + pavmt.PTINWD_Grade_var + "</font></td>" +
                    "<td align=left valign=top height=19><font size=2><b>Date Of Receipt </b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.INWD_ReceivedDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                      "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + pavmt.PTINWD_Description_var.ToString() + "</font></td>" +
                      "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.PTINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
                      "</tr>";

            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";


            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Age </br></br> (Days)</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Plan Area </br></br> (mm<sup>2</sup>) </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Actual Thickness </br> (mm) </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Weight </br> </br>(kg)</b></font></td>";
            mySql += "<td width= 15% align=center valign=top height=19 ><font size=2><b>Density </br></br> (kg/m <sup>3</sup>)</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Load </br></br>  (kN)</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Comp. Strength </br> (N/mm <sup>2</sup> )</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Average </br></br> (N/mm <sup>2</sup>)</b></font></td>";
            mySql += "</tr>";

            int SrNo = 0;
            int i = 0;
            var PT_WA = dc.Pavement_Test_View(ReferenceNo, TestId, "CS");
            var count = PT_WA.Count();
            var PT_WaterAbsorp = dc.Pavement_Test_View(ReferenceNo, TestId, "CS");
            foreach (var ptcs in PT_WaterAbsorp)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.IdMark_var) + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.Age_var) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.PlanArea_num) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.ActualThickness_int) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.Weight_dec) + "</font></td>";
                mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.Density_dec) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.Reading_var) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.CompStr_var) + "</font></td>";
                if (i == 0)
                {
                    mySql += "<td  BorderStyle=none width= 10%  align=center rowspan=" + count + " valign=middle height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.PTINWD_AvgStr_var) + "</font></td>";
                }
                i++;
                mySql += "</tr>";
            }

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";


            SrNo = 0;
            var matid = dc.Material_View("PT", "");
            foreach (var m in matid)
            {
                var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
                foreach (var cd in iscd)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }

            }
            SrNo = 0;
            var re = dc.Pavement_Test_Remark_View("", ReferenceNo, 0, TestId);
            foreach (var r in re)
            {
                var remark = dc.Pavement_Test_Remark_View("", "", Convert.ToInt32(r.RemarkId_int), TestId);
                foreach (var remk in remark)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.Remark_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            //if (lblEntry.Text == "Check")
            if (chkStatus == "Check")
            {
                var RecNo = dc.ReportStatus_View("Pavement Block Testing", null, null, 1, 0, 0, ReferenceNo, 0, 0, 0);
                foreach (var r in RecNo)
                {
                    if (Convert.ToString(r.PTINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.PTINWD_CheckedBy_tint) != string.Empty)
                    {
                        var U = dc.User_View(r.PTINWD_ApprovedBy_tint, -1, "", "", "");
                        foreach (var r1 in U)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
                            mySql += "</tr>";
                        }
                        var lgin = dc.User_View(r.PTINWD_CheckedBy_tint, -1, "", "", "");
                        foreach (var loginusr in lgin)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                            mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                        }
                    }
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999PTC014212" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
            mySql += "</tr>";

            mySql += "</table>";


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("Pavement_CS_Report", mySql);
        }
        public void Pavement_FS_Report_Html(string ReferenceNo, int TestId, string chkStatus)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Precast Concrete Blocks for Paving - Flexural Strength </b></font></td></tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


            var pt = dc.ReportStatus_View("Pavement Block Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);

            foreach (var pavmt in pt)
            {
                mySql += "<tr>" +
                "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td width='40%' height=19><font size=2>" + pavmt.CL_Name_var + "</font></td>" +
                "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td height=19><font size=2>" + "-" + "</font></td>" +
                "<td height=19><font size=2>&nbsp;</font></td>" +
                "</tr>";


                mySql += "<tr>" +
                     "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
                     "<td height=19><font size=2></font></td>" +
                    "<td width='40%' height=19><font size=2>" + pavmt.CL_OfficeAddress_var + "</font></td>" +
                     "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_SetOfRecord_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +

                 "<td width='20%' height=19><font size=2><b></b></font></td>" +
                 "<td width='2%' height=19><font size=2></font></td>" +
                 "<td width='10%' height=19><font size=2></font></td>" +
                 "<td height=19><font size=2><b>Sample Ref No.</b></font></td>" +
                 "<td height=19><font size=2>:</font></td>" +
                 "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_ReferenceNo_var + "</font></td>" +
                 "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + pavmt.SITE_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "PT" + "-" + " " + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +

                   "<td align=left valign=top height=19><font size=2><b>Nature Of Work</b></font></td>" +
                   "<td  width='2%' height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + pavmt.PTINWD_WorkingNature_var + "</font></td>" +
                   "<td align=left valign=top height=19><font size=2><b> Date of Casting</b></font></td>" +
                   "<td height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + pavmt.PTINWD_CastingDate_dt + "</font></td>" +
                   "</tr>";

                mySql += "<tr>" +
                    "<td align=left valign=top height=19><font size=2><b> Grade Of Concrete</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + pavmt.PTINWD_Grade_var + "</font></td>" +
                    "<td align=left valign=top height=19><font size=2><b>Date Of Receipt </b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.INWD_ReceivedDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                      "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + pavmt.PTINWD_Description_var.ToString() + "</font></td>" +
                      "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.PTINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
                      "</tr>";

            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";


            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Age </br> (Days)</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Thickness </br> (mm) </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Breaking Load </br> (kN) </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Flexural Strength </br> (N/mm <sup>2</sup>)</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Average </br> (N/mm <sup>2</sup>)</b></font></td>";
            mySql += "</tr>";

            int SrNo = 0;
            int i = 0;
            var PT_CS = dc.Pavement_Test_View(ReferenceNo, TestId, "FS");
            var count = PT_CS.Count();
            var PT_FlexualStr = dc.Pavement_Test_View(ReferenceNo, TestId, "FS");
            foreach (var ptcs in PT_FlexualStr)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.IdMark_var) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.Age_var) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.Thickness_var).Replace("mm", "") + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.BreakingLoad_dec) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.FlexuralStrength_dec) + "</font></td>";
                if (i == 0)
                {
                    mySql += "<td  BorderStyle=none width= 10%  align=center rowspan=" + count + " valign=middle height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.PTINWD_AvgStr_var) + "</font></td>";
                }
                i++;
                mySql += "</tr>";
            }

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";


            SrNo = 0;
            var matid = dc.Material_View("PT", "");
            foreach (var m in matid)
            {
                var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
                foreach (var cd in iscd)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }

            }

            SrNo = 0;
            var re = dc.Pavement_Test_Remark_View("", ReferenceNo, 0, TestId);
            foreach (var r in re)
            {
                var remark = dc.Pavement_Test_Remark_View("", "", Convert.ToInt32(r.RemarkId_int), TestId);
                foreach (var remk in remark)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.Remark_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            //if (lblEntry.Text == "Check")
            if (chkStatus == "Check")
            {
                var RecNo = dc.ReportStatus_View("Pavement Block Testing", null, null, 1, 0, 0, ReferenceNo, 0, 0, 0);
                foreach (var r in RecNo)
                {
                    if (Convert.ToString(r.PTINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.PTINWD_CheckedBy_tint) != string.Empty)
                    {
                        var U = dc.User_View(r.PTINWD_ApprovedBy_tint, -1, "", "", "");
                        foreach (var r1 in U)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
                            mySql += "</tr>";
                        }
                        var lgin = dc.User_View(r.PTINWD_CheckedBy_tint, -1, "", "", "");
                        foreach (var loginusr in lgin)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                            mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                        }
                    }
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999PTC014212" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
            mySql += "</tr>";

            mySql += "</table>";


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("Pavement_FS_Report", mySql);
        }
        public void PileReport_Html(string ReferenceNo, int RecordNo, string chkStatus)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Pile Integrity</b></font></td></tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var ct = dc.ReportStatus_View("Pile Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);

            foreach (var pile in ct)
            {
                mySql += "<tr>" +
                      "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td width='40%' height=19><font size=2>" + pile.CL_Name_var + "</font></td>" +
                      "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + "-" + "</font></td>" +
                      "<td height=19><font size=2>&nbsp;</font></td>" +
                      "</tr>";

                mySql += "<tr>" +

                     "<td width='20%' height=19><font size=2><b></b></font></td>" +
                     "<td width='2%' height=19><font size=2></font></td>" +
                     "<td width='10%' height=19><font size=2></font></td>" +
                     "<td height=19><font size=2><b>Record No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "PILE" + "-" + pile.PILEINWD_SetOfRecord_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +
                     "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
                     "<td  width='2%' height=19><font size=2></font>:</td>" +
                     "<td width='40%' height=19><font size=2>" + pile.CL_OfficeAddress_var + "</font></td>" +
                     "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "PILE" + "-" + pile.PILEINWD_ReferenceNo_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b></b></font></td>" +
                    "<td width='2%' height=19><font size=2></font></td>" +
                    "<td width='10%' height=19><font size=2></font></td>" +
                    "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "PILE" + "-" + "" + "</font></td>" +
                    "</tr>";


                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + pile.SITE_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + Convert.ToDateTime(pile.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                    "</tr>";



                mySql += "<tr>" +
                      "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + pile.PILEINWD_Description_var.ToString() + "</font></td>" +
                      "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(pile.PILEINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";


            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test Results : " + "</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";


            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";


            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Catagory</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Inference</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Pile Identification </b></font></td>";
            mySql += "</tr>";

            int i = 0;
            int SrNo = 0;
            int CountPiles = 0;
            var details = dc.PileDetailsView("", 0, "");
            foreach (var t in details)
            {

                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + t.PILE_Name_var + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + t.PILE_Description_var + "</font></td>";


                bool valid = true;

                string Identi = "";
                var pl = dc.PileDetailsView(ReferenceNo, 0, "");
                foreach (var p in pl)
                {
                    if (Convert.ToInt32(p.PILEDETAIL_CatagoryId_int) > 0)
                    {
                        var c = dc.PileDetailsView("", Convert.ToInt32(p.PILEDETAIL_CatagoryId_int), "");
                        foreach (var n in c)
                        {
                            if (p.PILEDETAIL_Identification_var != null)
                            {
                                if (n.PILE_Name_var.ToString() == t.PILE_Name_var.ToString())
                                {
                                    CountPiles++;
                                    valid = false;
                                    Identi = Identi + p.PILEDETAIL_Identification_var.ToString() + ",";
                                }
                            }
                        }
                    }
                }
                if (valid == true)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "NA" + "</font></td>";
                }
                else
                {
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + Identi.ToString() + "</font></td>";
                }

                mySql += "</tr>";
            }

            i++;

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "Total No Of Piles Tested :" + "</font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + CountPiles + "</font></td>";
            mySql += "</tr>";

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            SrNo = 0;
            var matid = dc.Material_View("PILE", "");
            foreach (var m in matid)
            {
                var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
                foreach (var cd in iscd)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }

            SrNo = 0;
            var re = dc.AllRemark_View("", ReferenceNo, 0, "PILE");
            foreach (var r in re)
            {
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.PILEDetail_RemarkId_int), "PILE");
                foreach (var remk in remark)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.PILE_Remark_var.ToString() + "</font></td>";
                    mySql += "</tr>";

                }
            }

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " Authorised Signatory" + "</font></td>";
            mySql += "</tr>";
            //if (lblEntry.Text == "Check")
            if (chkStatus == "Check")
            {
                var RecNo = dc.AllInwdDetails_View(ReferenceNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, RecordNo, "", 0, "PILE");
                foreach (var r in RecNo)
                {
                    var Auth = dc.AllInwdDetails_View(ReferenceNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, r.PILEINWD_ApprovedBy_tint, 0, 0, "", 0, "PILE");

                    foreach (var Approve in Auth)
                    {

                        mySql += "<tr>";
                        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Approve.USER_Name_var.ToString() + "</font></td>";
                        mySql += "</tr>";
                        mySql += "<tr>";
                        mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + Approve.USER_Designation_var.ToString() + ")" + "</font></td>";
                        mySql += "</tr>";

                    }
                }
            }


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "</table>";


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("PileReport_Html", mySql);
        }

        public void Pavement_WA_Report_Html(string ReferenceNo, int TestId, string chkStatus)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Precast Concrete Blocks for Paving - Water Absorption </b></font></td></tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


            var pt = dc.ReportStatus_View("Pavement Block Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);

            foreach (var pavmt in pt)
            {
                mySql += "<tr>" +
                "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td width='40%' height=19><font size=2>" + pavmt.CL_Name_var + "</font></td>" +
                "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td height=19><font size=2>" + "-" + "</font></td>" +
                "<td height=19><font size=2>&nbsp;</font></td>" +
                "</tr>";

                mySql += "<tr>" +
                     "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
                     "<td height=19><font size=2></font></td>" +
                    "<td width='40%' height=19><font size=2>" + pavmt.CL_OfficeAddress_var + "</font></td>" +
                     "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_SetOfRecord_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +

                 "<td width='20%' height=19><font size=2><b></b></font></td>" +
                 "<td width='2%' height=19><font size=2></font></td>" +
                 "<td width='10%' height=19><font size=2></font></td>" +
                 "<td height=19><font size=2><b>Sample Ref No.</b></font></td>" +
                 "<td height=19><font size=2>:</font></td>" +
                 "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_ReferenceNo_var + "</font></td>" +
                 "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + pavmt.SITE_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "PT" + "-" + " " + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +

                   "<td align=left valign=top height=19><font size=2><b>Nature Of Work</b></font></td>" +
                   "<td  width='2%' height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + pavmt.PTINWD_WorkingNature_var + "</font></td>" +
                   "<td align=left valign=top height=19><font size=2><b> Date of Casting</b></font></td>" +
                   "<td height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + pavmt.PTINWD_CastingDate_dt + "</font></td>" +
                   "</tr>";

                mySql += "<tr>" +
                    "<td align=left valign=top height=19><font size=2><b> Grade Of Concrete</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + pavmt.PTINWD_Grade_var + "</font></td>" +
                    "<td align=left valign=top height=19><font size=2><b>Date Of Receipt </b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + DateTime.Now.ToString("dd/MMM/yy") + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                      "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + pavmt.PTINWD_Description_var.ToString() + "</font></td>" +
                      "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.PTINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
                      "</tr>";

            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";


            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Dry Weight </br> (g)</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Wet Weight </br>  (g) </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Water Absorption </br>  (%)</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Average </br>  (%)</b></font></td>";
            mySql += "</tr>";

            int SrNo = 0;
            int i = 0;
            var PT_WA = dc.Pavement_Test_View(ReferenceNo, TestId, "WA");
            var count = PT_WA.Count();
            var PT_WaterAbsorp = dc.Pavement_Test_View(ReferenceNo, TestId, "WA");
            foreach (var ptwa in PT_WaterAbsorp)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.IdMark_var) + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.DryWeight_int) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.Wet_Weight_int) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.WaterAbsorption_dec) + "</font></td>";
                if (i == 0)
                {
                    mySql += "<td  BorderStyle=none width= 10%  align=center rowspan=" + count + " valign=middle height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.PTINWD_AvgStr_var) + "</font></td>";
                }
                i++;
                mySql += "</tr>";
            }

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";


            SrNo = 0;
            var matid = dc.Material_View("PT", "");
            foreach (var m in matid)
            {
                var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
                foreach (var cd in iscd)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }

            }
            SrNo = 0;
            var re = dc.Pavement_Test_Remark_View("", ReferenceNo, 0, TestId);
            foreach (var r in re)
            {
                var remark = dc.Pavement_Test_Remark_View("", "", Convert.ToInt32(r.RemarkId_int), TestId);
                foreach (var remk in remark)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.Remark_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            //if (lblEntry.Text == "Check")
            if (chkStatus == "Check")
            {
                var RecNo = dc.ReportStatus_View("Pavement Block Testing", null, null, 1, 0, 0, ReferenceNo, 0, 0, 0);
                foreach (var r in RecNo)
                {
                    if (Convert.ToString(r.PTINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.PTINWD_CheckedBy_tint) != string.Empty)
                    {
                        var U = dc.User_View(r.PTINWD_ApprovedBy_tint, -1, "", "", "");
                        foreach (var r1 in U)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
                            mySql += "</tr>";
                        }
                        var lgin = dc.User_View(r.PTINWD_CheckedBy_tint, -1, "", "", "");
                        foreach (var loginusr in lgin)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                            mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                        }
                    }
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
            mySql += "</tr>";


            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999PTC014212" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
            mySql += "</tr>";

            mySql += "</table>";


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("Pavement_WA_Report", mySql);
        }
        public void CCHReport_Html(string ReferenceNo, string strGrade, string chkStatus)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Hydraulic Cement(Chemical)</b></font></td></tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var water = dc.ReportStatus_View("Cement Chemical Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);

            foreach (var w in water)
            {
                mySql += "<tr>" +
                      "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td width='40%' height=19><font size=2>" + w.CL_Name_var + "</font></td>" +
                      "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + "-" + "</font></td>" +
                      "<td height=19><font size=2>&nbsp;</font></td>" +
                      "</tr>";

                mySql += "<tr>" +

                     "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
                     "<td height=19><font size=2></font>:</td>" +
                     "<td width='40%' height=19><font size=2>" + w.CL_OfficeAddress_var + "</font></td>" +
                     "<td height=19><font size=2><b>Record No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "CCH" + "-" + w.CCHINWD_SetOfRecord_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +
                    "<td width='20%' height=19><font size=2><b></b></font></td>" +
                    "<td width='2%' height=19><font size=2></font></td>" +
                    "<td width='10%' height=19><font size=2></font></td>" +
                     "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + "CCH" + "-" + w.CCHINWD_ReferenceNo_var.ToString() + "</font></td>" +
                     "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + w.SITE_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "CCH" + "-" + "" + "</font></td>" +
                    "</tr>";


                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Cement Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + w.CCHINWD_CementName_var + "</font></td>" +
                    "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + Convert.ToDateTime(w.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                    "</tr>";



                mySql += "<tr>" +
                      "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + w.CCHINWD_Description_var.ToString() + "</font></td>" +
                      "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(w.CCHINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                mySql += "<tr>" +
                    "<td align=left valign=top height=19><font size=2><b>Supplier Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + w.CCHINWD_SupplierName_var.ToString() + "</font></td>" +
                    "<td align=left valign=top height=19></td>" +
                    "<td height=19></td>" +
                    "<td height=19></td>" +
                    "</tr>";

            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";


            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "RESULTS : " + "</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";


            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";


            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Name Of Test</b></font></td>";
            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Result</b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Specified Limits (IS - 12269) </b></font></td>";
            mySql += "</tr>";


            int i = 0;
            int SrNo = 0;

            var details = dc.AllInwdDetails_View(ReferenceNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "CCH");
            foreach (var CCH in details)
            {

                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width=10% align=center valign=top height=19 ><font size=2>&nbsp;" + CCH.TEST_Name_var + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + CCH.CCHTEST_Result_dec + " " + CCH.splmt_Unit_var + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + CCH.splmt_SpecifiedLimit_var.ToString() + "</font></td>";
                mySql += "</tr>";
            }

            i++;

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";


            SrNo = 0;
            var matid = dc.Material_View("CCH", "");
            foreach (var m in matid)
            {
                var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, strGrade, 0, "CCH");
                foreach (var cd in iscd)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + " )" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }
            SrNo = 0;
            var re = dc.AllRemark_View("", ReferenceNo, 0, "CCH");
            foreach (var r in re)
            {
                var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CCHDetail_RemarkId_int), "CCH");
                foreach (var remk in remark)
                {
                    if (SrNo == 0)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
                        mySql += "</tr>";
                    }
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.CCH_Remark_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            //if (lblEntry.Text == "Check")
            if (chkStatus == "Check")
            {
                var RecNo = dc.ReportStatus_View("Cement Chemical Testing", null, null, 1, 0, 0, ReferenceNo, 0, 0, 0);
                foreach (var r in RecNo)
                {
                    if (r.CCHINWD_ApprovedBy_tint != null && r.CCHINWD_CheckedBy_tint != null)
                    {
                        var U = dc.User_View(r.CCHINWD_ApprovedBy_tint, -1, "", "", "");
                        foreach (var r1 in U)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                            mySql += "<tr>";
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
                            mySql += "</tr>";
                        }
                        var lgin = dc.User_View(r.CCHINWD_CheckedBy_tint, -1, "", "", "");
                        foreach (var loginusr in lgin)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                            mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
                            mySql += "</tr>";
                        }
                    }
                }
            }
            mySql += "<tr>";
            mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page of 1 of 1 " + "</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PNI999PTC014212" + "</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add - 1160/5,Gharpure Colony Shivaji Nagar, Pune 411005,Maharastra India." + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "</table>";


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("CCH_Report", mySql);
        }
        public void TrialInformation_Html_Old19072018(string ReferenceNo, int TrialId)
        {
            string mySql = "", tempSql = "";
            string mgrade = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=4><b> Durocrete </b></font> </td></tr>";
            if (cnStr.ToLower().Contains("mumbai") == true)
            {
                mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=3>Engineering Services Pvt. Ltd (PAP-D122/125,TTC Industrial Area,Behind Jai Mata Di Weighbridge,Turbhe,Navi Mumbai. Tel No: +91-9850500013,022-27622353)</font></td></tr>";
            }
            else if (cnStr.ToLower().Contains("nashik") == true)
            {
                mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=3>Engineering Services Pvt. Ltd (Sunil Towers,Behind KK Travels,Dwarka,Nashik. Tel No: +91-9527005478,7720006754)</font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=3>Engineering Services Pvt. Ltd (19/1,Hingane Khurd,Vitthalwadi,Sinhgad Road,Pune. Tel No: +91-9881735302,020-24345170,24348027)</font></td></tr>";
            }
            mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=4><b><u> Trial Information  </u></b></font> </td></tr>";

            var MFInwd = dc.ReportStatus_View("Mix Design", null, null, 0, 0, 0, ReferenceNo, 0, 0, 0);
            foreach (var m in MFInwd)
            {
                mySql += "<tr>" +
                    "<td width='20%' align=left valign=top height=19><font size=2>Customer Name</font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='40%' height=19><font size=2>" + m.CL_Name_var + "</font></td>" +
                    "<td height=19><font size=2>Record No.</font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "MF" + "-" + m.MFINWD_ReferenceNo_var + "</font></td>" +
                    "<td height=19><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                   "<td width='20%' align=left valign=top height=19><font size=2>Site Name</font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='40%' height=19><font size=2>" + m.SITE_Name_var + "</font></td>" +
                     "<td align=left valign=top height=19><font size=2>Grade of Concrete</font></td>" +
                     "<td width='2%' height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + m.MFINWD_Grade_var + "</font></td>" +
                     "</tr>";
                mySql += "<tr>" +
                   "<td width='20%' align=left valign=top height=19><font size=2></font></td>" +
                    "<td width='2%' height=19><font size=2></font></td>" +
                    "<td width='40%' height=19><font size=2></font></td>" +
                     "<td align=left valign=top height=19><font size=2>Slump Requirement</font></td>" +
                     "<td width='2%' height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + m.MFINWD_Slump_var + "</font></td>" +
                     "</tr>";
                mgrade = m.MFINWD_Grade_var.ToString().Replace("M ", "");
                bool flg = false;
                var trial = dc.TrialDetail_View(ReferenceNo, TrialId);
                foreach (var t in trial)
                {

                    if (flg == false)
                    {
                        mySql += "<tr>" +

                            "<td width='20%' height=19><font size=2>Special Requirement</font></td>" +
                            "<td width='2%' height=19><font size=2>:</font></td>" +
                            "<td width='10%' height=19><font size=2>" + m.MFINWD_SpecialRequirement_var + "</font></td>" +
                            "<td height=19><font size=2>Trial Date</font></td>" +
                            "<td width='2%' height=19><font size=2>:</font></td>" +
                            "<td height=19><font size=2>" + Convert.ToDateTime(t.Trial_Date).ToString("dd/MM/yyyy") + "</font></td>" +
                            "</tr>";

                        mySql += "<tr>" +

                            "<td width='20%' height=19><font size=2><b></b></font></td>" +
                            "<td width='2%' height=19><font size=2></font></td>" +
                            "<td width='10%' height=19><font size=2> </font></td>" +
                            "<td height=19><font size=2>Trial Time</font></td>" +
                            "<td width='2%' height=19><font size=2>:</font></td>" +
                            "<td height=19><font size=2>" + Convert.ToString(t.Trial_Time) + "</font></td>" +
                            "</tr>";
                        mySql += "<tr>" +

                           "<td width='20%' height=19><font size=2><b></b></font></td>" +
                           "<td width='2%' height=19><font size=2></font></td>" +
                           "<td width='10%' height=19><font size=2> </font></td>" +
                           "<td height=19><font size=2>Trial Name</font></td>" +
                           "<td width='2%' height=19><font size=2>:</font></td>" +
                           "<td height=19><font size=2><b>" + t.Trial_Name.ToString() + "</b></font></td>" +
                           "</tr>";

                        mySql += "<tr>" +

                           "<td align=left valign=top height=19><font size=2>Admixture Name</font></td>" +
                           "<td height=19><font size=2>:</font></td>" +
                           "<td height=19><font size=2>" + Convert.ToString(t.Trial_Admixture) + "</font></td>" +
                           "<td align=left valign=top height=19><font size=2> Fly Ash Name</font></td>" +
                           "<td width='2%' height=19><font size=2>:</font></td>" +
                           "<td height=19><font size=2>" + Convert.ToString(t.Trial_FlyashUsed) + "</font></td>" +
                           "</tr>";

                        mySql += "<tr>" +
                            "<td align=left valign=top height=19><font size=2> Cement Name </font></td>" +
                            "<td width='2%' height=19><font size=2>:</font></td>" +
                            "<td height=19><font size=2>" + Convert.ToString(t.Trial_CementUsed) + "</font></td>" +
                            "<td align=left valign=top height=19><font size=2><b> </b></font></td>" +
                            "<td height=19><font size=2> </font></td>" +
                            "<td height=19><font size=2></font></td>" +
                            "</tr>";
                        flg = true;
                    }
                }
            }
            string strTargetMeanStr = "---";
            Single stdDev = 0;
            if (Convert.ToDouble(mgrade) <= 15)
                stdDev = Convert.ToSingle("3.5");
            else if (Convert.ToDouble(mgrade) >= 20 && Convert.ToDouble(mgrade) <= 25)
                stdDev = Convert.ToSingle("4");
            else
                stdDev = Convert.ToSingle("5");
            strTargetMeanStr = mgrade + " +(1.65 x " + stdDev + " )= " + Convert.ToString(Convert.ToDouble(mgrade) + (1.65 * stdDev));
            strTargetMeanStr = strTargetMeanStr + " &nbsp;N/mm" + "<sup>2</sup>";
            mySql += "<tr>" +
                 "<td align=left valign=top height=19><font size=2>Target Mean Strength</font></td>" +
                 "<td height=19><font size=2>:</font></td>" +
                 "<td height=19><font size=2>" + strTargetMeanStr + "</font></td>" +
                 "<td align=left valign=top height=19><font size=2> </font></td>" +
                 "<td height=19><font size=2> </font></td>" +
                 "<td height=19><font size=2> </font></td>" +
                 "</tr>";
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 15% align=center valign=top height=19 ><font size=2><b>Material</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Material Proportions</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b> Required Wt.(kg)</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Corrections </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Net Wt. Taken(kg) </b></font></td>";
            mySql += "</tr>";

            var res = dc.TrialDetail_View(ReferenceNo, TrialId);
            foreach (var t in res)
            {
                mySql += "<tr>";
                mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_MaterialName) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_Weight) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(t.TrialDetail_ReqdWt).ToString("0.00") + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_Corrections) + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(t.TrialDetail_NetWeight).ToString("0.00") + "</font></td>";
                mySql += "</tr>";

            }

            var data = dc.TrialDetail_View(ReferenceNo, TrialId);
            foreach (var t in data)
            {

                mySql += "<table>";
                mySql += "<tr>";
                if (t.Trial_MC_NS != "" && t.Trial_MC_NS != null)
                {
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "'Moist'. " + "Moisture Content" + "-" + "Natural Sand " + " " + ":" + " " + Convert.ToString(t.Trial_MC_NS) + " % " + "</font></td>";
                }

                if (t.Trial_MC_CS != "" && t.Trial_MC_CS != null)
                {
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "'Moist'. " + "Moisture Content" + "-" + "Crushed Sand " + " " + ":" + " " + Convert.ToString(t.Trial_MC_CS) + " % " + "</font></td>";

                }
                mySql += "</tr>";
                mySql += "<tr>";
                if (t.Trial_MC_SD != "" && t.Trial_MC_SD != null)
                {
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "'Moist'. " + "Moisture Content" + "-" + "Stone Dust " + " " + ":" + " " + Convert.ToString(t.Trial_MC_SD) + " % " + "</font></td>";
                }
                if (t.Trial_MC_GT != "" && t.Trial_MC_GT != null)
                {
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>" + "'Moist'. " + "Moisture Content" + "-" + "Grit " + " " + ":" + " " + Convert.ToString(t.Trial_MC_SD) + " % " + "</font></td>";
                }
                mySql += "</tr>";
                mySql += "<tr>";
                if (t.Trial_WA_NS != "" && t.Trial_WA_NS != null)
                {
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Water Absorption" + "-" + "Natural Sand " + " " + ":" + " " + Convert.ToString(t.Trial_WA_NS) + " % " + "</font></td>";
                }
                if (t.Trial_WA_CS != "" && t.Trial_WA_CS != null)
                {
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Water Absorption" + "-" + "Crushed Sand " + " " + ":" + " " + Convert.ToString(t.Trial_WA_CS) + " % " + "</font></td>";
                }
                mySql += "</tr>";
                mySql += "<tr>";
                if (t.Trial_WA_SD != "" && t.Trial_WA_SD != null)
                {
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Water Absorption" + "-" + "Stone Dust " + " " + ":" + " " + Convert.ToString(t.Trial_WA_SD) + " % " + "</font></td>";
                }
                if (t.Trial_WA_GT != "" && t.Trial_WA_GT != null)
                {
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>" + "Water Absorption" + "-" + "Grit " + " " + ":" + " " + Convert.ToString(t.Trial_WA_GT) + " % " + "</font></td>";
                }
                mySql += "</tr>";

                mySql += "<tr>";

                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Final Water Cement Ratio" + " " + ":" + " " + "</font></td>";

                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Wt. Of Compacted Concrete" + " " + ":" + " " + "</font></td>";

                mySql += "</tr>";

                mySql += "<tr>";

                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Slump " + " " + ":" + t.Trial_Slump.ToString() + "</font></td>";

                string Slump = "";
                if (t.Trial_RetentionStatus == true)
                {
                    Slump = t.Trial_RetentionSlumpValue.ToString().Replace("|", ",");
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Retention Slump" + " " + ":" + t.Trial_RetentionSlumpValue.ToString() + "(" + " " + Slump + " " + ")" + "</font></td>";
                }
                mySql += "</tr>";

                mySql += "<tr>";

                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Yield " + " " + ":" + "" + "</font></td>";

                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> </font></td>";

                mySql += "</tr>";
                mySql += "<tr>";

                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Compaction Factor " + " " + ":" + " " + "</font></td>";

                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Segregation  " + " " + ":" + " " + "</font></td>";

                mySql += "</tr>";
                mySql += "<tr>";

                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Remark " + " " + ":" + Convert.ToString(t.Trial_Remark) + "</font></td>";

                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> </font></td>";

                mySql += "</tr>";
                break;
            }

            mySql += "<table width=100%>";
            mySql += "<tr>";
            mySql += "<td width=8% align=left valign=top height=20 ><font size=2>" + "Cast By " + "</font></td>";
            mySql += "<td width=25%  align=left valign=top height=20 ><font size=2><p><hr>&nbsp;</p></font></td>";
            mySql += "<td width=8% align=right valign=top height=20 ><font size=2>" + "CE By " + "</font></td>";
            mySql += "<td width=25%  align=left valign=top height=20 ><font size=2><p><hr>&nbsp;</p></font></td>";
            mySql += "<td width=10% align=right valign=top height=20 ><font size=2>" + "Checked By " + "</font></td>";
            mySql += "<td width=20%  align=left valign=top height=20 ><font size=2><p><hr></p></font></td>";
            mySql += "</tr>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Id Mark </b></font></td>";
            mySql += "<td width= 10% align=center colspan=3 valign=top height=19 ><font size=2><b> Dimensions(mm) </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Weight </b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b> Age (Days) </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Load (kN) </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Str(N/mm<sup>2</sup>) </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Avg. Str. </b></font></td>";
            mySql += "</tr>";

            for (int j = 0; j < 10; j++)
            {
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "</tr>";
            }

            mySql += "</table>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("TrialInformation", mySql);
        }

        public void TrialInformation_Html(string ReferenceNo, int TrialId)
        {
            string mySql = "", tempSql = "";
            string mgrade = "", mtest = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            //mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=4><b> Durocrete </b></font> </td></tr>";
            //if (cnStr.ToLower().Contains("mumbai") == true)
            //{
            //    mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=3>Engineering Services Pvt. Ltd (PAP-D122/125,TTC Industrial Area,Behind Jai Mata Di Weighbridge,Turbhe,Navi Mumbai. Tel No: +91-9850500013,022-27622353)</font></td></tr>";
            //}
            //else if (cnStr.ToLower().Contains("nashik") == true)
            //{
            //    mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=3>Engineering Services Pvt. Ltd (Sunil Towers,Behind KK Travels,Dwarka,Nashik. Tel No: +91-9527005478,7720006754)</font></td></tr>";
            //}
            //else
            //{
            //    mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=3>Engineering Services Pvt. Ltd (19/1,Hingane Khurd,Vitthalwadi,Sinhgad Road,Pune. Tel No: +91-9881735302,020-24345170,24348027)</font></td></tr>";
            //}
            mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=4><b><u> Trial Information  </u></b></font> </td></tr>";

            var MFInwd = dc.ReportStatus_View("Mix Design", null, null, 0, 0, 0, ReferenceNo, 0, 0, 0);
            foreach (var m in MFInwd)
            {
                mySql += "<tr>" +
                    "<td width='20%' align=left valign=top height=19><font size=2>Customer Name</font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='40%' height=19><font size=2>" + m.CL_Name_var + "</font></td>" +
                    "<td height=19><font size=2>Record No.</font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + "MF" + "-" + m.MFINWD_ReferenceNo_var + "</font></td>" +
                    "<td height=19><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                   "<td width='20%' align=left valign=top height=19><font size=2>Site Name</font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='40%' height=19><font size=2 >" + m.SITE_Name_var + "</font></td>" +
                     "<td align=left valign=top height=19><font size=2>Grade of Concrete</font></td>" +
                     "<td width='2%' height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + m.MFINWD_Grade_var + "</font></td>" +
                     "</tr>";
                mySql += "<tr>" +
                   "<td width='20%' align=left valign=top height=19><font size=2></font></td>" +
                    "<td width='2%' height=19><font size=2></font></td>" +
                    "<td width='40%' height=19><font size=2></font></td>" +
                     "<td align=left valign=top height=19><font size=2>Slump Requirement</font></td>" +
                     "<td width='2%' height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + m.MFINWD_Slump_var + "</font></td>" +
                     "</tr>";
                mgrade = m.MFINWD_Grade_var.ToString().Replace("M ", "");
                if (m.TEST_Sr_No == 2 || m.TEST_Sr_No == 6) //self compacting
                {
                    mtest = "Flow (mm)";
                }
                else
                {
                    mtest = "Slump (mm)";
                }
                bool flg = false;
                var trial = dc.TrialDetail_View(ReferenceNo, TrialId);
                foreach (var t in trial)
                {

                    if (flg == false)
                    {
                        mySql += "<tr>" +

                            "<td width='20%' height=19><font size=2>Special Requirement</font></td>" +
                            "<td width='2%' height=19><font size=2>:</font></td>" +
                            "<td width='10%' height=19><font size=2>" + m.MFINWD_SpecialRequirement_var + "</font></td>" +
                            "<td height=19><font size=2>Trial Date</font></td>" +
                            "<td width='2%' height=19><font size=2>:</font></td>" +
                            "<td height=19><font size=2>" + Convert.ToDateTime(t.Trial_Date).ToString("dd/MM/yyyy") + "</font></td>" +
                            "</tr>";

                        mySql += "<tr>" +

                            "<td width='20%' height=19><font size=2><b></b></font></td>" +
                            "<td width='2%' height=19><font size=2></font></td>" +
                            "<td width='10%' height=19><font size=2> </font></td>" +
                            "<td height=19><font size=2>Trial Time</font></td>" +
                            "<td width='2%' height=19><font size=2>:</font></td>" +
                            "<td height=19><font size=2>" + Convert.ToString(t.Trial_Time) + "</font></td>" +
                            "</tr>";
                        mySql += "<tr>" +

                           "<td align=left valign=top height=19><font size=2> Cement Name </font></td>" +
                            "<td width='2%' height=19><font size=2>:</font></td>" +
                            "<td height=19><font size=2>" + Convert.ToString(t.Trial_CementUsed) + "</font></td>" +
                           "<td height=19><font size=2>Trial Name</font></td>" +
                           "<td width='2%' height=19><font size=2>:</font></td>" +
                           "<td height=19><font size=2><b>" + t.Trial_Name.ToString() + "</b></font></td>" +
                           "</tr>";

                        mySql += "<tr>" +

                           "<td align=left valign=top height=19><font size=2>Admixture Name</font></td>" +
                           "<td height=19><font size=2>:</font></td>" +
                           "<td height=19><font size=2>" + Convert.ToString(t.Trial_Admixture) + "</font></td>" +
                           "<td align=left valign=top height=19><font size=2> Fly Ash Name</font></td>" +
                           "<td width='2%' height=19><font size=2>:</font></td>" +
                           "<td height=19><font size=2>" + Convert.ToString(t.Trial_FlyashUsed) + "</font></td>" +
                           "</tr>";

                        //mySql += "<tr>" +
                        //    "<td align=left valign=top height=19><font size=2> Cement Name </font></td>" +
                        //    "<td width='2%' height=19><font size=2>:</font></td>" +
                        //    "<td height=19><font size=2>" + Convert.ToString(t.Trial_CementUsed) + "</font></td>" +
                        //    "<td align=left valign=top height=19><font size=2><b> </b></font></td>" +
                        //    "<td height=19><font size=2> </font></td>" +
                        //    "<td height=19><font size=2></font></td>" +
                        //    "</tr>";
                        flg = true;
                    }
                }
            }
            string strTargetMeanStr = "---";
            Single stdDev = 0;
            if (Convert.ToDouble(mgrade) <= 15)
                stdDev = Convert.ToSingle("3.5");
            else if (Convert.ToDouble(mgrade) >= 20 && Convert.ToDouble(mgrade) <= 25)
                stdDev = Convert.ToSingle("4");
            else
                stdDev = Convert.ToSingle("5");
            strTargetMeanStr = mgrade + " +(1.65 x " + stdDev + " )= " + Convert.ToString(Convert.ToDouble(mgrade) + (1.65 * stdDev));
            strTargetMeanStr = strTargetMeanStr + " &nbsp;N/mm" + "<sup>2</sup>";
            mySql += "<tr>" +
                 "<td align=left valign=top height=19><font size=2>Target Mean Strength</font></td>" +
                 "<td height=19><font size=2>:</font></td>" +
                 "<td height=19><font size=2>" + strTargetMeanStr + "</font></td>" +
                 "<td align=left valign=top height=19><font size=2> </font></td>" +
                 "<td height=19><font size=2> </font></td>" +
                 "<td height=19><font size=2> </font></td>" +
                 "</tr>";
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Material</b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 8% align=center valign=top height=19 ><font size=2><b>Material Proportions</b></font></td>";
            mySql += "<td width= 8% align=center valign=top height=19 ><font size=2><b>Required Wt.(kg)</b></font></td>";
            mySql += "<td width= 8% align=center valign=top height=19 ><font size=2><b>Moisture Corrections </b></font></td>";
            mySql += "<td width= 8% align=center valign=top height=19 ><font size=2><b>Net Wt. Taken(kg) </b></font></td>";
            mySql += "<td width= 8% align=center valign=top height=19 ><font size=2><b>Corrections </b></font></td>";
            mySql += "</tr>";

            int FACount = 0, CACount = 0;
            var res = dc.TrialDetail_View(ReferenceNo, TrialId);
            foreach (var t in res)
            {
                if (t.TrialDetail_MaterialName != "Plastic Density" || t.TrialDetail_NetWeight != 0)
                {
                    mySql += "<tr>";
                    if (t.TrialDetail_MaterialName == "Natural Sand" || t.TrialDetail_MaterialName == "Crushed Sand"
                    || t.TrialDetail_MaterialName == "Stone Dust" || t.TrialDetail_MaterialName == "Grit")
                    {
                        FACount++;
                        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString("Fine Aggregate " + FACount) + "</font></td>";
                        mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_MaterialName + " - " + t.MaterialDetail_Information) + "</font></td>";
                    }
                    else if (t.TrialDetail_MaterialName == "10 mm" || t.TrialDetail_MaterialName == "20 mm"
                        || t.TrialDetail_MaterialName == "40 mm")
                    {
                        CACount++;
                        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString("Coarse Aggregate " + CACount) + "</font></td>";
                        mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_MaterialName + " - " + t.MaterialDetail_Information) + "</font></td>";
                    }
                    else if (t.TrialDetail_MaterialName == "Cement")
                    {
                        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_MaterialName) + "</font></td>";
                        mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.Trial_CementUsed) + "</font></td>";
                    }
                    else if (t.TrialDetail_MaterialName == "Admixture")
                    {
                        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_MaterialName) + "</font></td>";
                        mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.Trial_Admixture) + "</font></td>";
                    }
                    else if (t.TrialDetail_MaterialName == "Fly Ash")
                    {
                        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_MaterialName) + "</font></td>";
                        mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.Trial_FlyashUsed) + "</font></td>";
                    }
                    else
                    {
                        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_MaterialName) + "</font></td>";
                        mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.MaterialDetail_Information) + "</font></td>";
                    }
                    mySql += "<td width= 8% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_Weight) + "</font></td>";
                    mySql += "<td width= 8% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(t.TrialDetail_ReqdWt).ToString("0.00") + "</font></td>";
                    mySql += "<td width= 8% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(t.TrialDetail_Corrections) + "</font></td>";
                    mySql += "<td width= 8% align=center valign=top height=19 ><font size=2><b>&nbsp;" + Convert.ToDecimal(t.TrialDetail_NetWeight).ToString("0.00") + "</b></font></td>";
                    mySql += "<td width= 8% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "</tr>";
                }
            }
            mySql += "</table>";
            mySql += "</tr>";

            //mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            //mySql += "<tr><td colspan=6 align=left valign=top>";
            //mySql += "</tr>";

            # region commented
            //commented 03-07-2018
            //var data = dc.TrialDetail_View(ReferenceNo, TrialId);
            //foreach (var t in data)
            //{

            //    mySql += "<table>";
            //    mySql += "<tr>";
            //    if (t.Trial_MC_NS != "" && t.Trial_MC_NS != null)
            //    {
            //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "'Moist'. " + "Moisture Content" + "-" + "Natural Sand " + " " + ":" + " " + Convert.ToString(t.Trial_MC_NS) + " % " + "</font></td>";
            //    }

            //    if (t.Trial_MC_CS != "" && t.Trial_MC_CS != null)
            //    {
            //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "'Moist'. " + "Moisture Content" + "-" + "Crushed Sand " + " " + ":" + " " + Convert.ToString(t.Trial_MC_CS) + " % " + "</font></td>";

            //    }
            //    mySql += "</tr>";
            //    mySql += "<tr>";
            //    if (t.Trial_MC_SD != "" && t.Trial_MC_SD != null)
            //    {
            //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "'Moist'. " + "Moisture Content" + "-" + "Stone Dust " + " " + ":" + " " + Convert.ToString(t.Trial_MC_SD) + " % " + "</font></td>";
            //    }
            //    if (t.Trial_MC_GT != "" && t.Trial_MC_GT != null)
            //    {
            //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>" + "'Moist'. " + "Moisture Content" + "-" + "Grit " + " " + ":" + " " + Convert.ToString(t.Trial_MC_SD) + " % " + "</font></td>";
            //    }
            //    mySql += "</tr>";
            //    mySql += "<tr>";
            //    if (t.Trial_WA_NS != "" && t.Trial_WA_NS != null)
            //    {
            //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Water Absorption" + "-" + "Natural Sand " + " " + ":" + " " + Convert.ToString(t.Trial_WA_NS) + " % " + "</font></td>";
            //    }
            //    if (t.Trial_WA_CS != "" && t.Trial_WA_CS != null)
            //    {
            //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Water Absorption" + "-" + "Crushed Sand " + " " + ":" + " " + Convert.ToString(t.Trial_WA_CS) + " % " + "</font></td>";
            //    }
            //    mySql += "</tr>";
            //    mySql += "<tr>";
            //    if (t.Trial_WA_SD != "" && t.Trial_WA_SD != null)
            //    {
            //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Water Absorption" + "-" + "Stone Dust " + " " + ":" + " " + Convert.ToString(t.Trial_WA_SD) + " % " + "</font></td>";
            //    }
            //    if (t.Trial_WA_GT != "" && t.Trial_WA_GT != null)
            //    {
            //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>" + "Water Absorption" + "-" + "Grit " + " " + ":" + " " + Convert.ToString(t.Trial_WA_GT) + " % " + "</font></td>";
            //    }
            //    mySql += "</tr>";

            //    mySql += "<tr>";

            //    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Final Water Cement Ratio" + " " + ":" + " " + "</font></td>";

            //    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Wt. Of Compacted Concrete" + " " + ":" + " " + "</font></td>";

            //    mySql += "</tr>";

            //    mySql += "<tr>";

            //    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Slump " + " " + ":" + t.Trial_Slump.ToString() + "</font></td>";

            //    string Slump = "";
            //    if (t.Trial_RetentionStatus == true)
            //    {
            //        Slump = t.Trial_RetentionSlumpValue.ToString().Replace("|", ",");
            //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Retention Slump" + " " + ":" + t.Trial_RetentionSlumpValue.ToString() + "(" + " " + Slump + " " + ")" + "</font></td>";
            //    }
            //    mySql += "</tr>";

            //    mySql += "<tr>";

            //    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Yield " + " " + ":" + "" + "</font></td>";

            //    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> </font></td>";

            //    mySql += "</tr>";
            //    mySql += "<tr>";

            //    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Compaction Factor " + " " + ":" + " " + "</font></td>";

            //    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Segregation  " + " " + ":" + " " + "</font></td>";

            //    mySql += "</tr>";
            //    mySql += "<tr>";

            //    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> " + "Remark " + " " + ":" + Convert.ToString(t.Trial_Remark) + "</font></td>";

            //    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2> </font></td>";

            //    mySql += "</tr>";
            //    break;
            //}
            #endregion
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2>Retention time (min)</font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Initial</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>30</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>60 </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>90 </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>120 </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>150 </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>180 </b></font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2>&nbsp;" + mtest + "</font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
            mySql += "</tr>";
            mySql += "</table>";
            mySql += "</tr>";

            //mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            //mySql += "<tr><td colspan=6 align=left valign=top>";
            //mySql += "</tr>";

            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";

            mySql += "<table width=100%>";
            mySql += "<tr>";
            mySql += "<td width= 7% align=left valign=top height=19 ><font size=2> " + "Yield " + "&nbsp;&nbsp;" + "</font></td>";
            mySql += "<td width= 10%  align=left valign=top height=20 ><font size=2><hr>&nbsp;</font></td>";
            mySql += "<td width= 40% align=left valign=top height=19 ><font size=2> " + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Weight of concrete in cylinder (kg)" + "&nbsp;&nbsp;" + "</font></td>";
            mySql += "<td width= 20%  align=left valign=top height=20 ><font size=2><hr>&nbsp;</font></td>";
            mySql += "<td width= 20%  align=left valign=top height=20 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 6% align=left valign=top height=19 ><font size=2> " + "Comment " + "&nbsp;&nbsp;" + "</font></td>";
            //mySql += "<td width= 50%  align=left valign=top height=20 ><font size=2><p><hr>&nbsp;</p></font></td>";
            mySql += "<td width= 90%  align=left valign=top height=20 colspan=4><font size=2><hr>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "</table>";
            mySql += "</tr>";

            mySql += "<table width=100%>";
            mySql += "<tr>";
            mySql += "<td width=8% align=left valign=top height=20 ><font size=2>" + "Cast By " + "</font></td>";
            mySql += "<td width=20%  align=left valign=top height=20 ><font size=2><hr>&nbsp;</font></td>";
            mySql += "<td width=15% align=right valign=top height=20 ><font size=2>" + "Data Entered By " + "</font></td>";
            mySql += "<td width=25%  align=left valign=top height=20 ><font size=2><hr>&nbsp;</font></td>";
            mySql += "<td width=10% align=right valign=top height=20 ><font size=2>" + "Checked By " + "</font></td>";
            mySql += "<td width=20%  align=left valign=top height=20 ><font size=2><hr></font></td>";
            mySql += "</tr>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Id Mark </b></font></td>";
            mySql += "<td width= 10% align=center colspan=3 valign=top height=19 ><font size=2><b> Dimensions(mm) </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Weight </b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b> Age (Days) </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Load (kN) </b></font></td>";
            //mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Str(N/mm<sup>2</sup>) </b></font></td>";
            //mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Avg. Str. </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Tested by </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Data Entered by </b></font></td>";
            mySql += "</tr>";

            for (int j = 0; j < 12; j++)
            {
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "</tr>";
            }

            mySql += "</table>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("TrialInformation", mySql);
        }

        public void TrialProportion_Html(string ReferenceNo, int TrialId)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            //mySql += "<tr><td width='100%' height='10'>";
            //mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr><td width='99%' align=center colspan=7 height=19><font size=4><b> Trial Proportions </b></font> </td></tr>";
            #region Header
            int RecordNo = 0;
            var MFInwd = dc.ReportStatus_View("Mix Design", null, null, 0, 0, 0, ReferenceNo, 0, 0, 0);
            foreach (var m in MFInwd)
            {

                mySql += "<tr>" +
                "<td width='20%' align=left valign=top height=19><font size=2>Customer Name</font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td width='40%' height=19><font size=2>" + m.CL_Name_var + "</font></td>" +
                "<td height=19><font size=2>Ref No.</font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td height=19><font size=2>" + "MF" + "-" + m.MFINWD_ReferenceNo_var + "</font></td>" +
                "<td height=19><font size=2>&nbsp;</font></td>" +
                "</tr>";

                mySql += "<tr>" +
                  "<td width='20%' align=left valign=top height=19><font size=2>Site Name</font></td>" +
                   "<td width='2%' height=19><font size=2>:</font></td>" +
                   "<td width='40%' height=19><font size=2>" + m.SITE_Name_var + "</font></td>" +
                    "<td align=left valign=top height=19><font size=2>Grade of Concrete</font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + m.MFINWD_Grade_var + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +

                   "<td width='20%' height=19><font size=2>Special Requirement</font></td>" +
                   "<td width='2%' height=19><font size=2>:</font></td>" +
                   "<td width='10%' height=19><font size=2>" + m.MFINWD_SpecialRequirement_var + "</font></td>" +
                   "<td height=19><font size=2>Nature of Work</font></td>" +
                   "<td width='2%' height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + m.MFINWD_NatureofWork_var.ToString() + "</font></td>" +
                   "</tr>";

                mySql += "<tr>" +

                   "<td width='20%' height=19><font size=2><b></b></font></td>" +
                   "<td width='2%' height=19><font size=2></font></td>" +
                   "<td width='10%' height=19><font size=2> </font></td>" +
                   "<td height=19><font size=2> Slump Requirement </font></td>" +
                   "<td width='2%' height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + m.MFINWD_Slump_var.ToString() + "</font></td>" +
                   "</tr>";


                RecordNo = Convert.ToInt32(m.MFINWD_RecordNo_int);
                break;
            }
            //var trial = dc.TrialDetail_View(ReferenceNo, TrialId);
            Int32 aggCount = 0;
            clsData obj = new clsData();
            string tsql = "select b.MATERIAL_Id,Material_List,MaterialDetail_Information  ,Material_Type from tbl_MaterialDetail a,tbl_MaterialList  b ";
            tsql += "  where a.MaterialDetail_Id=b.MATERIAL_Id and a.MaterialDetail_RefNo='" + ReferenceNo + "'";
            DataTable dt = obj.getGeneralData(tsql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Material_Type"].ToString() == "Aggregate")
                {
                    aggCount++;
                }
                if (dt.Rows[i]["Material_List"].ToString() == "Cement")
                {
                    mySql += "<tr>" +

                   "<td align=left valign=top height=19><font size=2>Cement Name</font></td>" +
                   "<td height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + Convert.ToString(dt.Rows[i]["MaterialDetail_Information"]) + "</font></td>" +
                   "<td align=left valign=top height=19><font size=2> Record No. </font></td>" +
                   "<td width='2%' height=19><font size=2>  :  </font></td>" +
                   "<td height=19><font size=2>" + " MF- " + RecordNo.ToString() + "</font></td>" +
                   "</tr>";

                }
                else if (dt.Rows[i]["Material_List"].ToString() == "Admixture")
                {
                    mySql += "<tr>" +

                  "<td align=left valign=top height=19><font size=2>Admixture used </font></td>" +
                  "<td height=19><font size=2>:</font></td>" +
                  "<td height=19><font size=2>" + Convert.ToString(dt.Rows[i]["MaterialDetail_Information"]) + "</font></td>" +
                  "<td align=left valign=top height=19><font size=2> </font></td>" +
                  "<td width='2%' height=19><font size=2> </font></td>" +
                  "<td height=19><font size=2> </font></td>" +
                  "</tr>";
                }
                else if (dt.Rows[i]["Material_List"].ToString() == "G G B S")
                {
                    mySql += "<tr>" +

                 "<td align=left valign=top height=19><font size=2>GGBS used </font></td>" +
                 "<td height=19><font size=2>:</font></td>" +
                 "<td height=19><font size=2>" + Convert.ToString(dt.Rows[i]["MaterialDetail_Information"]) + "</font></td>" +
                 "<td align=left valign=top height=19><font size=2> </font></td>" +
                 "<td width='2%' height=19><font size=2> </font></td>" +
                 "<td height=19><font size=2> </font></td>" +
                 "</tr>";
                }
                else if (dt.Rows[i]["Material_List"].ToString() == "Fly Ash")
                {
                    mySql += "<tr>" +

                    "<td align=left valign=top height=19><font size=2>FlyAsh used </font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + Convert.ToString(dt.Rows[i]["MaterialDetail_Information"]) + "</font></td>" +
                    "<td align=left valign=top height=19><font size=2> </font></td>" +
                    "<td width='2%' height=19><font size=2> </font></td>" +
                    "<td height=19><font size=2> </font></td>" +
                    "</tr>";
                }
            }
            #endregion
            #region no.of trials
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 8% align=left valign=top height=19 ><font size=2>&nbsp;<b> Trial Date : </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td width= 8% align=left valign=top height=19 ><font size=2>&nbsp;<b> Material </b> </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Trial 1 </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Trial 2 </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Trial 3 </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Trial 4 </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Trial 5 </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Trial 6 </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Final Trial  </font></td>";
            mySql += "</tr>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //if (dt.Rows[i]["Material_Type"].ToString() == "Aggregate")
                //{
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + dt.Rows[i]["Material_List"].ToString() + "</font></td>";
                    for (int j = 0; j < 7; j++)
                    {
                        mySql += "<td width= 5% align=center valign=top height=19 > </td>";
                    }
                    mySql += "</tr>";

                //}
            }
            #endregion trials
            #region IS-Passing
            mySql += "<tr>";
            mySql += "<td  width= 5% align=left valign=top height=5 >&nbsp; Total </td>";
            for (int j = 0; j < 7; j++)
            {
                mySql += "<td width= 5% align=center valign=top height=5 > </td>";
            }
            mySql += "</tr>";

            mySql += "<table>";


            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            string[,] matAgg = new string[13, (aggCount * 2) + 1];

            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < matAgg.GetUpperBound(1); j++)
                {
                    matAgg[i, j] = "";
                }
            }
            string[] RefNo1 = Convert.ToString(ReferenceNo).Split('/');
            Int32 rwCnt = 0, colCnt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Material_Type"].ToString() == "Aggregate")
                {
                    Int32 mtID = Convert.ToInt32(dt.Rows[i]["MATERIAL_Id"].ToString());
                    var aggtTest = dc.MFSieveAnalysis(RefNo1[0].ToString() + "/%", Convert.ToInt32(mtID));
                    foreach (var aggSA in aggtTest)
                    {
                        matAgg[rwCnt, colCnt] = aggSA.AGGTSA_SeiveSize_var.ToString();
                        matAgg[rwCnt, colCnt + 1] = aggSA.AGGTSA_CumuPassing_dec.ToString();



                        rwCnt = rwCnt + 1;
                        if (aggSA.AGGTSA_SeiveSize_var.ToString() == "Total")
                        {
                            matAgg[11, colCnt] = "Specific Gravity";
                            matAgg[11, colCnt + 1] = aggSA.AGGTINWD_SpecificGravity_var.ToString();

                            matAgg[12, colCnt] = "W/A";
                            matAgg[12, colCnt + 1] = aggSA.AGGTINWD_WaterAborp_var.ToString();
                            break;
                        }
                    }
                    rwCnt = 0;
                    colCnt = colCnt + 2;
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Material_Type"].ToString() == "Aggregate")
                {
                    mySql += "<td width= 5% align=center valign=top height=19 colspan=2 ><font size=2><b>&nbsp;" + dt.Rows[i]["Material_List"].ToString() + "</b></font></td>";
                }
            }

            mySql += "</tr>";
            mySql += "<tr>";
            for (int j = 0; j < aggCount; j++)
            {
                mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2><b> Sieve Sizes </b></font></td>";
                mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2><b> Passing </b></font></td>";
            }
            mySql += "</tr>";

            for (int i = 0; i < 13; i++)
            {
                mySql += "<tr>";
                for (int j = 0; j < matAgg.GetUpperBound(1); j++)
                {
                    mySql += "<td width= 5% align=center  valign=top height=19 ><font size=2>" + matAgg[i, j].ToString() + " </font></td>";
                }
                mySql += "</tr>";
            }
            mySql += "<table>";
            #endregion
            #region followup & MDL-Final detail
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Date </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b> Follow up </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Contact Person </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Follow up By </b></font></td>";
            mySql += "</tr>";

            for (int j = 0; j < 10; j++)
            {
                mySql += "<tr>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "</tr>";
            }
            mySql += "<table>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Trial No. </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Ent. By </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Ent. Date </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Chk. By </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Chk. Date </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Prn. By </font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> Prn. Date </font></td>";
            mySql += "</tr>";

            for (int j = 0; j < 2; j++)
            {
                mySql += "<tr>";
                if (j == 0)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;MD Letter </font></td>";
                }
                else
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;Final Report </font></td>";
                }
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp; </font></td>";
                mySql += "</tr>";
            }

            mySql += "</table>";
            #endregion

            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            DownloadHtmlReport("TrialProportion", mySql);
        }
        public void LedegerView_Html(string CategoryName, int CategoryId, string costDesc, int CostID)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            mySql += "<tr>" +
           "<td width='15%' align=left valign=top height=19><font size=2><b> Catagory Name  </b></font></td>" +
           "<td width='2%' height=19><font size=2>:</font></td>" +
           "<td width='40%' height=19><font size=2>" + CategoryName + "</font></td>" +
           "</tr>";
            mySql += "<tr>" +
            "<td width='15%' align=left valign=top height=19><font size=2><b> Cost Center Description  </b></font></td>" +
            "<td width='2%' height=19><font size=2>:</font></td>" +
            "<td width='40%' height=19><font size=2>" + costDesc + "</font></td>" +
            "</tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=80% id=AutoNumber1>";

            int SrNo = 0;
            var data = dc.Ledger_View(CategoryId, CostID, false, "");
            foreach (var ledg in data)
            {
                if (SrNo == 0)
                {
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b> Sr No </b></font></td>";
                    mySql += "<td width= 50% align=center valign=medium height=19 ><font size=2><b> Ledger Name </b></font></td>";
                    mySql += "</tr>";
                }
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2> " + SrNo + " </font></td>";
                mySql += "<td width= 50% align=center valign=medium height=19 ><font size=2> " + Convert.ToString(ledg.LedgerName_Description) + "   </font></td>";
                mySql += "</tr>";
            }
            if (SrNo == 0)
            {
                mySql += "<tr>";
                mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2> There are no records  ! </font></td>";
                mySql += "</tr>";
            }
            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("Ledger_" + CategoryName, mySql);
        }



        public void TrialSieveAnalysis_Html_old(string ReferenceNo, string RecType)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            int RecordNo = 0;

            var AggtTest = dc.ReportStatus_View("Aggregate Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);
            foreach (var aggt in AggtTest)
            {

                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Test Report</b></font></td></tr>";
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Fine & Coarse Aggregate </b></font></td></tr>";
                mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

                mySql += "<tr>" +
                    "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='40%' height=19><font size=2>" + aggt.CL_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + System.DateTime.Now.ToString("dd/MMM/yy") + "</font></td>" +
                    "</tr>";

                mySql += "<tr>" +

                    "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='10%' height=19><font size=2>" + aggt.SITE_Name_var + "</font></td>" +
                    "<td height=19><font size=2><b>Record No.</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + RecType + "-" + aggt.AGGTINWD_ReferenceNo_var.ToString() + "</font></td>" +
                    "</tr>";

                RecordNo = Convert.ToInt32(aggt.AGGTINWD_RecordNo_int);
                break;
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";


            mySql += "<table>";
            mySql += "<tr><td  width=20% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td></tr>";

            bool SpecGrav = false; bool lbd = false; bool Moist = false; bool Sild = false; bool Flaki = false; //bool Impact = false; bool Elong = false; bool Crush = false;
            var MFTestname = dc.AllInwdDetails_View(ReferenceNo, "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, RecType);
            foreach (var aggtt in MFTestname)
            {
                if (RecType == "MF")
                {
                    if (aggtt.AGGTINWD_ImpactValue_var != null && aggtt.AGGTINWD_ImpactValue_var != "")
                    {
                        //Impact = true;
                    }
                    if (aggtt.AGGTINWD_Flakiness_var != null && aggtt.AGGTINWD_Flakiness_var != "")
                    {
                        Flaki = true;
                    }
                    if (aggtt.AGGTINWD_Flakiness_var != null && aggtt.AGGTINWD_Flakiness_var != "")
                    {
                        Flaki = true;
                    }
                    if (aggtt.AGGTINWD_LBD_var != null && aggtt.AGGTINWD_LBD_var != "")
                    {
                        lbd = true;
                    }
                    if (aggtt.AGGTINWD_MoistureContent_var != null && aggtt.AGGTINWD_MoistureContent_var != "")
                    {
                        Moist = true;
                    }
                    if (aggtt.AGGTINWD_SpecificGravity_var != null && aggtt.AGGTINWD_SpecificGravity_var != "")
                    {
                        SpecGrav = true;
                    }
                    if (aggtt.AGGTINWD_WaterAborp_var != null && aggtt.AGGTINWD_WaterAborp_var != "")
                    {
                        SpecGrav = true;
                    }
                    if (aggtt.AGGTINWD_SildContent_var != null && aggtt.AGGTINWD_SildContent_var != "")
                    {
                        Sild = true;
                    }
                    if (aggtt.AGGTINWD_Elongation_var != null && aggtt.AGGTINWD_Elongation_var != "")
                    {
                        //Elong = true;
                    }
                    if (aggtt.AGGTINWD_CrushingValue_var != null && aggtt.AGGTINWD_CrushingValue_var != "")
                    {
                        //Crush = true;
                    }
                }
            }

            int SrNo = 0;
            var Mix = dc.MaterialDetail_View(RecordNo, "", 0, "", null, null, "");
            foreach (var m in Mix)
            {
                if (SrNo != Convert.ToInt32(m.MaterialDetail_SrNo) || SrNo == 0)
                {
                    var MfInwd = dc.MF_View(ReferenceNo, Convert.ToInt32(m.Material_Id), RecType);
                    foreach (var aggt in MfInwd)
                    {
                        if (aggt.AGGTINWD_AggregateName_var == "Natural Sand" || aggt.AGGTINWD_AggregateName_var == "Crushed Sand" || aggt.AGGTINWD_AggregateName_var == "Stone Dust" || aggt.AGGTINWD_AggregateName_var == "Grit")
                        {
                            mySql += "<table>";
                            mySql += "<tr><td width= 3% align=left valign=top height=19 ><font size=2><b>" + "Fine Aggregate" + " (" + Convert.ToString(m.Material_List) + ")" + "</b></font></td></tr>";

                            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
                            mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
                            mySql += "<td width= 2% align=left  valign=top height=19 rowspan=4  ><font size=2> " + " " + " </font></td>";
                            mySql += "<td width= 3% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
                            mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
                            mySql += "</tr>";

                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Specific Gravity" + "</font></td>";
                            if (SpecGrav == true)
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SpecificGravity_var) + "</font></td>";
                            }
                            else
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                            }
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";

                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Moisture Content" + " </font></td>";
                            if (Moist == true)
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_MoistureContent_var) + "</font></td>";
                            }
                            else
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                            }
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " % " + "</font></td>";
                            mySql += "</tr>";

                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Water Absorption" + "</font></td>";
                            if (SpecGrav == true)
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_WaterAborp_var) + "</font></td>";
                            }
                            else
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                            }
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "% " + "</font></td>";

                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Condition of the Sample" + "</font></td>";
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " Satisfactory" + "</font></td>";

                            //mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SampleCondition_var) + "</font></td>";

                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "  " + "</b></font></td>";
                            mySql += "</tr>";

                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=middle height=19 ><font size=2>&nbsp;" + "Loose Bulk Density" + "</font></td>";
                            if (lbd == true)
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_LBD_var) + "</font></td>";
                            }
                            else
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                            }
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "kg/lit " + "</font></td>";

                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Material finer than 75 u </br> (by wet sieving)" + "</font></td>";
                            if (Sild == true)
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SildContent_var) + "</font></td>";
                            }
                            else
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                            }
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " %  " + "</font></td>";
                            mySql += "</tr>";
                        }
                        if (aggt.AGGTINWD_AggregateName_var == "10 mm" || aggt.AGGTINWD_AggregateName_var == "20 mm" || aggt.AGGTINWD_AggregateName_var == "40 mm" || aggt.AGGTINWD_AggregateName_var == "Mix Aggt")
                        {
                            mySql += "<table>";
                            if (Convert.ToString(aggt.AGGTINWD_AggregateName_var) != "Mix Aggt")
                            {

                                mySql += "<tr><td width= 3% align=left valign=top height=19 ><font size=2><b>" + "Coarse Aggregate" + " (" + Convert.ToString(m.Material_List) + ")" + "</b></font></td></tr>";
                            }
                            else
                            {
                                mySql += "<tr><td width= 3% align=left valign=top height=19 ><font size=2><b>" + "Fine /Coarse Aggregate" + " (" + Convert.ToString(m.Material_List) + ")" + "</b></font></td></tr>";
                            }
                            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
                            mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
                            mySql += "<td width= 2% align=left  valign=top height=19 rowspan=4  ><font size=2> " + " " + " </font></td>";
                            mySql += "<td width= 3% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
                            mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
                            mySql += "</tr>";

                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Specific Gravity" + "</font></td>";
                            if (SpecGrav == true)
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SpecificGravity_var) + "</font></td>";
                            }
                            else
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                            }
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";

                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Condition of the Sample" + "</font></td>";

                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SampleCondition_var) + "</font></td>";

                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "  " + "</b></font></td>";
                            mySql += "</tr>";

                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Water Absorption" + "</font></td>";
                            if (SpecGrav == true)
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_WaterAborp_var) + "</font></td>";
                            }
                            else
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                            }
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "% " + "</font></td>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Flakiness Index" + "</font></td>";
                            if (Flaki == true)
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_Flakiness_var) + "</font></td>";
                            }
                            else
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                            }
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "%" + "</font></td>";

                            mySql += "</tr>";

                            mySql += "<tr>";
                            mySql += "<td width= 2% align=left valign=middle height=19 ><font size=2>&nbsp;" + "Loose Bulk Density" + "</font></td>";
                            if (lbd == true)
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_LBD_var) + "</font></td>";
                            }
                            else
                            {
                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                            }
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "kg/lit " + "</font></td>";
                            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "&nbsp;" + "</font></td>";
                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "&nbsp;" + "</font></td>";
                            mySql += "</tr>";

                            //mySql += "<tr>";
                            //mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Elongness Value" + "</font></td>";
                            //if (Elong == true)
                            //{
                            //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_Elongation_var) + "</font></td>";
                            //}
                            //else
                            //{
                            //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                            //}
                            //mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";
                            //mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Crushing Value" + "</font></td>";
                            //if (Crush == true)
                            //{
                            //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_CrushingValue_var) + "</font></td>";
                            //}
                            //else
                            //{
                            //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
                            //}
                            //mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "  " + "</font></td>";
                            //mySql += "</tr>";
                        }

                        int i = 0;
                        bool addCol = false;
                        var aggtTest = dc.AggregateAllTestView(ReferenceNo, Convert.ToInt32(m.Material_Id), "AGGTSA");
                        foreach (var aggtt in aggtTest)
                        {
                            if (i == 0)
                            {
                                mySql += "<table>";
                                mySql += "<tr>";
                                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "Sieve Analysis (by dry sieving) " + "</b></font></td>";
                                mySql += "</tr>";

                                mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

                                mySql += "<tr>";
                                mySql += "<td width= 2%  rowspan=2 align=center valign=middle height=19 ><font size=2><b>Sieve Size</b></font></td>";
                                mySql += "<td width= 10%  align=center colspan=3 valign=top height=19 ><font size=2><b>Weight retained</b></font></td>";
                                mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2><b>Passing </b></font></td>";
                                if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
                                {
                                    mySql += "<td width= 2%   align=center rowspan=2 valign=middle height=19 ><font size=2><b>IS Passing % Limits </b></font></td>";
                                    addCol = true;
                                }
                                mySql += "</tr>";
                                mySql += "<tr>";
                                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(g)</b></font></td>";
                                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(%)</b></font></td>";
                                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>Cummu (%)</b></font></td>";
                                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(%)</b></font></td>";
                                mySql += "</tr>";
                            }
                            mySql += "<tr>";
                            mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_SeiveSize_var.ToString() + "</font></td>";
                            mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_Weight_num.ToString() + "</font></td>";
                            if (aggtt.AGGTSA_SeiveSize_var != "Total")
                            {
                                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + Convert.ToDecimal(aggtt.AGGTSA_WeightRet_dec).ToString("0.00") + "</font></td>";
                                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_CumuWeightRet_dec.ToString() + "</font></td>";
                                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_CumuPassing_dec.ToString() + "</font></td>";
                                if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
                                {
                                    mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2> " + aggtt.AGGTSA_IsPassingLmt_var.ToString() + " </font></td>";
                                }
                            }
                            else if (aggtt.AGGTSA_SeiveSize_var == "Total")
                            {
                                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + "" + "</font></td>";
                                if (aggt.AGGTINWD_AggregateName_var == "Natural Sand" || aggt.AGGTINWD_AggregateName_var == "Crushed Sand" || aggt.AGGTINWD_AggregateName_var == "Stone Dust" || aggt.AGGTINWD_AggregateName_var == "Grit")
                                {
                                    mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + "Fineness Modulus" + "</font></td>";
                                    mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggtt.AGGTINWD_FM_var) + "</font></td>";
                                }
                                else
                                {
                                    mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2> " + "" + " </font></td>";
                                    mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2> " + "" + " </font></td>";
                                }
                                if (addCol == true)
                                {
                                    mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2> " + "" + " </font></td>";
                                }

                            }
                            i++;

                            mySql += "</tr>";

                        }
                        //mySql += "<table>";
                        //mySql += "<tr><td width= 10% align=left valign=top height=10 ><font size=2>&nbsp;</font></td></tr>";
                    }
                }
                SrNo = Convert.ToInt32(m.MaterialDetail_SrNo);
            }

            mySql += "<table>";
            mySql += "<tr><td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td></tr>";

            //SrNo = 0;
            //var matid = dc.Material_View("AGGT");
            //foreach (var m in matid)
            //{
            //    var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
            //    foreach (var cd in iscd)
            //    {
            //        if (SrNo == 0)
            //        {
            //            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td></tr>";
            //        }
            //        SrNo++;
            //        mySql += "<tr><td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td></tr>";
            //    }
            //}

            //SrNo = 0;
            //var re = dc.AllRemark_View("", txt_RefNo.Text, 0, "AGGT");
            //foreach (var r in re)
            //{
            //    var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.AGGTDetail_RemarkId_int), "AGGT");
            //    foreach (var remk in remark)
            //    {
            //        if (SrNo == 0)
            //        {
            //            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td></tr>";
            //        }
            //        SrNo++;
            //        mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.AGGT_Remark_var.ToString() + "</font></td></tr>";
            //    }
            //}

            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td></tr>";
            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td></tr>";
            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td></tr>";
            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td></tr>";
            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td></tr>";

            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td></tr>";
            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Sachin Gaikwad" + "</font></td></tr>";
            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "(Asst. Manager)" + "</font></td></tr>";
            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td></tr>";
            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td></tr>";
            //if (System.Web.HttpContext.Current.Session["Check"] != null)
            //{
            //    var RecNo = dc.ReportStatus_View("Aggregate Testing", null, null, 1, 0, 0, txt_RefNo.Text, 0, 0, 0);
            //    foreach (var r in RecNo)
            //    {
            //        if (Convert.ToString(r.AGGTINWD_ApprovedBy_tint) != string.Empty)
            //        {
            //            var U = dc.User_View(r.AGGTINWD_ApprovedBy_tint, -1, "", "");
            //            foreach (var r1 in U)
            //            {
            //                mySql += "<tr><td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td></tr>";
            //                mySql += "<tr><td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td></tr>";
            //            }
            //        }
            //mySql += "<tr>";
            //if (WitnessBy != string.Empty)
            //{
            //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> Witness by : " + WitnessBy + " </font></td>";
            //}
            //if (Convert.ToString(r.AGGTINWD_CheckedBy_tint) != string.Empty)
            //{
            //    var lgin = dc.User_View(r.AGGTINWD_CheckedBy_tint, -1, "", "");
            //    foreach (var loginusr in lgin)
            //    {
            //        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            //        mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
            //    }
            //    mySql += "</tr>";
            //}
            //    }
            //}


            mySql += "<tr><td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td></tr>";
            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td></tr>";


            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("TrialSieveAnalysis_" + ReferenceNo.Replace("/", "-"), mySql);
        }
        public void EnquiryView_Html(int EnqId)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Enquiry View </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var viewEnquiry = dc.Enquiry_View(EnqId, -1, 0);
            foreach (var Enq in viewEnquiry)
            {
                var coupon = dc.Coupon_View("", 0, 0, Enq.CL_Id, Enq.SITE_Id, 0, DateTime.Now).ToList();
                mySql += "<tr>" +
                        "<td width='15%' align=left valign=top height=19><font size=2><b> Client Name  </b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='50%' height=19><font size=2>" + Enq.CL_Name_var + "</font></td>" +
                        "<td width='15%' align=left valign=top height=19><font size=2><b> Balance  </b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + Convert.ToDecimal(Enq.CL_BalanceAmt_mny).ToString("0.00") + "</font></td>" +
                    "</tr>";
                mySql += "<tr>" +
                     "<td width='15%' align=left valign=top height=19><font size=2><b> Site Name  </b></font></td>" +
                     "<td width='2%' height=19><font size=2>:</font></td>" +
                     "<td width='50%' height=19><font size=2>" + Enq.SITE_Name_var + "</font></td>" +
                     "<td width='15%' align=left valign=top height=19><font size=2><b> Limit  </b></font></td>" +
                     "<td width='2%' height=19><font size=2>:</font></td>" +
                     "<td width='40%' height=19><font size=2>" + Convert.ToDecimal(Enq.CL_Limit_mny).ToString("0.00") + "</font></td>" +
                  "</tr>";
                mySql += "<tr>" +
                    "<td width='15%' align=left valign=top height=19><font size=2><b> Site Address  </b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='50%' height=19><font size=2>" + Enq.SITE_Address_var + "</font></td>" +
                    "<td width='15%' align=left valign=top height=19><font size=2><b> No. of coupons  </b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='40%' height=19><font size=2>" + coupon.Count().ToString() + "</font></td>" +
               "</tr>";
                mySql += "<tr>" +
                    "<td width='15%' align=left valign=top height=19><font size=2><b> Land Mark  </b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='50%' height=19><font size=2>" + Enq.SITE_Landmark_var + "</font></td>" +
                    "<td width='15%' align=left valign=top height=19><font size=2><b> Quantity  </b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='50%' height=19><font size=2>" + Enq.ENQ_Quantity + "</font></td>" +
                 "</tr>";
                mySql += "<tr>" +
                    "<td width='15%' align=left valign=top height=19><font size=2><b> Contact Person  </b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='50%' height=19><font size=2>" + Enq.CONT_Name_var + "</font></td>" +
                    "</tr>";
                mySql += "<tr>" +
                     "<td width='15%' align=left valign=top height=19><font size=2><b> Contact No.  </b></font></td>" +
                     "<td width='2%' height=19><font size=2>:</font></td>" +
                     "<td width='50%' height=19><font size=2>" + Enq.CONT_ContactNo_var + "</font></td>" +
                     "</tr>";
                mySql += "<tr>" +
                     "<td width='15%' align=left valign=top height=19><font size=2><b> Inward Type  </b></font></td>" +
                     "<td width='2%' height=19><font size=2>:</font></td>" +
                     "<td width='50%' height=19><font size=2>" + Enq.MATERIAL_Name_var + "</font></td>" +
                    "</tr>";
                if (Enq.ENQ_OpenEnquiryStatus_var == "To be Collected")
                {
                    mySql += "<tr><td width='15%' align=left valign=top height=19><font size=2><u><b> To be Collected   </b></u></font></td></tr>";
                    var ld = dc.Location_View(Convert.ToInt32(Enq.ENQ_LOCATION_Id), "", 0);
                    foreach (var l in ld)
                    {
                        mySql += "<tr>" +
                           "<td width='15%' align=left valign=top height=19><font size=2><b> Location   </b></font></td>" +
                           "<td width='2%' height=19><font size=2>:</font></td>" +
                           "<td width='40%' height=19><font size=2>" + l.LOCATION_Name_var + "</font></td>" +
                           "</tr>";
                        break;
                    }
                    var rt = dc.Route_View(Convert.ToInt32(Enq.ENQ_ROUTE_Id), "", "False", 0);
                    foreach (var r in rt)
                    {
                        mySql += "<tr>" +
                          "<td width='15%' align=left valign=top height=19><font size=2><b> Route Name  </b></font></td>" +
                          "<td width='2%' height=19><font size=2>:</font></td>" +
                          "<td width='40%' height=19><font size=2>" + r.ROUTE_Name_var + "</font></td>" +
                          "</tr>";
                        break;
                    }
                    mySql += "<tr>" +
                       "<td width='15%' align=left valign=top height=19><font size=2><b> Collection Date  </b></font></td>" +
                       "<td width='2%' height=19><font size=2>:</font></td>" +
                       "<td width='40%' height=19><font size=2>" + Convert.ToDateTime(Enq.ENQ_CollectionDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    if (Enq.ENQ_UrgentStatus_bit == true)
                    {
                        mySql += "<tr>" +
                       "<td width='15%' align=left valign=top height=19><font size=2><b> Client Expected Date  </b></font></td>" +
                       "<td width='2%' height=19><font size=2>:</font></td>" +
                       "<td width='40%' height=19><font size=2>" + Convert.ToDateTime(Enq.ENQ_ClientExpectedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";
                    }
                }
                else if (Enq.ENQ_OpenEnquiryStatus_var == "Already Collected")
                {
                    if (Enq.ENQ_CollectedAt_var == "By Driver")
                    {
                        var drv = dc.User_View(Convert.ToInt32(Enq.ENQ_DriverId), -1, "", "", "");
                        foreach (var d in drv)
                        {
                            mySql += "<tr><td width='15%' align=left valign=top height=19><font size=2><b><u> Already Collected  </u> </b></font></td><tr>";
                            mySql += "<tr>" +
                           "<td width='15%' align=left valign=top height=19><font size=2><b>  Driver Name  </b></font></td>" +
                           "<td width='2%' height=19><font size=2>:</font></td>" +
                           "<td width='40%' height=19><font size=2>" + d.USER_Name_var + "</font></td>" +
                           "</tr>";
                        }
                    }
                    else if (Enq.ENQ_CollectedAt_var == "At Lab")
                    {
                        mySql += "<tr><td width='15%' align=left valign=top height=19><font size=2><b><u> Already Collected  </u> </b></font></td>" +
                        "<td width='2%' height=19><font size=2> :</font></td>" +
                        "<td width='50%' height=19><font size=2>" + Enq.ENQ_CollectedAt_var + "</font></td>" +
                        "<td width='15%' align=left valign=top height=19><font size=2><b>   </b></font></td>" +
                        "<td width='2%' height=19><font size=2></font></td>" +
                        "</tr>";
                    }
                }
                else if (Enq.ENQ_OpenEnquiryStatus_var == "Decision Pending")
                {
                    mySql += "<tr>" +
                    "<td width='15%' align=left valign=top height=19><font size=2><b><u> " + Enq.ENQ_OpenEnquiryStatus_var + " </u></b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='40%' height=19><font size=2>" + Enq.ENQ_Comment_var + "</font></td>" +
                    "</tr>";
                }
                else if (Enq.ENQ_OpenEnquiryStatus_var == "On site Testing")
                {
                    mySql += "<tr>" +
                    "<td width='15%' align=left valign=top height=19><font size=2><b><u> " + Enq.ENQ_OpenEnquiryStatus_var + " </u></b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='40%' height=19><font size=2>" + Convert.ToDateTime(Enq.ENQ_TestingDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                    "</tr>";
                }
                else if (Enq.ENQ_OpenEnquiryStatus_var == "Declined by us")
                {
                    mySql += "<tr>" +
                   "<td width='15%' align=left valign=top height=19><font size=2><b><u> " + Enq.ENQ_OpenEnquiryStatus_var + "  </u></b></font></td>" +
                   "<td width='2%' height=19><font size=2>:</font></td>" +
                   "<td width='40%' height=19><font size=2>" + Enq.ENQ_Comment_var + "</font></td>" +
                   "</tr>";
                }
                else if (Enq.ENQ_OpenEnquiryStatus_var == "Rejected By Client" || Enq.ENQ_OpenEnquiryStatus_var == "Declined By Client")
                {
                    mySql += "<tr>" +
                   "<td width='15%' align=left valign=top height=19><font size=2><b><u> " + Enq.ENQ_OpenEnquiryStatus_var + "  </u></b></font></td>" +
                   "<td width='2%' height=19><font size=2>:</font></td>" +
                   "<td width='40%' height=19><font size=2>" + Enq.ENQ_Comment_var + "</font></td>" +
                   "</tr>";
                }
                else if (Enq.ENQ_OpenEnquiryStatus_var == "Material Sending On Date" || Enq.ENQ_OpenEnquiryStatus_var == "Delivered by Client")
                {
                    mySql += "<tr>" +
                    "<td width='15%' align=left valign=top height=19><font size=2><b><u> " + Enq.ENQ_OpenEnquiryStatus_var + "  </u></b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='40%' height=19><font size=2>" + Convert.ToDateTime(Enq.ENQ_MaterialSendingOnDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                    "</tr>";
                }
                mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> Note  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + Enq.ENQ_Note_var + "</font></td>" +
               "</tr>";
                mySql += "<tr>" +
                "<td width='15%' align=left valign=top height=19><font size=2><b> Reference  </b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td width='40%' height=19><font size=2>" + Enq.ENQ_Reference_var + "</font></td>" +
                "</tr>";
            }
            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("Enquiry_" + EnqId, mySql);
        }

        #region HTMLReportEntry
        //public string getDetailReportAggtTesting()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";
        //    String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    string AggregateType = string.Empty;
        //    string AggregateName = string.Empty;
        //    string WitnessBy = string.Empty;
        //    var AggtTest = dc.ReportStatus_View("Aggregate Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 2, 0);
        //    foreach (var aggt in AggtTest)
        //    {
        //        AggregateName = aggt.AGGTINWD_AggregateName_var.ToString();
        //        if (Convert.ToString(aggt.AGGTINWD_AggregateName_var) == "10 mm" || Convert.ToString(aggt.AGGTINWD_AggregateName_var) == "20 mm" || Convert.ToString(aggt.AGGTINWD_AggregateName_var) == "40 mm")
        //        {
        //            AggregateType = "Coarse Aggregate";
        //        }
        //        else
        //        {
        //            AggregateType = "Fine Aggregate";
        //        }
        //        WitnessBy = aggt.AGGTINWD_WitnessBy_var.ToString();
        //        mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //        mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>" + AggregateType + "   " + "(" + Convert.ToString(aggt.AGGTINWD_AggregateName_var) + ")" + " </b></font></td></tr>";

        //        mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //        mySql += "<tr>" +
        //            "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //             "<td width='40%' height=19><font size=2>" + aggt.CL_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "-" + "</font></td>" +
        //            "<td height=19><font size=2>&nbsp;</font></td>" +
        //            "</tr>";


        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font></td>" +
        //            "<td width='40%' height=19><font size=2>" + aggt.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "AGGT" + "-" + aggt.AGGTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //          "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //          "<td width='2%' height=19><font size=2></font></td>" +
        //          "<td width='10%' height=19><font size=2></font></td>" +
        //          "<td height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
        //          "<td height=19><font size=2>:</font></td>" +
        //          "<td height=19><font size=2>" + "AGGT" + "-" + aggt.AGGTINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //          "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + aggt.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "DT" + "-" + " " + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +

        //             "<td width='20%' height=19><font size=2>  </font></td>" +
        //             "<td width='2%' height=19><font size=2> </font></td>" +
        //             "<td width='10%' height=19><font size=2></font></td>" +
        //             "<td height=19><font size=2><b>Date of receipt </b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + Convert.ToDateTime(aggt.INWD_ReceivedDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
        //            "</tr>";
        //        break;
        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";
        //    mySql += "</table>";

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";
        //    bool SpecGrav = false;
        //    bool lbd = false;
        //    bool Moist = false;
        //    bool Sild = false;
        //    bool Sieve = false;
        //    bool Impact = false;
        //    bool Elong = false;
        //    bool Crush = false;
        //    bool Flaki = false;
        //    var aggtTestname = dc.AllInwdDetails_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "AGGT");
        //    foreach (var aggtt in aggtTestname)
        //    {
        //        if (aggtt.TEST_Sr_No == 1)
        //        {
        //            Sieve = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 3)
        //        {
        //            SpecGrav = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 4)
        //        {
        //            lbd = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 9)
        //        {
        //            Moist = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 2)
        //        {
        //            Sild = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 7)
        //        {
        //            Impact = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 6)
        //        {
        //            Elong = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 8)
        //        {
        //            Crush = true;
        //        }
        //        if (aggtt.TEST_Sr_No == 5)
        //        {
        //            Flaki = true;
        //        }
        //    }
        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
        //    mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
        //    mySql += "<td width= 2% align=left  valign=top height=19 rowspan=5  ><font size=2> " + " " + " </font></td>";
        //    mySql += "<td width= 3% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test" + "</b></font></td>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "Result " + "</b></font></td>";
        //    mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>" + "Unit" + "</b></font></td>";
        //    mySql += "</tr>";

        //    var Inward_aggt = dc.ReportStatus_View("Aggregate Testing", null, null, 1, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 0, 0);
        //    foreach (var aggt in Inward_aggt)
        //    {
        //        if (aggt.AGGTINWD_AggregateName_var == "Natural Sand" || aggt.AGGTINWD_AggregateName_var == "Crushed Sand" || aggt.AGGTINWD_AggregateName_var == "Stone Dust" || aggt.AGGTINWD_AggregateName_var == "Grit")
        //        {
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Specific Gravity" + "</font></td>";
        //            if (SpecGrav == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SpecificGravity_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Moisture Content" + " </font></td>";
        //            if (Moist == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_MoistureContent_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " % " + "</font></td>";
        //            mySql += "</tr>";

        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Water Absorption" + "</font></td>";
        //            if (SpecGrav == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_WaterAborp_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "% " + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Condition of the Sample" + "</font></td>";

        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SampleCondition_var) + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "  " + "</b></font></td>";
        //            mySql += "</tr>";
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=middle height=19 ><font size=2>&nbsp;" + "Loose Bulk Density" + "</font></td>";
        //            if (lbd == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_LBD_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "kg/lit " + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Material finer than 75 u </br> (by wet sieving)" + "</font></td>";
        //            if (Sild == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SildContent_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " %  " + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //        if (aggt.AGGTINWD_AggregateName_var == "10 mm" || aggt.AGGTINWD_AggregateName_var == "20 mm" || aggt.AGGTINWD_AggregateName_var == "40 mm" || aggt.AGGTINWD_AggregateName_var == "Mix Aggt")
        //        {
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Specific Gravity" + "</font></td>";
        //            if (SpecGrav == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SpecificGravity_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Impact Value" + " </font></td>";
        //            if (Impact == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_ImpactValue_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "  " + "</font></td>";
        //            mySql += "</tr>";

        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Water Absorption" + "</font></td>";
        //            if (SpecGrav == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_WaterAborp_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "% " + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Condition of the Sample" + "</font></td>";

        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_SampleCondition_var) + "</font></td>";

        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "  " + "</b></font></td>";
        //            mySql += "</tr>";
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=middle height=19 ><font size=2>&nbsp;" + "Loose Bulk Density" + "</font></td>";
        //            if (lbd == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_LBD_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "kg/lit " + "</font></td>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Flakiness Value" + "</font></td>";
        //            if (Flaki == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_Flakiness_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + " " + "</font></td>";
        //            mySql += "</tr>";
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Elongness Value" + "</font></td>";
        //            if (Elong == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_Elongation_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " " + "</font></td>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Crushing Value" + "</font></td>";
        //            if (Crush == true)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + Convert.ToString(aggt.AGGTINWD_CrushingValue_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "Not Requested" + "</font></td>";
        //            }
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>" + "  " + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    mySql += "</table>";


        //    if (Sieve == true)
        //    {
        //        mySql += "<table>";
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "Sieve Analysis (by dry sieving) " + "</b></font></td>";
        //        mySql += "<td width= 3% align=left valign=top height=19 ><font size=2><b>" + AggregateName + "</b></font></td>";
        //        mySql += "</tr>";
        //        mySql += "</table>";
        //        mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

        //        int i = 0;
        //        var aggtTest = dc.AggregateAllTestView(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["MatID"]), "AGGTSA");
        //        foreach (var aggtt in aggtTest)
        //        {
        //            if (i == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2%  rowspan=2 align=center valign=middle height=19 ><font size=2><b>Sieve Size</b></font></td>";
        //                mySql += "<td width= 10%  align=center colspan=3 valign=top height=19 ><font size=2><b>Weight retained</b></font></td>";
        //                mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2><b>Passing </b></font></td>";
        //                if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
        //                {
        //                    mySql += "<td width= 2%   align=center rowspan=2 valign=middle height=19 ><font size=2><b>IS Passing % Limits </b></font></td>";
        //                }
        //                mySql += "</tr>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(g)</b></font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(%)</b></font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>Cummu (%)</b></font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2><b>(%)</b></font></td>";
        //            }
        //            mySql += "<tr>";
        //            mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_SeiveSize_var.ToString() + "</font></td>";
        //            mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_Weight_num.ToString() + "</font></td>";
        //            if (aggtt.AGGTSA_SeiveSize_var != "Total")
        //            {
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + Convert.ToDecimal(aggtt.AGGTSA_WeightRet_dec).ToString("0.00") + "</font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_CumuWeightRet_dec.ToString() + "</font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTSA_CumuPassing_dec.ToString() + "</font></td>";
        //                if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
        //                {
        //                    mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2> " + aggtt.AGGTSA_IsPassingLmt_var.ToString() + " </font></td>";
        //                }
        //            }
        //            else if (aggtt.AGGTSA_SeiveSize_var == "Total")
        //            {
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + "" + "</font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + "Fineness Modulus" + "</font></td>";
        //                mySql += "<td width= 2%  align=center valign=top height=19 ><font size=2>" + aggtt.AGGTINWD_FM_var.ToString() + "</font></td>";
        //                if (aggtt.AGGTSA_IsPassingLmt_var != string.Empty && aggtt.AGGTSA_IsPassingLmt_var != null)
        //                {
        //                    mySql += "<td width= 2%   align=center valign=top height=19 ><font size=2> " + "" + " </font></td>";
        //                }
        //            }
        //            i++;
        //        }
        //        mySql += "</tr>";
        //        mySql += "</table>";
        //    }
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    int SrNo = 0;
        //    var matid = dc.Material_View("AGGT", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, "AGGT");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.AGGTDetail_RemarkId_int), "AGGT");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.AGGT_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.ReportStatus_View("Aggregate Testing", null, null, 1, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (Convert.ToString(r.AGGTINWD_ApprovedBy_tint) != string.Empty)
        //            {
        //                var U = dc.User_View(r.AGGTINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //            mySql += "<tr>";
        //            if (WitnessBy != string.Empty)
        //            {
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> Witness by : " + WitnessBy + " </font></td>";
        //            }
        //            if (Convert.ToString(r.AGGTINWD_CheckedBy_tint) != string.Empty)
        //            {
        //                var lgin = dc.User_View(r.AGGTINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                }
        //                mySql += "</tr>";

        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999CRC014212" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";

        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        //public string getDetailReportCoreTesting()
        //{

        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";
        //    String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Concrete Core Compressive Strength </b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    int PrintPulse = 0;
        //    var Core = dc.ReportStatus_View("Core Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 2, 0);
        //    foreach (var CoreTest in Core)
        //    {
        //        mySql += "<tr>" +
        //        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td width='40%' height=19><font size=2>" + CoreTest.CL_Name_var + "</font></td>" +
        //        "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td height=19><font size=2>" + "-" + "</font></td>" +
        //        "<td height=19><font size=2>&nbsp;</font></td>" +
        //        "</tr>";


        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font></td>" +
        //            "<td width='40%' height=19><font size=2>" + CoreTest.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "CR" + "-" + CoreTest.CRINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //         "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //         "<td width='2%' height=19><font size=2></font></td>" +
        //         "<td width='10%' height=19><font size=2></font></td>" +
        //         "<td height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //         "<td height=19><font size=2>:</font></td>" +
        //         "<td height=19><font size=2>" + "CR" + "-" + CoreTest.CRINWD_ReferenceNo_var + "</font></td>" +
        //         "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + CoreTest.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Sample Ref No. </b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "CR" + "-" + "" + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //           "<td align=left valign=top height=19><font size=2><b> Concrete Member </b></font></td>" +
        //           "<td  width='2%' height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + CoreTest.CRINWD_ConcreteMember_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "CR" + "-" + " " + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +
        //            "<td align=left valign=top height=19><font size=2><b> Grade Of Concrete</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + CoreTest.CRINWD_Grade_var + "</font></td>" +
        //            "<td align=left valign=top height=19><font size=2><b>Specimen extraction Date </b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(CoreTest.CRINWD_SpecimenExtDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Curring Conditions</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + CoreTest.CRINWD_CurrCondition_var + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(CoreTest.CRINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
        //              "</tr>";
        //        if (Convert.ToString(CoreTest.CRINWD_PulseVelocity_bit) != null)
        //        {
        //            PrintPulse = Convert.ToInt32(CoreTest.CRINWD_PulseVelocity_bit);
        //        }
        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% rowspan=1 align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Diameter </br> </br> </br> (mm) </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Date of Casting </b></font></td>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Age of Concrete </br> </br> (Days) </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Area of Cross Section  (mm <sup>2</sup>)</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Weight before capping (kg)</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Density of concrete </br> </br> (kg/m <sup>3</sup>)</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Load at failure </br> </br> (kN)</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Comp. Strength </br> </br> (N/mm<sup>2</sup>)</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Corrected Comp. Strength (N/mm<sup>2</sup>)</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Equivalent cube Comp. Strength (N/mm<sup>2</sup>)</b></font></td>";
        //    mySql += "</tr>";

        //    int SrNo = 0;
        //    var CoreTesting = dc.TestDetail_Title_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, "CR", false);
        //    foreach (var core in CoreTesting)
        //    {
        //        SrNo++;
        //        if (Convert.ToString(core.Description_var) != "")
        //        {
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Description_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Dia_int) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Castingdate_var) + "</font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Age_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.CsArea_dec) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Weight_dec) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Density_dec) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Reading_dec) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.CompStr_dec) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.CorrCompStr_dec) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.EquCubeStr_dec) + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //        else
        //        {
        //            if (Convert.ToString(core.TitleId_int) != "")
        //            {
        //                if (Convert.ToInt32(core.TitleId_int) > 0)
        //                {
        //                    var crr = dc.TestDetail_Title_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToInt32(core.TitleId_int), "CR", false);
        //                    foreach (var title in crr)
        //                    {
        //                        mySql += "<tr>";
        //                        mySql += "<td width= 10% colspan=12 align=center valign=top height=19 ><font size=2> <b>&nbsp;" + Convert.ToString(title.TitleDesc_var) + "</b></font></td>";
        //                        mySql += "</tr>";
        //                        SrNo--;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    mySql += "</table>";
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "GENERAL INFORMATION & MODE OF FAILURE: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=80% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2%  rowspan=2 align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 10%  rowspan=2 align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";



        //    mySql += "<td width= 5% align=center colspan=2 valign=top height=19 ><font size=2><b>Correction Factor </b></font></td>";
        //    mySql += "<td width= 5% align=center colspan=2 valign=top height=19 ><font size=2><b>Core Length (mm) </b></font></td>";

        //    if (PrintPulse == 1)
        //    {
        //        mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Pulse  Velcocity</b></font></td>";
        //    }
        //    mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Mode of Failure</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "<td width= 5%  align=center valign=top height=19 ><font size=2><b>L/D </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Diameter </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Original </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>with cap</b></font></td>";

        //    SrNo = 0;
        //    decimal Diameter = 0;
        //    decimal Lenforcore = 0;
        //    decimal Multifactor = 0;

        //    var CrTesting = dc.TestDetail_Title_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, "CR", false);
        //    foreach (var core in CrTesting)
        //    {
        //        SrNo++;
        //        if (Convert.ToString(core.Description_var) != "")
        //        {
        //            if (core.Dia_int < 100)
        //            {
        //                Diameter = Convert.ToDecimal(1.08);
        //            }
        //            else if (core.Dia_int >= 100)
        //            {
        //                Diameter = Convert.ToDecimal(1);
        //            }
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";

        //            if (Convert.ToString(core.Dia_int) != "" && Convert.ToString(core.LengthCaping_num) != "")
        //            {
        //                Lenforcore = (Convert.ToDecimal(core.Dia_int) / Convert.ToDecimal(core.LengthCaping_num));
        //                if (Convert.ToDecimal(core.Dia_int) > 0 && Convert.ToDecimal(core.LengthCaping_num) > 0)
        //                {
        //                    Multifactor = (Convert.ToDecimal(0.106) * Lenforcore) + Convert.ToDecimal(0.786);
        //                }
        //            }
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Description_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(Multifactor).ToString("0.000") + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Diameter) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.Length_num) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.LengthCaping_num) + "</font></td>";
        //            if (PrintPulse == 1)
        //            {
        //                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.PulseVelocity_dec) + "</font></td>";
        //            }
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(core.ModeOfFailure_var) + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //        else
        //        {
        //            if (Convert.ToString(core.TitleId_int) != "")
        //            {
        //                if (Convert.ToInt32(core.TitleId_int) > 0)
        //                {
        //                    var crr = dc.TestDetail_Title_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToInt32(core.TitleId_int), "CR", false);
        //                    foreach (var title in crr)
        //                    {
        //                        mySql += "<tr>";
        //                        mySql += "<td width= 5% colspan=12 align=center valign=top height=19 ><font size=2><b>&nbsp;" + Convert.ToString(title.TitleDesc_var) + "</b></font></td>";
        //                        mySql += "</tr>";
        //                        SrNo--;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    SrNo = 0;
        //    var matid = dc.Material_View("CR", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }

        //    }

        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, "CR");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CRDetail_RemarkId_int), "CR");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.CR_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.ReportStatus_View("Core Testing", null, null, 1, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (Convert.ToString(r.CRINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.CRINWD_CheckedBy_tint) != string.Empty)
        //            {
        //                var U = dc.User_View(r.CRINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.CRINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999CRC014212" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        //public string getDetailReportNDTTesting()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";
        //    String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Non Destructive Testing of R.C.C. </b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


        //    string NdtBy_type = string.Empty;
        //    var NDT_Test = dc.ReportStatus_View("Non Destructive Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 2, 0);
        //    foreach (var NDTtest in NDT_Test)
        //    {
        //        mySql += "<tr>" +
        //        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td width='40%' height=19><font size=2>" + NDTtest.CL_Name_var + "</font></td>" +
        //        "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td height=19><font size=2>" + "-" + "</font></td>" +
        //        "<td height=19><font size=2>&nbsp;</font></td>" +
        //        "</tr>";


        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font></td>" +
        //            "<td width='40%' height=19><font size=2>" + NDTtest.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "NDT" + "-" + NDTtest.NDTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + NDTtest.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "NDT" + "-" + " " + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +
        //            "<td align=left valign=top height=19><font size=2><b> Kind Attention </b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + NDTtest.NDTINWD_KindAttention_var + "</font></td>" +
        //            "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + Convert.ToDateTime(NDTtest.NDTINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
        //           "</tr>";
        //        NdtBy_type = NDTtest.NDTINWD_NDTBy_var.ToString();
        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    if (NdtBy_type.Trim() == "UPV" || NdtBy_type.Trim() == "UPV with Grading")
        //    {
        //        mySql += "<td width= 2%  rowspan=1 align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
        //        mySql += "<td width= 10% rowspan=1 align=center valign=top height=19 ><font size=2><b>Location & </br> Identification</b></font></td>";
        //        mySql += "<td width= 5%  rowspan=1 align=center valign=top height=19 ><font size=2><b>Grade of </br> Concrete </b></font></td>";
        //        mySql += "<td width= 5% rowspan=1 align=center valign=top height=19 ><font size=2><b>Date of  </br> Casting </b></font></td>";
        //        mySql += "<td width= 2% rowspan=1 align=center valign=top height=19 ><font size=2><b>Age </br>  </br> (Days)</b></font></td>";

        //        if (NdtBy_type.Trim() != "Rebound Hammer")
        //        {
        //            mySql += "<td width= 5% rowspan=1 align=center valign=top height=19 ><font size=2><b>Pulse  </br> Velocity </br>(km/s)</b></font></td>";
        //        }
        //        if (NdtBy_type.Trim() == "UPV with Grading")
        //        {
        //            mySql += "<td width= 5% rowspan=1 align=center valign=top height=19 ><font size=2><b>Concrete </br> quality </br> grading</b></font></td>";
        //        }
        //        else
        //        {
        //            mySql += "<td width= 5% rowspan=1 align=center valign=top height=19 ><font size=2><b>Indicative </br> Strength </br> (N/mm<sup>2</sup>)</b></font></td>";
        //        }
        //        mySql += "</tr>";
        //    }
        //    else
        //    {
        //        mySql += "<td width= 2% rowspan=2 align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
        //        mySql += "<td width= 15% rowspan=2 align=center valign=top height=19 ><font size=2><b>Location & </br> Identification</b></font></td>";
        //        mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Grade of </br> Concrete </b></font></td>";
        //        mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Date of  </br> Casting </b></font></td>";
        //        mySql += "<td width= 2% rowspan=2 align=center valign=top height=19 ><font size=2><b>Age </br> </br> </br> (Days)</b></font></td>";
        //        if (NdtBy_type.Trim() != "UPV with Grading" && NdtBy_type.Trim() != "UPV")
        //        {
        //            mySql += "<td width= 2% colspan=2  align=center valign=top height=19 ><font size=2><b>Mech. Sclerometer </br> (Rebound Hammer) </b></font></td>";
        //        }
        //        if (NdtBy_type.Trim() != "Rebound Hammer")
        //        {
        //            mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Pulse  </br> Velocity  </br> </br>(km/s)</b></font></td>";
        //        }
        //        if (NdtBy_type.Trim() == "UPV with Grading")
        //        {
        //            mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Concrete </br> quality </br> grading</b></font></td>";
        //        }
        //        else
        //        {
        //            mySql += "<td width= 5% rowspan=2 align=center valign=top height=19 ><font size=2><b>Indicative  </br> Strength </br> </br> (N/mm<sup>2</sup>)</b></font></td>";
        //        }
        //        mySql += "</tr>";
        //        if (NdtBy_type.Trim() != "UPV with Grading" && NdtBy_type.Trim() != "UPV")
        //        {
        //            mySql += "<td width= 5%  align=center valign=top height=19 ><font size=2><b>Angle of inclination</b></font></td>";
        //            mySql += "<td width= 5%  align=center valign=top height=19 ><font size=2><b>Avearge Reading</b></font></td>";
        //        }
        //    }
        //    int SrNo = 0;
        //    var NDT_Testing = dc.TestDetail_Title_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, "NDT", false);
        //    foreach (var Ndt in NDT_Testing)
        //    {
        //        SrNo++;
        //        if (Convert.ToString(Ndt.Description_var) != "")
        //        {
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.Description_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.Grade_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.Castingdate_var) + "</font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.Age_var) + "</font></td>";
        //            if (NdtBy_type.Trim() != "UPV with Grading" && NdtBy_type.Trim() != "UPV")
        //            {
        //                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.AlphaAngle_var).Replace("°", "") + "<sup>0</sup> </font></td>";
        //            }
        //            if (Convert.ToString(Ndt.ReboundIndex_var) != "")
        //            {
        //                string[] Rebound = Convert.ToString(Ndt.ReboundIndex_var).Split('|');
        //                foreach (var RebdIndex in Rebound)
        //                {
        //                    if (RebdIndex != "")
        //                    {
        //                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + RebdIndex + "</font></td>";
        //                        break;
        //                    }
        //                }
        //            }
        //            if (NdtBy_type.Trim() != "Rebound Hammer")
        //            {
        //                if (Convert.ToString(Ndt.PulseVelocity_var) != "")
        //                {
        //                    string[] PulseVelc = Convert.ToString(Ndt.PulseVelocity_var).Split('|');
        //                    foreach (var Pulse in PulseVelc)
        //                    {
        //                        if (Pulse != "")
        //                        {
        //                            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Pulse + "</font></td>";
        //                            break;
        //                        }
        //                    }
        //                }
        //            }

        //            if (Ndt.EditedIndStr_var != null && Ndt.EditedIndStr_var != string.Empty)
        //            {
        //                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.EditedIndStr_var) + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(Ndt.IndicativeStrength_var) + "</font></td>";
        //            }
        //            mySql += "</tr>";
        //        }
        //        else
        //        {
        //            if (Convert.ToString(Ndt.TitleId_int) != "")
        //            {
        //                if (Convert.ToInt32(Ndt.TitleId_int) > 0)
        //                {
        //                    var crr = dc.TestDetail_Title_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToInt32(Ndt.TitleId_int), "NDT", false);
        //                    foreach (var title in crr)
        //                    {
        //                        mySql += "<tr>";
        //                        mySql += "<td width= 10% colspan=12 align=center valign=top height=19 ><font size=2> <b>&nbsp;" + Convert.ToString(title.TitleDesc_var) + "</b></font></td>";
        //                        mySql += "</tr>";
        //                        SrNo--;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    SrNo = 0;
        //    var matid = dc.Material_View("NDT", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }

        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, "NDT");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.NDTDetail_RemarkId_int), "NDT");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Uncertainty levels :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.NDT_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.ReportStatus_View("Non Destructive Testing", null, null, 1, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (Convert.ToString(r.NDTINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.NDTINWD_CheckedBy_tint) != string.Empty)
        //            {
        //                var U = dc.User_View(r.NDTINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.NDTINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999CRC014212" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        //public string getDetailReportPT_TS()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";
        //    String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Precast Concrete Blocks for Paving - Splitting Tensile Strength </b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


        //    var pt = dc.ReportStatus_View("Pavement Block Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 2, 0);

        //    foreach (var pavmt in pt)
        //    {
        //        mySql += "<tr>" +
        //        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td width='40%' height=19><font size=2>" + pavmt.CL_Name_var + "</font></td>" +
        //        "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td height=19><font size=2>" + "-" + "</font></td>" +
        //        "<td height=19><font size=2>&nbsp;</font></td>" +
        //        "</tr>";


        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font></td>" +
        //            "<td width='40%' height=19><font size=2>" + pavmt.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //         "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //         "<td width='2%' height=19><font size=2></font></td>" +
        //         "<td width='10%' height=19><font size=2></font></td>" +
        //         "<td height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //         "<td height=19><font size=2>:</font></td>" +
        //         "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_ReferenceNo_var + "</font></td>" +
        //         "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + pavmt.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "PT" + "-" + " " + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //           "<td align=left valign=top height=19><font size=2><b>Nature Of Work</b></font></td>" +
        //           "<td  width='2%' height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + pavmt.PTINWD_WorkingNature_var + "</font></td>" +
        //           "<td align=left valign=top height=19><font size=2><b> Date of Casting</b></font></td>" +
        //           "<td height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + pavmt.PTINWD_CastingDate_dt + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +
        //            "<td align=left valign=top height=19><font size=2><b> Grade Of Concrete</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + pavmt.PTINWD_Grade_var + "</font></td>" +
        //            "<td align=left valign=top height=19><font size=2><b>Date Of Receipt </b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.INWD_ReceivedDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + pavmt.PTINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.PTINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
        //              "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table cell=9>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<th width= 2%  rowspan=2 align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></th>";
        //    mySql += "<th width= 10% rowspan=2  align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></th>";
        //    mySql += "<th width= 5%  rowspan=2   align=center valign=top height=19 ><font size=2><b>Age </br></br> </br> (Days)</b></font></th>";
        //    mySql += "<th width= 10% rowspan=2  align=center valign=top height=19 ><font size=2><b>Thickness </br></br></br> (mm) </b></font></th>";
        //    mySql += "<th width= 10% rowspan=2  align=center valign=top height=19 ><font size=2><b>Failure Load </br></br></br> (N) </b></font></th>";
        //    mySql += "<th width= 10% align=center  colspan=2  valign=top height=19 ><font size=2><b>Mean Failure </br></br> </b></font></th>";
        //    mySql += "<th width= 10% align=center valign=top  rowspan=2  height=19 ><font size=2><b>Failure Load per Unit Length </br> (N/mm)</b></font></th>";
        //    mySql += "<th width= 10% align=center valign=top rowspan=2  height=19 ><font size=2><b>Splitting Tensile Strength (N/mm <sup>2</sup>)</b></font></th>";
        //    mySql += "<th width= 10% align=center valign=top rowspan=2  height=19 ><font size=2><b>Average </br></br> </br>(N/mm <sup>2</sup>)</b></font></t>";
        //    mySql += "</tr>";
        //    mySql += "<th width= 10%   align=center valign=top height=19 ><font size=2><b>Length </br> (mm)</b></font></th>";
        //    mySql += "<th width= 10%   align=center valign=top height=19 ><font size=2><b>Thickness </br> (mm)</b></font></th>";

        //    int SrNo = 0;
        //    int i = 0;
        //    var PT_TS = dc.Pavement_Test_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]), "TS");
        //    var count = PT_TS.Count();
        //    var PT_TensileStr = dc.Pavement_Test_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]), "TS");
        //    foreach (var ptts in PT_TensileStr)
        //    {
        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.IdMark_var) + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.Age_var) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.Thickness_var).Replace("mm", "") + "</font></td>";
        //        mySql += "<td width= 10%  align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.FailureLoad_num) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.FailureLength_num) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.FailureThickness_int) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.FailureLoadPerUnitLen_num) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.TensileStrength_dec) + "</font></td>";

        //        if (i == 0)
        //        {
        //            mySql += "<td  BorderStyle=none width= 10%  align=center rowspan=" + count + " valign=middle height=19 ><font size=2>&nbsp;" + Convert.ToString(ptts.PTINWD_AvgStr_var) + "</font></td>";
        //        }
        //        i++;
        //        mySql += "</tr>";
        //    }

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    SrNo = 0;
        //    var matid = dc.Material_View("PT", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }

        //    }


        //    SrNo = 0;
        //    var re = dc.Pavement_Test_Remark_View("", Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]));
        //    foreach (var r in re)
        //    {
        //        var remark = dc.Pavement_Test_Remark_View("", "", Convert.ToInt32(r.RemarkId_int), Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]));
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.ReportStatus_View("Pavement Block Testing", null, null, 1, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (Convert.ToString(r.PTINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.PTINWD_CheckedBy_tint) != string.Empty)
        //            {
        //                var U = dc.User_View(r.PTINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.PTINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999PTC014212" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";

        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        //public string getDetailReportPT_FS()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";
        //    String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Precast Concrete Blocks for Paving - Flexural Strength </b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


        //    var pt = dc.ReportStatus_View("Pavement Block Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 2, 0);

        //    foreach (var pavmt in pt)
        //    {
        //        mySql += "<tr>" +
        //        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td width='40%' height=19><font size=2>" + pavmt.CL_Name_var + "</font></td>" +
        //        "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td height=19><font size=2>" + "-" + "</font></td>" +
        //        "<td height=19><font size=2>&nbsp;</font></td>" +
        //        "</tr>";


        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font></td>" +
        //            "<td width='40%' height=19><font size=2>" + pavmt.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //         "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //         "<td width='2%' height=19><font size=2></font></td>" +
        //         "<td width='10%' height=19><font size=2></font></td>" +
        //         "<td height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //         "<td height=19><font size=2>:</font></td>" +
        //         "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_ReferenceNo_var + "</font></td>" +
        //         "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + pavmt.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "PT" + "-" + " " + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //           "<td align=left valign=top height=19><font size=2><b>Nature Of Work</b></font></td>" +
        //           "<td  width='2%' height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + pavmt.PTINWD_WorkingNature_var + "</font></td>" +
        //           "<td align=left valign=top height=19><font size=2><b> Date of Casting</b></font></td>" +
        //           "<td height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + pavmt.PTINWD_CastingDate_dt + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +
        //            "<td align=left valign=top height=19><font size=2><b> Grade Of Concrete</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + pavmt.PTINWD_Grade_var + "</font></td>" +
        //            "<td align=left valign=top height=19><font size=2><b>Date Of Receipt </b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.INWD_ReceivedDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + pavmt.PTINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.PTINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
        //              "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Age </br> (Days)</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Thickness </br> (mm) </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Breaking Load </br> (kN) </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Flexural Strength </br> (N/mm <sup>2</sup>)</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Average </br> (N/mm <sup>2</sup>)</b></font></td>";
        //    mySql += "</tr>";

        //    int SrNo = 0;
        //    int i = 0;
        //    var PT_FS = dc.Pavement_Test_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]), "FS");
        //    var count = PT_FS.Count();
        //    var PT_flexualStrength = dc.Pavement_Test_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]), "FS");
        //    foreach (var ptcs in PT_flexualStrength)
        //    {
        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.IdMark_var) + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.Age_var) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.Thickness_var).Replace("mm", "") + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.BreakingLoad_dec) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.FlexuralStrength_dec) + "</font></td>";
        //        if (i == 0)
        //        {
        //            mySql += "<td  BorderStyle=none width= 10%  align=center rowspan=" + count + " valign=middle height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.PTINWD_AvgStr_var) + "</font></td>";
        //        }
        //        i++;
        //        mySql += "</tr>";
        //    }

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    SrNo = 0;
        //    var matid = dc.Material_View("PT", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }

        //    }

        //    SrNo = 0;
        //    var re = dc.Pavement_Test_Remark_View("", Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]));
        //    foreach (var r in re)
        //    {
        //        var remark = dc.Pavement_Test_Remark_View("", "", Convert.ToInt32(r.RemarkId_int), Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]));
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.ReportStatus_View("Pavement Block Testing", null, null, 1, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (Convert.ToString(r.PTINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.PTINWD_CheckedBy_tint) != string.Empty)
        //            {
        //                var U = dc.User_View(r.PTINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.PTINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999PTC014212" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        //public string getDetailReportPT_CS()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";
        //    String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Precast Concrete Blocks for Paving - Compressive Strength </b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


        //    var pt = dc.ReportStatus_View("Pavement Block Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 2, 0);

        //    foreach (var pavmt in pt)
        //    {
        //        mySql += "<tr>" +
        //        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td width='40%' height=19><font size=2>" + pavmt.CL_Name_var + "</font></td>" +
        //        "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td height=19><font size=2>" + "-" + "</font></td>" +
        //        "<td height=19><font size=2>&nbsp;</font></td>" +
        //        "</tr>";


        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font></td>" +
        //            "<td width='40%' height=19><font size=2>" + pavmt.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //         "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //         "<td width='2%' height=19><font size=2></font></td>" +
        //         "<td width='10%' height=19><font size=2></font></td>" +
        //         "<td height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //         "<td height=19><font size=2>:</font></td>" +
        //         "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_ReferenceNo_var + "</font></td>" +
        //         "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + pavmt.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "PT" + "-" + " " + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //           "<td align=left valign=top height=19><font size=2><b>Nature Of Work</b></font></td>" +
        //           "<td  width='2%' height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + pavmt.PTINWD_WorkingNature_var + "</font></td>" +
        //           "<td align=left valign=top height=19><font size=2><b> Date of Casting</b></font></td>" +
        //           "<td height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + pavmt.PTINWD_CastingDate_dt + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +
        //            "<td align=left valign=top height=19><font size=2><b> Grade Of Concrete</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + pavmt.PTINWD_Grade_var + "</font></td>" +
        //            "<td align=left valign=top height=19><font size=2><b>Date Of Receipt </b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.INWD_ReceivedDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + pavmt.PTINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.PTINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
        //              "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Age </br></br> (Days)</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Plan Area </br></br> (mm<sup>2</sup>) </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Actual Thickness </br> (mm) </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Weight </br> </br>(kg)</b></font></td>";
        //    mySql += "<td width= 15% align=center valign=top height=19 ><font size=2><b>Density </br></br> (kg/m <sup>3</sup>)</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Load </br></br>  (kN)</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Comp. Strength </br> (N/mm <sup>2</sup> )</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Average </br></br> (N/mm <sup>2</sup>)</b></font></td>";
        //    mySql += "</tr>";

        //    int SrNo = 0;
        //    int i = 0;
        //    var PT_CS = dc.Pavement_Test_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]), "CS");
        //    var count = PT_CS.Count();
        //    var PT_CompressiveStr = dc.Pavement_Test_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]), "CS");
        //    foreach (var ptcs in PT_CompressiveStr)
        //    {
        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.IdMark_var) + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.Age_var) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.PlanArea_num) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.ActualThickness_int) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.Weight_dec) + "</font></td>";
        //        mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.Density_dec) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.Reading_var) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.CompStr_var) + "</font></td>";
        //        if (i == 0)
        //        {
        //            mySql += "<td  BorderStyle=none width= 10%  align=center rowspan=" + count + " valign=middle height=19 ><font size=2>&nbsp;" + Convert.ToString(ptcs.PTINWD_AvgStr_var) + "</font></td>";
        //        }
        //        i++;
        //        mySql += "</tr>";
        //    }

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    SrNo = 0;
        //    var matid = dc.Material_View("PT", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }

        //    }

        //    SrNo = 0;
        //    var re = dc.Pavement_Test_Remark_View("", Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]));
        //    foreach (var r in re)
        //    {
        //        var remark = dc.Pavement_Test_Remark_View("", "", Convert.ToInt32(r.RemarkId_int), Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]));
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.ReportStatus_View("Pavement Block Testing", null, null, 1, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (Convert.ToString(r.PTINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.PTINWD_CheckedBy_tint) != string.Empty)
        //            {
        //                var U = dc.User_View(r.PTINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.PTINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999PTC014212" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}

        //public string getDetailReportCT()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";
        //    String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Concrete Cube Compressive Strength</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


        //    var ct = dc.ReportStatus_View("Cube Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 2, 0);

        //    foreach (var cube in ct)
        //    {
        //        mySql += "<tr>" +
        //        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td width='40%' height=19><font size=2>" + cube.CL_Name_var + "</font></td>" +
        //        "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //        "<td height=19><font size=2>:</font></td>" +
        //        "<td height=19><font size=2>" + "-" + "</font></td>" +
        //        "<td height=19><font size=2>&nbsp;</font></td>" +
        //        "</tr>";


        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font></td>" +
        //            "<td width='40%' height=19><font size=2>" + cube.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "CT" + "-" + cube.CTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //         "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //         "<td width='2%' height=19><font size=2></font></td>" +
        //         "<td width='10%' height=19><font size=2></font></td>" +
        //         "<td height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //         "<td height=19><font size=2>:</font></td>" +
        //         "<td height=19><font size=2>" + "CT" + "-" + cube.CTINWD_ReferenceNo_var + "</font></td>" +
        //         "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + cube.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Coupon No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "CT" + "-" + cube.CTINWD_CouponNo_var.ToString() + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //           "<td align=left valign=top height=19><font size=2><b>Nature Of Work</b></font></td>" +
        //           "<td  width='2%' height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + cube.CTINWD_WorkingNature_var + "</font></td>" +
        //           "<td align=left valign=top height=19><font size=2><b> Date of Casting</b></font></td>" +
        //           "<td height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + cube.CTINWD_CastingDate_dt + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +
        //            "<td align=left valign=top height=19><font size=2><b> Grade Of Concrete</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "M " + "" + cube.CTINWD_Grade_int.ToString() + "</font></td>" +
        //            "<td align=left valign=top height=19><font size=2><b>Date Of Receipt </b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + DateTime.Now.ToString("dd/MM/yy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + cube.CTINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(cube.CTINWD_TestingDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //              "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";


        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Age</b></font></td>";
        //    mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Size Of specimen </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Weight</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>C/S Area</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Density</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Load</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Comp. Strength</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Avg. Comp Strength</b></font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2></font></td>";


        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>" + "(Days)" + "</b></font></td>";
        //    mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>" + "(mm )" + " </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>" + "(kg )" + "</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>" + "(mm <sup>2</sup>)" + "</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>" + "(kg/m <sup>3</sup>)" + "</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>" + "(kN)" + "</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>" + "(N/mm <sup>2</sup> )" + "</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>" + "(N/mm <sup>2</sup>)" + "</b></font></td>";
        //    mySql += "</tr>";

        //    int i = 0;
        //    int SrNo = 0;
        //    int Days = 0;
        //    if (System.Web.HttpContext.Current.Session["CubeCompStrength"] != null || System.Web.HttpContext.Current.Session["CementStrength"] != null)
        //    {

        //        string[] CompressiveTest = Convert.ToString(System.Web.HttpContext.Current.Session["ComressiveTest"]).Split('(', ')', ' ');
        //        foreach (var Comp in CompressiveTest)
        //        {
        //            if (Comp != "")
        //            {
        //                if (int.TryParse(Comp, out Days))
        //                {
        //                    Days = Convert.ToInt32(Comp.ToString());
        //                    break;
        //                }
        //            }
        //        }
        //        var cubeComp_CT = dc.OtherCubeTestView(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToString(System.Web.HttpContext.Current.Session["RecType"]), Convert.ToByte(Days), 0, Convert.ToString(System.Web.HttpContext.Current.Session["RecType"]), false, true);
        //        var count = cubeComp_CT.Count();
        //        var cubeCompstr = dc.OtherCubeTestView(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToString(System.Web.HttpContext.Current.Session["RecType"]), Convert.ToByte(Days), 0, Convert.ToString(System.Web.HttpContext.Current.Session["RecType"]), false, true);
        //        foreach (var cubecm in cubeCompstr)
        //        {
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(cubecm.IdMark_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(cubecm.Age_var) + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(cubecm.Length_var) + " " + "X" + " " + Convert.ToString(cubecm.Breadth_dec) + " " + "X" + " " + Convert.ToString(cubecm.Height_dec) + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(cubecm.Weight_dec) + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(cubecm.CSArea_dec) + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(cubecm.Density_dec) + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(cubecm.Reading_var) + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(cubecm.CompStr_var) + "</font></td>";

        //            if (i == 0)
        //            {
        //                mySql += "<td  BorderStyle=none width= 10%  align=center rowspan=" + count + " valign=middle height=19 ><font size=2>&nbsp;" + Convert.ToString(cubecm.Avg_var) + "</font></td>";
        //            }
        //            i++;
        //            mySql += "</tr>";
        //        }
        //    }
        //    else
        //    {
        //        var cube_CT = dc.CubeTestDetails_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "CT");
        //        var count = cube_CT.Count();
        //        var c = dc.CubeTestDetails_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "CT");
        //        foreach (var t in c)
        //        {
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + t.CTTEST_IdMark_var.ToString() + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + t.CTTEST_Age_var.ToString() + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + t.CTTEST_Length_dec + " " + "X" + " " + t.CTTEST_Breadth_dec + " " + "X" + " " + t.CTTEST_Height_dec + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + t.CTTEST_Weight_dec.ToString() + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + t.CTTEST_CSArea_dec.ToString() + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + t.CTTEST_Density_dec.ToString() + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + t.CTTEST_Reading_var.ToString() + "</font></td>";
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + t.CTTEST_CompStr_var.ToString() + "</font></td>";

        //            if (i == 0)
        //            {
        //                var ca = dc.ReportStatus_View("Cube Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 0, 0);
        //                foreach (var cavg in ca)
        //                {
        //                    mySql += "<td  BorderStyle=none width= 10%  align=center rowspan=" + count + " valign=middle height=19 ><font size=2>&nbsp;" + cavg.CTINWD_AvgStr_var.ToString() + "</font></td>";
        //                }
        //            }
        //            i++;
        //            mySql += "</tr>";
        //        }

        //    }
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2><b>" + " Compliance :" + " </b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "1)" + "The Test result complies with the requirements of IS 456-2000, subject to standard deviation less than 4" + "</b></font></td>";
        //    mySql += "</tr>";

        //    SrNo = 0;
        //    var matid = dc.Material_View("CT", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }

        //    }
        //    if (System.Web.HttpContext.Current.Session["CubeCompStrength"] != null || System.Web.HttpContext.Current.Session["CementStrength"] != null)
        //    {
        //        var re = dc.OtherCubeTestRemark_View("", Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, Convert.ToByte(Days), "CT");
        //        foreach (var rm in re)
        //        {
        //            var remark = dc.OtherCubeTestRemark_View("", "", Convert.ToInt32(rm.RemarkId_int), Convert.ToByte(Days), "CT");
        //            foreach (var rem in remark)
        //            {
        //                if (SrNo == 0)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                    mySql += "</tr>";
        //                }
        //                SrNo++;
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + rem.Remark_var + "</font></td>";
        //                mySql += "</tr>";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        SrNo = 0;
        //        var re = dc.AllRemark_View("", Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, "CT");
        //        foreach (var r in re)
        //        {
        //            var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CTDetail_RemarkId_int), "CT");
        //            foreach (var remk in remark)
        //            {
        //                if (SrNo == 0)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                    mySql += "</tr>";
        //                }
        //                SrNo++;
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.CT_Remark_var.ToString() + "</font></td>";
        //                mySql += "</tr>";
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.AllInwdDetails_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, Convert.ToInt32(System.Web.HttpContext.Current.Session["RecordNo"]), "", 0, "CT");
        //        foreach (var r in RecNo)
        //        {
        //            var Auth = dc.AllInwdDetails_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, r.CTINWD_ApprovedBy_tint, 0, 0, "", 0, "CT");
        //            foreach (var Approve in Auth)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Approve.USER_Name_var.ToString() + "</font></td>";
        //                mySql += "</tr>";
        //                mySql += "<tr>";
        //                mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + Approve.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                mySql += "</tr>";

        //            }
        //            var lgin = dc.AllInwdDetails_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, r.CTINWD_CheckedBy_tint, 0, "", 0, "CT");
        //            foreach (var loginusr in lgin)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                mySql += "</tr>";
        //            }
        //        }
        //    }

        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999PTC014212" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";
        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;
        //}
        //public string getDetailReportCCH()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";

        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Hydraulic Cement(Chemical)</b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    var water = dc.ReportStatus_View("Cement Chemical Testing", null, null, 0, 0, 0, System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, 2, 0);
        //    int j = 0;
        //    string Grade = "";
        //    foreach (var w in water)
        //    {
        //        if (j == 0)
        //        {
        //            Grade = Convert.ToString(w.CCHINWD_Grade_var);

        //            mySql += "<tr>" +
        //                  "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //                  "<td width='2%' height=19><font size=2>:</font></td>" +
        //                  "<td width='40%' height=19><font size=2>" + w.CL_Name_var + "</font></td>" +
        //                  "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //                  "<td height=19><font size=2>:</font></td>" +
        //                  "<td height=19><font size=2>" + "-" + "</font></td>" +
        //                  "<td height=19><font size=2>&nbsp;</font></td>" +
        //                  "</tr>";

        //            mySql += "<tr>" +

        //                 "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //                 "<td height=19><font size=2></font>:</td>" +
        //                 "<td width='40%' height=19><font size=2>" + w.CL_OfficeAddress_var + "</font></td>" +
        //                 "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //                 "<td height=19><font size=2>:</font></td>" +
        //                 "<td height=19><font size=2>" + "CCH" + "-" + w.CCHINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //                 "</tr>";

        //            mySql += "<tr>" +
        //                "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //                "<td width='2%' height=19><font size=2></font></td>" +
        //                "<td width='10%' height=19><font size=2></font></td>" +
        //                 "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //                 "<td height=19><font size=2>:</font></td>" +
        //                 "<td height=19><font size=2>" + "CCH" + "-" + w.CCHINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //                 "</tr>";

        //            mySql += "<tr>" +

        //                "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //                "<td width='2%' height=19><font size=2>:</font></td>" +
        //                "<td width='10%' height=19><font size=2>" + w.SITE_Name_var + "</font></td>" +
        //                "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //                "<td height=19><font size=2>:</font></td>" +
        //                "<td height=19><font size=2>" + "CCH" + "-" + "" + "</font></td>" +
        //                "</tr>";


        //            mySql += "<tr>" +

        //                "<td width='20%' height=19><font size=2><b>Cement Name</b></font></td>" +
        //                "<td width='2%' height=19><font size=2>:</font></td>" +
        //                "<td width='10%' height=19><font size=2>" + w.CCHINWD_CementName_var + "</font></td>" +
        //                "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
        //                "<td height=19><font size=2>:</font></td>" +
        //                "<td height=19><font size=2>" + Convert.ToDateTime(w.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //                "</tr>";



        //            mySql += "<tr>" +
        //                  "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //                  "<td width='2%' height=19><font size=2>:</font></td>" +
        //                  "<td height=19><font size=2>" + w.CCHINWD_Description_var.ToString() + "</font></td>" +
        //                  "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //                  "<td height=19><font size=2>:</font></td>" +
        //                  "<td height=19><font size=2>" + Convert.ToDateTime(w.CCHINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //                  "</tr>";

        //            mySql += "<tr>" +
        //                "<td align=left valign=top height=19><font size=2><b>Supplier Name</b></font></td>" +
        //                "<td width='2%' height=19><font size=2>:</font></td>" +
        //                "<td height=19><font size=2>" + w.CCHINWD_SupplierName_var.ToString() + "</font></td>" +
        //                "<td align=left valign=top height=19></td>" +
        //                "<td height=19></td>" +
        //                "<td height=19></td>" +
        //                "</tr>";
        //        }
        //        j++;
        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "RESULTS : " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";


        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";


        //    mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Name Of Test</b></font></td>";
        //    mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Result(%)</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Specified Limits (IS - 12269) </b></font></td>";
        //    mySql += "</tr>";


        //    int i = 0;
        //    int SrNo = 0;

        //    var details = dc.AllInwdDetails_View(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "CCH");
        //    foreach (var CCH in details)
        //    {

        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width=10% align=center valign=top height=19 ><font size=2>&nbsp;" + CCH.TEST_Name_var + "</font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + CCH.CCHTEST_Result_dec + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + CCH.splmt_SpecifiedLimit_var.ToString() + "</font></td>";
        //        mySql += "</tr>";
        //    }

        //    i++;

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    SrNo = 0;
        //    var matid = dc.Material_View("CCH", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, Grade, 0, "CCH");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + " )" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, "CCH");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CCHDetail_RemarkId_int), "CCH");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.CCH_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.ReportStatus_View("Cement Chemical Testing", null, null, 1, 0, 0, System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (r.CCHINWD_ApprovedBy_tint != null && r.CCHINWD_CheckedBy_tint != null)
        //            {
        //                var U = dc.User_View(r.CCHINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.CCHINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page of 1 of 1 " + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PNI999PTC014212" + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add - 1160/5,Gharpure Colony Shivaji Nagar, Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        //public string getDetailReportST()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";

        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Reinforcement Steel/Rebars </b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    var Steel = dc.ReportStatus_View("Steel Testing", null, null, 0, 0, 0, System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, 2, 0);
        //    foreach (var s in Steel)
        //    {
        //        mySql += "<tr>" +
        //              "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td width='40%' height=19><font size=2>" + s.CL_Name_var + "</font></td>" +
        //              "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + "-" + "</font></td>" +
        //              "<td height=19><font size=2>&nbsp;</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +

        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td width='2%' height=19><font size=2></font>:</td>" +
        //             "<td width='40%' height=19><font size=2>" + s.CL_OfficeAddress_var + "</font></td>" +
        //             "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td width='2%' height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "ST" + "-" + s.STINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //             "<td align=left valign=top height=19><font size=2></font></td>" +
        //             "<td width='2%'  height=19><font size=2></font></td>" +
        //             "<td width='40%' height=19><font size=2></font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //             "<td width='2%'  height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "ST" + "-" + s.STINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + s.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "ST" + "-" + "" + "</font></td>" +
        //            "</tr>";


        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Type Of Steel</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + s.STINWD_SteelType_var.ToString() + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(s.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //            "</tr>";


        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b> Grade Of Steel</b></font></td>" +
        //              "<td height=19><font size=2></font></td>" +
        //              "<td height=19><font size=2>" + "Fe" + " " + s.STINWD_Grade_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(s.STINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +

        //           "<td width='20%' height=19><font size=2><b>Description</b></font></td>" +
        //           "<td width='2%' height=19><font size=2>:</font></td>" +
        //           "<td width='10%' height=19><font size=2>" + s.STINWD_Description_var.ToString() + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Supplier Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + s.STINWD_SupplierName_var + "</font></td>" +
        //            "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

        //    int i = 0;
        //    int SrNo = 0;
        //    var SteelDetails = dc.SteelDetailInward_Update(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, "", false, true, false);
        //    var count = SteelDetails.Count();
        //    var details = dc.SteelDetailInward_Update(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, "", 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, "", false, true, false);
        //    foreach (var ST in details)
        //    {
        //        if (i == 0)
        //        {
        //            mySql += "<tr>";
        //            mySql += "<td width= 2%  align=center rowspan=2 valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 2%  align=center valign=medium height=19 ><font size=2><b>Dia. Of bar </b></font></td>";
        //            mySql += "<td width= 5%  align=center  rowspan=2 valign=medium height=19 ><font size=2><b>Id Mark</b></font></td>";
        //            if (ST.STDETAIL_CSArea_dec != null && ST.STDETAIL_CSArea_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>C/s Area </b></font></td>";
        //            }
        //            if (ST.STDETAIL_WtMeter_dec != null && ST.STDETAIL_WtMeter_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Wt/m </b></font></td>";
        //            }
        //            if (ST.STINWD_AvgWtMeter_dec != null && ST.STINWD_AvgWtMeter_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Avg Wt/m </b></font></td>";
        //            }
        //            if (ST.STDETAIL_Rebend_var != null && ST.STDETAIL_Rebend_var != "")
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Rebend Test</b></font></td>";
        //            }
        //            if (ST.STDETAIL_Bend_var != null && ST.STDETAIL_Bend_var != "")
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Bend Test</b></font></td>";
        //            }
        //            if (ST.STDETAIL_Elongation_dec != null && ST.STDETAIL_Elongation_dec != 0)
        //            {
        //                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Elongation</b></font></td>";
        //            }
        //            if (ST.STDETAIL_UltimateStress_dec != null && ST.STDETAIL_UltimateStress_dec != 0 && ST.STDETAIL_YieldStress_dec != null && ST.STDETAIL_YieldStress_dec != 0)
        //            {
        //                mySql += "<td colspan=2 width= 5%  align=center valign=medium height=19 ><font size=2><b>Tensile stress </br> (N/mm <sup>2</sup>) </b></font></td>";
        //            }
        //            mySql += "</tr>";
        //            mySql += "<td  align=center valign=medium height=19 ><font size=2><b> (mm) </b></font></td>";
        //            if (ST.STDETAIL_CSArea_dec != null && ST.STDETAIL_CSArea_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>(mm<sup>2</sup>)</b></font></td>";
        //            }
        //            if (ST.STDETAIL_WtMeter_dec != null && ST.STDETAIL_WtMeter_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>(kg) </b></font></td>";
        //            }
        //            if (ST.STINWD_AvgWtMeter_dec != null && ST.STINWD_AvgWtMeter_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>(kg)</b></font></td>";
        //            }
        //            if (ST.STDETAIL_Rebend_var != null && ST.STDETAIL_Rebend_var != "")
        //            {
        //                mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>(135<sup>0</sup>/157.5<sup>0</sup>)</b></font></td>";
        //            }
        //            if (ST.STDETAIL_Bend_var != null && ST.STDETAIL_Bend_var != "")
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>(180<sup>0</sup>) </b></font></td>";
        //            }
        //            if (ST.STDETAIL_Elongation_dec != null && ST.STDETAIL_Elongation_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>(%) </b></font></td>";
        //            }
        //            if (ST.STDETAIL_YieldStress_dec != null && ST.STDETAIL_YieldStress_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>0.2% Proof</b></font></td>";
        //            }
        //            if (ST.STDETAIL_UltimateStress_dec != null && ST.STDETAIL_UltimateStress_dec != 0)
        //            {
        //                mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Ultimate </b></font></td>";
        //            }
        //            mySql += "</tr>";
        //        }

        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + ST.STINWD_Diameter_tint + "</font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + ST.STDETAIL_IdMark_var + "</font></td>";
        //        if (ST.STDETAIL_CSArea_dec != null && ST.STDETAIL_CSArea_dec != 0)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_CSArea_dec + "</font></td>";
        //        }
        //        if (ST.STDETAIL_WtMeter_dec != null && ST.STDETAIL_WtMeter_dec != 0)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_WtMeter_dec + "</font></td>";
        //        }
        //        if (i == 0)
        //        {
        //            if (ST.STINWD_AvgWtMeter_dec != null && ST.STINWD_AvgWtMeter_dec != 0)
        //            {
        //                mySql += "<td width= 5% align=center valign=middle horizontalalign=center rowspan= " + count + " height=19 ><font size=2>" + ST.STINWD_AvgWtMeter_dec + "</font></td>";
        //            }
        //        }
        //        if (ST.STDETAIL_Rebend_var != null && ST.STDETAIL_Rebend_var != "")
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_Rebend_var + "</font></td>";
        //        }
        //        if (ST.STDETAIL_Bend_var != null && ST.STDETAIL_Bend_var != "")
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_Bend_var + "</font></td>";
        //        }
        //        if (ST.STDETAIL_Elongation_dec != null && ST.STDETAIL_Elongation_dec != 0)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_Elongation_dec + "</font></td>";
        //        }
        //        if (ST.STDETAIL_YieldStress_dec != null && ST.STDETAIL_YieldStress_dec != 0)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_YieldStress_dec + "</font></td>";
        //        }
        //        if (ST.STDETAIL_UltimateStress_dec != null && ST.STDETAIL_UltimateStress_dec != 0)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + ST.STDETAIL_UltimateStress_dec + "</font></td>";
        //        }
        //        mySql += "</tr>";
        //        i++;
        //    }
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    SrNo = 0;
        //    var matid = dc.Material_View("ST", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + " )" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }


        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, "ST");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.STDetail_RemarkId_int), "ST");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.ST_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";

        //        }
        //    }

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.ReportStatus_View("Steel Testing", null, null, 1, 0, 0, System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (r.STINWD_ApprovedBy_tint != null && r.STINWD_CheckedBy_tint != null)
        //            {
        //                var U = dc.User_View(r.STINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.STINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page of 1 of 1 " + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PNI999PTC014212" + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add - 1160/5,Gharpure Colony Shivaji Nagar, Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        //public string getDetailReportSTC()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";

        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Reinforcement Steel (Chemical)</b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    var water = dc.ReportStatus_View("Steel Chemical Testing", null, null, 0, 0, 0, System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, 2, 0);
        //    foreach (var w in water)
        //    {
        //        mySql += "<tr>" +
        //              "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td width='40%' height=19><font size=2>" + w.CL_Name_var + "</font></td>" +
        //              "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + "-" + "</font></td>" +
        //              "<td height=19><font size=2>&nbsp;</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +

        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td width='2%' height=19><font size=2></font>:</td>" +
        //             "<td width='40%' height=19><font size=2>" + w.CL_OfficeAddress_var + "</font></td>" +
        //             "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td width='2%' height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "STC" + "-" + w.STCINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //             "<td align=left valign=top height=19><font size=2></font></td>" +
        //             "<td width='2%'  height=19><font size=2></font></td>" +
        //             "<td width='40%' height=19><font size=2></font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //             "<td width='2%'  height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "STC" + "-" + w.STCINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + w.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "STC" + "-" + "" + "</font></td>" +
        //            "</tr>";


        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Type Of Steel</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + w.STCINWD_SteelType_var.ToString() + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(w.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //        "<td align=left valign=top height=19><font size=2><b> Grade Of Steel </b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td height=19><font size=2>" + Convert.ToString(w.STCINWD_Grade_var) + "</font></td>" +
        //        "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td height=19><font size=2>" + Convert.ToDateTime(w.STCINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //        "</tr>";

        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b> Sample Description</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + w.STCINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
        //              "<td width='2%' height=19><font size=2> </font></td>" +
        //              "<td height=19><font size=2>" + " " + "</font></td>" +
        //              "</tr>";


        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Supplier Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + w.STCINWD_SupplierName_var + "</font></td>" +
        //            "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "RESULTS & OBSERVATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";


        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";


        //    mySql += "<td width= 2%  align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 2%  align=center valign=medium height=19 ><font size=2><b>Id Mark</b></font></td>";
        //    mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Diameter </br>(mm)</b></font></td>";
        //    mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Carbon </br>(%) </b></font></td>";
        //    mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Manganese </br>(%) </b></font></td>";
        //    mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Sulphur </br>(%) </b></font></td>";
        //    mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Phosphorous </br>(%) </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Suphur + Phosphorous </br>(%) </b></font></td>";
        //    mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Compliance </b></font></td>";


        //    int i = 0;
        //    int SrNo = 0;
        //    bool valid = false;
        //    decimal Sulphur = 0;
        //    decimal Phosphorous = 0;
        //    decimal SumOfSulPhos = 0;

        //    var wInwd = dc.AllInwdDetails_View(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "STC");
        //    foreach (var STC in wInwd)
        //    {
        //        SrNo++;
        //        string[] line = STC.STCTEST_Sulphur_var.Split('*');
        //        foreach (string line1 in line)
        //        {
        //            if (line1 != "")
        //            {
        //                if (decimal.TryParse(line1, out Sulphur))
        //                {
        //                    Sulphur = Convert.ToDecimal(line1);
        //                }
        //            }
        //        }
        //        string[] line3 = STC.STCTEST_Sulphur_var.Split('*');
        //        foreach (string line4 in line3)
        //        {
        //            if (line4 != "")
        //            {
        //                if (decimal.TryParse(line4, out Phosphorous))
        //                {
        //                    Phosphorous = Convert.ToDecimal(line4);
        //                }
        //            }
        //        }
        //        SumOfSulPhos = Sulphur + Phosphorous;

        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + STC.STCINWD_IdMark_var + "</font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + STC.STCINWD_Daimeter_tint + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + STC.STCTEST_Carbon_var + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + STC.STCTEST_Manganese_var + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + STC.STCTEST_Sulphur_var + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + STC.STCTEST_Phosphorous_var + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + SumOfSulPhos.ToString("0.000") + "</font></td>";


        //        decimal SpecifiedLmt = 0;

        //        decimal result = 0;
        //        decimal Variation = 0;

        //        var gInwd = dc.AllInwdDetails_View(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, STC.STCINWD_Grade_var, 0, "STC");

        //        foreach (var grd in gInwd)
        //        {
        //            decimal Value = 0;
        //            if (grd.Constituents.ToString() == "Carbon")
        //            {
        //                SpecifiedLmt = Convert.ToDecimal(grd.SpecifiedLimit);
        //                var variat = dc.AllInwdDetails_View(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "% Variation", 0, "STC");
        //                foreach (var vat in variat)
        //                {
        //                    if (vat.Constituents.ToString() == "Carbon")
        //                    {
        //                        Variation = Convert.ToDecimal(vat.SpecifiedLimit);
        //                        result = Variation + SpecifiedLmt;
        //                        string[] s = STC.STCTEST_Carbon_var.Split('*');
        //                        foreach (string line1 in s)
        //                        {
        //                            if (line1 != "")
        //                            {
        //                                if (decimal.TryParse(line1, out Value))
        //                                {
        //                                    Value = Convert.ToDecimal(line1);
        //                                }
        //                            }
        //                        }
        //                        if (Value > result)
        //                        {
        //                            valid = true;
        //                        }
        //                        else
        //                        {

        //                        }
        //                        break;
        //                    }
        //                }

        //            }
        //            if (grd.Constituents.ToString() == "Sulphur")
        //            {
        //                SpecifiedLmt = Convert.ToDecimal(grd.SpecifiedLimit);
        //                var variat = dc.AllInwdDetails_View(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "% Variation", 0, "STC");
        //                foreach (var vat in variat)
        //                {
        //                    if (vat.Constituents.ToString() == "Sulphur")
        //                    {
        //                        Variation = Convert.ToDecimal(vat.SpecifiedLimit);
        //                        result = Variation + SpecifiedLmt;
        //                        string[] s = STC.STCTEST_Sulphur_var.Split('*');
        //                        foreach (string line1 in s)
        //                        {
        //                            if (line1 != "")
        //                            {
        //                                if (decimal.TryParse(line1, out Value))
        //                                {
        //                                    Value = Convert.ToDecimal(line1);
        //                                }
        //                            }
        //                        }

        //                        if (Value > result)
        //                        {
        //                            valid = true;
        //                        }
        //                        else
        //                        {

        //                        }
        //                        break;
        //                    }
        //                }
        //            }


        //            if (grd.Constituents.ToString() == "Phosphorous")
        //            {
        //                SpecifiedLmt = Convert.ToDecimal(grd.SpecifiedLimit);
        //                var variat = dc.AllInwdDetails_View(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "% Variation", 0, "STC");
        //                foreach (var vat in variat)
        //                {
        //                    if (vat.Constituents.ToString() == "Phosphorous")
        //                    {
        //                        Variation = Convert.ToDecimal(vat.SpecifiedLimit);
        //                        result = Variation + SpecifiedLmt;
        //                        string[] s = STC.STCTEST_Phosphorous_var.Split('*');
        //                        foreach (string line1 in s)
        //                        {
        //                            if (line1 != "")
        //                            {
        //                                if (decimal.TryParse(line1, out Value))
        //                                {
        //                                    Value = Convert.ToDecimal(line1);
        //                                }
        //                            }
        //                        }
        //                        if (Value > result)
        //                        {
        //                            valid = true;
        //                        }
        //                        else
        //                        {

        //                        }
        //                        break;
        //                    }
        //                }


        //            }
        //            if (grd.Constituents.ToString() == "Sulphur + Phosphorous")
        //            {
        //                SpecifiedLmt = Convert.ToDecimal(grd.SpecifiedLimit);
        //                var variat = dc.AllInwdDetails_View(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "% Variation", 0, "STC");
        //                foreach (var vat in variat)
        //                {
        //                    if (vat.Constituents.ToString() == "Sulphur + Phosphorous")
        //                    {
        //                        Variation = Convert.ToDecimal(vat.SpecifiedLimit);
        //                        if (SumOfSulPhos > Variation)
        //                        {
        //                            valid = true;
        //                        }
        //                        else
        //                        {

        //                        }
        //                        break;
        //                    }
        //                }
        //            }
        //        }

        //        if (valid == true)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + "Fail" + "</font></td>";
        //        }
        //        else
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>" + "Pass" + "</font></td>";
        //        }

        //        mySql += "</tr>";
        //        mySql += "<tr>";
        //        mySql += "</tr>";


        //    }

        //    i++;
        //    mySql += "<tr>";
        //    mySql += "<td colspan= 9 height=19 ></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td  colspan= 3 align=center valign=medium height=19 ><font size=2><b> Specified Limits as per IS 1786-2008 </b></font></td>";
        //    mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Carbon </br> (%) </b></font></td>";
        //    mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Manganese </br>(%) </b></font></td>";
        //    mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Sulphur </br>(%) </b></font></td>";
        //    mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b>Phosphorous</br>(%) </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Sulphur + Phosphorous </br>(%) </b></font></td>";
        //    mySql += "<td width= 5%  align=center valign=medium height=19 ><font size=2><b> --- </b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr rowspan=3>";
        //    mySql += "<td colspan= 3>&nbsp; </td>";

        //    string SteelOfGrade = "";
        //    int ApprvUserId = 0;
        //    int CheckedUserId = 0;
        //    var InwardSTC = dc.ReportStatus_View("Steel Chemical Testing", null, null, 1, 0, 0, System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, 0, 0);
        //    foreach (var STC in InwardSTC)
        //    {
        //        SteelOfGrade = STC.STCINWD_Grade_var;
        //        if (STC.STCINWD_ApprovedBy_tint.ToString() != null)
        //        {
        //            ApprvUserId = Convert.ToByte(STC.STCINWD_ApprovedBy_tint);
        //        }
        //        if (STC.STCINWD_CheckedBy_tint.ToString() != null)
        //        {
        //            CheckedUserId = Convert.ToByte(STC.STCINWD_CheckedBy_tint);
        //        }

        //    }


        //    var gradeSteel = dc.AllInwdDetails_View(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, SteelOfGrade, 0, "STC");

        //    foreach (var stlgrd in gradeSteel)
        //    {
        //        if (stlgrd.Constituents.ToString() == "Carbon")
        //        {
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2>" + stlgrd.SpecifiedLimit + "  </font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2> ---  </font></td>";
        //        }

        //        if (stlgrd.Constituents.ToString() == "Sulphur")
        //        {
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2>" + stlgrd.SpecifiedLimit + " </font></td>";
        //        }
        //        if (stlgrd.Constituents.ToString() == "Phosphorous")
        //        {
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2>" + stlgrd.SpecifiedLimit + "  </font></td>";
        //        }
        //        if (stlgrd.Constituents.ToString() == "Sulphur + Phosphorous")
        //        {
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2>" + stlgrd.SpecifiedLimit + "  </font></td>";
        //        }


        //    }
        //    mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2> --- </font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td  colspan= 3 align=center valign=top height=19 ><font size=2>Variation, over specified maximum limit,% max </font></td>";



        //    var FixVariation = dc.AllInwdDetails_View(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "% Variation", 0, "STC");

        //    foreach (var stlvar in FixVariation)
        //    {
        //        if (stlvar.Constituents.ToString() == "Carbon")
        //        {
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2>" + Convert.ToDecimal(stlvar.SpecifiedLimit).ToString("0.000") + " </font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 > <font size=2> --- </font></td>";
        //        }

        //        if (stlvar.Constituents.ToString() == "Sulphur")
        //        {
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2>" + Convert.ToDecimal(stlvar.SpecifiedLimit).ToString("0.000") + " </font></td>";
        //        }
        //        if (stlvar.Constituents.ToString() == "Phosphorous")
        //        {
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2>" + Convert.ToDecimal(stlvar.SpecifiedLimit).ToString("0.000") + "  </font></td>";
        //        }
        //        if (stlvar.Constituents.ToString() == "Sulphur + Phosphorous")
        //        {
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2>" + Convert.ToDecimal(stlvar.SpecifiedLimit).ToString("0.000") + "  </font></td>";
        //        }


        //    }
        //    mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2> --- </font></td>";
        //    mySql += "</tr>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";





        //    SrNo = 0;
        //    var matid = dc.Material_View("STC", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }

        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + " )" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }


        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, "STC");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.STCDetail_RemarkId_int), "STC");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.STC_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";

        //        }
        //    }

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.ReportStatus_View("Steel Chemical Testing", null, null, 1, 0, 0, System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (r.STCINWD_ApprovedBy_tint != null && r.STCINWD_CheckedBy_tint != null)
        //            {
        //                var U = dc.User_View(r.STCINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.STCINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page of 1 of 1 " + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PNI999PTC014212" + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add - 1160/5,Gharpure Colony Shivaji Nagar, Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        //public string getDetailReportWT()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";

        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Water For Construction Purpose</b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    var water = dc.ReportStatus_View("Water Testing", null, null, 0, 0, 0, System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, 2, 0);

        //    foreach (var w in water)
        //    {
        //        mySql += "<tr>" +
        //              "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td width='40%' height=19><font size=2>" + w.CL_Name_var + "</font></td>" +
        //              "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + "-" + "</font></td>" +
        //              "<td height=19><font size=2>&nbsp;</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +

        //             "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //             "<td width='2%' height=19><font size=2></font></td>" +
        //             "<td width='10%' height=19><font size=2></font></td>" +
        //             "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "WT" + "-" + w.WTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font>:</td>" +
        //             "<td width='40%' height=19><font size=2>" + w.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "WT" + "-" + w.WTINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //            "<td width='2%' height=19><font size=2></font></td>" +
        //            "<td width='10%' height=19><font size=2></font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "WT" + "-" + "" + "</font></td>" +
        //            "</tr>";


        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + w.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(w.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //            "</tr>";



        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td height=19><font size=2></font></td>" +
        //              "<td height=19><font size=2>" + w.WTINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(w.WTINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //              "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "RESULTS : " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";


        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";


        //    mySql += "<td rowspan=2 width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td rowspan=2 width= 15% align=center valign=medium height=19 ><font size=2><b>Test Parameters</b></font></td>";
        //    mySql += "<td rowspan=2 width= 5% align=center valign=medium height=19 ><font size=2><b>Unit</b></font></td>";
        //    mySql += "<td rowspan=2 width= 5% align=center valign=medium height=19 ><font size=2><b>Observations </b></font></td>";
        //    mySql += "<td rowspan=2 width= 5% align=center valign=medium height=19 ><font size=2><b>Compliance </b></font></td>";
        //    mySql += "<td   width= 10% align=center valign=top height=19 ><font size=2><b>Permissible Limit IS:456-2000 </b></font></td>";
        //    mySql += "<td rowspan=2 width= 20% align=center valign=medium height=19 ><font size=2><b>Test Specification Used </b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "<td  width= 10% align=center valign=top height=19 ><font size=2><b>Mixing and Curing </br> Water Clause 5:4 Table 1 </b></font></td>";

        //    int i = 0;
        //    int SrNo = 0;
        //    var wInwd = dc.AllInwdDetails_View(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "", 0, "WT");
        //    foreach (var wt in wInwd)
        //    {
        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width= 15% align=center valign=top height=19 ><font size=2>&nbsp;" + wt.TEST_Name_var + "</font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + wt.splmt_Unit_var.ToString() + "</font></td>";
        //        mySql += "<td width=2% align=center valign=top height=19 ><font size=2>&nbsp;" + wt.WTTEST_Result_var.ToString() + "</font></td>";
        //        string SpecifiedLmt = "";
        //        double Obsns = 0;
        //        int result = 0;
        //        string PRC = "";
        //        bool valid = false;
        //        string Compliance = "";
        //        string Observations = wt.WTTEST_Result_var;

        //        if (double.TryParse(Observations, out Obsns))
        //        {
        //            Obsns = Convert.ToDouble(Observations);
        //        }
        //        SpecifiedLmt = wt.splmt_SpecifiedLimit_var.ToString();
        //        string[] line = SpecifiedLmt.Split(' ', ',', '-');
        //        foreach (string line1 in line)
        //        {
        //            if (line1 != " ")
        //            {
        //                if (line1 == "PCC" || line1 == "RCC")
        //                {
        //                    PRC = line1.ToString();
        //                    if (Convert.ToInt32(System.Web.HttpContext.Current.Session["res"]) > 0)
        //                    {
        //                        //Maximum 2000 - PCC,Maximum 500 - RCC
        //                        if (Obsns < Convert.ToInt32(System.Web.HttpContext.Current.Session["res"]))
        //                        {
        //                            Compliance = Compliance + "Pass " + " " + "-" + " " + PRC + "," + "<br />";
        //                            valid = true;
        //                        }
        //                        else
        //                        {
        //                            Compliance = Compliance + "Fail " + " " + "-" + " " + PRC + "," + "<br />";
        //                            valid = true;
        //                        }
        //                    }
        //                }
        //                if (int.TryParse(line1, out result))
        //                {
        //                    result = Convert.ToInt32(line1);
        //                    System.Web.HttpContext.Current.Session["res"] = result.ToString();
        //                    // ViewState["res"] = Session["res"].ToString();
        //                }
        //            }
        //        }
        //        if (valid == true)
        //        {
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + Compliance + " </font></td>";
        //        }
        //        if (valid == false)
        //        {
        //            if (wt.WTTEST_Result_var == "NIL" || SpecifiedLmt == "---")
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //            }
        //            else if (Obsns < result)
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + "Pass" + "</font></td>";
        //            }
        //            else
        //            {
        //                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + "Fail" + "</font></td>";

        //            }
        //        }

        //        mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>" + wt.splmt_SpecifiedLimit_var.ToString() + "</font></td>";
        //        mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>" + wt.splmt_testingMethod_var.ToString() + "</font></td>";


        //        mySql += "</tr>";
        //        i++;
        //    }
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    SrNo = 0;
        //    var matid = dc.Material_View("WT", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + " )" + " " + cd.Isc_Description_var.ToString() + "(Fourth Revision)" + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }


        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), 0, "WT");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.WTDetail_RemarkId_int), "WT");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.WT_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.AllInwdDetails_View(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, Convert.ToInt32(System.Web.HttpContext.Current.Session["RecordNo"]), "", 0, "WT");
        //        foreach (var r in RecNo)
        //        {
        //            var Auth = dc.AllInwdDetails_View(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, r.WTINWD_ApprovedBy_tint, 0, 0, "", 0, "WT");

        //            foreach (var Approve in Auth)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Approve.USER_Name_var.ToString() + "</font></td>";
        //                mySql += "</tr>";
        //                mySql += "<tr>";
        //                mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + Approve.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                mySql += "</tr>";

        //            }
        //            var lgin = dc.AllInwdDetails_View(System.Web.HttpContext.Current.Session["ReferenceNo"].ToString(), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, r.WTINWD_CheckedBy_tint, 0, "", 0, "WT");

        //            foreach (var loginusr in lgin)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                mySql += "</tr>";
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page of 1 of 1 " + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PNI999PTC014212" + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add - 1160/5,Gharpure Colony Shivaji Nagar, Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";
        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        //public string getDetailReportCement()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";

        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Cement Testing</b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    var water = dc.ReportStatus_View("Cement Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 2, 0);

        //    foreach (var w in water)
        //    {
        //        mySql += "<tr>" +
        //              "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td width='40%' height=19><font size=2>" + w.CL_Name_var + "</font></td>" +
        //              "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + "-" + "</font></td>" +
        //              "<td height=19><font size=2>&nbsp;</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +

        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font>:</td>" +
        //             "<td width='40%' height=19><font size=2>" + w.CL_OfficeAddress_var + "</font></td>" +
        //             "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "CEMT" + "-" + w.CEMTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +
        //            "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //            "<td width='2%' height=19><font size=2></font></td>" +
        //            "<td width='10%' height=19><font size=2></font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "CEMT" + "-" + w.CEMTINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + w.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "CEMT" + "-" + "" + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Cement Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + w.CEMTINWD_CementName_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(w.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + w.CEMTINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(w.CEMTINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +
        //            "<td align=left valign=top height=19><font size=2><b>Supplier Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + w.CEMTINWD_SupplierName_var.ToString() + "</font></td>" +
        //            "<td align=left valign=top height=19></td>" +
        //            "<td height=19></td>" +
        //            "<td height=19></td>" +
        //            "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "RESULTS : " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";


        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Name Of Test</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Unit </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Result </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Specified Limits </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Compliance </b></font></td>";
        //    mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Method Of Testing </b></font></td>";
        //    mySql += "</tr>";

        //    int i = 0;
        //    int SrNo = 0;

        //    var details = dc.AllInwdDetails_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "CEMT");
        //    foreach (var CEMT in details)
        //    {
        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        bool valid = false;
        //        string TEST_Name_var = "";

        //        // string Grade=CEMT.CEMTINWD_Grade_var.ToString;
        //        if (CEMT.TEST_Name_var.ToString() == "Compressive Strength")
        //        {
        //            if (CEMT.CEMTTEST_Days_tint.ToString() != "" && CEMT.CEMTTEST_Days_tint.ToString() != null && CEMT.CEMTTEST_Days_tint.ToString() != "0")
        //            {
        //                TEST_Name_var = " " + "(" + "" + CEMT.CEMTTEST_Days_tint.ToString() + " " + "Days" + " " + ")" + " " + CEMT.TEST_Name_var.ToString();
        //                mySql += "<td width=10% align=center valign=top height=19 ><font size=2>&nbsp;" + TEST_Name_var + "</font></td>";
        //            }
        //        }
        //        else
        //        {
        //            mySql += "<td width=20% align=center valign=top height=19 ><font size=2>&nbsp;" + CEMT.TEST_Name_var.ToString() + "</font></td>";
        //        }
        //        var Id = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, true, CEMT.TEST_Id, "", 0, 0, 0, 0, 0, "", 0, "CEMT");
        //        foreach (var testid in Id)
        //        {
        //            valid = true;
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(testid.splmt_Unit_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(CEMT.CEMTTEST_Result_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(testid.splmt_SpecifiedLimit_var) + "</font></td>";
        //            int SpecifiedLmtRes = 0;
        //            bool validmax = false;
        //            string res = "";

        //            string[] SpceifiedLmt = Convert.ToString(testid.splmt_SpecifiedLimit_var).Split(' ', ',');
        //            foreach (var Comp in SpceifiedLmt)
        //            {
        //                if (Comp != "")
        //                {
        //                    if (Comp.Trim() == "Maximum")
        //                    {
        //                        validmax = true;
        //                    }
        //                    if (Comp.Trim() == "PCC" || Comp.Trim() == "RCC")
        //                    {
        //                        res = res + " " + "-" + " " + Comp + ',' + "</br>";
        //                    }
        //                    if (int.TryParse(Comp, out SpecifiedLmtRes))
        //                    {
        //                        SpecifiedLmtRes = Convert.ToInt32(Comp.ToString());
        //                        if (validmax == true)
        //                        {
        //                            if (Convert.ToString(CEMT.CEMTTEST_Result_var).Trim() != "Awaited" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "---" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "" && Convert.ToString(testid.splmt_SpecifiedLimit_var).Trim() != "Not Specified")
        //                            {
        //                                if ((Convert.ToDouble(CEMT.CEMTTEST_Result_var)) < Convert.ToInt32(SpecifiedLmtRes))
        //                                {
        //                                    res = res + "Pass ";
        //                                }
        //                                else
        //                                {
        //                                    res = res + "Fail ";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                res = "---";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (Convert.ToString(CEMT.CEMTTEST_Result_var).Trim() != "Awaited" && Convert.ToString(CEMT.CEMTTEST_Result_var).Trim() != "*" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "---" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "" && Convert.ToString(testid.splmt_SpecifiedLimit_var).Trim() != "Not Specified")
        //                            {
        //                                if ((Convert.ToDouble(CEMT.CEMTTEST_Result_var)) > Convert.ToInt32(SpecifiedLmtRes))
        //                                {
        //                                    res = res + "Pass ";
        //                                }
        //                                else
        //                                {
        //                                    res = res + "Fail ";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                res = "---";
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + res + "</font></td>";
        //            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2>" + Convert.ToString(testid.splmt_testingMethod_var) + "</font></td>";
        //            break;
        //        }
        //        if (valid == false)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(CEMT.CEMTTEST_Result_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //        }
        //        mySql += "</tr>";
        //    }
        //    i++;
        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    SrNo = 0;
        //    var matid = dc.Material_View("CEMT", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "CEMT");// ddl_Grade.SelectedItem.Text// instead of Grade
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + " )" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, "CEMT");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.CEMTDetail_RemarkId_int), "CEMT");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.CEMT_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.ReportStatus_View("Chemical Testing", null, null, 1, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (r.CEMTINWD_ApprovedBy_tint != null && r.CEMTINWD_CheckedBy_tint != null)
        //            {
        //                var U = dc.User_View(r.CEMTINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.CEMTINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page of 1 of 1 " + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PNI999PTC014212" + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add - 1160/5,Gharpure Colony Shivaji Nagar, Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";
        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        //public string getDetailReportFlyash()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";

        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Fly Ash Testing</b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    var FlyashInwd = dc.ReportStatus_View("Fly Ash Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 2, 0);
        //    string CementCubeStrength = string.Empty;
        //    var cemtavg = dc.OtherCubeTestView(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "FLYASH", 28, 0, "CEMT", false, true);
        //    foreach (var cmavg in cemtavg)
        //    {
        //        CementCubeStrength = Convert.ToString(cmavg.Avg_var);
        //        break;
        //    }
        //    foreach (var flyash in FlyashInwd)
        //    {
        //        mySql += "<tr>" +
        //              "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td width='40%' height=19><font size=2>" + flyash.CL_Name_var + "</font></td>" +
        //              "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + "-" + "</font></td>" +
        //              "<td height=19><font size=2>&nbsp;</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +

        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font>:</td>" +
        //             "<td width='40%' height=19><font size=2>" + flyash.CL_OfficeAddress_var + "</font></td>" +
        //             "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "FLYASH" + "-" + flyash.FLYASHINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +
        //            "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //            "<td width='2%' height=19><font size=2></font></td>" +
        //            "<td width='10%' height=19><font size=2></font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "FLYASH" + "-" + flyash.FLYASHINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + flyash.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "FLYASH" + "-" + "" + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Cement Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + flyash.FLYASHINWD_CementName_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(flyash.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td height=19><font size=2></font></td>" +
        //              "<td height=19><font size=2>" + flyash.FLYASHINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(flyash.FLYASHINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //              "</tr>";


        //        mySql += "<tr>" +
        //                    "<td align=left valign=top height=19><font size=2><b>Supplier Name</b></font></td>" +
        //                    "<td width='2%' height=19><font size=2>:</font></td>" +
        //                    "<td height=19><font size=2>" + flyash.FLYASHINWD_SupplierName_var.ToString() + "</font></td>" +
        //                     "<td align=left valign=top height=19><font size=2><b>Cement Cube Strength</b></font></td>" +
        //                     "<td height=19><font size=2>:</font></td>" +
        //                     "<td height=19><font size=2>" + CementCubeStrength + "</font></td>" +
        //            "</tr>";


        //        mySql += "<tr>" +
        //          "<td align=left valign=top height=19><font size=2><b>Flyash Name</b></font></td>" +
        //          "<td width='2%' height=19><font size=2>:</font></td>" +
        //          "<td height=19><font size=2>" + flyash.FLYASHINWD_FlyAshName_var + "</font></td>" +
        //          "<td align=left valign=top height=19></td>" +
        //          "<td height=19></td>" +
        //          "<td height=19></td>" +
        //          "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "RESULTS : " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";


        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Name Of Test</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Unit </b></font></td>";
        //    mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Result </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Specified Limits </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Compliance </b></font></td>";
        //    mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Method Of Testing </b></font></td>";
        //    mySql += "</tr>";


        //    int i = 0;
        //    int SrNo = 0;
        //    var details = dc.AllInwdDetails_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, 0, "", 0, "FLYASH");
        //    foreach (var FLYASH in details)
        //    {
        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        bool valid = false;
        //        string TEST_Name_var = "";

        //        if (FLYASH.TEST_Name_var.ToString() == "Compressive Strength")
        //        {
        //            if (FLYASH.FLYASHTEST_Days_tint.ToString() != "" && FLYASH.FLYASHTEST_Days_tint.ToString() != null && FLYASH.FLYASHTEST_Days_tint.ToString() != "0")
        //            {
        //                TEST_Name_var = " " + "(" + "" + FLYASH.FLYASHTEST_Days_tint.ToString() + " " + "Days" + " " + ")" + " " + FLYASH.TEST_Name_var.ToString();
        //                mySql += "<td width=10% align=center valign=top height=19 ><font size=2>&nbsp;" + TEST_Name_var + "</font></td>";
        //            }
        //        }
        //        else
        //        {
        //            mySql += "<td width=20% align=center valign=top height=19 ><font size=2>&nbsp;" + FLYASH.TEST_Name_var.ToString() + "</font></td>";
        //        }
        //        var Id = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, true, FLYASH.TEST_Id, "", 0, 0, 0, 0, 0, "", 0, "FLYASH");
        //        foreach (var testid in Id)
        //        {
        //            valid = true;
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(testid.splmt_Unit_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(FLYASH.FLYASHTEST_Result_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(testid.splmt_SpecifiedLimit_var) + "</font></td>";
        //            int SpecifiedLmtRes = 0;
        //            bool validmax = false;
        //            string res = "";

        //            string[] SpceifiedLmt = Convert.ToString(testid.splmt_SpecifiedLimit_var).Split(' ', ',');
        //            foreach (var Comp in SpceifiedLmt)
        //            {
        //                if (Comp != "")
        //                {
        //                    if (Comp.Trim() == "Maximum")
        //                    {
        //                        validmax = true;
        //                    }
        //                    if (Comp.Trim() == "PCC" || Comp.Trim() == "RCC")
        //                    {
        //                        res = res + " " + "-" + " " + Comp + ',' + "</br>";
        //                    }
        //                    if (int.TryParse(Comp, out SpecifiedLmtRes))
        //                    {
        //                        SpecifiedLmtRes = Convert.ToInt32(Comp.ToString());
        //                        if (validmax == true)
        //                        {
        //                            if (Convert.ToString(FLYASH.FLYASHTEST_Result_var).Trim() != "Awaited" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "---" && Convert.ToString(testid.splmt_SpecifiedLimit_var).Trim() != "Not Specified")
        //                            {
        //                                if ((Convert.ToDouble(FLYASH.FLYASHTEST_Result_var.ToString())) < Convert.ToInt32(SpecifiedLmtRes))
        //                                {
        //                                    res = res + "Pass ";
        //                                }
        //                                else
        //                                {
        //                                    res = res + "Fail ";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                res = "---";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (Convert.ToString(FLYASH.FLYASHTEST_Result_var).Trim() != "Awaited" && Convert.ToString(testid.splmt_SpecifiedLimit_var) != "---" && Convert.ToString(testid.splmt_SpecifiedLimit_var).Trim() != "Not Specified")
        //                            {
        //                                if ((Convert.ToDouble(FLYASH.FLYASHTEST_Result_var.ToString())) > Convert.ToInt32(SpecifiedLmtRes))
        //                                {
        //                                    res = res + "Pass ";
        //                                }
        //                                else
        //                                {
        //                                    res = res + "Fail ";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                res = "---";
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + res + "</font></td>";
        //            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2>" + Convert.ToString(testid.splmt_testingMethod_var) + "</font></td>";
        //            break;
        //        }
        //        if (valid == false)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(FLYASH.FLYASHTEST_Result_var) + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "---" + "</font></td>";
        //        }
        //        mySql += "</tr>";
        //    }
        //    i++;

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    SrNo = 0;
        //    var matid = dc.Material_View("FLYASH", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "FLYASH");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + " )" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, "FLYASH");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.FLYASHDetail_RemarkId_int), "FLYASH");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.FLYASH_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";

        //        }
        //    }

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.ReportStatus_View("Fly Ash Testing", null, null, 1, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (r.FLYASHINWD_ApprovedBy_tint != null && r.FLYASHINWD_CheckedBy_tint != null)
        //            {
        //                var U = dc.User_View(r.FLYASHINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.FLYASHINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page of 1 of 1 " + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PNI999PTC014212" + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add - 1160/5,Gharpure Colony Shivaji Nagar, Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        //public string getDetailReportPT_WA()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";
        //    String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Precast Concrete Blocks for Paving - Water Absorption </b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


        //    var pt = dc.ReportStatus_View("Pavement Block Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 2, 0);

        //    foreach (var pavmt in pt)
        //    {
        //        mySql += "<tr>" +
        //        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td width='40%' height=19><font size=2>" + pavmt.CL_Name_var + "</font></td>" +
        //        "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //        "<td width='2%' height=19><font size=2>:</font></td>" +
        //        "<td height=19><font size=2>" + "-" + "</font></td>" +
        //        "<td height=19><font size=2>&nbsp;</font></td>" +
        //        "</tr>";


        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font></td>" +
        //            "<td width='40%' height=19><font size=2>" + pavmt.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //         "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //         "<td width='2%' height=19><font size=2></font></td>" +
        //         "<td width='10%' height=19><font size=2></font></td>" +
        //         "<td height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //         "<td height=19><font size=2>:</font></td>" +
        //         "<td height=19><font size=2>" + "PT" + "-" + pavmt.PTINWD_ReferenceNo_var + "</font></td>" +
        //         "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + pavmt.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "PT" + "-" + " " + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //           "<td align=left valign=top height=19><font size=2><b>Nature Of Work</b></font></td>" +
        //           "<td  width='2%' height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + pavmt.PTINWD_WorkingNature_var + "</font></td>" +
        //           "<td align=left valign=top height=19><font size=2><b> Date of Casting</b></font></td>" +
        //           "<td height=19><font size=2>:</font></td>" +
        //           "<td height=19><font size=2>" + pavmt.PTINWD_CastingDate_dt + "</font></td>" +
        //           "</tr>";

        //        mySql += "<tr>" +
        //            "<td align=left valign=top height=19><font size=2><b> Grade Of Concrete</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + pavmt.PTINWD_Grade_var + "</font></td>" +
        //            "<td align=left valign=top height=19><font size=2><b>Date Of Receipt </b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + DateTime.Now.ToString("dd/MMM/yy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + pavmt.PTINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(pavmt.PTINWD_TestingDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
        //              "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Id Mark</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Dry Weight </br> (g)</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Wet Weight </br>  (g) </b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Water Absorption </br>  (%)</b></font></td>";
        //    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Average </br>  (%)</b></font></td>";
        //    mySql += "</tr>";

        //    int SrNo = 0;
        //    int i = 0;
        //    var PT_WA = dc.Pavement_Test_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]), "WA");
        //    var count = PT_WA.Count();
        //    var PT_WaterAbsorp = dc.Pavement_Test_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]), "WA");
        //    foreach (var ptwa in PT_WaterAbsorp)
        //    {
        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.IdMark_var) + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.DryWeight_int) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.Wet_Weight_int) + "</font></td>";
        //        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.WaterAbsorption_dec) + "</font></td>";
        //        if (i == 0)
        //        {
        //            mySql += "<td  BorderStyle=none width= 10%  align=center rowspan=" + count + " valign=middle height=19 ><font size=2>&nbsp;" + Convert.ToString(ptwa.PTINWD_AvgStr_var) + "</font></td>";
        //        }
        //        i++;
        //        mySql += "</tr>";
        //    }

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    SrNo = 0;
        //    var matid = dc.Material_View("PT", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    SrNo = 0;
        //    var re = dc.Pavement_Test_Remark_View("", Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]));
        //    foreach (var r in re)
        //    {
        //        var remark = dc.Pavement_Test_Remark_View("", "", Convert.ToInt32(r.RemarkId_int), Convert.ToInt32(System.Web.HttpContext.Current.Session["TestId"]));
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.ReportStatus_View("Pavement Block Testing", null, null, 1, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (Convert.ToString(r.PTINWD_ApprovedBy_tint) != string.Empty && Convert.ToString(r.PTINWD_CheckedBy_tint) != string.Empty)
        //            {
        //                var U = dc.User_View(r.PTINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.PTINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page 1 of 1" + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PN1999PTC014212" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add:- 1160/5, Gharpure Colony Shivaji Nagar ,Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}

        //public string getDetailReportPile()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";

        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Pile Integrity</b></font></td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    var ct = dc.ReportStatus_View("Pile Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 2, 0);

        //    foreach (var pile in ct)
        //    {
        //        mySql += "<tr>" +
        //              "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td width='40%' height=19><font size=2>" + pile.CL_Name_var + "</font></td>" +
        //              "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + "-" + "</font></td>" +
        //              "<td height=19><font size=2>&nbsp;</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +

        //             "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //             "<td width='2%' height=19><font size=2></font></td>" +
        //             "<td width='10%' height=19><font size=2></font></td>" +
        //             "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "PILE" + "-" + pile.PILEINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td  width='2%' height=19><font size=2></font>:</td>" +
        //             "<td width='40%' height=19><font size=2>" + pile.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "PILE" + "-" + pile.PILEINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //            "<td width='2%' height=19><font size=2></font></td>" +
        //            "<td width='10%' height=19><font size=2></font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "PILE" + "-" + "" + "</font></td>" +
        //            "</tr>";


        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + pile.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(pile.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //            "</tr>";



        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + pile.PILEINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(pile.PILEINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //              "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "Test Results : " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";


        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr>";


        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Catagory</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Inference</b></font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Pile Identification </b></font></td>";
        //    mySql += "</tr>";

        //    int i = 0;
        //    int SrNo = 0;
        //    int CountPiles = 0;
        //    var details = dc.PileDetailsView("", 0, "");
        //    foreach (var t in details)
        //    {

        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + t.PILE_Name_var + "</font></td>";
        //        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + t.PILE_Description_var + "</font></td>";


        //        bool valid = true;

        //        string Identi = "";
        //        var pl = dc.PileDetailsView(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, "");
        //        foreach (var p in pl)
        //        {
        //            if (Convert.ToInt32(p.PILEDETAIL_CatagoryId_int) > 0)
        //            {
        //                var c = dc.PileDetailsView("", Convert.ToInt32(p.PILEDETAIL_CatagoryId_int), "");
        //                foreach (var n in c)
        //                {
        //                    if (p.PILEDETAIL_Identification_var != null)
        //                    {
        //                        if (n.PILE_Name_var.ToString() == t.PILE_Name_var.ToString())
        //                        {
        //                            CountPiles++;
        //                            valid = false;
        //                            Identi = Identi + p.PILEDETAIL_Identification_var.ToString() + ",";
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (valid == true)
        //        {
        //            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "NA" + "</font></td>";
        //        }
        //        else
        //        {
        //            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + Identi.ToString() + "</font></td>";
        //        }

        //        mySql += "</tr>";
        //    }

        //    i++;

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "Total No Of Piles Tested :" + "</font></td>";
        //    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + CountPiles + "</font></td>";
        //    mySql += "</tr>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";




        //    SrNo = 0;
        //    var matid = dc.Material_View("PILE", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes:" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + " " + cd.Isc_Description_var.ToString() + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }


        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, "PILE");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.PILEDetail_RemarkId_int), "PILE");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.PILE_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";

        //        }
        //    }

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.AllInwdDetails_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, 0, 0, Convert.ToInt32(System.Web.HttpContext.Current.Session["RecordNo"]), "", 0, "PILE");
        //        foreach (var r in RecNo)
        //        {
        //            var Auth = dc.AllInwdDetails_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "", 0, "", 0, null, "", "", 0, false, false, true, 0, "", 0, 0, r.PILEINWD_ApprovedBy_tint, 0, 0, "", 0, "PILE");

        //            foreach (var Approve in Auth)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Approve.USER_Name_var.ToString() + "</font></td>";
        //                mySql += "</tr>";
        //                mySql += "<tr>";
        //                mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + Approve.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                mySql += "</tr>";

        //            }

        //        }
        //    }


        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}
        //public string getDetailReportOT()
        //{
        //    string reportStr = "", mySql = "", tempSql = "";
        //    mySql += "<html>";
        //    mySql += "<head>";
        //    mySql += "<style type='text/css'>";
        //    mySql += "body {margin-left:2em margin-right:2em}";
        //    mySql += "</style>";
        //    mySql += "</head>";
        //    mySql += "<tr><td width='100%' height='105'>";

        //    mySql += "&nbsp;</td></tr>";
        //    mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //    mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

        //    mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Office Copy</b></font></td></tr>";
        //    var wInwd = dc.AllInwdDetails_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "", 0, "OT");
        //    foreach (var O in wInwd)
        //    {
        //        if (O.OTINWD_ReportForTestId_int == 109)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Autoclaved aerated cellular concrete products compressive</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 110)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Autoclaved aerated cellular concrete products Density</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 111)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Chemical Admixture</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 112)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Concrete Beam Flexural Strength</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 113)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Chemical Testing of slag</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 114)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Coarse Aggregate</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 115)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Concrete Chloride</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 116)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Chemical resistance of ceramic Tiles</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 117)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Compressive Strength of Joining Mortar T9 Block Fix</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 118)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Door Shutter</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 119)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Deletarious Material</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 120)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Door Frame Wood</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 121)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Fuel Ash</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 122)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Fine Aggregate</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 123)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Gypsum</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 124)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Gypsum Chemical</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 125)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Grout Cube Compressive Strength</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 126)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Ground granulated blast furnance slag + cement</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 127)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Natural Building Stone</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 128)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>P.O.P Cube Compressive Strength</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 129)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Plywood</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 130)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Powder Coating Thickness</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 131)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Reinforcement Splice Bar</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 132)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Structural Steel Tensile</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 133)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Steel Chemical</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 134)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Silica Fume</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 135)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Wood Sample</b></font></td></tr>";
        //        }
        //        else if (O.OTINWD_ReportForTestId_int == 139)
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Other New Test</b></font></td></tr>";
        //        }
        //        else
        //        {
        //            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Other Testing</b></font></td></tr>";
        //        }

        //    }
        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    var water = dc.ReportStatus_View("Other Testing", null, null, 0, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 2, 0);

        //    foreach (var w in water)
        //    {
        //        mySql += "<tr>" +
        //              "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td width='40%' height=19><font size=2>" + w.CL_Name_var + "</font></td>" +
        //              "<td height=19><font size=2><b>Date Of Issue</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + DateTime.Now.ToString("dd/MM/yy") + "</font></td>" +
        //              "<td height=19><font size=2>&nbsp;</font></td>" +
        //              "</tr>";

        //        mySql += "<tr>" +

        //             "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //             "<td width='2%' height=19><font size=2>:</font></td>" +
        //             "<td width='10%' height=19><font size=2></font></td>" +
        //             "<td height=19><font size=2><b>Record No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "OT" + "-" + w.OTINWD_SetOfRecord_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +
        //             "<td align=left valign=top height=19><font size=2><b> Office Address</b></font></td>" +
        //             "<td height=19><font size=2></font>:</td>" +
        //             "<td width='40%' height=19><font size=2>" + w.CL_OfficeAddress_var + "</font></td>" +
        //             "<td align=left valign=top height=19><font size=2><b>Sample Ref No.</b></font></td>" +
        //             "<td height=19><font size=2>:</font></td>" +
        //             "<td height=19><font size=2>" + "OT" + "-" + w.OTINWD_ReferenceNo_var.ToString() + "</font></td>" +
        //             "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b></b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2></font></td>" +
        //            "<td height=19><font size=2><b>Bill No.</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + "OT" + "-" + "" + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +

        //            "<td width='20%' height=19><font size=2><b>Site Name</b></font></td>" +
        //            "<td width='2%' height=19><font size=2>:</font></td>" +
        //            "<td width='10%' height=19><font size=2>" + w.SITE_Name_var + "</font></td>" +
        //            "<td height=19><font size=2><b>Date Of Receipt</b></font></td>" +
        //            "<td height=19><font size=2>:</font></td>" +
        //            "<td height=19><font size=2>" + Convert.ToDateTime(w.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //            "</tr>";

        //        mySql += "<tr>" +
        //              "<td align=left valign=top height=19><font size=2><b>Description</b></font></td>" +
        //              "<td width='2%' height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + w.OTINWD_Description_var.ToString() + "</font></td>" +
        //              "<td align=left valign=top height=19><font size=2><b>Date Of Testing</b></font></td>" +
        //              "<td height=19><font size=2>:</font></td>" +
        //              "<td height=19><font size=2>" + Convert.ToDateTime(w.OTINWD_TestedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
        //              "</tr>";

        //    }
        //    mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //    mySql += "<tr><td colspan=6 align=left valign=top>";


        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>" + "OBSERVATIONS & CALCULATIONS : " + "</b></font></td>";
        //    mySql += "</tr>";
        //    mySql += "</table>";

        //    int i = 0;
        //    int SrNo = 0;
        //    string OtherDeatils = "";

        //    var oInwd = dc.AllInwdDetails_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "", 0, "OT");
        //    foreach (var OT in oInwd)
        //    {
        //        if (OT.OTINWD_ReportForTestId_int == 127)
        //        {
        //            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Name Of the Test</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Unit</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Result </b></font></td>";
        //            mySql += "</tr>";

        //        }
        //        string[] Line1;
        //        OtherDeatils = OT.OTDETAIL_DetailTest_var.ToString();
        //        Line1 = OtherDeatils.Split('$');
        //        if (OT.OTINWD_ReportForTestId_int == 127)
        //        {
        //            string[] testdetails = OtherDeatils.Split('`');
        //            foreach (string test in testdetails)
        //            {
        //                Line1 = test.Split('$');
        //            }
        //        }
        //        if (OT.OTINWD_ReportForTestId_int == 127)
        //        {
        //            string[] testdetails = OtherDeatils.Split('`');
        //            foreach (string test in testdetails)
        //            {
        //                string[] Testrow = test.Split('$');
        //                foreach (string txtrow in Testrow)
        //                {
        //                    if (txtrow != "")
        //                    {
        //                        mySql += "<tr>";
        //                        SrNo++;
        //                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //                        string[] TestDispaly = txtrow.Split('~');
        //                        foreach (string test1 in TestDispaly)
        //                        {
        //                            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + test1.ToString() + "</font></td>";
        //                        }
        //                        mySql += "</tr>";
        //                    }
        //                }
        //                mySql += "</table>";
        //                mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
        //                break;
        //            }
        //        }



        //        mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
        //        mySql += "<tr>";
        //        //mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //        if (OT.OTINWD_ReportForTestId_int == 109)
        //        {
        //            mySql += "<td width= 2% align=center rowspan=2 valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 5% align=center rowspan=2 valign=medium height=19 ><font size=2><b>ID Mark</b></font></td>";
        //            mySql += "<td width= 5% align=center colspan=3 valign=medium height=19 ><font size=2><b>Size of Specimen (mm)</b></font></td>";
        //            mySql += "<td width= 5% align=center rowspan=2 valign=medium height=19 ><font size=2><b>Cross </br> Sectional </br> Area (mm<sup>2</sup>)  </b></font></td>";
        //            mySql += "<td width= 5% align=center rowspan=2 valign=medium height=19 ><font size=2><b>Reading </br> (kN)  </b></font></td>";
        //            mySql += "<td width= 5% align=center rowspan=2 valign=medium height=19 ><font size=2><b>Comp. Strength (N/mm<sup>2</sup>)</b></font></td>";
        //            mySql += "<td width= 5% align=center rowspan=2 valign=medium height=19 ><font size=2><b>Avg. Comp Strength (N/mm<sup>2</sup>) </b></font></td>";
        //            mySql += "</tr> ";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Length</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Breadth </b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Height </b></font></td>";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 110)
        //        {
        //            mySql += "<td width= 2% align=center rowspan=2 valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 5% align=center rowspan=2 valign=medium height=19 ><font size=2><b>ID Mark</b></font></td>";
        //            mySql += "<td width= 5% align=center colspan=3 valign=medium height=19 ><font size=2><b>Size of specimen (mm)</b></font></td>";
        //            mySql += "<td width= 5% align=center rowspan=2 valign=medium height=19 ><font size=2><b>Weight </br>(kg) </b></font></td>";
        //            mySql += "<td width= 5% align=center rowspan=2 valign=medium height=19 ><font size=2><b>Density </br>(kg/m<sup>3</sup>)  </b></font></td>";
        //            mySql += "</tr> ";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Length</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Breadth </b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Height </b></font></td>";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 111)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Name of Test</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Unit </b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Test Result </b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 112)
        //        {
        //            mySql += "<td width= 2% align=center rowspan=2 valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 5% align=center  rowspan=2 valign=medium height=19 ><font size=2><b>ID Mark</b></font></td>";
        //            mySql += "<td width= 5% align=center  rowspan=2 valign=medium height=19 ><font size=2><b>Day</b></font></td>";
        //            mySql += "<td width= 5% align=center colspan=3 valign=medium height=19 ><font size=2><b>Size </br> 1 mm x b mm x d mm</b></font></td>";
        //            mySql += "<td width= 5% align=center  rowspan=2 valign=medium height=19 ><font size=2><b>Load </br>(kN)  </b></font></td>";
        //            mySql += "<td width= 5% align=center  rowspan=2 valign=medium height=19 ><font size=2><b>Flexural Strength (Modulus of rupture fb)(N/mm<sup> 2</sup>) </b></font></td>";
        //            mySql += "</tr> ";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Length</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Breadth </b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Height </b></font></td>";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 113)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Test</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Unit</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Results</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Requirement As per BS6699:1992  </b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Compliance  </b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 114)
        //        {
        //            mySql += "<td width= 2% align=center rowspan=2 valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 10% colspan=2 align=center valign=medium height=19 ><font size=2><b>Sieve Size</b></font>";
        //            mySql += "<td width= 10% align=center  rowspan=2  valign=medium height=19 ><font size=2><b>Grading of original Sample(%)</b></font></td>";
        //            mySql += "<td width= 10% align=center  rowspan=2  valign=medium height=19 ><font size=2><b>Weight of test fraction before test(gm)  </b></font></td>";
        //            mySql += "<td width= 10% align=center  rowspan=2  valign=medium height=19 ><font size=2><b>Weight of test fraction Retained after 5th cycle(gm) </b></font></td>";
        //            mySql += "<td width= 10% align=center  rowspan=2  valign=medium height=19 ><font size=2><b>% passing after test(Actual Loss)(%) </b></font></td>";
        //            mySql += "<td width= 10% align=center  rowspan=2  valign=medium height=19 ><font size=2><b>Weight average (Corrected % Loss) </b></font></td>";
        //            mySql += "</tr> ";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Passing</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Retained</b></font></td>";
        //        }

        //        else if (OT.OTINWD_ReportForTestId_int == 115)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 20% align=center valign=medium height=19 ><font size=2><b> Name of Test</b></font></td>";
        //            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Result</b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 116)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Sample Description</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Name of the Test</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Test Solution</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Visible Changes  </b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Classes of Resistance</b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 117)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Name of the Test</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Result</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Unit </b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 118)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Id Mark</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Name of the Test</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Sample </b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Observation/Remark </b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 119)
        //        {
        //            mySql += "<td width= 2% rowspan=2 align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 5% rowspan=2 align=center valign=medium height=19 ><font size=2><b>Contaminent</b></font></td>";
        //            mySql += "<td width= 5% rowspan=2  align=center valign=medium height=19 ><font size=2><b>Unit</b></font></td>";
        //            mySql += "<td width= 10% rowspan=2 align=center valign=medium height=19 ><font size=2><b>Result</b></font></td>";
        //            //mySql += "<td width= 10% colspan=2    align=center valign=medium height=19 ><font size=2><b>Specified limits as per IS 383</b></font></td>";

        //            mySql += "<td width= 10% align=center colspan=2 valign=medium height=19 ><font size=2><b>Fine Aggregate Percenatge By Weight,Max</b></font>";
        //            mySql += "<td width= 10% align=center colspan=2 valign=medium height=19 ><font size=2><b>Coarse Aggregate Percenatge By Weight,Max</b></font>";
        //            mySql += "</tr> ";

        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Uncrushed</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Crushed</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Uncrushed</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Crushed</b></font></td>";
        //            mySql += "</tr> ";

        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 120)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Density</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sample ID</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Average Density</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Moisture Content</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Average Moisture Content</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Moisture Content (%)Max</b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 121)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Test</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Unit</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Test Results</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Siliceous Pulverized Fuel Ash</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Calcareous Pulverized Fuel Ash</b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 122)
        //        {
        //            mySql += "<td width= 2% rowspan=2 align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 2% colspan=2 align=center valign=medium height=19 ><font size=2><b>Sieve Size</b></font></td>";
        //            mySql += "<td width= 5% rowspan=2 align=center valign=medium height=19 ><font size=2><b>Grading of original Sample(%)</b></font></td>";
        //            mySql += "<td width= 5% rowspan=2 align=center valign=medium height=19 ><font size=2><b>Weight of test fraction before test(gm)  </b></font></td>";
        //            mySql += "<td width= 5% rowspan=2 align=center valign=medium height=19 ><font size=2><b>Weight of test fraction Retained after 5th cycle(gm) </b></font></td>";
        //            mySql += "<td width= 5% rowspan=2 align=center valign=medium height=19 ><font size=2><b>% passing after test(Actual Loss)(%) </b></font></td>";
        //            mySql += "<td width= 5% rowspan=2 align=center valign=medium height=19 ><font size=2><b>Weight average (Corrected % Loss) </b></font></td>";
        //            mySql += "</tr>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Passing</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Retained</b></font></td>";

        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 123)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Name Of the Test</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Result </b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Specified Limit </b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Reference </b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 124)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Name Of the Test</b></font></td>";
        //            mySql += "<td width= 12% align=center valign=medium height=19 ><font size=2><b>Test Result </b></font></td>";
        //            mySql += "<td width= 12% align=center valign=medium height=19 ><font size=2><b>Specified Limit </b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 125)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Id Mark</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Age </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Length </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Width</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Height </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Weight </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Cross Sectional Area</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Density </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Load </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Comp. Strength </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Avg. Comp. Strength </b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 126)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Name Of the Test</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Result </b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Specified Limit </b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Unit</b></font></td>";
        //            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Method of Testing </b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 127)
        //        {
        //            mySql += "<td width= 2% align=center rowspan=2 valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 5% align=center  rowspan=2 valign=medium height=19 ><font size=2><b>Id Mark</b></font></td>";
        //            mySql += "<td width= 5% align=center colspan=3 valign=medium height=19 ><font size=2><b>Dimensions</b></font></td>";
        //            mySql += "<td width= 2% align=center  rowspan=2 valign=medium height=19 ><font size=2><b>Weight </br>   (kg) </b></font></td>";
        //            mySql += "<td width= 2% align=center  rowspan=2 valign=medium height=19 ><font size=2><b>Cross Sectional Area (mm<sup>2</sup>)</b></font></td>";
        //            mySql += "<td width= 2% align=center  rowspan=2 valign=medium height=19 ><font size=2><b>Density   (kg/Cum) </b></font></td>";
        //            mySql += "<td width= 2% align=center  rowspan=2 valign=medium height=19 ><font size=2><b>Load   (KN) </b></font></td>";
        //            mySql += "<td width= 2% align=center  rowspan=2 valign=medium height=19 ><font size=2><b>Comp. Strength (Kg/cm <sup>2</sup>) </b></font></td>";
        //            mySql += "<td width= 2% align=center  rowspan=2 valign=medium height=19 ><font size=2><b>Test </b></font></td>";
        //            mySql += "</tr> ";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Length </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Breadth</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Height </b></font></td>";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 128)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Id Mark</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Age</b></font></td>";
        //            mySql += "<td width= 2% colspan=3 align=center valign=medium height=19 ><font size=2><b>Size of Specimen (mm) </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Weight  </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Cross Sectional Area </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Density </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Load  </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Comp. Strength </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Avg. Comp. Strength </b></font></td>";

        //            mySql += "<tr> ";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2></font></td>";
        //            mySql += "<td  width= 2% align=center  valign=top height=19 ><font size=2><b>" + " Day" + "</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + " Length" + " </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + " Breadth " + "</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + " Height " + "</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "(Kg )" + "</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "(mm <sup>2</sup>)" + "</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "(kg/m <sup>3</sup>)" + "</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "(kN)" + "</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "(N/mm <sup>2</sup> )" + "</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "(N/mm <sup>2</sup>)" + "</b></font></td>";
        //            mySql += "</tr>";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 129)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Id Mark</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Name of the Test </b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Observation/Remark</b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 130)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Sample ID</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Avg Thk Of coating(micron) </b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 131)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Id Mark</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Dia Of Bar </b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>	Ultimate Load</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Ultimate Tensile </b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 132)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Id Mark</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Cross Sectional Area </b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>	Elongation</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Yield Stress </b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Ultimate Tensile Stress </b></font></td>";

        //            mySql += "<tr> ";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2></font></td>";
        //            mySql += "<td  width= 2% align=center  valign=top height=19 ><font size=2><b>" + "Test Specimen mm <sup>2</sup>" + "</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + " % " + " </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "N/mm <sup>2</sup>" + "</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>" + "N/mm <sup>2</sup>" + "</b></font></td>";
        //            mySql += "</tr>";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 133)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Dia Of Bar</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Carbon</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Manganese </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sulphur </b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Phosphorous</b></font></td>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Silicon</b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 134)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Test Parameters</b></font></td>";
        //            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>Unit</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Results </b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Requirements As Per IS 15388:2003 </b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Compliance</b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 135)
        //        {
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b>Sr No.</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Id Mark</b></font></td>";
        //            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Name Of the Test</b></font></td>";
        //            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>Unit </b></font></td>";
        //            mySql += "<td width= 15% align=center valign=medium height=19 ><font size=2><b>Result Obtained </b></font></td>";
        //            mySql += "</tr> ";
        //        }
        //        else if (OT.OTINWD_ReportForTestId_int == 139)
        //        {

        //            var OTtest = dc.AllInwdDetails_View(Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, 0, 0, 0, 0, "", 0, "OT");
        //            foreach (var t in OTtest)
        //            {
        //                string[] lines = t.OTDETAIL_DetailTest_var.Split('!');
        //                foreach (string line in lines)
        //                {
        //                    if (line != "")
        //                    {
        //                        if (SrNo == 0)
        //                        {
        //                            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b> Sr No.</b></font></td>";
        //                        }
        //                        else
        //                        {
        //                            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2>" + SrNo + " </font></td>";
        //                        }
        //                        SrNo++;
        //                        string[] line3 = line.Split('~');
        //                        foreach (string resline in line3)
        //                        {
        //                            if (resline != "")
        //                            {
        //                                if (SrNo == 1)
        //                                {
        //                                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>&nbsp;" + resline.ToString() + "</b></font></td>";
        //                                }
        //                                else
        //                                {
        //                                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + resline.ToString() + "</font></td>";
        //                                }
        //                            }
        //                        }
        //                    }
        //                    mySql += "</tr> ";
        //                }
        //            }
        //            SrNo = 0;
        //        }
        //        //  mySql += "</tr>";
        //        mySql += "<tr>";
        //        SrNo = 0;
        //        if (OT.OTINWD_ReportForTestId_int != 139)
        //        {
        //            foreach (string line2 in Line1)
        //            {
        //                int k = 0;
        //                k++;
        //                if (line2 != "" && OT.OTINWD_ReportForTestId_int != 114 && OT.OTINWD_ReportForTestId_int != 122 && OT.OTINWD_ReportForTestId_int != 133)
        //                {
        //                    SrNo++;
        //                    mySql += "<td width= 1% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //                }

        //                string[] lines = line2.Split('~');
        //                foreach (string line in lines)
        //                {
        //                    if (line != "")
        //                    {
        //                        if (OT.OTINWD_ReportForTestId_int == 114)
        //                        {
        //                            if (line.ToString() == "!")
        //                            {
        //                                k = 0;
        //                                if (k == 0)
        //                                {
        //                                    mySql += "<td width= 5% colspan=3 align=center valign=medium height=19 ><font size=2><b>Total </b></font></td>";
        //                                    k++;
        //                                }
        //                                string lineex = OtherDeatils.Substring(OtherDeatils.LastIndexOf('!') + 1);
        //                                string[] lineexf = lineex.Split('~');
        //                                foreach (string linefd in lineexf)
        //                                {
        //                                    if (linefd != "")
        //                                    {
        //                                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + linefd.ToString() + "</font></td>";
        //                                    }
        //                                }
        //                                break;
        //                            }
        //                            if (k == 1)
        //                            {
        //                                SrNo++;
        //                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //                            }
        //                            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + line.ToString() + "</font></td>";
        //                            k++;
        //                            if (k == 7)
        //                            {
        //                                k = 0;
        //                            }

        //                        }

        //                        else if (OT.OTINWD_ReportForTestId_int == 122)
        //                        {
        //                            if (line.ToString() == "!")
        //                            {
        //                                k = 0;
        //                                if (k == 0)
        //                                {
        //                                    mySql += "<td width= 5% colspan=3 align=center valign=medium height=19 ><font size=2><b>Total </b></font></td>";
        //                                    k++;
        //                                }
        //                                string lineex = OtherDeatils.Substring(OtherDeatils.LastIndexOf('!') + 1);
        //                                string[] lineexf = lineex.Split('~');
        //                                foreach (string linefd in lineexf)
        //                                {
        //                                    if (linefd != "")
        //                                    {

        //                                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + linefd.ToString() + "</font></td>";

        //                                    }
        //                                }
        //                                break;
        //                            }
        //                            if (k == 1)
        //                            {
        //                                SrNo++;
        //                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //                            }
        //                            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + line.ToString() + "</font></td>";
        //                            k++;
        //                            if (k == 7)
        //                            {
        //                                k = 0;
        //                            }
        //                        }

        //                        else if (OT.OTINWD_ReportForTestId_int == 133)
        //                        {
        //                            if (line.ToString() == "!")
        //                            {
        //                                k = 0;
        //                                if (k == 0)
        //                                {
        //                                    mySql += "<td width= 5% colspan=2 align=center valign=medium height=19 ><font size=2><b>Total </b></font></td>";
        //                                    k++;
        //                                }
        //                                string lineex = OtherDeatils.Substring(OtherDeatils.LastIndexOf('!') + 1);
        //                                string[] lineexf = lineex.Split('~');
        //                                foreach (string linefd in lineexf)
        //                                {
        //                                    if (linefd != "")
        //                                    {
        //                                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + linefd.ToString() + "</font></td>";
        //                                    }
        //                                }
        //                                break;
        //                            }
        //                            if (k == 1)
        //                            {
        //                                SrNo++;
        //                                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
        //                            }
        //                            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + line.ToString() + "</font></td>";
        //                            k++;
        //                            if (k == 6)
        //                            {
        //                                k = 0;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + line.ToString() + "</font></td>";
        //                        }
        //                    }
        //                }

        //                mySql += "</tr>";
        //            }
        //        }
        //    }
        //    i++;

        //    mySql += "<table>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";


        //    SrNo = 0;
        //    var matid = dc.Material_View("OT", "");
        //    foreach (var m in matid)
        //    {
        //        var iscd = dc.AllInwdDetails_View("", "", 0, "", 0, null, "", "", 0, false, false, false, 0, "", 0, m.MATERIAL_Id, 0, 0, 0, "", 0, "OT");
        //        foreach (var cd in iscd)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2><b>&nbsp;" + "References/Notes :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width=30% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + " )" + " " + cd.Isc_Description_var.ToString() + "(Fourth Revision)" + "</font></td>";
        //            mySql += "</tr>";
        //        }
        //    }


        //    SrNo = 0;
        //    var re = dc.AllRemark_View("", Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, "OT");
        //    foreach (var r in re)
        //    {
        //        var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.OTDetail_RemarkId_int), "OT");
        //        foreach (var remk in remark)
        //        {
        //            if (SrNo == 0)
        //            {
        //                mySql += "<tr>";
        //                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2> <b>&nbsp;" + "Remarks :" + "</b></font></td>";
        //                mySql += "</tr>";
        //            }
        //            SrNo++;
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + SrNo + ")" + remk.OT_Remark_var.ToString() + "</font></td>";
        //            mySql += "</tr>";

        //        }
        //    }

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + " Authorised Signatory" + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";
        //    if (System.Web.HttpContext.Current.Session["Check"] != null)
        //    {
        //        var RecNo = dc.ReportStatus_View("Other Testing", null, null, 1, 0, 0, Convert.ToString(System.Web.HttpContext.Current.Session["ReferenceNo"]), 0, 0, 0);
        //        foreach (var r in RecNo)
        //        {
        //            if (r.OTINWD_ApprovedBy_tint != null && r.OTINWD_CheckedBy_tint != null)
        //            {
        //                var U = dc.User_View(r.OTINWD_ApprovedBy_tint, -1, "", "", "");
        //                foreach (var r1 in U)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=1>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //                var lgin = dc.User_View(r.OTINWD_CheckedBy_tint, -1, "", "", "");
        //                foreach (var loginusr in lgin)
        //                {
        //                    mySql += "<tr>";
        //                    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>";
        //                    mySql += "</tr>";
        //                }
        //            }
        //        }
        //    }
        //    mySql += "<tr>";
        //    mySql += "<td width= 60% align=center valign=top height=19 ><font size=1>&nbsp;" + "---End Of Report--- " + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "Page of 1 of 1 " + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "CIN - U28939PNI999PTC014212" + "</font></td>";
        //    mySql += "</tr>";
        //    mySql += "<tr>";
        //    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Regd. Add - 1160/5,Gharpure Colony Shivaji Nagar, Pune 411005,Maharastra India." + "</font></td>";
        //    mySql += "</tr>";

        //    mySql += "<tr>";
        //    mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
        //    mySql += "</tr>";

        //    mySql += "</table>";


        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;

        //}

        #endregion

        public void RptHTMLgrid(string Header, string SubHeader, string Grddata)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> " + Header + " </b></font></td></tr>";
            int Srno = 0;
            string[] heading = SubHeader.Split('|');
            foreach (var head in heading)
            {
                if (Srno == 0)
                {
                    mySql += "<td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> " + head + " </b></font></td></tr>";
                    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
                    Srno++;
                }
                else if (Srno == 1)
                {
                    mySql += "<tr>";
                    mySql += "<td width='15%' align=left valign=top height=19><font size=2><b> " + head + " </b></font></td>";
                    Srno++;
                }
                else if (Srno == 2)
                {
                    mySql += "<td width='2%' height=19><font size=2>:</font></td>";
                    mySql += "<td width='40%' height=19><font size=2>" + head + " </font></td>";
                    mySql += "</tr>";
                    Srno = 1;
                }
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            Srno = 0;
            mySql += "<tr>";
            string[] grd = Grddata.Split('|');
            foreach (var header in grd)
            {
                Srno = Srno + 1;
                if (header != "")
                {
                    if (Srno != grd.Length)
                    {
                        mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> " + header.ToString() + " </b></font></td>";
                    }
                }
            }
            mySql += "</tr>";
            Srno = 0;
            string[] grddt = Grddata.Split('$');
            foreach (var d in grddt)
            {
                mySql += "<tr>";
                string[] dt = d.Split('~');
                foreach (var data in dt)
                {
                    if (Srno != 0)
                    {
                        if (data != "")
                        {
                            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>" + data.ToString() + "</font></td>";
                        }
                    }
                    Srno = Srno + 1;
                }
                mySql += "</tr>";
            }
            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            //return reportStr = mySql;
            DownloadHtmlReport("Grid_" + Header.Replace(" ", ""), mySql);
            //System.Web.HttpContext.Current.Response.Redirect("http://docs.google.com/viewer?url=" + mySql);
        }

        public void RptHTMLgrid_ClientSiteStatus(string Header, string SubHeader, string Grddata)
        {
            string mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> " + Header + " </b></font></td></tr>";
            int Srno = 0;

            mySql += "<td width='99%' colspan=7 align=left valign=top height=19><font size=2><b> " + SubHeader + " </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            Srno = 0;
            mySql += "<tr>";
            string[] grd = Grddata.Split('|');
            foreach (var header in grd)
            {
                Srno = Srno + 1;
                if (header != "")
                {   
                    if (Srno == 1)
                        mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> " + header.ToString() + " </b></font></td>";
                    else if (Srno == 2)
                        mySql += "<td width= 30% align=center valign=medium height=19 ><font size=2><b> " + header.ToString() + " </b></font></td>";
                    else if (Srno == 3)
                        mySql += "<td width= 50% align=center valign=medium height=19 ><font size=2><b> " + header.ToString() + " </b></font></td>";
                    else if (Srno == 4)
                        mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> " + header.ToString() + " </b></font></td>";
                }
            }
            mySql += "</tr>";
            Srno = 0;
            string[] grddt = Grddata.Split('$');
            foreach (var d in grddt)
            {
                mySql += "<tr>";
                string[] dt = d.Split('~');
                if (Srno != 0)
                {
                    mySql += "<td align=center valign=top height=19 ><font size=2>" + dt[0] + "</font></td>";
                    mySql += "<td align=left valign=top height=19 ><font size=2>" + dt[1] + "</font></td>";
                    mySql += "<td align=left valign=top height=19 ><font size=2>" + dt[2] + "</font></td>";
                    mySql += "<td align=left valign=top height=19 ><font size=2>" + dt[3] + "</font></td>";
                }
                Srno = Srno + 1;
                mySql += "</tr>";
            }
            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            DownloadHtmlReport("Grid_" + Header.Replace(" ", ""), mySql);
        }


        public void OtherReport_Template(string ReferenceNo, string chkStatus, string signVal, string signPath)
        {
            //bool RemotelyAppv = false;
            string filename = "OT-" + ReferenceNo.Replace("/", "_");
            StringBuilder mySql = new StringBuilder();
            string tempSql = "";
            //mySql.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>".ToString());
            mySql.Append("<html >".ToString());//xmlns='http://www.w3.org/1999/xhtml'
            mySql.Append("<head>".ToString());//form{ margin: 0; padding: 1em 0; border: 1px dotted red;}
            mySql.Append("<style type='text/css'>  TD{font-family: Square721 BT; } ".ToString());
            mySql.Append("body {margin-left:2em margin-right:2em  margin-top:0.5em}".ToString());
            mySql.Append("</style>".ToString());
            mySql.Append("</head>".ToString());

            mySql.Append("<table border=0 width=100% cellpadding=0 cellspacing=0 >".ToString());
            mySql.Append("<tr><td width= 60%><font size=3 >Durocrete Engineering Services Pvt.Ltd.</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
            if (cnStr.ToLower().Contains("mumbai") == true)
            {
                mySql.Append("<tr><td width= 60%><font size=1 >PAP-D122/125,TTC Industrial Area,Behind Jai Mata Di Weighbridge,</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Turbhe,Navi Mumbai-400705.</font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=3>Test with the Best</font></td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Tel No: +91-9850500013,022-27622353</font></td><td width= 10%>&nbsp</td><td width= 30% >&nbsp</td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Email: infomumbai@durocrete.acts-int.com</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 ><b> </b></font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=1>Website : www.durocrete.in</font></td></tr>".ToString());
                mySql.Append("</table>".ToString());
            }
            else if (cnStr.ToLower().Contains("nashik") == true)
            {
                mySql.Append("<tr><td width= 60%><font size=1 >Sunil Towers,Behind KK Travels,Dwarka,Nashik-422001.</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Tel No: +91-9527005478,7720006754</font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=3>Test with the Best</font></td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Email: infonashik@durocrete.acts-int.com</font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=1>Website : www.durocrete.in</font></td></tr>".ToString());
                //mySql.Append("<tr><td width= 60%><font size=1 ><b></b></font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=1>Website : www.durocrete.in</font></td></tr>".ToString());
                mySql.Append("</table>".ToString());

            }
            else
            {
                mySql.Append("<tr><td width= 60%><font size=1 >19/1,Hingane Khurd,Vitthalwadi,Sinhgad Road,Pune-411051.</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Tel No: +91-9881735302,020-24345170,24348027</font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=3>Test with the Best</font></td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Email: info@durocrete.acts-int.com</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 ><b></b></font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=1>Website : www.durocrete.in</font></td></tr>".ToString());
                mySql.Append("</table>".ToString());
            }
            //mySql.Append("<tr><td width= 60%><font size=3 >Durocrete Engineering Services Pvt.Ltd.</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
            //mySql.Append("<tr><td width= 60%><font size=1 >19/1,Hingane Khurd,Vitthalwadi,Sinhgad Road,Pune-411051.</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
            //mySql.Append("<tr><td width= 60%><font size=1 >Tel No: +91-9881735302,020-24345170,24348027</font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=3>Test with the Best</font></td></tr>".ToString());
            //mySql.Append("<tr><td width= 60%><font size=1 >Email: info@durocrete.acts-int.com</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
            //mySql.Append("<tr><td width= 60%><font size=1 ><b>An ISO/IEC 17025:2017 NABL Accredited Laboratory</b></font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=1>Website : www.durocrete.in</font></td></tr>".ToString());
            //mySql.Append("</table>".ToString());


            mySql.Append("<table border=0 cellpadding=0 cellspacing=0  width=100% id=AutoNumber1 >".ToString());
            //  mySql.Append("<tr><td border: none width='99%' colspan=7 align=center valign=top height=30><img height=60 width='99%' src=" + imageURL + "></td></tr>".ToString());//<img src =""" + imgPath + @""" width =""300"" height =""300""/>

            //mySql.Append("<tr><td border: none width='99%' colspan=7 align=center valign=top height=30><img height=60 width='99%' src=\"" + MakeImageSrcData(@"c:\testLogoPune.png") + "\"></td></tr>".ToString());
            mySql.Append("<tr><td border: none width='99%' colspan=7>&nbsp;</td></tr>".ToString());
            mySql.Append("<tr><td border: none width='99%' colspan=7 align=center valign=top height=19><font size=4 face=Times New Roman><b>Other Report</b></font></td></tr>".ToString());

            mySql.Append("<tr><td border: none width='99%' colspan=7>&nbsp;</td></tr>".ToString());

            int  SiteRouteId = 0;
          
            string authCode = reportAuthenticateCode();
            clsData obj = new clsData();
            string RouteName = obj.getRouteName(SiteRouteId);
            if (RouteName == "")
                RouteName = "NA";

            mySql.Append("<tr>" +
              "<td  border: none width='20%' align=left valign=top height=19><font size=2 face=Times New Roman><b>Authentication Code</b></font></td>" +
              "<td  border: none width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
              "<td border: none  width='40%' height=19><font size=2 face=Times New Roman>" + authCode + "</font></td>" +
              "<td  border: none height=19><font size=2 face=Times New Roman><b>Route</b></font></td>" +
              "<td  border: none width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
              "<td border: none  height=19><font size=2 face=Times New Roman>" + RouteName + "</font></td>" +
              "</tr>".ToString());


            int Approveby = 0;
            var OtherInwad = dc.ReportStatus_View("Other Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);
            foreach (var Odetails in OtherInwad)
            {

                if (Odetails.OTINWD_ApprovedBy_tint != null)
                {
                    Approveby = Convert.ToInt32(Odetails.OTINWD_ApprovedBy_tint);
                }

                //if (Odetails.OTINWD_RemoteApproved == true && Odetails.OTINWD_RemoteApproved != null)
                //{
                //    RemotelyAppv = true;
                //}
                mySql.Append("<tr>" +
               "<td  border: none width='20%' align=left valign=top height=19><font size=2 face=Times New Roman><b>Customer Name</b></font></td>" +
               "<td  border: none width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
               "<td border: none  width='40%' height=19><font size=2 face=Times New Roman>" + Odetails.CL_Name_var + "</font></td>" +
               "<td  border: none height=19><font size=2 face=Times New Roman><b>Date Of Issue</b></font></td>" +
               "<td  border: none width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
               "<td border: none  height=19><font size=2 face=Times New Roman>" + "-" + "</font></td>" +
               "</tr>".ToString());


                mySql.Append("<tr>" +
                    "<td align=left valign=top height=19><font size=2 face=Times New Roman><b> Office Address</b></font></td>" +
                    "<td height=19><font size=2 face=Times New Roman></font></td>" +
                   "<td width='40%' height=19><font size=2 face=Times New Roman>" + Odetails.CL_OfficeAddress_var + "</font></td>" +
                    "<td align=left valign=top height=19><font size=2 face=Times New Roman><b>Record No.</b></font></td>" +
                    "<td height=19><font size=2 face=Times New Roman>:</font></td>" +
                    "<td height=19><font size=2 face=Times New Roman>" + "OT" + "-" + Odetails.OTINWD_SetOfRecord_var.ToString() + "</font></td>" +
                    "</tr>".ToString());

                mySql.Append("<tr>" +

                "<td width='20%' height=19><font size=2 face=Times New Roman><b>Site Name</b></font></td>" +
                "<td width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
                "<td width='10%' height=19><font size=2 face=Times New Roman>" + Odetails.SITE_Name_var + "</font></td>" +
                "<td height=19><font size=2 face=Times New Roman><b>Sample Ref No.</b></font></td>" +
                "<td height=19><font size=2 face=Times New Roman>:</font></td>" +
                "<td height=19><font size=2 face=Times New Roman>" + "OT" + "-" + Odetails.OTINWD_ReferenceNo_var + "</font></td>" +
                "</tr>".ToString());

                mySql.Append("<tr>" +

                   "<td width='20%' height=19><font size=2 face=Times New Roman><b>Description</b></font></td>" +
                   "<td width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
                   "<td width='10%' height=19><font size=2 face=Times New Roman>" + Odetails.OTINWD_Description_var + "</font></td>" +
                   "<td height=19><font size=2 face=Times New Roman><b>Bill No.</b></font></td>" +
                   "<td height=19><font size=2 face=Times New Roman>:</font></td>" +
                   "<td height=19><font size=2 face=Times New Roman>" + Odetails.INWD_BILL_Id + "</font></td>" +
                   "</tr>".ToString());


                mySql.Append("<tr>" +
                   "<td align=left valign=top height=19><font size=2 face=Times New Roman><b>Supplier</b></font></td>" +
                   "<td width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
                   "<td height=19><font size=2 face=Times New Roman>" + Odetails.OTINWD_SupplierName_var.ToString() + "</font></td>" +
                   "<td align=left valign=top height=19><font size=2 face=Times New Roman><b>Date Of Receipt </b></font></td>" +
                   "<td height=19><font size=2 face=Times New Roman>:</font></td>" +
                   "<td height=19><font size=2 face=Times New Roman>" + Convert.ToDateTime(Odetails.INWD_ReceivedDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
                   "</tr>".ToString());

                mySql.Append("<tr>" +
                     "<td align=left valign=top height=19><font size=2 face=Times New Roman><b>Contact Details</b></font></td>" +
                   "<td width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
                   "<td height=19><font size=2 face=Times New Roman>" + Odetails.CONT_Name_var.ToString() + "-" + Odetails.CONT_ContactNo_var + "</font></td>" +
                   "<td align=left valign=top height=19><font size=2 face=Times New Roman><b>Date Of Testing</b></font></td>" +
                     "<td height=19><font size=2 face=Times New Roman>:</font></td>" +
                     "<td height=19><font size=2 face=Times New Roman>" + Convert.ToDateTime(Odetails.OTINWD_TestedDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
                     "</tr>".ToString());




            }
            mySql.Append("<tr><td width='99%'  colspan=7 align=left valign=top height=19 ><font size=2 face=Times New Roman>" + "&nbsp" + "</font></td></tr>".ToString());
            mySql.Append("<tr><td width='99%' colspan=7 align=left valign=top>".ToString());


            mySql.Append("<table border=0 width=100%>".ToString());
            mySql.Append("<tr>".ToString());
            mySql.Append("<td width=99% align=left valign=top height=19 ><font size=2 face=Times New Roman><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>".ToString());
            mySql.Append("</tr>".ToString());
            mySql.Append("</table>".ToString());

            mySql.Append("<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>".ToString());
            mySql.Append("<tr><td width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td width= 99% align=center valign=top height=19 ></td></tr>".ToString());

            int SrNo = 0;
            //int i = 0;


            mySql.Append("</table>".ToString());
            mySql.Append("<table border=0 width=100%>".ToString());
            mySql.Append("<tr>".ToString());
            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;</font></td>".ToString());
            mySql.Append("</tr>".ToString());
            mySql.Append("</table>".ToString());
            mySql.Append("<table border=0 width=100%>".ToString());
            mySql.Append("<tr>".ToString());
            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;</font></td>".ToString());
            mySql.Append("</tr>".ToString());


            SrNo = 0;
            var re = dc.AllRemark_View("", ReferenceNo, 0, "OT").ToList();
            foreach (var r in re)
            {
                if (r.OTDetail_RemarkType_var == "Reference")
                {
                    var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.OTDetail_RemarkId_int), "OT");
                    foreach (var remk in remark)
                    {
                        if (SrNo == 0)
                        {
                            mySql.Append("<tr>".ToString());
                            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman><b>&nbsp;" + "References:" + "</b></font></td>".ToString());
                            mySql.Append("</tr>".ToString());
                        }
                        SrNo++;
                        mySql.Append("<tr>".ToString());
                        mySql.Append("<td width=99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;" + SrNo + ")" + " " + remk.OT_Remark_var.ToString() + "</font></td>".ToString());
                        mySql.Append("</tr>".ToString());
                    }
                }

            }
            SrNo = 0;
            foreach (var r in re)
            {
                if (r.OTDetail_RemarkType_var == "Remark")
                {
                    var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.OTDetail_RemarkId_int), "OT");
                    foreach (var remk in remark)
                    {
                        if (SrNo == 0)
                        {
                            mySql.Append("<tr>".ToString());
                            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman> <b>&nbsp;" + "Remarks :" + "</b></font></td>".ToString());
                            mySql.Append("</tr>".ToString());
                        }
                        SrNo++;
                        mySql.Append("<tr>".ToString());
                        mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;" + SrNo + ")" + remk.OT_Remark_var.ToString() + "</font></td>".ToString());
                        mySql.Append("</tr>".ToString());
                    }
                }
            }
           
            mySql.Append("<tr>".ToString());
            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman><b>&nbsp;" + "Notes:" + "</b></font></td>".ToString());
            mySql.Append("</tr>".ToString());

            mySql.Append("<tr>".ToString());
            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;1)The test reports and results relate to the perticular specimen/sample(s) of the material as delivered/received and tested in the laboratory.</font></td>".ToString());
            mySql.Append("</tr>".ToString());

            mySql.Append("<tr>".ToString());
            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;2)Any test report shall not be reproduced except in full,without the written permission from Durocrete.</font></td>".ToString());
            mySql.Append("</tr>".ToString());

            mySql.Append("<tr>".ToString());
            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;3)Any alteration/change in the originally printed and issued report, would render this report as INVALID.</font></td>".ToString());
            mySql.Append("</tr>".ToString());

            mySql.Append("<tr>".ToString());
            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=3 face=Times New Roman><b>&nbsp;This report can be authenticated on our website www.durocrete.in</b></font></td>".ToString());
            mySql.Append("</tr>".ToString());

            mySql.Append("<tr>".ToString());
            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;</font></td>".ToString());
            mySql.Append("</tr>".ToString());

            #region Signature
            //bool sign = false;
            string userName = "", userDesg = "";
            if (signVal != "")
            {
                var User = dc.User_View(Convert.ToInt32(signVal), -1, "", "", "");
                foreach (var r1 in User)
                {
                    userName = r1.USER_Name_var.ToString();
                    userDesg = r1.USER_Designation_var.ToString();

                    break;
                }
            }

            mySql.Append("<tr>".ToString());
            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>Authorized Signatory</font></td>".ToString());
            mySql.Append("</tr>".ToString());
            if (signPath != "")
            {
                mySql.Append("<tr>".ToString());
                mySql.Append("<td width= 99% align=left valign=top height=30><b><img height=60 width=50 src=" + signPath + "></b>".ToString());
                mySql.Append("</td></tr>".ToString());
            }
            else
            {
                mySql.Append("<tr>".ToString());
                mySql.Append("<td width= 99% align=left valign=top height=30>&nbsp;".ToString());
                mySql.Append("</td></tr>".ToString());

            }
            if (userName != "")
            {
                mySql.Append("<tr>".ToString());
                mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;" + userName + "</font></td>".ToString());
                mySql.Append("</tr>".ToString());
            }
            if (userDesg != "")
            {
                mySql.Append("<tr>".ToString());
                mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=1  face=Times New Roman>&nbsp;" + "(" + userDesg + ")" + "</font></td>".ToString());
                mySql.Append("</tr>".ToString());
            }


            #endregion
            if (chkStatus == "Check")
            {
                var RecNo = dc.ReportStatus_View("Other Testing", null, null, 1, 0, 0, ReferenceNo, 0, 0, 0);
                foreach (var r in RecNo)
                {
                    if (r.OTINWD_ApprovedBy_tint != null && r.OTINWD_ApprovedBy_tint.ToString() != "" && r.OTINWD_ApprovedBy_tint > 0)
                    {
                        var U = dc.User_View(r.OTINWD_ApprovedBy_tint, -1, "", "", "");
                        foreach (var r1 in U)
                        {
                            mySql.Append("<tr>".ToString());
                            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2  face=Times New Roman>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>".ToString());
                            mySql.Append("</tr>".ToString());
                            mySql.Append("<tr>".ToString());
                            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=1  face=Times New Roman>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>".ToString());
                            mySql.Append("</tr>".ToString());

                            break;
                        }
                        if (r.OTINWD_CheckedBy_tint != null && r.OTINWD_CheckedBy_tint.ToString() != "" && r.OTINWD_CheckedBy_tint > 0)
                        {
                            var lgin = dc.User_View(r.OTINWD_CheckedBy_tint, -1, "", "", "");
                            foreach (var loginusr in lgin)
                            {
                                mySql.Append("<tr>".ToString());
                                mySql.Append("<td width= 99% align=right valign=top height=19 ><font size=2  face=Times New Roman>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>".ToString());
                                mySql.Append("</tr>".ToString());
                            }
                        }
                    }
                }
            }


            mySql.Append("</table>".ToString());

            mySql.Append("</td>".ToString());
            mySql.Append("</tr>".ToString());
            mySql.Append(tempSql);
            mySql.Append("<tr><td  colspan=7  width= 99% align=center valign=top height=19 ><font size=1  face=Times New Roman>&nbsp;" + "---End Of Report--- " + "</font></td></tr>".ToString());


            mySql.Append("<tr><td colspan=7  width= 99% align=center valign=bottom height=19 ><font size=2  face=Times New Roman>&nbsp;" + "Page 1 of 1" + "</font></td></tr>".ToString());
            // mySql.Append("<tr><td colspan=7 margin-bottom:0 padding-bottom:0 width= 99% align=left valign=bottom height=14 ><font size=1  face=Times New Roman>Regd. Office: 1160/5,Gharpure Colony, Shivaji Nagar,Pune 411005. Maharashtra,India. CIN: U28939PN1999PTC014212." + tollFree + "</font></td></tr>".ToString());


            mySql.Append("</table>".ToString());
            mySql.Append("</html>".ToString());

            DownloadWordReport(filename, mySql.ToString());
       

        }
        public void DownloadWordReportTemplateOpenOffice(string fileName)
        {

            fileName = fileName + ".odt";
            string reportPath = "C:/temp/" + fileName;
            System.Web.HttpContext.Current.Response.ContentType = "application/writer";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            System.Web.HttpContext.Current.Response.TransmitFile(reportPath);
            System.Web.HttpContext.Current.Response.End();
        }

        public void DownloadWordReport(string fileName, string reportStr)
        {
            //fileName = fileName + "_" + System.Web.HttpContext.Current.Session["LoginId"] + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".doc";
            fileName = fileName + ".doc";
            string reportPath = "C:/temp/" + fileName;

            StreamWriter sw;
            sw = File.CreateText(reportPath);
            sw.WriteLine(reportStr);
            sw.Close();
            System.Web.HttpContext.Current.Response.ContentType = "application/msword";
            // System.Web.HttpContext.Current.Response.ContentType = "text/html";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            System.Web.HttpContext.Current.Response.TransmitFile(reportPath);
            System.Web.HttpContext.Current.Response.End();
        }
        public void OtherReport_TemplateExcel(string ReferenceNo, string chkStatus, string signVal, string signPath)
        {
            //bool RemotelyAppv = false;
            string filename = "OT-" + ReferenceNo.Replace("/", "_");
            StringBuilder mySql = new StringBuilder();
            string tempSql = "";
            //mySql.Append("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>".ToString());
            mySql.Append("<html >".ToString());//xmlns='http://www.w3.org/1999/xhtml'
            mySql.Append("<head>".ToString());//form{ margin: 0; padding: 1em 0; border: 1px dotted red;}
            mySql.Append("<style type='text/css'>  TD{font-family: Square721 BT; } ".ToString());
            mySql.Append("body {margin-left:2em margin-right:2em  margin-top:0.5em}".ToString());
            mySql.Append("</style>".ToString());
            mySql.Append("</head>".ToString());

            mySql.Append("<table border=0 width=100% cellpadding=0 cellspacing=0 >".ToString());
            mySql.Append("<tr><td width= 60% colspan=2><font size=3 >Durocrete Engineering Services Pvt.Ltd.</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
            if (cnStr.ToLower().Contains("mumbai") == true)
            {
                mySql.Append("<tr><td width= 60%><font size=1 >PAP-D122/125,TTC Industrial Area,Behind Jai Mata Di Weighbridge,</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Turbhe,Navi Mumbai-400705.</font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=3>Test with the Best</font></td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Tel No: +91-9850500013,022-27622353</font></td><td width= 10%>&nbsp</td><td width= 30% >&nbsp</td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Email: infomumbai@durocrete.acts-int.com</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 ><b></b></font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=1>Website : www.durocrete.in</font></td></tr>".ToString());
                mySql.Append("</table>".ToString());
            }
            else if (cnStr.ToLower().Contains("nashik") == true)
            {
                mySql.Append("<tr><td width= 60%><font size=1 >Sunil Towers,Behind KK Travels,Dwarka,Nashik-422001.</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Tel No: +91-9527005478,7720006754</font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=3>Test with the Best</font></td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Email: infonashik@durocrete.acts-int.com</font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=1>Website : www.durocrete.in</font></td></tr>".ToString());
               // mySql.Append("<tr><td width= 60%><font size=1 ><b></b></font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=1></font></td></tr>".ToString());
                mySql.Append("</table>".ToString());

            }
            else
            {
                mySql.Append("<tr><td width= 60%><font size=1 >19/1,Hingane Khurd,Vitthalwadi,Sinhgad Road,Pune-411051.</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Tel No: +91-9881735302,020-24345170,24348027</font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=3>Test with the Best</font></td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 >Email: info@durocrete.acts-int.com</font></td><td width= 10%>&nbsp</td><td width= 30%>&nbsp</td></tr>".ToString());
                mySql.Append("<tr><td width= 60%><font size=1 ><b></b></font></td><td width= 10%>&nbsp</td><td width= 30% align=right><font size=1>Website : www.durocrete.in</font></td></tr>".ToString());
                mySql.Append("</table>".ToString());
            }


            mySql.Append("<table border=0 cellpadding=0 cellspacing=0  width=100% id=AutoNumber1 >".ToString());
            //  mySql.Append("<tr><td border: none width='99%' colspan=7 align=center valign=top height=30><img height=60 width='99%' src=" + imageURL + "></td></tr>".ToString());//<img src =""" + imgPath + @""" width =""300"" height =""300""/>

            //mySql.Append("<tr><td border: none width='99%' colspan=7 align=center valign=top height=30><img height=60 width='99%' src=\"" + MakeImageSrcData(@"c:\testLogoPune.png") + "\"></td></tr>".ToString());
            mySql.Append("<tr><td border: none width='99%' colspan=7>&nbsp;</td></tr>".ToString());
            mySql.Append("<tr><td border: none width='99%' colspan=7 align=center valign=top height=19><font size=4 face=Times New Roman><b>Other Report</b></font></td></tr>".ToString());

            mySql.Append("<tr><td border: none width='99%' colspan=7>&nbsp;</td></tr>".ToString());

            int Approveby = 0;
            var OtherInwad = dc.ReportStatus_View("Other Testing", null, null, 0, 0, 0, ReferenceNo, 0, 2, 0);
            foreach (var Odetails in OtherInwad)
            {

                if (Odetails.OTINWD_ApprovedBy_tint != null)
                {
                    Approveby = Convert.ToInt32(Odetails.OTINWD_ApprovedBy_tint);
                }

                //if (Odetails.OTINWD_RemoteApproved == true && Odetails.OTINWD_RemoteApproved != null)
                //{
                //    RemotelyAppv = true;
                //}
                mySql.Append("<tr>" +
               "<td  border: none width='20%' align=left valign=top height=19><font size=2 face=Times New Roman><b>Customer Name : </b></font></td>" +
               //"<td  border: none width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
               "<td border: none  width='40%' height=19><font size=2 face=Times New Roman>" + Odetails.CL_Name_var + "</font></td>" +
             "<td  border: none width='20%' height=19><font size=2 face=Times New Roman>&nbsp;</font></td>" +
               "<td  border: none height=19><font size=2 face=Times New Roman><b>Date Of Issue : </b></font></td>" +
              // "<td  border: none width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
               "<td border: none  height=19><font size=2 face=Times New Roman>" + "-" + "</font></td>" +
               "</tr>".ToString());


                mySql.Append("<tr>" +
                    "<td align=left valign=top height=19><font size=2 face=Times New Roman><b> Office Address : </b></font></td>" +
                 //   "<td height=19><font size=2 face=Times New Roman></font></td>" +
                   "<td width='40%' height=19><font size=2 face=Times New Roman>" + Odetails.CL_OfficeAddress_var + "</font></td>" +
                   "<td  border: none width='20%' height=19><font size=2 face=Times New Roman>&nbsp;</font></td>" +
              "<td align=left valign=top height=19><font size=2 face=Times New Roman><b>Record No. : </b></font></td>" +
                   // "<td height=19><font size=2 face=Times New Roman>:</font></td>" +
                    "<td height=19><font size=2 face=Times New Roman>" + "OT" + "-" + Odetails.OTINWD_SetOfRecord_var.ToString() + "</font></td>" +
                    "</tr>".ToString());

                mySql.Append("<tr>" +

                "<td width='20%' height=19><font size=2 face=Times New Roman><b>Site Name : </b></font></td>" +
              //  "<td width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
                "<td width='10%' height=19><font size=2 face=Times New Roman>" + Odetails.SITE_Name_var + "</font></td>" +
                "<td  border: none width='20%' height=19><font size=2 face=Times New Roman>&nbsp;</font></td>" +
             "<td height=19><font size=2 face=Times New Roman><b>Sample Ref No. : </b></font></td>" +
            //    "<td height=19><font size=2 face=Times New Roman>:</font></td>" +
                "<td height=19><font size=2 face=Times New Roman>" + "OT" + "-" + Odetails.OTINWD_ReferenceNo_var + "</font></td>" +
                "</tr>".ToString());

                mySql.Append("<tr>" +

                   "<td width='20%' height=19><font size=2 face=Times New Roman><b>Description : </b></font></td>" +
            //       "<td width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
                   "<td width='10%' height=19><font size=2 face=Times New Roman>" + Odetails.OTINWD_Description_var + "</font></td>" +
                "<td  border: none width='20%' height=19><font size=2 face=Times New Roman>&nbsp;</font></td>" +
                "<td height=19><font size=2 face=Times New Roman><b>Bill No. : </b></font></td>" +
             //      "<td height=19><font size=2 face=Times New Roman>:</font></td>" +
                   "<td height=19><font size=2 face=Times New Roman>" + Odetails.INWD_BILL_Id + "</font></td>" +
                   "</tr>".ToString());


                mySql.Append("<tr>" +
                   "<td align=left valign=top height=19><font size=2 face=Times New Roman><b>Supplier : </b></font></td>" +
               //    "<td width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
                   "<td height=19><font size=2 face=Times New Roman>" + Odetails.OTINWD_SupplierName_var.ToString() + "</font></td>" +
                   "<td  border: none width='20%' height=19><font size=2 face=Times New Roman>&nbsp;</font></td>" +
             "<td align=left valign=top height=19><font size=2 face=Times New Roman><b>Date Of Receipt : </b></font></td>" +
               //    "<td height=19><font size=2 face=Times New Roman>:</font></td>" +
                   "<td height=19 align=left><font size=2 face=Times New Roman>" + Convert.ToDateTime(Odetails.INWD_ReceivedDate_dt).ToString("dd/MMM/yy") + "</font></td>" +
                   "</tr>".ToString());

                mySql.Append("<tr>" +
                     "<td align=left valign=top height=19><font size=2 face=Times New Roman><b>Contact Details : </b></font></td>" +
                //   "<td width='2%' height=19><font size=2 face=Times New Roman>:</font></td>" +
                   "<td height=19><font size=2 face=Times New Roman>" + Odetails.CONT_Name_var.ToString() + "-" + Odetails.CONT_ContactNo_var + "</font></td>" +
                  "<td  border: none width='20%' height=19><font size=2 face=Times New Roman>&nbsp;</font></td>" +
              "<td align=left valign=top height=19><font size=2 face=Times New Roman><b>Date Of Testing : </b></font></td>" +
                //     "<td height=19><font size=2 face=Times New Roman>:</font></td>" +
                     "<td height=19><font size=2 face=Times New Roman>" + Convert.ToDateTime(Odetails.OTINWD_TestedDate_dt).ToString("dd/MMM/yyyy") + "</font></td>" +
                     "</tr>".ToString());




            }
            mySql.Append("<tr><td width='99%'  colspan=7 align=left valign=top height=19 ><font size=2 face=Times New Roman>" + "&nbsp" + "</font></td></tr>".ToString());
            mySql.Append("<tr><td width='99%' colspan=7 align=left valign=top>".ToString());


            //mySql.Append("<table border=0 width=100%>".ToString());
            mySql.Append("<tr>".ToString());
            mySql.Append("<td colspan=7 width=99% align=left valign=top height=19 ><font size=2 face=Times New Roman><b>" + "OBSERVATIONS & CALCULATIONS: " + "</b></font></td>".ToString());
            mySql.Append("</tr>".ToString());
            //mySql.Append("</table>".ToString());

            mySql.Append("<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>".ToString());
            mySql.Append("<tr><td colspan=7  width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td colspan=7  width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td colspan=7  width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td colspan=7  width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td  colspan=7 width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td  colspan=7 width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td  colspan=7 width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td colspan=7  width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td colspan=7  width= 99% align=center valign=top height=19 ></td></tr>".ToString());
            mySql.Append("<tr><td colspan=7  width= 99% align=center valign=top height=19 ></td></tr>".ToString());

            int SrNo = 0;
            //int i = 0;


            mySql.Append("</table>".ToString());
            mySql.Append("<table border=0 width=100%>".ToString());
            mySql.Append("<tr>".ToString());
            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;</font></td>".ToString());
            mySql.Append("</tr>".ToString());
            mySql.Append("</table>".ToString());
            mySql.Append("<table border=0 width=100%>".ToString());
            mySql.Append("<tr>".ToString());
            mySql.Append("<td width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;</font></td>".ToString());
            mySql.Append("</tr>".ToString());


            SrNo = 0;
            var re = dc.AllRemark_View("", ReferenceNo, 0, "OT").ToList();
            foreach (var r in re)
            {
                if (r.OTDetail_RemarkType_var == "Reference")
                {
                    var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.OTDetail_RemarkId_int), "OT");
                    foreach (var remk in remark)
                    {
                        if (SrNo == 0)
                        {
                            mySql.Append("<tr>".ToString());
                            mySql.Append("<td  colspan=7 width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman><b>&nbsp;" + "References:" + "</b></font></td>".ToString());
                            mySql.Append("</tr>".ToString());
                        }
                        SrNo++;
                        mySql.Append("<tr>".ToString());
                        mySql.Append("<td  colspan=7 width=99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;" + SrNo + ")" + " " + remk.OT_Remark_var.ToString() + "</font></td>".ToString());
                        mySql.Append("</tr>".ToString());
                    }
                }

            }
            SrNo = 0;
            foreach (var r in re)
            {
                if (r.OTDetail_RemarkType_var == "Remark")
                {
                    var remark = dc.AllRemark_View("", "", Convert.ToInt32(r.OTDetail_RemarkId_int), "OT");
                    foreach (var remk in remark)
                    {
                        if (SrNo == 0)
                        {
                            mySql.Append("<tr>".ToString());
                            mySql.Append("<td  colspan=7 width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman> <b>&nbsp;" + "Remarks :" + "</b></font></td>".ToString());
                            mySql.Append("</tr>".ToString());
                        }
                        SrNo++;
                        mySql.Append("<tr>".ToString());
                        mySql.Append("<td  colspan=7 width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;" + SrNo + ")" + remk.OT_Remark_var.ToString() + "</font></td>".ToString());
                        mySql.Append("</tr>".ToString());
                    }
                }
            }

            mySql.Append("<tr>".ToString());
            mySql.Append("<td colspan=7  width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman><b>&nbsp;" + "Notes:" + "</b></font></td>".ToString());
            mySql.Append("</tr>".ToString());

            mySql.Append("<tr>".ToString());
            mySql.Append("<td colspan=7  width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;1)The test reports and results relate to the perticular specimen/sample(s) of the material as delivered/received and tested in the laboratory.</font></td>".ToString());
            mySql.Append("</tr>".ToString());

            mySql.Append("<tr>".ToString());
            mySql.Append("<td colspan=7  width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;2)Any test report shall not be reproduced except in full,without the written permission from Durocrete.</font></td>".ToString());
            mySql.Append("</tr>".ToString());

            mySql.Append("<tr>".ToString());
            mySql.Append("<td colspan=7  width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;3)Any alteration/change in the originally printed and issued report, would render this report as INVALID.</font></td>".ToString());
            mySql.Append("</tr>".ToString());

            mySql.Append("<tr>".ToString());
            mySql.Append("<td  colspan=7 width= 99% align=left valign=top height=19 ><font size=3 face=Times New Roman><b>&nbsp;This report can be authenticated on our website www.durocrete.in</b></font></td>".ToString());
            mySql.Append("</tr>".ToString());

            mySql.Append("<tr>".ToString());
            mySql.Append("<td colspan=7  width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;</font></td>".ToString());
            mySql.Append("</tr>".ToString());

            #region Signature
            //bool sign = false;
            string userName = "", userDesg = "";
            if (signVal != "")
            {
                var User = dc.User_View(Convert.ToInt32(signVal), -1, "", "", "");
                foreach (var r1 in User)
                {
                    userName = r1.USER_Name_var.ToString();
                    userDesg = r1.USER_Designation_var.ToString();

                    break;
                }

            }
            mySql.Append("<tr>".ToString());
            mySql.Append("<td  colspan=2 width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>Authorized Signatory</font></td>".ToString());
            mySql.Append("</tr>".ToString());
            if (signPath != "")
            {
                mySql.Append("<tr>".ToString());
                mySql.Append("<td   colspan=2 width= 99% align=left valign=top height=30><b><img height=60 width=50 src=" + signPath + "></b>".ToString());
                mySql.Append("</td></tr>".ToString());
            }
            else
            {
                mySql.Append("<tr>".ToString());
                mySql.Append("<td  colspan=2 width= 99% align=left valign=top height=30>&nbsp;".ToString());
                mySql.Append("</td></tr>".ToString());

            }
            if (userName != "")
            {
                mySql.Append("<tr>".ToString());
                mySql.Append("<td   colspan=2 width= 99% align=left valign=top height=19 ><font size=2 face=Times New Roman>&nbsp;" + userName + "</font></td>".ToString());
                mySql.Append("</tr>".ToString());
            }
            if (userDesg != "")
            {
                mySql.Append("<tr>".ToString());
                mySql.Append("<td   colspan=2 width= 99% align=left valign=top height=19 ><font size=1  face=Times New Roman>&nbsp;" + "(" + userDesg + ")" + "</font></td>".ToString());
                mySql.Append("</tr>".ToString());
            }


            #endregion
            if (chkStatus == "Check")
            {
                var RecNo = dc.ReportStatus_View("Other Testing", null, null, 1, 0, 0, ReferenceNo, 0, 0, 0);
                foreach (var r in RecNo)
                {
                    if (r.OTINWD_ApprovedBy_tint != null && r.OTINWD_ApprovedBy_tint.ToString() != "" && r.OTINWD_ApprovedBy_tint > 0)
                    {
                        var U = dc.User_View(r.OTINWD_ApprovedBy_tint, -1, "", "", "");
                        foreach (var r1 in U)
                        {
                            mySql.Append("<tr>".ToString());
                            mySql.Append("<td  colspan=2 width= 99% align=left valign=top height=19 ><font size=2  face=Times New Roman>&nbsp;" + r1.USER_Name_var.ToString() + "</font></td>".ToString());
                            mySql.Append("</tr>".ToString());
                            mySql.Append("<tr>".ToString());
                            mySql.Append("<td  colspan=2 width= 99% align=left valign=top height=19 ><font size=1  face=Times New Roman>&nbsp;" + "(" + r1.USER_Designation_var.ToString() + ")" + "</font></td>".ToString());
                            mySql.Append("</tr>".ToString());

                            break;
                        }
                        if (r.OTINWD_CheckedBy_tint != null && r.OTINWD_CheckedBy_tint.ToString() != "" && r.OTINWD_CheckedBy_tint > 0)
                        {
                            var lgin = dc.User_View(r.OTINWD_CheckedBy_tint, -1, "", "", "");
                            foreach (var loginusr in lgin)
                            {
                                mySql.Append("<tr>".ToString());
                                mySql.Append("<td  colspan=2 width= 99% align=right valign=top height=19 ><font size=2  face=Times New Roman>&nbsp;" + "Checked By:" + loginusr.USER_Name_var.ToString() + "</font></td>".ToString());
                                mySql.Append("</tr>".ToString());
                            }
                        }
                    }
                }
            }


            mySql.Append("</table>".ToString());

            mySql.Append("</td>".ToString());
            mySql.Append("</tr>".ToString());
            mySql.Append(tempSql);
            mySql.Append("<tr><td  colspan=7  width= 99% align=center valign=top height=19 ><font size=1  face=Times New Roman>&nbsp;" + "---End Of Report--- " + "</font></td></tr>".ToString());


            mySql.Append("<tr><td colspan=7  width= 99% align=center valign=bottom height=19 ><font size=2  face=Times New Roman>&nbsp;" + "Page 1 of 1" + "</font></td></tr>".ToString());
            // mySql.Append("<tr><td colspan=7 margin-bottom:0 padding-bottom:0 width= 99% align=left valign=bottom height=14 ><font size=1  face=Times New Roman>Regd. Office: 1160/5,Gharpure Colony, Shivaji Nagar,Pune 411005. Maharashtra,India. CIN: U28939PN1999PTC014212." + tollFree + "</font></td></tr>".ToString());


            mySql.Append("</table>".ToString());
            mySql.Append("</html>".ToString());

           // DownloadWordReport(filename, mySql.ToString());
              DownloadExcelReport(filename, mySql.ToString());


            // DownloadWordReportTemplate("OtherReportOpen");


        }
   
        public void DownloadExcelReport(string fileName, string reportStr)
        {
            //fileName = fileName + "_" + System.Web.HttpContext.Current.Session["LoginId"] + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".doc";
            fileName = fileName + ".xls";
            string reportPath = "C:/temp/" + fileName;

            StreamWriter sw;
            sw = File.CreateText(reportPath);
            sw.WriteLine(reportStr);
            sw.Close();
          //  System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            System.Web.HttpContext.Current.Response.TransmitFile(reportPath);
            System.Web.HttpContext.Current.Response.End();
        }
    }

}




