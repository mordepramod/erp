<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MstPg_Veena.Master"
    CodeBehind="Water_Report.aspx.cs" Inherits="DESPLWEB.Water_Report" Theme="duro"
    Title="Water Test Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <div style="height: 1px" align="right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                    ImageAlign="Right" />
            </div>
            <asp:Panel ID="Panel2" runat="server" BorderStyle="Ridge" Width="942px" BorderColor="AliceBlue">
                <table style="width: 100%">
                    <tr style="background-color: #ECF5FF;">
                        <td>
                            <asp:Label ID="lbl_OtherPending" runat="server" Text="Other Pending Reports"></asp:Label>
                        </td>
                        <td width="30%">
                            <asp:DropDownList ID="ddl_OtherPendingRpt" Width="205px" runat="server">
                            </asp:DropDownList>
                            <asp:LinkButton ID="lnk_Fetch" runat="server" Style="text-decoration: underline;"
                                Font-Bold="true" OnClick="lnk_Fetch_Click">Fetch</asp:LinkButton>
                        </td>
                        <td width="10%" align="right">
                            <asp:Label ID="lbl_RptNo" runat="server" Text="Report No"></asp:Label>
                        </td>
                        <td width="20%" align="center">
                            <asp:TextBox ID="txt_RecordType" Width="40px" runat="server" ReadOnly="true"></asp:TextBox>
                            <asp:TextBox ID="txt_ReportNo" runat="server" Width="140px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td width="10%" align="right">
                            <asp:Label ID="lblEntry" runat="server" Text="Enter" Visible="false"></asp:Label>
                            <asp:Label ID="lblRecordNo" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td width="15%">
                            <asp:Label ID="Label1" runat="server" Text="Reference No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_ReferenceNo" Width="200px" runat="server" ReadOnly="true"></asp:TextBox>
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
                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Supplier "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Supplier" Width="200px" ReadOnly="true" runat="server"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label9" runat="server" Text="NABL Scope" Font-Bold="true"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="ddl_NablScope" AutoPostBack="true" Width="180px" runat="server">
                                <asp:ListItem Text="--Select--" />
                                <asp:ListItem Text="F" />
                                <asp:ListItem Text="NA" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Description" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                                    <asp:Label ID="Label11" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>
                        </td>
                        <td  align="center">
                       <asp:DropDownList ID="ddl_NABLLocation" runat="server" Width="180px" Enabled="true">
                        <asp:ListItem Value="0" Text="0" />
                        <asp:ListItem Value="1" Text="1" />
                        <asp:ListItem Value="2" Text="2" />
                    </asp:DropDownList>
                     </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="160px" BorderStyle="Ridge"
                Width="942px" BorderColor="AliceBlue">

               
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdWaterEntryRptInward" runat="server" SkinID="gridviewSkin">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr No.">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="48px" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name Of Test" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_NameOfTest" BorderWidth="0px" Width="200px" Text='<%# Eval("TEST_Name_var") %>'
                                            ReadOnly="true" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Result" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_result" BorderWidth="0px" Style="text-align: right" CssClass='<%#DataBinder.Eval(Container.DataItem, "WTTEST_TEST_Id")%>'
                                            runat="server" Width="100px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Unit" BorderWidth="0px" Width="110px" Style="text-align: center"
                                            Text='<%# Eval("splmt_Unit_var") %>' ReadOnly="true" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Specified Limits" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_SpecifiedLimits" BorderWidth="0px" Width="210px" ReadOnly="true"
                                            Text='<%# Eval("splmt_SpecifiedLimit_var") %>' runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Method Of Testing" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_MetodOfTsting" BorderWidth="0px" Width="210px" ReadOnly="true"
                                            Text='<%# Eval("splmt_testingMethod_var") %>' runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="140px" BorderStyle="Ridge"
                Width="942px" BorderColor="AliceBlue">
                <asp:GridView ID="grdWaterRemark" runat="server" SkinID="gridviewSkin">
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
                    <td width="15%">
                        <asp:CheckBox ID="chk_WitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chk_WitnessBy_CheckedChanged" />
                        <asp:Label ID="Label7" runat="server" Text="Witness By"></asp:Label>
                    </td>
                    <td width="35%">
                        <asp:TextBox ID="txt_witnessBy" Width="200px" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td align="right">
                        <asp:Label ID="lbl_TestedBy" runat="server" Text="Tested By"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:DropDownList ID="ddl_TestedBy" Width="200px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td width="10%" align="right">
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" Visible="false" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClick="lnk_Exit_Click" runat="server">Exit</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
