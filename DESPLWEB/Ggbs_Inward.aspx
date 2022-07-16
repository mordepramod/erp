<%@ Page Title="GGBS Inward" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="Ggbs_Inward.aspx.cs" Inherits="DESPLWEB.Ggbs_Inward" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Controls/UC_InwardHeader.ascx" TagName="UC_InwardHeader" TagPrefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <table style="width: 100%;">
            <tr>
                <td colspan="4">
                    <asp:Panel ID="pnlControl" runat="server" ScrollBars="Auto" Width="90%">
                        <uc1:UC_InwardHeader ID="UC_InwardHeader1" runat="server" OnTextChanged="TextChanged"
                            OnClick="Click" />
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
                <td colspan="4" valign="top">
                    <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="180px" Width="940px">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grdGgbsInward" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false"
                                    OnRowDataBound="grdGgbsInward_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="GGBS Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGgbsName" runat="server" Text='' Width="135px" BorderWidth="0px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cement Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCementName" runat="server" Text='' Width="135px" BorderWidth="0px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQty" runat="server" MaxLength="2" Width="50px" BorderWidth="0px"
                                                    Style="text-align: right" onchange="checkNum(this)"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDescription" runat="server" Width="130px" BorderWidth="0px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Supplier Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlSupplier" runat="server" BorderWidth="0px" Width="150px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Test Details" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTestDetails" runat="server" ReadOnly="true" Width="200px" BorderWidth="0px" />
                                                <asp:LinkButton ID="lnkRateList" runat="server" CommandName="Select" OnCommand="lnkRateList_Click"
                                                    Font-Underline="true">  Select Test</asp:LinkButton>
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
            <tr>
                <td>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                        PopupControlID="Panel2" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                    </asp:ModalPopupExtender>
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel2" runat="server">
            <div>
                <asp:UpdatePanel runat="server" ID="up2">
                    <ContentTemplate>
                        <table class="DetailPopup" width="350px">
                            <tr>
                                <td align="center" valign="bottom" colspan="2">
                                    <asp:ImageButton ID="imgCloseTestDetails" runat="server" ImageUrl="Images/T.jpeg"
                                        OnClick="imgCloseTestDetails_Click" ImageAlign="Right" Width="18px" Height="18px" />
                                    <asp:Label ID="lblpopuphead" runat="server" Text="Test Details" Font-Bold="True"
                                        ForeColor="#990033"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_ErrMsg" runat="server" Text="" Font-Bold="True" Visible="false"
                                        ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="500px" Width="500px">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                            <ContentTemplate>
                                                <asp:GridView ID="grdTestDetails" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                                    BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="None"
                                                    Width="100%" CellPadding="4" CellSpacing="0">
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Test" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTest" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200px">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDAYS" runat="server" Width="50px" MaxLength="2"></asp:TextBox>
                                                                <asp:Label ID="lblDays" runat="server" Text="Days"></asp:Label>
                                                                <asp:LinkButton ID="lnkAddRowTest" runat="server" OnCommand="lnkAddRowTest_Click"> </asp:LinkButton>
                                                                &nbsp; &nbsp;
                                                                <asp:LinkButton ID="lnkDeleteRowTest" runat="server" OnCommand="lnkDeleteRowTest_Click"> </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Test Id" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTestId" runat="server"></asp:Label>
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
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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
            </div>
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


