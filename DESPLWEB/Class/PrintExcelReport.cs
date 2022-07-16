using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Excel = Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Core;
using System.IO;

namespace DESPLWEB
{
    public class PrintExcelReport
    {
        //static string foldername = "C:/temp/Veena/";
        //static string foldername = "C:\\temp\\Veena\\";
        //private void DownloadReport(string fileName, string filePath)
        //{
        //    System.Web.HttpContext.Current.Response.ClearContent();
        //    System.Web.HttpContext.Current.Response.ClearHeaders();
        //    System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        //    System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
        //    System.Web.HttpContext.Current.Response.WriteFile(filePath);
        //    System.Web.HttpContext.Current.Response.Flush();
        //}

        //public void BankSlip_ExcelPrint()
        //{
        //    var fileName = "BankSlip_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".xls";
        //    if (!Directory.Exists(@foldername))
        //        Directory.CreateDirectory(@foldername);

        //    Excel.Application xlApp;
        //    Excel.Workbook xlWorkBook;
        //    Excel.Worksheet xlWorkSheet;
        //    object misValue = System.Reflection.Missing.Value;

        //    xlApp = new Excel.ApplicationClass(); //This is where the problem is??????
        //    xlWorkBook = xlApp.Workbooks.Add(misValue);
        //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //    //add some text 
        //    xlWorkSheet.Cells[1, 1] = "http://csharp.net-informations.com";
        //    xlWorkSheet.Cells[2, 1] = "Adding picture in Excel File";

        //    //xlWorkSheet.Shapes.AddPicture("C:\\csharp-xl-picture.JPG", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 50, 50, 300, 45);

        //    xlWorkBook.SaveAs(foldername + fileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //    xlWorkBook.Close(true, misValue, misValue);
        //    xlApp.Quit();

        //    releaseObject(xlApp);
        //    releaseObject(xlWorkBook);
        //    releaseObject(xlWorkSheet);

        //    DownloadReport(fileName, foldername + fileName);

        //}

        //private void releaseObject(object obj)
        //{
        //    try
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
        //        obj = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        obj = null;
        //        //MessageBox.Show("Unable to release the Object " + ex.ToString());
        //    }
        //    finally
        //    {
        //        GC.Collect();
        //    }
        //} 
    }
}