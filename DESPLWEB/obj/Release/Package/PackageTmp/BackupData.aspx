<%@ Page Title="Backup" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="BackupData.aspx.cs" Inherits="DESPLWEB.BackupData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 460px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="460px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <table style="width: 100%">
                <tr>
                    <td colspan="2">
                    &nbsp; 
                    </td>
                </tr> 
                <tr>
                    <td colspan="2">
                    &nbsp; 
                    </td>
                </tr>
                <tr>
                    <td style="width:90%">
                        <asp:TextBox ID="txtBackup" runat="server" MaxLength="250" Width="718px"
                            ></asp:TextBox>
                        &nbsp;
                        <asp:LinkButton ID="lnkBackup" runat="server" CssClass="LnkOver" OnClick="lnkBackup_Click"
                            Style="text-decoration: underline;" ValidationGroup="V1" Font-Bold="True">Start Backup</asp:LinkButton>
                    </td>
                    <td> &nbsp;
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td colspan="2">
                    &nbsp;                                
                        <asp:Label ID="lblMessage" runat="server" ForeColor="#990033" Text="lblMessage" Visible="False"></asp:Label>
                    </td>                    
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
</asp:Content>
