using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.IO;
using System.Data.OleDb;


namespace DESPLWEB
{
    public partial class RouteUpdate : System.Web.UI.Page
    {

        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool userRight = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"].ToString()), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_Account_right_bit == true)
                        userRight = true;
                }
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Route Updation";
                if (userRight == true)
                {
                    FileUpload1.Attributes.Add("onchange", "return checkFileExtension(this);");
                }
                else
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //string FilePath = ConfigurationManager.AppSettings["FilePath"].ToString();
            string filename = string.Empty;

          
            if (FileUpload1.HasFile)
            {

                //Get file name of selected file
                filename = Path.GetFileName(Server.MapPath(FileUpload1.FileName));
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);

                string FilePath = Server.MapPath("~/Images/" + FileUpload1.FileName.Replace(' ','_'));  
          
              
                //Save selected file into server location
               // FileUpload1.SaveAs(Server.MapPath(FilePath) + filename);
                FileUpload1.SaveAs(FilePath);
                
                
                //string filePath = Server.MapPath(FilePath) + filename;

                OleDbConnection con = null;
                if (Extension == ".xls")
                    con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=Excel 8.0;");
                else if (Extension == ".xlsx")
                    con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=Excel 8.0;");

                con.Open();
                //Get the list of sheet available in excel sheet
                DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //Get first sheet name
                string getExcelSheetName = dt.Rows[0]["Table_Name"].ToString();
                //Select rows from first sheet in excel sheet and fill into dataset
                OleDbCommand ExcelCommand = new OleDbCommand(@"SELECT * FROM [" + getExcelSheetName + @"]", con);
                OleDbDataAdapter ExcelAdapter = new OleDbDataAdapter(ExcelCommand);
                DataSet ExcelDataSet = new DataSet();
                ExcelAdapter.Fill(ExcelDataSet);
                con.Close();
                //DataTable newTable = ExcelDataSet.Tables[0].DefaultView.ToTable(true, "f12", "f15");
                //newTable.Rows[0].Delete();
                //newTable.Columns["f12"].ColumnName = "Site Name";
                //newTable.Columns["f15"].ColumnName = "Route";
                //newTable.AcceptChanges();
                
                DataTable newTable = ExcelDataSet.Tables[0].DefaultView.ToTable(false,"Client Name", "Site Name", "Route Name","Bill No");
               
                //Bind the dataset into gridview to display excel contents
                //gvDetails.DataSource = newTable;
                //gvDetails.DataBind();
             
                for (int i = 0; i < newTable.Rows.Count; i++)
                {
                    DataRow row = newTable.Rows[i];
                    if (row.IsNull(0) == true)
                    {
                        newTable.Rows[i].Delete();
                    }
                }
                newTable.AcceptChanges();
                gvDetails.DataSource = newTable;
                gvDetails.DataBind();
                //gvDetails.Columns[3].Visible = false;
                lblCount.Text = "Total No of Records : " + gvDetails.Rows.Count;
          
            }
        }




        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

       
        protected void lnkRouteUpdate_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
              
            if (gvDetails.Rows.Count > 0)
            {
                for (int i = 0; i < gvDetails.Rows.Count; i++)
                {
                    dc.Site_Update_RouteWise(0, -1, gvDetails.Rows[i].Cells[2].Text.TrimEnd(), gvDetails.Rows[i].Cells[3].Text.TrimEnd());//update route id against respective site
           
                }
                lblMsg.Text = "Updated Successfully..";
                lblMsg.Visible = true;

            }   
                   
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[3].Visible = false;
        }
    }
}