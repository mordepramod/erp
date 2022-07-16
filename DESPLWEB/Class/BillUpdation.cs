﻿using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.IO;

namespace DESPLWEB
{
    public class BillUpdation
    {
        LabDataDataContext dc = new LabDataDataContext();
        public string UpdateBill(string RecordType, int RecordNo, string BillNo)
        {
            //System.Web.HttpContext.Current.Session["LoginId"] = "1";

            string ClientName = "", TallyNarration = "", tmpPart = "";
            int i = 0, ClientId = 0, SiteId = 0, tmpQty = 0, tmpSrNo = 0, noOfPits = 0, noOfCore = 0, otherChargesAmt = 0, tmpTestId = 0, miscTestId = 0;
            decimal tmpRate = 0, tmpAmt = 0;
            decimal xGrossAmt = 0, xNetAmt = 0, xDisc = 0, xDiscAmt = 0, xSrvTax = 0, xSrvTaxAmt = 0, xSwTax = 0, xSwTaxAmt = 0
                , xKkTax = 0, xKkTaxAmt = 0, edCess = 0, highEdCess = 0, roundOff = 0, xCgst = 0, xCgstAmt = 0, xSgst = 0, xSgstAmt = 0, xIgst = 0, xIgstAmt = 0;
            string[,] arrTest = new string[100, 9];
            bool foundFlag = false, discPerFlag = false, mfTestMatFlag = false;
            bool NDTByUPVFlag = false;
            bool MinBillFlag = false;
            //discount
            //int tmpActualRate = 0, prevRate =0;
            decimal tmpActualRate = 0, prevRate = 0;
            decimal genericDiscPer = 0, tmpDiscRate = 0, tmpDiscPer = 0;
            bool appDiscFlag = false, tmpAppDiscFlag = false;
           // string tmpPartOT = "";
            //
            var inward = dc.Inward_Test_View(RecordType, RecordNo, "").ToList();
            foreach (var inwd in inward)
            {
                if (ClientId == 0)
                {
                    ClientId = Convert.ToInt32(inwd.INWD_CL_Id);
                    SiteId = Convert.ToInt32(inwd.INWD_SITE_Id);
                    ClientName = inwd.CL_Name_var;
                    tmpQty = Convert.ToInt32(inwd.INWD_TotalQty_int);
                    //**commented for generic discount
                    //xDisc = dc.Discount_View(inwd.INWD_CL_Id, inwd.INWD_SITE_Id, RecordType);
                    //xDisc = xDisc / 100;
                    //if (xDisc > 0)
                    //{
                    //    discPerFlag = true;
                    //}
                    //**
                    DateTime BillDate = DateTime.Now;
                    //if (BillDate < DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null))
                    if (CheckGSTFlag(BillDate) == false)
                    {
                        var masterSrvTax = dc.MasterSetting_View(inwd.INWD_SITE_Id);
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
                    //get generic discount for client
                    string[] strSplitStr = getDiscount(Convert.ToInt32(inwd.INWD_CL_Id), Convert.ToInt32(inwd.INWD_SITE_Id), RecordType, Convert.ToInt32(inwd.INWD_ENQ_Id)).Split('|');
                    genericDiscPer = Convert.ToDecimal(strSplitStr[0]);
                    appDiscFlag = Convert.ToBoolean(strSplitStr[1]);
                    //
                }
                var otherTest = dc.Test_View(0, 0, "MS", 0, 0, 0).ToList();
                if (otherTest.Count() > 0)
                {
                    miscTestId = Convert.ToInt32(otherTest.FirstOrDefault().TEST_Id);
                }
                if (int.TryParse(inwd.INWD_Charges_var, out otherChargesAmt) == true)
                {
                    otherChargesAmt = Convert.ToInt32(inwd.INWD_Charges_var);
                }
                switch (RecordType)
                {
                    case "AAC":
                        {
                            #region AAC
                            tmpPart = inwd.TEST_Name_var;
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
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            foundFlag = false;
                            for (int j = 0; j <= i; j++)
                            {
                                //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                {
                                    arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + tmpQty).ToString();
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    foundFlag = true;
                                }
                            }
                            if (foundFlag == false)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "AGGT":
                        {
                            #region aggt
                            tmpPart = inwd.TEST_Name_var;
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
                                tmpDiscPer = 100 - Math.Round(((tmpDiscRate * 100) / tmpActualRate),2); 
                                tmpRate = Convert.ToDecimal(tmpDiscRate);
                            }
                            //end
                            tmpQty = 1;
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            foundFlag = false;
                            for (int j = 0; j <= i; j++)
                            {
                                //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                {
                                    arrTest[j, 2] = arrTest[j, 2] + ", " + inwd.AGGTINWD_AggregateName_var;
                                    arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + 1).ToString();
                                    tmpQty = 1;
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    foundFlag = true;
                                }
                            }
                            if (foundFlag == false)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart + " of " + inwd.AGGTINWD_AggregateName_var;
                                tmpQty = 1;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "BT-":
                        {
                            #region bt-
                            tmpPart = inwd.TEST_Name_var;
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
                            foundFlag = false;
                            for (int j = 0; j <= i; j++)
                            {
                                //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                {
                                    if (arrTest[j, 2].Contains(inwd.BTINWD_BrickType_var) == false)
                                        arrTest[j, 2] = arrTest[j, 2] + ", " + inwd.BTINWD_BrickType_var;

                                    if (tmpSrNo == 3) //Dimension Analysis
                                        tmpQty = 1;
                                    else if (tmpSrNo == 5) //Density
                                        tmpQty = Convert.ToInt32(inwd.BTTEST_Quantity_tint);
                                    else
                                        tmpQty = 5;

                                    arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + tmpQty).ToString();

                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    foundFlag = true;
                                }
                            }
                            if (foundFlag == false)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart + " of " + inwd.BTINWD_BrickType_var;
                                if (tmpSrNo == 3) //Dimension Analysis
                                    tmpQty = 1;
                                else if (tmpSrNo == 5) //Density
                                    tmpQty = Convert.ToInt32(inwd.BTTEST_Quantity_tint);
                                else
                                    tmpQty = 5;

                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "CCH":
                        {
                            #region cch
                            tmpPart = inwd.TEST_Name_var;
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
                            foundFlag = false;
                            for (int j = 0; j <= i; j++)
                            {
                                //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                {
                                    arrTest[j, 2] = arrTest[j, 2] + ", " + inwd.CCHINWD_CementName_var;
                                    arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + 1).ToString();
                                    tmpQty = 1;

                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    foundFlag = true;
                                }
                            }
                            if (foundFlag == false)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart + " of " + inwd.CCHINWD_CementName_var;
                                tmpQty = 1;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "GGBSCH":
                        {
                            #region ggbsch
                            tmpPart = inwd.TEST_Name_var;
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
                            foundFlag = false;
                            for (int j = 0; j <= i; j++)
                            {
                                //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                {
                                    arrTest[j, 2] = arrTest[j, 2] + ", " + inwd.GGBSCHINWD_GgbsName_var;
                                    arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + 1).ToString();
                                    tmpQty = 1;

                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    foundFlag = true;
                                }
                            }
                            if (foundFlag == false)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart + " of " + inwd.GGBSCHINWD_GgbsName_var;
                                tmpQty = 1;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "CEMT":
                        {
                            #region cemt
                            if (inwd.CEMTTEST_Days_tint > 0)
                                tmpPart = inwd.CEMTTEST_Days_tint + " Days " + inwd.TEST_Name_var;
                            else
                                tmpPart = inwd.TEST_Name_var;
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
                            if (tmpSrNo == 4)
                            {
                                tmpPart = "Initial & Final Setting Time";
                            }
                            foundFlag = false;
                            if (tmpSrNo != 5)
                            {
                                for (int j = 0; j <= i; j++)
                                {
                                    //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                    if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                    {
                                        arrTest[j, 2] = arrTest[j, 2] + ", " + inwd.CEMTINWD_CementName_var;
                                        arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + 1).ToString();
                                        tmpQty = 1;
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        foundFlag = true;
                                    }
                                }
                                if (foundFlag == false)
                                {
                                    arrTest[i, 0] = tmpPart;
                                    arrTest[i, 2] = tmpPart + " of " + inwd.CEMTINWD_CementName_var;
                                    tmpQty = 1;
                                    arrTest[i, 1] = tmpRate.ToString();
                                    arrTest[i, 3] = tmpQty.ToString();
                                    //discount
                                    arrTest[i, 5] = tmpDiscPer.ToString();
                                    arrTest[i, 6] = tmpActualRate.ToString();
                                    arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                    //end
                                    arrTest[i, 8] = tmpTestId.ToString();
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    i = i + 1;
                                }
                            }
                            break;
                            #endregion
                        }
                    case "GGBS":
                        {
                            #region ggbs
                            tmpPart = inwd.TEST_Name_var;
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
                            if (tmpSrNo == 3)
                            {
                                tmpPart = "Initial & Final Setting Time";
                            }
                            foundFlag = false;
                            if (tmpSrNo != 4)
                            {
                                for (int j = 0; j <= i; j++)
                                {
                                    //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                    if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                    {
                                        arrTest[j, 2] = arrTest[j, 2] + ", " + inwd.GGBSINWD_GgbsName_var;
                                        arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + 1).ToString();
                                        tmpQty = 1;
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        foundFlag = true;
                                    }
                                }
                                if (foundFlag == false)
                                {
                                    arrTest[i, 0] = tmpPart;
                                    arrTest[i, 2] = tmpPart + " of " + inwd.GGBSINWD_GgbsName_var;
                                    tmpQty = 1;
                                    arrTest[i, 1] = tmpRate.ToString();
                                    arrTest[i, 3] = tmpQty.ToString();
                                    //discount
                                    arrTest[i, 5] = tmpDiscPer.ToString();
                                    arrTest[i, 6] = tmpActualRate.ToString();
                                    arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                    //end
                                    arrTest[i, 8] = tmpTestId.ToString();
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    i = i + 1;
                                }
                            }
                            break;
                            #endregion
                        }
                    case "CR":
                        {
                            #region cr
                            //commented on 21/09/2020
                            //if (i == 0)
                            //{

                            //    foreach (var CRinwd in inward)
                            //    {
                            //        tmpRate = Convert.ToDecimal(CRinwd.TEST_Rate_int);
                            //        tmpTestId = Convert.ToInt32(CRinwd.TEST_Id);
                            //        //discount
                            //        tmpAppDiscFlag = false;
                            //        tmpDiscPer = 0;
                            //        tmpDiscRate = 0;
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
                            //        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            //    }
                            //    //if (inwd.TEST_Name_var == "Cutting & Testing" || inwd.TEST_Name_var == "Core Testing including cutting")
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
                            //        if (inwd.TEST_Name_var == "Cutting & Testing" || 
                            //            inwd.TEST_Name_var == "Core Testing including cutting"  || 
                            //            inwd.TEST_Name_var == "Core Testing excluding cutting")
                            //        {
                            //            if (tmpAmt <= mst.MinBillforCoreCutTest_int)
                            //            {
                            //                MinBillFlag = true;
                            //                tmpAmt = Convert.ToDecimal(mst.MinBillforCoreCutTest_int);
                            //            }
                            //        }
                            //        else if (inwd.TEST_Name_var == "Only Core Testing" || inwd.TEST_Name_var == "Core Testing excluding cutting")
                            //        {
                            //            if (tmpAmt <= mst.MinBillforCoreTest_int)
                            //            {
                            //                MinBillFlag = true;
                            //                tmpAmt = Convert.ToDecimal(mst.MinBillforCoreTest_int);
                            //            }
                            //        }

                            //    }
                            //    if (MinBillFlag == false)
                            //    {
                            //        tmpAmt = 0;
                            //    }
                            //}
                            ////commented on 21/09/2020
                            if (i == 0)
                            {
                                tmpQty = 0;
                                tmpAmt = 0;
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
                                            tmpAmt = Convert.ToDecimal(mst.MinBillforCoreCutTest_int);
                                        }
                                    }
                                }
                            }
                            if (MinBillFlag == false)
                            {
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
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            else if (i == 0)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart;
                                //discount
                                tmpAppDiscFlag = false;
                                tmpDiscPer = 0;
                                tmpRate = tmpAmt;
                                tmpDiscRate = 0;
                                tmpActualRate = tmpRate;

                                tmpDiscPer = genericDiscPer;
                                tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                tmpRate = Convert.ToDecimal(tmpDiscRate);
                                tmpAppDiscFlag = appDiscFlag;

                                tmpAmt = tmpRate;
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 1] = tmpAmt.ToString();
                                arrTest[i, 3] = "1";
                                //test id
                                tmpTestId = 0;
                                var testMinBill = dc.Test(0, "", 0, "CR", tmpPart, 0);
                                foreach (var testId in testMinBill)
                                {
                                    tmpTestId = Convert.ToInt32(testId.TEST_Id);
                                }
                                arrTest[i, 8] = tmpTestId.ToString();
                                //
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "CORECUT":
                        {
                            #region corecut
                            //tmpPart = tmpPart + " for " + inwd.CORECUTINWD_DepthofCore_var + " X " + inwd.CORECUTINWD_DiameterofCore_var + " of core/s";
                            //tmpRate = Convert.ToInt32(35 * (Convert.ToInt32(inwd.CORECUTINWD_DiameterofCore_var) / 25) * (Convert.ToInt32(inwd.CORECUTINWD_DepthofCore_var) / 25));
                            //tmpQty = Convert.ToInt32(inwd.CORECUTINWD_Quantity_tint);

                            //foundFlag = false;
                            //for (int j = 0; j <= i; j++)
                            //{
                            //    if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                            //    {
                            //        arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + tmpQty).ToString();
                            //        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            //        foundFlag = true;
                            //    }
                            //}
                            //if (foundFlag == false)
                            //{
                            //    arrTest[i, 0] = tmpPart;
                            //    arrTest[i, 2] = tmpPart;
                            //    arrTest[i, 1] = tmpRate.ToString();
                            //    arrTest[i, 3] = tmpQty.ToString();
                            //    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            //    i = i + 1;
                            //}
                            //break;
                            if (i == 0)
                            {
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
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    tmpPart = "Minimum Billing for Core Cutting";
                                }
                                var master = dc.MasterSetting_View(0);
                                foreach (var mst in master)
                                {
                                    if (tmpAmt <= mst.MinBillforCoreCut_int)
                                    {
                                        MinBillFlag = true;
                                        tmpAmt = Convert.ToDecimal(mst.MinBillforCoreCut_int);
                                    }
                                }
                                if (MinBillFlag == false)
                                {
                                    tmpAmt = 0;
                                }
                            }
                            if (MinBillFlag == false)
                            {
                                tmpPart = inwd.TEST_Name_var;
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

                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end                                
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            else if (i == 0)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart;
                                //discount
                                tmpAppDiscFlag = false;
                                tmpDiscPer = 0;
                                tmpRate = tmpAmt;
                                tmpDiscRate = 0;
                                tmpActualRate = tmpRate;
                                tmpDiscPer = genericDiscPer;
                                tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                tmpRate = Convert.ToDecimal(tmpDiscRate);
                                tmpAmt = tmpRate;
                                tmpAppDiscFlag = appDiscFlag;
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 1] = tmpAmt.ToString();
                                arrTest[i, 3] = "1";
                                //test id
                                tmpTestId = 0;
                                var testMinBill = dc.Test(0, "", 0, "CORECUT", tmpPart, 0);
                                foreach (var testId in testMinBill)
                                {
                                    tmpTestId = Convert.ToInt32(testId.TEST_Id);
                                }
                                arrTest[i, 8] = tmpTestId.ToString();
                                //end
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
                                tmpPart = inwd.TEST_Name_var;
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
                                //tmpQty = 1;
                                tmpQty = Convert.ToInt32(inwd.INWD_TotalQty_int);
                                tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);

                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart;
                                if (inwd.CTINWD_TestType_var.Contains("Accelerated Curing") == true)
                                {
                                    tmpQty = 1;
                                }
                                else if (tmpQty < 3)
                                {
                                    tmpRate = 3 * tmpRate;
                                    tmpActualRate = 3 * tmpActualRate;
                                    arrTest[i, 2] = arrTest[i, 2] + " (Minimum Billing)";
                                    tmpQty = 1;
                                }
                                //else
                                //{
                                //    tmpQty = Convert.ToInt32(inwd.INWD_TotalQty_int);
                                //}
                                //TallyNarration = tmpPart + " " + tmpQty + " * " + tmpRate;

                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "FLYASH":
                        {
                            #region flyash
                            tmpPart = inwd.TEST_Name_var;
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
                            if (tmpSrNo == 2)
                            {
                                tmpPart = "Initial & Final Setting Time";
                            }
                            foundFlag = false;
                            if (tmpSrNo != 3)
                            {
                                for (int j = 0; j <= i; j++)
                                {
                                    //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                    if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                    {
                                        arrTest[j, 2] = arrTest[j, 2] + ", " + inwd.FLYASHINWD_FlyAshName_var;
                                        arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + 1).ToString();
                                        tmpQty = 1;
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        foundFlag = true;
                                    }
                                }
                                if (foundFlag == false)
                                {
                                    arrTest[i, 0] = tmpPart;
                                    arrTest[i, 2] = tmpPart + " of Fly Ash " + inwd.FLYASHINWD_FlyAshName_var;
                                    tmpQty = 1;
                                    arrTest[i, 1] = tmpRate.ToString();
                                    arrTest[i, 3] = tmpQty.ToString();
                                    //discount
                                    arrTest[i, 5] = tmpDiscPer.ToString();
                                    arrTest[i, 6] = tmpActualRate.ToString();
                                    arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                    //end
                                    arrTest[i, 8] = tmpTestId.ToString();
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    i = i + 1;
                                }
                            }
                            break;
                            #endregion
                        }
                    case "MF":
                        {
                            #region mf
                            tmpPart = inwd.TEST_Name_var;
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
                            foundFlag = false;
                            if (inwd.MFINWD_TestMaterial_bit == true)
                            {
                                mfTestMatFlag = true;
                            }
                            for (int j = 0; j <= i; j++)
                            {
                                //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                {
                                    arrTest[j, 2] = arrTest[j, 2].Replace(" Grade Of Concrete", "") + ", " + inwd.MFINWD_Grade_var + " Grade Of Concrete";
                                    arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + 1).ToString();
                                    tmpQty = 1;

                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    foundFlag = true;
                                }
                            }
                            if (foundFlag == false)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart + " of " + inwd.MFINWD_Grade_var + " Grade Of Concrete";
                                tmpQty = 1;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion

                        }
                    case "NDT":
                        {
                            #region ndt
                            int upvPoints = 0, reboundPoints = 0;
                            if (i == 0)
                            {
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
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    if (tmpPart == "")
                                        tmpPart = "Minimum Billing For NDT Of " + NDTinwd.NDTTEST_NatureofWorking_var + " by " + NDTinwd.NDTTEST_NDTBy_var;
                                    else
                                        tmpPart = tmpPart + ", " + NDTinwd.NDTTEST_NatureofWorking_var + " by " + NDTinwd.NDTTEST_NDTBy_var;
                                    if (NDTinwd.NDTTEST_NDTBy_var == "UPV")
                                    {
                                        NDTByUPVFlag = true;
                                        upvPoints = Convert.ToInt32(NDTinwd.NDTTEST_Points_tint);
                                    }
                                    else if (NDTinwd.NDTTEST_NDTBy_var == "Rebound Hammer")
                                    {
                                        reboundPoints = Convert.ToInt32(NDTinwd.NDTTEST_Points_tint);
                                    }
                                }
                                
                                var master = dc.MasterSetting_View(0);
                                foreach (var mst in master)
                                {
                                    if (NDTByUPVFlag == true)
                                    {
                                        if (tmpAmt <= mst.MinBillforNDTUPVandHamm_int)
                                        {
                                            MinBillFlag = true;
                                            tmpAmt = Convert.ToDecimal(mst.MinBillforNDTUPVandHamm_int);
                                        }
                                    }
                                    else
                                    {
                                        if (tmpAmt <= mst.MinBillforNDTHamm_int)
                                        {
                                            MinBillFlag = true;
                                            tmpAmt = Convert.ToDecimal(mst.MinBillforNDTHamm_int);
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
                                tmpPart = inwd.TEST_Name_var;
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

                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart;
                                tmpQty = Convert.ToInt32(inwd.NDTTEST_Points_tint);
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            else if (i == 0)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart;
                                //discount
                                tmpAppDiscFlag = false;
                                tmpDiscPer = 0;
                                tmpRate = tmpAmt;
                                tmpDiscRate = 0;
                                tmpActualRate = tmpRate;
                                tmpDiscPer = genericDiscPer;
                                tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                tmpRate = Convert.ToDecimal(tmpDiscRate);
                                tmpAmt = tmpRate;
                                tmpAppDiscFlag = appDiscFlag;
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 1] = tmpAmt.ToString();
                                arrTest[i, 3] = "1";
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
                                arrTest[i, 8] = tmpTestId.ToString();
                                //end
                                i = i + 1;
                            }

                            break;
                            #endregion
                        }
                    case "OT":
                        {
                            #region ot
                            tmpPart = inwd.TEST_Name_var;
                            //tmpPartOT = inwd.OTINWD_SubTest_var.ToString();
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
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            foundFlag = false;
                            for (int j = 0; j <= i; j++)
                            {
                                //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                {
                                    arrTest[j, 2] = arrTest[j, 2] + ", " + inwd.OTINWD_Description_var;
                                    arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + tmpQty).ToString();
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    foundFlag = true;
                                }
                            }
                            if (foundFlag == false)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart + " of " + inwd.OTINWD_Description_var;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "PT":
                        {
                            #region pt
                            //tmpPart = inwd.TEST_Name_var;
                            //tmpRate = Convert.ToInt32(inwd.TEST_Rate_int);

                            //arrTest[i, 0] = tmpPart;
                            //arrTest[i, 2] = tmpPart;
                            //tmpQty = Convert.ToInt32(inwd.PTINWD_Quantity_tint);
                            //arrTest[i, 1] = tmpRate.ToString();
                            //arrTest[i, 3] = tmpQty.ToString();
                            //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            //i = i + 1;

                            tmpPart = inwd.TEST_Name_var;
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
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            foundFlag = false;
                            for (int j = 0; j <= i; j++)
                            {
                                //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                {
                                    arrTest[j, 2] = arrTest[j, 2];
                                    arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + tmpQty).ToString();
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    foundFlag = true;
                                }
                            }
                            if (foundFlag == false)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }

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
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    if (tmpPart == "")
                                        tmpPart = "Minimum Billing for Pile Integrity Testing of " + PILEinwd.PILEINWD_Description_var;
                                    else
                                        tmpPart = tmpPart + ", " + PILEinwd.PILEINWD_Description_var;
                                }
                                var master = dc.MasterSetting_View(0);
                                foreach (var mst in master)
                                {
                                    if (tmpAmt <= mst.MinBillforPILE_int)
                                    {
                                        MinBillFlag = true;
                                        tmpAmt = Convert.ToDecimal(mst.MinBillforPILE_int);
                                    }
                                }
                                if (MinBillFlag == false)
                                {
                                    tmpAmt = 0;
                                }
                            }
                            if (MinBillFlag == false)
                            {
                                tmpPart = inwd.TEST_Name_var;
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
                                tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                                foundFlag = false;
                                for (int j = 0; j <= i; j++)
                                {
                                    //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                    if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                    {
                                        arrTest[j, 2] = arrTest[j, 2] + ", " + inwd.PILEINWD_Description_var;
                                        arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + tmpQty).ToString();
                                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                        foundFlag = true;
                                    }
                                }
                                if (foundFlag == false)
                                {
                                    arrTest[i, 0] = tmpPart;
                                    arrTest[i, 2] = tmpPart + " of " + inwd.PILEINWD_Description_var;
                                    arrTest[i, 1] = tmpRate.ToString();
                                    arrTest[i, 3] = tmpQty.ToString();
                                    //discount
                                    arrTest[i, 5] = tmpDiscPer.ToString();
                                    arrTest[i, 6] = tmpActualRate.ToString();
                                    arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                    //end
                                    arrTest[i, 8] = tmpTestId.ToString();
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    i = i + 1;
                                }
                            }
                            else if (i == 0)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart;
                                //discount
                                tmpAppDiscFlag = false;
                                tmpDiscPer = 0;
                                tmpRate = tmpAmt;
                                tmpDiscRate = 0;
                                tmpActualRate = tmpRate;
                                tmpDiscPer = genericDiscPer;
                                tmpDiscRate = tmpActualRate - (tmpActualRate * (genericDiscPer / 100));
                                tmpRate = Convert.ToDecimal(tmpDiscRate);
                                tmpAmt = tmpRate;
                                tmpAppDiscFlag = appDiscFlag;
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 1] = tmpAmt.ToString();
                                arrTest[i, 3] = "1";
                                //test id
                                tmpPart = "Minimum Billing for Pile Integrity Testing";
                                tmpTestId = 0;
                                var testMinBill = dc.Test(0, "", 0, "PILE", tmpPart, 0);
                                foreach (var testId in testMinBill)
                                {
                                    tmpTestId = Convert.ToInt32(testId.TEST_Id);
                                }
                                arrTest[i, 8] = tmpTestId.ToString();
                                //end
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "GT":
                    case "RWH":
                        {
                            #region gt,rwh
                            tmpAppDiscFlag = false;
                            tmpPart = inwd.GTTEST_Description_var;
                            tmpRate = Convert.ToDecimal(inwd.GTTEST_Rate_int);
                            tmpQty = Convert.ToInt32(inwd.GTTEST_Quantity_tint);
                            if (inwd.GTTEST_TEST_Id != null)
                                tmpTestId = Convert.ToInt32(inwd.GTTEST_TEST_Id);
                            else
                                tmpTestId = 0;
                            if (inwd.GTINW_LumpSump_tint == 1)
                            {
                                if (prevRate != tmpRate)
                                {
                                    if (RecordType == "GT")
                                        arrTest[0, 0] = "Soil Investigation(For details reffer attachment)";
                                    else
                                        arrTest[0, 0] = "Rain Water Harvesting(For details reffer attachment)";

                                    arrTest[0, 2] = arrTest[0, 0];
                                    arrTest[0, 1] = (Convert.ToInt32(arrTest[0, 1]) + tmpRate).ToString();
                                    arrTest[0, 3] = "1";
                                    arrTest[0, 5] = "0";
                                    arrTest[0, 6] = arrTest[0, 1];
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
                                    arrTest[i, 8] = tmpTestId.ToString();
                                    //end
                                    tmpAmt = tmpAmt + tmpRate;
                                    if (i == 0)
                                        i = i + 1;
                                }
                                prevRate = tmpRate;
                            }
                            else
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                arrTest[i, 5] = "0";
                                arrTest[i, 6] = tmpRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
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
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
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
                                                //tmpAmt = tmpAmt + tmpRate;
                                                tmpPart = "Minimum Billing For Soil Testing";

                                                arrTest[i, 0] = tmpPart;
                                                arrTest[i, 2] = tmpPart;
                                                arrTest[i, 1] = tmpRate.ToString();
                                                arrTest[i, 3] = tmpQty.ToString();
                                                //discount
                                                arrTest[i, 5] = tmpDiscPer.ToString();
                                                arrTest[i, 6] = tmpActualRate.ToString();
                                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                                //end
                                                //arrTest[i, 4] = tmpSrNo.ToString();

                                                //test id
                                                tmpTestId = 0;
                                                var testMinBill = dc.Test(0, "", 0, "SO", tmpPart, 0);
                                                foreach (var testId in testMinBill)
                                                {
                                                    tmpTestId = Convert.ToInt32(testId.TEST_Id);
                                                }
                                                arrTest[i, 8] = tmpTestId.ToString();
                                                //end
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
                            tmpPart = inwd.TEST_Name_var;
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
                            foundFlag = false;
                            for (int j = 0; j <= i; j++)
                            {
                                //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                {
                                    if (inwd.TEST_Sr_No == 10) //sand
                                    {
                                        tmpQty = Convert.ToInt32(inwd.SOINWD_Pits_tint);
                                        noOfPits = noOfPits + tmpQty;
                                    }
                                    else if (inwd.TEST_Sr_No == 11) //core
                                    {
                                        tmpQty = Convert.ToInt32(inwd.SOINWD_Cores_tint);
                                        noOfCore = noOfCore + tmpQty;
                                    }
                                    else if (inwd.TEST_Sr_No == 12)//classification
                                    {
                                        tmpQty = Convert.ToInt32(inwd.SOINWD_Quantity_tint);
                                    }

                                    arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + tmpQty).ToString();
                                    foundFlag = true;
                                }
                            }
                            if (foundFlag == false)
                            {
                                if (inwd.TEST_Sr_No == 10) //sand
                                {
                                    tmpQty = Convert.ToInt32(inwd.SOINWD_Pits_tint);
                                    noOfPits = noOfPits + tmpQty;
                                }
                                else if (inwd.TEST_Sr_No == 11) //core
                                {
                                    tmpQty = Convert.ToInt32(inwd.SOINWD_Cores_tint);
                                    noOfCore = noOfCore + tmpQty;
                                }
                                else if (inwd.TEST_Sr_No == 12)//classification
                                {
                                    tmpQty = Convert.ToInt32(inwd.SOINWD_Quantity_tint);
                                }
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                arrTest[i, 4] = tmpSrNo.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                //tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }

                    case "SOLID":
                        {
                            #region solid
                            tmpPart = inwd.TEST_Name_var;
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
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            foundFlag = false;
                            for (int j = 0; j <= i; j++)
                            {
                                //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                {
                                    arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + tmpQty).ToString();
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    foundFlag = true;
                                }
                            }
                            if (foundFlag == false)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "ST":
                        {
                            #region st
                            tmpPart = inwd.TEST_Name_var;
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
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            foundFlag = false;
                            for (int j = 0; j <= i; j++)
                            {
                                //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                {
                                    arrTest[j, 2] = arrTest[j, 2] + ", " + inwd.STINWD_Diameter_tint + " mm";
                                    arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + tmpQty).ToString();
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    foundFlag = true;
                                }
                            }
                            if (foundFlag == false)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart + " of " + inwd.STINWD_Diameter_tint + " mm";
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "STC":
                        {
                            #region stc
                            tmpPart = inwd.TEST_Name_var;
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

                            arrTest[i, 0] = tmpPart;
                            if (arrTest[i, 2] == "" || arrTest[i, 2] == null)
                                arrTest[i, 2] = tmpPart + " of " + inwd.STCINWD_Daimeter_tint + " mm";
                            else
                                arrTest[i, 2] = arrTest[i, 2] + ", " + inwd.STCINWD_Daimeter_tint + " mm";

                            arrTest[i, 1] = tmpRate.ToString();
                            if (arrTest[i, 3] == "" || arrTest[i, 3] == null)
                                arrTest[i, 3] = "0";
                            arrTest[i, 3] = (Convert.ToInt32(arrTest[i, 3]) + tmpQty).ToString();
                            tmpAmt = tmpAmt + (tmpRate * tmpQty);
                            //discount
                            arrTest[i, 5] = tmpDiscPer.ToString();
                            arrTest[i, 6] = tmpActualRate.ToString();
                            arrTest[i, 7] = tmpAppDiscFlag.ToString();
                            //end
                            arrTest[i, 8] = tmpTestId.ToString();
                            if (Convert.ToInt32(arrTest[i, 3]) == inwd.INWD_TotalQty_int)
                            {
                                if (arrTest[i, 2].Contains("mm,") == true)
                                    arrTest[i, 2] = arrTest[i, 2] + " Reinforcement Steel Bars";
                                else
                                    arrTest[i, 2] = arrTest[i, 2] + " Reinforcement Steel Bar";

                                i = i + 1;
                            }
                            
                            break;
                            #endregion
                        }
                    case "TILE":
                        {
                            #region tile
                            tmpPart = inwd.TEST_Name_var;
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
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            foundFlag = false;
                            for (int j = 0; j <= i; j++)
                            {
                                //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                {
                                    arrTest[j, 2] = arrTest[j, 2] + ", " + inwd.TILEINWD_TileType_var;
                                    arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + tmpQty).ToString();
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    foundFlag = true;
                                }
                            }
                            if (foundFlag == false)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart + " of " + inwd.TILEINWD_TileType_var;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                    case "WT":
                        {
                            #region wt
                            tmpPart = inwd.TEST_Name_var;
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
                            tmpSrNo = Convert.ToInt32(inwd.TEST_Sr_No);
                            foundFlag = false;
                            for (int j = 0; j <= i; j++)
                            {
                                //if (arrTest[j, 0] == tmpPart && arrTest[j, 1] == tmpRate.ToString())
                                if (arrTest[j, 8] == tmpTestId.ToString() && arrTest[j, 1] == tmpRate.ToString())
                                {
                                    arrTest[j, 2] = arrTest[j, 2] + ", " + inwd.WTINWD_Description_var;
                                    arrTest[j, 3] = (Convert.ToInt32(arrTest[j, 3]) + tmpQty).ToString();
                                    tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                    foundFlag = true;
                                }
                            }
                            if (foundFlag == false)
                            {
                                arrTest[i, 0] = tmpPart;
                                arrTest[i, 2] = tmpPart + " of Water " + inwd.WTINWD_Description_var;
                                arrTest[i, 1] = tmpRate.ToString();
                                arrTest[i, 3] = tmpQty.ToString();
                                //discount
                                arrTest[i, 5] = tmpDiscPer.ToString();
                                arrTest[i, 6] = tmpActualRate.ToString();
                                arrTest[i, 7] = tmpAppDiscFlag.ToString();
                                //end
                                arrTest[i, 8] = tmpTestId.ToString();
                                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                                i = i + 1;
                            }
                            break;
                            #endregion
                        }
                }
            }
            //some code for steel and soil and mf
            if (RecordType == "ST")
            {
                for (int j = 0; j < i; j++)
                {
                    if (arrTest[j, 2].Contains("Bend & Rebend") == true)
                        arrTest[j, 2] = arrTest[j, 2].Replace("Bend & Rebend", "Rebend Test");
                    else if (arrTest[j, 2].Contains("Bend") == true)
                        arrTest[j, 2] = arrTest[j, 2].Replace("Bend", "Bend Test");

                    if (arrTest[j, 2].Contains("mm,") == true)
                        arrTest[j, 2] = arrTest[j, 2] + " Reinforcement Steel Bars";
                    else
                        arrTest[j, 2] = arrTest[j, 2] + " Reinforcement Steel Bar";
                }
            }
            else if (RecordType == "SO")
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
                for (int j = 0; j < i; j++)
                {
                    if (arrTest[j, 4] == "10") //sand -pits
                    {
                        arrTest[j, 3] = noOfPits.ToString();
                    }
                    else if (arrTest[j, 4] == "11") //core
                    {
                        arrTest[j, 3] = noOfCore.ToString();
                    }
                    tmpAmt = tmpAmt + (Convert.ToDecimal(arrTest[j, 1]) * Convert.ToDecimal(arrTest[j, 3]));
                }
            }
            else if (RecordType == "MF")
            {
                if (mfTestMatFlag == true)
                {
                    //var test = dc.Test(9, "", 0, "MF", "", 0);
                    var test = dc.Test_View_ClientWise(ClientId, SiteId, 0, 9, "MF");
                    foreach (var t in test)
                    {
                        tmpPart = t.TEST_Name_var;
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
                        tmpSrNo = Convert.ToInt32(t.TEST_Sr_No);

                        arrTest[i, 0] = tmpPart;
                        arrTest[i, 2] = tmpPart;
                        tmpQty = 1;
                        arrTest[i, 1] = tmpRate.ToString();
                        arrTest[i, 3] = "1";
                        //discount
                        arrTest[i, 5] = tmpDiscPer.ToString();
                        arrTest[i, 6] = tmpActualRate.ToString();
                        arrTest[i, 7] = tmpAppDiscFlag.ToString();
                        //end
                        arrTest[i, 8] = tmpTestId.ToString();
                        tmpAmt = tmpAmt + (tmpRate * tmpQty);
                        i = i + 1;
                    }
                }
            }
            
            //Other Charges
            if (otherChargesAmt > 0)
            {
                tmpPart = "Other Charges";
                tmpRate = otherChargesAmt;
                tmpQty = 1;
                tmpSrNo = 0;

                arrTest[i, 0] = tmpPart;
                arrTest[i, 2] = tmpPart;
                tmpQty = 1;
                arrTest[i, 1] = tmpRate.ToString();
                arrTest[i, 3] = "1";
                arrTest[i, 5] = "0";
                arrTest[i, 6] = tmpRate.ToString();
                arrTest[i, 8] = miscTestId.ToString();
                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                i = i + 1;
            }
            //Other charges for NDT, Core, SO, GT, Other
            var re = dc.OtherCharges_View(RecordType, RecordNo);
            foreach (var r in re)
            {
                tmpPart = r.OTHERCHRG_Description_var;
                tmpRate = Convert.ToDecimal(r.OTHERCHRG_Rate_num);
                tmpQty = Convert.ToInt32(r.OTHERCHRG_Quantity_int);
                tmpSrNo = 0;

                arrTest[i, 0] = tmpPart;
                arrTest[i, 2] = tmpPart;
                arrTest[i, 1] = tmpRate.ToString();
                arrTest[i, 3] = tmpQty.ToString();
                //discount
                arrTest[i, 5] = "0";
                arrTest[i, 6] = tmpRate.ToString();
                //end
                arrTest[i, 8] = miscTestId.ToString();
                tmpAmt = tmpAmt + (tmpRate * tmpQty);
                i++;
            }
            //
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
            dc.BillDetail_Update(BillNo, 0, 0, 0, "", 0, "", 0, 0, false,0, true);
            //BillNo = 
            dc.Bill_Update(BillNo, ClientId, ClientName, SiteId, discPerFlag, xDisc, xDiscAmt, xSrvTax,
            xSrvTaxAmt, xSwTax, xSwTaxAmt, xKkTax, xKkTaxAmt, xCgst, xCgstAmt, xSgst, xSgstAmt, xIgst, xIgstAmt,
            0, xNetAmt, edCess, highEdCess, roundOff, RecordType, RecordNo, false, TallyNarration,
            false, Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["LoginId"]), false, "", billPrintLockStatus, "", insertBill);

            string SACCode = "";
            if (RecordType == "SO" || RecordType == "GT")
                SACCode = "998341";
            else
                SACCode = "998346";
            for (int j = 0; j < i; j++)
            {
                tmpAmt = Convert.ToDecimal(arrTest[j, 1]) * Convert.ToDecimal(arrTest[j, 3]);
                tmpAppDiscFlag = false;
                if (arrTest[j, 7] != null && arrTest[j, 7].ToString() != "")
                    tmpAppDiscFlag = Convert.ToBoolean(arrTest[j, 7]);
                if (arrTest[j, 8] == "" || arrTest[j, 8] == null)
                    arrTest[j, 8] = "0";
                dc.BillDetail_Update(BillNo, j + 1, Convert.ToInt32(arrTest[j, 3]), tmpAmt, arrTest[j, 2], Convert.ToDecimal(arrTest[j, 1]), SACCode, Convert.ToDecimal(arrTest[j, 6]), Convert.ToDecimal(arrTest[j, 5]), tmpAppDiscFlag, Convert.ToInt32(arrTest[j, 8]), false);
            }
            dc.Inward_Update_BillNo(RecordNo, RecordType, BillNo);

            return BillNo;

        }

        private string getDiscount(int clId, int siteId, string recType, int enqId)
        {
            //decimal totDisc = 0, totDiscA = 0, totDiscB = 0, introDiscA = 0, volDiscB = 0,
            //    timelyDiscC = 0, AdvDiscD = 0, loyDiscE = 0, propDiscF = 0, AppDiscG = 0;
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
        }
        
        public string getBillString(string BillNo, bool duplicateBillFlg)
        {
            string mySql = "", tempSql = "";
            decimal billTotal = 0;
            //mySql += "<html>";
            //mySql += "<head>";
            //mySql += "<style type='text/css'>";
            //mySql += "body {margin-left:2em margin-right:2em}";
            //mySql += "</style>";
            //mySql += "</head>";

            mySql += "<tr><td width='100%' height='105'>";
            //mySql += "<img border=0 src='Images/" + "cqralogo" + ".JPG'>";
            mySql += "&nbsp;</td></tr>";

            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            string strOfficeCopy = "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=2>Office Copy</font></td></tr>";
            mySql += strOfficeCopy;


            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Tax Invoice</b></font></td></tr>";
            if (duplicateBillFlg == true)
            {
                mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=2><b>Duplicate Copy</b></font></td></tr>";
            }
            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";
            //lblBtType = "Office Copy"
            //lblDuplicateCopy = "Duplicate Copy"
            //lblSubRecNo.Caption = rsRecordTable("WorkOrderNo")
            //If cntRABill > 0 Then
            //    lblBtType.Caption = lblBtType.Caption + " (RA Bill " & cntRABill & ")"
            //End If
            string result = "";
            var b = dc.Bill_View(BillNo, 0, 0, "", 0, false, false, null, null);
            foreach (var bill in b)
            {
                if (bill.BILL_ApproveStatus_bit == true)
                {
                    mySql = mySql.Replace(strOfficeCopy, "");
                }
                if (bill.SITE_RABillStatus_bit == true && bill.BILL_RecordType_var == "GT")
                {
                    int rabillcnt = 0;
                    var rabill = dc.Bill_View_RABill(bill.BILL_RecordNo_int);
                    foreach (var rab in rabill)
                    {
                        rabillcnt++;
                        if (rab.BILL_Id == bill.BILL_Id)
                        {
                            break;
                        }
                    }
                    mySql = mySql.Replace("Tax Invoice", "Tax Invoice (RA Bill " + rabillcnt + ")");
                }
                if (bill.BILL_Status_bit == true)
                {
                    mySql = mySql.Replace("Tax Invoice", "Tax Invoice(Cancelled)");
                }

                mySql += "<tr>" +
                    "<td width='13%' height=19 align=left valign=top><font size=2>Customer Name</font></td>" +
                    "<td width='1%' height=19><font size=2>:</font></td>" +
                    "<td width='45%' height=19 rowspan=2 align=left valign=top><font size=2>" + bill.CL_Name_var + "</font></td>" +
                    "<td width='13%' height=19 colspan=3><font size=2>Service Tax Reg. No. : AABCD2992CSD003</font></td>" +
                    "<td width='1%' height=19><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                    "<td height=19 align=left valign=top><font size=2>&nbsp;</font></td>" +
                    "<td height=19><font size=2>&nbsp;</font></td>" +
                    "<td height=19 colspan=3><font size=2>Service Tax Category : Technical Testing and Analysis</font></td>" +
                    "<td height=19><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                    "<td height=19 align=left valign=top><font size=2>Office Address</font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19 rowspan=2 align=left valign=top><font size=2>" + bill.CL_OfficeAddress_var + "</font></td>" +
                    "<td height=19 colspan=3><font size=2>Premises Code : 710704A001</font></td>" +
                    "<td height=19><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                    "<td align=left valign=top height=19><font size=2>&nbsp;</font></td>" +
                    "<td height=19><font size=2>&nbsp;</font></td>" +
                    "<td height=19 colspan=3><font size=2>PAN No. : AABCD2992C</font></td>" +
                    "<td height=19><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                    "<td height=19 width=13% align=left valign=top  ><font size=2>Site Name</font></td>" +
                    "<td height=19 width=1%><font size=2>:</font></td>" +
                    "<td height=19 width=45% rowspan=3 align=left valign=top><font size=2>" + bill.SITE_Name_var + "</font></td>" +
                    "<td height=19 width=13%><font size=2><b>Tax Invoice No.</b></font></td>" +
                    "<td height=19 width=1%><font size=2>:</font></td>" +
                    //"<td height=19 width=25%><font size=2><b>" + "DT - " + bill.BILL_Id.ToString() + "</b></font></td>" +
                    "<td height=19 width=25%><font size=2><b>" + bill.BILL_Id.ToString() + "</b></font></td>" +
                    "<td height=19 width=2%><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                    "<td height=19 align=left valign=top><font size=2>&nbsp;</font></td>" +
                    "<td height=19><font size=2>&nbsp;</font></td>" +
                    "<td height=19><font size=2><b>Date</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2><b>" + Convert.ToDateTime(bill.BILL_Date_dt).ToString("dd/MM/yyyy") + "</b></font></td>" +
                    "<td height=19><font size=2>&nbsp;</font></td>" +
                    "</tr>";
                string workOrderNo = "";
                if (bill.BILL_WorkOrderNo_var != null && bill.BILL_WorkOrderNo_var != "" && bill.BILL_WorkOrderNo_var != "NA" && bill.BILL_WorkOrderNo_var != "0" && bill.BILL_WorkOrderNo_var != "-")
                {
                    workOrderNo = bill.BILL_WorkOrderNo_var;
                }
                else if (bill.WorkOrderNo != null && bill.WorkOrderNo != "" && bill.WorkOrderNo != "NA" && bill.WorkOrderNo != "0" && bill.WorkOrderNo != "-")
                {
                    workOrderNo = bill.WorkOrderNo;
                }
                if (workOrderNo != "")
                {
                    mySql += "<tr>" +
                   "<td height=19>&nbsp;</td>" +
                   "<td height=19>&nbsp;</td>" +
                   "<td height=19 colspan=3><font size=2>" + workOrderNo + "</font></td>" + //WorkOrderNo
                   "<td height=19>&nbsp;</td>" +
                   "</tr>";
                }
                mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";

                mySql += "<tr><td colspan=6 align=left valign=top>";

                mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
                mySql += "<tr>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Sr.No.</b></font></td>";
                mySql += "<td width= 50% align=center valign=top height=19 ><font size=2><b>Particular</b></font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Quantity</b></font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Rate</b></font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Amount</b></font></td>";
                mySql += "</tr>";

                //tempSql = "<tr>" +
                //        "<td height=19 colspan=5><font size=2 >&nbsp;</font></td>" +
                //        "</tr>";

                tempSql += "<tr>" +
                        "<td height=19 align=right colspan=4><font size=2>Subtotal &nbsp;</font></td>" +
                        "<td height=19 align=right><font size=2>" + "BillTotal" + "</font></td>" +
                        "</tr>";

                tempSql += "<tr>" +
                        "<td height=19 align=right colspan=4><font size=2>Discount ";
                if (bill.BILL_DiscountPerStatus_bit == true)
                    tempSql += bill.BILL_Discount_num + " % ";
                tempSql += "&nbsp;</font></td>" +
                        "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_DiscountAmt_num).ToString("0.00") + "</font></td>" +
                        "</tr>";

                tempSql += "<tr>" +
                        "<td height=19 align=right colspan=4><font size=2>Service Tax " + bill.BILL_SerTax_num + " % &nbsp;</font></td>" +
                        "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_SerTaxAmt_num).ToString("0.00") + "</font></td>" +
                        "</tr>";

                tempSql += "<tr>" +
                        "<td height=19 align=right colspan=4><font size=2>Swachh Bharat Cess " + bill.BILL_SwachhBharatTax_num + " % &nbsp;</font></td>" +
                        "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_SwachhBharatTaxAmt_num).ToString("0.00") + "</font></td>" +
                        "</tr>";

                tempSql += "<tr>" +
                        "<td height=19 align=right colspan=4><font size=2>Krishi Kalyan Cess " + bill.BILL_KisanKrishiTax_num + " % &nbsp;</font></td>" +
                        "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_KisanKrishiTaxAmt_num).ToString("0.00") + "</font></td>" +
                        "</tr>";

                //tempSql += "<tr>" +
                //        "<td height=19 align=right colspan=4><font size=2>Education Cess 2 % &nbsp;</font></td>" +
                //        "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_EdCessAmt_num).ToString("0.00") + "</font></td>" +
                //        "</tr>";

                //tempSql += "<tr>" +
                //        "<td height=19 align=right colspan=4><font size=2>Secondary and Higher Education Cess 1 % &nbsp;</font></td>" +
                //        "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_HighEdCessAmt_num).ToString("0.00") + "</font></td>" +
                //        "</tr>";

                tempSql += "<tr>" +
                        "<td height=19 align=right colspan=4><font size=2>Round Off &nbsp;</font></td>" +
                        "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_RoundOffAmt_num).ToString("0.00") + "</font></td>" +
                        "</tr>";

                tempSql += "<tr>" +
                        "<td height=19 align=right colspan=4><font size=2>Net Total &nbsp;</font></td>" +
                        "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_NetAmt_num).ToString("0.00") + "</font></td>" +
                        "</tr>";
                result = "   " + CnvtAmttoWords(Convert.ToInt32(bill.BILL_NetAmt_num)).ToString() + " " + "Only.";

            }
            var bd = dc.BillDetail_View(BillNo);
            foreach (var bill in bd)
            {
                //If InStr(rsBillingTable("Part"), "Bend & Rebend Test Of") > 0 Then
                //    mySql = Replace(rsBillingTable("Part"), "Bend & Rebend Test Of", "Rebend Test Of")
                //Else
                //    mySql = mySql + rsBillingTable("Part")
                //End If

                mySql += "<tr>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + bill.BILLD_SrNo_int + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + bill.BILLD_TEST_Name_var + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + bill.BILLD_Quantity_int + "</font></td>";
                mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.BILLD_Rate_num).ToString("0.00") + "</font></td>";
                mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.BILLD_Amt_num).ToString("0.00") + "</font></td>";
                mySql += "</tr>";

                billTotal = billTotal + Convert.ToDecimal(bill.BILLD_Amt_num);
            }
            tempSql = tempSql.Replace("BillTotal", billTotal.ToString("0.00"));
            mySql += tempSql;

            mySql += "</table>";
            mySql += "</td>";

            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            //
            mySql += "<tr>";
            mySql += "<td colspan=7 width= 100% align=left valign=top height=19 ><font size=2>Amount in Words : " + " " + result + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2><b><i>Please issue the Cheque/D.D. in the name of Durocrete Engineering Services Pvt. Ltd..</i></b></font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
            if (cnStr.ToLower().Contains("mumbai") == true)
            {
                mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2><b>For all technical queries contact on 9850500013.</b></font></td>";
            }
            else if (cnStr.ToLower().Contains("nashik") == true)
            {
                mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2><b>For all technical queries contact on 9527005478.</b></font></td>";
            }
            else
            {
                mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2><b>For all technical queries contact on (020)24348027.</b></font></td>";
            }
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td colspan=4 width=100% align=left valign=top height=19 ><font size=2><b>Receiver's Signature</b></font></td>";
            mySql += "<td colspan=3 width=100% align=left valign=top height=19 ><font size=2><b>Authorized Sign</b></font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2>All our invoices are payable immediately, Delay of payment beyond 30 days from the date of billing shall attract an interest of 2% per month on the outstanding amount.</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2>CIN - U28939PN1999PTC014212.</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=1>Regd.Add - 1160/5, Gharpure Colony Shivaji Nagar,Pune 411005,Maharashtra India.</font></td>";
            mySql += "</tr>";
            //
            mySql += "</table>";
            //mySql += "</html>";
            return mySql;

        }

        public string getBillStringWithGST(string BillNo, bool duplicateBillFlg)
        {
            string mySql = "", tempSql = "";
            decimal billTotal = 0, discount = 0;

            mySql += "<tr><td width='100%' height='105'>";
            //mySql += "<img border=0 src='Images/" + "cqralogo" + ".JPG'>";
            mySql += "&nbsp;</td></tr>";

            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";
            string strOfficeCopy = "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=2>Office Copy</font></td></tr>";

            mySql += strOfficeCopy;


            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Tax Invoice</b></font></td></tr>";
            if (duplicateBillFlg == true)
            {
                mySql += "<tr><td width='99%' colspan=6 align=right valign=top height=19><font size=2><b>DUPLICATE</b></font></td></tr>";
            }
            //            mySql += "<tr><td width='99%' colspan=7>&nbsp;</td></tr>";

            string result = "";
            bool monthlyBillFlag = false, discountedRateFlag = false;
            int colspan = 5;
            var b = dc.Bill_View(BillNo, 0, 0, "", 0, false, false, null, null);
            var bd = dc.BillDetail_View(BillNo).ToList();
            foreach (var bill in b)
            {
                if (bill.BILL_ApproveStatus_bit == true)
                {
                    mySql = mySql.Replace(strOfficeCopy, "<tr><td width='99%' colspan=6 align=right valign=top height=17><font size=2>ORIGINAL</font></td></tr>");
                }
                if (bill.SITE_RABillStatus_bit == true && bill.BILL_RecordType_var == "GT")
                {
                    int rabillcnt = 0;
                    var rabill = dc.Bill_View_RABill(bill.BILL_RecordNo_int);
                    foreach (var rab in rabill)
                    {
                        rabillcnt++;
                        if (rab.BILL_Id == bill.BILL_Id)
                        {
                            break;
                        }
                    }
                    mySql = mySql.Replace("Tax Invoice", "Tax Invoice (RA Bill " + rabillcnt + ")");
                }
                if (bill.BILL_Status_bit == true)
                {
                    mySql = mySql.Replace("Tax Invoice", "Tax Invoice(Cancelled)");
                }

                mySql += "<tr>" +
                    "<td width='13%' height=19 align=left valign=top><font size=2><b>Tax Invoice No.</b></font></td>" +
                    "<td width='1%' height=19><font size=2>:</font></td>" +
                    "<td width='35%' height=19 align=left valign=top><font size=2>" + bill.BILL_Id.ToString() + "</font></td>" +
                    "<td width='49%' height=19 colspan=3><font size=2><b>GST No. of Service Provider :</b> 27AABCD2992C2ZS</font></td>" +
                    "<td width='1%' height=19><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                    "<td height=19><font size=2><b>Date of Issue</b></font></td>" +
                    "<td height=19><font size=2>:</font></td>" +
                    "<td height=19><font size=2>" + Convert.ToDateTime(bill.BILL_Date_dt).ToString("dd/MM/yyyy") + "</font></td>" +
                    "<td height=19 colspan=3><font size=2><b>PAN No.:</b> AABCD2992C &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>MSMED No.:</b> MH26F0020876   </font></td>" +
                    "<td height=19><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                if (bill.BILL_RecordType_var == "Monthly")
                {
                    monthlyBillFlag = true;
                    mySql += "<tr>" +
                        "<td height=19><font size=2><b>Billing Period</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + bill.BILL_BillingPeriod_var + "</font></td>" +
                        "<td height=19 colspan=3><font size=2>&nbsp;</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";
                }
                else if (bill.BILL_RecordNo_int > 0)
                {
                    string strRecNo = bill.BILL_RecordNo_int.ToString();
                    if (bill.BILL_RecordType_var == "OT")
                    {
                        var otherInward = dc.AllInward_View("OT", bill.BILL_RecordNo_int, "");
                        foreach (var n in otherInward)
                        {
                            if (n.OTINWD_InterBranchRefNo_var != null && n.OTINWD_InterBranchRefNo_var != "")
                                strRecNo = n.OTINWD_InterBranchRefNo_var;
                        }
                    }
                    mySql += "<tr>" +
                        "<td height=19><font size=2><b>Record No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + strRecNo + "</font></td>" +
                        "<td height=19 colspan=3><font size=2>&nbsp;</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";
                }
                string workOrderNo = "";
                if (bill.BILL_WorkOrderNo_var != null && bill.BILL_WorkOrderNo_var != "" && bill.BILL_WorkOrderNo_var != "NA" && bill.BILL_WorkOrderNo_var != "0" && bill.BILL_WorkOrderNo_var != "-")
                {
                    workOrderNo = bill.BILL_WorkOrderNo_var;
                }
                else if (bill.WorkOrderNo != null && bill.WorkOrderNo != "" && bill.WorkOrderNo != "NA" && bill.WorkOrderNo != "0" && bill.WorkOrderNo != "-")
                {
                    workOrderNo = bill.WorkOrderNo;
                }

                if (workOrderNo != "")
                {
                    mySql += "<tr>" +
                        "<td height=19><font size=2><b>Work Order No.</b></font></td>" +
                        "<td height=19><font size=2>:</font></td>" +
                        "<td height=19><font size=2>" + workOrderNo + "</font></td>" +
                        "<td height=19 colspan=3><font size=2>&nbsp;</font></td>" +
                        "<td height=19><font size=2>&nbsp;</font></td>" +
                        "</tr>";
                }

                mySql += "<tr>" +
                    "<td height=19 align=left colspan=3><font size=2><b>Billed to  </b></font></td>" +
                    "<td height=19 align=left colspan=3><font size=2><b>Place of Work </b></font></td>" +
                    "<td height=19 ><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                    "<td width='13%' height=19 align=left valign=top><font size=2><b>Name</b></font></td>" +
                    "<td width='1%' align=left valign=top><font size=2><b>:</b></font></td>" +
                    "<td width='35%' align=left valign=top><font size=2>" + bill.CL_Name_var + "</font></td>" +
                    "<td width=10% align=left valign=top><font size=2><b>Name</b></font></td>" +
                    "<td width=2% align=left valign=top><font size=2><b>:</b></font></td>" +
                    "<td width=35% align=left valign=top><font size=2>" + bill.CL_Name_var + "</font></td>" +
                    "<td width=2%><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                    "<td height=19 align=left valign=top><font size=2><b>Address</b></font></td>" +
                    "<td align=left valign=top><font size=2><b>:</b></font></td>" +
                    "<td align=left valign=top><font size=2>" + bill.CL_OfficeAddress_var + "</font></td>" +
                    "<td align=left valign=top><font size=2><b>Address</b></font></td>" +
                    "<td align=left valign=top><font size=2><b>:</b></font></td>" +
                    "<td align=left valign=top><font size=2>" + bill.SITE_Name_var + " " + bill.SITE_Address_var + "</font></td>" +
                    "<td><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                    "<td height=19 align=left valign=top><font size=2><b>State</b></font></td>" +
                    "<td align=left valign=top><font size=2><b>:</b></font></td>" +
                    "<td align=left valign=top><font size=2>" + bill.CL_State_var + "</font></td>" +
                    "<td align=left valign=top><font size=2><b>State</b></font></td>" +
                    "<td align=left valign=top><font size=2><b>:</b></font></td>" +
                    "<td align=left valign=top><font size=2>" + bill.SITE_State_var + "</font></td>" +
                    "<td><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                mySql += "<tr>" +
                    "<td height=19 align=left valign=top><font size=2><b>City</b></font></td>" +
                    "<td align=left valign=top><font size=2><b>:</b></font></td>" +
                    "<td align=left valign=top><font size=2>" + bill.CL_City_var + " - " + bill.CL_Pin_int + "</font></td>" +
                    "<td align=left valign=top><font size=2><b>City</b></font></td>" +
                    "<td align=left valign=top><font size=2><b>:</b></font></td>" +
                    "<td align=left valign=top><font size=2>" + bill.SITE_City_var + " - " + bill.SITE_Pincode_int + "</font></td>" +
                    "<td><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                //mySql += "<tr>" +
                //    "<td height=19 align=left valign=top><font size=2>Pin Code</font></td>" +
                //    "<td align=left valign=top><font size=2>:</font></td>" +
                //    "<td align=left valign=top><font size=2>" + bill.CL_Pin_int + "</font></td>" +
                //    "<td align=left valign=top><font size=2>Pin Code</font></td>" +
                //    "<td align=left valign=top><font size=2>:</font></td>" +
                //    "<td align=left valign=top><font size=2>" + bill.SITE_Pincode_int + "</font></td>" +
                //    "<td><font size=2>&nbsp;</font></td>" +
                //    "</tr>";

                string clientGSTNo = "NA", siteGSTNo = "NA";
                if (bill.BILL_CL_GSTNo_var != null && bill.BILL_CL_GSTNo_var != "")
                {
                    clientGSTNo = bill.BILL_CL_GSTNo_var;
                }
                else
                {
                    clientGSTNo = "Unregistered";
                }
                if (bill.BILL_SITE_GSTNo_var != null && bill.BILL_SITE_GSTNo_var != "")
                {
                    siteGSTNo = bill.BILL_SITE_GSTNo_var;
                }
                else
                {
                    siteGSTNo = "Unregistered";
                }

                mySql += "<tr>" +
                    "<td height=19 align=left valign=top><font size=2><b>GSTIN</b></font></td>" +
                    "<td align=left valign=top><font size=2><b>:</b></font></td>" +
                    "<td align=left valign=top><font size=2>" + clientGSTNo + "</font></td>" +
                    "<td align=left valign=top><font size=2><b>GSTIN</b></font></td>" +
                    "<td align=left valign=top><font size=2><b>:</b></font></td>" +
                    "<td align=left valign=top><font size=2>" + siteGSTNo + "</font></td>" +
                    "<td><font size=2>&nbsp;</font></td>" +
                    "</tr>";

                if (bill.reportClientName != null && bill.reportClientName != "")
                {
                    mySql += "<tr>" +
                        "<td height=19 align=left valign=top><font size=2><b>Report for</b></font></td>" +
                        "<td align=left valign=top><font size=2><b>:</b></font></td>" +
                        "<td align=left valign=top><font size=2>" + bill.reportClientName + "</font></td>" +
                        "<td align=left valign=top><font size=2><b>Site</b></font></td>" +
                        "<td align=left valign=top><font size=2><b>:</b></font></td>" +
                        "<td align=left valign=top><font size=2>" + bill.reportSiteName + "</font></td>" +
                        "<td><font size=2>&nbsp;</font></td>" +
                        "</tr>";
                }
                //mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";

                if (monthlyBillFlag == false)
                {
                    foreach (var bdd in bd)
                    {
                        if (bdd.BILLD_ActualRate_num != null && bdd.BILLD_ActualRate_num > 0)
                        {
                            discountedRateFlag = true;
                            colspan = 7;
                        }
                    }
                }
                mySql += "<tr><td colspan=6 align=left valign=top>";

                mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
                mySql += "<tr>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Sr.No.</b></font></td>";
                mySql += "<td width= 50% align=center valign=top height=19 ><font size=2><b>Particular</b></font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>SAC Code</b></font></td>";
                if (monthlyBillFlag == true)
                {
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Reference No.</b></font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Received Date</b></font></td>";
                }
                else if (discountedRateFlag == true)
                {
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Quantity</b></font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Rate</b></font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Discount (%)</b></font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Discounted Rate</b></font></td>";                   
                }
                else
                {
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Quantity</b></font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Rate</b></font></td>";
                }
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Amount</b></font></td>";
                mySql += "</tr>";

                discount = Convert.ToDecimal(bill.BILL_DiscountAmt_num);
                if (discount > 0)
                {
                    tempSql += "<tr>" +
                            "<td height=19 align=right colspan=" + colspan + "><font size=2>Subtotal &nbsp;</font></td>" +
                            "<td height=19 align=right><font size=2>" + "BillTotal" + "</font></td>" +
                            "</tr>";

                    tempSql += "<tr>" +
                            "<td height=19 align=right colspan=" + colspan + "><font size=2>Discount ";
                    if (bill.BILL_DiscountPerStatus_bit == true)
                        tempSql += bill.BILL_Discount_num + " % ";
                    tempSql += "&nbsp;</font></td>" +
                            "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_DiscountAmt_num).ToString("0.00") + "</font></td>" +
                            "</tr>";
                    tempSql += "<tr>" +
                            "<td height=19 align=right colspan=" + colspan + "><font size=2>Net Amount &nbsp;</font></td>" +
                            "<td height=19 align=right><font size=2>" + "NetAmount" + "</font></td>" +
                            "</tr>";
                }
                else
                {
                    tempSql += "<tr>" +
                        "<td height=19 align=right colspan=" + colspan + "><font size=2>Net Amount &nbsp;</font></td>" +
                        "<td height=19 align=right><font size=2>" + "NetAmount" + "</font></td>" +
                        "</tr>";
                }

                if (bill.BILL_CGST_num != null && bill.BILL_CGST_num != 0)
                {
                    tempSql += "<tr>" +
                            "<td height=19 align=right colspan=" + colspan + "><font size=2>CGST " + bill.BILL_CGST_num + " % &nbsp;</font></td>" +
                            "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_CGSTAmt_num).ToString("0.00") + "</font></td>" +
                            "</tr>";
                }
                if (bill.BILL_SGST_num != null && bill.BILL_SGST_num != 0)
                {
                    tempSql += "<tr>" +
                            "<td height=19 align=right colspan=" + colspan + "><font size=2>SGST " + bill.BILL_SGST_num + " % &nbsp;</font></td>" +
                            "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_SGSTAmt_num).ToString("0.00") + "</font></td>" +
                            "</tr>";
                }
                if (bill.BILL_IGST_num != null && bill.BILL_IGST_num != 0)
                {
                    tempSql += "<tr>" +
                            "<td height=19 align=right colspan=" + colspan + "><font size=2>IGST " + bill.BILL_IGST_num + " % &nbsp;</font></td>" +
                            "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_IGSTAmt_num).ToString("0.00") + "</font></td>" +
                            "</tr>";
                }
                if (Convert.ToDecimal(bill.BILL_RoundOffAmt_num) != 0)
                {
                    tempSql += "<tr>" +
                            "<td height=19 align=right colspan=" + colspan + "><font size=2>Round Off &nbsp;</font></td>" +
                            "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_RoundOffAmt_num).ToString("0.00") + "</font></td>" +
                            "</tr>";
                }
                tempSql += "<tr>" +
                        "<td height=19 align=right colspan=" + colspan + "><font size=2>Total Invoice Value &nbsp;</font></td>" +
                        "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_NetAmt_num).ToString("0.00") + "</font></td>" +
                        "</tr>";
                decimal tempNetBalance = 0;
                tempNetBalance = Convert.ToDecimal(bill.BILL_NetAmt_num);
                if (bill.BILL_AdvancePaid_num != null && Convert.ToDecimal(bill.BILL_AdvancePaid_num) > 0)
                {
                    tempSql += "<tr>" +
                            "<td height=19 align=right colspan=" + colspan + "><font size=2>Less Advance Paid &nbsp;</font></td>" +
                            "<td height=19 align=right><font size=2>" + Convert.ToDecimal(bill.BILL_AdvancePaid_num).ToString("0.00") + "</font></td>" +
                            "</tr>";

                    tempNetBalance = tempNetBalance - Convert.ToDecimal(bill.BILL_AdvancePaid_num);
                    tempSql += "<tr>" +
                            "<td height=19 align=right colspan=" + colspan + "><font size=2>Net Balance Payable &nbsp;</font></td>" +
                            "<td height=19 align=right><font size=2>" + tempNetBalance.ToString("0.00") + "</font></td>" +
                            "</tr>";
                }
                result = "   " + CnvtAmttoWords(Convert.ToInt32(tempNetBalance)).ToString() + " " + "Only.";
            }
            //var bd = dc.BillDetail_View(BillNo);
            foreach (var bill in bd)
            {
                mySql += "<tr>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + bill.BILLD_SrNo_int + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + bill.BILLD_TEST_Name_var + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + bill.BILLD_SACCode_var + "</font></td>";
                if (monthlyBillFlag == true)
                {
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + bill.BILLD_ReferenceNo_int.ToString() + "</font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDateTime(bill.BILLD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</font></td>";
                }
                else if (discountedRateFlag == true)
                {
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.BILLD_Quantity_int).ToString("0.##") + "</font></td>";
                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.BILLD_ActualRate_num).ToString("0.00") + "</font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.BILLD_DiscountPer_num).ToString("0.##") + "</font></td>";
                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.BILLD_Rate_num).ToString("0.00") + "</font></td>";

                }
                else
                {
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.BILLD_Quantity_int).ToString("0.##") + "</font></td>";
                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.BILLD_Rate_num).ToString("0.00") + "</font></td>";
                }
                mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.BILLD_Amt_num).ToString("0.00") + "</font></td>";
                mySql += "</tr>";

                billTotal = billTotal + Convert.ToDecimal(bill.BILLD_Amt_num);
            }

            tempSql = tempSql.Replace("BillTotal", billTotal.ToString("0.00"));
            tempSql = tempSql.Replace("NetAmount", (billTotal - discount).ToString("0.00"));
            mySql += tempSql;

            mySql += "</table>";
            mySql += "</td>";

            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            //
            mySql += "<tr>";
            mySql += "<td colspan=7 width= 100% align=left valign=top height=19 ><font size=2><b>Amount in Words:</b> " + " " + result + "</font></td>";
            mySql += "</tr>";

            //
            //mySql += "<tr><td colspan=7 align=left valign=top height=19 ><font size=2>" + "&nbsp" + "</font></td></tr>";

            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td colspan=4 align=left valign=top height=17 ><font size=2>Tax is payable on Reverse Charge.</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=center valign=top height=17 ><font size=2>Taxes</font></td>";
            mySql += "<td width= 10% align=center valign=top height=17 ><font size=2>Rate of Tax</font></td>";
            mySql += "<td width= 10% align=center valign=top height=17 ><font size=2>Taxable Value</font></td>";
            mySql += "<td width= 10% align=center valign=top height=17 ><font size=2>Tax Amount</font></td>";
            mySql += "</tr>";
            mySql += "<tr>";
            mySql += "<td width= 10% align=center valign=top height=17 ><font size=2>-</font></td>";
            mySql += "<td width= 10% align=center valign=top height=17 ><font size=2>-</font></td>";
            mySql += "<td width= 10% align=center valign=top height=17 ><font size=2>-</font></td>";
            mySql += "<td width= 10% align=center valign=top height=17 ><font size=2>-</font></td>";
            mySql += "</tr>";
            mySql += "</table>";
            mySql += "</td>";
            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";
            //


            //mySql += "<tr>";
            //mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2><b><i>Please issue the Cheque/D.D. in the name of Durocrete Engineering Services Pvt. Ltd..</i></b></font></td>";
            //mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td colspan=7 width= 10% align=left valign=top height=17 ><font size=2><b>Complaint/s against this invoice if any shall be made within 7 days from the date of invoice. Subject to Pune Jurisdiction</b></font></td>";
            mySql += "</tr>";

            string cnStr = System.Configuration.ConfigurationManager.AppSettings["conStr"].ToString();
            //mySql += "<tr>";
            //if (cnStr.ToLower().Contains("mumbai") == true)
            //{   
            //    mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2><b>For all technical queries contact on 9850500013.</b></font></td>";
            //}
            //else if (cnStr.ToLower().Contains("nashik") == true)
            //{
            //    mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2><b>For all technical queries contact on 9527005478.</b></font></td>";
            //}
            //else 
            //{
            //    mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2><b>For all technical queries contact on (020)24348027.</b></font></td>";
            //}
            //mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td colspan=7 width= 10% align=left valign=top height=17 ><font size=2><b>For NEFT/RTGS : </b>  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Beneficiary Name : </b> Durocrete Engineering Services Pvt. Ltd. &nbsp;&nbsp;&nbsp;&nbsp;<b>Bank Name : </b> HDFC Bank Ltd.</font></td>";
            mySql += "</tr>";

            //mySql += "<tr>";
            //mySql += "<td colspan=4 width= 10% align=left valign=top height=17 ><font size=2><b>Beneficiary Name : </b> Durocrete Engineering Services Pvt. Ltd.</font></td>";
            //mySql += "<td colspan=3 width= 10% align=left valign=top height=17 ><font size=2><b>Bank Name : </b> HDFC Bank Ltd.</font></td>";
            //mySql += "<td> &nbsp; </td>";
            //mySql += "</tr>";
            if (cnStr.ToLower().Contains("mumbai") == true)
            {
                mySql += "<tr>";
                mySql += "<td colspan=4 width= 10% align=left valign=top height=17 ><font size=2><b>Branch : </b> Vashi, Navi Mumbai  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>IFSC Code : </b> HDFC0000540</font></td>";
                mySql += "<td colspan=3 width= 10% align=left valign=top height=17 ><font size=2><b>Account No : </b> 05402000024568</font></td>";
                mySql += "</tr>";

                //mySql += "<tr>";
                //mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2></font></td>";
                //mySql += "</tr>";
            }
            else if (cnStr.ToLower().Contains("nashik") == true)
            {
                mySql += "<tr>";
                mySql += "<td colspan=4 width= 10% align=left valign=top height=17 ><font size=2><b>Branch : </b> Bhandarkar Road, Pune &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>IFSC Code : </b> HDFC0000007</font></td>";
                mySql += "<td colspan=3 width= 10% align=left valign=top height=17 ><font size=2><b>Account No : </b> 50200023762951</font></td>";
                mySql += "</tr>";

                //mySql += "<tr>";
                //mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2></font></td>";
                //mySql += "</tr>";
            }
            else
            {
                mySql += "<tr>";
                mySql += "<td colspan=4 width= 10% align=left valign=top height=17 ><font size=2><b>Branch : </b> Hingane Khurd, Pune  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b> IFSC Code :</b> HDFC0000825</font></td>";
                mySql += "<td colspan=3 width= 10% align=left valign=top height=17 ><font size=2><b>Account No : </b> 08252000000218</font></td>";
                mySql += "</tr>";

                //mySql += "<tr>";
                //mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2><b></font></td>";
                //mySql += "</tr>";
            }

            string tmpString = "";
            tmpString = "<b>Declaration :  </b>I / we certify that our registration certificate under the GST Act, 2017 is in force on the date on which the supply of goods specified in this Tax Invoice is made by " +
                "me/us & the transaction of supply covered by this Tax Invoice had been effected by me/us & it shall be accounted for in the turnover of supplies while filing of " +
                "return & the due tax if any payable on the supplies has been paid or shall be paid. Further certified that the particulars given above are true and correct & the " +
                "amount indicated represents the prices the actually charged and that there is no flow if additional consideration directly or indirectly from the buyer. Interest @ 18% " +
                "p.a. charged on all A/c outstanding more than one month after invoice has been rendered.";

            mySql += "<tr>";
            mySql += "<td colspan=7 width= 10% align=left valign=top height=15 ><font size=1.5>" + tmpString + "</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td colspan=7 width= 10% align=left valign=top height=15 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td colspan=7 width= 10% align=left valign=top height=15 ><font size=2>&nbsp;</font></td>";
            mySql += "</tr>";

            mySql += "<tr>";
            mySql += "<td colspan=4 width=100% align=left valign=top height=19 ><font size=2><b>Receiver's Signature</b></font></td>";
            mySql += "<td colspan=3 width=100% align=left valign=top height=19 ><font size=2><b>Authorized Sign</b></font></td>";
            mySql += "</tr>";

            //mySql += "<tr>";
            //mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=2>All our invoices are payable immediately, Delay of payment beyond 30 days from the date of billing shall attract an interest of 2% per month on the outstanding amount.</font></td>";
            //mySql += "</tr>";

            if (cnStr.ToLower().Contains("mumbai") == true || cnStr.ToLower().Contains("nashik") == true)
            {
                mySql += "<tr>";
                mySql += "<td colspan=3 width= 10% align=left valign=top height=19 ><font size=2>CIN - U28939PN1999PTC014212.</font></td>";
                mySql += "<td colspan=4 width= 20% align=left valign=top height=19 ><font size=1>Regd.Add - 1160/5, Gharpure Colony Shivaji Nagar,Pune 411005,Maharashtra India.</font></td>";
                mySql += "</tr>";
                //mySql += "<tr>";
                //mySql += "<td colspan=7 width= 10% align=left valign=top height=19 ><font size=1>Regd.Add - 1160/5, Gharpure Colony Shivaji Nagar,Pune 411005,Maharashtra India.</font></td>";
                //mySql += "</tr>";
            }
            mySql += "</table>";
            //mySql += "</html>";
            return mySql;

        }

        public void getBillPrintString(string BillNo, bool duplicateBillFlg)
        {

            string mySql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            DateTime BillDate = DateTime.Now;
            var b = dc.Bill_View(BillNo, 0, 0, "", 0, false, false, null, null);
            foreach (var bl in b)
            {
                BillDate = Convert.ToDateTime(bl.BILL_Date_dt);
            }
            //if (BillDate < DateTime.ParseExact("01/07/2017", "dd/MM/yyyy", null))
            if (CheckGSTFlag(BillDate) == false)
            {
                mySql += getBillString(BillNo, duplicateBillFlg);
            }
            else
            {
                mySql += getBillStringWithGST(BillNo, duplicateBillFlg);
            }

            mySql += "</html>";

            string reportPath;
            string reportStr = mySql;
            StreamWriter sw;

            string strFileName = "Bill_" + BillNo.Replace("/", "") + "_" + System.Web.HttpContext.Current.Session["LoginId"] + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".html";
            reportPath = @"C:/temp/" + strFileName;
            sw = File.CreateText(reportPath);
            sw.WriteLine(reportStr);
            sw.Close();
            System.Web.HttpContext.Current.Response.ContentType = "text/HTML";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileName);
            System.Web.HttpContext.Current.Response.TransmitFile(reportPath);
            System.Web.HttpContext.Current.Response.End();
        }
        public void getMonthlyBillDetailPrint(string BillNo)
        {
            string mySql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            //////////////////////////////////////////////////////
            mySql += "<tr><td width='100%' height='105'>";
            mySql += "&nbsp;</td></tr>";

            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Tax Invoice Detail</b></font></td></tr>";

            mySql += "<tr>" +
                "<td width='13%' height=19 align=left valign=top><font size=2><b>Tax Invoice No.</b></font></td>" +
                "<td width='1%' height=19><font size=2>:</font></td>" +
                "<td width='35%' height=19 align=left valign=top><font size=2>" + BillNo + "</font></td>" +
                "<td width='49%' height=19 colspan=3><font size=2></font></td>" +
                "<td width='1%' height=19><font size=2>&nbsp;</font></td>" +
                "</tr>";

            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Sr.No.</b></font></td>";
            mySql += "<td width= 40% align=center valign=top height=19 ><font size=2><b>Particular</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>SAC Code</b></font></td>";
            mySql += "<td width= 8% align=center valign=top height=19 ><font size=2><b>Quantity</b></font></td>";
            mySql += "<td width= 8% align=center valign=top height=19 ><font size=2><b>Rate</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Amount</b></font></td>";
            mySql += "<td width= 8% align=center valign=top height=19 ><font size=2><b>Discount (%)</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Discounted Amount</b></font></td>";
            mySql += "</tr>";

            decimal billTotal = 0, actualTotal = 0;
            string prevRefNo = "";
            bool starNoteFlag = false;
            var bd = dc.BillDetailMonthly_View(BillNo);
            foreach (var bill in bd)
            {
                if (prevRefNo != bill.MBILLD_ReferenceNo_int.ToString())
                {
                    mySql += "<tr>";
                    mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "<td width= 10% align=left valign=top height=19><font size=2><b>&nbsp; Reference No. : " + bill.MBILLD_ReferenceNo_int.ToString() + " - Received Date : " + Convert.ToDateTime(bill.MBILLD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</b></font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "<td width= 8% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "<td width= 8% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "<td width= 8% align=right valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "</tr>";
                }
                mySql += "<tr>";
                mySql += "<td width= 5% align=center valign=top height=19 ><font size=2>&nbsp;" + bill.MBILLD_SrNo_int + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + bill.MBILLD_TEST_Name_var + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + bill.MBILLD_SACCode_var + "</font></td>";
                mySql += "<td width= 8% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_Quantity_int).ToString("0.##") + "</font></td>";
                if (bill.MBILLD_ActualRate_num != null && bill.MBILLD_ActualRate_num > 0)
                {
                    mySql += "<td width= 8% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_ActualRate_num).ToString("0.00") + "</font></td>";
                    decimal actualAmount = 0;
                    actualAmount = Convert.ToDecimal(bill.MBILLD_Quantity_int * bill.MBILLD_ActualRate_num);
                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + actualAmount.ToString("0.00") + "</font></td>";
                    actualTotal += actualAmount;
                    if (bill.MBILLD_AppDiscStatus_bit == true)
                    {
                        mySql += "<td width= 8% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_DiscountPer_num).ToString("0.##") + "*" + "</font></td>";
                        starNoteFlag = true;
                    }
                    else
                    {
                        mySql += "<td width= 8% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_DiscountPer_num).ToString("0.##") + "</font></td>";
                    }
                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_Amt_num).ToString("0.00") + "</font></td>";
                }
                else
                {
                    mySql += "<td width= 8% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_Rate_num).ToString("0.00") + "</font></td>";
                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_Amt_num).ToString("0.00") + "</font></td>";
                    mySql += "<td width= 8% align=right valign=top height=19 ><font size=2>&nbsp;" + "0" + "</font></td>";
                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_Amt_num).ToString("0.00") + "</font></td>";
                }
                mySql += "</tr>";
                prevRefNo = bill.MBILLD_ReferenceNo_int.ToString();
                billTotal = billTotal + Convert.ToDecimal(bill.MBILLD_Amt_num);
            }
            mySql += "<tr>" +
                        "<td height=19 align=right colspan=5><font size=2>Total Amount &nbsp;</font></td>" +
                        "<td height=19 align=right><font size=2>" + actualTotal + "</font></td>" +
                        "<td height=19 align=right><font size=2>" + " " + "</font></td>" +
                        "<td height=19 align=right><font size=2>" + billTotal + "</font></td>" +
                        "</tr>";

            mySql += "</table>";
            mySql += "</td>";

            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";

            if (starNoteFlag == true)
            {

                mySql += "<tr>" +
                        "<td height=19 align=left colspan=7><font size=2>&nbsp; </font></td>" +
                        "</tr>";

                mySql += "<tr>" +
                        "<td height=19 align=left colspan=7><font size=2><b>Notes : </b></font></td>" +
                        "</tr>";

                mySql += "<tr>" +
                        "<td height=19 align=left colspan=7><font size=2>1) * includes additional 5 % discount for placing enquiry through Durocrete App. &nbsp;</font></td>" +
                        "</tr>";
            }

            /////////////////////////////////////////////////////
            mySql += "</html>";

            string reportPath;
            string reportStr = mySql;
            StreamWriter sw;

            string strFileName = "MonthlyBillDetail_" + BillNo.Replace("/", "") + "_" + System.Web.HttpContext.Current.Session["LoginId"] + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".html";
            reportPath = @"C:/temp/" + strFileName;
            sw = File.CreateText(reportPath);
            sw.WriteLine(reportStr);
            sw.Close();
            System.Web.HttpContext.Current.Response.ContentType = "text/HTML";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileName);
            System.Web.HttpContext.Current.Response.TransmitFile(reportPath);
            System.Web.HttpContext.Current.Response.End();
        }
        public void getMonthlyBillDetailPrint_old22052020(string BillNo)
        {
            string mySql = "";
            mySql += "<html>";
            mySql += "<head>";
            mySql += "<style type='text/css'>";
            mySql += "body {margin-left:2em margin-right:2em}";
            mySql += "</style>";
            mySql += "</head>";
            //////////////////////////////////////////////////////
            mySql += "<tr><td width='100%' height='105'>";
            mySql += "&nbsp;</td></tr>";

            mySql += "<table border=0 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";

            mySql += "<tr><td width='99%' colspan=7 height=60>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 height=19>&nbsp;</td></tr>";

            mySql += "<tr><td width='99%' colspan=7 align=center valign=top height=19><font size=4><b>Tax Invoice Detail</b></font></td></tr>";

            mySql += "<tr>" +
                "<td width='13%' height=19 align=left valign=top><font size=2><b>Tax Invoice No.</b></font></td>" +
                "<td width='1%' height=19><font size=2>:</font></td>" +
                "<td width='35%' height=19 align=left valign=top><font size=2>" + BillNo + "</font></td>" +
                "<td width='49%' height=19 colspan=3><font size=2></font></td>" +
                "<td width='1%' height=19><font size=2>&nbsp;</font></td>" +
                "</tr>";

            mySql += "<tr><td colspan=6 align=left valign=top>";
            mySql += "<table border=1 cellpadding=0 cellspacing=0 style=border-collapse: collapse bordercolor=#111111 width=100% id=AutoNumber1>";
            mySql += "<tr>";
            mySql += "<td width= 5% align=center valign=top height=19 ><font size=2><b>Sr.No.</b></font></td>";
            mySql += "<td width= 50% align=center valign=top height=19 ><font size=2><b>Particular</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>SAC Code</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Quantity</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Actual Rate</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Discount (%)</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Discounted Rate</b></font></td>";
            mySql += "<td width= 10% align=center valign=top height=19 ><font size=2><b>Amount</b></font></td>";
            mySql += "</tr>";

            decimal billTotal = 0;
            string prevRefNo = "";
            bool starNoteFlag = false;
            var bd = dc.BillDetailMonthly_View(BillNo);
            foreach (var bill in bd)
            {
                if (prevRefNo != bill.MBILLD_ReferenceNo_int.ToString())
                {
                    mySql += "<tr>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "<td width= 10% align=left valign=top height=19><font size=2><b>&nbsp; Reference No. : " + bill.MBILLD_ReferenceNo_int.ToString() + " - Received Date : " + Convert.ToDateTime(bill.MBILLD_ReceivedDate_dt).ToString("dd/MM/yyyy") + "</b></font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "</font></td>";
                    mySql += "</tr>";
                }
                mySql += "<tr>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + bill.MBILLD_SrNo_int + "</font></td>";
                mySql += "<td width= 10% align=left valign=top height=19 ><font size=2>&nbsp;" + bill.MBILLD_TEST_Name_var + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + bill.MBILLD_SACCode_var + "</font></td>";
                mySql += "<td width= 10% align=center valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_Quantity_int).ToString("0.##") + "</font></td>";
                if (bill.MBILLD_ActualRate_num != null && bill.MBILLD_ActualRate_num > 0)
                {
                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_ActualRate_num).ToString("0.00") + "</font></td>";
                    if (bill.MBILLD_AppDiscStatus_bit == true)
                    {
                        mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_DiscountPer_num).ToString("0.##") + "*" + "</font></td>";
                        starNoteFlag = true;
                    }
                    else
                    {
                        mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_DiscountPer_num).ToString("0.##") + "</font></td>";
                    }
                }
                else
                {
                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_Rate_num).ToString("0.00") + "</font></td>";
                    mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + "0" + "</font></td>";
                }
                mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_Rate_num).ToString("0.00") + "</font></td>";
                mySql += "<td width= 10% align=right valign=top height=19 ><font size=2>&nbsp;" + Convert.ToDecimal(bill.MBILLD_Amt_num).ToString("0.00") + "</font></td>";
                mySql += "</tr>";
                prevRefNo = bill.MBILLD_ReferenceNo_int.ToString();
                billTotal = billTotal + Convert.ToDecimal(bill.MBILLD_Amt_num);
            }
            mySql += "<tr>" +
                        "<td height=19 align=right colspan=7><font size=2>Total Amount &nbsp;</font></td>" +
                        "<td height=19 align=right><font size=2>" + billTotal + "</font></td>" +
                        "</tr>";

            mySql += "</table>";
            mySql += "</td>";

            mySql += "<td>&nbsp;</td>";
            mySql += "</tr>";

            if (starNoteFlag == true)
            {

                mySql += "<tr>" +
                        "<td height=19 align=left colspan=7><font size=2>&nbsp; </font></td>" +
                        "</tr>";

                mySql += "<tr>" +
                        "<td height=19 align=left colspan=7><font size=2><b>Notes : </b></font></td>" +
                        "</tr>";

                mySql += "<tr>" +
                        "<td height=19 align=left colspan=7><font size=2>1) * includes additional 5 % discount for placing enquiry through Durocrete App. &nbsp;</font></td>" +                        
                        "</tr>";
            }

            /////////////////////////////////////////////////////
            mySql += "</html>";

            string reportPath;
            string reportStr = mySql;
            StreamWriter sw;

            string strFileName = "MonthlyBillDetail_" + BillNo.Replace("/", "") + "_" + System.Web.HttpContext.Current.Session["LoginId"] + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss") + ".html";
            reportPath = @"C:/temp/" + strFileName;
            sw = File.CreateText(reportPath);
            sw.WriteLine(reportStr);
            sw.Close();
            System.Web.HttpContext.Current.Response.ContentType = "text/HTML";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + strFileName);
            System.Web.HttpContext.Current.Response.TransmitFile(reportPath);
            System.Web.HttpContext.Current.Response.End();
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
