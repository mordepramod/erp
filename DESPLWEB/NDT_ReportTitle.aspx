<%@ Page Title="NDT - Report Title" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="NDT_ReportTitle.aspx.cs" Inherits="DESPLWEB.NDT_ReportTitle" Theme="duro"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <%--<div id="stylized" class="myform" style="height: 2470px;">--%>
        <%--<asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="2470px"
            Style="background-color: #ECF5FF;">--%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
            <%--<asp:Panel ID="Panel1" runat="server" BorderStyle="Ridge" Width="942px" BorderColor="AliceBlue"
                Height="440px" ScrollBars="Auto">--%>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 15%">
                            <asp:Label ID="lblReferenceNo" runat="server" Text="Reference No. : "></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlReferenceNo" runat="server" Width="200px" OnSelectedIndexChanged="ddlReferenceNo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <asp:Label ID="lblReportId" runat="server" Text="" Visible="false"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label4" runat="server" Text="Enquiry No. : "></asp:Label>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="txtEnquiryNo" Width="100px" runat="server" text=""></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkGetData" OnClick="lnkGetAppData_Click" runat="server" Font-Bold="True"
                                        Style="text-decoration: underline;" >Get App Data</asp:LinkButton>
                            <%--OnClientClick="return confirm('Are you sure you want to Reset details if already data updated ?');"--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Name of Client"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClientName" Width="750px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Name of Project"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSiteName" runat="server" Width="750px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="1">
                            <asp:Label ID="Label10" runat="server" Text="WBS" Font-Bold="true"></asp:Label>
                        </td>
                        <td colspan="1" align="center">
                            <asp:LinkButton ID="lnkUpdateAllSelected" OnClick="lnkUpdateAllSelected_Click" runat="server" Font-Bold="True"
                                Style="text-decoration: underline;" >Update All Selected</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <%--<asp:Panel ID="pnlWbs" runat="server" ScrollBars="Auto" Height="160px" Width="900px"
                                BorderStyle="Ridge" BorderColor="AliceBlue">--%>
                                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>--%>
                                <asp:GridView ID="grdWBS" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelectWBS" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectWBS_CheckedChanged" />
                                                <asp:Label ID="lblWBSId" runat="server" Text="" Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Building/Structure" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBuilding" BorderWidth="0px" Width="100px" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Floor/Stage" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFloor" BorderWidth="0px" Width="100px" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MemberType" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMemberType" BorderWidth="0px" Width="100px" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MemberId" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMemberId" BorderWidth="0px" Width="100px" runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkUpdate" runat="server" OnClick="lnkUpdate_Click">Update</asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="10px" />
                                            </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                                <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%>
                            <%--</asp:Panel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblSelectedWbs" runat="server" Text="" Font-Bold="true" ForeColor="Brown"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <%--<asp:Panel ID="pnlNDTDetails" runat="server" ScrollBars="Auto" Height="160px" Width="900px"
                                BorderStyle="Ridge" BorderColor="AliceBlue">--%>
                                <asp:GridView ID="grdNDTDetails" runat="server" SkinID="gridviewSkin" Width="100%"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="lblDescription" HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="lblGrade" HeaderText="Grade" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="lblCastingDate" HeaderText="Casting Date" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="lblAlphaAngle" HeaderText="Alpha Angle" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="lblReboundIndex" HeaderText="Rebound Index Details" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="lblPulseVel" HeaderText="Pulse Vel Details" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                    </Columns>
                                </asp:GridView>
                            <%--</asp:Panel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1">
                            <asp:Label ID="Label3" runat="server" Text="WBS to be merge" Font-Bold="true"></asp:Label>
                        </td>
                        <td colspan="1" align="center">
                            <asp:LinkButton ID="lnkMergeAllSelected" OnClick="lnkMergeAllSelected_Click" runat="server" Font-Bold="True"
                                Style="text-decoration: underline;" >Merge All Selected</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                           <%-- <asp:Panel ID="pnlWbsMerge" runat="server" ScrollBars="Auto" Height="160px" Width="900px"
                                BorderStyle="Ridge" BorderColor="AliceBlue">--%>
                                <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>--%>
                                <asp:GridView ID="grdWBSMerge" runat="server" SkinID="gridviewSkin" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelectWBSMerge" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectWBSMerge_CheckedChanged" />
                                                <asp:Label ID="lblWBSId" runat="server" Text='<%# Eval("lblWBSId") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="Building" HeaderText="Building/Structure" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Floor" HeaderText="Floor/Stage" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="MemberType" HeaderText="Member Type" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="MemberId" HeaderText="Member Id" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkMerge" runat="server" OnClick="lnkMerge_Click">Merge</asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                            <%--</asp:Panel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblSelectedWbsMerge" runat="server" Text="" Font-Bold="true" ForeColor="Brown"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <%--<asp:Panel ID="pnlNDTDetails2" runat="server" ScrollBars="Auto" Height="160px" Width="900px"
                                BorderStyle="Ridge" BorderColor="AliceBlue">--%>
                                <asp:GridView ID="grdNDTDetails2" runat="server" SkinID="gridviewSkin" Width="100%"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="lblDescription" HeaderText="Description" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="lblGrade" HeaderText="Grade" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="lblCastingDate" HeaderText="Casting Date" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="lblAlphaAngle" HeaderText="Alpha Angle" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="lblReboundIndex" HeaderText="Rebound Index Details" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="lblPulseVel" HeaderText="Pulse Vel Details" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                    </Columns>
                                </asp:GridView>
                            <%--</asp:Panel>--%>
                        </td>
                    </tr>
                </table>
            <%--</asp:Panel>--%>                    
            </ContentTemplate>
            </asp:UpdatePanel>
            <div style="height: 5px">
                &nbsp;&nbsp;
            </div>
            <table width="100%">
                <tr>
                    <td></td>
                    <td align="right">
                        <asp:LinkButton ID="lnkExit" Font-Bold="True" Style="text-decoration: underline;"
                            OnClick="lnkExit_Click" runat="server">Exit</asp:LinkButton>
                    </td>
                </tr>
            </table>
            
            <div style="height: 20px">
                &nbsp;&nbsp;
            </div>
       <%-- </asp:Panel>--%>
    <%--</div>--%>

</asp:Content>
