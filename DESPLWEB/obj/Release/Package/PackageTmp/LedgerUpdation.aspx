<%@ Page Title="Ledger Updation" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" Theme="duro" CodeBehind="LedgerUpdation.aspx.cs" Inherits="DESPLWEB.LedgerUpdation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnl_LedgerUpdate" runat="server"
              Width="100%" Height="480px"  >
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">    
                <ContentTemplate>  
                <table style="width: 100%">
                <tr>
                    <td width="10%" style="text-align: left">
                        <asp:Label ID="lblCatagory" runat="server" Text="Catagory " Width="100px"></asp:Label>
                    </td>
                    <td style="text-align: left" >
                        <asp:DropDownList ID="ddl_CatagoryList" Width="300px" runat="server"  AutoPostBack="true"
                            onselectedindexchanged="ddl_CatagoryList_SelectedIndexChanged">
                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnk_AddNewCatagory" OnCommand="lnk_AddNewCatagory_Click"  Font-Bold="true" Font-Underline="true"
                            ToolTip="Add new Catagory" CommandArgument='<%#Eval("CostCatagory_Id")%>' runat="server">Add New Catagory</asp:LinkButton>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
              </table>
              </ContentTemplate>
              </asp:UpdatePanel>
           
            <div>
                <asp:Panel ID="PnlGvCostCenter" runat="server" Height="450px" ScrollBars="Auto" Width="940px"  BorderColor="AliceBlue" BorderStyle="Ridge">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">    
                <ContentTemplate>  
                    <asp:GridView ID="gvCostCenter" runat="server"  AutoGenerateColumns="False"  SkinID="gridviewSkin"  >
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgInsertCatagory" runat="server" ImageUrl="Images/AddNewitem.jpg"
                                       Height="18px" Width="18px" CausesValidation="false" OnCommand="ImgInsertCatagory_Click" ToolTip="Add New Catagory" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgDeleteCostCenter" runat="server" CausesValidation="false" Height="16px" Width="16px"
                                       ImageUrl="Images/DeleteItem.png" OnCommand="ImgDeleteCostCenter_Click"
                                        ToolTip="Delete Record" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Cost Center Description">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_CostcenterDescp"  MaxLength="200" BorderWidth="0px"  Width="670px"  runat="server"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField  Visible="false" >
                                <ItemTemplate>
                                    <asp:Label ID="lblCostID" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField  HeaderText="Ledger"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAddLedger" CommandName="add" Width="100px" OnCommand="lnkAddLedger_Click" CommandArgument='<%#Eval("CostCenter_Id")%>' Font-Underline="true" runat="server">  Add </asp:LinkButton>
                                </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="Ledger"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkViewLedger" Width="100px"  OnCommand="lnkViewLedger_Click" CommandArgument='<%#Eval("CostCenter_Id")%>'  Font-Underline="true" runat="server">  View </asp:LinkButton>
                                </ItemTemplate>
                          </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                </asp:UpdatePanel>
                 <div style="float:right">
                        <asp:LinkButton ID="lnk_SaveCostCenter"   Font-Bold="true" Font-Underline="true"
                         OnClick="lnk_SaveCostCenter_Click" runat="server" >Save Cost Center</asp:LinkButton>   &nbsp; &nbsp;&nbsp;&nbsp;
                 </div>
              </asp:Panel>
            </div>
            </asp:Panel>

            <asp:HiddenField ID="HiddenField2" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField2" BehaviorID ="ModalBehaviour1"
                PopupControlID="PnlNewCatagory" PopupDragHandleControlID="PopupHeader" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
               
            <asp:Panel ID="PnlNewCatagory" runat="server" Width="550px" Height="100px" >
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <table class="DetailPopup" style="width: 100%">
                            <tr valign="top">
                                <td align="center" valign="top"  colspan="2"  style="height:25px">
                                    <asp:Label ID="lblAddContact" runat="server" Font-Bold="True" ForeColor="#990033" 
                                        Text="Add New Cost Catagory" Font-Size="Small"></asp:Label>
                                </td>
                                <td  align="right">
                                     <asp:ImageButton ID="ImgExit" runat="server" ImageUrl="Images/cross_icon.png"  OnClick="imgExit_Click"   ImageAlign="Right" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label14" runat="server" Text=" Catagory Name"></asp:Label>
                                    &nbsp; &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_CostCatagoryName" runat="server" MaxLength="200" Width="400px" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblCtagoryMsg" Font-Bold="true" runat="server" ForeColor="Red" Text="" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right" valign="top">
                                    &nbsp;
                                    <asp:LinkButton ID="lnkSaveNewCatagory" runat="server" OnClientClick="return validateCatagory();" Font-Bold="true" Font-Underline="true" 
                                        OnClick="lnkSaveNewCatagory_Click" >Save</asp:LinkButton>
                                    &nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkExitCatagory"  Font-Bold="true" Font-Underline="true" runat="server"   OnClick="lnkExitCatagory_Click" >Exit</asp:LinkButton>&nbsp; &nbsp;
                                </td>
                            </tr>
                        </table>

                    </ContentTemplate>
                </asp:UpdatePanel>
              </asp:Panel>

            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="HiddenField1"  BehaviorID ="ModalBehaviour2"
            PopupControlID="PnlLedger" PopupDragHandleControlID="PopupHeader" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>

            <asp:Panel ID="PnlLedger" runat="server" Width="550px" Height="100px">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <table class="DetailPopup" style="width: 100%; height:100px">
                            <tr valign="top">
                                <td align="center" valign="top" colspan="2" style="height:20px"  > 
                                    <asp:Label ID="lblCostCenterName" runat="server" Font-Bold="True" ForeColor="#990033" Text=""  ></asp:Label>
                                </td>
                                <td  align="right">
                                     <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png"  OnClick="imgCloseClick"
                                        ImageAlign="Right" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp; &nbsp;
                                    <asp:Label ID="Label4" runat="server" Text="Ledger Name"></asp:Label>  
                                </td>
                                <td >
                                    <asp:TextBox ID="txt_LedgerName" MaxLength="200" runat="server" Width="400px" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                     <asp:Label ID="lblCostcentId" runat="server" Visible="false"  ></asp:Label>
                                </td>
                                <td style="height:20px">
                                    <asp:Label ID="lblErrMsg" Font-Bold="true" runat="server" ForeColor="Red" Text="" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right" valign="top">
                                    &nbsp;
                                    <asp:LinkButton ID="lnlSaveLedger" runat="server" OnClientClick="return validateLedgerName();" Font-Bold="true" Font-Underline="true" 
                                     OnCommand="lnlSaveLedger_Click" >Save</asp:LinkButton>
                                    &nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkExitLedger"  Font-Bold="true" Font-Underline="true" runat="server"  OnClick="lnkExitLedger_Click" >Exit</asp:LinkButton>&nbsp; &nbsp;
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
     
    
    </div>

    <script type="text/javascript">
        function validateLedgerName() {

            if (document.getElementById("<%=txt_LedgerName.ClientID%>").value == "") {

                document.getElementById('<%=lblErrMsg.ClientID %>').style.visibility = "visible";
                document.getElementById('<%=lblErrMsg.ClientID %>').innerHTML = "Please Enter Ledger Name";
                document.getElementById("<%=txt_LedgerName.ClientID%>").focus();
                return false;
            }
            else {
                document.getElementById('<%=lblErrMsg.ClientID %>').style.visibility = "hidden";
            }
        }

        function validateCatagory() {

            if (document.getElementById("<%=txt_CostCatagoryName.ClientID%>").value == "") {

                document.getElementById('<%=lblCtagoryMsg.ClientID %>').style.visibility = "visible";
                document.getElementById('<%=lblCtagoryMsg.ClientID %>').innerHTML = "Please Enter Catagory Name";
                document.getElementById("<%=lblCtagoryMsg.ClientID%>").focus();
                return false;
            }
            else {
                document.getElementById('<%=lblCtagoryMsg.ClientID %>').style.visibility = "hidden";
            }
        }

    </script>
        
</asp:Content>
