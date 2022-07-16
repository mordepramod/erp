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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;
using System.Text;
using iTextSharp.text.html.simpleparser;
using System.Collections.Generic;

namespace DESPLWEB
{
    public partial class MoistCorrection : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Method of Moisture Correction ";
                DisplayMoistCorr();
            }
        }


        protected void DisplayMoistCorr()
        {
            txt_RecType.Text = "MF";
            int s = 0;
            var res = dc.MaterialDetail_View(0, txt_RefNo.Text, 0, "", null, null, "");
            foreach (var t in res)
            {
                if (Convert.ToString(t.Material_List) == "Crushed Sand")
                {
                    s++;
                }
                if (Convert.ToString(t.Material_List) == "Natural Sand")
                {
                    s++;
                }
            }
            int Counter = 0;
            if (s == 2)
            {
                Counter = 10;

                grdMoistureCorr.Columns[1].Visible = false;
                grdMoistureCorr.Columns[2].Visible = false;
                grdMoistureCorr.Columns[3].Visible = true;
                grdMoistureCorr.Columns[4].Visible = true;
                grdMoistureCorr.Columns[5].Visible = true;
                grdMoistureCorr.Columns[6].Visible = true;
                grdMoistureCorr.Columns[7].Visible = true;
                grdMoistureCorr.Columns[8].Visible = true;
                grdMoistureCorr.Columns[9].Visible = true;
                grdMoistureCorr.Columns[10].Visible = true;
                grdMoistureCorr.Columns[11].Visible = true;
            }
            else if (s == 1)
            {
                Counter = 12;
                PnlNSICS.BorderStyle = BorderStyle.None;
                grdMoistureCorr.Columns[1].Visible = true;
                grdMoistureCorr.Columns[2].Visible = true;
                grdMoistureCorr.Columns[3].Visible = false;
                grdMoistureCorr.Columns[4].Visible = false;
                grdMoistureCorr.Columns[5].Visible = false;
                grdMoistureCorr.Columns[6].Visible = false;
                grdMoistureCorr.Columns[7].Visible = false;
                grdMoistureCorr.Columns[8].Visible = false;
                grdMoistureCorr.Columns[9].Visible = false;
                grdMoistureCorr.Columns[10].Visible = false;
                grdMoistureCorr.Columns[11].Visible = false;
            }

            for (int i = 0; i < Counter; i++)
            {
                AddRowMoistureCorr();

            }

            for (int i = 0; i < grdMoistureCorr.Rows.Count; i++)
            {
                TextBox txt_TotalMoisture = (TextBox)grdMoistureCorr.Rows[i].Cells[0].FindControl("txt_TotalMoisture");
                TextBox txt_WaterTobeAdded = (TextBox)grdMoistureCorr.Rows[i].Cells[1].FindControl("txt_WaterTobeAdded");
                TextBox txt_WtofFineAggt = (TextBox)grdMoistureCorr.Rows[i].Cells[2].FindControl("txt_WtofFineAggt");

                if (Counter == 12)
                {
                    txt_TotalMoisture.Width = 220;
                    txt_WaterTobeAdded.Width = 220;
                    txt_WtofFineAggt.Width = 220;
                }
                else
                {
                    txt_TotalMoisture.Width = 100;
                    txt_WaterTobeAdded.Width = 100;
                    txt_WtofFineAggt.Width = 100;
                }
            }
            if (Counter == 10)
            {

                grdNSICS.Visible = true;
                for (int j = 0; j < Counter; j++)
                {
                    AddRowNSICS();
                }
            }
            for (int j = 0; j < 4; j++)
            {
                AddRowEquipment();
            }

        }
        protected void AddRowMoistureCorr()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MoistureCorrTable"] != null)
            {
                GetCurrentDataMoistureCorr();
                dt = (DataTable)ViewState["MoistureCorrTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_TotalMoisture", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WaterTobeAdded", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_WtofFineAggt", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_1", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_2", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_3", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_4", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_5", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_6", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_7", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_8", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_9", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_TotalMoisture"] = string.Empty;
            dr["txt_WaterTobeAdded"] = string.Empty;
            dr["txt_WtofFineAggt"] = string.Empty;
            dr["txt_1"] = string.Empty;
            dr["txt_2"] = string.Empty;
            dr["txt_3"] = string.Empty;
            dr["txt_4"] = string.Empty;
            dr["txt_5"] = string.Empty;
            dr["txt_6"] = string.Empty;
            dr["txt_7"] = string.Empty;
            dr["txt_8"] = string.Empty;
            dr["txt_9"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["MoistureCorrTable"] = dt;
            grdMoistureCorr.DataSource = dt;
            grdMoistureCorr.DataBind();
            SetPreviousDataMoistureCorr();
        }
        protected void GetCurrentDataMoistureCorr()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_TotalMoisture", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WaterTobeAdded", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_WtofFineAggt", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_5", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_6", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_7", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_8", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_9", typeof(string)));

            for (int i = 0; i < grdMoistureCorr.Rows.Count; i++)
            {
                TextBox txt_TotalMoisture = (TextBox)grdMoistureCorr.Rows[i].Cells[0].FindControl("txt_TotalMoisture");
                TextBox txt_WaterTobeAdded = (TextBox)grdMoistureCorr.Rows[i].Cells[1].FindControl("txt_WaterTobeAdded");
                TextBox txt_WtofFineAggt = (TextBox)grdMoistureCorr.Rows[i].Cells[2].FindControl("txt_WtofFineAggt");
                TextBox txt_1 = (TextBox)grdMoistureCorr.Rows[i].Cells[3].FindControl("txt_1");
                TextBox txt_2 = (TextBox)grdMoistureCorr.Rows[i].Cells[4].FindControl("txt_2");
                TextBox txt_3 = (TextBox)grdMoistureCorr.Rows[i].Cells[5].FindControl("txt_3");
                TextBox txt_4 = (TextBox)grdMoistureCorr.Rows[i].Cells[6].FindControl("txt_4");
                TextBox txt_5 = (TextBox)grdMoistureCorr.Rows[i].Cells[7].FindControl("txt_5");
                TextBox txt_6 = (TextBox)grdMoistureCorr.Rows[i].Cells[8].FindControl("txt_6");
                TextBox txt_7 = (TextBox)grdMoistureCorr.Rows[i].Cells[9].FindControl("txt_7");
                TextBox txt_8 = (TextBox)grdMoistureCorr.Rows[i].Cells[10].FindControl("txt_8");
                TextBox txt_9 = (TextBox)grdMoistureCorr.Rows[i].Cells[11].FindControl("txt_9");

                drRow = dtTable.NewRow();
                drRow["txt_TotalMoisture"] = txt_TotalMoisture.Text;
                drRow["txt_WaterTobeAdded"] = txt_WaterTobeAdded.Text;
                drRow["txt_WtofFineAggt"] = txt_WtofFineAggt.Text;
                drRow["txt_1"] = txt_1.Text;
                drRow["txt_2"] = txt_2.Text;
                drRow["txt_3"] = txt_3.Text;
                drRow["txt_4"] = txt_4.Text;
                drRow["txt_5"] = txt_5.Text;
                drRow["txt_6"] = txt_6.Text;
                drRow["txt_7"] = txt_7.Text;
                drRow["txt_8"] = txt_8.Text;
                drRow["txt_9"] = txt_9.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["MoistureCorrTable"] = dtTable;

        }
        protected void SetPreviousDataMoistureCorr()
        {
            DataTable dt = (DataTable)ViewState["MoistureCorrTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_TotalMoisture = (TextBox)grdMoistureCorr.Rows[i].Cells[0].FindControl("txt_TotalMoisture");
                TextBox txt_WaterTobeAdded = (TextBox)grdMoistureCorr.Rows[i].Cells[1].FindControl("txt_WaterTobeAdded");
                TextBox txt_WtofFineAggt = (TextBox)grdMoistureCorr.Rows[i].Cells[2].FindControl("txt_WtofFineAggt");
                TextBox txt_1 = (TextBox)grdMoistureCorr.Rows[i].Cells[3].FindControl("txt_1");
                TextBox txt_2 = (TextBox)grdMoistureCorr.Rows[i].Cells[4].FindControl("txt_2");
                TextBox txt_3 = (TextBox)grdMoistureCorr.Rows[i].Cells[5].FindControl("txt_3");
                TextBox txt_4 = (TextBox)grdMoistureCorr.Rows[i].Cells[6].FindControl("txt_4");
                TextBox txt_5 = (TextBox)grdMoistureCorr.Rows[i].Cells[7].FindControl("txt_5");
                TextBox txt_6 = (TextBox)grdMoistureCorr.Rows[i].Cells[8].FindControl("txt_6");
                TextBox txt_7 = (TextBox)grdMoistureCorr.Rows[i].Cells[9].FindControl("txt_7");
                TextBox txt_8 = (TextBox)grdMoistureCorr.Rows[i].Cells[10].FindControl("txt_8");
                TextBox txt_9 = (TextBox)grdMoistureCorr.Rows[i].Cells[11].FindControl("txt_9");

                txt_TotalMoisture.Text = dt.Rows[i]["txt_TotalMoisture"].ToString();
                txt_WaterTobeAdded.Text = dt.Rows[i]["txt_WaterTobeAdded"].ToString();
                txt_WtofFineAggt.Text = dt.Rows[i]["txt_WtofFineAggt"].ToString();
                txt_1.Text = dt.Rows[i]["txt_1"].ToString();
                txt_2.Text = dt.Rows[i]["txt_2"].ToString();
                txt_3.Text = dt.Rows[i]["txt_3"].ToString();
                txt_4.Text = dt.Rows[i]["txt_4"].ToString();
                txt_5.Text = dt.Rows[i]["txt_5"].ToString();
                txt_6.Text = dt.Rows[i]["txt_6"].ToString();
                txt_7.Text = dt.Rows[i]["txt_7"].ToString();
                txt_8.Text = dt.Rows[i]["txt_8"].ToString();
                txt_9.Text = dt.Rows[i]["txt_9"].ToString();
            }
        }

        protected void AddRowEquipment()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["EquipmentTable"] != null)
            {
                GetCurrentDataEquipment();
                dt = (DataTable)ViewState["EquipmentTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_ConditionofSand", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Watertobe", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_ConditionofSand"] = string.Empty;
            dr["txt_Watertobe"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["EquipmentTable"] = dt;
            grdEquipment.DataSource = dt;
            grdEquipment.DataBind();
            SetPreviousDataEquipment();
        }
        protected void GetCurrentDataEquipment()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_ConditionofSand", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Watertobe", typeof(string)));
            for (int i = 0; i < grdEquipment.Rows.Count; i++)
            {
                TextBox txt_ConditionofSand = (TextBox)grdEquipment.Rows[i].Cells[0].FindControl("txt_ConditionofSand");
                TextBox txt_Watertobe = (TextBox)grdEquipment.Rows[i].Cells[1].FindControl("txt_Watertobe");
                drRow = dtTable.NewRow();
                drRow["txt_ConditionofSand"] = txt_ConditionofSand.Text;
                drRow["txt_Watertobe"] = txt_Watertobe.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["EquipmentTable"] = dtTable;

        }
        protected void SetPreviousDataEquipment()
        {
            DataTable dt = (DataTable)ViewState["EquipmentTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_ConditionofSand = (TextBox)grdEquipment.Rows[i].Cells[0].FindControl("txt_ConditionofSand");
                TextBox txt_Watertobe = (TextBox)grdEquipment.Rows[i].Cells[1].FindControl("txt_Watertobe");

                txt_ConditionofSand.Text = dt.Rows[i]["txt_ConditionofSand"].ToString();
                txt_Watertobe.Text = dt.Rows[i]["txt_Watertobe"].ToString();
            }
        }

        protected void AddRowNSICS()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["NSICSTable"] != null)
            {
                GetCurrentDataNSICS();
                dt = (DataTable)ViewState["NSICSTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_NSICS", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_1", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_2", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_3", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_4", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_5", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_6", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_7", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_8", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_9", typeof(string)));
            }
            dr = dt.NewRow();
            dr["txt_NSICS"] = string.Empty;
            dr["txt_1"] = string.Empty;
            dr["txt_2"] = string.Empty;
            dr["txt_3"] = string.Empty;
            dr["txt_4"] = string.Empty;
            dr["txt_5"] = string.Empty;
            dr["txt_6"] = string.Empty;
            dr["txt_7"] = string.Empty;
            dr["txt_8"] = string.Empty;
            dr["txt_9"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["NSICSTable"] = dt;
            grdNSICS.DataSource = dt;
            grdNSICS.DataBind();
            SetPreviousDataNSICS();
        }
        protected void GetCurrentDataNSICS()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_NSICS", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_5", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_6", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_7", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_8", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_9", typeof(string)));

            for (int i = 0; i < grdNSICS.Rows.Count; i++)
            {
                TextBox txt_NSICS = (TextBox)grdNSICS.Rows[i].Cells[0].FindControl("txt_NSICS");
                TextBox txt_1 = (TextBox)grdNSICS.Rows[i].Cells[1].FindControl("txt_1");
                TextBox txt_2 = (TextBox)grdNSICS.Rows[i].Cells[2].FindControl("txt_2");
                TextBox txt_3 = (TextBox)grdNSICS.Rows[i].Cells[3].FindControl("txt_3");
                TextBox txt_4 = (TextBox)grdNSICS.Rows[i].Cells[4].FindControl("txt_4");
                TextBox txt_5 = (TextBox)grdNSICS.Rows[i].Cells[5].FindControl("txt_5");
                TextBox txt_6 = (TextBox)grdNSICS.Rows[i].Cells[6].FindControl("txt_6");
                TextBox txt_7 = (TextBox)grdNSICS.Rows[i].Cells[7].FindControl("txt_7");
                TextBox txt_8 = (TextBox)grdNSICS.Rows[i].Cells[8].FindControl("txt_8");
                TextBox txt_9 = (TextBox)grdNSICS.Rows[i].Cells[9].FindControl("txt_9");

                drRow = dtTable.NewRow();
                drRow["txt_NSICS"] = txt_NSICS.Text;
                drRow["txt_1"] = txt_1.Text;
                drRow["txt_2"] = txt_2.Text;
                drRow["txt_3"] = txt_3.Text;
                drRow["txt_4"] = txt_4.Text;
                drRow["txt_5"] = txt_5.Text;
                drRow["txt_6"] = txt_6.Text;
                drRow["txt_7"] = txt_7.Text;
                drRow["txt_8"] = txt_8.Text;
                drRow["txt_9"] = txt_9.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["NSICSTable"] = dtTable;

        }
        protected void SetPreviousDataNSICS()
        {
            DataTable dt = (DataTable)ViewState["NSICSTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_NSICS = (TextBox)grdNSICS.Rows[i].Cells[0].FindControl("txt_NSICS");
                TextBox txt_1 = (TextBox)grdNSICS.Rows[i].Cells[1].FindControl("txt_1");
                TextBox txt_2 = (TextBox)grdNSICS.Rows[i].Cells[2].FindControl("txt_2");
                TextBox txt_3 = (TextBox)grdNSICS.Rows[i].Cells[3].FindControl("txt_3");
                TextBox txt_4 = (TextBox)grdNSICS.Rows[i].Cells[4].FindControl("txt_4");
                TextBox txt_5 = (TextBox)grdNSICS.Rows[i].Cells[5].FindControl("txt_5");
                TextBox txt_6 = (TextBox)grdNSICS.Rows[i].Cells[6].FindControl("txt_6");
                TextBox txt_7 = (TextBox)grdNSICS.Rows[i].Cells[7].FindControl("txt_7");
                TextBox txt_8 = (TextBox)grdNSICS.Rows[i].Cells[8].FindControl("txt_8");
                TextBox txt_9 = (TextBox)grdNSICS.Rows[i].Cells[9].FindControl("txt_9");

                txt_NSICS.Text = dt.Rows[i]["txt_NSICS"].ToString();
                txt_1.Text = dt.Rows[i]["txt_1"].ToString();
                txt_2.Text = dt.Rows[i]["txt_2"].ToString();
                txt_3.Text = dt.Rows[i]["txt_3"].ToString();
                txt_4.Text = dt.Rows[i]["txt_4"].ToString();
                txt_5.Text = dt.Rows[i]["txt_5"].ToString();
                txt_6.Text = dt.Rows[i]["txt_6"].ToString();
                txt_7.Text = dt.Rows[i]["txt_7"].ToString();
                txt_8.Text = dt.Rows[i]["txt_8"].ToString();
                txt_9.Text = dt.Rows[i]["txt_9"].ToString();
            }
        }


        protected void lnkSave_Click(object sender, EventArgs e)
        {

            string MoistureCorrDtl = "";
            string NSICSDtl = "";
            string EquipDtl = "";
            dc.MoistureCorrection_Update(txt_RefNo.Text, "", "", "", "", true);
            for (int i = 0; i < grdMoistureCorr.Rows.Count; i++)
            {
                TextBox txt_TotalMoisture = (TextBox)grdMoistureCorr.Rows[i].Cells[0].FindControl("txt_TotalMoisture");
                TextBox txt_WaterTobeAdded = (TextBox)grdMoistureCorr.Rows[i].Cells[1].FindControl("txt_WaterTobeAdded");
                TextBox txt_WtofFineAggt = (TextBox)grdMoistureCorr.Rows[i].Cells[2].FindControl("txt_WtofFineAggt");
                TextBox txt_1 = (TextBox)grdMoistureCorr.Rows[i].Cells[3].FindControl("txt_1");
                TextBox txt_2 = (TextBox)grdMoistureCorr.Rows[i].Cells[4].FindControl("txt_2");
                TextBox txt_3 = (TextBox)grdMoistureCorr.Rows[i].Cells[5].FindControl("txt_3");
                TextBox txt_4 = (TextBox)grdMoistureCorr.Rows[i].Cells[6].FindControl("txt_4");
                TextBox txt_5 = (TextBox)grdMoistureCorr.Rows[i].Cells[7].FindControl("txt_5");
                TextBox txt_6 = (TextBox)grdMoistureCorr.Rows[i].Cells[8].FindControl("txt_6");
                TextBox txt_7 = (TextBox)grdMoistureCorr.Rows[i].Cells[9].FindControl("txt_7");
                TextBox txt_8 = (TextBox)grdMoistureCorr.Rows[i].Cells[10].FindControl("txt_8");
                TextBox txt_9 = (TextBox)grdMoistureCorr.Rows[i].Cells[11].FindControl("txt_9");

                if (grdMoistureCorr.Columns[1].Visible == true && grdMoistureCorr.Columns[2].Visible == true)
                {

                    MoistureCorrDtl += txt_TotalMoisture.Text + "|" + txt_WaterTobeAdded.Text + "|" + txt_WtofFineAggt.Text + "|";
                }
                else
                {

                    MoistureCorrDtl += txt_TotalMoisture.Text + "|" + txt_1.Text + "|" + txt_2.Text + "|" + txt_3.Text + "|" + txt_4.Text + "|" + txt_5.Text + "|" + txt_6.Text + "|" + txt_7.Text + "|" + txt_8.Text + "|" + txt_9.Text + "|";

                }
            }
            for (int i = 0; i < grdNSICS.Rows.Count; i++)
            {
                TextBox txt_NSICS = (TextBox)grdNSICS.Rows[i].Cells[0].FindControl("txt_NSICS");
                TextBox txt_1 = (TextBox)grdNSICS.Rows[i].Cells[1].FindControl("txt_1");
                TextBox txt_2 = (TextBox)grdNSICS.Rows[i].Cells[2].FindControl("txt_2");
                TextBox txt_3 = (TextBox)grdNSICS.Rows[i].Cells[3].FindControl("txt_3");
                TextBox txt_4 = (TextBox)grdNSICS.Rows[i].Cells[4].FindControl("txt_4");
                TextBox txt_5 = (TextBox)grdNSICS.Rows[i].Cells[5].FindControl("txt_5");
                TextBox txt_6 = (TextBox)grdNSICS.Rows[i].Cells[6].FindControl("txt_6");
                TextBox txt_7 = (TextBox)grdNSICS.Rows[i].Cells[7].FindControl("txt_7");
                TextBox txt_8 = (TextBox)grdNSICS.Rows[i].Cells[8].FindControl("txt_8");
                TextBox txt_9 = (TextBox)grdNSICS.Rows[i].Cells[9].FindControl("txt_9");

                NSICSDtl += txt_NSICS.Text + "|" + txt_1.Text + "|" + txt_2.Text + "|" + txt_3.Text + "|" + txt_4.Text + "|" + txt_5.Text + "|" + txt_6.Text + "|" + txt_7.Text + "|" + txt_8.Text + "|" + txt_9.Text + "|";
            }
            for (int i = 0; i < grdEquipment.Rows.Count; i++)
            {
                TextBox txt_ConditionofSand = (TextBox)grdEquipment.Rows[i].Cells[0].FindControl("txt_ConditionofSand");
                TextBox txt_Watertobe = (TextBox)grdEquipment.Rows[i].Cells[1].FindControl("txt_Watertobe");

                EquipDtl += txt_ConditionofSand.Text + "|" + txt_Watertobe.Text + "|";

            }
            dc.MoistureCorrection_Update(txt_RefNo.Text, txt_Desc.Text, MoistureCorrDtl, NSICSDtl, EquipDtl, false);
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Text = "Records Saved Successfully";
            lblMsg.Visible = true;
            lblMsg.ForeColor = System.Drawing.Color.Green;
            lnkSave.Enabled = false;
        }

        protected void lnk_Exit_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
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


