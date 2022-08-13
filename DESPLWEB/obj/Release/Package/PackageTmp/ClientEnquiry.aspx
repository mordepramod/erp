<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="DESPLWEB.ClientEnquiry" Title="Add Enquiry" Codebehind="ClientEnquiry.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

        <center>
            <asp:Panel ID="pnlEnquiry" runat="server" BorderWidth="0px" Width="100%" >
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <table style="width: 100%" class="hlable">
                    <tr style="background-color: #f0b683">
                        <%--#ECF5FF--%>
                        <td colspan="3" align="center" style="">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                                                OnClick="imgClosePopup_Click" ImageAlign="Right" />
                            &nbsp;<asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#990000"
                                Text="Add Enquiry" class="title"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right">
                            &nbsp;<asp:Label ID="lblLoctn" runat="server"
                                Text="" Visible="false"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right"  style="width:40%" valign="top">
                            Company Name
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtClient" runat="server" CssClass="txt" MaxLength="255" Style="margin-left: 0px"
                                Width="250px"></asp:TextBox>
                            <asp:ImageButton ID="ImgBtnSearchClient" runat="server" ImageUrl="~/Images/Search-32.png"
                                Style="width: 14px" Visible="False" />
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtClient"
                                EnableClientScript="False" ErrorMessage="Input 'Company Name'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            Site Name
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSite" runat="server" CssClass="txt" MaxLength="255" Style="margin-left: 0px"
                                Width="250px"></asp:TextBox>
                            <asp:ImageButton ID="ImgBtnSearchSite" runat="server" ImageUrl="~/Images/Search-32.png"
                                Style="width: 14px" Visible="False" />
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSite"
                                EnableClientScript="False" ErrorMessage="Input 'Site Name'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            Site Address
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtAddress" runat="server" Height="50px" TextMode="MultiLine" Width="250px"
                                class="txt"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAddress"
                                EnableClientScript="False" ErrorMessage="Input 'Site Address'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            Contact Person
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td valign="top" align="left">
                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="txt" MaxLength="255"
                                Style="margin-left: 0px" Width="250px"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtContactPerson"
                                EnableClientScript="False" ErrorMessage="Input 'Contact Name'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="right">
                            &nbsp;Contact Number
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtContactNo" runat="server" MaxLength="15" Style="margin-left: 0px"
                                Width="250px" CssClass="txt"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtContactNo"
                                EnableClientScript="False" ErrorMessage="Input 'Contact Number'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            &nbsp;Email ID
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtEmailId" runat="server" MaxLength="50" Width="250px" class="txt"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEmailId"
                                EnableClientScript="False" ErrorMessage="Input 'Email Id'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server"
                                ControlToValidate="txtEmailId" EnableClientScript="False" ErrorMessage="Invalid Email"
                                ValidationGroup="V1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            Material to be Tested
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:ComboBox ID="cmbTestType" runat="server" AutoCompleteMode="Suggest" MaxLength="255"
                                Width="226px" Style="display: inline; font-family: 'Helvetica Neue' , 'Lucida Grande' , 'Segoe UI' , 'Arial', 'Helvetica', 'Verdana', 'sans-serif';
                                font-size: small;">
                            </asp:ComboBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="cmbTestType"
                                EnableClientScript="False" ErrorMessage="Input 'Material to be Tested'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            Any other Requirement
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDesc" runat="server" Height="50px" TextMode="MultiLine" Width="250px"
                                Style="display: inline; font-family: 'Helvetica Neue' , 'Lucida Grande' , 'Segoe UI' , 'Arial', 'Helvetica', 'Verdana', 'sans-serif';
                                font-size: small;"></asp:TextBox>
                        </td>
                    </tr>                    
                    <tr>
                        <td align="right" class="style2">
                            &nbsp;
                        </td>
                        <td class="style2">
                            &nbsp;
                        </td>
                        <td align="left" class="style2">
                            &nbsp;
                            <asp:Label ID="lblMessage" runat="server" ForeColor="#990033" Text="lblMsg" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Button ID="btnAdd" runat="server" Font-Bold="True" Font-Size="8pt" OnClick="btnAdd_Click"
                         BackColor="#f0b683" Text="Add" Width="55px" Style="margin-left: 0px" Height="25px" ValidationGroup="V1" />
                            <asp:Button ID="btnCancel" runat="server" Font-Bold="True" Font-Size="8pt" OnClick="btnCancel_Click"
                          BackColor="#f0b683"  Text="Cancel" Width="55px" Style="margin-left: 0px" Height="25px" CausesValidation="False" />
                        </td>
                    </tr>
                    <tr style="height:8px; Font-Size:4pt">
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
                <%--</ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </center>
        <asp:RoundedCornersExtender ID="RoundedCornersExtender2" runat="server" Enabled="True"
            Radius="3" TargetControlID="pnlEnquiry" BorderColor="Black" Color="BurlyWood">
        </asp:RoundedCornersExtender>--%>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImgBtnSearchClient"
            PopupControlID="pnlClient" PopupDragHandleControlID="PopupHeader" Drag="true"
            BackgroundCssClass="ModalPopupBG">
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnlClient" runat="server" BorderWidth="1px" Width="37%" Visible="False">
            <table style="width: 100%" class="DetailPopup">
                <tr style="background-color: #C0C0C0">
                    <td colspan="3">
                        <asp:Label ID="Label2" runat="server" class="title" Font-Bold="True" ForeColor="#990000"
                            Text="Clients"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                        <asp:ListBox ID="lstClients" runat="server" Height="300px" Width="294px" DataSourceID="SqlDataSource1"
                            DataTextField="CompanyName" DataValueField="CompanyName"></asp:ListBox>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Data Source=DIPL-SERVER;Initial Catalog=Duro;User ID=dipl;Password=dipl"
                            ProviderName="System.Data.SqlClient" SelectCommand="sp_getEnquiryClients" SelectCommandType="StoredProcedure">
                        </asp:SqlDataSource>
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                        <asp:Button ID="btnOkClient" runat="server" Font-Bold="True" Font-Size="8pt" Height="22px"
                            OnClick="btnOkClient_Click" Style="margin-left: 0px" Text="OK" Width="55px" />
                        <asp:Button ID="btnCancelClient" runat="server" CausesValidation="False" Font-Bold="True"
                            Font-Size="8pt" OnClick="btnCancelClient_Click" Style="margin-left: 11px" Text="Cancel"
                            Width="55px" Height="22px" />
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="ImgBtnSearchSite"
            PopupControlID="pnlSite" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnlSite" runat="server" BorderWidth="0px" Width="37%" Visible="False">
            <table style="width: 100%" class="DetailPopup">
                <tr style="background-color: #C0C0C0">
                    <td colspan="3">
                        <asp:Label ID="Label3" runat="server" class="title" Font-Bold="True" ForeColor="#990000"
                            Text="Sites"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                        <asp:ListBox ID="lstSites" runat="server" Height="300px" Width="294px" DataSourceID="SqlDataSource2"
                            DataTextField="SiteName" DataValueField="SiteName"></asp:ListBox>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="Data Source=DIPL-SERVER;Initial Catalog=Duro;User ID=dipl;Password=dipl"
                            ProviderName="System.Data.SqlClient" SelectCommand="sp_getEnquirySites" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtClient" DefaultValue="" Name="clientName" PropertyName="Text"
                                    Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                        <asp:Button ID="btnOkSite" runat="server" Font-Bold="True" Font-Size="8pt" Height="22px"
                            OnClick="btnOkSite_Click" Style="margin-left: 0px" Text="OK" Width="55px" />
                        <asp:Button ID="btnCancelSite" runat="server" CausesValidation="False" Font-Bold="True"
                            Font-Size="8pt" OnClick="btnCancelSite_Click" Style="margin-left: 11px" Text="Cancel"
                            Width="55px" Height="22px" />
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>   
             
        </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </center>
        <asp:RoundedCornersExtender ID="RoundedCornersExtender2" runat="server" Enabled="True"
            Radius="3" TargetControlID="pnlEnquiry" BorderColor="Black" Color="BurlyWood">
        </asp:RoundedCornersExtender>
</asp:Content>

