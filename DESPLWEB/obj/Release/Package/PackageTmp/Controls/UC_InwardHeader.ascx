<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="UC_InwardHeader.ascx.cs"
    Inherits="DESPLWEB.Controls.UC_InwardHeader"  %>
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="MKB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<table style="width: 100%; height: 179;">
    <tr>
        <td colspan="10" align="center">
            <asp:Label ID="lblGst" runat="server" Visible="false" Text="" ForeColor="Red" ></asp:Label>
        </td>
    </tr> 
    <tr>
        <td>
            Enquiry List
        </td>
        <td>
            :
        </td>
        <td>
            <asp:DropDownList ID="ddlEnquiryList" runat="server" Width="150px">
            </asp:DropDownList>
            &nbsp;&nbsp;
            <asp:LinkButton ID="lnkFetch" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                Font-Bold="True" OnClick="lnkFetch_Click">New Inward</asp:LinkButton>     
                  
             <asp:Label ID="lblRecType" runat="server" Visible="false" Text=""  ></asp:Label>
             <asp:Label ID="lblInwdStatus" runat="server" Text="" Visible="false"  ></asp:Label>      
             
             <asp:Label ID="lblEnqNoApp" runat="server" Text="" Visible="false" ></asp:Label>
            <asp:Label ID="lblEnqDate" runat="server" Text="" Visible="false" ></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td >
            <asp:CheckBox ID="chkOtherClient" Text=" Without Bill" Font-Bold="true" runat="server"
                Visible="false" AutoPostBack="true" oncheckedchanged="chkOtherClient_CheckedChanged" /> 
            </td>
            <td colspan="4">
                &nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblTestReqFormNo" runat="server" Text="" Visible="true"></asp:Label>
                  &nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblBilling" runat="server" Text="" Font-Bold="true" Visible="true"></asp:Label>
                 <asp:Label ID="lblMonthlyStatus" runat="server" Text="0" Font-Bold="true" Visible="false"></asp:Label>
                <asp:Label ID="lblCrLimitStatus" runat="server" Text="0" Font-Bold="true" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            Client Name
        </td>
        <td>
            :
        </td>
        <td>
            <asp:TextBox ID="txtClientName" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
            <asp:Label ID="lblClId" runat="server" Text="" Visible="false"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            Collection Date
        </td>
        <td>
            :
        </td>
        <td colspan="2">
            <asp:Label ID="lblReceivedDate" runat="server" Text="" Visible="false"></asp:Label>
            &nbsp;<asp:TextBox ID="txtCollectionDate" runat="server" Height="18px" Width="130px"
                BackColor="#CCCCCC" BorderStyle="Groove"></asp:TextBox>
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                Format="dd/MM/yyyy" CssClass="orange" TargetControlID="txtCollectionDate">
            </asp:CalendarExtender>

            &nbsp;
        </td>
        <td colspan="2" align="left">            
            <asp:TextBox ID="txtCollectionTime" runat="server" Height="16px" Width="75px"
                ></asp:TextBox>
            <asp:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="txtCollectionTime"
                MaskType="time" Mask="99:99" AutoComplete="true" AcceptAMPM="true" runat="server">
            </asp:MaskedEditExtender>
        </td>
    </tr>
    <tr>
        <td>
            Site Name
        </td>
        <td>
            :
        </td>
        <td>
            <asp:TextBox ID="txtSiteName" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
             <asp:Label ID="lblSiteId" runat="server" Text="" Visible="false"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            Contact Person
        </td>
        <td>
            :
        </td>
        <td colspan="4" valign="top">
            &nbsp;<asp:ComboBox ID="cmbContactPerson" 
                runat="server" Height="16px" Width="230px" AutoPostBack="true"
                onselectedindexchanged="cmbContactPerson_SelectedIndexChanged">
            </asp:ComboBox>
            <br />
        </td>
    </tr>
    <tr>
        <td>
            Enquiry no
        </td>
        <td>
            :
        </td>
        <td>
            <asp:TextBox ID="txtEnquiryNo" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
            <asp:Label ID="lblMaterialId" runat="server" Text="" Visible="false" ></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            Contact No
        </td>
        <td>
            :
        </td>
        <td colspan="4">
            &nbsp;<asp:TextBox ID="txtContactNo" runat="server" Height="16px" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            Record No
        </td>
        <td>
            :
        </td>
        <td>
            <asp:TextBox ID="txtRecordNo" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            Email Id
        </td>
        <td>
            :
        </td>
        <td colspan="4">
            &nbsp;<asp:TextBox ID="txtEmailId" runat="server" Height="16px" Width="250px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            Referece No
        </td>
        <td>
            :
        </td>
        <td>
            <asp:TextBox ID="txtReferenceNo" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            Building
        </td>
        <td>
            :
        </td>
        <td colspan="4" valign="top">
            &nbsp;<asp:ComboBox ID="cmbBuilding" runat="server" Height="16px" Width="230px">
            </asp:ComboBox>
            <br />
        </td>
    </tr>
    <tr>
        <td>
            Bill No
        </td>
        <td>
            :
        </td>
        <td>
            <asp:TextBox ID="txtBillNo" runat="server" Height="16px" Width="100px" ReadOnly="True" MaxLength="20"></asp:TextBox>
            &nbsp;&nbsp; Pro No : &nbsp;
            <asp:TextBox ID="txtProformaInvoiceNo" runat="server" Height="16px" Width="100px" ReadOnly="True" Visible="true"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            Work Order No
        </td>
        <td>
            :
        </td>
        <td style="width: 110px">
            &nbsp;<asp:TextBox ID="txtWorkOrder" runat="server" Height="16px" Width="78px"></asp:TextBox>
        </td>
        <td style="width: 60px">
            <%--Charges :--%>
            <asp:Label ID="lblCharges" runat="server" Text="Charges  :"></asp:Label>
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtCharges" runat="server" Height="16px" Width="70px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="3">           
           <%-- <asp:TextBox ID="txtDiscount" runat="server" Height="16px" Width="250px" ReadOnly="True"></asp:TextBox>--%>
          <%-- <asp:CheckBox ID="chkPropRateMatch" Text="Proposal rates match with email confirmation / work order" runat="server"/> 
                &nbsp;&nbsp;--%>
            <asp:LinkButton ID="lnkViewProposal" runat="server" CssClass="LnkOver" Style="text-decoration: underline;"
                Font-Bold="True" OnClick="lnkViewProposal_Click">View Proposal</asp:LinkButton>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            Total Quantity
        </td>
        <td>
            :
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtTotalQty" runat="server" Height="16px" Width="78px" onkeyup="checkNum(this);"
                MaxLength="3"></asp:TextBox>
            <%--<asp:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="txtTotalQty"
                MaskType="Number" Mask="999" AutoComplete="false" runat="server">
            </asp:MaskedEditExtender>--%>
        </td>
        <td>
            Subsets&nbsp;&nbsp;:
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtSubsets" runat="server" Height="16px" Width="70px" MaxLength="2"
                onkeyup="checkNum(this);" OnTextChanged="txtSubsets_TextChanged" AutoPostBack="true"></asp:TextBox>
            <%--<asp:MaskedEditExtender ID="MaskedEditExtender3" TargetControlID="txtSubsets"
                MaskType="Number" Mask="99" AutoComplete="false" runat="server">
            </asp:MaskedEditExtender>--%>
        </td>
    </tr>
    <tr>
        <td colspan="9">
            <asp:FileUpload ID="FileUploadPO" runat="server" Width="200px"/>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="lnkUploadPO" runat="server" ToolTip="Upload PO" 
                Style="text-decoration: underline;" onclick="lnkUploadPO_Click">&nbsp;Upload PO&nbsp;</asp:LinkButton>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblPOFileName" runat="server" Text=""></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="lnkDownloadFile" runat="server" Text="Download File" 
                Style="text-decoration: underline;" onclick="lnkDownloadFile_Click"></asp:LinkButton>
        </td>
    </tr>
</table>
<script type="text/javascript">
    function checkNum(x) {
        var s_len = x.value.length;
        var s_charcode = 0;
        for (var s_i = 0; s_i < s_len; s_i++) {
            s_charcode = x.value.charCodeAt(s_i);
            if (!((s_charcode >= 48 && s_charcode <= 57))) {
                x.value = '';
                x.focus();
                alert("Only Numeric Values Allowed");
                return false;
            }
        }
        return true;
    }
</script>
