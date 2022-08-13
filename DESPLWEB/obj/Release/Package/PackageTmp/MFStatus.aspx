<%@ Page Title="MF Status" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    Theme="duro" CodeBehind="MFStatus.aspx.cs" Inherits="DESPLWEB.MFStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr style="background-color: #ECF5FF;">
                    <td style="width: 10%">
                        <asp:Label ID="Label1" runat="server" Text="From Date"></asp:Label>
                    </td>
                    <td style="width: 10%">
                        <asp:TextBox ID="txt_FromDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td align="center" style="width: 10%">
                        <asp:Label ID="Label2" runat="server" Text="To Date"></asp:Label>
                    </td>
                    <td style="width: 10%">
                        <asp:TextBox ID="txt_ToDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_ToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_ToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td align="center" style="width: 5%">
                        &nbsp;</td>
                    <td style="width: 15%">
                        <asp:CheckBox ID="chkClientSpecific" Text="Client Specific" runat="server" />
                    </td>
                    <td align="left" style="width: 348px" >
                        <asp:TextBox ID="txt_Client" runat="server" Width="307px" AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                            CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                            ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                        <asp:HiddenField ID="hfClientId" runat="server" />
                        <asp:Label ID="lblClientId" runat="server" Text="0" Visible="false"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Report No"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_RptNo" runat="server" Width="148px"></asp:TextBox>
                </td>
                <td colspan="6">
                    <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" 
                        ImageUrl="~/Images/Search-32.png" OnClick="ImgBtnSearch_Click" 
                        Style="margin-top: 0px" />
                </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="440px"
                Width="940px" BorderColor="AliceBlue">

                <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 440px; overflow: auto">
                <asp:GridView ID="grdMFStatus" runat="server" AutoGenerateColumns="False" SkinID="gridviewSkin1"
                    Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Sr No.">
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mat. Rcvd.Date">
                            <ItemTemplate>
                                <asp:Label ID="lblRecvdDate" runat="server" Text='<%#Eval("MaterialDetail_ReceivedDt","{0:dd/MM/yy}") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Report No.">
                            <ItemTemplate>
                                <asp:Label ID="lblReportNo" runat="server" Text='<%#Eval("MaterialDetail_RefNo") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Blank Trial" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBlnkTrialPt" Enabled="false" ForeColor="DarkOrchid" OnCommand="lnkBlnkTrialPt_Click"
                                    runat="server" CommandArgument='<%#Eval("MaterialDetail_RefNo") %>' CssClass="linkbtn"> </asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sieve Analysis" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSA" Enabled="false" CssClass="linkbtn" OnCommand="lnkSA_Click"
                                      CommandName="SA" runat="server" CommandArgument='<%#Eval("MaterialDetail_RefNo")%>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trail" ItemStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnAddTrial" Enabled="false" runat="server" CommandName="AddNewTrial"
                                    OnCommand="imgBtnAddTrial_Click" CommandArgument='<%#Eval("MaterialDetail_RefNo") %>'
                                    ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                    ToolTip="Add New Trial" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trial Name" ItemStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTrail" ForeColor="Brown" CommandName="Trail" OnCommand="lnkTrail_Click"
                                    runat="server" CommandArgument='<%#Eval("MaterialDetail_RefNo") %>' CssClass="linkbtn"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lbl_TrialId" runat="server" Text="" Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trial" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTrialPt" ForeColor="DarkOrchid" Enabled="false" OnCommand="lnkTrialPt_Click"
                                    runat="server" CommandArgument='<%#Eval("MaterialDetail_RefNo") %>' CssClass="linkbtn">  </asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Other Info" ItemStyle-HorizontalAlign="Center"  Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkOtherInfo" Enabled="false" CommandName="OtherInfo" OnCommand="lnkOtherInfo_Click"
                                    runat="server" CommandArgument='<%#Eval("MaterialDetail_RefNo") + ";" + Eval("MaterialDetail_OtherInfo")  %>'
                                    CssClass="linkbtn"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comp Str" ItemStyle-HorizontalAlign="Center"  Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCompStr" ForeColor="Crimson" CommandName="CompStr" runat="server"
                                    OnCommand="lnkCompStr_Click" CssClass="linkbtn"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MDL Status" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkMDLIssue" Enabled="false" OnCommand="lnkMDLIssue_Click" CommandName="MDLIssue"
                                      runat="server" CommandArgument='<%#Eval("MaterialDetail_RefNo") + ";" + Eval("MaterialDetail_MDLIssue")  + ";" + Eval("MaterialDetail_Id")%>'
                                    CssClass="linkbtn" ToolTip="Enter MD Letter"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Final Report" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkFinalReport" Enabled="false" OnCommand="lnkFinalReport_Click"
                                      CommandName="FinalReport" runat="server" CommandArgument='<%#Eval("MaterialDetail_RefNo") + ";" + Eval("MaterialDetail_FinalReport")  + ";" + Eval("MaterialDetail_Id")%>'
                                    CssClass="linkbtn" ToolTip="Final Report"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Moisture Correction" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkMoistcor" ForeColor="#660033" Visible="false" Text="Print"
                                    OnCommand="lnkMoistcor_Click" CommandName="Moisture Correction" runat="server"
                                      CommandArgument='<%#Eval("MaterialDetail_RefNo") + ";" + Eval("MaterialDetail_FinalReport")  + ";" + Eval("MaterialDetail_Id")%>'
                                    CssClass="linkbtn" ToolTip="View Moisture correction"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cover Sheet" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkCoverSht" ForeColor="#660033" Visible="false" Text="Print"
                                      OnCommand="lnkCoverSht_Click" CommandName="Cover Sheet" runat="server" CommandArgument='<%#Eval("MaterialDetail_RefNo") + ";" + Eval("MaterialDetail_FinalReport")  + ";" + Eval("MaterialDetail_Id")%>'
                                    CssClass="linkbtn" ToolTip="View Cover Sheet"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div></div>
            </asp:Panel>
        </asp:Panel>
    </div>
     <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdMFStatus.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdMFStatus.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdMFStatus.ClientID%>').offset().top);

        });
    </script>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
