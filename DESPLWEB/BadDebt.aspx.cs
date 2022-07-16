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
    public partial class BadDebt : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool userRight = false;
                var user = dc.User_View(Convert.ToInt32( Session["LoginId"].ToString()), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_Account_right_bit == true)
                        userRight = true;
                }
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Import Bad Debts";
                if (userRight == true)
                {
                    FileUpload1.Attributes.Add("onchange", "return checkFileExtension(this);");
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                    pnlContent.Visible = false;
                    lblheading.Text = "Import Bad Debts";
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
            }
        }

        private object ReadToEnd(string filePath)
        {
            DataTable dtDataSource = new DataTable();
            string[] fileContent = File.ReadAllLines(filePath);
            if (fileContent.Count() > 0)
            {
                //Create data table columns
                string[] columns = fileContent[0].Split(',');
                for (int i = 0; i < columns.Count(); i++)
                {
                    dtDataSource.Columns.Add(columns[i]);
                }

                //Add row data
                for (int i = 1; i < fileContent.Count(); i++)
                {
                    string[] rowData = fileContent[i].Split(',');
                    dtDataSource.Rows.Add(rowData);
                }
            }
            return dtDataSource;
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            if (FileUpload1.HasFile && Path.GetExtension(FileUpload1.PostedFile.FileName)==".csv") //&& FileUpload1.PostedFile.ContentType.Equals("application/vnd.ms-excel"))
            {
                string FilePath = ConfigurationManager.AppSettings["FilePath"].ToString();
                string filename = string.Empty;
                filename = Path.GetFileName(Server.MapPath(FileUpload1.FileName));
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                //Save selected file into server location
                FileUpload1.SaveAs(Server.MapPath(FilePath) + filename);
                filePath = Server.MapPath(FilePath) + filename;
                gvDetails.DataSource = (DataTable)ReadToEnd(filePath);
                gvDetails.DataBind();
                lblAccess.Visible = false;
            }
            else
            {
                lblAccess.Text = "Please Upload valid file(extenstion must be .csv)";
                lblAccess.Visible = true;
            }

            //string FilePath = ConfigurationManager.AppSettings["FilePath"].ToString();
            //string filename = string.Empty;
            //if (FileUpload1.HasFile)
            //{

            //    //Get file name of selected file
            //    filename = Path.GetFileName(Server.MapPath(FileUpload1.FileName));
            //    string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
             
            //    //Save selected file into server location
            //    FileUpload1.SaveAs(Server.MapPath(FilePath) + filename);
            //    string filePath = Server.MapPath(FilePath) + filename;
          
            //    OleDbConnection con = null;
            //    if (Extension == ".xls")
            //        con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;");
            //    else if (Extension == ".xlsx")
            //        con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;");

            //    con.Open();
            //    //Get the list of sheet available in excel sheet
            //    DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //    //Get first sheet name
            //    string getExcelSheetName = dt.Rows[0]["Table_Name"].ToString();
            //    //Select rows from first sheet in excel sheet and fill into dataset
            //    OleDbCommand ExcelCommand = new OleDbCommand(@"SELECT * FROM [" + getExcelSheetName + @"]", con);
            //    OleDbDataAdapter ExcelAdapter = new OleDbDataAdapter(ExcelCommand);
            //    DataSet ExcelDataSet = new DataSet();
            //    ExcelAdapter.Fill(ExcelDataSet);
            //    con.Close();
            //    //Bind the dataset into gridview to display excel contents
            //    gvDetails.DataSource = ExcelDataSet;
            //    gvDetails.DataBind();
            //}
        }




        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            string mystr = "Error for bill Nos-";
            if (Page.IsValid)
            {
                                
                if (gvDetails.Rows.Count > 0)
                {
                    lnkUpdate.Enabled = false;
                    bool flgRecAvail = false;
                    DateTime CurrentDt = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null);
                    int ClientId = 0;
                    string year = "";
                    var result = dc.MasterSetting_View(0).ToList();
                    if (result.Count > 0)
                    {
                        string rest = Convert.ToString(result.FirstOrDefault().MASTER_AccountingYear_var);
                        year = rest.Substring(rest.Length - 4).ToString();
                    }

                   // string crNoteNo = "CR/18-19/";
                    string crNoteNo = "CR/" + String.Format("{0}-{1}", year.Substring(0, 2), year.Substring(2, 2)) +"/";//by renuka
                    Int32 cnt = 0;
                    if (txtReceiptNo.Text.Trim() != "")
                    {
                        cnt = Convert.ToInt32(txtReceiptNo.Text);
                        if (cnt > 0)
                        {
                            lblNotTransfered.Text = "";
                            for (int i = 0; i < gvDetails.Rows.Count; i++)
                            {
                                if (gvDetails.Rows[i].Cells[1].Text.ToString().Trim() != "")
                                {
                                    string billId = gvDetails.Rows[i].Cells[0].Text.Trim().ToString().Trim();
                                    var cl = dc.ClientID_View(billId).ToList();
                                    ClientId = 0;
                                    if (cl.Count() > 0)
                                    {
                                        ClientId = Convert.ToInt32(cl.FirstOrDefault().BILL_CL_Id.ToString());
                                        crNoteNo = "CR/" + String.Format("{0}-{1}", year.Substring(0, 2), year.Substring(2, 2)) +"/" + cnt.ToString();
                                        cnt++;
                                        dc.Journal_Update(crNoteNo, CurrentDt, false, ClientId, 0, "Bad debts", 0, Convert.ToDecimal(gvDetails.Rows[i].Cells[1].Text.ToString()), 1, false);

                                        dc.JournalDetail_Update(crNoteNo, 10, 1, 1, Convert.ToDecimal(gvDetails.Rows[i].Cells[1].Text.ToString()), "", false);
                                        dc.JournalDetail_Update(crNoteNo, 0, 0, 0, Convert.ToDecimal(gvDetails.Rows[i].Cells[1].Text.ToString()) * -1, billId, false);

                                        dc.CashDetail_Update(null, billId, CurrentDt, -(Convert.ToDecimal(gvDetails.Rows[i].Cells[1].Text.ToString())), "Part", 0, Convert.ToBoolean(0), true, ClientId, crNoteNo, false, false);
                                        flgRecAvail = true;
                                        
                                    }
                                    else
                                    {
                                       mystr +=  gvDetails.Rows[i].Cells[0].Text.Trim().ToString()+",";
                                       lblNotTransfered.Text = "Error for bill Nos-";
                                    }
                                    //save grid record to table
                                     //dc.BadDebt_Update(gvDetails.Rows[i].Cells[0].ToString(), gvDetails.Rows[i].Cells[1].ToString(), Convert.ToDecimal(gvDetails.Rows[i].Cells[2].ToString()),DateTime.Now);

                                }
                            }
                        }

                    }
                    else
                    {
                        lblAccess.Text = "Enter Receipt No.";
                        lblAccess.Visible = true;
                        txtReceiptNo.Focus();
                    }

                    if (flgRecAvail == true)
                    {
                        if (lblNotTransfered.Text == "Error for bill Nos-")
                        {
                            lblNotTransfered.Visible = true;
                            lblNotTransfered.Text = mystr;
                            lblAccess.Text = "Imported With Errors";
                        }
                        else
                        {
                             lblNotTransfered.Visible = false;
                            lblAccess.Text = "Imported Successfully..";
                     
                        }
                        
                        lblAccess.Visible = true;
                    }
                    else
                    {
                        lnkUpdate.Enabled = true;
                    }
                }
            }
        }

    }
}