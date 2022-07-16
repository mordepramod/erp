using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class ReportSalesApp : System.Web.UI.Page
    {
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Site Visit Report";

            }
        }
        protected void imgClose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        private void LoadInwarDType()
        {
            var inwd = dc.Material_View("", "");
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardTestType.DataValueField = "MATERIAL_Id";
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataBind();
            ddl_InwardTestType.Items.Insert(0, "---Select All---");
        }
        private void LoadGeoStatus()
        {
            var status = dc.GeoStatus_View();
            ddl_GeoTechStatus.DataTextField = "SITE_GeoInvstgn_var";
            ddl_GeoTechStatus.DataValueField = "SITE_GeoInvstgn_var";
            ddl_GeoTechStatus.DataSource = status;
            ddl_GeoTechStatus.DataBind();
            ddl_GeoTechStatus.Items.Insert(0, "---Select---");
        }
        private void LoadLabList()
        {
            var labList = dc.Lab_View();
            ddl_LabList.DataTextField = "Lab_Name_var";
            ddl_LabList.DataValueField = "Lab_Id";
            ddl_LabList.DataSource = labList;
            ddl_LabList.DataBind();
            ddl_LabList.Items.Insert(0, "---Select All---");
        }
        //private void LoadReasonList()
        //{
        //    var resnList = dc.Reason_View();
        //    ddl_Reason.DataTextField = "Lead_discription";
        //    ddl_Reason.DataValueField = "Lead_discription";
        //    ddl_Reason.DataSource = resnList;
        //    ddl_Reason.DataBind();
        //    ddl_Reason.Items.Insert(0, "---Select All---");
        //}
        protected void LoadMEList()
        {
            var data = dc.User_View_ME(-1);
            ddl_ME.DataSource = data;
            ddl_ME.DataTextField = "USER_Name_var";
            ddl_ME.DataValueField = "USER_Id";
            ddl_ME.DataBind();
            ddl_ME.Items.Insert(0, "---Select All---");
        }
        protected void ddl_ReportList_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearGrid();
            if (ddl_ReportList.SelectedIndex == 1 || ddl_ReportList.SelectedIndex == 0)
            {
                tr1.Visible = false;
                tr2.Visible = false;
                tr3.Visible = false;
                tr4.Visible = false;
                tr5.Visible = false;
                tr6.Visible = false;
                tr7.Visible = false;
            }
            else if (ddl_ReportList.SelectedIndex == 2)
            {
                LoadLabList();
                LoadInwarDType();
                LoadMEList();
                tr1.Visible = true;
                tr2.Visible = true;
                tr3.Visible = false;
                tr4.Visible = false;
                tr5.Visible = false;
                tr6.Visible = false;
                tr7.Visible = true;
            }
            else if (ddl_ReportList.SelectedIndex == 3)
            {
                LoadMEList();
                tr1.Visible = false;
                tr2.Visible = false;
                tr3.Visible = true;
                tr4.Visible = false;
                tr5.Visible = false;
                tr6.Visible = true;
                tr7.Visible = true;
            }
            else if (ddl_ReportList.SelectedIndex == 4)
            {
                LoadGeoStatus();
                LoadMEList();
                tr1.Visible = false;
                tr2.Visible = false;
                tr3.Visible = false;
                tr4.Visible = true;
                tr5.Visible = true;
                tr6.Visible = false;
                tr7.Visible = true;
            }
        }
        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            grdList.DataSource = null;
            grdList.DataBind();
                
            int User_ME_Id=0;
            if (ddl_ReportList.SelectedIndex > 1)
            {
                if (!ddl_ME.SelectedItem.Text.ToString().Equals("---Select All---"))
                    User_ME_Id = Convert.ToInt32(ddl_ME.SelectedValue);
            }
            if (ddl_ReportList.SelectedIndex == 1)
                LoadReportList(0,0, 0, "", 0, 0);
            else if (ddl_ReportList.SelectedIndex == 2)
            {
                
                    int val = 0,labId=0;
                    if (!(ddl_LabList.SelectedItem.Text.ToString().Equals("---Select All---")))
                        labId = Convert.ToInt32(ddl_LabList.SelectedValue);
                   
                    if (!ddl_InwardTestType.SelectedItem.Text.ToString().Equals("---Select All---"))
                        val = Convert.ToInt32(ddl_InwardTestType.SelectedValue);
                    LoadReportList(User_ME_Id, labId, Convert.ToInt32(val), "", 0, 1);
                
            }
            else if (ddl_ReportList.SelectedIndex == 3)
            {
                int status = -1;
                if (ddl_StageOfSite.SelectedItem.Text == "RCC")
                    status = 0;
                else if (ddl_StageOfSite.SelectedItem.Text == "Block Work Plaster")
                    status = 1;
                else if (ddl_StageOfSite.SelectedItem.Text == "Finishes")
                    status = 2;

                string str = "";
                if (!ddl_StageOfSiteStatus.SelectedItem.Text.ToString().Equals("---Select All---"))
                    str = ddl_StageOfSiteStatus.SelectedItem.Text.ToString();
    
                LoadReportList(User_ME_Id, 0, 0, str, status, 2);
            }
            else if (ddl_ReportList.SelectedIndex == 4)
            {
                LoadReportList(User_ME_Id, 0, 0, ddl_GeoTechStatus.SelectedItem.Text, 0, 3);
            }
        }

        private void LoadReportList(int User_ME_Id,int labId, int materialId,string filter,int status, int flag)
        {
            DataTable dt = new DataTable();
            DataRow dr1 = null; string buildUpAreaFrm = "0", buildUpAreaTo = "0";

            if (chkArea.Checked)
            {
                if (txt_AreaFrom.Text!="")
                    buildUpAreaFrm = txt_AreaFrom.Text;
                if (txt_AreaTo.Text != "")
                    buildUpAreaTo = txt_AreaTo.Text;
            }
            dt.Columns.Add(new DataColumn("Sr_No", typeof(string)));
            dt.Columns.Add(new DataColumn("Lab_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Aggregate_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("AAC_Block_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Brick_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Cement_Chemical_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Cement_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Core_Cutting", typeof(string)));
            dt.Columns.Add(new DataColumn("Core_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Cube_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Fly_Ash_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Soil_Investigation", typeof(string)));
            dt.Columns.Add(new DataColumn("Mix_Design", typeof(string)));
            dt.Columns.Add(new DataColumn("Non_Destructive_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Pile_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Pavement_Block_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Soil_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Masonary_Block_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Steel_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Steel_Chemical_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Tile_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Water_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Other_Testing", typeof(string)));
            dt.Columns.Add(new DataColumn("Rain_Water_Harvesting", typeof(string)));
            dt.Columns.Add(new DataColumn("USER_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("CL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Address_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_BulidUpArea_dec", typeof(string)));
            dt.Columns.Add(new DataColumn("Lead_discription", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("MATERIAL_Name_var", typeof(string)));
            dt.Columns.Add(new DataColumn("SITE_Id", typeof(string)));

            int rowNo = 1; int grdRowNo = 1;
            setColmVisibility();
            if (flag == 0)//report 1
            {
                var lab = dc.Lab_View();
                foreach (var log in lab)
                {
                    dr1 = dt.NewRow();
                    dr1["Sr_No"] = rowNo;
                    dr1["Lab_Name"] = log.Lab_Name_var;

                    var count = dc.SalesAppReport_View(User_ME_Id,Convert.ToInt32(log.Lab_Id), 0, "", buildUpAreaFrm, buildUpAreaTo, -1, 1).ToList();
                    for (int i = 0; i < count.Count; i++)
                    {
                        if(i==0)
                            dr1["Aggregate_Testing"] = count[i].SITE_Id;
                        else if(i==1)
                            dr1["AAC_Block_Testing"] = count[i].SITE_Id;
                        else if(i==2)
                            dr1["Brick_Testing"] = count[i].SITE_Id;
                        else if (i == 3)
                            dr1["Cement_Chemical_Testing"] = count[i].SITE_Id;
                        else if (i == 4)
                            dr1["Cement_Testing"] = count[i].SITE_Id;
                        else if (i == 5)
                            dr1["Core_Cutting"] = count[i].SITE_Id;
                        else if (i == 6)
                            dr1["Core_Testing"] = count[i].SITE_Id;
                        else if (i == 7)
                            dr1["Cube_Testing"] = count[i].SITE_Id;
                        else if (i == 8)
                            dr1["Fly_Ash_Testing"] = count[i].SITE_Id;
                        else if (i == 9)
                            dr1["Soil_Investigation"] = count[i].SITE_Id;
                        else if (i == 10)
                            dr1["Mix_Design"] = count[i].SITE_Id;
                        else if (i == 11)
                            dr1["Non_Destructive_Testing"] = count[i].SITE_Id;
                        else if (i == 12)
                            dr1["Pile_Testing"] = count[i].SITE_Id;
                        else if (i == 13)
                            dr1["Pavement_Block_Testing"] = count[i].SITE_Id;
                        else if (i == 14)
                            dr1["Soil_Testing"] = count[i].SITE_Id;
                        else if (i == 15)
                            dr1["Masonary_Block_Testing"] = count[i].SITE_Id;
                        else if (i == 16)
                            dr1["Steel_Testing"] = count[i].SITE_Id;
                        else if (i == 17)
                            dr1["Steel_Chemical_Testing"] = count[i].SITE_Id;
                        else if (i == 18)
                            dr1["Tile_Testing"] = count[i].SITE_Id;
                        else if (i == 20)
                            dr1["Water_Testing"] = count[i].SITE_Id;
                        else if (i == 21)
                            dr1["Other_Testing"] = count[i].SITE_Id;
                        else if (i == 22)
                            dr1["Rain_Water_Harvesting"] = count[i].SITE_Id;
                    }
                    dt.Rows.Add(dr1);
                    rowNo++;
                }

                grdList.Columns[1].Visible = true;
                grdList.Columns[2].Visible = true;
                grdList.Columns[3].Visible = true;
                grdList.Columns[4].Visible = true;
                grdList.Columns[5].Visible = true;
                grdList.Columns[6].Visible = true;
                grdList.Columns[7].Visible = true;
                grdList.Columns[8].Visible = true;
                grdList.Columns[9].Visible = true;
                grdList.Columns[10].Visible = true;
                grdList.Columns[11].Visible = true;
                grdList.Columns[12].Visible = true;
                grdList.Columns[13].Visible = true;
                grdList.Columns[14].Visible = true;
                grdList.Columns[15].Visible = true;
                grdList.Columns[16].Visible = true;
                grdList.Columns[17].Visible = true;
                grdList.Columns[18].Visible = true;
                grdList.Columns[19].Visible = true;
                grdList.Columns[20].Visible = true;
                grdList.Columns[21].Visible = true;
                grdList.Columns[22].Visible = true;
                grdList.Columns[23].Visible = true;
                
            }
            else if (flag == 1)//report 2
            {
                bool flagSameSite = false; string Lead_discription = "";
                var saleslog = dc.SalesAppReport_View(User_ME_Id,labId, materialId, "", buildUpAreaFrm, buildUpAreaTo, -1, 0);
                foreach (var log in saleslog)
                {
                    flagSameSite = false;
                    if (dt.Rows.Count > 0)
                    {
                        if ((dt.Rows[dt.Rows.Count - 1]["SITE_Id"].ToString() == Convert.ToString(log.SITE_Id)) && (Lead_discription == Convert.ToString(log.Lead_discription)))
                           flagSameSite = true;
                    }
                    dr1 = dt.NewRow();
                    if (!flagSameSite)
                    {
                        dr1["Sr_No"] = rowNo;
                        dr1["CL_Name_var"] = log.CL_Name_var;
                        dr1["SITE_Name_var"] = log.SITE_Name_var;
                        dr1["SITE_Address_var"] = log.SITE_Address_var;
                        dr1["USER_Name_var"] = log.USER_Name_var;
                        dr1["Lead_discription"] = log.Lead_discription;
                        rowNo++;
                    }
                    Lead_discription = log.Lead_discription;
                    dr1["SITE_Id"] = Convert.ToString(log.SITE_Id);
                    dr1["MATERIAL_Name_var"] = log.MATERIAL_Name_var;
                    dt.Rows.Add(dr1);
                    grdRowNo++;
                }
                //int srno = rowNo;
                //string reason = ""; int site_id = 0; bool flagSite = false;
                //var logSales = dc.SalesAppReport_View_NewSite(User_ME_Id, labId, materialId);
                //foreach (var log in logSales)
                //{
                //    flagSite = false;
                //    if (dt.Rows.Count > 0)
                //    {
                //        if (dt.Rows[dt.Rows.Count - 1]["SITE_Id"].ToString() == Convert.ToString(log.Site_Id))
                //           flagSite = true;
                //    }
                //    dr1 = dt.NewRow();
                //    dr1["MATERIAL_Name_var"] = log.MATERIAL_Name_var;
                //    dr1["SITE_Id"] = Convert.ToString(log.Site_Id);
                //    if (!flagSite)
                //    {
                //        dr1["Sr_No"] = rowNo;
                //        dr1["CL_Name_var"] = log.CL_Name_var;
                //        dr1["SITE_Name_var"] = log.SITE_Name_var;
                //        dr1["SITE_Address_var"] = log.SITE_Address_var;
                //        dr1["USER_Name_var"] = log.USER_Name_var;
                //        rowNo++;
                //        site_id = Convert.ToInt32(Convert.ToString(log.Site_Id));
                //        var rslt = dc.SalesAppReport_View_NewSiteReason(site_id).ToList();
                //        if (rslt.Count > 0)
                //        {

                //            if (Convert.ToString(rslt.FirstOrDefault().Rate) == "Yes")
                //                reason += "Rate,";
                //            if (Convert.ToString(rslt.FirstOrDefault().Service) == "Yes")
                //                reason += "Service,";
                //            if (Convert.ToString(rslt.FirstOrDefault().not_approach_by_durocrete) == "Yes")
                //                reason += "Not Approch By durocrete,";
                //            if (Convert.ToString(rslt.FirstOrDefault().satisfie_by_existing_lab) == "Yes")
                //                reason += "Satisfied By Existing Lab";

                //        }
                //        if (reason != "")
                //        {
                //            if (reason.Substring(reason.Length - 1) == ",")
                //                reason = reason.Remove(reason.Length - 1);
                //        }
                //        dr1["Lead_discription"] = reason;
                      
                //    }
                //    dt.Rows.Add(dr1);
                //    reason = ""; 
                //}
                grdList.Columns[24].Visible = true;
                grdList.Columns[25].Visible = true;
                grdList.Columns[26].Visible = true;
                grdList.Columns[27].Visible = true;
                grdList.Columns[28].Visible = true;
                grdList.Columns[30].Visible = true;            
                
            }
            else if (flag == 2)//report 3
            {
                var saleslog = dc.SalesAppReport_View(User_ME_Id,labId, materialId, filter, buildUpAreaFrm, buildUpAreaTo, status, 3);
                foreach (var log in saleslog)
                {
                    dr1 = dt.NewRow();
                    dr1["Sr_No"] = rowNo;
                    dr1["CL_Name_var"] = log.CL_Name_var;
                    dr1["SITE_Name_var"] = log.SITE_Name_var;
                    dr1["SITE_Address_var"] = log.SITE_Address_var;
                    dr1["USER_Name_var"] = log.USER_Name_var;
                    dr1["Status"] = log.SiteStatus;
                    dt.Rows.Add(dr1);
                    rowNo++;
                }
                grdList.Columns[24].Visible = true;
                grdList.Columns[25].Visible = true;
                grdList.Columns[26].Visible = true;
                grdList.Columns[29].Visible = true;
                grdList.Columns[30].Visible = true;
                           
            }
            else if (flag == 3)//report 4
            {
                var saleslog = dc.SalesAppReport_View(User_ME_Id,labId, materialId, filter, buildUpAreaFrm, buildUpAreaTo, -1, 2);
                foreach (var log in saleslog)
                {
                    dr1 = dt.NewRow();
                    dr1["Sr_No"] = rowNo;
                    dr1["SITE_Name_var"] = log.SITE_Name_var;
                    dr1["CL_Name_var"] = log.CL_Name_var;
                    dr1["SITE_Address_var"] = log.SITE_Address_var;
                    dr1["USER_Name_var"] = log.USER_Name_var;
                    dr1["SITE_BulidUpArea_dec"] = log.SITE_BulidUpArea_dec;
                    dt.Rows.Add(dr1);
                    rowNo++;
                }

                grdList.Columns[24].Visible = true;
                grdList.Columns[25].Visible = true;
                grdList.Columns[26].Visible = true;
                grdList.Columns[30].Visible = true;
                grdList.Columns[31].Visible = true;
              }
           
            grdList.DataSource = dt;
            grdList.DataBind();

            if (flag == 1)
            {
                for (int i = grdRowNo - 1; i < grdList.Rows.Count; i++)
                {
                    for (int j = 0; j < grdList.Columns.Count; j++)
                    {
                        grdList.Rows[i].Cells[j].BackColor = System.Drawing.Color.LightGray;
                    }
                }
            }
            lblTotalRecords.Text = "Total No of Records : " + grdList.Rows.Count;
        }
        private void setColmVisibility()
        {
            grdList.Columns[1].Visible = false;
            grdList.Columns[2].Visible = false;
            grdList.Columns[3].Visible = false;
            grdList.Columns[4].Visible = false;
            grdList.Columns[5].Visible = false;
            grdList.Columns[6].Visible = false;
            grdList.Columns[7].Visible = false;
            grdList.Columns[8].Visible = false;
            grdList.Columns[9].Visible = false;
            grdList.Columns[10].Visible = false;
            grdList.Columns[11].Visible = false;
            grdList.Columns[12].Visible = false;
            grdList.Columns[13].Visible = false;
            grdList.Columns[14].Visible = false;
            grdList.Columns[15].Visible = false;
            grdList.Columns[16].Visible = false;
            grdList.Columns[17].Visible = false;
            grdList.Columns[18].Visible = false;
            grdList.Columns[19].Visible = false;
            grdList.Columns[20].Visible = false;
            grdList.Columns[21].Visible = false;
            grdList.Columns[22].Visible = false;
            grdList.Columns[23].Visible = false;
            grdList.Columns[24].Visible = false;
            grdList.Columns[25].Visible = false;
            grdList.Columns[26].Visible = false;
            grdList.Columns[27].Visible = false;
            grdList.Columns[28].Visible = false;
            grdList.Columns[29].Visible = false;
            grdList.Columns[30].Visible = false;
            grdList.Columns[31].Visible = false;
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (grdList.Rows.Count > 0)
            {
                string rptNm = "SalesAppReport";
                if (ddl_ReportList.SelectedIndex>0)
                    rptNm=ddl_ReportList.SelectedItem.Text.Replace(" ","_");
                PrintGrid.PrintGridView_SalesAppReport(grdList, "Sales App Report - " + rptNm, rptNm);
            }
        }
        public void clearGrid()
        {
            txt_AreaFrom.Text = "";
            txt_AreaTo.Text = "";
            chkArea.Checked = false;
            ddl_GeoTechStatus.SelectedIndex = -1; 
            ddl_StageOfSite.SelectedIndex = 0;
            ddl_StageOfSiteStatus.SelectedIndex = 0;
            grdList.DataSource = null;
            grdList.DataBind();
            lblTotalRecords.Text = "Total No of Records : " + grdList.Rows.Count;

        }
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        protected void chkArea_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkArea.Checked)
            {
                txt_AreaFrom.Text = ""; txt_AreaTo.Text = "";
            }
        }

  
     
    }
}