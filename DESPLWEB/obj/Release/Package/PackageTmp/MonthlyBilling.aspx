<%@ Page Title="Monthly Billing" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="MonthlyBilling.aspx.cs" Inherits="DESPLWEB.MonthlyBilling"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td width="10%">
                        <asp:Label ID="lblFromDate" runat="server" Text="For Month "></asp:Label>
                        &nbsp;&nbsp;
                    </td>
                    <td width="45%">
                        <asp:TextBox ID="txtFromDate" runat="server" Width="120px"
                            OnTextChanged="txtFromDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <%--<asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>--%>
                        &nbsp; &nbsp;&nbsp; 
                        <asp:LinkButton ID="lnkDisplay" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkDisplay_Click" Font-Bold="True">Display</asp:LinkButton>
                      
                    </td>
                    <td width="45%" align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClient" runat="server" Text="Client Name "></asp:Label>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlClient" runat="server" Width="350px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Site Name"></asp:Label>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSite" runat="server" Width="350px">
                        </asp:DropDownList>
                        &nbsp; &nbsp;&nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" Visible="false" />&nbsp; &nbsp;
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:LinkButton ID="lnkLoadPendingList" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkLoadPendingList_Click" Font-Bold="True">Load Pending Client-Site List </asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkLoadPendingRptList" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkLoadPendingRptList_Click" Font-Bold="True">Load Pending Report List </asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkLoadBillList" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkLoadBillList_Click" Font-Bold="True">Load Monthly Bill List </asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkGenerateBill" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkGenerateBill_Click" Font-Bold="True">Generate Bill</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                   <asp:LinkButton ID="lnkMonlthlyClientList" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                       OnClick="lnkMonlthlyClientList_Click" Font-Bold="True">Download List of Monthly Client</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 5px"></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="pnlBillList" runat="server" ScrollBars="Auto" Height="350px" Width="940px"
                            BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="0" GroupingText="Bill List" Visible="false">
                            <asp:GridView ID="grdBill" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False"
                                OnRowCommand="grdBill_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="BILL_Id" HeaderText="Bill No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BILL_Date_dt" HeaderText="Bill Date" DataFormatString="{0:dd/MM/yyyy}"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <%--<asp:BoundField DataField="BILL_RecordType_var" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BILL_RecordNo_int" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ReferenceNo_int" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />--%>
                                    <asp:BoundField DataField="BILL_NetAmt_num" HeaderText="Bill Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BILL_DiscountAmt_num" HeaderText="Discount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BILL_CGSTAmt_num" HeaderText="CGST Amt" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BILL_SGSTAmt_num" HeaderText="SGST Amt" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BILL_IGSTAmt_num" HeaderText="IGST Amt" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BILL_PaidAmount" HeaderText="Paid Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n}" />
                                    <asp:BoundField DataField="BILL_PendingAmount" HeaderText="Pending Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n}" />
                                    <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CL_Id" HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SITE_Id" HeaderText="Site Id" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BILL_CL_GSTNo_var" HeaderText="Client GSTIN" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BILL_SITE_GSTNo_var" HeaderText="Site GSTIN" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPrintBill" runat="server" ToolTip="Print Bill" Style="text-decoration: underline;"
                                                CommandArgument='<%#Eval("Bill_Id")%>' CommandName="lnkPrintBill">&nbsp;Print&nbsp;</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPrintBillDetail" runat="server" ToolTip="Print Bill Detail" Style="text-decoration: underline;"
                                                CommandArgument='<%#Eval("Bill_Id")%>' CommandName="lnkPrintBillDetail">&nbsp;Print_Bill_Detail&nbsp;</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkModifyBill" runat="server" ToolTip="Modify Bill" Style="text-decoration: underline;"
                                                CommandArgument='<%#Eval("Bill_Id") %>' CommandName="lnkModifyBill">&nbsp;Modify&nbsp;</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="pnlPendingList" runat="server" ScrollBars="Auto" Height="350px" Width="940px"
                            BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="0" GroupingText="Pending List for bill generation" Visible="false">
                            <asp:GridView ID="grdClient" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" ItemStyle-Width="380px" />
                                    <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" ItemStyle-Width="300px" />
                                    <asp:BoundField DataField="ForMonth" HeaderText="Pending For Month"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="CL_Id" HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" Visible="false" />
                                    <asp:BoundField DataField="SITE_Id" HeaderText="Site Id" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" Visible="false" />
                                    <%--<asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkGenerateClientBill" runat="server" ToolTip="Generate Bill" Style="text-decoration: underline;"
                                                    OnClick="lnkGenerateClientBill_Click" Font-Bold="True">&nbsp;Generate Bill&nbsp;</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>

                <tr>
                    <td colspan="3">
                        <asp:Panel ID="pnlPendingReportList" runat="server" ScrollBars="Auto" Height="350px" Width="940px"
                            BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="0" GroupingText="Pending Report List for bill generation" Visible="false">
                            <asp:GridView ID="grdReport" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="INWD_RecordType_var" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="INWD_RecordNo_int" HeaderText="Record No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="INWD_ReferenceNo_int" HeaderText="Reference No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="INWD_ReceivedDate_dt" HeaderText="Received Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="100px" />

                                    <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="ForMonth" HeaderText="Pending For Month"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="CL_Id" HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" Visible="false" />
                                    <asp:BoundField DataField="SITE_Id" HeaderText="Site Id" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" Visible="false" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>

                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
    </script>
</asp:Content>
