<%@ Page Title="Tally Transfer" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="TallyTransfer.aspx.cs" Inherits="DESPLWEB.TallyTransfer"
    Theme="duro" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date : "></asp:Label>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txtFromDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="Label4" runat="server" Text="To Date : "></asp:Label>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txtToDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp;
                        <asp:Label ID="lblBankName" runat="server" Text="Bank A/c : " Visible="false"></asp:Label>
                        &nbsp; &nbsp;
                        <asp:DropDownList ID="ddlBankname" Width="200px" Visible="false" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton ID="optBill" Text="Bill" runat="server" GroupName="g1" OnCheckedChanged="optBill_CheckedChanged"
                            AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optReceipt" Text="Receipt" runat="server" GroupName="g1" OnCheckedChanged="optBill_CheckedChanged"
                            AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optDebitNote" Text="Debit Note" runat="server" GroupName="g1"
                            OnCheckedChanged="optBill_CheckedChanged" AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optCreditNote" Text="Credit Note" runat="server" GroupName="g1"
                            OnCheckedChanged="optBill_CheckedChanged" AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optAdvanceReceipt" Text="Advance Receipt" runat="server" GroupName="g1"
                            OnCheckedChanged="optBill_CheckedChanged" AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optAdjustAdvanceNote" Text="Adjust Advance Note" runat="server"
                            GroupName="g1" OnCheckedChanged="optBill_CheckedChanged" AutoPostBack="true" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="optCashPayment" Text="Cash Payment" runat="server"
                            GroupName="g1" OnCheckedChanged="optBill_CheckedChanged" AutoPostBack="true" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkTransfer" Font-Bold="true" runat="server" Font-Underline="true"
                            OnClick="lnkTransfer_Click">Transfer</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 5px">
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlList" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="370px"
                Width="940px" BorderColor="AliceBlue">

                  <%--<div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 350px; overflow: auto">--%>
                <asp:GridView ID="grdBill" runat="server"  SkinID="gridviewSkin1" AutoGenerateColumns="False" BackColor="#F7F6F3"
                            CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Horizontal"
                            Width="100%" CellPadding="0" CellSpacing="0">
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
                        <asp:TemplateField HeaderText="Bill No." HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblBillNo" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bill Date" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblBillDate" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bill Amount" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblBillAmount" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Client Name " HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblClientName" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblSiteName" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Narration" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblNarration" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amt" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblAmt" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Service Tax Amt" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceTaxAmt" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Test Type" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblTestType" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Service Tax" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceTax" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bill Status" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblBillStatus" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Travelling" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblTravelling" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Discount Amt" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblDiscountAmt" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mkt User" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblMktUser" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rounding Off" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblRoundingOff" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mobilization" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblMobilization" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Swachh Bharat Cess" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblSwachhBharatCess" runat="server" Visible="false" ></asp:Label>
                                <asp:Label ID="lblSwachhBharatCessAmt" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Kisan Krishi Cess" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblKisanKrishiCess" runat="server" Visible="false" ></asp:Label>
                                <asp:Label ID="lblKisanKrishiCessAmt" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CGST" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblCgst" runat="server" Visible="false" ></asp:Label>
                                <asp:Label ID="lblCgstAmt" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SGST" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblSgst" runat="server" Visible="false" ></asp:Label>
                                <asp:Label ID="lblSgstAmt" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IGST" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblIgst" runat="server" Visible="false" ></asp:Label>
                                <asp:Label ID="lblIgstAmt" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <%--</div><//div>--%>
            </asp:Panel>
        </asp:Panel>
    </div>
     <%--<script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdBill.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdBill.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdBill.ClientID%>').offset().top);

        });
    </script>--%>
</asp:Content>
