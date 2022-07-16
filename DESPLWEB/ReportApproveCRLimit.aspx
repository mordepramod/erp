<%@ Page Title="Approve CR Limit For Report" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="ReportApproveCRLimit.aspx.cs" Inherits="DESPLWEB.ReportApproveCRLimit" Theme="duro"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td style="width: 60%">
                        <asp:Label ID="Label2" runat="server" Text="Inward Type"></asp:Label>&nbsp;&nbsp;
                        <%--</td>
                    <td style="width: 40%">--%>
                        <asp:DropDownList ID="ddl_InwardTestType" Width="155px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_InwardTestType_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:DropDownList ID="ddlMF" Width="100px" runat="server" Visible="false" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlMF_SelectedIndexChanged">
                            <asp:ListItem Text="MD Letter" Value="MDL"></asp:ListItem>
                            <asp:ListItem Text="Final Report" Value="Final"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                        &nbsp; &nbsp;
                        <asp:Label ID="lbl_RecordsNo" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="width: 12%">
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                    </td>
                    <td style="width: 30%">
                        <asp:Label ID="lblUserId" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:TextBox ID="txt_Client" runat="server" Width="280px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px">
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblReason" runat="server" Text="Reason"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:DropDownList ID="ddlReasonSel" Width="155px" runat="server">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkAddReason" runat="server" Text="Add New Reason" Style="text-decoration: underline;"
                            OnClick="lnkAddReason_Click">
                        </asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txtDateSel" runat="server" Width="80px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtDateSel">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtDateSel" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;
                        <asp:Label ID="lblAmount" runat="server" Text="Amount"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txtAmountSel" runat="server" Width="80px"></asp:TextBox>
                        &nbsp;&nbsp;
                        <asp:Label ID="lblME" runat="server" Text="ME"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:DropDownList ID="ddlMESel" Width="155px" runat="server">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkApproveSelReports" runat="server" Style="text-decoration: underline;"
                            OnClick="lnkApproveSelReports_Click">Approve Selected Reports</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="400px"
                Width="940px" BorderColor="AliceBlue">
                <asp:GridView ID="grdReports" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                    CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                    Width="100%" CellPadding="0" CellSpacing="0" OnRowDataBound="grdReport_RowDataBound"
                    OnRowCommand="grdReport_RowCommand" OnRowCreated="grdReports_RowCreated">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbxSelectAll" runat="server" onclick="javascript:HeaderClick(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbxSelect" runat="server" />
                                <asp:Label ID="lblClientId" runat="server" Text='<%#Eval("CL_Id")%>' Visible="false"></asp:Label>
                                <asp:Label ID="lblSiteId" runat="server" Text='<%#Eval("SITE_Id")%>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="3%" />
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Sr.No.">
                            <ItemTemplate>
                                 <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>--%>
                        <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="CL_Limit_mny" HeaderText="Credit Limit" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" DataFormatString="{0:f2}" />
                        <%--<asp:BoundField DataField="CL_BalanceAmt_mny" HeaderText="Balance Amt" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" DataFormatString="{0:f2}" />--%> 
                        <asp:TemplateField HeaderText="Balance Amt">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBalanceAmt" runat="server" Text='<%# Eval("CL_BalanceAmt_mny", "{0:0.00}") %>' 
                                    ToolTip="View Outstanding" DataFormatString="{0:f2}" Style="text-decoration: underline;"
                                    CommandArgument='<%#Eval("CL_Id")%>' CommandName="ViewOutstanding">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RecordType" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="SetOfRecord" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_NetAmt_num" HeaderText="Bill Amt" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ApprovedDate" HeaderText="Approved Date" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="MDLStatus" HeaderText="Test Type" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="MEName" HeaderText="ME" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:TemplateField HeaderText="Reason">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlReason" Width="100px" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDate" runat="server" Width="80px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                    Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtDate">
                                </asp:CalendarExtender>
                                <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtDate" MaskType="Date"
                                    Mask="99/99/9999" AutoComplete="false" runat="server">
                                </asp:MaskedEditExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAmount" runat="server" Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ME">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlME" Width="100px" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Followup">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewReason" runat="server" ToolTip="View Reason" Style="text-decoration: underline;"
                                    CommandArgument='<%#Eval("CL_Id")%>' CommandName="ViewReason">View</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkApproveCRLimit" runat="server" ToolTip="Approve CR Limit"
                                    Style="text-decoration: underline;" CommandArgument='<%#Eval("RecordType") + ";" + Eval("RecordNo") + ";"+ Eval("ReferenceNo") + ";"+ Eval("MDLStatus") %>'
                                    CommandName="ApproveCRLimit">Approve</asp:LinkButton>
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
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                    PopupControlID="pnlDetails" PopupDragHandleControlID="PopupHeader" Drag="true"
                    BackgroundCssClass="ModalPopupBG">
                </asp:ModalPopupExtender>
                <asp:HiddenField ID="HiddenField2" runat="server" />
                <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="HiddenField2"
                    PopupControlID="pnlAddNewReason" PopupDragHandleControlID="PopupHeader" Drag="true"
                    BackgroundCssClass="ModalPopupBG">
                </asp:ModalPopupExtender>
                <asp:HiddenField ID="HiddenField3" runat="server" />
                <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="HiddenField3"
                    PopupControlID="pnlOutstandingList" PopupDragHandleControlID="PopupHeader" Drag="true"
                    BackgroundCssClass="ModalPopupBG">
                </asp:ModalPopupExtender>
            </asp:Panel>
             <asp:Panel runat="server" ID="pnlDetails">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table class="DetailPopup" width="800px" height="400px">
                            <tr>
                                <td align="center" valign="bottom" colspan="2">
                                    <asp:ImageButton ID="imgCloseDetails" runat="server" ImageUrl="Images/cross_icon.png"
                                                OnClick="imgCloseDetails_Click" ImageAlign="Right" />
                                    <asp:Label ID="lblpopuphead" runat="server" Text="Report Approval Reasons" Font-Bold="True"
                                        ForeColor="#990033"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="pnlReason" runat="server" ScrollBars="Auto" Height="350px" Width="800px">
                                        <asp:GridView ID="grdReason" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                            BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="None"
                                            Width="100%" CellPadding="4" CellSpacing="0">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="MISRecType" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="MISRecordNo" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="MISRefNo" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="CrFLUP_Reason_var" HeaderText="Approval Reason" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CrFLUP_PaymentDate_dt" HeaderText="Expected Payment Date" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                                                <asp:BoundField DataField="CrFLUP_Amount_num" HeaderText="Amount" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ME_Name" HeaderText="ME" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
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
            <asp:Panel runat="server" ID="pnlAddNewReason">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <table class="DetailPopup" width="300px" height="200px">
                            <tr>
                                <td align="center" valign="bottom">
                                    <asp:ImageButton ID="imgCloseReason" runat="server" ImageUrl="Images/cross_icon.png"
                                        OnClick="imgCloseReason_Click" ImageAlign="Right" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblpopuphead1" runat="server" Text="Add New Reason" Font-Bold="True"
                                        ForeColor="#990033"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;Reason &nbsp;&nbsp;
                                    <asp:TextBox ID="txtReason" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:LinkButton ID="lnkAdd" runat="server" Text="Add" Style="text-decoration: underline;"
                                        OnClick="lnkAdd_Click">
                                    </asp:LinkButton>
                                </td>
                            </tr>
                            <tr style="height: 50px">
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="imgCloseReason" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlOutstandingList">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <table class="DetailPopup" width="900px" height="500px">
                            <tr>
                                <td align="center" valign="bottom" colspan="2">
                                    <asp:ImageButton ID="imgCloseOutstandingList" runat="server" ImageUrl="Images/cross_icon.png"
                                        OnClick="imgCloseOutstandingList_Click" ImageAlign="Right" />
                                    <asp:Label ID="lblpopuphead2" runat="server" Text="Outstanding Bills" Font-Bold="True"
                                        ForeColor="#990033"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:Label ID="lblTotal" runat="server" Text="" Font-Bold="True" Font-Size="Small"
                                        ForeColor="OrangeRed"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Panel ID="pnlBillList" runat="server" ScrollBars="Auto" Height="450px" Width="890px">
                                        <asp:GridView ID="grdOutstanding" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False"
                                            BackColor="#F7F6F3" CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333"
                                            GridLines="Horizontal" Width="100%" CellPadding="0" CellSpacing="0">
                                            <Columns>
                                                <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <%--<asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Left" />--%>
                                                <asp:BoundField DataField="SiteName" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="BillNo" HeaderText="Bill No" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="BillDate" HeaderText="Bill Date" DataFormatString="{0:dd/MM/yyyy}"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="TestingType" HeaderText="Testing Type" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount " HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="PendingAmount" HeaderText="Pending Amount" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="DaysPending" HeaderText="Days Pending" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="MktUser" HeaderText="Mkt User" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Route" HeaderText="Route" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <%--<asp:BoundField DataField="Limit" HeaderText="Limit" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />--%>
                                                <asp:BoundField DataField="BillWiseOutstanding" HeaderText="Billwise Outstanding"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="OnAcBalance" HeaderText="On A/c Balance" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ActualOutstanding" HeaderText="Actual Outstanding" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                            </Columns>
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
    <script type="text/javascript">
        var TotalChkBx;
        var Counter;
        window.onload = function () {
            TotalChkBx = parseInt('<%= this.grdReports.Rows.Count %>');
            Counter = 0;
        }
        function HeaderClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.grdReports.ClientID %>');
            var TargetChildControl = "cbxSelect";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBx : 0;

        }
        function ChildClick(CheckBox, HCheckBox) {
            var HeaderCheckBox = document.getElementById(HCheckBox);
            if (CheckBox.checked && Counter < TotalChkBx)
                Counter++;
            else if (Counter > 0)
                Counter--;
            if (Counter < TotalChkBx)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBx)
                HeaderCheckBox.checked = true;
        }
        function CheckAll(Checkbox) {
            var grdRoute = document.getElementById("<%=grdReports.ClientID %>");
            for (i = 1; i < grdRoute.rows.length; i++) {
                grdRoute.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }

        }
    </script>
</asp:Content>
