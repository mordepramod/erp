<%@ Page Title="GGBS Test Report" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="Ggbs_Report.aspx.cs" Inherits="DESPLWEB.Ggbs_Report" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <div align="right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                    ImageAlign="Right" />
            </div>
            <asp:Panel ID="Panel2" runat="server" BorderStyle="Ridge" Width="942px" BorderColor="AliceBlue">
                <table style="width: 100%">
                    <tr>
                        <td width="15%">
                            <asp:Label ID="lbl_OtherPending" runat="server" Text="Other Pending Reports"></asp:Label>
                        </td>
                        <td width="30%">
                            <asp:DropDownList ID="ddl_OtherPendingRpt" Width="205px" runat="server">
                            </asp:DropDownList>
                            <asp:LinkButton ID="lnkFetch" runat="server" Style="text-decoration: underline;"
                                Font-Bold="true" OnClick="lnkFetch_Click">Fetch</asp:LinkButton>
                        </td>
                        <td width="15%" align="right">
                            <asp:Label ID="Label9" runat="server" Text="Report No"></asp:Label>
                        </td>
                        <td width="31%" align="center">
                            <asp:TextBox ID="txt_RecType" Width="50px" ReadOnly="true" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txt_ReportNo" runat="server" ReadOnly="true" Width="140px"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblRecordNo" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblEntry" runat="server" Text="Enter" Visible="false"></asp:Label>
                            <asp:Label ID="lblCubeCompStr" runat="server" Text="Enter" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Reference No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_ReferenceNo" Width="200px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="Label5" runat="server" Text="Received Date"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txt_ReceivedDate" Width="200px" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ReceivedDate">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_ReceivedDate"
                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                            </asp:MaskedEditExtender>
                        </td>                      
                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="GGBS Used"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_GgbsUsed" Width="200px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label2" runat="server" Text="Date Of Testing"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txt_DateOfTesting" Width="200px" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_DateOfTesting">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_DateOfTesting"
                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                            </asp:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Cement Used"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_CementUsed" Width="200px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label10" runat="server" Text="Supplier "></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txt_Supplier" Width="200px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Description" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label12" runat="server" Text="NABL Scope" Font-Bold="true"></asp:Label>
                        </td>
                        <td align="center" colspan="2">
                              <asp:DropDownList ID="ddl_NablScope" AutoPostBack="true" Width="80px" runat="server">
                                <asp:ListItem Text="--Select--" />
                                <asp:ListItem Text="F" />
                                <asp:ListItem Text="NA" />
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                            <asp:Label ID="Label13" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>
                            <asp:DropDownList ID="ddl_NABLLocation" runat="server" Width="80px" Enabled="true">
                            <asp:ListItem Value="0" Text="0" />
                                <asp:ListItem Value="1" Text="1" />
                                <asp:ListItem Value="2" Text="2" />
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="lnkGetData" OnClick="lnkGetAppData_Click" runat="server" Font-Bold="True"
                                Style="text-decoration: underline;">Get App Data</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 1px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="200px" BorderStyle="Ridge"
                Width="940px" BorderColor="AliceBlue">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdGgbsReport" runat="server" AutoGenerateColumns="false" SkinID="gridviewSkin">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr No.">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name Of the Test" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_NameOfTest" BorderWidth="0px" Width="580px" runat="server" ReadOnly="true"></asp:TextBox>
                                        <asp:LinkButton ID="lnkCementStrength" Font-Bold="True" Style="text-decoration: underline;"
                                            runat="server" OnCommand="lnkCementStrength_Click"> </asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="lnkStrength" Font-Bold="True" Style="text-decoration: underline;"
                                            runat="server" OnCommand="lnkStrength_Click"> </asp:LinkButton>
                                        <asp:HiddenField runat="server" Id="ggbsStrength" />
                                        <asp:HiddenField runat="server" Id="cementStrength" />
                                        <asp:HiddenField runat="server" Id="CubeCastingStatus" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Result" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="btnResult" Visible="false" OnCommand="btnResult_Click" runat="server"
                                            Text="" Width="158px" Style="border-style: none;" BackColor="White" />
                                        <asp:TextBox ID="txt_result" BorderWidth="0px" Style="text-align: center" runat="server"
                                            Width="155px" MaxLength="50" onchange="javascript:chkresult(this);" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_TestDetails" runat="server" Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Status" runat="server" Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_TestId" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <div style="height: 1px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" BorderColor="AliceBlue" BorderStyle="Ridge"
                Height="85px" Width="942px">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <asp:GridView ID="grdGgbsRemark" runat="server" SkinID="gridviewSkin">
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <table width="100%">
                <tr height="30px">
                    <td width="12%">
                        <asp:CheckBox ID="chkWitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chkWitnessBy_CheckedChanged" />
                        <asp:Label ID="Label7" runat="server" Text="Witness By"></asp:Label>
                    </td>
                    <td width="22%">
                        <asp:TextBox ID="txt_witnessBy" Width="200px" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td width="25%" style="text-align: right">
                        <asp:Label ID="lbl_TestedBy" runat="server" Text="Tested By"></asp:Label>
                    </td>
                    <td>&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                        <asp:DropDownList ID="ddl_TestedBy" Width="205px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" OnClientClick="return All();"
                            Font-Bold="True" Style="text-decoration: underline;">Save</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" Visible="false" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>
                        <asp:LinkButton ID="lnkExit" OnClick="lnkExit_Click" Font-Bold="True" Style="text-decoration: underline;"
                            runat="server">Exit</asp:LinkButton>
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
                <asp:UpdatePanel runat="server" ID="up2">
                    <ContentTemplate>
                        <table class="DetailPopup" width="350px">
                            <tr>
                                <td align="right">
                                    <asp:CheckBox ID="chk_Awaited" runat="server" Checked="false" />
                                    <asp:Label ID="Label8" runat="server" Text="Awaited"></asp:Label>
                                </td>
                                <td align="left" valign="top">
                                    <asp:ImageButton ID="imgCloseTestDetails" runat="server" ImageUrl="Images/T.jpeg"
                                        ImageAlign="Right" Width="18px" Height="18px" OnClick="imgCloseTestDetails_Click" />
                                    &nbsp; &nbsp; &nbsp;
                                </td>
                                <td style="width: 5%" valign="top">
                                    <asp:ImageButton ID="ImgExit" runat="server" OnClientClick="HideModalPopup()"
                                        ImageUrl="Images/cross_icon.png" ImageAlign="Right" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Panel ID="Panel5" runat="server" ScrollBars="Auto" Height="150px" Width="400px">                                        
                                        <div id="divDensity" runat="server" visible="false">
                                            <fieldset style="border-color: #008080; width: 94%; height: 130px;">
                                                <legend>Density</legend>
                                                <table width="100%">
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_Weight" runat="server" Text="Weight"></asp:Label>
                                                        </td>
                                                        <td>&nbsp;&nbsp;
                                                            <asp:TextBox ID="txt_Weight" Width="100px" Style="text-align: right" onkeyup="checkNumber(this);"
                                                                runat="server" MaxLength="4"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_Volume" runat="server" Text="Volume"></asp:Label>
                                                        </td>
                                                        <td>&nbsp;&nbsp;
                                                            <asp:TextBox ID="txt_Volume" Width="100px" Style="text-align: right" runat="server"
                                                                MaxLength="5" onchange="CheckValue(this);"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>
                                        <div id="divSndnesautoclave" runat="server" visible="false">
                                            <fieldset style="border-color: #008080; width: 94%; height: 130px;">
                                                <legend>Soundness By AutoClave</legend>
                                                <table width="100%">
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_IR" runat="server" Text="IR"></asp:Label>
                                                        </td>
                                                        <td>&nbsp;&nbsp;
                                                            <asp:TextBox ID="txt_IR" Width="100px" Style="text-align: right" onchange="javascript:Checktxt(this);"
                                                                MaxLength="5" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_FR" runat="server" Text="FR"></asp:Label>
                                                        </td>
                                                        <td>&nbsp;&nbsp;
                                                            <asp:TextBox ID="txt_FR" Width="100px" Style="text-align: right" onchange="javascript:Checktxt(this);"
                                                                runat="server" MaxLength="5"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>
                                        <div id="divIni_finSet" runat="server" visible="false">
                                            <fieldset style="border-color: #008080; width: 94%; height: 130px;">
                                                <legend>Initial Setting & Final Setting </legend>
                                                <table width="100%">
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_WaterAddded" runat="server" Text="Water Added" Width="100px"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_WaterAdded" Width="60px" runat="server"></asp:TextBox>
                                                            <asp:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txt_WaterAdded"
                                                                MaskType="time" Mask="99:99" AutoComplete="true" AcceptAMPM="true" runat="server">
                                                            </asp:MaskedEditExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_IST" runat="server" Text="Initial Setting Time"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_IST" Width="60px" runat="server"></asp:TextBox>
                                                            <asp:MaskedEditExtender ID="MaskedEditExtender4" TargetControlID="txt_IST" MaskType="time"
                                                                Mask="99:99" AutoComplete="true" AcceptAMPM="true" runat="server">
                                                            </asp:MaskedEditExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_FST" runat="server" Text="Final Setting Time"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_FST" Width="60px" runat="server"></asp:TextBox>
                                                            <asp:MaskedEditExtender ID="MaskedEditExtender5" TargetControlID="txt_FST" MaskType="time"
                                                                Mask="99:99" AutoComplete="true" AcceptAMPM="true" runat="server">
                                                            </asp:MaskedEditExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>
                                        <div id="divSndnsByLe_Chateliers" runat="server" visible="false">
                                            <fieldset style="border-color: #008080; width: 94%; height: 130px;">
                                                <legend>Soundness By Le-Chateliers Apparatus </legend>
                                                <table width="100%">
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="center">
                                                            <asp:GridView ID="grdIRFR" runat="server" AutoGenerateColumns="false" SkinID="gridviewSkin">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="IR" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txt_IRgrid" BorderWidth="0px" onchange="javascript:Checktxt(this);"
                                                                                Width="170px" runat="server" Style="text-align: right" MaxLength="12"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="FR" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txt_FRgrid" BorderWidth="0px" onchange="javascript:Checktxt(this);"
                                                                                Style="text-align: right" runat="server" Width="170px" MaxLength="12"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>                                            
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>               
            </asp:Panel>
        </asp:Panel>
    </div>
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

        function CheckValue(inputtxt) {
            var numbers = /^\d+(\.\d{1,2})?$/;
            if (inputtxt.value.match(numbers)) {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                return true;
            }
            else {

                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please enter valid integer or decimal number with 2 decimal places";
                inputtxt.focus();
                inputtxt.value = "";
                return false;
            }
        }
        
        function Checktxt(inputtxt) {
            var numbers = /^\d+(\.\d{1,2})?$/;
            if (inputtxt.value.match(numbers)) {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                return true;
            }
            else {

                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please enter valid integer or decimal number with 2 decimal places";
                inputtxt.focus();
                inputtxt.value = "";
                return false;
            }
        }

        function chkresult(inputtxt) {
            var numbers = /^\d+(\.\d{1,2})?$/;
            if (inputtxt.value == 'Awaited') {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                return true;
            }
            if (inputtxt.value == 'NIL') {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                return true;
            }
            if (inputtxt.value == 'Nil') {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                return true;
            }
            if (inputtxt.value.match(numbers)) {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                return true;
            }
            else {

                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please enter valid integer or decimal number with 2 decimal places";
                inputtxt.focus();
                inputtxt.value = "";
                return false;
            }
        }

        function HideModalPopup() {
            $find("ModalPopupBehaviorID").hide();
        }

        function SetTarget() {
            document.forms[0].target = "_blank";
        }

    </script>
</asp:Content>

