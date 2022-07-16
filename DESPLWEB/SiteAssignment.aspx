<%@ Page Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    EnableEventValidation="false" Theme="duro" CodeBehind="SiteAssignment.aspx.cs"
    Inherits="DESPLWEB.SiteAssignment" Title="Site Assignment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr style="background-color: #ECF5FF;">
                    <td>
                        <asp:Label ID="lblClient" runat="server" Text="Client"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Client" runat="server" Width="400px"></asp:TextBox>
                        &nbsp;&nbsp;
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td>
                        <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="0" CompletionInterval="10"
                            EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client" ID="AutoCompleteExtender1"
                            runat="server" FirstRowSelected="true" CompletionListCssClass="autocomplete_completionListElement">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td style="width: 30%">
                        <asp:Label ID="lblRec" runat="server" Text="Total No of Records : 0"></asp:Label>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Save</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" runat="server" Font-Bold="True"
                            Style="text-decoration: underline;">Print</asp:LinkButton>
                    </td>
                    <td>
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7" valign="top">
                        <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="420px" Width="940px"
                            BorderStyle="Ridge" BorderColor="AliceBlue">
                            <asp:GridView ID="grdSiteAssignment" runat="server" AutoGenerateColumns="False" OnRowCreated="grdSiteAssignment_RowCreated"
                                BorderWidth="0px" Width="100%" SkinID="gridviewSkin" OnRowDataBound="RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="cbxSelectAll" runat="server" onclick="javascript:HeaderClick(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbxSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Client Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClientName" Text='<%#Eval("CL_Name_var") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteName" Text='<%#Eval("SITE_Name_var") %>' runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSiteId" Text='<%#Eval("SITE_Id") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClId" Text='<%#Eval("SITE_CL_Id") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Region">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlRegion" Width="100px" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ME">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddl_ME" Width="100px" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CE">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddl_CE" Width="100px" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Monthly Billing">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_MnthBilling" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RA Bill">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_RABill" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tassco">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_Tassco" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <asp:Label ID="lblMonthlyBillingNote" runat="server" Text="Note : Only super admin can update monthly billing setting." ForeColor="Maroon" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblSuperAdminRight" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">
        function validateClient() {
            var lblmsg = document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>');
            if (document.getElementById("<%=txt_Client.ClientID%>").value == "") {
                lblmsg.innerHTML = "Please Type the Client Name";
                lblmsg.style.visibility = "visible";
                document.getElementById("<%=txt_Client.ClientID%>").focus();
                return false;
            }
            else {
                lblmsg.style.visibility = "hidden";
            }
        }

        var TotalChkBx;
        var Counter;
        window.onload = function () {
            TotalChkBx = parseInt('<%= this.grdSiteAssignment.Rows.Count %>');
            Counter = 0;
        }
        function HeaderClick(CheckBox) {
            var TargetBaseControl = document.getElementById('<%= this.grdSiteAssignment.ClientID %>');
            var TargetChildControl = "cbxSelect";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = CheckBox.checked;
            Counter = CheckBox.checked ? TotalChkBx : 0;

        }
        function ChildClick(CheckBox, HCheckBox) {
            var HeaderCheckBox = document.getElementById(HCheckBox);
            if (CheckBox.checked && Counter < TotalChkBx)
                Counter++;
            else if (Counter > 0)
                Counter--;
            if (Counter < TotalChkBx)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBx)
                HeaderCheckBox.checked = true;

            if (CheckBox.checked) {
                CheckBox.parentElement.parentElement.style.backgroundColor = '#FFDFDF';
            }
            else {
                CheckBox.parentElement.parentElement.style.backgroundColor = '#FFFFFF';
            }
        }
        function CheckAll(Checkbox) {
            var grdSiteAssignment = document.getElementById("<%=grdSiteAssignment.ClientID %>");
            for (i = 1; i < grdSiteAssignment.rows.length; i++) {
                grdSiteAssignment.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;

                if (Checkbox.checked) {
                    Checkbox.parentElement.parentElement.style.backgroundColor = '#FFDFDF';
                }
                else {
                    Checkbox.parentElement.parentElement.style.backgroundColor = '#FFFFFF';
                }
            }

        }
    </script>
</asp:Content>
