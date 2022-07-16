<%@ Page Title="New Receipt" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" EnableEventValidation="false" CodeBehind="CashReceipt_Entry.aspx.cs"
    Theme="duro" Inherits="DESPLWEB.CashReceipt_Entry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 480px;">
        <asp:Panel ID="pnlDetails" runat="server" Width="100%" BorderColor="AliceBlue" BorderStyle="Ridge">
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="Rect. No."></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_receivedNo" Width="150px" runat="server" Text="Create New..."
                            ReadOnly="True"></asp:TextBox>
                        <asp:Label ID="lblBillNo" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblStatus" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td style="width: 15%" align="center">
                        <asp:Label ID="Label8" runat="server" Text="Date"></asp:Label>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txt_date" runat="server" Width="90px" onchange="javascript:DateValid();"
                            Height="21px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_date">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_date" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td>
                    </td>
                    <td style="text-align: left">
                        <asp:Label ID="Label9" runat="server" Width="40px" Text="Status"></asp:Label>
                    </td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="ddl_status" runat="server" Width="145px">
                            <asp:ListItem Text="Ok" Value="false" />
                            <asp:ListItem Text="Hold" Value="true" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Client" runat="server" Text="Client"></asp:Label>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="txt_Client" runat="server" Width="305px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Visible="false"></asp:Label>
                        <%--<asp:DropDownList ID="ddl_Client" runat="server" AutoPostBack="True" Width="305px"
                            OnSelectedIndexChanged="ddl_Client_SelectedIndexChanged">
                        </asp:DropDownList>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label11" runat="server" Text="Payment By"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButton ID="rdn_cheque" runat="server" AutoPostBack="true" GroupName="c"
                            OnCheckedChanged="rdn_cheque_CheckedChanged" Text="" />
                        <asp:Label ID="Label3" runat="server" Text="Cheque"></asp:Label>
                        &nbsp;
                        <asp:RadioButton ID="rdn_cash" runat="server" AutoPostBack="true" GroupName="c" OnCheckedChanged="rdn_cash_CheckedChanged"
                            Text="" />
                        <asp:Label ID="Label4" runat="server" Text="Cash"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_chequno" runat="server" Style="text-align: right" Text="Cheque No"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_cheque_no" MaxLength="6" runat="server" Width="200px" onchange="checkNum(this)"
                            autocomplete="off"></asp:TextBox>
                    </td>
                    <td align="center">
                        <asp:Label ID="Label12" runat="server" Text="Date"></asp:Label>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txt_chquedate" Width="90px" runat="server" onchange="javascript:DateValidcheque();"
                            autocomplete="off"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_chquedate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_chquedate"
                            MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label13" runat="server" Text="Branch"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Branch" Width="140px" runat="server" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_bankname" runat="server" Text="Bank Name"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Bank_Name" runat="server" Width="305px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <%--<asp:LinkButton ID="lnkAddNew" runat="server" OnCommand="lnkAddNew_Click" CausesValidation="false"
                            Font-Bold="true" Font-Underline="true" EnableViewState="false" Text="New Bank"
                            Width="150px" ToolTip="Add Bank Name"></asp:LinkButton>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_amtrecved" runat="server" Text="Amt Received"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_AmtReceived" Width="200px" runat="server" AutoPostBack="true"
                            OnTextChanged="txt_AmtReceived_TextChanged" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_TDS" runat="server" Text="TDS"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_TDS" runat="server" Width="200px" AutoPostBack="true" OnTextChanged="txt_TDS_TextChanged"
                            autocomplete="off"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_TotalAmt" runat="server" Text="Total"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_totalAmt" runat="server" Width="200px" ReadOnly="true" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label14" runat="server" Text="TDS Amount to be adjusted" Width="200px"></asp:Label>
                    </td>
                    <td width="5%">
                        <asp:TextBox ID="txt_tdsallocatedamt" runat="server" Width="200px" CssClass="caltextbox"
                            ReadOnly="True" autocomplete="off"></asp:TextBox>
                    </td>
                    <td width="5%" style="text-align: center">
                        <asp:Label ID="Label15" runat="server" Text="Amount to be adjusted"></asp:Label>
                    </td>
                    <td align="left" colspan="3">
                        <asp:TextBox ID="txt_acallocatedamt" runat="server" Width="200px" ReadOnly="True"
                            CssClass="caltextbox" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="PnlGrd" runat="server" Height="165px" Width="100%" BorderStyle="Ridge"
            BorderColor="AliceBlue">
            <div style="width: 100%;">
                <div id="GHead" style="width: 940px">
                </div>
                <div style="height: 165px; overflow: auto">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvDetailsView" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin"
                                ShowFooter="True" OnRowDataBound="gvDetailsView_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Bill No" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBillNo" BorderWidth="0px" Width="150px" Style="text-align: center"
                                                runat="server" Text='<%#Eval("BILL_Id") %>' ReadOnly="true" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblAcF" BorderWidth="0px" Width="100px" runat="server" Text="On A/c:"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Bill Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBillDate" runat="server" Width="80px" Text='<%#Eval("BILL_Date_dt","{0:dd/MM/yyyy}") %>'
                                                BorderWidth="0px" ReadOnly="true" Style="text-align: center" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtBillDateFt" BorderWidth="0px" Width="80px" runat="server" Text="-"
                                                Style="text-align: center" ReadOnly="true" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="BillAmount" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBillAmt" runat="server" Width="150px" Text='<%#Eval("BILL_NetAmt_num","{0:f2}") %>'
                                                BorderWidth="0px" Style="text-align: right" ReadOnly="true" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtBillAmtFt" BorderWidth="0px" Width="150px" runat="server" Text="-"
                                                Style="text-align: center" ReadOnly="true" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Balance Amount"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBalAmt" Style="text-align: right" runat="server" BorderWidth="0px"
                                                Width="150px" ReadOnly="true" Text='<%#Eval("BalanceAmount","{0:f2}") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtBalAmtFt" runat="server" BorderWidth="0px" Width="150px" Text="-"
                                                Style="text-align: center" ReadOnly="true" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Settlement" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlSettlement" runat="server" Width="120px" BorderWidth="0px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSettlement_SelectedIndexChanged"
                                                Text='<%# Eval("CashDetail_Settlement_var").ToString() == "" ? "EMPTY" : Eval("CashDetail_Settlement_var").ToString() %>'>
                                                <asp:ListItem Value="0">---Select---</asp:ListItem>
                                                <asp:ListItem>Full</asp:ListItem>
                                                <asp:ListItem>Part</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtSettlementFt" runat="server" Text="-" Style="text-align: center"
                                                Width="120px" BorderWidth="0px" ReadOnly="true"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="TDS Amount" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTdsAmt" runat="server" Style="text-align: right" MaxLength="15"
                                                autocomplete="off" BorderWidth="0px" AutoPostBack="true" OnTextChanged="txtTdsAmt_TextChanged"
                                                EnableViewState="true" Width="100px" Text='<%#Eval("CashDetail_TDSAmount_money","{0:f2}") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtTdsAmtFt" BorderWidth="0px" Width="100px" runat="server" Style="text-align: right"
                                                autocomplete="off" MaxLength="15" AutoPostBack="true" EnableViewState="true"
                                                OnTextChanged="txtTdsAmtFt_TextChanged" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Adjust Amount"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAdjustAmt" runat="server" Style="text-align: right" Width="150px"
                                                autocomplete="off" MaxLength="15" BorderWidth="0px" AutoPostBack="true" EnableViewState="true"
                                                OnTextChanged="txtAdjustAmt_TextChanged" Text='<%#Eval("CashDetail_Amount_money","{0:d}") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtAdjustAmtFt" BorderWidth="0px" Width="150px" runat="server" Style="text-align: right"
                                                autocomplete="off" MaxLength="15" AutoPostBack="true" EnableViewState="true"
                                                OnTextChanged="txtAdjustAmtFt_TextChanged" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlOther" runat="server" Width="100%" BorderColor="AliceBlue" BorderStyle="Ridge">
            <table width="100%">
                <tr>
                    <td style="text-align: left">
                        <asp:Label ID="Label1" runat="server" Text="Note"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Note" MaxLength="250" runat="server" Width="410px" autocomplete="off"></asp:TextBox>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="Label17" runat="server" Text="Total "></asp:Label>
                    </td>
                    <td align="right">
                        <asp:TextBox ID="txtTotalTDSAmtGV" runat="server" Width="140px" ReadOnly="True" CssClass="caltextbox"></asp:TextBox>
                        <asp:TextBox ID="txtTotalAmtGV" runat="server" Width="150px" ReadOnly="True" CssClass="caltextbox"
                            OnTextChanged="txtTotalAmtGV_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkColleted" Checked="true" runat="server" AutoPostBack="true"
                            OnCheckedChanged="chkColleted_CheckedChanged" />
                        <asp:Label ID="lblColectfrm" runat="server" Text="Collected From"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlUserCollected" Width="206px" runat="server">
                        </asp:DropDownList>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txt_userip" runat="server" Width="190px" autocomplete="off"></asp:TextBox>
                    </td>
                    <td style="text-align: right">
                        <asp:LinkButton ID="LnkBtnSave" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="LnkBtnSave_Click" CssClass="SimpleColor">Save</asp:LinkButton>&nbsp;
                        &nbsp;
                        <asp:LinkButton ID="lnkNext" runat="server" Visible="false" Font-Bold="true" Font-Underline="true"
                            OnClick="lnkNext_Click" CssClass="SimpleColor">Next</asp:LinkButton>&nbsp; &nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Bold="true" Visible="false" Style="text-decoration: underline;"
                            OnClick="lnkPrint_Click">Print </asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;
                        <%--<asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClientClick="javascript:history.go(-1);return false;" runat="server" 
                            onclick="LnkExit_Click">Exit</asp:LinkButton>--%>
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            runat="server" OnClick="LnkExit_Click">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%--<asp:Panel ID="PnlPopup" runat="server" Visible="false">
            <table>
                <tr>
                    <td>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="HiddenField1"
                PopupControlID="pnlCsh" PopupDragHandleControlID="PopupHeader" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlCsh" runat="server" BorderWidth="0px" Width="450px" Height="100px"
                Visible="false" CssClass="rounded_BordersColor">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table class="DetailPopup" style="width: 100%; height: 100px">
                            <tr valign="top">
                                <td align="center" valign="bottom" colspan="2">
                                    <asp:Label ID="lblAddContact" runat="server" Font-Bold="True" ForeColor="#990033"
                                        Text="Add Bank Name" Font-Size="Small"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" runat="server" Text="Bank Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_EnterBankName" runat="server" Width="300px" autocomplete="off"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblpMsg" runat="server" ForeColor="Red" Font-Bold="true" Text="lblMsg"
                                        Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right" valign="top">
                                    &nbsp;
                                    <asp:LinkButton ID="lnkSaveBankNmae" runat="server" OnClientClick="return validate1();"
                                        Font-Bold="true" Font-Underline="true" OnClick="lnkSaveBankNmae_Click" CssClass="SimpleColor">Save</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkCancelCash" runat="server" OnClick="lnkCancelCash_Click"
                                        Font-Bold="true" Font-Underline="true" CssClass="SimpleColor">Exit</asp:LinkButton>&nbsp;
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                    <asp:PostBackTrigger ControlID="lnkCancelCash" />
                   </Triggers>
                </asp:UpdatePanel>
                
            </asp:Panel>
        </asp:Panel>--%>
    </div>
    <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=gvDetailsView.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=gvDetailsView.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=gvDetailsView.ClientID%>').offset().top);

        });
    </script>
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

        function validate2() {

            document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.color = "Red";
            var lblmsg = document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>');
            //            if (document.getElementById("<%=txt_Client.ClientID%>").value == "") {

            //                lblmsg.innerHTML = "Please Select the Client Name";
            //                lblmsg.style.visibility = "visible";
            //                document.getElementById("<%=txt_Client.ClientID%>").focus();
            //                return false;
            //            }
            //            else {
            //                lblmsg.style.visibility = "hidden";
            //            }

            if (document.getElementById("<%=txt_Client.ClientID%>").value == "" || document.getElementById("<%=txt_Client.ClientID%>").value == "0") {

                lblmsg.innerHTML = " Please Select the Client Name ";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_Client.ClientID%>").focus();
                return false;
            }

            else if (document.getElementById("<%=txt_date.ClientID%>").value == "") {

                lblmsg.innerHTML = " Please Enter Date ";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_date.ClientID%>").focus();
                return false;
            }


            else if (document.getElementById("<%=rdn_cheque.ClientID%>").checked == false &&
                 document.getElementById("<%=rdn_cash.ClientID%>").checked == false) {

                lblmsg.innerHTML = " Please select anyone of these radio button";
                lblmsg.style.visibility = "visible";
                return false;
            }


            else if (document.getElementById("<%=rdn_cheque.ClientID%>").checked == true) {
                if (document.getElementById("<%=txt_cheque_no.ClientID%>").value == "") {

                    lblmsg.innerHTML = " Please Enter Check No";
                    lblmsg.style.visibility = "visible";
                    document.getElementById("<%=txt_cheque_no.ClientID%>").focus();
                    return false;
                }
                else if (document.getElementById("<%=txt_Branch.ClientID%>").value == "") {

                    lblmsg.innerHTML = "Please Enter Branch Name ";
                    lblmsg.style.visibility = "visible";
                    document.getElementById("<%=txt_Branch.ClientID%>").focus();
                    return false;
                }
                else if (document.getElementById("<%=ddl_Bank_Name.ClientID%>").value == "---Select---") {

                    lblmsg.innerHTML = "Please Select Bank Name";
                    lblmsg.style.visibility = "visible";
                    document.getElementById("<%=ddl_Bank_Name.ClientID%>").focus();
                    return false;
                }
                else if (document.getElementById("<%=txt_AmtReceived.ClientID%>").value == "") {

                    lblmsg.innerHTML = "Please Enter Receipt Amount";
                    lblmsg.style.visibility = "visible";
                    document.getElementById("<%=txt_AmtReceived.ClientID%>").focus();
                    return false;

                }
                else if (document.getElementById("<%=txt_TDS.ClientID%>").value == "") {

                    lblmsg.innerHTML = "Please Enter TDS Amount";
                    lblmsg.style.visibility = "visible";
                    document.getElementById("<%=txt_TDS.ClientID%>").focus();
                    return false;
                }
            }
            else if (document.getElementById("<%=rdn_cash.ClientID%>").checked == true) {
                if (document.getElementById("<%=txt_AmtReceived.ClientID%>").value == "") {

                    lblmsg.innerHTML = "Please Enter Receipt Amount";
                    lblmsg.style.visibility = "visible";
                    document.getElementById("<%=txt_AmtReceived.ClientID%>").focus();
                    return false;

                }
                else if (document.getElementById("<%=txt_TDS.ClientID%>").value == "") {

                    lblmsg.innerHTML = "Please Enter TDS Amount";
                    lblmsg.style.visibility = "visible";
                    document.getElementById("<%=txt_TDS.ClientID%>").focus();
                    return false;
                }
            }

            if (document.getElementById("<%=txtTotalAmtGV.ClientID%>").value == "0.00") {

                lblmsg.innerHTML = "Please Enter some Adjust Amount";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_AmtReceived.ClientID%>").focus();
                return false;
            }

            var GVMaintainReceiptMaster = document.getElementById("<%= gvDetailsView.ClientID %>");
            for (var rowId = 0; rowId < GVMaintainReceiptMaster.rows.length; rowId++) {
                var balanceamt = GVMaintainReceiptMaster.rows[rowId].cells[3].children[0];
                var ddlsettlemt = GVMaintainReceiptMaster.rows[rowId].cells[4].children[0];
                var txtTds = GVMaintainReceiptMaster.rows[rowId].cells[5].children[0];
                var txtAdjst = GVMaintainReceiptMaster.rows[rowId].cells[6].children[0];
                if (txtTds.value != "0.00" || txtAdjst.value != "0.00") {
                    if (ddlsettlemt.value == "0") {

                        lblmsg.innerHTML = "Please select the Settlement";
                        lblmsg.style.visibility = "visible";
                        return false;
                    }

                }
                else if (txtTds.value == "") {

                    lblmsg.innerHTML = "Please Enter TDS Amount";
                    lblmsg.style.visibility = "visible";
                    return false;
                }
                else if (txtAdjst.value == "") {

                    lblmsg.innerHTML = "Please Enter Adjust Amount";
                    lblmsg.style.visibility = "visible";
                    return false;
                }
            }
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

        function Select(txt_AmtReceived) {
            var control = document.getElementById(txt_AmtReceived);
            control.select();
        }

        function DateValidcheque() {
            var myDate1 = document.getElementById("<%=txt_chquedate.ClientID%>").value;
            var date = myDate1.substring(0, 2);
            var month = myDate1.substring(3, 5);
            var year = myDate1.substring(6, 10);

            var myDate = new Date(year, month - 1, date);

            var today = new Date();

            var recptDate1 = document.getElementById("<%=txt_date.ClientID%>").value;
            var rcptDay = recptDate1.substring(0, 2);
            var rcptMonth = recptDate1.substring(3, 5);
            var rcptYear = recptDate1.substring(6, 10);
            var recptDate = new Date(rcptYear, rcptMonth - 1, rcptDay);

            if (myDate > today) {

                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Date should not be greater than current date";
                return false;
                myDate1.focus();

            }
            else if (myDate > recptDate) {

                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Date should not be greater than receipt date";
                return false;
                recptDate1.focus();

            }
            else {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                return true;
            }
        }
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
      
    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
