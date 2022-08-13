<%@ Page Title="Soil - Shrinkage Limit" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="Soil_Report_ShrinkageLimit.aspx.cs" Inherits="DESPLWEB.Soil_Report_ShrinkageLimit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <div style="height: 5px" align="right">
                <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                    OnClick="imgClosePopup_Click" ImageAlign="Right" />
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="pnlDetails" runat="server" Width="942px" BorderWidth="1px">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 97px">
                            <asp:Label ID="lblRefNo" runat="server" Text="Reference No"></asp:Label>
                        </td>
                        <td style="width: 305px">
                            <asp:TextBox ID="txtRefNo" runat="server" ReadOnly="true" Width="235px"></asp:TextBox>
                            <asp:Label ID="lblStatus" runat="server" Text="Enter" Visible="false"></asp:Label>
                        </td>
                        <td style="width: 113px">
                            <asp:Label ID="lblSampleName" runat="server" Text="Sample Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSampleName" runat="server" Width="235px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRecordNo" runat="server" Text="Report No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRecordType" runat="server" ReadOnly="true" Text="SO" Width="47px"></asp:TextBox>
                            <asp:TextBox ID="txtReportNo" runat="server" ReadOnly="true" Width="180px"></asp:TextBox>
                        </td>
                        <td style="width: 113px"></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="NABL Scope" Font-Bold="true"></asp:Label>
                        </td>
                        <td>

                            <asp:DropDownList ID="ddl_NablScope" AutoPostBack="true" Width="240px" runat="server">
                                <asp:ListItem Text="--Select--" />
                                <asp:ListItem Text="F" />
                                <asp:ListItem Text="NA" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>

                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_NABLLocation" runat="server" Width="242px" Enabled="true">
                                <asp:ListItem Value="0" Text="0" />
                                <asp:ListItem Value="1" Text="1" />
                                <asp:ListItem Value="2" Text="2" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="pnlSoilTab" runat="server" ScrollBars="Auto" Height="340px" Width="940px"
                BorderWidth="1px">
                <cc1:TabContainer ID="TabContainerSoil" runat="server" Width="100%" ActiveTabIndex="0">
                    <cc1:TabPanel runat="server" HeaderText="Shrinkage Limit" ID="TabShLim" Width="100%" Visible="false">
                        <ContentTemplate>
                            <asp:Panel ID="pnlShLim" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblShLimDateOfTesting" runat="server" Text="Date Of Testing"></asp:Label>
                                            &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtShLimDateOfTesting" runat="server" Width="130px"
                                                AutoPostBack="True"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtShLimDateOfTesting">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtShLimDateOfTesting"
                                                MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="">
                                            </cc1:MaskedEditExtender>                                            
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="lblShLimRows" runat="server" Text="Number of Rows"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:TextBox ID="txtShLimRows" runat="server" Width="30px" AutoPostBack="True" OnTextChanged="txtShLimRows_TextChanged"
                                                onkeyup="checkNum(this);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr> <td colspan="2"> &nbsp; </td></tr>
                                    <tr valign="top">
                                        <td colspan="2">
                                            <asp:Panel ID="pnlgrdShLim" runat="server" Height="220px" Width="920px" ScrollBars="Auto">
                                                <asp:GridView ID="grdShLim" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin" OnRowDataBound="grdShLim_OnRowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr. No.">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_SrNo" CssClass="ac" runat="server" ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>"
                                                                    BorderWidth="0" BackColor="LightGray">  </asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="50px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No. of Dish">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlShLim_NOOfDish" runat="server" BorderWidth="0" AutoPostBack="true" OnSelectedIndexChanged="ddlShLim_NOOfDish_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ControlStyle Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight of Dish + Dry soil">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_WeightOfDishPlusDrySoil" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this)"></asp:TextBox>                                                                
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ControlStyle Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight of Dish + Wet soil">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_WeightOfDishPlusWetSoil" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this)"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ControlStyle Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight of water">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_WeightOfWater" runat="server" CssClass="ac" BackColor="LightGray" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight of empty Dish">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_WeightOfEmptyDish" runat="server" CssClass="ac" BackColor="LightGray" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight of Wet Soil (M2)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_WeightOfWetSoil_M2" CssClass="ac" runat="server" BackColor="LightGray" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight of Dry Soil (M1)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_WeightOfDrySoil_M1" CssClass="ac" runat="server" BackColor="LightGray" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Moisture content Ratio (W1)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_MoistureContentRatio_W1" CssClass="ac" runat="server" BackColor="LightGray" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Moisture content %">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_MoistureContentPercent" CssClass="ac" runat="server" BackColor="LightGray" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight of Dish + Mercury filling">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_WeightOfDishPlusMercuryFilling" CssClass="ac" runat="server" BackColor="LightGray" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight of Mercury">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_WeightOfMercury" CssClass="ac" runat="server" onkeyup="checkDecimal(this)" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Volume of Wet Soil pat(V)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_VolumeOfWetSoilPat_V" CssClass="ac" runat="server" BackColor="LightGray" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight of Weighing Dish + Displaced Mercury">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_WeightOfWeighingDishPlusDisplacedMercury" CssClass="ac" runat="server" BackColor="LightGray" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight of Weighing Dish empty">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_WeightOfWeighingDishEmpty" CssClass="ac" runat="server" onkeyup="checkDecimal(this)" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight of Displaced Mercury">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_WeightOfDisplacedMercury" CssClass="ac" runat="server" onkeyup="checkDecimal(this)" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Volume of Dry Soil pat(Vd)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_VolumeOfDrySoilPat_Vd" CssClass="ac" runat="server" BackColor="LightGray" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Shrinkage Limit">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_ShrinkageLimit" CssClass="ac" runat="server" BackColor="LightGray" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Average Shrinkage Limit">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtShLim_AverageShrinkageLimit" CssClass="ac" runat="server" BackColor="LightGray" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Remark" ID="TabRemark">
                        <ContentTemplate>
                            <asp:Panel ID="pnlRemark" runat="server" ScrollBars="Auto" Height="322px" Width="100%"
                                BorderWidth="0px">
                                <div style="height: 40px">
                                </div>
                                <asp:GridView ID="grdRemark" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ImageUrl="~/Images/AddNewitem.jpg" ID="imgBtnAddRow" runat="server"
                                                    Width="18px" OnClick="imgBtnAddRow_Click" ImageAlign="Middle" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ImageUrl="~/Images/DeleteItem.png" ID="imgBtnDeleteRow" runat="server"
                                                    Width="16px" OnClick="imgBtnDeleteRow_Click" ImageAlign="Middle" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarkSrNo" CssClass="ac" runat="server" ReadOnly="true" AutoPostBack="true"
                                                    BorderWidth="0" Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="50px" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemark" runat="server" Width="800" BorderWidth="0px"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <table width="99%">
                <tr>
                    <td width="10%">
                        <asp:CheckBox ID="chkWitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chkWitnessBy_CheckedChanged" />
                        <asp:Label ID="lblWitnessBy" runat="server" Text="Witness By"></asp:Label>
                    </td>
                    <td width="17%">
                        <asp:TextBox ID="txtWitnesBy" Visible="false" runat="server" Width="140px"></asp:TextBox>
                    </td>
                    <td width="8%" style="text-align: right">
                        <asp:Label ID="lblTestdApprdBy" runat="server" Text="Approved By"></asp:Label>
                    </td>
                    <td width="17%" align="center">
                        <asp:DropDownList ID="ddlTestdApprdBy" runat="server" Width="140px">
                        </asp:DropDownList>
                    </td>
                    <td width="8%">
                        <%--<asp:Label ID="lblEntdChkdBy" runat="server" Text="Checked By"></asp:Label>--%>
                    </td>
                    <td width="17%" align="center">
                        <%--<asp:TextBox ID="txtEntdChkdBy" runat="server" Width="140px" ReadOnly="true"></asp:TextBox>--%>
                    </td>
                    <td width="30%" align="right">
                        <asp:LinkButton ID="lnkCalculate" Font-Size="Small" runat="server" OnClick="lnkCalculate_Click"
                            Font-Underline="true">Calculate</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkSave" Font-Size="Small" runat="server" OnClick="lnkSave_Click"
                            Font-Underline="true">Save</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Size="Small" Visible="false"
                            Font-Underline="true" OnClick="lnkPrint_Click">Print</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkExit" runat="server" Font-Size="Small" Font-Underline="true"
                            OnClick="lnkExit_Click">Exit</asp:LinkButton>
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
                    alert("Only Numeric Values Allowed");

                    return false;
                }
            }
            return true;
        }
        function checkDecimal(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 46))) {
                    x.value = '';
                    x.focus();
                    alert("Only Decimal Values Allowed");

                    return false;
                }

            }
            return true;
        }


    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>

