<%@ Page Title=" Proposal" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    Theme="duro" CodeBehind="Proposal.aspx.cs" Inherits="DESPLWEB.Proposal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .Initial {
            display: block;
            padding: 2px 15px 2px 15px;
            float: left;
            color: Gray;
            font-weight: bold;
        }

            .Initial:hover {
                color: White;
                background-color: Olive;
            }

        .Clicked {
            float: left;
            display: block;
            background-color: #9FB7E8;
            padding: 2px 15px 2px 15px;
            color: Black;
            font-weight: bold;
            color: Maroon;
        }
    </style>
    <div id="stylized" class="myform" style="height: 670px;">
        <asp:Panel ID="Panel1" runat="server" Width="100%">
            <table width="100%">
                <tr>
                    <td width="10%">
                        <asp:Label ID="Label1" runat="server" Text="Enquiry No. "></asp:Label>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="txt_EnquiryNo" ReadOnly="true" Width="185px" runat="server"></asp:TextBox>
                    </td>
                    <td width="10%">
                        <asp:Label ID="Label6" runat="server" Text="Proposal No."></asp:Label>
                    </td>
                    <td align="left" width="20%">
                        <asp:TextBox ID="txt_ProposalNo" CssClass="ctextbox" Text="Create New..." ReadOnly="true"
                            Width="185px" runat="server"></asp:TextBox>
                    </td>
                    <td width="17%" align="right">
                        <asp:Label ID="Label3" runat="server" Text="Proposal Date"></asp:Label>
                    </td>
                    <td width="20%">
                        <asp:TextBox ID="txt_ProposalDt" Width="200px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ProposalDt">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_ProposalDt"
                            MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClName" runat="server" Text="Client Name"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtClientNm" Width="285px" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkEditAdd" runat="server" OnClick="lnkEditAdd_Click" Font-Underline="true">Edit Client Address</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblSiteNm" runat="server" Text="Site Name"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSiteName" Width="318px" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Kind Attention"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_KindAttention" Width="185px" runat="server"></asp:TextBox>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblProContNo" runat="server" Text="Contact No"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtProContactNo" Width="185px" runat="server" MaxLength="15"></asp:TextBox>
                    </td>
                    <td align="left" colspan="2">
                        <asp:Label ID="Label8" runat="server" Text="Subject"></asp:Label>
                        &nbsp;&nbsp;
                        <%--  </td>
                    <td align="left">--%>
                        <asp:TextBox ID="txt_Subject" Width="267px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label4" runat="server" Text="Description"></asp:Label>
                        <asp:Label ID="lbl_TestType" runat="server" Font-Bold="true" Text=""></asp:Label>
                        <asp:Label ID="lblModifiedType" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblEnqNewClient" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="txt_Description" TextMode="MultiLine" Width="800px" Height="45px"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="5">
                        <asp:CheckBox ID="chkMOUWorkOrder" runat="server" Text="MOU / Work Order Available"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td valign="middle">
                        <asp:Label ID="Label14" runat="server" Text="Inward Type"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddl_InwardTestType" Width="135px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_InwardTestType_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:DropDownList ID="ddlOtherTest" Width="135px" runat="server" AutoPostBack="true"
                            Visible="false" OnSelectedIndexChanged="ddlOtherTest_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:LinkButton ID="Lnk_AddMainGrd" OnClick="Lnk_AddMainGrd_Click" Font-Underline="true"
                            runat="server">Add Inward Test</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lnkAddNewRow" OnClick="lnkAddNewRow_Click" Font-Underline="true"
                            runat="server" Visible="true">Add Row</asp:LinkButton>
                    </td>
                    <td align="left" colspan="2">
                        <asp:Label ID="lblLumSum" runat="server" Text="LumpSump" Visible="false"></asp:Label>
                        <asp:CheckBox ID="chkLums" OnCheckedChanged="chk_Lumshup_OnCheckedChanged" runat="server"
                            Visible="false" AutoPostBack="true" />
                        <asp:Label ID="Label12" runat="server" Text="FromRow" Visible="false"></asp:Label>
                        <asp:TextBox ID="txtFrmRow" runat="server" Width="25px" MaxLength="2" Visible="false"
                            onchange="checkNum(this)"></asp:TextBox>
                        <asp:Label ID="Label13" runat="server" Text="ToRow" Visible="false"></asp:Label>
                        <asp:TextBox ID="txtToRow" runat="server" Width="25px" MaxLength="2" Visible="false"
                            onchange="checkNum(this)"></asp:TextBox>
                        <asp:Button ID="Btn_Apply" Text="Apply" Visible="false" OnClick="Btn_Apply_OnClick"
                            Width="45px" runat="server" />
                        <%--    <asp:Label ID="lblDepth" runat="server" Text="Depth"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;                        
                             <asp:Button ID="btnApplyRate" Text="Apply" OnClick="Btn_ApplyRate_OnClick" 
                            Width="45px" runat="server"/>--%>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblRType" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lbl_MaterialId" runat="server" Visible="false" Text=""></asp:Label>
                        <asp:Label ID="lblProposalId" runat="server" Visible="false" Text=""></asp:Label>
                        <asp:Label ID="lblSuperAdmin" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblUserLevel" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:LinkButton ID="lnkDepth" OnClick="lnkDepth_Click" Font-Underline="true" runat="server"
                            Visible="false">Change Depth</asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkClear" OnClick="lnkClear_Click" Font-Underline="true" runat="server">Clear</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkQty" runat="server" Checked="true" Text="Print With Qty." />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:Panel ID="pnlStructAudit" runat="server" Width="100%" Visible="false">
                            <table width="100%">
                                <tr>
                                    <td width="18%">
                                        <asp:Label ID="lblStructNameOfApartSoc" runat="server" Text="Name of Apartment / Society"></asp:Label>
                                    </td>
                                    <td width="20%">
                                        <asp:TextBox ID="txtStructNameOfApartSoc" Width="200px" runat="server"></asp:TextBox>
                                    </td>
                                    <td width="25%">
                                        <asp:Label ID="lblStructAddress" runat="server" Text="Address"></asp:Label>
                                    </td>
                                    <td width="20%">
                                        <asp:TextBox ID="txtStructAddress" Text="" Width="200px" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblStructBuiltupArea" runat="server" Text="Builtup Area of Society"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStructBuiltupArea" Width="200px" runat="server" onchange="javascript:checkint(this);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblStructNoOfBuild" runat="server" Text="No of buildings in Society"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStructNoOfBuild" Text="" Width="200px" runat="server" onchange="javascript:checkint(this);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblStructAge" runat="server" Text="Age of Building"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStructAge" Width="200px" runat="server" onchange="javascript:checkint(this);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblStructConstWithin5Y" runat="server" Text="All buildings constructed with in 5 years range ?"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStructConstWithin5Y" runat="server" Width="100px">
                                            <asp:ListItem Text="-Select-"></asp:ListItem>
                                            <asp:ListItem Text="Yes"></asp:ListItem>
                                            <asp:ListItem Text="No"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblStructLocation" runat="server" Text="Location"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStructLocation" runat="server" Width="200px">
                                            <asp:ListItem Text="-Select-"></asp:ListItem>
                                            <asp:ListItem Text="Coastal ( 1 km from Sea )"></asp:ListItem>
                                            <asp:ListItem Text="Coastal  ( 5 Km from sea )"></asp:ListItem>
                                            <asp:ListItem Text="Noncoastal"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAddLoadExpc" runat="server" Text="Any additional loads expected on building"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStructAddLoadExpc" runat="server" Width="100px">
                                            <asp:ListItem Text="-Select-"></asp:ListItem>
                                            <asp:ListItem Text="Yes"></asp:ListItem>
                                            <asp:ListItem Text="No"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lblStructDistressObs" runat="server" Text="Any Distress Observed - Cracks wider than 1mm, more than 1 m length  and growing in last one year by 25% "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStructDistressObs" runat="server" Width="100px">
                                            <asp:ListItem Text="-Select-"></asp:ListItem>
                                            <asp:ListItem Text="Yes"></asp:ListItem>
                                            <asp:ListItem Text="No"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlEntryGrd" runat="server" Height="230px" ScrollBars="Vertical" Width="100%"
                BorderStyle="Ridge" BorderColor="AliceBlue">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table align="center" width="98%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblDiscApplyToAll" runat="server" Text="Discount"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="txtDiscApplyToAll" Width="100px" runat="server" onchange="javascript:checkDecimal(this);"></asp:TextBox>
                                    &nbsp;&nbsp;
                                    <asp:CheckBox ID="chkDiscApplyToAll" runat="server" Checked="false" Text="Apply To All" />
                                    &nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkDiscApplyToAll" OnClick="lnkDiscApplyToAll_Click" Font-Underline="true"
                                        runat="server">Apply</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="PnlGrd" runat="server" ScrollBars="Vertical" Width="98%" BorderStyle="Ridge"
                            BorderColor="AliceBlue">
                            <%--Height="270px"--%>
                            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>--%>
                            <asp:GridView ID="grdProposal" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin"
                                OnRowDataBound="grdProposal_RowDataBound" Width="100%">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgEdit" runat="server" OnCommand="imgEdit_Click" ImageUrl="Images/Edit.jpg"
                                                Height="18px" Width="18px" CausesValidation="false" ToolTip="Edit New Row" />
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" OnCommand="imgDelete_Click"
                                                Height="19px" ImageUrl="Images/DeleteItem.png" ToolTip="Delete Row" Width="18px" />
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Particular" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Particular" BorderWidth="0px" Width="210px" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Test Method"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_TestMethod" BorderWidth="0px" Width="120px" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Unit Rate" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Rate" BorderWidth="0px" Width="90px" Style="text-align: right"
                                                ReadOnly="true" MaxLength="8" runat="server" onchange="javascript:checkDecimal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Discount(%)"
                                        ItemStyle-HorizontalAlign="Center" Visible="False">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Discount" BorderWidth="0px" Width="100px" Style="text-align: right"
                                                runat="server" onchange="javascript:checkDecimal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Discounted Rate"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_DiscRate" BorderWidth="0px" Width="100px" Style="text-align: right"
                                                ReadOnly="true" runat="server" onchange="javascript:checkDecimal(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Quantity" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Quantity" Text="" BorderWidth="0px" Width="60px" Style="text-align: right"
                                                runat="server" onchange="javascript:checkint(this);"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Amount" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Amount" Text="" BorderWidth="0px" ReadOnly="true" Width="100px"
                                                Style="text-align: right" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterStyle BackColor="White" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Inward Type"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_InwdType" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="SiteRate" ItemStyle-HorizontalAlign="Center"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_SiteWiseRate" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="TestId" ItemStyle-HorizontalAlign="Center"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_TestId" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <%--</ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" >OnRowDataBound="grdGT_RowDataBound"
                    <ContentTemplate>--%>
                            <asp:GridView ID="grdGT" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                BorderColor="#DEBA84" BorderWidth="1px" ForeColor="#333333" GridLines="None"
                                CellPadding="0" CellSpacing="0">
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField Visible="false"></asp:TemplateField>
                                    <asp:TemplateField Visible="false"></asp:TemplateField>
                                    <asp:TemplateField Visible="false"></asp:TemplateField>
                                    <asp:TemplateField Visible="false"></asp:TemplateField>
                                    <asp:TemplateField Visible="false"></asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgDeleteGT" runat="server" CausesValidation="false" OnCommand="imgDeleteGT_Click"
                                                Height="19px" ImageUrl="Images/DeleteItem.png" ToolTip="Delete Row" Width="18px" />
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                        <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SrNo.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <HeaderStyle Width="60px" />
                                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Particular" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Particular" CssClass="textbox" Width="220px" runat="server"
                                                Style="text-align: left"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="320px" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Test Method"
                                        ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_TestMethod" BorderWidth="0px" Width="120px" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Unit" CssClass="textbox" Width="100px" Style="text-align: center"
                                                MaxLength="50" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Quantity" Text="" CssClass="textbox" Width="80px" Style="text-align: Center"
                                                MaxLength="4" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Rate">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_UnitRate" CssClass="textbox" Width="80px" Style="text-align: center"
                                                onchange="javascript:checkDecimal(this);" ReadOnly="true" MaxLength="15" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Disc. Rate">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_DiscRate" CssClass="textbox" Width="90px" Style="text-align: center"
                                                onchange="javascript:checkDecimal(this);" MaxLength="15" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Amount" Text="" CssClass="textbox" Width="100px" OnTextChanged="OnTextChanged"
                                                AutoPostBack="true" ReadOnly="true" MaxLength="15" Style="text-align: center"
                                                runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                        <%--OnTextChanged="OnTextChanged" AutoPostBack="true"--%>
                                        <FooterStyle BackColor="White" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inward Type" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_InwdType" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
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
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                            <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
                        </asp:Panel>
                        <table align="center" width="98%">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkGTDiscNote" runat="server" Checked="false" Visible="false" />
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblGTDiscNote" Visible="false" runat="server" Text="A discount of 5% of total billing will be given if 100% amount is paid as advance before start of work."></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="Panel4" runat="server" ScrollBars="Vertical" Width="98%">
                            <%--Height="150px"--%>
                            <table align="center" width="98%">
                                <tr>
                                    <td>
                                        <asp:Button ID="Tab_Notes" runat="server" BorderStyle="None" CssClass="Initial" Height="20px"
                                            OnClick="Tab_Notes_Click" Text="Notes" />
                                        <asp:Button ID="Tab_Discount" runat="server" BorderStyle="None" CssClass="Initial"
                                            Height="20px" OnClick="Tab_Discount_Click" Text="Discount Details" />
                                        <asp:Button ID="Tab_GenericDiscount" runat="server" BorderStyle="None" CssClass="Initial"
                                            Height="20px" OnClick="Tab_GenericDiscount_Click" Text="Generic Discount Details" />
                                        <asp:MultiView ID="MainView_Proposal" runat="server" ActiveViewIndex="0">
                                            <asp:View ID="View_Notes" runat="server">
                                                <asp:Panel ID="PanNotes" runat="server" BorderColor="AliceBlue" BorderStyle="Ridge"
                                                    ScrollBars="Vertical" Width="100%">
                                                    <%--Height="130px"--%>
                                                    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>--%>
                                                    <asp:GridView ID="Grd_Note" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin"
                                                        Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnAddRow" runat="server" CausesValidation="false" Height="18px"
                                                                        ImageUrl="Images/AddNewitem.jpg" OnCommand="imgBtnAddRow_Click" ToolTip="Add Row"
                                                                        Width="18px" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="18px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnDeleteRow" runat="server" CausesValidation="false" Height="16px"
                                                                        ImageUrl="Images/DeleteItem.png" OnCommand="imgBtnDeleteRow_Click" ToolTip="Delete Row"
                                                                        Width="16px" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="16px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="lblSrNo" HeaderStyle-HorizontalAlign="Center" HeaderText="Sr.No"
                                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Note" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_NOTE" runat="server" BorderWidth="0px" Text="" Width="99%"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <br />
                                                    <asp:Label ID="lblAddChargesGT" runat="server" Text="Additional Charges" Visible="false"></asp:Label>
                                                    <asp:GridView ID="Grd_NoteGT" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin"
                                                        Width="100%" Visible="false">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnAddNotesRowGT" runat="server" CausesValidation="false"
                                                                        Height="18px" ImageUrl="Images/AddNewitem.jpg" OnCommand="imgBtnAddNotesRowGT_Click"
                                                                        ToolTip="Add Row" Width="18px" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="18px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnDeleteNotesRowGT" runat="server" CausesValidation="false"
                                                                        Height="16px" ImageUrl="Images/DeleteItem.png" OnCommand="imgBtnDeleteNotesRowGT_Click"
                                                                        ToolTip="Delete Row" Width="16px" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="16px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="lblSrNoGT" HeaderStyle-HorizontalAlign="Center" HeaderText="Sr.No"
                                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Note" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_NOTEGT" runat="server" BorderWidth="0px" Text="" Width="99%"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <br />
                                                    <asp:Label ID="lblPayTermsGT" runat="server" Text="Payment Terms" Visible="false"></asp:Label>
                                                    <asp:GridView ID="grdPayTermsGT" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                                        BorderColor="#DEBA84" BorderWidth="1px" Visible="false" ForeColor="#333333" GridLines="Both"
                                                        Width="100%" CellPadding="0" CellSpacing="0">
                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnAddRowPayTermGT" runat="server" CausesValidation="false"
                                                                        Height="18px" ImageUrl="Images/AddNewitem.jpg" OnCommand="imgBtnAddRowPayTermGT_Click"
                                                                        ToolTip="Add Row" Width="18px" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="18px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnDeleteRowPayTermGT" runat="server" CausesValidation="false"
                                                                        Height="16px" ImageUrl="Images/DeleteItem.png" OnCommand="imgBtnDeleteRowPayTermGT_Click"
                                                                        ToolTip="Delete Row" Width="16px" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="16px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="lblSrNoPayTermGT" HeaderStyle-HorizontalAlign="Center" HeaderText="Sr.No"
                                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Note" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtNotePayTermGT" runat="server" BorderWidth="0px" Text="" Width="99%"></asp:TextBox>
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
                                                    <br />
                                                    <asp:Label ID="lblClientScope" runat="server" Text="Client Scope" Visible="false"></asp:Label>
                                                    <asp:GridView ID="grdClientScope" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                                        BorderColor="#DEBA84" BorderWidth="1px" Visible="false" ForeColor="#333333" GridLines="Both"
                                                        Width="100%" CellPadding="0" CellSpacing="0">
                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnAddClientScopeRow" runat="server" CausesValidation="false"
                                                                        Height="18px" ImageUrl="Images/AddNewitem.jpg" OnCommand="imgBtnAddClientScopeRow_Click"
                                                                        ToolTip="Add Row" Width="18px" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="18px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnDeleteClientScopeRow" runat="server" CausesValidation="false"
                                                                        Height="16px" ImageUrl="Images/DeleteItem.png" OnCommand="imgBtnDeleteClientScopeRow_Click"
                                                                        ToolTip="Delete Row" Width="16px" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="16px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="lblSrNoClientScope" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderText="Sr.No" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" />
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Note" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_NOTEClientScope" runat="server" BorderWidth="0px" Text="" Width="99%"></asp:TextBox>
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
                                                    <%--</ContentTemplate>
                                            </asp:UpdatePanel>--%>
                                                </asp:Panel>
                                            </asp:View>
                                            <asp:View ID="View_Discount" runat="server">
                                                <asp:Panel ID="Pnl_Discount" runat="server" BorderColor="AliceBlue" BorderStyle="Ridge"
                                                    ScrollBars="Both" Width="100%">
                                                    <%--Width="940px"--%>
                                                    <%--<asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>--%>
                                                    <asp:GridView ID="grdDiscount" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin"
                                                        Width="100%">
                                                        <Columns>
                                                            <%--    <asp:TemplateField HeaderText="Sr No.">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                        </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Material Name"
                                                                ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMaterialName" runat="server" Text="" Width="150px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Test Name" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTestName" runat="server" Text="" Width="270px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Discounted Rate&lt;Br/&gt;SiteWise(%)"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtSiteWiseDisc" runat="server" BorderWidth="0px" Style="text-align: right"
                                                                        Text="" Width="60px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Generic Disc.(%)"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtVolDisc" runat="server" BorderWidth="0px" Style="text-align: right"
                                                                        Text="" Width="50px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Applied Disc. %"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtAppliedDisc" runat="server" BorderWidth="0px" Style="text-align: right"
                                                                        Text="" Width="50px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <%--</ContentTemplate>
                                            </asp:UpdatePanel>--%>
                                                </asp:Panel>
                                            </asp:View>
                                            <asp:View ID="View_GenericDiscount" runat="server">
                                                <asp:Panel ID="pnlGenericDiscount" runat="server" BorderColor="AliceBlue" BorderStyle="Ridge"
                                                    ScrollBars="Vertical" Width="100%">
                                                    <table style="width: 820px;">
                                                        <tr>
                                                            <td colspan="6">&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblVol" runat="server" Text="Volumn (%) : 0"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblTime" runat="server" Text="Timely Payment (%) : 0"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblAdv" runat="server" Text="Advance (%) : 0"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblLoy" runat="server" Text="Loyalty (%) : 0"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblProp" runat="server" Text="Proposal (%) : 0"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblApp" runat="server" Text="App. (%) : 0"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCalcDisc" runat="server" Text="Calculated (%) : 0"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblDisc" runat="server" Font-Bold="true" Text="Applied Discount(%) : 0"></asp:Label>
                                                                <asp:Label ID="lblMax" runat="server" Text="Max (%) : 0" Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblIntro" runat="server" Text="Introductory "></asp:Label>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txtIntro" runat="server" MaxLength="2" onchange="javascript:checkint(this);"
                                                                    Text="0.00" Width="100px"></asp:TextBox>
                                                                <asp:LinkButton ID="lnkUpdateIntro" runat="server" Font-Underline="true" OnClick="lnkUpdateIntro_Click">Update Introductory Discount</asp:LinkButton>
                                                                <asp:Label ID="lblClId" runat="server" Text="" Visible="false"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <%--  <tr>
                                                            <td>
                                                                <asp:Label ID="lblBillVol" runat="server" Text="Bill Volumn "></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBillVol" runat="server" MaxLength="2" onchange="javascript:checkint(this);"
                                                                    Text="0.00" Width="100px"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblAdvPayment" runat="server" Text="Adv. Payment"></asp:Label>
                                                                &nbsp;
                                                                 <asp:TextBox ID="txtAdvPayment" runat="server" MaxLength="2" onchange="javascript:checkint(this);"
                                                                    Text="0.00" Width="100px"></asp:TextBox>
                                                            </td>
                                                        </tr>--%>
                                                    </table>
                                                </asp:Panel>
                                            </asp:View>
                                        </asp:MultiView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <table style="margin-top: 10px" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblPaymentTerm" runat="server" Text="Payment term"></asp:Label>
                    </td>

                    <td colspan="4">
                        <asp:TextBox ID="txtPaymentTerm" runat="server" MaxLength="500" Width="715px"></asp:TextBox>
                        <br />
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        <asp:Label ID="Label9" runat="server" Text="Proposal By"></asp:Label>
                    </td>
                    <td width="18%">
                        <asp:DropDownList ID="ddl_PrposalBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_PrposalBy_SelectedIndexChanged"
                            Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td align="center" width="10%">
                        <asp:Label ID="Label2" runat="server" Text="Contact No"></asp:Label>
                    </td>
                    <td align="right" width="10%">
                        <asp:TextBox ID="txt_ContactNo" runat="server" MaxLength="10" Width="150px"></asp:TextBox>
                    </td>
                    <td align="right">
                        <%-- <asp:CheckBox ID="chkApplyDiscount" Text="Apply Discount" runat="server" OnCheckedChanged="chkApplyDiscount_CheckedChanged" />
                        --%>&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label16" runat="server" Text="Net Amount"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txt_NetAmount" runat="server" ReadOnly="true" Style="text-align: right"
                            Text="0.00" Width="150px"></asp:TextBox>
                        <asp:Label ID="lblGstAmt" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblGrandTotal" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        <asp:Label ID="Label11" runat="server" Text="ME Name"></asp:Label>
                    </td>
                    <td width="18%">
                        <asp:TextBox ID="txtMe" runat="server" MaxLength="250" Width="145px"></asp:TextBox>
                    </td>
                    <td align="center" width="10%">
                        <asp:Label ID="Label15" runat="server" Text="Contact No"></asp:Label>
                    </td>
                    <td align="right" width="10%">
                        <asp:TextBox ID="txtMeNum" runat="server" MaxLength="10" Width="150px"></asp:TextBox>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="LnkBtnCal" runat="server" CssClass="SimpleColor" Font-Bold="true"
                            Font-Underline="true" OnClick="lnkCal_Click">Cal</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="LnkBtnSave" runat="server" CssClass="SimpleColor" Font-Bold="true"
                            Font-Underline="true" OnClick="LnkBtnSave_Click">Save</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSendForApproval" runat="server" CssClass="SimpleColor" Font-Bold="true"
                            Font-Underline="true" OnClick="lnkSendForApproval_Click">Send for Approval</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="SimpleColor" Font-Bold="true"
                            Font-Underline="true" OnClick="lnkPrint_Click" Visible="false">Print</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="LnkExit" runat="server" Font-Bold="True" OnClick="LnkExit_Click"
                            Style="text-decoration: underline;">Exit</asp:LinkButton>
                        <%--OnClientClick="javascript:history.go(-1);return false;" --%>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Pnl_TestDetails" runat="server">
            </asp:Panel>
            <asp:HiddenField ID="HiddenField2" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" BackgroundCssClass="ModalPopupBG"
                Drag="true" PopupControlID="PnlPerticularDtls" PopupDragHandleControlID="PopupHeader"
                TargetControlID="HiddenField2">
            </asp:ModalPopupExtender>
            <asp:Panel ID="PnlPerticularDtls" runat="server" class="DetailPopup" Height="150px"
                Width="420px">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="Label24" runat="server" Font-Bold="True" ForeColor="#990033" Text="Add/Edit Particular Details"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:ImageButton ID="imgExit" runat="server" ImageAlign="Right" ImageUrl="Images/cross_icon.png"
                                        OnClick="imgExit_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    <asp:Label ID="lblFlag" runat="server" Text="0" Visible="false"></asp:Label>
                                    <asp:Label ID="lblRecType" runat="server" Text="" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 30px" width="30%">
                                    <asp:Label ID="Label19" runat="server" Text="Particular Name"></asp:Label>
                                </td>
                                <td width="50%">
                                    <asp:TextBox ID="txt_PerticularName" runat="server" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 30px" width="30%">
                                    <asp:Label ID="Label20" runat="server" Text="Unit Rate"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_UnitRate" runat="server" MaxLength="7" onchange="checkPopupNumber(this);"
                                        Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 30px" width="30%">
                                    <asp:Label ID="Label10" runat="server" Text="Discounted Rate"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_DiscountRate" runat="server" MaxLength="7" onchange="checkPopupNumber(this);"
                                        Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:LinkButton ID="lnkAddPerticular" runat="server" Font-Underline="true" OnClick="lnkAddPerticular_Click">Add Particular</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="ModalPopupBG"
                Drag="true" PopupControlID="pnlAddress" PopupDragHandleControlID="PopupHeader"
                TargetControlID="HiddenField1">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlAddress" runat="server" class="DetailPopup" Height="160px" Width="520px">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="#990033" Text="Edit Address"></asp:Label>
                                </td>
                                <td>
                                    <asp:ImageButton ID="imgExitAdd" runat="server" ImageAlign="Right" ImageUrl="Images/cross_icon.png"
                                        OnClick="imgExitAdd_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                                    <asp:Label ID="lblSiteId" runat="server" Text="0" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 30px" valign="top">
                                    <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtAdd" runat="server" MaxLength="120" TextMode="MultiLine" Width="420px"></asp:TextBox>
                                    <%-- <asp:TextBox ID="txtAdd2" Width="420px" runat="server" MaxLength="60"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 30px" valign="top">
                                    <asp:Label ID="lblCity" runat="server" Text="City"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCity" runat="server" Width="150px"></asp:TextBox>
                                </td>
                                <td valign="top">
                                    <asp:Label ID="lblPin" runat="server" Text="Pin Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPin" runat="server" MaxLength="6" onchange="checkPopupNumber(this);"
                                        Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 30px" valign="top">
                                    <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="250" Width="420px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>
                                </td>
                                <td align="right" colspan="2">
                                    <asp:LinkButton ID="lnkUpdateAdd" runat="server" Font-Underline="true" OnClick="lnkUpdateAdd_Click">Update Address</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField3" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" BackgroundCssClass="ModalPopupBG"
                Drag="true" PopupControlID="PanelDepth" PopupDragHandleControlID="PopupHeader"
                TargetControlID="HiddenField3">
            </asp:ModalPopupExtender>
            <asp:Panel ID="PanelDepth" runat="server" class="DetailPopup" Height="100px" Width="370px">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td colspan="2" align="center">
                                    <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="#990033" Text="Edit Depth Rate"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:ImageButton ID="imgExitDepth" runat="server" ImageAlign="Right" ImageUrl="Images/cross_icon.png"
                                        OnClick="imgExitDepth_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 10px;" valign="top">
                                    <asp:Label ID="lblDepth" runat="server" Text="Select Depth"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlDepth" Width="205px" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlDepth_SelectedIndexChanged">
                                        <asp:ListItem>10 m</asp:ListItem>
                                        <asp:ListItem>15 m</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 10px" valign="top">
                                    <asp:Label ID="lblRate" runat="server" Text="Rate"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtGtRate" runat="server" Width="200px" MaxLength="8" onchange="checkDecimal(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lblGtRate" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>
                                </td>
                                <td align="right" colspan="2">
                                    <asp:LinkButton ID="lnkUpdateDepthRate" runat="server" Font-Underline="true" OnClick="lnkUpdateDepthRate_Click">Update Rate</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:Panel>
    </div>
    <script type="text/javascript">
        function checkPopupNumber(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    x.value = '';
                    x.focus();
                    document.getElementById('<%=lbl_Msg.ClientID%>').style.visibility = "visible";
                    document.getElementById('<%=lbl_Msg.ClientID%>').innerHTML = "Numeric Values Allowed";
                    return false;
                }
                else {
                    document.getElementById('<%=lbl_Msg.ClientID%>').style.visibility = "hidden";
                }

            }
            return true;
        }
        function checkDecimal(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 46))) {
                    x.value = '';
                    x.focus();
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Decimal Values Allowed";
                    return false;
                }
                else {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                }

            }
            return true;
        }
        function checkint(x) {

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
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
