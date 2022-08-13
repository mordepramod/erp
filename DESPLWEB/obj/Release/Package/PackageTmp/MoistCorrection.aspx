<%@ Page Title="Moisture Correction " Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="MoistCorrection.aspx.cs" Theme="duro" Inherits="DESPLWEB.MoistCorrection" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
                   <div style="float:right">
                    <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                        ImageAlign="Right" />
                   </div>
                    <table  width="100%" >
                     <tr style="background-color: #ECF5FF;">
                       <td  style="width:15%" >
                           <asp:Label ID="Label1" runat="server" Text="Record No"></asp:Label>
                       </td>
                       <td style="width:25%">
                           <asp:TextBox ID="txt_RecType" style="text-align:center" Width="41px" runat="server"  ></asp:TextBox>   
                           <asp:TextBox ID="txt_RefNo" Width="150px" runat="server"  ></asp:TextBox>   
                       </td>
                       <td >
                           <asp:Label ID="Label2" runat="server" Text="Description"></asp:Label>
                       </td>
                       <td >
                           <asp:TextBox ID="txt_Desc"  Width="470px" runat="server"  MaxLength="256"  ></asp:TextBox>
                       </td>
                     </tr>
                     <tr>
                       <td>
                           <asp:Label ID="Label4" runat="server" Font-Bold="true" BackColor="#EEDDFF" Text=" Correction Of Moisture"></asp:Label>
                      </td>
                     </tr>
                    </table>

                        <asp:TextBox ID="txtFA2" runat="server"   BackColor="#EBFCF7"
                        style="text-align:center" Text="FA 2 - Water To be Added " Width="700px"></asp:TextBox>
               
                <div style="float:left">
                <asp:TextBox ID="txtFA1" runat="server" Height="220px"   BackColor="#EBFCF7"
                    style="text-align:center" Text="FA1" Width="22px"></asp:TextBox>
                </div>
                <div style="float:left">
                <asp:Panel ID="Mainpan" runat="server" BorderColor="AliceBlue" 
                    BorderStyle="Ridge" Height="200px" ScrollBars="Auto" Width="700px">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdMoistureCorr" runat="server" AutoGenerateColumns="False" 
                                SkinID="gridviewSkin">
                                <Columns>
                                    <asp:TemplateField HeaderText="Total Moisture(%)">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_TotalMoisture" runat="server" BorderWidth="0px" 
                                                CssClass="caltextbox"  Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Water to be added">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_WaterTobeAdded" runat="server" BorderWidth="0px" 
                                                CssClass="caltextbox"  Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Wt. of Fine Aggt.">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_WtofFineAggt" runat="server" BorderWidth="0px" 
                                                CssClass="caltextbox"  Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="1">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_1" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                                 Width="60px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="2">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_2" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                                 Width="60px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="3">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_3" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                                 Width="60px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="4">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_4" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                                 Width="60px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="5">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_5" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                                 Width="60px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="6">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_6" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                                 Width="60px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="7">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_7" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                                 Width="60px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="8">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_8" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                                 Width="60px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="9">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_9" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                                 Width="60px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
               </div>
               <div style="float:right" >
                <asp:Panel ID="PnlEquip" runat="server" BorderColor="AliceBlue" 
                    BorderStyle="Ridge" Height="200px" ScrollBars="Auto" Width="210px">
                    <asp:Label ID="Label3" Font-Bold="true" runat="server" BackColor="#EEDDFF" Text="Method of moisture correction in absence of equipment."></asp:Label>
                     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdEquipment" runat="server" AutoGenerateColumns="False" 
                                SkinID="gridviewSkin">
                                <Columns>
                                    <asp:TemplateField HeaderText="Condition of Sand">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_ConditionofSand" runat="server" BorderWidth="0px" 
                                                 Style="text-align: right" Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Water to be" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Watertobe" runat="server" BorderWidth="0px" 
                                                 Style="text-align: right" Width="100px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>
            <asp:Panel ID="PnlNSICS" runat="server" BorderColor="AliceBlue"  
                BorderStyle="Ridge" Height="180px" ScrollBars="Auto" Width="940px">
                <asp:GridView ID="grdNSICS" runat="server" AutoGenerateColumns="False" Visible="false"
                    SkinID="gridviewSkin">
                    <Columns>
                        <asp:TemplateField HeaderText="NSICS">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_NSICS" runat="server" BorderWidth="0px" 
                                    CssClass="caltextbox"  Width="80px"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="1">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_1" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                     Width="90px"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="2">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_2" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                     Width="90px"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="3">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_3" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                     Width="90px"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="4">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_4" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                     Width="90px"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="5">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_5" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                     Width="90px"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="6">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_6" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                     Width="90px"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="7">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_7" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                     Width="90px"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="8">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_8" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                     Width="90px"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="9">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_9" runat="server" BorderWidth="0px" CssClass="caltextbox" 
                                     Width="90px"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <table width="100%">
                <tr>
                    <td align="right">
                        <asp:LinkButton ID="lnkSave" runat="server" Font-Bold="True" OnClientClick="return validateData();"
                            OnClick="lnkSave_Click" Style="text-decoration: underline;">Save</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="LnkExit" runat="server" Font-Bold="True" 
                            OnClick="lnk_Exit_Click" Style="text-decoration: underline;">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
         </asp:Panel>
        </div>
    
   <script type="text/javascript">

       function validateData() {
           var lblmsg = document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>');
           if (document.getElementById("<%=txt_Desc.ClientID%>").value == "") {
               lblmsg.innerHTML = "Please Enter Description";
               lblmsg.style.visibility = "visible";
               document.getElementById("<%=txt_Desc.ClientID%>").focus();
               return false;
           }
           else {
               lblmsg.style.visibility = "hidden";
           }
       }

  </script>
</asp:Content>
