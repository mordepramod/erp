<%@ Page Title="Dispatch Sheet" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="DispatchSheetMEWise.aspx.cs" Inherits="DESPLWEB.DispatchSheetMEWise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            ScrollBars="None" Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 10%">
                        <asp:Label ID="Label3" runat="server" Text="Select Date"></asp:Label>
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="txtDt" Width="150px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtDt">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtDt" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td>
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        <asp:Label ID="lblReport" runat="server" Text="Report for"></asp:Label>
                    </td>
                    <td style="width: 15%">
                        <asp:DropDownList ID="ddlReportFor" runat="server" Width="150px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlReportFor_SelectedIndexChanged">
                            <asp:ListItem>--Select--</asp:ListItem>
                            <asp:ListItem>RouteWise</asp:ListItem>
                            <asp:ListItem>MeWise</asp:ListItem>
                        </asp:DropDownList>
                        <%--   <asp:RadioButton ID="rbRouteWise" Checked="true" runat="server" GroupName="R1"
                            AutoPostBack="true" Text="Routewise Dispatch Sheet" OnCheckedChanged="rbRouteWise_CheckedChanged" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rbMeWise" runat="server" GroupName="R1" AutoPostBack="true"
                            Text="MEwise Dispatch Sheet" OnCheckedChanged="rbMeWise_CheckedChanged" />
                            <asp:Label ID="lblUserId" runat="server" Text="" Visible="false"></asp:Label>--%>
                    </td>
                    <td style="width: 8%">
                        <asp:Label ID="lblMe" runat="server" Text="List of Route"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_ME_Route" runat="server" Width="300px" OnSelectedIndexChanged="ddl_ME_Route_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp; &nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td>
                        <asp:Label ID="lblUserId" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblCount" runat="server" Text="Total No of Records : 0"></asp:Label>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Print</asp:LinkButton>
                    </td>
                </tr>
                <%-- <tr style="background-color: #ECF5FF;">
                    <td colspan="4" align="right">
                          
                    </td>
                    <td align="right">
                       
               </td>
                </tr>--%>
                <tr id="RouteDDL">
                    <td colspan="7" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="390px" Width="940px"
                            BorderStyle="Ridge" BorderColor="AliceBlue">
                            <asp:GridView ID="grdReports" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                                CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" Width="100%"
                                CellPadding="4">
                                <Columns>
                                    <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" Width="210px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MISRecType" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MISRecordNo" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MISRefNo" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MISCollectionDt" HeaderText="Collection Date" DataFormatString="{0:dd/MM/yyyy}"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MISRecievedDt" HeaderText="Received Date" DataFormatString="{0:dd/MM/yyyy}"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Route_Name_Var" HeaderText="Route Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CL_AccContactNo_var" HeaderText="Contact No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                                    </asp:BoundField>
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
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
</asp:Content>
