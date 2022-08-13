<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="CashBook.aspx.cs" Inherits="DESPLWEB.CashBook" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="50%">
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>&nbsp;&nbsp;
                        <asp:TextBox ID="txtFromDate" Width="120px" runat="server"></asp:TextBox>&nbsp;&nbsp;
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;
                        <asp:Label ID="lblToDate" runat="server" Text="To Date "></asp:Label>&nbsp;&nbsp;
                        <asp:TextBox ID="txtToDate" Width="120px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td width="30%">
                        <asp:TextBox ID="txtBillLockDate" Width="120px" runat="server"></asp:TextBox>&nbsp;&nbsp;
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtBillLockDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txtBillLockDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkUpdateBillLockDate" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkUpdateBillLockDate_Click" Font-Bold="True" Enabled="false">Update Bill Lock Date</asp:LinkButton>
                    </td>
                    <td align="right" width="20%" >
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td colspan="2">
                        <asp:RadioButton ID="optDebtorList" Text="Debtor List" runat="server" GroupName="g1"
                            OnCheckedChanged="opt_CheckedChanged" AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optSummary" Text="Summary" runat="server"
                            GroupName="g1" OnCheckedChanged="opt_CheckedChanged" AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optCashBook" Text="Cash Book" runat="server"
                            GroupName="g1" OnCheckedChanged="opt_CheckedChanged" AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optSalesRegister" Text="Sales Register" runat="server"
                            GroupName="g1" OnCheckedChanged="opt_CheckedChanged" AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optBRMovement" Text="BR Movement" runat="server"
                            GroupName="g1" OnCheckedChanged="opt_CheckedChanged" AutoPostBack="true" />
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkDisplay" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkDisplay_Click" Font-Bold="True">Display</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 5px">
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Panel ID="pnlCashBook" runat="server" ScrollBars="Auto" Height="200px" Width="940px"
                            BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">
                    <%--          <div style="width: 100%;">
                    <div id="GHead">
                    </div>
                    <div style="height: 200px; overflow: auto">--%>
                            <asp:GridView ID="grdCashBook" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                            CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Horizontal"
                            Width="100%" CellPadding="0" CellSpacing="0">
                                <Columns>
                                    <asp:BoundField DataField="SrNo" HeaderText=" " HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="ClientId" HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left"  />
                                    <asp:BoundField DataField="DebitBalance" HeaderText="Debit Balance" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px"  />
                                    <asp:BoundField DataField="CreditBalance" HeaderText="Credit Balance" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" />

                                    <asp:BoundField DataField="Date" HeaderText="Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="DebitAmount" HeaderText="Debit Amount"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CreditAmount" HeaderText="Credit Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BalanceAmount" HeaderText="Balance Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />

                                    <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="ClientId" HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="OpeningBalance" HeaderText="Opening Balance" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px" />
                                    <asp:BoundField DataField="DebitAmount" HeaderText="Debit Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px" />
                                    <asp:BoundField DataField="CreditAmount" HeaderText="Credit Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ClosingBalance" HeaderText="Closing Balance" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                                                           
                                    <asp:BoundField DataField="ClientId" HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Limit" HeaderText="Limit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Balance" HeaderText="Balance" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />

                                    <asp:BoundField DataField="Coupon" HeaderText="Coupon" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="AAC" HeaderText="AAC" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="AGGT" HeaderText="AGGT" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BT-" HeaderText="BT-" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CCH" HeaderText="CCH" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CEMT" HeaderText="CEMT" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CORECUT" HeaderText="CORECUT" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CR" HeaderText="CR" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CT" HeaderText="CT" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="FLYASH" HeaderText="FLYASH" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="GT" HeaderText="GT" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="MF" HeaderText="MF" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="NDT" HeaderText="NDT" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="OT" HeaderText="OT" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="PILE" HeaderText="PILE" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="PT" HeaderText="PT" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="RWH" HeaderText="RWH" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SO" HeaderText="SO" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SOLID" HeaderText="SOLID" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ST" HeaderText="ST" HeaderStyle-HorizontalAlign="Center" 
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                    <asp:BoundField DataField="STC" HeaderText="STC" HeaderStyle-HorizontalAlign="Center" 
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                    <asp:BoundField DataField="TILE" HeaderText="TILE" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="WT" HeaderText="WT" HeaderStyle-HorizontalAlign="Center" 
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />

                                    <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ClientId" HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BillNo" HeaderText="Bill No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BillingDate" HeaderText="Billing Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CollectionDate" HeaderText="Collection Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="TestingType" HeaderText="Testing type" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CollectedBy" HeaderText="Collected By" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Amount" HeaderText="Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Region" HeaderText="Region" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="MktUser" HeaderText="Mkt User" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Advance" HeaderText="Advance" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkViewReport" runat="server" ToolTip="View Report" Style="text-decoration: underline;"
                                                OnClick="lnkViewReport" Width="40px">View</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                           <%-- </div></div>--%>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Panel ID="pnlCashDetails" runat="server" ScrollBars="Auto" Height="15px"
                            Width="940px" BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1" Visible="false">
                            <asp:Label ID="lblDetailHeading" runat="server" Text="Label" Font-Bold="true" ForeColor="Brown"></asp:Label>                            
                            <div style="float: right">
                                <asp:LinkButton ID="lnkgrdPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                                    OnClick="lnkgrdPrint_Click" Font-Bold="True">Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlCashDetails1" runat="server" ScrollBars="Auto" Height="190px" Width="940px"
                          BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1" Visible="false">    
                           <%--  <div style="width: 940px;">
                    <div id="GHead1">
                    </div>
                    <div style="height: 200px; overflow: auto">      --%>                
                            <asp:GridView ID="grdCashDetails" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                            CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Horizontal"
                            Width="940px" CellPadding="0" CellSpacing="0">
                                <Columns>
                                    <asp:BoundField DataField="Date" HeaderText="Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                    <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BillNo" HeaderText="Bill No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="NoteNo" HeaderText="Note No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Settlement" HeaderText="Settlement" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Debit" HeaderText="Debit" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Credit" HeaderText="Credit" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BalanceAmount" HeaderText="Balance Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />                                    
                                </Columns>
                            </asp:GridView>
                          <%--  </div></div>--%>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
     <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdCashBook.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdCashBook.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdCashBook.ClientID%>').offset().top);

        });
    </script>
     <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdCashDetails.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdCashDetails.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead1").append(gridHeader);
            $('#GHead1').css('position', 'absolute');
            $('#GHead1').css('top', $('#<%=grdCashDetails.ClientID%>').offset().top);

        });
    </script>
</asp:Content>
