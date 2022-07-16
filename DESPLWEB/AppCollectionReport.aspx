<%@ Page Title="App Collection Report" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="AppCollectionReport.aspx.cs" Inherits="DESPLWEB.AppCollectionReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="12%">
                        <asp:Label ID="Label1" runat="server" Text="Collection From"></asp:Label>
                    </td>
                    <td style="width: 43%">
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
                    </td>
                    <td style="width: 10%">
                        &nbsp;
                    </td>
                    <td style="width: 348px">
                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_RecordsNo" runat="server" Text=""></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Bold="True" 
                            Style="text-decoration: underline;" onclick="lnkPrint_Click">Print</asp:LinkButton>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Driver Name"></asp:Label>
                    </td>
                    <td style="width: 40%">
                        <asp:DropDownList ID="ddl_DriverName" Width="155px" runat="server">
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                          </td>
                    <td style="width: 15%">
                        <%-- <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />--%>
                    </td>
                    <td align="left" style="width: 348px">
                        <%--   <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                       <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>--%>
                    </td>
                </tr>
                  <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Route Name"></asp:Label>
                    </td>
                    <td style="width: 40%">
                        <asp:DropDownList ID="ddl_RouteName" Width="155px" runat="server">
                        </asp:DropDownList>
                              </td>
                    <td style="width: 15%">
                    </td>
                    <td>
                    </td>
                </tr>
                    <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Status"></asp:Label>
                    </td>
                    <td style="width: 40%">
                        <asp:DropDownList ID="ddl_Status" Width="155px" runat="server">
                        <asp:ListItem Value="">---Select All---</asp:ListItem>
                         <asp:ListItem Value="Collected">Collected</asp:ListItem>
                        <asp:ListItem Value="Not Collected">Not Collected</asp:ListItem>
                       <asp:ListItem Value="Pending">Pending</asp:ListItem>
                        </asp:DropDownList>
                       &nbsp; &nbsp;
                              <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td style="width: 15%">
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="360px"
                Width="940px" BorderColor="AliceBlue">
                <asp:GridView ID="grdReportStatus" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                    CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                    Width="100%" CellPadding="0" CellSpacing="0">
                    <Columns>
                        <asp:BoundField DataField="collection_date" HeaderText="Collection Date" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px" />
                        <asp:BoundField DataField="CL_Name_var" HeaderText="Client name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left"  ItemStyle-Width="150px" />
                        <asp:BoundField DataField="Site_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left"  ItemStyle-Width="150px" />
                        <asp:BoundField DataField="enquiry_id" HeaderText="Enquiry No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="50px" />
                        <asp:BoundField DataField="ROUTE_Name_var" HeaderText="Route Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="50px" />
                        <asp:BoundField DataField="in_time" HeaderText="In Time" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="out_time" HeaderText="Out Time" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="50px" />
                        <asp:BoundField DataField="contact_person_name" HeaderText="Contact Person Name"
                            ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="contact_person_mob" HeaderText="Contact Person No." HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="remark" HeaderText="Remark" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="pick_up_status" HeaderText="PickUp Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="executive" HeaderText="Marketing Executive" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" />
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
            </asp:Panel>
        </asp:Panel>
    </div>
    <%-- <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
    </script>--%>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
