<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MstPg_Veena.Master"
    Title="Enquiry" CodeBehind="Enquiry.aspx.cs" Inherits="DESPLWEB.Enquiry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .Initial
        {
            display: block;
            padding: 4px 18px 4px 18px;
            float: left;
            color: Gray;
            font-weight: bold;
        }
        .Initial:hover
        {
            color: White;
            background-color: Olive;
        }
        .Clicked
        {
            float: left;
            display: block;
            background-color: #9FB7E8;
            padding: 4px 18px 4px 18px;
            color: Black;
            font-weight: bold;
            color: Maroon;
        }
    </style>
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table width="100%" style="border-color: #800000">
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Client"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Client" runat="server" Width="420px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:CheckBox ID="chkNewClient" Text="New" runat="server" />&nbsp;&nbsp;
                        <asp:Label ID="lblEnqId" runat="server" Text="" Visible="false"></asp:Label>&nbsp;&nbsp;
                        <asp:Label ID="lblEnqType" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblTempEnqId" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:LinkButton ID="lnkViewClientAddress" runat="server" OnClick="lnkViewClientAddress_Click"
                            ToolTip="Click to view Client Office Address">View Address</asp:LinkButton>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                    </td>
                    <%-- <td>
                        <asp:Label ID="lblBalance" runat="server" Text="Balance " Width="60px"></asp:Label>                        
                    </td>
                    <td>
                        <asp:Label ID="lblBalanceAmt" runat="server" Text=""></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>--%>
                    <td colspan="2">
                        <asp:Label ID="lblLogin" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClientAddress" runat="server" Text="Client Address" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtClientAddress" runat="server" Width="420px" TextMode="MultiLine"
                            Visible="false"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Site Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Site" runat="server" Width="420px" AutoPostBack="true" OnTextChanged="txt_Site_TextChanged"
                            Text=""></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetSitename" MinimumPrefixLength="0" OnClientItemSelected="SiteItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Site"
                            ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblBalance" runat="server" Text="Outstanding " Width="60px"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblBalanceAmt" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Site Address"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Site_address" runat="server" Width="420px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblBelow90Balance" runat="server" Text="Below 90 Balance" Width="100px" Visible="false"></asp:Label>&nbsp;<br />
                        <br />
                        <asp:Label ID="lblAbove90Balance" runat="server" Text="Above 90 Balance" Width="100px" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblBelow90BalanceAmt" runat="server" Text="" Visible="false"></asp:Label>&nbsp;<br />
                        <br />
                        <asp:Label ID="lblAbove90BalanceAmt" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="Landmark"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Site_LandMark" Width="420px" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblLimit" runat="server" Text="Credit Limit " ></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblLimitAmt" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="Contact Person"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_ContactPerson" AutoPostBack="true" OnTextChanged="txt_ContactPerson_TextChanged"
                            Width="420px" runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetContactname" MinimumPrefixLength="0"
                            OnClientItemSelected="ContactItemSelected" CompletionInterval="10" EnableCaching="false"
                            CompletionSetCount="1" TargetControlID="txt_ContactPerson" ID="AutoCompleteExtender3"
                            runat="server" FirstRowSelected="false">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblDiscount" runat="server" Text="Discount"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDiscountPer" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text="Contact No"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_ContactNo" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblCoupon" runat="server" Text="No. of Coupons " Width="90px"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCouponCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label9" runat="server" Text="Test Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_InwardType" Width="425px" runat="server">
                        </asp:DropDownList>
                        <asp:Label ID="lblSelectedInward" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label22" runat="server" Text="Quantity to be collected"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Quantity" runat="server" MaxLength="5" Width="140px" onchange="checkNum(this)"> </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label10" runat="server" Font-Bold="True" Text="Enquiry Status"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:HiddenField ID="hfSiteId" runat="server" />
                        <asp:HiddenField ID="hfContactId" runat="server" />
                    </td>
                </tr>
            </table>
            <table width="100%" align="center">
                <tr>
                    <td>
                        <asp:Button Text="Material To be Collected" BorderStyle="None" ID="Tab_To_be_Collected"
                            CssClass="Initial" runat="server" OnClick="Tab_To_be_Collected_Click" />
                        <asp:Button Text="Already Collected" BorderStyle="None" ID="Tab_Already_Collected"
                            CssClass="Initial" runat="server" OnClick="Tab_Already_Collected_Click" />
                        <asp:Button Text="Others" BorderStyle="None" ID="Tab_Others" CssClass="Initial" runat="server"
                            OnClick="Tab_Others_Click" />
                        <asp:Button Text="Test Type" BorderStyle="None" ID="Tab_TestType" CssClass="Initial"
                            runat="server" OnClick="Tab_TestType_Click" Visible="false" />
                        <asp:MultiView ID="MainView_EnquiryStatus" runat="server">
                            <asp:View ID="View_TobeCollected" runat="server">
                                <table style="width: 100%; border-width: 1px; border-color: #9FB7E8; border-style: solid;
                                    height: 135px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Location"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_Location" runat="server" Width="300px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_Location_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label11" runat="server" Text="Route Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlRouteName" runat="server" Width="300px" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlRouteName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Collection Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_CollectionDate" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="false" FirstDayOfWeek="Sunday"
                                                Format="dd/MM/yyyy" TargetControlID="txt_CollectionDate">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_CollectionDate"
                                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                                            </asp:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:CheckBox ID="chkbox_Urgent" runat="server" TextAlign="Right" AutoPostBack="True"
                                                OnCheckedChanged="chkbox_Urgent_CheckedChanged" />
                                            <span>Urgent</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="Lbl_Client_Expected_Date" runat="server" Text="Client Expected Date"
                                                Visible="false"></asp:Label>
                                            &nbsp; &nbsp; &nbsp;
                                            <asp:TextBox ID="txt_ClientExpected_Date" runat="server" Visible="false" Width="80px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                Format="dd/MM/yyyy" TargetControlID="txt_ClientExpected_Date">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ClientExpected_Date"
                                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                                            </asp:MaskedEditExtender>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View_AlreadyCollected" runat="server">
                                <table style="width: 100%; border-width: 1px; border-color: #9FB7E8; border-style: solid;
                                    height: 135px;">
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="Rdn_AtLab" runat="server" GroupName="AlreadyCollected" AutoPostBack="true"
                                                OnCheckedChanged="Rdn_AtLab_CheckedChanged" />
                                            <asp:Label ID="Label24" runat="server" Text="At Lab"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="Rdn_ByDriver" runat="server" GroupName="AlreadyCollected" AutoPostBack="true"
                                                OnCheckedChanged="Rdn_ByDriver_CheckedChanged" />
                                            <asp:Label ID="Label25" runat="server" Text="By Driver"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label13" runat="server" Text="Driver Name"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddl_DriverName" Enabled="false" Width="300px" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View_Others" runat="server">
                                <table style="width: 100%; border-width: 1px; border-color: #9FB7E8; border-style: solid">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="Rdn_DecisionPending" runat="server" GroupName="Other" AutoPostBack="true"
                                                OnCheckedChanged="Rdn_DecisionPending_CheckedChanged" />
                                            <asp:Label ID="Label26" runat="server" Text="Decision Pending"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" Text="Comment"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_CommentDecisionPending" Text="" Enabled="false" runat="server"
                                                Width="420px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="Rdn_DeclienedbyUs" runat="server" GroupName="Other" AutoPostBack="true"
                                                OnCheckedChanged="Rdn_DeclienedbyUs_CheckedChanged" />
                                            <asp:Label ID="Label27" runat="server" Text="Declined by us"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label15" runat="server" Text="Comment"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_CommentDeclinebyUs" Enabled="false" runat="server" Width="420px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="Rdn_RejectedByClient" runat="server" GroupName="Other" AutoPostBack="true"
                                                OnCheckedChanged="Rdn_RejectedByClient_CheckedChanged" />
                                            <%--<asp:Label ID="Label28" runat="server" Text="Rejected By Client"></asp:Label>--%>
                                            <asp:Label ID="Label28" runat="server" Text="Declined By Client"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkRateDiiference" runat="server" AutoPostBack="true" OnCheckedChanged="chkRateDiiference_CheckedChanged" />
                                            <asp:Label ID="Label12" runat="server" Text="Rate Difference"></asp:Label>&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="Label16" runat="server" Text="Comment"></asp:Label>&nbsp;&nbsp;&nbsp;
                                            <asp:TextBox ID="txt_CommentRejectedByClient" Enabled="false" runat="server" Width="420px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="Rdn_OnsiteTesting" runat="server" GroupName="Other" AutoPostBack="true"
                                                OnCheckedChanged="Rdn_OnsiteTesting_CheckedChanged" />
                                            <asp:Label ID="Label29" runat="server" Text="On site Testing"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label17" runat="server" Text="Test Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_OnsiteTesting_Date" Enabled="false" runat="server" Width="80px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                CssClass="orange" Format="dd/MM/yyyy" TargetControlID="txt_OnsiteTesting_Date">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender4" TargetControlID="txt_OnsiteTesting_Date"
                                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                                            </asp:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="Rdn_MaterialSendingOn_Date" runat="server" GroupName="Other"
                                                AutoPostBack="true" OnCheckedChanged="Rdn_MaterialSendingOn_Date_CheckedChanged" />
                                            <%--<asp:Label ID="Label30" runat="server" Text="Material Sending On Date"></asp:Label>--%>
                                            <asp:Label ID="Label30" runat="server" Text="Delivered by Client"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" Text="Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_MaterialSendingOnDate" Enabled="false" runat="server" Width="80px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                CssClass="orange" Format="dd/MM/yyyy" TargetControlID="txt_MaterialSendingOnDate">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txt_MaterialSendingOnDate"
                                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                                            </asp:MaskedEditExtender>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View_InwardType" runat="server">
                                <table style="width: 100%; border-width: 1px; border-color: #9FB7E8; border-style: solid">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlInwardType" runat="server" Height="150px" ScrollBars="Auto" Width="99%"
                                                BorderStyle="Solid" BorderWidth="1px">
                                                <asp:GridView ID="grdInwardType" runat="server" AutoGenerateColumns="False" CellPadding="2"
                                                    ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="#F7F6F3"
                                                    CssClass="Grid" BorderWidth="1px" Width="70%">
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
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label19" runat="server" Text="Note"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Note" runat="server" Width="600px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label20" runat="server" Text="Reference"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Reference" runat="server" Width="600px"></asp:TextBox>
                    </td>
                    <td style="width: 20%" align="left">
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSaveEnquiry" runat="server" CssClass="LnkOver" OnClick="lnkSaveEnquiry_Click"
                            Style="text-decoration: underline; font-weight: bold;" OnClientClick="return validateEnquiry();">Save</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnk_Inward" Visible="false" runat="server" CssClass="LnkOver"
                            OnClick="lnk_Inward_Click" Style="text-decoration: underline; font-weight: bold;">Inward</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnl_ViewPrevEnq" runat="server" CssClass="LnkOver"
                            OnClick="lnl_ViewPrevEnq_Click" Style="text-decoration: underline; font-weight: bold;" >View Prev Enq</asp:LinkButton>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnk_Exit" runat="server" CssClass="LnkOver" OnClick="lnk_Exit_Click"
                            Style="text-decoration: underline; font-weight: bold;">Exit</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                            PopupControlID="pnlPrevEnq" PopupDragHandleControlID="PopupHeader" Drag="true"
                            BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="pnlPrevEnq" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlEnqList" runat="server" BorderColor="AliceBlue" BorderStyle="Ridge"
                                        Height="400px" ScrollBars="Auto" Width="950px">
                                        <table class="DetailPopup" width="950px" Height="400px">
                                            <tr valign="top">
                                                <td align="left" valign="top">
                                                    <asp:ImageButton ID="imgCloseEnqList" runat="server" ImageUrl="Images/cross_icon.png"
                                                        OnClick="imgCloseEnqList_Click" ImageAlign="Right" />
                                                    &nbsp;
                                                    <br>
                                                    <asp:GridView ID="grdEnqList" runat="server" AutoGenerateColumns="False" CellPadding="2"
                                                        ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="#F7F6F3"
                                                        CssClass="Grid" BorderWidth="1px" Width="100%" OnRowDataBound="grdEnqList_RowDataBound">
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
                                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
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
                                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                        <EditRowStyle BackColor="#999999" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="imgCloseEnqList" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        function validateEnquiry() {
            document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.color = "Red";
            var lblmsg = document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>');
            if (document.getElementById("<%=txt_Client.ClientID%>").value == "") {

                lblmsg.innerHTML = "Please Select the Client Name";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_Client.ClientID%>").focus();
                return false;
            }
            else {
                lblmsg.style.visibility = "hidden";
            }
            if (document.getElementById("<%=txt_Site.ClientID%>").value == "") {
                lblmsg.innerHTML = "Please Select the Site Name";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_Site.ClientID%>").focus();
                return false;
            }
            else {
                lblmsg.style.visibility = "hidden";
            }

            if (document.getElementById("<%=txt_Site_address.ClientID%>").value == "") {
                lblmsg.innerHTML = "Please Enter Address";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_Site_address.ClientID%>").focus();
                return false;
            }
            else {
                lblmsg.style.visibility = "hidden";
            }

            if (document.getElementById("<%=txt_Site_LandMark.ClientID%>").value == "") {
                lblmsg.innerHTML = " Please Enter LandMark";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_Site_LandMark.ClientID%>").focus();
                return false;
            }
            else {
                lblmsg.style.visibility = "hidden";
            }
            if (document.getElementById("<%=txt_ContactPerson.ClientID%>").value == "") {
                lblmsg.innerHTML = "Please Enter Contact Person";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_ContactPerson.ClientID%>").focus();
                return false;
            }
            else {
                lblmsg.style.visibility = "hidden";
            }

            if (document.getElementById("<%=txt_ContactNo.ClientID%>").value == "") {
                lblmsg.innerHTML = "Please Enter Contact No.";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_ContactNo.ClientID%>").focus();
                return false;
            }
            else {
                lblmsg.style.visibility = "hidden";
            }
            //            var Inward = document.getElementById("<%=ddl_InwardType.ClientID %>");
            //            if (Inward.value == "----------------------------Select----------------------------") {

            //                lblmsg.innerHTML = "Please Select the InWard Type";
            //                lblmsg.style.visibility = "visible";
            //                Inward.focus();
            //                return false;
            //            }
            //            else {
            //                lblmsg.style.visibility = "hidden";
            //            }
            //            if (document.getElementById("<%=txt_Quantity.ClientID%>").value == "") {

            //                lblmsg.innerHTML = "Please Enter Quantity";
            //                lblmsg.style.visibility = "visible";
            //                document.getElementById("<%=txt_Quantity.ClientID%>").focus();
            //                return false;
            //            }
            //            else {
            //                lblmsg.style.visibility = "hidden";
            //            }
            //            if (document.getElementById("<%=txt_Note.ClientID%>").value == "") {

            //                lblmsg.innerHTML = " Please Enter Note";
            //                lblmsg.style.visibility = "visible";
            //                document.getElementById("<%=txt_Note.ClientID%>").focus();
            //                return false;
            //            }
            //            else {
            //                lblmsg.style.visibility = "hidden";
            //            }
            //            if (document.getElementById("<%=txt_Reference.ClientID%>").value == "") {

            //                lblmsg.innerHTML = "Please Enter Some Reference";
            //                lblmsg.style.visibility = "visible";
            //                document.getElementById("<%=txt_Reference.ClientID%>").focus();
            //                return false;
            //            }
            //            else {
            //                lblmsg.style.visibility = "hidden";
            //            }

        }
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
