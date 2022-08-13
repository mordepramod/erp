<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Solid_Inward.aspx.cs" Inherits="DESPLWEB.Solid_Inward" Title="Masonary Block Inward "
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Controls/UC_InwardHeader.ascx" TagName="UC_InwardHeader" TagPrefix="uc1" %>
<%@ Register Src="Controls/UC_InwardFooter.ascx" TagName="UC_InwardFooter" TagPrefix="uc2" %>
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
                                     OnClick="imgClose_Click" ImageAlign="Right" />
                            </td>
                        </tr>--%>
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
                            <asp:GridView ID="grdSolidBlockInward" runat="server" SkinID="gridviewSkin" OnRowDataBound="grdSolidBlockInward_RowDataBound"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Types Of Blocks" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddl_TypeOfBlocks" runat="server" BorderWidth="0px" Width="100px">
                                                <asp:ListItem Text="Select" />
                                                <asp:ListItem Text="Solid" />
                                                <asp:ListItem Text="Hollow" />
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID Mark" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_IdMark" runat="server" BorderWidth="0px" Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Casting Date" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDateofCasting" runat="server" BorderWidth="0px" Width="80px"
                                                MaxLength="10" onblur="CalculateCastingDt(this)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Test" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddl_Test" runat="server" BorderWidth="0px" Width="180px">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Quantity" runat="server" BorderWidth="0px" Width="50px" MaxLength="2"
                                                Style="text-align: right;" onchange="checkNum(this)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Schedule" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Schedule" runat="server" BorderWidth="0px" Width="50px" MaxLength="2"
                                                Style="text-align: right;" OnTextChanged="txt_Schedule_TextChanged" AutoPostBack="true"
                                                onchange="checkNum(this)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date Of Testing" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_DateofTesting" BorderWidth="0px" Width="90px" runat="server"
                                                onblur="CalculateDtOfTesting(this)" MaxLength="10"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Description" runat="server" BorderWidth="0px" Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supplier Name" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlSupplier" runat="server" BorderWidth="0px" Width="100px" />
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
