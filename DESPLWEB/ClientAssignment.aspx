<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="ClientAssignment.aspx.cs" Inherits="DESPLWEB.ClientAssignment" Title="Veena - Client Assignment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="stylized" class="myform" style="height:550px;">
        <asp:Panel ID="Panel1" runat="server" >
            <asp:HiddenField ID="HiddenField1" runat="server" />
                        
            <table style="width: 100%">
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr style="background-color: #ECF5FF;">
                                <td>
                                    <asp:Label ID="lblAssignment" runat="server" Font-Bold="True" Text="Client Assignment"></asp:Label>
                                </td>
                                <td>
                                <asp:Label ID="lblSelectedClient" runat="server" Font-Bold="True" Text="Select Client"></asp:Label>
                                <asp:DropDownList ID="DDLSelectedClient" runat="server" Width="140px" 
                                         AutoPostBack="true" OnSelectedIndexChanged = "DDLSelectedClient_SelectedIndexChanged">
                                      
                                </asp:DropDownList>
                                    
                                </td>
                                <td>
                                    <asp:Label ID="lblClient" runat="server" Text="" Visible="false"  ></asp:Label>
                                  
                                </td>
                                
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 23px" valign="top">
                                 <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="230px" Width="775px">
                     <asp:GridView ID="grdClientAssignment" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        DataKeyNames="CL_Id" ForeColor="#333333" GridLines="Vertical"
                                        BorderColor="#DEDFDE" BorderWidth="1px" Width="800px" 
                                         onrowcancelingedit="grdClientAssignment_RowCancelingEdit" 
                                         onrowediting="grdClientAssignment_RowEditing" 
                                         onrowupdating="grdClientAssignment_RowUpdating" 
                                         OnRowDataBound = "RowDataBound">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                     
                        
                        
                          <asp:TemplateField HeaderText="Edit" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
                <EditItemTemplate> 
                    <asp:LinkButton ID="lnkUpdateAssignment" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton> 
                    <asp:LinkButton ID="lnkCancelAssignment" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton> 
                </EditItemTemplate> 
               
                <ItemTemplate> 
                    <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton> 
                </ItemTemplate> 
                <ItemStyle Width="35px" />
            </asp:TemplateField> 
                       
                        <asp:TemplateField HeaderText="Client Name">
                            <ItemTemplate>
                                <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("CL_Name_var") %>'/>
                            </ItemTemplate>
                            
                            <ItemStyle Width="100px" />
                        </asp:TemplateField>
                 
                        <asp:TemplateField HeaderText="Credit Limit">
                            <ItemTemplate>
                              <asp:Label ID="lblCreditLimit" runat="server" Text='<%#Eval("CL_Limit_mny") %>'/>
                            </ItemTemplate>
                            <EditItemTemplate>
                            <asp:TextBox ID="txtCreditLimit" runat="server" Text='<%# Eval("CL_Limit_mny") %>'></asp:TextBox> 
                                
                            </EditItemTemplate> 
                            <ItemStyle Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Class">
                            <ItemTemplate>
                              <asp:Label ID="lblCategoryDesc" runat="server" Text='<%#Eval("CategoryDescription_var") %>'/>
                            </ItemTemplate>
                            <EditItemTemplate> 
                                <asp:DropDownList ID="DdlCategoryDesc" runat="server">
                                </asp:DropDownList>
                                <asp:Label ID="lblCategoryDesc" runat="server" Text='<%#Eval("CategoryDescription_var") %>' Visible="false"/>
                            </EditItemTemplate> 
                            <ItemStyle Width="100px" />
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
    </div>
</asp:Content>
