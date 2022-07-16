<%@ Page Title="Print Reports" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="PrintReport.aspx.cs" Inherits="DESPLWEB.PrintReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td width="10%">
                        <asp:RadioButton ID="optPending" runat="server" Text="Pending" GroupName="G1" AutoPostBack="true"
                            OnCheckedChanged="optPending_CheckedChanged" />
                    </td>
                    <td width="45%" valign="middle">
                        <asp:RadioButton ID="optPrinted" runat="server" Text="Printed" GroupName="G1" AutoPostBack="true"
                            OnCheckedChanged="optPrinted_CheckedChanged" />
                        &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="lblIssueDate" runat="server" Text="Issue Date" Visible="false"></asp:Label>
                        &nbsp; &nbsp;                   
                        <asp:TextBox ID="txt_FromDate" Width="90px" runat="server" Visible="false"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; 
                        <asp:TextBox ID="txt_ToDate" Width="90px" runat="server" Visible="false"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td width="12%">
                        <asp:Label ID="Label3" runat="server" Text="Authorized By"></asp:Label>
                    </td>
                    <td width="40%">
                        <asp:DropDownList ID="ddlApprovedBy" Width="310px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td width="2%" align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Inward Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_InwardTestType" Width="180px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_InwardTestType_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                        &nbsp; &nbsp;
                            <asp:Label ID="lbl_RecordsNo" runat="server" Text=""></asp:Label>
                        &nbsp; &nbsp;
                        <asp:LinkButton ID="lnkPrintAllCubeRpt" runat="server" CssClass="LnkOver"
                            Visible="false" Style="text-decoration: underline;" OnClick="lnkPrintAllCubeRpt_Click">Print All</asp:LinkButton>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMF" runat="server" Text="Sub-Report" Visible="false"></asp:Label>
                        <asp:Label ID="lblCategory" runat="server" Text="Category" Visible="false"></asp:Label>&nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMF" Width="155px" runat="server" Visible="false" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlMF_SelectedIndexChanged">
                            <asp:ListItem Text="MD Letter" Value="MDL"></asp:ListItem>
                            <asp:ListItem Text="Sieve Analysis"></asp:ListItem>
                            <asp:ListItem Text="Blank Trial"></asp:ListItem>
                            <asp:ListItem Text="Trial"></asp:ListItem>
                            <asp:ListItem Text="Cube Strength"></asp:ListItem>
                            <asp:ListItem Text="Moisture Correction"></asp:ListItem>
                            <asp:ListItem Text="Cover Sheet"></asp:ListItem>
                            <%--<asp:ListItem Text="Cement Report"></asp:ListItem>--%>
                            <asp:ListItem Text="Final Report" Value="Final"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_Category"  Width="200px" runat="server" Visible="false">
                        </asp:DropDownList>

                    </td>
                    <td>
                        <asp:Label ID="lblPageBreak" runat="server" Text="Page Break @" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPageBreak" runat="server" Width="120px" Text="36,72,108,144"
                            Visible="false"></asp:TextBox>
                        <asp:CheckBox ID="chkSplitReport" Text="Split Report" runat="server" Visible="false"/>

                        <%--     <asp:CheckBox ID="chkLocationAll"  Text="Location To All" runat="server" AutoPostBack="true" 
                            oncheckedchanged="chkLocationAll_CheckedChanged" />
                         <asp:DropDownList ID="ddlNABLLocationAll" runat="server" BorderWidth="0px" AutoPostBack="true" 
                            Width="75px" onselectedindexchanged="ddlNABLLocationAll_SelectedIndexChanged">
                                            <asp:ListItem Text="--Select--" />
                                            <asp:ListItem Text="1" />
                                            <asp:ListItem Text="2" />
                                            <asp:ListItem Text="3" />
                                            <asp:ListItem Text="4" />
                                            <asp:ListItem Text="5" />
                                        </asp:DropDownList>--%>
                    </td>
                    <td>
                        <asp:Label ID="lblUserId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="380px"
                Width="940px" BorderColor="AliceBlue">

                <asp:GridView ID="grdReports" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                    CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                    Width="100%" CellPadding="0" CellSpacing="0" OnRowCommand="grdReport_RowCommand" > <%--OnRowDataBound="grdReports_RowDataBound"--%>
                    <Columns>
                        <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="SetOfRecord" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CollectionDate" HeaderText="Collection Date" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ReceivedDate" HeaderText="Received Date" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="InterBranchReferenceNo" HeaderText="Inter Branch Reference No" 
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <%--<asp:TemplateField HeaderText="Email Id">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtEmailId" runat="server" Width="300px" Text='<%# Eval("EmailId") %>'
                                            ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="Contact No.">
                            <ItemTemplate>
                                <asp:Label ID="lblContactNo" runat="server" Width="150px" Text='<%# Eval("ContactNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trial Id" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblTrialId" runat="server" Text='<%# Eval("Trial_Id") %>' ReadOnly="true"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trial Name">
                            <ItemTemplate>
                                <asp:Label ID="lblTrialName" runat="server" Width="50px" Text='<%# Eval("Trial_Name") %>'
                                    ReadOnly="true"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Days">
                            <ItemTemplate>
                                <asp:Label ID="lblCubeDays" runat="server" Width="50px" Text='<%# Eval("Days_tint") %>'
                                    ReadOnly="true"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <%--  <asp:TemplateField HeaderText="NABL" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNABL" runat="server" Text='<%# Eval("NablStatus") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlNABL" runat="server" BorderWidth="0px" Width="90px">
                                            <asp:ListItem Text="--Select--" />
                                            <asp:ListItem Text="NABL" />
                                            <asp:ListItem Text="Non-NABL" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                        <asp:BoundField DataField="MEName" HeaderText="ME Name"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                        <asp:TemplateField HeaderText="NABL Scope" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                 <asp:Label ID="lblNABLScope" runat="server" Text='<%# Eval("NablScope") %>'></asp:Label>
                                <%--<asp:DropDownList ID="ddlNABLScope" runat="server" BorderWidth="0px" Width="75px" Enabled="true">
                                    <asp:ListItem Text="--Select--" />
                                    <asp:ListItem Text="F" />
                                    <asp:ListItem Text="P" />
                                    <asp:ListItem Text="NA" />
                                </asp:DropDownList>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NABL Location" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                 <asp:Label ID="lblNABLLocation" runat="server" Text='<%# Eval("NablLocation") %>'></asp:Label>
                               <%-- <asp:DropDownList ID="ddlNABLLocation" runat="server" BorderWidth="0px" Width="75px" Enabled="true">
                                    <asp:ListItem Value="0" Text="0" />
                                    <asp:ListItem Value="1" Text="1" />
                                    <asp:ListItem Value="2" Text="2" />
                                 </asp:DropDownList>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblBalanceStatus" runat="server" Width="50px" Text='<%# Eval("balanceStatus") %>'
                                    Visible="false"></asp:Label>
                                <asp:LinkButton ID="lnkPrintReport" runat="server" ToolTip="Print Report" Style="text-decoration: underline;"
                                    CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("ReferenceNo") %>'
                                    CommandName="PrintReport">Print</asp:LinkButton>
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

            </asp:Panel>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>

    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
