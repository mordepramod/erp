<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Other_Inward.aspx.cs" Inherits="DESPLWEB.Other_Inward" Title="Other Inward"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Controls/UC_InwardHeader.ascx" TagName="UC_InwardHeader" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="480px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <table style="width: 100%;">
                <tr>
                    <td colspan="4">
                        <asp:Panel ID="pnlControl" runat="server" ScrollBars="Auto" Width="90%">
                            <uc1:UC_InwardHeader ID="UC_InwardHeader1" OnTextChanged="TextChanged" OnClick="Click"
                                runat="server" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF; height: 26px;">
                    <td colspan="4">
                        <asp:Label ID="tblGridDetails" runat="server" Font-Bold="True" Text="Sample Details"></asp:Label>
                        &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Sub Test"></asp:Label>
                        &nbsp;&nbsp;
                        <%--<asp:TextBox ID="txt_SubTest" runat="server" Width="184px"></asp:TextBox>--%>
                        <asp:DropDownList ID="ddlTest" runat="server" Width="184px" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <%--<asp:Label ID="lblTestId" runat="server" Text="" Visible="false"></asp:Label>--%>
                        &nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                        <asp:CheckBox ID="chkAddNewTest" AutoPostBack="true" OnCheckedChanged="chkAddNewTest_CheckedChanged"
                            runat="server" Visible="false" />
                        <asp:Label ID="Label2" runat="server" Text="Add New Test" Visible="false"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txt_AddNewTest" Width="200px" Visible="false" runat="server"></asp:TextBox>
                        &nbsp;
                        <asp:Button ID="BtnAddNewTest" Width="40px" runat="server" Text="Add" Visible="false"
                            OnClick="BtnAddNewTest_Click" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSelectRptClientSite" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkSelectRptClientSite_Click">Select Client for Report</asp:LinkButton>
                        <asp:Label ID="lblRptClientId" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblRptSiteId" runat="server" Text="0" Visible="false"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkInterBranchRefNo" runat="server" Text="Inter Branch Reference No." OnCheckedChanged="chkInterBranchRefNo_CheckedChanged" AutoPostBack="true"/>
                        &nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtInterBranchRefNo" Width="100px" runat="server" Visible="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Width="920px">
                <%--Height="130px"--%>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdOtherInward" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin"
                            OnRowDataBound="grdOtherInward_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDescription" BorderWidth="0px" runat="server" Text='' Width="200px" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Name" HeaderStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <%--<asp:TextBox ID="txtSupplierName" BorderWidth="0px" runat="server" Text='' Width="200px" />--%>
                                        <asp:DropDownList ID="ddlSupplier" runat="server" BorderWidth="0px" Width="200px" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Test Details" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTestDetails" runat="server" Text='' BorderWidth="0px" Width="380px"
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
            <div>
                <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Other Charges"></asp:Label>
            </div>
            <asp:Panel ID="pnlOtherCharges" runat="server" ScrollBars="Auto" Width="920px">
                <%--Height="100px"--%>
                <asp:GridView ID="grdOtherCharges" runat="server" AutoGenerateColumns="false" SkinID="gridviewSkin"
                    CellPadding="2" CellSpacing="2">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgInsert" runat="server" OnCommand="imgInsert_Click" ImageUrl="Images/AddNewitem.jpg"
                                    Height="20px" Width="20px" CausesValidation="false" ToolTip="Add New Row" />
                            </ItemTemplate>
                            <ItemStyle Width="20px" />
                            <HeaderStyle HorizontalAlign="Center" Width="20px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" OnCommand="imgDelete_Click"
                                    Height="19px" ImageUrl="Images/DeleteItem.png" ToolTip="Delete Row" Width="18px" />
                            </ItemTemplate>
                            <ItemStyle Width="20px" />
                            <HeaderStyle HorizontalAlign="Center" Width="20px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="Sr.No" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" Width="35px" />
                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Particular" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDescription" runat="server" Width="380px" BorderWidth="0px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Quantity" ItemStyle-HorizontalAlign="right">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQuantity" runat="server" Width="60px" Style="text-align: Center;
                                    border-bottom: none; border-top: none" MaxLength="5" BorderWidth="0px" BorderColor="Black"
                                    AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged" onchange="javascript:checkNum(this);" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="62px" />
                            <ItemStyle HorizontalAlign="right" Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Rate" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtRate" runat="server" Width="100px" onchange="javascript:checkDec(this);"
                                    AutoPostBack="true" OnTextChanged="txtRate_TextChanged" MaxLength="15" BorderWidth="0px"
                                    Style="text-align: Center" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Amount" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAmount" runat="server" Width="100px" MaxLength="15" ReadOnly="true"
                                    Style="text-align: Center" BorderWidth="0px" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="215px" />
                            <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <table width="100%">
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
                            Style="text-decoration: underline;" OnClick="lnkSave_Click" Font-Bold="True">Save</asp:LinkButton>
                        &nbsp; &nbsp;
                        <asp:LinkButton ID="lnkExit" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnk_Exit_Click" Font-Bold="True">Exit</asp:LinkButton>
                        &nbsp; &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
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
                                    <asp:CheckBox ID="chkAddNewSubTest" AutoPostBack="true" OnCheckedChanged="chkAddNewSubTest_CheckedChanged"
                                        runat="server" Visible="false"/>
                                    <asp:Label ID="lblAddNewSubTest" runat="server" Text="Add New Test" Visible="false"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="txt_AddNewSubTest" Width="150px" Visible="false" runat="server"></asp:TextBox>
                                    &nbsp;
                                    <asp:Label ID="lblNewTestRate" runat="server" Text="Rate" Visible="false"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="txtNewTestRate" Width="100px" Visible="false" runat="server" onchange="checkNum(this)"></asp:TextBox>
                                    &nbsp;
                                    <asp:Button ID="BtnAddNewSubTest" Width="40px" runat="server" Text="Add" Visible="false"
                                        OnClick="BtnAddNewSubTest_Click" />
                                    <asp:Label ID="lblTestDetails" runat="server" Text="" Visible="false"></asp:Label>
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
                                                        <asp:Label ID="lblTest" Width="300px" runat="server" Text='<%#Eval("TEST_Name_var") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Test Id" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTestId" runat="server" Text='<%#Eval("TEST_Id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Qty">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQty" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Rate">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRate" runat="server" Width="50px" MaxLength="10" Text='<%#Eval("TEST_Rate_int") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
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
        function checkDec(inputtxt) {
            var numbers = /^\d+(\.\d{1,2})?$/;
            if (inputtxt.value.match(numbers)) {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";

            }
            else {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please enter valid integer or decimal number with 2 decimal places";
                inputtxt.focus();
                inputtxt.value = "";
                return false;
            }
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
