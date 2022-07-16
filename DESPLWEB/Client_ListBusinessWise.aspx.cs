using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class Client_ListBusinessWise : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "List of Client - Lost";
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Visible = false;
                txt_Fromdate.Text = DateTime.Today.AddDays(-21).ToString("dd/MM/yyyy");
                txt_ToDate.Text = DateTime.Today.AddDays(-14).ToString("dd/MM/yyyy");

                txt_FromdateND.Text = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
                txt_ToDateND.Text = DateTime.Today.ToString("dd/MM/yyyy");
                LoadRoutename();
                //bool userRight = false;
                if (Session["LoginId"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Showalert", "alert('Your Session has been Expired !' +'\\n'+ 'Login Again !');", true);
                    Response.Redirect("Login.aspx");
                }
                //else
                //{
                //    var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                //    foreach (var u in user)
                //    {
                //        if (u.USER_Admin_right_bit == true)
                //        {
                //            userRight = true;
                //        }
                //    }
                //}
                //if (userRight == false)
                //{
                //    pnlContent.Visible = false;
                //    lblAccess.Visible = true;
                //    lblAccess.Text = "Access is Denied.. ";
                //}
            }
        }
        private void LoadRoutename()
        {
            var routeList = dc.Route_View(0, "", "All", 0);
            ddlRouteName.DataTextField = "Route_Name_var";
            ddlRouteName.DataValueField = "Route_Id";
            ddlRouteName.DataSource = routeList;
            ddlRouteName.DataBind();
            ddlRouteName.Items.Insert(0, "-------------Select-------------");
            
        }
        protected void imgClosePopup_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
                
        protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            string[] strDate = txt_Fromdate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            strDate = txt_ToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

            strDate = txt_FromdateND.Text.Split('/');
            DateTime FromDateND = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            strDate = txt_ToDateND.Text.Split('/');
            DateTime ToDateND = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

            int routeId = 0;
            if (ddlRouteName.SelectedIndex > 0)
                routeId = Convert.ToInt32(ddlRouteName.SelectedValue);

            bool notDoneFlag = true;
            int perFrom = 0, perTo = 0;
            if (txtBusinessDropFrom.Text != "" && txtBusinessDropTo.Text != ""  && txtBusinessDropTo.Text != "0")
            {
                notDoneFlag = false;
                perFrom =  Convert.ToInt32(txtBusinessDropFrom.Text);
                perTo =  Convert.ToInt32(txtBusinessDropTo.Text);
                if (perTo > 100)
                    perTo = 100;
                if (perFrom > perTo)
                {
                    perFrom = perTo;
                }
                if (perFrom == 100 && perTo == 100)
                {
                    txtBusinessDropFrom.Text = "";
                    txtBusinessDropTo.Text = "";
                    notDoneFlag = true;
                }
                
            }
            DataTable dt = new DataTable();
            DataRow dr1 = null;

            dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt.Columns.Add(new DataColumn("SiteName", typeof(string)));
            dt.Columns.Add(new DataColumn("Address", typeof(string)));
            dt.Columns.Add(new DataColumn("InchargeName", typeof(string)));
            dt.Columns.Add(new DataColumn("InchargeEmailId", typeof(string)));
            dt.Columns.Add(new DataColumn("OfficeTelNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ClientEmailId", typeof(string)));
            dt.Columns.Add(new DataColumn("BusinessPrevious", typeof(string)));
            dt.Columns.Add(new DataColumn("BusinessCurrent", typeof(string)));
            dt.Columns.Add(new DataColumn("BusinessDropbyPercent", typeof(string)));
            int clId=0;
            var client = dc.Client_View_BusinessWise(FromDate, ToDate, FromDateND, ToDateND, notDoneFlag, perFrom, perTo).ToList();
            if (client.Count > 0)
            {              
                foreach (var item in client)
                {
                    dr1 = dt.NewRow();
                    dr1["ClientName"] = Convert.ToString(item.CL_Name_var);
                    dr1["Address"] = Convert.ToString(Regex.Replace(item.CL_OfficeAddress_var, @"\t|\n|\r", ""));
                    if (Convert.ToString(item.CL_DirectorName_var) != null)
                        dr1["InchargeName"] = Convert.ToString(Regex.Replace(item.CL_DirectorName_var, @"\t|\n|\r", ""));
                    else
                        dr1["InchargeName"] = Convert.ToString(item.CL_DirectorName_var);
                    if (Convert.ToString(item.CL_DirectorEmailID_var) != null)
                        dr1["InchargeEmailId"] = Convert.ToString(Regex.Replace(item.CL_DirectorEmailID_var, @"\t|\n|\r", ""));
                    else
                        dr1["InchargeEmailId"] = Convert.ToString(item.CL_DirectorEmailID_var);

                    if (Convert.ToString(item.CL_OfficeTelNo_var) != null)
                        dr1["OfficeTelNo"] = Convert.ToString(Regex.Replace(item.CL_OfficeTelNo_var, @"\t|\n|\r|\s", ""));
                    else
                        dr1["OfficeTelNo"] = Convert.ToString(item.CL_OfficeTelNo_var);

                    if (Convert.ToString(item.CL_EmailID_var)!= null)
                      dr1["ClientEmailId"] = Convert.ToString(Regex.Replace(item.CL_EmailID_var, @"\t|\n|\r", ""));
                    else
                       dr1["ClientEmailId"] = Convert.ToString(item.CL_EmailID_var);
                    dr1["BusinessPrevious"] = Convert.ToString(item.businessAmtPrev);
                    dr1["BusinessCurrent"] = Convert.ToString(item.businessAmtCurrent);
                    dr1["BusinessDropbyPercent"] = Convert.ToString(item.dropByPercent);
                    dt.Rows.Add(dr1);
                    clId = Convert.ToInt32(item.CL_Id);
                    var reslt = dc.Site_View_BusinessWise(routeId, clId, 0).ToList();
                    if (reslt.Count > 0)
                    {
                        foreach (var itm in reslt)
                        {
                            dr1 = dt.NewRow();
                            dr1["SiteName"] = Convert.ToString(itm.SITE_Name_var);
                            dr1["Address"] = Convert.ToString(Regex.Replace(itm.SITE_Address_var, @"\t|\n|\r", ""));
                            if (Convert.ToString(itm.SITE_Incharge_var) != null)
                                dr1["InchargeName"] = Convert.ToString(Regex.Replace(itm.SITE_Incharge_var, @"\t|\n|\r", ""));
                            else
                                dr1["InchargeName"] = Convert.ToString(itm.SITE_Incharge_var);
                            if (Convert.ToString(itm.SITE_EmailID_var) != null)
                                dr1["InchargeEmailId"] = Convert.ToString(Regex.Replace(itm.SITE_EmailID_var, @"\t|\n|\r", ""));
                            else
                                dr1["InchargeEmailId"] = Convert.ToString(itm.SITE_EmailID_var);

                            if (Convert.ToString(itm.SITE_Phno_int) != null)
                                dr1["OfficeTelNo"] = Convert.ToString(Regex.Replace(itm.SITE_Phno_int, @"\t|\n|\r|\s", ""));
                            else
                                dr1["OfficeTelNo"] = Convert.ToString(itm.SITE_Phno_int);
                            dt.Rows.Add(dr1);
                        }
                    }
                    else   //delete dat client whos route id is not matching wid selected route id
                    {
                        dt.Rows[dt.Rows.Count-1].Delete();
                        dt.AcceptChanges();
                    }
                }
            }
            grdClientList.DataSource = dt;
            grdClientList.DataBind();
            //lblTotalRecords.Text = "Total No of Records : "+grdClientList.Rows.Count;


        }
       protected void lnkPrint_Click(object sender, EventArgs e)
        {
           if(grdClientList.Rows.Count>0)
                PrintGrid.PrintGridView_ClientListBusinessWise(grdClientList, "Client List", "Client_List");
        }

        protected void lnkDiscountAmt_Click(object sender, EventArgs e)
        {
            decimal totalBusi = 0, withDiscBusi = 0, withoutDiscBusi = 0, ActwithDiscBusi = 0, ActwithoutDiscBusi = 0;
            string[] strDate = txt_Fromdate.Text.Split('/');
            DateTime FromDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            strDate = txt_ToDate.Text.Split('/');
            DateTime ToDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));

            var business = dc.Bill_View_BusinessTemp(FromDate, ToDate, false);
            foreach (var bus in business)
            {
                totalBusi = Convert.ToDecimal(bus.totalBusi);
                ActwithDiscBusi = Convert.ToDecimal(bus.withDiscBusi);
                ActwithoutDiscBusi = Convert.ToDecimal(bus.withoutDiscBusi);

                withDiscBusi = Convert.ToDecimal(bus.withDiscBusi);
                var bill = dc.Bill_View_BusinessTemp1(FromDate, ToDate, true);
                foreach (var bl in bill)
                {
                    decimal billGross = getBillGrossAmt(bl.BILL_RecordType_var, Convert.ToInt32(bl.BILL_RecordNo_int));
                    if (billGross > bl.billDetailGross)
                    {
                        withDiscBusi += Convert.ToDecimal(bl.BILL_NetAmt_num);
                    }
                    else
                    {
                        withoutDiscBusi += Convert.ToDecimal(bl.BILL_NetAmt_num);
                    }
                }
            }
            lblDiscountAmt.Text = "Total=" + totalBusi.ToString() + " WithDisc=" + withDiscBusi.ToString() + " WithoutDisc=" + withoutDiscBusi.ToString() + " ActWith=" + ActwithDiscBusi.ToString() + " ActWithout=" + ActwithoutDiscBusi.ToString();
        }

        public decimal getBillGrossAmt(string RecordType, int RecordNo)
        {

            int i = 0, tmpRate = 0, tmpQty = 0, tmpAmt = 0, tmpSrNo = 0, noOfPits = 0, noOfCore = 0, otherChargesAmt = 0;
            decimal xGrossAmt = 0;
            string[,] arrTest = new string[50, 5];
            bool mfTestMatFlag = false;
            bool NDTByUPVFlag = false;
            bool MinBillFlag = false;
            int pitRate = 0, coreRate = 0;

            var inward = dc.Inward_Test_View(RecordType, RecordNo, "").ToList();
            foreach (var inwd in inward)
            {
                tmpQty = Convert.ToInt32(inwd.INWD_TotalQty_int);
                if (int.TryParse(inwd.INWD_Charges_var, out otherChargesAmt) == true)
                {
                    otherChargesAmt = Convert.ToInt32(inwd.INWD_Charges_var);
                }
                switch (RecordType)
                {
                    case "AAC":
                        {
                            #region AAC
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = Convert.ToInt32(inwd.AACINWD_Quantity_tint);
                            tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            break;
                            #endregion
                        }
                    case "AGGT":
                        {
                            #region aggt
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = 1;
                            tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            break;
                            #endregion
                        }
                    case "BT-":
                        {
                            #region bt-
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = 1;
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            if (tmpSrNo == 3) //Dimension Analysis
                                tmpQty = 1;
                            else if (tmpSrNo == 5) //Density
                                tmpQty = Convert.ToInt32(inwd.BTTEST_Quantity_tint);
                            else
                                tmpQty = 5;
                            tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            break;
                            #endregion
                        }
                    case "CCH":
                        {
                            #region cch
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = 1;
                            tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            break;
                            #endregion
                        }
                    case "CEMT":
                        {
                            #region cemt
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = 1;
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            break;
                            #endregion
                        }
                    case "CR":
                        {
                            #region cr
                            if (i == 0)
                            {
                                foreach (var CRinwd in inward)
                                {
                                    tmpRate = Convert.ToInt32(CRinwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(CRinwd.CRINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                }
                                var master = dc.MasterSetting_View(0);
                                foreach (var mst in master)
                                {
                                    if (inwd.TEST_Name_var == "Cutting & Testing")
                                    {
                                        if (tmpAmt <= mst.MinBillforCoreCutTest_int)
                                        {
                                            MinBillFlag = true;
                                            tmpAmt = Convert.ToInt32(mst.MinBillforCoreCutTest_int);
                                        }
                                    }
                                    else if (inwd.TEST_Name_var == "Only Core Testing")
                                    {
                                        if (tmpAmt <= mst.MinBillforCoreTest_int)
                                        {
                                            MinBillFlag = true;
                                            tmpAmt = Convert.ToInt32(mst.MinBillforCoreTest_int);
                                        }
                                    }

                                }
                                if (MinBillFlag == false)
                                {
                                    tmpAmt = 0;
                                }
                            }
                            if (MinBillFlag == false)
                            {
                                tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                tmpQty = Convert.ToInt32(inwd.CRINWD_Quantity_tint);
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            else if (i == 0)
                            {
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "CORECUT":
                        {
                            #region corecut
                            if (i == 0)
                            {
                                foreach (var CRCuTinwd in inward)
                                {
                                    tmpRate = Convert.ToInt32(CRCuTinwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(CRCuTinwd.CORECUTINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);

                                }
                                var master = dc.MasterSetting_View(0);
                                foreach (var mst in master)
                                {
                                    if (tmpAmt <= mst.MinBillforCoreCut_int)
                                    {
                                        MinBillFlag = true;
                                        tmpAmt = Convert.ToInt32(mst.MinBillforCoreCut_int);
                                    }
                                }
                                if (MinBillFlag == false)
                                {
                                    tmpAmt = 0;
                                }
                            }
                            if (MinBillFlag == false)
                            {
                                tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                tmpQty = Convert.ToInt32(inwd.CORECUTINWD_Quantity_tint);

                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            else if (i == 0)
                            {
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }

                    case "CT":
                        {
                            #region ct
                            if (i == 0)
                            {
                                tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                tmpQty = Convert.ToInt32(inwd.INWD_TotalQty_int);
                                tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);

                                if (inwd.CTINWD_TestType_var.Contains("Accelerated Curing") == true)
                                {
                                    tmpQty = 1;
                                }
                                else if (tmpQty < 3)
                                {
                                    tmpRate = 3 * tmpRate;
                                    tmpQty = 1;
                                }
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "FLYASH":
                        {
                            #region flyash
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = 1;
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            if (tmpSrNo != 3)
                            {
                                tmpQty = 1;
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            }
                            break;
                            #endregion
                        }
                    case "MF":
                        {
                            #region mf
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = 1;
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            if (inwd.MFINWD_TestMaterial_bit == true)
                            {
                                mfTestMatFlag = true;
                            }
                            tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            break;
                            #endregion

                        }
                    case "NDT":
                        {
                            #region ndt
                            if (i == 0)
                            {
                                foreach (var NDTinwd in inward)
                                {
                                    tmpRate = Convert.ToInt32(NDTinwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(NDTinwd.NDTTEST_Points_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    if (NDTinwd.NDTTEST_NDTBy_var == "UPV")
                                        NDTByUPVFlag = true;
                                }
                                var master = dc.MasterSetting_View(0);
                                foreach (var mst in master)
                                {
                                    if (NDTByUPVFlag == true)
                                    {
                                        if (tmpAmt <= mst.MinBillforNDTUPVandHamm_int)
                                        {
                                            MinBillFlag = true;
                                            tmpAmt = Convert.ToInt32(mst.MinBillforNDTUPVandHamm_int);
                                        }
                                    }
                                    else
                                    {
                                        if (tmpAmt <= mst.MinBillforNDTHamm_int)
                                        {
                                            MinBillFlag = true;
                                            tmpAmt = Convert.ToInt32(mst.MinBillforNDTHamm_int);
                                        }
                                    }
                                }
                                if (MinBillFlag == false)
                                {
                                    tmpAmt = 0;
                                }
                            }
                            if (MinBillFlag == false)
                            {
                                tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                tmpQty = Convert.ToInt32(inwd.NDTTEST_Points_tint);
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            else if (i == 0)
                            {
                                i = i + 1;
                            }

                            break;
                            #endregion
                        }
                    case "OT":
                        {
                            #region ot
                            tmpRate = Convert.ToInt32(inwd.OTRATEIN_Rate_int);
                            tmpQty = Convert.ToInt32(inwd.OTRATEIN_Quantity_tint);
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            break;
                            #endregion
                        }
                    case "PT":
                        {
                            #region pt
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = Convert.ToInt32(inwd.PTINWD_Quantity_tint);
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            break;
                            #endregion
                        }
                    case "PILE":
                        {
                            #region pile
                            if (i == 0)
                            {
                                foreach (var PILEinwd in inward)
                                {
                                    tmpRate = Convert.ToInt32(PILEinwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(PILEinwd.PILEINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                }
                                var master = dc.MasterSetting_View(0);
                                foreach (var mst in master)
                                {
                                    if (tmpAmt <= mst.MinBillforPILE_int)
                                    {
                                        MinBillFlag = true;
                                        tmpAmt = Convert.ToInt32(mst.MinBillforPILE_int);
                                    }
                                }
                                if (MinBillFlag == false)
                                {
                                    tmpAmt = 0;
                                }
                            }
                            if (MinBillFlag == false)
                            {
                                tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                tmpQty = Convert.ToInt32(inwd.PILEINWD_Quantity_tint);
                                tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);

                            }
                            else if (i == 0)
                            {
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "GT":
                    case "RWH":
                        {
                            #region gt,rwh
                            tmpRate = Convert.ToInt32(inwd.GTTEST_Rate_int);
                            tmpQty = Convert.ToInt32(inwd.GTTEST_Quantity_tint);
                            if (inwd.GTINW_LumpSump_tint == 1)
                            {
                                tmpAmt = tmpAmt + tmpRate;
                            }
                            else
                            {
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);

                            }
                            break;
                            #endregion
                        }
                    case "SO":
                        {
                            #region so
                            if (i == 0)
                            {
                                foreach (var SOinwd in inward)
                                {
                                    if (SOinwd.TEST_Sr_No == 10 || SOinwd.TEST_Sr_No == 11)
                                    {
                                        var master = dc.MasterSetting_View(0);
                                        foreach (var mst in master)
                                        {
                                            if (mst.MinBillforSO_int != null && mst.MinBillforSO_int > 0)
                                            {
                                                MinBillFlag = true;
                                                tmpRate = Convert.ToInt32(mst.MinBillforSO_int);
                                                tmpQty = 1;
                                                i = i + 1;
                                                break;
                                            }
                                        }
                                    }
                                    if (MinBillFlag == true)
                                    {
                                        break;
                                    }
                                }
                                if (MinBillFlag == false)
                                {
                                    tmpAmt = 0;
                                }
                            }
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = 1;
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            if (inwd.TEST_Sr_No == 10) //sand
                            {
                                tmpQty = Convert.ToInt32(inwd.SOINWD_Pits_tint);
                                noOfPits = noOfPits + tmpQty;
                                pitRate = tmpRate;
                            }
                            else if (inwd.TEST_Sr_No == 11) //core
                            {
                                tmpQty = Convert.ToInt32(inwd.SOINWD_Cores_tint);
                                noOfCore = noOfCore + tmpQty;
                                coreRate = tmpRate;
                            }
                            else if (inwd.TEST_Sr_No == 12)//classification
                            {
                                tmpQty = Convert.ToInt32(inwd.SOINWD_Quantity_tint);
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            }
                            else
                            {
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            }
                            break;
                            #endregion
                        }

                    case "SOLID":
                        {
                            #region solid
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = Convert.ToInt32(inwd.SOLIDINWD_Quantity_tint);
                            tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            break;
                            #endregion
                        }
                    case "ST":
                        {
                            #region st
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = Convert.ToInt32(inwd.STINWD_Quantity_tint);
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            break;
                            #endregion
                        }
                    case "STC":
                        {
                            #region stc
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = Convert.ToInt32(inwd.STCINWD_Quantity_tint);

                            tmpAmt = tmpAmt + (tmpRate * tmpQty);

                            break;
                            #endregion
                        }
                    case "TILE":
                        {
                            #region tile
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = Convert.ToInt32(inwd.TILEINWD_Quantity_tint);
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            break;
                            #endregion
                        }
                    case "WT":
                        {
                            #region wt
                            tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                            tmpQty = Convert.ToInt32(inwd.WTTEST_Qty_tint);
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            break;
                            #endregion
                        }
                }
            }
            //some code for steel and soil and mf
            if (RecordType == "SO")
            {
                if (noOfCore >= 6)
                {
                    noOfCore = noOfCore - 6;
                }
                else
                {
                    tmpQty = 6 - noOfCore;
                    if (tmpQty < noOfPits)
                    {
                        noOfPits = noOfPits - tmpQty;
                        noOfCore = 0;
                    }
                    else
                    {
                        noOfPits = 0;
                        noOfCore = 0;
                    }
                }
                tmpAmt = tmpAmt + (pitRate * Convert.ToInt32(noOfPits));
                tmpAmt = tmpAmt + (coreRate * Convert.ToInt32(noOfCore));

            }
            else if (RecordType == "MF")
            {
                if (mfTestMatFlag == true)
                {
                    var test = dc.Test(9, "", 0, "MF", "", 0);
                    foreach (var t in test)
                    {
                        tmpRate = Convert.ToInt32(t.TEST_Rate_int);
                        tmpQty = 1;
                        tmpAmt = tmpAmt + (tmpRate * tmpQty);

                    }
                }
            }
            //Other Charges
            if (otherChargesAmt > 0)
            {
                tmpRate = otherChargesAmt;
                tmpQty = 1;
                tmpAmt = tmpAmt + (tmpRate * tmpQty);

            }

            xGrossAmt = tmpAmt;

            return xGrossAmt;

        }

        protected void chkClientSpecific_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClientSpecific.Checked)
            {
                if (grdClientList.Rows.Count > 0)
                {
                    grdClientList.Columns[1].Visible = false;

                    for (int i = 0; i < grdClientList.Rows.Count; i++)
                    {
                        Label lblClientName = (Label)grdClientList.Rows[i].Cells[1].FindControl("lblClientName");

                        if (lblClientName.Text == "")
                        {
                            grdClientList.Rows[i].Visible = false;
                        }
                    }
                }
            }
            else
            {
                grdClientList.Columns[1].Visible = true;
                for (int i = 0; i < grdClientList.Rows.Count; i++)
                {
                    grdClientList.Rows[i].Visible = true;
                }
            }
            //lblTotalRecords.Text = "Total No of Records : " + grdClientList.Rows.Count;

        }
    }
}