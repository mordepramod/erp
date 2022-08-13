<%@ Page Title="Complaint List" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="ComplaintList.aspx.cs" Inherits="DESPLWEB.ComplaintList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                
                <tr>
                    
                    <td>
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label>&nbsp;&nbsp;
                        <asp:TextBox ID="txt_FromDate" Width="143px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="lblToDate" runat="server" Text="To Date"></asp:Label>&nbsp;&nbsp;
                        <asp:TextBox ID="txt_ToDate" Width="149px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td>
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total No of Records : 0 "></asp:Label>
                    </td>
                                       <td align="right">
                                      <asp:LinkButton ID="lnkPrint" runat="server" OnClick="lnkPrint_Click" Font-Underline="true">Print</asp:LinkButton>
                       &nbsp; &nbsp;  <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 23px" valign="top">
                          <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto"  BorderStyle="Ridge"  Height="400px" Width="940px" BorderColor="AliceBlue" > 
                       
                            <asp:GridView ID="grdComplaint" runat="server" AutoGenerateColumns="False" CellPadding="4" 
                               ForeColor="#333333" BorderColor="#DEDFDE"  BackColor="#F7F6F3" 
                                BorderWidth="1px" Width="100%" >
                                <Columns>
                                      <asp:TemplateField >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkModify" runat="server" CommandArgument='<%#Eval("COMP_Id") %>'
                                                OnCommand="lnkModify_OnCommand"  Font-Underline="true"  Text="Modify">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" />
                                   </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Complaint No.">
                                       <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompId" runat="server" Text='<%#Eval("COMP_Id") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Complaint Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblComplaintDate" runat="server" Text='<%#Eval("COMP_Date_dt") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" />
                                       <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Client Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("CL_Name_var") %>' />
                                        </ItemTemplate>
                                          <ItemStyle Width="150px" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" runat="server" Text='<%#Eval("SITE_Name_var") %>' />
                                        </ItemTemplate>
                                          <ItemStyle Width="150px" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Complaint Status" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblComplaintStatus" runat="server" Text='<%#Eval("Complaint_Status") %>' />
                                        </ItemTemplate>
                                      <ItemStyle Width="100px" Wrap="False" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Complaint Type">
                                    <ItemTemplate>
                                            <asp:Label ID="lblComplaintType" runat="server" Text='<%#Eval("Complaint_Type") %>' />
                                      </ItemTemplate>
                                    <ItemStyle  Width="100px" Wrap="False" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Complaint Details">
                                    <ItemTemplate>
                                            <asp:Label ID="lblComplaintDetails" runat="server" Text='<%#Eval("COMP_DetailsOfComplaint_var") %>' />
                                      </ItemTemplate>
                                    <ItemStyle   Width="200px" Wrap="False" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Attended By">
                                    <ItemTemplate>
                                            <asp:Label ID="lblAttendedBy" runat="server" Text='<%#Eval("COMP_AttendedBy") %>' />
                                      </ItemTemplate>
                                    <ItemStyle   Width="150px" Wrap="False" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Inward Type">
                                    <ItemTemplate>
                                            <asp:Label ID="lblCOMP_RecordType" runat="server" Text='<%#Eval("COMP_RecordType") %>' />
                                      </ItemTemplate>
                                    <ItemStyle Width="150px" Wrap="False" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Action Initiated">
                                    <ItemTemplate>
                                            <asp:Label ID="lblCOMP_ActionIntiated" runat="server" Text='<%#Eval("COMP_ActionIntiated") %>' />
                                      </ItemTemplate>
                                    <ItemStyle   Width="150px" Wrap="False" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Closure Date">
                                    <ItemTemplate>
                                            <asp:Label ID="lblCOMP_ClouserDate" runat="server" Text='<%#Eval("COMP_ClouserDate") %>' />
                                      </ItemTemplate>
                                    <ItemStyle  Width="100px" Wrap="False" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Comment of Tech. Officer"> 
                                    <ItemTemplate>
                                            <asp:Label ID="lblCOMP_CommentOfTechOfficer" runat="server" Text='<%#Eval("COMP_CommentOfTechOfficer") %>' />
                                      </ItemTemplate>
                                    <ItemStyle   Width="350px" Wrap="False" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Action By">
                                    <ItemTemplate>
                                            <asp:Label ID="lblCOMP_ActionBy_var" runat="server" Text='<%#Eval("COMP_ActionBy_var") %>' />
                                      </ItemTemplate>
                                    <ItemStyle   Width="150px" Wrap="False" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Complaint CreatedBy">
                                    <ItemTemplate>
                                            <asp:Label ID="lblCOMP_CreatedBy_var" runat="server" Text='<%#Eval("COMP_CreatedBy_var") %>' />
                                      </ItemTemplate>
                                    <ItemStyle   Width="150px" Wrap="False" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Reviewed By M.D.">
                                    <ItemTemplate>
                                            <asp:Label ID="lblCOMP_ReviewBy_var" runat="server" Text='<%#Eval("COMP_ReviewBy_var") %>' />
                                      </ItemTemplate>
                                    <ItemStyle  Width="100px" Wrap="False" />
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
    
</asp:Content>
