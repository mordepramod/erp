<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" 
    Inherits="DESPLWEB.ClientRegistration" Title="Registration" Codebehind="ClientRegistration.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

        <center>
            <asp:Panel ID="pnlEnquiry" runat="server" BorderWidth="0px" Width="100%" >
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <table style="width: 100%" class="hlable">
                    <tr style="background-color: #f0b683">
                        <%--#ECF5FF--%>
                        <td colspan="3" align="center">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                                                OnClick="imgClosePopup_Click" ImageAlign="Right" />
                            &nbsp;<asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#990000"
                                Text="New Registration" class="title"></asp:Label></td>
                    </tr>
                    <tr>
                       <td align="right" style="width:40%" valign="top">
                       
                            Location
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlLocation" runat="server" Width="250px">
                                  <asp:ListItem Text="--Select--" Value="--Select--">--Select--</asp:ListItem>
                                                <asp:ListItem Text="Pune" Value="Pune"></asp:ListItem>
                                                <asp:ListItem Text="Mumbai" Value="Mumbai"></asp:ListItem>
                                                <asp:ListItem Text="Nashik" Value="Nashik"></asp:ListItem>
                                                <asp:ListItem Text="Metro" Value="Metro"></asp:ListItem>
                            </asp:DropDownList>
                             <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlLocation"
                             InitialValue="--Select--"   EnableClientScript="False" ErrorMessage="Select 'Location'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width:40%" valign="top">
                            Client Name
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtClient" runat="server" CssClass="txt" MaxLength="255" Style="margin-left: 0px"
                                Width="250px"></asp:TextBox>
                              <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtClient"
                                EnableClientScript="False" ErrorMessage="Input 'Client Name'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                          <tr>
                        <td align="right" valign="top">
                            Client Address

                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtClientAddress" runat="server"  Height="50px" TextMode="MultiLine" CssClass="txt"  Style="margin-left: 0px"
                                Width="250px"></asp:TextBox>
                           <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtClientAddress"
                                EnableClientScript="False" ErrorMessage="Input 'Client Address'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            Client Mobile No
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtClientMobNo" runat="server" Width="250px" MaxLength="10"   onkeypress="return validatenumerics(event);"
                                class="txt"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtClientMobNo"
                                EnableClientScript="False" ErrorMessage="Input 'Mobile Number'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                         <tr>
                        <td align="right" valign="top">
                            &nbsp;Email ID
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtEmailId" runat="server" MaxLength="50" Width="250px" class="txt"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEmailId"
                                EnableClientScript="False" ErrorMessage="Input 'Email Id'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server"
                                ControlToValidate="txtEmailId" EnableClientScript="False" ErrorMessage="Invalid Email"
                                ValidationGroup="V1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            Site Name
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSite" runat="server" CssClass="txt" MaxLength="255" Style="margin-left: 0px"
                                Width="250px"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSite"
                                EnableClientScript="False" ErrorMessage="Input 'Site Name'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            Site Address
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSiteAddress" runat="server" Height="50px" TextMode="MultiLine" Width="250px"
                                class="txt"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSiteAddress"
                                EnableClientScript="False" ErrorMessage="Input 'Site Address'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="right">
                            &nbsp; Site Mobile No
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSiteMobNo" runat="server" MaxLength="10" Style="margin-left: 0px"   onkeypress="return validatenumerics(event);"
                                Width="250px" CssClass="txt"></asp:TextBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtSiteMobNo"
                                EnableClientScript="False" ErrorMessage="Input 'Mobile Number'" SetFocusOnError="True"
                                ValidationGroup="V1"></asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                
                    <tr>
                        
                        <td colspan="3" align="center" class="style2">
                            &nbsp;
                            <asp:Label ID="lblMessage" runat="server" ForeColor="#990033" Text="lblMsg" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Button ID="btnAdd" runat="server" Font-Bold="True" Font-Size="8pt" OnClick="btnAdd_Click"
                            BackColor="#f0b683"    Text="Register" Width="70px" Style="margin-left: 0px" Height="25px" ValidationGroup="V1" />
                            <asp:Button ID="btnCancel" runat="server" Font-Bold="True" Font-Size="8pt" OnClick="btnCancel_Click"
                               BackColor="#f0b683"    Text="Cancel" Width="70px" Style="margin-left: 0px" Height="25px" CausesValidation="False" />
                        </td>
                    </tr>
                    <tr style="height:8px; Font-Size:4pt">
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <%--</ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </center>
        <asp:RoundedCornersExtender ID="RoundedCornersExtender2" runat="server" Enabled="True"
            Radius="3" TargetControlID="pnlEnquiry" BorderColor="Black" Color="BurlyWood">
        </asp:RoundedCornersExtender>--%>
     
             
        </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </center>
        <asp:RoundedCornersExtender ID="RoundedCornersExtender2" runat="server" Enabled="True"
            Radius="3" TargetControlID="pnlEnquiry" BorderColor="Black" Color="BurlyWood">
        </asp:RoundedCornersExtender>

      <script type="text/javascript" language="javascript">
        function validatenumerics(key) {
            //getting key code of pressed key
            var keycode = (key.which) ? key.which : key.keyCode;
            //comparing pressed keycodes

            if (keycode > 31 && (keycode < 48 || keycode > 57)) {
                alert(" You can enter only characters 0 to 9 ");
                return false;
            }
            else return true;


        }
    </script>
</asp:Content>

