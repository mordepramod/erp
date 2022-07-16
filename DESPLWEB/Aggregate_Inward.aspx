<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Aggregate_Inward.aspx.cs" Inherits="DESPLWEB.Aggregate_Inward"
    Title="Aggregate Inward" Theme="duro"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Controls/UC_InwardHeader.ascx" TagName="UC_InwardHeader" TagPrefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="480px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">

            <table style="width: 100%;">               
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="pnlControl" runat="server" ScrollBars="Auto" Width="90%">
                            <uc1:UC_InwardHeader ID="UC_InwardHeader1" OnTextChanged="TextChanged" OnClick="Click" runat="server" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td colspan="2">
                        <asp:Label ID="tblGridDetails" runat="server" Font-Bold="True" Text="Sample Details"></asp:Label>
                        <asp:CheckBox ID="ChkAll" OnClick="javascript:SelectAllCheckboxes(this);" runat="server" />
                        <asp:Label ID="Label1" runat="server" Text="Select All"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td >
                        <asp:Label ID="lblTestedAt" runat="server" Text="Tested At : "></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlTestedAt" runat="server" Width="145px" Height="20px">
                            <asp:ListItem Text="Lab" Value="0"/>
                            <asp:ListItem Text="Prayeja City" Value="1" />
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSelectRptClientSite" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkSelectRptClientSite_Click">Select Client for Report</asp:LinkButton>
                        <asp:Label ID="lblRptClientId" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblRptSiteId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="180px" Width="940px">
                            <asp:GridView ID="grdAggregateInward" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false" OnRowDataBound="grdAggregateInward_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="lblSrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="Aggregate Name" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlAggregateName" runat="server" BorderWidth="0px" Width="100px">
                                                <asp:ListItem Text="Select" />
                                                <asp:ListItem Text="Natural Sand" />
                                                <asp:ListItem Text="Crushed Sand" />
                                                <asp:ListItem Text="10 mm" />
                                                <asp:ListItem Text="20 mm" />
                                                <asp:ListItem Text="40 mm" />
                                                <asp:ListItem Text="Stone Dust" />
                                                <asp:ListItem Text="Grit" />
                                                <asp:ListItem Text="Mix Aggt" />
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQty" CssClass="textbox"   onchange="checkNum(this)"  MaxLength="2"  runat="server" BorderWidth="0px" Text="" Width="50px"  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sieve Analysis" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSieveAnalysis" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mat. finar than 75u" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkMatFinaThan75u" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Spec. Grav & Water Abs." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSpecGravWaterAbs" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bulk Density" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkBulkDensity" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Flaki. Ind." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkFlakiInd" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Elong. Ind." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkElongInd" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Imp. Val." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkImpVal" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Crush. Val." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkCrushVal" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Moisture Content" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkMoistureContent" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" BorderWidth="0px" Text='' Width="100px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supplier Name" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%--<asp:TextBox ID="txtSupplierName" runat="server" BorderWidth="0px" Text='' Width="100px" />--%>
                                            <asp:DropDownList ID="ddlSupplier" runat="server" BorderWidth="0px" Width="100px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td colspan="2" align="right">
                       
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
                            Style="text-decoration: underline;" OnClick="lnkSave_Click" Font-Bold="True" >Save</asp:LinkButton> <%--OnClientClick = "Confirm()"--%>
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
        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;

        window.onload = function () {
            UpperBound = parseInt('<%= this.grdAggregateInward.Rows.Count %>') - 1;
            LowerBound = 0;
            SelectedRowIndex = -1;
        }

        function SelectRow(CurrentRow, RowIndex) {
            if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)
                return;

            if (SelectedRow != null) {
                SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                SelectedRow.style.color = SelectedRow.originalForeColor;
            }

            if (CurrentRow != null) {
                CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
                CurrentRow.originalForeColor = CurrentRow.style.color;
                CurrentRow.style.backgroundColor = '#DCFC5C';
                CurrentRow.style.color = 'Black';
            }

            SelectedRow = CurrentRow;
            SelectedRowIndex = RowIndex;
            setTimeout("SelectedRow.focus();", 0);
        }

        function SelectSibling(e) {
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;

            if (KeyCode == 40)
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            else if (KeyCode == 38)
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);

            return false;
        }
    </script>

    <script type="text/javascript">

        function SelectAllCheckboxes(spanChk) {

            var IsChecked = spanChk.checked;
            var bAllChecked = true;
            var falseAllChecked = false;
            var grdAggregateInward = document.getElementById('<%= this.grdAggregateInward.ClientID %>');
            for (var rowId = 1; rowId < grdAggregateInward.rows.length; rowId++) {

                var chkone = grdAggregateInward.rows[rowId].cells[3].children[0];
                var chktwo = grdAggregateInward.rows[rowId].cells[4].children[0];
                var chkthree = grdAggregateInward.rows[rowId].cells[5].children[0];
                var chkfour = grdAggregateInward.rows[rowId].cells[6].children[0];
                var chkfive = grdAggregateInward.rows[rowId].cells[7].children[0];
                var chksix = grdAggregateInward.rows[rowId].cells[8].children[0];
                var chkseven = grdAggregateInward.rows[rowId].cells[9].children[0];
                var chkeight = grdAggregateInward.rows[rowId].cells[10].children[0];
                var chknine = grdAggregateInward.rows[rowId].cells[11].children[0];


                if (IsChecked == bAllChecked) {

                    chkone.checked = bAllChecked;
                    chktwo.checked = bAllChecked;
                    chkthree.checked = bAllChecked;
                    chkfour.checked = bAllChecked;
                    chkfive.checked = bAllChecked;
                    chksix.checked = bAllChecked;
                    chkseven.checked = bAllChecked;
                    chkeight.checked = bAllChecked;
                    chknine.checked = bAllChecked;
                }
                else {
                    chkone.checked = falseAllChecked;
                    chktwo.checked = falseAllChecked;
                    chkthree.checked = falseAllChecked;
                    chkfour.checked = falseAllChecked;
                    chkfive.checked = falseAllChecked;
                    chksix.checked = falseAllChecked;
                    chkseven.checked = falseAllChecked;
                    chkeight.checked = falseAllChecked;
                    chknine.checked = falseAllChecked;

                }
            }
        }
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
    <script type = "text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to save bill no. of other branch?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>


</asp:Content>
