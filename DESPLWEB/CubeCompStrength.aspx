<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MstPg_Veena.Master" CodeBehind="CubeCompStrength.aspx.cs" Theme="duro" Inherits="DESPLWEB.CubeCompStrength" Title="Cube Compressive Strength" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <div align="right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                    ImageAlign="Right" />
            </div>
            <table width="100%">
                <tr style="height: 80px">
                    <td>&nbsp;&nbsp;

                    </td>
                </tr>
                <tr valign="middle">
                    <td align="center">
                        <asp:Panel ID="Panel2" runat="server" BorderStyle="Ridge" Width="430px" BorderColor="AliceBlue">
                            <table style="width: 100%">
                                <tr valign="middle" style="background-color: #ECF5FF;">
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Reference No"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_ReferenceNo" Width="200px" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="background-color: #ECF5FF;">
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Record Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_RecordType" Width="200px" runat="server" ReadOnly="true"
                                            Height="22px"></asp:TextBox>

                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Casting Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_CatsingDt" Width="200px" runat="server"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_CatsingDt">
                                        </asp:CalendarExtender>
                                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_CatsingDt" MaskType="Date"
                                            Mask="99/99/9999" AutoComplete="false" runat="server">
                                        </asp:MaskedEditExtender>
                                    </td>
                                </tr>

                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <div style="height: 2px">
                &nbsp;&nbsp; 
            </div>
            <table width="100%">

                <tr valign="middle">
                    <td align="center">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="150px" BorderStyle="Ridge" Width="430px" BorderColor="AliceBlue">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdCubeCompStrength" runat="server" AutoGenerateColumns="false" SkinID="gridviewSkin">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Sr No.">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Days" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_Days" BorderWidth="0px" Style="text-align: center" runat="server" ReadOnly="true" onchange="javascript:checkNum(this);"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="No Of Cubes" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_NoOfcubes" BorderWidth="0px" Style="text-align: center" runat="server" Width="100px" MaxLength="2" onchange="javascript:checkNum(this);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="No Of Cement Cubes" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_NoOfCementCubes" BorderWidth="0px" Style="text-align: center" runat="server" Width="100px" MaxLength="2" onchange="javascript:checkNum(this);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <table>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lbl_CementCubes" runat="server" Text="28 Days Cement Cubes" Visible="false" Width="180px"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:TextBox ID="txt_CementCubes" runat="server" Style="text-align: center" Visible="false" Width="198px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>

            <table width="72%">
                <tr>
                    <td align="right">
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True" Style="text-decoration: underline;">Save</asp:LinkButton>
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

    </script>
</asp:Content>
