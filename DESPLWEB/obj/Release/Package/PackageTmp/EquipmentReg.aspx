<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="EquipmentReg.aspx.cs" Inherits="DESPLWEB.EquipmentReg" Theme="duro" %>

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
                        <td style="width: 79px">
                            <asp:Label runat="server" ID="lblEquipments" Text="Equipments :" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="width: 313px">
                            <asp:DropDownList runat="server" ID="ddlEquipments" Width="300px" OnSelectedIndexChanged="ddlEquipments_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkViewAll" AutoPostBack="true" OnCheckedChanged="chkViewAll_CheckedChanged" />
                            <asp:Label runat="server" ID="lblviewAll" Text="View All Equipment Details" Font-Bold="true"></asp:Label>
                               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:FileUpload ID="FileUpload1" runat="server" Width="190px" Visible="false" />
                            <asp:LinkButton ID="lnkExportExcel" runat="server" Font-Underline="true" Visible="false"
                                onclick="lnkExportExcel_Click">Import Data</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="PnlEquipment" runat="server" Width="942px" BorderWidth="1px" ScrollBars="Auto">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblEquipmentName" Text="Equipment Name :" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEquipmentName" Width="201px" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblSerialNo" Text="Serial Name :" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSerialNo" Width="201px" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblIDMark" Text="ID Mark :" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIDMark" Width="201px" BackColor="LightGray" ReadOnly="true" runat="server"
                                MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblMake" Text="Make :" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAutoMake" Width="201px" runat="server" MaxLength="50"></asp:TextBox>
                            <cc1:AutoCompleteExtender ServiceMethod="GetMakeList" MinimumPrefixLength="1" CompletionInterval="10"
                                EnableCaching="false" CompletionSetCount="1" TargetControlID="txtAutoMake" ID="AutoCompleteExtender1"
                                runat="server" FirstRowSelected="false">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblShortName" Text="Short Name :" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAutoShortName" Width="201px" runat="server" MaxLength="50"></asp:TextBox>
                            <cc1:AutoCompleteExtender ServiceMethod="GetShortNameList" MinimumPrefixLength="1"
                                CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtAutoShortName"
                                ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblSection" Text="Section :" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAutoSection" Width="201px" runat="server" MaxLength="10"></asp:TextBox>
                        </td>
                        <cc1:AutoCompleteExtender ServiceMethod="GetSectionList" MinimumPrefixLength="1"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtAutoSection"
                            ID="AutoCompleteExtender3" runat="server" FirstRowSelected="false">
                        </cc1:AutoCompleteExtender>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblLeastCount" Text="Least Count :" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAutoLeastCount" Width="201px" runat="server" MaxLength="50"></asp:TextBox>
                            <cc1:AutoCompleteExtender ServiceMethod="GetLeastCountList" MinimumPrefixLength="1"
                                CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtAutoLeastCount"
                                ID="AutoCompleteExtender4" runat="server" FirstRowSelected="false">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblRange" Text="Range :" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAutoRange" Width="201px" runat="server" MaxLength="50"></asp:TextBox>
                            <cc1:AutoCompleteExtender ServiceMethod="GetRangeList" MinimumPrefixLength="1" CompletionInterval="10"
                                EnableCaching="false" CompletionSetCount="1" TargetControlID="txtAutoRange" ID="AutoCompleteExtender5"
                                runat="server" FirstRowSelected="false">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblRecdOn" Text="Recd On :" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRecdOn" Width="201px" runat="server" onkeyup="checkDate(this);"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtRecdOn">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblCalibration" Text="Calibration Staus :" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAutoCalibration" Width="201px" runat="server" MaxLength="50"></asp:TextBox>
                            <cc1:AutoCompleteExtender ServiceMethod="GetCalibrationStatusList" MinimumPrefixLength="1"
                                CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtAutoCalibration"
                                ID="AutoCompleteExtender6" runat="server" FirstRowSelected="false">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCertificate" Text="Certificate :" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAutoCertificate" Width="201px" runat="server" MaxLength="50"></asp:TextBox>
                            <cc1:AutoCompleteExtender ServiceMethod="GetCertificateList" MinimumPrefixLength="1"
                                CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtAutoCertificate"
                                ID="AutoCompleteExtender7" runat="server" FirstRowSelected="false">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="pnlCalibration" runat="server" Width="942px" BorderWidth="1px" ScrollBars="Auto"
                Height="290px" BackColor="LightGray">
                <asp:GridView ID="grdCal" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                    Width="87%">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="~/Images/AddNewitem.jpg" ID="BtnAddRowCalibration" runat="server"
                                    OnClick="BtnAddRowCalibration_Click" Width="18px" ImageAlign="Middle" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="~/Images/DeleteItem.png" ID="BtnDeleteRowCalibration"
                                    OnClick="BtnDeleteRowCalibration_Click" runat="server" Width="16px" ImageAlign="Middle" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sr No.">
                            <ItemTemplate>
                                <asp:TextBox ID="txtSrNo" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                    MaxLength="10" Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="50px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date of last Calibration" Visible="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDateoflastCalibration" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDate(this);"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                    Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtDateoflastCalibration">
                                </cc1:CalendarExtender>
                            </ItemTemplate>
                            <ControlStyle Width="250px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Calibration due on" Visible="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCalibrationDueon" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDate(this);"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                    Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtCalibrationDueon">
                                </cc1:CalendarExtender>
                            </ItemTemplate>
                            <ControlStyle Width="250px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Calibrating Agency" Visible="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCalibratingAgency" runat="server" CssClass="ac" BorderWidth="0"
                                    MaxLength="50"></asp:TextBox>
                                <cc1:AutoCompleteExtender ServiceMethod="GetCalibrationAgencyList" MinimumPrefixLength="1"
                                    CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtCalibratingAgency"
                                    ID="AutoCompleteExtender4" runat="server" FirstRowSelected="false">
                                </cc1:AutoCompleteExtender>
                            </ItemTemplate>
                            <ControlStyle Width="250px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="pnlEquipmentDetails" runat="server" Width="942px" BorderWidth="1px"
                Height="410px" Visible="false">
                <table>
                    <tr>
                        <td colspan="3">
                        </td>                        
                        <td colspan="5" align="right">
                            <div style="margin-left: 90px">
                                <asp:LinkButton ID="lnkSelectAll" runat="server" Font-Size="Small" Font-Underline="true"
                                    OnClick="lnkSelectAll_Click">Select All</asp:LinkButton>&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkDeSelectAll" runat="server" Font-Size="Small" Font-Underline="true"
                                    OnClick="lnkDeSelectAll_Click">Deselect All</asp:LinkButton>&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkPrintLabel" runat="server" Font-Size="Small" Font-Underline="true"
                                      onclick="lnkPrintLabel_Click">Print EquipList(Excel)</asp:LinkButton>&nbsp;&nbsp; <%--OnClientClick="EquipmentLabelPDFFun()"--%>
                                <asp:LinkButton ID="lnkPrintEquipList" runat="server" Font-Size="Small" Font-Underline="true"
                                      onclick="lnkPrintEquipList_Click">Print EquipList(Pdf)</asp:LinkButton></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblEquSection" Text="Section :" Font-Bold="true" Width="53px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEquSection" Width="165px" OnSelectedIndexChanged="ddlEquSection_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCaliStatus" Text="Calibration Status :" Font-Bold="true"
                                Width="70px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEquCalibrationStatus" Width="165px" OnSelectedIndexChanged="ddlEquCalibrationStatus_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblEquMake" Text="Make :" Font-Bold="true" Width="43px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEquMake" Width="165px" OnSelectedIndexChanged="ddlEquMake_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblEquCertificate" Text="Certificate :" Font-Bold="true"
                                Width="65px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEquCertificate" Width="165px" OnSelectedIndexChanged="ddlEquCertificate_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <div style="height: 4px">
                    &nbsp;&nbsp;
                </div>
                <asp:Panel ID="Panel1" runat="server" Width="942px" BorderWidth="0px" ScrollBars="Auto"
                    Height="347px">
                    <asp:GridView ID="grdEquipmentDetails" runat="server" AutoGenerateColumns="False" 
                        SkinID="gridviewSkin1" Width="70%">
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelect" CssClass="ac"  AutoPostBack="True" OnCheckedChanged="chkSelect_CheckedChanged" />
                                </ItemTemplate>
                                <ControlStyle Width="35px" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblId" Text=""></asp:Label>
                                </ItemTemplate>
                                <ControlStyle Width="35px" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sr No.">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtSrNo" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                        MaxLength="10" BackColor="LightGray" Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Width="50px" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Equipment" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtEquipment" runat="server" Text='<%#Bind("txtEquipment") %>' BorderWidth="0"
                                        ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Width="220px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Section" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtSection" runat="server" Text='<%#Bind("txtSection") %>' BorderWidth="0"
                                        ReadOnly="true" CssClass="ac" BackColor="LightGray"></asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Width="80px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Internal Id Mark" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtInternalIdMark" Text='<%#Bind("txtInternalIdMark") %>' runat="server"
                                        CssClass="ac" BorderWidth="0" BackColor="LightGray" ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Width="200px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Calibration Status" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCalibrationStatus" runat="server" BorderWidth="0" Text='<%#Bind("txtCalibrationStatus") %>'
                                        BackColor="LightGray" CssClass="ac" ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Width="115px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Serial No." Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtSerialNo" runat="server" BorderWidth="0" Text='<%#Bind("txtSerialNo") %>'
                                        BackColor="LightGray" CssClass="ac" ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Width="85px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Make" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMake" runat="server" BorderWidth="0" Text='<%#Bind("txtMake") %>'
                                        BackColor="LightGray" CssClass="ac" ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Width="85px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Certificate" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCertificate" runat="server" BorderWidth="0" Text='<%#Bind("txtCertificate") %>'
                                        BackColor="LightGray" CssClass="ac" ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Width="105px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Least Count" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtLeastCount" runat="server" BorderWidth="0" Text='<%#Bind("txtLeastCount") %>'
                                        BackColor="LightGray" CssClass="ac" ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Width="105px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Recd On" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRecdOn" runat="server" BorderWidth="0" Text='<%#Bind("txtRecdOn") %>'
                                        BackColor="LightGray" CssClass="ac" ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Width="95px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Range" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRange" runat="server" BorderWidth="0" Text='<%#Bind("txtRange") %>'
                                        BackColor="LightGray" CssClass="ac" ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Width="160px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Staus" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtStatus" runat="server" BorderWidth="0" Text='<%#Bind("txtStatus") %>'
                                        BackColor="LightGray" CssClass="ac" ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Width="160px" />
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlButton" runat="server" Width="942px">
                <div style="margin-left: 520px" id="divButton">                    
                    <asp:LinkButton ID="lnkSave" Font-Size="Small" runat="server" Font-Underline="true"
                        OnClick="lnkSave_Click">Save</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnkPrintEquip" Font-Size="Small" runat="server" Font-Underline="true"
                          onclick="lnkPrintEquip_Click">Print Equip History Sheet</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnlCalibExpired" Font-Size="Small" runat="server" Font-Underline="true"
                          onclick="lnlCalibExpired_Click">Calib Expired List</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnkCalibDueList" Font-Size="Small" runat="server" Font-Underline="true"
                          onclick="lnkCalibDueList_Click">Calib Due List</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lnkExit" runat="server" Font-Size="Small" 
                        Font-Underline="true" onclick="lnkExit_Click">Exit</asp:LinkButton>
                </div>
            </asp:Panel>
        </asp:Panel>
    </div>
    <script type="text/javascript">

        function EquipmentLabelPDFFun() {
            window.open("FrmLabEquipmentPDFReport.aspx?Print=1");
        };
        function EquipmentlistPDFFun() {
            window.open("FrmLabEquipmentPDFReport.aspx?Print=2");
        };
        function CalibDuelistPDFFun() {
            window.open("FrmLabEquipmentPDFReport.aspx?Print=3");
        };
        function CalibExpiredlistPDFFun() {
            window.open("FrmLabEquipmentPDFReport.aspx?Print=4");
        };
        function EquipHistorylistPDFFun() {
            window.open("FrmLabEquipmentPDFReport.aspx?Print=5");
        };
        function checkDate(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 47))) {
                    x.value = '';
                    x.focus();
                    alert("Please enter valid date.");

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
