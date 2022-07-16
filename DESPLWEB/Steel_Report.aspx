<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MstPg_Veena.Master"
    CodeBehind="Steel_Report.aspx.cs" Inherits="DESPLWEB.Steel_Report" Title="Steel Report"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px"  Height="470px"
            Style="background-color: #ECF5FF;">
            <div align="right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                    ImageAlign="Right" />
            </div>
            <asp:Panel ID="Panel3" runat="server" BorderStyle="Ridge" Width="942px" BorderColor="AliceBlue">
                <table style="width: 100%">
                    <tr style="background-color: #ECF5FF;">
                        <td width="11%">
                            <asp:Label ID="Label12" runat="server" Text="Sub Reports"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:DropDownList ID="ddl_SubReports" Width="195px" runat="server">
                            </asp:DropDownList>
                            <asp:LinkButton ID="lnk_Fetch" runat="server" Style="text-decoration: underline;"
                                Font-Bold="true" OnClick="lnk_Fetch_Click">Fetch</asp:LinkButton>
                        </td>
                        <td width="15%" style="text-align: right">
                            <asp:Label ID="Label6" runat="server" Text="Report No"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txt_ST" Text="ST" Width="30px" ReadOnly="true" runat="server"></asp:TextBox>
                            &nbsp;
                            <asp:TextBox ID="txt_ReportNo" Width="145px" ReadOnly="true" runat="server"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblEntry" runat="server" Text="Enter" Visible="false"></asp:Label>
                            <asp:Label ID="lblRecordNo" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblUserId" runat="server" Text="0" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Reference No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_ReferenceNo" Width="190px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label4" runat="server" Text="Type Of Bar"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txt_typeofSteel" ReadOnly="true" Width="190px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="Supplier"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Supplier" Width="190px" ReadOnly="true" runat="server"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label10" runat="server" Text="Grade Of Steel "></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <asp:TextBox ID="txt_GradeSteel" Width="190px" ReadOnly="true" runat="server"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label8" runat="server" Text="Compliance Note"></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <asp:DropDownList ID="ddl_ComplainceNote" Width="70px" runat="server">
                                <asp:ListItem Value="0" Text="Select" />
                                <asp:ListItem Value="1" Text="Add" />
                                <asp:ListItem Value="2" Text="Do not Add" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Description" Width="190px" runat="server"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label2" runat="server" Text="Date Of Testing"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txt_DateOfTesting" Width="190px" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_DateOfTesting">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_DateOfTesting"
                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                            </asp:MaskedEditExtender>
                        </td>
                        <td style="text-align: left">&nbsp; &nbsp;
                            <asp:RadioButton ID="Rdn_LoadKg" runat="server" GroupName="Kn" />
                            <asp:Label ID="Label3" runat="server" Text="Load in Kg"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButton ID="Rdn_LoadKn" runat="server" GroupName="Kn" />
                            <asp:Label ID="Label5" runat="server" Text="Load in KN"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="100px" BorderStyle="Ridge"
                Width="942px" BorderColor="AliceBlue">

                <div>
                    &nbsp;
                    <asp:Label ID="Label13" runat="server" Font-Bold="true" Text="NABL Scope"></asp:Label>
                    <asp:DropDownList ID="ddl_NablScope" runat="server"  Width="80px">
                        <asp:ListItem Text="--Select--" />
                        <asp:ListItem Text="F" />
                        <asp:ListItem Text="NA" />
                    </asp:DropDownList>
                    &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                    <asp:Label ID="Label14" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="ddl_NABLLocation" runat="server" Width="80px" Enabled="true">
                        <asp:ListItem Value="0" Text="0" />
                                        <asp:ListItem Value="1" Text="1" />
                                        <asp:ListItem Value="2" Text="2" />
                    </asp:DropDownList>
                    <br />
                </div>

                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdSteelEntry" runat="server" SkinID="gridviewSkin">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr No.">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                        <asp:Label ID="lblStar" runat="server" Text="" Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="48px" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dia(mm)" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Dia" BorderWidth="0px" Width="420px" Style="text-align: center"
                                            ReadOnly="true" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Id Mark" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_IdMark" BorderWidth="0px" Width="430px" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Weight" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Weight" BorderWidth="0px" runat="server" Style="text-align: right"
                                            Width="80px" onchange="javascript:checkWeight(this);" /><%-- Text='<%# Eval("splmt_Unit_var") %>'-CssClass = '<%#DataBinder.Eval(Container.DataItem, "WTTEST_TEST_Id")%>'--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Length" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Length" BorderWidth="0px" Width="80px" Style="text-align: right"
                                            onchange="javascript:checkWeight(this);" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Final Length" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_FinalLength" BorderWidth="0px" Width="80px" Style="text-align: right"
                                            onchange="javascript:checkWeight(this);" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Yield Load(KN)" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_YieldLoad" BorderWidth="0px" Width="80px" Style="text-align: right"
                                            onchange="javascript:checkYield(this);" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ultimate Load(KN)" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_UltiMateLoad" BorderWidth="0px" Width="100px" Style="text-align: right"
                                            onchange="javascript:checkYield(this);" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rebend" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddl_Rebend" BorderWidth="0px" Width="110px" runat="server">
                                            <asp:ListItem Text="Select" />
                                            <asp:ListItem Text="Satisfactory" />
                                            <asp:ListItem Text="Unsatisfactory" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bend" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddl_Bend" BorderWidth="0px" Width="110px" runat="server">
                                            <asp:ListItem Text="Select" />
                                            <asp:ListItem Text="Satisfactory" />
                                            <asp:ListItem Text="Unsatisfactory" />
                                        </asp:DropDownList>
                                        <asp:Label ID="lblGaugeLength" runat="server" Visible="false"></asp:Label>   
                                        <asp:Label ID="lblWtMeterImage1" runat="server" Visible="false"></asp:Label>   
                                        <asp:Label ID="lblTensileImage1" runat="server" Visible="false"></asp:Label>   
                                        <asp:Label ID="lblTensileImage2" runat="server" Visible="false"></asp:Label>   
                                        <asp:Label ID="lblTensileImage3" runat="server" Visible="false"></asp:Label>   
                                        <asp:Label ID="lblBendImage1" runat="server" Visible="false"></asp:Label>   
                                        <asp:Label ID="lblRebendImage1" runat="server" Visible="false"></asp:Label> 
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Height="90px" BorderStyle="Ridge"
                Width="942px" BorderColor="AliceBlue">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdSteelCalulated" runat="server" SkinID="gridviewSkin">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr No.">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="48px" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Id Mark" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_IdMark" BorderWidth="0px" Width="100px" ReadOnly="true" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="C/S Area" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_CsArea" BorderWidth="0px" Style="text-align: right" ReadOnly="true"
                                            runat="server" Width="80px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Wt/Meter" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_WtMeter" BorderWidth="0px" Style="text-align: right" ReadOnly="true"
                                            Width="80px" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Avg. Wt/Meter" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_AvgWtMeter" BorderWidth="0px" Style="text-align: right" Width="80px"
                                            ReadOnly="true" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="% Elongation" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Elongation" BorderWidth="0px" Style="text-align: right" Width="80px"
                                            ReadOnly="true" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Yield Stress" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_YieldStress" BorderWidth="0px" Style="text-align: right" Width="80px"
                                            ReadOnly="true" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ultimate Stress" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_UltimateStress" BorderWidth="0px" Style="text-align: right"
                                            Width="80px" ReadOnly="true" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="90px" BorderStyle="Ridge"
                Width="942px" BorderColor="AliceBlue">
                <asp:GridView ID="grdSteelRemark" runat="server" SkinID="gridviewSkin">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnAddRow" runat="server" OnCommand="imgBtnAddRow_Click"
                                    ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                    ToolTip="Add Row" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnDeleteRow" runat="server" OnCommand="imgBtnDeleteRow_Click"
                                    ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                    ToolTip="Delete Row" />
                            </ItemTemplate>
                            <ItemStyle Width="16px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Remark" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_REMARK" BorderWidth="0px" Width="850px" runat="server" Text='' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <table width="100%">
                <tr height="30px">
                    <td width="11%">
                        <asp:CheckBox ID="chk_WitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chk_WitnessBy_CheckedChanged" />
                        <asp:Label ID="Label7" runat="server" Text="Witness By"></asp:Label>
                    </td>
                    <td width="22%">
                        <asp:TextBox ID="txt_witnessBy" Width="190px" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td width="15%" style="text-align: right">
                        <asp:Label ID="lbl_TestedBy" runat="server" Text="Tested By"></asp:Label>
                    </td>
                    <td align="left">&nbsp; &nbsp;&nbsp; &nbsp; &nbsp;
                        <asp:DropDownList ID="ddl_TestedBy" Width="200px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right"></td>
                    <td align="right">
                        <asp:LinkButton ID="Lnk_Calculate" OnClick="Lnk_Calculate_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Calculate</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" Visible="false" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClick="lnk_Exit_Click" runat="server">Exit</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <br />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        function checkWeight(inputtxt) {
            var numbers = /^\d+(\.\d{1,2})?$/;
            if (inputtxt.value.match(numbers)) {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                return true;

            }
            else {

                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please enter valid integer or decimal number with 2 decimal places";
                inputtxt.focus();
                inputtxt.value = "";
                return false;
            }
        }
        function checkYield(inputtxt) {
            var numbers = /^\d+(\.\d{1,3})?$/;
            if (inputtxt.value.match(numbers)) {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                return true;

            }
            else {

                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please enter valid integer or decimal number with 3 decimal places";
                inputtxt.focus();
                inputtxt.value = "";
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
