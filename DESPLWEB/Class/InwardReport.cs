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

namespace DESPLWEB
{
    public class InwardReport
    {
       LabDataDataContext dc = new LabDataDataContext();
       public string getDetailReportAACInward(int RecordNo, int RefNo, bool prntLabsheet)
       {
           string reportStr = "", mySql = "", tempSql = "";
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
           mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
           mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
           if (prntLabsheet == true)
           {
               mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>AAC Block Lab Sheet </b></font></td></tr>";
           }
           else
           {
               mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>AAC Block Inward</b></font></td></tr>";
           }
           mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

           var n1 = dc.AllInward_View("AAC", RecordNo, "");
           int SampleNo = 0;
           foreach (var c1 in n1)
           {
               SampleNo++;
           }

           var b = dc.ModifyInward_View(RecordNo, RefNo, null, "AAC", null, null);

           foreach (var nt in b)
           {
               CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
               if (prntLabsheet == true)
               {
                   mySql += "<tr>" +
                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                      "<td width='1%' height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                      "</tr>";

                   mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                      "<td width='1%' height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + "AAC" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                      "</tr>";

                   mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + "AAC" + "-" + RefNo + "</font></td>" +
                      "</tr>";

                   mySql += "<tr>" +

                    "<td width='20%' align=left valign=top height=19> </td>" +
                    "<td width='45%' height=19> </td>" +
                    "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + nt.INWD_BILL_Id + "</font></td>" +
                    "</tr>";

                   mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                     "</tr>";

                   mySql += "<tr>" +

                   "<td width='20%' align=left valign=top height=19> </td>" +
                   "<td width='45%' height=19> </td>" +
                   "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                   "<td height=19><font size=2>:</font></td>" +
                   "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                   "</tr>";

                   mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                     "</tr>";
               }
               else
               {

                   mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                       "<td width='2%' height=19><font size=2>:</font></td>" +
                       "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                       "<td height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "AAC" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "<td height=19><font size=2>&nbsp;</font></td>" +
                       "</tr>";

                   mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "AAC" + "-" + RefNo + "</font></td>" +
                        "</tr>";

                   mySql += "<tr>" +

                       "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                       "<td width='2%' height=19><font size=2>:</font></td>" +
                       "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                       "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_BILL_Id + "</font></td>" +
                       "</tr>";

                   mySql += "<tr>" +

                      "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";



                   mySql += "<tr>" +
                       "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                       "<td height=19><font size=2></font></td>" +
                       "<td height=19><font size=2></font></td>" +
                       "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                       "</tr>";

                   mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                         "</tr>";

                   mySql += "<tr>" +

                    "<td align=left valign=top height=19><font size=2><b>Site Address</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                    "</tr>";

               }
           }
           mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
           mySql += "<tr><td colspan=6 align=left valign=top>";
           mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

           mySql += "<tr>";
           mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
           mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";
           mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";
           mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Test to be done</b></font></td>";
           mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
           mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
           mySql += "</tr>";


           int i = 0;
           var n = dc.AllInward_View("AAC", RecordNo, "");
           int SrNo = 0;
           foreach (var c in n)
           {
               SrNo++;
               mySql += "<tr>";
               mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
               mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.AACINWD_Id_Mark_var + "</font></td>";
               mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.AACINWD_Quantity_tint + "</font></td>";

               int TestId = 0;
               TestId = Convert.ToInt32(c.AACINWD_TEST_Id.ToString());

               var Testtype = dc.Test_View(0, Convert.ToInt32(c.AACINWD_TEST_Id.ToString()), "", 0, 0, 0);
               foreach (var t in Testtype)
               {
                   mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + t.TEST_Name_var.ToString() + "</font></td>";
                   break;
               }

               mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.AACINWD_Description_var.ToString() + "</font></td>";
               mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.AACINWD_SupplierName_var + "</font></td>";
               mySql += "</tr>";
               i++;
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
        public string getDetailReportAGGT(int RecordNo, int RefNo, bool prntLabsheet)
        {          
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Aggregate LabSheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Aggregate Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("AGGT", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "AGGT", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";
                    
                    mySql += "<tr>" +

                        "<td width='20%' align=left valign=top height=19> </td>" +
                        "<td width='45%' height=19> </td>" +
                        "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td width='1%' height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "AGGT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "AGGT" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";
                       
                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                     "</tr>";
                }
                else
                {
                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "AGGT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "AGGT" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2> DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Aggregate Name</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Testing to be done as per </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>IS Code</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";



            int i = 0;
            var n = dc.AllInward_View("AGGT", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.AGGTINWD_AggregateName_var.ToString() + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.AGGTINWD_Quantity_tint.ToString() + "</font></td>";


                mySql += "<td >";
                mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";

                string TestName = "";
                var chk = dc.AllInward_View("AGGT", 0, c.AGGTINWD_ReferenceNo_var.ToString());
                foreach (var ch in chk)
                {
                    var sp = dc.Test_View(0, Convert.ToInt32(ch.AGGTTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var ag in sp)
                    {
                        TestName = ag.TEST_Name_var.ToString();
                        mySql += "<tr>";
                        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + TestName + "</font></td>";
                        mySql += "</tr>";

                    }
                }

                mySql += "</table>";
                mySql += "</td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "IS:2386(PartI)-1963)" + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.AGGTINWD_Description_var.ToString() + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.AGGTINWD_SupplierName_var.ToString() + "</font></td>";
                mySql += "</tr>";
                i++;
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
        public string getDetailReportBrickInward(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Brick Test Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Brick Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("BT-", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }
           

            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "BT-", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "BT " + "- " + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "BT " + "- " + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +
                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                         "<td width='20%' align=left valign=top height=19> </td>" +
                          "<td width='45%' height=19> </td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
                else
                {

                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "BT" + "- " + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "BT" + "- " + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Brick Type</b></font></td>";

            mySql += "<td>";
            mySql += "<table  width=100% id=AutoNumber2>";
            mySql += "<tr>";
            mySql += "<td width= 30% align=center valign=top height=19 ><font size=2><b> Test to be Conducted </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b> Qty</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";
            mySql += "</td>";


            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("BT-", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.BTINWD_IdMark_var + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.BTINWD_BrickType_var.ToString() + "</font></td>";

                mySql += "<td > ";
                mySql += "<table border=0 cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";

                var wttest = dc.AllInward_View("BT-", 0, c.BTINWD_ReferenceNo_var.ToString());
                foreach (var wt in wttest)
                {
                    var c1 = dc.Test_View(0, Convert.ToInt32(wt.BTTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var n2 in c1)
                    {
                        mySql += "<tr>";
                        mySql += "<td>";//

                        mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
                        mySql += "<tr>";
                        mySql += "<td width= 30% align=left valign=top height=19 ><font size=2>&nbsp;" + n2.TEST_Name_var.ToString() + "</font></td>";

                        var c11 = dc.Test_View(0, Convert.ToInt32(wt.BTTEST_TEST_Id), "", 0, 0, 0);
                        foreach (var d1 in c11)
                        {
                            string TEST_Name_var = d1.TEST_Name_var.ToString();
                            var Rateint = dc.Test_View(0, Convert.ToInt32(wt.BTTEST_TEST_Id), "", 0, 0, 0);
                            foreach (var r in Rateint)
                            {
                                if (wt.BTTEST_Quantity_tint.ToString() != null && wt.BTTEST_Quantity_tint.ToString() != "")
                                {
                                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + wt.BTTEST_Quantity_tint.ToString() + "</font></td>";
                                }
                            }
                        }

                        mySql += "</tr>";
                        mySql += "</table>";
                        mySql += "</td>";
                        mySql += "</tr>";
                    }
                }
                mySql += "</table>";
                mySql += "</td>";


                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.BTINWD_SupplierName_var + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.BTINWD_Description_var + "</font></td>";
                mySql += "</tr>";
                i++;
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
        public string getDetailReportCementChemicalInwd(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Cement Chemical Test Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Cement Chemical Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("CCH", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }
         

            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "CCH", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CCH" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CCH" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +
                         "<td width='20%' align=left valign=top height=19> </td>" +
                         "<td width='45%' height=19> </td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
                else
                {

                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "CCH" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "CCH" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Cement Name</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Qty </b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Test </b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("CCH", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CCHINWD_CementName_var + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CCHINWD_Quantity_tint.ToString() + "</font></td>";



                mySql += "<td >";
                mySql += "<table border=1 cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";

                var wttest = dc.AllInward_View("CCH", 0, c.CCHINWD_ReferenceNo_var.ToString());
                foreach (var wt in wttest)
                {
                    var c1 = dc.Test_View(0, Convert.ToInt32(wt.CCHTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var n2 in c1)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + n2.TEST_Name_var.ToString() + "</font></td>";
                        mySql += "</tr>";
                    }
                }


                mySql += "</table>";
                mySql += "</td>";

                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CCHINWD_Description_var + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CCHINWD_SupplierName_var + "</font></td>";
                mySql += "</tr>";
                i++;
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
        public string getDetailReportCementInward(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Cement Test Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Cement Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "CEMT", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CEMT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CEMT" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +
                          "<td width='20%' align=left valign=top height=19> </td>" +
                          "<td width='45%' height=19> </td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
                else
                {


                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "CEMT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "CEMT" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }

            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Cement Name</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Qty </b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Test </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Test Done As Per</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";

            int i = 0;
            var n = dc.AllInward_View("CEMT", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CEMTINWD_CementName_var + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CEMTINWD_Quantity_tint.ToString() + "</font></td>";

                mySql += "<td >";
                mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
                string strIsCode = "";
                var wttest = dc.AllInward_View("CEMT", 0, c.CEMTINWD_ReferenceNo_var.ToString());
                foreach (var wt in wttest)
                {
                    mySql += "<tr>";
                    var c1 = dc.Test_View(0, Convert.ToInt32(wt.CEMTTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var n2 in c1)
                    {
                        string TEST_Name_var = n2.TEST_Name_var.ToString();
                        if (TEST_Name_var == "Compressive Strength")
                        {
                            if (wt.CEMTTEST_Days_tint.ToString() != "" && wt.CEMTTEST_Days_tint.ToString() != null && wt.CEMTTEST_Days_tint.ToString() != "0")
                            {
                                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + wt.CEMTTEST_Days_tint.ToString() + " " + "Days" + " " + TEST_Name_var + "</font></td>";
                            }
                        }
                        else
                        {
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + n2.TEST_Name_var.ToString() + "</font></td>";
                        }
                        mySql += "</tr>";
                    }
                    var iscode = dc.SpecifiedLimits_View(wt.CEMTTEST_TEST_Id, "");
                    foreach (var isc in iscode)
                    {
                        strIsCode += isc.splmt_testingMethod_var + "#";
                        break;
                    }                    
                }

                mySql += "</table>";
                mySql += "</td>";

                mySql += "<td>";
                mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
                string[] isCode = strIsCode.Split('#');
                for (int j = 0; j < isCode.Count()-1; j++)
                {
                    mySql += "<tr>";
                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + isCode[j]+ "</font></td>";
                    mySql += "</tr>";
                }
                mySql += "</table>";
                mySql += "</td>";

                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CEMTINWD_Description_var + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CEMTINWD_SupplierName_var + "</font></td>";

                mySql += "</tr>";
                i++;
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
        public string getDetailReportCR(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Compressive Strength Of Concrete Core/s- Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Compressive Strength Of Concrete Core/s- Inward Form</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


         

            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "CR", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CR" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CR" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                          "<td width='20%' align=left valign=top height=19> </td>" +
                          "<td width='45%' height=19> </td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
                else
                {

                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "CR" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "CR" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Date of Casting </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Grade of Concrete </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";

            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("CR", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CRINWD_Description_var + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CRINWD_CastingDate_dt.ToString() + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CRINWD_Grade_var.ToString() + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CRINWD_Quantity_tint.ToString() + "</font></td>";

                mySql += "</tr>";
                i++;
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
        public string getDetailReportCubeInward(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Concrete  Cube Testing Lab Sheet </b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Compressive Strength Of Concrete Cube Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("CT", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "CT", null, null);
            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2,2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                      "<td width='20%' align=left valign=top height=19><font size=2><b> Enquiry No </b></font></td>" +
                      "<td width='2%' height=19><font size=2>:</font></td>" +
                      "<td width='40%' height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                      "<td height=19><font size=2><b>Received Date </b></font></td>" +
                      "<td width='2%'  height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + "CT" + "-" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "<td height=19><font size=2>&nbsp;</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +
                    "<td width='20%' align=left valign=top height=19><font size=2><b> Reference No </b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td width='40%' height=19><font size=2>" + "CT" + "-" + nt.INWD_ReferenceNo_int + " </font></td>" +
                    "<td height=19><font size=2><b>Received Time </b></font></td>" +
                    "<td width='2%' height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("hh:mm:ss tt") + "</font></td>" +
                    "<td height=19><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                    mySql += "<tr>" +
                     "<td width='20%' align=left valign=top height=19><font size=2><b> Total Blocks </b></font></td>" +
                     "<td width='2%' height=19><font size=2>:</font></td>" +
                     "<td width='40%' height=19><font size=2>" + nt.INWD_TotalQty_int + " </font></td>" +
                     "</tr>";
                }
                else
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                       "<td width='2%' height=19><font size=2>:</font></td>" +
                       "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                       "<td height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "<td height=19><font size=2>&nbsp;</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "CT" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";

                    mySql += "<tr>" +

                     "<td align=left valign=top height=19><font size=2><b>Site Address</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                     "</tr>";
                }
            }

            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            var n = dc.AllInward_View("CT", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                if (SrNo == 0)
                {
                    if (prntLabsheet == true)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Ref No.</b></font></td>";
                        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Testing Date</b></font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Casting Date</b></font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Grade  </b></font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Lab Id</b></font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Length (mm)</b></font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Breadth (mm)</b></font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Height (mm)</b></font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Weight (kg) </b></font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Load (kN) </b></font></td>";
                        mySql += "</tr>";
                    }
                    else
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
                        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";
                        mySql += "<td width= 8% align=center valign=top height=19 ><font size=2><b>Date Of Casting</b></font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Grade Of Conc.</b></font></td>";
                        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Nature Of Work</b></font></td>";
                        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Sched. Of Test</b></font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Testing Date</b></font></td>";
                        mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Coupon Nos.</b></font></td>";
                        mySql += "</tr>";
                    }
                }
                SrNo++;
                if (prntLabsheet == true)
                {
                    if (c.CTINWD_Quantity_tint != null)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 2% align=center valign=top height=19 rowspan= " + (c.CTINWD_Quantity_tint + 1) + " ><font size=2>&nbsp;" + c.CTINWD_ReferenceNo_var + "</font></td>";
                        mySql += "<td width= 5% align=left valign=top height=19 rowspan= " + (c.CTINWD_Quantity_tint + 1) + "  ><font size=2>&nbsp;" + c.CTINWD_IdMark_var + "</font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 rowspan= " + (c.CTINWD_Quantity_tint + 1) + " ><font size=2>&nbsp;" + Convert.ToDateTime(c.CTINWD_TestingDate_dt).ToString("dd/MM/yyyy") + "</font></td>";
                        mySql += "<td width= 5% align=center valign=top height=19 rowspan= " + (c.CTINWD_Quantity_tint + 1) + "  ><font size=2>&nbsp;" + c.CTINWD_CastingDate_dt.ToString() + "</font></td>";
                        if (c.CTINWD_Grade_var == "0")
                            mySql += "<td width= 5% align=center valign=top height=19 rowspan= " + (c.CTINWD_Quantity_tint + 1) + " ><font size=2>&nbsp;" + "NA" + "</font></td>";
                        else
                            mySql += "<td width= 5% align=center valign=top height=19 rowspan= " + (c.CTINWD_Quantity_tint + 1) + " ><font size=2>&nbsp;" + "M " + "" + c.CTINWD_Grade_var + "</font></td>";
                        for (int i = 0; i < Convert.ToInt32(c.CTINWD_Quantity_tint); i++)
                        {
                            mySql += "<tr>";
                            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> &nbsp; </font></td>";
                            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> &nbsp; </font></td>";
                            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> &nbsp; </font></td>";
                            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> &nbsp; </font></td>";
                            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> &nbsp; </font></td>";
                            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2> &nbsp; </font></td>";
                            mySql += "</tr>";
                        }
                        mySql += "</tr>";
                    }
                }
                else
                {
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_IdMark_var + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_Quantity_tint + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_CastingDate_dt.ToString() + "</font></td>";
                    if (c.CTINWD_Grade_var == "0")
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "NA" + "</font></td>";
                    else
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "M " + "" + c.CTINWD_Grade_var + "</font></td>";
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_WorkingNature_var + "</font></td>";
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_Description_var + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_Schedule_tint + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDateTime(c.CTINWD_TestingDate_dt).ToString("dd/MM/yyyy") + "</font></td>";
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.CTINWD_CouponNo_var.ToString() + "</font></td>";
                    mySql += "</tr>";
                }
            }

            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            if (prntLabsheet == true)
            {
                mySql += "<table>";
                mySql += "<tr>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp; Remarks :  </font></td>";
                mySql += "<td width= 30% align=left valign=top height=19  ><font size=2>&nbsp;" + "Machine CTM No.- Duro/Equip/Conc/CTM/Duro/Instu/Conc/VC/ " + "</font></td></tr>";
                mySql += "</table>";
                mySql += "<table>";
                mySql += "<tr><td width= 5% align=left valign=top height=19 ><font size=2>&nbsp; Reference : </font></td></tr>";
                mySql += "<tr><td width= 15% align=left valign=top height=19 ><font size=2>&nbsp; 1) IS 516- 1959 Method of tests for strength of concrete . </font></td></tr>";
                //mySql += "<tr><td width= 20% align=left valign=top height=19 ><font size=2>&nbsp; 2) IS 2185 Part I-2005 Concrete Masonary Units -  Specification Part I  Hollow & Solid concrete blocks.  </font></td></tr>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "</tr>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "</tr>";
                mySql += "<tr>";
                mySql += "</table>";
                mySql += "<table>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Tested by  </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Assisted by  </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Approved by  </font></td>";
                mySql += "</tr>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "</tr>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "</tr>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Checked by  </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Comp. Entry by  </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;  </font></td>";
                mySql += "</tr>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
                mySql += "</tr>";
                mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
                mySql += "<tr><td width= 10% align=left valign=top colspan=4 height=19 ><font size=2 ><b> &nbsp; Durocrete Engineering Services Pvt. Ltd.</b> </font></td></tr>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Doc. No. DUROqd3/15 </font></td>";
                mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp; Document : Lab Compressive Strength Test Sheet </font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp; Issue No. 1  </font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp; Dated : 09/12/05  </font></td>";
                mySql += "</tr>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Amendment No. -- </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Amendment Date : -- </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 colspan=2  ><font size=2>&nbsp; </font></td>";
                mySql += "</tr>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 colspan=2 ><font size=2>&nbsp; Approved By : M.T. </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19  ><font size=2>&nbsp; Issued By : M.Q. </font></td>";
                mySql += "<td width= 10% align=left valign=top height=19  ><font size=2>&nbsp; Page 1/1 </font></td>";
                mySql += "</tr>";
                mySql += "</table>";
            }
            else
            {
                mySql += "<table>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
                mySql += "</tr>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Remarks :" + "</font></td>";
                mySql += "</tr>";
                mySql += "<tr>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Testing done as per IS 516-1959." + "</font></td>";
                mySql += "</tr>";
                mySql += "</table>";
            }
            mySql += "</html>";
            return reportStr = mySql;
        }
        public string getDetailReportCubeInwardLabSheet(int RecordNo, int RefNo)
        {
            string reportStr = "", mySql = "", strHeader ="", strFooter="";
            //Footer
            mySql = "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += "</table>";

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp; Remarks :  </font></td>";
            mySql += "<td width= 30% align=left valign=top height=19  ><font size=2>&nbsp;" + "Machine CTM No.- Duro/Equip/Conc/CTM/ " + "</font></td></tr>";
            mySql += "<tr>";
            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp; </font></td>";
            mySql += "<td width= 30% align=left valign=top height=19  ><font size=2>&nbsp;" + "Duro/Instu/Conc/VC/ " + "</font></td></tr>";
            mySql += "</table>";
            mySql += "<table>";
            mySql += "<tr><td width= 5% align=left valign=top height=19 ><font size=2>&nbsp; Reference : </font></td></tr>";
            mySql += "<tr><td width= 15% align=left valign=top height=19 ><font size=2>&nbsp; 1) IS 516- 1959 Method of tests for strength of concrete . </font></td></tr>";
            mySql += "<tr><td width= 20% align=left valign=top height=19 ><font size=2>&nbsp; 2) IS 2185 Part I-2005 Concrete Masonary Units -  Specification Part I  Hollow & Solid concrete blocks.  </font></td></tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "</table>";
            mySql += "<table>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Tested by  </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Assisted by  </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Approved by  </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Checked by  </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Comp. Entry by  </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;  </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "</tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width= 10% align=left valign=top colspan=4 height=19 ><font size=2 ><b> &nbsp; Durocrete Engineering Services Pvt. Ltd.</b> </font></td></tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Doc. No. DUROqd3/10 </font></td>";
            mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp; Document : Lab Compressive Strength Test Sheet </font></td>";
            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp; Issue No. 3  </font></td>";
            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp; Dated : 09/12/05  </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Amendment No. 1 </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Amendment Date : 20-12-12 </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 colspan=2  ><font size=2>&nbsp; </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 colspan=2 ><font size=2>&nbsp; Approved By : M.T. </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19  ><font size=2>&nbsp; Issued By : M.Q. </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19  ><font size=2>&nbsp; Page 1/1 </font></td>";
            mySql += "</tr>";
            mySql += "</table>";
            mySql += "</html>";
            strFooter = mySql;
            ////
            mySql = "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b> Concrete  Cube Testing Lab Sheet </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("CT", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "CT", null, null);
            foreach (var nt in b)
            {
                mySql += "<tr>" +
                  "<td width='20%' align=left valign=top height=19><font size=2><b> Enquiry No </b></font></td>" +
                  "<td width='2%' height=19><font size=2>:</font></td>" +
                  "<td width='40%' height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                  "<td height=19><font size=2><b>Received Date </b></font></td>" +
                  "<td width='2%'  height=19><font size=2>:</font></td>" +
                  "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                  "<td height=19><font size=2>&nbsp;</font></td>" +
                  "</tr>";

                //TimeSpan.Parse(Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("HH:mm:ss tt")) 
                mySql += "<tr>" +
                "<td width='20%' align=left valign=top height=19><font size=2><b> Reference No </b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td width='40%' height=19><font size=2>" + "CT" + "-" + nt.INWD_ReferenceNo_int + " </font></td>" +
                "<td height=19><font size=2><b>Received Time </b></font></td>" +
                "<td width='2%' height=19><font size=2>:</font></td>" +
                "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("hh:mm:ss tt") + "</font></td>" +
                "<td height=19><font size=2>&nbsp;</font></td>" +
                "</tr>";

                mySql += "<tr>" +
                 "<td width='20%' align=left valign=top height=19><font size=2><b> Total Blocks </b></font></td>" +
                 "<td width='2%' height=19><font size=2>:</font></td>" +
                 "<td width='40%' height=19><font size=2>" + nt.INWD_TotalQty_int + " </font></td>" +
                 "</tr>";

            }

            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            var n = dc.AllInward_View("CT", RecordNo, "datewise");
            int SrNo = 0;
            DateTime? tempDate = null;
            foreach (var c in n)
            {
                if (SrNo == 0)
                {
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Ref No.</b></font></td>";
                    mySql += "<td width= 6% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";
                    mySql += "<td width= 4% align=center valign=top height=19 ><font size=2><b>Testing Date</b></font></td>";
                    mySql += "<td width= 4% align=center valign=top height=19 ><font size=2><b>Casting Date</b></font></td>";
                    mySql += "<td width= 3% align=center valign=top height=19 ><font size=2><b>Grade  </b></font></td>";
                    mySql += "<td width= 6% align=center valign=top height=19 ><font size=2><b>Lab Id</b></font></td>";
                    mySql += "<td width= 6% align=center valign=top height=19 ><font size=2><b>Length (mm)</b></font></td>";
                    mySql += "<td width= 6% align=center valign=top height=19 ><font size=2><b>Breadth (mm)</b></font></td>";
                    mySql += "<td width= 6% align=center valign=top height=19 ><font size=2><b>Height (mm)</b></font></td>";
                    mySql += "<td width= 6% align=center valign=top height=19 ><font size=2><b>Weight (kg) </b></font></td>";
                    mySql += "<td width= 8% align=center valign=top height=19 ><font size=2><b>Load (kN) </b></font></td>";
                    mySql += "</tr>";
                }
                if (strHeader == "")
                    strHeader = mySql;

                SrNo++;

                if (c.CTINWD_Quantity_tint != null)
                {
                    if (tempDate != null && tempDate != c.CTINWD_TestingDate_dt)
                    {   
                        mySql += strFooter;
                        mySql += "<DIV style='width:0px;height:0px;page-break-BEFORE:always;'\\></DIV>";
                        mySql += "<DIV style='width:0px;height:133px;'\\></DIV>";
                        mySql += strHeader;
                    }
                    mySql += "<tr border=3 >";
                    mySql += "<td  align=center valign=top height=30 rowspan= " + (c.CTINWD_Quantity_tint + 1) + " ><font size=2>&nbsp;" + c.CTINWD_ReferenceNo_var + "</font></td>";
                    mySql += "<td  align=left valign=top height=30 rowspan= " + (c.CTINWD_Quantity_tint + 1) + "  ><font size=2>&nbsp;" + c.CTINWD_IdMark_var + "</font></td>";
                    mySql += "<td align=center valign=top height=30 rowspan= " + (c.CTINWD_Quantity_tint + 1) + " ><font size=2>&nbsp;" + Convert.ToDateTime(c.CTINWD_TestingDate_dt).ToString("dd/MM/yyyy") + "</font></td>";
                    mySql += "<td  align=center valign=top height=30 rowspan= " + (c.CTINWD_Quantity_tint + 1) + "  ><font size=2>&nbsp;" + c.CTINWD_CastingDate_dt.ToString() + "</font></td>";
                    if (c.CTINWD_Grade_var == "0")
                        mySql += "<td  align=center valign=top height=30 rowspan= " + (c.CTINWD_Quantity_tint + 1) + " ><font size=2>&nbsp;" + "NA" + "</font></td>";
                    else
                        mySql += "<td  align=center valign=top height=30 rowspan= " + (c.CTINWD_Quantity_tint + 1) + " ><font size=2>&nbsp;" + "M " + "" + c.CTINWD_Grade_var + "</font></td>";
                    for (int i = 0; i < Convert.ToInt32(c.CTINWD_Quantity_tint); i++)
                    {
                        mySql += "<tr>";
                        mySql += "<td  align=center valign=top height=30 ><font size=2> &nbsp; </font></td>";
                        mySql += "<td  align=center valign=top height=30 ><font size=2> &nbsp; </font></td>";
                        mySql += "<td  align=center valign=top height=30 ><font size=2> &nbsp; </font></td>";
                        mySql += "<td  align=center valign=top height=30 ><font size=2> &nbsp; </font></td>";
                        mySql += "<td  align=center valign=top height=30 ><font size=2> &nbsp; </font></td>";
                        mySql += "<td  align=center valign=top height=30 ><font size=2> &nbsp; </font></td>";
                        mySql += "</tr>";
                    }
                    mySql += "</tr>";
                    tempDate = c.CTINWD_TestingDate_dt;
                }

            }
            mySql += strFooter;
            return reportStr = mySql;
        }

        public string getDetailReportFLYASH(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>FlyAsh Lab Sheet </b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>FlyAsh Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("FLYASH", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }

          //  DateTime Collectiondate = Convert.ToDateTime(System.Web.HttpContext.Current.Session["CollectionDate"]);
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "FLYASH", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "FLYASH" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "FLYASH" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' align=left valign=top height=19> </td>" +
                        "<td width='45%' height=19> </td>" +
                        "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                        "</tr>";
                }
                else
                {


                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "FLYASH" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "FLYASH" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Fly Ash Name</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Cement Name</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Qty </b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Test To Be Done</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>IS Code</b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("FLYASH", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.FLYASHINWD_FlyAshName_var.ToString() + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.FLYASHINWD_CementName_var.ToString() + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.FLYASHINWD_Quantity_tint.ToString() + "</font></td>";


                mySql += "<td >";
                mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";

                var wttest = dc.AllInward_View("FLYASH", 0, c.FLYASHINWD_ReferenceNo_var.ToString());
                foreach (var wt in wttest)
                {
                    mySql += "<tr>";
                    var c1 = dc.Test_View(0, Convert.ToInt32(wt.FLYASHTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var n2 in c1)
                    {
                        string TEST_Name_var = n2.TEST_Name_var.ToString();
                        if (TEST_Name_var == "Compressive Strength")
                        {
                            if (wt.FLYASHTEST_Days_tint.ToString() != "" && wt.FLYASHTEST_Days_tint.ToString() != null && wt.FLYASHTEST_Days_tint.ToString() != "0")
                            {
                                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + wt.FLYASHTEST_Days_tint.ToString() + " " + "Days" + " " + TEST_Name_var + "</font></td>";
                            }
                        }

                        else
                        {
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + n2.TEST_Name_var.ToString() + "</font></td>";
                        }
                        mySql += "</tr>";
                    }
                }


                mySql += "</table>";
                mySql += "</td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "IS 1727-1967" + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.FLYASHINWD_Description_var.ToString() + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.FLYASHINWD_SupplierName_var.ToString() + "</font></td>";
                mySql += "</tr>";
                i++;
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
        public string getDetailReportSoilInvestigation(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Soil Investigation Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Soil Investigation Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("GT", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }

         
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "GT", null, null);
            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' align=left valign=top height=19><font size=2><b>Enquiry No. </b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td  height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "GT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "GT" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                       "</tr>";
                }
                else
                {
                    mySql += "<tr>" +
                     "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                     "<td height=19><font size=2></font></td>" +
                     "<td height=19><font size=2></font></td>" +
                     "<td align=left valign=top height=19><font size=2><b>Enquiry No. </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "GT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "GT" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Samples </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            var n = dc.AllInward_View("GT", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                if (SrNo == 0)
                {
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
                    //if (Convert.ToInt32(c.GTTEST_Unit_var) != 0)
                    if (c.GTTEST_Unit_var != null && c.GTTEST_Unit_var != "")
                    {
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Unit</b></font></td>";
                    }
                    if (Convert.ToInt32(c.GTTEST_Quantity_tint) != 0)
                    {
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Quantity </b></font></td>";
                    }
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Rate</b></font></td>";
                        mySql += "</tr>";
                    
                }
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.GTTEST_Description_var + "</font></td>";
                //if (Convert.ToInt32(c.GTTEST_Unit_var) != 0)
                if (c.GTTEST_Unit_var != null && c.GTTEST_Unit_var != "")
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.GTTEST_Unit_var.ToString() + "</font></td>";
                }
                if (Convert.ToInt32(c.GTTEST_Quantity_tint) != 0)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.GTTEST_Quantity_tint.ToString() + "</font></td>";
                }
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.GTTEST_Rate_int.ToString() + "</font></td>";
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
        public string getDetailReportNDT(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>NDT Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>NDT Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

         
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "NDT", null, null);
            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "NT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "NT" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +


                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";



                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                       "</tr>";
                }
                else
                {

                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "NT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Nature Of Work</b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>NDT By</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Point Taken</b></font></td>";
            mySql += "</tr>";



            var n = dc.AllInward_View("NDT", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.NDTTEST_NatureofWorking_var.ToString() + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.NDTTEST_NDTBy_var.ToString() + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + c.NDTTEST_Points_tint.ToString() + "</font></td>";
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
        public string getDetailReportPileInward(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Pile Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Pile Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

         
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "PILE", null, null);


            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "PILE" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "PILE" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                       "</tr>";
                }
                else
                {


                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "PILE" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "PILE" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";


                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>No.Of Piles </b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Identification </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";

            var n = dc.AllInward_View("PILE", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.PILEINWD_Description_var + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.PILEINWD_Quantity_tint.ToString() + "</font></td>";

                var pile = dc.PileInward_Update("", RecordNo, "", "", 0, 0, "", "", 0, "", 0, 1, c.PILEINWD_ReferenceNo_var.ToString(), "",null,null,"","",0,0, false);
                bool valid = false;
                string Identification_var = "";
                foreach (var p in pile)
                {
                    Identification_var = Identification_var + p.PILEDETAIL_Identification_var.ToString() + ",";
                    valid = true;
                }
                if (valid == true)
                {
                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + Identification_var + "</font></td>";
                }
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.PILEINWD_SupplierName_var + "</font></td>";
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
        public string getDetailReportPavmentBlockInward(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Pavement Block Lab Sheet </b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Pavement Block Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("PT", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }
         
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "PT", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "PT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "PT" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                    "<td width='20%' align=left valign=top height=19> </td>" +
                    "<td width='45%' height=19> </td>" +
                    "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                    "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                      "</tr>";
                }
                else
                {

                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "PT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "PT" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";



                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";

                    mySql += "<tr>" +

                     "<td align=left valign=top height=19><font size=2><b>Site Address</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                     "</tr>";

                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Date Of Casting</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Type Of Block</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Schedule</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Test to be done</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("PT", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.PTINWD_Id_Mark_var + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.PTINWD_CastingDate_dt.ToString() + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.PTINWD_Description_var + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.PTINWD_BlockType_var.ToString() + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.PTINWD_Quantity_tint + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.PTINWD_Schedule_tint + "</font></td>";

                int TestId = 0;
                TestId = Convert.ToInt32(c.PTINWD_TEST_Id.ToString());

                var Testtype = dc.Test_View(0, Convert.ToInt32(c.PTINWD_TEST_Id.ToString()), "", 0, 0, 0);
                foreach (var t in Testtype)
                {
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + t.TEST_Name_var.ToString() + "</font></td>";
                }

                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.PTINWD_SupplierName_var + "</font></td>";
                mySql += "</tr>";
                i++;
            }


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";


            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Remark" + "</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Testing done as per IS 15658-2006." + "</font></td>";
            mySql += "</tr>";
            mySql += "</table>";


            mySql += "</html>";
            return reportStr = mySql;
        }
        public string getDetailReportSoilTestInward(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Soil Test Lab Sheet </b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Soil Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("SO", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }

         
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "SO", null, null);
            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "SO" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "SO" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                       "</tr>";
                }
                else
                {

                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "SO" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "SO" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";



                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Test to be done</b></font></td>";
            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("SO", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 1% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + c.SOINWD_IdMark_var + "</font></td>";
                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + c.SOINWD_Description_var + "</font></td>";


                mySql += "<td > ";

                var sttest = dc.AllInward_View("SO", 0, c.SOINWD_ReferenceNo_var.ToString());
                foreach (var st in sttest)
                {
                    var cc = dc.Test_View(0, Convert.ToInt32(st.SOTEST_TEST_int), "", 0, 0, 0);
                    foreach (var nn in cc)
                    {
                        string TEST_Name_var = nn.TEST_Name_var.ToString();
                        mySql += "<table border=0 cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
                        mySql += "<tr>";
                        var sq = dc.AllInward_View("SO", RecordNo, c.SOINWD_ReferenceNo_var.ToString());
                        foreach (var q in sq)
                        {
                            if (TEST_Name_var.Contains("Field Density by Sand") == true)
                            {

                                if (q.SOINWD_Pits_tint.ToString() != "" && q.SOINWD_Pits_tint.ToString() != null && q.SOINWD_Pits_tint.ToString() != "0")
                                {
                                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + TEST_Name_var + " " + "(" + q.SOINWD_Pits_tint.ToString() + ")" + "</font></td>";
                                }

                            }
                            else if (TEST_Name_var == "Field Density by Core Cutting")
                            {
                                if (q.SOINWD_Cores_tint.ToString() != "" && q.SOINWD_Cores_tint.ToString() != null && q.SOINWD_Cores_tint.ToString() != "0")
                                {
                                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + TEST_Name_var + " " + "(" + q.SOINWD_Cores_tint.ToString() + ")" + "</font></td>";
                                }

                            }
                            else if (TEST_Name_var == "Classification")
                            {
                                if (q.SOINWD_Quantity_tint.ToString() != "" && q.SOINWD_Cores_tint.ToString() != null && q.SOINWD_Quantity_tint.ToString() != "0")
                                {
                                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + TEST_Name_var + " " + "(" + q.SOINWD_Quantity_tint.ToString() + ")" + "</font></td>";
                                }

                            }
                            else
                            {
                                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + TEST_Name_var + "</font></td>";
                            }
                        }
                        mySql += "</tr>";
                        mySql += "</table>";



                    }
                }



                mySql += "</td>";
                mySql += "</tr>";

                i++;
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
        public string getDetailReportMasonaryInward(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Solid Masonry Block Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Solid Masonry Block Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("SOLID", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }
         
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "SOLID", null, null);
            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "SOLID" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "SOLID" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                      "</tr>";
                }
                else
                {

                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "SOLID" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "SOLID" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Type</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Date Of Casting</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sched.Of Test</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Test</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Date Of Testing</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("SOLID", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_BlockType_var + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_IdMark_var + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_CastingDate_nvar.ToString() + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_Quantity_tint + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_Schedule_tint + "</font></td>";


                var Testtype = dc.Test_View(0, Convert.ToInt32(c.SOLIDINWD_TEST_Id.ToString()), "", 0, 0, 0);
                foreach (var t in Testtype)
                {
                    mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + t.TEST_Name_var.ToString() + "</font></td>";
                }


                mySql += "<td width= 2  % align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDateTime(c.SOLIDINWD_TestingDate_dt).ToString("dd/MM/yyyy") + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_Description_var + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.SOLIDINWD_SupplierName_var + "</font></td>";


                mySql += "</tr>";
                i++;
            }


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Remark" + "</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Testing done as per IS-2185(Part I):2005." + "</font></td>";
            mySql += "</tr>";
            mySql += "</table>";

            mySql += "</html>";
            return reportStr = mySql;
        }
        public string getDetailReportSteelInward(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";
            String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
            string castingDate = "";
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Steel Test Lab Sheet </b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Steel Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("ST", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }
            
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "ST", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "ST" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "ST" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    DateTime Expectdate = Convert.ToDateTime(nt.INWD_ReceivedDate_dt);
                    castingDate = (DateTime.Parse(Expectdate.ToString()).AddDays(3)).ToString("dd/MM/yyyy");

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                       "</tr>";
                }
                else
                {


                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "ST" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "ST" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    DateTime Expectdate = Convert.ToDateTime(nt.INWD_ReceivedDate_dt);
                    castingDate = (DateTime.Parse(Expectdate.ToString()).AddDays(3)).ToString("dd/MM/yyyy");

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Dia(mm)</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";

            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Test</b></font></td>";

            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Grade</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sample Description</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Supplier Name/Manufacturer Name</b></font></td>";

            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("ST", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.STINWD_Diameter_tint + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.STINWD_Quantity_tint + "</font></td>";
                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + c.STINWD_IdMark_var + "</font></td>";



                mySql += "<td > ";
                mySql += "<table border=0 cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";

                var wttest = dc.AllInward_View("ST", 0, c.STINWD_ReferenceNo_var.ToString());
                foreach (var wt in wttest)
                {
                    var c1 = dc.Test_View(0, Convert.ToInt32(wt.STTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var n2 in c1)
                    {
                        mySql += "<tr>";
                        mySql += "<td>";//

                        mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
                        mySql += "<tr>";
                        mySql += "<td width= 30% align=left valign=top height=19 ><font size=2>&nbsp;" + n2.TEST_Name_var.ToString() + "</font></td>";


                        mySql += "</tr>";
                        mySql += "</table>";
                        mySql += "</td>";
                        mySql += "</tr>";
                    }
                }
                mySql += "</table>";
                mySql += "</td>";


                //mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + "Fe " + "" + c.STINWD_Grade_var + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.STINWD_Grade_var + "</font></td>";
                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + c.STINWD_Description_var + "</font></td>";
                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + c.STINWD_SupplierName_var + "</font></td>";


                mySql += "</tr>";
                i++;
            }


            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";


            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + "Expected Date Of Report - " + castingDate.ToString() + "</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + "* Tensile Test shall be carried out as per IS 1786 - 2008,Clause No. 8.2 or 8.2.1 & 8.2.2 . Client may indicate his choice before our testing schedule." + "</font></td>";
            mySql += "</tr>";
            mySql += "</table>";


            mySql += "</html>";
            return reportStr = mySql;
        }
        public string getDetailReportSteelChemicalTestInward(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Steel Chemical Test Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Steel Chemical Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("STC", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }

         
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "STC", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "STC" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "STC" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                       "</tr>";
                }
                else
                {


                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "STC" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "STC" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>Dia(mm)</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>ID Mark</b></font></td>";

            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Grade</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Sample Description</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Supplier Name/Manufacturer Name</b></font></td>";

            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("STC", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 1% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 1% align=center valign=top height=19 ><font size=2>&nbsp;" + c.STCINWD_Daimeter_tint + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.STCINWD_Quantity_tint + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.STCINWD_IdMark_var + "</font></td>";
                //mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + "Fe " + "" + c.STCINWD_Grade_var + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.STCINWD_Grade_var + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.STCINWD_Description_var + "</font></td>";
                mySql += "<td width= 2% align=left valign=top height=19 ><font size=2>&nbsp;" + c.STCINWD_SupplierName_var + "</font></td>";


                mySql += "</tr>";
                i++;
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
        public string getDetailReportTileInward(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Tile Test Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Tile Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("TILE", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }
         
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "TILE", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "TILE" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "TILE" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                      "</tr>";
                }
                else
                {


                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "TILE" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "TILE" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Samples </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Tile Type</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Test</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>ID.Mark </b></font></td>";
            mySql += "<td width=10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";


            var n = dc.AllInward_View("TILE", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.TILEINWD_TileType_var.ToString() + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.TILEINWD_Quantity_tint.ToString() + "</font></td>";
                string TestNameVar = "";
                var c1 = dc.Test_View(0, Convert.ToInt32(c.TILEINWD_TEST_Id.ToString()), "", 0, 0, 0);
                foreach (var n2 in c1)
                {
                    TestNameVar = n2.TEST_Name_var.ToString();
                }
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + TestNameVar + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.TILEINWD_IdMark_var.ToString() + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.TILEINWD_Description_var + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.TILEINWD_SupplierName_var + "</font></td>";
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
        public string getDetailReportWT(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Water Test Lab Sheet </b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Water Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("WT", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }

         
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "WT", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "WT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "WT" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                      "</tr>";
                }
                else
                {


                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "WT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "WT" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";

            mySql += "<td>";
            mySql += "<table  width=100% id=AutoNumber2>";
            mySql += "<tr>";
            mySql += "<td width= 30% align=center valign=top height=19 ><font size=2><b> Test </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b> Qty(lit)</b></font></td>";
            mySql += "</tr>";
            mySql += "</table>";
            mySql += "</td>";


            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";


            int i = 0;
            var n = dc.AllInward_View("WT", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.WTINWD_Description_var + "</font></td>";

                mySql += "<td > ";
                mySql += "<table border=0 cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";

                var wttest = dc.AllInward_View("WT", 0, c.WTINWD_ReferenceNo_var.ToString());
                foreach (var wt in wttest)
                {
                    var c1 = dc.Test_View(0, Convert.ToInt32(wt.WTTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var n2 in c1)
                    {
                        mySql += "<tr>";
                        mySql += "<td>";//

                        mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
                        mySql += "<tr>";
                        mySql += "<td width= 30% align=left valign=top height=19 ><font size=2>&nbsp;" + n2.TEST_Name_var.ToString() + "</font></td>";

                        var c11 = dc.Test_View(0, Convert.ToInt32(wt.WTTEST_TEST_Id), "", 0, 0, 0);
                        foreach (var d1 in c11)
                        {
                            string TEST_Name_var = d1.TEST_Name_var.ToString();
                            var Rateint = dc.Test_View(0, Convert.ToInt32(wt.WTTEST_TEST_Id), "", 0, 0, 0);
                            foreach (var r in Rateint)
                            {
                                if (wt.WTTEST_Qty_tint.ToString() != null && wt.WTTEST_Qty_tint.ToString() != "")
                                {
                                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + wt.WTTEST_Qty_tint.ToString() + "</font></td>";
                                }
                            }
                        }

                        mySql += "</tr>";
                        mySql += "</table>";
                        mySql += "</td>";
                        mySql += "</tr>";
                    }
                }
                mySql += "</table>";
                mySql += "</td>";


                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.WTINWD_SupplierName_var.ToString() + "</font></td>";
                mySql += "</tr>";
                i++;
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
        public string getDetailReportOT(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Other Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Other Inward</b></font></td></tr>";
            }
            var subtest = dc.AllInward_View("OT", RecordNo, "");
            string subset = "";
            foreach (var o in subtest)
            {
                subset = o.OTINWD_SubTest_var.ToString();
            }
            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>" + subset + "</b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

         
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "OT", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "OT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "OT" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +


                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";


                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                       "</tr>";
                }
                else
                {


                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "OT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";


                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "OT" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Sample Description</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b> Test </b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b> Qty </b></font></td>";
            mySql += "</tr>";

            var n = dc.AllInward_View("OT", RecordNo, "");
            int SrNo = 0;

            foreach (var c in n)
            {
                SrNo++;
                int i = 0;
                var test = dc.AllInward_View("OT", 0, c.OTINWD_ReferenceNo_var).ToList();
                int rowspan = test.Count;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 rowspan=" + rowspan + "><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19  rowspan=" + rowspan + "><font size=2>&nbsp;" + c.OTINWD_Description_var.ToString() + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19  rowspan=" + rowspan + "><font size=2>&nbsp;" + c.OTINWD_SupplierName_var.ToString() + "</font></td>";
                int j = 0;
                foreach (var ot in test)
                {
                    if (j > 0)
                    {
                        mySql += "<tr>";
                    }
                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + ot.TEST_Name_var.ToString() + "</font></td>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + ot.OTRATEIN_Quantity_tint.ToString() + "</font></td>";
                    mySql += "</tr>";
                    j++;
                }                
                i++;
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
        public string getDetailReportRWH(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Rain Water Harvesting Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Rain Water Harvesting Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("RWH", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }

         
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "RWH", null, null);
            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "RWH" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "RWH" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                      "</tr>";
                }
                else
                {

                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "RWH" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "RWH" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

        

            int i = 0;
            var n = dc.AllInward_View("RWH", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                if (SrNo == 0)
                {
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
                    //if (Convert.ToInt32(c.GTTEST_Unit_var) != 0)
                    if (c.GTTEST_Unit_var != null && c.GTTEST_Unit_var != "")
                    {
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Unit </b></font></td>";
                    }
                    if (Convert.ToInt32(c.GTTEST_Quantity_tint) != 0)
                    {
                        mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Quantity </b></font></td>";
                    }
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Rate</b></font></td>";
                    mySql += "</tr>";
                }

                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.GTTEST_Description_var + "</font></td>";
                //if (Convert.ToInt32(c.GTTEST_Unit_var) != 0)
                if (c.GTTEST_Unit_var != null && c.GTTEST_Unit_var != "")
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.GTTEST_Unit_var.ToString() + "</font></td>";
                }
                if (Convert.ToInt32(c.GTTEST_Quantity_tint) != 0)
                {
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.GTTEST_Quantity_tint.ToString() + "</font></td>";
                }
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.GTTEST_Rate_int.ToString() + "</font></td>";

                mySql += "</tr>";
                i++;
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
        public string getDetailReportCoreCutting(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Core Cutting Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Core Cutting Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "CORECUT", null, null);
            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CORECUT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "CORECUT" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +


                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";



                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                       "</tr>";
                }
                else
                {

                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "CORECUT" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 1% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Depth Of Core</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Diameter Of Core</b></font></td>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Qty</b></font></td>";
            mySql += "</tr>";
            var n = dc.AllInward_View("CORECUT", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 1% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CORECUTINWD_DepthofCore_var.ToString() + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CORECUTINWD_DiameterofCore_var.ToString() + "</font></td>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + c.CORECUTINWD_Quantity_tint.ToString() + "</font></td>";
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
        public string getDetailReportMF(int RecordNo, int RefNo, bool prntLabsheet)
        {
            
            string reportStr = "", mySql = "", tempSql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            //mySql += "<tr><td width='100%' height='105'>";
            mySql += "<tr><td width='100%' >";
            String CurrentYear = DateTime.Now.Year.ToString().Substring(2, 2);
            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            //mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            //mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Mix Design LabSheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Mix Design Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("MF", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }
            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "MF", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {   
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id  + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' align=left valign=top height=19> </td>" +
                        "<td width='45%' height=19> </td>" +
                        "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td width='1%' height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "MF" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "MF" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                     "</tr>";
                }
                else
                {
                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "MF" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "MF" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                         "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";

            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Grade of Concrete</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Nature of Work</b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Any Special Requirement</b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Slump Requirement </b></font></td>";
            mySql += "</tr>";

            var n = dc.AllInward_View("MF", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.MFINWD_Grade_var.ToString() + "</font></td>";
                mySql += "<td width= 20% align=center valign=top height=19 ><font size=2>&nbsp;" + c.MFINWD_NatureofWork_var.ToString() + "</font></td>";
                mySql += "<td width= 20% align=center valign=top height=19 ><font size=2>&nbsp;" + c.MFINWD_SpecialRequirement_var.ToString() + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + c.MFINWD_Slump_var.ToString() + "</font></td>";
                mySql += "</tr>";

            }
            mySql += "</table>";
            mySql += "</table>";
            mySql += "<tr><td colspan=7 align=left valign=top height=20 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=4>" + "Material Inward Details :" + "</font></td></tr>";


            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b> Sr No. </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Material Type </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b> Material </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Material Name </b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b> Qty </b></font></td>";
            mySql += "</tr>";

            SrNo = 0;
            int MatSrNo = 0;
            var Mix = dc.MaterialDetail_View(RecordNo, "", 0, "", null, null, "");
            foreach (var m in Mix)
            {
                if (MatSrNo != Convert.ToInt32(m.MaterialDetail_SrNo) || MatSrNo == 0)
                {
                    SrNo++;
                    mySql += "<tr>";
                    mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(m.Material_Type) + "</font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(m.Material_List) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(m.MaterialDetail_Information) + "</font></td>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToString(m.MaterialDetail_Quantity) + "</font></td>";
                    mySql += "</tr>";
                }
                MatSrNo = Convert.ToInt32(m.MaterialDetail_SrNo);
            }
            mySql += "<table >";

            mySql += "<tr><td width= 2% align=left valign=top height=19 ><font size=4>&nbsp;" + "For all technical queries contact on (020)24348027." + "</font></td></tr>";

            mySql += "</table>";
            mySql += "</tr>";
            mySql += tempSql;
            mySql += "</table>";
            mySql += "</html>";
            return reportStr = mySql;

        }

        public string getDetailReportMFInwardLabSheet(DateTime Fromdate, DateTime ToDate)
        {
            string reportStr = "", mySql = "", strHeader = "", strFooter = "";
            strFooter = "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += "</table>";
            ////
            mySql = "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=3 align=center valign=top height=19><font size=4><b> Trial  Cube Testing Lab Sheet </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=3>&nbsp;</td></tr>";

            mySql += "<tr>" +
                      "<td width='30%' align=left valign=top height=19><font size=2><b> From Date : " + Fromdate.ToString("dd/MM/yyyy") + "</b></font></td>" +
                      "<td><font size=2><b>To Date : " + ToDate.ToString("dd/MM/yyyy") + "</b></font></td>" +
                      "<td><font size=2>&nbsp;</font></td>" +
                      "</tr>";

            var trial = dc.TestingSchedule_View_MF(Fromdate, ToDate);
            int SrNo = 0, rowCount = 0;
            foreach (var t in trial)
            {
                if (SrNo == 0)
                {
                    strHeader = "<tr><td colspan=3 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
                    strHeader += "<tr><td colspan=3 align=left valign=top>";
                    strHeader += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
                    strHeader += "<tr>";
                    strHeader += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Ref No.</b></font></td>";
                    strHeader += "<td width= 6% align=center valign=top height=19 ><font size=2><b>Trial Name</b></font></td>";
                    strHeader += "<td width= 4% align=center valign=top height=19 ><font size=2><b>Testing Date</b></font></td>";
                    strHeader += "<td width= 4% align=center valign=top height=19 ><font size=2><b>Casting Date</b></font></td>";
                    strHeader += "<td width= 3% align=center valign=top height=19 ><font size=2><b>Grade  </b></font></td>";
                    strHeader += "<td width= 6% align=center valign=top height=19 ><font size=2><b>Lab Id</b></font></td>";
                    strHeader += "<td width= 6% align=center valign=top height=19 ><font size=2><b>Length (mm)</b></font></td>";
                    strHeader += "<td width= 6% align=center valign=top height=19 ><font size=2><b>Breadth (mm)</b></font></td>";
                    strHeader += "<td width= 6% align=center valign=top height=19 ><font size=2><b>Height (mm)</b></font></td>";
                    strHeader += "<td width= 6% align=center valign=top height=19 ><font size=2><b>Weight (kg) </b></font></td>";
                    strHeader += "<td width= 8% align=center valign=top height=19 ><font size=2><b>Load (kN) </b></font></td>";
                    strHeader += "</tr>";
                    mySql += strHeader;
                }
                SrNo++;

                if ((rowCount % 24 == 0 && rowCount > 0) || rowCount + t.Quantity > 24)
                {
                    mySql += strFooter;
                    mySql += "<DIV style='width:0px;height:0px;page-break-BEFORE:always;'\\></DIV>";
                    mySql += "<DIV style='width:0px;height:100px;'\\></DIV>";
                    mySql += strHeader;
                    rowCount = 0;
                }
                mySql += "<tr border=3 >";
                mySql += "<td  align=center valign=top height=30 rowspan= " + (t.Quantity + 1) + " ><font size=2>&nbsp;" + t.RefNo + "</font></td>";
                mySql += "<td  align=left valign=top height=30 rowspan= " + (t.Quantity + 1) + "  ><font size=2>&nbsp;" + t.Trial_Name + "</font></td>";
                mySql += "<td align=center valign=top height=30 rowspan= " + (t.Quantity + 1) + " ><font size=2>&nbsp;" + Convert.ToDateTime(t.TestingDt).ToString("dd/MM/yyyy") + "</font></td>";
                mySql += "<td  align=center valign=top height=30 rowspan= " + (t.Quantity + 1) + "  ><font size=2>&nbsp;" + t.CastingDt.ToString() + "</font></td>";
                mySql += "<td  align=center valign=top height=30 rowspan= " + (t.Quantity + 1) + " ><font size=2>&nbsp;" + t.Trial_Grade + "</font></td>";
                for (int i = 0; i < Convert.ToInt32(t.Quantity); i++)
                {
                    mySql += "<tr>";
                    mySql += "<td  align=center valign=top height=30 ><font size=2> &nbsp; </font></td>";
                    mySql += "<td  align=center valign=top height=30 ><font size=2> &nbsp; </font></td>";
                    mySql += "<td  align=center valign=top height=30 ><font size=2> &nbsp; </font></td>";
                    mySql += "<td  align=center valign=top height=30 ><font size=2> &nbsp; </font></td>";
                    mySql += "<td  align=center valign=top height=30 ><font size=2> &nbsp; </font></td>";
                    mySql += "<td  align=center valign=top height=30 ><font size=2> &nbsp; </font></td>";
                    mySql += "</tr>";
                    rowCount++;
                }
                mySql += "</tr>";

            }
            //Footer
            mySql += strFooter;

            if ((rowCount % 24 == 0 && rowCount > 0) || rowCount + 3 > 24)
            {
                mySql += "<DIV style='width:0px;height:0px;page-break-BEFORE:always;'\\></DIV>";
                mySql += "<DIV style='width:0px;height:100px;'\\></DIV>";
            }

            mySql += "<table>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Tested by  </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Assisted by  </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Approved by  </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Checked by  </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Comp. Entry by  </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Supervise by </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;   </font></td>";
            mySql += "</tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width= 10% align=left valign=top colspan=4 height=19 ><font size=2 ><b> &nbsp; Durocrete Engineering Services Pvt. Ltd.</b> </font></td></tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Doc. No. DUROqd3/10 </font></td>";
            mySql += "<td width= 20% align=left valign=top height=19 ><font size=2>&nbsp; Document : Lab Compressive Strength Test Sheet </font></td>";
            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp; Issue No. 3  </font></td>";
            mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp; Dated : 09/12/05  </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Amendment No. 1 </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp; Amendment Date : 20-12-12 </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19 colspan=2  ><font size=2>&nbsp; </font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=left valign=top height=19 colspan=2 ><font size=2>&nbsp; Approved By : M.T. </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19  ><font size=2>&nbsp; Issued By : M.Q. </font></td>";
            mySql += "<td width= 10% align=left valign=top height=19  ><font size=2>&nbsp; Page 1/1 </font></td>";
            mySql += "</tr>";
            mySql += "</table>";
            mySql += "</html>";
            return reportStr = mySql;
        }

        public string getDetailReportGgbsChemicalInwd(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>GGBS Chemical Test Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>GGBS Chemical Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            var n1 = dc.AllInward_View("GGBSCH", RecordNo, "");
            int SampleNo = 0;
            foreach (var c1 in n1)
            {
                SampleNo++;
            }


            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "GGBSCH", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "GGBSCH" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "GGBSCH" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +

                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +
                         "<td width='20%' align=left valign=top height=19> </td>" +
                         "<td width='45%' height=19> </td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
                else
                {

                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "GGBSCH" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "GGBSCH" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                        "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td height=19><font size=2></font></td>" +
                        "<td align=left valign=top height=19><font size=2><b>No of Sample </b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + SampleNo + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }
            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>GGBS Name</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Qty </b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Test </b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";
            
            int i = 0;
            var n = dc.AllInward_View("GGBSCH", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.GGBSCHINWD_GgbsName_var + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.GGBSCHINWD_Quantity_tint.ToString() + "</font></td>";
                
                mySql += "<td >";
                mySql += "<table border=1 cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";

                var wttest = dc.AllInward_View("GGBSCH", 0, c.GGBSCHINWD_ReferenceNo_var.ToString());
                foreach (var wt in wttest)
                {
                    var c1 = dc.Test_View(0, Convert.ToInt32(wt.GGBSCHTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var n2 in c1)
                    {
                        mySql += "<tr>";
                        mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + n2.TEST_Name_var.ToString() + "</font></td>";
                        mySql += "</tr>";
                    }
                }


                mySql += "</table>";
                mySql += "</td>";

                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.GGBSCHINWD_Description_var + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.GGBSCHINWD_SupplierName_var + "</font></td>";
                mySql += "</tr>";
                i++;
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

        public string getDetailReportGgbsInward(int RecordNo, int RefNo, bool prntLabsheet)
        {
            string reportStr = "", mySql = "", tempSql = "";
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
            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";
            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            if (prntLabsheet == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>GGBS Test Lab Sheet</b></font></td></tr>";
            }
            else
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>GGBS Test Inward</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";


            var b = dc.ModifyInward_View(RecordNo, RefNo, null, "GGBS", null, null);

            foreach (var nt in b)
            {
                CurrentYear = Convert.ToDateTime(nt.INWD_ReceivedDate_dt).Year.ToString().Substring(2, 2);
                if (prntLabsheet == true)
                {
                    mySql += "<tr>" +
                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Enquiry No</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.INWD_ENQ_Id + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td width='16%' height=19><font size=2><b>Record No.</b></font></td>" +
                       "<td width='1%' height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "GGBS" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +

                       "<td width='20%' align=left valign=top height=19> </td>" +
                       "<td width='45%' height=19> </td>" +
                       "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + "GGBS" + "-" + RefNo + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                     "<td width='20%' align=left valign=top height=19> </td>" +
                     "<td width='45%' height=19> </td>" +
                     "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                     "<td height=19><font size=2>:</font></td>" +
                     "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                     "</tr>";

                    mySql += "<tr>" +

                      "<td width='20%' align=left valign=top height=19> </td>" +
                      "<td width='45%' height=19> </td>" +
                      "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                      "<td height=19><font size=2>:</font></td>" +
                      "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                      "</tr>";

                    mySql += "<tr>" +
                          "<td width='20%' align=left valign=top height=19> </td>" +
                          "<td width='45%' height=19> </td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
                else
                {


                    mySql += "<tr>" +
                        "<td width='20%' align=left valign=top height=19><font size=2><b>Customer Name</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='40%' height=19><font size=2>" + nt.CL_Name_var + "</font></td>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + "GGBS" + "-" + nt.INWD_RecordNo_int + "</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +
                         "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td height=19><font size=2></font></td>" +
                         "<td align=left valign=top height=19><font size=2><b>Sample Ref. No.</b></font></td>" +
                         "<td height=19><font size=2>:</font></td>" +
                         "<td height=19><font size=2>" + "GGBS" + "-" + RefNo + "</font></td>" +
                         "</tr>";

                    mySql += "<tr>" +

                        "<td width='20%' height=19><font size=2><b>Office Address</b></font></td>" +
                        "<td width='2%' height=19><font size=2>:</font></td>" +
                        "<td width='10%' height=19><font size=2>" + nt.CL_OfficeAddress_var + "</font></td>" +
                        "<td height=19><font size=2><b>Bill No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>DT -  " + nt.INWD_BILL_Id + "</font></td>" +
                        "</tr>";

                    mySql += "<tr>" +

                       "<td align=left valign=top height=19><font size=2><b>Site Name</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + nt.SITE_Name_var + "</font></td>" +
                       "<td align=left valign=top height=19><font size=2><b> Received Date</b></font></td>" +
                       "<td height=19><font size=2>:</font></td>" +
                       "<td height=19><font size=2>" + Convert.ToDateTime(nt.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                       "</tr>";

                    mySql += "<tr>" +
                          "<td align=left valign=top height=19><font size=2><b></b></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td height=19><font size=2></font></td>" +
                          "<td align=left valign=top height=19><font size=2><b>Test Request Form No</b></font></td>" +
                          "<td height=19><font size=2>:</font></td>" +
                          "<td height=19><font size=2>" + nt.INWD_ReferenceNo_int + "/" + CurrentYear + "</font></td>" +
                          "</tr>";
                }
            }

            mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";
            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr>";
            mySql += "<td width= 2% align=center valign=top height=19 ><font size=2><b>Sr No.</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>GGBS Name</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Cement Name</b></font></td>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Qty </b></font></td>";
            mySql += "<td width= 20% align=center valign=top height=19 ><font size=2><b>Test </b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Test Done As Per</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Description</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Supplier Name</b></font></td>";
            mySql += "</tr>";

            int i = 0;
            var n = dc.AllInward_View("GGBS", RecordNo, "");
            int SrNo = 0;
            foreach (var c in n)
            {
                SrNo++;
                mySql += "<tr>";
                mySql += "<td width= 2% align=center valign=top height=19 ><font size=2>&nbsp;" + SrNo + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.GGBSINWD_GgbsName_var + "</font></td>";
                mySql += "<td width= 5% align=left valign=top height=19 ><font size=2>&nbsp;" + c.GGBSINWD_CementName_var + "</font></td>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + c.GGBSINWD_Quantity_tint.ToString() + "</font></td>";

                mySql += "<td >";
                mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
                string strIsCode = "";
                var wttest = dc.AllInward_View("GGBS", 0, c.GGBSINWD_ReferenceNo_var.ToString());
                foreach (var wt in wttest)
                {
                    mySql += "<tr>";
                    var c1 = dc.Test_View(0, Convert.ToInt32(wt.GGBSTEST_TEST_Id), "", 0, 0, 0);
                    foreach (var n2 in c1)
                    {
                        string TEST_Name_var = n2.TEST_Name_var.ToString();
                        if (TEST_Name_var == "Compressive Strength" || TEST_Name_var == "Slag activity index")
                        {
                            if (wt.GGBSTEST_Days_tint.ToString() != "" && wt.GGBSTEST_Days_tint.ToString() != null && wt.GGBSTEST_Days_tint.ToString() != "0")
                            {
                                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + wt.GGBSTEST_Days_tint.ToString() + " " + "Days" + " " + TEST_Name_var + "</font></td>";
                            }
                        }
                        else
                        {
                            mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + n2.TEST_Name_var.ToString() + "</font></td>";
                        }
                        mySql += "</tr>";
                    }
                    var iscode = dc.SpecifiedLimits_View(wt.GGBSTEST_TEST_Id, "");
                    foreach (var isc in iscode)
                    {
                        strIsCode += isc.splmt_testingMethod_var + "#";
                        break;
                    }
                }

                mySql += "</table>";
                mySql += "</td>";

                mySql += "<td>";
                mySql += "<table border=1 style=border-collapse:collapse cellpadding=0 cellspacing=0 width=100% id=AutoNumber1>";
                string[] isCode = strIsCode.Split('#');
                for (int j = 0; j < isCode.Count() - 1; j++)
                {
                    mySql += "<tr>";
                    mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + isCode[j] + "</font></td>";
                    mySql += "</tr>";
                }
                mySql += "</table>";
                mySql += "</td>";

                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.GGBSINWD_Description_var + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + c.GGBSINWD_SupplierName_var + "</font></td>";

                mySql += "</tr>";
                i++;
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
    }
}