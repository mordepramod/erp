<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Soil_Report_Gradation.aspx.cs" Inherits="DESPLWEB.Soil_Report_Gradation" Theme="duro" %>

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
                            <asp:TextBox ID="txtReportNo" runat="server" ReadOnly="true" Width="180px"></asp:TextBox>
                        </td>
                        <td style="width: 113px">
                        </td>
                        <td>
                            <asp:Label ID="lblGradationType" runat="server" Width="50px" Visible="false"></asp:Label>
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
            <asp:Panel ID="pnlGradationTab" runat="server" ScrollBars="Auto" Height="360px" Width="940px"
                BorderWidth="1px">
                <cc1:TabContainer ID="TabContainerGradation" runat="server" Width="100%" ActiveTabIndex="0">
                    <cc1:TabPanel runat="server" HeaderText="Gradation" ID="TabGradation" Width="100%"
                        Visible="false">
                        <ContentTemplate>
                            <asp:Panel ID="pnlGradation" runat="server" Height="322px" Width="100%" ScrollBars="Auto">
                                <table width="100%" style="height: 39px">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblGrdDateOfTesting" runat="server" Text="Date Of Testing :" 
                                                Width="120px" Height="16px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGrdDateOfTesting" runat="server" Width="100px" AutoPostBack="True"
                                                onClick="testDatePicker()"></asp:TextBox>&nbsp;
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtGrdDateOfTesting">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtGrdDateOfTesting"
                                                MaskType="Date" Mask="99/99/9999" AutoComplete="False" runat="server" Enabled="True"
                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="">
                                            </cc1:MaskedEditExtender>
                                        </td>
                                        <td>         
                                          <asp:Label ID="lblGrdOriginalWt" runat="server" Text="Original Wt.(g) :" 
                                                Width="109px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGrdOriginalWt" runat="server" onkeyup="checkDecimal(this);" Width="60px" CssClass="ac" MaxLength="10"></asp:TextBox> &nbsp;&nbsp;
                                        </td>
                                        <td style="padding-bottom:18px;">
                                            <asp:Label ID="lblGrdWtSample" runat="server" Height="16px" Text="Wt. of sample after 75 micron washing :" 
                                                Width="135px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGrdWtSampleWashing" runat="server" onkeyup="checkDecimal(this);" CssClass="ac" MaxLength="10" BackColor="LightGray"  ReadOnly="true"
                                                Width="60px" style="margin-left: 0px"></asp:TextBox>
                                        </td>
                                        <td style="padding-left:20px;">
                                            <asp:Label ID="lblGrdOvenDryswt" runat="server" Text="Oven Dry Wt. (g) :" 
                                                Width="139px"></asp:Label>
                                        </td>
                                        <td style="padding-right:20px;">
                                            <asp:TextBox ID="txtGrdOvenDryswt" runat="server" onkeyup="checkDecimal(this);" Width="60px" CssClass="ac" MaxLength="10"  BackColor="LightGray"  ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <div style="margin-left: 285px; height:17px; margin-top:5px;" >
                                     <asp:Label ID="lblCumulative" runat="server" Text="Cumulative" Font-Bold="True"></asp:Label>
                                </div>
                                <table width="100%">                                    
                                    <tr>
                                        <td colspan="3">
                                            <asp:GridView ID="grdGrdation" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="IS Sieve (mm)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGrdISSieve" runat="server" CssClass="ac" BorderWidth="0" MaxLength="10"
                                                                BackColor="LightGray" ReadOnly="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ControlStyle Width="120px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Wt. Retained (g)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGrdWtRetained" runat="server" CssClass="ac" BorderWidth="0" MaxLength="10"
                                                                onkeyup="checkDecimal(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="120px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="% Retained">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGrdPercentRetained1" runat="server" BorderWidth="0" CssClass="ac" MaxLength="10"
                                                                BackColor="LightGray" ReadOnly="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="120px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Wt. Passing (g)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGrdWtPassing" runat="server" BackColor="LightGray" CssClass="ac" MaxLength="10"
                                                                ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="120px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="% Passing">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGrdPassing" runat="server" CssClass="ac" BorderWidth="0" MaxLength="10"
                                                                BackColor="LightGray" ReadOnly="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="120px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="% Retained">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGrdPercentRetained2" runat="server" CssClass="ac" BackColor="LightGray" MaxLength="10"
                                                                ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="120px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGrdRemarks" runat="server" CssClass="ac" BorderWidth="0"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="120px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="width:660px">
                                            <asp:GridView ID="grdGrd" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                                Width="97%">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGrdCobble" runat="server" CssClass="ac" BorderWidth="0" MaxLength="10"
                                                              Width="100%"  BackColor="LightGray" ReadOnly="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ControlStyle Width="98%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGrdValues" runat="server" CssClass="ac" BorderWidth="0" MaxLength="10"
                                                            Width="100%" BackColor="LightGray" ReadOnly="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="98%" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGrdHundrad1" runat="server" BorderWidth="0" CssClass="ac" Width="100%" MaxLength="10"
                                                                BackColor="LightGray" onkeyup="checkDecimal(this)" ReadOnly="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="98%" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGrdHundrad2" runat="server" BackColor="LightGray" CssClass="ac" Width="100%" MaxLength="10"
                                                                BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="98%" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGrdHundrad3" runat="server" CssClass="ac" BorderWidth="0" Width="100%" MaxLength="10"
                                                                ReadOnly="true" BackColor="LightGray" onkeyup="checkDecimal(this)"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ControlStyle Width="98%" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblGrdD10" runat="server" Text="D10 :"></asp:Label>
                                            &nbsp;<asp:TextBox ID="txtGrdD10" runat="server" onkeyup="checkDecimal(this);" Width="100px" CssClass="ac" MaxLength="10"></asp:TextBox>
                                            <br />
                                            <br />
                                            <asp:Label ID="lblGrdD30" runat="server" Text="D30 :"></asp:Label>
                                            &nbsp;<asp:TextBox ID="txtGrdD30" runat="server" onkeyup="checkDecimal(this);" Width="100px" CssClass="ac" MaxLength="10"></asp:TextBox>
                                            <br />
                                            <br />
                                            <asp:Label ID="lblGrdD60" runat="server" Text="D60 :"></asp:Label>
                                            &nbsp;<asp:TextBox ID="txtGrdD60" runat="server" onkeyup="checkDecimal(this);" Width="100px" CssClass="ac" MaxLength="10"></asp:TextBox>
                                            <br />
                                            <br />
                                            <asp:Label ID="lblGrdCu" runat="server" Text="Cu  :"></asp:Label>
                                            &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtGrdCu" runat="server" ReadOnly="True" Width="100px" CssClass="ac" BackColor="LightGray"></asp:TextBox>
                                            <br />
                                            <br />
                                            <asp:Label ID="lblGrdCc" runat="server" Text="Cc  :"></asp:Label>
                                            &nbsp; &nbsp;<asp:TextBox ID="txtGrdCc" runat="server" ReadOnly="True" CssClass="ac" Width="100px" BackColor="LightGray"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Remark" ID="TabRemark" Visible="true">
                        <ContentTemplate>
                            <asp:Panel ID="pnlRemark" runat="server" ScrollBars="Auto" Height="322px" Width="100%"
                                BorderWidth="0px">
                               <br />
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
                                                <asp:TextBox ID="txtRemarkSrNo" CssClass="ac" runat="server" ReadOnly="true" AutoPostBack="true" MaxLength="10"
                                                    BorderWidth="0" Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                            </ItemTemplate>
                                            <ControlStyle Width="50px" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemark" runat="server" Width="800" BorderWidth="0px" MaxLength="250"></asp:TextBox>
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
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Size="Small"  Visible="false"
                              Font-Underline="true" onclick="lnkPrint_Click">Print</asp:LinkButton>&nbsp;  <%--OnClientClick="SoilDataPDFFun()"--%>
                        <asp:LinkButton ID="lnkExit" runat="server" Font-Size="Small" 
                            Font-Underline="true" onclick="lnkExit_Click">Exit</asp:LinkButton>
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
