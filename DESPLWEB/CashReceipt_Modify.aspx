<%@ Page Title=" Modify Receipt" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" Theme="duro" CodeBehind="CashReceipt_Modify.aspx.cs" Inherits="DESPLWEB.CashReceipt_Modify" %>

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
                    <td colspan="3" align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClient" runat="server" Text="Client"></asp:Label>
                    </td>
                    <td id="Td1" runat="server" align="left" width="440px" colspan="2">
                        <asp:DropDownList ID="ddlClient" Width="400px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Amount"></asp:Label>
                    </td>
                    <td id="Td2" runat="server" align="left">
                        <asp:DropDownList ID="ddlAmount" Width="200px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlAmount_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Payment Type"></asp:Label>
                    </td>
                    <td id="Td4" runat="server" align="left" colspan="2">
                        <asp:DropDownList ID="ddlPaymttype" Width="200px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlPaymttype_SelectedIndexChanged">
                            <asp:ListItem Text="Cash" Value="False" />
                            <asp:ListItem Text="Cheque" Value="True" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Status"></asp:Label>
                    </td>
                    <td id="Td3" runat="server" align="left" colspan="2">
                        <asp:DropDownList ID="ddlStatus" Width="200px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                            <asp:ListItem Text="Ok" Value="False" />
                            <asp:ListItem Text="Hold" Value="True" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="Approved Status"></asp:Label>
                    </td>
                    <td runat="server" align="left">
                        <asp:DropDownList ID="ddlApprStatus" Width="200px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlApprStatus_SelectedIndexChanged">
                            <asp:ListItem Text="Pending" Value="False" />
                            <asp:ListItem Text="Approved" Value="True" />
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:DropDownList ID="ddlBank" Width="100px" runat="server" >
                            <asp:ListItem Text="NKGSB"  />
                            <asp:ListItem Text="HDFC Pune 1" />
                            <asp:ListItem Text="HDFC Pune 2" />
                            <asp:ListItem Text="HDFC Mumbai" />
                            <asp:ListItem Text="HDFC Nashik" />
                        </asp:DropDownList>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrntBankSlip" OnClick="lnkPrntBankSlip_Click" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;"> Print Bank Slip </asp:LinkButton>
                            <asp:LinkButton ID="lnkPrntBankSlipExcel" OnClick="lnkPrntBankSlipExcel_Click" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;" Visible="false"> Print Bank Slip Excel </asp:LinkButton>
                    </td>
                    <td style="width: 15%">
                      <asp:TextBox ID="txtReceiptNo" runat="server" Width="55px" MaxLength="8" 
                            onchange="checkNum(this)"></asp:TextBox>
                    <asp:LinkButton ID="lnkPrintReceipt" OnClick="lnkPrintReceipt_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Print Receipt</asp:LinkButton>
                            </td>
                            <td align="right">
                      <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Print</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="300px"
                Width="940px" BorderColor="AliceBlue">
                <asp:UpdatePanel ID="UpdatePanelGrd" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvDetailsView" runat="server" AutoGenerateColumns="False" Width="100%"
                            ShowFooter="true" SkinID="gridviewSkin" OnRowDataBound="gvDetailsView_RowDataBound"
                            OnRowCommand="gvDetailsView_RowCommand">
                            <Columns>
                                <asp:TemplateField>
                                    <%--<HeaderTemplate>
                                        <asp:CheckBox ID="cbxSelectAll" OnClick="javascript:SelectAllCheckboxes(this);" AutoPostBack="true" runat="server" 
                                         OnCheckedChanged="cbxSelectAllOnCheckedChanged"/>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbxSelect" OnClick="javascript:OnOneCheckboxSelected(this);" AutoPostBack="true"
                                            OnCheckedChanged="cbxSelectOnCheckedChanged" runat="server" CssClass='<%#Eval( "Cash_ReceiptNo" )+ ";" + Eval("Cash_BankName_var") + ";"+ Eval("Cash_Branch_var")  + ";"+ Eval("Cash_ChequeNo_int") + ";"+ Eval("CL_Id") %>' />
                                    </ItemTemplate>--%>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cbxSelectAll" AutoPostBack="true" runat="server" 
                                         OnCheckedChanged="cbxSelectAllOnCheckedChanged"/>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbxSelect" AutoPostBack="true" runat="server"
                                            OnCheckedChanged="cbxSelectOnCheckedChanged" CssClass='<%#Eval( "Cash_ReceiptNo" )+ ";" + Eval("Cash_BankName_var") + ";"+ Eval("Cash_Branch_var")  + ";"+ Eval("Cash_ChequeNo_int") + ";"+ Eval("CL_Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%--<asp:LinkButton ID="lnkModifyReceipt" runat="server" ToolTip="Modify Receipt" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Cash_ReceiptNo") + ";" + Eval("CashDetail_BillNo_int") %>'
                                            CommandName="Modify Receipt">Modify</asp:LinkButton>--%>
                                            <asp:LinkButton ID="lnkModifyReceipt" runat="server" ToolTip="Modify Receipt" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Cash_ReceiptNo") + ";"   %>'
                                            CommandName="Modify Receipt">Modify</asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Cash_ReceiptNo" HeaderText="Receipt No">
                                    <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Cash_Date_date" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Cash_Amount_num" HeaderText="Receipt Amount" DataFormatString="{0:f2}">
                                    <HeaderStyle HorizontalAlign="Center" Width="200px" />
                                    <ItemStyle HorizontalAlign="Center" Width="18px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Cash_TDS_num" HeaderText="TDS Amount" DataFormatString="{0:f2}">
                                    <HeaderStyle HorizontalAlign="Center" Width="200px" />
                                    <ItemStyle HorizontalAlign="Center" Width="180px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name">
                                    <HeaderStyle HorizontalAlign="Center" Width="400px" />
                                    <ItemStyle HorizontalAlign="left" Width="350px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Cash_PaymentType_bit" HeaderText="Payment Type">
                                    <HeaderStyle HorizontalAlign="Center" Width="82px" />
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CashDetail_Status_bit" HeaderText="Status">
                                    <HeaderStyle HorizontalAlign="Center" Width="82px" />
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CashDetail_ApproveStatus_bit" HeaderText="Approved Status">
                                    <HeaderStyle HorizontalAlign="Center" Width="82px" />
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:BoundField>
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                          <asp:LinkButton ID="lnkApproveReceipt" runat="server" ToolTip="Approve" Visible="false"
                                            Style="text-decoration: underline;" CommandArgument='<%#Eval("Cash_ReceiptNo") + ";"   %>'
                                            CommandName="Approve">Approve</asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>
                               <%--  <asp:TemplateField ItemStyle-Width="30px">
                                    <ItemTemplate>
                                          <asp:LinkButton ID="lnkPrintReceipt" runat="server" ToolTip="Print"
                                            Style="text-decoration: underline;" CommandArgument='<%#Eval("Cash_ReceiptNo") + ";"   %>'
                                            CommandName="Print">Print</asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>--%>
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


        function checkNum(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    x.value = '';
                    x.focus();

                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Numeric Values Allowed";
                    return false;
                }
                else {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                }

            }
            return true;
        }


    </script>
</asp:Content>
