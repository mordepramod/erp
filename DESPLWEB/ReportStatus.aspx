<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportStatus.aspx.cs" MasterPageFile="~/MstPg_Veena.Master"
    Title="Report Status" Inherits="DESPLWEB.ReportStatus" %>

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
                        <asp:Label ID="lblTestingDt" Visible="false" runat="server" Text="Testing Date"></asp:Label>
                    </td>
                    <td style="width: 348px">
                        <asp:TextBox ID="txtTestingDt" Width="70px" Visible="false" ReadOnly="true" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtTestingDt">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txtTestingDt" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_RecordsNo" runat="server" Text=""></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Bold="True" OnCommand="lnkPrint_Click"
                            Style="text-decoration: underline;" >Print</asp:LinkButton>
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
                    <td style="width: 40%">
                        <asp:DropDownList ID="ddl_InwardTestType" Width="155px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_InwardTestType_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:DropDownList ID="ddl_ReportType" Width="155px" runat="server">
                            <asp:ListItem Text="All" Value="0" />
                            <asp:ListItem Text="Start Test" Value="12" />
                            <asp:ListItem Text="Enter" Value="1" />
                            <asp:ListItem Text="Check" Value="2" />
                            <asp:ListItem Text="Approve" Value="3" />
                            <asp:ListItem Text="Print" Value="4" />
                            <asp:ListItem Text="Outward" Value="5" />
                            <asp:ListItem Text="Physical Outward" Value="6" />
                            <asp:ListItem Text="Duplicate Print" Value="8" />
                        </asp:DropDownList>
                       
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Mail Status"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMailStatus" Width="155px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_InwardTestType_SelectedIndexChanged">
                            <asp:ListItem Text="All" Value="-1" />
                            <asp:ListItem Text="Pending" Value="0" />
                            <asp:ListItem Text="Mail Sent" Value="1" />
                        </asp:DropDownList>
                        &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>                    
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td style="width: 15%">
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                    </td>
                    <td align="left" style="width: 348px">
                        <asp:TextBox ID="txt_Client" runat="server" Width="327px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="380px"
                Width="940px" BorderColor="AliceBlue">
                <asp:GridView ID="grdReportStatus" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                    CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                    Width="100%" CellPadding="0" CellSpacing="0" OnRowCommand="grdReportStatus_RowCommand"
                    OnRowDataBound="grdReportStatus_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="CL_Name_var" HeaderText="Client name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Site_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
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
                        <asp:BoundField DataField="TestName" HeaderText="Test" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="rptStatus" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="rptCastStatus" HeaderText="" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="TestedDate" HeaderText="Tested Date" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="EnteredDate" HeaderText="Entered Date" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CheckedDate" HeaderText="Checked Date" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="PrintedDate" HeaderText="Printed Date" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ApprovedDate" HeaderText="Approved Date" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="RouteName" HeaderText="Route" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="MEName" HeaderText="Marketing Executive" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="InterBranchReferenceNo" HeaderText="Inter Branch Reference No" 
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="TEST_Name_var" HeaderText="Sub Test" 
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="MailStatus" HeaderText="Mail Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="Label3" runat="server" Text="Action"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEnterReport" runat="server" ToolTip="Enter Report" Style="text-decoration: underline;"
                                    CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("CollectionDate") + ";" +  Eval("ReferenceNo") %>'
                                    CommandName="EnterReport">Enter</asp:LinkButton>
                                <asp:LinkButton ID="lnkReEnterReport" runat="server" ToolTip="Re-Enter Report" Visible="false"
                                    Style="text-decoration: underline;" CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("CollectionDate") + ";" + Eval("ReferenceNo") %>'
                                    CommandName="ReEnterReport">Re-Enter</asp:LinkButton>
                                <asp:LinkButton ID="lnkGTSample" runat="server" ToolTip="Enter/Edit GT Sample" Visible="false"
                                    Style="text-decoration: underline;" CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("CollectionDate") + ";"  + Eval("ReferenceNo") %>'
                                    CommandName="EnterEditGTSample">Sample</asp:LinkButton>&nbsp;
                                <asp:LinkButton ID="lnkGTSummary" runat="server" ToolTip="Enter/Edit GT Summary"
                                    Visible="false" Style="text-decoration: underline;" CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("CollectionDate") + ";" + Eval("ReferenceNo") %>'
                                    CommandName="EnterEditGTSummary">Summary</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="Label7" runat="server" Text="Report"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewReport" Visible="false" runat="server" ToolTip="View Report"
                                    Style="text-decoration: underline;" CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("CollectionDate") + ";" + Eval("ReferenceNo") %>'
                                    CommandName="ViewReport">View</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="Label7" runat="server" Text="Cube Casting"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_CubeCasting" Visible="false" runat="server" Style="text-decoration: underline;"
                                    CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("CollectionDate") + ";"  + Eval("ReferenceNo") %>'
                                    CommandName="CubeCasting"></asp:LinkButton>
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
            </asp:Panel>
        </asp:Panel>
    </div>
    <%--<script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>--%>
    <%-- <script src="<%# ResolveUrl("~/") 
 %>App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdReportStatus.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdReportStatus.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdReportStatus .ClientID%>').offset().top);

        });
    </script>--%>
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
