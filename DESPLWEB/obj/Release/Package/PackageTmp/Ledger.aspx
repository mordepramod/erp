<%@ Page Title="Ledger Master" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="Ledger.aspx.cs" Inherits="DESPLWEB.Ledger" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 100%">
                        <asp:RadioButton ID="rdbCategory" Text="Cost Category" runat="server" AutoPostBack="true"
                            GroupName="g1" oncheckedchanged="rdbCategory_CheckedChanged" />&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rdbCostCenter" Text="Cost Center" runat="server" AutoPostBack="true" 
                            GroupName="g1" oncheckedchanged="rdbCostCenter_CheckedChanged" />&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rdbLedger" Text="Ledger" runat="server" AutoPostBack="true"
                            GroupName="g1" oncheckedchanged="rdbLedger_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlCategory" runat="server" Height="380px" ScrollBars="Auto" Width="940px"
                            BorderColor="AliceBlue" BorderStyle="Ridge">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdCategory" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgInsertCategory" runat="server" ImageUrl="Images/AddNewitem.jpg"
                                                        Height="18px" Width="18px" CausesValidation="false" OnCommand="ImgInsertCategory_Click"
                                                        ToolTip="Add Row" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgDeleteCategory" runat="server" CausesValidation="false"
                                                        Height="16px" Width="16px" ImageUrl="Images/DeleteItem.png" OnCommand="ImgDeleteCategory_Click"
                                                        ToolTip="Delete Row" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sr. No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" runat="server" Text="" Width="40px" ></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cost Category">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCategory" MaxLength="200" BorderWidth="0px" Width="670px"
                                                        runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategoryId" runat="server" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                        <asp:Panel ID="pnlCostCenter" runat="server" Height="380px" ScrollBars="Auto" Width="940px"
                            BorderColor="AliceBlue" BorderStyle="Ridge">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdCostCenter" runat="server" AutoGenerateColumns="False" 
                                        SkinID="gridviewSkin" onrowdatabound="grdCostCenter_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgInsertCostCenter" runat="server" ImageUrl="Images/AddNewitem.jpg"
                                                        Height="18px" Width="18px" CausesValidation="false" OnCommand="ImgInsertCostCenter_Click"
                                                        ToolTip="Add Row" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgDeleteCostCenter" runat="server" CausesValidation="false"
                                                        Height="16px" Width="16px" ImageUrl="Images/DeleteItem.png" OnCommand="ImgDeleteCostCenter_Click"
                                                        ToolTip="Delete Row" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sr. No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" runat="server" Text="" Width="40px"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cost Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCostCenter" MaxLength="200" BorderWidth="0px" Width="400px"
                                                        runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cost Category">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlCategory" Width="400px" runat="server"></asp:DropDownList>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCostCenterId" runat="server" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                        <asp:Panel ID="pnlLedger" runat="server" Height="380px" ScrollBars="Auto" Width="940px"
                            BorderColor="AliceBlue" BorderStyle="Ridge">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdLedger" runat="server" AutoGenerateColumns="False" 
                                        SkinID="gridviewSkin" onrowdatabound="grdLedger_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgInsertLedger" runat="server" ImageUrl="Images/AddNewitem.jpg"
                                                        Height="18px" Width="18px" CausesValidation="false" OnCommand="ImgInsertLedger_Click"
                                                        ToolTip="Add Row" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgDeleteLedger" runat="server" CausesValidation="false"
                                                        Height="16px" Width="16px" ImageUrl="Images/DeleteItem.png" OnCommand="ImgDeleteLedger_Click"
                                                        ToolTip="Delete Row" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sr. No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" runat="server" Text="" Width="40px"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ledger">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtLedger" MaxLength="200" BorderWidth="0px" Width="250px"
                                                        runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cost Center">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlCostCenter" Width="250px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCostCenter_SelectedIndexChanged"></asp:DropDownList>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cost Category">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlCategory" Width="250px" runat="server"></asp:DropDownList>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLedgerId" runat="server" Text=""></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:LinkButton ID="lnkSave" Font-Size="Small" runat="server" OnClick="lnkSave_Click"
                            Font-Underline="true">Save</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                        
                        <asp:LinkButton ID="lnkExit" runat="server" Font-Size="Small" Font-Underline="true"
                            OnClick="lnkExit_Click">Exit</asp:LinkButton> &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
