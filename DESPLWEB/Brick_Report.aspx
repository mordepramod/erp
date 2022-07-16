<%@ Page Language="C#" AutoEventWireup="true" Inherits="DESPLWEB.Brick_Report" MasterPageFile="~/MstPg_Veena.Master"
    CodeBehind="Brick_Report.aspx.cs" Title="Brick Report" Theme="duro" %>

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
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStatus" runat="server" Text="Enter"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRefNo" runat="server" ReadOnly="true" Width="235px"></asp:TextBox>
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtIdMark" runat="server" Width="235px" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRecordNo" runat="server" Text="Report No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRecordType" runat="server" ReadOnly="true" Text="BT-" Width="47px"></asp:TextBox>
                            <asp:TextBox ID="txtReportNo" runat="server" ReadOnly="true" Width="180px"></asp:TextBox>
                            <asp:Label ID="lblrecno" runat="server" Text="" Visible="false"></asp:Label>
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
                            <asp:Label ID="lblTypeOfBrick" runat="server" Text="Type of Brick"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTypeOfBrick" runat="server" ReadOnly="true" Width="234px"></asp:TextBox>
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
                        <asp:Label ID="Label3" runat="server" Text="Nabl Scope"  Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                           <asp:DropDownList ID="ddl_NablScope"  Width="235px" runat="server">
                        <asp:ListItem Text="--Select--" />
                        <asp:ListItem Text="F" />
                        <asp:ListItem Text="NA" />
                    </asp:DropDownList>
                        </td>
                        <td>
                              <asp:Label ID="Label8" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                              <asp:DropDownList ID="ddl_NABLLocation" runat="server" Width="235px" Enabled="true">
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
            <asp:Panel ID="pnlBrickTab" runat="server" ScrollBars="Auto" Height="300px" Width="942px"
                BorderWidth="1px">
              
                <cc1:TabContainer ID="TabContainerBrick" runat="server" Width="100%"
                    ActiveTabIndex="2">
                    <cc1:TabPanel runat="server" HeaderText="Dimension Analysis" ID="TabDA" Width="100%"
                        Visible="false">
                        <ContentTemplate>
                            <table width="100%" height="260px">
                                <tr>
                                    <td align="right" valign="top">Quantity:
                                        <asp:Label ID="lblDAQuantity" runat="server" Text="Label"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        Rows:
                                        <asp:TextBox ID="txtDARows" runat="server" Width="30px" AutoPostBack="True" OnTextChanged="txtDARows_TextChanged"
                                            onkeyup="checkNum(this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlgrdDA" runat="server" Height="220px" ScrollBars="Auto">
                                            <asp:GridView ID="grdDA" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDASrNo" runat="server" ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>"
                                                                BorderWidth="0" CssClass="ac" Width="30px">  </asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID Mark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDAIdMark" runat="server" BorderWidth="0" Width="120px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Length 1">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtL1" runat="server" CssClass="ac" BorderWidth="0" Width="70px"
                                                                onchange="javascript:checkNum(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Length 2">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtL2" runat="server" CssClass="ac" BorderWidth="0" Width="70px"
                                                                onchange="javascript:checkNum(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Length 3">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtL3" runat="server" BorderWidth="0" CssClass="ac" Width="70px"
                                                                onchange="javascript:checkNum(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Width 1">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtW1" runat="server" BorderWidth="0" CssClass="ac" Width="70px"
                                                                onchange="javascript:checkNum(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Width 2">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtW2" runat="server" BorderWidth="0" CssClass="ac" Width="70px"
                                                                onchange="javascript:checkNum(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Width 3">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtW3" runat="server" BorderWidth="0" CssClass="ac" Width="70px"
                                                                onchange="javascript:checkNum(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Height 1">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtH1" runat="server" BorderWidth="0" CssClass="ac" Width="70px"
                                                                onchange="javascript:checkNum(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Height 2">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtH2" runat="server" BorderWidth="0" CssClass="ac" Width="70px"
                                                                onchange="javascript:checkNum(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Height 3">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtH3" runat="server" BorderWidth="0" CssClass="ac" Width="70px"
                                                                onchange="javascript:checkNum(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <div style="height: 10px">
                                                &nbsp;&nbsp;
                                            </div>
                                            <asp:GridView ID="grdDACal" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr. No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDACalSrNo" runat="server" ReadOnly="true" AutoPostBack="true"
                                                                Text=" <%#Container.DataItemIndex+1 %>" CssClass="ac" BorderWidth="0" Width="30px">  </asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID Mark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDACalIdMark" runat="server" ReadOnly="true" BackColor="LightGray"
                                                                BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="100px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Length">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDACalLength" runat="server" ReadOnly="true" BackColor="LightGray"
                                                                BorderWidth="0" CssClass="ac"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="100px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Width">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDACalWidth" runat="server" ReadOnly="true" BackColor="LightGray"
                                                                BorderWidth="0" CssClass="ac"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="100px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Height">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDACalHeight" runat="server" BackColor="LightGray" ReadOnly="true"
                                                                BorderWidth="0" CssClass="ac"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="100px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Avg. Length">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDACalAvgLength" runat="server" ReadOnly="true" BackColor="LightGray"
                                                                BorderWidth="0" CssClass="ac"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="100px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Avg. Width">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDACalAvgWidth" runat="server" ReadOnly="true" BackColor="LightGray"
                                                                BorderWidth="0" CssClass="ac"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="100px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Avg. Height">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDACalAvgHeight" CssClass="ac" runat="server" BackColor="LightGray"
                                                                ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="100px" />
                                                        <ItemStyle HorizontalAlign="Center" />
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
                    <cc1:TabPanel runat="server" HeaderText="Water Absorption" ID="TabWA" Visible="false">
                        <ContentTemplate>
                            <table width="100%" height="260px">
                                <tr>
                                    <td align="right" valign="top">
                                        <asp:Label ID="lblWAQuantity" runat="server" Text="Quantity"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtWAQuantity" runat="server" Width="30px" AutoPostBack="True" OnTextChanged="txtWAQuantity_TextChanged"
                                            onkeyup="checkNum(this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlgrdWA" runat="server" Height="220px" ScrollBars="Auto">
                                            <asp:GridView ID="grdWA" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr. No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWASrNo" CssClass="ac" runat="server" ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>"
                                                                BorderWidth="0">  </asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="50px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID Mark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWAIdMark" runat="server" BackColor="Transparent" BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ControlStyle Width="250px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dry wt.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWADryWt" runat="server" CssClass="ac" onkeyup="checkDecimal(this)"
                                                                BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="120px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Wet wt.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWAWetWt" runat="server" CssClass="ac" onkeyup="checkDecimal(this)"
                                                                BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="120px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Water Absorption (%)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWA" CssClass="ac" runat="server" BackColor="LightGray" onkeyup="checkDecimal(this)"
                                                                ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="150px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Avg. Water Absorption (%)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtWAAvg" CssClass="ac" runat="server" BackColor="LightGray" AutoPostBack="true"
                                                                ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="180px" />
                                                        <ItemStyle HorizontalAlign="Center" />
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
                    <cc1:TabPanel runat="server" HeaderText="Compressive Strength" ID="TabCS" Visible="false">
                        <ContentTemplate>
                            <table width="100%" height="260px">
                                <tr>
                                    <td align="right" valign="top">
                                        <asp:Label ID="lblCSQuantity" runat="server" Text="Quantity"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtCSQuantity" runat="server" Width="30px" AutoPostBack="True" OnTextChanged="txtCSQuantity_TextChanged"
                                            onkeyup="checkNum(this);"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlgrdCS" runat="server" Height="220px" ScrollBars="Auto">
                                            <asp:GridView ID="grdCS" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin"><Columns>
<asp:TemplateField HeaderText="Sr. No."><ItemTemplate>
                                                            <asp:TextBox ID="txtCSSrNo" CssClass="ac" runat="server" ReadOnly="true" BorderWidth="0"
                                                                Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                                        
</ItemTemplate>

<ControlStyle Width="50px" />

<HeaderStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField HeaderText="ID Mark"><ItemTemplate>
                                                            <asp:TextBox ID="txtCSIdMark" runat="server" BorderWidth="0"></asp:TextBox>
                                                        
</ItemTemplate>

<ControlStyle Width="200px" />

<HeaderStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Length"><ItemTemplate>
                                                            <asp:TextBox ID="txtCSLength" runat="server" CssClass="ac" onkeyup="checkNum(this);"
                                                                BorderWidth="0"></asp:TextBox>
                                                        
</ItemTemplate>

<ControlStyle Width="85px" />

<HeaderStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Width"><ItemTemplate>
                                                            <asp:TextBox ID="txtCSWidth" runat="server" CssClass="ac" onkeyup="checkNum(this);"
                                                                BorderWidth="0"></asp:TextBox>
                                                        
</ItemTemplate>

<ControlStyle Width="85px" />

<HeaderStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Load"><ItemTemplate>
                                                            <asp:TextBox ID="txtCSLoad" runat="server" CssClass="ac" onkeyup="checkDecimal(this)"
                                                                BorderWidth="0"></asp:TextBox>
                                                        
</ItemTemplate>

<ControlStyle Width="100px" />

<HeaderStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Area"><ItemTemplate>
                                                            <asp:TextBox ID="txtCSArea" runat="server" CssClass="ac" ReadOnly="true" BorderWidth="0" BackColor="LightGray"></asp:TextBox>
                                                        
</ItemTemplate>

<ControlStyle Width="115px" />

<HeaderStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Strength"><ItemTemplate>
                                                            <asp:TextBox ID="txtCSStrength" runat="server" CssClass="ac" ReadOnly="true" BorderWidth="0" BackColor="LightGray"></asp:TextBox>
                                                        
</ItemTemplate>

<ControlStyle Width="115px" />

<HeaderStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Average"><ItemTemplate>
                                                            <asp:TextBox ID="txtCSAvg" runat="server" CssClass="ac" ReadOnly="true" BorderWidth="0" BackColor="LightGray"></asp:TextBox>
                                                        
</ItemTemplate>

<ControlStyle Width="130px" />

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
                    <cc1:TabPanel runat="server" HeaderText="Efflorescence Test" ID="TabET" Visible="false">
                        <ContentTemplate>
                            <table width="100%" height="260px">
                                <tr>
                                    <td align="right" valign="top">
                                        <asp:Label ID="lblETQuantity" runat="server" Text="Quantity"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtETQuantity" runat="server" Width="30px" AutoPostBack="True" OnTextChanged="txtETQuantity_TextChanged"
                                            onkeyup="checkNum(this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlgrdET" runat="server" Height="220px" ScrollBars="Auto">
                                            <asp:GridView ID="grdET" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtETSrNo" CssClass="ac" runat="server" ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>"
                                                                BorderWidth="0">  </asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="50px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID Mark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtETIdMark" runat="server" BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="300px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Observations">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlETObservations" runat="server" BorderWidth="0">
                                                                <asp:ListItem>Nil</asp:ListItem>
                                                                <asp:ListItem>Slight</asp:ListItem>
                                                                <asp:ListItem>Moderate</asp:ListItem>
                                                                <asp:ListItem>Heavy</asp:ListItem>
                                                                <asp:ListItem>Serious</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="150px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        
</ContentTemplate>
                    
</cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Density" ID="TabDensity" Visible="false">
                        <ContentTemplate>
                            <table width="100%" height="260px">
                                <tr>
                                    <td align="right" valign="top">
                                        <asp:Label ID="lblDSQuantity" runat="server" Text="Quantity"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtDSQuantity" runat="server" Width="30px" AutoPostBack="True" OnTextChanged="txtDSQuantity_TextChanged"
                                            onkeyup="checkNum(this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlgrdDS" runat="server" Height="220px" ScrollBars="Auto">
                                            <asp:GridView ID="grdDS" runat="server" AutoGenerateColumns="False"
                                                SkinID="gridviewSkin">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDSSrNo" CssClass="ac" runat="server" ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>"
                                                                BorderWidth="0">  </asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="50px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID Mark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDSIdMark" runat="server" BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="160px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Length">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDSLength" runat="server" CssClass="ac" onkeyup="checkNum(this);"
                                                                BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="60px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Width">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDSWidth" runat="server" CssClass="ac" onkeyup="checkNum(this);"
                                                                BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="60px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Thickness">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDSThickness" runat="server" CssClass="ac" onkeyup="checkNum(this);"
                                                                BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="80px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Oven dry wt.(Kg)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDSOvenDryWt" runat="server" CssClass="ac" onkeyup="checkDecimal(this)"
                                                                BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="130px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Volume(mm3)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDSVolume" runat="server" CssClass="ac" ReadOnly="true" BackColor="LightGray"
                                                                BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="120px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Density(kg/m3)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDS" runat="server" CssClass="ac" ReadOnly="true" BackColor="LightGray"
                                                                BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="115px" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Avg. Density">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDSAvg" runat="server" CssClass="ac" BackColor="LightGray" ReadOnly="true"
                                                                BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="110px" />
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
                    <cc1:TabPanel runat="server" HeaderText="Remark" ID="TabRemark">
                        <ContentTemplate>
                            <asp:Panel ID="pnlRemark" runat="server" ScrollBars="Auto" Height="260px" Width="100%"
                                BorderWidth="1px">
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
                    <td width="8%"></td>
                    <td width="17%" align="center"></td>
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

        function testDatePicker() {
            $("#TxtDateOfTest").datepicker({ minDate: -10000, maxDate: "+0D" }).focus();
        }
        $(function () {
            $("#TxtDateOfTest").datepicker({ minDate: -10000, maxDate: "+0D" });
        });

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
