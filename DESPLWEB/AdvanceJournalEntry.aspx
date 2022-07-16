<%@ Page Title="Advance Journal Entry" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    Theme="duro" AutoEventWireup="true" CodeBehind="AdvanceJournalEntry.aspx.cs" 
    Inherits="DESPLWEB.AdvanceJournalEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 480px;">
        <asp:Panel ID="Panel1" Height="460px" runat="server" Width="100%">
            <table width="100%">
                <tr>
                    <td style="width: 15%">
                        <asp:Label ID="Label7" runat="server" Text="Note No."></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_NoteNo" Width="150px" runat="server" Text="Create New..." ReadOnly="True"></asp:TextBox>
                    </td>
                    <td align="center">
                        <asp:Label ID="Label8" runat="server" Text="Date"></asp:Label>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txt_date" runat="server" Width="90px" onchange="javascript:DateValid();"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_date">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_date" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td>
                    </td>
                    <td style="text-align: left">
                        <asp:Label ID="Label9" runat="server" Width="40px" Text="Status"></asp:Label>
                    </td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="ddl_status" runat="server" Width="145px">
                            <asp:ListItem Text="Ok" Value="false" />
                            <asp:ListItem Text="Hold" Value="true" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblLedger" runat="server" Text="Ledger Name"></asp:Label>
                    </td>
                    <td width="20%">
                        <%--<asp:DropDownList ID="ddlLedgerName" runat="server" Width="350px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlLedgerName_SelectedIndexChanged">
                        </asp:DropDownList>--%>
                        <asp:TextBox ID="txt_Ledger" runat="server" Width="350px" AutoPostBack="true" OnTextChanged="txt_Ledger_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetLedgerName" MinimumPrefixLength="1" OnClientItemSelected="LedgerItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Ledger"
                            ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfLedgerId" runat="server" />
                        <asp:Label ID="lblLedgerId" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Client Name"></asp:Label>
                    </td>
                    <td width="20%">
                        <%--<asp:DropDownList ID="ddlClient" runat="server" Width="350px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                        </asp:DropDownList>--%>
                        <asp:TextBox ID="txt_Client" runat="server" Width="350px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td colspan="2" align="center">
                        <asp:Label ID="Label1" runat="server" Text="Amount to be allocated "></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txt_Amtallocated" ReadOnly="true" Width="205px" Text="0.00" Style="text-align: right"
                            runat="server" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PnlGrd" runat="server" Height="370px" ScrollBars="Auto" Width="100%"
                BorderStyle="Ridge" BorderColor="AliceBlue">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvAdvanceJournal" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin"
                            ShowFooter="true" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Bill No" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNo" Width="100px" runat="server" Text='<%#Eval("BILL_Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Bill Date" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillDate" Width="100px" runat="server" Text='<%#Eval("BILL_Date_dt","{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Bill Amount"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillAmount" Width="100px" runat="server" Text='<%#Eval("BILL_NetAmt_num","{0:f2}") %>'> </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Balance Amount"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBalAmount" Width="100px" runat="server" Text='<%#Eval("BalanceAmount","{0:f2}") %>'> </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="TDS Amount">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_TDSamount" BorderWidth="0px" Width="260px" MaxLength="12" runat="server" autocomplete="off"
                                            onchange="checkNum(this)" Style="text-align: right" AutoPostBack="true" OnTextChanged="txt_TDSamount_TextChanged">  </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Adjust Amount"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="txt_Adjustamount" OnCommand="txt_Adjustamount_Click" BorderWidth="0px"
                                            Width="270px" Style="text-align: right" BackColor="White" ForeColor="Black" MaxLength="12"
                                            runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblrecpdtls" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <table width="100%">
                <tr>
                    <td style="width: 15%">
                        <asp:Label ID="Label2" runat="server" Text="Note"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Note" MaxLength="300" runat="server" Width="352px" autocomplete="off"></asp:TextBox>
                    </td>
                    <td style="text-align: right">
                        <asp:LinkButton ID="LnkBtnSave" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="LnkBtnSave_Click" CssClass="SimpleColor">Save</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Bold="true" Visible="false" Style="text-decoration: underline;"
                        OnClick="lnkPrint_Click">Print </asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClick="lnk_Exit_Click" runat="server">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                PopupControlID="Panel2" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="Panel2" runat="server">
                <asp:UpdatePanel runat="server" ID="up2">
                    <ContentTemplate>
                        <table class="DetailPopup" width="800px">
                            <tr>
                                <td valign="middle" align="left">
                                    <asp:Label ID="Label3" runat="server" Text="Bill No :" Font-Bold="true"></asp:Label>&nbsp;&nbsp;
                                    <asp:Label ID="lblBILLNo" Width="180px" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td valign="middle">
                                    <asp:Label ID="lblpopuphead" runat="server" Text="Amount to be Adjusted : " Font-Bold="True"></asp:Label>&nbsp;&nbsp;
                                    <asp:Label ID="lblAmtAdjusted" Width="200px" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/T.jpeg" OnClick="imgClose_Click"
                                        ImageAlign="Right" Width="18px" Height="18px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lbl_ErrMsg" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Panel ID="pnl_" runat="server" ScrollBars="Auto" Height="500px" Width="800px">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                            <ContentTemplate>
                                                <asp:GridView ID="grdAdjstdtl" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    SkinID="gridviewSkin">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Advance Receipt No " HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAdvRecNo" Width="100px" runat="server" Text='<%#Eval("ReceiptNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText=" Date" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRecpDate" Width="100px" runat="server" Text='<%#Eval("ReceiptDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAmount" Width="150px" runat="server" Text='<%#Eval("Amount","{0:f2}") %>'></asp:Label>                                                                
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText=" Balance Amount" HeaderStyle-HorizontalAlign="center"
                                                            ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblbalAmount" Width="150px" runat="server" Text='<%#Eval("BalanceAmount","{0:f2}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Adjust Amount" HeaderStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_Adjstamount" MaxLength="12" BorderWidth="0px" Width="200px"
                                                                    runat="server" onchange="checkDecimal(this)" Style="text-align: right" autocomplete="off"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:Panel>
    </div>
    <script type="text/javascript">

        function checkpopupnum(x) {
            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    document.getElementById('<%=lbl_ErrMsg.ClientID %>').innerHTML = "Only Numeric Values Allowed";
                    x.value = '';
                    x.focus();
                    return false;
                }
                else {
                    document.getElementById('<%=lbl_ErrMsg.ClientID %>').innerHTML = "";
                }

            }
            return true;
        }

        function checkNum(x) {
            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {

                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Numeric Values Allowed";
                    x.value = '';
                    x.focus();
                    return false;
                }
                else {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                }

            }
            return true;
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

        function DateValid() {
            var lblmsg = document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>');
            lblmsg.style.visibility = "hidden";
            var myDate1 = document.getElementById("<%=txt_date.ClientID%>").value;
            var date = myDate1.substring(0, 2);
            var month = myDate1.substring(3, 5);
            var year = myDate1.substring(6, 10);

            var myDate = new Date(year, month - 1, date);

            var today = new Date();

            if (myDate > today) {

                lblmsg.innerHTML = "Date should not be entered after current date";
                lblmsg.style.visibility = "visible";
                return false;
                myDate1.focus();
            }
            else {
                return true;
            }

        }

        function Select(txt_AmtReceived) {
            var control = document.getElementById(txt_AmtReceived);
            control.select();
        }
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
        function LedgerItemSelected(sender, e) {
            $get("<%=hfLedgerId.ClientID %>").value = e.get_value();
        }
      
    </script>
</asp:Content>
