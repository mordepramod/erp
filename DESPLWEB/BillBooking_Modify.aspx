﻿<%@ Page Title="Modify Bill Booking / Cash Payment" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="BillBooking_Modify.aspx.cs" Inherits="DESPLWEB.BillBooking_Modify" Theme="duro" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:RadioButton ID="optBillBooking" Text="Bill Booking" runat="server" GroupName="g1" OnCheckedChanged="optBillBooking_CheckedChanged"
                            AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optCashPayment" Text="Cash Payment" runat="server" GroupName="g1" OnCheckedChanged="optCashPayment_CheckedChanged"
                            AutoPostBack="true" />
                    </td>
                    <td width="100px">
                        <asp:Label ID="Label2" runat="server" Text="Period"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_FrmDate" runat="server" Width="100px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FrmDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FrmDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txt_Todate" runat="server" Width="100px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_Todate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_Todate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp;                        
                        <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                        &nbsp; &nbsp;
                        <asp:DropDownList ID="ddlStatus" Width="70px" runat="server">
                            <asp:ListItem Text="Ok" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Hold" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp; &nbsp;                        
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/Search-32.png"
                            ToolTip="Search" Style="width: 19px; height: 18px" OnClientClick="return validate2();"
                            OnClick="ImgBtnSearch_Click" />
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Print</asp:LinkButton>
                    </td>
                    <td colspan="2" align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">&nbsp;
                    </td>
                </tr>
            </table>

            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="420px"
                Width="940px" BorderColor="AliceBlue">
                <asp:UpdatePanel ID="UpdatePanelGrd" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvDetailsView" runat="server" AutoGenerateColumns="False" Width="100%"
                            ShowFooter="false" SkinID="gridviewSkin" OnRowCommand="gvDetailsView_RowCommand">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkModifyBillBooking" runat="server" ToolTip="Modify Bill Booking" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("BILLBOOK_Id") + ";" + Eval("BILLBOOK_Vend_Id")%>'
                                            CommandName="ModifyBillBooking">Modify</asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="BILLBOOK_Id" HeaderText="Booking No.">
                                    <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BILLBOOK_Date_dt" HeaderText="Book Date" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BILLBOOK_SupplierInvoiceNo_var" HeaderText="Supplier Invoice No.">
                                    <HeaderStyle HorizontalAlign="Center" Width="110px" />
                                    <ItemStyle HorizontalAlign="left" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BILLBOOK_InvoiceDate_dt" HeaderText="Invoice Date" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BILLBOOK_NetPayableAmount_num" HeaderText="Total Net Payble" DataFormatString="{0:f2}">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BILLBOOK_Type_var" HeaderText="Type">
                                    <HeaderStyle HorizontalAlign="Center" Width="82px" />
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="VEND_FirmName_var" HeaderText="Vendor">
                                    <HeaderStyle HorizontalAlign="Center" Width="150px" />
                                    <ItemStyle HorizontalAlign="Left" Width="140px" />
                                </asp:BoundField>

                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkModifyCashPayment" runat="server" ToolTip="Modify Cash Payment" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("CASHPAY_Id") %>'
                                            CommandName="ModifyCashPayment">Modify</asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="CASHPAY_VoucherNo_var" HeaderText="Voucher No.">
                                    <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CASHPAY_VoucherDate_dt" HeaderText="Voucher Date" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CASHPAY_TotalAmount_num" HeaderText="Total Amount" DataFormatString="{0:f2}">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:Panel>
    </div>
    <script type="text/javascript">

        function OnOneCheckboxSelected(chkB) {
            var IsChecked = chkB.checked;
            var Parent = document.getElementById('<%= this.gvDetailsView.ClientID %>');
            var cbxAll;
            var items = Parent.getElementsByTagName('input');
            var bAllChecked = true;
            for (i = 0; i < items.length; i++) {
                if (chkB.checked) {
                    chkB.parentElement.parentElement.style.backgroundColor = '#FFB9B9';
                }
                else {
                    chkB.parentElement.parentElement.style.backgroundColor = '#FFFFFF';
                }
                if (items[i].id.indexOf('cbxSelectAll') != -1) {
                    cbxAll = items[i];
                    continue;
                }
                if (items[i].type == "checkbox" && items[i].checked == false) {
                    bAllChecked = false;
                    break;
                }
            }
            cbxAll.checked = bAllChecked;
        }

        function SelectAllCheckboxes(spanChk) {
            var IsChecked = spanChk.checked;
            var cbxAll = spanChk;
            var Parent = document.getElementById('<%= this.gvDetailsView.ClientID %>');
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != cbxAll.id && items[i].type == "checkbox") {
                    items[i].checked = IsChecked;
                    if (items[i].checked) {
                        items[i].parentElement.parentElement.style.backgroundColor = '#FFB9B9';
                    }
                    else {
                        items[i].parentElement.parentElement.style.backgroundColor = '#FFFFFF';
                    }
                }
            }
        }
    </script>
</asp:Content>
