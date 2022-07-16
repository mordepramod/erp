using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DESPLWEB
{
    public partial class Tile_Report : System.Web.UI.Page
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
                    txt_Rectype.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[1].Split('=');
                    lblRecordNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[2].Split('=');
                    txtRefNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[3].Split('=');
                    lblStatus.Text = arrIndMsg[1].ToString().Trim();
                }

                Label lblheading = (Label)Master.FindControl("lblheading");
                
                txtEntdChkdBy.Text = Session["LoginUserName"].ToString();
                // txtRefNo.Text = Session["ReferenceNo"].ToString();

                if (lblStatus.Text == "Enter")
                {
                    lblStatus.Text = "Enter";
                    lblheading.Text = "Tile - Report Entry";
                }
                else if (lblStatus.Text == "Check")
                {
                    lblStatus.Text = "Check";
                    lblheading.Text = "Tile - Report Checking";
                }
                            
                LoadReferenceNoList();
                LoadApprovedBy();
                DisplayTileTestDetails();
            }
        }

        #region private Methods
        private void LoadReferenceNoList()
        {
            byte reportStatus = 0;
            if (lblStatus.Text == "Enter")
                reportStatus = 1;
            else if (lblStatus.Text == "Check")
                reportStatus = 2;

            var reportList = dc.TileInward_View("", reportStatus);
            ddlRefNo.DataTextField = "TILEINWD_ReferenceNo_var";
            ddlRefNo.DataSource = reportList;
            ddlRefNo.DataBind();
            ddlRefNo.Items.Insert(0, new ListItem("---Select---", "0"));
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
            var apprUser = dc.ReportStatus_View("", null, null, 0, testBit, apprBit, "", 0, 0,0);
            ddlTestdApprdBy.DataSource = apprUser;
            ddlTestdApprdBy.DataBind();
            ddlTestdApprdBy.Items.Insert(0, "---Select---");
        }
        private void ClearData()
        {
            TabRemark.Visible = true;
            grdCR.DataSource = null;
            grdCR.DataBind();
            grdWA.DataSource = null;
            grdWA.DataBind();
            grdSH.DataSource = null;
            grdSH.DataBind();
            grdWT.DataSource = null;
            grdWT.DataBind();
            grdWTCal.DataSource = null;
            grdWTCal.DataBind();
            grdMR.DataSource = null;
            grdMR.DataBind();
            grdMRCal.DataSource = null;
            grdMRCal.DataBind();
            grdDA.DataSource = null;
            grdDA.DataBind();
            grdDACal.DataSource = null;
            grdDACal.DataBind();
            grdRemark.DataSource = null;
            grdRemark.DataBind();
            grdSpecifiedLimit.DataSource = null;
            grdSpecifiedLimit.DataBind();
            chkWitnessBy.Checked = false;
            txtWitnesBy.Visible = false;
            txtWitnesBy.Text = "";
            ddlTestdApprdBy.SelectedIndex = 0;
            lnkSave.Enabled = true;
            lnkCalculate.Enabled = true;
            ViewState["CRTable"] = null;
            ViewState["MRTable"] = null;
            ViewState["MRCalTable"] = null;
            ViewState["SHTable"] = null;
            ViewState["WATable"] = null;
            ViewState["WTTable"] = null;
            ViewState["WTCalTable"] = null;
            ViewState["DATable"] = null;
            ViewState["DACalTable"] = null;
            ViewState["RemarkTable"] = null;
            ViewState["SpecifiedLimit"] = null;
        }
        private void DisplayTileTestDetails()
        {
            //Tile Inward details
            var tileData = dc.TileInward_View(txtRefNo.Text, 0);

            int rowNo = 0;
            foreach (var tileDetails in tileData)
            {
                txtRefNo.Text = tileDetails.TILEINWD_ReferenceNo_var.ToString();
                if (tileDetails.TILEINWD_TestedDate_dt == null)
                    txtDateOfTest.Text = DateTime.Now.ToString("dd/MM/yyyy");
                else
                    txtDateOfTest.Text = Convert.ToDateTime(tileDetails.TILEINWD_TestedDate_dt).ToString("dd/MM/yyyy");
                if (txtDateOfTest.Text == "" || lblStatus.Text == "Enter")
                {
                    txtDateOfTest.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (tileDetails.TILEINWD_RptTileType_var != null)
                {
                    ddlTileType.SelectedValue = tileDetails.TILEINWD_RptTileType_var;
                }
                else
                {
                    ddlTileType.SelectedValue = tileDetails.TILEINWD_TileType_var;
                }
                txtSupplierName.Text = tileDetails.TILEINWD_SupplierName_var;
                txtDesc.Text = tileDetails.TILEINWD_Description_var;

                if (ddl_NablScope.Items.FindByValue(tileDetails.TILEINWD_NablScope_var) != null)
                {
                    ddl_NablScope.SelectedValue = Convert.ToString(tileDetails.TILEINWD_NablScope_var);
                }
                if (Convert.ToString(tileDetails.TILEINWD_NablLocation_int) != null && Convert.ToString(tileDetails.TILEINWD_NablLocation_int) != "")
                {
                    ddl_NABLLocation.SelectedValue = Convert.ToString(tileDetails.TILEINWD_NablLocation_int);
                }


                txtWitnesBy.Text = tileDetails.TILEINWD_WitnessBy_var;
                if (txtWitnesBy.Text != "")
                {
                    chkWitnessBy.Checked = true;
                    txtWitnesBy.Visible = true;
                }
                if (tileDetails.TILEINWD_Status_tint == 1)
                {
                    lblEntdChkdBy.Text = "Entered By";
                    lblTestdApprdBy.Text = "Tested By";
                }
                else if (tileDetails.TILEINWD_Status_tint == 2)
                {
                    lblEntdChkdBy.Text = "Checked By";
                    lblTestdApprdBy.Text = "Approved By";
                }

                if (tileDetails.TEST_Sr_No == 1)
                {
                    #region  display Dimension Analysis data

                    TabDA.Visible = true;
                    TabSpecifiedLimit.Visible = true;
                    TabWA.Visible = false;
                    TabWT.Visible = false;
                    TabCR.Visible = false;
                    TabMR.Visible = false;
                    TabSH.Visible = false;
                    txtDAQuantity.Text = tileDetails.TILEINWD_Quantity_tint.ToString();

                    var Tiledata = dc.TileDA_View(txtRefNo.Text);
                    rowNo = 0;
                    foreach (var tile in Tiledata)
                    {
                        AddRowDA();
                        TextBox txtDAIdMark = (TextBox)grdDA.Rows[rowNo].FindControl("txtDAIdMark");
                        TextBox txtL1 = (TextBox)grdDA.Rows[rowNo].FindControl("txtL1");
                        TextBox txtL2 = (TextBox)grdDA.Rows[rowNo].FindControl("txtL2");
                        TextBox txtL3 = (TextBox)grdDA.Rows[rowNo].FindControl("txtL3");
                        TextBox txtW1 = (TextBox)grdDA.Rows[rowNo].FindControl("txtW1");
                        TextBox txtW2 = (TextBox)grdDA.Rows[rowNo].FindControl("txtW2");
                        TextBox txtW3 = (TextBox)grdDA.Rows[rowNo].FindControl("txtW3");
                        TextBox txtT1 = (TextBox)grdDA.Rows[rowNo].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdDA.Rows[rowNo].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdDA.Rows[rowNo].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdDA.Rows[rowNo].FindControl("txtT4");
                        //TextBox txtT5 = (TextBox)grdDA.Rows[rowNo].FindControl("txtT5");
                        //TextBox txtT6 = (TextBox)grdDA.Rows[rowNo].FindControl("txtT6");
                        //TextBox txtT7 = (TextBox)grdDA.Rows[rowNo].FindControl("txtT7");
                        //TextBox txtT8 = (TextBox)grdDA.Rows[rowNo].FindControl("txtT8");
                        //TextBox txtT9 = (TextBox)grdDA.Rows[rowNo].FindControl("txtT9");
                        //TextBox txtT10 = (TextBox)grdDA.Rows[rowNo].FindControl("txtT10");
                        //TextBox txtT11 = (TextBox)grdDA.Rows[rowNo].FindControl("txtT11");
                        //TextBox txtT12 = (TextBox)grdDA.Rows[rowNo].FindControl("txtT12");

                        txtDAIdMark.Text = tile.TILEDA_IdMark_var;
                        string[] strData = tile.TILEDA_Length_var.Split('|');
                        txtL1.Text = strData[0];
                        txtL2.Text = strData[1];
                        txtL3.Text = strData[2];
                        strData = tile.TILEDA_Width_var.Split('|');
                        txtW1.Text = strData[0];
                        txtW2.Text = strData[1];
                        txtW3.Text = strData[2];
                        strData = tile.TILEDA_Thickness_var.Split('|');
                        txtT1.Text = strData[0];
                        txtT2.Text = strData[1];
                        txtT3.Text = strData[2];
                        txtT4.Text = strData[3];
                        //txtT5.Text = strData[4];
                        //txtT6.Text = strData[5];
                        //txtT7.Text = strData[6];
                        //txtT8.Text = strData[7];
                        //txtT9.Text = strData[8];
                        //txtT10.Text = strData[9];
                        //txtT11.Text = strData[10];
                        //txtT12.Text = strData[11];

                        AddRowDACal();

                        TextBox txtDACalIdMark = (TextBox)grdDACal.Rows[rowNo].FindControl("txtDACalIdMark");
                        TextBox txtLength = (TextBox)grdDACal.Rows[rowNo].FindControl("txtLength");
                        TextBox txtWidth = (TextBox)grdDACal.Rows[rowNo].FindControl("txtWidth");
                        TextBox txtThickness = (TextBox)grdDACal.Rows[rowNo].FindControl("txtThickness");
                        TextBox txtAvgLen = (TextBox)grdDACal.Rows[rowNo].FindControl("txtAvgLen");
                        TextBox txtAvgWidth = (TextBox)grdDACal.Rows[rowNo].FindControl("txtAvgWidth");
                        TextBox txtAvgThickness = (TextBox)grdDACal.Rows[rowNo].FindControl("txtAvgThickness");

                        txtDACalIdMark.Text = tile.TILEDA_IdMark_var;
                        txtLength.Text = tile.TILEDA_AvgLength_dec.ToString();
                        txtWidth.Text = tile.TILEDA_AvgWidth_dec.ToString();
                        txtThickness.Text = tile.TILEDA_AvgThickness_dec.ToString();
                        if (tile.TILEDA_Average_var != "" && tile.TILEDA_Average_var != null)
                        {
                            strData = tile.TILEDA_Average_var.Split('|');
                            txtAvgLen.Text = strData[0];
                            txtAvgWidth.Text = strData[1];
                            txtAvgThickness.Text = strData[2];
                        }
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        DARowsChanged();
                        for (int i = 0; i < grdDACal.Rows.Count; i++)
                        {
                            TextBox txtDACalIdMark = (TextBox)grdDACal.Rows[i].FindControl("txtDACalIdMark");
                            txtDACalIdMark.Text = tileDetails.TILEINWD_IdMark_var;
                        }
                    }
                    else
                    {
                        txtDAQuantity.Text = rowNo.ToString();
                    }
                    #endregion
                }
                else if (tileDetails.TEST_Sr_No == 2)
                {
                    #region display Water Absorption data

                    TabWA.Visible = true;
                    TabSpecifiedLimit.Visible = true;
                    TabMR.Visible = false;
                    TabCR.Visible = false;
                    TabDA.Visible = false;
                    TabWT.Visible = false;
                    TabSH.Visible = false;
                    lblIsCode.Visible = true;
                    txtIsCode.Visible = true;

                    try
                    {
                        txtIsCode.Text = tileDetails.TILEINWD_ReportDetails_var;
                    }
                    catch { }

                    rowNo = 0;
                    txtWAQuantity.Text = tileDetails.TILEINWD_Quantity_tint.ToString();

                    var WADetails = dc.TileWA_View(txtRefNo.Text).ToList();
                    foreach (var WAData in WADetails)
                    {
                        AddRowWA();
                        TextBox txtWAIdMark = (TextBox)grdWA.Rows[rowNo].FindControl("txtWAIdMark");
                        TextBox txtWADryWt = (TextBox)grdWA.Rows[rowNo].FindControl("txtWADryWt");
                        TextBox txtWAWetWt = (TextBox)grdWA.Rows[rowNo].FindControl("txtWAWetWt");
                        TextBox txtWA = (TextBox)grdWA.Rows[rowNo].FindControl("txtWA");
                        TextBox txtWAAvg = (TextBox)grdWA.Rows[rowNo].FindControl("txtWAAvg");

                        txtWAIdMark.Text = WAData.TILEWA_IdMark_var;
                        txtWADryWt.Text = WAData.TILEWA_DryWt_dec.ToString();
                        txtWAWetWt.Text = WAData.TILEWA_WetWt_dec.ToString();
                        txtWA.Text = WAData.TILEWA_WaterAbsorption_dec.ToString();
                        txtWAAvg.Text = WAData.TILEWA_Average_var.ToString();
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        WARowsChanged();
                        for (int i = 0; i < grdWA.Rows.Count; i++)
                        {
                            TextBox txtWAIdMark = (TextBox)grdWA.Rows[i].FindControl("txtWAIdMark");
                            txtWAIdMark.Text = tileDetails.TILEINWD_IdMark_var;
                        }
                    }
                    else
                    {
                        txtWAQuantity.Text = rowNo.ToString();
                    }
                    #endregion
                }
                else if (tileDetails.TEST_Sr_No == 3)
                {
                    #region  display Modulus of Rupture data
                    TabMR.Visible = true;
                    TabSpecifiedLimit.Visible = true;
                    TabCR.Visible = false;
                    TabDA.Visible = false;
                    TabWA.Visible = false;
                    TabWT.Visible = false;
                    TabSH.Visible = false;
                    txtMRQuantity.Text = tileDetails.TILEINWD_Quantity_tint.ToString();
                    RdbMRMachine.SelectedValue = "Mechanical Flexture Machine";
                    lblIsCode.Visible = true;
                    txtIsCode.Visible = true;

                    try
                    {
                        string[] reportDt = tileDetails.TILEINWD_ReportDetails_var.Split('|');

                        RdbMRMachine.SelectedValue = reportDt[0];
                        txtMORL.Text = reportDt[1];
                        txtMORB.Text = reportDt[2];
                        txtDiameterOfRod.Text = reportDt[3];
                        txtThicknessOfRubber.Text = reportDt[4];
                        txtOverlapBeyondEdge.Text = reportDt[5];
                        txtIsCode.Text = reportDt[6];
                    }
                    catch { }

                    var MRTiledata = dc.TileMOR_View(txtRefNo.Text);
                    rowNo = 0;
                    foreach (var tile in MRTiledata)
                    {
                        AddRowMR();
                        TextBox txtMRIdMark = (TextBox)grdMR.Rows[rowNo].FindControl("txtMRIdMark");
                        TextBox txtT1 = (TextBox)grdMR.Rows[rowNo].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdMR.Rows[rowNo].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdMR.Rows[rowNo].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdMR.Rows[rowNo].FindControl("txtT4");
                        TextBox txtT5 = (TextBox)grdMR.Rows[rowNo].FindControl("txtT5");
                        TextBox txtT6 = (TextBox)grdMR.Rows[rowNo].FindControl("txtT6");
                        TextBox txtMRLeadShots = (TextBox)grdMR.Rows[rowNo].FindControl("txtMRLeadShots");

                        txtMRIdMark.Text = tile.TILEMOR_IdMark_var;
                        string[] strData = tile.TILEMOR_Thickness_var.Split('|');
                        txtT1.Text = strData[0];
                        txtT2.Text = strData[1];
                        txtT3.Text = strData[2];
                        txtT4.Text = strData[3];
                        txtT5.Text = strData[4];
                        txtT6.Text = strData[5];
                        txtMRLeadShots.Text = tile.TILEMOR_WtOfLead_dec.ToString();

                        AddRowMRCal();

                        TextBox txtMRCalIdMark = (TextBox)grdMRCal.Rows[rowNo].FindControl("txtMRCalIdMark");
                        TextBox txtMRThickness = (TextBox)grdMRCal.Rows[rowNo].FindControl("txtMRThickness");
                        TextBox txtMRCalLeadShots = (TextBox)grdMRCal.Rows[rowNo].FindControl("txtMRCalLeadShots");
                        TextBox txtMRB = (TextBox)grdMRCal.Rows[rowNo].FindControl("txtMRB");
                        TextBox txtMRN = (TextBox)grdMRCal.Rows[rowNo].FindControl("txtMRN");
                        TextBox txtAvgBreakLoad = (TextBox)grdMRCal.Rows[rowNo].FindControl("txtAvgBreakLoad");
                        TextBox txtBreakStrength = (TextBox)grdMRCal.Rows[rowNo].FindControl("txtBreakStrength");
                        TextBox txtAvgBreakStrength = (TextBox)grdMRCal.Rows[rowNo].FindControl("txtAvgBreakStrength");
                        TextBox txtMRTransverse = (TextBox)grdMRCal.Rows[rowNo].FindControl("txtMRTransverse");
                        TextBox txtMOR = (TextBox)grdMRCal.Rows[rowNo].FindControl("txtMOR");
                        TextBox txtAvgMOR = (TextBox)grdMRCal.Rows[rowNo].FindControl("txtAvgMOR");

                        txtMRCalIdMark.Text = tile.TILEMOR_IdMark_var;
                        txtMRThickness.Text = tile.TILEMOR_AvgThickness_dec.ToString();
                        txtMRCalLeadShots.Text = tile.TILEMOR_WtOfLead_dec.ToString();
                        txtMRB.Text = tile.TILEMOR_B_dec.ToString();
                        txtMRN.Text = tile.TILEMOR_N_dec.ToString();
                        txtMRTransverse.Text = tile.TILEMOR_TransverseStrength_dec.ToString();
                        txtBreakStrength.Text = tile.TILEMOR_BreakingStrength_dec.ToString();
                        txtMOR.Text = tile.TILEMOR_MOR_dec.ToString();
                        string[] avgData = tile.TILEMOR_Average_var.Split('|');
                        txtAvgBreakLoad.Text = avgData[0];
                        txtAvgBreakStrength.Text = avgData[1];
                        txtAvgMOR.Text = avgData[2];
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        MRRowsChanged();
                        for (int i = 0; i < grdMRCal.Rows.Count; i++)
                        {
                            TextBox txtMRCalIdMark = (TextBox)grdMRCal.Rows[i].FindControl("txtMRCalIdMark");
                            txtMRCalIdMark.Text = tileDetails.TILEINWD_IdMark_var;
                        }
                    }
                    else
                    {
                        txtMRQuantity.Text = rowNo.ToString();
                    }
                    #endregion
                }
                else if (tileDetails.TEST_Sr_No == 4)
                {
                    #region display Crazing Resistance data
                    TabCR.Visible = true;
                    TabSpecifiedLimit.Visible = false;
                    TabDA.Visible = false;
                    TabWA.Visible = false;
                    TabWT.Visible = false;
                    TabMR.Visible = false;
                    TabSH.Visible = false;
                    rowNo = 0;
                    txtQuantity.Text = tileDetails.TILEINWD_Quantity_tint.ToString();
                    //ddlTestdApprdBy.SelectedValue =tileDetails.TILEINWD_TestedBy_tint.ToString();
                    //txtEntdChkdBy.Text = tileDetails.TILEINWD_EnteredBy_tint.ToString();
                    var CRDetails = dc.TileCR_View(txtRefNo.Text).ToList();
                    foreach (var crData in CRDetails)
                    {
                        AddRowCR();
                        TextBox txtIdMark = (TextBox)grdCR.Rows[rowNo].FindControl("txtIdMark");
                        DropDownList ddlCrazing = (DropDownList)grdCR.Rows[rowNo].FindControl("ddlCrazing");
                        txtIdMark.Text = crData.TILECR_IdMark_var;
                        if (crData.TILECR_Crazing_var == "Observed") ddlCrazing.SelectedIndex = 1;
                        else if (crData.TILECR_Crazing_var == "Not Observed") ddlCrazing.SelectedIndex = 2;
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        CRRowsChanged();
                        for (int i = 0; i < grdCR.Rows.Count; i++)
                        {
                            TextBox txtIdMark = (TextBox)grdCR.Rows[i].FindControl("txtIdMark");
                            txtIdMark.Text = tileDetails.TILEINWD_IdMark_var;
                        }
                    }
                    else
                    {
                        txtQuantity.Text = rowNo.ToString();
                    }
                    #endregion
                }
                else if (tileDetails.TEST_Sr_No == 5)
                {
                    #region  display Wet Transverse data

                    TabWT.Visible = true;
                    TabSpecifiedLimit.Visible = false;
                    TabWA.Visible = false;
                    TabMR.Visible = false;
                    TabCR.Visible = false;
                    TabDA.Visible = false;
                    TabSH.Visible = false;
                    RdbMachine.SelectedValue = "Mechanical Flexture Machine";
                    txtWTQuantity.Text = tileDetails.TILEINWD_Quantity_tint.ToString();
                    try
                    {
                        string[] reportDt = tileDetails.TILEINWD_ReportDetails_var.Split('|');
                        RdbMachine.SelectedValue = reportDt[0];
                        txtL.Text = reportDt[1];
                        txtB.Text = reportDt[2];
                    }
                    catch
                    {
                    }

                    var WTTiledata = dc.TileWT_View(txtRefNo.Text);
                    rowNo = 0;
                    foreach (var tile in WTTiledata)
                    {

                        AddRowWT();
                        TextBox txtWTIdMark = (TextBox)grdWT.Rows[rowNo].FindControl("txtWTIdMark");
                        TextBox txtT1 = (TextBox)grdWT.Rows[rowNo].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdWT.Rows[rowNo].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdWT.Rows[rowNo].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdWT.Rows[rowNo].FindControl("txtT4");
                        TextBox txtT5 = (TextBox)grdWT.Rows[rowNo].FindControl("txtT5");
                        TextBox txtT6 = (TextBox)grdWT.Rows[rowNo].FindControl("txtT6");
                        TextBox txtWtLeadShots = (TextBox)grdWT.Rows[rowNo].FindControl("txtWtLeadShots");


                        txtWTIdMark.Text = tile.TILEWT_IdMark_var;
                        string[] strData = tile.TILEWT_Thickness_var.Split('|');
                        txtT1.Text = strData[0];
                        txtT2.Text = strData[1];
                        txtT3.Text = strData[2];
                        txtT4.Text = strData[3];
                        txtT5.Text = strData[4];
                        txtT6.Text = strData[5];
                        txtWtLeadShots.Text = tile.TILEWT_WtOfLead_dec.ToString();

                        AddRowWTCal();

                        TextBox txtWTCalIdMark = (TextBox)grdWTCal.Rows[rowNo].FindControl("txtWTCalIdMark");
                        TextBox txtWTThickness = (TextBox)grdWTCal.Rows[rowNo].FindControl("txtWTThickness");
                        TextBox txtWtCalLeadShots = (TextBox)grdWTCal.Rows[rowNo].FindControl("txtWtCalLeadShots");
                        TextBox txtWtB = (TextBox)grdWTCal.Rows[rowNo].FindControl("txtWtB");
                        TextBox txtWtN = (TextBox)grdWTCal.Rows[rowNo].FindControl("txtWtN");
                        TextBox txtWtTransverse = (TextBox)grdWTCal.Rows[rowNo].FindControl("txtWtTransverse");
                        TextBox txtWetTr = (TextBox)grdWTCal.Rows[rowNo].FindControl("txtWetTr");
                        TextBox txtAvgWetTr = (TextBox)grdWTCal.Rows[rowNo].FindControl("txtAvgWetTr");

                        txtWTCalIdMark.Text = tile.TILEWT_IdMark_var;
                        txtWTThickness.Text = tile.TILEWT_AvgThickness_dec.ToString();
                        txtWtCalLeadShots.Text = tile.TILEWT_WtOfLead_dec.ToString();
                        txtWtB.Text = tile.TILEWT_B_dec.ToString();
                        txtWtN.Text = tile.TILEWT_N_dec.ToString();
                        txtWtTransverse.Text = tile.TILEWT_TransverseStrength_dec.ToString();
                        txtWetTr.Text = tile.TILEWT_WetTransverse_dec.ToString();
                        if (tile.TILEWT_Average_var != "" && tile.TILEWT_Average_var != null)
                        {
                            txtAvgWetTr.Text = tile.TILEWT_Average_var;
                        }
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        WTRowsChanged();
                        for (int i = 0; i < grdWTCal.Rows.Count; i++)
                        {
                            TextBox txtWTCalIdMark = (TextBox)grdWTCal.Rows[i].FindControl("txtWTCalIdMark");
                            txtWTCalIdMark.Text = tileDetails.TILEINWD_IdMark_var;
                        }
                    }
                    else
                    {
                        txtWTQuantity.Text = rowNo.ToString();
                    }
                    #endregion
                }
                else if (tileDetails.TEST_Sr_No == 7)
                {
                    #region display Scratch Hardness data
                    TabSH.Visible = true;
                    TabSpecifiedLimit.Visible = false;
                    TabWT.Visible = false;
                    TabWA.Visible = false;
                    TabMR.Visible = false;
                    TabCR.Visible = false;
                    TabDA.Visible = false;

                    var scracthTileData = dc.TileInward_View(txtRefNo.Text, 0).ToList();

                    rowNo = 0;
                    txtSHQuantity.Text = tileDetails.TILEINWD_Quantity_tint.ToString();

                    var ScratchDetails = dc.TileSH_View(txtRefNo.Text).ToList();
                    foreach (var ScratchData in ScratchDetails)
                    {
                        AddRowSH();

                        TextBox txtSHIdMark = (TextBox)grdSH.Rows[rowNo].FindControl("txtSHIdMark");
                        TextBox txtHardness = (TextBox)grdSH.Rows[rowNo].FindControl("txtHardness");

                        txtSHIdMark.Text = ScratchData.TILESH_IdMark_var;
                        txtHardness.Text = ScratchData.TILESH_ScratchHardness_var;
                        rowNo++;
                    }
                    if (rowNo == 0)
                    {
                        SHRowsChanged();
                        for (int i = 0; i < grdSH.Rows.Count; i++)
                        {
                            TextBox txtSHIdMark = (TextBox)grdSH.Rows[i].FindControl("txtSHIdMark");
                            txtSHIdMark.Text = tileDetails.TILEINWD_IdMark_var;
                        }
                    }
                    else
                    {
                        txtSHQuantity.Text = rowNo.ToString();
                    }

                    #endregion
                }
            }

            #region Remark details
            rowNo = 0;
            var remark = dc.TileRemarkDetail_View(txtRefNo.Text);
            foreach (var rem in remark)
            {
                AddRowRemark();
                TextBox txtRemark = (TextBox)grdRemark.Rows[rowNo].FindControl("txtRemark");
                txtRemark.Text = rem.TILEREM_Remark_var;
                rowNo++;
            }
            if (rowNo == 0)
                AddRowRemark();
            #endregion

            #region Specified Limits details
            string strRefNo = "";
            if (lblStatus.Text != "Enter")
            {
                strRefNo = txtRefNo.Text;
            }
            if (TabDA.Visible == true)
            {
                rowNo = 0;
                grdSpecifiedLimit.Columns[3].Visible = false;                
                grdSpecifiedLimit.Columns[4].Visible = true;
                grdSpecifiedLimit.Columns[5].Visible = true;
                grdSpecifiedLimit.Columns[6].Visible = true;
                ChkSpecifiedLimits.Checked = false;
                var SpecLimitsDA = dc.TileSpecLimitsDA_View(strRefNo).ToList();
                foreach (var speciData in SpecLimitsDA)
                {
                    AddRowSpecifiedLimit();
                    TextBox txtSpeciDescription = (TextBox)grdSpecifiedLimit.Rows[rowNo].FindControl("txtSpeciDescription");
                    TextBox txtSpeciLength = (TextBox)grdSpecifiedLimit.Rows[rowNo].FindControl("txtSpeciLength");
                    TextBox txtSpeciWidth = (TextBox)grdSpecifiedLimit.Rows[rowNo].FindControl("txtSpeciWidth");
                    TextBox txtSpeciThickness = (TextBox)grdSpecifiedLimit.Rows[rowNo].FindControl("txtSpeciThickness");

                    txtSpeciDescription.Text = speciData.TILESPLDA_Description_var;
                    txtSpeciLength.Text = speciData.TILESPLDA_Length_var;
                    txtSpeciWidth.Text = speciData.TILESPLDA_Width_var;
                    txtSpeciThickness.Text = speciData.TILESPLDA_Thickness_var;
                    rowNo++;
                }

                if (rowNo == 0)
                    AddRowSpecifiedLimit();
            }
            else if (TabWA.Visible == true)
            {
                rowNo = 0;
                grdSpecifiedLimit.Columns[3].Visible = false;
                grdSpecifiedLimit.Columns[4].Visible = false;
                grdSpecifiedLimit.Columns[5].Visible = false;
                grdSpecifiedLimit.Columns[6].Visible = false;
                ChkSpecifiedLimits.Checked = false;
                var SpecLimitsWA = dc.TileSpecLimitsWA_View(strRefNo);
                foreach (var specData in SpecLimitsWA)
                {
                    AddRowSpecifiedLimit();

                    TextBox txtSpeciDescription = (TextBox)grdSpecifiedLimit.Rows[rowNo].FindControl("txtSpeciDescription");
                    txtSpeciDescription.Text = specData.TILESPLWA_Description_var;
                    rowNo++;
                }
                if (rowNo == 0)
                    AddRowSpecifiedLimit();
            }
            else if (TabMR.Visible == true)
            {
                rowNo = 0;
                ChkSpecifiedLimits.Checked = false;
                grdSpecifiedLimit.Columns[3].Visible = true;
                grdSpecifiedLimit.Columns[4].Visible = false;
                grdSpecifiedLimit.Columns[5].Visible = false;
                grdSpecifiedLimit.Columns[6].Visible = false;
                var SpecLimitsMR = dc.TileSpecLimitsMOR_View(strRefNo);
                foreach (var specData in SpecLimitsMR)
                {
                    AddRowSpecifiedLimit();                    
                   
                    TextBox txtSpeciDescription = (TextBox)grdSpecifiedLimit.Rows[rowNo].FindControl("txtSpeciDescription");
                    TextBox txtSpeciLimits = (TextBox)grdSpecifiedLimit.Rows[rowNo].FindControl("txtSpeciLimits");
                    txtSpeciDescription.Text = specData.TILESPLMOR_Description_var;
                    txtSpeciLimits.Text = specData.TILESPLMOR_Limits_var;
                    rowNo++;
                }
                if (rowNo == 0)
                    AddRowSpecifiedLimit();
            }

            #endregion

            if (TabCR.Visible == true)
                TabContainerTile.ActiveTabIndex = 0;
            else if (TabSH.Visible == true)
                TabContainerTile.ActiveTabIndex = 1;
            else if (TabWA.Visible == true)
                TabContainerTile.ActiveTabIndex = 2;
            else if (TabWT.Visible == true)
                TabContainerTile.ActiveTabIndex = 3;
            else if (TabMR.Visible == true)
                TabContainerTile.ActiveTabIndex = 4;
            else if (TabDA.Visible == true)
                TabContainerTile.ActiveTabIndex = 5;
            else if (TabRemark.Visible == true)
                TabContainerTile.ActiveTabIndex = 6;
        }

        #endregion

        #region add/delete rows grdCR grid
        private void CRRowsChanged()
        {
            if (txtQuantity.Text != "")
            {
                if (Convert.ToInt32(txtQuantity.Text) < grdCR.Rows.Count)
                {
                    for (int i = grdCR.Rows.Count; i > Convert.ToInt32(txtQuantity.Text); i--)
                    {
                        if (grdCR.Rows.Count > 1)
                        {
                            DeleteRowCR(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtQuantity.Text) > grdCR.Rows.Count)
                {
                    for (int i = grdCR.Rows.Count + 1; i <= Convert.ToInt32(txtQuantity.Text); i++)
                    {
                        AddRowCR();
                    }
                }
            }
        }
        protected void AddRowCR()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["CRTable"] != null)
            {
                GetCurrentDataCR();
                dt = (DataTable)ViewState["CRTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlCrazing", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtIdMark"] = string.Empty;
            dr["ddlCrazing"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CRTable"] = dt;
            grdCR.DataSource = dt;
            grdCR.DataBind();
            SetPreviousDataCR();
        }
        protected void DeleteRowCR(int rowIndex)
        {
            GetCurrentDataCR();
            DataTable dt = ViewState["CRTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["CRTable"] = dt;
            grdCR.DataSource = dt;
            grdCR.DataBind();
            SetPreviousDataCR();
        }
        protected void SetPreviousDataCR()
        {
            DataTable dt = (DataTable)ViewState["CRTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtIdMark = (TextBox)grdCR.Rows[i].FindControl("txtIdMark");
                DropDownList ddlCrazing = (DropDownList)grdCR.Rows[i].FindControl("ddlCrazing");

                txtIdMark.Text = dt.Rows[i]["txtIdMark"].ToString();
                ddlCrazing.SelectedValue = dt.Rows[i]["ddlCrazing"].ToString();
            }
        }
        protected void GetCurrentDataCR()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtIdMark", typeof(string)));
            dt.Columns.Add(new DataColumn("ddlCrazing", typeof(string)));
            for (int i = 0; i < grdCR.Rows.Count; i++)
            {
                TextBox txtIdMark = (TextBox)grdCR.Rows[i].FindControl("txtIdMark");
                DropDownList ddlCrazing = (DropDownList)grdCR.Rows[i].FindControl("ddlCrazing");

                dr = dt.NewRow();
                dr["txtIdMark"] = txtIdMark.Text;
                dr["ddlCrazing"] = ddlCrazing.SelectedValue;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["CRTable"] = dt;
        }
        #endregion

        #region add/delete rows grdSH grid
        private void SHRowsChanged()
        {
            if (txtSHQuantity.Text != "")
            {
                if (Convert.ToInt32(txtSHQuantity.Text) < grdSH.Rows.Count)
                {
                    for (int i = grdSH.Rows.Count; i > Convert.ToInt32(txtSHQuantity.Text); i--)
                    {
                        if (grdSH.Rows.Count > 1)
                        {
                            DeleteRowSH(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtSHQuantity.Text) > grdSH.Rows.Count)
                {
                    for (int i = grdSH.Rows.Count + 1; i <= Convert.ToInt32(txtSHQuantity.Text); i++)
                    {
                        AddRowSH();
                    }
                }
            }
        }
        protected void AddRowSH()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["SHTable"] != null)
            {
                GetCurrentDataSH();
                dt = (DataTable)ViewState["SHTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtSHIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtHardness", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtSHIdMark"] = string.Empty;
            dr["txtHardness"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["SHTable"] = dt;
            grdSH.DataSource = dt;
            grdSH.DataBind();
            SetPreviousDataSH();
        }
        protected void DeleteRowSH(int rowIndex)
        {
            GetCurrentDataSH();
            DataTable dt = ViewState["SHTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["SHTable"] = dt;
            grdSH.DataSource = dt;
            grdSH.DataBind();
            SetPreviousDataSH();
        }
        protected void SetPreviousDataSH()
        {
            DataTable dt = (DataTable)ViewState["SHTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtSHIdMark = (TextBox)grdSH.Rows[i].FindControl("txtSHIdMark");
                TextBox txtHardness = (TextBox)grdSH.Rows[i].FindControl("txtHardness");

                txtSHIdMark.Text = dt.Rows[i]["txtSHIdMark"].ToString();
                txtHardness.Text = dt.Rows[i]["txtHardness"].ToString();
            }
        }
        protected void GetCurrentDataSH()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtSHIdMark", typeof(string)));
            dt.Columns.Add(new DataColumn("txtHardness", typeof(string)));
            for (int i = 0; i < grdSH.Rows.Count; i++)
            {
                TextBox txtSHIdMark = (TextBox)grdSH.Rows[i].FindControl("txtSHIdMark");
                TextBox txtHardness = (TextBox)grdSH.Rows[i].FindControl("txtHardness");

                dr = dt.NewRow();
                dr["txtSHIdMark"] = txtSHIdMark.Text;
                dr["txtHardness"] = txtHardness.Text;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["SHTable"] = dt;
        }
        #endregion

        #region add/delete rows grdWA grid
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
            dr["txtWAIdMark"] = string.Empty;
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

        #region add/delete rows grdWT grid
        private void WTRowsChanged()
        {
            if (txtWTQuantity.Text != "")
            {
                if (Convert.ToInt32(txtWTQuantity.Text) < grdWT.Rows.Count)
                {
                    for (int i = grdWT.Rows.Count; i > Convert.ToInt32(txtWTQuantity.Text); i--)
                    {
                        if (grdWT.Rows.Count > 1)
                        {
                            DeleteRowWT(i - 1);
                            DeleteRowWTCal(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtWTQuantity.Text) > grdWT.Rows.Count)
                {
                    for (int i = grdWT.Rows.Count + 1; i <= Convert.ToInt32(txtWTQuantity.Text); i++)
                    {
                        AddRowWT();
                        AddRowWTCal();

                    }
                }
            }
        }
        protected void AddRowWT()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["WTTable"] != null)
            {
                GetCurrentDataWT();
                dt = (DataTable)ViewState["WTTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtWTIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT1", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT2", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT3", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT4", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT5", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT6", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWtLeadShots", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtWTIdMark"] = string.Empty;
            dr["txtT1"] = string.Empty;
            dr["txtT2"] = string.Empty;
            dr["txtT3"] = string.Empty;
            dr["txtT4"] = string.Empty;
            dr["txtT5"] = string.Empty;
            dr["txtT6"] = string.Empty;
            dr["txtWtLeadShots"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["WTTable"] = dt;
            grdWT.DataSource = dt;
            grdWT.DataBind();
            SetPreviousDataWT();
        }
        protected void DeleteRowWT(int rowIndex)
        {
            GetCurrentDataWT();
            DataTable dt = ViewState["WTTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["WTTable"] = dt;
            grdWT.DataSource = dt;
            grdWT.DataBind();
            SetPreviousDataWT();
        }
        protected void SetPreviousDataWT()
        {
            DataTable dt = (DataTable)ViewState["WTTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtWTIdMark = (TextBox)grdWT.Rows[i].FindControl("txtWTIdMark");
                TextBox txtT1 = (TextBox)grdWT.Rows[i].FindControl("txtT1");
                TextBox txtT2 = (TextBox)grdWT.Rows[i].FindControl("txtT2");
                TextBox txtT3 = (TextBox)grdWT.Rows[i].FindControl("txtT3");
                TextBox txtT4 = (TextBox)grdWT.Rows[i].FindControl("txtT4");
                TextBox txtT5 = (TextBox)grdWT.Rows[i].FindControl("txtT5");
                TextBox txtT6 = (TextBox)grdWT.Rows[i].FindControl("txtT6");
                TextBox txtWtLeadShots = (TextBox)grdWT.Rows[i].FindControl("txtWtLeadShots");


                txtWTIdMark.Text = dt.Rows[i]["txtWTIdMark"].ToString();
                txtT1.Text = dt.Rows[i]["txtT1"].ToString();
                txtT2.Text = dt.Rows[i]["txtT2"].ToString();
                txtT3.Text = dt.Rows[i]["txtT3"].ToString();
                txtT4.Text = dt.Rows[i]["txtT4"].ToString();
                txtT5.Text = dt.Rows[i]["txtT5"].ToString();
                txtT6.Text = dt.Rows[i]["txtT6"].ToString();
                txtWtLeadShots.Text = dt.Rows[i]["txtWtLeadShots"].ToString();

            }
        }
        protected void GetCurrentDataWT()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtWTIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT5", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT6", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWtLeadShots", typeof(string)));

            for (int i = 0; i < grdWT.Rows.Count; i++)
            {
                TextBox txtWTIdMark = (TextBox)grdWT.Rows[i].FindControl("txtWTIdMark");
                TextBox txtT1 = (TextBox)grdWT.Rows[i].FindControl("txtT1");
                TextBox txtT2 = (TextBox)grdWT.Rows[i].FindControl("txtT2");
                TextBox txtT3 = (TextBox)grdWT.Rows[i].FindControl("txtT3");
                TextBox txtT4 = (TextBox)grdWT.Rows[i].FindControl("txtT4");
                TextBox txtT5 = (TextBox)grdWT.Rows[i].FindControl("txtT5");
                TextBox txtT6 = (TextBox)grdWT.Rows[i].FindControl("txtT6");
                TextBox txtWtLeadShots = (TextBox)grdWT.Rows[i].FindControl("txtWtLeadShots");

                drRow = dtTable.NewRow();
                drRow["txtWTIdMark"] = txtWTIdMark.Text;
                drRow["txtT1"] = txtT1.Text;
                drRow["txtT2"] = txtT2.Text;
                drRow["txtT3"] = txtT3.Text;
                drRow["txtT4"] = txtT4.Text;
                drRow["txtT5"] = txtT5.Text;
                drRow["txtT6"] = txtT6.Text;
                drRow["txtWtLeadShots"] = txtWtLeadShots.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["WTTable"] = dtTable;

        }
        #endregion

        #region add/delete rows grdWTCal grid

        protected void AddRowWTCal()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["WTCalTable"] != null)
            {
                GetCurrentDataWTCal();
                dt = (DataTable)ViewState["WTCalTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtWTCalIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWTThickness", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWtCalLeadShots", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWtB", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWtN", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWtTransverse", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWetTr", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAvgWetTr", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtWTCalIdMark"] = string.Empty;
            dr["txtWTThickness"] = string.Empty;
            dr["txtWtCalLeadShots"] = string.Empty;
            dr["txtWtB"] = string.Empty;
            dr["txtWtN"] = string.Empty;
            dr["txtWtTransverse"] = string.Empty;
            dr["txtWetTr"] = string.Empty;
            dr["txtAvgWetTr"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["WTCalTable"] = dt;
            grdWTCal.DataSource = dt;
            grdWTCal.DataBind();
            SetPreviousDataWTCal();
        }
        protected void DeleteRowWTCal(int rowIndex)
        {
            GetCurrentDataWTCal();
            DataTable dt = ViewState["WTCalTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["WTCalTable"] = dt;
            grdWTCal.DataSource = dt;
            grdWTCal.DataBind();
            SetPreviousDataWTCal();
        }
        protected void SetPreviousDataWTCal()
        {
            DataTable dt = (DataTable)ViewState["WTCalTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtWTCalIdMark = (TextBox)grdWTCal.Rows[i].FindControl("txtWTCalIdMark");
                TextBox txtWTThickness = (TextBox)grdWTCal.Rows[i].FindControl("txtWTThickness");
                TextBox txtWtCalLeadShots = (TextBox)grdWTCal.Rows[i].FindControl("txtWtCalLeadShots");
                TextBox txtWtB = (TextBox)grdWTCal.Rows[i].FindControl("txtWtB");
                TextBox txtWtN = (TextBox)grdWTCal.Rows[i].FindControl("txtWtN");
                TextBox txtWtTransverse = (TextBox)grdWTCal.Rows[i].FindControl("txtWtTransverse");
                TextBox txtWetTr = (TextBox)grdWTCal.Rows[i].FindControl("txtWetTr");
                TextBox txtAvgWetTr = (TextBox)grdWTCal.Rows[i].FindControl("txtAvgWetTr");

                txtWTCalIdMark.Text = dt.Rows[i]["txtWTCalIdMark"].ToString();
                txtWTThickness.Text = dt.Rows[i]["txtWTThickness"].ToString();
                txtWtCalLeadShots.Text = dt.Rows[i]["txtWtCalLeadShots"].ToString();
                txtWtB.Text = dt.Rows[i]["txtWtB"].ToString();
                txtWtN.Text = dt.Rows[i]["txtWtN"].ToString();
                txtWtTransverse.Text = dt.Rows[i]["txtWtTransverse"].ToString();
                txtWetTr.Text = dt.Rows[i]["txtWetTr"].ToString();
                txtAvgWetTr.Text = dt.Rows[i]["txtAvgWetTr"].ToString();
            }
        }
        protected void GetCurrentDataWTCal()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtWTCalIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWTThickness", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWtCalLeadShots", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWtB", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWtN", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWtTransverse", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWetTr", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAvgWetTr", typeof(string)));

            for (int i = 0; i < grdWTCal.Rows.Count; i++)
            {

                TextBox txtWTCalIdMark = (TextBox)grdWTCal.Rows[i].FindControl("txtWTCalIdMark");
                TextBox txtWTThickness = (TextBox)grdWTCal.Rows[i].FindControl("txtWTThickness");
                TextBox txtWtCalLeadShots = (TextBox)grdWTCal.Rows[i].FindControl("txtWtCalLeadShots");
                TextBox txtWtB = (TextBox)grdWTCal.Rows[i].FindControl("txtWtB");
                TextBox txtWtN = (TextBox)grdWTCal.Rows[i].FindControl("txtWtN");
                TextBox txtWtTransverse = (TextBox)grdWTCal.Rows[i].FindControl("txtWtTransverse");
                TextBox txtWetTr = (TextBox)grdWTCal.Rows[i].FindControl("txtWetTr");
                TextBox txtAvgWetTr = (TextBox)grdWTCal.Rows[i].FindControl("txtAvgWetTr");

                drRow = dtTable.NewRow();
                drRow["txtWTCalIdMark"] = txtWTCalIdMark.Text;
                drRow["txtWTThickness"] = txtWTThickness.Text;
                drRow["txtWtCalLeadShots"] = txtWtCalLeadShots.Text;
                drRow["txtWtB"] = txtWtB.Text;
                drRow["txtWtN"] = txtWtN.Text;
                drRow["txtWtTransverse"] = txtWtTransverse.Text;
                drRow["txtWetTr"] = txtWetTr.Text;
                drRow["txtAvgWetTr"] = txtAvgWetTr.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["WTCalTable"] = dtTable;

        }
        #endregion

        #region add/delete rows grdMR grid
        private void MRRowsChanged()
        {
            if (txtMRQuantity.Text != "")
            {
                if (Convert.ToInt32(txtMRQuantity.Text) < grdMR.Rows.Count)
                {
                    for (int i = grdMR.Rows.Count; i > Convert.ToInt32(txtMRQuantity.Text); i--)
                    {
                        if (grdMR.Rows.Count > 1)
                        {
                            DeleteRowMR(i - 1);
                            DeleteRowMRCal(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtMRQuantity.Text) > grdMR.Rows.Count)
                {
                    for (int i = grdMR.Rows.Count + 1; i <= Convert.ToInt32(txtMRQuantity.Text); i++)
                    {
                        AddRowMR();
                        AddRowMRCal();

                    }
                }
            }
        }
        protected void AddRowMR()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MRTable"] != null)
            {
                GetCurrentDataMR();
                dt = (DataTable)ViewState["MRTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtMRIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT1", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT2", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT3", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT4", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT5", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT6", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMRLeadShots", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtMRIdMark"] = string.Empty;
            dr["txtT1"] = string.Empty;
            dr["txtT2"] = string.Empty;
            dr["txtT3"] = string.Empty;
            dr["txtT4"] = string.Empty;
            dr["txtT5"] = string.Empty;
            dr["txtT6"] = string.Empty;
            dr["txtMRLeadShots"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["MRTable"] = dt;
            grdMR.DataSource = dt;
            grdMR.DataBind();
            SetPreviousDataMR();
        }
        protected void DeleteRowMR(int rowIndex)
        {
            GetCurrentDataMR();
            DataTable dt = ViewState["MRTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["MRTable"] = dt;
            grdMR.DataSource = dt;
            grdMR.DataBind();
            SetPreviousDataMR();
        }
        protected void SetPreviousDataMR()
        {
            DataTable dt = (DataTable)ViewState["MRTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtMRIdMark = (TextBox)grdMR.Rows[i].FindControl("txtMRIdMark");
                TextBox txtT1 = (TextBox)grdMR.Rows[i].FindControl("txtT1");
                TextBox txtT2 = (TextBox)grdMR.Rows[i].FindControl("txtT2");
                TextBox txtT3 = (TextBox)grdMR.Rows[i].FindControl("txtT3");
                TextBox txtT4 = (TextBox)grdMR.Rows[i].FindControl("txtT4");
                TextBox txtT5 = (TextBox)grdMR.Rows[i].FindControl("txtT5");
                TextBox txtT6 = (TextBox)grdMR.Rows[i].FindControl("txtT6");
                TextBox txtMRLeadShots = (TextBox)grdMR.Rows[i].FindControl("txtMRLeadShots");


                txtMRIdMark.Text = dt.Rows[i]["txtMRIdMark"].ToString();
                txtT1.Text = dt.Rows[i]["txtT1"].ToString();
                txtT2.Text = dt.Rows[i]["txtT2"].ToString();
                txtT3.Text = dt.Rows[i]["txtT3"].ToString();
                txtT4.Text = dt.Rows[i]["txtT4"].ToString();
                txtT5.Text = dt.Rows[i]["txtT5"].ToString();
                txtT6.Text = dt.Rows[i]["txtT6"].ToString();
                txtMRLeadShots.Text = dt.Rows[i]["txtMRLeadShots"].ToString();

            }
        }
        protected void GetCurrentDataMR()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtMRIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT4", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT5", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT6", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtMRLeadShots", typeof(string)));

            for (int i = 0; i < grdMR.Rows.Count; i++)
            {
                TextBox txtMRIdMark = (TextBox)grdMR.Rows[i].FindControl("txtMRIdMark");
                TextBox txtT1 = (TextBox)grdMR.Rows[i].FindControl("txtT1");
                TextBox txtT2 = (TextBox)grdMR.Rows[i].FindControl("txtT2");
                TextBox txtT3 = (TextBox)grdMR.Rows[i].FindControl("txtT3");
                TextBox txtT4 = (TextBox)grdMR.Rows[i].FindControl("txtT4");
                TextBox txtT5 = (TextBox)grdMR.Rows[i].FindControl("txtT5");
                TextBox txtT6 = (TextBox)grdMR.Rows[i].FindControl("txtT6");
                TextBox txtMRLeadShots = (TextBox)grdMR.Rows[i].FindControl("txtMRLeadShots");

                drRow = dtTable.NewRow();
                drRow["txtMRIdMark"] = txtMRIdMark.Text;
                drRow["txtT1"] = txtT1.Text;
                drRow["txtT2"] = txtT2.Text;
                drRow["txtT3"] = txtT3.Text;
                drRow["txtT4"] = txtT4.Text;
                drRow["txtT5"] = txtT5.Text;
                drRow["txtT6"] = txtT6.Text;
                drRow["txtMRLeadShots"] = txtMRLeadShots.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["MRTable"] = dtTable;

        }
        #endregion

        #region add/delete rows grdMRCal grid
        protected void AddRowMRCal()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["MRCalTable"] != null)
            {
                GetCurrentDataMRCal();
                dt = (DataTable)ViewState["MRCalTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtMRCalIdMark", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMRThickness", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMRCalLeadShots", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMRB", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMRN", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAvgBreakLoad", typeof(string)));
                dt.Columns.Add(new DataColumn("txtBreakStrength", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAvgBreakStrength", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMRTransverse", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMOR", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAvgMOR", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtMRCalIdMark"] = string.Empty;
            dr["txtMRThickness"] = string.Empty;
            dr["txtMRCalLeadShots"] = string.Empty;
            dr["txtMRB"] = string.Empty;
            dr["txtMRN"] = string.Empty;
            dr["txtAvgBreakLoad"] = string.Empty;
            dr["txtBreakStrength"] = string.Empty;
            dr["txtAvgBreakStrength"] = string.Empty;
            dr["txtMRTransverse"] = string.Empty;
            dr["txtMOR"] = string.Empty;
            dr["txtAvgMOR"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["MRCalTable"] = dt;
            grdMRCal.DataSource = dt;
            grdMRCal.DataBind();
            SetPreviousDataMRCal();
        }
        protected void DeleteRowMRCal(int rowIndex)
        {
            GetCurrentDataMRCal();
            DataTable dt = ViewState["MRCalTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["MRCalTable"] = dt;
            grdMRCal.DataSource = dt;
            grdMRCal.DataBind();
            SetPreviousDataMRCal();
        }
        protected void SetPreviousDataMRCal()
        {
            DataTable dt = (DataTable)ViewState["MRCalTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtMRCalIdMark = (TextBox)grdMRCal.Rows[i].FindControl("txtMRCalIdMark");
                TextBox txtMRThickness = (TextBox)grdMRCal.Rows[i].FindControl("txtMRThickness");
                TextBox txtMRCalLeadShots = (TextBox)grdMRCal.Rows[i].FindControl("txtMRCalLeadShots");
                TextBox txtMRB = (TextBox)grdMRCal.Rows[i].FindControl("txtMRB");
                TextBox txtMRN = (TextBox)grdMRCal.Rows[i].FindControl("txtMRN");
                TextBox txtAvgBreakLoad = (TextBox)grdMRCal.Rows[i].FindControl("txtAvgBreakLoad");
                TextBox txtBreakStrength = (TextBox)grdMRCal.Rows[i].FindControl("txtBreakStrength");
                TextBox txtAvgBreakStrength = (TextBox)grdMRCal.Rows[i].FindControl("txtAvgBreakStrength");
                TextBox txtMRTransverse = (TextBox)grdMRCal.Rows[i].FindControl("txtMRTransverse");
                TextBox txtMOR = (TextBox)grdMRCal.Rows[i].FindControl("txtMOR");
                TextBox txtAvgMOR = (TextBox)grdMRCal.Rows[i].FindControl("txtAvgMOR");

                txtMRCalIdMark.Text = dt.Rows[i]["txtMRCalIdMark"].ToString();
                txtMRThickness.Text = dt.Rows[i]["txtMRThickness"].ToString();
                txtMRCalLeadShots.Text = dt.Rows[i]["txtMRCalLeadShots"].ToString();
                txtMRB.Text = dt.Rows[i]["txtMRB"].ToString();
                txtMRN.Text = dt.Rows[i]["txtMRN"].ToString();
                txtAvgBreakLoad.Text = dt.Rows[i]["txtAvgBreakLoad"].ToString();
                txtBreakStrength.Text = dt.Rows[i]["txtBreakStrength"].ToString();
                txtAvgBreakStrength.Text = dt.Rows[i]["txtAvgBreakStrength"].ToString();
                txtMRTransverse.Text = dt.Rows[i]["txtMRTransverse"].ToString();
                txtMOR.Text = dt.Rows[i]["txtMOR"].ToString();
                txtAvgMOR.Text = dt.Rows[i]["txtAvgMOR"].ToString();
            }
        }
        protected void GetCurrentDataMRCal()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtMRCalIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtMRThickness", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtMRCalLeadShots", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtMRB", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtMRN", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAvgBreakLoad", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtBreakStrength", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAvgBreakStrength", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtMRTransverse", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtMOR", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAvgMOR", typeof(string)));

            for (int i = 0; i < grdMRCal.Rows.Count; i++)
            {

                TextBox txtMRCalIdMark = (TextBox)grdMRCal.Rows[i].FindControl("txtMRCalIdMark");
                TextBox txtMRThickness = (TextBox)grdMRCal.Rows[i].FindControl("txtMRThickness");
                TextBox txtMRCalLeadShots = (TextBox)grdMRCal.Rows[i].FindControl("txtMRCalLeadShots");
                TextBox txtMRB = (TextBox)grdMRCal.Rows[i].FindControl("txtMRB");
                TextBox txtMRN = (TextBox)grdMRCal.Rows[i].FindControl("txtMRN");
                TextBox txtAvgBreakLoad = (TextBox)grdMRCal.Rows[i].FindControl("txtAvgBreakLoad");
                TextBox txtBreakStrength = (TextBox)grdMRCal.Rows[i].FindControl("txtBreakStrength");
                TextBox txtAvgBreakStrength = (TextBox)grdMRCal.Rows[i].FindControl("txtAvgBreakStrength");
                TextBox txtMRTransverse = (TextBox)grdMRCal.Rows[i].FindControl("txtMRTransverse");
                TextBox txtMOR = (TextBox)grdMRCal.Rows[i].FindControl("txtMOR");
                TextBox txtAvgMOR = (TextBox)grdMRCal.Rows[i].FindControl("txtAvgMOR");


                drRow = dtTable.NewRow();
                drRow["txtMRCalIdMark"] = txtMRCalIdMark.Text;
                drRow["txtMRThickness"] = txtMRThickness.Text;
                drRow["txtMRCalLeadShots"] = txtMRCalLeadShots.Text;
                drRow["txtMRB"] = txtMRB.Text;
                drRow["txtMRN"] = txtMRN.Text;
                drRow["txtAvgBreakLoad"] = txtAvgBreakLoad.Text;
                drRow["txtBreakStrength"] = txtBreakStrength.Text;
                drRow["txtAvgBreakStrength"] = txtAvgBreakStrength.Text;
                drRow["txtMRTransverse"] = txtMRTransverse.Text;
                drRow["txtMOR"] = txtMOR.Text;
                drRow["txtAvgMOR"] = txtAvgMOR.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["MRCalTable"] = dtTable;

        }
        #endregion

        #region add/delete rows grdDA grid

        private void DARowsChanged()
        {
            if (txtDAQuantity.Text != "")
            {
                if (Convert.ToInt32(txtDAQuantity.Text) < grdDA.Rows.Count)
                {
                    for (int i = grdDA.Rows.Count; i > Convert.ToInt32(txtDAQuantity.Text); i--)
                    {
                        if (grdDA.Rows.Count > 1)
                        {
                            DeleteRowDA(i - 1);
                            DeleteRowDACal(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtDAQuantity.Text) > grdDA.Rows.Count)
                {
                    for (int i = grdDA.Rows.Count + 1; i <= Convert.ToInt32(txtDAQuantity.Text); i++)
                    {

                        AddRowDA();
                        AddRowDACal();
                    }
                }
            }
        }
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
                dt.Columns.Add(new DataColumn("txtT1", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT2", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT3", typeof(string)));
                dt.Columns.Add(new DataColumn("txtT4", typeof(string)));
                //dt.Columns.Add(new DataColumn("txtT5", typeof(string)));
                //dt.Columns.Add(new DataColumn("txtT6", typeof(string)));
                //dt.Columns.Add(new DataColumn("txtT7", typeof(string)));
                //dt.Columns.Add(new DataColumn("txtT8", typeof(string)));
                //dt.Columns.Add(new DataColumn("txtT9", typeof(string)));
                //dt.Columns.Add(new DataColumn("txtT10", typeof(string)));
                //dt.Columns.Add(new DataColumn("txtT11", typeof(string)));
                //dt.Columns.Add(new DataColumn("txtT12", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtDAIdMark"] = string.Empty;
            dr["txtL1"] = string.Empty;
            dr["txtL2"] = string.Empty;
            dr["txtL3"] = string.Empty;
            dr["txtW1"] = string.Empty;
            dr["txtW2"] = string.Empty;
            dr["txtW3"] = string.Empty;
            dr["txtT1"] = string.Empty;
            dr["txtT2"] = string.Empty;
            dr["txtT3"] = string.Empty;
            dr["txtT4"] = string.Empty;
            //dr["txtT5"] = string.Empty;
            //dr["txtT6"] = string.Empty;
            //dr["txtT7"] = string.Empty;
            //dr["txtT8"] = string.Empty;
            //dr["txtT9"] = string.Empty;
            //dr["txtT10"] = string.Empty;
            //dr["txtT11"] = string.Empty;
            //dr["txtT12"] = string.Empty;
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
                TextBox txtT1 = (TextBox)grdDA.Rows[i].FindControl("txtT1");
                TextBox txtT2 = (TextBox)grdDA.Rows[i].FindControl("txtT2");
                TextBox txtT3 = (TextBox)grdDA.Rows[i].FindControl("txtT3");
                TextBox txtT4 = (TextBox)grdDA.Rows[i].FindControl("txtT4");
                //TextBox txtT5 = (TextBox)grdDA.Rows[i].FindControl("txtT5");
                //TextBox txtT6 = (TextBox)grdDA.Rows[i].FindControl("txtT6");
                //TextBox txtT7 = (TextBox)grdDA.Rows[i].FindControl("txtT7");
                //TextBox txtT8 = (TextBox)grdDA.Rows[i].FindControl("txtT8");
                //TextBox txtT9 = (TextBox)grdDA.Rows[i].FindControl("txtT9");
                //TextBox txtT10 = (TextBox)grdDA.Rows[i].FindControl("txtT10");
                //TextBox txtT11 = (TextBox)grdDA.Rows[i].FindControl("txtT11");
                //TextBox txtT12 = (TextBox)grdDA.Rows[i].FindControl("txtT12");
                
                txtDAIdMark.Text = dt.Rows[i]["txtDAIdMark"].ToString();
                txtL1.Text = dt.Rows[i]["txtL1"].ToString();
                txtL2.Text = dt.Rows[i]["txtL2"].ToString();
                txtL3.Text = dt.Rows[i]["txtL3"].ToString();
                txtW1.Text = dt.Rows[i]["txtW1"].ToString();
                txtW2.Text = dt.Rows[i]["txtW2"].ToString();
                txtW3.Text = dt.Rows[i]["txtW3"].ToString();
                txtT1.Text = dt.Rows[i]["txtT1"].ToString();
                txtT2.Text = dt.Rows[i]["txtT2"].ToString();
                txtT3.Text = dt.Rows[i]["txtT3"].ToString();
                txtT4.Text = dt.Rows[i]["txtT4"].ToString();
                //txtT5.Text = dt.Rows[i]["txtT5"].ToString();
                //txtT6.Text = dt.Rows[i]["txtT6"].ToString();
                //txtT7.Text = dt.Rows[i]["txtT7"].ToString();
                //txtT8.Text = dt.Rows[i]["txtT8"].ToString();
                //txtT9.Text = dt.Rows[i]["txtT9"].ToString();
                //txtT10.Text = dt.Rows[i]["txtT10"].ToString();
                //txtT11.Text = dt.Rows[i]["txtT11"].ToString();
                //txtT12.Text = dt.Rows[i]["txtT12"].ToString();

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
            dtTable.Columns.Add(new DataColumn("txtT1", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT2", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT3", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtT4", typeof(string)));
            //dtTable.Columns.Add(new DataColumn("txtT5", typeof(string)));
            //dtTable.Columns.Add(new DataColumn("txtT6", typeof(string)));
            //dtTable.Columns.Add(new DataColumn("txtT7", typeof(string)));
            //dtTable.Columns.Add(new DataColumn("txtT8", typeof(string)));
            //dtTable.Columns.Add(new DataColumn("txtT9", typeof(string)));
            //dtTable.Columns.Add(new DataColumn("txtT10", typeof(string)));
            //dtTable.Columns.Add(new DataColumn("txtT11", typeof(string)));
            //dtTable.Columns.Add(new DataColumn("txtT12", typeof(string)));
            for (int i = 0; i < grdDA.Rows.Count; i++)
            {
                TextBox txtDAIdMark = (TextBox)grdDA.Rows[i].Cells[1].FindControl("txtDAIdMark");
                TextBox txtL1 = (TextBox)grdDA.Rows[i].Cells[2].FindControl("txtL1");
                TextBox txtL2 = (TextBox)grdDA.Rows[i].Cells[2].FindControl("txtL2");
                TextBox txtL3 = (TextBox)grdDA.Rows[i].Cells[2].FindControl("txtL3");
                TextBox txtW1 = (TextBox)grdDA.Rows[i].Cells[3].FindControl("txtW1");
                TextBox txtW2 = (TextBox)grdDA.Rows[i].Cells[3].FindControl("txtW2");
                TextBox txtW3 = (TextBox)grdDA.Rows[i].Cells[3].FindControl("txtW3");
                TextBox txtT1 = (TextBox)grdDA.Rows[i].FindControl("txtT1");
                TextBox txtT2 = (TextBox)grdDA.Rows[i].FindControl("txtT2");
                TextBox txtT3 = (TextBox)grdDA.Rows[i].FindControl("txtT3");
                TextBox txtT4 = (TextBox)grdDA.Rows[i].FindControl("txtT4");
                //TextBox txtT5 = (TextBox)grdDA.Rows[i].FindControl("txtT5");
                //TextBox txtT6 = (TextBox)grdDA.Rows[i].FindControl("txtT6");
                //TextBox txtT7 = (TextBox)grdDA.Rows[i].FindControl("txtT7");
                //TextBox txtT8 = (TextBox)grdDA.Rows[i].FindControl("txtT8");
                //TextBox txtT9 = (TextBox)grdDA.Rows[i].FindControl("txtT9");
                //TextBox txtT10 = (TextBox)grdDA.Rows[i].FindControl("txtT10");
                //TextBox txtT11 = (TextBox)grdDA.Rows[i].FindControl("txtT11");
                //TextBox txtT12 = (TextBox)grdDA.Rows[i].FindControl("txtT12");

                drRow = dtTable.NewRow();
                drRow["txtDAIdMark"] = txtDAIdMark.Text;
                drRow["txtL1"] = txtL1.Text;
                drRow["txtL2"] = txtL2.Text;
                drRow["txtL3"] = txtL3.Text;
                drRow["txtW1"] = txtW1.Text;
                drRow["txtW2"] = txtW2.Text;
                drRow["txtW3"] = txtW3.Text;
                drRow["txtT1"] = txtT1.Text;
                drRow["txtT2"] = txtT2.Text;
                drRow["txtT3"] = txtT3.Text;
                drRow["txtT4"] = txtT4.Text;
                //drRow["txtT5"] = txtT5.Text;
                //drRow["txtT6"] = txtT6.Text;
                //drRow["txtT7"] = txtT7.Text;
                //drRow["txtT8"] = txtT8.Text;
                //drRow["txtT9"] = txtT9.Text;
                //drRow["txtT10"] = txtT10.Text;
                //drRow["txtT11"] = txtT11.Text;
                //drRow["txtT12"] = txtT12.Text;

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
                dt.Columns.Add(new DataColumn("txtLength", typeof(string)));
                dt.Columns.Add(new DataColumn("txtWidth", typeof(string)));
                dt.Columns.Add(new DataColumn("txtThickness", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAvgLen", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAvgWidth", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAvgThickness", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtDACalIdMark"] = string.Empty;
            dr["txtLength"] = string.Empty;
            dr["txtWidth"] = string.Empty;
            dr["txtThickness"] = string.Empty;
            dr["txtAvgLen"] = string.Empty;
            dr["txtAvgWidth"] = string.Empty;
            dr["txtAvgThickness"] = string.Empty;
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
                TextBox txtLength = (TextBox)grdDACal.Rows[i].FindControl("txtLength");
                TextBox txtWidth = (TextBox)grdDACal.Rows[i].FindControl("txtWidth");
                TextBox txtThickness = (TextBox)grdDACal.Rows[i].FindControl("txtThickness");
                TextBox txtAvgLen = (TextBox)grdDACal.Rows[i].FindControl("txtAvgLen");
                TextBox txtAvgWidth = (TextBox)grdDACal.Rows[i].FindControl("txtAvgWidth");
                TextBox txtAvgThickness = (TextBox)grdDACal.Rows[i].FindControl("txtAvgThickness");

                txtDACalIdMark.Text = dt.Rows[i]["txtDACalIdMark"].ToString();
                txtLength.Text = dt.Rows[i]["txtLength"].ToString();
                txtWidth.Text = dt.Rows[i]["txtWidth"].ToString();
                txtThickness.Text = dt.Rows[i]["txtThickness"].ToString();
                txtAvgLen.Text = dt.Rows[i]["txtAvgLen"].ToString();
                txtAvgWidth.Text = dt.Rows[i]["txtAvgWidth"].ToString();
                txtAvgThickness.Text = dt.Rows[i]["txtAvgThickness"].ToString();
            }
        }
        protected void GetCurrentDataDACal()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txtDACalIdMark", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtLength", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtWidth", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtThickness", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAvgLen", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAvgWidth", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAvgThickness", typeof(string)));

            for (int i = 0; i < grdDACal.Rows.Count; i++)
            {
                TextBox txtDACalIdMark = (TextBox)grdDACal.Rows[i].FindControl("txtDACalIdMark");
                TextBox txtLength = (TextBox)grdDACal.Rows[i].FindControl("txtLength");
                TextBox txtWidth = (TextBox)grdDACal.Rows[i].FindControl("txtWidth");
                TextBox txtThickness = (TextBox)grdDACal.Rows[i].FindControl("txtThickness");
                TextBox txtAvgLen = (TextBox)grdDACal.Rows[i].FindControl("txtAvgLen");
                TextBox txtAvgWidth = (TextBox)grdDACal.Rows[i].FindControl("txtAvgWidth");
                TextBox txtAvgThickness = (TextBox)grdDACal.Rows[i].FindControl("txtAvgThickness");

                drRow = dtTable.NewRow();
                drRow["txtDACalIdMark"] = txtDACalIdMark.Text;
                drRow["txtLength"] = txtLength.Text;
                drRow["txtWidth"] = txtWidth.Text;
                drRow["txtThickness"] = txtThickness.Text;
                drRow["txtAvgLen"] = txtAvgLen.Text;
                drRow["txtAvgWidth"] = txtAvgWidth.Text;
                drRow["txtAvgThickness"] = txtAvgThickness.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["DACalTable"] = dtTable;
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

        #region add/delete rows grdSpecifiedLimit grid
        protected void AddRowSpecifiedLimit()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["SpecifiedLimit"] != null)
            {
                GetCurrentDataSpecifiedLimit();
                dt = (DataTable)ViewState["SpecifiedLimit"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtSpeciDescription", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpeciLimits", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpeciLength", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpeciWidth", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSpeciThickness", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtSpeciDescription"] = string.Empty;
            dr["txtSpeciLimits"] = string.Empty;
            dr["txtSpeciLength"] = string.Empty;
            dr["txtSpeciWidth"] = string.Empty;
            dr["txtSpeciThickness"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["SpecifiedLimit"] = dt;
            grdSpecifiedLimit.DataSource = dt;
            grdSpecifiedLimit.DataBind();
            SetPreviousDataSpecifiedLimit();
        }
        protected void DeleteRowSpecifiedLimit(int rowIndex)
        {
            GetCurrentDataSpecifiedLimit();
            DataTable dt = ViewState["SpecifiedLimit"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["SpecifiedLimit"] = dt;
            grdSpecifiedLimit.DataSource = dt;
            grdSpecifiedLimit.DataBind();
            SetPreviousDataSpecifiedLimit();
        }
        protected void SetPreviousDataSpecifiedLimit()
        {
            DataTable dt = (DataTable)ViewState["SpecifiedLimit"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtSpeciDescription = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciDescription");
                TextBox txtSpeciLimits = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciLimits");
                TextBox txtSpeciLength = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciLength");
                TextBox txtSpeciWidth = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciWidth");
                TextBox txtSpeciThickness = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciThickness");
                txtSpeciDescription.Text = dt.Rows[i]["txtSpeciDescription"].ToString();
                txtSpeciLimits.Text = dt.Rows[i]["txtSpeciLimits"].ToString();
                txtSpeciLength.Text = dt.Rows[i]["txtSpeciLength"].ToString();
                txtSpeciWidth.Text = dt.Rows[i]["txtSpeciWidth"].ToString();
                txtSpeciThickness.Text = dt.Rows[i]["txtSpeciThickness"].ToString();
            }
        }
        protected void GetCurrentDataSpecifiedLimit()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtSpeciDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpeciLimits", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpeciLength", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpeciWidth", typeof(string)));
            dt.Columns.Add(new DataColumn("txtSpeciThickness", typeof(string)));

            for (int i = 0; i < grdSpecifiedLimit.Rows.Count; i++)
            {
                TextBox txtSpeciDescription = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciDescription");
                TextBox txtSpeciLimits = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciLimits");
                TextBox txtSpeciLength = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciLength");
                TextBox txtSpeciWidth = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciWidth");
                TextBox txtSpeciThickness = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciThickness");

                dr = dt.NewRow();

                dr["txtSpeciDescription"] = txtSpeciDescription.Text;
                dr["txtSpeciLimits"] = txtSpeciLimits.Text;
                dr["txtSpeciLength"] = txtSpeciLength.Text;
                dr["txtSpeciWidth"] = txtSpeciWidth.Text;
                dr["txtSpeciThickness"] = txtSpeciThickness.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["SpecifiedLimit"] = dt;

        }
        protected void imgBtnAddSpecifiedRow_Click(object sender, EventArgs e)
        {
            AddRowSpecifiedLimit();
        }
        protected void imgBtnDeleteSpecifiedRow_Click(object sender, EventArgs e)
        {
            if (grdSpecifiedLimit.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowSpecifiedLimit(gvr.RowIndex);
            }
        }

        #endregion

        #region events
        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            CRRowsChanged();
        }
        protected void txtSHQuantity_TextChanged(object sender, EventArgs e)
        {
            SHRowsChanged();
        }
        protected void txtWAQuantity_TextChanged(object sender, EventArgs e)
        {
            WARowsChanged();
        }
        protected void txtWTQuantity_TextChanged(object sender, EventArgs e)
        {
            WTRowsChanged();
        }
        protected void txtMRQuantity_TextChanged(object sender, EventArgs e)
        {
            MRRowsChanged();
        }
        protected void txtDAQuantity_TextChanged(object sender, EventArgs e)
        {
            DARowsChanged();
        }
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }
        protected void lnkFetch_Click(object sender, EventArgs e)
        {
            if (ddlRefNo.SelectedIndex > 0)
            {
                ClearData();
                txtRefNo.Text = ddlRefNo.SelectedValue;
              //  Session["ReferenceNo"] = txtRefNo.Text;
                DisplayTileTestDetails();
                LoadReferenceNoList();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning", "alert('Please Select Reference No.');", true);
            }
        }
        protected void chkWitnessBy_CheckedChanged(object sender, EventArgs e)
        {
            txtWitnesBy.Text = "";
            if (chkWitnessBy.Checked == true)
                txtWitnesBy.Visible = true;
            else
                txtWitnesBy.Visible = false;
        }
        protected void RdbMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RdbMachine.SelectedValue == "Mechanical Flexture Machine"  )
            {
                grdWT.HeaderRow.Cells[8].Text = "Wt. of Lead Shots (N)";
                grdWTCal.HeaderRow.Cells[3].Text = "Wt. of Lead Shots (N)";
                grdWTCal.HeaderRow.Cells[4].Text = "B (k.g)";
            }
            else if (RdbMachine.SelectedValue == "Digital Flexture Machine")
            {
                grdWT.HeaderRow.Cells[8].Text = "Wt. of Load (k.g)";
                grdWTCal.HeaderRow.Cells[3].Text = "Wt. of Load (k.g)";
                grdWTCal.HeaderRow.Cells[4].Text = "Wt. of Load (k.g) * 12";
            }
        }
        protected void RdbMRMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (RdbMRMachine.SelectedValue == "Mechanical Flexture Machine")
                {
                    grdMR.HeaderRow.Cells[8].Text = "Wt. of Lead Shots (N)";
                    grdMRCal.HeaderRow.Cells[3].Text = "Wt. of Lead Shots (N)";
                    grdMRCal.Columns[4].Visible = true;
                   
                }
                else if (RdbMRMachine.SelectedValue == "Digital Flexture Machine")
                {
                    grdMR.HeaderRow.Cells[8].Text = "Wt. of Load (KN)";
                    grdMRCal.HeaderRow.Cells[3].Text = "Wt. of Load (KN)";
                    grdMRCal.Columns[4].Visible = false;
                    
                }
            }
            catch { }
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
            else if (ddlTileType.SelectedIndex <= 0)
            {
                dispalyMsg = "Please Select Tile Type.";
                txtDateOfTest.Focus();
                valid = false;
            }
            else if (txtDateOfTest.Text == "")
            {
                dispalyMsg = "Date Of Testing can not be blank.";
                txtDateOfTest.Focus();
                valid = false;
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

            if (TabWT.Visible == true)
            {
                #region validate WT data
                if (txtL.Text == "")
                {
                    dispalyMsg = "Please Enter value for L";
                    txtL.Focus();
                    valid = false;
                }
                else if (txtB.Text == "")
                {
                    dispalyMsg = "Please Enter value for B";
                    txtB.Focus();
                    valid = false;
                }

                if (valid == true)
                {
                    for (int i = 0; i < grdWT.Rows.Count; i++)
                    {

                        TextBox txtWTIdMark = (TextBox)grdWT.Rows[i].FindControl("txtWTIdMark");
                        TextBox txtT1 = (TextBox)grdWT.Rows[i].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdWT.Rows[i].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdWT.Rows[i].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdWT.Rows[i].FindControl("txtT4");
                        TextBox txtT5 = (TextBox)grdWT.Rows[i].FindControl("txtT5");
                        TextBox txtT6 = (TextBox)grdWT.Rows[i].FindControl("txtT6");
                        TextBox txtWtLeadShots = (TextBox)grdWT.Rows[i].FindControl("txtWtLeadShots");

                        if (txtWTIdMark.Text == "")
                        {
                            dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 3;
                            txtWTIdMark.Focus();
                            valid = false;                            
                            break;
                        }
                        else if (txtT1.Text == "")
                        {
                            dispalyMsg = "Enter T1 for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 3;
                            txtT1.Focus();
                            valid = false;                           
                            break;
                        }
                        else if (txtT2.Text == "")
                        {
                            dispalyMsg = "Enter T2 for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 3;
                            txtT2.Focus();
                            valid = false;                            
                            break;
                        }
                        else if (txtT3.Text == "")
                        {
                            dispalyMsg = "Enter T3 for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 3;
                            txtT3.Focus();
                            valid = false;                            
                            break;
                        }
                        else if (txtT4.Text == "")
                        {
                            dispalyMsg = "Enter T4 for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 3;
                            txtT4.Focus();
                            valid = false;
                           
                            break;
                        }
                        else if (txtT5.Text == "")
                        {
                            dispalyMsg = "Enter T5 for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 3;
                            txtT5.Focus();
                            valid = false;
                           
                            break;
                        }
                        else if (txtT6.Text == "")
                        {
                            dispalyMsg = "Enter T6 for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 3;
                            txtT6.Focus();
                            valid = false;                            
                            break;
                        }
                        else if (txtWtLeadShots.Text == "")
                        {
                            dispalyMsg = "Enter Wt. of Lead Shots / Wt. of Load  for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 3;
                            txtWtLeadShots.Focus();
                            valid = false;                            
                            break;
                        }
                    }
                }

                #endregion
            }
            if (TabMR.Visible == true)
            {
                #region validate Modulus of Rupture data
                if (txtDiameterOfRod.Text == "")
                {
                    dispalyMsg = "Please Enter value for Diameter Of Rod";
                    txtDiameterOfRod.Focus();
                    TabContainerTile.ActiveTabIndex = 4;
                    valid = false;
                }
                else if (txtMORL.Text == "")
                {
                    dispalyMsg = "Please Enter value for L";
                    txtMORL.Focus();
                    TabContainerTile.ActiveTabIndex = 4;
                    valid = false;
                }

                else if (txtThicknessOfRubber.Text == "")
                {
                    dispalyMsg = "Please Enter value for Thickness Of Rubber";
                    txtThicknessOfRubber.Focus();
                    TabContainerTile.ActiveTabIndex = 4;
                    valid = false;
                }
                else if (txtMORB.Text == "")
                {
                    dispalyMsg = "Please Enter value for B";
                    txtMORB.Focus();
                    TabContainerTile.ActiveTabIndex = 4;
                    valid = false;
                }

                else if (txtOverlapBeyondEdge.Text == "")
                {
                    dispalyMsg = "Please Enter value for Overlap Beyond Edge";
                    txtOverlapBeyondEdge.Focus();
                    TabContainerTile.ActiveTabIndex = 4;
                    valid = false;
                }
                else if (txtMRQuantity.Text == "")
                {
                    dispalyMsg = "Please Enter Quantity";
                    txtMRQuantity.Focus();
                    TabContainerTile.ActiveTabIndex = 4;
                    valid = false;
                }
                if (valid == true)
                {
                    for (int i = 0; i < grdMR.Rows.Count; i++)
                    {

                        TextBox txtMRIdMark = (TextBox)grdMR.Rows[i].FindControl("txtMRIdMark");
                        TextBox txtT1 = (TextBox)grdMR.Rows[i].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdMR.Rows[i].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdMR.Rows[i].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdMR.Rows[i].FindControl("txtT4");
                        TextBox txtT5 = (TextBox)grdMR.Rows[i].FindControl("txtT5");
                        TextBox txtT6 = (TextBox)grdMR.Rows[i].FindControl("txtT6");
                        TextBox txtMRLeadShots = (TextBox)grdMR.Rows[i].FindControl("txtMRLeadShots");

                        if (txtMRIdMark.Text == "")
                        {
                            dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
                            txtMRIdMark.Focus();
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 4;
                            break;
                        }
                        else if (txtT1.Text == "")
                        {
                            dispalyMsg = "Enter T1 for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 4;
                            txtT1.Focus();
                            break;
                        }
                        else if (txtT2.Text == "")
                        {
                            dispalyMsg = "Enter T2 for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 4;
                            txtT2.Focus();
                            break;
                        }
                        else if (txtT3.Text == "")
                        {
                            dispalyMsg = "Enter T3 for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 4;
                            txtT3.Focus();
                            break;
                        }
                        else if (txtT4.Text == "")
                        {
                            dispalyMsg = "Enter T4 for row no " + (i + 1) + ".";

                            valid = false;
                            TabContainerTile.ActiveTabIndex = 4;
                            txtT4.Focus();
                            break;
                        }
                        else if (txtT5.Text == "")
                        {
                            dispalyMsg = "Enter T5 for row no " + (i + 1) + ".";

                            valid = false;
                            TabContainerTile.ActiveTabIndex = 4;
                            txtT5.Focus();
                            break;
                        }
                        else if (txtT6.Text == "")
                        {
                            dispalyMsg = "Enter T6 for row no " + (i + 1) + ".";

                            valid = false;
                            TabContainerTile.ActiveTabIndex = 4;
                            txtT6.Focus();
                            break;
                        }
                        else if (txtMRLeadShots.Text == "")
                        {
                            dispalyMsg = "Enter Wt. of Lead Shots / Wt. of Load(KN) for row no " + (i + 1) + ".";
                            txtMRLeadShots.Focus();
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 4;
                            break;
                        }
                    }
                }

                #endregion
            }
            if (TabCR.Visible == true)
            {
                #region validate Crazing Resistance data
                if (valid == true)
                {
                    for (int i = 0; i < grdCR.Rows.Count; i++)
                    {
                        TextBox txtIdMark = (TextBox)grdCR.Rows[i].FindControl("txtIdMark");
                        DropDownList ddlCrazing = (DropDownList)grdCR.Rows[i].FindControl("ddlCrazing");
                        if (txtIdMark.Text == "")
                        {
                            dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 0;
                            txtIdMark.Focus();
                            valid = false;
                            break;
                        }
                        else if (ddlCrazing.SelectedItem.Text == "---Select---")
                        {
                            dispalyMsg = "Select Crazing for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 0;
                            ddlCrazing.Focus();
                            valid = false;
                            break;
                        }
                    }
                }
                #endregion
            }
            if (TabSH.Visible == true)
            {
                #region validate Scratch Hardness data
                if (valid == true)
                {
                    for (int i = 0; i < grdSH.Rows.Count; i++)
                    {
                        TextBox txtSHIdMark = (TextBox)grdSH.Rows[i].FindControl("txtSHIdMark");
                        TextBox txtHardness = (TextBox)grdSH.Rows[i].FindControl("txtHardness");

                        if (txtSHIdMark.Text == "")
                        {
                            dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 1;
                            txtSHIdMark.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtHardness.Text == "")
                        {
                            dispalyMsg = "Enter Scratch Hardness for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 1;
                            txtHardness.Focus();
                            valid = false;
                            break;
                        }
                    }
                }

                #endregion
            }
            if (TabWA.Visible == true)
            {
                #region validate Water Absorption data
                if (valid == true)
                {
                    for (int i = 0; i < grdWA.Rows.Count; i++)
                    {
                        TextBox txtWAIdMark = (TextBox)grdWA.Rows[i].FindControl("txtWAIdMark");
                        TextBox txtWADryWt = (TextBox)grdWA.Rows[i].FindControl("txtWADryWt");
                        TextBox txtWAWetWt = (TextBox)grdWA.Rows[i].FindControl("txtWAWetWt");
                        TextBox txtWA = (TextBox)grdWA.Rows[i].FindControl("txtWA");
                        TextBox txtWAAvg = (TextBox)grdWA.Rows[i].FindControl("txtWAAvg");

                        if (txtWAIdMark.Text == "")
                        {
                            dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 2;
                            txtWAIdMark.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtWADryWt.Text == "")
                        {
                            dispalyMsg = "Enter Dry Weight for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 2;
                            txtWADryWt.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtWAWetWt.Text == "")
                        {
                            dispalyMsg = "Enter Wet Weight for row no " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 2;
                            txtWAWetWt.Focus();
                            valid = false;
                            break;
                        }
                        else if (Convert.ToDecimal(txtWADryWt.Text) > Convert.ToDecimal(txtWAWetWt.Text))
                        {
                            dispalyMsg = "Dry wt. must be less than Wet Wt. for row number " + (i + 1) + ".";
                            TabContainerTile.ActiveTabIndex = 2;
                            txtWAWetWt.Focus();
                            valid = false;
                            break;
                        }
                    }
                }
                #endregion
            }

            if (TabDA.Visible == true)
            {
                #region validate Dimension Analysis data
                if (valid == true)
                {
                    for (int i = 0; i < grdDA.Rows.Count; i++)
                    {

                        TextBox txtDAIdMark = (TextBox)grdDA.Rows[i].FindControl("txtDAIdMark");

                        TextBox txtL1 = (TextBox)grdDA.Rows[i].FindControl("txtL1");
                        TextBox txtL2 = (TextBox)grdDA.Rows[i].FindControl("txtL2");
                        TextBox txtL3 = (TextBox)grdDA.Rows[i].FindControl("txtL3");
                        //Width
                        TextBox txtW1 = (TextBox)grdDA.Rows[i].FindControl("txtW1");
                        TextBox txtW2 = (TextBox)grdDA.Rows[i].FindControl("txtW2");
                        TextBox txtW3 = (TextBox)grdDA.Rows[i].FindControl("txtW3");

                        TextBox txtT1 = (TextBox)grdDA.Rows[i].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdDA.Rows[i].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdDA.Rows[i].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdDA.Rows[i].FindControl("txtT4");
                        //TextBox txtT5 = (TextBox)grdDA.Rows[i].FindControl("txtT5");
                        //TextBox txtT6 = (TextBox)grdDA.Rows[i].FindControl("txtT6");
                        //TextBox txtT7 = (TextBox)grdDA.Rows[i].FindControl("txtT7");
                        //TextBox txtT8 = (TextBox)grdDA.Rows[i].FindControl("txtT8");
                        //TextBox txtT9 = (TextBox)grdDA.Rows[i].FindControl("txtT9");
                        //TextBox txtT10 = (TextBox)grdDA.Rows[i].FindControl("txtT10");
                        //TextBox txtT11 = (TextBox)grdDA.Rows[i].FindControl("txtT11");
                        //TextBox txtT12 = (TextBox)grdDA.Rows[i].FindControl("txtT12");

                        if (txtDAIdMark.Text == "")
                        {
                            dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 5;
                            txtDAIdMark.Focus();
                            break;
                        }
                        else if (txtL1.Text == "")
                        {
                            dispalyMsg = "Enter L1 for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 5;
                            txtL1.Focus();
                            break;
                        }
                        else if (txtL2.Text == "")
                        {
                            dispalyMsg = "Enter L2 for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 5;
                            txtL2.Focus();
                            break;
                        }
                        else if (txtL3.Text == "")
                        {
                            dispalyMsg = "Enter L3 for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 5; txtL3.Focus();
                            break;
                        }
                        else if (txtW1.Text == "")
                        {
                            dispalyMsg = "Enter W1 for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 5; txtW1.Focus();
                            break;
                        }
                        else if (txtW2.Text == "")
                        {
                            dispalyMsg = "Enter W2 for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 5; txtW2.Focus();
                            break;
                        }
                        else if (txtW3.Text == "")
                        {
                            dispalyMsg = "Enter W3 for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 5; txtW3.Focus();
                            break;
                        }
                        else if (txtT1.Text == "")
                        {
                            dispalyMsg = "Enter T1 for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 5; txtT1.Focus();
                            break;
                        }
                        else if (txtT2.Text == "")
                        {
                            dispalyMsg = "Enter T2 for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 5; txtT2.Focus();
                            break;
                        }
                        else if (txtT3.Text == "")
                        {
                            dispalyMsg = "Enter T3 for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 5; txtT3.Focus();
                            break;
                        }
                        else if (txtT4.Text == "")
                        {
                            dispalyMsg = "Enter T4 for row no " + (i + 1) + ".";
                            valid = false;
                            TabContainerTile.ActiveTabIndex = 5; txtT4.Focus();
                            break;
                        }
                        //else if (txtT5.Text == "")
                        //{
                        //    dispalyMsg = "Enter T5 for row no " + (i + 1) + ".";
                        //    valid = false;
                        //    TabContainerTile.ActiveTabIndex = 5; txtT5.Focus();
                        //    break;
                        //}
                        //else if (txtT6.Text == "")
                        //{
                        //    dispalyMsg = "Enter T6 for row no " + (i + 1) + ".";
                        //    valid = false;
                        //    TabContainerTile.ActiveTabIndex = 5; txtT6.Focus();
                        //    break;
                        //}
                        //else if (txtT7.Text == "")
                        //{
                        //    dispalyMsg = "Enter T7 for row no " + (i + 1) + ".";
                        //    valid = false;
                        //    TabContainerTile.ActiveTabIndex = 5; txtT7.Focus();
                        //    break;
                        //}
                        //else if (txtT8.Text == "")
                        //{
                        //    dispalyMsg = "Enter T8 for row no " + (i + 1) + ".";
                        //    valid = false;
                        //    TabContainerTile.ActiveTabIndex = 5; txtT8.Focus();
                        //    break;
                        //}
                        //else if (txtT9.Text == "")
                        //{
                        //    dispalyMsg = "Enter T9 for row no " + (i + 1) + ".";
                        //    valid = false;
                        //    TabContainerTile.ActiveTabIndex = 5; txtT9.Focus();
                        //    break;
                        //}
                        //else if (txtT10.Text == "")
                        //{
                        //    dispalyMsg = "Enter T10 for row no " + (i + 1) + ".";
                        //    valid = false;
                        //    TabContainerTile.ActiveTabIndex = 5; txtT10.Focus();
                        //    break;
                        //}
                        //else if (txtT11.Text == "")
                        //{
                        //    dispalyMsg = "Enter T11 for row no " + (i + 1) + ".";
                        //    valid = false;
                        //    TabContainerTile.ActiveTabIndex = 5; txtT11.Focus();
                        //    break;
                        //}
                        //else if (txtT12.Text == "")
                        //{
                        //    dispalyMsg = "Enter T12 for row no " + (i + 1) + ".";
                        //    valid = false;
                        //    TabContainerTile.ActiveTabIndex = 5; txtT12.Focus();
                        //    break;
                        //}
                    }
                }
                #endregion
            }

            #region validate Remark data
            if (valid == true)
            {
                if (grdRemark.Rows.Count > 1)
                {
                    for (int i = 0; i < grdRemark.Rows.Count; i++)
                    {
                        TextBox txtRemark = (TextBox)grdRemark.Rows[i].FindControl("txtRemark");
                        txtRemark.Text = txtRemark.Text.Trim();
                        if (txtRemark.Text == "" && grdRemark.Rows.Count > 0)
                        {
                            dispalyMsg = "Please Enter Remark.";
                            TabContainerTile.ActiveTabIndex = 6;
                            txtRemark.Focus();
                            valid = false;
                            break;
                        }
                    }
                }
            }
            if (TabSpecifiedLimit.Visible == true)
            {
                if (TabMR.Visible == true || TabWA.Visible == true)
                {
                    if (valid == true)
                    {
                        if (txtIsCode.Text == "")
                        {
                            dispalyMsg = "Please Enter value for IS Code";
                            txtIsCode.Focus();
                            TabContainerTile.ActiveTabIndex = 7;
                            valid = false;
                        }
                    }
                }
            }
            #endregion

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
        protected Boolean ValidateDataForWT()
        {
            string dispalyMsg = "";
            Boolean valid = true;
            #region validate WT data
            if (valid == true)
            {
                for (int i = 0; i < grdWT.Rows.Count; i++)
                {

                    TextBox txtWTIdMark = (TextBox)grdWT.Rows[i].FindControl("txtWTIdMark");
                    TextBox txtT1 = (TextBox)grdWT.Rows[i].FindControl("txtT1");
                    TextBox txtT2 = (TextBox)grdWT.Rows[i].FindControl("txtT2");
                    TextBox txtT3 = (TextBox)grdWT.Rows[i].FindControl("txtT3");
                    TextBox txtT4 = (TextBox)grdWT.Rows[i].FindControl("txtT4");
                    TextBox txtT5 = (TextBox)grdWT.Rows[i].FindControl("txtT5");
                    TextBox txtT6 = (TextBox)grdWT.Rows[i].FindControl("txtT6");
                    TextBox txtWtLeadShots = (TextBox)grdWT.Rows[i].FindControl("txtWtLeadShots");

                    if (txtWTIdMark.Text == "")
                    {
                        dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
                        txtWTIdMark.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 3;
                        break;
                    }
                    else if (txtT1.Text == "")
                    {
                        dispalyMsg = "Enter T1 for row no " + (i + 1) + ".";
                        txtT1.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 3;
                        break;
                    }
                    else if (txtT2.Text == "")
                    {
                        dispalyMsg = "Enter T2 for row no " + (i + 1) + ".";
                        txtT2.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 3;
                        break;
                    }
                    else if (txtT3.Text == "")
                    {
                        dispalyMsg = "Enter T3 for row no " + (i + 1) + ".";
                        txtT3.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 3;
                        break;
                    }
                    else if (txtT4.Text == "")
                    {
                        dispalyMsg = "Enter T4 for row no " + (i + 1) + ".";
                        txtT4.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 3;
                        break;
                    }
                    else if (txtT5.Text == "")
                    {
                        dispalyMsg = "Enter T5 for row no " + (i + 1) + ".";
                        txtT5.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 3;
                        break;
                    }
                    else if (txtT6.Text == "")
                    {
                        dispalyMsg = "Enter T6 for row no " + (i + 1) + ".";
                        txtT6.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 3;
                        break;
                    }
                    else if (txtWtLeadShots.Text == "")
                    {
                        dispalyMsg = "Enter Wt. of Lead Shots / Wt. of Load  for row no " + (i + 1) + ".";
                        txtWtLeadShots.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 3;
                        break;
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
        protected Boolean ValidateDataForMR()
        {
            string dispalyMsg = "";
            Boolean valid = true;

            #region validate MR data
            if (valid == true)
            {
                for (int i = 0; i < grdMR.Rows.Count; i++)
                {

                    TextBox txtMRIdMark = (TextBox)grdMR.Rows[i].FindControl("txtMRIdMark");
                    TextBox txtT1 = (TextBox)grdMR.Rows[i].FindControl("txtT1");
                    TextBox txtT2 = (TextBox)grdMR.Rows[i].FindControl("txtT2");
                    TextBox txtT3 = (TextBox)grdMR.Rows[i].FindControl("txtT3");
                    TextBox txtT4 = (TextBox)grdMR.Rows[i].FindControl("txtT4");
                    TextBox txtT5 = (TextBox)grdMR.Rows[i].FindControl("txtT5");
                    TextBox txtT6 = (TextBox)grdMR.Rows[i].FindControl("txtT6");
                    TextBox txtMRLeadShots = (TextBox)grdMR.Rows[i].FindControl("txtMRLeadShots");

                    if (txtMRIdMark.Text == "")
                    {
                        dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
                        txtMRIdMark.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 4;
                        break;
                    }
                    else if (txtT1.Text == "")
                    {
                        dispalyMsg = "Enter T1 for row no " + (i + 1) + ".";
                        txtT1.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 4;
                        break;
                    }
                    else if (txtT2.Text == "")
                    {
                        dispalyMsg = "Enter T2 for row no " + (i + 1) + ".";
                        txtT2.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 4;
                        break;
                    }
                    else if (txtT3.Text == "")
                    {
                        dispalyMsg = "Enter T3 for row no " + (i + 1) + ".";
                        txtT3.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 4;
                        break;
                    }
                    else if (txtT4.Text == "")
                    {
                        dispalyMsg = "Enter T4 for row no " + (i + 1) + ".";
                        txtT4.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 4;
                        break;
                    }
                    else if (txtT5.Text == "")
                    {
                        dispalyMsg = "Enter T5 for row no " + (i + 1) + ".";
                        txtT5.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 4;
                        break;
                    }
                    else if (txtT6.Text == "")
                    {
                        dispalyMsg = "Enter T6 for row no " + (i + 1) + ".";
                        txtT6.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 4;
                        break;
                    }
                    else if (txtMRLeadShots.Text == "")
                    {
                        dispalyMsg = "Enter Wt. of Lead Shots / Wt. of Load(KN) for row no " + (i + 1) + ".";
                        txtMRLeadShots.Focus();
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 4;
                        break;
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
        protected Boolean ValidateDataForDA()
        {
            string dispalyMsg = "";
            Boolean valid = true;

            #region validate DA data
            if (valid == true)
            {
                for (int i = 0; i < grdDA.Rows.Count; i++)
                {

                    TextBox txtDAIdMark = (TextBox)grdDA.Rows[i].FindControl("txtDAIdMark");

                    TextBox txtL1 = (TextBox)grdDA.Rows[i].FindControl("txtL1");
                    TextBox txtL2 = (TextBox)grdDA.Rows[i].FindControl("txtL2");
                    TextBox txtL3 = (TextBox)grdDA.Rows[i].FindControl("txtL3");
                    //Width
                    TextBox txtW1 = (TextBox)grdDA.Rows[i].FindControl("txtW1");
                    TextBox txtW2 = (TextBox)grdDA.Rows[i].FindControl("txtW2");
                    TextBox txtW3 = (TextBox)grdDA.Rows[i].FindControl("txtW3");

                    TextBox txtT1 = (TextBox)grdDA.Rows[i].FindControl("txtT1");
                    TextBox txtT2 = (TextBox)grdDA.Rows[i].FindControl("txtT2");
                    TextBox txtT3 = (TextBox)grdDA.Rows[i].FindControl("txtT3");
                    TextBox txtT4 = (TextBox)grdDA.Rows[i].FindControl("txtT4");
                    //TextBox txtT5 = (TextBox)grdDA.Rows[i].FindControl("txtT5");
                    //TextBox txtT6 = (TextBox)grdDA.Rows[i].FindControl("txtT6");
                    //TextBox txtT7 = (TextBox)grdDA.Rows[i].FindControl("txtT7");
                    //TextBox txtT8 = (TextBox)grdDA.Rows[i].FindControl("txtT8");
                    //TextBox txtT9 = (TextBox)grdDA.Rows[i].FindControl("txtT9");
                    //TextBox txtT10 = (TextBox)grdDA.Rows[i].FindControl("txtT10");
                    //TextBox txtT11 = (TextBox)grdDA.Rows[i].FindControl("txtT11");
                    //TextBox txtT12 = (TextBox)grdDA.Rows[i].FindControl("txtT12");

                    if (txtDAIdMark.Text == "")
                    {
                        dispalyMsg = "Enter ID Mark for row no " + (i + 1) + ".";
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 5;
                        txtDAIdMark.Focus();
                        break;
                    }
                    else if (txtL1.Text == "")
                    {
                        dispalyMsg = "Enter L1 for row no " + (i + 1) + ".";
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 5;
                        txtL1.Focus();
                        break;
                    }
                    else if (txtL2.Text == "")
                    {
                        dispalyMsg = "Enter L2 for row no " + (i + 1) + ".";
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 5;
                        txtL2.Focus();
                        break;
                    }
                    else if (txtL3.Text == "")
                    {
                        dispalyMsg = "Enter L3 for row no " + (i + 1) + ".";
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 5; txtL3.Focus();
                        break;
                    }
                    else if (txtW1.Text == "")
                    {
                        dispalyMsg = "Enter W1 for row no " + (i + 1) + ".";
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 5; txtW1.Focus();
                        break;
                    }
                    else if (txtW2.Text == "")
                    {
                        dispalyMsg = "Enter W2 for row no " + (i + 1) + ".";
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 5; txtW2.Focus();
                        break;
                    }
                    else if (txtW3.Text == "")
                    {
                        dispalyMsg = "Enter W3 for row no " + (i + 1) + ".";
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 5; txtW3.Focus();
                        break;
                    }
                    else if (txtT1.Text == "")
                    {
                        dispalyMsg = "Enter T1 for row no " + (i + 1) + ".";
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 5; txtT1.Focus();
                        break;
                    }
                    else if (txtT2.Text == "")
                    {
                        dispalyMsg = "Enter T2 for row no " + (i + 1) + ".";
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 5; txtT2.Focus();
                        break;
                    }
                    else if (txtT3.Text == "")
                    {
                        dispalyMsg = "Enter T3 for row no " + (i + 1) + ".";
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 5; txtT3.Focus();
                        break;
                    }
                    else if (txtT4.Text == "")
                    {
                        dispalyMsg = "Enter T4 for row no " + (i + 1) + ".";
                        valid = false;
                        TabContainerTile.ActiveTabIndex = 5; txtT4.Focus();
                        break;
                    }
                    //else if (txtT5.Text == "")
                    //{
                    //    dispalyMsg = "Enter T5 for row no " + (i + 1) + ".";
                    //    valid = false;
                    //    TabContainerTile.ActiveTabIndex = 5; txtT5.Focus();
                    //    break;
                    //}
                    //else if (txtT6.Text == "")
                    //{
                    //    dispalyMsg = "Enter T6 for row no " + (i + 1) + ".";
                    //    valid = false;
                    //    TabContainerTile.ActiveTabIndex = 5; txtT6.Focus();
                    //    break;
                    //}
                    //else if (txtT7.Text == "")
                    //{
                    //    dispalyMsg = "Enter T7 for row no " + (i + 1) + ".";
                    //    valid = false;
                    //    TabContainerTile.ActiveTabIndex = 5; txtT7.Focus();
                    //    break;
                    //}
                    //else if (txtT8.Text == "")
                    //{
                    //    dispalyMsg = "Enter T8 for row no " + (i + 1) + ".";
                    //    valid = false;
                    //    TabContainerTile.ActiveTabIndex = 5; txtT8.Focus();
                    //    break;
                    //}
                    //else if (txtT9.Text == "")
                    //{
                    //    dispalyMsg = "Enter T9 for row no " + (i + 1) + ".";
                    //    valid = false;
                    //    TabContainerTile.ActiveTabIndex = 5; txtT9.Focus();
                    //    break;
                    //}
                    //else if (txtT10.Text == "")
                    //{
                    //    dispalyMsg = "Enter T10 for row no " + (i + 1) + ".";
                    //    valid = false;
                    //    TabContainerTile.ActiveTabIndex = 5; txtT10.Focus();
                    //    break;
                    //}
                    //else if (txtT11.Text == "")
                    //{
                    //    dispalyMsg = "Enter T11 for row no " + (i + 1) + ".";
                    //    valid = false;
                    //    TabContainerTile.ActiveTabIndex = 5; txtT11.Focus();
                    //    break;
                    //}
                    //else if (txtT12.Text == "")
                    //{
                    //    dispalyMsg = "Enter T12 for row no " + (i + 1) + ".";
                    //    valid = false;
                    //    TabContainerTile.ActiveTabIndex = 5; txtT12.Focus();
                    //    break;
                    //}
                }
            }

            #endregion

            if (valid == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
            return valid;
        }

        private void Calculate()
        {
            if (TabContainerTile.ActiveTabIndex == 2)
            {
                CalculateWA();
            }
            if (TabContainerTile.ActiveTabIndex == 3)
            {
                CalculateWT();
            }
            if (TabContainerTile.ActiveTabIndex == 4)
            {
                CalculateMR();
            }
            if (TabContainerTile.ActiveTabIndex == 5)
            {
                CalculateDA();
            }
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
                TextBox txtWAAvg = (TextBox)grdWA.Rows[i].FindControl("txtWAAvg");
                txtWAAvg.Text = "";
                wa = 0;
                if (txtWADryWt.Text != "" && txtWAWetWt.Text != "")
                {
                    if (Convert.ToDecimal(txtWADryWt.Text) > Convert.ToDecimal(txtWAWetWt.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning", "alert('Dry Weight must be less than Wet Weight for row no " + i + ".');", true);
                        break;
                    }
                    else
                    {
                        if (Convert.ToDecimal(txtWADryWt.Text) > 0)
                            wa = (100 * (Convert.ToDecimal(txtWAWetWt.Text) - Convert.ToDecimal(txtWADryWt.Text))) / Convert.ToDecimal(txtWADryWt.Text);
                        else
                            wa = 100 * (Convert.ToDecimal(txtWAWetWt.Text) - Convert.ToDecimal(txtWADryWt.Text));

                        txtWA.Text = wa.ToString("0.00");
                        avgwa = avgwa + wa;
                    }
                }
            }
            if (txtWAQuantity.Text != "")
            {
                TextBox txtWAAvg = (TextBox)grdWA.Rows[(grdWA.Rows.Count / 2)].FindControl("txtWAAvg"); //ceramic - 10 !=ceramic - 6
                if ((ddlTileType.SelectedItem.Text == "Ceramic" && Convert.ToInt32(txtWAQuantity.Text) < 5) || 
                    (ddlTileType.SelectedItem.Text != "Ceramic" && Convert.ToInt32(txtWAQuantity.Text) < 5))
                { 
                    txtWAAvg.Text = "***"; 
                }
                else
                {
                    txtWAAvg.Text = "0.00";
                    if (avgwa > 0)
                        txtWAAvg.Text = (avgwa / grdWA.Rows.Count).ToString("0.00");
                }
            }
        }
        private void CalculateDA()
        {
            if (ValidateDataForDA() == true)
            {
                try
                {
                    decimal len = 0, wdh = 0, het = 0;
                    decimal totlen = 0, totwdh = 0, totThick = 0;

                    for (int i = 0; i < grdDA.Rows.Count; i++)
                    {

                        TextBox txtDAIdMark = (TextBox)grdDA.Rows[i].FindControl("txtDAIdMark");
                        TextBox txtDACalIdMark = (TextBox)grdDACal.Rows[i].FindControl("txtDACalIdMark");

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

                        TextBox txtLength = (TextBox)grdDACal.Rows[i].FindControl("txtLength");
                        txtLength.Text = "0.00";
                        if (len > 0)
                        {
                            txtLength.Text = (len / 3).ToString("0.00");
                        }
                        totlen = totlen + Convert.ToDecimal(txtLength.Text);

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

                        TextBox txtWidth = (TextBox)grdDACal.Rows[i].FindControl("txtWidth");
                        txtWidth.Text = "0.00";
                        if (wdh > 0)
                        {
                            txtWidth.Text = (wdh / 3).ToString("0.00");
                        }
                        totwdh = totwdh + Convert.ToDecimal(txtWidth.Text);

                        //Height
                        TextBox txtT1 = (TextBox)grdDA.Rows[i].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdDA.Rows[i].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdDA.Rows[i].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdDA.Rows[i].FindControl("txtT4");
                        //TextBox txtT5 = (TextBox)grdDA.Rows[i].FindControl("txtT5");
                        //TextBox txtT6 = (TextBox)grdDA.Rows[i].FindControl("txtT6");
                        //TextBox txtT7 = (TextBox)grdDA.Rows[i].FindControl("txtT7");
                        //TextBox txtT8 = (TextBox)grdDA.Rows[i].FindControl("txtT8");
                        //TextBox txtT9 = (TextBox)grdDA.Rows[i].FindControl("txtT9");
                        //TextBox txtT10 = (TextBox)grdDA.Rows[i].FindControl("txtT10");
                        //TextBox txtT11 = (TextBox)grdDA.Rows[i].FindControl("txtT11");
                        //TextBox txtT12 = (TextBox)grdDA.Rows[i].FindControl("txtT12");
                        
                        het = 0;
                        if (txtT1.Text != "")
                            het = het + Convert.ToDecimal(txtT1.Text);
                        if (txtT2.Text != "")
                            het = het + Convert.ToDecimal(txtT2.Text);
                        if (txtT3.Text != "")
                            het = het + Convert.ToDecimal(txtT3.Text);
                        if (txtT4.Text != "")
                            het = het + Convert.ToDecimal(txtT4.Text);
                        //if (txtT5.Text != "")
                        //    het = het + Convert.ToDecimal(txtT5.Text);
                        //if (txtT6.Text != "")
                        //    het = het + Convert.ToDecimal(txtT6.Text);
                        //if (txtT7.Text != "")
                        //    het = het + Convert.ToDecimal(txtT7.Text);
                        //if (txtT8.Text != "")
                        //    het = het + Convert.ToDecimal(txtT8.Text);
                        //if (txtT9.Text != "")
                        //    het = het + Convert.ToDecimal(txtT9.Text);
                        //if (txtT10.Text != "")
                        //    het = het + Convert.ToDecimal(txtT10.Text);
                        //if (txtT11.Text != "")
                        //    het = het + Convert.ToDecimal(txtT11.Text);
                        //if (txtT12.Text != "")
                        //    het = het + Convert.ToDecimal(txtT12.Text);

                        TextBox txtThickness = (TextBox)grdDACal.Rows[i].FindControl("txtThickness");
                        txtThickness.Text = "0.00";
                        if (het > 0)
                        {
                            txtThickness.Text = (het / 4).ToString("0.00");
                        }
                        totThick = totThick + Convert.ToDecimal(txtThickness.Text);
                    }
                    TextBox txtAvgLen = null;
                    TextBox txtAvgWidth = null;
                    TextBox txtAvgThickness = null;

                    for (int i = 0; i < grdDACal.Rows.Count; i++)
                    {
                        txtAvgLen = (TextBox)grdDACal.Rows[i].FindControl("txtAvgLen");
                        txtAvgLen.Text = "";
                        txtAvgWidth = (TextBox)grdDACal.Rows[i].FindControl("txtAvgWidth");
                        txtAvgWidth.Text = "";
                        txtAvgThickness = (TextBox)grdDACal.Rows[i].FindControl("txtAvgThickness");
                        txtAvgThickness.Text = "";
                    }
                    
                    txtAvgLen = (TextBox)grdDACal.Rows[(grdDACal.Rows.Count / 2)].FindControl("txtAvgLen");
                    txtAvgWidth = (TextBox)grdDACal.Rows[(grdDACal.Rows.Count / 2)].FindControl("txtAvgWidth");
                    txtAvgThickness = (TextBox)grdDACal.Rows[(grdDACal.Rows.Count / 2)].FindControl("txtAvgThickness");

                    if (grdDACal.Rows.Count < 10)
                    {
                        txtAvgLen.Text = "***";
                        txtAvgWidth.Text = "***";
                        txtAvgThickness.Text = "***";
                    }
                    else
                    {
                        txtAvgLen.Text = (totlen / grdDACal.Rows.Count).ToString("0.00");
                        txtAvgWidth.Text = (totwdh / grdDACal.Rows.Count).ToString("0.00");
                        txtAvgThickness.Text = (totThick / grdDACal.Rows.Count).ToString("0.00");
                    }
                }
                catch
                {
                }
            }

        }
        private void CalculateWT()
        {
            if (ValidateDataForWT() == true)
            {
                //try
                //{
                    decimal het = 0, totAvg = 0;

                    for (int i = 0; i < grdWT.Rows.Count; i++)
                    {
                        TextBox txtWTIdMark = (TextBox)grdWT.Rows[i].FindControl("txtWTIdMark");
                        TextBox txtWTCalIdMark = (TextBox)grdWTCal.Rows[i].FindControl("txtWTCalIdMark");
                        TextBox txtWtLeadShots = (TextBox)grdWT.Rows[i].FindControl("txtWtLeadShots");

                        txtWTCalIdMark.Text = txtWTIdMark.Text;

                        //Height
                        TextBox txtT1 = (TextBox)grdWT.Rows[i].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdWT.Rows[i].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdWT.Rows[i].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdWT.Rows[i].FindControl("txtT4");
                        TextBox txtT5 = (TextBox)grdWT.Rows[i].FindControl("txtT5");
                        TextBox txtT6 = (TextBox)grdWT.Rows[i].FindControl("txtT6");

                        het = 0;
                        if (txtT1.Text != "")
                            het = het + Convert.ToDecimal(txtT1.Text);
                        if (txtT2.Text != "")
                            het = het + Convert.ToDecimal(txtT2.Text);
                        if (txtT3.Text != "")
                            het = het + Convert.ToDecimal(txtT3.Text);
                        if (txtT4.Text != "")
                            het = het + Convert.ToDecimal(txtT4.Text);
                        if (txtT5.Text != "")
                            het = het + Convert.ToDecimal(txtT5.Text);
                        if (txtT6.Text != "")
                            het = het + Convert.ToDecimal(txtT6.Text);

                        TextBox txtWtB = (TextBox)grdWTCal.Rows[i].FindControl("txtWtB");
                        TextBox txtWtN = (TextBox)grdWTCal.Rows[i].FindControl("txtWtN");
                        TextBox txtWtTransverse = (TextBox)grdWTCal.Rows[i].FindControl("txtWtTransverse");
                        TextBox txtWetTr = (TextBox)grdWTCal.Rows[i].FindControl("txtWetTr");

                        TextBox txtWTThickness = (TextBox)grdWTCal.Rows[i].FindControl("txtWTThickness");
                        txtWTThickness.Text = "0.00";
                        if (het > 0)
                        {
                            txtWTThickness.Text = (het / 6).ToString("0.00");
                        }

                        TextBox txtWtCalLeadShots = (TextBox)grdWTCal.Rows[i].FindControl("txtWtCalLeadShots");
                        // added on 16/09/19 change in put in N
                        
                        txtWtCalLeadShots.Text = Convert.ToString( Convert.ToDecimal(txtWtLeadShots.Text.ToString()) / Convert.ToDecimal (9.81 * 12));

                        txtWtCalLeadShots.Text = Convert.ToDecimal(txtWtCalLeadShots.Text).ToString("0.00");

                        decimal valueforB = Convert.ToDecimal(txtWtCalLeadShots.Text) * 12;
                        txtWtB.Text = valueforB.ToString("0.00");

                        string multiplicationValue = "9.81";
                        decimal valueforN = Convert.ToDecimal(txtWtB.Text) * Convert.ToDecimal(multiplicationValue);
                        txtWtN.Text = valueforN.ToString("0.00");


                        decimal valueForTransverse = (3 * valueforN * Convert.ToDecimal(txtL.Text)) / (2 * Convert.ToDecimal(txtB.Text) * Convert.ToDecimal(txtWTThickness.Text) * Convert.ToDecimal(txtWTThickness.Text));
                        txtWtTransverse.Text = valueForTransverse.ToString("0.00");

                        txtWetTr.Text = txtWtTransverse.Text;

                        totAvg = totAvg + Convert.ToDecimal(txtWetTr.Text);
                    }

                    TextBox txtAvgWetTr = null;
                    for (int i = 0; i < grdWTCal.Rows.Count; i++)
                    {
                        txtAvgWetTr = (TextBox)grdWTCal.Rows[i].FindControl("txtAvgWetTr");
                        txtAvgWetTr.Text = "";
                    }

                    txtAvgWetTr = (TextBox)grdWTCal.Rows[(grdWTCal.Rows.Count / 2)].FindControl("txtAvgWetTr");
                    if (grdWTCal.Rows.Count < 6)
                    {
                        txtAvgWetTr.Text = "***";
                    }
                    else
                    {
                        txtAvgWetTr.Text = (totAvg / grdWTCal.Rows.Count).ToString("0.00");
                    }
                //}
                //catch { }
            }

        }
        private void CalculateMR()
        {
            if (ValidateDataForMR() == true)
            {
                try
                {
                    decimal het = 0;
                    decimal totAvgMOR = 0, totAvgBreakLoad = 0, totAvgBreakStr = 0; //, totThick = 0;

                    for (int i = 0; i < grdMR.Rows.Count; i++)
                    {
                        TextBox txtMRIdMark = (TextBox)grdMR.Rows[i].FindControl("txtMRIdMark");
                        TextBox txtMRCalIdMark = (TextBox)grdMRCal.Rows[i].FindControl("txtMRCalIdMark");
                        TextBox txtMRLeadShots = (TextBox)grdMR.Rows[i].FindControl("txtMRLeadShots");

                        txtMRCalIdMark.Text = txtMRIdMark.Text;

                        //Height
                        TextBox txtT1 = (TextBox)grdMR.Rows[i].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdMR.Rows[i].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdMR.Rows[i].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdMR.Rows[i].FindControl("txtT4");
                        TextBox txtT5 = (TextBox)grdMR.Rows[i].FindControl("txtT5");
                        TextBox txtT6 = (TextBox)grdMR.Rows[i].FindControl("txtT6");

                        het = 0;
                        if (txtT1.Text != "")
                            het = het + Convert.ToDecimal(txtT1.Text);
                        if (txtT2.Text != "")
                            het = het + Convert.ToDecimal(txtT2.Text);
                        if (txtT3.Text != "")
                            het = het + Convert.ToDecimal(txtT3.Text);
                        if (txtT4.Text != "")
                            het = het + Convert.ToDecimal(txtT4.Text);
                        if (txtT5.Text != "")
                            het = het + Convert.ToDecimal(txtT5.Text);
                        if (txtT6.Text != "")
                            het = het + Convert.ToDecimal(txtT6.Text);

                        TextBox txtMRCalLeadShots = (TextBox)grdMRCal.Rows[i].FindControl("txtMRCalLeadShots");
                        TextBox txtMRThickness = (TextBox)grdMRCal.Rows[i].FindControl("txtMRThickness");
                        TextBox txtMRB = (TextBox)grdMRCal.Rows[i].FindControl("txtMRB");
                        TextBox txtMRN = (TextBox)grdMRCal.Rows[i].FindControl("txtMRN");
                        TextBox txtBreakStrength = (TextBox)grdMRCal.Rows[i].FindControl("txtBreakStrength");
                        TextBox txtMRTransverse = (TextBox)grdMRCal.Rows[i].FindControl("txtMRTransverse");
                        TextBox txtMOR = (TextBox)grdMRCal.Rows[i].FindControl("txtMOR");

                        txtMRCalLeadShots.Text = txtMRLeadShots.Text;

                        txtMRThickness.Text = "0.00";
                        if (het > 0)
                        {
                            txtMRThickness.Text = (het / 6).ToString("0.00");
                        }

                        decimal valueforB = Convert.ToDecimal(txtMRCalLeadShots.Text) * 12;
                        txtMRB.Text = valueforB.ToString("0.00");
                        decimal valueforN = 0;
                        if (RdbMRMachine.SelectedValue == "Mechanical Flexture Machine")
                        {
                            string multiplicationValue = "9.81";
                            valueforN = Convert.ToDecimal(txtMRB.Text) * Convert.ToDecimal(multiplicationValue);
                            txtMRN.Text = valueforN.ToString("0.00");
                        }
                        else if (RdbMRMachine.SelectedValue == "Digital Flexture Machine")
                        {
                            valueforN = Convert.ToDecimal(txtMRCalLeadShots.Text) * 1000;
                            txtMRN.Text = valueforN.ToString("0.00");
                        }

                        totAvgBreakLoad = totAvgBreakLoad + valueforN;

                        decimal valueforBreakStrength = (valueforN * Convert.ToDecimal(txtMORL.Text)) / Convert.ToDecimal(txtMORB.Text);
                        txtBreakStrength.Text = valueforBreakStrength.ToString("0.00");
                        totAvgBreakStr = totAvgBreakStr + valueforBreakStrength;

                        decimal valueForTransverse = (3 * valueforN * Convert.ToDecimal(txtMORL.Text)) / (2 * Convert.ToDecimal(txtMORB.Text) * Convert.ToDecimal(txtMRThickness.Text) * Convert.ToDecimal(txtMRThickness.Text));
                        txtMRTransverse.Text = valueForTransverse.ToString("0.00");


                        txtMOR.Text = txtMRTransverse.Text;
                        totAvgMOR = totAvgMOR + Convert.ToDecimal(txtMOR.Text);
                    }


                    TextBox txtAvgBreakLoad = null;
                    TextBox txtAvgBreakStrength = null;
                    TextBox txtAvgMOR = null;
                    for (int i = 0; i < grdMRCal.Rows.Count; i++)
                    {
                        txtAvgBreakLoad = (TextBox)grdMRCal.Rows[i].FindControl("txtAvgBreakLoad");
                        txtAvgBreakLoad.Text = "";

                        txtAvgBreakStrength = (TextBox)grdMRCal.Rows[i].FindControl("txtAvgBreakStrength");
                        txtAvgBreakStrength.Text = "";

                        txtAvgMOR = (TextBox)grdMRCal.Rows[i].FindControl("txtAvgMOR");
                        txtAvgMOR.Text = "";
                    }

                    txtAvgBreakLoad = (TextBox)grdMRCal.Rows[(grdMRCal.Rows.Count / 2)].FindControl("txtAvgBreakLoad");
                    txtAvgBreakStrength = (TextBox)grdMRCal.Rows[(grdMRCal.Rows.Count / 2)].FindControl("txtAvgBreakStrength");
                    txtAvgMOR = (TextBox)grdMRCal.Rows[(grdMRCal.Rows.Count / 2)].FindControl("txtAvgMOR");

                    if (grdMRCal.Rows.Count < 5)
                    {
                        txtAvgMOR.Text = "***";
                        txtAvgBreakLoad.Text = "***";
                        txtAvgBreakStrength.Text = "***";
                    }
                    else
                    {
                        //txtAvgMOR.Text = (totAvgMOR / grdMRCal.Rows.Count).ToString("0.00");
                        txtAvgMOR.Text = Math.Round((totAvgMOR / grdMRCal.Rows.Count)).ToString();
                        txtAvgBreakLoad.Text = (totAvgBreakLoad / grdMRCal.Rows.Count).ToString("0.00");
                        txtAvgBreakStrength.Text = (totAvgBreakStr / grdMRCal.Rows.Count).ToString("0.00");
                    }
                }
                catch { }
            }

        }

        protected void lnkCalculate_Click(object sender, EventArgs e)
        {
            Calculate();
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Calculate();
                byte reportStatus = 0, enteredBy = 0, checkedBy = 0, testedBy = 0, approvedBy = 0;
                if (lblStatus.Text == "Enter")
                {
                    reportStatus = 2;
                    enteredBy = Convert.ToByte(Session["LoginId"]);
                    testedBy = Convert.ToByte(ddlTestdApprdBy.SelectedItem.Value);
                    dc.MISDetail_Update(0, "TILE", txtRefNo.Text, "TILE", null, true, false, false, false, false, false, false);
                }
                else if (lblStatus.Text == "Check")
                {
                    reportStatus = 3;
                    checkedBy = Convert.ToByte(Session["LoginId"]);
                    approvedBy = Convert.ToByte(ddlTestdApprdBy.SelectedItem.Value);
                    dc.MISDetail_Update(0, "TILE", txtRefNo.Text, "TILE", null, false, true, false, false, false, false, false);
                }
                //update NABL details
                clsData cd = new clsData();
                cd.updateNABLDetails(txtRefNo.Text, "TILE", ddl_NablScope.SelectedItem.Text, Convert.ToInt32(ddl_NABLLocation.SelectedItem.Text));


                //reportStatus = 2;
                DateTime testingDate = new DateTime();
                testingDate = DateTime.ParseExact(txtDateOfTest.Text, "dd/MM/yyyy", null);
                string ReportDetails = "";
                if (TabWA.Visible == true)                
                {
                    ReportDetails = txtIsCode.Text;
                }
                else if (TabWT.Visible == true)
                {
                    if (txtL.Text != "" && txtB.Text != "")
                        ReportDetails = RdbMachine.SelectedValue + "|" + txtL.Text + "|" + txtB.Text;
                }
                else if (TabMR.Visible == true)
                {
                    if (txtMORL.Text != "" && txtMORB.Text != "" && txtDiameterOfRod.Text != "" && txtThicknessOfRubber.Text != "" && txtOverlapBeyondEdge.Text != "" && txtIsCode.Text != "")
                        ReportDetails = RdbMRMachine.SelectedValue + "|" + txtMORL.Text + "|" + txtMORB.Text + "|" + txtDiameterOfRod.Text + "|" + txtThicknessOfRubber.Text + "|" + txtOverlapBeyondEdge.Text + "|" + txtIsCode.Text;
                }

                dc.TileInward_Update_ReportData(reportStatus, txtRefNo.Text, txtDesc.Text, txtSupplierName.Text, 0, txtWitnesBy.Text, ddlTileType.SelectedItem.Text, testedBy, enteredBy, checkedBy, approvedBy, 0, 0, 0, testingDate, ReportDetails);
                #region save CR data
                if (TabCR.Visible == true)
                {
                    dc.TileCR_Update(0, txtRefNo.Text, "", "", true);
                    for (int i = 0; i < grdCR.Rows.Count; i++)
                    {
                        TextBox txtIdMark = (TextBox)grdCR.Rows[i].FindControl("txtIdMark");
                        DropDownList ddlCrazing = (DropDownList)grdCR.Rows[i].FindControl("ddlCrazing");
                        dc.TileCR_Update(i + 1, txtRefNo.Text, txtIdMark.Text, ddlCrazing.SelectedValue, false);
                    }
                }
                #endregion

                #region save SH data
                if (TabSH.Visible == true)
                {
                    dc.TileSH_Update(0, txtRefNo.Text, "", "", true);
                    for (int i = 0; i < grdSH.Rows.Count; i++)
                    {
                        TextBox txtSHIdMark = (TextBox)grdSH.Rows[i].FindControl("txtSHIdMark");
                        TextBox txtHardness = (TextBox)grdSH.Rows[i].FindControl("txtHardness");
                        dc.TileSH_Update(i + 1, txtRefNo.Text, txtSHIdMark.Text, txtHardness.Text, false);
                    }
                }
                #endregion

                #region save WA data
                if (TabWA.Visible == true)
                {
                    dc.TileWA_Update(0, txtRefNo.Text, "", 0, 0, 0, "", true);
                    for (int i = 0; i < grdWA.Rows.Count; i++)
                    {
                        TextBox txtWAIdMark = (TextBox)grdWA.Rows[i].FindControl("txtWAIdMark");
                        TextBox txtWADryWt = (TextBox)grdWA.Rows[i].FindControl("txtWADryWt");
                        TextBox txtWAWetWt = (TextBox)grdWA.Rows[i].FindControl("txtWAWetWt");
                        TextBox txtWA = (TextBox)grdWA.Rows[i].FindControl("txtWA");
                        TextBox txtWAAvg = (TextBox)grdWA.Rows[i].FindControl("txtWAAvg");

                        dc.TileWA_Update(i + 1, txtRefNo.Text, txtWAIdMark.Text, Convert.ToDecimal(txtWADryWt.Text), Convert.ToDecimal(txtWAWetWt.Text), Convert.ToDecimal(txtWA.Text), txtWAAvg.Text, false);
                    }
                }
                #endregion

                #region save WT data
                if (TabWT.Visible == true)
                {
                    dc.TileWT_Update(0, txtRefNo.Text, "", "", 0, 0, 0, 0, 0, 0, "", true);
                    for (int i = 0; i < grdWT.Rows.Count; i++)
                    {
                        TextBox txtWTIdMark = (TextBox)grdWT.Rows[i].FindControl("txtWTIdMark");
                        TextBox txtT1 = (TextBox)grdWT.Rows[i].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdWT.Rows[i].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdWT.Rows[i].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdWT.Rows[i].FindControl("txtT4");
                        TextBox txtT5 = (TextBox)grdWT.Rows[i].FindControl("txtT5");
                        TextBox txtT6 = (TextBox)grdWT.Rows[i].FindControl("txtT6");
                        TextBox txtWtLeadShots = (TextBox)grdWT.Rows[i].FindControl("txtWtLeadShots");

                        TextBox txtWTThickness = (TextBox)grdWTCal.Rows[i].FindControl("txtWTThickness");
                        TextBox txtWtCalLeadShots = (TextBox)grdWTCal.Rows[i].FindControl("txtWtCalLeadShots");
                        TextBox txtWtB = (TextBox)grdWTCal.Rows[i].FindControl("txtWtB");
                        TextBox txtWtN = (TextBox)grdWTCal.Rows[i].FindControl("txtWtN");
                        TextBox txtWtTransverse = (TextBox)grdWTCal.Rows[i].FindControl("txtWtTransverse");
                        TextBox txtWetTr = (TextBox)grdWTCal.Rows[i].FindControl("txtWetTr");
                        TextBox txtAvgWetTr = (TextBox)grdWTCal.Rows[i].FindControl("txtAvgWetTr");

                        dc.TileWT_Update(i + 1, txtRefNo.Text, txtWTIdMark.Text, (txtT1.Text + "|" + txtT2.Text + "|" + txtT3.Text + "|" + txtT4.Text + "|" + txtT5.Text + "|" + txtT6.Text), Convert.ToDecimal(txtWTThickness.Text), Convert.ToDecimal(txtWtLeadShots.Text), Convert.ToDecimal(txtWtB.Text), Convert.ToDecimal(txtWtN.Text), Convert.ToDecimal(txtWtTransverse.Text), Convert.ToDecimal(txtWetTr.Text), txtAvgWetTr.Text, false);
                    }
                }
                #endregion

                #region save MR data
                if (TabMR.Visible == true)
                {
                    dc.TileMOR_Update(0, txtRefNo.Text, "", "", 0, 0, 0, 0, 0, 0, 0, "", true);
                    for (int i = 0; i < grdMR.Rows.Count; i++)
                    {
                        TextBox txtMRIdMark = (TextBox)grdMR.Rows[i].FindControl("txtMRIdMark");
                        TextBox txtT1 = (TextBox)grdMR.Rows[i].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdMR.Rows[i].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdMR.Rows[i].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdMR.Rows[i].FindControl("txtT4");
                        TextBox txtT5 = (TextBox)grdMR.Rows[i].FindControl("txtT5");
                        TextBox txtT6 = (TextBox)grdMR.Rows[i].FindControl("txtT6");
                        TextBox txtMRLeadShots = (TextBox)grdMR.Rows[i].FindControl("txtMRLeadShots");


                        TextBox txtMRCalIdMark = (TextBox)grdMRCal.Rows[i].FindControl("txtMRCalIdMark");
                        TextBox txtMRThickness = (TextBox)grdMRCal.Rows[i].FindControl("txtMRThickness");
                        TextBox txtMRCalLeadShots = (TextBox)grdMRCal.Rows[i].FindControl("txtMRCalLeadShots");
                        TextBox txtMRB = (TextBox)grdMRCal.Rows[i].FindControl("txtMRB");
                        TextBox txtMRN = (TextBox)grdMRCal.Rows[i].FindControl("txtMRN");
                        TextBox txtAvgBreakLoad = (TextBox)grdMRCal.Rows[i].FindControl("txtAvgBreakLoad");
                        TextBox txtBreakStrength = (TextBox)grdMRCal.Rows[i].FindControl("txtBreakStrength");
                        TextBox txtAvgBreakStrength = (TextBox)grdMRCal.Rows[i].FindControl("txtAvgBreakStrength");
                        TextBox txtMRTransverse = (TextBox)grdMRCal.Rows[i].FindControl("txtMRTransverse");
                        TextBox txtMOR = (TextBox)grdMRCal.Rows[i].FindControl("txtMOR");
                        TextBox txtAvgMOR = (TextBox)grdMRCal.Rows[i].FindControl("txtAvgMOR");

                        dc.TileMOR_Update(i + 1, txtRefNo.Text, txtMRCalIdMark.Text, (txtT1.Text + "|" + txtT2.Text + "|" + txtT3.Text + "|" + txtT4.Text + "|" + txtT5.Text + "|" + txtT6.Text), Convert.ToDecimal(txtMRThickness.Text), Convert.ToDecimal(txtMRLeadShots.Text), Convert.ToDecimal(txtMRB.Text), Convert.ToDecimal(txtMRN.Text), Convert.ToDecimal(txtBreakStrength.Text), Convert.ToDecimal(txtMRTransverse.Text), Convert.ToDecimal(txtMOR.Text), (txtAvgBreakLoad.Text + "|" + txtAvgBreakStrength.Text + "|" + txtAvgMOR.Text), false);
                    }
                }
                #endregion

                #region save DA data
                if (TabDA.Visible == true)
                {
                    string strAvg = "";
                    dc.TileDA_Update(0, txtRefNo.Text, "", "", "", "", 0, 0, 0, "", true);
                    for (int i = 0; i < grdDA.Rows.Count; i++)
                    {
                        TextBox txtDAIdMark = (TextBox)grdDA.Rows[i].FindControl("txtDAIdMark");
                        TextBox txtL1 = (TextBox)grdDA.Rows[i].FindControl("txtL1");
                        TextBox txtL2 = (TextBox)grdDA.Rows[i].FindControl("txtL2");
                        TextBox txtL3 = (TextBox)grdDA.Rows[i].FindControl("txtL3");
                        TextBox txtW1 = (TextBox)grdDA.Rows[i].FindControl("txtW1");
                        TextBox txtW2 = (TextBox)grdDA.Rows[i].FindControl("txtW2");
                        TextBox txtW3 = (TextBox)grdDA.Rows[i].FindControl("txtW3");
                        TextBox txtT1 = (TextBox)grdDA.Rows[i].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdDA.Rows[i].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdDA.Rows[i].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdDA.Rows[i].FindControl("txtT4");
                        //TextBox txtT5 = (TextBox)grdDA.Rows[i].FindControl("txtT5");
                        //TextBox txtT6 = (TextBox)grdDA.Rows[i].FindControl("txtT6");
                        //TextBox txtT7 = (TextBox)grdDA.Rows[i].FindControl("txtT7");
                        //TextBox txtT8 = (TextBox)grdDA.Rows[i].FindControl("txtT8");
                        //TextBox txtT9 = (TextBox)grdDA.Rows[i].FindControl("txtT9");
                        //TextBox txtT10 = (TextBox)grdDA.Rows[i].FindControl("txtT10");
                        //TextBox txtT11 = (TextBox)grdDA.Rows[i].FindControl("txtT11");
                        //TextBox txtT12 = (TextBox)grdDA.Rows[i].FindControl("txtT12");

                        TextBox txtDACalIdMark = (TextBox)grdDACal.Rows[i].FindControl("txtDACalIdMark");
                        TextBox txtLength = (TextBox)grdDACal.Rows[i].FindControl("txtLength");
                        TextBox txtWidth = (TextBox)grdDACal.Rows[i].FindControl("txtWidth");
                        TextBox txtThickness = (TextBox)grdDACal.Rows[i].FindControl("txtThickness");
                        TextBox txtAvgLen = (TextBox)grdDACal.Rows[i].FindControl("txtAvgLen");
                        TextBox txtAvgWidth = (TextBox)grdDACal.Rows[i].FindControl("txtAvgWidth");
                        TextBox txtAvgThickness = (TextBox)grdDACal.Rows[i].FindControl("txtAvgThickness");

                        strAvg = "";
                        if (txtAvgLen.Text != "")
                        {
                            strAvg = txtAvgLen.Text + "|" + txtAvgWidth.Text + "|" + txtAvgThickness.Text;
                        }
                        //dc.TileDA_Update(i + 1, txtRefNo.Text, txtDACalIdMark.Text, (txtL1.Text + "|" + txtL2.Text + "|" + txtL3.Text), (txtW1.Text + "|" + txtW2.Text + "|" + txtW3.Text), (txtT1.Text + "|" + txtT2.Text + "|" + txtT3.Text + "|" + txtT4.Text + "|" + txtT5.Text + "|" + txtT6.Text + "|" + txtT7.Text + "|" + txtT8.Text + "|" + txtT9.Text + "|" + txtT10.Text + "|" + txtT11.Text + "|" + txtT12.Text), Convert.ToDecimal(txtLength.Text), Convert.ToDecimal(txtWidth.Text), Convert.ToDecimal(txtThickness.Text), strAvg, false);
                        dc.TileDA_Update(i + 1, txtRefNo.Text, txtDACalIdMark.Text, (txtL1.Text + "|" + txtL2.Text + "|" + txtL3.Text), (txtW1.Text + "|" + txtW2.Text + "|" + txtW3.Text), (txtT1.Text + "|" + txtT2.Text + "|" + txtT3.Text + "|" + txtT4.Text), Convert.ToDecimal(txtLength.Text), Convert.ToDecimal(txtWidth.Text), Convert.ToDecimal(txtThickness.Text), strAvg, false);
                    }
                }
                #endregion

                #region Remark Update
                dc.TileRemarkDetail_Update(0, txtRefNo.Text, true);
                string remId = "";
                for (int i = 0; i <= grdRemark.Rows.Count - 1; i++)
                {
                    TextBox txtRemark = (TextBox)grdRemark.Rows[i].FindControl("txtRemark");
                    if (txtRemark.Text != "")
                    {
                        dc.TileRemark_View(txtRemark.Text, ref remId);
                        if (remId == "" || remId == null)
                        {
                            remId = dc.TileRemark_Update(txtRemark.Text).ToString();
                        }
                        dc.TileRemarkDetail_Update(Convert.ToInt32(remId), txtRefNo.Text, false);
                    }
                }
                #endregion

                #region Specified Limit Update
                if (TabDA.Visible == true)
                {
                    dc.TileSpecLimitsDA_Update(txtRefNo.Text, "", "", "", "", true, true);
                    if (ChkSpecifiedLimits.Checked == true)
                    {
                        dc.TileSpecLimitsDA_Update("", "", "", "", "", true, true);
                    }
                    for (int i = 0; i <= grdSpecifiedLimit.Rows.Count - 1; i++)
                    {
                        TextBox txtSpeciDescription = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciDescription");
                        TextBox txtSpeciLength = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciLength");
                        TextBox txtSpeciWidth = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciWidth");
                        TextBox txtSpeciThickness = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciThickness");

                        if (ChkSpecifiedLimits.Checked == true)
                        {
                            dc.TileSpecLimitsDA_Update(txtRefNo.Text, txtSpeciDescription.Text, txtSpeciLength.Text, txtSpeciWidth.Text, txtSpeciThickness.Text, false, false);
                            dc.TileSpecLimitsDA_Update("", txtSpeciDescription.Text, txtSpeciLength.Text, txtSpeciWidth.Text, txtSpeciThickness.Text, true, false);
                        }
                        else
                        {
                            dc.TileSpecLimitsDA_Update(txtRefNo.Text, txtSpeciDescription.Text, txtSpeciLength.Text, txtSpeciWidth.Text, txtSpeciThickness.Text, false, false);
                        }
                    }
                }
                if (TabWA.Visible == true)
                {
                    dc.TileSpecLimitsWA_Update(txtRefNo.Text, "", true, true);
                    if (ChkSpecifiedLimits.Checked == true)
                    {
                        dc.TileSpecLimitsWA_Update("", "", true, true);
                    }
                    for (int i = 0; i <= grdSpecifiedLimit.Rows.Count - 1; i++)
                    {
                        TextBox txtSpeciDescription = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciDescription");
                        TextBox txtSpeciLength = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciLength");
                        TextBox txtSpeciWidth = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciWidth");
                        TextBox txtSpeciThickness = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciThickness");

                        if (ChkSpecifiedLimits.Checked == true)
                        {
                            dc.TileSpecLimitsWA_Update(txtRefNo.Text, txtSpeciDescription.Text, false, false);
                            dc.TileSpecLimitsWA_Update("", txtSpeciDescription.Text, true, false);
                        }
                        else
                        {
                            dc.TileSpecLimitsWA_Update(txtRefNo.Text, txtSpeciDescription.Text, false, false);
                        }
                    }
                }
                if (TabMR.Visible == true)
                {
                    dc.TileSpecLimitsMOR_Update(txtRefNo.Text, "", "", true, true);
                    if (ChkSpecifiedLimits.Checked == true)
                    {
                        dc.TileSpecLimitsMOR_Update("", "", "", true, true);
                    }
                    for (int i = 0; i <= grdSpecifiedLimit.Rows.Count - 1; i++)
                    {
                        TextBox txtSpeciDescription = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciDescription");
                        TextBox txtSpeciLimits = (TextBox)grdSpecifiedLimit.Rows[i].FindControl("txtSpeciLimits");

                        if (ChkSpecifiedLimits.Checked == true)
                        {
                            dc.TileSpecLimitsMOR_Update(txtRefNo.Text, txtSpeciDescription.Text, txtSpeciLimits.Text, false, false);
                            dc.TileSpecLimitsMOR_Update("", txtSpeciDescription.Text, txtSpeciLimits.Text, true, false);
                        }
                        else
                        {
                            dc.TileSpecLimitsMOR_Update(txtRefNo.Text, txtSpeciDescription.Text, txtSpeciLimits.Text, false, false);
                        }
                    }
                }
                #endregion

                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Report Saved Successfully');", true);

                lnkPrint.Visible = true;
                lnkSave.Enabled = false;
                lnkCalculate.Enabled = false;
            }
        }
        protected void lnkExit_Click(object sender, EventArgs e)
        {
            //object refUrl = ViewState["RefUrl"];
            //if (refUrl != null)
            //{
            //    Response.Redirect((string)refUrl);
            //}
            Response.Redirect("ReportStatus.aspx");
        }

        #endregion

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            PrintPDFReport rpt = new PrintPDFReport();
            //rpt.Tile_PDFReport(txtRefNo.Text, lblStatus.Text);
            rpt.PrintSelectedReport(txt_Rectype.Text, txtRefNo.Text, lblStatus.Text, "", "", "", "", "", "", "");
        }

        protected void lnkGetAppData_Click(object sender, EventArgs e)
        {
            clsData objcls = new clsData();
            string mySql = "";
            DataTable dt;
            //txtRefNo.Text = "5005/1-1";
            if (TabCR.Visible == true)
            {
                #region CR
                mySql = @"select ref.reference_number ,chead.*,cr.* " +
                        " from new_gt_app_db.dbo.reference_number ref ,new_gt_app_db.dbo.category_header chead,"+
                        " new_gt_app_db.dbo.tile_crazing cr "+
                        " where ref.reference_number = '" + txtRefNo.Text + "'" +
                        " and ref.pk_id=chead.reference_no and chead.pk_id=category_header_fk_id ";

                dt = objcls.getGeneralData(mySql);
                if (dt.Rows.Count > 0)
                {
                    txtDateOfTest.Text = Convert.ToDateTime(dt.Rows[0]["date_of_testing"]).ToString("dd/MM/yyyy");
                    ddlTileType.SelectedItem.Text = dt.Rows[0]["tile_type"].ToString();
                    if (dt.Rows[0]["witness_by"] != null || dt.Rows[0]["witness_by"].ToString() != "")
                    {
                        txtWitnesBy.Text = dt.Rows[0]["witness_by"].ToString();
                        chkWitnessBy.Checked = true;
                        txtWitnesBy.Visible = true;
                    }
                    txtQuantity.Text = dt.Rows.Count.ToString();
                    CRRowsChanged();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox txtIdMark = (TextBox)grdCR.Rows[i].FindControl("txtIdMark");
                        DropDownList ddlCrazing = (DropDownList)grdCR.Rows[i].FindControl("ddlCrazing");

                        txtIdMark.Text = dt.Rows[i]["Id_mark"].ToString();

                        if (dt.Rows[i]["crazing"].ToString() == "Observed")
                            ddlCrazing.SelectedIndex = 1;
                        else if (dt.Rows[i]["crazing"].ToString() == "Not Observed")
                            ddlCrazing.SelectedIndex = 2;

                    }
                }
                dt.Dispose();
                #endregion 
            }
            else if (TabSH.Visible == true)
            {
                #region Scrach

                
                mySql = @"select ref.reference_number ,chead.*,sh.id_mark as idm, sh.scratch_hardness   " +
                        "from new_gt_app_db.dbo.reference_number ref ,new_gt_app_db.dbo.category_header chead," +
                        " new_gt_app_db.dbo.tile_scratch_hardness sh" +
                        " where ref.reference_number ='" + txtRefNo.Text + "'" +
                        " and ref.pk_id=chead.reference_no and chead.pk_id=category_header_fk_id ";
                dt = objcls.getGeneralData(mySql);
                if (dt.Rows.Count > 0)
                {
                    txtDateOfTest.Text = Convert.ToDateTime(dt.Rows[0]["date_of_testing"]).ToString("dd/MM/yyyy");
                    ddlTileType.SelectedItem.Text = dt.Rows[0]["tile_type"].ToString();
                    if (dt.Rows[0]["witness_by"] != null || dt.Rows[0]["witness_by"].ToString() != "")
                    {
                        txtWitnesBy.Text = dt.Rows[0]["witness_by"].ToString();
                        chkWitnessBy.Checked = true;
                        txtWitnesBy.Visible = true;
                    }
                    txtSHQuantity.Text = dt.Rows.Count.ToString();
                    SHRowsChanged();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        TextBox txtSHIdMark = (TextBox)grdSH.Rows[i].FindControl("txtSHIdMark");
                        TextBox txtHardness = (TextBox)grdSH.Rows[i].FindControl("txtHardness");

                        txtSHIdMark.Text = dt.Rows[i]["idm"].ToString();
                        txtHardness.Text = dt.Rows[i]["scratch_hardness"].ToString();                      

                    }
                }
                dt.Dispose();
                #endregion
            }
            else if (TabWA.Visible == true)
            {
                #region Water


                mySql = @"select ref.reference_number ,chead.*,wa.id_mark  as idm, wa.dry_weight,wa.wet_weight   " +
                        "from new_gt_app_db.dbo.reference_number ref ,new_gt_app_db.dbo.category_header chead," +
                        " new_gt_app_db.dbo.tile_water_absorption  wa " +
                        " where ref.reference_number ='" + txtRefNo.Text + "'" +
                        " and ref.pk_id=chead.reference_no and chead.pk_id=category_header_fk_id ";
                dt = objcls.getGeneralData(mySql);
                if (dt.Rows.Count > 0)
                {
                    txtDateOfTest.Text = Convert.ToDateTime(dt.Rows[0]["date_of_testing"]).ToString("dd/MM/yyyy");
                    ddlTileType.SelectedItem.Text = dt.Rows[0]["tile_type"].ToString();
                    if (dt.Rows[0]["witness_by"] != null || dt.Rows[0]["witness_by"].ToString() != "")
                    {
                        txtWitnesBy.Text = dt.Rows[0]["witness_by"].ToString();
                        chkWitnessBy.Checked = true;
                        txtWitnesBy.Visible = true;
                    }
                    txtWAQuantity.Text = dt.Rows.Count.ToString();
                    WARowsChanged();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        TextBox txtWAIdMark = (TextBox)grdWA.Rows[i].FindControl("txtWAIdMark");
                        TextBox txtWADryWt = (TextBox)grdWA.Rows[i].FindControl("txtWADryWt");
                        TextBox txtWAWetWt = (TextBox)grdWA.Rows[i].FindControl("txtWAWetWt");

                        txtWAIdMark.Text = dt.Rows[i]["idm"].ToString();
                        txtWADryWt.Text = dt.Rows[i]["dry_weight"].ToString();
                        txtWAWetWt.Text = dt.Rows[i]["wet_weight"].ToString();

                    }
                }
                dt.Dispose();
                #endregion
            }
            else if (TabDA.Visible == true)
            {
                #region Dimension


                mySql = @"select ref.reference_number ,chead.*,da.id_mark as idm, da.l1,da.l2,da.l3,da.w1,da.w2,da.w3,da.t1,da.t2,da.t3,da.t4 "+  
                        "from new_gt_app_db.dbo.reference_number ref ,new_gt_app_db.dbo.category_header chead," +
                        " new_gt_app_db.dbo.tile_diamension   da  " +
                        " where ref.reference_number ='" + txtRefNo.Text + "'" +
                        " and ref.pk_id=chead.reference_no and chead.pk_id=category_header_fk_id ";
                dt = objcls.getGeneralData(mySql);
                if (dt.Rows.Count > 0)
                {
                    txtDateOfTest.Text = Convert.ToDateTime(dt.Rows[0]["date_of_testing"]).ToString("dd/MM/yyyy");
                    ddlTileType.SelectedItem.Text = dt.Rows[0]["tile_type"].ToString();
                    if (dt.Rows[0]["witness_by"] != null || dt.Rows[0]["witness_by"].ToString() != "")
                    {
                        txtWitnesBy.Text = dt.Rows[0]["witness_by"].ToString();
                        chkWitnessBy.Checked = true;
                        txtWitnesBy.Visible = true;
                    }
                    txtDAQuantity.Text = dt.Rows.Count.ToString();
                    DARowsChanged();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        TextBox txtDAIdMark = (TextBox)grdDA.Rows[i].FindControl("txtDAIdMark");
                        TextBox txtL1 = (TextBox)grdDA.Rows[i].FindControl("txtL1");
                        TextBox txtL2 = (TextBox)grdDA.Rows[i].FindControl("txtL2");
                        TextBox txtL3 = (TextBox)grdDA.Rows[i].FindControl("txtL3");
                        TextBox txtW1 = (TextBox)grdDA.Rows[i].FindControl("txtW1");
                        TextBox txtW2 = (TextBox)grdDA.Rows[i].FindControl("txtW2");
                        TextBox txtW3 = (TextBox)grdDA.Rows[i].FindControl("txtW3");
                        TextBox txtT1 = (TextBox)grdDA.Rows[i].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdDA.Rows[i].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdDA.Rows[i].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdDA.Rows[i].FindControl("txtT4");

                        txtDAIdMark.Text = dt.Rows[i]["idm"].ToString();
                        txtL1.Text = dt.Rows[i]["L1"].ToString();
                        txtL2.Text = dt.Rows[i]["L2"].ToString();
                        txtL3.Text = dt.Rows[i]["L3"].ToString();
                        txtW1.Text = dt.Rows[i]["W1"].ToString();
                        txtW2.Text = dt.Rows[i]["W2"].ToString();
                        txtW3.Text = dt.Rows[i]["W3"].ToString();
                        txtT1.Text = dt.Rows[i]["T1"].ToString();
                        txtT2.Text = dt.Rows[i]["T2"].ToString();
                        txtT3.Text = dt.Rows[i]["T3"].ToString();
                        txtT4.Text = dt.Rows[i]["T4"].ToString();

                    }
                }
                dt.Dispose();
                #endregion
            }
            else if (TabMR.Visible==true  )
            {
                #region MOR


                mySql = @"select ref.reference_number ,chead.*,mor.id_mark as idm,t1,t2,t3,t4,t5,t6,wt_of_lead_shots " +
                        "from new_gt_app_db.dbo.reference_number ref ,new_gt_app_db.dbo.category_header chead," +
                        " new_gt_app_db.dbo.tile_modulus_of_rupture  mor " +
                        " where ref.reference_number ='" + txtRefNo.Text + "'" +
                        " and ref.pk_id=chead.reference_no and chead.pk_id=category_header_fk_id ";
                dt = objcls.getGeneralData(mySql);
                if (dt.Rows.Count > 0)
                {
                    txtDateOfTest.Text = Convert.ToDateTime(dt.Rows[0]["date_of_testing"]).ToString("dd/MM/yyyy");
                    ddlTileType.SelectedItem.Text = dt.Rows[0]["tile_type"].ToString();
                    if (dt.Rows[0]["witness_by"] != null || dt.Rows[0]["witness_by"].ToString() != "")
                    {
                        txtWitnesBy.Text = dt.Rows[0]["witness_by"].ToString();
                        chkWitnessBy.Checked = true;
                        txtWitnesBy.Visible = true;
                    }

                    txtMRQuantity.Text = dt.Rows.Count.ToString();
                    //TextBox txtDiameterOfRod = (TextBox)grdMRCal.Rows[0].FindControl("txtDiameterOfRod");
                    //TextBox txtThicknessOfRubber = (TextBox)grdMRCal.Rows[0].FindControl("txtThicknessOfRubber");
                    //TextBox txtOverlapBeyondEdge = (TextBox)grdMRCal.Rows[0].FindControl("txtOverlapBeyondEdge");
                    //TextBox txtMORL = (TextBox)grdMRCal.Rows[0].FindControl("txtMORL");
                    //TextBox txtMORB = (TextBox)grdMRCal.Rows[0].FindControl("txtMORB");

                    txtDiameterOfRod.Text = dt.Rows[0]["rod_diameter"].ToString();
                    txtThicknessOfRubber.Text = dt.Rows[0]["rubber_thickness"].ToString();
                    txtOverlapBeyondEdge.Text = dt.Rows[0]["overlap_edge"].ToString();
                    txtMORL.Text = dt.Rows[0]["length"].ToString();
                    txtMORB.Text = dt.Rows[0]["breadth"].ToString();
                    MRRowsChanged();
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox txtMRIdMark = (TextBox)grdMR.Rows[i].FindControl("txtMRIdMark");
                        TextBox txtT1 = (TextBox)grdMR.Rows[i].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdMR.Rows[i].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdMR.Rows[i].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdMR.Rows[i].FindControl("txtT4");
                        TextBox txtT5 = (TextBox)grdMR.Rows[i].FindControl("txtT5");
                        TextBox txtT6 = (TextBox)grdMR.Rows[i].FindControl("txtT6");
                        TextBox txtMRLeadShots = (TextBox)grdMR.Rows[i].FindControl("txtMRLeadShots");


                        txtMRIdMark.Text = dt.Rows[i]["idm"].ToString();
                        txtT1.Text = dt.Rows[i]["T1"].ToString();
                        txtT2.Text = dt.Rows[i]["T2"].ToString();
                        txtT3.Text = dt.Rows[i]["T3"].ToString();
                        txtT4.Text = dt.Rows[i]["T4"].ToString();
                        txtT5.Text = dt.Rows[i]["T5"].ToString();
                        txtT6.Text = dt.Rows[i]["T6"].ToString();
                        txtMRLeadShots.Text = dt.Rows[i]["wt_of_lead_shots"].ToString();


                        TextBox txtMRCalIdMark = (TextBox)grdMRCal.Rows[i].FindControl("txtMRCalIdMark");
                     //   TextBox txtMRThickness = (TextBox)grdMRCal.Rows[i].FindControl("txtMRThickness");
                     //   TextBox txtMRCalLeadShots = (TextBox)grdMRCal.Rows[i].FindControl("txtMRCalLeadShots");
                       
                        txtMRCalIdMark.Text = dt.Rows[i]["idm"].ToString();
                        //txtMRThickness.Text = dt.Rows[i]["T1"].ToString();
                        //txtMRCalLeadShots.Text = dt.Rows[i]["wt_of_lead_shots"].ToString();

                    }
                }
                dt.Dispose();
                #endregion
            }
            else if (TabWT.Visible == true)
            {
                #region WT
                mySql = @"select ref.reference_number ,chead.*,wt.id_mark as idm,t1,t2,t3,t4,t5,t6,wt_of_lead_shots " +
                        "from new_gt_app_db.dbo.reference_number ref ,new_gt_app_db.dbo.category_header chead," +
                        " new_gt_app_db.dbo.tile_wet_transverse wt " +
                        " where ref.reference_number ='" + txtRefNo.Text + "'" +
                        " and ref.pk_id=chead.reference_no and chead.pk_id=category_header_fk_id ";
                dt = objcls.getGeneralData(mySql);
                if (dt.Rows.Count > 0)
                {
                    txtDateOfTest.Text= Convert.ToDateTime(dt.Rows[0]["date_of_testing"]).ToString("dd/MM/yyyy");
                    ddlTileType.SelectedItem.Text= dt.Rows[0]["tile_type"].ToString();
                    if (dt.Rows[0]["witness_by"] != null || dt.Rows[0]["witness_by"].ToString() !="" )
                    {
                        txtWitnesBy.Text = dt.Rows[0]["witness_by"].ToString();
                        chkWitnessBy.Checked = true;
                        txtWitnesBy.Visible = true;
                    }

                    txtWTQuantity.Text = dt.Rows.Count.ToString();
                    txtL.Text = dt.Rows[0]["Length"].ToString();
                    txtB.Text = dt.Rows[0]["breadth"].ToString();
                    WTRowsChanged();                                        
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        TextBox txtWTIdMark = (TextBox)grdWT.Rows[i].FindControl("txtWTIdMark");
                        TextBox txtT1 = (TextBox)grdWT.Rows[i].FindControl("txtT1");
                        TextBox txtT2 = (TextBox)grdWT.Rows[i].FindControl("txtT2");
                        TextBox txtT3 = (TextBox)grdWT.Rows[i].FindControl("txtT3");
                        TextBox txtT4 = (TextBox)grdWT.Rows[i].FindControl("txtT4");
                        TextBox txtT5 = (TextBox)grdWT.Rows[i].FindControl("txtT5");
                        TextBox txtT6 = (TextBox)grdWT.Rows[i].FindControl("txtT6");
                        TextBox txtWtLeadShots = (TextBox)grdWT.Rows[i].FindControl("txtWtLeadShots");


                        txtWTIdMark.Text = dt.Rows[i]["idm"].ToString();
                        txtT1.Text = dt.Rows[i]["T1"].ToString();
                        txtT2.Text = dt.Rows[i]["T2"].ToString();
                        txtT3.Text = dt.Rows[i]["T3"].ToString();
                        txtT4.Text = dt.Rows[i]["T4"].ToString();
                        txtT5.Text = dt.Rows[i]["T5"].ToString();
                        txtT6.Text = dt.Rows[i]["T6"].ToString();
                        txtWtLeadShots.Text = dt.Rows[i]["wt_of_lead_shots"].ToString();

                        TextBox txtWTCalIdMark = (TextBox)grdWTCal.Rows[i].FindControl("txtWTCalIdMark");
                        TextBox txtWTThickness = (TextBox)grdWTCal.Rows[i].FindControl("txtWTThickness");
                        TextBox txtWtCalLeadShots = (TextBox)grdWTCal.Rows[i].FindControl("txtWtCalLeadShots");

                        txtWTCalIdMark.Text = dt.Rows[i]["idm"].ToString();
                        txtWTThickness.Text = dt.Rows[i]["T1"].ToString();
                        txtWtCalLeadShots.Text = dt.Rows[i]["wt_of_lead_shots"].ToString();
                    }
                }

                dt.Dispose();
                #endregion
            }

            objcls = null;
        }



    }

}