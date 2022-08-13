<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Water_Inward.aspx.cs" Inherits="DESPLWEB.Water_Inward" Title="Water Inward"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Controls/UC_InwardHeader.ascx" TagName="UC_InwardHeader" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="480px"
            Style="background-color: #ECF5FF;">
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
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdWaterInward" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false"
                                        OnRowDataBound="grdWaterInward_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDescription" runat="server" Text='' BorderWidth="0px" Width="200px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Supplier Name" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%--<asp:TextBox ID="txtSupplierName" runat="server" Text='' BorderWidth="0px" Width="200px" />--%>
                                                    <asp:DropDownList ID="ddlSupplier" runat="server" BorderWidth="0px" Width="200px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Test Details" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTestDetails" runat="server" Text='' BorderWidth="0px" Width="400px"
                                                        ReadOnly="true" />
                                                    <asp:LinkButton ID="lnkRateList" runat="server" CommandName="Select" OnCommand="lnkRateList_Click"
                                                        Font-Underline="true"> Select Test</asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="500px" />
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
                <tr>
                    <td colspan="4">
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                            PopupControlID="Pnl_TestDetails" PopupDragHandleControlID="PopupHeader" Drag="true"
                            BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="Pnl_TestDetails">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table class="DetailPopup" width="350px">
                            <tr>
                                <td align="center" valign="bottom" colspan="2">
                                    <asp:ImageButton ID="imgCloseTestDetails" runat="server" ImageUrl="Images/T.jpeg"
                                        Width="18px" Height="18px" OnClick="imgCloseTestDetails_Click" ImageAlign="Right" />
                                    <asp:Label ID="lblpopuphead" runat="server" Text="Test Details" Font-Bold="True"
                                        ForeColor="#990033"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="250px" Width="500px">
                                        <asp:GridView ID="grdTestDetails" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                            BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="None"
                                            Width="100%" CellPadding="4" CellSpacing="0">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Test" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTest" Width="352px" runat="server" Text='<%#Eval("TEST_Name_var") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Test Id" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTestId" runat="server" Text='<%#Eval("TEST_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQty" runat="server" Width="30px" MaxLength="2"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="cbxSelectAll" OnClick="javascript:SelectAllCheckboxes(this);" runat="server" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbxSelect" Checked="false" OnClick="javascript:OnOneCheckboxSelected(this);"
                                                            runat="server" />
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
                                <td>
                                    <asp:Label ID="lblRateList" runat="server" ForeColor="#990033" Text="lblMsg" Visible="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        function OnOneCheckboxSelected(chkB) {
            var IsChecked = chkB.checked;
            var Parent = document.getElementById('<%= this.grdTestDetails.ClientID %>');
            var cbxAll;
            var items = Parent.getElementsByTagName('input');
            var bAllChecked = true;
            for (i = 0; i < items.length; i++) {
                if (items[i].id.indexOf('cbxSelectAll') != -1) {
                    cbxAll = items[i];
                    continue;
                }
                if (items[i].type == "checkbox" && items[i].checked == false) {
                    bAllChecked = false;
                    break;
                }
            }
            cbxAll.checked = bAllChecked;
        }

        function SelectAllCheckboxes(spanChk) {
            var IsChecked = spanChk.checked;
            var cbxAll = spanChk;
            var Parent = document.getElementById('<%= this.grdTestDetails.ClientID %>');
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != cbxAll.id && items[i].type == "checkbox") {
                    items[i].checked = IsChecked;
                }
            }
        }
    </script>
</asp:Content>
