<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="ReportDelayReason.aspx.cs" Inherits="DESPLWEB.ReportDelayReason" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <table style="width: 100%">
                <tr>
                    <td colspan="6">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" align="right">
                        Record type
                    </td>
                    <td style="width: 2%">
                        &nbsp;
                    </td>
                    <td style="width: 15%">
                        <asp:DropDownList ID="ddlRecordType" runat="server" Width="110px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 10%" align="right">
                        Reference no.
                    </td>
                    <td style="width: 2%">
                        &nbsp;
                    </td>
                    <td style="width: 50%" valign="top">
                        <asp:TextBox ID="txtReferenceNoPart1" runat="server" Width="100px"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblReferenceNoPart1" runat="server" Text=" / " Font-Bold="true" Font-Size="Large"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtReferenceNoPart2" runat="server" Width="50px"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblReferenceNoPart2" runat="server" Text=" -- " Font-Bold="true" Font-Size="Large"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtReferenceNoPart3" runat="server" Width="50px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Reason for delay
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="txtReason" runat="server" Width="538px"></asp:TextBox>                       
                    </td>
                </tr> 
                <tr>
                    <td colspan="6">&nbsp;
                    </td>
                </tr>               
                <tr>
                    <td colspan="6" align="center">
                        <asp:Label ID="lblReasonMessage" runat="server" ForeColor="#990033" Text="lblMsg"
                            Visible="False"></asp:Label>&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="6" align="center">
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" OnClick="lnkSave_Click"
                            Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V1">Save</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkClear" runat="server" CssClass="LnkOver" OnClick="lnkClear_Click"
                            Style="text-decoration: underline; font-weight: bold;">Clear</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>

</asp:Content>

