<%@ Page Title="Disposal Off Material" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="DisposeOffMaterial.aspx.cs" Inherits="DESPLWEB.DisposeOffMaterial" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 10%;">
                        <asp:Label ID="lblDuration" runat="server" Text="Duration"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" Width="130px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txtToDate" Width="130px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td style="width: 40%;">
                        <asp:CheckBox ID="chkSpecificDate" Text="Apply Specific Date for Disposal" AutoPostBack="true"
                            runat="server" oncheckedchanged="chkSpecificDate_CheckedChanged"  Visible="false"/>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txtSpecificDate" Width="130px" runat="server" Enabled="false" Visible="false"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtSpecificDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txtSpecificDate"
                            MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblInwardType" runat="server" Text="Inward Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlInwardType" Width="137px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlInwardType_SelectedIndexChanged">
                            <asp:ListItem Text="---Select---" Value=""></asp:ListItem>
                            <asp:ListItem Text="Aggregate Testing" Value="AGGT"></asp:ListItem>
                            <asp:ListItem Text="Cement Testing" Value="CEMT"></asp:ListItem>
                            <asp:ListItem Text="Mix Design" Value="MF"></asp:ListItem>
                            <asp:ListItem Text="Steel Testing" Value="ST"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp; &nbsp;&nbsp; &nbsp;
                        <asp:RadioButton ID="optPending" Text="Pending" runat="server" GroupName="g1" AutoPostBack="true"
                            oncheckedchanged="optPending_CheckedChanged" />
                        &nbsp; &nbsp;
                        <asp:RadioButton ID="optCompleted" Text="Completed" runat="server" AutoPostBack="true"
                            GroupName="g1" oncheckedchanged="optCompleted_CheckedChanged" />
                        &nbsp; &nbsp;&nbsp; &nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkSelectAll" Text="Select All" runat="server" AutoPostBack="true"
                            oncheckedchanged="chkSelectAll_CheckedChanged" />
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>
                        &nbsp; &nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" runat="server" Font-Bold="True"
                              Style="text-decoration: underline;">Print</asp:LinkButton>
                        &nbsp; &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="height:5px">
                    
                    </td>
                </tr>
                <tr>
                    <td colspan="4" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="380px" Width="940px"
                            BorderStyle="Ridge" BorderColor="AliceBlue">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                            <asp:GridView ID="grdDisposal" runat="server" AutoGenerateColumns="False" BackColor="#F7F6F3"
                                BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="Both"
                                CssClass="Grid" Width="100%" CellPadding="0" CellSpacing="0">
                                <Columns>
                                    <asp:TemplateField HeaderText="Disposal Off" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_OnCheckedChanged"/> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="RecordType" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="RecordNo" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ReceivedDate" HeaderText="Received Date" DataFormatString="{0:dd/MM/yyyy}"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="EnquiryNo" HeaderText="Enquiry No." 
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="DisposedOff" HeaderText="Disposed Off" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="DisposedDate" HeaderText="Disposed Date"  DataFormatString="{0:dd/MM/yyyy}" 
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    
                                    <asp:BoundField DataField="UnderTesting" HeaderText="Under Testing" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> <%--Aggt--%>
                                    <asp:BoundField DataField="UnderDisposal" HeaderText="Under Disposal" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> <%--Aggt--%>

                                    <asp:BoundField DataField="3DaysStr" HeaderText="3 Days Str." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> <%--Cemt--%>
                                    <asp:BoundField DataField="7DaysStr" HeaderText="7 Days Str." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> <%--Cemt--%>
                                    <asp:BoundField DataField="21DaysStr" HeaderText="21 Days Str." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> <%--Cemt--%>

                                    <asp:BoundField DataField="MaterialRecdWithQty" HeaderText="Material Recd. With Qty." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> <%--MF--%>
                                    <asp:BoundField DataField="Grade" HeaderText="Grade" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> <%--MF--%>
                                    <%--<asp:BoundField DataField="7DaysStrMF" HeaderText="7 Days Str." HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> --%><%--MF--%>
                                    <asp:BoundField DataField="StrReqOPC" HeaderText="Str Req (OPC)" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> <%--MF--%>
                                    <asp:BoundField DataField="StrReqPPC" HeaderText="Str Req (PPC)" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> <%--MF--%>

                                    <asp:BoundField DataField="TestingDate" HeaderText="Testing Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> <%--ST--%>
                                    <asp:BoundField DataField="UnderRetention" HeaderText="Under Retention" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> <%--ST--%>
                                    <asp:BoundField DataField="SentToDisposalBin" HeaderText="Send to Disposal Bin" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> <%--ST--%>
                                    <asp:BoundField DataField="SentToDisposalBinDate" HeaderText="Date" HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center" /> <%--ST--%>
                                    <asp:TemplateField HeaderText="Under Disposal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkUnderDisposalST" runat="server" /> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remark" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRemark" Width="148px" runat="server" Text='<%# Bind("txtRemark") %>'></asp:TextBox>
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
                        
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
