<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="DESPLWEB.ClientHomeMetro" CodeBehind="ClientHomeMetro.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<style>
    body {
        margin: 0px 0px 0px 0px;
        padding: 0;
    }

    .hlable {
        font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
        font-size: small;
        color: #000000;
        margin: 0;
        padding: 0;
        margin-right: 30;
    }

    footer {
        background-color: #111111;
        bottom: 0;
        box-shadow: 0 1px 1px #111111;
        height: 20px;
        left: 0;
        position: fixed;
        width: 100%;
        z-index: 100000;
    }
</style>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 900px; margin: 0 auto 0 auto; text-align: center">
            <center>
            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="hlable">
                <tr>
                    <td align="left" rowspan="2">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/DuroLogo.bmp" />
                    </td>
                    <td style="width: 500px" rowspan="2" colspan="7">
                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/metro-1.jpg" Height="71px" Width="500px" />
                    </td>
                    <td style="width: 100px" align ="left"  rowspan="2" colspan="1">
                        <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Images/mahametro-logo.png" Height="71px" Width="100px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkExit" runat="server" OnClick="lnkExit_Click" Font-Bold="True">Exit</asp:LinkButton>
                        <asp:LinkButton ID="lnkLogOut" runat="server" OnClick="lnkLogOut_Click" Font-Bold="True" Visible="false">Logout</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="9">
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/LogoLine.bmp"  Width="100%"/>
                    </td>
                </tr>
            </table>
        </center>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
        </div>
        <div style="width: 900px; margin: 0 auto 0 auto; text-align: center">
            <asp:Panel ID="Panel1" runat="server" Width="100%" BorderWidth="1px" Height="480px">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table width="100%" class="hlable" style="height: 440px">
                            <tr>
                                <td valign="top" align="right" class="auto-style1"></td>
                                <td class="auto-style2">&nbsp;
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblMetro" runat="server" BorderStyle="None" ForeColor="Blue" Font-Size="Medium"
                                        Width="464px"></asp:Label>
                                    <br />
                                    &nbsp;<br />
                                </td>
                                <td>&nbsp;
                                </td>
                                <td valign="top" align="left"></td>
                            </tr>
                            <tr>
                                <td valign="top" align="right" class="auto-style1">Contractor Name
                                </td>
                                <td class="auto-style2">&nbsp;
                                </td>
                                <%--<td align="left" class="style3">
                    <asp:Label ID="lblclientName" runat="server" BorderStyle="None" ForeColor="Blue"
                        Width="464px"></asp:Label>
                    <br />
                    &nbsp;<br />
                </td>--%>
                                <td align="left">
                                    <asp:DropDownList ID="ddlClient" runat="server" Height="23px" OnSelectedIndexChanged="ddlClient_SelectedIndexChanged"
                                        Width="468px" Style="font-family: 'Helvetica Neue' , 'Lucida Grande' , 'Segoe UI' , 'Arial', 'Helvetica', 'Verdana', 'sans-serif'; font-size: small;"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                    &nbsp;<asp:Label ID="lblClient" runat="server" ForeColor="Red" Text="Select Site Name.." Visible="False"></asp:Label>

                                </td>
                                <td class="style3">&nbsp;
                                </td>
                                <td valign="top" align="left" class="style2">

                                    <asp:Label ID="lblLocation" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                                    <asp:Label ID="lblConnection" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                                    <asp:Label ID="lblConnectionLive" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="right" class="auto-style1">Site Name
                                </td>
                                <td class="auto-style2">&nbsp;
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlSite" runat="server" Height="19px" OnSelectedIndexChanged="ddlSite_SelectedIndexChanged"
                                        Width="468px" Style="font-family: 'Helvetica Neue' , 'Lucida Grande' , 'Segoe UI' , 'Arial', 'Helvetica', 'Verdana', 'sans-serif'; font-size: small;"
                                        AutoPostBack="True">
                                    </asp:DropDownList>

                                    &nbsp;<asp:Label ID="lblSite" runat="server" ForeColor="Red" Text="Select Site Name.." Visible="False"></asp:Label></td>
                                <td>&nbsp;
                                </td>
                                <td valign="top" align="left">
                                    <asp:DropDownList ID="ddl_db" runat="server" AutoPostBack="true" Height="19px" OnSelectedIndexChanged="ddl_db_SelectedIndexChanged" Width="156px">
                                        <asp:ListItem Text="From May 2016" Value="veena2016"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="right" class="auto-style1">Location
                                </td>
                                <td class="auto-style2">&nbsp;
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlBuildings" runat="server" Height="23px"
                                        Width="468px" Style="font-family: 'Helvetica Neue' , 'Lucida Grande' , 'Segoe UI' , 'Arial', 'Helvetica', 'Verdana', 'sans-serif'; font-size: small;"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlBuildings_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;</td>
                                <td>&nbsp;
                                </td>
                                <td valign="top" align="left">
                                    <asp:Label ID="lblBuilding0" runat="server" ForeColor="Red" Text="Select Building.." Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="right" class="auto-style1">Material
                                </td>
                                <td class="auto-style2">&nbsp;
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlMaterial" runat="server" Height="23px" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged"
                                        Width="468px" Style="font-family: 'Helvetica Neue' , 'Lucida Grande' , 'Segoe UI' , 'Arial', 'Helvetica', 'Verdana', 'sans-serif'; font-size: small;">
                                        <asp:ListItem Text="---Select---"></asp:ListItem>
                                        <asp:ListItem Text="Aggregate" Value="AGGT"></asp:ListItem>
                                        <asp:ListItem Text="Cement" Value="CEMT"></asp:ListItem>
                                        <asp:ListItem Text="Concrete Core" Value="CR"></asp:ListItem>
                                        <asp:ListItem Text="Cube" Value="CT"></asp:ListItem>
                                        <asp:ListItem Text="Fly Ash" Value="FLYASH"></asp:ListItem>
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
                                    &nbsp;</td>
                                <td>&nbsp;
                                </td>
                                <td align="left" valign="top">
                                    <asp:Label ID="lblMaterial" runat="server" ForeColor="Red" Text="Select Material.." Visible="False"></asp:Label>
                                </td>
                            </tr>
                           

                            <tr>
                                <td valign="top" align="right" class="auto-style1">Period for 
                                </td>
                                <td class="auto-style2">&nbsp;
                                </td>
                                <td align="left">&nbsp; Month &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlMonth" runat="server" Height="25px"
                        Width="135px" Style="font-family: 'Helvetica Neue' , 'Lucida Grande' , 'Segoe UI' , 'Arial', 'Helvetica', 'Verdana', 'sans-serif'; font-size: small;"
                        AutoPostBack="true" OnSelectedIndexChanged="ddllMonth_SelectedIndexChanged">
                        <asp:ListItem Text="---Select---"></asp:ListItem>
                        <asp:ListItem Text="Jan" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                        <asp:ListItem Text="May" Value="5"></asp:ListItem>
                        <asp:ListItem Text="Jun" Value="6"></asp:ListItem>
                        <asp:ListItem Text="Jul" Value="7"></asp:ListItem>
                        <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                        <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                        <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                        <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                        <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp; Year &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlYear" runat="server" Height="23px"
                        Width="126px" Style="font-family: 'Helvetica Neue' , 'Lucida Grande' , 'Segoe UI' , 'Arial', 'Helvetica', 'Verdana', 'sans-serif'; font-size: small;"
                        AutoPostBack="true" OnSelectedIndexChanged="ddllYear_SelectedIndexChanged">
                        <asp:ListItem Text="---Select---"></asp:ListItem>
                        <asp:ListItem Text="2021" Value="2021"></asp:ListItem>
                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblMonth" runat="server" ForeColor="Red" Text="Select Month/Year." Visible="False"></asp:Label>
                                </td>
                                <td align="left" valign="top">
                                    <asp:Button ID="btnDisplay" runat="server" Font-Bold="True" Font-Size="8pt"
                                        Height="23px" OnClick="btnDisplay_Click" Text="Search" Width="70px" />
                                    &nbsp;&nbsp;
                                     <asp:Button ID="DownloadFiles" runat="server" Font-Bold="True" Font-Size="8pt"
                        Height="23px" Text="Download" Width="70px"
                        Visible="false" OnClick="DownloadFiles_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top" class="auto-style1">
                                    <asp:Label ID="lblMessage" runat="server" Text="Report List"></asp:Label>
                                </td>
                                <td class="auto-style2">&nbsp;
                                </td>
                                <td align="left" colspan="3" class="auto-style2">
                                    <asp:Panel ID="pnlReportList" runat="server" ScrollBars="Auto" Height="190px" Width="740px"
                                        BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">
                                        <asp:GridView ID="grdView" runat="server" AccessKey="2" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#DEBA84" BorderStyle="None"
                                            BorderWidth="1px" CellPadding="3" CellSpacing="2" GridLines="None"
                                            OnPageIndexChanging="grdView_PageIndexChanging" OnSelectedIndexChanged="grdView_SelectedIndexChanged"
                                            OnRowCommand="grdView_RowCommand" PageSize="4" Style="margin-bottom: 0px; margin-right: 0px; margin-top: 0px; font-size: small;"
                                            UseAccessibleHeader="False" Width="660px" OnRowDataBound="grdView_RowDataBound" Height="18px">
                                            <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                                            <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" Height="15px" />
                                            <EmptyDataRowStyle BorderStyle="None" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:BoundField DataField="DateOfReceiving" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Received Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Record_type" HeaderText="Test Type" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="INWD_Building_var" HeaderText="Building Name" NullDisplayText="--" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="CONT_Name_var" HeaderText="Contact Name" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="INWD_ContactNo_var" HeaderText="Contact No" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="RefNo" HeaderText="Report No" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:CommandField ButtonType="Link" SelectText="view" ShowSelectButton="true" HeaderStyle-HorizontalAlign="Center" HeaderText="Report" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
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
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Images">
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
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="grdView" />
                    </Triggers>
                </asp:UpdatePanel>               
                &nbsp;&nbsp;&nbsp;
                
            
            <asp:Panel ID="Panel2" runat="server" Width="100%" HorizontalAlign="Right">
            <asp:Button ID="btnViewBill" runat="server" Font-Bold="True" Font-Size="8pt"
                            Height="23px" OnClick="btnViewBill_Click" Text="View Bill" Width="70px" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   </asp:Panel>
                
                </asp:Panel>
        </div>
        
    </form>
</body>
</html>
