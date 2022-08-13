<%@ Page Title="Bill" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true" CodeBehind="Bill.aspx.cs" Inherits="DESPLWEB.Bill" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function Showalert() {
            alert('Record is sucessfully Updated...');
        }
    </script>
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Label ID="lblBillId" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblCouponBill" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;" ScrollBars="Auto">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table width="100%">
                        <tr>
                            <td width="3%">
                                <asp:Label ID="lblDiscountModifyRight" runat="server" Text="False" Visible="false"></asp:Label>
                                <asp:Label ID="lblClient" runat="server" Text="Client"></asp:Label>                                
                            </td>
                            <td width="15%">
                                <asp:DropDownList ID="ddlClient" runat="server" Width="350px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td width="6%">
                                <asp:Label ID="lblBillNo" runat="server" Text="Bill No"></asp:Label>
                            </td>
                            <td width="5%">
                                <asp:TextBox ID="txtBillNo" runat="server" ReadOnly="True" Width="120px">Create New.......</asp:TextBox>
                            </td>                            
                            <td width="10%">
                                <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td width="9%">
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="150px">
                                    <asp:ListItem Text="Ok" Value="Ok" />
                                    <asp:ListItem Text="Cancel" Value="Cancel" />
                                </asp:DropDownList>
                            </td>
                            <td width="2%" align="right">
                                <asp:ImageButton ID="imgClosePopup" runat="server" ImageUrl="Images/cross_icon.png"
                                    OnClick="imgClosePopup_Click" ImageAlign="Right" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSite" runat="server" Text="Site"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSite" runat="server" Width="350px" AutoPostBack="True" OnSelectedIndexChanged="ddlSite_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblDate" runat="server" MaxLength="11" Text="Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDate" runat="server" Width="120px" ReadOnly="true"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="false" FirstDayOfWeek="Sunday"
                                    Format="dd/MM/yyyy" TargetControlID="txtDate">
                                </asp:CalendarExtender>
                                <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtDate" MaskType="Date"
                                    Mask="99/99/9999" AutoComplete="false" runat="server">
                                </asp:MaskedEditExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblWorkOrderNo" runat="server" Text="Work Order/PO No."></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWorkOrderNo" Width="150px" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" runat="server" Width="345px" TextMode="SingleLine"></asp:TextBox>
                            </td>
                            <td valign="top">
                                <asp:Label ID="lblRecordNo" runat="server" Text="Record No."></asp:Label>
                            </td>
                            <td valign="top" height="10px">
                                <asp:TextBox ID="txtRecordType" Width="40px" runat="server" ReadOnly="true"></asp:TextBox>
                                <asp:TextBox ID="txtRecordNo" Width="70px" runat="server" ReadOnly="true"></asp:TextBox>                                
                            </td>
                            <td>
                                <asp:Label ID="lblMaterialId" runat="server" Text="0" Visible="false"></asp:Label>
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="height: 5px;">
                            <asp:Label ID="lblClientCouponSetting" runat="server" Text="" Visible="true"></asp:Label>
                            <asp:Label ID="lblSiteSpecCoupStatus" runat="server" Text="" Font-Bold="true" ForeColor="Brown"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="chkAsPerClient" runat="server" Text="Update as per client coupon setting" Visible="false"/>
                            </td>
                            <td colspan="3" style="height: 5px;">
                                <asp:RadioButton ID="rdoCT" runat ="server" Text="Cube" Visible="true" GroupName="bill"  />
                                <asp:RadioButton ID="rdoST" runat ="server" Text="Steel" Visible="true" GroupName="bill"  />
                            </td> 
                        </tr>
                    </table>
                    <div>
                        <%--<tr>
                            <td colspan="6" >--%>
                        <asp:Panel ID="pnlBillDetail" runat="server" Height="150px" ScrollBars="Auto" Width="99%"
                            BorderStyle="Solid" BorderWidth="1px">
                            <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>--%>
                                    <asp:GridView ID="grdDetails" runat="server" AutoGenerateColumns="false" SkinID="gridviewSkin"
                                        OnRowDataBound="grdDetails_RowDataBound" CellPadding="2" CellSpacing="2" >
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgInsert" runat="server" OnCommand="imgInsert_Click" ImageUrl="Images/AddNewitem.jpg"
                                                        Height="20px" Width="20px" CausesValidation="false" ToolTip="Add New Row" />
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                                <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" OnCommand="imgDelete_Click"
                                                        Height="19px" ImageUrl="Images/DeleteItem.png" ToolTip="Delete Row" Width="18px" />
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                                <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="Sr.No" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Width="35px" />
                                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Particular" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlSubTest" runat="server" Width="280px" Visible="false" OnSelectedIndexChanged="ddlSubTest_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    <asp:DropDownList ID="ddl_Test" runat="server" Width="280px" OnSelectedIndexChanged="ddl_Test_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    <asp:Label ID="lblSubTestId" runat="server" Text="" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblTestId" runat="server" Text="" Visible="false"></asp:Label>
                                                    <asp:TextBox ID="txtDescription" runat="server" Width="280px" BorderWidth="0px" />
                                                    <asp:Label ID="lblCoupFrom" runat="server" Text="Coupon From" Visible="false"></asp:Label>
                                                    <asp:TextBox ID="txtCoupFrom" runat="server" Width="90px" OnTextChanged="txtCoupFrom_TextChanged"
                                                        Style="text-align: center; border-bottom: none; border-top: none" AutoPostBack="true"
                                                        MaxLength="12" BorderWidth="1px" BorderColor="Black" onchange="javascript:checkNum1(this);" ReadOnly="true" Visible="false"/>
                                                    <asp:Label ID="lblCoupTo" runat="server" Text="Coupon To" Visible="false"></asp:Label>
                                                    <asp:TextBox ID="txtCoupTo" runat="server" Width="90px" ReadOnly="true" Style="text-align: center;
                                                        border-bottom: none; border-top: none" AutoPostBack="true" MaxLength="12" BorderWidth="1px"
                                                        BorderColor="Black" onchange="javascript:checkNum(this);" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="SAC Code" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSACCode" runat="server" Style="text-align: Center;" Width="80px" BorderWidth="0px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Quantity" ItemStyle-HorizontalAlign="right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQuantity" runat="server" Width="60px" OnTextChanged="txtQuantity_TextChanged"
                                                        Style="text-align: Center; border-bottom: none; border-top: none" AutoPostBack="true"
                                                        MaxLength="12" BorderWidth="0px" BorderColor="Black" onchange="javascript:checkNum(this);" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="62px" />
                                                <ItemStyle HorizontalAlign="right" Width="60px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Rate" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtActualRate" runat="server" Width="100px" onchange="javascript:checkNum(this);"
                                                        OnTextChanged="txtActualRate_TextChanged" AutoPostBack="true" MaxLength="15" BorderWidth="0px"
                                                        Style="text-align: Center" ReadOnly="true"/>
                                                    <asp:Label ID="lblExist" runat="server" Text="" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Disc(%)" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTestDiscount" runat="server" Width="60px" onchange="javascript:checkNum(this);"
                                                        OnTextChanged="txtActualRate_TextChanged" AutoPostBack="true" MaxLength="5" BorderWidth="0px"
                                                        Style="text-align: Center" ReadOnly="true"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Discounted Rate" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRate" runat="server" Width="100px" onchange="javascript:checkNum(this);"
                                                        OnTextChanged="txtRate_TextChanged" AutoPostBack="true" MaxLength="15" BorderWidth="0px"
                                                        Style="text-align: Center" ReadOnly="true"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Amount" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAmount" runat="server" Width="100px" MaxLength="15" ReadOnly="true"
                                                        Style="text-align: Center" BorderWidth="0px" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="215px" />
                                                <ItemStyle HorizontalAlign="Center" Width="200px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                <%--</ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="grdDetails" />
                                </Triggers>
                            </asp:UpdatePanel>--%>
                        </asp:Panel>
                    </div>
                    <table width="100%">
                    <%--</td>
                        </tr>--%>
                        <tr>
                            <td colspan="6" style="height: 5px;">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: right">
                                <%--<asp:CheckBox ID="chkAsPerCurrentRate" Text="Tax As Per Current Setting" 
                                    Visible="false" runat="server" 
                                    oncheckedchanged="chkAsPerCurrentRate_CheckedChanged" />--%>
                                <asp:LinkButton ID="lnkRateAsPerCurrent" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                                    OnClick="lnkRateAsPerCurrent_Click" Font-Bold="True" Text="Calculate Tax As Per Current Setting" Visible="false"></asp:LinkButton>
                                &nbsp; &nbsp;

                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                                &nbsp; &nbsp;
                            </td>
                            <td style="text-align: right" width="200px">
                                <asp:TextBox ID="txtTotal" runat="server" Width="200px" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" width="150px">
                                <%--&nbsp;--%>
                            </td>
                            <td style="text-align: right" width="140px">
                                <asp:RadioButton ID="optLumpsum" runat="server" AutoPostBack="True" GroupName="A"
                                    TextAlign="Right" OnCheckedChanged="optLumpsum_CheckedChanged" Text="Lump sum" Visible="false"/><%--&nbsp;&nbsp;--%>
                            </td>
                            <td style="text-align: right" width="140px">
                                <asp:RadioButton ID="optPercentage" runat="server" AutoPostBack="True" GroupName="A"
                                    TextAlign="Right" OnCheckedChanged="optPercentage_CheckedChanged" Text="Percentage" Visible="false"/><%--&nbsp;&nbsp;--%>
                            </td>
                            <td style="text-align: right" width="140px">
                                <asp:TextBox ID="txtDiscPer" runat="server" AutoPostBack="True" OnTextChanged="txtDiscPer_TextChanged"
                                    Width="120px" onchange="javascript:checkNum(this);" Visible="false"></asp:TextBox>
                            </td>
                            <td style="text-align: right">
                                <asp:CheckBox ID="chkDiscount" runat="server" Text="Discount" AutoPostBack="True"
                                    OnCheckedChanged="chkDiscount_CheckedChanged" TextAlign="Left" Font-Bold="false"
                                    Width="130px" Enabled="false" Visible="false"/><%--&nbsp; &nbsp;--%>
                            </td>
                            <td style="text-align: right" width="200px">
                                <asp:TextBox ID="txtDiscount" runat="server" Width="200px" ReadOnly="true" Style="text-align: right" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="text-align: right">
                                <asp:Label ID="lblServiceTax" runat="server" Text="Service Tax " />
                                <asp:Label ID="lblSerTaxPer" runat="server" Text="(12%)" />                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtServiceTax" runat="server" Width="200px" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="text-align: right">
                                <asp:Label ID="lblSwachhBharatTax" runat="server" Text="Swachh Bharat Cess " />
                                <asp:Label ID="lblSwachhBharatTaxPer" runat="server" Text="(0.5%)" />                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtSwachhBharatTax" runat="server" Width="200px" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="text-align: right">
                                <asp:Label ID="lblKisanKrishiTax" runat="server" Text="Krishi Kalyan Cess " />
                                <asp:Label ID="lblKisanKrishiTaxPer" runat="server" Text="(0.5%)" />                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtKisanKrishiTax" runat="server" Width="200px" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="text-align: right">
                                <asp:Label ID="lblCGST" runat="server" Text="CGST " />
                                <asp:Label ID="lblCGSTPer" runat="server" Text="(0.9%)" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtCGST" runat="server" Width="200px" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="text-align: right">
                                <asp:Label ID="lblSGST" runat="server" Text="SGST " />
                                <asp:Label ID="lblSGSTPer" runat="server" Text="(0.9%)" />                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtSGST" runat="server" Width="200px" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="text-align: right">
                                <asp:Label ID="lblIGST" runat="server" Text="IGST " />
                                <asp:Label ID="lblIGSTPer" runat="server" Text="(0.18%)" />                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtIGST" runat="server" Width="200px" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="5" style="text-align: right">
                                <asp:Label ID="lblRoundingOff" runat="server" Text="Rounding Off"></asp:Label>
                                &nbsp; &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtRoundingOff" runat="server" Width="200px" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                              <td colspan="2" style="text-align: right">
                                <asp:Label ID="lblAdvanceRcpt" runat="server" Text="Advance Receipt No" Visible="false"></asp:Label>
                                &nbsp; &nbsp;
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtAdvRecpt" runat="server" Width="80px"  MaxLength="8"  Visible="false" onchange="javascript:checkNum(this);" AutoPostBack="true" OnTextChanged="txtAdvRecpt_TextChanged"></asp:TextBox>
                              <asp:TextBox ID="txtAdvRecptBal" runat="server" Text="0" Width="100px" Enabled="false" Visible="false"></asp:TextBox>
                           
                                    </td>
                            <td style="text-align: right">
                                <asp:Label ID="lblNet" runat="server" Text="Net Total"></asp:Label>
                                &nbsp; &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtNet" runat="server" Width="200px" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: right">
                                <asp:Label ID="lblReason" runat="server" Text="Reason" Visible="false"></asp:Label>
                                &nbsp; &nbsp;&nbsp; &nbsp;
                                <asp:TextBox ID="txtReason" runat="server" Width="400px" MaxLength="250" Visible="false"></asp:TextBox>
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lblAdvancePaid" runat="server" Text="Advance Paid"></asp:Label>
                                &nbsp; &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtAdvancePaid" runat="server" Width="200px" Style="text-align: right" onchange="javascript:checkNum(this);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td colspan="5" align="center">
                                <asp:Label ID="lblMessage" runat="server" ForeColor="#990033" Text="lblMessage" Visible="False"></asp:Label>
                            </td>
                            <td style="text-align: right" valign="top">
                                <asp:LinkButton ID="lnkSave" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                                    OnClientClick="return validate1();" OnClick="lnkSave_Click" Font-Bold="True">Save</asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="lnkPrint" runat="server" OnClick="lnkPrint_Click" Visible="false"
                                    CssClass="LnkOver" Style="text-decoration: underline;">Print</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <asp:Label ID="lblAccess" Text="Access is denied." runat="server" Font-Bold="True"
            ForeColor="Red" Font-Size="Small" Visible="False"></asp:Label>
    </div>
    <script type="text/javascript">

        function validate1() {

            var clent = document.getElementById("<%=ddlClient.ClientID %>");
            if (clent.value == "0") {
                alert(" Please Select the Client Name");
                return false;
            }
            var site = document.getElementById("<%=ddlSite.ClientID %>");
            if (site.value == "0") {
                alert(" Please Select the Site Name");
                return false;
            }
            if (document.getElementById("<%=txtAddress.ClientID%>").value == "") {
                alert("Please enter Address ");
                document.getElementById("<%=txtAddress.ClientID%>").focus();
                return false;
            }
                        
            var GVMaintainBillMaster = document.getElementById("<%= grdDetails.ClientID %>");
            for (var rowId = 1; rowId < GVMaintainBillMaster.rows.length; rowId++) {
                var txttdescp = GVMaintainBillMaster.rows[rowId].cells[3].getElementsByTagName("input")[0];
                var ddlsubtest = GVMaintainBillMaster.rows[rowId].cells[3].children[0];
                var ddltest = GVMaintainBillMaster.rows[rowId].cells[3].children[1];                
                var txtsaccode = GVMaintainBillMaster.rows[rowId].cells[4].children[0];
                var txttquantity = GVMaintainBillMaster.rows[rowId].cells[5].children[0];
                var txttcost = GVMaintainBillMaster.rows[rowId].cells[6].children[0];
                var txttdiscrate = GVMaintainBillMaster.rows[rowId].cells[8].children[0];
                var txttCoupFrom = GVMaintainBillMaster.rows[rowId].cells[3].getElementsByTagName("input")[0];
                //var txttCoupTo = GVMaintainBillMaster.rows[rowId].cells[3].getElementsByTagName("input")[1];

                //alert(txttquantity.value)
                                
                if (document.getElementById("<%=txtRecordType.ClientID%>").value == "OT") {
                    if (ddlsubtest.value == "" || ddlsubtest.value == "0") {
                        alert("Please select Sub-Test");
                        ddlsubtest.style.borderColor = "#FF0000";
                        ddlsubtest.focus();
                        return false;
                    }
                }

                if (ddltest.value == "" || ddltest.value == "0") {
                    alert("Please select Test");
                    ddltest.style.borderColor = "#FF0000";
                    ddltest.focus();
                    return false;
                }

                if (txttdescp.value == "") {
                    alert("Please enter Particular");
                    txttdescp.style.borderColor = "#FF0000";
                    txttdescp.focus();
                    return false;
                }
                
                if (txtsaccode.value == "") {
                    alert("Please enter SAC Code");
                    txtsaccode.style.borderColor = "#FF0000";
                    txtsaccode.focus();
                    return false;
                }

                if (txttquantity.value == "" || txttquantity.value == "0") {
                    alert("Please enter Quantity");
                    txttquantity.style.borderColor = "#FF0000";
                    txttquantity.focus();
                    return false;
                }

                //if (txttcost.value == "" || txttcost.value == "0.00" || txttcost.value == "0") {
                //    alert("Please enter Rate");
                //    txttcost.style.borderColor = "#FF0000";
                //    txttcost.focus();
                //    return false;
                //}

                if (txttdiscrate.value == "" || txttdiscrate.value == "0.00" || txttdiscrate.value == "0") {
                    alert("Please enter Discounted Rate");
                    txttdiscrate.style.borderColor = "#FF0000";
                    txttdiscrate.focus();
                    return false;
                }
                
            }
            

            if (document.getElementById("<%=txtDate.ClientID%>").value == "") {
                alert("Please Enter Date:");
                return false;
            }
            
            
//            if (document.getElementById("<%=chkDiscount.ClientID%>").checked == true) {
//                if (document.getElementById("<%=txtDiscPer.ClientID%>").value == "") {
//                    alert("Please Enter Some value in discount field !" + "\n" + "Otherwise Uncheck it");
//                    return false;
//                }

//            }
            if (document.getElementById("<%=txtNet.ClientID%>").value == "0.00") {
                alert('Your Net aomunt is 0.00 ' + '\n' + 'Billl can not be generted');
                return false;
            }

            if (document.getElementById("<%=txtBillNo.ClientID%>").value != "Create New......." &&
                document.getElementById("<%=txtReason.ClientID%>").value == "") {
                alert('Please enter reason for bill modify');
                return false;
            }

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
        function checkNum1(inputtxt) {
            var numbers = /^\d+(\.\d{1,2})?$/;
            var GVMaintainBillMaster = document.getElementById("<%= grdDetails.ClientID %>");
            for (var rowId = 1; rowId < GVMaintainBillMaster.rows.length; rowId++) {
                var txttquantity = GVMaintainBillMaster.rows[rowId].cells[4].children[0];

                if (txttquantity.value == "" || txttquantity.value == "0") {
                    alert("Please enter Quantity");
                    txttquantity.style.borderColor = "#FF0000";
                    txttquantity.focus();
                    inputtxt.value = "";
                    return false;
                }
            }
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
