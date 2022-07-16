<%@ Page Language="C#" Title="Steel Chemical Report" AutoEventWireup="true" MasterPageFile="~/MstPg_Veena.Master" CodeBehind="STC_Report.aspx.cs" Theme="duro" Inherits="DESPLWEB.STC_Report" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform" style="height: 470px;">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">

            <div style="height: 5px" align="right">
                <asp:ImageButton ID="imgClose" runat="server" ImageUrl="Images/cross_icon.png" OnClick="imgClose_Click"
                    ImageAlign="Right" />
            </div>
            <asp:Panel ID="Panel3" runat="server" BorderStyle="Ridge" Width="942px" BorderColor="AliceBlue">
                <table style="width: 100%">
                    <tr style="background-color: #ECF5FF;">
                        <td width="15%">
                            <asp:Label ID="lbl_OtherPending" runat="server" Text="Other Pending Reports"></asp:Label>
                        </td>
                        <td width="28%">
                            <asp:DropDownList ID="ddl_OtherPendingRpt" Width="205px" runat="server">
                            </asp:DropDownList>
                            <asp:LinkButton ID="lnk_Fetch" runat="server"
                                Style="text-decoration: underline;" Font-Bold="true" OnClick="lnk_Fetch_Click">Fetch</asp:LinkButton>
                        </td>
                        <td width="10%" align="right">
                            <asp:Label ID="lbl_RptNo" runat="server" Text="Report No"></asp:Label>
                        </td>
                        <td>&nbsp;  &nbsp;
                                <asp:TextBox ID="txt_RecordType" Width="50px" runat="server" ReadOnly="true"></asp:TextBox>
                            <asp:TextBox ID="txt_ReportNo" runat="server" Width="140px" ReadOnly="true"></asp:TextBox>
                            <asp:Label ID="lblRecordNo" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblUserId" runat="server" Text="0" Visible="false"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblEntry" runat="server" Text="Enter" Visible="false"></asp:Label>
                            &nbsp;
                            <asp:Label ID="lblULRNo" runat="server" Text="ULR No : " Font-Bold="true" Visible="false"></asp:Label>
                        </td>

                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td width="12%">
                            <asp:Label ID="Label1" runat="server" Text="Reference No"></asp:Label>
                        </td>
                        <td width="22%">
                            <asp:TextBox ID="txt_ReferenceNo" Width="200px" runat="server" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td width="10%" style="text-align: right">
                            <asp:Label ID="Label2" runat="server" Text="Date Of Testing"></asp:Label>
                        </td>
                        <td>&nbsp;  &nbsp;
                                  <asp:TextBox ID="txt_DateOfTesting" Width="200px" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txt_DateOfTesting">
                            </asp:CalendarExtender>
                            <asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txt_DateOfTesting" MaskType="Date"
                                Mask="99/99/9999" AutoComplete="false" runat="server">
                            </asp:MaskedEditExtender>
                        </td>


                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Grade Of Steel "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_GradeOfSteel" Width="200px" ReadOnly="true" runat="server"></asp:TextBox>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label4" runat="server" Text="Sample Description"></asp:Label>
                        </td>
                        <td>&nbsp;  &nbsp;
                            <asp:TextBox ID="txt_Description" Width="200px" runat="server"></asp:TextBox>
                           
                        </td>
                        <td>
                             <asp:LinkButton ID="lnkGetData" OnClick="lnkGetAppData_Click" runat="server" Font-Bold="True"
                                Style="text-decoration: underline;">Get App Data</asp:LinkButton>
                        </td>
                    </tr>
                    <tr style="background-color: #ECF5FF;">
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Type Of Steel"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_typeOfSteel" Width="200px" runat="server" ReadOnly="true"></asp:TextBox>

                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="Label10" runat="server" Text="Supplier "></asp:Label>
                        </td>
                        <td width="25%" align="center">
                            <asp:TextBox ID="txt_Supplier" Width="200px" runat="server"></asp:TextBox>

                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Qty"></asp:Label>
                            &nbsp; 
                             
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Qty" CssClass="textbox" onchange="checkint(this);" MaxLength="2" Width="40px" runat="server" AutoPostBack="true" OnTextChanged="txt_QtyOnTextChanged"></asp:TextBox>
                        </td>

                    </tr>
                </table>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;
            </div>
            <asp:Panel ID="Mainpan" runat="server" ScrollBars="Auto" Height="160px" BorderStyle="Ridge" Width="942px" BorderColor="AliceBlue">

                <div>
                    &nbsp;
                    <asp:Label ID="Label9" runat="server" Font-Bold="true" Text="NABL Scope"></asp:Label>
                    <asp:DropDownList ID="ddl_NablScope" runat="server"  Width="80px">
                        <asp:ListItem Text="--Select--" />
                        <asp:ListItem Text="F" />
                        <asp:ListItem Text="NA" />
                    </asp:DropDownList>
                    &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                    <asp:Label ID="Label11" runat="server" Text="NABL Location" Font-Bold="true"></asp:Label>
                    <asp:DropDownList ID="ddl_NABLLocation" runat="server" Width="80px" Enabled="true">
                        <asp:ListItem Value="0" Text="0" />
                                        <asp:ListItem Value="1" Text="1" />
                                        <asp:ListItem Value="2" Text="2" />
                    </asp:DropDownList>
                    
                    <br />
                    <br />
                </div>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>

                        <asp:GridView ID="grdSTCEntryRptInward" runat="server" SkinID="gridviewSkin">
                            <Columns>

                                <asp:TemplateField HeaderText="Sr No.">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="48px" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dia Of Bar" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_DiaOfBar" BorderWidth="0px" Style="text-align: right" Width="100px" runat="server" MaxLength="2" onchange="checkint(this);"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Id Mark" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_IdMARK" BorderWidth="0px" runat="server" Width="180px" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Carbon (%)" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Carbon" BorderWidth="0px" Style="text-align: right" Width="100px" onchange="javascript:CheckValue(this);" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Manganese(%)" HeaderStyle-HorizontalAlign="Center"  visible="false">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Manganese" BorderWidth="0px" Style="text-align: right" Width="100px" visible="false"  onchange="javascript:CheckValue(this);" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sulphar (%)" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Sulphar" BorderWidth="0px" Style="text-align: right" Width="100px" onchange="javascript:CheckValue(this);" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Phosphorous (%)" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Phosphorous" BorderWidth="0px" Style="text-align: right" Width="100px" onchange="javascript:CheckValue(this);" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sulphar+Phosphorous (%)" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_SulpharPhosphorous" BorderWidth="0px" Style="text-align: right" Width="152px" ReadOnly="true" CssClass="caltextbox" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <div style="height: 5px">
                &nbsp;
            </div>

            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="140px" BorderStyle="Ridge" Width="942px" BorderColor="AliceBlue">
                <asp:GridView ID="grdSTCRemark" runat="server" SkinID="gridviewSkin">

                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnAddRow" runat="server" OnCommand="imgBtnAddRow_Click"
                                    ImageUrl="Images/AddNewitem.jpg" Height="18px" Width="18px" CausesValidation="false"
                                    ToolTip="Add Row" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgBtnDeleteRow" runat="server" OnCommand="imgBtnDeleteRow_Click"
                                    ImageUrl="Images/DeleteItem.png" Height="16px" Width="16px" CausesValidation="false"
                                    ToolTip="Delete Row" />
                            </ItemTemplate>
                            <ItemStyle Width="16px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="lblSrNo" HeaderText="Sr.No" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Remark" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_REMARK" BorderWidth="0px" Width="850px" runat="server" Text='' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>

            <table width="100%">
                <tr height="30px">
                    <td width="12%">
                        <asp:CheckBox ID="chk_WitnessBy" runat="server" AutoPostBack="true"
                            OnCheckedChanged="chk_WitnessBy_CheckedChanged" />
                        <asp:Label ID="Label7" runat="server" Text="Witness By"></asp:Label>
                    </td>
                    <td width="30%">&nbsp;&nbsp;
                                <asp:TextBox ID="txt_witnessBy" Width="200px" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td width="20%" align="right">&nbsp; &nbsp;
                                <asp:Label ID="lbl_TestedBy" runat="server" Text="Tested By"></asp:Label>

                    </td>
                    <td align="left">&nbsp; &nbsp;
                                <asp:DropDownList ID="ddl_TestedBy" Width="205px" runat="server">
                                </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:LinkButton ID="Lnk_Calculate" OnClick="Lnk_Calculate_Click" runat="server" Font-Bold="True" Style="text-decoration: underline;">Cal</asp:LinkButton>&nbsp;
                               <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" Font-Bold="True" Style="text-decoration: underline;">Save</asp:LinkButton>&nbsp;
                               <asp:LinkButton ID="lnkPrint" OnClick="lnkPrint_Click" Visible="false" runat="server" Font-Bold="True" Style="text-decoration: underline;">Print</asp:LinkButton>&nbsp;
                               <asp:LinkButton ID="LnkExit" Font-Bold="True" Style="text-decoration: underline;" OnClick="lnk_Exit_Click" runat="server">Exit</asp:LinkButton>
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
      function CheckValue(inputtxt) {
          var numbers = /^\d+(\.\d{1,3})?$/;
          if (inputtxt.value.match(numbers)) {
              document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
              return true;

          }
          else {

              document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
              document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please enter valid integer or decimal number with 3 decimal places";
              inputtxt.focus();
              inputtxt.value = "";
              return false;
          }
      }


      function All() {
          document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "hidden";
          var grdSTCEntryRptInward = document.getElementById("<%= grdSTCEntryRptInward.ClientID %>");
          for (var rowId = 1; rowId < grdSTCEntryRptInward.rows.length; rowId++) {

              var Carbon = grdSTCEntryRptInward.rows[rowId].cells[3].children[0];
              var Mangese = grdSTCEntryRptInward.rows[rowId].cells[4].children[0];
              var Sulphar = grdSTCEntryRptInward.rows[rowId].cells[5].children[0];
              var Phosphorus = grdSTCEntryRptInward.rows[rowId].cells[6].children[0];

              var numbers = /^\d+(\.\d{1,3})?$/;
              if (!Carbon.value.match(numbers)) {


                  document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                  document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please enter valid integer or decimal number with 3 decimal places";
                  Carbon.focus();
                  Carbon.value = "";
                  return false;
              }
              else if (!Mangese.value.match(numbers)) {

                  document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                  document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please enter valid integer or decimal number with 3 decimal places";
                  Mangese.focus();
                  Mangese.value = "";
                  return false;
              }
              else if (!Sulphar.value.match(numbers)) {

                  document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                  document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please enter valid integer or decimal number with 3 decimal places";
                  Sulphar.focus();
                  Sulphar.value = "";
                  return false;
              }
              else if (!Phosphorus.value.match(numbers)) {

                  document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').style.visibility = "visible";
                  document.getElementById('<%=Master.FindControl("lblMsg").ClientID %>').innerHTML = "Please enter valid integer or decimal number with 3 decimal places";
                  Phosphorus.focus();
                  Phosphorus.value = "";
                  return false;
              }
}
}
    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
