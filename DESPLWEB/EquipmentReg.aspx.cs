using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Data.OleDb;

namespace DESPLWEB
{
    public partial class EquipmentReg : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        string strID = "";
        PrintPDFReport pdf = new PrintPDFReport();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Lab Equipment";
                AddRowCalibration();
                LoadEquipment();
            }

        }

        #region add/delete rows Calibration grid
        protected void AddRowCalibration()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CalibrationTable"] != null)
            {
                GetCurrentDataCalibration();
                dt = (DataTable)ViewState["CalibrationTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtDateoflastCalibration", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCalibrationDueon", typeof(string)));
                dt.Columns.Add(new DataColumn("txtCalibratingAgency", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtDateoflastCalibration"] = string.Empty;
            dr["txtCalibrationDueon"] = string.Empty;
            dr["txtCalibratingAgency"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CalibrationTable"] = dt;
            grdCal.DataSource = dt;
            grdCal.DataBind();
            SetPreviousDataCalibration();
        }
        protected void DeleteRowCalibration(int rowIndex)
        {
            GetCurrentDataCalibration();
            DataTable dt = ViewState["CalibrationTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CalibrationTable"] = dt;
            grdCal.DataSource = dt;
            grdCal.DataBind();
            SetPreviousDataCalibration();
        }
        protected void SetPreviousDataCalibration()
        {
            DataTable dt = (DataTable)ViewState["CalibrationTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtDateoflastCalibration = (TextBox)grdCal.Rows[i].FindControl("txtDateoflastCalibration");
                TextBox txtCalibrationDueon = (TextBox)grdCal.Rows[i].FindControl("txtCalibrationDueon");
                TextBox txtCalibratingAgency = (TextBox)grdCal.Rows[i].FindControl("txtCalibratingAgency");
                txtDateoflastCalibration.Text = dt.Rows[i]["txtDateoflastCalibration"].ToString();
                txtCalibrationDueon.Text = dt.Rows[i]["txtCalibrationDueon"].ToString();
                txtCalibratingAgency.Text = dt.Rows[i]["txtCalibratingAgency"].ToString();
            }
        }
        protected void GetCurrentDataCalibration()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow dr = null;

            dtTable.Columns.Add(new DataColumn("txtDateoflastCalibration", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtCalibrationDueon", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtCalibratingAgency", typeof(string)));

            for (int i = 0; i < grdCal.Rows.Count; i++)
            {
                TextBox txtDateoflastCalibration = (TextBox)grdCal.Rows[rowIndex].FindControl("txtDateoflastCalibration");
                TextBox txtCalibrationDueon = (TextBox)grdCal.Rows[rowIndex].FindControl("txtCalibrationDueon");
                TextBox txtCalibratingAgency = (TextBox)grdCal.Rows[rowIndex].FindControl("txtCalibratingAgency");
                dr = dtTable.NewRow();
                dr["txtDateoflastCalibration"] = txtDateoflastCalibration.Text;
                dr["txtCalibrationDueon"] = txtCalibrationDueon.Text;
                dr["txtCalibratingAgency"] = txtCalibratingAgency.Text;
                dtTable.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["CalibrationTable"] = dtTable;
        }
        protected void BtnAddRowCalibration_Click(object sender, EventArgs e)
        {
            AddRowCalibration();
        }
        protected void BtnDeleteRowCalibration_Click(object sender, EventArgs e)
        {
            if (grdCal.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowCalibration(gvr.RowIndex);
            }
        }
        #endregion

        #region AutoComplete textbox code
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetMakeList(string prefixText, int count)
        {
            LabDataDataContext dc1 = new LabDataDataContext();
            List<string> autoCompleteNames = new List<string>();
            var autocompleteText = dc1.Equipment_View_Auto(prefixText, "Make").ToList();
            foreach (var d in autocompleteText)
            {
                autoCompleteNames.Add(d.itemName);
            }
            return autoCompleteNames;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetShortNameList(string prefixText, int count)
        {
            LabDataDataContext dc1 = new LabDataDataContext();
            List<string> autoCompleteNames = new List<string>();
            var autocompleteText = dc1.Equipment_View_Auto(prefixText, "ShortName").ToList();
            foreach (var d in autocompleteText)
            {
                autoCompleteNames.Add(d.itemName);
            }
            return autoCompleteNames;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetSectionList(string prefixText, int count)
        {
            LabDataDataContext dc1 = new LabDataDataContext();
            List<string> autoCompleteNames = new List<string>();
            var autocompleteText = dc1.Equipment_View_Auto(prefixText, "Section").ToList();
            foreach (var d in autocompleteText)
            {
                autoCompleteNames.Add(d.itemName);
            }
            return autoCompleteNames;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]

        public static List<string> GetLeastCountList(string prefixText, int count)
        {
            LabDataDataContext dc1 = new LabDataDataContext();
            List<string> autoCompleteNames = new List<string>();
            var autocompleteText = dc1.Equipment_View_Auto(prefixText, "LeastCount").ToList();
            foreach (var d in autocompleteText)
            {
                autoCompleteNames.Add(d.itemName);
            }
            return autoCompleteNames;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetRangeList(string prefixText, int count)
        {
            LabDataDataContext dc1 = new LabDataDataContext();
            List<string> autoCompleteNames = new List<string>();
            var autocompleteText = dc1.Equipment_View_Auto(prefixText, "Range").ToList();
            foreach (var d in autocompleteText)
            {
                autoCompleteNames.Add(d.itemName);
            }
            return autoCompleteNames;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetCalibrationStatusList(string prefixText, int count)
        {
            LabDataDataContext dc1 = new LabDataDataContext();
            List<string> autoCompleteNames = new List<string>();
            var autocompleteText = dc1.Equipment_View_Auto(prefixText, "CalibStatus").ToList();
            foreach (var d in autocompleteText)
            {
                autoCompleteNames.Add(d.itemName);
            }
            return autoCompleteNames;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetCertificateList(string prefixText, int count)
        {
            LabDataDataContext dc1 = new LabDataDataContext();
            List<string> autoCompleteNames = new List<string>();
            var autocompleteText = dc1.Equipment_View_Auto(prefixText, "Certificate").ToList();
            foreach (var d in autocompleteText)
            {
                autoCompleteNames.Add(d.itemName);
            }
            return autoCompleteNames;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetCalibrationAgencyList(string prefixText, int count)
        {
            LabDataDataContext dc1 = new LabDataDataContext();
            List<string> autoCompleteNames = new List<string>();
            var autocompleteText = dc1.Equipment_View_Auto(prefixText, "Agency").ToList();
            foreach (var d in autocompleteText)
            {
                autoCompleteNames.Add(d.itemName);
            }
            return autoCompleteNames;
        }
        #endregion

        private void LoadEquipment()
        {
            var equipData = dc.Equipment_View(0).ToList();
            ddlEquipments.DataSource = equipData;
            ddlEquipments.DataTextField = "EQP_InternalIdMark_var";
            ddlEquipments.DataValueField = "EQP_Id";
            ddlEquipments.DataBind();
            ddlEquipments.Items.Insert(0, "- - - Add New - - -");
        }
        private void DisplayEquipment()
        {
            
            if (ddlEquipments.SelectedItem.Text == "- - - Add New - - -")
            {
                grdCal.DataSource = null;
                grdCal.DataBind();
                AddRowCalibration();
                txtEquipmentName.Text = "";
                txtSerialNo.Text = "";
                txtIDMark.Text = "";
                txtAutoMake.Text = "";
                txtAutoShortName.Text = "";
                txtAutoSection.Text = "";
                txtAutoLeastCount.Text = "";
                txtAutoRange.Text = "";
                txtAutoCalibration.Text = "";
                txtAutoCertificate.Text = "";
                txtRecdOn.Text = "";
                txtEquipmentName.ReadOnly = false;
                txtSerialNo.ReadOnly = false;
                txtIDMark.ReadOnly = false;
                txtAutoMake.ReadOnly = false;
                txtAutoShortName.ReadOnly = false;
                txtAutoSection.ReadOnly = false;
                txtAutoLeastCount.ReadOnly = false;
                txtAutoRange.ReadOnly = false;
                txtAutoCalibration.ReadOnly = false;
                txtAutoCertificate.ReadOnly = false;
                txtRecdOn.ReadOnly = false;
                CalendarExtender1.Enabled = true;
            }
            else if (ddlEquipments.SelectedItem.Text != "- - - Add New - - -")
            {
                txtEquipmentName.ReadOnly = true;
                txtSerialNo.ReadOnly = true;
                txtIDMark.ReadOnly = true;
                txtAutoMake.ReadOnly = true;
                txtAutoShortName.ReadOnly = true;
                txtAutoSection.ReadOnly = true;
                txtAutoLeastCount.ReadOnly = true;
                txtAutoRange.ReadOnly = true;
                txtAutoCalibration.ReadOnly = true;
                txtAutoCertificate.ReadOnly = true;
                txtRecdOn.ReadOnly = true;
                CalendarExtender1.Enabled = false;
                var eqipmentData = dc.Equipment_View(Convert.ToInt32(ddlEquipments.SelectedValue));
                foreach (var data in eqipmentData)
                {
                    txtEquipmentName.Text = data.EQP_Name_var;
                    txtSerialNo.Text = data.EQP_SerialNo_var;
                    txtIDMark.Text = data.EQP_InternalIdMark_var;
                    txtAutoMake.Text = data.EQP_Make_var;
                    txtAutoShortName.Text = data.EQP_ShortName_var;
                    txtAutoSection.Text = data.EQP_Section_var;
                    txtAutoLeastCount.Text = data.EQP_LeastCount_var;
                    txtAutoRange.Text = data.EQP_Range_var;
                    txtAutoCalibration.Text = data.EQP_CalibStatus_var;
                    txtAutoCertificate.Text = data.EQP_Certificate_var;
                    if (data.EQP_RecdOnDate_dt == null)
                        txtRecdOn.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtRecdOn.Text = Convert.ToDateTime(data.EQP_RecdOnDate_dt).ToString("dd/MM/yyyy");
                }


                var euipmentDetailsData = dc.EquipmentDetail_View(ddlEquipments.SelectedItem.Text, 0).ToList();
                grdCal.DataSource = null;
                grdCal.DataBind();
                AddRowCalibration();
                int rowNo = 0;
                foreach (var d in euipmentDetailsData)
                {
                    TextBox txtDateoflastCalibration = (TextBox)grdCal.Rows[rowNo].FindControl("txtDateoflastCalibration");
                    TextBox txtCalibrationDueon = (TextBox)grdCal.Rows[rowNo].FindControl("txtCalibrationDueon");
                    TextBox txtCalibratingAgency = (TextBox)grdCal.Rows[rowNo].FindControl("txtCalibratingAgency");

                    if (d.EQPD_LastCalibDate_dt == null)
                        txtDateoflastCalibration.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtDateoflastCalibration.Text = Convert.ToDateTime(d.EQPD_LastCalibDate_dt).ToString("dd/MM/yyyy");

                    if (d.EQPD_CalibDueOnDate_dt == null)
                        txtCalibrationDueon.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    else
                        txtCalibrationDueon.Text = Convert.ToDateTime(d.EQPD_CalibDueOnDate_dt).ToString("dd/MM/yyyy");

                    txtCalibratingAgency.Text = d.EQPD_Agency_var;
                    if (rowNo < euipmentDetailsData.Count() - 1)
                        AddRowCalibration();
                    rowNo++;
                }
            }

            //Session["EqupID"] = ddlEquipments.SelectedValue;
            //Session["IDMarkVar"] = txtIDMark.Text;
        }

        protected void ddlEquipments_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayEquipment();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                #region Equipment
                var equipmentData = dc.Equipment_View(0).ToList();
                DateTime RecdOnDate = new DateTime();
                RecdOnDate = DateTime.ParseExact(txtRecdOn.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (equipmentData.Count() == 0)
                {
                    string[] recdYear = txtRecdOn.Text.Split('/');
                    string equipIdMark = "Duro/Equip/";
                    //recdYear[2] = recdYear[2].Substring(2);
                    equipIdMark += txtAutoSection.Text + "/" + txtAutoShortName.Text + "/" + "001" + "/" + recdYear[2];
                    dc.Equipment_Update(0, equipIdMark, txtEquipmentName.Text, txtAutoShortName.Text, txtAutoSection.Text, txtAutoCalibration.Text, txtSerialNo.Text, txtAutoMake.Text, txtAutoCertificate.Text, txtAutoLeastCount.Text, RecdOnDate, txtAutoRange.Text,true);
                    txtIDMark.Text = equipIdMark;
                    LoadEquipment();
                    ddlEquipments.SelectedIndex = ddlEquipments.Items.Count - 1;
                }
                else
                {
                    if (ddlEquipments.SelectedItem.Text == "- - - Add New - - -")
                    {
                        string[] recdYear = txtRecdOn.Text.Split('/');
                        string equipIdMark = "Duro/Equip/";
                        //recdYear[2] = recdYear[2].Substring(2);
                        int count = equipmentData.Count() + 1;
                        equipIdMark += txtAutoSection.Text + "/" + txtAutoShortName.Text + "/" + count.ToString("000") + "/" + recdYear[2];
                        dc.Equipment_Update(0, equipIdMark, txtEquipmentName.Text, txtAutoShortName.Text, txtAutoSection.Text, txtAutoCalibration.Text, txtSerialNo.Text, txtAutoMake.Text, txtAutoCertificate.Text, txtAutoLeastCount.Text, RecdOnDate, txtAutoRange.Text,true);
                        txtIDMark.Text = equipIdMark;
                        LoadEquipment();
                        ddlEquipments.SelectedIndex = ddlEquipments.Items.Count - 1;
                    }
                    else
                    {
                        dc.Equipment_Update(Convert.ToInt32(ddlEquipments.SelectedValue), ddlEquipments.SelectedItem.Text, txtEquipmentName.Text, txtAutoShortName.Text, txtAutoSection.Text, txtAutoCalibration.Text, txtSerialNo.Text, txtAutoMake.Text, txtAutoCertificate.Text, txtAutoLeastCount.Text, RecdOnDate, txtAutoRange.Text,false);
                    }
                }
                #endregion

                #region Equipment Details
                for (int i = 0; i < grdCal.Rows.Count; i++)
                {
                    TextBox txtSrNo = (TextBox)grdCal.Rows[i].FindControl("txtSrNo");
                    TextBox txtDateoflastCalibration = (TextBox)grdCal.Rows[i].FindControl("txtDateoflastCalibration");
                    TextBox txtCalibrationDueon = (TextBox)grdCal.Rows[i].FindControl("txtCalibrationDueon");
                    TextBox txtCalibratingAgency = (TextBox)grdCal.Rows[i].FindControl("txtCalibratingAgency");

                    DateTime DateoflastCalibration = new DateTime();
                    DateoflastCalibration = DateTime.ParseExact(txtDateoflastCalibration.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    DateTime CalibrationDueon = new DateTime();
                    CalibrationDueon = DateTime.ParseExact(txtCalibrationDueon.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var checkData = dc.EquipmentDetail_View(txtIDMark.Text, 0);
                    bool flag = false;
                    foreach (var d in checkData)
                    {
                        if (d.EQPD_InternalIdMark_var == txtIDMark.Text && d.EQPD_SrNo_int == Convert.ToInt32(txtSrNo.Text))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag == true)
                    {
                        dc.EquipmentDetail_Update(txtIDMark.Text, Convert.ToInt32(txtSrNo.Text), DateoflastCalibration, CalibrationDueon, txtCalibratingAgency.Text, 0, true);
                    }
                    else
                    {
                        dc.EquipmentDetail_Update(txtIDMark.Text, Convert.ToInt32(txtSrNo.Text), DateoflastCalibration, CalibrationDueon, txtCalibratingAgency.Text, 0, false);
                    }
                }

                #endregion

                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Report Saved Successfully');", true);
            }
        }
        protected void chkViewAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkViewAll.Checked == true)
            {
                pnlEquipmentDetails.Visible = true;
                PnlEquipment.Visible = false;
                pnlCalibration.Visible = false;
                pnlButton.Visible = false;
                LoadSection();
                LoadCalibrationStatus();
                LoadMake();
                LoadCertificate();
                AddRowEquipDetails();

            }
            else
            {
                pnlEquipmentDetails.Visible = false;
                PnlEquipment.Visible = true;
                pnlCalibration.Visible = true;
                pnlButton.Visible = true;
            }
        }


        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {

            CheckBox theCheckBox = sender as CheckBox;        

            var rowIndex = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            // Was the check box found?
            if (theCheckBox.Checked == true)
            {
                Label lblId = (Label)grdEquipmentDetails.Rows[rowIndex].FindControl("lblId");
               strID += lblId.Text + "|";
            }
        }
        private void LoadSection()
        {
            var equipmentDet = dc.Equipment_View_List("Section");
            ddlEquSection.DataSource = equipmentDet;
            ddlEquSection.DataTextField = "itemName";
            ddlEquSection.DataBind();
            ddlEquSection.Items.Insert(0, "All");
        }

        private void LoadCalibrationStatus()
        {
            var equipmentDet = dc.Equipment_View_List("CalibStatus");
            ddlEquCalibrationStatus.DataSource = equipmentDet;
            ddlEquCalibrationStatus.DataTextField = "itemName";
            ddlEquCalibrationStatus.DataBind();
            ddlEquCalibrationStatus.Items.Insert(0, "All");
        }

        private void LoadMake()
        {
            var equipmentDet = dc.Equipment_View_List("Make");
            ddlEquMake.DataSource = equipmentDet;
            ddlEquMake.DataTextField = "itemName";
            ddlEquMake.DataBind();
            ddlEquMake.Items.Insert(0, "All");
        }

        private void LoadCertificate()
        {
            var equipmentDet = dc.Equipment_View_List("Certificate");
            ddlEquCertificate.DataSource = equipmentDet;
            ddlEquCertificate.DataTextField = "itemName";
            ddlEquCertificate.DataBind();
            ddlEquCertificate.Items.Insert(0, "All");
        }

        protected void ddlEquSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            EquipmentFilteration();
        }
        protected void ddlEquCalibrationStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            EquipmentFilteration();
        }
        protected void ddlEquMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            EquipmentFilteration();
        }
        protected void ddlEquCertificate_SelectedIndexChanged(object sender, EventArgs e)
        {
            EquipmentFilteration();
        }

        protected void lnkSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grdEquipmentDetails.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdEquipmentDetails.Rows[i].FindControl("chkSelect");
                chkSelect.Checked = true;
            }
        }
        protected void lnkDeSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grdEquipmentDetails.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdEquipmentDetails.Rows[i].FindControl("chkSelect");
                chkSelect.Checked = false;
            }
        }
      

        protected Boolean ValidateData()
        {
            string dispalyMsg = "";
            Boolean valid = true;
            #region validate equipment data
            if (pnlCalibration.Visible == true)
            {
                if (txtEquipmentName.Text == "")
                {
                    dispalyMsg = "Enter Equipment Name.";
                    txtEquipmentName.Focus();
                    valid = false;                    
                }
                else if (txtSerialNo.Text == "")
                {
                    dispalyMsg = "Enter Serial No.";
                    txtSerialNo.Focus();
                    valid = false;
                }
                else if (txtAutoMake.Text == "")
                {
                    dispalyMsg = "Enter Make.";
                    txtAutoMake.Focus();
                    valid = false;
                }
                else if (txtAutoShortName.Text == "")
                {
                    dispalyMsg = "Enter Short Name.";
                    txtAutoShortName.Focus();
                    valid = false;
                }
                else if (txtAutoSection.Text == "")
                {
                    dispalyMsg = "Enter Section.";
                    txtAutoSection.Focus();
                    valid = false;
                }
                else if (txtAutoLeastCount.Text == "")
                {
                    dispalyMsg = "Enter Least Count.";
                    txtAutoLeastCount.Focus();
                    valid = false;
                }
                else if (txtAutoRange.Text == "")
                {
                    dispalyMsg = "Enter Range.";
                    txtAutoRange.Focus();
                    valid = false;
                }
                else if (txtRecdOn.Text == "")
                {
                    dispalyMsg = "Enter Recd On.";
                    txtRecdOn.Focus();
                    valid = false;
                }
                else if (txtAutoCalibration.Text == "")
                {
                    dispalyMsg = "Enter Calibration Status.";
                    txtAutoCalibration.Focus();
                    valid = false;
                }
                else if (txtAutoCertificate.Text == "")
                {
                    dispalyMsg = "Enter Certificate.";
                    txtAutoCertificate.Focus();
                    valid = false;
                }
                if (valid == true)
                {
                    for (int i = 0; i < grdCal.Rows.Count; i++)
                    {
                        TextBox txtDateoflastCalibration = (TextBox)grdCal.Rows[i].FindControl("txtDateoflastCalibration");
                        TextBox txtCalibrationDueon = (TextBox)grdCal.Rows[i].FindControl("txtCalibrationDueon");
                        TextBox txtCalibratingAgency = (TextBox)grdCal.Rows[i].FindControl("txtCalibratingAgency");

                        string[] lastCalibArray = txtDateoflastCalibration.Text.Split('/');
                        string[] CalibrationDueArray = txtCalibrationDueon.Text.Split('/');

                        txtDateoflastCalibration.Text = txtDateoflastCalibration.Text.Trim();
                        txtCalibrationDueon.Text = txtCalibrationDueon.Text.Trim();
                        txtCalibratingAgency.Text = txtCalibratingAgency.Text.Trim();
                        if (txtDateoflastCalibration.Text == "")
                        {
                            dispalyMsg = "Enter Date of last Calibaration for Sr No. " + (i + 1) + ".";
                            txtDateoflastCalibration.Focus();
                            valid = false;
                            break;
                        }
                        if (txtCalibrationDueon.Text == "")
                        {
                            dispalyMsg = "Enter Calibration due on date for Sr No. " + (i + 1) + ".";
                            txtCalibrationDueon.Focus();
                            valid = false;
                            break;
                        }
                        if (txtCalibratingAgency.Text == "")
                        {
                            dispalyMsg = "Enter Calibration agency for Sr No. " + (i + 1) + ".";
                            txtCalibratingAgency.Focus();
                            valid = false;
                            break;
                        }
                        if (Convert.ToInt32(lastCalibArray[2]) > Convert.ToInt32(CalibrationDueArray[2]))
                        {
                            dispalyMsg = "Calibration due on must be greater than Date of last Calibaration for Sr No. " + (i + 1) + ".";
                            valid = false;
                            break;

                        }
                        else if (Convert.ToInt32(lastCalibArray[2]) == Convert.ToInt32(CalibrationDueArray[2]))
                        {
                            if (Convert.ToInt32(lastCalibArray[1]) > Convert.ToInt32(CalibrationDueArray[1]))
                            {
                                dispalyMsg = "Calibration due on must be greater than Date of last Calibaration for Sr No. " + (i + 1) + ".";
                                valid = false;
                                break;
                            }
                            else if (Convert.ToInt32(lastCalibArray[1]) == Convert.ToInt32(CalibrationDueArray[1]))
                            {
                                if (Convert.ToInt32(lastCalibArray[0]) > Convert.ToInt32(CalibrationDueArray[0]))
                                {
                                    dispalyMsg = "Calibration due on must be greater than Date of last Calibaration for Sr No. " + (i + 1) + ".";
                                    valid = false;
                                    break;
                                }
                            }
                        }
                        
                        if (i != 0)
                        {
                            TextBox txtCalibrationDueonPriv = (TextBox)grdCal.Rows[i - 1].FindControl("txtCalibrationDueon");
                            string[] CalibrationDuePrevious = txtCalibrationDueonPriv.Text.Split('/');
                            if (Convert.ToInt32(CalibrationDuePrevious[2]) > Convert.ToInt32(lastCalibArray[2]))
                            {
                                dispalyMsg = "Date of last Calibration of Sr No. " + (i + 1) + " must be greater than Calibration due on of Sr No. " + (i);
                                valid = false;
                                break;
                            }
                            else if (Convert.ToInt32(CalibrationDuePrevious[2]) == Convert.ToInt32(lastCalibArray[2]))
                            {
                                if (Convert.ToInt32(CalibrationDuePrevious[1]) > Convert.ToInt32(lastCalibArray[1]))
                                {
                                    dispalyMsg = "Date of last Calibration of Sr No. " + (i + 1) + " must be greater than Calibration due on of Sr No. " + (i);
                                    valid = false;
                                    break;
                                }
                                else if (Convert.ToInt32(CalibrationDuePrevious[1]) == Convert.ToInt32(lastCalibArray[1]))
                                {
                                    if (Convert.ToInt32(CalibrationDuePrevious[0]) > Convert.ToInt32(lastCalibArray[0]))
                                    {
                                        dispalyMsg = "Date of last Calibration of Sr No. " + (i + 1) + " must be greater than Calibration due on of Sr No. " + (i);
                                        valid = false;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            #endregion

            if (valid == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
            return valid;
        }

        private void EquipmentFilteration()
        {
            for (int i = 0; i < grdEquipmentDetails.Rows.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)grdEquipmentDetails.Rows[i].FindControl("chkSelect");
                TextBox txtSrNo = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtSrNo");
                TextBox txtEquipment = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtEquipment");
                TextBox txtSection = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtSection");
                TextBox txtInternalIdMark = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtInternalIdMark");
                TextBox txtCalibrationStatus = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtCalibrationStatus");
                TextBox txtSerialNo = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtSerialNo");
                TextBox txtMake = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtMake");
                TextBox txtCertificate = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtCertificate");
                TextBox txtLeastCount = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtLeastCount");
                TextBox txtRecdOn = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtRecdOn");
                TextBox txtRange = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtRange");
                TextBox txtStatus = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtStatus");

                chkSelect.Visible = true;
                txtSrNo.Visible = true;
                txtEquipment.Visible = true;
                txtSection.Visible = true;
                txtInternalIdMark.Visible = true;
                txtCalibrationStatus.Visible = true;
                txtSerialNo.Visible = true;
                txtMake.Visible = true;
                txtCertificate.Visible = true;
                txtLeastCount.Visible = true;
                txtRecdOn.Visible = true;
                txtRange.Visible = true;
                txtStatus.Visible = true;
                bool showRow = true;
                if (ddlEquSection.SelectedItem.Text != "All")
                {
                    if (txtSection.Text != ddlEquSection.SelectedItem.Text)
                    {
                        //DeleteRow(i);                        
                        //i = i - 1;
                        showRow = false;
                    }
                }
                if (ddlEquCalibrationStatus.SelectedItem.Text != "All")
                {
                    if (txtCalibrationStatus.Text != ddlEquCalibrationStatus.SelectedItem.Text)
                    {
                        //DeleteRow(i);
                        //i = i - 1;
                        showRow = false;
                    }
                }
                if (ddlEquMake.SelectedItem.Text != "All")
                {
                    if (txtMake.Text != ddlEquMake.SelectedItem.Text)
                    {
                        //DeleteRow(i);
                        //i = i - 1;
                        showRow = false;
                    }
                }
                if (ddlEquCertificate.SelectedItem.Text != "All")
                {
                    if (txtCertificate.Text != ddlEquCertificate.SelectedItem.Text)
                    {
                        //DeleteRow(i);
                        //i = i - 1;
                        showRow = false;
                    }
                }
                if (showRow == false)
                {
                    //grdEquipmentDetails.Rows[i].Height = 0;
                    chkSelect.Visible = false;
                    txtSrNo.Visible = false;
                    txtEquipment.Visible = false;
                    txtSection.Visible = false;
                    txtInternalIdMark.Visible = false;
                    txtCalibrationStatus.Visible = false;
                    txtSerialNo.Visible = false;
                    txtMake.Visible = false;
                    txtCertificate.Visible = false;
                    txtLeastCount.Visible = false;
                    txtRecdOn.Visible = false;
                    txtRange.Visible = false;
                     txtStatus.Visible = false;
                }
            }
        }

        private void AddRowEquipDetails()
        {
            var equipmentDet = dc.Equipment_View(0).ToList();           
            DataTable dt = new DataTable();
            DataColumn dtcolumn = new DataColumn();

            for (int i = 0; i < equipmentDet.Count(); i++)
            {
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("txtSrNo", typeof(string));
                    dt.Columns.Add("lblId", typeof(string));
                    dt.Columns.Add("txtEquipment", typeof(string));
                    dt.Columns.Add("txtSection", typeof(string));
                    dt.Columns.Add("txtInternalIdMark", typeof(string));
                    dt.Columns.Add("txtCalibrationStatus", typeof(string));
                    dt.Columns.Add("txtSerialNo", typeof(string));
                    dt.Columns.Add("txtMake", typeof(string));
                    dt.Columns.Add("txtCertificate", typeof(string));
                    dt.Columns.Add("txtLeastCount", typeof(string));
                    dt.Columns.Add("txtRecdOn", typeof(string));
                    dt.Columns.Add("txtRange", typeof(string));
                    dt.Columns.Add("txtStatus", typeof(string));
                }
                DataRow NewRow = dt.NewRow();
                dt.Rows.Add(NewRow);
                grdEquipmentDetails.DataSource = dt;
                grdEquipmentDetails.DataBind();
            }
            int rowNo = 0;
            foreach (var grdEquipDetl in equipmentDet)
            {
                TextBox txtSrNo = (TextBox)grdEquipmentDetails.Rows[rowNo].FindControl("txtSrNo");
                Label lblId = (Label)grdEquipmentDetails.Rows[rowNo].FindControl("lblId");
                TextBox txtEquipment = (TextBox)grdEquipmentDetails.Rows[rowNo].FindControl("txtEquipment");
                TextBox txtSection = (TextBox)grdEquipmentDetails.Rows[rowNo].FindControl("txtSection");
                TextBox txtInternalIdMark = (TextBox)grdEquipmentDetails.Rows[rowNo].FindControl("txtInternalIdMark");
                TextBox txtCalibrationStatus = (TextBox)grdEquipmentDetails.Rows[rowNo].FindControl("txtCalibrationStatus");
                TextBox txtSerialNo = (TextBox)grdEquipmentDetails.Rows[rowNo].FindControl("txtSerialNo");
                TextBox txtMake = (TextBox)grdEquipmentDetails.Rows[rowNo].FindControl("txtMake");
                TextBox txtCertificate = (TextBox)grdEquipmentDetails.Rows[rowNo].FindControl("txtCertificate");
                TextBox txtLeastCount = (TextBox)grdEquipmentDetails.Rows[rowNo].FindControl("txtLeastCount");
                TextBox txtRecdOn = (TextBox)grdEquipmentDetails.Rows[rowNo].FindControl("txtRecdOn");
                TextBox txtRange = (TextBox)grdEquipmentDetails.Rows[rowNo].FindControl("txtRange");
                TextBox txtStatus = (TextBox)grdEquipmentDetails.Rows[rowNo].FindControl("txtStatus");
                txtSrNo.Text = Convert.ToString(rowNo + 1);
                lblId.Text = grdEquipDetl.EQP_Id.ToString();              
                txtEquipment.Text = grdEquipDetl.EQP_Name_var;
                txtSection.Text = grdEquipDetl.EQP_Section_var;
                txtInternalIdMark.Text = grdEquipDetl.EQP_InternalIdMark_var;
                txtCalibrationStatus.Text = grdEquipDetl.EQP_CalibStatus_var;
                txtSerialNo.Text = grdEquipDetl.EQP_SerialNo_var;
                txtMake.Text = grdEquipDetl.EQP_Make_var;
                txtCertificate.Text = grdEquipDetl.EQP_Certificate_var;
                txtLeastCount.Text = grdEquipDetl.EQP_LeastCount_var;
                txtRange.Text = grdEquipDetl.EQP_Range_var;
                //txtStatus.Text = Convert.ToString(grdEquipDetl.EQP_Status_bit);
                if (Convert.ToInt32(grdEquipDetl.EQP_Status_bit) == 1)
                    txtStatus.Text = "Active";
                else
                    txtStatus.Text = "Deactive";
               

                if (grdEquipDetl.EQP_RecdOnDate_dt == null)
                    txtRecdOn.Text = DateTime.Now.ToString("dd/MM/yyyy");
                else
                    txtRecdOn.Text = Convert.ToDateTime(grdEquipDetl.EQP_RecdOnDate_dt).ToString("dd/MM/yyyy");
                rowNo++;
            }
        }

        protected void DeleteRow(int rowIndex)
        {
            GetCurrentData();
            DataTable dt = ViewState["FilterTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["FilterTable"] = dt;
            grdEquipmentDetails.DataSource = dt;
            grdEquipmentDetails.DataBind();
            SetPreviousData();
        }

        protected void GetCurrentData()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow dr = null;

            dtTable.Columns.Add(new DataColumn("lblId", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtEquipment", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtSection", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtInternalIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtCalibrationStatus", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtSerialNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtMake", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtCertificate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtLeastCount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtRecdOn", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtRange", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtStatus", typeof(string)));

            for (int i = 0; i < grdEquipmentDetails.Rows.Count; i++)
            {
                Label lblId = (Label)grdEquipmentDetails.Rows[i].FindControl("lblId");
                TextBox txtEquipment = (TextBox)grdEquipmentDetails.Rows[rowIndex].FindControl("txtEquipment");
                TextBox txtSection = (TextBox)grdEquipmentDetails.Rows[rowIndex].FindControl("txtSection");
                TextBox txtInternalIdMark = (TextBox)grdEquipmentDetails.Rows[rowIndex].FindControl("txtInternalIdMark");
                TextBox txtCalibrationStatus = (TextBox)grdEquipmentDetails.Rows[rowIndex].FindControl("txtCalibrationStatus");
                TextBox txtSerialNo = (TextBox)grdEquipmentDetails.Rows[rowIndex].FindControl("txtSerialNo");
                TextBox txtMake = (TextBox)grdEquipmentDetails.Rows[rowIndex].FindControl("txtMake");
                TextBox txtCertificate = (TextBox)grdEquipmentDetails.Rows[rowIndex].FindControl("txtCertificate");
                TextBox txtLeastCount = (TextBox)grdEquipmentDetails.Rows[rowIndex].FindControl("txtLeastCount");
                TextBox txtRecdOn = (TextBox)grdEquipmentDetails.Rows[rowIndex].FindControl("txtRecdOn");
                TextBox txtRange = (TextBox)grdEquipmentDetails.Rows[rowIndex].FindControl("txtRange");
                TextBox txtStatus = (TextBox)grdEquipmentDetails.Rows[rowIndex].FindControl("txtStatus");

                dr = dtTable.NewRow();
                dr["lblId"] = lblId.Text;
                dr["txtEquipment"] = txtEquipment.Text;
                dr["txtSection"] = txtSection.Text;
                dr["txtInternalIdMark"] = txtInternalIdMark.Text;
                dr["txtCalibrationStatus"] = txtCalibrationStatus.Text;
                dr["txtSerialNo"] = txtSerialNo.Text;
                dr["txtMake"] = txtMake.Text;
                dr["txtCertificate"] = txtCertificate.Text;
                dr["txtLeastCount"] = txtLeastCount.Text;
                dr["txtRecdOn"] = txtRecdOn.Text;
                dr["txtRange"] = txtRange.Text;
                dr["txtStatus"] = txtStatus.Text;
                dtTable.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["FilterTable"] = dtTable;
        }

        protected void SetPreviousData()
        {
            DataTable dt = (DataTable)ViewState["FilterTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblId = (Label)grdEquipmentDetails.Rows[i].FindControl("lblId");
                TextBox txtEquipment = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtEquipment");
                TextBox txtSection = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtSection");
                TextBox txtInternalIdMark = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtInternalIdMark");
                TextBox txtCalibrationStatus = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtCalibrationStatus");
                TextBox txtSerialNo = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtSerialNo");
                TextBox txtMake = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtMake");
                TextBox txtCertificate = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtCertificate");
                TextBox txtLeastCount = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtLeastCount");
                TextBox txtRecdOn = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtRecdOn");
                TextBox txtRange = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtRange");
                TextBox txtStatus = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtStatus");

                lblId.Text = dt.Rows[i]["lblId"].ToString();
                txtEquipment.Text = dt.Rows[i]["txtEquipment"].ToString();
                txtSection.Text = dt.Rows[i]["txtSection"].ToString();
                txtInternalIdMark.Text = dt.Rows[i]["txtInternalIdMark"].ToString();
                txtCalibrationStatus.Text = dt.Rows[i]["txtCalibrationStatus"].ToString();
                txtSerialNo.Text = dt.Rows[i]["txtSerialNo"].ToString();
                txtMake.Text = dt.Rows[i]["txtMake"].ToString();
                txtCertificate.Text = dt.Rows[i]["txtCertificate"].ToString();
                txtLeastCount.Text = dt.Rows[i]["txtLeastCount"].ToString();
                txtRecdOn.Text = dt.Rows[i]["txtRecdOn"].ToString();
                txtRange.Text = dt.Rows[i]["txtRange"].ToString();
                txtStatus.Text = dt.Rows[i]["txtStatus"].ToString();
            }
        }

        protected void lnkExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void lnkPrintLabel_Click(object sender, EventArgs e)
        {
          //old logic 
           // string selEquip = ",";
           // for (int i = 0; i < grdEquipmentDetails.Rows.Count; i++)
           // {
           //     CheckBox chkSelect = (CheckBox)grdEquipmentDetails.Rows[i].FindControl("chkSelect");
           //     TextBox txtInternalIdMark = (TextBox)grdEquipmentDetails.Rows[i].FindControl("txtInternalIdMark");
           //     if (chkSelect.Checked == true)
           //         selEquip = selEquip + txtInternalIdMark.Text + ",";
           // }
           //// NewWindows.Redirect("FrmLabEquipmentPDFReport.aspx?Print=1&equipid=" + selEquip, "_blank", "menubar=0,width=700, height=550");
             // pdf.LabEquipmentLabel_PDFReport(ddlEquipments.SelectedValue, txtIDMark.Text, "1");

            //new Logic 10/01/2018
            if (grdEquipmentDetails.Rows.Count > 0)
            {
                var fileName = "LabEquipmentList" + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss");
                PrintGrid.EquipmentPrintGridView(grdEquipmentDetails, "Lab Equipment List", fileName);
            }
        }

        protected void lnkPrintEquipList_Click(object sender, EventArgs e)
        {
          //  NewWindows.Redirect("FrmLabEquipmentPDFReport.aspx?Print=2", "_blank", "menubar=0,width=700, height=550");
            pdf.LabEquipmentLabel_PDFReport(ddlEquipments.SelectedValue, txtIDMark.Text, "2");
        }

        protected void lnkPrintEquip_Click(object sender, EventArgs e)
        {
           // NewWindows.Redirect("FrmLabEquipmentPDFReport.aspx?Print=5", "_blank", "menubar=0,width=700, height=550");
            pdf.LabEquipmentLabel_PDFReport(ddlEquipments.SelectedValue, txtIDMark.Text, "5");
        }

        protected void lnlCalibExpired_Click(object sender, EventArgs e)
        {
          //  NewWindows.Redirect("FrmLabEquipmentPDFReport.aspx?Print=4", "_blank", "menubar=0,width=700, height=550");
            pdf.LabEquipmentLabel_PDFReport(ddlEquipments.SelectedValue, txtIDMark.Text, "4");
        }

        protected void lnkCalibDueList_Click(object sender, EventArgs e)
        {
           // NewWindows.Redirect("FrmLabEquipmentPDFReport.aspx?Print=3", "_blank", "menubar=0,width=700, height=550");
            pdf.LabEquipmentLabel_PDFReport(ddlEquipments.SelectedValue, txtIDMark.Text, "3");
        }

        protected void lnkExportExcel_Click(object sender, EventArgs e)
        {
            //string FilePath = ConfigurationManager.AppSettings["FilePath"].ToString();
            string filename = string.Empty;
            if (FileUpload1.HasFile)
            {

                //Get file name of selected file
                filename = Path.GetFileName(Server.MapPath(FileUpload1.FileName));
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);

                //Save selected file into server location
                FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Images/") + filename);
                //FileUpload1.SaveAs(Server.MapPath(FilePath) + filename);
                string filePath = Server.MapPath("~/Images/") + filename;


                OleDbConnection con = null;
                if (Extension == ".xls")
                    con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;");
                else if (Extension == ".xlsx")
                    con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;");

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

                      clsData obj = new clsData();
                for (int i = 1; i < ExcelDataSet.Tables[0].Rows.Count; i++)
                {
                           obj.Equipment_Insert(ExcelDataSet.Tables[0].Rows[i][3].ToString(), ExcelDataSet.Tables[0].Rows[i][1].ToString(), ExcelDataSet.Tables[0].Rows[i][11].ToString(), ExcelDataSet.Tables[0].Rows[i][2].ToString(), ExcelDataSet.Tables[0].Rows[i][4].ToString(), ExcelDataSet.Tables[0].Rows[i][0].ToString(), ExcelDataSet.Tables[0].Rows[i][6].ToString(), ExcelDataSet.Tables[0].Rows[i][7].ToString(), ExcelDataSet.Tables[0].Rows[i][8].ToString(), ExcelDataSet.Tables[0].Rows[i][9].ToString(), ExcelDataSet.Tables[0].Rows[i][10].ToString(), 1, ExcelDataSet.Tables[0].Rows[i][12].ToString(), "");
                }
            }
        }
    }
}