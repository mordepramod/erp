﻿<%@ Page Title="Route - Driver Allocation" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="RouteDriverAllocation.aspx.cs" Inherits="DESPLWEB.RouteDriverAllocation" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <asp:Label ID="lblRec" runat="server" Text="Total No of Records : 0"></asp:Label>
                    </td>
                    <td style="width: 40%" align="right">
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>
                    </td>
                    <td style="width: 10%" align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="420px" Width="940px"
                            BorderStyle="Ridge" BorderColor="AliceBlue">
                            <asp:GridView ID="grdRoute" runat="server" AutoGenerateColumns="False" OnRowCreated="grdRoute_RowCreated"
                                BorderWidth="0px" Width="100%" SkinID="gridviewSkin" > <%--OnRowDataBound="RowDataBound"--%>
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="cbxSelectAll" runat="server" onclick="javascript:HeaderClick(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbxSelect" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Route Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRouteName" Text='<%#Eval("ROUTE_Name_var") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="70%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRouteId" Text='<%#Eval("ROUTE_Id") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Driver">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddl_Driver" Width="240px" runat="server">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblDriverId" Text='<%#Eval("ROUTE_Driver_Id") %>' runat="server" Visible="false"/>
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>                        
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        var TotalChkBx;
        var Counter;
        window.onload = function () {
            TotalChkBx = parseInt('<%= this.grdRoute.Rows.Count %>');
            Counter = 0;
        }
        function HeaderClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.grdRoute.ClientID %>');
            var TargetChildControl = "cbxSelect";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBx : 0;

        }
        function ChildClick(CheckBox, HCheckBox) {
            var HeaderCheckBox = document.getElementById(HCheckBox);
            if (CheckBox.checked && Counter < TotalChkBx)
                Counter++;
            else if (Counter > 0)
                Counter--;
            if (Counter < TotalChkBx)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBx)
                HeaderCheckBox.checked = true;

//            if (CheckBox.checked) {
//                CheckBox.parentElement.parentElement.style.backgroundColor = '#FFDFDF';
//            }
//            else {
//                CheckBox.parentElement.parentElement.style.backgroundColor = '#FFFFFF';
//            }
        }
        function CheckAll(Checkbox) {
            var grdRoute = document.getElementById("<%=grdRoute.ClientID %>");
            for (i = 1; i < grdRoute.rows.length; i++) {
                grdRoute.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;

//                if (Checkbox.checked) {
//                    Checkbox.parentElement.parentElement.style.backgroundColor = '#FFDFDF';
//                }
//                else {
//                    Checkbox.parentElement.parentElement.style.backgroundColor = '#FFFFFF';
//                }
            }

        }
    </script>
</asp:Content>
