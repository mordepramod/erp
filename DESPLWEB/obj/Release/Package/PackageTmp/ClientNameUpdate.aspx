<%@ Page Title="Client Name Updation" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="ClientNameUpdate.aspx.cs" Inherits="DESPLWEB.ClientNameUpdate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <table width="100%">
                <tr valign="top">
                    <td colspan="3" align="center">
                        &nbsp;
                    </td>
                    <td width="20%" align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblClient" runat="server" Text="Client"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlClient" runat="server" Width="350px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td width="20%" align="right">
                    </td>
                </tr>
                 <tr>
                    <td colspan="4">
                    &nbsp;
                    </td>
                </tr>
                <tr>
                    <td width="20%">
                    </td>
                    <td width="10%">
                        <asp:Label ID="lblNewClientName" runat="server" Text="New Client Name"></asp:Label>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="txtNewClientName" runat="server" Width="350px" MaxLength="250"></asp:TextBox>
                        <br />
                        &nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNewClientName"
                            EnableClientScript="False" ErrorMessage="Input 'Client Name'" SetFocusOnError="True"
                            ValidationGroup="V1"></asp:RequiredFieldValidator>
                    </td>
                    <td width="20%" align="right">
                    </td>
                </tr>
                
                <tr valign="top">
                    <td colspan="4" align="center">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="#990033" Text="lblMessage" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2" align="center">
                    </td>
                    <td style="text-align: right" valign="top">
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkSave_Click" Font-Bold="True" ValidationGroup="V1">Save</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkExit" runat="server" OnClick="lnkExit_Click" Visible="false"
                            CssClass="LnkOver" Style="text-decoration: underline;">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
</asp:Content>