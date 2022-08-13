<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="ReportSalesApp.aspx.cs" Inherits="DESPLWEB.ReportSalesApp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;" valign="top">
                    <td width="8%" valign="top">
                        <asp:Label ID="Label1" runat="server" Text="Select Report Type"></asp:Label>
                    </td>
                    <td width="40%">
                        <asp:DropDownList ID="ddl_ReportList" Width="220px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_ReportList_SelectedIndexChanged">
                            <asp:ListItem>---Select---</asp:ListItem>
                            <asp:ListItem>Competitor Sales Data</asp:ListItem>
                            <asp:ListItem>Competitor Sales Data Details</asp:ListItem>
                            <asp:ListItem>Construction Stage Status</asp:ListItem>
                            <asp:ListItem>Geotech Potential</asp:ListItem>
                        </asp:DropDownList>
                        <%--
                        <asp:RadioButton ID="RdnRpt1" runat="server" AutoPostBack="true" Text="Report1" GroupName="Rpt"
                            OnCheckedChanged="RdnRpt1_CheckedChanged" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="RdnRpt2" runat="server" AutoPostBack="true" GroupName="Rpt"
                            OnCheckedChanged="RdnRpt2_CheckedChanged" Text="Report2" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="RdnRpt3" runat="server" AutoPostBack="true" Text="Report3" GroupName="Rpt"
                            OnCheckedChanged="RdnRpt3_CheckedChanged" />
                        &nbsp; &nbsp;
                        <asp:RadioButton ID="RdnRpt4" runat="server" AutoPostBack="true" Text="Report4" GroupName="Rpt"
                            OnCheckedChanged="RdnRpt4_CheckedChanged" />--%>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkDisplay" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                            OnClick="lnkDisplay_Click" Font-Bold="True">Display</asp:LinkButton>
                    </td>
                    <td align="right" width="10%" valign="middle">
                        <asp:Label ID="lblTotalRecords" runat="server" Text="Total No of Records : 0 "></asp:Label>
                    </td>
                    <td align="right" width="8%">
                        <asp:LinkButton ID="lnkPrint" runat="server" ValidationGroup="V1" CssClass="LnkOver"
                            Style="text-decoration: underline;" Font-Bold="True" OnClick="lnkPrint_Click">Print</asp:LinkButton>
                        &nbsp;
                        <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                            OnClick="imgClosePopup_Click" ImageAlign="Right" Style="width: 16px" />
                    </td>
                </tr>
                <tr id="tr1" runat="server" visible="false">
                    <td width="8%">
                        <asp:Label ID="lblName" runat="server" Text="List Of Labs"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_LabList" Width="220px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tr2" runat="server" visible="false">
                    <td width="8%">
                        <asp:Label ID="lblMaterialName" runat="server" Text="Material Type"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_InwardTestType" Width="220px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tr3" runat="server" visible="false">
                    <td width="8%">
                        <asp:Label ID="lblStage" runat="server" Text="Stage Of site"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_StageOfSite" Width="220px" runat="server">
                            <asp:ListItem>---Select---</asp:ListItem>
                            <asp:ListItem>RCC</asp:ListItem>
                            <asp:ListItem>Block Work Plaster</asp:ListItem>
                            <asp:ListItem>Finishes</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tr6" runat="server" visible="false">
                    <td width="8%">
                        <asp:Label ID="lblStageStatus" runat="server" Text="Status"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_StageOfSiteStatus" Width="220px" runat="server">
                            <asp:ListItem>---Select All---</asp:ListItem>
                            <asp:ListItem>In Progress</asp:ListItem>
                            <asp:ListItem>Yet to start</asp:ListItem>
                            <asp:ListItem>Completed</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tr4" runat="server" visible="false">
                    <td width="8%">
                        <asp:Label ID="lblGeo" runat="server" Text="GeoTech Status"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_GeoTechStatus" Width="220px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tr5" runat="server" style="height: 27px" visible="false">
                    <td width="8%">
                        <asp:CheckBox ID="chkArea" Text="Area(Sq. Ft.)" runat="server" AutoPostBack="true"
                            OnCheckedChanged="chkArea_CheckedChanged" />
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_AreaFrom" runat="server" MaxLength="20" Width="100px" onchange="javascript:checkDecimal(this);"></asp:TextBox>-
                        <asp:TextBox ID="txt_AreaTo" runat="server" MaxLength="20" Width="100px" onchange="javascript:checkDecimal(this);"></asp:TextBox>
                    </td>
                </tr>
                <tr id="tr7" runat="server" style="height: 27px" visible="false">
                    <td width="8%">
                        <asp:Label ID="Label2" runat="server" Text="Select ME"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_ME" Width="220px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="365px" Width="940px"
                            BorderStyle="Ridge" BorderColor="AliceBlue">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdList" runat="server" BackColor="#CCCCCC" BorderColor="#DEBA84"
                                        BorderWidth="1px" ForeColor="#333333" Width="100%" CellPadding="4" AutoGenerateColumns="False">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <%--<asp:TemplateField HeaderText="Sr.No" ItemStyle-Width="30px">
                                                <ItemTemplate>
                                                    <span>
                                                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                                    </span>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                            </asp:TemplateField>--%>
                                               <asp:BoundField DataField="Sr_No" HeaderText="Sr. No" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Lab_Name" HeaderText="Lab Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Aggregate_Testing" HeaderText="Aggregate Testing" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AAC_Block_Testing" HeaderText="AAC Block Testing" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Brick_Testing" HeaderText="Brick Testing" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Cement_Chemical_Testing" HeaderText="Cement Chemical Testing"
                                                ItemStyle-HorizontalAlign="Center" NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Cement_Testing" HeaderText="Cement Testing" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Core_Cutting" HeaderText="Core Cutting" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Core_Testing" HeaderText="Core Testing" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Cube_Testing" HeaderText="Cube Testing" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Fly_Ash_Testing" HeaderText="Fly Ash Testing" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Soil_Investigation" HeaderText="Soil Investigation" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Mix_Design" HeaderText="Mix Design" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Non_Destructive_Testing" HeaderText="Non Destructive Testing"
                                                ItemStyle-HorizontalAlign="Center" NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Pile_Testing" HeaderText="Pile Testing" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Pavement_Block_Testing" HeaderText="Pavement Block Testing"
                                                ItemStyle-HorizontalAlign="Center" NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Soil_Testing" HeaderText="Soil Testing" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Masonary_Block_Testing" HeaderText="Masonary Block Testing"
                                                ItemStyle-HorizontalAlign="Center" NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Steel_Testing" HeaderText="Steel Testing" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Steel_Chemical_Testing" HeaderText="Steel Chemical Testing"
                                                ItemStyle-HorizontalAlign="Center" NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Tile_Testing" HeaderText="Tile Testing" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Water_Testing" HeaderText="Water Testing" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Other_Testing" HeaderText="Other Testing" ItemStyle-HorizontalAlign="Center"
                                                NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Rain_Water_Harvesting" HeaderText="Rain Water Harvesting"
                                                ItemStyle-HorizontalAlign="Center" NullDisplayText="0">
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SITE_Name_var" ItemStyle-HorizontalAlign="Left" HeaderText="Site Name">
                                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CL_Name_var" HeaderText="Client Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Site_Address_var" ItemStyle-HorizontalAlign="Left" HeaderText="Site Address">
                                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MATERIAL_Name_var" HeaderText="Inward Type" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="90px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Lead_discription" ItemStyle-HorizontalAlign="Left" HeaderText="Reason">
                                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Status" ItemStyle-HorizontalAlign="Left" HeaderText="Status">
                                                <ItemStyle HorizontalAlign="Left" Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="USER_Name_var" HeaderText="ME Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="90px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SITE_BulidUpArea_dec" HeaderText="Area (Sq. ft.)" ItemStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="90px" />
                                            </asp:BoundField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                        <EmptyDataTemplate>
                                            No records to display
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script>
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
</asp:Content>
