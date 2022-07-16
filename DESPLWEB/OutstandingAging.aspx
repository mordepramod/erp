<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="OutstandingAging.aspx.cs" Inherits="DESPLWEB.OutstandingAging" Theme="duro" %>

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
                    <td colspan="3" style="height: 5px">
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlAging" runat="server" ScrollBars="Auto" Height="390px" Width="940px"
                BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">
                <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 390px; overflow: auto">
                        <asp:GridView ID="grdOutstanding" SkinID="gridviewSkin1" runat="server"
                            ShowFooter="True" AutoGenerateColumns="False" >
                            <Columns>
                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="0to30" HeaderText="0 - 30 day" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="31to60" HeaderText="31 - 60 day" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="61to90" HeaderText="61 - 90 day" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="91to120" HeaderText="91 - 120 day " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Right" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="121to180" HeaderText="121 - 180 Day" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Right" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="181to270" HeaderText="181 - 270 day" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="271to365" HeaderText="271 - 365 day" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="366to730" HeaderText="366 - 730 day" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MoreThan731" HeaderText="More than 731 day" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GrandTotal" HeaderText="Grand Total" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RecoveryUser" HeaderText="Recovery User" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" NullDisplayText="-" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>

                                <asp:BoundField DataField="PostedInAccountsAmount" HeaderText="Posted in accounts" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ReceivedAtHOAmount" HeaderText="Received at HO" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NotReceivedAmount" HeaderText="Not Received" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ResubmittedAmount" HeaderText="Resubmitted" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UnderReconciliationAmount" HeaderText="Under Reconciliation" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BlanksAmount" HeaderText="Blanks" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DebitNoteAmount" HeaderText="Debit Note" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OnAccAmount" HeaderText="On A/c" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Business1920" HeaderText="Business 2019-20" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
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
            var gridHeader = $('#<%=grdOutstanding.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdOutstanding.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdOutstanding.ClientID%>').offset().top);

        });
    </script>--%>
</asp:Content>
