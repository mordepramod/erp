<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MstPg_Veena.Master"  CodeBehind="DiscountSetting.aspx.cs" Inherits="DESPLWEB.DiscountSetting" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div id="stylized" class="myform" style="height: 470px;">
       <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
         
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                  <td width="10%">
                      <asp:Label ID="Label1" runat="server" Text="Client Name"></asp:Label>
                     
                  </td>  
                  <td>
                      <asp:TextBox ID="txt_ClientName" Width="200px" runat="server" ></asp:TextBox>
                      <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" 
                      ImageUrl="~/Images/Search-32.png" onclick="ImgBtnSearch_Click"  style="margin-top: 0px" 
                      />
                    
                  </td> 
                  <td align="right">
                       <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                       ImageAlign="Right" />
                  </td>     
                </tr>
               
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server"  Text="Site Name"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Site" AutoPostBack="true"  Width="205px"  runat="server" 
                            onselectedindexchanged="ddl_Site_SelectedIndexChanged" >
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkAllSite" runat="server" Text="Apply discount to all Site" />
                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                   <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True" Style="text-decoration: underline;" >Save</asp:LinkButton>
                  </td> 
                </tr>
                         
                <tr>
                    <td>  
                    </td>
                    <td colspan="2" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="410px" Width="300px" BorderStyle="Ridge"  BorderColor="AliceBlue">
                           <asp:UpdatePanel ID="UpdatePanel1" runat="server">    
                            <ContentTemplate>  
                             <asp:GridView ID="grdDiscount" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="None"  
                                Width="100%" CellPadding="0" CellSpacing="0"  >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <asp:BoundField DataField="MATERIAL_RecordType_var"   HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                      ItemStyle-HorizontalAlign="Center" />  
                                     <asp:BoundField DataField="MATERIAL_Name_var" HeaderText="Material Name" HeaderStyle-HorizontalAlign="Center"
                                      ItemStyle-HorizontalAlign="Center" />   
                                    <asp:TemplateField HeaderText="Discount(%)"  HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Discount" style="text-align:right" MaxLength="2" runat="server" onchange="checkNum(this)"  Width="60px"></asp:TextBox>
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
                           </ContentTemplate>
                          </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HiddenField3" runat="server" />   
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField3"
    PopupControlID="pnl1" BackgroundCssClass="ModalPopupBG"  Drag="true" PopupDragHandleControlID="PopupHeader"  >
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnl1" runat="server" 
     Width="600px"  CssClass="rounded_Borders" Height="500px" >
                          
                                <table  style="width: 100%" bgcolor="White"  height="500px" >
                                    <tr valign="top">
                                         <td align="center" colspan="4" bgcolor="White">
                                            <asp:Label ID="lblPnlHeading2" runat="server" Font-Bold="True" ForeColor="#990033"
                                                Text="Select Client"></asp:Label>
                                        </td>
                                    <td align="right" colspan="4"  style="text-align: right">
                                            <asp:ImageButton ID="ImgBtnClose" runat="server" 
                                                ImageUrl="~/Images/cross_icon.png" style="width: 16px;" ToolTip="Close" 
                                                Height="16px" onclick="ImgBtnClose_Click"  />
                                       </td>
                                    </tr>
                                    <tr  valign="top">
                                     <td align="center" colspan="7" bgcolor="White">
                                   <asp:TextBox ID="txt_ClentName" Width="500px" runat="server" ></asp:TextBox>
                                   <asp:ImageButton ID="ImgBtbSearchClient" runat="server" Height="15px" 
                                   ImageUrl="~/Images/Search-32.png" onclick="ImgBtbSearchClient_Click" Width="15px"  /> 
         
                                    </td>
                                     
                                   <td align="center" style="text-align: right">
                                       &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="center" valign="top" colspan="3">
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                       <td align="center" colspan="7"  valign="top" >
                                            <asp:ListBox ID="ListClients" runat="server" Height="400px" Width="520px">
                                            </asp:ListBox>
                                        </td>
                                    </tr>
                  
                                    <tr valign="top">
                                        <td align="right">
                                            &nbsp;</td>
                                       <td  colspan="5" align="right" >
                                          
                                            <asp:ImageButton ID="ImgBtnOk" runat="server" ImageUrl="~/Images/Ok.jpeg" 
                                                onclick="ImgBtnOk_Click" style="height: 30px" ToolTip="Ok" />
                                        </td>
                                        <td>
                                      </td>
                                 </tr>
                     </table>
            </asp:Panel>
         
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">

        function checkNum(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    x.value = '';
                    x.focus();
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Numeric Values Allowed";
                    return false;
                }
                else {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                }
            }
            return true;
        }
    </script>
</asp:Content>

