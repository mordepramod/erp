<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Soil_Report_CBR.aspx.cs" Inherits="DESPLWEB.Soil_Report_CBR" Theme="duro"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div style="height: 5px" align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            ImageAlign="Right" />
                        &nbsp;&nbsp;
                    </div>
                    <asp:Panel ID="pnlDetails" runat="server" Width="942px" BorderWidth="1px">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 97px">
                                    <asp:Label ID="lblRefNo" runat="server" Text="Reference No"></asp:Label>
                                </td>
                                <td style="width: 305px">
                                    <asp:TextBox ID="txtRefNo" runat="server" ReadOnly="true" Width="235px" MaxLength="50"></asp:TextBox>
                                    <asp:Label ID="lblStatus" runat="server" Text="Enter" Visible="false"></asp:Label>
                                </td>
                                <td style="width: 113px">
                                    <asp:Label ID="lblSampleName" runat="server" Text="Sample Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSampleName" runat="server" Width="235px" ReadOnly="true" MaxLength="250"
                                        Style="margin-left: 0px"></asp:TextBox>
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
                                <asp:Label ID="lblCheckSoak" runat="server" Text="CBR"></asp:Label>
                                </td>
                                <td>
                                <asp:TextBox ID="txtCheckSoak" runat="server" Width="235px" ReadOnly="true" MaxLength="10"
                                        Style="margin-left: 0px"></asp:TextBox>
                                </td>
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
                            <asp:DropDownList ID="ddl_NABLLocation" runat="server"  Width="241px" Enabled="true" Height="16px">
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
                            <cc1:TabPanel runat="server" HeaderText="CBR" ID="TabCBR" Width="100%" Visible="true">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlCBR" runat="server" Height="322px" Width="100%" ScrollBars="Auto">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 300px;">
                                                    <asp:Label ID="lblCBRDateOfTesting" runat="server" Text="Date Of Testing :"></asp:Label>
                                                    <asp:TextBox ID="txtCBRDateOfTesting" runat="server" Width="90px" AutoPostBack="True"
                                                        onClick="testDatePicker()"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                        Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtCBRDateOfTesting">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtCBRDateOfTesting"
                                                        MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                        CultureTimePlaceholder="">
                                                    </cc1:MaskedEditExtender>
                                                </td>
                                                <td colspan="2" style="width: 500px;">
                                                    <div style="margin-left: 160px;">
                                                        <asp:Label ID="lblCBRSoaked" Font-Bold="True" runat="server" Text="CBR-Soaked"></asp:Label>
                                                    </div>
                                                    <div style="margin-left: 340px;">
                                                        <asp:Label ID="lblNumberMouldsCasted" runat="server" Text="Number of moulds casted :"></asp:Label>
                                                        <asp:TextBox ID="txtMouldsCasted" runat="server" Width="90px" AutoPostBack="True"
                                                            MaxLength="10" CssClass="ac" OnTextChanged="txtMouldsCasted_TextChanged" onkeyup="checkNum(this);"></asp:TextBox>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 110px;">
                                                    <br />
                                                    <asp:Panel ID="pnlmould" runat="server" Height="180px" Width="100%" ScrollBars="Auto"
                                                        BackColor="LightGray">
                                                        <asp:GridView ID="grdMould" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                                            OnRowDataBound="grdMould_OnRowDataBound" Width="80%">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cbSelect" runat="server" OnCheckedChanged="cbSelect_CheckedChanged"
                                                                            AutoPostBack="true"></asp:CheckBox>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <ControlStyle Width="30px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sr. No.">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMouldSrNo" CssClass="ac" runat="server" ReadOnly="true" MaxLength="10"
                                                                            Text=" <%#Container.DataItemIndex+1 %>" BorderWidth="0" BackColor="LightGray">  </asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="50px" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Mould No">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlMouldNo" runat="server" BackColor="Transparent" BorderWidth="0">
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="80px" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Volume">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtVolume" ReadOnly="true" BackColor="LightGray" runat="server"
                                                                            CssClass="ac" BorderWidth="0" MaxLength="10"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="50px" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Height">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtHeight" ReadOnly="true" BackColor="LightGray" runat="server"
                                                                            CssClass="ac" BorderWidth="0" MaxLength="10"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="50px" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                                <td style="width: 450px;">
                                                    <asp:Label ID="lblDensityData" Font-Bold="True" runat="server" Text="Density Data"></asp:Label><br />
                                                    <asp:GridView ID="grdDensity" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                                        Width="500px">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Condition of Specimen">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtConditionSpecimen" runat="server" BorderWidth="0" ReadOnly="true"
                                                                        MaxLength="10" BackColor="LightGray"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ControlStyle Width="300px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Before Soaking">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtBeforeSoaking" runat="server" CssClass="ac" BorderWidth="0" MaxLength="10"
                                                                        onkeyup="checkDecimal(this);"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ControlStyle Width="110px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="After Soaking">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtAfterSoaking" runat="server" CssClass="ac" BorderWidth="0" MaxLength="10"
                                                                        onkeyup="checkDecimal(this);"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ControlStyle Width="100px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMouldDate" runat="server" Text="Moulding Date"></asp:Label>
                                                    <br />
                                                    <asp:TextBox ID="txtMouldingDate" runat="server" Width="90px" AutoPostBack="True"
                                                        onClick="testDatePicker()"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                        Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtMouldingDate">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtMouldingDate"
                                                        MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                        CultureTimePlaceholder="">
                                                    </cc1:MaskedEditExtender>
                                                    <br />
                                                    <br />
                                                    <asp:Label ID="lblPenetrationDate" runat="server" Text="Penetration Date"></asp:Label>
                                                    <br />
                                                    <asp:TextBox ID="txtPenetrationDate" runat="server" Width="90px" AutoPostBack="True"
                                                        onClick="testDatePicker()"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                        Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtPenetrationDate">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txtPenetrationDate"
                                                        MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                        CultureTimePlaceholder="">
                                                    </cc1:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblWCD" Font-Bold="True" runat="server" Text="Water Content Data"></asp:Label><br />
                                                    <asp:GridView ID="grdWaterContent" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin"
                                                        OnRowCreated="grdWaterContent_RowCreated" OnRowDataBound="grdWaterContent_OnRowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtgrdWCCol1" runat="server" BorderWidth="0" ReadOnly="true" BackColor="LightGray"
                                                                        MaxLength="10"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ControlStyle Width="330px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Before Compaction">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlBeforeCompaction" runat="server" BackColor="Transparent">
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox ID="txtBeforeCompaction" runat="server" CssClass="ac" BorderWidth="0"
                                                                        onkeyup="checkDecimal(this);" MaxLength="10"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ControlStyle Width="150px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="After Compaction">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlAfterCompaction" runat="server" BackColor="Transparent">
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox ID="txtAfterCompaction" runat="server" BorderWidth="0" CssClass="ac"
                                                                        onkeyup="checkDecimal(this);" MaxLength="10"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ControlStyle Width="140px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Top 30mm">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlTop30mm" runat="server" BackColor="Transparent">
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox ID="txtTop30mm" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this);"
                                                                        MaxLength="10"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ControlStyle Width="130px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Bulk">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlBulk" runat="server" BackColor="Transparent">
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox ID="txtBulk" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)"
                                                                        MaxLength="10"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ControlStyle Width="130px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td style="width: 550px">
                                                    <asp:Label ID="lblPenetrationData" Font-Bold="True" runat="server" Text="Penetration Data"></asp:Label><br />
                                                    <asp:Label ID="lblRingSize" runat="server" Text="Proving Ring Size : "></asp:Label>&nbsp;&nbsp;&nbsp;
                                                    <asp:DropDownList ID="ddlRingSize" runat="server" Width="50px" AutoPostBack="True" OnSelectedIndexChanged="ddlRingSize_SelectedIndexChanged">
                                                        <asp:ListItem Text="25"></asp:ListItem>
                                                        <asp:ListItem Text="10"></asp:ListItem>
                                                        <asp:ListItem Text="2.5"></asp:ListItem>
                                                        <asp:ListItem Text="2"></asp:ListItem>
                                                        <asp:ListItem Text="1"></asp:ListItem>
                                                    </asp:DropDownList><br />
                                                    <asp:GridView ID="grdPenetration" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                                        OnRowCreated="grdPenetration_RowCreated" Width="80%" OnRowDataBound="grdPenetration_OnRowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Penetration">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPenetration" runat="server" BorderWidth="0" CssClass="ac" BackColor="LightGray"
                                                                        MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ControlStyle Width="120px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Load Dial Reading">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlLoadDialReading" runat="server" BackColor="Transparent"
                                                                        BorderWidth="0">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <ControlStyle Width="130px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Corrected load from Chart">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtCorrectedloadfromChart" runat="server" BorderWidth="0" CssClass="ac" BackColor="LightGray"
                                                                        MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ControlStyle Width="210px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CBR">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtCBR" runat="server" CssClass="ac" BorderWidth="0" BackColor="LightGray"
                                                                        MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ControlStyle Width="120px" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                                <td style="width: 500px">
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 120px">
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:Label ID="lblCalculated" runat="server" Text="Calculated"></asp:Label>
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:Label ID="lblAspergraph" runat="server" Text="As per graph"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCBR25mm" runat="server" Text="CBR(Pent at 2.5mm)"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCalCBRPent25" runat="server" CssClass="ac" Width="60px" MaxLength="10"
                                                                    ReadOnly="True" BackColor="LightGray"></asp:TextBox>%
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtASPGCBRPent25" runat="server" CssClass="ac" Width="60px" onkeyup="checkDecimal(this);"
                                                                    MaxLength="10"></asp:TextBox>%
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <br />
                                                                <asp:Label ID="lblCBR50mm" runat="server" Text="CBR(Pent at 5.0mm)"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCalCBRPent50" runat="server" CssClass="ac" Width="60px" ReadOnly="True"
                                                                    BackColor="LightGray"></asp:TextBox>%
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtASPGCBRPent50" runat="server" CssClass="ac" Width="60px" onkeyup="checkDecimal(this);"
                                                                    MaxLength="10"></asp:TextBox>%
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <br />
                                                                <asp:Label ID="lblCBRtested" runat="server" Text="Final CBR for tested sample"></asp:Label>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txtFinalCBRSample" runat="server" CssClass="ac" Width="160px" onkeyup="checkDecimal(this);"
                                                                    MaxLength="10"></asp:TextBox>%
                                                            </td>
                                                        </tr>
                                                    </table>
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
                                        <div style="height: 10px">
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
                                <asp:CheckBox ID="chkWitnessBy" runat="server" AutoPostBack="true" />
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
                                <asp:LinkButton ID="lnkCalculate" Font-Size="Small" runat="server" Font-Underline="true"
                                    OnClick="lnkCalculate_Click">Calculate</asp:LinkButton>&nbsp;
                                <asp:LinkButton ID="lnkSave" Font-Size="Small" runat="server" Font-Underline="true"
                                    OnClick="lnkSave_Click">Save</asp:LinkButton>&nbsp;
                                <asp:LinkButton ID="lnkPrint" runat="server" Font-Size="Small" Font-Underline="true" Visible="false"
                                      OnClick="lnkPrint_Click">Print</asp:LinkButton>&nbsp;
                                <%--OnClientClick="SoilDataPDFFun()"--%>
                                <asp:LinkButton ID="lnkExit" runat="server" Font-Size="Small" 
                                    Font-Underline="true" onclick="lnkExit_Click">Exit</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <script type="text/javascript">

        function SoilDataPDFFun() {
            window.open("SoilTestPDFReport.aspx");
        };

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
                    alert("Only Numeric Values Allowed");

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
