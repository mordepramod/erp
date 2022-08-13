<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="CollectionReport.aspx.cs" Inherits="DESPLWEB.CollectionReport" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td colspan="2" width="100%">
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
                    <td align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:RadioButton ID="optCollection" Text="Collection Report" runat="server" GroupName="g1"
                            OnCheckedChanged="opt_CheckedChanged" AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optAsOnOpenAdvance" Text="As On Open Advance" runat="server"
                            GroupName="g1" OnCheckedChanged="opt_CheckedChanged" AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optPrevAdjAdvance" Text="Previous Adjusted Advance" runat="server"
                            GroupName="g1" OnCheckedChanged="opt_CheckedChanged" AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optOpenAdvanceAging" Text="As On Open Advance Report" runat="server"
                            GroupName="g1" OnCheckedChanged="opt_CheckedChanged" AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optOpenAdvanceAgingNew" Text="Open Advance Aging" runat="server"
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
                        <asp:Panel ID="pnlCollection" runat="server" ScrollBars="Auto" Height="100px" Width="100%"
                            BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">
                            <asp:GridView ID="grdCollection" SkinID="gridviewSkin" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="RowName" HeaderText=" " HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="Amount" HeaderText="Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="LedgerId" HeaderText="Ledger Id" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="LedgerName" HeaderText="Ledger Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" ItemStyle-Width="300px" />
                                    <asp:BoundField DataField="BalanceAmount" HeaderText="Balance Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="AdvanceReceivedFrom" HeaderText="Advance Received From"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="AdvanceDate" HeaderText="Advance Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BillNo" HeaderText="Bill No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BillDate" HeaderText="Bill Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="NoteNo" HeaderText="Note No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="AdjustedDate" HeaderText="Adjusted Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="AdjustedAmount" HeaderText="Adjusted Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="LedgerName" HeaderText="Ledger Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" ItemStyle-Width="300px" />
                                    <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ReceiptAmount" HeaderText="Receipt Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BalanceAmount" HeaderText="Balance Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Age" HeaderText="Age (days)" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                  
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkViewReport" runat="server" ToolTip="View Report" Style="text-decoration: underline;"
                                                OnClick="lnkViewReport" Width="50px">View</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="LedgerNameAdvance" HeaderText="Ledger Name" HeaderStyle-HorizontalAlign="Center" 
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="0to30" HeaderText="0 - 30 day" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="31to60" HeaderText="31 - 60 day" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="61to90" HeaderText="61 - 90 day" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="91to120" HeaderText="91 - 120 day " HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="121to180" HeaderText="121 - 180 Day" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="181to270" HeaderText="181 - 270 day" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="271to365" HeaderText="271 - 365 day" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="366to730" HeaderText="366 - 730 day" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="MoreThan731" HeaderText="More than 731 day" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="GrandTotal" HeaderText="Grand Total" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                       <asp:BoundField DataField="Note" HeaderText="Note" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Panel ID="pnlSort" runat="server" Height="5px" Width="940px" Visible="false">
                            <asp:Label ID="lblClient" runat="server" Text="Client"></asp:Label>&nbsp;
                            <asp:DropDownList ID="ddlClient" Width="150px" runat="server" OnSelectedIndexChanged="ddl_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblTestType" runat="server" Text="Test Type"></asp:Label>&nbsp;
                            <asp:DropDownList ID="ddlTestType" Width="150px" runat="server" OnSelectedIndexChanged="ddl_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblCollectedBy" runat="server" Text="Collected By"></asp:Label>&nbsp;
                            <asp:DropDownList ID="ddlCollectedBy" Width="150px" runat="server" OnSelectedIndexChanged="ddl_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblRegion" runat="server" Text="Region"></asp:Label>&nbsp;
                            <asp:DropDownList ID="ddlRegion" Width="150px" runat="server" OnSelectedIndexChanged="ddl_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </asp:Panel>
                        <div id="div1" style="float: right" visible="false" runat="server">
                            <asp:LinkButton ID="lnkgrdPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                                OnClick="lnkgrdPrint_Click" Font-Bold="True">Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Label ID="lblDetailHeading" runat="server" Text="" Font-Bold="true" ForeColor="Brown"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlCollectionDetails" runat="server" ScrollBars="None" Height="200px"
                Width="940px" BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1" Visible="false">
                <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 200px; overflow: auto">
                        <asp:GridView ID="grdCollDetails" SkinID="gridviewSkin1" runat="server" BackColor="#F7F6F3"
                            CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Horizontal"
                            Width="100%" CellPadding="0" CellSpacing="0">
                            <Columns>
                                <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="BillNo" HeaderText="Bill No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BillingDate" HeaderText="Billing Date" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="CollectionDate" HeaderText="Collection Date" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="TestingType" HeaderText="Testing Type" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CollectedBy" HeaderText="Collected By" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Amount" HeaderText="Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Region" HeaderText="Region" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="MktUser" HeaderText="Mkt User" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                 <asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AdvReceiptNo" HeaderText="Adv Receipt No." HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="LedgerName" HeaderText="Ledger Name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BalanceAmount" HeaderText="Balance Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                 <asp:BoundField DataField="Note" HeaderText="Note" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                               <asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No." HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ReceiptAmount" HeaderText="Receipt Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TDSAmount" HeaderText="TDS Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReceiptDate" HeaderText="Receipt Date" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReceiptNo" HeaderText="Receipt No." HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReceiptAmount" HeaderText="Receipt Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BalanceAmount" HeaderText="Balance Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AdjustedDate" HeaderText="Adjusted Date" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NoteNo" HeaderText="Note No." HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BillNo" HeaderText="Bill No." HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AdjustedAmount" HeaderText="Adjusted Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                 <asp:BoundField DataField="Note" HeaderText="Note" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
    </div>
  <%--  <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdCollDetails.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdCollDetails.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdCollDetails.ClientID%>').offset().top);

        });
    </script>--%>
</asp:Content>
