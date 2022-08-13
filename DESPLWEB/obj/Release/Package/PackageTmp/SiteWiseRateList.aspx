<%@ Page Title="" Language="C#" MasterPageFile="~/MstPg_Veena.Master" AutoEventWireup="true"
    CodeBehind="SiteWiseRateList.aspx.cs" Inherits="DESPLWEB.SiteWiseRateList" Theme="duro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="stylized" class="myform">
        <asp:Panel ID="pnlContent" runat="server" Width="100%" BorderWidth="0px" Height="470px"
            Style="background-color: #ECF5FF;">
            <table style="width: 100%">
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr style="background-color: #ECF5FF;">
                                <td>
                                    <asp:Label ID="lblClient" runat="server" Font-Bold="True" Text="Client Name"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txt_Client" runat="server" MaxLength="50" Width="343px" Height="16px"
                                        AutoPostBack="true" OnTextChanged="txt_Client_TextChanged"></asp:TextBox>
                                    &nbsp;
                                    <asp:AutoCompleteExtender ServiceMethod="GetClientname" MinimumPrefixLength="1" OnClientItemSelected="ClientItemSelected"
                                        CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Client"
                                        ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                                    </asp:AutoCompleteExtender>
                                    <asp:Label ID="lblSiteId" runat="server" Visible="false" Text="0"></asp:Label>
                                    <asp:Label ID="lblCLId" runat="server" Visible="false" Text="0"></asp:Label>
                                    <asp:Button ID="btnDeleteDuplicate" runat="server" Text="Delete Duplicate" Visible="false" OnClick="btnDeleteDuplicate_Click" Width="124px" />
                                </td>
                            </tr>
                            <tr style="background-color: #ECF5FF;">
                                <td>
                                    <asp:Label ID="lblSite" runat="server" Font-Bold="True" Text="Site Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Site" runat="server" MaxLength="50" Width="343px" Height="16px"
                                        OnTextChanged="txt_Site_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    &nbsp;
                                    <asp:AutoCompleteExtender ServiceMethod="GetSitename" MinimumPrefixLength="0" OnClientItemSelected="SiteItemSelected"
                                        CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txt_Site"
                                        ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement">
                                    </asp:AutoCompleteExtender>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td align="left">
                                    &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRecType" runat="server" Font-Bold="True" Text="Record Type"></asp:Label>&nbsp;&nbsp;
                                    <asp:HiddenField ID="hfClientId" runat="server" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRecordType" Width="180px" runat="server" 
                                         OnSelectedIndexChanged="ddlRecordType_OnSelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    &nbsp; &nbsp;
                                    <asp:DropDownList ID="ddlOtherTest" Width="180px" runat="server" Visible="false">
                                    </asp:DropDownList>
                                    &nbsp; &nbsp;
                                    <asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="~/Images/Search-32.png"
                                        Style="width: 14px" OnClick="ImgBtnSearch_Click" />
                                    <asp:HiddenField ID="hfSiteId" runat="server" />
                                </td>

                                <td align="right">

                                  <asp:TextBox ID="txtPer" runat="server"  onchange="javascript:checkNum(this);"  MaxLength="5" Width="30px" Height="16px" ></asp:TextBox>
                                   <asp:LinkButton ID="lnkApplyPer" runat="server" Font-Underline="True" ForeColor="Blue"
                                        OnClick="lnkApplyPer_Click">Apply (%)</asp:LinkButton>
                                    <asp:Label ID="lblDisc" runat="server" Font-Bold="True" Text="" Visible="false"></asp:Label>
                                  
                                    &nbsp; &nbsp;&nbsp; 
                                    <asp:CheckBox ID="chkApplyToAll" runat="server" Text="Apply current setting to all site" />
                                    &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                                    <asp:LinkButton ID="lnkUpdate" runat="server" Font-Underline="True" ForeColor="Blue"
                                        OnClick="lnkUpdate_Click">Update</asp:LinkButton>
                                    &nbsp; &nbsp;
                                    <asp:LinkButton ID="lnkClear" runat="server" Font-Underline="True" ForeColor="Blue"
                                        OnClick="lnkClear_Click">Clear</asp:LinkButton>
                                </td>
                            </tr>
                           <tr>
                                <td colspan="3">
                                    <asp:FileUpload ID="fileUpload1" runat="server" Width="210px" ForeColor="Red" Enabled="false" />
                                    <asp:Button ID="btnUpload" runat="server" Text="Upload File" OnClick="btnUpload_Click" ForeColor="Brown" />                                    
                                &nbsp; &nbsp; &nbsp;
                                    <asp:Label ID="lbltxt" runat="server" Font-Bold="true" ForeColor="Green" />
                                &nbsp; &nbsp; &nbsp;
                                    <asp:DropDownList ID="ddlFiles" Width="100px" CellPadding="5" runat="server" AutoGenerateColumns="false">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnDownload" Text="Download File" runat="server" OnClick="btnDownload_Click" ForeColor="Brown" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlClientList" runat="server" Width="100%" BorderWidth="0px" Height="370px"
                ScrollBars="Auto">
                <asp:UpdatePanel ID="UpdatePanelGrd" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdRate" runat="server" SkinID="gridviewSkin1" AutoGenerateColumns="False"
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
                                <asp:TemplateField HeaderText="Rate Id" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRateId" runat="server" Text='<%#Eval("SITERATE_Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Record Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecordType" runat="server" Text='<%#Eval("Test_RecType_var") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="20px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Test Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTestName" runat="server" Text='<%#Eval("TEST_Name_var") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="80px" Wrap="true" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other Test Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOtherTestType" runat="server" Text='<%#Eval("otherTestType") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="80px" Wrap="true" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Criteria">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCriteria" runat="server" Text='<%# string.Concat(Eval("TEST_From_num"), " - ", Eval("TEST_To_num"))%>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Current Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRate" runat="server" Text='<%#Eval("TEST_Rate_int") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="New Rate">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_NewRate" onchange="javascript:checkNum(this);" BorderWidth="0px"
                                            CssClass="textbox" MaxLength="12" runat="server" Text='<%#Eval("SITERATE_Test_Rate_int") %>'
                                            Width="90%" />
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
    <%--<script type="text/javascript">
     function checkFileExtension(elem) {
         var filePath = elem.value;

         if (filePath.indexOf('.') == -1)
             return false;

         var validExtensions = new Array();
         var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();

         validExtensions[0] = 'pdf';
         //validExtensions[1] = 'xlsx';

         for (var i = 0; i < validExtensions.length; i++) {
             if (ext == validExtensions[i])
                 return true;
         }

         alert('The file extension ' + ext.toUpperCase() + ' is not allowed!');
         return false;
     }
    </script>--%>

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
