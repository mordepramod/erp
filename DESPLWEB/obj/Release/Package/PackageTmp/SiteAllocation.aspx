<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="SiteAllocation.aspx.cs" Inherits="DESPLWEB.SiteAllocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;" >
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
         ScrollBars="None"   Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr>
                    <td colspan="6">
                        <asp:RadioButton ID="rbRouteWiseAllocation" Checked="true" runat="server" GroupName="R1"
                            AutoPostBack="true" Text="Routewise Site Allocation" OnCheckedChanged="rbRouteWiseAllocation_CheckedChanged" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rbMeWiseAllocation" runat="server" GroupName="R1" AutoPostBack="true"
                            Text="ME Allocation For Device" OnCheckedChanged="rbMeWiseAllocation_CheckedChanged" />
                    </td>
                    <td>
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td style="width: 5%">
                        <asp:Label ID="lblMe" runat="server" Text="Me" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlME" runat="server" Width="350px" Visible="false" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlME_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td  colspan="5">
                        <asp:Label ID="lblCount" runat="server" Text="" Visible="true"></asp:Label>
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="lblRoute" runat="server" Text="Route"></asp:Label>
                    </td>
                    <td style="width: 45%">
                        <asp:DropDownList ID="ddlRoute" runat="server" Width="350px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlRoute_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp; &nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                                <asp:Label ID="lbl_SiteId" runat="server" Text="0" Visible="false"></asp:Label>
                    
                    </td>
                    <td style="width: 18%">
                          <asp:Label ID="lblCountRows" runat="server" Text="" Visible="true"></asp:Label>
                      
                    </td>
                    <td>
                          <asp:LinkButton ID="lnkPrvAllocatedSite" OnClick="lnkPrvAllocatedSite_Click" runat="server" Font-Bold="True"
                            Visible="false" Style="text-decoration: underline;">View Allocated Site</asp:LinkButton>
               
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkClear" OnClick="lnkClearAllocn_Click" runat="server" Font-Bold="True"
                            Visible="false" Style="text-decoration: underline;">Clear Prv. Allocation</asp:LinkButton>
                    </td>
                    <td colspan="2" align="right">
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>
                    &nbsp;&nbsp;    <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" runat="server" Font-Bold="True"
                            Visible="false" Style="text-decoration: underline;">Print</asp:LinkButton>
                    </td>
                </tr>
                  <tr style="background-color: #ECF5FF;">
                  <td colspan="7">
                  
                      <asp:HiddenField ID="hfSiteId" runat="server" />
                  
                  </td>
                  </tr>
                <tr id="RouteDDL">
                    <td colspan="7" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="390px" Width="940px"
                            BorderStyle="Ridge" BorderColor="AliceBlue">
                            <%--  <asp:GridView ID="GridView1" runat="server">
                            </asp:GridView>--%>
                            <asp:GridView ID="grdSiteAllocation" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                ForeColor="#333333" GridLines="Vertical" BorderColor="#DEDFDE" BorderWidth="1px"
                                OnRowDataBound="RowDataBound" Width="100%">
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnAddRow" runat="server" OnCommand="imgBtnAddRow_Click"
                                                ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                ToolTip="Add Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="18px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnDeleteRow" runat="server" OnCommand="imgBtnDeleteRow_Click"
                                                ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                ToolTip="Delete Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Name">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSiteName" runat="server" Width="90%" OnTextChanged="txtSiteName_TextChanged"
                                                AutoPostBack="True"></asp:TextBox>
                                            <asp:AutoCompleteExtender ServiceMethod="GetSitename" MinimumPrefixLength="0" OnClientItemSelected="SiteItemSelected"
                                                CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtSiteName"
                                                ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                                            </asp:AutoCompleteExtender>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Client Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientName" Text='' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteId" Text='' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClId" Text='' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Address">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteAddress" Text='' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRouteId" Text='' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
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
                <tr id="MEDDL">
                    <td colspan="7" valign="top">
                        <asp:Panel ID="Mainpan1" runat="server" ScrollBars="Auto" Height="390px" Width="940px"
                            BorderStyle="Ridge" BorderColor="AliceBlue" Visible="false">
                            <asp:GridView ID="grdSiteAllocationME" runat="server" AutoGenerateColumns="False"
                                CellPadding="3" ForeColor="#333333" GridLines="Vertical" BorderColor="#DEDFDE"
                                BorderWidth="1px" Width="100%">
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="cbxSelectAll" runat="server" onclick="javascript:HeaderClick(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbxSelect" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="18px" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Site Name" DataField="SITE_Name_var" />
                                    <asp:BoundField HeaderText="Client Name" DataField="CL_Name_var" />
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteId" runat="server" Text='<%# Bind("SITE_Id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField HeaderText="Site Id" Visible="False"  DataField="SITE_Id"/>--%>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClId" runat="server" Text='<%# Bind("SITE_CL_Id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText="Client Id" Visible="False"  DataField="SITE_CL_Id"/>--%>
                                    <asp:BoundField HeaderText="Site Address" DataField="SITE_Address_var" />
                                </Columns>
                                <EmptyDataTemplate>
                                    No records to display
                                </EmptyDataTemplate>
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        function SiteItemSelected(sender, e) {
            $get("<%=hfSiteId.ClientID %>").value = e.get_value();
        }
    </script>
    <script type="text/javascript">
        function HeaderClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.grdSiteAllocationME.ClientID %>');
            var TargetChildControl = "cbxSelect";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBx : 0;

        }
        function ChildClick(CheckBox, HCheckBox) {
            var HeaderCheckBox = document.getElementById(HCheckBox);
            if (CheckBox.checked && Counter < TotalChkBx)
                Counter++;
            else if (Counter > 0)
                Counter--;
            if (Counter < TotalChkBx)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBx)
                HeaderCheckBox.checked = true;

            if (CheckBox.checked) {
                CheckBox.parentElement.parentElement.style.backgroundColor = '#FFDFDF';
            }
            else {
                CheckBox.parentElement.parentElement.style.backgroundColor = '#FFFFFF';
            }
        }
        function CheckAll(Checkbox) {
            var grdSiteAssignment = document.getElementById("<%=grdSiteAllocation.ClientID %>");
            for (i = 1; i < grdSiteAssignment.rows.length; i++) {
                grdSiteAssignment.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;

                if (Checkbox.checked) {
                    Checkbox.parentElement.parentElement.style.backgroundColor = '#FFDFDF';
                }
                else {
                    Checkbox.parentElement.parentElement.style.backgroundColor = '#FFFFFF';
                }
            }

        }
    </script>
</asp:Content>
