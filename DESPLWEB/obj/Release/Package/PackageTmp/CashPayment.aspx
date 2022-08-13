<%@ Page Title="Cash / Bank Payment" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="CashPayment.aspx.cs" Inherits="DESPLWEB.CashPayment" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="400px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <table style="width: 100%">
                <tr>
                    <td width="12%">
                        <asp:Label ID="lblCashBankPaymentId" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblVoucherNo" runat="server" Text="Voucher No."></asp:Label>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtVoucherNo" MaxLength="50" Width="200px" runat="server"></asp:TextBox>
                    </td>
                    <td width="12%">
                        <asp:Label ID="lblVoucherDate" runat="server" Text="Voucher Date"></asp:Label>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtVoucherDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtVoucherDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtVoucherDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>                    
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Payment Status" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButton ID="optCash" runat="server" Text="Cash" GroupName="g1" OnCheckedChanged="optBankVoucherType_CheckedChanged" AutoPostBack="true" Visible="false"/> &nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="optBankVoucherType" runat="server" Text="Bank Voucher Type" GroupName="g1" OnCheckedChanged="optBankVoucherType_CheckedChanged" AutoPostBack="true" Visible="false"/>
                    </td>
                    <td>
                        <asp:Label ID="lblBankVoucherType" runat="server" Text="Bank Voucher Type" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBankVoucherType" runat="server" Width="150px" Visible="false">
                            <asp:ListItem Text="---Select---"></asp:ListItem>
                            <asp:ListItem Text="BOM PAYMENT"></asp:ListItem>
                            <asp:ListItem Text="HDFC CC"></asp:ListItem>
                            <asp:ListItem Text="HDFC Payment"></asp:ListItem>
                            <asp:ListItem Text="NKGSB"></asp:ListItem>
                            <asp:ListItem Text="Payment"></asp:ListItem>
                            <asp:ListItem Text="HDFC Mumbai"></asp:ListItem>
                            <asp:ListItem Text="HDFC Nashik"></asp:ListItem>
                            <asp:ListItem Text="Sarswat Payment"></asp:ListItem>
                            <asp:ListItem Text="SBI Payment"></asp:ListItem>
                        </asp:DropDownList>
                    </td>                    
                </tr>

                <tr>
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr> 
                <tr>
                    <td colspan="4">
                        <asp:Panel ID="pnlDetailGrid" runat="server" Height="200" Width="920px" BorderWidth="1px" ScrollBars="Auto">
                            <asp:GridView ID="grdDetails" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                                CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Horizontal"
                                Width="100%" CellPadding="0" CellSpacing="0" ShowFooter="false" OnRowDataBound="grdDetails_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnAddRow" runat="server" OnCommand="imgBtnAddRow_Click"
                                                ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                ToolTip="Add Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="18px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnDeleteRow" runat="server" OnCommand="imgBtnDeleteRow_Click"
                                                ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                ToolTip="Delete Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="Item Name" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtItemName" runat="server" MaxLength="250" Width="162px" AutoPostBack="true" OnTextChanged="txtItemName_TextChanged" Visible="false"></asp:TextBox>
                                            <asp:AutoCompleteExtender ServiceMethod="GetItemName" MinimumPrefixLength="1" OnClientItemSelected="ItemItemSelected"
                                                CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtItemName"
                                                ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                                            </asp:AutoCompleteExtender>
                                            <asp:Label ID="lblItemId" runat="server" Text="0" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ledger Name" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLedgerName" runat="server" MaxLength="250" Width="192px"></asp:TextBox>
                                            <asp:AutoCompleteExtender ServiceMethod="GetLedgerName" MinimumPrefixLength="1" OnClientItemSelected="LedgerItemSelected"
                                                CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtLedgerName"
                                                ID="AutoCompleteExtender3" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                                            </asp:AutoCompleteExtender>
                                            <asp:Label ID="lblLedgerId" runat="server" Text="0" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cost Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlCostCenter" runat="server" Width="150px"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Category" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlCategory" runat="server" Width="150px"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAmount" runat="server" EnableViewState="False" MaxLength="250"
                                                Width="150px" onkeypress="return checkDecimal(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr> 
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblTotal" runat="server" Text="Total Amount"></asp:Label>
                        &nbsp;&nbsp;&nbsp; 
                        <asp:Label ID="lblTotalAmount" runat="server" Text="0.00"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                        <asp:HiddenField ID="hfLedgerId" runat="server" />
                        <asp:HiddenField ID="hfItemId" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblNarration" runat="server" Text="Narration"></asp:Label>
                        &nbsp;&nbsp;&nbsp; 
                        <asp:TextBox ID="txtNarration" Width="250px" runat="server"></asp:TextBox>
                    </td>                    
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="#990033" Text="lblMsg"
                            Visible="False"></asp:Label>
                    </td>
                    <td colspan="1" align="center">
                        <asp:LinkButton ID="lnkCalculate" runat="server" CssClass="LnkOver" OnClick="lnkCalculate_Click"
                            Style="text-decoration: underline; font-weight: bold;" >Calculate</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" OnClick="lnkSave_Click"
                            Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V2">Save</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkClear" runat="server" CssClass="LnkOver" OnClick="lnkClear_Click"
                            Style="text-decoration: underline; font-weight: bold;">Clear</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkExit" runat="server" CssClass="LnkOver" OnClick="lnkExit_Click"
                            Style="text-decoration: underline; font-weight: bold;">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">        
        function LedgerItemSelected(sender, e) {
            $get("<%=hfLedgerId.ClientID %>").value = e.get_value();
        }
        function ItemItemSelected(sender, e) {
            $get("<%=hfItemId.ClientID %>").value = e.get_value();
        }
        function onlyAlphabets(e, t) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 8)
                    return true;
                else
                    return false;
            }
            catch (err) {
                alert(err.Description);
            }
        }

        function CheckNumeric(e) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if ((charCode >= 48 && charCode <= 57) || charCode == 8)
                    return true;
                else
                    return false;
            }
            catch (err) {
                alert(err.Description);
            }

        }

        function checkDecimal(e) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if ((charCode >= 48 && charCode <= 57) || charCode == 8 || charCode == 46)
                    return true;
                else
                    return false;
            }
            catch (err) {
                alert(err.Description);
            }

        }

        function checkDecimal1(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 46))) {
                    x.value = '';
                    x.focus();
                    alert("Only Numeric Values Allowed");

                    return false;
                }

            }
            return true;
        }

        function CheckAlphaNumeric(e) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if ((charCode >= 48 && charCode <= 57) || (charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 8 || charCode == 32)
                    return true;
                else
                    return false;
            }
            catch (err) {
                alert(err.Description);
            }

        }
    </script>
</asp:Content>

