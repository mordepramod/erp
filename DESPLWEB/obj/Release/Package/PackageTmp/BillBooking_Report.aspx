<%@ Page Title="Bill Booking Report" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="BillBooking_Report.aspx.cs" Inherits="DESPLWEB.BillBooking_Report" Theme="duro"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>                    
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
                    <td colspan="4">&nbsp;
                    </td>
                </tr>
            </table>

            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="420px"
                Width="940px" BorderColor="AliceBlue">
                <asp:UpdatePanel ID="UpdatePanelGrd" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvDetailsView" runat="server" AutoGenerateColumns="False" Width="100%"
                            ShowFooter="false" SkinID="gridviewSkin" >
                            <Columns>
                                <asp:BoundField DataField="NameoftheSupplier" HeaderText="Name of the Supplier" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SupplierGSTIN" HeaderText="Supplier GSTIN" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PlaceofSupply" HeaderText="Place of Supply" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TotalInvoiceValue" HeaderText="Total Invoice Value" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TaxableValue" HeaderText="Taxable Value" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="GSTRate" HeaderText="GST Rate" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IGSTAmountofTaxPaid" HeaderText="IGST Amount of Tax Paid" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CGSTAmountofTaxPaid" HeaderText="CGST Amount of Tax Paid" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SGST/UTAmountofTaxPaid" HeaderText="SGST/UT Amount of Tax Paid" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CessAmountofTaxPaid" HeaderText="Cess Amount of Tax Paid" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="EligibilityforITC" HeaderText="Eligibility for ITC" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IGSTAmountofITCavailable" HeaderText="IGST Amount of ITC available" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CGSTAmountofITCavailable" HeaderText="CGST Amount of ITC available" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SGST/UTAmountITCavailable" HeaderText="SGST/UT Amount ITC available" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CessAmountofITCavailable" HeaderText="Cess Amount of ITC available" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="InvoiceType" HeaderText="Invoice Type" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReverseCharge" HeaderText="Reverse Charge (Indicate if supply attracts reverse charge)" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="HSN" HeaderText="HSN" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SAC" HeaderText="SAC" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UQC" HeaderText="UQC" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:Panel>
    </div>
</asp:Content>
