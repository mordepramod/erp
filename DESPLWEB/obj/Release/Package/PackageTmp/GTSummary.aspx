<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="GTSummary.aspx.cs" Inherits="DESPLWEB.GTSummary" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <div style="height: 480px; margin: 0 auto; width: 97%; padding: 14px; border: solid 1px #b7ddf2;
        background: #ebf4fb;">
    <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
        Style="background-color: #ECF5FF;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="height: 5px" align="right">
                    <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                        ImageAlign="Right" />
                    &nbsp;&nbsp;
                </div>
                <asp:Panel ID="pnlDetails" runat="server" Width="942px" BorderWidth="1px">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 92px">
                                <asp:Label ID="lblRefNo" runat="server" Text="Reference No :"></asp:Label>
                            </td>
                            <td style="width: 235px">
                                <asp:TextBox ID="txtRefNo" runat="server" ReadOnly="true" Width="228px" BackColor="LightGray"
                                    ></asp:TextBox>
                            </td>
                            <td style="width: 82px">
                                <asp:Label ID="lblClientName" runat="server" Text="Client Name :"></asp:Label>
                            </td>
                            <td style="width: 200px">
                                <asp:TextBox ID="txtClientmName" runat="server" ReadOnly="true" Width="185px" BackColor="LightGray"></asp:TextBox>
                            </td>
                            <td style="width: 69px">
                                <asp:Label ID="lblSiteName" runat="server" Text="Site Name :"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSiteName" runat="server" Width="235px" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="height: 2px;">
                    </div>
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Panel ID="panel3" runat="server" BorderWidth="1px" Height="30px" BorderColor="ActiveBorder"
                                    BackColor="#9bcaec" Width="100%">
                                    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="RdbDataForList"
                                        OnSelectedIndexChanged="RdbDataForList_SelectedIndexChanged" AutoPostBack="true"
                                        CssClass="RdbList" Font-Bold="true" Height="31px" RepeatLayout="Flow" 
                                        Width="925px">
                                        <asp:ListItem Text="Enquiry Details"></asp:ListItem>
                                        <asp:ListItem Text="Expenses"></asp:ListItem>
                                        <asp:ListItem Text="Work Visit"></asp:ListItem>
                                        <asp:ListItem Text="Notes"></asp:ListItem>
                                        <asp:ListItem Text="Individual BH - Summary"></asp:ListItem>
                                        <asp:ListItem Text="Machine Log"></asp:ListItem>
                                        <asp:ListItem Text="Operator Payment"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>                  
                    <div style="margin-left: 830px;">
                        <asp:LinkButton ID="lnkViewCashReceipt_Entry" Font-Size="Small" runat="server" Font-Underline="true">View Cash Details</asp:LinkButton>
                    </div>
                </asp:Panel>
                <div style="height: 5px">
                    &nbsp;&nbsp;
                </div>
                <asp:Panel ID="PnlMainData" runat="server" Width="942px" BorderWidth="1px">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlGTSummaryExpenses" runat="server" Height="337px" Width="100%" Visible="false">
                                    <div style="height: 3px;">
                                    </div>
                                    <asp:Panel ID="Panel2" runat="server" Height="300px" Width="100%" ScrollBars="Auto">
                                        <asp:GridView ID="grdGTSummary" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                            Width="60%">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="~/Images/AddNewitem.jpg" ID="imgBtnAddRow" runat="server"
                                                            Width="18px" OnClick="imgBtnAddRow_Click" ImageAlign="Middle" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="~/Images/DeleteItem.png" ID="imgBtnDeleteRow" runat="server"
                                                            Width="16px" OnClick="imgBtnDeleteRow_Click" ImageAlign="Middle" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Particular" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtParticular" runat="server" CssClass="ac" BorderWidth="0" MaxLength="200"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="255px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="ac" onkeyup="checkDecimal(this);"
                                                            MaxLength="10" AutoPostBack="true" OnTextChanged="txtAmount_TextChanged" BorderWidth="0"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="255px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                    <div style="height: 5px;">
                                    </div>
                                    <asp:Label runat="server" ID="lblTotalAmt" Text="Total Amount :" Font-Bold="true"></asp:Label>
                                    <asp:Label runat="server" ID="lbltotalAmtResult" Text="0" Font-Bold="true"></asp:Label>
                                </asp:Panel>
                                <asp:Panel ID="PnlEnq" runat="server" Height="337px" Width="100%" ScrollBars="Auto"
                                    Visible="true">
                                    <table>
                                        <tr>
                                            <td style="height: 25px">
                                                <asp:Label ID="lblEnquiryDetails" runat="server" Text="Enquiry Details : -" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td style="width: 337px; height: 25px">
                                            </td>
                                            <td style="height: 25px">
                                                <asp:Label ID="lblReportBillDetails" runat="server" Text="Report and Bill Details : -"
                                                    Font-Bold="True"></asp:Label>
                                            </td>
                                            <td style="height: 25px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAddress" runat="server" Text="Address :"></asp:Label>
                                            </td>
                                            <td style="width: 337px">
                                                <asp:TextBox ID="txtAddress" runat="server" Width="230px" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEnquiryNo" runat="server" Text="Enquiry No. :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEnquiryNo" runat="server" Width="230px" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLocation" runat="server" Text="Location :"></asp:Label>
                                            </td>
                                            <td style="width: 337px">
                                                <asp:TextBox ID="txtLocation" runat="server" Width="230px"  ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblReportNo" runat="server" Text="Report No :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtReportNo" runat="server" Width="230px"  ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEnquiryDate" runat="server" Text="Enquiry Date :"></asp:Label>
                                            </td>
                                            <td style="width: 337px">
                                                <asp:TextBox ID="txtEnquiryDate" runat="server" Width="230px" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblYear" runat="server" Text="Year :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtYear" runat="server" Width="230px" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblReference" runat="server" Text="Reference :"></asp:Label>
                                            </td>
                                            <td style="width: 337px">
                                                <asp:TextBox ID="txtReference" runat="server" Width="230px"  ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMonth" runat="server" Text="Month :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMonth" runat="server" Width="230px"  ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblContactPerson" runat="server" Text="Contact Person :"></asp:Label>
                                            </td>
                                            <td style="width: 337px">
                                                <asp:TextBox ID="txtContactPerson" runat="server" Width="230px"  ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBillNo" runat="server" Text="Bill No :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBillNo" runat="server" Width="230px"  ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblContactNo" runat="server" Text="Contact No :"></asp:Label>
                                            </td>
                                            <td style="width: 337px">
                                                <asp:TextBox ID="txtContactNo" runat="server" Width="230px"  ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBillDate" runat="server" Text="Bill Date :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBillDate" runat="server" Width="230px"  ReadOnly="true" BackColor="LightGray"></asp:TextBox>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblStatusOfSite" runat="server" Text="Status of Site :"></asp:Label>
                                            </td>
                                            <td style="width: 337px">
                                                <asp:DropDownList ID="ddlStatus" runat="server">
                                                    <asp:ListItem>Open</asp:ListItem>
                                                    <asp:ListItem>Close</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAmount" runat="server" Text="Amount (excl tax) :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAmountTax" runat="server" Width="230px"  ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEmail" runat="server" Text="E-mail Id :"></asp:Label>
                                            </td>
                                            <td style="width: 337px">
                                                <asp:TextBox ID="txtEmail" runat="server" Width="230px" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblServiceTax" runat="server" Text="Service Tax :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtServiceTax" runat="server" Width="230px"  ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="width: 337px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text="Total Amount :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotalAmount" runat="server" Width="230px"  ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblWater" runat="server" Text="Water :"></asp:Label>
                                            </td>
                                            <td style="width: 337px">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblProposalDetails" runat="server" Text="Proposal Details :" Font-Underline="True"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDrilling" runat="server" Text="For Drilling :"></asp:Label>
                                            </td>
                                            <td style="width: 337px">
                                                <asp:TextBox ID="txtDrilling" runat="server" Width="230px" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblProposalNo" runat="server" Text="Proposal No. :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtProposalNo" runat="server" Width="230px"  ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCharges" runat="server" Text="Charges :"></asp:Label>
                                            </td>
                                            <td style="width: 337px">
                                                <asp:TextBox ID="txtCharges" runat="server" Width="230px" MaxLength="10"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDate" runat="server" Text="Date :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDate" runat="server" Width="230px"  ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlIndividualSummary" runat="server" Height="332px" Width="100%" Visible="false">
                                    <div style="margin-left: 700px; margin-top: 5px; margin-bottom: 10px;">
                                        <asp:Label runat="server" Text="No. of Bore Holes :" ID="lblBoreHoles"></asp:Label>
                                        <asp:TextBox ID="txtBoreHoles" runat="server" CssClass="ac" Width="100px" AutoPostBack="true"
                                            OnTextChanged="txtBoreHoles_TextChanged"></asp:TextBox>
                                    </div>
                                    <asp:Panel ID="pnlIS" runat="server" Height="290px" Width="100%" ScrollBars="Auto">
                                        <asp:GridView ID="grdIndividualSummary" runat="server" AutoGenerateColumns="False"
                                            OnRowDataBound="grdIndividualSummary_OnRowDataBound" SkinID="gridviewSkin1" Width="97%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="BH No.">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBHNo" runat="server" CssClass="ac" BorderWidth="0" ReadOnly="true"
                                                            BackColor="LightGray"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="80px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Drilling m/c no.">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlDrillingMachineNo" runat="server" AutoPostBack="true" BackColor="Transparent"
                                                            OnSelectedIndexChanged="ddlDrillingMachineNo_SelectedIndexChanged" BorderWidth="0">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="125px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="m/c operator">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMachineOperator" runat="server" CssClass="ac" ReadOnly="true"
                                                            BackColor="LightGray" BorderWidth="0"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="140px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="from date">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtfromDate" runat="server" Width="135px" BorderWidth="0" CssClass="ac" onkeyup="checkDate(this);"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtfromDate">
                                                        </cc1:CalendarExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="to date">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txttoDate" runat="server" Width="135px" BorderWidth="0" CssClass="ac" onkeyup="checkDate(this);"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txttoDate">
                                                        </cc1:CalendarExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="total depth(Meter)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTotalDepth" runat="server" CssClass="ac" BorderWidth="0" onkeyup="checkDecimal(this)" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="140px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="by (durocrete/oth)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtByDurocreteOth" runat="server" CssClass="ac" ReadOnly="true"
                                                            BackColor="LightGray" BorderWidth="0"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="140px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </asp:Panel>
                                <asp:Panel ID="pnlWorkVisit" runat="server" Height="337px" Width="100%" ScrollBars="Auto"
                                    Visible="false">
                                    <asp:GridView ID="grdWorkVisit" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                        Width="90%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ImageUrl="~/Images/AddNewitem.jpg" ID="BtnAddRowWork" runat="server"
                                                        Width="18px" OnClick="BtnAddRowWork_Click" ImageAlign="Middle" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ImageUrl="~/Images/DeleteItem.png" ID="BtnDeleteRowWork" runat="server"
                                                        Width="16px" OnClick="BtnDeleteRowWork_Click" ImageAlign="Middle" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="visit on Date" Visible="true">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtVisitOnDate" runat="server" CssClass="ac" BorderWidth="0"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                                        Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtVisitOnDate">
                                                    </cc1:CalendarExtender>
                                                </ItemTemplate>
                                                <ControlStyle Width="150px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="visit by" Visible="true">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtVisitBy" runat="server" BorderWidth="0" MaxLength="200"></asp:TextBox>
                                                </ItemTemplate>
                                                <ControlStyle Width="288px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="purpose" Visible="true">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPurpose" runat="server" BorderWidth="0" MaxLength="200"></asp:TextBox>
                                                </ItemTemplate>
                                                <ControlStyle Width="430px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                                <asp:Panel ID="pnlNotes" runat="server" Height="337px" Width="100%" ScrollBars="Auto"
                                    Visible="false">
                                    <div style="height: 4px;">
                                    </div>
                                    <asp:GridView ID="grdNotes" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                        Width="60%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ImageUrl="~/Images/AddNewitem.jpg" ID="BtnAddRowNotes" runat="server"
                                                        Width="18px" OnClick="BtnAddRowNotes_Click" ImageAlign="Middle" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ImageUrl="~/Images/DeleteItem.png" ID="BtnDeleteRowNotes" runat="server"
                                                        Width="16px" OnClick="BtnDeleteRowNotes_Click" ImageAlign="Middle" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Repairs" Visible="true">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRepairs" runat="server" BorderWidth="0" MaxLength="200"></asp:TextBox>
                                                </ItemTemplate>
                                                <ControlStyle Width="300px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Missalenious" Visible="true">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMissalenious" runat="server" BorderWidth="0" MaxLength="200"></asp:TextBox>
                                                </ItemTemplate>
                                                <ControlStyle Width="300px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                                <asp:Panel ID="pnlMachineLog" runat="server" Height="337px" Width="100%" Visible="false">
                                    <br />
                                    <asp:Panel ID="pnML" runat="server" Height="300px" Width="100%" ScrollBars="Auto">
                                        <asp:GridView ID="grdMachineLog" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                                            Width="90%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Machine No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMachineNo" runat="server" BorderWidth="0" CssClass="ac" BackColor="LightGray"
                                                            ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="80px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No. of BH" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtNoBH" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                            BackColor="LightGray"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="70px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDate" runat="server" BorderWidth="0" CssClass="ac" ReadOnly="true"
                                                            BackColor="LightGray"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="200px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Operator" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtOperator" runat="server" BorderWidth="0" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="240px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Owner" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtOwner" runat="server" BorderWidth="0" ReadOnly="true" BackColor="LightGray"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="240px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </asp:Panel>
                                <asp:Panel ID="pnlOperatorPayment" runat="server" Height="337px" Width="100%" Visible="false">
                                    <div style="margin-left: 600px;">
                                        <asp:Label Text="Payment To:" runat="server" ID="lblPayment"></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlPayment" AutoPostBack="true" Width="150px">
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkPrintCreditNote" Font-Size="Small" runat="server" Font-Underline="true">Print Credit Note</asp:LinkButton>
                                    </div>
                                    <br />
                                    <asp:Panel ID="pnlOP" runat="server" Height="300px" Width="100%" ScrollBars="Auto">
                                        <asp:GridView ID="grdOperatorPayment" runat="server" AutoGenerateColumns="False"
                                            SkinID="gridviewSkin1" OnRowDataBound="grdOperatorPayment_OnRowDataBound" Width="97%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Operator Name" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtOperatorName" runat="server" BorderWidth="0" ReadOnly="true"
                                                            BackColor="LightGray"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="150px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="BH No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBHNo" runat="server" BorderWidth="0" ReadOnly="true" BackColor="LightGray"
                                                            CssClass="ac" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="70px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Meter" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMeter" runat="server" BorderWidth="0" CssClass="ac" BackColor="LightGray"
                                                            MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="80px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cal Amt." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCalAmt" runat="server" BorderWidth="0" ReadOnly="true" BackColor="LightGray"
                                                            CssClass="ac" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="90px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Additional Amt." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAdditionalAmt" runat="server" BorderWidth="0" CssClass="ac" MaxLength="10"
                                                            onkeyup="checkDecimal(this);"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="90px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Amt." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTotalAmt" runat="server" BorderWidth="0" ReadOnly="true" BackColor="LightGray"
                                                            CssClass="ac" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="90px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Approved Amt." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtApprovedAmt" runat="server" BorderWidth="0" CssClass="ac" MaxLength="10"
                                                            onkeyup="checkDecimal(this);"> </asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="90px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Print Date" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPrintDate" runat="server" BorderWidth="0" BackColor="LightGray"
                                                            CssClass="ac" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="80px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payment To" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlPaymentTo" runat="server" BorderWidth="0" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="140px" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div>
                    <table>
                        <tr>
                            <td style="width: 850px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkSave" Font-Size="Small" runat="server" Font-Underline="true"
                                    OnClick="lnkSave_Click">Save</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkExit" runat="server" Font-Size="Small" 
                                    Font-Underline="true" onclick="lnkExit_Click">Exit</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
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
                    alert("Only Numeric Values Allowed");

                    return false;
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
        function checkDate(x) {

            var s_len = x.value.length;
            var s_charcode = 0;
            for (var s_i = 0; s_i < s_len; s_i++) {
                s_charcode = x.value.charCodeAt(s_i);
                if (!((s_charcode >= 48 && s_charcode <= 57) || (s_charcode == 47))) {
                    x.value = '';
                    x.focus();
                    alert("Please enter valid date.");

                    return false;
                }

            }
            return true;
        }
           
           
    </script>
</asp:Content>
