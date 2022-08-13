<%@ Page Language="C#" Title="User Right" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="UserAccessRight.aspx.cs" Inherits="DESPLWEB.UserAccessRight" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        td.locked, th.locked
        {
            position: relative;
            top: expression(this.offsetParent.scrollTop);
            background-color: White;
        }
    </style>
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF; margin-top: 0px;">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <table width="100%">
                        <tr>
                            <td width="10%">
                                <asp:RadioButton ID="rdn_ActiveUser" runat="server" GroupName="userright" />
                                <asp:Label ID="Label1" runat="server" Text="Active User"></asp:Label>
                            </td>
                            <td width="10%">
                                <asp:RadioButton ID="rdn_AllUser" runat="server" GroupName="userright" />
                                <asp:Label ID="Label2" runat="server" Text="All User"></asp:Label>
                            </td>
                            <td width="25%">
                                <asp:TextBox ID="txt_UserSearch" Width="250px" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                                <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                                    OnClick="ImgBtnSearch_Click" />
                            </td>
                            <td align="right">
                                &nbsp;
                                <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                                    OnClick="imgClosePopup_Click" ImageAlign="Right" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="440px"
                        Width="945px" BorderColor="AliceBlue">
                        <asp:GridView ID="grdUserRight" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="False"
                            BackColor="#CCCCCC" OnRowDataBound="grdUserRight_RowDataBound" OnRowCommand="grdUserRight_RowCommand">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgInsertUser" runat="server" OnCommand="imgInsertUser_Click"
                                            ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                            BorderWidth="1px" ToolTip="Add New User" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgEditUser" runat="server" CommandArgument='<%#Eval("USER_Id") %> '
                                            OnCommand="imgEditUser_Click" ImageUrl="Images/Edit.jpg" Height="18px" Width="18px"
                                            BorderWidth="1px" CausesValidation="false" ToolTip="Edit User" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reset Password" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPwd" BorderWidth="0px" CommandName="ResetPwd" ToolTip="Reset Password"
                                            Style="text-decoration: underline;" CommandArgument='<%#Eval("USER_Id")%>' runat="server"
                                            OnClientClick="return confirm('Are you sure you want to Reset the Password ?');">Reset Pwd</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_UserName" Text='<%# Eval("USER_Name_var") %>' CssClass='<%#DataBinder.Eval(Container.DataItem, "USER_Id")%>'
                                            runat="server" ReadOnly="true" BorderWidth="0px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Designation" Text='<%# Eval("USER_Designation_var") %>' runat="server"
                                            ReadOnly="true" BorderWidth="0px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_EmailId" BorderWidth="0px" runat="server" Text='<%# Eval("USER_EmailId_var") %>'
                                            ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remote Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_RemoteId" BorderWidth="0px" runat="server" Text='<%# Eval("USER_RemoteId_var") %>'
                                            ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active Status" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkUSER_Status_bit_Right" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Entry " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkEntryRight" BorderWidth="0px" Width="50px" runat="server" onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Check " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCheckRight" BorderWidth="0px" Width="50px" runat="server" onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Re  Check " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chKRecheckRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Print " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkPrintRight" BorderWidth="0px" Width="50px" runat="server" onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkViewRight" BorderWidth="0px" Width="50px" runat="server" onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Enquiry Approval " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkApproveRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inwd " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkInwardRight" BorderWidth="0px" Width="50px" runat="server" onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bill " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBillRight" BorderWidth="0px" Width="50px" runat="server" onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Admin " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAdminRight" BorderWidth="0px" Width="50px" runat="server" onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Enquiry " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkEnquiryRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkUserRight" BorderWidth="0px" Width="50px" runat="server" onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Super Admin " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSuperAdminRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rpt Appr " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRptApprovalRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inwd Approve " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkInwardApproveRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mat. Collect " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCollectedStatusRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Receipt Appr " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkReceiptApproveRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAccountRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CRlimit Approve " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCRlimitApproveRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mkt " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkMktRight" BorderWidth="0px" Width="50px" runat="server" onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mat. Test " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkTestingRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mat. Recd " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkReceivingRight" BorderWidth="0px" Width="60px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rcpt Lock " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRcptLockRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Proposal " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkProposalRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CS " HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCSRight" BorderWidth="0px" Width="50px" runat="server" onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Client Appr " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkClientApprovalRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bill Print " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBillPrintRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AllEnq Appr " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAllEnqApprovalRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount Modify " HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDiscountModifyRight" BorderWidth="0px" Width="50px" runat="server"
                                            onclick="this.checked = !this.checked" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
            PopupControlID="pnlUser" PopupDragHandleControlID="PopupHeader" BackgroundCssClass="ModalPopupBG">
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnlUser" runat="server" Width="500px" ScrollBars="Auto" Height="570px">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="DetailPopup" width="100%">
                        <tr valign="top">
                            <td align="center" valign="bottom" colspan="4">
                                <asp:ImageButton ID="imgCloseUserPopup" runat="server" ImageUrl="Images/cross_icon.png"
                                    OnClick="imgCloseUserPopup_Click" ImageAlign="Right" />
                                <asp:Label ID="lblAddUser" runat="server" ForeColor="#990033" Text="Add New User"
                                    Font-Bold="True" Font-Size="Small"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                &nbsp;
                                <asp:Label ID="lblErrMsg" runat="server" Text="" Visible="true" Font-Bold="true"
                                    ForeColor="Red"></asp:Label>
                                <asp:Label ID="lblpmsg" runat="server" Text="" Font-Bold="true" Visible="false" ForeColor="Green"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="Label3" runat="server" Text="User Name"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txt_UserName" Width="300px" MaxLength="150" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="Label4" runat="server" Text="User Designation"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txt_UserDesignation" Width="300px" MaxLength="100" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="Label5" runat="server" Text="Email Id"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txt_EmailId" MaxLength="100" Width="300px" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="Label6" runat="server" Text="Remote Id"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txt_RemoteId" Width="300px" MaxLength="100" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:Label ID="Label36" runat="server" Text="User Section"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddl_Section" runat="server">
                                    <asp:ListItem Text="miscellaneous" />
                                    <asp:ListItem Text="Marketing" />
                                    <asp:ListItem Text="Collection" />
                                    <asp:ListItem Text="Testing" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_SelectAll" Checked="false" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                                <asp:Label ID="Label34" runat="server" Text="Select All" Font-Bold="true" ForeColor="DarkViolet"></asp:Label>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_Active" runat="server" Checked="false" />
                                <asp:Label ID="Label7" runat="server" Text="Active Status"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_Entry" runat="server" Checked="false" />
                                <asp:Label ID="Label8" runat="server" Text="Report Entry"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_User" runat="server" Checked="false" />
                                <asp:Label ID="Label17" runat="server" Text="New User Add"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_Check" runat="server" Checked="false" />
                                <asp:Label ID="Label9" runat="server" Text="Report Check"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_Clientapprv" runat="server" Checked="false" />
                                <asp:Label ID="Label31" runat="server" Text="New KYC Approval"></asp:Label>
                            </td>
                            <td>
                                 <asp:CheckBox ID="Chk_Print" runat="server" Checked="false" />
                                <asp:Label ID="Label10" runat="server" Text="Report Print"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_Enquiry" runat="server" Checked="false" />
                                <asp:Label ID="Label16" runat="server" Text="Enquiry Approval"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_Rptapprv" runat="server" Checked="false" />
                                <asp:Label ID="Label19" runat="server" Text="Report Approval"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_AllEnqApprv" runat="server" Checked="false" />
                                <asp:Label ID="Label33" runat="server" Text="CR Limit Approval"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_Recheck" runat="server" Checked="false" />
                                <asp:Label ID="Label35" runat="server" Text="Report Re Check"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_Proposal" runat="server" Checked="false" />
                                <asp:Label ID="Label29" runat="server" Text="Make Proposal"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_View" runat="server" Checked="false" />
                                <asp:Label ID="Label11" runat="server" Text="Report View"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_MatCollec" runat="server" Checked="false" />
                                <asp:Label ID="Label21" runat="server" Text="Material Collection"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_Account" runat="server" Checked="false" />
                                <asp:Label ID="Label23" runat="server" Text="Account"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_MatRecvd" runat="server" Checked="false" />
                                <asp:Label ID="Label27" runat="server" Text="New Enquiry"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_RecptApprv" runat="server" Checked="false" />
                                <asp:Label ID="Label22" runat="server" Text="Receipt. Approval"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_Inward" runat="server" Checked="false" />
                                <asp:Label ID="Label13" runat="server" Text="Material Inward"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_CRLmtApprv" runat="server" Checked="false" />
                                <asp:Label ID="Label24" runat="server" Text="Client CRLimit Approval"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_InwdApprv" runat="server" Checked="false" />
                                <asp:Label ID="Label20" runat="server" Text="Inward Approval"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_RecptLock" runat="server" Checked="false" />
                                <asp:Label ID="Label28" runat="server" Text="Receipt Lock"></asp:Label>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_Bill" runat="server" Checked="false" />
                                <asp:Label ID="Label14" runat="server" Text="Generate Bill"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_Mkt" runat="server" Checked="false" />
                                <asp:Label ID="Label25" runat="server" Text="Marketing"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_BillPrint" runat="server" Checked="false" />
                                <asp:Label ID="Label32" runat="server" Text="Bill Print"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_CS" runat="server" Checked="false" />
                                <asp:Label ID="Label30" runat="server" Text="Allocate RateList"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_MatTest" runat="server" Checked="false" />
                                <asp:Label ID="Label26" runat="server" Text="Material Testing(App)"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_Approve" runat="server" Checked="false" Visible="true" />
                                <asp:Label ID="Label12" runat="server" Text="Bill Approval" Visible="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_DiscountModify" runat="server" Checked="false" />
                                <asp:Label ID="Label37" runat="server" Text="Discount Modify"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_Admin" runat="server" Checked="false" />
                                <asp:Label ID="Label15" runat="server" Text="Admin"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="Chk_SuperAdmin" runat="server" Checked="false" />
                                <asp:Label ID="Label18" runat="server" Text="Super Admin"></asp:Label>
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <%--<tr>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="2">
                                &nbsp;
                                
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                &nbsp;<asp:Label ID="lblUserId" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td align="right" colspan="2">
                                <asp:LinkButton ID="lnkSaveUser" runat="server" CssClass="LnkOver" OnClick="lnkSaveUser_Click"
                                    OnClientClick="return validateUser();" Style="text-decoration: underline; font-weight: bold;">Save</asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="lnkCancelUser" runat="server" CssClass="LnkOver" OnClick="lnkCancelUser_Click"
                                    Style="text-decoration: underline; font-weight: bold;">Cancel</asp:LinkButton>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">

        function validateUser() {

            var lblmsg = document.getElementById('<%=lblErrMsg.ClientID %>');

            if (document.getElementById("<%=txt_UserName.ClientID%>").value == "") {

                lblmsg.innerHTML = "Please Enter User Name";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_UserName.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=txt_UserDesignation.ClientID%>").value == "") {

                lblmsg.innerHTML = "Please Enter User Designation";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_UserDesignation.ClientID%>").focus();
                return false;
            }

            else if (document.getElementById("<%=txt_EmailId.ClientID%>").value == "") {

                lblmsg.innerHTML = "Please Enter Email Id";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_EmailId.ClientID%>").focus();
                return false;
            }

            else if (document.getElementById("<%=txt_RemoteId.ClientID%>").value == "") {

                lblmsg.innerHTML = "Please Enter Remote Id";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_RemoteId.ClientID%>").focus();
                return false;
            }
            else {
                lblmsg.style.visibility = "hidden";
            }

        }

        function SelectAllCheckboxes(Chkbx) {

            var IsChecked = Chkbx.checked;
            var bAllChecked = true;
            var falseAllChecked = false;

            var chk_Active = document.getElementById("<%=chk_Active.ClientID%>");
            var chk_Rptapprv = document.getElementById("<%=chk_Rptapprv.ClientID%>");
            var Chk_Entry = document.getElementById("<%=Chk_Entry.ClientID%>");
            var Chk_InwdApprv = document.getElementById("<%=Chk_InwdApprv.ClientID%>");
            var Chk_Mkt = document.getElementById("<%=Chk_Mkt.ClientID%>");
            var Chk_Bill = document.getElementById("<%=Chk_Bill.ClientID%>");
            var Chk_MatTest = document.getElementById("<%=Chk_MatTest.ClientID%>");
            var Chk_Admin = document.getElementById("<%=Chk_Admin.ClientID%>");
            var Chk_MatRecvd = document.getElementById("<%=Chk_MatRecvd.ClientID%>");
            var Chk_Enquiry = document.getElementById("<%=Chk_Enquiry.ClientID%>");
            var Chk_RecptLock = document.getElementById("<%=Chk_RecptLock.ClientID%>");
            var Chk_User = document.getElementById("<%=Chk_User.ClientID%>");
            var Chk_Proposal = document.getElementById("<%=Chk_Proposal.ClientID%>");
            var Chk_SuperAdmin = document.getElementById("<%=Chk_SuperAdmin.ClientID%>");
            var chk_CS = document.getElementById("<%=chk_CS.ClientID%>");
            var Chk_Clientapprv = document.getElementById("<%=Chk_Clientapprv.ClientID%>");
            var Chk_BillPrint = document.getElementById("<%=Chk_BillPrint.ClientID%>");
            var Chk_AllEnqApprv = document.getElementById("<%=Chk_AllEnqApprv.ClientID%>");
            var Chk_Check = document.getElementById("<%=Chk_Check.ClientID%>");
            var Chk_MatCollec = document.getElementById("<%=Chk_MatCollec.ClientID%>");
            var Chk_Print = document.getElementById("<%=Chk_Print.ClientID%>");
            var Chk_RecptApprv = document.getElementById("<%=Chk_RecptApprv.ClientID%>");
            var Chk_View = document.getElementById("<%=Chk_View.ClientID%>");
            var Chk_Account = document.getElementById("<%=Chk_Account.ClientID%>");
            var Chk_Approve = document.getElementById("<%=Chk_Approve.ClientID%>");
            var Chk_CRLmtApprv = document.getElementById("<%=Chk_CRLmtApprv.ClientID%>");
            var Chk_Inward = document.getElementById("<%=Chk_Inward.ClientID%>");
            var Chk_Recheck = document.getElementById("<%=Chk_Recheck.ClientID%>");

            if (IsChecked == bAllChecked) {

                chk_Active.checked = bAllChecked;
                chk_Rptapprv.checked = bAllChecked;
                Chk_Entry.checked = bAllChecked;
                Chk_InwdApprv.checked = bAllChecked;
                Chk_Mkt.checked = bAllChecked;
                Chk_Bill.checked = bAllChecked;
                Chk_MatTest.checked = bAllChecked;
                Chk_Admin.checked = bAllChecked;
                Chk_MatRecvd.checked = bAllChecked;
                Chk_Enquiry.checked = bAllChecked;
                Chk_RecptLock.checked = bAllChecked;
                Chk_User.checked = bAllChecked;
                Chk_Proposal.checked = bAllChecked;
                Chk_SuperAdmin.checked = bAllChecked;
                chk_CS.checked = bAllChecked;
                Chk_Clientapprv.checked = bAllChecked;
                Chk_BillPrint.checked = bAllChecked;
                Chk_AllEnqApprv.checked = bAllChecked;
                Chk_Check.checked = bAllChecked;
                Chk_MatCollec.checked = bAllChecked;
                Chk_Print.checked = bAllChecked;
                Chk_RecptApprv.checked = bAllChecked;
                Chk_View.checked = bAllChecked;
                Chk_Account.checked = bAllChecked;
                Chk_Approve.checked = bAllChecked;
                Chk_CRLmtApprv.checked = bAllChecked;
                Chk_Inward.checked = bAllChecked;
                Chk_Recheck.checked = bAllChecked;
            }
            else {

                chk_Active.checked = falseAllChecked;
                chk_Rptapprv.checked = falseAllChecked;
                Chk_Entry.checked = falseAllChecked;
                Chk_InwdApprv.checked = falseAllChecked;
                Chk_Mkt.checked = falseAllChecked;
                Chk_Bill.checked = falseAllChecked;
                Chk_MatTest.checked = falseAllChecked;
                Chk_Admin.checked = falseAllChecked;
                Chk_MatRecvd.checked = falseAllChecked;
                Chk_Enquiry.checked = falseAllChecked;
                Chk_RecptLock.checked = falseAllChecked;
                Chk_User.checked = falseAllChecked;
                Chk_Proposal.checked = falseAllChecked;
                Chk_SuperAdmin.checked = falseAllChecked;
                chk_CS.checked = falseAllChecked;
                Chk_Clientapprv.checked = falseAllChecked;
                Chk_BillPrint.checked = falseAllChecked;
                Chk_AllEnqApprv.checked = falseAllChecked;
                Chk_Check.checked = falseAllChecked;
                Chk_MatCollec.checked = falseAllChecked;
                Chk_Print.checked = falseAllChecked;
                Chk_RecptApprv.checked = falseAllChecked;
                Chk_View.checked = falseAllChecked;
                Chk_Account.checked = falseAllChecked;
                Chk_Approve.checked = falseAllChecked;
                Chk_CRLmtApprv.checked = falseAllChecked;
                Chk_Inward.checked = falseAllChecked;
                Chk_Recheck.checked = falseAllChecked;
            }
        }
       
       
    </script>
</asp:Content>
