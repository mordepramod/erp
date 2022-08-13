<%@ Page Title="Bill Wise Report Status" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="BillWiseReportStatus.aspx.cs" Inherits="DESPLWEB.BillWiseReportStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="100%">
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date "></asp:Label>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="120px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="lblToDate" runat="server" Text="To Date "></asp:Label>
                        <asp:TextBox ID="txtToDate" Width="120px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkCRLimitApprPending" Text="Report pending for CR Limit Approval" runat="server" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkDisplay" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkDisplay_Click" Font-Bold="True">Display</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 5px">
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Label ID="lblTotal" runat="server" Text="" Font-Bold="True" Font-Size="Small"
                            ForeColor="OrangeRed"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlBillList" runat="server" ScrollBars="Auto" Height="400px" Width="940px"
                BorderStyle="Solid" BorderColor="AliceBlue">
                <asp:GridView ID="grdBillList" skinId="gridviewSkin1" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                    CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                    Width="100%" CellPadding="0" CellSpacing="0">
                    <Columns>
                        <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="SiteName" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="CRLimit" HeaderText="CR Limit" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BalanceAmt" HeaderText="Balance Amount" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BillNo" HeaderText="Bill No." HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="BillDate" HeaderText="Bill Date" 
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" /> <%--DataFormatString="{0:dd/MM/yyyy}"--%>
                        <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="BillModifiedDate" HeaderText="Modified Date"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BillRecordType" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" />
                        <asp:BoundField DataField="BillRecordNo" HeaderText="Record No." HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" />
                        <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No." HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="ApprovedDate" HeaderText="Approved Date" 
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ReportStatus" HeaderText="Report Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </asp:Panel>
       <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    
</asp:Content>

