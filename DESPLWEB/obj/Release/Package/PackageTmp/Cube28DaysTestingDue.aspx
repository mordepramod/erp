<%@ Page Title="28 Days Testing Due List" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="Cube28DaysTestingDue.aspx.cs" Inherits="DESPLWEB.Cube28DaysTestingDue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td width="16%">
                        <asp:Label ID="Label1" runat="server" Text="28 days Testing Due From"></asp:Label>
                    </td>
                    <td style="width: 50%">
                        <asp:TextBox ID="txt_FromDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txt_ToDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td style="width: 20%">
                        <asp:Label ID="lblTotalRecords" runat="server" Text=""></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Bold="True" OnCommand="lnkPrint_Click"
                            Style="text-decoration: underline;" onclick="lnkPrint_Click">Print</asp:LinkButton>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="420px"
                Width="940px" BorderColor="AliceBlue">
                <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 420px; overflow: auto">
                        <asp:GridView ID="grdReportList" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                            CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                            Width="100%" CellPadding="0" CellSpacing="0" >
                            <Columns>
                                <asp:BoundField DataField="CL_Name_var" HeaderText="Client name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Site_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CONT_Name_var" HeaderText="Contact Person" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ContactNo" HeaderText="Contact No." HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="RecordType" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="RecordNo" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Schedule" HeaderText="Schedule" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CastingDate" HeaderText="Casting Date" DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                 <asp:BoundField DataField="TestingDate" HeaderText="Testing Date" DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                             
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
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
    </div>
    <script src="<%# ResolveUrl("~/") 
 %>App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdReportList.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdReportList.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdReportList .ClientID%>').offset().top);

        });
    </script>
</asp:Content>
