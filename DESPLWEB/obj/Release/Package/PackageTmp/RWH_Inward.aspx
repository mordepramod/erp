<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="RWH_Inward.aspx.cs" Inherits="DESPLWEB.RWH_Inward" Title="Rain Water Harvesting Inward"
    Theme="duro" %>

<%@ Register Src="Controls/UC_InwardHeader.ascx" TagName="UC_InwardHeader" TagPrefix="uc1" %>
<%@ Register Src="Controls/UC_InwardFooter.ascx" TagName="UC_InwardFooter" TagPrefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <%--<table style="width: 100%;">
            <tr style="background-color: #ECF5FF;">
                <td>
                    <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Text="Client Details"></asp:Label>
                </td>
                <td colspan="2" align="right">
                    <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                        ImageAlign="Right" />
                </td>
            </tr>
        </table>--%>
        <table style="width: 100%;">
            <tr>
                <td colspan="4">
                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="775px">
                        <uc1:UC_InwardHeader ID="UC_InwardHeader1" runat="server" OnTextChanged="TextChanged"
                            OnClick="Click" />
                    </asp:Panel>
                </td>
            </tr>
            <tr style="background-color: #ECF5FF;" height="30px">
                <td width="40%">
                    <%--width="45%" --%>
                    <asp:Label ID="tblGridDetails" runat="server" Font-Bold="True" Text="Sample Details"></asp:Label>
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnkSelectRptClientSite" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                        Style="text-decoration: underline;" OnClick="lnkSelectRptClientSite_Click">Select Client for Report</asp:LinkButton>
                    <asp:Label ID="lblRptClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    <asp:Label ID="lblRptSiteId" runat="server" Text="0" Visible="false"></asp:Label>
                </td>
                <td align="left" width="100%">
                    <asp:Label ID="Label1" runat="server" Text="Lump Sump"></asp:Label>
                    <asp:CheckBox ID="chk_Lumshup" OnCheckedChanged="chk_Lumshup_OnCheckedChanged" runat="server"
                        AutoPostBack="true" />
                    &nbsp;
                    <asp:Label ID="Label2" runat="server" Text="From Row" Visible="false"></asp:Label>
                    <asp:TextBox ID="txtFrmRow" runat="server" Width="42px" MaxLength="2" Visible="false"
                        onchange="checkNum(this)"></asp:TextBox>
                    <asp:Label ID="Label3" runat="server" Text="To Row" Visible="false"></asp:Label>
                    <asp:TextBox ID="txtToRow" runat="server" Width="42px" MaxLength="2" Visible="false"
                        onchange="checkNum(this)"></asp:TextBox>
                    <asp:Button ID="Btn_Apply" Text="Apply" Visible="false" OnClick="Btn_Apply_OnClick"
                        runat="server" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="180px" Width="960px">
            <asp:GridView ID="grdRainWaterHarvestingInward" runat="server" AutoGenerateColumns="False"
                BackColor="#CCCCCC" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333"
                GridLines="None" CellPadding="0" CellSpacing="0">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:TemplateField Visible="false"></asp:TemplateField>
                    <asp:TemplateField Visible="false"></asp:TemplateField>
                    <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDescription" runat="server" Text='' Width="518px" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit">
                        <ItemTemplate>
                            <asp:TextBox ID="txtUnit" runat="server" Text='' MaxLength="2" Style="text-align: center"
                                Width="130px" onchange="checkNum(this)" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qty">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQty" runat="server" Text='' MaxLength="2" Width="100px" Style="text-align: center"
                                onchange="checkNum(this)" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRate" runat="server" Text='' Width="130px" MaxLength="12" Style="text-align: center"
                                onchange="checkNum(this)" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
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
        <table width="100%">
            <tr style="background-color: #ECF5FF;">
                <td align="right">
                </td>
                <td colspan="4" align="right">
                    <asp:LinkButton ID="lnkTemp" runat="server" OnClick="lnkTemp_Click"></asp:LinkButton>
                    &nbsp; &nbsp;
                    <asp:LinkButton ID="lnkPrint" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                        Visible="false" Style="text-decoration: underline;" OnClick="lnkPrint_Click">Print Inward</asp:LinkButton>
                    &nbsp;&nbsp;
                    <asp:LinkButton ID="lnkLabSheet" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                        Visible="false" Style="text-decoration: underline;" OnClick="lnkLabSheet_Click">Print LabSheet</asp:LinkButton>
                    &nbsp;&nbsp;
                    <asp:LinkButton ID="lnkBillPrint" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                        Visible="false" Style="text-decoration: underline;" OnClick="lnkBillPrint_Click">Print Tax Invoice</asp:LinkButton>
                    &nbsp; &nbsp;
                    <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                        Style="text-decoration: underline;" OnClick="lnkSave_Click" Font-Bold="True">Save</asp:LinkButton>
                    &nbsp; &nbsp;
                    <asp:LinkButton ID="lnkExit" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                        OnClick="lnk_Exit_Click" Font-Bold="True">Exit</asp:LinkButton>
                    &nbsp; &nbsp;&nbsp;
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
