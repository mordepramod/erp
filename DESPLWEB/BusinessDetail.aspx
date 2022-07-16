<%@ Page Title="Business Detail" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="BusinessDetail.aspx.cs" Inherits="DESPLWEB.BusinessDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table width="100%" style="border-color: #800000">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lbl_Total" runat="server" Text="" ForeColor="Brown" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" colspan="2">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <asp:Label ID="lblPeriod" runat="server" Text="Received in the Period"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_FromDate" Width="100px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txt_ToDate" Width="100px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClient" runat="server" Text="Client Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Client" runat="server" Width="420px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:HiddenField ID="hfClientId" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSite" runat="server" Text="Site Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Site" runat="server" Width="420px" AutoPostBack="true" OnTextChanged="txt_Site_TextChanged"
                            Text=""></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetSitename" MinimumPrefixLength="0" OnClientItemSelected="SiteItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Site"
                            ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:Label ID="lblSiteId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:HiddenField ID="hfSiteId" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label9" runat="server" Text="Marketing Executive"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlME" Width="225px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Testing Type"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlTestingType" Width="225px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                  <tr>
                  <td>
                        <asp:Label ID="Label4" runat="server" Text="Amount"></asp:Label>
                    </td>
                  <td colspan="3">
                   <asp:TextBox ID="txt_AmtFrom" runat="server" MaxLength="20" Width="103px" onchange="javascript:checkDecimal(this);"></asp:TextBox>-
                        <asp:TextBox ID="txt_AmtTo" runat="server" MaxLength="20" Width="103px" onchange="javascript:checkDecimal(this);"></asp:TextBox>
                        
                  </td>
                  </tr>
                <tr>
                    <td>
                       <asp:Label ID="Label3" runat="server" Text="Grouping By"></asp:Label>
                    </td>
                    <td>
                      <asp:CheckBox ID="chkClient" runat="server" Text="Client"  AutoPostBack="true"
                            oncheckedchanged="chkClient_CheckedChanged"/>&nbsp;&nbsp;
                     <asp:CheckBox ID="chkSite" runat="server" Text="Site"  AutoPostBack="true"
                            oncheckedchanged="chkSite_CheckedChanged" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <%--     <asp:RadioButton ID="RdnClient"  Font-Bold="false" runat="server" GroupName="Group"
                           Text="Client" />&nbsp;&nbsp;
                        <asp:RadioButton ID="RdnSite" runat="server" Font-Bold="false" GroupName="Group" Text="Site" />--%>
             
                    </td>
                    <td>
                        <asp:Label ID="lblRecords" runat="server" Text="Total No of Records : 0"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Print</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Panel ID="pnlDetail" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="250px"
                            Width="940px" BorderColor="AliceBlue">
                            <div style="width: 940px;">
                                <div id="GHead">
                                </div>
                                <div style="height: 250px; overflow: auto">
                                    <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                                        CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                                        Width="100%" CellPadding="1" CellSpacing="0">
                                        <Columns>
                                            <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="SiteName" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="BusinessFrom" HeaderText="Business From" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="left" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="BillAmt" HeaderText="Bill Amount" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="NewBusiness" HeaderText="New Business" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="PendingAmt" HeaderText="Pending Amount" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="BillNo" HeaderText="Bill No." HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" />
                                            <asp:BoundField DataField="TestingType" HeaderText="Testing Type" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" />
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                        <EmptyDataTemplate>
                                            No records to display
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Height="10px" />
                                        <EditRowStyle BackColor="#999999" />
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
            var gridHeader = $('#<%=grdDetail.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdDetail.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdDetail.ClientID%>').offset().top);

        });
    </script>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
        function SiteItemSelected(sender, e) {
            $get("<%=hfSiteId.ClientID %>").value = e.get_value();
        }
    </script>
</asp:Content>
