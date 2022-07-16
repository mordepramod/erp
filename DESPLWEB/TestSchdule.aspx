<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="TestSchdule.aspx.cs" Theme="duro" Inherits="DESPLWEB.TestSchdule" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 10%">
                        &nbsp;
                        <asp:Label ID="lblFrmDt" runat="server" Text="From Date"></asp:Label>
                    </td>
                    <td style="width: 10%">
                        <asp:TextBox ID="txt_FromDate" Width="100px" runat="server" OnTextChanged="txt_FromDate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td align="center" style="width: 5%">
                        <asp:Label ID="lblToDt" runat="server" Text="To Date "></asp:Label>
                    </td>
                    <td style="width: 10%">
                        <asp:TextBox ID="txt_Todate" Width="100px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_Todate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_Todate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td colspan="3" align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                        <asp:Label ID="Label2" runat="server" Text="Inward Type "></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_InwardType" Width="280px" runat="server" OnSelectedIndexChanged="ddl_InwardType_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="All" Value="All" />
                            <asp:ListItem Text="Cube Testing" Value="CT" />
                            <asp:ListItem Text="Cement Testing" Value="CEMT" />
                            <asp:ListItem Text="Pavement Block Testing" Value="PT" />
                            <asp:ListItem Text="Masonary Block Testing" Value="SOLID" />
                            <asp:ListItem Text="Mix Design " Value="MF" />
                            <asp:ListItem Text="Fly Ash Testing" Value="FLYASH" />
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td align="left">
                        <asp:LinkButton ID="lnk_Print" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="lnk_Print_Click">Print </asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnk_PrintLabSheet" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="lnk_PrintLabSheet_Click" Visible="false">Print Lab Sheet</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="400px"
                Width="940px" BorderColor="AliceBlue">
                     <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 400px; overflow: auto">
                <asp:GridView ID="grdTesting" runat="server" AutoGenerateColumns="False" Width="100%"
                    SkinID="gridviewSkin1">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr No." HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblSerial" Text="<%#Container.DataItemIndex+1 %>" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RefNo" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CastingDt" HeaderText="Date Of Casting" HeaderStyle-HorizontalAlign="Center"
                           ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center" FooterStyle-Font-Bold="true" />
                        <asp:BoundField DataField="TestingDt" HeaderText="Date Of Testing" HeaderStyle-HorizontalAlign="Center"
                            DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center" FooterStyle-Font-Bold="true" />
                        <asp:BoundField DataField="Days_tint" HeaderText="Schedule Of Testing" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" />
                         <asp:BoundField DataField="Curing_Detail" HeaderText="Curing Tank" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" />
                        <asp:BoundField DataField="enquiryNo" HeaderText="Enquiry No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                        
                    </Columns>
                </asp:GridView>
                </div></div>
            </asp:Panel>
        </asp:Panel>
    </div>
     <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdTesting.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdTesting.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdTesting.ClientID%>').offset().top);

        });
    </script>
</asp:Content>
