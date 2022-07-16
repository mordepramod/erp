<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="Other_Report.aspx.cs" Inherits="DESPLWEB.Other_Report" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 490px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="490px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <div style="height: 5px" align="right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                    ImageAlign="Right" />
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="Panel2" runat="server" Width="942px">
                <table style="width: 100%">
                    <tr>
                        <td width="10%">
                            <asp:Label ID="lbl_OtherPending" runat="server" Text="Other Pending Reports"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:DropDownList ID="ddl_OtherPendingRpt" Width="210px" runat="server">
                            </asp:DropDownList>
                            <asp:LinkButton ID="lnk_Fetch" runat="server" Style="text-decoration: underline;"
                                Font-Bold="true" OnClick="lnk_Fetch_Click">Fetch</asp:LinkButton>
                        </td>
                        <td width="12%" align="right">
                            <asp:Label ID="lbl_RptNo" runat="server" Text="Report No"></asp:Label>
                        </td>
                        <td width="20%" align="center">
                            <asp:TextBox ID="txt_RecordType" Width="50px" runat="server" ReadOnly="true"></asp:TextBox>
                            <asp:TextBox ID="txt_ReportNo" runat="server" Width="140px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td width="25%" align="right">
                            <asp:Label ID="lblEntry" runat="server" Text="Enter" Visible="false"></asp:Label>
                            <asp:Label ID="lblRecordNo" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblULRNo" runat="server" Text="ULR No : " Font-Bold="true" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Reference No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_ReferenceNo" Width="210px" ReadOnly="true" runat="server"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="Label10" runat="server" Text="Date Of Testing"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txt_DtOfTesting" Width="200px" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_DtOfTesting">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txt_DtOfTesting"
                                MaskType="Date" Mask="99/99/9999" AutoComplete="false" runat="server">
                            </asp:MaskedEditExtender>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblSection" runat="server" Text="Section"></asp:Label>
                            &nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlSection" Width="150px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Report For"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_ReportFor" AutoPostBack="true" OnSelectedIndexChanged="ddl_ReportFor_SelectedIndexChanged"
                                Width="216px" runat="server">
                                <asp:ListItem Text="---Other Testing---" Value="0" />
                                <asp:ListItem Text="Gypsum" Value="1" />
                                <asp:ListItem Text="Gypsum Chemical" Value="2" />
                                <asp:ListItem Text="Ground granulated blast furnance slag + cement" Value="3" />
                                <asp:ListItem Text="Plywood Test" Value="4" />
                                <asp:ListItem Text="Rock Core Compressive Strength" Value="5" />
                                <asp:ListItem Text="Evaluation Test of Jointing Mortar" Value="6" />
                                <asp:ListItem Text="Rebar Scanning" Value="7" />
                                <asp:ListItem Text="Water (Drinking Purpose)" Value="8" />
                                <asp:ListItem Text="Evaluation Test of Cementitious Mortar" Value="9" />
                                <asp:ListItem Text="Evaluation Test of Tile Adhesive" Value="10" />
                                <asp:ListItem Text="Concrete Carbonation Test" Value="11" />
                                <asp:ListItem Text="Half Cell Potentiometer" Value="12" />
                                <asp:ListItem Text="Reinforcement Splice Bar" Value="13" />
                                <asp:ListItem Text="Chemical Admixture" Value="14" />
                                <asp:ListItem Text="Deleterious Material" Value="15" />
                                <asp:ListItem Text="Laminated Door Shutter" Value="16" />
                                <asp:ListItem Text="Steel Chemical" Value="17" />
                                <asp:ListItem Text="Evaluation Test of Tile Adhesive" Value="18" />
                                <asp:ListItem Text="Coupler Chemical" Value="19" />
                                <asp:ListItem Text="Pull Off Test" Value="20" />
                                <asp:ListItem Text="Half Cell Potentiometer (New)" Value="21" />
                                <asp:ListItem Text="Concrete Carbonation Test (New)" Value="22" />
                                <asp:ListItem Text="PT Strand Cable" Value="23" />
                                <asp:ListItem Text="GGBS- Chemical" Value="24" />
                                <asp:ListItem Text="Fly Ash Chemical" Value="25" />
                                <asp:ListItem Text="Beam Flexural Strength" Value="26" />
                                <asp:ListItem Text="Concrete Water Permeability Test" Value="27" />
                                <asp:ListItem Text="Silica Fume" Value="28" />
                                <asp:ListItem Text="Sieve Analysis of Readymix Plaster " Value="29" />
                                <asp:ListItem Text="CLC BLOCKS DENSITY TEST" Value="30" />
                                <asp:ListItem Text="CLC BLOCKS WATER ABSORPTION TEST" Value="31" />
                                <asp:ListItem Text="CLC BLOCKS COMPRESSIVE TEST" Value="32" />
                                <%--<asp:ListItem Text="Modulus of Elasticity of Concrete using Extensometer" Value="34" />--%>
                                <asp:ListItem Text="Water Permeability Test" Value="35" />
                                <asp:ListItem Text="RAPID CHLORIDE PENETRATION TEST" Value="36" />
                            </asp:DropDownList>
                        </td>

                        <td align="right">
                            <asp:Label ID="Label8" runat="server" Text="Supplier Name"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txt_SupplierName" Width="200px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td align="right">
                            <%--<asp:Label ID="lblAddSign" runat="server" Text="Add Sign" Visible="true"></asp:Label>
                            <asp:DropDownList ID="ddlAddSign" runat="server" Visible="true" Width="135px">
                            </asp:DropDownList>--%>
                            <asp:Label ID="Label5" runat="server" Text="Template Format" Visible="true"></asp:Label>
                            <asp:DropDownList ID="ddl_Format" runat="server" Visible="true" Width="135px">
                                <asp:ListItem>Word</asp:ListItem>
                                <%--<asp:ListItem>Excel</asp:ListItem>--%>
                            </asp:DropDownList>
                            <br />
                            <asp:LinkButton ID="lnkTemplate" Font-Bold="false" Font-Underline="true" runat="server"
                                OnClick="lnkTemplate_Click">Generate Template</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Description" Width="210px" runat="server"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="Label3" runat="server" Text="Report(Max-7MB)"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:FileUpload ID="FileUpload1" Width="180px" runat="server" />  <%--onchange="__doPostBack('__Page', 'MyCustomArgument')"--%>
                            <asp:Label ID="lblFileName" runat="server" ForeColor="#990033" Text=""
                                Visible="False"></asp:Label>
                            &nbsp;<asp:Button ID="btnCancelDwnl" runat="server" OnClick="btnCancelDwnl_Click"
                                SkinID="button" Text="Cancel" Visible="False" Width="72px" />
                        </td>
                        <td align="right">
                            <asp:LinkButton ID="lnkRemove" Font-Bold="true" Font-Underline="true" runat="server"
                                OnClick="lnkRemove_Click">Remove</asp:LinkButton>
                            &nbsp;
                            <asp:LinkButton ID="lnkUpload" Font-Bold="true" Font-Underline="true" runat="server"
                                OnClick="lnkUpload_Click">Upload</asp:LinkButton>
                            &nbsp;
                            <asp:LinkButton ID="lnkDownload" Font-Bold="true" Font-Underline="true" runat="server"
                                OnClick="lnkDownload_Click">Download</asp:LinkButton>
                            <asp:LinkButton ID="lnlDownloadAllFiles" Font-Bold="true" Font-Underline="true" runat="server"
                                OnClick="lnlDownloadAllFiles_Click" Visible="false">Download All</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="PnlDetails" runat="server" ScrollBars="Auto" Height="190px" Width="942px"
                BorderColor="AliceBlue" BorderStyle="Ridge">
                <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>--%>
                        <table style="width: 100%">                            
                             <tr>
                                <td>
                                    <asp:CheckBox ID="chkBox8" Checked="true" runat="server" />
                                    <asp:Label ID="lblHeading8" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:GridView ID="grdDetail8" runat="server" SkinID="gridviewSkin"  OnRowDataBound="grdDetail8_RowDataBound" >
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnAddRow8" runat="server" OnCommand="imgBtnAddRow8_Click"
                                                        ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                        ToolTip="Add Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnDeleteRow8" runat="server" OnCommand="imgBtnDeleteRow8_Click"
                                                        ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                        ToolTip="Delete Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                               <asp:TemplateField>
                                                <ItemTemplate>
                                                 <asp:ImageButton ID="imgBtnMergeRow" runat="server" OnCommand="imgBtnMergeRow_Click"
                                                   ImageUrl="Images/m5.jpg" Height="16px" Width="16px" CausesValidation="false"
                                                   ToolTip="Merge Row" />
                                               </ItemTemplate>
                                              <ItemStyle Width="16px" />
                                           </asp:TemplateField>
                                       

                                            <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Test">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a1" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a1") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a2" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a2") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Test Result">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a3" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a3") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a4" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a4") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a5" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a5") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a6" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a6") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a7" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a7") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a8" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a8") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a9" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a9") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                               <asp:TemplateField Visible="false">
                                              <ItemTemplate>
                                               <asp:Label ID="lblMergFlag" runat="server" Text='<%#Eval("lblMergFlag") %>'></asp:Label>
                                                </ItemTemplate>
                                           </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkBox1" Checked="true" runat="server" />
                                    <asp:Label ID="lblHeading1" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdDetail1" runat="server" SkinID="gridviewSkin" OnRowDataBound="grdDetail1_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnAddRow1" runat="server" OnCommand="imgBtnAddRow1_Click"
                                                        ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                        ToolTip="Add Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnDeleteRow1" runat="server" OnCommand="imgBtnDeleteRow1_Click"
                                                        ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                        ToolTip="Delete Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                            
                                            <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Test">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a1" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a1") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a2" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a2") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Test Result">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a3" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a3") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a4" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a4") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a5" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a5") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a6" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a6") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a7" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a7") %>' OnTextChanged="txtNoOfSpecimens_TextChanged" AutoPostBack="true"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a8" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a8") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_a9" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_a9") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkBox2" Checked="true" runat="server" />
                                    <asp:Label ID="lblHeading2" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdDetail2" runat="server" Width="90%" SkinID="gridviewSkin" OnRowDataBound="grdDetail2_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnAddRow2" runat="server" OnCommand="imgBtnAddRow2_Click"
                                                        ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                        ToolTip="Add Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnDeleteRow2" runat="server" OnCommand="imgBtnDeleteRow2_Click"
                                                        ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                        ToolTip="Delete Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Test">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_b1" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_b1") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_b2" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_b2") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_b3" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_b3") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_b4" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_b4") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_b5" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_b5") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_b6" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_b6") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_b7" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_b7") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_b8" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_b8") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_b9" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_b9") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_b10" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_b10") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_b11" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_b11") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkBox3" Checked="true" runat="server" />
                                    <asp:Label ID="lblHeading3" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdDetail3" runat="server" Width="90%" SkinID="gridviewSkin" OnRowDataBound="grdDetail3_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnAddRow3" runat="server" OnCommand="imgBtnAddRow3_Click"
                                                        ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                        ToolTip="Add Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnDeleteRow3" runat="server" OnCommand="imgBtnDeleteRow3_Click"
                                                        ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                        ToolTip="Delete Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_c1" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_c1") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_c2" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_c2") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_c3" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_c3") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_c4" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_c4") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_c5" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_c5") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_c6" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_c6") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_c7" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_c7") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_c8" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_c8") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_c9" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_c9") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkBox4" Checked="true" runat="server" />
                                    <asp:Label ID="lblHeading4" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdDetail4" runat="server" Width="90%" SkinID="gridviewSkin"  OnRowDataBound="grdDetail4_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnAddRow4" runat="server" OnCommand="imgBtnAddRow4_Click"
                                                        ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                        ToolTip="Add Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnDeleteRow4" runat="server" OnCommand="imgBtnDeleteRow4_Click"
                                                        ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                        ToolTip="Delete Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_d1" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_d1") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_d2" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_d2") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_d3" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_d3") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_d4" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_d4") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_d5" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_d5") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_d6" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_d6") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_d7" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_d7") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_d8" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_d8") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_d9" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_d9") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkBox5" Checked="true" runat="server" />
                                    <asp:Label ID="lblHeading5" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdDetail5" runat="server" Width="90%" SkinID="gridviewSkin">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnAddRow5" runat="server" OnCommand="imgBtnAddRow5_Click"
                                                        ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                        ToolTip="Add Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnDeleteRow5" runat="server" OnCommand="imgBtnDeleteRow5_Click"
                                                        ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                        ToolTip="Delete Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_e1" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_e1") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_e2" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_e2") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_e3" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_e3") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_e4" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_e4") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_e5" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_e5") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkBox6" Checked="true" runat="server" />
                                    <asp:Label ID="lblHeading6" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdDetail6" runat="server" Width="90%" SkinID="gridviewSkin">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnAddRow6" runat="server" OnCommand="imgBtnAddRow6_Click"
                                                        ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                        ToolTip="Add Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnDeleteRow6" runat="server" OnCommand="imgBtnDeleteRow6_Click"
                                                        ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                        ToolTip="Delete Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_f1" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_f1") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_f2" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_f2") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_f3" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_f3") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_f4" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_f4") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_f5" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_f5") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkBox7" Checked="true" runat="server" />
                                    <asp:Label ID="lblHeading7" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdDetail7" runat="server" Width="90%" SkinID="gridviewSkin">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnAddRow7" runat="server" OnCommand="imgBtnAddRow7_Click"
                                                        ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                        ToolTip="Add Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnDeleteRow7" runat="server" OnCommand="imgBtnDeleteRow7_Click"
                                                        ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                        ToolTip="Delete Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                          
                                            <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_g1" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_g1") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_g2" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_g2") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_g3" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_g3") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_g4" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_g4") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_g5" BorderWidth="0px" onchange="checkValidate(this)" runat="server"
                                                        Text='<%#Eval("txt_g5") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>                                          
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>                            
                            <tr>
                                <td>
                                    <asp:Label ID="lblDiameterOfSample" runat="server" Text="Diameter of sample" Font-Bold="false" Visible="false"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="txtDiameterOfSample" Width="100px" runat="server" Visible="false"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblTemperatureDuringTest" runat="server" Text="Temperature During Test" Font-Bold="false" Visible="false"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="txtTemperatureDuringTest" Width="100px" runat="server" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                    </td>
                            </tr>
                        </table>
                    <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </asp:Panel>

            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <asp:Panel ID="pnlRemark" runat="server" ScrollBars="Auto" Height="70px" Width="942px"
                BorderColor="AliceBlue" BorderStyle="Ridge">
                <asp:GridView ID="grdOtherRemark" runat="server" SkinID="gridviewSkin" OnRowDataBound="grdOtherRemark_RowDataBound">
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
                        <asp:TemplateField HeaderText="References/Remark" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_REMARK" BorderWidth="0px" Width="600px" runat="server" Text='<%#Eval("txt_REMARK") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblType" Visible="false" runat="server" Text='<%#Eval("ddlRefType") %>' />
                                <asp:DropDownList ID="ddlRefType" Width="100px" runat="server">
                                    <asp:ListItem Text="Remark"></asp:ListItem>
                                    <asp:ListItem Text="Reference"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <table width="100%">
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblHeading" runat="server" Text="Report Title" Font-Bold="true"></asp:Label>
                        <asp:TextBox ID="txtHeading" runat="server" Text="" MaxLength="200" />
                        &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;
                        <asp:Label ID="Label6" runat="server" Text="Category" Font-Bold="true"></asp:Label>

                        <asp:DropDownList ID="ddl_Category" AutoPostBack="true" Width="200px" runat="server" Enabled="true">
                        </asp:DropDownList>

                        &nbsp;&nbsp;  &nbsp;&nbsp;    &nbsp;&nbsp;
                        <asp:Label ID="Label9" runat="server" Text="NABL Scope" Font-Bold="true"></asp:Label>

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
                        
                        
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        <asp:CheckBox ID="chk_WitnessBy" runat="server" AutoPostBack="true" OnCheckedChanged="chk_WitnessBy_CheckedChanged" />
                        <asp:Label ID="Label7" runat="server" Text="Witness By"></asp:Label>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txt_witnessBy" Width="200px" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td width="15%" style="text-align: right">
                        <asp:Label ID="lbl_TestedBy" runat="server" Text="Tested By"></asp:Label>
                    </td>
                    <td width="25%" align="center">
                        <asp:DropDownList ID="ddl_TestedBy" Width="205px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <%--<asp:LinkButton ID="lnkCal" OnClick="lnkCal_Click" runat="server" Visible="false"
                            Font-Bold="True" Style="text-decoration: underline;">Cal</asp:LinkButton>&nbsp;--%>

                        <asp:LinkButton ID="lnkCalculate" OnClick="lnkCalculate_Click" runat="server" Font-Bold="True" Visible="false"
                            Style="text-decoration: underline;">Calculate</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" Visible="false" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            runat="server" OnClick="lnk_Exit_Click">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>


        <script type="text/javascript">
            function checkValidate(inputtxt) {
                var numbers = (/[~!$`]/);
                if (!inputtxt.value.match(numbers)) {
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
                    return true;

                }
                else {
                    inputtxt.focus();
                    inputtxt.value = "";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                    document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "! ,~ ,$ ,`  are not allowed";
                    return false;
                }
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
    </div>
</asp:Content>
