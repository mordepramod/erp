<%@ Page Title="Discount List" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="DiscountList.aspx.cs" Inherits="DESPLWEB.DiscountList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="30%" align="left">
                        <asp:RadioButton ID="rdnAll" runat="server" GroupName="R1" AutoPostBack="true"
                            OnCheckedChanged="rdnAll_CheckedChanged" Text="All Client" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rdnSelected" runat="server" AutoPostBack="true" GroupName="R1"
                            OnCheckedChanged="rdnSelected_CheckedChanged" Text="Specific Client" />
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged" Visible="false"></asp:TextBox>
                      
                       <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/Search-32.png"  
                            Visible="false" OnClick="ImgBtnSearch_Click" Style="width: 14px" />
                   <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                      </td>
                    <td width="10%" align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr style="height: 27px">
                    <td colspan="3" align="right"> 
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total No of Records : 0 "></asp:Label>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkPrint" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="420px" Width="940px"
                            BorderStyle="Ridge" BorderColor="AliceBlue">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdDiscList" runat="server" BackColor="#CCCCCC" BorderColor="#DEBA84"
                                        Width="100%" BorderWidth="1px" ForeColor="#333333" CellPadding="4" AutoGenerateColumns="False">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DISCOUNT_Introductory_num" HeaderText="Introductory Disc(%)" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="110px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DISCOUNT_Volume_num" ItemStyle-HorizontalAlign="Left" HeaderText="Volume Disc(%)">
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DISCOUNT_TimelyPayment_num" HeaderText="Timely Payment Disc(%)" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DISCOUNT_Advance_num" HeaderText="Advance Disc(%)" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="90px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DISCOUNT_Loyalty_num" HeaderText="Loyalty Disc(%)" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DISCOUNT_Proposal_num" HeaderText="Proposal Disc(%)" ItemStyle-HorizontalAlign="Left"
                                                NullDisplayText="NA">
                                                <ItemStyle HorizontalAlign="Left" Width="110px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DISCOUNT_App_num" HeaderText="App Disc(%)" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                             <asp:BoundField DataField="Applicable" HeaderText="Applicable Disc(%)" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
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
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
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
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
