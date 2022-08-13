<%@ Page Title="Inward Test Satus" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="InwardTestStartedStatus.aspx.cs" Inherits="DESPLWEB.InwardTestStartedStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <%--<tr style="background-color: #ECF5FF;">
                    <td width="12%">
                        <asp:Label ID="Label1" runat="server" Text="Inward App. Date "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_FromDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txt_ToDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp;&nbsp; &nbsp;
                    </td>
                    <td colspan="2" align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>--%>
                <tr style="background-color: #ECF5FF;">
                    <td width="12%">
                        <asp:Label ID="Label2" runat="server" Text="Inward Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_InwardTestType" Width="155px" runat="server" AutoPostBack="true"
                            onselectedindexchanged="ddl_InwardTestType_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                   <asp:Label ID="lblUserId" runat="server" Text="0" Visible="false"></asp:Label>
                        </td>
                    <td align="left">
                        <asp:LinkButton ID="lnkUpdateStatus" OnClick="lnkUpdateStatus_Click" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;">Update</asp:LinkButton>
                    </td>
                    <td>
                        <asp:Label ID="lbl_RecordsNo" runat="server" Text=""></asp:Label>
                    </td>
                      <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="420px" Width="940px"
                            BorderStyle="Ridge" BorderColor="AliceBlue">
                          <%--<div style="width: 940px;">
                                <div id="GHead">
                                </div>
                                <div style="height: 420px; overflow: auto">--%>
                                    <asp:GridView ID="grdInward" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                                        BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                                        CssClass="Grid" Width="100%" CellPadding="0" CellSpacing="0" OnRowCreated="grdInward_RowCreated"
                                        OnRowCommand="grdInward_RowCommand">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="cbxSelectAll" runat="server" onclick="javascript:HeaderClick(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbxSelect" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="RecordType" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="RecordNo" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="CollectionDate" HeaderText="Collection Date" DataFormatString="{0:dd/MM/yyyy}"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ReceivedDate" HeaderText="Received Date" DataFormatString="{0:dd/MM/yyyy}"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                               <asp:BoundField DataField="ApprovedDate" HeaderText="Approved Date" DataFormatString="{0:dd/MM/yyyy}"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                         <%--   <asp:BoundField DataField="ContactNo" HeaderText="Contact No." HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="TESt_Name_var" HeaderText="Test Name" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />--%>
                                            <asp:BoundField DataField="RouteName" HeaderText="Route" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="MEName" HeaderText="Marketing Executive" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Left" />
                                        <%--    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkUpdateTestStatus" runat="server" ToolTip="Modify Inward" Style="text-decoration: underline;"
                                                        CommandArgument='<%#Eval("RecordType") + ";" + Eval("ReferenceNo")  %>' CommandName="UpdateTestStatus">Update Test Started Status</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                        <EmptyDataTemplate>
                                            No records to display
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                    </asp:GridView>
                              <%--</div>
                            </div>--%>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
          <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">

        var TotalChkBx;
        var Counter;
        window.onload = function () {
            TotalChkBx = parseInt('<%= this.grdInward.Rows.Count %>');
            Counter = 0;
        }
        function HeaderClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.grdInward.ClientID %>');
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
            var grdInward = document.getElementById("<%=grdInward.ClientID %>");
            for (i = 1; i < grdCredit.rows.length; i++) {
                grdInward.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;

                if (Checkbox.checked) {
                    Checkbox.parentElement.parentElement.style.backgroundColor = '#FFDFDF';
                }
                else {
                    Checkbox.parentElement.parentElement.style.backgroundColor = '#FFFFFF';
                }
            }

        }
    </script>
    <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdInward.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdInward.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdInward.ClientID%>').offset().top);

        });
    </script>
</asp:Content>
