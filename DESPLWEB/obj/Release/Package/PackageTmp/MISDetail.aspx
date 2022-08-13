<%@ Page Title=" MIS " Language="C#" MasterPageFile="~/MstPg_Veena.Master" Theme="duro"
    AutoEventWireup="true" CodeBehind="MISDetail.aspx.cs" Inherits="DESPLWEB.MISDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 10%">
                        &nbsp;
                        <asp:Label ID="Label3" runat="server" Text="From Date"></asp:Label>
                    </td>
                    <td style="width: 10%">
                        <asp:TextBox ID="txt_FromDate" Width="100px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_FromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txt_FromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td align="center" style="width: 5%">
                        <asp:Label ID="Label6" runat="server" Text="To Date "></asp:Label>
                    </td>
                    <td style="width: 10%">
                        <asp:TextBox ID="txt_Todate" Width="100px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_Todate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_Todate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Stage From"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddl_StageFrom" Width="150px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_StageFrom_SelectedIndexChanged">
                            <asp:ListItem Text="---Select---" Value="0" />
                            <asp:ListItem Text="Enquiry" Value="1" />
                            <asp:ListItem Text="Collection Date" Value="2" />
                            <asp:ListItem Text="Recieved Date" Value="3" />
                            <asp:ListItem Text="Entered Date" Value="4" />
                            <asp:ListItem Text="Checked Date" Value="5" />
                            <asp:ListItem Text="Approved Date" Value="6" />
                            <asp:ListItem Text="Print Date" Value="7" />
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:Label ID="Label4" runat="server" Text="To"></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddl_To" Width="150px" runat="server">
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnk_Filter" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="lnk_Filter_Click">Add Filter </asp:LinkButton>
                    </td>
                    <td colspan="4" align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                        <asp:Label ID="Label2" runat="server" Text="Inward Type "></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_InwardType" Width="280px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_InwardType_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Time" Width="40px" MaxLength="5" onchange="checkNum(this)" runat="server"></asp:TextBox>&nbsp;&nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td>
                        <asp:Label ID="lblTimereqd" runat="server" Text=""></asp:Label>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="lnk_Summary" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="lnk_Summary_Click"> Summary </asp:LinkButton>&nbsp;
                               <asp:LinkButton ID="lnk_TimeReport" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="lnk_TimeReport_Click"> Timewise Report </asp:LinkButton>
                    </td>
                    <td>

                        <asp:LinkButton ID="lnk_Print" runat="server" Font-Bold="true" Font-Underline="true"
                            OnClick="lnk_Print_Click">Print </asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" BorderStyle="Ridge" Height="390px"
                Width="940px" BorderColor="AliceBlue">
                    <div style="width: 940px;">
                    <div id="GHead">
                    </div>
                    <div style="height: 390px; overflow: auto">
                <asp:GridView ID="grdMISDetail" runat="server" AutoGenerateColumns="False" Width="100%"
                    SkinID="gridviewSkin1" OnRowDataBound="grdMISDetail_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="MISRecordNo" HeaderText="Record No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="MISRecType" HeaderText="Record Type" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="MISRefNo" HeaderText="Ref No." HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" />
                        <asp:BoundField DataField="MISTestType" HeaderText="Test Type" Visible="false" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" />
                        <asp:BoundField DataField="ENQ_Date_dt" HeaderText="Enquiry Date" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}"/>
                        <asp:BoundField DataField="MISCollectionDt" HeaderText="Collection Date" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}"/>
                        <asp:BoundField DataField="MISRecievedDt" HeaderText="Received Date" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}"/>
                        <asp:BoundField DataField="MISEnteredDt" HeaderText="Entered Date" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}"/>
                        <asp:BoundField DataField="MISCheckedDt" HeaderText="Checked Date" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}"/>
                        <asp:BoundField DataField="MISApprovedDt" HeaderText="Approved Date" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}"/>
                        <asp:BoundField DataField="MISPrintedDt" HeaderText="Print Date" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}"/>
                        <asp:BoundField DataField="MISOutwardDt" HeaderText="Outward Date" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}"/>
                        <asp:BoundField HeaderText="Reqd. Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px"
                            ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" FooterStyle-Font-Bold="true" />
                        <asp:BoundField HeaderText="Approved" HeaderStyle-HorizontalAlign="Center" Visible="false"
                            ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" />
                        <asp:BoundField HeaderText="Print" HeaderStyle-HorizontalAlign="Center" Visible="false"
                            ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" />
                        <asp:BoundField HeaderText="Outward" HeaderStyle-HorizontalAlign="Center" Visible="false"
                            ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" />
                        <asp:BoundField HeaderText="Physical Outward" HeaderStyle-HorizontalAlign="Center"
                            Visible="false" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                            FooterStyle-Font-Bold="true" />
                              
                    </Columns>
                </asp:GridView>
                </div></div>
                
             
            </asp:Panel>
            
            <table width="100%">
                <tr>
                    <td style="width: 42%">
                           <asp:Label ID="lblTimeSlot" runat="server" Text="" Visible="false"></asp:Label>
          <asp:TextBox ID="txtEnq" BackColor="NavajoWhite" Width="40px" Visible="false" runat="server"></asp:TextBox>
                        <asp:Label ID="lblenq" runat="server" Text="Enquiry Detail" Visible="false"></asp:Label>
                        <asp:TextBox ID="txtInwd" BackColor="MistyRose" Width="40px" Visible="false" runat="server"></asp:TextBox>
                        <asp:Label ID="lblInwd" runat="server" Text="Inward Detail" Visible="false"></asp:Label>
                        <asp:TextBox ID="txtReport" BackColor="Silver" Width="40px" Visible="false" runat="server"></asp:TextBox>
                        <asp:Label ID="lblRpt" runat="server" Text="Report Detail" Visible="false"></asp:Label>
                    </td>
                    <td style="width: 10%" align="right">
                        <asp:Label ID="lblTotalRecord" Width="200px" runat="server" Font-Bold="true" ForeColor="Blue"
                            Text="Total No of Records : 0 "></asp:Label>
                    </td>
                    <td style="width: 25%" align="right">
                        <asp:Label ID="lbl_Avg" runat="server" Text="" Width="260px" Font-Bold="true" ForeColor="Blue"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblAvgres" Width="30px" Font-Bold="true" ForeColor="Blue" runat="server"
                            Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <script src="App_Themes/duro/jquery-1.7.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var gridHeader = $('#<%=grdMISDetail.ClientID%>').clone(true); // Here Clone Copy of Gridview with style
            $(gridHeader).find("tr:gt(0)").remove(); // Here remove all rows except first row (header row)
            $('#<%=grdMISDetail.ClientID%> tr th').each(function (i) {
                // Here Set Width of each th from gridview to new table(clone table) th 
                $("th:nth-child(" + (i + 1) + ")", gridHeader).css('width', ($(this).width()).toString() + "px");
            });
            $("#GHead").append(gridHeader);
            $('#GHead').css('position', 'absolute');
            $('#GHead').css('top', $('#<%=grdMISDetail.ClientID%>').offset().top);

        });
    </script>
    <script type="text/javascript">
        function checkNum(x) {
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
    </script>
</asp:Content>
