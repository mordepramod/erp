<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Soil_Report_LLPLPI.aspx.cs" Inherits="DESPLWEB.Soil_Report_LLPLPI" Theme="duro" %>

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
                        <td style="width: 113px">
                        </td>
                        <td>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="NABL Scope" Font-Bold="true"></asp:Label>
                        </td>
                        <td>

                            <asp:DropDownList ID="ddl_NablScope" AutoPostBack="true" Width="240px" runat="server" Height="17px">
                                <asp:ListItem Text="--Select--" />
                                <asp:ListItem Text="F" />
                                <asp:ListItem Text="NA" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>

                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_NABLLocation" runat="server"  Width="242px" Enabled="true">
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
            <asp:Panel ID="pnlSoilTab" runat="server" ScrollBars="Auto" Height="360px" Width="940px"
                BorderWidth="1px">
                <cc1:TabContainer ID="TabContainerSoil" runat="server" Width="100%" ActiveTabIndex="0">
                    <cc1:TabPanel runat="server" HeaderText="LL / PL / PI" ID="TabLLPL" Width="100%" Visible="false">
                        <ContentTemplate>
                            <asp:Panel ID="pnlLLPL" runat="server">
                                <table width="98%" height="322px">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblLLPLDateOfTesting" runat="server" Text="Date Of Testing"></asp:Label>
                                            &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtLLPLDateOfTesting" runat="server" Width="130px"
                                                AutoPostBack="True"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtLLPLDateOfTesting">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtLLPLDateOfTesting"
                                                MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="">
                                            </cc1:MaskedEditExtender>&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblPI" runat="server" Text="Plasticity Index"></asp:Label>&nbsp;&nbsp;
                                            <asp:TextBox ID="txtPI" runat="server" Width="120px" ReadOnly="True" 
                                                BackColor="LightGray"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="lblLL" runat="server" Text="Liquid Limit" Font-Bold="True" ForeColor="DarkOrange"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblLLRows" runat="server" Text="Number of Rows"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:TextBox ID="txtLLRows" runat="server" Width="30px" AutoPostBack="True" OnTextChanged="txtLLRows_TextChanged"
                                                onkeyup="checkNum(this);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlgrdLL" runat="server" Height="120px" ScrollBars="Auto">
                                                <asp:GridView ID="grdLL" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin" OnRowDataBound="grdLL_OnRowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr. No.">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLLSrNo" CssClass="ac" runat="server" ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>"
                                                                    BorderWidth="0" BackColor="LightGray">  </asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="50px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cont No">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlLLContNo" runat="server" BorderWidth="0" AutoPostBack="true" OnSelectedIndexChanged="ddlLLContNo_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ControlStyle Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No Of Blows">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLLNoOfBlows" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this)"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ControlStyle Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass Wet S + Cont">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLLMassWetSPlusCont" runat="server" CssClass="ac" onkeyup="checkDecimal(this)"
                                                                    BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass Dry S + Cont">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLLMassDrySPlusCont" runat="server" CssClass="ac" onkeyup="checkDecimal(this)"
                                                                    BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass Of Cont">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLLMassOfCont" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass Of Moist">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLLMassOfMoist" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass Of Dry S">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLLMassOfDryS" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="% Moist">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLLMoist" CssClass="ac" runat="server" BackColor="LightGray" ReadOnly="true"
                                                                    BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="LiquidLimit">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLLLiquidLimit" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
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
                                    <tr>
                                    <td></td> 
                                        <td align="left" valign="top">
                                            <asp:Label ID="lblPL" runat="server" Text="Plastic Limit" Font-Bold="True" ForeColor="DarkOrange"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblPLRows" runat="server" Text="Number of Rows"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:TextBox ID="txtPLRows" runat="server" Width="30px" AutoPostBack="True" OnTextChanged="txtPLRows_TextChanged"
                                                onkeyup="checkNum(this);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlgrdPL" runat="server" Height="120px" ScrollBars="Auto">
                                                <asp:GridView ID="grdPL" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin" OnRowDataBound="grdPL_OnRowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr. No.">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPLSrNo" CssClass="ac" runat="server" ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>"
                                                                    BorderWidth="0" BackColor="LightGray">  </asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="50px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cont No">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlPLContNo" runat="server" BorderWidth="0" AutoPostBack="true" OnSelectedIndexChanged="ddlPLContNo_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ControlStyle Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass Wet S + Cont">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPLMassWetSPlusCont" runat="server" CssClass="ac" onkeyup="checkDecimal(this)"
                                                                    BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass Dry S + Cont">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPLMassDrySPlusCont" runat="server" CssClass="ac" onkeyup="checkDecimal(this)"
                                                                    BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass Of Cont">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPLMassOfCont" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass Of Moist">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPLMassOfMoist" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass Of Dry S">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPLMassOfDryS" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="% Moist">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPLMoist" CssClass="ac" runat="server" BackColor="LightGray" ReadOnly="true"
                                                                    BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="LiquidLimit">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPLPlasticLimit" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
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
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Size="Small"  Visible="false"
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
