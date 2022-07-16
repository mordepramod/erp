<%@ Page Title="NDT - Report" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="NDT_ReportSummary.aspx.cs" Inherits="DESPLWEB.NDT_ReportTestReqWise" Theme="duro" MaintainScrollPositionOnPostback="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">

            <asp:Panel ID="Panel1" runat="server" BorderStyle="Ridge" Width="942px" BorderColor="AliceBlue"
                Height="440px" ScrollBars="Auto">
                <table style="width: 100%">
                   <tr>
                        <td style="width: 15%">
                            <asp:Label ID="lblReferenceNo" runat="server" Text="Reference No. : "></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:DropDownList ID="ddlReferenceNo" runat="server" Width="200px" OnSelectedIndexChanged="ddlReferenceNo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <asp:Label ID="lblReportId" runat="server" Text="" Visible="false"></asp:Label>
                             &nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkFetchAsPerCurrentData" OnClick="lnkFetchAsPerCurrentData_Click" runat="server" Font-Bold="True"
                                Style="text-decoration: underline;" Visible="false">Fetch as per current data</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Name of Client"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtClientName" Width="750px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Name of Project"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtSiteName" runat="server" Width="750px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Address"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtAddress" Width="750px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Project Description"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:DropDownList ID="ddlProjectDesc" runat="server" Width="150px" Height="20px">
                                <asp:ListItem Text="---Select---" />
                                <asp:ListItem Text="Residential" />
                                <asp:ListItem Text="Commercial" />
                                <asp:ListItem Text="Industrial" />
                            </asp:DropDownList>
                            &nbsp;
                            <asp:Label ID="Label4" runat="server" Text="RCC Structure"></asp:Label>
                            &nbsp;
                            <asp:DropDownList ID="ddlRccStruct" runat="server" Width="150px" Height="20px">
                                <asp:ListItem Text="---Select---" />
                                <asp:ListItem Text="Under Construction" />
                                <asp:ListItem Text="Construction Complete" />
                                <asp:ListItem Text="Occupied" />
                            </asp:DropDownList>
                            &nbsp;
                            <asp:Label ID="Label34" runat="server" Text="where age of concrete is"></asp:Label>
                            &nbsp;
                            <asp:TextBox ID="txtAge" Width="50px" runat="server" MaxLength="10" onchange="checkNum(this)"></asp:TextBox>
                            &nbsp;
                            <asp:DropDownList ID="ddlAgeIn" runat="server" Width="80px" Height="20px">
                                <asp:ListItem Text="---Select---" />
                                <asp:ListItem Text="days" />
                                <asp:ListItem Text="months" />
                                <asp:ListItem Text="years" />
                            </asp:DropDownList>
                            &nbsp;<br/>&nbsp;
                            <asp:Label ID="Label35" runat="server" Text="was tested using non destructive testing to estimate compressive strength of RCC members."></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label5" runat="server" Text="Purpose of Evaluation" Font-Bold="true"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:CheckBox ID="chkPurposeOfEvaluation1" runat="server" Text="To ascertain Strength of Concrete" Font-Bold="false" />&nbsp;<br />
                            <asp:CheckBox ID="chkPurposeOfEvaluation2" runat="server" Text="To evaluate if Concrete can be accepted for the Design Grade" Font-Bold="false" />&nbsp;<br />
                            <asp:CheckBox ID="chkPurposeOfEvaluation3" runat="server" Text="To identify % of members that need structural strengthening and their residual strength if any" Font-Bold="false" />&nbsp;<br />
                            <asp:CheckBox ID="chkPurposeOfEvaluation4" runat="server" Text="Other" Font-Bold="false" /> &nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="txtPurposeOfEvaluationOther" Width="550px" runat="server" MaxLength="250"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label10" runat="server" Text="NDT Coverage (Population)" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdNDTCoverage" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Building" HeaderText="Building/Structure" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Floor" HeaderText="Floor/Stage" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="GradeOfConcrete" HeaderText="Grade of Concrete" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MemberType" HeaderText="Member Type" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="No. of Members"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNoOfMembers" runat="server" BorderWidth="0px" CssClass="textbox"
                                                        MaxLength="12" onchange="javascript:checkint(this);" Text='<%#Eval("NoOfMembers") %>' Width="90px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label6" runat="server" Text="Reason for NDT testing" Font-Bold="true"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:CheckBox ID="chkReasonForTest1" runat="server" Text="Structural Distress was noticed" Font-Bold="false" />&nbsp;<br />
                            <asp:CheckBox ID="chkReasonForTest2" runat="server" Text="Low Cube results were observed" Font-Bold="false" />&nbsp;<br />
                            <asp:CheckBox ID="chkReasonForTest3" runat="server" Text="Cubes results are not available" Font-Bold="false" />&nbsp;<br />
                            <asp:CheckBox ID="chkReasonForTest4" runat="server" Text="Reliability of Cube results is not ascertained" Font-Bold="false" />&nbsp;<br />
                            <asp:CheckBox ID="chkReasonForTest5" runat="server" Text="Quality assurance purpose" Font-Bold="false" />&nbsp;<br />
                            <asp:CheckBox ID="chkReasonForTest6" runat="server" Text="Structural Audit to ascertain structural stability" Font-Bold="false" />&nbsp;<br />
                            <asp:CheckBox ID="chkReasonForTest7" runat="server" Text="Additional loads are expected" Font-Bold="false" />&nbsp;<br />
                            <asp:CheckBox ID="chkReasonForTest8" runat="server" Text="Other" Font-Bold="false" /> &nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="txtReasonForTestOther" Width="550px" runat="server" MaxLength="250"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label7" runat="server" Text="Type of NDT used for evaluation :" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdTest" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnAddRowTest" runat="server" OnCommand="imgBtnAddRowTest_Click"
                                                        ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                                        ToolTip="Add Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="18px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnDeleteRowTest" runat="server" OnCommand="imgBtnDeleteRowTest_Click"
                                                        ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                                        ToolTip="Delete Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="16px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Type of Test">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTypeOfTest" runat="server" BorderWidth="0px" Text="" Width="200px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Testing method">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTestingMethod" runat="server" BorderWidth="0px" Text="" Width="200px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="IS Reference">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtISReference" runat="server" BorderWidth="0px" Text="" Width="200px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label8" runat="server" Text="Sampling : " Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label11" runat="server" Text="The recommended sample size was as follows :"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtSampleSize1" Width="500px" runat="server" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label12" runat="server" Text="Following sample was adopted based on recommendation of Structural Engineer / Client / Durocrete"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label13" runat="server" Text="NDT Sample" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdNDTSample" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Building" HeaderText="Building/Structure" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Floor" HeaderText="Floor/Stage" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="GradeOfConcrete" HeaderText="Grade of Concrete" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MemberType" HeaderText="Member Type" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <%--<asp:BoundField DataField="NoOfMembersTested" HeaderText="No of members tested" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" />--%>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="No. of members tested"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNoOfMembersTested" runat="server" BorderWidth="0px" CssClass="textbox"
                                                        MaxLength="12" onchange="javascript:checkint(this);" Text='<%#Eval("NoOfMembersTested") %>' Width="90px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PercOfSample" HeaderText="% of sample" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label14" runat="server" Text="For Ultra sonic Pulse velocity and Schmitz Hammer tests -2 locations spaced less than 1m are tested on each member. In case of variation of more than 15% additional location is tested on the member. "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label17" runat="server" Text="Method of Selection of Members : " Font-Bold="true"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtMethodOfSelection" Width="250px" runat="server" MaxLength="250"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label18" runat="server" Text="Summary of Results of UPV " Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdSummaryResultUPV" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <%--<asp:BoundField DataField="CastingDate" HeaderText="Casting date" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />--%>
                                            <asp:BoundField DataField="Building" HeaderText="Building/Structure" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Floor" HeaderText="Floor/Stage" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="GradeOfConcrete" HeaderText="Design Grade of concrete" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="MemberType" HeaderText="Member Type" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="AverageUPV" HeaderText="Average UPV (km/s)" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="StandardDeviation" HeaderText="Standard Deviation (km/s)" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />                                            
                                            <asp:BoundField DataField="Remark" HeaderText="Concrete quality grading" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />                                            
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label20" runat="server" Text="Summary of Results of Rebound Hammer " Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdSummaryResultRH" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Building" HeaderText="Building/Structure" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Floor" HeaderText="Floor/Stage" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="GradeOfConcrete" HeaderText="Design Grade of concrete" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="MemberType" HeaderText="Member Type" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="AverageRH" HeaderText="Average Rebound Hammer" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="StandardDeviation" HeaderText="Standard Deviation (km/s)" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="6">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label16" runat="server" Text="Following members have reading below 3.5 km/s for M 20" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdReadBelow3p5ForM20" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Building" HeaderText="Building/Structure" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Floor" HeaderText="Floor/Stage" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MemberType" HeaderText="Member Type" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MemberId" HeaderText="Member Id" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label19" runat="server" Text="Following members have reading below 3.75 km/s for M 25 and above" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdReadBelow3p75ForM25" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Building" HeaderText="Building/Structure" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Floor" HeaderText="Floor/Stage" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MemberType" HeaderText="Member Type" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MemberId" HeaderText="Member Id" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label15" runat="server" Text="The Net Standard Errors are as follows :" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdNetStdErrs" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Building" HeaderText="Building/Structure" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Floor" HeaderText="Floor/Stage" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="GradeOfConcrete" HeaderText="Concrete Grade" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MemberType" HeaderText="Member Type" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="StandardError" HeaderText="Standard Error (MPa)" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" />                                           
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label25" runat="server" Text="Summary of Compressive Strengths " Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdSummaryCompStr" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Building" HeaderText="Building/Structure" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Floor" HeaderText="Floor/Stage" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="GradeOfConcrete" HeaderText="Design Grade of concrete" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MemberType" HeaderText="Member Type" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="95ConfInSituCompStr" HeaderText="95% Confidence In Situ Compressive Strength (MPa)" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="RecomGradeOfConcrete" HeaderText="Recommended Grade of concrete (MPa)" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label27" runat="server" Text="Members in the sample having concrete grade 5 MPa lower than design grade" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdMembersGrade5" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Building" HeaderText="Building/Structure" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Floor" HeaderText="Floor/Stage" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MemberType" HeaderText="Member Type" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="NoOfMembers" HeaderText="No of members" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="MemberId" HeaderText="Member Id" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label28" runat="server" Text="Members in the sample having concrete grade 10 MPa lower than design grade" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdMembersGrade10" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Building" HeaderText="Building/Structure" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Floor" HeaderText="Floor/Stage" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MemberType" HeaderText="Member Type" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="NoOfMembers" HeaderText="No of members" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="MemberId" HeaderText="Member Id" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>                    
                    <tr>
                        <td colspan="6">&nbsp;
                        </td>
                    </tr>
                </table>
                 <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>
                <table>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label29" runat="server" Text="Conclusion / Recommendation" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                           <%-- <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>--%>
                                    <asp:GridView ID="grdConcRecom" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelectWBS" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectWBS_CheckedChanged" />
                                                    <asp:Label ID="lblConclusionRecommendation" runat="server" Text='<%# Eval("ConclusionRecommendation") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="Building" HeaderText="Building/Structure" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Floor" HeaderText="Floor/Stage" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="GradeOfConcrete" HeaderText="Grade of concrete" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="MemberType" HeaderText="Member Type" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Option" HeaderText="Conclusion Recommendation" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                                            <%--<asp:BoundField DataField="ConclusionRecommendation" HeaderText="Conclusion Recommendation" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />--%>
                                        </Columns>
                                    </asp:GridView>
                               <%-- </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="lblSelectedWbs" runat="server" Text="" Font-Bold="true" ForeColor="Brown"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:RadioButton ID="optConclusion1" Text="Conclusion/Recommendation 1" GroupName="g1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            1. <asp:TextBox ID="txtConclusion1_1" Width="750px" runat="server" MaxLength="255"></asp:TextBox>&nbsp;<br />
                            2. <asp:TextBox ID="txtConclusion1_2" Width="750px" runat="server" MaxLength="255"></asp:TextBox>&nbsp;<br />
                            3. <asp:TextBox ID="txtConclusion1_3" Width="750px" runat="server" MaxLength="255"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                           
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:RadioButton ID="optConclusion2" Text="Conclusion/Recommendation 2" GroupName="g1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            1. <asp:TextBox ID="txtConclusion2_1" Width="750px" runat="server" MaxLength="255"></asp:TextBox>&nbsp;<br />
                            2. <asp:TextBox ID="txtConclusion2_2" Width="750px" runat="server" MaxLength="255"></asp:TextBox>&nbsp;<br />
                            3. <asp:TextBox ID="txtConclusion2_3" Width="750px" runat="server" MaxLength="255"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:RadioButton ID="optConclusion3" Text="Conclusion/Recommendation 3" GroupName="g1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            1. <asp:TextBox ID="txtConclusion3_1" Width="750px" runat="server" MaxLength="255"></asp:TextBox>&nbsp;<br />
                            2. <asp:TextBox ID="txtConclusion3_2" Width="750px" runat="server" MaxLength="255"></asp:TextBox>&nbsp;<br />
                            3. <asp:TextBox ID="txtConclusion3_3" Width="750px" runat="server" MaxLength="255"></asp:TextBox>&nbsp;<br />
                            4. <asp:TextBox ID="txtConclusion3_4" Width="750px" runat="server" MaxLength="255"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:RadioButton ID="optConclusion4" Text="Conclusion/Recommendation 4" GroupName="g1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            1. <asp:TextBox ID="txtConclusion4_1" Width="750px" runat="server" MaxLength="255"></asp:TextBox>&nbsp;<br />
                            2. <asp:TextBox ID="txtConclusion4_2" Width="750px" runat="server" MaxLength="255"></asp:TextBox>&nbsp;<br />
                            3. <asp:TextBox ID="txtConclusion4_3" Width="750px" runat="server" MaxLength="255"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkSelectConcRecom" OnClick="lnkSelectConcRecom_Click" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;">Ok</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">&nbsp;
                            &nbsp;
                        </td>
                    </tr>
                </table>
             </ContentTemplate>
                            </asp:UpdatePanel>
                                    </asp:Panel>

            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblApproveBy" runat="server" Text="Approve By"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                         <asp:DropDownList ID="ddlApproveBy" Width="205px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnkCalculate" OnClick="lnkCalculate_Click" runat="server" OnClientClick="return All();"
                            Font-Bold="True" Style="text-decoration: underline;">Calculate</asp:LinkButton>                        
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" OnClientClick="return All();"
                            Font-Bold="True" Style="text-decoration: underline;">Save</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" Visible="false" runat="server"
                            Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClick="lnkExit_Click" runat="server">Exit</asp:LinkButton>
                    </td>
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

    </script>
</asp:Content>
