<%@ Page Title="Outstanding Bills" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="OutstandingList.aspx.cs" Inherits="DESPLWEB.OutstandingList"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
                <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
                    Style="background-color: #ECF5FF;">
                    <table style="width: 100%">
                        <tr style="background-color: #ECF5FF;">
                            <td width="100%">
                                <asp:CheckBox ID="chkAsOnDate" runat="server" Text="As on Date" OnCheckedChanged="chkAsOnDate_CheckedChanged"
                                    AutoPostBack="true" />
                                &nbsp;&nbsp;
                                <asp:Label ID="lblFromDate" runat="server" Text="From Date "></asp:Label>
                                <asp:TextBox ID="txtFromDate" runat="server" Width="120px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                    Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                                </asp:CalendarExtender>
                                <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                                    Mask="99/99/9999" AutoComplete="false" runat="server">
                                </asp:MaskedEditExtender>
                                &nbsp; &nbsp; &nbsp; &nbsp;
                                <asp:Label ID="lblToDate" runat="server" Text="To Date "></asp:Label>
                                <asp:TextBox ID="txtToDate" Width="120px" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                    Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                                </asp:CalendarExtender>
                                <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                                    Mask="99/99/9999" AutoComplete="false" runat="server">
                                </asp:MaskedEditExtender>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <%--<asp:Label ID="lblClient" runat="server" Text="Client"></asp:Label>&nbsp;&nbsp;
                                <asp:DropDownList ID="ddlClient" Width="260px" runat="server">
                                </asp:DropDownList>--%>
                                <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                                    OnClick="imgClosePopup_Click" ImageAlign="Right" />
                            </td>
                        </tr>
                        <tr style="background-color: #ECF5FF;">
                            <td colspan="3">                                
                                <asp:LinkButton ID="lnkDisplay" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                                    OnClick="lnkDisplay_Click" Font-Bold="True">Display</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;                                    
                                <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                                    OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkPrintBalanceCertificate" runat="server" CssClass="LnkOver"
                                    Style="text-decoration: underline;" OnClick="lnkPrintBalanceCertificate_Click"
                                    Font-Bold="True">Print Balance Certificate</asp:LinkButton>
                                &nbsp;<asp:Label ID="lblOutstanding" runat="server" Text="" Font-Size="Small" Visible="false"></asp:Label>
                                &nbsp;<asp:LinkButton ID="lnkUpdateClientOutstandng" runat="server" CssClass="LnkOver"
                                    Style="text-decoration: underline;" Font-Bold="True" OnClick="lnkUpdateClientOutstandng_Click">Update Client Outstandng</asp:LinkButton>
                              
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <asp:Label ID="Label1" runat="server" Text="Client"></asp:Label>&nbsp;
                                <asp:DropDownList ID="ddlClientSort" Width="150px" runat="server" OnSelectedIndexChanged="ddl_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblMktUser" runat="server" Text="Mkt User"></asp:Label>&nbsp;
                                <asp:DropDownList ID="ddlMktUserSort" Width="150px" runat="server" OnSelectedIndexChanged="ddl_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblDaysFrom" runat="server" Text="Days From"></asp:Label>&nbsp;
                                <asp:TextBox ID="txtDaysFrom" Width="40px" runat="server" onchange="checkNum(this)"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblDaysTo" runat="server" Text="To"></asp:Label>&nbsp;
                                <asp:TextBox ID="txtDaysTo" Width="40px" runat="server" onchange="checkNum(this)"></asp:TextBox>
                                <asp:LinkButton ID="lnkApply" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                                    OnClick="lnkApply_Click" Font-Bold="True">Apply</asp:LinkButton>
                                <%--</td>
                    <td colspan="2" align="right">--%>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblTotal" runat="server" Text="" Font-Bold="True" Font-Size="Small"
                                    ForeColor="OrangeRed"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" valign="top">
                                <asp:Panel ID="pnlBillList" runat="server" ScrollBars="Auto" Height="390px" Width="949px"
                                    BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">
                                    <asp:GridView ID="grdOutstanding" SkinID="gridviewSkin" runat="server" AutoGenerateColumns="False" 
                                        OnRowCommand="grdOutstanding_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ClientName" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="SiteName" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
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
                                            <asp:BoundField DataField="OUTW_AckDate_dt" HeaderText="Submission Date" DataFormatString="{0:dd/MM/yyyy}"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="OUTW_BookedDate_dt" HeaderText="Booked Date" DataFormatString="{0:dd/MM/yyyy}"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="RECV_Status_var" HeaderText="Recovery Status" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="RECV_BillBookedAmt_num" HeaderText="Booked Amount" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <%--<asp:BoundField DataField="RECV_ClearedFromSite_var" HeaderText="Cleared From Site" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />--%>
                                            <asp:BoundField DataField="RECV_Remark_var" HeaderText="Recovery Remark" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MktUser" HeaderText="Mkt User" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="RecoveryUser" HeaderText="Recovery User" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Region" HeaderText="Region" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Limit" HeaderText="Limit" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="TotalOutstading" HeaderText="Total Outstanding" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="OUTW_AckFileName_var" HeaderText="Acknowledgement" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <%--<asp:TemplateField HeaderText="Ack File">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkViewAckFile" runat="server" Text='<%# Eval("OUTW_AckFileName_var") %>' ToolTip="View Ack File" Style="text-decoration: underline;"
                                                        CommandArgument='<%#Eval("OUTW_AckFileName_var")%>' CommandName="ViewAckFile"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    
    <%--<asp:UpdateProgress ID="UpdWaitImage" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel1" >
        <ProgressTemplate>
            <div class="modal1">
                <div class="center1">
                    <asp:Image ID="imgProgress" ImageUrl="Images/ProgressImage.gif" runat="server" />
                    Please Wait...
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }

        function checkNum(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    x.value = '';
                    x.focus();
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Numeric Values Allowed";
                    return false;
                }
                else {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                }
            }
            return true;
        }
    </script>
    
</asp:Content>
