<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Inherits="DESPLWEB.ClientHome" Codebehind="ClientHome.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="Panel1" runat="server" Width="100%" BorderWidth="1px" Height ="540px">
   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>--%>
        <table width="100%" class="hlable">
            <tr>
                <td valign="top" align="right" class="style3">
                    Client Name
                </td>
                <td class="style3">
                    &nbsp;
                </td>
                <td align="left" class="style3">
                    <asp:Label ID="lblclientName" runat="server" BorderStyle="None" ForeColor="Blue"
                        Width="464px"></asp:Label>
                    <br />
                    &nbsp;<br />
                </td>
                <td class="style3">
                    &nbsp;
                </td>
                <td valign="top" align="left" class="style2">
                    
                    <asp:Label ID="lblLocation" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                    <asp:Label ID="lblConnection" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                    <asp:Label ID="lblConnectionLive" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    Site Name
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlSite" runat="server" Height="23px" OnSelectedIndexChanged="ddlSite_SelectedIndexChanged"
                        Width="468px" style="font-family: 'Helvetica Neue' , 'Lucida Grande' , 'Segoe UI' , 'Arial', 'Helvetica', 'Verdana', 'sans-serif';
                                            font-size: small;" AutoPostBack="True">
                    </asp:DropDownList>
                    <br />
                    &nbsp;<asp:Label ID="lblSite" runat="server" ForeColor="Red" Text="Select Site Name.." Visible="False"></asp:Label></td>
                <td>
                    &nbsp;
                </td>
                <td valign="top" align="left">
                    <asp:DropDownList ID="ddl_db" runat="server" AutoPostBack="true" Height="19px" onselectedindexchanged="ddl_db_SelectedIndexChanged" Width="156px">
                        <%--<asp:ListItem Text="From May 2016" Value="veena2016"></asp:ListItem>--%>
                        <asp:ListItem Text="From April 2020" Value="veena2016"></asp:ListItem>
                        <asp:ListItem Text="May 2016 - March 2020" Value="veena2020"></asp:ListItem>
                        <asp:ListItem Text="April 2015 - April 2016" Value="veena2015"></asp:ListItem>
                        <asp:ListItem Text="April 2013 - Mar 2014" Value="veena2013"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    Supplier/Manufacturer
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlSupplier" runat="server" Height="23px"
                        Width="468px" style="font-family: 'Helvetica Neue' , 'Lucida Grande' , 'Segoe UI' , 'Arial', 'Helvetica', 'Verdana', 'sans-serif';
                                            font-size: small;" 
                        AutoPostBack="true" onselectedindexchanged="ddlSupplier_SelectedIndexChanged">
                    </asp:DropDownList>
                    <br />
                    &nbsp;</td>
                <td>
                    &nbsp;
                </td>
                <td valign="top" align="left">
                    
                    <asp:Label ID="lblSupplier0" runat="server" ForeColor="Red" Text="Select Supplier.." Visible="False"></asp:Label>
                    
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    Building
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlBuildings" runat="server" Height="23px"
                        Width="468px" style="font-family: 'Helvetica Neue' , 'Lucida Grande' , 'Segoe UI' , 'Arial', 'Helvetica', 'Verdana', 'sans-serif';
                                            font-size: small;" 
                        AutoPostBack="true" onselectedindexchanged="ddlBuildings_SelectedIndexChanged">
                    </asp:DropDownList>
                    <br />
                    &nbsp;</td>
                <td>
                    &nbsp;
                </td>
                <td valign="top" align="left">
                    <asp:Label ID="lblBuilding0" runat="server" ForeColor="Red" Text="Select Building.." Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    Material
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlMaterial" runat="server" Height="23px" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged"
                        Width="468px" style="font-family: 'Helvetica Neue' , 'Lucida Grande' , 'Segoe UI' , 'Arial', 'Helvetica', 'Verdana', 'sans-serif';
                                            font-size: small;">
                        <asp:ListItem Text="---Select---"></asp:ListItem>
                        <asp:ListItem Text="Aggregate" Value="AGGT"></asp:ListItem>
                        <asp:ListItem Text="Cement" Value="CEMT"></asp:ListItem>
                        <asp:ListItem Text="Concrete Core" Value="CR"></asp:ListItem>
                        <asp:ListItem Text="Cube" Value="CT"></asp:ListItem>
                        <asp:ListItem Text="Fly Ash" Value="FLYASH"></asp:ListItem>
                        <asp:ListItem Text="GGBS" Value="GGBS"></asp:ListItem>
                        <asp:ListItem Text="Masonary Blocks / Bricks" Value="SOLID"></asp:ListItem>
                        <asp:ListItem Text="Mix Design" Value="MF"></asp:ListItem>
                        <asp:ListItem Text="Non Destructive Testing" Value="NDT"></asp:ListItem>
                        <asp:ListItem Text="Pavement Blocks" Value="PT"></asp:ListItem>
                        <asp:ListItem Text="Pile" Value="PILE"></asp:ListItem>
                        <asp:ListItem Text="Soil" Value="SO"></asp:ListItem>
                        <asp:ListItem Text="Steel" Value="ST"></asp:ListItem>
                        <asp:ListItem Text="Tile" Value="TILE"></asp:ListItem>
                        <asp:ListItem Text="Water" Value="WT"></asp:ListItem>
                        <asp:ListItem Text="Other" Value="OT"></asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    &nbsp;<asp:Label ID="lblMaterial" runat="server" ForeColor="Red" Text="Select Material.." Visible="False"></asp:Label></td>
                <td>
                    &nbsp;
                </td>
                <td align ="left" valign ="top">
                    
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    <asp:Label ID="lblTestType" runat="server" Text="Test Type" Visible="False"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlTestType" runat="server" Height="23px" AutoPostBack="true" OnSelectedIndexChanged="ddlTestType_SelectedIndexChanged"
                        Width="468px" style="font-family: 'Helvetica Neue' , 'Lucida Grande' , 'Segoe UI' , 'Arial', 'Helvetica', 'Verdana', 'sans-serif';
                                            font-size: small;" Visible="False">                        
                    </asp:DropDownList>
                    <br />
                    &nbsp;<asp:Label ID="lblTestTypeErr" runat="server" ForeColor="Red" Text="Select Test Type.." Visible="False"></asp:Label></td>
                <td>
                    &nbsp;
                </td>
                <td align ="left" valign ="top">
                    <asp:Button ID="btnDisplay" runat="server" Font-Bold="True" Font-Size="8pt" 
                       Height="27px" OnClick="btnDisplay_Click" Text="Search" Width="75px" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnAddEnquiry" runat="server" Font-Bold="True" Font-Size="8pt" 
                         Height="27px" OnClick="btnAddEnquiry_Click" Text="Add Enquiry" Width="85px" />
                    &nbsp;
                    <asp:Button ID="DownloadFiles" runat="server" Font-Bold="True" Font-Size="8pt" 
                     Height="27px" Text="Download" Width="75px" 
                        Visible="false" onclick="DownloadFiles_Click" />
                </td>
            </tr>
            
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="lblMessage" runat="server" Text="Report List"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left" colspan="3">
                     <asp:Panel ID="pnlReportList" runat="server" ScrollBars="Auto" Height="270px" Width="740px"
                        BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">
                    <asp:GridView ID="grdView" runat="server" AccessKey="2" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#DEBA84" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" CellSpacing="2" GridLines="None" 
                        OnPageIndexChanging="grdView_PageIndexChanging" OnSelectedIndexChanged="grdView_SelectedIndexChanged"
                        OnRowCommand="grdView_RowCommand" PageSize="9" Style="margin-bottom: 0px; margin-right: 0px; margin-top: 0px;"
                        UseAccessibleHeader="False" Width="704px" OnRowDataBound="grdView_RowDataBound">
                        <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" Height="16px" />
                        <EmptyDataRowStyle BorderStyle="None" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="DateOfReceiving" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Received Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="Record_type" HeaderText="Test Type"  HeaderStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="INWD_Building_var" HeaderText="Building Name" NullDisplayText="--"  HeaderStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="CONT_Name_var" HeaderText="Contact Name"  HeaderStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="INWD_ContactNo_var" HeaderText="Contact No"  HeaderStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="Status" HeaderText="Status"  HeaderStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="RefNo" HeaderText="Report No"  HeaderStyle-HorizontalAlign="Center"/>
                            <asp:CommandField ButtonType="Link" SelectText="view" ShowSelectButton="true"  HeaderStyle-HorizontalAlign="Center" HeaderText="Report"/>
                            <asp:TemplateField  ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblTestName" runat="server" Text='<%#Eval("Record_type") %>' Visible="False"></asp:Label>

                                    <asp:LinkButton ID="lnkViewMDLetter" runat="server" ToolTip="View MD Letter" Style="text-decoration: underline;"
                                        CommandArgument='<%#Eval("RefNo") %>' CommandName="lnkViewMDLetter" Visible="false">MD_Letter<br></br></asp:LinkButton>
                                    
                                    <asp:LinkButton ID="lnkViewSieveAnalysis" runat="server" ToolTip="View Sieve Analysis" Style="text-decoration: underline;"
                                        CommandArgument='<%#Eval("RefNo") %>' CommandName="lnkViewSieveAnalysis" Visible="false">Sieve_Analysis<br></br></asp:LinkButton>
                                    
                                         <asp:LinkButton ID="lnkViewMoistureCorrection" runat="server" ToolTip="View Moisture Correction" Style="text-decoration: underline;"
                                        CommandArgument='<%#Eval("RefNo") %>' CommandName="lnkViewMoistureCorrection" Visible="false">Moisture_Correction<br></br></asp:LinkButton>
                                    
                                        <asp:LinkButton ID="lnkViewCoverSheet" runat="server" ToolTip="View Cover Sheet" Style="text-decoration: underline;"
                                        CommandArgument='<%#Eval("RefNo") %>' CommandName="lnkViewCoverSheet" Visible="false">Cover_Sheet<br></br></asp:LinkButton>
                                    
                                    <asp:Label ID="lblMFFinalIssueDt" runat="server" Text='<%#Eval("MFINWD_FinalIssueDt") %>' Visible="False"></asp:Label>
                                    <asp:LinkButton ID="lnkViewFinalReport" runat="server" ToolTip="View Final Report" Style="text-decoration: underline;"
                                        CommandArgument='<%#Eval("RefNo") %>' CommandName="lnkViewFinalReport" Visible="false">Final_Report</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField  ItemStyle-HorizontalAlign="Center" HeaderText="Images">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkViewImages" runat="server" ToolTip="View Images" Style="text-decoration: underline;"
                                        CommandArgument='<%#Eval("RefNo") %>' CommandName="lnkViewImages" Visible="false">View<br></br></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                        <PagerStyle BorderColor="Blue" Font-Bold="True" Font-Italic="False" Font-Names="Arial"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False"
                            ForeColor="Blue" HorizontalAlign="Center" Wrap="True" />
                        <EmptyDataTemplate>
                            No reports to display...
                        </EmptyDataTemplate>
                     <%--   <SelectedRowStyle BackColor="#FFCCFF" Font-Bold="True" ForeColor="White" />--%>
                        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BorderStyle="None" />
                    </asp:GridView>
                    &nbsp;
                    <asp:Label ID="lblReport" runat="server" ForeColor="Red" 
                        Text="Report do not ready .." Visible="False"></asp:Label>
                    &nbsp; &nbsp;
                    </asp:Panel>
                </td>
            </tr>
        </table>
    <%--</ContentTemplate> 
    </asp:UpdatePanel> --%>
    </asp:Panel>
    <%--<asp:ListItem Value="GT">Soil Investigation</asp:ListItem>--%>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

</asp:Content>

