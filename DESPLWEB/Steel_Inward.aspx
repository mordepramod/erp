<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Steel_Inward.aspx.cs" Inherits="DESPLWEB.Steel_Inward" Title="Steel Inward"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Controls/UC_InwardHeader.ascx" TagName="UC_InwardHeader" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="480px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <table style="width: 100%;">
                <%--<tr style="background-color: #ECF5FF;">
                            <td>
                                <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Text="Client Details"></asp:Label>
                            </td>
                            <td colspan="3" align="right">
                                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png"
                                     OnClick="imgClose_Click"  ImageAlign="Right" />
                            </td>
                        </tr>--%>
                <tr>
                    <td colspan="4">
                        <asp:Panel ID="pnlControl" runat="server" ScrollBars="Auto" Width="90%">
                            <uc1:UC_InwardHeader ID="UC_InwardHeader1" OnTextChanged="TextChanged" OnClick="Click"
                                runat="server" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td colspan="4">
                        <asp:Label ID="tblGridDetails" runat="server" Font-Bold="True" Text="Sample Details"></asp:Label>
                        <asp:CheckBox ID="ChkAll" OnClick="javascript:SelectAllCheckboxes(this);" runat="server" />
                        <asp:Label ID="Label1" runat="server" Text="Select All"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSelectRptClientSite" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkSelectRptClientSite_Click">Select Client for Report</asp:LinkButton>
                        <asp:Label ID="lblRptClientId" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblRptSiteId" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblNoOfCoupons1" runat="server" Font-Bold="True" Text="Unused Coupons : "></asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblNoOfCoupons" runat="server" Font-Bold="True" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="170px" Width="940px">
                            <asp:GridView ID="grdSteelInward" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false"
                                OnRowDataBound="grdSteelInward_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="Dia" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiameter" runat="server" BorderWidth="0px" Width="30px" Text=''
                                                Style="text-align: right" MaxLength="2" onchange="checkNum(this)" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuantity" runat="server" BorderWidth="0px" Width="30px" Text=''
                                                Style="text-align: right" MaxLength="2" onchange="checkNum(this)" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID Mark" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtIdMark" BorderWidth="0px" Width="100px" runat="server" Text='' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Steel Type" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlSteelType" runat="server" BorderWidth="0px" Width="63px">
                                                <asp:ListItem Text="Select" />
                                                <asp:ListItem Text="Ribbed Steel" />
                                                <asp:ListItem Text="Twisted Steel" />
                                                <asp:ListItem Text="Mild Steel" />
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Grade" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlGrade" runat="server" BorderWidth="0px" Width="63px">
                                                <asp:ListItem Text="Select" />
                                                <asp:ListItem Text="Fe 250" />
                                                <asp:ListItem Text="Fe 415" />
                                                <asp:ListItem Text="Fe 415D" />
                                                <asp:ListItem Text="Fe 500" />
                                                <asp:ListItem Text="Fe 500D" />
                                                <asp:ListItem Text="Fe 550" />
                                                <asp:ListItem Text="Fe 550D" />
                                                <asp:ListItem Text="Fe 600" />
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tensile Strength" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkTensileStrength" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="% Elongation" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkElongation" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rebend" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRebend" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Wt.Per Meter" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkWeightPerMeter" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bend" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkBend" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" Text='' BorderWidth="0px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supplier" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSupplier" AutoPostBack="true" OnCheckedChanged="chkSupplier_CheckedChanged"
                                                runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%--<asp:TextBox ID="txtSupplierName" runat="server" Text='' BorderWidth="0px" />--%>
                                            <asp:DropDownList ID="ddlSupplier" runat="server" BorderWidth="0px" Width="180px"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td colspan="3" align="right">
                    </td>
                    <td align="right">
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
        </asp:Panel>
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

        function SelectAllCheckboxes(spanChk) {

            var IsChecked = spanChk.checked;
            var bAllChecked = true;
            var falseAllChecked = false;
            var grdSteelInward = document.getElementById('<%= this.grdSteelInward.ClientID %>');
            for (var rowId = 1; rowId < grdSteelInward.rows.length; rowId++) {

                var chkone = grdSteelInward.rows[rowId].cells[6].children[0];
                var chktwo = grdSteelInward.rows[rowId].cells[7].children[0];
                var chkthree = grdSteelInward.rows[rowId].cells[8].children[0];
                var chkfour = grdSteelInward.rows[rowId].cells[9].children[0];
                var chkfive = grdSteelInward.rows[rowId].cells[10].children[0];
                if (IsChecked == bAllChecked) {
                    chkone.checked = bAllChecked;
                    chktwo.checked = bAllChecked;
                    chkthree.checked = bAllChecked;
                    chkfour.checked = bAllChecked;
                    chkfive.checked = bAllChecked;
                }
                else {
                    chkone.checked = falseAllChecked;
                    chktwo.checked = falseAllChecked;
                    chkthree.checked = falseAllChecked;
                    chkfour.checked = falseAllChecked;
                    chkfive.checked = falseAllChecked;
                }
            }
        }
    </script>
</asp:Content>
