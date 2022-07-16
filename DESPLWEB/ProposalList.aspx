<%@ Page Title="Proposal List" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="ProposalList.aspx.cs" Inherits="DESPLWEB.ProposalList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:RadioButton ID="rbProposalPending" runat="server" GroupName="R1" Text="Proposal Pending"
                            AutoPostBack="true" Checked="true" OnCheckedChanged="rbProposalPending_CheckedChanged" />&nbsp;&nbsp;
                        <asp:RadioButton ID="rbProposalPendingForApproval" runat="server" GroupName="R1" Text="Proposal Pending For Approval"
                            AutoPostBack="true" OnCheckedChanged="rbProposalPendingForApproval_CheckedChanged" />&nbsp;&nbsp;
                        <asp:RadioButton ID="rbProposalComplete" runat="server" GroupName="R1" Text="Proposal Complete"
                            AutoPostBack="true" OnCheckedChanged="rbProposalComplete_CheckedChanged" />&nbsp;&nbsp;
                       <asp:RadioButton ID="rbProposalApp" runat="server" GroupName="R1" Text="Proposal From App"
                            AutoPostBack="true" OnCheckedChanged="rdProposalApp_CheckedChanged" />&nbsp;&nbsp;
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblInwdType" runat="server" Text="Inward Type" Visible="false"></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddl_InwardTestType" Width="200px" runat="server" Visible="false">
                        </asp:DropDownList>
                        &nbsp; &nbsp;  &nbsp; &nbsp;  &nbsp; &nbsp;  &nbsp; &nbsp;  &nbsp; &nbsp;  &nbsp; &nbsp;
                        <asp:CheckBox ID="chkClientSpecific" runat="server" Text="Client Specific" Visible="false" />
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" Visible="false" AutoPostBack="true"
                            OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                </tr>
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
                      <%--  <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />--%>
                         <asp:LinkButton ID="lnkDisplay" runat="server" Font-Underline="True" ForeColor="Blue"
                         OnClick="lnkDisplay_Click">Display</asp:LinkButton>
                        &nbsp; &nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Underline="True" OnClick="lnkPrint_Click" Visible="false" >Print</asp:LinkButton>
                        &nbsp; &nbsp;
                   <%--    <asp:LinkButton ID="lnkShowAllProposal" runat="server" Font-Underline="True" ForeColor="Blue"
                         Visible="false"   OnClick="lnkShowAllProposal_Click">Show all proposal with revised</asp:LinkButton>
                --%>
                        <asp:CheckBox ID="chkShowAll" Visible="false"  Text="Show all proposal with revised"  runat="server" />
                 </td>
                    <td>
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total No of Records : 0 "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 23px" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="370px"
                            Width="940px" BorderColor="AliceBlue">
                            <div style="width: 940px;">
                                <div id="GHead">
                                </div>
                                <div style="height: 370px; overflow: auto">
                                    <asp:GridView ID="grdProposal" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        DataKeyNames="ENQ_Id" ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE"
                                        BackColor="#F7F6F3" CssClass="Grid" BorderWidth="1px" Width="100%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkProposal" runat="server" CommandArgument='<%#Eval("ENQ_Id") + ";"  + Eval("ENQNewClientStatus")+ ";" + Eval("Proposal_Id")%>'
                                                        OnCommand="lnkProposal_OnCommand" Font-Underline="true" Text="Modify">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="40px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkProposalRev" runat="server" CommandArgument='<%#Eval("ENQ_Id") + ";" + Eval("ENQNewClientStatus")+ ";" + Eval("Proposal_Id")%>'
                                                        OnCommand="lnkProposalRev_OnCommand" Font-Underline="true" Text="Revise">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="40px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkPrint" runat="server" CommandArgument='<%#Eval("ENQ_Id") + ";" + Eval("ENQNewClientStatus")+ ";" + Eval("Proposal_Id")%>'
                                                        OnCommand="lnkPrint_OnCommand" Font-Underline="true" Text="Print">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="30px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkProposalEmail" runat="server" CommandArgument='<%#Eval("ENQ_Id") + ";" + Eval("ENQNewClientStatus")+ ";" + Eval("Proposal_Id")%>'
                                                        OnCommand="lnkProposalEmail_OnCommand" Font-Underline="true" Text="Email">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="40px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkOrderRec" runat="server" CommandArgument='<%#Eval("Proposal_Id")%>'
                                                        OnCommand="lnkOrderRec_OnCommand" Font-Underline="true" Text="Order Recieved">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="40px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkGenerateProforma" runat="server" CommandArgument='<%#Eval("ENQ_Id") + ";" + Eval("ENQNewClientStatus") + ";"  + Eval("Proposal_No") + ";"  + Eval("PROINV_Id")%>'
                                                        OnCommand="lnkGenerateProforma_OnCommand" Font-Underline="true" Text="Generate Proforma Invoice">
                                                    </asp:LinkButton>
                                                    <asp:Label ID="lblProformaInvoiceNo" runat="server" Text='<%#Eval("PROINV_Id") %>' Visible="false"/>
                                                </ItemTemplate>
                                                <ItemStyle Width="40px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Proposal Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProposalId" runat="server" Text='<%#Eval("Proposal_Id") %>' /><%--'<%#Eval("Proposal_No") %>'--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Proposal No." Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProposalNo" runat="server" Text='<%#Eval("Proposal_No") %>' /><%--'<%#Eval("Proposal_No") %>'--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                                <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Enq No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnquiryId" runat="server" Text='<%#Eval("ENQ_Id") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Enquiry Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnquiryDate" runat="server" Text='<%#Eval("ENQ_Date_dt" ,"{0:dd/MM/yy}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Proposal Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProposalDate" runat="server" Text='<%#Eval("Proposal_Date" ,"{0:dd/MM/yy}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Proposal Net Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProposalAmt" runat="server" Text='<%#Eval("Proposal_NetAmount" ,"{0:dd/MM/yy}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="80px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Material Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaterialName" runat="server" Text='<%#Eval("MATERIAL_Name_var") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Client Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("CL_Name_var") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Site Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSiteName" runat="server" Text='<%#Eval("SITE_Name_var") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Enquiry Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnquiryStatus" runat="server" Text='<%#Eval("ENQ_OpenEnquiryStatus_var") %>' />
                                                    <asp:Label ID="lblEnqNewClient" runat="server" Text='<%#Eval("ENQNewClientStatus") %>'
                                                        Visible="false" />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contact Name" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblContctName" runat="server" Text='<%#Eval("CONT_Name_var") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contact No" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblContctNo" runat="server" Text='<%#Eval("CONT_ContactNo_var") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderStatus" runat="server" Text='<%#Eval("ENQ_Status_tint") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email To">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtEmailIdTo" runat="server" Width="230px" Text='<%#Eval("SITE_EmailID_var") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email Cc">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtEmailIdCc" runat="server" Width="230px" Text='<%#Eval("Proposal_EmailToCc_var") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
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
                                </div>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr id="tr1">
                    <td colspan="2">
                        <asp:Label ID="lblGrayColor" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Gray colour indicates new client enquiry. &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                            BackColor="LightGray"></asp:Label>
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
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="ModalPopupBG"
                Drag="true" PopupControlID="PnlPerticularDtls" PopupDragHandleControlID="PopupHeader"
                TargetControlID="HiddenField1">
            </asp:ModalPopupExtender>
            <asp:Panel ID="PnlPerticularDtls" runat="server" class="DetailPopup" Height="145px"
                Width="420px">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="Label24" runat="server" Font-Bold="True" ForeColor="#990033" Text="Add/Edit Order Details"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:ImageButton ID="imgExit" runat="server" ImageAlign="Right" ImageUrl="Images/cross_icon.png"
                                        OnClick="imgExit_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    <asp:Label ID="lblProposalId" runat="server" Text="0" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 30px" width="30%">
                                    <asp:Label ID="Label19" runat="server" Text="Order No"></asp:Label>
                                </td>
                                <td width="50%">
                                    <asp:TextBox ID="txtOrderNo" runat="server" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 30px" width="30%">
                                    <asp:Label ID="Label20" runat="server" Text="Order Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOrderDate" runat="server" Width="250px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                        Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtOrderDate">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txtOrderDate" MaskType="Date"
                                        Mask="99/99/9999" AutoComplete="false" runat="server">
                                    </asp:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 30px" width="30%">
                                    <asp:Label ID="Label10" runat="server" Text="Order Value"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOrderValue" runat="server" MaxLength="10" onchange="checkDecimal(this);"
                                        Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:LinkButton ID="lnkAddOrderRec" runat="server" Font-Underline="true" OnClick="lnkAddOrderRec_Click">Save</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        function checkPopupNumber(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    x.value = '';
                    x.focus();
                    document.getElementById('<%=lblMessage.ClientID%>').style.visibility = "visible";
                    document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "Numeric Values Allowed";
                    return false;
                }
                else {
                    document.getElementById('<%=lblMessage.ClientID%>').style.visibility = "hidden";
                }

            }
            return true;
        }
        function checkDecimal(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 46))) {
                    x.value = '';
                    x.focus();
                    document.getElementById('<%=lblMessage.ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=lblMessage.ClientID %>').innerHTML = "Only Decimal Values Allowed";
                    return false;
                }
                else {
                    document.getElementById('<%=lblMessage.ClientID %>').style.visibility = "hidden";
                }

            }
            return true;
        }
      
    </script>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
    </script>
</asp:Content>
