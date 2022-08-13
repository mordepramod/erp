<%@ Page Title="Approve Reports" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="ApproveReport.aspx.cs" Inherits="DESPLWEB.ApproveReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="12%">
                        <asp:Label ID="Label1" runat="server" Text="Mat. Recd "></asp:Label>
                    </td>
                    <td style="width: 43%">
                        <asp:TextBox ID="txt_FromDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txt_ToDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td style="width: 10%">
                        <asp:CheckBox ID="chk_Sign" runat="server" Text="Add Sign" />
                        &nbsp;
                    </td>
                    <td style="width: 348px">
                        <asp:DropDownList ID="ddlMF" Width="100px" runat="server" Visible="false">
                            <asp:ListItem Text="MD Letter" Value="MDL"></asp:ListItem>
                            <asp:ListItem Text="Final Report" Value="Final"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lbl_RecordsNo" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblUserId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Inward Type"></asp:Label>
                    </td>
                    <td style="width: 60%">
                        <asp:DropDownList ID="ddl_InwardTestType" Width="155px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_InwardTestType_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:RadioButton ID="optPending" runat="server" Text="Pending" GroupName="G1" AutoPostBack="true"
                            OnCheckedChanged="optPending_CheckedChanged" />
                        &nbsp; &nbsp; &nbsp;
                        <asp:RadioButton ID="optApproved" runat="server" Text="Approved" GroupName="G1" AutoPostBack="true"
                            OnCheckedChanged="optApproved_CheckedChanged" />
                        &nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td style="width: 15%">
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                    </td>
                    <td align="left" style="width: 348px">
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                </tr>            
            <tr>
                <td colspan="4">
                    <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="420px"
                Width="940px" BorderColor="AliceBlue">
                <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 420px; overflow: auto">
                        <asp:GridView ID="grdReports" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                            CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Horizontal"
                            Width="100%" CellPadding="0" CellSpacing="0" OnRowCommand="grdReport_RowCommand"
                            OnRowDataBound="grdReport_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="SetOfRecord" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CollectionDate" HeaderText="Collection Date" DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReceivedDate" HeaderText="Received Date" DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <%--<asp:TemplateField HeaderText="Email Id">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtEmailId" runat="server" Width="300px" Text='<%# Eval("EmailId") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contact No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContactNo" runat="server" Width="150px" Text='<%# Eval("ContactNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkViewReport" runat="server" ToolTip="View Report" Style="text-decoration: underline;"
                                              CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";" +  Eval("ReferenceNo") %>'
                                            CommandName="ViewReport">View</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkApproveReport" runat="server" ToolTip="Approve Report" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";" + Eval("ReferenceNo") %>'
                                            CommandName="ApproveReport">Approve</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkUpdateContactDetails" runat="server" ToolTip="Update Contact Details" Style="text-decoration: underline;"
                                              CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";" +  Eval("ReferenceNo") %>'
                                            CommandName="UpdateContactDetails">Update Contact Details</asp:LinkButton>
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
                <td colspan="4">
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                        PopupControlID="pnlContactDetails" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnlContactDetails" runat="server" Width="450px">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table class="DetailPopup" style="width: 100%">
                                    <tr valign="top">
                                        <td align="center" valign="bottom" colspan="6">
                                            <asp:ImageButton ID="imgCloseContactDetailPopup" runat="server" ImageUrl="Images/cross_icon.png"
                                                OnClick="imgCloseContactDetailPopup_Click" ImageAlign="Right" />
                                            <asp:Label ID="lblContactDetail" runat="server" Font-Bold="True" ForeColor="#990033" Text="Update Contact Detail"
                                                Font-Size="Small"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            &nbsp;
                                        </td> 
                                    </tr>
                                    <tr valign="top">
                                        <td align="right">
                                            Record Type
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRecordType" runat="server" Font-Bold="True" ForeColor="#CC0099"
                                                Text="Record Type" Font-Size="12px"></asp:Label><br />
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="right">
                                            Record No.
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRecordNo" runat="server" Font-Bold="True" ForeColor="#CC0099"
                                                Text="Record No." Font-Size="12px"></asp:Label><br />
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="right">
                                            Contact Person Name
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="lblContactId" runat="server" Text="" Visible="false"></asp:Label>
                                            <%--<asp:Label ID="ContactName" runat="server" Font-Bold="True" ForeColor="#CC0099"
                                                Text="Contact Person Name" Font-Size="12px"></asp:Label>--%>
                                                <asp:TextBox ID="txtContactName" runat="server" MaxLength="250" Width="250px"></asp:TextBox>
                                             
                                            <br />&nbsp;
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="right">
                                            Contact Number
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtContactNo" runat="server" MaxLength="250" Width="250px"></asp:TextBox>
                                             <br />&nbsp;
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtContactNo"
                                                EnableClientScript="False" ErrorMessage="Input 'Contact No.'" SetFocusOnError="true"
                                                ValidationGroup="V2"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="right">
                                            Email Id
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtContactEmail" runat="server" MaxLength="250" Width="250px" EnableViewState="False"></asp:TextBox>
                                            <br />&nbsp;
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtContactEmail"
                                                EnableClientScript="False" ErrorMessage="Input 'Email'" SetFocusOnError="true"
                                                ValidationGroup="V2"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtContactEmail"
                                                EnableClientScript="False" ErrorMessage="Invalid Email" ValidationExpression="^([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+,*[\W]*)+$"
                                                ValidationGroup="V2"></asp:RegularExpressionValidator>
                                                 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Label ID="lblContactMessage" runat="server" ForeColor="#990033" Text="lblMsg" Visible="False"></asp:Label>
                                            <br />&nbsp;
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="3">
                                            <asp:LinkButton ID="lnkSaveContactDetail" runat="server" CssClass="LnkOver" OnClick="lnkSaveContactDetail_Click"
                                                Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V2">Save</asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkCancelContactDetail" runat="server" CssClass="LnkOver" OnClick="lnkCancelContactDetail_Click"
                                                Style="text-decoration: underline; font-weight: bold;" >Cancel</asp:LinkButton>
                                                <br />&nbsp;
                                        </td>
                                    </tr>
                                    
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkCancelContactDetail" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdReports.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdReports.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdReports .ClientID%>').offset().top);

        });
    </script>
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
