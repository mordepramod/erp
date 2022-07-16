using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class CouponStatus : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {   
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Coupon Status";
                txtFromDate.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                //LoadAllClientList();
                lnkCancelCoupons.Visible = false;
                lblComment.Visible = false;
                txtComment.Visible = false;
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_SuperAdmin_right_bit == true)
                    {
                        lnkCancelCoupons.Visible = true;
                        lblComment.Visible = true;
                        txtComment.Visible = true;
                    }
                }
                rdoCT.Checked = true;
            }
        }

        //protected void LoadAllClientList()
        //{
        //    ddlClient.DataTextField = "CL_Name_var";
        //    ddlClient.DataValueField = "CL_Id";
        //    var cl = dc.Client_View(0, 0, "");
        //    ddlClient.DataSource = cl;
        //    ddlClient.DataBind();
        //    ddlClient.Items.Insert(0, new ListItem("---All---", "0"));
        //}

        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdClient.Rows.Count > 0 && grdClient.Visible == true)
            {
                string reportStr = "";
                reportStr = RptCouponStatus();
                PrintHTMLReport rpt = new PrintHTMLReport();
                rpt.DownloadHtmlReport("Coupon", reportStr);

                //string Subheader = "";                
                //Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text + "|" + lblToDate.Text + "|" + txtToDate.Text ;
                //if(chkClientSpecific.Checked ==true )
                //    Subheader += "|" + "Client" + "|" + txt_Client.Text;
                //else
                //    Subheader += "|" + "" + "|" + "";
                
                //string grddata = "";
                //string grdColumn = "";
                //string grdview = "";
                //for (int j = 0; j < grdClient.Columns.Count; j++)
                //{
                //    if (grdClient.Columns[j].Visible == true)
                //    {
                //        grdColumn += grdClient.Columns[j].HeaderText + "|";
                //    }
                //}
                //for (int i = 0; i < grdClient.Rows.Count; i++)
                //{
                //    grddata += "$";
                //    for (int c = 0; c < grdClient.Rows[i].Cells.Count; c++)
                //    {
                //        grddata += grdClient.Rows[i].Cells[c].Text + "~";
                //    }
                //}
                //grdview = grdColumn + grddata;
                //rpt.RptHTMLgrid("Client Coupon Status", Subheader, grdview);
            }
        }

        protected void lnkgrdPrint_Click(object sender, EventArgs e)
        {
            if (grdCoupon.Rows.Count > 0 && grdCoupon.Visible == true)
            {
                string Subheader = "";
                //string Subheader1 = "Coupon List";
                string[] strDetail = lblDetailHeading.Text.Split(':'); 

                //Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text + "|" + lblToDate.Text + "|" + txtToDate.Text + "|" + "Client Name" + "|" + lblDetailHeading.Text.Replace("Coupon List [ Client Name : ", "").Replace("]","")  + "|";
                Subheader = "" + "|" + lblFromDate.Text + "|" + txtFromDate.Text + "|" + lblToDate.Text + "|" + txtToDate.Text + "|" + "Client Name" + "|" + strDetail[1].Replace("Site Name :", "") + "|" + "Site Name" + "|" + strDetail[2].Replace("Bill No. :","") + "|" + "Bill No." + "|" + strDetail[3].Replace("]","") + "|";
                PrintHTMLReport rpt = new PrintHTMLReport();
                string grddata = "";
                string grdColumn = "";
                string grdview = "";
                for (int j = 0; j < grdCoupon.Columns.Count; j++)
                {
                    if (grdCoupon.Columns[j].Visible == true)
                    {
                        grdColumn += grdCoupon.Columns[j].HeaderText + "|";
                    }
                }
                for (int i = 0; i < grdCoupon.Rows.Count; i++)
                {
                    grddata += "$";
                    for (int c = 0; c < grdCoupon.Rows[i].Cells.Count; c++)
                    {
                        grddata += grdCoupon.Rows[i].Cells[c].Text + "~";
                    }
                }
                grdview = grdColumn + grddata;
                rpt.RptHTMLgrid("Coupon List", Subheader, grdview);
                //NewWindows.PrintGridView(grdCoupon, lblDetailHeading.Text, "Coupon_Status");
            }
        }

        protected void lnkApply_Click(object sender, EventArgs e)
        {
            DataTable dtCoupon = (DataTable)Session["CouponTable"];
            DataTable dt = new DataTable();
            DataRow dr1 = null;

            if (dt != null)
            {
                dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("SiteName", typeof(string)));
                dt.Columns.Add(new DataColumn("CouponNo", typeof(string)));
                dt.Columns.Add(new DataColumn("Status", typeof(string)));
                dt.Columns.Add(new DataColumn("UsedOn", typeof(string)));
                dt.Columns.Add(new DataColumn("ExpiredOn", typeof(string)));

                int SrNo = 1;
                for (int i = 0; i < dtCoupon.Rows.Count;i++ )
                {
                    if (dtCoupon.Rows[i]["Status"].ToString() == ddlCouponStatus.SelectedItem.Text || ddlCouponStatus.SelectedItem.Text == "---All---")
                    {
                        dr1 = dt.NewRow();
                        dr1["SrNo"] = SrNo;
                        dr1["CouponNo"] = dtCoupon.Rows[i]["CouponNo"];
                        dr1["SiteName"] = dtCoupon.Rows[i]["SiteName"];
                        dr1["Status"] = dtCoupon.Rows[i]["Status"];
                        dr1["UsedOn"] = dtCoupon.Rows[i]["UsedOn"];
                        dr1["ExpiredOn"] = dtCoupon.Rows[i]["ExpiredOn"];
                        dt.Rows.Add(dr1);
                        SrNo++;
                    }
                }
                grdCoupon.DataSource = dt;
                grdCoupon.DataBind();
                for (int i = 0; i < grdCoupon.Rows.Count; i++)
                {
                    if (grdCoupon.Rows[i].Cells[4].Text != "Not Used")
                    {
                        CheckBox chkSelect = (CheckBox)grdCoupon.Rows[i].FindControl("chkSelect");
                        chkSelect.Enabled = false;
                    }
                }
            }
        }
        
        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            DisplayClientList();
        }
        protected void DisplayClientList()
        {
            pnlCouponList.Visible = false;
            lblDetailHeading.Visible = false;
            pnlCouponDetails.Visible = false;
            txtComment.Text = "";
            
            DataTable dt = new DataTable();
            DataRow dr1 = null;
            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            strDate = txtToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ClientId", typeof(string)));
            dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt.Columns.Add(new DataColumn("SiteName", typeof(string)));
            dt.Columns.Add(new DataColumn("BillNo", typeof(string)));
            dt.Columns.Add(new DataColumn("IssueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("Issued", typeof(string)));
            dt.Columns.Add(new DataColumn("Used", typeof(string)));
            dt.Columns.Add(new DataColumn("NotUsed", typeof(string)));
            dt.Columns.Add(new DataColumn("Expired", typeof(string)));
            int SrNo = 1;

            int ClientId = 0;
            if (chkClientSpecific.Checked == true)
            {
                if (lblClientId.Text != "")
                    ClientId = Convert.ToInt32(lblClientId.Text);
            }
            //var clientList = dc.Coupon_View_Status(Convert.ToInt32(ddlClient.SelectedValue), FromDate, ToDate);
            string rType = "";
            if (rdoCT.Checked)
                rType = "CT";
            else if (rdoST.Checked )
                rType = "ST";
            var clientList = dc.Coupon_View_Status(ClientId, FromDate, ToDate,rType);
            foreach (var client in clientList)
            {
                dr1 = dt.NewRow();
                dr1["SrNo"] = SrNo;
                dr1["ClientId"] = client.COUP_CL_Id;
                dr1["ClientName"] = client.CL_Name_var;
                dr1["SiteName"] = client.SITE_Name_var;
                dr1["BillNo"] = client.BILL_Id;
                if(client.BILL_Date_dt != null)
                    dr1["IssueDate"] = Convert.ToDateTime(client.BILL_Date_dt).ToString("dd/MM/yyyy"); 
                else
                    dr1["IssueDate"] = Convert.ToDateTime(client.COUP_Date_dt).ToString("dd/MM/yyyy"); 
                dr1["Issued"] = client.totalCount;
                dr1["Used"] = client.UsedCount;
                dr1["NotUsed"] = client.NotUsedCount;
                dr1["Expired"] = client.ExpiredCount;
                dt.Rows.Add(dr1);
                SrNo++;
            }
            grdClient.DataSource = dt;
            grdClient.DataBind();

        }
        protected void lnkViewReport(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int index = Convert.ToInt32(row.RowIndex);
            DataTable dt = new DataTable();
            DataRow dr1 = null;
            string[] strDate = txtFromDate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            strDate = txtToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

            string clientId = "", clientName = "", billId, siteName = "";
            clientId = grdClient.Rows[index].Cells[1].Text;
            clientName = grdClient.Rows[index].Cells[2].Text;
            siteName = grdClient.Rows[index].Cells[3].Text;
            billId = grdClient.Rows[index].Cells[4].Text;
            lblClientId.Text = clientId;
            lblDetailHeading.Text = "Coupon List [ Client Name : " + clientName + " Site Name : " + siteName + " Bill No. : " + billId + " ] ";
            pnlCouponList.Visible = true;
            lblDetailHeading.Visible = true;
            pnlCouponDetails.Visible = true;
            txtComment.Text = "";

            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("SiteName", typeof(string)));
            dt.Columns.Add(new DataColumn("CouponNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("UsedOn", typeof(string)));
            dt.Columns.Add(new DataColumn("ExpiredOn", typeof(string)));

            int SrNo = 1;

            if (rdoCT.Checked)
            {
                var couponList = dc.Coupon_View(billId, 0, 0, 0, 0, 0, null);
            
                foreach (var coupon in couponList)
                {
                    dr1 = dt.NewRow();
                    dr1["SrNo"] = SrNo;
                    dr1["CouponNo"] = coupon.COUP_Id;
                    if (coupon.COUP_Status_tint == 1)
                    {
                        dr1["Status"] = "Used";
                        dr1["UsedOn"] = Convert.ToDateTime(coupon.COUP_UsedOnDate_dt).ToString("dd/MM/yyyy");
                        if (coupon.COUP_ReferenceNo_var != null)
                            dr1["UsedOn"] += " / Ref No. " + coupon.COUP_ReferenceNo_var.ToString();
                        else
                            dr1["UsedOn"] += " / Cancelled ";
                        var site = dc.Coupon_View_Site(coupon.COUP_Id, coupon.COUP_CL_Id);
                        foreach (var st in site)
                        {
                            dr1["SiteName"] = st.SITE_Name_var;
                        }
                    }
                    else if (coupon.COUP_Status_tint == 0)
                    {
                        dr1["Status"] = "Not Used";
                        if (coupon.COUP_ExpiryDate_dt <= DateTime.Now)
                        {
                            dr1["Status"] = "Expired";
                            dr1["ExpiredOn"] = Convert.ToDateTime(coupon.COUP_ExpiryDate_dt).ToString("dd/MM/yyyy");
                        }
                    }

                    dt.Rows.Add(dr1);
                    SrNo++;
                }
            }
            else if (rdoST.Checked)
            {
                var couponList = dc.Coupon_ViewST(billId, 0, 0, 0, 0, 0, null);

                foreach (var coupon in couponList)
                {
                    dr1 = dt.NewRow();
                    dr1["SrNo"] = SrNo;
                    dr1["CouponNo"] = coupon.COUP_Id;
                    if (coupon.COUP_Status_tint == 1)
                    {
                        dr1["Status"] = "Used";
                        dr1["UsedOn"] = Convert.ToDateTime(coupon.COUP_UsedOnDate_dt).ToString("dd/MM/yyyy");
                        if (coupon.COUP_ReferenceNo_var != null)
                            dr1["UsedOn"] += " / Ref No. " + coupon.COUP_ReferenceNo_var.ToString();
                        else
                            dr1["UsedOn"] += " / Cancelled ";
                        var site = dc.Coupon_View_SiteST(coupon.COUP_Id, coupon.COUP_CL_Id);
                        foreach (var st in site)
                        {
                            dr1["SiteName"] = st.SITE_Name_var;
                        }
                    }
                    else if (coupon.COUP_Status_tint == 0)
                    {
                        dr1["Status"] = "Not Used";
                        if (coupon.COUP_ExpiryDate_dt <= DateTime.Now)
                        {
                            dr1["Status"] = "Expired";
                            dr1["ExpiredOn"] = Convert.ToDateTime(coupon.COUP_ExpiryDate_dt).ToString("dd/MM/yyyy");
                        }
                    }

                    dt.Rows.Add(dr1);
                    SrNo++;
                }
            }
            grdCoupon.DataSource = dt;
            grdCoupon.DataBind();
            Session["CouponTable"] = dt;
            for (int i = 0; i < grdCoupon.Rows.Count; i++)
            {
                if (grdCoupon.Rows[i].Cells[4].Text != "Not Used")
                {
                    CheckBox chkSelect = (CheckBox)grdCoupon.Rows[i].FindControl("chkSelect");
                    chkSelect.Enabled = false;
                }
            }
        }
        protected void lnkCancelCoupons_Click(object sender, EventArgs e)
        {
            bool selectedFlag = false;
            if (grdCoupon.Rows.Count > 0)
            {
                for (int i = 0; i < grdCoupon.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdCoupon.Rows[i].FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        selectedFlag = true;                        
                        break;
                    }
                }
            }
            if (selectedFlag == true )
            {
                txtComment.Text = txtComment.Text.Trim();
                if (txtComment.Text != "")
                {   
                    for (int i = 0; i < grdCoupon.Rows.Count; i++)
                    {
                        CheckBox chkSelect = (CheckBox)grdCoupon.Rows[i].FindControl("chkSelect");
                        if (chkSelect.Checked == true)
                        {
                            dc.Coupon_Update("", Convert.ToInt32(grdCoupon.Rows[i].Cells[3].Text), Convert.ToInt32(lblClientId.Text), 0, null, 1, DateTime.Now, "Cancel:" + txtComment.Text, 0, null, false, false);
                        }
                    }
                    DisplayClientList();
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Cancelled Successfully.');", true);
                }
                else
                {
                    txtComment.Focus();
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Enter comment/reason for cancellation');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Select at least one row');", true);
            }
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ChkBoxHeader = (CheckBox)grdCoupon.HeaderRow.FindControl("chkSelectAll");
            foreach (GridViewRow row in grdCoupon.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkSelect");
                if (ChkBoxHeader.Checked == true && row.Cells[4].Text == "Not Used")
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }

        protected void txt_Client_TextChanged(object sender, EventArgs e)
        {
            if (ChkClientName(txt_Client.Text) == true)
            {
                if (txt_Client.Text != "")
                {
                    lblClientId.Text = Request.Form[hfClientId.UniqueID];
                }
                else
                {
                    lblClientId.Text = "0";
                }
            }
        }

        protected Boolean ChkClientName(string searchHead)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            searchHead = txt_Client.Text;
            Boolean valid = false;
            var query = dc.Client_View(0, 0, searchHead, "");
            foreach (var obj in query)
            {
                valid = true;
            }
            if (valid == false)
            {
                txt_Client.Focus();
                lblMsg.Visible = true;
                lblMsg.Text = "This Client is not in the list";
            }
            else
            {
                lblMsg.Visible = false;
                valid = true;
            }
            return valid;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetClientname(string prefixText)
        {
            string searchHead = "";
            LabDataDataContext db = new LabDataDataContext();
            if (prefixText != "")
                searchHead = prefixText + "%";
            var query = db.Client_View(0, -1, searchHead, "");
            DataRow row = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("CL_Name_var");
            foreach (var rowObj in query)
            {
                row = dt.NewRow();
                string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                dt.Rows.Add(item);
            }
            if (row == null)
            {
                var clnm = db.Client_View(0, 0, "", "");
                foreach (var rowObj in clnm)
                {
                    row = dt.NewRow();
                    string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(rowObj.CL_Name_var, rowObj.CL_Id.ToString());
                    dt.Rows.Add(item);
                }
            }
            List<string> CL_Name_var = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CL_Name_var.Add(dt.Rows[i][0].ToString());
            }
            return CL_Name_var;

        }

        protected void lnkTemp_Click(object sender, EventArgs e)
        {
            var cubein = dc.temp_getcubeinward();
            foreach (var ct in cubein)
            {
                string[] strCoup = ct.CTINWD_CouponNo_var.Split(',');
                for (int i = 0; i < strCoup.Count()-1; i++)
                {
                    dc.Coupon_Update("",Convert.ToInt32(strCoup[i]), ct.INWD_CL_Id, 0, null, 1, ct.INWD_ReceivedDate_dt, ct.CTINWD_ReferenceNo_var, ct.CTINWD_RecordNo_int, null, false, false);
                }
            }
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Text = "Updated";
            lblMsg.Visible = true;
        }

        protected string RptCouponStatus()
        {
            string reportStr = "", mySql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            mySql += "<tr><td width='100%' height='105'>";

            mySql += "&nbsp;</td></tr>";
            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr><td width='99%' colspan=8 align=center valign=top height=19><font size=4><b> Coupon Status </b></font></td></tr>";
            mySql += "<tr><td width='99%' colspan=8>&nbsp;</td></tr>";

            mySql += "<tr>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b>  From Date   </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yyyy")  + "</font></td>" +
               "<td width='15%' align=left valign=top height=19><font size=2><b> To Date  </b></font></td>" +
               "<td width='2%' height=19><font size=2>:</font></td>" +
               "<td width='40%' height=19><font size=2>" + DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null).ToString("dd-MMM-yyyy") + "</font></td>" +
               "</tr>";


            mySql += "<tr><td width='99%' colspan=8>&nbsp;</td></tr>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 3% align=center valign=medium height=19 ><font size=2><b> Sr.No. </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Client Name  </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b> Site Name </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b> Bill No. </b></font></td>";
            mySql += "<td width= 8% align=center valign=medium height=19 ><font size=2><b> Issued On </b></font></td>";
            mySql += "<td width= 8% align=center valign=medium height=19 ><font size=2><b> Total Coupons </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 colspan=3 ><font size=2><b>  Used  </b></font></td>";
            mySql += "<td width= 10% align=center valign=medium height=19 ><font size=2><b>  Not Used  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Expired  </b></font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td align=center valign=medium height=19 ><font size=2><b>  </b></font></td>";
            mySql += "<td align=center valign=medium height=19 ><font size=2><b>  </b></font></td>";
            mySql += "<td align=center valign=medium height=19 ><font size=2><b>  </b></font></td>";
            mySql += "<td align=center valign=medium height=19 ><font size=2><b> </b></font></td>";
            mySql += "<td align=center valign=medium height=19 ><font size=2><b>  </b></font></td>";
            mySql += "<td align=center valign=medium height=19 ><font size=2><b>  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Coupon No.  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Date  </b></font></td>";
            mySql += "<td width= 5% align=center valign=medium height=19 ><font size=2><b>  Record No.  </b></font></td>";
            mySql += "<td align=center valign=medium height=19 ><font size=2><b>    </b></font></td>";
            mySql += "<td align=center valign=medium height=19 ><font size=2><b>    </b></font></td>";
            mySql += "</tr>";

            for (int i = 0; i < grdClient.Rows.Count; i++)
            {
                mySql += "<tr>";
                mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + grdClient.Rows[i].Cells[0].Text + "</font></td>";
                mySql += "<td align=left valign=top height=19 ><font size=2>&nbsp;" + grdClient.Rows[i].Cells[2].Text + "</font></td>";
                mySql += "<td align=left valign=top height=19 ><font size=2>&nbsp;" + grdClient.Rows[i].Cells[3].Text + "</font></td>";
                mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + grdClient.Rows[i].Cells[4].Text + "</font></td>";
                mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + grdClient.Rows[i].Cells[5].Text + "</font></td>";
                mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + grdClient.Rows[i].Cells[6].Text + "</font></td>";                
                mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" +"" + "</font></td>";
                mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + grdClient.Rows[i].Cells[8].Text + "</font></td>";
                mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + grdClient.Rows[i].Cells[9].Text + "</font></td>";
                mySql += "</tr>";

                int cnt = 0, usedCnt = 0;
                usedCnt = Convert.ToInt32(grdClient.Rows[i].Cells[7].Text);
                
                string strNotUsedCoupn = "", strExpiredCoupon = "";
                var couponList = dc.Coupon_View(grdClient.Rows[i].Cells[4].Text, 0, 0, 0, 0, 0, null);
                foreach (var coupon in couponList)
                {
                    string siteName = "";
                    if (coupon.COUP_Status_tint == 0)
                    {
                        if (coupon.COUP_ExpiryDate_dt <= DateTime.Now)
                        {
                            strExpiredCoupon += coupon.COUP_Id + ", ";
                        }
                        else
                        {
                            strNotUsedCoupn += coupon.COUP_Id + ", ";
                        }
                    }
                    else if (coupon.COUP_Status_tint == 1)
                    {
                        var site = dc.Coupon_View_Site(coupon.COUP_Id, coupon.COUP_CL_Id);
                        foreach (var st in site)
                        {
                            siteName = st.SITE_Name_var;
                        }

                        mySql += "<tr>";
                        mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                        mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                        mySql += "<td align=left valign=top height=19 ><font size=2>&nbsp;" + siteName + "</font></td>";
                        mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                        mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                        mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                        mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + coupon.COUP_Id + "</font></td>";
                        mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDateTime(coupon.COUP_UsedOnDate_dt).ToString("dd/MM/yyyy") + "</font></td>";
                        mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + coupon.COUP_RecordNo_int + "</font></td>";
                        if (cnt == 0)
                        {
                            mySql += "<td align=left valign=top height=19 rowspan=" + usedCnt + "><font size=2>&nbsp;" + strNotUsedCoupn + "</font></td>";
                            mySql += "<td align=left valign=top height=19 rowspan=" + usedCnt + "><font size=2>&nbsp;" + strExpiredCoupon + "</font></td>";

                        }                        
                        mySql += "</tr>";

                        cnt++;

                    }                                        
                }
                if (cnt == 0)
                {
                    mySql += "<tr>";
                    mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                    mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                    mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                    mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                    mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                    mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                    mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                    mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                    mySql += "<td align=center valign=top height=19 ><font size=2>&nbsp;" + "" + "</font></td>";
                    mySql += "<td align=left valign=top height=19 wraptext=true><font size=2>&nbsp;" + strNotUsedCoupn + "</font></td>";
                    mySql += "<td align=left valign=top height=19 ><font size=2>&nbsp;" + strExpiredCoupon + "</font></td>";
                    mySql += "</tr>";
                }
            }
            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            mySql += "</table>";
            mySql += "</html>";
            return reportStr = mySql;
        }

        //protected void rdoCT_CheckedChanged(object sender, EventArgs e)
        //{

        //    if (rdoCT.Checked)
        //        rdoST.Checked = false;
        //}

        //protected void rdoST_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rdoST.Checked)
        //        rdoCT.Checked = false;
        //}
    }
}