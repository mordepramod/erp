<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="Other_ReportSection.aspx.cs" Inherits="DESPLWEB.Other_ReportSection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px">
            <table style="width: 100%">
                <tr>
                    <td colspan="6">
                    </td> 
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>  
                <tr>
                    <td style="width: 5%">
                        &nbsp;
                    </td>
                    <td style="width: 10%">
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                    </td>
                    <td style="width: 30%">
                        <asp:TextBox ID="txtFromDate" Width="190px" runat="server" AutoPostBack="true" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td style="width: 10%">
                        <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                    </td>
                    <td style="width: 25%">
                        <asp:TextBox ID="txtToDate" Width="190px" runat="server" AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td style="width: 10%">
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>    
                    <td>
                        &nbsp;
                    </td>                
                    <td>
                        <asp:Label ID="lblReferenceNo" runat="server" Text="Reference No."></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlReferenceNo" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlReferenceNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSection" runat="server" Text="Section" ></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSection" runat="server" Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>                
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>                
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="5" align="right">
                        <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkSave_Click" Font-Bold="True">Save</asp:LinkButton>
                        &nbsp; &nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
</asp:Content>
