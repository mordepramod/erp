<%@ Page Title="Trial Other Information" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    Theme="duro" AutoEventWireup="true" CodeBehind="TrialOtherInfo.aspx.cs" Inherits="DESPLWEB.TrialOtherInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <%--<div>--%>
               <%-- &nbsp;&nbsp;--%>
                <%--<asp:Label ID="Label15" runat="server" Font-Bold="true" Text="Other Information"></asp:Label>--%>
                <%--<asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                    ImageAlign="Right" />
            </div>--%>
            
            <table width="100%">
                <tr>                    
                    <td align="center" valign="top" colspan="3"> <%--style="height:30px"--%>
                        <asp:Label ID="lblTrialName" Font-Bold="true" Font-Underline="true" runat="server"
                            Text=""  ForeColor="#000099" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>    
                    <td ></td>                
                    <td colspan="2" valign="middle" >
                        <asp:Label ID="lbl_OtherPending" runat="server" Text="Other Pending Reports"></asp:Label>&nbsp;&nbsp;
                    <%--</td> 
                    <td >--%>
                        <asp:DropDownList ID="ddl_OtherPendingRpt" Width="150px" runat="server" AutoPostBack="true"
                            onselectedindexchanged="ddl_OtherPendingRpt_SelectedIndexChanged" >
                            </asp:DropDownList> &nbsp;&nbsp;
                            <asp:Label ID="Label12" runat="server" Text="Trial"></asp:Label>&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlTrial" Width="100px" runat="server" >
                            </asp:DropDownList>&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkFetch" runat="server" Font-Bold="true" Style="text-decoration: underline;"
                                OnClick="lnkFetch_Click">Fetch</asp:LinkButton>     
                    </td>
                </tr>
                <tr>
                    <td style="width:25%"></td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Record No"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_RecType" Style="text-align: center" Width="25px" ReadOnly="true"
                            runat="server"></asp:TextBox>
                        <asp:TextBox ID="txt_RefNo" Width="180px" ReadOnly="true" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="chk_MDLetter" runat="server" />
                        <asp:Label ID="Label16" runat="server" Text="Prepare MD Letter"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_TrialId" Visible="false" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblMDLetterMsg" Visible="false" runat="server" Text="" ForeColor="Brown"></asp:Label>
                    </td>
                </tr>
                <tr>
                <td></td>
                    <td>
                        <asp:Label ID="Label17" runat="server" Text="Batching"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Batching" runat="server" Width="220px">
                            <asp:ListItem Text="---Select---" />
                            <asp:ListItem Text="Volume Batching" />
                            <asp:ListItem Text="Weight Batching" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                <td></td>
                    <td>
                        <asp:Label ID="Label18" runat="server" Text="Slump"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Slump1" Width="60px" MaxLength="4" runat="server" onkeyup="checkDecimal(this)" CssClass="ac"></asp:TextBox>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txt_Slump2" Width="60px" MaxLength="4" runat="server" onkeyup="checkDecimal(this)" CssClass="ac"></asp:TextBox>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txt_Slump3" Width="60px" MaxLength="4" runat="server" onkeyup="checkDecimal(this)" CssClass="ac"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td></td>
                    <td>
                       <asp:Label ID="Label19" runat="server" Text="Compactability"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButton ID="Rdn_Good" runat="server" GroupName="Compac" />
                        <asp:Label ID="lbl_Good" runat="server" Text="Good"></asp:Label>&nbsp;
                        <asp:RadioButton ID="Rdn_Average" runat="server" GroupName="Compac" />
                        <asp:Label ID="lbl_Avg" runat="server" Text="Average"></asp:Label>&nbsp;
                        <asp:RadioButton ID="Rdn_Poor" runat="server" GroupName="Compac" />
                        <asp:Label ID="lbl_Poor" runat="server" Text="Poor"></asp:Label>
                    </td>
                </tr>
                <tr>
                <td></td>
                    <td>
                        <asp:Label ID="Label23" runat="server" Text="Remark"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Remark" runat="server" Width="220px">
                            <asp:ListItem Text="---Select---" />
                            <asp:ListItem Text="Mix was Cohesive" />
                            <asp:ListItem Text="Mix was Non-Cohesive" />
                            <asp:ListItem Text="Mix was Harsh" />
                            <asp:ListItem Text="Mix was Stiff" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                <td></td>
                    <td style="height: 25px; width: 20%">
                        <asp:CheckBox ID="chk_Retention" runat="server" AutoPostBack="true" OnCheckedChanged="chk_Retention_CheckedChanged" />
                        <asp:Label ID="Label24" runat="server" Text="Retention Slump Required "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_EnterTime" runat="server" Text="Enter Time Duration After" ></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txt_AfterMin" runat="server" Width="40px" MaxLength="4" Enabled="false" CssClass="ac"></asp:TextBox>
                        <asp:Label ID="lbl_Min" runat="server" Text="Min"></asp:Label>
                    </td>
                </tr>
                <tr>
                <td></td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Slump"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_RetSlump1" Width="60" MaxLength="4" runat="server" Enabled="false" CssClass="ac"
                            onkeyup="checkDecimal(this)"></asp:TextBox>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txt_RetSlump2" Width="60" MaxLength="4" runat="server" Enabled="false" CssClass="ac"
                            onkeyup="checkDecimal(this)"></asp:TextBox>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txt_RetSlump3" Width="60" MaxLength="4" runat="server" Enabled="false" CssClass="ac"
                            onkeyup="checkDecimal(this)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td></td>
                    <td align="left">
                        <asp:Label ID="Label4" runat="server" Text="Yield"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtYield" Style="text-align: center" Width="220px" ReadOnly="true"
                            runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td></td>
                    <td align="left">
                        <asp:Label ID="Label5" runat="server" Text="Final W/C Ratio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFinalWCRatio" Style="text-align: center"  Width="220px"  ReadOnly="true"
                            runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td></td>
                    <td align="left">
                        <asp:Label ID="Label6" runat="server" Text="Compaction Factor"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCompactionFactor" Style="text-align: center" Width="220px" ReadOnly="true"
                            runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td></td>
                    <td align="left">
                        <asp:Label ID="Label2" runat="server" Text=" Trail No. of Cubes"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_TrialNoCubes" Style="text-align: center" Width="50px" ReadOnly="true"
                            runat="server"></asp:TextBox>
                             &nbsp;&nbsp;
                                  <asp:Label ID="Label7" runat="server" Text="Casting Date"></asp:Label>
                                  &nbsp;&nbsp;
                                  <asp:TextBox ID="txt_Castingdt" Width="70px"   runat="server" ReadOnly="true"></asp:TextBox>
                                  <%--<asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_Castingdt">
                                  </asp:CalendarExtender> 
                                  <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_Castingdt" MaskType="Date"
                                                Mask="99/99/9999" AutoComplete="false" runat="server">
                                  </asp:MaskedEditExtender>--%>
                    </td>
                </tr>
                <tr>
                <td></td>
                    <td valign="top" align="left">
                        <asp:CheckBox ID="chk_CubeCasting" runat="server" AutoPostBack="true" OnCheckedChanged="chk_CubeCasting_CheckedChanged" />
                        <asp:Label ID="Label25" runat="server" Text="Update Cube Casting"></asp:Label>
                    </td>
                    <td>
                        <asp:Panel ID="PnlCubeCast" ScrollBars="Auto" BorderStyle="Ridge" runat="server"
                            BorderColor="AliceBlue" Width="220px" Height="100px">
                            <asp:GridView ID="grdCubeCasting" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnAddRow" runat="server" OnCommand="imgBtnAddRow_Click"
                                                ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                ToolTip="Add Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="18px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnDeleteRow" runat="server" OnCommand="imgBtnDeleteRow_Click"
                                                ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                ToolTip="Delete Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="lblSrNo" ItemStyle-Width="20px" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="Days">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Days" BorderWidth="0px" Width="50px" runat="server" MaxLength="2"
                                                Style="text-align: center" onkeyup="checkNum(this)"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cubes" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Cubes" BorderWidth="0px" Width="50px" runat="server" MaxLength="2"
                                                Style="text-align: center" onkeyup="checkNum(this)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                <td></td>
                    <td colspan="2" align="right">
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClick="lnk_Exit_Click" runat="server">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
             
        </asp:Panel>
    </div>
    <script type="text/javascript">

        function checkDecimal(x) {
            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 46))) {
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
