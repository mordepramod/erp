<%@ Page Title="Route Updation" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="RouteUpdate.aspx.cs" Inherits="DESPLWEB.RouteUpdate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="12%">
                        Import File</td>
                    <td style="width:70%">
                        <asp:FileUpload ID="FileUpload1" runat="server" Width="190px"  />
                         &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" /> 
                 
                      </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                
                <tr style="background-color: #ECF5FF;">
                    <td>
                    </td>
                    <td style="width: 37%"  align="right" >
                        <asp:Label ID="lblCount" runat="server" Text="Total No of Records : 0"></asp:Label>
                        &nbsp;</td>
                    <td  align="right">
                        <asp:LinkButton ID="lnkRouteUpdate" runat="server" Font-Underline="True" 
                            onclick="lnkRouteUpdate_Click">Update</asp:LinkButton>
                        </td>
                    <td align="left">
                        &nbsp;</td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" BorderStyle="Ridge" Height="360px"
                Width="940px" BorderColor="AliceBlue">
                <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 360px; overflow: auto">
                    
                        <asp:GridView ID="gvDetails" runat="server"
                            CssClass="Grid" ForeColor="#333333" 
                            Width="900px" Height="360px" CellPadding="4" GridLines="Both" 
                            onrowdatabound="gvDetails_RowDataBound" >
                             <AlternatingRowStyle BackColor="White" />
                             <EmptyDataTemplate>
                                No records to display
                            </EmptyDataTemplate>
                            <RowStyle BackColor="#EFF3FB" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />                                                        
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
              
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

         validExtensions[0] = 'xls';
         validExtensions[1] = 'xlsx';

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
