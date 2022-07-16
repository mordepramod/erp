using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;


namespace DESPLWEB
{
    public partial class LedgerUpdation : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false;
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");

                if (lblheading != null)
                {
                    lblheading.Text = "Ledger Updation";
                }
                BindCostCatagories();
            }
        }
        private void BindCostCatagories()
        {
            var CostCatagory = dc.CostCatagory_View("");
            ddl_CatagoryList.DataSource = CostCatagory;
            ddl_CatagoryList.DataTextField = "CostCatagory_Description";
            ddl_CatagoryList.DataValueField = "CostCatagory_Id";
            ddl_CatagoryList.DataBind();
            ddl_CatagoryList.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        public void DisplayCostCenterDescp()
        {
            if (ddl_CatagoryList.SelectedItem.Text != "---Select---")
            {
                gvCostCenter.DataSource = null;
                gvCostCenter.DataBind();
                var result = dc.CostCenter_View(0,Convert.ToInt32(ddl_CatagoryList.SelectedValue));
                int i = 0;
                foreach (var cost in result)
                {
                    AddRowCostCenter();
                    TextBox txt_CostcenterDescp = (TextBox)gvCostCenter.Rows[i].Cells[2].FindControl("txt_CostcenterDescp");
                    Label lblCostID = (Label)gvCostCenter.Rows[i].Cells[3].FindControl("lblCostID");
                    txt_CostcenterDescp.Text = Convert.ToString(cost.CostCenter_Description);
                    lblCostID.Text = Convert.ToString(cost.CostCenter_Id);
                    i++;
                }
                if (i == 0)
                {
                    AddRowCostCenter();
                }
            }
        }

        protected void lnkAddLedger_Click(object sender, CommandEventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            TextBox txt_CostcenterDescp = (TextBox)clickedRow.FindControl("txt_CostcenterDescp");
            Label lblCostID = (Label)clickedRow.FindControl("lblCostID");
            if (lblCostID.Text != "" && txt_CostcenterDescp.Text != "")
            {
                lnlSaveLedger.Enabled = true;
                lblErrMsg.Text = "";
                ModalPopupExtender2.Show();
                lblCostCenterName.Text = txt_CostcenterDescp.Text;
                lblCostcentId.Text = lblCostID.Text;
                txt_LedgerName.Text = "";
            }
        }
        protected void lnkViewLedger_Click(object sender, CommandEventArgs e)
        {
            if (gvCostCenter.Rows.Count > 0 && gvCostCenter.Visible == true)
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                TextBox txt_CostcenterDescp = (TextBox)clickedRow.FindControl("txt_CostcenterDescp");
                Label lblCostID = (Label)clickedRow.FindControl("lblCostID");

                if (txt_CostcenterDescp.Text != "" && lblCostID.Text != "")
                {
                    //string reportPath;
                    //string reportStr = "";
                    //StreamWriter sw;
                    //reportPath = Server.MapPath("~") + "\\report.html";
                    //sw = File.CreateText(reportPath);
                    //reportStr = RptLedegerView(txt_CostcenterDescp.Text, Convert.ToInt32(lblCostID.Text));
                    //sw.WriteLine(reportStr);
                    //sw.Close();
                    //NewWindows.Redirect("report.html", "_blank", "menubar=1,HEIGHT=600,WIDTH=820,scrollbars=yes,resizable=yes");
                    PrintHTMLReport rpt = new PrintHTMLReport();
                    rpt.LedegerView_Html(ddl_CatagoryList.SelectedItem.Text, Convert.ToInt32(ddl_CatagoryList.SelectedValue), txt_CostcenterDescp.Text, Convert.ToInt32(lblCostID.Text));
                }
            }
        }
        //protected string RptLedegerView(string costDesc, int CostID)
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

        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

        //    mySql += "<tr>" +
        //   "<td width='15%' align=left valign=top height=19><font size=2><b> Catagory Name  </b></font></td>" +
        //   "<td width='2%' height=19><font size=2>:</font></td>" +
        //   "<td width='40%' height=19><font size=2>" + ddl_CatagoryList.SelectedItem.Text + "</font></td>" +
        //   "</tr>";
        //    mySql += "<tr>" +
        //    "<td width='15%' align=left valign=top height=19><font size=2><b> Cost Center Description  </b></font></td>" +
        //    "<td width='2%' height=19><font size=2>:</font></td>" +
        //    "<td width='40%' height=19><font size=2>" + costDesc + "</font></td>" +
        //    "</tr>";
        //    mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
        //    mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=80% id=AutoNumber1>";

        //    int SrNo = 0;
        //    var data = dc.Ledger_View(Convert.ToInt32(ddl_CatagoryList.SelectedValue), CostID, false);
        //    foreach (var ledg in data)
        //    {
        //        if (SrNo == 0)
        //        {
        //            mySql += "<tr>";
        //            mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2><b> Sr No </b></font></td>";
        //            mySql += "<td width= 50% align=center valign=medium height=19 ><font size=2><b> Ledger Name </b></font></td>";
        //            mySql += "</tr>";
        //        }
        //        SrNo++;
        //        mySql += "<tr>";
        //        mySql += "<td width= 2% align=center valign=medium height=19 ><font size=2> " + SrNo + " </font></td>";
        //        mySql += "<td width= 50% align=center valign=medium height=19 ><font size=2> " + Convert.ToString(ledg.LedgerName_Description) + "   </font></td>";
        //        mySql += "</tr>";
        //    }
        //    if (SrNo == 0)
        //    {
        //        mySql += "<tr>";
        //        mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2> There are no records  ! </font></td>";
        //        mySql += "</tr>";
        //    }
        //    mySql += "</table>";
        //    mySql += "</td>";
        //    mySql += "<td>&nbsp;</td>";
        //    mySql += "</tr>";
        //    mySql += tempSql;
        //    mySql += "</table>";
        //    mySql += "</html>";
        //    return reportStr = mySql;
        //}
        protected void lnlSaveLedger_Click(object sender, CommandEventArgs e)
        {
            if (ddl_CatagoryList.SelectedValue != "0")
            {
                bool LedgerName_LedgerType_bit = false;
                string[] strng = txt_LedgerName.Text.Split(' ');
                foreach (string s in strng)
                {
                    if (s != "")
                    {
                        if (s.Equals("Advance", StringComparison.InvariantCultureIgnoreCase))
                        {
                            LedgerName_LedgerType_bit = true;
                            break;
                        }
                    }
                }
                if (txt_LedgerName.Text != "")
                {
                    dc.Ledger_Update(0,Convert.ToInt32(ddl_CatagoryList.SelectedValue), Convert.ToInt32(lblCostcentId.Text), LedgerName_LedgerType_bit, txt_LedgerName.Text);
                    lblErrMsg.Text = "Record Saved Sucessfully";
                    lblErrMsg.ForeColor = System.Drawing.Color.Green;
                    lnlSaveLedger.Enabled = false;
                }
            }
        }
        protected void ddl_CatagoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            lnk_SaveCostCenter.Enabled = true;
            DisplayCostCenterDescp();
        }

        protected void lnk_AddNewCatagory_Click(object sender, CommandEventArgs e)
        {
            lblCtagoryMsg.Visible = false;
            ModalPopupExtender1.Show();
            lnkSaveNewCatagory.Enabled = true;
            txt_CostCatagoryName.Text = string.Empty;
            txt_CostCatagoryName.Focus();
        }
        protected void lnkSaveNewCatagory_Click(object sender, EventArgs e)
        {
            try
            {
                bool valid = true;
                txt_CostCatagoryName.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txt_CostCatagoryName.Text);
                var catg = dc.CostCatagory_View(txt_CostCatagoryName.Text);
                foreach (var catagory in catg)
                {
                    if (catagory.CostCatagory_Description.Equals(txt_CostCatagoryName.Text, StringComparison.InvariantCultureIgnoreCase))
                    {
                        lblCtagoryMsg.Text = "Duplicate Catagory Name...";
                        lblCtagoryMsg.ForeColor = System.Drawing.Color.Red;
                        lblCtagoryMsg.Visible = true;
                        valid = false;
                        break;
                    }
                }
                if (valid == true)
                {
                    dc.CostCatagory_Update(0,txt_CostCatagoryName.Text);
                    lblCtagoryMsg.Text = "Catagory Name is saved sucessfully";
                    lblCtagoryMsg.ForeColor = System.Drawing.Color.Green;
                    lblCtagoryMsg.Visible = true;
                    lnkSaveNewCatagory.Enabled = false;
                    int ddlvalue = 0;
                    if (ddl_CatagoryList.SelectedValue != "---Select---")
                    {
                        ddlvalue = Convert.ToInt32(ddl_CatagoryList.SelectedValue);
                    }
                    BindCostCatagories();
                    ddl_CatagoryList.SelectedValue = ddlvalue.ToString();
                    lnk_SaveCostCenter.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
        protected void imgExit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
        }
        protected void lnkExitLedger_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Hide();
        }
        protected void imgCloseClick(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender2.Hide();
        }
        protected void lnkExitCatagory_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();
            txt_CostCatagoryName.Text = string.Empty;
            lblCtagoryMsg.Visible = false;
        }
        protected void ImgInsertCatagory_Click(object sender, CommandEventArgs e)
        {
            lnk_SaveCostCenter.Enabled = true;
            AddRowCostCenter();
        }

        #region  CostCenterGridView
        protected void AddRowCostCenter()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            if (ViewState["CostCatagoryTable"] != null)
            {
                GetDataCostCenter();
                dt = (DataTable)ViewState["CostCatagoryTable"];
            }
            else
            {

                dt.Columns.Add(new DataColumn("CostcenterDescp", typeof(string)));
                dt.Columns.Add(new DataColumn("lblCostID", typeof(string)));
                dt.Columns.Add(new DataColumn("CostCenter_Id", typeof(string)));
            }
            dr = dt.NewRow();

            dr["CostcenterDescp"] = string.Empty;
            dr["lblCostID"] = string.Empty;
            dr["CostCenter_Id"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["CostCatagoryTable"] = dt;
            gvCostCenter.DataSource = dt;
            gvCostCenter.DataBind();
            SetDataCostCenter();

        }
        protected void GetDataCostCenter()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("CostcenterDescp", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblCostID", typeof(string)));
            dtTable.Columns.Add(new DataColumn("CostCenter_Id", typeof(string)));

            for (int i = 0; i < gvCostCenter.Rows.Count; i++)
            {
                TextBox txt_CostcenterDescp = (TextBox)gvCostCenter.Rows[i].Cells[2].FindControl("txt_CostcenterDescp");
                Label lblCostID = (Label)gvCostCenter.Rows[i].Cells[3].FindControl("lblCostID");
                drRow = dtTable.NewRow();
                drRow["CostcenterDescp"] = txt_CostcenterDescp.Text;
                drRow["lblCostID"] = lblCostID.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CostCatagoryTable"] = dtTable;

        }
        protected void SetDataCostCenter()
        {
            DataTable dt = (DataTable)ViewState["CostCatagoryTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_CostcenterDescp = (TextBox)gvCostCenter.Rows[i].Cells[2].FindControl("txt_CostcenterDescp");
                Label lblCostID = (Label)gvCostCenter.Rows[i].Cells[3].FindControl("lblCostID");

                txt_CostcenterDescp.Text = dt.Rows[i]["CostcenterDescp"].ToString();
                lblCostID.Text = dt.Rows[i]["lblCostID"].ToString();
            }
        }
        protected void ImgDeleteCostCenter_Click(object sender, CommandEventArgs e)
        {
            if (gvCostCenter.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowCostCenter(gvr.RowIndex);
            }
        }
        protected void DeleteRowCostCenter(int rowIndex)
        {
            GetDataCostCenter();
            DataTable dt = ViewState["CostCatagoryTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CostCatagoryTable"] = dt;
            gvCostCenter.DataSource = dt;
            gvCostCenter.DataBind();
            SetDataCostCenter();
        }

        #endregion

        protected void lnk_SaveCostCenter_Click(object sender, EventArgs e)
        {
            if (ValidateCostCenter() == true)
            {
                bool valid = false;
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                dc.CostCenter_Update(0,Convert.ToInt32(ddl_CatagoryList.SelectedValue), "", true);
                for (int i = 0; i < gvCostCenter.Rows.Count; i++)
                {
                    TextBox txt_CostcenterDescp = (TextBox)gvCostCenter.Rows[i].Cells[2].FindControl("txt_CostcenterDescp");

                    if (Convert.ToInt32(ddl_CatagoryList.SelectedValue) != 0 && txt_CostcenterDescp.Text != "")
                    {
                        dc.CostCenter_Update(0,Convert.ToInt32(ddl_CatagoryList.SelectedValue), txt_CostcenterDescp.Text, false);
                        valid = true;
                    }
                }
                if (valid == true)
                {
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Text = "Record Saved Sucessfully";
                    lnk_SaveCostCenter.Enabled = false;
                    DisplayCostCenterDescp();
                }
            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }
        protected Boolean ValidateCostCenter()
        {
            
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            for (int i = 0; i < gvCostCenter.Rows.Count; i++)
            {
                TextBox txt_CostcenterDescp = (TextBox)gvCostCenter.Rows[i].Cells[2].FindControl("txt_CostcenterDescp");
                if (txt_CostcenterDescp.Text == "")
                {
                    lblMsg.Text = "Enter Cost Center description for Sr No. " + (i + 1) + ".";
                    txt_CostcenterDescp.Focus();
                    valid = false;
                    break;
                }
            }
            if (valid == false)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Visible = true;
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + dispalyMsg + "');", true);
            }
            else
            {
                lblMsg.Visible = false;
            }
            return valid;
        }
        protected void lnk_Exit_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }


    }
}