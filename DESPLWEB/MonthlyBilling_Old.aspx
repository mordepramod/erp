<%@ Page Title="Monthly Billing" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="MonthlyBilling_Old.aspx.cs" Inherits="DESPLWEB.MonthlyBilling_Old"
    Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td width="100%">
                        <asp:Label ID="lblFromDate" runat="server" Text="From Date "></asp:Label>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txtFromDate" runat="server" Width="120px"  AutoPostBack="True"
                            ontextchanged="txtFromDate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="lblToDate" runat="server" Text="To Date "></asp:Label>
                        &nbsp;&nbsp;
                        <asp:TextBox ID="txtToDate" Width="120px" runat="server"  AutoPostBack="True"
                            ontextchanged="txtToDate_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                        <asp:RadioButton ID="optAll" Text="All" GroupName="g1" runat="server" 
                            oncheckedchanged="optAll_CheckedChanged" AutoPostBack="true"/>
                        &nbsp; &nbsp;
                        <asp:RadioButton ID="optPending" Text="Pending" GroupName="g1" runat="server" 
                            oncheckedchanged="optPending_CheckedChanged" AutoPostBack="true"/>
                        &nbsp; &nbsp;
                        <asp:RadioButton ID="optPrinted" Text="Printed" GroupName="g1" runat="server" 
                            oncheckedchanged="optPrinted_CheckedChanged" AutoPostBack="true"/>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" >
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server"  AutoPostBack="True"
                            oncheckedchanged="chkClientSpecific_CheckedChanged" />
                        &nbsp;&nbsp;
                        <asp:DropDownList ID="ddlClient" runat="server" Width="350px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label4" runat="server" Text="Site Name"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:DropDownList ID="ddlSite" runat="server" Width="350px"  AutoPostBack="True"
                            onselectedindexchanged="ddlSite_SelectedIndexChanged" >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:LinkButton ID="lnkDisplay" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkDisplay_Click" Font-Bold="True">Display</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkPrint_Click" Font-Bold="True">Print Summary</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrintBill" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkPrintBill_Click" Font-Bold="True">Print Bill</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" style="height: 5px">
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Panel ID="pnlBillList" runat="server" ScrollBars="Auto" Height="390px" Width="100%"
                            BorderStyle="Solid" BorderColor="AliceBlue" BorderWidth="1">
                            <asp:GridView ID="grdBill" SkinID="gridviewSkin" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BillNo" HeaderText="Bill No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BillDate" HeaderText="Bill Date" DataFormatString="{0:dd/MM/yyyy}"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="TestingType" HeaderText="Testing Type" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="Particular" HeaderText="Particular" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="ReportNo" HeaderText="Report No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount " HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="PrintStatus" HeaderText="Print Status" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        
        function checkNum(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    x.value = '';
                    x.focus();
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Numeric Values Allowed";
                    return false;
                }
                else {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                }
            }
            return true;
        }
    </script>
</asp:Content>
