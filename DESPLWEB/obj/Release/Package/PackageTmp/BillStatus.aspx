<%@ Page Title="Bill Status" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="BillStatus.aspx.cs" Inherits="DESPLWEB.BillStatus"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">   
              .cssPager span { background-color:#4f6b72; font-size:18px;}    
              .cssPager td
            {
                  padding-left: 4px;     
                  padding-right: 4px;    
              }
        </style>
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td width="12%">
                        <asp:Label ID="lblBilldt" runat="server" Text="Bill Date "></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtFromDate" Width="100px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txtToDate" Width="100px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="lblBillNo" runat="server" Text="Bill No. "></asp:Label>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txtBillNo" Width="100px" MaxLength="50" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkEnteredBillPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkEnteredBillPrint_Click" Font-Bold="True">Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkMonthlyBillDetail" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkMonthlyBillDetail_Click" Font-Bold="True">Monthly Bill Details</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSendForReapproval" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkSendForReapproval_Click" Font-Bold="True" Visible="false">Send for Re-Approval</asp:LinkButton>
                        <asp:LinkButton ID="lnkTemp" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkTemp_Click" Font-Bold="True" Visible="false">Check wrong bill</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lnkTemp2" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkTemp2_Click" Font-Bold="True" Visible="false">Update Test Id</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lnkTemp3" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkTemp3_Click" Font-Bold="True" Visible="false">Update monthly Test Id</asp:LinkButton>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblInwdtype" runat="server" Text="Inward Type"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_InwardTestType" Width="150px" runat="server">
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:CheckBox ID="chkDuplicateBill" Text="Duplicate Bill Print" runat="server" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td align="left"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" Text="Bill Status"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" Width="70px" runat="server">
                            <asp:ListItem Text="Ok" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Cancel" Value="1"></asp:ListItem>
                        </asp:DropDownList>&nbsp; &nbsp;
                        <asp:Label ID="lblApproveStatus" runat="server" Text="Approval"></asp:Label>&nbsp;&nbsp;
                       <asp:DropDownList ID="ddlApproveStatus" Width="80px" runat="server">
                            <asp:ListItem Text="All" Value="2"></asp:ListItem>
                             <asp:ListItem Text="Pending" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                        </asp:DropDownList>&nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                        &nbsp;&nbsp;
                        <asp:Label ID="lblClientApproveStatus" runat="server" Text="Client Approval"></asp:Label>&nbsp;&nbsp;
                         <asp:DropDownList ID="ddlClientApproveStatus" Width="80px" runat="server">
                            <asp:ListItem Text="All" Value="2"></asp:ListItem>
                             <asp:ListItem Text="Pending" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                        </asp:DropDownList>&nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearchClApprStatusWise" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearchClApprStatusWise_Click" Style="margin-top: 0px" />
                    </td>
                    <td colspan="2" align="right">
                        <asp:LinkButton ID="lnkFetchModifiedBills" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkFetchModifiedBills_Click" Font-Bold="True">Fetch Modified Bills</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                         <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                             OnClick="lnkPrint_Click" Font-Bold="True">Print List</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrintPendingList" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkPrintPendingList_Click" Font-Bold="True">Print Approval Pending List</asp:LinkButton>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="5" style="height: 5px"></td>
                </tr>

                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblTotal" runat="server" Text="" Font-Bold="true" ForeColor="OrangeRed"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblPaidAmount" runat="server" Text="" Font-Bold="true" ForeColor="OrangeRed"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblPendingAmount" runat="server" Text="" Font-Bold="true" ForeColor="OrangeRed"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlBillList" runat="server" ScrollBars="Auto" Height="370px" Width="940px"
                BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">
                <%--<div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 370px; overflow: auto">--%>
                        <asp:GridView ID="grdModifyBill" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False"
                            OnRowCommand="grdModifyInward_RowCommand" OnRowDataBound="grdModifyInward_RowDataBound"> 
                            <%--PageSize="5" ShowFooter="true" AllowPaging="true" OnPageIndexChanging="grdModifyInward_PageIndexChanging" OnPreRender="grdModifyInward_PreRender"--%>
                            <%--<PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />                            
                            <PagerStyle BorderColor="Blue" Font-Bold="True" Font-Italic="False" Font-Names="Arial"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False"
                            ForeColor="Blue" HorizontalAlign="Center" Wrap="True" />--%>
                            <Columns>
                                <asp:BoundField DataField="BILL_Id" HeaderText="Bill No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_Date_dt" HeaderText="Bill Date" DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_RecordType_var" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_RecordNo_int" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReferenceNo_int" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="GrossAmount" HeaderText="Gross Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_NetAmt_num" HeaderText="Bill Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_DiscountAmt_num" HeaderText="Discount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_SerTaxAmt_num" HeaderText="Service Tax" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_SwachhBharatTaxAmt_num" HeaderText="Swachh Bharat Cess"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_KisanKrishiTaxAmt_num" HeaderText="Krishi Kalyan Cess"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_CGSTAmt_num" HeaderText="CGST Amt" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_SGSTAmt_num" HeaderText="SGST Amt" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_IGSTAmt_num" HeaderText="IGST Amt" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_PaidAmount" HeaderText="Paid Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n}" />
                                <asp:BoundField DataField="BILL_PendingAmount" HeaderText="Pending Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n}" />
                                <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" />
                                <%--<asp:BoundField DataField="ProformaInvoiceNo" HeaderText="Proforma Invoice No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />--%>
                                <asp:BoundField DataField="CL_Id" HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SITE_Id" HeaderText="Site Id" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_CL_GSTNo_var" HeaderText="Client GSTIN" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_SITE_GSTNo_var" HeaderText="Site GSTIN" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CL_State_var" HeaderText="Client State" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SITE_State_var" HeaderText="Site State" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="RouteName" HeaderText="Route" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Section">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddl_Inward" Width="180px" runat="server">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblSectionId" Text='<%#Eval("Bill_SectionID_int") %>' runat="server" Visible="false" />
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPrintBill" runat="server" ToolTip="Print Bill" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Bill_Id")%>' CommandName="lnkPrintBill">&nbsp;Print&nbsp;</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkModifyBill" runat="server" ToolTip="Modify Bill" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Bill_Id") %>' CommandName="lnkModifyBill">&nbsp;Modify&nbsp;</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkViewRate" runat="server" ToolTip="View Rate" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Bill_Id") %>' CommandName="lnkViewRate">&nbsp;ViewRate&nbsp;</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkApproveBill" runat="server" ToolTip="Approve Bill" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Bill_Id") %>' CommandName="lnkApproveBill">&nbsp;Approve&nbsp;</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEmailBill" runat="server" ToolTip="Email Bill" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Bill_Id") %>' CommandName="lnkEmailBill">&nbsp;Email&nbsp;</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApproveStatus" runat="server" Text='<%# Bind("BILL_ApproveStatus_bit") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gst No">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkUpdateGst" runat="server" ToolTip="Update GstNo" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Bill_Id") %>' CommandName="lnkUpdateGst">&nbsp;Update&nbsp;</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BILL_Reason_var" HeaderText="Reason" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Upload PO">
                                    <ItemTemplate>
                                        <asp:FileUpload ID="FileUploadPO" runat="server" Width="100px"/>&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkUploadPO" runat="server" ToolTip="Upload PO" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Bill_Id") %>' CommandName="lnkUploadPO">&nbsp;Upload&nbsp;</asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Download PO">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDownloadFile" runat="server" Text='<%# Eval("BILL_POFileName_var") %>' ToolTip="Download File" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("BILL_POFileName_var")%>' CommandName="DownloadFile"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Download MOU">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDownloadMOUFile" runat="server" Text='<%# Eval("CL_MOUFileName_var") %>' ToolTip="Download File" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("CL_MOUFileName_var")%>' CommandName="DownloadMOUFile"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Client Approval">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClientApproveStatus" runat="server" Text='<%# Bind("BILL_ClientApproveStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Client Approval By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClientApprovedBy" runat="server" Text='<%# Bind("BILL_ClientApprovedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Client Approval Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClientApproveDate" runat="server" Text='<%# Eval("BILL_ClientApproveDate_dt","{0:dd/MM/yyyy hh:mm tt}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Proposal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkViewProposal" runat="server" Text='<%# Eval("Proposal_No") %>' ToolTip="View Proposal" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Proposal_EnqNo") + ";" + Eval("Proposal_Id") + ";" + Eval("Proposal_No") %>' CommandName="ViewProposal"></asp:LinkButton>                                        
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:GridView ID="grdTemp" runat="server" Visible="false">
                        </asp:GridView>
                    <%--</div>
                </div>--%>
            </asp:Panel>


            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                PopupControlID="pnlSiteWiseRateList" PopupDragHandleControlID="PopupHeader" Drag="true"
                BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlSiteWiseRateList" runat="server">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlList" runat="server" ScrollBars="Vertical">

                            <table class="DetailPopup" width="750px">
                                <tr valign="top">
                                    <td align="center" valign="bottom" colspan="6">
                                        <asp:ImageButton ID="imgCloseSitewiseRateListPopup" runat="server" ImageUrl="Images/cross_icon.png"
                                            OnClick="imgCloseSitewiseRateListPopup_Click" ImageAlign="Right" />
                                        <asp:Label ID="lblSitewiseRateList" runat="server" ForeColor="#990033" Text="Sitewise Rate List"
                                            Font-Bold="True" Font-Size="Small"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRecType" runat="server" Font-Bold="True" Text="Record Type"></asp:Label>&nbsp;&nbsp;
                                    
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRecordType" Width="180px" runat="server"
                                            OnSelectedIndexChanged="ddlRecordType_OnSelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        &nbsp; &nbsp;
                                    <asp:DropDownList ID="ddlOtherTest" Width="180px" runat="server" Visible="false">
                                    </asp:DropDownList>
                                        &nbsp; &nbsp;
                                    <asp:ImageButton ID="ImgBtnSearchRate" runat="server" ImageUrl="~/Images/Search-32.png"
                                        Style="width: 14px" OnClick="ImgBtnSearchRate_Click" />
                                        <asp:Label ID="lblBillId" runat="server" Text="0" Visible="false"></asp:Label>
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="pnlgrdList" runat="server" Width="100%" BorderWidth="0px" Height="390px"
                                            ScrollBars="Auto">
                                            <asp:UpdatePanel ID="UpdatePanelGrd" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="grdRate" runat="server" SkinID="gridviewSkin1" AutoGenerateColumns="False"
                                                        CellPadding="4" ForeColor="#333333" Width="100%">
                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <RowStyle BackColor="#EFF3FB" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sr. No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex + 1%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Test Id" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTestId" runat="server" Text='<%#Eval("Test_Id") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Rate Id" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRateId" runat="server" Text='<%#Eval("SITERATE_Id") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Record Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRecordType" runat="server" Text='<%#Eval("Test_RecType_var") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Test Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTestName" runat="server" Text='<%#Eval("TEST_Name_var") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="80px" Wrap="true" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Other Test Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOtherTestType" runat="server" Text='<%#Eval("otherTestType") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="80px" Wrap="true" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Criteria">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCriteria" runat="server" Text='<%# string.Concat(Eval("TEST_From_num"), " - ", Eval("TEST_To_num"))%>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Current Rate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRate" runat="server" Text='<%#Eval("TEST_Rate_int") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="50px" HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="New Rate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNewRate" runat="server" Text='<%#Eval("SITERATE_Test_Rate_int") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" Width="50px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No records to display
                                                        </EmptyDataTemplate>
                                                        <AlternatingRowStyle BackColor="White" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </td>
                                </tr>

                            </table>
                        </asp:Panel>
                    </ContentTemplate>

                </asp:UpdatePanel>
            </asp:Panel>

        </asp:Panel>
    </div>
    <%--<script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdModifyBill.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdModifyBill.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdModifyBill.ClientID%>').offset().top);

        });
    </script>--%>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
    </script>
    <%--<script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>--%>
</asp:Content>
