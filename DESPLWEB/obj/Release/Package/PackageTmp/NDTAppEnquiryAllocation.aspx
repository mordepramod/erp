<%@ Page Title="NDT App Enquiry Allocation" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="NDTAppEnquiryAllocation.aspx.cs" Inherits="DESPLWEB.NDTAppEnquiryAllocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr style="background-color: #ECF5FF;">
                    <td align="right">
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total No of Records : 0 "></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton ID="optPending" Text="Pending" runat="server" GroupName="g1" OnCheckedChanged="optEnquiry_CheckedChanged"
                            AutoPostBack="true" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optAssigned" Text="Assigned" runat="server" GroupName="g1" OnCheckedChanged="optEnquiry_CheckedChanged"
                            AutoPostBack="true" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblUser" runat="server" Text="User : "></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlUser" runat="server" Width="200px">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSave" Font-Bold="true" runat="server" Font-Underline="true"
                            OnClick="lnkSave_Click">Assign Enquiry</asp:LinkButton>
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 23px" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="420px" Width="940px" BorderColor="AliceBlue">
                            <div style="width: 940px;">
                                <div id="GHead">
                                </div>
                                <div style="height: 420px; overflow: auto">
                                    <asp:GridView ID="grdEnquiry" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        DataKeyNames="ENQ_Id" ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="#F7F6F3" CssClass="Grid"
                                        BorderWidth="1px" Width="100%" >
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Enq No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnquiryId" runat="server" Text='<%#Eval("ENQ_Id") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Enquiry Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnquiryDate" runat="server" Text='<%#Eval("ENQ_Date_dt" ,"{0:dd/MM/yy}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Client Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("CL_Name_var") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Site Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSiteName" runat="server" Text='<%#Eval("SITE_Name_var") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Enquiry Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnquiryStatus" runat="server" Text='<%#Eval("ENQ_OpenEnquiryStatus_var") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="User Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("USER_Name_var") %>' />
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
                                </div>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <%--<script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdEnquiry.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdEnquiry.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdEnquiry.ClientID%>').offset().top);

        });
    </script>--%>
</asp:Content>
