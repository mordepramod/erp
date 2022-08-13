<%@ Page Title="Outstanding Recovery Report" Language="C#" MasterPageFile="~/MstPg_Veena.Master" Theme="duro"
    AutoEventWireup="true" CodeBehind="OutstandingRecoveryReport.aspx.cs" Inherits="DESPLWEB.OutstandingRecoveryReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="100%">
                        <asp:Label ID="lblFromDate" runat="server" Text="As on Date "></asp:Label>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="120px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="lblToDate" runat="server" Text="Pending till Date "></asp:Label>
                        <asp:TextBox ID="txtToDate" Width="120px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;&nbsp;&nbsp;                        
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td colspan="3">
                        <asp:LinkButton ID="lnkDisplay" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkDisplay_Click" Font-Bold="True">Display</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;<asp:Label ID="lblOutstanding" runat="server" Text="" Font-Size="Small" Visible="false"></asp:Label>
                        &nbsp;<asp:Label ID="lblTotal" runat="server" Text="" Font-Bold="True" Font-Size="Small"
                            ForeColor="OrangeRed"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 5px">
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Panel ID="pnlBillList" runat="server" ScrollBars="Auto" Height="390px" Width="950px"
                            BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">
                            <asp:GridView ID="grdOutstanding" SkinID="gridviewSkin" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SiteName" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BillNo" HeaderText="Bill No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BillDate" HeaderText="Bill Date" DataFormatString="{0:dd/MM/yyyy}"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="TestingType" HeaderText="Testing Type" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount " HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="PendingAmount" HeaderText="As On Pending Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="PendingAmountTillDate" HeaderText="Till Date Pending Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />

                                    <%--<asp:BoundField DataField="DaysPending" HeaderText="Days Pending" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="MktUser" HeaderText="Mkt User" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Region" HeaderText="Region" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Limit" HeaderText="Limit" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="TotalOutstading" HeaderText="Total Outstading" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />--%>

                                    <asp:BoundField DataField="ReceivedAmount" HeaderText="Received Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />
                                    <%--<asp:BoundField DataField="OnAccount" HeaderText="On A/c" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />--%>

                                    <asp:BoundField DataField="SalesReversal" HeaderText="Sales Reversal" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="Discount" HeaderText="Discount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="BadDebts" HeaderText="Bad Debts" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />
                                    <%--<asp:BoundField DataField="CGST" HeaderText="CGST" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="SGST" HeaderText="SGST" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />--%>
                                    <asp:BoundField DataField="TDS" HeaderText="TDS" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />
                                    <%--<asp:BoundField DataField="ServiceTax" HeaderText="Service Tax" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />--%>
                                    <asp:BoundField DataField="Other" HeaderText="Other" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
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
