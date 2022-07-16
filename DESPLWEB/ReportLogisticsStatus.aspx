<%@ Page Title="Logistics Status" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="ReportLogisticsStatus.aspx.cs" Inherits="DESPLWEB.ReportLogisticsStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="12%">
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date "></asp:Label>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtFromDate" runat="server" Width="120px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                    </td>
                    <td width="10%">
                        <asp:Label ID="lblToDate" runat="server" Text="To Date "></asp:Label>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtToDate" Width="120px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td width="30%">
                        <asp:LinkButton ID="lnkDisplay" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkDisplay_Click" Font-Bold="True">Display</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkViewDescription" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkViewDescription_Click" Font-Bold="True">View Description</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="height: 5px">
                        <asp:Label ID="lblDescription" runat="server" Text="" Font-Bold="false" Font-Size="Small"
                            ForeColor="Maroon" Visible="false"></asp:Label>
                    </td>
                </tr>
                <%--</table>--%>
                <tr>
                    <td colspan="6" valign="top">
                        <asp:Panel ID="pnlReport" runat="server" ScrollBars="Auto" Height="400px" Width="940px"
                            BorderStyle="Solid" BorderColor="AliceBlue">
                            <asp:GridView ID="grdReport" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False"
                                BackColor="#F7F6F3" CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333"
                                GridLines="Both" Width="100%" CellPadding="0" CellSpacing="0" OnRowCommand="grdReport_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="Category" HeaderText="Category" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <%--<asp:BoundField DataField="NoOfOrdersReceived" HeaderText="No. of orders received"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="MaterialCollected" HeaderText="Material collected" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ReportsPrinted" HeaderText="Reports printed" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ReportPendingForPrintingDueToCRL" HeaderText="Report pending for printing due to credit limit"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ReportsOutward" HeaderText="Reports outward" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ReportsReceivedByClient" HeaderText="Reports received by client"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" /> -phy. outward---%>
                                    <asp:TemplateField HeaderText="No. of orders received">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkNoOfOrdersReceived" runat="server" Style="text-decoration: underline;"
                                                Text='<%#Eval("NoOfOrdersReceived") %>' CommandName="lnkNoOfOrdersReceived"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Material collected">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkMaterialCollected" runat="server" Style="text-decoration: underline;"
                                                Text='<%#Eval("MaterialCollected") %>' CommandName="lnkMaterialCollected"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reports printed">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkReportsPrinted" runat="server" Style="text-decoration: underline;"
                                                Text='<%#Eval("ReportsPrinted") %>' CommandName="lnkReportsPrinted"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Report pending for printing due to credit limit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkReportPendingForPrintingDueToCRL" runat="server" Style="text-decoration: underline;"
                                                Text='<%#Eval("ReportPendingForPrintingDueToCRL") %>' CommandName="lnkReportPendingForPrintingDueToCRL"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reports outward">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkReportsOutward" runat="server" Style="text-decoration: underline;"
                                                Text='<%#Eval("ReportsOutward") %>' CommandName="lnkReportsOutward"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reports received by client">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkReportsReceivedByClient" runat="server" Style="text-decoration: underline;"
                                                Text='<%#Eval("ReportsReceivedByClient") %>' CommandName="lnkReportsReceivedByClient"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" valign="top">
                        <asp:Panel ID="pnlDetails" runat="server" ScrollBars="Auto" Height="400px" Width="940px"
                            BorderStyle="Solid" BorderColor="AliceBlue" Visible="false">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDetail" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:ImageButton ID="imgCloseDetailPopup" runat="server" ImageUrl="Images/cross_icon.png"
                                            OnClick="imgCloseDetailPopup_Click" ImageAlign="Right" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="grdDetails" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False"
                                            BackColor="#F7F6F3" CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333"
                                            GridLines="Both" Width="100%" CellPadding="0" CellSpacing="0" OnRowDataBound="grdDetails_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr. No.">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="ENQ_Id" HeaderText="Enquiry No." HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ENQ_Date_dt" HeaderText="Enquiry Date" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                                <asp:BoundField DataField="MATERIAL_RecordType_var" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ENQ_OpenEnquiryStatus_var" HeaderText="Enquiry Type" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ENQ_Status_tint" HeaderText="Enquiry Status" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ENQ_ApproveDate_dt" HeaderText="Approved On" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                                <asp:BoundField DataField="ENQ_CollectedOn_dt" HeaderText="Collected On" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                                <asp:BoundField DataField="ENQ_Note_var" HeaderText="Note" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ENQ_Reference_var" HeaderText="Reference" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ENQ_CloseComment" HeaderText="Close Comment" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />

                                                <asp:BoundField DataField="Proposal_Id" HeaderText="Proposal No." HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Proposal_Date" HeaderText="Proposal Date" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                                <asp:BoundField DataField="Proposal_OrderNo_var" HeaderText="Proposal Order No." HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Proposal_OrderDate_dt" HeaderText="Proposal Order Date" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                                <asp:BoundField DataField="Proposal_OrderValue_dec" HeaderText="Order value" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />

                                                <asp:BoundField DataField="INWD_RecordNo_int" HeaderText="Record No." HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="MISRefNo" HeaderText="Reference No." HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center"/>
                                                <asp:BoundField DataField="MISApprovedDt" HeaderText="Approved Date" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                                <asp:BoundField DataField="CL_BalanceAmt_mny" HeaderText="Outstanding Balance" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}"/>
                                                <asp:BoundField DataField="CL_Limit_mny" HeaderText="Credit Limit" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}"/>
                                                <asp:BoundField DataField="MISIssueDt" HeaderText="Issue Date" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="MISOutwardDt" HeaderText="Outward Date" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                                <asp:BoundField DataField="MISPhysicalOutwardDt" HeaderText="Phy. Outward Date" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
</asp:Content>
