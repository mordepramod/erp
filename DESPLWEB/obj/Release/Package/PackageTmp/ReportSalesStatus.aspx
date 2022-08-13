﻿<%@ Page Title="Sales Status" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="ReportSalesStatus.aspx.cs" Inherits="DESPLWEB.ReportSalesStatus" %>

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
                        <asp:TextBox ID="txtFromDate" runat="server" Width="120px" OnTextChanged="txtFromDate_TextChanged"
                            AutoPostBack="true"></asp:TextBox>
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
                        <asp:TextBox ID="txtToDate" Width="120px" runat="server" OnTextChanged="txtFromDate_TextChanged"
                            AutoPostBack="true"></asp:TextBox>
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
                    <td>
                        <asp:Label ID="lblME" runat="server" Text="Marketing Person"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlME" Width="200px" runat="server" OnSelectedIndexChanged="ddlME_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblRoute" runat="server" Text="Route"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRoute" Width="200px" runat="server" OnSelectedIndexChanged="ddlRoute_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlRouteList" Width="200px" runat="server" Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="height: 5px">
                        <asp:Label ID="lblDescription" runat="server" Text="" Font-Bold="false" Font-Size="Small"
                            ForeColor="Maroon" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:Label ID="lblTotal" runat="server" Text="" Font-Bold="True" Font-Size="Small"
                            ForeColor="OrangeRed"></asp:Label>
                    </td>
                </tr>
                <%--</table>--%>
                <tr>
                    <td colspan="6" valign="top">
                        <asp:Panel ID="pnlReport" runat="server" ScrollBars="Auto" Height="380px" Width="940px"
                            BorderStyle="Solid" BorderColor="AliceBlue">
                            <asp:GridView ID="grdReport" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False"
                                BackColor="#F7F6F3" CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333"
                                GridLines="Both" Width="100%" CellPadding="0" CellSpacing="0" OnRowCommand="grdReport_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="Category" HeaderText="Category" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="No. of orders received">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkNoOfOrdersReceived" runat="server" Style="text-decoration: underline;"
                                                Text='<%#Eval("NoOfOrdersReceived") %>' CommandName="lnkNoOfOrdersReceived"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value of orders received">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkValueOfOrdersReceived" runat="server" Style="text-decoration: underline;"
                                                Text='<%#Eval("ValueOfOrdersReceived") %>' CommandName="lnkValueOfOrdersReceived"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="OutstandingValue" HeaderText="Outstanding value" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />--%>
                                    <asp:TemplateField HeaderText="Sale">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSale" runat="server" Style="text-decoration: underline;"
                                                Text='<%#Eval("Sale") %>' CommandName="lnkSale"></asp:LinkButton>
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
                                    <asp:TemplateField HeaderText="No of bills modified">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkNoOfBillsModified" runat="server" Style="text-decoration: underline;"
                                                Text='<%#Eval("NoOfBillsModified") %>' CommandName="lnkNoOfBillsModified"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value of bills modified">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkValueOfBillsModified" runat="server" Style="text-decoration: underline;"
                                                Text='<%#Eval("ValueOfBillsModified") %>' CommandName="lnkValueOfBillsModified"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="MoneyReceived" HeaderText="Money received" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="TDS" HeaderText="TDS" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="GST" HeaderText="GST" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="MoneyKeptOnAcc" HeaderText="Money kept on A/c" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="OpenAdvance" HeaderText="Open advance" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />--%>
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

                                                <asp:BoundField DataField="BILL_Id" HeaderText="Bill No." HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="BILL_NetAmt_num" HeaderText="Bill Amount" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}"/>
                                                <asp:BoundField DataField="BILL_Date_dt" HeaderText="Bill Date" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                                <asp:BoundField DataField="BILL_CreatedOn_dt" HeaderText="Created Date" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy hh:mm tt}"/>
                                                <asp:BoundField DataField="BILL_ModifiedOn_dt" HeaderText="Modified Date" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy hh:mm tt}"/>
                                                
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
