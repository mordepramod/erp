<%@ Page Title="Proforma Invoice Status" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="ProformaInvoiceStatus.aspx.cs" Inherits="DESPLWEB.ProformaInvoiceStatus" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td width="12%">
                        <asp:Label ID="lblProformaInvoicedt" runat="server" Text="Proforma Invoice Date "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txtToDate" Width="150px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="lblProformaInvoiceNo" runat="server" Text="Proforma Invoice No. "></asp:Label>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txtProformaInvoiceNo" Width="100px" MaxLength="50" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkEnteredProformaInvoicePrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkEnteredProformaInvoicePrint_Click" Font-Bold="True">Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td colspan="2" align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblInwdtype" runat="server" Text="Inward Type"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddl_InwardTestType" Width="150px" runat="server">
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />&nbsp; &nbsp;
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="height: 5px">
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblTotal" runat="server" Text="" Font-Bold="true" ForeColor="OrangeRed"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblPaidAmount" runat="server" Text="" Font-Bold="true" ForeColor="OrangeRed"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblPendingAmount" runat="server" Text="" Font-Bold="true" ForeColor="OrangeRed"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlProformaInvoiceList" runat="server" ScrollBars="Auto" Height="390px" Width="940px"
                BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">
            <%--    <div style="width: 940px;">
                    <div id="GHead">
                    </div>--%>
                    <div style="height: 390px; overflow: auto">
                        <asp:GridView ID="grdModifyProformaInvoice" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False"
                            OnRowCommand="grdModifyInward_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="PROINV_Id" HeaderText="Proforma Invoice No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PROINV_Date_dt" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PROINV_RecordType_var" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PROINV_RecordNo_int" HeaderText="Proposal Id" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PROINV_NetAmt_num" HeaderText="Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PROINV_DiscountAmt_num" HeaderText="Discount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PROINV_SerTaxAmt_num" HeaderText="Service Tax" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PROINV_SwachhBharatTaxAmt_num" HeaderText="Swachh Bharat Cess"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PROINV_KisanKrishiTaxAmt_num" HeaderText="Krishi Kalyan Cess"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PROINV_CGSTAmt_num" HeaderText="CGST Amt" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PROINV_SGSTAmt_num" HeaderText="SGST Amt" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PROINV_IGSTAmt_num" HeaderText="IGST Amt" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPrintProformaInvoice" runat="server" ToolTip="Print Proforma Invoice" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("PROINV_Id")%>' CommandName="lnkPrintProformaInvoice">&nbsp;Print&nbsp;</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkModifyProformaInvoice" runat="server" ToolTip="Modify Proforma Invoice" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("PROINV_Id") %>' CommandName="lnkModifyProformaInvoice" Visible="false">&nbsp;Modify&nbsp;</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkApproveProformaInvoice" runat="server" ToolTip="Approve Proforma Invoice" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("PROINV_Id") %>' CommandName="lnkApproveProformaInvoice" Visible="false">&nbsp;Approve&nbsp;</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEmailProformaInvoice" runat="server" ToolTip="Email Proforma Invoice" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("PROINV_Id") %>' CommandName="lnkEmailProformaInvoice" Visible="false">&nbsp;Email&nbsp;</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApproveStatus" runat="server" Text='<%# Bind("PROINV_ApproveStatus_bit") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        
                    </div>
              <%--  </div>--%>
            </asp:Panel>
        </asp:Panel>
    </div>
    <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdModifyProformaInvoice.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdModifyProformaInvoice.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdModifyProformaInvoice.ClientID%>').offset().top);

        });
    </script>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
    </script>
    <%--<script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>--%>
</asp:Content>
