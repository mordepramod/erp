<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="BillOutward.aspx.cs" Inherits="DESPLWEB.BillOutward" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td style="width: 50%">
                        <asp:RadioButton ID="optOutwardPending" runat="server" Text="Outward Pending" GroupName="G1"
                            AutoPostBack="true" OnCheckedChanged="optOutwardPending_CheckedChanged" />
                        &nbsp; 
                        <asp:RadioButton ID="optOutwardCompleted" runat="server" Text="Phy. Outward Pending"
                            GroupName="G1" AutoPostBack="true" OnCheckedChanged="optOutwardCompleted_CheckedChanged" />
                        &nbsp;
                        <asp:RadioButton ID="optPhysicalOutwardCompleted" runat="server" Text="Phy. Outward Completed"
                            GroupName="G1" AutoPostBack="true" OnCheckedChanged="optPhysicalOutwardCompleted_CheckedChanged" />
                    </td>
                    <td style="width: 60%">
                        <asp:Label ID="lblBillDate" runat="server" Text="Bill date " ></asp:Label>&nbsp;&nbsp;
                        <asp:TextBox ID="txt_FromDate" Width="80px" runat="server" ></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; 
                        <asp:TextBox ID="txt_ToDate" Width="80px" runat="server" ></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; 
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                        &nbsp;&nbsp;
                        <asp:Label ID="lblRecords" runat="server" Text="" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblUserName" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 5px">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblOutwardBySel" runat="server" Text="Outward By" Visible="false"></asp:Label>&nbsp;&nbsp;
                        <asp:TextBox ID="txtOutwardBySel" runat="server" Width="150px" Text='' ReadOnly="true"
                            Visible="false"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblOutwardToSel" runat="server" Text="Outward To" Visible="false"></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlUser" runat="server" Width="150px" Visible="false">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblOutwardDateSel" runat="server" Text="Outward Date" Visible="false"></asp:Label>&nbsp;&nbsp;
                        <asp:TextBox ID="txtOutwardDateSel" runat="server" Width="120px" Text='' Visible="false"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtOutwardDateSel">
                        </asp:CalendarExtender>
                        <asp:LinkButton ID="lnkOutwardBillSel" runat="server" OnClick="lnkOutwardBillSel_Click">Outward</asp:LinkButton>
                        <%--&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblAckDate" runat="server" Text="Acknowledgement Date" Visible="false"></asp:Label>&nbsp;&nbsp;
                        <asp:TextBox ID="txtAckDate" runat="server" Width="130px" Text='' Visible="false"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtAckDate">
                        </asp:CalendarExtender> &nbsp;&nbsp;
                        <asp:Label ID="lblAckFile" runat="server" Text="Acknowledgement File" Visible="false"></asp:Label>&nbsp;&nbsp;
                        <asp:FileUpload ID="FileUploadAck" runat="server" Visible="false"/> --%>
                    </td>
                    <td align="right">
                         <asp:LinkButton ID="lnkPrint" runat="server" OnClick="lnkPrint_Click">Print</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 5px">
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="380px"
                Width="940px" BorderColor="AliceBlue">
                <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 380px; overflow: auto">
                        <asp:GridView ID="grdBills" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                            CssClass="Grid" BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                            Width="100%" CellPadding="0" CellSpacing="0" OnRowCommand="grdBills_RowCommand"
                            OnRowDataBound="grdBills_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="BILL_Id" HeaderText="Bill No" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_Date_dt" HeaderText="Bill Date" DataFormatString="{0:dd/MM/yyyy}"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="BILL_RecordType_var" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Outward By">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOutwardBy" runat="server" Width="150px" Text='<%# Eval("OUTW_OutwardBy_var") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Outward To">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOutwardTo" runat="server" Width="150px" Text='<%# Eval("OUTW_OutwardTo_var") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Outward Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOutwardDate" runat="server" Width="80px" Text='<%# Eval("OUTW_OutwardDate_dt","{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtOutwardDate">
                                        </asp:CalendarExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ack Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAckDate" runat="server" Width="80px" Text='<%# Eval("OUTW_AckDate_dt","{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtAckDate">
                                        </asp:CalendarExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Upload Ack File">
                                    <ItemTemplate>
                                        <asp:FileUpload ID="FileUploadAck" runat="server" Width="100px"/>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ack Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAckDate" runat="server" Text='<%# Eval("OUTW_AckDate_dt","{0:dd/MM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Download Ack File">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblFileName" runat="server" Text='<%# Eval("OUTW_AckFileName_var") %>'></asp:Label>--%>
                                        <asp:LinkButton ID="lnkDownloadFile" runat="server" Text='<%# Eval("OUTW_AckFileName_var") %>' ToolTip="Download File" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("OUTW_AckFileName_var")%>' CommandName="DownloadFile"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Booked Date">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBookedDate" runat="server" Width="80px" Text='<%# Eval("OUTW_BookedDate_dt","{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender6" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtBookedDate">
                                        </asp:CalendarExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                    <asp:BoundField DataField="Route_Name_var" HeaderText="Route" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkOutwardBill" runat="server" ToolTip="Outward Bill" Style="text-decoration: underline;"
                                            CommandArgument='<%#Eval("Bill_Id")%>' CommandName="OutwardBill">Outward</asp:LinkButton>
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
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <%--<script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdBills.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdBills.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdBills.ClientID%>').offset().top);

        });
    </script>--%>
    <script type="text/javascript">


        function checkNum(x) {
            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    x.value = '';
                    x.focus();
                    alert("Only Numeric Values Allowed");

                    return false;
                }
            }
            return true;
        }
                   
    </script>
</asp:Content>
