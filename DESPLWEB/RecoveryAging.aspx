<%@ Page Title="Recovery Aging" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="RecoveryAging.aspx.cs" Inherits="DESPLWEB.RecoveryAging" Theme="duro"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="100%">
                        <asp:Label ID="lblToDate" runat="server" Text="As On Date "></asp:Label>&nbsp;&nbsp;
                        <asp:TextBox ID="txtToDate" Width="120px" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
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
                    <td colspan="3" style="height: 5px"></td>
                </tr>
            </table>
            <asp:Panel ID="pnlAging" runat="server" ScrollBars="Auto" Height="430px" Width="940px"
                BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">
                <%--<div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 340px; overflow: auto">--%>
                        <asp:GridView ID="grdRecovery" SkinID="gridviewSkin1" runat="server"
                            ShowFooter="true" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="PendingDays" HeaderText=" " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SubmittedNos" HeaderText="Submitted Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SubmittedAmount" HeaderText="Submitted Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NotReceivedAtSiteNos" HeaderText="Not received at site Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NotReceivedAtSiteAmount" HeaderText="Not received at site Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReceivedAtSiteNos" HeaderText="Received at site Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReceivedAtSiteAmount" HeaderText="Received at site Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NotPostedInAccountsNos" HeaderText="Not posted in accounts Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NotPostedInAccountsAmount" HeaderText="Not posted in accounts Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PostedInAccountsNos" HeaderText="Posted in Accounts Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PostedInAccountsAmount" HeaderText="Posted in Accounts Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AckNotAvailableNos" HeaderText="Ack not available Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AckNotAvailableAmount" HeaderText="Ack not available Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UnderReconciliationNos" HeaderText="Under Reconciliation Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UnderReconciliationAmount" HeaderText="Under Reconciliation Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BlanksNos" HeaderText="Blanks Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BlanksAmount" HeaderText="Blanks Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TotalNos" HeaderText="Total Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />                                
                            </Columns>
                        </asp:GridView>
                    <%--</div>
                </div>--%>
                <div style="height: 20px">
                    </div>
                <%--<div style="width: 940px;">
                    <div id="GHead2">
                    </div>
                    <div style="height: 150px; overflow: auto">--%>
                        <asp:GridView ID="grdRecovery2" SkinID="gridviewSkin1" runat="server"
                            ShowFooter="true" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="PendingDays" HeaderText=" " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SubmittedNos" HeaderText="Submitted Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SubmittedAmount" HeaderText="Submitted Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NotReceivedAtSiteNos" HeaderText="Not received at site Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NotReceivedAtSiteAmount" HeaderText="Not received at site Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReceivedAtSiteNos" HeaderText="Received at site Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReceivedAtSiteAmount" HeaderText="Received at site Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NotPostedInAccountsNos" HeaderText="Not posted in accounts Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NotPostedInAccountsAmount" HeaderText="Not posted in accounts Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PostedInAccountsNos" HeaderText="Posted in Accounts Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PostedInAccountsAmount" HeaderText="Posted in Accounts Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AckNotAvailableNos" HeaderText="Ack not available Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AckNotAvailableAmount" HeaderText="Ack not available Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UnderReconciliationNos" HeaderText="Under Reconciliation Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UnderReconciliationAmount" HeaderText="Under Reconciliation Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BlanksNos" HeaderText="Blanks Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BlanksAmount" HeaderText="Blanks Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TotalNos" HeaderText="Total Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />                                
                            </Columns>
                        </asp:GridView>
                    <%--</div>
                </div>--%>
                <div style="height: 20px">
                    </div>
                <%--<div style="width: 940px;">
                    <div id="GHead3">
                    </div>
                    <div style="height: 150px; overflow: auto">--%>
                        <asp:GridView ID="grdRecovery3" SkinID="gridviewSkin1" runat="server"
                             AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="PendingDays" HeaderText=" " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                               <asp:BoundField DataField="SubmittedNos" HeaderText="Submitted Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SubmittedAmount" HeaderText="Submitted Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NotReceivedAtSiteNos" HeaderText="Not received at site Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NotReceivedAtSiteAmount" HeaderText="Not received at site Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReceivedAtSiteNos" HeaderText="Received at site Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReceivedAtSiteAmount" HeaderText="Received at site Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NotPostedInAccountsNos" HeaderText="Not posted in accounts Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NotPostedInAccountsAmount" HeaderText="Not posted in accounts Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PostedInAccountsNos" HeaderText="Posted in Accounts Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PostedInAccountsAmount" HeaderText="Posted in Accounts Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AckNotAvailableNos" HeaderText="Ack not available Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="AckNotAvailableAmount" HeaderText="Ack not available Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UnderReconciliationNos" HeaderText="Under Reconciliation Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UnderReconciliationAmount" HeaderText="Under Reconciliation Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BlanksNos" HeaderText="Blanks Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BlanksAmount" HeaderText="Blanks Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TotalNos" HeaderText="Total Nos" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />                                
                            </Columns>
                        </asp:GridView>
                    <%--</div>
                </div>--%>
            </asp:Panel>
        </asp:Panel>
    </div>
    <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdRecovery.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdRecovery.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdRecovery.ClientID%>').offset().top);

        });
    </script>
</asp:Content>

