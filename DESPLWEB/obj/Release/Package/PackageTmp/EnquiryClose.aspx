<%@ Page Title=" Enquiry Close" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="EnquiryClose.aspx.cs" Inherits="DESPLWEB.EnquiryClose"  Theme="duro"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="lblEnuiry" runat="server"  Text="From Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_FromDate"  Width="143px" runat="server"></asp:TextBox>
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
                        <asp:Label ID="Label2" runat="server" Text="Filter For" ></asp:Label>
                    </td>
                      <td>
                        <asp:DropDownList ID="ddl_EnquiryGridColumnList" Width="150px" AutoPostBack="true"
                            runat="server" OnSelectedIndexChanged="ddl_EnquiryGridColumnList_SelectedIndexChanged">
                            <asp:ListItem Text="Pending" />
                            <asp:ListItem Text="Closed" />
                            <asp:ListItem Text="All" />
                            <asp:ListItem Text="Client Name" />
                            <asp:ListItem Text="Site Name" />
                            <asp:ListItem Text="Test Type" />
                           
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:Label ID="Label1" runat="server" Text="="></asp:Label>
                        &nbsp;
                        <asp:TextBox ID="txt_Filter" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txt_Site" runat="server" Width="200px" Visible="false" ></asp:TextBox>
                        <asp:TextBox ID="txt_TestType" runat="server" Width="200px" Visible="false" ></asp:TextBox>
                       
                        &nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td>
                                    <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="0"  
                                     CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Filter"  
                                     ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" 
                                     CompletionListCssClass="autocomplete_completionListElement" >
                                    </asp:AutoCompleteExtender>  
                                    <asp:AutoCompleteExtender ServiceMethod="GetSitename" MinimumPrefixLength="0"  
                                     CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Site"  
                                     ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" 
                                     CompletionListCssClass="autocomplete_completionListElement" >
                                    </asp:AutoCompleteExtender>  
                                    <asp:AutoCompleteExtender ServiceMethod="GetTestType" MinimumPrefixLength="0"  
                                     CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_TestType"  
                                     ID="AutoCompleteExtender3" runat="server" FirstRowSelected="false" 
                                     CompletionListCssClass="autocomplete_completionListElement" >
                                    </asp:AutoCompleteExtender>  
                                 
                    </td>
                    <td>
                        <asp:Label ID="lblnoofRecrds" runat="server"  Text="Total No of Records : 0"></asp:Label>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" runat="server" Font-Bold="True" Style="text-decoration: underline;" >Print</asp:LinkButton>
                   </td>
                </tr>
                <tr>
                    <td colspan="6" style="height: 23px" valign="top">
                          <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto"  BorderStyle="Ridge"  Height="410px" Width="940px" BorderColor="AliceBlue" > 
                             <div style="width: 940px;">
                    <div id="GHead" >
                    </div>
                    <div style="height: 410px; overflow: auto">
                            <asp:GridView ID="grdEnquiryClose" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                DataKeyNames="ENQ_Id" ForeColor="#333333" GridLines="Vertical" BorderColor="#DEDFDE"
                                BorderWidth="1px" Width="100%" OnRowDataBound="grdEnquiryClose_RowDataBound">
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                   <asp:TemplateField HeaderText="View" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView"  ToolTip="View Enquiry"   Font-Underline="true"  OnCommand="lnkView_Oncommand" CommandArgument='<%#Eval("ENQ_Id")%>'  runat="server"  > View </asp:LinkButton>
                                        </ItemTemplate>
                                       <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEnqClose"  ToolTip="Close Enquiry"   Font-Underline="true"  OnCommand="lnkEnqClose_Oncommand" CommandArgument='<%#Eval("ENQ_Id")%>'  runat="server"  > Close </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Client Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientName" Width="150px" runat="server" Text='<%#Eval("CL_Name_var") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" Width="150px" runat="server" Text='<%#Eval("SITE_Name_var") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Enquiry No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEnquiryId" runat="server" Text='<%#Eval("ENQ_Id") %>' />
                                        </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Enquiry Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEnquiryDate" runat="server" Text='<%#Eval("ENQ_Date_dt" ,"{0:dd/MM/yyyy}") %>' />
                                        </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Test Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMaterialName" Width="100px"  runat="server" Text='<%#Eval("MATERIAL_Name_var") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Enquiry Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOpenEnquiryStatus_var" Width="100px"  runat="server" Text='<%#Eval("ENQ_OpenEnquiryStatus_var") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ENQ_Status_tint" HeaderText="Enquiry Status" HeaderStyle-HorizontalAlign="Center" 
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField  HeaderText="Comment" >
                                       <ItemTemplate>
                                           <asp:TextBox ID="txt_Comment" MaxLength="100" Width="100px" Text='<%#Eval("ENQ_CloseComment") %>' runat="server"></asp:TextBox>
                                       </ItemTemplate>
                                       <ItemStyle Width="100px" />
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
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                            </div></div>

                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>                
            </table>
        </asp:Panel>
    </div>
     <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdEnquiryClose.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdEnquiryClose.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdEnquiryClose.ClientID%>').offset().top);

        });
    </script>
</asp:Content>
