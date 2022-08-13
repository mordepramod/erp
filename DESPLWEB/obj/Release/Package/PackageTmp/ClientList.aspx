<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="ClientList.aspx.cs" Inherits="DESPLWEB.ClientList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td colspan="3">
                        <asp:RadioButton ID="RdnClient" runat="server" AutoPostBack="true" OnCheckedChanged="AllClient_CheckedChanged"
                            Text="All" GroupName="Client" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="RdnContinued" runat="server" AutoPostBack="true" GroupName="Client"
                            OnCheckedChanged="contClient_CheckedChanged" Text="Continued" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="RdnDiscontinued" runat="server" AutoPostBack="true" Text="Discontinued"
                            GroupName="Client" OnCheckedChanged="disContClient_CheckedChanged" />
                        &nbsp; &nbsp;
                        <asp:RadioButton ID="RdnModifiedList" runat="server" AutoPostBack="true" Text="Modified"
                            GroupName="Client" OnCheckedChanged="RdnModifiedList_CheckedChanged" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="RdnGstComplete" runat="server" AutoPostBack="true" Text="GST Complete"
                            GroupName="Client" OnCheckedChanged="RdnGstComplete_CheckedChanged" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="RdnGstInComplete" runat="server" AutoPostBack="true" Text="GST Incomplete"
                            GroupName="Client" OnCheckedChanged="RdnGstInComplete_CheckedChanged" />
                    </td>
                    <td width="8%" align="right">
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
                <tr style="height: 27px">
                    <td>
                        <asp:Label ID="lblFromDt" runat="server" Text="From Date" Visible="false"></asp:Label>
                        &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txt_Fromdate" runat="server" Width="190px" Visible="false"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="orange" Enabled="True"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txt_Fromdate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" AutoComplete="false"
                            Mask="99/99/9999" MaskType="Date" TargetControlID="txt_Fromdate">
                        </asp:MaskedEditExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblToDt" runat="server" Text="To Date" Visible="false"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txt_ToDate" runat="server" Visible="false" Width="190px"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="orange" Enabled="True"
                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AutoComplete="false"
                            Mask="99/99/9999" MaskType="Date" TargetControlID="txt_ToDate">
                        </asp:MaskedEditExtender>
                        &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/Search-32.png"
                            Visible="false" OnClick="ImgBtnSearch_Click" Style="width: 14px" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total No of Records : 0 "></asp:Label>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkPrint" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkPrint_Click" Font-Bold="True">Print</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="420px" Width="940px"
                            BorderStyle="Ridge" BorderColor="AliceBlue">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdClientList" runat="server" BackColor="#CCCCCC" BorderColor="#DEBA84"
                                        BorderWidth="1px" ForeColor="#333333" Width="100%" CellPadding="4" AutoGenerateColumns="False">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CL_OfficeAddress_var" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Office Address">
                                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CL_State_var" HeaderText="State" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="90px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CL_City_var" HeaderText="City" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="90px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CL_Pin_int" HeaderText="Pin Code" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CL_PANNo_var" HeaderText="PAN No" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CL_TANNo_var" HeaderText="TAN No" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CL_GstNo_var" HeaderText="GST No" ItemStyle-HorizontalAlign="Left"
                                                NullDisplayText="NA">
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CL_AccContactName_var" HeaderText="Contact Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CL_AccContactNo_var" HeaderText="Contact No" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CL_AccEmailId_var" HeaderText="Contact EmailId" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="120px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CL_ModifiedOn_dt" HeaderText="Modified On" ItemStyle-HorizontalAlign="Left"
                                                DataFormatString="{0:dd/MM/yyyy}">
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
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
</asp:Content>
