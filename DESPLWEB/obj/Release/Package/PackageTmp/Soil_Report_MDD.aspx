<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Soil_Report_MDD.aspx.cs" Inherits="DESPLWEB.Soil_Report_MDD" Theme="duro" %>

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
                            <asp:Label ID="lblMDDType" runat="server" Text="MDD/OMC"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMDDType" runat="server" Width="235px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="NABL Scope" Font-Bold="true"></asp:Label>
                        </td>
                        <td>

                            <asp:DropDownList ID="ddl_NablScope" AutoPostBack="true" Width="230px" runat="server">
                                <asp:ListItem Text="--Select--" />
                                <asp:ListItem Text="F" />
                                <asp:ListItem Text="NA" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>

                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_NABLLocation" runat="server"  Width="230px" Enabled="true">
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
                    <cc1:TabPanel runat="server" HeaderText="MDD/OMC" ID="TabMDD" Width="100%" Visible="false">
                        <ContentTemplate>
                            <asp:Panel ID="pnlMDD" runat="server">
                                <table width="98%" height="322px">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMDDDateOfTesting" runat="server" Text="Date Of Testing"></asp:Label>
                                            &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtMDDDateOfTesting" runat="server" Width="130px"
                                                AutoPostBack="True" ></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtMDDDateOfTesting">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtMDDDateOfTesting"
                                                MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="">
                                            </cc1:MaskedEditExtender>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label ID="lblMDDMouldNo" runat="server" Text="Mould No."></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlMDDMouldNo" runat="server" Width="100px" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlMDDMouldNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblMDDRows" runat="server" Text="Number of Rows"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:TextBox ID="txtMDDStdRows" runat="server" Width="30px" AutoPostBack="True" OnTextChanged="txtMDDStdRows_TextChanged"
                                                onkeyup="checkNum(this);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlgrdMDD" runat="server" Height="260px" ScrollBars="Auto">
                                                <div style="height: 20px">
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblMDDStdReadings" runat="server" Text="Readings" Font-Bold="True"
                                                        ForeColor="DarkOrange"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </div>
                                                <asp:GridView ID="grdMDDStd" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr. No.">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdSrNo" CssClass="ac" runat="server" ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>"
                                                                    BorderWidth="0" BackColor="LightGray">  </asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="50px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mould No" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdMouldNo" runat="server" BackColor="LightGray" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ControlStyle Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mould Volume">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdMouldVolume" runat="server" BackColor="LightGray" BorderWidth="0" ReadOnly="true" ></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ControlStyle Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass of Mould">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdMassOfMould" runat="server" CssClass="ac" BackColor="LightGray"
                                                                    BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass of Mould + Material">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdMassPlusMat" runat="server" CssClass="ac" onkeyup="checkDecimal(this)"
                                                                    BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Wet Density">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdWetDensity" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    onkeyup="checkDecimal(this)" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass Of Wet Material">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdMassOfWetMat" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    AutoPostBack="true" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <div style="height: 20px">
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblMDDStdObservations" runat="server" Text="Observations" Font-Bold="True"
                                                        ForeColor="DarkOrange"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </div>
                                                <asp:GridView ID="grdMDDStdObs" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin"
                                                    OnRowDataBound="grdMDDStdObs_OnRowDataBound" 
                                                    onrowediting="grdMDDStdObs_RowEditing">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr. No.">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdObsSrNo" CssClass="ac" runat="server" ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>"
                                                                    BorderWidth="0" BackColor="LightGray">  </asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="50px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cont No">
                                                            <ItemTemplate>
                                                                <%--<asp:DropDownList ID="ddlMDDStdObsContNo" runat="server" BackColor="Transparent"
                                                                    BorderWidth="0" AutoPostBack="True" OnSelectedIndexChanged="ddlMDDStdObsContNo_SelectedIndexChanged">--%>
                                                                    <asp:DropDownList ID="ddlMDDStdObsContNo" runat="server" BackColor="Transparent" BorderWidth="0" EnableViewState="true" >
                                                                    <%--AutoPostBack="true" OnSelectedIndexChanged="ddlMDDStdObsContNo_SelectedIndexChanged"--%> 
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ControlStyle Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cont + Wt Sample">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdObsContWtSample" runat="server" CssClass="ac" onkeyup="checkDecimal(this)"
                                                                    BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cont + Dry Sample">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdObsContDrySample" runat="server" CssClass="ac" onkeyup="checkDecimal(this)"
                                                                    BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass of Cont">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdObsMassOfCont" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    onkeyup="checkDecimal(this)" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass Of Water">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdObsMassOfWater" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    AutoPostBack="true" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mass Of Dry Material">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdObsMassOfDryMat" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    AutoPostBack="true" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Moisture Content">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdObsMoistCont" CssClass="ac" runat="server" BackColor="LightGray"
                                                                    AutoPostBack="true" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <div style="height: 20px">
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblMDDStdResult" runat="server" Text="Result" Font-Bold="True" ForeColor="DarkOrange"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </div>
                                                <asp:GridView ID="grdMDDStdResult" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr. No.">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdObsSrNo" CssClass="ac" runat="server" ReadOnly="true" Text=" <%#Container.DataItemIndex+1 %>"
                                                                    BorderWidth="0" BackColor="LightGray">  </asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="50px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dry Density">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdResDryDens" runat="server"  CssClass="ac" ReadOnly="true" BackColor="LightGray" BorderWidth="0"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ControlStyle Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Moisture Content (%)">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtMDDStdResMoistCont" runat="server" CssClass="ac"  ReadOnly="true" BackColor="LightGray" 
                                                                    BorderWidth="0"></asp:TextBox>
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
                                        <td>
                                        <asp:CheckBox ID="chkEditMDDOMC" runat="server" Visible="False" 
                                                AutoPostBack="true" oncheckedchanged="chkEditMDDOMC_CheckedChanged"/>
                                                <asp:Label ID="lblEditMDDOMC" runat="server" Text="Edit"></asp:Label>&nbsp;
                                            <asp:Label ID="lblMDDStd" runat="server" Text="MDD"></asp:Label>&nbsp;&nbsp;
                                            <asp:TextBox ID="txtMDDStd" runat="server" Width="100px" ReadOnly="True" 
                                                BackColor="LightGray" onkeyup="checkDecimal(this)"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblOMCStd" runat="server" Text="OMC"></asp:Label>&nbsp;&nbsp;
                                            <asp:TextBox ID="txtOMCStd" runat="server" Width="100px" ReadOnly="True" 
                                                BackColor="LightGray" onkeyup="checkDecimal(this)"></asp:TextBox>
                                                                                            
                                        </td>
                                        <td align="right" valign="top">
                                            <asp:Label ID="lblMDDStdSampleForCBR" runat="server" Text="Sample for CBR"></asp:Label>&nbsp;&nbsp;
                                            <asp:TextBox ID="txtMDDStdSampleForCBR" runat="server" Width="120px" 
                                                ReadOnly="True" BackColor="LightGray"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblMDDStdWaterForCBR" runat="server" Text="Water for CBR"></asp:Label>&nbsp;&nbsp;
                                            <asp:TextBox ID="txtMDDStdWaterForCBR" runat="server" Width="120px" 
                                                ReadOnly="True" BackColor="LightGray"></asp:TextBox>
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

        function myDataFun() {
            window.open("BrickReportPDF.aspx");
        };


        //        function testDatePicker() {
        //            $("#TxtDateOfTest").datepicker({ minDate: -10000, maxDate: "+0D" }).focus();
        //        }
        //        $(function () {
        //            $("#TxtDateOfTest").datepicker({ minDate: -10000, maxDate: "+0D" });
        //        });

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
