<%@ Page Title="Add New Other Test" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="DESPLWEB.Test" %>

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
                    <td colspan="2">
                        <asp:Label ID="Label1" runat="server" Text="Add test in other testing" ForeColor="Brown"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblMaterialType" runat="server" Text="Material Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlInwardType" Width="350px" runat="server">
                        <asp:ListItem Text="Other Testing"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td width="20%">
                    </td>
                    <td width="10%">
                        <asp:Label ID="lblTestName" runat="server" Text="Test Name"></asp:Label>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="txtTestName" runat="server" Width="350px" MaxLength="250"></asp:TextBox>
                        <br />&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTestName"
                            EnableClientScript="False" ErrorMessage="Input 'Test Name'" SetFocusOnError="True"
                            ValidationGroup="V1"></asp:RequiredFieldValidator>
                    </td>
                    <td width="20%">
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkAddTest" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkAddTest_Click" Font-Bold="True" ValidationGroup ="V1">Add Test</asp:LinkButton>
                            &nbsp;&nbsp;
                        <%--<asp:LinkButton ID="lnkExit" runat="server" OnClick="lnkExit_Click" Visible="false"
                            CssClass="LnkOver" Style="text-decoration: underline;">Exit</asp:LinkButton>--%>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="Label2" runat="server" Text="Add sub test in other testing" ForeColor="Brown"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblTest" runat="server" Text="Test"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTest" Width="350px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblSubTestName" runat="server" Text="Sub Test Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubTestName" runat="server" Width="350px" MaxLength="250"></asp:TextBox>
                        <br />&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSubTestName"
                            EnableClientScript="False" ErrorMessage="Input 'Sub Test Name'" SetFocusOnError="True"
                            ValidationGroup="V2"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td width="20%">
                    </td>
                    <td width="10%">
                        <asp:Label ID="lblSubTestRate" runat="server" Text="Sub Test Rate"></asp:Label>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="txtSubTestRate" runat="server" Width="350px" MaxLength="250" onchange="checkNum(this)"></asp:TextBox>
                        <br />&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSubTestRate"
                            EnableClientScript="False" ErrorMessage="Input 'Sub Test Rate'" SetFocusOnError="True"
                            ValidationGroup="V2"></asp:RequiredFieldValidator>
                    </td>
                    <td width="20%">
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkAddSubTest" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkAddSubTest_Click" Font-Bold="True" ValidationGroup ="V2">Add Sub Test</asp:LinkButton>
                            &nbsp;&nbsp;
                        <%--<asp:LinkButton ID="lnkExit" runat="server" OnClick="lnkExit_Click" Visible="false"
                            CssClass="LnkOver" Style="text-decoration: underline;">Exit</asp:LinkButton>--%>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="4" align="center">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="#990033" Text="lblMessage" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>

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
</asp:Content>

