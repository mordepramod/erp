<%@ Page Title="Device Order" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="DeviceOrder.aspx.cs" Inherits="DESPLWEB.DeviceOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr>
                    <td>
                        <asp:RadioButton ID="rbEnqWithLogin" runat="server" Text="Enquiry with Login" AutoPostBack="true" GroupName="A" OnCheckedChanged="rbEnqWithLogin_CheckedChanged" />&nbsp;&nbsp;
                        <asp:RadioButton ID="rbEnqWithoutLogin" runat="server" Text="Enquiry without Login" AutoPostBack="true" GroupName="A" OnCheckedChanged="rbEnqWithoutLogin_CheckedChanged" />&nbsp;&nbsp;
                        <asp:RadioButton ID="rbEnqWithTestReq" runat="server" Text="Enquiry with Test Request Details" AutoPostBack="true" GroupName="A" OnCheckedChanged="rbEnqWithTestReq_CheckedChanged" />&nbsp;&nbsp;
                         <asp:LinkButton ID="lnkDisplay" runat="server" Font-Underline="True" ForeColor="Blue"
                             Width="25%" OnClick="lnkDisplay_Click">Display</asp:LinkButton>

                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total No of Records : 0 "></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <%--   <td id="chkOption" colspan="2" runat="server"> 
                        <asp:RadioButton ID="rbCollected" runat="server" Text="Collected" GroupName="A1" Checked="true"/>&nbsp;&nbsp;&nbsp;
                           <asp:RadioButton ID="rbNotCollected" runat="server" Text="Not Collected"  GroupName="A1"/>
                    </td>--%>
                    <td colspan="2" align="right">
                        <asp:LinkButton ID="lnkPrint" runat="server" OnClick="lnkPrint_Click">Print</asp:LinkButton>
                    </td>
                </tr>
                <%--  <tr>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="2" style="height: 23px" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="420px"
                            Width="940px" BorderColor="AliceBlue">
                            <div style="height: 420px; overflow: auto">
                                <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="#F7F6F3"
                                    OnRowCommand="grdReportStatus_RowCommand" CssClass="Grid" BorderWidth="1px" Width="100%" OnRowDataBound="grdReport_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="cbxSelectAll" OnClick="javascript:SelectAllCheckboxes(this);" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbxSelect" OnClick="javascript:OnOneCheckboxSelected(this);" AutoPostBack="true"
                                                    runat="server" CssClass='<%#Eval( "EnqDetails" ) %>' /><%----%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Enq No." HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEnq_Id" runat="server" Text='<%# Eval("Enq_Id") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="enq_date" HeaderText="Enquiry Date" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="160px" />
                                        <asp:BoundField DataField="Site_Name_var" HeaderText="Site Name" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="160px" />
                                        <asp:BoundField DataField="MATERIAL_Name_var" HeaderText="Material Name" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150px" />
                                        <asp:BoundField DataField="test_Name" HeaderText="Test Name" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ENQ_contact_person" HeaderText="Contact Person" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" />
                                        <asp:BoundField DataField="ENQ_contact_number" HeaderText="Contact No" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkModifyReport" runat="server" ToolTip="Modify Report" Style="text-decoration: underline;"
                                                    CommandArgument='<%#Eval("testEnq_id")  + ";" + Eval("material_id")+ ";" + Eval("MATERIAL_Name_var") %>' CommandName="ModifyReport">Modify</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEnqDetails" runat="server" Text='<%#Eval("EnqDetails")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="EnqVeena No." HeaderStyle-Width="50px" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEnqVeena_Id" runat="server" Text='<%# Eval("VeenaEnqNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkViewReport" runat="server" ToolTip="View Report" Style="text-decoration: underline;"
                                                    CommandArgument='<%#Eval("testEnq_id") + ";" + Eval("material_id")+ ";" + Eval("MATERIAL_Name_var") %>' CommandName="ViewReport">Details</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEditEnq" runat="server" ToolTip="Edit Enquiry" Style="text-decoration: underline;"
                                                    CommandArgument='<%#Eval("testEnq_id") + ";" + Eval("material_id")+ ";" + Eval("MATERIAL_Name_var") %>' CommandName="EditEnquiry">Edit Enquiry</asp:LinkButton>
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
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                PopupControlID="Panel2" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="Panel2" runat="server" Height="470px">
                <table class="DetailPopup" width="850px" height="350px">
                    <tr>
                        <td>
                            <b>Test Request Form Details : </b>
                        </td>
                        <td align="right">
                            <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                                OnClick="imgClosePopup_Click" ImageAlign="Right" Width="15px" Height="15px" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                            <asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Vertical">
                                <asp:GridView ID="grdDetails" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="#F7F6F3"
                                    CssClass="Grid" BorderWidth="1px" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="test_Name" HeaderText="Test Name" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" />--%>
                                        <asp:BoundField DataField="Make" HeaderText="Make" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="supplier" HeaderText="Supplier" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="grade" HeaderText="Grade" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="description" HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="specimen" HeaderText="Specimen" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="Idmark1" HeaderText="Idmark" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />

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
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField2" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="HiddenField1"
                PopupControlID="Panel3" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="Panel3" runat="server" Height="470px" ScrollBars="Horizontal" Width="1000px">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table class="DetailPopup" width="98%" height="400px">
                            <tr>
                                <td>
                                    <b>Test Request Form Details : </b>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblMsgCommon" runat="server" ForeColor="#990033" Text="Updated Successfully."
                                        Visible="False"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:LinkButton ID="lnkUpdateCommon" runat="server" Style="text-decoration: underline;" OnClick="lnkUpdateCommon_Click">Update</asp:LinkButton>
                                    &nbsp;            
                            <asp:ImageButton ID="imgClosePopup1" runat="server" ImageUrl="Images/cross_icon.png"
                                OnClick="imgClosePopup1_Click" ImageAlign="Right" Width="15px" Height="15px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblDetailCommom" runat="server" ForeColor="#990033" Text=""
                                        Font-Bold="True" Font-Size="Small"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <asp:Panel ID="Panel4" runat="server" Height="400px" Width="970px">
                                        <asp:GridView ID="grdCommon" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="#F7F6F3"
                                            CssClass="Grid" BorderWidth="1px" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTestId" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Make">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMake" runat="server" Width="80px" Text='<%# Eval("make") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Supplier">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSupplier" runat="server" Width="95%" Text='<%# Eval("supplier") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Material Specification/Grade">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtgrade" runat="server" Width="95%" Wrap="true" Text='<%# Eval("specification") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDescription" runat="server" Width="95%" Text='<%# Eval("description") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No Of Specimen">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSpecimen" runat="server" Width="95%" Text='<%# Eval("specimen") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Idmark">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtIdmark" runat="server" Width="95%" Text='<%# Eval("Idmark1") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
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
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="imgClosePopup1" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField3" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="HiddenField3"
                PopupControlID="Panel5" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="Panel5" runat="server" Height="470px" ScrollBars="Horizontal" Width="1020px">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <table class="DetailPopup" width="100%" height="400px">
                            <tr>
                                <td>
                                    <b>Test Request Form Details : </b>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblMsgCube" runat="server" ForeColor="#990033" Text="Updated Successfully."
                                        Visible="False"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:LinkButton ID="lnkUpdateCube" runat="server" Style="text-decoration: underline;" OnClick="lnkUpdateCube_Click">Update</asp:LinkButton>
                                    &nbsp;            
                                <asp:ImageButton ID="imgClosePopup2" runat="server" ImageUrl="Images/cross_icon.png"
                                    OnClick="imgClosePopup2_Click" ImageAlign="Right" Width="15px" Height="15px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblDetailCube" runat="server" ForeColor="#990033" Text=""
                                        Font-Bold="True" Font-Size="Small"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <asp:Panel ID="Panel6" runat="server" Height="400px" Width="990px">
                                        <asp:GridView ID="grdCube" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="#F7F6F3"
                                            CssClass="Grid" BorderWidth="1px" Width="98%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Grade">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlGrade" runat="server" BorderWidth="0px" Width="100px">
                                                            <asp:ListItem Text="Select" Value="Select" />
                                                            <asp:ListItem Text="M5" Value="M5" />
                                                            <asp:ListItem Text="M7.5" Value="M7.5" />
                                                            <asp:ListItem Text="M10" Value="M10" />
                                                            <asp:ListItem Text="M15" Value="M15" />
                                                            <asp:ListItem Text="M20" Value="M20" />
                                                            <asp:ListItem Text="M25" Value="M25" />
                                                            <asp:ListItem Text="M30" Value="M30" />
                                                            <asp:ListItem Text="M35" Value="M35" />
                                                            <asp:ListItem Text="M37" Value="M37" />
                                                            <asp:ListItem Text="M40" Value="M40" />
                                                            <asp:ListItem Text="M45" Value="M45" />
                                                            <asp:ListItem Text="M50" Value="M50" />
                                                            <asp:ListItem Text="M55" Value="M55" />
                                                            <asp:ListItem Text="M60" Value="M60" />
                                                            <asp:ListItem Text="M65" Value="M65" />
                                                            <asp:ListItem Text="M70" Value="M70" />
                                                            <asp:ListItem Text="M75" Value="M75" />
                                                            <asp:ListItem Text="M80" Value="M80" />
                                                            <asp:ListItem Text="M85" Value="M85" />
                                                            <asp:ListItem Text="M90" Value="M90" />
                                                            <asp:ListItem Text="M95" Value="M95" />
                                                            <asp:ListItem Text="M100" Value="M100" />
                                                            <asp:ListItem Text="NA" />
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Casting Date Of Pour">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCastingDt" Width="100px" ReadOnly="false" runat="server"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description Of Work(Footing,Column,Slab etc)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDescriptionWork" runat="server" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location Of Work">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLoactnWork" runat="server" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Make/Suppiler(RMC)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMake" runat="server" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No Of Specimen">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSpecimen" runat="server" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Testing Schedule">
                                                    <ItemTemplate>
                                                        <%--<asp:DropDownList ID="ddlTestingSchedule" runat="server" BorderWidth="0px" Width="100px">
                                                </asp:DropDownList>--%>
                                                        <asp:TextBox ID="txtTestingSchedule" runat="server" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Idmark">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtIdmark" runat="server" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTestId" runat="server" Text='<%# Eval("lblTestId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
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
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="imgClosePopup2" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField4" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="HiddenField4"
                PopupControlID="Panel9" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="Panel9" runat="server" Height="470px" ScrollBars="Horizontal" Width="1020px">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <table class="DetailPopup" width="100%" height="400px">
                            <tr>
                                <td>
                                    <b>Test Request Form Details : </b>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblMsgSteel" runat="server" ForeColor="#990033" Text="Updated Successfully."
                                        Visible="False"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:LinkButton ID="lnkUpdateSteel" runat="server" Style="text-decoration: underline;" OnClick="lnkUpdateSteel_Click">Update</asp:LinkButton>
                                    &nbsp;            
                          <asp:ImageButton ID="imgClosePopup3" runat="server" ImageUrl="Images/cross_icon.png"
                              OnClick="imgClosePopup3_Click" ImageAlign="Right" Width="15px" Height="15px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblDetailSteel" runat="server" ForeColor="#990033" Text=""
                                        Font-Bold="True" Font-Size="Small"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <asp:Panel ID="Panel8" runat="server" Height="400px" Width="990px">
                                        <asp:GridView ID="grdSteel" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="#F7F6F3"
                                            CssClass="Grid" BorderWidth="1px" Width="98%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Diameter Selection">
                                                    <ItemTemplate>
                                                        <%--<asp:DropDownList ID="ddlDiameter" runat="server" BorderWidth="0px" Width="100px">                                                    
                                                </asp:DropDownList>--%>
                                                        <asp:TextBox ID="txtDiameter" runat="server" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Make">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMake" runat="server" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Suppiler">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtSuppiler" runat="server" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Material Specifiction/Grade">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlGrade" runat="server" BorderWidth="0px" Width="100px">
                                                            <asp:ListItem Text="Select" Value="Select" />
                                                            <asp:ListItem Text="Fe 250" Value="Fe 250" />
                                                            <asp:ListItem Text="Fe 415" Value="Fe 415" />
                                                            <asp:ListItem Text="Fe 415D" Value="Fe 415D" />
                                                            <asp:ListItem Text="Fe 500" Value="Fe 500" />
                                                            <asp:ListItem Text="Fe 500D" Value="Fe 500D" />
                                                            <asp:ListItem Text="Fe 550" Value="Fe 550" />
                                                            <asp:ListItem Text="Fe 550D" Value="Fe 550D" />
                                                            <asp:ListItem Text="Fe 600" Value="Fe 600" />
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description(Challan No)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDescription" runat="server" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No of Bars">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBars" runat="server" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Idmark">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtIdmark" runat="server" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTestId" runat="server" Text='<%# Eval("lblTestId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
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
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="imgClosePopup3" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField5" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="HiddenField5"
                PopupControlID="Panel7" PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="Panel7" runat="server" Height="470px" ScrollBars="Horizontal" Width="1020px">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table class="DetailPopup" width="100%" height="400px">
                            <tr>
                                <td>
                                    <b>Test Request Form Details : </b>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblMsgMixDesign" runat="server" ForeColor="#990033" Text="Updated Successfully."
                                        Visible="False"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:LinkButton ID="lnkUpdateMixDesign" runat="server" Style="text-decoration: underline;" OnClick="lnkUpdateMixDesign_Click">Update</asp:LinkButton>
                                    &nbsp;            
                            <asp:ImageButton ID="imgClosePopup4" runat="server" ImageUrl="Images/cross_icon.png"
                                OnClick="imgClosePopup4_Click" ImageAlign="Right" Width="15px" Height="15px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblDetailMixDesign" runat="server" ForeColor="#990033" Text=""
                                        Font-Bold="True" Font-Size="Small"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <asp:Panel ID="Panel10" runat="server" Height="500px" Width="990px">
                                        <asp:Panel ID="Panel11" runat="server" Height="300px" Width="990px">
                                            <asp:GridView ID="grdMixDesign" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="#F7F6F3"
                                                CssClass="Grid" BorderWidth="1px" Width="98%" OnRowCommand="grdMixDesign_RowCommand">
                                                <%--OnRowDataBound="grdMixDesign_RowDataBound"--%>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Type Of Design">
                                                        <ItemTemplate>
                                                            <%-- <asp:DropDownList ID="ddlDesign" runat="server" BorderWidth="0px" Width="100px">
                                                </asp:DropDownList>--%>
                                                            <asp:TextBox ID="txtDesign" runat="server" Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Grade">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlGrade" runat="server" BorderWidth="0px" Width="100px">
                                                                <asp:ListItem Text="Select" />
                                                                <asp:ListItem Text="M5" />
                                                                <asp:ListItem Text="M7.5" />
                                                                <asp:ListItem Text="M10" />
                                                                <asp:ListItem Text="M15" />
                                                                <asp:ListItem Text="M20" />
                                                                <asp:ListItem Text="M25" />
                                                                <asp:ListItem Text="M30" />
                                                                <asp:ListItem Text="M35" />
                                                                <asp:ListItem Text="M37" />
                                                                <asp:ListItem Text="M37.5" />
                                                                <asp:ListItem Text="M40" />
                                                                <asp:ListItem Text="M45" />
                                                                <asp:ListItem Text="M50" />
                                                                <asp:ListItem Text="M55" />
                                                                <asp:ListItem Text="M60" />
                                                                <asp:ListItem Text="M65" />
                                                                <asp:ListItem Text="M70" />
                                                                <asp:ListItem Text="M75" />
                                                                <asp:ListItem Text="M80" />
                                                                <asp:ListItem Text="M85" />
                                                                <asp:ListItem Text="M90" />
                                                                <asp:ListItem Text="M95" />
                                                                <asp:ListItem Text="M100" />
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Material Combination Required">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtMterialCombn" runat="server" Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Slump(mm)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSlump" runat="server" Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Retention Period">
                                                        <ItemTemplate>
                                                            <%--<asp:DropDownList ID="ddlRetenstionPeriod" runat="server" BorderWidth="0px" Width="100px">
                                                </asp:DropDownList>--%>
                                                            <asp:TextBox ID="txtRetenstionPeriod" runat="server" Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Flow(mm)">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFlow" runat="server" Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nature Of Work">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtNatureWork" runat="server" Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Id" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTestId" runat="server" Text='<%# Eval("lblTestId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkViewMaterial" runat="server" ToolTip="View Material" Style="text-decoration: underline;"
                                                                CommandArgument='<%#Eval("lblTestId")%>' CommandName="ViewMaterial">Material</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
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
                                          
                                        </asp:Panel>
                                        <asp:Panel ID="pnlMaterial" runat="server" Height="100px" Width="990px">
                                            <asp:GridView ID="grdMaterial" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                ForeColor="#333333" GridLines="Both" BorderColor="#DEDFDE" BackColor="#F7F6F3"
                                                CssClass="Grid" BorderWidth="1px" Width="98%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="materialName" HeaderText="Material Name" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="description" HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="make" HeaderText="Make" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" />
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
                                        </asp:Panel>
                                    </asp:Panel>
                                </td>
                            </tr>

                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="imgClosePopup4" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:Panel>
    </div>
      <script type="text/javascript">

        function OnOneCheckboxSelected(chkB) {
            var IsChecked = chkB.checked;
            var Parent = document.getElementById('<%= this.grdReport.ClientID %>');
            var cbxAll;
            var items = Parent.getElementsByTagName('input');
            var bAllChecked = true;
            for (i = 0; i < items.length; i++) {
                if (chkB.checked) {
                    chkB.parentElement.parentElement.style.backgroundColor = '#FFFFFF';
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
            var Parent = document.getElementById('<%= this.grdReport.ClientID %>');
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != cbxAll.id && items[i].type == "checkbox") {
                    items[i].checked = IsChecked;
                    if (items[i].checked) {
                        items[i].parentElement.parentElement.style.backgroundColor = '#FFFFFF';
                    }
                    else {
                        items[i].parentElement.parentElement.style.backgroundColor = '#FFFFFF';
                    }
                }
            }
        }
    </script>
</asp:Content>
