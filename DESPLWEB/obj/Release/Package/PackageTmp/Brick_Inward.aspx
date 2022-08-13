<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Brick_Inward.aspx.cs" Inherits="DESPLWEB.Brick_Inward" Title="Brick Inward"
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
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                             ImageAlign="Right" />
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="4">
                        <asp:Panel ID="pnlControl" runat="server" ScrollBars="Auto" Width="90%">
                            <uc1:UC_InwardHeader ID="UC_InwardHeader1" OnTextChanged="TextChanged" OnClick="Click" runat="server" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td colspan="4">
                        <asp:Label ID="tblGridDetails" runat="server" Font-Bold="True" Text="Sample Details"></asp:Label>
                        <asp:CheckBox ID="ChkAll" OnClick="javascript:SelectAllCheckboxes(this);" runat="server" />
                        <asp:Label ID="Label1" runat="server" Text="Select All"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSelectRptClientSite" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkSelectRptClientSite_Click">Select Client for Report</asp:LinkButton>
                        <asp:Label ID="lblRptClientId" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblRptSiteId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="180px" Width="940px">
                            <asp:GridView ID="grdBrickInward" runat="server" SkinID="gridviewSkin" OnRowDataBound="grdBrickInward_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="Type of Brick" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlTypeOfBrick" runat="server" BorderWidth="0px" Width="80px">
                                                <asp:ListItem Text="Select" />
                                                <asp:ListItem Text="Burnt Clay" />
                                                <asp:ListItem Text="Fly Ash" />
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID Mark/Details" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtIdMark" runat="server" Text='' BorderWidth="0px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CS" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkCS" runat="server" OnCheckedChanged="chkCS_CheckedChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCSQty" runat="server" Text='' BorderWidth="0px" Width="40px"
                                                onchange="checkNum(this)" MaxLength="2" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="WA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkWA" runat="server" OnCheckedChanged="chkWA_CheckedChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWAQty" runat="server" Text='' BorderWidth="0px" Width="40px"
                                                onchange="checkNum(this)" MaxLength="2" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DA" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDA" runat="server" OnCheckedChanged="chkDA_CheckedChanged" AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDAQty" runat="server" Text='' BorderWidth="0px" Width="40px"
                                                onchange="checkNum(this)" MaxLength="2" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Eff." HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkEff" runat="server" OnCheckedChanged="chkEff_CheckedChanged"
                                                AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtEffQty" runat="server" BorderWidth="0px" Text='' Width="40px"
                                                onchange="checkNum(this)" MaxLength="2" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Density" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDensity" runat="server" OnCheckedChanged="chkDensity_CheckedChanged"
                                                AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDensityQty" runat="server" Text='' BorderWidth="0px" Width="40px"
                                                onchange="checkNum(this)" MaxLength="2" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" Text='' BorderWidth="0px" Width="150px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supplier Name" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%--<asp:TextBox ID="txtSupplierName" runat="server" Text='' BorderWidth="0px" Width="150px" />--%>
                                            <asp:DropDownList ID="ddlSupplier" runat="server" BorderWidth="0px" Width="150px" />
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
                        <asp:LinkButton ID="lnkExit" runat="server" CssClass="LnkOver" Style="text-decoration: underline;" OnClick="lnk_Exit_Click"
                             Font-Bold="True">Exit</asp:LinkButton>
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
            var grdBrickInward = document.getElementById('<%= this.grdBrickInward.ClientID %>');
            for (var rowId = 1; rowId < grdBrickInward.rows.length; rowId++) {

                var chkone = grdBrickInward.rows[rowId].cells[3].children[0];
                var chktwo = grdBrickInward.rows[rowId].cells[5].children[0];
                var chkthree = grdBrickInward.rows[rowId].cells[7].children[0];
                var chkfour = grdBrickInward.rows[rowId].cells[9].children[0];
                //  var chkfive = grdBrickInward.rows[rowId].cells[11].children[0];
                if (IsChecked == bAllChecked) {
                    chkone.checked = bAllChecked;
                    chktwo.checked = bAllChecked;
                    chkthree.checked = bAllChecked;
                    chkfour.checked = bAllChecked;
                    // chkfive.checked = bAllChecked;
                }
                else {
                    chkone.checked = falseAllChecked;
                    chktwo.checked = falseAllChecked;
                    chkthree.checked = falseAllChecked;
                    chkfour.checked = falseAllChecked;
                    // chkfive.checked = falseAllChecked;
                }
            }
        }
    </script>

</asp:Content>
