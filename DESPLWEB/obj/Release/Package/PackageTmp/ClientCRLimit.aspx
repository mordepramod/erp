<%@ Page Title="Client Credit Limit Setting" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="ClientCRLimit.aspx.cs" Inherits="DESPLWEB.ClientCRLimit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td colspan="5">
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td width="28%">
                        <asp:RadioButton ID="optBypassCreditLimit" Text="Bypass Credit Limit" runat="server"
                            GroupName="g1" AutoPostBack="true" OnCheckedChanged="optBypassCreditLimit_CheckedChanged" />&nbsp;&nbsp;
                        <asp:RadioButton ID="optMonthlyBilling" Text="Monthly Billing" runat="server" AutoPostBack="true"
                            GroupName="g1" OnCheckedChanged="optMonthlyBilling_CheckedChanged" />&nbsp;&nbsp;
                        <asp:RadioButton ID="OptMetro" Text="Metro" runat="server" AutoPostBack="true"
                            GroupName="g1" OnCheckedChanged="optMetro_CheckedChanged" />             
                        <asp:Label ID="lblRight" runat="server" Text="" Visible="false"></asp:Label>

                    </td>
                    <td width="7%">
                        <asp:Label ID="lblClient" runat="server" Text="Client"></asp:Label>
                    </td>
                    <td style="width: 32%">
                        <asp:TextBox ID="txt_Client" runat="server" Width="295px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td style="width: 10%">
                        <asp:Label ID="lblCrLimit" runat="server" Text="Credit Limit"></asp:Label>
                    </td>
                    <td style="width: 12%">
                        <asp:TextBox ID="txtCrLimit" MaxLength="15" runat="server" Width="100px" onchange="javascript:checkDecimal(this)"></asp:TextBox>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkSave" Font-Size="Small" runat="server" OnClick="lnkSave_Click"
                            Font-Underline="true">Save</asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkExit" runat="server" Font-Size="Small" Font-Underline="true"
                            Visible="false" OnClick="lnkExit_Click">Exit</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="height: 23px" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="370px"
                            Width="920px" BorderColor="AliceBlue">
                            <asp:GridView ID="grdCredit" runat="server" AutoGenerateColumns="False" CellPadding="1"
                                ForeColor="#333333" BorderColor="#DEDFDE" BackColor="#F7F6F3" CssClass="Grid"
                                BorderWidth="1px" Width="100%" OnRowCreated="grdCredit_RowCreated">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="cbxSelectAll" runat="server" onclick="javascript:HeaderClick(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbxSelect" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteId" runat="server" Text='<%#Eval("Site_Id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" runat="server" Text='<%#Eval("Site_Name_var") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="false" />
                                    </asp:TemplateField>
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
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
        function checkDecimal(inputtxt) {
            var numbers = /^\d+(\.\d{1,2})?$/;
            if (inputtxt.value.match(numbers)) {
                return true;

            }
            else {
                alert('Please enter valid integer or decimal number with 2 decimal places');
                inputtxt.focus();
                inputtxt.value = "";
                return false;
            }
        }

        var TotalChkBx;
        var Counter;
        window.onload = function () {
            TotalChkBx = parseInt('<%= this.grdCredit.Rows.Count %>');
            Counter = 0;
        }
        function HeaderClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.grdCredit.ClientID %>');
            var TargetChildControl = "cbxSelect";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBx : 0;

        }
        function ChildClick(CheckBox, HCheckBox) {
            var HeaderCheckBox = document.getElementById(HCheckBox);
            if (CheckBox.checked && Counter < TotalChkBx)
                Counter++;
            else if (Counter > 0)
                Counter--;
            if (Counter < TotalChkBx)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBx)
                HeaderCheckBox.checked = true;

            if (CheckBox.checked) {
                CheckBox.parentElement.parentElement.style.backgroundColor = '#FFDFDF';
            }
            else {
                CheckBox.parentElement.parentElement.style.backgroundColor = '#FFFFFF';
            }
        }
        function CheckAll(Checkbox) {
            var grdCredit = document.getElementById("<%=grdCredit.ClientID %>");
            for (i = 1; i < grdCredit.rows.length; i++) {
                grdCredit.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;

                if (Checkbox.checked) {
                    Checkbox.parentElement.parentElement.style.backgroundColor = '#FFDFDF';
                }
                else {
                    Checkbox.parentElement.parentElement.style.backgroundColor = '#FFFFFF';
                }
            }

        }
    </script>
</asp:Content>
