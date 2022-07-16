using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace DESPLWEB
{
    public partial class GeneralSettings : System.Web.UI.Page
    {

        LabDataDataContext dc = new LabDataDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "Soil Settings";
                bool userRight = false;
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                else if (Session["LoginId"].ToString() == "99")
                {
                    userRight = true;
                }
                else
                {
                    var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                    foreach (var u in user)
                    {
                        if (u.USER_Admin_right_bit == true)
                        {
                            userRight = true;
                        }
                    }
                }
                if (userRight == false)
                {
                    pnlContent.Visible = false;
                    lblAccess.Visible = true;
                    lblAccess.Text = "Access is Denied.. ";
                }
                else
                {
                    RdbGenericList.SelectedValue = "Soil Settings";
                    if (RdbGenericList.SelectedValue == "Soil Settings")
                    {
                        pnlSoilSettings.Visible = true;
                        pnlMachineSettings.Visible = false;
                        PanelWeightOfContainer.Visible = true;
                        lnkCalculate.Visible = false;
                        RadioButtonList1.SelectedValue = "Weight Of Container";
                    }
                    else if (RdbGenericList.SelectedValue == "Machine Settings")
                    {
                        pnlMachineSettings.Visible = true;
                        pnlSoilSettings.Visible = false;
                        PanelWeightOfContainer.Visible = false;
                    }
                    else
                    {
                        pnlSoilSettings.Visible = false;
                        pnlMachineSettings.Visible = false;
                        PanelWeightOfContainer.Visible = false;
                    }

                    //dateparsing
                    try
                    {
                        txt_From_Date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        // string[] arrayFor = txt_From_Date.Text.Split('/');
                        // txt_From_Date.Text = arrayFor[1] + "/" + arrayFor[0] + "/" + arrayFor[2];
                    }
                    catch
                    { }

                    getWeightOfContainerData("Weight Of Container");
                }
            }

        }
        #region Events
        //radio button events
        protected void RdbGenericList_OnTextChanged(object sender, EventArgs e)
        {
            if (RdbGenericList.SelectedValue == "Soil Settings")
            {
                pnlSoilSettings.Visible = true;
                RadioButtonList1.SelectedValue = "Weight Of Container";
                PanelWeightOfContainer.Visible = true;

                PanelMouldVolume.Visible = false;
                OptionsPanel.Visible = false;
                PanelCBRProviding.Visible = false;
                OptionsCBRProviding.Visible = false;
                OptionCalibration.Visible = false;
                PanelCalibrationSand.Visible = false;
                PanelCoreCutter.Visible = false;
                PanelWeightOfEmptyDish.Visible = false;
                PanelWeightOfEmptyBottle.Visible = false;
                PanelWeightOfEmptyPycnometer.Visible = false;
                getWeightOfContainerData(RadioButtonList1.SelectedValue.ToString());

                pnlMachineSettings.Visible = false;
            }
            else if (RdbGenericList.SelectedValue == "Machine Settings")
            {
                pnlMachineSettings.Visible = true;
                PanelWeightOfContainer.Visible = false;
                pnlSoilSettings.Visible = false;
                getMachineSettingsData();
            }
        }
        protected void RadioButtonList1_OnTextChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedValue == "Weight Of Container")
            {
                lnkCalculate.Visible = false;
                PanelWeightOfContainer.Visible = true;
                PanelMouldVolume.Visible = false;
                OptionsPanel.Visible = false;
                PanelCBRProviding.Visible = false;
                OptionsCBRProviding.Visible = false;
                OptionCalibration.Visible = false;
                PanelCalibrationSand.Visible = false;
                PanelCoreCutter.Visible = false;
                PanelWeightOfEmptyDish.Visible = false;
                PanelWeightOfEmptyBottle.Visible = false;
                PanelWeightOfEmptyPycnometer.Visible = false;
                getWeightOfContainerData(RadioButtonList1.SelectedValue.ToString());
            }
            else if (RadioButtonList1.SelectedValue == "Mould Volume")
            {
                lnkCalculate.Visible = false;
                PanelMouldVolume.Visible = true;
                OptionsPanel.Visible = true;
                PanelWeightOfContainer.Visible = false;
                PanelCBRProviding.Visible = false;
                OptionsCBRProviding.Visible = false;
                PanelCoreCutter.Visible = false;
                OptionCalibration.Visible = false;
                PanelCalibrationSand.Visible = false;
                PanelWeightOfEmptyDish.Visible = false;
                PanelWeightOfEmptyBottle.Visible = false;
                PanelWeightOfEmptyPycnometer.Visible = false;
                getMouldVolumeData("MDD", "Standard");
            }
            else if (RadioButtonList1.SelectedValue == "CBR Proving Ring")
            {
                lnkCalculate.Visible = false;
                PanelMouldVolume.Visible = false;
                OptionsPanel.Visible = false;
                PanelWeightOfContainer.Visible = false;
                PanelCoreCutter.Visible = false;
                OptionCalibration.Visible = false;
                PanelCalibrationSand.Visible = false;
                PanelCBRProviding.Visible = true;
                OptionsCBRProviding.Visible = true;
                PanelWeightOfEmptyDish.Visible = false;
                PanelWeightOfEmptyBottle.Visible = false;
                PanelWeightOfEmptyPycnometer.Visible = false;
                getCBRProvidingData();
            }
            else if (RadioButtonList1.SelectedValue == "Core Cutter Dimensions")
            {
                lnkCalculate.Visible = true;
                PanelMouldVolume.Visible = false;
                OptionsPanel.Visible = false;
                PanelWeightOfContainer.Visible = false;
                PanelCBRProviding.Visible = false;
                OptionsCBRProviding.Visible = false;
                OptionCalibration.Visible = false;
                PanelCalibrationSand.Visible = false;
                PanelCoreCutter.Visible = true;
                PanelWeightOfEmptyDish.Visible = false;
                PanelWeightOfEmptyBottle.Visible = false;
                PanelWeightOfEmptyPycnometer.Visible = false;
                getCoreCutterData();
            }
            else if (RadioButtonList1.SelectedValue == "Calibration of Sand / Cylinder Cone")
            {
                lnkCalculate.Visible = true;
                PanelMouldVolume.Visible = false;
                OptionsPanel.Visible = false;
                PanelWeightOfContainer.Visible = false;
                PanelCBRProviding.Visible = false;
                OptionsCBRProviding.Visible = false;
                PanelCoreCutter.Visible = false;
                OptionCalibration.Visible = true;
                PanelCalibrationSand.Visible = true;
                PanelWeightOfEmptyDish.Visible = false;
                PanelWeightOfEmptyBottle.Visible = false;
                PanelWeightOfEmptyPycnometer.Visible = false;
                RdbCalibration.SelectedValue = "Small";
                getCalibrationSandData("Calibration of Sand / Cylinder Cone-small");
            }
            else if (RadioButtonList1.SelectedValue == "Weight of empty Dish")
            {
                lnkCalculate.Visible = false;
                PanelWeightOfContainer.Visible = false;
                PanelMouldVolume.Visible = false;
                OptionsPanel.Visible = false;
                PanelCBRProviding.Visible = false;
                OptionsCBRProviding.Visible = false;
                OptionCalibration.Visible = false;
                PanelCalibrationSand.Visible = false;
                PanelCoreCutter.Visible = false;
                PanelWeightOfEmptyDish.Visible = true;
                PanelWeightOfEmptyBottle.Visible = false;
                PanelWeightOfEmptyPycnometer.Visible = false;
                getWeightOfEmptyDishData(RadioButtonList1.SelectedValue.ToString());
            }
            else if (RadioButtonList1.SelectedValue == "Weight of empty Bottle")
            {
                lnkCalculate.Visible = false;
                PanelWeightOfContainer.Visible = false;
                PanelMouldVolume.Visible = false;
                OptionsPanel.Visible = false;
                PanelCBRProviding.Visible = false;
                OptionsCBRProviding.Visible = false;
                OptionCalibration.Visible = false;
                PanelCalibrationSand.Visible = false;
                PanelCoreCutter.Visible = false;
                PanelWeightOfEmptyDish.Visible = false;
                PanelWeightOfEmptyBottle.Visible = true;
                PanelWeightOfEmptyPycnometer.Visible = false;
                getWeightOfEmptyBottleData(RadioButtonList1.SelectedValue.ToString());
            }
            else if (RadioButtonList1.SelectedValue == "Weight of empty Pycnometer")
            {
                lnkCalculate.Visible = false;
                PanelWeightOfContainer.Visible = false;
                PanelMouldVolume.Visible = false;
                OptionsPanel.Visible = false;
                PanelCBRProviding.Visible = false;
                OptionsCBRProviding.Visible = false;
                OptionCalibration.Visible = false;
                PanelCalibrationSand.Visible = false;
                PanelCoreCutter.Visible = false;
                PanelWeightOfEmptyDish.Visible = false;
                PanelWeightOfEmptyBottle.Visible = false;
                PanelWeightOfEmptyPycnometer.Visible = true;
                getWeightOfEmptyPycnometerData(RadioButtonList1.SelectedValue.ToString());
            }
        }
        protected void RdbMouldVolume1_OnTextChanged(object sender, EventArgs e)
        {
            if (RdbMouldVolume1.SelectedValue == "CBR")
            {
                RdbMouldVolume2.Items[0].Enabled = false;
                RdbMouldVolume2.SelectedValue = "Modified";
            }
            else
            {
                RdbMouldVolume2.Items[0].Enabled = true;
            }
            if (RdbMouldVolume1.SelectedValue == "MDD" && RdbMouldVolume2.SelectedValue == "Standard")
            {
                getMouldVolumeData("MDD", "Standard");
            }
            if (RdbMouldVolume1.SelectedValue == "MDD" && RdbMouldVolume2.SelectedValue == "Modified")
            {
                getMouldVolumeData("MDD", "Modified");
            }
            if (RdbMouldVolume1.SelectedValue == "CBR" && RdbMouldVolume2.SelectedValue == "Modified")
            {
                getMouldVolumeForCBR();
            }
        }
        protected void RdbMouldVolume2_OnTextChanged(object sender, EventArgs e)
        {
            if (RdbMouldVolume1.SelectedValue == "MDD" && RdbMouldVolume2.SelectedValue == "Standard")
            {
                getMouldVolumeData("MDD", "Standard");
            }
            if (RdbMouldVolume1.SelectedValue == "MDD" && RdbMouldVolume2.SelectedValue == "Modified")
            {
                getMouldVolumeData("MDD", "Modified");
            }
            if (RdbMouldVolume1.SelectedValue == "CBR" && RdbMouldVolume2.SelectedValue == "Modified")
            {
                getMouldVolumeForCBR();
            }
        }

        protected void RdbCalibration_OnTextChanged(object sender, EventArgs e)
        {
            if (RdbCalibration.SelectedValue == "Small")
            {
                //getCalibrationSandData("Calibration of Sand Small");
                getCalibrationSandData("Calibration of Sand / Cylinder Cone-small");
            }
            else if (RdbCalibration.SelectedValue == "Medium")
            {
                getCalibrationSandData("Calibration of Sand / Cylinder Cone-medium");
            }
            else if (RdbCalibration.SelectedValue == "Big")
            {
                getCalibrationSandData("Calibration of Sand / Cylinder Cone-big");
            }
        }

        protected void ddlRingSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            getCBRProvidingData();
        }

        //actions button and save button event
        protected void imgBtnAddRow_Click(object sender, EventArgs e)
        {
            AddNewRow();
        }
        protected void imgBtnDeleteRow_Click(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
            int rowIndex = currentRow.RowIndex;
            DeleteRow(rowIndex);
        }
        protected void imgBtnAddRowDish_Click(object sender, EventArgs e)
        {
            AddNewRowEmptyDish();
        }
        protected void imgBtnDeleteRowDish_Click(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
            int rowIndex = currentRow.RowIndex;
            DeleteRowEmptyDish(rowIndex);
        }
        protected void imgBtnAddRowBottle_Click(object sender, EventArgs e)
        {
            AddNewRowEmptyBottle();
        }
        protected void imgBtnDeleteRowBottle_Click(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
            int rowIndex = currentRow.RowIndex;
            DeleteRowEmptyBottle(rowIndex);
        }
        protected void imgBtnAddRowPycnometer_Click(object sender, EventArgs e)
        {
            AddNewRowEmptyPycnometer();
        }
        protected void imgBtnDeleteRowPycnometer_Click(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
            int rowIndex = currentRow.RowIndex;
            DeleteRowEmptyPycnometer(rowIndex);
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedValue == "Calibration of Sand / Cylinder Cone")
            {
                calculationOfSand();
            }
            else if (RadioButtonList1.SelectedValue == "Core Cutter Dimensions")
            {
                CalculateVolume();
            }

            if (ValidateData() == true)
            {
                DateTime fromDate = new DateTime();
                fromDate = DateTime.ParseExact(txt_From_Date.Text, "dd/MM/yyyy", null);


                if (pnlSoilSettings.Visible == true)
                {
                    string valueForRadio = string.Empty;

                    if (RadioButtonList1.SelectedValue == "Weight Of Container") valueForRadio = RadioButtonList1.SelectedValue;
                    else if (RadioButtonList1.SelectedValue == "Mould Volume") valueForRadio = RadioButtonList1.SelectedValue;
                    else if (RadioButtonList1.SelectedValue == "CBR Proving Ring") valueForRadio = RadioButtonList1.SelectedValue;
                    else if (RadioButtonList1.SelectedValue == "Core Cutter Dimensions") valueForRadio = RadioButtonList1.SelectedValue;
                    else if (RadioButtonList1.SelectedValue == "Calibration of Sand / Cylinder Cone") valueForRadio = RadioButtonList1.SelectedValue;
                    else if (RadioButtonList1.SelectedValue == "Weight of empty Dish") valueForRadio = RadioButtonList1.SelectedValue;
                    else if (RadioButtonList1.SelectedValue == "Weight of empty Bottle") valueForRadio = RadioButtonList1.SelectedValue;
                    else if (RadioButtonList1.SelectedValue == "Weight of empty Pycnometer") valueForRadio = RadioButtonList1.SelectedValue;
                    //decimal f1 = 0, f2 = 0, f3 = 0, f4 = 0, f5 = 0, f6 = 0;
                    if (valueForRadio == "Weight Of Container")
                    {

                        dc.SoilSetting_Update(valueForRadio, null, null, null, null, null, null, null, true);
                        foreach (GridViewRow row in grdViewSettings.Rows)
                        {
                            TextBox t1 = row.FindControl("txtContainerNo") as TextBox;
                            TextBox t2 = row.FindControl("txtWtContainer") as TextBox;

                            dc.SoilSetting_Update(valueForRadio, t1.Text, t2.Text, null, null, null, null, fromDate, false);
                            dc.SubmitChanges();
                        }
                    }
                    else if (valueForRadio == "Mould Volume")
                    {
                        if (RdbMouldVolume1.SelectedValue == "MDD" && RdbMouldVolume2.SelectedValue == "Standard")
                        {
                            dc.SoilSetting_Update("MDD Std Mould Volume", null, null, null, null, null, null, null, true);
                            foreach (GridViewRow row in GrdViewMould.Rows)
                            {
                                TextBox t1 = row.FindControl("txtMouldNo") as TextBox;
                                DropDownList t2 = row.FindControl("txtVolume") as DropDownList;
                                TextBox t3 = row.FindControl("txtWeight") as TextBox;

                                dc.SoilSetting_Update("MDD Std Mould Volume", t1.Text, t2.SelectedValue, t3.Text, null, null, null, fromDate, false);
                                dc.SubmitChanges();
                            }

                        }
                        else if (RdbMouldVolume1.SelectedValue == "MDD" && RdbMouldVolume2.SelectedValue == "Modified")
                        {
                            dc.SoilSetting_Update("MDD Modi Mould Volume", null, null, null, null, null, null, null, true);
                            foreach (GridViewRow row in GrdViewMould.Rows)
                            {
                                TextBox t1 = row.FindControl("txtMouldNo") as TextBox;
                                DropDownList d2 = row.FindControl("txtVolume") as DropDownList;
                                TextBox t3 = row.FindControl("txtWeight") as TextBox;

                                dc.SoilSetting_Update("MDD Modi Mould Volume", t1.Text, d2.SelectedValue, t3.Text, null, null, null, fromDate, false);
                                dc.SubmitChanges();
                            }

                        }
                        else if (RdbMouldVolume1.SelectedValue == "CBR" && RdbMouldVolume2.SelectedValue == "Modified")
                        {

                            dc.SoilSetting_Update("CBR Modi Mould Volume", null, null, null, null, null, null, null, true);
                            foreach (GridViewRow row in GrdViewMould.Rows)
                            {
                                TextBox t1 = row.FindControl("txtMouldNo") as TextBox;
                                DropDownList t2 = row.FindControl("txtVolume") as DropDownList;
                                TextBox t3 = row.FindControl("txtHeight") as TextBox;
                                TextBox t4 = row.FindControl("txtWeightOfMould") as TextBox;

                                dc.SoilSetting_Update("CBR Modi Mould Volume", t1.Text, t2.SelectedValue, t3.Text, t4.Text, null, null, fromDate, false);
                                dc.SubmitChanges();
                            }
                        }
                    }
                    else if (valueForRadio == "CBR Proving Ring")
                    {

                        dc.SoilSetting_Update("CBR Proving Ring" + "-" + ddlRingSize.SelectedValue, null, null, null, null, null, null, null, true);
                        foreach (GridViewRow row in GrdViewCBRProviding.Rows)
                        {
                            TextBox t1 = row.FindControl("txtDeflection") as TextBox;
                            TextBox t2 = row.FindControl("txtLoad") as TextBox;

                            dc.SoilSetting_Update("CBR Proving Ring" + "-" + ddlRingSize.SelectedValue, t1.Text, t2.Text, null, null, null, null, fromDate, false);
                            dc.SubmitChanges();
                        }
                    }
                    else if (valueForRadio == "Core Cutter Dimensions")
                    {

                        dc.SoilSetting_Update("Core Cutter Dimensions", null, null, null, null, null, null, null, true);
                        foreach (GridViewRow row in GrdViewCoreCutter.Rows)
                        {

                            TextBox t1 = row.FindControl("txtCoreCutterNo") as TextBox;
                            TextBox t2 = row.FindControl("txtWeight") as TextBox;
                            TextBox t3 = row.FindControl("txtHeight") as TextBox;
                            TextBox t4 = row.FindControl("txtDiameter") as TextBox;
                            TextBox t5 = row.FindControl("txtVolume") as TextBox;
                            TextBox t6 = row.FindControl("txtActualVolume") as TextBox;

                            dc.SoilSetting_Update("Core Cutter Dimensions", t1.Text, t2.Text, t3.Text, t4.Text, t5.Text, t6.Text, fromDate, false);
                            dc.SubmitChanges();
                        }

                    }
                    else if (valueForRadio == "Calibration of Sand / Cylinder Cone")
                    {
                        string sandValue = string.Empty;
                        if (RdbCalibration.SelectedValue == "Small")
                        {
                            sandValue = "Calibration of Sand / Cylinder Cone-small";
                        }

                        else if (RdbCalibration.SelectedValue == "Medium")
                        {
                            sandValue = "Calibration of Sand / Cylinder Cone-medium";
                        }

                        else if (RdbCalibration.SelectedValue == "Big")
                        {
                            sandValue = "Calibration of Sand / Cylinder Cone-big";
                        }

                        dc.SoilSetting_Update(sandValue, null, null, null, null, null, null, null, true);
                        foreach (GridViewRow row in GrdViewCalibrationSand.Rows)
                        {
                            //if (row.RowIndex != 0 && row.RowIndex != 3)
                            //{
                            TextBox t1 = row.FindControl("txtOne") as TextBox;
                            TextBox t2 = row.FindControl("txtTwo") as TextBox;
                            TextBox t3 = row.FindControl("txtThree") as TextBox;
                            TextBox t4 = row.FindControl("txtFour") as TextBox;
                            TextBox t5 = row.FindControl("txtFive") as TextBox;
                            dc.SoilSetting_Update(sandValue, t1.Text, t2.Text, t3.Text, t4.Text, t5.Text, null, fromDate, false);
                            dc.SubmitChanges();
                            //}
                        }

                    }
                    else if (valueForRadio == "Weight of empty Dish")
                    {
                        dc.SoilSetting_Update(valueForRadio, null, null, null, null, null, null, null, true);
                        foreach (GridViewRow row in grdWeightOfEmptyDish.Rows)
                        {
                            TextBox t1 = row.FindControl("txtNoOfDish") as TextBox;
                            TextBox t2 = row.FindControl("txtWtDish") as TextBox;

                            dc.SoilSetting_Update(valueForRadio, t1.Text, t2.Text, null, null, null, null, fromDate, false);
                            dc.SubmitChanges();
                        }
                    }
                    else if (valueForRadio == "Weight of empty Bottle")
                    {
                        dc.SoilSetting_Update(valueForRadio, null, null, null, null, null, null, null, true);
                        foreach (GridViewRow row in grdWeightOfEmptyBottle.Rows)
                        {
                            TextBox t1 = row.FindControl("txtNoOfBottle") as TextBox;
                            TextBox t2 = row.FindControl("txtWtBottle") as TextBox;
                            TextBox t3 = row.FindControl("txtWtBottlePlusDistWater") as TextBox;

                            dc.SoilSetting_Update(valueForRadio, t1.Text, t2.Text, t3.Text, null, null, null, fromDate, false);
                            dc.SubmitChanges();
                        }
                    }
                    else if (valueForRadio == "Weight of empty Pycnometer")
                    {
                        dc.SoilSetting_Update(valueForRadio, null, null, null, null, null, null, null, true);
                        foreach (GridViewRow row in grdWeightOfEmptyPycnometer.Rows)
                        {
                            TextBox t1 = row.FindControl("txtNoOfPycnometer") as TextBox;
                            TextBox t2 = row.FindControl("txtWtPycnometer") as TextBox;
                            TextBox t3 = row.FindControl("txtWtPycnometerPlusDistWater") as TextBox;

                            dc.SoilSetting_Update(valueForRadio, t1.Text, t2.Text, t3.Text, null, null, null, fromDate, false);
                            dc.SubmitChanges();
                        }
                    }
                }
                else if (pnlMachineSettings.Visible == true)
                {
                    if (RdbGenericList.SelectedValue == "Machine Settings")
                    {
                        dc.SoilSetting_Update("Machine Settings", null, null, null, null, null, null, null, true);
                        for (int i = 0; i < grdMachineSettings.Rows.Count; i++)
                        {
                            TextBox txtMachineNo = (TextBox)grdMachineSettings.Rows[i].FindControl("txtMachineNo");
                            TextBox txtOperatorName = (TextBox)grdMachineSettings.Rows[i].FindControl("txtOperatorName");
                            TextBox txtOwnerName = (TextBox)grdMachineSettings.Rows[i].FindControl("txtOwnerName");

                            dc.SoilSetting_Update("Machine Settings", txtMachineNo.Text, txtOperatorName.Text, txtOwnerName.Text, null, null, null, fromDate, false);
                            dc.SubmitChanges();
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Updated Successfully');", true);
            }

        }
        protected void lnkExit_Click(object sender, EventArgs e)
        {
            var result = System.Windows.Forms.MessageBox.Show("Do you really want to Exit?", "Warning", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning, System.Windows.Forms.MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Response.Redirect("Home.aspx");
            }
            else
            {
                return;
            }
        }
        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedValue == "Calibration of Sand / Cylinder Cone")
            {
                calculationOfSand();
            }
            else if (RadioButtonList1.SelectedValue == "Core Cutter Dimensions")
            {
                CalculateVolume();
            }
        }
        #endregion

        #region Weight Of Container Methods
        public void getWeightOfContainerData(string radselectedValue)
        {

            var SoilSettings_list = dc.SoilSetting_View(radselectedValue).ToList();

            if (SoilSettings_list.Count() == 0)
            {

                //if remark is not present add empty row 
                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("Col1", typeof(string)));
                dt1.Columns.Add(new DataColumn("Col2", typeof(string)));
                dr1 = dt1.NewRow();
                dr1["Col1"] = string.Empty;
                dr1["Col2"] = string.Empty;
                dt1.Rows.Add(dr1);
                ViewState["CubeInwardTable"] = dt1;
                grdViewSettings.DataSource = dt1;
                grdViewSettings.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Col1", typeof(string)));
                dt.Columns.Add(new DataColumn("Col2", typeof(string)));
                DataRow dr = null;
                for (int i = 0; i < SoilSettings_list.Count(); i++)
                {

                    dr = dt.NewRow();
                    dr["Col1"] = string.Empty;
                    dr["Col2"] = string.Empty;
                    dt.Rows.Add(dr);
                    ViewState["CubeInwardTable"] = dt;
                }
                grdViewSettings.DataSource = dt;
                grdViewSettings.DataBind();
            }
            int cnt = 0;

            foreach (var w in SoilSettings_list)
            {
                // AddNewRow();
                TextBox txtContainerNo = (TextBox)grdViewSettings.Rows[cnt].Cells[0].FindControl("txtContainerNo");
                TextBox txtWtContainer = (TextBox)grdViewSettings.Rows[cnt].Cells[1].FindControl("txtWtContainer");
                int containerNo = Convert.ToInt32(w.SOSET_F1_var);
                txtContainerNo.Text = containerNo.ToString();
                txtWtContainer.Text = w.SOSET_F2_var.ToString();                
                cnt++;
            }
            lblrowcount.Text = grdViewSettings.Rows.Count.ToString();
        }
        protected void AddNewRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CubeInwardTable"] != null)
            {
                GetCurrentData();
                dt = (DataTable)ViewState["CubeInwardTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtContainerNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWtContainer", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtContainerNo"] = string.Empty;
            dr["txtWtContainer"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CubeInwardTable"] = dt;
            grdViewSettings.DataSource = dt;
            grdViewSettings.DataBind();
            SetPreviousData();
            
        }
       
        protected void DeleteRow(int rowIndex)
        {
            int rowCount = Convert.ToInt32(lblrowcount.Text);
            if (rowIndex > rowCount - 1)
            {
                GetCurrentData();
                DataTable dt = ViewState["CubeInwardTable"] as DataTable;
                dt.Rows[rowIndex].Delete();
                ViewState["CubeInwardTable"] = dt;
                grdViewSettings.DataSource = dt;
                grdViewSettings.DataBind();
                SetPreviousData();
            }
        }
        protected void SetPreviousData()
        {
            DataTable dt = (DataTable)ViewState["CubeInwardTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdViewSettings.Rows[i].Cells[0].FindControl("txtContainerNo");
                TextBox box3 = (TextBox)grdViewSettings.Rows[i].Cells[1].FindControl("txtWtContainer");

                box2.Text = dt.Rows[i]["txtContainerNo"].ToString();
                box3.Text = dt.Rows[i]["txtWtContainer"].ToString();

            }
        }
        protected void GetCurrentData()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtContainerNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWtContainer", typeof(string)));

            for (int i = 0; i < grdViewSettings.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdViewSettings.Rows[i].Cells[0].FindControl("txtContainerNo");
                TextBox box3 = (TextBox)grdViewSettings.Rows[i].Cells[1].FindControl("txtWtContainer");

                drRow = dtTable.NewRow();
                drRow["txtContainerNo"] = box2.Text;
                drRow["txtWtContainer"] = box3.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CubeInwardTable"] = dtTable;

        }

        #endregion

        #region Mould Volume methods + events

        public void getMouldVolumeData(string option1, string option2)
        {

            GrdViewMould.Columns[4].Visible = true;
            GrdViewMould.Columns[5].Visible = false;
            GrdViewMould.Columns[6].Visible = false;

            List<SoilSetting_ViewResult> Mould_Volume = new List<SoilSetting_ViewResult>();

            if (option1 == "MDD" && option2 == "Standard")
            {
                RdbMouldVolume1.SelectedValue = "MDD";
                RdbMouldVolume2.SelectedValue = "Standard";
                Mould_Volume = dc.SoilSetting_View("MDD Std Mould Volume").ToList();
            }
            else if (option1 == "MDD" && option2 == "Modified")
            {
                Mould_Volume = dc.SoilSetting_View("MDD Modi Mould Volume").ToList();
            }

            if (Mould_Volume.Count() == 0)
            {

                //if remark is not present add empty row
                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("Col1", typeof(string)));
                dt1.Columns.Add(new DataColumn("Col2", typeof(string)));
                dt1.Columns.Add(new DataColumn("Col3", typeof(string)));
                dr1 = dt1.NewRow();
                dr1["Col1"] = string.Empty;
                dr1["Col2"] = string.Empty;
                dr1["Col3"] = string.Empty;
                dt1.Rows.Add(dr1);
                ViewState["MouldVolumeTable"] = dt1;
                GrdViewMould.DataSource = dt1;
                GrdViewMould.DataBind();
            }
            else
            {

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Col1", typeof(string)));
                dt.Columns.Add(new DataColumn("Col2", typeof(string)));
                dt.Columns.Add(new DataColumn("Col3", typeof(string)));
                DataRow dr = null;
                for (int i = 0; i < Mould_Volume.Count(); i++)
                {

                    dr = dt.NewRow();
                    dr["Col1"] = string.Empty;
                    dr["Col2"] = string.Empty;
                    dr["Col3"] = string.Empty;
                    dt.Rows.Add(dr);
                    ViewState["MouldVolumeTable"] = dt;

                }
                GrdViewMould.DataSource = dt;
                GrdViewMould.DataBind();
            }
            int cnt = 0;

            foreach (var w in Mould_Volume)
            {
                // AddNewRow();

                TextBox txtMouldNo = (TextBox)GrdViewMould.Rows[cnt].Cells[0].FindControl("txtMouldNo");
                DropDownList drpVolume = (DropDownList)GrdViewMould.Rows[cnt].Cells[1].FindControl("txtVolume");
                TextBox txtWeight = (TextBox)GrdViewMould.Rows[cnt].Cells[2].FindControl("txtWeight");

                txtMouldNo.Text = w.SOSET_F1_var;
                if (w.SOSET_F2_var == "2250")
                {
                    drpVolume.SelectedIndex = 2;
                }
                else if (w.SOSET_F2_var == "1000")
                {
                    drpVolume.SelectedIndex = 1;
                }
                txtWeight.Text = w.SOSET_F3_var.ToString();
                txtMouldNo.ReadOnly = true;
                txtWeight.ReadOnly = true;

                cnt++;
            }

            lblMouldCBRRowCount.Text= GrdViewMould.Rows.Count.ToString();

        }
        public void getMouldVolumeForCBR()
        {

            GrdViewMould.Columns[4].Visible = false;
            GrdViewMould.Columns[5].Visible = true;
            GrdViewMould.Columns[6].Visible = true;

            List<SoilSetting_ViewResult> Mould_Volume = new List<SoilSetting_ViewResult>();

            Mould_Volume = dc.SoilSetting_View("CBR Modi Mould Volume").ToList();

            if (Mould_Volume.Count() == 0)
            {

                //if remark is not present add empty row
                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("Col1", typeof(string)));
                dt1.Columns.Add(new DataColumn("Col2", typeof(string)));
                dt1.Columns.Add(new DataColumn("Col3", typeof(string)));
                dt1.Columns.Add(new DataColumn("Col4", typeof(string)));
                dr1 = dt1.NewRow();
                dr1["Col1"] = string.Empty;
                dr1["Col2"] = string.Empty;
                dr1["Col3"] = string.Empty;
                dr1["Col4"] = string.Empty;
                dt1.Rows.Add(dr1);
                ViewState["MouldVolumeTable"] = dt1;
                GrdViewMould.DataSource = dt1;
                GrdViewMould.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Col1", typeof(string)));
                dt.Columns.Add(new DataColumn("Col2", typeof(string)));
                dt.Columns.Add(new DataColumn("Col3", typeof(string)));
                dt.Columns.Add(new DataColumn("Col4", typeof(string)));
                DataRow dr = null;
                for (int i = 0; i < Mould_Volume.Count(); i++)
                {

                    dr = dt.NewRow();
                    dr["Col1"] = string.Empty;
                    dr["Col2"] = string.Empty;
                    dr["Col3"] = string.Empty;
                    dr["Col4"] = string.Empty;
                    dt.Rows.Add(dr);
                    ViewState["MouldVolumeTable"] = dt;

                }
                GrdViewMould.DataSource = dt;
                GrdViewMould.DataBind();
            }
            int cnt = 0;

            foreach (var w in Mould_Volume)
            {
                // AddNewRow();             
                TextBox txtMouldNo = (TextBox)GrdViewMould.Rows[cnt].Cells[0].FindControl("txtMouldNo");
                DropDownList txtVolume = (DropDownList)GrdViewMould.Rows[cnt].Cells[1].FindControl("txtVolume");
                TextBox txtHeight = (TextBox)GrdViewMould.Rows[cnt].Cells[3].FindControl("txtHeight");
                TextBox txtWeightOfMould = (TextBox)GrdViewMould.Rows[cnt].Cells[4].FindControl("txtWeightOfMould");

                txtMouldNo.Text = w.SOSET_F1_var;
                if (w.SOSET_F2_var == "2250")
                {
                    txtVolume.SelectedIndex = 2;
                }
                else if (w.SOSET_F2_var == "1000")
                {
                    txtVolume.SelectedIndex = 1;
                }
                txtHeight.Text = "127.3";
                txtWeightOfMould.Text = w.SOSET_F4_var.ToString();
                cnt++;
            }

            lblMouldCBRRowCount.Text = GrdViewMould.Rows.Count.ToString();

        }
        protected void AddNewRowForMould()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MouldVolumeTable"] != null)
            {
                GetCurrentDataForMould();
                dt = (DataTable)ViewState["MouldVolumeTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtMouldNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtVolume", typeof(string)));
                if (RdbMouldVolume1.SelectedValue != "CBR")
                {
                    dt.Columns.Add(new DataColumn("txtWeight", typeof(string)));
                }
                else if (RdbMouldVolume1.SelectedValue == "CBR")
                {
                    dt.Columns.Add(new DataColumn("txtHeight", typeof(string)));
                    dt.Columns.Add(new DataColumn("txtWeightOfMould", typeof(string)));
                }

            }

            dr = dt.NewRow();
            dr["txtMouldNo"] = string.Empty;
            dr["txtVolume"] = string.Empty;
            if (RdbMouldVolume1.SelectedValue != "CBR")
            {
                dr["txtWeight"] = string.Empty;
            }
            else if (RdbMouldVolume1.SelectedValue == "CBR")
            {
                dr["txtHeight"] = "127.3";
                dr["txtWeightOfMould"] = string.Empty;
            }

            dt.Rows.Add(dr);

            ViewState["MouldVolumeTable"] = dt;
            GrdViewMould.DataSource = dt;
            GrdViewMould.DataBind();
            SetPreviousDataForMould();
           
        }
        
        protected void GetCurrentDataForMould()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtMouldNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtVolume", typeof(string)));
            if (RdbMouldVolume1.SelectedValue != "CBR")
            {
                dtTable.Columns.Add(new DataColumn("txtWeight", typeof(string)));
            }
            else if (RdbMouldVolume1.SelectedValue == "CBR")
            {
                dtTable.Columns.Add(new DataColumn("txtHeight", typeof(string)));
                dtTable.Columns.Add(new DataColumn("txtWeightOfMould", typeof(string)));
            }

            for (int i = 0; i < GrdViewMould.Rows.Count; i++)
            {
                TextBox txtMouldNo = (TextBox)GrdViewMould.Rows[i].Cells[0].FindControl("txtMouldNo");
                // TextBox txtVolume = (TextBox)GrdViewMould.Rows[i].Cells[1].FindControl("txtVolume");
                DropDownList txtVolume = (DropDownList)GrdViewMould.Rows[i].Cells[1].FindControl("txtVolume");
                TextBox txtWeight = null;
                TextBox txtHeight = null;
                TextBox txtWeightOfMould = null;
                if (RdbMouldVolume1.SelectedValue != "CBR")
                {
                    txtWeight = (TextBox)GrdViewMould.Rows[i].Cells[2].FindControl("txtWeight");
                }
                else if (RdbMouldVolume1.SelectedValue == "CBR")
                {
                    txtHeight = (TextBox)GrdViewMould.Rows[i].Cells[3].FindControl("txtHeight");
                    txtWeightOfMould = (TextBox)GrdViewMould.Rows[i].Cells[4].FindControl("txtWeightOfMould");
                }
                drRow = dtTable.NewRow();
                drRow["txtMouldNo"] = txtMouldNo.Text;
                drRow["txtVolume"] = txtVolume.Text;
                if (RdbMouldVolume1.SelectedValue != "CBR")
                {
                    drRow["txtWeight"] = txtWeight.Text;
                }
                else if (RdbMouldVolume1.SelectedValue == "CBR")
                {
                    drRow["txtHeight"] = txtHeight.Text;
                    drRow["txtWeightOfMould"] = txtWeightOfMould.Text;
                }

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["MouldVolumeTable"] = dtTable;
        }
        protected void SetPreviousDataForMould()
        {
            DataTable dt = (DataTable)ViewState["MouldVolumeTable"];
            TextBox txtWeight = null;
            TextBox txtHeight = null;
            TextBox txtWeightOfMould = null;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtMouldNo = (TextBox)GrdViewMould.Rows[i].Cells[0].FindControl("txtMouldNo");
                //TextBox txtVolume = (TextBox)GrdViewMould.Rows[i].Cells[1].FindControl("txtVolume");
                DropDownList txtVolume = (DropDownList)GrdViewMould.Rows[i].Cells[1].FindControl("txtVolume");
                txtMouldNo.Text = dt.Rows[i]["txtMouldNo"].ToString();
                txtVolume.Text = dt.Rows[i]["txtVolume"].ToString();

                if (RdbMouldVolume1.SelectedValue != "CBR")
                {
                    txtWeight = (TextBox)GrdViewMould.Rows[i].Cells[2].FindControl("txtWeight");
                    txtWeight.Text = dt.Rows[i]["txtWeight"].ToString();
                }
                else if (RdbMouldVolume1.SelectedValue == "CBR")
                {
                    txtHeight = (TextBox)GrdViewMould.Rows[i].Cells[3].FindControl("txtHeight");
                    txtWeightOfMould = (TextBox)GrdViewMould.Rows[i].Cells[4].FindControl("txtWeightOfMould");

                    txtHeight.Text = dt.Rows[i]["txtHeight"].ToString();
                    txtWeightOfMould.Text = dt.Rows[i]["txtWeightOfMould"].ToString();
                }

            }
        }
        protected void AddRowForMould_Click(object sender, EventArgs e)
        {
            AddNewRowForMould();
        }
        protected void DeleteRowForMould_Click(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
            int rowIndex = currentRow.RowIndex;
            DeleteRowForMould(rowIndex);
        }
        protected void DeleteRowForMould(int rowIndex)
        {
            int rowCount = 0;
            if (RdbMouldVolume1.SelectedValue != "CBR")
            {
                rowCount = Convert.ToInt32(  lblMouldRowCount.Text);
            }
            else
            {
                rowCount = Convert.ToInt32(lblMouldCBRRowCount.Text);
            }
            if (rowIndex > rowCount - 1)
            {
                GetCurrentDataForMould();
                DataTable dt = ViewState["MouldVolumeTable"] as DataTable;
                dt.Rows[rowIndex].Delete();
                ViewState["MouldVolumeTable"] = dt;
                GrdViewMould.DataSource = dt;
                GrdViewMould.DataBind();
                SetPreviousDataForMould();
            }
        }
        #endregion

        #region CBR Proving
        public void getCBRProvidingData()
        {

            var SoilSettings_list = dc.SoilSetting_View("CBR Proving Ring" + "-" + ddlRingSize.SelectedValue).ToList();


            if (SoilSettings_list.Count() == 0)
            {

                //if remark is not present add empty row 
                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("txtDeflection", typeof(string)));
                dt1.Columns.Add(new DataColumn("txtLoad", typeof(string)));
                dr1 = dt1.NewRow();
                dr1["txtDeflection"] = string.Empty;
                dr1["txtLoad"] = string.Empty;
                dt1.Rows.Add(dr1);
                ViewState["CBRProvidingTable"] = dt1;
                GrdViewCBRProviding.DataSource = dt1;
                GrdViewCBRProviding.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("txtDeflection", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLoad", typeof(string)));
                DataRow dr = null;
                for (int i = 0; i < SoilSettings_list.Count(); i++)
                {

                    dr = dt.NewRow();
                    dr["txtDeflection"] = string.Empty;
                    dr["txtLoad"] = string.Empty;
                    dt.Rows.Add(dr);
                    ViewState["CBRProvidingTable"] = dt;

                }
                GrdViewCBRProviding.DataSource = dt;
                GrdViewCBRProviding.DataBind();
            }
            int cnt = 0;

            foreach (var w in SoilSettings_list)
            {
                // AddNewRow();
                TextBox txtDeflection = (TextBox)GrdViewCBRProviding.Rows[cnt].Cells[0].FindControl("txtDeflection");
                TextBox txtLoad = (TextBox)GrdViewCBRProviding.Rows[cnt].Cells[1].FindControl("txtLoad");
                txtDeflection.Text = w.SOSET_F1_var;
                txtLoad.Text = w.SOSET_F2_var;               
                cnt++;
            }
            lblRowCBRCount.Text = GrdViewCBRProviding.Rows.Count.ToString();
        }
        protected void AddNewRowForCBRProving()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CBRProvidingTable"] != null)
            {
                CBRGetCurrentData();
                dt = (DataTable)ViewState["CBRProvidingTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtDeflection", typeof(string)));
                dt.Columns.Add(new DataColumn("txtLoad", typeof(string)));

            }

            dr = dt.NewRow();
            dr["txtDeflection"] = string.Empty;
            dr["txtLoad"] = string.Empty;

            dt.Rows.Add(dr);

            ViewState["CBRProvidingTable"] = dt;
            GrdViewCBRProviding.DataSource = dt;
            GrdViewCBRProviding.DataBind();
            CBRSetPreviousData();
            
        }
       
        protected void CBRDeleteRow(int rowIndex)
        {
            int rowCount = Convert.ToInt32(lblRowCBRCount.Text);
            if (rowIndex > rowCount - 1)
            {
                CBRGetCurrentData();
                DataTable dt = ViewState["CBRProvidingTable"] as DataTable;
                dt.Rows[rowIndex].Delete();
                ViewState["CBRProvidingTable"] = dt;
                GrdViewCBRProviding.DataSource = dt;
                GrdViewCBRProviding.DataBind();
                CBRSetPreviousData();
            }
        }
        protected void CBRSetPreviousData()
        {
            DataTable dt = (DataTable)ViewState["CBRProvidingTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)GrdViewCBRProviding.Rows[i].Cells[0].FindControl("txtDeflection");
                TextBox box3 = (TextBox)GrdViewCBRProviding.Rows[i].Cells[1].FindControl("txtLoad");

                box2.Text = dt.Rows[i]["txtDeflection"].ToString();
                box3.Text = dt.Rows[i]["txtLoad"].ToString();

            }
        }
        protected void CBRGetCurrentData()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtDeflection", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtLoad", typeof(string)));

            for (int i = 0; i < GrdViewCBRProviding.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)GrdViewCBRProviding.Rows[i].Cells[0].FindControl("txtDeflection");
                TextBox box3 = (TextBox)GrdViewCBRProviding.Rows[i].Cells[1].FindControl("txtLoad");

                drRow = dtTable.NewRow();
                drRow["txtDeflection"] = box2.Text;
                drRow["txtLoad"] = box3.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CBRProvidingTable"] = dtTable;

        }

        protected void AddRowCBRProviding_Click(object sender, EventArgs e)
        {
            AddNewRowForCBRProving();
        }
        protected void DeleteRowCBRProviding_Click(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
            int rowIndex = currentRow.RowIndex;
            CBRDeleteRow(rowIndex);
        }

        #endregion

        #region Core Cutter Dimensions
        public void getCoreCutterData()
        {
            var SoilSettings_list = dc.SoilSetting_View("Core Cutter Dimensions").ToList();
            //dateparsing
            //try
            //{
            //    string datevalue = SoilSettings_list.FirstOrDefault().SOSET_FromDate.Value.ToShortDateString();
            //    string[] arrayFor = datevalue.Split('/');
            //    txt_From_Date.Text = arrayFor[1] + "/" + arrayFor[0] + "/" + arrayFor[2];
            //}
            //catch
            //{ }

            if (SoilSettings_list.Count() == 0)
            {

                //if remark is not present add empty row 
                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("txtCoreCutterNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txtWeight", typeof(string)));
                dt1.Columns.Add(new DataColumn("txtHeight", typeof(string)));
                dt1.Columns.Add(new DataColumn("txtDiameter", typeof(string)));
                dt1.Columns.Add(new DataColumn("txtVolume", typeof(string)));
                dt1.Columns.Add(new DataColumn("txtActualVolume", typeof(string)));
                dr1 = dt1.NewRow();
                dr1["txtCoreCutterNo"] = string.Empty;
                dr1["txtWeight"] = string.Empty;
                dr1["txtHeight"] = string.Empty;
                dr1["txtDiameter"] = string.Empty;
                dr1["txtVolume"] = string.Empty;
                dr1["txtActualVolume"] = string.Empty;
                dt1.Rows.Add(dr1);
                ViewState["CoreCutterTable"] = dt1;
                GrdViewCoreCutter.DataSource = dt1;
                GrdViewCoreCutter.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("txtCoreCutterNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWeight", typeof(string)));
                dt.Columns.Add(new DataColumn("txtHeight", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDiameter", typeof(string)));
                dt.Columns.Add(new DataColumn("txtVolume", typeof(string)));
                dt.Columns.Add(new DataColumn("txtActualVolume", typeof(string)));
                DataRow dr = null;
                for (int i = 0; i < SoilSettings_list.Count(); i++)
                {

                    dr = dt.NewRow();

                    dr["txtCoreCutterNo"] = string.Empty;
                    dr["txtWeight"] = string.Empty;
                    dr["txtHeight"] = string.Empty;
                    dr["txtDiameter"] = string.Empty;
                    dr["txtVolume"] = string.Empty;
                    dr["txtActualVolume"] = string.Empty;
                    dt.Rows.Add(dr);
                    ViewState["CoreCutterTable"] = dt;
                }
                GrdViewCoreCutter.DataSource = dt;
                GrdViewCoreCutter.DataBind();
            }
            int cnt = 0;

            foreach (var w in SoilSettings_list)
            {
                // AddNewRow();
                TextBox txtCoreCutterNo = (TextBox)GrdViewCoreCutter.Rows[cnt].Cells[0].FindControl("txtCoreCutterNo");
                TextBox txtWeight = (TextBox)GrdViewCoreCutter.Rows[cnt].Cells[1].FindControl("txtWeight");
                TextBox txtHeight = (TextBox)GrdViewCoreCutter.Rows[cnt].Cells[2].FindControl("txtHeight");
                TextBox txtDiameter = (TextBox)GrdViewCoreCutter.Rows[cnt].Cells[3].FindControl("txtDiameter");
                TextBox txtVolume = (TextBox)GrdViewCoreCutter.Rows[cnt].Cells[4].FindControl("txtVolume");
                TextBox txtActualVolume = (TextBox)GrdViewCoreCutter.Rows[cnt].Cells[5].FindControl("txtActualVolume");

                txtCoreCutterNo.Text = w.SOSET_F1_var;
                txtWeight.Text = w.SOSET_F2_var;
                txtHeight.Text = w.SOSET_F3_var;
                txtDiameter.Text = w.SOSET_F4_var;
                txtVolume.Text = w.SOSET_F5_var;
                txtActualVolume.Text = w.SOSET_F6_var;
                txtVolume.ReadOnly = true;              
                cnt++;
            }
             lblCoreCutterRowCount.Text = GrdViewCoreCutter.Rows.Count.ToString();
        }
        protected void AddNewRowForCoreCutter()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CoreCutterTable"] != null)
            {
                CoreCutterGetCurrentData();
                dt = (DataTable)ViewState["CoreCutterTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtCoreCutterNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWeight", typeof(string)));
                dt.Columns.Add(new DataColumn("txtHeight", typeof(string)));
                dt.Columns.Add(new DataColumn("txtDiameter", typeof(string)));
                dt.Columns.Add(new DataColumn("txtVolume", typeof(string)));
                dt.Columns.Add(new DataColumn("txtActualVolume", typeof(string)));

            }

            dr = dt.NewRow();
            dr["txtCoreCutterNo"] = string.Empty;
            dr["txtWeight"] = string.Empty;
            dr["txtHeight"] = string.Empty;
            dr["txtDiameter"] = string.Empty;
            dr["txtVolume"] = string.Empty;
            dr["txtActualVolume"] = string.Empty;

            dt.Rows.Add(dr);

            ViewState["CoreCutterTable"] = dt;
            GrdViewCoreCutter.DataSource = dt;
            GrdViewCoreCutter.DataBind();
            CoreCutterSetPreviousData();
           
        }
        
        protected void CoreCutterDeleteRow(int rowIndex)
        {
            int rowCount = Convert.ToInt32( lblCoreCutterRowCount.Text);
            if (rowIndex > rowCount - 1)
            {
                CoreCutterGetCurrentData();
                DataTable dt = ViewState["CoreCutterTable"] as DataTable;
                dt.Rows[rowIndex].Delete();
                ViewState["CoreCutterTable"] = dt;
                GrdViewCoreCutter.DataSource = dt;
                GrdViewCoreCutter.DataBind();
                CoreCutterSetPreviousData();
            }
        }
        protected void CoreCutterSetPreviousData()
        {
            DataTable dt = (DataTable)ViewState["CoreCutterTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtCoreCutterNo = (TextBox)GrdViewCoreCutter.Rows[i].Cells[0].FindControl("txtCoreCutterNo");
                TextBox txtWeight = (TextBox)GrdViewCoreCutter.Rows[i].Cells[1].FindControl("txtWeight");
                TextBox txtHeight = (TextBox)GrdViewCoreCutter.Rows[i].Cells[2].FindControl("txtHeight");
                TextBox txtDiameter = (TextBox)GrdViewCoreCutter.Rows[i].Cells[3].FindControl("txtDiameter");
                TextBox txtVolume = (TextBox)GrdViewCoreCutter.Rows[i].Cells[4].FindControl("txtVolume");
                TextBox txtActualVolume = (TextBox)GrdViewCoreCutter.Rows[i].Cells[5].FindControl("txtActualVolume");

                txtCoreCutterNo.Text = dt.Rows[i]["txtCoreCutterNo"].ToString();
                txtWeight.Text = dt.Rows[i]["txtWeight"].ToString();
                txtHeight.Text = dt.Rows[i]["txtHeight"].ToString();
                txtDiameter.Text = dt.Rows[i]["txtDiameter"].ToString();
                txtVolume.Text = dt.Rows[i]["txtVolume"].ToString();
                txtActualVolume.Text = dt.Rows[i]["txtActualVolume"].ToString();

            }
        }
        protected void CoreCutterGetCurrentData()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtCoreCutterNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWeight", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtHeight", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtDiameter", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtVolume", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtActualVolume", typeof(string)));

            for (int i = 0; i < GrdViewCoreCutter.Rows.Count; i++)
            {
                TextBox txtCoreCutterNo = (TextBox)GrdViewCoreCutter.Rows[i].Cells[0].FindControl("txtCoreCutterNo");
                TextBox txtWeight = (TextBox)GrdViewCoreCutter.Rows[i].Cells[1].FindControl("txtWeight");
                TextBox txtHeight = (TextBox)GrdViewCoreCutter.Rows[i].Cells[2].FindControl("txtHeight");
                TextBox txtDiameter = (TextBox)GrdViewCoreCutter.Rows[i].Cells[3].FindControl("txtDiameter");
                TextBox txtVolume = (TextBox)GrdViewCoreCutter.Rows[i].Cells[4].FindControl("txtVolume");
                TextBox txtActualVolume = (TextBox)GrdViewCoreCutter.Rows[i].Cells[5].FindControl("txtActualVolume");


                drRow = dtTable.NewRow();

                drRow["txtCoreCutterNo"] = txtCoreCutterNo.Text;
                drRow["txtWeight"] = txtWeight.Text;
                drRow["txtHeight"] = txtHeight.Text;
                drRow["txtDiameter"] = txtDiameter.Text;
                drRow["txtVolume"] = txtVolume.Text;
                drRow["txtActualVolume"] = txtActualVolume.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["CoreCutterTable"] = dtTable;

        }
        protected void AddRowForCoreCutter_Click(object sender, EventArgs e)
        {
            AddNewRowForCoreCutter();
        }
        protected void DeleteRowForCoreCutter_Click(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
            int rowIndex = currentRow.RowIndex;
            CoreCutterDeleteRow(rowIndex);
        }

        public void CalculateVolume()
        {
            for (int rowIndex = 0; rowIndex < GrdViewCoreCutter.Rows.Count; rowIndex++)
            {

                TextBox txtHeight = (TextBox)GrdViewCoreCutter.Rows[rowIndex].FindControl("txtHeight");
                TextBox txtDiameter = (TextBox)GrdViewCoreCutter.Rows[rowIndex].FindControl("txtDiameter");
                TextBox txtVolume = (TextBox)GrdViewCoreCutter.Rows[rowIndex].FindControl("txtVolume");
                decimal height = 0, diameter = 0, volume = 0;

                if (txtHeight.Text != "" && txtDiameter.Text != "")
                {
                    height = Convert.ToDecimal(txtHeight.Text);
                    diameter = Convert.ToDecimal(txtDiameter.Text);
                    decimal PI = Convert.ToDecimal(3.14);

                    volume = (PI * (diameter * diameter) * height) / 4;
                    volume = Math.Round(volume, 2);
                    txtVolume.Text = volume.ToString();
                }
            }
        }
        #endregion

        #region Calibration of Sand
        public void getCalibrationSandData(string sandValue)
        {

            var SoilSettings_list = dc.SoilSetting_View(sandValue).ToList();

            string[] arryForTest = new string[11];
            arryForTest[0] = "Wt. of Cylinder + before Pouring Sand into Calibrating Can, g";
            arryForTest[1] = "Wt. of Cylinder + Sand after Pouring into Calibrating Can, g";
            arryForTest[2] = "Wt. of Sand in Cone + Calibrating Can, g";
            arryForTest[3] = "Wt. of Sand before Pouring into Level Platform, g";
            arryForTest[4] = "Wt. of Sand after Pouring into Level Platform, g";
            arryForTest[5] = "Wt. of Sand in Cone, g";
            arryForTest[6] = "Wt. of Sand in Calibrating Can, g";
            arryForTest[7] = "Vol. of Calibrating Can, cc";
            arryForTest[8] = "Bulk Density of Sand, g/cc";
            arryForTest[9] = "Avg. Bulk Density, g/cc";
            arryForTest[10] = "Avg. of Wt. of Sand in Cone, gm";

            //if remark is not present add empty row 
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;
            dt1.Columns.Add(new DataColumn("txtTestNo", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtOne", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtTwo", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtThree", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtFour", typeof(string)));
            dt1.Columns.Add(new DataColumn("txtFive", typeof(string)));



            for (int i = 0; i < 11; i++)
            {
                dr1 = dt1.NewRow();
                dr1["txtTestNo"] = string.Empty;
                dr1["txtOne"] = string.Empty;
                dr1["txtTwo"] = string.Empty;
                dr1["txtThree"] = string.Empty;
                dr1["txtFour"] = string.Empty;
                dr1["txtFive"] = string.Empty;
                dt1.Rows.Add(dr1);
            }

            ViewState["CalibrationTable"] = dt1;
            GrdViewCalibrationSand.DataSource = dt1;
            GrdViewCalibrationSand.DataBind();

            for (int i = 0; i < 11; i++)
            {
                TextBox txtTestNo = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[0].FindControl("txtTestNo");
                TextBox txtOne = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[1].FindControl("txtOne");
                TextBox txtTwo = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[2].FindControl("txtTwo");
                TextBox txtThree = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[3].FindControl("txtThree");
                TextBox txtFour = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[4].FindControl("txtFour");
                TextBox txtFive = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[5].FindControl("txtFive");

                txtTestNo.Text = arryForTest[i];
                if (i == 0 || i == 2 || i == 3 || i == 5 || i == 8 || i == 9 || i == 10)
                {
                    //GrdViewCalibrationSand.RowStyle. = System.Drawing.Color.Gray;
                    txtOne.ReadOnly = true;
                    txtOne.BackColor = System.Drawing.Color.LightGray;
                    txtTwo.ReadOnly = true;
                    txtTwo.BackColor = System.Drawing.Color.LightGray;
                    txtThree.ReadOnly = true;
                    txtThree.BackColor = System.Drawing.Color.LightGray;
                    txtFour.ReadOnly = true;
                    txtFour.BackColor = System.Drawing.Color.LightGray;
                    txtFive.ReadOnly = true;
                    txtFive.BackColor = System.Drawing.Color.LightGray;
                }

            }
            int cnt = 0;
            if (SoilSettings_list.Count == 0)
            {
                for (int i = 0; i < 11; i++)
                {
                    // AddNewRow();

                    TextBox txtOne = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[1].FindControl("txtOne");
                    TextBox txtTwo = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[2].FindControl("txtTwo");
                    TextBox txtThree = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[3].FindControl("txtThree");
                    TextBox txtFour = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[4].FindControl("txtFour");
                    TextBox txtFive = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[5].FindControl("txtFive");

                    if (RdbCalibration.SelectedValue == "Small")
                    {
                        if (i == 0 || i == 3)
                        {
                            txtOne.Text = "10000";
                            txtTwo.Text = "10000";
                            txtThree.Text = "10000";
                            txtFour.Text = "10000";
                            txtFive.Text = "10000";
                        }
                    }
                    else if (RdbCalibration.SelectedValue == "Medium")
                    {
                        if (i == 0 || i == 3)
                        {
                            txtOne.Text = "15000";
                            txtTwo.Text = "15000";
                            txtThree.Text = "15000";
                            txtFour.Text = "15000";
                            txtFive.Text = "15000";
                        }
                    }
                    else if (RdbCalibration.SelectedValue == "Big")
                    {
                        if (i == 0 || i == 3)
                        {
                            txtOne.Text = "30000";
                            txtTwo.Text = "30000";
                            txtThree.Text = "30000";
                            txtFour.Text = "30000";
                            txtFive.Text = "30000";
                        }
                    }
                }
            }

            else
            {
                foreach (var w in SoilSettings_list)
                {
                    // AddNewRow();
                    //if (cnt == 0 || cnt == 3)
                    //{
                    //    cnt = cnt + 1;
                    //}
                    TextBox txtTestNo = (TextBox)GrdViewCalibrationSand.Rows[cnt].Cells[0].FindControl("txtTestNo");
                    TextBox txtOne = (TextBox)GrdViewCalibrationSand.Rows[cnt].Cells[1].FindControl("txtOne");
                    TextBox txtTwo = (TextBox)GrdViewCalibrationSand.Rows[cnt].Cells[2].FindControl("txtTwo");
                    TextBox txtThree = (TextBox)GrdViewCalibrationSand.Rows[cnt].Cells[3].FindControl("txtThree");
                    TextBox txtFour = (TextBox)GrdViewCalibrationSand.Rows[cnt].Cells[4].FindControl("txtFour");
                    TextBox txtFive = (TextBox)GrdViewCalibrationSand.Rows[cnt].Cells[5].FindControl("txtFive");

                    if (cnt != 8 && cnt != 9 && cnt != 10)
                    {
                        txtOne.Text = w.SOSET_F1_var;
                        txtTwo.Text = w.SOSET_F2_var;
                        txtThree.Text = w.SOSET_F3_var;
                        txtFour.Text = w.SOSET_F4_var;
                        txtFive.Text = w.SOSET_F5_var;
                    }
                    else
                    {
                        txtOne.Text = w.SOSET_F1_var;
                        txtTwo.Text = w.SOSET_F2_var;
                        txtThree.Text = w.SOSET_F3_var;
                        txtFour.Text = w.SOSET_F4_var;
                        txtFive.Text = w.SOSET_F5_var;
                    }
                    if (cnt == 9 || cnt == 10)
                    {
                        txtOne.Text = "";
                        txtTwo.Text = "";
                        txtFour.Text = "";
                        txtFive.Text = "";
                    }
                    cnt++;
                }
            }
        }
        public void calculationOfSand()
        {
            for (int rowIndex = 0; rowIndex < GrdViewCalibrationSand.Rows.Count; rowIndex++)
            {
                TextBox txtOne1 = null;
                TextBox txtOne2 = null;
                TextBox txtOne3 = null;
                string controlId = "";
                decimal val1 = 0, val2 = 0, val3 = 0;

                if (rowIndex == 0 || rowIndex == 2 || rowIndex == 3 || rowIndex == 5 || rowIndex == 8 || rowIndex == 9 || rowIndex == 10)
                {

                }
                else
                {
                    for (int column = 0; column < GrdViewCalibrationSand.Columns.Count; column++)
                    {
                        if (column == 1)
                            controlId = "txtOne";
                        if (column == 2)
                            controlId = "txtTwo";
                        if (column == 3)
                            controlId = "txtThree";
                        if (column == 4)
                            controlId = "txtFour";
                        if (column == 5)
                            controlId = "txtFive";
                        if (column != 0)
                        {

                            if (rowIndex != 6)
                            {
                                txtOne1 = (TextBox)GrdViewCalibrationSand.Rows[rowIndex - 1].Cells[column].FindControl(controlId);
                                txtOne2 = (TextBox)GrdViewCalibrationSand.Rows[rowIndex].Cells[column].FindControl(controlId);
                                txtOne3 = (TextBox)GrdViewCalibrationSand.Rows[rowIndex + 1].Cells[column].FindControl(controlId);
                            }
                            else if (rowIndex == 6)
                            {
                                txtOne1 = (TextBox)GrdViewCalibrationSand.Rows[rowIndex].Cells[column].FindControl(controlId);
                                txtOne2 = (TextBox)GrdViewCalibrationSand.Rows[rowIndex + 1].Cells[column].FindControl(controlId);
                                txtOne3 = (TextBox)GrdViewCalibrationSand.Rows[rowIndex + 2].Cells[column].FindControl(controlId);
                            }

                            if (txtOne1.Text != "" && txtOne2.Text != "")
                            {
                                val1 = Convert.ToDecimal(txtOne1.Text);
                                val2 = Convert.ToDecimal(txtOne2.Text);
                                if (rowIndex == 1 || rowIndex == 4)
                                {
                                    val3 = val1 - val2;
                                    txtOne3.Text = val3.ToString("0.000");
                                }
                                else if (rowIndex == 6 || rowIndex == 7)
                                {
                                    val3 = val1 / val2;
                                    txtOne3.Text = val3.ToString("0.000");

                                }
                            }
                        }
                    }

                    if (rowIndex == 6 || rowIndex == 7 || rowIndex == 4)
                    {
                        TextBox txtThree = null;
                        int index = 0;
                        if (rowIndex == 6 || rowIndex == 7)
                        {
                            index = 8;
                        }
                        else if (rowIndex == 4)
                        {
                            index = 5;
                        }
                        TextBox txtOne = (TextBox)GrdViewCalibrationSand.Rows[index].Cells[1].FindControl("txtOne");
                        TextBox txtTwo = (TextBox)GrdViewCalibrationSand.Rows[index].Cells[2].FindControl("txtTwo");
                        txtThree = (TextBox)GrdViewCalibrationSand.Rows[index].Cells[3].FindControl("txtThree");
                        TextBox txtFour = (TextBox)GrdViewCalibrationSand.Rows[index].Cells[4].FindControl("txtFour");
                        TextBox txtFive = (TextBox)GrdViewCalibrationSand.Rows[index].Cells[5].FindControl("txtFive");
                        decimal one = 0, two = 0, three = 0, four = 0, five = 0;
                        if (txtOne.Text != "" && txtTwo.Text != "" && txtThree.Text != "" && txtFour.Text != "" && txtFive.Text != "")
                        {
                            one = Convert.ToDecimal(txtOne.Text); two = Convert.ToDecimal(txtTwo.Text);
                            three = Convert.ToDecimal(txtThree.Text); four = Convert.ToDecimal(txtFour.Text);
                            five = Convert.ToDecimal(txtFive.Text);

                            decimal totalOfBulk = one + two + three + four + five;

                            totalOfBulk = totalOfBulk / 5;
                            if (rowIndex == 6 || rowIndex == 7)
                            {
                                txtThree = (TextBox)GrdViewCalibrationSand.Rows[9].Cells[3].FindControl("txtThree");
                            }
                            else if (rowIndex == 4)
                            {
                                txtThree = (TextBox)GrdViewCalibrationSand.Rows[10].Cells[3].FindControl("txtThree");
                            }

                            txtThree.Text = totalOfBulk.ToString("0.000");
                        }

                    }

                }

            }

        }
        #endregion

        #region Weight Of Empty Dish
        public void getWeightOfEmptyDishData(string radselectedValue)
        {
            var SoilSettings_list = dc.SoilSetting_View(radselectedValue).ToList();
            if (SoilSettings_list.Count() == 0)
            {
                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("Col1", typeof(string)));
                dt1.Columns.Add(new DataColumn("Col2", typeof(string)));
                dr1 = dt1.NewRow();
                dr1["Col1"] = string.Empty;
                dr1["Col2"] = string.Empty;
                dt1.Rows.Add(dr1);
                ViewState["EmptyDishTable"] = dt1;
                grdWeightOfEmptyDish.DataSource = dt1;
                grdWeightOfEmptyDish.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Col1", typeof(string)));
                dt.Columns.Add(new DataColumn("Col2", typeof(string)));
                DataRow dr = null;
                for (int i = 0; i < SoilSettings_list.Count(); i++)
                {
                    dr = dt.NewRow();
                    dr["Col1"] = string.Empty;
                    dr["Col2"] = string.Empty;
                    dt.Rows.Add(dr);
                    ViewState["EmptyDishTable"] = dt;
                }
                grdWeightOfEmptyDish.DataSource = dt;
                grdWeightOfEmptyDish.DataBind();
            }
            int cnt = 0;

            foreach (var w in SoilSettings_list)
            {
                TextBox txtNoOfDish = (TextBox)grdWeightOfEmptyDish.Rows[cnt].Cells[0].FindControl("txtNoOfDish");
                TextBox txtWtDish = (TextBox)grdWeightOfEmptyDish.Rows[cnt].Cells[1].FindControl("txtWtDish");

                txtNoOfDish.Text = w.SOSET_F1_var.ToString();
                txtWtDish.Text = w.SOSET_F2_var.ToString();

                cnt++;
            }
            lblrowcount.Text = grdWeightOfEmptyDish.Rows.Count.ToString();
        }
        protected void AddNewRowEmptyDish()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["EmptyDishTable"] != null)
            {
                GetCurrentDataEmptyDish();
                dt = (DataTable)ViewState["EmptyDishTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtNoOfDish", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWtDish", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtNoOfDish"] = string.Empty;
            dr["txtWtDish"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["EmptyDishTable"] = dt;
            grdWeightOfEmptyDish.DataSource = dt;
            grdWeightOfEmptyDish.DataBind();
            SetPreviousDataEmptyDish();

        }

        protected void DeleteRowEmptyDish(int rowIndex)
        {
            int rowCount = Convert.ToInt32(lblrowcount.Text);
            if (rowIndex > rowCount - 1)
            {
                GetCurrentDataEmptyDish();
                DataTable dt = ViewState["EmptyDishTable"] as DataTable;
                dt.Rows[rowIndex].Delete();
                ViewState["EmptyDishTable"] = dt;
                grdWeightOfEmptyDish.DataSource = dt;
                grdWeightOfEmptyDish.DataBind();
                SetPreviousDataEmptyDish();
            }
        }
        protected void SetPreviousDataEmptyDish()
        {
            DataTable dt = (DataTable)ViewState["EmptyDishTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdWeightOfEmptyDish.Rows[i].Cells[0].FindControl("txtNoOfDish");
                TextBox box3 = (TextBox)grdWeightOfEmptyDish.Rows[i].Cells[1].FindControl("txtWtDish");

                box2.Text = dt.Rows[i]["txtNoOfDish"].ToString();
                box3.Text = dt.Rows[i]["txtWtDish"].ToString();

            }
        }
        protected void GetCurrentDataEmptyDish()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtNoOfDish", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWtDish", typeof(string)));

            for (int i = 0; i < grdWeightOfEmptyDish.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdWeightOfEmptyDish.Rows[i].Cells[0].FindControl("txtNoOfDish");
                TextBox box3 = (TextBox)grdWeightOfEmptyDish.Rows[i].Cells[1].FindControl("txtWtDish");

                drRow = dtTable.NewRow();
                drRow["txtNoOfDish"] = box2.Text;
                drRow["txtWtDish"] = box3.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["EmptyDishTable"] = dtTable;

        }

        #endregion

        #region Weight Of Empty Bottle
        public void getWeightOfEmptyBottleData(string radselectedValue)
        {
            var SoilSettings_list = dc.SoilSetting_View(radselectedValue).ToList();
            if (SoilSettings_list.Count() == 0)
            {
                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("Col1", typeof(string)));
                dt1.Columns.Add(new DataColumn("Col2", typeof(string)));
                dt1.Columns.Add(new DataColumn("Col3", typeof(string)));
                dr1 = dt1.NewRow();
                dr1["Col1"] = string.Empty;
                dr1["Col2"] = string.Empty;
                dr1["Col3"] = string.Empty;
                dt1.Rows.Add(dr1);
                ViewState["EmptyBottleTable"] = dt1;
                grdWeightOfEmptyBottle.DataSource = dt1;
                grdWeightOfEmptyBottle.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Col1", typeof(string)));
                dt.Columns.Add(new DataColumn("Col2", typeof(string)));
                dt.Columns.Add(new DataColumn("Col3", typeof(string)));
                DataRow dr = null;
                for (int i = 0; i < SoilSettings_list.Count(); i++)
                {
                    dr = dt.NewRow();
                    dr["Col1"] = string.Empty;
                    dr["Col2"] = string.Empty;
                    dr["Col3"] = string.Empty;
                    dt.Rows.Add(dr);
                    ViewState["EmptyBottleTable"] = dt;
                }
                grdWeightOfEmptyBottle.DataSource = dt;
                grdWeightOfEmptyBottle.DataBind();
            }
            int cnt = 0;

            foreach (var w in SoilSettings_list)
            {
                TextBox txtNoOfBottle = (TextBox)grdWeightOfEmptyBottle.Rows[cnt].Cells[0].FindControl("txtNoOfBottle");
                TextBox txtWtBottle = (TextBox)grdWeightOfEmptyBottle.Rows[cnt].Cells[1].FindControl("txtWtBottle");
                TextBox txtWtBottlePlusDistWater = (TextBox)grdWeightOfEmptyBottle.Rows[cnt].Cells[2].FindControl("txtWtBottlePlusDistWater");

                txtNoOfBottle.Text = w.SOSET_F1_var.ToString();
                txtWtBottle.Text = w.SOSET_F2_var.ToString();
                txtWtBottlePlusDistWater.Text = w.SOSET_F3_var.ToString();

                cnt++;
            }
            lblrowcount.Text = grdWeightOfEmptyBottle.Rows.Count.ToString();
        }
        protected void AddNewRowEmptyBottle()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["EmptyBottleTable"] != null)
            {
                GetCurrentDataEmptyBottle();
                dt = (DataTable)ViewState["EmptyBottleTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtNoOfBottle", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWtBottle", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWtBottlePlusDistWater", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtNoOfBottle"] = string.Empty;
            dr["txtWtBottle"] = string.Empty;
            dr["txtWtBottlePlusDistWater"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["EmptyBottleTable"] = dt;
            grdWeightOfEmptyBottle.DataSource = dt;
            grdWeightOfEmptyBottle.DataBind();
            SetPreviousDataEmptyBottle();

        }

        protected void DeleteRowEmptyBottle(int rowIndex)
        {
            int rowCount = Convert.ToInt32(lblrowcount.Text);
            if (rowIndex > rowCount - 1)
            {
                GetCurrentDataEmptyBottle();
                DataTable dt = ViewState["EmptyBottleTable"] as DataTable;
                dt.Rows[rowIndex].Delete();
                ViewState["EmptyBottleTable"] = dt;
                grdWeightOfEmptyBottle.DataSource = dt;
                grdWeightOfEmptyBottle.DataBind();
                SetPreviousDataEmptyBottle();
            }
        }
        protected void SetPreviousDataEmptyBottle()
        {
            DataTable dt = (DataTable)ViewState["EmptyBottleTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdWeightOfEmptyBottle.Rows[i].Cells[0].FindControl("txtNoOfBottle");
                TextBox box3 = (TextBox)grdWeightOfEmptyBottle.Rows[i].Cells[1].FindControl("txtWtBottle");
                TextBox box4 = (TextBox)grdWeightOfEmptyBottle.Rows[i].Cells[2].FindControl("txtWtBottlePlusDistWater");

                box2.Text = dt.Rows[i]["txtNoOfBottle"].ToString();
                box3.Text = dt.Rows[i]["txtWtBottle"].ToString();
                box4.Text = dt.Rows[i]["txtWtBottlePlusDistWater"].ToString();

            }
        }
        protected void GetCurrentDataEmptyBottle()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtNoOfBottle", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWtBottle", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWtBottlePlusDistWater", typeof(string)));

            for (int i = 0; i < grdWeightOfEmptyBottle.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdWeightOfEmptyBottle.Rows[i].Cells[0].FindControl("txtNoOfBottle");
                TextBox box3 = (TextBox)grdWeightOfEmptyBottle.Rows[i].Cells[1].FindControl("txtWtBottle");
                TextBox box4 = (TextBox)grdWeightOfEmptyBottle.Rows[i].Cells[1].FindControl("txtWtBottlePlusDistWater");

                drRow = dtTable.NewRow();
                drRow["txtNoOfBottle"] = box2.Text;
                drRow["txtWtBottle"] = box3.Text;
                drRow["txtWtBottlePlusDistWater"] = box4.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["EmptyBottleTable"] = dtTable;

        }

        #endregion

        #region Weight Of Empty Pycnometer
        public void getWeightOfEmptyPycnometerData(string radselectedValue)
        {
            var SoilSettings_list = dc.SoilSetting_View(radselectedValue).ToList();
            if (SoilSettings_list.Count() == 0)
            {
                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("Col1", typeof(string)));
                dt1.Columns.Add(new DataColumn("Col2", typeof(string)));
                dt1.Columns.Add(new DataColumn("Col3", typeof(string)));
                dr1 = dt1.NewRow();
                dr1["Col1"] = string.Empty;
                dr1["Col2"] = string.Empty;
                dr1["Col3"] = string.Empty;
                dt1.Rows.Add(dr1);
                ViewState["EmptyPycnometerTable"] = dt1;
                grdWeightOfEmptyPycnometer.DataSource = dt1;
                grdWeightOfEmptyPycnometer.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Col1", typeof(string)));
                dt.Columns.Add(new DataColumn("Col2", typeof(string)));
                dt.Columns.Add(new DataColumn("Col3", typeof(string)));
                DataRow dr = null;
                for (int i = 0; i < SoilSettings_list.Count(); i++)
                {
                    dr = dt.NewRow();
                    dr["Col1"] = string.Empty;
                    dr["Col2"] = string.Empty;
                    dr["Col3"] = string.Empty;
                    dt.Rows.Add(dr);
                    ViewState["EmptyPycnometerTable"] = dt;
                }
                grdWeightOfEmptyPycnometer.DataSource = dt;
                grdWeightOfEmptyPycnometer.DataBind();
            }
            int cnt = 0;

            foreach (var w in SoilSettings_list)
            {
                TextBox txtNoOfPycnometer = (TextBox)grdWeightOfEmptyPycnometer.Rows[cnt].Cells[0].FindControl("txtNoOfPycnometer");
                TextBox txtWtPycnometer = (TextBox)grdWeightOfEmptyPycnometer.Rows[cnt].Cells[1].FindControl("txtWtPycnometer");
                TextBox txtWtPycnometerPlusDistWater = (TextBox)grdWeightOfEmptyPycnometer.Rows[cnt].Cells[2].FindControl("txtWtPycnometerPlusDistWater");

                txtNoOfPycnometer.Text = w.SOSET_F1_var.ToString();
                txtWtPycnometer.Text = w.SOSET_F2_var.ToString();
                txtWtPycnometerPlusDistWater.Text = w.SOSET_F3_var.ToString();

                cnt++;
            }
            lblrowcount.Text = grdWeightOfEmptyPycnometer.Rows.Count.ToString();
        }
        protected void AddNewRowEmptyPycnometer()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["EmptyPycnometerTable"] != null)
            {
                GetCurrentDataEmptyPycnometer();
                dt = (DataTable)ViewState["EmptyPycnometerTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtNoOfPycnometer", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWtPycnometer", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWtPycnometerPlusDistWater", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtNoOfPycnometer"] = string.Empty;
            dr["txtWtPycnometer"] = string.Empty;
            dr["txtWtPycnometerPlusDistWater"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["EmptyPycnometerTable"] = dt;
            grdWeightOfEmptyPycnometer.DataSource = dt;
            grdWeightOfEmptyPycnometer.DataBind();
            SetPreviousDataEmptyPycnometer();

        }

        protected void DeleteRowEmptyPycnometer(int rowIndex)
        {
            int rowCount = Convert.ToInt32(lblrowcount.Text);
            if (rowIndex > rowCount - 1)
            {
                GetCurrentDataEmptyPycnometer();
                DataTable dt = ViewState["EmptyPycnometerTable"] as DataTable;
                dt.Rows[rowIndex].Delete();
                ViewState["EmptyPycnometerTable"] = dt;
                grdWeightOfEmptyPycnometer.DataSource = dt;
                grdWeightOfEmptyPycnometer.DataBind();
                SetPreviousDataEmptyPycnometer();
            }
        }
        protected void SetPreviousDataEmptyPycnometer()
        {
            DataTable dt = (DataTable)ViewState["EmptyPycnometerTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdWeightOfEmptyPycnometer.Rows[i].Cells[0].FindControl("txtNoOfPycnometer");
                TextBox box3 = (TextBox)grdWeightOfEmptyPycnometer.Rows[i].Cells[1].FindControl("txtWtPycnometer");
                TextBox box4 = (TextBox)grdWeightOfEmptyPycnometer.Rows[i].Cells[2].FindControl("txtWtPycnometerPlusDistWater");

                box2.Text = dt.Rows[i]["txtNoOfPycnometer"].ToString();
                box3.Text = dt.Rows[i]["txtWtPycnometer"].ToString();
                box4.Text = dt.Rows[i]["txtWtPycnometerPlusDistWater"].ToString();

            }
        }
        protected void GetCurrentDataEmptyPycnometer()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtNoOfPycnometer", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWtPycnometer", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWtPycnometerPlusDistWater", typeof(string)));

            for (int i = 0; i < grdWeightOfEmptyPycnometer.Rows.Count; i++)
            {
                TextBox box2 = (TextBox)grdWeightOfEmptyPycnometer.Rows[i].Cells[0].FindControl("txtNoOfPycnometer");
                TextBox box3 = (TextBox)grdWeightOfEmptyPycnometer.Rows[i].Cells[1].FindControl("txtWtPycnometer");
                TextBox box4 = (TextBox)grdWeightOfEmptyPycnometer.Rows[i].Cells[1].FindControl("txtWtPycnometerPlusDistWater");

                drRow = dtTable.NewRow();
                drRow["txtNoOfPycnometer"] = box2.Text;
                drRow["txtWtPycnometer"] = box3.Text;
                drRow["txtWtPycnometerPlusDistWater"] = box4.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["EmptyPycnometerTable"] = dtTable;

        }

        #endregion

        #region Machice Settings
        public void getMachineSettingsData()
        {

            var machineSettings_list = dc.SoilSetting_View("Machine Settings").ToList();

            if (machineSettings_list.Count() == 0)
            {

                //if remark is not present add empty row 
                DataTable dt1 = new DataTable();
                DataRow dr1 = null;
                dt1.Columns.Add(new DataColumn("txtMachineNo", typeof(string)));
                dt1.Columns.Add(new DataColumn("txtOperatorName", typeof(string)));
                dt1.Columns.Add(new DataColumn("txtOwnerName", typeof(string)));
                dr1 = dt1.NewRow();
                int srNo = 0;
                dr1["txtMachineNo"] = "DM " + (srNo + 1).ToString();
                dr1["txtOperatorName"] = string.Empty;
                dr1["txtOwnerName"] = string.Empty;
                dt1.Rows.Add(dr1);
                ViewState["MachineTable"] = dt1;
                grdMachineSettings.DataSource = dt1;
                grdMachineSettings.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("txtMachineNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtOperatorName", typeof(string)));
                dt.Columns.Add(new DataColumn("txtOwnerName", typeof(string)));

                DataRow dr = null;
                for (int i = 0; i < machineSettings_list.Count(); i++)
                {

                    dr = dt.NewRow();
                    dr["txtMachineNo"] = string.Empty;
                    dr["txtOperatorName"] = string.Empty;
                    dr["txtOwnerName"] = string.Empty;
                    dt.Rows.Add(dr);
                    ViewState["MachineTable"] = dt;

                }
                grdMachineSettings.DataSource = dt;
                grdMachineSettings.DataBind();
            }
            int cnt = 0;

            foreach (var w in machineSettings_list)
            {
                // AddNewRow();
                TextBox txtMachineNo = (TextBox)grdMachineSettings.Rows[cnt].FindControl("txtMachineNo");
                TextBox txtOperatorName = (TextBox)grdMachineSettings.Rows[cnt].FindControl("txtOperatorName");
                TextBox txtOwnerName = (TextBox)grdMachineSettings.Rows[cnt].FindControl("txtOwnerName");
                txtMachineNo.Text = w.SOSET_F1_var;
                txtOperatorName.Text = w.SOSET_F2_var;
                txtOwnerName.Text = w.SOSET_F3_var;
                cnt++;
            }
            lblRowMachineCount.Text = grdMachineSettings.Rows.Count.ToString();
        }
        protected void AddNewRowForMachineSettings()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MachineTable"] != null)
            {
                MachineGetCurrentData();
                dt = (DataTable)ViewState["MachineTable"];
            }
            else
            {

                dt.Columns.Add(new DataColumn("txtMachineNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtOperatorName", typeof(string)));
                dt.Columns.Add(new DataColumn("txtOwnerName", typeof(string)));

            }

            dr = dt.NewRow();
            dr["txtMachineNo"] = string.Empty;
            dr["txtOperatorName"] = string.Empty;
            dr["txtOwnerName"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["MachineTable"] = dt;
            grdMachineSettings.DataSource = dt;
            grdMachineSettings.DataBind();
            MachineSetPreviousData();

        }

        protected void MachineDeleteRow(int rowIndex)
        {
            int rowCount = Convert.ToInt32(lblRowMachineCount.Text);
            if (rowIndex > rowCount - 1)
            {
                MachineGetCurrentData();
                DataTable dt = ViewState["MachineTable"] as DataTable;
                dt.Rows[rowIndex].Delete();
                ViewState["MachineTable"] = dt;
                grdMachineSettings.DataSource = dt;
                grdMachineSettings.DataBind();
                MachineSetPreviousData();
            }
        }
        protected void MachineSetPreviousData()
        {
            DataTable dt = (DataTable)ViewState["MachineTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                TextBox txtMachineNo = (TextBox)grdMachineSettings.Rows[i].FindControl("txtMachineNo");
                TextBox txtOperatorName = (TextBox)grdMachineSettings.Rows[i].FindControl("txtOperatorName");
                TextBox txtOwnerName = (TextBox)grdMachineSettings.Rows[i].FindControl("txtOwnerName");

                txtMachineNo.Text = dt.Rows[i]["txtMachineNo"].ToString();
                txtOperatorName.Text = dt.Rows[i]["txtOperatorName"].ToString();
                txtOwnerName.Text = dt.Rows[i]["txtOwnerName"].ToString();
            }
        }
        protected void MachineGetCurrentData()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;


            dtTable.Columns.Add(new DataColumn("txtMachineNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtOperatorName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtOwnerName", typeof(string)));

            for (int i = 0; i < grdMachineSettings.Rows.Count; i++)
            {
                TextBox txtMachineNo = (TextBox)grdMachineSettings.Rows[i].FindControl("txtMachineNo");
                TextBox txtOperatorName = (TextBox)grdMachineSettings.Rows[i].FindControl("txtOperatorName");
                TextBox txtOwnerName = (TextBox)grdMachineSettings.Rows[i].FindControl("txtOwnerName");


                drRow = dtTable.NewRow();
                drRow["txtMachineNo"] = txtMachineNo.Text;
                drRow["txtOperatorName"] = txtOperatorName.Text;
                drRow["txtOwnerName"] = txtOwnerName.Text;
                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["MachineTable"] = dtTable;

        }
        protected void AddRowMachine_Click(object sender, EventArgs e)
        {
            AddNewRowForMachineSettings();
            ResetSrNo();
        }
        protected void ResetSrNo()
        {
            for (int i = 0; i < grdMachineSettings.Rows.Count; i++)
            {
                TextBox txtMachineNo = (TextBox)grdMachineSettings.Rows[i].FindControl("txtMachineNo");
                string srno = Convert.ToString(i + 1);
                txtMachineNo.Text = "DM " + srno;
            }
        }
        protected void DeleteRowMachine_Click(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
            int rowIndex = currentRow.RowIndex;
            MachineDeleteRow(rowIndex);
            ResetSrNo();
        }

        #endregion

        protected Boolean ValidateData()
        {
            string dispalyMsg = "";
            Boolean valid = true;

            //Witness by validation

            if (RdbGenericList.SelectedValue == "")
            {
                dispalyMsg = "Please select any settings option.";
                valid = false;
            }
            if (txt_From_Date.Text == "")
            {
                dispalyMsg = "Please select From Date.";
                valid = false;
            }
            else if (valid == true)
            {
                if (pnlSoilSettings.Visible == true)
                {
                    if (RadioButtonList1.SelectedValue == "Weight Of Container")
                    {

                        for (int j = 0; j < grdViewSettings.Rows.Count; j++)
                        {
                            TextBox txtContainerNo = (TextBox)grdViewSettings.Rows[grdViewSettings.Rows.Count - 1].Cells[0].FindControl("txtContainerNo");
                            TextBox txtContainerNo1 = (TextBox)grdViewSettings.Rows[j].Cells[0].FindControl("txtContainerNo");
                            if (j != grdViewSettings.Rows.Count - 1)
                            {
                                if (txtContainerNo.Text != "")
                                {
                                    if (txtContainerNo.Text == txtContainerNo1.Text)
                                    {
                                        dispalyMsg = "Duplicate Container Number " + txtContainerNo.Text + ".";
                                        txtContainerNo.Focus();
                                        valid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    dispalyMsg = "Enter Container No for row no " + (grdViewSettings.Rows.Count) + ".";
                                    txtContainerNo.Focus();
                                    valid = false;
                                    break;
                                }
                            }
                        }

                        if (valid == true)
                        {
                            for (int i = 0; i < grdViewSettings.Rows.Count; i++)
                            {
                                TextBox txtContainerNo = (TextBox)grdViewSettings.Rows[i].Cells[0].FindControl("txtContainerNo");
                                TextBox txtWtContainer = (TextBox)grdViewSettings.Rows[i].Cells[1].FindControl("txtWtContainer");


                                if (txtContainerNo.Text == "")
                                {
                                    dispalyMsg = "Enter Container No for row no " + (i + 1) + ".";
                                    txtContainerNo.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtWtContainer.Text == "")
                                {
                                    dispalyMsg = "Enter Wt of Container + Lid for row number " + (i + 1) + ".";
                                    txtWtContainer.Focus();
                                    valid = false;
                                    break;
                                }

                            }
                        }

                    }
                    else if (RadioButtonList1.SelectedValue == "Mould Volume")
                    {
                        for (int j = 0; j < GrdViewMould.Rows.Count; j++)
                        {
                            TextBox txtMouldNo = (TextBox)GrdViewMould.Rows[GrdViewMould.Rows.Count - 1].Cells[0].FindControl("txtMouldNo");
                            TextBox txtMouldNo1 = (TextBox)GrdViewMould.Rows[j].Cells[0].FindControl("txtMouldNo");
                            if (j != GrdViewMould.Rows.Count - 1)
                            {
                                if (txtMouldNo.Text != "")
                                {
                                    if (txtMouldNo.Text == txtMouldNo1.Text)
                                    {
                                        dispalyMsg = "Duplicate Mould Number " + txtMouldNo.Text + ".";
                                        txtMouldNo.Focus();
                                        valid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    dispalyMsg = "Enter Mould Number for row no " + (GrdViewMould.Rows.Count) + ".";
                                    txtMouldNo.Focus();
                                    valid = false;
                                    break;
                                }
                            }
                        }

                        if (valid == true)
                        {
                            for (int i = 0; i < GrdViewMould.Rows.Count; i++)
                            {
                                TextBox txtMouldNo = (TextBox)GrdViewMould.Rows[i].Cells[0].FindControl("txtMouldNo");
                                DropDownList txtVolume = (DropDownList)GrdViewMould.Rows[i].Cells[1].FindControl("txtVolume");

                                if (txtMouldNo.Text == "")
                                {
                                    dispalyMsg = "Enter Mould No for row no " + (i + 1) + ".";
                                    txtMouldNo.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtVolume.SelectedIndex <= 0)
                                {
                                    dispalyMsg = "Select Volume for row number " + (i + 1) + ".";
                                    txtVolume.Focus();
                                    valid = false;
                                    break;
                                }

                                if (RdbMouldVolume1.SelectedValue != "CBR")
                                {
                                    TextBox txtWeight = (TextBox)GrdViewMould.Rows[i].Cells[2].FindControl("txtWeight");
                                    if (txtWeight.Text == "")
                                    {
                                        dispalyMsg = "Enter Weight for row no " + (i + 1) + ".";
                                        txtWeight.Focus();
                                        valid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    TextBox txtHeight = (TextBox)GrdViewMould.Rows[i].Cells[3].FindControl("txtHeight");
                                    TextBox txtWeightOfMould = (TextBox)GrdViewMould.Rows[i].Cells[4].FindControl("txtWeightOfMould");
                                    if (txtHeight.Text == "")
                                    {
                                        dispalyMsg = "Enter Height for row no " + (i + 1) + ".";
                                        txtHeight.Focus();
                                        valid = false;
                                        break;
                                    }
                                    else if (txtWeightOfMould.Text == "")
                                    {
                                        dispalyMsg = "Enter Weight of empty mould for row number " + (i + 1) + ".";
                                        txtWeightOfMould.Focus();
                                        valid = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (RadioButtonList1.SelectedValue == "CBR Proving Ring")
                    {
                        for (int j = 0; j < GrdViewCBRProviding.Rows.Count; j++)
                        {
                            TextBox txtDeflection = (TextBox)GrdViewCBRProviding.Rows[GrdViewCBRProviding.Rows.Count - 1].Cells[0].FindControl("txtDeflection");
                            TextBox txtDeflection1 = (TextBox)GrdViewCBRProviding.Rows[j].Cells[0].FindControl("txtDeflection");
                            if (j != GrdViewCBRProviding.Rows.Count - 1)
                            {
                                if (txtDeflection.Text != "")
                                {
                                    if (txtDeflection.Text == txtDeflection1.Text)
                                    {
                                        dispalyMsg = "Duplicate Deflection Number " + txtDeflection1.Text + ".";
                                        txtDeflection.Focus();
                                        valid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    dispalyMsg = "Enter Deflection Number for row no " + (GrdViewCBRProviding.Rows.Count) + ".";
                                    txtDeflection.Focus();
                                    valid = false;
                                    break;
                                }
                            }
                        }
                        if (valid == true)
                        {
                            for (int i = 0; i < GrdViewCBRProviding.Rows.Count; i++)
                            {
                                TextBox txtDeflection = (TextBox)GrdViewCBRProviding.Rows[i].Cells[0].FindControl("txtDeflection");
                                TextBox txtLoad = (TextBox)GrdViewCBRProviding.Rows[i].Cells[1].FindControl("txtLoad");

                                if (txtDeflection.Text == "")
                                {
                                    dispalyMsg = "Enter Deflection for row no " + (i + 1) + ".";
                                    txtDeflection.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtLoad.Text == "")
                                {
                                    dispalyMsg = "Enter Load for row number " + (i + 1) + ".";
                                    txtLoad.Focus();
                                    valid = false;
                                    break;
                                }
                            }
                        }

                    }
                    else if (RadioButtonList1.SelectedValue == "Core Cutter Dimensions")
                    {

                        for (int j = 0; j < GrdViewCoreCutter.Rows.Count; j++)
                        {
                            TextBox txtCoreCutterNo = (TextBox)GrdViewCoreCutter.Rows[GrdViewCoreCutter.Rows.Count - 1].Cells[0].FindControl("txtCoreCutterNo");
                            TextBox txtCoreCutterNo1 = (TextBox)GrdViewCoreCutter.Rows[j].Cells[0].FindControl("txtCoreCutterNo");
                            if (j != GrdViewCoreCutter.Rows.Count - 1)
                            {
                                if (txtCoreCutterNo.Text != "")
                                {
                                    if (txtCoreCutterNo.Text == txtCoreCutterNo1.Text)
                                    {
                                        dispalyMsg = "Duplicate Core Cutter Number " + txtCoreCutterNo.Text + ".";
                                        txtCoreCutterNo.Focus();
                                        valid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    dispalyMsg = "Enter Core Cutter Number for row no " + (GrdViewCoreCutter.Rows.Count) + ".";
                                    txtCoreCutterNo.Focus();
                                    valid = false;
                                    break;
                                }
                            }
                        }
                        if (valid == true)
                        {

                            for (int i = 0; i < GrdViewCoreCutter.Rows.Count; i++)
                            {
                                TextBox txtCoreCutterNo = (TextBox)GrdViewCoreCutter.Rows[i].Cells[0].FindControl("txtCoreCutterNo");
                                TextBox txtWeight = (TextBox)GrdViewCoreCutter.Rows[i].Cells[1].FindControl("txtWeight");
                                TextBox txtHeight = (TextBox)GrdViewCoreCutter.Rows[i].Cells[2].FindControl("txtHeight");
                                TextBox txtDiameter = (TextBox)GrdViewCoreCutter.Rows[i].Cells[3].FindControl("txtDiameter");
                                TextBox txtVolume = (TextBox)GrdViewCoreCutter.Rows[i].Cells[4].FindControl("txtVolume");
                                TextBox txtActualVolume = (TextBox)GrdViewCoreCutter.Rows[i].Cells[5].FindControl("txtActualVolume");

                                if (txtCoreCutterNo.Text == "")
                                {
                                    dispalyMsg = "Enter Core Cutter No for row no " + (i + 1) + ".";
                                    txtCoreCutterNo.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtWeight.Text == "")
                                {
                                    dispalyMsg = "Enter Weight for row number " + (i + 1) + ".";
                                    txtWeight.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtHeight.Text == "")
                                {
                                    dispalyMsg = "Enter Height for row number " + (i + 1) + ".";
                                    txtHeight.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtDiameter.Text == "")
                                {
                                    dispalyMsg = "Enter Diameter for row number " + (i + 1) + ".";
                                    txtDiameter.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtVolume.Text == "")
                                {
                                    dispalyMsg = "Enter Volume for row number " + (i + 1) + ".";
                                    txtVolume.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtActualVolume.Text == "")
                                {
                                    dispalyMsg = "Enter Actual Volume for row number " + (i + 1) + ".";
                                    txtActualVolume.Focus();
                                    valid = false;
                                    break;
                                }

                            }
                        }
                    }
                    else if (RadioButtonList1.SelectedValue == "Calibration of Sand / Cylinder Cone")
                    {
                        for (int i = 0; i < GrdViewCalibrationSand.Rows.Count; i++)
                        {

                            TextBox txtOne = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[1].FindControl("txtOne");
                            TextBox txtTwo = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[2].FindControl("txtTwo");
                            TextBox txtThree = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[3].FindControl("txtThree");
                            TextBox txtFour = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[4].FindControl("txtFour");
                            TextBox txtFive = (TextBox)GrdViewCalibrationSand.Rows[i].Cells[5].FindControl("txtFive");

                            if (i != 9 && i != 10)
                            {
                                if (txtOne.Text == "")
                                {
                                    dispalyMsg = "Enter value for row no " + (i + 1) + ".";
                                    txtOne.Focus();
                                    valid = false;
                                    break;
                                }

                                else if (txtTwo.Text == "")
                                {
                                    dispalyMsg = "Enter value for row no " + (i + 1) + ".";
                                    txtTwo.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtThree.Text == "")
                                {
                                    dispalyMsg = "Enter value for row no " + (i + 1) + ".";
                                    txtThree.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtFour.Text == "")
                                {
                                    dispalyMsg = "Enter value for row no " + (i + 1) + ".";
                                    txtFour.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtFive.Text == "")
                                {
                                    dispalyMsg = "Enter value for row no " + (i + 1) + ".";
                                    txtFive.Focus();
                                    valid = false;
                                    break;
                                }
                            }

                        }
                    }
                    else if (RadioButtonList1.SelectedValue == "Weight of empty Dish")
                    {

                        for (int j = 0; j < grdWeightOfEmptyDish.Rows.Count; j++)
                        {
                            TextBox txtNoOfDish = (TextBox)grdWeightOfEmptyDish.Rows[grdWeightOfEmptyDish.Rows.Count - 1].Cells[0].FindControl("txtNoOfDish");
                            TextBox txtNoOfDish1 = (TextBox)grdWeightOfEmptyDish.Rows[j].Cells[0].FindControl("txtNoOfDish");
                            if (j != grdWeightOfEmptyDish.Rows.Count - 1)
                            {
                                if (txtNoOfDish.Text != "")
                                {
                                    if (txtNoOfDish.Text == txtNoOfDish1.Text)
                                    {
                                        dispalyMsg = "Duplicate No. of Dish " + txtNoOfDish.Text + ".";
                                        txtNoOfDish.Focus();
                                        valid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    dispalyMsg = "Enter No. of Dish for row no " + (grdWeightOfEmptyDish.Rows.Count) + ".";
                                    txtNoOfDish.Focus();
                                    valid = false;
                                    break;
                                }
                            }
                        }

                        if (valid == true)
                        {
                            for (int i = 0; i < grdWeightOfEmptyDish.Rows.Count; i++)
                            {
                                TextBox txtNoOfDish = (TextBox)grdWeightOfEmptyDish.Rows[i].Cells[0].FindControl("txtNoOfDish");
                                TextBox txtWtDish = (TextBox)grdWeightOfEmptyDish.Rows[i].Cells[1].FindControl("txtWtDish");

                                if (txtNoOfDish.Text == "")
                                {
                                    dispalyMsg = "Enter Container No for row no " + (i + 1) + ".";
                                    txtNoOfDish.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtWtDish.Text == "")
                                {
                                    dispalyMsg = "Enter Empty Dish weight for row number " + (i + 1) + ".";
                                    txtWtDish.Focus();
                                    valid = false;
                                    break;
                                }

                            }
                        }

                    }

                    else if (RadioButtonList1.SelectedValue == "Weight of empty Bottle")
                    {

                        for (int j = 0; j < grdWeightOfEmptyBottle.Rows.Count; j++)
                        {
                            TextBox txtNoOfBottle = (TextBox)grdWeightOfEmptyBottle.Rows[grdWeightOfEmptyBottle.Rows.Count - 1].Cells[0].FindControl("txtNoOfBottle");
                            TextBox txtNoOfBottle1 = (TextBox)grdWeightOfEmptyBottle.Rows[j].Cells[0].FindControl("txtNoOfBottle");
                            if (j != grdWeightOfEmptyBottle.Rows.Count - 1)
                            {
                                if (txtNoOfBottle.Text != "")
                                {
                                    if (txtNoOfBottle.Text == txtNoOfBottle1.Text)
                                    {
                                        dispalyMsg = "Duplicate No. of Bottle " + txtNoOfBottle.Text + ".";
                                        txtNoOfBottle.Focus();
                                        valid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    dispalyMsg = "Enter No. of Bottle for row no " + (grdWeightOfEmptyBottle.Rows.Count) + ".";
                                    txtNoOfBottle.Focus();
                                    valid = false;
                                    break;
                                }
                            }
                        }

                        if (valid == true)
                        {
                            for (int i = 0; i < grdWeightOfEmptyBottle.Rows.Count; i++)
                            {
                                TextBox txtNoOfBottle = (TextBox)grdWeightOfEmptyBottle.Rows[i].Cells[0].FindControl("txtNoOfBottle");
                                TextBox txtWtBottle = (TextBox)grdWeightOfEmptyBottle.Rows[i].Cells[1].FindControl("txtWtBottle");
                                TextBox txtWtBottlePlusDistWater = (TextBox)grdWeightOfEmptyBottle.Rows[i].Cells[2].FindControl("txtWtBottlePlusDistWater");

                                if (txtNoOfBottle.Text == "")
                                {
                                    dispalyMsg = "Enter Container No for row no " + (i + 1) + ".";
                                    txtNoOfBottle.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtWtBottle.Text == "")
                                {
                                    dispalyMsg = "Enter Empty Bottle weight for row number " + (i + 1) + ".";
                                    txtWtBottle.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtWtBottlePlusDistWater.Text == "")
                                {
                                    dispalyMsg = "Enter Wt. of Bottle + Dist. Water for row number " + (i + 1) + ".";
                                    txtWtBottlePlusDistWater.Focus();
                                    valid = false;
                                    break;
                                }

                            }
                        }

                    }
                    else if (RadioButtonList1.SelectedValue == "Weight of empty Pycnometer")
                    {

                        for (int j = 0; j < grdWeightOfEmptyPycnometer.Rows.Count; j++)
                        {
                            TextBox txtNoOfPycnometer = (TextBox)grdWeightOfEmptyPycnometer.Rows[grdWeightOfEmptyPycnometer.Rows.Count - 1].Cells[0].FindControl("txtNoOfPycnometer");
                            TextBox txtNoOfPycnometer1 = (TextBox)grdWeightOfEmptyPycnometer.Rows[j].Cells[0].FindControl("txtNoOfPycnometer");
                            if (j != grdWeightOfEmptyPycnometer.Rows.Count - 1)
                            {
                                if (txtNoOfPycnometer.Text != "")
                                {
                                    if (txtNoOfPycnometer.Text == txtNoOfPycnometer1.Text)
                                    {
                                        dispalyMsg = "Duplicate No. of Pycnometer " + txtNoOfPycnometer.Text + ".";
                                        txtNoOfPycnometer.Focus();
                                        valid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    dispalyMsg = "Enter No. of Pycnometer for row no " + (grdWeightOfEmptyPycnometer.Rows.Count) + ".";
                                    txtNoOfPycnometer.Focus();
                                    valid = false;
                                    break;
                                }
                            }
                        }

                        if (valid == true)
                        {
                            for (int i = 0; i < grdWeightOfEmptyPycnometer.Rows.Count; i++)
                            {
                                TextBox txtNoOfPycnometer = (TextBox)grdWeightOfEmptyPycnometer.Rows[i].Cells[0].FindControl("txtNoOfPycnometer");
                                TextBox txtWtPycnometer = (TextBox)grdWeightOfEmptyPycnometer.Rows[i].Cells[1].FindControl("txtWtPycnometer");
                                TextBox txtWtPycnometerPlusDistWater = (TextBox)grdWeightOfEmptyPycnometer.Rows[i].Cells[2].FindControl("txtWtPycnometerPlusDistWater");

                                if (txtNoOfPycnometer.Text == "")
                                {
                                    dispalyMsg = "Enter Container No for row no " + (i + 1) + ".";
                                    txtNoOfPycnometer.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtWtPycnometer.Text == "")
                                {
                                    dispalyMsg = "Enter Empty Pycnometer weight for row number " + (i + 1) + ".";
                                    txtWtPycnometer.Focus();
                                    valid = false;
                                    break;
                                }
                                else if (txtWtPycnometerPlusDistWater.Text == "")
                                {
                                    dispalyMsg = "Enter Wt. of Pycnometer + Dist. Water for row number " + (i + 1) + ".";
                                    txtWtPycnometerPlusDistWater.Focus();
                                    valid = false;
                                    break;
                                }

                            }
                        }

                    }
                }
                else if (pnlMachineSettings.Visible == true)
                {
                    if (RdbGenericList.SelectedValue == "Machine Settings")
                    {
                        for (int i = 0; i < grdMachineSettings.Rows.Count; i++)
                        {
                            TextBox txtOperatorName = (TextBox)grdMachineSettings.Rows[i].FindControl("txtOperatorName");
                            TextBox txtOwnerName = (TextBox)grdMachineSettings.Rows[i].FindControl("txtOwnerName");
                            if (txtOperatorName.Text == "")
                            {
                                dispalyMsg = "Enter Operator Name for row no " + (i + 1) + ".";
                                txtOperatorName.Focus();
                                valid = false;
                                break;
                            }
                            else if (txtOwnerName.Text == "")
                            {
                                dispalyMsg = "Enter Owner Name for row no " + (i + 1) + ".";
                                txtOwnerName.Focus();
                                valid = false;
                                break;
                            }
                        }
                    }
                }
            }
            if (valid == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
            return valid;
        }

        
    }
}


