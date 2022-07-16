<%@ Page Title="Material Collection Status" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    EnableEventValidation="false" Theme="duro" AutoEventWireup="true" CodeBehind="MaterialCollection.aspx.cs"
    Inherits="DESPLWEB.MaterialCollection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 15%">
                        &nbsp;
                        <asp:Label ID="Label7" runat="server" Text="List For "></asp:Label>
                    </td>
                    <td style="width: 10%">
                        <asp:DropDownList ID="ddl_ListFor" AutoPostBack="true" runat="server" Width="206px"
                            OnSelectedIndexChanged="ddl_ListFor_SelectedIndexChanged">
                            <asp:ListItem Text="---Select---" Value="0" />
                            <asp:ListItem Text="Collected" />
                            <asp:ListItem Text="Not Collected" />
                        </asp:DropDownList>
                    </td>
                    <td colspan="7" align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        <asp:CheckBox ID="chk_CollectionDt" Checked="true" AutoPostBack="true" OnCheckedChanged="chk_CollectionDt_CheckedChanged"
                            runat="server" />
                        <asp:Label ID="lblCollecDt" runat="server" Text="Collection Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_CollectionDate" Width="200px" runat="server" AutoPostBack="true"
                            OnTextChanged="txt_CollectionDate_TextChanged" ReadOnly="false"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_CollectionDate">
                        </asp:CalendarExtender>
                        <%--<asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_CollectionDate"
                            MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>--%>
                    </td>
                    <td align="center" style="width: 10%">
                        <asp:Label ID="Label6" runat="server" Text="Collected On "></asp:Label>
                    </td>
                    <td style="width: 10%">
                        <asp:TextBox ID="txt_ActualCollectedOnDt" Width="100px" Enabled="false" runat="server" ReadOnly="true"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ActualCollectedOnDt">
                        </asp:CalendarExtender>
                        <%--<asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ActualCollectedOnDt"
                            MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>--%>
                    </td>
                    <td colspan="3" align="right">
                        <asp:Label ID="lblTotal" runat="server" Text="Total Record   :  0" Width="100px"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:Label ID="lblCarryfwd" runat="server" Text="Carry Forward  :  0" Width="100px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Filter "></asp:Label>
                    </td>
                    <td style="width: 10%">
                        <asp:DropDownList ID="ddlFilter" AutoPostBack="true" Width="206px" runat="server"
                            OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged">
                            <asp:ListItem Text="All" />
                            <asp:ListItem Text="Specific Route" />
                            <asp:ListItem Text="Specific Driver" />
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlListRoute_Driver" Width="204px" runat="server" Enabled="false"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlListRoute_Driver_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" OnClientClick="return validateMat();"
                            ImageUrl="~/Images/Search-32.png" OnClick="ImgBtnSearch_Click" />
                    </td>
                    <td align="center" style="width: 15%">
                        <asp:LinkButton ID="lnk_Carryforword" runat="server" Font-Bold="true" Font-Underline="true"
                            ForeColor="#007979" OnClick="lnk_Carryforword_Click">Carry Forword</asp:LinkButton>
                    </td>
                    <td style="width: 10%">
                        <asp:LinkButton ID="lnk_Collect" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="lnk_Collect_Click">Collected </asp:LinkButton>
                    </td>
                    <td style="width: 7%">
                        <asp:LinkButton ID="lnkSave" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="lnkSave_Click">Save </asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnk_Print" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="lnk_Print_Click">Print </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="9" align="right">
                        <asp:Label ID="Label2" runat="server" Text="Driver "></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlDriver" Width="204px" runat="server" >
                        </asp:DropDownList>
                          &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkClearPreviousAllocation" runat="server"/>
                             <asp:LinkButton ID="lnkClearPreviousAllocation" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="lnkClearPreviousAllocation_Click">Clear Previous Enquiry Allocation</asp:LinkButton>
                     
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkUpdatePickUp" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="lnkUpdatePickUp_Click" >Assign enquiry to driver </asp:LinkButton><%----OnClientClick="return confirm('Are you sure to reset previously allocated enquiry of driver?');"--%>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="370px"
                Width="940px" BorderColor="AliceBlue">
                <asp:GridView ID="grdMaterial" runat="server" AutoGenerateColumns="False" Width="100%"
                    SkinID="gridviewSkin" OnRowDataBound="grdMaterial_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbxSelectAll" OnClick="javascript:SelectAllCheckboxes(this);" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbxSelect" OnClick="javascript:OnOneCheckboxSelected(this);" CssClass='<%#DataBinder.Eval(Container.DataItem, "ENQ_Id")%>'
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ENQ_Id" HeaderText="Enq. No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ENQ_Date_dt" HeaderText="Enq. Date" HeaderStyle-HorizontalAlign="Center"
                            DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="200px" 
                            ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ENQ_ClientExpectedDate_dt" HeaderText="Exp. Date" HeaderStyle-HorizontalAlign="Center"
                            DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="200px" 
                            ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SITE_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MATERIAL_Name_var" HeaderText="Inward Type" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ENQ_CollectedOn_dt" HeaderText="Collected Date" HeaderStyle-HorizontalAlign="Center"
                            DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="200px" 
                            ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ENQ_CollectionDate_dt" HeaderText="Collection Date" HeaderStyle-HorizontalAlign="Center"
                            DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="200px" 
                            ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Edited Collection Date" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_EditedCollectionDt" Text='<%#Eval("ENQ_ModifiedCollectionDate_dt") %>'
                                    runat="server" ReadOnly="true" ></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                    Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_EditedCollectionDt">
                                </asp:CalendarExtender>
                                <%--<asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_EditedCollectionDt"
                                    MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                                </asp:MaskedEditExtender>--%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="LOCATION_Name_var" HeaderText="Location" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Route" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddl_Route" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle Width="150px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CL_Limit_mny" HeaderText="Limit" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:f2}">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CL_BalanceAmt_mny" HeaderText="Balance" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:f2}">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ENQ_Comment_var" HeaderText="Comment" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CONT_Name_var" HeaderText="Contact Person" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CONT_ContactNo_var" HeaderText="Contact No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ENQ_Carryforword_dt" HeaderText="Carry Forword Date "
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="200px" DataFormatString="{0:dd/MM/yyyy}"
                            ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ME_Name" HeaderText="ME" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                         
                        <asp:BoundField DataField="unUsedCoupon" HeaderText="Unused Coupons " HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ENQ_Quantity" HeaderText="Quanity " HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                          <asp:BoundField DataField="ENQ_Reference_var" HeaderText="Reference " HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                          <asp:BoundField DataField="ENQ_Note_var" HeaderText="Remark" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                        </asp:BoundField>
                         <asp:TemplateField HeaderText="Carry Forward Reason" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                            <ItemTemplate>
                                   <asp:TextBox ID="txt_ENQ_CarryFwdReason_var" MaxLength="250"  runat="server" Text='<%#Eval("ENQ_CarryFwdReason_var") %>'></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle Width="150px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ENQ_MobileAppEnqNo_int" HeaderText="App EnqNo" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" NullDisplayText="0" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                       <%-- <asp:BoundField DataField="ENQ_LetterNo_var" HeaderText="Letter No." HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>--%>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </asp:Panel>
    </div>
    <script type="text/javascript">

        function OnOneCheckboxSelected(chkB) {
            var IsChecked = chkB.checked;
            var Parent = document.getElementById('<%= this.grdMaterial.ClientID %>');
            var cbxAll;
            var items = Parent.getElementsByTagName('input');
            var bAllChecked = true;
            for (i = 0; i < items.length; i++) {
                if (chkB.checked) {
                    chkB.parentElement.parentElement.style.backgroundColor = '#FFB9B9';
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
            var Parent = document.getElementById('<%= this.grdMaterial.ClientID %>');
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != cbxAll.id && items[i].type == "checkbox") {
                    items[i].checked = IsChecked;
                    if (items[i].checked) {
                        items[i].parentElement.parentElement.style.backgroundColor = '#FFB9B9';
                    }
                    else {
                        items[i].parentElement.parentElement.style.backgroundColor = '#FFFFFF';
                    }
                }
            }
        }
    </script>
    <script type="text/javascript">
        function validateMat() {

            if (document.getElementById("<%=ddlListRoute_Driver.ClientID%>").value == "---Select---" &&
               document.getElementById("<%=ddlFilter.ClientID%>").value == "Specific Route") {
                alert("Please Select the Specific Route");
                return false;
            }
            else if (document.getElementById("<%=ddlListRoute_Driver.ClientID%>").value == "---Select---" &&
               document.getElementById("<%=ddlFilter.ClientID%>").value == "Specific Driver") {
                alert("Please Select the Specific Driver");
                return false;
            }
        }
    </script>
</asp:Content>
