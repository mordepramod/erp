<%@ Page Title="Client - Upload MOU" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="ClientMOUUpload.aspx.cs" Inherits="DESPLWEB.ClientMOUUpload" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <table style="width: 100%">
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr style="background-color: #ECF5FF;">
                                <td style="width:100px">
                                    <asp:Label ID="lblClient" runat="server" Font-Bold="True" Text="Clients"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearch" runat="server" MaxLength="50" Width="343px" Height="16px"></asp:TextBox>
                                    <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtSearch"
                                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                                        </asp:AutoCompleteExtender>
                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/Search-32.png" OnClick="ImgBtnSearch_Click" Width="16px" />                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                   <asp:HiddenField ID="hfClientId" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 23px" valign="top">
                                    <asp:Panel ID="pnlClientList" runat="server" Width="100%" BorderWidth="0px" Height="400px"
                                        ScrollBars="Auto">
                                        <asp:GridView ID="grdClient" runat="server" AutoGenerateColumns="False" CellPadding="2"
                                            DataKeyNames="CL_Id" ForeColor="#333333" GridLines="Vertical" BorderColor="#DEDFDE"
                                            BorderWidth="1px" Width="100%" OnRowCommand="grdClient_RowCommand">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns> 
                                                <asp:TemplateField HeaderText="Client Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClientId" runat="server" Text='<%#Eval("CL_Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Client Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("CL_Name_var") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Upload MOU">
                                                    <ItemTemplate>
                                                        <asp:FileUpload ID="FileUploadMOU" runat="server" Width="100px"/>&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lnkUploadMOU" runat="server" ToolTip="Upload MOU" Style="text-decoration: underline;"
                                                            CommandArgument='<%#Eval("CL_Id") %>' CommandName="lnkUploadMOU">&nbsp;Upload&nbsp;</asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Download MOU">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDownloadFile" runat="server" Text='<%# Eval("CL_MOUFileName_var") %>' ToolTip="Download File" Style="text-decoration: underline;"
                                                            CommandArgument='<%#Eval("CL_MOUFileName_var")%>' CommandName="DownloadFile"></asp:LinkButton>
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
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }        
    </script>
</asp:Content>


