<%@ Page Title="Trial MD Letter" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" Theme="duro" CodeBehind="MixDesignLetter.aspx.cs" Inherits="DESPLWEB.MixDesignLetter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table width="100%">
                <tr>
                    <td style="width: 15%">
                        <asp:RadioButton ID="optMDL" Text="MDL" GroupName="G1" runat="server" AutoPostBack="true"
                            OnCheckedChanged="optMDL_CheckedChanged" />&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="optFinal" Text="Final" GroupName="G1" runat="server" AutoPostBack="true"
                            OnCheckedChanged="optFinal_CheckedChanged" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOtherPendingRpt" Width="150px" runat="server" >
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkFetch" runat="server" Font-Bold="true" OnClick="lnkFetch_Click">Fetch</asp:LinkButton>
                    </td>
                    <td >
                        <asp:Label ID="Label1" runat="server" Text="Record No"></asp:Label>
                    </td>
                    <td >
                        <asp:TextBox ID="txt_RecType" Style="text-align: center" Width="41px" runat="server"
                            ReadOnly="true"></asp:TextBox>
                        <asp:TextBox ID="txt_RefNo" Width="100px" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblTrialName" Font-Bold="true" Font-Underline="true" runat="server"
                            Text=""></asp:Label>
                    </td>
                    
                    <td >
                        <asp:LinkButton ID="lnkSievePrnt" OnCommand="lnkSievePrnt_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Sieve Analysis</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkCoverShtPrnt" Font-Bold="true" Font-Underline="true" Visible="false"
                            runat="server" OnCommand="lnkCoverShtPrnt_Click"> Cover Sheet </asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkMoisuteCorrPrnt" Font-Bold="true" Font-Underline="true" Visible="false"
                            runat="server" OnClick="lnkMoisuteCorrPrnt_Click"> Moisture Correction </asp:LinkButton>
                    </td>
                    <td>
                        <asp:Label ID="lbl_TrialId" Visible="false" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblReportType" Visible="false" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblStatus" Visible="false" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="180px"
                Width="940px" BorderColor="AliceBlue">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdMD" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin">
                            <Columns>
                                <asp:TemplateField HeaderText=" ">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_MaterialName" Width="120px" BorderWidth="0px" runat="server" />
                                        <asp:Label ID="lbl_MaterialNameActual" Width="120px" BorderWidth="0px" runat="server"
                                            Visible="false" />
                                        <asp:Label ID="lbl_MaterialId" Width="120px" BorderWidth="0px" runat="server" Visible="false" />
                                    </ItemTemplate>
                                    <ItemStyle Width="120px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Description" Width="400px" BorderWidth="0px" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="400px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Per m3" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_PerM3" Width="120px" BorderWidth="0px" runat="server" ReadOnly="true"
                                            CssClass="caltextbox"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Per 50 kg Cement" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Per50Kg" BorderWidth="0px" Width="120px" runat="server" ReadOnly="true"
                                            CssClass="caltextbox"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="120px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Volumetric" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Volume" BorderWidth="0px" Width="120px" runat="server" ReadOnly="true"
                                            CssClass="caltextbox"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="120px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <div id="div1" style="height: 1px">                
                <asp:Button Text="Trial Information" ID="Tab_OtherInfo" runat="server" OnClick="Tab_OtherInfo_Click"
                    CssClass="Initiative" />
                <asp:Button Text="Cube Compressive Strength" ID="Tab_CubeCompStr" runat="server"
                    OnClick="Tab_CubeCompStr_Click" CssClass="Initiative" />
            </div>
            <asp:MultiView ID="MainView" runat="server">                
                <asp:View ID="View_OtherInfo" runat="server">
                    <asp:Panel ID="Pnl_OtherInfo" runat="server" CssClass="Pnlstyle" ScrollBars="Auto"
                        Height="130px" Width="940px" >
                        <table width="100%">
                           <tr>
                               <td colspan="7">
                                   <asp:CheckBox ID="chkAdmixtureInKg" Text="Admixture in Kg" runat="server" Visible="false" OnCheckedChanged="chkAdmixtureInKg_CheckedChanged" AutoPostBack="true"/>
                                   &nbsp;&nbsp;&nbsp;
                                   <asp:TextBox ID="txtAdmixtureInKg" Text="" Width="70px" runat="server" Style="text-align: center" ReadOnly="true" Visible="false"></asp:TextBox>
                               </td>
                            </tr>
                           <tr>
                                 <td style="width:10%">
                                    <asp:Label ID="lblBatching" runat="server" Text="Batching"></asp:Label>&nbsp;&nbsp;
                                </td>
                                <td style="width:25%">
                                    <asp:DropDownList ID="ddlBatching" runat="server" Width="150px" AutoPostBack="true"
                                        onselectedindexchanged="ddlBatching_SelectedIndexChanged">
                                        <asp:ListItem Text="Weight Batching" />
                                        <asp:ListItem Text="Volume Batching" />
                                    </asp:DropDownList>
                                </td>
                                <td style="width:6%">
                                    <asp:Label ID="lblYield" runat="server" Text="Yield"></asp:Label>&nbsp;&nbsp;
                                </td>
                                <td style="width:20%">
                                    <asp:TextBox ID="txtYield" Text="" Width="100px" runat="server" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width:20%">
                                    <asp:Label ID="lblWtOfConcreteInCylinder" runat="server" Text="Wt. of Concrete in Cylinder (Kg)"></asp:Label>&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtWtOfConcreteInCylinder" Text="" Width="100px" 
                                        runat="server" Style="text-align: center" ReadOnly="true"></asp:TextBox>
                                </td>
                                 <td>
                                    <asp:CheckBox ID="chkFlow" runat="server" Text="Flow" />
                                </td>
                            </tr>
                           <tr>
                               <td colspan="7">
                                    &nbsp;
                               </td>
                            </tr>
                           <tr>
                               <td colspan="7">
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
                                                    <asp:Label ID="txt_0" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="30">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_30" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="60">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_60" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="90">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_90" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="120">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_120" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="150">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_150" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="180">
                                                <ItemTemplate>
                                                    <asp:Label ID="txt_180" Width="80px" BorderWidth="0px" runat="server" Style="text-align: center"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View_CubeCompStr" runat="server">
                    <asp:Panel ID="PnlCubeCast" CssClass="Pnlstyle" ScrollBars="Auto" runat="server" Width="940px" Height="120px">
                            <table width="100%">
                                <tr>
                                    <td style="width:10%">
                                        <asp:Label ID="Label2" runat="server" Text="Kind Attention"></asp:Label>
                                    </td>
                                    <td style="width:80%">
                                        <asp:TextBox ID="txt_KindAttention" Width="250px" runat="server"></asp:TextBox>
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
                                    <asp:TemplateField HeaderText="Expected 28 days Comp Strength" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Exp28DaysCompStr" BorderWidth="0px" Width="130px" runat="server" Style="text-align: right"
                                                ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Target Expected 28 days Comp Strength" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_TargetExp28DaysCompStr" BorderWidth="0px" Width="130px" runat="server" Style="text-align: right"
                                                ReadOnly="true"></asp:TextBox>
                                            <asp:Label ID="lbl_StandardError" BorderWidth="0px" runat="server" Visible="false"/>
                                            <asp:Label ID="lbl_RValue" BorderWidth="0px" runat="server" Visible="false"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Target Mean Strength" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_TargetMeanStr" BorderWidth="0px" Width="130px" runat="server" Style="text-align: right"
                                                ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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
                <tr>
                    <td width="10%">
                        <asp:CheckBox ID="chkWitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chkWitnessBy_CheckedChanged" />
                        <asp:Label ID="lblWitnessBy" runat="server" Text="Witness By"></asp:Label>
                    </td>
                    <td width="17%">
                        <asp:TextBox ID="txtWitnessBy" Visible="false" runat="server" Width="140px"></asp:TextBox>
                    </td>
                    <td width="8%" style="text-align: right">
                        <asp:Label ID="lblTestdBy" runat="server" Text="Tested By"></asp:Label>
                    </td>
                    <td width="17%" align="center">
                        <asp:DropDownList ID="ddlTestdBy" runat="server" Width="140px">
                        </asp:DropDownList>
                    </td>
                    <td width="8%" style="text-align: right">
                        <asp:Label ID="lblApprdBy" runat="server" Text="Approved By"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:DropDownList ID="ddlApprdBy" runat="server" Width="140px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblEntdChkdBy" runat="server" Text="Checked By"  Visible="false"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtEntdChkdBy" runat="server" Width="140px" ReadOnly="true" Visible="false"></asp:TextBox>
                    </td>
                    <td align="right">
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
</asp:Content>
