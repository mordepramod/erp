<%@ Page Title="Aggregate Entry Report" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="Aggregate_Report.aspx.cs" Inherits="DESPLWEB.Aggregate_Report"
    Theme="duro" %>

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
                    <tr >
                        <td>
                            <asp:Label ID="lbl_OtherPendingMF" runat="server" Text="Other Pending Reports" Visible="false"></asp:Label>
                        </td>
                        <td >
                            <asp:DropDownList ID="ddl_OtherPendingRptMF" Width="205px" runat="server" Visible="false">
                            </asp:DropDownList>
                        </td>
                        <td >
                            
                        </td>
                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td width="15%">
                            <asp:Label ID="lbl_OtherPending" runat="server" Text="Other Pending Reports"></asp:Label>
                        </td>
                        <td width="30%">
                            <asp:DropDownList ID="ddl_OtherPendingRpt" Width="205px" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_MaterialName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:LinkButton ID="lnk_Fetch" runat="server" Style="text-decoration: underline;"
                                Font-Bold="true" OnClick="lnk_Fetch_Click">Fetch</asp:LinkButton>
                        </td>
                        <td width="12%" align="right">
                            <asp:Label ID="Label9" runat="server" Text="Report No"></asp:Label>
                        </td>
                        <td width="30%" align="center">
                            <asp:TextBox ID="txt_RecType" Width="50px" ReadOnly="true" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txt_ReportNo" runat="server" ReadOnly="true" Width="140px"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblReportNo" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblRecordNo" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblEntry" runat="server" Text="Enter" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td width="12%">
                            <asp:Label ID="Label1" runat="server" Text="Reference No"></asp:Label>
                        </td>
                        <td width="22%">
                            <asp:TextBox ID="txt_ReferenceNo" Width="200px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td width="15%" align="right">
                            <asp:Label ID="Label2" runat="server" Text="Date Of Testing"></asp:Label>
                        </td>
                        <td width="30%" align="center">
                            <asp:TextBox ID="txt_DateOfTesting" Width="200px" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_DateOfTesting">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_DateOfTesting"
                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                            </asp:MaskedEditExtender>
                        </td>
                        <td style="text-align: right">
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
                            <asp:Label ID="Label3" runat="server" Text="Supplier Name"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txt_SupplierName" Width="200px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Material Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_MaterialName" ReadOnly="true" Width="200px" runat="server"></asp:TextBox>
                            <asp:Label ID="lbl_MaterialId" runat="server" Visible="false" Text=""></asp:Label>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label17" runat="server" Text="Sample Condition"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txt_SampleCondition" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td align="center">
                            <asp:CheckBox ID="chkComplete" Text="Complete" runat="server" ForeColor="Brown" Font-Bold="true" Visible="false"/>
                        </td> 
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 1px">
                &nbsp;&nbsp;
            </div>
             <div>
                    <asp:Label ID="Label18" runat="server" Text="Nabl Scope"></asp:Label>

                    &nbsp;<asp:DropDownList ID="ddl_NablScope" AutoPostBack="true" Width="80px" runat="server">
                        <asp:ListItem Text="--Select--" />
                        <asp:ListItem Text="F" />
                        <asp:ListItem Text="NA" />
                    </asp:DropDownList>
                    &nbsp;&nbsp;
                                    <asp:Label ID="Label23" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="ddl_NABLLocation" runat="server" Width="80px" Enabled="true">
                        <asp:ListItem Value="0" Text="0" />
                        <asp:ListItem Value="1" Text="1" />
                        <asp:ListItem Value="2" Text="2" />
                    </asp:DropDownList>
                </div>
            <div style="height: 1px">
                <asp:Button Text="Sieve Analysis" ID="Tab_SieveAnalysis" runat="server" OnClick="Tab_SieveAnalysis_Click"
                    CssClass="Initiative" Enabled="false" />
                <asp:Button Text="Specific Gravity" ID="Tab_SpecGravity" runat="server" OnClick="Tab_SpecGravity_Click"
                    CssClass="Initiative" Enabled="false" />
                <asp:Button Text="Flakiness" ID="Tab_Flaki" runat="server" OnClick="Tab_Flaki_Click"
                    CssClass="Initiative" Enabled="false" />
                <asp:Button Text="Elongation" ID="Tab_Elong" runat="server" OnClick="Tab_Elong_Click"
                    CssClass="Initiative" Enabled="false" />
                <asp:Button Text="Impact Value" ID="Tab_Impval" runat="server" OnClick="Tab_Impval_Click"
                    CssClass="Initiative" Enabled="false" />
                <asp:Button Text="Crushing Value" ID="Tab_Crushval" runat="server" OnClick="Tab_Crushval_Click"
                    CssClass="Initiative" Enabled="false" />
                <asp:Button Text="Other Tests" ID="Tab_OtherTest" runat="server" OnClick="Tab_OtherTest_Click"
                    CssClass="Initiative" Enabled="false" />
                <asp:Button Text="Remarks" ID="Tab_Remark" runat="server" CssClass="Initiative" OnClick="Tab_Remark_Click" />
            </div>
            <asp:MultiView ID="MainView" runat="server">
                <asp:View ID="View_SieveAnalysis" runat="server">
                    <asp:Panel ID="Pnl_SeiveAnalysis" runat="server" CssClass="Pnlstyle">
                        <%-- <asp:Label ID="lbl_SATestName" Font-Bold="true"  runat="server" ></asp:Label> &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; --%>
                        <asp:Label ID="lbl_FM" Font-Bold="true" runat="server" Text="F.M    :"></asp:Label>
                        <asp:Label ID="lbl_FMres" Font-Bold="true" runat="server" Text=""></asp:Label>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grdAggtEntryRptInward" runat="server" AutoGenerateColumns="false"
                                    SkinID="gridviewSkin">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sieve Sizes" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_SieveSize" BorderWidth="0px" Style="text-align: left" Width="100px"
                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt(gms)" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_Wt" BorderWidth="0px" runat="server" Width="150px" CssClass="textbox"
                                                    MaxLength="50" onchange="javascript:checkNumber(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt Ret.%" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_WtRet" BorderWidth="0px" Width="150" CssClass="caltextbox" runat="server"
                                                    ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Cumu Wt Ret.%" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_CumWtRet" BorderWidth="0px" CssClass="caltextbox" Width="150px"
                                                    runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cumu Passing %" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_CumuPassing" BorderWidth="0px" CssClass="caltextbox" Width="150px"
                                                    runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IS Passing % Limits" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_Ispassinglmt" BorderWidth="0px" Width="150px" CssClass="ctextbox"
                                                    runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View_SpecificGravity" runat="server">
                    <asp:Panel ID="Pnl_SpecificGravity" runat="server" CssClass="Pnlstyle">
                        <%--<asp:Label ID="lbl_SpecTestName" Font-Bold="true"   runat="server" ></asp:Label> --%>
                        <asp:Label ID="lbl_Spec" Font-Bold="true" runat="server" Text="Specific Gravity :"></asp:Label>
                        <asp:Label ID="lbl_Specres" Font-Bold="true" runat="server" Text=""></asp:Label>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grd_SpecificGravity" runat="server" AutoGenerateColumns="false"
                                    SkinID="gridviewSkin">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="S.S.D" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_SSD" BorderWidth="0px" CssClass="textbox" Width="100px" runat="server"
                                                    onchange="javascript:checkNumber(this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Weight of the Bottle,Sample & Distilled Water" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_WtOfBottleSample" BorderWidth="0px" runat="server" Width="250px"
                                                    CssClass="textbox" MaxLength="50" onchange="javascript:checkNumber(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Weight of Bottle & Distilled Water" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_WtOfBottleDistilled" BorderWidth="0px" Width="200px" CssClass="textbox"
                                                    runat="server" onchange="javascript:checkNumber(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Oven Dry Weight" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_OvenDryWeight" BorderWidth="0px" CssClass="textbox" Width="150px"
                                                    runat="server" onchange="javascript:checkNumber(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Specific Gravity" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_SpecificGravity" BorderWidth="0px" CssClass="caltextbox" Width="150px"
                                                    runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br/> &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSGViewImages" runat="server" Style="text-decoration: underline;" ToolTip="View Images" OnClick="lnkSGViewImages_Click" Visible="false">View Images</asp:LinkButton>
                        <asp:Label ID="lblSSDImage1" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblWtBottleSampleImage1" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblOverDryWeightImage1" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblSSDImage2" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblWtBottleSampleImage2" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblOverDryWeightImage2" runat="server" Visible="false"></asp:Label>
                        <br/>                     
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View_FlakinessIndex" runat="server">
                    <asp:Panel ID="Pnl_FlakinessIndex" runat="server" CssClass="Pnlstyle">
                        <%-- <asp:Label ID="lbl_FlakiTestName" Font-Bold="true"  runat="server" ></asp:Label> &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; --%>
                        <asp:Label ID="lbl_Flaki" Font-Bold="true" runat="server" Text="Flakiness Index :"></asp:Label>
                        <asp:Label ID="lbl_Flakires" Font-Bold="true" runat="server" Text=""></asp:Label>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grd_Flakiness" runat="server" AutoGenerateColumns="false" SkinID="gridviewSkin">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sieve Sizes" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_SieveSize" BorderWidth="0px" Style="text-align: left" Width="90px"
                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Retained Mass on Sieve" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_RetainedmassonSieve" BorderWidth="0px" runat="server" Width="140px"
                                                    CssClass="textbox" MaxLength="50" onchange="javascript:checkNumber(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Of piceses passing " HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_picesespassing" BorderWidth="0px" Width="130" CssClass="textbox"
                                                    runat="server" onchange="javascript:checkNumber(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Gauge Passing Weight" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_GaugePassingWt" BorderWidth="0px" CssClass="caltextbox" Width="150px"
                                                    runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gauge Passing Sample Nos." HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_GaugePassingSampleNo" BorderWidth="0px" CssClass="caltextbox"
                                                    Width="200px" runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_Total" BorderWidth="0px" Width="150px" CssClass="ctextbox" runat="server"
                                                    ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View_Elongation" runat="server">
                    <asp:Panel ID="Pnl_Elongation" runat="server" CssClass="Pnlstyle">
                        <%-- <asp:Label ID="lbl_ElongTestName" Font-Bold="true"  runat="server" ></asp:Label> &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; --%>
                        <asp:Label ID="lbl_Elong" Font-Bold="true" runat="server" Text="Elongation Index   :"></asp:Label>
                        <asp:Label ID="lbl_Elongres" Font-Bold="true" runat="server" Text=""></asp:Label>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grd_Elongation" runat="server" AutoGenerateColumns="false" SkinID="gridviewSkin">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sieve Sizes" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_SieveSize" BorderWidth="0px" Style="text-align: left" Width="80px"
                                                    runat="server" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Retained Mass on Sieve" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_RetainedmassonSieve" BorderWidth="0px" runat="server" Width="140px"
                                                    CssClass="textbox" MaxLength="50" onchange="javascript:checkNumber(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Mass No. Of piceses retained " HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_picesesretained" BorderWidth="0px" Width="180" CssClass="textbox"
                                                    runat="server" onchange="javascript:checkNumber(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Gauge retained Weight" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_GaugeretainedWt" BorderWidth="0px" CssClass="caltextbox" Width="150px"
                                                    runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gauge retained Sample Nos." HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_GaugeretainedSampleNo" BorderWidth="0px" CssClass="caltextbox"
                                                    Width="180px" runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_Total" BorderWidth="0px" Width="130px" CssClass="ctextbox" runat="server"
                                                    ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View_Impact" runat="server">
                    <asp:Panel ID="Pnl_Impact" runat="server" CssClass="Pnlstyle">
                        <%--<asp:Label ID="lbl_ImpactTestName" Font-Bold="true"  runat="server" ></asp:Label> &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; --%>
                        <asp:Label ID="lbl_Impact" Font-Bold="true" runat="server" Text="Impact Value  :"></asp:Label>
                        <asp:Label ID="lbl_Impactres" Font-Bold="true" runat="server" Text=""></asp:Label>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grd_ImpactValue" runat="server" AutoGenerateColumns="false" SkinID="gridviewSkin">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnImpactAddRow" runat="server" OnCommand="imgBtnImpactAddRow_Click"
                                                    ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                    ToolTip="Add Row" />
                                            </ItemTemplate>
                                            <ItemStyle Width="18px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnImpactDeleteRow" runat="server" OnCommand="imgBtnImpactDeleteRow_Click"
                                                    ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                    ToolTip="Delete Row" />
                                            </ItemTemplate>
                                            <ItemStyle Width="16px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Initial Wt. of Sample" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_Initialwtsample" BorderWidth="0px" CssClass="textbox" Width="210px"
                                                    runat="server" onchange="javascript:checkNumber(this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Sample retained on 2.36 mm" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_WtSampleretained" BorderWidth="0px" runat="server" Width="210px"
                                                    CssClass="textbox" MaxLength="50" onchange="javascript:checkNumber(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Sample through on 2.36 mm" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_WtSamplepassing" BorderWidth="0px" Width="210" CssClass="caltextbox"
                                                    runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" % of Passing" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_Passing" BorderWidth="0px" CssClass="caltextbox" Width="210px"
                                                    runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View_Crushing" runat="server">
                    <asp:Panel ID="Pnl_Crushing" runat="server" CssClass="Pnlstyle">
                        <%-- <asp:Label ID="lbl_CrshingTestName" Font-Bold="true"  runat="server" ></asp:Label> &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; --%>
                        <asp:Label ID="lbl_Crushing" Font-Bold="true" runat="server" Text="Crushing Value :"></asp:Label>
                        <asp:Label ID="lbl_Crushingres" Font-Bold="true" Text="" runat="server"></asp:Label>
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grd_CrushValue" runat="server" AutoGenerateColumns="false" SkinID="gridviewSkin">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnCrushAddRow" runat="server" OnCommand="imgBtnCrushAddRow_Click"
                                                    ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                    ToolTip="Add Row" />
                                            </ItemTemplate>
                                            <ItemStyle Width="18px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnCrushDeleteRow" runat="server" OnCommand="imgBtnCrushDeleteRow_Click"
                                                    ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                    ToolTip="Delete Row" />
                                            </ItemTemplate>
                                            <ItemStyle Width="16px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Initial Wt. of Sample" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_Initialwtsample" BorderWidth="0px" CssClass="textbox" Width="140px"
                                                    runat="server" onchange="javascript:checkNumber(this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Sample retained on 2.36 mm" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_WtSampleretained" BorderWidth="0px" runat="server" Width="250px"
                                                    CssClass="textbox" MaxLength="50" onchange="javascript:checkNumber(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt. of Sample Passing through on 2.36 mm" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_WtSamplepassing" BorderWidth="0px" Width="250" CssClass="caltextbox"
                                                    runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" % of Passing" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_Passing" BorderWidth="0px" CssClass="caltextbox" Width="200px"
                                                    runat="server" ReadOnly="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View_OtherTest" runat="server">
                    <asp:Panel ID="Panel1" runat="server" CssClass="Pnlstyle">
                    <div style="float: left; height: 300px">
                        <asp:Panel ID="Pnl_LBD" runat="server" CssClass="Pnlstyle" Height="70px" Width="900px" Visible="false">
                            &nbsp;<asp:Label ID="lbl_LBDTestName" Font-Bold="true" Text="LBD" runat="server"></asp:Label>
                            <table width="100%">
                                <tr>
                                    <td style="width: 25%" align="left">
                                        <asp:Label ID="Label6" runat="server" Text="Total Weight of the Aggregate"></asp:Label>
                                    </td>
                                    <td style="width: 29%" align="left">
                                        <asp:TextBox ID="txt_TotalwtofAggregate" CssClass="textbox" Text="" Width="200px"
                                            runat="server" onchange="javascript:checkDecimal(this);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_LBDStatus" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label8" runat="server" Text="Volume of the Cylender"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_VolumeofCylender" CssClass="textbox" Text="" Width="200px" runat="server"
                                            onchange="javascript:checkNumber(this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 12%" align="left">
                                        <asp:Label ID="Label10" runat="server" Text="LBD"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_LBDresult" ReadOnly="true" Text="" CssClass="caltextbox" Width="202px"
                                            runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Pnl_WaterAbsorbtion" runat="server" CssClass="Pnlstyle" Height="70px"  Width="900px"
                            Visible="false">
                            &nbsp;<asp:Label ID="lbl_WTtestName" Font-Bold="true" Text="Water Absorption" runat="server"></asp:Label>
                            <table width="100%">
                                <tr>
                                    <td style="width: 25%" align="left">
                                        <asp:Label ID="Label11" runat="server" Text="SSD"></asp:Label>
                                    </td>
                                    <td style="width: 29%" align="left">
                                        <asp:TextBox ID="txt_SSDforWT" Width="200px" CssClass="textbox" Text="" runat="server"
                                            onchange="javascript:checkNumber(this);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_WtStatus" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label12" runat="server" Text="Oven Dry Weight"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_OvenDrywtforWT" CssClass="textbox" Text="" Width="200px" runat="server"
                                            onchange="javascript:checkNumber(this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 12%" align="left">
                                        <asp:Label ID="Label15" runat="server" Text="Water Absorption"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_WaterAbsorption" ReadOnly="true" CssClass="caltextbox" Text=""
                                            Width="202px" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Pnl_MoistureContent" runat="server" CssClass="Pnlstyle" Height="70px"  Width="900px"
                            Visible="false">
                            &nbsp;<asp:Label ID="lbl_MoisContentTestName" Font-Bold="true" Text="Moisture Content"
                                runat="server"></asp:Label>
                            <table width="100%">
                                <tr>
                                    <td style="width: 25%" align="left">
                                        <asp:Label ID="Label13" runat="server" Text="Initial Weight"></asp:Label>
                                    </td>
                                    <td style="width: 29%" align="left">
                                        <asp:TextBox ID="txt_InitialWt" CssClass="textbox" Text="" Width="200px" runat="server"
                                            onchange="javascript:checkNumber(this);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_MoistStatus" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%" align="left">
                                        <asp:Label ID="Label14" runat="server" Text="Dry Weight"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_DryWt" CssClass="textbox" Text="" Width="200px" runat="server"
                                            onchange="javascript:checkNumber(this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 12%" align="left">
                                        <asp:Label ID="Label16" runat="server" Text="Moisture Content"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_MoistureContent" ReadOnly="true" CssClass="caltextbox" Text=""
                                            Width="202px" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Pnl_SildContent" runat="server" CssClass="Pnlstyle" Height="70px"  Width="900px"
                            Visible="false">
                            &nbsp;<asp:Label ID="Label19" Font-Bold="true" Text="Silt Content" runat="server"></asp:Label>
                            <table width="100%">
                                <tr>
                                    <td style="width: 25%" align="left">
                                        <asp:Label ID="Label20" runat="server" Text="Initial Weight"></asp:Label>
                                    </td>
                                    <td style="width: 29%" align="left">
                                        <asp:TextBox ID="txt_InitialWtofSild" CssClass="textbox" Text="" Width="200px" runat="server"
                                            onchange="javascript:checkNumber(this);"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_SildStatus" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%" align="left">
                                        <asp:Label ID="Label21" runat="server" Text="Retained Weight on 75 micron Sieve"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_RetainedWtmicronSieve" CssClass="textbox" Text="" Width="200px"
                                            runat="server" onchange="javascript:checkNumber(this);"></asp:TextBox>
                                    </td>
                                    <td style="width: 12%" align="left">
                                        <asp:Label ID="Label22" runat="server" Text="Sild Content"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txt_SildContent" ReadOnly="true" CssClass="caltextbox" Text="" Width="202px"
                                            runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View_Remarks" runat="server">
                    <asp:Panel ID="Pnl_Remark" runat="server" CssClass="Pnlstyle">
                        <asp:GridView ID="grdAggtRemark" runat="server" SkinID="gridviewSkin">
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
                </asp:View>
            </asp:MultiView>
            <div style="height: 1px">
                &nbsp;&nbsp;
            </div>
            <table width="100%">
                <tr style="height: 30px">
                    <td width="15%">
                        <asp:CheckBox ID="chk_WitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chk_WitnessBy_CheckedChanged" />
                        <asp:Label ID="Label7" runat="server" Text="Witness By"></asp:Label>
                    </td>
                    <td width="22%">
                        <asp:TextBox ID="txt_witnessBy" Width="200px" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td width="20%" style="text-align: right">
                        <asp:Label ID="lbl_TestedBy" runat="server" Text="Tested By"></asp:Label>
                    </td>
                    <td>
                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                        <asp:DropDownList ID="ddl_TestedBy" Width="205px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="Lnk_Calculate" OnClick="Lnk_Calculate_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Cal</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" Visible="false" runat="server"
                              Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClick="lnk_Exit_Click" runat="server">Exit</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <style type="text/css">
        .Pnlstyle
        {
            border-style: groove;
            background-color: #E6E6E6;
            border-top-color: #E6E6E6;
            border-bottom-color: Teal;
            border-left-color: Teal;
            border-right-color: Teal;
            height: 260px;
            width: 942px;
            overflow: auto;
        }
        
        .Initiative
        {
            border-top-color: #ECF5FF;
            border-bottom-color: #ECF5FF;
            border-left-color: #ECF5FF;
            border-right-color: Black;
            background-color: #ECF5FF;
            padding: 1px 17px 1px 17px;
            float: left;
        }
        .Initiative:hover
        {
            color: Black;
            background-color: #FFAAC6;
        }
        .Click
        {
            float: left;
            display: block;
            background-color: #E6E6E6;
            border-style: solid;
            border-left-color: Teal;
            border-right-color: Teal;
            border-top-color: Teal;
            border-bottom-color: #E6E6E6;
            padding: 1px 17px 1px 17px;
            color: Black;
        }
        
        .DisableTab
        {
            border-top-color: #ECF5FF;
            border-bottom-color: #ECF5FF;
            border-left-color: #ECF5FF;
            border-right-color: Black;
            background-color: #ECF5FF;
            padding: 1px 17px 1px 17px;
            float: left;
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
</asp:Content>
