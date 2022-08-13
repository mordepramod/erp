<%@ Page Title="App Login Approval" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="WebMobileUsers.aspx.cs" Inherits="DESPLWEB.WebMobileUsers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="Panel1" runat="server" Width="100%" BorderWidth="1px" Height="540px">
        <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>--%>
        <table width="100%" class="hlable">
            <tr>
                <td valign="top" align="left" class="style3" colspan="3">
                    &nbsp;
                    <asp:Label ID="Label1" runat="server" BorderStyle="None" Font-Bold="true" Text="Mobile Users List"
                        ForeColor="Black" Width="464px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <br />
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="margin-left: 10px;">
                    &nbsp;&nbsp;&nbsp;Client Name
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <asp:Label ID="lblclientName" runat="server" BorderStyle="None" ForeColor="#000066"
                        Width="464px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" style="margin-left: 10px;">
                    &nbsp;&nbsp;
                </td>
                <td align="right">
                    <%--<asp:LinkButton ID="lnkSave" runat="server" Font-Bold="True" 
                        Style="text-decoration: underline; margin-right: 10px" OnClick="lnkSave_Click">Save</asp:LinkButton>--%>
                    <asp:Button ID="btnSave" runat="server" Font-Bold="True" ForeColor="Black" OnClick="btnSave_Click"
                        Text="Save" Width="69px" Height="27px" />
                    &nbsp; &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" colspan="3">
                    <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                        CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                        Style="margin-left: 10px; margin-right: 10px;" Width="98%" CellPadding="0" CellSpacing="0">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgInsert" runat="server" OnClick="imgInsert_Click" ImageUrl="Images/AddNewitem.jpg"
                                        Height="20px" Width="20px" ToolTip="Add New Route" />
                                </ItemTemplate>
                                <ItemStyle Width="20px" />
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="DL_ContPerson_var" HeaderText="Login Name" />--%>
                            <asp:TemplateField HeaderText="Login Name" HeaderStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtContcPerson" runat="server" BorderWidth="0px" Width="95%" Text='<%#Eval("DL_ContPerson_var") %>'
                                        Style="text-align: left" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--   <asp:BoundField DataField="DL_ContEmail_var" HeaderText="Mail Id" />--%>
                            <asp:TemplateField HeaderText="Mail Id" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMailId" runat="server" BorderWidth="0px" Width="95%" Text='<%#Eval("DL_ContEmail_var") %>'
                                        Style="text-align: left" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--   <asp:BoundField DataField="DL_ContNo_var" HeaderText="Contact No" />--%>
                            <asp:TemplateField HeaderText="Contact No" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtContact" runat="server" BorderWidth="0px" Width="95%" Text='<%#Eval("DL_ContNo_var") %>'
                                        onkeypress="return validatenumerics(event);" MaxLength="10" Style="text-align: left" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Site">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddl_Site" Width="240px" runat="server">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblSiteId" Text='<%#Eval("DL_Site_id") %>' runat="server" Visible="false" />
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bill Approval" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkBillApproval" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="10%" HorizontalAlign="Center"/>
                            </asp:TemplateField>
                            <%-- <asp:BoundField DataField="DL_Status_bit" HeaderText="Status" />--%>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddl_Status" Width="120px" runat="server">
                                        <asp:ListItem Value="Active">Active</asp:ListItem>
                                        <asp:ListItem Value="Deactive">Deactive</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblStatus" Text='<%#Eval("DL_Status_bit") %>' runat="server" Visible="false" />
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDLId" Text='<%#Eval("DL_Id") %>' runat="server" Visible="false" />
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
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <br />
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                    <asp:Label ID="lblLocation" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                    <asp:Label ID="lblConnection" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                    <asp:Label ID="lblClId" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                    <asp:Label ID="lblEmail" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
        <%--</ContentTemplate> 
    </asp:UpdatePanel> --%>
    </asp:Panel>
    <script type="text/javascript" language="javascript">
        function validatenumerics(key) {
            //getting key code of pressed key
            var keycode = (key.which) ? key.which : key.keyCode;
            //comparing pressed keycodes

            if (keycode > 31 && (keycode < 48 || keycode > 57)) {
                alert(" You can enter only characters 0 to 9 ");
                return false;
            }
            else return true;


        }
    </script>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="head">
</asp:Content>
