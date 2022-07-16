using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace DESPLWEB
{
    public partial class classPdfFooter1 : PdfPageEventHelper
    {
        static string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
        //private int nablStatus;
        private int printCnt = 0;
         public classPdfFooter1()
        { }
        public classPdfFooter1(int printCnt)
        {
            this.printCnt = printCnt;
        }
       public override void OnEndPage(PdfWriter writer, Document doc)
        {
            int pageN = writer.PageNumber;
            String text = "Page No " + pageN;// +" of ";
         
            string tollFree = "", strPrintCnt = "";
            if (cnStr.ToLower().Contains("mumbai") == true)
                tollFree = " Contact No : 9850500013";
            else if (cnStr.ToLower().Contains("nashik") == true)
                tollFree = "";
            else
                tollFree = " Toll-Free No : 18001206465"; //180030000096

            if (printCnt > 0)
                strPrintCnt = "#" + printCnt;
        
            PdfPTable footerTbl = new PdfPTable(1);

          //  Paragraph footer = new Paragraph("Regd. Office: 1160/5,Gharpure Colony, Shivaji Nagar,Pune 411005. Maharashtra,India. CIN: U28939PN1999PTC014212." + tollFree + "   " + strPrintCnt, FontFactory.GetFont(FontFactory.TIMES, 7F, iTextSharp.text.Font.NORMAL));
            Paragraph footer = new Paragraph("Regd. Office: 1160/5,Gharpure Colony, Shivaji Nagar,Pune 411005. Maharashtra,India. CIN: U28939PN1999PTC014212." + tollFree, FontFactory.GetFont(FontFactory.TIMES, 7F, iTextSharp.text.Font.NORMAL));
            footer.Alignment = Element.ALIGN_LEFT;
            footerTbl.TotalWidth = doc.PageSize.Width - doc.LeftMargin;
            footerTbl.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfPCell cell = new PdfPCell(footer);
            cell.Border = 0;
            cell.PaddingLeft = 10;
            cell.PaddingRight = 10;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            footerTbl.AddCell(cell);

            //cell = new PdfPCell(new Phrase("Regd. Office: 1160/5,Gharpure Colony, Shivaji Nagar,Pune 411005. Maharashtra,India. CIN: U28939PN1999PTC014212", FontFactory.GetFont(FontFactory.TIMES, 7F, iTextSharp.text.Font.NORMAL)));
            //cell.Border = 0;
            //cell.PaddingLeft = 10;
            //cell.PaddingRight = 10;
            //footerTbl.AddCell(cell);
            //footerTbl.HorizontalAlignment = Element.ALIGN_RIGHT;

          // footerTbl.WriteSelectedRows(0, -1, 45,13,  writer.DirectContent);
           footerTbl.WriteSelectedRows(0, -1, doc.LeftMargin, footerTbl.TotalHeight, writer.DirectContent);
           doc.SetMargins(doc.LeftMargin, doc.RightMargin, doc.TopMargin, doc.BottomMargin+5);//+30
           //doc.SetMargins(55f, 25f, 21f, 10f);
            // footerTbl.WriteSelectedRows(0, -1, 150, doc.Bottom, writer.DirectContent);
         //  footerTbl.WriteSelectedRows(0, -1, 0, (doc.PageSize.Height - 6), writer.DirectContent);
    
        }

     

    }
}