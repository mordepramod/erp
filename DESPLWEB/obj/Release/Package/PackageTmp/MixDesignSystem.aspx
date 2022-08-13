<%@ Page Title="Mix Design - System" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="MixDesignSystem.aspx.cs" Inherits="DESPLWEB.MixDesignSystem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <table width="98%">
                <tr>
                    <td style="width: 10%">
                        <strong>Reference No. </strong>
                    </td>
                    <td style="width: 20%">
                        <asp:DropDownList ID="ddlReferenceNo" Width="150px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlReferenceNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5%">
                        <asp:LinkButton ID="lnkFetch" runat="server" Font-Bold="true" Style="text-decoration: underline;"
                            OnClick="lnkFetch_Click" Visible="false">Fetch</asp:LinkButton>
                    </td>
                    <td style="width: 5%">
                        <strong>Trial</strong>
                    </td>
                    <td style="width: 20%">
                        <asp:DropDownList ID="ddlTrial" Width="150px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTrial_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 40%">
                        <asp:CheckBox ID="chkApprove" Text="Approve" runat="server" />
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkCalculate" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkCalculate_Click" Font-Bold="True"> Calculate </asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkSave_Click" Font-Bold="True"> Save </asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkTrial" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkTrial_Click" Font-Bold="True"> Trial </asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSaveCostMaster" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkSaveCostMaster_Click" Font-Bold="True"> Save Cost in Master </asp:LinkButton>
                    </td>
                    <td >
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" />
                    </td>
                </tr>
            </table>
            <div style="height: 5px">
            </div>
            <asp:Panel ID="pnlDetails" runat="server" Width="100%" BorderWidth="0px" Height="435px"
                ScrollBars="Auto" Visible="false">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlInput" runat="server" Width="98%" BorderWidth="1px" Height="360px"
                            ScrollBars="Auto" BorderStyle="Solid">
                            <table>
                                <tr>
                                    <td style="width: 17%">
                                        Grade
                                    </td>
                                    <td style="width: 20%">
                                        <asp:DropDownList ID="ddlGrade" runat="server" Width="150px">
                                            <asp:ListItem Text="---Select---" />
                                            <asp:ListItem Text="M 5" />
                                            <%--<asp:ListItem Text="M 7.5" />--%>
                                            <asp:ListItem Text="M 10" />
                                            <asp:ListItem Text="M 15" />
                                            <asp:ListItem Text="M 20" />
                                            <asp:ListItem Text="M 25" />
                                            <asp:ListItem Text="M 30" />
                                            <asp:ListItem Text="M 35" />
                                            <%--<asp:ListItem Text="M 37" />
                                            <asp:ListItem Text="M 37.5" />--%>
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
                                    </td>
                                    <td style="width: 12%">
                                    </td>
                                    <td style="width: 20%">
                                        Fine Aggregate Percentage
                                    </td>
                                    <td style="width: 25%">
                                        FA1 : 
                                        <asp:TextBox ID="txtPercentageFA1" runat="server" Width="50px" Text=""></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;
                                        FA2 : 
                                        <asp:TextBox ID="txtPercentageFA2" runat="server" Width="50px" Text=""></asp:TextBox>
                                         &nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkFetchFAPercentWise" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                                            OnClick="lnkFetchFAPercentWise_Click" Font-Bold="True" Visible="false"> Fetch </asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlType" runat="server" Width="150px">
                                            <asp:ListItem Text="Pumpable" />
                                            <asp:ListItem Text="Non Pumpable" />
                                            <asp:ListItem Text="Self Compacting" />
                                            <%--<asp:ListItem Text="Concrete Mix Design" />
                                            <asp:ListItem Text="Pumpable Concrete Mix Design" />
                                            <asp:ListItem Text="Self Compacting Concrete" />--%>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        4.75 passing %
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt4p75PassingPercent" runat="server" Width="150px" Text="" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Slump
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSlump" runat="server" Width="150px" Text=""></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        2.36 passing %
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt2p36PassingPercent" runat="server" Width="150px" Text="" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Cement Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCementType" runat="server" Width="150px">
                                            <%--<asp:ListItem Text="---Select---" />--%>
                                            <asp:ListItem Text="Type F" />
                                            <asp:ListItem Text="Type E" />
                                            <asp:ListItem Text="Type D" />
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        600 micron %
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt600MicronPercent" runat="server" Width="150px" Text="" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Decide % Replacement </strong>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <strong>Standard</strong>
                                    </td>
                                    <td>
                                        300 micron passing
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt300micronPassing" runat="server" Width="150px" Text="" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkFlyashPercent" Text="Fly Ash" runat="server" AutoPostBack="true"
                                            OnCheckedChanged="chkFlyashPercent_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFlyashPercent" runat="server" Width="150px" Text=""></asp:TextBox>
                                    </td>
                                    <td>
                                        25
                                    </td>
                                    <td>
                                        150 micron passing
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt150micronPassing" runat="server" Width="150px" Text="" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkGGBSPercent" Text="GGBS" runat="server" AutoPostBack="true"
                                            OnCheckedChanged="chkGGBSPercent_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGGBSPercent" runat="server" Width="150px" Text=""></asp:TextBox>
                                    </td>
                                    <td>
                                        40
                                    </td>
                                    <td>
                                        Specific Gravity of Cement
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSPGCement" runat="server" Width="150px" Text="" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkMicrosillicaPercent" Text="Microsilica" runat="server" AutoPostBack="true"
                                            OnCheckedChanged="chkMicrosillicaPercent_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMicrosillicaPercent" runat="server" Width="150px" Text="" ></asp:TextBox>
                                    </td>
                                    <td>
                                        5
                                    </td>
                                    <td>
                                        Specific Gravity of Flyash
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSPGFlyash" runat="server" Width="150px" Text="" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Maximum Size of Aggregate
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAggregate" runat="server" Width="150px">
                                            <asp:ListItem Text="---Select---" />
                                            <asp:ListItem Text="10 mm" />
                                            <asp:ListItem Text="20 mm" />
                                            <asp:ListItem Text="40 mm" />
                                            <%--<asp:ListItem Text="Natural Sand" />
                                            <asp:ListItem Text="Crushed Sand" />
                                            <asp:ListItem Text="Grit" />
                                            <asp:ListItem Text="Stone Dust" />--%>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chk10mmPresent" Text="10mm Present" runat="server" />
                                    </td>
                                    <td>
                                        Specific Gravity of GGBS
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSPGGGBS" runat="server" Width="150px" Text="" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Flakyness
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButton ID="optFlakynessLessThan30" Text="Less than 30" runat="server" GroupName="g1" />&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="optFlakyness30to40" Text="30 to 40" runat="server" GroupName="g1" />&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="optFlakynessGreaterThan40" Text="Greater than 40" runat="server"
                                            GroupName="g1" />
                                    </td>
                                    <td>
                                        Specific Gravity of Microsilica
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSPGMicroSilica" runat="server" Width="150px" Text="" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Aggregate Type
                                    </td>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkRoundedAggregate" Text="Rounded Aggregate" runat="server" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkNaturalSand" Text="Natural Sand" runat="server" />
                                    </td>
                                    <td>
                                        Specific Gravity of Fine Aggregate</td>
                                    <td>
                                        <asp:TextBox ID="txtSPGFA" runat="server" Width="150px" Text="" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Admixture %
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAdmixturePercent" runat="server" Width="50px" Text=""></asp:TextBox>
                                        &nbsp;&nbsp;
                                        Max Admixture %
                                        &nbsp;
                                        <asp:TextBox ID="txtMaxAdmixturePercent" runat="server" Width="50px" Text=""></asp:TextBox>
                                    </td>
                                    
                                    <td>
                                        Specific Gravity of Aggregate
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSPGAggt" runat="server" Width="150px" Text="" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Reduction Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlReductionType" runat="server" Width="150px">
                                            <asp:ListItem Text="---Select---" />
                                            <asp:ListItem Text="Type A (naptha)" />
                                            <asp:ListItem Text="Type B (Mid PC)" />
                                            <asp:ListItem Text="Type C (PC)" />
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        Assume plastic density = 2500
                                    </td>
                                    <td align="center">
                                        
                                    </td>
                                </tr>
                                <tr style="height: 5px">
                                    <td colspan="3">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <div style="height: 5px">
                        </div>                        
                        <asp:Panel ID="pnlResult" runat="server" Width="98%" BorderWidth="1px" Height="430px"
                            ScrollBars="Auto" BorderStyle="Solid">
                            <table>
                                <tr>
                                    <td style="width: 14%">
                                        
                                    </td>
                                    <td style="width: 15%">
                                        
                                    </td>
                                    <td style="width: 12%">
                                    </td>
                                    <td style="width: 15%">
                                        <strong>Wieght per m3</strong>
                                    </td>
                                    <td style="width: 15%">
                                        <strong>Unit cost per kg </strong>
                                    </td>
                                    <td style="width: 15%">
                                        <strong>Cost </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Plastic density
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultPlasticDensity" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        
                                    </td>
                                    <td>
                                        
                                    </td>
                                    <td>
                                        
                                    </td>
                                    <td>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Water Cement Ratio
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultWCRatio" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        Water
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultWater" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultWaterUnitCost" runat="server" Width="100px" Text="" onchange="checkDecimal(this)"
                                            MaxLength="15" AutoPostBack="true" OnTextChanged="txtResultWaterUnitCost_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultWaterCost" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Binder Content
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultBinderContent" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        % Cement
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultCementPercent" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        Cement
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultCement" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultCementUnitCost" runat="server" Width="100px" Text="" onchange="checkDecimal(this)"
                                            MaxLength="15" AutoPostBack="true" OnTextChanged="txtResultCementUnitCost_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultCementCost" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        % Flyash
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultFlyashPercent" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        Flyash
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultFlyash" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultFlyashUnitCost" runat="server" Width="100px" Text="" onchange="checkDecimal(this)"
                                            MaxLength="15" AutoPostBack="true" OnTextChanged="txtResultFlyashUnitCost_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultFlyashCost" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        % GGBS
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultGGBSPercent" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        GGBS
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultGGBS" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultGGBSUnitCost" runat="server" Width="100px" Text="" onchange="checkDecimal(this)"
                                            MaxLength="15" AutoPostBack="true" OnTextChanged="txtResultGGBSUnitCost_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultGGBSCost" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        % Microsilica
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultMicrosilicaPercent" runat="server" Width="100px" Text=""
                                            BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        Microsilica
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultMicrosilica" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultMicrosilicaUnitCost" runat="server" Width="100px" Text=""
                                            onchange="checkDecimal(this)" MaxLength="15" AutoPostBack="true" OnTextChanged="txtResultMicrosilicaUnitCost_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultMicrosilicaCost" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Admixture dosage %
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultAdmixturePercent" runat="server" Width="100px" Text=""
                                            BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        Admixture
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultAdmixture" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultAdmixtureUnitCost" runat="server" Width="100px" Text=""
                                            onchange="checkDecimal(this)" MaxLength="15" AutoPostBack="true" OnTextChanged="txtResultAdmixtureUnitCost_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultAdmixtureCost" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        FA %
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultFAPercent" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        Fine Aggregate 1
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultFA1" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultFA1UnitCost" runat="server" Width="100px" Text="" onchange="checkDecimal(this)"
                                            MaxLength="15" AutoPostBack="true" OnTextChanged="txtResultFA1UnitCost_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultFA1Cost" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        
                                    </td>
                                    <td>
                                        
                                    </td>
                                    <td>
                                        Fine Aggregate 2
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultFA2" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultFA2UnitCost" runat="server" Width="100px" Text="" onchange="checkDecimal(this)"
                                            MaxLength="15" AutoPostBack="true" OnTextChanged="txtResultFA2UnitCost_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultFA2Cost" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        10mm %
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResult10mmPercent" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        10mm
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResult10mm" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResult10mmUnitCost" runat="server" Width="100px" Text="" onchange="checkDecimal(this)"
                                            MaxLength="15" AutoPostBack="true" OnTextChanged="txtResult10mmUnitCost_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResult10mmCost" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        20mm %
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResult20mmPercent" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        20mm
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResult20mm" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResult20mmUnitCost" runat="server" Width="100px" Text="" onchange="checkDecimal(this)"
                                            MaxLength="15" AutoPostBack="true" OnTextChanged="txtResult20mmUnitCost_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResult20mmCost" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        40mm %
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResult40mmPercent" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        40mm
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResult40mm" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResult40mmUnitCost" runat="server" Width="100px" Text="" onchange="checkDecimal(this)"
                                            MaxLength="15" AutoPostBack="true" OnTextChanged="txtResult40mmUnitCost_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResult40mmCost" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        W 300
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultW300" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="center">
                                        <strong>Total cost </strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalCost" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Mortar Volume %
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultMortarVolume" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                        Water powder ratio
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtResultWaterPowderRatio" runat="server" Width="100px" Text="" BackColor="LightGray"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>                                       
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
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
                    alert("Only Numeric Values Allowed");

                    return false;
                }

            }
            return true;
        }
    </script>
</asp:Content>
