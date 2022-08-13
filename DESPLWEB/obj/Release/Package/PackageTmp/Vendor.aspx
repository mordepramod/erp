<%@ Page Title="Vendor Master" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="Vendor.aspx.cs" Inherits="DESPLWEB.Vendor" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:RadioButton ID="optAddNew" Text="Add New" runat="server" GroupName="g1" OnCheckedChanged="optAddNew_CheckedChanged" AutoPostBack="true"/>&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="optEditExisting" Text="Edit Existing" runat="server"  GroupName="g1" OnCheckedChanged="optEditExisting_CheckedChanged" AutoPostBack="true"/>
                    </td>
                                
                    <td>
                        <asp:Label ID="lblVendor" runat="server" Text="Vendors" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVendor" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txtVendor_TextChanged" Visible="false"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetVendorName" MinimumPrefixLength="1" OnClientItemSelected="VendorItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtVendor"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfVendorId" runat="server" />
                        <asp:Label ID="lblVendorId" runat="server" Text="0" Visible="false"></asp:Label> 
                        &nbsp;&nbsp;                         
                        <asp:Button ID="btnDisplay" runat="server" Text="Display" OnClick="btnDisplay_Click"  Visible="false"/>
                    </td>
                </tr>   
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr> 
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="pnlVendor" runat="server" BorderWidth="1px">
                            <table>
                                <tr>
                                    <td colspan="6">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top" style="width: 20%">
                                        Firm Name
                                    </td>
                                    <td style="width: 2%">&nbsp;
                                    </td>
                                    <td style="width: 30%">
                                        <asp:TextBox ID="txtFirmName" runat="server" MaxLength="250" Width="192px" onkeypress="return CheckAlphaNumeric(event,this);"></asp:TextBox>
                                        <br />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirmName"
                                            EnableClientScript="False" ErrorMessage="Input 'Firm Name'" SetFocusOnError="True"
                                            ValidationGroup="V1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right" style="width: 15%" valign="top" >
                                        Address
                                    </td>
                                    <td style="width: 2%">&nbsp;
                                    </td>
                                    <td style="width: 30%">
                                        <asp:TextBox ID="txtAddress" runat="server" MaxLength="250" Width="192px" onkeypress="return CheckAlphaNumeric(event,this);"></asp:TextBox>
                                        &nbsp;
                                        <br />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtAddress"
                                            EnableClientScript="False" ErrorMessage="Input 'Address'" SetFocusOnError="true"
                                            ValidationGroup="V1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right" valign="top">Contact No.
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContactNo" runat="server" MaxLength="250" Width="192px" onkeypress="return CheckNumeric(event,this);"></asp:TextBox>
                                        <br />
                                      <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtContactNo"
                                        EnableClientScript="False" ErrorMessage="Input 'Contact No.'" SetFocusOnError="true"
                                        ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td align="right" valign="top">Email Id
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtEmailId" runat="server" EnableViewState="False" MaxLength="250"
                                            Width="192px"></asp:TextBox>
                                        <br />
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="txtEmailId"
                                            EnableClientScript="False" ErrorMessage="Input 'Email Id'" SetFocusOnError="true"
                                            ValidationGroup="V1"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtEmailId"
                                            EnableClientScript="False" ErrorMessage="Invalid Email Id" ValidationExpression="^([a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+,*[\W]*)+$"
                                            ValidationGroup="V1"></asp:RegularExpressionValidator>--%>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right" valign="top">Country
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCountry" runat="server" Width="192px" >
                                        </asp:DropDownList>
                                        <br />
                                      <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlCountry"
                                            EnableClientScript="False" ErrorMessage="Input 'Country'" SetFocusOnError="true"
                                            InitialValue="0" ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td align="right" valign="top">State
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlState" runat="server" Width="192px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                        <br />
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="ddlState"
                                            EnableClientScript="False" ErrorMessage="Input 'State'" SetFocusOnError="true"
                                            InitialValue="0" ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right" valign="top">City
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td valign="top">
                                        <asp:DropDownList ID="ddlCity" runat="server" Width="192px" OnFocus="this.style.borderColor='red'"
                                            AutoPostBack="true" OnBlur="this.style.borderColor='green'">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtCity" runat="server" MaxLength="250" Visible="false" onkeypress="return onlyAlphabets(event,this);"
                                            Width="192px"></asp:TextBox>
                                        <br />                                        
                                        <asp:LinkButton ID="lnkAddCity" runat="server" Font-Underline="true" OnClick="lnkAddCity_Click">New City</asp:LinkButton>
                                        &nbsp;&nbsp;<asp:LinkButton ID="lnkSaveCity" runat="server" Font-Underline="true"
                                            Visible="false" OnClick="lnkSaveCity_Click">Save City</asp:LinkButton>                                        
                                    </td>
                                    <td align="right" valign="top">Pincode
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPincode" runat="server" MaxLength="6" Width="192px" onkeypress="return CheckNumeric(event,this);"></asp:TextBox>
                                        <br />
                                     <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPincode"
                                            EnableClientScript="False" ErrorMessage="Input 'Pin code'" SetFocusOnError="True"
                                            ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right" valign="top">Pan No
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPanNo" runat="server" MaxLength="50" Width="192px" onkeypress="return CheckAlphaNumeric(event,this);"></asp:TextBox>
                                        <br />
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtPanNo"
                                            EnableClientScript="False" ErrorMessage="Input 'Pan No'" SetFocusOnError="True"
                                            ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td align="right" valign="top">GST Registration type
                                    </td>
                                    <td >                                        
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlGSTRegistrationType" runat="server" Width="192px" OnSelectedIndexChanged="ddlGSTRegistrationType_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="---Select---"></asp:ListItem>
                                            <asp:ListItem Text="Composition Consumer"></asp:ListItem>
                                            <asp:ListItem Text="Regular"></asp:ListItem>
                                            <asp:ListItem Text="Un Registered"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                      <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlGSTRegistrationType"
                                            EnableClientScript="False" ErrorMessage="Input 'GST Registration type'" SetFocusOnError="true"
                                            InitialValue="---Select---" ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top">GST No
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtGstNo" runat="server" Enabled="false" MaxLength="15" Width="192px" onkeypress="return CheckAlphaNumeric(event,this);"></asp:TextBox>
                                        <br />
                                        <asp:Label ID="valName1" runat="server" ForeColor="Red" Text="Input 'GST No'" Visible="false"></asp:Label>
                                    </td>
                                    <td align="right" valign="top">Owner Name
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOwnerName" runat="server" MaxLength="250" Width="192px" onkeypress="return CheckAlphaNumeric(event,this);"></asp:TextBox>
                                        <br />
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtOwnerName"
                                            EnableClientScript="False" ErrorMessage="Input 'Owner Name'" SetFocusOnError="True"
                                            ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right" valign="top">Bank Name
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBankName" runat="server" MaxLength="250" Width="192px"></asp:TextBox>
                                        <br />
                                     <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtBankName"
                                            EnableClientScript="False" ErrorMessage="Input 'Bank Name.'" SetFocusOnError="True"
                                            ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td align="right" valign="top">Account No.
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAccountNo" runat="server" MaxLength="50" Width="192px"></asp:TextBox>
                                        <br />
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtAccountNo"
                                            EnableClientScript="False" ErrorMessage="Input 'Account No.'" SetFocusOnError="True"
                                            ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right" valign="top">IFSC
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtIFSC" runat="server" EnableViewState="False" MaxLength="50"
                                            Width="192px"></asp:TextBox>
                                        <br />
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtIFSC"
                                            EnableClientScript="False" ErrorMessage="Input 'IFSC'" SetFocusOnError="true"
                                            ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td align="right" valign="top">Branch
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtBranch" runat="server" MaxLength="250" Width="192px"></asp:TextBox>
                                        <br />
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="txtBranch"
                                            EnableClientScript="False" ErrorMessage="Input 'Branch'" SetFocusOnError="True"
                                            ValidationGroup="V1"></asp:RequiredFieldValidator>--%>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" OnClick="lnkSave_Click"
                            Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V1">Save</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkClear" runat="server" CssClass="LnkOver" OnClick="lnkClear_Click"
                            Style="text-decoration: underline; font-weight: bold;" >Clear</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                       <asp:Label ID="lblVendorMessage" runat="server" ForeColor="#990033" Text="lblMsg"
                                                        Visible="False"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        function VendorItemSelected(sender, e) {
            $get("<%=hfVendorId.ClientID %>").value = e.get_value();
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

        function CheckAlphaNumeric(e) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if ((charCode >= 48 && charCode <= 57) || (charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 8 || charCode == 32)
                    return true;
                else
                    return false;
            }
            catch (err) {
                alert(err.Description);
            }

        }
    </script>
</asp:Content>

