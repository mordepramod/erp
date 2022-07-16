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
    public partial class GTSummary : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        string[] dmNoArray = new string[1];
        string[] operatorArray = new string[1];
        string[] ownerArray = new string[1];
        string[] newarryForGTSummary = new string[1];
        string[] DMTempArray = new string[1];
        string[] FromDateArray = new string[1];
        string[] ToDateArray = new string[1];
        bool flag = false;
        int arryCount = 0;
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strReq = "";
                strReq = Request.RawUrl;
                strReq = strReq.Substring(strReq.IndexOf('?') + 1);
                if (!strReq.Equals(""))
                {
                    strReq = obj.Decrypt(strReq);
                    if ( strReq.Contains("=") == false)
                    {
                        Session.Abandon();
                        Response.Redirect("Login.aspx");
                    }
                    string[] arrMsgs = strReq.Split('&');
                    string[] arrIndMsg;

                    arrIndMsg = arrMsgs[2].Split('=');
                    txtRefNo.Text = arrIndMsg[1].ToString().Trim();
                   
                }

                Label lblheading = (Label)Master.FindControl("lblheading");
                lblheading.Text = "GT Summary";

                //txtRefNo.Text = Session["ReferenceNo"].ToString();
                
                RdbDataForList.SelectedValue = "Enquiry Details";
                AddRowGTSummary();
                AddRowWorkVisit();
                AddRowNotes();
                DisplayGTSummary();
            }
        }

        private string[] ArrayResizeMethod(string[] newarray)
        {
            Array.Resize(ref newarray, newarray.Length + 1);
            return newarray;
        }

        private void DisplayGTSummary()
        {
            ////Remark details
            int rowNo = 0;
            if (RdbDataForList.SelectedValue == "Enquiry Details")
            {
                #region Enquiry Details

                pnlIndividualSummary.Visible = false;
                pnlGTSummaryExpenses.Visible = false;
                PnlEnq.Visible = true;
                pnlWorkVisit.Visible = false;
                pnlNotes.Visible = false;
                pnlMachineLog.Visible = false;
                pnlOperatorPayment.Visible = false;

                var gtData = dc.ReportStatus_View("Soil Investigation", null, null, 0, 0, 0, txtRefNo.Text, 0, 0, 0);
                foreach (var gt in gtData)
                {
                    txtClientmName.Text = gt.CL_Name_var;
                    txtSiteName.Text = gt.SITE_Name_var;
                    txtAddress.Text = gt.SITE_Address_var;
                    var loc = dc.Location_View(gt.ENQ_LOCATION_Id ,"",0);
                    foreach (var locid in loc)
                    {
                        txtLocation.Text = locid.LOCATION_Name_var;
                    }
                    txtContactPerson.Text = gt.CONT_Name_var;
                    txtContactNo.Text = gt.CONT_ContactNo_var;
                    txtReference.Text = gt.ENQ_Reference_var;
                    txtReportNo.Text = gt.GTINW_SetOfRecord_var;
                    txtBillNo.Text = gt.INWD_BILL_Id.ToString();
                    var b = dc.Bill_View(gt.INWD_BILL_Id, 0, 0, "", 0, false, false, null, null);
                    foreach (var bill in b)
                    {
                        DateTime dt1=new DateTime();
                        dt1 = Convert.ToDateTime(bill.BILL_Date_dt);
                        txtBillDate.Text = dt1.ToString("dd/MMM/yyyy");
                        txtYear.Text = dt1.ToString("yyyy");
                        txtMonth.Text = dt1.ToString("MMM");
                        txtAmountTax.Text = (bill.BILL_NetAmt_num - bill.BILL_SerTaxAmt_num).ToString();
                        txtServiceTax.Text = bill.BILL_SerTaxAmt_num.ToString();
                        txtTotalAmount.Text = bill.BILL_NetAmt_num.ToString();
                    }
                    txtProposalNo.Text = "NA";
                    txtDate.Text = "NA";
                }
                
                var gtSummaryData = dc.GTSummary_View(txtRefNo.Text, "Enquiry Details").ToList();
                foreach (var gtSummary in gtSummaryData)
                {
                    string[] arryForGTSummary = gtSummary.GTSUM_Summary_var.Split('~');
                    for (int i = 0; i < arryForGTSummary.Length - 1; i++)
                    {
                        string[] arryForEnquiry = arryForGTSummary[i].Split('#');
                        ddlStatus.SelectedItem.Text = arryForEnquiry[0];
                        txtEmail.Text = arryForEnquiry[1];
                        txtDrilling.Text = arryForEnquiry[2];
                        txtCharges.Text = arryForEnquiry[3];
                    }
                }
                #endregion
            }
            if (RdbDataForList.SelectedValue == "Expenses")
            {
                #region Expenses
                pnlIndividualSummary.Visible = false;
                pnlGTSummaryExpenses.Visible = true;
                PnlEnq.Visible = false;
                pnlWorkVisit.Visible = false;
                pnlNotes.Visible = false;
                pnlMachineLog.Visible = false;
                pnlOperatorPayment.Visible = false;
                var gtSummaryData = dc.GTSummary_View(txtRefNo.Text, "Expenses").ToList();
                grdGTSummary.DataSource = null;
                grdGTSummary.DataBind();
                foreach (var gtSummary in gtSummaryData)
                {
                    string[] arryForGTSummary = gtSummary.GTSUM_Summary_var.Split('~');
                    for (int i = 0; i < arryForGTSummary.Length - 1; i++)
                    {
                        AddRowGTSummary();
                        TextBox txtParticular = (TextBox)grdGTSummary.Rows[rowNo].FindControl("txtParticular");
                        TextBox txtAmount = (TextBox)grdGTSummary.Rows[rowNo].FindControl("txtAmount");
                        string[] arryForGTSummaryResult = arryForGTSummary[i].Split('#');
                        txtParticular.Text = arryForGTSummaryResult[0];
                        txtAmount.Text = arryForGTSummaryResult[1];
                        rowNo++;
                    }
                }
                CalculateAmount();
                if (rowNo == 0)
                    AddRowGTSummary();
                #endregion
            }
            else if (RdbDataForList.SelectedValue == "Individual BH - Summary")
            {
                #region Individual BH - Summary
                pnlIndividualSummary.Visible = true;
                pnlGTSummaryExpenses.Visible = false;
                PnlEnq.Visible = false;
                pnlWorkVisit.Visible = false;
                pnlNotes.Visible = false;
                pnlMachineLog.Visible = false;
                pnlOperatorPayment.Visible = false;
                var gtSummaryData = dc.GTSummary_View(txtRefNo.Text, "Individual BH - Summary").ToList();
                grdIndividualSummary.DataSource = null;
                grdIndividualSummary.DataBind();
                foreach (var gtSummary in gtSummaryData)
                {
                    string[] arryForGTSummary = gtSummary.GTSUM_Summary_var.Split('~');
                    for (int i = 0; i < arryForGTSummary.Length - 1; i++)
                    {
                        AddRowIS();
                        TextBox txtBHNo = (TextBox)grdIndividualSummary.Rows[rowNo].FindControl("txtBHNo");
                        DropDownList ddlDrillingMachineNo = (DropDownList)grdIndividualSummary.Rows[rowNo].FindControl("ddlDrillingMachineNo");
                        TextBox txtMachineOperator = (TextBox)grdIndividualSummary.Rows[rowNo].FindControl("txtMachineOperator");
                        TextBox txtfromDate = (TextBox)grdIndividualSummary.Rows[rowNo].FindControl("txtfromDate");
                        TextBox txttoDate = (TextBox)grdIndividualSummary.Rows[rowNo].FindControl("txttoDate");
                        TextBox txtTotalDepth = (TextBox)grdIndividualSummary.Rows[rowNo].FindControl("txtTotalDepth");
                        TextBox txtByDurocreteOth = (TextBox)grdIndividualSummary.Rows[rowNo].FindControl("txtByDurocreteOth");

                        string[] arryForGTSummaryResult = arryForGTSummary[i].Split('#');
                        txtBHNo.Text = arryForGTSummaryResult[0];
                        //ddlDrillingMachineNo.SelectedValue = arryForGTSummaryResult[1];
                        ddlDrillingMachineNo.SelectedValue = arryForGTSummaryResult[2] + "|" + arryForGTSummaryResult[6];
                        txtMachineOperator.Text = arryForGTSummaryResult[2];
                        txtfromDate.Text = arryForGTSummaryResult[3];
                        txttoDate.Text = arryForGTSummaryResult[4];
                        txtTotalDepth.Text = arryForGTSummaryResult[5];
                        txtByDurocreteOth.Text = arryForGTSummaryResult[6];
                        rowNo++;
                    }
                    txtBoreHoles.Text = grdIndividualSummary.Rows.Count.ToString();
                }
                if (rowNo == 0)
                    AddRowIS();
                #endregion
            }
            else if (RdbDataForList.SelectedValue == "Machine Log")
            {
                #region Machine Log
                pnlIndividualSummary.Visible = false;
                pnlGTSummaryExpenses.Visible = false;
                PnlEnq.Visible = false;
                pnlWorkVisit.Visible = false;
                pnlNotes.Visible = false;
                pnlMachineLog.Visible = true;
                pnlOperatorPayment.Visible = false;
                var gtSummaryData = dc.GTSummary_View(txtRefNo.Text, "Individual BH - Summary").ToList();
                foreach (var gtSummary in gtSummaryData)
                {
                    string[] arryForGTSummary = gtSummary.GTSUM_Summary_var.Split('~');
                    int j = 0;
                    for (int i = 0; i < arryForGTSummary.Length - 1; i++)
                    {
                        string[] arryForGTSummaryResult = arryForGTSummary[i].Split('#');
                        dmNoArray[j] = arryForGTSummaryResult[1];
                        dmNoArray = ArrayResizeMethod(dmNoArray);
                        j++;
                        newarryForGTSummary[i] = arryForGTSummaryResult[1] + "|" + arryForGTSummaryResult[2] + "|" + arryForGTSummaryResult[3] + "|" + arryForGTSummaryResult[4] + "|" + arryForGTSummaryResult[6];
                        newarryForGTSummary = ArrayResizeMethod(newarryForGTSummary);
                    }

                    for (int i = 0; i < newarryForGTSummary.Length - 1; i++)
                    {
                        try
                        {
                            if (i == 0)
                            {
                                string[] tempA = newarryForGTSummary[i].Split('|');
                                DMTempArray[0] = tempA[0];
                                Array.Resize(ref DMTempArray, DMTempArray.Length + 1);
                                operatorArray[arryCount] = tempA[1];
                                ownerArray[arryCount] = tempA[4];
                                FromDateArray[arryCount] = tempA[2] + "|" + tempA[0];
                                ToDateArray[arryCount] = tempA[3] + "|" + tempA[0];
                                FromDateArray = ArrayResizeMethod(FromDateArray);
                                ToDateArray = ArrayResizeMethod(ToDateArray);
                                operatorArray = ArrayResizeMethod(operatorArray);
                                ownerArray = ArrayResizeMethod(ownerArray);
                                arryCount++;
                            }
                            else
                            {
                                string[] tempA = newarryForGTSummary[i].Split('|');
                                for (int k = 0; k < DMTempArray.Length; k++)
                                {
                                    if (DMTempArray[k] != null)
                                    {
                                        if (tempA[0] == DMTempArray[k])
                                        {
                                            for (int h = 0; h < FromDateArray.Length - 1; h++)
                                            {
                                                if (FromDateArray[k].Contains(tempA[0]))
                                                {
                                                    string[] temp1FromDate = FromDateArray[k].Split('/');
                                                    string[] temp2fromDate = tempA[2].Split('/');
                                                    temp1FromDate[2] = temp1FromDate[2].Remove(4);
                                                    if (Convert.ToInt32(temp2fromDate[2]) <= Convert.ToInt32(temp1FromDate[2]))
                                                    {
                                                        if (Convert.ToInt32(temp2fromDate[2]) < Convert.ToInt32(temp1FromDate[2]))
                                                        {
                                                            FromDateArray[k] = tempA[2] + "|" + tempA[0];
                                                            break;
                                                        }
                                                        else if (Convert.ToInt32(temp2fromDate[1]) < Convert.ToInt32(temp1FromDate[1]))
                                                        {
                                                            FromDateArray[k] = tempA[2] + "|" + tempA[0];
                                                            break;
                                                        }
                                                        else if (Convert.ToInt32(temp2fromDate[0]) < Convert.ToInt32(temp1FromDate[0]))
                                                        {
                                                            FromDateArray[k] = tempA[2] + "|" + tempA[0];
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            for (int h = 0; h < ToDateArray.Length - 1; h++)
                                            {
                                                if (ToDateArray[k].Contains(tempA[0]))
                                                {
                                                    string[] temp1ToDate = ToDateArray[k].Split('/');
                                                    string[] temp2ToDate = tempA[3].Split('/');
                                                    temp1ToDate[2] = temp1ToDate[2].Remove(4);
                                                    if (Convert.ToInt32(temp2ToDate[2]) >= Convert.ToInt32(temp1ToDate[2]))
                                                    {
                                                        if (Convert.ToInt32(temp2ToDate[2]) > Convert.ToInt32(temp1ToDate[2]))
                                                        {
                                                            ToDateArray[k] = tempA[3] + "|" + tempA[0];
                                                            break;
                                                        }
                                                        else if (Convert.ToInt32(temp2ToDate[1]) > Convert.ToInt32(temp1ToDate[1]))
                                                        {
                                                            ToDateArray[k] = tempA[3] + "|" + tempA[0];
                                                            break;
                                                        }
                                                        else if (Convert.ToInt32(temp2ToDate[0]) > Convert.ToInt32(temp1ToDate[0]))
                                                        {
                                                            ToDateArray[k] = tempA[3] + "|" + tempA[0];
                                                            break;
                                                        }
                                                    }
                                                }
                                            }

                                            flag = true;
                                            break;
                                        }

                                    }
                                }
                                if (flag == false)
                                {
                                    int dmIndex = DMTempArray.Length - 1;
                                    DMTempArray[dmIndex] = tempA[0];
                                    operatorArray[arryCount] = tempA[1];
                                    ownerArray[arryCount] = tempA[4];
                                    operatorArray = ArrayResizeMethod(operatorArray);
                                    ownerArray = ArrayResizeMethod(ownerArray);
                                    DMTempArray = ArrayResizeMethod(DMTempArray);
                                    int fromindex = FromDateArray.Length - 1;
                                    FromDateArray[fromindex] = tempA[2] + "|" + tempA[0];
                                    FromDateArray = ArrayResizeMethod(FromDateArray);
                                    int toindex = ToDateArray.Length - 1;
                                    ToDateArray[toindex] = tempA[3] + "|" + tempA[0];
                                    ToDateArray = ArrayResizeMethod(ToDateArray);
                                    arryCount++;
                                }
                                else if (flag == true)
                                {
                                    flag = false;
                                }
                            }
                        }
                        catch
                        { }
                    }
                    var dict = new Dictionary<string, int>();

                    foreach (var value in dmNoArray)
                    {
                        if (value != null)
                        {
                            if (dict.ContainsKey(value))
                                dict[value]++;
                            else
                                dict[value] = 1;
                        }
                    }
                    rowNo = 0;
                    DataTable dt = new DataTable();
                    DataColumn dtcolumn = new DataColumn();

                    for (int i = 0; i < dict.Count; i++)
                    {
                        if (dt.Columns.Count == 0)
                        {
                            dt.Columns.Add("txtMachineNo", typeof(string));
                            dt.Columns.Add("txtNoBH", typeof(string));
                            dt.Columns.Add("txtDate", typeof(string));
                            dt.Columns.Add("txtOperator", typeof(string));
                            dt.Columns.Add("txtOwner", typeof(string));
                        }

                        DataRow NewRow = dt.NewRow();
                        dt.Rows.Add(NewRow);
                        grdMachineLog.DataSource = dt;
                        grdMachineLog.DataBind();
                    }

                    foreach (var pair in dict)
                    {
                        TextBox txtMachineNo = (TextBox)grdMachineLog.Rows[rowNo].FindControl("txtMachineNo");
                        TextBox txtNoBH = (TextBox)grdMachineLog.Rows[rowNo].FindControl("txtNoBH");
                        TextBox txtOperator = (TextBox)grdMachineLog.Rows[rowNo].FindControl("txtOperator");
                        TextBox txtOwner = (TextBox)grdMachineLog.Rows[rowNo].FindControl("txtOwner");
                        TextBox txtDate = (TextBox)grdMachineLog.Rows[rowNo].FindControl("txtDate");

                        FromDateArray[rowNo] = FromDateArray[rowNo].Remove(10);
                        ToDateArray[rowNo] = ToDateArray[rowNo].Remove(10);
                        txtDate.Text = FromDateArray[rowNo] + "  To  " + ToDateArray[rowNo];
                        txtMachineNo.Text = pair.Key;
                        txtNoBH.Text = pair.Value.ToString();
                        txtOperator.Text = operatorArray[rowNo];
                        txtOwner.Text = ownerArray[rowNo];
                        rowNo++;
                    }
                }
                #endregion
            }
            else if (RdbDataForList.SelectedValue == "Work Visit")
            {
                #region Work Visit
                pnlIndividualSummary.Visible = false;
                pnlGTSummaryExpenses.Visible = false;
                PnlEnq.Visible = false;
                pnlWorkVisit.Visible = true;
                pnlNotes.Visible = false;
                pnlMachineLog.Visible = false;
                pnlOperatorPayment.Visible = false;
                var gtSummaryData = dc.GTSummary_View(txtRefNo.Text, "Work Visit").ToList();
                grdWorkVisit.DataSource = null;
                grdWorkVisit.DataBind();
                foreach (var gtSummary in gtSummaryData)
                {
                    string[] arryForGTSummary = gtSummary.GTSUM_Summary_var.Split('~');
                    for (int i = 0; i < arryForGTSummary.Length - 1; i++)
                    {
                        AddRowWorkVisit();
                        TextBox txtVisitOnDate = (TextBox)grdWorkVisit.Rows[rowNo].FindControl("txtVisitOnDate");
                        TextBox txtVisitBy = (TextBox)grdWorkVisit.Rows[rowNo].FindControl("txtVisitBy");
                        TextBox txtPurpose = (TextBox)grdWorkVisit.Rows[rowNo].FindControl("txtPurpose");

                        string[] arryForGTSummaryResult = arryForGTSummary[rowNo].Split('#');
                        txtVisitOnDate.Text = arryForGTSummaryResult[0];
                        txtVisitBy.Text = arryForGTSummaryResult[1];
                        txtPurpose.Text = arryForGTSummaryResult[2];
                        rowNo++;
                    }
                }
                if (rowNo == 0)
                    AddRowWorkVisit();
                #endregion
            }
            else if (RdbDataForList.SelectedValue == "Notes")
            {
                #region Notes
                pnlIndividualSummary.Visible = false;
                pnlGTSummaryExpenses.Visible = false;
                PnlEnq.Visible = false;
                pnlWorkVisit.Visible = false;
                pnlNotes.Visible = true;
                pnlMachineLog.Visible = false;
                pnlOperatorPayment.Visible = false;
                var gtSummaryData = dc.GTSummary_View(txtRefNo.Text, "Notes").ToList();
                grdNotes.DataSource = null;
                grdNotes.DataBind();
                foreach (var gtSummary in gtSummaryData)
                {
                    string[] arryForGTSummary = gtSummary.GTSUM_Summary_var.Split('~');
                    for (int i = 0; i < arryForGTSummary.Length - 1; i++)
                    {
                        AddRowNotes();
                        TextBox txtRepairs = (TextBox)grdNotes.Rows[rowNo].FindControl("txtRepairs");
                        TextBox txtMissalenious = (TextBox)grdNotes.Rows[rowNo].FindControl("txtMissalenious");

                        string[] arryForGTSummaryResult = arryForGTSummary[i].Split('#');
                        txtRepairs.Text = arryForGTSummaryResult[0];
                        txtMissalenious.Text = arryForGTSummaryResult[1];
                        rowNo++;
                    }
                }
                if (rowNo == 0)
                    AddRowNotes();
                #endregion
            }
            else if (RdbDataForList.SelectedValue == "Operator Payment")
            {
                #region Operator Payment
                pnlIndividualSummary.Visible = false;
                pnlGTSummaryExpenses.Visible = false;
                PnlEnq.Visible = false;
                pnlWorkVisit.Visible = false;
                pnlNotes.Visible = false;
                pnlMachineLog.Visible = false;
                pnlOperatorPayment.Visible = true;
                var gtSummaryData = dc.GTSummary_View(txtRefNo.Text, "Individual BH - Summary").ToList();
                foreach (var gtSummary in gtSummaryData)
                {
                    string[] arryForGTSummary = gtSummary.GTSUM_Summary_var.Split('~');
                    int bhCount = arryForGTSummary.Length - 1;
                    string[] bhArray = new string[1];
                    string[] opNameArray = new string[1];
                    bool bhFlag = false;
                    for (int i = 0; i < arryForGTSummary.Length - 1; i++)
                    {
                        string[] subGTSummary = arryForGTSummary[i].Split('#');
                        if (i == 0)
                        {
                            bhArray[i] = subGTSummary[0] + "|" + subGTSummary[5] + "|" + subGTSummary[2];
                            opNameArray[i] = subGTSummary[2];
                            bhArray = ArrayResizeMethod(bhArray);
                            opNameArray = ArrayResizeMethod(opNameArray);
                        }
                        else
                        {
                            for (int k = 0; k < opNameArray.Length; k++)
                            {
                                if (opNameArray[k] != null)
                                {
                                    if (subGTSummary[2] == opNameArray[k])
                                    {
                                        for (int h = 0; h < bhArray.Length - 1; h++)
                                        {
                                            if (bhArray[k].Contains(subGTSummary[2]))
                                            {
                                                bhArray[k] = bhArray[k] + "~" + subGTSummary[0] + "|" + subGTSummary[5] + "|" + subGTSummary[2];
                                                break;
                                            }
                                        }
                                        bhFlag = true;
                                        break;
                                    }
                                }
                            }
                            if (bhFlag == false)
                            {
                                int bhIndex = bhArray.Length - 1;
                                bhArray[bhIndex] = subGTSummary[0] + "|" + subGTSummary[5] + "|" + subGTSummary[2];
                                bhArray = ArrayResizeMethod(bhArray);
                                int opIndex = opNameArray.Length - 1;
                                opNameArray[opIndex] = subGTSummary[2];
                                opNameArray = ArrayResizeMethod(opNameArray);
                            }
                            else if (bhFlag == true)
                            {
                                bhFlag = false;
                            }
                        }
                    }

                    rowNo = 0;
                    DataTable dt = new DataTable();
                    DataColumn dtcolumn = new DataColumn();

                    for (int i = 0; i < bhCount; i++)
                    {
                        if (dt.Columns.Count == 0)
                        {
                            dt.Columns.Add("txtOperatorName", typeof(string));
                            dt.Columns.Add("txtBHNo", typeof(string));
                            dt.Columns.Add("txtMeter", typeof(string));
                            dt.Columns.Add("txtCalAmt", typeof(string));
                            dt.Columns.Add("txtAdditionalAmt", typeof(string));
                            dt.Columns.Add("txtTotalAmt", typeof(string));
                            dt.Columns.Add("txtApprovedAmt", typeof(string));
                            dt.Columns.Add("txtPrintDate", typeof(string));
                            dt.Columns.Add("ddlPaymentTo", typeof(string));
                        }
                        DataRow NewRow = dt.NewRow();
                        dt.Rows.Add(NewRow);
                        grdOperatorPayment.DataSource = dt;
                        grdOperatorPayment.DataBind();
                    }
                    for (int i = 0; i < bhArray.Length - 1; i++)
                    {
                        if (bhArray[i] != null)
                        {
                            string[] tempBHArray = bhArray[i].Split('~');

                            for (int j = 0; j < tempBHArray.Length; j++)
                            {
                                TextBox txtOperatorName = (TextBox)grdOperatorPayment.Rows[rowNo].FindControl("txtOperatorName");
                                TextBox txtBHNo = (TextBox)grdOperatorPayment.Rows[rowNo].FindControl("txtBHNo");
                                TextBox txtMeter = (TextBox)grdOperatorPayment.Rows[rowNo].FindControl("txtMeter");
                                TextBox txtCalAmt = (TextBox)grdOperatorPayment.Rows[rowNo].FindControl("txtCalAmt");
                                TextBox txtTotalAmt = (TextBox)grdOperatorPayment.Rows[rowNo].FindControl("txtTotalAmt");
                                TextBox txtApprovedAmt = (TextBox)grdOperatorPayment.Rows[rowNo].FindControl("txtApprovedAmt");
                                string[] tempA = tempBHArray[j].Split('|');
                                if (j == 0)
                                {
                                    txtOperatorName.Text = tempA[2];
                                }
                                else
                                {
                                    txtOperatorName.Text = "";
                                }
                                txtBHNo.Text = tempA[0];
                                txtMeter.Text = Convert.ToDecimal(tempA[1]).ToString("0.00");
                                decimal d = Convert.ToDecimal(tempA[1]) * 450;
                                txtCalAmt.Text = d.ToString("0.00");
                                txtTotalAmt.Text = d.ToString("0.00");
                                txtApprovedAmt.Text = d.ToString("0.00");
                                rowNo++;
                            }
                        }
                    }

                    var gtOPData = dc.GTSummary_View(txtRefNo.Text, "Operator Payment").ToList();
                    rowNo = 0;
                    foreach (var operPayment in gtOPData)
                    {
                        string[] opertorPaymentArry = operPayment.GTSUM_Summary_var.Split('~');
                        for (int j = 0; j < opertorPaymentArry.Length; j++)
                        {
                            if (opertorPaymentArry[j] != "")
                            {
                                TextBox txtAdditionalAmt = (TextBox)grdOperatorPayment.Rows[rowNo].FindControl("txtAdditionalAmt");
                                TextBox txtApprovedAmt = (TextBox)grdOperatorPayment.Rows[rowNo].FindControl("txtApprovedAmt");
                                TextBox txtPrintDate = (TextBox)grdOperatorPayment.Rows[rowNo].FindControl("txtPrintDate");
                                DropDownList ddlPaymentTo = (DropDownList)grdOperatorPayment.Rows[rowNo].FindControl("ddlPaymentTo");
                                string[] opArray = opertorPaymentArry[j].Split('#');
                                txtAdditionalAmt.Text = opArray[4];
                                txtApprovedAmt.Text = opArray[6];
                                txtPrintDate.Text = opArray[7];
                                ddlPaymentTo.SelectedValue = opArray[8];
                                rowNo++;
                            }
                        }
                    }
                }

                #endregion
            }
        }
        private void CalculateAmount()
        {
            decimal TotalAmount = 0;
            for (int i = 0; i < grdGTSummary.Rows.Count; i++)
            {
                TextBox txtAmount = (TextBox)grdGTSummary.Rows[i].FindControl("txtAmount");
                if (txtAmount.Text != "")
                {
                    TotalAmount += Convert.ToDecimal(txtAmount.Text);
                }
            }
            lbltotalAmtResult.Text = TotalAmount.ToString();
        }
        protected Boolean ValidateData()
        {
            string dispalyMsg = "";
            Boolean valid = true;
            if (PnlEnq.Visible == true)
            {
                #region validate GTSummary Enquiry data

                if (valid == true)
                {                    
                    if (txtEmail.Text == "")
                    {
                        dispalyMsg = "Enter Email ID.";
                        txtEmail.Focus();
                        valid = false;
                    }
                    else if (txtDrilling.Text == "")
                    {
                        dispalyMsg = "Enter Drilling.";
                        txtDrilling.Focus();
                        valid = false;
                    }
                    else if (txtCharges.Text == "")
                    {
                        dispalyMsg = "Enter Charges.";
                        txtCharges.Focus();
                        valid = false;
                    }
                }

                #endregion
            }
            else if (pnlGTSummaryExpenses.Visible == true)
            {
                #region validate GTSummary Expenses data

                if (valid == true)
                {
                    for (int i = 0; i < grdGTSummary.Rows.Count; i++)
                    {
                        TextBox txtParticular = (TextBox)grdGTSummary.Rows[i].FindControl("txtParticular");
                        TextBox txtAmount = (TextBox)grdGTSummary.Rows[i].FindControl("txtAmount");

                        if (txtParticular.Text == "")
                        {
                            dispalyMsg = "Enter Particular for row no " + (i + 1) + ".";
                            txtParticular.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtAmount.Text == "")
                        {
                            dispalyMsg = "Enter Amount for row no " + (i + 1) + ".";
                            txtAmount.Focus();
                            valid = false;
                            break;
                        }

                    }
                }

                #endregion
            }
            else if (pnlIndividualSummary.Visible == true)
            {
                #region validate GT Individual Summary data

                if (valid == true)
                {
                    for (int i = 0; i < grdIndividualSummary.Rows.Count; i++)
                    {

                        DropDownList ddlDrillingMachineNo = (DropDownList)grdIndividualSummary.Rows[i].FindControl("ddlDrillingMachineNo");
                        TextBox txtfromDate = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtfromDate");
                        TextBox txttoDate = (TextBox)grdIndividualSummary.Rows[i].FindControl("txttoDate");
                        TextBox txtTotalDepth = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtTotalDepth");

                        if (ddlDrillingMachineNo.SelectedItem.Text == "--Select--")
                        {
                            dispalyMsg = "Select Drilling Machine No for row no " + (i + 1) + ".";
                            ddlDrillingMachineNo.Focus();
                            valid = false;
                            break;
                        }
                        else if(txtfromDate.Text == "")
                        {
                            dispalyMsg = "Select From Date for row no " + (i + 1) + ".";
                            txtfromDate.Focus();
                            valid = false;
                            break;
                        }
                        else if (txttoDate.Text == "")
                        {
                            dispalyMsg = "Select To Date for row no " + (i + 1) + ".";
                            txttoDate.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtTotalDepth.Text == "")
                        {
                            dispalyMsg = "Enter Total Depth for row no " + (i + 1) + ".";
                            txtTotalDepth.Focus();
                            valid = false;
                            break;
                        }
                    }
                }
                if (valid == true)
                {
                    //date validation
                    for (int i = 0; i < grdIndividualSummary.Rows.Count; i++)
                    {
                        TextBox txttoDate = (TextBox)grdIndividualSummary.Rows[i].FindControl("txttoDate");
                        TextBox txtfromDate = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtfromDate");

                        string[] toDate = txttoDate.Text.Split('/');
                        string[] fromDate = txtfromDate.Text.Split('/');

                        if (Convert.ToInt32(fromDate[2]) > Convert.ToInt32(toDate[2]))
                        {
                            dispalyMsg = "To Date must be greater than From Date for row " + (i + 1) + ".";                           
                            txttoDate.Text = "";
                            txttoDate.Focus();
                            valid = false;
                            break;

                        }
                        else if (Convert.ToInt32(fromDate[2]) == Convert.ToInt32(toDate[2]))
                        {
                            if (Convert.ToInt32(fromDate[1]) > Convert.ToInt32(toDate[1]))
                            {
                                dispalyMsg = "To Date must be greater than From Date for row " + (i + 1) + ".";
                                txttoDate.Text = "";
                                txttoDate.Focus();
                                valid = false;
                                break;
                            }
                            else if (Convert.ToInt32(fromDate[1]) == Convert.ToInt32(toDate[1]))
                            {
                                if (Convert.ToInt32(fromDate[0]) > Convert.ToInt32(toDate[0]))
                                {
                                    dispalyMsg = "To Date must be greater than From Date for row " + (i + 1) + ".";
                                    txttoDate.Text = "";
                                    txttoDate.Focus();
                                    valid = false;
                                    break;
                                }
                            }
                        }

                    }
                }

                #endregion
            }
            else if (pnlWorkVisit.Visible == true)
            {
                #region validate GTSummary Work Visit data

                if (valid == true)
                {
                    for (int i = 0; i < grdWorkVisit.Rows.Count; i++)
                    {
                        TextBox txtVisitOnDate = (TextBox)grdWorkVisit.Rows[i].FindControl("txtVisitOnDate");
                        TextBox txtVisitBy = (TextBox)grdWorkVisit.Rows[i].FindControl("txtVisitBy");
                        TextBox txtPurpose = (TextBox)grdWorkVisit.Rows[i].FindControl("txtPurpose");

                        if (txtVisitOnDate.Text == "")
                        {
                            dispalyMsg = "Select Visit On Date for row no " + (i + 1) + ".";
                            txtVisitOnDate.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtVisitBy.Text == "")
                        {
                            dispalyMsg = "Enter Visit By for row no " + (i + 1) + ".";
                            txtVisitBy.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtPurpose.Text == "")
                        {
                            dispalyMsg = "Enter Purpose for row no " + (i + 1) + ".";
                            txtPurpose.Focus();
                            valid = false;
                            break;
                        }
                    }
                }

                #endregion
            }
            else if (pnlNotes.Visible == true)
            {
                #region validate GTSummary Notes data

                if (valid == true)
                {
                    for (int i = 0; i < grdNotes.Rows.Count; i++)
                    {
                        TextBox txtRepairs = (TextBox)grdNotes.Rows[i].FindControl("txtRepairs");
                        TextBox txtMissalenious = (TextBox)grdNotes.Rows[i].FindControl("txtMissalenious");
                        if (txtRepairs.Text == "")
                        {
                            dispalyMsg = "Enter Repairs for row no " + (i + 1) + ".";
                            txtRepairs.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtMissalenious.Text == "")
                        {
                            dispalyMsg = "Enter Missalenious for row no " + (i + 1) + ".";
                            txtMissalenious.Focus();
                            valid = false;
                            break;
                        }
                    }
                }

                #endregion
            }
            else if (pnlOperatorPayment.Visible == true)
            {
                #region validate GTSummary Operator Payment data

                if (valid == true)
                {
                    for (int i = 0; i < grdOperatorPayment.Rows.Count; i++)
                    {
                        TextBox txtAdditionalAmt = (TextBox)grdOperatorPayment.Rows[i].FindControl("txtAdditionalAmt");
                        TextBox txtApprovedAmt = (TextBox)grdOperatorPayment.Rows[i].FindControl("txtApprovedAmt");
                        TextBox txtPrintDate = (TextBox)grdOperatorPayment.Rows[i].FindControl("txtPrintDate");
                        DropDownList ddlPaymentTo = (DropDownList)grdOperatorPayment.Rows[i].FindControl("ddlPaymentTo");

                        if (txtAdditionalAmt.Text == "")
                        {
                            dispalyMsg = "Enter Additional Amt. for row no " + (i + 1) + ".";
                            txtAdditionalAmt.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtApprovedAmt.Text == "")
                        {
                            dispalyMsg = "Enter Approved Amt. for row no " + (i + 1) + ".";
                            txtApprovedAmt.Focus();
                            valid = false;
                            break;
                        }
                        else if (txtPrintDate.Text == "")
                        {
                            dispalyMsg = "Enter Print Date for row no " + (i + 1) + ".";
                            txtPrintDate.Focus();
                            valid = false;
                            break;
                        }
                        else if (ddlPaymentTo.Text == "--Select--")
                        {
                            dispalyMsg = "Select Payment To for row no " + (i + 1) + ".";
                            ddlPaymentTo.Focus();
                            valid = false;
                            break;
                        }
                    }
                }

                #endregion
            }
            if (valid == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('" + dispalyMsg + "');", true);
            }
            return valid;
        }
        protected void ResetSrNo()
        {
            for (int i = 0; i < grdIndividualSummary.Rows.Count; i++)
            {
                TextBox txtBHNo = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtBHNo");
                string srno = Convert.ToString(i + 1);
                txtBHNo.Text = "BH " + srno;
            }
        }
        protected void RdbDataForList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayGTSummary();
        }
        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
            CalculateAmount();
        }
        protected void txtBoreHoles_TextChanged(object sender, EventArgs e)
        {
            BoreHolesRowsChanged();
            ResetSrNo();
        }
        protected void grdIndividualSummary_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlDrillingMachineNo = (e.Row.FindControl("ddlDrillingMachineNo") as DropDownList);
                var soset = dc.SoilSetting_View("Machine Settings").ToList();
                ddlDrillingMachineNo.DataSource = soset;
                ddlDrillingMachineNo.DataTextField = "SOSET_F1_var";
                ddlDrillingMachineNo.DataValueField = "F2plusF3";
                ddlDrillingMachineNo.DataBind();
                if (ddlDrillingMachineNo.Items.Count > 0)
                    ddlDrillingMachineNo.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
        protected void grdOperatorPayment_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlPaymentTo = (e.Row.FindControl("ddlPaymentTo") as DropDownList);
                var soset = dc.SoilSetting_View("Machine Settings").ToList();
                ddlPaymentTo.DataSource = soset;
                ddlPaymentTo.DataTextField = "SOSET_F2_var";
                ddlPaymentTo.DataValueField = "SOSET_F2_var";
                ddlPaymentTo.DataBind();
                if (ddlPaymentTo.Items.Count > 0)
                    ddlPaymentTo.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
        protected void ddlDrillingMachineNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
            if (gvr != null)
            {
                TextBox txtMachineOperator = (TextBox)gvr.FindControl("txtMachineOperator");
                TextBox txtByDurocreteOth = (TextBox)gvr.FindControl("txtByDurocreteOth");
                DropDownList ddlDrillingMachineNo = (DropDownList)gvr.FindControl("ddlDrillingMachineNo");

                if (ddlDrillingMachineNo.SelectedIndex > 0)
                {
                    string[] strMddNo;
                    strMddNo = ddlDrillingMachineNo.SelectedItem.Value.Split('|');
                    txtMachineOperator.Text = strMddNo[0];
                    txtByDurocreteOth.Text = strMddNo[1];
                    ddlDrillingMachineNo.Focus();
                }
            }
        }
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                if (RdbDataForList.SelectedValue == "Enquiry Details")
                {
                    string summaryVar = "";

                    summaryVar += ddlStatus.SelectedItem.Text + "#" + txtEmail.Text + "#" + txtDrilling.Text + "#" + txtCharges.Text + "~";
                    dc.GTSummary_Update(txtRefNo.Text, "Enquiry Details", summaryVar);                    

                }
                else if (RdbDataForList.SelectedValue == "Expenses")
                {
                    string summaryVar = "";

                    for (int i = 0; i < grdGTSummary.Rows.Count; i++)
                    {
                        TextBox txtParticular = (TextBox)grdGTSummary.Rows[i].FindControl("txtParticular");
                        TextBox txtAmount = (TextBox)grdGTSummary.Rows[i].FindControl("txtAmount");
                        summaryVar += txtParticular.Text + "#" + txtAmount.Text + "~";
                    }

                    dc.GTSummary_Update(txtRefNo.Text, "Expenses", summaryVar);

                }
                else if (RdbDataForList.SelectedValue == "Individual BH - Summary")
                {
                    string summaryVar = "";

                    for (int i = 0; i < grdIndividualSummary.Rows.Count; i++)
                    {
                        TextBox txtBHNo = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtBHNo");
                        DropDownList ddlDrillingMachineNo = (DropDownList)grdIndividualSummary.Rows[i].FindControl("ddlDrillingMachineNo");
                        TextBox txtMachineOperator = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtMachineOperator");
                        TextBox txtfromDate = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtfromDate");
                        TextBox txttoDate = (TextBox)grdIndividualSummary.Rows[i].FindControl("txttoDate");
                        TextBox txtTotalDepth = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtTotalDepth");
                        TextBox txtByDurocreteOth = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtByDurocreteOth");

                        summaryVar += txtBHNo.Text + "#" + ddlDrillingMachineNo.SelectedItem.Text + "#" + txtMachineOperator.Text + "#" + txtfromDate.Text + "#" + txttoDate.Text + "#" + txtTotalDepth.Text + "#" + txtByDurocreteOth.Text + "~";
                    }

                    dc.GTSummary_Update(txtRefNo.Text, "Individual BH - Summary", summaryVar);
                }
                else if (RdbDataForList.SelectedValue == "Machine Log")
                {
                    string summaryVar = "";

                    for (int i = 0; i < grdMachineLog.Rows.Count; i++)
                    {
                        TextBox txtMachineNo = (TextBox)grdMachineLog.Rows[i].FindControl("txtMachineNo");
                        TextBox txtNoBH = (TextBox)grdMachineLog.Rows[i].FindControl("txtNoBH");
                        TextBox txtDate = (TextBox)grdMachineLog.Rows[i].FindControl("txtDate");
                        TextBox txtOperator = (TextBox)grdMachineLog.Rows[i].FindControl("txtOperator");
                        TextBox txtOwner = (TextBox)grdMachineLog.Rows[i].FindControl("txtOwner");

                        summaryVar += txtMachineNo.Text + "#" + txtNoBH.Text + "#" + txtDate.Text + "#" + txtOperator.Text + "#" + txtOwner.Text + "~";
                    }

                    dc.GTSummary_Update(txtRefNo.Text, "Machine Log", summaryVar);
                }

                else if (RdbDataForList.SelectedValue == "Work Visit")
                {
                    string summaryVar = "";

                    for (int i = 0; i < grdWorkVisit.Rows.Count; i++)
                    {
                        TextBox txtVisitOnDate = (TextBox)grdWorkVisit.Rows[i].FindControl("txtVisitOnDate");
                        TextBox txtVisitBy = (TextBox)grdWorkVisit.Rows[i].FindControl("txtVisitBy");
                        TextBox txtPurpose = (TextBox)grdWorkVisit.Rows[i].FindControl("txtPurpose");

                        summaryVar += txtVisitOnDate.Text + "#" + txtVisitBy.Text + "#" + txtPurpose.Text + "~";
                    }

                    dc.GTSummary_Update(txtRefNo.Text, "Work Visit", summaryVar);

                }
                else if (RdbDataForList.SelectedValue == "Notes")
                {
                    string summaryVar = "";

                    for (int i = 0; i < grdNotes.Rows.Count; i++)
                    {
                        TextBox txtRepairs = (TextBox)grdNotes.Rows[i].FindControl("txtRepairs");
                        TextBox txtMissalenious = (TextBox)grdNotes.Rows[i].FindControl("txtMissalenious");

                        summaryVar += txtRepairs.Text + "#" + txtMissalenious.Text + "~";
                    }
                    dc.GTSummary_Update(txtRefNo.Text, "Notes", summaryVar);

                }

                else if (RdbDataForList.SelectedValue == "Operator Payment")
                {
                    string summaryVar = "";

                    for (int i = 0; i < grdOperatorPayment.Rows.Count; i++)
                    {
                        TextBox txtOperatorName = (TextBox)grdOperatorPayment.Rows[i].FindControl("txtOperatorName");
                        TextBox txtBHNo = (TextBox)grdOperatorPayment.Rows[i].FindControl("txtBHNo");
                        TextBox txtMeter = (TextBox)grdOperatorPayment.Rows[i].FindControl("txtMeter");
                        TextBox txtCalAmt = (TextBox)grdOperatorPayment.Rows[i].FindControl("txtCalAmt");
                        TextBox txtAdditionalAmt = (TextBox)grdOperatorPayment.Rows[i].FindControl("txtAdditionalAmt");
                        TextBox txtTotalAmt = (TextBox)grdOperatorPayment.Rows[i].FindControl("txtTotalAmt");
                        TextBox txtApprovedAmt = (TextBox)grdOperatorPayment.Rows[i].FindControl("txtApprovedAmt");
                        TextBox txtPrintDate = (TextBox)grdOperatorPayment.Rows[i].FindControl("txtPrintDate");
                        DropDownList ddlPaymentTo = (DropDownList)grdOperatorPayment.Rows[i].FindControl("ddlPaymentTo");

                        summaryVar += txtOperatorName.Text + "#" + txtBHNo.Text + "#" + txtMeter.Text + "#" + txtCalAmt.Text + "#" + txtAdditionalAmt.Text + "#" + txtTotalAmt.Text + "#" + txtApprovedAmt.Text + "#" + txtPrintDate.Text + "#" + ddlPaymentTo.SelectedValue + "~";
                    }

                    dc.GTSummary_Update(txtRefNo.Text, "Operator Payment", summaryVar);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myAlert", "alert('Updated Successfully');", true);
            }
        }
        protected void txtRefNo_TextChanged(object sender, EventArgs e)
        {
            DisplayGTSummary();
        }
        #region add/delete rows GTSummary grid
        protected void AddRowGTSummary()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["GTSummaryTable"] != null)
            {
                GetCurrentDataGTSummary();
                dt = (DataTable)ViewState["GTSummaryTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtParticular", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAmount", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtParticular"] = string.Empty;
            dr["txtAmount"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["GTSummaryTable"] = dt;
            grdGTSummary.DataSource = dt;
            grdGTSummary.DataBind();
            SetPreviousDataGTSummary();
        }
        protected void DeleteRowGTSummary(int rowIndex)
        {
            GetCurrentDataGTSummary();
            DataTable dt = ViewState["GTSummaryTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["GTSummaryTable"] = dt;
            grdGTSummary.DataSource = dt;
            grdGTSummary.DataBind();
            SetPreviousDataGTSummary();
        }
        protected void SetPreviousDataGTSummary()
        {
            DataTable dt = (DataTable)ViewState["GTSummaryTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtParticular = (TextBox)grdGTSummary.Rows[i].FindControl("txtParticular");
                TextBox txtAmount = (TextBox)grdGTSummary.Rows[i].FindControl("txtAmount");
                txtParticular.Text = dt.Rows[i]["txtParticular"].ToString();
                txtAmount.Text = dt.Rows[i]["txtAmount"].ToString();
            }
        }
        protected void GetCurrentDataGTSummary()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtParticular", typeof(string)));
            dt.Columns.Add(new DataColumn("txtAmount", typeof(string)));
            for (int i = 0; i < grdGTSummary.Rows.Count; i++)
            {
                TextBox txtParticular = (TextBox)grdGTSummary.Rows[i].FindControl("txtParticular");
                TextBox txtAmount = (TextBox)grdGTSummary.Rows[i].FindControl("txtAmount");

                dr = dt.NewRow();
                dr["txtParticular"] = txtParticular.Text;
                dr["txtAmount"] = txtAmount.Text;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["GTSummaryTable"] = dt;

        }
        protected void imgBtnAddRow_Click(object sender, EventArgs e)
        {
            AddRowGTSummary();
        }
        protected void imgBtnDeleteRow_Click(object sender, EventArgs e)
        {
            if (grdGTSummary.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowGTSummary(gvr.RowIndex);
            }
        }
        #endregion

        #region add/delete rows grdIS grid
        private void BoreHolesRowsChanged()
        {
            if (txtBoreHoles.Text != "")
            {
                if (Convert.ToInt32(txtBoreHoles.Text) < grdIndividualSummary.Rows.Count)
                {
                    for (int i = grdIndividualSummary.Rows.Count; i > Convert.ToInt32(txtBoreHoles.Text); i--)
                    {
                        if (grdIndividualSummary.Rows.Count > 1)
                        {
                            DeleteRowIS(i - 1);
                        }
                    }
                }
                if (Convert.ToInt32(txtBoreHoles.Text) > grdIndividualSummary.Rows.Count)
                {
                    for (int i = grdIndividualSummary.Rows.Count + 1; i <= Convert.ToInt32(txtBoreHoles.Text); i++)
                    {
                        AddRowIS();
                    }
                }
            }
        }
        protected void AddRowIS()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["ISTable"] != null)
            {
                GetCurrentDataIS();
                dt = (DataTable)ViewState["ISTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtBHNo", typeof(string)));
                dt.Columns.Add(new DataColumn("ddlDrillingMachineNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMachineOperator", typeof(string)));
                dt.Columns.Add(new DataColumn("txtfromDate", typeof(string)));
                dt.Columns.Add(new DataColumn("txttoDate", typeof(string)));
                dt.Columns.Add(new DataColumn("txtTotalDepth", typeof(string)));
                dt.Columns.Add(new DataColumn("txtByDurocreteOth", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtBHNo"] = string.Empty;
            dr["ddlDrillingMachineNo"] = string.Empty;
            dr["txtMachineOperator"] = string.Empty;
            dr["txtfromDate"] = string.Empty;
            dr["txttoDate"] = string.Empty;
            dr["txtTotalDepth"] = string.Empty;
            dr["txtByDurocreteOth"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["ISTable"] = dt;
            grdIndividualSummary.DataSource = dt;
            grdIndividualSummary.DataBind();
            SetPreviousDataIS();
        }
        protected void DeleteRowIS(int rowIndex)
        {
            GetCurrentDataIS();
            DataTable dt = ViewState["ISTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["ISTable"] = dt;
            grdIndividualSummary.DataSource = dt;
            grdIndividualSummary.DataBind();
            SetPreviousDataIS();
        }
        protected void SetPreviousDataIS()
        {
            DataTable dt = (DataTable)ViewState["ISTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtBHNo = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtBHNo");
                DropDownList ddlDrillingMachineNo = (DropDownList)grdIndividualSummary.Rows[i].FindControl("ddlDrillingMachineNo");
                TextBox txtMachineOperator = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtMachineOperator");
                TextBox txtfromDate = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtfromDate");
                TextBox txttoDate = (TextBox)grdIndividualSummary.Rows[i].FindControl("txttoDate");
                TextBox txtTotalDepth = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtTotalDepth");
                TextBox txtByDurocreteOth = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtByDurocreteOth");

                txtBHNo.Text = dt.Rows[i]["txtBHNo"].ToString();
                //ddlDrillingMachineNo.SelectedItem.Text = dt.Rows[i]["ddlDrillingMachineNo"].ToString();
                if ( dt.Rows[i]["ddlDrillingMachineNo"].ToString() !="")
                    ddlDrillingMachineNo.SelectedValue = dt.Rows[i]["ddlDrillingMachineNo"].ToString();
                txtMachineOperator.Text = dt.Rows[i]["txtMachineOperator"].ToString();
                txtfromDate.Text = dt.Rows[i]["txtfromDate"].ToString();
                txttoDate.Text = dt.Rows[i]["txttoDate"].ToString();
                txtTotalDepth.Text = dt.Rows[i]["txtTotalDepth"].ToString();
                txtByDurocreteOth.Text = dt.Rows[i]["txtByDurocreteOth"].ToString();
            }
        }
        protected void GetCurrentDataIS()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow drRow = null;

            dt.Columns.Add(new DataColumn("txtBHNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ddlDrillingMachineNo", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMachineOperator", typeof(string)));
            dt.Columns.Add(new DataColumn("txtfromDate", typeof(string)));
            dt.Columns.Add(new DataColumn("txttoDate", typeof(string)));
            dt.Columns.Add(new DataColumn("txtTotalDepth", typeof(string)));
            dt.Columns.Add(new DataColumn("txtByDurocreteOth", typeof(string)));

            for (int i = 0; i < grdIndividualSummary.Rows.Count; i++)
            {
                TextBox txtBHNo = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtBHNo");
                DropDownList ddlDrillingMachineNo = (DropDownList)grdIndividualSummary.Rows[i].FindControl("ddlDrillingMachineNo");
                TextBox txtMachineOperator = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtMachineOperator");
                TextBox txtfromDate = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtfromDate");
                TextBox txttoDate = (TextBox)grdIndividualSummary.Rows[i].FindControl("txttoDate");
                TextBox txtTotalDepth = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtTotalDepth");
                TextBox txtByDurocreteOth = (TextBox)grdIndividualSummary.Rows[i].FindControl("txtByDurocreteOth");

                drRow = dt.NewRow();
                drRow["txtBHNo"] = txtBHNo.Text;
                if (ddlDrillingMachineNo.SelectedIndex >=0 )
                    drRow["ddlDrillingMachineNo"] = ddlDrillingMachineNo.SelectedValue;
                else
                    drRow["ddlDrillingMachineNo"] = "";
                drRow["txtMachineOperator"] = txtMachineOperator.Text;
                drRow["txtfromDate"] = txtfromDate.Text;
                drRow["txttoDate"] = txttoDate.Text;
                drRow["txtTotalDepth"] = txtTotalDepth.Text;
                drRow["txtByDurocreteOth"] = txtByDurocreteOth.Text;

                dt.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["ISTable"] = dt;

        }
        #endregion

        #region add/delete rows Work Visit grid
        protected void AddRowWorkVisit()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["WorkVisitTable"] != null)
            {
                GetCurrentDataWorkVisit();
                dt = (DataTable)ViewState["WorkVisitTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txtVisitOnDate", typeof(string)));
                dt.Columns.Add(new DataColumn("txtVisitBy", typeof(string)));
                dt.Columns.Add(new DataColumn("txtPurpose", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtVisitOnDate"] = string.Empty;
            dr["txtVisitBy"] = string.Empty;
            dr["txtPurpose"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["WorkVisitTable"] = dt;
            grdWorkVisit.DataSource = dt;
            grdWorkVisit.DataBind();
            SetPreviousDataWorkVisit();
        }
        protected void DeleteRowWorkVisit(int rowIndex)
        {
            GetCurrentDataWorkVisit();
            DataTable dt = ViewState["WorkVisitTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["WorkVisitTable"] = dt;
            grdWorkVisit.DataSource = dt;
            grdWorkVisit.DataBind();
            SetPreviousDataWorkVisit();
        }
        protected void SetPreviousDataWorkVisit()
        {
            DataTable dt = (DataTable)ViewState["WorkVisitTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtVisitOnDate = (TextBox)grdWorkVisit.Rows[i].FindControl("txtVisitOnDate");
                TextBox txtVisitBy = (TextBox)grdWorkVisit.Rows[i].FindControl("txtVisitBy");
                TextBox txtPurpose = (TextBox)grdWorkVisit.Rows[i].FindControl("txtPurpose");
                txtVisitOnDate.Text = dt.Rows[i]["txtVisitOnDate"].ToString();
                txtVisitBy.Text = dt.Rows[i]["txtVisitBy"].ToString();
                txtPurpose.Text = dt.Rows[i]["txtPurpose"].ToString();
            }
        }
        protected void GetCurrentDataWorkVisit()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtVisitOnDate", typeof(string)));
            dt.Columns.Add(new DataColumn("txtVisitBy", typeof(string)));
            dt.Columns.Add(new DataColumn("txtPurpose", typeof(string)));

            for (int i = 0; i < grdWorkVisit.Rows.Count; i++)
            {
                TextBox txtVisitOnDate = (TextBox)grdWorkVisit.Rows[i].FindControl("txtVisitOnDate");
                TextBox txtVisitBy = (TextBox)grdWorkVisit.Rows[i].FindControl("txtVisitBy");
                TextBox txtPurpose = (TextBox)grdWorkVisit.Rows[i].FindControl("txtPurpose");

                dr = dt.NewRow();
                dr["txtVisitOnDate"] = txtVisitOnDate.Text;
                dr["txtVisitBy"] = txtVisitBy.Text;
                dr["txtPurpose"] = txtPurpose.Text;

                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["WorkVisitTable"] = dt;

        }
        protected void BtnAddRowWork_Click(object sender, EventArgs e)
        {
            AddRowWorkVisit();
        }
        protected void BtnDeleteRowWork_Click(object sender, EventArgs e)
        {
            if (grdWorkVisit.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowWorkVisit(gvr.RowIndex);
            }
        }
        #endregion

        #region add/delete rows Notes grid
        protected void AddRowNotes()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["NotesTable"] != null)
            {
                GetCurrentDataNotes();
                dt = (DataTable)ViewState["NotesTable"];
            }
            else
            {

                dt.Columns.Add(new DataColumn("txtRepairs", typeof(string)));
                dt.Columns.Add(new DataColumn("txtMissalenious", typeof(string)));
            }

            dr = dt.NewRow();
            dr["txtRepairs"] = string.Empty;
            dr["txtMissalenious"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["NotesTable"] = dt;
            grdNotes.DataSource = dt;
            grdNotes.DataBind();
            SetPreviousDataNotes();
        }
        protected void DeleteRowNotes(int rowIndex)
        {
            GetCurrentDataNotes();
            DataTable dt = ViewState["NotesTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["NotesTable"] = dt;
            grdNotes.DataSource = dt;
            grdNotes.DataBind();
            SetPreviousDataNotes();
        }
        protected void SetPreviousDataNotes()
        {
            DataTable dt = (DataTable)ViewState["NotesTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtRepairs = (TextBox)grdNotes.Rows[i].FindControl("txtRepairs");
                TextBox txtMissalenious = (TextBox)grdNotes.Rows[i].FindControl("txtMissalenious");

                txtRepairs.Text = dt.Rows[i]["txtRepairs"].ToString();
                txtMissalenious.Text = dt.Rows[i]["txtMissalenious"].ToString();
            }
        }
        protected void GetCurrentDataNotes()
        {
            int rowIndex = 0;
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("txtRepairs", typeof(string)));
            dt.Columns.Add(new DataColumn("txtMissalenious", typeof(string)));

            for (int i = 0; i < grdNotes.Rows.Count; i++)
            {
                TextBox txtRepairs = (TextBox)grdNotes.Rows[i].FindControl("txtRepairs");
                TextBox txtMissalenious = (TextBox)grdNotes.Rows[i].FindControl("txtMissalenious");
                dr = dt.NewRow();
                dr["txtRepairs"] = txtRepairs.Text;
                dr["txtMissalenious"] = txtMissalenious.Text;
                dt.Rows.Add(dr);
                rowIndex++;
            }
            ViewState["NotesTable"] = dt;

        }
        protected void BtnAddRowNotes_Click(object sender, EventArgs e)
        {
            AddRowNotes();
        }
        protected void BtnDeleteRowNotes_Click(object sender, EventArgs e)
        {
            if (grdNotes.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                DeleteRowNotes(gvr.RowIndex);
            }
        }
        #endregion

        protected void lnkExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReportStatus.aspx");
        }
    }
}