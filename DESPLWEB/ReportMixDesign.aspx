<%@ Page Title="Mix Design Report" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="ReportMixDesign.aspx.cs" Inherits="DESPLWEB.ReportMixDesign" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="100%">
                        <asp:Label ID="lblGrade" runat="server" Text="Grade "></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddl_Grade" runat="server" Width="100px">
                            <asp:ListItem Text="M 5" />
                            <asp:ListItem Text="M 7.5" />
                            <asp:ListItem Text="M 10" />
                            <asp:ListItem Text="M 15" />
                            <asp:ListItem Text="M 20" />
                            <asp:ListItem Text="M 25" />
                            <asp:ListItem Text="M 30" />
                            <asp:ListItem Text="M 35" />
                            <asp:ListItem Text="M 37" />
                            <asp:ListItem Text="M 37.5" />
                            <asp:ListItem Text="M 40" />
                            <asp:ListItem Text="M 45" />
                            <asp:ListItem Text="M 50" />
                            <asp:ListItem Text="M 55" />
                            <asp:ListItem Text="M 60" />
                            <asp:ListItem Text="M 65" />
                            <asp:ListItem Text="M 70" />
                            <asp:ListItem Text="M 75" />
                            <asp:ListItem Text="M 80" />
                            <asp:ListItem Text="M 85" />
                            <asp:ListItem Text="M 90" />
                            <asp:ListItem Text="M 95" />
                            <asp:ListItem Text="M 100" />
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTest" runat="server" Text="Test "></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddl_Test" runat="server" Width="200px">
                            <asp:ListItem Text="Concrete Mix Design" />
                            <asp:ListItem Text="Pumpable Concrete Mix Design" />
                            <asp:ListItem Text="Self Compacting Concrete" />
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lbl600micron" runat="server" Text="600 micron "></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddl_600micron" runat="server" Width="100px">
                            <asp:ListItem Text="---All---" />
                            <asp:ListItem Text="10 - 15" />
                            <asp:ListItem Text="15 - 20" />
                            <asp:ListItem Text="20 - 25" />
                            <asp:ListItem Text="25 - 30" />
                            <asp:ListItem Text="30 - 35" />
                            <asp:ListItem Text="35 - 40" />
                            <asp:ListItem Text="40 - 45" />
                            <asp:ListItem Text="45 - 50" />
                            <asp:ListItem Text="50 - 55" />
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;

                        <asp:LinkButton ID="lnkDisplay" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkDisplay_Click" Font-Bold="True">Display</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 5px">
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlMF" runat="server" ScrollBars="Auto" Height="400px" Width="940px"
                BorderStyle="Solid" BorderColor="AliceBlue">
                <asp:GridView ID="grdMF" skinId="gridviewSkin1" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                    CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                    Width="100%" CellPadding="0" CellSpacing="0">
                    <Columns>
                        <asp:BoundField DataField="Slump" HeaderText="Slump" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left"  />
                        <asp:BoundField DataField="Cement" HeaderText="Cement" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="WCRatio" HeaderText="W/C Ratio" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Water" HeaderText="Water" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="FlyAsh" HeaderText="Fly Ash" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="GGBS" HeaderText="GGBS" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="10mm" HeaderText="10 mm" HeaderStyle-HorizontalAlign="Center" 
                            ItemStyle-HorizontalAlign="Center" /> 
                        <asp:BoundField DataField="20mm" HeaderText="20 mm" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="Sand" HeaderText="Sand" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="300micronPassing" HeaderText="300 micron passing" HeaderStyle-HorizontalAlign="Center" 
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="300micronPlusCement" HeaderText="300 micron + Cement + Flyash + GGBS" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Admixture" HeaderText="Admixture" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Cement4.75mmWaterDivPlasticDensity" HeaderText="(Cement + Flyash + GGBS + Water + (Sand Wt. * 4.75 mm passing)) / Plastic Density" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="3DaysStrength" HeaderText="3 Days Strength" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="7DaysStrength" HeaderText="7 Days Strength" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="28DaysStrength" HeaderText="28 Days Strength" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="AvgCount" HeaderText="Count" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </asp:Panel>
       <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
</asp:Content>
