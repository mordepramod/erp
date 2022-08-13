<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="MixDesign_Inward.aspx.cs" Title="Mix Design Inward" Theme="duro"
    Inherits="DESPLWEB.MixDesign_Inward" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Controls/UC_InwardHeader.ascx" TagName="UC_InwardHeader" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Style="background-color: #ECF5FF;"
            ScrollBars="Auto">
            <table style="width: 100%;">
                <%--<tr style="background-color: #ECF5FF;">
                        <td>
                             <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Text="Client Details"></asp:Label>
                        </td>
                        <td colspan="3" align="right">
                             <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png"
                                    OnClick="imgClose_Click" ImageAlign="Right" />
                        </td>
                </tr>--%>
                <tr>
                    <td colspan="4">
                        <asp:Panel ID="pnlControl" runat="server" ScrollBars="Auto" Width="775px">
                            <uc1:UC_InwardHeader ID="UC_InwardHeader1" OnTextChanged="TextChanged" OnClick="Click"
                                runat="server" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lbl_MatName" runat="server" Text="Testing of Material"></asp:Label>
                        <asp:CheckBox ID="chk_MaterialName" runat="server" />
                        <asp:Label ID="lblModifyInwd" runat="server" Text="" Visible="false"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSelectRptClientSite" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" OnClick="lnkSelectRptClientSite_Click">Select Client for Report</asp:LinkButton>
                        <asp:Label ID="lblRptClientId" runat="server" Text="0" Visible="false"></asp:Label>
                        <asp:Label ID="lblRptSiteId" runat="server" Text="0" Visible="false"></asp:Label>
                    &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTestedAt" runat="server" Text="Tested At : "></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlTestedAt" runat="server" Width="145px" Height="20px">
                            <asp:ListItem Text="Lab" Value="0" />
                            <asp:ListItem Text="Prayeja City" Value="1" />
                        </asp:DropDownList>
                         &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblDescription" runat="server" Text="Description : "></asp:Label>
                         &nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtDescription" runat="server" Text="" Width="200px" MaxLength="250" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="100px" Width="100%">
                <asp:GridView ID="grdMixDesignInward" runat="server" AutoGenerateColumns="False"
                    SkinID="gridviewSkin" OnRowDataBound="grdMixDesignInward_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No." HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Grade" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddl_Grade" runat="server" BorderWidth="0px" Width="70px">
                                    <asp:ListItem Text="Select" />
                                    <asp:ListItem Text="M 5" />
                                    <asp:ListItem Text="M 7.5" />
                                    <asp:ListItem Text="M 10" />
                                    <asp:ListItem Text="M 15" />
                                    <asp:ListItem Text="M 20" />
                                    <asp:ListItem Text="M 25" />
                                    <asp:ListItem Text="M 30" />
                                    <asp:ListItem Text="M 35" />
                                    <asp:ListItem Text="M 37" />
                                    <asp:ListItem Text="M 37.5" />
                                    <asp:ListItem Text="M 40" />
                                    <asp:ListItem Text="M 45" />
                                    <asp:ListItem Text="M 50" />
                                    <asp:ListItem Text="M 55" />
                                    <asp:ListItem Text="M 60" />
                                    <asp:ListItem Text="M 65" />
                                    <asp:ListItem Text="M 70" />
                                    <asp:ListItem Text="M 75" />
                                    <asp:ListItem Text="M 80" />
                                    <asp:ListItem Text="M 85" />
                                    <asp:ListItem Text="M 90" />
                                    <asp:ListItem Text="M 95" />
                                    <asp:ListItem Text="M 100" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Slump" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Slump" BorderWidth="0px" runat="server" Text="" Width="100px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type of Design" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddl_TestName" BorderWidth="0px" Width="250px" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nature of Work" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_NatureofWork" BorderWidth="0px" runat="server" Width="240px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Any Special Requirement" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_AnySpecialRequirement" BorderWidth="0px" runat="server" Width="200px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="Panel1" runat="server" Height="100px" ScrollBars="Auto" BorderStyle="Ridge"
                BorderColor="AliceBlue" Width="800px">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="Grd_Material" runat="server" AutoGenerateColumns="False" OnRowDataBound="Grd_Material_RowDataBound"
                            SkinID="gridviewSkin" Width="800px">
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
                                <asp:BoundField DataField="lblSrNo" HeaderStyle-HorizontalAlign="Center" HeaderText="Sr.No."
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                    HeaderText="Material List">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddl_Material" runat="server" BorderWidth="0px" Width="150px">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"
                                    HeaderText="Qty">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Qty" runat="server" BorderWidth="0px" MaxLength="2" Style="text-align: center"
                                            onchange="checkNum(this)" Width="50px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Information">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Information" MaxLength="250" runat="server" BorderWidth="0px"
                                            Width="300px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="1" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SrNo" Width="20px" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="2" ItemStyle-HorizontalAlign="Center"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SrNo1" Width="20px" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="3" ItemStyle-HorizontalAlign="Center"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SrNo2" Width="20px" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="4" ItemStyle-HorizontalAlign="Center"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SrNo3" Width="20px" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="5" ItemStyle-HorizontalAlign="Center"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SrNo4" Width="20px" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="6" ItemStyle-HorizontalAlign="Center"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SrNo5" Width="20px" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="7" ItemStyle-HorizontalAlign="Center"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SrNo6" Width="20px" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="8" ItemStyle-HorizontalAlign="Center"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SrNo7" Width="20px" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="9" ItemStyle-HorizontalAlign="Center"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SrNo8" Width="20px" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="10" ItemStyle-HorizontalAlign="Center"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SrNo9" Width="20px" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <table width="100%">
                <tr style="background-color: #ECF5FF;">
                    <td align="right" colspan="3">
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkPrint" runat="server" CssClass="LnkOver" OnClick="lnkPrint_Click"
                            Style="text-decoration: underline;" ValidationGroup="V1" Visible="false">Print Inward</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkLabSheet" runat="server" CssClass="LnkOver" OnClick="lnkLabSheet_Click"
                            Style="text-decoration: underline;" ValidationGroup="V1" Visible="false">Print LabSheet</asp:LinkButton>
                        &nbsp;&nbsp;
                        <%--<asp:LinkButton ID="lnkBillPrint" runat="server" CssClass="LnkOver" OnClick="lnkBillPrint_Click"
                            Style="text-decoration: underline;" ValidationGroup="V1" Visible="false">Print Proforma Invoice</asp:LinkButton>--%>
                        <asp:LinkButton ID="lnkBillPrint" runat="server" CssClass="LnkOver" OnClick="lnkBillPrint_Click"
                            Style="text-decoration: underline;" ValidationGroup="V1" Visible="false">Print Tax Invoice</asp:LinkButton>
                        &nbsp; &nbsp;
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" Font-Bold="True" OnClick="lnkSave_Click"
                            Style="text-decoration: underline;" ValidationGroup="V1">Save</asp:LinkButton>
                        &nbsp; &nbsp;
                        <asp:LinkButton ID="lnkExit" runat="server" CssClass="LnkOver" Font-Bold="True" OnClick="lnk_Exit_Click"
                            Style="text-decoration: underline;">Exit</asp:LinkButton>
                        &nbsp; &nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="ModalPopupBG"
                Drag="true" PopupControlID="Panel3" PopupDragHandleControlID="PopupHeader" TargetControlID="HiddenField1">
            </asp:ModalPopupExtender>
            <asp:Panel ID="Panel3" runat="server" class="DetailPopup" Height="300px" ScrollBars="Auto"
                Width="200px">
                <div>
                    <asp:UpdatePanel ID="up2" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td align="center" colspan="2" valign="bottom">
                                        <asp:ImageButton ID="imgCloseSrNo" runat="server" Height="18px" ImageAlign="Right"
                                            ImageUrl="Images/T.jpeg" OnClick="imgCloseSrNo_Click" Width="18px" />
                                        <asp:Label ID="lblpopuphead" runat="server" Font-Bold="True" ForeColor="#990033"
                                            Text="Use for design SrNo."></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:ImageButton ID="ImgExit_" runat="server" ImageAlign="Right" ImageUrl="Images/cross_icon.png"
                                            OnClick="Img_Exit_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:CheckBoxList ID="chk_SrNo" runat="server" BorderColor="Crimson" BorderWidth="1"
                                            TextAlign="Right">
                                        </asp:CheckBoxList>
                                    </td>
                                    <td align="right">
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
        </asp:Panel>
           <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    <%--<asp:UpdateProgress ID="UpdWaitImage" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="modal1">
                <div class="center1">
                    <asp:Image ID="imgProgress" ImageUrl="Images/ProgressImage.gif" runat="server" />
                    Please Wait...
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
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
