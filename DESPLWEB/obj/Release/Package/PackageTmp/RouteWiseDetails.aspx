<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"  Title="RouteWise Collection Details"  CodeBehind="RouteWiseDetails.aspx.cs" Inherits="DESPLWEB.RouteWiseDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                   
                 
                    <td align="right" colspan=2>                    
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        
                    </td>
                </tr>
                <tr>
                <td>
                    <asp:RadioButton ID="rbRouteWise" runat="server" Text="RouteWise Monthly Business" GroupName="v1"/> &nbsp;
                    <asp:RadioButton ID="rbMaterialWise" runat="server" Text="MaterialWise Monthly Enquiries" GroupName="v1"/>
                    &nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" 
                        ImageAlign="Top" ImageUrl="~/Images/Search-32.png" OnClick="ImgBtnSearch_Click" 
                        Style="margin-top: 0px" />
                </td>
                 <td align="right">  
                <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">Print</asp:LinkButton></td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="400px"
                Width="940px" BorderColor="AliceBlue">
                <asp:GridView ID="grdReportStatus" runat="server"  BackColor="#F7F6F3"
                    CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333"
                    Width="100%" CellPadding="0" 
                    onrowdatabound="grdReportStatus_RowDataBound"  >
                    <FooterStyle BackColor="#CCCC99" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <EmptyDataTemplate>
                        No records to display
                    </EmptyDataTemplate>
                     <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Width="400px" Height="35px" />
                    <EditRowStyle BackColor="#999999" />
                </asp:GridView>
              
            </asp:Panel>
            <div><br /></div>
             <div>
                 <asp:Label ID="Label2" runat="server" Text="Monthly Summary" Font-Bold="True"  Visible=false
                     ForeColor="#3333FF"></asp:Label>
                 <br /></div>
               <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="120px" Visible=false
                Width="940px" BorderColor="AliceBlue">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                    CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333"
                    Width="100%" CellPadding="0"  >
                    <Columns>
                        <asp:BoundField DataField="Month" HeaderText="Month-Year" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                    
                        <asp:BoundField DataField="BillingAmount" HeaderText="Bussiness" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
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
                </asp:GridView>
              
            </asp:Panel>
        </asp:Panel>
    </div>

</asp:Content>
