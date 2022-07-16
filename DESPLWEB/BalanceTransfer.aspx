<%@ Page Title="Balance Transfer" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="BalanceTransfer.aspx.cs" Inherits="DESPLWEB.BalanceTransfer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td width="10%">
                        <asp:Label ID="lblReceiptNo" runat="server" Text="Receipt No."></asp:Label>
                    </td>
                    <td style="width: 35%">
                        <asp:DropDownList ID="ddlReceiptNo" Width="155px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlReceiptNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td width="8%">
                        <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
                    </td>
                    <td style="width: 16%">
                        <asp:TextBox ID="txtDate" Width="120px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td width="13%">
                        <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                    </td>
                    <td style="width: 18%">
                        <asp:DropDownList ID="ddlStatus" Width="120px" runat="server">
                            <asp:ListItem Text="Ok" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Hold" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClient" runat="server" Text="Client"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblUserId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblBillNo" runat="server" Text="Bill No."></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillNo" Width="120px" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCrDrAmount" Width="110px" runat="server">
                            <asp:ListItem Text="Debit Amount"></asp:ListItem>
                            <asp:ListItem Text="Credit Amount"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCrDrAmount" Width="120px" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkAdd" OnClick="lnkAdd_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;" onchange="checkDecimal(this);">Add</asp:LinkButton>&nbsp; &nbsp;&nbsp;&nbsp;                        
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        &nbsp;
                    </td>
                </tr>
              </table>
                        <asp:Panel ID="pnlCrDrEntry" runat="server" ScrollBars="Auto" BorderStyle="Ridge"
                            Height="280px" Width="940px" BorderColor="AliceBlue">
                             <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 280px; overflow: auto">
                            <asp:GridView ID="grdCrDrEntry" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                                CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                                Width="100%" CellPadding="0" CellSpacing="0">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnEditRow" runat="server" OnCommand="imgBtnEditRow_Click"
                                                ImageUrl="Images/Edit.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                ToolTip="Edit Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnDeleteRow" runat="server" OnCommand="imgBtnDeleteRow_Click"
                                                ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                ToolTip="Delete Row" />
                                            <asp:Label ID="ClientId" runat="server" Text="" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BillNo" HeaderText="Bill No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="DebitAmount" HeaderText="Debit Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CreditAmount" HeaderText="Credit Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
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
                            </div></div>
                        </asp:Panel>
                    
                 
             
                    <div align="right">
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>
                        <caption>
                            &nbsp;
                        </caption>
                            </div>
              
           
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
     <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdCrDrEntry.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdCrDrEntry.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdCrDrEntry.ClientID%>').offset().top);

        });
    </script>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
        function checkDecimal(x) {

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
    </script>
</asp:Content>
