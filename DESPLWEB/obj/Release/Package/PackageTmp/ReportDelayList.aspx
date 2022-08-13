<%@ Page Title="Delay Report List" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="ReportDelayList.aspx.cs" Inherits="DESPLWEB.ReportDelayList" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr>
                    <td align="right">
                        <asp:LinkButton ID="lnkFetch" runat="server" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkFetch_Click" Visible="false">Display</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkPrint_Click">Print</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total Records : 0 "></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;&nbsp;
                        <asp:CheckBox ID="chkPending" Text="Pending" runat="server" AutoPostBack="true" OnCheckedChanged="chkPending_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 23px" valign="top">
                        <asp:Panel ID="pnlReports" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="400px"
                            Width="940px" BorderColor="AliceBlue">
                            <asp:GridView ID="grdReports" runat="server" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2"
                                ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="White"
                                CssClass="Grid" BorderWidth="1px" Width="100%" OnRowCommand="grdReports_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ALERTDLRPT_RecordType_var" HeaderText="Record_Type" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ALERTDLRPT_ReferenceNo_var" HeaderText="Reference_No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ALERTDLRPT_RecordNo_int" HeaderText="Record_No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ALERTDLRPT_ReceivedDate_dt" HeaderText="Received_Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="ALERTDLRPT_TargetDate_dt" HeaderText="Target_Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="ALERTDLRPT_DelayInDays_int" HeaderText="Delay_In_Days" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ALERTDLRPT_CL_Name_var" HeaderText="Client_Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ALERTDLRPT_SITE_Name_var" HeaderText="SITE_Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ALERTDLRPT_ReportStatus_var" HeaderText="Report_Status" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ALERTDLRPT_Reason_var" HeaderText="Reason_for_Delay" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField HeaderText="Update Reason">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReason" runat="server" Width="150px" Text=''></asp:TextBox> <%--Text='<%# Eval("ALERTDLRPT_Reason_var") %>'--%>
                                            <asp:label ID="lblPending" runat="server" Visible="false" Text='<%# Eval("ALERTDLRPT_Pending_bit") %>'></asp:label> <%----%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkUpdateReason" runat="server" ToolTip="Update Reason"
                                                Style="text-decoration: underline;" CommandName="UpdateReason">Update Reason</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#CCCC99" />
                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                <EmptyDataTemplate>
                                    No records to display
                                </EmptyDataTemplate>
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
