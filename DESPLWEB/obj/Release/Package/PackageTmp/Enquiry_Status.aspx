<%@ Page Language="C#" Title="Enquiry Status " MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="Enquiry_Status.aspx.cs" Inherits="DESPLWEB.Enquiry_Status" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="lblEnuiry" runat="server" Text="From Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_FromDate" Width="143px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txt_ToDate" Width="149px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Filter For"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_EnquiryGridColumnList" Width="150px" AutoPostBack="true"
                            runat="server" OnSelectedIndexChanged="ddl_EnquiryGridColumnList_SelectedIndexChanged">
                            <asp:ListItem Text="All" />
                            <asp:ListItem Text="Client Name" />
                            <asp:ListItem Text="Site Name" />
                            <asp:ListItem Text="Test Type" />
                            <asp:ListItem Text="Enquiry Status" />
                            <asp:ListItem Text="Route Not Assigned" />
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:Label ID="Label1" runat="server" Text="="></asp:Label>
                        &nbsp;
                        <asp:TextBox ID="txt_Filter" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txt_Site" runat="server" Width="200px" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txt_TestType" runat="server" Width="200px" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txt_EnqStatus" runat="server" Width="200px" Visible="false"></asp:TextBox>
                        &nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="0" CompletionInterval="10"
                            EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Filter" ID="AutoCompleteExtender1"
                            runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:AutoCompleteExtender ServiceMethod="GetSitename" MinimumPrefixLength="0" CompletionInterval="10"
                            EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Site" ID="AutoCompleteExtender2"
                            runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:AutoCompleteExtender ServiceMethod="GetTestType" MinimumPrefixLength="0" CompletionInterval="10"
                            EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_TestType" ID="AutoCompleteExtender3"
                            runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:AutoCompleteExtender ServiceMethod="GetEnqStatus" MinimumPrefixLength="0" CompletionInterval="10"
                            EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_EnqStatus"
                            ID="AutoCompleteExtender4" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblnoofRecrds" runat="server" Text="Total No of Records : 0"></asp:Label>
                        
                        
                    </td>
                    <td>
                        <asp:CheckBox ID="chkHtmlPrint" Text="Html" runat="server" />
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Print</asp:LinkButton>
                          &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrintTestRequest" OnClick="lnkPrintTestRequest_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Print TestRequest</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="height: 23px" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="390px"
                            Width="940px" BorderColor="AliceBlue">
                           <%--    <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 410px; overflow: auto">--%>
                            <asp:GridView ID="grdEnquiry" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                DataKeyNames="ENQ_Id" ForeColor="#333333" GridLines="Vertical" BorderColor="#DEDFDE"
                                BorderWidth="1px" Width="100%" OnRowDataBound="grdEnquiry_RowDataBound">
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                     <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll"  OnCheckedChanged="chkSelectAll_CheckedChanged"  AutoPostBack="true" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect"   OnCheckedChanged="chkSelect_CheckedChanged"  AutoPostBack="true" runat="server" /><%----%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    <asp:TemplateField >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" ToolTip="View Enquiry" Font-Underline="true" OnCommand="lnkView_Oncommand"
                                                CommandArgument='<%#Eval("ENQ_Id")%>' runat="server"> View </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ENQ_Id" HeaderText="Enquiry No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ENQ_Date_dt" HeaderText="Enquiry Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MATERIAL_Name_var" HeaderText="Test Type" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ENQ_OpenEnquiryStatus_var" HeaderText="Enquiry Type" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ENQ_Status_tint" HeaderText="Enquiry Status" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ENQ_ApprovedBy" HeaderText="Approved by" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ENQ_ApproveDate_dt" HeaderText="Approved On" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ENQ_CollectedOn_dt" HeaderText="Collected On" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ENQ_ClosedOn_dt" HeaderText="Closed On" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MATERIAL_RecordType_var" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="INWD_RecordNo_int" HeaderText="Record No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ENQ_Note_var" HeaderText="Note" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ENQ_Reference_var" HeaderText="Reference" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Proposal_No" HeaderText="Proposal No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="INWD_BILL_Id" HeaderText="Bill No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BILL_NetAmt_num" HeaderText="Bill Amount" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="" HeaderText="Outstanding" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ENQ_EnteredBy" HeaderText="Entered by" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RouteName" HeaderText="Route" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MEName" HeaderText="Marketing Executive" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                      <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                      <asp:BoundField DataField="ENQ_MobileAppEnqNo_int" HeaderText="App EnqNo" HeaderStyle-HorizontalAlign="Center"
                                     ItemStyle-HorizontalAlign="Center" NullDisplayText="0" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="INWD_ReferenceNo_int" HeaderText="Reference No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="#CCCC99" />
                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                <EmptyDataTemplate>
                                    No records to display
                                </EmptyDataTemplate>
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                               <%-- <EditRowStyle BackColor="#999999" />--%>
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                          <%--  </div></div>--%>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                   <asp:Label ID="lblGrayColor" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Orange colour indicates Enquiry from App. &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                            BackColor="#FFCC99"></asp:Label>
                    </td>
               
                </tr>
            </table>
        </asp:Panel>
    </div>
     <script type="text/javascript">

        function checkAll(gvExample, colIndex) {

            var GridView = gvExample.parentNode.parentNode.parentNode;

            for (var i = 1; i < GridView.rows.length; i++) {

                var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];

                chb.checked = gvExample.checked;

            }

        }

 

        function checkItem_All(objRef, colIndex) {

            var GridView = objRef.parentNode.parentNode.parentNode;

            var selectAll = GridView.rows[0].cells[colIndex].getElementsByTagName("input")[0];
            var enqNo = GridView.rows[0].cells[24].getElementsByTagName("input")[0];

            if (!objRef.checked) {

                selectAll.checked = false;

            }

            else {

                var checked = true;

                for (var i = 1; i < GridView.rows.length; i++) {

                    var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];

                    if (!chb.checked) {

                        checked = false;

                        break;

                    }
                    if(enqNo!=0)
                        chb.parentElement.parentElement.style.backgroundColor = '#FFCC99';
                }

                selectAll.checked = checked;

            }

         


        }
         </script>
      <%--<script type="text/javascript">

        function OnOneCheckboxSelected(chkB) {
            var IsChecked = chkB.checked;
            var Parent = document.getElementById('<%= this.grdEnquiry.ClientID %>');
            var cbxAll;
            var items = Parent.getElementsByTagName('input');
            var bAllChecked = true;
            for (i = 0; i < items.length; i++) {
                if (chkB.checked) {
                   // chkB.parentElement.parentElement.style.backgroundColor = '#FFCC99';
                }
                else {
                    //chkB.parentElement.parentElement.style.backgroundColor = '#FFCC99';
                }
                if (items[i].id.indexOf('cbxSelectAll') != -1) {
                    cbxAll = items[i];
                    continue;
                }
                if (items[i].type == "checkbox" && items[i].checked == false) {
                    bAllChecked = false;
                    break;
                }
            }
            cbxAll.checked = bAllChecked;
        }

        function SelectAllCheckboxes(spanChk) {
            var IsChecked = spanChk.checked;
            var cbxAll = spanChk;
            var Parent = document.getElementById('<%= this.grdEnquiry.ClientID %>');
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != cbxAll.id && items[i].type == "checkbox") {
                    items[i].checked = IsChecked;
                    if (items[i].checked) {
                      //  items[i].parentElement.parentElement.style.backgroundColor = '#FFCC99';
                    }
                    else {
                      //  items[i].parentElement.parentElement.style.backgroundColor = '#FFCC99';
                    }
                }
            }
        }
    </script>--%>
</asp:Content>
