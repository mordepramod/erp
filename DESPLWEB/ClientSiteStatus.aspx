<%@ Page Language="C#" Title="Client Site Status" MasterPageFile="~/MstPg_Veena.Master"  AutoEventWireup="true" CodeBehind="ClientSiteStatus.aspx.cs" Inherits="DESPLWEB.ClientSiteStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="stylized" class="myform" style="height: 470px;">
       <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                   <td width="10%" align="left" style="background-color: #B7CEB0" >
                       <asp:RadioButton ID="RdnClient" runat="server" AutoPostBack="true"  OnCheckedChanged="RdnClient_CheckedChanged" GroupName="Client"/>
                       <asp:Label ID="Label1" runat="server" Text="Client"  ></asp:Label> 
                       <asp:RadioButton ID="RdnSite" runat="server" AutoPostBack="true" OnCheckedChanged="RdnSite_CheckedChanged" GroupName="Client"  />
                       <asp:Label ID="Label3" runat="server" Text="Site"  ></asp:Label> 
                   </td >
                   <td  >
                     <div style="background-color:#EBC5D8;  width:210px">
                     <asp:RadioButton ID="RdnContinued" runat="server"   GroupName="status" />
                     <asp:Label ID="Label4" runat="server" Text="Continued"  ></asp:Label> 
                     <asp:RadioButton ID="RdnDiscontinued" runat="server"    GroupName="status"/>
                     <asp:Label ID="Label5" runat="server" Text="Discontinued"  ></asp:Label> 
                     </div>
                   </td>
                    <td width="10%" align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                         OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
 
                  </tr>
                       <tr  style="height:27px">
                           <td width="12%"  >
                                <asp:CheckBox ID="ChkSpecificClient" runat="server" AutoPostBack="true" 
                                    oncheckedchanged="ChkSpecificClient_CheckedChanged"   />
                                <asp:Label ID="lblChkSpcfic" Text="Specific Client" runat="server"  ></asp:Label> 
                            
                            </td>
                            <td>
                                 &nbsp;
                                <asp:DropDownList ID="ddl_ClientAndSite" runat="server" Visible="false" 
                                    width="306px" >
                                </asp:DropDownList>
                                <asp:TextBox ID="txt_ClientName" width="300px"  Visible="false"  runat="server"></asp:TextBox> &nbsp;
                                <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" OnClientClick="return validateClient();"
                                     ImageUrl="~/Images/Search-32.png" onclick="ImgBtnSearch_Click"  style="margin-top: 0px" 
                                     Visible="false"  />  &nbsp; &nbsp; &nbsp;
                            </td>
                        <td align="right">
                         <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                         Style="text-decoration: underline;" OnClick="lnkSave_Click" Font-Bold="True">Save</asp:LinkButton>
                         &nbsp;&nbsp;&nbsp;
                         <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;" 
                              OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>
                        </td>
                    </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="420px" Width="940px" BorderStyle="Ridge"  BorderColor="AliceBlue">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>    
                            <asp:GridView ID="grdClientSiteStatus" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="None"
                                Width="100%" CellPadding="0" CellSpacing="0"   >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                <asp:TemplateField>
                                      <HeaderTemplate>
                                          <asp:CheckBox ID="cbxSelectAll" OnClick="javascript:SelectAllCheckboxes(this);" runat="server" />
                                      </HeaderTemplate>
                                      <ItemTemplate>
                                          <asp:CheckBox ID="cbxSelect" OnClick="javascript:OnOneCheckboxSelected(this);"  CssClass = '<%#DataBinder.Eval(Container.DataItem, "CL_Id")%>'  runat="server"  />
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                      <asp:Label ID="lbl_Name" runat="server" Text="Name"></asp:Label>
                                  </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Name" Width="450px" runat="server" CssClass = '<%#DataBinder.Eval(Container.DataItem, "SITE_Id")%>'  ReadOnly="true" BackColor="White"></asp:TextBox>
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                      <asp:Label ID="lbl_address" runat="server"  Text="Address"></asp:Label>
                                  </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Address" Width="250px" runat="server" ReadOnly="true" BackColor="White"></asp:TextBox>
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderTemplate>
                                      <asp:Label ID="lbl_status" runat="server" Text="Status"></asp:Label>
                                  </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Status" Width="70px" runat="server" ReadOnly="true"  BackColor="White"></asp:TextBox>
                                     </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                 </asp:TemplateField>
                                <asp:TemplateField>
                                  <HeaderTemplate>
                                      <asp:Label ID="lbl_Note" runat="server" Text="Note"></asp:Label>
                                  </HeaderTemplate>
                                  <ItemTemplate>
                                        <asp:TextBox ID="txt_Note" Width="105px"  runat="server"></asp:TextBox>
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
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        function validateClient() {
            var clent = document.getElementById("<%=ddl_ClientAndSite.ClientID %>");
            if (clent.value == "0") {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please Select the Client Name";
                return false;
            }
            else {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
            }
        }

        function OnOneCheckboxSelected(chkB) {
            var IsChecked = chkB.checked;
            var Parent = document.getElementById('<%= this.grdClientSiteStatus.ClientID %>');
            var cbxAll;
            var items = Parent.getElementsByTagName('input');
            var bAllChecked = true;
            for (i = 0; i < items.length; i++) {
                if (chkB.checked) {
                    chkB.parentElement.parentElement.style.backgroundColor = '#88AAFF';
                }
                else {
                    chkB.parentElement.parentElement.style.backgroundColor = '#FFFFFF';
                }
                if (items[i].id.indexOf('cbxSelectAll') != -1) {
                    cbxAll = items[i];
                    continue;
                }
                if (items[i].type == "checkbox" && items[i].checked == false) {
                    bAllChecked = false;
                    break;
                }
            }
            cbxAll.checked = bAllChecked;
        }

        function SelectAllCheckboxes(spanChk) {
            var IsChecked = spanChk.checked;
            var cbxAll = spanChk;
            var Parent = document.getElementById('<%= this.grdClientSiteStatus.ClientID %>');
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != cbxAll.id && items[i].type == "checkbox") {
                    items[i].checked = IsChecked;
                    if (items[i].checked) {
                        items[i].parentElement.parentElement.style.backgroundColor = '#88AAFF';
                    }
                    else {
                        items[i].parentElement.parentElement.style.backgroundColor = '#FFFFFF';
                    }
                }
            }
        }
    </script>
</asp:Content>
