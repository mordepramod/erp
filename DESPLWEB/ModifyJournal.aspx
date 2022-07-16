<%@ Page Title="Modify Journal" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" Theme="duro" CodeBehind="ModifyJournal.aspx.cs" Inherits="DESPLWEB.ModifyJournal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td width="100px">
                        <asp:Label ID="Label2" runat="server" Text="Period"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_FrmDate" runat="server" Width="90px"></asp:TextBox>

                         <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FrmDate">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FrmDate" MaskType="Date"
                                                Mask="99/99/9999" AutoComplete="false" runat="server">
                                    </asp:MaskedEditExtender>

                        <asp:TextBox ID="txt_Todate" runat="server" Width="90px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_Todate">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_Todate" MaskType="Date"
                                                Mask="99/99/9999" AutoComplete="false" runat="server">
                                    </asp:MaskedEditExtender>
                    
                    </td>
                    <td style="text-align: center; width:5%">
                        <asp:RadioButton ID="Rdn_Ok" runat="server" Text="" AutoPostBack="True" GroupName="aaa"
                            OnCheckedChanged="Rdn_Ok_CheckedChanged" />
                            <asp:Label ID="Label1" runat="server" Text="Ok"></asp:Label>
                    </td>
                   <td style="text-align: center; width:8%">
                        <asp:RadioButton ID="Rdn_Hold" runat="server" Text="" AutoPostBack="True" GroupName="aaa"
                            OnCheckedChanged="Rdn_Hold_CheckedChanged" />
                           <asp:Label ID="Label3" runat="server" Text="Hold"></asp:Label>
                   </td>
                   <td style="text-align: center; width:10%">
                        <asp:RadioButton ID="Rdn_Debit" runat="server" Text="" AutoPostBack="True" 
                            GroupName="bb" oncheckedchanged="Rdn_Debit_CheckedChanged"
                           />
                            <asp:Label ID="Label4" runat="server" Text="Debit Note"></asp:Label>
                    </td>
                    <td style="text-align: center; width:10%">
                        <asp:RadioButton ID="Rdn_Credit" runat="server" Text="" AutoPostBack="True" 
                            GroupName="bb" oncheckedchanged="Rdn_Credit_CheckedChanged"
                             />
                         <asp:Label ID="Label5" runat="server" Text="Credit Note"></asp:Label>
                    </td>
                    <td   align="right">
                         <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                           OnClientClick="javascript:history.go(-1);return false;"   ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Client" runat="server" Text="Client"></asp:Label>
                    </td>
                      <td id ="tdImg" runat="server"  align="left"  width="440px"    >
                        <asp:TextBox ID="txt_Client" runat="server" Width="400px"></asp:TextBox>

                            <asp:HiddenField ID="hfClientId" runat="server" />
                            <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1"   OnClientItemSelected = "ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"  
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" 
                            CompletionListCssClass="autocomplete_completionListElement" >
                            </asp:AutoCompleteExtender>  
                           &nbsp;
                         <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/Search-32.png"
                         ToolTip="Search" Style=" Width:19px; Height:18px"  OnClientClick="return validate2();" OnClick="ImgBtnSearch_Click" />
                      </td>
                      <td colspan="5">
                      <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>
                      </td>
                </tr>
            </table>
                   <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto"  BorderStyle="Ridge"  Height="410px" Width="940px" BorderColor="AliceBlue" > 
                <%--     <asp:UpdatePanel ID="UpdatePanelGrd" runat="server">
                       <ContentTemplate>--%>
                        <asp:GridView ID="grdDetailsView" runat="server" AutoGenerateColumns="False" Width="100%"
                            SkinID="gridviewSkin"  OnRowDataBound="grdDetailsView_RowDataBound" OnRowCommand="grdDetailsView_RowCommand">

                            <Columns>
                               <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                      <asp:LinkButton ID="lnkModifyReceipt" runat="server" ToolTip="Modify Receipt"   
                                      Style="text-decoration: underline;"   CommandArgument='<%#Eval("Journal_NoteNo_var") %>'  CommandName="Modify Receipt">Modify</asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                    <ItemStyle  Width="10px" />
                                </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                      <asp:LinkButton ID="lnkPrintReceipt" runat="server" ToolTip="Print"   
                                      Style="text-decoration: underline;"   CommandArgument='<%#Eval("Journal_NoteNo_var") %>'  CommandName="Print Receipt">Print</asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                    <ItemStyle  Width="10px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Journal_NoteNo_var" HeaderText="Receipt No">
                                    <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Journal_Date_dt" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Journal_Amount_dec" HeaderText="Amount" DataFormatString="{0:f2}">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name">
                                    <HeaderStyle HorizontalAlign="Center" Width="400px" />
                                    <ItemStyle HorizontalAlign="left" Width="350px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Journal_Status_bit" HeaderText="Status">
                                    <HeaderStyle HorizontalAlign="Center" Width="82px" />
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:BoundField>

                            </Columns>

                        </asp:GridView>
                  <%--  </ContentTemplate>
                </asp:UpdatePanel>--%>
            </asp:Panel>
           
        </asp:Panel>
    </div>
 
 <script type = "text/javascript">
     function ClientItemSelected(sender, e) {
         $get("<%=hfClientId.ClientID %>").value = e.get_value();
     }
</script>

</asp:Content>
