<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    Theme="duro" CodeBehind="GeneralSettings.aspx.cs" Inherits="DESPLWEB.GeneralSettings"
    EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">



        function testDatePicker() {
            $("#txt_From_Date").datepicker({ minDate: -10000, maxDate: "+0D" }).focus();
        }
        $(function () {
            $("#txt_From_Date").datepicker({ minDate: -10000, maxDate: "+0D" });
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
                    alert("Only Numeric Values Allowed");

                    return false;
                }

            }
            return true;
        }   

    </script>
    <div style="height: 480px; margin: 0 auto; width: 97%; padding: 14px; border: solid 1px #b7ddf2;
        background: #ebf4fb;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="1px" Height="480px"
            BorderColor="AliceBlue" Style="background-color: #ECF5FF;">
            <asp:Panel ID="panel2" runat="server" BorderWidth="1px" Height="45px" BorderColor="ActiveBorder"
                BackColor="#9bcaec" Width="100%">
                <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="RdbGenericList"
                    AutoPostBack="true" OnTextChanged="RdbGenericList_OnTextChanged" Width="100%"
                    CssClass="RdbList" Font-Bold="true" Height="45px" RepeatLayout="Flow">
                    <asp:ListItem Text="Service Tax"></asp:ListItem>
                    <asp:ListItem Text="Change Rate"></asp:ListItem>
                    <asp:ListItem Text="Calibration Note"></asp:ListItem>
                    <asp:ListItem Text="Bypass credit limit checking"></asp:ListItem>
                    <asp:ListItem Text="Soil Settings"></asp:ListItem>
                    <asp:ListItem Text="Unlock Coupons"></asp:ListItem>
                    <asp:ListItem Text="Machine Settings"></asp:ListItem>
                    <asp:ListItem Text="Payment To Settings"></asp:ListItem>
                </asp:RadioButtonList>
            </asp:Panel>
            <asp:Panel ID="pnlSoilSettings" runat="server" BorderWidth="0px" Height="40px" Visible="false"
                BackColor="#9bcaec" Width="100%">
                <div style="margin-top: 4px;">
                    <asp:Panel ID="Panel1" runat="server" Width="100%" BorderWidth="1px" Height="40px"
                        BorderColor="ActiveBorder">
                        <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="RadioButtonList1"
                            CssClass="RdbList" Font-Bold="true" OnTextChanged="RadioButtonList1_OnTextChanged"
                            AutoPostBack="true" Width="83%" Height="40px" RepeatLayout="Flow">
                            <asp:ListItem Text="Weight Of Container"></asp:ListItem>
                            <asp:ListItem Text="Mould Volume"></asp:ListItem>
                            <asp:ListItem Text="CBR Proving Ring"></asp:ListItem>
                            <asp:ListItem Text="Core Cutter Dimensions"></asp:ListItem>
                            <asp:ListItem Text="Calibration of Sand / Cylinder Cone"></asp:ListItem>
                            <asp:ListItem Text="Weight of empty Dish"></asp:ListItem>
                            <asp:ListItem Text="Weight of empty Bottle"></asp:ListItem>
                            <asp:ListItem Text="Weight of empty Pycnometer"></asp:ListItem>
                        </asp:RadioButtonList>
                         <asp:Label ID="lblMouldRowCount" runat="server" Text="" Visible="false" ></asp:Label>
                         <asp:Label ID="lblCoreCutterRowCount" runat="server" Text="" Visible="false" ></asp:Label>
                         <asp:Label ID="lblrowcount" runat="server" Text="" Visible="false" ></asp:Label>
                         <asp:Label ID="lblMouldCBRRowCount" runat="server" Text="" Visible="false" ></asp:Label>
                         <asp:Label ID="lblRowCBRCount" runat="server" Text="" Visible="false" ></asp:Label>
                         <asp:Label ID="lblRowMachineCount" runat="server" Text="" Visible="false" ></asp:Label>
                    </asp:Panel>
                </div>
                <div style="margin-left: 775px; margin-bottom: 7px; margin-top: 6px; height: 20px;">
                    <asp:Label ID="Lbl_From_Date" Font-Bold="true" runat="server" Text="From Date :"></asp:Label>
                    <asp:TextBox ID="txt_From_Date" runat="server" Width="75px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                        Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_From_Date">
                    </asp:CalendarExtender>
                    <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_From_Date"
                        MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                        CultureTimePlaceholder="">
                    </asp:MaskedEditExtender>
                </div>
                <asp:Panel ID="PanelWeightOfContainer" runat="server" Width="100%" ScrollBars="Auto"
                    BorderStyle="Ridge" BorderColor="LightGray" Visible="false" BorderWidth="0px"
                    Height="310px">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdViewSettings" runat="server" Height="100%" Width="25%" AutoGenerateColumns="false"
                                SkinID="gridviewSkin1">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnAddRow" runat="server" OnClick="imgBtnAddRow_Click" ImageUrl="Images/AddNewitem.jpg"
                                                Height="18px" Width="18px" CausesValidation="false" ToolTip="Add Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="18px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnDeleteRow" runat="server" OnClick="imgBtnDeleteRow_Click"
                                                ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                ToolTip="Delete Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Container No" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtContainerNo" CssClass="ac" runat="server" Width="250px"
                                               MaxLength="50"  onkeyup="checkNum(this);" BorderWidth="0px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Wt of Container + Lid" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWtContainer" CssClass="ac" runat="server" BorderWidth="0px" 
                                              MaxLength="50"  onkeyup="checkDecimal(this);" Width="250px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:Panel ID="OptionsPanel" runat="server" Visible="false">
                    <table border="1px" style="margin-bottom: 7px;">
                        <tr>
                            <td>
                                <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="RdbMouldVolume1"
                                    OnTextChanged="RdbMouldVolume1_OnTextChanged" AutoPostBack="true">
                                    <asp:ListItem Text="MDD"></asp:ListItem>
                                    <asp:ListItem Text="CBR"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="RdbMouldVolume2"
                                    OnTextChanged="RdbMouldVolume2_OnTextChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Standard"></asp:ListItem>
                                    <asp:ListItem Text="Modified"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelMouldVolume" runat="server" Width="100%" ScrollBars="Auto" BorderWidth="0px"
                    BorderStyle="Ridge" BorderColor="LightGray" Visible="false" Height="277px">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GrdViewMould" runat="server" Height="100%" Width="60%" AutoGenerateColumns="false"
                                SkinID="gridviewSkin1">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="AddRowForMould" runat="server" OnClick="AddRowForMould_Click"
                                                ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                ToolTip="Add Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="18px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="DeleteRowForMould" runat="server" OnClick="DeleteRowForMould_Click"
                                                ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                ToolTip="Delete Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mould No" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMouldNo" CssClass="ac" runat="server" onkeyup="checkNum(this);" MaxLength="50"
                                                 BorderWidth="0px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ControlStyle Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Volume" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="txtVolume" runat="server" Width="100px">
                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                                <asp:ListItem Text="1000"></asp:ListItem>
                                                <asp:ListItem Text="2250"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Weight" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWeight" CssClass="ac" runat="server" BorderWidth="0px" onkeyup="checkDecimal(this);" MaxLength="50"
                                            />
                                        </ItemTemplate>
                                        <ControlStyle Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Height" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtHeight" CssClass="ac" Text="127.3" runat="server" BorderWidth="0px" MaxLength="50" BackColor="LightGray" ReadOnly="true"
                                                onkeyup="checkDecimal(this);" Width="200px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Weight of empty mould + base plate" HeaderStyle-HorizontalAlign="Center"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWeightOfMould" CssClass="ac" runat="server" BorderWidth="0px" MaxLength="50"
                                                onkeyup="checkDecimal(this);" Width="220px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:Panel ID="OptionsCBRProviding" runat="server" Visible="false">
                    <table border="1px" style="margin-bottom: 7px;">
                        <tr>
                            <td>
                                Ring Size : &nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="ddlRingSize" runat="server" Width="50px" AutoPostBack="true" OnSelectedIndexChanged="ddlRingSize_SelectedIndexChanged">
                                    <asp:ListItem Text="25"></asp:ListItem>
                                    <asp:ListItem Text="10"></asp:ListItem>
                                    <asp:ListItem Text="2.5"></asp:ListItem>
                                    <asp:ListItem Text="2"></asp:ListItem>
                                    <asp:ListItem Text="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelCBRProviding" runat="server" Width="100%" ScrollBars="Auto" BorderStyle="Ridge"
                    BorderColor="LightGray" Visible="false" BorderWidth="0px" Height="317px">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GrdViewCBRProviding" runat="server" Height="100%" Width="40%" AutoGenerateColumns="false"
                                SkinID="gridviewSkin1">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="AddRowCBRProviding" runat="server" OnClick="AddRowCBRProviding_Click"
                                                ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                ToolTip="Add Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="18px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="DeleteRowCBRProviding" runat="server" OnClick="DeleteRowCBRProviding_Click"
                                                ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                ToolTip="Delete Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deflection" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDeflection" CssClass="ac" runat="server" Width="250px" MaxLength="50"
                                                onkeyup="checkNum(this);" BorderWidth="0px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Load" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLoad" CssClass="ac" runat="server" BorderWidth="0px" onkeyup="checkDecimal(this);" MaxLength="50"
                                                 Width="250px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:Panel ID="PanelCoreCutter" runat="server" Width="100%" ScrollBars="Auto" BorderWidth="0px"
                    BorderStyle="Ridge" BorderColor="LightGray" Visible="false" Height="317px">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GrdViewCoreCutter" runat="server" Height="100%" AutoGenerateColumns="False"
                                Width="80%" SkinID="gridviewSkin1">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="AddRowForCoreCutter" runat="server" OnClick="AddRowForCoreCutter_Click"
                                                ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                ToolTip="Add Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="18px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="DeleteRowForCoreCutter" runat="server" OnClick="DeleteRowForCoreCutter_Click"
                                                ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                ToolTip="Delete Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Core Cutter No" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCoreCutterNo" CssClass="ac" runat="server" Width="130px" MaxLength="50"
                                                onkeyup="checkNum(this);" BorderWidth="0px"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Weight" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWeight" CssClass="ac" runat="server" BorderWidth="0px" onkeyup="checkDecimal(this);" MaxLength="50"
                                               Width="140px" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Height" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtHeight" CssClass="ac" runat="server" BorderWidth="0px" onkeyup="checkDecimal(this);" MaxLength="50"
                                           Width="140px" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Diameter" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDiameter" CssClass="ac" runat="server" BorderWidth="0px" onkeyup="checkDecimal(this);" MaxLength="50"
                                                 Width="140px" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Volume" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVolume" CssClass="ac" runat="server" BorderWidth="0px" onkeyup="checkDecimal(this);" MaxLength="50"
                                                BackColor="LightGray" ReadOnly="true" Width="140px" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Volume" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtActualVolume" CssClass="ac" runat="server" BorderWidth="0px" MaxLength="50"
                                                onkeyup="checkDecimal(this);" Width="140px" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:Panel ID="OptionCalibration" runat="server" Visible="false">
                    <table border="1px" style="margin-bottom: 6px;" frame="box" title="Sand Pouring Cylinder"
                        width="200px">
                        <tr>
                            <td>
                                <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="RdbCalibration"
                                    OnTextChanged="RdbCalibration_OnTextChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Small"></asp:ListItem>
                                    <asp:ListItem Text="Medium"></asp:ListItem>
                                    <asp:ListItem Text="Big"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelCalibrationSand" runat="server" Width="100%" ScrollBars="Auto"
                    BorderWidth="0px" BorderStyle="Ridge" BorderColor="LightGray" Visible="false"
                    Height="270px">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GrdViewCalibrationSand" runat="server" Height="100%" AutoGenerateColumns="False"
                                SkinID="gridviewSkin">
                                <Columns>
                                    <asp:TemplateField HeaderText="Test No" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTestNo" runat="server" Width="355px" BorderWidth="0px"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="1" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtOne" CssClass="ac" runat="server" BorderWidth="0px" onkeyup="checkDecimal(this);" MaxLength="50"
                                                Width="98%" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="2" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTwo" CssClass="ac" runat="server" BorderWidth="0px" onkeyup="checkDecimal(this);" MaxLength="50"
                                                Width="98%" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="3" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtThree" CssClass="ac" runat="server" BorderWidth="0px" onkeyup="checkDecimal(this);" MaxLength="50"
                                                Width="98%" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="4" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFour" CssClass="ac" runat="server" BorderWidth="0px" onkeyup="checkDecimal(this);" MaxLength="50"
                                                Width="98%" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="5" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFive" CssClass="ac" runat="server" BorderWidth="0px" onkeyup="checkDecimal(this);" MaxLength="50"
                                                Width="98%" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:Panel ID="PanelWeightOfEmptyDish" runat="server" Width="100%" ScrollBars="Auto"
                    BorderStyle="Ridge" BorderColor="LightGray" Visible="false" BorderWidth="0px"
                    Height="310px">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdWeightOfEmptyDish" runat="server" Height="100%" Width="25%" AutoGenerateColumns="false"
                                SkinID="gridviewSkin1">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnAddRowDish" runat="server" OnClick="imgBtnAddRowDish_Click" ImageUrl="Images/AddNewitem.jpg"
                                                Height="18px" Width="18px" CausesValidation="false" ToolTip="Add Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="18px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnDeleteRowDish" runat="server" OnClick="imgBtnDeleteRowDish_Click"
                                                ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                ToolTip="Delete Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No. of Dish" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNoOfDish" CssClass="ac" runat="server" Width="250px"
                                               MaxLength="50"  onkeyup="checkNum(this);" BorderWidth="0px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Empty Dish weight" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWtDish" CssClass="ac" runat="server" BorderWidth="0px" 
                                              MaxLength="50"  onkeyup="checkDecimal(this);" Width="250px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:Panel ID="PanelWeightOfEmptyBottle" runat="server" Width="100%" ScrollBars="Auto"
                    BorderStyle="Ridge" BorderColor="LightGray" Visible="false" BorderWidth="0px"
                    Height="310px">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdWeightOfEmptyBottle" runat="server" Height="100%" Width="25%" AutoGenerateColumns="false"
                                SkinID="gridviewSkin1">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnAddRowBottle" runat="server" OnClick="imgBtnAddRowBottle_Click" ImageUrl="Images/AddNewitem.jpg"
                                                Height="18px" Width="18px" CausesValidation="false" ToolTip="Add Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="18px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnDeleteRowBottle" runat="server" OnClick="imgBtnDeleteRowBottle_Click"
                                                ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                ToolTip="Delete Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No of Bottle" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNoOfBottle" CssClass="ac" runat="server" Width="250px"
                                               MaxLength="50"  onkeyup="checkNum(this);" BorderWidth="0px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Empty Bottle weight" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWtBottle" CssClass="ac" runat="server" BorderWidth="0px" 
                                              MaxLength="50"  onkeyup="checkDecimal(this);" Width="250px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Wt. of Bottle + Dist. Water" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWtBottlePlusDistWater" CssClass="ac" runat="server" BorderWidth="0px" 
                                              MaxLength="50"  onkeyup="checkDecimal(this);" Width="250px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:Panel ID="PanelWeightOfEmptyPycnometer" runat="server" Width="100%" ScrollBars="Auto"
                    BorderStyle="Ridge" BorderColor="LightGray" Visible="false" BorderWidth="0px"
                    Height="310px">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdWeightOfEmptyPycnometer" runat="server" Height="100%" Width="25%" AutoGenerateColumns="false"
                                SkinID="gridviewSkin1">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnAddRowPycnometer" runat="server" OnClick="imgBtnAddRowPycnometer_Click" ImageUrl="Images/AddNewitem.jpg"
                                                Height="18px" Width="18px" CausesValidation="false" ToolTip="Add Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="18px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnDeleteRowPycnometers" runat="server" OnClick="imgBtnDeleteRowPycnometer_Click"
                                                ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                ToolTip="Delete Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No of Pycnometer" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNoOfPycnometer" CssClass="ac" runat="server" Width="250px"
                                               MaxLength="50"  onkeyup="checkNum(this);" BorderWidth="0px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Empty Pycnometers weight" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWtPycnometer" CssClass="ac" runat="server" BorderWidth="0px" 
                                              MaxLength="50"  onkeyup="checkDecimal(this);" Width="250px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Wt. of pycnometers + Dist. Water" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtWtPycnometerPlusDistWater" CssClass="ac" runat="server" BorderWidth="0px" 
                                              MaxLength="50"  onkeyup="checkDecimal(this);" Width="250px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </asp:Panel>
            <div style="height: 4px;">
            </div>
            <asp:Panel ID="pnlMachineSettings" runat="server" BorderWidth="0px" Height="45px"
                Visible="false" Width="100%">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdMachineSettings" runat="server" Height="100%" Width="55%" AutoGenerateColumns="false"
                            SkinID="gridviewSkin1">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="AddRowMachine" runat="server" OnClick="AddRowMachine_Click"
                                            ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                            ToolTip="Add Row" />
                                    </ItemTemplate>
                                    <ItemStyle Width="18px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="DeleteRowMachine" runat="server" OnClick="DeleteRowMachine_Click"
                                            ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                            ToolTip="Delete Row" />
                                    </ItemTemplate>
                                    <ItemStyle Width="16px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Machine No." HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMachineNo" CssClass="ac" runat="server" Width="150px" ReadOnly="true" BackColor="LightGray"
                                            BorderWidth="0px" Text="DM 1"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Operator Name" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOperatorName" CssClass="ac" runat="server" BorderWidth="0px" MaxLength="50"
                                            Width="250px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Owner Name" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOwnerName" CssClass="ac" runat="server" BorderWidth="0px" Width="250px" MaxLength="50"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <div style="margin-left: 820px; margin-top: 360px;">
                <asp:LinkButton ID="lnkCalculate" Font-Bold="true" runat="server" Font-Underline="true"
                    OnClick="lnkCalculate_Click">Calculate</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="lnkSave" Font-Bold="true" runat="server" Font-Underline="true"
                    OnClick="lnkSave_Click">Save</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="lnkExit" Font-Bold="true" runat="server" Font-Underline="true"
                    OnClick="lnkExit_Click">Exit</asp:LinkButton>
            </div>
            <br />
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
</asp:Content>
