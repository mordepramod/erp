<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="TestWiseNablScope.aspx.cs" Inherits="DESPLWEB.TestWiseNablScope" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td>
                        <table style="width: 100%;">
                          
                            <tr>
                                <td>
                                    <asp:Label ID="lblRecType" runat="server" Font-Bold="True" Text="Record Type"></asp:Label>&nbsp;&nbsp;
                                    <asp:HiddenField ID="hfClientId" runat="server" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRecordType" Width="250px" runat="server" >
                                       <%-- OnSelectedIndexChanged="ddlRecordType_OnSelectedIndexChanged" AutoPostBack="true"--%>
                                    </asp:DropDownList>
                                    &nbsp; &nbsp;
                                    <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/Search-32.png"
                                        Style="width: 14px" OnClick="ImgBtnSearch_Click" />
                                    <asp:HiddenField ID="hfSiteId" runat="server" />
                                </td>
                                <td align="right">

                                    &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                                    <asp:LinkButton ID="lnkUpdate" runat="server" Font-Underline="True" ForeColor="Blue"
                                        OnClick="lnkUpdate_Click">Update</asp:LinkButton>
                                    &nbsp; &nbsp;
                                    <%--<asp:LinkButton ID="lnkClear" runat="server" Font-Underline="True" ForeColor="Blue"
                                        OnClick="lnkClear_Click">Clear</asp:LinkButton>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlClientList" runat="server" Width="100%" BorderWidth="1px" Height="390px"
                ScrollBars="Auto">
                <asp:UpdatePanel ID="UpdatePanelGrd" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdTest" runat="server" SkinID="gridviewSkin1" AutoGenerateColumns="False"
                            CellPadding="4" ForeColor="#333333" Width="100%">
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sr. No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex + 1%>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="20px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Test Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTestId" runat="server" Text='<%#Eval("Test_Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Record Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecordType" runat="server" Text='<%#Eval("Test_RecType_var") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Test Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTestName" runat="server" Text='<%#Eval("TEST_Name_var") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="180px" Wrap="true" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nabl Scope">
                                    <ItemTemplate>
                                         <asp:Label ID="lblNABLScope" runat="server" Text='<%# Eval("TEST_NablScope_var") %>' Visible = "false" />
                                      <asp:DropDownList ID="ddlNABLScope" runat="server" BorderWidth="0px" Width="100%">
                                            <asp:ListItem  Value="0"  Text="--Select--" />
                                            <asp:ListItem Value="F" Text="F" />
                                            <asp:ListItem Value="P" Text="P" />
                                            <asp:ListItem Value="NA" Text="NA" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Nabl Location">
                                    <ItemTemplate>
                                         <asp:Label ID="lblNABLLocation" runat="server" Text='<%# Eval("TEST_NablLocation_int") %>' Visible = "false" />
                                       <asp:DropDownList ID="ddlNABLLocation" runat="server" BorderWidth="0px" Width="100%">
                                            <asp:ListItem  Value="0" Text="--Select--" />
                                            <asp:ListItem  Value="1" Text="1" />
                                            <asp:ListItem  Value="2" Text="2" />
                                            <asp:ListItem  Value="3" Text="3" />
                                            <asp:ListItem  Value="4" Text="4" />
                                            <asp:ListItem  Value="5" Text="5" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No records to display
                            </EmptyDataTemplate>
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        function ClientItemSelected(sender, e) {
            $get("<%=hfClientId.ClientID %>").value = e.get_value();
        }
        function SiteItemSelected(sender, e) {
            $get("<%=hfSiteId.ClientID %>").value = e.get_value();
        }

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
