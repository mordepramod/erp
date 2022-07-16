<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Soil_Report_FSI.aspx.cs" Inherits="DESPLWEB.Soil_Report_FSI" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <div style="height: 5px" align="Right">
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
                            <asp:TextBox ID="txtSampleName" runat="server" Width="235px" ></asp:TextBox>
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
                        <td>
                            
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                      <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="NABL Scope" Font-Bold="true"></asp:Label>
                        </td>
                        <td>

                            <asp:DropDownList ID="ddl_NablScope" AutoPostBack="true" Width="240px" runat="server" >
                                <asp:ListItem Text="--Select--" />
                                <asp:ListItem Text="F" />
                                <asp:ListItem Text="NA" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>

                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_NABLLocation" runat="server"  Width="241px" Enabled="true">
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
                <cc1:TabContainer ID="TabContainerSoil" runat="server" Width="100%" 
                    ActiveTabIndex="1">
                    <cc1:TabPanel runat="server" HeaderText="FSI" ID="TabFSI" Width="100%" Visible="false">
                        <ContentTemplate>
                        <asp:Panel ID="pnlFSI" runat="server" >
                            <table width="98%" height="322px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFSIDateOfTesting" runat="server" Text="Date Of Testing"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtFSIDateOfTesting" runat="server" Width="130px" AutoPostBack="True"
                                            onClick="testDatePicker()"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFSIDateOfTesting">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFSIDateOfTesting"
                                            MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:Label ID="lblFSIRows" runat="server" Text="Number of Rows"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtFSIRows" runat="server" Width="30px" AutoPostBack="True" OnTextChanged="txtFSIRows_TextChanged"
                                            onkeyup="checkNum(this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="pnlgrdFSI" runat="server" Height="280px" ScrollBars="Auto">
                                            <asp:GridView ID="grdFSI" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFSISrNo" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                                Text=" <%#Container.DataItemIndex+1 %>" Width="30px">  </asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Determination No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFSIDeterminationNo" runat="server" BorderWidth="0" Width="180px" ></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Keroscene">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFSIKeroscene" runat="server" BorderWidth="0" CssClass="ac" onchange="javascript:checkNum(this);"
                                                                Width="120px" ></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Distilled Water">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFSIDistilledWater" runat="server" BorderWidth="0" CssClass="ac"
                                                                onchange="javascript:checkNum(this);" Width="120px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="FSI(%)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFSI" runat="server" BackColor="LightGray" BorderWidth="0" CssClass="ac"
                                                                ReadOnly="true" Width="120px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Average FSI(%)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFSIAvg" runat="server" BackColor="LightGray" BorderWidth="0"
                                                                CssClass="ac" ReadOnly="true" Width="120px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
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
                    <cc1:TabPanel runat="server" HeaderText="Water Content" ID="TabWC" Visible="false">
                        <ContentTemplate>
                        <asp:Panel ID="pnlWC" runat="server" >
                            <table width="100%">
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label ID="lblWCDateOfTesting" runat="server" Text="Date Of Testing"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWCDateOfTesting" runat="server" Width="200px" AutoPostBack="True"
                                            onClick="testDatePicker()"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtWCDateOfTesting">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtWCDateOfTesting"
                                            MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblWCDryWt" runat="server" Text="Dry Weight"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWCDryWt" runat="server" Width="200px" onkeyup="checkDecimal(this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblWCWetWt" runat="server" Text="Wet Weight"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWCWetWt" runat="server" Width="200px" onkeyup="checkDecimal(this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblWCContNo" runat="server" Text="Container No."></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlWCContNo" runat="server" Width="205px" 
                                            onselectedindexchanged="ddlWCContNo_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblWCContWt" runat="server" Text="Container Weight"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWCContWt" runat="server" Width="200px" ReadOnly="True" 
                                            BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblWCWaterContent" runat="server" Text="Water Content"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWCWaterContent" runat="server" Width="200px" ReadOnly="True"
                                            BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style ="height:143px">
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel> 
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Classification" ID="TabClassification" Visible="false">
                        <ContentTemplate>
                        <asp:Panel ID="pnlCL" runat="server" >
                            <table width="100%">
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label ID="lblCLDateOfTesting" runat="server" Text="Date Of Testing"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCLDateOfTesting" runat="server" Width="200px" AutoPostBack="True"
                                            onClick="testDatePicker()"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtCLDateOfTesting">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txtCLDateOfTesting"
                                            MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCLClassification" runat="server" Text="Classification"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCLClassification" runat="server" Width="200px" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCLMappingColour" runat="server" Text="Mapping Colour"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCLMappingColour" runat="server" Width="200px" ></asp:TextBox>
                                    </td>
                                </tr>                                
                                <tr style ="height:218px">
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel> 
                        </ContentTemplate>
                    </cc1:TabPanel>
                    
                    <cc1:TabPanel runat="server" HeaderText="Direct Shear" ID="TabDirectShear" Visible="false">
                        <ContentTemplate>
                        <asp:Panel ID="pnlDS" runat="server" >
                            <table width="100%">
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label ID="lblDSDateOfTesting" runat="server" Text="Date Of Testing"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDSDateOfTesting" runat="server" Width="200px" AutoPostBack="True"
                                            onClick="testDatePicker()"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtDSDateOfTesting">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender4" TargetControlID="txtDSDateOfTesting"
                                            MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDSCohesionC" runat="server" Text="Cohesion - C"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDSCohesionC" runat="server" Width="200px" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDSAngle" runat="server" Text="Angle of Internal Friction - Ø "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDSAngle" runat="server" Width="200px" ></asp:TextBox>
                                    </td>
                                </tr>                                
                                <tr style ="height:206px">
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel> 
                        </ContentTemplate>
                    </cc1:TabPanel>

                    <cc1:TabPanel runat="server" HeaderText="pH" ID="TabPH" Visible="false">
                        <ContentTemplate>
                        <asp:Panel ID="pnlPH" runat="server" >
                            <table width="100%">
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label ID="lblPHDateOfTesting" runat="server" Text="Date Of Testing"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPHDateOfTesting" runat="server" Width="200px" AutoPostBack="True"
                                            onClick="testDatePicker()"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtPHDateOfTesting">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender5" TargetControlID="txtPHDateOfTesting"
                                            MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPH" runat="server" Text="pH"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPH" runat="server" Width="200px" ></asp:TextBox>
                                    </td>
                                </tr>
                                                             
                                <tr style ="height:228px">
                                    <td colspan="2">&nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel> 
                        </ContentTemplate>
                    </cc1:TabPanel>

                    <cc1:TabPanel runat="server" HeaderText="Remark" ID="TabRemark">
                        <ContentTemplate>
                            <asp:Panel ID="pnlRemark" runat="server" ScrollBars="Auto" Height="322px" Width="100%"
                                BorderWidth="0px">
                                <div style="height:40px">
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
                              Font-Underline="true" OnClick="lnkPrint_Click">Print</asp:LinkButton>&nbsp; <%--OnClientClick="myDataFun()"--%>
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
