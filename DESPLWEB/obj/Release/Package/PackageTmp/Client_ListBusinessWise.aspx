<%@ Page Title="Client List - Business Wise" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="Client_ListBusinessWise.aspx.cs" Inherits="DESPLWEB.Client_ListBusinessWise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td width="20%">
                        <asp:Label ID="Label1" runat="server" Text="Previous Business done for period" Font-Bold="true"></asp:Label>
                    </td>
                    <td width="50%">
                        <asp:Label ID="lblFromDt" runat="server" Text="From Date"></asp:Label>
                        &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txt_Fromdate" runat="server" Width="120px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="orange" Enabled="True"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txt_Fromdate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" AutoComplete="false"
                            Mask="99/99/9999" MaskType="Date" TargetControlID="txt_Fromdate">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblToDt" runat="server" Text="To Date"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txt_ToDate" runat="server" Width="120px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="orange" Enabled="True"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AutoComplete="false"
                            Mask="99/99/9999" MaskType="Date" TargetControlID="txt_ToDate">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkDiscountAmt" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkDiscountAmt_Click" Font-Bold="True" Visible="false">discount</asp:LinkButton>
                        <asp:Label ID="lblDiscountAmt" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td width="5%" align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Current Business not done for period"
                            Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblFromDtND" runat="server" Text="From Date"></asp:Label>
                        &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txt_FromdateND" runat="server" Width="120px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" CssClass="orange" Enabled="True"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txt_FromdateND">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" AutoComplete="false"
                            Mask="99/99/9999" MaskType="Date" TargetControlID="txt_FromdateND">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblToDtND" runat="server" Text="To Date"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txt_ToDateND" runat="server" Width="120px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="orange" Enabled="True"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txt_ToDateND">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender4" runat="server" AutoComplete="false"
                            Mask="99/99/9999" MaskType="Date" TargetControlID="txt_ToDateND">
                        </asp:MaskedEditExtender>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="width: 18px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Business drop by Percent" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="From Percent"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtBusinessDropFrom" runat="server" Width="100px" MaxLength="3"
                            onchange="checkNum(this)"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label5" runat="server" Text="To Percent"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtBusinessDropTo" runat="server" Width="110px" MaxLength="3" onchange="checkNum(this)"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <%--<asp:ImageButton ID="ImgBtnSearchPercentWise" runat="server" ImageUrl="~/Images/Search-32.png"
                             OnClick="ImgBtnSearchPercentWise_Click" Style="width: 18px" Visible="false" />--%>
                    </td>
                    <td>
                    </td>
                </tr>
                  <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="Search by Route" Font-Bold="true"></asp:Label>
                    </td>
                    <td  colspan="3">
                       
                        <asp:DropDownList ID="ddlRouteName" runat="server" Width="195px">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" AutoPostBack="true" OnCheckedChanged="chkClientSpecific_CheckedChanged" />&nbsp;&nbsp;
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <%--<asp:Label ID="lblTotalRecords" runat="server" Text="Total No of Records : 0 "></asp:Label>--%>
                    </td>
                    
                </tr>
                <tr>
                    <td colspan="4" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="350px" Width="940px"
                            BorderStyle="Ridge" BorderColor="AliceBlue">
                            <asp:GridView ID="grdClientList" runat="server" BackColor="#CCCCCC" BorderColor="#DEBA84"
                                BorderWidth="1px" ForeColor="#333333" Width="100%" CellPadding="4" AutoGenerateColumns="False"
                              >
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Client Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientName" runat="server" Text='<%#Eval("ClientName") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Site Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" runat="server" Text='<%#Eval("SiteName") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Address">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Director/Site Incharge Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInchargeName" runat="server" Text='<%#Eval("InchargeName") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Director/Site Incharge EmailId">
                                        <ItemTemplate>
                                           <asp:TextBox ID="lblInchargeEmailId" runat="server" Text='<%# Eval("InchargeEmailId")%>' />
                                         </ItemTemplate>
                                        <ItemStyle Width="270px" HorizontalAlign="Left"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Office Tel. No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOfficeTelNo" runat="server" Text='<%#Eval("OfficeTelNo") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="90px" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Client EmailId">
                                        <ItemTemplate>
                                                <asp:TextBox ID="lblClientEmailId" runat="server" Text='<%# Eval("ClientEmailId")%>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="270px"  HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Business Previous">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBusinessPrevious" runat="server" Text='<%#Eval("BusinessPrevious") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Business Current">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBusinessCurrent" runat="server" Text='<%#Eval("BusinessCurrent") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Business Drop by Percent">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBusinessDropbyPercent" runat="server" Text='<%#Eval("BusinessDropbyPercent") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="true" />
                                    </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="ClientName" HeaderText="Client Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="25%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SiteName" HeaderText="Site Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="25%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Address" HeaderText="Director/Site Address" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="InchargeName" 
                                                HeaderText="Director/Site Incharge Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="InchargeEmailId" 
                                                HeaderText="Director/Site Incharge EmailId" ItemStyle-HorizontalAlign="Left">
                                            <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OfficeTelNo" HeaderText="Office Tel. No." ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ClientEmailId" ItemStyle-Width="150px"  HeaderText="Client EmailId" ItemStyle-HorizontalAlign="Left">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BusinessPrevious" HeaderText="Business Previous" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BusinessCurrent" HeaderText="Business Current" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="BusinessDropbyPercent" 
                                                HeaderText="Business Drop by Percent"  ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                                            </asp:BoundField>--%>
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
