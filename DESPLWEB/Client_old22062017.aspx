<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="True" Theme="duro"
    CodeBehind="Client.aspx.cs" Inherits="DESPLWEB.Client" Title="Client - Site Updation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
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
    </script>
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
                                    <asp:CheckBox ID="chkApprPending" runat="server" Text="Approval Pending"/>
                                    </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 23px" valign="top">
                                    <asp:Panel ID="pnlClientList" runat="server" Width="100%" BorderWidth="0px" Height="150px"
                                        ScrollBars="Auto">
                                        <asp:GridView ID="grdClient" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            DataKeyNames="CL_Id" ForeColor="#333333" GridLines="Vertical" BorderColor="#DEDFDE"
                                            BorderWidth="1px" Width="100%" OnRowDataBound="grdClient_RowDataBound">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgInsertClient" runat="server" OnCommand="imgInsertClient_Click"
                                                            ImageUrl="Images/AddNewitem.jpg" Height="20px" Width="20px" CausesValidation="false"
                                                            ToolTip="Add New Client" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgEditClient" runat="server" CommandArgument='<%#Eval("CL_Id")%>'
                                                            OnCommand="imgEditClient_Click" ImageUrl="Images/Edit.jpg" Height="18px" Width="18px"
                                                            CausesValidation="false" ToolTip="Edit Client" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnSites" runat="server" CommandArgument='<%#Eval("CL_Id")%>'
                                                            OnCommand="lnkLoadSites" Text="Sites">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25px" />
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
                            <tr style="background-color: #ECF5FF;">
                                <td colspan="2">
                                    <asp:CheckBox ID="chkApprPendingSite" runat="server" Text="Approval Pending Sites" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblSite" runat="server" Font-Bold="True" Text="Sites" Visible="False"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblEditedClient" runat="server" Font-Bold="True" ForeColor="#990033"
                                        Text="Client : " Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 23px" valign="top">
                                    <asp:Panel ID="pnlSiteList" runat="server" Width="100%" BorderWidth="0px" Height="110px"
                                        ScrollBars="Auto">
                                        <asp:GridView ID="grdSite" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#DEDFDE" BorderWidth="1px" CellPadding="4" DataKeyNames="SITE_Id"
                                            ForeColor="Black" GridLines="Vertical" Width="100%" OnRowDataBound="grdSite_RowDataBound">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgInsertSite" runat="server" OnCommand="imgInsertSite_Click"
                                                            ImageUrl="Images/AddNewitem.jpg" Height="20px" Width="20px" CausesValidation="false"
                                                            ToolTip="Add New Site" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgEditSite" runat="server" CommandArgument='<%#Eval("SITE_Id")%>'
                                                            OnCommand="imgEditSite_Click" ImageUrl="Images/Edit.jpg" Height="18px" Width="18px"
                                                            CausesValidation="false" ToolTip="Edit Site" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnContactPersons" runat="server" CommandArgument='<%#Eval("SITE_Id")%>'
                                                            OnCommand="lnkLoadContactPersons" Text="Contacts">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Site Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSiteId" runat="server" Text='<%#Eval("SITE_Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Site Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSiteName" runat="server" Text='<%#Eval("SITE_Name_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Site Address">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSiteAddress" runat="server" Text='<%#Eval("SITE_Address_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Site Email Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSiteEmailID" runat="server" Text='<%#Eval("SITE_EmailID_var") %>' />
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
                            <tr style="background-color: #ECF5FF;">
                                <td colspan="2">
                                    <asp:Label ID="lblContPer" runat="server" Font-Bold="True" Text="Contact Persons"
                                        Visible="False"></asp:Label>
                                    &nbsp; &nbsp;
                                    <asp:Label ID="lblEditedSite" runat="server" Font-Bold="True" ForeColor="#990033"
                                        Text="Site : " Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 23px" valign="top">
                                    <asp:Panel ID="pnlContactList" runat="server" Width="100%" BorderWidth="0px" Height="110px"
                                        ScrollBars="Auto">
                                        <asp:GridView ID="grdContact" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#DEDFDE" BorderWidth="1px" CellPadding="4" DataKeyNames="CONT_Id"
                                            ForeColor="Black" GridLines="Vertical" Width="100%" OnRowDataBound="grdContact_RowDataBound">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgInsertContact" runat="server" OnCommand="imgInsertContact_Click"
                                                            ImageUrl="Images/AddNewitem.jpg" Height="20px" Width="20px" CausesValidation="false"
                                                            ToolTip="Add New Contact Person" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgEditContact" runat="server" CommandArgument='<%#Eval("CONT_Id")%>'
                                                            OnCommand="imgEditContact_Click" ImageUrl="Images/Edit.jpg" Height="18px" Width="18px"
                                                            CausesValidation="false" ToolTip="Edit Contact Person" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Contact Person Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblContactPersonId" runat="server" Text='<%#Eval("CONT_Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Contact Person Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblContactPersonName" runat="server" Text='<%#Eval("CONT_Name_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Contact Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblContactNo" runat="server" Text='<%#Eval("CONT_ContactNo_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Email Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblContactEmail" runat="server" Text='<%#Eval("CONT_EmailID_var") %>' />
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
                                            <td align="right" valign="top">
                                                Current Client Name
                                            </td>
                                            <td colspan="4">
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="lblCurrentClientName" runat="server" runat="server" Font-Bold="True" ForeColor="#CC0099"
                                                    Text="" Font-Size="12px"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                &nbsp;
                                            </td>
                                        </tr> 
                                        <tr valign="top">
                                            <td align="right" valign="top" style="width: 130px">
                                                New Client Name
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp; <asp:HiddenField ID="hfClientId" runat="server" />
                                            </td>
                                            <td style="width: 250px">
                                                <asp:TextBox ID="txtClientName" runat="server" MaxLength="250" Width="192px" autocomplete="off"  ></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtClientName"
                                                    EnableClientScript="False" ErrorMessage="Input 'Client Name'" SetFocusOnError="True"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                         DelimiterCharacters=" "  ShowOnlyCurrentWordInCompletionListItem="true"   CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtClientName"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                                            </td>
                                            <td align="right" valign="top" style="width: 180px">
                                                Director/ Proprietor Name
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 215px">
                                                <asp:TextBox ID="txtDirector" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtDirector"
                                                    EnableClientScript="False" ErrorMessage="Input 'Director/ Proprietor Name'" SetFocusOnError="true"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" valign="top">
                                                Ledger Name
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLedgerName" runat="server" MaxLength="255" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLedgerName"
                                                    EnableClientScript="False" ErrorMessage="Input 'Ledger Name'" SetFocusOnError="true"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                            <td align="right" valign="top">
                                                Director/ Proprietor Email Id
                                            </td>
                                            <td>
                                                &nbsp;
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
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" valign="top">
                                                Office Address
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOffAddress" runat="server" MaxLength="255" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtOffAddress"
                                                    EnableClientScript="False" ErrorMessage="Input 'Office Address'" SetFocusOnError="true"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                            <td align="right" valign="top">
                                                Office Tel. No.
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOffTelNo" runat="server" MaxLength="255" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtOffTelNo"
                                                    EnableClientScript="False" ErrorMessage="Input 'Office Tel. No.'" SetFocusOnError="True"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" valign="top">
                                                Pan No
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPanNo" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPanNo"
                                                    EnableClientScript="False" ErrorMessage="Input 'Pan No'" SetFocusOnError="True"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                            <td align="right" valign="top">
                                                Tan No
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTanNo" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtTanNo"
                                                    EnableClientScript="False" ErrorMessage="Input 'Tan No'" SetFocusOnError="True"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                            
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" valign="top">
                                                Office Fax No.
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOffFaxNo" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtOffFaxNo"
                                                    EnableClientScript="False" ErrorMessage="Input 'Office Fax No.'" SetFocusOnError="True"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                            <td align="right" valign="top">
                                                Account Contact Name
                                            </td>
                                            <td>
                                                &nbsp;
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
                                            <td align="right" valign="top">
                                                Client Email Id
                                            </td>
                                            <td>
                                                &nbsp;
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
                                            <td align="right" valign="top">
                                                &nbsp;Account Contact No.
                                            </td>
                                            <td>
                                                &nbsp;
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
                                            <td align="right" valign="top">
                                                &nbsp;Authorised person view report
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="left" valign="middle">
                                                <asp:TextBox ID="txtAuthPerson" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtAuthPerson"
                                                    EnableClientScript="False" ErrorMessage="Input 'Authorised person'" SetFocusOnError="True"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                            <td align="right" valign="top">
                                                Account Email Id
                                            </td>
                                            <td>
                                                &nbsp;
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
                                            <td align="right" valign="top">
                                                &nbsp;Client Group
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="left" valign="middle" style="height:40px">
                                                <asp:DropDownList ID="ddlClientGroup" runat="server" Width="200px" >
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtClientGroup" runat="server" MaxLength="50" Width="192px" Visible="false"></asp:TextBox>
                                                &nbsp;
                                                <asp:LinkButton ID="lnkNewGroup" runat="server" CssClass="LnkOver" Font-Bold="True"
                                                    OnClick="lnkNewGroup_Click" Style="text-decoration: underline;" ValidationGroup="V1">New</asp:LinkButton>
                                                    <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtClientGroup"
                                                    EnableClientScript="False" ErrorMessage="Input 'Client Group'" SetFocusOnError="True" Visible="false"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlClientGroup" InitialValue="0"
                                                    EnableClientScript="False" ErrorMessage="Select 'Client Group'" SetFocusOnError="True"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                            <td align="right" valign="top">
                                                Client Login Name
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtLoginName" runat="server" MaxLength="50" Width="192px" ReadOnly="true"></asp:TextBox>
                                                <br />
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtLoginName"
                                                    EnableClientScript="False" ErrorMessage="Input 'Login Name'" SetFocusOnError="True"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                &nbsp; Client Active Status
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkClientStatus" runat="server" Text=" " />
                                                &nbsp;&nbsp;&nbsp; Site Specific Coupon &nbsp;
                                                <asp:CheckBox ID="chkCouponSetting" runat="server" Text=" " Enabled="false" />
                                            </td>
                                            <td align="right" valign="top"> 
                                                &nbsp;Password
                                            </td>
                                            <td>
                                                &nbsp;
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
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
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
                                            <td>
                                                <asp:Label ID="lblClientMessage" runat="server" ForeColor="#990033" Text="lblMsg"
                                                    Visible="False"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnkSaveClient" runat="server" CssClass="LnkOver" OnClick="lnkSave_Click"
                                                    Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V1">Save</asp:LinkButton>
                                                &nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkCancelClient" runat="server" CssClass="LnkOver" OnClick="lnkCancelClient_Click"
                                                    Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V1">Cancel</asp:LinkButton>
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
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lnkCancelClient" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="HiddenField1"
                            PopupControlID="pnlSite" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="pnlSite" runat="server" BorderWidth="0px" Width="449px">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table class="DetailPopup" style="width: 99%">
                                        <tr valign="top">
                                            <td align="center" valign="bottom" colspan="3">
                                                <asp:ImageButton ID="imgCloseSitePopup" runat="server" ImageUrl="Images/cross_icon.png"
                                                    OnClick="imgCloseSitePopup_Click" ImageAlign="Right" />
                                                <asp:Label ID="lblAddSite" runat="server" Font-Bold="True" ForeColor="#990033" Text="Add New Site"
                                                    Font-Size="Small"></asp:Label>
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
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" style="width: 170px">
                                                Client Name
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSiteClient" runat="server" Font-Bold="True" ForeColor="#CC0099"
                                                    Text="Site name" Font-Size="12px"></asp:Label>
                                                <br />
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" style="width: 170px">
                                                Current Site Name
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCurrentSiteName" runat="server" runat="server" Font-Bold="True" ForeColor="#CC0099"
                                                    Text="" Font-Size="12px"></asp:Label>
                                                <br />
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" style="width: 170px">
                                                Site Name
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSiteName" runat="server" MaxLength="250" Width="192px" autocomplete="off"  ></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtSiteName"
                                                    EnableClientScript="False" ErrorMessage="Input 'Site Name'" SetFocusOnError="True"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                Site Address
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSiteAddress" runat="server" MaxLength="255" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtSiteAddress"
                                                    EnableClientScript="False" ErrorMessage="Input 'Office Address'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" valign="top">
                                                Email Id
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtSiteEmail" runat="server" EnableViewState="False" MaxLength="250"
                                                    Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtSiteEmail"
                                                    EnableClientScript="False" ErrorMessage="Input 'Email'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSiteEmail"
                                                    EnableClientScript="False" ErrorMessage="Invalid Email" ValidationExpression="^([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+,*[\W]*)+$"
                                                    ValidationGroup="V2"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" valign="top">
                                                Site Active Status
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <asp:CheckBox ID="chkSiteStatus" runat="server" Text=" " />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" valign="top">
                                                Service Tax Not Applicable
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <asp:CheckBox ID="chkServiceTax" runat="server" Text=" " />
                                                <br />
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
                                                <asp:Label ID="lblSiteMessage" runat="server" ForeColor="#990033" Text="lblMsg" Visible="False"></asp:Label>
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
                                                &nbsp;
                                                <asp:LinkButton ID="lnkSaveSite" runat="server" CssClass="LnkOver" OnClick="lnkSaveSite_Click"
                                                    Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V2">Save</asp:LinkButton>
                                                &nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkCancelSite" runat="server" CssClass="LnkOver" OnClick="lnkCancelSite_Click"
                                                    Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V2">Cancel</asp:LinkButton>
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
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lnkCancelSite" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="HiddenField1"
                            PopupControlID="pnlContactPerson" PopupDragHandleControlID="PopupHeader" Drag="true"
                            BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="pnlContactPerson" runat="server" BorderWidth="0px" Width="452px">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <table class="DetailPopup" style="width: 100%">
                                        <tr valign="top">
                                            <td align="center" valign="bottom" colspan="3">
                                                <asp:ImageButton ID="imgCloseContactPopup" runat="server" ImageUrl="Images/cross_icon.png"
                                                    OnClick="imgCloseContactPopup_Click" ImageAlign="Right" />
                                                <asp:Label ID="lblAddContact" runat="server" Font-Bold="True" ForeColor="#990033"
                                                    Text="Add New Contact Person" Font-Size="Small"></asp:Label>
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
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" style="width: 170px">
                                                Site Name
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblContactSite" runat="server" Font-Bold="True" ForeColor="#CC0099"
                                                    Text="Site name" Font-Size="12px"></asp:Label>
                                                <br />
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" style="width: 170px">
                                                Contact Person Name
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContactPer" runat="server" MaxLength="50" Width="192px" autocomplete="off" ></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtContactPer"
                                                    EnableClientScript="False" ErrorMessage="Input 'Contact Person Name'" SetFocusOnError="True"
                                                    ValidationGroup="V3"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                Contact Number
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContactNo" runat="server" MaxLength="255" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtContactNo"
                                                    EnableClientScript="False" ErrorMessage="Input 'Contact Number'" SetFocusOnError="true"
                                                    ValidationGroup="V3"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" valign="top">
                                                Email Id
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtContactEmail" runat="server" EnableViewState="False" MaxLength="250"
                                                    Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtContactEmail"
                                                    EnableClientScript="False" ErrorMessage="Input 'Email'" SetFocusOnError="true"
                                                    ValidationGroup="V3"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtContactEmail"
                                                    EnableClientScript="False" ErrorMessage="Invalid Email" ValidationExpression="^([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+,*[\W]*)+$"
                                                    ValidationGroup="V3"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" valign="top">
                                                Active Status
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <asp:CheckBox ID="chkContactStatus" runat="server" Text=" " />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblContMessage" runat="server" ForeColor="#990033" Text="lblMsg" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnkSaveContact" runat="server" CssClass="LnkOver" OnClick="lnkSaveContact_Click"
                                                    Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V3">Save</asp:LinkButton>
                                                &nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkCancelContact" runat="server" CssClass="LnkOver" OnClick="lnkCancelContact_Click"
                                                    Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V3">Cancel</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lnkCancelContact" />
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
      

    </script>
</asp:Content>
