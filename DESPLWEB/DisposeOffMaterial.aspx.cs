using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class DisposeOffMaterial : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Disposal Off Material";
                txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                txtSpecificDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                optPending.Checked = true;
            }
        }

        private void LoadDisposalDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("RecordType", typeof(string)));
            dt.Columns.Add(new DataColumn("RecordNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ReferenceNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ReceivedDate", typeof(string)));
            dt.Columns.Add(new DataColumn("EnquiryNo", typeof(string)));
            dt.Columns.Add(new DataColumn("DisposedOff", typeof(string)));
            dt.Columns.Add(new DataColumn("DisposedDate", typeof(string)));
            dt.Columns.Add(new DataColumn("UnderTesting", typeof(string)));
            dt.Columns.Add(new DataColumn("UnderDisposal", typeof(string)));
            dt.Columns.Add(new DataColumn("3DaysStr", typeof(string)));
            dt.Columns.Add(new DataColumn("7DaysStr", typeof(string)));
            dt.Columns.Add(new DataColumn("21DaysStr", typeof(string)));
            dt.Columns.Add(new DataColumn("MaterialRecdWithQty", typeof(string)));
            dt.Columns.Add(new DataColumn("Grade", typeof(string)));
            //dt.Columns.Add(new DataColumn("7DaysStrMF", typeof(string)));
            dt.Columns.Add(new DataColumn("StrReqOPC", typeof(string)));
            dt.Columns.Add(new DataColumn("StrReqPPC", typeof(string)));
            dt.Columns.Add(new DataColumn("TestingDate", typeof(string)));
            dt.Columns.Add(new DataColumn("UnderRetention", typeof(string)));
            dt.Columns.Add(new DataColumn("SentToDisposalBin", typeof(string)));
            dt.Columns.Add(new DataColumn("SentToDisposalBinDate", typeof(string)));
            dt.Columns.Add(new DataColumn("txtRemark", typeof(string)));
            dt.Columns.Add(new DataColumn("chkUnderDisposalST", typeof(string)));
            dt.Columns.Add(new DataColumn("chkDisposedOff", typeof(string)));

            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            
            strDate = txtToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            
            if (ddlInwardType.SelectedValue == "AGGT")
            {
                var aggtDisp = dc.DisposeMaterial_View("AGGT", FromDate, ToDate, optCompleted.Checked);
                foreach (var aggt in aggtDisp)
                {
                    dr = dt.NewRow();
                    dr["SrNo"] = dt.Rows.Count + 1;
                    dr["RecordType"] = aggt.AGGTINWD_RecordType_var;
                    dr["RecordNo"] = aggt.AGGTINWD_RecordNo_int;
                    dr["ReferenceNo"] = aggt.AGGTINWD_ReferenceNo_var;
                    dr["ReceivedDate"] = Convert.ToDateTime(aggt.AGGTINWD_ReceivedDate_dt).ToString("dd/MM/yyyy");
                    dr["EnquiryNo"] = aggt.INWD_ENQ_Id.ToString();
                    if (aggt.AGGTINWD_DisposedOffMaterial_bit == true)
                        dr["DisposedOff"] = "Yes";
                    else
                        dr["DisposedOff"] = "No";
                    if (aggt.AGGTINWD_DisposedDate_dt != null)
                        dr["DisposedDate"] = Convert.ToDateTime(aggt.AGGTINWD_DisposedDate_dt).ToString("dd/MM/yyyy");

                    if (aggt.AGGTINWD_Status_tint <= 1)
                        dr["UnderTesting"] = "Yes";
                    else
                        dr["UnderTesting"] = "No";
                    if (aggt.AGGTINWD_CheckedDate_dt != null &&
                        Convert.ToDateTime(aggt.AGGTINWD_CheckedDate_dt).AddDays(7) <= DateTime.Now)
                        dr["UnderDisposal"] = "Yes";
                    else
                        dr["UnderDisposal"] = "No";

                    dt.Rows.Add(dr);
                }
                grdDisposal.DataSource = dt;
                grdDisposal.DataBind();
            }
            else if (ddlInwardType.SelectedValue == "CEMT")
            { 
                var cemtDisp = dc.DisposeMaterial_View("CEMT", FromDate, ToDate, optCompleted.Checked);
                foreach (var cemt in cemtDisp)
                {
                    dr = dt.NewRow();
                    dr["SrNo"] = dt.Rows.Count + 1;
                    dr["RecordType"] = cemt.CEMTINWD_RecordType_var;
                    dr["RecordNo"] = cemt.CEMTINWD_RecordNo_int;
                    dr["ReferenceNo"] = cemt.CEMTINWD_ReferenceNo_var;
                    dr["ReceivedDate"] = Convert.ToDateTime(cemt.CEMTINWD_ReceivedDate_dt).ToString("dd/MM/yyyy");
                    dr["EnquiryNo"] = cemt.INWD_ENQ_Id.ToString();
                    if (cemt.CEMTINWD_DisposedOffMaterial_bit == true)
                        dr["DisposedOff"] = "Yes";
                    else
                        dr["DisposedOff"] = "No";
                    if (cemt.CEMTINWD_DisposedDate_dt != null)
                        dr["DisposedDate"] = Convert.ToDateTime(cemt.CEMTINWD_DisposedDate_dt).ToString("dd/MM/yyyy");
                    var CompStr3 = dc.OtherCubeTestView(cemt.CEMTINWD_ReferenceNo_var, "CEMT", 3, 0, "CEMT", false, true);
                    foreach (var cms in CompStr3)
                    {
                        dr["3DaysStr"] = cms.Avg_var;
                    }
                    var CompStr7 = dc.OtherCubeTestView(cemt.CEMTINWD_ReferenceNo_var, "CEMT", 7, 0, "CEMT", false, true);
                    foreach (var cms in CompStr7)
                    {
                        dr["7DaysStr"] = cms.Avg_var;
                    }
                    var CompStr21 = dc.OtherCubeTestView(cemt.CEMTINWD_ReferenceNo_var, "CEMT", 21, 0, "CEMT", false, true);
                    foreach (var cms in CompStr21)
                    {
                        dr["21DaysStr"] = cms.Avg_var;
                    }
                    dt.Rows.Add(dr);
                }
                grdDisposal.DataSource = dt;
                grdDisposal.DataBind();
            }
            else if (ddlInwardType.SelectedValue == "MF")
            {
                var mfDisp = dc.DisposeMaterial_View("MF", FromDate, ToDate, optCompleted.Checked);
                foreach (var mf in mfDisp)
                {
                    dr = dt.NewRow();
                    dr["SrNo"] = dt.Rows.Count + 1;
                    dr["RecordType"] = mf.MFINWD_RecordType_var;
                    dr["RecordNo"] = mf.MFINWD_RecordNo_int;
                    dr["ReferenceNo"] = mf.MFINWD_ReferenceNo_var;
                    dr["ReceivedDate"] = Convert.ToDateTime(mf.MFINWD_ReceivedDate_dt).ToString("dd/MM/yyyy");
                    dr["EnquiryNo"] = mf.INWD_ENQ_Id.ToString();
                    if (mf.MFINWD_DisposedOffMaterial_bit == true)
                        dr["DisposedOff"] = "Yes";
                    else
                        dr["DisposedOff"] = "No";
                    if (mf.MFINWD_DisposedDate_dt != null)
                        dr["DisposedDate"] = Convert.ToDateTime(mf.MFINWD_DisposedDate_dt).ToString("dd/MM/yyyy");

                    string strGrade = mf.MFINWD_Grade_var;
                    if (strGrade != "" && strGrade != null)
                    {
                        dr["Grade"] = strGrade;
                        if (strGrade == "M 20" )
                        {
                            dr["StrReqOPC"] = "18.62";
                            dr["StrReqPPC"] = "17.29";
                        }
                        else if (strGrade == "M 25" )
                        {
                            dr["StrReqOPC"] = "22.12";
                            dr["StrReqPPC"] = "20.54";
                        }
                        else if (strGrade == "M 30" )
                        {
                            dr["StrReqOPC"] = "26.77";
                            dr["StrReqPPC"] = "24.86";
                        }
                        else if (strGrade == "M 35" )
                        {
                            dr["StrReqOPC"] = "30.27";
                            dr["StrReqPPC"] = "28.11";
                        }
                        else if (strGrade == "M 40" )
                        {
                            dr["StrReqOPC"] = "33.77";
                            dr["StrReqPPC"] = "31.36";
                        }
                        else if (strGrade == "M 45" )
                        {
                            dr["StrReqOPC"] = "37.27";
                            dr["StrReqPPC"] = "34.61";
                        }
                        else if (strGrade == "M 50" )
                        {
                            dr["StrReqOPC"] = "40.77";
                            dr["StrReqPPC"] = "37.86";
                        }
                    }
                    else
                    {
                        dr["Grade"] = "NA";
                        dr["StrReqOPC"] = "-";
                        dr["StrReqPPC"] = "-";
                    }
                    string strMaterialQty = "";
                    var material = dc.MaterialDetail_View(0, mf.MFINWD_ReferenceNo_var, 0, "", null, null, "");
                    foreach (var mat in material)
                    {
                        strMaterialQty = strMaterialQty + mat.Material_List +  " - " + mat.MaterialDetail_Quantity + " , ";
                    }
                    dr["MaterialRecdWithQty"] = strMaterialQty;
                    dt.Rows.Add(dr);
                }
                grdDisposal.DataSource = dt;
                grdDisposal.DataBind();
            }
            else if (ddlInwardType.SelectedValue == "ST")
            {
                var stDisp = dc.DisposeMaterial_View("ST", FromDate, ToDate, optCompleted.Checked);
                foreach (var st in stDisp)
                {
                    dr = dt.NewRow();
                    dr["SrNo"] = dt.Rows.Count + 1;
                    dr["RecordType"] = st.STINWD_RecordType_var;
                    dr["RecordNo"] = st.STINWD_RecordNo_int;
                    dr["ReferenceNo"] = st.STINWD_ReferenceNo_var;
                    dr["ReceivedDate"] = Convert.ToDateTime(st.STINWD_ReceivedDate_dt).ToString("dd/MM/yyyy");
                    dr["EnquiryNo"] = st.INWD_ENQ_Id.ToString();
                    if (st.STINWD_DisposedOffMaterial_bit == true)
                        dr["DisposedOff"] = "Yes";
                    else
                        dr["DisposedOff"] = "No";
                    if (st.STINWD_DisposedDate_dt != null)
                        dr["DisposedDate"] = Convert.ToDateTime(st.STINWD_DisposedDate_dt).ToString("dd/MM/yyyy");

                    //Test_Status" = 1  THEN
                    if (st.STINWD_TestedDate_dt != null)
                        dr["TestingDate"] = Convert.ToDateTime(st.STINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                    if (st.STINWD_TestedDate_dt != null &&
                        Convert.ToDateTime(st.STINWD_TestedDate_dt).AddDays(7) <= DateTime.Now)
                        dr["UnderRetention"] = "Yes";
                    else
                        dr["UnderRetention"] = "No";
                    if (st.STINWD_UnderDisposal_bit == true)
                    {
                        dr["SentToDisposalBin"] = "Yes";
                        dr["SentToDisposalBinDate"] = Convert.ToDateTime(st.STINWD_SentToDisposalBinDate_dt).ToString("dd/MM/yyyy");
                        dr["chkUnderDisposalST"] = "True";
                    }
                    else
                    {
                        dr["SentToDisposalBin"] = "No";
                        dr["chkUnderDisposalST"] = "True";
                    }

                    dr["txtRemark"] = st.STINWD_DisposalRemark_var;
                    dt.Rows.Add(dr);
                }
                grdDisposal.DataSource = dt;
                grdDisposal.DataBind();
            }
        }

        protected void ddlInwardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 9; i <= grdDisposal.Columns.Count - 1; i++)
            {
                grdDisposal.Columns[i].Visible = false;
            }
            grdDisposal.DataSource = null;
            grdDisposal.DataBind();
            if (ddlInwardType.SelectedValue == "AGGT")
            {
                for (int i = 9; i <= 10; i++)
                {
                    grdDisposal.Columns[i].Visible = true;
                }
            }
            else if (ddlInwardType.SelectedValue == "CEMT")
            {
                for (int i = 11; i <= 13; i++)
                {
                    grdDisposal.Columns[i].Visible = true;
                }
            }
            else if (ddlInwardType.SelectedValue == "MF")
            {
                for (int i = 14; i <= 17; i++)
                {
                    grdDisposal.Columns[i].Visible = true;
                }
            }
            else if (ddlInwardType.SelectedValue == "ST")
            {
                for (int i = 18; i <= 23; i++)
                {
                    grdDisposal.Columns[i].Visible = true;
                }
            }

        }

        protected void optPending_CheckedChanged(object sender, EventArgs e)
        {
            grdDisposal.DataSource = null;
            grdDisposal.DataBind();
            grdDisposal.Columns[0].Visible = true;
            LoadDisposalDetails(); 
        }

        protected void optCompleted_CheckedChanged(object sender, EventArgs e)
        {
            grdDisposal.DataSource = null;
            grdDisposal.DataBind();
            grdDisposal.Columns[0].Visible = false;
            LoadDisposalDetails();
        }

        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            LoadDisposalDetails();
        }

        protected void chkSpecificDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSpecificDate.Checked == true)
                txtSpecificDate.Enabled = true;
            else
                txtSpecificDate.Enabled = false;
        }

        protected void chkSelect_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)chk.NamingContainer;

            if (chk.Checked == true)
            {
                grdDisposal.Rows[gvr.RowIndex].Cells[8].Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                grdDisposal.Rows[gvr.RowIndex].Cells[8].Text = "";
            }
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {

            for (int i = 0; i < grdDisposal.Rows.Count - 1; i++)
            {
                CheckBox chkSelect = (CheckBox)grdDisposal.Rows[i].FindControl("chkSelect");
                if (chkSelectAll.Checked == true)
                {
                    chkSelect.Checked = true;
                    grdDisposal.Rows[i].Cells[8].Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                    chkSelect.Checked = false;
                    txtSpecificDate.Enabled = false;
                    grdDisposal.Rows[i].Cells[8].Text = "";
                }
            }
            
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            bool selFlag = false;
            DateTime? disposalDate = null;
            DateTime? SentToDisposalBinDate = null;

            for (int i = 0; i < grdDisposal.Rows.Count-1; i++)
            {
                disposalDate = null;
                CheckBox chkSelect = (CheckBox)grdDisposal.Rows[i].FindControl("chkSelect");                
                if (chkSelect.Checked == true && 
                    grdDisposal.Rows[i].Cells[8].Text != "&nbsp;" && grdDisposal.Rows[i].Cells[8].Text != "")
                {
                    disposalDate = DateTime.ParseExact(grdDisposal.Rows[i].Cells[8].Text, "dd/MM/yyyy", null);
                }
                if (ddlInwardType.SelectedValue == "ST" )
                {
                    TextBox txtRemark = (TextBox)grdDisposal.Rows[i].FindControl("txtRemark");
                    CheckBox chkUnderDisposalST = (CheckBox)grdDisposal.Rows[i].FindControl("chkUnderDisposalST");                    
                    SentToDisposalBinDate = null;
                    bool UnderDisposalFlag = false;
                    if (chkUnderDisposalST.Checked == true || grdDisposal.Rows[i].Cells[20].Text == "Yes")
                    {
                        UnderDisposalFlag = true;
                        if (grdDisposal.Rows[i].Cells[21].Text != "" && grdDisposal.Rows[i].Cells[21].Text != "&nbsp;")
                        {
                            SentToDisposalBinDate = DateTime.ParseExact(grdDisposal.Rows[i].Cells[21].Text, "dd/MM/yyyy", null);
                        }
                        else
                        {
                            DateTime testingDate = DateTime.ParseExact(grdDisposal.Rows[i].Cells[18].Text, "dd/MM/yyyy", null);
                            SentToDisposalBinDate = testingDate.AddDays(7);
                        }
                    }
                    if (chkSelect.Checked == true || chkUnderDisposalST.Checked == true)
                    {
                        selFlag = true;
                        dc.DisposeMaterial_Update(ddlInwardType.SelectedValue, grdDisposal.Rows[i].Cells[4].Text, chkSelect.Checked, disposalDate, txtRemark.Text, UnderDisposalFlag, SentToDisposalBinDate);
                    }
                }
                else if (chkSelect.Checked == true)
                {
                    selFlag = true;
                    dc.DisposeMaterial_Update(ddlInwardType.SelectedValue, grdDisposal.Rows[i].Cells[4].Text, chkSelect.Checked, disposalDate, "", false, null);
                }
            }
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = true;
            if (selFlag == true)
            {
                lblMsg.Text = "Record saved successfully.";
                lblMsg.ForeColor = System.Drawing.Color.Green;
                LoadDisposalDetails();
            }
            else
            {   
                lblMsg.Text = "Please select at least one record.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            if (ddlInwardType.SelectedValue == "AGGT")
            {
                rpt.AggregateDisposal_PDFReport(optCompleted.Checked, txtFromDate.Text , txtToDate.Text);
            }
            else if (ddlInwardType.SelectedValue == "CEMT")
            {
                rpt.CementDisposal_PDFReport(optCompleted.Checked, txtFromDate.Text, txtToDate.Text);
            }
            else if (ddlInwardType.SelectedValue == "MF")
            {
                rpt.MixDesignDisposal_PDFReport(optCompleted.Checked, txtFromDate.Text, txtToDate.Text);
            }
            else if (ddlInwardType.SelectedValue == "ST")
            {
                rpt.SteelDisposal_PDFReport(optCompleted.Checked, txtFromDate.Text, txtToDate.Text);
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
    }
}