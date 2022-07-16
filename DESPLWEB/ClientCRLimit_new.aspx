<%@ Page Title="Client Credit Limit Setting" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="ClientCRLimit_new.aspx.cs" Inherits="DESPLWEB.ClientCRLimit_new" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblRight" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td width="7%">
                        <asp:Label ID="lblClient" runat="server" Text="Client"></asp:Label>
                    </td>
                    <td style="width: 32%">
                        <asp:TextBox ID="txt_Client" runat="server" Width="295px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td width="10%">
                        
                    </td>
                    <td style="width: 15%">
                        <asp:Label ID="lblCrLimit" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="width: 15%">
                        <asp:Label ID="lblBalance" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="6">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:CheckBox ID="chkByPassCreditLimit" Text="By Pass Credit Limit" runat="server"/>
                    </td>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblReason" runat="server" Text="Reason" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtReason" runat="server" Width="295px" Visible="false" MaxLength="255"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSave" Font-Size="Small" runat="server" OnClick="lnkSave_Click"
                            Font-Underline="true">Save</asp:LinkButton>&nbsp;&nbsp;
                    </td>
                    <td>                        
                        <asp:LinkButton ID="lnk1daybyPass" Font-Size="Small" runat="server" OnClick="lnk1daybyPass_Click"
                            Font-Underline="true">Appove for a Day</asp:LinkButton>&nbsp;&nbsp;

                        <asp:LinkButton ID="lnkExit" runat="server" Font-Size="Small" Font-Underline="true"
                            Visible="false" OnClick="lnkExit_Click">Exit</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="5">
                        <asp:Button ID="btnBlockClient" runat="server" Text="Block Client" Visible="false" OnClick="btnBlockClient_Click"/>
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
