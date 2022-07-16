<%@ Page Title="Bill generation pending report list" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="BillPendingReportList.aspx.cs" Inherits="DESPLWEB.BillPendingReportList"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td width="90%">
                        <asp:RadioButton ID="optViewList" Text="Reports not ready" runat="server" AutoPostBack="true"
                            GroupName="g1" oncheckedchanged="optViewList_CheckedChanged"/>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="optGenerateBill" Text="Generate Bill for Approved Reports" 
                            runat="server" GroupName="g1"  AutoPostBack="true"
                            oncheckedchanged="optGenerateBill_CheckedChanged"/>
                        &nbsp;&nbsp;&nbsp;&nbsp;
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
                                            <asp:BoundField DataField="SITE_MonthlyBillingStatus_bit" HeaderText="Monthly Status" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90px" />
                                            <asp:TemplateField HeaderText="Bill">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkGenerateBill" runat="server" ToolTip="Generate Bill" Style="text-decoration: underline;"
                                                        CommandArgument='<%#Eval("INWD_RecordType_var") + ";" + Eval("INWD_RecordNo_int") + ";" + Eval("INWD_ReferenceNo_int") %>'
                                                        CommandName="GenerateBill">Generate</asp:LinkButton>
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
    <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdReport.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdReport.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdReport.ClientID%>').offset().top);

        });
    </script>
</asp:Content>
