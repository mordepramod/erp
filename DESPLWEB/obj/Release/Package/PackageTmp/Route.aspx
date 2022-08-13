<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Route.aspx.cs" Inherits="DESPLWEB.Route3" Title="Veena - Route Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 473px;">
        <asp:Panel ID="pnlContent" runat="server">
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <table style="width: 100%">
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr style="background-color: #ECF5FF;">
                                <td>
                                    <asp:Label ID="lblRouteTitle" runat="server" Font-Bold="True" Text="Route"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <%--<asp:Label ID="lblSelectedRouteStatus" runat="server" Font-Bold="True" Text="Select Route Status"></asp:Label>
                                <asp:DropDownList ID="DDLSelectedRouteStatus" runat="server" Width="140px" 
                                         AutoPostBack="true" 
                                        onselectedindexchanged="DDLSelectedRouteStatus_SelectedIndexChanged">
                                            <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                                            <asp:ListItem Text="Open" Value="true"></asp:ListItem> 
                                            <asp:ListItem Text="Close" Value="false"></asp:ListItem>
                                </asp:DropDownList>--%>
                                </td>
                                <td align="right">
                                     <a href="RouteUpdate.aspx" class="LnkOver" style="text-decoration: underline;">Update Route From Excel</a>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                     <a href="Vehicle.aspx" class="LnkOver" style="text-decoration: underline;">Update Vehicle</a>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a href="Location.aspx" class="LnkOver" style="text-decoration: underline;">
                                        Assign Location</a>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 23px" valign="top">
                                    <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="230px" Width="100%">
                                        <asp:GridView ID="GrdRouteMst" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            DataKeyNames="Route_Id" ForeColor="#333333" GridLines="Vertical" BorderColor="#DEDFDE"
                                            BorderWidth="1px" Width="100%" OnRowDataBound="GrdRouteMst_RowDataBound">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgRouteInsert" runat="server" OnClick="imgRouteInsert_Click"
                                                            ImageUrl="Images/AddNewitem.jpg" Height="20px" Width="20px" ToolTip="Add New Route" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgRouteEdit" runat="server" OnClick="imgRouteEdit_Click" CommandArgument='<%#Eval("Route_Id")%>'
                                                            ImageUrl="Images/Edit.jpg" Height="18px" Width="18px" ToolTip="Edit Route" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDaySpecific" runat="server" CommandArgument='<%#Eval("Route_Id")%>'
                                                            OnCommand="lnkLoadDaySpecific" Text="Collection Days" class="LnkOver">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Route Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRouteId" runat="server" Text='<%#Eval("Route_Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Route Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRouteName" runat="server" Text='<%#Eval("Route_Name_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Route Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRouteStatus" runat="server" Text='<%#Eval("Route_Status_bit").ToString() == "False" ? "Active" : "Deactive" %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="130px" />
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
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr style="background-color: #ECF5FF;">
                                <td colspan="2">
                                    <asp:Label ID="lblRouteGrdDaySpecific" runat="server" Font-Bold="True" Text="Collection Days"
                                        Visible="False"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblDaySpecificRoute" runat="server" Font-Bold="True" ForeColor="#990033"
                                        Text="Route : " Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Height="160px" Width="100%">
                                        <asp:GridView ID="grdDaySpecific" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#DEDFDE" BorderWidth="1px" CellPadding="4" DataKeyNames="Coll_Route_Id_int"
                                            ForeColor="Black" GridLines="Vertical" Width="100%" OnRowDataBound="grdDaySpecific_RowDataBound">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgInsertDaySpecific" runat="server" CausesValidation="false"
                                                            CommandArgument='<%#Eval("Coll_Day_var") %>' Height="20px" ImageUrl="Images/AddNewitem.jpg"
                                                            OnCommand="imgInsertDaySpecific_Click" ToolTip="Add New Site" Width="20px" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgEditDaySpecific" runat="server" CausesValidation="false"
                                                            CommandArgument='<%#Eval("Coll_Day_var") %>' Height="18px" ImageUrl="Images/Edit.jpg"
                                                            OnCommand="imgEditDaySpecific_Click" ToolTip="Edit Day Speficic" Width="18px" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Day">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollectionDay" runat="server" Text='<%#Eval("Coll_Day_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Route Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDRouteId" runat="server" Text='<%#Eval("Coll_Route_Id_int") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Driver Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDriverId" runat="server" Text='<%#Eval("USER_Name_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vehicle Name and No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVehicleId" runat="server" Text='<%# String.Format("{0} -- {1}", Eval("Vehicle_Name_var"), Eval("Vehicle_No_var")) %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--  <asp:TemplateField HeaderText="Temp Id" Visible="false">
                                                <ItemTemplate>
                                                   <asp:Label ID="lblTempId" runat="server" Text='<%#Eval("Coll_Temp_Id") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
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
                            PopupControlID="Pnl_RouteMaster" PopupDragHandleControlID="PopupHeader" Drag="true"
                            BackgroundCssClass="ModalPopupBG" BehaviorID="lbinsert_Click">
                        </asp:ModalPopupExtender>
                        <asp:Panel runat="server" ID="Pnl_RouteMaster" height="200px" >
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table class="DetailPopup" width="400px" >
                                        <tr>
                                            <td align="center" valign="bottom" colspan="2">
                                                <asp:ImageButton ID="imgClosePopupRoute" runat="server" ImageUrl="Images/cross_icon.png"
                                                    ImageAlign="Right" OnClick="imgClosePopupRoute_Click" />
                                                <asp:Label ID="lblpopuphead" runat="server" Text="" Font-Bold="True" ForeColor="#990033"></asp:Label>
                                            </td>
                                        </tr>
                                        <asp:Label ID="lblPRouteId" runat="server" Visible="false">Route Id</asp:Label>
                                        <asp:TextBox ID="txtRouteId" runat="server" Visible="false" />
                                        <tr>
                                            <td colspan="1">
                                                <asp:Label ID="lblRouteStatus" runat="server" Text="" Visible="false"></asp:Label>
                                            </td>
                                            <td align="right">
                                                &nbsp;
                                                <asp:LinkButton ID="lnkRouteSave" runat="server" CssClass="LnkOver" ValidationGroup="vpopup"
                                                    Style="text-decoration: underline; font-weight: bold;" OnClick="lnkRouteSave_Click">Save</asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                Route Name&nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRouteName" runat="server" Width="200px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please Enter Route Name"
                                                    ControlToValidate="txtRouteName" ValidationGroup="vpopup"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                Route Status&nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlRouteStatus" runat="server" Width="205px">
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
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="#990033" Text="lblMsg" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="height:5px">
                                                &nbsp;
                                            </td>
                                            </tr> 
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="imgClosePopupRoute" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="HiddenField1"
                            PopupControlID="pnlDaySpecifc" BehaviorID="imgInsertDaySpecific_Click" PopupDragHandleControlID="PopupHeader"
                            Drag="true" BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="pnlDaySpecifc" runat="server" BorderWidth="0px" Width="449px">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table class="DetailPopup" width="400px">
                                        
                                        <tr>
                                            <td align="center" valign="bottom" colspan="2">
                                                <asp:ImageButton ID="imgClosePopupDaySpecific" runat="server" ImageUrl="Images/cross_icon.png"
                                                    ImageAlign="Right" OnClick="imgClosePopupDaySpecific_Click" />
                                                <asp:Label ID="lblPopupHeadDaySpecific" runat="server" Font-Bold="True" ForeColor="#990033"
                                                    Text="Add New Day Specific Details"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="1">
                                                <asp:Label ID="lblCollRouteId" runat="server" Text="" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCollDriverId" runat="server" Text="" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCollVehicleId" runat="server" Text="" Visible="false"></asp:Label>
                                            </td>
                                            <td align="right">
                                                &nbsp;
                                                <asp:LinkButton ID="lnkDaySpecificSave" runat="server" CssClass="LnkOver" ValidationGroup="aaa"
                                                    Style="text-decoration: underline; font-weight: bold;" OnClick="lnkDaySpecificSave_Click">Save</asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                Collection Day&nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCollectionDay" runat="server" Width="205px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                Driver&nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDriverName" runat="server" Width="205px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select Driver Name"
                                                    ControlToValidate="ddlDriverName" ValidationGroup="aaa"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" valign="top">
                                                Vehicle&nbsp;
                                            </td>
                                            <td valign="Top">
                                                <asp:DropDownList ID="ddlVehicleNumber" runat="server" Width="205px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDaySpecificMessage" runat="server" ForeColor="#990033" Text="lblMsg"
                                                    Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="imgClosePopupDaySpecific" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
</asp:Content>
