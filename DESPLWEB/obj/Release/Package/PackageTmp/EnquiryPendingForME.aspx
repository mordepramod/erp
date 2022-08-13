<%@ Page Title="Pending Enquiry List" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="EnquiryPendingForME.aspx.cs" Inherits="DESPLWEB.EnquiryPendingForME"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:LinkButton ID="lnkFetch" runat="server" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkFetch_Click" Visible="false">Display</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblSuperAdmin" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="lblME" runat="server" Text="Marketing Executive"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlME" Width="140px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlME_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Inward Type "></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlInwardType" Width="155px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlInwardType_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="Enquiry entered from "></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlEnteredFrom" Width="100px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlEnteredFrom_SelectedIndexChanged">
                            <%--<asp:ListItem Text="---Select---" Value=""></asp:ListItem>
                            <asp:ListItem Text="Mobile App"></asp:ListItem>
                            <asp:ListItem Text="Web"></asp:ListItem>--%>
                        </asp:DropDownList>
                        
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkPrintEnq" runat="server"  OnClick="lnkPrintEnq_Click">Print</asp:LinkButton>
                        &nbsp;
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total Records : 0 "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 23px" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="200px"
                            Width="940px" BorderColor="AliceBlue">
                        <%--    <div style="width: 940px;">
                                <div id="GHead">
                                </div>
                                <div style="height: 200px; overflow: auto">--%>
                                    <asp:GridView ID="grdEnquiry" runat="server" AutoGenerateColumns="False" CellPadding="1"
                                        ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="#F7F6F3"
                                        CssClass="Grid" BorderWidth="1px" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="ENQ_Id" HeaderText="Enquiry_No" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ENQ_Date_dt" HeaderText="Enquiry_Date" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                            <asp:BoundField DataField="MATERIAL_Name_var" HeaderText="Material" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CL_Name_var" HeaderText="Client_Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="SITE_Name_var" HeaderText="Site_Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ENQ_OpenEnquiryStatus_var" HeaderText="Enquiry_Status" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />                                           
                                           <asp:BoundField DataField="EnteredFrom" HeaderText="Entered_From" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="EnteredByUser" HeaderText="Entered_By" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MEName" HeaderText="Marketing_Executive" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <%--<asp:TemplateField HeaderText="Enq_No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnquiryId" runat="server" Text='<%#Eval("ENQ_Id") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Enquiry_Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnquiryDate" runat="server" Text='<%#Eval("ENQ_Date_dt" ,"{0:dd/MM/yyyy}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Material_Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaterialName" runat="server" Text='<%#Eval("MATERIAL_Name_var") %>' />
                                                    <asp:Label ID="lblMaterialId" runat="server" Text='<%#Eval("ENQ_MATERIAL_Id") %>'
                                                        Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Client_Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("CL_Name_var") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Site_Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSiteName" runat="server" Text='<%#Eval("SITE_Name_var") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Enquiry_Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnquiryStatus" runat="server" Text='<%#Eval("ENQ_OpenEnquiryStatus_var") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entered_From">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnteredFrom" runat="server" Text='<%#Eval("EnteredFrom") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entered_By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnteredByUser" runat="server" Text='<%#Eval("EnteredByUser") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Marketing_Executive">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMEName" runat="server" Text='<%#Eval("MEName") %>' />
                                                    <asp:Label ID="lblMEId" runat="server" Text='<%#Eval("ROUTE_ME_Id") %>' Visible="false" />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>--%>
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
                               <%-- </div>
                            </div>--%>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>                        
                        <asp:Label ID="lblMERpt" runat="server" Text="Marketing Executive"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlMERpt" Width="140px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMERpt_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label4" runat="server" Text="Inward Type "></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlInwardTypeRpt" Width="155px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlInwardTypeRpt_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkPrintRpt" runat="server"  OnClick="lnkPrintRpt_Click">Print</asp:LinkButton>
                        &nbsp;
                        <asp:Label ID="lblTotalRecordsRpt" runat="server" Text="Total Records : 0 "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 23px" valign="top">
                        <asp:Panel ID="pnlReports" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="200px"
                            Width="940px" BorderColor="AliceBlue">
                            <div style="width: 940px;">
                                <div id="GHead2">
                                </div>
                                <div style="height: 200px; overflow: auto">
                                    <asp:GridView ID="grdReports" runat="server" AutoGenerateColumns="False" CellPadding="1"
                                        ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="#F7F6F3"
                                        CssClass="Grid" BorderWidth="1px" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="CL_Name_var" HeaderText="Client_Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="SITE_Name_var" HeaderText="Site_Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CL_Limit_mny" HeaderText="Credit_Limit" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:f2}"/>                                            
                                            <asp:BoundField DataField="CL_BalanceAmt_mny" HeaderText="Balance_Amt" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:f2}"/>                                            
                                            <asp:BoundField DataField="MATERIAL_Name_var" HeaderText="Material_Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />                                           
                                           <asp:BoundField DataField="MISRecordNo" HeaderText="Record_No" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="MISRefNo" HeaderText="Reference_No" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />                                            
                                            <asp:BoundField DataField="MISCollectionDt" HeaderText="Collection_Date" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                            <asp:BoundField DataField="MISRecievedDt" HeaderText="Received_Date" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                            <asp:BoundField DataField="MEName" HeaderText="Marketing_Executive" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <%--<asp:TemplateField HeaderText="Client_Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("CL_Name_var") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Site_Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSiteName" runat="server" Text='<%#Eval("SITE_Name_var") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Credit_Limit">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClientLimit" runat="server" Text='<%#Eval("CL_Limit_mny") %>' />
                                                </ItemTemplate>      
                                                <ItemStyle HorizontalAlign="Center" />                                          
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance_Amt">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalanceAmt" runat="server" Text='<%#Eval("CL_BalanceAmt_mny") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Material_Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaterialName" runat="server" Text='<%#Eval("MATERIAL_Name_var") %>' />
                                                    <asp:Label ID="lblMaterialId" runat="server" Text='<%#Eval("MATERIAL_Id") %>'
                                                        Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Record_No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSetOfRecord" runat="server" Text='<%#Eval("MISRecordNo") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reference_No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReferenceNo" runat="server" Text='<%#Eval("MISRefNo") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Collection_Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCollectionDate" runat="server" Text='<%#Eval("MISCollectionDt" ,"{0:dd/MM/yyyy}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Received_Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceivedDate" runat="server" Text='<%#Eval("MISRecievedDt" ,"{0:dd/MM/yyyy}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Marketing_Executive">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMEName" runat="server" Text='<%#Eval("MEName") %>' />
                                                    <asp:Label ID="lblMEId" runat="server" Text='<%#Eval("ROUTE_ME_Id") %>' Visible="false" />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>--%>
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

            var gridHeader2 = $('#<%=grdReports.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader2).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdReports.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader2).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead2").append(gridHeader2);
            $('#GHead2').css('position', 'absolute');
            $('#GHead2').css('top', $('#<%=grdReports.ClientID%>').offset().top);
        });
    </script>
</asp:Content>
