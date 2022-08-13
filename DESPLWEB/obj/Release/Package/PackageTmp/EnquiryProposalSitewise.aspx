<%@ Page Title="EnquiryProposal SiteWise" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="EnquiryProposalSitewise.aspx.cs" Inherits="DESPLWEB.EnquiryProposalSitewise"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .Initial
        {
            display: block;
            padding: 2px 15px 2px 15px;
            float: left;
            color: Gray;
            font-weight: bold;
        }
        .Initial:hover
        {
            color: White;
            background-color: #5D7B9D;
        }
        .Clicked
        {
            float: left;
            display: block;
            background-color: #e7f5fe;
            padding: 2px 15px 2px 15px;
            color: Black;
            font-weight: bold;
            color: Black;
        }
    </style>
    <div class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="1px" ScrollBars="Vertical"
            Height="480px" Style="background-color: #ffffff; font-family: Book Antiqua">
            <table style="width: 100%">
                <tr valign="top">
                    <td align="center" colspan="5">
                        <asp:Label ID="lblProposalId" runat="server" Visible="false" Text=""></asp:Label>
                        <asp:Label ID="lblPrvRecType" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblEnqType" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblEnqId" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblTempEnqId" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblModifiedType" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblSiteId" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblMonthlyStatus" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblRouteId" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblMEEmail" runat="server" Text="" Visible="false"></asp:Label>
                         <asp:Label ID="lblUserLevel" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            Visible="false" OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="#990033" Text="lblMsg" Visible="False"></asp:Label>
                    </td>
                    <td colspan="3" align="right">
                        <asp:Button ID="btn_Cal" runat="server" BackColor="#e7f5fe" Text="Cal" Width="60px"
                            OnClick="btn_Cal_Click" EnableViewState="False" Font-Names="Bookman Old Style" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btn_SaveEnquiryProposal" runat="server" BackColor="#e7f5fe" Text="Save"
                            Width="60px" OnClick="btn_SaveEnquiryProposal_Click" EnableViewState="False"
                            Font-Names="Bookman Old Style" CssClass="btn" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btn_Inward" Visible="false" BackColor="#e7f5fe" runat="server" Text="Inward"
                            Width="60px" OnClick="btn_Inward_Click" EnableViewState="False" Font-Names="Bookman Old Style" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btn_Exit" runat="server" BackColor="#e7f5fe" Text="Exit" Width="60px"
                            OnClick="btn_Exit_Click" EnableViewState="False" Font-Names="Bookman Old Style" />
                    </td>
                </tr>
                <tr valign="top" style="background-color: #ECF5FF">
                    <td colspan="6">
                        &nbsp; &nbsp; <b>
                            <asp:Label ID="Label42" runat="server" Text="Client Information"></asp:Label></b>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td style="width: 100%" colspan="6">
                        &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                        <asp:Label ID="lblEnqNo" runat="server" Text="" Font-Bold="true" Visible="false"></asp:Label>
                        &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                        &nbsp;&nbsp; &nbsp;
                        <asp:CheckBox ID="chkConifrmEnq" runat="server" Checked="true" AutoPostBack="true"
                            OnCheckedChanged="chkConifrmEnq_CheckedChanged" />&nbsp;
                        <asp:Label ID="Label13" runat="server" Text="Confirm Enquiry"></asp:Label>
                        <asp:CheckBox ID="chkWalkIn" runat="server" />&nbsp;
                        <asp:Label ID="Label41" runat="server" Text="Walking Customer"></asp:Label>
                        &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right" style="width: 15%">
                        <asp:Label ID="Label3" runat="server" Text="Site Name"></asp:Label>
                    </td>
                    <td style="width: 1%">
                        &nbsp;
                    </td>
                    <td style="width: 55%" colspan="3">

                     <asp:TextBox ID="txt_Site" runat="server" Width="350px" AutoPostBack="true" OnTextChanged="txt_Site_TextChanged"
                            Text=""></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetSitename" MinimumPrefixLength="0" OnClientItemSelected="SiteItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Site"
                            ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>

                     
                        <br />
                        &nbsp;
                    </td>
                    <td style="width: 33%">
                        <asp:Label ID="lblLogin" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                        <br />
                    </td>
                </tr>
               <%-- <tr valign="top" id="trAdd" runat="server" visible="false">
                    <td align="right">
                        <asp:Label ID="lblClientAddress" runat="server" Text="Client Address"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtClientAddress" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox>
                        &nbsp;
                        <br />
                        &nbsp;
                    </td>
                    <td colspan="2">
                    </td>
                </tr>--%>
                <tr valign="top">
                    <td align="right">
                        <asp:Label ID="Label4" runat="server" Text="Client"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                          <asp:TextBox ID="txt_Client" runat="server" Width="350px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                       <%-- <asp:CheckBox ID="chkNewClient" Text="New" runat="server" AutoPostBack="true"
                            oncheckedchanged="chkNewClient_CheckedChanged" />&nbsp;
                        <asp:LinkButton ID="lnkViewClientAddress" runat="server" OnClick="lnkViewClientAddress_Click"
                            Font-Names="Bookman Old Style" ToolTip="Click to view Client Office Address">View Address</asp:LinkButton>--%>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td align="right">
                        &nbsp;
                        <asp:Label ID="lblBalance" runat="server" Text="Outstanding "></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblBalanceAmt" runat="server" Text=""></asp:Label>
                        &nbsp;<br />
                        &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right">
                        <asp:Label ID="Label5" runat="server" Text="Site Address"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Site_address" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox>
                        <br />
                        &nbsp;
                    </td>
                    <td align="right">
                        <asp:Label ID="lblBelow90Balance" runat="server" Text="Below90Bal" Width="100px"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblBilling" runat="server" Text="Billing" Width="100px" Font-Bold="true"></asp:Label>
                        <br />
                        <asp:Label ID="lblClientType" runat="server" Text="Client" Width="100px" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblAbove90Balance" runat="server" Text="Above90Bal" Width="100px"
                            Visible="false"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblBillngStatus" runat="server" Text=""  Font-Bold="true"></asp:Label>&nbsp;
                        <asp:Label ID="lblBelow90BalanceAmt" runat="server" Text="" Visible="false"></asp:Label>&nbsp;
                      <br />
                         <asp:Label ID="lblClientStatus" runat="server" Text=""  Font-Bold="true"></asp:Label>&nbsp;
                        <asp:Label ID="lblAbove90BalanceAmt" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right">
                        <asp:Label ID="Label6" runat="server" Text="Landmark"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Site_LandMark" Width="350px" runat="server"></asp:TextBox>
                        <br />
                        &nbsp;
                    </td>
                    <td align="right">
                        <asp:Label ID="lblLimit" runat="server" Text="Credit Limit "></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblLimitAmt" runat="server" Text=""></asp:Label>
                        <br />
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right">
                        <asp:Label ID="Label7" runat="server" Text="Contact Person"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txt_ContactPerson" AutoPostBack="true" OnTextChanged="txt_ContactPerson_TextChanged"
                            Width="350px" runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetContactname" MinimumPrefixLength="0"
                            OnClientItemSelected="ContactItemSelected" CompletionInterval="10" EnableCaching="false"
                            CompletionSetCount="1" TargetControlID="txt_ContactPerson" ID="AutoCompleteExtender3"
                            runat="server" FirstRowSelected="false">
                        </asp:AutoCompleteExtender>
                        <br />
                        &nbsp;
                    </td>
                    <td align="right">
                        <asp:Label ID="lblDiscount" runat="server" Text="Discount"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblDiscountPer" runat="server" Text=""></asp:Label>
                        <br />
                        &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right">
                        <asp:Label ID="Label8" runat="server" Text="Contact No"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Contact_No" runat="server" MaxLength="100" Width="350px"></asp:TextBox>
                        <br />
                        &nbsp;
                    </td>
                    <td align="right">
                        <asp:Label ID="lblCoupon" runat="server" Text="Unused Coupons "></asp:Label>
                        <br />
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblCouponCount" runat="server" Text=""></asp:Label>
                        <br />
                        &nbsp;
                    </td>
                </tr>
                  <tr valign="top">
                    <td align="right">
                        <asp:Label ID="Label45" runat="server" Text="Email"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txt_EnqEmail" runat="server" MaxLength="100" Width="350px"></asp:TextBox>
                        <br />
                        &nbsp;
                    </td>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:HiddenField ID="hfSiteId" runat="server" />
                        <asp:HiddenField ID="hfContactId" runat="server" />
                        <asp:HiddenField ID="hfClientId" runat="server" />
                    </td>
                </tr>
                <tr valign="top" style="background-color: #ECF5FF">
                    <td colspan="6">
                        &nbsp; &nbsp;<b><asp:Label ID="lblMobEnq" runat="server" Text="Pending Enquiry List"></asp:Label></b>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="6">
                        <div id="divEnqPendList" runat="server" visible="false" style="border-style: solid;
                            border-color: inherit; border-width: thin; overflow: auto; width: 100%; height: 100px">
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdEnqList" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                        BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                                        Width="100%" CellPadding="0" CellSpacing="0" OnRowDataBound="grdEnqList_RowDataBound">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <asp:BoundField DataField="ENQ_Id" HeaderText="Enquiry No." HeaderStyle-HorizontalAlign="Center"
                                               ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ENQ_Date_dt" HeaderText="Enquiry Date" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="MATERIAL_Name_var" HeaderText="Test Type" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ENQ_OpenEnquiryStatus_var" HeaderText="Enquiry Type" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ENQ_Status_tint" HeaderText="Enquiry Status" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ENQ_ApproveDate_dt" HeaderText="Approved On" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="ENQ_CollectedOn_dt" HeaderText="Collected On" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="MATERIAL_RecordType_var" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ENQ_Note_var" HeaderText="Note" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ENQ_Reference_var" HeaderText="Reference" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ENQ_EnteredBy" HeaderText="Entered by" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                        <EmptyDataTemplate>
                                            No records to display
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"/>
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                    <asp:GridView ID="grdInwardType" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                        BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                                        Width="100%" CellPadding="0" CellSpacing="0">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Material Id" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaterialId" runat="server" Text='<%#Eval("material_id") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Material Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaterialName" runat="server" Text='<%#Eval("material_name") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaterialQuantity" runat="server" Text='<%#Eval("material_quantity") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                        <EmptyDataTemplate>
                                            No records to display
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="6">
                        &nbsp;
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF">
                    <td colspan="6">
                        &nbsp; &nbsp;<b><asp:Label ID="Label43" runat="server" Text="Enquiry Details"></asp:Label></b>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    </td>
                </tr>
                  <tr valign="top">
                   <td colspan="2">
                   &nbsp;
                   </td>
                    <td colspan="4">&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                        &nbsp;&nbsp; &nbsp;
                        <asp:RadioButton ID="rbOrderConfirm" runat="server" Text="Order Confirm"  AutoPostBack="true"
                            GroupName="R1" oncheckedchanged="rbOrderConfirm_CheckedChanged"/> &nbsp; &nbsp;
                        <asp:RadioButton ID="rbOrderLoss" runat="server"  Text="Order Loss" AutoPostBack="true"
                            GroupName="R1" oncheckedchanged="rbOrderLoss_CheckedChanged"/>
                        &nbsp; &nbsp;
                         <asp:RadioButton ID="rbDecisionPending" runat="server"  Text="Decision Pending" AutoPostBack="true"
                            GroupName="R1" oncheckedchanged="rbDecisionPending_CheckedChanged"/>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top">
                        Material Status
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="4">
                        <asp:Panel ID="panelMatCollStatus" runat="server" Height="80%" BorderWidth="0px">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div id="dvScroll" style="width: 750px; height: 70px;">
                                        <table id="tblDropDown" runat="server" style="width: 100%;" visible="true">
                                            <tr>
                                                <td colspan="3" valign="top">
                                                    <asp:DropDownList ID="ddl_MatCollectn" runat="server" Width="350px" OnSelectedIndexChanged="ddl_MatCollectn_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                        <asp:ListItem>----------------------------Select----------------------------</asp:ListItem>
                                                      <%--  <asp:ListItem>Material To be Collected</asp:ListItem>
                                                        <asp:ListItem>Already Collected</asp:ListItem>
                                                        <asp:ListItem>Decision Pending</asp:ListItem>
                                                        <asp:ListItem>Declined by us</asp:ListItem>
                                                        <asp:ListItem>Declined by Client</asp:ListItem>
                                                        <asp:ListItem>On site Testing</asp:ListItem>
                                                        <asp:ListItem>Delivered by Client</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblQty" runat="server" Visible="true" Text="Qty to be collected" onchange="checkNum(this)"></asp:Label>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_Qty" runat="server" Visible="true" MaxLength="5" Width="215px"
                                                        onchange="checkNum(this)"> </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="tblMatToBeCollected" runat="server" style="width: 100%;" visible="false">
                                            <tr>
                                                <td align="left" valign="top" style="width: 14%">
                                                    <asp:Label ID="Label1" runat="server" Text="Location"></asp:Label>
                                                </td>
                                                <%--  <td>
                                                    &nbsp;
                                                </td>--%>
                                                <td align="left" valign="top" style="width: 28%">
                                                    <asp:DropDownList ID="ddl_Location" runat="server" Width="256px" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddl_Location_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 10%">
                                                </td>
                                                <td colspan="2" style="width: 15%">
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="Label11" runat="server" Text="Route Name"></asp:Label>
                                                </td>
                                                <td style="width: 35%" valign="top">
                                                    <asp:DropDownList ID="ddlRouteName" runat="server" Width="215px" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlRouteName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" style="width: 14%">
                                                    <asp:Label ID="Label2" runat="server" Text="Collection Date"></asp:Label>
                                                </td>
                                                <%-- <td>
                                                    &nbsp;
                                                </td>--%>
                                                <td align="left" valign="top" style="width: 28%">
                                                    <asp:TextBox ID="txt_CollectionDate" runat="server" Width="251px" ReadOnly="true"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="false" FirstDayOfWeek="Sunday"
                                                        Format="dd/MM/yyyy" TargetControlID="txt_CollectionDate">
                                                    </asp:CalendarExtender>
                                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_CollectionDate"
                                                        MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                                                    </asp:MaskedEditExtender>
                                                </td>
                                                <td style="width: 10%">
                                                </td>
                                                <td align="left" colspan="2" style="width: 10%">
                                                    &nbsp;&nbsp;
                                                    <asp:CheckBox ID="chkbox_Urgent" runat="server" TextAlign="Right" AutoPostBack="True"
                                                        Text="Urgent" OnCheckedChanged="chkbox_Urgent_CheckedChanged" />
                                                </td>
                                                <td style="width: 35%" valign="top">
                                                    <asp:Label ID="Lbl_Client_Expected_Date" runat="server" Text="Client Expected Date"
                                                        Visible="false"></asp:Label>
                                                    &nbsp; &nbsp; &nbsp;
                                                    <asp:TextBox ID="txt_ClientExpected_Date" runat="server" Visible="false" Width="76px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                        Format="dd/MM/yyyy" TargetControlID="txt_ClientExpected_Date">
                                                    </asp:CalendarExtender>
                                                    <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ClientExpected_Date"
                                                        MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                                                    </asp:MaskedEditExtender>
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="tblAlredyCollected" runat="server" style="width: 100%;" visible="false">
                                            <tr>
                                                <td align="left" valign="top" style="width: 10%">
                                                    <asp:RadioButton ID="Rdn_AtLab" runat="server" GroupName="AlreadyCollected" AutoPostBack="true"
                                                        OnCheckedChanged="Rdn_AtLab_CheckedChanged" />
                                                    <asp:Label ID="Label24" runat="server" Text="At Lab"></asp:Label>
                                                </td>
                                                <td align="left" valign="top" colspan="2" style="width: 90%">
                                                    <asp:RadioButton ID="Rdn_ByDriver" runat="server" GroupName="AlreadyCollected" AutoPostBack="true"
                                                        OnCheckedChanged="Rdn_ByDriver_CheckedChanged" />
                                                    <asp:Label ID="Label25" runat="server" Text="By Driver"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trByDriver" visible="false">
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="left" valign="top" colspan="2">
                                                    &nbsp; &nbsp; &nbsp; &nbsp;
                                                    <asp:Label ID="lblDriverName" runat="server" Text="Driver Name"></asp:Label>
                                                    &nbsp;
                                                    <asp:DropDownList ID="ddl_DriverName" Width="170px" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="tblOther" runat="server" style="width: 100%;" visible="false">
                                            <tr runat="server" id="trDecisionPending" visible="false">
                                                <td align="left" valign="top" colspan="6">
                                                    <asp:Label ID="Label14" runat="server" Text="Comment"></asp:Label>
                                                    &nbsp;
                                                    <%--</td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>--%>
                                                    <asp:TextBox ID="txt_CommentDecisionPending" Text="" runat="server" Width="655px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trDeclinebyUs" visible="false">
                                                <td align="left" valign="top" colspan="6">
                                                    <asp:Label ID="Label15" runat="server" Text="Comment"></asp:Label>
                                                    &nbsp;
                                                    <%--  </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>--%>
                                                    <asp:TextBox ID="txt_CommentDeclinebyUs" runat="server" Width="655px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trDeclinedByClient" visible="false">
                                                <td align="left" valign="top" colspan="6">
                                                    <asp:CheckBox ID="chkRateDiiference" runat="server" AutoPostBack="true" OnCheckedChanged="chkRateDiiference_CheckedChanged" />
                                                    <asp:Label ID="Label12" runat="server" Text="Rate Difference"></asp:Label>
                                                    <%--     </td>
                                                <td>
                                                    &nbsp;
                                                </td> 
                                                <td>--%>
                                                    <asp:Label ID="Label16" runat="server" Text="Comment"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="txt_CommentRejectedByClient" runat="server" Width="546px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trOnsiteTesting" visible="false">
                                                <td align="left" valign="top" colspan="6">
                                                    <asp:Label ID="Label17" runat="server" Text="Test Date"></asp:Label>
                                                    &nbsp;
                                                    <%-- </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>--%>
                                                    <asp:TextBox ID="txt_OnsiteTesting_Date" runat="server" Width="285px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                        CssClass="orange" Format="dd/MM/yyyy" TargetControlID="txt_OnsiteTesting_Date">
                                                    </asp:CalendarExtender>
                                                    <asp:MaskedEditExtender ID="MaskedEditExtender4" TargetControlID="txt_OnsiteTesting_Date"
                                                        MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                                                    </asp:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trDeliveredbyClient" visible="false">
                                                <td align="left" valign="top" colspan="6">
                                                    <asp:Label ID="Label18" runat="server" Text="Date"></asp:Label>&nbsp;
                                                    <%-- </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>--%>
                                                    <asp:TextBox ID="txt_MaterialSendingOnDate" runat="server" Width="313px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                        CssClass="orange" Format="dd/MM/yyyy" TargetControlID="txt_MaterialSendingOnDate">
                                                    </asp:CalendarExtender>
                                                    <asp:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txt_MaterialSendingOnDate"
                                                        MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                                                    </asp:MaskedEditExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="6">
                        &nbsp;
                    </td>
                </tr>
                <tr valign="top" style="background-color: #ECF5FF">
                    <td colspan="6">
                        &nbsp; &nbsp;<b><asp:Label ID="Label44" runat="server" Text="Additional Information"></asp:Label></b>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right">
                        <asp:Label ID="Label34" runat="server" Text="Enquiry Status"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_EnqStatus" Width="350px" runat="server">
                            <asp:ListItem>----------------------------Select----------------------------</asp:ListItem>
                            <asp:ListItem>Hot</asp:ListItem>
                            <asp:ListItem>Cold</asp:ListItem>
                            <asp:ListItem>Warm</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        &nbsp;
                    </td>
                    <td align="right">
                        <asp:Label ID="Label35" runat="server" Text="Enquiry Type"></asp:Label>
                        <br />
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_EnqType" Width="215px" runat="server">
                            <asp:ListItem>-----------Select-----------</asp:ListItem>
                            <asp:ListItem>Auto</asp:ListItem>
                            <asp:ListItem>Generated By ME</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td valign="top" align="right">
                        <asp:Label ID="Label19" runat="server" Text="Note"></asp:Label><br />
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="txt_Note" runat="server" Width="720px"></asp:TextBox><br />
                        &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right">
                        <asp:Label ID="Label20" runat="server" Text="Reference"></asp:Label><br />
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="txt_Reference" runat="server" Width="720px"></asp:TextBox><br />
                        &nbsp;
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr valign="top" style="background-color: #ECF5FF">
                            <td colspan="6">
                                &nbsp; &nbsp;<b><asp:Label ID="Lbl44" runat="server" Text="Proposal"></asp:Label></b>
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="right" style="width: 15%">
                                <asp:Label ID="Label10" runat="server" Text="Proposal No."></asp:Label>
                            </td>
                            <td style="width: 1%">
                                &nbsp;
                            </td>
                            <td style="width: 45%">
                                <asp:TextBox ID="txt_ProposalNo" CssClass="ctextbox" Text="Create New..." ReadOnly="true"
                                    Width="350px" runat="server"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label21" runat="server" Text="Proposal Date"></asp:Label>
                                <br />
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txt_ProposalDt" Width="210px" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                    Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ProposalDt">
                                </asp:CalendarExtender>
                                <asp:MaskedEditExtender ID="MaskedEditExtender5" TargetControlID="txt_ProposalDt"
                                    MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                                </asp:MaskedEditExtender>
                                <br />
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="right">
                                <asp:Label ID="Label9" runat="server" Text="Inward Test Type"></asp:Label>
                                <br />
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="4">
                                <asp:DropDownList ID="ddl_InwardType" Width="350px" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddl_InwardType_SelectedIndexChanged">
                                </asp:DropDownList>
                                &nbsp;
                                <asp:DropDownList ID="ddlOtherTest" Width="200px" runat="server" AutoPostBack="true"
                                    Visible="false">
                                </asp:DropDownList>
                                &nbsp;
                                <asp:LinkButton ID="lnkAddInwardTestToGrid" OnClick="lnkAddInwardTestToGrid_Click"
                                    Font-Underline="true" runat="server">Add Inward Test</asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="lnkAddNewRowToGrid" OnClick="lnkAddNewRowToGrid_Click" Font-Underline="true"
                                    runat="server" Visible="false">Add New Row</asp:LinkButton>
                                <asp:Label ID="lblSelectedInward" runat="server" Text="" Visible="false"></asp:Label>
                                <br />
                                <asp:Label ID="lblProposalError" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="right">
                                <asp:Label ID="Label22" runat="server" Text="Kind Attention"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txt_KindAttention" Width="350px" runat="server"></asp:TextBox>
                                &nbsp;
                                <%--<asp:LinkButton ID="lnkEditAdd" OnClick="lnkEditClientAddress_Click" runat="server"
                                    Font-Underline="true">Edit Client Email</asp:LinkButton>--%>
                                <asp:Label ID="lbl10mDepthRate" runat="server" Text="" Visible="false"></asp:Label>
                                <br />
                                &nbsp;
                            </td>
                        </tr>
                         <tr valign="top">
                            <td align="right">
                                <asp:Label ID="Label46" runat="server" Text="Site Email"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="4">
                               <asp:TextBox ID="txtEmail" runat="server" MaxLength="250" Width="350px"></asp:TextBox>
                                 <br />
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td valign="top" align="right">
                                <asp:Label ID="Label27" runat="server" Text="Subject"></asp:Label>
                                <br />
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txt_Subject" Width="720px" runat="server"></asp:TextBox>
                                <br />
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td valign="top" align="right">
                                <asp:Label ID="Label26" runat="server" Text="Description"></asp:Label><br />
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txt_Description" TextMode="MultiLine" Width="720px" Height="45px"
                                    runat="server"></asp:TextBox>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="4">
                                <asp:CheckBox ID="chkMOUWorkOrder" runat="server" Text="MOU / Work Order Available">
                                </asp:CheckBox>
                            </td>
                        </tr>
                        <tr id="trLumsum">
                            <td valign="top" align="right">
                                <asp:Label ID="lblLumSum" runat="server" Text="Merge Rows" Visible="false"></asp:Label>
                                &nbsp;
                            </td>
                            <td colspan="3">
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="chkLums" OnCheckedChanged="chk_Lumshup_OnCheckedChanged" runat="server"
                                    Visible="false" AutoPostBack="true" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblFromRow" runat="server" Text="FromRow" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtFrmRow" runat="server" Width="35px" MaxLength="2" Visible="false"
                                    onchange="checkNum(this)"></asp:TextBox>&nbsp;&nbsp;
                                <asp:Label ID="lblToRow" runat="server" Text="ToRow" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtToRow" runat="server" Width="35px" MaxLength="2" Visible="false"
                                    onchange="checkNum(this)"></asp:TextBox>&nbsp;&nbsp;
                                <asp:Button ID="btnApplyLumpsum" Text="Apply" Visible="false" OnClick="btnApplyLumpsum_OnClick"
                                    runat="server" BackColor="#e7f5fe" Width="60px" Font-Names="Bookman Old Style"
                                    CssClass="btn" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkDepth" OnClick="lnkDepth_Click" Font-Underline="true" runat="server"
                                    Visible="false">Change Depth</asp:LinkButton>&nbsp;&nbsp;
                            </td>
                            <td align="right" colspan="2">
                                <asp:LinkButton ID="lnkClear" OnClick="lnkClear_Click" Font-Underline="true" runat="server">Clear</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="chkQty" runat="server" Checked="true" Text="Print With Qty." />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Panel ID="pnlStructAudit" runat="server" Width="100%" Visible="false">
                                    <table width="100%">
                                        <tr>
                                            <td width="18%">
                                                <asp:Label ID="lblStructNameOfApartSoc" runat="server" Text="Name of Apartment / Society"></asp:Label>
                                            </td>
                                            <td width="20%">
                                                <asp:TextBox ID="txtStructNameOfApartSoc" Width="200px" runat="server"></asp:TextBox>
                                            </td>
                                            <td width="25%">
                                                <asp:Label ID="lblStructAddress" runat="server" Text="Address"></asp:Label>
                                            </td>
                                            <td width="20%">
                                                <asp:TextBox ID="txtStructAddress" Text="" Width="200px" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblStructBuiltupArea" runat="server" Text="Builtup Area of Society"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStructBuiltupArea" Width="200px" runat="server" onchange="javascript:checkint(this);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblStructNoOfBuild" runat="server" Text="No of buildings in Society"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStructNoOfBuild" Text="" Width="200px" runat="server" onchange="javascript:checkint(this);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblStructAge" runat="server" Text="Age of Building"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStructAge" Width="200px" runat="server" onchange="javascript:checkint(this);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblStructConstWithin5Y" runat="server" Text="All buildings constructed with in 5 years range ?"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlStructConstWithin5Y" runat="server" Width="100px">
                                                    <asp:ListItem Text="-Select-"></asp:ListItem>
                                                    <asp:ListItem Text="Yes"></asp:ListItem>
                                                    <asp:ListItem Text="No"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblStructLocation" runat="server" Text="Location"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlStructLocation" runat="server" Width="200px">
                                                    <asp:ListItem Text="-Select-"></asp:ListItem>
                                                    <asp:ListItem Text="Coastal ( 1 km from Sea )"></asp:ListItem>
                                                    <asp:ListItem Text="Coastal  ( 5 Km from sea )"></asp:ListItem>
                                                    <asp:ListItem Text="Noncoastal"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAddLoadExpc" runat="server" Text="Any additional loads expected on building"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlStructAddLoadExpc" runat="server" Width="100px">
                                                    <asp:ListItem Text="-Select-"></asp:ListItem>
                                                    <asp:ListItem Text="Yes"></asp:ListItem>
                                                    <asp:ListItem Text="No"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblStructDistressObs" runat="server" Text="Any Distress Observed - Cracks wider than 1mm, more than 1 m length  and growing in last one year by 25% "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlStructDistressObs" runat="server" Width="100px">
                                                    <asp:ListItem Text="-Select-"></asp:ListItem>
                                                    <asp:ListItem Text="Yes"></asp:ListItem>
                                                    <asp:ListItem Text="No"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td colspan="6">
                                <asp:Label ID="lblDiscApplyToAll" runat="server" Text="Discount"></asp:Label>
                                &nbsp;&nbsp;
                                <asp:TextBox ID="txtDiscApplyToAll" Width="100px" runat="server" onchange="javascript:checkDecimal(this);"></asp:TextBox>
                                &nbsp;&nbsp;
                                <asp:CheckBox ID="chkDiscApplyToAll" runat="server" Checked="false" Text="Apply To All" />
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="lnkDiscApplyToAll" OnClick="lnkDiscApplyToAll_Click" Font-Underline="true"
                                    runat="server">Apply</asp:LinkButton>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td colspan="6">
                                <div id="divProposal" runat="server" style="border-style: solid; border-color: inherit;
                                    border-width: thin; overflow: auto; width: 100%; height: 200px">
                                    <asp:GridView ID="grdProposal" runat="server" OnRowDataBound="grdProposal_RowDataBound"
                                        AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#DEBA84" BorderWidth="1px"
                                        ForeColor="#333333" GridLines="Both" Width="100%" CellPadding="0" CellSpacing="0">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgEdit" runat="server" OnCommand="imgEdit_Click" ImageUrl="Images/Edit.jpg"
                                                        Height="18px" Width="18px" CausesValidation="false" ToolTip="Edit New Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" OnCommand="imgDelete_Click"
                                                        Height="19px" ImageUrl="Images/DeleteItem.png" ToolTip="Delete Row" Width="18px" />
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                                <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SNo.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Particular" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_Particular" BorderWidth="0px" Width="250px" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Test Method"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_TestMethod" BorderWidth="0px" Width="120px" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Unit Rate" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_Rate" BorderWidth="0px" Width="90px" Style="text-align: right"
                                                        ReadOnly="true" MaxLength="8" runat="server" onchange="javascript:checkDecimal(this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Discount(%)"
                                                ItemStyle-HorizontalAlign="Center" Visible="False">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_Discount" BorderWidth="0px" Width="100px" Style="text-align: right"
                                                        runat="server" onchange="javascript:checkDecimal(this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Discounted Rate"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_DiscRate" BorderWidth="0px" Width="100px" Style="text-align: right"
                                                        ReadOnly="true" runat="server" onchange="javascript:checkDecimal(this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Quantity" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_Quantity" Text="" BorderWidth="0px" Width="60px" Style="text-align: right"
                                                        runat="server" onchange="javascript:checkint(this);"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Amount" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_Amount" Text="" BorderWidth="0px" ReadOnly="true" Width="100px"
                                                        Style="text-align: right" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterStyle BackColor="White" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Inward Type"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_InwdType" runat="server" Text="" Width="75px"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="SiteRate" ItemStyle-HorizontalAlign="Center"
                                                Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_SiteWiseRate" runat="server" Text=""></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="TestId" ItemStyle-HorizontalAlign="Center"
                                                Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_TestId" runat="server" Text=""></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                        <EmptyDataTemplate>
                                            No records to display
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                    <asp:GridView ID="grdGT" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                        BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="none"
                                        Width="100%" CellPadding="0" CellSpacing="0">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField Visible="false"></asp:TemplateField>
                                            <asp:TemplateField Visible="false"></asp:TemplateField>
                                            <asp:TemplateField Visible="false"></asp:TemplateField>
                                            <asp:TemplateField Visible="false"></asp:TemplateField>
                                            <asp:TemplateField Visible="false"></asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDeleteGT" runat="server" CausesValidation="false" OnCommand="imgDeleteGT_Click"
                                                        Height="19px" ImageUrl="Images/DeleteItem.png" ToolTip="Delete Row" Width="18px" />
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                                <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SrNo.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                                <HeaderStyle Width="60px" />
                                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Particular" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_Particular" CssClass="textbox" Width="220px" runat="server"
                                                        Style="text-align: left"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="320px" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Test Method"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_TestMethod" BorderWidth="0px" Width="120px" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_Unit" CssClass="textbox" Width="100px" Style="text-align: center"
                                                        MaxLength="50" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_Quantity" Text="" CssClass="textbox" Width="80px" Style="text-align: Center"
                                                        MaxLength="4" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit Rate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_UnitRate" CssClass="textbox" Width="80px" Style="text-align: center"
                                                        onchange="javascript:checkDecimal(this);" ReadOnly="true" MaxLength="15" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Disc. Rate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_DiscRate" CssClass="textbox" Width="90px" Style="text-align: center"
                                                        onchange="javascript:checkDecimal(this);" MaxLength="15" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_Amount" Text="" CssClass="textbox" Width="100px" OnTextChanged="OnTextChanged"
                                                        AutoPostBack="true" ReadOnly="true" MaxLength="15" Style="text-align: center"
                                                        runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterStyle BackColor="White" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Inward Type" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_InwdType" runat="server" Text=""></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                        <EmptyDataTemplate>
                                            No records to display
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="right" colspan="6">
                                <asp:Label ID="Label38" runat="server" Text="Net Amount"></asp:Label>
                                &nbsp;&nbsp;&nbsp;
                                <asp:TextBox ID="txt_NetAmount" runat="server" ReadOnly="true" Style="text-align: right"
                                    Text="0.00" Width="200px"></asp:TextBox>
                                <asp:Label ID="lblGstAmt" runat="server" Text="0" Visible="false"></asp:Label>
                                <asp:Label ID="lblGrandTotal" runat="server" Text="0" Visible="false"></asp:Label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:CheckBox ID="chkGTDiscNote" runat="server" Checked="false" Visible="false" />
                                &nbsp;&nbsp;
                                <asp:Label ID="lblGTDiscNote" Visible="false" runat="server" Text="A discount of 5% of total billing will be given if 100% amount is paid as advance before start of work."></asp:Label>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td colspan="6">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                 <%--   <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="ModalPopupBG"
                        Drag="true" PopupControlID="pnlAddress" PopupDragHandleControlID="PopupHeader"
                        TargetControlID="HiddenField1">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnlAddress" runat="server" class="DetailPopup" Height="100px" Width="520px">
                        <table width="100%">
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="center" colspan="2">
                                    <asp:Label ID="Label23" runat="server" Font-Bold="True" ForeColor="#990033" Text="Edit Email"></asp:Label>
                                </td>
                                <td style="width: 10%">
                                    <asp:ImageButton ID="imgExitAddress" runat="server" ImageAlign="Right" ImageUrl="Images/cross_icon.png"
                                        OnClick="imgExitAddress_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Label ID="lbl_CL_Id" runat="server" Text="0" Visible="false"></asp:Label>
                                    <asp:Label ID="lbl_Site_Id" runat="server" Text="0" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 30px" valign="top">
                                    <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="250" Width="420px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lblMessage1" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>
                                </td>
                                <td align="right" colspan="2">
                                    <asp:LinkButton ID="lnkUpdateAddress" runat="server" Font-Underline="true" OnClick="lnkUpdateAddress_Click">Update Email</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>--%>
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" BackgroundCssClass="ModalPopupBG"
                        Drag="true" PopupControlID="PnlPerticularDtls" PopupDragHandleControlID="PopupHeader"
                        TargetControlID="HiddenField2">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="PnlPerticularDtls" runat="server" class="DetailPopup" Height="150px"
                        Width="420px">
                        <table width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="Label28" runat="server" Font-Bold="True" ForeColor="#990033" Text="Add/Edit Particular Details"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:ImageButton ID="imgExitFromNewPerticular" runat="server" ImageAlign="Right"
                                        ImageUrl="Images/cross_icon.png" OnClick="imgExitFromNewPerticular_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    <asp:Label ID="lblFlag" runat="server" Text="0" Visible="false"></asp:Label>
                                    <asp:Label ID="lblRecType" runat="server" Text="" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 30px" width="30%">
                                    <asp:Label ID="Label29" runat="server" Text="Particular Name"></asp:Label>
                                </td>
                                <td width="50%">
                                    <asp:TextBox ID="txt_PerticularName" runat="server" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 30px" width="30%">
                                    <asp:Label ID="Label31" runat="server" Text="Unit Rate"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_UnitRate" runat="server" MaxLength="10" onchange="checkDecimal(this);"
                                        Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 30px" width="30%">
                                    <asp:Label ID="Label32" runat="server" Text="Discounted Rate"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_DiscountRate" runat="server" MaxLength="10" onchange="checkDecimal(this);"
                                        Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:LinkButton ID="lnkAddNewPerticularToGrid" runat="server" Font-Underline="true"
                                        OnClick="lnkAddNewPerticularToGrid_Click">Add Particular</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:HiddenField ID="HiddenField3" runat="server" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" BackgroundCssClass="ModalPopupBG"
                        Drag="true" PopupControlID="PanelDepth" PopupDragHandleControlID="PopupHeader"
                        TargetControlID="HiddenField3">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="PanelDepth" runat="server" class="DetailPopup" Height="100px" Width="370px">
                        <table width="100%">
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="2" align="center">
                                    <asp:Label ID="Label33" runat="server" Font-Bold="True" ForeColor="#990033" Text="Edit Depth Rate"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:ImageButton ID="imgExitDepth" runat="server" ImageAlign="Right" ImageUrl="Images/cross_icon.png"
                                        OnClick="imgExitDepth_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 10px;" valign="top">
                                    <asp:Label ID="lblDepth" runat="server" Text="Select Depth"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlDepth" Width="205px" runat="server">
                                        <asp:ListItem>10 m</asp:ListItem>
                                        <asp:ListItem>15 m</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 10px" valign="top">
                                    <asp:Label ID="lblRate" runat="server" Text="Rate"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtGtRate" runat="server" Width="200px" MaxLength="8" onchange="checkDecimal(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lblGtRate" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>
                                </td>
                                <td align="right" colspan="2">
                                    <asp:LinkButton ID="lnkUpdateDepthRate" runat="server" Font-Underline="true" OnClick="lnkUpdateDepthRate_Click">Update Rate</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <table style="width: 100%">
                <tr valign="top" style="background-color: #ECF5FF">
                    <td colspan="6">
                        &nbsp; &nbsp;<b><asp:Label ID="Lbl46" runat="server" Text="Proposal - Other Details"></asp:Label></b>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="6">
                        <div id="divProposal_Other" runat="server" style="border-style: solid; border-color: inherit;
                            border-width: thin; overflow: auto; width: 100%; height: 180px">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <table align="center" width="99%">
                                        <tr>
                                            <td>
                                                <asp:Button ID="Tab_Notes" runat="server" BorderStyle="None" CssClass="Initial" Height="20px"
                                                    OnClick="Tab_Notes_Click" Text="Notes" />
                                                <asp:Button ID="Tab_Discount" runat="server" BorderStyle="None" CssClass="Initial"
                                                    Height="20px" OnClick="Tab_Discount_Click" Text="Discount Details" />
                                                <asp:Button ID="Tab_GenericDiscount" runat="server" BorderStyle="None" CssClass="Initial"
                                                    Height="20px" OnClick="Tab_GenericDiscount_Click" Text="Generic Discount Details" />
                                                <asp:MultiView ID="MainView_Proposal" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="View_Notes" runat="server">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="PanNotes" runat="server" BorderColor="AliceBlue" BorderStyle="Ridge"
                                                                    ScrollBars="Vertical" Width="100%">
                                                                    <asp:GridView ID="Grd_Note" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                                                        BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                                                                        Width="100%" CellPadding="0" CellSpacing="0">
                                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgBtnAddRowNote" runat="server" CausesValidation="false" Height="18px"
                                                                                        ImageUrl="Images/AddNewitem.jpg" OnCommand="imgBtnAddRowNote_Click" ToolTip="Add Row"
                                                                                        Width="18px" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="18px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgBtnDeleteRowNote" runat="server" CausesValidation="false"
                                                                                        Height="16px" ImageUrl="Images/DeleteItem.png" OnCommand="imgBtnDeleteRowNote_Click"
                                                                                        ToolTip="Delete Row" Width="16px" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="16px" />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="lblSrNo" HeaderStyle-HorizontalAlign="Center" HeaderText="Sr.No"
                                                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Note" ItemStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txt_NOTE" runat="server" BorderWidth="0px" Text="" Width="99%"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <EmptyDataTemplate>
                                                                            No records to display
                                                                        </EmptyDataTemplate>
                                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                        <EditRowStyle BackColor="#999999" />
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                    </asp:GridView>
                                                                    <br />
                                                                    <asp:Label ID="lblAddChargesGT" runat="server" Text="Additional Charges" Visible="false"></asp:Label>
                                                                    <asp:GridView ID="Grd_NoteGT" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                                                        BorderColor="#DEBA84" BorderWidth="1px" Visible="false" ForeColor="#333333" GridLines="Both"
                                                                        Width="100%" CellPadding="0" CellSpacing="0">
                                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgBtnAddNotesRowGT" runat="server" CausesValidation="false"
                                                                                        Height="18px" ImageUrl="Images/AddNewitem.jpg" OnCommand="imgBtnAddNotesRowGT_Click"
                                                                                        ToolTip="Add Row" Width="18px" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="18px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgBtnDeleteNotesRowGT" runat="server" CausesValidation="false"
                                                                                        Height="16px" ImageUrl="Images/DeleteItem.png" OnCommand="imgBtnDeleteNotesRowGT_Click"
                                                                                        ToolTip="Delete Row" Width="16px" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="16px" />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="lblSrNoGT" HeaderStyle-HorizontalAlign="Center" HeaderText="Sr.No"
                                                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Note" ItemStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txt_NOTEGT" runat="server" BorderWidth="0px" Text="" Width="99%"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <EmptyDataTemplate>
                                                                            No records to display
                                                                        </EmptyDataTemplate>
                                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                        <EditRowStyle BackColor="#999999" />
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                    </asp:GridView>
                                                                    <br />
                                                                    <asp:Label ID="lblPayTermsGT" runat="server" Text="Payment Terms" Visible="false"></asp:Label>
                                                                    <asp:GridView ID="grdPayTermsGT" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                                                        BorderColor="#DEBA84" BorderWidth="1px" Visible="false" ForeColor="#333333" GridLines="Both"
                                                                        Width="100%" CellPadding="0" CellSpacing="0">
                                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgBtnAddRowPayTermGT" runat="server" CausesValidation="false"
                                                                                        Height="18px" ImageUrl="Images/AddNewitem.jpg" OnCommand="imgBtnAddRowPayTermGT_Click"
                                                                                        ToolTip="Add Row" Width="18px" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="18px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgBtnDeleteRowPayTermGT" runat="server" CausesValidation="false"
                                                                                        Height="16px" ImageUrl="Images/DeleteItem.png" OnCommand="imgBtnDeleteRowPayTermGT_Click"
                                                                                        ToolTip="Delete Row" Width="16px" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="16px" />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="lblSrNoPayTermGT" HeaderStyle-HorizontalAlign="Center" HeaderText="Sr.No"
                                                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Note" ItemStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtNotePayTermGT" runat="server" BorderWidth="0px" Text="" Width="99%"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <EmptyDataTemplate>
                                                                            No records to display
                                                                        </EmptyDataTemplate>
                                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                        <EditRowStyle BackColor="#999999" />
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                    </asp:GridView>
                                                                    <br />
                                                                    <asp:Label ID="lblClientScope" runat="server" Text="Client Scope" Visible="false"></asp:Label>
                                                                    <asp:GridView ID="grdClientScope" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                                                        BorderColor="#DEBA84" BorderWidth="1px" Visible="false" ForeColor="#333333" GridLines="Both"
                                                                        Width="100%" CellPadding="0" CellSpacing="0">
                                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgBtnAddClientScopeRow" runat="server" CausesValidation="false"
                                                                                        Height="18px" ImageUrl="Images/AddNewitem.jpg" OnCommand="imgBtnAddClientScopeRow_Click"
                                                                                        ToolTip="Add Row" Width="18px" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="18px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgBtnDeleteClientScopeRow" runat="server" CausesValidation="false"
                                                                                        Height="16px" ImageUrl="Images/DeleteItem.png" OnCommand="imgBtnDeleteClientScopeRow_Click"
                                                                                        ToolTip="Delete Row" Width="16px" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="16px" />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="lblSrNoClientScope" HeaderStyle-HorizontalAlign="Center"
                                                                                HeaderText="Sr.No" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Note" ItemStyle-HorizontalAlign="Left">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txt_NOTEClientScope" runat="server" BorderWidth="0px" Text="" Width="99%"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <EmptyDataTemplate>
                                                                            No records to display
                                                                        </EmptyDataTemplate>
                                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                        <EditRowStyle BackColor="#999999" />
                                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </asp:View>
                                                    <asp:View ID="View_Discount" runat="server">
                                                        <asp:Panel ID="Pnl_Discount" runat="server" BorderColor="AliceBlue" BorderStyle="Ridge"
                                                            Width="100%">
                                                            <asp:GridView ID="grdDiscount" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                                                BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                                                                Width="100%" CellPadding="0" CellSpacing="0">
                                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Material Name"
                                                                        ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMaterialName" runat="server" Text="" Width="150px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Test Name" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTestName" runat="server" Text="" Width="250px"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Discounted Rate&lt;Br/&gt;SiteWise(%)"
                                                                        ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtSiteWiseDisc" runat="server" BorderWidth="0px" Style="text-align: right"
                                                                                Text="" Width="80px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Generic Disc.(%)"
                                                                        ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtVolDisc" runat="server" BorderWidth="0px" Style="text-align: right"
                                                                                Text="" Width="50px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Applied Disc. %"
                                                                        ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtAppliedDisc" runat="server" BorderWidth="0px" Style="text-align: right"
                                                                                Text="" Width="50px"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    No records to display
                                                                </EmptyDataTemplate>
                                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                <EditRowStyle BackColor="#999999" />
                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </asp:View>
                                                    <asp:View ID="View_GenericDiscount" runat="server">
                                                        <asp:Panel ID="pnlGenericDiscount" runat="server" BorderColor="AliceBlue" BorderStyle="Ridge"
                                                            ScrollBars="Vertical" Width="100%">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td colspan="6">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblVol" runat="server" Text="Volumn (%) : 0"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblTime" runat="server" Text="Timely Payment (%) : 0"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblAdv" runat="server" Text="Advance (%) : 0"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblLoy" runat="server" Text="Loyalty (%) : 0"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblProp" runat="server" Text="Proposal (%) : 0"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblApp" runat="server" Text="App. (%) : 0"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblCalcDisc" runat="server" Text="Calculated (%) : 0"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblDisc" runat="server" Font-Bold="true" Text="Applied Discount(%) : 0"></asp:Label>
                                                                        <asp:Label ID="lblMax" runat="server" Text="Max (%) : 0" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblIntro" runat="server" Text="Introductory "></asp:Label>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:TextBox ID="txtIntro" runat="server" MaxLength="2" onchange="javascript:checkint(this);"
                                                                            Text="0.00" Width="100px"></asp:TextBox>
                                                                        <asp:LinkButton ID="lnkUpdateIntro" runat="server" Font-Underline="true" OnClick="lnkUpdateIntro_Click">Update Introductory Discount</asp:LinkButton>
                                                                        <asp:Label ID="lblClId" runat="server" Text="" Visible="false"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </asp:View>
                                                    <asp:View ID="View_InwardType" runat="server">
                                                        <asp:Panel ID="pnlInwardType" runat="server" BorderColor="AliceBlue" BorderStyle="Ridge"
                                                            Width="100%">
                                                        </asp:Panel>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <%--   <tr valign="top" style="background-color: #ECF5FF">
                            <td colspan="6">
                                &nbsp; &nbsp;<b>Proposal - Client Scope</b> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            </td>
                        </tr>
                       <tr id="trCLScope" valign="top" runat="server" visible="false"> 
                            <td colspan="6">
                                <div id="divCLScope" runat="server" style="border-style: solid; border-color: inherit;
                                    border-width: thin; overflow: auto; width: 100%; height: 180px">
                                    <table align="center" width="99%">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlCLScope" runat="server" BorderColor="AliceBlue" BorderStyle="Ridge"
                                                    ScrollBars="Vertical" Width="100%">
                                                    <asp:GridView ID="grdCLScope" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                                        BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                                                        Width="100%" CellPadding="0" CellSpacing="0">
                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnAddCLScopeRow" runat="server" CausesValidation="false"
                                                                        Height="18px" ImageUrl="Images/AddNewitem.jpg" OnCommand="imgBtnAddNotesRowGT_Click"
                                                                        ToolTip="Add Row" Width="18px" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="18px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnDeleteCLScopeRow" runat="server" CausesValidation="false"
                                                                        Height="16px" ImageUrl="Images/DeleteItem.png" OnCommand="imgBtnDeleteNotesRowGT_Click"
                                                                        ToolTip="Delete Row" Width="16px" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="16px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="lblSrNoCLScope" HeaderStyle-HorizontalAlign="Center" HeaderText="Sr.No"
                                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Note" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_NOTECLScope" runat="server" BorderWidth="0px" Text="" Width="99%"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            No records to display
                                                        </EmptyDataTemplate>
                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                        <EditRowStyle BackColor="#999999" />
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                &nbsp;
                            </td>
                        </tr>--%>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr valign="top">
                            <td align="right">
                                <asp:Label ID="lblPaymentTerm" runat="server" Text="Payment term"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtPaymentTerm" runat="server" MaxLength="500" Width="715px"></asp:TextBox>
                                <br />
                                &nbsp;
                            </td>                            
                        </tr>
                        <tr valign="top">
                            <td align="right" style="width: 15%">
                                <asp:Label ID="Label36" runat="server" Text="Proposal By"></asp:Label>
                            </td>
                            <td style="width: 1%">
                                &nbsp;
                            </td>
                            <td style="width: 45%">
                                <asp:DropDownList ID="ddl_PrposalBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_PrposalBy_SelectedIndexChanged"
                                    Width="350px">
                                </asp:DropDownList>
                                <br />
                                &nbsp;
                            </td>
                            <td align="right">
                                <asp:Label ID="Label37" runat="server" Text="Contact No"></asp:Label>
                                <br />
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txt_ContactNo" runat="server" MaxLength="15" Width="215px"></asp:TextBox>
                                <br />
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="right">
                                <asp:Label ID="Label39" runat="server" Text="ME Name"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtMe" runat="server" MaxLength="250" Width="350px"></asp:TextBox>
                                <br />
                                &nbsp;
                            </td>
                            <td align="right">
                                <asp:Label ID="Label40" runat="server" Text="Contact No"></asp:Label>
                                <br />
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtMeNum" runat="server" MaxLength="15" Width="215px"></asp:TextBox>
                                <br />
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="right">
                                <asp:Label ID="Label30" runat="server" Text="Mail CC"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtMailCC" runat="server" Width="720px"></asp:TextBox>
                                <br />
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="4">
                                <asp:CheckBox ID="chkPropSendForAppr" runat="server" Text="Proposal Send for Approval">
                                </asp:CheckBox>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <table style="width: 100%">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="3" align="right">
                        <asp:Button ID="btn_Cal1" runat="server" BackColor="#e7f5fe" Text="Cal" Width="60px"
                            OnClick="btn_Cal_Click" EnableViewState="false" Font-Names="Bookman Old Style" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btn_SaveEnquiryProposal1" runat="server" BackColor="#e7f5fe" Text="Save"
                            Width="60px" OnClick="btn_SaveEnquiryProposal_Click" EnableViewState="False"
                            Font-Names="Bookman Old Style" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btn_Inward1" Visible="false" runat="server" BackColor="#e7f5fe" Text="Inward"
                            Width="60px" OnClick="btn_Inward_Click" EnableViewState="False" Font-Names="Bookman Old Style" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btn_Exit1" BackColor="#e7f5fe" runat="server" Text="Exit" Width="60px"
                            OnClick="btn_Exit_Click" EnableViewState="False" Font-Names="Bookman Old Style" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.11.0.min.js"></script>
    <script type="text/javascript">
        //Ensure that you place the Javascript code after the script manager
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            xPos = $get('dvScroll').scrollLeft;
            yPos = $get('dvScroll').scrollTop;
        }
        function EndRequestHandler(sender, args) {
            $get('dvScroll').scrollLeft = xPos;
            $get('dvScroll').scrollTop = yPos;
        }
    </script>
    <script type="text/javascript">
        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblProposalError.ClientID %>").style.display = "none";
            }, seconds * 500);
        };
    </script>
    <script type="text/javascript">
        function checkNum(x) {
            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    x.value = '';
                    x.focus();

                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Numeric Values Allowed";
                    return false;
                }
                else {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                }

            }
            return true;
        }

        function checkDecimal(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 46))) {
                    x.value = '';
                    x.focus();
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Decimal Values Allowed";
                    return false;
                }
                else {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                }

            }
            return true;
        }

        function checkint(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    x.value = '';
                    x.focus();

                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Numeric Values Allowed";
                    return false;
                }
                else {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                }

            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
        function SiteItemSelected(sender, e) {
            $get("<%=hfSiteId.ClientID %>").value = e.get_value();
        }
        function ContactItemSelected(sender, e) {
            $get("<%=hfContactId.ClientID %>").value = e.get_value();
        }

    </script>
</asp:Content>
