<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tile_Report.aspx.cs" Inherits="DESPLWEB.Tile_Report"
    MasterPageFile="~/MstPg_Veena.Master" Theme="duro" %>

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
            <asp:Panel ID="pnlDetails" runat="server" Width="942px" BorderWidth="1px" BorderColor="DimGray">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 97px">
                            <asp:Label ID="lblRefNo" runat="server" Text="Reference No"></asp:Label>
                        </td>
                        <td style="width: 305px">
                            <asp:DropDownList ID="ddlRefNo" runat="server" AutoPostBack="true" Width="240px">
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkFetch" runat="server" OnClick="lnkFetch_Click">Fetch</asp:LinkButton>
                        </td>
                        <td style="width: 113px">
                            <asp:Label ID="lblSupplierName" runat="server" Text="Supplier Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSupplierName" runat="server" Width="235px"></asp:TextBox>
                            <asp:TextBox ID="txt_Rectype" runat="server" Visible="false"></asp:TextBox>
                            <asp:Label ID="lblRecordNo" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStatus" runat="server" Text="Enter"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRefNo" runat="server" ReadOnly="true" Width="235px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblDesc" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDesc" runat="server" Width="235px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTileType" runat="server" Text="Tile Type"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTileType" runat="server" Width="240px">
                                <asp:ListItem>---Select---</asp:ListItem>
                                <asp:ListItem>Ceramic</asp:ListItem>
                                <asp:ListItem>Chequered</asp:ListItem>
                                <asp:ListItem>Mosaic</asp:ListItem>
                                <asp:ListItem>Vitrified</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblDateOfTesting" runat="server" Text="Date Of Testing"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateOfTest" runat="server" Width="235px" AutoPostBack="true"
                                onClick="testDatePicker()"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtDateOfTest">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtDateOfTest"
                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                            </cc1:MaskedEditExtender>
                        </td>
                    </tr>
                      <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="NABL Scope" Font-Bold="true"></asp:Label>
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
                            <asp:DropDownList ID="ddl_NABLLocation" runat="server"  Width="241px" Enabled="true">
                                <asp:ListItem Value="0" Text="0" />
                                <asp:ListItem Value="1" Text="1" />
                                <asp:ListItem Value="2" Text="2" />
                            </asp:DropDownList>
                            <asp:LinkButton ID="lnkGetData" OnClick="lnkGetAppData_Click" runat="server" Font-Bold="True"
                                        Style="text-decoration: underline;">Get App Data</asp:LinkButton>&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="pnlTileTab" runat="server" Height="340px" Width="942px" BorderColor="DimGray"
                BorderWidth="1px">

               

                <cc1:TabContainer ID="TabContainerTile" runat="server" Width="942px" ActiveTabIndex="4"
                    Height="280px">
                    <cc1:TabPanel runat="server" HeaderText="Crazing Resistance" ID="TabCR" Width="100%">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td valign="top">
                                        <div style="margin-left: 790px">
                                            <asp:Label ID="lblQuantity" runat="server" Text="Quantity"></asp:Label>
                                            &nbsp;
                                            <asp:TextBox ID="txtQuantity" runat="server" AutoPostBack="True" onkeyup="checkDecimal(this);"
                                                CssClass="ac" MaxLength="3" OnTextChanged="txtQuantity_TextChanged"
                                                Width="30px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Panel ID="pnlTileCR" runat="server" ScrollBars="Auto" Height="270px">
                                            <asp:GridView ID="grdCR" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                                Width="97%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSrNo" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                                Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="70px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID Mark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtIdMark" runat="server" BorderWidth="0" Width="300px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="505px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Crazing">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlCrazing" runat="server" BorderWidth="0" Width="200px">
                                                                <asp:ListItem>---Select---</asp:ListItem>
                                                                <asp:ListItem>Observed</asp:ListItem>
                                                                <asp:ListItem>Not Observed</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="300px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Scratch Hardness" ID="TabSH" Width="100%">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td valign="top">
                                        <div style="margin-left: 790px">
                                            <asp:Label ID="lblSHQty" runat="server" Text="Quantity"></asp:Label>
                                            &nbsp;
                                            <asp:TextBox ID="txtSHQuantity" runat="server" AutoPostBack="True" onkeyup="checkDecimal(this);"
                                                CssClass="ac" MaxLength="3" OnTextChanged="txtSHQuantity_TextChanged"
                                                Width="30px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Panel ID="pnlSH" runat="server" ScrollBars="Auto" Height="270px">
                                            <asp:GridView ID="grdSH" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                                Width="97%">
                                                <Columns>
                                                    <asp:TemplateField
                                                        HeaderText="Sr No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSHSrNo" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                                Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="70px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID Mark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSHIdMark" runat="server" BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="430px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Scratch Hardness">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtHardness" runat="server" BorderWidth="0" CssClass="ac"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="390px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Water Absorption" ID="TabWA" Width="100%">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td valign="top">
                                        <div style="margin-left: 790px">
                                            <asp:Label ID="lblWAQty" runat="server" Text="Quantity"></asp:Label>
                                            &nbsp;
                                            <asp:TextBox ID="txtWAQuantity" runat="server" AutoPostBack="True" onkeyup="checkDecimal(this);"
                                                CssClass="ac" MaxLength="3" OnTextChanged="txtWAQuantity_TextChanged"
                                                Width="30px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="270px">
                                            <asp:GridView ID="grdWA" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                                Width="97%">
                                                <Columns>
                                                    <asp:TemplateField
                                                        HeaderText="Sr No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWASrNo" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                                Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="50px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID Mark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWAIdMark" runat="server" BorderWidth="0" Width="200px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="200px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dry Weight">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWADryWt" runat="server" BorderWidth="0" CssClass="ac" Width="120px"
                                                                onkeyup="checkDecimal(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="120px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Wet Weight">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWAWetWt" runat="server" BorderWidth="0" CssClass="ac" Width="120px"
                                                                onkeyup="checkDecimal(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="120px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Water Absorption">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWA" runat="server" BackColor="LightGray" BorderWidth="0" CssClass="ac"
                                                                Width="150px" ReadOnly="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Avg. Water Absorption(%)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWAAvg" runat="server" BackColor="LightGray" BorderWidth="0" CssClass="ac"
                                                                Width="250px" ReadOnly="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="240px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Wet Transverse" ID="TabWT" Width="100%">
                        <ContentTemplate>
                            <table width="100%" style="height: 56px">
                                <tr>
                                    <td style="width: 123px; padding-bottom: 1px; padding-top: 0px;">
                                        <asp:Label ID="lblWTQty" runat="server" Text="Quantity"></asp:Label>
                                        &nbsp;
                                        <asp:TextBox ID="txtWTQuantity" runat="server" AutoPostBack="True" onkeyup="checkDecimal(this);"
                                            CssClass="ac" MaxLength="3" OnTextChanged="txtWTQuantity_TextChanged" Width="30px"></asp:TextBox>
                                    </td>
                                    <td style="width: 279px; padding-bottom: 1px; padding-top: 0px;">
                                        <asp:Label ID="lblL" runat="server" Text="L ="></asp:Label>&nbsp;
                                        <asp:TextBox ID="txtL" runat="server" onkeyup="checkDecimal(this);" Width="66px"
                                            MaxLength="10" CssClass="ac"></asp:TextBox>&nbsp;
                                        <asp:Label ID="lblmm1" runat="server" Text="mm"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblB" runat="server" Text="B ="></asp:Label>&nbsp;
                                        <asp:TextBox ID="txtB" runat="server" onkeyup="checkDecimal(this);" Width="66px"
                                            MaxLength="10" CssClass="ac"></asp:TextBox>&nbsp;<asp:Label ID="lblmm2" runat="server"
                                                Text="mm"></asp:Label>
                                    </td>
                                    <td style="width: 227px; padding-bottom: 1px; padding-top: 0px;">
                                        <asp:RadioButtonList ID="RdbMachine" runat="server" RepeatColumns="1" AutoPostBack="True"
                                            OnSelectedIndexChanged="RdbMachine_SelectedIndexChanged" CssClass="mylist" Width="302px">
                                            <asp:ListItem>Mechanical Flexture Machine</asp:ListItem>
                                            <asp:ListItem>Digital Flexture Machine</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <div style="height: 5px;">
                            </div>
                            <asp:Panel ID="pnlWT" runat="server" ScrollBars="Auto" Height="230px">
                                <asp:GridView ID="grdWT" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                    Width="97%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWTSrNo" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                    Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="60px" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID Mark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWTIdMark" runat="server" BorderWidth="0" Width="400px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="205px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T1">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtT1" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T2">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtT2" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T3">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtT3" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T4">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtT4" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T5">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtT5" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T6">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtT6" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Lead Shots (k.g)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWtLeadShots" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="150px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="120px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:GridView>
                                <div style="height: 8px">
                                    &nbsp;&nbsp;
                                </div>
                                <asp:GridView ID="grdWTCal" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                    Width="97%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWTCalSrNo" runat="server" BorderWidth="0" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="60px" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID Mark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWTCalIdMark" runat="server" BorderWidth="0" Width="130px" ReadOnly="true"
                                                    BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="130px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Thickness">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWTThickness" runat="server" BorderWidth="0" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" Width="105px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="110px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Lead Shots (k.g)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWtCalLeadShots" runat="server" BorderWidth="0" CssClass="ac"
                                                    BackColor="LightGray" ReadOnly="true" Width="120px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="120px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="B (k.g)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWtB" runat="server" BorderWidth="0" CssClass="ac" Width="80px"
                                                    BackColor="LightGray" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="N (P)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWtN" runat="server" BorderWidth="0" CssClass="ac" Width="80px"
                                                    BackColor="LightGray" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Transverse Str (3 PL/2BTT)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWtTransverse" runat="server" BorderWidth="0" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" Width="110px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="110px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wet Tr.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWetTr" runat="server" BorderWidth="0" CssClass="ac" Width="90px"
                                                    BackColor="LightGray" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Avg. Wet Tr.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAvgWetTr" runat="server" BorderWidth="0" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" Width="90px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="90px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Modulus Of Rupture" ID="TabMR" Width="100%">
                        <ContentTemplate>
                            <table width="100%" style="height: 70px">
                                <tr>
                                    <td style="width: 168px">
                                        <asp:Label ID="lblDiameter" runat="server" Text="Diameter of Rod :"></asp:Label>
                                    </td>
                                    <td style="width: 159px">
                                        <asp:TextBox ID="txtDiameterOfRod" runat="server" onkeyup="checkDecimal(this);" Width="50px"
                                            MaxLength="10" CssClass="ac" Height="15px"></asp:TextBox>&nbsp;<asp:Label ID="Label7"
                                                runat="server" Text="mm"></asp:Label>
                                    </td>
                                    <td style="width: 70px">
                                        <asp:Label ID="Label2" runat="server" Text="L :"></asp:Label>
                                    </td>
                                    <td style="width: 170px">
                                        <asp:TextBox ID="txtMORL" runat="server" onkeyup="checkDecimal(this);" Width="50px"
                                            Height="15px" CssClass="ac" MaxLength="10"></asp:TextBox>
                                        &nbsp;<asp:Label ID="Label3" runat="server" Text="mm"></asp:Label>
                                    </td>
                                    <td rowspan="3" style="width: 250px">
                                        <asp:RadioButtonList ID="RdbMRMachine" runat="server" RepeatColumns="1" AutoPostBack="True"
                                            OnSelectedIndexChanged="RdbMRMachine_SelectedIndexChanged" CssClass="mylist"
                                            Width="222px">
                                            <asp:ListItem>Mechanical Flexture Machine</asp:ListItem>
                                            <asp:ListItem>Digital Flexture Machine</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 168px">
                                        <asp:Label ID="lblThickness" runat="server" Text="Thickness of Rubber :"></asp:Label>
                                    </td>
                                    <td style="width: 159px">
                                        <asp:TextBox ID="txtThicknessOfRubber" runat="server" onkeyup="checkDecimal(this);"
                                            Width="50px" CssClass="ac" Height="15px" MaxLength="10"></asp:TextBox>&nbsp;<asp:Label
                                                ID="Label9" runat="server" Text="mm"></asp:Label>
                                    </td>
                                    <td style="width: 70px">
                                        <asp:Label ID="Label4" runat="server" Text="B :"></asp:Label>
                                    </td>
                                    <td style="width: 170px">
                                        <asp:TextBox ID="txtMORB" runat="server" onkeyup="checkDecimal(this);" Width="50px"
                                            Height="15px" CssClass="ac" MaxLength="10"></asp:TextBox>
                                        &nbsp;<asp:Label ID="Label5" runat="server" Text="mm"></asp:Label>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="width: 168px">
                                        <asp:Label ID="lblOverlap" runat="server" Text="Overlap Beyond Edge (l):"></asp:Label>
                                    </td>
                                    <td style="width: 159px">
                                        <asp:TextBox ID="txtOverlapBeyondEdge" runat="server" onkeyup="checkDecimal(this);"
                                            Width="50px" MaxLength="10" CssClass="ac" Height="15px"></asp:TextBox>&nbsp;<asp:Label
                                                ID="Label8" runat="server" Text="mm"></asp:Label>
                                    </td>
                                    <td style="width: 70px">
                                        <asp:Label ID="lblMRQuantity" runat="server" Text="Quantity :"></asp:Label>
                                    </td>
                                    <td style="width: 170px">
                                        <asp:TextBox ID="txtMRQuantity" runat="server" AutoPostBack="True" onkeyup="checkDecimal(this);"
                                            MaxLength="3" CssClass="ac" OnTextChanged="txtMRQuantity_TextChanged" Width="50px"
                                            Height="15px"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                            <br />
                            <asp:Panel ID="pnlMR" runat="server" ScrollBars="Auto" Height="180px">
                                <asp:GridView ID="grdMR" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMRSrNo" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                    Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="50px" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID Mark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMRIdMark" runat="server" BorderWidth="0" Width="150px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="98%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T1">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtT1" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="60px"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ControlStyle Width="98%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T2">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtT2" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="60px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ControlStyle Width="98%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T3">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtT3" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="60px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ControlStyle Width="98%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T4">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtT4" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="60px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ControlStyle Width="98%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T5">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtT5" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="60px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ControlStyle Width="98%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="T6">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtT6" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="40px"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ControlStyle Width="90%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Lead Shots (k.g)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMRLeadShots" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                    Width="80%"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="98%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:GridView>
                                <div style="height: 8px">
                                    &nbsp;&nbsp;
                                </div>
                                <asp:GridView ID="grdMRCal" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMRCalSrNo" runat="server" BorderWidth="0" CssClass="ac" BackColor="LightGray"
                                                    ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="50px" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID Mark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMRCalIdMark" runat="server" BorderWidth="0" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="85px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Thickness">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMRThickness" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                    BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="77px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Lead Shots (k.g)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMRCalLeadShots" runat="server" BorderWidth="0" CssClass="ac"
                                                    ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="85px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="B (k.g)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMRB" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                    BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="57px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="N (P)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMRN" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                    BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="57px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Avg. Breaking Load">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAvgBreakLoad" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                    BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="75px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Breaking Strength">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBreakStrength" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                    BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Avg. Breaking Strength">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAvgBreakStrength" runat="server" BorderWidth="0" CssClass="ac"
                                                    ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="75px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Transverse Str (3 PL/2BTT)">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMRTransverse" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                    BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="85px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MOR">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMOR" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                    BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="70px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Avg. MOR">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAvgMOR" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                    BackColor="LightGray"></asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="70px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Dimension Analysis" ID="TabDA" Width="942px">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td valign="top">
                                        <div style="margin-left: 790px">
                                            <asp:Label ID="lblDAQty" runat="server" Text="Quantity"></asp:Label>
                                            &nbsp;
                                            <asp:TextBox ID="txtDAQuantity" runat="server" AutoPostBack="True" onkeyup="checkDecimal(this);"
                                                CssClass="ac" MaxLength="3" OnTextChanged="txtDAQuantity_TextChanged" Width="30px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Panel ID="pnlDA" runat="server" ScrollBars="Auto" Height="250px">
                                            <asp:GridView ID="grdDA" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                                Width="97%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDASrNo" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                                Text=" <%#Container.DataItemIndex+1 %>" Width="29px">  </asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID Mark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDAIdMark" runat="server" BorderWidth="0" Width="80px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="L1">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtL1" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                                Width="40px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" BackColor="LightCoral" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="L2">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtL2" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                                Width="40px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" BackColor="LightCoral" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="L3">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtL3" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                                Width="40px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" BackColor="LightCoral" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="W1">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtW1" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                                Width="40px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" BackColor="Green" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="W2">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtW2" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                                Width="40px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" BackColor="Green" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="W3">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtW3" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                                Width="40px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" BackColor="Green" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="T1">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtT1" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                                Width="40px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" BackColor="LightSeaGreen" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="T2">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtT2" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                                Width="40px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" BackColor="LightSeaGreen" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="T3">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtT3" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                                Width="40px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" BackColor="LightSeaGreen" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="T4">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtT4" runat="server" BorderWidth="0" CssClass="ac" onkeyup="checkDecimal(this);"
                                                                Width="40px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" BackColor="LightSeaGreen" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <div style="height: 10px">
                                                &nbsp;&nbsp;
                                            </div>
                                            <asp:GridView ID="grdDACal" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                                Width="97%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDACalSrNo" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                                Text=" <%#Container.DataItemIndex+1 %>" BackColor="LightGray">  </asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="50px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID Mark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDACalIdMark" runat="server" BorderWidth="0" Width="210px" ReadOnly="true"
                                                                BackColor="LightGray"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="210px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Length">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLength" runat="server" BorderWidth="0" ReadOnly="true" CssClass="ac"
                                                                Width="100px" BackColor="LightGray"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Width">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWidth" runat="server" BorderWidth="0" ReadOnly="true" CssClass="ac"
                                                                Width="100px" BackColor="LightGray"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Thickness">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtThickness" runat="server" BorderWidth="0" ReadOnly="true" CssClass="ac"
                                                                BackColor="LightGray" Width="110px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="110px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Avg. Length">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAvgLen" runat="server" BackColor="LightGray" BorderWidth="0"
                                                                ReadOnly="true" CssClass="ac" Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Avg. Width">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAvgWidth" runat="server" BackColor="LightGray" BorderWidth="0"
                                                                ReadOnly="true" CssClass="ac" Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Avg. Thickness">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAvgThickness" runat="server" BackColor="LightGray" BorderWidth="0"
                                                                ReadOnly="true" CssClass="ac" Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="100px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Remark" ID="TabRemark" Width="100%">
                        <ContentTemplate>
                            <asp:Panel ID="pnlRemark" runat="server" ScrollBars="Auto" Height="290px">
                                <asp:GridView ID="grdRemark" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                    Width="85%">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnAddRow" runat="server" ImageAlign="Middle" ImageUrl="~/Images/AddNewitem.jpg"
                                                    OnClick="imgBtnAddRow_Click" Width="18px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnDeleteRow" runat="server" ImageAlign="Middle" ImageUrl="~/Images/DeleteItem.png"
                                                    OnClick="imgBtnDeleteRow_Click" Width="16px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarkSrNo" runat="server" AutoPostBack="true" BorderWidth="0"
                                                    CssClass="ac" ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="50px" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemark" runat="server" BorderWidth="0px" Width="800"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Specified Limits" ID="TabSpecifiedLimit"
                        Width="100%">
                        <ContentTemplate>
                            <div style="width: 100%">
                                <asp:Label ID="lblSpecifiedLimits" runat="server" Text="Specified Limits :"></asp:Label>
                                <asp:CheckBox ID="ChkSpecifiedLimits" runat="server" Width="18px" Height="16px" />
                                <asp:Label ID="lblapplyonwords" runat="server" Font-Bold="True" Text="Apply Onwards"></asp:Label>
                                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                <asp:Label ID="lblIsCode" runat="server" Text="IS Code :" Visible="False"></asp:Label>
                                <asp:TextBox ID="txtIsCode" runat="server" Width="100px" Visible="False"></asp:TextBox>
                            </div>
                            <br />
                            <asp:Panel ID="pnlSpecLimit" runat="server" ScrollBars="Auto" Height="260px">
                                <asp:GridView ID="grdSpecifiedLimit" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                    Width="97%">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnAddSpecifiedRow" runat="server" ImageAlign="Middle" ImageUrl="~/Images/AddNewitem.jpg"
                                                    OnClick="imgBtnAddSpecifiedRow_Click" Width="18px" />
                                            </ItemTemplate>
                                            <ControlStyle Width="18px" />
                                            <ItemStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnDeleteSpecifiedRow" runat="server" ImageAlign="Middle"
                                                    ImageUrl="~/Images/DeleteItem.png" OnClick="imgBtnDeleteSpecifiedRow_Click" Width="16px" />
                                            </ItemTemplate>
                                            <ControlStyle Width="16px" />
                                            <ItemStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSpeciDescription" runat="server" BorderWidth="0px" Visible="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ControlStyle Width="400px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Limits">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSpeciLimits" runat="server" CssClass="ac" BorderWidth="0px"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle VerticalAlign="Middle" />
                                            <ControlStyle Width="250px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Length">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSpeciLength" runat="server" BorderWidth="0px" CssClass="ac"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle VerticalAlign="Middle" />
                                            <ControlStyle Width="110px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Width">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSpeciWidth" runat="server" BorderWidth="0px" CssClass="ac"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ControlStyle Width="110px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Thickness">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSpeciThickness" runat="server" BorderWidth="0px" CssClass="ac"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ControlStyle Width="110px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </asp:Panel>
            <div style="height: 6px">
                &nbsp;&nbsp; &nbsp;&nbsp;
            </div>
            <table width="99%">
                <tr>
                    <td width="10%">
                        <asp:CheckBox ID="chkWitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chkWitnessBy_CheckedChanged" />
                        <%--  --%>
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
                        <asp:Label ID="lblEntdChkdBy" runat="server" Text="Checked By"></asp:Label>
                    </td>
                    <td width="17%" align="center">
                        <asp:TextBox ID="txtEntdChkdBy" runat="server" Width="140px" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="30%" align="right">
                        <asp:LinkButton ID="lnkCalculate" Font-Size="Small" runat="server" OnClick="lnkCalculate_Click"
                            Font-Underline="true">Calculate</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkSave" Font-Size="Small" runat="server" OnClick="lnkSave_Click"
                            Font-Underline="true">Save</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Size="Small" OnClick="lnkPrint_Click"
                            Font-Underline="true" Visible="false">Print</asp:LinkButton>&nbsp;
                        <%-- OnClientClick="myDataPDFFun()"--%>
                        <asp:LinkButton ID="lnkExit" runat="server" Font-Size="Small" Font-Underline="true"
                            OnClick="lnkExit_Click">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <script type="text/javascript">

        function myDataPDFFun() {
            window.open("TileReportPDF.aspx");
        };


        function testDatePicker() {
            $("#TxtDateOfTest").datepicker({ minDate: -10000, maxDate: "+0D" }).focus();
        }
        $(function () {
            $("#TxtDateOfTest").datepicker({ minDate: -10000, maxDate: "+0D" });
        });


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
