<%@ Page Title="User Sign" Language="C#" MasterPageFile="~/MstPg_Veena.Master" Theme="duro" AutoEventWireup="true" CodeBehind="UserSign.aspx.cs" Inherits="DESPLWEB.UserSign" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="480px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">

            <table width="100%">
            <tr>   
                <td  style="width:12%">
                    <asp:Label ID="Label1" runat="server" Text="User Name"></asp:Label>
                </td>
                <td  style="width:25%">
                    <asp:DropDownList ID="ddlUser" Width="200px" runat="server">
                    </asp:DropDownList>
                </td>
               <td  style="width:5%">
                    <asp:FileUpload ID="FileUploadSign" runat="server" />
               </td>
               <td>
                    <asp:LinkButton ID="lnkAdd" Font-Bold="true" Font-Underline="true" 
                        runat="server" onclick="lnkAdd_Click">Upload</asp:LinkButton>
               </td>
             </tr>
           </table>
           </asp:Panel>
</asp:Content>
