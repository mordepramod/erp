<%@ Page Language="C#" AutoEventWireup="true" Inherits="DESPLWEB.WebHome" Title="Durocrete"
    EnableEventValidation="false" CodeBehind="WebHome.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%--<link href="Styles1.css" rel="stylesheet" type="text/css" />--%>
<head id="Head1" runat="server">
    <title>Durocrete</title>
    <link rel="SHORTCUT ICON" href="Images/Duro.png" type="image/x-icon" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta name="description" content="Expand, contract, animate forms with jQuery wihtout leaving the page" />
    <meta name="keywords" content="expand, form, css3, jquery, animate, width, height, adapt, unobtrusive javascript" />
    <link rel="shortcut icon" href="../favicon.ico" type="image/x-icon" />
    <link rel="stylesheet" type="text/css" href="css/style1.css" />
    <script type="text/javascript">
        Cufon.replace('h1', { textShadow: '1px 1px #fff' });
        Cufon.replace('h2', { textShadow: '1px 1px #fff' });
        Cufon.replace('h3', { textShadow: '1px 1px #000' });
        //			Cufon.replace('.back');

    </script>
    <style type='text/css'>
        .PnlCss
        {
            border-width: 1px;
            border-style: solid;
            border-color: #f0b683;
            background-color: White;
        }
    </style>
</head>
<body>
    <div class="wrapper">
        <div>
            <div id="form_wrapper" class="form_wrapper">
                <form id="Form1" class="login active" runat="server">
                <div runat="server" align="center">
                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="hlable">
                        <tr>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td align="left" rowspan="2">
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/DuroLogo.bmp" />
                            </td>
                            <td align="right">
                                &nbsp;
                            </td>
                            <td style="width: 400px">
                                &nbsp;&nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <%-- <asp:Label ID="Label1" Text="Web Home" runat="server" ForeColor="Black" Font-Bold="true"></asp:Label>--%>
                    <%-- </Left>--%>
                </div>
                <center>
                    <div id="div1" style="border: thin solid #f0b683; width: 400px;">
                        <div style="background-color: #f0b683">
                            <br />
                        </div>
                        <div>
                            &nbsp;
                            <asp:Label ID="lblClientId" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                            <asp:Label ID="lblLocation" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                            <asp:Label ID="lblConnection" runat="server" ForeColor="Red" Text="" Visible="False"></asp:Label>
                        </div>
                        <asp:Button ID="btnUpdateMobile" runat="server" Font-Bold="True" Font-Size="9pt"
                            ForeColor="Black" BackColor="White" Height="30px" OnClick="btnUpdateMobile_Click"
                            Text="Update Mobile Users" Width="195px" />
                        <div>
                        </div>
                        <br />
                        <asp:Button ID="btnBillApproval" runat="server" Font-Bold="True" Font-Size="9pt" ForeColor="Black"
                            BackColor="White" Height="30px" OnClick="btnBillApproval_Click" Text="Bill Approval"
                            Width="195px" />
                        <div>
                        </div>
                        <br />
                        <asp:Button ID="btnViewReports" runat="server" Font-Bold="True" Font-Size="9pt" ForeColor="Black"
                            BackColor="White" Height="30px" OnClick="btnViewReports_Click" Text="View Reports"
                            Width="195px" />
                        <div>
                        </div>
                        <br />
                        <asp:Button ID="btnLogout" runat="server" Font-Bold="True" Font-Size="9pt" ForeColor="Black"
                            BackColor="White" Height="30px" OnClick="btnLogout_Click" Text="LogOut" Width="195px" />
                        <div>
                            &nbsp;
                        </div>
                    </div>
                    <div id="div2" style="border: thin solid #f0b683; width: 400px;">
                        <div style="background-color: #f0b683">
                            <br />
                        </div>
                    </div>
                </center>
                </form>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(function () {

            var $form_wrapper = $('#form_wrapper'),
            //the current form is the one with class active
					$currentForm = $form_wrapper.children('form.active'),
            //the change form links
					$linkform = $form_wrapper.find('.linkform');

            //get width and height of each form and store them for later						
            $form_wrapper.children('form').each(function (i) {
                var $theForm = $(this);
                //solve the inline display none problem when using fadeIn fadeOut
                if (!$theForm.hasClass('active'))
                    $theForm.hide();
                $theForm.data({
                    width: $theForm.width(),
                    height: $theForm.height()
                });
            });

            //set width and height of wrapper (same of current form)
            setWrapperWidth();

            /*
            clicking a link (change form event) in the form
            makes the current form hide.
            The wrapper animates its width and height to the 
            width and height of the new current form.
            After the animation, the new form is shown
            */
            $linkform.bind('click', function (e) {
                var $link = $(this);
                var target = $link.attr('rel');
                $currentForm.fadeOut(400, function () {
                    //remove class active from current form
                    $currentForm.removeClass('active');
                    //new current form
                    $currentForm = $form_wrapper.children('form.' + target);
                    //animate the wrapper
                    $form_wrapper.stop()
									 .animate({
									     width: $currentForm.data('width') + 'px',
									     height: $currentForm.data('height') + 'px'
									 }, 500, function () {
									     //new form gets class active
									     $currentForm.addClass('active');
									     //show the new form
									     $currentForm.fadeIn(400);
									 });
                });
                e.preventDefault();
            });

            function setWrapperWidth() {
                $form_wrapper.css({
                    width: $currentForm.data('width') + 'px',
                    height: $currentForm.data('height') + 'px'
                });
            }

            /*
            for the demo we disabled the submit buttons
            if you submit the form, you need to check the 
            which form was submited, and give the class active 
            to the form you want to show
            */
            $form_wrapper.find('input[type="submit"]')
							 .click(function (e) {
							     e.preventDefault();
							 });
        });
    </script>
</body>
</html>
