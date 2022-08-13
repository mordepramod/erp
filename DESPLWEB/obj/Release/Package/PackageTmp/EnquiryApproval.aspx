<%@ Page Title="Enquiry Approval" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="EnquiryApproval.aspx.cs" Inherits="DESPLWEB.EnquiryApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 18%">
                        <asp:RadioButton ID="optPending" runat="server" GroupName="g1" Text="Pending for Approval"
                            AutoPostBack="true" oncheckedchanged="optPending_CheckedChanged" />
                    </td>
                    <td style="width: 22%">
                        <asp:RadioButton ID="optCRPending" runat="server" GroupName="g1" Text="Pending for CR Limit Approval"
                            AutoPostBack="true" oncheckedchanged="optPending_CheckedChanged" />
                    </td>
                    <td style="width: 16%">
                        <asp:RadioButton ID="optApproved" runat="server" GroupName="g1" Text="Approved Enquiry"
                            AutoPostBack="true" oncheckedchanged="optApproved_CheckedChanged" />
                    </td>
                    <td colspan="2" style="width: 55%">
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;<asp:CheckBox ID="chkSearchByDate" runat="server" Text="Enquiry Date" 
                            AutoPostBack="true" oncheckedchanged="chkSearchByDate_CheckedChanged" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" Width="143px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblRecords" runat="server" Text="Total No of Records : 0" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkFetch" OnClick="lnkFetch_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Fetch</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkApprove" OnClick="lnkApprove_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Approve</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Print</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" style="height: 23px" valign="top">
                        <asp:Panel ID="pnlEnquiry" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="410px"
                            Width="940px" BorderColor="AliceBlue">
                              <%-- <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 410px; overflow: auto">--%>
                            <asp:GridView ID="grdEnquiry" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                ForeColor="#333333" GridLines="Vertical" BorderColor="#DEDFDE" BorderWidth="1px"
                                Width="100%" OnRowDataBound="grdEnquiry_RowDataBound">
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="10px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EnquiryDate" HeaderText="Enquiry Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ExpectedDate" HeaderText="Expected Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SiteName" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="EnquiryFor" HeaderText="Enquiry For" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CollectionDate" HeaderText="Collection Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />                                    
                                    <asp:TemplateField HeaderText="Edited Collection Date" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtEditedCollectionDate" runat="server" Text='<%#Eval("txtEditedCollectionDate") %>'></asp:TextBox>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtEditedCollectionDate" MaskType="Date"
                                                Mask="99/99/9999" AutoComplete="false" runat="server">
                                            </asp:MaskedEditExtender>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Location" HeaderText="Location" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Route" HeaderText="Route" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Limit" HeaderText="Limit" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BalanceAmount" HeaderText="Balance Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="EnquiryNo" HeaderText="Enquiry No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Comment" HeaderText="Comment" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="RouteId" HeaderText="RouteId" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="LocationId" HeaderText="LocationId" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="DriverId" HeaderText="DriverId" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="VehicleId" HeaderText="VehicleId" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ContactNo" HeaderText="Contact No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="PaymentMode" HeaderText="Payment Mode" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CarryForwardDate" HeaderText="Carry Forward Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />                                    
                                    <asp:TemplateField HeaderText="Reason for Approve" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReasonForApprove" runat="server" Text='<%#Eval("txtReasonForApprove") %>'></asp:TextBox>
                                            <asp:Label ID="lblEnquiryStatus" runat="server" Text='<%#Eval("lblEnquiryStatus") %>' Visible="false"/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ME" HeaderText="ME" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Note" HeaderText="Note" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ContactNoForApproval" HeaderText="Contact No for approval"
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
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                            <%--</div></div>--%>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <%--  <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdEnquiry.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdEnquiry.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdEnquiry.ClientID%>').offset().top);
//            $('#GHead').scrollTop($('#<%=grdEnquiry.ClientID%>'));
        });
    </script>--%>
</asp:Content>
