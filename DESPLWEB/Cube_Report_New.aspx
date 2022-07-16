<%@ Page Language="C#" AutoEventWireup="true" Title="Cube Test Report" MasterPageFile="~/MstPg_Veena.Master"
    CodeBehind="Cube_Report_New.aspx.cs" Inherits="DESPLWEB.Cube_Report_New" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <div style="height: 1px" align="right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                    ImageAlign="Right" />
            </div>
            <asp:Panel ID="Panel2" runat="server" BorderStyle="Ridge" Width="946px" BorderColor="AliceBlue" Height="138px" ScrollBars="Auto">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblOtherPendingRptMF" runat="server" Text="Other Pending Reports" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOtherPendingRptMF" Width="205px" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlOtherPendingRptMF_SelectedIndexChanged" Visible="false">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblTrial" runat="server" Text="Trial" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTrial" Width="205px" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlTrial_SelectedIndexChanged" Visible="false">
                            </asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td width="15%">
                            <asp:Label ID="lbl_OtherPending" runat="server" Text="Other Pending Reports"></asp:Label>
                        </td>
                        <td width="30%">
                            <asp:DropDownList ID="ddl_OtherPendingRpt" AutoPostBack="true" OnSelectedIndexChanged="ddl_OtherPendingRpt_SelectedIndexChanged"
                                Width="205px" runat="server">
                            </asp:DropDownList>
                            <asp:LinkButton ID="lnk_Fetch" runat="server" Style="text-decoration: underline;"
                                Font-Bold="true" OnClick="lnk_Fetch_Click">Fetch</asp:LinkButton>
                        </td>
                        <td width="10%">
                            <asp:Label ID="lbl_RptNo" runat="server" Text="Report No"></asp:Label>
                        </td>
                        <td width="30%">
                            <asp:TextBox ID="txt_RecordType" Style="text-align: center" Width="52px" runat="server"
                                ReadOnly="true"></asp:TextBox>
                            <asp:TextBox ID="txt_ReportNo" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                            &nbsp;&nbsp;
                            <asp:Label ID="lblEnqNo" runat="server" Text="Enq No."></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblRecordNo" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblEntry" runat="server" Text="Enter" Visible="false"></asp:Label>
                            <asp:Label ID="lblTrialId" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblCubecompstr" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblResult" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblComprTest" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblChkStatus" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblDays" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblcubetype" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Reference No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_ReferenceNo" Width="200px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Date Of Testing"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_DtOfTesting" Width="200px" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_DtOfTesting">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txt_DtOfTesting"
                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                            </asp:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Date Of Casting"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_DtOfCasting" Width="100px" runat="server" onblur="CalculateCastingDt(this)"></asp:TextBox>


                        </td>
                        <td>
                            <asp:Label ID="lbl_NatureOfWork" runat="server" Text="Nature Of Work"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_NatureOfwork" Width="200px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_GradeOfConcrte" runat="server" Text="Grade Of Concrete"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_gradeOfConcrete" Width="205px" runat="server">
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
                        </td>
                        <td>
                            <asp:Label ID="lbl_Testtype" runat="server" Text="Test Type"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_testtype" Width="205px" runat="server">
                                <asp:ListItem Text="Concrete Cube" />
                                <asp:ListItem Text="Mortar Cube" />
                                <asp:ListItem Text="Accelerated Curing" />
                                <asp:ListItem Text="Site Cube Casting" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Desc" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Description" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblCubeSize" runat="server" Text="Cube Size (mm)"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCubeSize" Width="70px" runat="server">
                                <asp:ListItem Text="150" />
                                <asp:ListItem Text="50" />
                                <asp:ListItem Text="70" />
                                <asp:ListItem Text="100" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Qty"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Qty" CssClass="textbox" MaxLength="2" Width="50px" runat="server"
                                onchange="checkint(this);" AutoPostBack="true" OnTextChanged="txt_QtyOnTextChanged"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="125px" Width="942px"
                BorderStyle="Ridge" BorderColor="AliceBlue">
                <div>
                    &nbsp;
                     <asp:Label ID="Label9" runat="server" Font-Bold="true" Text="NABL Scope"></asp:Label>
                    <asp:DropDownList ID="ddl_NablScope" runat="server" Width="80px">
                        <asp:ListItem Text="--Select--" />
                        <asp:ListItem Text="F" />
                        <asp:ListItem Text="NA" />
                    </asp:DropDownList>
                    &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                    <asp:Label ID="Label11" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="ddl_NABLLocation" runat="server" Width="80px" Enabled="true">
                        <asp:ListItem Value="0" Text="0" />
                        <asp:ListItem Value="1" Text="1" />
                        <asp:ListItem Value="2" Text="2" />
                    </asp:DropDownList>
                    <br />
                    <br />
                </div>
                <asp:GridView ID="grdCubeInward" runat="server" SkinID="gridviewSkin" OnRowCommand="grdCubeInward_RowCommand" Width="1137px">
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
                                <asp:TextBox ID="txt_IdMark" BorderWidth="0px" Width="70px" runat="server" Text='' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Length" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Length" BorderWidth="0px" CssClass="textbox" MaxLength="12"
                                    runat="server" Text='' Width="70px" onchange="javascript:checkNum(this);" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Breadth" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Breadth" BorderWidth="0px" CssClass="textbox" MaxLength="12"
                                    runat="server" Text='' Width="70px" onchange="javascript:checkNum(this);" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Height" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Height" BorderWidth="0px" CssClass="textbox" MaxLength="12"
                                    Width="70px" runat="server" onchange="javascript:checkNum(this);"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Weight" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Weight" BorderWidth="0px" CssClass="textbox" MaxLength="12"
                                    runat="server" Text='' Width="70px" onchange="javascript:CheckWeight(this);" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reading" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Reading" BorderWidth="0px" CssClass="textbox" MaxLength="12"
                                    runat="server" Text='' Width="70px" onchange="javascript:checkReading(this);" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Age" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Age" BorderWidth="0px" CssClass="ctextbox" Width="50px" MaxLength="2"
                                    runat="server" Text='' ReadOnly="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="C/S Area" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_CSArea" BorderWidth="0px" CssClass="caltextbox" runat="server"
                                    Text='' Width="70px" ReadOnly="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Density" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_DEnsity" BorderWidth="0px" CssClass="caltextbox" runat="server"
                                    Text='' Width="70px" ReadOnly="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comp Str" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_CompStr" BorderWidth="0px" CssClass="caltextbox" Width="70px"
                                    runat="server" ReadOnly="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Avg. Str" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_AvgStr" BorderWidth="0px" CssClass="caltextbox" runat="server"
                                    Text='' Width="70px" ReadOnly="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Images" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewImages" runat="server" ToolTip="View Images"
                                    Style="text-decoration: underline;" CommandName="ViewImages">view</asp:LinkButton>
                                <asp:Label ID="lblImage1" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblImage2" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblImage3" runat="server" Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--<asp:TemplateField HeaderText="Image" HeaderStyle-HorizontalAlign="Center">
                              <ItemTemplate>
                               <asp:Label ID="lblIlmageNm" runat="server" Visible="false"></asp:Label>                        
                                <asp:LinkButton ID="lnkViewImage" runat="server" ToolTip="View Image"
                                   Style="text-decoration: underline;"
                                CommandName="ViewImage">view</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Image" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                               <asp:Label ID="lblIlmageNm1" runat="server" Visible="false"></asp:Label>                        
                                <asp:LinkButton ID="lnkViewImage1" runat="server" ToolTip="View Image"
                                   Style="text-decoration: underline;"
                                CommandName="ViewImage1">view</asp:LinkButton>
                            </ItemTemplate>
                       </asp:TemplateField>--%>
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
                    <td width="30%">
                        <asp:TextBox ID="txt_witnessBy" Width="200px" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td width="10%" style="text-align: right">
                        <asp:Label ID="lbl_TestedBy" runat="server" Text="Tested By"></asp:Label>
                    </td>
                    <td width="30%" align="center">
                        <asp:DropDownList ID="ddl_TestedBy" Width="205px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td width="100%" align="left">
                        <asp:LinkButton ID="Lnk_Calculate" OnClick="Lnk_Calculate_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Cal</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" Visible="false" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="LnkExit" OnClick="LnkExit_Click" Font-Bold="True" Style="text-decoration: underline;"
                            runat="server">Exit</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>
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
                background-color: ActiveBorder;
            }
        </style>
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
    </div>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
