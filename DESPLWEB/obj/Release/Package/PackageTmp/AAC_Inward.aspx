<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="AAC_Inward.aspx.cs" Inherits="DESPLWEB.AAC_Inward" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Controls/UC_InwardHeader.ascx" TagName="UC_InwardHeader" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="480px"
            Style="background-color: #ECF5FF;" ScrollBars="Vertical">
            <table style="width: 100%;">

                <tr>
                    <td colspan="4">
                        <asp:Panel ID="pnlControl" runat="server" ScrollBars="Auto" Width="90%">
                            <uc1:UC_InwardHeader ID="UC_InwardHeader1" OnTextChanged="TextChanged" OnClick="Click"
                                runat="server" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td colspan="2">
                        <asp:Label ID="tblGridDetails" runat="server" Font-Bold="True" Text="Sample Details"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTestedAt" runat="server" Text="Tested At : "></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlTestedAt" runat="server" Width="145px" Height="20px">
                            <asp:ListItem Text="Lab" Value="0" />
                            <asp:ListItem Text="Prayeja City" Value="1" />
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSelectRptClientSite" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkSelectRptClientSite_Click">Select Client for Report</asp:LinkButton>
                        <asp:Label ID="lblRptClientId" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblRptSiteId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>

                    <td colspan="2">&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>

            <%--   <tr>
                    <td colspan="3" valign="top">--%>
           <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Width="920px">
                 <asp:GridView ID="grdAACInward" runat="server" SkinID="gridviewSkin" OnRowDataBound="grdAACInward_RowDataBound"
                    AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="lblSrNo" HeaderText="&nbsp;Sr.No&nbsp;" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="ID Mark" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtIdMark" runat="server" Text='' BorderWidth="0px" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCSQty" runat="server" Text='' BorderWidth="0px" Width="50px"
                                    Style="text-align: center" MaxLength="2" onchange="checkNum(this)" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Test" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddl_Test" runat="server" BorderWidth="0px" Width="150px">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description"
                            HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDescription" runat="server" BorderWidth="0px" Text=''
                                    Width="300px" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Supplier Name" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlSupplier" runat="server" BorderWidth="0px" Width="200px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <div>
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Other Charges"></asp:Label>
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
                                <asp:TextBox ID="txtQuantity" runat="server" Width="60px" Style="text-align: Center; border-bottom: none; border-top: none"
                                    MaxLength="5" BorderWidth="0px" BorderColor="Black"
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
                    <td colspan="3" align="right"></td>
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
        function CalculateCastingDt(dateFmt) {
            var re = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
            var res = dateFmt.value.toUpperCase();
            if (dateFmt.value != '') {
                if (res == 'NA') {
                    return true;
                }
                if (re.test(dateFmt.value)) {
                    birthdayDate = new Date(dateFmt.value);
                    dateNow = new Date();
                    var years = dateNow.getFullYear() - birthdayDate.getFullYear();
                    var months = dateNow.getMonth() - birthdayDate.getMonth();
                    var days = dateNow.getDate() - birthdayDate.getDate();
                    if (isNaN(years)) {

                        document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                        document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Input date is incorrect";
                        dateFmt.value = "";
                        dateFmt.focus();
                        return false;
                    }
                    else {
                        document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                        if (months < 0 || (months == 0 && days < 0)) {
                            years = parseInt(years) - 1;

                        }
                        else {

                        }
                    }
                }
                else {

                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Date must be dd/MM/yyyy format";
                    dateFmt.value = "";
                    dateFmt.focus();
                    return false;
                }
            }
        }

        function CalculateDtOfTesting(dateFmt) {
            var re = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
            if (dateFmt.value != '') {
                if (re.test(dateFmt.value)) {
                    birthdayDate = new Date(dateFmt.value);
                    dateNow = new Date();
                    var years = dateNow.getFullYear() - birthdayDate.getFullYear();
                    var months = dateNow.getMonth() - birthdayDate.getMonth();
                    var days = dateNow.getDate() - birthdayDate.getDate();
                    if (isNaN(years)) {

                        document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                        document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Input date is incorrect";
                        dateFmt.value = "";
                        dateFmt.focus();
                        return false;
                    }
                    else {
                        document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                        if (months < 0 || (months == 0 && days < 0)) {
                            years = parseInt(years) - 1;

                        }
                        else {

                        }
                    }
                }
                else {

                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Date must be dd/MM/yyyy format";
                    dateFmt.value = "";
                    dateFmt.focus();
                    return false;
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
