<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="GST.aspx.cs" Inherits="DESPLWEB.GST" Theme="duro"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <%--<asp:Panel ID="pnlContent" runat="server" Width="920px" Height="480px" CssClass="rounded_Borders">--%>
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <asp:Panel ID="PnlFirst" runat="server" Width="100%">
                <table width="100%">
                   
                    <tr>
                        <td width="80px" style="text-align: right">
                            <asp:Label ID="lbl" runat="server" Text="From Date"></asp:Label>
                        </td>
                        <td width="6px" style="text-align: right">
                            <asp:TextBox ID="txt_Fromdate" SkinID="txt" runat="server" Width="190px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" TargetControlID="txt_Fromdate" CssClass="orange">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" AutoComplete="false" 
                                Mask="99/99/9999" MaskType="Date" TargetControlID="txt_Fromdate">
                            </asp:MaskedEditExtender>
                        
                        </td>
                        
                        <td style="text-align: left" colspan="6">
                            <asp:Label ID="lblErrMsg" Visible="false"  runat="server" Text="Date Should be greater than current date." ForeColor="Red"></asp:Label>
                        </td>
                          <%--<td style="text-align: right">
                        </td>--%>
                          <td style="text-align: right" >
                        </td>
                    </tr>
                     <tr>
                        <td width="80px" style="text-align: right" valign="top">
                            <asp:Label ID="Label1" runat="server" Text="SGST"></asp:Label>
                        </td>
                        <td width="60px" style="text-align: right">
                            <asp:TextBox ID="txtSGST" runat="server" Width="190px"  MaxLength="12" onchange="javascript:checkNum(this);"></asp:TextBox>
                            </td>
                            <td width="20px" style="text-align: left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  runat="server" ErrorMessage="*" ControlToValidate="txtSGST"></asp:RequiredFieldValidator>
                        </td>
                        <td style="text-align: right" width="50px" valign="top">
                           <asp:Label ID="Label4" runat="server" Text="CGST"></asp:Label>
                        </td>
                        <td width="60px" style="text-align: right">
                            <asp:TextBox ID="txtCGST" runat="server" Width="190px"  MaxLength="12" onchange="javascript:checkNum(this);"></asp:TextBox>
                            </td>
                            <td width="20px" style="text-align: left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtCGST"></asp:RequiredFieldValidator>
                        </td>
                        <td width="50px" style="text-align: right" valign="top">
                            <asp:Label ID="Label5" runat="server" Text="IGST"></asp:Label>
                        </td>
                        <td width="80px" style="text-align: right">
                            <asp:TextBox ID="txtIGST" runat="server" Width="190px"   MaxLength="12" onchange="javascript:checkNum(this);"></asp:TextBox>
                            </td>
                            <td width="20px" style="text-align: left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtIGST"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                     
                     
                    <tr>
                    <td colspan="8" style="height: 5px" align="center">
                      <asp:Label ID="lblresult" runat="server" ForeColor="Green"></asp:Label>
                      </td>
                        <td  style="height: 5px" align="right">
                            <asp:LinkButton ID="LnkBtnSave" runat="server" CssClass="SimpleColor" OnClick="LnkBtnSave_Click">Save</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PnlGvDeatils" runat="server" Height="380px" ScrollBars="Vertical"
                BorderColor="#D7D7E3" BorderStyle="Solid" BorderWidth="1" Width="99%" ForeColor="#D66112">
                
                <asp:GridView ID="gvDetailsView" runat="server"  ForeColor="#333333"
                    Width="100%" AutoGenerateColumns="False"   >
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField HeaderText="From Date" DataField="GST_From_dt" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="To Date" DataField="GST_To_dt" 
                            ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" 
                            NullDisplayText="NULL">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="SGST" DataField="GST_SGST_dec" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CGST" DataField="GST_CGST_dec" 
                            ItemStyle-HorizontalAlign="Center">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="IGST" DataField="GST_IGST_dec" 
                            ItemStyle-HorizontalAlign="Center">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                   <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="10%" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                   
                </asp:GridView>
            </asp:Panel>
       <table width="100%">
                        <tr>
                            <td style="text-align: right">
                            </td>
                            <td style="text-align: right">
                            </td>
                        </tr>
                    </table>
          
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script language="javascript">
       function checkNum(inputtxt) {
            var numbers = /^\d+(\.\d{1,2})?$/;
            if (inputtxt.value.match(numbers)) {
                return true;

            }
            else {
                alert('Please enter valid integer or decimal number with 2 decimal places');
                inputtxt.focus();
                inputtxt.value = "";
                return false;
            }
        }
        
    </script>
</asp:Content>
