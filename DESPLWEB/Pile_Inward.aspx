<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Pile_Inward.aspx.cs" Inherits="DESPLWEB.Pile_Inward" Title="Pile Inward"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Controls/UC_InwardHeader.ascx" TagName="UC_InwardHeader" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
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
                    &nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnkSelectRptClientSite" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                        Style="text-decoration: underline;" OnClick="lnkSelectRptClientSite_Click">Select Client for Report</asp:LinkButton>
                    <asp:Label ID="lblRptClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    <asp:Label ID="lblRptSiteId" runat="server" Text="0" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="180px" Width="100%">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grdPileInward" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false"
                                    OnRowDataBound="grdPileInward_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDescription" runat="server" Text='' BorderWidth="0px" Width="98%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="No. Of Pile" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNoOfPile" runat="server" Text='' BorderWidth="0px" Width="99%"
                                                    Style="text-align: center" MaxLength="2" onchange="checkNum(this)" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Supplier Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%--<asp:TextBox ID="txtSupplierName" runat="server" Text='' BorderWidth="0px" Width="99%" />--%>
                                                <asp:DropDownList ID="ddlSupplier" runat="server" BorderWidth="0px" Width="99%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Identification" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtIdentification" runat="server" Text='' BorderWidth="0px" Width="80%"
                                                    ReadOnly="true" />
                                                <asp:LinkButton ID="lnkIdentificationDetails" runat="server" CommandName="Select"
                                                    Font-Underline="true" OnCommand="lnkIdentificationDetails_Click" Width="10%">Add</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
            PopupControlID="Pnl_Identification" BehaviorID="txtIdentification_TextChanged"
            PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
        </asp:ModalPopupExtender>
        <asp:Panel runat="server" ID="Pnl_Identification">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <table class="DetailPopup" width="350px">
                        <tr>
                            <td align="center" valign="bottom" colspan="2">
                                <asp:ImageButton ID="imgCloseIdentification" runat="server" ImageUrl="Images/T.jpeg"
                                    Width="18px" Height="18px" OnClick="imgCloseIdentification_OnClick" ImageAlign="Right" />
                                <asp:Label ID="lblpopupheadIdentification" runat="server" Text="Identification Details"
                                    Font-Bold="True" ForeColor="#990033"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                &nbsp;<asp:Label ID="lblMessageIdentification" runat="server" ForeColor="Red" Font-Bold="true"
                                    Text="" Visible="False"></asp:Label>
                            </td>
                            <td align="right">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="200px" Width="352px">
                                    <asp:GridView ID="grdIdentificationDtl" runat="server" AutoGenerateColumns="False"
                                        BackColor="#CCCCCC" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333"
                                        GridLines="None" Width="100%" CellPadding="4" CellSpacing="0">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField></asp:TemplateField>
                                            <asp:TemplateField></asp:TemplateField>
                                            <asp:BoundField DataField="lblSrNoIdentification" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Identification">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtIdentificationDtl" runat="server" Text='' MaxLength="50" Width="200px" />
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
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
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
    </script>
</asp:Content>
