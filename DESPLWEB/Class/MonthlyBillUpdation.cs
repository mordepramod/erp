using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace DESPLWEB
{
    public class MonthlyBillUpdation
    {
        LabDataDataContext dc = new LabDataDataContext();

        public string UpdateBill(int ClientId, int SiteId, DateTime Fromdate, DateTime Todate, string billingPeriod, string BillNo)
        {
            string ClientName = "", TallyNarration = "", tmpPart = "";
            int i = 0, iDetail = 0, tmpQty = 0, tmpSrNo = 0, noOfPits = 0, noOfCore = 0, otherChargesAmt = 0, tmpTestId = 0, miscTestId = 0;  //, otherChargesAmtDetail = 0, mfTestMatFlagCnt = 0;
            decimal tmpRate = 0, tmpAmt = 0, inwardAmt = 0;
            decimal xGrossAmt = 0, xNetAmt = 0, xDisc = 0, xDiscAmt = 0, xSrvTax = 0, xSrvTaxAmt = 0, xSwTax = 0, xSwTaxAmt = 0
                , xKkTax = 0, xKkTaxAmt = 0, edCess = 0, highEdCess = 0, roundOff = 0, xCgst = 0, xCgstAmt = 0, xSgst = 0, xSgstAmt = 0, xIgst = 0, xIgstAmt = 0;
            decimal ytmpAmt = 0;
            string[,] arrTest = new string[500, 10];
            string[,] arrTestDetail = new string[500, 13];
            bool foundFlag = false, discPerFlag = false, mfTestMatFlag = false;
            //bool mfTestMatFlagChange = false;
            bool NDTByUPVFlag = false;
            bool MinBillFlag = false;
            string tmpPartOT = "";
            decimal pitsRate = 0, coreRate = 0;
            //discount
            //int tmpActualRate = 0;
            decimal tmpActualRate = 0;
            decimal genericDiscPer = 0, tmpDiscRate = 0, tmpDiscPer = 0;
            bool appDiscFlag = false, tmpAppDiscFlag = false;
            //
            //ClientName = inwd.CL_Name_var;
            //tmpQty = Convert.ToInt32(inwd.INWD_TotalQty_int);
            //xDisc = dc.Discount_View(ClientId, SiteId, RecordType);
            //xDisc = xDisc / 100;
            //if (xDisc > 0)
            //{
            //    discPerFlag = true;
            //}
            DateTime BillDate = DateTime.Now;
            if (CheckGSTFlag(BillDate) == false)
            {
                var masterSrvTax = dc.MasterSetting_View(SiteId);
                foreach (var serTax in masterSrvTax)
                {
                    xSrvTax = Convert.ToDecimal(serTax.MASTER_ServiceTax_num);
                }
                if (xSrvTax > 0)
                {
                    xSrvTax = xSrvTax / 100;
                }
                if (xSrvTax > 0)
                {
                    var master = dc.MasterSetting_View(0);
                    foreach (var mst in master)
                    {
                        xSwTax = Convert.ToDecimal(mst.MASTER_SwachhBharatTax_num);
                        xKkTax = Convert.ToDecimal(mst.MASTER_KisanKrishiTax_num);
                    }
                }
                if (xSwTax > 0)
                {
                    xSwTax = xSwTax / 100;
                }
                if (xKkTax > 0)
                {
                    xKkTax = xKkTax / 100;
                }
            }
            else
            {
                bool cgstFlag = false, sgstFlag = false, igstFlag = false;
                var site = dc.Site_View(SiteId, 0, 0, "");
                foreach (var st in site)
                {
                    if (st.SITE_GST_bit != null)
                    {
                        if (st.SITE_SEZStatus_bit == false && st.SITE_State_var.ToLower() == "maharashtra")
                        {
                            cgstFlag = true;
                            sgstFlag = true;
                        }
                        else
                        {
                            igstFlag = true;
                        }
                    }
                }
                if (cgstFlag == false && sgstFlag == false && igstFlag == false)
                {
                    cgstFlag = true;
                    sgstFlag = true;
                }
                //decimal xCGST = 0, xSGST = 0, xIGST = 0;
                bool siteGSTFlag = false;
                var siteGst = dc.GST_Site_View(SiteId);
                foreach (var GSTax in siteGst)
                {
                    if (cgstFlag == true)
                        xCgst = Convert.ToDecimal(GSTax.GSTSITE_CGST_dec);
                    if (sgstFlag == true)
                        xSgst = Convert.ToDecimal(GSTax.GSTSITE_SGST_dec);
                    if (igstFlag == true)
                        xIgst = Convert.ToDecimal(GSTax.GSTSITE_IGST_dec);
                    siteGSTFlag = true;
                    break;
                }
                if (siteGSTFlag == false)
                {
                    var master = dc.GST_View(1, BillDate);
                    foreach (var GSTax in master)
                    {
                        if (cgstFlag == true)
                            xCgst = Convert.ToDecimal(GSTax.GST_CGST_dec);
                        if (sgstFlag == true)
                            xSgst = Convert.ToDecimal(GSTax.GST_SGST_dec);
                        if (igstFlag == true)
                            xIgst = Convert.ToDecimal(GSTax.GST_IGST_dec);
                        break;
                    }
                }
                if (xCgst > 0)
                {
                    xCgst = xCgst / 100;
                }
                if (xSgst > 0)
                {
                    xSgst = xSgst / 100;
                }
                if (xIgst > 0)
                {
                    xIgst = xIgst / 100;
                }
            }
            var otherTest = dc.Test_View(0, 0, "MS", 0, 0, 0).ToList();
            if (otherTest.Count() > 0)
            {
                miscTestId = Convert.ToInt32(otherTest.FirstOrDefault().TEST_Id);
            }
            ////get generic discount for client
            //genericDiscPer = getDiscount(ClientId, SiteId);
            ////
            string RecordType = ""; int RecordNo = 0, RecTypeStartRow = 0, RecTypeStartRowDetail = 0;
            var RecType = dc.Inward_View_Monthly(ClientId, SiteId, Fromdate, Todate, true, "");
            foreach (var recT in RecType)
            {
                RecTypeStartRow = i;
                //RecTypeStartRowDetail = iDetail;
                tmpQty = 0;

                RecordType = recT.INWD_RecordType_var;
                ////get generic discount for client
                //genericDiscPer = getDiscount(ClientId, SiteId, RecordType);
                ////
                string inwdDescr = "";
                string SACCode = "";
                if (RecordType == "SO" || RecordType == "GT")
                    SACCode = "998341";
                else
                    SACCode = "998346";
                var RecNo = dc.Inward_View_Monthly(ClientId, SiteId, Fromdate, Todate, false, RecordType);
                foreach (var recN in RecNo)
                {
                    RecTypeStartRowDetail = iDetail;
                    RecordNo = Convert.ToInt32(recN.INWD_RecordNo_int);

                    mfTestMatFlag = false;
                    int RecNoStartRow = 0;
                    MinBillFlag = false;
                    NDTByUPVFlag = false;
                    inwardAmt = 0;
                    tmpPartOT = "";
                    inwdDescr = "";
                    #region calculate record number specific gross amount
                    var inward = dc.Inward_Test_View(RecordType, RecordNo, "").ToList();
                    foreach (var inwd in inward)
                    {
                        //get generic discount for client
                        string[] strSplitStr = getDiscount(Convert.ToInt32(inwd.INWD_CL_Id), Convert.ToInt32(inwd.INWD_SITE_Id), RecordType, Convert.ToInt32(inwd.INWD_ENQ_Id)).Split('|');
                        genericDiscPer = Convert.ToDecimal(strSplitStr[0]);
                        appDiscFlag = Convert.ToBoolean(strSplitStr[1]);
                        //
                        if (int.TryParse(inwd.INWD_Charges_var, out otherChargesAmt) == true && RecNoStartRow == 0)
                        {
                            otherChargesAmt = Convert.ToInt32(inwd.INWD_Charges_var);
                        }
                        switch (RecordType)
                        {
                            case "AAC":
                                {
                                    #region aac
                                    inwdDescr = inwd.AACINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = Convert.ToInt32(inwd.AACINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region AAC
                                    tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = Convert.ToInt32(inwd.AACINWD_Quantity_tint);
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    foundFlag = false;
                                    for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                    {
                                        //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                        if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                        {
                                            arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + tmpQty).ToString();
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            foundFlag = true;
                                        }
                                    }
                                    if (foundFlag == false)
                                    {
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart;
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        //arrTestDetail[iDetail, 8] = genericDiscPer.ToString();
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion 
                                    break;
                                }
                            case "AGGT":
                                {
                                    #region Aggt
                                    inwdDescr = inwd.AGGTINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = 1;
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region aggt
                                    tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = 1;
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    foundFlag = false;
                                    for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                    {
                                        //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                        if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                        {
                                            arrTestDetail[j, 2] = arrTestDetail[j, 2] + ", " + inwd.AGGTINWD_AggregateName_var;
                                            arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + 1).ToString();
                                            //tmpQty = 1;
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            foundFlag = true;
                                        }
                                    }
                                    if (foundFlag == false)
                                    {
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart + " of " + inwd.AGGTINWD_AggregateName_var;
                                        //tmpQty = 1;
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end	
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "BT-":
                                {
                                    #region BT
                                    inwdDescr = inwd.BTINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    tmpQty = 1;
                                    if (tmpSrNo == 3) //Dimension Analysis
                                        tmpQty = 1;
                                    else if (tmpSrNo == 5) //Density
                                        tmpQty = Convert.ToInt32(inwd.BTTEST_Quantity_tint);
                                    else
                                        tmpQty = 5;
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region bt-
                                    tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = 1;
                                    //tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    foundFlag = false;
                                    for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                    {
                                        //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                        if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                        {
                                            if (arrTestDetail[j, 2].Contains(inwd.BTINWD_BrickType_var) == false)
                                                arrTestDetail[j, 2] = arrTestDetail[j, 2] + ", " + inwd.BTINWD_BrickType_var;

                                            //if (tmpSrNo == 3) //Dimension Analysis
                                            //    tmpQty = 1;
                                            //else if (tmpSrNo == 5) //Density
                                            //    tmpQty = Convert.ToInt32(inwd.BTTEST_Quantity_tint);
                                            //else
                                            //    tmpQty = 5;

                                            arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + tmpQty).ToString();

                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            foundFlag = true;
                                        }
                                    }
                                    if (foundFlag == false)
                                    {
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart + " of " + inwd.BTINWD_BrickType_var;
                                        //if (tmpSrNo == 3) //Dimension Analysis
                                        //    tmpQty = 1;
                                        //else if (tmpSrNo == 5) //Density
                                        //    tmpQty = Convert.ToInt32(inwd.BTTEST_Quantity_tint);
                                        //else
                                        //    tmpQty = 5;

                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end	
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "CCH":
                                {
                                    #region CCH
                                    inwdDescr = inwd.CCHINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    if (tmpRate > 0)
                                    {
                                        tmpAppDiscFlag = false;
                                        tmpDiscPer = 0;
                                        tmpDiscRate = 0;
                                        tmpActualRate = tmpRate;
                                        if (inwd.SITERATE_TestRateFlag == 0)
                                        {
                                            tmpDiscPer = genericDiscPer;
                                            tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                            tmpAppDiscFlag = appDiscFlag;
                                        }
                                        else
                                        {
                                            tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                            tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        }
                                        //end
                                        tmpQty = 1;
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                        #region cch
                                        tmpPart = inwd.TEST_Name_var;
                                        //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                        //tmpQty = 1;
                                        tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                        foundFlag = false;
                                        for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                        {
                                            //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                            if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                            {
                                                arrTestDetail[j, 2] = arrTestDetail[j, 2] + ", " + inwd.CCHINWD_CementName_var;
                                                arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + 1).ToString();
                                                //tmpQty = 1;
                                                //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                                foundFlag = true;
                                            }
                                        }
                                        if (foundFlag == false)
                                        {
                                            arrTestDetail[iDetail, 0] = tmpPart;
                                            arrTestDetail[iDetail, 2] = tmpPart + " of " + inwd.CCHINWD_CementName_var;
                                            //tmpQty = 1;
                                            arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                            arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                            arrTestDetail[iDetail, 5] = SACCode;
                                            //discount
                                            arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                            arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                            arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                            //end
                                            arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                            arrTestDetail[iDetail, 12] = inwdDescr;
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            iDetail = iDetail + 1;
                                        }
                                        #endregion
                                    }
                                    #endregion cch
                                    break;
                                }
                            case "GGBSCH":
                                {
                                    #region GGBSCH
                                    inwdDescr = inwd.GGBSCHINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    if (tmpRate > 0)
                                    {
                                        tmpAppDiscFlag = false;
                                        tmpDiscPer = 0;
                                        tmpDiscRate = 0;
                                        tmpActualRate = tmpRate;
                                        if (inwd.SITERATE_TestRateFlag == 0)
                                        {
                                            tmpDiscPer = genericDiscPer;
                                            tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                            tmpAppDiscFlag = appDiscFlag;
                                        }
                                        else
                                        {
                                            tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                            tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        }
                                        //end
                                        tmpQty = 1;
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                        #region ggbs
                                        tmpPart = inwd.TEST_Name_var;
                                        //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                        //tmpQty = 1;
                                        tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                        foundFlag = false;
                                        for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                        {
                                            if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                            {
                                                arrTestDetail[j, 2] = arrTestDetail[j, 2] + ", " + inwd.GGBSCHINWD_GgbsName_var;
                                                arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + 1).ToString();
                                                foundFlag = true;
                                            }
                                        }
                                        if (foundFlag == false)
                                        {
                                            arrTestDetail[iDetail, 0] = tmpPart;
                                            arrTestDetail[iDetail, 2] = tmpPart + " of " + inwd.GGBSCHINWD_GgbsName_var;
                                            //tmpQty = 1;
                                            arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                            arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                            arrTestDetail[iDetail, 5] = SACCode;
                                            //discount
                                            arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                            arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                            arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                            //end
                                            arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                            arrTestDetail[iDetail, 12] = inwdDescr;
                                            
                                            iDetail = iDetail + 1;
                                        }
                                        #endregion
                                    }
                                    #endregion ggbsch
                                    break;
                                }
                            case "CEMT":
                                {
                                    #region CEMT
                                    inwdDescr = inwd.CEMTINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = 1;
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    if (tmpSrNo != 5)
                                    {
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    }

                                    #region cemt
                                    if (inwd.CEMTTEST_Days_tint > 0)
                                        tmpPart = inwd.CEMTTEST_Days_tint + " Days " + inwd.TEST_Name_var;
                                    else
                                        tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = 1;
                                    //tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    if (tmpSrNo == 4)
                                    {
                                        tmpPart = "Initial & Final Setting Time";
                                    }
                                    foundFlag = false;
                                    if (tmpSrNo != 5)
                                    {
                                        for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                        {
                                            //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                            if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                            {
                                                arrTestDetail[j, 2] = arrTestDetail[j, 2] + ", " + inwd.CEMTINWD_CementName_var;
                                                arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + 1).ToString();
                                                //tmpQty = 1;
                                                //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                                foundFlag = true;
                                            }
                                        }
                                        if (foundFlag == false)
                                        {
                                            arrTestDetail[iDetail, 0] = tmpPart;
                                            arrTestDetail[iDetail, 2] = tmpPart + " of " + inwd.CEMTINWD_CementName_var;
                                            //tmpQty = 1;
                                            arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                            arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                            arrTestDetail[iDetail, 5] = SACCode;
                                            //discount
                                            arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                            arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                            arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                            //end
                                            arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                            arrTestDetail[iDetail, 12] = inwdDescr;
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            iDetail = iDetail + 1;
                                        }
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "GGBS":
                                {
                                    #region GGBS
                                    inwdDescr = inwd.GGBSINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = 1;
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    if (tmpSrNo != 4)
                                    {
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    }

                                    #region ggbs
                                    if (inwd.GGBSTEST_Days_tint > 0)
                                        tmpPart = inwd.GGBSTEST_Days_tint + " Days " + inwd.TEST_Name_var;
                                    else
                                        tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = 1;
                                    //tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    if (tmpSrNo == 3)
                                    {
                                        tmpPart = "Initial & Final Setting Time";
                                    }
                                    foundFlag = false;
                                    if (tmpSrNo != 4)
                                    {
                                        for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                        {
                                            //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                            if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                            {
                                                arrTestDetail[j, 2] = arrTestDetail[j, 2] + ", " + inwd.GGBSINWD_GgbsName_var;
                                                arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + 1).ToString();
                                                //tmpQty = 1;
                                                //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                                foundFlag = true;
                                            }
                                        }
                                        if (foundFlag == false)
                                        {
                                            arrTestDetail[iDetail, 0] = tmpPart;
                                            arrTestDetail[iDetail, 2] = tmpPart + " of " + inwd.GGBSINWD_GgbsName_var;
                                            //tmpQty = 1;
                                            arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                            arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                            arrTestDetail[iDetail, 5] = SACCode;
                                            //discount
                                            arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                            arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                            arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                            //end
                                            arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                            arrTestDetail[iDetail, 12] = inwdDescr;
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            iDetail = iDetail + 1;
                                        }
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "CR":
                                {
                                    #region CR
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    ////discount
                                    //tmpDiscRate = 0;
                                    //tmpActualRate = tmpRate;
                                    //if (inwd.SITERATE_TestRateFlag == 0)
                                    //{
                                    //    tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                    //    tmpRate = Convert.ToInt32(tmpDiscRate);
                                    //}
                                    ////end
                                    //tmpQty = Convert.ToInt32(inwd.CRINWD_Quantity_tint);
                                    //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    //inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region cr
                                    //if (RecNoStartRow == 0)
                                    //{
                                    //    ytmpAmt = 0;
                                    //    foreach (var CRinwd in inward)
                                    //    {
                                    //        tmpRate = Convert.ToDecimal(CRinwd.TEST_Rate_int);
                                    //        tmpTestId = Convert.ToInt32(CRinwd.TEST_Id);
                                    //        //discount
                                    //        tmpAppDiscFlag = false;
                                    //        tmpDiscRate = 0;
                                    //        tmpDiscPer = 0;
                                    //        tmpActualRate = tmpRate;
                                    //        if (CRinwd.SITERATE_TestRateFlag == 0)
                                    //        {
                                    //            tmpDiscPer = genericDiscPer;
                                    //            tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                    //            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    //            tmpAppDiscFlag = appDiscFlag;
                                    //        }
                                    //        else
                                    //        {
                                    //            tmpDiscRate = Convert.ToDecimal(CRinwd.DiscountedRate);
                                    //            tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                    //            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    //        }
                                    //        //end
                                    //        tmpQty = Convert.ToInt32(CRinwd.CRINWD_Quantity_tint);
                                    //        ytmpAmt = ytmpAmt + (tmpRate * tmpQty);
                                    //    }
                                    //    //if (inwd.TEST_Name_var == "Cutting & Testing")
                                    //    //{
                                    //    //    tmpPart = "Minimum Billing for core cutting and testing";
                                    //    //}
                                    //    //else if (inwd.TEST_Name_var == "Only Core Testing" || inwd.TEST_Name_var == "Core Testing excluding cutting")
                                    //    //{
                                    //        tmpPart = "Minimum Billing for core testing";
                                    //    //}
                                    //    var master = dc.MasterSetting_View(0);
                                    //    foreach (var mst in master)
                                    //    {
                                    //        if (inwd.TEST_Name_var == "Cutting & Testing")
                                    //        {
                                    //            if (ytmpAmt <= mst.MinBillforCoreCutTest_int)
                                    //            {
                                    //                MinBillFlag = true;
                                    //                ytmpAmt = Convert.ToDecimal(mst.MinBillforCoreCutTest_int);
                                    //            }
                                    //        }
                                    //        else if (inwd.TEST_Name_var == "Only Core Testing" || inwd.TEST_Name_var == "Core Testing excluding cutting")
                                    //        {
                                    //            if (ytmpAmt <= mst.MinBillforCoreTest_int)
                                    //            {
                                    //                MinBillFlag = true;
                                    //                ytmpAmt = Convert.ToDecimal(mst.MinBillforCoreTest_int);
                                    //            }
                                    //        }

                                    //    }
                                    //    if (MinBillFlag == false)
                                    //    {
                                    //        ytmpAmt = 0;
                                    //    }
                                    //}
                                    inwdDescr = inwd.CRINWD_Description_var;
                                    if (RecNoStartRow == 0)
                                    {
                                        ytmpAmt = 0;
                                        tmpQty = 0;
                                        MinBillFlag = false;
                                        if (inwd.TEST_Name_var == "Core Testing including cutting")
                                        {
                                            foreach (var CRinwd in inward)
                                            {
                                                tmpQty += Convert.ToInt32(CRinwd.CRINWD_Quantity_tint);
                                            }
                                            if (tmpQty < 4)
                                            {
                                                tmpPart = "Minimum Billing for core testing";
                                                var master = dc.MasterSetting_View(0);
                                                foreach (var mst in master)
                                                {
                                                    MinBillFlag = true;
                                                    ytmpAmt = Convert.ToDecimal(mst.MinBillforCoreCutTest_int);
                                                }
                                            }
                                        }
                                    }
                                    if (MinBillFlag == false)
                                    {
                                        tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                        tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                        //discount
                                        tmpAppDiscFlag = false;
                                        tmpDiscPer = 0;
                                        tmpDiscRate = 0;
                                        tmpActualRate = tmpRate;
                                        if (inwd.SITERATE_TestRateFlag == 0)
                                        {
                                            tmpDiscPer = genericDiscPer;
                                            tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                            tmpAppDiscFlag = appDiscFlag;
                                        }
                                        else
                                        {
                                            tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                            tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        }
                                        //end
                                        tmpQty = Convert.ToInt32(inwd.CRINWD_Quantity_tint);
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                        //detail
                                        if (inwd.TEST_Name_var == "Cutting & Testing")
                                        {
                                            tmpPart = "Bill for core cutting and testing of " + inwd.CRINWD_Description_var;
                                        }
                                        else if (inwd.TEST_Name_var == "Only Core Testing" || inwd.TEST_Name_var == "Core Testing excluding cutting")
                                        {
                                            tmpPart = "Bill for core testing of " + inwd.CRINWD_Description_var;
                                        }
                                        else
                                        {
                                            tmpPart = inwd.TEST_Name_var;
                                        }
                                        //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                        //tmpQty = Convert.ToInt32(inwd.CRINWD_Quantity_tint);
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart;
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        iDetail = iDetail + 1;
                                        RecNoStartRow++;
                                    }
                                    else if (RecNoStartRow == 0)
                                    {
                                        tmpRate = Convert.ToDecimal(ytmpAmt);
                                        //discount
                                        tmpAppDiscFlag = false;
                                        tmpDiscPer = 0;
                                        tmpDiscRate = 0;
                                        tmpActualRate = tmpRate;
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        ytmpAmt = tmpRate;
                                        //end
                                        tmpQty = 1;
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                        //detail
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 1] = ytmpAmt.ToString();
                                        arrTestDetail[iDetail, 3] = "1";
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //test id
                                        tmpTestId = 0;
                                        var testMinBill = dc.Test(0, "", 0, "CR", tmpPart, 0);
                                        foreach (var testId in testMinBill)
                                        {
                                            tmpTestId = Convert.ToInt32(testId.TEST_Id);
                                        }
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        //
                                        iDetail = iDetail + 1;
                                        RecNoStartRow++;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "CORECUT":
                                {
                                    #region CR Cutt
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    ////discount
                                    //tmpDiscRate = 0;
                                    //tmpActualRate = tmpRate;
                                    //if (inwd.SITERATE_TestRateFlag == 0)
                                    //{
                                    //    tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                    //    tmpRate = Convert.ToInt32(tmpDiscRate);
                                    //}
                                    ////end
                                    //tmpQty = Convert.ToInt32(inwd.CORECUTINWD_Quantity_tint);
                                    //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    //inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region corecut
                                    if (RecNoStartRow == 0)
                                    {
                                        ytmpAmt = 0;
                                        foreach (var CRCuTinwd in inward)
                                        {
                                            tmpRate = Convert.ToDecimal(CRCuTinwd.TEST_Rate_int);
                                            tmpTestId = Convert.ToInt32(CRCuTinwd.TEST_Id);
                                            //discount
                                            tmpAppDiscFlag = false;
                                            tmpDiscPer = 0;
                                            tmpDiscRate = 0;
                                            tmpActualRate = tmpRate;
                                            if (CRCuTinwd.SITERATE_TestRateFlag == 0)
                                            {
                                                tmpDiscPer = genericDiscPer;
                                                tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                                tmpRate = Convert.ToDecimal(tmpDiscRate);
                                                tmpAppDiscFlag = appDiscFlag;
                                            }
                                            else
                                            {
                                                tmpDiscRate = Convert.ToDecimal(CRCuTinwd.DiscountedRate);
                                                tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                                tmpRate = Convert.ToDecimal(tmpDiscRate);
                                            }
                                            //end
                                            tmpQty = Convert.ToInt32(CRCuTinwd.CORECUTINWD_Quantity_tint);
                                            ytmpAmt = ytmpAmt + (tmpRate * tmpQty);
                                            tmpPart = "Minimum Billing for Core Cutting";
                                        }
                                        var master = dc.MasterSetting_View(0);
                                        foreach (var mst in master)
                                        {
                                            if (ytmpAmt <= mst.MinBillforCoreCut_int)
                                            {
                                                MinBillFlag = true;
                                                ytmpAmt = Convert.ToDecimal(mst.MinBillforCoreCut_int);
                                            }
                                        }
                                        if (MinBillFlag == false)
                                        {
                                            ytmpAmt = 0;
                                        }
                                    }
                                    if (MinBillFlag == false)
                                    {
                                        tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                        tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                        //discount
                                        tmpAppDiscFlag = false;
                                        tmpDiscPer = 0;
                                        tmpDiscRate = 0;
                                        tmpActualRate = tmpRate;
                                        if (inwd.SITERATE_TestRateFlag == 0)
                                        {
                                            tmpDiscPer = genericDiscPer;
                                            tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                            tmpAppDiscFlag = appDiscFlag;
                                        }
                                        else
                                        {
                                            tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                            tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        }
                                        //end
                                        tmpQty = Convert.ToInt32(inwd.CORECUTINWD_Quantity_tint);
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                        //detail
                                        tmpPart = inwd.TEST_Name_var;
                                        //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                        //tmpQty = Convert.ToInt32(inwd.CORECUTINWD_Quantity_tint);

                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart;
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        iDetail = iDetail + 1;
                                        RecNoStartRow++;
                                    }
                                    else if (RecNoStartRow == 0)
                                    {
                                        tmpRate = Convert.ToDecimal(ytmpAmt);
                                        //discount
                                        tmpAppDiscFlag = false;
                                        tmpDiscPer = 0;
                                        tmpDiscRate = 0;
                                        tmpActualRate = tmpRate;
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        ytmpAmt = tmpRate;
                                        tmpAppDiscFlag = appDiscFlag;
                                        //end
                                        tmpQty = 1;
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                        //
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 1] = ytmpAmt.ToString();
                                        arrTestDetail[iDetail, 3] = "1";
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //test id
                                        tmpTestId = 0;
                                        var testMinBill = dc.Test(0, "", 0, "CORECUT", tmpPart, 0);
                                        foreach (var testId in testMinBill)
                                        {
                                            tmpTestId = Convert.ToInt32(testId.TEST_Id);
                                        }
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();

                                        iDetail = iDetail + 1;
                                        RecNoStartRow++;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "CT":
                                {
                                    #region CT
                                    inwdDescr = inwd.CTINWD_Description_var;
                                    if (RecNoStartRow == 0)
                                    {
                                        tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                        tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                        //discount
                                        tmpAppDiscFlag = false;
                                        tmpDiscPer = 0;
                                        tmpDiscRate = 0;
                                        tmpActualRate = tmpRate;
                                        if (inwd.SITERATE_TestRateFlag == 0)
                                        {
                                            tmpDiscPer = genericDiscPer;
                                            tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                            tmpAppDiscFlag = appDiscFlag;
                                        }
                                        else
                                        {
                                            tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                            tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        }
                                        //end
                                        tmpQty = Convert.ToInt32(inwd.INWD_TotalQty_int);
                                        if (inwd.CTINWD_TestType_var.Contains("Accelerated Curing") == true)
                                        {
                                            tmpQty = 1;
                                        }
                                        else if (tmpQty < 3)
                                        {
                                            tmpRate = 3 * tmpRate;
                                            tmpActualRate = 3 * tmpActualRate;
                                            tmpQty = 1;
                                        }
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                        //RecNoStartRow++;
                                    }
                                    #region ct
                                    if (RecNoStartRow == 0)
                                    {
                                        //if (iDetail > RecTypeStartRow)
                                        //    iDetail--;
                                        tmpPart = inwd.TEST_Name_var;
                                        //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                        //tmpQty += Convert.ToInt32(inwd.INWD_TotalQty_int);
                                        tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);

                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart;
                                        if (inwd.CTINWD_TestType_var.Contains("Accelerated Curing") == true)
                                        {
                                            tmpQty = 1;
                                        }
                                        else if (tmpQty < 3)
                                        {
                                            //tmpRate = 3 * tmpRate;
                                            arrTestDetail[iDetail, 2] = arrTestDetail[iDetail, 2] + " (Minimum Billing)";
                                            tmpQty = 1;
                                        }
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                        //tmpAmt = tmpRate * tmpQty;
                                        iDetail = iDetail + 1;
                                        RecNoStartRow++;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "FLYASH":
                                {
                                    #region FA
                                    inwdDescr = inwd.FLYASHINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = 1;
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    if (tmpSrNo != 3)
                                    {
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    }

                                    #region flyash
                                    tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = 1;
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    if (tmpSrNo == 2)
                                    {
                                        tmpPart = "Initial & Final Setting Time";
                                    }
                                    foundFlag = false;
                                    if (tmpSrNo != 3)
                                    {
                                        for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                        {
                                            //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                            if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                            {
                                                arrTestDetail[j, 2] = arrTestDetail[j, 2] + ", " + inwd.FLYASHINWD_FlyAshName_var;
                                                arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + 1).ToString();
                                                //tmpQty = 1;
                                                //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                                foundFlag = true;
                                            }
                                        }
                                        if (foundFlag == false)
                                        {
                                            arrTestDetail[iDetail, 0] = tmpPart;
                                            arrTestDetail[iDetail, 2] = tmpPart + " of Fly Ash " + inwd.FLYASHINWD_FlyAshName_var;
                                            //tmpQty = 1;
                                            arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                            arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                            arrTestDetail[iDetail, 5] = SACCode;
                                            //discount
                                            arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                            arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                            arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                            //end
                                            arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                            arrTestDetail[iDetail, 12] = inwdDescr;
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            iDetail = iDetail + 1;
                                        }
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "MF":
                                {
                                    #region MF
                                    inwdDescr = "";
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = 1;
                                    if (inwd.MFINWD_TestMaterial_bit == true)
                                    {
                                        mfTestMatFlag = true;
                                    }
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region mf
                                    tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = 1;
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    foundFlag = false;
                                    //if (inwd.MFINWD_TestMaterial_bit == true)
                                    //{
                                    //    mfTestMatFlag = true;
                                    //}
                                    for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                    {
                                        //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                        if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                        {
                                            arrTestDetail[j, 2] = arrTestDetail[j, 2].Replace(" Grade Of Concrete", "") + ", " + inwd.MFINWD_Grade_var + " Grade Of Concrete";
                                            arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + 1).ToString();
                                            //tmpQty = 1;
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            foundFlag = true;
                                        }
                                    }
                                    if (foundFlag == false)
                                    {
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart + " of " + inwd.MFINWD_Grade_var + " Grade Of Concrete";
                                        //tmpQty = 1;
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "NDT":
                                {
                                    #region NDT
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    ////discount
                                    //tmpDiscRate = 0;
                                    //tmpActualRate = tmpRate;
                                    //if (inwd.SITERATE_TestRateFlag == 0)
                                    //{
                                    //    tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                    //    tmpRate = Convert.ToInt32(tmpDiscRate);
                                    //}
                                    ////end
                                    //tmpQty = Convert.ToInt32(inwd.NDTTEST_Points_tint);
                                    //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    //inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    inwdDescr = "";
                                    #region ndt
                                    if (RecNoStartRow == 0)
                                    {
                                        ytmpAmt = 0;
                                        tmpPart = "";
                                        foreach (var NDTinwd in inward)
                                        {
                                            tmpRate = Convert.ToDecimal(NDTinwd.TEST_Rate_int);
                                            tmpTestId = Convert.ToInt32(NDTinwd.TEST_Id);
                                            //discount
                                            tmpAppDiscFlag = false;
                                            tmpDiscPer = 0;
                                            tmpDiscRate = 0;
                                            tmpActualRate = tmpRate;
                                            if (NDTinwd.SITERATE_TestRateFlag == 0)
                                            {
                                                tmpDiscPer = genericDiscPer;
                                                tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                                tmpRate = Convert.ToDecimal(tmpDiscRate);
                                                tmpAppDiscFlag = appDiscFlag;
                                            }
                                            else
                                            {
                                                tmpDiscRate = Convert.ToDecimal(NDTinwd.DiscountedRate);
                                                tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                                tmpRate = Convert.ToDecimal(tmpDiscRate);
                                            }
                                            //end
                                            tmpQty = Convert.ToInt32(NDTinwd.NDTTEST_Points_tint);
                                            ytmpAmt = ytmpAmt + (tmpRate * tmpQty);
                                            if (tmpPart == "")
                                                tmpPart = "Minimum Billing For NDT Of " + NDTinwd.NDTTEST_NatureofWorking_var + " by " + NDTinwd.NDTTEST_NDTBy_var;
                                            else
                                                tmpPart = tmpPart + ", " + NDTinwd.NDTTEST_NatureofWorking_var + " by " + NDTinwd.NDTTEST_NDTBy_var;
                                            if (NDTinwd.NDTTEST_NDTBy_var == "UPV")
                                                NDTByUPVFlag = true;
                                        }
                                        var master = dc.MasterSetting_View(0);
                                        foreach (var mst in master)
                                        {
                                            if (NDTByUPVFlag == true)
                                            {
                                                if (ytmpAmt <= mst.MinBillforNDTUPVandHamm_int)
                                                {
                                                    MinBillFlag = true;
                                                    ytmpAmt = Convert.ToDecimal(mst.MinBillforNDTUPVandHamm_int);
                                                }
                                            }
                                            else
                                            {
                                                if (ytmpAmt <= mst.MinBillforNDTHamm_int)
                                                {
                                                    MinBillFlag = true;
                                                    ytmpAmt = Convert.ToDecimal(mst.MinBillforNDTHamm_int);
                                                }
                                            }
                                        }
                                        if (MinBillFlag == false)
                                        {
                                            ytmpAmt = 0;
                                        }
                                    }
                                    if (MinBillFlag == false)
                                    {
                                        tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                        tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                        //discount
                                        tmpAppDiscFlag = false;
                                        tmpDiscPer = 0;
                                        tmpDiscRate = 0;
                                        tmpActualRate = tmpRate;
                                        if (inwd.SITERATE_TestRateFlag == 0)
                                        {
                                            tmpDiscPer = genericDiscPer;
                                            tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                            tmpAppDiscFlag = appDiscFlag;
                                        }
                                        else
                                        {
                                            tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                            tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        }
                                        //end
                                        tmpQty = Convert.ToInt32(inwd.NDTTEST_Points_tint);
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                        tmpPart = inwd.TEST_Name_var;
                                        //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);

                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart;
                                        //tmpQty = Convert.ToInt32(inwd.NDTTEST_Points_tint);
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        iDetail = iDetail + 1;
                                    }
                                    else if (RecNoStartRow == 0)
                                    {
                                        tmpRate = Convert.ToDecimal(ytmpAmt);
                                        //discount
                                        tmpAppDiscFlag = false;
                                        tmpDiscPer = 0;
                                        tmpDiscRate = 0;
                                        tmpActualRate = tmpRate;
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        ytmpAmt = tmpRate;
                                        tmpAppDiscFlag = appDiscFlag;
                                        //end
                                        tmpQty = 1;
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 1] = ytmpAmt.ToString();
                                        arrTestDetail[iDetail, 3] = "1";
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //test id
                                        if (NDTByUPVFlag == true)
                                            tmpPart = "Minimum Billing For NDT by UPV & Rebound Hammer";
                                        else
                                            tmpPart = "Minimum Billing For NDT by Rebound Hammer";
                                        tmpTestId = 0;
                                        var testMinBill = dc.Test(0, "", 0, "NDT", tmpPart, 0);
                                        foreach (var testId in testMinBill)
                                        {
                                            tmpTestId = Convert.ToInt32(testId.TEST_Id);
                                        }
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        //
                                        iDetail = iDetail + 1;
                                        RecNoStartRow++;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "OT":
                                {
                                    #region Other
                                    inwdDescr = inwd.OTINWD_Description_var;
                                    //tmpRate = Convert.ToDecimal(inwd.OTRATEIN_Rate_int);
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = Convert.ToInt32(inwd.OTRATEIN_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region ot
                                    tmpPart = inwd.TEST_Name_var;
                                    tmpPartOT = inwd.OTINWD_SubTest_var.ToString();
                                    //tmpRate = Convert.ToInt32(inwd.OTRATEIN_Rate_int);
                                    //tmpQty = Convert.ToInt32(inwd.OTRATEIN_Quantity_tint);
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    foundFlag = false;
                                    for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                    {
                                        //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                        if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                        {
                                            arrTestDetail[j, 2] = arrTestDetail[j, 2] + ", " + inwd.OTINWD_Description_var;
                                            arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + tmpQty).ToString();
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            foundFlag = true;
                                        }
                                    }
                                    if (foundFlag == false)
                                    {
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart + " of " + inwd.OTINWD_Description_var;
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "PT":
                                {
                                    #region PT
                                    inwdDescr = inwd.PTINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = Convert.ToInt32(inwd.PTINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region pt
                                    tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = Convert.ToInt32(inwd.PTINWD_Quantity_tint);
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    foundFlag = false;
                                    for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                    {
                                        //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                        if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                        {
                                            arrTestDetail[j, 2] = arrTestDetail[j, 2];
                                            arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + tmpQty).ToString();
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            foundFlag = true;
                                        }
                                    }
                                    if (foundFlag == false)
                                    {
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart;
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "PILE":
                                {
                                    #region Pile
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    ////discount
                                    //tmpDiscRate = 0;
                                    //tmpActualRate = tmpRate;
                                    //if (inwd.SITERATE_TestRateFlag == 0)
                                    //{
                                    //    tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                    //    tmpRate = Convert.ToInt32(tmpDiscRate);
                                    //}
                                    ////end
                                    //tmpQty = Convert.ToInt32(inwd.PILEINWD_Quantity_tint);
                                    //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    //inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region pile
                                    inwdDescr = inwd.PILEINWD_Description_var;
                                    if (RecNoStartRow == 0)
                                    {
                                        ytmpAmt = 0;
                                        foreach (var PILEinwd in inward)
                                        {
                                            tmpRate = Convert.ToDecimal(PILEinwd.TEST_Rate_int);
                                            tmpTestId = Convert.ToInt32(PILEinwd.TEST_Id);
                                            //discount
                                            tmpAppDiscFlag = false;
                                            tmpDiscPer = 0;
                                            tmpDiscRate = 0;
                                            tmpActualRate = tmpRate;
                                            if (PILEinwd.SITERATE_TestRateFlag == 0)
                                            {
                                                tmpDiscPer = genericDiscPer;
                                                tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                                tmpRate = Convert.ToDecimal(tmpDiscRate);
                                                tmpAppDiscFlag = appDiscFlag;
                                            }
                                            else
                                            {
                                                tmpDiscRate = Convert.ToDecimal(PILEinwd.DiscountedRate);
                                                tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                                tmpRate = Convert.ToDecimal(tmpDiscRate);
                                            }
                                            //end
                                            tmpQty = Convert.ToInt32(PILEinwd.PILEINWD_Quantity_tint);
                                            ytmpAmt = ytmpAmt + (tmpRate * tmpQty);
                                            if (tmpPart == "")
                                                tmpPart = "Minimum Billing for Pile Integrity Testing of " + PILEinwd.PILEINWD_Description_var;
                                            else
                                                tmpPart = tmpPart + ", " + PILEinwd.PILEINWD_Description_var;
                                        }
                                        var master = dc.MasterSetting_View(0);
                                        foreach (var mst in master)
                                        {
                                            if (ytmpAmt <= mst.MinBillforPILE_int)
                                            {
                                                MinBillFlag = true;
                                                ytmpAmt = Convert.ToDecimal(mst.MinBillforPILE_int);
                                            }
                                        }
                                        if (MinBillFlag == false)
                                        {
                                            ytmpAmt = 0;
                                        }
                                    }
                                    if (MinBillFlag == false)
                                    {
                                        tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                        tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                        //discount
                                        tmpAppDiscFlag = false;
                                        tmpDiscPer = 0;
                                        tmpDiscRate = 0;
                                        tmpActualRate = tmpRate;
                                        if (inwd.SITERATE_TestRateFlag == 0)
                                        {
                                            tmpDiscPer = genericDiscPer;
                                            tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                            tmpAppDiscFlag = appDiscFlag;
                                        }
                                        else
                                        {
                                            tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                            tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                            tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        }
                                        //end
                                        tmpQty = Convert.ToInt32(inwd.PILEINWD_Quantity_tint);
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                        tmpPart = inwd.TEST_Name_var;
                                        //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                        //tmpQty = Convert.ToInt32(inwd.PILEINWD_Quantity_tint);
                                        tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                        foundFlag = false;
                                        for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                        {
                                            //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                            if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                            {
                                                arrTestDetail[j, 2] = arrTestDetail[j, 2] + ", " + inwd.PILEINWD_Description_var;
                                                arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + tmpQty).ToString();
                                                //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                                foundFlag = true;
                                            }
                                        }
                                        if (foundFlag == false)
                                        {
                                            arrTestDetail[iDetail, 0] = tmpPart;
                                            arrTestDetail[iDetail, 2] = tmpPart + " of " + inwd.PILEINWD_Description_var;
                                            arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                            arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                            arrTestDetail[iDetail, 5] = SACCode;
                                            //discount
                                            arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                            arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                            arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                            //end
                                            arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                            arrTestDetail[iDetail, 12] = inwdDescr;
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            iDetail = iDetail + 1;
                                        }
                                    }
                                    else if (RecNoStartRow == 0)
                                    {
                                        tmpRate = Convert.ToDecimal(ytmpAmt);
                                        //discount
                                        tmpAppDiscFlag = false;
                                        tmpDiscPer = 0;
                                        tmpDiscRate = 0;
                                        tmpActualRate = tmpRate;
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        ytmpAmt = tmpRate;
                                        tmpAppDiscFlag = appDiscFlag;
                                        //end
                                        tmpQty = 1;
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 1] = ytmpAmt.ToString();
                                        arrTestDetail[iDetail, 3] = "1";
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //test id
                                        tmpPart = "Minimum Billing for Pile Integrity Testing";
                                        tmpTestId = 0;
                                        var testMinBill = dc.Test(0, "", 0, "PILE", tmpPart, 0);
                                        foreach (var testId in testMinBill)
                                        {
                                            tmpTestId = Convert.ToInt32(testId.TEST_Id);
                                        }
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        //
                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "GT":
                            case "RWH":
                                {
                                    #region RWH    
                                    inwdDescr = "";
                                    tmpRate = Convert.ToDecimal(inwd.GTTEST_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.GTTEST_Quantity_tint);
                                    if (inwd.GTTEST_TEST_Id != null)
                                        tmpTestId = Convert.ToInt32(inwd.GTTEST_TEST_Id);
                                    else
                                        tmpTestId = 0;
                                    if (inwd.GTINW_LumpSump_tint == 1)
                                    {
                                        tmpAmt = tmpAmt + tmpRate;
                                        inwardAmt = inwardAmt + tmpRate;
                                    }
                                    else
                                    {
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    }

                                    #region gt,rwh
                                    tmpPart = inwd.GTTEST_Description_var;
                                    //tmpRate = Convert.ToInt32(inwd.GTTEST_Rate_int);
                                    //tmpQty = Convert.ToInt32(inwd.GTTEST_Quantity_tint);
                                    if (inwd.GTINW_LumpSump_tint == 1)
                                    {
                                        if (RecordType == "GT")
                                            arrTestDetail[iDetail, 0] = "Soil Investigation(For details reffer attachment)";
                                        else
                                            arrTestDetail[iDetail, 0] = "Rain Water Harvesting(For details reffer attachment)";

                                        arrTestDetail[iDetail, 2] = arrTestDetail[iDetail, 0];
                                        arrTestDetail[iDetail, 1] = (Convert.ToInt32(arrTestDetail[iDetail, 1]) + tmpRate).ToString();
                                        arrTestDetail[iDetail, 3] = "1";
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //tmpAmt = tmpAmt + tmpRate;
                                        //test id
                                        if (RecordType == "GT")
                                            tmpPart = "Soil Investigation";
                                        else
                                            tmpPart = "Rain Water Harvesting";
                                        tmpTestId = 0;
                                        var testMinBill = dc.Test(0, "", 0, RecordType, tmpPart, 0);
                                        foreach (var testId in testMinBill)
                                        {
                                            tmpTestId = Convert.ToInt32(testId.TEST_Id);
                                        }
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                        //end
                                    }
                                    else
                                    {
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart;
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        //test id
                                        if (tmpTestId == 0)
                                        {
                                            if (RecordType == "GT")
                                                tmpPart = "Soil Investigation";
                                            else
                                                tmpPart = "Rain Water Harvesting";
                                            tmpTestId = 0;
                                            var testMinBill = dc.Test(0, "", 0, RecordType, tmpPart, 0);
                                            foreach (var testId in testMinBill)
                                            {
                                                tmpTestId = Convert.ToInt32(testId.TEST_Id);
                                            }
                                        }
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "SO":
                                {
                                    #region SO
                                    inwdDescr = inwd.SOINWD_Description_var;
                                    if (RecNoStartRow == 0)
                                    {
                                        ytmpAmt = 0;
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
                                                        tmpRate = Convert.ToDecimal(mst.MinBillforSO_int);
                                                        //discount
                                                        tmpAppDiscFlag = false;
                                                        tmpDiscPer = 0;
                                                        tmpDiscRate = 0;
                                                        tmpActualRate = tmpRate;
                                                        tmpDiscPer = genericDiscPer;
                                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                                        tmpAppDiscFlag = appDiscFlag;
                                                        //end
                                                        tmpQty = 1;
                                                        tmpAmt = tmpAmt + tmpRate;
                                                        inwardAmt = inwardAmt + tmpRate;
                                                        tmpPart = "Minimum Billing For Soil Testing";

                                                        arrTestDetail[iDetail, 0] = tmpPart;
                                                        arrTestDetail[iDetail, 2] = tmpPart;
                                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                                        arrTestDetail[iDetail, 5] = SACCode;
                                                        //discount
                                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                                        //end
                                                        //arrTest[i, 4] = tmpSrNo.ToString();
                                                        //test id
                                                        tmpTestId = 0;
                                                        var testMinBill = dc.Test(0, "", 0, "SO", tmpPart, 0);
                                                        foreach (var testId in testMinBill)
                                                        {
                                                            tmpTestId = Convert.ToInt32(testId.TEST_Id);
                                                        }
                                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                                        //
                                                        iDetail = iDetail + 1;
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
                                            ytmpAmt = 0;
                                        }
                                    }
                                    ////
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = 1;
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);

                                    if (tmpSrNo == 10) //sand
                                    {
                                        tmpQty = Convert.ToInt32(inwd.SOINWD_Pits_tint);
                                        noOfPits = noOfPits + tmpQty;
                                        pitsRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    }
                                    else if (tmpSrNo == 11) //core
                                    {
                                        tmpQty = Convert.ToInt32(inwd.SOINWD_Cores_tint);
                                        noOfCore = noOfCore + tmpQty;
                                        coreRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    }
                                    else if (tmpSrNo == 12)//classification
                                    {
                                        tmpQty = Convert.ToInt32(inwd.SOINWD_Quantity_tint);
                                    }
                                    if (tmpSrNo != 10 && tmpSrNo != 11)
                                    {
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    }

                                    #region so
                                    tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = 1;
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    foundFlag = false;
                                    for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                    {
                                        //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                        if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                        {
                                            //if (inwd.TEST_Sr_No == 10) //sand
                                            //{
                                            //    tmpQty = Convert.ToInt32(inwd.SOINWD_Pits_tint);
                                            //    noOfPits = noOfPits + tmpQty;
                                            //}
                                            //else if (inwd.TEST_Sr_No == 11) //core
                                            //{
                                            //    tmpQty = Convert.ToInt32(inwd.SOINWD_Cores_tint);
                                            //    noOfCore = noOfCore + tmpQty;
                                            //}
                                            //else if (inwd.TEST_Sr_No == 12)//classification
                                            //{
                                            //    tmpQty = Convert.ToInt32(inwd.SOINWD_Quantity_tint);
                                            //}                                            
                                            arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + tmpQty).ToString();
                                            foundFlag = true;
                                        }
                                    }
                                    if (foundFlag == false)
                                    {
                                        //if (inwd.TEST_Sr_No == 10) //sand
                                        //{
                                        //    tmpQty = Convert.ToInt32(inwd.SOINWD_Pits_tint);
                                        //    noOfPits = noOfPits + tmpQty;
                                        //}
                                        //else if (inwd.TEST_Sr_No == 11) //core
                                        //{
                                        //    tmpQty = Convert.ToInt32(inwd.SOINWD_Cores_tint);
                                        //    noOfCore = noOfCore + tmpQty;
                                        //}
                                        //else if (inwd.TEST_Sr_No == 12)//classification
                                        //{
                                        //    tmpQty = Convert.ToInt32(inwd.SOINWD_Quantity_tint);
                                        //}

                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart;
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 4] = tmpSrNo.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);                                        
                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }

                            case "SOLID":
                                {
                                    #region Solid
                                    inwdDescr = inwd.SOLIDINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = Convert.ToInt32(inwd.SOLIDINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region solid
                                    tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = Convert.ToInt32(inwd.SOLIDINWD_Quantity_tint);
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    foundFlag = false;
                                    for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                    {
                                        //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                        if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                        {
                                            arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + tmpQty).ToString();
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            foundFlag = true;
                                        }
                                    }
                                    if (foundFlag == false)
                                    {
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart;
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "ST":
                                {
                                    #region steel
                                    inwdDescr = inwd.STINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = Convert.ToInt32(inwd.STINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region st
                                    tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = Convert.ToInt32(inwd.STINWD_Quantity_tint);
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    foundFlag = false;
                                    for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                    {
                                        //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                        if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                        {
                                            arrTestDetail[j, 2] = arrTestDetail[j, 2] + ", " + inwd.STINWD_Diameter_tint + " mm";
                                            arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + tmpQty).ToString();
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            foundFlag = true;
                                        }
                                    }
                                    if (foundFlag == false)
                                    {
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart + " of " + inwd.STINWD_Diameter_tint + " mm";
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "STC":
                                {
                                    #region STc
                                    inwdDescr = inwd.STCINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = Convert.ToInt32(inwd.STCINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region stc
                                    tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = Convert.ToInt32(inwd.STCINWD_Quantity_tint);

                                    arrTestDetail[iDetail, 0] = tmpPart;
                                    if (arrTestDetail[iDetail, 2] == "" || arrTestDetail[iDetail, 2] == null)
                                        arrTestDetail[iDetail, 2] = tmpPart + " of " + inwd.STCINWD_Daimeter_tint + " mm";
                                    else
                                        arrTestDetail[iDetail, 2] = arrTestDetail[iDetail, 2] + ", " + inwd.STCINWD_Daimeter_tint + " mm";

                                    arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                    if (arrTestDetail[iDetail, 3] == "" || arrTestDetail[iDetail, 3] == null)
                                        arrTestDetail[iDetail, 3] = "0";
                                    arrTestDetail[iDetail, 3] = (Convert.ToInt32(arrTestDetail[iDetail, 3]) + tmpQty).ToString();
                                    arrTestDetail[iDetail, 5] = SACCode;
                                    //discount
                                    arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                    arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                    arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                    //end
                                    arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                    arrTestDetail[iDetail, 12] = inwdDescr;
                                    //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    if (Convert.ToInt32(arrTestDetail[iDetail, 3]) == inwd.INWD_TotalQty_int)
                                    {
                                        if (arrTestDetail[iDetail, 2].Contains("mm,") == true)
                                            arrTestDetail[iDetail, 2] = arrTestDetail[iDetail, 2] + " Reinforcement Steel Bars";
                                        else
                                            arrTestDetail[iDetail, 2] = arrTestDetail[iDetail, 2] + " Reinforcement Steel Bar";

                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "TILE":
                                {
                                    #region tile
                                    inwdDescr = inwd.TILEINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = Convert.ToInt32(inwd.TILEINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region tile
                                    tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = Convert.ToInt32(inwd.TILEINWD_Quantity_tint);
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    foundFlag = false;
                                    for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                    {
                                        //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                        if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                        {
                                            arrTestDetail[j, 2] = arrTestDetail[j, 2] + ", " + inwd.TILEINWD_TileType_var;
                                            arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + tmpQty).ToString();
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            foundFlag = true;
                                        }
                                    }
                                    if (foundFlag == false)
                                    {
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart + " of " + inwd.TILEINWD_TileType_var;
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                            case "WT":
                                {
                                    #region WT
                                    inwdDescr = inwd.WTINWD_Description_var;
                                    tmpRate = Convert.ToDecimal(inwd.TEST_Rate_int);
                                    tmpTestId = Convert.ToInt32(inwd.TEST_Id);
                                    //discount
                                    tmpAppDiscFlag = false;
                                    tmpDiscPer = 0;
                                    tmpDiscRate = 0;
                                    tmpActualRate = tmpRate;
                                    if (inwd.SITERATE_TestRateFlag == 0)
                                    {
                                        tmpDiscPer = genericDiscPer;
                                        tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                        tmpAppDiscFlag = appDiscFlag;
                                    }
                                    else
                                    {
                                        tmpDiscRate = Convert.ToDecimal(inwd.DiscountedRate);
                                        tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                        tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    }
                                    //end
                                    tmpQty = Convert.ToInt32(inwd.WTTEST_Qty_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                    #region wt
                                    tmpPart = inwd.TEST_Name_var;
                                    //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //tmpQty = Convert.ToInt32(inwd.WTTEST_Qty_tint);
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    foundFlag = false;
                                    for (int j = RecTypeStartRowDetail; j <= iDetail; j++)
                                    {
                                        //if (arrTestDetail[j, 0] == tmpPart && arrTestDetail[j, 1] == tmpRate.ToString())
                                        if (arrTestDetail[j, 11] == tmpTestId.ToString() && arrTestDetail[j, 1] == tmpRate.ToString())
                                        {
                                            arrTestDetail[j, 2] = arrTestDetail[j, 2] + ", " + inwd.WTINWD_Description_var;
                                            arrTestDetail[j, 3] = (Convert.ToInt32(arrTestDetail[j, 3]) + tmpQty).ToString();
                                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                            foundFlag = true;
                                        }
                                    }
                                    if (foundFlag == false)
                                    {
                                        arrTestDetail[iDetail, 0] = tmpPart;
                                        arrTestDetail[iDetail, 2] = tmpPart + " of Water " + inwd.WTINWD_Description_var;
                                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                        arrTestDetail[iDetail, 5] = SACCode;
                                        //discount
                                        arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                        arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                        arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                        //end
                                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        arrTestDetail[iDetail, 12] = inwdDescr;
                                        iDetail = iDetail + 1;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                }
                        }
                    }
                    # endregion

                    //some code for steel and soil and mf
                    if (RecordType == "ST")
                    {
                        for (int j = RecTypeStartRowDetail; j < iDetail; j++)
                        {
                            if (arrTestDetail[j, 2].Contains("Bend & Rebend") == true)
                                arrTestDetail[j, 2] = arrTestDetail[j, 2].Replace("Bend & Rebend", "Rebend Test");
                            else if (arrTestDetail[j, 2].Contains("Bend") == true)
                                arrTestDetail[j, 2] = arrTestDetail[j, 2].Replace("Bend", "Bend Test");

                            if (arrTestDetail[j, 2].Contains("mm,") == true)
                                arrTestDetail[j, 2] = arrTestDetail[j, 2] + " Reinforcement Steel Bars";
                            else
                                arrTestDetail[j, 2] = arrTestDetail[j, 2] + " Reinforcement Steel Bar";
                        }
                    }
                    else if (RecordType == "SO")
                    {
                        #region So
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
                        if (noOfPits > 0)
                        {
                            tmpAmt = tmpAmt + (pitsRate * noOfPits);
                            inwardAmt = inwardAmt + (pitsRate * noOfPits);
                        }
                        if (noOfCore > 0)
                        {
                            tmpAmt = tmpAmt + (coreRate * noOfCore);
                            inwardAmt = inwardAmt + (coreRate * noOfCore);
                        }
                        for (int j = RecTypeStartRowDetail; j < iDetail; j++)
                        {
                            if (arrTestDetail[j, 4] == "10") //sand -pits
                            {
                                arrTestDetail[j, 3] = noOfPits.ToString();
                            }
                            else if (arrTestDetail[j, 4] == "11") //core
                            {
                                arrTestDetail[j, 3] = noOfCore.ToString();
                            }
                            //tmpAmt = tmpAmt + (Convert.ToInt32(arrTestDetail[j, 1]) * Convert.ToInt32(arrTestDetail[j, 3]));
                        }
                        #endregion 
                    }
                    else if (RecordType == "MF")
                    {
                        if (mfTestMatFlag == true)
                        {
                            //var test = dc.Test(9, "", 0, "MF", "", 0);
                            #region MF
                            var test = dc.Test_View_ClientWise(ClientId, SiteId, 0, 9, "MF");
                            foreach (var t in test)
                            {
                                tmpRate = Convert.ToDecimal(t.TEST_Rate_int);
                                tmpTestId = Convert.ToInt32(t.TEST_Id);
                                //discount
                                tmpAppDiscFlag = false;
                                tmpDiscPer = 0;
                                tmpDiscRate = 0;
                                tmpActualRate = tmpRate;
                                if (t.SITERATE_TestRateFlag == 0)
                                {
                                    tmpDiscPer = genericDiscPer;
                                    tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                    tmpRate = Convert.ToDecimal(tmpDiscRate);
                                    tmpAppDiscFlag = appDiscFlag;
                                }
                                else
                                {
                                    tmpDiscRate = Convert.ToDecimal(t.DiscountedRate);
                                    tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate), 2);
                                    tmpRate = Convert.ToDecimal(tmpDiscRate);
                                }
                                //end
                                tmpQty = 1;
                                //tmpQty = mfTestMatFlagCnt;
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                inwardAmt = inwardAmt + (tmpRate * tmpQty);

                                tmpPart = t.TEST_Name_var;
                                //tmpRate = Convert.ToInt32(t.TEST_Rate_int);
                                //tmpQty = 1;
                                arrTestDetail[iDetail, 0] = tmpPart;
                                arrTestDetail[iDetail, 2] = tmpPart;
                                arrTestDetail[iDetail, 1] = tmpRate.ToString();
                                arrTestDetail[iDetail, 3] = tmpQty.ToString();
                                arrTestDetail[iDetail, 5] = SACCode;
                                //discount
                                arrTestDetail[iDetail, 8] = tmpDiscPer.ToString();
                                arrTestDetail[iDetail, 9] = tmpActualRate.ToString();
                                arrTestDetail[iDetail, 10] = tmpAppDiscFlag.ToString();
                                //end
                                arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                                //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                iDetail = iDetail + 1;
                            }
                            #endregion
                        }
                    }

                    //Other Charges
                    #region

                    if (otherChargesAmt > 0)
                    {
                        tmpRate = otherChargesAmt;
                        tmpTestId = miscTestId;
                        tmpQty = 1;
                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                        inwardAmt = inwardAmt + (tmpRate * tmpQty);

                        tmpPart = "Other Charges";
                        //tmpRate = otherChargesAmt;
                        //tmpQty = 1;
                        //tmpSrNo = 0;
                        arrTestDetail[iDetail, 0] = tmpPart;
                        arrTestDetail[iDetail, 2] = tmpPart;
                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                        arrTestDetail[iDetail, 3] = "1";
                        arrTestDetail[iDetail, 5] = SACCode;
                        //discount
                        arrTestDetail[iDetail, 8] = "0";
                        arrTestDetail[iDetail, 9] = tmpRate.ToString();
                        //end
                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                        //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                        iDetail = iDetail + 1;
                    }
                    //Other charges for NDT, Core, SO, GT, Other
                    var re = dc.OtherCharges_View(RecordType, RecordNo);
                    foreach (var r in re)
                    {
                        tmpTestId = miscTestId;
                        tmpPart = r.OTHERCHRG_Description_var;
                        tmpRate = Convert.ToDecimal(r.OTHERCHRG_Rate_num);
                        tmpQty = Convert.ToInt32(r.OTHERCHRG_Quantity_int);
                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                        inwardAmt = inwardAmt + (tmpRate * tmpQty);

                        arrTestDetail[iDetail, 0] = tmpPart;
                        arrTestDetail[iDetail, 2] = tmpPart;
                        arrTestDetail[iDetail, 1] = tmpRate.ToString();
                        arrTestDetail[iDetail, 3] = tmpQty.ToString();
                        arrTestDetail[iDetail, 5] = SACCode;
                        //discount
                        arrTestDetail[iDetail, 8] = "0";
                        arrTestDetail[iDetail, 9] = tmpRate.ToString();
                        //end
                        arrTestDetail[iDetail, 11] = tmpTestId.ToString();
                        iDetail++;
                    }
                    //
                    for (int j = RecTypeStartRowDetail; j < iDetail; j++)
                    {
                        arrTestDetail[j, 6] = recN.INWD_ReferenceNo_int.ToString();
                        arrTestDetail[j, 7] = Convert.ToDateTime(recN.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy");
                    }
                    arrTest[i, 0] = recN.MATERIAL_Name_var;
                    arrTest[i, 2] = recN.MATERIAL_Name_var;
                    if (recN.INWD_RecordType_var.ToString() == "OT")
                    {
                        arrTest[i, 0] = tmpPartOT;
                        arrTest[i, 2] = tmpPartOT;
                    }
                    tmpQty = 1;
                    tmpRate = inwardAmt;
                    arrTest[i, 1] = tmpRate.ToString();
                    arrTest[i, 3] = tmpQty.ToString();
                    arrTest[i, 5] = SACCode;
                    arrTest[i, 6] = recN.INWD_ReferenceNo_int.ToString();
                    arrTest[i, 7] = Convert.ToDateTime(recN.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy");
                    //discount
                    arrTest[i, 8] = "0";
                    arrTest[i, 9] = tmpRate.ToString();
                    //end                    
                    i = i + 1;
                }
            }
            #endregion
            //Narration 
            for (int j = 0; j < i; j++)
            {
                if (TallyNarration == "")
                {
                    TallyNarration = arrTest[j, 2];
                }
                else
                {
                    TallyNarration = TallyNarration + ", " + arrTest[j, 2];
                }
            }
            TallyNarration = TallyNarration.ToUpper();
            //Calculation
            xGrossAmt = tmpAmt;
            if (xDisc > 0)
                xDiscAmt = xGrossAmt * xDisc;
            if (xSrvTax > 0)
                xSrvTaxAmt = (xGrossAmt - xDiscAmt) * xSrvTax;
            xSrvTaxAmt = Decimal.Round(xSrvTaxAmt, 2);
            if (xSwTax > 0)
                xSwTaxAmt = (xGrossAmt - xDiscAmt) * xSwTax;
            xSwTaxAmt = Decimal.Round(xSwTaxAmt, 2);
            if (xKkTax > 0)
                xKkTaxAmt = (xGrossAmt - xDiscAmt) * xKkTax;
            xKkTaxAmt = Decimal.Round(xKkTaxAmt, 2);
            //edCess = xSrvTaxAmt * Convert.ToDecimal(0.02);
            //edCess = Decimal.Round(edCess, 2);
            //highEdCess = xSrvTaxAmt * Convert.ToDecimal(0.01);
            //highEdCess = Decimal.Round(highEdCess, 2);
            if (xCgst > 0)
                xCgstAmt = (xGrossAmt - xDiscAmt) * xCgst;
            xCgstAmt = Decimal.Round(xCgstAmt, 2);
            if (xSgst > 0)
                xSgstAmt = (xGrossAmt - xDiscAmt) * xSgst;
            xSgstAmt = Decimal.Round(xSgstAmt, 2);
            if (xIgst > 0)
                xIgstAmt = (xGrossAmt - xDiscAmt) * xIgst;
            xIgstAmt = Decimal.Round(xIgstAmt, 2);

            xNetAmt = xGrossAmt - xDiscAmt + xSrvTaxAmt + xSwTaxAmt + xKkTaxAmt + edCess + highEdCess + xCgstAmt + xSgstAmt + xIgstAmt;

            xNetAmt = Decimal.Round(xNetAmt, 0);
            roundOff = Decimal.Round((xNetAmt - (xGrossAmt - xDiscAmt + xSrvTaxAmt + xSwTaxAmt + xKkTaxAmt + edCess + highEdCess + xCgstAmt + xSgstAmt + xIgstAmt)), 2);
            //xNetAmt = Decimal.Round(xNetAmt, 2);
            xSrvTax = xSrvTax * 100;
            xSwTax = xSwTax * 100;
            xKkTax = xKkTax * 100;
            xCgst = xCgst * 100;
            xSgst = xSgst * 100;
            xIgst = xIgst * 100;
            xDisc = xDisc * 100;
            bool billPrintLockStatus = false;
            bool insertBill = false;
            //string BillNo = "0";
            if (BillNo == "0")
            {
                //var client = dc.Client_View(ClientId, 0, "", "");
                //foreach (var cl in client)
                //{
                //    billPrintLockStatus = Convert.ToBoolean(cl.CL_MonthlyBilling_bit);
                //}
                var site = dc.Site_View(SiteId, 0, 0, "");
                foreach (var st in site)
                {
                    billPrintLockStatus = Convert.ToBoolean(st.SITE_MonthlyBillingStatus_bit);
                }
                int NewrecNo = 0;
                clsData clsObj = new clsData();
                NewrecNo = clsObj.GetnUpdateRecordNo("BillNo");
                var master = dc.MasterSetting_View(0);
                foreach (var mst in master)
                {
                    //BillNo = mst.MASTER_AccountingYear_var + "/" + mst.MASTER_Region_var + "/" +  NewrecNo.ToString();
                    BillNo = mst.MASTER_AccountingYear_var + mst.MASTER_Region_var + NewrecNo.ToString();
                }
                insertBill = true;
            }
            dc.BillDetail_Update(BillNo, 0, 0, 0, "", 0, "", 0, 0, false, 0, true);
            dc.BillDetailMonthly_Update(BillNo, 0, 0, 0, "", 0, "", 0, null, "", 0, 0, false, 0, true, "");
            //BillNo = 
            dc.Bill_Update(BillNo, ClientId, ClientName, SiteId, discPerFlag, xDisc, xDiscAmt, xSrvTax,
            xSrvTaxAmt, xSwTax, xSwTaxAmt, xKkTax, xKkTaxAmt, xCgst, xCgstAmt, xSgst, xSgstAmt, xIgst,
            xIgstAmt, 0, xNetAmt, edCess, highEdCess, roundOff, "Monthly", 0, false, TallyNarration,
            false, Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), false, "", billPrintLockStatus, "", insertBill);

            //string SACCode = "";
            //if (RecordType == "SO" || RecordType == "GT")
            //    SACCode = "998341";
            //else
            //    SACCode = "998346";

            for (int j = 0; j < i; j++)
            {
                tmpAmt = Convert.ToDecimal(arrTest[j, 1]) * Convert.ToDecimal(arrTest[j, 3]);
                DateTime receivedDate = DateTime.ParseExact(arrTest[j, 7], "dd/MM/yyyy", null);
                dc.BillDetail_Update_Monthly(BillNo, j + 1, Convert.ToInt32(arrTest[j, 3]), tmpAmt, arrTest[j, 2], Convert.ToDecimal(arrTest[j, 1]), arrTest[j, 5], Convert.ToInt32(arrTest[j, 6]), receivedDate, billingPeriod, Convert.ToDecimal(arrTest[j, 9]), Convert.ToDecimal(arrTest[j, 8]), false);
            }
            for (int j = 0; j < iDetail; j++)
            {
                tmpAmt = Convert.ToDecimal(arrTestDetail[j, 1]) * Convert.ToDecimal(arrTestDetail[j, 3]);
                DateTime receivedDate = DateTime.ParseExact(arrTestDetail[j, 7], "dd/MM/yyyy", null);
                tmpAppDiscFlag = false;
                if (arrTestDetail[j, 10] != null && arrTestDetail[j, 10].ToString() != "")
                    tmpAppDiscFlag = Convert.ToBoolean(arrTestDetail[j, 10]);
                if (arrTestDetail[j, 11] == "" || arrTestDetail[j, 11] == null)
                    arrTestDetail[j, 11] = "0";
                if (arrTestDetail[j, 12] == null)
                    arrTestDetail[j, 12] = "";
                dc.BillDetailMonthly_Update(BillNo, j + 1, Convert.ToInt32(arrTestDetail[j, 3]), tmpAmt, arrTestDetail[j, 2], Convert.ToDecimal(arrTestDetail[j, 1]), arrTestDetail[j, 5], Convert.ToInt32(arrTestDetail[j, 6]), receivedDate, billingPeriod, Convert.ToDecimal(arrTestDetail[j, 9]), Convert.ToDecimal(arrTestDetail[j, 8]), tmpAppDiscFlag, Convert.ToInt32(arrTestDetail[j, 11]), false, arrTestDetail[j, 12].ToString());
            }
            //dc.Inward_Update_BillNo(RecordNo, RecordType, BillNo);
            var allinward = dc.Inward_View_Monthly(ClientId, SiteId, Fromdate, Todate, false, "");
            foreach (var allinwd in allinward)
            {
                dc.Inward_Update_MonthlyBillStatus(allinwd.INWD_RecordNo_int, allinwd.INWD_RecordType_var, true);
            }
            return BillNo;

        }

        private string getDiscount(int clId, int siteId, string recType, int enqId)
        {
            //decimal totDisc = 0, totDiscA = 0, totDiscB = 0, introDiscA = 0, volDiscB = 0,
            //    timelyDiscC = 0, AdvDiscD = 0, loyDiscE = 0, propDiscF = 0, AppDiscG = 0,
            decimal maxDisc = 0, appliedDisc = 0;
            bool enqFromMobApp = false;

            //get enquiry status whether it is from mob or not
            var enq = dc.Enquiry_View(enqId, -1, 0);
            foreach (var item in enq)
            {
                if (Convert.ToString(item.ENQ_MobileAppEnqNo_int) != null || Convert.ToString(item.ENQ_MobileAppEnqNo_int) != "")
                {
                    if (Convert.ToInt32(item.ENQ_MobileAppEnqNo_int) > 0)
                        enqFromMobApp = true;
                }
            }

            //get max discount
            var maxDiscDetails = dc.Material_View(recType, "");
            foreach (var max in maxDiscDetails)
            {
                maxDisc = Convert.ToInt32(max.MATERIAL_MaxDiscount_int);
                break;
            }
            //
            var clDisc = dc.DiscountSetting_View(clId, "");//view all discount of that client
            foreach (var item in clDisc)
            {
                //introDiscA = Convert.ToDecimal(item.DISCOUNT_Introductory_num.ToString());
                //volDiscB = Convert.ToDecimal(item.DISCOUNT_Volume_num.ToString());
                //timelyDiscC = Convert.ToDecimal(item.DISCOUNT_TimelyPayment_num.ToString());
                //AdvDiscD = Convert.ToDecimal(item.DISCOUNT_Advance_num.ToString());
                //loyDiscE = Convert.ToDecimal(item.DISCOUNT_Loyalty_num.ToString());
                //propDiscF = Convert.ToDecimal(item.DISCOUNT_Proposal_num.ToString());
                //AppDiscG = Convert.ToDecimal(item.DISCOUNT_App_num.ToString());

                appliedDisc = Convert.ToDecimal(item.Applicable.ToString());
            }

            if (appliedDisc > maxDisc)
                appliedDisc = maxDisc;

            //if (dc.Connection.Database.ToLower().ToString() == "veenalive")
            //{
            //    //for ST  if applied dis<30 then set it to bydefault 30
            //    if (recType == "ST")
            //    {
            //        if (appliedDisc < 30)
            //            appliedDisc = 30;
            //    }
            //}

            if (enqFromMobApp)
                appliedDisc = appliedDisc + 5;//if mob enq then add 5% discount

            return appliedDisc + "|" + enqFromMobApp.ToString();

            //totDiscA = introDiscA + AdvDiscD + propDiscF + AppDiscG;
            //totDiscB = volDiscB + timelyDiscC + loyDiscE + propDiscF + AppDiscG;
            //totDisc = totDiscA;
            //if (introDiscA > 0)//new client
            //{
            //    if (totDiscB > totDiscA)
            //        totDisc = totDiscB;
            //}
            //else//old client
            //    totDisc = totDiscB;
            //if (totDisc > maxDisc)
            //    totDisc = maxDisc;
            //return totDisc;
        }

        public string UpdateBill_Old(int ClientId, int SiteId, DateTime Fromdate, DateTime Todate, string billingPeriod)
        {
            string ClientName = "", TallyNarration = ""; //, tmpPart = "";
            int i = 0, tmpRate = 0, tmpQty = 0, tmpAmt = 0, inwardAmt = 0, tmpSrNo = 0, noOfPits = 0, noOfCore = 0, otherChargesAmt = 0, mfTestMatFlagCnt = 0;
            decimal xGrossAmt = 0, xNetAmt = 0, xDisc = 0, xDiscAmt = 0, xSrvTax = 0, xSrvTaxAmt = 0, xSwTax = 0, xSwTaxAmt = 0
                , xKkTax = 0, xKkTaxAmt = 0, edCess = 0, highEdCess = 0, roundOff = 0, xCgst = 0, xCgstAmt = 0, xSgst = 0, xSgstAmt = 0, xIgst = 0, xIgstAmt = 0;
            int pitsRate = 0, coreRate = 0;
            string[,] arrTest = new string[100, 8];
            bool discPerFlag = false, mfTestMatFlag = false, mfTestMatFlagChange = false; //foundFlag = false,
            //bool NDTByUPVFlag = false;
            //bool MinBillFlag = false;

            //ClientName = inwd.CL_Name_var;
            //tmpQty = Convert.ToInt32(inwd.INWD_TotalQty_int);
            //xDisc = dc.Discount_View(ClientId, SiteId, RecordType);
            //xDisc = xDisc / 100;
            //if (xDisc > 0)
            //{
            //    discPerFlag = true;
            //}
            DateTime BillDate = DateTime.Now;
            if (CheckGSTFlag(BillDate) == false)
            {
                var masterSrvTax = dc.MasterSetting_View(SiteId);
                foreach (var serTax in masterSrvTax)
                {
                    xSrvTax = Convert.ToDecimal(serTax.MASTER_ServiceTax_num);
                }
                if (xSrvTax > 0)
                {
                    xSrvTax = xSrvTax / 100;
                }
                if (xSrvTax > 0)
                {
                    var master = dc.MasterSetting_View(0);
                    foreach (var mst in master)
                    {
                        xSwTax = Convert.ToDecimal(mst.MASTER_SwachhBharatTax_num);
                        xKkTax = Convert.ToDecimal(mst.MASTER_KisanKrishiTax_num);
                    }
                }
                if (xSwTax > 0)
                {
                    xSwTax = xSwTax / 100;
                }
                if (xKkTax > 0)
                {
                    xKkTax = xKkTax / 100;
                }
            }
            else
            {
                bool cgstFlag = false, sgstFlag = false, igstFlag = false;
                var site = dc.Site_View(SiteId, 0, 0, "");
                foreach (var st in site)
                {
                    if (st.SITE_SEZStatus_bit == false && st.SITE_State_var.ToLower() == "maharashtra")
                    {
                        cgstFlag = true;
                        sgstFlag = true;
                    }
                    else
                    {
                        igstFlag = true;
                    }
                }

                //decimal xCGST = 0, xSGST = 0, xIGST = 0;
                bool siteGSTFlag = false;
                var siteGst = dc.GST_Site_View(SiteId);
                foreach (var GSTax in siteGst)
                {
                    if (cgstFlag == true)
                        xCgst = Convert.ToDecimal(GSTax.GSTSITE_CGST_dec);
                    if (sgstFlag == true)
                        xSgst = Convert.ToDecimal(GSTax.GSTSITE_SGST_dec);
                    if (igstFlag == true)
                        xIgst = Convert.ToDecimal(GSTax.GSTSITE_IGST_dec);
                    siteGSTFlag = true;
                    break;
                }
                if (siteGSTFlag == false)
                {
                    var master = dc.GST_View(1, BillDate);
                    foreach (var GSTax in master)
                    {
                        if (cgstFlag == true)
                            xCgst = Convert.ToDecimal(GSTax.GST_CGST_dec);
                        if (sgstFlag == true)
                            xSgst = Convert.ToDecimal(GSTax.GST_SGST_dec);
                        if (igstFlag == true)
                            xIgst = Convert.ToDecimal(GSTax.GST_IGST_dec);
                        break;
                    }
                }
                if (xCgst > 0)
                {
                    xCgst = xCgst / 100;
                }
                if (xSgst > 0)
                {
                    xSgst = xSgst / 100;
                }
                if (xIgst > 0)
                {
                    xIgst = xIgst / 100;
                }
            }

            string RecordType = ""; int RecordNo = 0, RecTypeStartRow = 0;

            var RecType = dc.Inward_View_Monthly(ClientId, SiteId, Fromdate, Todate, true, "");
            foreach (var recT in RecType)
            {
                RecTypeStartRow = i;
                tmpQty = 0;
                RecordType = recT.INWD_RecordType_var;
                string SACCode = "";
                if (RecordType == "SO" || RecordType == "GT")
                    SACCode = "998341";
                else
                    SACCode = "998346";
                var RecNo = dc.Inward_View_Monthly(ClientId, SiteId, Fromdate, Todate, false, RecordType);
                foreach (var recN in RecNo)
                {
                    RecordNo = Convert.ToInt32(recN.INWD_RecordNo_int);
                    mfTestMatFlagChange = false;
                    int RecNoStartRow = 0;
                    inwardAmt = 0;
                    #region calculate record no gross
                    var inward = dc.Inward_Test_View(RecordType, RecordNo, "").ToList();
                    foreach (var inwd in inward)
                    {
                        if (int.TryParse(inwd.INWD_Charges_var, out otherChargesAmt) == true && RecNoStartRow == 0)
                        {
                            otherChargesAmt = Convert.ToInt32(inwd.INWD_Charges_var);
                        }
                        switch (RecordType)
                        {
                            case "AAC":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.AACINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "AGGT":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = 1;
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "BT-":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    tmpQty = 1;
                                    if (tmpSrNo == 3) //Dimension Analysis
                                        tmpQty = 1;
                                    else if (tmpSrNo == 5) //Density
                                        tmpQty = Convert.ToInt32(inwd.BTTEST_Quantity_tint);
                                    else
                                        tmpQty = 5;
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "CCH":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = 1;
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "CEMT":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = 1;
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    if (tmpSrNo != 5)
                                    {
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    }
                                    break;
                                }
                            case "CR":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.CRINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "CORECUT":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.CORECUTINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "CT":
                                {
                                    if (RecNoStartRow == 0)
                                    {
                                        tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                        tmpQty = Convert.ToInt32(inwd.INWD_TotalQty_int);
                                        if (inwd.CTINWD_TestType_var.Contains("Accelerated Curing") == true)
                                        {
                                            tmpQty = 1;
                                        }
                                        //else if (tmpQty < 3)
                                        //{
                                        //    tmpRate = 3 * tmpRate;
                                        //    arrTest[i, 2] = arrTest[i, 2] + " (Minimum Billing)";
                                        //    tmpQty = 1;
                                        //}
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                        RecNoStartRow++;
                                    }
                                    //if (RecNoStartRow == 0)
                                    //{
                                    //    if (i > RecTypeStartRow)
                                    //        i--;
                                    //    tmpPart = inwd.TEST_Name_var;
                                    //    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    //    //tmpQty = 1;
                                    //    tmpQty += Convert.ToInt32(inwd.INWD_TotalQty_int);
                                    //    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);

                                    //    arrTest[RecTypeStartRow, 0] = tmpPart;
                                    //    arrTest[RecTypeStartRow, 2] = tmpPart;
                                    //    if (inwd.CTINWD_TestType_var.Contains("Accelerated Curing") == true)
                                    //    {
                                    //        tmpQty += 1;
                                    //    }
                                    //    //else if (tmpQty < 3)
                                    //    //{
                                    //    //    tmpRate = 3 * tmpRate;
                                    //    //    arrTest[i, 2] = arrTest[i, 2] + " (Minimum Billing)";
                                    //    //    tmpQty = 1;
                                    //    //}

                                    //    arrTest[RecTypeStartRow, 1] = tmpRate.ToString();
                                    //    arrTest[RecTypeStartRow, 3] = tmpQty.ToString();
                                    //    arrTest[RecTypeStartRow, 5] = SACCode;
                                    //    //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    //    tmpAmt = tmpRate * tmpQty;
                                    //    i = i + 1;
                                    //    RecNoStartRow++;
                                    //}
                                    break;
                                }
                            case "FLYASH":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = 1;
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                    if (tmpSrNo != 3)
                                    {
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    }
                                    break;
                                }
                            case "MF":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = 1;
                                    if (inwd.MFINWD_TestMaterial_bit == true)
                                    {
                                        mfTestMatFlag = true;
                                        if (mfTestMatFlagChange == false)
                                        {
                                            mfTestMatFlagCnt++;
                                            mfTestMatFlagChange = true;
                                        }
                                    }
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "NDT":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.NDTTEST_Points_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "OT":
                                {
                                    tmpRate = Convert.ToInt32(inwd.OTRATEIN_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.OTRATEIN_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "PT":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.PTINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "PILE":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.PILEINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "GT":
                            case "RWH":
                                {
                                    tmpRate = Convert.ToInt32(inwd.GTTEST_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.GTTEST_Quantity_tint);
                                    if (inwd.GTINW_LumpSump_tint == 1)
                                    {
                                        tmpAmt = tmpAmt + tmpRate;
                                        inwardAmt = inwardAmt + tmpRate;
                                    }
                                    else
                                    {
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    }
                                    break;
                                }
                            case "SO":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = 1;
                                    tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);

                                    if (tmpSrNo == 10) //sand
                                    {
                                        tmpQty = Convert.ToInt32(inwd.SOINWD_Pits_tint);
                                        noOfPits = noOfPits + tmpQty;
                                        pitsRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    }
                                    else if (tmpSrNo == 11) //core
                                    {
                                        tmpQty = Convert.ToInt32(inwd.SOINWD_Cores_tint);
                                        noOfCore = noOfCore + tmpQty;
                                        coreRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    }
                                    else if (tmpSrNo == 12)//classification
                                    {
                                        tmpQty = Convert.ToInt32(inwd.SOINWD_Quantity_tint);
                                    }
                                    if (tmpSrNo != 10 && tmpSrNo != 11)
                                    {
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    }
                                    break;
                                }

                            case "SOLID":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.SOLIDINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "ST":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.STINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "STC":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.STCINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "TILE":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.TILEINWD_Quantity_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                            case "WT":
                                {
                                    tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);
                                    tmpQty = Convert.ToInt32(inwd.WTTEST_Qty_tint);
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    inwardAmt = inwardAmt + (tmpRate * tmpQty);
                                    break;
                                }
                        }
                    }
                    # endregion

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
                        if (noOfPits > 0)
                        {
                            tmpAmt = tmpAmt + (pitsRate * noOfPits);
                            inwardAmt = inwardAmt + (pitsRate * noOfPits);
                        }
                        if (noOfCore > 0)
                        {
                            tmpAmt = tmpAmt + (coreRate * noOfCore);
                            inwardAmt = inwardAmt + (coreRate * noOfCore);
                        }
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
                                tmpQty = mfTestMatFlagCnt;
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                inwardAmt = inwardAmt + (tmpRate * tmpQty);
                            }
                        }
                    }
                    //Other Charges
                    if (otherChargesAmt > 0)
                    {
                        tmpRate = otherChargesAmt;
                        tmpQty = 1;
                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                        inwardAmt = inwardAmt + (tmpRate * tmpQty);
                    }

                    arrTest[i, 0] = recN.MATERIAL_Name_var;
                    arrTest[i, 2] = recN.MATERIAL_Name_var;
                    tmpQty = 1;
                    tmpRate = inwardAmt;
                    arrTest[i, 1] = tmpRate.ToString();
                    arrTest[i, 3] = tmpQty.ToString();
                    arrTest[i, 5] = SACCode;
                    arrTest[i, 6] = recN.INWD_ReferenceNo_int.ToString();
                    arrTest[i, 7] = Convert.ToDateTime(recN.INWD_ReceivedDate_dt).ToString("dd/MM/yyyy");
                    i = i + 1;
                }
            }

            //Narration 
            for (int j = 0; j < i; j++)
            {
                if (TallyNarration == "")
                {
                    TallyNarration = arrTest[j, 2];
                }
                else
                {
                    TallyNarration = TallyNarration + ", " + arrTest[j, 2];
                }
            }
            TallyNarration = TallyNarration.ToUpper();
            //Calculation
            xGrossAmt = tmpAmt;
            if (xDisc > 0)
                xDiscAmt = xGrossAmt * xDisc;
            if (xSrvTax > 0)
                xSrvTaxAmt = (xGrossAmt - xDiscAmt) * xSrvTax;
            xSrvTaxAmt = Decimal.Round(xSrvTaxAmt, 2);
            if (xSwTax > 0)
                xSwTaxAmt = (xGrossAmt - xDiscAmt) * xSwTax;
            xSwTaxAmt = Decimal.Round(xSwTaxAmt, 2);
            if (xKkTax > 0)
                xKkTaxAmt = (xGrossAmt - xDiscAmt) * xKkTax;
            xKkTaxAmt = Decimal.Round(xKkTaxAmt, 2);
            //edCess = xSrvTaxAmt * Convert.ToDecimal(0.02);
            //edCess = Decimal.Round(edCess, 2);
            //highEdCess = xSrvTaxAmt * Convert.ToDecimal(0.01);
            //highEdCess = Decimal.Round(highEdCess, 2);
            if (xCgst > 0)
                xCgstAmt = (xGrossAmt - xDiscAmt) * xCgst;
            xCgstAmt = Decimal.Round(xCgstAmt, 2);
            if (xSgst > 0)
                xSgstAmt = (xGrossAmt - xDiscAmt) * xSgst;
            xSgstAmt = Decimal.Round(xSgstAmt, 2);
            if (xIgst > 0)
                xIgstAmt = (xGrossAmt - xDiscAmt) * xIgst;
            xIgstAmt = Decimal.Round(xIgstAmt, 2);

            xNetAmt = xGrossAmt - xDiscAmt + xSrvTaxAmt + xSwTaxAmt + xKkTaxAmt + edCess + highEdCess + xCgstAmt + xSgstAmt + xIgstAmt;

            xNetAmt = Decimal.Round(xNetAmt, 0);
            roundOff = Decimal.Round((xNetAmt - (xGrossAmt - xDiscAmt + xSrvTaxAmt + xSwTaxAmt + xKkTaxAmt + edCess + highEdCess + xCgstAmt + xSgstAmt + xIgstAmt)), 2);
            //xNetAmt = Decimal.Round(xNetAmt, 2);
            xSrvTax = xSrvTax * 100;
            xSwTax = xSwTax * 100;
            xKkTax = xKkTax * 100;
            xCgst = xCgst * 100;
            xSgst = xSgst * 100;
            xIgst = xIgst * 100;
            xDisc = xDisc * 100;
            bool billPrintLockStatus = false;
            bool insertBill = false;
            string BillNo = "0";
            if (BillNo == "0")
            {
                //var client = dc.Client_View(ClientId, 0, "", "");
                //foreach (var cl in client)
                //{
                //    billPrintLockStatus = Convert.ToBoolean(cl.CL_MonthlyBilling_bit);
                //}
                var site = dc.Site_View(SiteId, 0, 0, "");
                foreach (var st in site)
                {
                    billPrintLockStatus = Convert.ToBoolean(st.SITE_MonthlyBillingStatus_bit);
                }
                int NewrecNo = 0;
                clsData clsObj = new clsData();
                NewrecNo = clsObj.GetnUpdateRecordNo("BillNo");
                var master = dc.MasterSetting_View(0);
                foreach (var mst in master)
                {
                    //BillNo = mst.MASTER_AccountingYear_var + "/" + mst.MASTER_Region_var + "/" +  NewrecNo.ToString();
                    BillNo = mst.MASTER_AccountingYear_var + mst.MASTER_Region_var + NewrecNo.ToString();
                }
                insertBill = true;
            }
            dc.BillDetail_Update(BillNo, 0, 0, 0, "", 0, "", 0, 0, false, 0, true);
            //BillNo = 
            dc.Bill_Update(BillNo, ClientId, ClientName, SiteId, discPerFlag, xDisc, xDiscAmt, xSrvTax,
            xSrvTaxAmt, xSwTax, xSwTaxAmt, xKkTax, xKkTaxAmt, xCgst, xCgstAmt, xSgst, xSgstAmt, xIgst, xIgstAmt, 0, xNetAmt, edCess, highEdCess, roundOff, "Monthly", 0, false, TallyNarration,
            false, Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), false, "", billPrintLockStatus, "", insertBill);

            //string SACCode = "";
            //if (RecordType == "SO" || RecordType == "GT")
            //    SACCode = "998341";
            //else
            //    SACCode = "998346";

            for (int j = 0; j < i; j++)
            {
                tmpAmt = Convert.ToInt32(arrTest[j, 1]) * Convert.ToInt32(arrTest[j, 3]);
                DateTime receivedDate = DateTime.ParseExact(arrTest[j, 7], "dd/MM/yyyy", null);
                dc.BillDetail_Update_Monthly(BillNo, j + 1, Convert.ToInt32(arrTest[j, 3]), tmpAmt, arrTest[j, 2], Convert.ToInt32(arrTest[j, 1]), arrTest[j, 5], Convert.ToInt32(arrTest[j, 6]), receivedDate, billingPeriod, 0, 0, false);
            }
            //dc.Inward_Update_BillNo(RecordNo, RecordType, BillNo);
            var allinward = dc.Inward_View_Monthly(ClientId, SiteId, Fromdate, Todate, false, "");
            foreach (var allinwd in allinward)
            {
                dc.Inward_Update_MonthlyBillStatus(allinwd.INWD_RecordNo_int, allinwd.INWD_RecordType_var, true);
            }
            return BillNo;

        }

        public static string CnvtAmttoWords(int number)
        {
            if (number == 0)
                return "Zero";
            if (number < 0)
                return "minus " + CnvtAmttoWords(Math.Abs(number));
            string words = "";
            if ((number / 10000000) > 0)
            {
                words += CnvtAmttoWords(number / 10000000) + " Crore ";
                number %= 10000000;
            }
            if ((number / 100000) > 0)
            {
                words += CnvtAmttoWords(number / 100000) + " Lakh ";
                number %= 100000;
            }
            if ((number / 1000) > 0)
            {
                words += CnvtAmttoWords(number / 1000) + " Thousand ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += CnvtAmttoWords(number / 100) + " Hundred ";
                number %= 100;
            }
            if (number > 0)
            {
                if (words != "")
                    words += "and ";
                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }

        public bool CheckGSTFlag(DateTime BillDate)
        {
            bool gstFlag = false;
            //string[] strDate = txtDate.Text.Split('/');
            //DateTime BillDate = new DateTime(Convert.ToInt32(strDate[2]), Convert.ToInt32(strDate[1]), Convert.ToInt32(strDate[0]));
            var master = dc.GST_View(1, BillDate);
            if (master.Count() > 0)
            {
                gstFlag = true;
            }
            else
            {
                gstFlag = false;
            }
            return gstFlag;
        }
    }
}