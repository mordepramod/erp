<%@ Page Title="Bill Status" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="BillStatusUser.aspx.cs" Inherits="DESPLWEB.BillStatusUser" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td width="12%">
                        <asp:Label ID="lblBilldt" runat="server" Text="Bill Date "></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtFromDate" Width="100px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txtToDate" Width="100px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="lblBillNo" runat="server" Text="Bill No. "></asp:Label>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txtBillNo" Width="100px" MaxLength="50" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkEnteredBillPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkEnteredBillPrint_Click" Font-Bold="True">Print</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkMonthlyBillDetail" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkMonthlyBillDetail_Click" Font-Bold="True">Monthly Bill Details</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblBranch" runat="server" ForeColor="Brown" Text="" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblLocation" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                        <asp:Label ID="lblConnection" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                        <asp:Label ID="lblConnectionLive" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                    </td>
                    <td align="right">
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblInwdtype" runat="server" Text="Inward Type"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_InwardTestType" Width="150px" runat="server">
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td align="left"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStatus" runat="server" Text="Bill Status"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" Width="70px" runat="server">
                            <asp:ListItem Text="Ok" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Cancel" Value="1"></asp:ListItem>
                        </asp:DropDownList>&nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />

                        &nbsp;&nbsp;
                        <asp:Label ID="lblClientApproveStatus" runat="server" Text="Client Approval"></asp:Label>&nbsp;&nbsp;
                         <asp:DropDownList ID="ddlClientApproveStatus" Width="80px" runat="server">
                             <asp:ListItem Text="All" Value="2"></asp:ListItem>
                             <asp:ListItem Text="Pending" Value="0"></asp:ListItem>
                             <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                         </asp:DropDownList>&nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearchClApprStatusWise" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearchClApprStatusWise_Click" Style="margin-top: 0px" />
                    </td>
                    <td colspan="2" align="right">
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                             OnClick="lnkPrint_Click" Font-Bold="True">Print List</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="5" style="height: 5px"></td>
                </tr>

                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblTotal" runat="server" Text="" Font-Bold="true" ForeColor="OrangeRed"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblPaidAmount" runat="server" Text="" Font-Bold="true" ForeColor="OrangeRed"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblPendingAmount" runat="server" Text="" Font-Bold="true" ForeColor="OrangeRed"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlBillList" runat="server" ScrollBars="Auto" Height="370px" Width="880px"
                BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">

                <asp:GridView ID="grdModifyBill" SkinID="gridviewSkin1" runat="server" AutoGenerateColumns="False"
                    OnRowCommand="grdModifyInward_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="BILL_Id" HeaderText="Bill No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_Date_dt" HeaderText="Bill Date" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_RecordType_var" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_RecordNo_int" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ReferenceNo_int" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="GrossAmount" HeaderText="Gross Amount" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_NetAmt_num" HeaderText="Bill Amount" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_DiscountAmt_num" HeaderText="Discount" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_SerTaxAmt_num" HeaderText="Service Tax" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_SwachhBharatTaxAmt_num" HeaderText="Swachh Bharat Cess"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_KisanKrishiTaxAmt_num" HeaderText="Krishi Kalyan Cess"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_CGSTAmt_num" HeaderText="CGST Amt" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_SGSTAmt_num" HeaderText="SGST Amt" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_IGSTAmt_num" HeaderText="IGST Amt" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_PaidAmount" HeaderText="Paid Amount" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n}" />
                        <asp:BoundField DataField="BILL_PendingAmount" HeaderText="Pending Amount" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" DataFormatString="{0:n}" />
                        <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CL_Id" HeaderText="Client Id" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="SITE_Id" HeaderText="Site Id" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_CL_GSTNo_var" HeaderText="Client GSTIN" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="BILL_SITE_GSTNo_var" HeaderText="Site GSTIN" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="CL_State_var" HeaderText="Client State" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="SITE_State_var" HeaderText="Site State" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="RouteName" HeaderText="Route" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Section">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddl_Inward" Width="180px" runat="server">
                                </asp:DropDownList>
                                <asp:Label ID="lblSectionId" Text='<%#Eval("Bill_SectionID_int") %>' runat="server" Visible="false" />
                            </ItemTemplate>
                            <ItemStyle Width="30%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPrintBill" runat="server" ToolTip="Print Bill" Style="text-decoration: underline;"
                                    CommandArgument='<%#Eval("Bill_Id")%>' CommandName="lnkPrintBill">&nbsp;Print&nbsp;</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblApproveStatus" runat="server" Text='<%# Bind("BILL_ApproveStatus_bit") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BILL_Reason_var" HeaderText="Reason" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Download PO">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDownloadFile" runat="server" Text='<%# Eval("BILL_POFileName_var") %>' ToolTip="Download File" Style="text-decoration: underline;"
                                    CommandArgument='<%#Eval("BILL_POFileName_var")%>' CommandName="DownloadFile"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Download MOU">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDownloadMOUFile" runat="server" Text='<%# Eval("CL_MOUFileName_var") %>' ToolTip="Download File" Style="text-decoration: underline;"
                                    CommandArgument='<%#Eval("CL_MOUFileName_var")%>' CommandName="DownloadMOUFile"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Client Approval">
                            <ItemTemplate>
                                <asp:Label ID="lblClientApproveStatus" runat="server" Text='<%# Bind("BILL_ClientApproveStatus") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Client Approval By">
                            <ItemTemplate>
                                <asp:Label ID="lblClientApprovedBy" runat="server" Text='<%# Bind("BILL_ClientApprovedBy") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Client Approval Date">
                            <ItemTemplate>
                                <asp:Label ID="lblClientApproveDate" runat="server" Text='<%# Eval("BILL_ClientApproveDate_dt","{0:dd/MM/yyyy hh:mm tt}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="View Proposal">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewProposal" runat="server" Text='<%# Eval("Proposal_No") %>' ToolTip="View Proposal" Style="text-decoration: underline;"
                                    CommandArgument='<%#Eval("Proposal_EnqNo") + ";" + Eval("Proposal_Id") + ";" + Eval("Proposal_No") %>' CommandName="ViewProposal"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
    </script>

</asp:Content>

