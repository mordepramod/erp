using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Net;

namespace DESPLWEB
{
    public partial class Brick_Report : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
                string strReq = "";
                strReq = Request.RawUrl;
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);
                if (!strReq.Equals(""))
                {
                    strReq = obj.Decrypt(strReq);
                    if (strReq.Contains("=") == false)
                    {
                        Session.Abandon();
                        Response.Redirect("Login.aspx");
                    }
                    string[] arrMsgs = strReq.Split('&');
                    string[] arrIndMsg;
                    arrIndMsg = arrMsgs[0].Split('=');
                    txtRecordType.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[1].Split('=');
                    lblrecno.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[2].Split('=');
                    txtRefNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[3].Split('=');
                    lblStatus.Text = arrIndMsg[1].ToString().Trim();
                }

                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Brick - Report Entry";

                //txtEntdChkdBy.Text = Session["LoginUserName"].ToString();
               // txtRefNo.Text = Session["ReferenceNo"].ToString();
                if (lblStatus.Text ==  "Enter")
                    lblStatus.Text = "Enter";
                else if (lblStatus.Text == "Check")
                    lblStatus.Text = "Check";

                LoadReferenceNoList();
                LoadApprovedBy();
                DisplayBrickDetails();
            }
        }

        private void LoadReferenceNoList()
        {
            byte reportStatus = 0;
            if (lblStatus.Text == "Enter")
                reportStatus = 1;
            else if (lblStatus.Text == "Check")
                reportStatus = 2;

            //var reportList = dc.ReportStatus_View("Brick Testing", null, null, 0, 0, 0, "", 0, reportStatus);
            var reportList = dc.BrickInward_View("", reportStatus);
            ddlRefNo.DataTextField = "BTINWD_ReferenceNo_var";
            ddlRefNo.DataSource = reportList;
            ddlRefNo.DataBind();
            ddlRefNo.Items.Insert(0, new ListItem("---Select---","0"));
            ddlRefNo.Items.Remove(txtRefNo.Text);
        }
        
        private void LoadApprovedBy()
        {
            byte testBit = 0;
            byte apprBit = 0;
            if (lblStatus.Text == "Enter")
            {
                testBit = 1;
                apprBit = 0;
            }
            else if (lblStatus.Text == "Check")
            {
                testBit = 0;
                apprBit = 1;
            }

            ddlTestdApprdBy.DataTextField = "USER_Name_var";
            ddlTestdApprdBy.DataValueField = "USER_Id";
            var apprUser = dc.ReportStatus_View("", null, null, 0, testBit, apprBit, "", 0, 0, 0);
            ddlTestdApprdBy.DataSource = apprUser;
            ddlTestdApprdBy.DataBind();
            ddlTestdApprdBy.Items.Insert(0, "---Select---");
        }
        
        protected void txtDARows_TextChanged(object sender, EventArgs e)
        {
            DARowsChanged();
        }
        
        private void DARowsChanged()
        {
            if (txtDARows.Text != "")
            {
                if (Convert.ToInt32(txtDARows.Text) < grdDA.Rows.Count)
                {
                    for (int i = grdDA.Rows.Count; i > Convert.ToInt32(txtDARows.Text); i--)
                    {
                        if (grdDA.Rows.Count > 1)
                        {
                            DeleteRowDA(i - 1);
                            DeleteRowDACal(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtDARows.Text) > grdDA.Rows.Count)
                {
                    for (int i = grdDA.Rows.Count + 1; i <= Convert.ToInt32(txtDARows.Text); i++)
                    {
                        AddRowDA();
                        AddRowDACal();
                    }
                }
            }
        }

        protected void txtWAQuantity_TextChanged(object sender, EventArgs e)
        {
            WARowsChanged();
        }

        private void WARowsChanged()
        {
            if (txtWAQuantity.Text != "")
            {
                if (Convert.ToInt32(txtWAQuantity.Text) < grdWA.Rows.Count)
                {
                    for (int i = grdWA.Rows.Count; i > Convert.ToInt32(txtWAQuantity.Text); i--)
                    {
                        if (grdWA.Rows.Count > 1)
                        {
                            DeleteRowWA(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtWAQuantity.Text) > grdWA.Rows.Count)
                {
                    for (int i = grdWA.Rows.Count + 1; i <= Convert.ToInt32(txtWAQuantity.Text); i++)
                    {
                        AddRowWA();
                    }
                }
            }
        }

        protected void txtCSQuantity_TextChanged(object sender, EventArgs e)
        {
            CSRowsChanged();
        }

        private void CSRowsChanged()
        {
            if (txtCSQuantity.Text != "")
            {
                if (Convert.ToInt32(txtCSQuantity.Text) < grdCS.Rows.Count)
                {
                    for (int i = grdCS.Rows.Count; i > Convert.ToInt32(txtCSQuantity.Text); i--)
                    {
                        if (grdCS.Rows.Count > 1)
                        {
                            DeleteRowCS(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtCSQuantity.Text) > grdCS.Rows.Count)
                {
                    for (int i = grdCS.Rows.Count + 1; i <= Convert.ToInt32(txtCSQuantity.Text); i++)
                    {
                        AddRowCS();
                    }
                }
            }
        }

        protected void txtETQuantity_TextChanged(object sender, EventArgs e)
        {
            ETRowsChanged();
        }

        private void ETRowsChanged()
        {
            if (txtETQuantity.Text != "")
            {
                if (Convert.ToInt32(txtETQuantity.Text) < grdET.Rows.Count)
                {
                    for (int i = grdET.Rows.Count; i > Convert.ToInt32(txtETQuantity.Text); i--)
                    {
                        if (grdET.Rows.Count > 1)
                        {
                            DeleteRowET(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtETQuantity.Text) > grdET.Rows.Count)
                {
                    for (int i = grdET.Rows.Count + 1; i <= Convert.ToInt32(txtETQuantity.Text); i++)
                    {
                        AddRowET();
                    }
                }
            }
        }

        protected void txtDSQuantity_TextChanged(object sender, EventArgs e)
        {
            DSRowsChanged();
        }

        private void DSRowsChanged()
        {
            if (txtDSQuantity.Text != "")
            {
                if (Convert.ToInt32(txtDSQuantity.Text) < grdDS.Rows.Count)
                {
                    for (int i = grdDS.Rows.Count; i > Convert.ToInt32(txtDSQuantity.Text); i--)
                    {
                        if (grdDS.Rows.Count > 1)
                        {
                            DeleteRowDS(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtDSQuantity.Text) > grdDS.Rows.Count)
                {
                    for (int i = grdDS.Rows.Count + 1; i <= Convert.ToInt32(txtDSQuantity.Text); i++)
                    {
                        AddRowDS();
                    }
                }
            }
        }

        #region add/delete rows grdDA grid
        protected void AddRowDA()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["DATable"] != null)
            {
                GetCurrentDataDA();
                dt = (DataTable)ViewState["DATable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtDAIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtL1", typeof(string)));
                dt.Columns.Add(new DataColumn("txtL2", typeof(string)));
                dt.Columns.Add(new DataColumn("txtL3", typeof(string)));
                dt.Columns.Add(new DataColumn("txtW1", typeof(string)));
                dt.Columns.Add(new DataColumn("txtW2", typeof(string)));
                dt.Columns.Add(new DataColumn("txtW3", typeof(string)));
                dt.Columns.Add(new DataColumn("txtH1", typeof(string)));
                dt.Columns.Add(new DataColumn("txtH2", typeof(string)));
                dt.Columns.Add(new DataColumn("txtH3", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtDAIdMark"] = txtIdMark.Text;
            dr["txtL1"] = string.Empty;
            dr["txtL2"] = string.Empty;
            dr["txtL3"] = string.Empty;
            dr["txtW1"] = string.Empty;
            dr["txtW2"] = string.Empty;
            dr["txtW3"] = string.Empty;
            dr["txtH1"] = string.Empty;
            dr["txtH2"] = string.Empty;
            dr["txtH3"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["DATable"] = dt;
            grdDA.DataSource = dt;
            grdDA.DataBind();
            SetPreviousDataDA();
        }
        protected void DeleteRowDA(int rowIndex)
        {
            GetCurrentDataDA();
            DataTable dt = ViewState["DATable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["DATable"] = dt;
            grdDA.DataSource = dt;
            grdDA.DataBind();
            SetPreviousDataDA();
        }
        protected void SetPreviousDataDA()
        {
            DataTable dt = (DataTable)ViewState["DATable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtDAIdMark = (TextBox)grdDA.Rows[i].FindControl("txtDAIdMark");
                TextBox txtL1 = (TextBox)grdDA.Rows[i].FindControl("txtL1");
                TextBox txtL2 = (TextBox)grdDA.Rows[i].FindControl("txtL2");
                TextBox txtL3 = (TextBox)grdDA.Rows[i].FindControl("txtL3");
                TextBox txtW1 = (TextBox)grdDA.Rows[i].FindControl("txtW1");
                TextBox txtW2 = (TextBox)grdDA.Rows[i].FindControl("txtW2");
                TextBox txtW3 = (TextBox)grdDA.Rows[i].FindControl("txtW3");
                TextBox txtH1 = (TextBox)grdDA.Rows[i].FindControl("txtH1");
                TextBox txtH2 = (TextBox)grdDA.Rows[i].FindControl("txtH2");
                TextBox txtH3 = (TextBox)grdDA.Rows[i].FindControl("txtH3");

                txtDAIdMark.Text = dt.Rows[i]["txtDAIdMark"].ToString();
                txtL1.Text = dt.Rows[i]["txtL1"].ToString();
                txtL2.Text = dt.Rows[i]["txtL2"].ToString();
                txtL3.Text = dt.Rows[i]["txtL3"].ToString();
                txtW1.Text = dt.Rows[i]["txtW1"].ToString();
                txtW2.Text = dt.Rows[i]["txtW2"].ToString();
                txtW3.Text = dt.Rows[i]["txtW3"].ToString();
                txtH1.Text = dt.Rows[i]["txtH1"].ToString();
                txtH2.Text = dt.Rows[i]["txtH2"].ToString();
                txtH3.Text = dt.Rows[i]["txtH3"].ToString();

            }
        }
        protected void GetCurrentDataDA()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtDAIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtL1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtL2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtL3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtW1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtW2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtW3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtH1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtH2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtH3", typeof(string))); 
            for (int i = 0; i < grdDA.Rows.Count; i++)
            {
                TextBox txtDAIdMark = (TextBox)grdDA.Rows[i].Cells[1].FindControl("txtDAIdMark");
                TextBox txtL1 = (TextBox)grdDA.Rows[i].Cells[2].FindControl("txtL1");
                TextBox txtL2 = (TextBox)grdDA.Rows[i].Cells[2].FindControl("txtL2");
                TextBox txtL3 = (TextBox)grdDA.Rows[i].Cells[2].FindControl("txtL3");
                TextBox txtW1 = (TextBox)grdDA.Rows[i].Cells[3].FindControl("txtW1");
                TextBox txtW2 = (TextBox)grdDA.Rows[i].Cells[3].FindControl("txtW2");
                TextBox txtW3 = (TextBox)grdDA.Rows[i].Cells[3].FindControl("txtW3");
                TextBox txtH1 = (TextBox)grdDA.Rows[i].Cells[4].FindControl("txtH1");
                TextBox txtH2 = (TextBox)grdDA.Rows[i].Cells[4].FindControl("txtH2");
                TextBox txtH3 = (TextBox)grdDA.Rows[i].Cells[4].FindControl("txtH3");

                drRow = dtTable.NewRow();
                drRow["txtDAIdMark"] = txtDAIdMark.Text;
                drRow["txtL1"] = txtL1.Text;
                drRow["txtL2"] = txtL2.Text;
                drRow["txtL3"] = txtL3.Text;
                drRow["txtW1"] = txtW1.Text;
                drRow["txtW2"] = txtW2.Text;
                drRow["txtW3"] = txtW3.Text;
                drRow["txtH1"] = txtH1.Text;
                drRow["txtH2"] = txtH2.Text;
                drRow["txtH3"] = txtH3.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["DATable"] = dtTable;

        }
        #endregion

        #region add/delete rows grdDACal grid
        protected void AddRowDACal()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["DACalTable"] != null)
            {
                GetCurrentDataDACal();
                dt = (DataTable)ViewState["DACalTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtDACalIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDACalLength", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDACalWidth", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDACalHeight", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDACalAvgLength", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDACalAvgWidth", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDACalAvgHeight", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtDACalIdMark"] = string.Empty;
            dr["txtDACalLength"] = string.Empty;
            dr["txtDACalWidth"] = string.Empty;
            dr["txtDACalHeight"] = string.Empty;
            dr["txtDACalAvgLength"] = string.Empty;
            dr["txtDACalAvgWidth"] = string.Empty;
            dr["txtDACalAvgHeight"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["DACalTable"] = dt;
            grdDACal.DataSource = dt;
            grdDACal.DataBind();
            SetPreviousDataDACal();
        }
        protected void DeleteRowDACal(int rowIndex)
        {
            GetCurrentDataDACal();
            DataTable dt = ViewState["DACalTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["DACalTable"] = dt;
            grdDACal.DataSource = dt;
            grdDACal.DataBind();
            SetPreviousDataDACal();
        }
        protected void SetPreviousDataDACal()
        {
            DataTable dt = (DataTable)ViewState["DACalTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtDACalIdMark = (TextBox)grdDACal.Rows[i].FindControl("txtDACalIdMark");
                TextBox txtDACalLength = (TextBox)grdDACal.Rows[i].FindControl("txtDACalLength");
                TextBox txtDACalWidth = (TextBox)grdDACal.Rows[i].FindControl("txtDACalWidth");
                TextBox txtDACalHeight = (TextBox)grdDACal.Rows[i].FindControl("txtDACalHeight");
                TextBox txtDACalAvgLength = (TextBox)grdDACal.Rows[i].FindControl("txtDACalAvgLength");
                TextBox txtDACalAvgWidth = (TextBox)grdDACal.Rows[i].FindControl("txtDACalAvgWidth");
                TextBox txtDACalAvgHeight = (TextBox)grdDACal.Rows[i].FindControl("txtDACalAvgHeight");

                txtDACalIdMark.Text = dt.Rows[i]["txtDACalIdMark"].ToString();
                txtDACalLength.Text = dt.Rows[i]["txtDACalLength"].ToString();
                txtDACalWidth.Text = dt.Rows[i]["txtDACalWidth"].ToString();
                txtDACalHeight.Text = dt.Rows[i]["txtDACalHeight"].ToString();
                txtDACalAvgLength.Text = dt.Rows[i]["txtDACalAvgLength"].ToString();
                txtDACalAvgWidth.Text = dt.Rows[i]["txtDACalAvgWidth"].ToString();
                txtDACalAvgHeight.Text = dt.Rows[i]["txtDACalAvgHeight"].ToString();
            }
        }
        protected void GetCurrentDataDACal()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtDACalIdMark", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDACalLength", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDACalWidth", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDACalHeight", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDACalAvgLength", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDACalAvgWidth", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDACalAvgHeight", typeof(string)));
            for (int i = 0; i < grdDACal.Rows.Count; i++)
            {
                TextBox txtDACalIdMark = (TextBox)grdDACal.Rows[i].FindControl("txtDACalIdMark");
                TextBox txtDACalLength = (TextBox)grdDACal.Rows[i].FindControl("txtDACalLength");
                TextBox txtDACalWidth = (TextBox)grdDACal.Rows[i].FindControl("txtDACalWidth");
                TextBox txtDACalHeight = (TextBox)grdDACal.Rows[i].FindControl("txtDACalHeight");
                TextBox txtDACalAvgLength = (TextBox)grdDACal.Rows[i].FindControl("txtDACalAvgLength");
                TextBox txtDACalAvgWidth = (TextBox)grdDACal.Rows[i].FindControl("txtDACalAvgWidth");
                TextBox txtDACalAvgHeight = (TextBox)grdDACal.Rows[i].FindControl("txtDACalAvgHeight");

                dr = dt.NewRow();
                dr["txtDACalIdMark"] = txtDACalIdMark.Text;
                dr["txtDACalLength"] = txtDACalLength.Text;
                dr["txtDACalWidth"] = txtDACalWidth.Text;
                dr["txtDACalHeight"] = txtDACalHeight.Text;
                dr["txtDACalAvgLength"] = txtDACalAvgLength.Text;
                dr["txtDACalAvgWidth"] = txtDACalAvgWidth.Text;
                dr["txtDACalAvgHeight"] = txtDACalAvgHeight.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["DACalTable"] = dt;

        }
        #endregion

        #region add/delete rows grdRemark grid
        protected void AddRowRemark()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["RemarkTable"] != null)
            {
                GetCurrentDataRemark();
                dt = (DataTable)ViewState["RemarkTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtRemark", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtRemark"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["RemarkTable"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataRemark();
        }
        protected void DeleteRowRemark(int rowIndex)
        {
            GetCurrentDataRemark();
            DataTable dt = ViewState["RemarkTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["RemarkTable"] = dt;
            grdRemark.DataSource = dt;
            grdRemark.DataBind();
            SetPreviousDataRemark();
        }
        protected void SetPreviousDataRemark()
        {
            DataTable dt = (DataTable)ViewState["RemarkTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtRemark = (TextBox)grdRemark.Rows[i].FindControl("txtRemark");
                txtRemark.Text = dt.Rows[i]["txtRemark"].ToString();
            }
        }
        protected void GetCurrentDataRemark()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtRemark", typeof(string)));
            for (int i = 0; i < grdRemark.Rows.Count; i++)
            {
                TextBox txtRemark = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txtRemark");

                dr = dt.NewRow();
                dr["txtRemark"] = txtRemark.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["RemarkTable"] = dt;

        }
        protected void imgBtnAddRow_Click(object sender, EventArgs e)
        {
            AddRowRemark();
        }
        protected void imgBtnDeleteRow_Click(object sender, EventArgs e)
        {
            if (grdRemark.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowRemark(gvr.RowIndex);                
            }
        }
        #endregion

        #region add/delete rows grdWA grid
        protected void AddRowWA()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["WATable"] != null)
            {
                GetCurrentDataWA();
                dt = (DataTable)ViewState["WATable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtWAIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWADryWt", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWAWetWt", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWA", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWAAvg", typeof(string)));                
            }

            dr = dt.NewRow();
            dr["txtWAIdMark"] = txtIdMark.Text;
            dr["txtWADryWt"] = string.Empty;
            dr["txtWAWetWt"] = string.Empty;
            dr["txtWA"] = string.Empty;
            dr["txtWAAvg"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["WATable"] = dt;
            grdWA.DataSource = dt;
            grdWA.DataBind();
            SetPreviousDataWA();
        }
        protected void DeleteRowWA(int rowIndex)
        {
            GetCurrentDataWA();
            DataTable dt = ViewState["WATable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["WATable"] = dt;
            grdWA.DataSource = dt;
            grdWA.DataBind();
            SetPreviousDataWA();
        }
        protected void SetPreviousDataWA()
        {
            DataTable dt = (DataTable)ViewState["WATable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtWAIdMark = (TextBox)grdWA.Rows[i].FindControl("txtWAIdMark");
                TextBox txtWADryWt = (TextBox)grdWA.Rows[i].FindControl("txtWADryWt");
                TextBox txtWAWetWt = (TextBox)grdWA.Rows[i].FindControl("txtWAWetWt");
                TextBox txtWA = (TextBox)grdWA.Rows[i].FindControl("txtWA");
                TextBox txtWAAvg = (TextBox)grdWA.Rows[i].FindControl("txtWAAvg");

                txtWAIdMark.Text = dt.Rows[i]["txtWAIdMark"].ToString();
                txtWADryWt.Text = dt.Rows[i]["txtWADryWt"].ToString();
                txtWAWetWt.Text = dt.Rows[i]["txtWAWetWt"].ToString();
                txtWA.Text = dt.Rows[i]["txtWA"].ToString();
                txtWAAvg.Text = dt.Rows[i]["txtWAAvg"].ToString();
            }
        }
        protected void GetCurrentDataWA()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtWAIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWADryWt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWAWetWt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWA", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWAAvg", typeof(string)));
            for (int i = 0; i < grdWA.Rows.Count; i++)
            {
                TextBox txtWAIdMark = (TextBox)grdWA.Rows[i].FindControl("txtWAIdMark");
                TextBox txtWADryWt = (TextBox)grdWA.Rows[i].FindControl("txtWADryWt");
                TextBox txtWAWetWt = (TextBox)grdWA.Rows[i].FindControl("txtWAWetWt");
                TextBox txtWA = (TextBox)grdWA.Rows[i].FindControl("txtWA");
                TextBox txtWAAvg = (TextBox)grdWA.Rows[i].FindControl("txtWAAvg");

                drRow = dtTable.NewRow();
                drRow["txtWAIdMark"] = txtWAIdMark.Text;
                drRow["txtWADryWt"] = txtWADryWt.Text;
                drRow["txtWAWetWt"] = txtWAWetWt.Text;
                drRow["txtWA"] = txtWA.Text;
                drRow["txtWAAvg"] = txtWAAvg.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["WATable"] = dtTable;

        }
        #endregion

        #region add/delete rows grdCS grid
        protected void AddRowCS()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CSTable"] != null)
            {
                GetCurrentDataCS();
                dt = (DataTable)ViewState["CSTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtCSIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCSLength", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCSWidth", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCSLoad", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCSArea", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCSStrength", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCSAvg", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtCSIdMark"] = txtIdMark.Text;
            dr["txtCSLength"] = string.Empty;
            dr["txtCSWidth"] = string.Empty;
            dr["txtCSLoad"] = string.Empty;
            dr["txtCSArea"] = string.Empty;
            dr["txtCSStrength"] = string.Empty;
            dr["txtCSAvg"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CSTable"] = dt;
            grdCS.DataSource = dt;
            grdCS.DataBind();
            SetPreviousDataCS();
        }
        protected void DeleteRowCS(int rowIndex)
        {
            GetCurrentDataCS();
            DataTable dt = ViewState["CSTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CSTable"] = dt;
            grdCS.DataSource = dt;
            grdCS.DataBind();
            SetPreviousDataCS();
        }
        protected void SetPreviousDataCS()
        {
            DataTable dt = (DataTable)ViewState["CSTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtCSIdMark = (TextBox)grdCS.Rows[i].FindControl("txtCSIdMark");
                TextBox txtCSLength = (TextBox)grdCS.Rows[i].FindControl("txtCSLength");
                TextBox txtCSWidth = (TextBox)grdCS.Rows[i].FindControl("txtCSWidth");
                TextBox txtCSLoad = (TextBox)grdCS.Rows[i].FindControl("txtCSLoad");
                TextBox txtCSArea = (TextBox)grdCS.Rows[i].FindControl("txtCSArea");
                TextBox txtCSStrength = (TextBox)grdCS.Rows[i].FindControl("txtCSStrength");
                TextBox txtCSAvg = (TextBox)grdCS.Rows[i].FindControl("txtCSAvg");

                txtCSIdMark.Text = dt.Rows[i]["txtCSIdMark"].ToString();
                txtCSLength.Text = dt.Rows[i]["txtCSLength"].ToString();
                txtCSWidth.Text = dt.Rows[i]["txtCSWidth"].ToString();
                txtCSLoad.Text = dt.Rows[i]["txtCSLoad"].ToString();
                txtCSArea.Text = dt.Rows[i]["txtCSArea"].ToString();
                txtCSStrength.Text = dt.Rows[i]["txtCSStrength"].ToString();
                txtCSAvg.Text = dt.Rows[i]["txtCSAvg"].ToString();
            }
        }
        protected void GetCurrentDataCS()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtCSIdMark", typeof(string)));
            dt.Columns.Add(new DataColumn("txtCSLength", typeof(string)));
            dt.Columns.Add(new DataColumn("txtCSWidth", typeof(string)));
            dt.Columns.Add(new DataColumn("txtCSLoad", typeof(string)));
            dt.Columns.Add(new DataColumn("txtCSArea", typeof(string)));
            dt.Columns.Add(new DataColumn("txtCSStrength", typeof(string)));
            dt.Columns.Add(new DataColumn("txtCSAvg", typeof(string)));
            for (int i = 0; i < grdCS.Rows.Count; i++)
            {
                TextBox txtCSIdMark = (TextBox)grdCS.Rows[i].FindControl("txtCSIdMark");
                TextBox txtCSLength = (TextBox)grdCS.Rows[i].FindControl("txtCSLength");
                TextBox txtCSWidth = (TextBox)grdCS.Rows[i].FindControl("txtCSWidth");
                TextBox txtCSLoad = (TextBox)grdCS.Rows[i].FindControl("txtCSLoad");
                TextBox txtCSArea = (TextBox)grdCS.Rows[i].FindControl("txtCSArea");
                TextBox txtCSStrength = (TextBox)grdCS.Rows[i].FindControl("txtCSStrength");
                TextBox txtCSAvg = (TextBox)grdCS.Rows[i].FindControl("txtCSAvg");

                dr = dt.NewRow();
                dr["txtCSIdMark"] = txtCSIdMark.Text;
                dr["txtCSLength"] = txtCSLength.Text;
                dr["txtCSWidth"] = txtCSWidth.Text;
                dr["txtCSLoad"] = txtCSLoad.Text;
                dr["txtCSArea"] = txtCSArea.Text;
                dr["txtCSStrength"] = txtCSStrength.Text;
                dr["txtCSAvg"] = txtCSAvg.Text;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["CSTable"] = dt;

        }
        #endregion

        #region add/delete rows grdET grid
        protected void AddRowET()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["ETTable"] != null)
            {
                GetCurrentDataET();
                dt = (DataTable)ViewState["ETTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtETIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlETObservations", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtETIdMark"] = txtIdMark.Text;
            dr["ddlETObservations"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["ETTable"] = dt;
            grdET.DataSource = dt;
            grdET.DataBind();
            SetPreviousDataET();
        }
        protected void DeleteRowET(int rowIndex)
        {
            GetCurrentDataET();
            DataTable dt = ViewState["ETTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["ETTable"] = dt;
            grdET.DataSource = dt;
            grdET.DataBind();
            SetPreviousDataET();
        }
        protected void SetPreviousDataET()
        {
            DataTable dt = (DataTable)ViewState["ETTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtETIdMark = (TextBox)grdET.Rows[i].FindControl("txtETIdMark");
                DropDownList ddlETObservations = (DropDownList)grdET.Rows[i].FindControl("ddlETObservations");

                txtETIdMark.Text = dt.Rows[i]["txtETIdMark"].ToString();
                ddlETObservations.SelectedValue = dt.Rows[i]["ddlETObservations"].ToString();
            }
        }
        protected void GetCurrentDataET()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtETIdMark", typeof(string)));
            dt.Columns.Add(new DataColumn("ddlETObservations", typeof(string)));
            for (int i = 0; i < grdET.Rows.Count; i++)
            {
                TextBox txtETIdMark = (TextBox)grdET.Rows[i].FindControl("txtETIdMark");
                DropDownList ddlETObservations = (DropDownList)grdET.Rows[i].FindControl("ddlETObservations");

                dr = dt.NewRow();
                dr["txtETIdMark"] = txtETIdMark.Text;
                dr["ddlETObservations"] = ddlETObservations.SelectedValue;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["ETTable"] = dt;

        }
        #endregion

        #region add/delete rows grdDS grid
        protected void AddRowDS()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["DSTable"] != null)
            {
                GetCurrentDataDS();
                dt = (DataTable)ViewState["DSTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtDSIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDSLength", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDSWidth", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDSThickness", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDSOvenDryWt", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDSVolume", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDS", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDSAvg", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtDSIdMark"] = txtIdMark.Text;
            dr["txtDSLength"] = string.Empty;
            dr["txtDSWidth"] = string.Empty;
            dr["txtDSThickness"] = string.Empty;
            dr["txtDSOvenDryWt"] = string.Empty;
            dr["txtDSVolume"] = string.Empty;
            dr["txtDS"] = string.Empty;
            dr["txtDSAvg"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["DSTable"] = dt;
            grdDS.DataSource = dt;
            grdDS.DataBind();
            SetPreviousDataDS();
        }
        protected void DeleteRowDS(int rowIndex)
        {
            GetCurrentDataDS();
            DataTable dt = ViewState["DSTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["DSTable"] = dt;
            grdDS.DataSource = dt;
            grdDS.DataBind();
            SetPreviousDataDS();
        }
        protected void SetPreviousDataDS()
        {
            DataTable dt = (DataTable)ViewState["DSTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtDSIdMark = (TextBox)grdDS.Rows[i].FindControl("txtDSIdMark");
                TextBox txtDSLength = (TextBox)grdDS.Rows[i].FindControl("txtDSLength");
                TextBox txtDSWidth = (TextBox)grdDS.Rows[i].FindControl("txtDSWidth");
                TextBox txtDSThickness = (TextBox)grdDS.Rows[i].FindControl("txtDSThickness");
                TextBox txtDSOvenDryWt = (TextBox)grdDS.Rows[i].FindControl("txtDSOvenDryWt");
                TextBox txtDSVolume = (TextBox)grdDS.Rows[i].FindControl("txtDSVolume");
                TextBox txtDS = (TextBox)grdDS.Rows[i].FindControl("txtDS");
                TextBox txtDSAvg = (TextBox)grdDS.Rows[i].FindControl("txtDSAvg");

                txtDSIdMark.Text = dt.Rows[i]["txtDSIdMark"].ToString();
                txtDSLength.Text = dt.Rows[i]["txtDSLength"].ToString();
                txtDSWidth.Text = dt.Rows[i]["txtDSWidth"].ToString();
                txtDSThickness.Text = dt.Rows[i]["txtDSThickness"].ToString();
                txtDSOvenDryWt.Text = dt.Rows[i]["txtDSOvenDryWt"].ToString();
                txtDSVolume.Text = dt.Rows[i]["txtDSVolume"].ToString();
                txtDS.Text = dt.Rows[i]["txtDS"].ToString();
                txtDSAvg.Text = dt.Rows[i]["txtDSAvg"].ToString();
            }
        }
        protected void GetCurrentDataDS()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtDSIdMark", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDSLength", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDSWidth", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDSThickness", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDSOvenDryWt", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDSVolume", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDS", typeof(string)));
            dt.Columns.Add(new DataColumn("txtDSAvg", typeof(string)));
            for (int i = 0; i < grdDS.Rows.Count; i++)
            {
                TextBox txtDSIdMark = (TextBox)grdDS.Rows[i].FindControl("txtDSIdMark");
                TextBox txtDSLength = (TextBox)grdDS.Rows[i].FindControl("txtDSLength");
                TextBox txtDSWidth = (TextBox)grdDS.Rows[i].FindControl("txtDSWidth");
                TextBox txtDSThickness = (TextBox)grdDS.Rows[i].FindControl("txtDSThickness");
                TextBox txtDSOvenDryWt = (TextBox)grdDS.Rows[i].FindControl("txtDSOvenDryWt");
                TextBox txtDSVolume = (TextBox)grdDS.Rows[i].FindControl("txtDSVolume");
                TextBox txtDS = (TextBox)grdDS.Rows[i].FindControl("txtDS");
                TextBox txtDSAvg = (TextBox)grdDS.Rows[i].FindControl("txtDSAvg");

                dr = dt.NewRow();
                dr["txtDSIdMark"] = txtDSIdMark.Text;
                dr["txtDSLength"] = txtDSLength.Text;
                dr["txtDSWidth"] = txtDSWidth.Text;
                dr["txtDSThickness"] = txtDSThickness.Text;
                dr["txtDSOvenDryWt"] = txtDSOvenDryWt.Text;
                dr["txtDSVolume"] = txtDSVolume.Text;
                dr["txtDS"] = txtDS.Text;
                dr["txtDSAvg"] = txtDSAvg.Text;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["DSTable"] = dt;

        }
        #endregion

        private void ClearData()
        {
            TabRemark.Visible = true;
            TabDA.Visible = false;
            TabCS.Visible = false;
            TabDensity.Visible = false;
            TabET.Visible = false;
            TabWA.Visible = false;
            grdDA.DataSource = null;
            grdDA.DataBind();
            grdDACal.DataSource = null;
            grdDACal.DataBind();
            grdCS.DataSource = null;
            grdCS.DataBind();
            grdDS.DataSource = null;
            grdDS.DataBind();
            grdET.DataSource = null;
            grdET.DataBind();
            grdWA.DataSource = null;
            grdWA.DataBind();
            grdRemark.DataSource = null;
            grdRemark.DataBind();
            chkWitnessBy.Checked = false;
            txtWitnesBy.Visible = false;
            ViewState["RemarkTable"] = null;
            ViewState["DATable"] = null;
            ViewState["DACalTable"] = null;
            ViewState["WATable"] = null;
            ViewState["CSTable"] = null;
            ViewState["ETTable"] = null;
            ViewState["DSTable"] = null;
            txtWitnesBy.Text = "";
            ddlTestdApprdBy.SelectedIndex = 0;
            lnkSave.Enabled = true;
            lnkCalculate.Enabled = true;
        }
        
        private void DisplayBrickDetails()
        { 
            //Inward details
            var inwd = dc.BrickInward_View(txtRefNo.Text,0);
            foreach (var btinwd in inwd)
            {
                txtRefNo.Text = btinwd.BTINWD_ReferenceNo_var.ToString();
                if (btinwd.BTINWD_TestedDate_dt == null)
                    txtDateOfTest.Text = DateTime.Now.ToString("dd/MM/yyyy");
                else
                    txtDateOfTest.Text = Convert.ToDateTime(btinwd.BTINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                txtReportNo.Text = btinwd.BTINWD_SetOfRecord_var;
                txtTypeOfBrick.Text = btinwd.BTINWD_BrickType_var;
                txtSupplierName.Text = btinwd.BTINWD_SupplierName_var;
                txtDesc.Text = btinwd.BTINWD_Description_var;
                txtIdMark.Text = btinwd.BTINWD_IdMark_var;
                txtWitnesBy.Text = btinwd.BTINWD_WitnessBy_var;                
                if (ddl_NablScope.Items.FindByValue(btinwd.BTINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(btinwd.BTINWD_NablScope_var);
                }
                if (Convert.ToString(btinwd.BTINWD_NablLocation_int) != null && Convert.ToString(btinwd.BTINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(btinwd.BTINWD_NablLocation_int);
                }

                if (txtWitnesBy.Text != "")
                {
                    chkWitnessBy.Checked = true;
                    txtWitnesBy.Visible = true;
                }
                if (btinwd.BTINWD_Status_tint == 1)
                {
                    //lblEntdChkdBy.Text = "Entered By";
                    lblTestdApprdBy.Text = "Tested By"; 
                }
                else if (btinwd.BTINWD_Status_tint == 2)
                {
                    //lblEntdChkdBy.Text = "Checked By";
                    lblTestdApprdBy.Text = "Approved By";
                }
            }
            //Test Details            
            int rowNo = 0;
            var test = dc.BrickTest_View(txtRefNo.Text);
            foreach (var bttest in test)
            {
                rowNo = 0;
                if (bttest.TEST_Sr_No == 1)
                {
                    #region display CS data
                    TabCS.Visible = true;
                    txtCSQuantity.Text = bttest.BTTEST_Quantity_tint.ToString();
                    var cs = dc.BrickCS_View(txtRefNo.Text);
                    foreach (var btcs in cs)
                    {
                        AddRowCS();
                        TextBox txtCSIdMark = (TextBox)grdCS.Rows[rowNo].FindControl("txtCSIdMark");
                        TextBox txtCSLength = (TextBox)grdCS.Rows[rowNo].FindControl("txtCSLength");
                        TextBox txtCSWidth = (TextBox)grdCS.Rows[rowNo].FindControl("txtCSWidth");
                        TextBox txtCSLoad = (TextBox)grdCS.Rows[rowNo].FindControl("txtCSLoad");
                        TextBox txtCSArea = (TextBox)grdCS.Rows[rowNo].FindControl("txtCSArea");
                        TextBox txtCSStrength = (TextBox)grdCS.Rows[rowNo].FindControl("txtCSStrength");
                        TextBox txtCSAvg = (TextBox)grdCS.Rows[rowNo].FindControl("txtCSAvg");

                        txtCSIdMark.Text =btcs.BTCS_IdMark_var;
                        txtCSLength.Text = btcs.BTCS_Length_dec.ToString();
                        txtCSWidth.Text = btcs.BTCS_Width_dec.ToString();
                        txtCSLoad.Text = btcs.BTCS_Load_dec.ToString();
                        txtCSArea.Text = btcs.BTCS_Area_dec.ToString();
                        txtCSStrength.Text = btcs.BTCS_Strength_dec.ToString();
                        txtCSAvg.Text = btcs.BTCS_Average_var;
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        CSRowsChanged();
                    }
                    else
                    {
                        txtCSQuantity.Text = rowNo.ToString();
                    }
                    #endregion
                }
                else if (bttest.TEST_Sr_No == 2)
                {
                    #region display WA data
                    TabWA.Visible = true;
                    txtWAQuantity.Text = bttest.BTTEST_Quantity_tint.ToString();                    
                    var wa = dc.BrickWA_View(txtRefNo.Text);
                    foreach (var btwa in wa)
                    {
                        AddRowWA();
                        TextBox txtWAIdMark = (TextBox)grdWA.Rows[rowNo].FindControl("txtWAIdMark");
                        TextBox txtWADryWt = (TextBox)grdWA.Rows[rowNo].FindControl("txtWADryWt");
                        TextBox txtWAWetWt = (TextBox)grdWA.Rows[rowNo].FindControl("txtWAWetWt");
                        TextBox txtWA = (TextBox)grdWA.Rows[rowNo].FindControl("txtWA");
                        TextBox txtWAAvg = (TextBox)grdWA.Rows[rowNo].FindControl("txtWAAvg");

                        txtWAIdMark.Text = btwa.BTWA_IdMark_var;
                        txtWADryWt.Text = btwa.BTWA_DryWt_dec.ToString();
                        txtWAWetWt.Text = btwa.BTWA_WetWt_dec.ToString();
                        txtWA.Text = btwa.BTWA_WaterAbsorption_dec.ToString();
                        txtWAAvg.Text = btwa.BTWA_Average_dec.ToString();
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        WARowsChanged();
                    }
                    else
                    {
                        txtWAQuantity.Text = rowNo.ToString();
                    }
                    #endregion
                }
                else if (bttest.TEST_Sr_No == 3)
                {
                    #region display DA data
                    TabDA.Visible = true;
                    txtDARows.Text = bttest.BTTEST_Quantity_tint.ToString();
                    lblDAQuantity.Text = bttest.BTTEST_Quantity_tint.ToString();
                    var da = dc.BrickDA_View(txtRefNo.Text);
                    foreach (var btda in da)
                    {
                        AddRowDA();
                        TextBox txtDAIdMark = (TextBox)grdDA.Rows[rowNo].FindControl("txtDAIdMark");
                        TextBox txtL1 = (TextBox)grdDA.Rows[rowNo].FindControl("txtL1");
                        TextBox txtL2 = (TextBox)grdDA.Rows[rowNo].FindControl("txtL2");
                        TextBox txtL3 = (TextBox)grdDA.Rows[rowNo].FindControl("txtL3");
                        TextBox txtW1 = (TextBox)grdDA.Rows[rowNo].FindControl("txtW1");
                        TextBox txtW2 = (TextBox)grdDA.Rows[rowNo].FindControl("txtW2");
                        TextBox txtW3 = (TextBox)grdDA.Rows[rowNo].FindControl("txtW3");
                        TextBox txtH1 = (TextBox)grdDA.Rows[rowNo].FindControl("txtH1");
                        TextBox txtH2 = (TextBox)grdDA.Rows[rowNo].FindControl("txtH2");
                        TextBox txtH3 = (TextBox)grdDA.Rows[rowNo].FindControl("txtH3");

                        txtDAIdMark.Text = btda.BTDA_IdMark_var;
                        string[] strData= btda.BTDA_Length_var.Split('|');
                        txtL1.Text = strData[0];
                        txtL2.Text = strData[1];
                        txtL3.Text = strData[2];
                        strData = btda.BTDA_Width_var.Split('|');
                        txtW1.Text = strData[0];
                        txtW2.Text = strData[1];
                        txtW3.Text = strData[2];
                        strData = btda.BTDA_Height_var.Split('|');
                        txtH1.Text = strData[0];
                        txtH2.Text = strData[1];
                        txtH3.Text = strData[2];
                        AddRowDACal();
                        TextBox txtDACalIdMark = (TextBox)grdDACal.Rows[rowNo].FindControl("txtDACalIdMark");
                        TextBox txtDACalLength = (TextBox)grdDACal.Rows[rowNo].FindControl("txtDACalLength");
                        TextBox txtDACalWidth = (TextBox)grdDACal.Rows[rowNo].FindControl("txtDACalWidth");
                        TextBox txtDACalHeight = (TextBox)grdDACal.Rows[rowNo].FindControl("txtDACalHeight");
                        TextBox txtDACalAvgLength = (TextBox)grdDACal.Rows[rowNo].FindControl("txtDACalAvgLength");
                        TextBox txtDACalAvgWidth = (TextBox)grdDACal.Rows[rowNo].FindControl("txtDACalAvgWidth");
                        TextBox txtDACalAvgHeight = (TextBox)grdDACal.Rows[rowNo].FindControl("txtDACalAvgHeight");
                        txtDACalIdMark.Text = btda.BTDA_IdMark_var;
                        txtDACalLength.Text = btda.BTDA_AvgLength_dec.ToString();
                        txtDACalWidth.Text = btda.BTDA_AvgWidth_dec.ToString();
                        txtDACalHeight.Text = btda.BTDA_AvgHeight_dec.ToString();
                        if (btda.BTDA_Average_var != "" && btda.BTDA_Average_var != null)
                        {
                            strData = btda.BTDA_Average_var.Split('|');
                            txtDACalAvgLength.Text = strData[0];
                            txtDACalAvgWidth.Text = strData[1];
                            txtDACalAvgHeight.Text = strData[2];
                        }
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        DARowsChanged();
                    }
                    else
                    {
                        txtDARows.Text = rowNo.ToString();
                    }
                    #endregion
                }
                else if (bttest.TEST_Sr_No == 4)
                {                    
                    #region display ET data
                    TabET.Visible = true;
                    txtETQuantity.Text = bttest.BTTEST_Quantity_tint.ToString();
                    var et = dc.BrickET_View(txtRefNo.Text);
                    foreach (var btet in et)
                    {
                        AddRowET();
                        TextBox txtETIdMark = (TextBox)grdET.Rows[rowNo].FindControl("txtETIdMark");
                        DropDownList ddlETObservations = (DropDownList)grdET.Rows[rowNo].FindControl("ddlETObservations");

                        txtETIdMark.Text = btet.BTET_IdMark_var;
                        ddlETObservations.SelectedValue = btet.BTET_Observation_var;
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        ETRowsChanged();
                    }
                    else
                    {
                        txtETQuantity.Text = rowNo.ToString();
                    }
                    #endregion
                }
                else if (bttest.TEST_Sr_No == 5)
                {                    
                    #region display DS data
                    TabDensity.Visible = true;
                    txtDSQuantity.Text = bttest.BTTEST_Quantity_tint.ToString();
                    var ds = dc.BrickDS_View(txtRefNo.Text);
                    foreach (var btds in ds)
                    {
                        AddRowDS();
                        TextBox txtDSIdMark = (TextBox)grdDS.Rows[rowNo].FindControl("txtDSIdMark");
                        TextBox txtDSLength = (TextBox)grdDS.Rows[rowNo].FindControl("txtDSLength");
                        TextBox txtDSWidth = (TextBox)grdDS.Rows[rowNo].FindControl("txtDSWidth");
                        TextBox txtDSThickness = (TextBox)grdDS.Rows[rowNo].FindControl("txtDSThickness");
                        TextBox txtDSOvenDryWt = (TextBox)grdDS.Rows[rowNo].FindControl("txtDSOvenDryWt");
                        TextBox txtDSVolume = (TextBox)grdDS.Rows[rowNo].FindControl("txtDSVolume");
                        TextBox txtDS = (TextBox)grdDS.Rows[rowNo].FindControl("txtDS");
                        TextBox txtDSAvg = (TextBox)grdDS.Rows[rowNo].FindControl("txtDSAvg");

                        txtDSIdMark.Text = btds.BTDS_IdMark_var;
                        txtDSLength.Text = btds.BTDS_Length_dec.ToString();
                        txtDSWidth.Text = btds.BTDS_Width_dec.ToString();
                        txtDSThickness.Text = btds.BTDS_Thickness_dec.ToString();
                        txtDSOvenDryWt.Text = btds.BTDS_OvenDryWt_dec.ToString();
                        txtDSVolume.Text = btds.BTDS_Volume_dec.ToString();
                        txtDS.Text = btds.BTDS_Density_dec.ToString();
                        txtDSAvg.Text = btds.BTDS_Average_var;
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        DSRowsChanged();
                    }
                    else
                    {
                        txtDSQuantity.Text = rowNo.ToString();
                    }
                    #endregion
                }
            }
            
            if (TabDA.Visible == true)
                TabContainerBrick.ActiveTabIndex = 0;
            else if (TabWA.Visible == true)
                TabContainerBrick.ActiveTabIndex = 1;
            else if (TabCS.Visible == true)
                TabContainerBrick.ActiveTabIndex = 2;
            else if (TabET.Visible == true)
                TabContainerBrick.ActiveTabIndex = 3;
            else if (TabDensity.Visible == true)
                TabContainerBrick.ActiveTabIndex = 4;
            else if (TabRemark.Visible == true)
                TabContainerBrick.ActiveTabIndex = 5;
            //Remark details
            rowNo=0;
            var remark = dc.BrickRemarkDetail_View(txtRefNo.Text);
            foreach (var rem in remark)
            {
                AddRowRemark();                
                TextBox txtRemark = (TextBox)grdRemark.Rows[rowNo].FindControl("txtRemark");
                txtRemark.Text = rem.BTREM_Remark_var;
                rowNo++;
            }
            if (rowNo==0)
                AddRowRemark();
                
        }
        private void addSpecimenRemark()
        {
            bool flgAdd = false, flgAvailable=false;
            int RowNo = 0;
            if (TabDA.Visible == true)
            {
                if (Convert.ToInt32(lblDAQuantity.Text) < 20)
                    flgAdd = true;
            }
            else if (TabWA.Visible == true)
            {
                if (grdWA.Rows.Count < 5)
                    flgAdd = true;
            } 
            else if (TabCS.Visible == true)
            {
                if (grdCS.Rows.Count < 5)
                    flgAdd = true;
            }    
            else if (TabET.Visible == true)
            {
                if (grdET.Rows.Count < 5)
                    flgAdd = true;
            }    
            else if (TabDensity.Visible == true)
            {
                if (grdDS.Rows.Count < 5)
                    flgAdd = true;
            }
            //*** IS 3495 - 1992-RA(2011) ; states that atleast five specimens as required for testing. However as per customer's request testing is done on lesser no. of specimens
            for (int i = 0; i < grdRemark.Rows.Count; i++)
            {
                TextBox txtRemark = (TextBox)grdRemark.Rows[i].Cells[1].FindControl("txtRemark");
                if (txtRemark.Text.Contains("atleast five specimens") == true || txtRemark.Text.Contains("atleast twenty specimens") == true)
                {
                    flgAvailable = true;
                    RowNo = i;
                    break;
                }
            }
            if (flgAdd == true && flgAvailable == false)//add remark
            {
                TextBox txtRemark = (TextBox)grdRemark.Rows[0].FindControl("txtRemark");
                if (txtRemark.Text != "")
                {
                    AddRowRemark();
                    txtRemark = (TextBox)grdRemark.Rows[grdRemark.Rows.Count-1].FindControl("txtRemark");
                }
                if (TabDA.Visible == true)
                    txtRemark.Text = "*** IS 3495 - 1992-RA(2011) ; states that atleast twenty specimens as required for testing. However as per customer's request testing is done on lesser no. of specimens";                
                else
                    txtRemark.Text = "*** IS 3495 - 1992-RA(2011) ; states that atleast five specimens as required for testing. However as per customer's request testing is done on lesser no. of specimens";
            }
            else if (flgAdd == false && flgAvailable == true)//delete remark
            {
                if (RowNo == 0)
                {
                    TextBox txtRemark = (TextBox)grdRemark.Rows[0].FindControl("txtRemark");
                    txtRemark.Text = "";
                }
                else
                {
                    DeleteRowRemark(RowNo);
                }
            }
        }
        private void CalculateDA()
        {
            decimal len = 0,wdh = 0,het = 0;
            decimal totlen = 0, totwdh = 0,tothet = 0;
            
            for (int i = 0; i < grdDA.Rows.Count; i++)
            {
                TextBox txtDAIdMark = (TextBox)grdDA.Rows[i].FindControl("txtDAIdMark");
                TextBox txtDACalIdMark = (TextBox)grdDACal.Rows[i].FindControl("txtDACalIdMark");
                TextBox txtDACalAvgLength1 = (TextBox)grdDACal.Rows[i].FindControl("txtDACalAvgLength");
                TextBox txtDACalAvgWidth1 = (TextBox)grdDACal.Rows[i].FindControl("txtDACalAvgWidth");
                TextBox txtDACalAvgHeight1 = (TextBox)grdDACal.Rows[i].FindControl("txtDACalAvgHeight");
                txtDACalAvgLength1.Text = "";
                txtDACalAvgWidth1.Text = "";
                txtDACalAvgHeight1.Text = "";

                txtDACalIdMark.Text = txtDAIdMark.Text;
                //Length
                TextBox txtL1 = (TextBox)grdDA.Rows[i].FindControl("txtL1");
                TextBox txtL2 = (TextBox)grdDA.Rows[i].FindControl("txtL2");
                TextBox txtL3 = (TextBox)grdDA.Rows[i].FindControl("txtL3");
                len = 0;
                if (txtL1.Text != "")
                    len = len + Convert.ToDecimal(txtL1.Text);
                if (txtL2.Text != "")
                    len = len + Convert.ToDecimal(txtL2.Text);
                if (txtL3.Text != "")
                    len = len + Convert.ToDecimal(txtL3.Text);

                TextBox txtDACalLength = (TextBox)grdDACal.Rows[i].FindControl("txtDACalLength");
                txtDACalLength.Text = "0.00";
                if (len > 0)
                {
                    txtDACalLength.Text = (len / 3).ToString("0.00");
                }
                totlen = totlen + Convert.ToDecimal(txtDACalLength.Text);

                //Width
                TextBox txtW1 = (TextBox)grdDA.Rows[i].FindControl("txtW1");
                TextBox txtW2 = (TextBox)grdDA.Rows[i].FindControl("txtW2");
                TextBox txtW3 = (TextBox)grdDA.Rows[i].FindControl("txtW3");
                wdh = 0;
                if (txtW1.Text != "")
                    wdh = wdh + Convert.ToDecimal(txtW1.Text);
                if (txtW2.Text != "")
                    wdh = wdh + Convert.ToDecimal(txtW2.Text);
                if (txtW3.Text != "")
                    wdh = wdh + Convert.ToDecimal(txtW3.Text);
                
                TextBox txtDACalWidth = (TextBox)grdDACal.Rows[i].FindControl("txtDACalWidth");
                txtDACalWidth.Text = "0.00";
                if (wdh > 0)
                {
                    txtDACalWidth.Text = (wdh / 3).ToString("0.00");
                }
                totwdh = totwdh + Convert.ToDecimal(txtDACalWidth.Text);

                //Height
                TextBox txtH1 = (TextBox)grdDA.Rows[i].FindControl("txtH1");
                TextBox txtH2 = (TextBox)grdDA.Rows[i].FindControl("txtH2");
                TextBox txtH3 = (TextBox)grdDA.Rows[i].FindControl("txtH3");
                het = 0;
                if (txtH1.Text != "")
                    het = het + Convert.ToDecimal(txtH1.Text);
                if (txtH2.Text != "")
                    het = het + Convert.ToDecimal(txtH2.Text);
                if (txtH3.Text != "")
                    het = het + Convert.ToDecimal(txtH3.Text);

                TextBox txtDACalHeight = (TextBox)grdDACal.Rows[i].FindControl("txtDACalHeight");
                txtDACalHeight.Text = "0.00";
                if (het > 0)
                {
                    txtDACalHeight.Text = (het / 3).ToString("0.00");
                }
                tothet = tothet + Convert.ToDecimal(txtDACalHeight.Text);
            }
            TextBox txtDACalAvgLength = (TextBox)grdDACal.Rows[(grdDACal.Rows.Count / 2)].FindControl("txtDACalAvgLength");
            txtDACalAvgLength.Text = (totlen / grdDACal.Rows.Count).ToString("0.00");

            TextBox txtDACalAvgWidth = (TextBox)grdDACal.Rows[(grdDACal.Rows.Count/2)].FindControl("txtDACalAvgWidth");
            txtDACalAvgWidth.Text = (totwdh / grdDACal.Rows.Count).ToString("0.00");

            TextBox txtDACalAvgHeight = (TextBox)grdDACal.Rows[(grdDACal.Rows.Count / 2)].FindControl("txtDACalAvgHeight");
            txtDACalAvgHeight.Text = (tothet / grdDACal.Rows.Count).ToString("0.00");
            ////if (grdDACal.Rows.Count < 5)
            //if (Convert.ToInt32(lblDAQuantity.Text) < 20)
            //    txtDACalAvgHeight.Text = "***";            
        }

        private void CalculateWA()
        {
            decimal wa = 0, avgwa = 0;

            for (int i = 0; i < grdWA.Rows.Count; i++)
            {                
                //water absoption
                TextBox txtWADryWt = (TextBox)grdWA.Rows[i].FindControl("txtWADryWt");
                TextBox txtWAWetWt = (TextBox)grdWA.Rows[i].FindControl("txtWAWetWt");
                TextBox txtWA = (TextBox)grdWA.Rows[i].FindControl("txtWA");
                TextBox txtWAAvg1 = (TextBox)grdWA.Rows[i].FindControl("txtWAAvg");
                wa = 0;
                if (txtWADryWt.Text != "" && txtWAWetWt.Text != "")
                {
                    if (Convert.ToDecimal(txtWADryWt.Text) > Convert.ToDecimal(txtWAWetWt.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning", "alert('Dry Weight must be less than Wet Weight for row no " + (i+1) + ".');", true);
                        break;
                    }
                    else
                    { 
                        if (Convert.ToDecimal(txtWADryWt.Text) > 0)
                            wa = (100 * (Convert.ToDecimal(txtWAWetWt.Text) - Convert.ToDecimal(txtWADryWt.Text)) )/ Convert.ToDecimal(txtWADryWt.Text);
                        else
                            wa = 100 * (Convert.ToDecimal(txtWAWetWt.Text) - Convert.ToDecimal(txtWADryWt.Text)) ;

                        txtWA.Text = wa.ToString("0.00");
                        avgwa = avgwa + wa;
                    }
                }
                txtWAAvg1.Text = "";
            }
            TextBox txtWAAvg = (TextBox)grdWA.Rows[(grdWA.Rows.Count / 2)].FindControl("txtWAAvg");
            txtWAAvg.Text = "0.00";
            if (avgwa > 0)
                txtWAAvg.Text = (avgwa / grdWA.Rows.Count).ToString("0.00");
            if (grdWA.Rows.Count < 5)
                txtWAAvg.Text = "***";
            
        }

        private void CalculateCS()
        {
            decimal cs = 0, avgcs = 0;

            for (int i = 0; i < grdCS.Rows.Count; i++)
            {
                //compressive strength
                TextBox txtCSIdMark = (TextBox)grdCS.Rows[i].FindControl("txtCSIdMark");
                TextBox txtCSLength = (TextBox)grdCS.Rows[i].FindControl("txtCSLength");
                TextBox txtCSWidth = (TextBox)grdCS.Rows[i].FindControl("txtCSWidth");
                TextBox txtCSLoad = (TextBox)grdCS.Rows[i].FindControl("txtCSLoad");
                TextBox txtCSArea = (TextBox)grdCS.Rows[i].FindControl("txtCSArea");
                TextBox txtCSStrength = (TextBox)grdCS.Rows[i].FindControl("txtCSStrength");
                TextBox txtCSAvg1 = (TextBox)grdCS.Rows[i].FindControl("txtCSAvg");
                
                cs = 0;
                if (txtCSLength.Text != "" && txtCSWidth.Text != "")
                {
                    txtCSArea.Text = (Convert.ToDecimal(txtCSLength.Text) * Convert.ToDecimal(txtCSWidth.Text)).ToString("0.0");
                    if (txtCSLoad.Text != "")
                    {
                        if (Convert.ToDecimal(txtCSArea.Text) > 0)
                            cs = Convert.ToDecimal(txtCSLoad.Text) / Convert.ToDecimal(txtCSArea.Text) *1000;
                        else
                            cs = Convert.ToDecimal(txtCSLoad.Text) * 1000;

                        txtCSStrength.Text = cs.ToString("0.00");
                        avgcs = avgcs + cs;
                    }
                }
                txtCSAvg1.Text = "";
            }
            TextBox txtCSAvg = (TextBox)grdCS.Rows[(grdCS.Rows.Count / 2)].FindControl("txtCSAvg");
            txtCSAvg.Text = "0.00";
            if (avgcs > 0)
                txtCSAvg.Text = (avgcs / grdCS.Rows.Count).ToString("0.00");

            if (grdCS.Rows.Count < 5)
                txtCSAvg.Text = "***";            
        }

        private void CalculateDS()
        {
            decimal ds = 0, avgds = 0;

            for (int i = 0; i < grdDS.Rows.Count; i++)
            {
                //density
                TextBox txtDSIdMark = (TextBox)grdDS.Rows[i].FindControl("txtDSIdMark");
                TextBox txtDSLength = (TextBox)grdDS.Rows[i].FindControl("txtDSLength");
                TextBox txtDSWidth = (TextBox)grdDS.Rows[i].FindControl("txtDSWidth");
                TextBox txtDSThickness = (TextBox)grdDS.Rows[i].FindControl("txtDSThickness");
                TextBox txtDSOvenDryWt = (TextBox)grdDS.Rows[i].FindControl("txtDSOvenDryWt");
                TextBox txtDSVolume = (TextBox)grdDS.Rows[i].FindControl("txtDSVolume");
                TextBox txtDS = (TextBox)grdDS.Rows[i].FindControl("txtDS");
                TextBox txtDSAvg1 = (TextBox)grdDS.Rows[i].FindControl("txtDSAvg");

                ds = 0;
                if (txtDSLength.Text != "" && txtDSWidth.Text != "" && txtDSThickness.Text != "")
                {
                    txtDSVolume.Text = (Convert.ToDecimal(txtDSLength.Text) * Convert.ToDecimal(txtDSWidth.Text) * Convert.ToDecimal(txtDSThickness.Text)).ToString("0");
                    if (txtDSVolume.Text != "")
                    {
                        if (Convert.ToDecimal(txtDSVolume.Text) > 0)
                            ds = (1000000000 * Convert.ToDecimal(txtDSOvenDryWt.Text)) / Convert.ToDecimal(txtDSVolume.Text);
                        else
                            ds = (1000000000 * Convert.ToDecimal(txtDSOvenDryWt.Text));

                        txtDS.Text = ds.ToString("0");
                        avgds = avgds + ds;
                    }
                }
                txtDSAvg1.Text = "";
            }
            TextBox txtDSAvg = (TextBox)grdDS.Rows[(grdDS.Rows.Count / 2)].FindControl("txtDSAvg");
            txtDSAvg.Text = "0";
            if (avgds > 0)
                txtDSAvg.Text = (avgds / grdDS.Rows.Count).ToString("0");

            if (grdDS.Rows.Count < 5)
                txtDSAvg.Text = "***";
        }

        protected Boolean ValidateData()
        {
            string dispalyMsg = "";
            Boolean valid = true;
                        
            if (txtSupplierName.Text == "")
            {
                dispalyMsg = "Please Enter Supplier Name";
                txtSupplierName.Focus();
                valid = false;
            }
            else if (txtDesc.Text == "")
            {
                dispalyMsg = "Please Enter Description";
                txtDesc.Focus();
                valid = false;
            }
            //date validation            
            else if (txtDateOfTest.Text == "")
            {
                dispalyMsg = "Date Of Testing can not be blank.";
                txtDateOfTest.Focus();
                valid = false;
            }
            else if (txtDateOfTest.Text != "")
            {
                //DateTime dateTest = DateTime.Now;
                //dateTest = Convert.ToDateTime(txtDateOfTest.Text);
                string strCurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime TestingDate = DateTime.ParseExact(txtDateOfTest.Text, "dd/MM/yyyy", null);
                DateTime CurrentDate = DateTime.ParseExact(strCurrentDate, "dd/MM/yyyy", null);

                //if (dateTest > System.DateTime.Now)
                if (TestingDate > CurrentDate)
                {
                    dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
                    txtDateOfTest.Focus();
                    valid = false;
                }
            }
            else if (ddl_NablScope.SelectedItem.Text == "--Select--")
            {
                dispalyMsg = "Select 'NABL Scope'.";
                valid = false;
                ddl_NablScope.Focus();
            }
            else if (ddl_NABLLocation.SelectedItem.Text == "--Select--")
            {
                dispalyMsg = "Select 'NABL Location'.";
                valid = false;
                ddl_NABLLocation.Focus();
            }
            //validate DA data
            #region validate da data
            if (TabDA.Visible == true && valid == true)
            {
                if (txtDARows.Text == "" || txtDARows.Text == "0")
                {
                    dispalyMsg = "Rows can not be zero or blank.";
                    txtDARows.Focus();
                    valid = false;
                }

                if (valid == true)
                {
                    for (int i = 0; i < grdDA.Rows.Count; i++)
                    {
                        TextBox txtDAIdMark = (TextBox)grdDA.Rows[i].FindControl("txtDAIdMark");
                        TextBox txtL1 = (TextBox)grdDA.Rows[i].FindControl("txtL1");
                        TextBox txtL2 = (TextBox)grdDA.Rows[i].FindControl("txtL2");
                        TextBox txtL3 = (TextBox)grdDA.Rows[i].FindControl("txtL3");
                        TextBox txtW1 = (TextBox)grdDA.Rows[i].FindControl("txtW1");
                        TextBox txtW2 = (TextBox)grdDA.Rows[i].FindControl("txtW2");
                        TextBox txtW3 = (TextBox)grdDA.Rows[i].FindControl("txtW3");
                        TextBox txtH1 = (TextBox)grdDA.Rows[i].FindControl("txtH1");
                        TextBox txtH2 = (TextBox)grdDA.Rows[i].FindControl("txtH2");
                        TextBox txtH3 = (TextBox)grdDA.Rows[i].FindControl("txtH3");

                        if (txtDAIdMark.Text == "")
                        {
                            dispalyMsg = "Enter Id Mark for row no " + (i + 1) + ".";
                            txtDAIdMark.Focus();
                            valid = false;
                            TabContainerBrick.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtL1.Text == "")
                        {
                            dispalyMsg = "Enter Length 1 for row number " + (i + 1) + ".";
                            txtL1.Focus();
                            valid = false;
                            TabContainerBrick.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtL2.Text == "")
                        {
                            dispalyMsg = "Enter Length 2 for row number " + (i + 1) + ".";
                            txtL2.Focus();
                            valid = false;
                            TabContainerBrick.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtL3.Text == "")
                        {
                            dispalyMsg = "Enter Length 3 for row number " + (i + 1) + ".";
                            txtL3.Focus();
                            valid = false;
                            TabContainerBrick.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtW1.Text == "")
                        {
                            dispalyMsg = "Enter Width 1 for row number " + (i + 1) + ".";
                            txtW1.Focus();
                            valid = false;
                            TabContainerBrick.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtW2.Text == "")
                        {
                            dispalyMsg = "Enter Width 2 for row number " + (i + 1) + ".";
                            txtW2.Focus();
                            valid = false;
                            TabContainerBrick.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtW3.Text == "")
                        {
                            dispalyMsg = "Enter Width 3 for row number " + (i + 1) + ".";
                            txtW3.Focus();
                            valid = false;
                            TabContainerBrick.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtH1.Text == "")
                        {
                            dispalyMsg = "Enter Height 1 for row number " + (i + 1) + ".";
                            txtH1.Focus();
                            valid = false;
                            TabContainerBrick.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtH2.Text == "")
                        {
                            dispalyMsg = "Enter Height 2 for row number " + (i + 1) + ".";
                            txtH2.Focus();
                            valid = false;
                            TabContainerBrick.ActiveTabIndex = 0;
                            break;
                        }
                        else if (txtH3.Text == "")
                        {
                            dispalyMsg = "Enter Height 3 for row number " + (i + 1) + ".";
                            txtH3.Focus();
                            valid = false;
                            TabContainerBrick.ActiveTabIndex = 0;
                            break;
                        }
                    }

                }
            }
            #endregion
            //validate WA data
            #region validate wa data
            if (TabWA.Visible == true && valid == true)
            {

                for (int i = 0; i < grdWA.Rows.Count; i++)
                {
                    TextBox txtWAIdMark = (TextBox)grdWA.Rows[i].FindControl("txtWAIdMark");
                    TextBox txtWADryWt = (TextBox)grdWA.Rows[i].FindControl("txtWADryWt");
                    TextBox txtWAWetWt = (TextBox)grdWA.Rows[i].FindControl("txtWAWetWt");

                    if (txtWAIdMark.Text == "")
                    {
                        dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
                        txtWAIdMark.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 1;
                        break;
                    }
                    else if (txtWADryWt.Text == "")
                    {
                        dispalyMsg = "Enter Dry Wt. for row number " + (i + 1) + ".";
                        txtWADryWt.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 1;
                        break;
                    }
                    else if (txtWAWetWt.Text == "")
                    {
                        dispalyMsg = "Enter Wet Wt. for row number " + (i + 1) + ".";
                        txtWAWetWt.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 1;
                        break;
                    }
                    else if (Convert.ToDecimal(txtWADryWt.Text) > Convert.ToDecimal(txtWAWetWt.Text))
                    {
                        dispalyMsg = "Dry wt. must be less than Wet Wt. for row number " + (i + 1) + ".";
                        txtWAWetWt.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 1;
                        break;
                    }
                }
            }
            #endregion
            ////validate CS data
            #region validate cs data
            if (TabCS.Visible == true && valid == true)
            {
                for (int i = 0; i < grdCS.Rows.Count; i++)
                {
                    TextBox txtCSIdMark = (TextBox)grdCS.Rows[i].FindControl("txtCSIdMark");
                    TextBox txtCSLength = (TextBox)grdCS.Rows[i].FindControl("txtCSLength");
                    TextBox txtCSWidth = (TextBox)grdCS.Rows[i].FindControl("txtCSWidth");
                    TextBox txtCSLoad = (TextBox)grdCS.Rows[i].FindControl("txtCSLoad");
                    if (txtCSIdMark.Text == "")
                    {
                        dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
                        txtCSIdMark.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 2;
                        break;
                    }
                    else if (txtCSLength.Text == "")
                    {
                        dispalyMsg = "Enter Length for row number " + (i + 1) + ".";
                        txtCSLength.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 2;
                        break;
                    }
                    else if (txtCSWidth.Text == "")
                    {
                        dispalyMsg = "Enter Width for row number " + (i + 1) + ".";
                        txtCSWidth.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 2;
                        break;
                    }
                    else if (txtCSLoad.Text == "")
                    {
                        dispalyMsg = "Enter Load for row number " + (i + 1) + ".";
                        txtCSLoad.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 2;
                        break;
                    }
                }                
            }
            #endregion
            ////validate ET data
            #region validate et data
            if (TabET.Visible == true && valid == true)
            {
                for (int i = 0; i < grdET.Rows.Count; i++)
                {
                    TextBox txtETIdMark = (TextBox)grdET.Rows[i].FindControl("txtETIdMark");
                    DropDownList ddlETObservations = (DropDownList)grdET.Rows[i].FindControl("ddlETObservations");
                    if (txtETIdMark.Text == "")
                    {
                        dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
                        txtETIdMark.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 3;
                        break;
                    }
                    else if (ddlETObservations.SelectedItem.Text == "")
                    {
                        dispalyMsg = "Select Observation for row no " + (i + 1) + ".";
                        ddlETObservations.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 3;
                        break;
                    }
                }
            }
            #endregion
            ////validate Density data
            #region validate ds data
            if (TabDensity.Visible == true && valid == true)
            {
                for (int i = 0; i < grdDS.Rows.Count; i++)
                {
                    TextBox txtDSIdMark = (TextBox)grdDS.Rows[i].FindControl("txtDSIdMark");
                    TextBox txtDSLength = (TextBox)grdDS.Rows[i].FindControl("txtDSLength");
                    TextBox txtDSWidth = (TextBox)grdDS.Rows[i].FindControl("txtDSWidth");
                    TextBox txtDSThickness = (TextBox)grdDS.Rows[i].FindControl("txtDSThickness");
                    TextBox txtDSOvenDryWt = (TextBox)grdDS.Rows[i].FindControl("txtDSOvenDryWt");
                    if (txtDSIdMark.Text == "")
                    {
                        dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
                        txtIdMark.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 4;
                        break;
                    }
                    else if (txtDSLength.Text == "")
                    {
                        dispalyMsg = "Enter Length for row number " + (i + 1) + ".";
                        txtDSLength.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 4;
                        break;
                    }
                    else if (txtDSWidth.Text == "")
                    {
                        dispalyMsg = "Enter Width for row number " + (i + 1) + ".";
                        txtDSWidth.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 4;
                        break;
                    }
                    else if (txtDSThickness.Text == "")
                    {
                        dispalyMsg = "Enter Thickness for row number " + (i + 1) + ".";
                        txtDSThickness.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 4;
                        break;
                    }
                    else if (txtDSOvenDryWt.Text == "")
                    {
                        dispalyMsg = "Enter Oven Dry Wt. for row number " + (i + 1) + ".";
                        txtDSOvenDryWt.Focus();
                        valid = false;
                        TabContainerBrick.ActiveTabIndex = 4;
                        break;
                    }
                }                
            }
            #endregion
            if (valid == true)
            {
                for (int i = 0; i <= grdRemark.Rows.Count - 1; i++)
                {
                    TextBox txtRemark = (TextBox)grdRemark.Rows[i].FindControl("txtRemark");
                    txtRemark.Text = txtRemark.Text.Trim();
                    if (txtRemark.Text == "" && grdRemark.Rows.Count > 1)
                    {
                        dispalyMsg = "Please Enter Remark.";
                        TabContainerBrick.ActiveTabIndex = 5;
                        txtRemark.Focus();
                        valid = false;
                        break;
                    }
                }
            }
            if (valid == true)
            {
                // Witness by validation
                if (chkWitnessBy.Checked == true && txtWitnesBy.Text == "")
                {                   
                    dispalyMsg = "Please Enter Witness By Name.";
                    txtWitnesBy.Focus();
                    valid = false;                    
                }
                else if (ddlTestdApprdBy.SelectedIndex <= 0)
                {
                    //dispalyMsg = "Please Select Tested By/Approved By Name.";
                    dispalyMsg = "Please Select " + lblTestdApprdBy.Text + " Name.";
                    ddlTestdApprdBy.Focus();
                    valid = false; 
                }
            }
            if (valid == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
            return valid;
        }

        protected void lnkFetch_Click(object sender, EventArgs e)
        {
            if (ddlRefNo.SelectedIndex > 0)
            {
                ClearData();
                txtRefNo.Text = ddlRefNo.SelectedValue;
             //   Session["ReferenceNo"] = txtRefNo.Text;
                DisplayBrickDetails();
                LoadReferenceNoList();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning", "alert('Please Select Reference No.');", true);
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {            
            if (ValidateData() == true)
            {
                Calculate();
                addSpecimenRemark();
                //inward update
                byte reportStatus = 0, enteredBy = 0 , checkedBy = 0, testedBy = 0 ,approvedBy = 0;
                if (lblStatus.Text == "Enter")
                {
                    reportStatus = 2;
                    enteredBy = Convert.ToByte(Session["LoginId"]);
                    testedBy = Convert.ToByte(ddlTestdApprdBy.SelectedItem.Value);
                    dc.MISDetail_Update(0, "BT-", txtRefNo.Text, "BT-", null, true, false, false, false, false, false, false);
                }
                else if (lblStatus.Text == "Check")
                {
                    reportStatus = 3;
                    checkedBy = Convert.ToByte(Session["LoginId"]);
                    approvedBy = Convert.ToByte(ddlTestdApprdBy.SelectedItem.Value);
                    dc.MISDetail_Update(0, "BT-", txtRefNo.Text, "BT-", null, false, true, false, false, false, false, false);
                }

                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txtRefNo.Text, "BT-", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));

                DateTime TestingDt = DateTime.ParseExact(txtDateOfTest.Text, "dd/MM/yyyy", null);
                dc.BrickInward_Update_ReportData(reportStatus, txtRefNo.Text, txtDesc.Text, txtSupplierName.Text, 0, txtWitnesBy.Text, testedBy, enteredBy, checkedBy, approvedBy, 0, 0, 0, TestingDt);
                //test data update
                string strAvg = "";
                //DA
                #region save DA data
                if (TabDA.Visible == true)
                {                    
                    dc.BrickDA_Update(0,txtRefNo.Text,"","","","",0,0,0,"",true); 
                    for (int i = 0; i < grdDA.Rows.Count; i++)
                    {
                        TextBox txtDAIdMark = (TextBox)grdDA.Rows[i].FindControl("txtDAIdMark");
                        TextBox txtL1 = (TextBox)grdDA.Rows[i].FindControl("txtL1");
                        TextBox txtL2 = (TextBox)grdDA.Rows[i].FindControl("txtL2");
                        TextBox txtL3 = (TextBox)grdDA.Rows[i].FindControl("txtL3");
                        TextBox txtW1 = (TextBox)grdDA.Rows[i].FindControl("txtW1");
                        TextBox txtW2 = (TextBox)grdDA.Rows[i].FindControl("txtW2");
                        TextBox txtW3 = (TextBox)grdDA.Rows[i].FindControl("txtW3");
                        TextBox txtH1 = (TextBox)grdDA.Rows[i].FindControl("txtH1");
                        TextBox txtH2 = (TextBox)grdDA.Rows[i].FindControl("txtH2");
                        TextBox txtH3 = (TextBox)grdDA.Rows[i].FindControl("txtH3");

                        TextBox txtDACalLength = (TextBox)grdDACal.Rows[i].FindControl("txtDACalLength");
                        TextBox txtDACalWidth = (TextBox)grdDACal.Rows[i].FindControl("txtDACalWidth");
                        TextBox txtDACalHeight = (TextBox)grdDACal.Rows[i].FindControl("txtDACalHeight");
                        TextBox txtDACalAvgLength = (TextBox)grdDACal.Rows[i].FindControl("txtDACalAvgLength");
                        TextBox txtDACalAvgWidth = (TextBox)grdDACal.Rows[i].FindControl("txtDACalAvgWidth");
                        TextBox txtDACalAvgHeight = (TextBox)grdDACal.Rows[i].FindControl("txtDACalAvgHeight");
                        strAvg = "";
                        if (txtDACalAvgLength.Text != "")
                        {
                            strAvg = txtDACalAvgLength.Text + "|" + txtDACalAvgWidth.Text + "|" + txtDACalAvgHeight.Text;
                        }
                        dc.BrickDA_Update(i + 1, txtRefNo.Text, txtDAIdMark.Text, (txtL1.Text + "|" + txtL2.Text + "|" + txtL3.Text), (txtW1.Text + "|" + txtW2.Text + "|" + txtW3.Text), (txtH1.Text + "|" + txtH2.Text + "|" + txtH3.Text), Convert.ToDecimal(txtDACalLength.Text), Convert.ToDecimal(txtDACalWidth.Text), Convert.ToDecimal(txtDACalHeight.Text), strAvg, false);
                    }
                }
                #endregion
                //WA
                #region save WA data
                if (TabWA.Visible == true)
                {
                    decimal WAAvg = 0;
                    dc.BrickWA_Update(0, txtRefNo.Text, "", 0, 0,0,0, true);
                    for (int i = 0; i < grdWA.Rows.Count; i++)
                    {
                        TextBox txtWAIdMark = (TextBox)grdWA.Rows[i].FindControl("txtWAIdMark");
                        TextBox txtWADryWt = (TextBox)grdWA.Rows[i].FindControl("txtWADryWt");
                        TextBox txtWAWetWt = (TextBox)grdWA.Rows[i].FindControl("txtWAWetWt");
                        TextBox txtWA = (TextBox)grdWA.Rows[i].FindControl("txtWA");
                        TextBox txtWAAvg = (TextBox)grdWA.Rows[i].FindControl("txtWAAvg");
                        if (txtWAAvg.Text != "" && txtWAAvg.Text != "***")
                            WAAvg = Convert.ToDecimal(txtWAAvg.Text);
                        else
                            WAAvg = 0;
                        dc.BrickWA_Update(i + 1, txtRefNo.Text, txtWAIdMark.Text, Convert.ToDecimal(txtWADryWt.Text), Convert.ToDecimal(txtWAWetWt.Text), Convert.ToDecimal(txtWA.Text), WAAvg, false);
                    }
                }
                #endregion
                //CS
                #region save CS data
                if (TabCS.Visible == true)
                {                    
                    dc.BrickCS_Update(0, txtRefNo.Text, "", 0, 0, 0, 0,0,"", true);
                    for (int i = 0; i < grdCS.Rows.Count; i++)
                    {
                        TextBox txtCSIdMark = (TextBox)grdCS.Rows[i].FindControl("txtCSIdMark");
                        TextBox txtCSLength = (TextBox)grdCS.Rows[i].FindControl("txtCSLength");
                        TextBox txtCSWidth = (TextBox)grdCS.Rows[i].FindControl("txtCSWidth");
                        TextBox txtCSLoad = (TextBox)grdCS.Rows[i].FindControl("txtCSLoad");
                        TextBox txtCSArea = (TextBox)grdCS.Rows[i].FindControl("txtCSArea");
                        TextBox txtCSStrength = (TextBox)grdCS.Rows[i].FindControl("txtCSStrength");
                        TextBox txtCSAvg = (TextBox)grdCS.Rows[i].FindControl("txtCSAvg");

                        dc.BrickCS_Update(i + 1, txtRefNo.Text, txtCSIdMark.Text, Convert.ToDecimal(txtCSLength.Text), Convert.ToDecimal(txtCSWidth.Text), Convert.ToDecimal(txtCSLoad.Text), Convert.ToDecimal(txtCSArea.Text), Convert.ToDecimal(txtCSStrength.Text), txtCSAvg.Text, false);
                    }
                }
                #endregion
                //ET
                #region save ET data
                if (TabET.Visible == true)
                {
                    dc.BrickET_Update(0, txtRefNo.Text, "", "", true);
                    for (int i = 0; i < grdET.Rows.Count; i++)
                    {
                        TextBox txtETIdMark = (TextBox)grdET.Rows[i].FindControl("txtETIdMark");
                        DropDownList ddlETObservations = (DropDownList)grdET.Rows[i].FindControl("ddlETObservations");

                        dc.BrickET_Update(i + 1, txtRefNo.Text, txtETIdMark.Text, ddlETObservations.SelectedValue, false);
                    }
                }
                #endregion
                //DS
                #region save DS data
                if (TabDensity.Visible == true)
                {
                    dc.BrickDS_Update(0, txtRefNo.Text, "", 0, 0, 0, 0, 0,0,"", true);
                    for (int i = 0; i < grdDS.Rows.Count; i++)
                    {
                        TextBox txtDSIdMark = (TextBox)grdDS.Rows[i].FindControl("txtDSIdMark");
                        TextBox txtDSLength = (TextBox)grdDS.Rows[i].FindControl("txtDSLength");
                        TextBox txtDSWidth = (TextBox)grdDS.Rows[i].FindControl("txtDSWidth");
                        TextBox txtDSThickness = (TextBox)grdDS.Rows[i].FindControl("txtDSThickness");
                        TextBox txtDSOvenDryWt = (TextBox)grdDS.Rows[i].FindControl("txtDSOvenDryWt");
                        TextBox txtDSVolume = (TextBox)grdDS.Rows[i].FindControl("txtDSVolume");
                        TextBox txtDS = (TextBox)grdDS.Rows[i].FindControl("txtDS");
                        TextBox txtDSAvg = (TextBox)grdDS.Rows[i].FindControl("txtDSAvg");

                        dc.BrickDS_Update(i + 1, txtRefNo.Text, txtDSIdMark.Text, Convert.ToDecimal(txtDSLength.Text), Convert.ToDecimal(txtDSWidth.Text), Convert.ToDecimal(txtDSThickness.Text), Convert.ToDecimal(txtDSOvenDryWt.Text), Convert.ToDecimal(txtDSVolume.Text), Convert.ToDecimal(txtDS.Text), txtDSAvg.Text, false);
                    }
                }
                #endregion
                //remark update
                dc.BrickRemarkDetail_Update(0, txtRefNo.Text, true);
                string remId = "";
                for (int i = 0; i <= grdRemark.Rows.Count - 1; i++)
                {
                    TextBox txtRemark = (TextBox)grdRemark.Rows[i].FindControl("txtRemark");
                    if (txtRemark.Text != "")
                    {
                        dc.BrickRemark_View(txtRemark.Text, ref remId);
                        if (remId == "" || remId == null)
                        {
                            remId = dc.BrickRemark_Update(txtRemark.Text).ToString();
                        }
                        dc.BrickRemarkDetail_Update(Convert.ToInt32(remId), txtRefNo.Text, false);
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Report Saved Successfully');", true);
                lnkPrint.Visible = true;
                lnkSave.Enabled = false;
                lnkCalculate.Enabled = false;
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            //rpt.Brick_PDFReport(txtRefNo.Text, lblStatus.Text);
            rpt.PrintSelectedReport(txtRecordType.Text, txtRefNo.Text, lblStatus.Text, "", "", "", "", "", "", "");

        }

        protected void lnkExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportStatus.aspx");                 
        }

        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            Calculate();
        }
        private void Calculate()
        {
            if (TabDA.Visible == true)
                CalculateDA();
            if (TabWA.Visible == true)
                CalculateWA();
            if (TabCS.Visible == true)
                CalculateCS();
            if (TabDensity.Visible == true)
                CalculateDS(); 
        }
        protected void chkWitnessBy_CheckedChanged(object sender, EventArgs e)
        {
            txtWitnesBy.Text = "";
            if (chkWitnessBy.Checked == true)
                txtWitnesBy.Visible = true;
            else
                txtWitnesBy.Visible = false;
        }
        //#region bind grid
        //public void bindGridData(string rowcount)
        //{
        //    //idmark
        //    GridHeight.Visible = false;
        //    GridWidth.Visible = false;
        //    GridLength.Visible = false;
        //    var queryData = cntxt.Brick_Inward_View(TxtRefNo.Text);
        //    string idMark = "";
        //    foreach (var data in queryData)
        //    {
        //        idMark = data.BTINWD_IdMark_var;
        //    }

        //    DataTable dt = new DataTable();
        //    var list = cntxt.Brick_DA_View(TxtRefNo.Text);
        //    var count = cntxt.Brick_DA_View_count(TxtRefNo.Text).ToList();
        //    int c = count.Count();
        //    int row = 0;
        //    if (rowcount != "")
        //    {
        //        row = Convert.ToInt32(rowcount);
        //    }

        //    for (int i = 0; i < row; i++)
        //    {
        //        if (dt.Columns.Count == 0)
        //        {
        //            dt.Columns.Add("Sr. No.", typeof(string));
        //            dt.Columns.Add("ID Mark", typeof(string));
        //            dt.Columns.Add("Length", typeof(string));
        //            dt.Columns.Add("Width", typeof(string));
        //            dt.Columns.Add("Height", typeof(string));
        //            dt.Columns.Add("Avg. Length", typeof(string));
        //            dt.Columns.Add("Avg. Width", typeof(string));
        //            dt.Columns.Add("Avg. Height", typeof(string));
        //        }
        //        DataRow NewRow = dt.NewRow();
        //        dt.Rows.Add(NewRow);
        //        grdDA.DataSource = dt;
        //        grdDA.DataBind();
        //    }
        //    int g = 0;


        //    var testId = cntxt.Get_TestID(3, "BT-").ToList();
        //    int T_ID = testId[0].TEST_Id;
        //    var qua = cntxt.BrickDA_Qua(T_ID, TxtRefNo.Text).ToList();
        //    if (qua.Count() > 0)
        //    {
        //        lblquantity.Text = Convert.ToString(qua[0].BTTEST_Quantity_tint);
        //    }
        //    else
        //    {
        //        lblquantity.Text = "";
        //    }

        //    foreach (var grdData in list)
        //    {
        //        try
        //        {
        //            TextBox TxtIdMark = (TextBox)grdDA.Rows[g].Cells[1].FindControl("txtIdMark");
        //            Button TxtLength = (Button)grdDA.Rows[g].Cells[2].FindControl("TxtLength");
        //            Button TxtWidth = (Button)grdDA.Rows[g].Cells[3].FindControl("TxtWidth");
        //            Button TxtHeight = (Button)grdDA.Rows[g].Cells[4].FindControl("TxtHeight");
        //            TxtIdMark.Text = grdData.Brick_DA_ID_Mark_var;
        //            TxtLength.Text = grdData.Brick_DA_Length_int.ToString();
        //            TxtWidth.Text = grdData.Brick_DA_width_int.ToString();
        //            TxtHeight.Text = grdData.Brick_DA_Height_int.ToString();

        //            if (c == g + 1)
        //            {
        //                TextBox TxtAvgLength = (TextBox)grdDA.Rows[row / 2].Cells[5].FindControl("TxtAvgLen");
        //                TextBox TxtAvgWidth = (TextBox)grdDA.Rows[row / 2].Cells[6].FindControl("TxtAvgWidth");
        //                TextBox TxtAvgHeight = (TextBox)grdDA.Rows[row / 2].Cells[7].FindControl("TxtAvgHeight");

        //                TxtAvgLength.Text = grdData.Brick_DA_Avg_Length_int.ToString();
        //                TxtAvgWidth.Text = grdData.Brick_DA_Avg_width_int.ToString();
        //                TxtAvgHeight.Text = grdData.Brick_DA_Avg_Height_int.ToString();
        //            }
        //            g++;
        //        }
        //        catch (Exception ex)
        //        {
        //            string msg = "Number of Rows should be greater than or equal to ";
        //            var countRows = cntxt.Brick_DA_View_count(TxtRefNo.Text).ToList();
        //            int rowsC = countRows.Count();
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + msg + rowsC + " for this reference no." + "');", true);
        //            txtrows.Text = rowsC.ToString();
        //            txtrows_TextChanged(txtrows.Text, null);
        //            break;
        //        }

        //    }

        //    if (c == 0)
        //    {
        //        for (int j = 0; j < grdDA.Rows.Count; j++)
        //        {
        //            TextBox txtIdMark = (TextBox)grdDA.Rows[j].Cells[1].FindControl("txtIdMark");
        //            if (idMark == "" || idMark == null)
        //            {
        //                txtIdMark.Text = "-";
        //            }
        //            else
        //            {
        //                txtIdMark.Text = idMark;
        //            }
        //        }
        //        ChkboxWitnessBy.Checked = false;
        //        TxtWitnesBy.Visible = false;
        //        TxtWitnesBy.Text = "";
        //    }
        //    else
        //    {
        //        getFooterdata();
        //    }
        //}
        //#endregion

        //#region text change
        //protected void TxtLength_Click(object sender, EventArgs e)
        //{
        //    Session["exitFlag"] = "true";
        //    int totalRows = 0;
        //    GridHeight.Visible = false;
        //    GridWidth.Visible = false;
        //    GridLength.Visible = true;
        //    GridViewRow currentRow = (GridViewRow)((Button)sender).Parent.Parent;
        //    Session["index"] = currentRow.RowIndex;
        //    int rowindex = currentRow.RowIndex + 1;
        //    var list = cntxt.Brick_DA_Type_View(TxtRefNo.Text, "Length", rowindex);
        //    var DA_count = cntxt.Brick_DA_View_count(TxtRefNo.Text).ToList();
        //    int c = DA_count.Count;
        //    if (c == 0 && txtrows.Text == "")
        //    {
        //        totalRows = 1;
        //    }
        //    else
        //    {
        //        if (txtrows.Text != "")
        //        {
        //            totalRows = Convert.ToInt32(txtrows.Text);
        //        }
        //        else
        //        {
        //            txtrows.Focus();
        //            string displaymsg = "Rows can not be zero or blank";
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + displaymsg + "');", true);
        //            return;

        //        }
        //    }

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Sr. No.", typeof(string));
        //    dt.Columns.Add("Length1", typeof(string));
        //    dt.Columns.Add("Length2", typeof(string));
        //    dt.Columns.Add("Length3", typeof(string));
        //    DataRow NewRow = dt.NewRow();
        //    dt.Rows.Add(NewRow);
        //    GridLength.DataSource = dt;
        //    GridLength.DataBind();
        //    TextBox srNo = (TextBox)GridLength.Rows[0].Cells[1].FindControl("txtsrNo_Length");
        //    srNo.Text = Convert.ToString(rowindex);
        //    decimal total_length = 0;
        //    var type_count = cntxt.Brick_DA_Type_count(TxtRefNo.Text, "Length", rowindex).ToList();
        //    int cnt = type_count.Count();

        //    if (cnt > 0)
        //    {
        //        foreach (var grdData in list)
        //        {
        //            TextBox TxtLength1 = (TextBox)GridLength.Rows[0].Cells[1].FindControl("txtLength1");
        //            TextBox TxtLength2 = (TextBox)GridLength.Rows[0].Cells[2].FindControl("txtLength2");
        //            TextBox TxtLength3 = (TextBox)GridLength.Rows[0].Cells[3].FindControl("txtLength3");
        //            TxtLength1.Text = grdData.Brick_DA_D1.ToString();
        //            TxtLength2.Text = grdData.Brick_DA_D2.ToString();
        //            TxtLength3.Text = grdData.Brick_DA_D3.ToString();
        //            total_length = (grdData.Brick_DA_D1.Value + grdData.Brick_DA_D2.Value + grdData.Brick_DA_D3.Value) / 3;
        //        }

        //        Button TxtLength = (Button)grdDA.Rows[currentRow.RowIndex].Cells[2].FindControl("TxtLength");
        //        TxtLength.Text = total_length.ToString();
        //        decimal val = 0;

        //        for (int i = 0; i < c; i++)
        //        {
        //            Button TxLength = (Button)grdDA.Rows[i].Cells[2].FindControl("TxtLength");
        //            if (TxLength.Text != "")
        //            {
        //                val = Convert.ToDecimal(TxLength.Text);
        //                val += val;

        //            }
        //        }
        //        if (totalRows != 0)
        //        {
        //            TextBox TxtAvgLength = (TextBox)grdDA.Rows[totalRows / 2].Cells[5].FindControl("TxtAvgLen");
        //            TxtAvgLength.Text = val.ToString();
        //        }
        //    }
        //    else
        //    {
        //        addlength();
        //    }
        //}
        //protected void TxtWidth_Click(object sender, EventArgs e)
        //{
        //    Session["exitFlag"] = "true";
        //    int totalRows = 0;
        //    GridWidth.Visible = true;
        //    GridHeight.Visible = false;
        //    GridLength.Visible = false;
        //    GridViewRow currentRow = (GridViewRow)((Button)sender).Parent.Parent;
        //    Session["index"] = currentRow.RowIndex;
        //    int rowindex = currentRow.RowIndex + 1;
        //    var list = cntxt.Brick_DA_Type_View(TxtRefNo.Text, "Width", rowindex);
        //    DataTable dt = new DataTable();
        //    var DA_count = cntxt.Brick_DA_View_count(TxtRefNo.Text).ToList();
        //    int c = DA_count.Count();
        //    if (c == 0 && txtrows.Text == "")
        //    {
        //        totalRows = 1;
        //    }
        //    else
        //    {
        //        if (txtrows.Text != "")
        //        {
        //            totalRows = Convert.ToInt32(txtrows.Text);
        //        }
        //        else
        //        {
        //            txtrows.Focus();
        //            string displaymsg = "Rows can not be zero or blank";
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + displaymsg + "');", true);
        //            return;

        //        }
        //    }

        //    decimal total_width = 0;
        //    dt.Columns.Add("Sr. No.", typeof(string));
        //    dt.Columns.Add("Width1", typeof(string));
        //    dt.Columns.Add("Width2", typeof(string));
        //    dt.Columns.Add("Width3", typeof(string));
        //    DataRow NewRow = dt.NewRow();
        //    dt.Rows.Add(NewRow);
        //    GridWidth.DataSource = dt;
        //    GridWidth.DataBind();
        //    TextBox srNo = (TextBox)GridWidth.Rows[0].Cells[1].FindControl("txtsrNo_width");
        //    srNo.Text = Convert.ToString(rowindex);
        //    var type_ount = cntxt.Brick_DA_Type_count(TxtRefNo.Text, "Width", rowindex).ToList();
        //    int cnt = type_ount.Count();

        //    if (cnt > 0)
        //    {
        //        foreach (var grdData in list)
        //        {
        //            TextBox TxtWidth1 = (TextBox)GridWidth.Rows[0].Cells[1].FindControl("txtWidth1");
        //            TextBox TxtWidth2 = (TextBox)GridWidth.Rows[0].Cells[2].FindControl("txtWidth2");
        //            TextBox TxtWidth3 = (TextBox)GridWidth.Rows[0].Cells[3].FindControl("txtWidth3");
        //            TxtWidth1.Text = grdData.Brick_DA_D1.ToString();
        //            TxtWidth2.Text = grdData.Brick_DA_D2.ToString();
        //            TxtWidth3.Text = grdData.Brick_DA_D3.ToString();
        //            total_width = (grdData.Brick_DA_D1.Value + grdData.Brick_DA_D2.Value + grdData.Brick_DA_D3.Value) / 3;
        //        }

        //        Button TxtWidth = (Button)grdDA.Rows[currentRow.RowIndex].Cells[3].FindControl("TxtWidth");
        //        TxtWidth.Text = total_width.ToString();
        //        decimal val = 0;

        //        for (int i = 0; i < c; i++)
        //        {
        //            Button Txwidth = (Button)grdDA.Rows[i].Cells[2].FindControl("TxtWidth");
        //            if (Txwidth.Text != "")
        //            {
        //                val = Convert.ToDecimal(Txwidth.Text);
        //                val += val;

        //            }
        //        }

        //        if (totalRows != 0)
        //        {
        //            TextBox TxtAvgWidth = (TextBox)grdDA.Rows[totalRows / 2].Cells[6].FindControl("TxtAvgWidth");
        //            TxtAvgWidth.Text = val.ToString();
        //        }
        //    }
        //    else
        //    {
        //        addwidth();
        //    }
        //}        
        //protected void TxtHeight_Click(object sender, EventArgs e)
        //{
        //    Session["exitFlag"] = "true";
        //    int totalRows = 0;
        //    GridHeight.Visible = true;
        //    GridLength.Visible = false;
        //    GridWidth.Visible = false;
        //    GridViewRow currentRow = (GridViewRow)((Button)sender).Parent.Parent;
        //    int rowindex = currentRow.RowIndex + 1;
        //    var DA_count = cntxt.Brick_DA_View_count(TxtRefNo.Text).ToList();
        //    int c = DA_count.Count();
        //    if (c == 0 && txtrows.Text == "")
        //    {
        //        totalRows = 1;
        //    }
        //    else
        //    {

        //        if (txtrows.Text != "")
        //        {
        //            totalRows = Convert.ToInt32(txtrows.Text);
        //        }
        //        else
        //        {
        //            txtrows.Focus();
        //            string displaymsg = "Rows can not be zero or blank";
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + displaymsg + "');", true);
        //            return;

        //        }

        //    }

        //    Session["index"] = currentRow.RowIndex;
        //    var list = cntxt.Brick_DA_Type_View(TxtRefNo.Text, "Height", rowindex);
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Sr. No.", typeof(string));
        //    dt.Columns.Add("Height1", typeof(string));
        //    dt.Columns.Add("Height2", typeof(string));
        //    dt.Columns.Add("Height3", typeof(string));
        //    DataRow NewRow = dt.NewRow();
        //    dt.Rows.Add(NewRow);
        //    GridHeight.DataSource = dt;
        //    GridHeight.DataBind();
        //    TextBox srNo = (TextBox)GridHeight.Rows[0].Cells[1].FindControl("txtsrNo_Height");
        //    srNo.Text = Convert.ToString(rowindex);
        //    decimal total_height = 0;
        //    var type_ount = cntxt.Brick_DA_Type_count(TxtRefNo.Text, "Height", rowindex).ToList();
        //    int cnt = type_ount.Count();

        //    if (cnt > 0)
        //    {
        //        foreach (var grdData in list)
        //        {
        //            TextBox TxtHeight1 = (TextBox)GridHeight.Rows[0].Cells[1].FindControl("txtHeight1");
        //            TextBox TxtHeight2 = (TextBox)GridHeight.Rows[0].Cells[2].FindControl("txtHeight2");
        //            TextBox TxtHeight3 = (TextBox)GridHeight.Rows[0].Cells[3].FindControl("txtHeight3");
        //            TxtHeight1.Text = grdData.Brick_DA_D1.ToString();
        //            TxtHeight2.Text = grdData.Brick_DA_D2.ToString();
        //            TxtHeight3.Text = grdData.Brick_DA_D3.ToString();
        //            total_height = (grdData.Brick_DA_D1.Value + grdData.Brick_DA_D2.Value + grdData.Brick_DA_D3.Value) / 3;
        //        }
        //        Button TxtHeight = (Button)grdDA.Rows[currentRow.RowIndex].Cells[4].FindControl("TxtHeight");
        //        TxtHeight.Text = total_height.ToString();
        //        decimal val = 0;

        //        for (int i = 0; i < c; i++)
        //        {
        //            Button Txtheight = (Button)grdDA.Rows[i].Cells[2].FindControl("TxtHeight");
        //            if (Txtheight.Text != "")
        //            {
        //                val = Convert.ToDecimal(Txtheight.Text);
        //                val += val;

        //            }
        //        }
        //        if (totalRows != 0)
        //        {
        //            TextBox TxtAvgheight = (TextBox)grdDA.Rows[totalRows / 2].Cells[7].FindControl("TxtAvgHeight");
        //            TxtAvgheight.Text = val.ToString();
        //        }
        //    }
        //    else
        //    {
        //        addheight();
        //    }
        //}
        //#endregion

        //protected void txtrows_TextChanged(object sender, EventArgs e)
        //{
        //    Session["exitFlag"] = "true";
        //    if (txtrow.Text == "0" || txtrows.Text == "")
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Rows can not be zero or blank.');", true);
        //        return;
        //    }
        //    bindGridData(txtrows.Text);
        //}
                

        //public void addlength()
        //{

        //    string type = "Length";
        //    int inx = Convert.ToInt32(Session["index"].ToString());
        //    TextBox TxtLength1 = (TextBox)GridLength.Rows[0].Cells[1].FindControl("txtLength1");
        //    TextBox TxtLength2 = (TextBox)GridLength.Rows[0].Cells[2].FindControl("txtLength2");
        //    TextBox TxtLength3 = (TextBox)GridLength.Rows[0].Cells[3].FindControl("txtLength3");
        //    TextBox srNo = (TextBox)GridLength.Rows[0].Cells[0].FindControl("txtsrNo_Length");
        //    int totalRows = 0;
        //    var DA_count = cntxt.Brick_DA_View_count(TxtRefNo.Text).ToList();
        //    int Dcnt = DA_count.Count();
        //    if (Dcnt == 0 && txtrows.Text == "")
        //    {
        //        totalRows = 1;
        //    }
        //    else
        //    {
        //        if (txtrows.Text != "")
        //        {
        //            totalRows = Convert.ToInt32(txtrows.Text);
        //        }
        //        else
        //        {
        //            txtrows.Focus();
        //            string displaymsg = "Rows can not be zero or blank";
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + displaymsg + "');", true);
        //            return;

        //        }
        //    }

        //    bool updateflag;
        //    decimal val = 0;
        //    decimal totalavg = 0;
        //    var Brick_DA_count = cntxt.Brick_DA_Type_count(TxtRefNo.Text, type, Convert.ToInt32(srNo.Text)).ToList();
        //    int c = Brick_DA_count.Count();
        //    string txtIdMark = "", length = "", width = "", Height = "", avglength = "", avgwidth = "", avgheight = "";
        //    if (TxtLength1.Text != "" && TxtLength2.Text != "" && TxtLength3.Text != "")
        //    {
        //        if (c == 0)
        //        {
        //            updateflag = false;

        //        }
        //        else
        //        {
        //            updateflag = true;

        //            int i = 1;
        //            foreach (GridViewRow row in grdDA.Rows)
        //            {
        //                if (totalRows == i)
        //                {
        //                    avglength = ((TextBox)grdDA.Rows[totalRows - 1].Cells[5].FindControl("TxtAvgLen")).Text;
        //                    avgwidth = ((TextBox)grdDA.Rows[totalRows - 1].Cells[6].FindControl("TxtAvgWidth")).Text;
        //                    avgheight = ((TextBox)grdDA.Rows[totalRows - 1].Cells[7].FindControl("TxtAvgHeight")).Text;
        //                }
        //                i++;
        //            }
        //            if (avglength == "" && avgwidth == "" && avgheight == "")
        //            {
        //                avglength = ((TextBox)grdDA.Rows[totalRows / 2].Cells[5].FindControl("TxtAvgLen")).Text;
        //                avgwidth = ((TextBox)grdDA.Rows[totalRows / 2].Cells[6].FindControl("TxtAvgWidth")).Text;
        //                avgheight = ((TextBox)grdDA.Rows[totalRows / 2].Cells[7].FindControl("TxtAvgHeight")).Text;

        //            }

        //            cntxt.Brick_DA_Delete(TxtRefNo.Text);

        //            foreach (GridViewRow row in grdDA.Rows)
        //            {
        //                txtIdMark = (row.FindControl("txtIdMark") as TextBox).Text;
        //                length = (row.FindControl("TxtLength") as Button).Text;
        //                width = (row.FindControl("TxtWidth") as Button).Text;
        //                Height = (row.FindControl("TxtHeight") as Button).Text;

        //                if (length != "" && width != "" && Height != "")
        //                {
        //                    cntxt.BrickDA_Insert_View(TxtRefNo.Text, txtIdMark, Convert.ToInt32(length), Convert.ToInt32(width), Convert.ToInt32(Height), Convert.ToDecimal(avglength), Convert.ToDecimal(avgwidth), Convert.ToDecimal(avgheight), false);
        //                    cntxt.SubmitChanges();
        //                }

        //            }
        //        }

        //        # region add n edit length
        //        cntxt.Brick_DA_AddLenth(TxtRefNo.Text, Convert.ToInt32(srNo.Text), Convert.ToInt32(TxtLength1.Text), Convert.ToInt32(TxtLength2.Text), Convert.ToInt32(TxtLength3.Text), type, updateflag);


        //        Button TxtLength = (Button)grdDA.Rows[inx].Cells[2].FindControl("TxtLength");
        //        TxtLength.Text = Convert.ToString((Convert.ToInt32(TxtLength1.Text) + Convert.ToInt32(TxtLength2.Text) + Convert.ToInt32(TxtLength3.Text)) / 3);


        //        for (int i = 0; i < totalRows; i++)
        //        {
        //            Button TxLength = (Button)grdDA.Rows[i].Cells[2].FindControl("TxtLength");
        //            if (TxLength.Text != "")
        //            {
        //                val = Convert.ToDecimal(TxLength.Text);
        //                totalavg += val;

        //            }
        //        }
        //        totalavg = totalavg / totalRows;

        //    }
        //    if (totalRows != 0)
        //    {
        //        TextBox TxtAvgLength = (TextBox)grdDA.Rows[totalRows / 2].Cells[5].FindControl("TxtAvgLen");
        //        TxtAvgLength.Text = Math.Round(totalavg, 2).ToString();
        //    }
        //        #endregion
        //}

        //public void addwidth()
        //{
        //    string type = "Width";
        //    int inx = Convert.ToInt32(Session["index"].ToString());
        //    TextBox TxtWidth1 = (TextBox)GridWidth.Rows[0].Cells[1].FindControl("txtWidth1");
        //    TextBox TxtWidth2 = (TextBox)GridWidth.Rows[0].Cells[2].FindControl("txtWidth2");
        //    TextBox TxtWidth3 = (TextBox)GridWidth.Rows[0].Cells[3].FindControl("txtWidth3");
        //    TextBox srNo = (TextBox)GridWidth.Rows[0].Cells[0].FindControl("txtsrNo_width");
        //    bool updateflag;
        //    int totalRows = 0;
        //    decimal val = 0;
        //    var Brick_DA_count = cntxt.Brick_DA_Type_count(TxtRefNo.Text, type, Convert.ToInt32(srNo.Text)).ToList();
        //    int c = Brick_DA_count.Count();
        //    decimal totalavg = 0;

        //    var DA_count = cntxt.Brick_DA_View_count(TxtRefNo.Text).ToList();
        //    int Dcnt = DA_count.Count();
        //    if (Dcnt == 0 && txtrows.Text == "")
        //    {
        //        totalRows = 1;
        //    }
        //    else
        //    {
        //        if (txtrows.Text != "")
        //        {
        //            totalRows = Convert.ToInt32(txtrows.Text);
        //        }
        //        else
        //        {
        //            txtrows.Focus();
        //            string displaymsg = "Rows can not be zero or blank";
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + displaymsg + "');", true);
        //            return;

        //        }
        //    }

        //    if (TxtWidth1.Text != "" && TxtWidth2.Text != "" && TxtWidth3.Text != "")
        //    {
        //        if (c == 0)
        //        {
        //            updateflag = false;

        //        }
        //        else
        //        {
        //            updateflag = true;
        //        }


        //        cntxt.Brick_DA_AddLenth(TxtRefNo.Text, Convert.ToInt32(srNo.Text), Convert.ToInt32(TxtWidth1.Text), Convert.ToInt32(TxtWidth2.Text), Convert.ToInt32(TxtWidth3.Text), type, updateflag);


        //        Button TxtWidth = (Button)grdDA.Rows[inx].Cells[2].FindControl("TxtWidth");
        //        TxtWidth.Text = Convert.ToString((Convert.ToInt32(TxtWidth1.Text) + Convert.ToInt32(TxtWidth2.Text) + Convert.ToInt32(TxtWidth3.Text)) / 3);


        //        for (int i = 0; i < totalRows; i++)
        //        {
        //            Button TxLength = (Button)grdDA.Rows[i].Cells[2].FindControl("TxtWidth");
        //            if (TxLength.Text != "")
        //            {
        //                val = Convert.ToDecimal(TxLength.Text);
        //                totalavg += val;
        //            }
        //        }
        //        totalavg = totalavg / totalRows;

        //    }

        //    if (totalRows != 0)
        //    {
        //        TextBox TxtAvgwidth = (TextBox)grdDA.Rows[totalRows / 2].Cells[6].FindControl("TxtAvgWidth");
        //        TxtAvgwidth.Text = Math.Round(totalavg, 2).ToString();
        //    }
        //}

        //public void addheight()
        //{
        //    decimal totalavg = 0;
        //    string type = "Height";
        //    int inx = Convert.ToInt32(Session["index"].ToString());
        //    int totalRows = 0;
        //    bool updateflag;
        //    decimal val = 0;
        //    TextBox TxtHeight1 = (TextBox)GridHeight.Rows[0].Cells[1].FindControl("txtHeight1");
        //    TextBox TxtHeight2 = (TextBox)GridHeight.Rows[0].Cells[2].FindControl("txtHeight2");
        //    TextBox TxtHeight3 = (TextBox)GridHeight.Rows[0].Cells[3].FindControl("txtHeight3");
        //    TextBox srNo = (TextBox)GridHeight.Rows[0].Cells[0].FindControl("txtsrNo_Height");
        //    var Brick_DA_count = cntxt.Brick_DA_Type_count(TxtRefNo.Text, type, Convert.ToInt32(srNo.Text)).ToList();
        //    int c = Brick_DA_count.Count();
        //    var DA_count = cntxt.Brick_DA_View_count(TxtRefNo.Text).ToList();
        //    int Dcnt = DA_count.Count();
        //    if (Dcnt == 0 && txtrows.Text == "")
        //    {
        //        totalRows = 1;
        //    }
        //    else
        //    {
        //        if (txtrows.Text != "")
        //        {
        //            totalRows = Convert.ToInt32(txtrows.Text);
        //        }
        //        else
        //        {
        //            txtrows.Focus();
        //            string displaymsg = "Rows can not be zero or blank";
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + displaymsg + "');", true);
        //            return;

        //        }
        //    }


        //    if (TxtHeight1.Text != "" && TxtHeight2.Text != "" && TxtHeight3.Text != "")
        //    {
        //        if (c == 0)
        //        {
        //            updateflag = false;

        //        }
        //        else
        //        {
        //            updateflag = true;
        //        }


        //        cntxt.Brick_DA_AddLenth(TxtRefNo.Text, Convert.ToInt32(srNo.Text), Convert.ToInt32(TxtHeight1.Text), Convert.ToInt32(TxtHeight2.Text), Convert.ToInt32(TxtHeight3.Text), type, updateflag);


        //        Button Txtheight = (Button)grdDA.Rows[inx].Cells[2].FindControl("TxtHeight");
        //        Txtheight.Text = Convert.ToString((Convert.ToInt32(TxtHeight1.Text) + Convert.ToInt32(TxtHeight2.Text) + Convert.ToInt32(TxtHeight3.Text)) / 3);


        //        for (int i = 0; i < totalRows; i++)
        //        {
        //            Button TxLength = (Button)grdDA.Rows[i].Cells[2].FindControl("TxtHeight");
        //            if (TxLength.Text != "")
        //            {
        //                val = Convert.ToDecimal(TxLength.Text);
        //                totalavg += val;

        //            }
        //        }
        //        totalavg = totalavg / totalRows;
        //    }
        //    if (totalRows != 0)
        //    {
        //        TextBox TxtAvgheight = (TextBox)grdDA.Rows[totalRows / 2].Cells[7].FindControl("TxtAvgHeight");
        //        TxtAvgheight.Text = Math.Round(totalavg, 2).ToString();
        //    }
        //}

        
        //public void checkgridData()
        //{
        //    #region print button enable / disabled

        //    bool printFlag = false;
        //    if (TabDA.Visible == true)
        //    {
        //        for (int j = 0; j < grdDA.Rows.Count; j++)
        //        {
        //            Button TxtLength = (Button)grdDA.Rows[j].Cells[2].FindControl("TxtLength");
        //            Button TxtWidth = (Button)grdDA.Rows[j].Cells[3].FindControl("TxtWidth");
        //            Button TxtHeight = (Button)grdDA.Rows[j].Cells[4].FindControl("TxtHeight");

        //            if (TxtLength.Text == "" || TxtWidth.Text == "" || TxtHeight.Text == "")
        //            {
        //                printFlag = true;
        //                break;
        //            }
        //        }
        //    }
        //    if (TabCS.Visible == true)
        //    {
        //        for (int j = 0; j < GrdViewCS.Rows.Count; j++)
        //        {
        //            TextBox TxtCSLength = (TextBox)GrdViewCS.Rows[j].Cells[2].FindControl("TxtCSLength");
        //            TextBox TxtCSWidth = (TextBox)GrdViewCS.Rows[j].Cells[3].FindControl("TxtCSWidth");
        //            TextBox TxtLoad = (TextBox)GrdViewCS.Rows[j].Cells[4].FindControl("TxtLoad");
        //            TextBox TxtArea = (TextBox)GrdViewCS.Rows[j].Cells[4].FindControl("TxtArea");

        //            if (TxtCSLength.Text == "" || TxtCSWidth.Text == "" || TxtLoad.Text == "" || TxtArea.Text == "")
        //            {
        //                printFlag = true;
        //                break;
        //            }
        //        }
        //    }
        //    if (TabWA.Visible == true)
        //    {
        //        for (int j = 0; j < GrdViewWA.Rows.Count; j++)
        //        {
        //            TextBox TxtDryWt = (TextBox)GrdViewWA.Rows[j].Cells[2].FindControl("TxtDryWt");
        //            TextBox TxtWetWt = (TextBox)GrdViewWA.Rows[j].Cells[3].FindControl("TxtWetWt");


        //            if (TxtDryWt.Text == "" || TxtWetWt.Text == "")
        //            {
        //                printFlag = true;
        //                break;
        //            }
        //        }
        //    }
        //    if (TabDensity.Visible == true)
        //    {
        //        for (int j = 0; j < GrdViewDensity.Rows.Count; j++)
        //        {
        //            TextBox TxtDLength = (TextBox)GrdViewDensity.Rows[j].Cells[2].FindControl("TxtDLength");
        //            TextBox TxtDWidth = (TextBox)GrdViewDensity.Rows[j].Cells[3].FindControl("TxtDWidth");
        //            TextBox TxtThickness = (TextBox)GrdViewDensity.Rows[j].Cells[4].FindControl("TxtThickness");
        //            TextBox TxtOvenDryWt = (TextBox)GrdViewDensity.Rows[j].Cells[5].FindControl("TxtOvenDryWt");
        //            if (TxtDLength.Text == "" || TxtDWidth.Text == "" || TxtThickness.Text == "" || TxtOvenDryWt.Text == "")
        //            {
        //                printFlag = true;
        //                break;
        //            }
        //        }
        //    }
        //    if (printFlag == true)
        //    {
        //        BtnPrint.Enabled = false;
        //    }
        //    else
        //    {
        //        BtnPrint.Enabled = true;
        //    }
        //    #endregion
        //}
        //public void getremarkData()
        //{
        //    #region Remark Gridview

        //    //if remark is present                

        //    var remark_list = cntxt.Brick_Remark_View(TxtRefNo.Text);

        //    List<tbl_Brick_Remark> Idlist = remark_list.AsEnumerable()
        //                  .Select(o => new tbl_Brick_Remark
        //                  {
        //                      ID = o.ID,
        //                      Name = o.Name
        //                  }).ToList();
        //    var remarkcount = Idlist.Count();
        //    if (remarkcount == 0)
        //    {

        //        //if remark is not present add empty row
        //        DataTable dt1 = new DataTable();
        //        DataRow dr1 = null;
        //        dt1.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        //        dt1.Columns.Add(new DataColumn("Col2", typeof(string)));
        //        dr1 = dt1.NewRow();
        //        dr1["RowNumber"] = 1;
        //        dr1["Col2"] = string.Empty;
        //        dt1.Rows.Add(dr1);
        //        ViewState["CurrentTable"] = dt1;
        //        GrdRemark.DataSource = dt1;
        //        GrdRemark.DataBind();
        //    }
        //    else
        //    {
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        //        dt.Columns.Add(new DataColumn("Col2", typeof(string)));
        //        DataRow dr = null;
        //        for (int i = 0; i < remarkcount; i++)
        //        {

        //            dr = dt.NewRow();
        //            dr["RowNumber"] = i + 1;
        //            dr["Col2"] = string.Empty;
        //            dt.Rows.Add(dr);
        //            ViewState["CurrentTable"] = dt;

        //        }
        //        GrdRemark.DataSource = dt;
        //        GrdRemark.DataBind();
        //    }

        //    int cnt = 0;

        //    foreach (GridViewRow row in GrdRemark.Rows)
        //    {
        //        TextBox txtRemark = (row.FindControl("TxtRemark") as TextBox);
        //        if (remarkcount != 0)
        //        {
        //            txtRemark.Text = Idlist[cnt].Name;
        //        }
        //        cnt++;
        //    }



        //    #endregion
        //}

        

        //public void bindTabData()
        //{
        //    Session["txtDensityQTY"] = "";
        //    Session["txtCSQTY"] = "";
        //    Session["txtWAQTY"] = "";
        //    TabContainerBrickReport.ActiveTabIndex = 0;
        //    TabDA.Visible = false;
        //    TabCS.Visible = false;
        //    TabWA.Visible = false;
        //    TabET.Visible = false;
        //    TabDensity.Visible = false;
        //    var tabData = cntxt.Brick_Test_View(TxtRefNo.Text).ToList();
        //    int i = 0, k = 0;
        //    string[] arryForRecType = new string[5];
        //    int[] arrayForSrNo = new int[5];
        //    foreach (var tabs in tabData)
        //    {
        //        arryForRecType[i] = tabs.Test_RecType_var;
        //        arrayForSrNo[i] = Convert.ToInt32(tabs.TEST_Sr_No);
        //        i++;
        //    }

        //    while (k < arryForRecType.Length)
        //    {
        //        if (arryForRecType[k] == "BT-" && arrayForSrNo[k] == 3)
        //        {
        //            TabDA.Visible = true;
        //            var count = cntxt.Brick_DA_View_count(TxtRefNo.Text).ToList();
        //            int c = count.Count();

        //            if (c > 0)
        //            {
        //                txtrows.Text = Convert.ToString(c);
        //            }
        //            else
        //            {
        //                if (txtrows.Text == "")
        //                {
        //                    txtrows.Text = "1";
        //                }


        //            }

        //            bindGridData(txtrows.Text);
        //        }
        //        else if (arryForRecType[k] == "BT-" && arrayForSrNo[k] == 2)
        //        {
        //            TabWA.Visible = true;
        //            getWAData();
        //        }
        //        else if (arryForRecType[k] == "BT-" && arrayForSrNo[k] == 1)
        //        {
        //            TabCS.Visible = true;
        //            getCSData();
        //        }
        //        else if (arryForRecType[k] == "BT-" && arrayForSrNo[k] == 4)
        //        {
        //            TabET.Visible = true;
        //            getETData();
        //        }
        //        else if (arryForRecType[k] == "BT-" && arrayForSrNo[k] == 5)
        //        {
        //            TabDensity.Visible = true;
        //            getDensityData();
        //        }
        //        k++;
        //    }

        //    if (TabCS.Visible == false)
        //    {
        //        TxtCSqty.Text = "";
        //    }
        //    if (TabWA.Visible == false)
        //    {
        //        TxtWAQty.Text = "";
        //    }
        //    if (TabET.Visible == false)
        //    {
        //        TxtETQty.Text = "";
        //    }
        //    if (TabDensity.Visible == false)
        //    {
        //        TxtDensityQty.Text = "";
        //    }
        //    checkgridData();

        //}

        //public void getFooterdata()
        //{
        //    var witnessBy = from w in cntxt.tbl_Brick_Inwards where w.BTINWD_ReferenceNo_var == TxtRefNo.Text select w;
        //    string witnesBy = witnessBy.FirstOrDefault().BTINWD_WitnessBy_var;
        //    int approved_By = Convert.ToInt32(witnessBy.FirstOrDefault().BTINWD_ApprovedBy_tint);
        //    var approvedUser = from u in cntxt.tbl_Users where u.USER_Id == approved_By select u.USER_Name_var;
        //    string appby = approvedUser.FirstOrDefault();
        //    DrpApprovedBy.SelectedValue = approvedUser.FirstOrDefault();

        //    if (witnesBy != null)
        //    {
        //        ChkboxWitnessBy.Checked = true;
        //        TxtWitnesBy.Visible = true;
        //        TxtWitnesBy.Text = witnesBy;
        //    }
        //    else
        //    {
        //        ChkboxWitnessBy.Checked = false;
        //        TxtWitnesBy.Visible = false;
        //    }
        //}
        //public void getWAData()
        //{
        //    try
        //    {
        //        #region disply Brick WA data

        //        //idmark
        //        var queryData = cntxt.Brick_Inward_View(TxtRefNo.Text);
        //        string idMark = "";
        //        foreach (var data in queryData)
        //        {
        //            idMark = data.BTINWD_IdMark_var;
        //        }

        //        //bind quantity
        //        var quantity = from q in cntxt.tbl_Brick_Tests join t in cntxt.tbl_Tests on q.BTTEST_TEST_Id equals t.TEST_Id where q.BTTEST_ReferenceNo_var == TxtRefNo.Text && t.TEST_Sr_No == 2 select q.BTTEST_Quantity_tint;
        //        TxtWAQty.Text = quantity.FirstOrDefault().ToString();

        //        int qty = Convert.ToInt32(TxtWAQty.Text);

        //        Session["txtWAQTY"] = TxtWAQty.Text;
        //        //To Repeate Rows according to quantity 
        //        DataTable dt = new DataTable();
        //        DataColumn dc = new DataColumn();

        //        for (int i = 0; i < qty; i++)
        //        {
        //            if (dt.Columns.Count == 0)
        //            {
        //                dt.Columns.Add("Sr. No.", typeof(string));
        //                dt.Columns.Add("ID Mark", typeof(string));
        //                dt.Columns.Add("Dry Weight", typeof(string));
        //                dt.Columns.Add("Wet Weight", typeof(string));
        //                dt.Columns.Add("Water Absorption ", typeof(string));
        //                dt.Columns.Add("Avg. Water Absorption %", typeof(string));
        //                dt.Columns.Add("ID", typeof(string));
        //            }

        //            DataRow NewRow = dt.NewRow();
        //            dt.Rows.Add(NewRow);

        //            GrdViewWA.DataSource = dt;
        //            GrdViewWA.DataBind();

        //        }


        //        //Display data in grid       
        //        var list = cntxt.Brick_WA_View(TxtRefNo.Text).ToList();
        //        int cnt = 0;
        //        foreach (var grdData in list)
        //        {
        //            TextBox TxtIdMark = (TextBox)GrdViewWA.Rows[cnt].Cells[1].FindControl("lblWAIdMark");
        //            TextBox TxtDryWt = (TextBox)GrdViewWA.Rows[cnt].Cells[2].FindControl("TxtDryWt");
        //            TextBox TxtWetWt = (TextBox)GrdViewWA.Rows[cnt].Cells[3].FindControl("TxtWetWt");
        //            TextBox TxtWaterAbsorption = (TextBox)GrdViewWA.Rows[cnt].Cells[4].FindControl("TxtWaterAbsorption");
        //            TextBox TxtAvgWaterAbsorp = (TextBox)GrdViewWA.Rows[qty / 2].Cells[5].FindControl("TxtAvgWaterAbsorp");
        //            Label LblID = (Label)GrdViewWA.Rows[cnt].Cells[6].FindControl("LblWAID");
        //            string Ids = grdData.ID.ToString();
        //            LblID.Text = Ids;
        //            TxtIdMark.Text = grdData.Brick_WA_ID_Mark_var;
        //            decimal DryWt = Convert.ToDecimal(grdData.Brick_WA_Dry_wt);
        //            TxtDryWt.Text = DryWt.ToString();
        //            decimal WetWt = Convert.ToDecimal(grdData.Brick_WA_Wet_wt);
        //            TxtWetWt.Text = WetWt.ToString();
        //            decimal WaterAbsorption = Convert.ToDecimal(grdData.Brick_WA_Water_Abs);
        //            TxtWaterAbsorption.Text = WaterAbsorption.ToString();
        //            decimal AvgWaterAbsorp = Convert.ToDecimal(grdData.Brick_WA_Avg_WA);
        //            TxtAvgWaterAbsorp.Text = AvgWaterAbsorp.ToString();
        //            if (qty < 3)
        //            {
        //                TxtAvgWaterAbsorp.Text = "***";
        //            }
        //            else
        //            {
        //                TxtAvgWaterAbsorp.Text = AvgWaterAbsorp.ToString();
        //            }

        //            cnt++;
        //        }

        //        if (list.Count() == 0)
        //        {
        //            for (int j = 0; j < GrdViewWA.Rows.Count; j++)
        //            {
        //                TextBox txtIdMark = (TextBox)GrdViewWA.Rows[j].Cells[1].FindControl("lblWAIdMark");
        //                if (idMark == "" || idMark == null)
        //                {
        //                    txtIdMark.Text = "-";
        //                }
        //                else
        //                {
        //                    txtIdMark.Text = idMark;
        //                }
        //            }

        //            ChkboxWitnessBy.Checked = false;
        //            TxtWitnesBy.Visible = false;
        //            TxtWitnesBy.Text = "";

        //        }
        //        else
        //        {
        //            getFooterdata();
        //        }

        //        #endregion
        //    }
        //    catch (Exception ex)
        //    { }
        //}

        //public void getCSData()
        //{
        //    try
        //    {
        //        #region Display Brick CS data
        //        //idmark
        //        var queryData = cntxt.Brick_Inward_View(TxtRefNo.Text);
        //        string idMark = "";
        //        foreach (var data in queryData)
        //        {
        //            idMark = data.BTINWD_IdMark_var;
        //        }

        //        //To Repeate Rows according to quantity   

        //        var quantity = from q in cntxt.tbl_Brick_Tests join t in cntxt.tbl_Tests on q.BTTEST_TEST_Id equals t.TEST_Id where q.BTTEST_ReferenceNo_var == TxtRefNo.Text && t.TEST_Sr_No == 1 select q.BTTEST_Quantity_tint;
        //        TxtCSqty.Text = quantity.FirstOrDefault().ToString();

        //        int qty = Convert.ToInt32(TxtCSqty.Text);

        //        Session["txtCSQTY"] = TxtCSqty.Text;
        //        DataTable dt = new DataTable();
        //        DataColumn dc = new DataColumn();

        //        for (int i = 0; i < qty; i++)
        //        {
        //            if (dt.Columns.Count == 0)
        //            {
        //                dt.Columns.Add("Sr. No.", typeof(string));
        //                dt.Columns.Add("ID Mark", typeof(string));
        //                // dt.Columns.Add("Age", typeof(string));

        //                dt.Columns.Add("Length", typeof(string));
        //                dt.Columns.Add("Width", typeof(string));
        //                dt.Columns.Add("Load", typeof(string));

        //                dt.Columns.Add("Area", typeof(string));
        //                dt.Columns.Add("Strength", typeof(string));
        //                dt.Columns.Add("Average", typeof(string));
        //                dt.Columns.Add("ID", typeof(string));
        //            }

        //            DataRow NewRow = dt.NewRow();
        //            dt.Rows.Add(NewRow);

        //            GrdViewCS.DataSource = dt;
        //            GrdViewCS.DataBind();

        //        }


        //        //Display data in Grid
        //        var list = cntxt.Brick_CS_View(TxtRefNo.Text).ToList();
        //        int g = 0;
        //        foreach (var grdData in list)
        //        {
        //            TextBox TxtIdMark = (TextBox)GrdViewCS.Rows[g].Cells[1].FindControl("lblCSIdMark");
        //            TextBox TxtCSLength = (TextBox)GrdViewCS.Rows[g].Cells[2].FindControl("TxtCSLength");
        //            TextBox TxtCSWidth = (TextBox)GrdViewCS.Rows[g].Cells[3].FindControl("TxtCSWidth");
        //            TextBox TxtLoad = (TextBox)GrdViewCS.Rows[g].Cells[4].FindControl("TxtLoad");
        //            TextBox TxtArea = (TextBox)GrdViewCS.Rows[g].Cells[5].FindControl("TxtArea");
        //            TextBox TxtStrength = (TextBox)GrdViewCS.Rows[g].Cells[6].FindControl("TxtStrength");
        //            TextBox TxtAvg = (TextBox)GrdViewCS.Rows[qty / 2].Cells[7].FindControl("TxtAvg");
        //            Label LblID = (Label)GrdViewCS.Rows[g].Cells[8].FindControl("LblCSID");

        //            LblID.Text = grdData.ID.ToString();
        //            TxtIdMark.Text = grdData.Brick_CS_ID_Mark_var;
        //            TxtCSLength.Text = grdData.Brick_CS_Length_int.ToString();
        //            TxtCSWidth.Text = grdData.Brick_CS_width_int.ToString();
        //            TxtLoad.Text = grdData.Brick_CS_Load_Decimal.ToString();
        //            TxtArea.Text = grdData.Brick_CS_Area_int.ToString();
        //            TxtStrength.Text = grdData.Brick_CS_Strength_Decimal.ToString();
        //            decimal avg = Convert.ToDecimal(grdData.Brick_CS_Average_Decimal);
        //            if (qty < 8)
        //            {
        //                TxtAvg.Text = "***";
        //            }
        //            else
        //            {
        //                TxtAvg.Text = avg.ToString();
        //            }

        //            g++;
        //        }

        //        if (list.Count() == 0)
        //        {

        //            for (int j = 0; j < GrdViewCS.Rows.Count; j++)
        //            {
        //                TextBox txtIdMark = (TextBox)GrdViewCS.Rows[j].Cells[1].FindControl("lblCSIdMark");
        //                if (idMark == "" || idMark == null)
        //                {
        //                    txtIdMark.Text = "-";
        //                }
        //                else
        //                {
        //                    txtIdMark.Text = idMark;
        //                }
        //            }
        //            ChkboxWitnessBy.Checked = false;
        //            TxtWitnesBy.Visible = false;
        //            TxtWitnesBy.Text = "";
        //        }
        //        else
        //        {
        //            getFooterdata();
        //        }

        //        #endregion
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //}

        //public void getDensityData()
        //{
        //    try
        //    {
        //        #region Display Brick Density data
        //        //idmark
        //        var queryData = cntxt.Brick_Inward_View(TxtRefNo.Text);
        //        string idMark = "";
        //        foreach (var data in queryData)
        //        {
        //            idMark = data.BTINWD_IdMark_var;
        //        }

        //        //To Repeate Rows according to quantity   

        //        var quantity = from q in cntxt.tbl_Brick_Tests join t in cntxt.tbl_Tests on q.BTTEST_TEST_Id equals t.TEST_Id where q.BTTEST_ReferenceNo_var == TxtRefNo.Text && t.TEST_Sr_No == 5 select q.BTTEST_Quantity_tint;
        //        TxtDensityQty.Text = quantity.FirstOrDefault().ToString();

        //        int qty = Convert.ToInt32(TxtDensityQty.Text);


        //        Session["txtDensityQTY"] = TxtDensityQty.Text;

        //        DataTable dt = new DataTable();
        //        DataColumn dc = new DataColumn();

        //        for (int i = 0; i < qty; i++)
        //        {
        //            if (dt.Columns.Count == 0)
        //            {
        //                dt.Columns.Add("Sr. No.", typeof(string));
        //                dt.Columns.Add("ID Mark", typeof(string));

        //                dt.Columns.Add("Length", typeof(string));
        //                dt.Columns.Add("Width", typeof(string));
        //                dt.Columns.Add("Thickness", typeof(string));

        //                dt.Columns.Add("Oven dry wt.(Kg)", typeof(string));
        //                dt.Columns.Add("Volume(mm3)", typeof(string));
        //                dt.Columns.Add("Density(kg/m3)", typeof(string));
        //                dt.Columns.Add("Avg. Density", typeof(string));
        //                dt.Columns.Add("ID", typeof(string));
        //            }

        //            DataRow NewRow = dt.NewRow();
        //            dt.Rows.Add(NewRow);

        //            GrdViewDensity.DataSource = dt;
        //            GrdViewDensity.DataBind();

        //        }


        //        //Display data in Grid
        //        var list = cntxt.Brick_Density_View(TxtRefNo.Text).ToList();

        //        int g = 0;
        //        foreach (var grdData in list)
        //        {
        //            TextBox TxtIdMark = (TextBox)GrdViewDensity.Rows[g].Cells[1].FindControl("lblDIdMark");
        //            TextBox TxtDLength = (TextBox)GrdViewDensity.Rows[g].Cells[2].FindControl("TxtDLength");
        //            TextBox TxtDWidth = (TextBox)GrdViewDensity.Rows[g].Cells[3].FindControl("TxtDWidth");
        //            TextBox TxtThickness = (TextBox)GrdViewDensity.Rows[g].Cells[4].FindControl("TxtThickness");
        //            TextBox TxtOvenDryWt = (TextBox)GrdViewDensity.Rows[g].Cells[5].FindControl("TxtOvenDryWt");
        //            TextBox TxtVolume = (TextBox)GrdViewDensity.Rows[g].Cells[6].FindControl("TxtVolume");
        //            TextBox TxtDensity = (TextBox)GrdViewDensity.Rows[g].Cells[7].FindControl("TxtDensity");
        //            TextBox TxtAvgDensity = (TextBox)GrdViewDensity.Rows[qty / 2].Cells[8].FindControl("TxtAvgDensity");
        //            Label LblID = (Label)GrdViewDensity.Rows[g].Cells[9].FindControl("LblDID");

        //            LblID.Text = grdData.ID.ToString();
        //            TxtIdMark.Text = grdData.Brick_Density_ID_Mark_var;
        //            TxtDLength.Text = grdData.Brick_Density_Length_int.ToString();
        //            TxtDWidth.Text = grdData.Brick_Density_width_int.ToString();
        //            TxtThickness.Text = grdData.Brick_Density_Thickness_int.ToString();
        //            TxtOvenDryWt.Text = grdData.Brick_Density_Oven_dry_wt_Decimal.ToString();
        //            TxtVolume.Text = grdData.Brick_Density_Volume_int.ToString();
        //            TxtDensity.Text = grdData.Brick_Density_int.ToString();
        //            TxtAvgDensity.Text = grdData.Brick_Density_Avg_int.ToString();

        //            decimal avgDensity = Convert.ToDecimal(grdData.Brick_Density_Avg_int);
        //            if (qty < 5)
        //            {
        //                TxtAvgDensity.Text = "***";
        //            }
        //            else
        //            {
        //                TxtAvgDensity.Text = avgDensity.ToString();
        //            }

        //            g++;
        //        }

        //        if (list.Count() == 0)
        //        {

        //            for (int j = 0; j < GrdViewDensity.Rows.Count; j++)
        //            {
        //                TextBox txtIdMark = (TextBox)GrdViewDensity.Rows[j].Cells[1].FindControl("lblDIdMark");
        //                if (idMark == "" || idMark == null)
        //                {
        //                    txtIdMark.Text = "-";
        //                }
        //                else
        //                {
        //                    txtIdMark.Text = idMark;
        //                }
        //            }

        //            ChkboxWitnessBy.Checked = false;
        //            TxtWitnesBy.Visible = false;
        //            TxtWitnesBy.Text = "";

        //        }
        //        else
        //        {
        //            getFooterdata();
        //        }

        //        #endregion
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //}

        //public void getETData()
        //{
        //    try
        //    {
        //        #region disply Brick ET data

        //        //idmark
        //        var queryData = cntxt.Brick_Inward_View(TxtRefNo.Text);
        //        string idMark = "";
        //        foreach (var data in queryData)
        //        {
        //            idMark = data.BTINWD_IdMark_var;
        //        }

        //        //bind quantity
        //        var quantity = from q in cntxt.tbl_Brick_Tests join t in cntxt.tbl_Tests on q.BTTEST_TEST_Id equals t.TEST_Id where q.BTTEST_ReferenceNo_var == TxtRefNo.Text && t.TEST_Sr_No == 4 select q.BTTEST_Quantity_tint;
        //        TxtETQty.Text = quantity.FirstOrDefault().ToString();

        //        int qty = Convert.ToInt32(TxtETQty.Text);

        //        //To Repeate Rows according to quantity 
        //        DataTable dt = new DataTable();
        //        DataColumn dc = new DataColumn();

        //        for (int i = 0; i < qty; i++)
        //        {
        //            if (dt.Columns.Count == 0)
        //            {
        //                dt.Columns.Add("Sr. No.", typeof(string));
        //                dt.Columns.Add("ID Mark", typeof(string));
        //                dt.Columns.Add("Observations", typeof(string));
        //                dt.Columns.Add("ID", typeof(string));
        //            }

        //            DataRow NewRow = dt.NewRow();
        //            dt.Rows.Add(NewRow);

        //            GrdViewET.DataSource = dt;
        //            GrdViewET.DataBind();

        //        }

        //        //Display data in grid       
        //        var list = cntxt.Brick_ET_View(TxtRefNo.Text).ToList();
        //        int cnt = 0;
        //        foreach (var grdData in list)
        //        {
        //            TextBox TxtIdMark = (TextBox)GrdViewET.Rows[cnt].Cells[1].FindControl("lblETIdMark");
        //            DropDownList drplst = (DropDownList)GrdViewET.Rows[cnt].Cells[2].FindControl("DrpdwnObservations");
        //            Label LblID = (Label)GrdViewET.Rows[cnt].Cells[3].FindControl("LblETID");
        //            string Ids = grdData.ID.ToString();
        //            LblID.Text = Ids;
        //            TxtIdMark.Text = grdData.Brick_ET_ID_Mark_var;
        //            drplst.SelectedValue = grdData.Brick_ET_Observations_var;
        //            cnt++;
        //        }

        //        if (list.Count() == 0)
        //        {
        //            for (int j = 0; j < GrdViewET.Rows.Count; j++)
        //            {
        //                TextBox txtIdMark = (TextBox)GrdViewET.Rows[j].Cells[1].FindControl("lblETIdMark");
        //                if (idMark == "" || idMark == null)
        //                {
        //                    txtIdMark.Text = "-";
        //                }
        //                else
        //                {
        //                    txtIdMark.Text = idMark;
        //                }
        //            }
        //            ChkboxWitnessBy.Checked = false;
        //            TxtWitnesBy.Visible = false;
        //            TxtWitnesBy.Text = "";
        //        }
        //        else
        //        {
        //            getFooterdata();
        //        }

        //        #endregion
        //    }
        //    catch (Exception ex)
        //    { }
        //}

        //protected void TxtDryWt_TextChanged(object sender, EventArgs e)
        //{
        //    Session["exitFlag"] = "true";
        //    GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //    waterAbsCal(currentRow);
        //    Session["flagDrywtEdit"] = "true";
        //}
        //protected void TxtWetWt_TextChanged(object sender, EventArgs e)
        //{
        //    Session["exitFlag"] = "true";
        //    GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //    waterAbsCal(currentRow);

        //}

        //protected void waterAbsCal(GridViewRow currentRow)
        //{
        //    try
        //    {
        //        TextBox txtDryWet = (TextBox)currentRow.FindControl("TxtDryWt");
        //        TextBox txtWetWt = (TextBox)currentRow.FindControl("TxtWetWt");
        //        TextBox txtwaterAbsorption = (TextBox)currentRow.FindControl("TxtWaterAbsorption");
        //        decimal dryWet = 0, wetWt = 0;

        //        if (txtDryWet.Text != "" && txtWetWt.Text != "")
        //        {
        //            dryWet = Convert.ToDecimal(txtDryWet.Text);
        //            wetWt = Convert.ToDecimal(txtWetWt.Text);

        //            if (dryWet > wetWt)
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Dry wt. must be less than Wet Wt.');", true);

        //                return;
        //            }

        //            if (dryWet != 0)
        //            {
        //                decimal waterAbsorption = Math.Round((100 * (wetWt - dryWet)) / dryWet, 2);
        //                txtwaterAbsorption.Text = waterAbsorption.ToString();
        //            }
        //            else
        //            {
        //                txtwaterAbsorption.Text = "0.00";
        //            }

        //            decimal total = 0;
        //            int qty = Convert.ToInt32(TxtWAQty.Text);
        //            foreach (GridViewRow row in GrdViewWA.Rows)
        //            {
        //                var numberLabel = row.FindControl("TxtWaterAbsorption") as TextBox;

        //                decimal number;
        //                if (decimal.TryParse(numberLabel.Text, out number))
        //                {
        //                    total += number;
        //                }

        //            }

        //            int rows = qty / 2;
        //            string emptyRemark = ((TextBox)GrdViewWA.Rows[qty - 1].Cells[4].FindControl("TxtWaterAbsorption")).Text;
        //            if (qty >= 3)
        //            {
        //                decimal avgWA = total / qty;
        //                decimal average = Math.Round(avgWA, 2);

        //                if (emptyRemark != "")
        //                {
        //                    TextBox TxtAvgWaterAbsorp = (TextBox)GrdViewWA.Rows[rows].Cells[5].FindControl("TxtAvgWaterAbsorp");
        //                    TxtAvgWaterAbsorp.Text = average.ToString();
        //                }
        //            }
        //            else
        //            {
        //                if (emptyRemark != "")
        //                {
        //                    TextBox TxtAvgWaterAbsorp = (TextBox)GrdViewWA.Rows[qty / 2].Cells[5].FindControl("TxtAvgWaterAbsorp");
        //                    TxtAvgWaterAbsorp.Text = "***";
        //                }

        //            }

        //        }

        //    }
        //    catch
        //    { }
        //}

       
        //protected void TxtCSLength_TextChanged(object sender, EventArgs e)
        //{
        //    Session["exitFlag"] = "true";
        //    GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //    areacal(currentRow);
        //}
        
        //protected void TxtCSWidth_TextChanged(object sender, EventArgs e)
        //{
        //    Session["exitFlag"] = "true";
        //    GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //    areacal(currentRow);
        //}

        //public void areacal(GridViewRow currentRow)
        //{
        //    try
        //    {
        //        TextBox TxtCSLength = (TextBox)currentRow.FindControl("TxtCSLength");
        //        TextBox TxtCSWidth = (TextBox)currentRow.FindControl("TxtCSWidth");
        //        int length = 0, width = 0;
        //        if (TxtCSLength.Text != "" && TxtCSWidth.Text != "")
        //        {
        //            length = Convert.ToInt32(TxtCSLength.Text);
        //            width = Convert.ToInt32(TxtCSWidth.Text);
        //            int area = (length * width);

        //            TextBox txtArea = (TextBox)currentRow.FindControl("TxtArea");
        //            txtArea.Text = area.ToString();

        //            //change strength            
        //            TextBox txtLoad = (TextBox)currentRow.FindControl("TxtLoad");
        //            decimal load = Math.Round(Convert.ToDecimal(txtLoad.Text), 1);
        //            if (txtLoad.Text != "")
        //            {
        //                TextBox txtStrength = (TextBox)currentRow.FindControl("TxtStrength");

        //                if (area != 0)
        //                {
        //                    decimal strength = Math.Round((load / area) * 1000, 2);
        //                    txtStrength.Text = strength.ToString();
        //                }
        //                else
        //                {
        //                    txtStrength.Text = "0.00";
        //                }
        //            }
        //            avgCal();
        //        }


        //    }
        //    catch
        //    { }


        //}
        //public void avgCal()
        //{
        //    decimal total = 0, avg = 0; int rowIndex = 0;

        //    foreach (GridViewRow row in GrdViewCS.Rows)
        //    {
        //        rowIndex = row.RowIndex + 1;
        //        var txtstrength = row.FindControl("TxtStrength") as TextBox;
        //        decimal number;
        //        if (decimal.TryParse(txtstrength.Text, out number))
        //        {
        //            total += number;

        //        }

        //    }
        //    int qty = Convert.ToInt32(TxtCSqty.Text);
        //    string emptyRow = ((TextBox)GrdViewCS.Rows[qty - 1].Cells[5].FindControl("TxtLoad")).Text;
        //    if (qty >= 8)
        //    {
        //        avg = total / qty;
        //        decimal average = Math.Round(avg, 2);
        //        int rows = qty / 2;
        //        if (emptyRow != "")
        //        {
        //            TextBox TxtAvg = (TextBox)GrdViewCS.Rows[rows].Cells[8].FindControl("TxtAvg");
        //            TxtAvg.Text = average.ToString();
        //        }
        //    }
        //    else
        //    {
        //        if (qty < 8)
        //        {
        //            if (emptyRow != "")
        //            {
        //                TextBox TxtAvg = (TextBox)GrdViewCS.Rows[qty / 2].Cells[8].FindControl("TxtAvg");
        //                TxtAvg.Text = "***";
        //            }

        //        }
        //    }

        //}


        //protected void TxtLoad_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Session["exitFlag"] = "true";
        //        int qty = Convert.ToInt32(TxtCSqty.Text);
        //        GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //        TextBox txtLoad = (TextBox)currentRow.FindControl("TxtLoad");
        //        decimal load = Math.Round(Convert.ToDecimal(txtLoad.Text), 1);

        //        TextBox txtArea = (TextBox)currentRow.FindControl("TxtArea");
        //        int area = Convert.ToInt32(txtArea.Text);
        //        TextBox txtStrength = (TextBox)currentRow.FindControl("TxtStrength");

        //        if (area != 0)
        //        {
        //            decimal strength = Math.Round((load / area) * 1000, 2);
        //            txtStrength.Text = strength.ToString();
        //        }
        //        else
        //        {
        //            txtStrength.Text = "0.00";
        //        }

        //        avgCal();
        //    }
        //    catch (Exception ex)
        //    { }

        //}

        //protected void LnkbtnFetch_Click(object sender, EventArgs e)
        //{
        //    if (DrpRefNo.SelectedValue != "-Select-")
        //    {
        //        Session["exitFlag"] = "false";
        //        TxtRefNo.Text = DrpRefNo.SelectedValue;
        //        bindHeaderData();
        //        txtrows.Text = "";
        //        bindTabData();
        //        getremarkData();
        //        var QueryRefNo = cntxt.Brick_RefNo_View(true);
        //        List<string> refNoList = new List<string>();
        //        foreach (var q in QueryRefNo)
        //        {
        //            // TxtRefNo.Text = q.SOLIDINWD_ReferenceNo_var;
        //            refNoList.Add(q.BTINWD_ReferenceNo_var);
        //        }
        //        DrpRefNo.DataSource = refNoList;
        //        DrpRefNo.DataBind();
        //        DrpRefNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select-"));
        //        DrpRefNo.Items.Remove(TxtRefNo.Text);
        //        bindApprovedBy();

        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning", "alert('Please Select Reference No.');", true);
        //    }
        //}

        //public void bindApprovedBy()
        //{
        //    var approvedBy = from k in cntxt.tbl_Users where k.USER_Approve_right_bit == true select k.USER_Name_var;
        //    DrpApprovedBy.DataSource = approvedBy;
        //    DrpApprovedBy.DataBind();
        //}
             

        //protected void imgBtnDeleteRow_Click(object sender, EventArgs e)
        //{
        //    GridViewRow currentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        //    int rowIndex = currentRow.RowIndex;
        //    TextBox TxtRemark = (TextBox)GrdRemark.Rows[rowIndex].Cells[1].FindControl("TxtRemark");
        //    string remarkname = TxtRemark.Text;

        //    var remarkid = from re in cntxt.tbl_Brick_Remarks where re.Name == remarkname select re.ID;

        //    var rm = from remarkdetail in cntxt.tbl_Brick_Remark_dtls where remarkdetail.Brick_Remark_ID == remarkid.FirstOrDefault() && remarkdetail.Brick_ReferenceNo_var == TxtRefNo.Text select remarkdetail.ID;

        //    int id = Convert.ToInt32(rm.FirstOrDefault());
        //    if (id != 0)
        //    {
        //        int arraycnt = remarkDelId.Count() + 1;
        //        Array.Resize(ref remarkDelId, arraycnt);
        //        remarkDelId[arraycnt - 1] = id;
        //    }

        //    SetRowData();
        //    if (ViewState["CurrentTable"] != null)
        //    {
        //        DataTable dt = (DataTable)ViewState["CurrentTable"];
        //        DataRow drCurrentRow = null;
        //        //int rowIndex = Convert.ToInt32(e.RowIndex);
        //        if (dt.Rows.Count > 0)
        //        {
        //            dt.Rows.Remove(dt.Rows[rowIndex]);
        //            drCurrentRow = dt.NewRow();
        //            ViewState["CurrentTable"] = dt;
        //            GrdRemark.DataSource = dt;
        //            GrdRemark.DataBind();

        //            SetPreviousData();
        //        }
        //        if (dt.Rows.Count == 0)
        //        {
        //            DataTable dt1 = new DataTable();
        //            DataRow dr1 = null;
        //            dt1.Columns.Add(new DataColumn("RowNumber", typeof(string)));
        //            dt1.Columns.Add(new DataColumn("Col2", typeof(string)));
        //            dr1 = dt1.NewRow();
        //            dr1["RowNumber"] = 1;
        //            dr1["Col2"] = string.Empty;
        //            dt1.Rows.Add(dr1);
        //            ViewState["CurrentTable"] = dt1;
        //            GrdRemark.DataSource = dt1;
        //            GrdRemark.DataBind();
        //        }
        //    }
        //}

        
        //private void AddNewRow()
        //{
        //    int rowIndex = 0;

        //    if (ViewState["CurrentTable"] != null)
        //    {
        //        DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
        //        DataRow drCurrentRow = null;
        //        if (dtCurrentTable.Rows.Count > 0)
        //        {
        //            for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
        //            {
        //                TextBox TxtRemark = (TextBox)GrdRemark.Rows[rowIndex].Cells[1].FindControl("TxtRemark");
        //                drCurrentRow = dtCurrentTable.NewRow();
        //                drCurrentRow["RowNumber"] = i + 1;
        //                dtCurrentTable.Rows[i - 1]["Col2"] = TxtRemark.Text;
        //                rowIndex++;
        //            }
        //            dtCurrentTable.Rows.Add(drCurrentRow);
        //            ViewState["CurrentTable"] = dtCurrentTable;

        //            GrdRemark.DataSource = dtCurrentTable;
        //            GrdRemark.DataBind();
        //        }
        //    }
        //    else
        //    {

        //    }
        //    SetPreviousData();
        //}

        //private void SetPreviousData()
        //{
        //    int rowIndex = 0;
        //    if (ViewState["CurrentTable"] != null)
        //    {
        //        DataTable dt = (DataTable)ViewState["CurrentTable"];
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                TextBox TxtRemark = (TextBox)GrdRemark.Rows[rowIndex].Cells[1].FindControl("TxtRemark");
        //                TxtRemark.Text = dt.Rows[i]["Col2"].ToString();
        //                rowIndex++;
        //            }
        //        }
        //    }
        //}

        //private void SetRowData()
        //{
        //    int rowIndex = 0;

        //    if (ViewState["CurrentTable"] != null)
        //    {
        //        DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
        //        DataRow drCurrentRow = null;
        //        if (dtCurrentTable.Rows.Count > 0)
        //        {
        //            for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
        //            {
        //                TextBox TxtRemark = (TextBox)GrdRemark.Rows[rowIndex].Cells[1].FindControl("TxtRemark");
        //                drCurrentRow = dtCurrentTable.NewRow();
        //                drCurrentRow["RowNumber"] = i + 1;
        //                dtCurrentTable.Rows[i - 1]["Col2"] = TxtRemark.Text;
        //                rowIndex++;
        //            }

        //            ViewState["CurrentTable"] = dtCurrentTable;

        //        }
        //    }
        //    else
        //    {

        //    }

        //}

        
        //public void saveCSdata()
        //{

        //    string length = "", width = "", area = "", load = "", strength = "", average = "", age = "", IdMark = "";
        //    int Pk_ID = 0;
        //    decimal totalAvr = 0;

        //    var referenceno = cntxt.Brick_CS_View(TxtRefNo.Text).ToList();

        //    int rowcount = 0, cnt = 0;
        //    foreach (GridViewRow row in GrdViewCS.Rows)
        //    {
        //        length = (row.FindControl("TxtCSLength") as TextBox).Text;
        //        width = (row.FindControl("TxtCSWidth") as TextBox).Text;
        //        area = (row.FindControl("TxtArea") as TextBox).Text;
        //        load = (row.FindControl("TxtLoad") as TextBox).Text;
        //        strength = (row.FindControl("TxtStrength") as TextBox).Text;
        //        int qty = Convert.ToInt32(TxtCSqty.Text);

        //        if (rowcount == qty - 1)
        //        {
        //            average = ((TextBox)GrdViewCS.Rows[qty / 2].Cells[8].FindControl("TxtAvg")).Text;
        //            if (average == "***" || average == "")
        //            {
        //                average = "0.00";
        //                totalAvr = Convert.ToDecimal(average);
        //            }
        //            else
        //            {
        //                totalAvr = Convert.ToDecimal(average);
        //            }
        //        }


        //        IdMark = (row.FindControl("lblCSIdMark") as TextBox).Text;

        //        if (referenceno.Count() == 0)
        //        {
        //            cntxt.Brick_CS_Update(null, TxtRefNo.Text, IdMark, Convert.ToInt32(length), Convert.ToInt32(width), Convert.ToInt32(area), Convert.ToDecimal(load), Convert.ToDecimal(strength), totalAvr, false);
        //            cntxt.SubmitChanges();
        //        }
        //        else
        //        {
        //            Pk_ID = Convert.ToInt32((row.FindControl("LblCSID") as Label).Text);
        //            cntxt.Brick_CS_Update(Pk_ID, TxtRefNo.Text, IdMark, Convert.ToInt32(length), Convert.ToInt32(width), Convert.ToInt32(area), Convert.ToDecimal(load), Convert.ToDecimal(strength), totalAvr, true);
        //            cntxt.SubmitChanges();
        //        }

        //        rowcount++;
        //    }

        //    //fetch PK Id
        //    var list = cntxt.Brick_CS_View(TxtRefNo.Text).ToList();
        //    int Idcnt = 0;
        //    foreach (var grdData in list)
        //    {
        //        Label LblID = (Label)GrdViewCS.Rows[Idcnt].Cells[8].FindControl("LblCSID");
        //        LblID.Text = grdData.ID.ToString();
        //        Idcnt++;
        //    }
        //}

        //public void saveDA()
        //{
        //    string txtIdMark = "", length = "", width = "", Height = "", avglength = "", avgwidth = "", avgheight = "";
        //    var referenceno = cntxt.Brick_DA_View(TxtRefNo.Text).ToList();
        //    int count = referenceno.Count();

        //    int totalRows = 0;
        //    var DA_count = cntxt.Brick_DA_View_count(TxtRefNo.Text).ToList();
        //    int Dcnt = DA_count.Count();
        //    if (Dcnt == 0 && txtrows.Text == "")
        //    {
        //        totalRows = 1;
        //    }
        //    else
        //    {
        //        if (txtrows.Text != "" || txtrows.Text != "0")
        //        {
        //            totalRows = Convert.ToInt32(txtrows.Text);
        //        }
        //        else
        //        {
        //            txtrows.Focus();
        //            string displaymsg = "Rows can not be zero or blank";
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + displaymsg + "');", true);
        //            return;

        //        }
        //    }

        //    totalRows = totalRows / 2;

        //    int i = 0;
        //    foreach (GridViewRow row in grdDA.Rows)
        //    {
        //        if (totalRows == i)
        //        {
        //            avglength = ((TextBox)grdDA.Rows[totalRows].Cells[5].FindControl("TxtAvgLen")).Text;
        //            avgwidth = ((TextBox)grdDA.Rows[totalRows].Cells[6].FindControl("TxtAvgWidth")).Text;
        //            avgheight = ((TextBox)grdDA.Rows[totalRows].Cells[7].FindControl("TxtAvgHeight")).Text;
        //        }
        //        i++;
        //    }

        //    if (count != 0)
        //    {
        //        cntxt.Brick_DA_Delete(TxtRefNo.Text);
        //    }

        //    foreach (GridViewRow row in grdDA.Rows)
        //    {
        //        txtIdMark = (row.FindControl("txtIdMark") as TextBox).Text;
        //        length = (row.FindControl("TxtLength") as Button).Text;
        //        width = (row.FindControl("TxtWidth") as Button).Text;
        //        Height = (row.FindControl("TxtHeight") as Button).Text;

        //        if (length != "" && width != "" && Height != "")
        //        {
        //            cntxt.BrickDA_Insert_View(TxtRefNo.Text, txtIdMark, Convert.ToInt32(length), Convert.ToInt32(width), Convert.ToInt32(Height), Convert.ToDecimal(avglength), Convert.ToDecimal(avgwidth), Convert.ToDecimal(avgheight), false);
        //            cntxt.SubmitChanges();
        //        }

        //    }
        //}

        //public void saveWAdata()
        //{
        //    var referenceno = cntxt.Brick_WA_View(TxtRefNo.Text).ToList();
        //    string IdMark = "", average = "";
        //    decimal dryWt = 0, Wetwt = 0, waterAbs = 0;
        //    int Pk_ID = 0;

        //    int rowcount = 0;
        //    int cnt = 0;
        //    foreach (GridViewRow row in GrdViewWA.Rows)
        //    {
        //        dryWt = Convert.ToDecimal((row.FindControl("TxtDryWt") as TextBox).Text);
        //        Wetwt = Convert.ToDecimal((row.FindControl("TxtWetWt") as TextBox).Text);
        //        waterAbs = Convert.ToDecimal((row.FindControl("TxtWaterAbsorption") as TextBox).Text);
        //        IdMark = (row.FindControl("lblWAIdMark") as TextBox).Text;

        //        int qty = Convert.ToInt32(TxtWAQty.Text);
        //        if (rowcount == qty - 1)
        //        {
        //            average = ((TextBox)GrdViewWA.Rows[qty / 2].Cells[5].FindControl("TxtAvgWaterAbsorp")).Text;
        //        }
        //        if (average == "***" || average == "")
        //        {
        //            average = "0.00";
        //        }

        //        if (referenceno.Count() == 0)
        //        {
        //            cntxt.Brick_WA_Update(null, TxtRefNo.Text, IdMark, dryWt, Wetwt, Convert.ToDecimal(waterAbs), Convert.ToDecimal(average), false);
        //            cntxt.SubmitChanges();
        //        }
        //        else
        //        {
        //            Pk_ID = Convert.ToInt32((row.FindControl("LblWAID") as Label).Text);
        //            cntxt.Brick_WA_Update(Pk_ID, TxtRefNo.Text, IdMark, dryWt, Wetwt, Convert.ToDecimal(waterAbs), Convert.ToDecimal(average), true);
        //            cntxt.SubmitChanges();

        //        }
        //        rowcount++;
        //    }

        //    //fetch PK Id
        //    var list = cntxt.Brick_WA_View(TxtRefNo.Text).ToList();
        //    int Idcnt = 0;
        //    foreach (var grdData in list)
        //    {
        //        Label LblID = (Label)GrdViewWA.Rows[Idcnt].Cells[6].FindControl("LblWAID");
        //        LblID.Text = grdData.ID.ToString();
        //        Idcnt++;
        //    }
        //}

        //public void saveDensitydata()
        //{
        //    string length = "", width = "", thickness = "", ovenDrywt = "", volume = "", density = "", average = "", IdMark = "";
        //    int Pk_ID = 0;
        //    decimal totalAvgDensity = 0;

        //    var referenceno = cntxt.Brick_Density_View(TxtRefNo.Text).ToList();

        //    int rowcount = 0, cnt = 0;
        //    foreach (GridViewRow row in GrdViewDensity.Rows)
        //    {
        //        length = (row.FindControl("TxtDLength") as TextBox).Text;
        //        width = (row.FindControl("TxtDWidth") as TextBox).Text;
        //        thickness = (row.FindControl("TxtThickness") as TextBox).Text;
        //        ovenDrywt = (row.FindControl("TxtOvenDryWt") as TextBox).Text;
        //        volume = (row.FindControl("TxtVolume") as TextBox).Text;
        //        density = (row.FindControl("TxtDensity") as TextBox).Text;
        //        int qty = Convert.ToInt32(TxtDensityQty.Text);

        //        if (rowcount == qty - 1)
        //        {
        //            average = ((TextBox)GrdViewDensity.Rows[qty / 2].Cells[8].FindControl("TxtAvgDensity")).Text;
        //            if (average == "***" || average == "")
        //            {
        //                average = "0.00";
        //                totalAvgDensity = Convert.ToDecimal(average);
        //            }
        //            else
        //            {
        //                totalAvgDensity = Convert.ToDecimal(average);
        //            }
        //        }

        //        IdMark = (row.FindControl("lblDIdMark") as TextBox).Text;

        //        if (referenceno.Count() == 0)
        //        {
        //            cntxt.Brick_Density_Update(null, TxtRefNo.Text, IdMark, Convert.ToInt32(length), Convert.ToInt32(width), Convert.ToInt32(thickness), Convert.ToDecimal(ovenDrywt), Convert.ToInt32(volume), Convert.ToInt32(totalAvgDensity), Convert.ToInt32(density), false);
        //            cntxt.SubmitChanges();
        //        }
        //        else
        //        {
        //            Pk_ID = Convert.ToInt32((row.FindControl("LblDID") as Label).Text);
        //            cntxt.Brick_Density_Update(Pk_ID, TxtRefNo.Text, IdMark, Convert.ToInt32(length), Convert.ToInt32(width), Convert.ToInt32(thickness), Convert.ToDecimal(ovenDrywt), Convert.ToInt32(volume), Convert.ToInt32(totalAvgDensity), Convert.ToInt32(density), true);
        //            cntxt.SubmitChanges();
        //        }

        //        rowcount++;

        //    }

        //    //fetch PK Id
        //    var list = cntxt.Brick_Density_View(TxtRefNo.Text).ToList();
        //    int Idcnt = 0;
        //    foreach (var grdData in list)
        //    {
        //        Label LblID = (Label)GrdViewDensity.Rows[Idcnt].Cells[9].FindControl("LblDID");
        //        LblID.Text = grdData.ID.ToString();
        //        Idcnt++;
        //    }
        //}

        //public void saveETdata()
        //{
        //    string IdMark = "", observation = "";
        //    int Pk_ID = 0;

        //    var referenceno = cntxt.Brick_ET_View(TxtRefNo.Text).ToList();

        //    int rowcount = 0, cnt = 0;
        //    foreach (GridViewRow row in GrdViewET.Rows)
        //    {
        //        int qty = Convert.ToInt32(TxtETQty.Text);

        //        IdMark = (row.FindControl("lblETIdMark") as TextBox).Text;
        //        observation = (row.FindControl("DrpdwnObservations") as DropDownList).Text;

        //        if (referenceno.Count() == 0)
        //        {
        //            cntxt.Brick_ET_Update(null, TxtRefNo.Text, IdMark, observation, false);
        //            cntxt.SubmitChanges();
        //        }
        //        else
        //        {
        //            Pk_ID = Convert.ToInt32((row.FindControl("LblETID") as Label).Text);
        //            cntxt.Brick_ET_Update(Pk_ID, TxtRefNo.Text, IdMark, observation, true);
        //            cntxt.SubmitChanges();
        //        }

        //        rowcount++;
        //    }

        //    //fetch PK Id
        //    var list = cntxt.Brick_ET_View(TxtRefNo.Text).ToList();
        //    int Idcnt = 0;
        //    foreach (var grdData in list)
        //    {
        //        Label LblID = (Label)GrdViewET.Rows[Idcnt].Cells[3].FindControl("LblETID");
        //        LblID.Text = grdData.ID.ToString();
        //        Idcnt++;
        //    }
        //}
        //public void saveRemarkdata()
        //{
        //    #region insert data in remark table

        //    var remarkName = from rm in cntxt.tbl_Brick_Remarks select rm;

        //    var remarklist = cntxt.Brick_Remark_View(TxtRefNo.Text);
        //    List<tbl_Brick_Remark_dtl> remarkIdlist = remarklist.AsEnumerable()
        //                  .Select(o => new tbl_Brick_Remark_dtl
        //                  {
        //                      ID = o.ID,
        //                      Brick_Remark_ID = o.Brick_Remark_ID
        //                  }).ToList();
        //    var remarkcnt = remarkIdlist.Count();


        //    //first delete data
        //    var rDetail = from rDtl in cntxt.tbl_Brick_Remark_dtls where rDtl.Brick_ReferenceNo_var == TxtRefNo.Text select rDtl;
        //    cntxt.tbl_Brick_Remark_dtls.DeleteAllOnSubmit(rDetail);
        //    cntxt.SubmitChanges();

        //    if (remarkName.Count() > 0)
        //    {
        //        int count = 0;

        //        foreach (GridViewRow remarkRow in GrdRemark.Rows)
        //        {
        //            string remark_txt = (remarkRow.FindControl("TxtRemark") as TextBox).Text;

        //            foreach (var rmName in remarkName)
        //            {
        //                if (rmName.Name == remark_txt)
        //                {
        //                    count = 0;
        //                    break;
        //                }
        //                else
        //                {
        //                    count = 1;
        //                }
        //            }

        //            if (count != 0)
        //            {
        //                if (remark_txt != "")
        //                {
        //                    //sp
        //                    cntxt.Brick_Remark(null, remark_txt, false);
        //                    cntxt.SubmitChanges();

        //                    var remarkId = from r in cntxt.tbl_Brick_Remarks where r.Name == remark_txt select r.ID;

        //                    cntxt.Brick_Remark_dtl(null, TxtRefNo.Text, remarkId.FirstOrDefault(), false);
        //                    cntxt.SubmitChanges();
        //                }
        //            }
        //            else
        //            {
        //                var remark_ID = from r in cntxt.tbl_Brick_Remarks where r.Name == remark_txt select r.ID;
        //                var r_dtl = from rdtl in cntxt.tbl_Brick_Remark_dtls where rdtl.Brick_ReferenceNo_var == TxtRefNo.Text && rdtl.Brick_Remark_ID == remark_ID.FirstOrDefault() select rdtl;

        //                if (r_dtl.Count() == 0)
        //                {
        //                    var remarkId = from r in cntxt.tbl_Brick_Remarks where r.Name == remark_txt select r.ID;

        //                    if (remark_txt != "")
        //                    {
        //                        //sp
        //                        cntxt.Brick_Remark_dtl(null, TxtRefNo.Text, remarkId.FirstOrDefault(), false);
        //                        cntxt.SubmitChanges();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {

        //        int count = 0;
        //        foreach (GridViewRow remarkRow in GrdRemark.Rows)
        //        {
        //            string remark_txt = (remarkRow.FindControl("TxtRemark") as TextBox).Text;
        //            foreach (var rmName in remarkName)
        //            {
        //                if (rmName.Name == remark_txt)
        //                {
        //                    count = 0;
        //                    break;
        //                }
        //                else
        //                {
        //                    count = 1;
        //                }
        //            }

        //            if (count != 0 || remarkName.Count() == 0)
        //            {
        //                if (remark_txt != "")
        //                {
        //                    //sp for add new remark
        //                    cntxt.Brick_Remark(null, remark_txt, false);
        //                    cntxt.SubmitChanges();

        //                    var remarkId = from r in cntxt.tbl_Brick_Remarks where r.Name == remark_txt select r.ID;

        //                    cntxt.Brick_Remark_dtl(null, TxtRefNo.Text, remarkId.FirstOrDefault(), false);
        //                    cntxt.SubmitChanges();
        //                }
        //            }
        //            else
        //            {
        //                var remark_ID = from r in cntxt.tbl_Brick_Remarks where r.Name == remark_txt select r.ID;
        //                var r_dtl = from rdtl in cntxt.tbl_Brick_Remark_dtls where rdtl.Brick_ReferenceNo_var == TxtRefNo.Text && rdtl.Brick_Remark_ID == remark_ID.FirstOrDefault() select rdtl;

        //                if (r_dtl.Count() == 0)
        //                {
        //                    var remarkId = from r in cntxt.tbl_Brick_Remarks where r.Name == remark_txt select r.ID;

        //                    if (remark_txt != "")
        //                    {
        //                        //sp
        //                        cntxt.Brick_Remark_dtl(null, TxtRefNo.Text, remarkId.FirstOrDefault(), false);
        //                        cntxt.SubmitChanges();
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    #endregion

        //    #region delete remarks
        //    //remarkDelId

        //    for (int d_id = 0; d_id < remarkDelId.Count(); d_id++)
        //    {

        //        var delete = from del in cntxt.tbl_Brick_Remark_dtls where del.ID == remarkDelId[d_id] select del;
        //        foreach (var d in delete)
        //        {
        //            cntxt.tbl_Brick_Remark_dtls.DeleteOnSubmit(d);
        //        }
        //        cntxt.SubmitChanges();
        //    }

        //    #endregion
        //}

        //protected void BtnSave_Click(object sender, EventArgs e)
        //{
        //    if (ValidateData() == true)
        //    {
        //        if (TabDA.Visible == true)
        //        {
        //            saveDA();
        //        }

        //        if (TabCS.Visible == true)
        //        {
        //            saveCSdata();
        //        }
        //        if (TabWA.Visible == true)
        //        {
        //            saveWAdata();
        //        }
        //        if (TabDensity.Visible == true)
        //        {
        //            saveDensitydata();
        //        }
        //        if (TabET.Visible == true)
        //        {
        //            saveETdata();
        //        }

        //        #region Update data in Brick inward
        //        string approved_by = "", checked_by = "";
        //        int approvedBy = 0, checkedBy = 0;

        //        approved_by = DrpApprovedBy.SelectedValue;
        //        checked_by = TxtcheckBy.Text;

        //        var approved = from a in cntxt.tbl_Users where a.USER_Name_var == approved_by select a.USER_Id;
        //        approvedBy = approved.FirstOrDefault();
        //        var checkd = from c in cntxt.tbl_Users where c.USER_Name_var == checked_by select c.USER_Id;
        //        checkedBy = checkd.FirstOrDefault();

        //        string wtnesBy = null;
        //        bool WitnessFlag = false;
        //        if (ChkboxWitnessBy.Checked)
        //        {
        //            wtnesBy = TxtWitnesBy.Text;
        //            WitnessFlag = true;
        //        }

        //        cntxt.Brick_Inword_Update(TxtRefNo.Text, TxtDesc.Text, TxtSupplierName.Text, wtnesBy, Convert.ToDateTime(TxtDateOfTest.Text), TxtReportNo.Text, approvedBy, checkedBy, WitnessFlag);
        //        cntxt.SubmitChanges();
        //        #endregion

        //        #region save remark data
        //        saveRemarkdata();
        //        #endregion

        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Report Saved Successfully');", true);
        //        BtnPrint.Enabled = true;
        //    }

        //}


        //public string compl_notes(string average)
        //{
        //    #region comp

        //    int flgCompl = 0;
        //    string tGrade = "", Complaince_Note = ""; ;
        //    double avg = 0;
        //    if (average != "***")
        //    {
        //        avg = Convert.ToDouble(average);
        //    }

        //    if (average != "***")
        //    {
        //        if (avg >= 3.5)
        //        {
        //            flgCompl = 1;

        //            if (avg >= 15)
        //            {

        //                tGrade = "A(15.0)";
        //            }
        //            else if (avg >= 12.5)
        //            {

        //                tGrade = "A(12.5)";
        //            }
        //            else if (avg >= 10)
        //            {

        //                tGrade = "A(10)";
        //            }
        //            else if (avg >= 8.5)
        //            {

        //                tGrade = "A(8.5)";
        //            }
        //            else if (avg >= 7)
        //            {

        //                tGrade = "A(7.0)";
        //            }
        //            else if (avg >= 5.5)
        //            {

        //                tGrade = "A(5.5)";
        //            }

        //            else if (avg >= 4.5)
        //            {

        //                tGrade = "A(4.5)";
        //            }
        //            else if (avg >= 3.5)
        //            {

        //                tGrade = "A(3.5)";
        //            }
        //            else
        //            {
        //                tGrade = "";
        //                flgCompl = 3;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        flgCompl = 3;
        //    }

        //    if (flgCompl > 0)
        //    {
        //        if (flgCompl == 1)
        //        {
        //            Complaince_Note = tGrade;
        //        }

        //        if (flgCompl == 3)
        //        {
        //            Complaince_Note = "Tested sample does not comply to any grade of Masonary Units";
        //        }
        //    }
        //    #endregion

        //    return Complaince_Note;
        //}

        //protected Boolean ValidateData()
        //{
        //    string dispalyMsg = "";
        //    Boolean valid = true;

        //    // Witness by validation
        //    if (ChkboxWitnessBy.Checked == true)
        //    {
        //        if (TxtWitnesBy.Text == "")
        //        {
        //            dispalyMsg = "Please Enter Witness By Name.";
        //            TxtWitnesBy.Focus();
        //            valid = false;
        //        }
        //    }

        //    //date validation  
        //    DateTime dateTest = DateTime.Now;
        //    if (TxtDateOfTest.Text != "")
        //    {
        //        dateTest = Convert.ToDateTime(TxtDateOfTest.Text);
        //    }
        //    else
        //    {
        //        dispalyMsg = "Date Of Testing can not be blank.";
        //        TxtDateOfTest.Focus();
        //        valid = false;
        //    }

        //    if (dateTest > System.DateTime.Now)
        //    {
        //        dispalyMsg = "Date Of Testing must be less than or equal to Current Date.";
        //        TxtDateOfTest.Focus();
        //        valid = false;
        //    }
        //    string TestedDate = TxtDateOfTest.Text;
        //    if (TestedDate == "1/1/0001")
        //    {
        //        dispalyMsg = "Enter valid Date of Testing.";
        //        TxtDateOfTest.Focus();
        //        valid = false;
        //    }

        //    if (TxtDesc.Text == "")
        //    {
        //        dispalyMsg = "Please Enter Description";
        //        TxtDesc.Focus();
        //        valid = false;
        //    }
        //    else if (TxtSupplierName.Text == "")
        //    {
        //        dispalyMsg = "Please Enter Supplier Name";
        //        TxtSupplierName.Focus();
        //        valid = false;
        //    }

        //    //validate DA data
        //    if (TabDA.Visible == true)
        //    {
        //        if (txtrows.Text == "" || txtrows.Text == "0")
        //        {
        //            dispalyMsg = "Rows can not be zero or blank.";
        //            TxtWitnesBy.Focus();
        //            valid = false;

        //        }

        //        if (valid == true)
        //        {
        //            for (int i = 0; i < grdDA.Rows.Count; i++)
        //            {
        //                TextBox TxtIdMark = (TextBox)grdDA.Rows[i].Cells[1].FindControl("txtIdMark");
        //                Button TxtLength = (Button)grdDA.Rows[i].Cells[2].FindControl("TxtLength");
        //                Button TxtWidth = (Button)grdDA.Rows[i].Cells[3].FindControl("TxtWidth");
        //                Button TxtHeight = (Button)grdDA.Rows[i].Cells[4].FindControl("TxtHeight");

        //                if (TxtIdMark.Text == "")
        //                {
        //                    dispalyMsg = "Enter Id Mark for row no " + (i + 1) + ".";
        //                    TxtIdMark.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 0;
        //                    break;
        //                }
        //                else if (TxtLength.Text == "")
        //                {
        //                    dispalyMsg = "Enter Length for row number " + (i + 1) + ".";
        //                    TxtIdMark.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 0;
        //                    break;
        //                }
        //                else if (TxtWidth.Text == "")
        //                {
        //                    dispalyMsg = "Enter Width for row number " + (i + 1) + ".";
        //                    TxtIdMark.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 0;
        //                    break;
        //                }
        //                else if (TxtHeight.Text == "")
        //                {
        //                    dispalyMsg = "Enter Height for row number " + (i + 1) + ".";
        //                    TxtIdMark.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 0;
        //                    break;
        //                }
        //            }

        //        }
        //    }

        //    //validate WA data
        //    if (TabWA.Visible == true)
        //    {
        //        if (valid == true)
        //        {
        //            for (int i = 0; i < GrdViewWA.Rows.Count; i++)
        //            {
        //                TextBox lblWAIdMark = (TextBox)GrdViewWA.Rows[i].Cells[1].FindControl("lblWAIdMark");
        //                TextBox txtDryWt = (TextBox)GrdViewWA.Rows[i].Cells[2].FindControl("TxtDryWt");
        //                TextBox txtWetWt = (TextBox)GrdViewWA.Rows[i].Cells[3].FindControl("TxtWetWt");

        //                if (txtDryWt.Text != "" && txtWetWt.Text != "")
        //                {
        //                    decimal dryWet = Convert.ToDecimal(txtDryWt.Text);
        //                    decimal wetWt = Convert.ToDecimal(txtWetWt.Text);

        //                    if (dryWet > wetWt)
        //                    {
        //                        dispalyMsg = "Dry wt. must be less than Wet Wt.";
        //                        txtWetWt.Focus();
        //                        valid = false;
        //                        TabContainerBrickReport.ActiveTabIndex = 1;
        //                        break;
        //                    }
        //                }

        //                if (lblWAIdMark.Text == "")
        //                {
        //                    dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
        //                    lblWAIdMark.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 1;
        //                    break;
        //                }
        //                else if (txtDryWt.Text == "")
        //                {
        //                    dispalyMsg = "Enter Dry Wt. for row number " + (i + 1) + ".";
        //                    txtDryWt.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 1;
        //                    break;
        //                }
        //                else if (txtWetWt.Text == "")
        //                {
        //                    dispalyMsg = "Enter Wet Wt. for row number " + (i + 1) + ".";
        //                    txtWetWt.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 1;
        //                    break;
        //                }
        //            }

        //        }
        //    }

        //    //validate CS data
        //    if (TabCS.Visible == true)
        //    {

        //        if (valid == true)
        //        {
        //            for (int i = 0; i < GrdViewCS.Rows.Count; i++)
        //            {
        //                TextBox lblCSIdMark = (TextBox)GrdViewCS.Rows[i].Cells[1].FindControl("lblCSIdMark");
        //                TextBox txtlength = (TextBox)GrdViewCS.Rows[i].Cells[2].FindControl("TxtCSLength");
        //                TextBox txtwidth = (TextBox)GrdViewCS.Rows[i].Cells[3].FindControl("TxtCSWidth");
        //                TextBox txtLoad = (TextBox)GrdViewCS.Rows[i].Cells[4].FindControl("TxtLoad");
        //                if (lblCSIdMark.Text == "")
        //                {
        //                    dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
        //                    lblCSIdMark.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 2;
        //                    break;
        //                }
        //                else if (txtlength.Text == "")
        //                {
        //                    dispalyMsg = "Enter Length for row number " + (i + 1) + ".";
        //                    txtlength.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 2;
        //                    break;
        //                }
        //                else if (txtwidth.Text == "")
        //                {
        //                    dispalyMsg = "Enter Width for row number " + (i + 1) + ".";
        //                    txtwidth.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 2;
        //                    break;
        //                }
        //                else if (txtLoad.Text == "")
        //                {
        //                    dispalyMsg = "Enter Load for row number " + (i + 1) + ".";
        //                    txtLoad.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 2;
        //                    break;
        //                }
        //            }

        //        }
        //    }

        //    //validate ET data
        //    if (TabET.Visible == true)
        //    {

        //        if (valid == true)
        //        {
        //            for (int i = 0; i < GrdViewET.Rows.Count; i++)
        //            {
        //                TextBox lblETIdMark = (TextBox)GrdViewET.Rows[i].Cells[1].FindControl("lblETIdMark");
        //                if (lblETIdMark.Text == "")
        //                {
        //                    dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
        //                    lblETIdMark.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 3;
        //                    break;
        //                }
        //            }
        //        }

        //    }

        //    //validate Density data
        //    if (TabDensity.Visible == true)
        //    {
        //        if (valid == true)
        //        {
        //            for (int i = 0; i < GrdViewDensity.Rows.Count; i++)
        //            {
        //                TextBox txtIdMark = (TextBox)GrdViewDensity.Rows[i].Cells[1].FindControl("lblDIdMark");
        //                TextBox txtlength = (TextBox)GrdViewDensity.Rows[i].Cells[2].FindControl("TxtDLength");
        //                TextBox txtwidth = (TextBox)GrdViewDensity.Rows[i].Cells[3].FindControl("TxtDWidth");
        //                TextBox txtThickness = (TextBox)GrdViewDensity.Rows[i].Cells[4].FindControl("TxtThickness");
        //                TextBox txtOvenDryWt = (TextBox)GrdViewDensity.Rows[i].Cells[5].FindControl("TxtOvenDryWt");
        //                if (txtIdMark.Text == "")
        //                {
        //                    dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
        //                    txtIdMark.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 4;
        //                    break;
        //                }
        //                else if (txtlength.Text == "")
        //                {
        //                    dispalyMsg = "Enter Length for row number " + (i + 1) + ".";
        //                    txtlength.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 4;
        //                    break;
        //                }
        //                else if (txtwidth.Text == "")
        //                {
        //                    dispalyMsg = "Enter Width for row number " + (i + 1) + ".";
        //                    txtwidth.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 4;
        //                    break;
        //                }
        //                else if (txtThickness.Text == "")
        //                {
        //                    dispalyMsg = "Enter Thickness for row number " + (i + 1) + ".";
        //                    txtThickness.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 4;
        //                    break;
        //                }
        //                else if (txtOvenDryWt.Text == "")
        //                {
        //                    dispalyMsg = "Enter Oven Dry Wt. for row number " + (i + 1) + ".";
        //                    txtOvenDryWt.Focus();
        //                    valid = false;
        //                    TabContainerBrickReport.ActiveTabIndex = 4;
        //                    break;
        //                }
        //            }

        //        }
        //    }


        //    if (valid == false)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
        //    }
        //    return valid;
        //}

        //public void volumeCal(GridViewRow currentRow)
        //{
        //    try
        //    {
        //        TextBox txtDLength = (TextBox)currentRow.FindControl("TxtDLength");
        //        TextBox txtDWidth = (TextBox)currentRow.FindControl("TxtDWidth");
        //        TextBox txtThickness = (TextBox)currentRow.FindControl("TxtThickness");
        //        int dLength = 0, dWidth = 0, thickness = 0;
        //        if (txtDLength.Text != "" && txtDWidth.Text != "" && txtThickness.Text != "")
        //        {
        //            dLength = Convert.ToInt32(txtDLength.Text);
        //            dWidth = Convert.ToInt32(txtDWidth.Text);
        //            thickness = Convert.ToInt32(txtThickness.Text);
        //            int volume = (dLength * dWidth * thickness);

        //            TextBox txtVolume = (TextBox)currentRow.FindControl("TxtVolume");
        //            txtVolume.Text = volume.ToString();

        //            //cal density  when volume changes
        //            densityCal(currentRow);
        //        }

        //    }
        //    catch
        //    { }

        //}

        //public void densityCal(GridViewRow currentRow)
        //{
        //    try
        //    {
        //        int qty = Convert.ToInt32(TxtDensityQty.Text);
        //        TextBox txtVolume = (TextBox)currentRow.FindControl("TxtVolume");
        //        TextBox txtOvenDryWt = (TextBox)currentRow.FindControl("TxtOvenDryWt");

        //        if (txtVolume.Text != "" && txtOvenDryWt.Text != "")
        //        {
        //            decimal ovendryWt = Convert.ToDecimal(txtOvenDryWt.Text);
        //            int volume = Convert.ToInt32(txtVolume.Text);
        //            TextBox txtDensity = (TextBox)currentRow.FindControl("TxtDensity");
        //            long density = 0;
        //            if (volume != 0)
        //            {
        //                decimal tempDensity = ((1000000000 * ovendryWt) / volume);
        //                density = Convert.ToInt64(Math.Ceiling(tempDensity));
        //                txtDensity.Text = density.ToString();
        //            }
        //            else
        //            {
        //                txtDensity.Text = "0";
        //            }

        //            long total = 0, avgDensity = 0;
        //            foreach (GridViewRow row in GrdViewDensity.Rows)
        //            {
        //                var rowDensity = row.FindControl("TxtDensity") as TextBox;
        //                total += Convert.ToInt64(rowDensity.Text);

        //            }
        //            string emptyRow = ((TextBox)GrdViewDensity.Rows[qty - 1].Cells[5].FindControl("TxtOvenDryWt")).Text;
        //            if (qty >= 5)
        //            {
        //                decimal tempAvg = total / qty;
        //                avgDensity = Convert.ToInt32(Math.Ceiling(tempAvg));
        //                if (emptyRow != "")
        //                {
        //                    TextBox TxtAvgDensity = (TextBox)GrdViewDensity.Rows[qty / 2].Cells[8].FindControl("TxtAvgDensity");
        //                    TxtAvgDensity.Text = avgDensity.ToString();
        //                }
        //            }
        //            else
        //            {
        //                if (qty < 5)
        //                {
        //                    if (emptyRow != "")
        //                    {
        //                        TextBox TxtAvg = (TextBox)GrdViewDensity.Rows[qty / 2].Cells[8].FindControl("TxtAvgDensity");
        //                        TxtAvg.Text = "***";
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }
        //}

        //protected void TxtDLength_TextChanged(object sender, EventArgs e)
        //{
        //    Session["exitFlag"] = "true";
        //    GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //    volumeCal(currentRow);
        //}
        //protected void TxtDWidth_TextChanged(object sender, EventArgs e)
        //{
        //    Session["exitFlag"] = "true";
        //    GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //    volumeCal(currentRow);
        //}
        //protected void Txtthickness_TextChanged(object sender, EventArgs e)
        //{
        //    Session["exitFlag"] = "true";
        //    GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //    volumeCal(currentRow);
        //}
        //protected void TxtOvenDryWt_TextChanged(object sender, EventArgs e)
        //{
        //    Session["exitFlag"] = "true";
        //    GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        //    densityCal(currentRow);
        //}
       
        //protected void BtnExit_Click(object sender, EventArgs e)
        //{
        //    if (Session["exitFlag"].ToString() == "true")
        //    {
        //        var result = System.Windows.Forms.MessageBox.Show("Do you really want to Exit?", "Warning", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning, System.Windows.Forms.MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);

        //        if (result == System.Windows.Forms.DialogResult.Yes)
        //        {
        //            Response.Redirect("Home.aspx");
        //        }
        //        else
        //        {
        //            return;
        //        }

        //    }
        //    bool flag = false;
        //    //DA
        //    if (TabDA.Visible == true)
        //    {

        //        foreach (GridViewRow row in grdDA.Rows)
        //        {
        //            string length = "", width = "", height = "", idmark = "";
        //            idmark = (row.FindControl("txtIdMark") as TextBox).Text;
        //            length = (row.FindControl("TxtLength") as Button).Text;
        //            width = (row.FindControl("TxtWidth") as Button).Text;
        //            height = (row.FindControl("TxtHeight") as Button).Text;

        //            if (length == "" || width == "" || height == "" || idmark == "")
        //            {
        //                flag = true;
        //                break;
        //            }
        //        }
        //    }

        //    //CS
        //    if (TabCS.Visible == true)
        //    {
        //        foreach (GridViewRow row in GrdViewCS.Rows)
        //        {
        //            string length = "", width = "", load = "", idmark = "";
        //            idmark = (row.FindControl("lblCSIdMark") as TextBox).Text;
        //            length = (row.FindControl("TxtCSLength") as TextBox).Text;
        //            width = (row.FindControl("TxtCSWidth") as TextBox).Text;
        //            load = (row.FindControl("TxtLoad") as TextBox).Text;

        //            if (length == "" || width == "" || load == "" || idmark == "")
        //            {
        //                flag = true;
        //                break;
        //            }
        //        }
        //    }
        //    //WA
        //    if (TabWA.Visible == true)
        //    {
        //        foreach (GridViewRow row in GrdViewWA.Rows)
        //        {
        //            string TxtDry = "", TxtWet = "", idmark = "";
        //            idmark = (row.FindControl("lblWAIdMark") as TextBox).Text;
        //            TxtDry = (row.FindControl("TxtDryWt") as TextBox).Text;
        //            TxtWet = (row.FindControl("TxtWetWt") as TextBox).Text;


        //            if (TxtDry == "" || TxtWet == "" || idmark == "")
        //            {
        //                flag = true;
        //                break;
        //            }
        //        }
        //    }
        //    //ET
        //    if (TabET.Visible == true)
        //    {
        //        foreach (GridViewRow row in GrdViewET.Rows)
        //        {
        //            string idmark = "";
        //            idmark = (row.FindControl("lblETIdMark") as TextBox).Text;
        //            if (idmark == "")
        //            {
        //                flag = true;
        //                break;
        //            }
        //        }
        //    }
        //    //Density
        //    if (TabDensity.Visible == true)
        //    {
        //        foreach (GridViewRow row in GrdViewDensity.Rows)
        //        {
        //            string Txtlen = "", TxtWigth = "", thickness = "", ovenDrywt = "", idmark = "";
        //            idmark = (row.FindControl("lblDIdMark") as TextBox).Text;
        //            Txtlen = (row.FindControl("TxtDLength") as TextBox).Text;
        //            TxtWigth = (row.FindControl("TxtDWidth") as TextBox).Text;
        //            thickness = (row.FindControl("TxtThickness") as TextBox).Text;
        //            ovenDrywt = (row.FindControl("TxtOvenDryWt") as TextBox).Text;

        //            if (idmark == "" || Txtlen == "" || TxtWigth == "" || thickness == "" || ovenDrywt == "")
        //            {
        //                flag = true;
        //                break;
        //            }
        //        }
        //    }
        //    if (flag == true)
        //    {
        //        Session["exitFlag"] = "false";
        //        var exitResult = System.Windows.Forms.MessageBox.Show("Do you really want to Exit?", "Warning", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning, System.Windows.Forms.MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
        //        if (exitResult == System.Windows.Forms.DialogResult.Yes)
        //        {
        //            Response.Redirect("Home.aspx");
        //        }
        //        else
        //        {
        //            // Response.Redirect("BrickReport.aspx");
        //            return;
        //        }
        //    }
        //    if (Session["exitFlag"].ToString() == "false" && flag == false)
        //    {
        //        Response.Redirect("Home.aspx");
        //    }

        //}
        
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            //Response.Redirect("ReportStatus.aspx");
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }
               
       
    }

}