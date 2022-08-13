<%@ Page Title="Recovery Updation" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="RecoveryUpdation.aspx.cs" Inherits="DESPLWEB.RecoveryUpdation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td style="width: 13%">
                        <%--<asp:Label ID="lblBillDate" runat="server" Text="Bill date " ></asp:Label>&nbsp;&nbsp;--%>
                        <asp:CheckBox ID="chkAsOnDate" runat="server" Text="As on Date" OnCheckedChanged="chkAsOnDate_CheckedChanged"
                                AutoPostBack="true" />
                    </td>
                    <td style="width: 40%">
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date "></asp:Label>&nbsp;
                        <asp:TextBox ID="txt_FromDate" Width="80px" runat="server" ></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; 
                        <asp:Label ID="lblToDate" runat="server" Text="To Date "></asp:Label>&nbsp;
                        <asp:TextBox ID="txt_ToDate" Width="80px" runat="server" ></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td style="width: 10%">
                        <asp:Label ID="lblRecoveryUser" runat="server" Text="Recovery User"></asp:Label>&nbsp;
                    </td>
                    <td style="width: 35%">
                        <asp:DropDownList ID="ddlRecoveryUser" runat="server" Width="200px" OnSelectedIndexChanged="ddlRecoveryUser_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td align="right" style="width: 5%">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClient" runat="server" Text="Client" ></asp:Label>
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
                    <td>
                        <asp:Label ID="lblSite" runat="server" Text="Site" ></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlSite" Width="307px" runat="server" OnSelectedIndexChanged="ddlSite_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:Label ID="lblClientAddress" runat="server" Text="Address" ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtClientAddress" runat="server" Text="" Width="307px"></asp:TextBox>
                    </td> 
                    <td>
                        <asp:Label ID="lblSiteAddress" runat="server" Text="Address" ></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSiteAddress" runat="server" Text="" Width="307px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Label1" runat="server" Text="Account Contact Details" Font-Bold="true"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkUpdateClientContactDetails" runat="server" Font-Bold="true" OnClick="lnkUpdateClientContactDetails_Click">Update</asp:LinkButton>
                    </td>                    
                    <td colspan="3">
                        <asp:Label ID="Label2" runat="server" Text="Site Incharge Contact Details" Font-Bold="true"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkUpdateSiteContactDetails" runat="server" Font-Bold="true" OnClick="lnkUpdateSiteContactDetails_Click">Update</asp:LinkButton>
                    </td>                    
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClientContactPerson" runat="server" Text="Contact Person" ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtClientContactPerson" runat="server" Text="" Width="307px"></asp:TextBox>
                    </td> 
                    <td>
                        <asp:Label ID="lblSiteContactPerson" runat="server" Text="Contact Person" ></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSiteContactPerson" runat="server" Text="" Width="307px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClientPhoneNo" runat="server" Text="Phone No." ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtClientPhoneNo" runat="server" Text="" Width="307px"></asp:TextBox>
                    </td> 
                    <td>
                        <asp:Label ID="lblSitePhoneNo" runat="server" Text="Phone No." ></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSitePhoneNo" runat="server" Text="" Width="307px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClientEmailId" runat="server" Text="Email Id" ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtClientEmailId" runat="server" Text="" Width="307px"></asp:TextBox>
                    </td> 
                    <td>
                        <asp:Label ID="lblSiteEmailId" runat="server" Text="Email Id" ></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSiteEmailId" runat="server" Text="" Width="307px"></asp:TextBox>
                    </td>
                    
                </tr>
                 <tr>
                    <td>
                        <asp:Label ID="lblClientUnderReconciliation" runat="server" Text="Under Reconciliation" ></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkClientUnderReconciliation" runat="server" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkUpdateClAsUnderRecon" runat="server" Font-Bold="true" OnClick="lnkUpdateClAsUnderRecon_Click">Update</asp:LinkButton>
                    </td> 
                    <td colspan="3" align="right">
                        <asp:LinkButton ID="lnkFetch" runat="server" Font-Bold="true" OnClick="lnkFetch_Click">Fetch</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkUpdateAllSel" runat="server" Font-Bold="true" OnClick="lnkUpdateAllSel_Click">Update All Selected</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Bold="true" OnClick="lnkPrint_Click">Print</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblRecords" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblTotal" runat="server" Text="" Font-Bold="true" ForeColor="DarkRed"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblStatusFilter" runat="server" Text="Staus"></asp:Label>&nbsp;
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlStatusFilter" runat="server" Width="200px" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="---All---"></asp:ListItem>

                            <%--<asp:ListItem Text="Posted in accounts"></asp:ListItem>
                            <asp:ListItem Text="Received at HO"></asp:ListItem> - Approve & send to HO
                            <asp:ListItem Text="Not Received"></asp:ListItem>
                            <asp:ListItem Text="Resubmitted"></asp:ListItem>
                            <asp:ListItem Text="Received at site"></asp:ListItem> - Confirmation at site
                            <asp:ListItem Text="Client not responding"></asp:ListItem>
                            <asp:ListItem Text="Technical - Report issue"></asp:ListItem>--%>

                            <%--<asp:ListItem Text="Submitted"></asp:ListItem>
                            <asp:ListItem Text="Resubmitted"></asp:ListItem> - Not received at site
                            <asp:ListItem Text="Confirmation at site"></asp:ListItem> - Received at site
                            <asp:ListItem Text="Approve & send to HO"></asp:ListItem> - Posted in accounts
                            <asp:ListItem Text="Posted in accounts"></asp:ListItem>--%>

                            <asp:ListItem Text="Submitted"></asp:ListItem>
                            <asp:ListItem Text="Not received at site"></asp:ListItem>
                            <asp:ListItem Text="Received at site"></asp:ListItem>
                            <asp:ListItem Text="Not posted in accounts"></asp:ListItem>
                            <asp:ListItem Text="Posted in accounts"></asp:ListItem>
                            <asp:ListItem Text="Acknowledgement not available"></asp:ListItem>
                            
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="260px"
                Width="940px" BorderColor="AliceBlue">
                <%--<asp:UpdatePanel ID="UpdatePanelGrd" runat="server">
                    <ContentTemplate>--%>
                <%--<div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 280px; overflow: auto">--%>
                        <asp:GridView ID="grdBills" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                            CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                            Width="100%" CellPadding="0" CellSpacing="0" OnRowCommand="grdBills_RowCommand" OnRowDataBound="grdBills_RowDataBound">
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
                                <asp:BoundField DataField="BILL_Id" HeaderText="Bill No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_Date_dt" HeaderText="Bill Date" DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_NetAmt_num" HeaderText="Bill Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_PendingAmount" HeaderText="Pending Amount" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n}" />
                                <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="MEName" HeaderText="ME Name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CONT_Name_var" HeaderText="Contact Person" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="INWD_ContactNo_var" HeaderText="Contact No." HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="Bill">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkViewBill" runat="server" ToolTip="View Bill" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Bill_Id")%>' CommandName="ViewBill">View</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ack File">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkViewAckFile" runat="server" Text='<%# Eval("OUTW_AckFileName_var") %>' ToolTip="View Ack File" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("OUTW_AckFileName_var")%>' CommandName="ViewAckFile"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecvId" runat="server" Text='<%# Eval("RECV_Id") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("RECV_Status_var") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="130px">
                                            <asp:ListItem Text="---Select---"></asp:ListItem>
                                            <%--<asp:ListItem Text="Posted in accounts"></asp:ListItem>
                                            <asp:ListItem Text="Received at HO"></asp:ListItem>
                                            <asp:ListItem Text="Not Received"></asp:ListItem>
                                            <asp:ListItem Text="Resubmitted"></asp:ListItem>
                                            <asp:ListItem Text="Received at site"></asp:ListItem>
                                            <asp:ListItem Text="Client not responding"></asp:ListItem>
                                            <asp:ListItem Text="Technical - Report issue"></asp:ListItem>  --%>                                   
                                            <%--<asp:ListItem Text="Submitted"></asp:ListItem>
                                            <asp:ListItem Text="Resubmitted"></asp:ListItem>
                                            <asp:ListItem Text="Confirmation at site"></asp:ListItem>
                                            <asp:ListItem Text="Approve & send to HO"></asp:ListItem>
                                            <asp:ListItem Text="Posted in accounts"></asp:ListItem>  --%>                                            
                                            <asp:ListItem Text="Submitted"></asp:ListItem>
                                            <asp:ListItem Text="Not received at site"></asp:ListItem>
                                            <asp:ListItem Text="Received at site"></asp:ListItem>
                                            <asp:ListItem Text="Not posted in accounts"></asp:ListItem>
                                            <asp:ListItem Text="Posted in accounts"></asp:ListItem>
                                            <asp:ListItem Text="Acknowledgement not available"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bill Booked Amount">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBillBookedAmt" runat="server" Width="70px" Text='<%# Eval("RECV_BillBookedAmt_num") %>' onkeyup="checkDecimal(this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reason">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReason" runat="server" Text='<%# Eval("RECV_Reason_var") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlReason" runat="server" Width="110px"  >
                                            <asp:ListItem Text="---Select---"></asp:ListItem>
                                            <asp:ListItem Text="Technical issue"></asp:ListItem>
                                            <asp:ListItem Text="New invoice with current date"></asp:ListItem>
                                            <asp:ListItem Text="Client not responding"></asp:ListItem>
                                            <asp:ListItem Text="Funds issue"></asp:ListItem>
                                            <asp:ListItem Text="Client not traceable"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRemark" runat="server" Width="150px" Font-Size="Smaller" Text='<%# Eval("RECV_Remark_var") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expected Payment Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtExpectedPaymentDate" runat="server" Width="70px" Text='<%# Eval("RECV_ExpectedPaymentDate_dt") %>'></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtExpectedPaymentDate">
                                        </asp:CalendarExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expected Amount">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtExpectedAmt" runat="server" Width="70px" Text='<%# Eval("RECV_ExpectedAmt_num") %>' onkeyup="checkDecimal(this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkUpdate" runat="server" ToolTip="Update" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Bill_Id")%>' CommandName="UpdateBill">Update</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkFollowup" runat="server" ToolTip="Add Followup" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Bill_Id")%>' CommandName="AddFollowup">Followup</asp:LinkButton>
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
                    <%--</div>
                </div>--%>
                       <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                PopupControlID="pnlFollowupDetail" PopupDragHandleControlID="PopupHeader" Drag="true"
                BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel runat="server" ID="pnlFollowupDetail">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table class="DetailPopup" width="300px" height="400px">
                            <tr>
                                <td align="center" valign="bottom" colspan="2">
                                    <asp:ImageButton ID="imgCloseDetails" runat="server" ImageUrl="Images/cross_icon.png"
                                                OnClick="imgCloseDetails_Click" ImageAlign="Right" />
                                    <asp:Label ID="lblpopuphead" runat="server" Text="Add Followup" Font-Bold="True"
                                        ForeColor="#990033"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFollowupBillNo" runat="server" Text="Bill No. : " Font-Bold="True"
                                        ForeColor="#cc00cc"></asp:Label>
                                    &nbsp;&nbsp;Description &nbsp;&nbsp;
                                    <asp:TextBox ID="txtDescription" runat="server" Width="200px"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                     <asp:LinkButton ID="lnkAdd" runat="server" Text="Add" Style="text-decoration: underline;"
                                        OnClick="lnkAdd_Click">
                                    </asp:LinkButton>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="pnlFollowup" runat="server" ScrollBars="Auto" Height="350px" Width="700px">
                                        <asp:GridView ID="grdFollowup" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                            BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="None"
                                            Width="100%" CellPadding="4" CellSpacing="0">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <asp:BoundField DataField="RECFLUP_Date_dt" HeaderText="Follwup Date" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                                <asp:BoundField DataField="AddedByUser" HeaderText="Followup By User" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="RECFLUP_Description_var" HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Left" />
                                            </Columns>
                                            <FooterStyle BackColor="#CCCC99" />
                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                            <EmptyDataTemplate>
                                                No records to display
                                            </EmptyDataTemplate>
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <EditRowStyle BackColor="#999999" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>

        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <%--<script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdBills.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdBills.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdBills.ClientID%>').offset().top);

        });
    </script>--%>
    <script type="text/javascript">
        function checkDecimal(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 46))) {
                    x.value = '';
                    x.focus();
                    alert("Only Decimal Values Allowed");

                    return false;
                }

            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
    </script>
</asp:Content>

