<%@ Page Title="Sectionwise Email" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="SectionWiseEmail.aspx.cs" Inherits="DESPLWEB.SectionWiseEmail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="480px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="#990033" Text="lblMsg" Visible="False"></asp:Label>
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True" Text="Error: Invalid Data."
                            Visible="False"></asp:Label>
                    </td>
                    <td colspan="4" align="right">
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" OnClick="lnkSave_Click"
                            Style="text-decoration: underline;" ValidationGroup="V1" Font-Bold="True">Save </asp:LinkButton>&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlEmail" runat="server" Height="420" Width="100%" BorderWidth="0px"
                Visible="true" ScrollBars="Vertical">
                <asp:GridView ID="grdSectionEmail" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                    BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" ForeColor="#333333"
                    GridLines="None" Width="99%" CellPadding="1" CellSpacing="1">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:TemplateField HeaderText="Sr.No" ItemStyle-Width="30px">
                            <ItemTemplate>
                                <span>
                                    <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                </span>
                            </ItemTemplate>
                            <ItemStyle Width="30px" />
                        </asp:TemplateField>
                          <asp:TemplateField HeaderText="Material Id" ItemStyle-Width="30px" Visible="false">
                            <ItemTemplate>
                                <span>
                                    <asp:Label ID="lblMaterial_Id" Text='' runat="server" />
                                </span>
                            </ItemTemplate>
                            <ItemStyle Width="30px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Material_Name_var" HeaderText="Material Type" ItemStyle-Width="300px">
                            <ItemStyle Width="300px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Email Id" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtEmailId" runat="server" Text='' BorderWidth="0px" Width="300px" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Width="300px" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                    <PagerStyle BorderColor="Blue" Font-Bold="True" Font-Italic="False" Font-Names="Arial"
                        Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False"
                        ForeColor="Blue" HorizontalAlign="Center" Wrap="True" />
                    <EmptyDataTemplate>
                        No records to display...
                    </EmptyDataTemplate>
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
            </asp:Panel>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
</asp:Content>
