<%@ Page Title="Site Visit Log - Device" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="ReportSiteVisitLog.aspx.cs" Inherits="DESPLWEB.ReportSiteVisitLog" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="12%">
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date "></asp:Label>
                    </td>
                    <td width="50%" colspan="3">
                        <asp:TextBox ID="txtFromDate" runat="server" Width="100px" AutoPostBack="true"></asp:TextBox><%--OnTextChanged="txtFromDate_TextChanged"--%>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp;
                        <asp:Label ID="lblToDate" runat="server" Text="To Date "></asp:Label>
                        &nbsp; &nbsp; &nbsp;<asp:TextBox ID="txtToDate" Width="100px" runat="server" AutoPostBack="true"></asp:TextBox><%--OnTextChanged="txtFromDate_TextChanged"--%>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;
                    </td>
                    <td width="50%">
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" AutoPostBack="true"
                            oncheckedchanged="chkClientSpecific_CheckedChanged" />
                          &nbsp;&nbsp;
                     <asp:TextBox ID="txt_Client" runat="server" Width="280px" AutoPostBack="true"
                           Visible="false"   OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
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
                <tr>
                    <td width="12%">
                        <asp:Label ID="Label2" runat="server" Text="Select Executive"></asp:Label>&nbsp;&nbsp;
                    </td>
                    <td width="50%" colspan="3">
                        <asp:DropDownList ID="ddl_ME" Width="208px" runat="server">
                            <%--    onselectedindexchanged="ddl_ME_SelectedIndexChanged"--%>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkDisplay" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkDisplay_Click" Font-Bold="True">Display</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td width="50%">
                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblSiteNm" Visible="false" runat="server" Font-Bold="true" Text="Select Site"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txt_Site" runat="server" Width="280px" AutoPostBack="true" 
                       Visible="false"   OnTextChanged="txt_Site_TextChanged"
                         Text=""></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetSitename" MinimumPrefixLength="0" OnClientItemSelected="SiteItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Site"
                            ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfSiteId" runat="server" />
                        <asp:Label ID="lblSiteId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" valign="top">
                        <asp:Panel ID="pnlReport" runat="server" ScrollBars="Auto" Height="400px" Width="940px">
                            <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                                CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                                Width="100%" CellPadding="0" CellSpacing="0">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSrNo" runat="server" Text='<%# Eval("Sr_No") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Me Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMEName" runat="server" Text='<%# Eval("USER_Name_var") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="date" HeaderText="Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:TemplateField HeaderText="Client Name">
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblCL_Name_var" runat="server" Text='<%# Eval("CL_Name_var")%>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="300px" HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Name">
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblSITE_Name_var" runat="server" Text='<%# Eval("SITE_Name_var")%>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="300px" HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="in_time" HeaderText="In Time" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:hh:mm tt}" />
                                    <asp:BoundField DataField="out_time" HeaderText="Out Time" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:hh:mm tt}" />
                                    <asp:BoundField DataField="contact_person" HeaderText="Contact Person" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="contact_no" HeaderText="Contact No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="designation" HeaderText="Designation" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Lead_discription" HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Response" HeaderText="Response" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Next_date" HeaderText="Next Visit date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:TemplateField HeaderText="Site Address">
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblSITE_Address_var" runat="server" Text='<%# Eval("SITE_Address_var")%>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="270px" HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SITE_BulidUpArea_dec" HeaderText="BuildUp Area" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SITE_RERA_var" HeaderText="RERA No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="SITE_ConstPeriod_var" HeaderText="Constraction Period (In Months)"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SITE_ComplitionDate_dt" HeaderText="Complition Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="SITE_RccConsltnt_var" HeaderText="RCC Consultant" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="SITE_Architect_var" HeaderText="Architect" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="SITE_ConstMangmt_var" HeaderText="Construction Management"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                    <%--  <asp:TemplateField HeaderText="OutSource name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOutSourceName" runat="server" Text='<%# Eval("OutSourceName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>--%>
                                    <asp:BoundField DataField="SITE_GeoInvstgn_var" HeaderText="GeoTechnical Investigation"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="SITE_BldgsUnderConst_int" HeaderText="Buildings under construction"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SITE_WorkStatus_ForRCC" HeaderText="WorkStatus For RCC"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="SITE_WorkStatus_ForBWP" HeaderText="WorkStatus For Block Work Plaster"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="SITE_WorkStatus_ForFinishesh" HeaderText="WorkStatus For Finishes"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="SITE_ProposedBldgs_int" HeaderText="Proposed Buildings"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SITE_StartDate_dt" HeaderText="Proposed Start date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="SITE_CompletedBldgs_int" HeaderText="Completed Bulidings"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SITE_CurrTestingDetails_var" HeaderText="Current Testing Details"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                    <%-- <asp:TemplateField HeaderText="Site KYC">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteId" runat="server" Text='<%# Eval("Site_Id") %>' Visible="false"></asp:Label>
                                            <asp:LinkButton ID="lnkViewSiteKyc" runat="server" Style="text-decoration: underline;"
                                                Text="View" CommandName="ViewSiteKyc"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
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
                        </asp:Panel>
                    </td>
                </tr>
                
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
        function SiteItemSelected(sender, e) {
            $get("<%=hfSiteId.ClientID %>").value = e.get_value();
        }
    </script>
</asp:Content>
