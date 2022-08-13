<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportForClient.aspx.cs" Inherits="DESPLWEB.ReportForClient" Theme="duro"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="stylized" class="myform">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>  
        <asp:Panel ID="pnlContent" runat="server" Width="600px" BorderWidth="0px" Height="270px"
            Style="background-color: #ECF5FF;">
            <table width="100%" style="border-color: #800000">
                <tr>
                    <td colspan="3" style="height:50px" align="center">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td style="width:2%">
                    </td>
                    <td>
                        <asp:Label ID="lblEnqId" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblRecType" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblRecNo" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" Visible="false"/>
                    </td>
                </tr> 
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Client Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Client" runat="server" Width="420px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                    </td>
                    
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
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
                    
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>                        
                    </td>
                    <td>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:HiddenField ID="hfSiteId" runat="server" />
                    </td>
                    
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    
                    <td align="right">
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" OnClick="lnkSave_Click"
                            Style="text-decoration: underline; font-weight: bold;">Save</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkExit" runat="server" CssClass="LnkOver" OnClick="lnk_Exit_Click"
                            Style="text-decoration: underline; font-weight: bold;" Visible="false">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        
    </div>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
        function SiteItemSelected(sender, e) {
            $get("<%=hfSiteId.ClientID %>").value = e.get_value();
        }

    </script>
    </form>
</body>
</html>
