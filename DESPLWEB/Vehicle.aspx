<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Vehicle.aspx.cs" Inherits="DESPLWEB.Vehicle" Title="Route" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 473px;">
        <asp:Panel ID="Mainpan" runat="server">
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <table style="width: 100%">
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr style="background-color: #ECF5FF;">
                                <td colspan="2">
                                    <asp:Label ID="lblVehicleTitle" runat="server" Font-Bold="True" Text="Vehicle"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 23px" valign="top">
                                    <asp:Panel runat="server" ID="pnlContainer" ScrollBars="Auto" Height="375px" Width="100%">
                                        <asp:GridView ID="grdVehicle" runat="server" DataKeyNames="Vehicle_Id" AutoGenerateColumns="False"
                                            CellPadding="4" ForeColor="#333333" GridLines="Vertical" BorderColor="#DEDFDE"
                                            BorderWidth="1px" Width="100%" OnRowDataBound="grdVehicle_RowDataBound">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgVehicleInsert" runat="server" OnClick="imgVehicleInsert_Click"
                                                            ImageUrl="Images/AddNewitem.jpg" Height="20px" Width="20px" ToolTip="Add New Vehicle" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgVehicleEdit" runat="server" OnClick="imgVehicleEdit_Click"
                                                            ImageUrl="Images/Edit.jpg" Height="18px" Width="18px" ToolTip="Edit Vehicle" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vehicle Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVehicleId" runat="server" Text='<%#Eval("Vehicle_Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vehicle Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVehicleName" runat="server" Text='<%#Eval("Vehicle_Name_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vehicle No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVehicleNo" runat="server" Text='<%#Eval("Vehicle_No_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vehicle Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVehicleStatus" runat="server" Text='<%#Eval("Vehicle_Status_bit").ToString() == "False" ? "Active" : "Deactive"  %>' />
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
                <tr>
                    <td>
                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                            PopupControlID="Pnl_VehMst" BehaviorID="lbinsert_Click" PopupDragHandleControlID="PopupHeader"
                            Drag="true" BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>
                        <asp:Panel runat="server" ID="Pnl_VehMst">
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
                                        <asp:Label ID="locId" runat="server" Visible="false">Vehicle Id</asp:Label>
                                        <asp:TextBox ID="txtVehicleId" runat="server" ReadOnly="True" Visible="false"></asp:TextBox>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblVehiclId" runat="server" Text="" Visible="false"></asp:Label>
                                            </td>
                                            <td align="right">
                                                &nbsp;
                                                <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" ValidationGroup="vpopup"
                                                    Style="text-decoration: underline; font-weight: bold;" OnClick="lnkSave_Click">Save</asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                Vehicle Name&nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtVehicleName" runat="server" Width="200px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Vehicle Name"
                                                    ControlToValidate="txtVehicleName" ValidationGroup="vpopup"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                Vehicle No&nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtVehicleNo" runat="server" Width="200px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Vehicle No"
                                                    ControlToValidate="txtVehicleNo" ValidationGroup="vpopup"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                Vehicle Status&nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlfVehicleStatus" runat="server" Height="25px" Width="205px">
                                                    <asp:ListItem Text="Active" Value="true" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Deactive" Value="false"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblVehicleMessage" runat="server" ForeColor="#990033" Text="lblMsg"
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
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
