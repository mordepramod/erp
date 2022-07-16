<%@ Page Title="NDT Report Entry" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="NDT_ReportNew1.aspx.cs" Inherits="DESPLWEB.NDT_ReportNew1" Theme="duro" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="myform">
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px">
            <div style="height: 1px" align="right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                    ImageAlign="Right" />
            </div>
            <asp:Panel ID="Panel2" runat="server" BorderStyle="Ridge" Width="942px" BorderColor="AliceBlue">
                <table style="width: 100%">
                    <tr>
                        <td width="15%">
                            <asp:Label ID="lbl_OtherPending" runat="server" Text="Other Pending Reports"></asp:Label>
                        </td>
                        <td width="27%">
                            <asp:DropDownList ID="ddl_OtherPendingRpt" Width="205px" runat="server">
                            </asp:DropDownList>
                            <asp:LinkButton ID="lnk_Fetch" runat="server" Style="text-decoration: underline;"
                                Font-Bold="true" OnClick="lnk_Fetch_Click">Fetch</asp:LinkButton>
                        </td>
                        <td width="12%" align="right">
                            <asp:Label ID="Label10" runat="server" Text="Report No"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td width="20%">
                            <asp:TextBox ID="txt_RecType" Width="50px" ReadOnly="true" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txt_ReportNo" runat="server" ReadOnly="true" Width="140px"></asp:TextBox>
                        </td>
                        <td width="28%" align="right">
                            <asp:Label ID="lblEntry" runat="server" Text="Enter" Visible="false"></asp:Label>
                            <asp:Label ID="lblRecordNo" runat="server" Text="" Visible="false"></asp:Label>
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
                            <asp:Label ID="Label2" runat="server" Text="Testing Date"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td align="center">
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
                            <asp:Label ID="Label5" runat="server" Text="Kind Attention"></asp:Label>&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txt_KindAttention" Width="200px" runat="server"></asp:TextBox>&nbsp;
                        </td>
                        <td align="right">
                            <asp:Label ID="Label8" runat="server" Text="Hammer No"></asp:Label>&nbsp;&nbsp;
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="ddl_HammerNo" Width="205px" runat="server" Enabled="false">
                                <asp:ListItem Text="Select" />
                                <asp:ListItem Text="151309" />
                                <asp:ListItem Text="161854" />
                                <asp:ListItem Text="140609" />
                                <asp:ListItem Text="164305" />
                                <asp:ListItem Text="167501" />
                                <asp:ListItem Text="156298" />
                                <asp:ListItem Text="156310" />
                                <asp:ListItem Text="13006083" />
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;&nbsp;
                                    <asp:Label ID="lblEnquiryNo" runat="server" Text="" Visible="false"></asp:Label>&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkGetData" OnClick="lnkGetAppData_Click" runat="server" Font-Bold="True"
                                        Style="text-decoration: underline;" Visible="false">Get App Data</asp:LinkButton>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="NDT By"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_NDTBy" Width="205px" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_NDTBy_SelectedIndexChanged">
                                <asp:ListItem Text="UPV" />
                                <asp:ListItem Text="Rebound Hammer" />
                                <asp:ListItem Text="Both" />
                                <asp:ListItem Text="UPV with Grading" />
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;&nbsp; 
                                    
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr style="height: 30px">
                        <td colspan="5">
                            <asp:CheckBox ID="Chk_Indirect" runat="server" AutoPostBack="true" OnCheckedChanged="Chk_Indirect_CheckedChanged" Text="InDirect" />
                            &nbsp;
                            <asp:CheckBox ID="Chk_CorrectionFactor" runat="server" Text="Apply Correction Factor" AutoPostBack="true" OnCheckedChanged="Chk_CorrectionFactor_CheckedChanged"
                                Visible="false" />&nbsp;
                            <asp:TextBox ID="txt_CorectionFact" Visible="false" AutoPostBack="true" OnTextChanged="txt_CorectionFact_TextChanged"
                                Text='' onchange="CheckCorrFact(this);" Width="30px" runat="server" MaxLength="5"></asp:TextBox>
                            <asp:CheckBox ID="Chk_EditedIndStr" runat="server" Text="Add Column for edited Ind Str." AutoPostBack="true" OnCheckedChanged="Chk_EditedIndStr_CheckedChanged"
                                Visible="false" />
                        </td>
                    </tr>
                    <tr style="height: 30px">
                        <td colspan="5">
                            <asp:CheckBox ID="chkClusterAnalysis" Text="Cluster Analysis" runat="server" AutoPostBack="true" OnCheckedChanged="chkClusterAnalysis_CheckedChanged" />
                            &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblClusterAnalysis" runat="server" Text="Apply Cluster Analysis Str. Equation" Font-Bold="true"></asp:Label>&nbsp;&nbsp;
                                    <asp:RadioButton ID="optMinus10" Text="-10" GroupName="g1" runat="server" />&nbsp;&nbsp;
                                    <asp:RadioButton ID="opt10" Text="10" GroupName="g1" runat="server" />&nbsp;&nbsp;
                                    <asp:RadioButton ID="optWithin10" Text="Within 10" GroupName="g1" runat="server" />&nbsp;&nbsp;
                                    <asp:RadioButton ID="optShapoorji" Text="Shapoorji" GroupName="g1" runat="server" />

                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" valign="middle">
                            <asp:Label ID="Label6" runat="server" Text="NABL Scope" Font-Bold="true"></asp:Label>&nbsp;&nbsp;
                            <asp:DropDownList ID="ddl_NablScope" AutoPostBack="true" Width="80px" runat="server">
                                <asp:ListItem Text="--Select--" />
                                <asp:ListItem Text="F" />
                                <asp:ListItem Text="NA" />
                            </asp:DropDownList>
                            &nbsp;&nbsp;  &nbsp;&nbsp;    &nbsp;&nbsp;
                                    <asp:Label ID="Label12" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>&nbsp;&nbsp;
                            <asp:DropDownList ID="ddl_NABLLocation" runat="server" Width="80px" Enabled="true">
                                <asp:ListItem Value="0" Text="0" />
                                <asp:ListItem Value="1" Text="1" />
                                <asp:ListItem Value="2" Text="2" />
                            </asp:DropDownList>
                            &nbsp;&nbsp;  &nbsp;&nbsp;&nbsp;&nbsp;                                    
                                    <asp:CheckBox ID="ChkOldGrading" Text="Apply Old Grading Criteria " runat="server" AutoPostBack="true" OnCheckedChanged="ChkOldGrading_CheckedChanged" />

                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="pnlWbs" runat="server" ScrollBars="Auto" Height="240px" Width="942px"
                BorderStyle="Ridge" BorderColor="AliceBlue">
                <asp:GridView ID="grdWBS" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectWBS" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectWBS_CheckedChanged" />
                                <asp:Label ID="lblWBSId" runat="server" Text='<%# Eval("lblWBSId") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("lblStatus") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblClusterAnalysisStatus" runat="server" Text='<%# Eval("lblClusterAnalysisStatus") %>' Visible="false"></asp:Label> 
                                <asp:Label ID="lblClusterAnalysisEquation" runat="server" Text='<%# Eval("lblClusterAnalysisEquation") %>' Visible="false"></asp:Label> 
                                <asp:Label ID="lblCubeStrengthWbs1" runat="server" Text='<%# Eval("lblCubeStrengthWbs1") %>' Visible="false" ></asp:Label>
                                <asp:Label ID="lblCubeStrengthWbs2" runat="server" Text='<%# Eval("lblCubeStrengthWbs2") %>' Visible="false" ></asp:Label>
                                <asp:Label ID="lblCubeStrengthWbs3" runat="server" Text='<%# Eval("lblCubeStrengthWbs3") %>' Visible="false" ></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Building" HeaderText="Building/Structure" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Floor" HeaderText="Floor/Stage" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="MemberType" HeaderText="Member Type" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="MemberId" HeaderText="Member Id" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <div>
                <asp:Label ID="lblSelectedWbs" runat="server" Text="" Font-Bold="true" ForeColor="Brown"></asp:Label>
            </div>
            <div>
                <asp:Label ID="lblCubeStrength" runat="server" Text="Equivalant Cube Strength of Core (MPa) : " Font-Bold="true" ></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;1.&nbsp;
                <asp:TextBox ID="txtCubeStrength1" runat="server" Width="100px" onchange="checkNo1(this);" MaxLength="10"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;2.&nbsp;
                <asp:TextBox ID="txtCubeStrength2" runat="server" Width="100px" onchange="checkNo1(this);" MaxLength="10"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;3.&nbsp;
                <asp:TextBox ID="txtCubeStrength3" runat="server" Width="100px" onchange="checkNo1(this);" MaxLength="10"></asp:TextBox>
            </div>
            <div style="height: 10px">
                &nbsp;&nbsp;
            </div>
            <%--<asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>--%>
            <%--</ContentTemplate>
            </asp:UpdatePanel>--%>
            <asp:Panel ID="Panel3" runat="server" ScrollBars="Auto" Height="240px" Width="942px"
                BorderStyle="Ridge" BorderColor="AliceBlue">
                <asp:GridView ID="grdNDTInward" runat="server" SkinID="gridviewSkin" Width="100%"
                    AutoGenerateColumns="false" OnRowCommand="grdNDTInward_RowCommand">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnNDTTestAddRow" runat="server" OnCommand="imgBtnNDTTestAddRow_Click"
                                    ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                    ToolTip="Add Row" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnNDTTestDeleteRow" runat="server" OnCommand="imgBtnNDTTestDeleteRow_Click"
                                    ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                    ToolTip="Delete Row" />
                            </ItemTemplate>
                            <ItemStyle Width="16px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnMergeRow" runat="server" OnCommand="imgBtnMergeRow_Click"
                                    ImageUrl="Images/m5.jpg" Height="16px" Width="16px" CausesValidation="false"
                                    ToolTip="Merge Row" Visible="false"/>
                            </ItemTemplate>
                            <ItemStyle Width="16px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Description" BorderWidth="0px" Width="150px" MaxLength="250"
                                    runat="server" Text='' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Grade" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddl_Grade" runat="server" BorderWidth="0px" Width="70px">
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
                        <asp:TemplateField HeaderText="Alpha Angle" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddl_AlphaAngle" BorderWidth="0px" runat="server" Width="70px">
                                    <asp:ListItem Text="Select" />
                                    <asp:ListItem Text="a = -90°" />
                                    <asp:ListItem Text="a = +90°" />
                                    <asp:ListItem Text="a = 0°" />
                                    <asp:ListItem Text="---" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rebound Index" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="ReboundIndex" BorderWidth="0px" CssClass="textbox" MaxLength="10"
                                    runat="server" Text='' Width="150px" BackColor="White" OnCommand="ReboundIndex_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pulse Vel" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="PulseVel" BorderWidth="0px" CssClass="textbox" MaxLength="10" runat="server"
                                    Text='' Width="150px" BackColor="White" OnCommand="PulseVel_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Age" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Age" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                    runat="server" Text='' Width="40px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ind Str(N/mm²)" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_IndStr" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                    runat="server" Text='' Width="100px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_ReboundIndexDetails" Visible="false" ReadOnly="true" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_PulseVelDetails" Visible="false" ReadOnly="true" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edited Ind Str." Visible="false" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_EditedIndStr" BorderWidth="0px" CssClass="textbox" runat="server"
                                    Text='' Width="100px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Corr Factor" Visible="false" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_CorrFactor" BorderWidth="0px" CssClass="textbox" runat="server"
                                    Width="100px" onchange="CheckCorrFact(this);" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMergFlag" runat="server" Text="0"></asp:Label>
                                <asp:Label ID="lblWBS" runat="server" Visible="false" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ind Str(-10)" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_IndStr_10" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                    runat="server" Text='' Width="100px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ind Str(10)" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_IndStr10" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                    runat="server" Text='' Width="100px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ind Str(Within10)" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_IndStrWithin10" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                    runat="server" Text='' Width="100px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ind Str(Shapoorji)" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_IndStrShapoorji" BorderWidth="0px" CssClass="caltextbox" ReadOnly="true"
                                    runat="server" Text='' Width="100px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Corr Str." HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_IndCorrStr" BorderWidth="0px" CssClass="textbox"
                                    runat="server" Text='' Width="100px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cluster Analysis" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_IndClusterAnalysis" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Corrected Pulse Vel" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_CorrPulseVal" BorderWidth="0px" CssClass="textbox"
                                    runat="server" Text='' Width="100px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Image1" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblImage1" runat="server" Visible="false"></asp:Label>
                                <asp:LinkButton ID="lnkViewImage1" runat="server" ToolTip="View Image"
                                    Style="text-decoration: underline;"
                                    CommandName="ViewImage1">view</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Image2" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblImage2" runat="server" Visible="false"></asp:Label>
                                <asp:LinkButton ID="lnkViewImage2" runat="server" ToolTip="View Image"
                                    Style="text-decoration: underline;"
                                    CommandName="ViewImage2">view</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Image3" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblImage3" runat="server" Visible="false"></asp:Label>
                                <asp:LinkButton ID="lnkViewImage3" runat="server" ToolTip="View Image"
                                    Style="text-decoration: underline;"
                                    CommandName="ViewImage3">view</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Image4" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblImage4" runat="server" Visible="false"></asp:Label>
                                <asp:LinkButton ID="lnkViewImage4" runat="server" ToolTip="View Image"
                                    Style="text-decoration: underline;"
                                    CommandName="ViewImage4">view</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>

            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <table width="100%">
                <tr style="height: 30px">
                    <td align="right">
                        <asp:LinkButton ID="Lnk_Calculate" OnClick="Lnk_Calculate_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Calculate</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSaveWbsWise" OnClick="lnkSaveWbsWise_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save Title</asp:LinkButton>&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>
            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="200px" Width="942px"
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
                    <td width="11%">
                        <asp:CheckBox ID="chk_WitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chk_WitnessBy_CheckedChanged" />
                        <asp:Label ID="Label7" runat="server" Text="Witness By"></asp:Label>
                    </td>
                    <td width="22%">
                        <asp:TextBox ID="txt_witnessBy" Width="190px" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td width="10%" style="text-align: right">
                        <asp:Label ID="lbl_TestedBy" runat="server" Text="Tested By"></asp:Label>
                    </td>
                    <td width="30%" align="center">
                        <asp:DropDownList ID="ddl_TestedBy" Width="205px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td width="100%" align="right">
                        <asp:CheckBox ID="chkComplete" Text="Check Complete" runat="server" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" Visible="false" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClick="lnk_Exit_Click" runat="server">Exit</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
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
                        <table class="DetailPopup" width="250px">
                            <tr>
                                <td>
                                    <asp:Label ID="lblErr" runat="server" Font-Bold="true" ForeColor="Red" Text=""></asp:Label>
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
                                    <div id="divReboundIndex" runat="server" visible="false">
                                        <fieldset style="border-color: #008080; width: 91%; height: 320px;">
                                            <legend>Rebound Index</legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="Chk_NoReboudIndx" runat="server" Checked="false" />
                                                        <asp:Label ID="Label13" runat="server" Text="NO"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_R1" runat="server" Text="R1"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_R1" Width="120px" Style="text-align: right" onchange="checkNo(this);"
                                                            runat="server" MaxLength="5"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_R2" runat="server" Text="R2"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_R2" Width="120px" Style="text-align: right" runat="server" MaxLength="5"
                                                            onchange="checkNo(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_R3" runat="server" Text="R3"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_R3" Width="120px" Style="text-align: right" runat="server" MaxLength="5"
                                                            onchange="checkNo(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_R4" runat="server" Text="R4"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_R4" Width="120px" Style="text-align: right" runat="server" MaxLength="5"
                                                            onchange="checkNo(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_R5" runat="server" Text="R5"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_R5" Width="120px" Style="text-align: right" runat="server" MaxLength="5"
                                                            onchange="checkNo(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_R6" runat="server" Text="R6"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_R6" Width="120px" Style="text-align: right" runat="server" MaxLength="5"
                                                            onchange="checkNo(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_R7" runat="server" Text="R7"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_R7" Width="120px" Style="text-align: right" runat="server" MaxLength="5"
                                                            onchange="checkNo(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_R8" runat="server" Text="R8"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_R8" Width="120px" Style="text-align: right" runat="server" MaxLength="5"
                                                            onchange="checkNo(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_R9" runat="server" Text="R9"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_R9" Width="120px" Style="text-align: right" runat="server" MaxLength="5"
                                                            onchange="checkNo(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_R10" runat="server" Text="R10"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_R10" Width="120px" Style="text-align: right" runat="server"
                                                            MaxLength="5" onchange="checkNo(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                    <div id="divPulseVel" runat="server" visible="false">
                                        <fieldset style="border-color: #008080; width: 91%; height: 200px;">
                                            <legend>Pulse Velocity</legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="Chk_NO" runat="server" Checked="false" />
                                                        <asp:Label ID="Label4" runat="server" Text="NO"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label9" runat="server" Text="Distance"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_Distance" Width="120px" Style="text-align: right" onchange="checkNo(this);"
                                                            runat="server" MaxLength="5"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label11" runat="server" Text="Time"></asp:Label>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                        <asp:TextBox ID="txt_Time" Width="120px" Style="text-align: right" runat="server"
                                                            MaxLength="5" onchange="checkNo1(this);"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Panel ID="pnlIndirect" runat="server" ScrollBars="Auto" Height="110px" Width="200px"
                                                BorderStyle="Ridge" BorderColor="AliceBlue">
                                                <table width="100%">
                                                    <tr>
                                                        <td colspan="1">
                                                            <asp:Label ID="lblDistances" runat="server" Text=""></asp:Label>
                                                        </td>
                                                        <td colspan="1">
                                                            <asp:Label ID="lblTimes" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>150</td>
                                                        <td>
                                                            <asp:TextBox ID="txt150" runat="server" Text="" Width="80px" onchange="checkNo1(this);" MaxLength="10"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1">300</td>
                                                        <td>
                                                            <asp:TextBox ID="txt300" runat="server" Text="" Width="80px" onchange="checkNo1(this);" MaxLength="10"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1">450</td>
                                                        <td>
                                                            <asp:TextBox ID="txt450" runat="server" Text="" Width="80px" onchange="checkNo1(this);" MaxLength="10"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1">550</td>
                                                        <td>
                                                            <asp:TextBox ID="txt550" runat="server" Text="" Width="80px" onchange="checkNo1(this);" MaxLength="10"></asp:TextBox></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </fieldset>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="imgCloseTestDetails" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:Panel>
        </ContentTemplate>
            </asp:UpdatePanel>
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
            function checkValue(x) {
                var res = x.value.toUpperCase();
                if (x.value != '') {
                    if (res == 'NO') {
                        document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                        return true;
                    }
                    else {
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
                    }
                }
                return true;
            }
            function HideModalPopup() {
                $find("ModalPopupBehaviorID").hide();
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
            function CheckCorrFact(inputtxt) {
                var numbers = /^\d+(\.\d{1,3})?$/;
                if (inputtxt.value.match(numbers)) {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                    return true;
                }
                else {

                    inputtxt.focus();
                    inputtxt.value = "";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
        document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Only Numeric Values Allowed \n or \n After decimal point only 2 values allowed";
                    return false;
                }
            }

            function checkNo(x) {
                var res = x.value.toUpperCase();
                if (x.value != '') {
                    if (res == 'NO') {
                        document.getElementById('<%=lblErr.ClientID %>').style.visibility = "hidden";
            return true;
        }
        else {
            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57))) {
                    x.value = '';
                    x.focus();
                    document.getElementById('<%=lblErr.ClientID %>').style.visibility = "visible";
                                document.getElementById('<%=lblErr.ClientID %>').innerHTML = "Only Numeric Values Allowed";
                                return false;
                            }
                            else {
                                document.getElementById('<%=lblErr.ClientID %>').style.visibility = "hidden";
                            }

                        }
                    }
                }
                return true;
            }

            function checkNo1(x) {
                var res = x.value.toUpperCase();
                if (x.value != '') {
                    if (res == 'NO') {
                        document.getElementById('<%=lblErr.ClientID %>').style.visibility = "hidden";
                        return true;
                    }
                    else {
                        var s_len = x.value.length;
                        var s_charcode = 0;
                        for (var s_i = 0; s_i < s_len; s_i++) {
                            s_charcode = x.value.charCodeAt(s_i);
                            if (!((s_charcode >= 48 && s_charcode <= 57) || s_charcode == 46)) {
                                x.value = '';
                                x.focus();
                                document.getElementById('<%=lblErr.ClientID %>').style.visibility = "visible";
                                document.getElementById('<%=lblErr.ClientID %>').innerHTML = "Only Numeric Values Allowed";
                                return false;
                            }
                            else {
                                document.getElementById('<%=lblErr.ClientID %>').style.visibility = "hidden";
                            }

                        }
                    }
                }
                return true;
            }
        </script>

    </div>
</asp:Content>

