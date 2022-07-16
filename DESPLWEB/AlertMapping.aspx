<%@ Page Title="Alert Mapping" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="AlertMapping.aspx.cs" Inherits="DESPLWEB.AlertMapping"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <%-- <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="600px"
            Style="background-color: #ECF5FF;">--%>
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" ScrollBars="Vertical"
            Height="480px" Style="background-color: #ECF5FF;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 10%;">
                                <asp:RadioButton ID="RdnTrigger" runat="server" AutoPostBack="true" OnCheckedChanged="RdnTrigger_CheckedChanged"
                                    Text="Trigger" GroupName="A" Checked="True" />
                            </td>
                            <td style="width: 10%;">
                                <asp:RadioButton ID="RdnEscalation" runat="server" AutoPostBack="true" GroupName="A"
                                    OnCheckedChanged="RdnEscalation_CheckedChanged" Text="Escalation" />
                            </td>
                            <td>
                                <asp:Label ID="lblMessage" runat="server" ForeColor="#990033" Text="lblMsg" Visible="False"></asp:Label>
                                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True" Text="Error: Invalid Data."
                                    Visible="False"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                                    OnClick="imgClosePopup_Click" ImageAlign="Right" />
                            </td>
                        </tr>
                        <tr>
                            <%--  <td style="width: 10%;">
                     </td>--%>
                            <td colspan="3" valign="top"> 
                            <asp:CheckBox ID="chkAll" runat="server" Text="Analysis(all)" Checked="true"  AutoPostBack="true"
                                    onclick="fnCheckOne(this)" oncheckedchanged="chkAll_CheckedChanged" />&nbsp;
                                <asp:CheckBox ID="chkPending" runat="server" Text="Pending"  AutoPostBack="true"
                                    onclick="fnCheckOne(this)" oncheckedchanged="chkPending_CheckedChanged" />
                                &nbsp; &nbsp; &nbsp; &nbsp;
                               
                                <asp:Label ID="lblFrm" runat="server" Text="From Date"></asp:Label>
                                &nbsp; &nbsp;
                                <asp:TextBox ID="txt_FromDate" Width="148px" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                    Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                                </asp:CalendarExtender>
                                <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                                    Mask="99/99/9999" AutoComplete="false" runat="server">
                                </asp:MaskedEditExtender>
                                &nbsp; &nbsp;
                                <asp:Label ID="lblTo" runat="server" Text="To Date"></asp:Label>
                                &nbsp; &nbsp;
                                <asp:TextBox ID="txt_ToDate" Width="148px" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                    Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                                </asp:CalendarExtender>
                                <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ToDate" MaskType="Date"
                                    Mask="99/99/9999" AutoComplete="false" runat="server">
                                </asp:MaskedEditExtender>
                                &nbsp; &nbsp;
                                <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/Search-32.png"
                                    OnClick="ImgBtnSearch_Click" Style="width: 14px" /><br />
                            </td>
                            <td align="right">
                                <asp:LinkButton ID="lnkMail" runat="server" CssClass="LnkOver" OnClick="lnkMail_Click"
                                    Visible="false" Style="text-decoration: underline;" ValidationGroup="V1" Font-Bold="True"> mail </asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" OnClick="lnkSave_Click"
                                    Style="text-decoration: underline;" ValidationGroup="V1" Font-Bold="True">Save </asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkGenerate" runat="server" CssClass="LnkOver" OnClick="lnkGenerate_Click"
                                    Visible="false" Style="text-decoration: underline;" ValidationGroup="V1" Font-Bold="True">Generate Report</asp:LinkButton>
                            </td>
                        </tr>
                        <%--    <tr>
                    <td colspan="4">
                        &nbsp;<br />
                    </td>
                </tr>--%>
                        <tr>
                            <td colspan="4" valign="top">
                                <asp:Panel ID="pnlAlert" runat="server" Width="100%" BorderWidth="0px" Visible="true">
                                    <asp:GridView ID="grdAlert" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                        BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" ForeColor="#333333"
                                        GridLines="None" Width="99%" CellPadding="1" CellSpacing="1">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No" ItemStyle-Width="30px">
                                                <ItemTemplate>
                                                    <span>
                                                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                                    </span>
                                                </ItemTemplate>
                                                <ItemStyle Width="30px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Alert Id" ItemStyle-Width="30px" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAlertId" runat="server" Text='' />
                                                </ItemTemplate>
                                                <ItemStyle Width="30px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Alert_Description_var" HeaderText="Alert" ItemStyle-Width="300px">
                                                <ItemStyle Width="300px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Alert_Delay_int" HeaderText="Delay" ItemStyle-Width="30px">
                                                <ItemStyle Width="30px" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Email Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="300px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtEmailId" runat="server" Text='' BorderWidth="0px" Width="300px" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="300px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Middle">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkViewDetails" runat="server" OnCommand="lnkViewDetails" Text="view"
                                                        class="LnkOver"> </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                                        <PagerStyle BorderColor="Blue" Font-Bold="True" Font-Italic="False" Font-Names="Arial"
                                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False"
                                            ForeColor="Blue" HorizontalAlign="Center" Wrap="True" />
                                        <EmptyDataTemplate>
                                            No records to display...
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" valign="top">
                                <br />
                            </td>
                        </tr>
                        <tr id="tr1" runat="server" visible="false">
                            <td valign="top" colspan="3">
                                <asp:Label ID="lblAlertNo" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lblAlertName" runat="server" Text=""></asp:Label>
                            </td>
                            <td align="right" valign="top">
                                <asp:Label ID="lbl_RecordsNo" runat="server" Text="Total Records : 0"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr2" runat="server" visible="false">
                            <td colspan="4" valign="top">
                                <asp:Panel ID="Panel1" runat="server" Width="100%" Style="border-style: solid; border-color: inherit;
                                    border-width: thin; height: 200px; overflow: auto" Visible="true">
                                    <asp:GridView ID="grdAlertList" runat="server" AutoGenerateColumns="False" CellPadding="2"
                                        ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="#F7F6F3"
                                        CssClass="Grid" BorderWidth="1px" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="AlertDtl_EnqNo_int" HeaderText="Enquiry No." HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="AlertDtl_EnqDate_date" HeaderText="Enquiry Date" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="AlertDtl_CollctnEnqDate_date" HeaderText="Collection Date"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="AlertDtl_ProNo_var" HeaderText="Proposal No." HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="AlertDtl_ProDate_date" HeaderText="Proposal Date" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="AlertDtl_ProOrderDate_date" HeaderText="Proposal Order Date"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="AlertDtl_BillNo_int" HeaderText="Bill No." HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="AlertDtl_BillDate_date" HeaderText="Bill Date" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="AlertDtl_BillModifiedDate_date" HeaderText="Bill Modified Date"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="AlertDtl_OutwrdDate_date" HeaderText="Bill Outward Date"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="AlertDtl_BillAmt_dec" HeaderText="Bill Amount" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="AlertDtl_RefNo_var" HeaderText="Reference No." HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" /><%--11--%>
                                            <asp:BoundField DataField="AlertDtl_RecNo_int" HeaderText="Record No." HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                           <asp:BoundField DataField="AlertDtl_InwdRecDate_date" HeaderText="Inward Received Date"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="AlertDtl_AppDate_date" HeaderText="Approved Date"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="AlertDtl_EnteredDate_date" HeaderText="Entered Date"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="AlertDtl_CheckedDate_date" HeaderText="Checked Date"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                             <asp:BoundField DataField="AlertDtl_CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="AlertDtl_SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                             <asp:BoundField DataField="AlertDtl_TestName_var" HeaderText="Test Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px" />
                                           <asp:BoundField DataField="AlertDtl_RecType_var" HeaderText="Material Type" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px" />
                                            <asp:BoundField DataField="AlertDtl_MEName_var" HeaderText="ME Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px" />
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
                            </td>
                        </tr>
                        <tr id="tr3" runat="server" visible="false">
                            <td align="right" colspan="4">
                                <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                                    OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lnkPrint" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script>
        function fnCheckOne(me) {
            me.checked = true;
            var chkary = document.getElementsByTagName('input');
            for (i = 0; i < chkary.length; i++) {
                if (chkary[i].type == 'checkbox') {
                    if (chkary[i].id != me.id)
                        chkary[i].checked = false;
                }
            }
        }
    </script>
</asp:Content>
