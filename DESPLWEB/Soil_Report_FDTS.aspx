<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Soil_Report_FDTS.aspx.cs" Inherits="DESPLWEB.Soil_Report_FDTS" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
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
                            <asp:TextBox ID="txtSampleName" runat="server" Width="235px" ReadOnly="true" MaxLength="250"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRecordNo" runat="server" Text="Report No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRecordType" runat="server" ReadOnly="true" Text="SO" Width="47px"></asp:TextBox>
                            <asp:TextBox ID="txtReportNo" runat="server" ReadOnly="true" Width="179px"></asp:TextBox>
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
            <asp:Panel ID="pnlSoilTab" runat="server" Height="365px" Width="940px" BorderWidth="1px">
                <cc1:TabContainer ID="TabContainerSoil" runat="server" Width="100%" 
                    ActiveTabIndex="1">
                    <cc1:TabPanel runat="server" HeaderText="FDT By Sand" ID="TabFDTS" Width="100%" Visible="false">
                        <ContentTemplate>
                            <table width="98%">
                                <tr>
                                    <td style="width: 147px;">
                                        <asp:Label ID="lblFDTSDateOfTesting" runat="server" Text="Date Of Testing :"></asp:Label>
                                    </td>
                                    <td style="width: 252px">
                                        <asp:TextBox ID="txtFDTSDateOfTesting" runat="server" Width="175px" AutoPostBack="True"
                                            onClick="testDatePicker()" Style="margin-left: 0px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFDTSDateOfTesting">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFDTSDateOfTesting"
                                            MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                    <td align="left" valign="top" style="width: 114px;">
                                        <asp:Label ID="lblFDTSRows" runat="server" Text="Number of Rows:"></asp:Label>
                                    </td>
                                    <td style="width: 52px;">
                                        <asp:TextBox ID="txtFDTSRows" runat="server" Width="44px" AutoPostBack="True" CssClass="ac"
                                            MaxLength="3" BackColor="LightGray" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td style="width: 169px;">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 147px">
                                        <asp:Label ID="lblFDTSSandPouring" runat="server" Text="Sand Pouring Cylinder"></asp:Label>
                                    </td>
                                    <td style="width: 252px">
                                        <asp:DropDownList ID="ddlFDTSCylinder" runat="server" BackColor="Transparent" OnSelectedIndexChanged="ddlFDTSCylinder_SelectedIndexChanged"
                                            AutoPostBack="True" Width="181px">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 114px">
                                        <asp:Label ID="lblFDTSLabMDD" runat="server" Text="LAB MDD:"></asp:Label>
                                    </td>
                                    <td style="width: 52px">
                                        <asp:TextBox ID="txtFDTSLabMDD" runat="server" Width="44px" CssClass="ac" MaxLength="10"
                                            onkeyup="checkDecimal(this)" AutoPostBack="True" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td style="width: 169px">
                                        <asp:Label ID="lblFDTSOMC" runat="server" Text="OMC (%) :"></asp:Label>
                                        <asp:TextBox ID="txtFDTSOMC" runat="server" Width="44px" CssClass="ac" MaxLength="10"
                                            onkeyup="checkDecimal(this)" AutoPostBack="True" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <div style="height: 3px">
                            </div>
                            <asp:Panel ID="pnlFDTS" runat="server" Height="270px" Width="100%" Visible="False"
                                ScrollBars="Auto">
                                <asp:GridView ID="grdFDTS" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin"
                                    OnRowDataBound="grdFDTS_OnRowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSSrNo" CssClass="ac" runat="server" ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>"
                                                    BorderWidth="0" BackColor="LightGray">  </asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="50px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSLocation" runat="server" BorderWidth="0" MaxLength="250"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ControlStyle Width="90px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Initial Weight (g)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSInitialWeight" runat="server" BackColor="LightGray" CssClass="ac"
                                                    ReadOnly="true" MaxLength="10" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="110px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. After Pouring (g)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSWtAfterPouring" runat="server" BorderWidth="0" CssClass="ac"
                                                    MaxLength="10" onkeyup="checkDecimal(this)"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="135px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sand in Cone + Hole (g)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSSandInConeHole" runat="server" BackColor="LightGray" CssClass="ac"
                                                    ReadOnly="true" MaxLength="10" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="150px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sand in Cone (g)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSSandInCone" runat="server" CssClass="ac" BorderWidth="0"
                                                    MaxLength="10" onkeyup="checkDecimal(this)"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="95px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sand in Hole (g)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSSandInHole" runat="server" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="95px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bulk Density of Sand (g/cc)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSBulkDensity" runat="server" CssClass="ac" BorderWidth="0"
                                                    MaxLength="10" onkeyup="checkDecimal(this)"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="150px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vol. of Hole (C.C)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSVolHole" runat="server" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Wet Soil Samples from Hole">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSWetSoilSamples" runat="server" CssClass="ac" BorderWidth="0"
                                                    MaxLength="10" onkeyup="checkDecimal(this)"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="150px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wet Density (g/cc)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSWetDensity" runat="server" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Initial Wt. Of container + Sample">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSInitialcontainerSample" runat="server" CssClass="ac" MaxLength="10"
                                                    onkeyup="checkDecimal(this)" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="150px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. Of Container + Sample after drying">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSContainerSample" runat="server" CssClass="ac" BorderWidth="0"
                                                    MaxLength="10" onkeyup="checkDecimal(this)"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="150px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Container No.">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlFDTSContainerNo" runat="server" BackColor="Transparent"
                                                    BorderWidth="0" > <%--OnSelectedIndexChanged="ddlFDTSContainerNo_SelectedIndexChanged"--%>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Container (gm)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSWtofContainer" runat="server" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="120px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WC %">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSWC" runat="server" CssClass="ac" BackColor="LightGray" ReadOnly="true"
                                                    BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dry Density (g/cc)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSDryDensity" runat="server" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Compaction (%)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSCompaction" runat="server" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Depth of Hole (cm)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTSDepthOfHole" runat="server" CssClass="ac" BorderWidth="0"
                                                    MaxLength="10" onkeyup="checkDecimal(this)"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="110px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" Height="38px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="FDT By Core Cutter" ID="TabFDTCoreCutter"
                        Width="100%" Visible="false">
                        <ContentTemplate>
                            <table width="98%">
                                <tr>
                                    <td style="width: 130px;">
                                        <asp:Label ID="Label1" runat="server" Text="Date Of Testing :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFDTCCDateOfTesting" runat="server" Width="130px" AutoPostBack="True"
                                            onClick="testDatePicker()"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFDTCCDateOfTesting">
                                        </cc1:CalendarExtender>
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtFDTCCDateOfTesting"
                                            MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:Label ID="lblFDTCCRows" runat="server" Text="Number of Rows:"></asp:Label>
                                        <asp:TextBox ID="txtFDTCCRows" runat="server" Width="44px" AutoPostBack="True" BackColor="LightGray"
                                            ReadOnly="True" CssClass="ac" MaxLength="3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 130px">
                                        <asp:Label ID="lblmaxdrydensity" runat="server" Text="Max dry density :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMaxDryDensity" runat="server" CssClass="ac" MaxLength="10" 
                                            onkeyup="checkDecimal(this)" Width="44px" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOptimumMoisture" runat="server" Text="Optimum Moisture Content (%):"></asp:Label>
                                        <asp:TextBox ID="txtOptimumMoisture" runat="server" Width="44px"
                                            onkeyup="checkDecimal(this)" BackColor="LightGray" CssClass="ac" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="height: 3px">
                            </div>
                            <asp:Panel ID="pnlFDTCoreCutter" runat="server" Height="270px" Width="100%" ScrollBars="Auto"
                                Visible="False">
                                <asp:GridView ID="grdFDTCC" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin"
                                    OnRowDataBound="grdFDTCC_OnRowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr. No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCSrNo" CssClass="ac" runat="server" ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>"
                                                    BorderWidth="0" BackColor="LightGray">  </asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="50px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCLocation" runat="server" BorderWidth="0" MaxLength="250"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ControlStyle Width="90px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Determination No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCDeterminationNo" runat="server" MaxLength="10" CssClass="ac"
                                                    onkeyup="checkDecimal(this)" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="95px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Core-Cutter + Wet Soil">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCWtofCoreCutterWetSoil" runat="server" BorderWidth="0" CssClass="ac"
                                                    ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="150px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Core Cutter No.">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlFDTCCCoreCutterNo" runat="server" BackColor="Transparent"
                                                     BorderWidth="0" > <%--OnSelectedIndexChanged="ddlFDTCCCoreCutterNo_SelectedIndexChanged"--%>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Core-Cutter">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCWtofCoreCutter" runat="server" CssClass="ac" BorderWidth="0"
                                                    BackColor="LightGray" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Wet Soil">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCWtofWetSoil" runat="server" CssClass="ac" BorderWidth="0"
                                                    onkeyup="checkDecimal(this)" MaxLength="10"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Volume of Core Cutter">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCVolCoreCutter" runat="server" CssClass="ac" BorderWidth="0"
                                                    onkeyup="checkDecimal(this)" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bulk Density">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCBulkDensity" runat="server" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Container No.">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlFDTCCContainerNo" runat="server" BackColor="Transparent"
                                                     BorderWidth="0"> <%--OnSelectedIndexChanged="ddlFDTCCContainerNo_SelectedIndexChanged"--%>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Container with Wet Soil Sample">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCWtContainerWetSoilSample" runat="server" CssClass="ac" onkeyup="checkDecimal(this)"
                                                    MaxLength="10" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="155px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Container with Dry Soil">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCWtofContainerDrySoil" runat="server" CssClass="ac" onkeyup="checkDecimal(this)"
                                                    MaxLength="10" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="150px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Weight of Container">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCWeightofContainer" runat="server" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Moisture Content">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCMoistureContent" runat="server" CssClass="ac" BorderWidth="0"
                                                    ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dry Density">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCDryDensity" runat="server" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="% of Compaction">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFDTCCCompaction" runat="server" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" Height="38px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Remark" ID="TabRemark">
                        <ContentTemplate>
                            <div style="height: 10px">
                            </div>
                            <asp:Panel ID="pnlRemark" runat="server" ScrollBars="Auto" Height="317px" Width="100%"
                                BorderWidth="0px">
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
                        <asp:LinkButton ID="lnkCalculate" Font-Size="Small" runat="server" Font-Underline="true"
                            OnClick="lnkCalculate_Click">Calculate</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkSave" Font-Size="Small" runat="server" Font-Underline="true"
                            OnClick="lnkSave_Click">Save</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Size="Small" Font-Underline="true"  Visible="false"
                             onclick="lnkPrint_Click">Print</asp:LinkButton>&nbsp;  <%--OnClientClick="SoilDataPDFFun()"--%>
                        <asp:LinkButton ID="lnkExit" runat="server" Font-Size="Small" Font-Underline="true"
                            OnClick="lnkExit_Click">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
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
