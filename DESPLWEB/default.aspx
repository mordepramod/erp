<%@ Page Language="C#" AutoEventWireup="true" Inherits="DESPLWEB._Default" Title="Durocrete" EnableEventValidation="false"
    CodeBehind="default.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Durocrete</title>
    <link rel="SHORTCUT ICON" href="Images/Duro.png" type="image/x-icon">
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
        .PnlCss {
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
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td align="left" rowspan="2">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/DuroLogo.bmp" />
                                </td>
                                <td align="right">&nbsp;
                                </td>
                                <td style="width: 400px">&nbsp;&nbsp; &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />

                    <asp:Label ID="Label1" Text="Client Login" runat="server" ForeColor="Black" Font-Bold="True" Font-Size="Large"></asp:Label>
                    
                    </div>
                    
                    <center>

                        <div id="div1" style="border: thin solid #f0b683; margin-top:6px; width: 400px;">
                            <div style="background-color: #f0b683">
                                <br />
                            </div>
                            <div>
                                &nbsp;
                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            </div>
                            <div>
                                <label>
                                    Location
                                </label>
                                &nbsp; &nbsp; &nbsp;
                               <asp:DropDownList ID="ddlReportAuthenicate" runat="server" Width="250px">
                                   <asp:ListItem Text="--Select--" Value="--Select--">--Select--</asp:ListItem>
                                   <asp:ListItem Text="01" Value="Pune">Pune</asp:ListItem>
                                   <asp:ListItem Text="02" Value="Mumbai">Mumbai</asp:ListItem>
                                   <asp:ListItem Text="03" Value="Nashik">Nashik</asp:ListItem>
                                   <asp:ListItem>MAHA-METRO</asp:ListItem>
                               </asp:DropDownList>
                            </div>
                            <br />
                            <div>
                                <label>
                                    Login Id
                                </label>
                                &nbsp; &nbsp; &nbsp;
                                <asp:TextBox ID="userName" Width="250px" runat="server"></asp:TextBox>
                            </div>
                            <br />
                            <div>
                                <label>
                                    Password
                                </label>
                                &nbsp; &nbsp;
                              <asp:TextBox ID="txtPass" Width="250px" runat="server" TextMode="Password"></asp:TextBox>
                            </div>
                            <br />
                            <div>
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:Button ID="LoginButton" runat="server" BackColor="#f0b683" ForeColor="white" Text="Sign In"
                                class="btn right" OnClick="LoginButton_Click" />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:LinkButton ID="lnkChangePassword" runat="server" Font-Underline="true" ToolTip="Change Password"
                                OnClick="lnkChangePassword_Click">Change Password</asp:LinkButton>

                            </div>
                            <div style="height: 7px">
                                &nbsp;                            
                            </div>
                        </div>
                        <div id="div2" style="border: thin solid #f0b683; width: 400px;">
                            <div style="background-color: #f0b683">
                                <br />
                                <%--<asp:LinkButton ID="lnkNewReg" runat="server" Font-Underline="true" ToolTip="New Registration"
                                OnClick="lnkNewReg_Click">New Registration</asp:LinkButton>--%>
                            </div>
                        </div>
                    </center>

                    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                    </asp:ToolkitScriptManager>
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenField2"
                        PopupControlID="PnlChangePassowrd" PopupDragHandleControlID="PopupHeader" BackgroundCssClass="ModalPopupBG">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="PnlChangePassowrd" runat="server" Visible="false" CssClass="PnlCss"
                        Width="500px" Height="280px">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <table width="100%" style="vertical-align: top">
                                    <div style="color: White; background-color: #f0b683; font-weight: bold;">
                                        <center>
                                            Change Your Password
                                        </center>
                                    </div>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="UserNameLabel0" runat="server">Login Id </asp:Label>
                                        </td>
                                        <td>&nbsp; &nbsp;
                                        <asp:TextBox ID="txtUserName" ReadOnly="true" runat="server" Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="CurrentPasswordLabel" runat="server">Current Password </asp:Label>
                                        </td>
                                        <td>&nbsp; &nbsp;
                                        <asp:TextBox ID="txtCurrentPassword" Text="" runat="server" TextMode="Password" Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="NewPasswordLabel" runat="server">New Password </asp:Label>
                                        </td>
                                        <td>&nbsp; &nbsp;
                                        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" MaxLength="50"
                                            Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="ConfirmNewPasswordLabel" runat="server">Confirm Password </asp:Label>
                                        </td>
                                        <td>&nbsp; &nbsp;
                                        <asp:TextBox ID="txtConfirmNewPassword" runat="server" MaxLength="50" TextMode="Password"
                                            Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>&nbsp; &nbsp;
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right"></td>
                                        <td style="text-align: center">
                                            <asp:LinkButton ID="lnkChangePasswordButton" runat="server" ToolTip="Save" OnClick="lnkChangePasswordButton_Click"> Save </asp:LinkButton>
                                            &nbsp;&nbsp;&nbsp; &nbsp;
                                        <asp:LinkButton ID="lnkExit" runat="server" ToolTip="Exit" OnClick="lnkExit_Click">Exit</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </form>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    <%--<script type="text/javascript">

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
            //$linkform.bind('click', function (e) {
            //    var $link = $(this);
            //    var target = $link.attr('rel');
            //    $currentForm.fadeOut(400, function () {
            //        //remove class active from current form
            //        $currentForm.removeClass('active');
            //        //new current form
            //        $currentForm = $form_wrapper.children('form.' + target);
            //        //animate the wrapper
            //        $form_wrapper.stop()
			//						 .animate({
			//						     width: $currentForm.data('width') + 'px',
			//						     height: $currentForm.data('height') + 'px'
			//						 }, 500, function () {
			//						     //new form gets class active
			//						     $currentForm.addClass('active');
			//						     //show the new form
			//						     $currentForm.fadeIn(400);
			//						 });
            //    });
            //    e.preventDefault();
            //});

            //function setWrapperWidth() {
            //    $form_wrapper.css({
            //        width: $currentForm.data('width') + 'px',
            //        height: $currentForm.data('height') + 'px'
            //    });
            //}

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
    </script>--%>

    
</body>
</html>
