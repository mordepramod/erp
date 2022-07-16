<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Enquiry_List.aspx.cs" Inherits="DESPLWEB.Enquiry_List" Title="Enquiry List" %>
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
                    <td >
                        <asp:Label ID="Label1" runat="server" Text="Inward Type "></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlInwardType" Width="155px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlInwardType_SelectedIndexChanged">
                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total No of Records : 0 "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 23px" valign="top">
                          <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto"  BorderStyle="Ridge"  Height="420px" Width="940px" BorderColor="AliceBlue" > 
                             <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 420px; overflow: auto">
                            <asp:GridView ID="grdEnquiry" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                DataKeyNames="ENQ_Id" ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE"  BackColor="#F7F6F3" CssClass="Grid"
                                BorderWidth="1px" Width="100%" OnRowDataBound="grdEnquiry_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgInsertEnquiry" runat="server" OnCommand="imgInsertEnquiry_Click"
                                                ImageUrl="Images/AddNewitem.jpg" Height="20px" Width="20px" CausesValidation="false"
                                                ToolTip="Add New Enquiry" />
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgEditEnquiry" runat="server" CommandArgument='<%#Eval("ENQ_Id") + ";" + Eval("CL_Id")+ ";" + Eval("SITE_Id")+ ";" + Eval("CONT_Id")  %> '
                                                OnCommand="imgEditEnquiry_Click" ImageUrl="Images/Edit.jpg" Height="18px" Width="18px"
                                                CausesValidation="false" ToolTip="Edit Enquiry" CommandName="EditEnquiry" />
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnInward" runat="server" CommandArgument='<%#Eval("ENQ_Id") + ";" + Eval("MATERIAL_Name_var")%>'
                                                OnCommand="lnkAddInward"  Font-Underline="true"  Text="Add Inward">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="70px" />
                                    </asp:TemplateField>
                                   <%--<asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkProposal" runat="server" CommandArgument='<%#Eval("ENQ_Id") + ";" + Eval("MATERIAL_Name_var")%>'
                                                OnCommand="lnkProposal_OnCommand"  Font-Underline="true"  Text="Proposal">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" />
                                   </asp:TemplateField>--%>
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
                                    <asp:TemplateField HeaderText="Material Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMaterialName" runat="server" Text='<%#Eval("MATERIAL_Name_var") %>' />
                                        </ItemTemplate>
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
                            </div></div>
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
     <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
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
    </script>
</asp:Content>
