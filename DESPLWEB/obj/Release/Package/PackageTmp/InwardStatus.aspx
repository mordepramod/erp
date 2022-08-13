<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InwardStatus.aspx.cs" MasterPageFile="~/MstPg_Veena.Master"
    Title="InwardStatus" Inherits="DESPLWEB.InwardStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="12%">
                        <asp:Label ID="Label1" runat="server" Text="Mat. Recd "></asp:Label>
                    </td>
                    <td>
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
                        &nbsp; &nbsp;&nbsp; &nbsp;
                        <asp:CheckBox ID="chkGTGenerateBill" runat="server" Text="Generate Bill" Visible="false"/>&nbsp; &nbsp;
                        <asp:CheckBox ID="chkGTGenerateRABill" runat="server" Text="Generate RA Bill" Visible="false"/>
                        <asp:Label ID="lblApprRight" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblWithoutBillRight" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td colspan="2" align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Inward Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_InwardTestType" Width="155px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_InwardTestType_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp; &nbsp;&nbsp; &nbsp;
                        <asp:RadioButton ID="rdnPending" runat="server" GroupName="status" />
                        <asp:Label ID="lblPending" runat="server" Text="Pending"></asp:Label>
                        <asp:RadioButton ID="rdnApproved" runat="server" GroupName="status" />
                        <asp:Label ID="lblApproved" runat="server" Text="Approved"></asp:Label>&nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblReason" runat="server" Text="Reason"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txtReason" Width="150px" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_RecordsNo" runat="server" Text=""></asp:Label>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Print</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="420px" Width="940px"
                            BorderStyle="Ridge" BorderColor="AliceBlue">
                     <%--      <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 420px; overflow: auto">--%>
                            <asp:GridView ID="grdModifyInward" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                                BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                                CssClass="Grid" Width="100%" CellPadding="0" CellSpacing="0" OnRowCommand="grdModifyInward_RowCommand">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkApprove" runat="server" Style="text-decoration: underline;"
                                                CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("ReferenceNo") + ";"+ Eval("CollectionDate") + ";"+ Eval("withoutBillStatus") %>'
                                                CommandName="ApproveInward" ToolTip="Approve Inward">Approve</asp:LinkButton> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RecordType" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="RecordNo" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CollectionDate" HeaderText="Collection Date" DataFormatString="{0:dd/MM/yyyy}"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ReceivedDate" HeaderText="Received Date" DataFormatString="{0:dd/MM/yyyy}"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ContactNo" HeaderText="Contact No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ReceivedBy" HeaderText="Received By" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="RouteName" HeaderText="Route" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="MEName" HeaderText="Marketing Executive" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkModifyInward" runat="server" ToolTip="Modify Inward" Visible="false"
                                                Style="text-decoration: underline;" 
                                                CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("ReferenceNo") + ";"+ Eval("CollectionDate") + ";"+ Eval("withoutBillStatus") %>'
                                                CommandName="ModifyInward">Modify Inward</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="Label6" runat="server" Text="View"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPrntInward" runat="server" ToolTip="View Inward" Style="text-decoration: underline;"
                                                CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("ReferenceNo") + ";"+ Eval("CollectionDate") + ";"+ Eval("withoutBillStatus") %>'
                                                CommandName="PrintInward">Inward</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="Label7" runat="server" Text="View"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPrntLabsheet" runat="server" ToolTip="View LabSheet" Style="text-decoration: underline;"
                                                CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("ReferenceNo") + ";"+ Eval("CollectionDate") + ";"+ Eval("withoutBillStatus") %>'
                                                CommandName="PrintLabSheet">Lab Sheet</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
                           <%-- </div></div>--%>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
   <%--  <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdModifyInward.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdModifyInward.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdModifyInward.ClientID%>').offset().top);

        });
    </script>--%>
</asp:Content>
