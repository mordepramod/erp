<%@ Page Title="Client - Recovery User Allocation" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="ClientRecoveryUser.aspx.cs" Inherits="DESPLWEB.ClientRecoveryUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <table style="width: 100%">
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr style="background-color: #ECF5FF;">
                                <td style="width:100px">
                                    <asp:Label ID="lblClient" runat="server" Font-Bold="True" Text="Clients"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearch" runat="server" MaxLength="50" Width="343px" Height="16px"></asp:TextBox>
                                    <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtSearch"
                                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                                        </asp:AutoCompleteExtender>
                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/Search-32.png" OnClick="ImgBtnSearch_Click" Width="16px" />                                    
                                </td>
                                <td style="width:100px">
                                    <asp:Label ID="lblRecoveryUser" runat="server" Font-Bold="True" Text="Recovery User"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRecoveryUser" runat="server" Width="200px"></asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                                     OnClick="lnkSave_Click" Font-Bold="True">Save</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                   <asp:HiddenField ID="hfClientId" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="height: 23px" valign="top">
                                    <asp:Panel ID="pnlClientList" runat="server" Width="100%" BorderWidth="0px" Height="400px"
                                        ScrollBars="Auto">
                                        <asp:GridView ID="grdClient" runat="server" AutoGenerateColumns="False" CellPadding="2"
                                            DataKeyNames="CL_Id" ForeColor="#333333" GridLines="Vertical" BorderColor="#DEDFDE"
                                            BorderWidth="1px" Width="100%" >
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns> 
                                                <asp:TemplateField HeaderStyle-Width="20px">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="cbxSelectAll" runat="server" onclick="javascript:HeaderClick(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbxSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                               
                                                <asp:TemplateField HeaderText="Client Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClientId" runat="server" Text='<%#Eval("CL_Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Client Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("CL_Name_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Recovery User Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRecoveryUserName" runat="server" Text='<%#Eval("RecoveryUserName") %>' />
                                                    </ItemTemplate>
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
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>

    <script type="text/javascript">        
        var TotalChkBx;
        var Counter;
        window.onload = function () {
            TotalChkBx = parseInt('<%= this.grdClient.Rows.Count %>');
            Counter = 0;
        }
        function HeaderClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.grdClient.ClientID %>');
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
            var grdClient = document.getElementById("<%=grdClient.ClientID %>");
            for (i = 1; i < grdClient.rows.length; i++) {
                grdSiteAssignment.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;

                if (Checkbox.checked) {
                    Checkbox.parentElement.parentElement.style.backgroundColor = '#FFDFDF';
                }
                else {
                    Checkbox.parentElement.parentElement.style.backgroundColor = '#FFFFFF';
                }
            }

        }
   
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }        
    </script>
</asp:Content>

