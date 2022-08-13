<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Cube_Inward.aspx.cs" Inherits="DESPLWEB.Cube_Inward" Title="Cube Inward"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Controls/UC_InwardHeader.ascx" TagName="UC_InwardHeader" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="480px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <table style="width: 100%;">
                <%-- <tr style="background-color: #ECF5FF;">
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
                            <uc1:UC_InwardHeader ID="UC_InwardHeader1" OnTextChanged="TextChanged" OnClick="Click"
                                runat="server" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td colspan="4">
                        <asp:Label ID="tblGridDetails" runat="server" Font-Bold="True" Text="Sample Details"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        Test Type :
                        <asp:DropDownList ID="ddlTestType" runat="server" Width="145px" Height="20px">
                            <asp:ListItem Text="Concrete Cube" />
                            <asp:ListItem Text="Accelerated Curing" />
                            <asp:ListItem Text="Mortar Cube" />
                            <asp:ListItem Text="Site Cube Casting" />
                        </asp:DropDownList>                       
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
                    <%--</td>                  
                    <td colspan="2">--%>
                        <asp:Label ID="lblNoOfCoupons1" runat="server" Font-Bold="True" Text="Unused Coupons : "></asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblNoOfCoupons" runat="server" Font-Bold="True" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="170px" Width="940px">
                            <asp:GridView ID="grdCubeInward" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="lblSrNo" HeaderText="&nbsp;Sr.No&nbsp;" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="ID Mark" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtIdMark" runat="server" Text='' BorderWidth="0px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCSQty" runat="server" Text='' BorderWidth="0px" Width="50px"
                                                Style="text-align: center" MaxLength="2" onchange="checkNum(this)" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Casting Date" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDateOfCasting" runat="server" BorderWidth="0px" Text='' Width="85px"
                                                Style="text-align: center" MaxLength="10" AutoPostBack="true"   OnTextChanged="txt_Schedule_TextChanged"/> <%--onchange="CalculateCastingDt(this)"--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Grade" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlGrade" runat="server" BorderWidth="0px" Width="63px">
                                                <asp:ListItem Text="Select" />
                                                <asp:ListItem Text="M 5" />
                                                <asp:ListItem Text="M 7.5" />
                                                <asp:ListItem Text="M 10" />
                                                <asp:ListItem Text="M 15" />
                                                <asp:ListItem Text="M 20" />
                                                <asp:ListItem Text="M 25" />
                                                <asp:ListItem Text="M 30" />
                                                <asp:ListItem Text="M 35" />
                                                <asp:ListItem Text="M 37" />
                                                <asp:ListItem Text="M 40" />
                                                <asp:ListItem Text="M 45" />
                                                <asp:ListItem Text="M 50" />
                                                <asp:ListItem Text="M 55" />
                                                <asp:ListItem Text="M 60" />
                                                <asp:ListItem Text="M 65" />
                                                <asp:ListItem Text="M 70" />
                                                <asp:ListItem Text="M 75" />
                                                <asp:ListItem Text="M 80" />
                                                <asp:ListItem Text="M 85" />
                                                <asp:ListItem Text="M 90" />
                                                <asp:ListItem Text="M 95" />
                                                <asp:ListItem Text="M 100" />
                                                <asp:ListItem Text="NA" />
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="&nbsp;Schedule&nbsp;" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSchedule" runat="server" Text='' BorderWidth="0px" Width="60px"
                                                Style="text-align: center" MaxLength="3" OnTextChanged="txt_Schedule_TextChanged"
                                                AutoPostBack="true" onchange="checkNum(this)" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Testing Date" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDateOfTesting" runat="server" Text='' BorderWidth="0px" Width="85px"
                                                Style="text-align: center" MaxLength="10" AutoPostBack="true" onchange="CalculateDtOfTesting(this)"  OnTextChanged="txt_Schedule_TextChanged"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nature Of Work" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNatureOfWork" runat="server" Text='' BorderWidth="0px" Width="180px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDescription" runat="server" Text='' BorderWidth="0px" Width="200px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>

                    <%--<td colspan="1" valign="top">
                        <asp:Panel ID="pnlCouponList" runat="server" ScrollBars="Auto" Height="180px" Width="80px">
                            <asp:Label ID="lblCoupon" runat="server" Text="Coupon No." Font-Bold="true"></asp:Label>
                            <asp:CheckBoxList ID="chkListCoupon" runat="server">
                            </asp:CheckBoxList>
                        </asp:Panel>
                    </td>--%>
                </tr>
                <tr>
                    <td colspan="2" style="height:22px">
                        <asp:CheckBox ID="chk_WitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chk_WitnessBy_CheckedChanged" />
                        <asp:Label ID="Label7" runat="server" Text="Witness By"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txt_witnessBy" Width="200px" runat="server" Visible="false"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td colspan="2" align="right">
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
