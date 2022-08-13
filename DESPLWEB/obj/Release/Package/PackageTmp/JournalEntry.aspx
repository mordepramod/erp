<%@ Page Title=" Journal Entry" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    Theme="duro" AutoEventWireup="true" CodeBehind="JournalEntry.aspx.cs" Inherits="DESPLWEB.JournalEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 480px;">
        <asp:Panel ID="Panel1" runat="server" Width="100%">
            <asp:Panel ID="Panel2" runat="server" Width="100%">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td width="15%">
                                    <asp:Label ID="Label1" runat="server" Text="Note No: "></asp:Label>
                                </td>
                                <td width="11%">
                                    <asp:TextBox ID="txt_DBNoteNo" ReadOnly="true" Width="155px" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RadioButton ID="Rdn_Debit" runat="server" GroupName="c" Checked="false" AutoPostBack="true"
                                        OnCheckedChanged="Rdn_Debit_CheckedChanged" />
                                    <asp:Label ID="Label5" runat="server" Text="Debit Note"></asp:Label>
                                    <asp:RadioButton ID="Rdn_Credit" runat="server" GroupName="c" Checked="false" AutoPostBack="true"
                                        OnCheckedChanged="Rdn_Credit_CheckedChanged" />
                                    <asp:Label ID="Label6" runat="server" Text="Credit Note"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td width="10%">
                                    <asp:Label ID="Label3" runat="server" Text="Status"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_Status" Width="105px" runat="server">
                                        <asp:ListItem Text="Ok" Value="false" />
                                        <asp:ListItem Text="Hold" Value="true" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="lblJournalNoteNo" runat="server" Text="" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Shri/M/s."></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_Client" runat="server" Width="350px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                                    <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                                        CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                                        ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                                    </asp:AutoCompleteExtender>
                                    <asp:HiddenField ID="hfClientId" runat="server" />
                                    <asp:Label ID="lblClientId" runat="server" Visible="false"></asp:Label>
                                    <%--<asp:DropDownList ID="ddl_ClientName" Width="350px" runat="server" OnSelectedIndexChanged="ddl_ClientName_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>--%>
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Date" Width="100px" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                        Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_Date">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_Date" MaskType="Date"
                                        Mask="99/99/9999" AutoComplete="false" runat="server">
                                    </asp:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_SiteName" runat="server" Text="Site Name"></asp:Label>
                                    <asp:Label ID="lbl_Amtallocated" runat="server" Visible="false" Text="Amount to be allocated"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddl_SiteName" Width="350px" runat="server" OnSelectedIndexChanged="ddl_SiteName_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_Amtallocated" Width="200px" Style="text-align: right" Visible="false"
                                        ReadOnly="true" runat="server" ></asp:TextBox>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbl_BillNo" runat="server" Text="List of Bill No."></asp:Label>
                                    <asp:CheckBox ID="chk_ServiceTax" runat="server" AutoPostBack="true" Visible="false"
                                        OnCheckedChanged="chk_ServiceTax_CheckedChanged" />
                                    <asp:Label ID="lbl_ServiceTax" runat="server" Text="Service Tax" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_ListOfBillNo" Width="105px" runat="server">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_ServiceTax" Width="100px" Visible="false" ReadOnly="true" Style="text-align: right"
                                        runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:Panel ID="PnlGrd" runat="server" Height="360px" ScrollBars="Vertical" Width="100%"
                BorderStyle="Ridge" BorderColor="AliceBlue">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdJournalEntry" OnRowDataBound="grdJournalEntry_RowDataBound"
                            runat="server" ShowFooter="true" AutoGenerateColumns="False" SkinID="gridviewSkin"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Ledger Name"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddl_LedgerName" OnSelectedIndexChanged="ddl_LedgerName_SelectedIndexChanged"
                                            AutoPostBack="true" BorderWidth="0px" Width="100px" runat="server">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbl_Tota" runat="server" Text="Total Amount :"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Bill/Ledger"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_BillLedger" BorderWidth="0px" ReadOnly="true" Width="200px"
                                            Style="text-align: center" runat="server" Text='<%#Eval("txt_BillLedger") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbl_TotalAmt" runat="server" Text="Total Amount :"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Bill Date" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_BillDate" BorderWidth="0px" ReadOnly="true" Width="80px"
                                            Style="text-align: center" runat="server" Text='<%#Eval("txt_BillDate") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterStyle BackColor="White" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Bill Amount"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_BillAmt" BorderWidth="0px" ReadOnly="true" Width="100px"
                                            Style="text-align: right" runat="server" Text='<%#Eval("txt_BillAmt") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterStyle BackColor="White" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Bal. Amount"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_BalAmt" BorderWidth="0px" ReadOnly="true" Width="100px" Style="text-align: right"
                                            runat="server" Text='<%#Eval("txt_BalAmt") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_TotalBalAmt" BorderWidth="0px" ReadOnly="true" Width="100px"
                                            Style="text-align: right" runat="server" ></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Cost Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_CostCenter" BorderWidth="0px" ReadOnly="true" Width="100px"
                                            runat="server" Text='<%#Eval("txt_CostCenter") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterStyle BackColor="White" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Cost Catagory"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_CostCatagory" BorderWidth="0px" ReadOnly="true" Width="100px"
                                            runat="server" Text='<%#Eval("txt_CostCatagory") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterStyle BackColor="White" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Amount" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Amount" BorderWidth="0px" Width="100px" runat="server" autocomplete="off"
                                            Style="text-align: right" onkeyup="checkDecimal(this)" 
                                            Text=""></asp:TextBox> <%--AutoPostBack="true" OnTextChanged="txt_Amount_TextChanged"--%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_TotalAmt" BorderWidth="0px" ReadOnly="true" Width="100px" Style="text-align: right"
                                            runat="server"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Debit" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Debit" BorderWidth="0px" Width="100px" Style="text-align: right"
                                            onkeyup="checkDecimal(this)"  
                                            runat="server" Text='<%#Eval("txt_Debit") %>'></asp:TextBox> <%--AutoPostBack="true" OnTextChanged="txt_Debit_TextChanged"--%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_TotalDebitAmt" BorderWidth="0px" ReadOnly="true" Width="100px"
                                            Style="text-align: right" runat="server" ></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Credit" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Credit" BorderWidth="0px" Width="100px" Style="text-align: right"
                                            onkeyup="checkDecimal(this)" 
                                            runat="server" Text='<%#Eval("txt_Credit") %>'></asp:TextBox> <%-- AutoPostBack="true" OnTextChanged="txt_Credit_TextChanged"--%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txt_TotalCreditAmt" BorderWidth="0px" ReadOnly="true" Width="100px"
                                            Style="text-align: right" runat="server"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" HeaderText="">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Id" Visible="false" runat="server" Text='<%#Eval("txt_Id") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <table width="100%">
                <tr>
                    <td width="10%">
                        <asp:Label ID="Label9" runat="server" Text="Note"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Note" Width="400px" runat="server" autocomplete="off"></asp:TextBox>
                    </td>
                    <td align="right">

                        <asp:LinkButton ID="LnkBtnSave" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="LnkBtnSave_Click" CssClass="SimpleColor">Save</asp:LinkButton>
                        &nbsp; &nbsp; &nbsp;
            
                        <asp:LinkButton ID="lnkNext" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="LnkNext_Click" CssClass="SimpleColor">Next</asp:LinkButton>
                        &nbsp; &nbsp; &nbsp;           
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Bold="true" Visible="false" Style="text-decoration: underline;"
                              OnClick="lnkPrint_Click">Print </asp:LinkButton>&nbsp; &nbsp; &nbsp;
                        
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <script type="text/javascript">

        function checkDecimal(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 46))) {
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
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }

    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
