using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Ionic.Zip;

namespace DESPLWEB
{
    public partial class BillOutward : System.Web.UI.Page
    {   
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUserName.Text = Session["LoginUserName"].ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Outward Bill";
                LoadUserList();
                txt_FromDate.Text = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");
                txt_ToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                optOutwardPending.Checked = true;
                lblOutwardBySel.Visible = true;
                txtOutwardBySel.Visible = true;
                lblOutwardToSel.Visible = true;
                ddlUser.Visible = true;
                lblOutwardDateSel.Visible = true;
                txtOutwardDateSel.Visible = true;
                lnkOutwardBillSel.Visible = true;
                txtOutwardBySel.Text = lblUserName.Text;
            }
        }

        private void LoadUserList()
        {
            ddlUser.DataTextField = "USER_Name_var";
            ddlUser.DataValueField = "USER_Id";
            //var user = dc.User_View(0, 0, "", "", "");
            var user = dc.Driver_View(true);
            //var user = dc.Driver_View();
            ddlUser.DataSource = user;
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, "---Select---");
        }

        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            grdBills.Visible = true;
            DisplayBills();
            if (optOutwardPending.Checked == true)
            {
                grdBills.Columns[0].Visible = true;
                grdBills.Columns[4].Visible = false;
                grdBills.Columns[7].Visible = false;
                grdBills.Columns[8].Visible = false;
                grdBills.Columns[9].Visible = false;
                grdBills.Columns[10].Visible = false;
                grdBills.Columns[11].Visible = false;
            }
            else if (optOutwardCompleted.Checked == true)
            {
                grdBills.Columns[0].Visible = false;
                grdBills.Columns[4].Visible = true;
                grdBills.Columns[7].Visible = true;
                grdBills.Columns[8].Visible = true;
                grdBills.Columns[9].Visible = false;
                grdBills.Columns[10].Visible = true;
                grdBills.Columns[11].Visible = false;
            }
            else if (optPhysicalOutwardCompleted.Checked == true)
            {
                grdBills.Columns[0].Visible = false;
                grdBills.Columns[4].Visible = true;
                grdBills.Columns[7].Visible = false;
                grdBills.Columns[8].Visible = true;
                grdBills.Columns[9].Visible = true;
                grdBills.Columns[10].Visible = true;
                grdBills.Columns[11].Visible = true;
            }
        }

        public void DisplayBills()
        {
            DateTime Fromdate = DateTime.ParseExact(txt_FromDate.Text, "dd/MM/yyyy", null);
            DateTime Todate = DateTime.ParseExact(txt_ToDate.Text, "dd/MM/yyyy", null);
            byte outwStatus = 0;
            if (optOutwardPending.Checked == true)
                outwStatus = 0;
            else if (optOutwardCompleted.Checked == true)
                outwStatus = 1;
            else if (optPhysicalOutwardCompleted.Checked == true)
                outwStatus = 2;

            var Inward = dc.Bill_View_Outward(Fromdate, Todate, outwStatus).ToList();
            grdBills.DataSource = Inward;
            grdBills.DataBind();
            lblRecords.Text = "Total Records   :  " + grdBills.Rows.Count;
        }
        protected void grdBills_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                LinkButton lnkOutwardBill = (LinkButton)e.Row.FindControl("lnkOutwardBill");
                TextBox txtOutwardBy = (TextBox)e.Row.FindControl("txtOutwardBy");
                TextBox txtOutwardTo = (TextBox)e.Row.FindControl("txtOutwardTo");
                TextBox txtOutwardDate = (TextBox)e.Row.FindControl("txtOutwardDate");
                AjaxControlToolkit.CalendarExtender CalendarExtender4 = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("CalendarExtender4");
                TextBox txtAckDate = (TextBox)e.Row.FindControl("txtAckDate");
                FileUpload FileUploadAck = (FileUpload)e.Row.FindControl("FileUploadAck");
                Label lblAckDate = (Label)e.Row.FindControl("lblAckDate");
                //Label lblFileName = (Label)e.Row.FindControl("lblFileName");
                TextBox txtBookedDate = (TextBox)e.Row.FindControl("txtBookedDate");

                if (optOutwardCompleted.Checked == true)
                {
                    chkSelect.Visible = false;
                    txtOutwardBy.ReadOnly = true;
                    txtOutwardTo.ReadOnly = true;
                    txtOutwardDate.ReadOnly = true;
                    CalendarExtender4.Enabled = false;
                    //lnkOutwardBill.Visible = false;
                    lnkOutwardBill.Text = "Update Ack";
                }
                else if (optPhysicalOutwardCompleted.Checked == true)
                {
                    chkSelect.Visible = false;
                    txtOutwardBy.ReadOnly = true;
                    txtOutwardTo.ReadOnly = true;
                    txtOutwardDate.ReadOnly = true;
                    CalendarExtender4.Enabled = false;
                    //lnkOutwardBill.Visible = false;
                    lnkOutwardBill.Text = "Update";
                }
                else
                {
                    //lblAckDate.Visible = false;
                    //lblFileName.Visible = false;
                    lnkOutwardBill.Text = "Outward";
                }
            }
        }
        protected void grdBills_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strBillNo = Convert.ToString(e.CommandArgument);

            if (e.CommandName == "DownloadFile")
            {
                string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                string filePath = "D:/AckFiles/";
                if (cnStr.ToLower().Contains("mumbai") == true)
                    filePath += "Mumbai/";
                else if (cnStr.ToLower().Contains("nashik") == true)
                    filePath += "Nashik/";
                else if (cnStr.ToLower().Contains("metro") == true)
                    filePath += "Metro/";
                else
                    filePath += "Pune/";
                if (strBillNo.Contains(',') == false)
                {
                    filePath += strBillNo;
                    if (File.Exists(@filePath))
                    {
                        HttpResponse res = HttpContext.Current.Response;
                        res.Clear();
                        res.AppendHeader("content-disposition", "attachment; filename=" + filePath);
                        res.ContentType = "application/octet-stream";
                        res.WriteFile(filePath);
                        res.Flush();
                        res.End();
                    }
                }
                else
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                        zip.AddDirectoryByName("Files");
                        string[] strFiles = strBillNo.Split(',');

                        foreach (string filename in strFiles)
                        {
                            zip.AddFile(filePath + filename.Trim(), "Files");
                        }

                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.BufferOutput = false;
                        string zipName = String.Format("BillAck_{0}.zip", DateTime.Now.ToString("dd-MM-yyyy-HHmmss"));
                        HttpContext.Current.Response.ContentType = "application/zip";
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                        zip.Save(HttpContext.Current.Response.OutputStream);
                        //foreach (string filePath in filePaths)
                        //{
                        //    File.Delete(filePath);
                        //}
                        HttpContext.Current.Response.End();
                    }
                }
            }
            else if (e.CommandName == "OutwardBill")
            {
                GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int rowindex = grdrow.RowIndex;
                TextBox txtOutwardBy = (TextBox)grdBills.Rows[rowindex].FindControl("txtOutwardBy");
                TextBox txt_OutwardTo = (TextBox)grdBills.Rows[rowindex].FindControl("txtOutwardTo");
                TextBox txt_OutwardDate = (TextBox)grdBills.Rows[rowindex].FindControl("txtOutwardDate");
                TextBox txtAckDate = (TextBox)grdBills.Rows[rowindex].FindControl("txtAckDate");
                FileUpload FileUploadAck = (FileUpload)grdBills.Rows[rowindex].FindControl("FileUploadAck");
                //Label lblFileName = (Label)grdBills.Rows[rowindex].FindControl("lblFileName");
                LinkButton lnkDownloadFile = (LinkButton)grdBills.Rows[rowindex].FindControl("lnkDownloadFile");
                TextBox txtBookedDate = (TextBox)grdBills.Rows[rowindex].FindControl("txtBookedDate");
                
                bool updateFlag = true;
                if (optOutwardPending.Checked == true)
                {
                    txtOutwardBy.Text = lblUserName.Text;

                    if (txt_OutwardTo.Text.Trim() == "" && txt_OutwardDate.Text.Trim() == "")
                    {
                        if (ddlUser.SelectedIndex == 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select Outward To..');", true);
                            ddlUser.Focus();
                            updateFlag = false;
                        }
                        else if (txtOutwardDateSel.Text.Trim() == "")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Outward Date..');", true);
                            txtOutwardDateSel.Focus();
                            updateFlag = false;
                        }
                        else
                        {
                            txt_OutwardTo.Text = ddlUser.SelectedItem.Text;
                            txt_OutwardDate.Text = txtOutwardDateSel.Text;
                        }
                    }
                    else
                    {
                        if (txt_OutwardTo.Text.Trim() == "")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Outward To..');", true);
                            txt_OutwardTo.Focus();
                            updateFlag = false;
                        }
                        else if (txt_OutwardDate.Text.Trim() == "")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Outward Date..');", true);
                            txt_OutwardDate.Focus();
                            updateFlag = false;
                        }
                    }

                    if (updateFlag == true)
                    {
                        dc.Bill_Update_OutwardStatus(strBillNo, true);
                        dc.Outward_Update("DT", strBillNo, 0, "", "", txtOutwardBy.Text, txt_OutwardTo.Text, DateTime.ParseExact(txt_OutwardDate.Text, "dd/MM/yyyy", null), 0, "", null, "", false);
                        DisplayBills();
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Outward Successfully..');", true);
                    }
                }
                else if (optOutwardCompleted.Checked == true)
                {
                    if (FileUploadAck.HasFile == false && txtAckDate.Text.Trim() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('No data available..');", true);
                        txtAckDate.Focus();
                        updateFlag = false;
                    }
                    if (updateFlag == true)
                    {
                        DateTime? ackDate = null;
                        string filename = "";
                        if (txtAckDate.Text.Trim() != "")
                        {
                            ackDate = DateTime.ParseExact(txtAckDate.Text, "dd/MM/yyyy", null);
                        }
                        if (FileUploadAck.HasFile == true)
                        {
                            string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                            filename = Path.GetFileName(FileUploadAck.PostedFile.FileName);
                            string ext = Path.GetExtension(filename);

                            //string filePath = Server.MapPath("~/AckFiles/") + Path.GetFileName(filename);
                            string filePath = "D:/AckFiles/";
                            if (cnStr.ToLower().Contains("mumbai") == true)
                                filePath += "Mumbai/";
                            else if (cnStr.ToLower().Contains("nashik") == true)
                                filePath += "Nashik/";
                            else if (cnStr.ToLower().Contains("metro") == true)
                                filePath += "Metro/";
                            else
                                filePath += "Pune/";
                            if (!Directory.Exists(@filePath))
                                Directory.CreateDirectory(@filePath);
                            filePath += Path.GetFileName(filename);
                            FileUploadAck.PostedFile.SaveAs(filePath);
                            if (lnkDownloadFile.Text != "")
                            {
                                filename = lnkDownloadFile.Text + ", " + filename;
                            }
                        }
                        else if (lnkDownloadFile.Text != "")
                        {
                            filename = lnkDownloadFile.Text;
                        }
                        dc.Outward_Update_AckData("DT", strBillNo, ackDate, filename, null);
                        DisplayBills();
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Acknowledgement data saved successfully..');", true);
                    }
                }
                else if (optPhysicalOutwardCompleted.Checked == true)
                {
                    if (txtBookedDate.Text.Trim() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Booked Date..');", true);
                        txtBookedDate.Focus();
                        updateFlag = false;
                    }
                    string filename = "";
                    if (FileUploadAck.HasFile == true)
                    {
                        string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
                        filename = Path.GetFileName(FileUploadAck.PostedFile.FileName);
                        string ext = Path.GetExtension(filename);

                        //string filePath = Server.MapPath("~/AckFiles/") + Path.GetFileName(filename);
                        string filePath = "D:/AckFiles/";
                        if (cnStr.ToLower().Contains("mumbai") == true)
                            filePath += "Mumbai/";
                        else if (cnStr.ToLower().Contains("nashik") == true)
                            filePath += "Nashik/";
                        else if (cnStr.ToLower().Contains("metro") == true)
                            filePath += "Metro/";
                        else
                            filePath += "Pune/";
                        if (!Directory.Exists(@filePath))
                            Directory.CreateDirectory(@filePath);
                        filePath += Path.GetFileName(filename);
                        FileUploadAck.PostedFile.SaveAs(filePath);
                        if (lnkDownloadFile.Text != "")
                        {
                            filename = lnkDownloadFile.Text + ", " + filename;
                        }
                    }
                    else if (lnkDownloadFile.Text != "")
                    {
                        filename = lnkDownloadFile.Text;
                    }
                    if (updateFlag == true)
                    {
                        dc.Outward_Update_AckData("DT", strBillNo, null, filename, DateTime.ParseExact(txtBookedDate.Text, "dd/MM/yyyy", null));
                        DisplayBills();
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Date saved successfully..');", true);
                    }
                }
            }
        }
        protected void lnkOutwardBillSel_Click(object sender, EventArgs e)
        {
            bool selectedFlag = false;
            bool updateFlag = true;
            if (grdBills.Rows.Count > 0)
            {
                for (int i = 0; i < grdBills.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdBills.Rows[i].FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        selectedFlag = true;
                        break;
                    }
                }
            }
            if (selectedFlag == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select at least one row');", true);
                updateFlag = false;
            }
            else
            {
                if (optOutwardPending.Checked == true)
                {
                    txtOutwardBySel.Text = lblUserName.Text;
                    if (ddlUser.SelectedIndex == -1 || ddlUser.SelectedIndex == 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select Outward To..');", true);
                        ddlUser.Focus();
                        updateFlag = false;
                    }
                    else if (txtOutwardDateSel.Text.Trim() == "")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter Outward Date..');", true);
                        txtOutwardDateSel.Focus();
                        updateFlag = false;
                    }
                }
                
            }
            if (updateFlag == true)
            {
                for (int i = 0; i < grdBills.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdBills.Rows[i].FindControl("chkSelect");                    
                    if (chkSelect.Checked == true)
                    {
                        string strBillNo = grdBills.Rows[i].Cells[1].Text;
                        dc.Bill_Update_OutwardStatus(strBillNo, true);
                        dc.Outward_Update("DT", strBillNo, 0, "", "", txtOutwardBySel.Text, ddlUser.SelectedItem.Text, DateTime.ParseExact(txtOutwardDateSel.Text, "dd/MM/yyyy", null), 0, "", null, "", false);
                    }
                }
                DisplayBills();
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Outward Successfully..');", true);
            }
        }
        
        private void ClearBillList()
        {
            lblRecords.Text = "";
            grdBills.Visible = false;
            grdBills.DataSource = null;
            grdBills.DataBind();
        }

        protected void optOutwardPending_CheckedChanged(object sender, EventArgs e)
        {
            ClearBillList();
            lblOutwardBySel.Visible = true;
            txtOutwardBySel.Visible = true;
            lblOutwardToSel.Visible = true;
            ddlUser.Visible = true;
            lblOutwardDateSel.Visible = true;
            txtOutwardDateSel.Visible = true;
            lnkOutwardBillSel.Visible = true;
            lnkOutwardBillSel.Text = "Outward";
          //  lblBillDate.Visible = true;
           // lblBillDate.Text = "Bill Date";
          //  txt_FromDate.Visible = true;
          //  txt_ToDate.Visible = true;

        }

        protected void optOutwardCompleted_CheckedChanged(object sender, EventArgs e)
        {
            ClearBillList();
            lblOutwardBySel.Visible = false;
            txtOutwardBySel.Visible = false;
            lblOutwardToSel.Visible = false;
            ddlUser.Visible = false;
            lblOutwardDateSel.Visible = false;
            txtOutwardDateSel.Visible = false;
            lnkOutwardBillSel.Visible = false;
          //  lblBillDate.Visible = true;
         //   lblBillDate.Text = "Outward Date";
            //txt_FromDate.Visible = true;
            //txt_ToDate.Visible = true;
        }

        protected void optPhysicalOutwardCompleted_CheckedChanged(object sender, EventArgs e)
        {
            ClearBillList();
            lblOutwardBySel.Visible = false;
            txtOutwardBySel.Visible = false;
            lblOutwardToSel.Visible = false;
            ddlUser.Visible = false;
            lblOutwardDateSel.Visible = false;
            txtOutwardDateSel.Visible = false;
            lnkOutwardBillSel.Visible = false;
           // lblBillDate.Visible = true;
       //     lblBillDate.Text = "Outward Date";
            //txt_FromDate.Visible = true;
            //txt_ToDate.Visible = true;
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)grdBills.HeaderRow.FindControl("chkSelectAll");
            foreach (GridViewRow row in grdBills.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkSelect");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if(grdBills.Rows.Count>0)
            {
                string optName = "";
                if (optOutwardPending.Checked)
                    optName = "Outward_Pending";
                else if (optOutwardCompleted.Checked)
                    optName = "Physical_Outward_Pending";
                else if (optPhysicalOutwardCompleted.Checked)
                    optName = "Physical_Outward_Completed";

                PrintGrid.PrintGridViewBillOutward(grdBills,optName.Replace("_"," ").ToString(),optName);
            }
        }
    }

}
