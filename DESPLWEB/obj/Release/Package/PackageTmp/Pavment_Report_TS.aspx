﻿<%@ Page Title="Pavement Tensile Strength" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="Pavment_Report_TS.aspx.cs" Inherits="DESPLWEB.Pavement_Report_TS"
    Theme="duro" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <div style="height: 1px" align="right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                    ImageAlign="Right" />
            </div>
            <asp:Panel ID="Panel2" runat="server" BorderStyle="Ridge" Width="942px" BorderColor="AliceBlue">
                <table style="width: 100%">
                    <tr style="background-color: #ECF5FF;">
                        <td>
                            <asp:Label ID="lbl_OtherPending" runat="server" Text="Other Pending Reports"></asp:Label>
                        </td>
                        <td width="30%">
                            <asp:DropDownList ID="ddl_OtherPendingRpt" Width="205px" runat="server">
                            </asp:DropDownList>
                            <asp:LinkButton ID="lnk_Fetch" runat="server" Style="text-decoration: underline;"
                                Font-Bold="true" OnClick="lnk_Fetch_Click">Fetch</asp:LinkButton>
                        </td>
                        <td width="10%" align="right">
                            <asp:Label ID="Label10" runat="server" Text="Report No"></asp:Label>
                        </td>
                        <td width="20%" align="center">
                            <asp:TextBox ID="txt_RecType" ReadOnly="true" Width="50px" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txt_ReportNo" ReadOnly="true" runat="server" Width="140px"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblEntry" runat="server" Text="Enter" Visible="false"></asp:Label>
                            <asp:Label ID="lblRecordNo" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td width="18%">
                            <asp:Label ID="Label1" runat="server" Text="Reference No"></asp:Label>
                        </td>
                        <td width="20%">
                            <asp:TextBox ID="txt_ReferenceNo" Width="200px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td width="10%" align="right">
                            <asp:Label ID="lbl_DtOfCasting" runat="server" Text="Date Of Casting"></asp:Label>
                        </td>
                        <td width="25%" align="center">
                            <asp:TextBox ID="txt_DtOfCasting" Width="200px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Collection Date"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_CollectionDt" Width="200px" ReadOnly="true" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_CollectionDt">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_CollectionDt"
                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                            </asp:MaskedEditExtender>
                        </td>
                        <td width="10%" align="right">
                            <asp:Label ID="Label2" runat="server" Text="Testing Date"></asp:Label>
                        </td>
                        <td width="25%" align="center">
                            <asp:TextBox ID="txt_TestingDt" Width="200px" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_TestingDt">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_TestingDt"
                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                            </asp:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_desc" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Description" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td width="15%" align="right">
                            <asp:Label ID="Label3" runat="server" Text="Grade Of Concrete"></asp:Label>
                        </td>
                        <td width="25%" align="center">
                            <asp:TextBox ID="txt_GradeOfConcrete" ReadOnly="true" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td width ="10%">
                            <asp:LinkButton ID="lnkGetData" OnClick="lnkGetAppData_Click" runat="server" Font-Bold="True"
                                Style="text-decoration: underline;">Get App Data</asp:LinkButton>&nbsp;
                        </td>
                    
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Supplier Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_SupplierName" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td width="15%" align="right">
                            <asp:Label ID="Label6" runat="server" Text="Nature Of Work"></asp:Label>
                        </td>
                        <td width="25%" align="center">
                            <asp:TextBox ID="txt_NatureofWork" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Qty"></asp:Label>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Qty" CssClass="textbox" MaxLength="2" Width="40px" onchange="checkNumber(this);"
                                AutoPostBack="true" OnTextChanged="txt_QtyOnTextChanged" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txt_TestId" Visible="false" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="150px" Width="942px"
                BorderStyle="Ridge" BorderColor="AliceBlue">
                <div>
                    <asp:Label ID="Label13" runat="server" Text="NABL Scope" Font-Bold="true"></asp:Label>

                    <asp:DropDownList ID="ddl_NablScope" AutoPostBack="true" Width="80px" runat="server">
                        <asp:ListItem Text="--Select--" />
                        <asp:ListItem Text="F" />
                        <asp:ListItem Text="NA" />
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label14" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="ddl_NABLLocation" runat="server" Width="80px" Enabled="true">
                        <asp:ListItem Value="0" Text="0" />
                        <asp:ListItem Value="1" Text="1" />
                        <asp:ListItem Value="2" Text="2" />
                    </asp:DropDownList>

                </div>
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <asp:GridView ID="grdPT_TSInward" runat="server" SkinID="gridviewSkin" Width="100%"
                            AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr No.">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID Mark" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_IdMark" BorderWidth="0px" Width="100px" runat="server" Text='' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Block Type" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddl_BlockType" BorderWidth="0px" runat="server" Width="80px">
                                            <asp:ListItem Text="Plain" />
                                            <asp:ListItem Text="Chamfered" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Thickness" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddl_Thickness" BorderWidth="0px" runat="server" Width="65px">
                                            <asp:ListItem Text="40 mm" />
                                            <asp:ListItem Text="50 mm" />
                                            <asp:ListItem Text="60 mm" />
                                            <asp:ListItem Text="70 mm" />
                                            <asp:ListItem Text="80 mm" />
                                            <asp:ListItem Text="90 mm" />
                                            <asp:ListItem Text="100 mm" />
                                            <asp:ListItem Text="110 mm" />
                                            <asp:ListItem Text="120 mm" />
                                            <asp:ListItem Text="130 mm" />
                                            <asp:ListItem Text="140 mm" />
                                            <asp:ListItem Text="150 mm" />
                                            <asp:ListItem Text="160 mm" />
                                            <asp:ListItem Text="170 mm" />
                                            <asp:ListItem Text="180 mm" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Failure Thickness" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="FailureThickness" BackColor="White" OnCommand="FailureThickness_Click"
                                            BorderWidth="0px" Width="60px" ForeColor="Black" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Failure Load" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_FailureLoad" CssClass="textbox" BorderWidth="0px" Width="80px"
                                            ForeColor="Black" runat="server" onchange="checkDecimal(this);" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Failure Length" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="FailureLength" BorderWidth="0px" CssClass="textbox" MaxLength="10"
                                            runat="server" Text='' Width="75px" BackColor="White" OnCommand="FailureLength_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Corr Factor" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_CorectionFactor" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                            runat="server" Text='' Width="50px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Age" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Age" BorderWidth="0px" CssClass="ctextbox" Visible="false" ReadOnly="true"
                                            runat="server" Text='' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Area of Failure" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_AreaOfFailure" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                            runat="server" Text='' Width="100px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Failure Load/ Unit Len" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_FaliureLoadPerLen" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                            runat="server" Text='' Width="100px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tensile Strength" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_TensileStrength" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                            runat="server" Text='' Width="85px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Avg Str" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_AvgStr" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                            runat="server" Text='' Width="85" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_ActualresultDtls" Visible="false" ReadOnly="true" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_FaliureLenDtls" Visible="false" ReadOnly="true" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="130px" Width="942px"
                BorderStyle="Ridge" BorderColor="AliceBlue">
                <asp:GridView ID="grdRemark" runat="server" SkinID="gridviewSkin">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnAddRow" runat="server" OnCommand="imgBtnAddRow_Click"
                                    ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                    ToolTip="Add Row" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnDeleteRow" runat="server" OnCommand="imgBtnDeleteRow_Click"
                                    ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                    ToolTip="Delete Row" />
                            </ItemTemplate>
                            <ItemStyle Width="16px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Remark" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_REMARK" BorderWidth="0px" Width="850px" runat="server" Text='' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <table width="99%">
                <tr style="height: 30px">
                    <td width="10%">
                        <asp:CheckBox ID="chk_WitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chk_WitnessBy_CheckedChanged" />
                        <asp:Label ID="Label7" runat="server" Text="Witness By"></asp:Label>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txt_witnessBy" Width="200px" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td width="10%" style="text-align: right">
                        <asp:Label ID="lbl_TestedBy" runat="server" Text="Tested By"></asp:Label>
                    </td>
                    <td width="20%" align="center">
                        <asp:DropDownList ID="ddl_TestedBy" Width="205px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td width="25%" align="right">
                        <asp:LinkButton ID="Lnk_Calculate" OnClick="Lnk_Calculate_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Cal</asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" Visible="false" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClick="lnk_Exit_Click" runat="server">Exit</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BehaviorID="ModalPopupBehaviorID"
                TargetControlID="HiddenField1" PopupControlID="Panel4" PopupDragHandleControlID="PopupHeader"
                Drag="true" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="Panel4" runat="server">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                    <ContentTemplate>
                        <table class="DetailPopup" width="200px">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_Msg" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                                </td>
                                <td align="right" valign="top">
                                    <asp:ImageButton ID="ImgExit" runat="server" OnClientClick="HideModalPopup()" ImageUrl="Images/cross_icon.png"
                                        ImageAlign="Right" />
                                    &nbsp; &nbsp;
                                    <asp:ImageButton ID="imgCloseTestDetails" runat="server" ImageUrl="Images/T.jpeg"
                                        ImageAlign="Right" Width="18px" Height="18px" OnClick="imgCloseTestDetails_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="divFailureThickness" runat="server" visible="false">
                                        <fieldset style="border-color: #008080; width: 89%; height: 130px;">
                                            <legend>Failure Thickness</legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_T1" runat="server" Text="T1"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_T1" Width="120px" Style="text-align: right" onkeyup="checkPopupNumber(this);"
                                                            runat="server" MaxLength="2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_T2" runat="server" Text="T2"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_T2" Width="120px" Style="text-align: right" runat="server" MaxLength="2"
                                                            onchange="checkPopupNumber(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_T3" runat="server" Text="T3"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_T3" Width="120px" Style="text-align: right" runat="server" MaxLength="2"
                                                            onchange="checkPopupNumber(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                    <div id="divLen" runat="server" visible="false">
                                        <fieldset style="border-color: #008080; width: 89%; height: 130px;">
                                            <legend>Failure Length</legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label9" runat="server" Text="L1"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_L1" Width="120px" Style="text-align: right" onkeyup="checkPopupNumber(this);"
                                                            runat="server" MaxLength="3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label11" runat="server" Text="L2"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_L2" Width="120px" Style="text-align: right" runat="server" MaxLength="3"
                                                            onchange="checkPopupNumber(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label12" runat="server" Text="L3"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_L3" Width="120px" Style="text-align: right" runat="server" MaxLength="3"
                                                            onchange="checkPopupNumber(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:Panel>
        <style type="text/css">
            .textbox {
                display: inline;
                text-align: right;
            }

            .caltextbox {
                display: inline;
                text-align: right;
                background-color: ActiveBorder;
            }

            .ctextbox {
                display: inline;
                text-align: center;
            }
        </style>
        <script type="text/javascript">

            function checkNumber(x) {
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
            function HideModalPopup() {
                $find("ModalPopupBehaviorID").hide();
            }
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
        </script>
    </div>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
