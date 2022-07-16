<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_InwardFooter.ascx.cs"
    Inherits="DESPLWEB.Controls.UC_InwardFooter" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<style type="text/css">

.ajax__combobox_buttoncontainer button
{
    background-image: url(mvwres://AjaxControlToolkit, Version=3.5.40412.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e/ComboBox.arrow-down.gif);
    background-position: center;
    background-repeat: no-repeat;
    border-color: ButtonFace;
    height: 15px;
    width: 15px;
}
.ajax__combobox_buttoncontainer button
{
    background-image: url(mvwres://AjaxControlToolkit, Version=3.5.40412.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e/ComboBox.arrow-down.gif);
    background-position: center;
    background-repeat: no-repeat;
    border-color: ButtonFace;
    height: 15px;
    width: 15px;
}
</style>
<table style="width: 97%; height: 10%;">
    <tr>
        <td style="width: 74px;">
            Recieved By
        </td>
        <td>
            :
        </td>
        <td>
            <%--<asp:DropDownList ID="ddlReceivedBy" runat="server" Width="265px">
            </asp:DropDownList>--%>
            <asp:ComboBox ID="cmbReceivedBy" runat="server" DropDownStyle="DropDown" Height="16px" Width="230px">
            </asp:ComboBox>
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
            <asp:LinkButton ID="lnkSave" CssClass="LnkOver" Style="text-decoration: underline;"
                runat="server" onclick="lnkSave_Click">Save</asp:LinkButton>
        </td>
        <td>
            <asp:LinkButton ID="lnkExit" CssClass="LnkOver" Style="text-decoration: underline;"
                runat="server">Exit</asp:LinkButton>
        </td>
    </tr>
</table>
