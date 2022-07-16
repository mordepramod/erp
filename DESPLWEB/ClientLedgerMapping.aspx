<%@ Page Title="Client - Ledger Mapping" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="ClientLedgerMapping.aspx.cs" Inherits="DESPLWEB.ClientLedgerMapping"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                    <td width="20%" align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="3">
                        <asp:RadioButton ID="rbClient" runat="server" Text="Client Specific" Checked="true" GroupName="R1"
                        AutoPostBack="true"    OnCheckedChanged="rbClient_CheckedChanged" />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:RadioButton ID="rbAll" runat="server"  AutoPostBack="true"   Text="All" OnCheckedChanged="rbAll_CheckedChanged" GroupName="R1" />
                    </td>
                </tr>
                <tr id="tr1" runat="server">
                    <td width="10%">
                        <asp:Label ID="lblClient" runat="server" Text="Client Name"></asp:Label>
                    </td>
                    <td width="40%">
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="5" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td width="20%">
                        &nbsp;
                    </td>
                    <td width="20%">
                        &nbsp;
                    </td>
                </tr>
                <tr id="tr2" runat="server">
                    <td>
                        <asp:Label ID="lblLedger" runat="server" Text="Ledger Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Ledger" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Ledger_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetLedgername" MinimumPrefixLength="1" OnClientItemSelected="LedgerItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Ledger"
                            ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfLedgerId" runat="server" />
                        <asp:Label ID="lblLedgerId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
                <tr id="tr3" runat="server">
                    <td>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkSave_Click" Font-Bold="True">Save</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkDelete" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkDelete_Click" Font-Bold="True">Delete</asp:LinkButton>
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
                <tr  id="tr4" runat="server" visible="false">
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="List for "></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_List" Width="300px" runat="server">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="1">Pending</asp:ListItem>
                            <asp:ListItem Value="2">Completed</asp:ListItem>
                        </asp:DropDownList>
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="width: 14px" />
                    </td>
                    <td>
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total No of Records : 0 "></asp:Label>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkUpdateAll" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkUpdateAll_Click" Font-Bold="True">Update All</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlMappingList" runat="server" Height="320px" Width="940px" ScrollBars="Auto"
                BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">
                <asp:GridView ID="grdMapping" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False"
                    Width="900px">
                    <Columns>
                        <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="400px" />
                        <asp:BoundField DataField="LedgerName_Description" HeaderText="Ledger Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="350px" />
                        <asp:TemplateField HeaderText="ClientLedgerId" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="CLLedger_CL_Id" runat="server" Text='<%#Eval("CLLedger_CL_Id") %>'></asp:Label>
                                <asp:Label ID="CLLedger_Ledger_Id" runat="server" Text='<%#Eval("CLLedger_Ledger_Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="grdMappingList" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="false"
                    Visible="false" Width="900px">
                    <%--OnRowDataBound="grdMappingList_RowDataBound"--%>
                    <Columns>
                        <asp:BoundField DataField="LedgerName_Description" HeaderText="Ledger Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="350px" />
                        <asp:TemplateField HeaderText="ClientLedgerId" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="CLLedger_Ledger_Id" runat="server" Text='<%#Eval("CLLedger_Ledger_Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblCL_Id" runat="server" Visible="false" Text='<%#Eval("CLLedger_CL_Id") != null ? Eval("CLLedger_CL_Id") : "0"%>'></asp:Label>
                                <asp:Label ID="lblCL_IdNew" runat="server" Visible="false" Text='<%#Eval("CLLedger_CL_Id") != null ? Eval("CLLedger_CL_Id") : "0" %>'></asp:Label>
                                <asp:TextBox ID="txtClient" runat="server" Text='<%#Bind("CL_Name_var")%>' BorderWidth="0px"
                                    AutoPostBack="true" OnTextChanged="txt_ClientTextChanged" Width="380px" />
                                <asp:AutoCompleteExtender ServiceMethod="Get_Client_name" MinimumPrefixLength="1"
                                    OnClientItemSelected="ClientItemSelectedGrd" CompletionInterval="10" EnableCaching="false"
                                    CompletionSetCount="1" TargetControlID="txtClient" ID="AutoCompleteExtender1"
                                    runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                                </asp:AutoCompleteExtender>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" runat="server" Style="text-decoration: underline;"
                                    CommandArgument='<%#Eval("CLLedger_Ledger_Id") %>' OnCommand="lnkUpdate">Update</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="hfClId" runat="server" />
            </asp:Panel>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
        function ClientItemSelectedGrd(sender, e) {
            $get("<%=hfClId.ClientID %>").value = e.get_value();
        }
        function LedgerItemSelected(sender, e) {
            $get("<%=hfLedgerId.ClientID %>").value = e.get_value();
        }
    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
