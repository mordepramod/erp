<%@ Page Title="BillWise Report Details" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="BillWiseReportDetails.aspx.cs" Inherits="DESPLWEB.BillWiseReportDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="12%">
                        <asp:Label ID="Label1" runat="server" Text="From Date"></asp:Label>
                    </td>
                    <td style="width: 53%" colspan="2">
                        <asp:TextBox ID="txt_FromDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                           <asp:Label ID="Label3" runat="server" Text="To Date"></asp:Label>  &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txt_ToDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    
                    <td style="width: 348px">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        
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
                    <td style="width: 43%">
                        <asp:DropDownList ID="ddl_InwardTestType" Width="155px" runat="server"
                           >
                        </asp:DropDownList>
                        &nbsp; &nbsp; &nbsp;
                        <%--<asp:DropDownList ID="ddl_ReportType" runat="server" Width="140px">
                            <asp:ListItem Text="All" Value="0" />
                            <asp:ListItem Text="Enter" Value="1" />
                            <asp:ListItem Text="Check" Value="2" />
                            <asp:ListItem Text="Approve" Value="3" />
                            <asp:ListItem Text="Print" Value="4" />
                            <asp:ListItem Text="Outward" Value="5" />
                            <asp:ListItem Text="Physical Outward" Value="6" />
                            <asp:ListItem Text="Duplicate Print" Value="8" />
                        </asp:DropDownList>--%>
                        &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" 
                            ImageAlign="TextTop" />
                    </td>
                    <td colspan="2" align="right">
                    <asp:LinkButton ID="lnkPrint" runat="server" Font-Bold="True" OnCommand="lnkPrint_Click"
                            Style="text-decoration: underline;" >Print</asp:LinkButton>
                        <%--<asp:Label ID="Label4" runat="server" Text="Client Specific"></asp:Label>--%>
                    </td>
                    <%--<td align="left" style="width: 348px">
                        &nbsp;</td>--%>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="400px"
                Width="940px" BorderColor="AliceBlue">
          
                        <asp:GridView ID="grdReportStatus" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                            CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333"
                            Width="100%" CellPadding="4">
                            <Columns>
                                <asp:BoundField DataField="BILL_Id" HeaderText="Bill No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField><asp:BoundField DataField="BILL_Date_dt" HeaderText="Bill Date" HeaderStyle-HorizontalAlign="Center"  DataFormatString="{0:dd/MM/yyyy}"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="BILL_NetAmt_num" HeaderText="Bill Amount" 
                                    HeaderStyle-HorizontalAlign="Center"  DataFormatString="{0:n}"
                                    ItemStyle-HorizontalAlign="Center" NullDisplayText="0.00" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                    <asp:BoundField DataField="BILL_PaidAmount" HeaderText="Paid Amount" 
                                    HeaderStyle-HorizontalAlign="Center"  DataFormatString="{0:n}"
                                    ItemStyle-HorizontalAlign="Center" NullDisplayText="0.00" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BILL_PendingAmount" HeaderText="Pending Amount"  DataFormatString="{0:n}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" 
                                    NullDisplayText="0.00" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CL_Name_var" HeaderText="Client name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Site_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="TestName" HeaderText="Test Type" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="MEName" HeaderText="ME Name" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <%--  <asp:BoundField DataField="RecordType" HeaderText="Record Type"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="BILL_RecordNo_int" HeaderText="Record No"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Left" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="rptStatus" HeaderText="Report Status" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                               <asp:BoundField DataField="DispatchDate" HeaderText="Disptd. Date" HeaderStyle-HorizontalAlign="Center"
                                   DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" >
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
                            <EditRowStyle BackColor="#999999" />
                        </asp:GridView>
                   </asp:Panel>
        </asp:Panel>
    </div>
  
   <%-- <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>--%>
</asp:Content>
