<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="True"
    Theme="duro" CodeBehind="Client.aspx.cs" Inherits="DESPLWEB.Client" Title="Client Updation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<script type="text/javascript">
        function clickButton(e, buttonid) {
            var evt = e ? e : window.event;
            var bt = document.getElementById(buttonid);
            if (bt) {
                if (evt.keyCode == 13) {
                    bt.click();
                    return false;
                }
            }
        }
        function catchEsc(e) {
            var kC = (window.event) ?    // MSIE or Firefox?
                 event.keyCode : e.keyCode;
            var Esc = (window.event) ?
                27 : e.DOM_VK_ESCAPE // MSIE : Firefox
            if (kC == Esc) {
                var mpu = $find('ModalPopupExtender1');
                mpu.hide();
            }
        }
    </script>--%>
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <table style="width: 100%">
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr style="background-color: #ECF5FF;">
                                <td>
                                    <asp:Label ID="lblClient" runat="server" Font-Bold="True" Text="Clients"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearch" runat="server" MaxLength="50" Width="343px" Height="16px"></asp:TextBox>
                                    <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/Search-32.png"
                                        Style="width: 14px" OnClick="ImgBtnSearch_Click" />
                                    &nbsp;
                                    <%--  <asp:LinkButton ID="lnkLoadAllClient" runat="server" CssClass="LnkOver" OnClick="lnkLoadAllClient_Click"
                                        Style="text-decoration: underline; font-weight: bold;">Load All Client</asp:LinkButton>--%>
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblClientApprRight" runat="server" Text="" Visible="false"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkApprPending" runat="server" Text="Approval Pending" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkAddNewSite" runat="server" OnClick="imgInsertClient_Click"
                                        Font-Underline="true" CssClass="SimpleColor" ToolTip="Add New Client">Add New Client</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hfClientId" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 23px" valign="top">
                                    <asp:Panel ID="pnlClientList" runat="server" Width="100%" BorderWidth="0px" Height="420px"
                                        ScrollBars="Auto">
                                        <asp:GridView ID="grdClient" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            DataKeyNames="CL_Id" ForeColor="#333333" GridLines="Vertical" BorderColor="#DEDFDE"
                                            BorderWidth="1px" Width="100%" OnRowDataBound="grdClient_RowDataBound">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <%-- <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgInsertClient" runat="server" OnCommand="imgInsertClient_Click"
                                                            ImageUrl="Images/AddNewitem.jpg" Height="20px" Width="20px" CausesValidation="false"
                                                            ToolTip="Add New Client" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgEditClient" runat="server" CommandArgument='<%#Eval("CL_Id")%>'
                                                            OnCommand="imgEditClient_Click" ImageUrl="Images/Edit.jpg" Height="18px" Width="18px"
                                                            CausesValidation="false" ToolTip="Edit Client" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Client Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClientId" runat="server" Text='<%#Eval("CL_Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Client Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("CL_Name_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Office Address">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOfficeAddress" runat="server" Text='<%#Eval("CL_OfficeAddress_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Office Tel No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOfficeTelNo" runat="server" Text='<%#Eval("CL_OfficeTelNo_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Client Email Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClientEmailId" runat="server" Text='<%#Eval("CL_EmailId_var") %>' />
                                                    </ItemTemplate>
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
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                            PopupControlID="pnlClient" PopupDragHandleControlID="PopupHeader" Drag="true"
                            BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="pnlClient" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlClientSearch" runat="server" BorderColor="AliceBlue" BorderStyle="Ridge"
                                        Height="500px" ScrollBars="Auto" Visible="false" Width="750px">
                                        <table class="DetailPopup" width="750px">
                                            <tr valign="top">
                                                <td align="center" valign="top">
                                                    <asp:ImageButton ID="imgCloseClientSearch" runat="server" ImageUrl="Images/cross_icon.png"
                                                        OnClick="imgCloseClientSearch_Click" ImageAlign="Right" />
                                                    &nbsp;
                                                    <asp:LinkButton ID="lnkOkClientSearch" runat="server" Font-Underline="true" OnClick="lnkOkClientSearch_Click" Visible="false">Ok</asp:LinkButton>
                                                    &nbsp;
                                                    <br>
                                                    <asp:GridView ID="grdClientSearch" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                                                        BorderColor="#DEBA84" BorderWidth="1px" CellPadding="0" CellSpacing="0" CssClass="Grid"
                                                        ForeColor="#333333" GridLines="Horizontal" SkinID="gridviewSkin1" Width="100%" OnRowDataBound="grdClientSearch_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Client Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblClientId" runat="server" Text='<%#Eval("CL_Id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("CL_Name_var") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblClientStatus" runat="server" Text='<%#Eval("CL_Status_bit") %>'></asp:Label>
                                                                    <%--<asp:Label ID="lblClientStatus" runat="server" Text='<%# (string)Eval("CL_Status_bit") == "0" ? "Active":"Deactive"%>'></asp:Label>--%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlClient1" runat="server" ScrollBars="Vertical"   Height="650px">
                                        <table class="DetailPopup">
                                            <tr valign="top">
                                                <td align="center" valign="bottom" colspan="6">
                                                    <asp:ImageButton ID="imgCloseClientPopup" runat="server" ImageUrl="Images/cross_icon.png"
                                                        OnClick="imgCloseClientPopup_Click" ImageAlign="Right" />
                                                    <asp:Label ID="lblAddClient" runat="server" ForeColor="#990033" Text="Add New Client"
                                                        Font-Bold="True" Font-Size="Small"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top" style="width: 140px">Current Client Name
                                                </td>
                                                <td style="width: 3px">&nbsp;
                                                </td>
                                                <td style="width: 250px" valign="top">
                                                    <asp:Label ID="lblCurrentClientName" runat="server" Font-Bold="True" ForeColor="#CC0099"
                                                        Text="" Font-Size="12px"></asp:Label>&nbsp;
                                                    <asp:Label ID="lblCurrentClientId" runat="server" Font-Bold="True" ForeColor="#CC0099"
                                                        Text="" Font-Size="12px"></asp:Label>
                                                    <asp:Label ID="lblClId" runat="server" Text="" Visible="false" ></asp:Label>
                                                </td>
                                                <td align="right" valign="top" style="width: 180px">
                                                    <asp:LinkButton ID="lnkViewAllClient" runat="server" CssClass="LnkOver" Font-Bold="True"
                                                        OnClick="lnkViewAllClient_Click" Style="text-decoration: underline;" Visible="true">View All Client</asp:LinkButton>
                                                    New Client Name
                                                </td>
                                                <td style="width: 3px">&nbsp;
                                                </td>
                                                <td style="width: 215px">
                                                    <asp:TextBox ID="txtClientName" runat="server" MaxLength="250" Width="192px" autocomplete="off"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtClientName"
                                                        EnableClientScript="False" ErrorMessage="Input 'Client Name'" SetFocusOnError="True"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                    <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                                                        DelimiterCharacters=" " ShowOnlyCurrentWordInCompletionListItem="true" CompletionInterval="10"
                                                        EnableCaching="false" CompletionSetCount="1" TargetControlID="txtClientName"
                                                        ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="right" valign="top">Director/ Proprietor Name
                                                </td>
                                                <td style="width: 3px">&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDirector" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                                    &nbsp;
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtDirector"
                                                        EnableClientScript="False" ErrorMessage="Input 'Director/ Proprietor Name'" SetFocusOnError="true"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="right" valign="top">Ledger Name
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLedgerName" runat="server" MaxLength="255" Width="192px"></asp:TextBox>
                                                    <br />
                                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLedgerName"
                                                    EnableClientScript="False" ErrorMessage="Input 'Ledger Name'" SetFocusOnError="true"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                    --%>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="right" valign="top">Director/ Proprietor Email Id
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtDirectorEmail" runat="server" EnableViewState="False" MaxLength="250"
                                                        Width="192px"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="txtDirectorEmail"
                                                        EnableClientScript="False" ErrorMessage="Input 'Email'" SetFocusOnError="true"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtDirectorEmail"
                                                        EnableClientScript="False" ErrorMessage="Invalid Email" ValidationExpression="^([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+,*[\W]*)+$"
                                                        ValidationGroup="V1"></asp:RegularExpressionValidator>
                                                </td>
                                                <td align="right" valign="top">Nature of Firm
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlNatureOfFirm" runat="server" Width="192px" AutoPostBack="true">
                                                        <asp:ListItem Text="--Select--"></asp:ListItem>
                                                        <asp:ListItem Text="Individual"></asp:ListItem>
                                                        <asp:ListItem Text="Partnership"></asp:ListItem>
                                                        <asp:ListItem Text="HUF"></asp:ListItem>
                                                        <asp:ListItem Text="LLP"></asp:ListItem>
                                                        <asp:ListItem Text="Company"></asp:ListItem>
                                                        <asp:ListItem Text="Other"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="ddlNatureOfFirm"
                                                        EnableClientScript="False" ErrorMessage="Input 'Nature Of Firm'" SetFocusOnError="true"
                                                        InitialValue="--Select--" ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="right" valign="top">Office Tel. No.
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOffTelNo" runat="server" MaxLength="255" Width="192px"></asp:TextBox>
                                                    &nbsp;
                                                    <asp:LinkButton ID="lnkUpdateDirectorDetail" runat="server" CssClass="LnkOver" Font-Bold="True"
                                                        OnClick="lnkUpdateDirectorDetail_Click" Style="text-decoration: underline;">Update</asp:LinkButton>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtOffTelNo"
                                                        EnableClientScript="False" ErrorMessage="Input 'Office Tel. No.'" SetFocusOnError="True"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                    <br />
                                                    <asp:Label ID="lblDirectorDetailUpdateDate" runat="server" Font-Bold="false" ForeColor="#000099"
                                                        Text="" Font-Size="12px"></asp:Label>
                                                </td>
                                                <td align="right" valign="top">Office Address
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOffAddress" runat="server" MaxLength="255" Width="192px" TextMode="MultiLine"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtOffAddress"
                                                        EnableClientScript="False" ErrorMessage="Input 'Office Address'" SetFocusOnError="true"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="right" valign="top">Nature Of Business
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNatureBusiness" runat="server" MaxLength="255" Width="192px"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtNatureBusiness"
                                                        EnableClientScript="False" ErrorMessage="Input 'Nature of Business'" SetFocusOnError="True"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                </td>
                                                
                                                <td align="right" valign="top">State
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlState" runat="server" Width="192px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="ddlState"
                                                        EnableClientScript="False" ErrorMessage="Input 'State'" SetFocusOnError="true"
                                                        InitialValue="0" ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="right" valign="top">Pincode
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPincode" runat="server" MaxLength="6" Width="192px" onkeypress="return CheckNumeric(event,this);"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPincode"
                                                        EnableClientScript="False" ErrorMessage="Input 'Pin code'" SetFocusOnError="True"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="right" valign="top">City
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtCity" runat="server" MaxLength="50" Visible="false" onkeypress="return onlyAlphabets(event,this);"
                                                        Width="192px"></asp:TextBox>
                                                    <asp:DropDownList ID="ddlCity" runat="server" Width="192px" OnFocus="this.style.borderColor='red'"
                                                        AutoPostBack="true" OnBlur="this.style.borderColor='green'">
                                                    </asp:DropDownList>
                                                    <br />
                                                    <%--   <asp:ImageButton ID="imgAddCity" runat="server"  OnCommand="lnkAddCity_Click" ImageUrl="Images/AddNewitem.jpg" 
                                                    Height="20px" Width="20px" CausesValidation="false" ToolTip="Add New City" 
                                                    />--%>
                                                    <asp:LinkButton ID="lnkAddCity" runat="server" Font-Underline="true" OnClick="lnkAddCity_Click">New City</asp:LinkButton>
                                                    &nbsp;&nbsp;<asp:LinkButton ID="lnkSaveCity" runat="server" Font-Underline="true"
                                                        Visible="false" OnClick="lnkSaveCity_Click">Save City</asp:LinkButton>
                                                    <%--
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator120" runat="server" ControlToValidate="txtCity"
                                                    EnableClientScript="False" ErrorMessage="Input 'City'" SetFocusOnError="True"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="right" valign="top">Pan No
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPanNo" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtPanNo"
                                                        EnableClientScript="False" ErrorMessage="Input 'Pan No'" SetFocusOnError="True"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="right" valign="top">Tan No
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTanNo" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtTanNo"
                                                        EnableClientScript="False" ErrorMessage="Input 'Tan No'" SetFocusOnError="True"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">GST
                                                </td>
                                                <td colspan="5">
                                                    <asp:RadioButton ID="rbSiteReg" AutoPostBack="true" runat="server" Text="Registered"
                                                        GroupName="R2" OnCheckedChanged="rbSiteReg_CheckedChanged" />
                                                    &nbsp;
                                                    <asp:RadioButton ID="rbSiteUnReg" AutoPostBack="true" runat="server" Text="Unregistered"
                                                        GroupName="R2" OnCheckedChanged="rbSiteUnReg_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="right" valign="top">Provisional GST No
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtGstNo" runat="server" Enabled="false" MaxLength="15" Width="192px"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="valName1" runat="server" ForeColor="Red" Text="Input 'GST No'" Visible="false"></asp:Label>
                                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtGstDate"
                                                    Enabled="false" EnableClientScript="False" ErrorMessage=" Input 'GST No'" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                              <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtGstNo" MaskType="None"
                                                    Mask="99 ?????????? ???" AutoComplete="false" runat="server">
                                                </asp:MaskedEditExtender>--%>
                                                </td>
                                                <td align="right" valign="top">Provisional GST Date of Registration
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtGstDate" runat="server" Enabled="false" MaxLength="50" Width="192px"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="valName2" runat="server" ForeColor="Red" Text="Input 'GST Date'" Visible="false"></asp:Label>
                                                    <%--    <asp:RequiredFieldValidator ID="v11" runat="server" ControlToValidate="txtGstDate"
                                                    Enabled="false" EnableClientScript="False" ErrorMessage=" Input 'GST Date'" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    --%>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                        Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtGstDate">
                                                    </asp:CalendarExtender>
                                                    <%--   <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtGstDate" MaskType="Date"
                                                    Mask="99/99/9999" AutoComplete="false" runat="server">
                                                </asp:MaskedEditExtender>--%>
                                                    <br />
                                            </tr>
                                            <tr valign="top">
                                                <td align="right" valign="top">Office Fax No.
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOffFaxNo" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtOffFaxNo"
                                                        EnableClientScript="False" ErrorMessage="Input 'Office Fax No.'" SetFocusOnError="True"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="right" valign="top">Account Contact Name
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAccContact" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtAccContact"
                                                        EnableClientScript="False" ErrorMessage="Input 'Account Contact Name'" SetFocusOnError="True"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="right" valign="top">Client Email Id
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtClientEmail" runat="server" EnableViewState="False" MaxLength="250"
                                                        Width="192px"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtClientEmail"
                                                        EnableClientScript="False" ErrorMessage="Input 'Email'" SetFocusOnError="true"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtClientEmail"
                                                        EnableClientScript="False" ErrorMessage="Invalid Email" ValidationExpression="^([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+,*[\W]*)+$"
                                                        ValidationGroup="V1"></asp:RegularExpressionValidator>
                                                </td>
                                                <td align="right" valign="top">&nbsp;Account Contact No.
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtAccContactNo" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="txtAccContactNo"
                                                        EnableClientScript="False" ErrorMessage="Input 'Account Contact Number'" SetFocusOnError="True"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">&nbsp;Authorised person view report
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td align="left" valign="middle">
                                                    <asp:TextBox ID="txtAuthPerson" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                                    &nbsp;
                                                    <asp:LinkButton ID="lnkUpdateAuthPersonDetail" runat="server" CssClass="LnkOver" Font-Bold="True"
                                                        OnClick="lnkUpdateAuthPersonDetail_Click" Style="text-decoration: underline;">Update</asp:LinkButton>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtAuthPerson"
                                                        EnableClientScript="False" ErrorMessage="Input 'Authorised person'" SetFocusOnError="True"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="right" valign="top">Account Email Id
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtAccEmail" runat="server" EnableViewState="False" MaxLength="250"
                                                        Width="192px"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtAccEmail"
                                                        EnableClientScript="False" ErrorMessage="Input 'Email'" SetFocusOnError="true"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtAccEmail"
                                                        EnableClientScript="False" ErrorMessage="Invalid Email" ValidationExpression="^([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+,*[\W]*)+$"
                                                        ValidationGroup="V1"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">&nbsp;Client Group
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td align="left" valign="middle" style="height: 40px">
                                                    <asp:DropDownList ID="ddlClientGroup" runat="server" Width="200px">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtClientGroup" runat="server" MaxLength="50" Width="192px" Visible="false"></asp:TextBox>
                                                    &nbsp;
                                                    <asp:LinkButton ID="lnkNewGroup" runat="server" CssClass="LnkOver" Font-Bold="True"
                                                        OnClick="lnkNewGroup_Click" Style="text-decoration: underline;" ValidationGroup="V1">New</asp:LinkButton>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtClientGroup"
                                                        EnableClientScript="False" ErrorMessage="Input 'Client Group'" SetFocusOnError="True"
                                                        Visible="false" ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlClientGroup"
                                                        InitialValue="0" EnableClientScript="False" ErrorMessage="Select 'Client Group'"
                                                        SetFocusOnError="True" ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="right" valign="top">Client Login Name
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtLoginName" runat="server" MaxLength="50" Width="192px" ReadOnly="true"></asp:TextBox>
                                                    <br />
                                                    &nbsp;
                                                    <asp:LinkButton ID="lnkmailLogin" runat="server" CssClass="LnkOver" Font-Bold="True"
                                                        OnClick="lnkmailLogin_Click" Style="text-decoration: underline;" >Email-Login details </asp:LinkButton>
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtLoginName"
                                                    EnableClientScript="False" ErrorMessage="Input 'Login Name'" SetFocusOnError="True"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">&nbsp; Client Active Status
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkClientStatus" runat="server" Text=" " />
                                                    &nbsp; Site Specific Coupon &nbsp;
                                                    <asp:CheckBox ID="chkCouponSetting" runat="server" Text=" " Enabled="false" />
                                                </td>
                                                <td align="right" valign="top">&nbsp;Password
                                                </td>
                                                <td>&nbsp;   
                                                </td>
                                                <td valign="Top">
                                                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" Width="192px" ReadOnly="true"></asp:TextBox>
                                                    <br />
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtPassword"
                                                    EnableClientScript="False" ErrorMessage="Input 'Password'" SetFocusOnError="True"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">&nbsp; Priority Client
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkPriorityClient" runat="server" Text=" " />
                                                    &nbsp;&nbsp;     
                                                    <asp:Label ID="lblClientMessage" runat="server" ForeColor="#990033" Text="lblMsg"
                                                        Visible="False"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;    
                                                </td>
                                                <td align="right">
                                                    <asp:LinkButton ID="lnkSaveClient" runat="server" CssClass="LnkOver" OnClick="lnkSave_Click"
                                                        Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V1">Save</asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkCancelClient" runat="server" CssClass="LnkOver" OnClick="lnkCancelClient_Click"
                                                        Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V1">Cancel</asp:LinkButton>
                                                    <br />&nbsp;
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="right" valign="top">Credit Limit - Standard
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtCrLimitStd" runat="server" EnableViewState="False" 
                                                        Width="192px" ReadOnly="true"></asp:TextBox>
                                                    <br />&nbsp;
                                                </td>
                                                <td align="right" valign="top">&nbsp;Credit Limit - Modified
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtCrLimitMod" runat="server" MaxLength="50" Width="192px" ReadOnly="true" onkeyup="checkDecimal(this);"></asp:TextBox>
                                                    <br />&nbsp;
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="right" valign="top">Credit Period - Standard
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtCrPeriodStd" runat="server" EnableViewState="False" 
                                                        Width="192px" ReadOnly="true"></asp:TextBox>
                                                    <br />&nbsp;
                                                </td>
                                                <td align="right" valign="top">&nbsp;Credit Period - Modified
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtCrPeriodMod" runat="server" MaxLength="50" Width="192px" ReadOnly="true" onkeypress="return CheckNumeric(event,this);"></asp:TextBox>
                                                    <br />&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="right">&nbsp; Average Business
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                     <asp:TextBox ID="txtAvegBusiness" runat="server" EnableViewState="False" 
                                                        Width="192px" ReadOnly="true"></asp:TextBox>
                                                    <br />&nbsp;
                                                </td>
                                                <td align="right" valign="top">&nbsp; 
                                                </td>
                                                <td>&nbsp; 
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkUpdateCreditDetails" runat="server" CssClass="LnkOver" Font-Bold="True"
                                                        OnClick="lnkUpdateCreditDetails_Click" Style="text-decoration: underline;" Enabled="false">Update Credit Details</asp:LinkButton>
                                                    <br />
                                                </td>
                                            </tr>                                            
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lnkCancelClient" />
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
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
        function onlyAlphabets(e, t) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 8)
                    return true;
                else
                    return false;
            }
            catch (err) {
                alert(err.Description);
            }
        }

        function CheckNumeric(e) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if ((charCode >= 48 && charCode <= 57) || charCode == 8)
                    return true;
                else
                    return false;
            }
            catch (err) {
                alert(err.Description);
            }
           
        }

        function checkNum(x) {
            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {

                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Numeric Values Allowed";
                    x.value = '';
                    x.focus();
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
                    alert("Only Numeric Values Allowed");

                    return false;
                }

            }
            return true;
        }
    </script>
</asp:Content>
