<%@ Page Title="Generate Bill for Approved Reports" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="BillGenerationForApprovedRpt.aspx.cs" Inherits="DESPLWEB.BillGenerationForApprovedRpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td width="90%">
                        <asp:LinkButton ID="lnkLoadPendingRptList" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkLoadPendingRptList_Click" Font-Bold="True">Display </asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTotalRecords" runat="server" Text=""></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 10px">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="pnlPendingReportList" runat="server" ScrollBars="Auto" Height="420px"
                            Width="940px" BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="0" GroupingText="">
                            <div style="width: 940px;">
                                <div id="GHead">
                                </div>
                                <div style="height: 420px; overflow: auto">
                                    <asp:GridView ID="grdReport" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False"
                                    OnRowCommand="grdReport_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px" />
                                            <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px" />
                                            <asp:BoundField DataField="INWD_RecordType_var" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" />
                                            <asp:BoundField DataField="INWD_RecordNo_int" HeaderText="Record No." HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" />
                                            <asp:BoundField DataField="INWD_ReferenceNo_int" HeaderText="Reference No." HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" />
                                            <asp:BoundField DataField="INWD_ReceivedDate_dt" HeaderText="Received Date" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="90px" />
                                            <%--<asp:BoundField DataField="MISApprovedDt" HeaderText="Approved Date" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="90px" />--%>
                                            <asp:BoundField DataField="CL_MonthlyBilling_bit" HeaderText="Monthly Status" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px" />
                                            <asp:TemplateField >
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkGenerateBill" runat="server" ToolTip="Generate Bill" Style="text-decoration: underline;"
                                                        CommandArgument='<%#Eval("INWD_RecordType_var") + ";" + Eval("INWD_RecordNo_int") + ";" + Eval("INWD_ReferenceNo_int") %>'
                                                        CommandName="GenerateBill">Generate Bill</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    
</asp:Content>
