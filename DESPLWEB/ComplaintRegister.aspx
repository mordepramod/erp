<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="ComplaintRegister.aspx.cs" Inherits="DESPLWEB.ComplaintRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 480px;">
        <asp:Panel ID="pnlDetails" runat="server" Width="100%" BorderColor="AliceBlue" BorderStyle="Ridge">
            <table width="100%">
                <tr>
                    <td style="width: 20%">
                        <asp:Label ID="Label14" runat="server" Text="Complaint No."></asp:Label><asp:Label
                            ID="lblCompId" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td style="width: 35%">
                        <asp:TextBox ID="txt_ComplaintNo" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                    </td>
                    <td style="width: 18%">
                        <asp:Label ID="Label8" runat="server" Text="Complaint Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_date" runat="server" Width="230px" onchange="javascript:DateValid();"
                            Enabled="false"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_date">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_date" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        <asp:Label ID="lblLoginId" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <asp:Label ID="Client" runat="server" Text="Client Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Client" runat="server" Width="250px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Visible="false"></asp:Label>
                            </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Site Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Site" runat="server" Width="230px" AutoPostBack="true" OnTextChanged="txt_Site_TextChanged"
                            Text=""></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetSitename" MinimumPrefixLength="0" OnClientItemSelected="SiteItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Site"
                            ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfSiteId" runat="server" />
                        <asp:Label ID="lblSiteId" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label15" runat="server" Text="Client Representative"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_ClRepresentative" runat="server" Width="250px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="Type of Complaint"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_ComplaintType" runat="server" Width="255px">
                            <asp:ListItem>---Select---</asp:ListItem>
                            <asp:ListItem Value="1">Service (Collection & Report)</asp:ListItem>
                            <asp:ListItem Value="2">Technical</asp:ListItem>
                            <asp:ListItem Value="3">Marketing</asp:ListItem>
                            <asp:ListItem Value="4">Testing</asp:ListItem>
                            <asp:ListItem Value="5">Billing</asp:ListItem>
                            <asp:ListItem Value="6">Response</asp:ListItem>
                            <asp:ListItem Value="7">Other</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label10" runat="server" Text="Details of Complaint"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_DetailsOfComplaint" runat="server" Width="700px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label16" runat="server" Text="Complaint Attended By"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_CompAttendedBy" Width="255px" runat="server">
                            <%-- AutoPostBack="true" OnSelectedIndexChanged="ddl_InwardTestType_SelectedIndexChanged"--%>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <asp:Label ID="Label5" runat="server" Text="Inward Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_RecordType" Width="255px" runat="server" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <%--  <asp:Label ID="lblEnqNo" runat="server" Text="Enquiry No"></asp:Label>--%>
                        <asp:Label ID="lblRefNo" runat="server" Text="Reference No"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_RefenceNo" runat="server" Width="230px" onchange="checkNum(this)"></asp:TextBox>
                        <%--  <asp:DropDownList ID="ddl_EnquiryList" runat="server" Width="235px">
                        </asp:DropDownList>--%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <asp:Label ID="Label9" runat="server" Text="Corrective Action  Initiated"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_CorrAction" runat="server" Width="700px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <asp:Label ID="Label3" runat="server" Text="Complaint Status"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_ActionStatus" runat="server" Width="255px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_ActionStatus_SelectedIndexChanged">
                            <asp:ListItem Value="0">Open</asp:ListItem>
                            <asp:ListItem Value="1">Close</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lblActionTime" runat="server" Text="Time Taken For Action" Visible="false"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_ActionTime" runat="server" Width="230px" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label11" runat="server" Text="Complaint Form Ref."></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_ComplaintFormRef" runat="server" Width="250px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label12" runat="server" Text="Complaint Closure Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_ClosureDate" runat="server" Width="230px" onchange="javascript:DateValid();"
                            Height="16px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ClosureDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_ClosureDate"
                            MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label13" runat="server" Text="Comment of Technical Officer"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_TechOfficerComment" runat="server" Width="700px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Action By"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_ActionBy" runat="server" Width="235px">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:CheckBox ID="cb_ReviewedBy" runat="server" Text="Reviewed By M.D." />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="right">
                        <asp:LinkButton ID="LnkBtnSave" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="LnkBtnSave_Click" CssClass="SimpleColor">Save</asp:LinkButton>&nbsp;
                        &nbsp; &nbsp; &nbsp;
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            runat="server" OnClick="LnkExit_Click">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <script type="text/javascript">


        function checkNum(x) {
            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {

                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Numeric Values Allowed";
                    x.value = '';
                    x.focus();
                    return false;
                }
                else {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                }

            }
            return true;
        }


        function DateValid() {
            var lblmsg = document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>');
            lblmsg.style.visibility = "hidden";
            var myDate1 = document.getElementById("<%=txt_date.ClientID%>").value;
            var date = myDate1.substring(0, 2);
            var month = myDate1.substring(3, 5);
            var year = myDate1.substring(6, 10);

            var myDate = new Date(year, month - 1, date);

            var today = new Date();

            if (myDate > today) {

                lblmsg.innerHTML = "Date should not be greater than current date";
                lblmsg.style.visibility = "visible";
                return false;
                myDate1.focus();
            }
            else {
                return true;
            }

        }

      
    </script>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
        function SiteItemSelected(sender, e) {
            $get("<%=hfSiteId.ClientID %>").value = e.get_value();
        }
         
    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
