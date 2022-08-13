<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" EnableEventValidation="false"
    Title="Mistake Register" Theme="duro" AutoEventWireup="true" CodeBehind="MistakeRegister.aspx.cs"
    Inherits="DESPLWEB.MistakeRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF; margin-top: 0px;">
            <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>--%>
            <table style="width: 50%" align="left">
                <tr style="background-color: #ECF5FF;">
                    <td align="left">
                        <asp:Label ID="lblMistakeId" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblMistakeRecNo" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblMistakeRectype" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButton ID="RdnNewMistake" runat="server" OnCheckedChanged="RdnNewMistake_CheckedChanged"
                            AutoPostBack="true" GroupName="Mistake" />
                        <asp:Label ID="Label1" runat="server" Text="New Mistake"></asp:Label>
                        &nbsp; &nbsp;
                        <asp:RadioButton ID="RdnApproveMIstake" runat="server" OnCheckedChanged="RdnApproveMIstake_CheckedChanged"
                            AutoPostBack="true" GroupName="Mistake" />
                        <asp:Label ID="Label4" runat="server" Text="Approve Mistake"></asp:Label>
                        &nbsp; &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="Label15" runat="server" Text="From"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_FromDate" Width="100px" runat="server" OnTextChanged="txt_FromDate_TextChanged"
                            AutoPostBack="true"></asp:TextBox>&nbsp; &nbsp;
                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender4" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        <asp:Label ID="Label16" runat="server" Text="To"></asp:Label>
                        &nbsp; &nbsp;
                        <asp:TextBox ID="txt_ToDate" Width="100px" runat="server" OnTextChanged="txt_ToDate_TextChanged"
                            AutoPostBack="true"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender5" TargetControlID="txt_ToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Mistake For"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_MistakeFor" Width="250px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_MistakeFor_SelectedIndexChanged">
                            <asp:ListItem Text="---Select---" />
                            <asp:ListItem Text="Modify Inward" />
                            <asp:ListItem Text="Modify Bill" />
                            <asp:ListItem Text="Recheck Report" />
                            <asp:ListItem Text="Other" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Type Of Mistake"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_TypeofMistake" Width="250px" runat="server">
                            <asp:ListItem Text="---Select---" />
                            <asp:ListItem Text="Billing" />
                            <asp:ListItem Text="Inward" />
                            <asp:ListItem Text="Outward" />
                            <asp:ListItem Text="Process" />
                            <asp:ListItem Text="Report" />
                            <asp:ListItem Text="Spelling" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Done By"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_DoneBy" Width="245px" ReadOnly="true" runat="server"></asp:TextBox>
                        <%--<asp:DropDownList ID="ddl_DoneBy" Width="250px" AutoPostBack="true" runat="server"
                            OnSelectedIndexChanged="ddl_DoneBy_SelectedIndexChanged">
                        </asp:DropDownList>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="Detected By"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_DetectedBy" Width="250px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label12" runat="server" Text="Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_MistakeDate" Width="100px" ReadOnly="true" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="false" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_MistakeDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txt_MistakeDate"
                            MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlMF" Width="70px" runat="server" Visible="false" 
                            onselectedindexchanged="ddlMF_SelectedIndexChanged" AutoPostBack="true">                            
                            <asp:ListItem Text="MDL" Value="MDL"></asp:ListItem>
                            <asp:ListItem Text="Final" Value="Final"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkAddSupersedeNote" Text="Add Supersede Note" runat="server" Visible="false"/>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label7" runat="server" Text="Description"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Description" Width="350px" runat="server" TextMode="MultiLine"
                            MaxLength="256"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label8" runat="server" Text="Reason"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Reason" runat="server" Width="350px" TextMode="MultiLine" MaxLength="256"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label9" runat="server" Text="Preventive Measure"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_PreventiveMeasure" Width="350px" runat="server" TextMode="MultiLine"
                            MaxLength="256"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label10" runat="server" Text="Corrective Action"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_CorrectiveAction" Width="350px" runat="server" TextMode="MultiLine"
                            MaxLength="256"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <table align="right" width="50%">
                <tr>
                    <td align="center">
                        <asp:Label ID="Label11" runat="server" Text="Record Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_InwardType_DT" Width="180px" runat="server" OnSelectedIndexChanged="ddl_InwardType_DT_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkViewList" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkViewList_Click" Font-Bold="True">View List</asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSave" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkSave_Click" Font-Bold="True">Save</asp:LinkButton>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="3" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="420px" BorderStyle="Ridge"
                            BorderColor="AliceBlue" Width="170px">
                            <asp:GridView ID="grdMistake" runat="server" OnRowDataBound="grdMistake_RowDataBound"
                                AutoGenerateColumns="False" SkinID="gridviewSkin">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="cbxSelectAll" OnClick="javascript:SelectAllCheckboxes(this);" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbxSelect" OnClick="javascript:OnOneCheckboxSelected(this);" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="left" />
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Record No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Number" Width="145px" Style="text-align: center" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Recheck By">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddl_RecheckBy" BorderWidth="0px" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
    </div>
    </asp:Panel> </td> </tr> </table>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
        PopupControlID="pnlMistake" PopupDragHandleControlID="PopupHeader" BackgroundCssClass="ModalPopupBG">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlMistake" runat="server" Width="900px" Height="500px">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table class="DetailPopup" style="width: 100%">
                    <tr valign="top">
                        <td align="center">
                            <asp:Label ID="lblApproveMistake" runat="server" ForeColor="#990033" Text="Approve Mistake"
                                Font-Bold="True" Font-Size="Small"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:ImageButton ID="imgCloseMistakePopup" runat="server" ImageUrl="Images/cross_icon.png"
                                OnClick="imgCloseMistakePopup_Click" ImageAlign="Right" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3">
                            <asp:Label ID="lblFromSearch" runat="server" Text="From" Visible="false"></asp:Label>
                            <asp:TextBox ID="txtFromSearch" Width="100px" runat="server" Visible="false"></asp:TextBox>&nbsp;
                            &nbsp;
                            <asp:CalendarExtender ID="CalendarExtender6" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromSearch">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender6" TargetControlID="txtFromSearch"
                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                            </asp:MaskedEditExtender>
                            <asp:Label ID="lblToSearch" runat="server" Text="To" Visible="false"></asp:Label>
                            &nbsp; &nbsp;
                            <asp:TextBox ID="txtToSearch" Width="100px" runat="server" Visible="false"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender7" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToSearch">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender7" TargetControlID="txtToSearch" MaskType="Date"
                                Mask="99/99/9999" AutoComplete="false" runat="server">
                            </asp:MaskedEditExtender>
                            &nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="optPending" runat="server" Text="Pending" GroupName="g2" />&nbsp;&nbsp;&nbsp;<asp:RadioButton
                                ID="optApproved" runat="server" Text="Approved" GroupName="g2" />&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkFetchList" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                                OnClick="lnkFetchList_Click" Font-Bold="True" Visible="false">Fetch</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div style="width: 100%; height: 400px; overflow: scroll;">
                                <asp:GridView ID="grdApproveMistake" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                    BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="None"
                                    OnRowCommand="grdApproveMistake_RowCommand" Width="100%" CellPadding="0" CellSpacing="0">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%--<asp:LinkButton ID="lnkViewMisTake" runat="server" ToolTip="View Inward" Style="text-decoration: underline;"
                                                                    CommandArgument='<%#Eval("MISTAKE_Id_int") + ";" + Eval("MISTAKE_RecordType_var") + ";" + Eval("MISTAKE_RecordNo_int") %>'
                                                                    CommandName="ViewMisTake">View</asp:LinkButton>--%>
                                                <asp:CheckBox ID="chkSelect" OnCheckedChanged="chkSelect_CheckedChanged" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MISTAKE_Id_int" HeaderText="Mistake No">
                                            <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                            <ItemStyle HorizontalAlign="center" Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MISTAKE_Date" HeaderText="Date" DataFormatString="{0:dd/MM/yy}">
                                            <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                            <ItemStyle HorizontalAlign="center" Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MISTAKE_RecordType_var" HeaderText="Record Type">
                                            <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                            <ItemStyle HorizontalAlign="center" Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MISTAKE_RecordNo_int" HeaderText="Record No">
                                            <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                            <ItemStyle HorizontalAlign="center" Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MISTAKE_MistakeType_var" HeaderText="Type Of Mistake">
                                            <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                            <ItemStyle HorizontalAlign="center" Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MISTAKE_Description_var" HeaderText="Description">
                                            <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MISTAKE_DoneBy_var" HeaderText="Done By">
                                            <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MISTAKE_DetectedBy_var" HeaderText="Deteced By">
                                            <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MISTAKE_Reason_var" HeaderText="Reason">
                                            <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MISTAKE_PreventiveMeasure_var" HeaderText="Preventive Measure">
                                            <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MISTAKE_CorrectiveAction_var" HeaderText="Corrective Action">
                                            <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MISTAKE_MFFinalStatus_bit" HeaderText="Final Status">
                                            <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                            <ItemStyle HorizontalAlign="Left" Width="120px" />
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
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="imgCloseMistakePopup" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
    <%--</ContentTemplate>
            </asp:UpdatePanel>--%>
    </asp:Panel> </div>
    <style type="text/css">
        .Grid th
        {
            color: #fff;
            background-color: #3AC0F2;
        }
        
        .Grid, .Grid th, .Grid td
        {
            border: 1px solid #fff000;
        }
    </style>
    <script type="text/javascript">
        function OnOneCheckboxSelected(chkB) {
            var IsChecked = chkB.checked;
            var Parent = document.getElementById('<%= this.grdMistake.ClientID %>');
            var cbxAll;
            var items = Parent.getElementsByTagName('input');
            var bAllChecked = true;
            for (i = 0; i < items.length; i++) {

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
            var Parent = document.getElementById('<%= this.grdMistake.ClientID %>');
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != cbxAll.id && items[i].type == "checkbox") {
                    items[i].checked = IsChecked;

                }
            }
        }
    </script>
</asp:Content>
