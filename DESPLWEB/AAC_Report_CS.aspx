<%@ Page Title="AAC Compressive Test" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="AAC_Report_CS.aspx.cs" Inherits="DESPLWEB.AAC_Report_CS" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <div style="height: 1px" align="right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                    ImageAlign="Right" />
            </div>
            <asp:Panel ID="Panel2" runat="server" BorderStyle="Ridge" Width="942px" BorderColor="AliceBlue" Height="110px" ScrollBars="Auto">
                <table style="width: 100%">
                    <tr style="background-color: #ECF5FF;">
                        <td width="15%">
                            <asp:Label ID="lbl_OtherPending" runat="server" Text="Other Pending Reports"></asp:Label>
                        </td>
                        <td width="22%">
                            <asp:DropDownList ID="ddl_OtherPendingRpt" Width="205px" runat="server">
                            </asp:DropDownList>
                            <asp:LinkButton ID="lnk_Fetch" runat="server" Style="text-decoration: underline;"
                                Font-Bold="true" OnClick="lnk_Fetch_Click">Fetch</asp:LinkButton>
                        </td>
                        <td width="5%" align="right">
                            <asp:Label ID="Label8" runat="server" Text="Report No"></asp:Label>
                        </td>
                        <td width="10%" align="left">
                            <asp:TextBox ID="txt_RecType" Width="50px" ReadOnly="true" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txt_ReportNo" runat="server" ReadOnly="true" Width="140px"></asp:TextBox>
                            <asp:Label ID="lblRecordNo" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblEntry" runat="server" Text="Enter" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td width="12%">
                            <asp:Label ID="Label1" runat="server" Text="Reference No"></asp:Label>
                        </td>
                        <td width="20%">
                            <asp:TextBox ID="txt_ReferenceNo" Width="200px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td width="10%" align="right">
                            <asp:Label ID="Label11" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td width="35%" align="left">
                            <asp:TextBox ID="txt_Description" runat="server" Width="200px"></asp:TextBox>
                            &nbsp;
                            <asp:LinkButton ID="lnkGetData" OnClick="lnkGetAppData_Click" runat="server" Font-Bold="True"
                                Style="text-decoration: underline;">Get App Data</asp:LinkButton>
                        </td>
                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td>
                            <asp:Label ID="Label12" runat="server" Text="Date Of Testing"></asp:Label>
                        </td>
                        <td width="20%">
                            <asp:TextBox ID="txt_DateOfTesting" runat="server" Width="200px"></asp:TextBox>
                            <asp:CalendarExtender ID="txt_DateOfTesting_CalendarExtender" runat="server" CssClass="orange"
                                Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txt_DateOfTesting">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="txt_DateOfTesting_MaskedEditExtender" runat="server"
                                AutoComplete="false" Mask="99/99/9999" MaskType="Date" TargetControlID="txt_DateOfTesting">
                            </asp:MaskedEditExtender>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label13" runat="server" Text="Supplier "></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_Supplier" runat="server" ReadOnly="true" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Collection Date"></asp:Label>
                        </td>
                        <td width="22%">

                            <asp:TextBox ID="txt_CollectionDt" runat="server" ReadOnly="true" Width="200px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="orange"
                                Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy"
                                TargetControlID="txt_CollectionDt">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server"
                                AutoComplete="false" Mask="99/99/9999" MaskType="Date"
                                TargetControlID="txt_CollectionDt">
                            </asp:MaskedEditExtender>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label14" runat="server" Text="Qty"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_Qty" runat="server" AutoPostBack="true" CssClass="textbox" MaxLength="2"
                                onchange="checkNumber(this);" OnTextChanged="txt_QtyOnTextChanged" Width="40px"></asp:TextBox>
                            <asp:TextBox ID="txt_TestId" runat="server" Visible="false"></asp:TextBox>
                         &nbsp;&nbsp;     <asp:Label ID="Label9" runat="server" Text="NABL Scope" Font-Bold="true"></asp:Label>

                    <asp:DropDownList ID="ddl_NablScope" AutoPostBack="true" Width="75px" runat="server">
                        <asp:ListItem Text="--Select--" />
                        <asp:ListItem Text="F" />
                        <asp:ListItem Text="NA" />
                    </asp:DropDownList>
                     &nbsp;&nbsp;
                                    <asp:Label ID="Label2" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="ddl_NABLLocation" runat="server" Width="75px" Enabled="true">
                         <asp:ListItem Value="0" Text="0" />
                        <asp:ListItem Value="1" Text="1" />
                        <asp:ListItem Value="2" Text="2" />
                    </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="150px" Width="942px"
                BorderStyle="Ridge" BorderColor="AliceBlue">

         
                <asp:GridView ID="grdAAC" runat="server" SkinID="gridviewSkin">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr No.">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="70px" />
                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="ID Mark">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_IdMark" runat="server" BorderWidth="0px" Text=""
                                    Width="130px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Length"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Length" runat="server" BorderWidth="0px"
                                    CssClass="textbox" MaxLength="12" onchange="javascript:checkNum(this);" Text=""
                                    Width="90px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Breadth">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Breadth" runat="server" BorderWidth="0px"
                                    CssClass="textbox" MaxLength="12" onchange="javascript:checkNum(this);" Text=""
                                    Width="90px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Height">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Height" runat="server" BorderWidth="0px"
                                    CssClass="textbox" MaxLength="12" onchange="javascript:checkNum(this);"
                                    Width="90px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="C/S Area"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_CSArea" runat="server" BorderWidth="0px" ReadOnly="true"
                                    CssClass="caltextbox" Text="" Width="100px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Load">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Load" runat="server" BorderWidth="0px"
                                    CssClass="textbox" MaxLength="12" onchange="javascript:checkNum(this);"
                                    Width="100px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Comp Str">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_CompStr" runat="server" BorderWidth="0px"
                                    CssClass="caltextbox" ReadOnly="true" Width="100px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Avg. Str">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_AvgStr" runat="server" BorderWidth="0px"
                                    CssClass="caltextbox" ReadOnly="true" Text="" Width="100px" />
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
                <asp:GridView ID="grdAACRemark" runat="server" SkinID="gridviewSkin">
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
                <tr height="30px">
                    <td width="12%">
                        <asp:CheckBox ID="chk_WitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chk_WitnessBy_CheckedChanged" />
                        <asp:Label ID="Label7" runat="server" Text="Witness By"></asp:Label>
                    </td>
                    <td width="22%">
                        <asp:TextBox ID="txt_witnessBy" Width="200px" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td width="20%" style="text-align: right">
                        <asp:Label ID="lbl_TestedBy" runat="server" Text="Tested By"></asp:Label>
                    </td>
                    <td>&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                        <asp:DropDownList ID="ddl_TestedBy" Width="205px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkCal" OnClick="lnkCal_Click" runat="server" OnClientClick="return All();"
                            Font-Bold="True" Style="text-decoration: underline;">Cal</asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" OnClientClick="return All();"
                            Font-Bold="True" Style="text-decoration: underline;">Save</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" Visible="false" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClick="lnk_Exit_Click" runat="server">Exit</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <script type="text/javascript">
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
        function checkReading(inputtxt) {
            var Sap = /^[#]+$/;
            if (inputtxt.value.match(Sap)) {
                return true;
            }

            var numbers = /^\d+(\.\d{1,1})?$/;
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

        function checkNum(inputtxt) {
            var numbers = /^\d+(\.\d{1,1})?$/;
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
        function CalculateCastingDt(dateFmt) {
            var re = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
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
    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
