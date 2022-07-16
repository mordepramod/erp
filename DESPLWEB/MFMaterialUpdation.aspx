<%@ Page Title="Material Updation" Language="C#" MasterPageFile="~/MstPg_Veena.Master"
    AutoEventWireup="true" CodeBehind="MFMaterialUpdation.aspx.cs" Inherits="DESPLWEB.MFMaterialUpdation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%;">
                <tr>
                    <td >
                    </td>
                    <td colspan="6" style="height: 5px">
                    </td>
                    <td align="right">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                            ImageAlign="Right" />
                    </td>
                </tr>
                <tr style="background-color: #ECF5FF;">
                    <td style="width: 15%">
                    </td>
                    <td style="width: 10%">
                        <asp:Label ID="Label1" runat="server" Text="From Date"></asp:Label>
                    </td>
                    <td style="width: 15%">
                        <asp:TextBox ID="txtFromDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtFromDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td style="width: 5%">
                    </td> 
                    <td style="width: 15%">
                        <asp:Label ID="Label2" runat="server" Text="To Date"></asp:Label>
                    </td>
                    <td style="width: 20%">
                        <asp:TextBox ID="txtToDate" Width="148px" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                            Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtToDate" MaskType="Date"
                            Mask="99/99/9999" AutoComplete="false" runat="server">
                        </asp:MaskedEditExtender>
                    </td>
                    <td style="width: 15%">
                        <asp:ImageButton ID="ImgBtnSearch" runat="server" Height="18px" ImageUrl="~/Images/Search-32.png"
                            OnClick="ImgBtnSearch_Click" Style="margin-top: 0px" />
                    </td>
                    <td >
                    </td>
                </tr>
                <tr>
                <td >
                    </td>
                    <td colspan="6" style="height: 5px">
                    </td>
                </tr>
                <tr>
                <td >
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Reference No."></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlReferenceNo" runat="server" Width="150px" AutoPostBack="true"
                            onselectedindexchanged="ddlReferenceNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Material"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMaterial" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:LinkButton ID="lnkNotToUseMaterial" runat="server" 
                            onclick="lnkNotToUseMaterial_Click">Not to Use Material</asp:LinkButton>
                    </td>
                </tr> 
                <tr>
                    <td>
                    </td>
                    <td colspan="6" style="height: 5px">
                    </td>
                </tr>
                <tr>
                    <td >
                    </td>
                    <td>
                        <asp:Label ID="Label9" runat="server" Text="Information"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtInformation" Width="148px" runat="server" MaxLength="250"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label11" runat="server" Text="Quantity"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuantity" Width="148px" runat="server" MaxLength="10" onchange="checkNum(this)"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        
                    </td>
                </tr>
                <tr>
                    <td >
                    </td>
                    <td colspan="6" style="height: 5px">
                    </td>
                </tr>
                <tr>
                    <td >
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblMaterialAliasName" runat="server" Text="Alias Name of Material"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMaterialAliasName" Width="148px" runat="server" MaxLength="250" ></asp:TextBox>
                    </td>
                    <td colspan="2">
                        <asp:LinkButton ID="lnkUpdateAliasName" runat="server" 
                            onclick="lnkUpdateAliasName_Click">Update Alias Name for Material</asp:LinkButton>
                    </td>
                </tr>  
                <tr>
                    <td >
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        
                    </td>
                    <td>
                    </td>
                    <td>
                        e.g. - Washed Sand
                    </td>
                    <td>
                        
                    </td>
                    <td colspan="2">
                       
                    </td>
                </tr>
                <tr>
                <td >
                    </td>
                    <td colspan="6">
                        <asp:Label ID="Label5" runat="server" Font-Bold="true" Text="Existing Material" ForeColor="OrangeRed"></asp:Label>
                    </td>
                </tr>
                <tr>
                <td >
                    </td>
                    <td colspan="6" style="height: 5px">
                    </td>
                </tr>
                <tr>
                    <td >
                    </td>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="Reference No."></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlExReferenceNo" runat="server" Width="150px" AutoPostBack="true"
                            onselectedindexchanged="ddlExReferenceNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="Material"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlExMaterial" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:LinkButton ID="lnkUseMaterial" runat="server" 
                            onclick="lnkUseMaterial_Click">Use Material</asp:LinkButton>
                    </td>
                </tr> 
                
                <tr>
                    <td>
                    </td>
                    <td colspan="6" style="height: 5px">
                    </td>
                </tr>
                <tr>
                    <td >
                    </td>
                    <td colspan="6">
                        <asp:Label ID="Label8" runat="server" Font-Bold="true" Text="New Material" ForeColor="OrangeRed"></asp:Label>
                    </td>
                </tr>
                <tr>
                <td >
                    </td>
                    <td colspan="6" style="height: 5px">
                    </td>
                </tr>
                <tr>
                <td >
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label10" runat="server" Text="Material"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlNewMaterial" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:LinkButton ID="lnkAddMaterial" runat="server" 
                            onclick="lnkAddMaterial_Click">Add Material</asp:LinkButton>
                    </td>
                </tr> 
                <tr>
                    <td>
                    </td>
                    <td colspan="6" style="height: 5px">
                    </td>
                </tr>
                <tr>
                <td >
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" Text="" Width="148px" MaxLength="250" />
                    </td>
                    <td colspan="2">
                        <asp:LinkButton ID="lnkUpdateDescription" runat="server" 
                            onclick="lnkUpdateDescription_Click">Update Description</asp:LinkButton>
                    </td>
                </tr> 
                <tr>
                    <td>
                    </td>
                    <td colspan="6" style="height: 5px">
                    </td>
                </tr>
            </table>
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
