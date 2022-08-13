<%@ Page Title="Verify Site" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    MaintainScrollPositionOnPostback="true"
    AutoEventWireup="true" CodeBehind="SiteVerify.aspx.cs" Inherits="DESPLWEB.SiteVerify" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 460px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="450px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr>
                    <td align="right">&nbsp;
                    </td>
                    <td align="right"></td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkFetchSiteFromApp" runat="server" Font-Underline="true"
                            OnClick="lnkFetchSiteFromApp_Click">New Site Confirmation Added by SalesApp</asp:LinkButton>

                    </td>
                    <td align="right">
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total No of Records : 0 "></asp:Label>
                        &nbsp;&nbsp;
                     <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                         ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hfClientId" runat="server" />

                    </td>
                    <td>
                        <asp:HiddenField ID="hfSiteId" runat="server" />

                    </td>
                </tr>
            </table>


            <asp:Panel ID="Mainpan" runat="server" BorderStyle="Ridge" Height="380px" ScrollBars="Auto"
                Width="940px" BorderColor="AliceBlue">

                <asp:GridView ID="grdSite" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE"
                    BackColor="#F7F6F3" CssClass="Grid" BorderWidth="1px" Width="100%">
                    <Columns>
                        <%--  <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkApproveSite" runat="server" CommandArgument='<%#Eval("Site_Id") %> '
                                                OnCommand="lnkApproveSite" Font-Underline="true" Text="Approve">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="70px" />
                                    </asp:TemplateField>--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDisableSite" runat="server" CommandArgument='<%#Eval("Cl_Id")%> '
                                    OnCommand="lnkDisableSite" Font-Underline="true" Text="Disable">
                                </asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Client Name">
                            <ItemTemplate>
                                <asp:Label ID="lblClientIdNew" runat="server" Text='<%#Eval("CL_Id") %>' Visible="false" />
                                <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("CL_Name_var") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Client Address">
                            <ItemTemplate>
                                <asp:Label ID="lblClientAddress" runat="server" Text='<%#Eval("CL_OfficeAddress_var") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Client Mobile No.">
                            <ItemTemplate>
                                <asp:Label ID="lblClientMobileNo" runat="server" Text='<%#Eval("CL_mobile_No") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Verified Client">
                            <ItemTemplate>
                                <asp:Label ID="lblClientId" Text='' runat="server" Visible="false" />
                                <asp:TextBox ID="txt_Client" runat="server" Width="200px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                                <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                                    CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                                    ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                                </asp:AutoCompleteExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Site Name">
                            <ItemTemplate>
                                <asp:Label ID="lblSiteIdNew" runat="server" Text='<%#Eval("SITE_Id") %>' Visible="false" />
                                <asp:Label ID="lblSiteName" runat="server" Text='<%#Eval("SITE_Name_var") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Site Address">
                            <ItemTemplate>
                                <asp:Label ID="lblSiteAddress" runat="server" Text='<%#Eval("SITE_Address_var") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Site Mobile No.">
                            <ItemTemplate>
                                <asp:Label ID="lblSiteMobileNo" runat="server" Text='<%#Eval("SITE_mobile_No") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Verified Site">
                            <ItemTemplate>
                                <asp:Label ID="lblSiteId" Text='' runat="server" Visible="false" />
                                <asp:TextBox ID="txt_Site" runat="server" Width="200px" AutoPostBack="true" OnTextChanged="txt_Site_TextChanged"
                                    Text=""></asp:TextBox>
                                <asp:AutoCompleteExtender ServiceMethod="GetSitename" MinimumPrefixLength="0" OnClientItemSelected="SiteItemSelected"
                                    CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Site"
                                    ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                                </asp:AutoCompleteExtender>
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
                </asp:GridView>

            </asp:Panel>


        </asp:Panel>
    </div>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
        function SiteItemSelected(sender, e) {
            $get("<%=hfSiteId.ClientID %>").value = e.get_value();
        }
    </script>
</asp:Content>
