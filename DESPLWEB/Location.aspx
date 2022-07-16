<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Location.aspx.cs" Inherits="DESPLWEB.Location" Title="Location" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 473px;">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <table style="width: 100%">
            <tr>
                <td>
                    <table style="width: 100%;">
                        <tr style="background-color: #ECF5FF;">
                            <td colspan="2">
                                <asp:Label ID="lblLocationTitle" runat="server" Font-Bold="True" Text="Location"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 23px" valign="top">
                                <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="375px" Width="100%">
                                    <asp:GridView ID="grdLocation" runat="server" DataKeyNames="LOCATION_Id" AutoGenerateColumns="False"
                                        CellPadding="4" ForeColor="#333333" GridLines="Vertical" BorderColor="#DEDFDE"
                                        BorderWidth="1px" Width="100%" OnRowDataBound="grdLocation_RowDataBound">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgLocationInsert" runat="server" OnClick="imgLocationInsert_Click"
                                                        ImageUrl="Images/AddNewitem.jpg" Height="20px" Width="20px" ToolTip="Add New Location" />
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgLocationEdit" runat="server" OnClick="imgLocationEdit_Click"
                                                        ImageUrl="Images/Edit.jpg" Height="18px" Width="18px" ToolTip="Edit Location" />
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Location Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLocationId" runat="server" Text='<%#Eval("LOCATION_Id") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Location Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLocationName" runat="server" Text='<%#Eval("LOCATION_Name_var") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Location Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLocationStatus" runat="server" Text='<%#Eval("LOCATION_Status_bit").ToString() == "False" ? "Active" : "Deactive"  %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Created On">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLocationDate" runat="server" Text='<%#Eval("LOCATION_CreatedOn_dt", "{0:dd/MM/yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                        <EmptyDataTemplate>
                                            No records to display
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblresult" runat="server"></asp:Label>
        <br />
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
            PopupControlID="Pnl_LocMst" BehaviorID="lbinsert_Click" PopupDragHandleControlID="PopupHeader"
            Drag="true" BackgroundCssClass="ModalPopupBG">
        </asp:ModalPopupExtender>
    </div>
    <asp:Panel runat="server" ID="Pnl_LocMst">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="DetailPopup" width="400px">
                    <tr>
                        <td align="center" valign="bottom" colspan="2">
                            <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                                OnClick="imgClosePopup_Click" ImageAlign="Right" />
                            <asp:Label ID="lblpopuphead" runat="server" Text="" Font-Bold="True" ForeColor="#990033"></asp:Label>
                        </td>
                    </tr>
                    <asp:Label ID="locId" runat="server" Visible="false">Location Id</asp:Label>
                    <asp:TextBox ID="txtLocationId" runat="server" ReadOnly="True" Visible="false" />
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="right">
                            &nbsp;
                            <asp:LinkButton ID="lnkLocationSave" runat="server" CssClass="LnkOver" ValidationGroup="vpopup"
                                Style="text-decoration: underline; font-weight: bold;" OnClick="lnkLocationSave_Click">Save</asp:LinkButton>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            Location Name&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtLocationName" runat="server" Width="192px" />
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLocationName"
                                ErrorMessage="Please Enter Locaction Name" ValidationGroup="vpopup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            Location Status&nbsp;
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlLocationStatus" runat="server" Width="195px">
                                <asp:ListItem Text="Active" Value="true" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Deactive" Value="false"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label ID="lblRoute" runat="server" Text="Route"></asp:Label>
                        </td>
                        <td>
                            <asp:Panel ID="chkPnl" runat="server" Style="border-style: solid; border-color: inherit;
                                border-width: thin; overflow: auto; width: 192px; height: 148px">
                                <asp:CheckBoxList ID="chkRoute" runat="server">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblLocationMessage" runat="server" ForeColor="#990033" Text="lblMsg"
                                Visible="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="imgClosePopup" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
