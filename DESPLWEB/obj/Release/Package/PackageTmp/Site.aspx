<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Site.aspx.cs" Inherits="DESPLWEB.Site" Title="Site Updation" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function clickButton(e, buttonid) {
            var evt = e ? e : window.event;
            var bt = document.getElementById(buttonid);
            if (bt) {
                if (evt.keyCode == 13) {
                    bt.click();
                    return false;
                }
            }
        }
        function catchEsc(e) {
            var kC = (window.event) ?    // MSIE or Firefox?
                 event.keyCode : e.keyCode;
            var Esc = (window.event) ?
                27 : e.DOM_VK_ESCAPE // MSIE : Firefox
            if (kC == Esc) {
                var mpu = $find('ModalPopupExtender1');
                mpu.hide();
            }
        }           
    </script>
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="480px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="lblClient" runat="server" Font-Bold="True" Text="Select Client"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Client" runat="server" Width="343px" Height="16px" AutoPostBack="true"
                            OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <%--    <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/Search-32.png"
                                        Style="width: 14px" OnClick="ImgBtnSearch_Click" />--%>
                        &nbsp; &nbsp;&nbsp;
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Text="Select Client" Visible="False"
                            ForeColor="Red"></asp:Label>
                        &nbsp;
                        <asp:LinkButton ID="lnkAddNewSite" runat="server" OnClick="imgInsertSite_Click" Font-Underline="true"
                            CssClass="SimpleColor" ToolTip="Add New Site">Add New Site</asp:LinkButton>
                        &nbsp;
                        <asp:CheckBox ID="chkApprPendingSite" runat="server" AutoPostBack="true" Text="Approval Pending Sites"
                            OnCheckedChanged="chkApprPendingSite_CheckedChanged" /><%-- --%>
                            <asp:Label ID="lblClientApprRight" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="pnlSiteList" runat="server" Width="100%" BorderWidth="0px" Height="420px"
                            ScrollBars="Auto">
                            <asp:GridView ID="grdSite" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#DEDFDE" BorderWidth="1px" CellPadding="4" DataKeyNames="SITE_Id"
                                ForeColor="Black" GridLines="Vertical" Width="100%" OnRowDataBound="grdSite_RowDataBound">
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <%--  <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgInsertSite" runat="server" OnCommand="imgInsertSite_Click"
                                                            ImageUrl="Images/AddNewitem.jpg" Height="20px" Width="20px" CausesValidation="false"
                                                            ToolTip="Add New Site" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgEditSite" runat="server" CommandArgument='<%#Eval("SITE_Id")%>'
                                                OnCommand="imgEditSite_Click" ImageUrl="Images/Edit.jpg" Height="18px" Width="18px"
                                                CausesValidation="false" ToolTip="Edit Site" />
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnSites" runat="server" CommandArgument='<%#Eval("SITE_Id")%>'
                                                OnCommand="lnkLoadKYCDetails" Text="Update KYC" Font-Underline="true"> </asp:LinkButton>
                                            <%--    <asp:LinkButton ID="btnContactPersons" runat="server" CommandArgument='<%#Eval("SITE_Id")%>'
                                                            OnCommand="lnkLoadContactPersons" Text="Contacts">  </asp:LinkButton>--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteId" runat="server" Text='<%#Eval("SITE_Id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" runat="server" Text='<%#Eval("SITE_Name_var") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Address">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteAddress" runat="server" Text='<%#Eval("SITE_Address_var") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Email Id">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteEmailID" runat="server" Text='<%#Eval("SITE_EmailID_var") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteStatus" runat="server" Text='<%#Convert.ToBoolean(Eval("SITE_Status_bit")) == true ? "Deactive" : "Active" %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Billing Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteMonthlyStatus" runat="server" Text='<%#Convert.ToBoolean(Eval("SITE_MonthlyBillingStatus_bit")) == true ? "Monthly" : "Regular" %>' />
                                        </ItemTemplate>
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
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="HiddenField1"
                            PopupControlID="pnlSite" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="pnlSite" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <%-- <asp:Panel ID="pnlSite" runat="server" BorderWidth="1px" Height="330px" ScrollBars="Vertical"
                            Style="background-color: #ECF5FF;" Visible="false">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">--%>
                                <ContentTemplate>
                                    <asp:Panel ID="pnlSiteSearch" runat="server" BorderColor="AliceBlue" BorderStyle="Ridge"
                                        Height="500px" ScrollBars="Auto" Visible="false" Width="750px">
                                        <table class="DetailPopup" width="750px">
                                            <tr valign="top">
                                                <td align="center" valign="top">
                                                    <asp:ImageButton ID="imgCloseSiteSearch" runat="server" ImageUrl="Images/cross_icon.png"
                                                        OnClick="imgCloseSiteSearch_Click" ImageAlign="Right" />
                                                    &nbsp;
                                                    <asp:LinkButton ID="lnkOkSiteSearch" runat="server" Font-Underline="true" OnClick="lnkOkSiteSearch_Click" Visible="false">Ok</asp:LinkButton>
                                                    &nbsp;
                                                    <br>
                                                    <asp:GridView ID="grdSiteSearch" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                                                        BorderColor="#DEBA84" BorderWidth="1px" CellPadding="0" CellSpacing="0" CssClass="Grid"
                                                        ForeColor="#333333" GridLines="Horizontal" SkinID="gridviewSkin1" Width="100%" OnRowDataBound="grdSiteSearch_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Client Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblClientId" runat="server" Text='<%#Eval("SITE_CL_Id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("CL_Name_var") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblClientStatus" runat="server" Text='<%#Eval("CL_Status_bit") %>'></asp:Label>
                                                                    <%--<asp:Label ID="lblClientStatus" runat="server" Text='<%# (string)Eval("CL_Status_bit") == "0" ? "Active":"Deactive"%>'></asp:Label>--%>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Site Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSiteId" runat="server" Text='<%#Eval("SITE_Id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblSiteName" runat="server" Text='<%#Eval("SITE_Name_var") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSiteStatus" runat="server" Text='<%#Eval("SITE_Status_bit") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlSite1" runat="server">
                                    <table class="DetailPopup" style="width: 100%">
                                        <tr valign="top">
                                            <td align="center" valign="bottom" colspan="6">
                                                <asp:ImageButton ID="imgCloseSitePopup" runat="server" ImageUrl="Images/cross_icon.png"
                                                    OnClick="imgCloseSitePopup_Click" ImageAlign="Right" />
                                                <asp:Label ID="lblAddSite" runat="server" Font-Bold="True" ForeColor="#990033" Text="Add New Site"
                                                    Font-Size="Small"></asp:Label>
                                            </td>
                                        </tr>                                        
                                        <tr valign="top">
                                            <td align="right">
                                                Client Name
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td colspan="4">
                                                <asp:Label ID="lblSiteClient" runat="server" Font-Bold="True" ForeColor="#CC0099"
                                                    Text="client name" Font-Size="12px"></asp:Label><br />
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                Current Site Name
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCurrentSiteName" runat="server" Font-Bold="True"
                                                    ForeColor="#CC0099" Text="" Font-Size="12px"></asp:Label>&nbsp;
                                                <asp:Label ID="lblCurrentSiteId" runat="server" Font-Bold="True" ForeColor="#CC0099"
                                                    Text="" Font-Size="12px"></asp:Label>
                                                <br />
                                                &nbsp;
                                            </td>
                                            <td align="right">
                                                <asp:LinkButton ID="lnkViewAllSites" runat="server" CssClass="LnkOver" Font-Bold="True"
                                                        OnClick="lnkViewAllSites_Click" Style="text-decoration: underline;" Visible="true">View All Sites</asp:LinkButton> &nbsp;&nbsp;
                                                Site Name
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSiteName" runat="server" MaxLength="250" Width="192px" autocomplete="off"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtSiteName"
                                                    EnableClientScript="False" ErrorMessage="Input 'Site Name'" SetFocusOnError="True"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top">
                                                &nbsp;
                                            </td>
                                            <td colspan="5">
                                                <asp:CheckBox ID="chkDetailSameAsClient" runat="server" Text="Details Same as Client"
                                                    AutoPostBack="true" OnCheckedChanged="chkDetailSameAsClient_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                Site Address
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSiteAddress" runat="server" MaxLength="255" Width="192px" TextMode="MultiLine"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtSiteAddress"
                                                    EnableClientScript="False" ErrorMessage="Input 'Office Address'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>
                                            </td>
                                            <td align="right">
                                                Pin Code
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPinCode" runat="server" MaxLength="6" onkeypress="return CheckNumeric(event,this);"
                                                    Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPinCode"
                                                    EnableClientScript="False" ErrorMessage="Input 'Pin Code'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                State
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlState" runat="server" Width="192px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlState"
                                                    InitialValue="0" EnableClientScript="False" ErrorMessage="Input 'State'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>
                                            </td>
                                            <td align="right">
                                                City
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCity" runat="server" MaxLength="50" Visible="false" onkeypress="return onlyAlphabets(event,this);"
                                                    Width="192px"></asp:TextBox>
                                                <asp:DropDownList ID="ddlCity" runat="server" Width="192px" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:LinkButton ID="lnkAddCity" runat="server" Font-Underline="true" OnClick="lnkAddCity_Click">New City</asp:LinkButton>
                                                &nbsp;&nbsp;<asp:LinkButton ID="lnkSaveCity" runat="server" Font-Underline="true"
                                                    Visible="false" OnClick="lnkSaveCity_Click">Save City</asp:LinkButton>
                                                <%--  <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCity"
                                                    EnableClientScript="False" ErrorMessage="Input 'City'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                Telephone No
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPhno" runat="server" MaxLength="15" onkeypress="return CheckNumeric(event,this);"
                                                    Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPhno"
                                                    EnableClientScript="False" ErrorMessage="Input 'Phone No'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>
                                            </td>
                                            <td align="right" style="width: 210px">
                                                Nature Of Site/Work Place
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNatureOfSite" runat="server" MaxLength="255" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNatureOfSite"
                                                    EnableClientScript="False" ErrorMessage="Input 'Nature Of Site'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>
                                            </td>
                                            <tr>
                                                <td align="right" valign="top">
                                                    GST
                                                </td>
                                                <td colspan="5">
                                                    <asp:RadioButton ID="rbSiteReg" AutoPostBack="true" runat="server" Text="Registered"
                                                        GroupName="R2" OnCheckedChanged="rbSiteReg_CheckedChanged" />
                                                    &nbsp;
                                                    <asp:RadioButton ID="rbSiteUnReg" AutoPostBack="true" runat="server" Text="Unregistered"
                                                        GroupName="R2" OnCheckedChanged="rbSiteUnReg_CheckedChanged" />
                                                </td>
                                            </tr>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                Site GST No
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGstNo" runat="server" MaxLength="15" Width="192px" Enabled="false"></asp:TextBox>
                                                &nbsp;
                                                <br />
                                                <asp:Label ID="valName1" runat="server" ForeColor="Red" Text="Input 'GST No'" Visible="false"></asp:Label>
                                                <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtGstNo"
                                                    EnableClientScript="False" ErrorMessage="Input 'GST no'" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                               
                                                <asp:MaskedEditExtender ID="MaskedEditExtender4" TargetControlID="txtGstNo" MaskType="None"
                                                    Mask="99 ?????????? ???" AutoComplete="false" runat="server">
                                                </asp:MaskedEditExtender> --%>
                                            </td>
                                            <td align="right">
                                                Provisional GST Date of Registration
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGstDate" runat="server" MaxLength="255" Width="192px" Enabled="false"></asp:TextBox>
                                                <br />
                                                &nbsp;
                                                <asp:Label ID="valName2" runat="server" ForeColor="Red" Text="Input 'GST Date'" Visible="false"></asp:Label>
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                    Format="dd/MM/yyyy" TargetControlID="txtGstDate">
                                                </asp:CalendarExtender>
                                                <%-- <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" AutoComplete="false"
                                                    Mask="99/99/9999" MaskType="Date" TargetControlID="txtGstDate">
                                                </asp:MaskedEditExtender>
                                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtGstDate"
                                                    EnableClientScript="False" ErrorMessage="Input 'GST date'" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                --%>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                Site Incharge
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSiteIncharge" runat="server" MaxLength="255" Width="192px"></asp:TextBox>
                                                <br />
                                                &nbsp;
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtSiteIncharge"
                                                    EnableClientScript="False" ErrorMessage="Input 'Site Incharge'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>
                                            </td>
                                            <td align="right">
                                                Site Incharge Mob. No
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSiteIncMobNo" runat="server" MaxLength="10" onkeypress="return CheckNumeric(event,this);"
                                                    Width="192px"></asp:TextBox>
                                                <br />
                                                &nbsp;
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtSiteIncMobNo"
                                                    EnableClientScript="False" ErrorMessage="Input 'Site Incharge Mobile No'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                Email Id
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtSiteEmail" runat="server" MaxLength="250" Width="192px" EnableViewState="False"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtSiteEmail"
                                                    EnableClientScript="False" ErrorMessage="Input 'Email'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSiteEmail"
                                                    EnableClientScript="False" ErrorMessage="Invalid Email" ValidationExpression="^([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+,*[\W]*)+$"
                                                    ValidationGroup="V2"></asp:RegularExpressionValidator>
                                            </td>
                                            <td align="right" valign="top">
                                                Site Active Status
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkSiteStatus" runat="server" Text=" " />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                SEZ
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="chkSEZ" runat="server" Text=" " /><br />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top">
                                                <asp:Label ID="lblRera" runat="server" Text="ReRa No."></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtReRa" runat="server" MaxLength="15" Width="192px"></asp:TextBox>
                                                <br />
                                                &nbsp;
                                                <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtReRa"
                                                    EnableClientScript="False" ErrorMessage="Input 'Site ReRa No'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td align="right" valign="top">
                                                Location
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:DropDownList ID="ddlLocation" runat="server" Width="192px" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top">
                                                <asp:Label ID="lblContactPer" runat="server" Text="Contact Person" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <asp:DropDownList ID="ddlContactNm" runat="server" Width="192px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlContactNm_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <br />
                                                &nbsp;
                                            </td>
                                              <td align="right" valign="top">
                                                   <asp:Label ID="lblRoute" runat="server" Text="Route"  Visible="True"></asp:Label>
                                           
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:DropDownList ID="ddlRoute" runat="server" Width="192px" Visible="True" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                Contact Person Name
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContactPer" runat="server" MaxLength="50" Width="192px" autocomplete="off"></asp:TextBox>
                                                <br />
                                                <%--   &nbsp;
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtSiteIncharge"
                                                    EnableClientScript="False" ErrorMessage="Input 'Site Incharge'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td align="right">
                                                Contact Number
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContactNo" runat="server" MaxLength="12" Width="192px"></asp:TextBox>
                                                <br />
                                                <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtSiteIncMobNo"
                                                    EnableClientScript="False" ErrorMessage="Input 'Site Incharge Mobile No'" SetFocusOnError="true"
                                                    ValidationGroup="V2"></asp:RequiredFieldValidator>--%>
                                                <asp:LinkButton ID="lnkSaveContact" runat="server" CssClass="LnkOver" OnClick="lnkSaveContact_Click"
                                                    Visible="false" Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V3">Save Contact</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" colspan="4">
                                                &nbsp;
                                                <asp:Label ID="lblSiteMessage" runat="server" ForeColor="#990033" Text="lblMsg" Visible="False"></asp:Label>
                                            </td>
                                            <td align="right" colspan="2">
                                                <asp:LinkButton ID="lnkSaveSite" runat="server" CssClass="LnkOver" OnClick="lnkSaveSite_Click"
                                                    Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V2">Save</asp:LinkButton>
                                                &nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkCancelSite" runat="server" CssClass="LnkOver" OnClick="lnkCancelSite_Click"
                                                    Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V2">Cancel</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lnkCancelSite" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                            PopupControlID="Panel1" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="Panel1" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table class="DetailPopup">
                                        <tr valign="top">
                                            <td align="center" valign="bottom" colspan="6">
                                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="Images/cross_icon.png"
                                                    OnClick="imgClosePopup_Click" ImageAlign="Right" />
                                                <asp:Label ID="lblAddClient" runat="server" ForeColor="#990033" Text="Add KYC Information"
                                                    Font-Bold="True" Font-Size="Small"></asp:Label>
                                                <asp:Label ID="lblSiteId" runat="server" Text="" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" align="center">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top" style="width: 20%">
                                                Site Name
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 35%">
                                                <asp:Label ID="lblSiteName" runat="server" ForeColor="#CC0099" Text=" " Visible="true"></asp:Label>&nbsp;
                                                <asp:Label ID="lblKycSiteId" runat="server" ForeColor="#CC0099" Text=" " Visible="true"></asp:Label><br />
                                            </td>
                                            <td colspan="3" valign="top">
                                                <asp:Label ID="lblClientMessage" runat="server" ForeColor="Red" Text="lblMsg" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" valign="top" style="width: 20%">
                                                Site Address
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 35%">
                                                <asp:TextBox ID="txtSiteAdd" runat="server" MaxLength="250" Width="192px" TextMode="MultiLine"
                                                    autocomplete="off"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator0" runat="server" ControlToValidate="txtSiteAdd"
                                                    EnableClientScript="False" ErrorMessage="Input 'Site Address'" SetFocusOnError="True"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                            <td valign="top" align="right" style="width: 15%">
                                                Build Up Area
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td style="width: 25%">
                                                <asp:TextBox ID="txtArea" runat="server" onkeypress="return  validateFloatKeyPress(this,event);"
                                                    MaxLength="15" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtArea"
                                                    EnableClientScript="False" ErrorMessage="Input 'Area'" SetFocusOnError="true"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td valign="top" align="right">
                                                Constraction Period
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtConsPeriod" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtConsPeriod"
                                                    EnableClientScript="False" ErrorMessage="Input 'Constraction Period'" SetFocusOnError="true"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                            <td valign="top" align="right">
                                                Complition Date
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCompDate" runat="server" Width="192px"></asp:TextBox><br />
                                                <asp:Label ID="lblCompDt" runat="server" Text="Input 'Complition Date'" Visible="False"
                                                    ForeColor="Red"></asp:Label>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                    Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtCompDate">
                                                </asp:CalendarExtender>
                                                <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtCompDate" MaskType="Date"
                                                    Mask="99/99/9999" AutoComplete="false" runat="server">
                                                </asp:MaskedEditExtender>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                RCC Consultant
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRccConslt" runat="server" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtRccConslt"
                                                    EnableClientScript="False" ErrorMessage="Input 'RCC Consultant'" SetFocusOnError="true"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                            <td valign="top" align="right">
                                                Architect
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtArchitect" runat="server" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator08" runat="server" ControlToValidate="txtArchitect"
                                                    EnableClientScript="False" ErrorMessage="Input 'Architect'" SetFocusOnError="true"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top">
                                                Construction Management
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlConstMangmt" AutoPostBack="true" runat="server" Width="196px"
                                                    OnSelectedIndexChanged="ddlConstMangmt_OnSelectedIndexChanged">
                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                    <asp:ListItem>Own</asp:ListItem>
                                                    <asp:ListItem>Out Sourced</asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                                &nbsp;
                                            </td>
                                            <td align="right" valign="top">
                                                <asp:Label ID="lblOutSource" runat="server" Text="OutSource name" Visible="false"></asp:Label>
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOutSource" runat="server" MaxLength="150" Width="192px" Visible="false"></asp:TextBox>
                                                <br />
                                                &nbsp;
                                                <asp:Label ID="lblOtSource" runat="server" Text="Input 'OutSource name'" Visible="False"
                                                    ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top">
                                                Geo technical Investigation
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlGeoInvestigatn" runat="server" Width="196px">
                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                    <asp:ListItem>To be done</asp:ListItem>
                                                    <asp:ListItem>Complete</asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                                &nbsp;
                                            </td>
                                            <td align="right" valign="top">
                                                No. of Buildings under construction
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNoOfBldg" runat="server" onkeypress="return CheckNumeric(event,this);"
                                                    MaxLength="4" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtNoOfBldg"
                                                    EnableClientScript="False" ErrorMessage="Input 'No. of Buildings under construction'"
                                                    SetFocusOnError="true" ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-weight: bold" align="right">
                                                Work Status Details
                                            </td>
                                            <td colspan="5">
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" valign="top">
                                                &nbsp
                                            </td>
                                            <td align="left" colspan="5">
                                                <asp:GridView ID="grdWorkStatus" runat="server" AutoGenerateColumns="False" Width="50%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Work">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWork" runat="server" Text="Label"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlStatus" runat="server" Width="100%">
                                                                    <asp:ListItem>Yet to start</asp:ListItem>
                                                                    <asp:ListItem>In Progress</asp:ListItem>
                                                                    <asp:ListItem>Complete</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCC99" />
                                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle Font-Bold="True" ForeColor="Black" />
                                                    <EditRowStyle BackColor="#999999" />
                                                </asp:GridView>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right">
                                                No of proposed buildings
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNoOfProposedBldg" MaxLength="4" onkeypress="return CheckNumeric(event,this);"
                                                    runat="server" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtNoOfProposedBldg"
                                                    EnableClientScript="False" ErrorMessage="Input 'No of proposed buildings'" SetFocusOnError="true"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                            <td valign="top" align="right">
                                                Proposed start date
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtProposedDate" runat="server" Width="192px"></asp:TextBox><br />
                                                <asp:Label ID="lblPrpDate" runat="server" Text="Input 'Proposed Date'" Visible="False"
                                                    ForeColor="Red"></asp:Label>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                    Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtProposedDate">
                                                </asp:CalendarExtender>
                                                <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtProposedDate"
                                                    MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                                                </asp:MaskedEditExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="right">
                                                No. of buildings complete
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNoOfBldgComp" onkeypress="return CheckNumeric(event,this);" MaxLength="4"
                                                    runat="server" Width="192px"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtNoOfBldgComp"
                                                    EnableClientScript="False" ErrorMessage="Input 'No of buildings complete'" SetFocusOnError="true"
                                                    ValidationGroup="V1"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-weight: bold" align="right">
                                                Current Testing Details
                                            </td>
                                            <td colspan="5">
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right" valign="top">
                                                &nbsp
                                            </td>
                                            <td align="left" colspan="5">
                                                <asp:Panel ID="pnl" runat="server" Width="100%" BorderWidth="0px" Height="80px" ScrollBars="Vertical">
                                                    <asp:GridView ID="grdTestingDetails" runat="server" AutoGenerateColumns="False" Width="50%"
                                                        OnRowDataBound="grdTestingDetails_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgInsertTestingDetails" runat="server" OnCommand="imgInsertTestingDetails_Click"
                                                                        ImageUrl="Images/AddNewitem.jpg" Height="16px" Width="16px" CausesValidation="false"
                                                                        ToolTip="Add New WorkStatus" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="16px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnDeleteRow" runat="server" OnClick="imgBtnDeleteRow_TestingDetails"
                                                                        ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                                        ToolTip="Delete Row" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="16px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Material Selection">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlMatSelect" runat="server" Width="100%">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Lab Selection">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlLabSelect" runat="server" Width="100%">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle BackColor="#CCCC99" />
                                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                        <HeaderStyle Font-Bold="True" ForeColor="Black" />
                                                        <EditRowStyle BackColor="#999999" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                            <td colspan="6">
                                                &nbsp;
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td colspan="5">
                                                &nbsp;
                                            </td>
                                            <td align="right">
                                                <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" OnClick="lnkSave_Click"
                                                    Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V1">Save</asp:LinkButton>
                                                &nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkCancel" runat="server" CssClass="LnkOver" OnClick="lnkCancel_Click"
                                                    Style="text-decoration: underline; font-weight: bold;">Cancel</asp:LinkButton>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lnkCancelSite" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <script language="javascript" type="text/javascript">
        function CheckNumeric(e) {

            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if ((charCode >= 48 && charCode <= 57) || charCode == 8)
                    return true;
                else
                    return false;
            }
            catch (err) {
                alert(err.Description);
            }
        }

        function validateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var number = el.value.split('.');
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            //just one dot (thanks ddlab)
            if (number.length > 1 && charCode == 46) {
                return false;
            }
            //get the carat position
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                return false;
            }
            return true;
        }

        function onlyAlphabets(e, t) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 8)
                    return true;
                else
                    return false;
            }
            catch (err) {
                alert(err.Description);
            }
        }
    </script>
    <script type="text/javascript">
        function checkNum(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    x.value = '';
                    x.focus();
                    alert("Only Numeric Values Allowed");
                    return false;
                }
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function checkDecimal(x) {
            var ex = /^[0-9]+\.?[0-9]*$/;
            if (ex.test(x.value) == false) {
                x.value = '';
                x.focus();
                alert('Only valid Integer/Decimal Values Allowed');
            }
            //            var s_len = x.value.length;
            //            var s_charcode = 0;
            //            for (var s_i = 0; s_i < s_len; s_i++) {
            //                s_charcode = x.value.charCodeAt(s_i);
            //                if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 46))) {
            //                    x.value = '';
            //                    x.focus();
            //                    alert("Only Decimal Values Allowed");

            //                    return false;
            //                }

            //            }
            //            return true;
        }
    </script>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
    </script>
</asp:Content>
