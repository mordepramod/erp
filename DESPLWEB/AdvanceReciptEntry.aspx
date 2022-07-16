<%@ Page Title="Advance Receipt Entry" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    Theme="duro" AutoEventWireup="true" CodeBehind="AdvanceReciptEntry.aspx.cs" Inherits="DESPLWEB.AdvanceReciptEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Controls/UC_InwardHeader.ascx" TagName="UC_InwardHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 480px;">
        <asp:Panel ID="Panel1" Height="450px" runat="server" Width="100%" BorderColor="AliceBlue"
            BorderStyle="Ridge">
            <table width="100%">
                <tr>
                    <td style="width: 15%">
                        <asp:Label ID="Label7" runat="server" Text="Rect. No."></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_receivedNo" Width="150px" runat="server" Text="Create New..."
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td align="center">
                        <asp:Label ID="Label8" runat="server" Text="Date"></asp:Label>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txt_date" runat="server" Width="90px" onchange="javascript:DateValid();"
                            Height="21px"></asp:TextBox>
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
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
               <tr>
                   <td>
                      <asp:Label ID="lblLedger" runat="server" Text="Ledger Name"></asp:Label>
                   </td>
                    <td width="20%">
                        <asp:TextBox ID="txt_Client" runat="server" Width="350px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Visible="false"></asp:Label>
                    </td>
                 </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClient" runat="server" Text="Client Name"></asp:Label>
                    </td>
                    <td>
                         <asp:TextBox ID="txt_Client1" runat="server" Width="350px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged1"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname1" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected1"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client1"
                            ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId1" runat="server" />
                        <asp:Label ID="lblClientId1" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label11" runat="server" Text="Payment By"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButton ID="rdn_cheque" runat="server" AutoPostBack="true" GroupName="c"
                            OnCheckedChanged="rdn_cheque_CheckedChanged" Text="" />
                        <asp:Label ID="Label3" runat="server" Text="Cheque"></asp:Label>
                        &nbsp;
                        <asp:RadioButton ID="rdn_cash" Checked="true" runat="server" AutoPostBack="true"
                            GroupName="c" OnCheckedChanged="rdn_cash_CheckedChanged" Text="" />
                        <asp:Label ID="Label4" runat="server" Text="Cash"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_chequno" runat="server" Style="text-align: right" Text="Cheque No"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_cheque_no" MaxLength="6" Enabled="false" runat="server" Width="200px"
                            onchange="checkNum(this)" autocomplete="off"></asp:TextBox>
                    </td>
                    <td align="center">
                        <asp:Label ID="Label12" runat="server" Text="Date"></asp:Label>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txt_chquedate" Enabled="false" Width="90px" runat="server" onchange="javascript:DateValidcheque();" autocomplete="off"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_chquedate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_chquedate"
                            MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>

                        <%--<asp:TextBox ID="TextBox1" runat="server" Width="90px" onchange="javascript:DateValid();"
                            Height="21px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_date">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txt_date" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>--%>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label13" runat="server" Text="Branch"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Branch" Enabled="false" Width="140px" runat="server" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_bankname" runat="server" Text="Bank Name"></asp:Label>
                    </td>
                    <td>
                        <%--<asp:ComboBox ID="CmbBankname" Enabled="false" runat="server" Width="350px">
                        </asp:ComboBox>--%>
                        <asp:DropDownList ID="ddl_Bank_Name" Enabled="false" runat="server" Width="350px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_amtrecved" runat="server" Text="Amt Received"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_AmtReceived" Width="200px" runat="server" MaxLength="12" AutoPostBack="true"
                            OnTextChanged="txt_AmtReceived_TextChanged" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_TDS" runat="server" Text="TDS"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_TDS" runat="server" Width="200px" MaxLength="12" AutoPostBack="true"
                            OnTextChanged="txt_TDS_TextChanged" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_TotalAmt" runat="server" Text="Total"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_totalAmt" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <asp:Label ID="Label1" runat="server" Text="Ledger"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLedger" Width="206px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkColleted" Checked="true" runat="server" AutoPostBack="true"
                            OnCheckedChanged="chkColleted_CheckedChanged" />
                        <asp:Label ID="lblColectfrm" runat="server" Text="Collected From"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlUserCollected" Width="206px" runat="server">
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txt_userip" runat="server" Width="130px" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Note"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txt_Note" MaxLength="250" runat="server" Width="352px" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <table width="100%">
            <tr>
                <td style="text-align: right">
                    <asp:LinkButton ID="LnkBtnSave" runat="server" Font-Bold="true" Font-Underline="true"
                        OnClick="LnkBtnSave_Click" CssClass="SimpleColor">Save</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnkNext" runat="server" Visible="false"
                        Font-Bold="true" Font-Underline="true" OnClick="lnkNext_Click" CssClass="SimpleColor">Next</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnkPrint" runat="server" Font-Bold="true" Visible="false" Style="text-decoration: underline;"
                          OnClick="lnkPrint_Click">Print </asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                        runat="server" OnClick="LnkExit_Click">Exit</asp:LinkButton>
                </td>
            </tr>
        </table>
        
    </div>
    <script type="text/javascript">

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



        function DateValidcheque() {
            var myDate1 = document.getElementById("<%=txt_chquedate.ClientID%>").value;
            var date = myDate1.substring(0, 2);
            var month = myDate1.substring(3, 5);
            var year = myDate1.substring(6, 10);

            var myDate = new Date(year, month - 1, date);

            var today = new Date();

            if (myDate > today) {

                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Date should not be entered after current date";
                return false;
                myDate1.focus();

            }
            else {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                return true;
            }
        }
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
            
        }
         function ClientItemSelected1(sender, e) {
            $get("<%=hfClientId1.ClientID %>").value = e.get_value();
            
        }
    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
