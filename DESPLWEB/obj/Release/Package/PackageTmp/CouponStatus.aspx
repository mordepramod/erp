<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="CouponStatus.aspx.cs" Inherits="DESPLWEB.CouponStatus" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="100%" colspan="2">
                        <asp:Label ID="lblCouponsIssued" runat="server" Text="Coupons Issued" Font-Bold="true"></asp:Label>&nbsp;&nbsp;
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date "></asp:Label>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="120px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="lblToDate" runat="server" Text="To Date "></asp:Label>
                        <asp:TextBox ID="txtToDate" Width="120px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <%--<asp:Label ID="lblClient" runat="server" Text="Client"></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlClient" Width="260px" runat="server">
                        </asp:DropDownList>--%>
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:RadioButton ID="rdoCT" runat="server" Text="Cube" GroupName="status" /> 
                        <asp:RadioButton ID="rdoST" runat="server" Text="Steel" GroupName="status" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:LinkButton ID="lnkDisplay" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkDisplay_Click" Font-Bold="True">Display</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkTemp" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkTemp_Click" Font-Bold="True" Visible="false">Temp</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <%--<tr>
                    <td colspan="3" style="height: 1px">
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Panel ID="pnlClientList" runat="server" ScrollBars="Auto" Height="180px" Width="940px"
                            BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">
                            
                  <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 180px; overflow: auto">
                            <asp:GridView ID="grdClient" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px" />
                                    <asp:BoundField DataField="ClientId" HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="SiteName" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="BillNo" HeaderText="Bill No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="100px"
                                        HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="Issued" HeaderText="Issued" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="Used" HeaderText="Used" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="NotUsed" HeaderText="Not Used" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px" HeaderStyle-Wrap="false" />
                                    <asp:BoundField DataField="Expired" HeaderText="Expired" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Wrap="false" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkViewReport" runat="server" ToolTip="View Report" Style="text-decoration: underline;"
                                                OnClick="lnkViewReport" Width="30px">View</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </div></div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                    <br />
                        <asp:Label ID="lblDetailHeading" runat="server" Text="Label" Font-Bold="true" ForeColor="Brown"
                            Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                    <asp:Panel ID="pnlCouponDetails" runat="server" Height="30px" Width="940px"
                            BorderWidth="0" Visible="false">
                        <asp:Label ID="lblCouponStatus" runat="server" Text="Coupon Status" ></asp:Label>&nbsp;
                        <asp:DropDownList ID="ddlCouponStatus" Width="150px" runat="server" >
                            <asp:ListItem Text="---All---"></asp:ListItem>
                            <asp:ListItem Text="Used"></asp:ListItem>
                            <asp:ListItem Text="Not Used"></asp:ListItem>
                            <asp:ListItem Text="Expired"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkApply" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkApply_Click" Font-Bold="True" >Apply</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkgrdPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkgrdPrint_Click" Font-Bold="True" >Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblComment" runat="server" Text="Comment "></asp:Label>
                        <asp:TextBox ID="txtComment" Width="220px" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkCancelCoupons" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            Font-Bold="True" OnClick="lnkCancelCoupons_Click">Cancel Coupons</asp:LinkButton>
                            </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Panel ID="pnlCouponList" runat="server" ScrollBars="Auto" Height="180px" Width="940px"
                            BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1" Visible="false">
                            
                  <div style="width: 940px;">
                    <div id="GHead1">
                    </div>
                    <div style="height: 180px; overflow: auto">
                            <asp:GridView ID="grdCoupon" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                            CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Horizontal"
                            Width="100%" CellPadding="0" CellSpacing="0" >
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
                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="SiteName" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="CouponNo" HeaderText="Coupon No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="UsedOn" HeaderText="Used On" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="ExpiredOn" HeaderText="Expired On" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" />
                                </Columns>
                            </asp:GridView>
                            </div></div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
     <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdClient.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdClient.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdClient.ClientID%>').offset().top);

        });
    </script>
     <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdCoupon.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdCoupon.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead1").append(gridHeader);
            $('#GHead1').css('position', 'absolute');
            $('#GHead1').css('top', $('#<%=grdCoupon.ClientID%>').offset().top);

        });
    </script>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
    </script>
</asp:Content>
