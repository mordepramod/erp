<%@ Page Title="Outward Reports" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="OutwardReport.aspx.cs" Inherits="DESPLWEB.OutwardReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="8%">
                        <asp:Label ID="Label1" runat="server" Text="Mat. Recd "></asp:Label>
                    </td>
                    <td style="width: 35%" colspan="2">
                        <asp:TextBox ID="txt_FromDate" Width="130px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txt_ToDate" Width="130px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td style="width: 12%" align="left">
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                    </td>
                    <td style="width: 45%">
                        <asp:TextBox ID="txt_Client" runat="server" Width="270px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                       
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td valign="middle">
                        <asp:Label ID="Label2" runat="server" Text="Inward Type"></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:DropDownList ID="ddl_InwardTestType" Width="150px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_InwardTestType_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:DropDownList ID="ddlMF" Width="100px" runat="server" Visible="false">
                            <asp:ListItem Text="MD Letter" Value="MDL"></asp:ListItem>
                            <asp:ListItem Text="Final Report" Value="Final"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                        <asp:RadioButton ID="optOutwardPending" runat="server" Text="Outward Pending" GroupName="G1"
                            AutoPostBack="true" OnCheckedChanged="optOutwardPending_CheckedChanged" />
                        &nbsp;
                        <asp:RadioButton ID="optOutwardCompleted" runat="server" Text="Phy. Outward Pending"
                            GroupName="G1" AutoPostBack="true" OnCheckedChanged="optOutwardCompleted_CheckedChanged" />
                        &nbsp;
                        <asp:RadioButton ID="optPhysicalOutwardCompleted" runat="server" Text="Phy. Outward Completed"
                            GroupName="G1" AutoPostBack="true" OnCheckedChanged="optPhysicalOutwardCompleted_CheckedChanged" />
                        &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                         &nbsp;&nbsp;
                        <asp:Label ID="lbl_RecordsNo" runat="server" Text="" Font-Bold="true"></asp:Label>
                      <%--  <asp:LinkButton ID="lnkFilter" runat="server" OnClick="lnkFilter_Click" Visible="false">Add Filter</asp:LinkButton>
                        &nbsp;
                        <asp:DropDownList ID="ddlFilter" runat="server" Width="110px" Visible="false" >
                        </asp:DropDownList>--%>
                        &nbsp;
                        <asp:Label ID="lblUserName" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblOutwardBy" runat="server" Text="Outward By" Visible="false"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtOutwardBy" runat="server" Width="100px" Text='' ReadOnly="true"
                            Visible="false"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblOutwardTo" runat="server" Text="Outward To" Visible="false"></asp:Label>&nbsp;
                        <%--<asp:TextBox ID="txtOutwardTo" runat="server" Width="100px" Text='' Visible="false"></asp:TextBox>--%>&nbsp;
                        <asp:DropDownList ID="ddlUser" runat="server" Width="120px" Visible="false">
                        </asp:DropDownList>
                        <asp:Label ID="lblOutwardDate" runat="server" Text="Outward Date" Visible="false"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtOutwardDate" runat="server" Width="100px" Text='' Visible="false"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtOutwardDate">
                        </asp:CalendarExtender>
                        <asp:Label ID="lblRegisterNo" runat="server" Text="Register No" Visible="false"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtRegisterNo" runat="server" Width="100px" Text='' Visible="false"
                            onkeyup="checkNum(this);"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblDeliveredTo" runat="server" Text="Delivered To" Visible="false"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtDeliveredTo" runat="server" Width="100px" Text='' Visible="false"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblDeliveredDate" runat="server" Text="Delivered Date" Visible="false"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtDeliveredDate" runat="server" Width="100px" Text='' Visible="false"></asp:TextBox>&nbsp;
                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtDeliveredDate">
                        </asp:CalendarExtender>
                        &nbsp;
                        <asp:Label ID="lblRemark" runat="server" Text="Remark" Visible="false"></asp:Label>&nbsp;
                        <asp:TextBox ID="txtRemark" runat="server" Width="100px" Text='' Visible="false"></asp:TextBox>&nbsp;
                        <asp:CheckBox ID="chkWithBill" runat="server" Text="With Bill" Checked="true"/>&nbsp;
                      <asp:LinkButton ID="lnkOutwardReport" runat="server" OnClick="lnkOutwardReport_Click">Outward</asp:LinkButton>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkPrint" runat="server" OnClick="lnkPrint_Click" Visible="false">Print</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="380px"
                Width="940px" BorderColor="AliceBlue">
                <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 380px; overflow: auto">
                        <asp:GridView ID="grdReports" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                            CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Horizontal"
                            Width="100%" CellPadding="0" CellSpacing="0" OnRowCommand="grdReport_RowCommand"
                            OnRowDataBound="grdReportStatus_RowDataBound">
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
                                <asp:BoundField DataField="SetOfRecord" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Bill_No" HeaderText="Bill No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                               <asp:TemplateField HeaderText="Outward By">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOutwardBy" runat="server" Width="100px" Text='<%# Eval("OUTW_OutwardBy_var") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Outward To
                                        <br />
                                        <asp:DropDownList ID="ddlOTFilter" runat="server" AutoPostBack="true"  OnSelectedIndexChanged = "ddlOTFilter_SelectedIndexChanged"
                                        Width="100px" AppendDataBoundItems="true">
                                             </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOutwardTo" runat="server" Width="100px" Text='<%# Eval("OUTW_OutwardTo_var") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Outward Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOutwardDate" runat="server" Width="75px" Text='<%# Eval("OUTW_OutwardDate_dt","{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtOutwardDate">
                                        </asp:CalendarExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Register No.">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRegisterNo" runat="server" Width="80px" Text='<%# Eval("OUTW_RegisterNo_int") %>'
                                            onkeyup="checkNum(this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delivered To">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDeliveredTo" runat="server" Width="100px" Text='<%# Eval("OUTW_DeliveredTo_var") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delivered Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDeliveredDate" runat="server" Width="75px" Text='<%# Eval("OUTW_DeliveredDate_dt","{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtDeliveredDate">
                                        </asp:CalendarExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRemark" runat="server" Width="100px" Text='<%# Eval("OUTW_Remark_var") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkOutwardReport" runat="server" ToolTip="Outward Report" Style="text-decoration: underline;" 
                                            CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("ReferenceNo") + ";"+ Eval("SetOfRecord")  + ";" + Eval("MISTestType") + ";" + Eval("Bill_No")%>'
                                            CommandName="OutwardReport">Outward</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MISTestType" HeaderText="Test Type" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Record Detail" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecordDetail" runat="server" Text='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("ReferenceNo") + ";"+ Eval("SetOfRecord") + ";" + Eval("MISTestType") + ";" + Eval("Bill_No")%>'></asp:Label>
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

        function checkNum(x) {
            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    x.value = '';
                    x.focus();
                    alert("Only Numeric Values Allowed");

                    return false;
                }
            }
            return true;
        }
                   
    </script>
</asp:Content>
