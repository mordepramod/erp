<%@ Page Title="Core Test Report" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="Core_Report.aspx.cs" Inherits="DESPLWEB.Core_Report"
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
                            <asp:TextBox ID="txt_RecType" Width="50px" ReadOnly="true" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txt_ReportNo" runat="server" ReadOnly="true" Width="140px"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblRecordNo" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblEntry" runat="server" Text="Enter" Visible="false"></asp:Label>
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
                            <%--<asp:Label ID="lbl_DtOfCasting" runat="server" Text="Date Of Casting"></asp:Label>--%>
                            <asp:Label ID="Label4" runat="server" Text="Date of Spec. Extraction"></asp:Label>
                        </td>
                        <td width="25%" align="center">
                            <%-- <asp:TextBox ID="txt_DtOfCasting" Width="200px" runat="server"></asp:TextBox>--%>
                            <asp:TextBox ID="txt_DtSpecExtraction" Width="200px" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_DtSpecExtraction">
                            </asp:CalendarExtender>
                            <%--<asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_DtSpecExtraction"
                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                            </asp:MaskedEditExtender>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Grade Of Concrete"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_GradeOfConcrete" ReadOnly="false" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td width="15%" align="right">
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
                            <asp:Label ID="Label5" runat="server" Text="Concrete Member"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_ConcreteMember" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td width="10%" align="right">
                            <asp:Label ID="Label9" runat="server" Text="Curring Condition"></asp:Label>
                        </td>
                        <td width="25%" align="center">
                            <asp:DropDownList ID="ddl_CurrCondition" runat="server" Width="205px">
                                <asp:ListItem Text="Select" />
                                <asp:ListItem Text="Saturated surface Dry" />
                                <asp:ListItem Text="Dry" />
                                <asp:ListItem Text="Wet" />
                                <asp:ListItem Text="Shear" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_desc" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Description" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td width="10%" align="right"></td>
                        <td width="25%" align="left">
                            <asp:CheckBox ID="chkCylinder" Width="100px" runat="server" Text="Cylinder"></asp:CheckBox>
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
                    <asp:Label ID="Label8" runat="server" Text="NABL Scope" Font-Bold="true"></asp:Label>

                    <asp:DropDownList ID="ddl_NablScope" AutoPostBack="true" Width="80px" runat="server">
                        <asp:ListItem Text="--Select--" />
                        <asp:ListItem Text="F" />
                        <asp:ListItem Text="NA" />
                    </asp:DropDownList>
                    &nbsp;&nbsp;  &nbsp;&nbsp;    &nbsp;&nbsp;
                                    <asp:Label ID="Label11" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="ddl_NABLLocation" runat="server" Width="80px" Enabled="true">
                         <asp:ListItem Value="0" Text="0" />
                                        <asp:ListItem Value="1" Text="1" />
                                        <asp:ListItem Value="2" Text="2" />
                    </asp:DropDownList>
                    <br />
                </div>
                <asp:GridView ID="grdCoreTestInward" runat="server" SkinID="gridviewSkin" Width="100%"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnCoreTestAddRow" runat="server" OnCommand="imgBtnCoreTestAddRow_Click"
                                    ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                    ToolTip="Add Row" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnCoreTestDeleteRow" runat="server" OnCommand="imgBtnCoreTestDeleteRow_Click"
                                    ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                    ToolTip="Delete Row" />
                            </ItemTemplate>
                            <ItemStyle Width="16px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnMergeRow" runat="server" OnCommand="imgBtnMergeRow_Click"
                                    ImageUrl="Images/m5.jpg" Height="16px" Width="16px" CausesValidation="false"
                                    ToolTip="Merge Row" />
                            </ItemTemplate>
                            <ItemStyle Width="16px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Description" BorderWidth="0px" Width="100px" MaxLength="250"
                                    runat="server" Text='' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Grade" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddl_Grade" BorderWidth="0px" runat="server" Width="65px">
                                    <asp:ListItem Text="Select" />
                                    <asp:ListItem Text="M 10" />
                                    <asp:ListItem Text="M 15" />
                                    <asp:ListItem Text="M 20" />
                                    <asp:ListItem Text="M 25" />
                                    <asp:ListItem Text="M 30" />
                                    <asp:ListItem Text="M 35" />
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
                                    <asp:ListItem Text="NA" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Casting Date" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_CastingDt" BorderWidth="0px" MaxLength="10" runat="server" Text=''
                                    Width="80px" onblur="CastingDt(this)" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dia" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Dia" BorderWidth="0px" CssClass="textbox" runat="server" Text=''
                                    Width="60px" onchange="checkDecimal(this);" />
                                <%--checkNumber--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Length" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_length" BorderWidth="0px" CssClass="textbox" runat="server"
                                    Text='' Width="80px" onchange="checkDecimal(this);" />
                                <%--checkNumber--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Length with Caping" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_lengthwthCaping" BorderWidth="0px" CssClass="textbox" runat="server"
                                    Text='' Width="80px" onchange="checkDecimal(this);" />
                                <%--checkNumber--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Weight" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_weight" BorderWidth="0px" CssClass="textbox" runat="server"
                                    Text='' Width="80px" onchange="CheckWeight(this);" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reading" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Reading" BorderWidth="0px" CssClass="textbox" runat="server"
                                    Text='' Width="80px"  onchange="CheckReading(this)" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pulse Vel" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_PulseVel" BorderWidth="0px" CssClass="textbox" runat="server"
                                    Text='' Width="80px" onchange="CheckPulseVel(this);" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mode Of Failure" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddl_ModeOfFailure" BorderWidth="0px" runat="server" Width="65px">
                                    <asp:ListItem Text="Select" />
                                    <asp:ListItem Text="Satisfactory" />
                                    <asp:ListItem Text="Not Satisfactory" />
                                    <asp:ListItem Text="Mortar" />
                                    <asp:ListItem Text="Aggregate" />
                                    <asp:ListItem Text="Shear" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Age" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_age" BorderWidth="0px" CssClass="ctextbox" ReadOnly="true" runat="server"
                                    Text='' Width="40px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="C/s Area" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_CsArea" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                    runat="server" Text='' Width="80px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Density" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Density" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                    runat="server" Text='' Width="80px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comp Str" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_CompStr" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                    runat="server" Text='' Width="80px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Corr Comp Str" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_CorrCompStr" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                    runat="server" Text='' Width="80px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Equ Cube Str" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_EquCubeStr" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                    runat="server" Text='' Width="80px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
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
            <table width="100%">
                <tr style="height: 30px">
                    <td width="18%">
                        <asp:CheckBox ID="chk_WitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chk_WitnessBy_CheckedChanged" />
                        <asp:Label ID="Label7" runat="server" Text="Witness By"></asp:Label>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txt_witnessBy" Width="200px" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td align="left">
                        <asp:CheckBox ID="Chk_PrntPulseVel" runat="server" />
                        <asp:Label ID="Label6" runat="server" Text="Print Pulse Velocity"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_TestedBy" runat="server" Text="Tested By"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_TestedBy" Width="205px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="Lnk_Calculate" OnClick="Lnk_Calculate_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Cal</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" Visible="false" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClick="lnk_Exit_Click" runat="server">Exit</asp:LinkButton>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>
        </asp:Panel>
        <script type="text/javascript">
            function CastingDt(dateFmt) {
                var re = /^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d+$/;
                var res = dateFmt.value.toUpperCase();
                if (dateFmt.value != '') {
                    if (res == 'NA') {
                        return true;
                    }
                    if (re.test(dateFmt.value)) {
                        birthdayDate = new Date(dateFmt.value);
                        dateNow = new Date();
                        var years = dateNow.getFullYear() - birthdayDate.getFullYear();
                        var months = dateNow.getMonth() - birthdayDate.getMonth();
                        var days = dateNow.getDate() - birthdayDate.getDate();
                        if (isNaN(years)) {

                            document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Input date is incorrect";
                return false;
            }
            else {
                document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                if (months < 0 || (months == 0 && days < 0)) {
                    years = parseInt(years) - 1;

                }
                else {

                }
            }
        }
        else {

            document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
         document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Date must be dd/MM/yyyy format";
         dateFmt.value = "";
         dateFmt.focus();
         return false;
     }
 }
}

function CheckReading(inputtxt) {
    var numbers = /^\d+(\.\d{1,2})?$/;
    if (inputtxt.value.match(numbers)) {
        document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                    return true;

                }
                else {

                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please enter valid integer or decimal number with 1 decimal places";
                    inputtxt.focus();
                    inputtxt.value = "";
                    return false;
                }
            }
            function CheckWeight(inputtxt) {
                var numbers = /^\d+(\.\d{1,3})?$/;
                if (inputtxt.value.match(numbers)) {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                    return true;

                }
                else {

                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please enter valid integer or decimal number with 3 decimal places";
                    inputtxt.focus();
                    inputtxt.value = "";
                    return false;
                }
            }
            function CheckPulseVel(inputtxt) {
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
            function checkDecimal(x) {

                var s_len = x.value.length;
                var s_charcode = 0;
                for (var s_i = 0; s_i < s_len; s_i++) {
                    s_charcode = x.value.charCodeAt(s_i);
                    if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 46))) {
                        x.value = '';
                        x.focus();
                        alert("Only Numeric Values Allowed");

                        return false;
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
    </div>
</asp:Content>
