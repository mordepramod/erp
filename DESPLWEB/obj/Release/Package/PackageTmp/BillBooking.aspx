<%@ Page Title="Bill Booking" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="BillBooking.aspx.cs" Inherits="DESPLWEB.BillBooking" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <table style="width: 100%">
                <tr>
                    <td width="7%">
                        <asp:Label ID="lblBookingNo" runat="server" Text="Booking No."></asp:Label>
                    </td>
                    <td width="12%">
                        <asp:Label ID="lblBookingId" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:TextBox ID="txtBookingNo" Width="200px" ReadOnly="true" runat="server"></asp:TextBox>
                    </td>
                    <td width="12%">
                        <asp:Label ID="lblBookDate" runat="server" Text="Book Date"></asp:Label>
                    </td>
                    <td width="10%">
                        <asp:TextBox ID="txtBookDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtBookDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtBookDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td width="10%">
                        <asp:Label ID="lblSupplierInvoiceNo" runat="server" Text="Supplier Invoice No."></asp:Label>
                    </td>
                    <td width="18%">
                        <asp:TextBox ID="txtSupplierInvoiceNo" Width="145px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblVendor" runat="server" Text="Vendor"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVendor" runat="server" Width="200px" AutoPostBack="true" OnTextChanged="txtVendor_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblInvoiceDate" runat="server" Text="Invoice Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtInvoiceDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtInvoiceDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtInvoiceDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblType" runat="server" Text="Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBookingType" runat="server" Width="150px">
                            <asp:ListItem Text="---Select---"></asp:ListItem>
                            <asp:ListItem Text="Journal"></asp:ListItem>
                            <asp:ListItem Text="Purchase"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;  
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Composite Tax Payer"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCompTaxPayer" runat="server" Width="150px">
                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="150px">
                            <asp:ListItem Text="Ok" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Hold" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:AutoCompleteExtender ServiceMethod="GetVendorName" MinimumPrefixLength="1" OnClientItemSelected="VendorItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtVendor"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfVendorId" runat="server" />
                    </td>
                    <td></td>
                    <td>
                        <asp:Label ID="lblVendorId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                </tr>
                <%--<tr>
                    <td colspan="6">
                        &nbsp;
                    </td>
                </tr> --%>
                <tr>
                    <td colspan="6">
                        <asp:Panel ID="pnlDetailGrid" runat="server" Height="300px" Width="920px" BorderWidth="1px" ScrollBars="Auto">
                            <asp:GridView ID="grdDetails" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                                CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Horizontal"
                                Width="100%" CellPadding="0" CellSpacing="0" ShowFooter="true" OnRowDataBound="grdDetails_RowDataBound">
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
                                    <asp:TemplateField HeaderText="Ledger Name" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLedgerName" runat="server" MaxLength="250" Width="192px"></asp:TextBox>
                                            <asp:AutoCompleteExtender ServiceMethod="GetLedgerName" MinimumPrefixLength="1" OnClientItemSelected="LedgerItemSelected"
                                                CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtLedgerName"
                                                ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
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
                                                Width="100px" onkeypress="return checkDecimal(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HSN code" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtHSNcode" runat="server" EnableViewState="False" MaxLength="250"
                                                Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SAC code" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSACcode" runat="server" EnableViewState="False" MaxLength="250"
                                                Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlType" runat="server" Width="100px">
                                                <asp:ListItem Text="---Select---"></asp:ListItem>
                                                <asp:ListItem Text="Capital Goods"></asp:ListItem>
                                                <asp:ListItem Text="Input Services"></asp:ListItem>
                                                <asp:ListItem Text="Input"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CGST %" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlCgstPercent" runat="server" Width="100px">
                                                <asp:ListItem Text="---Select---"></asp:ListItem>
                                                <asp:ListItem Text="0"></asp:ListItem>
                                                <asp:ListItem Text="2.5"></asp:ListItem>
                                                <asp:ListItem Text="6"></asp:ListItem>
                                                <asp:ListItem Text="9"></asp:ListItem>
                                                <asp:ListItem Text="14"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SGST %" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlSgstPercent" runat="server" Width="100px">
                                                <asp:ListItem Text="---Select---"></asp:ListItem>
                                                <asp:ListItem Text="0"></asp:ListItem>
                                                <asp:ListItem Text="2.5"></asp:ListItem>
                                                <asp:ListItem Text="6"></asp:ListItem>
                                                <asp:ListItem Text="9"></asp:ListItem>
                                                <asp:ListItem Text="14"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IGST %" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlIgstPercent" runat="server" Width="100px">
                                                <asp:ListItem Text="---Select---"></asp:ListItem>
                                                <asp:ListItem Text="0"></asp:ListItem>
                                                <asp:ListItem Text="5"></asp:ListItem>
                                                <asp:ListItem Text="12"></asp:ListItem>
                                                <asp:ListItem Text="18"></asp:ListItem>
                                                <asp:ListItem Text="28"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CGST Amount" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCgstAmount" runat="server" EnableViewState="False" MaxLength="250"
                                                Width="100px" onkeypress="return checkDecimal(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SGST Amount" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSgstAmount" runat="server" EnableViewState="False" MaxLength="250"
                                                Width="100px" onkeypress="return checkDecimal(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IGST Amount" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtIgstAmount" runat="server" EnableViewState="False" MaxLength="250"
                                                Width="100px" onkeypress="return checkDecimal(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dr/Cr" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlDrCr" runat="server" Width="100px">
                                                <asp:ListItem Text="---Select---"></asp:ListItem>
                                                <asp:ListItem Text="Debit"></asp:ListItem>
                                                <asp:ListItem Text="Credit"></asp:ListItem>
                                               </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" EnableViewState="False"
                                                Width="200px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UQC" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlUqc" runat="server" Width="100px">
                                                <asp:ListItem Text="---Select---"></asp:ListItem>
                                                <asp:ListItem Text="BAG"></asp:ListItem>
                                                <asp:ListItem Text="BAL"></asp:ListItem>
                                                <asp:ListItem Text="BDL"></asp:ListItem>
                                                <asp:ListItem Text="BKL"></asp:ListItem>
                                                <asp:ListItem Text="BOU"></asp:ListItem>
                                                <asp:ListItem Text="BOX"></asp:ListItem>
                                                <asp:ListItem Text="BTL"></asp:ListItem>
                                                <asp:ListItem Text="BUN"></asp:ListItem>
                                                <asp:ListItem Text="CAN"></asp:ListItem>
                                                <asp:ListItem Text="CBM"></asp:ListItem>
                                                <asp:ListItem Text="CCM"></asp:ListItem>
                                                <asp:ListItem Text="CMS"></asp:ListItem>
                                                <asp:ListItem Text="CTN"></asp:ListItem>
                                                <asp:ListItem Text="DOZ"></asp:ListItem>
                                                <asp:ListItem Text="DRM"></asp:ListItem>
                                                <asp:ListItem Text="GGR"></asp:ListItem>
                                                <asp:ListItem Text="GMS"></asp:ListItem>
                                                <asp:ListItem Text="GRS"></asp:ListItem>
                                                <asp:ListItem Text="GYD"></asp:ListItem>
                                                <asp:ListItem Text="KGS"></asp:ListItem>
                                                <asp:ListItem Text="KLR"></asp:ListItem>
                                                <asp:ListItem Text="KME"></asp:ListItem>
                                                <asp:ListItem Text="MLT"></asp:ListItem>
                                                <asp:ListItem Text="MTR"></asp:ListItem>
                                                <asp:ListItem Text="MTS"></asp:ListItem>
                                                <asp:ListItem Text="NOS"></asp:ListItem>
                                                <asp:ListItem Text="PAC"></asp:ListItem>
                                                <asp:ListItem Text="PCS"></asp:ListItem>
                                                <asp:ListItem Text="PRS"></asp:ListItem>
                                                <asp:ListItem Text="QTL"></asp:ListItem>
                                                <asp:ListItem Text="ROL"></asp:ListItem>
                                                <asp:ListItem Text="SET"></asp:ListItem>
                                                <asp:ListItem Text="SQF"></asp:ListItem>
                                                <asp:ListItem Text="SQM"></asp:ListItem>
                                                <asp:ListItem Text="SQY"></asp:ListItem>
                                                <asp:ListItem Text="TBS"></asp:ListItem>
                                                <asp:ListItem Text="TGM"></asp:ListItem>
                                                <asp:ListItem Text="THD"></asp:ListItem>
                                                <asp:ListItem Text="TON"></asp:ListItem>
                                                <asp:ListItem Text="TUB"></asp:ListItem>
                                                <asp:ListItem Text="UGS"></asp:ListItem>
                                                <asp:ListItem Text="UNT"></asp:ListItem>
                                                <asp:ListItem Text="YDS"></asp:ListItem>
                                                <asp:ListItem Text="OTH"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQty" runat="server" EnableViewState="False" MaxLength="10"
                                                Width="100px" onkeypress="return CheckNumeric(event,this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblTotalNetPayble" runat="server" Text="Total Net Payble"></asp:Label>
                        &nbsp;&nbsp;&nbsp; 
                        <asp:Label ID="lblTotalNetPaybleAmount" runat="server" Text="0.00"></asp:Label>
                    </td>
                    <td colspan="3">&nbsp;
                        <asp:HiddenField ID="hfLedgerId" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblNarration" runat="server" Text="Narration"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtNarration" Width="330px" runat="server"></asp:TextBox>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblComment" runat="server" Text="Comment"></asp:Label>
                        &nbsp;&nbsp;&nbsp;                       
                        <asp:TextBox ID="txtComment" Width="330px" runat="server"></asp:TextBox>
                    </td>

                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="#990033" Text="lblMsg"
                            Visible="False"></asp:Label>
                    </td>
                    <td colspan="2" align="center">
                        <asp:LinkButton ID="lnkCalculate" runat="server" CssClass="LnkOver" OnClick="lnkCalculate_Click"
                            Style="text-decoration: underline; font-weight: bold;">Calculate</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" OnClick="lnkSave_Click"
                            Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V2">Save</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkClear" runat="server" CssClass="LnkOver" OnClick="lnkClear_Click"
                            Style="text-decoration: underline; font-weight: bold;">Clear</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        function VendorItemSelected(sender, e) {
            $get("<%=hfVendorId.ClientID %>").value = e.get_value();
        }
        function LedgerItemSelected(sender, e) {
            $get("<%=hfLedgerId.ClientID %>").value = e.get_value();
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


