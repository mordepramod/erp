<%@ Page Title="Duplicate Reports" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="DuplicateReport.aspx.cs" Inherits="DESPLWEB.DuplicateReport"
    Theme="duro" %>

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
                        <%--<asp:Label ID="Label3" runat="server" Text="Stationary"></asp:Label>--%>
                        &nbsp;
                    </td>
                    <td style="width: 348px">
                        <%--<asp:DropDownList ID="ddlStationary" Width="155px" runat="server" >
                            <asp:ListItem Text="Regular"></asp:ListItem>
                            <asp:ListItem Text="Logo - With NABL"></asp:ListItem>
                            <asp:ListItem Text="Logo - Without NABL"></asp:ListItem>
                        </asp:DropDownList>--%>
                        &nbsp;
                        <%--<asp:Label ID="lblMF" runat="server" Text="Sub-Report" Visible="false"></asp:Label>&nbsp;--%>
                        <asp:DropDownList ID="ddlMF" Width="100px" runat="server" Visible="false">
                            <asp:ListItem Text="MD Letter" Value="MDL"></asp:ListItem>
                            <asp:ListItem Text="Final Report" Value="Final"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Inward Type"></asp:Label>
                    </td>
                    <td style="width: 60%">
                        <asp:DropDownList ID="ddl_InwardTestType" Width="155px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_InwardTestType_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lbl_RecordsNo" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblUserId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td style="width: 15%">
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                    </td>
                    <td align="left" style="width: 348px">
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="400px"
                Width="940px" BorderColor="AliceBlue">
                <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 400px; overflow: auto">
                        <asp:GridView ID="grdReports" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                            CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Horizontal"
                            Width="100%" CellPadding="0" CellSpacing="0" OnRowCommand="grdReport_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="SetOfRecord" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CollectionDate" HeaderText="Collection Date" DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReceivedDate" HeaderText="Received Date" DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <%--<asp:BoundField DataField="rptStatus" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />--%>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPrintReport" runat="server" ToolTip="Print Report" Style="text-decoration: underline;"
                                              CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("ReferenceNo") %>'
                                            CommandName="DisplayReport">Print</asp:LinkButton>
                                        &nbsp;&nbsp;
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
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdReports.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdReports.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdReports.ClientID%>').offset().top);

        });
    </script>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
