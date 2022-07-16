<%@ Page Title="Trial MD Letter" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" Theme="duro" CodeBehind="MDLetter.aspx.cs" Inherits="DESPLWEB.MDLetter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <%--<div style="float: right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                    ImageAlign="Right" />
            </div>--%>
            <table width="100%">
                <tr>
                <td colspan="2">
                    <asp:RadioButton ID="optMDL" Text="MDL" GroupName="G1" runat="server" 
                        AutoPostBack="true" oncheckedchanged="optMDL_CheckedChanged" />&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="optFinal" Text="Final" GroupName="G1" runat="server" 
                        AutoPostBack="true" oncheckedchanged="optFinal_CheckedChanged" />&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                <td colspan="2">
                    <asp:RadioButton ID="optEnter" Text="Enter" GroupName="G2" runat="server" 
                        AutoPostBack="true" oncheckedchanged="optEnter_CheckedChanged" />&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="optCheck" Text="Check" GroupName="G2" runat="server" 
                        AutoPostBack="true" oncheckedchanged="optCheck_CheckedChanged" />&nbsp;&nbsp;&nbsp;
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlOtherPendingRpt" Width="205px" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlOtherPendingRpt_SelectedIndexChanged">
                            </asp:DropDownList>&nbsp;&nbsp;
                    <asp:LinkButton ID="lnkFetch" runat="server" 
                                Font-Bold="true" OnClick="lnkFetch_Click">Fetch</asp:LinkButton>
                                &nbsp;&nbsp;
                                 <asp:Label ID="lblRecDate" Width="100px" BorderWidth="0px" runat="server" Visible=false  />
                </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td style="width: 10%">
                        <asp:Label ID="Label1" runat="server" Text="Record No"></asp:Label>
                    </td>
                    <td style="width: 25%">
                        <asp:TextBox ID="txt_RecType" Style="text-align: center" Width="41px" runat="server"
                            ReadOnly="true"></asp:TextBox>
                        <asp:TextBox ID="txt_RefNo" Width="150px" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblTrialName" Font-Bold="true" Font-Underline="true" runat="server"
                            Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_TrialId" Visible="false" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblReportType" Visible="false" runat="server" Text=""></asp:Label>    
                        <asp:Label ID="lblStatus" Visible="false" runat="server" Text=""></asp:Label>                        
                    </td>
                    <td style="width: 15%" align="center">
                        <asp:LinkButton ID="lnkLoadMDLValues" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;" onclick="lnkLoadMDLValues_Click" Visible="false">Load values as per MDL</asp:LinkButton>
                    </td> 
                    <td style="width: 10%" align="center">
                        <asp:LinkButton ID="lnkSievePrnt" OnCommand="lnkSievePrnt_Click" runat="server" Font-Bold="True"
                              Style="text-decoration: underline;">Sieve Analysis</asp:LinkButton>
                    </td>
                    <td style="width: 8%">
                        <asp:LinkButton ID="lnkCoverShtPrnt" Font-Bold="true" Font-Underline="true" Visible="false"
                              runat="server" OnCommand="lnkCoverShtPrnt_Click"> Cover Sheet </asp:LinkButton>
                    </td>
                    <td style="width: 15%">
                        <asp:LinkButton ID="lnkMoisuteCorrPrnt" Font-Bold="true" Font-Underline="true" Visible="false"
                              runat="server" OnClick="lnkMoisuteCorrPrnt_Click"> Moisture Correction </asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="110px"
                Width="940px" BorderColor="AliceBlue">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdMDLetter" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                            <Columns>
                                <asp:TemplateField HeaderText=" ">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Name" Width="100px" BorderWidth="0px" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cement">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Cement" Width="100px" BorderWidth="0px" runat="server" CssClass="caltextbox"
                                            onkeyup="checkDecimal(this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FlyAsh" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_FlyAsh" Width="100px" BorderWidth="0px" runat="server" CssClass="caltextbox"
                                            onkeyup="checkDecimal(this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="G G B S" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_GGBS" BorderWidth="0px" Width="100px" runat="server" CssClass="caltextbox"
                                            onkeyup="checkDecimal(this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Micro Silica" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_MicroSilica" BorderWidth="0px" Width="100px" runat="server"
                                            CssClass="caltextbox" onkeyup="checkDecimal(this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Metakaolin" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Metakaolin" BorderWidth="0px" Width="100px" runat="server"
                                            CssClass="caltextbox" onkeyup="checkDecimal(this)"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="Water Binder" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_WaterBinder" runat="server" BorderWidth="0px" 
                                            CssClass="caltextbox" onkeyup="checkDecimal(this)" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Water to be added" 
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Watertobeadded" runat="server" BorderWidth="0px" 
                                            CssClass="caltextbox" onkeyup="checkDecimal(this)" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Natural Sand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_NaturalSand" runat="server" BorderWidth="0px" 
                                            CssClass="caltextbox" onkeyup="checkDecimal(this)" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Crushed Sand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_CrushedSand" runat="server" BorderWidth="0px" 
                                            CssClass="caltextbox" onkeyup="checkDecimal(this)" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stone Dust" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_StoneDust" runat="server" BorderWidth="0px" 
                                            CssClass="caltextbox" onkeyup="checkDecimal(this)" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Grit" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Grit" runat="server" BorderWidth="0px" 
                                            CssClass="caltextbox" onkeyup="checkDecimal(this)" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="10 mm" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_10mm" runat="server" BorderWidth="0px" 
                                            CssClass="caltextbox" onkeyup="checkDecimal(this)" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="20 mm" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_20mm" runat="server" BorderWidth="0px" 
                                            CssClass="caltextbox" onkeyup="checkDecimal(this)" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="40 mm" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_40mm" runat="server" BorderWidth="0px" 
                                            CssClass="caltextbox" onkeyup="checkDecimal(this)" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Admixure" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Admixture" runat="server" BorderWidth="0px" 
                                            CssClass="caltextbox" onkeyup="checkDecimal(this)" Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <div id="div1" style="height: 1px">
                <asp:Button Text="Material Detail" ID="Tab_MaterialDtl" runat="server" OnClick="Tab_MaterialDtl_Click"
                    CssClass="Initiative" />
                <asp:Button Text="Other Information" ID="Tab_OtherInfo" runat="server" OnClick="Tab_OtherInfo_Click"
                    CssClass="Initiative" />
                <asp:Button Text="Cube Compressive Strength" ID="Tab_CubeCompStr" runat="server"
                    OnClick="Tab_CubeCompStr_Click" CssClass="Initiative" />
            </div>
            <asp:MultiView ID="MainView" runat="server">
                <asp:View ID="View_MaterialDtl" runat="server">
                    <asp:Panel ID="Panel1" runat="server" CssClass="Pnlstyle" Height="150px" Width="940px">
                        <asp:Panel ID="Pnl_Material" runat="server" ScrollBars="Auto" Height="148px" Width="480px">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdTrialInfo" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                        <Columns>
                                            <asp:TemplateField HeaderText=" Material Name ">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaterialName" Width="200px" BorderWidth="0px" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Value">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_TrialValue" ReadOnly="true" Width="120px" BorderWidth="0px"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Specific Gravity" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_SpecGravity" ReadOnly="true" Width="120px" BorderWidth="0px"
                                                        runat="server" Style="text-align: right" onkeyup="checkDecimal(this)"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View_OtherInfo" runat="server">
                    <asp:Panel ID="Pnl_OtherInfo" runat="server" CssClass="Pnlstyle" ScrollBars="Auto"
                        Height="150px" Width="940px">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCemetitious" runat="server" Text="Total Cementitious Material"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCementitious"  runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblCompaction" runat="server" Text="Compaction Factor"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCompaction"  runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblAfter" runat="server" Text="After"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAfter"  runat="server"></asp:TextBox><span><asp:Label ID="lblAfterMin" runat="server" Text="Min"></asp:Label></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSlumpnotExced" runat="server" Text="Slump Not to Exceed"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSlumpnotExceed"  runat="server"></asp:TextBox><span>
                                        mm</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblCement" runat="server" Visible="false" Text="Cement Used"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCementUsed"  Visible="false" runat="server"></asp:TextBox>
                                </td>                                
                                <td>
                                    <asp:Label ID="lblSlumpNotExcd" runat="server" Text="Slump Not to Exceed"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSlumpnotExcdAfter"  runat="server"></asp:TextBox>
                                    <span><asp:Label ID="lblSlumpNotExcdmm" runat="server" Text="mm"></asp:Label></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblGrade" runat="server" Text="Grade of Concrete"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGradeofConcrete" runat="server" ontextchanged="txtGradeofConcrete_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblStdDev" runat="server" Text="Std. Dev. Assumed"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStdDev"  runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblAdmix" runat="server" Visible="false" Text="Admixture Used"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdmixureUsed" Visible="false"  runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSpecReq" runat="server" Text="Special Requirement"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSpecReqment"  runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblNaturework" runat="server" Text="Nature of Work"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNatureofWork"  runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblFlyash" runat="server" Visible="false" Text="Fly Ash Used"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFlyashUsed"  Visible="false" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View_CubeCompStr" runat="server">
                    <asp:Panel ID="Panel2" runat="server" CssClass="Pnlstyle" Height="150px" Width="940px">
                        <asp:Panel ID="PnlCubeCast" ScrollBars="Auto" runat="server" Width="480px" Height="148px">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Kind Attention"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_KindAttention" Width="250px" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chk_Flow" runat ="server" Text =" Flow " />
                                       
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="grdCubeCasting" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_CoverSheet" runat="server" AutoPostBack="true" OnCheckedChanged="chk_CoverSheet_OnCheckedChanged" />
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Days">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Days" BorderWidth="0px" Width="110px" runat="server" Style="text-align: right"
                                                ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cubes" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Cubes" BorderWidth="0px" Width="110px" runat="server" Style="text-align: right"
                                                ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Avg Comp Strength" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_AvgCompStr" BorderWidth="0px" Width="130px" runat="server" Style="text-align: right"
                                                ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </asp:Panel>
                </asp:View>
            </asp:MultiView>
            <asp:Panel ID="PnlRemark" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="80px"
                Width="940px" BorderColor="AliceBlue">
                <asp:GridView ID="grdRemark" runat="server" SkinID="gridviewSkin">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnAddRowRemark" runat="server" OnCommand="imgBtnAddRowRemark_Click"
                                    ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                    ToolTip="Add Row" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnDeleteRowRemark" runat="server" OnCommand="imgBtnDelRowRemark_Click"
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
                <%--<tr style="height: 26px">
                    <td style="width: 15%">
                        <asp:CheckBox ID="chk_WitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chk_WitnessBy_CheckedChanged" />
                        <asp:Label ID="Label10" runat="server" Text="Witness By "></asp:Label>
                    </td>
                    <td width="40%">
                        <asp:TextBox ID="txt_witnessBy" Width="200px" Visible="false" runat="server"></asp:TextBox>
                    </td>
                    <td align="left">
                        <asp:Label ID="lbl_TestedBy" runat="server" Text="Entered By"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_TestedBy" Width="205px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        
                    </td>
                </tr>--%>
                <tr>
                    <td width="10%">
                        <asp:CheckBox ID="chkWitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chkWitnessBy_CheckedChanged" />                        
                        <asp:Label ID="lblWitnessBy" runat="server" Text="Witness By"></asp:Label>
                    </td>
                    <td width="17%">
                        <asp:TextBox ID="txtWitnessBy" Visible="false" runat="server" Width="140px"></asp:TextBox>
                    </td>
                    <td width="8%" style="text-align: right">
                        <asp:Label ID="lblTestdApprdBy" runat="server" Text="Approved By"></asp:Label>
                    </td>
                    <td width="17%" align="center">
                        <asp:DropDownList ID="ddlTestdApprdBy" runat="server" Width="140px">
                        </asp:DropDownList>
                    </td>
                    <td width="8%">
                        <asp:Label ID="lblEntdChkdBy" runat="server" Text="Checked By"></asp:Label>
                    </td>
                    <td width="17%" align="center">
                        <asp:TextBox ID="txtEntdChkdBy" runat="server" Width="140px" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td width="30%" align="right">
                       <%--<asp:LinkButton ID="Lnk_Calculate" OnClick="Lnk_Calculate_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;" Visible="false">Cal</asp:LinkButton>--%>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" runat="server" Visible="false"
                             Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClick="lnk_Exit_Click" runat="server">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
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
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
