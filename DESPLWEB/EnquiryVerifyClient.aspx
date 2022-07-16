<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="EnquiryVerifyClient.aspx.cs" Inherits="DESPLWEB.EnquiryVerifyClient" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr style="background-color: #ECF5FF;">
                    <td align="right">
                        <%--<asp:Label ID="lblTotalRecords" runat="server" Text="Total No of Records : 0 "></asp:Label>--%>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Inward Type "></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlInwardType" Width="155px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlInwardType_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkLoadAppEnquiry" 
                            Text="Load Enquiry from Mobile Application" runat="server" AutoPostBack="true"
                            oncheckedchanged="chkLoadAppEnquiry_CheckedChanged" Visible="true"/>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total No of Records : 0 "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 23px" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="420px"
                            Width="940px" BorderColor="AliceBlue">
                            <div style="height: 420px; overflow: auto">
                                <asp:GridView ID="grdEnquiry" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    DataKeyNames="ENQNEW_Id" ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE"
                                    BackColor="#F7F6F3" CssClass="Grid" BorderWidth="1px" Width="100%" >
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkVerifyClient" runat="server" CommandArgument='<%#Eval("ENQNEW_Id") + ";" + Eval("CL_Id")+ ";" + Eval("SITE_Id")  %> '
                                                    OnCommand="lnkVerifyClient" Font-Underline="true" Text="Verify Client">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="70px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Enq No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEnquiryId" runat="server" Text='<%#Eval("ENQNEW_Id") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Enquiry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEnquiryDate" runat="server" Text='<%#Eval("ENQNEW_Date_dt" ,"{0:dd/MM/yy}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                       <%-- <asp:TemplateField HeaderText="Material Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMaterialName" runat="server" Text='<%#Eval("MATERIAL_Name_var") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Client Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("ENQNEW_ClientName_var") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Site Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSiteName" runat="server" Text='<%#Eval("ENQNEW_SiteName_var") %>' />
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
            </table>
        </asp:Panel>
    </div>
</asp:Content>
