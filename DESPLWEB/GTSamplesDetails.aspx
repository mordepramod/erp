<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="GTSamplesDetails.aspx.cs" Inherits="DESPLWEB.GTSamplesDetails" Theme="duro" %>

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
                                <td>
                                    <asp:Label runat="server" ID="lblRefNo" Text="Ref. No. :"></asp:Label>&nbsp;
                                    <asp:TextBox runat="server" ID="txtRefNo" Width="140px" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblRecordNo" Text="Record No. :"></asp:Label>&nbsp;
                                    <asp:TextBox runat="server" ID="txtRecordNo" Width="140px" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblDateReceipt" Text="Date of Receipt :"></asp:Label>&nbsp;
                                    <asp:TextBox runat="server" ID="txtDateOfReceipt" Width="140px" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblDateTesting" Text="Date of Testing :"></asp:Label>&nbsp;
                                    <asp:TextBox runat="server" ID="txtDateOfTesting" Width="140px" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblClient" Text="Client. :"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox runat="server" ID="txtClient" Width="369px" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblSiteName" Text="Site Name :"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox runat="server" ID="txtSiteName" Width="390px" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <div style="height: 5px">
                        &nbsp;&nbsp;
                    </div>
                    <asp:Panel ID="PnlMainData" runat="server" Width="942px" BorderWidth="1px" ScrollBars="Auto">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <div>
                                        <asp:RadioButtonList runat="server" ID="rdbSample" AutoPostBack="true" RepeatDirection="Horizontal"
                                            CssClass="mylist" OnSelectedIndexChanged="rdbSample_SelectedIndexChanged" Height="16px">
                                            <asp:ListItem Text="Sampling"></asp:ListItem>
                                            <asp:ListItem Text="Testing"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div style="margin-top: 10px;">
                                        <asp:Label runat="server" ID="lblRock" Text="Rock" Font-Bold="true"></asp:Label>
                                    </div>
                                    <asp:Panel runat="server" ID="pnlSamples" Width="98%" Height="300px">
                                        <asp:GridView ID="grdGTRock" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                            Width="70%">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="~/Images/AddNewitem.jpg" ID="btnAddRowRock" runat="server"
                                                            OnClick="btnAddRowRock_Click" Width="18px" ImageAlign="Middle" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="~/Images/DeleteItem.png" ID="btnDeleteRowRock" OnClick="btnDeleteRowRock_Click"
                                                            runat="server" Width="16px" ImageAlign="Middle" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sr No.">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSrNo" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                            BackColor="LightGray" Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Lab ID No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLabIdNo" runat="server" Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txtLabIDNo" runat="server" CssClass="ac" BorderWidth="0" BackColor="LightGray"
                                                            ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="80px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="BH No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBHNo" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)"
                                                            MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="80px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Depth" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDepth" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)"
                                                            MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="80px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Piece No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPieceNo" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="90px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="ac" BorderWidth="0" MaxLength="250"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="200px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Avg. dia of core" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAvgdiaofCore" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="90px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Avg. height of core" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtheightCore" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="105px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SSD wt in water" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSSDwtWater" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="100px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SSD wt surface dry" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSSDwtSurfaceDry" runat="server" CssClass="ac" BorderWidth="0"
                                                            onkeyup="checkDecimal(this)" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="120px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Oven Dry Wt" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtOvenDryWt" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="90px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Load at failure" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLoadatfailure" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="100px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Comment" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtComment" runat="server" CssClass="ac" BorderWidth="0" MaxLength="250"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="118px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                        <asp:Label runat="server" ID="lblSoil" Text="Soil" Font-Bold="true"></asp:Label>
                                        <asp:GridView ID="grdGTSoil" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                            Width="87%">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="~/Images/AddNewitem.jpg" ID="btnAddRowSoil" runat="server"
                                                            OnClick="btnAddRowSoil_Click" Width="18px" ImageAlign="Middle" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="~/Images/DeleteItem.png" ID="btnDeleteRowSoil" OnClick="btnDeleteRowSoil_Click"
                                                            runat="server" Width="16px" ImageAlign="Middle" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sr No.">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSrNo" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                            BackColor="LightGray" Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="50px" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Lab ID No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLabIdNo" runat="server" Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txtLabIDNo" runat="server" CssClass="ac" BorderWidth="0" BackColor="LightGray"
                                                            ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="80px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="BH No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBHNo" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="80px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Depth" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDepth" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="80px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DS No./SPT No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDSNoSPTNo" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="90px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="ac" BorderWidth="0" MaxLength="250"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="200px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Moisture" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMoisture" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="150px" />
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
                    <div style="height: 10px">
                        &nbsp;&nbsp; &nbsp;&nbsp;
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td width="27%">
                                    <asp:Label ID="lblSampledBy" runat="server" Text="Sampled By"></asp:Label>
                                    &nbsp;
                                    <asp:DropDownList ID="ddlSampledBy" runat="server" Width="140px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 8%">
                                    <asp:Label ID="lblEntdChkdBy" runat="server" Text="Checked By"></asp:Label>
                                </td>
                                <td width="18%" align="center">
                                    <asp:DropDownList ID="ddlEntdChkdBy" runat="server" Width="140px">
                                    </asp:DropDownList>
                                </td>
                                <td width="8%">
                                    <asp:Label ID="lblTestdApprdBy" runat="server" Text="Approved By"></asp:Label>
                                </td>
                                <td width="17%" align="center">
                                    <asp:DropDownList ID="ddlTestdApprdBy" runat="server" Width="140px">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: right" width="20%" align="center">
                                    <asp:LinkButton ID="lnkSave" Font-Size="Small" runat="server" Font-Underline="true"
                                        OnClick="lnkSave_Click">Save</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkExit" runat="server" Font-Size="Small" 
                                        Font-Underline="true" onclick="lnkExit_Click">Exit</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
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
                    alert("Only Numeric Values Allowed");

                    return false;
                }

            }
            return true;
        }
           
           
    </script>
</asp:Content>
