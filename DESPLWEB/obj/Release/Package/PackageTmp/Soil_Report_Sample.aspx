<%@ Page Title="Soil Sample" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="Soil_Report_Sample.aspx.cs" Inherits="DESPLWEB.Soil_Report_Sample"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <div style="height: 5px" align="Right">
                <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                    OnClick="imgClosePopup_Click" ImageAlign="Right" />
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="pnlDetails" runat="server" Width="942px" BorderWidth="0px">
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
                            <asp:Label ID="lblSamplename" runat="server" Text="" Visible="false"></asp:Label>
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
                            <asp:Label ID="lblIdMark" runat="server" Text="Id Mark"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIdMark" runat="server" Width="235px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRecordNo" runat="server" Text="Report No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRecordType" runat="server" ReadOnly="true" Text="BT-" Width="47px"></asp:TextBox>
                            <asp:TextBox ID="txtReportNo" runat="server" ReadOnly="true" Width="180px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblDesc" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDesc" runat="server" Width="235px"></asp:TextBox>
                        </td>
                    </tr>
                     
                    <tr style="height: 5px">
                        <td colspan="4"></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Panel ID="pnlSample" runat="server" Height="320px" Width="98%" BorderWidth="1px"
                                ScrollBars="Auto">
                                <asp:GridView ID="grdSample" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin" DataKeyNames="TEST_Sr_No">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgInsertSample" runat="server" OnCommand="imgInsertSample_Click"
                                                    ImageUrl="Images/AddNewitem.jpg" Height="20px" Width="20px" CausesValidation="false"
                                                    ToolTip="Add New Sample" />
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgEditSample" runat="server" CommandArgument='<%#Eval("SOSMPLTEST_SampleName_var")%>'
                                                    OnCommand="imgEditSample_Click" ImageUrl="Images/Edit.jpg" Height="18px" Width="18px"
                                                    CausesValidation="false" ToolTip="Edit Client" />
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Test Sr No" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTestId" runat="server" Text='<%#Eval("TEST_Sr_No") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sample Name" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSampleName1" runat="server" Text='<%#Eval("SOSMPLTEST_SampleName_var") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sample Name" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSampleName" runat="server" Text='<%#Eval("SOSMPLTEST_SampleName_var") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Test" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTestName" runat="server" Text='<%#Eval("TEST_Name_var") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkAction" runat="server" Text='<%#Eval("action1") %>' CommandArgument='<%# Container.DataItemIndex %>'
                                                    OnCommand="lnkAction_Click">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="70px" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr style="height: 5px">
                        <td colspan="4"></td>
                    </tr>
                    <%--<tr>
                        <td>
                            <asp:Label ID="lblTestdApprdBy" runat="server" Text="Approved By"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTestdApprdBy" runat="server" Width="240px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblEntdChkdBy" runat="server" Text="Checked By"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEntdChkdBy" runat="server" Width="235px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="height:20px">
                        <td>
                            <asp:CheckBox ID="chkWitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chkWitnessBy_CheckedChanged" />
                            <asp:Label ID="lblWitnessBy" runat="server" Text="Witness By"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtWitnesBy" Visible="false" runat="server" Width="235px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td align="right">
                            <asp:LinkButton ID="lnkSave" Font-Size="Small" runat="server" OnClick="lnkSave_Click"
                                Font-Underline="true">Save</asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="lnkExit" runat="server" Font-Size="Small" Font-Underline="true"
                                OnClick="lnkExit_Click">Exit</asp:LinkButton>
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="4">
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                                PopupControlID="pnlSampleDetail" PopupDragHandleControlID="PopupHeader" Drag="true"
                                BackgroundCssClass="ModalPopupBG">
                            </asp:ModalPopupExtender>
                            <asp:Panel ID="pnlSampleDetail" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <table class="DetailPopup">
                                            <tr valign="top">
                                                <td align="center" valign="bottom" colspan="3">
                                                    <asp:ImageButton ID="imgCloseSamplePopup" runat="server" ImageUrl="Images/cross_icon.png"
                                                        OnClick="imgCloseSamplePopup_Click" ImageAlign="Right" />
                                                    <asp:Label ID="lblAddSample" runat="server" ForeColor="#990033" Text="Add New Sample"
                                                        Font-Bold="True" Font-Size="Small"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="right" valign="top" style="width: 100px">Sample Name
                                                </td>
                                                <td style="width: 3px">&nbsp;
                                                </td>
                                                <td style="width: 300px">
                                                    <asp:TextBox ID="txtSampleName" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSampleName"
                                                        EnableClientScript="False" ErrorMessage="Input 'Sample Name'" SetFocusOnError="True"
                                                        ValidationGroup="V1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="right" valign="top">Sample Test
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <div style="border-style: solid; border-color: inherit; border-width: thin; overflow: auto; width: 250px; height: 200px">
                                                        <asp:CheckBoxList ID="chkTest" runat="server">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                    <asp:Label ID="lblTest" runat="server" ForeColor="Red" Text="Select at least single test.." Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="center">&nbsp;
                                                    <asp:Label ID="lblSampleMessage" runat="server" ForeColor="#990033" Text="lblMsg"
                                                        Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="right">
                                                    <asp:LinkButton ID="lnkSaveSample" runat="server" CssClass="LnkOver" OnClick="lnkSaveSample_Click"
                                                        Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V1">Save</asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkCancelSample" runat="server" CssClass="LnkOver" OnClick="lnkCancelSample_Click"
                                                        Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V1">Cancel</asp:LinkButton>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="lnkCancelSample" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
