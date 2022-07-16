<%@ Page Title="BadDebt List" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="BadDebt.aspx.cs" Inherits="DESPLWEB.BadDebt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="12%">
                        Import File</td>
                    <td style="width: 37%">
                        <asp:FileUpload ID="FileUpload1" runat="server" Width="190px"  />
                         &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" /> 
                 
                        <br />
                       <%--   <asp:RegularExpressionValidator id="FileUpLoadValidator" runat="server" ErrorMessage="Upload Excel only."
                        ValidationExpression="([a-z]\w*)(.xlsx|.xlsm|.xls)$" ValidationGroup="V1"
                            ControlToValidate="FileUpload1">
                        </asp:RegularExpressionValidator>--%>
                        </td>

                    <td style="width: 70%">
                    <asp:Label ID="Label1" runat="server" Text="format= Bill No |Amount " 
                            style="font-weight: 700"></asp:Label>
                        &nbsp; </td>
                    <td style="width: 348px">
                        
                        &nbsp;</td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td style="height: 19px">
                        Date</td>
                    <td style="width: 37%; height: 19px;">
                         <asp:TextBox ID="txtDate" Width="190px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDate"
                            EnableClientScript="False" ErrorMessage="Input 'Date'" SetFocusOnError="True"
                            ValidationGroup="V1"></asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 15%; height: 19px;">
                        
                        <asp:Label ID="lblNotTransfered" runat="server"></asp:Label>
                        
                        </td>
                    <td align="left" style="width: 348px; height: 19px;">
                        </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td>
                    </td>
                    <td style="width: 37%">
                        &nbsp;</td>
                    <td>
                        &nbsp;
                        </td>
                    <td align="left">
                        &nbsp;</td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" BorderStyle="Ridge" Height="340px"
                Width="940px" BorderColor="AliceBlue">
                <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 330px; overflow: auto">
                    
                        <asp:GridView ID="gvDetails" runat="server"
                            CssClass="Grid" ForeColor="#333333" 
                            Width="100%" CellPadding="4" >
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                No records to display
                            </EmptyDataTemplate>
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />                                                        
                             
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
            <br />
               <div style="width: 940px;">
               <table width="100%">
               <tr>
               <td>
                <asp:Label ID="Label2" runat="server" Text="From Receipt No. "></asp:Label>
                <asp:TextBox ID="txtReceiptNo" Width="100px" MaxLength="50" runat="server"></asp:TextBox>
               </td>
               <td align="right">
                          <asp:LinkButton ID="lnkUpdate" Font-Underline="true" runat="server"   ValidationGroup="V1" 
                              onclick="lnkUpdate_Click">Update</asp:LinkButton>
       
               </td>
               </tr>
               </table>
              </div>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
 <script type="text/javascript">
     function checkFileExtension(elem) {
         var filePath = elem.value;

         if (filePath.indexOf('.') == -1)
             return false;

         var validExtensions = new Array();
         var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();

         validExtensions[0] = 'csv';
         //validExtensions[1] = 'xlsx';

         for (var i = 0; i < validExtensions.length; i++) {
             if (ext == validExtensions[i])
                 return true;
         }

         alert('The file extension ' + ext.toUpperCase() + ' is not allowed!');
         return false;
     }
    </script>

    </div>
 
</asp:Content>
