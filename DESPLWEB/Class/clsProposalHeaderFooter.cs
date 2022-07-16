using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace DESPLWEB
{
    public partial class clsProposalHeaderFooter : PdfPageEventHelper
    {
        PdfContentByte pdfContent;
        //PdfTemplate template;
        protected BaseFont helv;
        //BaseFont bf = null;

        static string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
        private int nablStatus;
        private int instance = 0;
        public clsProposalHeaderFooter()
        { }
        public clsProposalHeaderFooter(int instance)
        {
            this.instance = instance;
        }
        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            string imageURL = "";
            nablStatus = 0;
            if (cnStr.ToLower().Contains("mumbai") == true)
            {
                if (nablStatus == 0)//nonnabl
                    imageURL = System.Web.HttpContext.Current.Server.MapPath(".") + "/Images/testLogoMum.png";
                else if (nablStatus == 1)
                    imageURL = System.Web.HttpContext.Current.Server.MapPath(".") + "/Images/testLogoMumNABL.png";

            }
            else if (cnStr.ToLower().Contains("nashik") == true)
            {
                imageURL = System.Web.HttpContext.Current.Server.MapPath(".") + "/Images/testLogoNashik.png";
            }
            else
            {
                if (nablStatus == 0)//nonnabl
                    imageURL = System.Web.HttpContext.Current.Server.MapPath(".") + "/Images/testLogoPune.png";
                else if (nablStatus == 1)
                    imageURL = System.Web.HttpContext.Current.Server.MapPath(".") + "/Images/testLogoPuneNABL.png";
            }
            iTextSharp.text.Image imgPDF = iTextSharp.text.Image.GetInstance(imageURL);

            PdfPTable pdfTab = new PdfPTable(1);
            imgPDF.ScaleAbsolute(500, 72);
            //imgPDF.SetAbsolutePosition(document.PageSize.Width - 50.0F - 500.0F, document.PageSize.Height - 780.0F - 300.6F);
            //imgPDF.SetAbsolutePosition(document.PageSize.Width - 40.0F - 560.0F, document.PageSize.Height - 780.0F - 300.6F); OO
            PdfPCell pdfCell1 = new PdfPCell(imgPDF);
            pdfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell1.Border = 0;
            pdfCell1.Indent = 0;

            pdfTab.AddCell(pdfCell1);
            pdfTab.SpacingAfter = 50f;
            pdfTab.TotalWidth = document.PageSize.Width;

            pdfTab.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 5, writer.DirectContent);
            if (instance == 1)
            {
                //document.SetMargins(document.LeftMargin, document.RightMargin, document.TopMargin +70, document.BottomMargin + 30);
                instance = 0;
            }
            else
                document.SetMargins(document.LeftMargin, document.RightMargin, document.TopMargin, document.BottomMargin + 30);
          
            //pdfTab.WriteSelectedRows(0, -1, 10, document.PageSize.Height - 15, writer.DirectContent); OO
            pdfContent = writer.DirectContent;
            //pdfContent.MoveTo(30, document.PageSize.Height - 100);
            pdfContent.Stroke();
        }
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            int pageN = writer.PageNumber;
            String text = "Page No " + pageN;// +" of ";

            string tollFree = ""; //, strPrintCnt = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                tollFree = " 9850500013";
            else if (cnStr.ToLower().Contains("nashik") == true)
                tollFree = "";
            else
                tollFree = " Toll-Free No : 18001206465";

            //if (printCnt > 0)
            //    strPrintCnt = "#" + printCnt;


            Phrase p1Footer = new Phrase("Regd. Office: 1160/5,Gharpure Colony, Shivaji Nagar,Pune 411005. Maharashtra,India. CIN: U28939PN1999PTC014212." + tollFree + "      " + text, FontFactory.GetFont(FontFactory.TIMES, 7F, iTextSharp.text.Font.NORMAL));
            //iTextSharp.text.Image imgPDF = iTextSharp.text.Image.GetInstance(HttpRuntime.AppDomainAppPath + "\\Images\\logo.gif");
            PdfPTable pdfTabFooter = new PdfPTable(1);
            //PdfPCell pdfCell1 = new PdfPCell(imgPDF);
            PdfPCell pdfCell2 = new PdfPCell(p1Footer);
            //pdfCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            //pdfCell1.Border = 0;
            pdfCell2.Border = 0;
            //pdfTab.AddCell(pdfCell1);
            pdfTabFooter.TotalWidth = 500f;
            float[] cfWidths = new float[] { 50f };
            pdfTabFooter.SetWidths(cfWidths);
            pdfTabFooter.AddCell(pdfCell2);
            //pdfTabFooter.TotalWidth = document.PageSize.Width - 20;
            pdfTabFooter.WriteSelectedRows(0, -1, document.LeftMargin, pdfTabFooter.TotalHeight, writer.DirectContent);
            //pdfTabFooter.WriteSelectedRows(0, -1, 10, document.PageSize.Height - 15, writer.DirectContent);
           // document.SetMargins(document.LeftMargin, document.RightMargin, document.TopMargin + 50, document.BottomMargin + 30);
            document.SetMargins(document.LeftMargin, document.RightMargin, document.TopMargin+10, document.BottomMargin + 30);
            pdfContent = writer.DirectContent;
            pdfContent.MoveTo(30, document.PageSize.Height - 50);
            //pdfContent.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 40);
            pdfContent.Stroke();
        }

    }
}