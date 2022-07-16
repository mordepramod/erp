using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Net;
using Ionic.Zip;

namespace DESPLWEB
{
    public static class NewWindows
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

        //public static void PrintRFI_Pdf(string strRFIId)
        //{

        //    try
        //    {
        //        RFIDataDataContext dc = new RFIDataDataContext();
        //        //string strRFIId = Session["RFIId"].ToString();

        //        var r = dc.RFI_View(Convert.ToInt32(strRFIId), 0, 0, 0, 0, 0, "");
        //        foreach (var rfi in r)
        //        {
        //            Paragraph paragraph = new Paragraph();
        //            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 50f, 0f);
        //            var fileName = rfi.RFI_No_var.Replace('/', '-') + " " + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".pdf";
        //            string foldername = "RFIPDFReport";
        //            string driveName = "D:/";
        //            if (!Directory.Exists(@driveName + foldername))
        //                Directory.CreateDirectory(@driveName + foldername);

        //            //string Subfoldername = foldername + "/Brick";
        //            //if (!Directory.Exists(@"D:\" + Subfoldername))
        //            //    Directory.CreateDirectory(@"D:/" + Subfoldername);
        //            //string Subfoldername1 = Subfoldername;

        //            PdfWriter.GetInstance(pdfDoc, new FileStream(@driveName + foldername + "/" + fileName, FileMode.Create));
        //            pdfDoc.Open();

        //            PdfPTable table1 = null;
        //            pdfDoc.Open();
        //            PdfPTable MaindataTable = new PdfPTable(4); //2
        //            MaindataTable.WidthPercentage = 100;
        //            MaindataTable.DefaultCell.Border = PdfPCell.NO_BORDER;
        //            paragraph.Alignment = Element.ALIGN_CENTER;

        //            iTextSharp.text.Font headingFont = iTextSharp.text.FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14, iTextSharp.text.Font.NORMAL);
        //            paragraph.Font = headingFont;

        //            paragraph.Add("RFI Detail");
        //            paragraph.SpacingAfter = 20;
        //            pdfDoc.Add(paragraph);

        //            paragraph = new Paragraph();
        //            Font fontTitle = new Font(Font.FontFamily.TIMES_ROMAN, 11, Font.BOLD);
        //            Font fontH1 = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.UNDEFINED);
        //            Font fontH2 = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD);

        //            int cntCol = 0;
        //            //float[] widths = new float[] { 20f, 55f };
        //            float[] widths = new float[] { 15f, 30f, 15f, 30f };
        //            MaindataTable.SetWidths(widths);
        //            PdfPCell Cust_Namecell = new PdfPCell(new Phrase("Client", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            try
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.ClientName, fontH1));
        //            }
        //            catch (Exception ex)
        //            {

        //                Cust_Namecell = new PdfPCell(new Phrase(": -", fontH1));
        //            }
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            cntCol++;
        //            Cust_Namecell = new PdfPCell(new Phrase("Project", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);

        //            try
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.ProjectName, fontH1));
        //            }
        //            catch (Exception ex)
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase(": -", fontH1));
        //            }
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            cntCol++;
        //            //Structure
        //            Cust_Namecell = new PdfPCell(new Phrase("Structure", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            try
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.Parent1Name, fontH1));
        //            }
        //            catch (Exception ex)
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase(": -", fontH1));
        //            }

        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            cntCol++;
        //            //Stage
        //            if (rfi.Parent2Name != "" && rfi.Parent2Name != null)
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase("Stage", fontH1));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);

        //                try
        //                {
        //                    Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.Parent2Name, fontH1));
        //                }
        //                catch (Exception ex)
        //                {

        //                    Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //                }
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);
        //                cntCol++;
        //            }

        //            //Unit
        //            if (rfi.Parent3Name != "" && rfi.Parent3Name != null)
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase("Unit", fontH1));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);

        //                try
        //                {
        //                    Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.Parent3Name, fontH1));
        //                }
        //                catch (Exception ex)
        //                {

        //                    Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //                }

        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);
        //                cntCol++;
        //            }
        //            //Sub-Unit
        //            if (rfi.Parent4Name != "" && rfi.Parent4Name != null)
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase("Sub-Unit", fontH1));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);

        //                try
        //                {
        //                    Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.Parent4Name, fontH1));
        //                }
        //                catch (Exception ex)
        //                {

        //                    Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //                }

        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);
        //                cntCol++;
        //            }
        //            //Element
        //            if (rfi.Parent5Name != "" && rfi.Parent5Name != null)
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase("Element", fontH1));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);
        //                try
        //                {
        //                    Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.Parent5Name, fontH1));
        //                }
        //                catch (Exception ex)
        //                {

        //                    Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //                }

        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell); cntCol++;
        //            }
        //            //Sub-Element
        //            if (rfi.Parent6Name != "" && rfi.Parent6Name != null)
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase("Sub-Element", fontH1));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);
        //                try
        //                {
        //                    Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.Parent6Name, fontH1));
        //                }
        //                catch (Exception ex)
        //                {

        //                    Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //                }

        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell); cntCol++;
        //            }
        //            //Work Type
        //            Cust_Namecell = new PdfPCell(new Phrase("Work Type", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            try
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.WorkTypeName, fontH1));
        //            }
        //            catch (Exception ex)
        //            {

        //                Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //            }

        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell); cntCol++;
        //            //Checklist
        //            Cust_Namecell = new PdfPCell(new Phrase("Check List", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            try
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.CheckListName, fontH1));
        //            }
        //            catch (Exception ex)
        //            {

        //                Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //            }

        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell); cntCol++;
        //            //Group
        //            Cust_Namecell = new PdfPCell(new Phrase("Group", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            try
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.GroupName, fontH1));
        //            }
        //            catch (Exception ex)
        //            {

        //                Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //            }

        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell); cntCol++;
        //            //Coverage
        //            Cust_Namecell = new PdfPCell(new Phrase("Coverage", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            try
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.RFI_Coverage_var, fontH1));
        //            }
        //            catch (Exception ex)
        //            {

        //                Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //            }

        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell); cntCol++;
        //            //Drawing number
        //            Cust_Namecell = new PdfPCell(new Phrase("Drawing No", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            try
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.RFI_DrawingNo_var, fontH1));
        //            }
        //            catch (Exception ex)
        //            {

        //                Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //            }

        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell); cntCol++;
        //            //maker contractor name
        //            if (rfi.MakerContractorName != null)
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase("Maker Representing", fontH1));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);
        //                try
        //                {
        //                    Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.MakerContractorName, fontH1));
        //                }
        //                catch (Exception ex)
        //                {

        //                    Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //                }

        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell); cntCol++;
        //            }
        //            //checker contractor name
        //            if (rfi.CheckerContractorName != null)
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase("Checker Representing", fontH1));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);
        //                try
        //                {
        //                    Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.CheckerContractorName, fontH1));
        //                }
        //                catch (Exception ex)
        //                {

        //                    Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //                }

        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell); cntCol++;
        //            }
        //            //approver contractor name
        //            if (rfi.ApproverContractorName != null)
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase("Approver Representing", fontH1));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);
        //                try
        //                {
        //                    Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.ApproverContractorName, fontH1));
        //                }
        //                catch (Exception ex)
        //                {

        //                    Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //                }

        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell); cntCol++;
        //            }
        //            //RFI number                    
        //            Cust_Namecell = new PdfPCell(new Phrase("RFI No.", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);
        //            try
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.RFI_No_var, fontH1));
        //            }
        //            catch (Exception ex)
        //            {

        //                Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //            }
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell); cntCol++;
        //            //Raised date
        //            DateTime tempDate;
        //            if (rfi.RFI_CreatedOnServer_dt != null)
        //            {
        //                tempDate = Convert.ToDateTime(rfi.RFI_CreatedOnServer_dt);

        //                Cust_Namecell = new PdfPCell(new Phrase("Raised Date", fontH1));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);
        //                try
        //                {
        //                    Cust_Namecell = new PdfPCell(new Phrase(": " + tempDate.ToString("dd/MM/yyyy hh:mm tt"), fontH1));
        //                }
        //                catch (Exception ex)
        //                {

        //                    Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //                }
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell); cntCol++;
        //            }
        //            //Status
        //            Cust_Namecell = new PdfPCell(new Phrase("Status", fontH1));
        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell);

        //            try
        //            {
        //                if (rfi.RFI_CancelStatus_bit == true)
        //                {
        //                    Cust_Namecell = new PdfPCell(new Phrase(": " + "Cancelled", fontH1));
        //                }
        //                else
        //                {
        //                    if (rfi.RFI_Status_tint == 3)
        //                    {
        //                        if (rfi.rfiTime != null)
        //                        {
        //                            Cust_Namecell = new PdfPCell(new Phrase(": " + "Cleared within " + (rfi.rfiTime + 1) + " day ", fontH1));
        //                        }
        //                        else
        //                        {
        //                            Cust_Namecell = new PdfPCell(new Phrase(": " + "Cleared ", fontH1));
        //                        }

        //                    }
        //                    else
        //                    {
        //                        Cust_Namecell = new PdfPCell(new Phrase(": " + "Pending ", fontH1));
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //                Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //            }

        //            Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //            MaindataTable.AddCell(Cust_Namecell); cntCol++;

        //            //cancelled comment
        //            if (rfi.RFI_CancelRemark_var != null)
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase("Cancelled Comment", fontH1));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);
        //                try
        //                {
        //                    Cust_Namecell = new PdfPCell(new Phrase(": " + rfi.RFI_CancelRemark_var, fontH1));
        //                }
        //                catch (Exception ex)
        //                {

        //                    Cust_Namecell = new PdfPCell(new Phrase(": ", fontH1));
        //                }
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                //Cust_Namecell.Colspan = 3;
        //                MaindataTable.AddCell(Cust_Namecell); cntCol++;

        //            }
        //            if (cntCol % 2 != 0)
        //            {
        //                Cust_Namecell = new PdfPCell(new Phrase(" ", fontH1));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);

        //                Cust_Namecell = new PdfPCell(new Phrase(" ", fontH1));
        //                Cust_Namecell.Border = PdfPCell.NO_BORDER;
        //                MaindataTable.AddCell(Cust_Namecell);
        //            }
        //            //           
        //            pdfDoc.Add(MaindataTable);

        //            // Headers for table.
        //            string[] header = { "Maker", "Entered Date", "Entered Time", "Question", "Answer", "Remark", "Image1", "Image2", " ", "Checker", "Checked Date", "Checked Time", "Answer", "Remark", "Image1", "Image1" };

        //            #region RFI Details
        //            table1 = new PdfPTable(16);
        //            table1.SpacingBefore = 5;
        //            float[] widthsDetail = new float[] { 10f, 10f, 10f, 20f, 10f, 10f, 10f, 10f, 1f, 10f, 10f, 10f, 10f, 10f, 10f, 10f };
        //            table1.SetWidths(widthsDetail);
        //            var rd = dc.RFIDetail_View(Convert.ToInt32(strRFIId), 0, false).ToList();
        //            var count = rd.Count();

        //            PdfPCell cell1;
        //            for (int h = 0; h < header.Count(); h++)
        //            {
        //                cell1 = new PdfPCell(new Phrase(header[h], fontH2));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                table1.AddCell(cell1);
        //            }

        //            int i = 0, round = 1;
        //            DateTime enterDate = DateTime.Now;
        //            DateTime createdDate;

        //            string queId = "";
        //            table1.HorizontalAlignment = Element.ALIGN_LEFT;
        //            table1.WidthPercentage = 100;
        //            for (int j = 0; j < count; j++)
        //            {
        //                createdDate = Convert.ToDateTime(rd[j].RFIDETAIL_CreatedOn_dt);
        //                if ((enterDate != createdDate && i > 0) || (queId.Contains("," + rd[j].RFIDETAIL_QUE_Id.ToString() + ",") == true) || j == 0)
        //                {
        //                    //for (int z = 1; z <= 16; z++)
        //                    //{
        //                    cell1 = new PdfPCell(new Phrase("Round (" + round.ToString() + ")", fontH2));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    cell1.Colspan = 16;
        //                    table1.AddCell(cell1);
        //                    //}
        //                    queId = "";
        //                    round++;
        //                    i++;
        //                }
        //                queId += "," + rd[j].RFIDETAIL_QUE_Id.ToString() + ",";
        //                enterDate = Convert.ToDateTime(rd[j].RFIDETAIL_CreatedOn_dt);

        //                string strTemp = "";
        //                strTemp = rd[j].MakerName;
        //                cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                table1.AddCell(cell1);

        //                strTemp = "";
        //                if (rd[j].RFIDETAIL_UpdatedOnOffL_dt != null)
        //                {
        //                    tempDate = Convert.ToDateTime(rd[j].RFIDETAIL_UpdatedOnOffL_dt);
        //                    strTemp = tempDate.ToString("dd/MM/yyyy");
        //                }
        //                else if (rd[j].RFIDETAIL_CreatedOn_dt != null)
        //                {
        //                    tempDate = Convert.ToDateTime(rd[j].RFIDETAIL_CreatedOn_dt);
        //                    strTemp = tempDate.ToString("dd/MM/yyyy");
        //                }
        //                cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                table1.AddCell(cell1);

        //                strTemp = "";
        //                if (rd[j].RFIDETAIL_UpdatedOnOffL_time != null)
        //                {
        //                    tempDate = Convert.ToDateTime(rd[j].RFIDETAIL_UpdatedOnOffL_time.ToString());
        //                    strTemp = tempDate.ToShortTimeString();
        //                }
        //                else if (rd[j].RFIDETAIL_CreatedOnOffL_time != null)
        //                {
        //                    tempDate = Convert.ToDateTime(rd[j].RFIDETAIL_CreatedOnOffL_time.ToString());
        //                    strTemp = tempDate.ToShortTimeString();
        //                }
        //                cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                table1.AddCell(cell1);

        //                strTemp = rd[j].QUE_Description_var;
        //                cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                table1.AddCell(cell1);

        //                strTemp = rd[j].RFIDETAIL_Answer_var;
        //                cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                table1.AddCell(cell1);

        //                strTemp = rd[j].RFIDETAIL_Remark_var;
        //                cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                table1.AddCell(cell1);

        //                //string imgLink = "http://www.cqra-qa.com/RFI/images/";
        //                //string imgLink = "http://www.cqra-qaqc.com/~cqraqate/RFI/images/";
        //                string imgLink = "http://cqra-qaqc.com/RFI/images/";
        //                iTextSharp.text.Image gif;
        //                strTemp = "";
        //                if (rd[j].RFIDETAIL_ImagePath1_var != "")
        //                {
        //                    if (CheckFileExist(imgLink + rd[j].RFIDETAIL_ImagePath1_var) == true)
        //                    {
        //                        strTemp = imgLink + rd[j].RFIDETAIL_ImagePath1_var;

        //                        gif = iTextSharp.text.Image.GetInstance(strTemp);
        //                        cell1 = new PdfPCell(gif, true);
        //                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        table1.AddCell(cell1);
        //                    }

        //                }
        //                if (strTemp == "")
        //                {
        //                    cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);
        //                }

        //                strTemp = "";
        //                if (rd[j].RFIDETAIL_ImagePath2_var != "")
        //                {
        //                    if (CheckFileExist(imgLink + rd[j].RFIDETAIL_ImagePath2_var) == true)
        //                    {
        //                        strTemp = imgLink + rd[j].RFIDETAIL_ImagePath2_var;

        //                        gif = iTextSharp.text.Image.GetInstance(strTemp);
        //                        cell1 = new PdfPCell(gif, true);
        //                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        table1.AddCell(cell1);
        //                    }
        //                }
        //                if (strTemp == "")
        //                {
        //                    cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);
        //                }

        //                cell1 = new PdfPCell(new Phrase("|", fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                table1.AddCell(cell1);

        //                strTemp = "";
        //                if (rd[j].RFIDETAIL_CheckBy_int > 0)
        //                    strTemp = rd[j].CheckerName.ToString();
        //                cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                table1.AddCell(cell1);

        //                strTemp = "";
        //                if (rd[j].RFIDETAIL_CheckedOn_dt != null)
        //                {
        //                    tempDate = Convert.ToDateTime(rd[j].RFIDETAIL_CheckedOn_dt);
        //                    strTemp = tempDate.ToString("dd/MM/yyyy");
        //                }
        //                cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                table1.AddCell(cell1);

        //                strTemp = "";
        //                if (rd[j].RFIDETAIL_CheckedOnOffL_time != null)
        //                {
        //                    tempDate = Convert.ToDateTime(rd[j].RFIDETAIL_CheckedOnOffL_time.ToString());
        //                    strTemp = tempDate.ToShortTimeString();
        //                }
        //                cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                table1.AddCell(cell1);

        //                strTemp = rd[j].RFIDETAIL_Status_tint.ToString();
        //                if (strTemp == "0" && rd[j].RFIDETAIL_CheckBy_int > 0)
        //                    strTemp = "Send Back";
        //                else if (strTemp == "1" || rd[j].RFIDETAIL_CheckBy_int == null
        //                    || rd[j].RFIDETAIL_CheckBy_int == 0)
        //                    strTemp = "";
        //                else if (strTemp == "2")
        //                    strTemp = "Ok";
        //                else if (strTemp == "3")
        //                    strTemp = "Ok";
        //                cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                table1.AddCell(cell1);

        //                strTemp = rd[j].RFIDETAIL_ChkRemark_var;
        //                cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                table1.AddCell(cell1);

        //                strTemp = "";
        //                if (rd[j].RFIDETAIL_ChkImagePath1_var != "")
        //                {
        //                    if (CheckFileExist(imgLink + rd[j].RFIDETAIL_ChkImagePath1_var) == true)
        //                    {
        //                        strTemp = imgLink + rd[j].RFIDETAIL_ChkImagePath1_var;

        //                        gif = iTextSharp.text.Image.GetInstance(strTemp);
        //                        cell1 = new PdfPCell(gif, true);
        //                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        table1.AddCell(cell1);
        //                    }
        //                }
        //                if (strTemp == "")
        //                {
        //                    cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);
        //                }

        //                strTemp = "";
        //                if (rd[j].RFIDETAIL_ChkImagePath2_var != "")
        //                {
        //                    if (CheckFileExist(imgLink + rd[j].RFIDETAIL_ChkImagePath2_var) == true)
        //                    {
        //                        strTemp = imgLink + rd[j].RFIDETAIL_ChkImagePath2_var;

        //                        gif = iTextSharp.text.Image.GetInstance(strTemp);
        //                        cell1 = new PdfPCell(gif, true);
        //                        cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        table1.AddCell(cell1);
        //                    }
        //                }
        //                if (strTemp == "")
        //                {
        //                    cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);
        //                }
        //            }
        //            //                            
        //            pdfDoc.Add(table1);
        //            #endregion

        //            #region RFI approved details
        //            var apprd = dc.RFIApproveDetail_View(Convert.ToInt32(strRFIId)).ToList();
        //            var apprcount = apprd.Count();
        //            if (apprcount > 0)
        //            {
        //                string[] headerAppr = { "Approved By", "Approved Date", "Approved Time", "Comment", "Status" };
        //                table1 = new PdfPTable(5);
        //                table1.SpacingBefore = 5;
        //                float[] widthsAppr = new float[] { 10f, 10f, 10f, 10f, 10f };
        //                table1.SetWidths(widthsAppr);

        //                for (int h = 0; h < headerAppr.Count(); h++)
        //                {
        //                    cell1 = new PdfPCell(new Phrase(headerAppr[h], fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);
        //                }

        //                pdfDoc.Add(new Paragraph("Approved Details", fontH2));
        //                table1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                table1.WidthPercentage = 100;
        //                for (int j = 0; j < apprcount; j++)
        //                {
        //                    string strTemp = "";
        //                    strTemp = apprd[j].ApproveByName;
        //                    cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);

        //                    strTemp = "";
        //                    if (apprd[j].APPR_ApprovedOn_dt != null)
        //                    {
        //                        tempDate = Convert.ToDateTime(apprd[j].APPR_ApprovedOn_dt);
        //                        strTemp = tempDate.ToString("dd/MM/yyyy");
        //                    }
        //                    cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);

        //                    strTemp = "";
        //                    if (apprd[j].APPR_ApprovedOn_dt != null)
        //                    {
        //                        tempDate = Convert.ToDateTime(apprd[j].APPR_ApprovedOn_dt.ToString());
        //                        strTemp = tempDate.ToShortTimeString();
        //                    }
        //                    cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);

        //                    strTemp = apprd[j].APPR_ApprComment_var;
        //                    cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);

        //                    strTemp = apprd[j].APPR_Status_bit.ToString();
        //                    if (strTemp == "0")
        //                        strTemp = "Approved";
        //                    else
        //                        strTemp = "Not Approved";
        //                    cell1 = new PdfPCell(new Phrase(strTemp, fontH1));
        //                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    table1.AddCell(cell1);
        //                }
        //                //                            
        //                pdfDoc.Add(table1);
        //            }
        //            #endregion
        //            pdfDoc.Close();

        //            string pdfPath = @driveName + foldername + "/" + fileName;
        //            //Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
        //            //Response.ContentType = "application/pdf";
        //            //byte[] ar = new byte[(int)pdfPath.Length];
        //            //Response.BinaryWrite(ar);
        //            //Response.End();

        //            ////WebClient User = new WebClient();
        //            ////Byte[] FileBuffer = User.DownloadData(pdfPath);
        //            ////if (FileBuffer != null)
        //            ////{
        //            ////    Response.ContentType = "application/pdf";
        //            ////    Response.AddHeader("content-length", FileBuffer.Length.ToString());
        //            ////    Response.BinaryWrite(FileBuffer);
        //            ////}

        //            NewWindows.Redirect(pdfPath, "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");

        //            //Process process = new Process();
        //            //process.StartInfo.UseShellExecute = true;
        //            //process.StartInfo.FileName = pdfPath;
        //            //process.Start();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}
        
        
        public static void PrintReport_Html(string strImages, string strPath)
        {
            string reportStr = "", reportPath;
            StreamWriter sw;
            reportPath = strPath + "\\report.html";
            sw = File.CreateText(reportPath);
            reportStr = strImages;
            sw.WriteLine(reportStr);
            sw.Close();

            NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");

        }

        public static bool CheckFileExist(string strUrl)
        {
            try
            {
                //return true;
                // Check to see if url1 exists or not
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static void DownloadHtmlReport(string fileName, string reportStr)
        {
            string foldername = "C:/temp/RFI";
            fileName = fileName.Replace("/", "-") + ".html";
            if (!Directory.Exists(@foldername))
                Directory.CreateDirectory(@foldername);
            string reportPath = @foldername + "/" + fileName;
            StreamWriter sw;
            sw = File.CreateText(reportPath);
            sw.WriteLine(reportStr);
            sw.Close();
        }

        public static void DownloadFiles_Zip()
        {
            string[] filePaths = Directory.GetFiles("C:/temp/RFI/");
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("Files");

                foreach (string filePath in filePaths)
                {
                    zip.AddFile(filePath, "Files");
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.BufferOutput = false;
                string zipName = String.Format("RFI_{0}.zip", DateTime.Now.ToString("dd-MM-yyyy-HHmmss"));
                HttpContext.Current.Response.ContentType = "application/zip";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(HttpContext.Current.Response.OutputStream);
                foreach (string filePath in filePaths)
                {
                    File.Delete(filePath);
                }
                HttpContext.Current.Response.End();
            }
        }

    }
}