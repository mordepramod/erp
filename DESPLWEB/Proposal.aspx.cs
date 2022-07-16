using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace DESPLWEB
{
    public partial class Proposal : System.Web.UI.Page
    {
        LabDataDataContext dc = new LabDataDataContext();
        EncryptDecryptQueryString obj = new EncryptDecryptQueryString();
        //static double groupA = 0, groupB = 0, groupC = 0, calculatedDisc = 0, maxDiscnt = 0, appliedDisc = 0, totDisc = 0, totDiscA = 0, totDiscB = 0, introDiscA = 0, volDiscB = 0, timelyDiscC = 0, AdvDiscD = 0, loyDiscE = 0, propDiscF = 0, AppDiscG = 0;
        static double calculatedDisc = 0, maxDiscnt = 0, appliedDisc = 0, introDiscA = 0, volDiscB = 0, timelyDiscC = 0, AdvDiscD = 0, loyDiscE = 0, propDiscF = 0, AppDiscG = 0;
        static bool otFlag = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var user = dc.User_View(Convert.ToInt32(Session["LoginId"]), 0, "", "", "");
                foreach (var u in user)
                {
                    if (u.USER_SuperAdmin_right_bit == true)
                        lblSuperAdmin.Text = "1";
                    lblUserLevel.Text = u.USER_Level_tint.ToString();
                    break;
                }
                ViewState["RefUrl"] = Convert.ToString(Request.UrlReferrer);
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
                    txt_EnquiryNo.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[1].Split('=');
                    lblModifiedType.Text = arrIndMsg[1].ToString().Trim();//0-New proposal,1-revise proposal,2-Modify proposal

                    arrIndMsg = arrMsgs[2].Split('=');
                    lblProposalId.Text = arrIndMsg[1].ToString().Trim();
                    //lblPropStatus.Text = arrIndMsg[1].ToString().Trim();

                    arrIndMsg = arrMsgs[3].Split('=');
                    lblEnqNewClient.Text = arrIndMsg[1].ToString().Trim();


                }
                Tab_Notes.CssClass = "Clicked";
                MainView_Proposal.ActiveViewIndex = 0;
                Label lblheaderModify = (Label)Master.FindControl("lblHeading");
                if (txt_EnquiryNo.Text != "")
                {
                    lblheaderModify.Text = DisplayHeader("");
                    //  Session["LoginID"] = 1; //has to remove
                    LoadUserList();
                    LoadInwarDType();
                    ModifyProposal();
                    // AddRowDiscount();
                    //Discount();
                    LoadOtherTestList();
                }
            }
        }
        private void LoadInwarDType()
        {
            var inwd = dc.Material_View("", "");
            ddl_InwardTestType.DataTextField = "MATERIAL_Name_var";
            ddl_InwardTestType.DataValueField = "MATERIAL_RecordType_var";
            ddl_InwardTestType.DataSource = inwd;
            ddl_InwardTestType.DataBind();

            for (int i = 0; i < ddl_InwardTestType.Items.Count; i++)
            {
                if (ddl_InwardTestType.Items[i].Text == "Core Cutting")
                    ddl_InwardTestType.Items.RemoveAt(i);
            }
            ddl_InwardTestType.Items.Insert(0, new ListItem("Coupon", "Coupon"));
            ddl_InwardTestType.Items.Insert(0, "---Select---");
        }

        protected void LoadOtherTestList()
        {
            ddlOtherTest.Items.Clear();
            //int MaterialId = 0;
            //var InwardId = dc.Material_View("Other", "");
            //foreach (var n in InwardId)
            //{
            //    MaterialId = Convert.ToInt32(n.MATERIAL_Id.ToString());
            //}
            var a = dc.Test_View_ForProposal(0, 0, "OTHER", 0, 0, 0);
            ddlOtherTest.DataSource = a;
            ddlOtherTest.DataTextField = "TEST_Name_var";
            ddlOtherTest.DataValueField = "TEST_Id";
            ddlOtherTest.DataBind();
            ddlOtherTest.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        public void LoadUserList()
        {
            var user = dc.User_View(0, 0, "", "", "");
            ddl_PrposalBy.DataSource = user;
            ddl_PrposalBy.DataTextField = "USER_Name_var";
            ddl_PrposalBy.DataValueField = "USER_Id";
            ddl_PrposalBy.DataBind();
            ddl_PrposalBy.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        public string DisplayHeader(string Heading)
        {
            txt_ProposalDt.Text = DateTime.Today.ToString("dd/MM/yyyy");
            ddl_PrposalBy.SelectedValue = Convert.ToString(Session["LoginId"]);
            txt_Description.Text = "This has reference to our discussion for requirement of testing services as mentioned below. It will be a pleasure for us to provide testing services to your project. Please find the proposal below; ";
            if (lblEnqNewClient.Text != "True")//existing system client
            {
                //clsData obj=new clsData();
                //DataTable dt = obj.getEnquiryDetails(Convert.ToInt32(txt_EnquiryNo.Text),0);
                var res = dc.Enquiry_View_Details(Convert.ToInt32(txt_EnquiryNo.Text), false).ToList();
                foreach (var e in res)
                {
                    Heading = "Proposal " + "(" + Convert.ToString(e.MATERIAL_Name_var) + ")";
                    txt_EnquiryNo.Text = Convert.ToString(e.ENQ_Id);
                    txt_KindAttention.Text = Convert.ToString(e.CONT_Name_var);
                    lblRType.Text = Convert.ToString(e.MATERIAL_Name_var);
                    txtMe.Text = Convert.ToString(e.ME_Name_var);
                    txtMeNum.Text = Convert.ToString(e.ME_Contact_No);
                    txt_Subject.Text = "Commercial offer for " + Convert.ToString(e.MATERIAL_Name_var) + " requirement for your site " + e.SITE_Name_var;
                    txtClientNm.Text = Convert.ToString(e.CL_Name_var);
                    txtProContactNo.Text = Convert.ToString(e.CONT_ContactNo_var);
                    txtSiteName.Text = Convert.ToString(e.SITE_Name_var);
                    break;
                }
                txtSiteName.ReadOnly = true;
                // txtProContactNo.ReadOnly = true;

            }
            else
            {
                var res1 = dc.EnquiryNewClient_View_Details(Convert.ToInt32(txt_EnquiryNo.Text), false).ToList();
                foreach (var e in res1)
                {
                    Heading = "Proposal " + "(" + Convert.ToString(e.MATERIAL_Name_var) + ")";
                    txt_EnquiryNo.Text = Convert.ToString(e.ENQNEW_Id);
                    txt_KindAttention.Text = Convert.ToString(e.ENQNEW_ContactPersonName_var);
                    lblRType.Text = Convert.ToString(e.MATERIAL_Name_var);
                    txtMe.Text = "";
                    txtMeNum.Text = "";
                    txt_Subject.Text = "Commercial offer for " + Convert.ToString(e.MATERIAL_Name_var) + " requirement for your site " + e.ENQNEW_SiteName_var;
                    txtClientNm.Text = Convert.ToString(e.ENQNEW_ClientName_var);
                    txtSiteName.Text = Convert.ToString(e.ENQNEW_SiteName_var);
                    txtProContactNo.Text = Convert.ToString(e.ENQNEW_ContactNo_var);
                    break;
                }
                //lnkEditAdd.Visible = false;
                txtSiteName.ReadOnly = false;
                //txtProContactNo.ReadOnly = false;
                lnkUpdateIntro.Visible = false;
            }
            return Heading;
        }
        private void showGTFields()
        {
            grdGT.Visible = true;
            Grd_NoteGT.Visible = true;
            //chkGTDiscNote.Visible = true;
            //lblGTDiscNote.Visible = true;
            lblAddChargesGT.Visible = true;
            chkLums.Visible = true;
            lblLumSum.Visible = true;
            grdProposal.Visible = false;
            grdPayTermsGT.Visible = true;
            lblPayTermsGT.Visible = true;
            lblPaymentTerm.Visible = false;
            txtPaymentTerm.Visible = false;
        }
        private void hideGTFields()
        {
            grdProposal.Visible = true;
            grdGT.Visible = false;
            Grd_NoteGT.Visible = false;
            //chkGTDiscNote.Visible = false;
            //lblGTDiscNote.Visible = false;
            lblAddChargesGT.Visible = false;
            chkLums.Visible = false; chkLums.Checked = false;
            lblLumSum.Visible = false;
            //lblClientScope.Visible = false;
            //grdClientScope.Visible = false;
            grdPayTermsGT.Visible = false;
            lblPayTermsGT.Visible = false;
            lblPaymentTerm.Visible = true;
            txtPaymentTerm.Visible = true;
        }
        public void ModifyProposal()
        {
            int i = 0, mergeFrom = 0, mergeTo = 0, subTestId = 0; //status = 0, 
            //if (lblModifiedType.Text == "1")//if revise proposal is there then it will show revise details ie latest proposal detail on same enuiry
            //    status = 1;
            string gtDiscNote = "", addGtCharges = "", strStructAudDetails = "", paymentTermsGT = "";
            var res = dc.Proposal_View(Convert.ToInt32(txt_EnquiryNo.Text), Convert.ToBoolean(lblEnqNewClient.Text), Convert.ToInt32(lblProposalId.Text), "");
            foreach (var e in res)
            {
                if (i == 0)
                {
                    //txt_EnquiryNo.Text = Convert.ToString(e.ENQ_Id);
                    txt_EnquiryNo.Text = Convert.ToString(e.Proposal_EnqNo);
                    txt_ProposalNo.Text = Convert.ToString(e.Proposal_No);
                    txt_KindAttention.Text = e.Proposal_KindAttention.ToString();
                    txt_ProposalDt.Text = Convert.ToDateTime(e.Proposal_Date).ToString("dd/MM/yyyy");
                    txt_Subject.Text = Convert.ToString(e.Proposal_Subject);
                    txt_Description.Text = Convert.ToString(e.Proposal_Description);
                    ddl_PrposalBy.SelectedValue = e.Proposal_LoginId.ToString();
                    txt_ContactNo.Text = Convert.ToString(e.Proposal_ContactNo);
                    txt_NetAmount.Text = Convert.ToDecimal(e.Proposal_NetAmount).ToString();
                    mergeFrom = Convert.ToInt32(e.Proposal_MergeFrom);
                    mergeTo = Convert.ToInt32(e.Proposal_MergeTo);
                    chkQty.Checked = Convert.ToBoolean(e.Proposal_WithQty);
                    txtMe.Text = Convert.ToString(e.Proposal_MEName);
                    txtMeNum.Text = Convert.ToString(e.Proposal_MeContactNo);
                    gtDiscNote = Convert.ToString(e.Proposal_DiscNoteGT_var);
                    subTestId = Convert.ToInt32(e.Proposal_OTSubTestId_int);
                    addGtCharges = Convert.ToString(e.Proposal_AddChargesForGT);
                    strStructAudDetails = Convert.ToString(e.Proposal_StructAuditDetails_var);
                    chkMOUWorkOrder.Checked = Convert.ToBoolean(e.Proposal_MOUWorkOrderAvail_bit);
                    if (lblRType.Text == "Soil Investigation")
                        paymentTermsGT = Convert.ToString(e.Proposal_PaymentTerm_var);
                    else
                        txtPaymentTerm.Text = Convert.ToString(e.Proposal_PaymentTerm_var);
                    if (Convert.ToString(e.Proposal_AppliedDiscount) != "" && Convert.ToString(e.Proposal_AppliedDiscount) != null)
                    {
                        string[] strDiscDetails = Convert.ToString(e.Proposal_AppliedDiscount).Split('~');
                        lblVol.Text = strDiscDetails[0];
                        lblTime.Text = strDiscDetails[1];
                        lblAdv.Text = strDiscDetails[2];
                        lblLoy.Text = strDiscDetails[3];
                        lblProp.Text = strDiscDetails[4];
                        lblApp.Text = strDiscDetails[5];
                        lblCalcDisc.Text = strDiscDetails[6];
                        lblMax.Text = strDiscDetails[7];
                        txtIntro.Text = strDiscDetails[8];
                        lblDisc.Text = strDiscDetails[9];
                    }
                }

                if (Convert.ToString(e.Proposal_ClientScope_var) != "")
                {
                    int j = 0;
                    string[] scopedata = Convert.ToString(e.Proposal_ClientScope_var).Split('|');
                    foreach (string scp in scopedata)
                    {
                        if (scp != "")
                        {
                            AddRowClientScope();
                            TextBox txt_NOTEClientScope = (TextBox)grdClientScope.Rows[j].FindControl("txt_NOTEClientScope");
                            txt_NOTEClientScope.Text = scp.ToString();
                            j++;
                        }
                    }
                }

                if (Grd_Note.Rows.Count <= 0)
                {
                    if (Convert.ToString(e.Proposal_Notes) != "")
                    {
                        int j = 0;
                        string[] notedata = Convert.ToString(e.Proposal_Notes).Split('|');
                        foreach (string note in notedata)
                        {
                            if (note != "" && note.TrimEnd() != "GST @ 18% will be applicable extra.")
                            {
                                AddRowNote();
                                TextBox txt_NOTE = (TextBox)Grd_Note.Rows[j].Cells[2].FindControl("txt_NOTE");
                                txt_NOTE.Text = note.ToString();
                                j++;
                            }
                        }
                    }

                }
                /////Additional Charges GT
                if (lblRType.Text == "Soil Investigation")
                    showGTFields();

                if (addGtCharges != "")
                {
                    Grd_NoteGT.Visible = true;
                    lblAddChargesGT.Visible = true;
                }                
                if (Grd_NoteGT.Visible == true)
                {
                    if (Grd_NoteGT.Rows.Count <= 0)
                    {
                        if (Convert.ToString(e.Proposal_AddChargesForGT) != "")
                        {
                            int j = 0;
                            string[] notedata = Convert.ToString(e.Proposal_AddChargesForGT).Split('|');
                            foreach (string note in notedata)
                            {
                                if (note != "")
                                {
                                    AddRowNoteGT();
                                    TextBox txt_NOTEGT = (TextBox)Grd_NoteGT.Rows[j].Cells[2].FindControl("txt_NOTEGT");
                                    txt_NOTEGT.Text = note.ToString();
                                    j++;
                                }
                            }
                        }

                    }
                }
                if (paymentTermsGT != "")
                {
                    grdPayTermsGT.Visible = true;
                    lblPayTermsGT.Visible = true;
                }
                if (grdPayTermsGT.Visible == true)
                {
                    if (grdPayTermsGT.Rows.Count <= 0)
                    {
                        if (Convert.ToString(e.Proposal_PaymentTerm_var) != "")
                        {
                            int j = 0;
                            string[] notedata = Convert.ToString(e.Proposal_PaymentTerm_var).Split('|');
                            foreach (string note in notedata)
                            {
                                if (note != "")
                                {
                                    AddRowPayTermGT();
                                    TextBox txtNotePayTermGT = (TextBox)grdPayTermsGT.Rows[j].Cells[2].FindControl("txtNotePayTermGT");
                                    txtNotePayTermGT.Text = note.ToString();
                                    j++;
                                }
                            }
                        }

                    }
                }
                i++;
            }

            addTermsConditionNotes(lblRType.Text);

            clsData obj = new clsData();
            string inwardTypeValue = obj.getInwardTypeValue(lblRType.Text);
            if (lblModifiedType.Text == "0")//for new proposal load data
            {
                getDiscount(inwardTypeValue);
                if (inwardTypeValue != "CORECUT")
                {
                    if (ddl_InwardTestType.Items.FindByValue(inwardTypeValue).Value != null || ddl_InwardTestType.Items.FindByValue(inwardTypeValue).Value != "")
                        ddl_InwardTestType.SelectedValue = ddl_InwardTestType.Items.FindByValue(inwardTypeValue).Value;
                }
                if (ddl_InwardTestType.SelectedValue == "OT")
                    ddlOtherTest.Visible = true;
                else
                    ddlOtherTest.Visible = false;

                if (lblRType.Text.Equals("Soil Investigation"))
                {
                    showGTFields();
                    addTestToGridGT("GT");
                    chkGTDiscNote.Visible = true;
                    lblGTDiscNote.Visible = true;
                    lnkDepth.Visible = true;
                    mergeFrom = 2; mergeTo = 8;

                    if (mergeFrom != 0 && mergeTo != 0)
                    {
                        chkLums.Checked = true;
                        ChkLumShup();
                        txtToRow.Text = mergeTo.ToString();
                        txtFrmRow.Text = mergeFrom.ToString();
                    }
                }
                else
                {
                    hideGTFields();
                    addTestToGrid(inwardTypeValue);
                    if (inwardTypeValue == "RWH")
                    {
                        Grd_NoteGT.Visible = true;
                        lblAddChargesGT.Visible = true;

                        grdPayTermsGT.Visible = true;
                        lblPayTermsGT.Visible = true;

                    }

                }
                DisplayGenericDiscountDetails();
            }
            else //for modify or revised proposal fetch saved data 
            {

                if (lblModifiedType.Text == "1")//ie revisw proposal
                {
                    txt_ProposalDt.Text = DateTime.Today.ToString("dd/MM/yyyy");
                }
                chkGTDiscNote.Visible = false;
                lblGTDiscNote.Visible = false;
                lblDepth.Visible = false;
                int k = 0; string testName = "", recType = "";
                var reslt = dc.ProposalDetail_View(Convert.ToString(txt_ProposalNo.Text.TrimEnd())).ToList();
                if (reslt.Count > 0)
                {
                    recType = reslt.FirstOrDefault().ProposalDetail_RecType.ToString();

                    foreach (var item in reslt)
                    {
                        if (Convert.ToString(item.ProposalDetail_RecType) != "" && Convert.ToString(item.ProposalDetail_RecType) != null)
                            if (Convert.ToString(item.ProposalDetail_RecType) == "GT")
                            {
                                lnkDepth.Visible = true;
                                if (gtDiscNote != "" && gtDiscNote != null)
                                {
                                    chkGTDiscNote.Checked = true;
                                    chkGTDiscNote.Visible = true;
                                    lblGTDiscNote.Visible = true;
                                }
                                else
                                {
                                    chkGTDiscNote.Checked = false;
                                    chkGTDiscNote.Visible = true;
                                    lblGTDiscNote.Visible = true;
                                }
                            }

                    }
                }
                if (recType == "OTHER")
                {
                    LoadOtherTestList();
                    var rslt = dc.Test_View(0, subTestId, "", 0, 0, 0).ToList();
                    testName = rslt.FirstOrDefault().TEST_Name_var;
                    ddlOtherTest.Visible = true;
                    if (subTestId > 0)
                        ddlOtherTest.SelectedValue = ddlOtherTest.Items.FindByText(testName).Value;
                    recType = "OT";
                }
                string materialName = obj.getInwardTypeName(recType);
                getDiscount(recType);
                if (recType != "" && recType != null)
                    ddl_InwardTestType.SelectedValue = ddl_InwardTestType.Items.FindByValue(recType).Value;

                if (materialName.Equals("Soil Investigation") || testName.Equals("SBC by SPT") || testName.Equals("Water Test for Drinking/Domestic Purpose"))
                    showGTFields();
                else
                    hideGTFields();

                if (testName.Equals("Water Test for Drinking/Domestic Purpose"))
                {
                    Grd_NoteGT.Visible = false;
                    lblAddChargesGT.Visible = false;

                    grdPayTermsGT.Visible = false;
                    lblPayTermsGT.Visible = false;
                }
                foreach (var es in reslt)
                {
                    if (materialName.Equals("Soil Investigation") || testName.Equals("SBC by SPT") || testName.Equals("Water Test for Drinking/Domestic Purpose"))
                    {
                        //showGTFields();

                        AddRowProposalGT();
                        TextBox txt_Particular = (TextBox)grdGT.Rows[k].Cells[2].FindControl("txt_Particular");
                        TextBox txt_TestMethod = (TextBox)grdGT.Rows[k].Cells[3].FindControl("txt_TestMethod");
                        TextBox txt_Unit = (TextBox)grdGT.Rows[k].Cells[4].FindControl("txt_Unit");
                        TextBox txt_Quantity = (TextBox)grdGT.Rows[k].Cells[5].FindControl("txt_Quantity");
                        TextBox txt_UnitRate = (TextBox)grdGT.Rows[k].Cells[6].FindControl("txt_UnitRate");
                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[k].Cells[7].FindControl("txt_DiscRate");
                        TextBox txt_Amount = (TextBox)grdGT.Rows[k].Cells[8].FindControl("txt_Amount");
                        Label lbl_InwdType = (Label)grdGT.Rows[k].Cells[9].FindControl("lbl_InwdType");


                        if (Convert.ToString(es.ProposalDetail_Particular) != "")
                            txt_Particular.Text = es.ProposalDetail_Particular.ToString();

                        if (Convert.ToString(es.ProposalDetail_TestMethod) != "")
                            txt_TestMethod.Text = es.ProposalDetail_TestMethod.ToString();

                        if (Convert.ToString(es.ProposalDetail_DiscountedRate) != "" || Convert.ToString(es.ProposalDetail_DiscountedRate) != null)
                        {
                            if (Convert.ToDecimal(es.ProposalDetail_DiscountedRate) >= 0)
                                txt_DiscRate.Text = Convert.ToString(es.ProposalDetail_DiscountedRate);
                        }

                        if (Convert.ToString(es.ProposalDetail_Rate) != "" || Convert.ToString(es.ProposalDetail_Rate) != null)
                        {
                            if (Convert.ToDecimal(es.ProposalDetail_Rate) >= 0)
                                txt_UnitRate.Text = es.ProposalDetail_Rate.ToString();
                        }

                        if (Convert.ToString(es.ProposalDetail_Unit) != "" && Convert.ToString(es.ProposalDetail_Unit) != null)
                            txt_Unit.Text = es.ProposalDetail_Unit.ToString();

                        if (Convert.ToString(es.ProposalDetail_Quanity) != "")
                            txt_Quantity.Text = es.ProposalDetail_Quanity.ToString();

                        if (Convert.ToDecimal(es.ProposalDetail_Amount) >= 0)
                            txt_Amount.Text = es.ProposalDetail_Amount.ToString();

                        //lbl_InwdType.Text = "Soil Investigation";
                        lbl_InwdType.Text = "GT";
                    }
                    else
                    {
                        //hideGTFields();
                        AddRowProposal();
                        TextBox txt_Particular = (TextBox)grdProposal.Rows[k].Cells[3].FindControl("txt_Particular");
                        TextBox txt_TestMethod = (TextBox)grdProposal.Rows[k].Cells[4].FindControl("txt_TestMethod");
                        TextBox txt_Rate = (TextBox)grdProposal.Rows[k].Cells[5].FindControl("txt_Rate");
                        TextBox txt_DiscRate = (TextBox)grdProposal.Rows[k].Cells[6].FindControl("txt_DiscRate");
                        TextBox txt_Discount = (TextBox)grdProposal.Rows[k].Cells[7].FindControl("txt_Discount");
                        TextBox txt_Quantity = (TextBox)grdProposal.Rows[k].Cells[8].FindControl("txt_Quantity");
                        TextBox txt_Amount = (TextBox)grdProposal.Rows[k].Cells[9].FindControl("txt_Amount");
                        Label lbl_InwdType = (Label)grdProposal.Rows[k].Cells[10].FindControl("lbl_InwdType");
                        Label lbl_TestId = (Label)grdProposal.Rows[k].Cells[12].FindControl("lbl_TestId");

                        if (Convert.ToString(es.ProposalDetail_Particular) != "")
                            txt_Particular.Text = es.ProposalDetail_Particular.ToString();

                        if (Convert.ToString(es.ProposalDetail_TestMethod) != "" && Convert.ToString(es.ProposalDetail_TestMethod) != null)
                            txt_TestMethod.Text = es.ProposalDetail_TestMethod.ToString();
                        else
                            txt_TestMethod.Text = "NA";

                        if (Convert.ToDecimal(es.ProposalDetail_Rate) > 0)
                            txt_Rate.Text = es.ProposalDetail_Rate.ToString();
                        else
                            txt_Rate.Text = "0.00";

                        if (Convert.ToString(es.ProposalDetail_Discount) != "")
                        {
                            if (Convert.ToDecimal(es.ProposalDetail_Discount) > 0)
                                txt_Discount.Text = es.ProposalDetail_Discount.ToString();
                        }

                        if (Convert.ToDecimal(es.ProposalDetail_DiscountedRate) > 0)
                            txt_DiscRate.Text = es.ProposalDetail_DiscountedRate.ToString();
                        else
                            txt_DiscRate.Text = "0.00";


                        if (Convert.ToDecimal(es.ProposalDetail_Quanity) > 0)
                            txt_Quantity.Text = es.ProposalDetail_Quanity.ToString();

                        if (Convert.ToString(es.ProposalDetail_RecType) != "" && Convert.ToString(es.ProposalDetail_RecType) != null)
                            lbl_InwdType.Text = es.ProposalDetail_RecType.ToString();//matType = es.ProposalDetail_RecType.ToString();
                        else
                            lbl_InwdType.Text = lblRType.Text; //matType = lblRType.Text;

                        if (Convert.ToDecimal(es.ProposalDetail_Amount) > 0)
                            txt_Amount.Text = es.ProposalDetail_Amount.ToString();

                        if (Convert.ToString(es.ProposalDetail_TestId) != "" && Convert.ToString(es.ProposalDetail_TestId) != null)
                            lbl_TestId.Text = es.ProposalDetail_TestId.ToString();
                        else
                            lbl_TestId.Text = "";
                    }
                    k++;
                }
                if (addGtCharges != "")
                {
                    Grd_NoteGT.Visible = true;
                    lblAddChargesGT.Visible = true;
                }
                if (paymentTermsGT != "")
                {
                    grdPayTermsGT.Visible = true;
                    lblPayTermsGT.Visible = true;
                }
                if (materialName.Equals("Soil Investigation") || testName.Equals("SBC by SPT") || testName.Equals("Water Test for Drinking/Domestic Purpose"))
                {
                    if (mergeFrom != 0 && mergeTo != 0)
                    {
                        chkLums.Checked = true;
                        ChkLumShup();
                        txtToRow.Text = mergeTo.ToString();
                        txtFrmRow.Text = mergeFrom.ToString();
                        if (materialName.Equals("Soil Investigation"))
                            ShowMerge(mergeFrom, mergeTo, "GT");
                        else if (testName.Equals("SBC by SPT"))
                            ShowMerge(2, 3, "OTHER");
                        else if (testName.Equals("Water Test for Drinking/Domestic Purpose"))
                            ShowMerge(1, 3, "OTHER");

                        txt_EnquiryNo.Focus();

                    }
                }
                if (recType == "OT" && testName == "Structural Audit")
                {
                    pnlStructAudit.Visible = true;
                    grdProposal.Columns[3].HeaderText = "Members";
                    grdProposal.Columns[4].HeaderText = "Samples";
                    if (strStructAudDetails != null && strStructAudDetails != "")
                    {
                        string[] strVal = strStructAudDetails.Split('~');
                        if (strVal.Count() > 0)
                        {
                            txtStructNameOfApartSoc.Text = strVal[0];
                            txtStructAddress.Text = strVal[1];
                            txtStructBuiltupArea.Text = strVal[2];
                            txtStructNoOfBuild.Text = strVal[3];
                            txtStructAge.Text = strVal[4];
                            ddlStructConstWithin5Y.SelectedValue = strVal[5];
                            ddlStructLocation.SelectedValue = strVal[6];
                            ddlStructAddLoadExpc.SelectedValue = strVal[7];
                            ddlStructDistressObs.SelectedValue = strVal[8];
                        }
                    }
                }

            }

            if (Grd_Note.Rows.Count <= 0)
            {
                AddRowNote();
            }
            if (grdProposal.Rows.Count <= 0)
            {
                ShowHeader();
            }
            if (Grd_NoteGT.Rows.Count <= 0)
            {
                AddRowNoteGT();
            }
            if (grdPayTermsGT.Visible == true && grdPayTermsGT.Rows.Count <= 0)
            {
                AddRowPayTermGT();
            }
            if (grdClientScope.Visible == true && grdClientScope.Rows.Count <= 0)
            {
                AddRowClientScope();
            }
        }

        private void addAdditionalDiscNotes(string addNotes)
        {
            if (addNotes != "")
            {

                int j = 0;
                string[] notedata = Convert.ToString(addNotes).Split('|');
                foreach (string note in notedata)
                {
                    if (note != "")
                    {
                        AddRowNoteGT();
                        TextBox txt_NOTEGT = (TextBox)Grd_NoteGT.Rows[j].Cells[2].FindControl("txt_NOTEGT");
                        txt_NOTEGT.Text = note.ToString();
                        j++;
                    }
                }
            }
        }
        private void addTestToGridGT(string testType)
        {
            if (testType == "GT")
            {
                string[] gtTest = new string[] {@"Mobilization of drilling equipments, accessories,personnel, etc. to site, and demobilization of the same after completion of work.",
                    "Setting and shifting of rig at borehole locations",
                    "Drilling exploratory holes of size, up to 100mm dia. in soil of all sorts and collecting disturbed samples (Soil where SPT N<50)",
                    "Drilling ‘NX’ size exploratory bore holes in rock,extracting rock cores, serially marking them as per specification. (Rock where SPT N >50)",
                    "Conducting Standard Penetration Test (SPT) in soil and collecting/preserving SPT soil samples at every 1.5 m vertical interval",
                    "Preservation & handing over of soil and rock samples on site during & after completion of job in wooden core boxes",
                    "Conducting Laboratory Test on Soil Samples- a) Sieve Analysis b) Atterberg’s Limits (LL, PL) c) Sulphate & Chloride d) Free Swell Index",
                    "Conducting Laboratory Test on Rock Core Samples- a) Dry Density b) Specific gravity c) Water absorption d) Porosity e) Saturated crushing Strength/ Point Load",
                    "Submission of geotechnical report including results of all field and laboratory tests, foundation recommendations, and any other relevant geotechnical issues" };

                //LnkBtnCal.Visible = false;
                for (int o = 0; o <= 8; o++)
                {
                    AddRowProposalGT();
                    TextBox txt_Particular = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[2].FindControl("txt_Particular");
                    Label lbl_InwdType = (Label)grdGT.Rows[grdGT.Rows.Count - 1].Cells[8].FindControl("lbl_InwdType");

                    txt_Particular.Text = gtTest[o];
                    //lbl_InwdType.Text = "Soil Investigation";//ddl_InwardTestType.SelectedItem.Text;
                    lbl_InwdType.Text = "GT";
                }

                ShowMerge(2, 8, "GT");
            }
            else if (testType == "SBC by SPT" || testType == "Water Test for Drinking/Domestic Purpose")
            {
                string recType = "";
                int subTestId = 0;
                if (ddl_InwardTestType.SelectedValue == "OT")
                {
                    if (ddlOtherTest.SelectedValue.ToString() != "")
                    {
                        subTestId = Convert.ToInt32(ddlOtherTest.SelectedValue);
                        recType = "OTHER";
                    }
                }


                var res = dc.Test_View_ForProposal(-1, 0, recType, 0, 0, subTestId);


                foreach (var re in res)
                {
                    AddRowProposalGT();
                    TextBox txt_Particular = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[2].FindControl("txt_Particular");
                    TextBox txt_TestMethod = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[3].FindControl("txt_TestMethod");
                    Label lbl_InwdType = (Label)grdGT.Rows[grdGT.Rows.Count - 1].Cells[8].FindControl("lbl_InwdType");

                    txt_Particular.Text = re.TEST_Name_var.ToString();
                    if (Convert.ToString(re.TEST_Method_var) == "" || Convert.ToString(re.TEST_Method_var) == null)
                        txt_TestMethod.Text = "NA";
                    else
                        txt_TestMethod.Text = re.TEST_Method_var.ToString();
                    lbl_InwdType.Text = "OTHER";
                }

                //if (testType == "Water Test for Drinking/Domestic Purpose")
                //    grdGT.Columns[3].Visible = false;
                //else
                //    grdGT.Columns[3].Visible = true;

                if (testType == "SBC by SPT")
                    ShowMerge(2, 3, "OTHER");
                else
                    ShowMerge(1, 3, "OTHER");

            }
        }
        //private void addTermsConditionNotes(string inwdType)
        //{
        //    if (Grd_Note.Rows.Count == 0)
        //    {

        //        if (inwdType == "Soil Investigation" || inwdType == "SBC by SPT" || inwdType == "Rain Water Harvesting" || inwdType == "Plate Load Testing" || inwdType == "Earth Resistivity Test")// tems for GT only
        //        {
        //            for (int l = 0; l < 11; l++)
        //            {
        //                AddRowNote();
        //                TextBox txt_NOTE = (TextBox)Grd_Note.Rows[l].Cells[2].FindControl("txt_NOTE");
        //                if (l == 0)
        //                    txt_NOTE.Text = "You shall issue us a firm work order written or email from company mail ID.";
        //                else if (l == 1)
        //                {
        //                    if (inwdType == "Plate Load Testing")
        //                        txt_NOTE.Text = "All Local problems like right of way, obstructions of local peoples,etc shall be settled by you before our setup reach site.We shall deploy our machines only after getting confirmation from your end.";
        //                    else
        //                        txt_NOTE.Text = "All Local problems like right of way, obstructions of local peoples,etc shall be settled by you before our machines reach site.We shall deploy our machines only after getting confirmation from your end.";
        //                }
        //                else if (l == 2)
        //                {
        //                    if (inwdType == "Plate Load Testing" || inwdType == "SBC by SPT")
        //                        txt_NOTE.Text = "Clearance of the site from shrubs and bushes at the locations and reasonable access to the locations,shall be in client scope.";
        //                    else if (inwdType == "Earth Resistivity Test")
        //                        txt_NOTE.Text = "Clearance of the site from shrubs and bushes at the locations and the way to the locations,shall be in client scope.";
        //                    else
        //                        txt_NOTE.Text = "Clearance of the site from shrubs and bushes at the bore hole locations and the way to the borehole locations,shall be in client scope.";
        //                }
        //                else if (l == 3)
        //                    txt_NOTE.Text = "We shall carry our camping equipment with us to site.You shall provide us with open space at site for our labor camp free of cost.";
        //                else if (l == 4)
        //                {
        //                    if (inwdType == "Rain Water Harvesting" || inwdType == "Plate Load Testing" || inwdType == "Earth Resistivity Test")
        //                        txt_NOTE.Text = "All the underground utility services shall be checked and marked by you prior to start of the drilling activity. We shall not be responsible for any damage caused to them.";
        //                    else if (inwdType == "SBC by SPT")
        //                        txt_NOTE.Text = "All the underground utility services shall be checked and marked by you prior to start of the testing. We shall not be responsible for any damage caused to them during drilling operation.";
        //                    else
        //                        txt_NOTE.Text = "All the underground utility services shall be checked and marked by you prior to start of the drilling activity. We shall not be responsible for any damage caused to them during drilling operation.";

        //                }
        //                else if (l == 5)
        //                {
        //                    if (inwdType == "Rain Water Harvesting" || inwdType == "Plate Load Testing" || inwdType == "Earth Resistivity Test")
        //                        txt_NOTE.Text = "Locations of all the points shall be marked by your representative at site. The testing shall be carried out by us only at the locations marked by you.";
        //                    else if (inwdType != "SBC by SPT")
        //                        txt_NOTE.Text = "Locations of all the boreholes shall be marked by your representative at site. The drilling shall be carried out by us only at the locations marked by you.";

        //                }
        //                else if (l == 6)
        //                {
        //                    //if (inwdType == "Rain Water Harvesting")
        //                    //    txt_NOTE.Text = "The water required for the drilling activity shall be provided by you at the borehole testing location free of cost.";
        //                    //else
        //                    if (inwdType == "Plate Load Testing")
        //                        txt_NOTE.Text = "In case if required JCB or Poclaine machine shall be provided by you for shifting / setting of PLT setup at location on site.";
        //                    //else if (inwdType == "SBC by SPT")
        //                    //    txt_NOTE.Text = "In case if required JCB or Poclaine machine shall be provided by you for shifting or setting drilling machine at borehole location on site.";
        //                    else if (inwdType != "Earth Resistivity Test" && inwdType != "Rain Water Harvesting" && inwdType != "SBC by SPT")
        //                        txt_NOTE.Text = "The water required for the drilling activity shall be provided by you at the borehole drilling location free of cost.In case if required JCB or Poclaine machine shall be provided by you for shifting or setting drilling machine at borehole location on site.";

        //                }
        //                else if (l == 7)
        //                    txt_NOTE.Text = "All work executed will be witnessed by your representative jointly so as to avoid any discrepancy between the work executed and your requirement.";
        //                else if (l == 8)
        //                    txt_NOTE.Text = "Any other requirement of detail calculations/additional recommendations which are not part of above proposal will be charged extra.";
        //                else if (l == 9)
        //                    txt_NOTE.Text = "Draft report will be submitted in soft format for your perusal along with final bill after completion of field work and laboratory tests.";
        //                else if (l == 10)
        //                    txt_NOTE.Text = "Final report(Hard copy-One color) will be submitted after receipt of entire payment.";

        //            }

        //            if (inwdType == "SBC by SPT")
        //            {
        //                DeleteRowNote(5); DeleteRowNote(5);
        //            }

        //            if (inwdType == "Earth Resistivity Test" || inwdType == "Rain Water Harvesting")
        //                DeleteRowNote(6);

        //        }
        //        else
        //        {
        //           for (int l = 0; l < 2; l++)
        //            {
        //                if (l == 0)
        //                {
        //                    AddRowNote();
        //                    TextBox txt_NOTE = (TextBox)Grd_Note.Rows[l].Cells[2].FindControl("txt_NOTE");
        //                    txt_NOTE.Text = "You shall issue us a firm work order written or email from company mail ID.";
        //                }
        //                if (l == 1)
        //                {
        //                    if (inwdType != "Non Destructive Testing" &&  inwdType != "Core Cutting" && inwdType !="Core Testing")
        //                    {
        //                        AddRowNote();
        //                        TextBox txt_NOTE = (TextBox)Grd_Note.Rows[l].Cells[2].FindControl("txt_NOTE");
        //                        txt_NOTE.Text = "You can place order by filling the Test Request Form on Durocrete APP to get additional 5% discount.";
        //                    }
        //                }
        //                //else if (l == 2)
        //                //txt_NOTE.Text = "This is not GST Tax Invoice. To avail GST credit, Please ask for GST Tax Invoice.";
        //                //else if (l == 3)
        //                //    txt_NOTE.Text = "The above rates are excluding GST and the same shall be levied on the bill value as per prevailing rates.";
        //            }

        //            if (inwdType == "Steel Testing")
        //            {
        //                AddRowNote();
        //                TextBox txt_NOTE = (TextBox)Grd_Note.Rows[Grd_Note.Rows.Count - 1].Cells[2].FindControl("txt_NOTE");
        //                txt_NOTE.Text = "Required 3 pieces of 1400mm for steel bar dia up to 25mm and 1600mm for 32mm diameter.";

        //            }
        //        }
        //    }
        //    if (inwdType == "Soil Investigation" || inwdType == "Rain Water Harvesting" || inwdType == "SBC by SPT")
        //    {
        //        Grd_NoteGT.Visible = true;
        //        lblAddChargesGT.Visible = true;
        //        if (Grd_NoteGT.Rows.Count == 0)
        //        {
        //            /////Additional Charges GT
        //            if (Grd_NoteGT.Visible == true)
        //            {
        //                if (inwdType == "Soil Investigation")
        //                {
        //                    for (int l = 0; l < 3; l++)
        //                    {
        //                        AddRowNoteGT();
        //                        TextBox txt_NOTEGT = (TextBox)Grd_NoteGT.Rows[l].Cells[2].FindControl("txt_NOTEGT");

        //                        if (l == 0)
        //                            txt_NOTEGT.Text = "In case total depth of boreholes exceeds 10m, additional charges for drilling will be charged on pro rata basis ie charges per borehole divided by 10, per running meter.";
        //                        else if (l == 1)
        //                            txt_NOTEGT.Text = "If water is not provided by you the same shall arranged by Durocrete at cost of Rs. 1500/boreholes.";
        //                        else if (l == 2)
        //                            txt_NOTEGT.Text = "If site topography has hillocks or if distance between boreholes is more than 30m movement of rigs shall require tracter/JCB and the same shall be charged to client at cost of Rs. 2200/boreholes. ";
        //                    }
        //                }
        //                else
        //                {
        //                    AddRowNoteGT();
        //                    TextBox txt_NOTEGT = (TextBox)Grd_NoteGT.Rows[0].Cells[2].FindControl("txt_NOTEGT");

        //                    if (inwdType == "Rain Water Harvesting")
        //                        txt_NOTEGT.Text = "For certain test water is required during testing procedure, If water is not provided by you the same shall arranged by Durocrete at cost of Rs. 1500/boreholes.";
        //                    else
        //                        txt_NOTEGT.Text = "If site topography has hillocks or if distance between locations is more than 30m movement of rigs shall require tracter/JCB and the same shall be charged to client at cost of Rs. 2200/boreholes. ";

        //                }
        //            }
        //        }
        //    }
        //    else if (inwdType == "Non Destructive Testing" || inwdType == "Core Cutting" || inwdType == "Core Testing")
        //    {
        //        grdClientScope.Visible = true;
        //        lblClientScope.Visible = true;
        //        if (grdClientScope.Rows.Count == 0)
        //        {
        //            /////Client Scope
        //            if (grdClientScope.Visible == true)
        //            {
        //                for (int l = 0; l < 3; l++)
        //                {
        //                    AddRowClientScope();
        //                    TextBox txt_NOTE = (TextBox)grdClientScope.Rows[l].Cells[2].FindControl("txt_NOTEClientScope");

        //                    if (l == 0)
        //                        txt_NOTE.Text = "Client has to provide two labours for help, water, electricity & scaffolding etc.";
        //                    else if (l == 1)
        //                        txt_NOTE.Text = "Chiselling of plaster to expose the concrete if required.";
        //                    else if (l == 2)
        //                        txt_NOTE.Text = "Providing safe access and safe working conditions for our team at site.";
        //                }

        //            }
        //        }
        //    }
        //}
        private void addTermsConditionNotes(string inwdType)
        {

            if (inwdType == "Soil Investigation" || inwdType == "SBC by SPT" || inwdType == "Rain Water Harvesting" || inwdType == "Plate Load Testing" || inwdType == "Earth Resistivity Test")// tems for GT only
            {
                if (Grd_Note.Rows.Count == 0)
                {
                    for (int l = 0; l < 12; l++)
                    {
                        AddRowNote();
                        TextBox txt_NOTE = (TextBox)Grd_Note.Rows[l].Cells[2].FindControl("txt_NOTE");

                        if (l == 0)
                        {
                            txt_NOTE.Text = "You shall issue us a firm work order written or email from company mail ID.";//work order along with 50% advance.";
                        }
                        else if (l == 1)
                        {
                            if (inwdType == "Plate Load Testing")
                                txt_NOTE.Text = "All Local problems like right of way, obstructions of local peoples,etc shall be settled by you before our setup reach site.We shall deploy our machines only after getting confirmation from your end.";
                            else
                                txt_NOTE.Text = "All Local problems like right of way, obstructions of local peoples,etc shall be settled by you before our machines reach site.We shall deploy our machines only after getting confirmation from your end.";
                        }
                        else if (l == 2)
                        {
                            if (inwdType == "Plate Load Testing" || inwdType == "SBC by SPT")
                                txt_NOTE.Text = "Clearance of the site from shrubs and bushes at the locations and reasonable access to the locations,shall be in client scope.";
                            else if (inwdType == "Earth Resistivity Test")
                                txt_NOTE.Text = "Clearance of the site from shrubs and bushes at the locations and the way to the locations,shall be in client scope.";
                            else
                                txt_NOTE.Text = "Clearance of the site from shrubs and bushes at the bore hole locations and the way to the borehole locations,shall be in client scope.";
                        }
                        else if (l == 3)
                        {
                            txt_NOTE.Text = "We shall carry our camping equipment with us to site.You shall provide us with open space at site for our labor camp free of cost.";
                        }
                        else if (l == 4)
                        {
                            if (inwdType == "Rain Water Harvesting" || inwdType == "Plate Load Testing" || inwdType == "Earth Resistivity Test")
                                txt_NOTE.Text = "All the underground utility services shall be checked and marked by you prior to start of the drilling activity. We shall not be responsible for any damage caused to them.";
                            else if (inwdType == "SBC by SPT")
                                txt_NOTE.Text = "All the underground utility services shall be checked and marked by you prior to start of the testing. We shall not be responsible for any damage caused to them during drilling operation.";
                            else
                                txt_NOTE.Text = "All the underground utility services shall be checked and marked by you prior to start of the drilling activity. We shall not be responsible for any damage caused to them during drilling operation.";

                        }
                        else if (l == 5)
                        {
                            if (inwdType == "Rain Water Harvesting" || inwdType == "Plate Load Testing" || inwdType == "Earth Resistivity Test")
                                txt_NOTE.Text = "Locations of all the points shall be marked by your representative at site. The testing shall be carried out by us only at the locations marked by you.";
                            else if (inwdType != "SBC by SPT")
                                txt_NOTE.Text = "Locations of all the boreholes shall be marked by your representative at site. The drilling shall be carried out by us only at the locations marked by you.";

                        }
                        else if (l == 6)
                        {
                            if (inwdType == "Plate Load Testing")
                                txt_NOTE.Text = "In case if required JCB or Poclaine machine shall be provided by you for shifting / setting of PLT setup at location on site.";
                            else if (inwdType != "Earth Resistivity Test" && inwdType != "Rain Water Harvesting" && inwdType != "SBC by SPT")
                                txt_NOTE.Text = "The water required for the drilling activity shall be provided by you at the borehole drilling location free of cost.In case if required JCB or Poclaine machine shall be provided by you for shifting or setting drilling machine at borehole location on site.";

                        }
                        else if (l == 7)
                        {
                            txt_NOTE.Text = "All work executed will be witnessed by your representative jointly so as to avoid any discrepancy between the work executed and your requirement.";
                        }
                        else if (l == 8)
                        {
                            txt_NOTE.Text = "Any other requirement of detail calculations/additional recommendations which are not part of above proposal will be charged extra.";
                        }
                        else if (l == 9)
                        {
                            txt_NOTE.Text = "Draft report will be submitted in soft format for your perusal along with final bill after completion of field work and laboratory tests.";
                        }
                        else if (l == 10)
                        {
                            txt_NOTE.Text = "Final report(Hard copy-One color) will be submitted after receipt of entire payment.";
                        }
                        else if (l == 11)
                        {
                            if (inwdType == "Soil Investigation" || inwdType == "Rain Water Harvesting" || inwdType == "Plate Load Testing")
                                txt_NOTE.Text = "The validity of proposal is for 30 days from the date of proposal submission.";
                            else
                                DeleteRowNote(Grd_Note.Rows.Count - 1);
                        }
                    }

                    if (inwdType == "SBC by SPT")
                    {
                        DeleteRowNote(5);
                        DeleteRowNote(5);
                    }
                    if (inwdType == "Earth Resistivity Test" || inwdType == "Rain Water Harvesting")
                    {
                        DeleteRowNote(6);
                    }

                }
                if (inwdType == "Soil Investigation" || inwdType == "Rain Water Harvesting" || inwdType == "SBC by SPT")
                {
                    Grd_NoteGT.Visible = true;
                    lblAddChargesGT.Visible = true;
                    if (Grd_NoteGT.Rows.Count == 0)
                    {
                        /////Additional Charges GT
                        if (Grd_NoteGT.Visible == true)
                        {
                            if (inwdType == "Soil Investigation")
                            {
                                for (int l = 0; l < 3; l++)
                                {
                                    AddRowNoteGT();
                                    TextBox txt_NOTEGT = (TextBox)Grd_NoteGT.Rows[l].Cells[2].FindControl("txt_NOTEGT");

                                    if (l == 0)
                                        txt_NOTEGT.Text = "In case total depth of boreholes exceeds 10m, additional charges for drilling will be charged on pro rata basis ie charges per borehole divided by 10, per running meter.";
                                    else if (l == 1)
                                        txt_NOTEGT.Text = "If water is not provided by you the same shall arranged by Durocrete at cost of Rs. 1500/boreholes.";
                                    else if (l == 2)
                                        txt_NOTEGT.Text = "If site topography has hillocks or if distance between boreholes is more than 30m movement of rigs shall require Crane/tractor/JCB and the same shall be charged to client at cost of Rs. 2200/boreholes. ";
                                }
                            }
                            else
                            {
                                AddRowNoteGT();
                                TextBox txt_NOTEGT = (TextBox)Grd_NoteGT.Rows[0].Cells[2].FindControl("txt_NOTEGT");

                                if (inwdType == "Rain Water Harvesting")
                                    txt_NOTEGT.Text = "For certain test water is required during testing procedure, If water is not provided by you the same shall arranged by Durocrete at cost of Rs. 1500/boreholes.";
                                else
                                    txt_NOTEGT.Text = "If site topography has hillocks or if distance between locations is more than 30m movement of rigs shall require Crane/tractor/JCB and the same shall be charged to client at cost of Rs. 2200/boreholes. ";

                            }
                        }
                    }
                }
                lblPayTermsGT.Visible = true;
                grdPayTermsGT.Visible = true;
                if (grdPayTermsGT.Rows.Count == 0)
                {
                    /////payment terms GT
                    string[] arrPayTerm = {
                    "Mobilization charges and 50% advance to be paid before mobilization of equipment at site.",
                    "25% of bill value to be paid after submission of provisional report.",
                    "Balanced 25% of bill value to be paid within two weeks of submission of final report.",
                    "Visit of our competent Technical Officer after conducting the geotechnical investigation or during site exploration for inspection or verification will be charged extra as below:-",
                    "   a)    PMC & PCMC - Rs. 5000 (Travelling & GST Extra).",
                    "   b)    Out of Pune – Rs. 7500 (Travelling & GST Extra)."};

                    for (int i = 0; i < arrPayTerm.Length; i++)
                    {
                        AddRowPayTermGT();
                        TextBox txtNotePayTermGT = (TextBox)grdPayTermsGT.Rows[i].FindControl("txtNotePayTermGT");
                        txtNotePayTermGT.Text = arrPayTerm[i];
                    }

                }
            }
            else
            {
                if (Grd_Note.Rows.Count == 0)
                {
                    string[] arrTerm = {
                        "We have well organised door to door collection service for PMC and PCMC Area . We request you to arrange material on time to avoid delay as the vehicle has futher sites to visit.",
                        "Travelling charges will be applicable extra.",
                        "Minimum billing Rs. 20000/- will be applicable if the scope of work is reduced or not carried out due to unavailability of client's support.",
                        "The validity of proposal is for 30 days from the date of proposal submission.",
                        "Required 3 pieces of 1400mm for steel bar dia up to 25mm and 1600mm for 32mm diameter."};

                    for (int i = 0; i < arrTerm.Length; i++)
                    {
                        if (i <= 3 || (i == 4 && inwdType == "Steel Testing"))
                        {
                            AddRowNote();
                            TextBox txt_NOTE = (TextBox)Grd_Note.Rows[i].FindControl("txt_NOTE");
                            txt_NOTE.Text = arrTerm[i];
                        }
                    }
                }

                grdClientScope.Visible = true;
                lblClientScope.Visible = true;
                if (grdClientScope.Rows.Count == 0)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        string strNote = "";
                        if (i == 0 && (inwdType == "Core Cutting" || inwdType == "Core Testing"))
                        {
                            strNote = "Client has to provide two labours for help, water, electricity & scaffolding etc.";
                        }
                        else if (i == 1 && inwdType == "Non Destructive Testing")
                        {
                            strNote = "Chiselling, cleaning and rework of plaster to expose the concrete if required.";
                        }
                        else if (i == 2 && inwdType == "Non Destructive Testing")
                        {
                            strNote = "Client has to provide two labours for help & scaffolding etc.";
                        }
                        else if (i == 3)
                        {
                            strNote = "Providing necessary permissions, safe access and safe working conditions for our team at site.";
                        }
                        else if (i == 4 && inwdType == "Pile Testing")
                        {
                            strNote = "Chiselling of piles till sound concrete.";
                        }
                        else if (i == 5 && inwdType == "Pile Dynamic Test")
                        {
                            strNote = "Hydra, nylon rope and hammer as per load required.";
                        }
                        else if (i == 6 && inwdType == "Plate Load Testing")
                        {
                            strNote = "Poclain/Sand bags as reaction load and labours for loading and unloading.";
                        }
                        else if (i == 7 && (inwdType == "Non Destructive Testing" || inwdType == "Core Cutting" || inwdType == "Core Testing"
                            || inwdType == "Soil Testing" || inwdType == "Plate Load Testing" || inwdType == "Slab Load Test"
                            || inwdType.Contains("Pull Out Test") == true))
                        {
                            strNote = "For outstation projects lodging and boarding.";
                        }
                        else if (i == 8 && (inwdType == "Structural Audit" || inwdType == "Retrofiting test"))
                        {
                            strNote = "Architectural & structural drawings in CAD format.";
                        }
                        else if (i == 9 && inwdType == "Slab Deflection test")
                        {
                            strNote = "Aggregate/Sand bags and labours, crain for loading and unloading etc.";
                        }
                        else if (i == 10 && inwdType == "Mix Design")
                        {
                            strNote = "Labours, mix design report, calibration certificate of mixture/batching plant, cube mould, slump cone, weighing balance and tamping rod etc.";
                        }
                        else if (i == 11 && (inwdType == "Pile Testing" || inwdType == "Pile Dynamic Test"))
                        {
                            strNote = "Hydra, nylon rope and hammer as per load required.";
                        }
                        if (strNote != "")
                        {
                            AddRowClientScope();
                            TextBox txt_NOTE = (TextBox)grdClientScope.Rows[grdClientScope.Rows.Count - 1].FindControl("txt_NOTEClientScope");
                            txt_NOTE.Text = strNote;
                        }
                    }

                }
                txtPaymentTerm.Text = "Payment- Provide 100% advance along with work order/email confirmation.";
            }

        }
        protected void OnTextChanged(object sender, EventArgs e)
        {
            float total = 0;
            foreach (GridViewRow gridViewRow in grdGT.Rows)
            {
                TextBox txt_Amount = (TextBox)gridViewRow.FindControl("txt_Amount");
                float rowValue = 0;
                if (float.TryParse(txt_Amount.Text.Trim(), out rowValue))
                    total += rowValue;
            }
            txt_NetAmount.Text = Math.Round(total).ToString("0.00");

        }
        public void ShowHeader()
        {
            if (grdProposal.Rows.Count <= 0)
            {
                AddRowProposal();
                grdProposal.Rows[0].Visible = false;
                ViewState["ProposalTable"] = null;
            }
        }
        protected void AddRowProposal()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["ProposalTable"] != null)
            {
                GetCurrentDataProposal();
                dt = (DataTable)ViewState["ProposalTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_Particular", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_TestMethod", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Rate", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Discount", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_DiscRate", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Quantity", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Amount", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_InwdType", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_SiteWiseRate", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_TestId", typeof(string)));
            }
            dr = dt.NewRow();

            dr["txt_Particular"] = string.Empty;
            dr["txt_TestMethod"] = string.Empty;
            dr["txt_Rate"] = string.Empty;
            dr["txt_Discount"] = string.Empty;
            dr["txt_DiscRate"] = string.Empty;
            dr["txt_Quantity"] = string.Empty;
            dr["txt_Amount"] = string.Empty;
            if (ddl_InwardTestType.SelectedIndex != -1)
            {
                if (ddlOtherTest.Visible == true && ddlOtherTest.SelectedIndex != -1 && ddlOtherTest.SelectedIndex != 0)
                    dr["lbl_InwdType"] = "OTHER";
                else
                    dr["lbl_InwdType"] = ddl_InwardTestType.SelectedItem.Value;
            }
            else
                dr["lbl_InwdType"] = string.Empty;
            dr["lbl_SiteWiseRate"] = string.Empty;
            dr["lbl_TestId"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["ProposalTable"] = dt;
            grdProposal.DataSource = dt;
            grdProposal.DataBind();
            SetPreviousDataProposal();
        }
        protected void SetPreviousDataProposal()
        {
            DataTable dt = (DataTable)ViewState["ProposalTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Particular = (TextBox)grdProposal.Rows[i].Cells[3].FindControl("txt_Particular");
                TextBox txt_TestMethod = (TextBox)grdProposal.Rows[i].Cells[4].FindControl("txt_TestMethod");
                TextBox txt_Rate = (TextBox)grdProposal.Rows[i].Cells[5].FindControl("txt_Rate");
                TextBox txt_Discount = (TextBox)grdProposal.Rows[i].Cells[6].FindControl("txt_Discount");
                TextBox txt_DiscRate = (TextBox)grdProposal.Rows[i].Cells[7].FindControl("txt_DiscRate");
                TextBox txt_Quantity = (TextBox)grdProposal.Rows[i].Cells[8].FindControl("txt_Quantity");
                TextBox txt_Amount = (TextBox)grdProposal.Rows[i].Cells[9].FindControl("txt_Amount");
                Label lbl_InwdType = (Label)grdProposal.Rows[i].Cells[10].FindControl("lbl_InwdType");
                Label lbl_SiteWiseRate = (Label)grdProposal.Rows[i].Cells[11].FindControl("lbl_SiteWiseRate");
                Label lbl_TestId = (Label)grdProposal.Rows[i].Cells[12].FindControl("lbl_TestId");

                txt_Particular.Text = dt.Rows[i]["txt_Particular"].ToString();
                txt_TestMethod.Text = dt.Rows[i]["txt_TestMethod"].ToString();
                txt_Rate.Text = dt.Rows[i]["txt_Rate"].ToString();
                txt_Discount.Text = dt.Rows[i]["txt_Discount"].ToString();
                txt_DiscRate.Text = dt.Rows[i]["txt_DiscRate"].ToString();
                txt_Quantity.Text = dt.Rows[i]["txt_Quantity"].ToString();
                txt_Amount.Text = dt.Rows[i]["txt_Amount"].ToString();
                lbl_InwdType.Text = dt.Rows[i]["lbl_InwdType"].ToString();
                lbl_SiteWiseRate.Text = dt.Rows[i]["lbl_SiteWiseRate"].ToString();
                lbl_TestId.Text = dt.Rows[i]["lbl_TestId"].ToString();
            }
        }
        protected void GetCurrentDataProposal()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_Particular", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_TestMethod", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Rate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Discount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_DiscRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Quantity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Amount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_InwdType", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_SiteWiseRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_TestId", typeof(string)));

            for (int i = 0; i < grdProposal.Rows.Count; i++)
            {
                TextBox txt_Particular = (TextBox)grdProposal.Rows[i].Cells[3].FindControl("txt_Particular");
                TextBox txt_TestMethod = (TextBox)grdProposal.Rows[i].Cells[4].FindControl("txt_TestMethod");
                TextBox txt_Rate = (TextBox)grdProposal.Rows[i].Cells[5].FindControl("txt_Rate");
                TextBox txt_Discount = (TextBox)grdProposal.Rows[i].Cells[6].FindControl("txt_Discount");
                TextBox txt_DiscRate = (TextBox)grdProposal.Rows[i].Cells[7].FindControl("txt_DiscRate");
                TextBox txt_Quantity = (TextBox)grdProposal.Rows[i].Cells[8].FindControl("txt_Quantity");
                TextBox txt_Amount = (TextBox)grdProposal.Rows[i].Cells[9].FindControl("txt_Amount");
                Label lbl_InwdType = (Label)grdProposal.Rows[i].Cells[10].FindControl("lbl_InwdType");
                Label lbl_SiteWiseRate = (Label)grdProposal.Rows[i].Cells[11].FindControl("lbl_SiteWiseRate");
                Label lbl_TestId = (Label)grdProposal.Rows[i].Cells[12].FindControl("lbl_TestId");


                drRow = dtTable.NewRow();
                drRow["txt_Particular"] = txt_Particular.Text;
                drRow["txt_TestMethod"] = txt_TestMethod.Text;
                drRow["txt_Rate"] = txt_Rate.Text;
                drRow["txt_Discount"] = txt_Discount.Text;
                drRow["txt_DiscRate"] = txt_DiscRate.Text;
                drRow["txt_Quantity"] = txt_Quantity.Text;
                drRow["txt_Amount"] = txt_Amount.Text;
                drRow["lbl_InwdType"] = lbl_InwdType.Text;
                drRow["lbl_SiteWiseRate"] = lbl_SiteWiseRate.Text;
                drRow["lbl_TestId"] = lbl_TestId.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["ProposalTable"] = dtTable;
        }

        protected void AddRowProposalGT()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["ProposalTableGT"] != null)
            {
                GetCurrentDataProposalGT();
                dt = (DataTable)ViewState["ProposalTableGT"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("txt_Particular", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_TestMethod", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Unit", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Quantity", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_UnitRate", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_DiscRate", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_Amount", typeof(string)));
                dt.Columns.Add(new DataColumn("lbl_InwdType", typeof(string)));

            }
            dr = dt.NewRow();

            dr["txt_Particular"] = string.Empty;
            dr["txt_TestMethod"] = string.Empty;
            dr["txt_DiscRate"] = string.Empty;
            dr["txt_UnitRate"] = string.Empty;
            dr["txt_Unit"] = string.Empty;
            dr["txt_Quantity"] = string.Empty;
            dr["txt_Amount"] = string.Empty;
            if (ddl_InwardTestType.SelectedIndex != -1)
            {
                if (ddlOtherTest.Visible == true && ddlOtherTest.SelectedIndex != -1 && ddlOtherTest.SelectedIndex != 0)
                    dr["lbl_InwdType"] = "OTHER";
                else
                    dr["lbl_InwdType"] = ddl_InwardTestType.SelectedItem.Value;
            }
            else
                dr["lbl_InwdType"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["ProposalTableGT"] = dt;
            grdGT.DataSource = dt;
            grdGT.DataBind();
            SetPreviousDataProposalGT();
        }
        protected void SetPreviousDataProposalGT()
        {
            DataTable dt = (DataTable)ViewState["ProposalTableGT"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_Particular = (TextBox)grdGT.Rows[i].Cells[2].FindControl("txt_Particular");
                TextBox txt_TestMethod = (TextBox)grdGT.Rows[i].Cells[3].FindControl("txt_TestMethod");
                TextBox txt_Unit = (TextBox)grdGT.Rows[i].Cells[4].FindControl("txt_Unit");
                TextBox txt_Quantity = (TextBox)grdGT.Rows[i].Cells[5].FindControl("txt_Quantity");
                TextBox txt_UnitRate = (TextBox)grdGT.Rows[i].Cells[6].FindControl("txt_UnitRate");
                TextBox txt_DiscRate = (TextBox)grdGT.Rows[i].Cells[7].FindControl("txt_DiscRate");
                TextBox txt_Amount = (TextBox)grdGT.Rows[i].Cells[8].FindControl("txt_Amount");
                Label lbl_InwdType = (Label)grdGT.Rows[i].Cells[9].FindControl("lbl_InwdType");

                txt_Particular.Text = dt.Rows[i]["txt_Particular"].ToString();
                txt_TestMethod.Text = dt.Rows[i]["txt_TestMethod"].ToString();
                txt_Unit.Text = dt.Rows[i]["txt_Unit"].ToString();
                txt_Quantity.Text = dt.Rows[i]["txt_Quantity"].ToString();
                txt_UnitRate.Text = dt.Rows[i]["txt_UnitRate"].ToString();
                txt_DiscRate.Text = dt.Rows[i]["txt_DiscRate"].ToString();
                txt_Amount.Text = dt.Rows[i]["txt_Amount"].ToString();
                lbl_InwdType.Text = dt.Rows[i]["lbl_InwdType"].ToString();
            }
        }
        protected void GetCurrentDataProposalGT()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("txt_Particular", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_TestMethod", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Unit", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Quantity", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_UnitRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_DiscRate", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_Amount", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lbl_InwdType", typeof(string)));

            for (int i = 0; i < grdGT.Rows.Count; i++)
            {
                TextBox txt_Particular = (TextBox)grdGT.Rows[i].Cells[2].FindControl("txt_Particular");
                TextBox txt_TestMethod = (TextBox)grdGT.Rows[i].Cells[3].FindControl("txt_TestMethod");
                TextBox txt_Unit = (TextBox)grdGT.Rows[i].Cells[4].FindControl("txt_Unit");
                TextBox txt_Quantity = (TextBox)grdGT.Rows[i].Cells[5].FindControl("txt_Quantity");
                TextBox txt_UnitRate = (TextBox)grdGT.Rows[i].Cells[6].FindControl("txt_UnitRate");
                TextBox txt_DiscRate = (TextBox)grdGT.Rows[i].Cells[7].FindControl("txt_DiscRate");
                TextBox txt_Amount = (TextBox)grdGT.Rows[i].Cells[8].FindControl("txt_Amount");
                Label lbl_InwdType = (Label)grdGT.Rows[i].Cells[9].FindControl("lbl_InwdType");


                drRow = dtTable.NewRow();
                drRow["txt_Particular"] = txt_Particular.Text;
                drRow["txt_TestMethod"] = txt_TestMethod.Text;
                drRow["txt_Unit"] = txt_Unit.Text;
                drRow["txt_Quantity"] = txt_Quantity.Text;
                drRow["txt_UnitRate"] = txt_UnitRate.Text;
                drRow["txt_DiscRate"] = txt_DiscRate.Text;
                drRow["txt_Amount"] = txt_Amount.Text;
                drRow["lbl_InwdType"] = lbl_InwdType.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["ProposalTableGT"] = dtTable;
        }
        //payment terms gt
        protected void AddRowPayTermGT()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["PayTermGTTable"] != null)
            {
                GetCurrentDataPayTermGT();
                dt = (DataTable)ViewState["PayTermGTTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNoPayTermGT", typeof(string)));
                dt.Columns.Add(new DataColumn("txtNotePayTermGT", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNoPayTermGT"] = dt.Rows.Count + 1;
            dr["txtNotePayTermGT"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["PayTermGTTable"] = dt;
            grdPayTermsGT.DataSource = dt;
            grdPayTermsGT.DataBind();
            SetPreviousDataPayTermGT();
        }
        protected void SetPreviousDataPayTermGT()
        {
            DataTable dt = (DataTable)ViewState["PayTermGTTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txtNotePayTermGT = (TextBox)grdPayTermsGT.Rows[i].Cells[2].FindControl("txtNotePayTermGT");
                grdPayTermsGT.Rows[i].Cells[2].Text = (i + 1).ToString();
                txtNotePayTermGT.Text = dt.Rows[i]["txtNotePayTermGT"].ToString();

            }
        }
        protected void GetCurrentDataPayTermGT()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNoPayTermGT", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtNotePayTermGT", typeof(string)));
            for (int i = 0; i < grdPayTermsGT.Rows.Count; i++)
            {
                TextBox txtNotePayTermGT = (TextBox)grdPayTermsGT.Rows[i].Cells[2].FindControl("txtNotePayTermGT");

                drRow = dtTable.NewRow();
                drRow["lblSrNoPayTermGT"] = i + 1;
                drRow["txtNotePayTermGT"] = txtNotePayTermGT.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["PayTermGTTable"] = dtTable;
        }
        protected void DeleteRowPayTermGT(int rowIndex)
        {
            GetCurrentDataPayTermGT();
            DataTable dt = ViewState["PayTermGTTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["PayTermGTTable"] = dt;
            grdPayTermsGT.DataSource = dt;
            grdPayTermsGT.DataBind();
            SetPreviousDataPayTermGT();
        }
        protected void imgBtnAddRowPayTermGT_Click(object sender, CommandEventArgs e)
        {
            AddRowPayTermGT();
        }
        protected void imgBtnDeleteRowPayTermGT_Click(object sender, CommandEventArgs e)
        {
            if (grdPayTermsGT.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdPayTermsGT.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowPayTermGT(gvr.RowIndex);
            }
        }
        //
        protected void AddRowDiscountGT()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["Discount_TableGT"] != null)
            {
                GetCurrentDataDiscountGT();
                dt = (DataTable)ViewState["Discount_TableGT"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblMaterialName", typeof(string)));
                dt.Columns.Add(new DataColumn("lblTestName", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSiteWiseDisc", typeof(string)));
                dt.Columns.Add(new DataColumn("txtVolDisc", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAppliedDisc", typeof(string)));

            }
            dr = dt.NewRow();
            dr["lblMaterialName"] = string.Empty;
            dr["lblTestName"] = string.Empty;
            dr["txtSiteWiseDisc"] = string.Empty;
            dr["txtVolDisc"] = string.Empty;
            dr["txtAppliedDisc"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["Discount_TableGT"] = dt;
            grdDiscount.DataSource = dt;
            grdDiscount.DataBind();
            SetPreviousDataDiscountGT();
        }
        protected void GetCurrentDataDiscountGT()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblMaterialName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblTestName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtSiteWiseDisc", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtVolDisc", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAppliedDisc", typeof(string)));

            for (int i = 0; i < grdDiscount.Rows.Count; i++)
            {
                Label lblMaterialName = (Label)grdDiscount.Rows[i].Cells[0].FindControl("lblMaterialName");
                Label lblTestName = (Label)grdDiscount.Rows[i].Cells[1].FindControl("lblTestName");
                TextBox txtSiteWiseDisc = (TextBox)grdDiscount.Rows[i].Cells[2].FindControl("txtSiteWiseDisc");
                TextBox txtVolDisc = (TextBox)grdDiscount.Rows[i].Cells[3].FindControl("txtVolDisc");
                TextBox txtAppliedDisc = (TextBox)grdDiscount.Rows[i].Cells[4].FindControl("txtAppliedDisc");
                drRow = dtTable.NewRow();

                drRow["lblMaterialName"] = lblMaterialName.Text;
                drRow["lblTestName"] = lblTestName.Text;
                drRow["txtSiteWiseDisc"] = txtSiteWiseDisc.Text;
                drRow["txtVolDisc"] = txtVolDisc.Text;
                drRow["txtAppliedDisc"] = txtAppliedDisc.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["Discount_TableGT"] = dtTable;

        }
        protected void SetPreviousDataDiscountGT()
        {
            DataTable dt = (DataTable)ViewState["Discount_TableGT"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblMaterialName = (Label)grdDiscount.Rows[i].Cells[0].FindControl("lblMaterialName");
                Label lblTestName = (Label)grdDiscount.Rows[i].Cells[1].FindControl("lblTestName");
                TextBox txtSiteWiseDisc = (TextBox)grdDiscount.Rows[i].Cells[2].FindControl("txtSiteWiseDisc");
                TextBox txtVolDisc = (TextBox)grdDiscount.Rows[i].Cells[3].FindControl("txtVolDisc");
                TextBox txtAppliedDisc = (TextBox)grdDiscount.Rows[i].Cells[4].FindControl("txtAppliedDisc");

                lblMaterialName.Text = dt.Rows[i]["lblMaterialName"].ToString();
                lblTestName.Text = dt.Rows[i]["lblTestName"].ToString();
                txtSiteWiseDisc.Text = dt.Rows[i]["txtSiteWiseDisc"].ToString();
                txtVolDisc.Text = dt.Rows[i]["txtVolDisc"].ToString();
                txtAppliedDisc.Text = dt.Rows[i]["txtAppliedDisc"].ToString();

            }
        }

        protected void AddRowDiscount()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["Discount_Table"] != null)
            {
                GetCurrentDataDiscount();
                dt = (DataTable)ViewState["Discount_Table"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblMaterialName", typeof(string)));
                dt.Columns.Add(new DataColumn("lblTestName", typeof(string)));
                dt.Columns.Add(new DataColumn("txtSiteWiseDisc", typeof(string)));
                dt.Columns.Add(new DataColumn("txtVolDisc", typeof(string)));
                dt.Columns.Add(new DataColumn("txtAppliedDisc", typeof(string)));

            }
            dr = dt.NewRow();
            dr["lblMaterialName"] = string.Empty;
            dr["lblTestName"] = string.Empty;
            dr["txtSiteWiseDisc"] = string.Empty;
            dr["txtVolDisc"] = string.Empty;
            dr["txtAppliedDisc"] = string.Empty;
            dt.Rows.Add(dr);
            ViewState["Discount_Table"] = dt;
            grdDiscount.DataSource = dt;
            grdDiscount.DataBind();
            SetPreviousDataDiscount();
        }
        protected void GetCurrentDataDiscount()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblMaterialName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("lblTestName", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtSiteWiseDisc", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtVolDisc", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txtAppliedDisc", typeof(string)));

            for (int i = 0; i < grdDiscount.Rows.Count; i++)
            {
                Label lblMaterialName = (Label)grdDiscount.Rows[i].Cells[0].FindControl("lblMaterialName");
                Label lblTestName = (Label)grdDiscount.Rows[i].Cells[1].FindControl("lblTestName");
                TextBox txtSiteWiseDisc = (TextBox)grdDiscount.Rows[i].Cells[2].FindControl("txtSiteWiseDisc");
                TextBox txtVolDisc = (TextBox)grdDiscount.Rows[i].Cells[3].FindControl("txtVolDisc");
                TextBox txtAppliedDisc = (TextBox)grdDiscount.Rows[i].Cells[4].FindControl("txtAppliedDisc");
                drRow = dtTable.NewRow();

                drRow["lblMaterialName"] = lblMaterialName.Text;
                drRow["lblTestName"] = lblTestName.Text;
                drRow["txtSiteWiseDisc"] = txtSiteWiseDisc.Text;
                drRow["txtVolDisc"] = txtVolDisc.Text;
                drRow["txtAppliedDisc"] = txtAppliedDisc.Text;


                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["Discount_Table"] = dtTable;

        }
        protected void SetPreviousDataDiscount()
        {
            DataTable dt = (DataTable)ViewState["Discount_Table"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Label lblMaterialName = (Label)grdDiscount.Rows[i].Cells[0].FindControl("lblMaterialName");
                Label lblTestName = (Label)grdDiscount.Rows[i].Cells[1].FindControl("lblTestName");
                TextBox txtSiteWiseDisc = (TextBox)grdDiscount.Rows[i].Cells[2].FindControl("txtSiteWiseDisc");
                TextBox txtVolDisc = (TextBox)grdDiscount.Rows[i].Cells[3].FindControl("txtVolDisc");
                TextBox txtAppliedDisc = (TextBox)grdDiscount.Rows[i].Cells[4].FindControl("txtAppliedDisc");

                lblMaterialName.Text = dt.Rows[i]["lblMaterialName"].ToString();
                lblTestName.Text = dt.Rows[i]["lblTestName"].ToString();
                txtSiteWiseDisc.Text = dt.Rows[i]["txtSiteWiseDisc"].ToString();
                txtVolDisc.Text = dt.Rows[i]["txtVolDisc"].ToString();
                txtAppliedDisc.Text = dt.Rows[i]["txtAppliedDisc"].ToString();

            }


        }

        protected void AddRowNote()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["NoteTable"] != null)
            {
                GetCurrentDataNote();
                dt = (DataTable)ViewState["NoteTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_NOTE", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNo"] = dt.Rows.Count + 1;
            dr["txt_NOTE"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["NoteTable"] = dt;
            Grd_Note.DataSource = dt;
            Grd_Note.DataBind();
            SetPreviousDataNote();
        }
        protected void SetPreviousDataNote()
        {
            DataTable dt = (DataTable)ViewState["NoteTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_NOTE = (TextBox)Grd_Note.Rows[i].Cells[2].FindControl("txt_NOTE");
                Grd_Note.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_NOTE.Text = dt.Rows[i]["txt_NOTE"].ToString();

            }
        }
        protected void GetCurrentDataNote()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNo", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_NOTE", typeof(string)));
            for (int i = 0; i < Grd_Note.Rows.Count; i++)
            {
                TextBox txt_NOTE = (TextBox)Grd_Note.Rows[i].Cells[2].FindControl("txt_NOTE");

                drRow = dtTable.NewRow();
                drRow["lblSrNo"] = i + 1;
                drRow["txt_NOTE"] = txt_NOTE.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["NoteTable"] = dtTable;
        }
        protected void DeleteRowNote(int rowIndex)
        {
            GetCurrentDataNote();
            DataTable dt = ViewState["NoteTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["NoteTable"] = dt;
            Grd_Note.DataSource = dt;
            Grd_Note.DataBind();
            SetPreviousDataNote();
        }
        protected void imgBtnAddRow_Click(object sender, CommandEventArgs e)
        {
            AddRowNote();
        }
        protected void imgBtnDeleteRow_Click(object sender, CommandEventArgs e)
        {
            if (Grd_Note.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (Grd_Note.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowNote(gvr.RowIndex);
            }
        }

        ////Additional Charges GT
        protected void AddRowNoteGT()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["NoteTableGT"] != null)
            {
                GetCurrentDataNoteGT();
                dt = (DataTable)ViewState["NoteTableGT"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNoGT", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_NOTEGT", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNoGT"] = dt.Rows.Count + 1;
            dr["txt_NOTEGT"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["NoteTableGT"] = dt;
            Grd_NoteGT.DataSource = dt;
            Grd_NoteGT.DataBind();
            SetPreviousDataNoteGT();
        }
        protected void SetPreviousDataNoteGT()
        {
            DataTable dt = (DataTable)ViewState["NoteTableGT"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_NOTE = (TextBox)Grd_NoteGT.Rows[i].Cells[2].FindControl("txt_NOTEGT");
                Grd_NoteGT.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_NOTE.Text = dt.Rows[i]["txt_NOTEGT"].ToString();

            }
        }
        protected void GetCurrentDataNoteGT()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNoGT", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_NOTEGT", typeof(string)));
            for (int i = 0; i < Grd_NoteGT.Rows.Count; i++)
            {
                TextBox txt_NOTE = (TextBox)Grd_NoteGT.Rows[i].Cells[2].FindControl("txt_NOTEGT");

                drRow = dtTable.NewRow();
                drRow["lblSrNoGT"] = i + 1;
                drRow["txt_NOTEGT"] = txt_NOTE.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["NoteTableGT"] = dtTable;
        }
        protected void DeleteRowNoteGT(int rowIndex)
        {
            GetCurrentDataNoteGT();
            DataTable dt = ViewState["NoteTableGT"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["NoteTableGT"] = dt;
            Grd_NoteGT.DataSource = dt;
            Grd_NoteGT.DataBind();
            SetPreviousDataNoteGT();
        }

        protected void imgBtnAddNotesRowGT_Click(object sender, CommandEventArgs e)
        {
            AddRowNoteGT();
        }
        protected void imgBtnDeleteNotesRowGT_Click(object sender, CommandEventArgs e)
        {
            if (Grd_NoteGT.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (Grd_NoteGT.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowNoteGT(gvr.RowIndex);
            }
        }


        protected void imgInsert_Click(object sender, CommandEventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            if (ddl_InwardTestType.SelectedItem.Text != "---Select---")
            {
                lblMsg.Visible = false;
                AddRowProposal();
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Please Select Inward Type";
                lblMsg.ForeColor = System.Drawing.Color.Red;

            }
        }
        protected void imgExitDepth_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender3.Hide();
        }
        protected void imgExit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender2.Hide();
        }
        protected void imgExitAdd_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender1.Hide();
        }
        protected void imgEdit_Click(object sender, CommandEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            Label lbl_TestId = (Label)grdProposal.Rows[gvr.RowIndex].Cells[11].FindControl("lbl_TestId");
            if (lbl_TestId.Text == "0")
            {
                ModalPopupExtender2.Show();
                TextBox txt_Particular = (TextBox)grdProposal.Rows[gvr.RowIndex].Cells[3].FindControl("txt_Particular");
                TextBox txt_Rate = (TextBox)grdProposal.Rows[gvr.RowIndex].Cells[4].FindControl("txt_Rate");
                TextBox txt_DiscRate = (TextBox)grdProposal.Rows[gvr.RowIndex].Cells[6].FindControl("txt_DiscRate");

                txt_PerticularName.Text = txt_Particular.Text;
                txt_UnitRate.Text = txt_Rate.Text;
                txt_DiscountRate.Text = txt_DiscRate.Text;
                lblFlag.Text = gvr.RowIndex.ToString();
            }

        }
        protected void lnkUpdateDepthRate_Click(object sender, EventArgs e)
        {
            ModalPopupExtender3.Hide();
            if (ddlDepth.SelectedIndex == 1 && txtGtRate.Text != "")//means 15m
            {
                int FromRowNo = 2, ToRowNo = 8;
                if (txtFrmRow.Text != "")
                    FromRowNo = Convert.ToInt32(txtFrmRow.Text);
                if (txtToRow.Text != "")
                    ToRowNo = Convert.ToInt32(txtToRow.Text);

                for (int i = 0; i < grdGT.Rows.Count; i++)
                {
                    if (FromRowNo == i)
                    {
                        TextBox txt_Rate = (TextBox)grdGT.Rows[i - 1].Cells[5].FindControl("txt_UnitRate");
                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[i - 1].Cells[6].FindControl("txt_DiscRate");
                        txt_Rate.Text = txtGtRate.Text;
                        txt_DiscRate.Text = txtGtRate.Text;
                        break;
                    }

                }
            }
        }
        protected void lnkEditAdd_Click(object sender, EventArgs e)
        {
            int clId = 0, siteId = 0;
            txtAdd.Text = "";
            txtCity.Text = "";
            txtPin.Text = "";
            //txtEmail.Text = "";
            lblMessage.Visible = false;
            if (lblEnqNewClient.Text == "False")
            {
                var details = dc.Enquiry_View(Convert.ToInt32(txt_EnquiryNo.Text), 1, 0);
                foreach (var ds in details)
                {
                    clId = Convert.ToInt32(ds.CL_Id);
                    siteId = Convert.ToInt32(ds.SITE_Id);
                    break;
                }
                lblClientId.Text = clId.ToString();
                lblSiteId.Text = siteId.ToString();

                var addDetails = dc.Client_View(clId, 0, "", "");
                foreach (var add in addDetails)
                {
                    txtAdd.Text = Convert.ToString(add.CL_OfficeAddress_var);
                    txtCity.Text = Convert.ToString(add.CL_City_var);
                    txtPin.Text = Convert.ToString(add.CL_Pin_int);
                    break;
                }

                var siteDetails = dc.Site_View(siteId, clId, 0, "").ToList();
                txtEmail.Text = Convert.ToString(siteDetails.FirstOrDefault().SITE_EmailID_var);


            }
            else
            {
                var details = dc.EnquiryNewClient_View(Convert.ToInt32(txt_EnquiryNo.Text), false, 0);
                foreach (var add in details)
                {
                    txtAdd.Text = Convert.ToString(add.ENQNEW_ClientOfficeAddress_var);
                    txtCity.Text = Convert.ToString(add.ENQNEW_ClientCity_var);
                    txtPin.Text = Convert.ToString(add.ENQNEW_ClientPin_int);
                }
                if (lblProposalId.Text != "" && lblProposalId.Text != "0")
                {
                    var proDetails = dc.Proposal_View(Convert.ToInt32(txt_EnquiryNo.Text), true, Convert.ToInt32(lblProposalId.Text), "").ToList();
                    txtEmail.Text = Convert.ToString(proDetails.FirstOrDefault().Proposal_NewClientMailId_var);
                }

                lblClientId.Text = "0";
            }
            ModalPopupExtender1.Show();

        }
        public bool IsValidEmailAddress(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            else
            {
                var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regex.IsMatch(s) && !s.EndsWith(".");
                //string[] eid = s.Split(',');
                //for (int i = 0; i < eid.Length; i++)
                //{
                //    string strRegex = @"/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$/";
                //    Regex re = new Regex(strRegex);
                //    if (!(re.IsMatch(eid[i].ToString())))
                //        return false;
                //}
                //return true;


                //var result = s.Split(',');
                //for (var i = 0; i < result.Length; i++)
                //    if (!validateEmail(result[i]))
                //        return false;
                //return true;
            }
        }

        protected void lnkUpdateAdd_Click(object sender, EventArgs e)
        {

            bool flag = true;
            lblMessage.Visible = false;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            if (txtAdd.Text == "" && lblSiteId.Text != "0")
            {
                flag = false;
                lblMessage.Visible = true;
                lblMessage.Text = "Input Address";
            }
            else if (txtCity.Text == "" && lblSiteId.Text != "0")
            {
                flag = false;
                lblMessage.Visible = true;
                lblMessage.Text = "Input City";
            }
            else if (txtPin.Text == "" && lblSiteId.Text != "0")
            {
                flag = false;
                lblMessage.Visible = true;
                lblMessage.Text = "Input Pincode";
            }
            else if (txtEmail.Text == "")
            {
                flag = false;
                lblMessage.Visible = true;
                lblMessage.Text = "Input Email";
            }
            else if (IsValidEmailAddress(txtEmail.Text.Trim()) == false)
            {
                flag = false;
                lblMessage.Visible = true;
                lblMessage.Text = "Invalid Email";

            }


            if (flag)
            {
                if (lblEnqNewClient.Text == "False" && lblClientId.Text != "0")
                {
                    dc.Client_Update_Address(Convert.ToInt32(lblClientId.Text), txtAdd.Text, txtCity.Text, Convert.ToInt32(txtPin.Text), 0);
                }
                else if (lblEnqNewClient.Text == "True")
                {
                    if(txtAdd.Text!="" && txtCity.Text!="" && txtPin.Text!="")
                        dc.EnquiryNewClient_Update_Address(Convert.ToInt32(txt_EnquiryNo.Text), txtAdd.Text, txtCity.Text, Convert.ToInt32(txtPin.Text));
                }

                if (lblSiteId.Text != "0")
                    dc.SITE_Update_EmailId(Convert.ToInt32(lblSiteId.Text), txtEmail.Text);

                lblMessage.Visible = true;
                lblMessage.Text = "Updated Successfully..";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }

        }
        protected void lnkAddNewRow_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            if (ddl_InwardTestType.SelectedItem.Text != "---Select---")
            {
                lblMsg.Visible = false;
                ModalPopupExtender2.Show();
                txt_PerticularName.Text = "";
                txt_UnitRate.Text = "";
                txt_DiscountRate.Text = "";
                lblFlag.Text = "0";
                lblRecType.Text = "";
                if (ddl_InwardTestType.SelectedValue == "GT")
                    lblRecType.Text = "GT";
                else if (ddl_InwardTestType.SelectedValue == "OT")
                {
                    if (ddlOtherTest.Visible == true)
                    {
                        if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                            lblRecType.Text = "SPT";
                        else if (ddlOtherTest.SelectedItem.Text == "Water Test for Drinking/Domestic Purpose")
                            lblRecType.Text = "WT";
                    }
                }
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Please Select Inward Type";
                lblMsg.ForeColor = System.Drawing.Color.Red;

            }
        }
        protected void lnkDepth_Click(object sender, EventArgs e)
        {
            ModalPopupExtender3.Show();
            txtGtRate.Text = lblGtRate.Text;
            ddlDepth.SelectedIndex = 0;
            txtGtRate.ReadOnly = true;
        }
        protected void lnkAddPerticular_Click(object sender, EventArgs e)
        {
            if (grdProposal.Visible == true)
            {
                if (txt_PerticularName.Text != "" && txt_UnitRate.Text != "" && txt_DiscountRate.Text != "")
                {
                    double calDisc = 0;
                    ModalPopupExtender2.Hide();
                    if (lblFlag.Text == "0")//0 indicates new row added by lnkAddNewRow click
                    {
                        AddRowProposal();
                        TextBox txt_Particular = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[3].FindControl("txt_Particular");
                        TextBox txt_TestMethod = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[4].FindControl("txt_TestMethod");
                        TextBox txt_Rate = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[5].FindControl("txt_Rate");
                        TextBox txt_Discount = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[6].FindControl("txt_Discount");
                        TextBox txt_DisRate = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[7].FindControl("txt_DiscRate");
                        Label lbl_TestId = (Label)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[12].FindControl("lbl_TestId");

                        txt_Particular.Text = txt_PerticularName.Text;
                        txt_TestMethod.Text = txt_TestMethod.Text;
                        if (txt_TestMethod.Text == "")
                            txt_TestMethod.Text = "NA";
                        txt_Rate.Text = txt_UnitRate.Text;
                        txt_DisRate.Text = txt_DiscountRate.Text;
                        calDisc = Math.Round(((Convert.ToDouble(txt_Rate.Text) - (Convert.ToDouble(txt_DisRate.Text))) * 100) / Convert.ToDouble(txt_Rate.Text));
                        txt_Discount.Text = calDisc.ToString();
                        lbl_TestId.Text = "0";
                    }
                    else // edit added row
                    {
                        int rowindex = Convert.ToInt32(lblFlag.Text);
                        TextBox txt_Particular = (TextBox)grdProposal.Rows[rowindex].Cells[3].FindControl("txt_Particular");
                        TextBox txt_TestMethod = (TextBox)grdProposal.Rows[rowindex].Cells[4].FindControl("txt_TestMethod");
                        TextBox txt_Rate = (TextBox)grdProposal.Rows[rowindex].Cells[5].FindControl("txt_Rate");
                        TextBox txt_Discount = (TextBox)grdProposal.Rows[rowindex].Cells[6].FindControl("txt_Discount");
                        TextBox txt_DisRate = (TextBox)grdProposal.Rows[rowindex].Cells[7].FindControl("txt_DiscRate");
                        calDisc = Math.Round(((Convert.ToDouble(txt_Rate.Text) - (Convert.ToDouble(txt_DisRate.Text))) * 100) / Convert.ToDouble(txt_Rate.Text));
                        Label lbl_TestId = (Label)grdProposal.Rows[rowindex].Cells[12].FindControl("lbl_TestId");

                        txt_Particular.Text = txt_PerticularName.Text;
                        txt_TestMethod.Text = txt_TestMethod.Text;
                        if (txt_TestMethod.Text == "")
                            txt_TestMethod.Text = "NA";

                        txt_Rate.Text = txt_UnitRate.Text;
                        txt_Discount.Text = calDisc.ToString();
                        txt_DisRate.Text = txt_DiscountRate.Text;
                        lbl_TestId.Text = "0";

                    }
                }
            }
            else if (grdGT.Visible == true)
            {
                int FromRowNo = 0, ToRowNo = 0;
                if (chkLums.Checked)
                {
                    if (txtFrmRow.Text != "" && txtToRow.Text != "")
                    {
                        FromRowNo = Convert.ToInt32(txtFrmRow.Text);
                        ToRowNo = Convert.ToInt32(txtToRow.Text);


                    }
                }

                if (lblRecType.Text == "GT")
                {
                    if (txt_PerticularName.Text != "" && txt_UnitRate.Text != "" && txt_DiscountRate.Text != "")
                    {
                        ModalPopupExtender2.Hide();
                        AddRowProposalGT();
                        TextBox txt_Particular = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[2].FindControl("txt_Particular");
                        TextBox txt_Rate = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[6].FindControl("txt_UnitRate");
                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[7].FindControl("txt_DiscRate");
                        Label lbl_InwdType = (Label)grdGT.Rows[grdGT.Rows.Count - 1].Cells[8].FindControl("lbl_InwdType");

                        txt_Particular.Text = txt_PerticularName.Text;
                        txt_Rate.Text = txt_UnitRate.Text;
                        txt_DiscRate.Text = txt_DiscountRate.Text;
                        lbl_InwdType.Text = "GT";

                        ShowMerge(FromRowNo, ToRowNo, "GT");
                    }
                }
                else if (lblRecType.Text == "SPT")
                {
                    if (txt_PerticularName.Text != "" && txt_UnitRate.Text != "" && txt_DiscountRate.Text != "")
                    {
                        ModalPopupExtender2.Hide();
                        AddRowProposalGT();
                        TextBox txt_Particular = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[2].FindControl("txt_Particular");
                        TextBox txt_Rate = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[6].FindControl("txt_UnitRate");
                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[7].FindControl("txt_DiscRate");
                        Label lbl_InwdType = (Label)grdGT.Rows[grdGT.Rows.Count - 1].Cells[8].FindControl("lbl_InwdType");

                        txt_Particular.Text = txt_PerticularName.Text;
                        txt_Rate.Text = txt_UnitRate.Text;
                        txt_DiscRate.Text = txt_DiscountRate.Text;
                        lbl_InwdType.Text = "OTHER";

                        ShowMerge(FromRowNo, ToRowNo, "OTHER");
                    }
                }
                else if (lblRecType.Text == "WT")//Water Test for Drinking/Domestic Purpose
                {
                    if (txt_PerticularName.Text != "" && txt_UnitRate.Text != "" && txt_DiscountRate.Text != "")
                    {
                        ModalPopupExtender2.Hide();
                        AddRowProposalGT();
                        TextBox txt_Particular = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[2].FindControl("txt_Particular");
                        TextBox txt_Rate = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[6].FindControl("txt_UnitRate");
                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[grdGT.Rows.Count - 1].Cells[7].FindControl("txt_DiscRate");
                        Label lbl_InwdType = (Label)grdGT.Rows[grdGT.Rows.Count - 1].Cells[8].FindControl("lbl_InwdType");

                        txt_Particular.Text = txt_PerticularName.Text;
                        txt_Rate.Text = txt_UnitRate.Text;
                        txt_DiscRate.Text = txt_DiscountRate.Text;
                        lbl_InwdType.Text = "OTHER";

                        ShowMerge(FromRowNo, ToRowNo, "OTHER");
                    }
                }
            }

        }
        protected void imgDelete_Click(object sender, CommandEventArgs e)
        {
            if (grdProposal.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdProposal.Rows[gvr.RowIndex].Cells[1].Text == "")
                {
                    DeleteRowProposal(gvr.RowIndex);
                    GetCurrentDataDiscount();
                }
            }
        }
        protected void imgDeleteGT_Click(object sender, CommandEventArgs e)
        {
            if (grdGT.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                //if (grdGT.Rows[gvr.RowIndex].Cells[1].Text == "")
                //{
                DeleteRowProposalGT(gvr.RowIndex);
                //GetCurrentDataDiscount();
                // }

                if (ddl_InwardTestType.SelectedValue == "GT")
                {
                    if (grdGT.Rows.Count >= 7 || grdGT.Rows.Count >= 8)
                    {
                        ShowMerge(2, 8, "GT");
                    }
                }
                else if (ddl_InwardTestType.SelectedValue == "OT")
                {
                    if (ddlOtherTest.Visible == true)
                    {
                        if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                        {
                            if (grdGT.Rows.Count >= 3)
                            {
                                ShowMerge(2, 3, "OTHER");
                            }
                        }
                        else if (ddlOtherTest.SelectedItem.Text == "Water Test for Drinking/Domestic Purpose")
                        {
                            if (grdGT.Rows.Count >= 3)
                            {
                                ShowMerge(1, 3, "OTHER");
                            }
                        }
                    }
                }
            }
        }
        protected void DeleteRowProposalGT(int rowIndex)
        {
            GetCurrentDataProposalGT();
            DataTable dt = ViewState["ProposalTableGT"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["ProposalTableGT"] = dt;
            grdGT.DataSource = dt;
            grdGT.DataBind();
            SetPreviousDataProposalGT();
        }
        private void DeleteRowDiscount(int rowIndex)
        {
            GetCurrentDataDiscount();
            DataTable dt = ViewState["Discount_Table"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count <= rowIndex)
                {
                    dt.Rows[rowIndex].Delete();
                    ViewState["Discount_Table"] = dt;
                    grdDiscount.DataSource = dt;
                    grdDiscount.DataBind();
                    SetPreviousDataDiscount();
                }
            }
        }
        protected void DeleteRowProposal(int rowIndex)
        {
            GetCurrentDataProposal();
            DataTable dt = ViewState["ProposalTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["ProposalTable"] = dt;
            grdProposal.DataSource = dt;
            grdProposal.DataBind();
            SetPreviousDataProposal();
        }


        protected void lnkCal_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Calculation();
            }
        }

        private static string GetNumbers(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        public void ShowMerge(int FromRowNo, int ToRowNo, string recType)
        {
            if (FromRowNo.ToString() != "" && ToRowNo.ToString() != "")
            {
                double soilTestRate = 0;
                if (recType == "GT")
                {
                    var res = dc.Test_View_ForProposal(-1, 0, "GT", 0, 0, 0).ToList();
                    if (res.Count > 0)
                    {
                        soilTestRate = Convert.ToDouble(res.FirstOrDefault().TEST_Rate_int);
                    }

                    lblGtRate.Text = soilTestRate.ToString();
                    //int FromRowNo = Convert.ToInt32(txtFrmRow.Text);
                    //int ToRowNo = Convert.ToInt32(txtToRow.Text);
                    if (ToRowNo > FromRowNo)
                    {
                        for (int i = 0; i < grdGT.Rows.Count; i++)
                        {
                            TextBox txt_TestMethod1 = (TextBox)grdGT.Rows[i].Cells[3].FindControl("txt_TestMethod");
                            TextBox txt_Unit1 = (TextBox)grdGT.Rows[i].Cells[4].FindControl("txt_Unit");
                            TextBox txt_Quantity1 = (TextBox)grdGT.Rows[i].Cells[5].FindControl("txt_Quantity");
                            TextBox txt_UnitRate1 = (TextBox)grdGT.Rows[i].Cells[6].FindControl("txt_UnitRate");
                            TextBox txt_DiscRate1 = (TextBox)grdGT.Rows[i].Cells[7].FindControl("txt_DiscRate");
                            TextBox txt_Amount1 = (TextBox)grdGT.Rows[i].Cells[8].FindControl("txt_Amount");
                            if (txt_ProposalNo.Text == "Create New..." && (i == 0 || i == 8))
                            {
                                txt_Unit1.Text = "LS";
                            }
                            Boolean foundIt = false;
                            if (i >= FromRowNo && i <= ToRowNo)
                            {
                                for (int j = 0; j < ToRowNo; j++)
                                {
                                    if (j == i)
                                    {
                                        TextBox txt_TestMethod = (TextBox)grdGT.Rows[i - 1].Cells[3].FindControl("txt_TestMethod");
                                        TextBox txt_Unit = (TextBox)grdGT.Rows[i - 1].Cells[4].FindControl("txt_Unit");
                                        TextBox txt_Quantity = (TextBox)grdGT.Rows[i - 1].Cells[5].FindControl("txt_Quantity");
                                        TextBox txt_UnitRate = (TextBox)grdGT.Rows[i - 1].Cells[6].FindControl("txt_UnitRate");
                                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[i - 1].Cells[7].FindControl("txt_DiscRate");
                                        TextBox txt_Amount = (TextBox)grdGT.Rows[i - 1].Cells[8].FindControl("txt_Amount");
                                        txt_TestMethod.BorderStyle = BorderStyle.None;
                                        txt_Unit.BorderStyle = BorderStyle.None;
                                        txt_Quantity.BorderStyle = BorderStyle.None;
                                        txt_UnitRate.BorderStyle = BorderStyle.None;
                                        txt_DiscRate.BorderStyle = BorderStyle.None;
                                        txt_Amount.BorderStyle = BorderStyle.None;
                                        if (txt_ProposalNo.Text == "Create New...")
                                        {
                                            txt_Unit.Text = "Lump sum Up to 10 m Depth";
                                            txt_UnitRate.Text = soilTestRate.ToString();
                                            txtGtRate.Text = soilTestRate.ToString();
                                            double disc = getDiscount("GT");
                                            double discountedRate = Convert.ToDouble(soilTestRate) - (Convert.ToDouble(soilTestRate) * (disc / 100));
                                            if (discountedRate == 0)
                                                txt_DiscRate.Text = soilTestRate.ToString("0.00");
                                            else
                                                txt_DiscRate.Text = discountedRate.ToString("0.00");

                                        }

                                        grdGT.Rows[i].Cells[8].Visible = false;
                                        grdGT.Rows[i].Cells[9].Visible = false;
                                        grdGT.Rows[i].Cells[10].Visible = false;
                                        grdGT.Rows[i].Cells[11].Visible = false;
                                        grdGT.Rows[i].Cells[12].Visible = false;
                                        grdGT.Rows[i].Cells[13].Visible = false;
                                        grdGT.BackColor = System.Drawing.Color.White;
                                        foundIt = true;
                                        break;
                                    }
                                    else
                                    {
                                        txt_TestMethod1.BorderStyle = BorderStyle.NotSet;
                                        txt_DiscRate1.BorderStyle = BorderStyle.NotSet;
                                        txt_UnitRate1.BorderStyle = BorderStyle.NotSet;
                                        txt_Unit1.BorderStyle = BorderStyle.NotSet;
                                        txt_Quantity1.BorderStyle = BorderStyle.NotSet;
                                        txt_Amount1.BorderStyle = BorderStyle.NotSet;
                                        grdGT.Rows[i].Cells[8].Visible = true;
                                        grdGT.Rows[i].Cells[8].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[9].Visible = true;
                                        grdGT.Rows[i].Cells[9].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[10].Visible = true;
                                        grdGT.Rows[i].Cells[10].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[11].Visible = true;
                                        grdGT.Rows[i].Cells[11].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[12].Visible = true;
                                        grdGT.Rows[i].Cells[12].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[13].Visible = true;
                                        grdGT.Rows[i].Cells[13].BorderColor = System.Drawing.Color.White;

                                    }
                                }
                            }
                            if (foundIt == false)
                            {
                                grdGT.Rows[i].Cells[8].Visible = true;
                                grdGT.Rows[i].Cells[9].Visible = true;
                                grdGT.Rows[i].Cells[10].Visible = true;
                                grdGT.Rows[i].Cells[11].Visible = true;
                                grdGT.Rows[i].Cells[12].Visible = true;
                                grdGT.Rows[i].Cells[13].Visible = true;
                                txt_TestMethod1.BorderStyle = BorderStyle.NotSet;
                                txt_DiscRate1.BorderStyle = BorderStyle.NotSet;
                                txt_UnitRate1.BorderStyle = BorderStyle.NotSet;
                                txt_Unit1.BorderStyle = BorderStyle.NotSet;
                                txt_Quantity1.BorderStyle = BorderStyle.NotSet;
                                txt_Amount1.BorderStyle = BorderStyle.NotSet;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < grdGT.Rows.Count; i++)
                        {
                            TextBox txt_TestMethod1 = (TextBox)grdGT.Rows[i].Cells[3].FindControl("txt_TestMethod");
                            TextBox txt_Unit1 = (TextBox)grdGT.Rows[i].Cells[4].FindControl("txt_Unit");
                            TextBox txt_Quantity1 = (TextBox)grdGT.Rows[i].Cells[5].FindControl("txt_Quantity");
                            TextBox txt_UnitRate1 = (TextBox)grdGT.Rows[i].Cells[6].FindControl("txt_UnitRate");
                            TextBox txt_DiscRate1 = (TextBox)grdGT.Rows[i].Cells[7].FindControl("txt_DiscRate");
                            TextBox txt_Amount1 = (TextBox)grdGT.Rows[i].Cells[8].FindControl("txt_Amount");

                            txt_TestMethod1.BorderStyle = BorderStyle.NotSet;
                            txt_DiscRate1.BorderStyle = BorderStyle.NotSet;
                            txt_UnitRate1.BorderStyle = BorderStyle.NotSet;
                            txt_Unit1.BorderStyle = BorderStyle.NotSet;
                            txt_Quantity1.BorderStyle = BorderStyle.NotSet;
                            txt_Amount1.BorderStyle = BorderStyle.NotSet;
                            grdGT.Rows[i].Cells[11].Visible = true;
                            grdGT.Rows[i].Cells[11].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[8].Visible = true;
                            grdGT.Rows[i].Cells[8].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[9].Visible = true;
                            grdGT.Rows[i].Cells[9].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[10].Visible = true;
                            grdGT.Rows[i].Cells[10].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[12].Visible = true;
                            grdGT.Rows[i].Cells[12].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[13].Visible = true;
                            grdGT.Rows[i].Cells[13].BorderColor = System.Drawing.Color.White;
                        }
                    }
                }
                else if (recType == "OTHER")
                {
                    int subTestId = 0;
                    if (ddl_InwardTestType.SelectedValue == "OT")
                    {
                        if (ddlOtherTest.SelectedValue.ToString() != "")
                        {
                            subTestId = Convert.ToInt32(ddlOtherTest.SelectedValue);
                            recType = "OTHER";
                        }
                    }
                    double[] arrTestRate = new double[5];
                    var res = dc.Test_View_ForProposal(-1, 0, recType, 0, 0, subTestId).ToList();
                    if (res.Count > 0)
                    {
                        for (int i = 0; i < res.Count; i++)
                        {
                            arrTestRate[i] = Convert.ToDouble(res[i].TEST_Rate_int);
                        }
                    }
                    double disc = getDiscount("OT");
                    double discountedRate = 0;
                    if (ToRowNo > FromRowNo)
                    {
                        for (int i = 0; i < grdGT.Rows.Count; i++)
                        {
                            TextBox txt_TestMethod1 = (TextBox)grdGT.Rows[i].Cells[3].FindControl("txt_TestMethod");
                            TextBox txt_Unit1 = (TextBox)grdGT.Rows[i].Cells[4].FindControl("txt_Unit");
                            TextBox txt_Quantity1 = (TextBox)grdGT.Rows[i].Cells[5].FindControl("txt_Quantity");
                            TextBox txt_UnitRate1 = (TextBox)grdGT.Rows[i].Cells[6].FindControl("txt_UnitRate");
                            TextBox txt_DiscRate1 = (TextBox)grdGT.Rows[i].Cells[7].FindControl("txt_DiscRate");
                            TextBox txt_Amount1 = (TextBox)grdGT.Rows[i].Cells[8].FindControl("txt_Amount");
                            if (ddlOtherTest.SelectedItem.Text == "SBC by SPT" || ddlOtherTest.SelectedItem.Text == "Water Test for Drinking/Domestic Purpose")
                            {
                                if (i == 0 || i == 3)
                                {
                                    if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                                        txt_Unit1.Text = "LS";
                                }
                                //txt_UnitRate1.Text = arrTestRate[i].ToString();
                                //discountedRate = Convert.ToDouble(arrTestRate[i]) - (Convert.ToDouble(arrTestRate[i]) * (disc / 100));
                                //if (discountedRate == 0)
                                //    txt_DiscRate1.Text = arrTestRate[i].ToString("0.00");
                                //else
                                //    txt_DiscRate1.Text = discountedRate.ToString("0.00");
                            }


                            Boolean foundIt = false;
                            if (i >= FromRowNo && i <= ToRowNo)
                            {
                                for (int j = 0; j < ToRowNo; j++)
                                {
                                    if (j == i)
                                    {
                                        TextBox txt_TestMethod = (TextBox)grdGT.Rows[i - 1].Cells[3].FindControl("txt_TestMethod");
                                        TextBox txt_Unit = (TextBox)grdGT.Rows[i - 1].Cells[4].FindControl("txt_Unit");
                                        TextBox txt_Quantity = (TextBox)grdGT.Rows[i - 1].Cells[5].FindControl("txt_Quantity");
                                        TextBox txt_UnitRate = (TextBox)grdGT.Rows[i - 1].Cells[6].FindControl("txt_UnitRate");
                                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[i - 1].Cells[7].FindControl("txt_DiscRate");
                                        TextBox txt_Amount = (TextBox)grdGT.Rows[i - 1].Cells[8].FindControl("txt_Amount");
                                        //if (ddlOtherTest.SelectedItem.Text != "Water Test for Drinking/Domestic Purpose")
                                        txt_TestMethod.BorderStyle = BorderStyle.None;
                                        txt_Unit.BorderStyle = BorderStyle.None;
                                        txt_Quantity.BorderStyle = BorderStyle.None;
                                        txt_UnitRate.BorderStyle = BorderStyle.None;
                                        txt_DiscRate.BorderStyle = BorderStyle.None;
                                        txt_Amount.BorderStyle = BorderStyle.None;

                                        if (txt_ProposalNo.Text == "Create New...")
                                        {
                                            if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                                            {
                                                txt_Unit.Text = "Nos";
                                            }
                                            txt_UnitRate.Text = arrTestRate[i].ToString();
                                            discountedRate = Convert.ToDouble(arrTestRate[i]) - (Convert.ToDouble(arrTestRate[i]) * (disc / 100));
                                            if (discountedRate == 0)
                                                txt_DiscRate1.Text = arrTestRate[i].ToString("0.00");
                                            else
                                                txt_DiscRate.Text = discountedRate.ToString("0.00");
                                        }


                                        grdGT.Rows[i].Cells[8].Visible = false;
                                        grdGT.Rows[i].Cells[9].Visible = false;
                                        grdGT.Rows[i].Cells[10].Visible = false;
                                        grdGT.Rows[i].Cells[11].Visible = false;
                                        grdGT.Rows[i].Cells[12].Visible = false;
                                        grdGT.Rows[i].Cells[13].Visible = false;
                                        grdGT.BackColor = System.Drawing.Color.White;
                                        foundIt = true;
                                        break;
                                    }
                                    else
                                    {
                                        txt_TestMethod1.BorderStyle = BorderStyle.NotSet;
                                        txt_DiscRate1.BorderStyle = BorderStyle.NotSet;
                                        txt_UnitRate1.BorderStyle = BorderStyle.NotSet;
                                        txt_Unit1.BorderStyle = BorderStyle.NotSet;
                                        txt_Quantity1.BorderStyle = BorderStyle.NotSet;
                                        txt_Amount1.BorderStyle = BorderStyle.NotSet;
                                        grdGT.Rows[i].Cells[11].Visible = true;
                                        grdGT.Rows[i].Cells[11].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[8].Visible = true;
                                        grdGT.Rows[i].Cells[8].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[9].Visible = true;
                                        grdGT.Rows[i].Cells[9].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[10].Visible = true;
                                        grdGT.Rows[i].Cells[10].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[12].Visible = true;
                                        grdGT.Rows[i].Cells[12].BorderColor = System.Drawing.Color.White;
                                        grdGT.Rows[i].Cells[13].Visible = true;
                                        grdGT.Rows[i].Cells[13].BorderColor = System.Drawing.Color.White;
                                    }
                                }
                            }
                            if (foundIt == false)
                            {
                                grdGT.Rows[i].Cells[8].Visible = true;
                                grdGT.Rows[i].Cells[9].Visible = true;
                                grdGT.Rows[i].Cells[10].Visible = true;
                                grdGT.Rows[i].Cells[11].Visible = true;
                                grdGT.Rows[i].Cells[12].Visible = true;
                                grdGT.Rows[i].Cells[13].Visible = true;
                                txt_TestMethod1.BorderStyle = BorderStyle.NotSet;
                                txt_DiscRate1.BorderStyle = BorderStyle.NotSet;
                                txt_UnitRate1.BorderStyle = BorderStyle.NotSet;
                                txt_Unit1.BorderStyle = BorderStyle.NotSet;
                                txt_Quantity1.BorderStyle = BorderStyle.NotSet;
                                txt_Amount1.BorderStyle = BorderStyle.NotSet;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < grdGT.Rows.Count; i++)
                        {
                            TextBox txt_TestMethod1 = (TextBox)grdGT.Rows[i].Cells[3].FindControl("txt_TestMethod");
                            TextBox txt_Unit1 = (TextBox)grdGT.Rows[i].Cells[4].FindControl("txt_Unit");
                            TextBox txt_Quantity1 = (TextBox)grdGT.Rows[i].Cells[5].FindControl("txt_Quantity");
                            TextBox txt_UnitRate1 = (TextBox)grdGT.Rows[i].Cells[6].FindControl("txt_UnitRate");
                            TextBox txt_DiscRate1 = (TextBox)grdGT.Rows[i].Cells[7].FindControl("txt_DiscRate");
                            TextBox txt_Amount1 = (TextBox)grdGT.Rows[i].Cells[8].FindControl("txt_Amount");

                            txt_TestMethod1.BorderStyle = BorderStyle.NotSet;
                            txt_DiscRate1.BorderStyle = BorderStyle.NotSet;
                            txt_UnitRate1.BorderStyle = BorderStyle.NotSet;
                            txt_Unit1.BorderStyle = BorderStyle.NotSet;
                            txt_Quantity1.BorderStyle = BorderStyle.NotSet;
                            txt_Amount1.BorderStyle = BorderStyle.NotSet;
                            grdGT.Rows[i].Cells[11].Visible = true;
                            grdGT.Rows[i].Cells[11].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[8].Visible = true;
                            grdGT.Rows[i].Cells[8].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[9].Visible = true;
                            grdGT.Rows[i].Cells[9].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[10].Visible = true;
                            grdGT.Rows[i].Cells[10].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[12].Visible = true;
                            grdGT.Rows[i].Cells[12].BorderColor = System.Drawing.Color.White;
                            grdGT.Rows[i].Cells[13].Visible = true;
                            grdGT.Rows[i].Cells[13].BorderColor = System.Drawing.Color.White;
                        }
                    }
                }
            }
        }
        protected void Btn_ApplyRate_OnClick(object sender, EventArgs e)
        {

        }
        protected void Btn_Apply_OnClick(object sender, EventArgs e)
        {
            ChkLumShup();
            //ShowMerge();
            if (txtFrmRow.Text != "" && txtToRow.Text != "")
            {
                int FromRowNo = Convert.ToInt32(txtFrmRow.Text);
                int ToRowNo = Convert.ToInt32(txtToRow.Text);

                if (ddl_InwardTestType.SelectedItem.Text == "Soil Investigation")
                    ShowMerge(FromRowNo, ToRowNo, "GT");
                else if (ddlOtherTest.Visible == true)
                {
                    if (ddlOtherTest.SelectedItem.Text.Equals("SBC by SPT"))
                        ShowMerge(FromRowNo, ToRowNo, "OTHER");
                    else if (ddlOtherTest.SelectedItem.Text.Equals("Water Test for Drinking/Domestic Purpose"))
                        ShowMerge(FromRowNo, ToRowNo, "OTHER");
                }
            }

        }
        protected void chk_Lumshup_OnCheckedChanged(object sender, EventArgs e)
        {
            ChkLumShup();
            //ShowMerge();
            if (txtFrmRow.Text != "" && txtToRow.Text != "")
            {
                int FromRowNo = Convert.ToInt32(txtFrmRow.Text);
                int ToRowNo = Convert.ToInt32(txtToRow.Text);

                //if (lblRType.Text == "Soil Investigation")
                if (ddl_InwardTestType.SelectedItem.Text == "Soil Investigation")
                    ShowMerge(FromRowNo, ToRowNo, "GT");
                else if (ddlOtherTest.Visible==true)
                {
                    if (ddlOtherTest.SelectedItem.Text.Equals("SBC by SPT"))
                        ShowMerge(FromRowNo, ToRowNo, "OTHER");
                    else if (ddlOtherTest.SelectedItem.Text.Equals("Water Test for Drinking/Domestic Purpose"))
                        ShowMerge(FromRowNo, ToRowNo, "OTHER");
                }
            }
            else
            {
                int FromRowNo = 0;
                int ToRowNo = 0;
                if (ddl_InwardTestType.SelectedItem.Text == "Soil Investigation")
                    ShowMerge(FromRowNo, ToRowNo, "GT");
                else if (ddlOtherTest.Visible == true
                    )
                {
                      if (ddlOtherTest.SelectedItem.Text.Equals("SBC by SPT"))
                          ShowMerge(FromRowNo, ToRowNo, "OTHER");
                      else if (ddlOtherTest.SelectedItem.Text.Equals("Water Test for Drinking/Domestic Purpose"))
                          ShowMerge(FromRowNo, ToRowNo, "OTHER");
                }
            }


        }

        public void ChkLumShup()
        {
            if (chkLums.Checked == true)
            {
                txtFrmRow.Focus();
                txtFrmRow.Visible = true;
                txtToRow.Visible = true;
                Btn_Apply.Visible = true;
                Label12.Visible = true;
                Label13.Visible = true;
            }
            else
            {
                txtFrmRow.Text = string.Empty;
                txtFrmRow.Visible = false;
                txtToRow.Visible = false;
                txtToRow.Text = string.Empty;
                Btn_Apply.Visible = false;
                Label12.Visible = false;
                Label13.Visible = false;

            }

        }

        protected void LnkBtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData() == true)
            {
                Tab_Notes.CssClass = "Clicked";
                Tab_Discount.CssClass = "Initial";
                Tab_GenericDiscount.CssClass = "Initial";
                MainView_Proposal.ActiveViewIndex = 0;

                DateTime ProposalDt = DateTime.ParseExact(txt_ProposalDt.Text, "dd/MM/yyyy", null);
                string[] arr;
                string appliedDisc = "0", ndtClientScope = "", gtDiscNote = "", Note = string.Empty, NoteGT = string.Empty, PayTermGT = "", splitString = "", splitNo = "0";
                int DiscNotesVisibility_bit = 0, SiteWiseRateApplied_bit = 0, propsalNo = 0, proposalStatus = 0, mergeFrom = 0, mergeTo = 0;
                
                arr = lblDisc.Text.Split('=');
                appliedDisc = arr[1];
                if (appliedDisc != "")
                {
                    if (Convert.ToInt32(appliedDisc) > 0)
                        DiscNotesVisibility_bit = 1;
                }
                //if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                //{
                //    mergeFrom = 2;
                //    mergeTo = 3;
                //}
                //else if (ddlOtherTest.SelectedItem.Text == "Water Test for Drinking/Domestic Purpose")
                //{
                //    mergeFrom = 1;
                //    mergeTo = 3;
                //}
                //else if (ddl_InwardTestType.SelectedValue == "GT")
                //{
                //    mergeFrom = 2;
                //    mergeTo = 8;
                //}
                
                if (txtFrmRow.Text != "")
                    mergeFrom = Convert.ToInt32(txtFrmRow.Text);
                if (txtToRow.Text != "")
                    mergeTo = Convert.ToInt32(txtToRow.Text);

                for (int i = 0; i < Grd_Note.Rows.Count; i++)
                {
                    TextBox txt_NOTE = (TextBox)Grd_Note.Rows[i].FindControl("txt_NOTE");
                    if (txt_NOTE.Text != "")
                    {
                        Note = Note + txt_NOTE.Text + "|";
                    }
                }
                //additional Charges GT
                if (Grd_NoteGT.Visible == true)
                {
                    for (int i = 0; i < Grd_NoteGT.Rows.Count; i++)
                    {
                        TextBox txt_NOTEGT = (TextBox)Grd_NoteGT.Rows[i].FindControl("txt_NOTEGT");
                        if (txt_NOTEGT.Text != "")
                        {
                            NoteGT = NoteGT + txt_NOTEGT.Text + "|";
                        }
                    }
                }
                //payment terms GT
                if (grdPayTermsGT.Visible == true)
                {
                    for (int i = 0; i < grdPayTermsGT.Rows.Count; i++)
                    {
                        TextBox txtNotePayTermGT = (TextBox)grdPayTermsGT.Rows[i].FindControl("txtNotePayTermGT");
                        if (txtNotePayTermGT.Text != "")
                        {
                            PayTermGT = PayTermGT + txtNotePayTermGT.Text + "|";
                        }
                    }
                }
                //Client Scope 
                if (grdClientScope.Visible == true)
                {
                    for (int i = 0; i < grdClientScope.Rows.Count; i++)
                    {
                        TextBox txt_NOTEClientScope = (TextBox)grdClientScope.Rows[i].FindControl("txt_NOTEClientScope");
                        if (txt_NOTEClientScope.Text != "")
                        {
                            ndtClientScope = ndtClientScope + txt_NOTEClientScope.Text + "|";
                        }
                    }
                }
                if (lblRType.Text == "Soil Investigation")
                {
                    if (chkGTDiscNote.Checked == true && chkGTDiscNote.Visible == true)
                        gtDiscNote = lblGTDiscNote.Text;
                }
                clsData clsObj = new clsData();

                if (txt_ProposalNo.Text != "Create New..." && lblModifiedType.Text == "1")//revised
                {
                    //int index = txt_ProposalNo.Text.IndexOf("/R");
                    //if (index > 0)
                    //{
                    //    dc.Proposal_Update(txt_ProposalNo.Text, 0, null, "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 1, proposalStatus, 0, 0, false, "", "", "", "", "", false, "", 0, "", false);
                    //    txt_ProposalNo.Text = txt_ProposalNo.Text.Remove(index);
                    //}
                    //txt_ProposalNo.Text = txt_ProposalNo.Text + "/R" + propsalNo;
                    //proposalStatus = 2;
                    ////dc.ProposalDetail_Update(txt_ProposalNo.Text, null, "", 0, 0, 0, "", true,"");
                    //dc.Proposal_Update_Status(Convert.ToInt32(txt_EnquiryNo.Text), true);

                    int statusOfNewClient = 0;
                    if (lblEnqNewClient.Text == "True")
                        statusOfNewClient = 1;

                    string ProNo = "";
                    DataTable dt = clsObj.getProposalNo(Convert.ToInt32(txt_EnquiryNo.Text), statusOfNewClient);
                    if (dt.Rows.Count > 0)
                    {
                        ProNo = dt.Rows[0][0].ToString();
                    }
                    splitString = ProNo.Split('/').Last();
                    if (splitString.Contains("R"))
                        splitNo = GetNumbers(splitString);
                    propsalNo = Convert.ToInt32(splitNo) + 1;
                    int index = txt_ProposalNo.Text.IndexOf("/R");
                    //  int index = ProNo.IndexOf("/R");
                    if (index > 0)
                    {
                        dc.Proposal_Update(txt_ProposalNo.Text, 0, null, "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 1, proposalStatus, 0, 0, false, "", "", "", "", "", false, "", 0, "", false, 0, 0, 0, "","", false, "", false, "");//it update proposalStatus to 0
                        txt_ProposalNo.Text = txt_ProposalNo.Text.Remove(index);//remove /R from given index from proposal no textbox  
                    }
                    else
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dc.Proposal_Update(dt.Rows[i][0].ToString(), 0, null, "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 1, proposalStatus, 0, 0, false, "", "", "", "", "", false, "", 0, "", false, 0, 0, 0, "","",false, "", false, "");//it update proposalStatus to 0
                            }
                        }
                    }
                    txt_ProposalNo.Text = txt_ProposalNo.Text + "/R" + propsalNo;
                    proposalStatus = 2;
                    dc.Proposal_Update_Status(Convert.ToInt32(txt_EnquiryNo.Text), true, statusOfNewClient);//it update Proposal_ActiveStatus_bit to 1 for all proposal of given enqNo (beacause for new proposal it is defaul 0)


                }
                else if (txt_ProposalNo.Text != "Create New..." && lblModifiedType.Text == "2")//modified
                {
                    dc.Proposal_Update(txt_ProposalNo.Text, 0, null, "", "", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 2, proposalStatus, 0, 0, false, "", "", "", "", "", false, "", 0, "", false, 0, 0, 0, "","",false, "", false,"");
                    dc.ProposalDetail_Update(txt_ProposalNo.Text, null, "", "", 0, "", 0, "0", 0, true, "", 0, "");
                    proposalStatus = 1;
                }
                else
                {
                    Int32 NewrecNo = 0;
                    proposalStatus = 1;
                    NewrecNo = clsObj.GetnUpdateRecordNo("ProposalNo");
                    //txt_ProposalNo.Text = "Duro/Pro/" + NewrecNo + "/" + Convert.ToDateTime(txt_ProposalDt.Text).Year;
                    txt_ProposalNo.Text = "Duro/Pro/" + NewrecNo + "/" + DateTime.ParseExact(txt_ProposalDt.Text, "dd/MM/yyyy", null).Year;
                }
                
                if (grdProposal.Visible == true)
                {
                    for (int j = 0; j < grdProposal.Rows.Count; j++)
                    {
                        TextBox txt_Particular = (TextBox)grdProposal.Rows[j].Cells[3].FindControl("txt_Particular");
                        TextBox txt_TestMethod = (TextBox)grdProposal.Rows[j].Cells[4].FindControl("txt_TestMethod");
                        TextBox txt_Rate = (TextBox)grdProposal.Rows[j].Cells[5].FindControl("txt_Rate");
                        TextBox txt_Discount = (TextBox)grdProposal.Rows[j].Cells[6].FindControl("txt_Discount");
                        TextBox txt_DiscRate = (TextBox)grdProposal.Rows[j].Cells[7].FindControl("txt_DiscRate");
                        TextBox txt_Quantity = (TextBox)grdProposal.Rows[j].Cells[8].FindControl("txt_Quantity");
                        TextBox txt_Amount = (TextBox)grdProposal.Rows[j].Cells[9].FindControl("txt_Amount");
                        Label lbl_InwdType = (Label)grdProposal.Rows[j].Cells[10].FindControl("lbl_InwdType");
                        Label lbl_SiteWiseRate = (Label)grdProposal.Rows[j].Cells[11].FindControl("lbl_SiteWiseRate");
                        Label lbl_TestId = (Label)grdProposal.Rows[j].Cells[12].FindControl("lbl_TestId");
                        if (txt_TestMethod.Text == "" || txt_TestMethod.Text == "---")
                            txt_TestMethod.Text = "NA";
                        if (lbl_SiteWiseRate.Text != "0")
                            SiteWiseRateApplied_bit = 1;

                        dc.ProposalDetail_Update(txt_ProposalNo.Text, ProposalDt, txt_Particular.Text, txt_TestMethod.Text, Convert.ToDecimal(txt_Rate.Text), txt_Discount.Text, Convert.ToDecimal(txt_DiscRate.Text), txt_Quantity.Text, Convert.ToDecimal(txt_Amount.Text), false, lbl_InwdType.Text, Convert.ToInt32(lbl_TestId.Text), "");

                        dc.SiteWiseRate_Update_FromProposal(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblSiteId.Text), Convert.ToInt32(lbl_TestId.Text), Convert.ToDecimal(txt_DiscRate.Text)); 
                    }
                }
                else if (grdGT.Visible == true)
                {
                    for (int j = 0; j < grdGT.Rows.Count; j++)
                    {
                        TextBox txt_Particular = (TextBox)grdGT.Rows[j].Cells[2].FindControl("txt_Particular");
                        TextBox txt_TestMethod = (TextBox)grdGT.Rows[j].Cells[3].FindControl("txt_TestMethod");
                        TextBox txt_Unit = (TextBox)grdGT.Rows[j].Cells[4].FindControl("txt_Unit");
                        TextBox txt_Quantity = (TextBox)grdGT.Rows[j].Cells[5].FindControl("txt_Quantity");
                        TextBox txt_UnitRate = (TextBox)grdGT.Rows[j].Cells[6].FindControl("txt_UnitRate");
                        TextBox txt_DiscRate = (TextBox)grdGT.Rows[j].Cells[7].FindControl("txt_DiscRate");
                        TextBox txt_Amount = (TextBox)grdGT.Rows[j].Cells[8].FindControl("txt_Amount");
                        Label lbl_InwdType = (Label)grdGT.Rows[j].Cells[9].FindControl("lbl_InwdType");

                        if (grdGT.Rows[j].Cells[8].Visible == false)
                            txt_TestMethod.Text = "";
                        if (grdGT.Rows[j].Cells[9].Visible == false)
                            txt_Unit.Text = "";
                        if (grdGT.Rows[j].Cells[10].Visible == false)
                            txt_Quantity.Text = "";
                        if (grdGT.Rows[j].Cells[11].Visible == false)
                            txt_UnitRate.Text = "";
                        if (grdGT.Rows[j].Cells[12].Visible == false)
                            txt_DiscRate.Text = "";
                        if (grdGT.Rows[j].Cells[13].Visible == false)
                            txt_Amount.Text = "";

                        if (txt_TestMethod.Text == "" || txt_TestMethod.Text == "---")
                            txt_TestMethod.Text = "NA";
                        if (txt_UnitRate.Text == "")
                        {
                            txt_UnitRate.Text = "0";
                        }
                        if (txt_DiscRate.Text == "")
                        {
                            txt_DiscRate.Text = "0";
                        }
                        if (txt_Amount.Text == "")
                        {
                            txt_Amount.Text = "0";
                        }


                        dc.ProposalDetail_Update(txt_ProposalNo.Text, ProposalDt, txt_Particular.Text, txt_TestMethod.Text, Convert.ToDecimal(txt_UnitRate.Text), "0", Convert.ToDecimal(txt_DiscRate.Text), txt_Quantity.Text.ToString(), Convert.ToDecimal(txt_Amount.Text), false, lbl_InwdType.Text, 0, txt_Unit.Text);

                        //dc.SiteWiseRate_Update_FromProposal(Convert.ToInt32(lblClientId.Text), Convert.ToInt32(lblSiteId.Text), Convert.ToInt32(lbl_TestId.Text), Convert.ToDecimal(txt_DiscRate.Text)); 
                    }
                }
                int subTestId = 0;
                if (ddlOtherTest.SelectedIndex != -1 && ddlOtherTest.Visible == true)
                    subTestId = Convert.ToInt32(ddlOtherTest.SelectedValue.ToString());

                string strDiscDetails = "", email = "";
                strDiscDetails += lblVol.Text + "~" + lblTime.Text + "~" + lblAdv.Text + "~" + lblLoy.Text + "~" + lblProp.Text + "~" + lblApp.Text + "~" + lblCalcDisc.Text + "~" + lblMax.Text + "~" + txtIntro.Text + "~" + lblDisc.Text;

                if (lblClientId.Text == "0" && lblEnqNewClient.Text == "True")
                {
                    email = txtEmail.Text;
                    dc.Client_Update_Address(Convert.ToInt32(txt_EnquiryNo.Text), txtSiteName.Text, txtProContactNo.Text, 0, 1);//here we update site name of client which is not in system
                }
                if (lblEnqNewClient.Text == "False") //update contact no of existing client
                {
                    dc.Client_Update_Contact(Convert.ToInt32(txt_EnquiryNo.Text), txtProContactNo.Text);//here we update contact no of client which is  in system

                }
                if (ddlOtherTest.Visible == true && ddl_InwardTestType.SelectedValue == "OT")
                {
                    if (ddlOtherTest.SelectedItem.Text == "Earth Resistivity Test" || ddlOtherTest.SelectedItem.Text == "Plate Load Testing" || ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                        txt_Subject.Text = "Commercial offer for Soil Testing requirement for your site " + txtSiteName.Text + ".";
                }
                string structAudDetails = "";
                if (ddl_InwardTestType.SelectedValue == "OT" && ddlOtherTest.SelectedItem.Text == "Structural Audit")
                {
                    structAudDetails = txtStructNameOfApartSoc.Text + "~" + txtStructAddress.Text + "~" + txtStructBuiltupArea.Text + "~" +
                        txtStructNoOfBuild.Text + "~" + txtStructAge.Text + "~" + ddlStructConstWithin5Y.SelectedValue + "~" + ddlStructLocation.SelectedValue + "~" +
                        ddlStructAddLoadExpc.SelectedValue + "~" + ddlStructDistressObs.SelectedValue;
                }
                if (grdPayTermsGT.Visible == false)
                {
                    PayTermGT = txtPaymentTerm.Text.Trim();
                }
                dc.Proposal_Update(txt_ProposalNo.Text, Convert.ToInt32(txt_EnquiryNo.Text), ProposalDt, txt_KindAttention.Text, txt_Subject.Text, txt_Description.Text, txt_ContactNo.Text, 0, 0, 0, 0, 0, Convert.ToDecimal(txt_NetAmount.Text), Convert.ToInt32(ddl_PrposalBy.SelectedValue), Convert.ToInt32(Session["LoginID"]), 0, proposalStatus, mergeFrom, mergeTo, chkQty.Checked, txtMe.Text, txtMeNum.Text, Note, NoteGT, strDiscDetails, Convert.ToBoolean(lblEnqNewClient.Text), gtDiscNote, subTestId, email, Convert.ToBoolean(DiscNotesVisibility_bit), Convert.ToDecimal(18), Convert.ToDecimal(lblGstAmt.Text), Convert.ToDecimal(lblGrandTotal.Text), ndtClientScope, txtEmail.Text, Convert.ToBoolean(SiteWiseRateApplied_bit), structAudDetails, chkMOUWorkOrder.Checked, PayTermGT);
                dc.Proposal_Update_ApprovalPendingStatus(txt_ProposalNo.Text, false);
                
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                lblMsg.Text = "Record Saved Successfully";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Green;
                LnkBtnSave.Enabled = false;
                lnkSendForApproval.Enabled = false;
                lnkPrint.Visible = true;
            }
        }
        protected void grdProposal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string particular = "";
            GridViewRow grdrow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int rowindex = grdrow.RowIndex;
            //LinkButton lnkDiscount = (LinkButton)grdProposal.Rows[rowindex].FindControl("lnkDiscount");
            TextBox txt_Discount = (TextBox)grdProposal.Rows[rowindex].FindControl("txt_Discount");
            TextBox txtParticular = (TextBox)grdProposal.Rows[rowindex].FindControl("txt_Particular");
            particular = txtParticular.Text;
            if (e.CommandName == "discount" && txt_Discount.Text != "0")
            {

                //   Tab_Discount_Click(sender, (EventArgs)e);

                Tab_Discount.CssClass = "Clicked";
                Tab_Notes.CssClass = "Initial";
                MainView_Proposal.ActiveViewIndex = 1;



            }
        }
        protected void Lnk_AddMainGrd_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false; bool validFlag = false;
            string inwdType = lblRType.Text;//this is enquiry material rec type
            clsData obj = new clsData();
            string inwardTypeValue = obj.getInwardTypeValue(inwdType);
            string ddlInwrdType = ddl_InwardTestType.SelectedItem.Text;//this is random type which is select by us to add new test of other rec type
            string ddlInwrdTypeValue = ddl_InwardTestType.SelectedValue.ToString();

            //for terms and conditions variation
            if (inwardTypeValue == "GT")
            {
                if (ddlInwrdTypeValue == "GT")
                    validFlag = true;

            }
            else if (inwardTypeValue == "RWH")
            {
                if (ddlInwrdTypeValue == "RWH")
                    validFlag = true;
            }
            //else if (inwardTypeValue == "ST")
            //{
            //    if (ddlInwrdTypeValue == "ST")
            //        validFlag = true;
            //}
            else if (inwardTypeValue == "NDT" || inwardTypeValue == "CORECUT" || inwardTypeValue == "CR")
            {
                if (ddlInwrdTypeValue == "NDT" || ddlInwrdTypeValue == "CORECUT" || ddlInwrdTypeValue == "CR")
                    validFlag = true;
            }
            else if (inwardTypeValue == "AAC" || inwardTypeValue == "AGGT" || inwardTypeValue == "BT-" || inwardTypeValue == "CCH" || inwardTypeValue == "CEMT" || inwardTypeValue == "CT" || inwardTypeValue == "FLYASH" || inwardTypeValue == "SOLID" || inwardTypeValue == "MF" || inwardTypeValue == "PT" || inwardTypeValue == "PILE" || inwardTypeValue == "SO" || inwardTypeValue == "STC" || inwardTypeValue == "TILE" || inwardTypeValue == "WT" || inwardTypeValue == "ST")
            {
                if (ddlInwrdTypeValue == "AAC" || ddlInwrdTypeValue == "Coupon" || ddlInwrdTypeValue == "AGGT" || ddlInwrdTypeValue == "BT-" || ddlInwrdTypeValue == "CCH" || ddlInwrdTypeValue == "CEMT" || ddlInwrdTypeValue == "CT" || ddlInwrdTypeValue == "FLYASH" || ddlInwrdTypeValue == "SOLID" || ddlInwrdTypeValue == "MF" || ddlInwrdTypeValue == "PT" || ddlInwrdTypeValue == "PILE" || ddlInwrdTypeValue == "SO" || ddlInwrdTypeValue == "STC" || ddlInwrdTypeValue == "TILE" || ddlInwrdTypeValue == "WT" || ddlInwrdTypeValue == "ST")
                    validFlag = true;
            }
            else if (inwardTypeValue == "OT")
            {
                if (ddlInwrdTypeValue == "OT" && ddlOtherTest.Visible == true)
                {
                    string otherInwrdTest = ddlOtherTest.SelectedItem.Text;
                    DataTable dt = obj.getOtherSubTest();
                    if (ddlOtherTest.SelectedItem.Text == "Structural Audit")
                    {
                        grdProposal.DataSource = null;
                        grdProposal.DataBind();
                        validFlag = true; otFlag = true;
                        pnlStructAudit.Visible = true;
                        DisplayStructuralAuditDetails();
                    }
                    else if (ddlOtherTest.SelectedItem.Text == "Plate Load Testing")
                    {
                        grdProposal.DataSource = null;
                        grdProposal.DataBind();
                        validFlag = true; otFlag = true;
                    }
                    else if (ddlOtherTest.SelectedItem.Text == "Earth Resistivity Test")
                    {
                        grdProposal.DataSource = null;
                        grdProposal.DataBind();
                        validFlag = true; otFlag = true;
                    }
                    else if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                    {
                        grdGT.DataSource = null;
                        grdGT.DataBind();
                        validFlag = true; otFlag = true;

                        chkLums.Checked = true;
                        ChkLumShup();
                        txtFrmRow.Text = 2.ToString(); 
                        txtToRow.Text = 3.ToString();
                    }
                    else
                    {
                        if (otFlag == true)
                        {
                            otFlag = false;
                            grdProposal.DataSource = null;
                            grdProposal.DataBind();
                        }
                        foreach (DataRow item in dt.Rows)
                        {
                            if (otherInwrdTest == item["Test_name_var"].ToString())
                                validFlag = true;
                        }

                    }
                    if (ddlOtherTest.SelectedItem.Text == "Water Test for Drinking/Domestic Purpose")
                    {
                        chkLums.Checked = true;
                        ChkLumShup();
                        txtFrmRow.Text = 1.ToString();
                        txtToRow.Text = 3.ToString();
                    }
                }
            }

            if (validFlag == false)
            {
                string val = "---Select---";
                if (grdProposal.Visible == true)
                {
                    if (grdProposal.Rows.Count > 0)
                    {
                        Label lbl_InwdType = (Label)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[10].FindControl("lbl_InwdType");
                        val = lbl_InwdType.Text;
                        ddl_InwardTestType.SelectedValue = ddl_InwardTestType.Items.FindByValue(val).Value;
                    }
                }
                else if (inwardTypeValue == "GT")
                    ddl_InwardTestType.SelectedValue = ddl_InwardTestType.Items.FindByValue("GT").Value;
                else if (inwardTypeValue == "RWH")
                    ddl_InwardTestType.SelectedValue = ddl_InwardTestType.Items.FindByValue("RWH").Value;
                else if (inwardTypeValue == "ST")
                    ddl_InwardTestType.SelectedValue = ddl_InwardTestType.Items.FindByValue("ST").Value;
                //else if (inwardTypeValue == "OT")
                //{
                //    ddl_InwardTestType.SelectedValue = ddl_InwardTestType.Items.FindByValue("OT").Value;
                //    ddlOtherTest.SelectedValue = ddl_InwardTestType.Items.FindByValue(val).Value;
                //}
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Terms and conditions are different as per Inward Type.. So you can not add selected Inward Type Test" + "');", true);
                return;

            }

            if (inwdType != ddlInwrdType)
                inwdType = ddlInwrdType;

            //
            if (ddl_InwardTestType.SelectedValue == "OT" && ddlOtherTest.SelectedValue == "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Select Sub test" + "');", true);
                ddlOtherTest.Focus();
            }
            else
            {
                chkGTDiscNote.Visible = false;
                lblGTDiscNote.Visible = false;
                lblDepth.Visible = false;
                if (grdGT.Visible == false)
                {
                    grdGT.DataSource = null; grdGT.DataBind();
                }
                if (grdProposal.Visible == false)
                {
                    grdProposal.DataSource = null; grdProposal.DataBind();
                }

                if (inwdType == "Soil Investigation" && ddl_InwardTestType.SelectedValue == "GT")
                {
                    chkGTDiscNote.Visible = true;
                    lblGTDiscNote.Visible = true;
                    lnkDepth.Visible = true;
                    showGTFields();
                    Grd_Note.DataSource = null;
                    Grd_Note.DataBind();
                    Grd_NoteGT.DataSource = null;
                    Grd_NoteGT.DataBind();
                    grdPayTermsGT.DataSource = null;
                    grdPayTermsGT.DataBind();
                    addTermsConditionNotes("Soil Investigation");
                    addTestToGridGT("GT");
                }
                else if (ddl_InwardTestType.SelectedValue == "OT" && (ddlOtherTest.SelectedItem.Text == "SBC by SPT" || ddlOtherTest.SelectedItem.Text == "Water Test for Drinking/Domestic Purpose"))
                {
                    showGTFields();
                    if (ddlOtherTest.SelectedItem.Text.Equals("Water Test for Drinking/Domestic Purpose"))
                    {
                        Grd_NoteGT.Visible = false;
                        lblAddChargesGT.Visible = false;

                        grdPayTermsGT.Visible = false;
                        lblPayTermsGT.Visible = false;
                    }
                    ChkLumShup();
                    grdProposal.Visible = false;
                    if (ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                    {
                        ddlInwrdType = "SBC by SPT";
                        Grd_Note.DataSource = null;
                        Grd_Note.DataBind();
                        Grd_NoteGT.DataSource = null;
                        Grd_NoteGT.DataBind();
                        grdPayTermsGT.DataSource = null;
                        grdPayTermsGT.DataBind();
                    }
                    addTermsConditionNotes(ddlInwrdType);
                    addTestToGridGT(ddlOtherTest.SelectedItem.Text);

                }
                //  else if (lblRType.Text != "Soil Investigation" && ddl_InwardTestType.SelectedValue != "GT")
                else if (inwdType != "Soil Investigation" && ddl_InwardTestType.SelectedValue != "GT")
                {
                    hideGTFields();
                    ChkLumShup();
                    Grd_Note.DataSource = null;
                    Grd_Note.DataBind();
                    Grd_NoteGT.DataSource = null;
                    Grd_NoteGT.DataBind();
                    grdPayTermsGT.DataSource = null;
                    grdPayTermsGT.DataBind();
                    ddlInwrdType = ddl_InwardTestType.SelectedItem.Text;
                    if (ddl_InwardTestType.SelectedValue == "RWH")
                        ddlInwrdType = "Rain Water Harvesting";
                    else if (ddlOtherTest.SelectedItem.Text == "Plate Load Testing")
                    {
                        ddlInwrdType = "Plate Load Testing";
                    }
                    else if (ddlOtherTest.SelectedItem.Text == "Earth Resistivity Test")
                    {
                        ddlInwrdType = "Earth Resistivity Test";
                    }
                    addTermsConditionNotes(ddlInwrdType);
                    bool addTest = true;
                    if (ddl_InwardTestType.SelectedValue == "OT" && ddlOtherTest.SelectedItem.Text  == "Structural Audit")
                        addTest = false;
                    if (addTest == true)
                        addTestToGrid(ddl_InwardTestType.SelectedValue.ToString());

                }

                DisplayGenericDiscountDetails();
            }

        }
        private void DisplayStructuralAuditDetails()
        {
            int age = 0, area = 0, category1 = 0, category2 = 0;
            if (txtStructAge.Text != "")
            {
                age = Convert.ToInt32(txtStructAge.Text); 
            }
            if (txtStructBuiltupArea.Text != "")
            {
                area = Convert.ToInt32(txtStructBuiltupArea.Text);
            }
            
            if (age < 15)
                category1 = 1;
            else if (age >= 15 && age <= 25)
                category1 = 2;
            else if (age > 25)
                category1 = 3;

            if (ddlStructLocation.SelectedValue == "Noncoastal")
                category1 = 2;
            if (ddlStructAddLoadExpc.SelectedValue == "Yes")
                category1 = 3;
            if (ddlStructDistressObs.SelectedValue == "Yes")
                category1 = 3;
            //if (ddlStructAddLoadExpc.SelectedValue == "Yes")
            //    category1 = 3;
            if (area < 50000)
                category2 = 1;
            else
                category2 = 2;

            for (int i = 0; i <= 4; i++)
            {
                AddRowProposal();
                TextBox txt_Particular = (TextBox)grdProposal.Rows[i].FindControl("txt_Particular");
                TextBox txt_TestMethod = (TextBox)grdProposal.Rows[i].FindControl("txt_TestMethod");
                TextBox txt_Rate = (TextBox)grdProposal.Rows[i].FindControl("txt_Rate");
                TextBox txt_Quantity = (TextBox)grdProposal.Rows[i].FindControl("txt_Quantity");                
                TextBox txt_Discount = (TextBox)grdProposal.Rows[i].FindControl("txt_Discount");
                TextBox txt_DisRate = (TextBox)grdProposal.Rows[i].FindControl("txt_DiscRate");
                TextBox txt_Amount = (TextBox)grdProposal.Rows[i].FindControl("txt_Amount");
                Label lbl_TestId = (Label)grdProposal.Rows[i].FindControl("lbl_TestId");

                if (category1 == 1 && category2 == 1)
                {
                    if (i == 0)
                    {
                        txt_Particular.Text = "UPV";
                        txt_TestMethod.Text = "10";
                        txt_Rate.Text = "500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 1)
                    {
                        txt_Particular.Text = "Hammer";
                        txt_TestMethod.Text = "10";
                        txt_Rate.Text = "300";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "300";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 2)
                    {
                        txt_Particular.Text = "Core";
                        txt_TestMethod.Text = "3";
                        txt_Rate.Text = "5000";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "5000";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 3)
                    {
                        txt_Particular.Text = "Carbonation";
                        txt_TestMethod.Text = "3";
                        txt_Rate.Text = "1500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "1500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 4)
                    {
                        txt_Particular.Text = "Halfcell";
                        txt_TestMethod.Text = "3";
                        txt_Rate.Text = "2500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "2500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                }
                else if (category1 == 1 && category2 == 2)
                {
                    if (i == 0)
                    {
                        txt_Particular.Text = "UPV";
                        txt_TestMethod.Text = "30";
                        txt_Rate.Text = "500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 1)
                    {
                        txt_Particular.Text = "Hammer";
                        txt_TestMethod.Text = "30";
                        txt_Rate.Text = "300";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "300";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 2)
                    {
                        txt_Particular.Text = "Core";
                        txt_TestMethod.Text = "9";
                        txt_Rate.Text = "5000";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "5000";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 3)
                    {
                        txt_Particular.Text = "Carbonation";
                        txt_TestMethod.Text = "9";
                        txt_Rate.Text = "1500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "1500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 4)
                    {
                        txt_Particular.Text = "Halfcell";
                        txt_TestMethod.Text = "9";
                        txt_Rate.Text = "2500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "2500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }                
                }
                else if (category1 == 2 && category2 == 1)
                {
                    if (i == 0)
                    {
                        txt_Particular.Text = "UPV";
                        txt_TestMethod.Text = "15";
                        txt_Rate.Text = "500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 1)
                    {
                        txt_Particular.Text = "Hammer";
                        txt_TestMethod.Text = "15";
                        txt_Rate.Text = "300";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "300";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 2)
                    {
                        txt_Particular.Text = "Core";
                        txt_TestMethod.Text = "6";
                        txt_Rate.Text = "5000";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "5000";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 3)
                    {
                        txt_Particular.Text = "Carbonation";
                        txt_TestMethod.Text = "6";
                        txt_Rate.Text = "1500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "1500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 4)
                    {
                        txt_Particular.Text = "Halfcell";
                        txt_TestMethod.Text = "6";
                        txt_Rate.Text = "2500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "2500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                }
                else if (category1 == 2 && category2 == 2)
                {
                    if (i == 0)
                    {
                        txt_Particular.Text = "UPV";
                        txt_TestMethod.Text = "35";
                        txt_Rate.Text = "500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 1)
                    {
                        txt_Particular.Text = "Hammer";
                        txt_TestMethod.Text = "35";
                        txt_Rate.Text = "300";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "300";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 2)
                    {
                        txt_Particular.Text = "Core";
                        txt_TestMethod.Text = "12";
                        txt_Rate.Text = "5000";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "5000";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 3)
                    {
                        txt_Particular.Text = "Carbonation";
                        txt_TestMethod.Text = "12";
                        txt_Rate.Text = "1500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "1500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 4)
                    {
                        txt_Particular.Text = "Halfcell";
                        txt_TestMethod.Text = "12";
                        txt_Rate.Text = "2500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "2500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                }
                else if (category1 == 3 && category2 == 1)
                {
                    if (i == 0)
                    {
                        txt_Particular.Text = "UPV";
                        txt_TestMethod.Text = "20";
                        txt_Rate.Text = "500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 1)
                    {
                        txt_Particular.Text = "Hammer";
                        txt_TestMethod.Text = "20";
                        txt_Rate.Text = "300";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "300";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 2)
                    {
                        txt_Particular.Text = "Core";
                        txt_TestMethod.Text = "9";
                        txt_Rate.Text = "5000";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "5000";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 3)
                    {
                        txt_Particular.Text = "Carbonation";
                        txt_TestMethod.Text = "9";
                        txt_Rate.Text = "1500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "1500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 4)
                    {
                        txt_Particular.Text = "Halfcell";
                        txt_TestMethod.Text = "9";
                        txt_Rate.Text = "2500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "2500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                }
                else if (category1 == 3 && category2 == 2)
                {
                    if (i == 0)
                    {
                        txt_Particular.Text = "UPV";
                        txt_TestMethod.Text = "40";
                        txt_Rate.Text = "500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 1)
                    {
                        txt_Particular.Text = "Hammer";
                        txt_TestMethod.Text = "40";
                        txt_Rate.Text = "300";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "300";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 2).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 2)
                    {
                        txt_Particular.Text = "Core";
                        txt_TestMethod.Text = "15";
                        txt_Rate.Text = "5000";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "5000";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 3)
                    {
                        txt_Particular.Text = "Carbonation";
                        txt_TestMethod.Text = "15";
                        txt_Rate.Text = "1500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "1500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                    else if (i == 4)
                    {
                        txt_Particular.Text = "Halfcell";
                        txt_TestMethod.Text = "15";
                        txt_Rate.Text = "2500";
                        txt_Discount.Text = "0";
                        txt_DisRate.Text = "2500";
                        txt_Quantity.Text = (Convert.ToInt32(txt_TestMethod.Text) * 1).ToString();
                        lbl_TestId.Text = "0";
                        txt_Amount.Text = (Convert.ToInt32(txt_DisRate.Text) * Convert.ToInt32(txt_Quantity.Text)).ToString();
                    }
                }
            }
        }
        private void addTestToGrid(string testType)
        {
            int testId; double siteRate = 0, disc = 0, calDisc = 0, discountedRate = 0;
            int MaterialId = -1, subTestId = 0;
            string subTestOt = "";
            if (ddl_InwardTestType.SelectedValue == "OT")
            {

                if (ddlOtherTest.SelectedValue.ToString() != "")
                {
                    subTestId = Convert.ToInt32(ddlOtherTest.SelectedValue);
                    testType = "Other";
                    subTestOt = ddlOtherTest.SelectedItem.Text.ToString();
                }
            }

            if (grdProposal.Visible == true)
            {

                var res = dc.Test_View_ForProposal(MaterialId, 0, testType, 0, 0, subTestId);
                int i = 0;
                string testName = "", str = "";
                foreach (var re in res)
                {
                    bool flag = false;

                    for (int j = 0; j < grdProposal.Rows.Count; j++)//check if datagrid have same test  
                    {
                        TextBox txtParticular = (TextBox)grdProposal.Rows[j].Cells[3].FindControl("txt_Particular");

                        if (re.Test_RecType_var == "MF")
                        {
                            int frmNum = Convert.ToInt32(re.TEST_From_num);
                            int toNum = Convert.ToInt32(re.TEST_To_num);

                            if (toNum == 30)
                                str = "Upto M30";
                            else if (frmNum == 31)
                                str = "Above M30";
                            else
                                str = "";

                            testName = re.TEST_Name_var.ToString() + " " + str;
                        }
                        else
                            testName = re.TEST_Name_var.ToString();

                        if (testType == "CEMT" && testName == "Compressive Strength")
                        {
                            testName = "Compressive Strength (3, 7, 28)";
                        }
                        if (testType == "FLYASH" && testName == "Compressive Strength")
                        {
                            testName = "Compressive Strength (7, 28, 90)";
                        }
                        if (txtParticular.Text == testName)//&& txtRate.Text == Convert.ToDecimal(re.TEST_Rate_int).ToString("0.00"))
                        {

                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        testId = Convert.ToInt32(re.TEST_Id);
                        siteRate = chkSiteWiseRate(testId);
                        AddRowProposal();

                        TextBox txt_Particular = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[3].FindControl("txt_Particular");
                        TextBox txt_TestMethod = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[4].FindControl("txt_TestMethod");
                        TextBox txt_Rate = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[5].FindControl("txt_Rate");
                        TextBox txt_Discount = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[6].FindControl("txt_Discount");
                        TextBox txt_DiscRate = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[7].FindControl("txt_DiscRate");
                        TextBox txt_Quantity = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[8].FindControl("txt_Quantity");
                        TextBox txt_Amount = (TextBox)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[9].FindControl("txt_Amount");
                        Label lbl_InwdType = (Label)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[10].FindControl("lbl_InwdType");
                        Label lbl_SiteWiseRate = (Label)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[11].FindControl("lbl_SiteWiseRate");
                        Label lbl_TestId = (Label)grdProposal.Rows[grdProposal.Rows.Count - 1].Cells[12].FindControl("lbl_TestId");

                        if (testType == "ST")
                            txt_Particular.Text = re.TEST_Name_var.ToString() + "(" + re.TEST_From_num.ToString() + "mm - " + re.TEST_To_num.ToString() + "mm)";
                        else if (testType == "MF")
                        {
                            int frmNum = Convert.ToInt32(re.TEST_From_num);
                            int toNum = Convert.ToInt32(re.TEST_To_num);
                            if (toNum == 30)
                                str = "Upto M30";
                            else if (frmNum == 31)
                                str = "Above M30";
                            else
                                str = "";

                            txt_Particular.Text = re.TEST_Name_var.ToString() + " " + str;
                        }
                        else
                            txt_Particular.Text = re.TEST_Name_var.ToString();

                        if (Convert.ToString(re.TEST_Method_var) != "" && Convert.ToString(re.TEST_Method_var) != null)
                            txt_TestMethod.Text = re.TEST_Method_var.ToString();
                        else
                            txt_TestMethod.Text = "NA";

                        txt_Rate.Text = re.TEST_Rate_int.ToString();

                        if (testType == "Other")
                            lbl_InwdType.Text = "OTHER";
                        else
                            lbl_InwdType.Text = ddl_InwardTestType.SelectedItem.Value;

                        //txt_Quantity.Text = "1";

                        lbl_TestId.Text = re.TEST_Id.ToString();
                        if (siteRate != 0)//apply sitewise rate setting
                        {
                            txt_DiscRate.Text = siteRate.ToString("0.00");
                            //calculate rev. discount
                            calDisc = Math.Round(((Convert.ToDouble(re.TEST_Rate_int) - (Convert.ToDouble(siteRate))) * 100) / Convert.ToDouble(re.TEST_Rate_int));
                            //txt_Discount.Text = calDisc.ToString();
                            txt_Discount.Text = "-";
                            lbl_SiteWiseRate.Text = siteRate.ToString();
                        }
                        else //apply discount setting
                        {
                            if (lbl_InwdType.Text == "OTHER")
                                disc = getDiscount("OT");
                            else
                                disc = getDiscount(lbl_InwdType.Text);

                            txt_Discount.Text = disc.ToString();
                            discountedRate = Convert.ToDouble(re.TEST_Rate_int) - (Convert.ToDouble(re.TEST_Rate_int) * (disc / 100));
                            txt_DiscRate.Text = discountedRate.ToString("0.00");
                            lbl_SiteWiseRate.Text = "0";
                        }
                        //if (testType == "CEMT" && txt_Particular.Text == "Compressive Strength")
                        //{
                        //    txt_Particular.Text = "Compressive Strength (3, 7, 28)";
                        //    txt_Rate.Text = (re.TEST_Rate_int * 3).ToString();
                        //    txt_DiscRate.Text = (Convert.ToDecimal(txt_DiscRate.Text) * 3).ToString();
                        //}
                        //if (testType == "FLYASH" && txt_Particular.Text == "Compressive Strength")
                        //{
                        //    txt_Particular.Text = "Compressive Strength (7, 28, 90)";
                        //    txt_Rate.Text = (re.TEST_Rate_int * 3).ToString();
                        //    txt_DiscRate.Text = (Convert.ToDecimal(txt_DiscRate.Text) * 3).ToString();
                        //}
                        i++;
                    }
                }

            }
        }
        private double chkSiteWiseRate(int testId)
        {
            double siteWiseRate = 0;
            int clId = 0, siteId = 0;
            if (lblEnqNewClient.Text == "False")
            {
                var details = dc.Enquiry_View(Convert.ToInt32(txt_EnquiryNo.Text), 1, 0);
                foreach (var ds in details)
                {
                    clId = Convert.ToInt32(ds.CL_Id);
                    siteId = Convert.ToInt32(ds.SITE_Id);
                    break;
                }

                //check whether sitewise rate is there or not
                var chk = dc.SiteWiseRate_View(clId, siteId, testId, true).ToList();
                if (chk.Count() > 0)//sitewise rate is applicable
                    siteWiseRate = Convert.ToDouble(chk.FirstOrDefault().SITERATE_Test_Rate_int);

            }
            return siteWiseRate;
        }
        public int getMaxDiscount(string matrialType)
        {
            int maxDisc = 0;
            var maxDiscDetails = dc.Material_View(matrialType, "");
            foreach (var max in maxDiscDetails)
            {
                maxDisc = Convert.ToInt32(max.MATERIAL_MaxDiscount_int);
                break;
            }
            return maxDisc;

        }
        public double getDiscount(string recordType)
        {

            int clId = 0, siteId = 0, maxDisc = 0;
            //groupA = 0; groupB = 0; groupC = 0; totDisc = 0; totDiscA = 0; totDiscB = 0;
            calculatedDisc = 0; maxDiscnt = 0; appliedDisc = 0; introDiscA = 0; volDiscB = 0; timelyDiscC = 0; AdvDiscD = 0; loyDiscE = 0; propDiscF = 0; AppDiscG = 0;

            var details = dc.Enquiry_View(Convert.ToInt32(txt_EnquiryNo.Text), 1, 0);
            foreach (var ds in details)
            {
                clId = Convert.ToInt32(ds.CL_Id);
                siteId = Convert.ToInt32(ds.SITE_Id);
                break;
            }

            maxDisc = getMaxDiscount(recordType);
            lblMax.Text = "Max (%) = " + maxDisc.ToString();


            lblClId.Text = clId.ToString();
            if (clId > 0)
            {
                var clDisc = dc.DiscountSetting_View(clId, "");//view all discount of that client
                foreach (var item in clDisc)
                {
                     appliedDisc = Convert.ToDouble(item.Applicable.ToString());
                    break;
                }
            }
            if (appliedDisc > maxDisc)
                appliedDisc = maxDisc;

            // if (dc.Connection.Database.ToLower().ToString() == "veenalive")
            //{
            //    //for ST  if applied dis<30 then set it to bydefault 30
            //    if (recordType == "ST")
            //    {
            //        if (appliedDisc < 30)
            //            appliedDisc = 30;
            //    }
            //}

            return appliedDisc;
        }
        protected Boolean ValidateData()
        {

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean valid = true;
            DateTime ProposalDate = DateTime.ParseExact(txt_ProposalDt.Text, "dd/MM/yyyy", null);
            DateTime CurrentDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);

            if (ProposalDate > CurrentDate)
            {
                lblMsg.Text = "Proposal Date should be always less than or equal to the Current Date.";
                txt_ProposalDt.Focus();
                valid = false;
            }
            else if (txtSiteName.Text == "")
            {
                lblMsg.Text = "Enter Site Name";
                txtSiteName.Focus();
                valid = false;
            }
            else if (txt_KindAttention.Text == "")
            {
                lblMsg.Text = "Enter Kind Attention";
                txt_KindAttention.Focus();
                valid = false;
            }
            else if (txt_Subject.Text == "")
            {
                lblMsg.Text = "Enter Subject";
                txt_Subject.Focus();
                valid = false;
            }
            else if (txt_Description.Text == "")
            {
                lblMsg.Text = "Enter Description";
                txt_Description.Focus();
                valid = false;
            }
            //else if (txtPaymentTerm.Text == "")
            //{
            //    msg = "Enter Payment term";
            //    txtPaymentTerm.Focus();
            //    valid = false;
            //}
            else if (ddl_PrposalBy.SelectedItem.Text == "---Select---")
            {
                lblMsg.Text = "Select Proposal By";
                ddl_PrposalBy.Focus();
                valid = false;
            }
            else if (txt_ContactNo.Text == "")
            {
                lblMsg.Text = "Select the Contact No.";
                txt_ContactNo.Focus();
                valid = false;
            }
            else if (txtMe.Text == "")
            {
                lblMsg.Text = "Enter ME Name";
                txtMe.Focus();
                valid = false;
            }
            else if (txtMeNum.Text == "")
            {
                lblMsg.Text = "Enter Contact No.";
                txtMeNum.Focus();
                valid = false;
            }
            else if(txtEmail.Text=="")
            {
                lblMsg.Text = "Enter Email Id";
                valid = false;
            }
            else if (grdProposal.Rows.Count == 0 && grdProposal.Visible == true)//&& chk_Proposal.Checked)
            {
                lblMsg.Text = "Please enter Inward Test";
                valid = false;
            }
            else if (ddl_InwardTestType.SelectedValue == "OT" && ddlOtherTest.SelectedIndex > 0 && ddlOtherTest.SelectedItem.Text == "Structural Audit")
            {
                if (txtStructNameOfApartSoc.Text == "")
                {
                    lblMsg.Text = "Enter 'Name of Apartment / Society'";
                    txtStructNameOfApartSoc.Focus();
                    valid = false;
                }
                else if (txtStructAddress.Text == "")
                {
                    lblMsg.Text = "Enter 'Address'";
                    txtStructAddress.Focus();
                    valid = false;
                }
                else if (txtStructBuiltupArea.Text == "")
                {
                    lblMsg.Text = "Enter 'Builtup Area of Society'";
                    txtStructBuiltupArea.Focus();
                    valid = false;
                }
                else if (txtStructNoOfBuild.Text == "")
                {
                    lblMsg.Text = "Enter 'No of buildings in Society'";
                    txtStructNoOfBuild.Focus();
                    valid = false;
                }
                else if (txtStructAge.Text == "")
                {
                    lblMsg.Text = "Enter 'Age of Building'";
                    txtStructAge.Focus();
                    valid = false;
                }
                else if (ddlStructConstWithin5Y.SelectedIndex == 0)
                {
                    lblMsg.Text = "Select 'All buildings constructed with in 5 years range'";
                    ddlStructConstWithin5Y.Focus();
                    valid = false;
                }
                else if (ddlStructLocation.SelectedIndex == 0)
                {
                    lblMsg.Text = "Select 'Location'";
                    ddlStructLocation.Focus();
                    valid = false;
                }
                else if (ddlStructAddLoadExpc.SelectedIndex == 0)
                {
                    lblMsg.Text = "Select 'Any additional loads expected on building'";
                    ddlStructAddLoadExpc.Focus();
                    valid = false;
                }
                else if (ddlStructDistressObs.SelectedIndex == 0)
                {
                    lblMsg.Text = "Select 'Any Distress Observed'";
                    ddlStructDistressObs.Focus();
                    valid = false;
                }
            }
            if (valid == true && grdProposal.Visible == true)
            {
                int flag = 0;//to chk discount is blank or not in case of editable distount rate validation 
                for (int j = 0; j < grdProposal.Rows.Count; j++)
                {
                    TextBox txt_Quantity = (TextBox)grdProposal.Rows[j].Cells[5].FindControl("txt_Quantity");
                    TextBox txt_Rate = (TextBox)grdProposal.Rows[j].Cells[4].FindControl("txt_Rate");
                    TextBox txt_DiscRate = (TextBox)grdProposal.Rows[j].Cells[6].FindControl("txt_DiscRate");

                    if (ddlOtherTest.Visible == true && ddl_InwardTestType.SelectedValue == "OT")
                    {
                        flag = 1;
                        if (ddlOtherTest.SelectedIndex > 0)
                        {
                            if (ddlOtherTest.SelectedItem.Text == "Earth Resistivity Test" || ddlOtherTest.SelectedItem.Text == "Plate Load Testing")
                            {
                                //if (txt_DiscRate.Text != "")
                                if (j != 1)
                                    txt_Rate.Text = txt_DiscRate.Text;
                                //else if (txt_Rate.Text != "")
                                //    txt_DiscRate.Text = txt_Rate.Text;
                            }
                        }
                    }
                    else if (ddl_InwardTestType.SelectedValue == "RWH")
                    {
                        flag = 1;
                        // if (txt_DiscRate.Text != "")
                        if (j != 1)
                            txt_Rate.Text = txt_DiscRate.Text;
                        //else if (txt_Rate.Text != "")
                        //    txt_DiscRate.Text = txt_Rate.Text;
                    }


                    //if (txt_Rate.Text == "")
                    //{
                    //    lblMsg.Text = "Input Rate for row no " + (j + 1) + ".";
                    //    valid = false;
                    //    break;
                    //}
                    //else 

                    if (flag == 1)
                    {
                        if (txt_DiscRate.Text == "")
                        {
                            lblMsg.Text = "Input Discount Rate for row no " + (j + 1) + ".";
                            valid = false;
                            break;
                        }
                        else if (Convert.ToDecimal(txt_DiscRate.Text) == 0)
                        {
                            lblMsg.Text = "Discount Rate should be greater than 0 for row no " + (j + 1) + ".";
                            valid = false;
                            break;
                        }

                    }

                    if (txt_Quantity.Text == "")
                    {
                        lblMsg.Text = "Input Quanity for row no " + (j + 1) + ".";
                        valid = false;
                        break;
                    }
                    else if (txt_Rate.Text != "" && txt_DiscRate.Text != "")
                    {
                        if (Convert.ToDecimal(txt_Rate.Text) > 0)
                        {
                            if (Convert.ToDecimal(txt_Rate.Text) < Convert.ToDecimal(txt_DiscRate.Text))
                            {
                                lblMsg.Text = "Discounted Rate should be less than equal to Unit Rate for row no " + (j + 1) + ".";
                                valid = false;
                                break;
                            }
                        }
                    }

                }
            }
            else if (valid == true && grdGT.Visible == true)//29/01/2018
            {

                for (int j = 0; j < grdGT.Rows.Count; j++)
                {
                    TextBox txt_Quantity = (TextBox)grdGT.Rows[j].Cells[4].FindControl("txt_Quantity");
                    TextBox txt_UnitRate = (TextBox)grdGT.Rows[j].Cells[5].FindControl("txt_UnitRate");
                    TextBox txt_DiscRate = (TextBox)grdGT.Rows[j].Cells[6].FindControl("txt_DiscRate");

                    if (txt_UnitRate.Visible == true && txt_DiscRate.Visible == true)
                    {
                        if (j != 1)
                        {
                            if (txt_DiscRate.Text != "")
                                txt_UnitRate.Text = txt_DiscRate.Text;
                        }

                        if (txt_Quantity.Text == "")
                        {
                            lblMsg.Text = "Input Quantity for row no " + (j + 1) + ".";
                            valid = false;
                            break;
                        }
                        //else if (txt_UnitRate.Text == "")
                        //{
                        //    lblMsg.Text = "Input Unit Rate for row no " + (j + 1) + ".";
                        //    valid = false;
                        //    break;
                        //}
                        else if (txt_DiscRate.Text == "")
                        {
                            lblMsg.Text = "Input Discount Rate for row no " + (j + 1) + ".";
                            valid = false;
                            break;
                        }
                        else if (Convert.ToDecimal(txt_DiscRate.Text) == 0)
                        {
                            lblMsg.Text = "Discount Rate should be greater than 0 for row no " + (j + 1) + ".";
                            valid = false;
                            break;
                        }
                        else if (txt_UnitRate.Text != "")
                        {
                            if (Convert.ToDecimal(txt_UnitRate.Text) > 0)
                            {
                                if (Convert.ToDecimal(txt_UnitRate.Text) < Convert.ToDecimal(txt_DiscRate.Text))
                                {
                                    lblMsg.Text = "Discounted Rate should be less than equal to Unit Rate for row no " + (j + 1) + ".";
                                    valid = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (valid == false)
            {
                lblMsg.Visible = true;
            }
            else
            {
                lblMsg.Visible = false;
                Calculation();
            }
            return valid;
        }
        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            if (txt_EnquiryNo.Text != "")
            {
                int  qtyFlag = 0, proposalId = 0; //status = 0,

                if (chkQty.Checked)
                    qtyFlag = 1;

                var res = dc.Proposal_View(Convert.ToInt32(txt_EnquiryNo.Text), Convert.ToBoolean(lblEnqNewClient.Text), 0, txt_ProposalNo.Text);
                foreach (var reslt in res)
                {
                    proposalId = reslt.Proposal_Id;
                    break;
                }

                PrintPDFReport rpt = new PrintPDFReport();
                rpt.Proposal_PDF(Convert.ToInt32(txt_EnquiryNo.Text), qtyFlag, proposalId, Convert.ToBoolean(lblEnqNewClient.Text), "View", txt_ProposalNo.Text);
            }
        }
        protected void ddl_PrposalBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_ContactNo.Text = "";
            if (ddl_PrposalBy.SelectedIndex > 0)
            {
                var proposal = dc.Proposal_View_ContactNo(Convert.ToInt32(ddl_PrposalBy.SelectedValue));
                foreach (var prop in proposal)
                {
                    txt_ContactNo.Text = prop.Proposal_ContactNo;
                    break;
                }
            }
        }
        public void Calculation()
        {
            //gross total
            decimal GrossAmount = 0, GstCalculatedAmt = 0, GstAmount = 0;
            if (grdProposal.Visible == true)
            {
                foreach (GridViewRow r in grdProposal.Rows)
                {
                    TextBox txt_Amount = r.FindControl("txt_Amount") as TextBox;
                    TextBox txt_Rate = r.FindControl("txt_Rate") as TextBox;
                    TextBox txt_Quantity = r.FindControl("txt_Quantity") as TextBox;
                    TextBox txt_DiscRate = r.FindControl("txt_DiscRate") as TextBox;

                    if (txt_DiscRate.Text != "" && txt_Quantity.Text != "")
                    {
                        txt_Amount.Text = Convert.ToDecimal(Convert.ToDecimal(txt_Quantity.Text) * Convert.ToDecimal(txt_DiscRate.Text)).ToString("0.00");
                    }
                    decimal number = 0; 
                    if (decimal.TryParse(txt_Amount.Text, out number))
                    {
                        GrossAmount += number;
                    }
                }
                txt_NetAmount.Text = Math.Round(GrossAmount).ToString("0.00");
            }
            else if (grdGT.Visible == true)
            {
                int count = 0;
                foreach (GridViewRow r in grdGT.Rows)
                {
                    TextBox txt_Amount = r.FindControl("txt_Amount") as TextBox;
                    TextBox txt_UnitRate = r.FindControl("txt_UnitRate") as TextBox;
                    TextBox txt_DiscRate = r.FindControl("txt_DiscRate") as TextBox;
                    TextBox txt_Quantity = r.FindControl("txt_Quantity") as TextBox;

                    if (count != 1)
                    {
                        if (txt_DiscRate.Text != "")
                            txt_UnitRate.Text = txt_DiscRate.Text;
                    }
                    decimal qty;
                    if (decimal.TryParse(txt_Quantity.Text, out qty))
                    {
                        // it's a valid integer => you could use the distance variable here
                        if (txt_DiscRate.Text != "" && txt_Quantity.Text != "")
                        {
                            txt_Amount.Text = Convert.ToDecimal(Convert.ToDecimal(txt_Quantity.Text) * Convert.ToDecimal(txt_DiscRate.Text)).ToString("0.00");
                        }
                    }
                    else
                    {
                        //if (txt_DiscRate.Text != "")
                        txt_Amount.Text = txt_DiscRate.Text;
                        //else if (txt_UnitRate.Text != "")
                        //    txt_Amount.Text = txt_UnitRate.Text;
                    }

                    decimal number = 0;
                    if (txt_Amount.Visible == true)
                    {
                        if (decimal.TryParse(txt_Amount.Text, out number))
                        {
                            GrossAmount += number;
                        }
                    }

                    count++;
                }
                txt_NetAmount.Text = Math.Round(GrossAmount).ToString("0.00");
            }
            GstAmount = GrossAmount * Convert.ToDecimal(0.18);
            GstCalculatedAmt = GrossAmount + GstAmount;
            lblGstAmt.Text = GstAmount.ToString("0.00");
            lblGrandTotal.Text = Math.Round(GstCalculatedAmt).ToString("0.00");
        }
        protected void Tab_Discount_Click(object sender, EventArgs e)
        {
            Tab_Discount.CssClass = "Clicked";
            Tab_Notes.CssClass = "Initial";
            Tab_GenericDiscount.CssClass = "Initial";
            MainView_Proposal.ActiveViewIndex = 1;

            ViewState["Discount_Table"] = null;
            string testNm = "";
            if (grdProposal.Visible == true)
            {
                for (int i = 0; i < grdProposal.Rows.Count; i++)
                {
                    TextBox txt_Particular = (TextBox)grdProposal.Rows[i].Cells[3].FindControl("txt_Particular");
                    TextBox txt_Rate = (TextBox)grdProposal.Rows[i].Cells[4].FindControl("txt_Rate");
                    TextBox txt_Discount = (TextBox)grdProposal.Rows[i].Cells[5].FindControl("txt_Discount");
                    TextBox txt_DiscRate = (TextBox)grdProposal.Rows[i].Cells[6].FindControl("txt_DiscRate");
                    Label lbl_InwdType = (Label)grdProposal.Rows[i].Cells[9].FindControl("lbl_InwdType");
                    Label lbl_SiteWiseRate = (Label)grdProposal.Rows[i].Cells[10].FindControl("lbl_SiteWiseRate");


                    AddRowDiscount();
                    Label lblMaterialName = (Label)grdDiscount.Rows[i].Cells[0].FindControl("lblMaterialName");
                    Label lblTestName = (Label)grdDiscount.Rows[i].Cells[1].FindControl("lblTestName");
                    TextBox txtSiteWiseDisc = (TextBox)grdDiscount.Rows[i].Cells[2].FindControl("txtSiteWiseDisc");
                    TextBox txtVolDisc = (TextBox)grdDiscount.Rows[i].Cells[3].FindControl("txtVolDisc");
                    TextBox txtAppliedDisc = (TextBox)grdDiscount.Rows[i].Cells[4].FindControl("txtAppliedDisc");


                    lblMaterialName.Text = lbl_InwdType.Text;
                    if (i == 0)
                        testNm = lbl_InwdType.Text;

                    if (i != 0)
                    {
                        if (lblMaterialName.Text == testNm)
                            lblMaterialName.Text = "";
                        else
                            testNm = lbl_InwdType.Text;
                    }
                    lblTestName.Text = txt_Particular.Text;
                    if (txt_Discount.Text == "-")
                    {
                        double calDisc = Math.Round(((Convert.ToDouble(txt_Rate.Text) - (Convert.ToDouble(lbl_SiteWiseRate.Text))) * 100) / Convert.ToDouble(txt_Rate.Text));

                        txtSiteWiseDisc.Text = calDisc.ToString();
                        txtAppliedDisc.Text = calDisc.ToString();
                        txtVolDisc.Text = "-";
                    }
                    else if (txt_Discount.Text != "-")
                    {
                        string dis = "0";
                        if (txt_Discount.Text != "")
                            dis = txt_Discount.Text;

                        txtVolDisc.Text = dis;
                        txtAppliedDisc.Text = dis;
                        txtSiteWiseDisc.Text = "-";
                    }

                }

            }
            else
            {
                //for (int i = 0; i < grdGT.Rows.Count; i++)
                //{
                //TextBox txt_Particular = (TextBox)grdGT.Rows[i].Cells[3].FindControl("txt_Particular");
                //TextBox txt_Rate = (TextBox)grdGT.Rows[0].Cells[4].FindControl("txt_Rate");
                //TextBox txt_Discount = (TextBox)grdGT.Rows[i].Cells[5].FindControl("txt_Discount");
                //TextBox txt_DiscRate = (TextBox)grdGT.Rows[i].Cells[6].FindControl("txt_DiscRate");
                Label lbl_InwdType = (Label)grdGT.Rows[0].Cells[9].FindControl("lbl_InwdType");
                //Label lbl_SiteWiseRate = (Label)grdGT.Rows[0].Cells[10].FindControl("lbl_SiteWiseRate");


                AddRowDiscountGT();
                Label lblMaterialName = (Label)grdDiscount.Rows[0].Cells[0].FindControl("lblMaterialName");
                Label lblTestName = (Label)grdDiscount.Rows[0].Cells[1].FindControl("lblTestName");
                TextBox txtSiteWiseDisc = (TextBox)grdDiscount.Rows[0].Cells[2].FindControl("txtSiteWiseDisc");
                TextBox txtVolDisc = (TextBox)grdDiscount.Rows[0].Cells[3].FindControl("txtVolDisc");
                TextBox txtAppliedDisc = (TextBox)grdDiscount.Rows[0].Cells[4].FindControl("txtAppliedDisc");


                lblMaterialName.Text = lbl_InwdType.Text;
                double disc = getDiscount(lblRType.Text);
                lblTestName.Text = "";
                if (disc > 0)
                {
                    txtVolDisc.Text = disc.ToString();
                    txtAppliedDisc.Text = disc.ToString();
                    txtSiteWiseDisc.Text = "-";


                }
                else
                {

                    txtSiteWiseDisc.Text = "0";
                    txtAppliedDisc.Text = "0";
                    txtVolDisc.Text = "-";
                }

                //}

            }

        }
        protected void Tab_Notes_Click(object sender, EventArgs e)
        {
            Tab_Notes.CssClass = "Clicked";
            Tab_Discount.CssClass = "Initial";
            Tab_GenericDiscount.CssClass = "Initial";
            MainView_Proposal.ActiveViewIndex = 0;
        }

        protected void Tab_GenericDiscount_Click(object sender, EventArgs e)
        {
            Tab_Notes.CssClass = "Initial";
            Tab_Discount.CssClass = "Initial";
            Tab_GenericDiscount.CssClass = "Clicked";
            MainView_Proposal.ActiveViewIndex = 2;
            //DisplayGenericDiscountDetails();
        }

        private void DisplayGenericDiscountDetails()
        {
            //lblIntro.ForeColor = System.Drawing.Color.Black;
            //lblVol.ForeColor = System.Drawing.Color.Black;
            //lblTime.ForeColor = System.Drawing.Color.Black;
            //lblLoy.ForeColor = System.Drawing.Color.Black;
            //lblProp.ForeColor = System.Drawing.Color.Black;
            //lblMax.ForeColor = System.Drawing.Color.Black;
            //lblDisc.ForeColor = System.Drawing.Color.Black;
            //lblCalcDisc.ForeColor = System.Drawing.Color.Black;

            if (grdProposal.Rows.Count > 0 || grdGT.Rows.Count > 0)
            {
                txtIntro.Text = introDiscA.ToString();
                lblVol.Text = "Volumn (%) = " + volDiscB.ToString();
                lblTime.Text = "Timely Payment (%) = " + timelyDiscC.ToString();
                lblAdv.Text = "Advance (%) = " + AdvDiscD.ToString();
                lblLoy.Text = "Loyalty (%) = " + loyDiscE.ToString();
                lblProp.Text = "Proposal (%) = " + propDiscF.ToString();
                lblApp.Text = "App (%) = " + AppDiscG.ToString();
                lblMax.Text = "Max (%) = " + maxDiscnt.ToString();
                lblDisc.Text = "Applied (%) = " + appliedDisc.ToString();
                lblCalcDisc.Text = "Calculated (%) = " + calculatedDisc.ToString();

                //if (groupA == 1)
                //{
                //    lblIntro.ForeColor = System.Drawing.Color.Green;
                //    lblAdv.ForeColor = System.Drawing.Color.Green;
                //    lblProp.ForeColor = System.Drawing.Color.Green;
                //    lblApp.ForeColor = System.Drawing.Color.Green;
                //    lblCalcDisc.ForeColor = System.Drawing.Color.Green;
                //}
                //else if (groupB == 1)
                //{
                //    lblVol.ForeColor = System.Drawing.Color.Green;
                //    lblTime.ForeColor = System.Drawing.Color.Green;
                //    lblLoy.ForeColor = System.Drawing.Color.Green;
                //    lblProp.ForeColor = System.Drawing.Color.Green;
                //    lblApp.ForeColor = System.Drawing.Color.Green;
                //    lblCalcDisc.ForeColor = System.Drawing.Color.Green;
                //}
                //else if (groupC == 1)
                //{
                //    lblMax.ForeColor = System.Drawing.Color.Green;
                //}
            }
        }

        protected void lnkUpdateIntro_Click(object sender, EventArgs e)
        {
            int introDisc = 0;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            lblMsg.Visible = false; clsData obj = new clsData();
            //update introductory discount
            if (lblClId.Text != "" && lblClId.Text != "0")
            {
                if (txtIntro.Text != "")
                {
                    introDisc = Convert.ToInt32(txtIntro.Text);
                    if (introDisc <= 10)
                    {
                        //string discountType = "Introductory";
                        //update intro discount
                        dc.DiscountSetting_Update_Introductory(Convert.ToInt32(lblClId.Text), introDisc);
                        //apply changed discount to tests
                        //grdProposal.DataSource = null;
                        //grdProposal.DataBind();
                        if (grdProposal.Visible == true)
                        {
                            for (int i = 0; i < grdProposal.Rows.Count; i++)
                            {
                                Label lbl_InwdType = (Label)grdProposal.Rows[i].Cells[9].FindControl("lbl_InwdType");

                                //inwardType = obj.getInwardTypeValue(lbl_InwdType.Text);
                                if (lbl_InwdType.Text != "")
                                {
                                    //  addTestToGrid(inwardType);
                                    updateTestToGrid(lbl_InwdType.Text, i);
                                }
                            }
                        }
                        else if (grdGT.Visible == true)
                        {
                            updateTestToGrid("GT", 1);
                        }
                        DisplayGenericDiscountDetails();
                        Calculation();
                        //discountType = "Applicable";
                        dc.DiscountSetting_Update_Introductory(Convert.ToInt32(lblClId.Text), Convert.ToDecimal(appliedDisc));
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Introductory Discount should be less than equal to 10.";
                    }

                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Input Introductoty Discount";
                }
            }
        }
        private void updateTestToGrid(string inwardType, int rowPos)
        {
            int testId = 0; double siteRate = 0, disc = 0, calDisc = 0, discountedRate = 0;
            var res = dc.Test_View_ForProposal(-1, 0, inwardType, 0, 0, 0);
            if (grdProposal.Visible == true)
            {
                TextBox txt_Particular = (TextBox)grdProposal.Rows[rowPos].Cells[3].FindControl("txt_Particular");
                TextBox txt_Rate = (TextBox)grdProposal.Rows[rowPos].Cells[4].FindControl("txt_Rate");
                TextBox txt_Discount = (TextBox)grdProposal.Rows[rowPos].Cells[5].FindControl("txt_Discount");
                TextBox txt_DiscRate = (TextBox)grdProposal.Rows[rowPos].Cells[6].FindControl("txt_DiscRate");
                TextBox txt_Quantity = (TextBox)grdProposal.Rows[rowPos].Cells[7].FindControl("txt_Quantity");
                TextBox txt_Amount = (TextBox)grdProposal.Rows[rowPos].Cells[8].FindControl("txt_Amount");
                Label lbl_InwdType = (Label)grdProposal.Rows[rowPos].Cells[9].FindControl("lbl_InwdType");
                Label lbl_SiteWiseRate = (Label)grdProposal.Rows[rowPos].Cells[10].FindControl("lbl_SiteWiseRate");
                Label lbl_TestId = (Label)grdProposal.Rows[rowPos].Cells[11].FindControl("lbl_TestId");

                if (lbl_TestId.Text != "")
                    testId = Convert.ToInt32(lbl_TestId.Text);

                siteRate = chkSiteWiseRate(testId);

                if (siteRate != 0)//apply sitewise rate setting
                {
                    txt_DiscRate.Text = siteRate.ToString("0.00");
                    //calculate rev. discount
                    calDisc = Math.Round(((Convert.ToDouble(txt_Rate.Text) - (Convert.ToDouble(siteRate))) * 100) / Convert.ToDouble(txt_Rate.Text));
                    //txt_Discount.Text = calDisc.ToString();
                    txt_Discount.Text = "-";
                    //string inType = ddl_InwardTestType.Items.FindByValue(re.Test_RecType_var).Text;
                    // discListDetails = discListDetails + txt_Particular.Text + "~" + calDisc + "~" + "-" + "~" + calDisc + "|";
                    lbl_SiteWiseRate.Text = siteRate.ToString();
                }
                else //apply discount setting
                {
                    disc = getDiscount(lbl_InwdType.Text);
                    txt_Discount.Text = disc.ToString();
                    discountedRate = Convert.ToDouble(txt_Rate.Text) - (Convert.ToDouble(txt_Rate.Text) * (disc / 100));
                    txt_DiscRate.Text = discountedRate.ToString("0.00");
                    //string inType = ddl_InwardTestType.Items.FindByValue(re.Test_RecType_var).Text;
                    //  discListDetails = discListDetails + txt_Particular.Text + "~" + "-" + "~" + disc + "~" + disc + "|";
                    lbl_SiteWiseRate.Text = "0";
                }
            }
            else if (grdGT.Visible == true)
            {
                TextBox txt_UnitRate = (TextBox)grdGT.Rows[rowPos].Cells[5].FindControl("txt_UnitRate");
                TextBox txt_DiscRate = (TextBox)grdGT.Rows[rowPos].Cells[6].FindControl("txt_DiscRate");

                disc = getDiscount("GT");
                discountedRate = Convert.ToDouble(txt_UnitRate.Text) - (Convert.ToDouble(txt_UnitRate.Text) * (disc / 100));
                txt_DiscRate.Text = discountedRate.ToString("0.00");

            }
        }
        protected void LnkExit_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }
        protected void grdProposal_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ddlOtherTest.Visible == true && ddl_InwardTestType.SelectedValue == "OT")
                {
                    //TextBox txt_Rate = (TextBox)e.Row.FindControl("txt_Rate");
                    //txt_Rate.ReadOnly = false;
                    if (ddlOtherTest.SelectedIndex != -1 && ddlOtherTest.SelectedIndex != 0)
                    {
                        if ((ddlOtherTest.SelectedItem.Text == "Earth Resistivity Test" || ddlOtherTest.SelectedItem.Text == "Plate Load Testing"))
                        {
                            TextBox txt_DiscRate = (TextBox)e.Row.FindControl("txt_DiscRate");
                            txt_DiscRate.ReadOnly = false;
                        }
                    }
                }
                else if (ddl_InwardTestType.SelectedValue == "RWH")
                {
                    TextBox txt_DiscRate = (TextBox)e.Row.FindControl("txt_DiscRate");
                    txt_DiscRate.ReadOnly = false;
                }
                else
                {
                    TextBox txt_DiscRate = (TextBox)e.Row.FindControl("txt_DiscRate");
                    txt_DiscRate.ReadOnly = true;
                }
            }


        }
        protected void grdGT_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if (ddlOtherTest.Visible==true && (ddlOtherTest.SelectedItem.Text=="Earth Resistivity Test" ||ddlOtherTest.SelectedItem.Text=="Plate Load Testing"))
                //{
                //    //TextBox txt_Rate = (TextBox)e.Row.FindControl("txt_Rate");
                //    //txt_Rate.ReadOnly = false;

                //    TextBox txt_DiscRate = (TextBox)e.Row.FindControl("txt_DiscRate");
                //    txt_DiscRate.ReadOnly = false;
                //}
                //else if (ddl_InwardTestType.SelectedValue == "RWH")
                //{
                //    TextBox txt_DiscRate = (TextBox)e.Row.FindControl("txt_DiscRate");
                //    txt_DiscRate.ReadOnly = false;
                //}
            }

        }
        protected void lnkClear_Click(object sender, EventArgs e)
        {
            ViewState["ProposalTable"] = null;
            grdProposal.DataSource = null;
            grdProposal.DataBind();

            ViewState["ProposalTableGT"] = null;
            grdGT.DataSource = null;
            grdGT.DataBind();
            grdProposal.DataSource = null;
            grdProposal.DataBind();

            Grd_Note.DataSource = null;
            Grd_Note.DataBind();
            Grd_NoteGT.DataSource = null;
            Grd_NoteGT.DataBind();
            grdPayTermsGT.DataSource = null;
            grdPayTermsGT.DataBind();
            grdClientScope.DataSource = null;
            grdClientScope.DataBind();

            txtPaymentTerm.Text = "";
        }

        protected void ddl_InwardTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_InwardTestType.SelectedValue == "OT")
            {
                ddlOtherTest.Visible = true;
                //LoadOtherTestList();
            }
            else
            {
                if (ddl_InwardTestType.SelectedValue == "RWH")
                {
                    Grd_NoteGT.Visible = true;
                    lblAddChargesGT.Visible = true;

                    grdPayTermsGT.Visible = true;
                    lblPayTermsGT.Visible = true;
                }

                //if (ddl_InwardTestType.SelectedValue == "NDT" && ddl_InwardTestType.SelectedValue == "CR"
                //    && ddl_InwardTestType.SelectedValue == "CORECUT")
                //{
                    lblClientScope.Visible = true;
                    grdClientScope.Visible = true;
                //}
                //else
                //{
                //    lblClientScope.Visible = false;
                //    grdClientScope.Visible = false;
                //}

                ddlOtherTest.Visible = false;
            }
            ddlOtherTest.SelectedIndex = -1;
        }
        ////Client Scope NDT CR CORECUT
        protected void AddRowClientScope()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            if (ViewState["ClientScopeTable"] != null)
            {
                GetCurrentDataClientScope();
                dt = (DataTable)ViewState["ClientScopeTable"];
            }
            else
            {
                dt.Columns.Add(new DataColumn("lblSrNoClientScope", typeof(string)));
                dt.Columns.Add(new DataColumn("txt_NOTEClientScope", typeof(string)));
            }
            dr = dt.NewRow();
            dr["lblSrNoClientScope"] = dt.Rows.Count + 1;
            dr["txt_NOTEClientScope"] = string.Empty;

            dt.Rows.Add(dr);
            ViewState["ClientScopeTable"] = dt;
            grdClientScope.DataSource = dt;
            grdClientScope.DataBind();
            SetPreviousDataClientScope();
        }
        protected void SetPreviousDataClientScope()
        {
            DataTable dt = (DataTable)ViewState["ClientScopeTable"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TextBox txt_NOTE = (TextBox)grdClientScope.Rows[i].Cells[2].FindControl("txt_NOTEClientScope");
                grdClientScope.Rows[i].Cells[2].Text = (i + 1).ToString();
                txt_NOTE.Text = dt.Rows[i]["txt_NOTEClientScope"].ToString();

            }
        }
        protected void GetCurrentDataClientScope()
        {
            int rowIndex = 0;
            DataTable dtTable = new DataTable();
            DataRow drRow = null;

            dtTable.Columns.Add(new DataColumn("lblSrNoClientScope", typeof(string)));
            dtTable.Columns.Add(new DataColumn("txt_NOTEClientScope", typeof(string)));
            for (int i = 0; i < grdClientScope.Rows.Count; i++)
            {
                TextBox txt_NOTE = (TextBox)grdClientScope.Rows[i].Cells[2].FindControl("txt_NOTEClientScope");

                drRow = dtTable.NewRow();
                drRow["lblSrNoClientScope"] = i + 1;
                drRow["txt_NOTEClientScope"] = txt_NOTE.Text;

                dtTable.Rows.Add(drRow);
                rowIndex++;
            }
            ViewState["ClientScopeTable"] = dtTable;
        }
        protected void DeleteRowClientScope(int rowIndex)
        {
            GetCurrentDataClientScope();
            DataTable dt = ViewState["ClientScopeTable"] as DataTable;
            dt.Rows[rowIndex].Delete();
            ViewState["ClientScopeTable"] = dt;
            grdClientScope.DataSource = dt;
            grdClientScope.DataBind();
            SetPreviousDataClientScope();
        }
        protected void imgBtnAddClientScopeRow_Click(object sender, CommandEventArgs e)
        {
            AddRowClientScope();
        }
        protected void imgBtnDeleteClientScopeRow_Click(object sender, CommandEventArgs e)
        {
            if (grdClientScope.Rows.Count > 1)
            {
                ImageButton btn = (ImageButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                if (grdClientScope.Rows[gvr.RowIndex].Cells[1].Text == "")
                    DeleteRowClientScope(gvr.RowIndex);
            }
        }
        protected void ddlOtherTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pnlStructAudit.Visible == true)
            {
                grdProposal.DataSource = null;
                grdProposal.DataBind();
                pnlStructAudit.Visible = false;
                grdProposal.Columns[3].HeaderText = "Particular";
                grdProposal.Columns[4].HeaderText = "Test Method";
            }
            if (ddl_InwardTestType.SelectedValue == "OT")
            {
                if (ddlOtherTest.SelectedItem.Text == "Structural Audit")
                {
                    grdProposal.DataSource = null;
                    grdProposal.DataBind();
                    pnlStructAudit.Visible = true;
                    grdProposal.Columns[3].HeaderText = "Members";
                    grdProposal.Columns[4].HeaderText = "Samples";
                }
                if (ddlOtherTest.SelectedItem.Text == "Plate Load Testing" || ddlOtherTest.SelectedItem.Text == "Earth Resistivity Test" || ddlOtherTest.SelectedItem.Text == "SBC by SPT")
                {
                    Grd_Note.DataSource = null;
                    Grd_Note.DataBind();
                    Grd_NoteGT.DataSource = null;
                    Grd_NoteGT.DataBind();
                    grdPayTermsGT.DataSource = null;
                    grdPayTermsGT.DataBind();
                    addTermsConditionNotes("Soil Investigation");
                }
                else
                {
                    Grd_NoteGT.Visible = false;
                    lblAddChargesGT.Visible = false;
                    grdPayTermsGT.Visible = false;
                    lblPayTermsGT.Visible = false;
                    Grd_Note.Visible = true;
                    Grd_Note.DataSource = null;
                    Grd_Note.DataBind();
                    //addTermsConditionNotes("OT");
                    addTermsConditionNotes(ddlOtherTest.SelectedItem.Text);
                }

            }
            
        }

        protected void ddlDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDepth.SelectedIndex == 0)
            {
                txtGtRate.ReadOnly = true; txtGtRate.Text = lblGtRate.Text;
            }
            else
                txtGtRate.ReadOnly = false;
        }

        protected void lnkDiscApplyToAll_Click(object sender, EventArgs e)
        {
            string msg = "";
            bool valid = true;
            if (txtDiscApplyToAll.Text == "")
            {
                msg = "Please input discount";
                valid = false;
            }
            else if (Convert.ToDecimal(txtDiscApplyToAll.Text) >= 100)
            {
                msg = "Please input discount less than 100.";
                valid = false;
            }
            else
            {
                int maxUserDiscount = 0;
                var material = dc.Material_View(ddl_InwardTestType.SelectedValue, ddl_InwardTestType.SelectedItem.Text);
                foreach (var mat in material)
                {
                    if (lblUserLevel.Text == "1")
                        maxUserDiscount = Convert.ToInt32(mat.MATERIAL_Level1Discount_int);
                    else if (lblUserLevel.Text == "2")
                        maxUserDiscount = Convert.ToInt32(mat.MATERIAL_Level2Discount_int);
                    else if (lblUserLevel.Text == "3")
                        maxUserDiscount = Convert.ToInt32(mat.MATERIAL_Level3Discount_int);
                }
                if (Convert.ToDecimal(txtDiscApplyToAll.Text) > maxUserDiscount)
                {
                    msg = "Can not apply discount greater than " + maxUserDiscount + ".";
                    valid = false;
                }
                if (valid == true)
                {
                    if (grdProposal.Visible == true)
                    {
                        for (int i = 0; i < grdProposal.Rows.Count; i++)
                        {
                            TextBox txt_Rate = (TextBox)grdProposal.Rows[i].FindControl("txt_Rate");
                            TextBox txt_DiscRate = (TextBox)grdProposal.Rows[i].FindControl("txt_DiscRate");
                            TextBox txt_Discount = (TextBox)grdProposal.Rows[i].FindControl("txt_Discount");
                            TextBox txt_Quantity = (TextBox)grdProposal.Rows[i].FindControl("txt_Quantity");
                            TextBox txt_Amount = (TextBox)grdProposal.Rows[i].FindControl("txt_Amount");
                            Label lbl_InwdType = (Label)grdProposal.Rows[i].FindControl("lbl_InwdType");
                            decimal maxTempDiscount = 0;
                            if (lbl_InwdType.Text == ddl_InwardTestType.SelectedValue || (lbl_InwdType.Text == "OTHER" && ddl_InwardTestType.SelectedValue == "OT"))
                            {                                
                                maxTempDiscount = Convert.ToDecimal(txtDiscApplyToAll.Text);
                            }
                            else
                            {
                                var material2 = dc.Material_View(lbl_InwdType.Text, "");
                                foreach (var mat in material2)
                                {
                                    if (lblUserLevel.Text == "1")
                                        maxTempDiscount = Convert.ToInt32(mat.MATERIAL_Level1Discount_int);
                                    else if (lblUserLevel.Text == "2")
                                        maxTempDiscount = Convert.ToInt32(mat.MATERIAL_Level2Discount_int);
                                    else if (lblUserLevel.Text == "3")
                                        maxTempDiscount = Convert.ToInt32(mat.MATERIAL_Level3Discount_int);
                                }
                                if (maxTempDiscount > Convert.ToDecimal(txtDiscApplyToAll.Text))
                                    maxTempDiscount = Convert.ToDecimal(txtDiscApplyToAll.Text);
                            }
                            if (chkDiscApplyToAll.Checked == true || txt_Rate.Text == txt_DiscRate.Text || txt_Discount.Text == "" || Convert.ToDecimal(txt_Discount.Text) == 0)
                            {
                                //txt_Discount.Text = txtDiscApplyToAll.Text;
                                txt_Discount.Text = maxTempDiscount.ToString();
                                txt_DiscRate.Text = Convert.ToDecimal(Convert.ToDecimal(txt_Rate.Text) - (Convert.ToDecimal(txt_Rate.Text) * (Convert.ToDecimal(txt_Discount.Text) / 100))).ToString("0.00");
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < grdGT.Rows.Count; i++)
                        {
                            TextBox txt_UnitRate = (TextBox)grdGT.Rows[i].FindControl("txt_UnitRate");
                            TextBox txt_DiscRate = (TextBox)grdGT.Rows[i].FindControl("txt_DiscRate");
                            TextBox txt_Quantity = (TextBox)grdGT.Rows[i].FindControl("txt_Quantity");
                            TextBox txt_Amount = (TextBox)grdGT.Rows[i].FindControl("txt_Amount");
                            if (chkDiscApplyToAll.Checked == true || txt_UnitRate.Text == txt_DiscRate.Text)
                            {
                                txt_DiscRate.Text = Convert.ToDecimal(Convert.ToDecimal(txt_UnitRate.Text) - (Convert.ToDecimal(txt_UnitRate.Text) * (Convert.ToDecimal(txtDiscApplyToAll.Text) / 100))).ToString("0.00");
                            }
                        }
                    }
                    Calculation();
                }
            }
            if (msg != "")
            {
                //ClientScript.RegisterStartupScript(UpdatePanel1.GetType(), "myalert", "alert('" + msg + "');", true);
                //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", msg, true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, GetType(), "Showalert", "alert('" + msg + "');", true);
            }
        }

        protected void lnkSendForApproval_Click(object sender, EventArgs e)
        {
            LnkBtnSave_Click(sender, e);
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            if (lblMsg.Visible == true && lblMsg.Text == "Record Saved Successfully")
            {
                dc.Proposal_Update_ApprovalPendingStatus(txt_ProposalNo.Text, true);
            }
        }
        
    }
}


