<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="ClientSiteChange.aspx.cs" Inherits="DESPLWEB.ClientSiteChange" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td style="width: 17%">
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>
                    </td>
                    <td style="width: 40%">
                        <asp:TextBox ID="txtFromDate" Width="148px" runat="server" AutoPostBack="true" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td style="width: 18%">
                        <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>
                    </td>
                    <td style="width: 25%">
                        <asp:TextBox ID="txtToDate" Width="148px" runat="server" AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td style="width: 10%"></td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td colspan="2">&nbsp;
                    </td>
                    <td>
                        <asp:RadioButton ID="optRefNo" Text="Reference No." runat="server" AutoPostBack="true"
                            GroupName="g1" OnCheckedChanged="optRefNo_CheckedChanged" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="optRecNo" Text="Record No." runat="server" GroupName="g1" AutoPostBack="true"
                            OnCheckedChanged="optRecNo_CheckedChanged" />
                        &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <asp:Label ID="lblInwardType" runat="server" Text="Inward Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlInwardType" Width="155px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlInwardType_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td valign="top">
                        <asp:Label ID="lblRefRecNo" runat="server" Text="Select Ref/Record No"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRefRecNo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRefRecNo_SelectedIndexChanged"
                            Width="155px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblBillNo" runat="server" Text="Previous Bill No." Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPrvBillNo" runat="server" Visible="false" Width="155px"></asp:TextBox>
                        &nbsp;
                        <asp:LinkButton ID="lnkUpdatePrvBillNo" runat="server" Font-Underline="True"
                            Visible="false" OnClick="lnkUpdatePrvBillNo_Click">Update Previous BillNo</asp:LinkButton>
                    </td>
                </tr>
                <%--  <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        <asp:Label ID="lblClientName" runat="server" Text="Client Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtClientName" runat="server" Width="307px" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblNewClientName" runat="server" Text="New Client Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNewClient" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txtNewClient_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtNewClient"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>

                    </td>
                </tr>
                
                <tr>
                    <td>
                        <asp:Label ID="lblSiteName" runat="server" Text="Site Name"></asp:Label>

                    </td>
                    <td>
                        <asp:TextBox ID="txtSiteName" runat="server" Width="307px" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblNewSiteName" runat="server" Text="New Site Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNewSite" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txtNewSite_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetSitename" MinimumPrefixLength="1" OnClientItemSelected="SiteItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtNewSite"
                            ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfSiteId" runat="server" />
                        <asp:Label ID="lblSiteId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Contact Person"></asp:Label>
                    </td>
                    <td>  
                        <asp:TextBox ID="txtContactPerson" runat="server" Width="307px" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="New Contact Person"></asp:Label>
                    </td>
                    <td>
                          <asp:DropDownList ID="ddlNewContactPerson" Width="312px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlNewContactPerson_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Contact No"></asp:Label>
                    </td>
                    <td><asp:TextBox ID="txtContactNo" runat="server" Width="307px" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="New Contact No"></asp:Label>
                    </td>
                    <td><asp:TextBox ID="txtNewContactNo" runat="server" Height="16px" Width="307px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Contact Email"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmailId" runat="server" Width="307px" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="New Contact Email"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNewEmailId" runat="server" Height="16px" Width="307px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="right">
                        <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkSave_Click" Font-Bold="True">Save</asp:LinkButton>
                        &nbsp; &nbsp;
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
