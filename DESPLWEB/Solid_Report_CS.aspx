﻿<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    Title="Solid Masonary CS Report" CodeBehind="Solid_Report_CS.aspx.cs" Inherits="DESPLWEB.Solid_Masonary_CS"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <div align="right">
                <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                    OnClick="imgClosePopup_Click" ImageAlign="Right" />
            </div>
            <asp:Panel ID="pnlDetails" runat="server" Width="942px" BorderWidth="1px">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 12%">
                            <asp:Label ID="lbl_OtherPending" runat="server" Text="Other Pending Reports:" ></asp:Label>
                        </td>
                        <td style="width: 25%">
                            <asp:DropDownList ID="DrpRefNo" runat="server" AutoPostBack="true" Height="24px"
                                Width="190px">
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="LnkbtnFetch" runat="server" Font-Bold="true" Style="text-decoration: underline;"
                                OnClick="LnkbtnFetch_Click">Fetch</asp:LinkButton>
                        </td>
                        <td style="width: 10%">
                            <asp:Label ID="LblReportNo" runat="server" Text="Report No:"></asp:Label>
                        </td>
                        <td width="30%">
                            <asp:TextBox ID="TxtreportNoSolid" runat="server" ReadOnly="true" Text="SOLID" Width="47px"></asp:TextBox>
                            &nbsp;-&nbsp;&nbsp;
                            <asp:TextBox ID="TxtReportNo" runat="server" ReadOnly="true" Width="141px"></asp:TextBox>
                            <asp:Label ID="lblEnter" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblRecordNo" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblTestId" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td>
                            <asp:Label ID="LblRefNo" runat="server" Text="Reference No:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtRefNo" runat="server" ReadOnly="true" Width="228px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Supplier Name:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtSupplierName" runat="server" OnTextChanged="TxtSupplierName_TextChanged"
                                Width="213px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Description:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDesc" runat="server" Width="228px" OnTextChanged="TxtDesc_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Date Of Testing:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDateOfTest" runat="server" ClientIDMode="Static" AutoPostBack="true"
                                OnTextChanged="TxtDateOfTest_TextChanged" onClick="testDatePicker()" Width="118px"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;<asp:Label ID="Label6" runat="server"
                                Text="Qty:"></asp:Label>&nbsp;<asp:TextBox ID="TxtQty" runat="server" ReadOnly="true"
                                    Width="39px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Date Of Casting:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtDateOfCast" Width="228px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                         <asp:Label ID="Label3" runat="server" Text="Nabl Scope"></asp:Label>
                        </td>
                        <td   colspan="2">
                               &nbsp;<asp:DropDownList ID="ddl_NablScope" AutoPostBack="true" Width="80px" runat="server">
                                <asp:ListItem Text="--Select--" />
                                <asp:ListItem Text="F" />
                                <asp:ListItem Text="NA" />
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                                    <asp:Label ID="Label8" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>
                            <asp:DropDownList ID="ddl_NABLLocation" runat="server" Width="80px" Enabled="true">
                                <asp:ListItem Value="0" Text="0" />
                                <asp:ListItem Value="1" Text="1" />
                                <asp:ListItem Value="2" Text="2" />
                            </asp:DropDownList>
                            &nbsp;
                            <asp:LinkButton ID="lnkGetData" OnClick="lnkGetAppData_Click" runat="server" Font-Bold="True"
                                        Style="text-decoration: underline;">Get App Data</asp:LinkButton>&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="pnlSolid" runat="server" ScrollBars="Auto" Height="160px" Width="942px"
                BorderWidth="1px">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                <asp:GridView ID="GrdViewCS" runat="server" AutoGenerateColumns="false" SkinID="gridviewSkin">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr No." ControlStyle-Width="40px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="srnotxt" CssClass="textAlign" runat="server" ReadOnly="true" AutoPostBack="true"
                                    BorderWidth="0px" BackColor="Transparent" Text=" <%#Container.DataItemIndex+1 %>">  </asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="40px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID Mark" ControlStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="lblIdMark" runat="server" AutoPostBack="true" BackColor="Transparent"
                                    BorderWidth="0px" OnTextChanged="lblIdMark_TextChanged"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="150px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Age" ControlStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox BackColor="Transparent" CssClass="textAlign" ID="LblAge" runat="server"
                                    BorderWidth="0px" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="100px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Length" ControlStyle-Width="80px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtLength" runat="server" CssClass="textAlign" BackColor="Transparent"
                                    BorderWidth="0px" OnTextChanged="TxtLength_TextChanged" onkeyup="checkNum(this);"
                                    AutoPostBack="true"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="80px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Width" ControlStyle-Width="80px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtWidth" runat="server" CssClass="textAlign" BackColor="Transparent"
                                    BorderWidth="0px" OnTextChanged="TxtWidth_TextChanged" onkeyup="checkNum(this);"
                                    AutoPostBack="true"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="80px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Load" ControlStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtLoad" runat="server" CssClass="textAlign" BackColor="Transparent"
                                    BorderWidth="0px" OnTextChanged="TxtLoad_TextChanged" onkeyup="checkDecimal(this)"
                                    AutoPostBack="true"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="100px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Area" ControlStyle-Width="140px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtArea" runat="server" CssClass="textAlign" ReadOnly="true" BackColor="LightGray"
                                    BorderWidth="0px"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="140px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Strength" ControlStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtStrength" runat="server" CssClass="textAlign" ReadOnly="true"
                                    BorderWidth="0px" BackColor="LightGray"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="100px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Average" ControlStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="TxtAvg" runat="server" CssClass="textAlign" BackColor="LightGray"
                                    BorderWidth="0px" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="100px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="LblID" runat="server"></asp:Label>
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
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GrdRemark" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                            <Columns>
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:ImageButton ImageUrl="~/Images/AddNewitem.jpg" ID="ButtonAddNewRow" runat="server"
                                            OnClick="ButtonAddNewRow_Click" Width="18px" ImageAlign="Middle" />
                                        <asp:ImageButton ImageUrl="~/Images/DeleteItem.png" ID="ButtonDeleteNewRow" runat="server"
                                            OnClick="ButtonDeleteRow_Click" Width="16px" ImageAlign="Middle" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TxtRemark" BorderWidth="0px" runat="server" Width="870px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <%--<div style="margin-bottom: auto;">
                            <asp:CheckBox ID="ChkboxWitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="ChkboxWitnessBy_CheckedChanged" />
                            <asp:Label ID="Label9" runat="server" Text="Witness By:"></asp:Label>
                            &nbsp;
                            <asp:TextBox ID="TxtWitnesBy" Visible="false" runat="server" Width="228px"></asp:TextBox>
                        </div>--%>
            <div style="width: 100%;">
                <table width="100%">
                    <tr style="height: 30px">
                        <td width="15%">
                            <asp:CheckBox ID="ChkboxWitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="ChkboxWitnessBy_CheckedChanged" />
                            <asp:Label ID="Label7" runat="server" Text="Witness By"></asp:Label>
                        </td>
                        <td width="22%">
                            <asp:TextBox ID="TxtWitnesBy" Width="200px" runat="server" Visible="false"></asp:TextBox>
                        </td>
                        <td width="20%" style="text-align: right">
                            <asp:Label ID="lbl_TestedBy" runat="server" Text="Tested By"></asp:Label>
                        </td>
                        <td>
                            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                            <asp:DropDownList ID="ddl_TestedBy" Width="205px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <%-- <asp:LinkButton ID="Lnk_Calculate" OnClick="Lnk_Calculate_Click" runat="server"  Font-Bold="True" Style="text-decoration: underline;">Cal</asp:LinkButton>--%>
                            <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                                Style="text-decoration: underline;">Save</asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" Visible="false" runat="server"
                                   Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                                OnClick="lnk_Exit_Click" runat="server">Exit</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
                <%--<table style="width: 100%">
                                <tr>
                                    <td style="width: 522px">
                                        <asp:Label ID="Label7" runat="server" Text="Approved By :"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="DrpApprovedBy" runat="server" Height="27px" Width="233px">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right" style="width: 707px">
                                        <asp:Label ID="Label8" runat="server" Text="Checked By :"></asp:Label>
                                        &nbsp;
                                      <asp:TextBox ID="TxtcheckBy" runat="server" Width="228px" ReadOnly="true"></asp:TextBox>
                                 
                                    </td>
                                    <td align="left" style="width: 50px">
                                        <asp:LinkButton ID="BtnSave"  Font-Bold="true" runat="server" OnClick="BtnSave_Click"
                                            Font-Underline="true">Save</asp:LinkButton>
                                    </td>
                                    <td style="width: 50px">
                                        <asp:UpdatePanel ID="u2" runat="server">
                                            <ContentTemplate>
                                                <asp:LinkButton ID="BtnPrint" runat="server"  Font-Bold="true" OnClick="BtnPrint_Click"
                                                    Font-Underline="true">Print</asp:LinkButton>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="BtnPrint" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td style="width:50px">
                                        <asp:LinkButton ID="BtnExit" runat="server"  Font-Bold="true" OnClick="BtnExit_Click"
                                            Font-Underline="true">Exit</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>--%>
            </div>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        function testDatePicker() {
            $("#TxtDateOfTest").datepicker({ minDate: -10000, maxDate: "+0D" }).focus();
        }
        $(function () {
            $("#TxtDateOfTest").datepicker({ minDate: -10000, maxDate: "+0D" });
        });


        function checkNum(x) {
            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    x.value = '';
                    x.focus();
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Numeric Values Allowed";
                    return false;
                }
                else {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                }
            }
            return true;
        }
        function checkDecimal(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 46))) {
                    x.value = '';
                    x.focus();
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Decimal Values Allowed";
                    return false;
                }
                else {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                }

            }
            return true;
        }   

    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
