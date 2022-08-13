<%@ Page Title="Bill Approval" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ClientBillApproval.aspx.cs" Inherits="DESPLWEB.ClientBillApproval" Theme="duro" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="Panel1" runat="server" Width="100%" BorderWidth="1px" Height ="540px">
   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>--%>
    <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="530px" Width="940px">
        <table width="100%" class="hlable">
            <tr>
                <td valign="top" align="right" class="style3">
                    Client Name
                </td>
                <td class="style3">
                    &nbsp;
                </td>
                <td align="left" class="style3">
                    <asp:Label ID="lblClientName" runat="server" BorderStyle="None" ForeColor="Blue"
                        Width="464px"></asp:Label>
                    <%--<br />--%>
                    &nbsp;
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
                    <%--<br />--%>
                    &nbsp;<asp:Label ID="lblSite" runat="server" ForeColor="Red" Text="Select Site .." Visible="False"></asp:Label></td>
                <td>
                    &nbsp;
                </td>
                <td valign="top" align="left">
                    <asp:DropDownList ID="ddl_db" runat="server" Width="156px" Visible="false">
                        <asp:ListItem Text="From May 2016" Value="veena2016"></asp:ListItem>
                        <%--<asp:ListItem Text="April 2015 - April 2016" Value="veena2015"></asp:ListItem>
                        <asp:ListItem Text="April 2013 - Mar 2014" Value="veena2013"></asp:ListItem>--%>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    Month
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="txtForMonth" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="MM/yyyy" CssClass="orange" TargetControlID="txtForMonth">
                        </asp:CalendarExtender>
                        <%--<asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtForMonth" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>--%>
                    <%--<br />--%>
                    &nbsp;<asp:Label ID="lblMonth" runat="server" ForeColor="Red" Text="Select Month.." Visible="False"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td valign="top" align="left">
                    <asp:Label ID="lblMOUFileName" runat="server" Text="" Visible="false"></asp:Label>&nbsp;
                    <asp:LinkButton ID="lnkDownloadMOUFile" runat="server" Text="Download MOU" Visible="false"
                        ToolTip="Download File" Style="text-decoration: underline;" onclick="lnkDownloadMOUFile_Click"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    &nbsp;</td>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    &nbsp; &nbsp;
                    <asp:RadioButton ID="optAll" Text="All" GroupName="g1" runat="server" 
                        oncheckedchanged="optAll_CheckedChanged" AutoPostBack="true"/>
                    &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:RadioButton ID="optPending" Text="Pending" GroupName="g1" runat="server" 
                        oncheckedchanged="optPending_CheckedChanged" AutoPostBack="true"/>
                    &nbsp; &nbsp;
                    <asp:RadioButton ID="optApproved" Text="Approved" GroupName="g1" runat="server" 
                        oncheckedchanged="optApproved_CheckedChanged" AutoPostBack="true"/>
                    &nbsp; &nbsp;&nbsp; &nbsp;
                    <asp:Button ID="btnDisplay" runat="server" Font-Bold="True" Font-Size="8pt" 
                       Height="27px" OnClick="btnDisplay_Click" Text="Search" Width="75px" />
                        &nbsp;&nbsp;
                   
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td valign="top" align="left">
                    
                </td>
            </tr>
            
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="lblBillList" runat="server" Text="Bill List"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left" colspan="3">
                    <asp:GridView ID="grdBill" runat="server" AccessKey="2" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#DEBA84" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" CellSpacing="2" GridLines="None" 
                        OnPageIndexChanging="grdBill_PageIndexChanging" OnRowCommand="grdBill_RowCommand"
                        PageSize="9" Style="margin-bottom: 0px; margin-right: 0px; margin-top: 0px;"
                        UseAccessibleHeader="False" Width="704px" >
                        <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" Height="16px" />
                        <EmptyDataRowStyle BorderStyle="None" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="BILL_Date_dt" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Date" />
                            <asp:BoundField DataField="BILL_Id" HeaderText="Bill No." />
                            <%--<asp:CommandField ButtonType="Link" SelectText="View" ShowSelectButton="true" HeaderText="PO"/>
                            <asp:CommandField ButtonType="Link" SelectText="View" ShowSelectButton="true" HeaderText="Test Report" ItemStyle-HorizontalAlign="Center"/>
                            <asp:CommandField ButtonType="Link" SelectText="View" ShowSelectButton="true" HeaderText="Bill"/>
                            <asp:CommandField ButtonType="Link" SelectText="Approve" ShowSelectButton="true" HeaderText="Approve"/>--%>

                            <asp:TemplateField HeaderText="PO">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkViewPO" runat="server" ToolTip="View PO" Style="text-decoration: underline;"
                                        CommandArgument='<%#Eval("Bill_Id") %>' CommandName="lnkViewPO">&nbsp;View&nbsp;</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Test Report" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkViewTestReport" runat="server" ToolTip="View Test Report" Style="text-decoration: underline;"
                                        CommandArgument='<%#Eval("Bill_Id") %>' CommandName="lnkViewTestReport">&nbsp;View&nbsp;</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bill">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkViewBill" runat="server" ToolTip="View Bill" Style="text-decoration: underline;"
                                        CommandArgument='<%#Eval("Bill_Id") %>' CommandName="lnkViewBill">&nbsp;View&nbsp;</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approve">
                                <ItemTemplate>
                                    <asp:Label ID="lblBillApprStatus" runat="server" Text='<%#Eval("BILL_ClientApproveStatus_bit") %>' Visible="False"></asp:Label>
                                    <asp:LinkButton ID="lnkApproveBill" runat="server" ToolTip="Approve Bill" Style="text-decoration: underline;"
                                        CommandArgument='<%#Eval("Bill_Id") %>' CommandName="lnkApproveBill">&nbsp;Approve&nbsp;</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BILL_ClientApprovedBy" HeaderText="Approved By" />
                            <asp:BoundField DataField="BILL_ClientApprovedByTel" HeaderText="Tel" />
                            <asp:BoundField DataField="BILL_ClientApproveDate_dt" HeaderText="Date and time" DataFormatString="{0:dd/MM/yyyy hh:mm:tt}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                        <PagerStyle BorderColor="Blue" Font-Bold="True" Font-Italic="False" Font-Names="Arial"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False"
                            ForeColor="Blue" HorizontalAlign="Center" Wrap="True" />
                        <EmptyDataTemplate>
                            No bills to display...
                        </EmptyDataTemplate>
                     
                        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BorderStyle="None" />
                    </asp:GridView>
                    &nbsp;
                    <asp:Label ID="lblBillMsg" runat="server" ForeColor="Red" 
                        Text="Bill list not available .." Visible="False"></asp:Label>
                    &nbsp; &nbsp;
                </td>
            </tr>
             <tr>
                <td valign="top" align="right">
                    <asp:Label ID="lblSelectedBillNo1" runat="server" Text="Bill No." Visible="False"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left">
                    <asp:Label ID="lblSelectedBillNo" runat="server" ForeColor="Blue" Text="" Visible="False"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblSelectedBillDate1" runat="server" Text="Bill Date" Visible="False"></asp:Label>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblSelectedBillDate" runat="server" ForeColor="Blue" Text="" Visible="False"></asp:Label>
                </td>
                <td>
                   
                </td>
                <td>
                    
                </td>
            </tr>
            
            <tr>
                <td valign="top" align="right">
                    <asp:Label ID="lblSelectedBillSiteName1" runat="server" Text="Site Name" Visible="False"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left" colspan="3">
                    <asp:Label ID="lblSelectedBillSiteName" runat="server" ForeColor="Blue" Text="" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    <asp:Label ID="lblReportList" runat="server" Text="Report List"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="left" colspan="3">
                    <asp:GridView ID="grdView" runat="server" AccessKey="2" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#DEBA84" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" CellSpacing="2" GridLines="None" 
                        OnPageIndexChanging="grdView_PageIndexChanging" OnSelectedIndexChanged="grdView_SelectedIndexChanged"
                        PageSize="9" Style="margin-bottom: 0px; margin-right: 0px; margin-top: 0px;"
                        UseAccessibleHeader="False" Width="704px" >
                        <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" Height="16px" />
                        <EmptyDataRowStyle BorderStyle="None" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="DateOfReceiving" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Received Date" />
                            <asp:BoundField DataField="Record_type" HeaderText="Test Name" />
                            <asp:BoundField DataField="INWD_Building_var" HeaderText="Building Name" NullDisplayText="--" />
                            <asp:BoundField DataField="CONT_Name_var" HeaderText="Contact Name" />
                            <asp:BoundField DataField="INWD_ContactNo_var" HeaderText="Contact No" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="RefNo" HeaderText="Report No" />
                            <asp:CommandField ButtonType="Link" SelectText="view" ShowSelectButton="true" />
                        </Columns>
                        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                        <PagerStyle BorderColor="Blue" Font-Bold="True" Font-Italic="False" Font-Names="Arial"
                            Font-Overline="False" Font-Size="Medium" Font-Strikeout="False" Font-Underline="False"
                            ForeColor="Blue" HorizontalAlign="Center" Wrap="True" />
                        <EmptyDataTemplate>
                            No reports to display...
                        </EmptyDataTemplate>
                     
                        <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BorderStyle="None" />
                    </asp:GridView>
                    &nbsp;
                    <asp:Label ID="lblReportMsg" runat="server" ForeColor="Red" 
                        Text="Report do not ready .." Visible="False"></asp:Label>
                    &nbsp; &nbsp;
                </td>
            </tr>


        </table>
    </asp:Panel>
    <%--</ContentTemplate> 
    </asp:UpdatePanel> --%>
    </asp:Panel>    
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

</asp:Content>
