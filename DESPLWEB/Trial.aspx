<%@ Page Title="Trial" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    Theme="duro" CodeBehind="Trial.aspx.cs" Inherits="DESPLWEB.Trial" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <div style="float: right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                    ImageAlign="Right" />
            </div>
            <asp:Panel ID="Panel4" runat="server" ScrollBars="Auto" Width="99%" BorderStyle="Ridge"
                BorderColor="AliceBlue">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_OtherPending" runat="server" Text="Other Pending Reports"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_OtherPendingRpt" Width="150px" runat="server">
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkFetch" runat="server" Font-Bold="true" Style="text-decoration: underline;"
                                OnClick="lnkFetch_Click">Fetch</asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label ID="Label12" runat="server" Text="Trial"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTrial" Width="100px" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlTrial_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lbl_MaterialId" Visible="false" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lbl_TrialId" Visible="false" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblStatus" runat="server" Text="" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            <asp:Label ID="Label1" runat="server" Text="Record No"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txt_RecType" Style="text-align: center" Width="41px" runat="server"
                                ReadOnly="true"></asp:TextBox>
                            <asp:TextBox ID="txt_RefNo" Width="150px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 12%">
                            <asp:Label ID="Label3" runat="server" Text="Trial Date"></asp:Label>
                        </td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txt_TrialDate" runat="server" Width="200px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_TrialDate">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_TrialDate"
                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                            </asp:MaskedEditExtender>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="Label4" runat="server" Text="Trial Time"></asp:Label>
                        </td>
                        <td style="width: 10%">
                            <asp:TextBox ID="txt_TrialTime" Width="60px" runat="server"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_TrialTime"
                                MaskType="time" Mask="99:99" AutoComplete="true" AcceptAMPM="true" runat="server">
                            </asp:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Grade of Concrete"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_GradeofConcrete" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Cement Used"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_CementUsed" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="No Of Cubes"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_NoofCubes" Width="60px" MaxLength="2" runat="server" onkeyup="checkDecimal(this)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Nature Of Work"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Natureofwork" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Admixture Used"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Admixture" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblTotCementitiousMat" runat="server" Text="Total Cementitious Material"
                                Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotCementitiousMat" Width="60px" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Slump Required"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Slump" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="Fly Ash Used"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_FlyashUsed" runat="server" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblFlyAshReplacement" runat="server" Text="Fly Ash Replacement" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFlyAshReplacement" Width="60px" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="float: left">
                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="100px"
                    Width="467px" BorderColor="AliceBlue">
                    <div>
                        &nbsp;
                        <asp:Label ID="Label14" runat="server" Font-Bold="true" Text="Total Moisture"></asp:Label>
                    </div>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_McNatural" runat="server" Text="Natural Sand" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_McNaturalSand" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_McCrushed" runat="server" Text="Crushed Sand" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_McCrushed" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_McStoneDust" runat="server" Text="Stone Dust" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_McStoneDust" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_McGrit" runat="server" Text="Grit" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_McGrit" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Mc10mm" runat="server" Text="10 mm" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Mc10mm" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Mc20mm" runat="server" Text="20 mm" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Mc20mm" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Mc40mm" runat="server" Text="40 mm" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Mc40mm" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div style="float: left">
                <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="100px"
                    Width="467px" BorderColor="AliceBlue">
                    <div>
                        &nbsp;
                        <asp:Label ID="Label13" runat="server" Font-Bold="true" Text="Water Absorption"></asp:Label>
                    </div>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_waNatural" runat="server" Text="Natural Sand" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_waNaturalSand" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right" ReadOnly="true" CssClass="caltextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_waCrushed" runat="server" Text="Crushed Sand" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_waCrushedSand" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right" ReadOnly="true" CssClass="caltextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_waStoneDust" runat="server" Text="Stone Dust" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_waStoneDust" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right" ReadOnly="true" CssClass="caltextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_waGrit" runat="server" Text="Grit" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_waGrit" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right" ReadOnly="true" CssClass="caltextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_wa10mm" runat="server" Text="10 mm" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_wa10mm" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right" ReadOnly="true" CssClass="caltextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_wa20mm" runat="server" Text="20 mm" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_wa20mm" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right" ReadOnly="true" CssClass="caltextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_wa40mm" runat="server" Text="40 mm" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_wa40mm" Text="" Width="200px" runat="server" Visible="false"
                                    Style="text-align: right" ReadOnly="true" CssClass="caltextbox"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="180px"
                Width="940px" BorderColor="AliceBlue">
                <asp:CheckBox ID="chkValidationFor" runat="server" Text="Validation For" OnCheckedChanged="chkValidationFor_CheckedChanged" AutoPostBack="true"/>
                <asp:GridView ID="grdTrail" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                    <Columns>
                        <asp:TemplateField HeaderText=" ">
                            <ItemTemplate>
                                <asp:Label ID="lbl_MaterialName" BorderWidth="0px" runat="server" /><%-- Width="110px"--%>
                                <asp:Label ID="lbl_TrailUnit" BorderWidth="0px" runat="server"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="110px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="System Trial">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_TrailSystem" Width="80px" BorderWidth="0px" runat="server" Style="text-align: right"
                                    ReadOnly="true" CssClass="caltextbox"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Proposed Trial">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Trail" Width="60px" BorderWidth="0px" runat="server" Style="text-align: right"></asp:TextBox>
                                <asp:Label ID="lbl_TrailUnit2" BorderWidth="0px" runat="server" Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sp Grav" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_SpGrav" Width="60px" BorderWidth="0px" runat="server" Style="text-align: right"
                                    onkeyup="checkDecimal(this)"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Volume" ItemStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Volume" Width="80px" BorderWidth="0px" runat="server" ReadOnly="true"
                                    CssClass="caltextbox"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reqd. Wt." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Reqdwt" Width="80px" BorderWidth="0px" runat="server" ReadOnly="true"
                                    CssClass="caltextbox"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Surface Moisture" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_SurfaceMoisture" Width="60px" BorderWidth="0px" runat="server"
                                    ReadOnly="true" CssClass="caltextbox"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Moisture Corrections" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Corrections" Width="80px" BorderWidth="0px" runat="server" ReadOnly="true"
                                    CssClass="caltextbox"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Net Wt. (Kg)" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_NetWt" Width="80px" BorderWidth="0px" runat="server" ReadOnly="true"
                                    CssClass="caltextbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_MatId" runat="server" Visible="false"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Corrections" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_CorrectionsNew" Width="80px" BorderWidth="0px" runat="server"
                                    Style="text-align: right"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actual Trial" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_TrailActual" Width="80px" BorderWidth="0px" runat="server" ReadOnly="true"
                                    CssClass="caltextbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>                
            </asp:Panel>
            <table width="100%">
                <tr>
                    <td style="width: 10%">
                        <asp:Label ID="lblSuperwiseBy" runat="server" Text="Supervise By :"></asp:Label>
                    </td>
                    <td style="width: 10%">
                        <asp:TextBox ID="txt_SuperwiseBy" Width="150px" runat="server"></asp:TextBox>
                    </td>
                    <td style="width: 10%">
                        <asp:CheckBox ID="chk_WitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chk_WitnessBy_CheckedChanged" />
                        <asp:Label ID="Label10" runat="server" Text="Witness By :"></asp:Label>
                    </td>
                    <td style="width: 10%">
                        <asp:TextBox ID="txt_witnessBy" Width="150px" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkAllInAggt" OnClick="lnkAllInAggt_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;" Visible="false"> All in AGGT </asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnksievAnalysisPrint" OnClick="lnksieveprnt_Click" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;" Visible="false"> Sieve Analysis </asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="Lnk_Calculate" OnClick="Lnk_Calculate_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Calculate</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkTrialOtherInfo" OnClick="lnkTrialOtherInfo_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Trial Information</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="lnkPrint_Click">Print Trial Sheet</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClick="lnk_Exit_Click" runat="server">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField1"
                PopupControlID="pnlAllInAGGT" PopupDragHandleControlID="PopupHeader" Drag="true"
                BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlAllInAGGT" runat="server">
                <asp:UpdatePanel runat="server" ID="up2">
                    <ContentTemplate>
                        <table class="DetailPopup" width="800px">
                            <tr>
                                <td align="center" valign="bottom" colspan="2">
                                    <asp:ImageButton ID="imgCloseAllInAGGT" runat="server" ImageUrl="Images/cross_icon.png"
                                        OnClick="imgCloseAllInAGGT_Click" ImageAlign="Right" Width="18px" Height="18px" />
                                    <%--<asp:Label ID="lblpopuphead" runat="server" Text="Test Details" Font-Bold="True"
                                        ForeColor="#990033"></asp:Label>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdAllInAGGT" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        BorderColor="Black" BorderStyle="None" BorderWidth="1px" Width="99%" CellPadding="2"
                                        CellSpacing="1" CssClass="Grid" Font-Size="12px">
                                        <Columns>
                                            <asp:BoundField DataField="SieveSize" HeaderText="Sieve Size" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="NaturalSand" HeaderText="Natural Sand" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="CrushedSand" HeaderText="Crushed Sand" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="StoneDust" HeaderText="Stone Dust" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Grit" HeaderText="Grit" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="10mm" HeaderText="10 mm" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="20mm" HeaderText="20 mm" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="40mm" HeaderText="40 mm" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="MixAggregate" HeaderText="Mix Aggregate" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="NaturalSandPer" HeaderText="Natural Sand" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="CrushedSandPer" HeaderText="Crushed Sand" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="StoneDustPer" HeaderText="Stone Dust" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="GritPer" HeaderText="Grit" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="10mmPer" HeaderText="10 mm" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="20mmPer" HeaderText="20 mm" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="40mmPer" HeaderText="40 mm" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="MixAggregate" HeaderText="Mix Aggregate" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="CombinedPassing" HeaderText="% Combined Passing" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Passing(IS:383)" HeaderText="% Passing(IS:383)" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No records to display...
                                        </EmptyDataTemplate>
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField2" runat="server" />
            <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="HiddenField1"
                PopupControlID="pnlOtherInfo" PopupDragHandleControlID="PopupHeader" Drag="true"
                BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlOtherInfo" runat="server">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <table class="DetailPopup" width="840px">
                            <tr>
                                <td align="center" valign="bottom" colspan="5">
                                    <asp:ImageButton ID="imgCloseOtherInfo" runat="server" ImageUrl="Images/cross_icon.png"
                                        OnClick="imgCloseOtherInfo_Click" ImageAlign="Right" Width="18px" Height="18px" />
                                    <asp:Label ID="lblOtherInfo" runat="server" ForeColor="#990033" Text="Trial Information"
                                        Font-Bold="True" Font-Size="Small"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="4">
                                    <asp:GridView ID="grdSlump" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Retention time (Min) ">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Name" BorderWidth="0px" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="160px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Initial">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_0" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="30">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_30" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="60">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_60" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="90">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_90" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="120">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_120" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="150">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_150" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="180">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_180" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width:10%">
                                    <asp:Label ID="lblYield" runat="server" Text="Yield"></asp:Label>&nbsp;&nbsp;
                                </td>
                                <td style="width:30%">
                                    <asp:TextBox ID="txtYield" Text="" Width="100px" runat="server" Style="text-align: center"></asp:TextBox>
                                </td>
                                <td style="width:25%">
                                    <asp:Label ID="lblWtOfConcreteInCylinder" runat="server" Text="Wt. of Concrete in Cylinder (Kg)"></asp:Label>&nbsp;&nbsp;
                                </td>
                                <td style="width:20%">
                                    <asp:TextBox ID="txtWtOfConcreteInCylinder" Text="" Width="100px" 
                                        runat="server" Style="text-align: center" onkeyup="checkDecimal(this)" 
                                        ontextchanged="txtWtOfConcreteInCylinder_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </td>
                                 <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblComment" runat="server" Text="Comment"></asp:Label>&nbsp;&nbsp;
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtComment" Text="" Width="600px" runat="server"></asp:TextBox>
                                </td>
                                 <td>
                                    <asp:Label ID="lblOtherDay" runat="server" Text="Other Day"></asp:Label>&nbsp;&nbsp;
                                    <asp:TextBox ID="txtOtherDay" Text="" Width="50px" runat="server" onchange="checkNum(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:CheckBox ID="chk_CubeCasting" runat="server" Text="Update Cube Casting"/>
                                </td>
                                <td colspan="4">
                                    <asp:GridView ID="grdTestSchedule" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Test Schedule">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Name" BorderWidth="0px" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="220px" />
                                                <ItemStyle HorizontalAlign="left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Accl Strength">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_1daySch" runat="server" />
                                                    <asp:Label ID="lbl_1dayStr" BorderWidth="0px" runat="server" Style="text-align: center" Text="1 day str"></asp:Label>
                                                     <asp:TextBox ID="txt_1dayStr"  Width="80px"  runat="server"  MaxLength="50" Text=""></asp:TextBox>
                                               </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="3 day">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_3daySch" runat="server" />
                                                    <asp:Label ID="lbl_3dayStr" BorderWidth="0px" runat="server" Style="text-align: center" Text="3 day str"></asp:Label>
                                                    <asp:Label ID="lbl_3dayExp28DaysCompStr" BorderWidth="0px" runat="server" Style="text-align: center" Text=""></asp:Label>
                                                    <asp:Label ID="lbl_3dayTargetExp28DaysCompStr" BorderWidth="0px" runat="server" Style="text-align: center" Text=""></asp:Label>
                                                    <asp:Label ID="lbl_3dayTargetMeanStr" BorderWidth="0px" runat="server" Style="text-align: center" Text=""></asp:Label>
                                                     <asp:TextBox ID="txt_3dayStr"  Width="80px"  runat="server"  MaxLength="50" Text=""></asp:TextBox>
                                               </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="7 day">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_7daySch" runat="server" />
                                                    <asp:Label ID="lbl_7dayStr" BorderWidth="0px" runat="server" Style="text-align: center" Text="7 day str"></asp:Label>
                                                    <asp:Label ID="lbl_7dayExp28DaysCompStr" BorderWidth="0px" runat="server" Style="text-align: center" Text=""></asp:Label>
                                                    <asp:Label ID="lbl_7dayTargetExp28DaysCompStr" BorderWidth="0px" runat="server" Style="text-align: center" Text=""></asp:Label>
                                                    <asp:Label ID="lbl_7dayTargetMeanStr" BorderWidth="0px" runat="server" Style="text-align: center" Text=""></asp:Label>
                                                       <asp:TextBox ID="txt_7dayStr"  Width="80px"  runat="server"  MaxLength="50" Text=""></asp:TextBox>
                                              </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="28 day">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_28daySch" runat="server" />
                                                    <asp:Label ID="lbl_28dayStr" BorderWidth="0px" runat="server" Style="text-align: center" Text="28 day str"></asp:Label>
                                                           <asp:TextBox ID="txt_28dayStr"  Width="80px"  runat="server"  MaxLength="50" Text=""></asp:TextBox>
                                          </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="56 day">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_56daySch" runat="server" />
                                                    <asp:Label ID="lbl_56dayStr" BorderWidth="0px" runat="server" Style="text-align: center" Text="56 day str"></asp:Label>
                                                          <asp:TextBox ID="txt_56dayStr"  Width="80px"   runat="server" MaxLength="50"  Text=""></asp:TextBox>
                                           </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="90 day">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_90daySch" runat="server" />
                                                    <asp:Label ID="lbl_90dayStr" BorderWidth="0px" runat="server" Style="text-align: center" Text="90 day str"></asp:Label>
                                                          <asp:TextBox ID="txt_90dayStr"  Width="80px"  runat="server" MaxLength="50" Text=""></asp:TextBox>
                                           </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Other day">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_OtherdaySch" runat="server" />
                                                    <asp:Label ID="lbl_OtherdayStr" BorderWidth="0px" runat="server" Style="text-align: center" Text="Other day str"></asp:Label>
                                                        <asp:TextBox ID="txt_OtherdayStr" Width="80px"  runat="server" MaxLength="50" Text=""></asp:TextBox>
                                             </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblApproveTrial" runat="server" Text="Approve Trial"></asp:Label>&nbsp;&nbsp;
                                </td>
                                <td >
                                    <asp:CheckBox ID="chk_ApproveTrial" runat="server" />
                                </td>
                                 <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    &nbsp;
                                    <asp:Label ID="lblTrialInfoMessage" runat="server" ForeColor="#990033" Text="lblMsg" Visible="False"></asp:Label>
                                </td>
                                <td colspan="2" align="right">
                                    <asp:LinkButton ID="lnkSaveOtherInfo" runat="server" CssClass="LnkOver" OnClick="lnkSaveOtherInfo_Click"
                                        Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V1">Save</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkMDLetter" runat="server" CssClass="LnkOver" OnClick="lnkMDLetter_Click"
                                        Style="text-decoration: underline; font-weight: bold;" ValidationGroup="V1" Enabled="false">MD Letter</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkCancelOtherInfo" runat="server" CssClass="LnkOver" OnClick="lnkCancelOtherInfo_Click"
                                        Style="text-decoration: underline; font-weight: bold;">Cancel</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:Panel>
    </div>
    <script type="text/javascript">

        function checkDecimal(x) {
            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 46))) {
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
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
